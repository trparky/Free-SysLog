Imports System.ComponentModel

Public Class Ignored_Logs_and_Search_Results
    Public LogsToBeDisplayed As List(Of MyDataGridViewRow)
    Private m_SortingColumn1, m_SortingColumn2 As ColumnHeader
    Private boolDoneLoading As Boolean = False

    Private intColumnNumber As Integer ' Define intColumnNumber at class level
    Private sortOrder As SortOrder = SortOrder.Descending ' Define soSortOrder at class level
    Private ReadOnly dataGridLockObject As New Object

    Private Sub OpenLogViewerWindow()
        If logs.Rows.Count > 0 Then
            Dim selectedRow As MyDataGridViewRow = logs.Rows(logs.SelectedCells(0).RowIndex)

            Using LogViewer As New Log_Viewer With {.strLogText = selectedRow.Cells(3).Value, .StartPosition = FormStartPosition.CenterParent, .Icon = Icon}
                LogViewer.lblLogDate.Text = $"Log Date: {selectedRow.Cells(0).Value}"
                LogViewer.lblSource.Text = $"Source IP Address: {selectedRow.Cells(2).Value}"
                LogViewer.ShowDialog(Me)
            End Using
        End If
    End Sub

    Private Sub Logs_ColumnHeaderMouseClick(sender As Object, e As DataGridViewCellMouseEventArgs) Handles logs.ColumnHeaderMouseClick
        ' Disable user sorting
        logs.AllowUserToOrderColumns = False

        Dim column As DataGridViewColumn = logs.Columns(e.ColumnIndex)

        If e.ColumnIndex = 0 Then
            If sortOrder = SortOrder.Descending Then
                sortOrder = SortOrder.Ascending
            ElseIf sortOrder = SortOrder.Ascending Then
                sortOrder = SortOrder.Descending
            Else
                sortOrder = SortOrder.Ascending
            End If

            colIPAddress.HeaderCell.SortGlyphDirection = SortOrder.None
            colLog.HeaderCell.SortGlyphDirection = SortOrder.None
            colType.HeaderCell.SortGlyphDirection = SortOrder.None

            SortLogsByDateObject(column.Index, sortOrder)
        Else
            sortOrder = SortOrder.None
        End If
    End Sub

    Private Sub SortLogsByDateObject(columnIndex As Integer, order As SortOrder)
        SyncLock dataGridLockObject
            logs.AllowUserToOrderColumns = False
            logs.Enabled = False

            Dim comparer As New DataGridViewComparer(columnIndex, order)
            Dim rows As MyDataGridViewRow() = logs.Rows.Cast(Of DataGridViewRow)().OfType(Of MyDataGridViewRow)().ToArray()

            Array.Sort(rows, Function(row1, row2) comparer.Compare(row1, row2))

            logs.Rows.Clear()
            logs.Rows.AddRange(rows)

            logs.Enabled = True
            logs.AllowUserToOrderColumns = True
        End SyncLock
    End Sub

    Private Sub Logs_DoubleClick(sender As Object, e As EventArgs)
        OpenLogViewerWindow()
    End Sub

    Private Sub Ignored_Logs_and_Search_Results_ResizeEnd(sender As Object, e As EventArgs) Handles Me.ResizeEnd
        My.Settings.ignoredWindowSize = Size
    End Sub

    Private Sub Ignored_Logs_and_Search_Results_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Size = My.Settings.ignoredWindowSize
        Location = VerifyWindowLocation(My.Settings.ignoredWindowLocation, Me)
        colTime.Width = My.Settings.columnTimeSize
        colType.Width = My.Settings.columnTypeSize
        colIPAddress.Width = My.Settings.columnIPSize
        colLog.Width = My.Settings.columnLogSize

        Dim rowStyle As New DataGridViewCellStyle() With {.BackColor = My.Settings.searchColor}
        logs.AlternatingRowsDefaultCellStyle = rowStyle

        logs.Rows.AddRange(LogsToBeDisplayed.ToArray())

        boolDoneLoading = True
    End Sub

    Private Sub Ignored_Logs_and_Search_Results_LocationChanged(sender As Object, e As EventArgs) Handles Me.LocationChanged
        If boolDoneLoading Then My.Settings.ignoredWindowLocation = Location
    End Sub

    Private Sub Ignored_Logs_and_Search_Results_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        ignoredLogsWindow = Nothing
    End Sub
End Class