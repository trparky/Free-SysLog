Public Class SavedData
    Public time, ip, log, fileName, header, logType, hostname, appName, rawLogData As String
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
            End With

            Return MyDataGridViewRow
        End Using
    End Function
End Class