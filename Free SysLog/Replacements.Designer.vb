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
        Me.replacementsListView = New System.Windows.Forms.ListView()
        Me.Replace = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ReplaceWith = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.Regex = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.btnAdd = New System.Windows.Forms.Button()
        Me.btnDelete = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'replacementsListView
        '
        Me.replacementsListView.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.replacementsListView.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.Replace, Me.ReplaceWith, Me.Regex})
        Me.replacementsListView.FullRowSelect = True
        Me.replacementsListView.HideSelection = False
        Me.replacementsListView.Location = New System.Drawing.Point(12, 12)
        Me.replacementsListView.Name = "replacementsListView"
        Me.replacementsListView.Size = New System.Drawing.Size(791, 397)
        Me.replacementsListView.TabIndex = 4
        Me.replacementsListView.UseCompatibleStateImageBehavior = False
        Me.replacementsListView.View = System.Windows.Forms.View.Details
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
        'btnAdd
        '
        Me.btnAdd.Location = New System.Drawing.Point(12, 415)
        Me.btnAdd.Name = "btnAdd"
        Me.btnAdd.Size = New System.Drawing.Size(75, 23)
        Me.btnAdd.TabIndex = 5
        Me.btnAdd.Text = "Add"
        Me.btnAdd.UseVisualStyleBackColor = True
        '
        'btnDelete
        '
        Me.btnDelete.Location = New System.Drawing.Point(93, 415)
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(75, 23)
        Me.btnDelete.TabIndex = 6
        Me.btnDelete.Text = "Delete"
        Me.btnDelete.UseVisualStyleBackColor = True
        '
        'Replacements
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(815, 450)
        Me.Controls.Add(Me.btnDelete)
        Me.Controls.Add(Me.btnAdd)
        Me.Controls.Add(Me.replacementsListView)
        Me.Name = "Replacements"
        Me.Text = "Replacements"
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents replacementsListView As ListView
    Friend WithEvents Replace As ColumnHeader
    Friend WithEvents ReplaceWith As ColumnHeader
    Friend WithEvents Regex As ColumnHeader
    Friend WithEvents btnAdd As Button
    Friend WithEvents btnDelete As Button
End Class
