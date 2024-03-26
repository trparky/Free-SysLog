<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Log_Viewer
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.LogText = New System.Windows.Forms.TextBox()
        Me.BtnClose = New System.Windows.Forms.Button()
        Me.LblLogDate = New System.Windows.Forms.Label()
        Me.LblSource = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'LogText
        '
        Me.LogText.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.LogText.BackColor = System.Drawing.SystemColors.Window
        Me.LogText.Location = New System.Drawing.Point(12, 25)
        Me.LogText.Multiline = True
        Me.LogText.Name = "LogText"
        Me.LogText.ReadOnly = True
        Me.LogText.Size = New System.Drawing.Size(776, 129)
        Me.LogText.TabIndex = 1
        '
        'BtnClose
        '
        Me.BtnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.BtnClose.Location = New System.Drawing.Point(713, 160)
        Me.BtnClose.Name = "btnClose"
        Me.BtnClose.Size = New System.Drawing.Size(75, 23)
        Me.BtnClose.TabIndex = 0
        Me.BtnClose.Text = "&Close"
        Me.BtnClose.UseVisualStyleBackColor = True
        '
        'LblLogDate
        '
        Me.LblLogDate.AutoSize = True
        Me.LblLogDate.Location = New System.Drawing.Point(12, 9)
        Me.LblLogDate.Name = "lblLogDate"
        Me.LblLogDate.Size = New System.Drawing.Size(54, 13)
        Me.LblLogDate.TabIndex = 2
        Me.LblLogDate.Text = "Log Date:"
        '
        'LblSource
        '
        Me.LblSource.AutoSize = True
        Me.LblSource.Location = New System.Drawing.Point(238, 9)
        Me.LblSource.Name = "lblSource"
        Me.LblSource.Size = New System.Drawing.Size(98, 13)
        Me.LblSource.TabIndex = 3
        Me.LblSource.Text = "Source IP Address:"
        '
        'Log_Viewer
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(800, 195)
        Me.Controls.Add(Me.LblSource)
        Me.Controls.Add(Me.LblLogDate)
        Me.Controls.Add(Me.BtnClose)
        Me.Controls.Add(Me.LogText)
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(816, 173)
        Me.Name = "Log_Viewer"
        Me.Text = "Log Viewer"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents LogText As TextBox
    Friend WithEvents BtnClose As Button
    Friend WithEvents LblLogDate As Label
    Friend WithEvents LblSource As Label
End Class
