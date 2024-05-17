<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class IgnoredWordsAndPhrases
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
        Me.BtnAdd = New System.Windows.Forms.Button()
        Me.BtnDelete = New System.Windows.Forms.Button()
        Me.IgnoredListView = New System.Windows.Forms.ListView()
        Me.Replace = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.Regex = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.CaseSensitive = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.BtnEdit = New System.Windows.Forms.Button()
        Me.ColEnabled = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ListViewMenu = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.EnableDisableToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.BtnEnableDisable = New System.Windows.Forms.Button()
        Me.ListViewMenu.SuspendLayout()
        Me.SuspendLayout()
        '
        'BtnAdd
        '
        Me.BtnAdd.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.BtnAdd.Location = New System.Drawing.Point(12, 236)
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
        Me.BtnDelete.Location = New System.Drawing.Point(83, 236)
        Me.BtnDelete.Name = "BtnDelete"
        Me.BtnDelete.Size = New System.Drawing.Size(65, 23)
        Me.BtnDelete.TabIndex = 2
        Me.BtnDelete.Text = "Delete"
        Me.BtnDelete.UseVisualStyleBackColor = True
        '
        'IgnoredListView
        '
        Me.IgnoredListView.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.IgnoredListView.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.Replace, Me.Regex, Me.CaseSensitive, Me.ColEnabled})
        Me.IgnoredListView.ContextMenuStrip = Me.ListViewMenu
        Me.IgnoredListView.FullRowSelect = True
        Me.IgnoredListView.HideSelection = False
        Me.IgnoredListView.Location = New System.Drawing.Point(12, 12)
        Me.IgnoredListView.Name = "IgnoredListView"
        Me.IgnoredListView.Size = New System.Drawing.Size(630, 218)
        Me.IgnoredListView.TabIndex = 5
        Me.IgnoredListView.UseCompatibleStateImageBehavior = False
        Me.IgnoredListView.View = System.Windows.Forms.View.Details
        '
        'Replace
        '
        Me.Replace.Text = "Replace"
        Me.Replace.Width = 345
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
        'ListViewMenu
        '
        Me.ListViewMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.EnableDisableToolStripMenuItem})
        Me.ListViewMenu.Name = "ContextMenuStrip1"
        Me.ListViewMenu.Size = New System.Drawing.Size(153, 26)
        '
        'EnableDisableToolStripMenuItem
        '
        Me.EnableDisableToolStripMenuItem.Name = "EnableDisableToolStripMenuItem"
        Me.EnableDisableToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
        Me.EnableDisableToolStripMenuItem.Text = "Enable/Disable"
        '
        'BtnEdit
        '
        Me.BtnEdit.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.BtnEdit.Enabled = False
        Me.BtnEdit.Location = New System.Drawing.Point(154, 236)
        Me.BtnEdit.Name = "BtnEdit"
        Me.BtnEdit.Size = New System.Drawing.Size(75, 23)
        Me.BtnEdit.TabIndex = 8
        Me.BtnEdit.Text = "Edit"
        Me.BtnEdit.UseVisualStyleBackColor = True
        '
        'ColEnabled
        '
        Me.ColEnabled.Text = "Enabled"
        '
        'BtnEnableDisable
        '
        Me.BtnEnableDisable.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.BtnEnableDisable.Enabled = False
        Me.BtnEnableDisable.Location = New System.Drawing.Point(235, 236)
        Me.BtnEnableDisable.Name = "BtnEnableDisable"
        Me.BtnEnableDisable.Size = New System.Drawing.Size(75, 23)
        Me.BtnEnableDisable.TabIndex = 10
        Me.BtnEnableDisable.Text = "Disable"
        Me.BtnEnableDisable.UseVisualStyleBackColor = True
        '
        'IgnoredWordsAndPhrases
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(654, 265)
        Me.Controls.Add(Me.BtnEnableDisable)
        Me.Controls.Add(Me.BtnEdit)
        Me.Controls.Add(Me.IgnoredListView)
        Me.Controls.Add(Me.BtnDelete)
        Me.Controls.Add(Me.BtnAdd)
        Me.MinimumSize = New System.Drawing.Size(495, 304)
        Me.Name = "IgnoredWordsAndPhrases"
        Me.Text = "Ignored Words and Phrases"
        Me.ListViewMenu.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents BtnAdd As Button
    Friend WithEvents BtnDelete As Button
    Friend WithEvents IgnoredListView As ListView
    Friend WithEvents Replace As ColumnHeader
    Friend WithEvents Regex As ColumnHeader
    Friend WithEvents CaseSensitive As ColumnHeader
    Friend WithEvents BtnEdit As Button
    Friend WithEvents ColEnabled As ColumnHeader
    Friend WithEvents ListViewMenu As ContextMenuStrip
    Friend WithEvents EnableDisableToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents BtnEnableDisable As Button
End Class
