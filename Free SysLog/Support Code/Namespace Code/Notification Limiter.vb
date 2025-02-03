Imports System.IO
Imports Microsoft.Toolkit.Uwp.Notifications

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
                    If timeSinceLastNotification.TotalSeconds < My.Settings.TimeBetweenSameNotifications Then Return
                End If

                ' Update the last shown time for this message
                lastNotificationTime(tipText) = currentTime
            End SyncLock

            Dim strIconPath As String = Nothing
            Dim notification As New ToastContentBuilder()

            notification.AddText(tipText)
            notification.SetToastDuration(If(My.Settings.NotificationLength = 0, ToastDuration.Short, ToastDuration.Long))

            If tipIcon = ToolTipIcon.Error Then
                strIconPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "error.png")
            ElseIf tipIcon = ToolTipIcon.Warning Then
                strIconPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "warning.png")
            ElseIf tipIcon = ToolTipIcon.Info Then
                strIconPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "info.png")
            End If

            If My.Settings.IncludeButtonsOnNotifications Then
                Dim strNotificationPacket As String = Newtonsoft.Json.JsonConvert.SerializeObject(New NotificationDataPacket With {.alerttext = tipText, .logdate = strLogDate, .logtext = strLogText, .sourceip = strSourceIP, .rawlogtext = strRawLogText})

                notification.AddButton(New ToastButton().SetContent("View Log").AddArgument("action", SupportCode.strViewLog).AddArgument("datapacket", strNotificationPacket))
                notification.AddButton(New ToastButton().SetContent("Open SysLog").AddArgument("action", SupportCode.strOpenSysLog))
            Else
                notification.AddArgument("action", SupportCode.strOpenSysLog)
            End If

            If Not String.IsNullOrWhiteSpace(strIconPath) AndAlso File.Exists(strIconPath) Then notification.AddAppLogoOverride(New Uri(strIconPath), ToastGenericAppLogoCrop.Circle)

            notification.Show()
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