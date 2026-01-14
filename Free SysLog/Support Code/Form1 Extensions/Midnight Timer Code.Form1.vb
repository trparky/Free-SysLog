Imports Microsoft.Win32
Imports Free_SysLog.SupportCode

Partial Class Form1
    ' This implementation is based on code found at https://www.codeproject.com/Articles/18201/Midnight-Timer-A-Way-to-Detect-When-it-is-Midnight.
    ' I have rewritten the code to ensure that I fully understand it and to avoid blatantly copying someone else's work.
    ' Using their code as-is without making an effort to learn from it or to create my own implementation doesn't sit well with me.

    Private MyMidnightTimer As Timers.Timer

    Private Sub CreateNewMidnightTimer()
        If MyMidnightTimer IsNot Nothing Then
            MyMidnightTimer.Stop()
            MyMidnightTimer.Dispose()
            MyMidnightTimer = Nothing
        End If

        ' Calculate the time span until midnight
        Dim ts As TimeSpan = GetMidnight(1).Subtract(Date.Now)
        Dim tsMidnight As New TimeSpan(ts.Hours, ts.Minutes, ts.Seconds)

        ' Create and start the new timer
        MyMidnightTimer = New Timers.Timer(tsMidnight.TotalMilliseconds)

        AddHandler MyMidnightTimer.Elapsed, AddressOf MidnightEvent
        AddHandler SystemEvents.TimeChanged, AddressOf WindowsTimeChangeHandler

        MyMidnightTimer.Start()
    End Sub

    Private Sub MidnightEvent(sender As Object, e As Timers.ElapsedEventArgs)
        If Logs.InvokeRequired Then
            Logs.Invoke(New Action(Of Object, Timers.ElapsedEventArgs)(AddressOf MidnightEvent), sender, e)
            Return
        End If

        SyncLock dataGridLockObject
            If My.Settings.BackupOldLogsAfterClearingAtMidnight Then MakeLogBackup()

            Dim oldLogCount As Integer = Logs.Rows.Count
            Logs.Rows.Clear()

            Logs.Rows.Add(SyslogParser.MakeLocalDataGridRowEntry($"The program deleted {oldLogCount:N0} log {If(oldLogCount = 1, "entry", "entries")} at midnight.", Logs))

            UpdateLogCount()
            SelectLatestLogEntry()
            BtnSaveLogsToDisk.Enabled = True

            recentUniqueObjects.Clear()

            NumberOfLogs.Text = $"Number of Log Entries: {Logs.Rows.Count:N0}"
        End SyncLock

        CreateNewMidnightTimer()
    End Sub

    Private Sub WindowsTimeChangeHandler(sender As Object, e As EventArgs)
        CreateNewMidnightTimer()
    End Sub

    Private Function GetMidnight(minutesAfterMidnight As Integer) As Date
        Dim tomorrow As Date = Date.Now.AddDays(1)
        Return New Date(tomorrow.Year, tomorrow.Month, tomorrow.Day, 0, minutesAfterMidnight, 0)
    End Function

    Private Sub BackupOldLogsAfterClearingAtMidnight_Click(sender As Object, e As EventArgs) Handles BackupOldLogsAfterClearingAtMidnight.Click
        My.Settings.DeleteOldLogsAtMidnight = BackupOldLogsAfterClearingAtMidnight.Checked
    End Sub

    Private Sub DeleteOldLogsAtMidnight_Click(sender As Object, e As EventArgs) Handles DeleteOldLogsAtMidnight.Click
        My.Settings.DeleteOldLogsAtMidnight = DeleteOldLogsAtMidnight.Checked
        BackupOldLogsAfterClearingAtMidnight.Enabled = DeleteOldLogsAtMidnight.Checked

        If Not DeleteOldLogsAtMidnight.Checked Then
            DeleteOldLogsAtMidnight.Checked = False
            My.Settings.DeleteOldLogsAtMidnight = False
        End If

        If DeleteOldLogsAtMidnight.Checked Then
            CreateNewMidnightTimer()
        Else
            If MyMidnightTimer IsNot Nothing Then
                MyMidnightTimer.Stop()
                MyMidnightTimer.Dispose()
                MyMidnightTimer = Nothing
            End If
        End If
    End Sub
End Class