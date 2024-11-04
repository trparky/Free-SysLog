Imports Free_SysLog.SupportCode

Public Class SavedData
    Public time, ip, log, fileName, logType, hostname, appName, rawLogData, alertText As String
    Public alertType As AlertType
    Public DateObject, ServerDate As Date
    Public BoolAlerted As Boolean = False

    Public Function MakeDataGridRow(ByRef dataGrid As DataGridView, height As Integer) As MyDataGridViewRow
        Using MyDataGridViewRow As New MyDataGridViewRow
            With MyDataGridViewRow
                .CreateCells(dataGrid)
                .Cells(ColumnIndex_ComputedTime).Value = time
                .Cells(ColumnIndex_ComputedTime).Style.Alignment = DataGridViewContentAlignment.MiddleCenter
                .Cells(ColumnIndex_LogType).Value = If(String.IsNullOrWhiteSpace(logType), "", logType)
                .Cells(ColumnIndex_IPAddress).Value = ip
                .Cells(ColumnIndex_RemoteProcess).Value = If(String.IsNullOrWhiteSpace(appName), "", appName)
                .Cells(ColumnIndex_Hostname).Value = If(String.IsNullOrWhiteSpace(hostname), "", hostname)
                .Cells(ColumnIndex_LogText).Value = log
                .Cells(ColumnIndex_Alerted).Value = If(BoolAlerted, "Yes", "No")
                .Cells(ColumnIndex_Alerted).Style.Alignment = DataGridViewContentAlignment.MiddleCenter
                .Cells(ColumnIndex_Alerted).Style.WrapMode = DataGridViewTriState.True
                .Cells(ColumnIndex_ServerTime).Value = If(ServerDate = Date.MinValue, "", ToIso8601Format(ServerDate))
                .DateObject = DateObject
                .BoolAlerted = BoolAlerted
                .MinimumHeight = height
                .RawLogData = rawLogData
                .AlertText = alertText
                .alertType = alertType

                If My.Settings.font IsNot Nothing Then
                    .Cells(ColumnIndex_ComputedTime).Style.Font = My.Settings.font
                    .Cells(ColumnIndex_LogType).Style.Font = My.Settings.font
                    .Cells(ColumnIndex_IPAddress).Style.Font = My.Settings.font
                    .Cells(ColumnIndex_RemoteProcess).Style.Font = My.Settings.font
                    .Cells(ColumnIndex_Hostname).Style.Font = My.Settings.font
                    .Cells(ColumnIndex_LogText).Style.Font = My.Settings.font
                    .Cells(ColumnIndex_Alerted).Style.Font = My.Settings.font
                    .Cells(ColumnIndex_ServerTime).Style.Font = My.Settings.font
                End If
            End With

            Return MyDataGridViewRow
        End Using
    End Function
End Class

Public Class ReplacementsClass
    Public BoolRegex As Boolean
    Public BoolCaseSensitive As Boolean
    Public StrReplace, StrReplaceWith As String
    Public BoolEnabled As Boolean = True

    Public Function ToListViewItem() As MyReplacementsListViewItem
        Dim listViewItem As New MyReplacementsListViewItem(StrReplace)
        listViewItem.SubItems.Add(StrReplaceWith)
        listViewItem.SubItems.Add(If(BoolRegex, "Yes", "No"))
        listViewItem.SubItems.Add(If(BoolCaseSensitive, "Yes", "No"))
        listViewItem.SubItems.Add(If(BoolEnabled, "Yes", "No"))
        listViewItem.BoolRegex = BoolRegex
        listViewItem.BoolCaseSensitive = BoolCaseSensitive
        listViewItem.BoolEnabled = BoolEnabled
        If My.Settings.font IsNot Nothing Then listViewItem.Font = My.Settings.font
        Return listViewItem
    End Function
End Class

Public Class AlertsHistory
    Public strTime, strAlertText, strIP, strLog, strRawLog As String
    Public alertType As AlertType

    Public Function MakeDataGridRow(ByRef dataGrid As DataGridView, height As Integer) As AlertsHistoryDataGridViewRow
        Using AlertsHistoryDataGridViewRow As New AlertsHistoryDataGridViewRow
            With AlertsHistoryDataGridViewRow
                .CreateCells(dataGrid)
                .Cells(0).Value = strTime
                .Cells(0).Style.Alignment = DataGridViewContentAlignment.MiddleCenter

                If alertType = AlertType.ErrorMsg Then
                    .Cells(1).Value = "Error"
                ElseIf alertType = AlertType.Warning Then
                    .Cells(1).Value = "Warning"
                ElseIf alertType = AlertType.Info Then
                    .Cells(1).Value = "Information"
                Else
                    .Cells(1).Value = ""
                End If

                .Cells(1).Style.Alignment = DataGridViewContentAlignment.MiddleCenter
                .Cells(2).Value = strAlertText

                If My.Settings.font IsNot Nothing Then
                    .Cells(0).Style.Font = My.Settings.font
                    .Cells(1).Style.Font = My.Settings.font
                    .Cells(2).Style.Font = My.Settings.font
                End If

                .strRawLog = strRawLog
                .strIP = strIP
                .strLog = strLog
                .strTime = strTime
                .strAlertText = strAlertText
            End With

            Return AlertsHistoryDataGridViewRow
        End Using
    End Function
