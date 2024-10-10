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
            Dim keysToRemove As New List(Of String)

            ' Check each entry in the dictionary
            For Each kvp As KeyValuePair(Of String, Date) In lastNotificationTime
                Dim timeSinceLastUse As TimeSpan = currentTime - kvp.Value

                ' If the entry is older than the cleanup threshold, mark it for removal
                If timeSinceLastUse.TotalMinutes > CleanupThresholdInMinutes Then keysToRemove.Add(kvp.Key)
            Next

            ' Remove the old entries
            For Each key As String In keysToRemove
                lastNotificationTime.Remove(key)
            Next
        End Sub
    End Class
End Namespace