<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Clear_logs_older_than
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
        Me.LblLogCount = New System.Windows.Forms.Label()
        Me.DateTimePicker = New System.Windows.Forms.DateTimePicker()
        Me.LblOlderThan = New System.Windows.Forms.Label()
        Me.BtnClearLogs = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'LblLogCount
        '
        Me.LblLogCount.AutoSize = True
        Me.LblLogCount.Location = New System.Drawing.Point(12, 9)
        Me.LblLogCount.Name = "lblLogCount"
        Me.LblLogCount.Size = New System.Drawing.Size(124, 13)
        Me.LblLogCount.TabIndex = 0
        Me.LblLogCount.Text = "Number of Log Entries: 0"
        '
        'DateTimePicker
        '
        Me.DateTimePicker.Location = New System.Drawing.Point(134, 28)
        Me.DateTimePicker.Name = "DateTimePicker"
        Me.DateTimePicker.Size = New System.Drawing.Size(200, 20)
        Me.DateTimePicker.TabIndex = 1
        '
        'LblOlderThan
        '
        Me.LblOlderThan.AutoSize = True
        Me.LblOlderThan.Location = New System.Drawing.Point(12, 34)
        Me.LblOlderThan.Name = "lblOlderThan"
        Me.LblOlderThan.Size = New System.Drawing.Size(116, 13)
        Me.LblOlderThan.TabIndex = 2
        Me.LblOlderThan.Text = "Clear Logs older than..."
        '
        'BtnClearLogs
        '
        Me.BtnClearLogs.Enabled = False
        Me.BtnClearLogs.Location = New System.Drawing.Point(12, 54)
        Me.BtnClearLogs.Name = "btnClearLogs"
        Me.BtnClearLogs.Size = New System.Drawing.Size(322, 23)
        Me.BtnClearLogs.TabIndex = 3
        Me.BtnClearLogs.Text = "Clear Logs"
        Me.BtnClearLogs.UseVisualStyleBackColor = True
        '
        'Clear_logs_older_than
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(344, 88)
        Me.Controls.Add(Me.BtnClearLogs)
        Me.Controls.Add(Me.LblOlderThan)
        Me.Controls.Add(Me.DateTimePicker)
        Me.Controls.Add(Me.LblLogCount)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "Clear_logs_older_than"
        Me.Text = "Clear logs older than..."
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents LblLogCount As Label
    Friend WithEvents DateTimePicker As DateTimePicker
    Friend WithEvents LblOlderThan As Label
    Friend WithEvents BtnClearLogs As Button
End Class
