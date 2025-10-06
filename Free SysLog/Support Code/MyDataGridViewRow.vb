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
            End With
        Next

        newDataGridRow.DefaultCellStyle.Padding = New Padding(0, 2, 0, 2)
        Return newDataGridRow
    End Function
End Class