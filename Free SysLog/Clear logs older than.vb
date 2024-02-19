Public Class Clear_logs_older_than
    Public longLogCount As Long
    Public dateChosenDate As Date

    Private Sub Clear_logs_older_than_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        lblLogCount.Text = $"Number of Log Entries: {longLogCount:N0}"

        DateTimePicker.MinDate = Date.Now.AddDays(-15)
        DateTimePicker.MaxDate = Date.Now
    End Sub

    Private Sub btnClearLogs_Click(sender As Object, e As EventArgs) Handles btnClearLogs.Click
        dateChosenDate = DateTimePicker.Value
        Close()
    End Sub

    Private Sub DateTimePicker_ValueChanged(sender As Object, e As EventArgs) Handles DateTimePicker.ValueChanged
        btnClearLogs.Enabled = True
    End Sub
End Class