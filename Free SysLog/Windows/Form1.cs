using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Free_SysLog.SupportCode;
using Microsoft.Toolkit.Uwp.Notifications;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using Microsoft.Win32;

namespace Free_SysLog
{

    public partial class Form1
    {
        private bool boolMaximizedBeforeMinimize;
        private bool boolDoneLoading = false;
        public long longNumberOfIgnoredLogs = 0;
        public List<MyDataGridViewRow> IgnoredLogs = new List<MyDataGridViewRow>();
        public Dictionary<string, Regex> regexCache = new Dictionary<string, Regex>();
        public int intSortColumnIndex = 0; // Define intColumnNumber at class level
        public SortOrder sortOrder = SortOrder.Ascending; // Define soSortOrder at class level
        public readonly object dataGridLockObject = new object();
        public readonly object IgnoredLogsLockObject = new object();
        private const string strPayPal = "https://paypal.me/trparky";
        private System.Threading.Thread serverThread;
        private SyslogTcpServer.SyslogTcpServer SyslogTcpServer;
        private bool boolServerRunning = false;
        private bool boolTCPServerRunning = false;

        #region --== Midnight Timer Code ==--
        // This implementation is based on code found at https://www.codeproject.com/Articles/18201/Midnight-Timer-A-Way-to-Detect-When-it-is-Midnight.
        // I have rewritten the code to ensure that I fully understand it and to avoid blatantly copying someone else's work.
        // Using their code as-is without making an effort to learn from it or to create my own implementation doesn't sit well with me.

        private System.Timers.Timer MyMidnightTimer;

        public Form1()
        {
            InitializeComponent();
        }

        private void CreateNewMidnightTimer()
        {
            if (MyMidnightTimer is not null)
            {
                MyMidnightTimer.Stop();
                MyMidnightTimer.Dispose();
                MyMidnightTimer = null;
            }

            // Calculate the time span until midnight
            var ts = GetMidnight(1).Subtract(DateTime.Now);
            var tsMidnight = new TimeSpan(ts.Hours, ts.Minutes, ts.Seconds);

            // Create and start the new timer
            MyMidnightTimer = new System.Timers.Timer(tsMidnight.TotalMilliseconds);

            MyMidnightTimer.Elapsed += MidnightEvent;
            SystemEvents.TimeChanged += WindowsTimeChangeHandler;

            MyMidnightTimer.Start();
        }

        private void MidnightEvent(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (Logs.InvokeRequired)
            {
                Logs.Invoke(new Action<object, System.Timers.ElapsedEventArgs>(MidnightEvent), sender, e);
                return;
            }

            lock (dataGridLockObject)
            {
                if (My.MySettingsProperty.Settings.BackupOldLogsAfterClearingAtMidnight)
                    MakeLogBackup();

                int oldLogCount = Logs.Rows.Count;
                Logs.Rows.Clear();

                MyDataGridViewRow localMakeLocalDataGridRowEntry() { var argdataGrid = Logs; var ret = SyslogParser.SyslogParser.MakeLocalDataGridRowEntry($"The program deleted {oldLogCount:N0} log {(oldLogCount == 1 ? "entry" : "entries")} at midnight.", ref argdataGrid); Logs = argdataGrid; return ret; }

                Logs.Rows.Add(localMakeLocalDataGridRowEntry());

                UpdateLogCount();
                SelectLatestLogEntry();
                BtnSaveLogsToDisk.Enabled = true;

                NumberOfLogs.Text = $"Number of Log Entries: {Logs.Rows.Count:N0}";
            }

            CreateNewMidnightTimer();
        }

        private void WindowsTimeChangeHandler(object sender, EventArgs e)
        {
            CreateNewMidnightTimer();
        }

        private DateTime GetMidnight(int minutesAfterMidnight)
        {
            var tomorrow = DateTime.Now.AddDays(1d);
            return new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 0, minutesAfterMidnight, 0);
        }

        private void BackupOldLogsAfterClearingAtMidnight_Click(object sender, EventArgs e)
        {
            My.MySettingsProperty.Settings.DeleteOldLogsAtMidnight = BackupOldLogsAfterClearingAtMidnight.Checked;
        }

        private void DeleteOldLogsAtMidnight_Click(object sender, EventArgs e)
        {
            My.MySettingsProperty.Settings.DeleteOldLogsAtMidnight = DeleteOldLogsAtMidnight.Checked;
            BackupOldLogsAfterClearingAtMidnight.Enabled = DeleteOldLogsAtMidnight.Checked;

            if (!DeleteOldLogsAtMidnight.Checked)
            {
                DeleteOldLogsAtMidnight.Checked = false;
                My.MySettingsProperty.Settings.DeleteOldLogsAtMidnight = false;
            }

            if (DeleteOldLogsAtMidnight.Checked)
            {
                CreateNewMidnightTimer();
            }
            else if (MyMidnightTimer is not null)
            {
                MyMidnightTimer.Stop();
                MyMidnightTimer.Dispose();
                MyMidnightTimer = null;
            }
        }
        #endregion

