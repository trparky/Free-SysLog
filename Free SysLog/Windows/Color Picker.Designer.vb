<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Color_Picker
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
        Me.ColorWheel1 = New Cyotek.Windows.Forms.ColorWheel()
        Me.ColorGrid1 = New Cyotek.Windows.Forms.ColorGrid()
        Me.ColorEditor1 = New Cyotek.Windows.Forms.ColorEditor()
        Me.lblColorShower = New System.Windows.Forms.Label()
        Me.btnChooseColor = New System.Windows.Forms.Button()
        Me.btnSaveColorToCustomColors = New System.Windows.Forms.Button()
        Me.btnClearCustomColors = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'ColorWheel1
        '
        Me.ColorWheel1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ColorWheel1.Color = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.ColorWheel1.Location = New System.Drawing.Point(471, 12)
        Me.ColorWheel1.Name = "ColorWheel1"
        Me.ColorWheel1.Size = New System.Drawing.Size(239, 260)
        Me.ColorWheel1.TabIndex = 0
        '
        'ColorGrid1
        '
        Me.ColorGrid1.Location = New System.Drawing.Point(12, 12)
        Me.ColorGrid1.Name = "ColorGrid1"
        Me.ColorGrid1.Size = New System.Drawing.Size(247, 165)
        Me.ColorGrid1.TabIndex = 1
        '
        'ColorEditor1
        '
        Me.ColorEditor1.Location = New System.Drawing.Point(265, 12)
        Me.ColorEditor1.Name = "ColorEditor1"
        Me.ColorEditor1.Size = New System.Drawing.Size(200, 260)
        Me.ColorEditor1.TabIndex = 2
        '
        'lblColorShower
        '
        Me.lblColorShower.Location = New System.Drawing.Point(12, 191)
        Me.lblColorShower.Name = "lblColorShower"
        Me.lblColorShower.Size = New System.Drawing.Size(247, 60)
        Me.lblColorShower.TabIndex = 3
        '
        'btnChooseColor
        '
        Me.btnChooseColor.Location = New System.Drawing.Point(12, 254)
        Me.btnChooseColor.Name = "btnChooseColor"
        Me.btnChooseColor.Size = New System.Drawing.Size(117, 23)
        Me.btnChooseColor.TabIndex = 4
        Me.btnChooseColor.Text = "Choose Color"
        Me.btnChooseColor.UseVisualStyleBackColor = True
        '
        'btnSaveColorToCustomColors
        '
        Me.btnSaveColorToCustomColors.Location = New System.Drawing.Point(135, 254)
        Me.btnSaveColorToCustomColors.Name = "btnSaveColorToCustomColors"
        Me.btnSaveColorToCustomColors.Size = New System.Drawing.Size(176, 23)
        Me.btnSaveColorToCustomColors.TabIndex = 5
        Me.btnSaveColorToCustomColors.Text = "Save Color to Custom Colors"
        Me.btnSaveColorToCustomColors.UseVisualStyleBackColor = True
        '
        'btnClearCustomColors
        '
        Me.btnClearCustomColors.Location = New System.Drawing.Point(317, 254)
        Me.btnClearCustomColors.Name = "btnClearCustomColors"
        Me.btnClearCustomColors.Size = New System.Drawing.Size(148, 23)
        Me.btnClearCustomColors.TabIndex = 6
        Me.btnClearCustomColors.Text = "Clear Custom Colors"
        Me.btnClearCustomColors.UseVisualStyleBackColor = True
        '
        'Color_Picker
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(717, 287)
        Me.Controls.Add(Me.btnClearCustomColors)
        Me.Controls.Add(Me.btnSaveColorToCustomColors)
        Me.Controls.Add(Me.btnChooseColor)
        Me.Controls.Add(Me.lblColorShower)
        Me.Controls.Add(Me.ColorEditor1)
        Me.Controls.Add(Me.ColorGrid1)
        Me.Controls.Add(Me.ColorWheel1)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(733, 326)
        Me.Name = "Color_Picker"
        Me.Text = "Color Picker"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents ColorWheel1 As Cyotek.Windows.Forms.ColorWheel
    Friend WithEvents ColorGrid1 As Cyotek.Windows.Forms.ColorGrid
    Friend WithEvents ColorEditor1 As Cyotek.Windows.Forms.ColorEditor
    Friend WithEvents lblColorShower As Label
    Friend WithEvents btnChooseColor As Button
    Friend WithEvents btnSaveColorToCustomColors As Button
    Friend WithEvents btnClearCustomColors As Button
End Class
