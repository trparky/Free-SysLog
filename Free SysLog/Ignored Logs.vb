﻿Public Class Ignored_Logs
    Public IgnoredLogs As List(Of MyListViewItem)
    Private m_SortingColumn1, m_SortingColumn2 As ColumnHeader
    Private boolDoneLoading As Boolean = False

    Private Sub Ignored_Logs_ResizeEnd(sender As Object, e As EventArgs) Handles Me.ResizeEnd
        My.Settings.ignoredWindowSize = Size
    End Sub

    Private Sub Ignored_Logs_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Size = My.Settings.ignoredWindowSize
        Location = VerifyWindowLocation(My.Settings.ignoredWindowLocation)
        Time.Width = My.Settings.columnTimeSize
        Type.Width = My.Settings.columnTypeSize
        IPAddressCol.Width = My.Settings.columnIPSize
        Log.Width = My.Settings.columnLogSize

        logs.Items.AddRange(IgnoredLogs.ToArray())

        boolDoneLoading = True
    End Sub

    Private Sub Logs_ColumnClick(sender As Object, e As ColumnClickEventArgs) Handles logs.ColumnClick
        Dim new_sorting_column As ColumnHeader = logs.Columns(e.Column)

        ' Figure out the new sorting order.
        Dim sort_order As SortOrder

        If m_SortingColumn2 Is Nothing Then
            ' New column. Sort ascending.
            sort_order = SortOrder.Ascending
        Else
            ' See if this is the same column.
            If new_sorting_column.Equals(m_SortingColumn2) Then
                ' Same column. Switch the sort order.
                sort_order = If(m_SortingColumn2.Text.StartsWith("> "), SortOrder.Descending, SortOrder.Ascending)
            Else
                ' New column. Sort ascending.
                sort_order = SortOrder.Ascending
            End If

            ' Remove the old sort indicator.
            m_SortingColumn2.Text = m_SortingColumn2.Text.Substring(2)
        End If

        ' Display the new sort order.
        m_SortingColumn2 = new_sorting_column
        m_SortingColumn2.Text = If(sort_order = SortOrder.Ascending, $"> {m_SortingColumn2.Text}", $"< {m_SortingColumn2.Text}")

        ' Create a comparer.
        logs.ListViewItemSorter = New ListViewComparer(e.Column, sort_order)

        ' Sort.
        logs.Sort()
    End Sub

    Private Sub Ignored_Logs_LocationChanged(sender As Object, e As EventArgs) Handles Me.LocationChanged
        If boolDoneLoading Then My.Settings.ignoredWindowLocation = Location
    End Sub
End Class