Imports System.IO
Imports System.Reflection
Imports Microsoft.Toolkit.Uwp.Notifications

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

            Dim strIconPath As String = Nothing
            Dim notification As New ToastContentBuilder()
            notification.AddText(tipText)

            If tipIcon = ToolTipIcon.Error Then
                strIconPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "error.png")
            ElseIf tipIcon = ToolTipIcon.Warning Then
                strIconPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "warning.png")
            ElseIf tipIcon = ToolTipIcon.Info Then
                strIconPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "info.png")
            End If

            If Not String.IsNullOrWhiteSpace(strIconPath) AndAlso File.Exists(strIconPath) Then notification.AddAppLogoOverride(New Uri(strIconPath), ToastGenericAppLogoCrop.Circle)

            notification.Show()
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