<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class AddReplacement
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
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtReplace = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtReplaceWith = New System.Windows.Forms.TextBox()
        Me.chkRegex = New System.Windows.Forms.CheckBox()
        Me.btnAdd = New System.Windows.Forms.Button()
        Me.chkCaseSensitive = New System.Windows.Forms.CheckBox()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(47, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Replace"
        '
        'txtReplace
        '
        Me.txtReplace.Location = New System.Drawing.Point(65, 6)
        Me.txtReplace.Name = "txtReplace"
        Me.txtReplace.Size = New System.Drawing.Size(723, 20)
        Me.txtReplace.TabIndex = 1
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(30, 35)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(29, 13)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "With"
        '
        'txtReplaceWith
        '
        Me.txtReplaceWith.Location = New System.Drawing.Point(65, 32)
        Me.txtReplaceWith.Name = "txtReplaceWith"
        Me.txtReplaceWith.Size = New System.Drawing.Size(723, 20)
        Me.txtReplaceWith.TabIndex = 3
        '
        'chkRegex
        '
        Me.chkRegex.AutoSize = True
        Me.chkRegex.Location = New System.Drawing.Point(15, 58)
        Me.chkRegex.Name = "chkRegex"
        Me.chkRegex.Size = New System.Drawing.Size(478, 17)
        Me.chkRegex.TabIndex = 4
        Me.chkRegex.Text = "Regex? (Be careful with Regex, a broken regex pattern could cause the program to " &
    "malfunction)"
        Me.chkRegex.UseVisualStyleBackColor = True
        '
        'btnAdd
        '
        Me.btnAdd.Location = New System.Drawing.Point(12, 81)
        Me.btnAdd.Name = "btnAdd"
        Me.btnAdd.Size = New System.Drawing.Size(75, 23)
        Me.btnAdd.TabIndex = 5
        Me.btnAdd.Text = "Add"
        Me.btnAdd.UseVisualStyleBackColor = True
        '
        'chkCaseSensitive
        '
        Me.chkCaseSensitive.AutoSize = True
        Me.chkCaseSensitive.Location = New System.Drawing.Point(542, 58)
        Me.chkCaseSensitive.Name = "chkCaseSensitive"
        Me.chkCaseSensitive.Size = New System.Drawing.Size(102, 17)
        Me.chkCaseSensitive.TabIndex = 6
        Me.chkCaseSensitive.Text = "Case Sensitive?"
        Me.chkCaseSensitive.UseVisualStyleBackColor = True
        '
        'AddReplacement
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(800, 112)
        Me.Controls.Add(Me.chkCaseSensitive)
        Me.Controls.Add(Me.btnAdd)
        Me.Controls.Add(Me.chkRegex)
        Me.Controls.Add(Me.txtReplaceWith)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.txtReplace)
        Me.Controls.Add(Me.Label1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "AddReplacement"
        Me.Text = "AddReplacement"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Label1 As Label
    Friend WithEvents txtReplace As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents txtReplaceWith As TextBox
    Friend WithEvents chkRegex As CheckBox
    Friend WithEvents btnAdd As Button
    Friend WithEvents chkCaseSensitive As CheckBox
End Class
