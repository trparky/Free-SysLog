using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Free_SysLog.SupportCode;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace Free_SysLog
{

    public partial class ViewLogBackups
    {
        public Form1 MyParentForm;
        public List<SavedData> currentLogs;
        private bool boolDoneLoading = false;
        public int intSortColumnIndex = 0; // Define intColumnNumber at class level
        public SortOrder sortOrder = SortOrder.Ascending; // Define soSortOrder at class level

        public ViewLogBackups()
        {
            InitializeComponent();
        }

        private void Logs_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                // Disable user sorting
                FileList.AllowUserToOrderColumns = false;

                var column = FileList.Columns[e.ColumnIndex];
                intSortColumnIndex = e.ColumnIndex;

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

                colHidden.HeaderCell.SortGlyphDirection = SortOrder.None;
                ColFileName.HeaderCell.SortGlyphDirection = SortOrder.None;
                ColFileSize.HeaderCell.SortGlyphDirection = SortOrder.None;

                FileList.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection = sortOrder;

                SortLogsByDateObject(column.Index, sortOrder);
            }
        }

        private void SortLogsByDateObject(int columnIndex, SortOrder order)
        {
            SortLogsByDateObjectNoLocking(columnIndex, order);
        }

        public void SortLogsByDateObjectNoLocking(int columnIndex, SortOrder order)
        {
            FileList.AllowUserToOrderColumns = false;
            FileList.Enabled = false;

            var comparer = new MyDataGridViewFileRowComparer(columnIndex, order);
            MyDataGridViewFileRow[] rows = FileList.Rows.Cast<DataGridViewRow>().OfType<MyDataGridViewFileRow>().ToArray();

            Array.Sort(rows, comparer.Compare);

            FileList.SuspendLayout();
            FileList.Rows.Clear();
            FileList.Rows.AddRange(rows);
            FileList.ResumeLayout();

            FileList.Enabled = true;
            FileList.AllowUserToOrderColumns = true;
        }

        private int GetEntryCount(string strFileName)
        {
            try
            {
                using (var fileStream = new StreamReader(strFileName))
                {
                    return Newtonsoft.Json.JsonConvert.DeserializeObject<List<SavedData>>(fileStream.ReadToEnd().Trim(), SupportCode.SupportCode.JSONDecoderSettingsForLogFiles).Count;
                }
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        private void LoadFileList()
        {
            FileInfo[] filesInDirectory;

            if (ChkShowHidden.Checked)
            {
                filesInDirectory = new DirectoryInfo(SupportCode.SupportCode.strPathToDataBackupFolder).GetFiles();
            }
            else
            {
                filesInDirectory = new DirectoryInfo(SupportCode.SupportCode.strPathToDataBackupFolder).GetFiles().Where((fileinfo) => (fileinfo.Attributes & FileAttributes.Hidden) != FileAttributes.Hidden).ToArray();
            }

            var listOfDataGridViewRows = new List<DataGridViewRow>();
            int intHiddenTotalLogCount = default, intFileCount = default, intHiddenFileCount = default;
            long longTotalLogCount = default, longUsedDiskSpace = default;

            Parallel.ForEach(filesInDirectory, (@file) =>
                {
                    bool boolIsHidden = (@file.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden;
                    int intCount = GetEntryCount(@file.FullName);

                    if (intCount != -1)
                    {
                        // Accumulate counts and totals
                        Interlocked.Add(ref longUsedDiskSpace, @file.Length);

                        if (boolIsHidden)
                        {
                            Interlocked.Increment(ref intHiddenFileCount);
                            Interlocked.Add(ref intHiddenTotalLogCount, intCount);
                        }
                        else
                        {
                            Interlocked.Increment(ref intFileCount);
                            Interlocked.Add(ref longTotalLogCount, intCount);
                        }

                        var row = new MyDataGridViewFileRow();

                        row.CreateCells(FileList);
                        row.fileDate = @file.CreationTime;
                        row.fileSize = @file.Length;
                        row.entryCount = intCount;
                        row.Cells[0].Value = @file.Name;
                        row.Cells[0].Style.Alignment = DataGridViewContentAlignment.MiddleLeft;

                        row.Cells[1].Value = $"{@file.CreationTime.ToLongDateString()} {@file.CreationTime.ToLongTimeString()}";
                        row.Cells[2].Style.Alignment = DataGridViewContentAlignment.MiddleLeft;

                        row.Cells[2].Value = SupportCode.SupportCode.FileSizeToHumanSize(@file.Length);
                        row.Cells[2].Style.Alignment = DataGridViewContentAlignment.MiddleLeft;

                        row.Cells[3].Value = $"{intCount:N0}";
                        row.Cells[3].Style.Alignment = DataGridViewContentAlignment.MiddleLeft;

                        row.Cells[4].Value = boolIsHidden ? "Yes" : "No";
                        row.Cells[4].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

                        row.DefaultCellStyle.Padding = new Padding(0, 2, 0, 2);


                        foreach (DataGridViewCell cell in row.Cells)
                        {
                            cell.Style.Font = My.MySettingsProperty.Settings.font;
                            if (boolIsHidden && ChkShowHiddenAsGray.Checked)
                                cell.Style.ForeColor = Color.Gray;
                        }

                        // Thread-safe add to list
                        lock (listOfDataGridViewRows)
                            listOfDataGridViewRows.Add(row);
                    }
                });

            Invoke(new Action(() =>
                {
                    listOfDataGridViewRows = listOfDataGridViewRows.OrderBy(row => Convert.ToDateTime(row.Cells["ColFileDate"].Value)).ToList();

                    if (My.MySettingsProperty.Settings.font is not null)
                    {
                        FileList.DefaultCellStyle.Font = My.MySettingsProperty.Settings.font;
                        FileList.ColumnHeadersDefaultCellStyle.Font = My.MySettingsProperty.Settings.font;
                    }

                    FileList.SuspendLayout();
                    FileList.Rows.Clear();
                    FileList.Rows.AddRange(listOfDataGridViewRows.ToArray());
                    FileList.ResumeLayout();

                    lblNumberOfFiles.Text = $"Number of Files: {intFileCount:N0}";
                    LblTotalDiskSpace.Text = $"Total Disk Space Used: {SupportCode.SupportCode.FileSizeToHumanSize(longUsedDiskSpace)}";
                    lblTotalNumberOfLogs.Text = $"Total Number of Logs: {longTotalLogCount:N0}";

                    lblNumberOfHiddenFiles.Visible = intHiddenFileCount > 0;
                    lblTotalNumberOfHiddenLogs.Visible = intHiddenFileCount > 0;
                    lblNumberOfHiddenFiles.Text = $"Number of Hidden Files: {intHiddenFileCount:N0}";
                    lblTotalNumberOfHiddenLogs.Text = $"Number of Hidden Logs: {intHiddenTotalLogCount:N0}";
                }));
        }

        private void ViewLogBackups_Load(object sender, EventArgs e)
        {
            ChkIgnoreSearchResultsLimits.Checked = My.MySettingsProperty.Settings.IgnoreSearchResultLimits;
            var flags = System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.SetProperty;
            var propInfo = typeof(DataGridView).GetProperty("DoubleBuffered", flags);
            propInfo?.SetValue(FileList, true, null);

            FileList.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle() { BackColor = My.MySettingsProperty.Settings.searchColor };
            FileList.DefaultCellStyle = new DataGridViewCellStyle() { WrapMode = DataGridViewTriState.True };
            FileList.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;

            ColFileDate.Width = My.MySettingsProperty.Settings.ColViewLogBackupsFileDate;
            ColFileName.Width = My.MySettingsProperty.Settings.ColViewLogBackupsFileName;
            ColFileSize.Width = My.MySettingsProperty.Settings.ColViewLogBackupsFileSize;
            colEntryCount.Width = My.MySettingsProperty.Settings.viewLogBackupsEntryCountColumnSize;
            colHidden.Width = My.MySettingsProperty.Settings.viewLogBackupsHiddenColumnSize;

            var argcolumns = FileList.Columns;
            var argspecializedStringCollection = My.MySettingsProperty.Settings.fileListColumnOrder;
            SupportCode.SupportCode.LoadColumnOrders(ref argcolumns, ref argspecializedStringCollection);
            My.MySettingsProperty.Settings.fileListColumnOrder = argspecializedStringCollection;

            ColFileDate.HeaderCell.SortGlyphDirection = SortOrder.Ascending;

            colHidden.Visible = My.MySettingsProperty.Settings.boolShowHiddenFilesOnViewLogBackyupsWindow;
            ChkShowHidden.Checked = My.MySettingsProperty.Settings.boolShowHiddenFilesOnViewLogBackyupsWindow;
            ChkShowHiddenAsGray.Checked = My.MySettingsProperty.Settings.boolShowHiddenAsGray;
            ChkShowHiddenAsGray.Enabled = ChkShowHidden.Checked;
            ChkLogFileDeletions.Checked = My.MySettingsProperty.Settings.LogFileDeletions;
            Size = My.MySettingsProperty.Settings.ViewLogBackupsSize;
            SupportCode.SupportCode.CenterFormOverParent(MyParentForm, this);
            ThreadPool.QueueUserWorkItem((_) => LoadFileList());
            boolDoneLoading = true;
        }

        private void FileList_DoubleClick(object sender, EventArgs e)
        {
            BtnView.PerformClick();
        }

        private void BtnView_Click(object sender, EventArgs e)
        {
            if (FileList.SelectedRows.Count > 0)
            {
                string fileName = Path.Combine(SupportCode.SupportCode.strPathToDataBackupFolder, Conversions.ToString(FileList.SelectedRows[0].Cells[0].Value));

                if (File.Exists(fileName))
                {
                    var searchResultsWindow = new IgnoredLogsAndSearchResults(this, IgnoreOrSearchWindowDisplayMode.viewer) { MainProgramForm = MyParentForm, Icon = Icon, Text = "Log Viewer", strFileToLoad = fileName, boolLoadExternalData = true };
                    searchResultsWindow.ChkColLogsAutoFill.Checked = My.MySettingsProperty.Settings.colLogAutoFill;
                    searchResultsWindow.ShowDialog(this);
                }
            }
        }

        private void FileList_Click(object sender, EventArgs e)
        {
            if (FileList.SelectedRows.Count > 0)
            {
                BtnDelete.Enabled = true;
                BtnView.Enabled = FileList.SelectedRows.Count <= 1;
            }
            else
            {
                BtnDelete.Enabled = false;
                BtnView.Enabled = false;
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (FileList.SelectedRows.Count > 0)
            {
                if (FileList.SelectedRows.Count == 1)
                {
                    string fileName = Path.Combine(SupportCode.SupportCode.strPathToDataBackupFolder, Conversions.ToString(FileList.SelectedRows[0].Cells[0].Value));

                    if (Interaction.MsgBox($"Are you sure you want to delete the file named \"{FileList.SelectedRows[0].Cells[0].Value}\"?", (MsgBoxStyle)((int)MsgBoxStyle.Question + (int)MsgBoxStyle.YesNo + (int)MsgBoxStyle.DefaultButton2), Text) == MsgBoxResult.Yes)
                    {
                        File.Delete(fileName);

                        if (ChkLogFileDeletions.Checked)
                            SyslogParser.SyslogParser.AddToLogList(null, System.Net.IPAddress.Loopback.ToString(), $"The user deleted \"{FileList.SelectedRows[0].Cells[0].Value}\" from the log backups folder.");

                        ThreadPool.QueueUserWorkItem((_) => LoadFileList());
                    }
                }
                else
                {
                    string msgBoxText = "Are you sure you want to delete the following files?" + Constants.vbCrLf + Constants.vbCrLf;
                    var listOfFilesThatAreToBeDeleted = new List<string>();

                    foreach (MyDataGridViewFileRow item in FileList.SelectedRows)
                        listOfFilesThatAreToBeDeleted.Add(Conversions.ToString(item.Cells[0].Value));

                    string listOfFilesThatAreToBeDeletedInHumanReadableFormat = SupportCode.SupportCode.ConvertListOfStringsToString(listOfFilesThatAreToBeDeleted, true);

                    msgBoxText += listOfFilesThatAreToBeDeletedInHumanReadableFormat;

                    if (Interaction.MsgBox(msgBoxText.Trim(), (MsgBoxStyle)((int)MsgBoxStyle.Question + (int)MsgBoxStyle.YesNo + (int)MsgBoxStyle.DefaultButton2), Text) == MsgBoxResult.Yes)
                    {
                        string strDeletedFilesLog = $"The user deleted the following {FileList.SelectedRows.Count} files from the log backups folder...";

                        foreach (MyDataGridViewFileRow item in FileList.SelectedRows)
                            File.Delete(Path.Combine(SupportCode.SupportCode.strPathToDataBackupFolder, Conversions.ToString(item.Cells[0].Value)));

                        strDeletedFilesLog += Constants.vbCrLf + listOfFilesThatAreToBeDeletedInHumanReadableFormat;

                        if (ChkLogFileDeletions.Checked)
                            SyslogParser.SyslogParser.AddToLogList(null, System.Net.IPAddress.Loopback.ToString(), strDeletedFilesLog.ToString());

                        ThreadPool.QueueUserWorkItem((_) => LoadFileList());
                    }
                }
            }
        }

        private void ViewLogBackups_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
            {
                ThreadPool.QueueUserWorkItem((_) => LoadFileList());
            }
            else if (e.KeyCode == Keys.Delete)
            {
                BtnDelete.PerformClick();
            }
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            ThreadPool.QueueUserWorkItem((_) => LoadFileList());
        }

        private void ViewLogBackups_ResizeEnd(object sender, EventArgs e)
        {
            if (boolDoneLoading)
                My.MySettingsProperty.Settings.ViewLogBackupsSize = Size;
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtSearchTerms.Text))
            {
                Interaction.MsgBox("You must provide something to search for.", MsgBoxStyle.Critical, Text);
                return;
            }

            bool boolShowSearchResults = true;
            var listOfSearchResults = new HashSet<MyDataGridViewRow>();
            var listOfSearchResults2 = new List<MyDataGridViewRow>();
            Regex regexCompiledObject = null;
            var searchResultsWindow = new IgnoredLogsAndSearchResults(this, IgnoreOrSearchWindowDisplayMode.search) { MainProgramForm = MyParentForm, Icon = Icon, Text = "Search Results" };
            searchResultsWindow.ChkColLogsAutoFill.Checked = My.MySettingsProperty.Settings.colLogAutoFill;

            if (My.MySettingsProperty.Settings.font is not null)
                searchResultsWindow.Logs.DefaultCellStyle.Font = My.MySettingsProperty.Settings.font;

            BtnSearch.Enabled = false;

            var worker = new BackgroundWorker();












            worker.DoWork += (a, b) => { try { RegexOptions regExOptions = (RegexOptions)(ChkCaseInsensitiveSearch.Checked ? (int)RegexOptions.Compiled + (int)RegexOptions.IgnoreCase : (int)RegexOptions.Compiled); if (ChkRegExSearch.Checked) { regexCompiledObject = new Regex(TxtSearchTerms.Text, regExOptions); } else { regexCompiledObject = new Regex(Regex.Escape(TxtSearchTerms.Text), regExOptions); } FileInfo[] filesInDirectory; if (ChkShowHidden.Checked) { filesInDirectory = new DirectoryInfo(SupportCode.SupportCode.strPathToDataBackupFolder).GetFiles(); } else { filesInDirectory = new DirectoryInfo(SupportCode.SupportCode.strPathToDataBackupFolder).GetFiles().Where((fileinfo) => (fileinfo.Attributes & FileAttributes.Hidden) != FileAttributes.Hidden).ToArray(); } List<SavedData> dataFromFile; MyDataGridViewRow myDataGridRow; Parallel.ForEach(filesInDirectory, (@file) => { using (var fileStream = new StreamReader(@file.FullName)) { dataFromFile = Newtonsoft.Json.JsonConvert.DeserializeObject<List<SavedData>>(fileStream.ReadToEnd().Trim(), SupportCode.SupportCode.JSONDecoderSettingsForLogFiles); foreach (SavedData item in dataFromFile) { if (regexCompiledObject.IsMatch(item.log)) { var argdataGrid = searchResultsWindow.Logs; myDataGridRow = item.MakeDataGridRow(ref argdataGrid); searchResultsWindow.Logs = argdataGrid; myDataGridRow.Cells[SupportCode.SupportCode.ColumnIndex_FileName].Value = @file.Name; myDataGridRow.DefaultCellStyle.Padding = new Padding(0, 2, 0, 2); lock (listOfSearchResults) listOfSearchResults.Add(myDataGridRow); } } } }); foreach (MyDataGridViewRow item in listOfSearchResults) { if (My.MySettingsProperty.Settings.font is not null) item.Cells[SupportCode.SupportCode.ColumnIndex_FileName].Style.Font = My.MySettingsProperty.Settings.font; } foreach (SavedData item in currentLogs) { if (regexCompiledObject.IsMatch(item.log)) { var argdataGrid = searchResultsWindow.Logs; myDataGridRow = item.MakeDataGridRow(ref argdataGrid); searchResultsWindow.Logs = argdataGrid; myDataGridRow.Cells[SupportCode.SupportCode.ColumnIndex_FileName].Value = "Current Log Data"; listOfSearchResults.Add(myDataGridRow); myDataGridRow = null; } } if (listOfSearchResults.Count > 4000) { if (!My.MySettingsProperty.Settings.IgnoreSearchResultLimits) { Interaction.MsgBox($"Your search results contains more than four thousand results. It's highly recommended that you narrow your search terms.{Constants.vbCrLf}{Constants.vbCrLf}Search aborted.", MsgBoxStyle.Information, Text); boolShowSearchResults = false; return; } else if (Interaction.MsgBox("There are more than 4000 search results, are you sure you want to display them?", (MsgBoxStyle)((int)MsgBoxStyle.Question + (int)MsgBoxStyle.YesNo), Text) == MsgBoxResult.No) { boolShowSearchResults = false; return; } } listOfSearchResults2 = listOfSearchResults.Distinct().ToList().OrderBy(row => row.Cells[SupportCode.SupportCode.ColumnIndex_LogText].Value.ToString()).ThenBy(row => row.Cells[SupportCode.SupportCode.ColumnIndex_ComputedTime].Value.ToString()).ToList(); } catch (ArgumentException ex) { Interaction.MsgBox("Malformed RegEx pattern detected, search aborted.", MsgBoxStyle.Critical, Text); } }; // Ensure thread safety

            worker.RunWorkerCompleted += (a, b) =>
                {
                    if (boolShowSearchResults)
                    {
                        if (listOfSearchResults2.Count > 0)
                        {
                            searchResultsWindow.LogsToBeDisplayed = listOfSearchResults2;
                            searchResultsWindow.ColFileName.Visible = true;
                            searchResultsWindow.OpenLogFileForViewingToolStripMenuItem.Visible = true;
                            searchResultsWindow.ShowDialog(this);
                        }
                        else
                        {
                            Interaction.MsgBox("Search terms not found.", MsgBoxStyle.Information, Text);
                        }
                    }

                    Invoke(new Action(() => BtnSearch.Enabled = true));
                };

            worker.RunWorkerAsync();
        }

        private void TxtSearchTerms_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                BtnSearch.PerformClick();
            }
        }

        private void ContextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            if (FileList.SelectedRows.Count > 0)
            {
                DeleteToolStripMenuItem.Enabled = true;
                ViewToolStripMenuItem.Enabled = FileList.SelectedRows.Count <= 1;

                string fileName = Path.Combine(SupportCode.SupportCode.strPathToDataBackupFolder, Conversions.ToString(FileList.SelectedRows[0].Cells[0].Value));

                if ((new FileInfo(fileName).Attributes & FileAttributes.Hidden) == FileAttributes.Hidden)
                {
                    UnhideToolStripMenuItem.Visible = true;
                    HideToolStripMenuItem.Visible = false;
                }
                else
                {
                    UnhideToolStripMenuItem.Visible = false;
                    HideToolStripMenuItem.Visible = true;
                }
            }
            else
            {
                DeleteToolStripMenuItem.Enabled = false;
                ViewToolStripMenuItem.Enabled = false;
            }
        }

        private void DeleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BtnDelete.PerformClick();
        }

        private void ViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BtnView.PerformClick();
        }

        private void ChkShowHidden_Click(object sender, EventArgs e)
        {
            My.MySettingsProperty.Settings.boolShowHiddenFilesOnViewLogBackyupsWindow = ChkShowHidden.Checked;
            ChkShowHiddenAsGray.Enabled = ChkShowHidden.Checked;
            colHidden.Visible = ChkShowHidden.Checked;
            BtnRefresh.PerformClick();
        }

        private void UnhideToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string fileName;

            if (FileList.SelectedRows.Count > 1)
            {
                foreach (DataGridViewRow item in FileList.SelectedRows)
                {
                    fileName = Path.Combine(SupportCode.SupportCode.strPathToDataBackupFolder, Conversions.ToString(item.Cells[0].Value));
                    UnhideFile(fileName);
                }
            }
            else
            {
                fileName = Path.Combine(SupportCode.SupportCode.strPathToDataBackupFolder, Conversions.ToString(FileList.SelectedRows[0].Cells[0].Value));
                UnhideFile(fileName);
            }

            BtnRefresh.PerformClick();
        }

        private void HideToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string fileName;

            if (FileList.SelectedRows.Count > 1)
            {
                foreach (DataGridViewRow item in FileList.SelectedRows)
                {
                    fileName = Path.Combine(SupportCode.SupportCode.strPathToDataBackupFolder, Conversions.ToString(item.Cells[0].Value));
                    HideFile(fileName);
                }
            }
            else
            {
                fileName = Path.Combine(SupportCode.SupportCode.strPathToDataBackupFolder, Conversions.ToString(FileList.SelectedRows[0].Cells[0].Value));
                HideFile(fileName);
            }

            BtnRefresh.PerformClick();
        }

        private void HideFile(string fileName)
        {
            if (File.Exists(fileName))
            {
                var attributes = File.GetAttributes(fileName);
                attributes = attributes | FileAttributes.Hidden;
                File.SetAttributes(fileName, attributes);
            }
        }

        private void UnhideFile(string fileName)
        {
            if (File.Exists(fileName))
            {
                var attributes = File.GetAttributes(fileName);
                attributes = attributes & ~FileAttributes.Hidden;
                File.SetAttributes(fileName, attributes);
            }
        }

        private void ChkShowHiddenAsGray_Click(object sender, EventArgs e)
        {
            My.MySettingsProperty.Settings.boolShowHiddenAsGray = ChkShowHiddenAsGray.Checked;
            BtnRefresh.PerformClick();
        }

        private void FileList_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            if (boolDoneLoading)
            {
                My.MySettingsProperty.Settings.ColViewLogBackupsFileDate = ColFileDate.Width;
                My.MySettingsProperty.Settings.ColViewLogBackupsFileName = ColFileName.Width;
                My.MySettingsProperty.Settings.ColViewLogBackupsFileSize = ColFileSize.Width;
                My.MySettingsProperty.Settings.viewLogBackupsEntryCountColumnSize = colEntryCount.Width;
                My.MySettingsProperty.Settings.viewLogBackupsHiddenColumnSize = colHidden.Width;
            }
        }

        private void ViewLogBackups_FormClosing(object sender, FormClosingEventArgs e)
        {
            My.MySettingsProperty.Settings.fileListColumnOrder = SupportCode.SupportCode.SaveColumnOrders(FileList.Columns);
        }

        private void FileList_MouseDown(object sender, MouseEventArgs e)
        {
            var hitTest = FileList.HitTest(e.X, e.Y);

            if (hitTest.Type == DataGridViewHitTestType.ColumnHeader)
            {
                FileList.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            }
        }

        private void FileList_MouseUp(object sender, MouseEventArgs e)
        {
            var hitTest = FileList.HitTest(e.X, e.Y);

            if (hitTest.Type == DataGridViewHitTestType.ColumnHeader)
            {
                FileList.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
            }
        }

        private void ChkIgnoreSearchResultsLimits_Click(object sender, EventArgs e)
        {
            My.MySettingsProperty.Settings.IgnoreSearchResultLimits = ChkIgnoreSearchResultsLimits.Checked;
        }

        private void ChkLogFileDeletions_Click(object sender, EventArgs e)
        {
            My.MySettingsProperty.Settings.LogFileDeletions = ChkLogFileDeletions.Checked;
        }
    }
}