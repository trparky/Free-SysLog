using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using Free_SysLog.SupportCode;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace Free_SysLog
{

    public partial class IgnoredLogsAndSearchResults
    {
        public List<MyDataGridViewRow> LogsToBeDisplayed;
        private IgnoreOrSearchWindowDisplayMode _WindowDisplayMode;
        private ColumnHeader m_SortingColumn1, m_SortingColumn2;
        private bool boolDoneLoading = false;
        public Form1 MainProgramForm;

        public bool boolLoadExternalData = false;
        public string strFileToLoad;

        private int intColumnNumber; // Define intColumnNumber at class level
        private SortOrder sortOrder = SortOrder.Ascending; // Define soSortOrder at class level
        private readonly object dataGridLockObject = new object();

        private const int intBatchSize = 25;

        private void OpenLogViewerWindow()
        {
            if (Logs.Rows.Count > 0 & Logs.SelectedCells.Count > 0)
            {
                MyDataGridViewRow selectedRow = (MyDataGridViewRow)Logs.Rows[Logs.SelectedCells[0].RowIndex];
                string strLogText = Conversions.ToString(selectedRow.Cells[SupportCode.SupportCode.ColumnIndex_LogText].Value);
                string strRawLogText = Conversions.ToString(string.IsNullOrWhiteSpace(selectedRow.RawLogData) ? selectedRow.Cells[SupportCode.SupportCode.ColumnIndex_LogText].Value : selectedRow.RawLogData.Replace("{newline}", Constants.vbCrLf, StringComparison.OrdinalIgnoreCase));

                using (var LogViewerInstance = new LogViewer() { strRawLogText = strRawLogText, strLogText = strLogText, StartPosition = FormStartPosition.CenterParent, Icon = Icon })
                {
                    LogViewerInstance.LblLogDate.Text = $"Log Date: {selectedRow.Cells[SupportCode.SupportCode.ColumnIndex_ComputedTime].Value}";
                    LogViewerInstance.LblSource.Text = $"Source IP Address: {selectedRow.Cells[SupportCode.SupportCode.ColumnIndex_IPAddress].Value}";

                    if (!string.IsNullOrEmpty(selectedRow.AlertText))
                    {
                        LogViewerInstance.lblAlertText.Text = $"Alert Text: {selectedRow.AlertText}";
                    }
                    else
                    {
                        LogViewerInstance.lblAlertText.Visible = false;
                    }

                    LogViewerInstance.ShowDialog(this);
                }
            }
        }

        private void Logs_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                // Disable user sorting
                Logs.AllowUserToOrderColumns = false;

                var column = Logs.Columns[e.ColumnIndex];

                if (sortOrder == SortOrder.Descending)
                {
                    sortOrder = SortOrder.Ascending;
                }
                else if (sortOrder == SortOrder.Ascending)
                {
                    sortOrder = SortOrder.Descending;
                }
                else
                {
                    sortOrder = SortOrder.Ascending;
                }

                ColAlerts.HeaderCell.SortGlyphDirection = SortOrder.None;
                ColIPAddress.HeaderCell.SortGlyphDirection = SortOrder.None;
                ColLog.HeaderCell.SortGlyphDirection = SortOrder.None;
                ColTime.HeaderCell.SortGlyphDirection = SortOrder.None;

                Logs.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection = sortOrder;

                SortLogsByDateObject(column.Index, sortOrder);
            }
        }

        private void SortLogsByDateObject(int columnIndex, SortOrder order)
        {
            lock (dataGridLockObject)
            {
                Logs.Invoke(new Action(() =>
                    {
                        Logs.AllowUserToOrderColumns = false;
                        Logs.Enabled = false;

                        var comparer = new DataGridViewComparer(columnIndex, order);
                        MyDataGridViewRow[] rows = Logs.Rows.Cast<DataGridViewRow>().OfType<MyDataGridViewRow>().ToArray();

                        Array.Sort(rows, (row1, row2) => comparer.Compare(row1, row2));

                        Logs.SuspendLayout();
                        Logs.Rows.Clear();
                        Logs.Rows.AddRange(rows);
                        Logs.ResumeLayout();

                        Logs.Enabled = true;
                        Logs.AllowUserToOrderColumns = true;
                    }));
            }
        }

        private void Logs_DoubleClick(object sender, EventArgs e)
        {
            OpenLogViewerWindow();
        }

        private void Ignored_Logs_and_Search_Results_ResizeBegin(object sender, EventArgs e)
        {
            Logs.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
        }

        private void Ignored_Logs_and_Search_Results_ResizeEnd(object sender, EventArgs e)
        {
            if (boolDoneLoading)
            {
                if (_WindowDisplayMode == IgnoreOrSearchWindowDisplayMode.ignored)
                {
                    My.MySettingsProperty.Settings.ignoredWindowSize = Size;
                }
                else if (_WindowDisplayMode == IgnoreOrSearchWindowDisplayMode.search)
                {
                    My.MySettingsProperty.Settings.searchWindowSize = Size;
                }
                else if (_WindowDisplayMode == IgnoreOrSearchWindowDisplayMode.viewer)
                {
                    My.MySettingsProperty.Settings.logFileViewerSize = Size;
                }
            }

            Logs.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
        }

        private void Logs_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            if (boolDoneLoading)
            {
                My.MySettingsProperty.Settings.columnFileNameSize = ColFileName.Width;
            }
        }

        private void Logs_MouseDown(object sender, MouseEventArgs e)
        {
            var hitTest = Logs.HitTest(e.X, e.Y);

            if (hitTest.Type == DataGridViewHitTestType.ColumnHeader)
            {
                Logs.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            }
        }

        private void Logs_MouseUp(object sender, MouseEventArgs e)
        {
            var hitTest = Logs.HitTest(e.X, e.Y);

            if (hitTest.Type == DataGridViewHitTestType.ColumnHeader)
            {
                Logs.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
            }
        }

        private void Ignored_Logs_and_Search_Results_Load(object sender, EventArgs e)
        {
            if (My.MySettingsProperty.Settings.font is not null)
            {
                Logs.DefaultCellStyle.Font = My.MySettingsProperty.Settings.font;
                Logs.ColumnHeadersDefaultCellStyle.Font = My.MySettingsProperty.Settings.font;
            }

            ColLog.AutoSizeMode = My.MySettingsProperty.Settings.colLogAutoFill ? DataGridViewAutoSizeColumnMode.Fill : DataGridViewAutoSizeColumnMode.NotSet;
            Logs.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;

            if (_WindowDisplayMode == IgnoreOrSearchWindowDisplayMode.ignored)
            {
                BtnClearIgnoredLogs.Visible = true;
                BtnViewMainWindow.Visible = true;
            }
            else if (_WindowDisplayMode == IgnoreOrSearchWindowDisplayMode.viewer)
            {
                BtnExport.Visible = false;
                BtnViewMainWindow.Visible = true;

                LblSearchLabel.Visible = true;
                TxtSearchTerms.Visible = true;
                ChkRegExSearch.Visible = true;
                ChkCaseInsensitiveSearch.Visible = true;
                BtnSearch.Visible = true;
            }

            if (_WindowDisplayMode == IgnoreOrSearchWindowDisplayMode.ignored)
            {
                Size = My.MySettingsProperty.Settings.ignoredWindowSize;
                Form argwindow = this;
                Location = SupportCode.SupportCode.VerifyWindowLocation(My.MySettingsProperty.Settings.ignoredWindowLocation, ref argwindow);
            }
            else if (_WindowDisplayMode == IgnoreOrSearchWindowDisplayMode.search)
            {
                Size = My.MySettingsProperty.Settings.searchWindowSize;
                Form argwindow1 = this;
                Location = SupportCode.SupportCode.VerifyWindowLocation(My.MySettingsProperty.Settings.searchWindowLocation, ref argwindow1);
            }
            else if (_WindowDisplayMode == IgnoreOrSearchWindowDisplayMode.viewer)
            {
                Size = My.MySettingsProperty.Settings.logFileViewerSize;
                Form argwindow2 = this;
                Location = SupportCode.SupportCode.VerifyWindowLocation(My.MySettingsProperty.Settings.logFileViewerLocation, ref argwindow2);
            }

            ColTime.Width = My.MySettingsProperty.Settings.columnTimeSize;
            colServerTime.Width = My.MySettingsProperty.Settings.ServerTimeWidth;
            colLogType.Width = My.MySettingsProperty.Settings.LogTypeWidth;
            ColIPAddress.Width = My.MySettingsProperty.Settings.columnIPSize;
            ColHostname.Width = My.MySettingsProperty.Settings.HostnameWidth;
            ColRemoteProcess.Width = My.MySettingsProperty.Settings.RemoteProcessHeaderSize;
            ColLog.Width = My.MySettingsProperty.Settings.columnLogSize;
            ColAlerts.Width = My.MySettingsProperty.Settings.columnAlertedSize;
            ColFileName.Width = My.MySettingsProperty.Settings.columnFileNameSize;

            var argcolumns = Logs.Columns;
            var argspecializedStringCollection = My.MySettingsProperty.Settings.logsColumnOrder;
            SupportCode.SupportCode.LoadColumnOrders(ref argcolumns, ref argspecializedStringCollection);
            My.MySettingsProperty.Settings.logsColumnOrder = argspecializedStringCollection;

            ColHostname.Visible = My.MySettingsProperty.Settings.boolShowHostnameColumn;
            colServerTime.Visible = My.MySettingsProperty.Settings.boolShowServerTimeColumn;
            colLogType.Visible = My.MySettingsProperty.Settings.boolShowLogTypeColumn;

            ColTime.HeaderCell.SortGlyphDirection = SortOrder.Ascending;

            var flags = BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty;
            var propInfo = typeof(DataGridView).GetProperty("DoubleBuffered", flags);
            propInfo?.SetValue(Logs, true, null);

            Logs.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle() { BackColor = My.MySettingsProperty.Settings.searchColor };
            Logs.DefaultCellStyle = new DataGridViewCellStyle() { WrapMode = DataGridViewTriState.True };
            ColLog.DefaultCellStyle = new DataGridViewCellStyle() { WrapMode = DataGridViewTriState.True };

            if (_WindowDisplayMode != IgnoreOrSearchWindowDisplayMode.viewer)
            {
                LogsToBeDisplayed.Sort((x, y) => x.DateObject.CompareTo(y.DateObject));

                Logs.SuspendLayout();

                Task.Run(() =>
                    {
                        for (int index = 0, loopTo = LogsToBeDisplayed.Count - 1; index <= loopTo; index += intBatchSize)
                        {
                            MyDataGridViewRow[] batch = LogsToBeDisplayed.Skip(index).Take(intBatchSize).ToArray();
                            Logs.Invoke(new Action(() => Logs.Rows.AddRange(batch))); // Invoke needed for UI updates
                        }

                        Logs.Invoke(new Action(() => Logs.ResumeLayout()));
                    });

                if (_WindowDisplayMode == IgnoreOrSearchWindowDisplayMode.ignored)
                {
                    LblCount.Text = $"Number of ignored logs: {LogsToBeDisplayed.Count:N0}";
                }
                else
                {
                    LblCount.Text = $"Number of search results: {LogsToBeDisplayed.Count:N0}";
                }
            }

            if (_WindowDisplayMode == IgnoreOrSearchWindowDisplayMode.viewer && boolLoadExternalData && !string.IsNullOrEmpty(strFileToLoad))
            {
                System.Threading.ThreadPool.QueueUserWorkItem((a) =>
                    {
                        LoadData(strFileToLoad);
                        LblCount.Text = $"Number of logs: {Logs.Rows.Count:N0}";
                        boolDoneLoading = true;
                        SortLogsByDateObject(0, SortOrder.Ascending);
                    });
            }

            boolDoneLoading = true;
        }

        public void AddIgnoredDatagrid(MyDataGridViewRow ItemToAdd, bool BoolAutoScroll)
        {
            Invoke(new Action(() =>
                {
                    Logs.Rows.Add(ItemToAdd);
                    if (BoolAutoScroll)
                        Logs.FirstDisplayedScrollingRowIndex = Logs.Rows.Count - 1;
                    LblCount.Text = $"Number of ignored logs: {LogsToBeDisplayed.Count:N0}";
                }));
        }

        protected override void WndProc(ref Message m)
        {
            const int WM_EXITSIZEMOVE = 0x232;

            base.WndProc(ref m);

            if (m.Msg == WM_EXITSIZEMOVE && boolDoneLoading)
            {
                if (_WindowDisplayMode == IgnoreOrSearchWindowDisplayMode.ignored)
                {
                    My.MySettingsProperty.Settings.ignoredWindowLocation = Location;
                }
                else if (_WindowDisplayMode == IgnoreOrSearchWindowDisplayMode.search)
                {
                    My.MySettingsProperty.Settings.searchWindowLocation = Location;
                }
                else if (_WindowDisplayMode == IgnoreOrSearchWindowDisplayMode.viewer)
                {
                    My.MySettingsProperty.Settings.logFileViewerLocation = Location;
                }
            }
        }

        private void BtnClearIgnoredLogs_Click(object sender, EventArgs e)
        {
            if (parentForm is Form1 && Interaction.MsgBox("Are you sure you want to clear the ignored logs stored in system memory?", (MsgBoxStyle)((int)MsgBoxStyle.Question + (int)MsgBoxStyle.YesNo + (int)Constants.vbDefaultButton2), Text) == MsgBoxResult.Yes)
            {
                Logs.Rows.Clear();
                LblCount.Text = "Number of ignored logs: 0";
                ((dynamic)parentForm).ClearIgnoredLogs();
            }
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            SaveFileDialog.Filter = "CSV (Comma Separated Value)|*.csv|JSON File|*.json|XML File|*.xml";
            SaveFileDialog.Title = "Export Data...";

            if (SaveFileDialog.ShowDialog() == DialogResult.OK)
            {
                var fileInfo = new FileInfo(SaveFileDialog.FileName);

                var collectionOfSavedData = new List<SavedData>();
                MyDataGridViewRow myItem;
                var csvStringBuilder = new System.Text.StringBuilder();
                SavedData savedData;
                string strLogType, strTime, strSourceIP, strHeader, strLogText, strAlerted, strHostname, strRemoteProcess, strServerTime, strFileName;

                if (fileInfo.Extension.Equals(".csv", StringComparison.OrdinalIgnoreCase))
                {
                    if (ColFileName.Visible)
                    {
                        csvStringBuilder.AppendLine("Time,Server Time,Log Type,IP Address,Hostname,Remote Process,Log Text,Alerted,File Name");
                    }
                    else
                    {
                        csvStringBuilder.AppendLine("Time,Server Time,Log Type,IP Address,Hostname,Remote Process,Log Text,Alerted");
                    }
                }

                foreach (DataGridViewRow item in Logs.Rows)
                {
                    if (!string.IsNullOrWhiteSpace(Conversions.ToString(item.Cells[SupportCode.SupportCode.ColumnIndex_ComputedTime].Value)))
                    {
                        myItem = (MyDataGridViewRow)item;

                        if (fileInfo.Extension.Equals(".csv", StringComparison.OrdinalIgnoreCase))
                        {
                            strTime = SupportCode.SupportCode.SanitizeForCSV(Conversions.ToString(myItem.Cells[SupportCode.SupportCode.ColumnIndex_ComputedTime].Value));
                            strLogType = SupportCode.SupportCode.SanitizeForCSV(Conversions.ToString(myItem.Cells[SupportCode.SupportCode.ColumnIndex_LogType].Value));
                            strSourceIP = SupportCode.SupportCode.SanitizeForCSV(Conversions.ToString(myItem.Cells[SupportCode.SupportCode.ColumnIndex_IPAddress].Value));
                            strHeader = SupportCode.SupportCode.SanitizeForCSV(Conversions.ToString(myItem.Cells[SupportCode.SupportCode.ColumnIndex_RemoteProcess].Value));
                            strLogText = SupportCode.SupportCode.SanitizeForCSV(Conversions.ToString(myItem.Cells[SupportCode.SupportCode.ColumnIndex_LogText].Value));
                            strHostname = SupportCode.SupportCode.SanitizeForCSV(Conversions.ToString(myItem.Cells[SupportCode.SupportCode.ColumnIndex_Hostname].Value));
                            strRemoteProcess = SupportCode.SupportCode.SanitizeForCSV(Conversions.ToString(myItem.Cells[SupportCode.SupportCode.ColumnIndex_RemoteProcess].Value));
                            strServerTime = SupportCode.SupportCode.SanitizeForCSV(Conversions.ToString(myItem.Cells[SupportCode.SupportCode.ColumnIndex_ServerTime].Value));
                            strAlerted = myItem.BoolAlerted ? "Yes" : "No";

                            if (ColFileName.Visible)
                            {
                                strFileName = SupportCode.SupportCode.SanitizeForCSV(Conversions.ToString(myItem.Cells[SupportCode.SupportCode.ColumnIndex_FileName].Value));
                                csvStringBuilder.AppendLine($"{strTime},{strServerTime},{strLogType},{strSourceIP},{strHostname},{strRemoteProcess},{strLogText},{strAlerted},{strFileName}");
                            }
                            else
                            {
                                csvStringBuilder.AppendLine($"{strTime},{strServerTime},{strLogType},{strSourceIP},{strHostname},{strRemoteProcess},{strLogText},{strAlerted}");
                            }
                        }
                        else
                        {
                            savedData = new SavedData()
                            {
                                time = Conversions.ToString(myItem.Cells[SupportCode.SupportCode.ColumnIndex_ComputedTime].Value),
                                ServerDate = Conversions.ToDate(myItem.Cells[SupportCode.SupportCode.ColumnIndex_ServerTime].Value),
                                logType = Conversions.ToString(myItem.Cells[SupportCode.SupportCode.ColumnIndex_LogType].Value),
                                ip = Conversions.ToString(myItem.Cells[SupportCode.SupportCode.ColumnIndex_IPAddress].Value),
                                hostname = Conversions.ToString(myItem.Cells[SupportCode.SupportCode.ColumnIndex_Hostname].Value),
                                appName = Conversions.ToString(myItem.Cells[SupportCode.SupportCode.ColumnIndex_RemoteProcess].Value),
                                log = Conversions.ToString(myItem.Cells[SupportCode.SupportCode.ColumnIndex_LogText].Value),
                                DateObject = myItem.DateObject,
                                BoolAlerted = myItem.BoolAlerted,
                                rawLogData = myItem.RawLogData
                            };
                            if (ColFileName.Visible)
                                savedData.fileName = Conversions.ToString(myItem.Cells[SupportCode.SupportCode.ColumnIndex_FileName].Value);
                            collectionOfSavedData.Add(savedData);
                        }
                    }
                }

                using (var fileStream = new StreamWriter(SaveFileDialog.FileName))
                {
                    if (fileInfo.Extension.Equals(".xml", StringComparison.OrdinalIgnoreCase))
                    {
                        var xmlSerializerObject = new XmlSerializer(collectionOfSavedData.GetType());
                        xmlSerializerObject.Serialize(fileStream, collectionOfSavedData);
                    }
                    else if (fileInfo.Extension.Equals(".json", StringComparison.OrdinalIgnoreCase))
                    {
                        fileStream.Write(Newtonsoft.Json.JsonConvert.SerializeObject(collectionOfSavedData, Newtonsoft.Json.Formatting.Indented));
                    }
                    else if (fileInfo.Extension.Equals(".csv", StringComparison.OrdinalIgnoreCase))
                    {
                        fileStream.Write(csvStringBuilder.ToString().Trim());
                    }
                }

                if (Interaction.MsgBox($"Data exported to \"{SaveFileDialog.FileName}\" successfully.{Constants.vbCrLf}{Constants.vbCrLf}Do you want to open Windows Explorer to the location of the file?", (MsgBoxStyle)((int)MsgBoxStyle.Question + (int)MsgBoxStyle.YesNo + (int)Constants.vbDefaultButton2), Text) == MsgBoxResult.Yes)
                {
                    SupportCode.SupportCode.SelectFileInWindowsExplorer(SaveFileDialog.FileName);
                }
            }
        }

        private void BtnViewMainWindow_Click(object sender, EventArgs e)
        {
            if (parentForm is Form1)
            {
                ((dynamic)parentForm).RestoreWindow();
                BtnViewMainWindow.Enabled = false;
                BringToFront();
            }
        }

        private void LogsContextMenu_Opening(object sender, CancelEventArgs e)
        {
            if (_WindowDisplayMode == IgnoreOrSearchWindowDisplayMode.viewer && boolLoadExternalData && !string.IsNullOrEmpty(strFileToLoad))
            {
                ExportSelectedLogsToolStripMenuItem.Visible = true;
                CopyLogTextToolStripMenuItem.Visible = false;
                CreateAlertToolStripMenuItem.Visible = false;
                OpenLogFileForViewingToolStripMenuItem.Visible = false;
            }
            else
            {
                ExportSelectedLogsToolStripMenuItem.Visible = false;
                CopyLogTextToolStripMenuItem.Visible = true;
                CreateAlertToolStripMenuItem.Visible = true;
                OpenLogFileForViewingToolStripMenuItem.Visible = true;
            }
        }

        private void CopyLogTextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MyDataGridViewRow selectedRow = (MyDataGridViewRow)Logs.Rows[Logs.SelectedCells[0].RowIndex];
            SupportCode.SupportCode.CopyTextToWindowsClipboard(Conversions.ToString(selectedRow.Cells[SupportCode.SupportCode.ColumnIndex_LogText].Value), Text);
        }

        private MyDataGridViewRow MakeDataGridRow(DateTime dateObject, string strLog, ref DataGridView dataGrid)
        {
            using (var MyDataGridViewRow = new MyDataGridViewRow())
            {
                MyDataGridViewRow.CreateCells(dataGrid);
                MyDataGridViewRow.Cells[SupportCode.SupportCode.ColumnIndex_ComputedTime].Value = DateTime.Now.ToString();
                MyDataGridViewRow.Cells[SupportCode.SupportCode.ColumnIndex_LogText].Value = strLog;
                MyDataGridViewRow.DefaultCellStyle.Padding = new Padding(0, 2, 0, 2);

                return MyDataGridViewRow;
            }
        }

        private void LoadData(string strFileName)
        {
            var stopWatch = Stopwatch.StartNew();

            Logs.Invoke(new Action(() =>
                {
                    MyDataGridViewRow localMakeDataGridRow() { var argdataGrid = Logs; var ret = MakeDataGridRow(DateTime.Now, "Loading data and populating data grid... Please Wait.", ref argdataGrid); Logs = argdataGrid; return ret; }

                    Logs.Rows.Add(localMakeDataGridRow());
                }));

            var collectionOfSavedData = new List<SavedData>();

            try
            {
                using (var fileStream = new StreamReader(strFileName))
                {
                    collectionOfSavedData = Newtonsoft.Json.JsonConvert.DeserializeObject<List<SavedData>>(fileStream.ReadToEnd().Trim(), SupportCode.SupportCode.JSONDecoderSettingsForSettingsFiles);
                }

                if (collectionOfSavedData.Any())
                {
                    var listOfLogEntries = new List<MyDataGridViewRow>();

                    foreach (SavedData item in collectionOfSavedData)
                    {
                        MyDataGridViewRow localMakeDataGridRow() { var argdataGrid1 = Logs; var ret = item.MakeDataGridRow(ref argdataGrid1); Logs = argdataGrid1; return ret; }

                        listOfLogEntries.Add(localMakeDataGridRow());
                    }

                    Logs.Invoke(new Action(() =>
                        {
                            listOfLogEntries.Sort((x, y) => x.DateObject.CompareTo(y.DateObject));

                            Logs.SuspendLayout();
                            Logs.Rows.Clear();

                            Task.Run(() =>
                {
                                for (int index = 0, loopTo = listOfLogEntries.Count - 1; index <= loopTo; index += intBatchSize)
                                {
                                    MyDataGridViewRow[] batch = listOfLogEntries.Skip(index).Take(intBatchSize).ToArray();
                                    Logs.Invoke(new Action(() => Logs.Rows.AddRange(batch))); // Invoke needed for UI updates
                                }

                                Logs.Invoke(new Action(() => Logs.ResumeLayout()));
                            });

                            LogsLoadedInLabel.Visible = true;
                            LogsLoadedInLabel.Text = $"Logs Loaded In: {SupportCode.SupportCode.MyRoundingFunction(stopWatch.Elapsed.TotalMilliseconds / 1000d, 2)} seconds";
                        }));
                }
            }
            catch (Newtonsoft.Json.JsonSerializationException ex)
            {
                SyslogParser.SyslogParser.AddToLogList(null, System.Net.IPAddress.Loopback.ToString(), $"Exception Type: {ex.GetType()}{Constants.vbCrLf}Exception Message: {ex.Message}{Constants.vbCrLf}{Constants.vbCrLf}Exception Stack Trace{Constants.vbCrLf}{ex.StackTrace}");
                Interaction.MsgBox("There was an error decoding JSON data.", MsgBoxStyle.Critical, Text);
            }
            catch (Newtonsoft.Json.JsonReaderException ex)
            {
                SyslogParser.SyslogParser.AddToLogList(null, System.Net.IPAddress.Loopback.ToString(), $"Exception Type: {ex.GetType()}{Constants.vbCrLf}Exception Message: {ex.Message}{Constants.vbCrLf}{Constants.vbCrLf}Exception Stack Trace{Constants.vbCrLf}{ex.StackTrace}");
                Interaction.MsgBox("There was an error decoding JSON data.", MsgBoxStyle.Critical, Text);
            }
        }

        private void CreateAlertToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var Alerts = new Alerts() { StartPosition = FormStartPosition.CenterParent, Icon = Icon })
            {
                Alerts.TxtLogText.Text = Conversions.ToString(Logs.SelectedRows[0].Cells[SupportCode.SupportCode.ColumnIndex_LogText].Value);
                Alerts.ShowDialog(this);
            }
        }

        private void Ignored_Logs_and_Search_Results_Closing(object sender, CancelEventArgs e)
        {
            SupportCode.SupportCode.IgnoredLogsAndSearchResultsInstance = null;
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtSearchTerms.Text))
            {
                Interaction.MsgBox("You must provide something to search for.", MsgBoxStyle.Critical, Text);
                return;
            }

            string strLogText;
            var listOfSearchResults = new List<MyDataGridViewRow>();
            Regex regexCompiledObject = null;
            MyDataGridViewRow MyDataGridRowItem;

            BtnSearch.Enabled = false;

            var worker = new BackgroundWorker();





            worker.DoWork += (a, b) => { try { RegexOptions regExOptions = (RegexOptions)(ChkCaseInsensitiveSearch.Checked ? (int)RegexOptions.Compiled + (int)RegexOptions.IgnoreCase : (int)RegexOptions.Compiled); if (ChkRegExSearch.Checked) { regexCompiledObject = new Regex(TxtSearchTerms.Text, regExOptions); } else { regexCompiledObject = new Regex(Regex.Escape(TxtSearchTerms.Text), regExOptions); } lock (dataGridLockObject) { foreach (DataGridViewRow item in Logs.Rows) { MyDataGridRowItem = item as MyDataGridViewRow; if (MyDataGridRowItem is not null) { strLogText = Conversions.ToString(MyDataGridRowItem.Cells[SupportCode.SupportCode.ColumnIndex_RemoteProcess].Value); if (!string.IsNullOrWhiteSpace(strLogText) && regexCompiledObject.IsMatch(strLogText)) { listOfSearchResults.Add((MyDataGridViewRow)MyDataGridRowItem.Clone()); } } } } } catch (ArgumentException ex) { Interaction.MsgBox("Malformed RegEx pattern detected, search aborted.", MsgBoxStyle.Critical, Text); } };

            worker.RunWorkerCompleted += (a, b) =>
                {
                    if (listOfSearchResults.Any())
                    {
                        var searchResultsWindow = new IgnoredLogsAndSearchResults(this, IgnoreOrSearchWindowDisplayMode.search) { MainProgramForm = MainProgramForm, Icon = Icon, LogsToBeDisplayed = listOfSearchResults, Text = "Search Results" };
                        searchResultsWindow.ChkColLogsAutoFill.Checked = My.MySettingsProperty.Settings.colLogAutoFill;
                        searchResultsWindow.ShowDialog(this);
                    }
                    else
                    {
                        Interaction.MsgBox("Search terms not found.", MsgBoxStyle.Information, Text);
                    }

                    Invoke(new Action(() => BtnSearch.Enabled = true));
                };

            worker.RunWorkerAsync();
        }

        private void OpenLogFileForViewingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var logFileViewer = new IgnoredLogsAndSearchResults(this, IgnoreOrSearchWindowDisplayMode.viewer) { MainProgramForm = MainProgramForm, Icon = Icon, Text = "Log File Viewer", strFileToLoad = Path.Combine(SupportCode.SupportCode.strPathToDataBackupFolder, Conversions.ToString(Logs.SelectedRows[0].Cells[SupportCode.SupportCode.ColumnIndex_FileName].Value)), boolLoadExternalData = true };
            logFileViewer.ChkColLogsAutoFill.Checked = My.MySettingsProperty.Settings.colLogAutoFill;
            logFileViewer.Show(this);
        }

        private void TxtSearchTerms_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                BtnSearch.PerformClick();
            }
        }

        private void ExportSelectedLogsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataHandling.DataHandling.ExportSelectedLogs(Logs.SelectedRows);
        }

        private void ChkColLogsAutoFill_Click(object sender, EventArgs e)
        {
            My.MySettingsProperty.Settings.colLogAutoFill = ChkColLogsAutoFill.Checked;
            ColLog.AutoSizeMode = My.MySettingsProperty.Settings.colLogAutoFill ? DataGridViewAutoSizeColumnMode.Fill : DataGridViewAutoSizeColumnMode.NotSet;

            if (MainProgramForm is not null)
            {
                MainProgramForm.ColLogsAutoFill.Checked = ChkColLogsAutoFill.Checked;
                MainProgramForm.ColLog.AutoSizeMode = My.MySettingsProperty.Settings.colLogAutoFill ? DataGridViewAutoSizeColumnMode.Fill : DataGridViewAutoSizeColumnMode.NotSet;
            }
        }
    }
}