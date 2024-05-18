<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Replacements
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
        Me.ReplacementsListView = New System.Windows.Forms.ListView()
        Me.Replace = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ReplaceWith = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.Regex = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.CaseSensitive = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.BtnAdd = New System.Windows.Forms.Button()
        Me.BtnDelete = New System.Windows.Forms.Button()
        Me.BtnEdit = New System.Windows.Forms.Button()
        Me.ColEnabled = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ListViewMenu = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.EnableDisableToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.BtnEnableDisable = New System.Windows.Forms.Button()
        Me.BtnExport = New System.Windows.Forms.Button()
        Me.BtnImport = New System.Windows.Forms.Button()
        Me.ListViewMenu.SuspendLayout()
        Me.SuspendLayout()
        '
        'ReplacementsListView
        '
        Me.ReplacementsListView.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ReplacementsListView.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.Replace, Me.ReplaceWith, Me.Regex, Me.CaseSensitive, Me.ColEnabled})
        Me.ReplacementsListView.ContextMenuStrip = Me.ListViewMenu
        Me.ReplacementsListView.FullRowSelect = True
        Me.ReplacementsListView.HideSelection = False
        Me.ReplacementsListView.Location = New System.Drawing.Point(12, 12)
        Me.ReplacementsListView.Name = "ReplacementsListView"
        Me.ReplacementsListView.Size = New System.Drawing.Size(1011, 397)
        Me.ReplacementsListView.TabIndex = 4
        Me.ReplacementsListView.UseCompatibleStateImageBehavior = False
        Me.ReplacementsListView.View = System.Windows.Forms.View.Details
        '
        'Replace
        '
        Me.Replace.Text = "Replace"
        Me.Replace.Width = 345
        '
        'ReplaceWith
        '
        Me.ReplaceWith.Text = "With"
        Me.ReplaceWith.Width = 345
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
        'BtnAdd
        '
        Me.BtnAdd.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.BtnAdd.Location = New System.Drawing.Point(12, 415)
        Me.BtnAdd.Name = "BtnAdd"
        Me.BtnAdd.Size = New System.Drawing.Size(75, 23)
        Me.BtnAdd.TabIndex = 5
        Me.BtnAdd.Text = "Add"
        Me.BtnAdd.UseVisualStyleBackColor = True
        '
        'BtnDelete
        '
        Me.BtnDelete.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.BtnDelete.Enabled = False
        Me.BtnDelete.Location = New System.Drawing.Point(93, 415)
        Me.BtnDelete.Name = "BtnDelete"
        Me.BtnDelete.Size = New System.Drawing.Size(75, 23)
        Me.BtnDelete.TabIndex = 6
        Me.BtnDelete.Text = "Delete"
        Me.BtnDelete.UseVisualStyleBackColor = True
        '
        'BtnEdit
        '
        Me.BtnEdit.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.BtnEdit.Enabled = False
        Me.BtnEdit.Location = New System.Drawing.Point(174, 415)
        Me.BtnEdit.Name = "BtnEdit"
        Me.BtnEdit.Size = New System.Drawing.Size(75, 23)
        Me.BtnEdit.TabIndex = 7
        Me.BtnEdit.Text = "Edit"
        Me.BtnEdit.UseVisualStyleBackColor = True
        '
        'ColEnabled
        '
        Me.ColEnabled.Text = "Enabled"
        '
        'ListViewMenu
        '
        Me.ListViewMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.EnableDisableToolStripMenuItem})
        Me.ListViewMenu.Name = "ContextMenuStrip1"
        Me.ListViewMenu.Size = New System.Drawing.Size(181, 48)
        '
        'EnableDisableToolStripMenuItem
        '
        Me.EnableDisableToolStripMenuItem.Name = "EnableDisableToolStripMenuItem"
        Me.EnableDisableToolStripMenuItem.Size = New System.Drawing.Size(180, 22)
        Me.EnableDisableToolStripMenuItem.Text = "Enable/Disable"
        '
        'BtnEnableDisable
        '
        Me.BtnEnableDisable.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.BtnEnableDisable.Enabled = False
        Me.BtnEnableDisable.Location = New System.Drawing.Point(255, 415)
        Me.BtnEnableDisable.Name = "BtnEnableDisable"
        Me.BtnEnableDisable.Size = New System.Drawing.Size(75, 23)
        Me.BtnEnableDisable.TabIndex = 9
        Me.BtnEnableDisable.Text = "Disable"
        Me.BtnEnableDisable.UseVisualStyleBackColor = True
        '
        'BtnExport
        '
        Me.BtnExport.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.BtnExport.Location = New System.Drawing.Point(867, 415)
        Me.BtnExport.Name = "BtnExport"
        Me.BtnExport.Size = New System.Drawing.Size(75, 23)
        Me.BtnExport.TabIndex = 14
        Me.BtnExport.Text = "Export"
        Me.BtnExport.UseVisualStyleBackColor = True
        '
        'BtnImport
        '
        Me.BtnImport.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.BtnImport.Location = New System.Drawing.Point(948, 415)
        Me.BtnImport.Name = "BtnImport"
        Me.BtnImport.Size = New System.Drawing.Size(75, 23)
        Me.BtnImport.TabIndex = 13
        Me.BtnImport.Text = "Import"
        Me.BtnImport.UseVisualStyleBackColor = True
        '
        'Replacements
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1035, 450)
        Me.Controls.Add(Me.BtnExport)
        Me.Controls.Add(Me.BtnImport)
        Me.Controls.Add(Me.BtnEnableDisable)
        Me.Controls.Add(Me.BtnEdit)
        Me.Controls.Add(Me.BtnDelete)
        Me.Controls.Add(Me.BtnAdd)
        Me.Controls.Add(Me.ReplacementsListView)
        Me.MinimumSize = New System.Drawing.Size(917, 489)
        Me.Name = "Replacements"
        Me.Text = "Replacements"
        Me.ListViewMenu.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents ReplacementsListView As ListView
    Friend WithEvents Replace As ColumnHeader
    Friend WithEvents ReplaceWith As ColumnHeader
    Friend WithEvents Regex As ColumnHeader
    Friend WithEvents BtnAdd As Button
    Friend WithEvents BtnDelete As Button
    Friend WithEvents CaseSensitive As ColumnHeader
    Friend WithEvents BtnEdit As Button
    Friend WithEvents ColEnabled As ColumnHeader
    Friend WithEvents ListViewMenu As ContextMenuStrip
    Friend WithEvents EnableDisableToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents BtnEnableDisable As Button
    Friend WithEvents BtnExport As Button
    Friend WithEvents BtnImport As Button
End Class
