Namespace NotificationLimiter
    Public Class NotificationLimiter
        Private Property NotifyIcon As NotifyIcon

        ' Dictionary to track when the notification was last shown, keyed by message text
        Private Shared lastNotificationTime As New Dictionary(Of String, Date)(StringComparison.OrdinalIgnoreCase)

        ' Time limit between showing the same notification (in seconds)
        Private Const TimeLimitInSeconds As Integer = 30

        ' Time after which an unused entry is considered stale (in minutes)
        Private Const CleanupThresholdInMinutes As Integer = 10

        Public Sub New(_NotifyIcon As NotifyIcon)
            NotifyIcon = _NotifyIcon
        End Sub

        Public Sub ShowNotification(timeout As Integer, tipTitle As String, tipText As String, tipIcon As ToolTipIcon)
            ' Get the current time
            Dim currentTime As Date = Date.Now

            ' Clean up old notification entries
            CleanUpOldEntries(currentTime)

            ' Check if this message has been shown recently
            If lastNotificationTime.ContainsKey(tipText) Then
                Dim lastTime As Date = lastNotificationTime(tipText)
                Dim timeSinceLastNotification As TimeSpan = currentTime - lastTime

                ' If the message was shown within the time limit, do not show it again
                If timeSinceLastNotification.TotalSeconds < TimeLimitInSeconds Then Return
            End If

            ' Update the last shown time for this message
            lastNotificationTime(tipText) = currentTime

            ' Create and display the toast notification (example using Windows 10 Toast Notification)
            NotifyIcon.ShowBalloonTip(timeout, tipTitle, tipText, tipIcon)
        End Sub

        ' Function to clean up old notification entries
        Private Shared Sub CleanUpOldEntries(currentTime As Date)
            Dim keysToRemove As List(Of String) = lastNotificationTime.Where(Function(kvp) (currentTime - kvp.Value).TotalMinutes > CleanupThresholdInMinutes).Select(Function(kvp) kvp.Key).ToList()

            For Each key As String In keysToRemove
                lastNotificationTime.Remove(key)
            Next
        End Sub
    End Class
End Namespace