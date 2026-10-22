Public Class DateRangeCalendar
    Inherits UserControl

    Private ReadOnly _calendar As MonthCalendar
    Private ReadOnly _startLabel As Label
    Private ReadOnly _endLabel As Label

    Private _updating As Boolean = False

    Public Event RangeChanged As EventHandler

    Public Sub New()
        MyBase.New()

        _calendar = New MonthCalendar() With {
            .Location = New Point(0, 0),
            .MaxSelectionCount = 3650,
            .ShowToday = True
        }

        _startLabel = New Label() With {
            .AutoSize = True,
            .Location = New Point(0, _calendar.Bottom + 8)
        }

        _endLabel = New Label() With {
            .AutoSize = True,
            .Location = New Point(0, _calendar.Bottom + 28)
        }

        Controls.Add(_calendar)
        Controls.Add(_startLabel)
        Controls.Add(_endLabel)

        Size = New Size(
            _calendar.Width,
            _calendar.Bottom + 55
        )

        AddHandler _calendar.DateChanged, AddressOf Calendar_DateChanged

        ' Initially select today's date.
        _calendar.SetDate(Date.Today)

        UpdateLabels()
    End Sub

    Public Property StartDate As Date
        Get
            Return _calendar.SelectionStart.Date
        End Get
        Set(value As Date)
            _calendar.SelectionRange = New SelectionRange(value.Date, value.Date)
        End Set
    End Property

    Public Property EndDate As Date
        Get
            Return _calendar.SelectionEnd.Date
        End Get
        Set(value As Date)
            _calendar.SelectionRange = New SelectionRange(value.Date, value.Date)
        End Set
    End Property

    Public Sub SetRange(startDate As Date, endDate As Date)

        startDate = startDate.Date
        endDate = endDate.Date

        If endDate < startDate Then Throw New ArgumentException("End date cannot precede start date.")

        Dim numberOfDays As Double = (endDate - startDate).TotalDays + 1

        If numberOfDays > _calendar.MaxSelectionCount Then Throw New ArgumentOutOfRangeException(NameOf(endDate), "The selected range is too large.")

        _updating = True

        Try
            _calendar.SetSelectionRange(startDate, endDate)
        Finally
            _updating = False
        End Try

        UpdateLabels()

        RaiseEvent RangeChanged(Me, EventArgs.Empty)
    End Sub

    Private Sub Calendar_DateChanged(sender As Object, e As DateRangeEventArgs)
        If _updating Then Return

        UpdateLabels()

        RaiseEvent RangeChanged(Me, EventArgs.Empty)
    End Sub

    Private Sub UpdateLabels()
        _startLabel.Text = "Start: " & StartDate.ToShortDateString()
        _endLabel.Text = "End: " & EndDate.ToShortDateString()
    End Sub


    Public Property MinDate As Date
        Get
            Return _calendar.MinDate.Date
        End Get
        Set(value As Date)
            ApplyDateLimit(value.Date, True)
        End Set
    End Property

    Public Property MaxDate As Date
        Get
            Return _calendar.MaxDate.Date
        End Get
        Set(value As Date)
            ApplyDateLimit(value.Date, False)
        End Set
    End Property

    Private Sub ApplyDateLimit(value As Date, isMinimum As Boolean)

        Dim oldStart As Date = StartDate
        Dim oldEnd As Date = EndDate

        _updating = True

        Try
            If isMinimum Then
                _calendar.MinDate = value
            Else
                _calendar.MaxDate = value
            End If

            ' Keep the selection inside the new boundaries.
            Dim newStart As Date = ClampDate(oldStart)
            Dim newEnd As Date = ClampDate(oldEnd)

            _calendar.SetSelectionRange(newStart, newEnd)

        Finally
            _updating = False
        End Try

        UpdateLabels()

        If oldStart <> StartDate OrElse oldEnd <> EndDate Then
            RaiseEvent RangeChanged(Me, EventArgs.Empty)
        End If
    End Sub

    Private Function ClampDate(value As Date) As Date
        If value < _calendar.MinDate Then
            Return _calendar.MinDate.Date
        End If

        If value > _calendar.MaxDate Then
            Return _calendar.MaxDate.Date
        End If

        Return value.Date
    End Function
End Class