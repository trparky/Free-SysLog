<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class IgnoredWordsAndPhrases
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
        Me.BtnAdd = New System.Windows.Forms.Button()
        Me.BtnDelete = New System.Windows.Forms.Button()
        Me.IgnoredListView = New System.Windows.Forms.ListView()
        Me.Ignored = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.Regex = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.CaseSensitive = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColEnabled = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.colHits = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.colTarget = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.colDateCreated = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ListViewMenu = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.EnableDisableToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ResetHitsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.BtnEdit = New System.Windows.Forms.Button()
        Me.BtnEnableDisable = New System.Windows.Forms.Button()
        Me.BtnImport = New System.Windows.Forms.Button()
        Me.BtnExport = New System.Windows.Forms.Button()
        Me.btnDeleteAll = New System.Windows.Forms.Button()
        Me.BtnDown = New System.Windows.Forms.Button()
        Me.BtnUp = New System.Windows.Forms.Button()
        Me.SeparatingLine = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.ChkEnabled = New System.Windows.Forms.CheckBox()
        Me.ChkCaseSensitive = New System.Windows.Forms.CheckBox()
        Me.ChkRegex = New System.Windows.Forms.CheckBox()
        Me.TxtIgnored = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.BtnCancel = New System.Windows.Forms.Button()
        Me.btnDeleteDuringEditing = New System.Windows.Forms.Button()
        Me.ChkRemoteProcess = New System.Windows.Forms.CheckBox()
        Me.btnResetHits = New System.Windows.Forms.Button()
        Me.txtComment = New System.Windows.Forms.TextBox()
        Me.lblCommentLabel = New System.Windows.Forms.Label()
        Me.lblTotalHits = New System.Windows.Forms.Label()
        Me.colDateOfLastEvent = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.btnUpdateHits = New System.Windows.Forms.Button()
        Me.colSinceLastEvent = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ListViewMenu.SuspendLayout()
        Me.SuspendLayout()
        '
        'BtnAdd
        '
        Me.BtnAdd.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.BtnAdd.Location = New System.Drawing.Point(12, 402)
        Me.BtnAdd.Name = "BtnAdd"
        Me.BtnAdd.Size = New System.Drawing.Size(65, 23)
        Me.BtnAdd.TabIndex = 1
        Me.BtnAdd.Text = "Add"
        Me.BtnAdd.UseVisualStyleBackColor = True
        '
        'BtnDelete
        '
        Me.BtnDelete.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.BtnDelete.Enabled = False
        Me.BtnDelete.Location = New System.Drawing.Point(12, 251)
        Me.BtnDelete.Name = "BtnDelete"
        Me.BtnDelete.Size = New System.Drawing.Size(65, 23)
        Me.BtnDelete.TabIndex = 2
        Me.BtnDelete.Text = "Delete"
        Me.BtnDelete.UseVisualStyleBackColor = True
        '
        'IgnoredListView
        '
        Me.IgnoredListView.AllowColumnReorder = True
        Me.IgnoredListView.AllowDrop = True
        Me.IgnoredListView.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.IgnoredListView.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.Ignored, Me.Regex, Me.CaseSensitive, Me.ColEnabled, Me.colHits, Me.colTarget, Me.colDateCreated, Me.colDateOfLastEvent, Me.colSinceLastEvent})
        Me.IgnoredListView.ContextMenuStrip = Me.ListViewMenu
        Me.IgnoredListView.FullRowSelect = True
        Me.IgnoredListView.HideSelection = False
        Me.IgnoredListView.Location = New System.Drawing.Point(12, 12)
        Me.IgnoredListView.Name = "IgnoredListView"
        Me.IgnoredListView.Size = New System.Drawing.Size(950, 233)
        Me.IgnoredListView.TabIndex = 5
        Me.IgnoredListView.UseCompatibleStateImageBehavior = False
        Me.IgnoredListView.View = System.Windows.Forms.View.Details
        '
        'Ignored
        '
        Me.Ignored.Text = "Ignored Word and/or Phrase"
        Me.Ignored.Width = 345
        '
        'Regex
        '
        Me.Regex.Text = "Regex"
        '
        'CaseSensitive
        '
        Me.CaseSensitive.Text = "Case Sensitive"
        Me.CaseSensitive.Width = 91
        '
        'ColEnabled
        '
        Me.ColEnabled.Text = "Enabled"
        '
        'colHits
        '
        Me.colHits.Text = "Hits"
        Me.colHits.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'colTarget
        '
        Me.colTarget.Text = "Target"
        Me.colTarget.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.colTarget.Width = 125
        '
        'colDateCreated
        '
        Me.colDateCreated.Text = "Date Created"
        Me.colDateCreated.Width = 180
        '
        'ListViewMenu
        '
        Me.ListViewMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.EnableDisableToolStripMenuItem, Me.ResetHitsToolStripMenuItem})
        Me.ListViewMenu.Name = "ContextMenuStrip1"
        Me.ListViewMenu.Size = New System.Drawing.Size(153, 48)
        '
        'EnableDisableToolStripMenuItem
        '
        Me.EnableDisableToolStripMenuItem.Name = "EnableDisableToolStripMenuItem"
        Me.EnableDisableToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
        Me.EnableDisableToolStripMenuItem.Text = "Enable/Disable"
        '
        'ResetHitsToolStripMenuItem
        '
        Me.ResetHitsToolStripMenuItem.Name = "ResetHitsToolStripMenuItem"
        Me.ResetHitsToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
        Me.ResetHitsToolStripMenuItem.Text = "Reset Hit"
        '
        'BtnEdit
        '
        Me.BtnEdit.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.BtnEdit.Enabled = False
        Me.BtnEdit.Location = New System.Drawing.Point(83, 251)
        Me.BtnEdit.Name = "BtnEdit"
        Me.BtnEdit.Size = New System.Drawing.Size(75, 23)
        Me.BtnEdit.TabIndex = 8
        Me.BtnEdit.Text = "Edit"
        Me.BtnEdit.UseVisualStyleBackColor = True
        '
        'BtnEnableDisable
        '
        Me.BtnEnableDisable.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.BtnEnableDisable.Enabled = False
        Me.BtnEnableDisable.Location = New System.Drawing.Point(164, 251)
        Me.BtnEnableDisable.Name = "BtnEnableDisable"
        Me.BtnEnableDisable.Size = New System.Drawing.Size(75, 23)
        Me.BtnEnableDisable.TabIndex = 10
        Me.BtnEnableDisable.Text = "Disable"
        Me.BtnEnableDisable.UseVisualStyleBackColor = True
        '
        'BtnImport
        '
        Me.BtnImport.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.BtnImport.Location = New System.Drawing.Point(917, 251)
        Me.BtnImport.Name = "BtnImport"
        Me.BtnImport.Size = New System.Drawing.Size(75, 23)
        Me.BtnImport.TabIndex = 11
        Me.BtnImport.Text = "Import"
        Me.BtnImport.UseVisualStyleBackColor = True
        '
        'BtnExport
        '
        Me.BtnExport.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.BtnExport.Location = New System.Drawing.Point(836, 251)
        Me.BtnExport.Name = "BtnExport"
        Me.BtnExport.Size = New System.Drawing.Size(75, 23)
        Me.BtnExport.TabIndex = 12
        Me.BtnExport.Text = "Export"
        Me.BtnExport.UseVisualStyleBackColor = True
        '
        'btnDeleteAll
        '
        Me.btnDeleteAll.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnDeleteAll.Location = New System.Drawing.Point(245, 251)
        Me.btnDeleteAll.Name = "btnDeleteAll"
        Me.btnDeleteAll.Size = New System.Drawing.Size(75, 23)
        Me.btnDeleteAll.TabIndex = 17
        Me.btnDeleteAll.Text = "Delete All"
        Me.btnDeleteAll.UseVisualStyleBackColor = True
        '
        'BtnDown
        '
        Me.BtnDown.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.BtnDown.Location = New System.Drawing.Point(969, 222)
        Me.BtnDown.Name = "BtnDown"
        Me.BtnDown.Size = New System.Drawing.Size(24, 23)
        Me.BtnDown.TabIndex = 19
        Me.BtnDown.Text = "▼"
        Me.BtnDown.UseVisualStyleBackColor = True
        '
        'BtnUp
        '
        Me.BtnUp.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.BtnUp.Location = New System.Drawing.Point(968, 12)
        Me.BtnUp.Name = "BtnUp"
        Me.BtnUp.Size = New System.Drawing.Size(24, 23)
        Me.BtnUp.TabIndex = 18
        Me.BtnUp.Text = "▲"
        Me.BtnUp.UseVisualStyleBackColor = True
        '
        'SeparatingLine
        '
        Me.SeparatingLine.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SeparatingLine.BackColor = System.Drawing.Color.Black
        Me.SeparatingLine.Location = New System.Drawing.Point(-1, 286)
        Me.SeparatingLine.Name = "SeparatingLine"
        Me.SeparatingLine.Size = New System.Drawing.Size(1010, 1)
        Me.SeparatingLine.TabIndex = 26
        '
        'Label4
        '
        Me.Label4.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(12, 299)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(161, 13)
        Me.Label4.TabIndex = 39
        Me.Label4.Text = "Add Ignored Words and Phrases"
        '
        'ChkEnabled
        '
        Me.ChkEnabled.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.ChkEnabled.AutoSize = True
        Me.ChkEnabled.Checked = True
        Me.ChkEnabled.CheckState = System.Windows.Forms.CheckState.Checked
        Me.ChkEnabled.Location = New System.Drawing.Point(607, 351)
        Me.ChkEnabled.Name = "ChkEnabled"
        Me.ChkEnabled.Size = New System.Drawing.Size(71, 17)
        Me.ChkEnabled.TabIndex = 44
        Me.ChkEnabled.Text = "Enabled?"
        Me.ChkEnabled.UseVisualStyleBackColor = True
        '
        'ChkCaseSensitive
        '
        Me.ChkCaseSensitive.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.ChkCaseSensitive.AutoSize = True
        Me.ChkCaseSensitive.Location = New System.Drawing.Point(499, 351)
        Me.ChkCaseSensitive.Name = "ChkCaseSensitive"
        Me.ChkCaseSensitive.Size = New System.Drawing.Size(102, 17)
        Me.ChkCaseSensitive.TabIndex = 43
        Me.ChkCaseSensitive.Text = "Case Sensitive?"
        Me.ChkCaseSensitive.UseVisualStyleBackColor = True
        '
        'ChkRegex
        '
        Me.ChkRegex.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.ChkRegex.AutoSize = True
        Me.ChkRegex.Location = New System.Drawing.Point(15, 351)
        Me.ChkRegex.Name = "ChkRegex"
        Me.ChkRegex.Size = New System.Drawing.Size(478, 17)
        Me.ChkRegex.TabIndex = 42
        Me.ChkRegex.Text = "Regex? (Be careful with Regex, a broken regex pattern could cause the program to " &
    "malfunction)"
        Me.ChkRegex.UseVisualStyleBackColor = True
        '
        'TxtIgnored
        '
        Me.TxtIgnored.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TxtIgnored.Location = New System.Drawing.Point(61, 325)
        Me.TxtIgnored.Name = "TxtIgnored"
        Me.TxtIgnored.Size = New System.Drawing.Size(932, 20)
        Me.TxtIgnored.TabIndex = 41
        '
        'Label1
        '
        Me.Label1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 328)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(43, 13)
        Me.Label1.TabIndex = 40
        Me.Label1.Text = "Ignored"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'BtnCancel
        '
        Me.BtnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.BtnCancel.Location = New System.Drawing.Point(83, 402)
        Me.BtnCancel.Name = "BtnCancel"
        Me.BtnCancel.Size = New System.Drawing.Size(75, 23)
        Me.BtnCancel.TabIndex = 45
        Me.BtnCancel.Text = "Cancel"
        Me.BtnCancel.UseVisualStyleBackColor = True
        '
        'btnDeleteDuringEditing
        '
        Me.btnDeleteDuringEditing.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnDeleteDuringEditing.Location = New System.Drawing.Point(164, 402)
        Me.btnDeleteDuringEditing.Name = "btnDeleteDuringEditing"
        Me.btnDeleteDuringEditing.Size = New System.Drawing.Size(75, 23)
        Me.btnDeleteDuringEditing.TabIndex = 46
        Me.btnDeleteDuringEditing.Text = "Delete"
        Me.btnDeleteDuringEditing.UseVisualStyleBackColor = True
        '
        'ChkRemoteProcess
        '
        Me.ChkRemoteProcess.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.ChkRemoteProcess.AutoSize = True
        Me.ChkRemoteProcess.Location = New System.Drawing.Point(684, 351)
        Me.ChkRemoteProcess.Name = "ChkRemoteProcess"
        Me.ChkRemoteProcess.Size = New System.Drawing.Size(110, 17)
        Me.ChkRemoteProcess.TabIndex = 47
        Me.ChkRemoteProcess.Text = "Remote Process?"
        Me.ChkRemoteProcess.UseVisualStyleBackColor = True
        '
        'btnResetHits
        '
        Me.btnResetHits.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnResetHits.Location = New System.Drawing.Point(326, 251)
        Me.btnResetHits.Name = "btnResetHits"
        Me.btnResetHits.Size = New System.Drawing.Size(75, 23)
        Me.btnResetHits.TabIndex = 48
        Me.btnResetHits.Text = "Reset Hits"
        Me.btnResetHits.UseVisualStyleBackColor = True
        '
        'txtComment
        '
        Me.txtComment.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtComment.Location = New System.Drawing.Point(73, 374)
        Me.txtComment.Name = "txtComment"
        Me.txtComment.Size = New System.Drawing.Size(919, 20)
        Me.txtComment.TabIndex = 49
        '
        'lblCommentLabel
        '
        Me.lblCommentLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblCommentLabel.AutoSize = True
        Me.lblCommentLabel.Location = New System.Drawing.Point(16, 377)
        Me.lblCommentLabel.Name = "lblCommentLabel"
        Me.lblCommentLabel.Size = New System.Drawing.Size(51, 13)
        Me.lblCommentLabel.TabIndex = 50
        Me.lblCommentLabel.Text = "Comment"
        '
        'lblTotalHits
        '
        Me.lblTotalHits.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblTotalHits.AutoSize = True
        Me.lblTotalHits.Location = New System.Drawing.Point(583, 256)
        Me.lblTotalHits.Name = "lblTotalHits"
        Me.lblTotalHits.Size = New System.Drawing.Size(103, 13)
        Me.lblTotalHits.TabIndex = 51
        Me.lblTotalHits.Text = "Total Ignored Hits: 0"
        '
        'colDateOfLastEvent
        '
        Me.colDateOfLastEvent.Text = "Date of Last Event"
        Me.colDateOfLastEvent.Width = 240
        '
        'btnUpdateHits
        '
        Me.btnUpdateHits.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnUpdateHits.Location = New System.Drawing.Point(407, 251)
        Me.btnUpdateHits.Name = "btnUpdateHits"
        Me.btnUpdateHits.Size = New System.Drawing.Size(170, 23)
        Me.btnUpdateHits.TabIndex = 52
        Me.btnUpdateHits.Text = "Update Hits and Last Events"
        Me.btnUpdateHits.UseVisualStyleBackColor = True
        '
        'colSinceLastEvent
        '
        Me.colSinceLastEvent.Text = "Since Last Event"
        Me.colSinceLastEvent.Width = 80
        '
        'IgnoredWordsAndPhrases
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1004, 437)
        Me.Controls.Add(Me.btnUpdateHits)
        Me.Controls.Add(Me.lblTotalHits)
        Me.Controls.Add(Me.ChkRemoteProcess)
        Me.Controls.Add(Me.btnDeleteDuringEditing)
        Me.Controls.Add(Me.BtnCancel)
        Me.Controls.Add(Me.ChkEnabled)
        Me.Controls.Add(Me.ChkCaseSensitive)
        Me.Controls.Add(Me.ChkRegex)
        Me.Controls.Add(Me.TxtIgnored)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.SeparatingLine)
        Me.Controls.Add(Me.BtnDown)
        Me.Controls.Add(Me.BtnUp)
        Me.Controls.Add(Me.btnDeleteAll)
        Me.Controls.Add(Me.BtnExport)
        Me.Controls.Add(Me.BtnImport)
        Me.Controls.Add(Me.BtnEnableDisable)
        Me.Controls.Add(Me.BtnEdit)
        Me.Controls.Add(Me.IgnoredListView)
        Me.Controls.Add(Me.BtnDelete)
        Me.Controls.Add(Me.BtnAdd)
        Me.Controls.Add(Me.btnResetHits)
        Me.Controls.Add(Me.lblCommentLabel)
        Me.Controls.Add(Me.txtComment)
        Me.KeyPreview = True
        Me.MinimumSize = New System.Drawing.Size(815, 417)
        Me.Name = "IgnoredWordsAndPhrases"
        Me.Text = "Ignored Words and Phrases"
        Me.ListViewMenu.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents BtnAdd As Button
    Friend WithEvents BtnDelete As Button
    Friend WithEvents IgnoredListView As ListView
    Friend WithEvents Ignored As ColumnHeader
    Friend WithEvents Regex As ColumnHeader
    Friend WithEvents CaseSensitive As ColumnHeader
    Friend WithEvents BtnEdit As Button
    Friend WithEvents ColEnabled As ColumnHeader
    Friend WithEvents ListViewMenu As ContextMenuStrip
    Friend WithEvents EnableDisableToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents BtnEnableDisable As Button
    Friend WithEvents BtnImport As Button
    Friend WithEvents BtnExport As Button
    Friend WithEvents btnDeleteAll As Button
    Friend WithEvents BtnDown As Button
    Friend WithEvents BtnUp As Button
    Friend WithEvents SeparatingLine As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents ChkEnabled As CheckBox
    Friend WithEvents ChkCaseSensitive As CheckBox
    Friend WithEvents ChkRegex As CheckBox
    Friend WithEvents TxtIgnored As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents BtnCancel As Button
    Friend WithEvents btnDeleteDuringEditing As Button
    Friend WithEvents ChkRemoteProcess As CheckBox
    Friend WithEvents colHits As ColumnHeader
    Friend WithEvents colTarget As ColumnHeader
    Friend WithEvents btnResetHits As Button
    Friend WithEvents colDateCreated As ColumnHeader
    Friend WithEvents txtComment As TextBox
    Friend WithEvents lblCommentLabel As Label
    Friend WithEvents ResetHitsToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents lblTotalHits As Label
    Friend WithEvents colDateOfLastEvent As ColumnHeader
    Friend WithEvents btnUpdateHits As Button
    Friend WithEvents colSinceLastEvent As ColumnHeader
End Class