End Class

Public Class IgnoredClass
    Public BoolRegex As Boolean
    Public BoolCaseSensitive As Boolean
    Public StrIgnore As String
    Public BoolEnabled As Boolean = True

    Public Function ToListViewItem() As MyIgnoredListViewItem
        Dim listViewItem As New MyIgnoredListViewItem(StrIgnore)
        listViewItem.SubItems.Add(If(BoolRegex, "Yes", "No"))
        listViewItem.SubItems.Add(If(BoolCaseSensitive, "Yes", "No"))
        listViewItem.SubItems.Add(If(BoolEnabled, "Yes", "No"))
        listViewItem.BoolRegex = BoolRegex
        listViewItem.BoolCaseSensitive = BoolCaseSensitive
        listViewItem.BoolEnabled = BoolEnabled
        If My.Settings.font IsNot Nothing Then listViewItem.Font = My.Settings.font
        Return listViewItem
    End Function
End Class

Public Enum AlertType As Byte
    Warning
    Info
    ErrorMsg
    None
End Enum

Public Class AlertsClass
    Public BoolRegex As Boolean
    Public BoolCaseSensitive As Boolean
    Public StrLogText, StrAlertText As String
    Public alertType As AlertType = AlertType.None
    Public BoolEnabled As Boolean = True

    Public Function ToListViewItem() As AlertsListViewItem
        Dim listViewItem As New AlertsListViewItem(StrLogText) With {.StrLogText = StrLogText, .StrAlertText = StrAlertText}
        listViewItem.SubItems.Add(If(String.IsNullOrWhiteSpace(StrAlertText), "(Shows Log Text)", StrAlertText))
        listViewItem.SubItems.Add(If(BoolRegex, "Yes", "No"))
        listViewItem.SubItems.Add(If(BoolCaseSensitive, "Yes", "No"))

        Select Case alertType
            Case AlertType.Warning
                listViewItem.SubItems.Add("Warning")
            Case AlertType.ErrorMsg
                listViewItem.SubItems.Add("Error")
            Case AlertType.Info
                listViewItem.SubItems.Add("Information")
            Case AlertType.None
                listViewItem.SubItems.Add("None")
        End Select

        listViewItem.SubItems.Add(If(BoolEnabled, "Yes", "No"))

        listViewItem.BoolRegex = BoolRegex
        listViewItem.BoolCaseSensitive = BoolCaseSensitive
        listViewItem.AlertType = alertType
        listViewItem.BoolEnabled = BoolEnabled
        If My.Settings.font IsNot Nothing Then listViewItem.Font = My.Settings.font
        Return listViewItem
    End Function
End Class

Public Class ProxiedSysLogData
    Public ip, log As String
End Class

Public Class NotificationDataPacket
    Public logtext, alerttext, logdate, sourceip, rawlogtext As String
End Class

Public Class CustomHostname
    Public ip, deviceName As String

    Public Function ToListViewItem() As ListViewItem
        Dim ListViewItem As New ListViewItem(ip)

        With ListViewItem
            .SubItems.Add(deviceName)
        End With

        If My.Settings.font IsNot Nothing Then ListViewItem.Font = My.Settings.font

        Return ListViewItem
    End Function
End Class

Public Class SysLogProxyServer
    Public ip As String
    Public name As String = Nothing
    Public port As Integer
    Public boolEnabled As Boolean = True

    Public Function ToListViewItem() As ServerListViewItem
        Dim ServerListView As New ServerListViewItem(ip)

        With ServerListView
            .SubItems.Add(port.ToString)
            .SubItems.Add(If(boolEnabled, "Yes", "No"))
            .SubItems.Add(name)
            .StrName = name
            .BoolEnabled = boolEnabled
            If My.Settings.font IsNot Nothing Then ServerListView.Font = My.Settings.font
        End With

        Return ServerListView
    End Function
End Class