        private string GetDateStringBasedOnUserPreference(DateTime dateObject)
        {
            switch (My.MySettingsProperty.Settings.DateFormat)
            {
                case 1:
                    {
                        return dateObject.ToLongDateString().Replace("/", "-").Replace(@"\", "-");
                    }
                case 2:
                    {
                        return dateObject.ToShortDateString().Replace("/", "-").Replace(@"\", "-");
                    }
                case 3:
                    {
                        return dateObject.ToString(My.MySettingsProperty.Settings.CustomDateFormat).Replace("/", "-").Replace(@"\", "-");
                    }

                default:
                    {
                        return dateObject.ToLongDateString().Replace("/", "-").Replace(@"\", "-");
                    }
            }
        }

        private void MakeLogBackup()
        {
            DataHandling.DataHandling.WriteLogsToDisk();
            File.Copy(SupportCode.SupportCode.strPathToDataFile, GetUniqueFileName(Path.Combine(SupportCode.SupportCode.strPathToDataBackupFolder, $"{GetDateStringBasedOnUserPreference(DateTime.Now.AddDays(-1))} Backup.json")));
        }

        private void ChkStartAtUserStartup_Click(object sender, EventArgs e)
        {
            if (ChkEnableStartAtUserStartup.Checked)
            {
                TaskHandling.TaskHandling.CreateTask();
                StartUpDelay.Enabled = true;
                StartUpDelay.Text = "        Startup Delay (60 Seconds)";
            }
            else
            {
                using (var taskService = new Microsoft.Win32.TaskScheduler.TaskService())
                {
                    taskService.RootFolder.DeleteTask($"Free SysLog for {Environment.UserName}");
                }

                StartUpDelay.Enabled = false;
            }
        }

        public void SelectLatestLogEntry()
        {
            if (ChkEnableAutoScroll.Checked && Logs.Rows.Count > 0 && intSortColumnIndex == 0)
            {
                SupportCode.SupportCode.boolIsProgrammaticScroll = true;
                Logs.BeginInvoke(new Action(() =>
                    {
                        Logs.FirstDisplayedScrollingRowIndex = sortOrder == SortOrder.Ascending ? Logs.Rows.Count - 1 : 0;
                        SupportCode.SupportCode.boolIsProgrammaticScroll = false;
                    }));
            }
        }

        private void Form1_ResizeBegin(object sender, EventArgs e)
        {
            Logs.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
        }

        private void Form1_ResizeEnd(object sender, EventArgs e)
        {
            My.MySettingsProperty.Settings.mainWindowSize = Size;
            System.Threading.Thread.Sleep(100);
            SelectLatestLogEntry();
            Logs.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
            SupportCode.SupportCode.boolIsProgrammaticScroll = false;
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (boolDoneLoading)
            {
                SupportCode.SupportCode.boolIsProgrammaticScroll = true;

                if (WindowState == FormWindowState.Minimized)
                {
                    if (My.MySettingsProperty.Settings.boolDeselectItemsWhenMinimizing)
                    {
                        Logs.ClearSelection();
                        LblItemsSelected.Visible = false;
                    }

                    boolMaximizedBeforeMinimize = WindowState == FormWindowState.Maximized;
                }
                else
                {
                    My.MySettingsProperty.Settings.boolMaximized = WindowState == FormWindowState.Maximized;
                }

                if (SupportCode.SupportCode.IgnoredLogsAndSearchResultsInstance is not null)
                    SupportCode.SupportCode.IgnoredLogsAndSearchResultsInstance.BtnViewMainWindow.Enabled = WindowState == FormWindowState.Minimized;
                if (MinimizeToClockTray.Checked)
                    ShowInTaskbar = WindowState != FormWindowState.Minimized;

                Logs.Invalidate();
                Logs.Refresh();
            }
        }

        private void ChkAutoSave_Click(object sender, EventArgs e)
        {
            SaveTimer.Enabled = ChkEnableAutoSave.Checked;
            ChangeLogAutosaveIntervalToolStripMenuItem.Visible = ChkEnableAutoSave.Checked;
            LblAutoSaved.Visible = ChkEnableAutoSave.Checked;
        }

        private void SaveTimer_Tick(object sender, EventArgs e)
        {
            DataHandling.DataHandling.WriteLogsToDisk();
            LblAutoSaved.Text = $"Last Auto-Saved At: {DateTime.Now:h:mm:ss tt}";
        }

        public void RestoreWindow()
        {
            if (boolMaximizedBeforeMinimize)
            {
                WindowState = FormWindowState.Maximized;
            }
            else if (My.MySettingsProperty.Settings.boolMaximized)
            {
                WindowState = FormWindowState.Maximized;
            }
            else
            {
                WindowState = FormWindowState.Normal;
            }

            BringToFront();
            Activate();

            SelectLatestLogEntry();
        }

        private void NotifyIcon_DoubleClick(object sender, EventArgs e)
        {
            RestoreWindow();
        }

        private void LoadCheckboxSettings()
        {
            if (My.MySettingsProperty.Settings.NotificationLength == 0)
            {
                NotificationLengthShort.Checked = true;
                NotificationLengthLong.Checked = false;
            }
            else
            {
                NotificationLengthShort.Checked = false;
                NotificationLengthLong.Checked = true;
            }

            ColLog.AutoSizeMode = My.MySettingsProperty.Settings.colLogAutoFill ? DataGridViewAutoSizeColumnMode.Fill : DataGridViewAutoSizeColumnMode.NotSet;

            ColLogsAutoFill.Checked = My.MySettingsProperty.Settings.colLogAutoFill;
            IncludeButtonsOnNotifications.Checked = My.MySettingsProperty.Settings.IncludeButtonsOnNotifications;
            AutomaticallyCheckForUpdates.Checked = My.MySettingsProperty.Settings.boolCheckForUpdates;
            ChkDeselectItemAfterMinimizingWindow.Checked = My.MySettingsProperty.Settings.boolDeselectItemsWhenMinimizing;
            ChkEnableRecordingOfIgnoredLogs.Checked = My.MySettingsProperty.Settings.recordIgnoredLogs;
            IgnoredLogsToolStripMenuItem.Visible = ChkEnableRecordingOfIgnoredLogs.Checked;
            ZerooutIgnoredLogsCounterToolStripMenuItem.Visible = !ChkEnableRecordingOfIgnoredLogs.Checked;
            ChkEnableAutoScroll.Checked = My.MySettingsProperty.Settings.autoScroll;
            ChkDisableAutoScrollUponScrolling.Enabled = ChkEnableAutoScroll.Checked;
            ChkEnableAutoSave.Checked = My.MySettingsProperty.Settings.autoSave;
            ChkEnableConfirmCloseToolStripItem.Checked = My.MySettingsProperty.Settings.boolConfirmClose;
            LblAutoSaved.Visible = ChkEnableAutoSave.Checked;
            ColAlerts.Visible = My.MySettingsProperty.Settings.boolShowAlertedColumn;
            ChkShowAlertedColumn.Checked = My.MySettingsProperty.Settings.boolShowAlertedColumn;
            MinimizeToClockTray.Checked = My.MySettingsProperty.Settings.MinimizeToClockTray;
            StopServerStripMenuItem.Visible = SupportCode.SupportCode.boolDoWeOwnTheMutex;
            ChkEnableStartAtUserStartup.Checked = Conversions.ToBoolean(TaskHandling.TaskHandling.DoesTaskExist());
            DeleteOldLogsAtMidnight.Checked = My.MySettingsProperty.Settings.DeleteOldLogsAtMidnight;
            BackupOldLogsAfterClearingAtMidnight.Enabled = My.MySettingsProperty.Settings.DeleteOldLogsAtMidnight;
            BackupOldLogsAfterClearingAtMidnight.Checked = My.MySettingsProperty.Settings.BackupOldLogsAfterClearingAtMidnight;
            ViewLogBackups.Visible = BackupOldLogsAfterClearingAtMidnight.Checked;
            ChkEnableTCPSyslogServer.Checked = My.MySettingsProperty.Settings.EnableTCPServer;
            ColHostname.Visible = My.MySettingsProperty.Settings.boolShowHostnameColumn;
            ChkShowHostnameColumn.Checked = My.MySettingsProperty.Settings.boolShowHostnameColumn;
            colServerTime.Visible = My.MySettingsProperty.Settings.boolShowServerTimeColumn;
            ChkShowServerTimeColumn.Checked = My.MySettingsProperty.Settings.boolShowServerTimeColumn;
            colLogType.Visible = My.MySettingsProperty.Settings.boolShowLogTypeColumn;
            ChkShowLogTypeColumn.Checked = My.MySettingsProperty.Settings.boolShowLogTypeColumn;
            RemoveNumbersFromRemoteApp.Checked = My.MySettingsProperty.Settings.RemoveNumbersFromRemoteApp;
            IPv6Support.Checked = My.MySettingsProperty.Settings.IPv6Support;
            ChkDisableAutoScrollUponScrolling.Checked = My.MySettingsProperty.Settings.disableAutoScrollUponScrolling;
            ChkDebug.Checked = My.MySettingsProperty.Settings.boolDebug;
            ConfirmDelete.Checked = My.MySettingsProperty.Settings.ConfirmDelete;
        }

        private void LoadAndDeserializeArrays()
        {
            ReplacementsClass tempReplacementsClass;
            SysLogProxyServer tempSysLogProxyServer;
            IgnoredClass tempIgnoredClass;
            AlertsClass tempAlertsClass;

            if (My.MySettingsProperty.Settings.replacements is not null && My.MySettingsProperty.Settings.replacements.Count > 0)
            {
                foreach (string strJSONString in My.MySettingsProperty.Settings.replacements)
                {
                    tempReplacementsClass = Newtonsoft.Json.JsonConvert.DeserializeObject<ReplacementsClass>(strJSONString, SupportCode.SupportCode.JSONDecoderSettingsForSettingsFiles);
                    if (tempReplacementsClass.BoolEnabled)
                        SupportCode.SupportCode.replacementsList.Add(tempReplacementsClass);
                    tempReplacementsClass = null;
                }
            }

            if (My.MySettingsProperty.Settings.hostnames is not null && My.MySettingsProperty.Settings.hostnames.Count > 0)
            {
                CustomHostname customHostname;

                foreach (string strJSONString in My.MySettingsProperty.Settings.hostnames)
                {
                    customHostname = Newtonsoft.Json.JsonConvert.DeserializeObject<CustomHostname>(strJSONString, SupportCode.SupportCode.JSONDecoderSettingsForSettingsFiles);
                    SupportCode.SupportCode.hostnames.Add(customHostname.ip, customHostname.deviceName);
                }
            }

            if (My.MySettingsProperty.Settings.ServersToSendTo is not null && My.MySettingsProperty.Settings.ServersToSendTo.Count > 0)
            {
                foreach (string strJSONString in My.MySettingsProperty.Settings.ServersToSendTo)
                {
                    tempSysLogProxyServer = Newtonsoft.Json.JsonConvert.DeserializeObject<SysLogProxyServer>(strJSONString, SupportCode.SupportCode.JSONDecoderSettingsForSettingsFiles);
                    if (tempSysLogProxyServer.boolEnabled)
                        SupportCode.SupportCode.serversList.Add(tempSysLogProxyServer);
                    tempSysLogProxyServer = null;
                }
            }

            if (My.MySettingsProperty.Settings.ignored2 is null)
            {
                My.MySettingsProperty.Settings.ignored2 = new System.Collections.Specialized.StringCollection();

                if (My.MySettingsProperty.Settings.ignored is not null && My.MySettingsProperty.Settings.ignored.Count > 0)
                {
                    foreach (string strIgnoredString in My.MySettingsProperty.Settings.ignored)
                        My.MySettingsProperty.Settings.ignored2.Add(Newtonsoft.Json.JsonConvert.SerializeObject(new IgnoredClass() { BoolCaseSensitive = false, BoolRegex = false, StrIgnore = strIgnoredString }));

                    My.MySettingsProperty.Settings.ignored.Clear();
                    My.MySettingsProperty.Settings.Save();
                }
            }

            if (My.MySettingsProperty.Settings.ignored2 is not null && My.MySettingsProperty.Settings.ignored2.Count > 0)
            {
                foreach (string strJSONString in My.MySettingsProperty.Settings.ignored2)
                {
                    tempIgnoredClass = Newtonsoft.Json.JsonConvert.DeserializeObject<IgnoredClass>(strJSONString, SupportCode.SupportCode.JSONDecoderSettingsForSettingsFiles);
                    if (tempIgnoredClass.BoolEnabled)
                        SupportCode.SupportCode.ignoredList.Add(tempIgnoredClass);
                    tempIgnoredClass = null;
                }
            }

            if (My.MySettingsProperty.Settings.alerts is not null && My.MySettingsProperty.Settings.alerts.Count > 0)
            {
                foreach (string strJSONString in My.MySettingsProperty.Settings.alerts)
                {
                    tempAlertsClass = Newtonsoft.Json.JsonConvert.DeserializeObject<AlertsClass>(strJSONString, SupportCode.SupportCode.JSONDecoderSettingsForSettingsFiles);
                    if (tempAlertsClass.BoolEnabled)
                        SupportCode.SupportCode.alertsList.Add(tempAlertsClass);
                    tempAlertsClass = null;
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            SyslogParser.SyslogParser.SetParentForm = this;
            DataHandling.DataHandling.SetParentForm = this;
            TaskHandling.TaskHandling.SetParentForm = this;

            TaskHandling.TaskHandling.ConvertRegistryRunCommandToTask();

            if (My.MySettingsProperty.Settings.DeleteOldLogsAtMidnight)
                CreateNewMidnightTimer();

            ChangeLogAutosaveIntervalToolStripMenuItem.Text = $"        Change Log Autosave Interval ({My.MySettingsProperty.Settings.autoSaveMinutes} Minutes)";
            ChangeSyslogServerPortToolStripMenuItem.Text = $"Change Syslog Server Port (Port Number {My.MySettingsProperty.Settings.sysLogPort})";
            ConfigureTimeBetweenSameNotifications.Text = $"Configure Time Between Same Notifications ({My.MySettingsProperty.Settings.TimeBetweenSameNotifications} Seconds)";

            ColTime.HeaderCell.Style.Padding = new Padding(0, 0, 1, 0);
            ColIPAddress.HeaderCell.Style.Padding = new Padding(0, 0, 2, 0);

            Logs.DefaultCellStyle.Padding = new Padding(0, 20, 0, 20); // Left, Top, Right, Bottom

            ColTime.HeaderCell.SortGlyphDirection = SortOrder.Ascending;
            Icon = Icon.ExtractAssociatedIcon(SupportCode.SupportCode.strEXEPath);
            Form argwindow = this;
            Location = SupportCode.SupportCode.VerifyWindowLocation(My.MySettingsProperty.Settings.windowLocation, ref argwindow);
            if (My.MySettingsProperty.Settings.boolMaximized)
                WindowState = FormWindowState.Maximized;
            NotifyIcon.Icon = Icon;
            NotifyIcon.Text = "Free SysLog";

            LoadCheckboxSettings();

            var flags = BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty;
            var propInfo = typeof(DataGridView).GetProperty("DoubleBuffered", flags);
            propInfo?.SetValue(Logs, true, null);

            Logs.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle() { BackColor = My.MySettingsProperty.Settings.searchColor, ForeColor = SupportCode.SupportCode.GetGoodTextColorBasedUponBackgroundColor(My.MySettingsProperty.Settings.searchColor) };
            Logs.DefaultCellStyle = new DataGridViewCellStyle() { WrapMode = DataGridViewTriState.True };
            ColLog.DefaultCellStyle = new DataGridViewCellStyle() { WrapMode = DataGridViewTriState.True };

            LoadAndDeserializeArrays();
            var argcolumns = Logs.Columns;
            var argspecializedStringCollection = My.MySettingsProperty.Settings.logsColumnOrder;
            SupportCode.SupportCode.LoadColumnOrders(ref argcolumns, ref argspecializedStringCollection);
            My.MySettingsProperty.Settings.logsColumnOrder = argspecializedStringCollection;

            if (My.MySettingsProperty.Settings.autoSave)
            {
                SaveTimer.Interval = (int)Math.Round(TimeSpan.FromMinutes(My.MySettingsProperty.Settings.autoSaveMinutes).TotalMilliseconds);
                SaveTimer.Enabled = true;
            }

            Size = My.MySettingsProperty.Settings.mainWindowSize;

            ColTime.Width = My.MySettingsProperty.Settings.columnTimeSize;
            colServerTime.Width = My.MySettingsProperty.Settings.ServerTimeWidth;
            colLogType.Width = My.MySettingsProperty.Settings.LogTypeWidth;
            ColIPAddress.Width = My.MySettingsProperty.Settings.columnIPSize;
            ColHostname.Width = My.MySettingsProperty.Settings.HostnameWidth;
            ColRemoteProcess.Width = My.MySettingsProperty.Settings.RemoteProcessHeaderSize;
            ColLog.Width = My.MySettingsProperty.Settings.columnLogSize;
            ColAlerts.Width = My.MySettingsProperty.Settings.columnAlertedSize;

            if (My.MySettingsProperty.Settings.font is not null)
            {
                Logs.DefaultCellStyle.Font = My.MySettingsProperty.Settings.font;
                Logs.ColumnHeadersDefaultCellStyle.Font = My.MySettingsProperty.Settings.font;
            }

            boolDoneLoading = true;

            var worker = new BackgroundWorker();
            worker.DoWork += (a,b) => LoadDataFile();
            worker.RunWorkerCompleted += RunWorkerCompleted;
            worker.RunWorkerAsync();

            AddNotificationActionHandler();
        }

        private void AddNotificationActionHandler()
        {
            ToastNotificationManagerCompat.OnActivated += args =>
                {
                    // Parse arguments
                    var argsDictionary = new Dictionary<string, string>();
                    string[] itemSplit;

                    foreach (string item in args.Argument.Split(';'))
                    {
                        if (!string.IsNullOrWhiteSpace(item))
                        {
                            itemSplit = item.Split('=');

                            if (itemSplit.Length == 2 && !string.IsNullOrWhiteSpace(itemSplit[0]))
                            {
                                argsDictionary[itemSplit[0]] = itemSplit[1];
                            }
                        }
                    }

                    if (argsDictionary.ContainsKey("action"))
                    {
                        if (argsDictionary["action"].ToString().Equals(SupportCode.SupportCode.strOpenSysLog, StringComparison.OrdinalIgnoreCase))
                        {
                            Invoke(new Action(() => RestoreWindow()));
                        }
                        else if (argsDictionary["action"].ToString().Equals(SupportCode.SupportCode.strViewLog, StringComparison.OrdinalIgnoreCase) && argsDictionary.ContainsKey("datapacket"))
                        {
                            try
                            {
                                var NotificationDataPacket = Newtonsoft.Json.JsonConvert.DeserializeObject<NotificationDataPacket>(argsDictionary["datapacket"].ToString(), SupportCode.SupportCode.JSONDecoderSettingsForSettingsFiles);

                                OpenLogViewerWindow(NotificationDataPacket.logtext, NotificationDataPacket.alerttext, NotificationDataPacket.logdate, NotificationDataPacket.sourceip, NotificationDataPacket.rawlogtext);
                            }
                            catch (Exception ex)
                            {
                            }
                        }
                    }
                };
        }

        private async void StartTCPServer()
        {
            SyslogTcpServer = new SyslogTcpServer.SyslogTcpServer(new Action<string, string>(SyslogParser.SyslogParser.ProcessIncomingLog), My.MySettingsProperty.Settings.sysLogPort);
            await SyslogTcpServer.StartAsync();
        }

        private void RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (My.MySettingsProperty.Settings.boolCheckForUpdates)
            {
                System.Threading.ThreadPool.QueueUserWorkItem((a) =>
                {
                    var checkForUpdatesClassObject = new checkForUpdates.CheckForUpdatesClass(this);
                    checkForUpdatesClassObject.CheckForUpdates(false);
                });
            }

            if (SupportCode.SupportCode.boolDoWeOwnTheMutex)
            {
                serverThread = new System.Threading.Thread(SysLogThread) { Name = "UDP Server Thread", Priority = System.Threading.ThreadPriority.Normal };
                serverThread.Start();

                if (My.MySettingsProperty.Settings.EnableTCPServer)
                {
                    StartTCPServer();
                    boolTCPServerRunning = true;
                }

                boolServerRunning = true;
            }
            else
            {
                Interaction.MsgBox("Unable to start syslog server, perhaps another instance of this program is running on your system.", (MsgBoxStyle)((int)MsgBoxStyle.Critical + (int)MsgBoxStyle.ApplicationModal), Text);
            }
        }

        private void LoadDataFile(bool boolShowDataLoadedEvent = true)
        {
            if (File.Exists(SupportCode.SupportCode.strPathToDataFile))
            {
                try
                {
                    Invoke(new Action(() =>
                        {
                            MyDataGridViewRow localMakeLocalDataGridRowEntry() { var argdataGrid1 = Logs; var ret = SyslogParser.SyslogParser.MakeLocalDataGridRowEntry("Loading data and populating data grid... Please Wait.", ref argdataGrid1); Logs = argdataGrid1; return ret; }

                            Logs.Rows.Add(localMakeLocalDataGridRowEntry());
                            LblLogFileSize.Text = $"Log File Size: {SupportCode.SupportCode.FileSizeToHumanSize(new FileInfo(SupportCode.SupportCode.strPathToDataFile).Length)}";
                        }));

                    var collectionOfSavedData = new List<SavedData>();

                    using (var fileStream = new StreamReader(SupportCode.SupportCode.strPathToDataFile))
                    {
                        collectionOfSavedData = Newtonsoft.Json.JsonConvert.DeserializeObject<List<SavedData>>(fileStream.ReadToEnd().Trim(), SupportCode.SupportCode.JSONDecoderSettingsForLogFiles);
                    }

                    var listOfLogEntries = new List<MyDataGridViewRow>();
                    var stopwatch = Stopwatch.StartNew();

                    if (collectionOfSavedData.Any())
                    {
                        int intProgress = 0;

                        Invoke(new Action(() => LoadingProgressBar.Visible = true));

                        foreach (SavedData item in collectionOfSavedData)
                        {
                            MyDataGridViewRow localMakeDataGridRow() { var argdataGrid2 = Logs; var ret = item.MakeDataGridRow(ref argdataGrid2); Logs = argdataGrid2; return ret; }

                            listOfLogEntries.Add(localMakeDataGridRow());
                            intProgress += 1;
                            Invoke(new Action(() => LoadingProgressBar.Value = (int)Math.Round(intProgress / (double)collectionOfSavedData.Count * 100d)));
                        }

                        Invoke(new Action(() => LoadingProgressBar.Visible = false));
                    }

                    if (boolShowDataLoadedEvent)
                    {
                        MyDataGridViewRow localMakeLocalDataGridRowEntry() { var argdataGrid3 = Logs; var ret = SyslogParser.SyslogParser.MakeLocalDataGridRowEntry($"Free SysLog Server Started. Data loaded in {SupportCode.SupportCode.MyRoundingFunction(stopwatch.Elapsed.TotalMilliseconds / 1000d, 2)} seconds.", ref argdataGrid3); Logs = argdataGrid3; return ret; }

                        listOfLogEntries.Add(localMakeLocalDataGridRowEntry());
                    }

                    lock (dataGridLockObject)
                    {
                        Invoke(new Action(() =>
                            {
                                Logs.SuspendLayout();
                                Logs.Rows.Clear();
                                Logs.Rows.AddRange(listOfLogEntries.ToArray());
                                Logs.ResumeLayout();

                                SelectLatestLogEntry();

                                Logs.SelectedRows[0].Selected = false;
                                UpdateLogCount();
                                Logs.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
                            }));
                    }
                }
                catch (Newtonsoft.Json.JsonSerializationException ex)
                {
                    HandleLogFileLoadException(ex);
                }
                catch (Newtonsoft.Json.JsonReaderException ex)
                {
                    HandleLogFileLoadException(ex);
                }
            }
        }

        private void HandleLogFileLoadException(Exception ex)
        {
            if (File.Exists($"{SupportCode.SupportCode.strPathToDataFile}.bad"))
            {
                File.Copy(SupportCode.SupportCode.strPathToDataFile, GetUniqueFileName($"{SupportCode.SupportCode.strPathToDataFile}.bad"));
            }
            else
            {
                File.Copy(SupportCode.SupportCode.strPathToDataFile, $"{SupportCode.SupportCode.strPathToDataFile}.bad");
            }

            File.WriteAllText(SupportCode.SupportCode.strPathToDataFile, "[]");
            LblLogFileSize.Text = $"Log File Size: {SupportCode.SupportCode.FileSizeToHumanSize(new FileInfo(SupportCode.SupportCode.strPathToDataFile).Length)}";

            lock (dataGridLockObject)
            {
                Invoke(new Action(() =>
                    {
                        Logs.SuspendLayout();
                        Logs.Rows.Clear();

                        MyDataGridViewRow localMakeLocalDataGridRowEntry() { var argdataGrid4 = Logs; var ret = SyslogParser.SyslogParser.MakeLocalDataGridRowEntry("Free SysLog Server Started.", ref argdataGrid4); Logs = argdataGrid4; return ret; }
                        MyDataGridViewRow localMakeLocalDataGridRowEntry1() { var argdataGrid5 = Logs; var ret = SyslogParser.SyslogParser.MakeLocalDataGridRowEntry("There was an error while decoing the JSON data, existing data was copied to another file and the log file was reset.", ref argdataGrid5); Logs = argdataGrid5; return ret; }
                        MyDataGridViewRow localMakeLocalDataGridRowEntry2() { var argdataGrid6 = Logs; var ret = SyslogParser.SyslogParser.MakeLocalDataGridRowEntry($"Exception Type: {ex.GetType()}{Constants.vbCrLf}Exception Message: {ex.Message}{Constants.vbCrLf}{Constants.vbCrLf}Exception Stack Trace{Constants.vbCrLf}{ex.StackTrace}", ref argdataGrid6); Logs = argdataGrid6; return ret; }
                        var listOfLogEntries = new List<MyDataGridViewRow>() { localMakeLocalDataGridRowEntry(), localMakeLocalDataGridRowEntry1(), localMakeLocalDataGridRowEntry2() };

                        Logs.Rows.AddRange(listOfLogEntries.ToArray());
                        Logs.ResumeLayout();
                        UpdateLogCount();
                    }));
            }
        }

        private string GetUniqueFileName(string fileName)
        {
            var fileInfo = new FileInfo(fileName);

            string strDirectory = fileInfo.DirectoryName;
            string strFileBase = Path.GetFileNameWithoutExtension(fileInfo.Name);
            string strFileExtension = fileInfo.Extension;

            if (string.IsNullOrWhiteSpace(strDirectory))
                strDirectory = Directory.GetCurrentDirectory();

            string strNewFileName = Path.Combine(strDirectory, fileInfo.Name);
            int intCount = 1;

            while (File.Exists(strNewFileName))
            {
                strNewFileName = Path.Combine(strDirectory, $"{strFileBase} ({intCount}){strFileExtension}");
                intCount += 1;
            }

            return strNewFileName;
        }

        private void BtnOpenLogLocation_Click(object sender, EventArgs e)
        {
            SupportCode.SupportCode.SelectFileInWindowsExplorer(SupportCode.SupportCode.strPathToDataFile);
        }

        private void OpenLogViewerWindow(string strLogText, string strAlertText, string strLogDate, string strSourceIP, string strRawLogText)
        {
            strRawLogText = strRawLogText.Replace("{newline}", Constants.vbCrLf, StringComparison.OrdinalIgnoreCase);

            using (var LogViewerInstance = new LogViewer() { strRawLogText = strRawLogText, strLogText = strLogText, StartPosition = FormStartPosition.CenterParent, Icon = Icon })
            {
                LogViewerInstance.LblLogDate.Text = $"Log Date: {strLogDate}";
                LogViewerInstance.LblSource.Text = $"Source IP Address: {strSourceIP}";
                LogViewerInstance.TopMost = true;
                LogViewerInstance.lblAlertText.Text = $"Alert Text: {strAlertText}";

                LogViewerInstance.ShowDialog();
            }
        }

        private void OpenLogViewerWindow()
        {
            if (Logs.Rows.Count > 0 & Logs.SelectedCells.Count > 0)
            {
                MyDataGridViewRow selectedRow = (MyDataGridViewRow)Logs.Rows[Logs.SelectedCells[0].RowIndex];
                string strLogText = Conversions.ToString(selectedRow.Cells[SupportCode.SupportCode.ColumnIndex_LogText].Value);
                string strRawLogText = Conversions.ToString(string.IsNullOrWhiteSpace(selectedRow.RawLogData) ? selectedRow.Cells[SupportCode.SupportCode.ColumnIndex_LogText].Value : selectedRow.RawLogData.Replace("{newline}", Constants.vbCrLf, StringComparison.OrdinalIgnoreCase));

                using (var LogViewerInstance = new LogViewer() { strRawLogText = strRawLogText, strLogText = strLogText, StartPosition = FormStartPosition.CenterParent, Icon = Icon, MyParentForm = this })
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

        private void Logs_DoubleClick(object sender, EventArgs e)
        {
            OpenLogViewerWindow();
        }

        private void Logs_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == (int)Keys.Enter)
            {
                OpenLogViewerWindow();
            }
            else if (e.KeyValue == (int)Keys.Delete)
            {
                lock (dataGridLockObject)
                {
                    int intNumberOfLogsDeleted = Logs.SelectedRows.Count;

                    if (ConfirmDelete.Checked)
                    {
                        Confirm_Delete.UserChoice choice;

                        using (var Confirm_Delete = new Confirm_Delete(intNumberOfLogsDeleted) { Icon = Icon, StartPosition = FormStartPosition.CenterParent })
                        {
                            Confirm_Delete.lblMainLabel.Text = $"Are you sure you want to delete the {intNumberOfLogsDeleted} selected {(intNumberOfLogsDeleted == 1 ? "log" : "logs")}?";
                            Confirm_Delete.ShowDialog(this);
                            choice = Confirm_Delete.choice;
                        }

                        if (choice == Confirm_Delete.UserChoice.NoDelete)
                        {
                            Interaction.MsgBox("Logs not deleted.", MsgBoxStyle.Information, Text);
                            return;
                        }
                        else if (choice == Confirm_Delete.UserChoice.YesDeleteYesBackup)
                        {
                            MakeLogBackup();
                        }
                    }

                    foreach (DataGridViewRow item in Logs.SelectedRows)
                        Logs.Rows.Remove(item);

                    MyDataGridViewRow localMakeLocalDataGridRowEntry() { var argdataGrid7 = Logs; var ret = SyslogParser.SyslogParser.MakeLocalDataGridRowEntry($"The user deleted {intNumberOfLogsDeleted:N0} log {(intNumberOfLogsDeleted == 1 ? "entry" : "entries")}.", ref argdataGrid7); Logs = argdataGrid7; return ret; }

                    Logs.Rows.Add(localMakeLocalDataGridRowEntry());

                    SelectLatestLogEntry();
                }

                UpdateLogCount();
                SaveLogsToDiskSub();
            }
        }

        private void Logs_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                e.Handled = true;
        }

        private void Logs_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            e.Cancel = true;
        }

        public void UpdateLogCount()
        {
            BtnClearLog.Enabled = Logs.Rows.Count != 0;
            NumberOfLogs.Text = $"Number of Log Entries: {Logs.Rows.Count:N0}";
        }

        private void ChkAutoScroll_Click(object sender, EventArgs e)
        {
            My.MySettingsProperty.Settings.autoScroll = ChkEnableAutoScroll.Checked;
            ChkDisableAutoScrollUponScrolling.Enabled = ChkEnableAutoScroll.Checked;
        }

        private void Logs_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            if (boolDoneLoading)
            {
                My.MySettingsProperty.Settings.columnTimeSize = ColTime.Width;
                My.MySettingsProperty.Settings.ServerTimeWidth = colServerTime.Width;
                My.MySettingsProperty.Settings.LogTypeWidth = colLogType.Width;
                My.MySettingsProperty.Settings.columnIPSize = ColIPAddress.Width;
                My.MySettingsProperty.Settings.HostnameWidth = ColHostname.Width;
                My.MySettingsProperty.Settings.RemoteProcessHeaderSize = ColRemoteProcess.Width;
                My.MySettingsProperty.Settings.columnLogSize = ColLog.Width;
                My.MySettingsProperty.Settings.columnAlertedSize = ColAlerts.Width;
            }
        }

        private void BtnClearAllLogs_Click(object sender, EventArgs e)
        {
            if (Interaction.MsgBox("Are you sure you want to clear the logs?", (MsgBoxStyle)((int)MsgBoxStyle.Question + (int)MsgBoxStyle.YesNo + (int)Constants.vbDefaultButton2), Text) == MsgBoxResult.Yes)
            {
                lock (dataGridLockObject)
                {
                    if (Interaction.MsgBox("Do you want to make a backup of the logs before deleting them?", (MsgBoxStyle)((int)MsgBoxStyle.Question + (int)MsgBoxStyle.YesNo + (int)Constants.vbDefaultButton2), Text) == MsgBoxResult.Yes)
                        MakeLogBackup();

                    int intOldCount = Logs.Rows.Count;
                    Logs.Rows.Clear();
                    MyDataGridViewRow localMakeLocalDataGridRowEntry() { var argdataGrid8 = Logs; var ret = SyslogParser.SyslogParser.MakeLocalDataGridRowEntry($"The user deleted {intOldCount:N0} log {(intOldCount == 1 ? "entry" : "entries")}.", ref argdataGrid8); Logs = argdataGrid8; return ret; }

                    Logs.Rows.Add(localMakeLocalDataGridRowEntry());

                    SelectLatestLogEntry();
                }

                UpdateLogCount();
                SaveLogsToDiskSub();
            }
        }

        public void SaveLogsToDiskSub()
        {
            DataHandling.DataHandling.WriteLogsToDisk();
            LblAutoSaved.Text = $"Last Saved At: {DateTime.Now:h:mm:ss tt}";
            SaveTimer.Enabled = false;
            SaveTimer.Enabled = true;
        }

        private void BtnSaveLogsToDisk_Click(object sender, EventArgs e)
        {
            SaveLogsToDiskSub();
        }

        private void BtnCheckForUpdates_Click(object sender, EventArgs e)
        {
            SaveLogsToDiskSub();

            System.Threading.ThreadPool.QueueUserWorkItem((a) =>
                {
                    var checkForUpdatesClassObject = new checkForUpdates.CheckForUpdatesClass(this);
                    checkForUpdatesClassObject.CheckForUpdates();
                });
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing && My.MySettingsProperty.Settings.boolConfirmClose)
            {
                using (var CloseFreeSysLog = new CloseFreeSysLogDialog())
                {
                    CloseFreeSysLog.StartPosition = FormStartPosition.CenterParent;
                    CloseFreeSysLog.MyParentForm = this;

                    var result = CloseFreeSysLog.ShowDialog(this);

                    if (result == DialogResult.No)
                    {
                        e.Cancel = true;
                        return;
                    }
                    else if (result == DialogResult.Cancel)
                    {
                        WindowState = FormWindowState.Minimized;
                        e.Cancel = true;
                        return;
                    }
                }
            }

            if (e.CloseReason == CloseReason.WindowsShutDown)
            {
                lock (dataGridLockObject)
                {
                    MyDataGridViewRow localMakeLocalDataGridRowEntry() { var argdataGrid9 = Logs; var ret = SyslogParser.SyslogParser.MakeLocalDataGridRowEntry("Windows shutdown initiated, Free Syslog is closing.", ref argdataGrid9); Logs = argdataGrid9; return ret; }

                    Logs.Rows.Add(localMakeLocalDataGridRowEntry());
                }
            }

            My.MySettingsProperty.Settings.logsColumnOrder = SupportCode.SupportCode.SaveColumnOrders(Logs.Columns);
            My.MySettingsProperty.Settings.Save();
            DataHandling.DataHandling.WriteLogsToDisk();

            if (SupportCode.SupportCode.boolDoWeOwnTheMutex)
            {
                SupportCode.SupportCode.SendMessageToSysLogServer(SupportCode.SupportCode.strTerminate, My.MySettingsProperty.Settings.sysLogPort);
                if (My.MySettingsProperty.Settings.EnableTCPServer)
                    SupportCode.SupportCode.SendMessageToTCPSysLogServer(SupportCode.SupportCode.strTerminate, My.MySettingsProperty.Settings.sysLogPort);
            }

            try
            {
                SupportCode.SupportCode.mutex.ReleaseMutex();
            }
            catch (ApplicationException ex)
            {
            }

            Process.GetCurrentProcess().Kill();
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
            var stopWatch = Stopwatch.StartNew();

            BtnSearch.Enabled = false;

            var worker = new BackgroundWorker();

            worker.DoWork += (a, b) => { try { RegexOptions regExOptions = (RegexOptions)(ChkCaseInsensitiveSearch.Checked ? (int)RegexOptions.Compiled + (int)RegexOptions.IgnoreCase : (int)RegexOptions.Compiled); if (ChkRegExSearch.Checked) { regexCompiledObject = new Regex(TxtSearchTerms.Text, regExOptions); } else { regexCompiledObject = new Regex(Regex.Escape(TxtSearchTerms.Text), regExOptions); } lock (dataGridLockObject) { foreach (DataGridViewRow item in Logs.Rows) { MyDataGridRowItem = item as MyDataGridViewRow; if (MyDataGridRowItem is not null) { strLogText = Conversions.ToString(MyDataGridRowItem.Cells[SupportCode.SupportCode.ColumnIndex_LogText].Value); if (!string.IsNullOrWhiteSpace(strLogText) && regexCompiledObject.IsMatch(strLogText)) { listOfSearchResults.Add((MyDataGridViewRow)MyDataGridRowItem.Clone()); } } } } } catch (ArgumentException ex) { Interaction.MsgBox("Malformed RegEx pattern detected, search aborted.", MsgBoxStyle.Critical, Text); } };

            worker.RunWorkerCompleted += (a, b) =>
                {
                    if (listOfSearchResults.Any())
                    {
                        var searchResultsWindow = new IgnoredLogsAndSearchResults(this, IgnoreOrSearchWindowDisplayMode.search) { MainProgramForm = this, Icon = Icon, LogsToBeDisplayed = listOfSearchResults, Text = "Search Results" };
                        searchResultsWindow.LogsLoadedInLabel.Visible = true;
                        searchResultsWindow.LogsLoadedInLabel.Text = $"Search took {SupportCode.SupportCode.MyRoundingFunction(stopWatch.Elapsed.TotalMilliseconds / 1000d, 2)} seconds";
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

        private void TxtSearchTerms_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                BtnSearch.PerformClick();
            }
        }

        private void Logs_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                // Disable user sorting
                Logs.AllowUserToOrderColumns = false;

                var column = Logs.Columns[e.ColumnIndex];
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
                SortLogsByDateObjectNoLocking(columnIndex, order);
        }

        public void SortLogsByDateObjectNoLocking(int columnIndex, SortOrder order)
        {
            Logs.AllowUserToOrderColumns = false;
            Logs.Enabled = false;

            var comparer = new DataGridViewComparer(columnIndex, order);
            MyDataGridViewRow[] rows = Logs.Rows.Cast<DataGridViewRow>().OfType<MyDataGridViewRow>().ToArray();

            Array.Sort(rows, comparer.Compare);

            Logs.SuspendLayout();
            Logs.Rows.Clear();
            Logs.Rows.AddRange(rows);
            Logs.ResumeLayout();

            Logs.Enabled = true;
            Logs.AllowUserToOrderColumns = true;
        }

        private void IgnoredWordsAndPhrasesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var IgnoredWordsAndPhrasesOrAlertsInstance = new IgnoredWordsAndPhrases() { Icon = Icon, StartPosition = FormStartPosition.CenterParent })
            {
                IgnoredWordsAndPhrasesOrAlertsInstance.ShowDialog(this);
                if (IgnoredWordsAndPhrasesOrAlertsInstance.boolChanged)
                    regexCache.Clear();
            }
        }

        private void ViewIgnoredLogsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (SupportCode.SupportCode.IgnoredLogsAndSearchResultsInstance is null)
            {
                SupportCode.SupportCode.IgnoredLogsAndSearchResultsInstance = new IgnoredLogsAndSearchResults(this, IgnoreOrSearchWindowDisplayMode.ignored) { MainProgramForm = this, Icon = Icon, LogsToBeDisplayed = IgnoredLogs, Text = "Ignored Logs" };
                SupportCode.SupportCode.IgnoredLogsAndSearchResultsInstance.ChkColLogsAutoFill.Checked = My.MySettingsProperty.Settings.colLogAutoFill;
                SupportCode.SupportCode.IgnoredLogsAndSearchResultsInstance.Show();
            }
            else
            {
                SupportCode.SupportCode.IgnoredLogsAndSearchResultsInstance.WindowState = FormWindowState.Normal;
                SupportCode.SupportCode.IgnoredLogsAndSearchResultsInstance.BringToFront();
            }
        }

        private void ClearIgnoredLogsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Interaction.MsgBox("Are you sure you want to clear the ignored logs stored in system memory?", (MsgBoxStyle)((int)MsgBoxStyle.Question + (int)MsgBoxStyle.YesNo + (int)Constants.vbDefaultButton2), Text) == MsgBoxResult.Yes)
                ClearIgnoredLogs();
        }

        public void ClearIgnoredLogs()
        {
            lock (IgnoredLogsLockObject)
            {
                IgnoredLogs.Clear();
                longNumberOfIgnoredLogs = 0;
                ClearIgnoredLogsToolStripMenuItem.Enabled = false;
                LblNumberOfIgnoredIncomingLogs.Text = $"Number of ignored incoming logs: {longNumberOfIgnoredLogs:N0}";
            }
        }

        protected override void WndProc(ref Message m)
        {
            const int WM_EXITSIZEMOVE = 0x232;

            base.WndProc(ref m);

            if (m.Msg == WM_EXITSIZEMOVE && boolDoneLoading)
            {
                Form argwindow = this;
                Location = SupportCode.SupportCode.VerifyWindowLocation(Location, ref argwindow);
                My.MySettingsProperty.Settings.windowLocation = Location;
            }
        }

        private void ChkRecordIgnoredLogs_Click(object sender, EventArgs e)
        {
            My.MySettingsProperty.Settings.recordIgnoredLogs = ChkEnableRecordingOfIgnoredLogs.Checked;
            IgnoredLogsToolStripMenuItem.Visible = ChkEnableRecordingOfIgnoredLogs.Checked;
            ZerooutIgnoredLogsCounterToolStripMenuItem.Visible = !ChkEnableRecordingOfIgnoredLogs.Checked;

            if (!ChkEnableRecordingOfIgnoredLogs.Checked)
            {
                IgnoredLogs.Clear();
                LblNumberOfIgnoredIncomingLogs.Text = "Number of ignored incoming logs: 0";
            }
        }

        private void LogsOlderThanToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var ClearLogsOlderThanInstance = new ClearLogsOlderThan() { Icon = Icon, StartPosition = FormStartPosition.CenterParent })
            {
                ClearLogsOlderThanInstance.LblLogCount.Text = $"Number of Log Entries: {Logs.Rows.Count:N0}";
                ClearLogsOlderThanInstance.ShowDialog(this);

                if (ClearLogsOlderThanInstance.DialogResult == DialogResult.OK)
                {
                    try
                    {
                        var dateChosenDate = ClearLogsOlderThanInstance.dateChosenDate;

                        lock (dataGridLockObject)
                        {
                            Logs.AllowUserToOrderColumns = false;
                            Logs.Enabled = false;

                            int intOldCount = Logs.Rows.Count;
                            var newListOfLogs = new List<MyDataGridViewRow>();

                            foreach (MyDataGridViewRow item in Logs.Rows)
                            {
                                if (item.DateObject.Date >= dateChosenDate.Date)
                                {
                                    newListOfLogs.Add((MyDataGridViewRow)item.Clone());
                                }
                            }

                            Logs.Enabled = true;
                            Logs.AllowUserToOrderColumns = true;

                            if (Interaction.MsgBox("Do you want to make a backup of the logs before deleting them?", (MsgBoxStyle)((int)MsgBoxStyle.Question + (int)MsgBoxStyle.YesNo + (int)Constants.vbDefaultButton2), Text) == MsgBoxResult.Yes)
                                MakeLogBackup();

                            Logs.SuspendLayout();
                            Logs.Rows.Clear();
                            Logs.Rows.AddRange(newListOfLogs.ToArray());

                            int intCountDifference = intOldCount - Logs.Rows.Count;
                            MyDataGridViewRow localMakeLocalDataGridRowEntry() { var argdataGrid10 = Logs; var ret = SyslogParser.SyslogParser.MakeLocalDataGridRowEntry($"The user deleted {intCountDifference:N0} log {(intCountDifference == 1 ? "entry" : "entries")}.", ref argdataGrid10); Logs = argdataGrid10; return ret; }

                            Logs.Rows.Add(localMakeLocalDataGridRowEntry());

                            Logs.ResumeLayout();

                            SelectLatestLogEntry();
                        }

                        UpdateLogCount();
                        SaveLogsToDiskSub();
                    }
                    catch (ArgumentOutOfRangeException ex)
                    {
                    }
                }
            }
        }

        private void ZerooutIgnoredLogsCounterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            longNumberOfIgnoredLogs = 0;
            LblNumberOfIgnoredIncomingLogs.Text = $"Number of ignored incoming logs: {longNumberOfIgnoredLogs:N0}";
            ZerooutIgnoredLogsCounterToolStripMenuItem.Enabled = false;
        }

        private void ConfigureReplacementsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var ReplacementsInstance = new Replacements() { Icon = Icon, StartPosition = FormStartPosition.CenterParent })
            {
                ReplacementsInstance.ShowDialog(this);
                if (ReplacementsInstance.boolChanged)
                    regexCache.Clear();
            }
        }

        private void ConfigureAlternatingColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var ColorDialog = new ColorDialog())
            {
                if (ColorDialog.ShowDialog() == DialogResult.OK)
                {
                    My.MySettingsProperty.Settings.searchColor = ColorDialog.Color;

                    var rowStyle = new DataGridViewCellStyle() { BackColor = ColorDialog.Color, ForeColor = SupportCode.SupportCode.GetGoodTextColorBasedUponBackgroundColor(ColorDialog.Color) };
                    Logs.AlternatingRowsDefaultCellStyle = rowStyle;
                }
            }
        }

        private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
