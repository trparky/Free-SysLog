using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Free_SysLog
{

    [Microsoft.VisualBasic.CompilerServices.DesignerGenerated()]
    public partial class Form1 : Form
    {

        // Form overrides dispose to clean up the component list.
        [DebuggerNonUserCode()]
        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing && components is not null)
                {
                    components.Dispose();
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        // Required by the Windows Form Designer
        private System.ComponentModel.IContainer components;

        // NOTE: The following procedure is required by the Windows Form Designer
        // It can be modified using the Windows Form Designer.  
        // Do not modify it using the code editor.
        [DebuggerStepThrough()]
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            BtnOpenLogLocation = new ToolStripMenuItem();
            BtnOpenLogLocation.Click += new EventHandler(BtnOpenLogLocation_Click);
            BtnOpenLogForViewing = new ToolStripMenuItem();
            BtnOpenLogForViewing.Click += new EventHandler(BtnOpenLogForViewing_Click);
            BtnClearLog = new ToolStripMenuItem();
            AlertsHistory = new ToolStripMenuItem();
            AlertsHistory.Click += new EventHandler(AlertsHistory_Click);
            BtnClearAllLogs = new ToolStripMenuItem();
            BtnClearAllLogs.Click += new EventHandler(BtnClearAllLogs_Click);
            LogsOlderThanToolStripMenuItem = new ToolStripMenuItem();
            LogsOlderThanToolStripMenuItem.Click += new EventHandler(LogsOlderThanToolStripMenuItem_Click);
            BtnSaveLogsToDisk = new ToolStripMenuItem();
            BtnSaveLogsToDisk.Click += new EventHandler(BtnSaveLogsToDisk_Click);
            ExportAllLogsToolStripMenuItem = new ToolStripMenuItem();
            ExportAllLogsToolStripMenuItem.Click += new EventHandler(ExportAllLogsToolStripMenuItem_Click);
            StatusStrip = new StatusStrip();
            NumberOfLogs = new ToolStripStatusLabel();
            LblAutoSaved = new ToolStripStatusLabel();
            LblItemsSelected = new ToolStripStatusLabel();
            LblLogFileSize = new ToolStripStatusLabel();
            LblLogFileSize.Click += new EventHandler(LblLogFileSize_Click);
            LblNumberOfIgnoredIncomingLogs = new ToolStripStatusLabel();
            ChkEnableAutoScroll = new ToolStripMenuItem();
            ChkEnableAutoScroll.Click += new EventHandler(ChkAutoScroll_Click);
            ChkDisableAutoScrollUponScrolling = new ToolStripMenuItem();
            ChkDisableAutoScrollUponScrolling.Click += new EventHandler(ChkDisableAutoScrollUponScrolling_Click);
            AutomaticallyCheckForUpdates = new ToolStripMenuItem();
            AutomaticallyCheckForUpdates.Click += new EventHandler(AutomaticallyCheckForUpdates_Click);
            BtnCheckForUpdates = new ToolStripMenuItem();
            BtnCheckForUpdates.Click += new EventHandler(BtnCheckForUpdates_Click);
            SaveTimer = new Timer(components);
            SaveTimer.Tick += new EventHandler(SaveTimer_Tick);
            ChkEnableAutoSave = new ToolStripMenuItem();
            ChkEnableAutoSave.Click += new EventHandler(ChkAutoSave_Click);
            DeleteOldLogsAtMidnight = new ToolStripMenuItem();
            DeleteOldLogsAtMidnight.Click += new EventHandler(DeleteOldLogsAtMidnight_Click);
            BackupOldLogsAfterClearingAtMidnight = new ToolStripMenuItem();
            BackupOldLogsAfterClearingAtMidnight.Click += new EventHandler(BackupOldLogsAfterClearingAtMidnight_Click);
            ChkEnableStartAtUserStartup = new ToolStripMenuItem();
            ChkEnableStartAtUserStartup.Click += new EventHandler(ChkStartAtUserStartup_Click);
            ChkEnableTCPSyslogServer = new ToolStripMenuItem();
            ChkEnableTCPSyslogServer.Click += new EventHandler(ChkEnableTCPSyslogServer_Click);
            StartUpDelay = new ToolStripMenuItem();
            StartUpDelay.Click += new EventHandler(StartUpDelay_Click);
            ToolTip = new ToolTip(components);
            ChkRegExSearch = new CheckBox();
            MenuStrip = new MenuStrip();
            MainMenuToolStripMenuItem = new ToolStripMenuItem();
            LogFunctionsToolStripMenuItem = new ToolStripMenuItem();
            LogFunctionsToolStripMenuItem.DropDownOpening += new EventHandler(LogFunctionsToolStripMenuItem_DropDownOpening);
            IgnoredLogsToolStripMenuItem = new ToolStripMenuItem();
            ClearIgnoredLogsToolStripMenuItem = new ToolStripMenuItem();
            ClearIgnoredLogsToolStripMenuItem.Click += new EventHandler(ClearIgnoredLogsToolStripMenuItem_Click);
            ViewIgnoredLogsToolStripMenuItem = new ToolStripMenuItem();
            ViewIgnoredLogsToolStripMenuItem.Click += new EventHandler(ViewIgnoredLogsToolStripMenuItem_Click);
            ZerooutIgnoredLogsCounterToolStripMenuItem = new ToolStripMenuItem();
            ZerooutIgnoredLogsCounterToolStripMenuItem.Click += new EventHandler(ZerooutIgnoredLogsCounterToolStripMenuItem_Click);
            ViewLogBackups = new ToolStripMenuItem();
            ViewLogBackups.Click += new EventHandler(ViewLogBackups_Click);
            SettingsToolStripMenuItem = new ToolStripMenuItem();
            ConfigureReplacementsToolStripMenuItem = new ToolStripMenuItem();
            ConfigureReplacementsToolStripMenuItem.Click += new EventHandler(ConfigureReplacementsToolStripMenuItem_Click);
            ConfigureIgnoredWordsAndPhrasesToolStripMenuItem = new ToolStripMenuItem();
            ConfigureIgnoredWordsAndPhrasesToolStripMenuItem.Click += new EventHandler(IgnoredWordsAndPhrasesToolStripMenuItem_Click);
            ChkEnableRecordingOfIgnoredLogs = new ToolStripMenuItem();
            ChkEnableRecordingOfIgnoredLogs.Click += new EventHandler(ChkRecordIgnoredLogs_Click);
            LblSearchLabel = new Label();
            TxtSearchTerms = new TextBox();
            TxtSearchTerms.KeyDown += new KeyEventHandler(TxtSearchTerms_KeyDown);
            BtnSearch = new Button();
            BtnSearch.Click += new EventHandler(BtnSearch_Click);
            ChkCaseInsensitiveSearch = new CheckBox();
            Logs = new DataGridView();
            Logs.DoubleClick += new EventHandler(Logs_DoubleClick);
            Logs.KeyUp += new KeyEventHandler(Logs_KeyUp);
            Logs.KeyDown += new KeyEventHandler(Logs_KeyDown);
            Logs.UserDeletingRow += new DataGridViewRowCancelEventHandler(Logs_UserDeletingRow);
            Logs.ColumnWidthChanged += new DataGridViewColumnEventHandler(Logs_ColumnWidthChanged);
            Logs.ColumnHeaderMouseClick += new DataGridViewCellMouseEventHandler(Logs_ColumnHeaderMouseClick);
            Logs.CellMouseClick += new DataGridViewCellMouseEventHandler(Logs_CellMouseClick);
            Logs.MouseDown += new MouseEventHandler(Logs_MouseDown);
            Logs.MouseUp += new MouseEventHandler(Logs_MouseUp);
            Logs.SelectionChanged += new EventHandler(Logs_SelectionChanged);
            Logs.Scroll += new ScrollEventHandler(Logs_Scroll);
            ColTime = new DataGridViewTextBoxColumn();
            colServerTime = new DataGridViewTextBoxColumn();
            ColIPAddress = new DataGridViewTextBoxColumn();
            colLogType = new DataGridViewTextBoxColumn();
            ColLog = new DataGridViewTextBoxColumn();
            ColAlerts = new DataGridViewTextBoxColumn();
            ColRemoteProcess = new DataGridViewTextBoxColumn();
            ColHostname = new DataGridViewTextBoxColumn();
            NotifyIcon = new NotifyIcon(components);
            NotifyIcon.DoubleClick += new EventHandler(NotifyIcon_DoubleClick);
            ChkEnableConfirmCloseToolStripItem = new ToolStripMenuItem();
            ChkEnableConfirmCloseToolStripItem.Click += new EventHandler(ChkConfirmCloseToolStripItem_Click);
            ChangeAlternatingColorToolStripMenuItem = new ToolStripMenuItem();
            ChangeAlternatingColorToolStripMenuItem.Click += new EventHandler(ConfigureAlternatingColorToolStripMenuItem_Click);
            ChangeFont = new ToolStripMenuItem();
            ChangeFont.Click += new EventHandler(ChangeFont_Click);
            ConfigureAlertsToolStripMenuItem = new ToolStripMenuItem();
            ConfigureAlertsToolStripMenuItem.Click += new EventHandler(ConfigureAlertsToolStripMenuItem_Click);
            ConfigureHostnames = new ToolStripMenuItem();
            ConfigureHostnames.Click += new EventHandler(ConfigureHostnames_Click);
            ColumnControls = new ToolStripMenuItem();
            ClearNotificationLimits = new ToolStripMenuItem();
            ClearNotificationLimits.Click += new EventHandler(ClearNotificationLimits_Click);
            ColLogsAutoFill = new ToolStripMenuItem();
            ColLogsAutoFill.Click += new EventHandler(ColLogsAutoFill_Click);
            LogsMenu = new ContextMenuStrip(components);
            LogsMenu.Opening += new System.ComponentModel.CancelEventHandler(LogsMenu_Opening);
            CopyLogTextToolStripMenuItem = new ToolStripMenuItem();
            CopyLogTextToolStripMenuItem.Click += new EventHandler(CopyLogTextToolStripMenuItem_Click);
            AboutToolStripMenuItem = new ToolStripMenuItem();
            AboutToolStripMenuItem.Click += new EventHandler(AboutToolStripMenuItem_Click);
            CloseMe = new ToolStripMenuItem();
            CloseMe.Click += new EventHandler(CloseMe_Click);
            ToolStripMenuSeparator = new ToolStripSeparator();
            OpenLogViewerToolStripMenuItem = new ToolStripMenuItem();
            OpenLogViewerToolStripMenuItem.Click += new EventHandler(OpenLogViewerToolStripMenuItem_Click);
            DeleteLogsToolStripMenuItem = new ToolStripMenuItem();
            DeleteLogsToolStripMenuItem.Click += new EventHandler(DeleteLogsToolStripMenuItem_Click);
            ExportsLogsToolStripMenuItem = new ToolStripMenuItem();
            ExportsLogsToolStripMenuItem.Click += new EventHandler(ExportsLogsToolStripMenuItem_Click);
            ImportExportSettingsToolStripMenuItem = new ToolStripMenuItem();
            IncludeButtonsOnNotifications = new ToolStripMenuItem();
            IncludeButtonsOnNotifications.Click += new EventHandler(IncludeButtonsOnNotifications_Click);
            IPv6Support = new ToolStripMenuItem();
            IPv6Support.Click += new EventHandler(IPv6Support_Click);
            ExportToolStripMenuItem = new ToolStripMenuItem();
            ExportToolStripMenuItem.Click += new EventHandler(ExportToolStripMenuItem_Click);
            ImportToolStripMenuItem = new ToolStripMenuItem();
            ImportToolStripMenuItem.Click += new EventHandler(ImportToolStripMenuItem_Click);
            OlderThan1DayToolStripMenuItem = new ToolStripMenuItem();
            OlderThan1DayToolStripMenuItem.Click += new EventHandler(OlderThan1DayToolStripMenuItem_Click);
            OlderThan2DaysToolStripMenuItem = new ToolStripMenuItem();
            OlderThan2DaysToolStripMenuItem.Click += new EventHandler(OlderThan2DaysToolStripMenuItem_Click);
            OlderThan3DaysToolStripMenuItem = new ToolStripMenuItem();
            OlderThan3DaysToolStripMenuItem.Click += new EventHandler(OlderThan3DaysToolStripMenuItem_Click);
            OlderThanAWeekToolStripMenuItem = new ToolStripMenuItem();
            OlderThanAWeekToolStripMenuItem.Click += new EventHandler(OlderThanAWeekToolStripMenuItem_Click);
            OpenWindowsExplorerToAppConfigFile = new ToolStripMenuItem();
            OpenWindowsExplorerToAppConfigFile.Click += new EventHandler(OpenWindowsExplorerToAppConfigFile_Click);
            ChkShowAlertedColumn = new ToolStripMenuItem();
            ChkShowAlertedColumn.Click += new EventHandler(ChkShowAlertedColumn_Click);
            RemoveNumbersFromRemoteApp = new ToolStripMenuItem();
            RemoveNumbersFromRemoteApp.Click += new EventHandler(RemoveNumbersFromRemoteApp_Click);
            ShowRawLogOnLogViewer = new ToolStripMenuItem();
            ShowRawLogOnLogViewer.Click += new EventHandler(ShowRawLogOnLogViewer_Click);
            ChkShowLogTypeColumn = new ToolStripMenuItem();
            ChkShowLogTypeColumn.Click += new EventHandler(ChkShowLogTypeColumn_Click);
            ChkShowServerTimeColumn = new ToolStripMenuItem();
            ChkShowServerTimeColumn.Click += new EventHandler(ChkShowServerTimeColumn_Click);
            ChkShowHostnameColumn = new ToolStripMenuItem();
            ChkShowHostnameColumn.Click += new EventHandler(ChkShowHostnameColumn_Click);
            CreateAlertToolStripMenuItem = new ToolStripMenuItem();
            CreateAlertToolStripMenuItem.Click += new EventHandler(CreateAlertToolStripMenuItem_Click);
            DonationStripMenuItem = new ToolStripMenuItem();
            DonationStripMenuItem.Click += new EventHandler(DonationStripMenuItem_Click);
            StopServerStripMenuItem = new ToolStripMenuItem();
            StopServerStripMenuItem.Click += new EventHandler(StopServerStripMenuItem_Click);
            ChangeSyslogServerPortToolStripMenuItem = new ToolStripMenuItem();
            ChangeSyslogServerPortToolStripMenuItem.Click += new EventHandler(ChangeSyslogServerPortToolStripMenuItem_Click);
            ChangeLogAutosaveIntervalToolStripMenuItem = new ToolStripMenuItem();
            ChangeLogAutosaveIntervalToolStripMenuItem.Click += new EventHandler(ChangeAutosaveIntervalToolStripMenuItem_Click);
            CreateIgnoredLogToolStripMenuItem = new ToolStripMenuItem();
            CreateIgnoredLogToolStripMenuItem.Click += new EventHandler(CreateIgnoredLogToolStripMenuItem_Click);
            CreateReplacementToolStripMenuItem = new ToolStripMenuItem();
            CreateReplacementToolStripMenuItem.Click += new EventHandler(CreateReplacementToolStripMenuItem_Click);
            ConfigureSysLogMirrorServers = new ToolStripMenuItem();
            ConfigureSysLogMirrorServers.Click += new EventHandler(ConfigureSysLogMirrorServers_Click);
            ConfigureTimeBetweenSameNotifications = new ToolStripMenuItem();
            ConfigureTimeBetweenSameNotifications.Click += new EventHandler(ConfigureTimeBetweenSameNotifications_Click);
            ConfirmDelete = new ToolStripMenuItem();
            ConfirmDelete.Click += new EventHandler(ConfirmDelete_Click);
            ChkDeselectItemAfterMinimizingWindow = new ToolStripMenuItem();
            ChkDeselectItemAfterMinimizingWindow.Click += new EventHandler(ChkDeselectItemAfterMinimizingWindow_Click);
            ChkDebug = new ToolStripMenuItem();
            ChkDebug.Click += new EventHandler(ChkDebug_Click);
            BackupFileNameDateFormatChooser = new ToolStripMenuItem();
            BackupFileNameDateFormatChooser.Click += new EventHandler(BackupFileNameDateFormatChooser_Click);
            MinimizeToClockTray = new ToolStripMenuItem();
            MinimizeToClockTray.Click += new EventHandler(MinimizeToClockTray_Click);
            NotificationLength = new ToolStripMenuItem();
            NotificationLengthLong = new ToolStripMenuItem();
            NotificationLengthLong.Click += new EventHandler(NotificationLengthLong_Click);
            NotificationLengthShort = new ToolStripMenuItem();
            NotificationLengthShort.Click += new EventHandler(NotificationLengthShort_Click);
            LoadingProgressBar = new ProgressBar();
            IconMenu = new ContextMenuStrip(components);
            ReOpenToolStripMenuItem = new ToolStripMenuItem();
            ReOpenToolStripMenuItem.Click += new EventHandler(ReOpenToolStripMenuItem_Click);
            StatusStrip.SuspendLayout();
            MenuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)Logs).BeginInit();
            LogsMenu.SuspendLayout();
            IconMenu.SuspendLayout();
            SuspendLayout();
            // 
            // NotificationLength
            // 
            NotificationLength.DropDownItems.AddRange(new ToolStripItem[] { NotificationLengthLong, NotificationLengthShort });
            NotificationLength.Name = "NotificationLength";
            NotificationLength.Size = new Size(339, 22);
            NotificationLength.Text = "Notification Length";
            // 
            // NotificationLengthLong
            // 
            NotificationLengthLong.CheckOnClick = true;
            NotificationLengthLong.Name = "NotificationLengthLong";
            NotificationLengthLong.Size = new Size(50, 22);
            NotificationLengthLong.Text = "Long";
            // 
            // NotificationLengthShort
            // 
            NotificationLengthShort.CheckOnClick = true;
            NotificationLengthShort.Name = "NotificationLengthShort";
            NotificationLengthShort.Size = new Size(50, 22);
            NotificationLengthShort.Text = "Short";
            // 
            // CreateAlertToolStripMenuItem
            // 
            CreateAlertToolStripMenuItem.Name = "CreateAlertToolStripMenuItem";
            CreateAlertToolStripMenuItem.Size = new Size(188, 22);
            CreateAlertToolStripMenuItem.Text = "Create Alert";
            // 
            // BtnOpenLogLocation
            // 
            BtnOpenLogLocation.Name = "BtnOpenLogLocation";
            BtnOpenLogLocation.Size = new Size(239, 22);
            BtnOpenLogLocation.Text = "Open Log File Location";
            // 
            // BtnOpenLogForViewing
            // 
            BtnOpenLogForViewing.Name = "BtnOpenLogForViewing";
            BtnOpenLogForViewing.Size = new Size(239, 22);
            BtnOpenLogForViewing.Text = "Open Log File for Viewing";
            // 
            // AlertsHistory
            // 
            AlertsHistory.Enabled = true;
            AlertsHistory.Name = "AlertsHistory";
            AlertsHistory.Size = new Size(239, 22);
            AlertsHistory.Text = "Alerts History";
            // 
            // BtnClearLog
            // 
            BtnClearLog.DropDownItems.AddRange(new ToolStripItem[] { BtnClearAllLogs, LogsOlderThanToolStripMenuItem });
            BtnClearLog.Enabled = false;
            BtnClearLog.Name = "BtnClearLog";
            BtnClearLog.Size = new Size(239, 22);
            BtnClearLog.Text = "Clear Logs";
            // 
            // BtnClearAllLogs
            // 
            BtnClearAllLogs.Name = "BtnClearAllLogs";
            BtnClearAllLogs.Size = new Size(165, 22);
            BtnClearAllLogs.Text = "All Logs";
            // 
            // LogsOlderThanToolStripMenuItem
            // 
            LogsOlderThanToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { OlderThan1DayToolStripMenuItem, OlderThan2DaysToolStripMenuItem, OlderThan3DaysToolStripMenuItem, OlderThanAWeekToolStripMenuItem });
            LogsOlderThanToolStripMenuItem.Name = "LogsOlderThanToolStripMenuItem";
            LogsOlderThanToolStripMenuItem.Size = new Size(165, 22);
            LogsOlderThanToolStripMenuItem.Text = "Logs older than...";
            // 
            // BtnSaveLogsToDisk
            // 
            BtnSaveLogsToDisk.Enabled = false;
            BtnSaveLogsToDisk.Name = "BtnSaveLogsToDisk";
            BtnSaveLogsToDisk.Size = new Size(239, 22);
            BtnSaveLogsToDisk.Text = "Save Logs to Disk";
            // 
            // StatusStrip
            // 
            StatusStrip.Items.AddRange(new ToolStripItem[] { NumberOfLogs, LblItemsSelected, LblAutoSaved, LblLogFileSize, LblNumberOfIgnoredIncomingLogs });
            StatusStrip.Location = new Point(0, 424);
            StatusStrip.Name = "StatusStrip";
            StatusStrip.Size = new Size(1175, 22);
            StatusStrip.TabIndex = 4;
            StatusStrip.Text = "StatusStrip";
            // 
            // NumberOfLogs
            // 
            NumberOfLogs.Margin = new Padding(0, 3, 25, 2);
            NumberOfLogs.Name = "NumberOfLogs";
            NumberOfLogs.Size = new Size(138, 17);
            NumberOfLogs.Text = "Number of Log Entries: 0";
            // 
            // LblItemsSelected
            // 
            LblItemsSelected.Margin = new Padding(0, 3, 25, 2);
            LblItemsSelected.Name = "LblItemsSelected";
            LblItemsSelected.Size = new Size(91, 17);
            LblItemsSelected.Text = "Selected Logs: 0";
            LblItemsSelected.Visible = false;
            // 
            // LblAutoSaved
            // 
            LblAutoSaved.Margin = new Padding(0, 3, 25, 2);
            LblAutoSaved.Name = "LblAutoSaved";
            LblAutoSaved.Size = new Size(193, 17);
            LblAutoSaved.Text = "Last Auto-Saved At: (Not Specified)";
            // 
            // LblLogFileSize
            // 
            LblLogFileSize.Margin = new Padding(0, 3, 25, 2);
            LblLogFileSize.Name = "LblLogFileSize";
            LblLogFileSize.Size = new Size(156, 17);
            LblLogFileSize.Text = "Log File Size: (Not Specified)";
            // 
            // LblNumberOfIgnoredIncomingLogs
            // 
            LblNumberOfIgnoredIncomingLogs.Name = "LblNumberOfIgnoredIncomingLogs";
            LblNumberOfIgnoredIncomingLogs.Size = new Size(200, 17);
            LblNumberOfIgnoredIncomingLogs.Text = "Number of ignored incoming logs: 0";
            // 
            // AutomaticallyCheckForUpdates
            // 
            AutomaticallyCheckForUpdates.CheckOnClick = true;
            AutomaticallyCheckForUpdates.Name = "AutomaticallyCheckForUpdates";
            AutomaticallyCheckForUpdates.Size = new Size(339, 22);
            AutomaticallyCheckForUpdates.Text = "Automatically Check for Updates";
            // 
            // ChkEnableAutoScroll
            // 
            ChkEnableAutoScroll.CheckOnClick = true;
            ChkEnableAutoScroll.Name = "ChkEnableAutoScroll";
            ChkEnableAutoScroll.Size = new Size(339, 22);
            ChkEnableAutoScroll.Text = "Enable Auto Scroll";
            // 
            // ToolStripMenuItem
            // 
            ChkDisableAutoScrollUponScrolling.CheckOnClick = true;
            ChkDisableAutoScrollUponScrolling.Name = "ChkDisableAutoScrollUponScrolling";
            ChkDisableAutoScrollUponScrolling.Size = new Size(339, 22);
            ChkDisableAutoScrollUponScrolling.Text = "        Disable Auto Scroll upon scrolling";
            // 
            // BtnCheckForUpdates
            // 
            BtnCheckForUpdates.Name = "BtnCheckForUpdates";
            BtnCheckForUpdates.Size = new Size(171, 22);
            BtnCheckForUpdates.Text = "Check for Updates";
            // 
            // SaveTimer
            // 
            SaveTimer.Interval = 300000;
            // 
            // ChkEnableAutoSave
            // 
            ChkEnableAutoSave.CheckOnClick = true;
            ChkEnableAutoSave.Name = "ChkEnableAutoSave";
            ChkEnableAutoSave.Size = new Size(339, 22);
            ChkEnableAutoSave.Text = "Enable Auto Save";
            // 
            // DeleteOldLogsAtMidnight
            // 
            DeleteOldLogsAtMidnight.CheckOnClick = true;
            DeleteOldLogsAtMidnight.Name = "DeleteOldLogsAtMidnight";
            DeleteOldLogsAtMidnight.Size = new Size(339, 22);
            DeleteOldLogsAtMidnight.Text = "Delete Old Logs at Midnight";
            // 
            // BackupOldLogsAfterClearingAtMidnight
            // 
            BackupOldLogsAfterClearingAtMidnight.CheckOnClick = true;
            BackupOldLogsAfterClearingAtMidnight.Enabled = false;
            BackupOldLogsAfterClearingAtMidnight.Name = "BackupOldLogsAfterClearingAtMidnight";
            BackupOldLogsAfterClearingAtMidnight.Size = new Size(339, 22);
            BackupOldLogsAfterClearingAtMidnight.Text = "        Backup old logs after clearing at midnight";
            // 
            // MinimizeToClockTray
            // 
            MinimizeToClockTray.CheckOnClick = true;
            MinimizeToClockTray.Name = "MinimizeToClockTray";
            MinimizeToClockTray.Size = new Size(339, 22);
            MinimizeToClockTray.Text = "Minimize to Clock Tray";
            // 
            // ChkDebug
            // 
            ChkDebug.CheckOnClick = true;
            ChkDebug.Name = "ChkDebug";
            ChkDebug.Size = new Size(339, 22);
            ChkDebug.Text = "Debug Mode";
            ChkDebug.ToolTipText = "Enables debug data from the program to be written to the Syslog Data.";
            // 
            // ChkDeselectItemAfterMinimizingWindow
            // 
            ChkDeselectItemAfterMinimizingWindow.CheckOnClick = true;
            ChkDeselectItemAfterMinimizingWindow.Name = "ChkDeselectItemAfterMinimizingWindow";
            ChkDeselectItemAfterMinimizingWindow.Size = new Size(339, 22);
            ChkDeselectItemAfterMinimizingWindow.Text = "De-Select Items When Minimizing Window";
            // 
            // BackupFileNameDateFormatChooser
            // 
            BackupFileNameDateFormatChooser.Name = "BackupFileNameDateFormatChooser";
            BackupFileNameDateFormatChooser.Size = new Size(339, 22);
            BackupFileNameDateFormatChooser.Text = "Backup File Name Date Format Chooser";
            // 
            // ChkEnableStartAtUserStartup
            // 
            ChkEnableStartAtUserStartup.CheckOnClick = true;
            ChkEnableStartAtUserStartup.Name = "ChkEnableStartAtUserStartup";
            ChkEnableStartAtUserStartup.Size = new Size(339, 22);
            ChkEnableStartAtUserStartup.Text = "Enable Start at Startup";
            // 
            // ChkEnableTCPSyslogServer
            // 
            ChkEnableTCPSyslogServer.CheckOnClick = true;
            ChkEnableTCPSyslogServer.Name = "ChkEnableTCPSyslogServer";
            ChkEnableTCPSyslogServer.Size = new Size(339, 22);
            ChkEnableTCPSyslogServer.Text = "Enable TCP Syslog Server";
            // 
            // StartUpDelay
            // 
            StartUpDelay.Name = "StartUpDelay";
            StartUpDelay.Enabled = false;
            StartUpDelay.Size = new Size(339, 22);
            StartUpDelay.Text = "        Startup Delay";
            // 
            // ChkRegExSearch
            // 
            ChkRegExSearch.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            ChkRegExSearch.AutoSize = true;
            ChkRegExSearch.Location = new Point(239, 31);
            ChkRegExSearch.Name = "ChkRegExSearch";
            ChkRegExSearch.Size = new Size(63, 17);
            ChkRegExSearch.TabIndex = 16;
            ChkRegExSearch.Text = "Regex?";
            ToolTip.SetToolTip(ChkRegExSearch, "Be careful with regex searches, a malformed regex pattern may cause the program t" + "o malfunction.");
            ChkRegExSearch.UseVisualStyleBackColor = true;
            // 
            // MenuStrip
            // 
            MenuStrip.Items.AddRange(new ToolStripItem[] { MainMenuToolStripMenuItem, LogFunctionsToolStripMenuItem, SettingsToolStripMenuItem, DonationStripMenuItem });
            MenuStrip.Location = new Point(0, 0);
            MenuStrip.Name = "MenuStrip";
            MenuStrip.Size = new Size(1175, 24);
            MenuStrip.TabIndex = 12;
            MenuStrip.Text = "MenuStrip1";
            // 
            // MainMenuToolStripMenuItem
            // 
            MainMenuToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { AboutToolStripMenuItem, BtnCheckForUpdates, StopServerStripMenuItem, ToolStripMenuSeparator, CloseMe });
            MainMenuToolStripMenuItem.Name = "MainMenuToolStripMenuItem";
            MainMenuToolStripMenuItem.Size = new Size(80, 20);
            MainMenuToolStripMenuItem.Text = "Main Menu";
            // 
            // ToolStripMenuSeparator
            // 
            ToolStripMenuSeparator.Name = "ToolStripMenuSeparator";
            ToolStripMenuSeparator.Size = new Size(177, 6);
            // 
            // CloseMe
            // 
            CloseMe.Name = "CloseMe";
            CloseMe.Size = new Size(171, 22);
            CloseMe.Text = "Close";
            // 
            // LogFunctionsToolStripMenuItem
            // 
            LogFunctionsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { AlertsHistory, BtnClearLog, ClearNotificationLimits, ExportAllLogsToolStripMenuItem, IgnoredLogsToolStripMenuItem, BtnOpenLogLocation, BtnOpenLogForViewing, BtnSaveLogsToDisk, ViewLogBackups, ZerooutIgnoredLogsCounterToolStripMenuItem });
            LogFunctionsToolStripMenuItem.Name = "LogFunctionsToolStripMenuItem";
            LogFunctionsToolStripMenuItem.Size = new Size(94, 20);
            LogFunctionsToolStripMenuItem.Text = "Log Functions";
            // 
            // IgnoredLogsToolStripMenuItem
            // 
            IgnoredLogsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { ClearIgnoredLogsToolStripMenuItem, ViewIgnoredLogsToolStripMenuItem });
            IgnoredLogsToolStripMenuItem.Name = "IgnoredLogsToolStripMenuItem";
            IgnoredLogsToolStripMenuItem.Size = new Size(239, 22);
            IgnoredLogsToolStripMenuItem.Text = "Ignored Logs";
            // 
            // ClearIgnoredLogsToolStripMenuItem
            // 
            ClearIgnoredLogsToolStripMenuItem.Name = "ClearIgnoredLogsToolStripMenuItem";
            ClearIgnoredLogsToolStripMenuItem.Enabled = false;
            ClearIgnoredLogsToolStripMenuItem.Size = new Size(101, 22);
            ClearIgnoredLogsToolStripMenuItem.Text = "Clear";
            // 
            // ViewIgnoredLogsToolStripMenuItem
            // 
            ViewIgnoredLogsToolStripMenuItem.Name = "ViewIgnoredLogsToolStripMenuItem";
            ViewIgnoredLogsToolStripMenuItem.Size = new Size(101, 22);
            ViewIgnoredLogsToolStripMenuItem.Text = "View";
            // 
            // ZerooutIgnoredLogsCounterToolStripMenuItem
            // 
            ZerooutIgnoredLogsCounterToolStripMenuItem.Enabled = false;
            ZerooutIgnoredLogsCounterToolStripMenuItem.Name = "ZerooutIgnoredLogsCounterToolStripMenuItem";
            ZerooutIgnoredLogsCounterToolStripMenuItem.Size = new Size(239, 22);
            ZerooutIgnoredLogsCounterToolStripMenuItem.Text = "Zero-out Ignored Logs Counter";
            ZerooutIgnoredLogsCounterToolStripMenuItem.Visible = false;
            // 
            // ViewLogBackups
            // 
            ViewLogBackups.Name = "ViewLogBackups";
            ViewLogBackups.Size = new Size(239, 22);
            ViewLogBackups.Text = "View Log Backups";
            ViewLogBackups.Visible = false;
            // 
            // SettingsToolStripMenuItem
            // 
            SettingsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { AutomaticallyCheckForUpdates, BackupFileNameDateFormatChooser, ChangeAlternatingColorToolStripMenuItem, ChangeFont, ChangeSyslogServerPortToolStripMenuItem, ColumnControls, ConfigureAlertsToolStripMenuItem, ConfigureHostnames, ConfigureIgnoredWordsAndPhrasesToolStripMenuItem, ConfigureReplacementsToolStripMenuItem, ConfigureSysLogMirrorServers, ConfigureTimeBetweenSameNotifications, ConfirmDelete, ChkDebug, ChkDeselectItemAfterMinimizingWindow, DeleteOldLogsAtMidnight, BackupOldLogsAfterClearingAtMidnight, ChkEnableAutoSave, ChangeLogAutosaveIntervalToolStripMenuItem, ChkEnableAutoScroll, ChkDisableAutoScrollUponScrolling, ChkEnableConfirmCloseToolStripItem, IPv6Support, ChkEnableRecordingOfIgnoredLogs, ChkEnableTCPSyslogServer, ChkEnableStartAtUserStartup, StartUpDelay, ImportExportSettingsToolStripMenuItem, IncludeButtonsOnNotifications, ColLogsAutoFill, MinimizeToClockTray, NotificationLength, OpenWindowsExplorerToAppConfigFile, RemoveNumbersFromRemoteApp, ShowRawLogOnLogViewer });
            SettingsToolStripMenuItem.Name = "SettingsToolStripMenuItem";
            SettingsToolStripMenuItem.Size = new Size(61, 20);
            SettingsToolStripMenuItem.Text = "Settings";
            // 
            // ClearNotificationLimits
            // 
            ClearNotificationLimits.Name = "ClearNotificationLimits";
            ClearNotificationLimits.Size = new Size(239, 22);
            ClearNotificationLimits.Text = "Clear Notification Limits";
            // 
            // ColumnControls
            // 
            ColumnControls.DropDownItems.AddRange(new ToolStripItem[] { ChkShowAlertedColumn, ChkShowHostnameColumn, ChkShowLogTypeColumn, ChkShowServerTimeColumn });
            ColumnControls.Name = "ColumnControls";
            ColumnControls.Size = new Size(339, 22);
            ColumnControls.Text = "Column Controls";
            // 
            // ColLogsAutoFill
            // 
            ColLogsAutoFill.CheckOnClick = true;
            ColLogsAutoFill.Name = "ColLogsAutoFill";
            ColLogsAutoFill.Size = new Size(339, 22);
            ColLogsAutoFill.Text = "Log Column AutoFill";
            // 
            // ConfigureReplacementsToolStripMenuItem
            // 
            ConfigureReplacementsToolStripMenuItem.Name = "ConfigureReplacementsToolStripMenuItem";
            ConfigureReplacementsToolStripMenuItem.Size = new Size(339, 22);
            ConfigureReplacementsToolStripMenuItem.Text = "Configure Replacements";
            // 
            // ConfigureIgnoredWordsAndPhrasesToolStripMenuItem
            // 
            ConfigureIgnoredWordsAndPhrasesToolStripMenuItem.Name = "ConfigureIgnoredWordsAndPhrasesToolStripMenuItem";
            ConfigureIgnoredWordsAndPhrasesToolStripMenuItem.Size = new Size(339, 22);
            ConfigureIgnoredWordsAndPhrasesToolStripMenuItem.Text = "Configure Ignored Words and Phrases";
            // 
            // ConfigureSysLogMirrorServers
            // 
            ConfigureSysLogMirrorServers.Name = "ConfigureSysLogMirrorServers";
            ConfigureSysLogMirrorServers.Size = new Size(339, 22);
            ConfigureSysLogMirrorServers.Text = "Configure SysLog Mirror Clients";
            // 
            // ConfigureTimeBetweenSameNotifications
            // 
            ConfigureTimeBetweenSameNotifications.Name = "ConfigureTimeBetweenSameNotifications";
            ConfigureTimeBetweenSameNotifications.Size = new Size(339, 22);
            ConfigureTimeBetweenSameNotifications.Text = "Configure Time Between Same Notifications";
            // 
            // ConfirmDelete
            // 
            ConfirmDelete.CheckOnClick = true;
            ConfirmDelete.Name = "ConfirmDelete";
            ConfirmDelete.Size = new Size(339, 22);
            ConfirmDelete.Text = "Confirm Deletion of Logs";
            // 
            // ChkEnableRecordingOfIgnoredLogs
            // 
            ChkEnableRecordingOfIgnoredLogs.CheckOnClick = true;
            ChkEnableRecordingOfIgnoredLogs.Name = "ChkEnableRecordingOfIgnoredLogs";
            ChkEnableRecordingOfIgnoredLogs.Size = new Size(339, 22);
            ChkEnableRecordingOfIgnoredLogs.Text = "Enable Recording of Ignored Logs";
            ChkEnableRecordingOfIgnoredLogs.ToolTipText = "When enabled, ignored logs are only stored in the program's memory and are not wr" + "itten to disk.";
            // 
            // LblSearchLabel
            // 
            LblSearchLabel.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            LblSearchLabel.AutoSize = true;
            LblSearchLabel.Location = new Point(12, 31);
            LblSearchLabel.Name = "LblSearchLabel";
            LblSearchLabel.Size = new Size(67, 13);
            LblSearchLabel.TabIndex = 13;
            LblSearchLabel.Text = "Search Logs";
            // 
            // TxtSearchTerms
            // 
            TxtSearchTerms.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            TxtSearchTerms.Location = new Point(85, 28);
            TxtSearchTerms.Name = "TxtSearchTerms";
            TxtSearchTerms.Size = new Size(148, 20);
            TxtSearchTerms.TabIndex = 14;
            // 
            // BtnSearch
            // 
            BtnSearch.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            BtnSearch.Location = new Point(416, 27);
            BtnSearch.Name = "BtnSearch";
            BtnSearch.Size = new Size(52, 23);
            BtnSearch.TabIndex = 15;
            BtnSearch.Text = "Search";
            BtnSearch.UseVisualStyleBackColor = true;
            // 
            // ChkCaseInsensitiveSearch
            // 
            ChkCaseInsensitiveSearch.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            ChkCaseInsensitiveSearch.AutoSize = true;
            ChkCaseInsensitiveSearch.Checked = true;
            ChkCaseInsensitiveSearch.CheckState = CheckState.Checked;
            ChkCaseInsensitiveSearch.Location = new Point(308, 31);
            ChkCaseInsensitiveSearch.Name = "ChkCaseInsensitiveSearch";
            ChkCaseInsensitiveSearch.Size = new Size(109, 17);
            ChkCaseInsensitiveSearch.TabIndex = 17;
            ChkCaseInsensitiveSearch.Text = "Case Insensitive?";
            ChkCaseInsensitiveSearch.UseVisualStyleBackColor = true;
            // 
            // Logs
            // 
            Logs.AllowUserToAddRows = false;
            Logs.AllowUserToOrderColumns = true;
            Logs.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            Logs.BackgroundColor = SystemColors.ButtonHighlight;
            Logs.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            Logs.Columns.AddRange(new DataGridViewColumn[] { ColTime, colServerTime, colLogType, ColIPAddress, ColHostname, ColRemoteProcess, ColLog, ColAlerts });
            Logs.ContextMenuStrip = LogsMenu;
            Logs.Location = new Point(12, 52);
            Logs.Name = "Logs";
            Logs.ReadOnly = true;
            Logs.RowHeadersVisible = false;
            Logs.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            Logs.Size = new Size(1151, 369);
            Logs.TabIndex = 18;
            // 
            // colServerTime
            // 
            colServerTime.HeaderText = "Server Time";
            colServerTime.Name = "colServerTime";
            colServerTime.ReadOnly = true;
            colServerTime.SortMode = DataGridViewColumnSortMode.Programmatic;
            colServerTime.ToolTipText = "The time on the server at which the log entry came in.";
            // 
            // ColTime
            // 
            ColTime.HeaderText = "Time";
            ColTime.Name = "ColTime";
            ColTime.ReadOnly = true;
            ColTime.SortMode = DataGridViewColumnSortMode.Programmatic;
            ColTime.ToolTipText = "The time at which the log entry came in.";
            // 
            // ColIPAddress
            // 
            ColIPAddress.HeaderText = "IP Address";
            ColIPAddress.Name = "ColIPAddress";
            ColIPAddress.ReadOnly = true;
            ColIPAddress.SortMode = DataGridViewColumnSortMode.Programmatic;
            ColIPAddress.ToolTipText = "The IP address of the system from which the log came from.";
            // 
            // colLogType
            // 
            colLogType.HeaderText = "Log Type";
            colLogType.Name = "colLogType";
            colLogType.ReadOnly = true;
            colLogType.Width = 200;
            // 
            // ColAlerts
            // 
            ColAlerts.HeaderText = "Alerted";
            ColAlerts.Name = "ColAlerts";
            ColAlerts.ReadOnly = true;
            ColAlerts.SortMode = DataGridViewColumnSortMode.Programmatic;
            ColAlerts.Width = 50;
            ColAlerts.ToolTipText = "Yes or No. Indicates if the log entry triggered an alert from this program.";
            // 
            // ColHostname
            // 
            ColHostname.HeaderText = "Hostname/Device Name";
            ColHostname.Name = "ColHostname";
            ColHostname.ReadOnly = true;
            ColHostname.SortMode = DataGridViewColumnSortMode.Programmatic;
            ColHostname.Width = 150;
            // 
            // ColRemoteProcess
            // 
            ColRemoteProcess.HeaderText = "Remote Process";
            ColRemoteProcess.Name = "ColRemoteProcess";
            ColRemoteProcess.ReadOnly = true;
            ColRemoteProcess.SortMode = DataGridViewColumnSortMode.Programmatic;
            ColRemoteProcess.Width = 150;
            // 
            // ColLog
            // 
            ColLog.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            ColLog.HeaderText = "Log";
            ColLog.Name = "ColLog";
            ColLog.ReadOnly = true;
            ColLog.SortMode = DataGridViewColumnSortMode.Programmatic;
            ColLog.ToolTipText = "The text contents of the log.";
            // 
            // ConfigureAlertsToolStripMenuItem
            // 
            ConfigureAlertsToolStripMenuItem.Name = "ConfigureAlertsToolStripMenuItem";
            ConfigureAlertsToolStripMenuItem.Size = new Size(339, 22);
            ConfigureAlertsToolStripMenuItem.Text = "Configure Alerts";
            // 
            // ConfigureHostnames
            // 
            ConfigureHostnames.Name = "ConfigureHostnames";
            ConfigureHostnames.Size = new Size(339, 22);
            ConfigureHostnames.Text = "Configure Custom Hostnames/Device Names";
            // 
            // ChangeAlternatingColorToolStripMenuItem
            // 
            ChangeAlternatingColorToolStripMenuItem.Name = "ChangeAlternatingColorToolStripMenuItem";
            ChangeAlternatingColorToolStripMenuItem.Size = new Size(339, 22);
            ChangeAlternatingColorToolStripMenuItem.Text = "Change Alternating Row Color";
            // 
            // ChangeFont
            // 
            ChangeFont.Name = "ChangeFont";
            ChangeFont.Size = new Size(339, 22);
            ChangeFont.Text = "Change Font";
            // 
            // AboutToolStripMenuItem
            // 
            AboutToolStripMenuItem.Name = "AboutToolStripMenuItem";
            AboutToolStripMenuItem.Size = new Size(171, 22);
            AboutToolStripMenuItem.Text = "About";
            // 
            // ImportExportSettingsToolStripMenuItem
            // 
            ImportExportSettingsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { ExportToolStripMenuItem, ImportToolStripMenuItem });
            ImportExportSettingsToolStripMenuItem.Name = "ImportExportSettingsToolStripMenuItem";
            ImportExportSettingsToolStripMenuItem.Size = new Size(339, 22);
            ImportExportSettingsToolStripMenuItem.Text = "Import/Export Program Settings";
            // 
            // IncludeButtonsOnNotifications
            // 
            IncludeButtonsOnNotifications.CheckOnClick = true;
            IncludeButtonsOnNotifications.Name = "ImportExportSIncludeButtonsOnNotificationsettingsToolStripMenuItem";
            IncludeButtonsOnNotifications.Size = new Size(339, 22);
            IncludeButtonsOnNotifications.Text = "Include Buttons on Notifications";
            // 
            // IPv6Support
            // 
            IPv6Support.CheckOnClick = true;
            IPv6Support.Name = "IPv6Support";
            IPv6Support.Size = new Size(339, 22);
            IPv6Support.Text = "Enable IPv6 Support";
            // 
            // ExportToolStripMenuItem
            // 
            ExportToolStripMenuItem.Name = "ExportToolStripMenuItem";
            ExportToolStripMenuItem.Size = new Size(110, 22);
            ExportToolStripMenuItem.Text = "Export";
            // 
            // ImportToolStripMenuItem
            // 
            ImportToolStripMenuItem.Name = "ImportToolStripMenuItem";
            ImportToolStripMenuItem.Size = new Size(110, 22);
            ImportToolStripMenuItem.Text = "Import";
            // 
            // NotifyIcon
            // 
            NotifyIcon.ContextMenuStrip = IconMenu;
            NotifyIcon.Text = "NotifyIcon";
            NotifyIcon.Visible = true;
            // 
            // OlderThan1DayToolStripMenuItem
            // 
            OlderThan1DayToolStripMenuItem.Name = "OlderThan1DayToolStripMenuItem";
            OlderThan1DayToolStripMenuItem.Size = new Size(169, 22);
            OlderThan1DayToolStripMenuItem.Text = "Older than 1 day";
            // 
            // OlderThan2DaysToolStripMenuItem
            // 
            OlderThan2DaysToolStripMenuItem.Name = "OlderThan2DaysToolStripMenuItem";
            OlderThan2DaysToolStripMenuItem.Size = new Size(169, 22);
            OlderThan2DaysToolStripMenuItem.Text = "Older than 2 days";
            // 
            // OlderThan3DaysToolStripMenuItem
            // 
            OlderThan3DaysToolStripMenuItem.Name = "OlderThan3DaysToolStripMenuItem";
            OlderThan3DaysToolStripMenuItem.Size = new Size(169, 22);
            OlderThan3DaysToolStripMenuItem.Text = "Older than 3 days";
            // 
            // OlderThanAWeekToolStripMenuItem
            // 
            OlderThanAWeekToolStripMenuItem.Name = "OlderThanAWeekToolStripMenuItem";
            OlderThanAWeekToolStripMenuItem.Size = new Size(169, 22);
            OlderThanAWeekToolStripMenuItem.Text = "Older than a week";
            // 
            // ChkEnableConfirmCloseToolStripItem
            // 
            ChkEnableConfirmCloseToolStripItem.CheckOnClick = true;
            ChkEnableConfirmCloseToolStripItem.Name = "ChkEnableConfirmCloseToolStripItem";
            ChkEnableConfirmCloseToolStripItem.Size = new Size(339, 22);
            ChkEnableConfirmCloseToolStripItem.Text = "Enable Confirm Close";
            // 
            // LogsMenu
            // 
            LogsMenu.Items.AddRange(new ToolStripItem[] { CopyLogTextToolStripMenuItem, CreateAlertToolStripMenuItem, CreateIgnoredLogToolStripMenuItem, CreateReplacementToolStripMenuItem, DeleteLogsToolStripMenuItem, ExportsLogsToolStripMenuItem, OpenLogViewerToolStripMenuItem });
            LogsMenu.Name = "LogsMenu";
            LogsMenu.Size = new Size(189, 158);
            // 
            // CopyLogTextToolStripMenuItem
            // 
            CopyLogTextToolStripMenuItem.Name = "CopyLogTextToolStripMenuItem";
            CopyLogTextToolStripMenuItem.Size = new Size(188, 22);
            CopyLogTextToolStripMenuItem.Text = "Copy Log Text";
            // 
            // OpenLogViewerToolStripMenuItem
            // 
            OpenLogViewerToolStripMenuItem.Name = "OpenLogViewerToolStripMenuItem";
            OpenLogViewerToolStripMenuItem.Size = new Size(188, 22);
            OpenLogViewerToolStripMenuItem.Text = "Open Log Viewer";
            // 
            // DeleteLogsToolStripMenuItem
            // 
            DeleteLogsToolStripMenuItem.Name = "DeleteLogsToolStripMenuItem";
            DeleteLogsToolStripMenuItem.Size = new Size(188, 22);
            DeleteLogsToolStripMenuItem.Text = "Delete Selected Logs";
            // 
            // ExportAllLogsToolStripMenuItem
            // 
            ExportAllLogsToolStripMenuItem.Name = "ExportAllLogsToolStripMenuItem";
            ExportAllLogsToolStripMenuItem.Size = new Size(188, 22);
            ExportAllLogsToolStripMenuItem.Text = "Export All Logs";
            // 
            // ExportsLogsToolStripMenuItem
            // 
            ExportsLogsToolStripMenuItem.Name = "ExportsLogsToolStripMenuItem";
            ExportsLogsToolStripMenuItem.Size = new Size(188, 22);
            ExportsLogsToolStripMenuItem.Text = "Export Selected Logs";
            // 
            // DonationStripMenuItem
            // 
            DonationStripMenuItem.Name = "DonationStripMenuItem";
            DonationStripMenuItem.Size = new Size(113, 20);
            DonationStripMenuItem.Text = "Donate via PayPal";
            // 
            // StopServerStripMenuItem
            // 
            StopServerStripMenuItem.Name = "StopServerStripMenuItem";
            StopServerStripMenuItem.Size = new Size(171, 22);
            StopServerStripMenuItem.Text = "Stop Server";
            // 
            // ChangeSyslogServerPortToolStripMenuItem
            // 
            ChangeSyslogServerPortToolStripMenuItem.Name = "ChangeSyslogServerPortToolStripMenuItem";
            ChangeSyslogServerPortToolStripMenuItem.Size = new Size(339, 22);
            ChangeSyslogServerPortToolStripMenuItem.Text = "Change Syslog Server Port";
            // 
            // ChangeLogAutosaveIntervalToolStripMenuItem
            // 
            ChangeLogAutosaveIntervalToolStripMenuItem.Name = "ChangeLogAutosaveIntervalToolStripMenuItem";
            ChangeLogAutosaveIntervalToolStripMenuItem.Size = new Size(339, 22);
            ChangeLogAutosaveIntervalToolStripMenuItem.Text = "        Change Log Autosave Interval";
            // 
            // OpenWindowsExplorerToAppConfigFile
            // 
            OpenWindowsExplorerToAppConfigFile.Name = "OpenWindowsExplorerToAppConfigFile";
            OpenWindowsExplorerToAppConfigFile.Size = new Size(339, 22);
            OpenWindowsExplorerToAppConfigFile.Text = "Open Windows Explorer to Application Config File";
            // 
            // ChkShowLogTypeColumn
            // 
            ChkShowLogTypeColumn.CheckOnClick = true;
            ChkShowLogTypeColumn.Name = "ChkShowLogTypeColumn";
            ChkShowLogTypeColumn.Size = new Size(339, 22);
            ChkShowLogTypeColumn.Text = "Show Log Type Column";
            // 
            // ChkShowServerTimeColumn
            // 
            ChkShowServerTimeColumn.CheckOnClick = true;
            ChkShowServerTimeColumn.Name = "ChkShowServerTimeColumn";
            ChkShowServerTimeColumn.Size = new Size(339, 22);
            ChkShowServerTimeColumn.Text = "Show Server Time Column";
            // 
            // ChkShowHostnameColumn
            // 
            ChkShowHostnameColumn.CheckOnClick = true;
            ChkShowHostnameColumn.Name = "ChkShowHostnameColumn";
            ChkShowHostnameColumn.Size = new Size(339, 22);
            ChkShowHostnameColumn.Text = "Show Hostname Column";
            // 
            // ChkShowAlertedColumn
            // 
            ChkShowAlertedColumn.CheckOnClick = true;
            ChkShowAlertedColumn.Name = "ChkShowAlertedColumn";
            ChkShowAlertedColumn.Size = new Size(339, 22);
            ChkShowAlertedColumn.Text = "Show Alerted Column";
            // 
            // RemoveNumbersFromRemoteApp
            // 
            RemoveNumbersFromRemoteApp.CheckOnClick = true;
            RemoveNumbersFromRemoteApp.Name = "RemoveNumbersFromRemoteApp";
            RemoveNumbersFromRemoteApp.Size = new Size(188, 22);
            RemoveNumbersFromRemoteApp.Text = "Remove Numbers From Remote App";
            // 
            // ShowRawLogOnLogViewer
            // 
            ShowRawLogOnLogViewer.CheckOnClick = true;
            ShowRawLogOnLogViewer.Name = "ShowRawLogOnLogViewer";
            ShowRawLogOnLogViewer.Size = new Size(188, 22);
            ShowRawLogOnLogViewer.Text = "Show Raw Log on Log Viewer Window";
            // 
            // CreateIgnoredLogToolStripMenuItem
            // 
            CreateIgnoredLogToolStripMenuItem.Name = "CreateIgnoredLogToolStripMenuItem";
            CreateIgnoredLogToolStripMenuItem.Size = new Size(188, 22);
            CreateIgnoredLogToolStripMenuItem.Text = "Create Ignored Log";
            // 
            // CreateReplacementToolStripMenuItem
            // 
            CreateReplacementToolStripMenuItem.Name = "CreateReplacementToolStripMenuItem";
            CreateReplacementToolStripMenuItem.Size = new Size(188, 22);
            CreateReplacementToolStripMenuItem.Text = "Create Replacement";
            // 
            // LoadingProgressBar
            // 
            LoadingProgressBar.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            LoadingProgressBar.Location = new Point(474, 27);
            LoadingProgressBar.Name = "LoadingProgressBar";
            LoadingProgressBar.Size = new Size(689, 23);
            LoadingProgressBar.TabIndex = 19;
            LoadingProgressBar.Visible = false;
            // 
            // IconMenu
            // 
            IconMenu.Items.AddRange(new ToolStripItem[] { ReOpenToolStripMenuItem });
            IconMenu.Name = "IconMenu";
            IconMenu.Size = new Size(181, 48);
            // 
            // ReOpenToolStripMenuItem
            // 
            ReOpenToolStripMenuItem.Name = "ReOpenToolStripMenuItem";
            ReOpenToolStripMenuItem.Size = new Size(180, 22);
            ReOpenToolStripMenuItem.Text = "Re-Open";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(6.0f, 13.0f);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1175, 446);
            Controls.Add(LoadingProgressBar);
            Controls.Add(Logs);
            Controls.Add(ChkCaseInsensitiveSearch);
            Controls.Add(ChkRegExSearch);
            Controls.Add(BtnSearch);
            Controls.Add(TxtSearchTerms);
            Controls.Add(LblSearchLabel);
            Controls.Add(StatusStrip);
            Controls.Add(MenuStrip);
            MainMenuStrip = MenuStrip;
            MinimumSize = new Size(1191, 485);
            Name = "Form1";
            Text = "Free SysLog Server";
            StatusStrip.ResumeLayout(false);
            StatusStrip.PerformLayout();
            MenuStrip.ResumeLayout(false);
            MenuStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)Logs).EndInit();
            LogsMenu.ResumeLayout(false);
            IconMenu.ResumeLayout(false);
            ResizeBegin += new EventHandler(Form1_ResizeBegin);
            ResizeEnd += new EventHandler(Form1_ResizeEnd);
            Resize += new EventHandler(Form1_Resize);
            FormClosing += new FormClosingEventHandler(Form1_FormClosing);
            Load += new EventHandler(Form1_Load);
            ResumeLayout(false);
            PerformLayout();

        }
        internal ToolStripMenuItem BtnOpenLogLocation;
        internal ToolStripMenuItem BtnOpenLogForViewing;
        internal SaveFileDialog SaveFileDialog;
        internal StatusStrip StatusStrip;
        internal ToolStripStatusLabel NumberOfLogs;
        internal ToolStripMenuItem ChkEnableAutoScroll;
        internal ToolStripMenuItem ChkDisableAutoScrollUponScrolling;
        internal ToolStripMenuItem AutomaticallyCheckForUpdates;
        internal ToolStripMenuItem BtnClearLog;
        internal ToolStripMenuItem AlertsHistory;
        internal ToolStripMenuItem BtnSaveLogsToDisk;
        internal ToolStripMenuItem BtnCheckForUpdates;
        internal Timer SaveTimer;
        internal ToolStripMenuItem ChkEnableAutoSave;
        internal ToolStripMenuItem DeleteOldLogsAtMidnight;
        internal ToolStripMenuItem BackupOldLogsAfterClearingAtMidnight;
        internal ToolStripStatusLabel LblAutoSaved;
        internal ToolStripMenuItem ChkEnableStartAtUserStartup;
        internal ToolStripMenuItem ChkEnableTCPSyslogServer;
        internal ToolStripMenuItem StartUpDelay;
        internal ToolStripStatusLabel LblLogFileSize;
        internal ToolTip ToolTip;
        internal MenuStrip MenuStrip;
        internal ToolStripMenuItem MainMenuToolStripMenuItem;
        internal ToolStripMenuItem LogFunctionsToolStripMenuItem;
        internal ToolStripMenuItem SettingsToolStripMenuItem;
        internal Label LblSearchLabel;
        internal TextBox TxtSearchTerms;
        internal Button BtnSearch;
        internal ToolStripMenuItem ConfigureIgnoredWordsAndPhrasesToolStripMenuItem;
        internal ToolStripStatusLabel LblNumberOfIgnoredIncomingLogs;
        internal ToolStripMenuItem ViewIgnoredLogsToolStripMenuItem;
        internal ToolStripMenuItem ClearIgnoredLogsToolStripMenuItem;
        internal ToolStripMenuItem IgnoredLogsToolStripMenuItem;
        internal ToolStripMenuItem ChkEnableRecordingOfIgnoredLogs;
        internal ToolStripMenuItem BtnClearAllLogs;
        internal ToolStripMenuItem LogsOlderThanToolStripMenuItem;
        internal ToolStripMenuItem ZerooutIgnoredLogsCounterToolStripMenuItem;
        internal ToolStripMenuItem ViewLogBackups;
        internal ToolStripMenuItem ConfigureReplacementsToolStripMenuItem;
        internal CheckBox ChkRegExSearch;
        internal CheckBox ChkCaseInsensitiveSearch;
        internal DataGridView Logs;
        internal DataGridViewTextBoxColumn ColTime;
        internal DataGridViewTextBoxColumn colServerTime;
        internal DataGridViewTextBoxColumn ColIPAddress;
        internal DataGridViewTextBoxColumn colLogType;
        internal DataGridViewTextBoxColumn ColLog;
        internal DataGridViewTextBoxColumn ColAlerts;
        internal DataGridViewTextBoxColumn ColRemoteProcess;
        internal DataGridViewTextBoxColumn ColHostname;
        internal ColorDialog ColorDialog;
        internal ToolStripMenuItem ChangeAlternatingColorToolStripMenuItem;
        internal ToolStripMenuItem ChangeFont;
        internal ToolStripMenuItem ConfigureAlertsToolStripMenuItem;
        internal ToolStripMenuItem ConfigureHostnames;
        internal ToolStripMenuItem ColumnControls;
        internal ToolStripMenuItem ClearNotificationLimits;
        internal ToolStripMenuItem ColLogsAutoFill;
        internal ToolStripMenuItem AboutToolStripMenuItem;
        internal ToolStripMenuItem CloseMe;
        internal ToolStripSeparator ToolStripMenuSeparator;
        internal ToolStripMenuItem ImportExportSettingsToolStripMenuItem;
        internal ToolStripMenuItem IncludeButtonsOnNotifications;
        internal ToolStripMenuItem IPv6Support;
        internal ToolStripMenuItem ExportToolStripMenuItem;
        internal ToolStripMenuItem ImportToolStripMenuItem;
        internal OpenFileDialog OpenFileDialog;
        internal ToolStripMenuItem OlderThan1DayToolStripMenuItem;
        internal ToolStripMenuItem OlderThan2DaysToolStripMenuItem;
        internal ToolStripMenuItem OlderThan3DaysToolStripMenuItem;
        internal ToolStripMenuItem OlderThanAWeekToolStripMenuItem;
        internal NotifyIcon NotifyIcon;
        internal ToolStripMenuItem ChkEnableConfirmCloseToolStripItem;
        internal ContextMenuStrip LogsMenu;
        internal ToolStripMenuItem CopyLogTextToolStripMenuItem;
        internal ToolStripMenuItem OpenLogViewerToolStripMenuItem;
        internal ToolStripMenuItem DeleteLogsToolStripMenuItem;
        internal ToolStripMenuItem ExportsLogsToolStripMenuItem;
        internal ToolStripMenuItem ExportAllLogsToolStripMenuItem;
        internal ToolStripMenuItem DonationStripMenuItem;
        internal ToolStripMenuItem StopServerStripMenuItem;
        internal ToolStripMenuItem OpenWindowsExplorerToAppConfigFile;
        internal ToolStripMenuItem CreateAlertToolStripMenuItem;
        internal ToolStripMenuItem ChangeSyslogServerPortToolStripMenuItem;
        internal ToolStripMenuItem ChangeLogAutosaveIntervalToolStripMenuItem;
        internal ToolStripMenuItem CreateIgnoredLogToolStripMenuItem;
        internal ToolStripMenuItem CreateReplacementToolStripMenuItem;
        internal ToolStripStatusLabel LblItemsSelected;
        internal ToolStripMenuItem ChkDeselectItemAfterMinimizingWindow;
        internal ToolStripMenuItem ChkDebug;
        internal ToolStripMenuItem ChkShowAlertedColumn;
        internal ToolStripMenuItem RemoveNumbersFromRemoteApp;
        internal ToolStripMenuItem ShowRawLogOnLogViewer;
        internal ToolStripMenuItem ChkShowLogTypeColumn;
        internal ToolStripMenuItem ChkShowServerTimeColumn;
        internal ToolStripMenuItem ChkShowHostnameColumn;
        internal ToolStripMenuItem ConfigureSysLogMirrorServers;
        internal ToolStripMenuItem ConfigureTimeBetweenSameNotifications;
        internal ToolStripMenuItem ConfirmDelete;
        internal ProgressBar LoadingProgressBar;
        internal ToolStripMenuItem MinimizeToClockTray;
        internal ToolStripMenuItem BackupFileNameDateFormatChooser;
        internal ToolStripMenuItem NotificationLength;
        internal ToolStripMenuItem NotificationLengthLong;
        internal ToolStripMenuItem NotificationLengthShort;
        internal ContextMenuStrip IconMenu;
        internal ToolStripMenuItem ReOpenToolStripMenuItem;
    }
}