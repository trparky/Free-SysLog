Public Class MyDataGridViewFileRowComparer
    Implements IComparer(Of MyDataGridViewFileRow)

    Private ReadOnly intColumnNumber As Integer
    Private ReadOnly soSortOrder As SortOrder

    Public Sub New(columnNumber As Integer, sortOrder As SortOrder)
        intColumnNumber = columnNumber
        soSortOrder = sortOrder
    End Sub

    Public Function Compare(row1 As MyDataGridViewFileRow, row2 As MyDataGridViewFileRow) As Integer Implements IComparer(Of MyDataGridViewFileRow).Compare
        ' Compare them.
        If intColumnNumber = 1 Then
            Dim date1 As Date = row1.fileDate
            Dim date2 As Date = row2.fileDate

            Return If(soSortOrder = SortOrder.Ascending, Date.Compare(date1, date2), Date.Compare(date2, date1))
        ElseIf intColumnNumber = 2 Then
            Dim size1 As Long = row1.fileSize
            Dim size2 As Long = row2.fileSize

            Return If(soSortOrder = SortOrder.Ascending, size1.CompareTo(size2), size2.CompareTo(size1))
        ElseIf intColumnNumber = 3 Then
            Dim entry1 As UInteger = row1.entryCount
            Dim entry2 As UInteger = row2.entryCount

            Return If(soSortOrder = SortOrder.Ascending, entry1.CompareTo(entry2), entry2.CompareTo(entry1))
        Else
            Dim strFirstString As String = If(row1.Cells.Count <= intColumnNumber, "", row1.Cells(intColumnNumber).Value?.ToString())
            Dim strSecondString As String = If(row2.Cells.Count <= intColumnNumber, "", row2.Cells(intColumnNumber).Value?.ToString())

            Return If(soSortOrder = SortOrder.Descending, String.Compare(strFirstString, strSecondString), String.Compare(strSecondString, strFirstString))
        End If

        Return 0
    End Function
End Class