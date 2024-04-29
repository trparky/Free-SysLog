<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class AddIgnored
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
        Me.ChkCaseSensitive = New System.Windows.Forms.CheckBox()
        Me.BtnAdd = New System.Windows.Forms.Button()
        Me.ChkRegex = New System.Windows.Forms.CheckBox()
        Me.TxtIgnored = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'ChkCaseSensitive
        '
        Me.ChkCaseSensitive.AutoSize = True
        Me.ChkCaseSensitive.Location = New System.Drawing.Point(542, 32)
        Me.ChkCaseSensitive.Name = "ChkCaseSensitive"
        Me.ChkCaseSensitive.Size = New System.Drawing.Size(102, 17)
        Me.ChkCaseSensitive.TabIndex = 13
        Me.ChkCaseSensitive.Text = "Case Sensitive?"
        Me.ChkCaseSensitive.UseVisualStyleBackColor = True
        '
        'BtnAdd
        '
        Me.BtnAdd.Location = New System.Drawing.Point(12, 55)
        Me.BtnAdd.Name = "BtnAdd"
        Me.BtnAdd.Size = New System.Drawing.Size(75, 23)
        Me.BtnAdd.TabIndex = 12
        Me.BtnAdd.Text = "Add"
        Me.BtnAdd.UseVisualStyleBackColor = True
        '
        'ChkRegex
        '
        Me.ChkRegex.AutoSize = True
        Me.ChkRegex.Location = New System.Drawing.Point(15, 32)
        Me.ChkRegex.Name = "ChkRegex"
        Me.ChkRegex.Size = New System.Drawing.Size(478, 17)
        Me.ChkRegex.TabIndex = 11
        Me.ChkRegex.Text = "Regex? (Be careful with Regex, a broken regex pattern could cause the program to " &
    "malfunction)"
        Me.ChkRegex.UseVisualStyleBackColor = True
        '
        'TxtIgnored
        '
        Me.TxtIgnored.Location = New System.Drawing.Point(61, 6)
        Me.TxtIgnored.Name = "TxtIgnored"
        Me.TxtIgnored.Size = New System.Drawing.Size(727, 20)
        Me.TxtIgnored.TabIndex = 8
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(43, 13)
        Me.Label1.TabIndex = 7
        Me.Label1.Text = "Ignored"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'AddIgnored
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(798, 83)
        Me.Controls.Add(Me.ChkCaseSensitive)
        Me.Controls.Add(Me.BtnAdd)
        Me.Controls.Add(Me.ChkRegex)
        Me.Controls.Add(Me.TxtIgnored)
        Me.Controls.Add(Me.Label1)
        Me.KeyPreview = True
        Me.Name = "AddIgnored"
        Me.Text = "Add Ignored"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents ChkCaseSensitive As CheckBox
    Friend WithEvents BtnAdd As Button
    Friend WithEvents ChkRegex As CheckBox
    Friend WithEvents TxtIgnored As TextBox
    Friend WithEvents Label1 As Label
End Class
