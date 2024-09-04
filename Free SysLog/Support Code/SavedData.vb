Public Class SavedData
    Public time, ip, log, fileName, header, logType As String
    Public DateObject As Date
    Public BoolAlerted As Boolean = False

    Public Function MakeDataGridRow(ByRef dataGrid As DataGridView, height As Integer) As MyDataGridViewRow
        Using MyDataGridViewRow As New MyDataGridViewRow
            With MyDataGridViewRow
                .CreateCells(dataGrid)
                .Cells(ColumnIndex_ComputedTime).Value = time
                .Cells(ColumnIndex_ComputedTime).Style.Alignment = DataGridViewContentAlignment.MiddleCenter
                .Cells(ColumnIndex_LogType).Value = If(String.IsNullOrWhiteSpace(logType), "(None)", logType)
                .Cells(ColumnIndex_IPAddress).Value = ip
                .Cells(ColumnIndex_RFC5424).Value = If(String.IsNullOrWhiteSpace(header), "(None)", header)
                .Cells(ColumnIndex_LogText).Value = log
                .Cells(ColumnIndex_Alerted).Value = If(BoolAlerted, "Yes", "No")
                .Cells(ColumnIndex_Alerted).Style.Alignment = DataGridViewContentAlignment.MiddleCenter
                .Cells(ColumnIndex_Alerted).Style.WrapMode = DataGridViewTriState.True
                .DateObject = DateObject
                .BoolAlerted = BoolAlerted
                .MinimumHeight = height
            End With

            Return MyDataGridViewRow
        End Using
    End Function
End Class