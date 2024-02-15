<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Ignored_Words_and_Phrases
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
        Me.listOfWords = New System.Windows.Forms.ListView()
        Me.btnAdd = New System.Windows.Forms.Button()
        Me.ColumnHeader1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.btnDelete = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'listOfWords
        '
        Me.listOfWords.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1})
        Me.listOfWords.FullRowSelect = True
        Me.listOfWords.HideSelection = False
        Me.listOfWords.Location = New System.Drawing.Point(12, 12)
        Me.listOfWords.Name = "listOfWords"
        Me.listOfWords.Size = New System.Drawing.Size(457, 218)
        Me.listOfWords.TabIndex = 0
        Me.listOfWords.UseCompatibleStateImageBehavior = False
        Me.listOfWords.View = System.Windows.Forms.View.Details
        '
        'btnAdd
        '
        Me.btnAdd.Location = New System.Drawing.Point(12, 236)
        Me.btnAdd.Name = "btnAdd"
        Me.btnAdd.Size = New System.Drawing.Size(65, 23)
        Me.btnAdd.TabIndex = 1
        Me.btnAdd.Text = "Add"
        Me.btnAdd.UseVisualStyleBackColor = True
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "Ignored Word/Phrase"
        Me.ColumnHeader1.Width = 425
        '
        'btnDelete
        '
        Me.btnDelete.Enabled = False
        Me.btnDelete.Location = New System.Drawing.Point(83, 236)
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(65, 23)
        Me.btnDelete.TabIndex = 2
        Me.btnDelete.Text = "Delete"
        Me.btnDelete.UseVisualStyleBackColor = True
        '
        'Ignored_Words_and_Phrases
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(479, 265)
        Me.Controls.Add(Me.btnDelete)
        Me.Controls.Add(Me.btnAdd)
        Me.Controls.Add(Me.listOfWords)
        Me.Name = "Ignored_Words_and_Phrases"
        Me.Text = "Ignored Words and Phrases"
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents listOfWords As ListView
    Friend WithEvents btnAdd As Button
    Friend WithEvents ColumnHeader1 As ColumnHeader
    Friend WithEvents btnDelete As Button
End Class
