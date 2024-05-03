Public Class SavedData
    Public time, ip, log As String
    Public DateObject As Date
    Public BoolAlerted As Boolean = False

    Public Function MakeDataGridRow(ByRef dataGrid As DataGridView) As MyDataGridViewRow
        Dim MyDataGridViewRow As New MyDataGridViewRow

        With MyDataGridViewRow
            .CreateCells(dataGrid)
            .Cells(0).Value = time
            .Cells(0).Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            .Cells(1).Value = ip
            .Cells(1).Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            .Cells(2).Value = log
            .Cells(3).Value = If(BoolAlerted, "Yes", "No")
            .DateObject = DateObject
            .BoolAlerted = BoolAlerted
        End With

        Return MyDataGridViewRow
    End Function
End Class