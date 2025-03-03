<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class CloseFreeSysLogDialog
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
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.BtnNo = New System.Windows.Forms.Button()
        Me.BtnYes = New System.Windows.Forms.Button()
        Me.BtnMinimize = New System.Windows.Forms.Button()
        Me.ToolTip = New System.Windows.Forms.ToolTip(Me.components)
        Me.ChkConfirmClose = New System.Windows.Forms.CheckBox()
        Me.Panel1.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.White
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Controls.Add(Me.PictureBox1)
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(293, 58)
        Me.Panel1.TabIndex = 2
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(50, 14)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(220, 13)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = "Are you sure you want to close Free SysLog?"
        '
        'PictureBox1
        '
        Me.PictureBox1.Location = New System.Drawing.Point(12, 14)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(32, 32)
        Me.PictureBox1.TabIndex = 2
        Me.PictureBox1.TabStop = False
        '
        'BtnNo
        '
        Me.BtnNo.Location = New System.Drawing.Point(123, 64)
        Me.BtnNo.Name = "BtnNo"
        Me.BtnNo.Size = New System.Drawing.Size(75, 23)
        Me.BtnNo.TabIndex = 0
        Me.BtnNo.Text = "&No"
        Me.ToolTip.SetToolTip(Me.BtnNo, "Can be activated by pressing the N key.")
        Me.BtnNo.UseVisualStyleBackColor = True
        '
        'BtnYes
        '
        Me.BtnYes.Location = New System.Drawing.Point(42, 64)
        Me.BtnYes.Name = "BtnYes"
        Me.BtnYes.Size = New System.Drawing.Size(75, 23)
        Me.BtnYes.TabIndex = 2
        Me.BtnYes.Text = "&Yes"
        Me.ToolTip.SetToolTip(Me.BtnYes, "Can be activated by pressing the Y key.")
        Me.BtnYes.UseVisualStyleBackColor = True
        '
        'BtnMinimize
        '
        Me.BtnMinimize.Location = New System.Drawing.Point(204, 64)
        Me.BtnMinimize.Name = "BtnMinimize"
        Me.BtnMinimize.Size = New System.Drawing.Size(75, 23)
        Me.BtnMinimize.TabIndex = 1
        Me.BtnMinimize.Text = "&Minimize"
        Me.ToolTip.SetToolTip(Me.BtnMinimize, "Can be activated by pressing the M key.")
        Me.BtnMinimize.UseVisualStyleBackColor = True
        '
        'ChkConfirmClose
        '
        Me.ChkConfirmClose.AutoSize = True
        Me.ChkConfirmClose.Location = New System.Drawing.Point(12, 93)
        Me.ChkConfirmClose.Name = "ChkConfirmClose"
        Me.ChkConfirmClose.Size = New System.Drawing.Size(90, 17)
        Me.ChkConfirmClose.TabIndex = 3
        Me.ChkConfirmClose.Text = "Confirm Close"
        Me.ChkConfirmClose.UseVisualStyleBackColor = True
        '
        'CloseFreeSysLogDialog
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(291, 116)
        Me.Controls.Add(Me.ChkConfirmClose)
        Me.Controls.Add(Me.BtnMinimize)
        Me.Controls.Add(Me.BtnYes)
        Me.Controls.Add(Me.BtnNo)
        Me.Controls.Add(Me.Panel1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "CloseFreeSysLogDialog"
        Me.Text = "Close Free SysLog?"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Panel1 As Panel
    Friend WithEvents Label1 As Label
    Friend WithEvents PictureBox1 As PictureBox
    Friend WithEvents BtnNo As Button
    Friend WithEvents BtnYes As Button
    Friend WithEvents BtnMinimize As Button
    Friend WithEvents ToolTip As ToolTip
    Friend WithEvents ChkConfirmClose As CheckBox
End Class
