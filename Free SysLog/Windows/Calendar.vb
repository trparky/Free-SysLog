Public Class frmCalendar
    Public Property results As MsgBoxResult = MsgBoxResult.Cancel
    Public endDate, startDate As Date

    Public Sub setMinAndMaxDateRange(minDate As Date, maxDate As Date)
        DateRangeCalendar.MinDate = minDate
        DateRangeCalendar.MaxDate = maxDate
    End Sub

    Public Sub setRange(minDate As Date, maxDate As Date)
        DateRangeCalendar.SetRange(minDate, maxDate)
    End Sub

    Private Sub btnOK_Click(sender As Object, e As EventArgs) Handles btnOK.Click
        startDate = DateRangeCalendar.StartDate
        endDate = DateRangeCalendar.EndDate
        results = MsgBoxResult.Ok
        Close()
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Close()
    End Sub
End Class