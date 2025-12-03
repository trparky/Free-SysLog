Imports Free_SysLog.SupportCode

Public Class IgnoredWordsAndPhrases
    Private boolDoneLoading As Boolean = False
    Public boolChanged As Boolean = False
    Private boolColumnOrderChanged As Boolean = False
    Private boolEditMode As Boolean = False
    Public strIgnoredPattern As String = Nothing
    Private draggedItem As ListViewItem
    Private m_SortingColumn As ColumnHeader

    Private Sub IgnoredListView_ItemDrag(sender As Object, e As ItemDragEventArgs) Handles IgnoredListView.ItemDrag
        draggedItem = CType(e.Item, ListViewItem)
        DoDragDrop(e.Item, DragDropEffects.Move)
    End Sub

    Private Sub IgnoredListView_DragEnter(sender As Object, e As DragEventArgs) Handles IgnoredListView.DragEnter
        If e.Data.GetDataPresent(GetType(ListViewItem)) Then
            e.Effect = DragDropEffects.Move
        Else
            e.Effect = DragDropEffects.None
        End If
    End Sub

    Private Sub IgnoredListView_DragOver(sender As Object, e As DragEventArgs) Handles IgnoredListView.DragOver
        e.Effect = DragDropEffects.Move
    End Sub

    Private Sub IgnoredListView_DragDrop(sender As Object, e As DragEventArgs) Handles IgnoredListView.DragDrop
        If draggedItem Is Nothing Then Return

        Dim cp As Point = IgnoredListView.PointToClient(New Point(e.X, e.Y))
        Dim targetItem As ListViewItem = IgnoredListView.GetItemAt(cp.X, cp.Y)

        If targetItem Is Nothing OrElse targetItem Is draggedItem Then Return

        Dim targetIndex As Integer = targetItem.Index
        Dim draggedIndex As Integer = draggedItem.Index

        ' Remove and re-insert the dragged item
        IgnoredListView.Items.RemoveAt(draggedIndex)
        IgnoredListView.Items.Insert(targetIndex, draggedItem)

        ' Re-select the moved item
        draggedItem.Selected = True
        draggedItem.Focused = True

        boolChanged = True ' Mark changes
    End Sub

    Private Function CheckForExistingItem(strIgnored As String) As Boolean
        Return IgnoredListView.Items.Cast(Of MyIgnoredListViewItem).Any(Function(item As MyIgnoredListViewItem)
                                                                            Return item.SubItems(0).Text.Equals(strIgnored, StringComparison.OrdinalIgnoreCase)
                                                                        End Function)
    End Function

    Private Sub BtnAdd_Click(sender As Object, e As EventArgs) Handles BtnAdd.Click
        If Not String.IsNullOrWhiteSpace(TxtIgnored.Text) Then
            If ChkRegex.Checked AndAlso Not IsRegexPatternValid(TxtIgnored.Text) Then
                MsgBox("Invalid regex pattern detected.", MsgBoxStyle.Critical, Text)
                Exit Sub
            End If

            If boolEditMode Then
                Dim selectedItemObject As MyIgnoredListViewItem = DirectCast(IgnoredListView.SelectedItems(0), MyIgnoredListViewItem)

                With selectedItemObject
                    .SubItems(0).Text = TxtIgnored.Text
                    .SubItems(1).Text = If(ChkRegex.Checked, "Yes", "No")
                    .SubItems(2).Text = If(ChkCaseSensitive.Checked, "Yes", "No")
                    .SubItems(3).Text = If(ChkEnabled.Checked, "Yes", "No")
                    .SubItems(5).Text = If(ChkRemoteProcess.Checked, "Remote App", "Main Log Text")
                    .BoolCaseSensitive = ChkCaseSensitive.Checked
                    .BoolEnabled = ChkEnabled.Checked
                    .BoolRegex = ChkRegex.Checked
                    .IgnoreType = If(ChkRemoteProcess.Checked, IgnoreType.RemoteApp, IgnoreType.MainLog)
                    .BackColor = If(.BoolEnabled, Color.LightGreen, Color.Pink)
                    .strComment = txtComment.Text

                    If .dateCreated = Date.MinValue Then ' Just in case it was never set
                        .dateCreated = Date.Now
                        .SubItems(6).Text = Date.Now.ToLongDateString
                    End If
                End With

                IgnoredListView.Enabled = True
                BtnAdd.Text = "Add"
                Label4.Text = "Add Ignored Words and Phrases"
            Else
                Dim IgnoredListViewItem As New MyIgnoredListViewItem(TxtIgnored.Text)

                With IgnoredListViewItem
                    .SubItems.Add(If(ChkRegex.Checked, "Yes", "No"))
                    .SubItems.Add(If(ChkCaseSensitive.Checked, "Yes", "No"))
                    .SubItems.Add(If(ChkEnabled.Checked, "Yes", "No"))
                    .SubItems.Add("0")
                    .SubItems.Add(If(ChkRemoteProcess.Checked, "Remote App", "Main Log Text"))
                    .SubItems.Add(Date.Now.ToLongDateString)
                    .BoolRegex = ChkRegex.Checked
                    .BoolCaseSensitive = ChkCaseSensitive.Checked
                    .BoolEnabled = ChkEnabled.Checked
                    .IgnoreType = If(ChkRemoteProcess.Checked, IgnoreType.RemoteApp, IgnoreType.MainLog)
                    .strComment = txtComment.Text
                    .dateCreated = Date.Now
                    If My.Settings.font IsNot Nothing Then .Font = My.Settings.font
                    .BackColor = If(.BoolEnabled, Color.LightGreen, Color.Pink)
                End With

                IgnoredListView.Items.Add(IgnoredListViewItem)
            End If

            boolEditMode = False
            boolChanged = True
            TxtIgnored.Text = Nothing
            txtComment.Text = Nothing
            ChkCaseSensitive.Checked = False
            ChkRegex.Checked = False
            ChkEnabled.Checked = True
            ChkRemoteProcess.Checked = False
        End If
    End Sub

    Private Sub IgnoredListView_ColumnReordered(sender As Object, e As ColumnReorderedEventArgs) Handles IgnoredListView.ColumnReordered
        boolColumnOrderChanged = True
    End Sub

    Private Sub IgnoredWordsAndPhrases_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        If boolColumnOrderChanged Then
            My.Settings.IgnoredWordsAndPhrasesColumnOrder = SaveColumnOrders(IgnoredListView.Columns)
            My.Settings.Save()
        End If

        If Not boolChanged Then Exit Sub

        Dim newIgnoredList As New ThreadSafetyLists.ThreadSafeIgnoredList
        Dim tempIgnoredRules As New Specialized.StringCollection()

        Try
            Dim ignoredClass As IgnoredClass

            For Each item As MyIgnoredListViewItem In IgnoredListView.Items
                ignoredClass = New IgnoredClass() With {
                    .StrIgnore = item.SubItems(0).Text,
                    .BoolCaseSensitive = item.BoolCaseSensitive,
                    .BoolRegex = item.BoolRegex,
                    .BoolEnabled = item.BoolEnabled,
                    .IgnoreType = item.IgnoreType,
                    .dateCreated = item.dateCreated,
                    .strComment = item.strComment
                }

                If ignoredClass.BoolEnabled Then newIgnoredList.Add(ignoredClass)
                tempIgnoredRules.Add(Newtonsoft.Json.JsonConvert.SerializeObject(ignoredClass))
            Next

            newIgnoredList.Sort(Function(x As IgnoredClass, y As IgnoredClass) x.BoolRegex.CompareTo(y.BoolRegex))

            ' We now save the new list to the main lists in memory now that we know nothing wrong happened above.
            ignoredList.Clear()
            ignoredList.Merge(newIgnoredList.GetSnapshot())

            My.Settings.ignored2 = tempIgnoredRules
            My.Settings.Save()
        Catch ex As Exception
            SyncLock SupportCode.ParentForm.dataGridLockObject
                SupportCode.ParentForm.Logs.Rows.Add(
                    SyslogParser.MakeLocalDataGridRowEntry(
                        $"Unable to save user preferences on ""Ignored Words and Phrases"" window to program settings.{vbCrLf}{vbCrLf}Exception: {ex.Message}{vbCrLf}{ex.StackTrace}",
                        SupportCode.ParentForm.Logs
                    )
                )

                If SupportCode.ParentForm.ChkEnableAutoScroll.Checked Then SupportCode.ParentForm.Logs.FirstDisplayedScrollingRowIndex = SupportCode.ParentForm.Logs.Rows.Count - 1
                SupportCode.ParentForm.NumberOfLogs.Text = $"Number of Log Entries: {SupportCode.ParentForm.Logs.Rows.Count:N0}"
            End SyncLock
        End Try
    End Sub

    Private Sub IgnoredWordsAndPhrases_Load(sender As Object, e As EventArgs) Handles Me.Load
        SetDoubleBufferingFlag(IgnoredListView)

        LoadColumnOrders(IgnoredListView.Columns, My.Settings.IgnoredWordsAndPhrasesColumnOrder)

        BtnCancel.Visible = False
        btnDeleteDuringEditing.Visible = False
        Location = VerifyWindowLocation(My.Settings.ignoredWordsLocation, Me)
        Dim MyIgnoredListViewItem As New List(Of MyIgnoredListViewItem)

        Dim longTotalHits As Long = 0

        If My.Settings.ignored2 IsNot Nothing AndAlso My.Settings.ignored2.Count > 0 Then
            For Each strJSONString As String In My.Settings.ignored2
                MyIgnoredListViewItem.Add(Newtonsoft.Json.JsonConvert.DeserializeObject(Of IgnoredClass)(strJSONString, JSONDecoderSettingsForSettingsFiles).ToListViewItem(longTotalHits))
            Next
        End If

        lblTotalHits.Text = $"Total Ignored Hits: {longTotalHits:N0}"

        IgnoredListView.Items.AddRange(MyIgnoredListViewItem.ToArray())

        Ignored.Width = My.Settings.colIgnoredReplace
        Regex.Width = My.Settings.colIgnoredRegex
        CaseSensitive.Width = My.Settings.colIgnoredCaseSensitive
        ColEnabled.Width = My.Settings.colIgnoredEnabled
        colDateCreated.Width = My.Settings.colIgnoredDateCreated
        colDateOfLastEvent.Width = My.Settings.DateOfLastEventColumnWidth
        colSinceLastEvent.Width = My.Settings.ColSinceLastEventWidth

        Size = My.Settings.ConfigureIgnoredSize

        boolDoneLoading = True

        If Not String.IsNullOrWhiteSpace(strIgnoredPattern) AndAlso CheckForExistingItem(strIgnoredPattern) Then
            For Each item As ListViewItem In IgnoredListView.Items
                If item.SubItems(0).Text.Equals(strIgnoredPattern, StringComparison.OrdinalIgnoreCase) Then
                    item.Selected = True
                    btnDeleteDuringEditing.Visible = True
                    IgnoredListView.Refresh()
                    'IgnoredListView_Click(Nothing, Nothing)
                    EditItem()
                    Exit For
                End If
            Next
        End If
    End Sub

    Private Sub IgnoredListView_KeyUp(sender As Object, e As KeyEventArgs) Handles IgnoredListView.KeyUp
        If e.KeyCode = Keys.Delete And IgnoredListView.SelectedItems().Count > 0 Then BtnDelete.PerformClick()
    End Sub

    Private Sub BtnDelete_Click(sender As Object, e As EventArgs) Handles BtnDelete.Click
        If IgnoredListView.SelectedItems.Count > 0 Then
            If IgnoredListView.SelectedItems.Count = 1 Then
                IgnoredListView.Items.Remove(IgnoredListView.SelectedItems(0))
            Else
                For i As Integer = IgnoredListView.SelectedItems.Count - 1 To 0 Step -1
                    IgnoredListView.SelectedItems(i).Remove()
                Next
            End If

            BtnDelete.Enabled = False
            BtnEdit.Enabled = False
            boolChanged = True
        End If
    End Sub

    Private Sub IgnoredListView_Click(sender As Object, e As EventArgs) Handles IgnoredListView.Click
        If IgnoredListView.SelectedItems.Count > 0 Then
            BtnDelete.Enabled = True
            BtnEdit.Enabled = IgnoredListView.SelectedItems.Count = 1
            BtnEnableDisable.Enabled = True

            BtnEnableDisable.Text = If(DirectCast(IgnoredListView.SelectedItems(0), MyIgnoredListViewItem).BoolEnabled, "Disable", "Enable")

            BtnUp.Enabled = IgnoredListView.SelectedIndices(0) <> 0
            BtnDown.Enabled = IgnoredListView.SelectedIndices(0) <> IgnoredListView.Items.Count - 1
        Else
            BtnDelete.Enabled = False
            BtnEdit.Enabled = False
            BtnEnableDisable.Enabled = False
        End If
    End Sub

    Protected Overrides Sub WndProc(ByRef m As Message)
        Const WM_EXITSIZEMOVE As Integer = &H232

        MyBase.WndProc(m)

        If m.Msg = WM_EXITSIZEMOVE AndAlso boolDoneLoading Then My.Settings.ignoredWordsLocation = Location
    End Sub

    Private Sub EditItem()
        If IgnoredListView.SelectedItems.Count > 0 Then
            BtnCancel.Visible = True
            IgnoredListView.Enabled = False
            boolEditMode = True
            BtnAdd.Text = "Save"
            Label4.Text = "Edit Ignored Words and Phrases"

            Dim selectedItemObject As MyIgnoredListViewItem = DirectCast(IgnoredListView.SelectedItems(0), MyIgnoredListViewItem)

            ChkRemoteProcess.Checked = selectedItemObject.IgnoreType <> IgnoreType.MainLog
            TxtIgnored.Text = selectedItemObject.SubItems(0).Text
            ChkRegex.Checked = selectedItemObject.BoolRegex
            ChkCaseSensitive.Checked = selectedItemObject.BoolCaseSensitive
            ChkEnabled.Checked = selectedItemObject.BoolEnabled
            txtComment.Text = selectedItemObject.strComment
        End If
    End Sub

    Private Sub IgnoredListView_DoubleClick(sender As Object, e As EventArgs) Handles IgnoredListView.DoubleClick
        EditItem()
    End Sub

    Private Sub BtnEdit_Click(sender As Object, e As EventArgs) Handles BtnEdit.Click
        EditItem()
    End Sub

    Private Sub ListViewMenu_Opening(sender As Object, e As ComponentModel.CancelEventArgs) Handles ListViewMenu.Opening
        ' Default visibility settings
        EnableDisableToolStripMenuItem.Visible = False
        ResetHitsToolStripMenuItem.Visible = False

        ' Handle different cases based on the number of selected items
        If IgnoredListView.SelectedItems.Count = 0 Then
            ' If there are no selected items, cancel opening the context menu.
            e.Cancel = True
            Exit Sub
        End If

        ' If exactly one item is selected, show enable/disable and reset options
        If IgnoredListView.SelectedItems.Count = 1 Then
            EnableDisableToolStripMenuItem.Visible = True
            ResetHitsToolStripMenuItem.Visible = True

            ' Update the Enable/Disable text based on the item's BoolEnabled property
            Dim selectedItem As MyIgnoredListViewItem = IgnoredListView.SelectedItems(0)
            EnableDisableToolStripMenuItem.Text = If(selectedItem.BoolEnabled, "Disable", "Enable")

            ' Make Reset Hits option be singular.
            ResetHitsToolStripMenuItem.Text = "Reset Hit"
        Else
            ' If multiple items are selected, only show the reset hits option
            ResetHitsToolStripMenuItem.Visible = True

            ' Make Reset Hits option be plural.
            ResetHitsToolStripMenuItem.Text = "Reset Hits"
        End If
    End Sub

    Private Sub DisableEnableItem()
        If IgnoredListView.SelectedItems.Count = 1 Then
            Dim selectedItem As MyIgnoredListViewItem = IgnoredListView.SelectedItems(0)

            If selectedItem.BoolEnabled Then
                selectedItem.BackColor = Color.Pink
                selectedItem.BoolEnabled = False
                selectedItem.SubItems(3).Text = "No"
                BtnEnableDisable.Text = "Enable"
            Else
                selectedItem.BackColor = Color.LightGreen
                selectedItem.BoolEnabled = True
                selectedItem.SubItems(3).Text = "Yes"
                BtnEnableDisable.Text = "Disable"
            End If
        Else
            For Each item As MyIgnoredListViewItem In IgnoredListView.SelectedItems
                If item.BoolEnabled Then
                    item.BackColor = Color.Pink
                    item.BoolEnabled = False
                    item.SubItems(3).Text = "No"
                Else
                    item.BackColor = Color.LightGreen
                    item.BoolEnabled = True
                    item.SubItems(3).Text = "Yes"
                End If
            Next
        End If

        boolChanged = True
    End Sub

    Private Sub BtnEnableDisable_Click(sender As Object, e As EventArgs) Handles BtnEnableDisable.Click
        DisableEnableItem()
    End Sub

    Private Sub EnableDisableToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles EnableDisableToolStripMenuItem.Click
        DisableEnableItem()
    End Sub

    Private Sub IgnoredListView_ColumnWidthChanged(sender As Object, e As ColumnWidthChangedEventArgs) Handles IgnoredListView.ColumnWidthChanged
        If boolDoneLoading Then
            My.Settings.colIgnoredReplace = Ignored.Width
            My.Settings.colIgnoredRegex = Regex.Width
            My.Settings.colIgnoredCaseSensitive = CaseSensitive.Width
            My.Settings.colIgnoredEnabled = ColEnabled.Width
            My.Settings.colIgnoredDateCreated = colDateCreated.Width
            My.Settings.DateOfLastEventColumnWidth = colDateOfLastEvent.Width
            My.Settings.ColSinceLastEventWidth = colSinceLastEvent.Width
        End If
    End Sub

    Private Sub IgnoredWordsAndPhrases_ResizeEnd(sender As Object, e As EventArgs) Handles Me.ResizeEnd
        If boolDoneLoading Then My.Settings.ConfigureIgnoredSize = Size
    End Sub

    Private Sub BtnExport_Click(sender As Object, e As EventArgs) Handles BtnExport.Click
        If IgnoredListView.Items.Count() = 0 Then
            MsgBox("There's nothing to export.", MsgBoxStyle.Critical, Text)
            Exit Sub
        End If

        Dim saveFileDialog As New SaveFileDialog() With {.Title = "Export Ignored Words and Phrases", .Filter = "JSON File|*.json", .OverwritePrompt = True}
        Dim listOfIgnoredClass As New List(Of IgnoredClass)

        If saveFileDialog.ShowDialog() = DialogResult.OK Then
            For Each item As MyIgnoredListViewItem In IgnoredListView.Items
                listOfIgnoredClass.Add(New IgnoredClass() With {.StrIgnore = item.SubItems(0).Text, .BoolCaseSensitive = item.BoolCaseSensitive, .BoolRegex = item.BoolRegex, .BoolEnabled = item.BoolEnabled, .IgnoreType = item.IgnoreType, .dateCreated = item.dateCreated, .strComment = item.strComment})
            Next

            IO.File.WriteAllText(saveFileDialog.FileName, Newtonsoft.Json.JsonConvert.SerializeObject(listOfIgnoredClass, Newtonsoft.Json.Formatting.Indented))

            If My.Settings.AskOpenExplorer Then
                Using OpenExplorer As New OpenExplorer()
                    OpenExplorer.StartPosition = FormStartPosition.CenterParent
                    OpenExplorer.MyParentForm = Me

                    Dim result As DialogResult = OpenExplorer.ShowDialog(Me)

                    If result = DialogResult.No Then
                        Exit Sub
                    ElseIf result = DialogResult.Yes Then
                        SelectFileInWindowsExplorer(saveFileDialog.FileName)
                    End If
                End Using
            Else
                MsgBox("Data exported successfully.", MsgBoxStyle.Information, SupportCode.ParentForm.Text)
            End If
        End If
    End Sub

    Private Sub BtnImport_Click(sender As Object, e As EventArgs) Handles BtnImport.Click
        Dim openFileDialog As New OpenFileDialog() With {.Title = "Import Ignored Words and Phrases", .Filter = "JSON File|*.json"}
        Dim listOfIgnoredClass As New List(Of IgnoredClass)

        If openFileDialog.ShowDialog() = DialogResult.OK Then
            Try
                listOfIgnoredClass = Newtonsoft.Json.JsonConvert.DeserializeObject(Of List(Of IgnoredClass))(IO.File.ReadAllText(openFileDialog.FileName), JSONDecoderSettingsForLogFiles)

                IgnoredListView.Items.Clear()
                ignoredList.Clear()

                Dim tempIgnored As New Specialized.StringCollection()

                For Each item As IgnoredClass In listOfIgnoredClass
                    If item.BoolEnabled Then ignoredList.Add(item)
                    tempIgnored.Add(Newtonsoft.Json.JsonConvert.SerializeObject(item))
                    IgnoredListView.Items.Add(item.ToListViewItem())
                Next

                My.Settings.ignored2 = tempIgnored
                My.Settings.Save()

                MsgBox("Data imported successfully.", MsgBoxStyle.Information, Text)
                boolChanged = True
            Catch ex As Newtonsoft.Json.JsonSerializationException
                MsgBox("There was an error decoding the JSON data.", MsgBoxStyle.Critical, Text)
            End Try
        End If
    End Sub

    Private Sub IgnoredWordsAndPhrases_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode = Keys.Escape Then
            Close()
        ElseIf e.KeyCode = Keys.F5 Then
            btnUpdateHits.PerformClick()
        End If
    End Sub

    Private Sub btnDeleteAll_Click(sender As Object, e As EventArgs) Handles btnDeleteAll.Click
        If MsgBox("Are you sure you want to delete all of the ignored words and phrases?", MsgBoxStyle.Question + MsgBoxStyle.YesNo, Text) = MsgBoxResult.Yes Then
            IgnoredListView.Items.Clear()
            boolChanged = True
        End If
    End Sub

    Private Sub BtnUp_Click(sender As Object, e As EventArgs) Handles BtnUp.Click
        If IgnoredListView.SelectedItems.Count = 0 Then Return ' No item selected
        Dim selectedIndex As Integer = IgnoredListView.SelectedIndices(0)

        ' Ensure the item is not already at the top
        If selectedIndex > 0 Then
            Dim item As MyIgnoredListViewItem = IgnoredListView.SelectedItems(0)
            IgnoredListView.Items.RemoveAt(selectedIndex)
            IgnoredListView.Items.Insert(selectedIndex - 1, item)
            IgnoredListView.Items(selectedIndex - 1).Selected = True
            boolChanged = True
        End If

        BtnUp.Enabled = IgnoredListView.SelectedIndices(0) <> 0
        BtnDown.Enabled = IgnoredListView.SelectedIndices(0) <> IgnoredListView.Items.Count - 1
    End Sub

    Private Sub BtnDown_Click(sender As Object, e As EventArgs) Handles BtnDown.Click
        If IgnoredListView.SelectedItems.Count = 0 Then Return ' No item selected
        Dim selectedIndex As Integer = IgnoredListView.SelectedIndices(0)

        ' Ensure the item is not already at the bottom
        If selectedIndex < IgnoredListView.Items.Count - 1 Then
            Dim item As MyIgnoredListViewItem = IgnoredListView.SelectedItems(0)
            IgnoredListView.Items.RemoveAt(selectedIndex)
            IgnoredListView.Items.Insert(selectedIndex + 1, item)
            IgnoredListView.Items(selectedIndex + 1).Selected = True
            boolChanged = True
        End If

        BtnUp.Enabled = IgnoredListView.SelectedIndices(0) <> 0
        BtnDown.Enabled = IgnoredListView.SelectedIndices(0) <> IgnoredListView.Items.Count - 1
    End Sub

    Private Sub BtnCancel_Click(sender As Object, e As EventArgs) Handles BtnCancel.Click
        IgnoredListView.Enabled = True
        BtnAdd.Text = "Add"
        Label4.Text = "Add Ignored Words and Phrases"
        boolEditMode = False
        boolChanged = True
        TxtIgnored.Text = Nothing
        txtComment.Text = Nothing
        ChkCaseSensitive.Checked = False
        ChkRegex.Checked = False
        ChkEnabled.Checked = True
        BtnCancel.Visible = False
    End Sub

    Private Sub btnDeleteDuringEditing_Click(sender As Object, e As EventArgs) Handles btnDeleteDuringEditing.Click
        IgnoredListView.SelectedItems(0).Remove()
        IgnoredListView.Enabled = True
        BtnAdd.Text = "Add"
        Label4.Text = "Add Ignored Words and Phrases"
        boolEditMode = False
        boolChanged = True
        TxtIgnored.Text = Nothing
        ChkCaseSensitive.Checked = False
        ChkRegex.Checked = False
        ChkEnabled.Checked = True
    End Sub

    Private Sub IgnoredListView_ColumnClick(sender As Object, e As ColumnClickEventArgs) Handles IgnoredListView.ColumnClick
        SortByClickedColumnForIgnoredList(IgnoredListView, e.Column, m_SortingColumn)
    End Sub

    Private Sub SortByClickedColumnForIgnoredList(ByRef ListView As ListView, intColumn As Integer, ByRef m_SortingColumn As ColumnHeader)
        ' Get the new sorting column.
        Dim new_sorting_column As ColumnHeader = ListView.Columns(intColumn)

        ' Figure out the new sorting order.
        Dim sort_order As SortOrder
        If m_SortingColumn Is Nothing Then
            ' New column. Sort ascending.
            sort_order = SortOrder.Ascending
        Else
            ' See if this is the same column.
            If new_sorting_column.Equals(m_SortingColumn) Then
                ' Same column. Switch the sort order.
                If m_SortingColumn.Text.StartsWith("> ") Then
                    sort_order = SortOrder.Descending
                Else
                    sort_order = SortOrder.Ascending
                End If
            Else
                ' New column. Sort ascending.
                sort_order = SortOrder.Ascending
            End If

            ' Remove the old sort indicator.
            m_SortingColumn.Text = m_SortingColumn.Text.Substring(2)
        End If

        ' Display the new sort order.
        m_SortingColumn = new_sorting_column
        m_SortingColumn.Text = If(sort_order = SortOrder.Ascending, "> " & m_SortingColumn.Text, "< " & m_SortingColumn.Text)

        ' Create a comparer.
        ListView.ListViewItemSorter = New listViewSorter.IgnoredWordsAndPhrasesListViewComparer(intColumn, sort_order)

        ' Sort.
        ListView.Sort()
    End Sub

    Private Sub btnResetHits_Click(sender As Object, e As EventArgs) Handles btnResetHits.Click
        If IgnoredListView.SelectedItems.Count > 0 Then
            For Each item As MyIgnoredListViewItem In IgnoredListView.SelectedItems
                If IgnoredHits.TryRemove(item.SubItems(0).Text, Nothing) Then
                    item.SubItems(4).Text = "0"
                End If
            Next
        Else
            IgnoredHits.Clear()

            For Each item As MyIgnoredListViewItem In IgnoredListView.Items
                item.SubItems(4).Text = "0"
            Next
        End If
    End Sub

    Private Sub ResetHitsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ResetHitsToolStripMenuItem.Click
        If IgnoredListView.SelectedItems.Count > 0 Then
            For Each item As MyIgnoredListViewItem In IgnoredListView.SelectedItems
                If IgnoredHits.TryRemove(item.SubItems(0).Text, Nothing) Then
                    item.SubItems(4).Text = "0"
                End If
            Next
        End If
    End Sub

    Private Sub btnUpdateHits_Click(sender As Object, e As EventArgs) Handles btnUpdateHits.Click
        Dim longTotalHits As Long = 0
        Dim sinceLastEvent As TimeSpan

        IgnoredListView.BeginUpdate()

        For Each item As MyIgnoredListViewItem In IgnoredListView.Items
            Dim intHits As Integer = 0
            Dim dateOfLastEvent As Date = Date.MinValue

            If IgnoredHits.TryGetValue(item.SubItems(0).Text, intHits) Then
                item.SubItems(4).Text = intHits.ToString("N0")
                longTotalHits += intHits
            End If

            If IgnoredLastEvent.TryGetValue(item.SubItems(0).Text, dateOfLastEvent) Then
                dateOfLastEvent = dateOfLastEvent.ToLocalTime
                sinceLastEvent = Now.ToLocalTime - dateOfLastEvent
                item.timeSpanOfLastOccurrence = sinceLastEvent
                item.dateOfLastOccurrence = dateOfLastEvent
                item.SubItems(7).Text = $"{dateOfLastEvent.ToLongDateString} {dateOfLastEvent.ToLongTimeString}"
                item.SubItems(8).Text = TimespanToHMS(sinceLastEvent)
                item.intHits = intHits
            End If
        Next

        lblTotalHits.Text = $"Total Ignored Hits: {longTotalHits:N0}"

        If IgnoredListView.ListViewItemSorter IsNot Nothing Then IgnoredListView.Sort()

        IgnoredListView.EndUpdate()
    End Sub
End Class