#if DEBUG
            Interaction.MsgBox($"Free SysLog.{Constants.vbCrLf}{Constants.vbCrLf}Version {checkForUpdates.checkForUpdatesModule.versionString} (Debug Build){Constants.vbCrLf}{Constants.vbCrLf}Copyright Thomas Parkison © 2023-2025", MsgBoxStyle.Information, Text);
#else
            Interaction.MsgBox($"Free SysLog.{Constants.vbCrLf}{Constants.vbCrLf}Version {checkForUpdates.checkForUpdatesModule.versionString}{Constants.vbCrLf}{Constants.vbCrLf}Copyright Thomas Parkison © 2023-2025", MsgBoxStyle.Information, Text);
#endif
        }

        private void ExportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var SaveFileDialog = new SaveFileDialog())
            {
                SaveFileDialog.Title = "Safe Program Settings...";
                SaveFileDialog.Filter = "JSON File|*.json";

                if (SaveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        SaveAppSettings.SaveAppSettings.SaveApplicationSettingsToFile(SaveFileDialog.FileName);
                        if (Interaction.MsgBox("Application settings have been saved to disk. Do you want to open Windows Explorer to the location of the file?", (MsgBoxStyle)((int)MsgBoxStyle.Question + (int)MsgBoxStyle.YesNo + (int)MsgBoxStyle.DefaultButton2), Text) == MsgBoxResult.Yes)
                            SupportCode.SupportCode.SelectFileInWindowsExplorer(SaveFileDialog.FileName);
                    }
                    catch (Exception ex)
                    {
                        Interaction.MsgBox("There was an issue saving your exported settings to disk, export failed.", MsgBoxStyle.Critical, Text);
                    }
                }
            }
        }

        private void ImportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var OpenFileDialog = new OpenFileDialog())
            {
                OpenFileDialog.Title = "Import Program Settings...";
                OpenFileDialog.Filter = "JSON File|*.json";

                if (OpenFileDialog.ShowDialog() == DialogResult.OK && SaveAppSettings.SaveAppSettings.LoadApplicationSettingsFromFile(OpenFileDialog.FileName, Text))
                {
                    My.MySettingsProperty.Settings.Save();

                    System.Threading.Thread.Sleep(500);

                    Interaction.MsgBox("Free SysLog will now close and restart itself for the imported settings to take effect.", MsgBoxStyle.Information, Text);
                    Process.Start(SupportCode.SupportCode.strEXEPath);

                    try
                    {
                        SupportCode.SupportCode.mutex.ReleaseMutex();
                    }
                    catch (ApplicationException ex)
                    {
                    }

                    Process.GetCurrentProcess().Kill();
                }
            }
        }

        private void ClearLogsOlderThan(int daysToKeep)
        {
            try
            {
                lock (dataGridLockObject)
                {
                    Logs.AllowUserToOrderColumns = false;
                    Logs.Enabled = false;

                    int intOldCount = Logs.Rows.Count;
                    var newListOfLogs = new List<MyDataGridViewRow>();
                    var dateChosenDate = DateTime.Now.AddDays(daysToKeep * -1);

                    foreach (MyDataGridViewRow item in Logs.Rows)
                    {
                        if (item.DateObject.Date >= dateChosenDate)
                        {
                            newListOfLogs.Add((MyDataGridViewRow)item.Clone());
                        }
                    }

                    if (Interaction.MsgBox("Do you want to make a backup of the logs before deleting them?", (MsgBoxStyle)((int)MsgBoxStyle.Question + (int)MsgBoxStyle.YesNo + (int)Constants.vbDefaultButton2), Text) == MsgBoxResult.Yes)
                        MakeLogBackup();

                    Logs.Enabled = true;
                    Logs.AllowUserToOrderColumns = true;

                    Logs.SuspendLayout();
                    Logs.Rows.Clear();
                    Logs.Rows.AddRange(newListOfLogs.ToArray());

                    int intCountDifference = intOldCount - Logs.Rows.Count;
                    MyDataGridViewRow localMakeLocalDataGridRowEntry() { var argdataGrid11 = Logs; var ret = SyslogParser.SyslogParser.MakeLocalDataGridRowEntry($"The user deleted {intCountDifference:N0} log {(intCountDifference == 1 ? "entry" : "entries")}.", ref argdataGrid11); Logs = argdataGrid11; return ret; }

                    Logs.Rows.Add(localMakeLocalDataGridRowEntry());

                    Logs.ResumeLayout();

                    SelectLatestLogEntry();
                }

                UpdateLogCount();
                SaveLogsToDiskSub();
            }
            catch (ArgumentOutOfRangeException ex)
            {
                // Handle exception if necessary
            }
        }

        private void OlderThan1DayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ClearLogsOlderThan(1);
        }

        private void OlderThan2DaysToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ClearLogsOlderThan(2);
        }

        private void OlderThan3DaysToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ClearLogsOlderThan(3);
        }

        private void OlderThanAWeekToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ClearLogsOlderThan(7);
        }

        private void ChkConfirmCloseToolStripItem_Click(object sender, EventArgs e)
        {
            My.MySettingsProperty.Settings.boolConfirmClose = ChkEnableConfirmCloseToolStripItem.Checked;
        }

        private void LogsMenu_Opening(object sender, CancelEventArgs e)
        {
            if (Logs.SelectedRows.Count == 0)
            {
                DeleteLogsToolStripMenuItem.Visible = false;
                ExportsLogsToolStripMenuItem.Visible = false;
            }
            else
            {
                DeleteLogsToolStripMenuItem.Visible = true;
                ExportsLogsToolStripMenuItem.Visible = true;
            }

            if (Logs.SelectedRows.Count == 1)
            {
                CopyLogTextToolStripMenuItem.Visible = true;
                OpenLogViewerToolStripMenuItem.Visible = true;
                CreateAlertToolStripMenuItem.Visible = true;
                CreateReplacementToolStripMenuItem.Visible = true;
                CreateIgnoredLogToolStripMenuItem.Visible = true;
            }
            else
            {
                CopyLogTextToolStripMenuItem.Visible = false;
                OpenLogViewerToolStripMenuItem.Visible = false;
                CreateAlertToolStripMenuItem.Visible = false;
                CreateReplacementToolStripMenuItem.Visible = false;
                CreateIgnoredLogToolStripMenuItem.Visible = false;
            }

            DeleteLogsToolStripMenuItem.Text = Logs.SelectedRows.Count == 1 ? "Delete Selected Log" : "Delete Selected Logs";
            ExportsLogsToolStripMenuItem.Text = Logs.SelectedRows.Count == 1 ? "Export Selected Log" : "Export Selected Logs";
        }

        private void CopyLogTextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SupportCode.SupportCode.CopyTextToWindowsClipboard(Conversions.ToString(Logs.SelectedRows[0].Cells[SupportCode.SupportCode.ColumnIndex_LogText].Value), Text);
        }

        private void Logs_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                Logs.ClearSelection();
                Logs.Rows[e.RowIndex].Cells[e.ColumnIndex].Selected = true;
            }
        }

        private void Logs_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                int currentMouseOverRow = Logs.HitTest(e.X, e.Y).RowIndex;

                if (currentMouseOverRow >= 0)
                {
                    if (Logs.SelectedRows.Count <= 1)
                    {
                        Logs.ClearSelection();
                        Logs.Rows[currentMouseOverRow].Selected = true;
                    }
                }
            }

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

        private void OpenLogViewerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenLogViewerWindow();
        }

        private void DeleteLogsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            lock (dataGridLockObject)
            {
                int intNumberOfLogsDeleted = Logs.SelectedRows.Count;
                Confirm_Delete.UserChoice choice;

                using (var Confirm_Delete = new Confirm_Delete(intNumberOfLogsDeleted) { Icon = Icon, StartPosition = FormStartPosition.CenterParent })
                {
                    Confirm_Delete.lblMainLabel.Text = $"Are you sure you want to delete the {intNumberOfLogsDeleted} selected {(intNumberOfLogsDeleted == 1 ? "log" : "logs")}?";
                    Confirm_Delete.ShowDialog(this);
                    choice = Confirm_Delete.choice;
                }

                if (choice == Confirm_Delete.UserChoice.NoDelete)
                {
                    Interaction.MsgBox("Logs not deleted.", MsgBoxStyle.Information, Text);
                    return;
                }
                else if (choice == Confirm_Delete.UserChoice.YesDeleteYesBackup)
                {
                    MakeLogBackup();
                }

                foreach (DataGridViewRow item in Logs.SelectedRows)
                    Logs.Rows.Remove(item);

                MyDataGridViewRow localMakeLocalDataGridRowEntry() { var argdataGrid12 = Logs; var ret = SyslogParser.SyslogParser.MakeLocalDataGridRowEntry($"The user deleted {intNumberOfLogsDeleted:N0} log {(intNumberOfLogsDeleted == 1 ? "entry" : "entries")}.", ref argdataGrid12); Logs = argdataGrid12; return ret; }

                Logs.Rows.Add(localMakeLocalDataGridRowEntry());

                SelectLatestLogEntry();
            }

            UpdateLogCount();
            SaveLogsToDiskSub();
        }

        private void ExportAllLogsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataHandling.DataHandling.ExportAllLogs(Logs.Rows);
        }

        private void ExportsLogsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataHandling.DataHandling.ExportSelectedLogs(Logs.SelectedRows);
        }

        private void DonationStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start(strPayPal);
        }

        private void StopServerStripMenuItem_Click(object sender, EventArgs e)
        {
            if (StopServerStripMenuItem.Text == "Stop Server")
            {
                SupportCode.SupportCode.SendMessageToSysLogServer(SupportCode.SupportCode.strTerminate, My.MySettingsProperty.Settings.sysLogPort);
                if (My.MySettingsProperty.Settings.EnableTCPServer)
                    SupportCode.SupportCode.SendMessageToTCPSysLogServer(SupportCode.SupportCode.strTerminate, My.MySettingsProperty.Settings.sysLogPort);
                StopServerStripMenuItem.Text = "Start Server";
                boolServerRunning = false;
            }
            else if (StopServerStripMenuItem.Text == "Start Server" & SupportCode.SupportCode.boolDoWeOwnTheMutex)
            {
                boolServerRunning = true;
                serverThread = new System.Threading.Thread(SysLogThread) { Name = "UDP Server Thread", Priority = System.Threading.ThreadPriority.Normal };
                serverThread.Start();

                if (My.MySettingsProperty.Settings.EnableTCPServer)
                {
                    StartTCPServer();
                    boolTCPServerRunning = true;
                }

                lock (dataGridLockObject)
                {
                    MyDataGridViewRow localMakeLocalDataGridRowEntry() { var argdataGrid13 = Logs; var ret = SyslogParser.SyslogParser.MakeLocalDataGridRowEntry("Free SysLog Server Started.", ref argdataGrid13); Logs = argdataGrid13; return ret; }

                    Logs.Rows.Add(localMakeLocalDataGridRowEntry());
                    SelectLatestLogEntry();
                    UpdateLogCount();
                }

                StopServerStripMenuItem.Text = "Stop Server";
            }
        }

        private void ConfigureAlertsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var Alerts = new Alerts() { Icon = Icon, StartPosition = FormStartPosition.CenterParent })
            {
                Alerts.ShowDialog(this);
                if (Alerts.boolChanged)
                    regexCache.Clear();
            }
        }

        private void OpenWindowsExplorerToAppConfigFile_Click(object sender, EventArgs e)
        {
            Interaction.MsgBox("Modifying the application XML configuration file by hand may cause the program to malfunction. Caution is advised.", MsgBoxStyle.Information, Text);
            SupportCode.SupportCode.SelectFileInWindowsExplorer(ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal).FilePath);
        }

        private void CreateAlertToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var Alerts = new Alerts() { StartPosition = FormStartPosition.CenterParent, Icon = Icon })
            {
                Alerts.TxtLogText.Text = Conversions.ToString(Logs.SelectedRows[0].Cells[SupportCode.SupportCode.ColumnIndex_LogText].Value);
                Alerts.ShowDialog(this);
            }
        }

        private void ChangeSyslogServerPortToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (SupportCode.SupportCode.boolDoWeOwnTheMutex)
            {
                using (var IntegerInputForm = new IntegerInputForm(1, 65535) { Icon = Icon, Text = "Change Syslog Server Port", StartPosition = FormStartPosition.CenterParent })
                {
                    IntegerInputForm.lblSetting.Text = "Server Port";
                    IntegerInputForm.TxtSetting.Text = My.MySettingsProperty.Settings.sysLogPort.ToString();

                    IntegerInputForm.ShowDialog(this);

                    if (IntegerInputForm.DialogResult == DialogResult.OK)
                    {
                        if (IntegerInputForm.intResult < 1 | IntegerInputForm.intResult > 65535)
                        {
                            Interaction.MsgBox("The port number must be in the range of 1 - 65535.", MsgBoxStyle.Critical, Text);
                        }
                        else
                        {
                            if (SupportCode.SupportCode.boolDoWeOwnTheMutex)
                            {
                                SupportCode.SupportCode.SendMessageToSysLogServer(SupportCode.SupportCode.strTerminate, My.MySettingsProperty.Settings.sysLogPort);
                                if (My.MySettingsProperty.Settings.EnableTCPServer)
                                    SupportCode.SupportCode.SendMessageToTCPSysLogServer(SupportCode.SupportCode.strTerminate, My.MySettingsProperty.Settings.sysLogPort);
                            }

                            ChangeSyslogServerPortToolStripMenuItem.Text = $"Change Syslog Server Port (Port Number {IntegerInputForm.intResult})";

                            My.MySettingsProperty.Settings.sysLogPort = IntegerInputForm.intResult;
                            My.MySettingsProperty.Settings.Save();

                            if (serverThread.IsAlive)
                                serverThread.Abort();

                            serverThread = new System.Threading.Thread(SysLogThread) { Name = "UDP Server Thread", Priority = System.Threading.ThreadPriority.Normal };
                            serverThread.Start();

                            lock (dataGridLockObject)
                            {
                                MyDataGridViewRow localMakeLocalDataGridRowEntry() { var argdataGrid14 = Logs; var ret = SyslogParser.SyslogParser.MakeLocalDataGridRowEntry("Free SysLog Server Started.", ref argdataGrid14); Logs = argdataGrid14; return ret; }

                                Logs.Rows.Add(localMakeLocalDataGridRowEntry());
                                SelectLatestLogEntry();
                                UpdateLogCount();
                            }

                            Interaction.MsgBox("Done.", MsgBoxStyle.Information, Text);
                        }
                    }
                }
            }
        }

        private void ChangeAutosaveIntervalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var IntegerInputForm = new IntegerInputForm(1, 20) { Icon = Icon, Text = "Change Log Autosave Interval", StartPosition = FormStartPosition.CenterParent })
            {
                IntegerInputForm.lblSetting.Text = "Auto Save (In Minutes)";
                IntegerInputForm.TxtSetting.Text = My.MySettingsProperty.Settings.autoSaveMinutes.ToString();

                IntegerInputForm.ShowDialog(this);

                if (IntegerInputForm.DialogResult == DialogResult.OK)
                {
                    ChangeLogAutosaveIntervalToolStripMenuItem.Text = $"Change Log Autosave Interval ({IntegerInputForm.intResult} Minutes)";
                    SaveTimer.Interval = (int)Math.Round(TimeSpan.FromMinutes(IntegerInputForm.intResult).TotalMilliseconds);
                    My.MySettingsProperty.Settings.autoSaveMinutes = (short)IntegerInputForm.intResult;

                    Interaction.MsgBox("Done.", MsgBoxStyle.Information, Text);
                }
            }
        }

        private void CreateIgnoredLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var Ignored = new IgnoredWordsAndPhrases() { StartPosition = FormStartPosition.CenterParent, Icon = Icon })
            {
                Ignored.TxtIgnored.Text = Conversions.ToString(Logs.SelectedRows[0].Cells[SupportCode.SupportCode.ColumnIndex_LogText].Value);
                Ignored.ShowDialog(this);
            }
        }

        private void CreateReplacementToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var Replacements = new Replacements() { StartPosition = FormStartPosition.CenterParent, Icon = Icon })
            {
                Replacements.TxtReplace.Text = Conversions.ToString(Logs.SelectedRows[0].Cells[SupportCode.SupportCode.ColumnIndex_LogText].Value);
                Replacements.ShowDialog(this);
            }
        }

        private void Logs_SelectionChanged(object sender, EventArgs e)
        {
            LblItemsSelected.Visible = Logs.SelectedRows.Count > 1;
            LblItemsSelected.Text = $"Selected Logs: {Logs.SelectedRows.Count:N0}";
        }

        private void ChkDeselectItemAfterMinimizingWindow_Click(object sender, EventArgs e)
        {
            My.MySettingsProperty.Settings.boolDeselectItemsWhenMinimizing = ChkDeselectItemAfterMinimizingWindow.Checked;
        }

        private void ConfigureSysLogMirrorServers_Click(object sender, EventArgs e)
        {
            using (var ConfigureSysLogMirrorClients = new ConfigureSysLogMirrorClients() { StartPosition = FormStartPosition.CenterParent, Icon = Icon })
            {
                ConfigureSysLogMirrorClients.ShowDialog(this);
                if (ConfigureSysLogMirrorClients.boolSuccess)
                    Interaction.MsgBox("Done", MsgBoxStyle.Information, Text);
            }
        }

        private void ChkShowAlertedColumn_Click(object sender, EventArgs e)
        {
            My.MySettingsProperty.Settings.boolShowAlertedColumn = ChkShowAlertedColumn.Checked;
            ColAlerts.Visible = ChkShowAlertedColumn.Checked;
        }

        private void MinimizeToClockTray_Click(object sender, EventArgs e)
        {
            My.MySettingsProperty.Settings.MinimizeToClockTray = MinimizeToClockTray.Checked;
        }

        private void BtnOpenLogForViewing_Click(object sender, EventArgs e)
        {
            using (var OpenFileDialog = new OpenFileDialog() { Title = "Open Log File", Filter = "JSON File|*.json" })
            {
                if (OpenFileDialog.ShowDialog() == DialogResult.OK)
                {
                    var logFileViewer = new IgnoredLogsAndSearchResults(this, IgnoreOrSearchWindowDisplayMode.viewer) { MainProgramForm = this, Icon = Icon, Text = "Log File Viewer", strFileToLoad = OpenFileDialog.FileName, boolLoadExternalData = true };
                    logFileViewer.ChkColLogsAutoFill.Checked = My.MySettingsProperty.Settings.colLogAutoFill;
                    logFileViewer.Show(this);
                }
            }
        }

        private void AutomaticallyCheckForUpdates_Click(object sender, EventArgs e)
        {
            My.MySettingsProperty.Settings.boolCheckForUpdates = AutomaticallyCheckForUpdates.Checked;
        }

        private void BackupFileNameDateFormatChooser_Click(object sender, EventArgs e)
        {
            var DateFormatChooser = new DateFormatChooser() { Icon = Icon, StartPosition = FormStartPosition.CenterParent };
            DateFormatChooser.Show(this);
        }

        private void ViewLogBackups_Click(object sender, EventArgs e)
        {
            var collectionOfSavedData = new List<SavedData>();

            lock (dataGridLockObject)
            {
                MyDataGridViewRow myItem;

                foreach (DataGridViewRow item in Logs.Rows)
                {
                    if (!string.IsNullOrWhiteSpace(Conversions.ToString(item.Cells[SupportCode.SupportCode.ColumnIndex_ComputedTime].Value)))
                    {
                        myItem = (MyDataGridViewRow)item;

                        collectionOfSavedData.Add(new SavedData()
                        {
                            time = Conversions.ToString(myItem.Cells[SupportCode.SupportCode.ColumnIndex_ComputedTime].Value),
                            ip = Conversions.ToString(myItem.Cells[SupportCode.SupportCode.ColumnIndex_LogType].Value),
                            log = Conversions.ToString(myItem.Cells[SupportCode.SupportCode.ColumnIndex_IPAddress].Value),
                            DateObject = myItem.DateObject,
                            BoolAlerted = myItem.BoolAlerted
                        });
                    }
                }
            }

            var ViewLogBackups = new ViewLogBackups() { Icon = Icon, MyParentForm = this, currentLogs = collectionOfSavedData };
            ViewLogBackups.Show(this);
        }

        private void CloseMe_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void StartUpDelay_Click(object sender, EventArgs e)
        {
            double dblSeconds = 0;

            using (var taskService = new Microsoft.Win32.TaskScheduler.TaskService())
            {
                Microsoft.Win32.TaskScheduler.Task task = null;

                var argtaskServiceObject = taskService;
                if (TaskHandling.TaskHandling.GetTaskObject(ref argtaskServiceObject, $"Free SysLog for {Environment.UserName}", ref task))
                {
                    if (task.Definition.Triggers.Any())
                    {
                        var trigger = task.Definition.Triggers[0];
                        if (trigger.TriggerType == Microsoft.Win32.TaskScheduler.TaskTriggerType.Logon)
                            dblSeconds = ((Microsoft.Win32.TaskScheduler.LogonTrigger)trigger).Delay.TotalSeconds;
                    }
                }
            }

            using (var IntegerInputForm = new IntegerInputForm(1, 300) { Icon = Icon, Text = "Change Startup Delay", StartPosition = FormStartPosition.CenterParent })
            {
                IntegerInputForm.lblSetting.Text = "Time in Seconds";
                IntegerInputForm.TxtSetting.Text = dblSeconds.ToString();

                IntegerInputForm.ShowDialog(this);

                if (IntegerInputForm.DialogResult == DialogResult.OK)
                {
                    if (IntegerInputForm.intResult < 1 | IntegerInputForm.intResult > 300)
                    {
                        Interaction.MsgBox("The time in seconds must be in the range of 1 - 300.", MsgBoxStyle.Critical, Text);
                    }
                    else
                    {
                        using (var taskService = new Microsoft.Win32.TaskScheduler.TaskService())
                        {
                            Microsoft.Win32.TaskScheduler.Task task = null;

                            var argtaskServiceObject1 = taskService;
                            if (TaskHandling.TaskHandling.GetTaskObject(ref argtaskServiceObject1, $"Free SysLog for {Environment.UserName}", ref task))
                            {
                                if (task.Definition.Triggers.Any())
                                {
                                    var trigger = task.Definition.Triggers[0];

                                    if (trigger.TriggerType == Microsoft.Win32.TaskScheduler.TaskTriggerType.Logon)
                                    {
                                        ((Microsoft.Win32.TaskScheduler.LogonTrigger)trigger).Delay = new TimeSpan(0, 0, IntegerInputForm.intResult);
                                        task.RegisterChanges();
                                    }
                                }
                            }
                        }

                        StartUpDelay.Text = $"        Startup Delay ({IntegerInputForm.intResult} {(IntegerInputForm.intResult == 1 ? "Second" : "Seconds")})";
                        Interaction.MsgBox("Done.", MsgBoxStyle.Information, Text);
                    }
                }
                else if (IntegerInputForm.DialogResult == DialogResult.Cancel)
                {
                    using (var taskService = new Microsoft.Win32.TaskScheduler.TaskService())
                    {
                        Microsoft.Win32.TaskScheduler.Task task = null;

                        var argtaskServiceObject2 = taskService;
                        if (TaskHandling.TaskHandling.GetTaskObject(ref argtaskServiceObject2, $"Free SysLog for {Environment.UserName}", ref task))
                        {
                            if (task.Definition.Triggers.Any())
                            {
                                var trigger = task.Definition.Triggers[0];

                                if (trigger.TriggerType == Microsoft.Win32.TaskScheduler.TaskTriggerType.Logon)
                                {
                                    ((Microsoft.Win32.TaskScheduler.LogonTrigger)trigger).Delay = default;
                                    task.RegisterChanges();
                                }
                            }
                        }
                    }

                    StartUpDelay.Checked = false;
                    StartUpDelay.Text = $"        Startup Delay";
                    Interaction.MsgBox("Done.", MsgBoxStyle.Information, Text);
                }
            }
        }

        private void ChkEnableTCPSyslogServer_Click(object sender, EventArgs e)
        {
            My.MySettingsProperty.Settings.EnableTCPServer = ChkEnableTCPSyslogServer.Checked;

            if (ChkEnableTCPSyslogServer.Checked)
            {
                StartTCPServer();
            }
            else
            {
                SupportCode.SupportCode.SendMessageToTCPSysLogServer(SupportCode.SupportCode.strTerminate, My.MySettingsProperty.Settings.sysLogPort);
            }
        }

        private void ChkShowHostnameColumn_Click(object sender, EventArgs e)
        {
            My.MySettingsProperty.Settings.boolShowHostnameColumn = ChkShowHostnameColumn.Checked;
            ColHostname.Visible = My.MySettingsProperty.Settings.boolShowHostnameColumn;
        }

        private void ChkShowLogTypeColumn_Click(object sender, EventArgs e)
        {
            My.MySettingsProperty.Settings.boolShowLogTypeColumn = ChkShowLogTypeColumn.Checked;
            colLogType.Visible = My.MySettingsProperty.Settings.boolShowLogTypeColumn;
        }

        private void ChkShowServerTimeColumn_Click(object sender, EventArgs e)
        {
            My.MySettingsProperty.Settings.boolShowServerTimeColumn = ChkShowServerTimeColumn.Checked;
            colServerTime.Visible = My.MySettingsProperty.Settings.boolShowServerTimeColumn;
        }

        private void RemoveNumbersFromRemoteApp_Click(object sender, EventArgs e)
        {
            My.MySettingsProperty.Settings.RemoveNumbersFromRemoteApp = RemoveNumbersFromRemoteApp.Checked;
        }

        private void IPv6Support_Click(object sender, EventArgs e)
        {
            My.MySettingsProperty.Settings.IPv6Support = IPv6Support.Checked;
            My.MySettingsProperty.Settings.Save();

            if (SupportCode.SupportCode.boolDoWeOwnTheMutex && boolServerRunning && Interaction.MsgBox("Changing this setting will require a reset of the Syslog Client. Do you want to restart the Syslog Client now?", (MsgBoxStyle)((int)MsgBoxStyle.YesNo + (int)MsgBoxStyle.Question), Text) == MsgBoxResult.Yes)
            {
                System.Threading.ThreadPool.QueueUserWorkItem((a) =>
                    {
                        SupportCode.SupportCode.SendMessageToSysLogServer(SupportCode.SupportCode.strTerminate, My.MySettingsProperty.Settings.sysLogPort);
                        if (boolTCPServerRunning)
                            SupportCode.SupportCode.SendMessageToTCPSysLogServer(SupportCode.SupportCode.strTerminate, My.MySettingsProperty.Settings.sysLogPort);

                        System.Threading.Thread.Sleep(5000);

                        serverThread = new System.Threading.Thread(SysLogThread) { Name = "UDP Server Thread", Priority = System.Threading.ThreadPriority.Normal };
                        serverThread.Start();

                        if (My.MySettingsProperty.Settings.EnableTCPServer)
                            StartTCPServer();
                    });
            }
        }

        private void ShowRawLogOnLogViewer_Click(object sender, EventArgs e)
        {
            My.MySettingsProperty.Settings.boolShowRawLogOnLogViewer = ShowRawLogOnLogViewer.Checked;
        }

        private void LblLogFileSize_Click(object sender, EventArgs e)
        {
            SupportCode.SupportCode.SelectFileInWindowsExplorer(SupportCode.SupportCode.strPathToDataFile);
        }

        private void ConfigureHostnames_Click(object sender, EventArgs e)
        {
            using (var hostnames = new Hostnames() { Icon = Icon })
            {
                hostnames.ShowDialog();
            }
        }

        private void ChangeFont_Click(object sender, EventArgs e)
        {
            using (var FontDialog = new FontDialog())
            {
                if (My.MySettingsProperty.Settings.font is not null)
                    FontDialog.Font = My.MySettingsProperty.Settings.font;

                if (FontDialog.ShowDialog() == DialogResult.OK)
                {
                    My.MySettingsProperty.Settings.font = FontDialog.Font;

                    Logs.DefaultCellStyle.Font = My.MySettingsProperty.Settings.font;
                    Logs.ColumnHeadersDefaultCellStyle.Font = My.MySettingsProperty.Settings.font;

                    DataHandling.DataHandling.WriteLogsToDisk();

                    lock (dataGridLockObject)
                        System.Threading.Tasks.Task.Run(() => LoadDataFile(false));
                }
            }
        }

        private void ConfigureTimeBetweenSameNotifications_Click(object sender, EventArgs e)
        {
            using (var IntegerInputForm = new IntegerInputForm(30, 240) { Icon = Icon, Text = "Configure Time Between Same Notifications", StartPosition = FormStartPosition.CenterParent })
            {
                IntegerInputForm.lblSetting.Text = "Time Between Same Notifications (In Seconds)";
                IntegerInputForm.TxtSetting.Text = My.MySettingsProperty.Settings.TimeBetweenSameNotifications.ToString();
                IntegerInputForm.Width += 60;

                IntegerInputForm.ShowDialog(this);

                if (IntegerInputForm.DialogResult == DialogResult.OK)
                {
                    if (IntegerInputForm.intResult < 30 | IntegerInputForm.intResult > 240)
                    {
                        Interaction.MsgBox("The time in seconds must be in the range of 30 - 240.", MsgBoxStyle.Critical, Text);
                    }
                    else
                    {
                        ConfigureTimeBetweenSameNotifications.Text = $"Configure Time Between Same Notifications ({IntegerInputForm.intResult} Seconds)";

                        My.MySettingsProperty.Settings.TimeBetweenSameNotifications = IntegerInputForm.intResult;
                        My.MySettingsProperty.Settings.Save();

                        Interaction.MsgBox("Done.", MsgBoxStyle.Information, Text);
                    }
                }
            }
        }

        private void IncludeButtonsOnNotifications_Click(object sender, EventArgs e)
        {
            My.MySettingsProperty.Settings.IncludeButtonsOnNotifications = IncludeButtonsOnNotifications.Checked;
        }

        private void NotificationLengthShort_Click(object sender, EventArgs e)
        {
            NotificationLengthLong.Checked = false;
            My.MySettingsProperty.Settings.NotificationLength = 0;
        }

        private void NotificationLengthLong_Click(object sender, EventArgs e)
        {
            NotificationLengthShort.Checked = false;
            My.MySettingsProperty.Settings.NotificationLength = 1;
        }

        private void AlertsHistory_Click(object sender, EventArgs e)
        {
            if (Logs.Rows.Count > 0)
            {
                var DataToLoad = new List<AlertsHistory>();

                lock (dataGridLockObject)
                {
                    foreach (MyDataGridViewRow item in Logs.Rows)
                    {
                        if (item.BoolAlerted)
                        {
                            DataToLoad.Add(new AlertsHistory()
                            {
                                strTime = Conversions.ToString(item.Cells[SupportCode.SupportCode.ColumnIndex_ComputedTime].Value),
                                alertType = item.alertType,
                                strAlertText = item.AlertText,
                                strIP = Conversions.ToString(item.Cells[SupportCode.SupportCode.ColumnIndex_IPAddress].Value),
                                strLog = Conversions.ToString(item.Cells[SupportCode.SupportCode.ColumnIndex_LogText].Value),
                                strRawLog = item.RawLogData
                            });
                        }
                    }
                }

                if (DataToLoad.Count == 0)
                {
                    Interaction.MsgBox("There are no alerts to show in the Alerts History.", MsgBoxStyle.Information, Text);
                }
                else
                {
                    using (var Alerts_History = new Alerts_History() { Icon = Icon, DataToLoad = DataToLoad, StartPosition = FormStartPosition.CenterParent, SetParentForm = this })
                    {
                        Alerts_History.ShowDialog(this);
                    }
                }
            }
        }

        private void ColLogsAutoFill_Click(object sender, EventArgs e)
        {
            My.MySettingsProperty.Settings.colLogAutoFill = ColLogsAutoFill.Checked;
            ColLog.AutoSizeMode = My.MySettingsProperty.Settings.colLogAutoFill ? DataGridViewAutoSizeColumnMode.Fill : DataGridViewAutoSizeColumnMode.NotSet;
        }

        private void Logs_Scroll(object sender, ScrollEventArgs e)
        {
            if (ChkDisableAutoScrollUponScrolling.Checked && !SupportCode.SupportCode.boolIsProgrammaticScroll && ChkEnableAutoScroll.Checked && e.NewValue < e.OldValue)
            {
                My.MySettingsProperty.Settings.autoScroll = false;
                ChkEnableAutoScroll.Checked = false;
            }
        }

        private void ChkDisableAutoScrollUponScrolling_Click(object sender, EventArgs e)
        {
            My.MySettingsProperty.Settings.disableAutoScrollUponScrolling = ChkDisableAutoScrollUponScrolling.Checked;
        }

        private void ClearNotificationLimits_Click(object sender, EventArgs e)
        {
            if (NotificationLimiter.NotificationLimiterModule.lastNotificationTime is not null && NotificationLimiter.NotificationLimiterModule.lastNotificationTime.Any())
            {
                NotificationLimiter.NotificationLimiterModule.lastNotificationTime.Clear();
                Interaction.MsgBox("Done.", MsgBoxStyle.Information, Text);
            }
        }

        private void ReOpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RestoreWindow();
        }

        private void ChkDebug_Click(object sender, EventArgs e)
        {
            My.MySettingsProperty.Settings.boolDebug = ChkDebug.Checked;
        }

        private void ConfirmDelete_Click(object sender, EventArgs e)
        {
            My.MySettingsProperty.Settings.ConfirmDelete = ConfirmDelete.Checked;
        }

        #region -- SysLog Server Code --
        public void SysLogThread()
        {
            try
            {
                // These are initialized as IPv4 mode.
                var addressFamilySetting = AddressFamily.InterNetwork;
                var ipAddressSetting = IPAddress.Any;

                if (My.MySettingsProperty.Settings.IPv6Support)
                {
                    addressFamilySetting = AddressFamily.InterNetworkV6;
                    ipAddressSetting = IPAddress.IPv6Any;
                }

                using (var socket = new Socket(addressFamilySetting, SocketType.Dgram, ProtocolType.Udp))
                {
                    if (My.MySettingsProperty.Settings.IPv6Support)
                        socket.DualMode = true;
                    socket.Bind(new IPEndPoint(ipAddressSetting, My.MySettingsProperty.Settings.sysLogPort));

                    bool boolDoServerLoop = true;
                    var buffer = new byte[4096];
                    EndPoint remoteEndPoint = new IPEndPoint(ipAddressSetting, 0);
                    ProxiedSysLogData ProxiedSysLogData;

                    while (boolDoServerLoop)
                    {
                        int bytesReceived = socket.ReceiveFrom(buffer, ref remoteEndPoint);
                        string strReceivedData = Encoding.UTF8.GetString(buffer, 0, bytesReceived);
                        string strSourceIP = SupportCode.SupportCode.GetIPv4Address(((IPEndPoint)remoteEndPoint).Address).ToString();

                        if (strReceivedData.Trim().Equals(SupportCode.SupportCode.strRestore, StringComparison.OrdinalIgnoreCase))
                        {
                            Invoke(new Action(RestoreWindowAfterReceivingRestoreCommand));
                        }
                        else if (strReceivedData.Trim().Equals(SupportCode.SupportCode.strTerminate, StringComparison.OrdinalIgnoreCase))
                        {
                            boolDoServerLoop = false;
                        }
                        else if (strReceivedData.Trim().StartsWith(SupportCode.SupportCode.strProxiedString, StringComparison.OrdinalIgnoreCase))
                        {
                            try
                            {
                                strReceivedData = strReceivedData.Replace(SupportCode.SupportCode.strProxiedString, "", StringComparison.OrdinalIgnoreCase);
                                ProxiedSysLogData = Newtonsoft.Json.JsonConvert.DeserializeObject<ProxiedSysLogData>(strReceivedData, SupportCode.SupportCode.JSONDecoderSettingsForLogFiles);
                                SyslogParser.SyslogParser.ProcessIncomingLog(ProxiedSysLogData.log, ProxiedSysLogData.ip);
                            }
                            catch (Newtonsoft.Json.JsonSerializationException ex)
                            {
                            }
                        }
                        else
                        {
                            if (SupportCode.SupportCode.serversList is not null && SupportCode.SupportCode.serversList.Any())
                            {
                                System.Threading.ThreadPool.QueueUserWorkItem((a) =>
                                    {
                                        ProxiedSysLogData = new ProxiedSysLogData() { ip = strSourceIP, log = strReceivedData };
                                        string strDataToSend = SupportCode.SupportCode.strProxiedString + Newtonsoft.Json.JsonConvert.SerializeObject(ProxiedSysLogData);

                                        foreach (SysLogProxyServer item in SupportCode.SupportCode.serversList)
                                            SupportCode.SupportCode.SendMessageToSysLogServer(strDataToSend, item.ip, item.port);

                                        ProxiedSysLogData = null;
                                        strDataToSend = null;
                                    });
                            }

                            SyslogParser.SyslogParser.ProcessIncomingLog(strReceivedData, strSourceIP);
                        }

                        strReceivedData = null;
                        strSourceIP = null;
                    }
                }
            }
            catch (System.Threading.ThreadAbortException ex)
            {
            }
            // Does nothing
            catch (Exception e)
            {
                Invoke(new Action(() =>
                    {
                        lock (dataGridLockObject)
                        {
                            MyDataGridViewRow localMakeLocalDataGridRowEntry() { var argdataGrid15 = Logs; var ret = SyslogParser.SyslogParser.MakeLocalDataGridRowEntry($"Exception Type: {e.GetType()}{Constants.vbCrLf}Exception Message: {e.Message}{Constants.vbCrLf}{Constants.vbCrLf}Exception Stack Trace{Constants.vbCrLf}{e.StackTrace}", ref argdataGrid15); Logs = argdataGrid15; return ret; }

                            Logs.Rows.Add(localMakeLocalDataGridRowEntry());
                            SelectLatestLogEntry();
                            UpdateLogCount();
                        }

                        Interaction.MsgBox("Unable to start syslog server, perhaps another instance of this program is running on your system.", (MsgBoxStyle)((int)MsgBoxStyle.Critical + (int)MsgBoxStyle.ApplicationModal), Text);
                    }));
            }
        }

        private async void RestoreWindowAfterReceivingRestoreCommand()
        {
            if (boolMaximizedBeforeMinimize)
            {
                WindowState = FormWindowState.Maximized;
            }
            else if (My.MySettingsProperty.Settings.boolMaximized)
            {
                WindowState = FormWindowState.Maximized;
            }
            else
            {
                WindowState = FormWindowState.Normal;
            }

            await System.Threading.Tasks.Task.Delay(100);

            if (SupportCode.SupportCode.boolDebugBuild | ChkDebug.Checked)
            {
                MyDataGridViewRow localMakeLocalDataGridRowEntry() { var argdataGrid16 = Logs; var ret = SyslogParser.SyslogParser.MakeLocalDataGridRowEntry("Restore command received.", ref argdataGrid16); Logs = argdataGrid16; return ret; }

                Logs.Rows.Add(localMakeLocalDataGridRowEntry());
                SelectLatestLogEntry();
                UpdateLogCount();
                BtnSaveLogsToDisk.Enabled = true;
            }

            SelectLatestLogEntry();
        }
        #endregion
    }
}