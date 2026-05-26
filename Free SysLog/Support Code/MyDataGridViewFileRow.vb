Public Class MyDataGridViewFileRow
    Inherits DataGridViewRow
    Implements ICloneable
    Public Property fileDate As Date
    Public Property fileSize As Long
    Public Property entryCount As Long

    Public Overrides Function Clone()
        Dim newDataGridRow As New MyDataGridViewFileRow()
        newDataGridRow.CreateCells(Me.DataGridView)
        newDataGridRow.fileDate = Me.fileDate
        newDataGridRow.fileSize = Me.fileSize
        newDataGridRow.entryCount = Me.entryCount

        For index As Short = 0 To Me.Cells.Count - 1
            With newDataGridRow
                .Cells(index).Value = Me.Cells(index).Value
                .Cells(index).Style.Alignment = Me.Cells(index).Style.Alignment
                .Cells(index).Style.Font = Me.Cells(index).Style.Font
                .MinimumHeight = Me.MinimumHeight
            End With
        Next

        Return newDataGridRow
    End Function
End Class

Public Module InitializeMyDataGridViewFileRowComparer
    Public Sub InitializeMyDataGridViewFileRowComparer(ByRef FileList As DataGridView, columnIndex As Integer, order As SortOrder)
        FileList.AllowUserToOrderColumns = False

        Try
            FileList.SuspendLayout()

            Dim comparer As New MyDataGridViewFileRowComparer(columnIndex, order)
            FileList.Sort(comparer)

            For Each col As DataGridViewColumn In FileList.Columns
                col.HeaderCell.SortGlyphDirection = SortOrder.None
            Next

            FileList.Columns(columnIndex).HeaderCell.SortGlyphDirection = order
        Finally
            FileList.ResumeLayout()
            FileList.AllowUserToOrderColumns = True
        End Try
    End Sub
End Module

Public Class MyDataGridViewFileRowComparer
    Implements IComparer

    Private ReadOnly intColumnNumber As Integer
    Private ReadOnly soSortOrder As SortOrder

    Public Sub New(columnNumber As Integer, sortOrder As SortOrder)
        intColumnNumber = columnNumber
        soSortOrder = sortOrder
    End Sub

    Public Function Compare(x As Object, y As Object) As Integer Implements IComparer.Compare
        Dim row1 As MyDataGridViewFileRow = DirectCast(x, MyDataGridViewFileRow)
        Dim row2 As MyDataGridViewFileRow = DirectCast(y, MyDataGridViewFileRow)

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