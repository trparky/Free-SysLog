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
        Me.ListOfWords = New System.Windows.Forms.ListView()
        Me.BtnAdd = New System.Windows.Forms.Button()
        Me.ColumnHeader1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.BtnDelete = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'ListOfWords
        '
        Me.ListOfWords.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1})
        Me.ListOfWords.FullRowSelect = True
        Me.ListOfWords.HideSelection = False
        Me.ListOfWords.Location = New System.Drawing.Point(12, 12)
        Me.ListOfWords.Name = "listOfWords"
        Me.ListOfWords.Size = New System.Drawing.Size(457, 218)
        Me.ListOfWords.TabIndex = 0
        Me.ListOfWords.UseCompatibleStateImageBehavior = False
        Me.ListOfWords.View = System.Windows.Forms.View.Details
        '
        'BtnAdd
        '
        Me.BtnAdd.Location = New System.Drawing.Point(12, 236)
        Me.BtnAdd.Name = "btnAdd"
        Me.BtnAdd.Size = New System.Drawing.Size(65, 23)
        Me.BtnAdd.TabIndex = 1
        Me.BtnAdd.Text = "Add"
        Me.BtnAdd.UseVisualStyleBackColor = True
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "Ignored Word/Phrase"
        Me.ColumnHeader1.Width = 425
        '
        'BtnDelete
        '
        Me.BtnDelete.Enabled = False
        Me.BtnDelete.Location = New System.Drawing.Point(83, 236)
        Me.BtnDelete.Name = "btnDelete"
        Me.BtnDelete.Size = New System.Drawing.Size(65, 23)
        Me.BtnDelete.TabIndex = 2
        Me.BtnDelete.Text = "Delete"
        Me.BtnDelete.UseVisualStyleBackColor = True
        '
        'Ignored_Words_and_Phrases
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(479, 265)
        Me.Controls.Add(Me.BtnDelete)
        Me.Controls.Add(Me.BtnAdd)
        Me.Controls.Add(Me.ListOfWords)
        Me.MinimumSize = New System.Drawing.Size(495, 304)
        Me.Name = "Ignored_Words_and_Phrases"
        Me.Text = "Ignored Words and Phrases"
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents ListOfWords As ListView
    Friend WithEvents BtnAdd As Button
    Friend WithEvents ColumnHeader1 As ColumnHeader
    Friend WithEvents BtnDelete As Button
End Class
