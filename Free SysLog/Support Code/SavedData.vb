Public Class SavedData
    Public time, ip, log, fileName, header, logType As String
    Public DateObject As Date
    Public BoolAlerted As Boolean = False

    Public Function MakeDataGridRow(ByRef dataGrid As DataGridView, height As Integer) As MyDataGridViewRow
        Using MyDataGridViewRow As New MyDataGridViewRow
            With MyDataGridViewRow
                .CreateCells(dataGrid)
                .Cells(0).Value = time
                .Cells(0).Style.Alignment = DataGridViewContentAlignment.MiddleCenter
                .Cells(1).Value = If(String.IsNullOrWhiteSpace(logType), "(None)", logType)
                .Cells(2).Value = ip
                .Cells(3).Value = If(String.IsNullOrWhiteSpace(header), "(None)", header)
                .Cells(4).Value = log
                .Cells(5).Value = If(BoolAlerted, "Yes", "No")
                .Cells(5).Style.Alignment = DataGridViewContentAlignment.MiddleCenter
                .Cells(5).Style.WrapMode = DataGridViewTriState.True
                .DateObject = DateObject
                .BoolAlerted = BoolAlerted
                .MinimumHeight = height
            End With

            Return MyDataGridViewRow
        End Using
    End Function
End Class