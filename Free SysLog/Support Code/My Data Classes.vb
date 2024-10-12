Imports Free_SysLog.SupportCode

Public Class SavedData
    Public time, ip, log, fileName, logType, hostname, appName, rawLogData, alertText As String
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
        Return listViewItem
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
        Return listViewItem
    End Function
End Class

Public Class ProxiedSysLogData
    Public ip, log As String
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
        End With

        Return ServerListView
    End Function
End Class