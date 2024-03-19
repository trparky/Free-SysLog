Public Class Clear_logs_older_than
    Public dateChosenDate As Date
    Public boolSuccess As Boolean = False

    Private Sub Clear_logs_older_than_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        DateTimePicker.MinDate = Date.Now.AddDays(-15)
        DateTimePicker.MaxDate = Date.Now
    End Sub

    Private Sub btnClearLogs_Click(sender As Object, e As EventArgs) Handles btnClearLogs.Click
        dateChosenDate = DateTimePicker.Value
        Close()
    End Sub

    Private Sub DateTimePicker_ValueChanged(sender As Object, e As EventArgs) Handles DateTimePicker.ValueChanged
        btnClearLogs.Enabled = True
        boolSuccess = True
    End Sub

    Private Sub Clear_logs_older_than_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode = Keys.Escape Then Close()
    End Sub
End Class