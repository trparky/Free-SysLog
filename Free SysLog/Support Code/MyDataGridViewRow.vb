Public Class MyDataGridViewRow
    Inherits DataGridViewRow
    Implements ICloneable
    Public Property DateObject As Date
    Public Property ServerDate As Date
    Public Property BoolAlerted As Boolean = False
    Public Property RawLogData As String
    Public Property AlertText As String
    Public Property alertType As AlertType
    Public Property IgnoredPattern As String
    Public Property GUID As Guid

    Public Sub UncheckRow()
        Me.Cells("colDelete").Value = False
    End Sub

    Public Sub CheckRow()
        Me.Cells("colDelete").Value = True
    End Sub

    Public Sub InvertRowCheckboxStatus()
        Me.Cells("colDelete").Value = Not Me.Cells("colDelete").Value
    End Sub

    Public Overrides Function Clone()
        Dim newDataGridRow As New MyDataGridViewRow()
        newDataGridRow.CreateCells(Me.DataGridView)
        newDataGridRow.DateObject = Me.DateObject
        newDataGridRow.BoolAlerted = Me.BoolAlerted
        newDataGridRow.AlertText = Me.AlertText
        newDataGridRow.RawLogData = Me.RawLogData
        newDataGridRow.alertType = Me.alertType
        newDataGridRow.IgnoredPattern = Me.IgnoredPattern

        For index As Short = 0 To Me.Cells.Count - 1
            With newDataGridRow
                .Cells(index).Value = Me.Cells(index).Value
                .Cells(index).Style.Alignment = Me.Cells(index).Style.Alignment
                .Cells(index).Style.Font = Me.Cells(index).Style.Font
                .MinimumHeight = Me.MinimumHeight
                .DefaultCellStyle = Me.DefaultCellStyle
            End With
        Next

        newDataGridRow.DefaultCellStyle.Padding = New Padding(0, 2, 0, 2)
        Return newDataGridRow
    End Function
End Class

Public Module InitializeMyDataGridViewRowComparer
    Public Sub InitializeMyDataGridViewRowComparer(ByRef LogList As DataGridView, columnIndex As Integer, order As SortOrder)
        LogList.AllowUserToOrderColumns = False

        Try
            LogList.SuspendLayout()

            Dim comparer As New DataGridViewComparer(columnIndex, order)
            LogList.Sort(comparer)

            For Each col As DataGridViewColumn In LogList.Columns
                col.HeaderCell.SortGlyphDirection = SortOrder.None
            Next

            LogList.Columns(columnIndex).HeaderCell.SortGlyphDirection = order
        Finally
            LogList.ResumeLayout()
            LogList.AllowUserToOrderColumns = True
        End Try
    End Sub
End Module

Public Class DataGridViewComparer
    Implements IComparer

    Private ReadOnly intColumnNumber As Integer
    Private ReadOnly soSortOrder As SortOrder

    Public Sub New(columnNumber As Integer, sortOrder As SortOrder)
        intColumnNumber = columnNumber
        soSortOrder = sortOrder
    End Sub

    Public Function Compare(x As Object, y As Object) As Integer Implements IComparer.Compare
        Dim row1 As MyDataGridViewRow = DirectCast(x, MyDataGridViewRow)
        Dim row2 As MyDataGridViewRow = DirectCast(y, MyDataGridViewRow)

        ' Compare them.
        If intColumnNumber = 0 Then
            Dim date1 As Date = row1.DateObject
            Dim date2 As Date = row2.DateObject

            Return If(soSortOrder = SortOrder.Ascending, Date.Compare(date1, date2), Date.Compare(date2, date1))
        ElseIf intColumnNumber = 1 Then
            Dim date1 As Date = row1.ServerDate
            Dim date2 As Date = row2.ServerDate

            Return If(soSortOrder = SortOrder.Ascending, Date.Compare(date1, date2), Date.Compare(date2, date1))
        Else
            Dim strFirstString As String = If(row1.Cells.Count <= intColumnNumber, "", row1.Cells(intColumnNumber).Value?.ToString())
            Dim strSecondString As String = If(row2.Cells.Count <= intColumnNumber, "", row2.Cells(intColumnNumber).Value?.ToString())

            Return If(soSortOrder = SortOrder.Descending, String.Compare(strFirstString, strSecondString), String.Compare(strSecondString, strFirstString))
        End If

        Return 0
    End Function
End Class