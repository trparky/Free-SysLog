Public Class DataGridViewComparer
    Implements IComparer(Of MyDataGridViewRow)

    Private ReadOnly intColumnNumber As Integer
    Private ReadOnly soSortOrder As SortOrder

    Public Sub New(columnNumber As Integer, sortOrder As SortOrder)
        intColumnNumber = columnNumber
        soSortOrder = sortOrder
    End Sub

    Public Function Compare(row1 As MyDataGridViewRow, row2 As MyDataGridViewRow) As Integer Implements IComparer(Of MyDataGridViewRow).Compare
        Dim strFirstString, strSecondString As String
        Dim date1, date2 As Date

        ' Get the cell values.
        strFirstString = If(row1.Cells.Count <= intColumnNumber, "", row1.Cells(intColumnNumber).Value?.ToString())
        strSecondString = If(row2.Cells.Count <= intColumnNumber, "", row2.Cells(intColumnNumber).Value?.ToString())

        ' Compare them.
        If intColumnNumber = 0 Then
            date1 = row1.DateObject
            date2 = row2.DateObject
            Return If(soSortOrder = SortOrder.Ascending, Date.Compare(date1, date2), Date.Compare(date2, date1))
        Else
            Return If(soSortOrder = SortOrder.Ascending, String.Compare(strFirstString, strSecondString), String.Compare(strSecondString, strFirstString))
        End If

        Return 0
    End Function
End Class