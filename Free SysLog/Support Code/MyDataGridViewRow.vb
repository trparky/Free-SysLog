Public Class MyDataGridViewRow
    Inherits DataGridViewRow
    Implements ICloneable
    Public Property DateObject As Date
    Public Property BoolAlerted As Boolean = False

    Public Overrides Function Clone()
        Dim newDataGridRow As New MyDataGridViewRow()
        newDataGridRow.CreateCells(Me.DataGridView)
        newDataGridRow.DateObject = Me.DateObject
        newDataGridRow.BoolAlerted = Me.BoolAlerted

        For index As Short = 0 To Me.Cells.Count - 1
            With newDataGridRow
                .Cells(index).Value = Me.Cells(index).Value
                .Cells(index).Style.Alignment = Me.Cells(index).Style.Alignment
            End With
        Next

        Return newDataGridRow
    End Function
End Class