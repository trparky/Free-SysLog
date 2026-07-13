Namespace NotificationLimiter
    Public Module NotificationLimiter
        Public lastNotificationTime As New Concurrent.ConcurrentDictionary(Of String, Date)(StringComparer.OrdinalIgnoreCase)

        ' Time after which an unused entry is considered stale (in minutes)
        Private Const CleanupThresholdInMinutes As Integer = 10

        Public Sub ShowNotification(tipText As String, tipIcon As ToolTipIcon, strLogText As String, strLogDate As String, strSourceIP As String, strRawLogText As String, alertType As AlertType, rowGUID As Guid)
            ' Get the current time
            Dim currentTime As Date = Date.Now

            ' Clean up old notification entries
            CleanUpOldEntries(currentTime)

            Dim shouldShow As Boolean = False

            lastNotificationTime.AddOrUpdate(tipText, Function(key As String) ' key not present — always show
                                                          shouldShow = True
                                                          Return currentTime
                                                      End Function,
                                             Function(key As String, existingTime As Date) ' key present — decide atomically
                                                 If (currentTime - existingTime).TotalSeconds >= My.Settings.TimeBetweenSameNotifications Then
                                                     shouldShow = True
                                                     Return currentTime
                                                 Else
                                                     Return existingTime ' leave timestamp untouched, don't show
                                                 End If
                                             End Function)

            If Not shouldShow Then Exit Sub

            SupportCode.ShowToastNotification(tipText, tipIcon, strLogText, strLogDate, strSourceIP, strRawLogText, alertType, rowGUID)
        End Sub

        ' Function to clean up old notification entries
        Private Sub CleanUpOldEntries(currentTime As Date)
            Dim keysToRemove As List(Of String) = lastNotificationTime.Where(Function(kvp As KeyValuePair(Of String, Date)) (currentTime - kvp.Value).TotalMinutes > CleanupThresholdInMinutes).Select(Function(kvp As KeyValuePair(Of String, Date)) kvp.Key).ToList()

            For Each key As String In keysToRemove
                lastNotificationTime.TryRemove(key, Nothing)
            Next
        End Sub
    End Module
End Namespace