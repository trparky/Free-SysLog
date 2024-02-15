﻿<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
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
        Me.btnServerController = New System.Windows.Forms.ToolStripMenuItem()
        Me.btnOpenLogLocation = New System.Windows.Forms.ToolStripMenuItem()
        Me.btnClearLog = New System.Windows.Forms.ToolStripMenuItem()
        Me.btnSaveLogsToDisk = New System.Windows.Forms.ToolStripMenuItem()
        Me.logs = New System.Windows.Forms.ListView()
        Me.Time = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.Type = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.IPAddressCol = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.Log = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.SaveFileDialog = New System.Windows.Forms.SaveFileDialog()
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.NumberOfLogs = New System.Windows.Forms.ToolStripStatusLabel()
        Me.lblAutoSaved = New System.Windows.Forms.ToolStripStatusLabel()
        Me.lblLogFileSize = New System.Windows.Forms.ToolStripStatusLabel()
        Me.chkAutoScroll = New System.Windows.Forms.ToolStripMenuItem()
        Me.btnCheckForUpdates = New System.Windows.Forms.ToolStripMenuItem()
        Me.SaveTimer = New System.Windows.Forms.Timer(Me.components)
        Me.chkAutoSave = New System.Windows.Forms.ToolStripMenuItem()
        Me.lblAutoSaveLabel = New System.Windows.Forms.Label()
        Me.NumericUpDown = New System.Windows.Forms.NumericUpDown()
        Me.chkStartAtUserStartup = New System.Windows.Forms.ToolStripMenuItem()
        Me.btnMoveLogFile = New System.Windows.Forms.ToolStripMenuItem()
        Me.lblSyslogServerPortLabel = New System.Windows.Forms.Label()
        Me.txtSysLogServerPort = New System.Windows.Forms.TextBox()
        Me.ToolTip = New System.Windows.Forms.ToolTip(Me.components)
        Me.MenuStrip = New System.Windows.Forms.MenuStrip()
        Me.MainMenuToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.LogFunctionsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SettingsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.chkRecordIgnoredLogs = New System.Windows.Forms.ToolStripMenuItem()
        Me.lblSearchLabel = New System.Windows.Forms.Label()
        Me.txtSearchTerms = New System.Windows.Forms.TextBox()
        Me.btnSearch = New System.Windows.Forms.Button()
        Me.ChangeSearchHighlightColorToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ColorDialog = New System.Windows.Forms.ColorDialog()
        Me.ClearIgnoredLogsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SelectedHeader = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.IgnoredWordsAndPhrasesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.lblNumberOfIgnoredIncomingLogs = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ViewIgnoredLogsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.IgnoredLogsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.StatusStrip1.SuspendLayout()
        CType(Me.NumericUpDown, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.MenuStrip.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnServerController
        '
        Me.btnServerController.Name = "btnServerController"
        Me.btnServerController.Size = New System.Drawing.Size(173, 22)
        Me.btnServerController.Text = "Stop SysLog Server"
        '
        'btnOpenLogLocation
        '
        Me.btnOpenLogLocation.Name = "btnOpenLogLocation"
        Me.btnOpenLogLocation.Size = New System.Drawing.Size(218, 22)
        Me.btnOpenLogLocation.Text = "Open Log File Location"
        '
        'btnClearLog
        '
        Me.btnClearLog.Name = "btnClearLog"
        Me.btnClearLog.Size = New System.Drawing.Size(218, 22)
        Me.btnClearLog.Text = "Clear Logs"
        '
        'btnSaveLogsToDisk
        '
        Me.btnSaveLogsToDisk.Enabled = False
        Me.btnSaveLogsToDisk.Name = "btnSaveLogsToDisk"
        Me.btnSaveLogsToDisk.Size = New System.Drawing.Size(218, 22)
        Me.btnSaveLogsToDisk.Text = "Save Logs to Disk"
        '
        'logs
        '
        Me.logs.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.logs.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.Time, Me.Type, Me.IPAddressCol, Me.Log, Me.SelectedHeader})
        Me.logs.FullRowSelect = True
        Me.logs.HideSelection = False
        Me.logs.Location = New System.Drawing.Point(12, 52)
        Me.logs.Name = "logs"
        Me.logs.Size = New System.Drawing.Size(1151, 369)
        Me.logs.TabIndex = 3
        Me.logs.UseCompatibleStateImageBehavior = False
        Me.logs.View = System.Windows.Forms.View.Details
        '
        'Time
        '
        Me.Time.Text = "Time"
        Me.Time.Width = 196
        '
        'Type
        '
        Me.Type.Text = "Type"
        Me.Type.Width = 110
        '
        'IPAddressCol
        '
        Me.IPAddressCol.Text = "IP Address"
        Me.IPAddressCol.Width = 102
        '
        'Log
        '
        Me.Log.Text = "Log"
        Me.Log.Width = 670
        '
        'StatusStrip1
        '
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.NumberOfLogs, Me.lblAutoSaved, Me.lblLogFileSize, Me.lblNumberOfIgnoredIncomingLogs})
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
        'chkAutoScroll
        '
        Me.chkAutoScroll.CheckOnClick = True
        Me.chkAutoScroll.Name = "chkAutoScroll"
        Me.chkAutoScroll.Size = New System.Drawing.Size(238, 22)
        Me.chkAutoScroll.Text = "Auto Scroll"
        '
        'btnCheckForUpdates
        '
        Me.btnCheckForUpdates.Name = "btnCheckForUpdates"
        Me.btnCheckForUpdates.Size = New System.Drawing.Size(173, 22)
        Me.btnCheckForUpdates.Text = "Check for Updates"
        '
        'SaveTimer
        '
        Me.SaveTimer.Interval = 300000
        '
        'chkAutoSave
        '
        Me.chkAutoSave.CheckOnClick = True
        Me.chkAutoSave.Name = "chkAutoSave"
        Me.chkAutoSave.Size = New System.Drawing.Size(238, 22)
        Me.chkAutoSave.Text = "Auto Save"
        '
        'lblAutoSaveLabel
        '
        Me.lblAutoSaveLabel.AutoSize = True
        Me.lblAutoSaveLabel.Location = New System.Drawing.Point(202, 28)
        Me.lblAutoSaveLabel.Name = "lblAutoSaveLabel"
        Me.lblAutoSaveLabel.Size = New System.Drawing.Size(143, 13)
        Me.lblAutoSaveLabel.TabIndex = 7
        Me.lblAutoSaveLabel.Text = "Auto save every (in minutes):"
        '
        'NumericUpDown
        '
        Me.NumericUpDown.Location = New System.Drawing.Point(351, 26)
        Me.NumericUpDown.Maximum = New Decimal(New Integer() {20, 0, 0, 0})
        Me.NumericUpDown.Name = "NumericUpDown"
        Me.NumericUpDown.Size = New System.Drawing.Size(40, 20)
        Me.NumericUpDown.TabIndex = 8
        Me.NumericUpDown.Value = New Decimal(New Integer() {5, 0, 0, 0})
        '
        'lblAutoSaved
        '
        Me.lblAutoSaved.Margin = New System.Windows.Forms.Padding(0, 3, 25, 2)
        Me.lblAutoSaved.Name = "lblAutoSaved"
        Me.lblAutoSaved.Size = New System.Drawing.Size(193, 17)
        Me.lblAutoSaved.Text = "Last Auto-Saved At: (Not Specified)"
        '
        'chkStartAtUserStartup
        '
        Me.chkStartAtUserStartup.CheckOnClick = True
        Me.chkStartAtUserStartup.Name = "chkStartAtUserStartup"
        Me.chkStartAtUserStartup.Size = New System.Drawing.Size(238, 22)
        Me.chkStartAtUserStartup.Text = "Start at Startup"
        '
        'lblLogFileSize
        '
        Me.lblLogFileSize.Margin = New System.Windows.Forms.Padding(0, 3, 25, 2)
        Me.lblLogFileSize.Name = "lblLogFileSize"
        Me.lblLogFileSize.Size = New System.Drawing.Size(156, 17)
        Me.lblLogFileSize.Text = "Log File Size: (Not Specified)"
        '
        'btnMoveLogFile
        '
        Me.btnMoveLogFile.Name = "btnMoveLogFile"
        Me.btnMoveLogFile.Size = New System.Drawing.Size(218, 22)
        Me.btnMoveLogFile.Text = "Move Log File"
        '
        'lblSyslogServerPortLabel
        '
        Me.lblSyslogServerPortLabel.AutoSize = True
        Me.lblSyslogServerPortLabel.Location = New System.Drawing.Point(12, 28)
        Me.lblSyslogServerPortLabel.Name = "lblSyslogServerPortLabel"
        Me.lblSyslogServerPortLabel.Size = New System.Drawing.Size(97, 13)
        Me.lblSyslogServerPortLabel.TabIndex = 10
        Me.lblSyslogServerPortLabel.Text = "Syslog Server Port:"
        '
        'txtSysLogServerPort
        '
        Me.txtSysLogServerPort.Location = New System.Drawing.Point(115, 25)
        Me.txtSysLogServerPort.Name = "txtSysLogServerPort"
        Me.txtSysLogServerPort.Size = New System.Drawing.Size(40, 20)
        Me.txtSysLogServerPort.TabIndex = 11
        Me.ToolTip.SetToolTip(Me.txtSysLogServerPort, "Default Port: 514")
        '
        'MenuStrip
        '
        Me.MenuStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MainMenuToolStripMenuItem, Me.LogFunctionsToolStripMenuItem, Me.SettingsToolStripMenuItem})
        Me.MenuStrip.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip.Name = "MenuStrip"
        Me.MenuStrip.Size = New System.Drawing.Size(1175, 24)
        Me.MenuStrip.TabIndex = 12
        Me.MenuStrip.Text = "MenuStrip1"
        '
        'MainMenuToolStripMenuItem
        '
        Me.MainMenuToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.btnCheckForUpdates, Me.btnServerController})
        Me.MainMenuToolStripMenuItem.Name = "MainMenuToolStripMenuItem"
        Me.MainMenuToolStripMenuItem.Size = New System.Drawing.Size(80, 20)
        Me.MainMenuToolStripMenuItem.Text = "Main Menu"
        '
        'LogFunctionsToolStripMenuItem
        '
        Me.LogFunctionsToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.btnClearLog, Me.IgnoredLogsToolStripMenuItem, Me.IgnoredWordsAndPhrasesToolStripMenuItem, Me.btnMoveLogFile, Me.btnOpenLogLocation, Me.btnSaveLogsToDisk})
        Me.LogFunctionsToolStripMenuItem.Name = "LogFunctionsToolStripMenuItem"
        Me.LogFunctionsToolStripMenuItem.Size = New System.Drawing.Size(94, 20)
        Me.LogFunctionsToolStripMenuItem.Text = "Log Functions"
        '
        'IgnoredLogsToolStripMenuItem
        '
        Me.IgnoredLogsToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ClearIgnoredLogsToolStripMenuItem, Me.ViewIgnoredLogsToolStripMenuItem})
        Me.IgnoredLogsToolStripMenuItem.Enabled = False
        Me.IgnoredLogsToolStripMenuItem.Name = "IgnoredLogsToolStripMenuItem"
        Me.IgnoredLogsToolStripMenuItem.Size = New System.Drawing.Size(218, 22)
        Me.IgnoredLogsToolStripMenuItem.Text = "Ignored Logs"
        '
        'SettingsToolStripMenuItem
        '
        Me.SettingsToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ChangeSearchHighlightColorToolStripMenuItem, Me.chkAutoSave, Me.chkAutoScroll, Me.chkRecordIgnoredLogs, Me.chkStartAtUserStartup})
        Me.SettingsToolStripMenuItem.Name = "SettingsToolStripMenuItem"
        Me.SettingsToolStripMenuItem.Size = New System.Drawing.Size(61, 20)
        Me.SettingsToolStripMenuItem.Text = "Settings"
        '
        'chkRecordIgnoredLogs
        '
        Me.chkRecordIgnoredLogs.CheckOnClick = True
        Me.chkRecordIgnoredLogs.Name = "RecordIgnoredLogsToolStripMenuItem"
        Me.chkRecordIgnoredLogs.Size = New System.Drawing.Size(238, 22)
        Me.chkRecordIgnoredLogs.Text = "Record Ignored Logs"
        '
        'lblSearchLabel
        '
        Me.lblSearchLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblSearchLabel.AutoSize = True
        Me.lblSearchLabel.Location = New System.Drawing.Point(836, 32)
        Me.lblSearchLabel.Name = "lblSearchLabel"
        Me.lblSearchLabel.Size = New System.Drawing.Size(67, 13)
        Me.lblSearchLabel.TabIndex = 13
        Me.lblSearchLabel.Text = "Search Logs"
        '
        'txtSearchTerms
        '
        Me.txtSearchTerms.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtSearchTerms.Location = New System.Drawing.Point(909, 29)
        Me.txtSearchTerms.Name = "txtSearchTerms"
        Me.txtSearchTerms.Size = New System.Drawing.Size(148, 20)
        Me.txtSearchTerms.TabIndex = 14
        '
        'btnSearch
        '
        Me.btnSearch.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSearch.Location = New System.Drawing.Point(1063, 27)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(100, 23)
        Me.btnSearch.TabIndex = 15
        Me.btnSearch.Text = "Search"
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        'SelectedHeader
        '
        Me.SelectedHeader.Text = "*"
        Me.SelectedHeader.Width = 20
        '
        'IgnoredWordsAndPhrasesToolStripMenuItem
        '
        Me.IgnoredWordsAndPhrasesToolStripMenuItem.Name = "IgnoredWordsAndPhrasesToolStripMenuItem"
        Me.IgnoredWordsAndPhrasesToolStripMenuItem.Size = New System.Drawing.Size(218, 22)
        Me.IgnoredWordsAndPhrasesToolStripMenuItem.Text = "Ignored Words and Phrases"
        '
        'lblNumberOfIgnoredIncomingLogs
        '
        Me.lblNumberOfIgnoredIncomingLogs.Name = "lblNumberOfIgnoredIncomingLogs"
        Me.lblNumberOfIgnoredIncomingLogs.Size = New System.Drawing.Size(200, 17)
        Me.lblNumberOfIgnoredIncomingLogs.Text = "Number of ignored incoming logs: 0"
        '
        'ViewIgnoredLogsToolStripMenuItem
        '
        Me.ViewIgnoredLogsToolStripMenuItem.Name = "ViewIgnoredLogsToolStripMenuItem"
        Me.ViewIgnoredLogsToolStripMenuItem.Size = New System.Drawing.Size(101, 22)
        Me.ViewIgnoredLogsToolStripMenuItem.Text = "View"
        '
        'ClearIgnoredLogsToolStripMenuItem
        '
        Me.ClearIgnoredLogsToolStripMenuItem.Name = "ClearIgnoredLogsToolStripMenuItem"
        Me.ClearIgnoredLogsToolStripMenuItem.Size = New System.Drawing.Size(101, 22)
        Me.ClearIgnoredLogsToolStripMenuItem.Text = "Clear"
        '
        'ChangeSearchHighlightColorToolStripMenuItem
        '
        Me.ChangeSearchHighlightColorToolStripMenuItem.Name = "ChangeSearchHighlightColorToolStripMenuItem"
        Me.ChangeSearchHighlightColorToolStripMenuItem.Size = New System.Drawing.Size(238, 22)
        Me.ChangeSearchHighlightColorToolStripMenuItem.Text = "Change Search Highlight Color"
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1175, 446)
        Me.Controls.Add(Me.btnSearch)
        Me.Controls.Add(Me.txtSearchTerms)
        Me.Controls.Add(Me.lblSearchLabel)
        Me.Controls.Add(Me.NumericUpDown)
        Me.Controls.Add(Me.lblAutoSaveLabel)
        Me.Controls.Add(Me.StatusStrip1)
        Me.Controls.Add(Me.MenuStrip)
        Me.Controls.Add(Me.logs)
        Me.Controls.Add(Me.txtSysLogServerPort)
        Me.Controls.Add(Me.lblSyslogServerPortLabel)
        Me.MainMenuStrip = Me.MenuStrip
        Me.Name = "Form1"
        Me.Text = "Free SysLog Server"
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        CType(Me.NumericUpDown, System.ComponentModel.ISupportInitialize).EndInit()
        Me.MenuStrip.ResumeLayout(False)
        Me.MenuStrip.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnServerController As ToolStripMenuItem
    Friend WithEvents btnOpenLogLocation As ToolStripMenuItem
    Friend WithEvents logs As System.Windows.Forms.ListView
    Friend WithEvents Time As System.Windows.Forms.ColumnHeader
    Friend WithEvents Type As System.Windows.Forms.ColumnHeader
    Friend WithEvents IPAddressCol As System.Windows.Forms.ColumnHeader
    Friend WithEvents Log As System.Windows.Forms.ColumnHeader
    Friend WithEvents SaveFileDialog As SaveFileDialog
    Friend WithEvents StatusStrip1 As StatusStrip
    Friend WithEvents NumberOfLogs As ToolStripStatusLabel
    Friend WithEvents chkAutoScroll As ToolStripMenuItem
    Friend WithEvents btnClearLog As ToolStripMenuItem
    Friend WithEvents btnSaveLogsToDisk As ToolStripMenuItem
    Friend WithEvents btnCheckForUpdates As ToolStripMenuItem
    Friend WithEvents SaveTimer As Timer
    Friend WithEvents chkAutoSave As ToolStripMenuItem
    Friend WithEvents lblAutoSaveLabel As Label
    Friend WithEvents NumericUpDown As NumericUpDown
    Friend WithEvents lblAutoSaved As ToolStripStatusLabel
    Friend WithEvents chkStartAtUserStartup As ToolStripMenuItem
    Friend WithEvents lblLogFileSize As ToolStripStatusLabel
    Friend WithEvents btnMoveLogFile As ToolStripMenuItem
    Friend WithEvents lblSyslogServerPortLabel As Label
    Friend WithEvents txtSysLogServerPort As TextBox
    Friend WithEvents ToolTip As ToolTip
    Friend WithEvents MenuStrip As MenuStrip
    Friend WithEvents MainMenuToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents LogFunctionsToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents SettingsToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents lblSearchLabel As Label
    Friend WithEvents txtSearchTerms As TextBox
    Friend WithEvents btnSearch As Button
    Friend WithEvents SelectedHeader As ColumnHeader
    Friend WithEvents IgnoredWordsAndPhrasesToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents lblNumberOfIgnoredIncomingLogs As ToolStripStatusLabel
    Friend WithEvents ViewIgnoredLogsToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ClearIgnoredLogsToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ChangeSearchHighlightColorToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ColorDialog As ColorDialog
    Friend WithEvents IgnoredLogsToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents chkRecordIgnoredLogs As ToolStripMenuItem
End Class
