<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class IntegerInputForm
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
        Me.lblSetting = New System.Windows.Forms.Label()
        Me.TxtSetting = New System.Windows.Forms.TextBox()
        Me.BtnUp = New System.Windows.Forms.Button()
        Me.BtnDown = New System.Windows.Forms.Button()
        Me.BtnSave = New System.Windows.Forms.Button()
        Me.BtnCancel = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'lblSetting
        '
        Me.lblSetting.AutoSize = True
        Me.lblSetting.Location = New System.Drawing.Point(12, 9)
        Me.lblSetting.Name = "lblSetting"
        Me.lblSetting.Size = New System.Drawing.Size(40, 13)
        Me.lblSetting.TabIndex = 0
        Me.lblSetting.Text = "Setting"
        '
        'TxtSetting
        '
        Me.TxtSetting.Location = New System.Drawing.Point(15, 26)
        Me.TxtSetting.Name = "TxtSetting"
        Me.TxtSetting.Size = New System.Drawing.Size(186, 20)
        Me.TxtSetting.TabIndex = 1
        '
        'BtnUp
        '
        Me.BtnUp.Location = New System.Drawing.Point(207, 9)
        Me.BtnUp.Name = "BtnUp"
        Me.BtnUp.Size = New System.Drawing.Size(24, 23)
        Me.BtnUp.TabIndex = 2
        Me.BtnUp.Text = "▲"
        Me.BtnUp.UseVisualStyleBackColor = True
        '
        'BtnDown
        '
        Me.BtnDown.Location = New System.Drawing.Point(207, 32)
        Me.BtnDown.Name = "BtnDown"
        Me.BtnDown.Size = New System.Drawing.Size(24, 23)
        Me.BtnDown.TabIndex = 3
        Me.BtnDown.Text = "▼"
        Me.BtnDown.UseVisualStyleBackColor = True
        '
        'BtnSave
        '
        Me.BtnSave.Location = New System.Drawing.Point(15, 52)
        Me.BtnSave.Name = "BtnSave"
        Me.BtnSave.Size = New System.Drawing.Size(90, 23)
        Me.BtnSave.TabIndex = 4
        Me.BtnSave.Text = "Save"
        Me.BtnSave.UseVisualStyleBackColor = True
        '
        'BtnCancel
        '
        Me.BtnCancel.Location = New System.Drawing.Point(111, 52)
        Me.BtnCancel.Name = "BtnCancel"
        Me.BtnCancel.Size = New System.Drawing.Size(90, 23)
        Me.BtnCancel.TabIndex = 5
        Me.BtnCancel.Text = "Cancel"
        Me.BtnCancel.UseVisualStyleBackColor = True
        '
        'IntegerInputForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(243, 84)
        Me.Controls.Add(Me.BtnCancel)
        Me.Controls.Add(Me.BtnSave)
        Me.Controls.Add(Me.BtnDown)
        Me.Controls.Add(Me.BtnUp)
        Me.Controls.Add(Me.TxtSetting)
        Me.Controls.Add(Me.lblSetting)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "IntegerInputForm"
        Me.Text = "Integer Input Form"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents lblSetting As Label
    Friend WithEvents TxtSetting As TextBox
    Friend WithEvents BtnUp As Button
    Friend WithEvents BtnDown As Button
    Friend WithEvents BtnSave As Button
    Friend WithEvents BtnCancel As Button
End Class
