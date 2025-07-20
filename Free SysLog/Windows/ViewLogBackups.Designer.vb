<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ViewLogBackups
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.FileList = New System.Windows.Forms.DataGridView()
        Me.ColFileName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ColFileDate = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ColFileSize = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.DeleteToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ViewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.BtnView = New System.Windows.Forms.Button()
        Me.BtnDelete = New System.Windows.Forms.Button()
        Me.BtnRefresh = New System.Windows.Forms.Button()
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.lblNumberOfFiles = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ChkCaseInsensitiveSearch = New System.Windows.Forms.CheckBox()
        Me.ChkRegExSearch = New System.Windows.Forms.CheckBox()
        Me.BtnSearch = New System.Windows.Forms.Button()
        Me.TxtSearchTerms = New System.Windows.Forms.TextBox()
        Me.LblSearchLabel = New System.Windows.Forms.Label()
        Me.lblTotalNumberOfLogs = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ChkShowHidden = New System.Windows.Forms.CheckBox()
        Me.colHidden = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ChkShowHiddenAsGray = New System.Windows.Forms.CheckBox()
        Me.colEntryCount = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.HideToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.UnhideToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolTip = New System.Windows.Forms.ToolTip(Me.components)
        Me.ChkIgnoreSearchResultsLimits = New System.Windows.Forms.CheckBox()
        Me.ChkLogFileDeletions = New System.Windows.Forms.CheckBox()
        Me.lblNumberOfHiddenFiles = New System.Windows.Forms.ToolStripStatusLabel()
        Me.lblTotalNumberOfHiddenLogs = New System.Windows.Forms.ToolStripStatusLabel()
        Me.LblTotalDiskSpace = New System.Windows.Forms.ToolStripStatusLabel()
        Me.lblLimitBy = New System.Windows.Forms.Label()
        Me.boxLimitBy = New System.Windows.Forms.ComboBox()
        Me.boxLimiter = New System.Windows.Forms.ComboBox()
        Me.btnViewLogsWithLimits = New System.Windows.Forms.Button()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.StatusStrip1.SuspendLayout()
        CType(Me.FileList, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'FileList
        '
        Me.FileList.AllowUserToAddRows = False
        Me.FileList.AllowUserToDeleteRows = False
        Me.FileList.AllowUserToOrderColumns = True
        Me.FileList.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.FileList.BackgroundColor = System.Drawing.SystemColors.ControlLightLight
        Me.FileList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.FileList.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.ColFileName, Me.ColFileDate, Me.ColFileSize, Me.colEntryCount, Me.colHidden})
        Me.FileList.ContextMenuStrip = Me.ContextMenuStrip1
        Me.FileList.Location = New System.Drawing.Point(13, 12)
        Me.FileList.Name = "FileList"
        Me.FileList.ReadOnly = True
        Me.FileList.RowHeadersVisible = False
        Me.FileList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.FileList.Size = New System.Drawing.Size(929, 243)
        Me.FileList.TabIndex = 36
        '
        'ColFileName
        '
        Me.ColFileName.HeaderText = "File Name"
        Me.ColFileName.Name = "ColFileName"
        Me.ColFileName.ReadOnly = True
        Me.ColFileName.Width = 240
        '
        'ColFileDate
        '
        Me.ColFileDate.HeaderText = "Modified Date"
        Me.ColFileDate.Name = "ColFileDate"
        Me.ColFileDate.ReadOnly = True
        Me.ColFileDate.Width = 240
        '
        'ColFileSize
        '
        Me.ColFileSize.HeaderText = "File Size"
        Me.ColFileSize.Name = "ColFileSize"
        Me.ColFileSize.ReadOnly = True
        Me.ColFileSize.Width = 240
        '
        'colEntryCount
        '
        Me.colEntryCount.HeaderText = "Entry Count"
        Me.colEntryCount.Name = "colEntryCount"
        Me.colEntryCount.ReadOnly = True
        Me.colEntryCount.Width = 125
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.DeleteToolStripMenuItem, Me.ViewToolStripMenuItem, Me.HideToolStripMenuItem, Me.UnhideToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(147, 92)
        '
        'DeleteToolStripMenuItem
        '
        Me.DeleteToolStripMenuItem.Name = "DeleteToolStripMenuItem"
        Me.DeleteToolStripMenuItem.Size = New System.Drawing.Size(146, 22)
        Me.DeleteToolStripMenuItem.Text = "&Delete"
        '
        'ViewToolStripMenuItem
        '
        Me.ViewToolStripMenuItem.Name = "ViewToolStripMenuItem"
        Me.ViewToolStripMenuItem.Size = New System.Drawing.Size(146, 22)
        Me.ViewToolStripMenuItem.Text = "&View"
        '
        'HideToolStripMenuItem
        '
        Me.HideToolStripMenuItem.Name = "HideToolStripMenuItem"
        Me.HideToolStripMenuItem.Size = New System.Drawing.Size(146, 22)
        Me.HideToolStripMenuItem.Text = "Hide"
        '
        'UnhideToolStripMenuItem
        '
        Me.UnhideToolStripMenuItem.Name = "UnhideToolStripMenuItem"
        Me.UnhideToolStripMenuItem.Size = New System.Drawing.Size(146, 22)
        Me.UnhideToolStripMenuItem.Text = "Unhide/Show"
        '
        'BtnView
        '
        Me.BtnView.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.BtnView.Enabled = False
        Me.BtnView.Location = New System.Drawing.Point(12, 261)
        Me.BtnView.Name = "BtnView"
        Me.BtnView.Size = New System.Drawing.Size(75, 23)
        Me.BtnView.TabIndex = 2
        Me.BtnView.Text = "&View"
        Me.BtnView.UseVisualStyleBackColor = True
        '
        'BtnDelete
        '
        Me.BtnDelete.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.BtnDelete.Enabled = False
        Me.BtnDelete.Location = New System.Drawing.Point(93, 261)
        Me.BtnDelete.Name = "BtnDelete"
        Me.BtnDelete.Size = New System.Drawing.Size(75, 23)
        Me.BtnDelete.TabIndex = 3
        Me.BtnDelete.Text = "&Delete"
        Me.BtnDelete.UseVisualStyleBackColor = True
        '
        'BtnRefresh
        '
        Me.BtnRefresh.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.BtnRefresh.Location = New System.Drawing.Point(174, 261)
        Me.BtnRefresh.Name = "BtnRefresh"
        Me.BtnRefresh.Size = New System.Drawing.Size(95, 23)
        Me.BtnRefresh.TabIndex = 4
        Me.BtnRefresh.Text = "Re&fresh (F5)"
        Me.BtnRefresh.UseVisualStyleBackColor = True
        '
        'StatusStrip1
        '
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.lblNumberOfFiles, Me.lblNumberOfHiddenFiles, Me.lblTotalNumberOfLogs, Me.lblTotalNumberOfHiddenLogs, Me.LblTotalDiskSpace})
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 342)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New System.Drawing.Size(954, 22)
        Me.StatusStrip1.TabIndex = 5
        Me.StatusStrip1.Text = "StatusStrip1"
        '
        'lblNumberOfFiles
        '
        Me.lblNumberOfFiles.Name = "lblNumberOfFiles"
        Me.lblNumberOfFiles.Size = New System.Drawing.Size(94, 17)
        Me.lblNumberOfFiles.Text = "Number of Files:"
        '
        'ChkCaseInsensitiveSearch
        '
        Me.ChkCaseInsensitiveSearch.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.ChkCaseInsensitiveSearch.AutoSize = True
        Me.ChkCaseInsensitiveSearch.Checked = True
        Me.ChkCaseInsensitiveSearch.CheckState = System.Windows.Forms.CheckState.Checked
        Me.ChkCaseInsensitiveSearch.Location = New System.Drawing.Point(388, 319)
        Me.ChkCaseInsensitiveSearch.Name = "ChkCaseInsensitiveSearch"
        Me.ChkCaseInsensitiveSearch.Size = New System.Drawing.Size(109, 17)
        Me.ChkCaseInsensitiveSearch.TabIndex = 33
        Me.ChkCaseInsensitiveSearch.Text = "Case Insensitive?"
        Me.ChkCaseInsensitiveSearch.UseVisualStyleBackColor = True
        '
        'ChkRegExSearch
        '
        Me.ChkRegExSearch.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.ChkRegExSearch.AutoSize = True
        Me.ChkRegExSearch.Location = New System.Drawing.Point(319, 319)
        Me.ChkRegExSearch.Name = "ChkRegExSearch"
        Me.ChkRegExSearch.Size = New System.Drawing.Size(63, 17)
        Me.ChkRegExSearch.TabIndex = 32
        Me.ChkRegExSearch.Text = "Regex?"
        Me.ChkRegExSearch.UseVisualStyleBackColor = True
        '
        'BtnSearch
        '
        Me.BtnSearch.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.BtnSearch.Location = New System.Drawing.Point(496, 315)
        Me.BtnSearch.Name = "BtnSearch"
        Me.BtnSearch.Size = New System.Drawing.Size(52, 23)
        Me.BtnSearch.TabIndex = 31
        Me.BtnSearch.Text = "&Search"
        Me.BtnSearch.UseVisualStyleBackColor = True
        '
        'TxtSearchTerms
        '
        Me.TxtSearchTerms.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.TxtSearchTerms.Location = New System.Drawing.Point(93, 317)
        Me.TxtSearchTerms.Name = "TxtSearchTerms"
        Me.TxtSearchTerms.Size = New System.Drawing.Size(220, 20)
        Me.TxtSearchTerms.TabIndex = 30
        '
        'LblSearchLabel
        '
        Me.LblSearchLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.LblSearchLabel.AutoSize = True
        Me.LblSearchLabel.Location = New System.Drawing.Point(10, 320)
        Me.LblSearchLabel.Name = "LblSearchLabel"
        Me.LblSearchLabel.Size = New System.Drawing.Size(81, 13)
        Me.LblSearchLabel.TabIndex = 29
        Me.LblSearchLabel.Text = "Search All Logs"
        '
        'lblTotalNumberOfLogs
        '
        Me.lblTotalNumberOfLogs.Margin = New System.Windows.Forms.Padding(25, 3, 0, 2)
        Me.lblTotalNumberOfLogs.Name = "lblTotalNumberOfLogs"
        Me.lblTotalNumberOfLogs.Size = New System.Drawing.Size(124, 17)
        Me.lblTotalNumberOfLogs.Text = "Total Number of Logs:"
        '
        'ChkShowHidden
        '
        Me.ChkShowHidden.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.ChkShowHidden.AutoSize = True
        Me.ChkShowHidden.Location = New System.Drawing.Point(275, 265)
        Me.ChkShowHidden.Name = "ChkShowHidden"
        Me.ChkShowHidden.Size = New System.Drawing.Size(114, 17)
        Me.ChkShowHidden.TabIndex = 34
        Me.ChkShowHidden.Text = "Show Hidden Files"
        Me.ChkShowHidden.UseVisualStyleBackColor = True
        '
        'colHidden
        '
        Me.colHidden.HeaderText = "Hidden?"
        Me.colHidden.Name = "colHidden"
        Me.colHidden.ReadOnly = True
        Me.colHidden.Width = 60
        '
        'ChkShowHiddenAsGray
        '
        Me.ChkShowHiddenAsGray.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.ChkShowHiddenAsGray.AutoSize = True
        Me.ChkShowHiddenAsGray.Location = New System.Drawing.Point(395, 265)
        Me.ChkShowHiddenAsGray.Name = "ChkShowHiddenAsGray"
        Me.ChkShowHiddenAsGray.Size = New System.Drawing.Size(153, 17)
        Me.ChkShowHiddenAsGray.TabIndex = 35
        Me.ChkShowHiddenAsGray.Text = "Show Hidden Files as Gray"
        Me.ChkShowHiddenAsGray.UseVisualStyleBackColor = True
        '
        'lblNumberOfHiddenFiles
        '
        Me.lblNumberOfHiddenFiles.Margin = New System.Windows.Forms.Padding(25, 3, 0, 2)
        Me.lblNumberOfHiddenFiles.Name = "lblNumberOfHiddenFiles"
        Me.lblNumberOfHiddenFiles.Size = New System.Drawing.Size(136, 17)
        Me.lblNumberOfHiddenFiles.Text = "Number of Hidden Files:"
        Me.lblNumberOfHiddenFiles.Visible = False
        '
        'lblTotalNumberOfHiddenLogs
        '
        Me.lblTotalNumberOfHiddenLogs.Margin = New System.Windows.Forms.Padding(25, 3, 0, 2)
        Me.lblTotalNumberOfHiddenLogs.Name = "lblTotalNumberOfHiddenLogs"
        Me.lblTotalNumberOfHiddenLogs.Size = New System.Drawing.Size(138, 17)
        Me.lblTotalNumberOfHiddenLogs.Text = "Number of Hidden Logs:"
        Me.lblTotalNumberOfHiddenLogs.Visible = False
        '
        'LblTotalDiskSpace
        '
        Me.LblTotalDiskSpace.Margin = New System.Windows.Forms.Padding(25, 3, 0, 2)
        Me.LblTotalDiskSpace.Name = "LblTotalDiskSpace"
        Me.LblTotalDiskSpace.Size = New System.Drawing.Size(123, 17)
        Me.LblTotalDiskSpace.Text = "Total Disk Space Used:"
        '
        'ChkIgnoreSearchResultsLimits
        '
        Me.ChkIgnoreSearchResultsLimits.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.ChkIgnoreSearchResultsLimits.AutoSize = True
        Me.ChkIgnoreSearchResultsLimits.Location = New System.Drawing.Point(554, 292)
        Me.ChkIgnoreSearchResultsLimits.Name = "ChkIgnoreSearchResultsLimits"
        Me.ChkIgnoreSearchResultsLimits.Size = New System.Drawing.Size(160, 17)
        Me.ChkIgnoreSearchResultsLimits.TabIndex = 37
        Me.ChkIgnoreSearchResultsLimits.Text = "Ignore Search Results Limits"
        Me.ToolTip.SetToolTip(Me.ChkIgnoreSearchResultsLimits, "Warning: Enabling this could cause performance issues.")
        Me.ChkIgnoreSearchResultsLimits.UseVisualStyleBackColor = True
        '
        'ChkLogFileDeletions
        '
        Me.ChkLogFileDeletions.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.ChkLogFileDeletions.AutoSize = True
        Me.ChkLogFileDeletions.Location = New System.Drawing.Point(554, 265)
        Me.ChkLogFileDeletions.Name = "ChkLogFileDeletions"
        Me.ChkLogFileDeletions.Size = New System.Drawing.Size(127, 17)
        Me.ChkLogFileDeletions.TabIndex = 38
        Me.ChkLogFileDeletions.Text = "Log Deletions of Files"
        Me.ChkLogFileDeletions.UseVisualStyleBackColor = True
        '
        'lblLimitBy
        '
        Me.lblLimitBy.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblLimitBy.AutoSize = True
        Me.lblLimitBy.Location = New System.Drawing.Point(10, 293)
        Me.lblLimitBy.Name = "lblLimitBy"
        Me.lblLimitBy.Size = New System.Drawing.Size(46, 13)
        Me.lblLimitBy.TabIndex = 39
        Me.lblLimitBy.Text = "Limit By:"
        '
        'boxLimitBy
        '
        Me.boxLimitBy.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.boxLimitBy.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.boxLimitBy.FormattingEnabled = True
        Me.boxLimitBy.Items.AddRange(New Object() {"(Not Specified)", "Log Type", "Remote Process", "Source Hostname", "Source IP Address"})
        Me.boxLimitBy.Location = New System.Drawing.Point(55, 290)
        Me.boxLimitBy.Name = "boxLimitBy"
        Me.boxLimitBy.Size = New System.Drawing.Size(121, 21)
        Me.boxLimitBy.Text = "(Not Specified)"
        Me.boxLimitBy.TabIndex = 40
        '
        'boxLimiter
        '
        Me.boxLimiter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.boxLimiter.Enabled = False
        Me.boxLimiter.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.boxLimiter.FormattingEnabled = True
        Me.boxLimiter.Location = New System.Drawing.Point(182, 290)
        Me.boxLimiter.Name = "boxLimiter"
        Me.boxLimiter.Size = New System.Drawing.Size(250, 21)
        Me.boxLimiter.Text = "(Not Specified)"
        Me.boxLimiter.TabIndex = 41
        '
        'btnViewLogsWithLimits
        '
        Me.btnViewLogsWithLimits.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnViewLogsWithLimits.Location = New System.Drawing.Point(438, 288)
        Me.btnViewLogsWithLimits.Name = "btnViewLogsWithLimits"
        Me.btnViewLogsWithLimits.Size = New System.Drawing.Size(110, 23)
        Me.btnViewLogsWithLimits.TabIndex = 42
        Me.btnViewLogsWithLimits.Text = "View All with Limits"
        Me.btnViewLogsWithLimits.UseVisualStyleBackColor = True
        '
        'ViewLogBackups
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(954, 364)
        Me.Controls.Add(Me.btnViewLogsWithLimits)
        Me.Controls.Add(Me.boxLimiter)
        Me.Controls.Add(Me.boxLimitBy)
        Me.Controls.Add(Me.lblLimitBy)
        Me.Controls.Add(Me.ChkIgnoreSearchResultsLimits)
        Me.Controls.Add(Me.ChkShowHiddenAsGray)
        Me.Controls.Add(Me.ChkShowHidden)
        Me.Controls.Add(Me.ChkCaseInsensitiveSearch)
        Me.Controls.Add(Me.ChkRegExSearch)
        Me.Controls.Add(Me.BtnSearch)
        Me.Controls.Add(Me.TxtSearchTerms)
        Me.Controls.Add(Me.LblSearchLabel)
        Me.Controls.Add(Me.StatusStrip1)
        Me.Controls.Add(Me.BtnRefresh)
        Me.Controls.Add(Me.BtnDelete)
        Me.Controls.Add(Me.BtnView)
        Me.Controls.Add(Me.FileList)
        Me.Controls.Add(Me.ChkLogFileDeletions)
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(970, 403)
        Me.Name = "ViewLogBackups"
        Me.Text = "View Log Backups"
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        CType(Me.FileList, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents FileList As DataGridView
    Friend WithEvents ColFileName As DataGridViewTextBoxColumn
    Friend WithEvents ColFileDate As DataGridViewTextBoxColumn
    Friend WithEvents ColFileSize As DataGridViewTextBoxColumn
    Friend WithEvents BtnView As Button
    Friend WithEvents BtnDelete As Button
    Friend WithEvents BtnRefresh As Button
    Friend WithEvents StatusStrip1 As StatusStrip
    Friend WithEvents lblNumberOfFiles As ToolStripStatusLabel
    Friend WithEvents ChkCaseInsensitiveSearch As CheckBox
    Friend WithEvents ChkRegExSearch As CheckBox
    Friend WithEvents BtnSearch As Button
    Friend WithEvents TxtSearchTerms As TextBox
    Friend WithEvents LblSearchLabel As Label
    Friend WithEvents ContextMenuStrip1 As ContextMenuStrip
    Friend WithEvents DeleteToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ViewToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents lblTotalNumberOfLogs As ToolStripStatusLabel
    Friend WithEvents ChkShowHidden As CheckBox
    Friend WithEvents HideToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents UnhideToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents colHidden As DataGridViewTextBoxColumn
    Friend WithEvents ChkShowHiddenAsGray As CheckBox
    Friend WithEvents lblNumberOfHiddenFiles As ToolStripStatusLabel
    Friend WithEvents lblTotalNumberOfHiddenLogs As ToolStripStatusLabel
    Friend WithEvents LblTotalDiskSpace As ToolStripStatusLabel
    Friend WithEvents colEntryCount As DataGridViewTextBoxColumn
    Friend WithEvents ToolTip As ToolTip
    Friend WithEvents ChkIgnoreSearchResultsLimits As CheckBox
    Friend WithEvents ChkLogFileDeletions As CheckBox
    Friend WithEvents lblLimitBy As Label
    Friend WithEvents boxLimitBy As ComboBox
    Friend WithEvents boxLimiter As ComboBox
    Friend WithEvents btnViewLogsWithLimits As Button
End Class
