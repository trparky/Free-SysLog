<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class IgnoredLogsAndSearchResults
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

    Private Shadows parentForm As Object

    Public Sub New(parentForm As Object)
        InitializeComponent()
        Me.parentForm = parentForm
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.Logs = New System.Windows.Forms.DataGridView()
        Me.colServerTime = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ColTime = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ColIPAddress = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ColLog = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ColAlerts = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ColRemoteProcess = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.LblCount = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ColFileName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.BtnExport = New System.Windows.Forms.Button()
        Me.SaveFileDialog = New System.Windows.Forms.SaveFileDialog()
        Me.BtnClearIgnoredLogs = New System.Windows.Forms.Button()
        Me.BtnViewMainWindow = New System.Windows.Forms.Button()
        Me.CreateAlertToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.LogsContextMenu = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.CopyLogTextToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ChkCaseInsensitiveSearch = New System.Windows.Forms.CheckBox()
        Me.ChkRegExSearch = New System.Windows.Forms.CheckBox()
        Me.BtnSearch = New System.Windows.Forms.Button()
        Me.TxtSearchTerms = New System.Windows.Forms.TextBox()
        Me.LblSearchLabel = New System.Windows.Forms.Label()
        Me.colLogType = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ColHostname = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.OpenLogFileForViewingToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        CType(Me.Logs, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.LogsContextMenu.SuspendLayout()
        Me.StatusStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'ColHostname
        '
        Me.ColHostname.HeaderText = "Hostname"
        Me.ColHostname.Name = "ColHostname"
        Me.ColHostname.ReadOnly = True
        Me.ColHostname.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic
        Me.ColHostname.Width = 150
        '
        'colLogType
        '
        Me.colLogType.HeaderText = "Log Type"
        Me.colLogType.Name = "colLogType"
        Me.colLogType.ReadOnly = True
        Me.colLogType.Width = 200
        '
        'StatusStrip1
        '
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.LblCount})
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 403)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New System.Drawing.Size(1152, 22)
        Me.StatusStrip1.TabIndex = 5
        Me.StatusStrip1.Text = "StatusStrip1"
        '
        'LblCount
        '
        Me.LblCount.Name = "LblCount"
        Me.LblCount.Size = New System.Drawing.Size(53, 17)
        Me.LblCount.Text = "lblCount"
        '
        'Logs
        '
        Me.Logs.AllowUserToAddRows = False
        Me.Logs.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Logs.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.Logs.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.ColTime, Me.colServerTime, Me.colLogType, Me.ColIPAddress, Me.ColHostname, Me.ColRemoteProcess, Me.ColLog, Me.ColAlerts, Me.ColFileName})
        Me.Logs.ContextMenuStrip = Me.LogsContextMenu
        Me.Logs.Location = New System.Drawing.Point(12, 12)
        Me.Logs.Name = "Logs"
        Me.Logs.ReadOnly = True
        Me.Logs.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.Logs.Size = New System.Drawing.Size(1128, 359)
        Me.Logs.TabIndex = 19
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
        Me.ColTime.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
        Me.ColTime.HeaderCell.Style.Padding = New Padding(0, 0, 1, 0)
        Me.ColTime.HeaderText = "Time"
        Me.ColTime.Name = "ColTime"
        Me.ColTime.ReadOnly = True
        Me.ColTime.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic
        Me.ColTime.ToolTipText = "The time at which the log entry came in."
        '
        'ColIPAddress
        '
        Me.ColIPAddress.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
        Me.ColIPAddress.HeaderCell.Style.Padding = New Padding(0, 0, 2, 0)
        Me.ColIPAddress.HeaderText = "IP Address"
        Me.ColIPAddress.Name = "ColIPAddress"
        Me.ColIPAddress.ReadOnly = True
        Me.ColIPAddress.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic
        Me.ColIPAddress.ToolTipText = "The IP address of the system from which the log came from."
        '
        'ColFileName
        '
        Me.ColFileName.HeaderText = "File Name"
        Me.ColFileName.Name = "ColFileName"
        Me.ColFileName.ReadOnly = True
        Me.ColFileName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic
        Me.ColFileName.Visible = False
        Me.ColFileName.Width = 150
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
        'ColAlerts
        '
        Me.ColAlerts.HeaderText = "Alerted"
        Me.ColAlerts.Name = "ColAlerts"
        Me.ColAlerts.ReadOnly = True
        Me.ColAlerts.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic
        Me.ColAlerts.ToolTipText = "True or False. Indicates if the log entry triggered an alert from this program."
        Me.ColAlerts.Width = 50
        '
        'ColSyslogHeader
        '
        Me.ColRemoteProcess.HeaderText = "Remote Process"
        Me.ColRemoteProcess.Name = "ColRemoteProcess"
        Me.ColRemoteProcess.ReadOnly = True
        Me.ColRemoteProcess.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic
        Me.ColRemoteProcess.Width = 150
        '
        'BtnExport
        '
        Me.BtnExport.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.BtnExport.Location = New System.Drawing.Point(1065, 377)
        Me.BtnExport.Name = "BtnExport"
        Me.BtnExport.Size = New System.Drawing.Size(75, 23)
        Me.BtnExport.TabIndex = 20
        Me.BtnExport.Text = "Export"
        Me.BtnExport.UseVisualStyleBackColor = True
        '
        'BtnClearIgnoredLogs
        '
        Me.BtnClearIgnoredLogs.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.BtnClearIgnoredLogs.Location = New System.Drawing.Point(923, 377)
        Me.BtnClearIgnoredLogs.Name = "BtnClearIgnoredLogs"
        Me.BtnClearIgnoredLogs.Size = New System.Drawing.Size(136, 23)
        Me.BtnClearIgnoredLogs.TabIndex = 21
        Me.BtnClearIgnoredLogs.Text = "Clear Ignored Logs"
        Me.BtnClearIgnoredLogs.UseVisualStyleBackColor = True
        Me.BtnClearIgnoredLogs.Visible = False
        '
        'LogsContextMenu
        '
        Me.LogsContextMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.CopyLogTextToolStripMenuItem, Me.CreateAlertToolStripMenuItem, Me.OpenLogFileForViewingToolStripMenuItem})
        Me.LogsContextMenu.Name = "LogsContextMenu"
        Me.LogsContextMenu.Size = New System.Drawing.Size(211, 92)
        '
        'CopyLogTextToolStripMenuItem
        '
        Me.CopyLogTextToolStripMenuItem.Name = "CopyLogTextToolStripMenuItem"
        Me.CopyLogTextToolStripMenuItem.Size = New System.Drawing.Size(180, 22)
        Me.CopyLogTextToolStripMenuItem.Text = "Copy Log Text"
        '
        'BtnViewMainWindow
        '
        Me.BtnViewMainWindow.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.BtnViewMainWindow.Enabled = False
        Me.BtnViewMainWindow.Location = New System.Drawing.Point(12, 377)
        Me.BtnViewMainWindow.Name = "BtnViewMainWindow"
        Me.BtnViewMainWindow.Size = New System.Drawing.Size(117, 23)
        Me.BtnViewMainWindow.TabIndex = 22
        Me.BtnViewMainWindow.Text = "View Main Window"
        Me.BtnViewMainWindow.UseVisualStyleBackColor = True
        Me.BtnViewMainWindow.Visible = False
        '
        'CreateAlertToolStripMenuItem
        '
        Me.CreateAlertToolStripMenuItem.Name = "CreateAlertToolStripMenuItem"
        Me.CreateAlertToolStripMenuItem.Size = New System.Drawing.Size(180, 22)
        Me.CreateAlertToolStripMenuItem.Text = "Create Alert"
        '
        'ChkCaseInsensitiveSearch
        '
        Me.ChkCaseInsensitiveSearch.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.ChkCaseInsensitiveSearch.AutoSize = True
        Me.ChkCaseInsensitiveSearch.Checked = True
        Me.ChkCaseInsensitiveSearch.CheckState = System.Windows.Forms.CheckState.Checked
        Me.ChkCaseInsensitiveSearch.Location = New System.Drawing.Point(431, 382)
        Me.ChkCaseInsensitiveSearch.Name = "ChkCaseInsensitiveSearch"
        Me.ChkCaseInsensitiveSearch.Size = New System.Drawing.Size(109, 17)
        Me.ChkCaseInsensitiveSearch.TabIndex = 28
        Me.ChkCaseInsensitiveSearch.Text = "Case Insensitive?"
        Me.ChkCaseInsensitiveSearch.UseVisualStyleBackColor = True
        Me.ChkCaseInsensitiveSearch.Visible = False
        '
        'ChkRegExSearch
        '
        Me.ChkRegExSearch.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.ChkRegExSearch.AutoSize = True
        Me.ChkRegExSearch.Location = New System.Drawing.Point(362, 382)
        Me.ChkRegExSearch.Name = "ChkRegExSearch"
        Me.ChkRegExSearch.Size = New System.Drawing.Size(63, 17)
        Me.ChkRegExSearch.TabIndex = 27
        Me.ChkRegExSearch.Text = "Regex?"
        Me.ChkRegExSearch.UseVisualStyleBackColor = True
        Me.ChkRegExSearch.Visible = False
        '
        'BtnSearch
        '
        Me.BtnSearch.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.BtnSearch.Location = New System.Drawing.Point(539, 378)
        Me.BtnSearch.Name = "BtnSearch"
        Me.BtnSearch.Size = New System.Drawing.Size(52, 23)
        Me.BtnSearch.TabIndex = 26
        Me.BtnSearch.Text = "Search"
        Me.BtnSearch.UseVisualStyleBackColor = True
        Me.BtnSearch.Visible = False
        '
        'TxtSearchTerms
        '
        Me.TxtSearchTerms.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.TxtSearchTerms.Location = New System.Drawing.Point(208, 379)
        Me.TxtSearchTerms.Name = "TxtSearchTerms"
        Me.TxtSearchTerms.Size = New System.Drawing.Size(148, 20)
        Me.TxtSearchTerms.TabIndex = 25
        Me.TxtSearchTerms.Visible = False
        '
        'LblSearchLabel
        '
        Me.LblSearchLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.LblSearchLabel.AutoSize = True
        Me.LblSearchLabel.Location = New System.Drawing.Point(135, 382)
        Me.LblSearchLabel.Name = "LblSearchLabel"
        Me.LblSearchLabel.Size = New System.Drawing.Size(67, 13)
        Me.LblSearchLabel.TabIndex = 24
        Me.LblSearchLabel.Text = "Search Logs"
        Me.LblSearchLabel.Visible = False
        '
        'OpenLogFileForViewingToolStripMenuItem
        '
        Me.OpenLogFileForViewingToolStripMenuItem.Name = "OpenLogFileForViewingToolStripMenuItem"
        Me.OpenLogFileForViewingToolStripMenuItem.Size = New System.Drawing.Size(210, 22)
        Me.OpenLogFileForViewingToolStripMenuItem.Text = "Open Log File for Viewing"
        Me.OpenLogFileForViewingToolStripMenuItem.Visible = False
        '
        'IgnoredLogsAndSearchResults
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1152, 425)
        Me.Controls.Add(Me.BtnViewMainWindow)
        Me.Controls.Add(Me.BtnExport)
        Me.Controls.Add(Me.Logs)
        Me.Controls.Add(Me.StatusStrip1)
        Me.Controls.Add(Me.BtnClearIgnoredLogs)
        Me.Controls.Add(Me.ChkCaseInsensitiveSearch)
        Me.Controls.Add(Me.ChkRegExSearch)
        Me.Controls.Add(Me.BtnSearch)
        Me.Controls.Add(Me.TxtSearchTerms)
        Me.Controls.Add(Me.LblSearchLabel)
        Me.MaximizeBox = False
        Me.MinimumSize = New System.Drawing.Size(1168, 464)
        Me.Name = "IgnoredLogsAndSearchResults"
        Me.Text = "Ignored Logs"
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        CType(Me.Logs, System.ComponentModel.ISupportInitialize).EndInit()
        Me.LogsContextMenu.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents StatusStrip1 As StatusStrip
    Friend WithEvents LblCount As ToolStripStatusLabel
    Friend WithEvents Logs As DataGridView
    Friend WithEvents ColTime As DataGridViewTextBoxColumn
    Friend WithEvents colServerTime As DataGridViewTextBoxColumn
    Friend WithEvents ColIPAddress As DataGridViewTextBoxColumn
    Friend WithEvents ColLog As DataGridViewTextBoxColumn
    Friend WithEvents BtnExport As Button
    Friend WithEvents SaveFileDialog As SaveFileDialog
    Friend WithEvents BtnClearIgnoredLogs As Button
    Friend WithEvents BtnViewMainWindow As Button
    Friend WithEvents LogsContextMenu As ContextMenuStrip
    Friend WithEvents CopyLogTextToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents CreateAlertToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ColAlerts As DataGridViewTextBoxColumn
    Friend WithEvents ColRemoteProcess As DataGridViewTextBoxColumn
    Friend WithEvents ColFileName As DataGridViewTextBoxColumn
    Friend WithEvents ChkCaseInsensitiveSearch As CheckBox
    Friend WithEvents ChkRegExSearch As CheckBox
    Friend WithEvents BtnSearch As Button
    Friend WithEvents TxtSearchTerms As TextBox
    Friend WithEvents LblSearchLabel As Label
    Friend WithEvents OpenLogFileForViewingToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents colLogType As DataGridViewTextBoxColumn
    Friend WithEvents ColHostname As DataGridViewTextBoxColumn
End Class
