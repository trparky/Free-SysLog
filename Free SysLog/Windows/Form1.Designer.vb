Imports Windows.Networking.Sockets

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.BtnOpenLogLocation = New System.Windows.Forms.ToolStripMenuItem()
        Me.BtnOpenLogForViewing = New System.Windows.Forms.ToolStripMenuItem()
        Me.BtnClearLog = New System.Windows.Forms.ToolStripMenuItem()
        Me.AlertsHistory = New System.Windows.Forms.ToolStripMenuItem()
        Me.BtnClearAllLogs = New System.Windows.Forms.ToolStripMenuItem()
        Me.LogsOlderThanToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.BtnSaveLogsToDisk = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExportAllLogsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.StatusStrip = New System.Windows.Forms.StatusStrip()
        Me.NumberOfLogs = New System.Windows.Forms.ToolStripStatusLabel()
        Me.LblAutoSaved = New System.Windows.Forms.ToolStripStatusLabel()
        Me.LblItemsSelected = New System.Windows.Forms.ToolStripStatusLabel()
        Me.LblLogFileSize = New System.Windows.Forms.ToolStripStatusLabel()
        Me.LblNumberOfIgnoredIncomingLogs = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ChkEnableAutoScroll = New System.Windows.Forms.ToolStripMenuItem()
        Me.AutomaticallyCheckForUpdates = New System.Windows.Forms.ToolStripMenuItem()
        Me.BtnCheckForUpdates = New System.Windows.Forms.ToolStripMenuItem()
        Me.SaveTimer = New System.Windows.Forms.Timer(Me.components)
        Me.ChkEnableAutoSave = New System.Windows.Forms.ToolStripMenuItem()
        Me.DeleteOldLogsAtMidnight = New System.Windows.Forms.ToolStripMenuItem()
        Me.BackupOldLogsAfterClearingAtMidnight = New System.Windows.Forms.ToolStripMenuItem()
        Me.ChkEnableStartAtUserStartup = New System.Windows.Forms.ToolStripMenuItem()
        Me.ChkEnableTCPSyslogServer = New System.Windows.Forms.ToolStripMenuItem()
        Me.StartUpDelay = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolTip = New System.Windows.Forms.ToolTip(Me.components)
        Me.ChkRegExSearch = New System.Windows.Forms.CheckBox()
        Me.MenuStrip = New System.Windows.Forms.MenuStrip()
        Me.MainMenuToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.LogFunctionsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.IgnoredLogsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ClearIgnoredLogsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ViewIgnoredLogsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ZerooutIgnoredLogsCounterToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ViewLogBackups = New System.Windows.Forms.ToolStripMenuItem()
        Me.SettingsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ConfigureReplacementsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ConfigureIgnoredWordsAndPhrasesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ChkEnableRecordingOfIgnoredLogs = New System.Windows.Forms.ToolStripMenuItem()
        Me.LblSearchLabel = New System.Windows.Forms.Label()
        Me.TxtSearchTerms = New System.Windows.Forms.TextBox()
        Me.BtnSearch = New System.Windows.Forms.Button()
        Me.ChkCaseInsensitiveSearch = New System.Windows.Forms.CheckBox()
        Me.Logs = New System.Windows.Forms.DataGridView()
        Me.ColTime = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colServerTime = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ColIPAddress = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colLogType = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ColLog = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ColAlerts = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ColRemoteProcess = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ColHostname = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.NotifyIcon = New System.Windows.Forms.NotifyIcon(Me.components)
        Me.ChkEnableConfirmCloseToolStripItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ChangeAlternatingColorToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ChangeFont = New System.Windows.Forms.ToolStripMenuItem()
        Me.ConfigureAlertsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ConfigureHostnames = New System.Windows.Forms.ToolStripMenuItem()
        Me.ColumnControls = New System.Windows.Forms.ToolStripMenuItem()
        Me.LogsMenu = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.CopyLogTextToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AboutToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CloseMe = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuSeparator = New System.Windows.Forms.ToolStripSeparator()
        Me.OpenLogViewerToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.DeleteLogsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExportsLogsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ImportExportSettingsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.IncludeButtonsOnNotifications = New System.Windows.Forms.ToolStripMenuItem()
        Me.IPv6Support = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExportToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ImportToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.OlderThan1DayToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.OlderThan2DaysToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.OlderThan3DaysToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.OlderThanAWeekToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.OpenWindowsExplorerToAppConfigFile = New System.Windows.Forms.ToolStripMenuItem()
        Me.ChkShowAlertedColumn = New System.Windows.Forms.ToolStripMenuItem()
        Me.RemoveNumbersFromRemoteApp = New System.Windows.Forms.ToolStripMenuItem()
        Me.ShowRawLogOnLogViewer = New System.Windows.Forms.ToolStripMenuItem()
        Me.ChkShowLogTypeColumn = New System.Windows.Forms.ToolStripMenuItem()
        Me.ChkShowServerTimeColumn = New System.Windows.Forms.ToolStripMenuItem()
        Me.ChkShowHostnameColumn = New System.Windows.Forms.ToolStripMenuItem()
        Me.CreateAlertToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.DonationStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.DarkMode = New System.Windows.Forms.ToolStripMenuItem()
        Me.StopServerStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ChangeSyslogServerPortToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ChangeLogAutosaveIntervalToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CreateIgnoredLogToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CreateReplacementToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ConfigureSysLogMirrorServers = New System.Windows.Forms.ToolStripMenuItem()
        Me.ConfigureTimeBetweenSameNotifications = New System.Windows.Forms.ToolStripMenuItem()
        Me.ChkDeselectItemAfterMinimizingWindow = New System.Windows.Forms.ToolStripMenuItem()
        Me.BackupFileNameDateFormatChooser = New System.Windows.Forms.ToolStripMenuItem()
        Me.MinimizeToClockTray = New System.Windows.Forms.ToolStripMenuItem()
        Me.NotificationLength = New System.Windows.Forms.ToolStripMenuItem()
        Me.NotificationLengthLong = New System.Windows.Forms.ToolStripMenuItem()
        Me.NotificationLengthShort = New System.Windows.Forms.ToolStripMenuItem()
        Me.LoadingProgressBar = New System.Windows.Forms.ProgressBar()
        Me.StatusStrip.SuspendLayout()
        Me.MenuStrip.SuspendLayout()
        CType(Me.Logs, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.LogsMenu.SuspendLayout()
        Me.SuspendLayout()
        '
        'NotificationLength
        '
        Me.NotificationLength.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.NotificationLengthLong, Me.NotificationLengthShort})
        Me.NotificationLength.Name = "NotificationLength"
        Me.NotificationLength.Size = New System.Drawing.Size(339, 22)
        Me.NotificationLength.Text = "Notification Length"
        '
        'NotificationLengthLong
        '
        Me.NotificationLengthLong.CheckOnClick = True
        Me.NotificationLengthLong.Name = "NotificationLengthLong"
        Me.NotificationLengthLong.Size = New System.Drawing.Size(50, 22)
        Me.NotificationLengthLong.Text = "Long"
        '
        'NotificationLengthShort
        '
        Me.NotificationLengthShort.CheckOnClick = True
        Me.NotificationLengthShort.Name = "NotificationLengthShort"
        Me.NotificationLengthShort.Size = New System.Drawing.Size(50, 22)
        Me.NotificationLengthShort.Text = "Short"
        '
        'CreateAlertToolStripMenuItem
        '
        Me.CreateAlertToolStripMenuItem.Name = "CreateAlertToolStripMenuItem"
        Me.CreateAlertToolStripMenuItem.Size = New System.Drawing.Size(188, 22)
        Me.CreateAlertToolStripMenuItem.Text = "Create Alert"
        '
        'BtnOpenLogLocation
        '
        Me.BtnOpenLogLocation.Name = "BtnOpenLogLocation"
        Me.BtnOpenLogLocation.Size = New System.Drawing.Size(239, 22)
        Me.BtnOpenLogLocation.Text = "Open Log File Location"
        '
        'BtnOpenLogForViewing
        '
        Me.BtnOpenLogForViewing.Name = "BtnOpenLogForViewing"
        Me.BtnOpenLogForViewing.Size = New System.Drawing.Size(239, 22)
        Me.BtnOpenLogForViewing.Text = "Open Log File for Viewing"
        '
        'AlertsHistory
        '
        Me.AlertsHistory.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.BtnClearAllLogs, Me.LogsOlderThanToolStripMenuItem})
        Me.AlertsHistory.Enabled = True
        Me.AlertsHistory.Name = "AlertsHistory"
        Me.AlertsHistory.Size = New System.Drawing.Size(239, 22)
        Me.AlertsHistory.Text = "Alerts History"
        '
        'BtnClearLog
        '
        Me.BtnClearLog.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.BtnClearAllLogs, Me.LogsOlderThanToolStripMenuItem})
        Me.BtnClearLog.Enabled = False
        Me.BtnClearLog.Name = "BtnClearLog"
        Me.BtnClearLog.Size = New System.Drawing.Size(239, 22)
        Me.BtnClearLog.Text = "Clear Logs"
        '
        'BtnClearAllLogs
        '
        Me.BtnClearAllLogs.Name = "BtnClearAllLogs"
        Me.BtnClearAllLogs.Size = New System.Drawing.Size(165, 22)
        Me.BtnClearAllLogs.Text = "All Logs"
        '
        'LogsOlderThanToolStripMenuItem
        '
        Me.LogsOlderThanToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.OlderThan1DayToolStripMenuItem, Me.OlderThan2DaysToolStripMenuItem, Me.OlderThan3DaysToolStripMenuItem, Me.OlderThanAWeekToolStripMenuItem})
        Me.LogsOlderThanToolStripMenuItem.Name = "LogsOlderThanToolStripMenuItem"
        Me.LogsOlderThanToolStripMenuItem.Size = New System.Drawing.Size(165, 22)
        Me.LogsOlderThanToolStripMenuItem.Text = "Logs older than..."
        '
        'BtnSaveLogsToDisk
        '
        Me.BtnSaveLogsToDisk.Enabled = False
        Me.BtnSaveLogsToDisk.Name = "BtnSaveLogsToDisk"
        Me.BtnSaveLogsToDisk.Size = New System.Drawing.Size(239, 22)
        Me.BtnSaveLogsToDisk.Text = "Save Logs to Disk"
        '
        'StatusStrip
        '
        Me.StatusStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.NumberOfLogs, Me.LblItemsSelected, Me.LblAutoSaved, Me.LblLogFileSize, Me.LblNumberOfIgnoredIncomingLogs})
        Me.StatusStrip.Location = New System.Drawing.Point(0, 424)
        Me.StatusStrip.Name = "StatusStrip"
        Me.StatusStrip.Size = New System.Drawing.Size(1175, 22)
        Me.StatusStrip.TabIndex = 4
        Me.StatusStrip.Text = "StatusStrip"
        '
        'NumberOfLogs
        '
        Me.NumberOfLogs.Margin = New System.Windows.Forms.Padding(0, 3, 25, 2)
        Me.NumberOfLogs.Name = "NumberOfLogs"
        Me.NumberOfLogs.Size = New System.Drawing.Size(138, 17)
        Me.NumberOfLogs.Text = "Number of Log Entries: 0"
        '
        'LblItemsSelected
        '
        Me.LblItemsSelected.Margin = New System.Windows.Forms.Padding(0, 3, 25, 2)
        Me.LblItemsSelected.Name = "LblItemsSelected"
        Me.LblItemsSelected.Size = New System.Drawing.Size(91, 17)
        Me.LblItemsSelected.Text = "Selected Logs: 0"
        Me.LblItemsSelected.Visible = False
        '
        'LblAutoSaved
        '
        Me.LblAutoSaved.Margin = New System.Windows.Forms.Padding(0, 3, 25, 2)
        Me.LblAutoSaved.Name = "LblAutoSaved"
        Me.LblAutoSaved.Size = New System.Drawing.Size(193, 17)
        Me.LblAutoSaved.Text = "Last Auto-Saved At: (Not Specified)"
        '
        'LblLogFileSize
        '
        Me.LblLogFileSize.Margin = New System.Windows.Forms.Padding(0, 3, 25, 2)
        Me.LblLogFileSize.Name = "LblLogFileSize"
        Me.LblLogFileSize.Size = New System.Drawing.Size(156, 17)
        Me.LblLogFileSize.Text = "Log File Size: (Not Specified)"
        '
        'LblNumberOfIgnoredIncomingLogs
        '
        Me.LblNumberOfIgnoredIncomingLogs.Name = "LblNumberOfIgnoredIncomingLogs"
        Me.LblNumberOfIgnoredIncomingLogs.Size = New System.Drawing.Size(200, 17)
        Me.LblNumberOfIgnoredIncomingLogs.Text = "Number of ignored incoming logs: 0"
        '
        'AutomaticallyCheckForUpdates
        '
        Me.AutomaticallyCheckForUpdates.CheckOnClick = True
        Me.AutomaticallyCheckForUpdates.Name = "AutomaticallyCheckForUpdates"
        Me.AutomaticallyCheckForUpdates.Size = New System.Drawing.Size(339, 22)
        Me.AutomaticallyCheckForUpdates.Text = "Automatically Check for Updates"
        '
        'ChkEnableAutoScroll
        '
        Me.ChkEnableAutoScroll.CheckOnClick = True
        Me.ChkEnableAutoScroll.Name = "ChkEnableAutoScroll"
        Me.ChkEnableAutoScroll.Size = New System.Drawing.Size(339, 22)
        Me.ChkEnableAutoScroll.Text = "Enable Auto Scroll"
        '
        'BtnCheckForUpdates
        '
        Me.BtnCheckForUpdates.Name = "BtnCheckForUpdates"
        Me.BtnCheckForUpdates.Size = New System.Drawing.Size(171, 22)
        Me.BtnCheckForUpdates.Text = "Check for Updates"
        '
        'SaveTimer
        '
        Me.SaveTimer.Interval = 300000
        '
        'ChkEnableAutoSave
        '
        Me.ChkEnableAutoSave.CheckOnClick = True
        Me.ChkEnableAutoSave.Name = "ChkEnableAutoSave"
        Me.ChkEnableAutoSave.Size = New System.Drawing.Size(339, 22)
        Me.ChkEnableAutoSave.Text = "Enable Auto Save"
        '
        'DeleteOldLogsAtMidnight
        '
        Me.DeleteOldLogsAtMidnight.CheckOnClick = True
        Me.DeleteOldLogsAtMidnight.Name = "DeleteOldLogsAtMidnight"
        Me.DeleteOldLogsAtMidnight.Size = New System.Drawing.Size(339, 22)
        Me.DeleteOldLogsAtMidnight.Text = "Delete Old Logs at Midnight"
        '
        'BackupOldLogsAfterClearingAtMidnight
        '
        Me.BackupOldLogsAfterClearingAtMidnight.CheckOnClick = True
        Me.BackupOldLogsAfterClearingAtMidnight.Enabled = False
        Me.BackupOldLogsAfterClearingAtMidnight.Name = "BackupOldLogsAfterClearingAtMidnight"
        Me.BackupOldLogsAfterClearingAtMidnight.Size = New System.Drawing.Size(339, 22)
        Me.BackupOldLogsAfterClearingAtMidnight.Text = "        Backup old logs after clearing at midnight"
        '
        'MinimizeToClockTray
        '
        Me.MinimizeToClockTray.CheckOnClick = True
        Me.MinimizeToClockTray.Name = "MinimizeToClockTray"
        Me.MinimizeToClockTray.Size = New System.Drawing.Size(339, 22)
        Me.MinimizeToClockTray.Text = "Minimize to Clock Tray"
        '
        'ChkDeselectItemAfterMinimizingWindow
        '
        Me.ChkDeselectItemAfterMinimizingWindow.CheckOnClick = True
        Me.ChkDeselectItemAfterMinimizingWindow.Name = "ChkDeselectItemAfterMinimizingWindow"
        Me.ChkDeselectItemAfterMinimizingWindow.Size = New System.Drawing.Size(339, 22)
        Me.ChkDeselectItemAfterMinimizingWindow.Text = "De-Select Items When Minimizing Window"
        '
        'BackupFileNameDateFormatChooser
        '
        Me.BackupFileNameDateFormatChooser.Name = "BackupFileNameDateFormatChooser"
        Me.BackupFileNameDateFormatChooser.Size = New System.Drawing.Size(339, 22)
        Me.BackupFileNameDateFormatChooser.Text = "Backup File Name Date Format Chooser"
        '
        'ChkEnableStartAtUserStartup
        '
        Me.ChkEnableStartAtUserStartup.CheckOnClick = True
        Me.ChkEnableStartAtUserStartup.Name = "ChkEnableStartAtUserStartup"
        Me.ChkEnableStartAtUserStartup.Size = New System.Drawing.Size(339, 22)
        Me.ChkEnableStartAtUserStartup.Text = "Enable Start at Startup"
        '
        'ChkEnableTCPSyslogServer
        '
        Me.ChkEnableTCPSyslogServer.CheckOnClick = True
        Me.ChkEnableTCPSyslogServer.Name = "ChkEnableTCPSyslogServer"
        Me.ChkEnableTCPSyslogServer.Size = New System.Drawing.Size(339, 22)
        Me.ChkEnableTCPSyslogServer.Text = "Enable TCP Syslog Server"
        '
        'StartUpDelay
        '
        Me.StartUpDelay.Name = "StartUpDelay"
        Me.StartUpDelay.Enabled = False
        Me.StartUpDelay.Size = New System.Drawing.Size(339, 22)
        Me.StartUpDelay.Text = "        Startup Delay"
        '
        'ChkRegExSearch
        '
        Me.ChkRegExSearch.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.ChkRegExSearch.AutoSize = True
        Me.ChkRegExSearch.Location = New System.Drawing.Point(239, 31)
        Me.ChkRegExSearch.Name = "ChkRegExSearch"
        Me.ChkRegExSearch.Size = New System.Drawing.Size(63, 17)
        Me.ChkRegExSearch.TabIndex = 16
        Me.ChkRegExSearch.Text = "Regex?"
        Me.ToolTip.SetToolTip(Me.ChkRegExSearch, "Be careful with regex searches, a malformed regex pattern may cause the program t" &
        "o malfunction.")
        Me.ChkRegExSearch.UseVisualStyleBackColor = True
        '
        'MenuStrip
        '
        Me.MenuStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MainMenuToolStripMenuItem, Me.LogFunctionsToolStripMenuItem, Me.SettingsToolStripMenuItem, Me.DonationStripMenuItem})
        Me.MenuStrip.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip.Name = "MenuStrip"
        Me.MenuStrip.Size = New System.Drawing.Size(1175, 24)
        Me.MenuStrip.TabIndex = 12
        Me.MenuStrip.Text = "MenuStrip1"
        '
        'MainMenuToolStripMenuItem
        '
        Me.MainMenuToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AboutToolStripMenuItem, Me.BtnCheckForUpdates, Me.StopServerStripMenuItem, Me.ToolStripMenuSeparator, Me.CloseMe})
        Me.MainMenuToolStripMenuItem.Name = "MainMenuToolStripMenuItem"
        Me.MainMenuToolStripMenuItem.Size = New System.Drawing.Size(80, 20)
        Me.MainMenuToolStripMenuItem.Text = "Main Menu"
        '
        'ToolStripMenuSeparator
        '
        Me.ToolStripMenuSeparator.Name = "ToolStripMenuSeparator"
        Me.ToolStripMenuSeparator.Size = New System.Drawing.Size(177, 6)
        '
        'CloseMe
        '
        Me.CloseMe.Name = "CloseMe"
        Me.CloseMe.Size = New System.Drawing.Size(171, 22)
        Me.CloseMe.Text = "Close"
        '
        'LogFunctionsToolStripMenuItem
        '
        Me.LogFunctionsToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AlertsHistory, Me.BtnClearLog, Me.ExportAllLogsToolStripMenuItem, Me.IgnoredLogsToolStripMenuItem, Me.BtnOpenLogLocation, Me.BtnOpenLogForViewing, Me.BtnSaveLogsToDisk, Me.ViewLogBackups, Me.ZerooutIgnoredLogsCounterToolStripMenuItem})
        Me.LogFunctionsToolStripMenuItem.Name = "LogFunctionsToolStripMenuItem"
        Me.LogFunctionsToolStripMenuItem.Size = New System.Drawing.Size(94, 20)
        Me.LogFunctionsToolStripMenuItem.Text = "Log Functions"
        '
        'IgnoredLogsToolStripMenuItem
        '
        Me.IgnoredLogsToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ClearIgnoredLogsToolStripMenuItem, Me.ViewIgnoredLogsToolStripMenuItem})
        Me.IgnoredLogsToolStripMenuItem.Name = "IgnoredLogsToolStripMenuItem"
        Me.IgnoredLogsToolStripMenuItem.Size = New System.Drawing.Size(239, 22)
        Me.IgnoredLogsToolStripMenuItem.Text = "Ignored Logs"
        '
        'ClearIgnoredLogsToolStripMenuItem
        '
        Me.ClearIgnoredLogsToolStripMenuItem.Name = "ClearIgnoredLogsToolStripMenuItem"
        Me.ClearIgnoredLogsToolStripMenuItem.Enabled = False
        Me.ClearIgnoredLogsToolStripMenuItem.Size = New System.Drawing.Size(101, 22)
        Me.ClearIgnoredLogsToolStripMenuItem.Text = "Clear"
        '
        'ViewIgnoredLogsToolStripMenuItem
        '
        Me.ViewIgnoredLogsToolStripMenuItem.Name = "ViewIgnoredLogsToolStripMenuItem"
        Me.ViewIgnoredLogsToolStripMenuItem.Size = New System.Drawing.Size(101, 22)
        Me.ViewIgnoredLogsToolStripMenuItem.Text = "View"
        '
        'ZerooutIgnoredLogsCounterToolStripMenuItem
        '
        Me.ZerooutIgnoredLogsCounterToolStripMenuItem.Enabled = False
        Me.ZerooutIgnoredLogsCounterToolStripMenuItem.Name = "ZerooutIgnoredLogsCounterToolStripMenuItem"
        Me.ZerooutIgnoredLogsCounterToolStripMenuItem.Size = New System.Drawing.Size(239, 22)
        Me.ZerooutIgnoredLogsCounterToolStripMenuItem.Text = "Zero-out Ignored Logs Counter"
        Me.ZerooutIgnoredLogsCounterToolStripMenuItem.Visible = False
        '
        'ViewLogBackups
        '
        Me.ViewLogBackups.Name = "ViewLogBackups"
        Me.ViewLogBackups.Size = New System.Drawing.Size(239, 22)
        Me.ViewLogBackups.Text = "View Log Backups"
        Me.ViewLogBackups.Visible = False
        '
        'SettingsToolStripMenuItem
        '
        Me.SettingsToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AutomaticallyCheckForUpdates, Me.BackupFileNameDateFormatChooser, Me.ChangeAlternatingColorToolStripMenuItem, Me.ChangeFont, Me.ChangeSyslogServerPortToolStripMenuItem, Me.ColumnControls, Me.ConfigureAlertsToolStripMenuItem, Me.ConfigureHostnames, Me.ConfigureIgnoredWordsAndPhrasesToolStripMenuItem, Me.ConfigureReplacementsToolStripMenuItem, Me.ConfigureSysLogMirrorServers, Me.ConfigureTimeBetweenSameNotifications, Me.ChkDeselectItemAfterMinimizingWindow, Me.DarkMode, Me.DeleteOldLogsAtMidnight, Me.BackupOldLogsAfterClearingAtMidnight, Me.ChkEnableAutoSave, Me.ChangeLogAutosaveIntervalToolStripMenuItem, Me.ChkEnableAutoScroll, Me.ChkEnableConfirmCloseToolStripItem, Me.IPv6Support, Me.ChkEnableRecordingOfIgnoredLogs, Me.ChkEnableTCPSyslogServer, Me.ChkEnableStartAtUserStartup, Me.StartUpDelay, Me.ImportExportSettingsToolStripMenuItem, Me.IncludeButtonsOnNotifications, Me.MinimizeToClockTray, Me.NotificationLength, Me.OpenWindowsExplorerToAppConfigFile, Me.RemoveNumbersFromRemoteApp, Me.ShowRawLogOnLogViewer})
        Me.SettingsToolStripMenuItem.Name = "SettingsToolStripMenuItem"
        Me.SettingsToolStripMenuItem.Size = New System.Drawing.Size(61, 20)
        Me.SettingsToolStripMenuItem.Text = "Settings"
        '
        'ColumnControls
        '
        Me.ColumnControls.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ChkShowAlertedColumn, Me.ChkShowHostnameColumn, Me.ChkShowLogTypeColumn, Me.ChkShowServerTimeColumn})
        Me.ColumnControls.Name = "ColumnControls"
        Me.ColumnControls.Size = New System.Drawing.Size(339, 22)
        Me.ColumnControls.Text = "Column Controls"
        '
        'ConfigureReplacementsToolStripMenuItem
        '
        Me.ConfigureReplacementsToolStripMenuItem.Name = "ConfigureReplacementsToolStripMenuItem"
        Me.ConfigureReplacementsToolStripMenuItem.Size = New System.Drawing.Size(339, 22)
        Me.ConfigureReplacementsToolStripMenuItem.Text = "Configure Replacements"
        '
        'ConfigureIgnoredWordsAndPhrasesToolStripMenuItem
        '
        Me.ConfigureIgnoredWordsAndPhrasesToolStripMenuItem.Name = "ConfigureIgnoredWordsAndPhrasesToolStripMenuItem"
        Me.ConfigureIgnoredWordsAndPhrasesToolStripMenuItem.Size = New System.Drawing.Size(339, 22)
        Me.ConfigureIgnoredWordsAndPhrasesToolStripMenuItem.Text = "Configure Ignored Words and Phrases"
        '
        'ConfigureSysLogMirrorServers
        '
        Me.ConfigureSysLogMirrorServers.Name = "ConfigureSysLogMirrorServers"
        Me.ConfigureSysLogMirrorServers.Size = New System.Drawing.Size(339, 22)
        Me.ConfigureSysLogMirrorServers.Text = "Configure SysLog Mirror Servers"
        '
        'ConfigureTimeBetweenSameNotifications
        '
        Me.ConfigureTimeBetweenSameNotifications.Name = "ConfigureTimeBetweenSameNotifications"
        Me.ConfigureTimeBetweenSameNotifications.Size = New System.Drawing.Size(339, 22)
        Me.ConfigureTimeBetweenSameNotifications.Text = "Configure Time Between Same Notifications"
        '
        'ChkEnableRecordingOfIgnoredLogs
        '
        Me.ChkEnableRecordingOfIgnoredLogs.CheckOnClick = True
        Me.ChkEnableRecordingOfIgnoredLogs.Name = "ChkEnableRecordingOfIgnoredLogs"
        Me.ChkEnableRecordingOfIgnoredLogs.Size = New System.Drawing.Size(339, 22)
        Me.ChkEnableRecordingOfIgnoredLogs.Text = "Enable Recording of Ignored Logs"
        Me.ChkEnableRecordingOfIgnoredLogs.ToolTipText = "When enabled, ignored logs are only stored in the program's memory and are not wr" &
    "itten to disk."
        '
        'LblSearchLabel
        '
        Me.LblSearchLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.LblSearchLabel.AutoSize = True
        Me.LblSearchLabel.Location = New System.Drawing.Point(12, 31)
        Me.LblSearchLabel.Name = "LblSearchLabel"
        Me.LblSearchLabel.Size = New System.Drawing.Size(67, 13)
        Me.LblSearchLabel.TabIndex = 13
        Me.LblSearchLabel.Text = "Search Logs"
        '
        'TxtSearchTerms
        '
        Me.TxtSearchTerms.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.TxtSearchTerms.Location = New System.Drawing.Point(85, 28)
        Me.TxtSearchTerms.Name = "TxtSearchTerms"
        Me.TxtSearchTerms.Size = New System.Drawing.Size(148, 20)
        Me.TxtSearchTerms.TabIndex = 14
        '
        'BtnSearch
        '
        Me.BtnSearch.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.BtnSearch.Location = New System.Drawing.Point(416, 27)
        Me.BtnSearch.Name = "BtnSearch"
        Me.BtnSearch.Size = New System.Drawing.Size(52, 23)
        Me.BtnSearch.TabIndex = 15
        Me.BtnSearch.Text = "Search"
        Me.BtnSearch.UseVisualStyleBackColor = True
        '
        'ChkCaseInsensitiveSearch
        '
        Me.ChkCaseInsensitiveSearch.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.ChkCaseInsensitiveSearch.AutoSize = True
        Me.ChkCaseInsensitiveSearch.Checked = True
        Me.ChkCaseInsensitiveSearch.CheckState = System.Windows.Forms.CheckState.Checked
        Me.ChkCaseInsensitiveSearch.Location = New System.Drawing.Point(308, 31)
        Me.ChkCaseInsensitiveSearch.Name = "ChkCaseInsensitiveSearch"
        Me.ChkCaseInsensitiveSearch.Size = New System.Drawing.Size(109, 17)
        Me.ChkCaseInsensitiveSearch.TabIndex = 17
        Me.ChkCaseInsensitiveSearch.Text = "Case Insensitive?"
        Me.ChkCaseInsensitiveSearch.UseVisualStyleBackColor = True
        '
        'Logs
        '
        Me.Logs.AllowUserToAddRows = False
        Me.Logs.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Logs.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight
        Me.Logs.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.Logs.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.ColTime, Me.colServerTime, Me.colLogType, Me.ColIPAddress, Me.ColHostname, Me.ColRemoteProcess, Me.ColLog, Me.ColAlerts})
        Me.Logs.ContextMenuStrip = Me.LogsMenu
        Me.Logs.Location = New System.Drawing.Point(12, 52)
        Me.Logs.Name = "Logs"
        Me.Logs.ReadOnly = True
        Me.Logs.RowHeadersVisible = False
        Me.Logs.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.Logs.Size = New System.Drawing.Size(1151, 369)
        Me.Logs.TabIndex = 18
        '
        'colServerTime
        '
        Me.colServerTime.HeaderText = "Server Time"
        Me.colServerTime.Name = "colServerTime"
        Me.colServerTime.ReadOnly = True
        Me.colServerTime.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic
        Me.colServerTime.ToolTipText = "The time on the server at which the log entry came in."
        '
        'ColTime
        '
        Me.ColTime.HeaderText = "Time"
        Me.ColTime.Name = "ColTime"
        Me.ColTime.ReadOnly = True
        Me.ColTime.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic
        Me.ColTime.ToolTipText = "The time at which the log entry came in."
        '
        'ColIPAddress
        '
        Me.ColIPAddress.HeaderText = "IP Address"
        Me.ColIPAddress.Name = "ColIPAddress"
        Me.ColIPAddress.ReadOnly = True
        Me.ColIPAddress.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic
        Me.ColIPAddress.ToolTipText = "The IP address of the system from which the log came from."
        '
        'colLogType
        '
        Me.colLogType.HeaderText = "Log Type"
        Me.colLogType.Name = "colLogType"
        Me.colLogType.ReadOnly = True
        Me.colLogType.Width = 200
        '
        'ColAlerts
        '
        Me.ColAlerts.HeaderText = "Alerted"
        Me.ColAlerts.Name = "ColAlerts"
        Me.ColAlerts.ReadOnly = True
        Me.ColAlerts.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic
        Me.ColAlerts.Width = 50
        Me.ColAlerts.ToolTipText = "Yes or No. Indicates if the log entry triggered an alert from this program."
        '
        'ColHostname
        '
        Me.ColHostname.HeaderText = "Hostname/Device Name"
        Me.ColHostname.Name = "ColHostname"
        Me.ColHostname.ReadOnly = True
        Me.ColHostname.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic
        Me.ColHostname.Width = 150
        '
        'ColRemoteProcess
        '
        Me.ColRemoteProcess.HeaderText = "Remote Process"
        Me.ColRemoteProcess.Name = "ColRemoteProcess"
        Me.ColRemoteProcess.ReadOnly = True
        Me.ColRemoteProcess.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic
        Me.ColRemoteProcess.Width = 150
        '
        'ColLog
        '
        Me.ColLog.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        Me.ColLog.HeaderText = "Log"
        Me.ColLog.Name = "ColLog"
        Me.ColLog.ReadOnly = True
        Me.ColLog.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic
        Me.ColLog.ToolTipText = "The text contents of the log."
        '
        'ConfigureAlertsToolStripMenuItem
        '
        Me.ConfigureAlertsToolStripMenuItem.Name = "ConfigureAlertsToolStripMenuItem"
        Me.ConfigureAlertsToolStripMenuItem.Size = New System.Drawing.Size(339, 22)
        Me.ConfigureAlertsToolStripMenuItem.Text = "Configure Alerts"
        '
        'ConfigureHostnames
        '
        Me.ConfigureHostnames.Name = "ConfigureHostnames"
        Me.ConfigureHostnames.Size = New System.Drawing.Size(339, 22)
        Me.ConfigureHostnames.Text = "Configure Custom Hostnames/Device Names"
        '
        'ChangeAlternatingColorToolStripMenuItem
        '
        Me.ChangeAlternatingColorToolStripMenuItem.Name = "ChangeAlternatingColorToolStripMenuItem"
        Me.ChangeAlternatingColorToolStripMenuItem.Size = New System.Drawing.Size(339, 22)
        Me.ChangeAlternatingColorToolStripMenuItem.Text = "Change Alternating Row Color"
        '
        'ChangeFont
        '
        Me.ChangeFont.Name = "ChangeFont"
        Me.ChangeFont.Size = New System.Drawing.Size(339, 22)
        Me.ChangeFont.Text = "Change Font"
        '
        'AboutToolStripMenuItem
        '
        Me.AboutToolStripMenuItem.Name = "AboutToolStripMenuItem"
        Me.AboutToolStripMenuItem.Size = New System.Drawing.Size(171, 22)
        Me.AboutToolStripMenuItem.Text = "About"
        '
        'ImportExportSettingsToolStripMenuItem
        '
        Me.ImportExportSettingsToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ExportToolStripMenuItem, Me.ImportToolStripMenuItem})
        Me.ImportExportSettingsToolStripMenuItem.Name = "ImportExportSettingsToolStripMenuItem"
        Me.ImportExportSettingsToolStripMenuItem.Size = New System.Drawing.Size(339, 22)
        Me.ImportExportSettingsToolStripMenuItem.Text = "Import/Export Program Settings"
        '
        'IncludeButtonsOnNotifications
        '
        Me.IncludeButtonsOnNotifications.CheckOnClick = True
        Me.IncludeButtonsOnNotifications.Name = "ImportExportSIncludeButtonsOnNotificationsettingsToolStripMenuItem"
        Me.IncludeButtonsOnNotifications.Size = New System.Drawing.Size(339, 22)
        Me.IncludeButtonsOnNotifications.Text = "Include Buttons on Notifications"
        '
        'IPv6Support
        '
        Me.IPv6Support.CheckOnClick = True
        Me.IPv6Support.Name = "IPv6Support"
        Me.IPv6Support.Size = New System.Drawing.Size(339, 22)
        Me.IPv6Support.Text = "Enable IPv6 Support"
        '
        'ExportToolStripMenuItem
        '
        Me.ExportToolStripMenuItem.Name = "ExportToolStripMenuItem"
        Me.ExportToolStripMenuItem.Size = New System.Drawing.Size(110, 22)
        Me.ExportToolStripMenuItem.Text = "Export"
        '
        'ImportToolStripMenuItem
        '
        Me.ImportToolStripMenuItem.Name = "ImportToolStripMenuItem"
        Me.ImportToolStripMenuItem.Size = New System.Drawing.Size(110, 22)
        Me.ImportToolStripMenuItem.Text = "Import"
        '
        'NotifyIcon
        '
        Me.NotifyIcon.Text = "NotifyIcon"
        Me.NotifyIcon.Visible = True
        '
        'OlderThan1DayToolStripMenuItem
        '
        Me.OlderThan1DayToolStripMenuItem.Name = "OlderThan1DayToolStripMenuItem"
        Me.OlderThan1DayToolStripMenuItem.Size = New System.Drawing.Size(169, 22)
        Me.OlderThan1DayToolStripMenuItem.Text = "Older than 1 day"
        '
        'OlderThan2DaysToolStripMenuItem
        '
        Me.OlderThan2DaysToolStripMenuItem.Name = "OlderThan2DaysToolStripMenuItem"
        Me.OlderThan2DaysToolStripMenuItem.Size = New System.Drawing.Size(169, 22)
        Me.OlderThan2DaysToolStripMenuItem.Text = "Older than 2 days"
        '
        'OlderThan3DaysToolStripMenuItem
        '
        Me.OlderThan3DaysToolStripMenuItem.Name = "OlderThan3DaysToolStripMenuItem"
        Me.OlderThan3DaysToolStripMenuItem.Size = New System.Drawing.Size(169, 22)
        Me.OlderThan3DaysToolStripMenuItem.Text = "Older than 3 days"
        '
        'OlderThanAWeekToolStripMenuItem
        '
        Me.OlderThanAWeekToolStripMenuItem.Name = "OlderThanAWeekToolStripMenuItem"
        Me.OlderThanAWeekToolStripMenuItem.Size = New System.Drawing.Size(169, 22)
        Me.OlderThanAWeekToolStripMenuItem.Text = "Older than a week"
        '
        'ChkEnableConfirmCloseToolStripItem
        '
        Me.ChkEnableConfirmCloseToolStripItem.CheckOnClick = True
        Me.ChkEnableConfirmCloseToolStripItem.Name = "ChkEnableConfirmCloseToolStripItem"
        Me.ChkEnableConfirmCloseToolStripItem.Size = New System.Drawing.Size(339, 22)
        Me.ChkEnableConfirmCloseToolStripItem.Text = "Enable Confirm Close"
        '
        'LogsMenu
        '
        Me.LogsMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.CopyLogTextToolStripMenuItem, Me.CreateAlertToolStripMenuItem, Me.CreateIgnoredLogToolStripMenuItem, Me.CreateReplacementToolStripMenuItem, Me.DeleteLogsToolStripMenuItem, Me.ExportsLogsToolStripMenuItem, Me.OpenLogViewerToolStripMenuItem})
        Me.LogsMenu.Name = "LogsMenu"
        Me.LogsMenu.Size = New System.Drawing.Size(189, 158)
        '
        'CopyLogTextToolStripMenuItem
        '
        Me.CopyLogTextToolStripMenuItem.Name = "CopyLogTextToolStripMenuItem"
        Me.CopyLogTextToolStripMenuItem.Size = New System.Drawing.Size(188, 22)
        Me.CopyLogTextToolStripMenuItem.Text = "Copy Log Text"
        '
        'OpenLogViewerToolStripMenuItem
        '
        Me.OpenLogViewerToolStripMenuItem.Name = "OpenLogViewerToolStripMenuItem"
        Me.OpenLogViewerToolStripMenuItem.Size = New System.Drawing.Size(188, 22)
        Me.OpenLogViewerToolStripMenuItem.Text = "Open Log Viewer"
        '
        'DeleteLogsToolStripMenuItem
        '
        Me.DeleteLogsToolStripMenuItem.Name = "DeleteLogsToolStripMenuItem"
        Me.DeleteLogsToolStripMenuItem.Size = New System.Drawing.Size(188, 22)
        Me.DeleteLogsToolStripMenuItem.Text = "Delete Selected Logs"
        '
        'ExportAllLogsToolStripMenuItem
        '
        Me.ExportAllLogsToolStripMenuItem.Name = "ExportAllLogsToolStripMenuItem"
        Me.ExportAllLogsToolStripMenuItem.Size = New System.Drawing.Size(188, 22)
        Me.ExportAllLogsToolStripMenuItem.Text = "Export All Logs"
        '
        'ExportsLogsToolStripMenuItem
        '
        Me.ExportsLogsToolStripMenuItem.Name = "ExportsLogsToolStripMenuItem"
        Me.ExportsLogsToolStripMenuItem.Size = New System.Drawing.Size(188, 22)
        Me.ExportsLogsToolStripMenuItem.Text = "Export Selected Logs"
        '
        'DonationStripMenuItem
        '
        Me.DonationStripMenuItem.Name = "DonationStripMenuItem"
        Me.DonationStripMenuItem.Size = New System.Drawing.Size(113, 20)
        Me.DonationStripMenuItem.Text = "Donate via PayPal"
        '
        'DarkMode
        '
        Me.DarkMode.CheckOnClick = True
        Me.DarkMode.Name = "DarkMode"
        Me.DarkMode.Size = New System.Drawing.Size(113, 20)
        Me.DarkMode.Text = "Dark Mode?"
        '
        'StopServerStripMenuItem
        '
        Me.StopServerStripMenuItem.Name = "StopServerStripMenuItem"
        Me.StopServerStripMenuItem.Size = New System.Drawing.Size(171, 22)
        Me.StopServerStripMenuItem.Text = "Stop Server"
        '
        'ChangeSyslogServerPortToolStripMenuItem
        '
        Me.ChangeSyslogServerPortToolStripMenuItem.Name = "ChangeSyslogServerPortToolStripMenuItem"
        Me.ChangeSyslogServerPortToolStripMenuItem.Size = New System.Drawing.Size(339, 22)
        Me.ChangeSyslogServerPortToolStripMenuItem.Text = "Change Syslog Server Port"
        '
        'ChangeLogAutosaveIntervalToolStripMenuItem
        '
        Me.ChangeLogAutosaveIntervalToolStripMenuItem.Name = "ChangeLogAutosaveIntervalToolStripMenuItem"
        Me.ChangeLogAutosaveIntervalToolStripMenuItem.Size = New System.Drawing.Size(339, 22)
        Me.ChangeLogAutosaveIntervalToolStripMenuItem.Text = "        Change Log Autosave Interval"
        '
        'OpenWindowsExplorerToAppConfigFile
        '
        Me.OpenWindowsExplorerToAppConfigFile.Name = "OpenWindowsExplorerToAppConfigFile"
        Me.OpenWindowsExplorerToAppConfigFile.Size = New System.Drawing.Size(339, 22)
        Me.OpenWindowsExplorerToAppConfigFile.Text = "Open Windows Explorer to Application Config File"
        '
        'ChkShowLogTypeColumn
        '
        Me.ChkShowLogTypeColumn.CheckOnClick = True
        Me.ChkShowLogTypeColumn.Name = "ChkShowLogTypeColumn"
        Me.ChkShowLogTypeColumn.Size = New System.Drawing.Size(339, 22)
        Me.ChkShowLogTypeColumn.Text = "Show Log Type Column"
        '
        'ChkShowServerTimeColumn
        '
        Me.ChkShowServerTimeColumn.CheckOnClick = True
        Me.ChkShowServerTimeColumn.Name = "ChkShowServerTimeColumn"
        Me.ChkShowServerTimeColumn.Size = New System.Drawing.Size(339, 22)
        Me.ChkShowServerTimeColumn.Text = "Show Server Time Column"
        '
        'ChkShowHostnameColumn
        '
        Me.ChkShowHostnameColumn.CheckOnClick = True
        Me.ChkShowHostnameColumn.Name = "ChkShowHostnameColumn"
        Me.ChkShowHostnameColumn.Size = New System.Drawing.Size(339, 22)
        Me.ChkShowHostnameColumn.Text = "Show Hostname Column"
        '
        'ChkShowAlertedColumn
        '
        Me.ChkShowAlertedColumn.CheckOnClick = True
        Me.ChkShowAlertedColumn.Name = "ChkShowAlertedColumn"
        Me.ChkShowAlertedColumn.Size = New System.Drawing.Size(339, 22)
        Me.ChkShowAlertedColumn.Text = "Show Alerted Column"
        '
        'RemoveNumbersFromRemoteApp
        '
        Me.RemoveNumbersFromRemoteApp.CheckOnClick = True
        Me.RemoveNumbersFromRemoteApp.Name = "RemoveNumbersFromRemoteApp"
        Me.RemoveNumbersFromRemoteApp.Size = New System.Drawing.Size(188, 22)
        Me.RemoveNumbersFromRemoteApp.Text = "Remove Numbers From Remote App"
        '
        'ShowRawLogOnLogViewer
        '
        Me.ShowRawLogOnLogViewer.CheckOnClick = True
        Me.ShowRawLogOnLogViewer.Name = "ShowRawLogOnLogViewer"
        Me.ShowRawLogOnLogViewer.Size = New System.Drawing.Size(188, 22)
        Me.ShowRawLogOnLogViewer.Text = "Show Raw Log on Log Viewer Window"
        '
        'CreateIgnoredLogToolStripMenuItem
        '
        Me.CreateIgnoredLogToolStripMenuItem.Name = "CreateIgnoredLogToolStripMenuItem"
        Me.CreateIgnoredLogToolStripMenuItem.Size = New System.Drawing.Size(188, 22)
        Me.CreateIgnoredLogToolStripMenuItem.Text = "Create Ignored Log"
        '
        'CreateReplacementToolStripMenuItem
        '
        Me.CreateReplacementToolStripMenuItem.Name = "CreateReplacementToolStripMenuItem"
        Me.CreateReplacementToolStripMenuItem.Size = New System.Drawing.Size(188, 22)
        Me.CreateReplacementToolStripMenuItem.Text = "Create Replacement"
        '
        'LoadingProgressBar
        '
        Me.LoadingProgressBar.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.LoadingProgressBar.Location = New System.Drawing.Point(474, 27)
        Me.LoadingProgressBar.Name = "LoadingProgressBar"
        Me.LoadingProgressBar.Size = New System.Drawing.Size(689, 23)
        Me.LoadingProgressBar.TabIndex = 19
        Me.LoadingProgressBar.Visible = False
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1175, 446)
        Me.Controls.Add(Me.LoadingProgressBar)
        Me.Controls.Add(Me.Logs)
        Me.Controls.Add(Me.ChkCaseInsensitiveSearch)
        Me.Controls.Add(Me.ChkRegExSearch)
        Me.Controls.Add(Me.BtnSearch)
        Me.Controls.Add(Me.TxtSearchTerms)
        Me.Controls.Add(Me.LblSearchLabel)
        Me.Controls.Add(Me.StatusStrip)
        Me.Controls.Add(Me.MenuStrip)
        Me.MainMenuStrip = Me.MenuStrip
        Me.MinimumSize = New System.Drawing.Size(1191, 485)
        Me.Name = "Form1"
        Me.Text = "Free SysLog Server"
        Me.StatusStrip.ResumeLayout(False)
        Me.StatusStrip.PerformLayout()
        Me.MenuStrip.ResumeLayout(False)
        Me.MenuStrip.PerformLayout()
        CType(Me.Logs, System.ComponentModel.ISupportInitialize).EndInit()
        Me.LogsMenu.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents BtnOpenLogLocation As ToolStripMenuItem
    Friend WithEvents BtnOpenLogForViewing As ToolStripMenuItem
    Friend WithEvents SaveFileDialog As SaveFileDialog
    Friend WithEvents StatusStrip As StatusStrip
    Friend WithEvents NumberOfLogs As ToolStripStatusLabel
    Friend WithEvents ChkEnableAutoScroll As ToolStripMenuItem
    Friend WithEvents AutomaticallyCheckForUpdates As ToolStripMenuItem
    Friend WithEvents BtnClearLog As ToolStripMenuItem
    Friend WithEvents AlertsHistory As ToolStripMenuItem
    Friend WithEvents BtnSaveLogsToDisk As ToolStripMenuItem
    Friend WithEvents BtnCheckForUpdates As ToolStripMenuItem
    Friend WithEvents SaveTimer As Timer
    Friend WithEvents ChkEnableAutoSave As ToolStripMenuItem
    Friend WithEvents DeleteOldLogsAtMidnight As ToolStripMenuItem
    Friend WithEvents BackupOldLogsAfterClearingAtMidnight As ToolStripMenuItem
    Friend WithEvents LblAutoSaved As ToolStripStatusLabel
    Friend WithEvents ChkEnableStartAtUserStartup As ToolStripMenuItem
    Friend WithEvents ChkEnableTCPSyslogServer As ToolStripMenuItem
    Friend WithEvents StartUpDelay As ToolStripMenuItem
    Friend WithEvents LblLogFileSize As ToolStripStatusLabel
    Friend WithEvents ToolTip As ToolTip
    Friend WithEvents MenuStrip As MenuStrip
    Friend WithEvents MainMenuToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents LogFunctionsToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents SettingsToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents LblSearchLabel As Label
    Friend WithEvents TxtSearchTerms As TextBox
    Friend WithEvents BtnSearch As Button
    Friend WithEvents ConfigureIgnoredWordsAndPhrasesToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents LblNumberOfIgnoredIncomingLogs As ToolStripStatusLabel
    Friend WithEvents ViewIgnoredLogsToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ClearIgnoredLogsToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents IgnoredLogsToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ChkEnableRecordingOfIgnoredLogs As ToolStripMenuItem
    Friend WithEvents BtnClearAllLogs As ToolStripMenuItem
    Friend WithEvents LogsOlderThanToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ZerooutIgnoredLogsCounterToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ViewLogBackups As ToolStripMenuItem
    Friend WithEvents ConfigureReplacementsToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ChkRegExSearch As CheckBox
    Friend WithEvents ChkCaseInsensitiveSearch As CheckBox
    Friend WithEvents Logs As DataGridView
    Friend WithEvents ColTime As DataGridViewTextBoxColumn
    Friend WithEvents colServerTime As DataGridViewTextBoxColumn
    Friend WithEvents ColIPAddress As DataGridViewTextBoxColumn
    Friend WithEvents colLogType As DataGridViewTextBoxColumn
    Friend WithEvents ColLog As DataGridViewTextBoxColumn
    Friend WithEvents ColAlerts As DataGridViewTextBoxColumn
    Friend WithEvents ColRemoteProcess As DataGridViewTextBoxColumn
    Friend WithEvents ColHostname As DataGridViewTextBoxColumn
    Friend WithEvents ColorDialog As ColorDialog
    Friend WithEvents ChangeAlternatingColorToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ChangeFont As ToolStripMenuItem
    Friend WithEvents ConfigureAlertsToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ConfigureHostnames As ToolStripMenuItem
    Friend WithEvents ColumnControls As ToolStripMenuItem
    Friend WithEvents AboutToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents CloseMe As ToolStripMenuItem
    Friend WithEvents ToolStripMenuSeparator As ToolStripSeparator
    Friend WithEvents ImportExportSettingsToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents IncludeButtonsOnNotifications As ToolStripMenuItem
    Friend WithEvents IPv6Support As ToolStripMenuItem
    Friend WithEvents ExportToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ImportToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents OpenFileDialog As OpenFileDialog
    Friend WithEvents OlderThan1DayToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents OlderThan2DaysToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents OlderThan3DaysToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents OlderThanAWeekToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents NotifyIcon As NotifyIcon
    Friend WithEvents ChkEnableConfirmCloseToolStripItem As ToolStripMenuItem
    Friend WithEvents LogsMenu As ContextMenuStrip
    Friend WithEvents CopyLogTextToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents OpenLogViewerToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents DeleteLogsToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ExportsLogsToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ExportAllLogsToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents DonationStripMenuItem As ToolStripMenuItem
    Friend WithEvents DarkMode As ToolStripMenuItem
    Friend WithEvents StopServerStripMenuItem As ToolStripMenuItem
    Friend WithEvents OpenWindowsExplorerToAppConfigFile As ToolStripMenuItem
    Friend WithEvents CreateAlertToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ChangeSyslogServerPortToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ChangeLogAutosaveIntervalToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents CreateIgnoredLogToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents CreateReplacementToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents LblItemsSelected As ToolStripStatusLabel
    Friend WithEvents ChkDeselectItemAfterMinimizingWindow As ToolStripMenuItem
    Friend WithEvents ChkShowAlertedColumn As ToolStripMenuItem
    Friend WithEvents RemoveNumbersFromRemoteApp As ToolStripMenuItem
    Friend WithEvents ShowRawLogOnLogViewer As ToolStripMenuItem
    Friend WithEvents ChkShowLogTypeColumn As ToolStripMenuItem
    Friend WithEvents ChkShowServerTimeColumn As ToolStripMenuItem
    Friend WithEvents ChkShowHostnameColumn As ToolStripMenuItem
    Friend WithEvents ConfigureSysLogMirrorServers As ToolStripMenuItem
    Friend WithEvents ConfigureTimeBetweenSameNotifications As ToolStripMenuItem
    Friend WithEvents LoadingProgressBar As ProgressBar
    Friend WithEvents MinimizeToClockTray As ToolStripMenuItem
    Friend WithEvents BackupFileNameDateFormatChooser As ToolStripMenuItem
    Friend WithEvents NotificationLength As ToolStripMenuItem
    Friend WithEvents NotificationLengthLong As ToolStripMenuItem
    Friend WithEvents NotificationLengthShort As ToolStripMenuItem
End Class
