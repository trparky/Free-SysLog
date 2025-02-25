Public Class ClearLogsOlderThan
    Public dateChosenDate As Date

    Private Sub Clear_logs_older_than_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        DateTimePicker.MinDate = Date.Now.AddDays(-15)
        DateTimePicker.MaxDate = Date.Now
    End Sub

    Private Sub BtnClearLogs_Click(sender As Object, e As EventArgs) Handles BtnClearLogs.Click
        dateChosenDate = DateTimePicker.Value
        Close()
    End Sub

    Private Sub DateTimePicker_ValueChanged(sender As Object, e As EventArgs) Handles DateTimePicker.ValueChanged
        BtnClearLogs.Enabled = True
        DialogResult = DialogResult.OK
    End Sub

    Private Sub Clear_logs_older_than_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode = Keys.Escape Then
            DialogResult = DialogResult.Cancel
            Close()
        End If
    End Sub
End Class