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
        Me.lblLogCount = New System.Windows.Forms.Label()
        Me.DateTimePicker = New System.Windows.Forms.DateTimePicker()
        Me.lblOlderThan = New System.Windows.Forms.Label()
        Me.btnClearLogs = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'lblLogCount
        '
        Me.lblLogCount.AutoSize = True
        Me.lblLogCount.Location = New System.Drawing.Point(12, 9)
        Me.lblLogCount.Name = "lblLogCount"
        Me.lblLogCount.Size = New System.Drawing.Size(124, 13)
        Me.lblLogCount.TabIndex = 0
        Me.lblLogCount.Text = "Number of Log Entries: 0"
        '
        'DateTimePicker
        '
        Me.DateTimePicker.Location = New System.Drawing.Point(134, 28)
        Me.DateTimePicker.Name = "DateTimePicker"
        Me.DateTimePicker.Size = New System.Drawing.Size(200, 20)
        Me.DateTimePicker.TabIndex = 1
        '
        'lblOlderThan
        '
        Me.lblOlderThan.AutoSize = True
        Me.lblOlderThan.Location = New System.Drawing.Point(12, 34)
        Me.lblOlderThan.Name = "lblOlderThan"
        Me.lblOlderThan.Size = New System.Drawing.Size(116, 13)
        Me.lblOlderThan.TabIndex = 2
        Me.lblOlderThan.Text = "Clear Logs older than..."
        '
        'btnClearLogs
        '
        Me.btnClearLogs.Enabled = False
        Me.btnClearLogs.Location = New System.Drawing.Point(12, 54)
        Me.btnClearLogs.Name = "btnClearLogs"
        Me.btnClearLogs.Size = New System.Drawing.Size(322, 23)
        Me.btnClearLogs.TabIndex = 3
        Me.btnClearLogs.Text = "Clear Logs"
        Me.btnClearLogs.UseVisualStyleBackColor = True
        '
        'Clear_logs_older_than
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(344, 88)
        Me.Controls.Add(Me.btnClearLogs)
        Me.Controls.Add(Me.lblOlderThan)
        Me.Controls.Add(Me.DateTimePicker)
        Me.Controls.Add(Me.lblLogCount)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "Clear_logs_older_than"
        Me.Text = "Clear logs older than..."
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents lblLogCount As Label
    Friend WithEvents DateTimePicker As DateTimePicker
    Friend WithEvents lblOlderThan As Label
    Friend WithEvents btnClearLogs As Button
End Class
