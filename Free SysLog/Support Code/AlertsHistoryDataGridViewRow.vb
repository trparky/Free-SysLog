Public Class AlertsHistoryDataGridViewRow
    Inherits DataGridViewRow
    Implements ICloneable
    Public Property strIP As String
    Public Property strLog As String
    Public Property strRawLog As String
    Public Property strTime As String
    Public Property strAlertText As String
    Public Property strFileName As String
    Public Property alertDate As Date
    Public alertType As AlertType = AlertType.None

    Public Overrides Function Clone()
        Dim AlertsHistoryDataGridViewRow As New AlertsHistoryDataGridViewRow()

        With AlertsHistoryDataGridViewRow
            .CreateCells(Me.DataGridView)
            .strRawLog = Me.strRawLog
            .strLog = Me.strLog
            .strIP = Me.strIP
            .strTime = Me.strTime
            .strAlertText = Me.strAlertText
            .strFileName = Me.strFileName

            For index As Short = 0 To Me.Cells.Count - 1
                .Cells(index).Value = Me.Cells(index).Value
                .Cells(index).Style.Alignment = Me.Cells(index).Style.Alignment
                .Cells(index).Style.Font = Me.Cells(index).Style.Font
                .MinimumHeight = Me.MinimumHeight
            Next
        End With

        Return AlertsHistoryDataGridViewRow
    End Function
End Class

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