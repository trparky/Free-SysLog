Imports Free_SysLog.SupportCode

Public Class Alerts
    Private boolDoneLoading As Boolean = False
    Public boolChanged As Boolean = False
    Private boolEditMode As Boolean = False
    Private draggedItem As ListViewItem
    Private m_SortingColumn As ColumnHeader
    Private boolColumnOrderChanged As Boolean = False

    Private Sub IgnoredListView_ItemDrag(sender As Object, e As ItemDragEventArgs) Handles AlertsListView.ItemDrag
        draggedItem = CType(e.Item, ListViewItem)
        DoDragDrop(e.Item, DragDropEffects.Move)
    End Sub

    Private Sub IgnoredListView_DragEnter(sender As Object, e As DragEventArgs) Handles AlertsListView.DragEnter
        If e.Data.GetDataPresent(GetType(ListViewItem)) Then
            e.Effect = DragDropEffects.Move
        Else
            e.Effect = DragDropEffects.None
        End If
    End Sub

    Private Sub IgnoredListView_DragOver(sender As Object, e As DragEventArgs) Handles AlertsListView.DragOver
        e.Effect = DragDropEffects.Move
    End Sub

    Private Sub IgnoredListView_DragDrop(sender As Object, e As DragEventArgs) Handles AlertsListView.DragDrop
        If draggedItem Is Nothing Then Return

        Dim cp As Point = AlertsListView.PointToClient(New Point(e.X, e.Y))
        Dim targetItem As ListViewItem = AlertsListView.GetItemAt(cp.X, cp.Y)

        If targetItem Is Nothing OrElse targetItem Is draggedItem Then Return

        Dim targetIndex As Integer = targetItem.Index
        Dim draggedIndex As Integer = draggedItem.Index

        ' Remove and re-insert the dragged item
        AlertsListView.Items.RemoveAt(draggedIndex)
        AlertsListView.Items.Insert(targetIndex, draggedItem)

        ' Re-select the moved item
        draggedItem.Selected = True
        draggedItem.Focused = True

        boolChanged = True ' Mark changes
    End Sub

    Private Sub AlertTypeComboBox_SelectedIndexChanged(sender As Object, e As EventArgs) Handles AlertTypeComboBox.SelectedIndexChanged
        If AlertTypeComboBox.SelectedIndex = 0 Then
            IconPictureBox.Image = SystemIcons.Warning.ToBitmap()
        ElseIf AlertTypeComboBox.SelectedIndex = 1 Then
            IconPictureBox.Image = SystemIcons.Error.ToBitmap()
        ElseIf AlertTypeComboBox.SelectedIndex = 2 Then
            IconPictureBox.Image = SystemIcons.Information.ToBitmap()
        ElseIf AlertTypeComboBox.SelectedIndex = 3 Then
            IconPictureBox.Image = Nothing
        End If

        If boolDoneLoading Then boolChanged = True
    End Sub

    Private Function CheckForExistingItem(strIgnored As String) As Boolean
        Return AlertsListView.Items.Cast(Of AlertsListViewItem).Any(Function(item As AlertsListViewItem)
                                                                        Return item.SubItems(0).Text.Equals(strIgnored, StringComparison.OrdinalIgnoreCase)
                                                                    End Function)
    End Function

    Private Sub AlertsListView_ColumnReordered(sender As Object, e As ColumnReorderedEventArgs) Handles AlertsListView.ColumnReordered
        boolColumnOrderChanged = True
    End Sub

    Private Sub Alerts_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadColumnOrders(AlertsListView.Columns, My.Settings.alertsColumnOrder)
        BtnCancel.Visible = False
        Location = VerifyWindowLocation(My.Settings.alertsLocation, Me)
        Dim MyIgnoredListViewItem As New List(Of AlertsListViewItem)

        If My.Settings.alerts IsNot Nothing Then
            For Each strJSONString As String In My.Settings.alerts
                MyIgnoredListViewItem.Add(Newtonsoft.Json.JsonConvert.DeserializeObject(Of AlertsClass)(strJSONString, JSONDecoderSettingsForSettingsFiles).ToListViewItem)
            Next

            AlertsListView.Items.AddRange(MyIgnoredListViewItem.ToArray)
        End If

        AlertLogText.Width = My.Settings.colAlertsAlertLogText
        AlertText.Width = My.Settings.colAlertsAlertText
        Regex.Width = My.Settings.colAlertsRegex
        CaseSensitive.Width = My.Settings.colAlertsCaseSensitive
        AlertTypeColumn.Width = My.Settings.colAlertsType
        ColEnabled.Width = My.Settings.colAlertsEnabled

        Size = My.Settings.ConfigureAlertsSize

        boolDoneLoading = True
    End Sub

    Private Sub AlertsListView_KeyUp(sender As Object, e As KeyEventArgs) Handles AlertsListView.KeyUp
        If e.KeyCode = Keys.Delete And AlertsListView.SelectedItems().Count > 0 Then BtnDelete.PerformClick()
    End Sub

    Private Sub AlertsListView_Click(sender As Object, e As EventArgs) Handles AlertsListView.Click
        If AlertsListView.SelectedItems.Count > 0 Then
            BtnDelete.Enabled = True
            BtnEdit.Enabled = True
            BtnEnableDisable.Enabled = True

            BtnEnableDisable.Text = If(DirectCast(AlertsListView.SelectedItems(0), AlertsListViewItem).BoolEnabled, "Disable", "Enable")

            BtnUp.Enabled = AlertsListView.SelectedIndices(0) <> 0
            BtnDown.Enabled = AlertsListView.SelectedIndices(0) <> AlertsListView.Items.Count - 1
        Else
            BtnDelete.Enabled = False
            BtnEdit.Enabled = False
            BtnEnableDisable.Enabled = False
        End If
    End Sub

    Private Sub BtnDelete_Click(sender As Object, e As EventArgs) Handles BtnDelete.Click
        If AlertsListView.SelectedItems.Count > 0 Then
            If AlertsListView.SelectedItems.Count = 1 Then
                AlertsListView.Items.Remove(AlertsListView.SelectedItems(0))
            Else
                For i As Integer = AlertsListView.SelectedItems.Count - 1 To 0 Step -1
                    AlertsListView.SelectedItems(i).Remove()
                Next
            End If

            BtnDelete.Enabled = False
            BtnEdit.Enabled = False
            boolChanged = True
        End If
    End Sub

    Protected Overrides Sub WndProc(ByRef m As Message)
        Const WM_EXITSIZEMOVE As Integer = &H232

        MyBase.WndProc(m)

        If m.Msg = WM_EXITSIZEMOVE AndAlso boolDoneLoading Then My.Settings.alertsLocation = Location
    End Sub

    Private Sub EditItem()
        If AlertsListView.SelectedItems.Count > 0 Then
            BtnCancel.Visible = True
            AlertsListView.Enabled = False
            boolEditMode = True
            BtnAdd.Text = "Save"
            Label4.Text = "Edit Alert"

            Dim selectedItemObject As AlertsListViewItem = DirectCast(AlertsListView.SelectedItems(0), AlertsListViewItem)

            TxtAlertText.Text = selectedItemObject.StrAlertText
            TxtLogText.Text = selectedItemObject.StrLogText
            ChkEnabled.Checked = selectedItemObject.BoolEnabled
            ChkCaseSensitive.Checked = selectedItemObject.BoolCaseSensitive
            ChkRegex.Checked = selectedItemObject.BoolRegex
            ChkLimited.Checked = selectedItemObject.BoolLimited

            If selectedItemObject.AlertType = AlertType.Warning Then
                IconPictureBox.Image = SystemIcons.Warning.ToBitmap()
                AlertTypeComboBox.SelectedIndex = 0
            ElseIf selectedItemObject.AlertType = AlertType.ErrorMsg Then
                IconPictureBox.Image = SystemIcons.Error.ToBitmap()
                AlertTypeComboBox.SelectedIndex = 1
            ElseIf selectedItemObject.AlertType = AlertType.Info Then
                IconPictureBox.Image = SystemIcons.Information.ToBitmap()
                AlertTypeComboBox.SelectedIndex = 2
            ElseIf selectedItemObject.AlertType = AlertType.None Then
                IconPictureBox.Image = Nothing
                AlertTypeComboBox.SelectedIndex = 3
            End If
        End If
    End Sub

    Private Sub AlertsListView_DoubleClick(sender As Object, e As EventArgs) Handles AlertsListView.DoubleClick
        EditItem()
    End Sub

    Private Sub BtnEdit_Click(sender As Object, e As EventArgs) Handles BtnEdit.Click
        EditItem()
    End Sub

    Private Sub BtnAdd_Click(sender As Object, e As EventArgs) Handles BtnAdd.Click
        If Not String.IsNullOrWhiteSpace(TxtLogText.Text) Then
            If ChkRegex.Checked AndAlso Not IsRegexPatternValid(TxtLogText.Text) Then
                MsgBox("Invalid regex pattern detected.", MsgBoxStyle.Critical, Text)
                Exit Sub
            End If

            If boolEditMode Then
                Dim selectedItemObject As AlertsListViewItem = DirectCast(AlertsListView.SelectedItems(0), AlertsListViewItem)

                With selectedItemObject
                    .StrLogText = TxtLogText.Text
                    .StrAlertText = TxtAlertText.Text
                    .SubItems(0).Text = TxtLogText.Text
                    .SubItems(1).Text = If(String.IsNullOrWhiteSpace(TxtAlertText.Text), "(Shows Log Text)", TxtAlertText.Text)
                    .SubItems(2).Text = If(ChkRegex.Checked, "Yes", "No")
                    .SubItems(3).Text = If(ChkCaseSensitive.Checked, "Yes", "No")

                    Dim AlertType As AlertType

                    If AlertTypeComboBox.SelectedIndex = 0 Then
                        AlertType = AlertType.Warning
                        .SubItems(4).Text = "Warning"
                    ElseIf AlertTypeComboBox.SelectedIndex = 1 Then
                        AlertType = AlertType.ErrorMsg
                        .SubItems(4).Text = "Error"
                    ElseIf AlertTypeComboBox.SelectedIndex = 2 Then
                        AlertType = AlertType.Info
                        .SubItems(4).Text = "Information"
                    ElseIf AlertTypeComboBox.SelectedIndex = 3 Then
                        AlertType = AlertType.None
                        .SubItems(4).Text = "None"
                    End If

                    .SubItems(5).Text = If(ChkLimited.Checked, "Yes", "No")
                    .SubItems(6).Text = If(ChkEnabled.Checked, "Yes", "No")
                    .BoolRegex = ChkRegex.Checked
                    .BoolCaseSensitive = ChkCaseSensitive.Checked
                    .AlertType = AlertType
                    .BoolEnabled = ChkEnabled.Checked
                    .BoolLimited = ChkLimited.Checked
                    .BackColor = If(ChkEnabled.Checked, Color.LightGreen, Color.Pink)
                End With

                AlertsListView.Enabled = True
                BtnAdd.Text = "Add"
                Label4.Text = "Add Alert"
            Else
                Dim AlertsListViewItem As New AlertsListViewItem(TxtLogText.Text) With {.StrLogText = TxtLogText.Text, .StrAlertText = TxtAlertText.Text}

                With AlertsListViewItem
                    .SubItems.Add(If(String.IsNullOrWhiteSpace(TxtAlertText.Text), "(Shows Log Text)", TxtAlertText.Text))
                    .SubItems.Add(If(ChkRegex.Checked, "Yes", "No"))
                    .SubItems.Add(If(ChkCaseSensitive.Checked, "Yes", "No"))

                    Dim AlertType As AlertType

                    If AlertTypeComboBox.SelectedIndex = 0 Then
                        AlertType = AlertType.Warning
                        .SubItems.Add("Warning")
                    ElseIf AlertTypeComboBox.SelectedIndex = 1 Then
                        AlertType = AlertType.ErrorMsg
                        .SubItems.Add("Error")
                    ElseIf AlertTypeComboBox.SelectedIndex = 2 Then
                        AlertType = AlertType.Info
                        .SubItems.Add("Information")
                    ElseIf AlertTypeComboBox.SelectedIndex = 3 Then
                        AlertType = AlertType.None
                        .SubItems.Add("None")
                    End If

                    .SubItems.Add(If(ChkLimited.Checked, "Yes", "No"))
                    .SubItems.Add(If(ChkEnabled.Checked, "Yes", "No"))
                    .BoolRegex = ChkRegex.Checked
                    .BoolCaseSensitive = ChkCaseSensitive.Checked
                    .AlertType = AlertType
                    .BoolEnabled = ChkEnabled.Checked
                    .BoolLimited = ChkLimited.Checked
                    .BackColor = If(ChkEnabled.Checked, Color.LightGreen, Color.Pink)
                    If My.Settings.font IsNot Nothing Then .Font = My.Settings.font
                End With

                AlertsListView.Items.Add(AlertsListViewItem)
            End If

            boolEditMode = False
            boolChanged = True
            TxtAlertText.Text = Nothing
            TxtLogText.Text = Nothing
            IconPictureBox.Image = Nothing
            AlertTypeComboBox.SelectedIndex = -1
            ChkCaseSensitive.Checked = False
            ChkRegex.Checked = False
            ChkEnabled.Checked = True
            ChkLimited.Checked = True
        Else
            MsgBox("You need to fill in the appropriate information to create an alert.", MsgBoxStyle.Critical, Text)
        End If
    End Sub

    Private Sub Alerts_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        If boolColumnOrderChanged Then
            My.Settings.alertsColumnOrder = SaveColumnOrders(AlertsListView.Columns)
            My.Settings.Save()
        End If

        If Not boolChanged Then Exit Sub

        Dim newAlertsList As New ThreadSafetyLists.ThreadSafeAlertsList
        Dim tempAlerts As New Specialized.StringCollection()

        Try
            Dim AlertsClass As AlertsClass

            For Each item As AlertsListViewItem In AlertsListView.Items
                AlertsClass = New AlertsClass() With {
                    .StrLogText = item.StrLogText,
                    .StrAlertText = item.StrAlertText,
                    .BoolCaseSensitive = item.BoolCaseSensitive,
                    .BoolRegex = item.BoolRegex,
                    .alertType = item.AlertType,
                    .BoolEnabled = item.BoolEnabled,
                    .BoolLimited = item.BoolLimited
                }

                If AlertsClass.BoolEnabled Then newAlertsList.Add(AlertsClass)
                tempAlerts.Add(Newtonsoft.Json.JsonConvert.SerializeObject(AlertsClass))
            Next

            newAlertsList.Sort(Function(x As AlertsClass, y As AlertsClass) x.BoolRegex.CompareTo(y.BoolRegex))

            ' We now save the new list to the main lists in memory now that we know nothing wrong happened above.
            alertsList.Clear()
            alertsList.Merge(newAlertsList.GetSnapshot())

            My.Settings.alerts = tempAlerts
            My.Settings.Save()
        Catch ex As Exception
            SyncLock SupportCode.ParentForm.dataGridLockObject
                SupportCode.ParentForm.Logs.Rows.Add(
                    SyslogParser.MakeLocalDataGridRowEntry(
                        $"Unable to save user preferences on ""Alerts"" window to program settings.{vbCrLf}{vbCrLf}Exception: {ex.Message}{vbCrLf}{ex.StackTrace}",
                        SupportCode.ParentForm.Logs
                    )
                )

                If SupportCode.ParentForm.ChkEnableAutoScroll.Checked Then SupportCode.ParentForm.Logs.FirstDisplayedScrollingRowIndex = SupportCode.ParentForm.Logs.Rows.Count - 1
                SupportCode.ParentForm.NumberOfLogs.Text = $"Number of Log Entries: {SupportCode.ParentForm.Logs.Rows.Count:N0}"
            End SyncLock
        End Try
    End Sub

    Private Sub ListViewMenu_Opening(sender As Object, e As ComponentModel.CancelEventArgs) Handles ListViewMenu.Opening
        If AlertsListView.SelectedItems.Count = 0 And AlertsListView.SelectedItems.Count > 1 Then
            e.Cancel = True
            Exit Sub
        Else
            Dim selectedItem As AlertsListViewItem = AlertsListView.SelectedItems(0)
            EnableDisableToolStripMenuItem.Text = If(selectedItem.BoolEnabled, "Disable", "Enable")
        End If
    End Sub

    Private Sub DisableEnableItem()
        Dim selectedItem As AlertsListViewItem = AlertsListView.SelectedItems(0)

        If selectedItem.BoolEnabled Then
            selectedItem.BoolEnabled = False
            selectedItem.SubItems(5).Text = "No"
            BtnEnableDisable.Text = "Enable"
            selectedItem.BackColor = Color.Pink
        Else
            selectedItem.BoolEnabled = True
            selectedItem.SubItems(5).Text = "Yes"
            BtnEnableDisable.Text = "Disable"
            selectedItem.BackColor = Color.LightGreen
        End If

        boolChanged = True
    End Sub

    Private Sub BtnEnableDisable_Click(sender As Object, e As EventArgs) Handles BtnEnableDisable.Click
        DisableEnableItem()
    End Sub

    Private Sub EnableDisableToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles EnableDisableToolStripMenuItem.Click
        DisableEnableItem()
    End Sub

    Private Sub AlertsListView_ColumnWidthChanged(sender As Object, e As ColumnWidthChangedEventArgs) Handles AlertsListView.ColumnWidthChanged
        If boolDoneLoading Then
            My.Settings.colAlertsAlertLogText = AlertLogText.Width
            My.Settings.colAlertsAlertText = AlertText.Width
            My.Settings.colAlertsRegex = Regex.Width
            My.Settings.colAlertsCaseSensitive = CaseSensitive.Width
            My.Settings.colAlertsType = AlertTypeColumn.Width
            My.Settings.colAlertsEnabled = ColEnabled.Width
        End If
    End Sub

    Private Sub Alerts_ResizeEnd(sender As Object, e As EventArgs) Handles Me.ResizeEnd
        If boolDoneLoading Then My.Settings.ConfigureAlertsSize = Size
    End Sub

    Private Sub BtnExport_Click(sender As Object, e As EventArgs) Handles BtnExport.Click
        If AlertsListView.Items.Count() = 0 Then
            MsgBox("There's nothing to export.", MsgBoxStyle.Critical, Text)
            Exit Sub
        End If

        Dim saveFileDialog As New SaveFileDialog() With {.Title = "Export Alerts", .Filter = "JSON File|*.json", .OverwritePrompt = True}
        Dim listOfAlertsClass As New List(Of AlertsClass)

        If saveFileDialog.ShowDialog() = DialogResult.OK Then
            For Each item As AlertsListViewItem In AlertsListView.Items
                listOfAlertsClass.Add(New AlertsClass() With {.StrLogText = item.StrLogText, .StrAlertText = item.StrAlertText, .BoolCaseSensitive = item.BoolCaseSensitive, .BoolRegex = item.BoolRegex, .alertType = item.AlertType, .BoolEnabled = item.BoolEnabled, .BoolLimited = item.BoolLimited})
            Next

            IO.File.WriteAllText(saveFileDialog.FileName, Newtonsoft.Json.JsonConvert.SerializeObject(listOfAlertsClass, Newtonsoft.Json.Formatting.Indented))

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
        Dim openFileDialog As New OpenFileDialog() With {.Title = "Import Alerts", .Filter = "JSON File|*.json"}
        Dim listOfAlertsClass As New List(Of AlertsClass)

        If openFileDialog.ShowDialog() = DialogResult.OK Then
            Try
                listOfAlertsClass = Newtonsoft.Json.JsonConvert.DeserializeObject(Of List(Of AlertsClass))(IO.File.ReadAllText(openFileDialog.FileName), JSONDecoderSettingsForLogFiles)

                AlertsListView.Items.Clear()
                alertsList.Clear()

                Dim tempAlerts As New Specialized.StringCollection()

                For Each item As AlertsClass In listOfAlertsClass
                    alertsList.Add(item)
                    tempAlerts.Add(Newtonsoft.Json.JsonConvert.SerializeObject(item))
                    AlertsListView.Items.Add(item.ToListViewItem())
                Next

                My.Settings.alerts = tempAlerts
                My.Settings.Save()

                MsgBox("Data imported successfully.", MsgBoxStyle.Information, Text)
                boolChanged = True
            Catch ex As Newtonsoft.Json.JsonSerializationException
                MsgBox("There was an error decoding the JSON data.", MsgBoxStyle.Critical, Text)
            End Try
        End If
    End Sub

    Private Sub Alerts_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode = Keys.Escape Then Close()
    End Sub

    Private Sub BtnDeleteAll_Click(sender As Object, e As EventArgs) Handles BtnDeleteAll.Click
        If MsgBox("Are you sure you want to delete all of the configured alerts?", MsgBoxStyle.Question + MsgBoxStyle.YesNo, Text) = MsgBoxResult.Yes Then
            AlertsListView.Items.Clear()
            boolChanged = True
        End If
    End Sub

    Private Sub BtnUp_Click(sender As Object, e As EventArgs) Handles BtnUp.Click
        If AlertsListView.SelectedItems.Count = 0 Then Return ' No item selected
        Dim selectedIndex As Integer = AlertsListView.SelectedIndices(0)

        ' Ensure the item is not already at the top
        If selectedIndex > 0 Then
            Dim item As AlertsListViewItem = AlertsListView.SelectedItems(0)
            AlertsListView.Items.RemoveAt(selectedIndex)
            AlertsListView.Items.Insert(selectedIndex - 1, item)
            AlertsListView.Items(selectedIndex - 1).Selected = True
            boolChanged = True
        End If

        BtnUp.Enabled = AlertsListView.SelectedIndices(0) <> 0
        BtnDown.Enabled = AlertsListView.SelectedIndices(0) <> AlertsListView.Items.Count - 1
    End Sub

    Private Sub BtnDown_Click(sender As Object, e As EventArgs) Handles BtnDown.Click
        If AlertsListView.SelectedItems.Count = 0 Then Return ' No item selected
        Dim selectedIndex As Integer = AlertsListView.SelectedIndices(0)

        ' Ensure the item is not already at the bottom
        If selectedIndex < AlertsListView.Items.Count - 1 Then
            Dim item As AlertsListViewItem = AlertsListView.SelectedItems(0)
            AlertsListView.Items.RemoveAt(selectedIndex)
            AlertsListView.Items.Insert(selectedIndex + 1, item)
            AlertsListView.Items(selectedIndex + 1).Selected = True
            boolChanged = True
        End If

        BtnUp.Enabled = AlertsListView.SelectedIndices(0) <> 0
        BtnDown.Enabled = AlertsListView.SelectedIndices(0) <> AlertsListView.Items.Count - 1
    End Sub

    Private Sub BtnCancel_Click(sender As Object, e As EventArgs) Handles BtnCancel.Click
        AlertsListView.Enabled = True
        BtnAdd.Text = "Add"
        Label4.Text = "Add Alert"
        boolEditMode = False
        boolChanged = True
        TxtAlertText.Text = Nothing
        TxtLogText.Text = Nothing
        IconPictureBox.Image = Nothing
        AlertTypeComboBox.SelectedIndex = -1
        ChkCaseSensitive.Checked = False
        ChkRegex.Checked = False
        ChkEnabled.Checked = True
        BtnCancel.Visible = False
    End Sub

    Private Sub IgnoredListView_ColumnClick(sender As Object, e As ColumnClickEventArgs) Handles AlertsListView.ColumnClick
        SortByClickedColumn(AlertsListView, e.Column, m_SortingColumn)
    End Sub
End Class