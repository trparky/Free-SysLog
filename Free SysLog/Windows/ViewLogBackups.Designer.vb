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
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.FileList = New System.Windows.Forms.ListView()
        Me.ColFileName = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColFileDate = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColFileSize = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
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
        Me.ContextMenuStrip1.SuspendLayout()
        Me.StatusStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'FileList
        '
        Me.FileList.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.FileList.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColFileName, Me.ColFileDate, Me.ColFileSize})
        Me.FileList.ContextMenuStrip = Me.ContextMenuStrip1
        Me.FileList.FullRowSelect = True
        Me.FileList.HideSelection = False
        Me.FileList.Location = New System.Drawing.Point(12, 12)
        Me.FileList.Name = "FileList"
        Me.FileList.Size = New System.Drawing.Size(776, 298)
        Me.FileList.TabIndex = 1
        Me.FileList.UseCompatibleStateImageBehavior = False
        Me.FileList.View = System.Windows.Forms.View.Details
        '
        'ColFileName
        '
        Me.ColFileName.Text = "File Name"
        Me.ColFileName.Width = 240
        '
        'ColFileDate
        '
        Me.ColFileDate.Text = "Date"
        Me.ColFileDate.Width = 240
        '
        'ColFileSize
        '
        Me.ColFileSize.Text = "Size"
        Me.ColFileSize.Width = 240
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.DeleteToolStripMenuItem, Me.ViewToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(181, 70)
        '
        'DeleteToolStripMenuItem
        '
        Me.DeleteToolStripMenuItem.Name = "DeleteToolStripMenuItem"
        Me.DeleteToolStripMenuItem.Size = New System.Drawing.Size(180, 22)
        Me.DeleteToolStripMenuItem.Text = "&Delete"
        '
        'ViewToolStripMenuItem
        '
        Me.ViewToolStripMenuItem.Name = "ViewToolStripMenuItem"
        Me.ViewToolStripMenuItem.Size = New System.Drawing.Size(180, 22)
        Me.ViewToolStripMenuItem.Text = "&View"
        '
        'BtnView
        '
        Me.BtnView.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.BtnView.Enabled = False
        Me.BtnView.Location = New System.Drawing.Point(13, 316)
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
        Me.BtnDelete.Location = New System.Drawing.Point(94, 316)
        Me.BtnDelete.Name = "BtnDelete"
        Me.BtnDelete.Size = New System.Drawing.Size(75, 23)
        Me.BtnDelete.TabIndex = 3
        Me.BtnDelete.Text = "&Delete"
        Me.BtnDelete.UseVisualStyleBackColor = True
        '
        'BtnRefresh
        '
        Me.BtnRefresh.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.BtnRefresh.Location = New System.Drawing.Point(175, 316)
        Me.BtnRefresh.Name = "BtnRefresh"
        Me.BtnRefresh.Size = New System.Drawing.Size(95, 23)
        Me.BtnRefresh.TabIndex = 4
        Me.BtnRefresh.Text = "Re&fresh (F5)"
        Me.BtnRefresh.UseVisualStyleBackColor = True
        '
        'StatusStrip1
        '
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.lblNumberOfFiles, Me.lblTotalNumberOfLogs})
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 342)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New System.Drawing.Size(800, 22)
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
        Me.ChkCaseInsensitiveSearch.Location = New System.Drawing.Point(586, 321)
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
        Me.ChkRegExSearch.Location = New System.Drawing.Point(517, 321)
        Me.ChkRegExSearch.Name = "ChkRegExSearch"
        Me.ChkRegExSearch.Size = New System.Drawing.Size(63, 17)
        Me.ChkRegExSearch.TabIndex = 32
        Me.ChkRegExSearch.Text = "Regex?"
        Me.ChkRegExSearch.UseVisualStyleBackColor = True
        '
        'BtnSearch
        '
        Me.BtnSearch.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.BtnSearch.Location = New System.Drawing.Point(694, 317)
        Me.BtnSearch.Name = "BtnSearch"
        Me.BtnSearch.Size = New System.Drawing.Size(52, 23)
        Me.BtnSearch.TabIndex = 31
        Me.BtnSearch.Text = "&Search"
        Me.BtnSearch.UseVisualStyleBackColor = True
        '
        'TxtSearchTerms
        '
        Me.TxtSearchTerms.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.TxtSearchTerms.Location = New System.Drawing.Point(363, 318)
        Me.TxtSearchTerms.Name = "TxtSearchTerms"
        Me.TxtSearchTerms.Size = New System.Drawing.Size(148, 20)
        Me.TxtSearchTerms.TabIndex = 30
        '
        'LblSearchLabel
        '
        Me.LblSearchLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.LblSearchLabel.AutoSize = True
        Me.LblSearchLabel.Location = New System.Drawing.Point(276, 321)
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
        'ViewLogBackups
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(800, 364)
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
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(816, 403)
        Me.Name = "ViewLogBackups"
        Me.Text = "View Log Backups"
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents FileList As ListView
    Friend WithEvents ColFileName As ColumnHeader
    Friend WithEvents ColFileDate As ColumnHeader
    Friend WithEvents ColFileSize As ColumnHeader
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
End Class
