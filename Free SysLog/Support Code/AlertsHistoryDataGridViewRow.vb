Public Class AlertsHistoryDataGridViewRow
    Inherits DataGridViewRow
    Implements ICloneable
    Public Property strIP As String
    Public Property strLog As String
    Public Property strRawLog As String
    Public Property strTime As String
    Public Property strAlertText As String

    Public Overrides Function Clone()
        Dim AlertsHistoryDataGridViewRow As New AlertsHistoryDataGridViewRow()

        With AlertsHistoryDataGridViewRow
            .CreateCells(Me.DataGridView)
            .strRawLog = Me.strRawLog
            .strLog = Me.strLog
            .strIP = Me.strIP
            .strTime = Me.strTime
            .strAlertText = Me.strAlertText

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