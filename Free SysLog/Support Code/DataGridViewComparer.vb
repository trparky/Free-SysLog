Public Class DataGridViewComparer
    Implements IComparer(Of DataGridViewRow)

    Private ReadOnly intColumnNumber As Integer
    Private ReadOnly soSortOrder As SortOrder

    Public Sub New(columnNumber As Integer, sortOrder As SortOrder)
        intColumnNumber = columnNumber
        soSortOrder = sortOrder
    End Sub

    Public Function Compare(row1 As DataGridViewRow, row2 As DataGridViewRow) As Integer Implements IComparer(Of DataGridViewRow).Compare
        Dim strFirstString, strSecondString As String
        Dim date1, date2 As Date

        ' Get the cell values.
        strFirstString = If(row1.Cells.Count <= intColumnNumber, "", row1.Cells(intColumnNumber).Value?.ToString())
        strSecondString = If(row2.Cells.Count <= intColumnNumber, "", row2.Cells(intColumnNumber).Value?.ToString())

        ' Compare them.
        If intColumnNumber = 0 Then
            If TypeOf row1 Is MyDataGridViewRow AndAlso TypeOf row2 Is MyDataGridViewRow Then
                date1 = DirectCast(row1, MyDataGridViewRow).DateObject
                date2 = DirectCast(row2, MyDataGridViewRow).DateObject
                Return If(soSortOrder = SortOrder.Ascending, Date.Compare(date1, date2), Date.Compare(date2, date1))
            End If
        Else
            Return If(soSortOrder = SortOrder.Ascending, String.Compare(strFirstString, strSecondString), String.Compare(strSecondString, strFirstString))
        End If

        Return 0
    End Function
End Class