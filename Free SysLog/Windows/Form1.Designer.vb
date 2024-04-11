<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
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
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.BtnOpenLogLocation = New System.Windows.Forms.ToolStripMenuItem()
        Me.BtnClearLog = New System.Windows.Forms.ToolStripMenuItem()
        Me.BtnClearAllLogs = New System.Windows.Forms.ToolStripMenuItem()
        Me.LogsOlderThanToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.BtnSaveLogsToDisk = New System.Windows.Forms.ToolStripMenuItem()
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.NumberOfLogs = New System.Windows.Forms.ToolStripStatusLabel()
        Me.LblAutoSaved = New System.Windows.Forms.ToolStripStatusLabel()
        Me.LblLogFileSize = New System.Windows.Forms.ToolStripStatusLabel()
        Me.LblNumberOfIgnoredIncomingLogs = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ChkAutoScroll = New System.Windows.Forms.ToolStripMenuItem()
        Me.BtnCheckForUpdates = New System.Windows.Forms.ToolStripMenuItem()
        Me.SaveTimer = New System.Windows.Forms.Timer(Me.components)
        Me.ChkAutoSave = New System.Windows.Forms.ToolStripMenuItem()
        Me.LblAutoSaveLabel = New System.Windows.Forms.Label()
        Me.NumericUpDown = New System.Windows.Forms.NumericUpDown()
        Me.ChkStartAtUserStartup = New System.Windows.Forms.ToolStripMenuItem()
        Me.BtnMoveLogFile = New System.Windows.Forms.ToolStripMenuItem()
        Me.LblSyslogServerPortLabel = New System.Windows.Forms.Label()
        Me.TxtSysLogServerPort = New System.Windows.Forms.TextBox()
        Me.ToolTip = New System.Windows.Forms.ToolTip(Me.components)
        Me.ChkRegExSearch = New System.Windows.Forms.CheckBox()
        Me.MenuStrip = New System.Windows.Forms.MenuStrip()
        Me.MainMenuToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.LogFunctionsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.IgnoredLogsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ClearIgnoredLogsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ViewIgnoredLogsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ZerooutIgnoredLogsCounterToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SettingsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ConfigureReplacementsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.IgnoredWordsAndPhrasesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ChkRecordIgnoredLogs = New System.Windows.Forms.ToolStripMenuItem()
        Me.LblSearchLabel = New System.Windows.Forms.Label()
        Me.TxtSearchTerms = New System.Windows.Forms.TextBox()
        Me.BtnSearch = New System.Windows.Forms.Button()
        Me.ChkCaseInsensitiveSearch = New System.Windows.Forms.CheckBox()
        Me.Logs = New System.Windows.Forms.DataGridView()
        Me.ColTime = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ColIPAddress = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ColLog = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.NotifyIcon = New System.Windows.Forms.NotifyIcon(Me.components)
        Me.ChkConfirmCloseToolStripItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ConfigureAlternatingColorToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.LogsMenu = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.CopyLogTextToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AboutToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.OpenLogViewerToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.DeleteLogsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExportsLogsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ImportExportSettingsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExportToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ImportToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.OlderThan1DayToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.OlderThan2DaysToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.OlderThan3DaysToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.OlderThanAWeekToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.DonationStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.StopServerStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.StatusStrip1.SuspendLayout()
        CType(Me.NumericUpDown, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.MenuStrip.SuspendLayout()
        CType(Me.Logs, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.LogsMenu.SuspendLayout()
        Me.SuspendLayout()
        '
        'BtnOpenLogLocation
        '
        Me.BtnOpenLogLocation.Name = "btnOpenLogLocation"
        Me.BtnOpenLogLocation.Size = New System.Drawing.Size(239, 22)
        Me.BtnOpenLogLocation.Text = "Open Log File Location"
        '
        'BtnClearLog
        '
        Me.BtnClearLog.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.BtnClearAllLogs, Me.LogsOlderThanToolStripMenuItem})
        Me.BtnClearLog.Enabled = False
        Me.BtnClearLog.Name = "btnClearLog"
        Me.BtnClearLog.Size = New System.Drawing.Size(239, 22)
        Me.BtnClearLog.Text = "Clear Logs"
        '
        'BtnClearAllLogs
        '
        Me.BtnClearAllLogs.Name = "btnClearAllLogs"
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
        Me.BtnSaveLogsToDisk.Name = "btnSaveLogsToDisk"
        Me.BtnSaveLogsToDisk.Size = New System.Drawing.Size(239, 22)
        Me.BtnSaveLogsToDisk.Text = "Save Logs to Disk"
        '
        'StatusStrip1
        '
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.NumberOfLogs, Me.LblAutoSaved, Me.LblLogFileSize, Me.LblNumberOfIgnoredIncomingLogs})
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 424)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New System.Drawing.Size(1175, 22)
        Me.StatusStrip1.TabIndex = 4
        Me.StatusStrip1.Text = "StatusStrip1"
        '
        'NumberOfLogs
        '
        Me.NumberOfLogs.Margin = New System.Windows.Forms.Padding(0, 3, 25, 2)
        Me.NumberOfLogs.Name = "NumberOfLogs"
        Me.NumberOfLogs.Size = New System.Drawing.Size(138, 17)
        Me.NumberOfLogs.Text = "Number of Log Entries: 0"
        '
        'LblAutoSaved
        '
        Me.LblAutoSaved.Margin = New System.Windows.Forms.Padding(0, 3, 25, 2)
        Me.LblAutoSaved.Name = "lblAutoSaved"
        Me.LblAutoSaved.Size = New System.Drawing.Size(193, 17)
        Me.LblAutoSaved.Text = "Last Auto-Saved At: (Not Specified)"
        '
        'LblLogFileSize
        '
        Me.LblLogFileSize.Margin = New System.Windows.Forms.Padding(0, 3, 25, 2)
        Me.LblLogFileSize.Name = "lblLogFileSize"
        Me.LblLogFileSize.Size = New System.Drawing.Size(156, 17)
        Me.LblLogFileSize.Text = "Log File Size: (Not Specified)"
        '
        'LblNumberOfIgnoredIncomingLogs
        '
        Me.LblNumberOfIgnoredIncomingLogs.Name = "lblNumberOfIgnoredIncomingLogs"
        Me.LblNumberOfIgnoredIncomingLogs.Size = New System.Drawing.Size(200, 17)
        Me.LblNumberOfIgnoredIncomingLogs.Text = "Number of ignored incoming logs: 0"
        '
        'ChkAutoScroll
        '
        Me.ChkAutoScroll.CheckOnClick = True
        Me.ChkAutoScroll.Name = "chkAutoScroll"
        Me.ChkAutoScroll.Size = New System.Drawing.Size(243, 22)
        Me.ChkAutoScroll.Text = "Auto Scroll"
        '
        'BtnCheckForUpdates
        '
        Me.BtnCheckForUpdates.Name = "btnCheckForUpdates"
        Me.BtnCheckForUpdates.Size = New System.Drawing.Size(171, 22)
        Me.BtnCheckForUpdates.Text = "Check for Updates"
        '
        'SaveTimer
        '
        Me.SaveTimer.Interval = 300000
        '
        'ChkAutoSave
        '
        Me.ChkAutoSave.CheckOnClick = True
        Me.ChkAutoSave.Name = "chkAutoSave"
        Me.ChkAutoSave.Size = New System.Drawing.Size(243, 22)
        Me.ChkAutoSave.Text = "Auto Save"
        '
        'LblAutoSaveLabel
        '
        Me.LblAutoSaveLabel.AutoSize = True
        Me.LblAutoSaveLabel.Location = New System.Drawing.Point(202, 28)
        Me.LblAutoSaveLabel.Name = "lblAutoSaveLabel"
        Me.LblAutoSaveLabel.Size = New System.Drawing.Size(143, 13)
        Me.LblAutoSaveLabel.TabIndex = 7
        Me.LblAutoSaveLabel.Text = "Auto save every (in minutes):"
        '
        'NumericUpDown
        '
        Me.NumericUpDown.Location = New System.Drawing.Point(351, 25)
        Me.NumericUpDown.Maximum = New Decimal(New Integer() {20, 0, 0, 0})
        Me.NumericUpDown.Name = "NumericUpDown"
        Me.NumericUpDown.Size = New System.Drawing.Size(40, 20)
        Me.NumericUpDown.TabIndex = 8
        Me.NumericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.NumericUpDown.Value = New Decimal(New Integer() {5, 0, 0, 0})
        '
        'ChkStartAtUserStartup
        '
        Me.ChkStartAtUserStartup.CheckOnClick = True
        Me.ChkStartAtUserStartup.Name = "chkStartAtUserStartup"
        Me.ChkStartAtUserStartup.Size = New System.Drawing.Size(243, 22)
        Me.ChkStartAtUserStartup.Text = "Start at Startup"
        '
        'BtnMoveLogFile
        '
        Me.BtnMoveLogFile.Name = "btnMoveLogFile"
        Me.BtnMoveLogFile.Size = New System.Drawing.Size(243, 22)
        Me.BtnMoveLogFile.Text = "Move Log File"
        '
        'LblSyslogServerPortLabel
        '
        Me.LblSyslogServerPortLabel.AutoSize = True
        Me.LblSyslogServerPortLabel.Location = New System.Drawing.Point(12, 28)
        Me.LblSyslogServerPortLabel.Name = "lblSyslogServerPortLabel"
        Me.LblSyslogServerPortLabel.Size = New System.Drawing.Size(97, 13)
        Me.LblSyslogServerPortLabel.TabIndex = 10
        Me.LblSyslogServerPortLabel.Text = "Syslog Server Port:"
        '
        'TxtSysLogServerPort
        '
        Me.TxtSysLogServerPort.Location = New System.Drawing.Point(115, 25)
        Me.TxtSysLogServerPort.Name = "txtSysLogServerPort"
        Me.TxtSysLogServerPort.Size = New System.Drawing.Size(40, 20)
        Me.TxtSysLogServerPort.TabIndex = 11
        Me.TxtSysLogServerPort.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.ToolTip.SetToolTip(Me.TxtSysLogServerPort, "Default Port: 514")
        '
        'ChkRegExSearch
        '
        Me.ChkRegExSearch.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ChkRegExSearch.AutoSize = True
        Me.ChkRegExSearch.Location = New System.Drawing.Point(934, 31)
        Me.ChkRegExSearch.Name = "chkRegExSearch"
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
        Me.MainMenuToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.BtnCheckForUpdates, Me.StopServerStripMenuItem, Me.AboutToolStripMenuItem})
        Me.MainMenuToolStripMenuItem.Name = "MainMenuToolStripMenuItem"
        Me.MainMenuToolStripMenuItem.Size = New System.Drawing.Size(80, 20)
        Me.MainMenuToolStripMenuItem.Text = "Main Menu"
        '
        'LogFunctionsToolStripMenuItem
        '
        Me.LogFunctionsToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.BtnClearLog, Me.IgnoredLogsToolStripMenuItem, Me.BtnOpenLogLocation, Me.BtnSaveLogsToDisk, Me.ZerooutIgnoredLogsCounterToolStripMenuItem})
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
        'SettingsToolStripMenuItem
        '
        Me.SettingsToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ChkAutoSave, Me.ChkAutoScroll, Me.ConfigureAlternatingColorToolStripMenuItem, Me.ConfigureReplacementsToolStripMenuItem, Me.ChkConfirmCloseToolStripItem, Me.IgnoredWordsAndPhrasesToolStripMenuItem, Me.ImportExportSettingsToolStripMenuItem, Me.BtnMoveLogFile, Me.ChkRecordIgnoredLogs, Me.ChkStartAtUserStartup})
        Me.SettingsToolStripMenuItem.Name = "SettingsToolStripMenuItem"
        Me.SettingsToolStripMenuItem.Size = New System.Drawing.Size(61, 20)
        Me.SettingsToolStripMenuItem.Text = "Settings"
        '
        'ConfigureReplacementsToolStripMenuItem
        '
        Me.ConfigureReplacementsToolStripMenuItem.Name = "ConfigureReplacementsToolStripMenuItem"
        Me.ConfigureReplacementsToolStripMenuItem.Size = New System.Drawing.Size(243, 22)
        Me.ConfigureReplacementsToolStripMenuItem.Text = "Configure Replacements"
        '
        'IgnoredWordsAndPhrasesToolStripMenuItem
        '
        Me.IgnoredWordsAndPhrasesToolStripMenuItem.Name = "IgnoredWordsAndPhrasesToolStripMenuItem"
        Me.IgnoredWordsAndPhrasesToolStripMenuItem.Size = New System.Drawing.Size(243, 22)
        Me.IgnoredWordsAndPhrasesToolStripMenuItem.Text = "Ignored Words and Phrases"
        '
        'ChkRecordIgnoredLogs
        '
        Me.ChkRecordIgnoredLogs.CheckOnClick = True
        Me.ChkRecordIgnoredLogs.Name = "chkRecordIgnoredLogs"
        Me.ChkRecordIgnoredLogs.Size = New System.Drawing.Size(243, 22)
        Me.ChkRecordIgnoredLogs.Text = "Record Ignored Logs"
        '
        'LblSearchLabel
        '
        Me.LblSearchLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.LblSearchLabel.AutoSize = True
        Me.LblSearchLabel.Location = New System.Drawing.Point(707, 32)
        Me.LblSearchLabel.Name = "lblSearchLabel"
        Me.LblSearchLabel.Size = New System.Drawing.Size(67, 13)
        Me.LblSearchLabel.TabIndex = 13
        Me.LblSearchLabel.Text = "Search Logs"
        '
        'TxtSearchTerms
        '
        Me.TxtSearchTerms.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TxtSearchTerms.Location = New System.Drawing.Point(780, 29)
        Me.TxtSearchTerms.Name = "txtSearchTerms"
        Me.TxtSearchTerms.Size = New System.Drawing.Size(148, 20)
        Me.TxtSearchTerms.TabIndex = 14
        '
        'BtnSearch
        '
        Me.BtnSearch.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.BtnSearch.Location = New System.Drawing.Point(1111, 27)
        Me.BtnSearch.Name = "btnSearch"
        Me.BtnSearch.Size = New System.Drawing.Size(52, 23)
        Me.BtnSearch.TabIndex = 15
        Me.BtnSearch.Text = "Search"
        Me.BtnSearch.UseVisualStyleBackColor = True
        '
        'ChkCaseInsensitiveSearch
        '
        Me.ChkCaseInsensitiveSearch.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ChkCaseInsensitiveSearch.AutoSize = True
        Me.ChkCaseInsensitiveSearch.Checked = True
        Me.ChkCaseInsensitiveSearch.CheckState = System.Windows.Forms.CheckState.Checked
        Me.ChkCaseInsensitiveSearch.Location = New System.Drawing.Point(1003, 31)
        Me.ChkCaseInsensitiveSearch.Name = "chkCaseInsensitiveSearch"
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
        Me.Logs.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.Logs.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.ColTime, Me.ColIPAddress, Me.ColLog})
        Me.Logs.ContextMenuStrip = Me.LogsMenu
        Me.Logs.Location = New System.Drawing.Point(12, 52)
        Me.Logs.Name = "logs"
        Me.Logs.ReadOnly = True
        Me.Logs.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.Logs.Size = New System.Drawing.Size(1151, 369)
        Me.Logs.TabIndex = 18
        '
        'ColTime
        '
        Me.ColTime.HeaderText = "Time"
        Me.ColTime.Name = "colTime"
        Me.ColTime.ReadOnly = True
        Me.ColTime.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic
        '
        'ColIPAddress
        '
        Me.ColIPAddress.HeaderText = "IP Address"
        Me.ColIPAddress.Name = "colIPAddress"
        Me.ColIPAddress.ReadOnly = True
        '
        'ColLog
        '
        Me.ColLog.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        Me.ColLog.HeaderText = "Log"
        Me.ColLog.Name = "colLog"
        Me.ColLog.ReadOnly = True
        '
        'ConfigureAlternatingColorToolStripMenuItem
        '
        Me.ConfigureAlternatingColorToolStripMenuItem.Name = "ConfigureAlternatingColorToolStripMenuItem"
        Me.ConfigureAlternatingColorToolStripMenuItem.Size = New System.Drawing.Size(243, 22)
        Me.ConfigureAlternatingColorToolStripMenuItem.Text = "Configure Alternating Color"
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
        Me.ImportExportSettingsToolStripMenuItem.Size = New System.Drawing.Size(243, 22)
        Me.ImportExportSettingsToolStripMenuItem.Text = "Import/Export Program Settings"
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
        'ChkConfirmCloseToolStripItem
        '
        Me.ChkConfirmCloseToolStripItem.CheckOnClick = True
        Me.ChkConfirmCloseToolStripItem.Name = "chkConfirmCloseToolStripItem"
        Me.ChkConfirmCloseToolStripItem.Size = New System.Drawing.Size(243, 22)
        Me.ChkConfirmCloseToolStripItem.Text = "Confirm Close"
        '
        'LogsMenu
        '
        Me.LogsMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.CopyLogTextToolStripMenuItem, Me.DeleteLogsToolStripMenuItem, Me.ExportsLogsToolStripMenuItem, Me.OpenLogViewerToolStripMenuItem})
        Me.LogsMenu.Name = "LogsMenu"
        Me.LogsMenu.Size = New System.Drawing.Size(165, 48)
        '
        'CopyLogTextToolStripMenuItem
        '
        Me.CopyLogTextToolStripMenuItem.Name = "CopyLogTextToolStripMenuItem"
        Me.CopyLogTextToolStripMenuItem.Size = New System.Drawing.Size(164, 22)
        Me.CopyLogTextToolStripMenuItem.Text = "Copy Log Text"
        '
        'OpenLogViewerToolStripMenuItem
        '
        Me.OpenLogViewerToolStripMenuItem.Name = "OpenLogViewerToolStripMenuItem"
        Me.OpenLogViewerToolStripMenuItem.Size = New System.Drawing.Size(164, 22)
        Me.OpenLogViewerToolStripMenuItem.Text = "Open Log Viewer"
        '
        'DeleteLogsToolStripMenuItem
        '
        Me.DeleteLogsToolStripMenuItem.Name = "DeleteLogsToolStripMenuItem"
        Me.DeleteLogsToolStripMenuItem.Size = New System.Drawing.Size(164, 22)
        Me.DeleteLogsToolStripMenuItem.Text = "Delete Selected Logs"
        '
        'ExportsLogsToolStripMenuItem
        '
        Me.ExportsLogsToolStripMenuItem.Name = "ExportsLogsToolStripMenuItem"
        Me.ExportsLogsToolStripMenuItem.Size = New System.Drawing.Size(164, 22)
        Me.ExportsLogsToolStripMenuItem.Text = "Exports Selected Logs"
        '
        ' DonationStripMenuItem
        '
        Me.DonationStripMenuItem.Name = "DonationStripMenuItem"
        Me.DonationStripMenuItem.Size = New System.Drawing.Size(164, 22)
        Me.DonationStripMenuItem.Text = "Donate via PayPal"
        '
        'StopServerStripMenuItem
        '
        Me.StopServerStripMenuItem.Name = "StopServerStripMenuItem"
        Me.StopServerStripMenuItem.Size = New System.Drawing.Size(164, 22)
        Me.StopServerStripMenuItem.Text = "Stop Server"
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1175, 446)
        Me.Controls.Add(Me.Logs)
        Me.Controls.Add(Me.ChkCaseInsensitiveSearch)
        Me.Controls.Add(Me.ChkRegExSearch)
        Me.Controls.Add(Me.BtnSearch)
        Me.Controls.Add(Me.TxtSearchTerms)
        Me.Controls.Add(Me.LblSearchLabel)
        Me.Controls.Add(Me.NumericUpDown)
        Me.Controls.Add(Me.LblAutoSaveLabel)
        Me.Controls.Add(Me.StatusStrip1)
        Me.Controls.Add(Me.MenuStrip)
        Me.Controls.Add(Me.TxtSysLogServerPort)
        Me.Controls.Add(Me.LblSyslogServerPortLabel)
        Me.MainMenuStrip = Me.MenuStrip
        Me.MinimumSize = New System.Drawing.Size(1191, 485)
        Me.Name = "Form1"
        Me.Text = "Free SysLog Server"
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        CType(Me.NumericUpDown, System.ComponentModel.ISupportInitialize).EndInit()
        Me.MenuStrip.ResumeLayout(False)
        Me.MenuStrip.PerformLayout()
        CType(Me.Logs, System.ComponentModel.ISupportInitialize).EndInit()
        Me.LogsMenu.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents BtnOpenLogLocation As ToolStripMenuItem
    Friend WithEvents SaveFileDialog As SaveFileDialog
    Friend WithEvents StatusStrip1 As StatusStrip
    Friend WithEvents NumberOfLogs As ToolStripStatusLabel
    Friend WithEvents ChkAutoScroll As ToolStripMenuItem
    Friend WithEvents BtnClearLog As ToolStripMenuItem
    Friend WithEvents BtnSaveLogsToDisk As ToolStripMenuItem
    Friend WithEvents BtnCheckForUpdates As ToolStripMenuItem
    Friend WithEvents SaveTimer As Timer
    Friend WithEvents ChkAutoSave As ToolStripMenuItem
    Friend WithEvents LblAutoSaveLabel As Label
    Friend WithEvents NumericUpDown As NumericUpDown
    Friend WithEvents LblAutoSaved As ToolStripStatusLabel
    Friend WithEvents ChkStartAtUserStartup As ToolStripMenuItem
    Friend WithEvents LblLogFileSize As ToolStripStatusLabel
    Friend WithEvents BtnMoveLogFile As ToolStripMenuItem
    Friend WithEvents LblSyslogServerPortLabel As Label
    Friend WithEvents TxtSysLogServerPort As TextBox
    Friend WithEvents ToolTip As ToolTip
    Friend WithEvents MenuStrip As MenuStrip
    Friend WithEvents MainMenuToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents LogFunctionsToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents SettingsToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents LblSearchLabel As Label
    Friend WithEvents TxtSearchTerms As TextBox
    Friend WithEvents BtnSearch As Button
    Friend WithEvents IgnoredWordsAndPhrasesToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents LblNumberOfIgnoredIncomingLogs As ToolStripStatusLabel
    Friend WithEvents ViewIgnoredLogsToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ClearIgnoredLogsToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents IgnoredLogsToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ChkRecordIgnoredLogs As ToolStripMenuItem
    Friend WithEvents BtnClearAllLogs As ToolStripMenuItem
    Friend WithEvents LogsOlderThanToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ZerooutIgnoredLogsCounterToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ConfigureReplacementsToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ChkRegExSearch As CheckBox
    Friend WithEvents ChkCaseInsensitiveSearch As CheckBox
    Friend WithEvents Logs As DataGridView
    Friend WithEvents ColTime As DataGridViewTextBoxColumn
    Friend WithEvents ColIPAddress As DataGridViewTextBoxColumn
    Friend WithEvents ColLog As DataGridViewTextBoxColumn
    Friend WithEvents ColorDialog As ColorDialog
    Friend WithEvents ConfigureAlternatingColorToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents AboutToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ImportExportSettingsToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ExportToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ImportToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents OpenFileDialog As OpenFileDialog
    Friend WithEvents OlderThan1DayToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents OlderThan2DaysToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents OlderThan3DaysToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents OlderThanAWeekToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents NotifyIcon As NotifyIcon
    Friend WithEvents ChkConfirmCloseToolStripItem As ToolStripMenuItem
    Friend WithEvents LogsMenu As ContextMenuStrip
    Friend WithEvents CopyLogTextToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents OpenLogViewerToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents DeleteLogsToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ExportsLogsToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents DonationStripMenuItem As ToolStripMenuItem
    Friend WithEvents StopServerStripMenuItem As ToolStripMenuItem
End Class
