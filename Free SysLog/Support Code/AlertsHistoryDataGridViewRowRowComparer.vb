Public Class AlertsHistoryDataGridViewRowComparer
    Implements IComparer

    Private ReadOnly intColumnNumber As Integer
    Private ReadOnly soSortOrder As SortOrder

    Public Sub New(columnNumber As Integer, sortOrder As SortOrder)
        intColumnNumber = columnNumber
        soSortOrder = sortOrder
    End Sub

    Public Function Compare(x As Object, y As Object) As Integer Implements IComparer.Compare
        Dim row1 As AlertsHistoryDataGridViewRow = DirectCast(x, AlertsHistoryDataGridViewRow)
        Dim row2 As AlertsHistoryDataGridViewRow = DirectCast(y, AlertsHistoryDataGridViewRow)

        If intColumnNumber = 0 Then
            Dim date1 As Date = row1.alertDate
            Dim date2 As Date = row2.alertDate

            Return If(soSortOrder = SortOrder.Ascending, Date.Compare(date1, date2), Date.Compare(date2, date1))
        Else
            Dim strFirstString As String = If(row1.Cells.Count <= intColumnNumber, "", row1.Cells(intColumnNumber).Value?.ToString())
            Dim strSecondString As String = If(row2.Cells.Count <= intColumnNumber, "", row2.Cells(intColumnNumber).Value?.ToString())

            Return If(soSortOrder = SortOrder.Descending, String.Compare(strFirstString, strSecondString), String.Compare(strSecondString, strFirstString))
        End If

        Return 0
    End Function
End Class