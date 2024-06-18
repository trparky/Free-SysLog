<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class DateFormatChooser
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(DateFormatChooser))
        Me.DateFormat1 = New System.Windows.Forms.RadioButton()
        Me.DateFormat2 = New System.Windows.Forms.RadioButton()
        Me.DateFormat3 = New System.Windows.Forms.RadioButton()
        Me.TxtCustom = New System.Windows.Forms.TextBox()
        Me.lblCustom = New System.Windows.Forms.Label()
        Me.lblExplain = New System.Windows.Forms.Label()
        Me.lblCustomDateOutput = New System.Windows.Forms.Label()
        Me.BtnSave = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'DateFormat1
        '
        Me.DateFormat1.AutoSize = True
        Me.DateFormat1.Location = New System.Drawing.Point(12, 50)
        Me.DateFormat1.Name = "DateFormat1"
        Me.DateFormat1.Size = New System.Drawing.Size(90, 17)
        Me.DateFormat1.TabIndex = 0
        Me.DateFormat1.TabStop = True
        Me.DateFormat1.Text = "RadioButton1"
        Me.DateFormat1.UseVisualStyleBackColor = True
        '
        'DateFormat2
        '
        Me.DateFormat2.AutoSize = True
        Me.DateFormat2.Location = New System.Drawing.Point(12, 73)
        Me.DateFormat2.Name = "DateFormat2"
        Me.DateFormat2.Size = New System.Drawing.Size(90, 17)
        Me.DateFormat2.TabIndex = 1
        Me.DateFormat2.TabStop = True
        Me.DateFormat2.Text = "RadioButton1"
        Me.DateFormat2.UseVisualStyleBackColor = True
        '
        'DateFormat3
        '
        Me.DateFormat3.AutoSize = True
        Me.DateFormat3.Location = New System.Drawing.Point(12, 96)
        Me.DateFormat3.Name = "DateFormat3"
        Me.DateFormat3.Size = New System.Drawing.Size(95, 17)
        Me.DateFormat3.TabIndex = 2
        Me.DateFormat3.TabStop = True
        Me.DateFormat3.Text = "Custom Format"
        Me.DateFormat3.UseVisualStyleBackColor = True
        '
        'TxtCustom
        '
        Me.TxtCustom.Enabled = False
        Me.TxtCustom.Location = New System.Drawing.Point(92, 119)
        Me.TxtCustom.Name = "TxtCustom"
        Me.TxtCustom.Size = New System.Drawing.Size(168, 20)
        Me.TxtCustom.TabIndex = 3
        Me.TxtCustom.Text = "MM-dd-yyyy"
        '
        'lblCustom
        '
        Me.lblCustom.AutoSize = True
        Me.lblCustom.Location = New System.Drawing.Point(9, 122)
        Me.lblCustom.Name = "lblCustom"
        Me.lblCustom.Size = New System.Drawing.Size(77, 13)
        Me.lblCustom.TabIndex = 4
        Me.lblCustom.Text = "Custom Format"
        '
        'lblExplain
        '
        Me.lblExplain.AutoSize = True
        Me.lblExplain.Location = New System.Drawing.Point(9, 148)
        Me.lblExplain.Name = "lblExplain"
        Me.lblExplain.Size = New System.Drawing.Size(313, 208)
        Me.lblExplain.TabIndex = 5
        Me.lblExplain.Text = resources.GetString("lblExplain.Text")
        '
        'lblCustomDateOutput
        '
        Me.lblCustomDateOutput.AutoSize = True
        Me.lblCustomDateOutput.Location = New System.Drawing.Point(266, 122)
        Me.lblCustomDateOutput.Name = "lblCustomDateOutput"
        Me.lblCustomDateOutput.Size = New System.Drawing.Size(39, 13)
        Me.lblCustomDateOutput.TabIndex = 6
        Me.lblCustomDateOutput.Text = "Label1"
        '
        'BtnSave
        '
        Me.BtnSave.Location = New System.Drawing.Point(185, 148)
        Me.BtnSave.Name = "BtnSave"
        Me.BtnSave.Size = New System.Drawing.Size(75, 23)
        Me.BtnSave.TabIndex = 7
        Me.BtnSave.Text = "Save"
        Me.BtnSave.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(442, 26)
        Me.Label1.TabIndex = 8
        Me.Label1.Text = "Be very careful using this tool." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "This is an advanced tool and preference for tho" &
    "se who known how to construct date strings."
        '
        'DateFormatChooser
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(473, 366)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.BtnSave)
        Me.Controls.Add(Me.lblCustomDateOutput)
        Me.Controls.Add(Me.lblExplain)
        Me.Controls.Add(Me.lblCustom)
        Me.Controls.Add(Me.TxtCustom)
        Me.Controls.Add(Me.DateFormat3)
        Me.Controls.Add(Me.DateFormat2)
        Me.Controls.Add(Me.DateFormat1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "DateFormatChooser"
        Me.Text = "Backup File Name Date Format Chooser"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents DateFormat1 As RadioButton
    Friend WithEvents DateFormat2 As RadioButton
    Friend WithEvents DateFormat3 As RadioButton
    Friend WithEvents TxtCustom As TextBox
    Friend WithEvents lblCustom As Label
    Friend WithEvents lblExplain As Label
    Friend WithEvents lblCustomDateOutput As Label
    Friend WithEvents BtnSave As Button
    Friend WithEvents Label1 As Label
End Class
