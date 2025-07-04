Namespace NotificationLimiter
    Public Module NotificationLimiterModule
        Public lastNotificationTime As New Dictionary(Of String, Date)(StringComparison.OrdinalIgnoreCase)
    End Module

    Public Class NotificationLimiter
        ' Time after which an unused entry is considered stale (in minutes)
        Private Const CleanupThresholdInMinutes As Integer = 10

        Public Sub ShowNotification(tipText As String, tipIcon As ToolTipIcon, strLogText As String, strLogDate As String, strSourceIP As String, strRawLogText As String)
            ' Get the current time
            Dim currentTime As Date = Date.Now

            SyncLock lastNotificationTime
                ' Clean up old notification entries
                CleanUpOldEntries(currentTime)

                ' Check if this message has been shown recently
                If lastNotificationTime.ContainsKey(tipText) Then
                    Dim lastTime As Date = lastNotificationTime(tipText)
                    Dim timeSinceLastNotification As TimeSpan = currentTime - lastTime

                    ' If the message was shown within the time limit, do not show it again
                    If timeSinceLastNotification.TotalSeconds < My.Settings.TimeBetweenSameNotifications Then Exit Sub
                End If

                ' Update the last shown time for this message
                lastNotificationTime(tipText) = currentTime
            End SyncLock

            SupportCode.ShowToastNotification(tipText, tipIcon, strLogText, strLogDate, strSourceIP, strRawLogText)
        End Sub

        ' Function to clean up old notification entries
        Private Sub CleanUpOldEntries(currentTime As Date)
            Dim keysToRemove As List(Of String) = lastNotificationTime.Where(Function(kvp As KeyValuePair(Of String, Date)) (currentTime - kvp.Value).TotalMinutes > CleanupThresholdInMinutes).Select(Function(kvp As KeyValuePair(Of String, Date)) kvp.Key).ToList()

            For Each key As String In keysToRemove
                lastNotificationTime.Remove(key)
            Next
        End Sub
    End Class
End Namespace