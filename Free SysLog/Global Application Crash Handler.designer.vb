<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmCrash
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
        Me.txtStackTrace = New System.Windows.Forms.TextBox()
        Me.btnSubmitCrashData = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtWhatWereYouDoing = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtEmailAddress = New System.Windows.Forms.TextBox()
        Me.lblHeader = New System.Windows.Forms.Label()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.SuspendLayout()
        '
        'txtStackTrace
        '
        Me.txtStackTrace.BackColor = System.Drawing.SystemColors.Window
        Me.txtStackTrace.Location = New System.Drawing.Point(12, 55)
        Me.txtStackTrace.Multiline = True
        Me.txtStackTrace.Name = "txtStackTrace"
        Me.txtStackTrace.ReadOnly = True
        Me.txtStackTrace.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtStackTrace.Size = New System.Drawing.Size(602, 178)
        Me.txtStackTrace.TabIndex = 2
        '
        'btnSubmitCrashData
        '
        Me.btnSubmitCrashData.Location = New System.Drawing.Point(349, 456)
        Me.btnSubmitCrashData.Name = "btnSubmitCrashData"
        Me.btnSubmitCrashData.Size = New System.Drawing.Size(120, 23)
        Me.btnSubmitCrashData.TabIndex = 3
        Me.btnSubmitCrashData.Text = "Submit Crash Data"
        Me.btnSubmitCrashData.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 39)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(102, 13)
        Me.Label1.TabIndex = 4
        Me.Label1.Text = "Detailed Crash Data"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(9, 253)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(522, 13)
        Me.Label2.TabIndex = 5
        Me.Label2.Text = "Please explain what you were doing at the time of the crash.  Please be as detail" & _
    "ed as possible. (English Only)"
        '
        'txtWhatWereYouDoing
        '
        Me.txtWhatWereYouDoing.BackColor = System.Drawing.SystemColors.Window
        Me.txtWhatWereYouDoing.Location = New System.Drawing.Point(12, 271)
        Me.txtWhatWereYouDoing.Multiline = True
        Me.txtWhatWereYouDoing.Name = "txtWhatWereYouDoing"
        Me.txtWhatWereYouDoing.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtWhatWereYouDoing.Size = New System.Drawing.Size(602, 178)
        Me.txtWhatWereYouDoing.TabIndex = 6
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(12, 461)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(76, 13)
        Me.Label3.TabIndex = 7
        Me.Label3.Text = "Email Address:"
        '
        'txtEmailAddress
        '
        Me.txtEmailAddress.Location = New System.Drawing.Point(94, 458)
        Me.txtEmailAddress.Name = "txtEmailAddress"
        Me.txtEmailAddress.Size = New System.Drawing.Size(249, 20)
        Me.txtEmailAddress.TabIndex = 9
        '
        'lblHeader
        '
        Me.lblHeader.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblHeader.ForeColor = System.Drawing.Color.Red
        Me.lblHeader.Location = New System.Drawing.Point(15, 9)
        Me.lblHeader.Name = "lblHeader"
        Me.lblHeader.Size = New System.Drawing.Size(599, 23)
        Me.lblHeader.TabIndex = 10
        Me.lblHeader.Text = "Unhandled Application Error Detected!"
        Me.lblHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Timer1
        '
        Me.Timer1.Enabled = True
        Me.Timer1.Interval = 250
        '
        'frmCrash
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(628, 487)
        Me.Controls.Add(Me.lblHeader)
        Me.Controls.Add(Me.txtEmailAddress)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.txtWhatWereYouDoing)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.btnSubmitCrashData)
        Me.Controls.Add(Me.txtStackTrace)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmCrash"
        Me.Text = "Unhandled Application Error Detected!"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents txtStackTrace As System.Windows.Forms.TextBox
    Friend WithEvents btnSubmitCrashData As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtWhatWereYouDoing As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txtEmailAddress As System.Windows.Forms.TextBox
    Friend WithEvents lblHeader As System.Windows.Forms.Label
    Friend WithEvents Timer1 As System.Windows.Forms.Timer
End Class
