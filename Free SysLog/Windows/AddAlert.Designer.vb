<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class AddAlert
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
        Me.ChkCaseSensitive = New System.Windows.Forms.CheckBox()
        Me.BtnAdd = New System.Windows.Forms.Button()
        Me.ChkRegex = New System.Windows.Forms.CheckBox()
        Me.TxtLogText = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.TxtAlertText = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.ToolTip = New System.Windows.Forms.ToolTip(Me.components)
        Me.SuspendLayout()
        '
        'ChkCaseSensitive
        '
        Me.ChkCaseSensitive.AutoSize = True
        Me.ChkCaseSensitive.Location = New System.Drawing.Point(542, 61)
        Me.ChkCaseSensitive.Name = "ChkCaseSensitive"
        Me.ChkCaseSensitive.Size = New System.Drawing.Size(102, 17)
        Me.ChkCaseSensitive.TabIndex = 18
        Me.ChkCaseSensitive.Text = "Case Sensitive?"
        Me.ChkCaseSensitive.UseVisualStyleBackColor = True
        '
        'BtnAdd
        '
        Me.BtnAdd.Location = New System.Drawing.Point(12, 84)
        Me.BtnAdd.Name = "BtnAdd"
        Me.BtnAdd.Size = New System.Drawing.Size(75, 23)
        Me.BtnAdd.TabIndex = 17
        Me.BtnAdd.Text = "Add"
        Me.BtnAdd.UseVisualStyleBackColor = True
        '
        'ChkRegex
        '
        Me.ChkRegex.AutoSize = True
        Me.ChkRegex.Location = New System.Drawing.Point(15, 61)
        Me.ChkRegex.Name = "ChkRegex"
        Me.ChkRegex.Size = New System.Drawing.Size(478, 17)
        Me.ChkRegex.TabIndex = 16
        Me.ChkRegex.Text = "Regex? (Be careful with Regex, a broken regex pattern could cause the program to " &
    "malfunction)"
        Me.ChkRegex.UseVisualStyleBackColor = True
        '
        'TxtLogText
        '
        Me.TxtLogText.Location = New System.Drawing.Point(67, 6)
        Me.TxtLogText.Name = "TxtLogText"
        Me.TxtLogText.Size = New System.Drawing.Size(721, 20)
        Me.TxtLogText.TabIndex = 15
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(49, 13)
        Me.Label1.TabIndex = 14
        Me.Label1.Text = "Log Text"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'TxtAlertText
        '
        Me.TxtAlertText.Location = New System.Drawing.Point(67, 32)
        Me.TxtAlertText.Name = "TxtAlertText"
        Me.TxtAlertText.Size = New System.Drawing.Size(721, 20)
        Me.TxtAlertText.TabIndex = 19
        Me.ToolTip.SetToolTip(Me.TxtAlertText, "What's shown in the balloon alert." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Leave blank if you want to show the log text " &
        "itself.")
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(12, 35)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(52, 13)
        Me.Label2.TabIndex = 20
        Me.Label2.Text = "Alert Text"
        '
        'AddAlert
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(800, 116)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.TxtAlertText)
        Me.Controls.Add(Me.ChkCaseSensitive)
        Me.Controls.Add(Me.BtnAdd)
        Me.Controls.Add(Me.ChkRegex)
        Me.Controls.Add(Me.TxtLogText)
        Me.Controls.Add(Me.Label1)
        Me.Name = "AddAlert"
        Me.Text = "Add Alert"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents ChkCaseSensitive As CheckBox
    Friend WithEvents BtnAdd As Button
    Friend WithEvents ChkRegex As CheckBox
    Friend WithEvents TxtLogText As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents TxtAlertText As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents ToolTip As ToolTip
End Class
