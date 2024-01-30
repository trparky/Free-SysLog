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
        Me.btnServerController = New System.Windows.Forms.Button()
        Me.btnOpenLogLocation = New System.Windows.Forms.Button()
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.btnClearLog = New System.Windows.Forms.Button()
        Me.btnSaveLogsToDisk = New System.Windows.Forms.Button()
        Me.logs = New System.Windows.Forms.ListView()
        Me.Time = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.Type = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.IPAddressCol = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.Log = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.SaveFileDialog = New System.Windows.Forms.SaveFileDialog()
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.NumberOfLogs = New System.Windows.Forms.ToolStripStatusLabel()
        Me.chkAutoScroll = New System.Windows.Forms.CheckBox()
        Me.btnCheckForUpdates = New System.Windows.Forms.Button()
        Me.SaveTimer = New System.Windows.Forms.Timer(Me.components)
        Me.chkAutoSave = New System.Windows.Forms.CheckBox()
        Me.lblAutoSaveLabel = New System.Windows.Forms.Label()
        Me.NumericUpDown = New System.Windows.Forms.NumericUpDown()
        Me.lblAutoSaved = New System.Windows.Forms.ToolStripStatusLabel()
        Me.chkStartAtUserStartup = New System.Windows.Forms.CheckBox()
        Me.lblLogFileSize = New System.Windows.Forms.ToolStripStatusLabel()
        Me.btnMoveLogFile = New System.Windows.Forms.Button()
        Me.lblSyslogServerPortLabel = New System.Windows.Forms.Label()
        Me.txtSysLogServerPort = New System.Windows.Forms.TextBox()
        Me.ToolTip = New System.Windows.Forms.ToolTip(Me.components)
        Me.TableLayoutPanel1.SuspendLayout()
        Me.StatusStrip1.SuspendLayout()
        CType(Me.NumericUpDown, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'btnServerController
        '
        Me.btnServerController.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnServerController.Location = New System.Drawing.Point(3, 3)
        Me.btnServerController.Name = "btnServerController"
        Me.btnServerController.Size = New System.Drawing.Size(185, 31)
        Me.btnServerController.TabIndex = 0
        Me.btnServerController.Text = "Stop SysLog Server"
        Me.btnServerController.UseVisualStyleBackColor = True
        '
        'btnOpenLogLocation
        '
        Me.btnOpenLogLocation.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnOpenLogLocation.Location = New System.Drawing.Point(194, 3)
        Me.btnOpenLogLocation.Name = "btnOpenLogLocation"
        Me.btnOpenLogLocation.Size = New System.Drawing.Size(185, 31)
        Me.btnOpenLogLocation.TabIndex = 1
        Me.btnOpenLogLocation.Text = "Open Log File Location"
        Me.btnOpenLogLocation.UseVisualStyleBackColor = True
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel1.ColumnCount = 6
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667!))
        Me.TableLayoutPanel1.Controls.Add(Me.btnServerController, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.btnCheckForUpdates, 5, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.btnSaveLogsToDisk, 4, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.btnClearLog, 3, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.btnOpenLogLocation, 1, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.btnMoveLogFile, 2, 0)
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(12, 12)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 1
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(1151, 37)
        Me.TableLayoutPanel1.TabIndex = 2
        '
        'btnClearLog
        '
        Me.btnClearLog.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClearLog.Location = New System.Drawing.Point(576, 3)
        Me.btnClearLog.Name = "btnClearLog"
        Me.btnClearLog.Size = New System.Drawing.Size(185, 31)
        Me.btnClearLog.TabIndex = 2
        Me.btnClearLog.Text = "Clear Logs"
        Me.btnClearLog.UseVisualStyleBackColor = True
        '
        'btnSaveLogsToDisk
        '
        Me.btnSaveLogsToDisk.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSaveLogsToDisk.Enabled = False
        Me.btnSaveLogsToDisk.Location = New System.Drawing.Point(767, 3)
        Me.btnSaveLogsToDisk.Name = "btnSaveLogsToDisk"
        Me.btnSaveLogsToDisk.Size = New System.Drawing.Size(185, 31)
        Me.btnSaveLogsToDisk.TabIndex = 3
        Me.btnSaveLogsToDisk.Text = "Save Logs to Disk"
        Me.btnSaveLogsToDisk.UseVisualStyleBackColor = True
        '
        'logs
        '
        Me.logs.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.logs.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.Time, Me.Type, Me.IPAddressCol, Me.Log})
        Me.logs.FullRowSelect = True
        Me.logs.HideSelection = False
        Me.logs.Location = New System.Drawing.Point(12, 78)
        Me.logs.Name = "logs"
        Me.logs.Size = New System.Drawing.Size(1151, 343)
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
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.NumberOfLogs, Me.lblAutoSaved, Me.lblLogFileSize})
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
        Me.chkAutoScroll.AutoSize = True
        Me.chkAutoScroll.Location = New System.Drawing.Point(12, 55)
        Me.chkAutoScroll.Name = "chkAutoScroll"
        Me.chkAutoScroll.Size = New System.Drawing.Size(77, 17)
        Me.chkAutoScroll.TabIndex = 5
        Me.chkAutoScroll.Text = "Auto Scroll"
        Me.chkAutoScroll.UseVisualStyleBackColor = True
        '
        'btnCheckForUpdates
        '
        Me.btnCheckForUpdates.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCheckForUpdates.Location = New System.Drawing.Point(958, 3)
        Me.btnCheckForUpdates.Name = "btnCheckForUpdates"
        Me.btnCheckForUpdates.Size = New System.Drawing.Size(190, 31)
        Me.btnCheckForUpdates.TabIndex = 4
        Me.btnCheckForUpdates.Text = "Check for Updates"
        Me.btnCheckForUpdates.UseVisualStyleBackColor = True
        '
        'SaveTimer
        '
        Me.SaveTimer.Interval = 300000
        '
        'chkAutoSave
        '
        Me.chkAutoSave.AutoSize = True
        Me.chkAutoSave.Location = New System.Drawing.Point(198, 55)
        Me.chkAutoSave.Name = "chkAutoSave"
        Me.chkAutoSave.Size = New System.Drawing.Size(76, 17)
        Me.chkAutoSave.TabIndex = 6
        Me.chkAutoSave.Text = "Auto Save"
        Me.chkAutoSave.UseVisualStyleBackColor = True
        '
        'lblAutoSaveLabel
        '
        Me.lblAutoSaveLabel.AutoSize = True
        Me.lblAutoSaveLabel.Location = New System.Drawing.Point(280, 56)
        Me.lblAutoSaveLabel.Name = "lblAutoSaveLabel"
        Me.lblAutoSaveLabel.Size = New System.Drawing.Size(143, 13)
        Me.lblAutoSaveLabel.TabIndex = 7
        Me.lblAutoSaveLabel.Text = "Auto save every (in minutes):"
        '
        'NumericUpDown
        '
        Me.NumericUpDown.Location = New System.Drawing.Point(429, 54)
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
        Me.chkStartAtUserStartup.AutoSize = True
        Me.chkStartAtUserStartup.Location = New System.Drawing.Point(95, 55)
        Me.chkStartAtUserStartup.Name = "chkStartAtUserStartup"
        Me.chkStartAtUserStartup.Size = New System.Drawing.Size(97, 17)
        Me.chkStartAtUserStartup.TabIndex = 9
        Me.chkStartAtUserStartup.Text = "Start at Startup"
        Me.chkStartAtUserStartup.UseVisualStyleBackColor = True
        '
        'lblLogFileSize
        '
        Me.lblLogFileSize.Name = "lblLogFileSize"
        Me.lblLogFileSize.Size = New System.Drawing.Size(156, 17)
        Me.lblLogFileSize.Text = "Log File Size: (Not Specified)"
        '
        'btnMoveLogFile
        '
        Me.btnMoveLogFile.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnMoveLogFile.Location = New System.Drawing.Point(385, 3)
        Me.btnMoveLogFile.Name = "btnMoveLogFile"
        Me.btnMoveLogFile.Size = New System.Drawing.Size(185, 31)
        Me.btnMoveLogFile.TabIndex = 5
        Me.btnMoveLogFile.Text = "Move Log File"
        Me.btnMoveLogFile.UseVisualStyleBackColor = True
        '
        'lblSyslogServerPortLabel
        '
        Me.lblSyslogServerPortLabel.AutoSize = True
        Me.lblSyslogServerPortLabel.Location = New System.Drawing.Point(506, 56)
        Me.lblSyslogServerPortLabel.Name = "lblSyslogServerPortLabel"
        Me.lblSyslogServerPortLabel.Size = New System.Drawing.Size(97, 13)
        Me.lblSyslogServerPortLabel.TabIndex = 10
        Me.lblSyslogServerPortLabel.Text = "Syslog Server Port:"
        '
        'txtSysLogServerPort
        '
        Me.txtSysLogServerPort.Location = New System.Drawing.Point(609, 53)
        Me.txtSysLogServerPort.Name = "txtSysLogServerPort"
        Me.txtSysLogServerPort.Size = New System.Drawing.Size(40, 20)
        Me.txtSysLogServerPort.TabIndex = 11
        Me.ToolTip.SetToolTip(Me.txtSysLogServerPort, "Default Port: 514")
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1175, 446)
        Me.Controls.Add(Me.chkStartAtUserStartup)
        Me.Controls.Add(Me.NumericUpDown)
        Me.Controls.Add(Me.lblAutoSaveLabel)
        Me.Controls.Add(Me.chkAutoSave)
        Me.Controls.Add(Me.chkAutoScroll)
        Me.Controls.Add(Me.StatusStrip1)
        Me.Controls.Add(Me.logs)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.Controls.Add(Me.txtSysLogServerPort)
        Me.Controls.Add(Me.lblSyslogServerPortLabel)
        Me.Name = "Form1"
        Me.Text = "Free SysLog Server"
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        CType(Me.NumericUpDown, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnServerController As System.Windows.Forms.Button
    Friend WithEvents btnOpenLogLocation As System.Windows.Forms.Button
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents logs As System.Windows.Forms.ListView
    Friend WithEvents Time As System.Windows.Forms.ColumnHeader
    Friend WithEvents Type As System.Windows.Forms.ColumnHeader
    Friend WithEvents IPAddressCol As System.Windows.Forms.ColumnHeader
    Friend WithEvents Log As System.Windows.Forms.ColumnHeader
    Friend WithEvents SaveFileDialog As SaveFileDialog
    Friend WithEvents StatusStrip1 As StatusStrip
    Friend WithEvents NumberOfLogs As ToolStripStatusLabel
    Friend WithEvents chkAutoScroll As CheckBox
    Friend WithEvents btnClearLog As Button
    Friend WithEvents btnSaveLogsToDisk As Button
    Friend WithEvents btnCheckForUpdates As Button
    Friend WithEvents SaveTimer As Timer
    Friend WithEvents chkAutoSave As CheckBox
    Friend WithEvents lblAutoSaveLabel As Label
    Friend WithEvents NumericUpDown As NumericUpDown
    Friend WithEvents lblAutoSaved As ToolStripStatusLabel
    Friend WithEvents chkStartAtUserStartup As CheckBox
    Friend WithEvents lblLogFileSize As ToolStripStatusLabel
    Friend WithEvents btnMoveLogFile As Button
    Friend WithEvents lblSyslogServerPortLabel As Label
    Friend WithEvents txtSysLogServerPort As TextBox
    Friend WithEvents ToolTip As ToolTip
End Class
