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