﻿<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
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
        Me.btnClose = New System.Windows.Forms.Button()
        Me.lblLogDate = New System.Windows.Forms.Label()
        Me.lblSource = New System.Windows.Forms.Label()
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
        'btnClose
        '
        Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClose.Location = New System.Drawing.Point(713, 160)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(75, 23)
        Me.btnClose.TabIndex = 0
        Me.btnClose.Text = "&Close"
        Me.btnClose.UseVisualStyleBackColor = True
        '
        'lblLogDate
        '
        Me.lblLogDate.AutoSize = True
        Me.lblLogDate.Location = New System.Drawing.Point(12, 9)
        Me.lblLogDate.Name = "lblLogDate"
        Me.lblLogDate.Size = New System.Drawing.Size(54, 13)
        Me.lblLogDate.TabIndex = 2
        Me.lblLogDate.Text = "Log Date:"
        '
        'lblSource
        '
        Me.lblSource.AutoSize = True
        Me.lblSource.Location = New System.Drawing.Point(238, 9)
        Me.lblSource.Name = "lblSource"
        Me.lblSource.Size = New System.Drawing.Size(98, 13)
        Me.lblSource.TabIndex = 3
        Me.lblSource.Text = "Source IP Address:"
        '
        'Log_Viewer
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(800, 195)
        Me.Controls.Add(Me.lblSource)
        Me.Controls.Add(Me.lblLogDate)
        Me.Controls.Add(Me.btnClose)
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
    Friend WithEvents btnClose As Button
    Friend WithEvents lblLogDate As Label
    Friend WithEvents lblSource As Label
End Class
