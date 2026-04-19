Public Class frmCalendar
    Public Property startDate As Date
        Get
            Return startDateTimePicker.Value
        End Get
        Set(value As Date)
            If Not value.Equals(Date.MinValue) Then startDateTimePicker.Value = value
        End Set
    End Property

    Public Property endDate As Date
        Get
            Return endDateTimePicker.Value
        End Get
        Set(value As Date)
            If Not value.Equals(Date.MaxValue) Then endDateTimePicker.Value = value
        End Set
    End Property

    Public WriteOnly Property minDate As Date
        Set(value As Date)
            startDateTimePicker.MinDate = value
        End Set
    End Property

    Private Sub btnOK_Click(sender As Object, e As EventArgs) Handles btnOK.Click
        startDate = startDateTimePicker.Value
        endDate = endDateTimePicker.Value
        Close()
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Close()
    End Sub

    Private Sub frmCalendar_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        endDateTimePicker.MaxDate = Date.Now.AddDays(-1)
    End Sub
End Class