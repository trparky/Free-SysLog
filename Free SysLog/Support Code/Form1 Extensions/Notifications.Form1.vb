Imports Microsoft.Toolkit.Uwp.Notifications
Imports Free_SysLog.SupportCode

Partial Public Class Form1
    Private Sub AddNotificationActionHandler()
        AddHandler ToastNotificationManagerCompat.OnActivated, Sub(args)
                                                                   ' Parse arguments
                                                                   Dim argsDictionary As New Dictionary(Of String, String)
                                                                   Dim itemSplit As String()

                                                                   For Each item As String In args.Argument.Split(";")
                                                                       If Not String.IsNullOrWhiteSpace(item) Then
                                                                           itemSplit = item.Split("=")

                                                                           If itemSplit.Length = 2 AndAlso Not String.IsNullOrWhiteSpace(itemSplit(0)) Then
                                                                               argsDictionary(itemSplit(0)) = itemSplit(1)
                                                                           End If
                                                                       End If
                                                                   Next

                                                                   If argsDictionary.ContainsKey("action") Then
                                                                       If argsDictionary("action").ToString.Equals(strOpenSysLog, StringComparison.OrdinalIgnoreCase) Then
                                                                           Invoke(Sub() RestoreWindow())
                                                                       ElseIf argsDictionary("action").ToString.Equals(strViewLog, StringComparison.OrdinalIgnoreCase) AndAlso argsDictionary.ContainsKey("datapacket") Then
                                                                           Try
                                                                               Dim NotificationDataPacket As NotificationDataPacket = Newtonsoft.Json.JsonConvert.DeserializeObject(Of NotificationDataPacket)(argsDictionary("datapacket").ToString, JSONDecoderSettingsForSettingsFiles)

                                                                               OpenLogViewerWindow(NotificationDataPacket.logtext, NotificationDataPacket.alerttext, NotificationDataPacket.logdate, NotificationDataPacket.sourceip, NotificationDataPacket.rawlogtext, NotificationDataPacket.alertType)
                                                                           Catch ex As Exception
                                                                           End Try
                                                                       End If
                                                                   End If
                                                               End Sub
    End Sub
End Class