﻿Imports Free_SysLog.SupportCode

Public Class Replacements
    Private boolDoneLoading As Boolean = False
    Public boolChanged As Boolean = False
    Private boolEditMode As Boolean = False

    Private Function CheckForExistingItem(strReplace As String, strReplaceWith As String) As Boolean
        Return ReplacementsListView.Items.Cast(Of MyReplacementsListViewItem).Any(Function(item As MyReplacementsListViewItem)
                                                                                      Return item.SubItems(0).Text.Equals(strReplace, StringComparison.OrdinalIgnoreCase) And item.SubItems(1).Text.Equals(strReplaceWith, StringComparison.OrdinalIgnoreCase)
                                                                                  End Function)
    End Function

    Private Sub BtnAdd_Click(sender As Object, e As EventArgs) Handles BtnAdd.Click
        If Not String.IsNullOrWhiteSpace(TxtReplace.Text) Then
            If ChkRegex.Checked AndAlso Not IsRegexPatternValid(TxtReplace.Text) Then
                MsgBox("Invalid regex pattern detected.", MsgBoxStyle.Critical, Text)
                Exit Sub
            End If

            If boolEditMode Then
                Dim selectedItemObject As MyReplacementsListViewItem = DirectCast(ReplacementsListView.SelectedItems(0), MyReplacementsListViewItem)

                With selectedItemObject
                    .SubItems(0).Text = TxtReplace.Text
                    .SubItems(1).Text = TxtReplaceWith.Text
                    .SubItems(2).Text = If(ChkRegex.Checked, "Yes", "No")
                    .SubItems(3).Text = If(ChkCaseSensitive.Checked, "Yes", "No")
                    .SubItems(4).Text = If(ChkEnabled.Checked, "Yes", "No")
                    .BoolRegex = ChkRegex.Checked
                    .BoolCaseSensitive = ChkCaseSensitive.Checked
                    .BoolEnabled = ChkEnabled.Checked
                End With

                ReplacementsListView.Enabled = True
                BtnAdd.Text = "Add"
                Label3.Text = "Add Replacement"
            Else
                If CheckForExistingItem(TxtReplace.Text, TxtReplaceWith.Text) Then
                    MsgBox("A similar item has already been found in your replacements list.", MsgBoxStyle.Critical, Text)
                    Exit Sub
                End If

                Dim MyReplacementsListViewItem As New MyReplacementsListViewItem(TxtReplace.Text)

                With MyReplacementsListViewItem
                    .SubItems.Add(TxtReplaceWith.Text)
                    .SubItems.Add(If(ChkRegex.Checked, "Yes", "No"))
                    .SubItems.Add(If(ChkCaseSensitive.Checked, "Yes", "No"))
                    .SubItems.Add(If(ChkEnabled.Checked, "Yes", "No"))
                    .BoolRegex = ChkRegex.Checked
                    .BoolCaseSensitive = ChkCaseSensitive.Checked
                    .BoolEnabled = ChkEnabled.Checked
                    If My.Settings.font IsNot Nothing Then .Font = My.Settings.font
                End With

                ReplacementsListView.Items.Add(MyReplacementsListViewItem)
                boolChanged = True
            End If

            boolEditMode = False
            boolChanged = True
            TxtReplace.Text = Nothing
            TxtReplaceWith.Text = Nothing
            ChkCaseSensitive.Checked = False
            ChkRegex.Checked = False
            ChkEnabled.Checked = True
        Else
            MsgBox("You need to fill in the appropriate information to create an replacement.", MsgBoxStyle.Critical, Text)
        End If
    End Sub

    Protected Overrides Sub WndProc(ByRef m As Message)
        Const WM_EXITSIZEMOVE As Integer = &H232

        MyBase.WndProc(m)

        If m.Msg = WM_EXITSIZEMOVE AndAlso boolDoneLoading Then My.Settings.replacementsLocation = Location
    End Sub

    Private Sub Replacements_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        BtnCancel.Visible = False
        Location = VerifyWindowLocation(My.Settings.replacementsLocation, Me)
        Dim listOfReplacementsToAdd As New List(Of MyReplacementsListViewItem)

        If My.Settings.replacements IsNot Nothing AndAlso My.Settings.replacements.Count > 0 Then
            For Each strJSONString As String In My.Settings.replacements
                listOfReplacementsToAdd.Add(Newtonsoft.Json.JsonConvert.DeserializeObject(Of ReplacementsClass)(strJSONString, JSONDecoderSettingsForSettingsFiles).ToListViewItem())
            Next
        End If

        ReplacementsListView.Items.AddRange(listOfReplacementsToAdd.ToArray())

        Replace.Width = My.Settings.colReplacementsReplace
        ReplaceWith.Width = My.Settings.colReplacementsReplaceWith
        Regex.Width = My.Settings.colReplacementsRegex
        CaseSensitive.Width = My.Settings.colReplacementsCaseSensitive
        ColEnabled.Width = My.Settings.colReplacementsEnabled

        Size = My.Settings.ConfigureReplacementsSize

        boolDoneLoading = True
    End Sub

    Private Sub Replacements_Closing(sender As Object, e As ComponentModel.CancelEventArgs) Handles Me.Closing
        If boolChanged Then
            replacementsList.Clear()

            Dim replacementsClass As ReplacementsClass
            Dim tempReplacements As New Specialized.StringCollection()

            For Each item As MyReplacementsListViewItem In ReplacementsListView.Items
                replacementsClass = New ReplacementsClass With {.BoolRegex = item.BoolRegex, .StrReplace = item.SubItems(0).Text, .StrReplaceWith = item.SubItems(1).Text, .BoolCaseSensitive = item.BoolCaseSensitive, .BoolEnabled = item.BoolEnabled}
                If replacementsClass.BoolEnabled Then replacementsList.Add(replacementsClass)
                tempReplacements.Add(Newtonsoft.Json.JsonConvert.SerializeObject(replacementsClass))
            Next

            My.Settings.replacements = tempReplacements
            My.Settings.Save()
        End If
    End Sub

    Private Sub ReplacementsListView_KeyUp(sender As Object, e As KeyEventArgs) Handles ReplacementsListView.KeyUp
        If ReplacementsListView.SelectedItems.Count > 0 Then
            If e.KeyCode = Keys.Delete Then
                ReplacementsListView.Items.Remove(ReplacementsListView.SelectedItems(0))
                BtnDelete.Enabled = False
                BtnEdit.Enabled = False
                boolChanged = True
            ElseIf e.KeyCode = Keys.Enter Then
                EditItem()
            End If
        End If
    End Sub

    Private Sub BtnDelete_Click(sender As Object, e As EventArgs) Handles BtnDelete.Click
        If ReplacementsListView.SelectedItems.Count > 0 Then
            If ReplacementsListView.SelectedItems.Count = 1 Then
                ReplacementsListView.Items.Remove(ReplacementsListView.SelectedItems(0))
            Else
                For Each item As ListViewItem In ReplacementsListView.SelectedItems
                    item.Remove()
                Next
            End If

            boolChanged = True
        End If
    End Sub

    Private Sub EditItem()
        If ReplacementsListView.SelectedItems.Count > 0 Then
            BtnCancel.Visible = True
            ReplacementsListView.Enabled = False
            boolEditMode = True
            BtnAdd.Text = "Save"
            Label3.Text = "Edit Replacement"

            Dim selectedItemObject As MyReplacementsListViewItem = DirectCast(ReplacementsListView.SelectedItems(0), MyReplacementsListViewItem)

            TxtReplace.Text = selectedItemObject.SubItems(0).Text
            TxtReplaceWith.Text = selectedItemObject.SubItems(1).Text
            ChkEnabled.Checked = selectedItemObject.BoolEnabled
            ChkCaseSensitive.Checked = selectedItemObject.BoolCaseSensitive
            ChkRegex.Checked = selectedItemObject.BoolRegex
        End If
    End Sub

    Private Sub BtnEdit_Click(sender As Object, e As EventArgs) Handles BtnEdit.Click
        EditItem()
    End Sub

    Private Sub ReplacementsListView_Click(sender As Object, e As EventArgs) Handles ReplacementsListView.Click
        If ReplacementsListView.SelectedItems.Count > 0 Then
            BtnDelete.Enabled = True
            BtnEdit.Enabled = True
            BtnEnableDisable.Enabled = True

            BtnEnableDisable.Text = If(DirectCast(ReplacementsListView.SelectedItems(0), MyReplacementsListViewItem).BoolEnabled, "Disable", "Enable")

            BtnUp.Enabled = ReplacementsListView.SelectedIndices(0) <> 0
            BtnDown.Enabled = ReplacementsListView.SelectedIndices(0) <> ReplacementsListView.Items.Count - 1
        Else
            BtnDelete.Enabled = False
            BtnEdit.Enabled = False
            BtnEnableDisable.Enabled = False
        End If
    End Sub

    Private Sub ReplacementsListView_DoubleClick(sender As Object, e As EventArgs) Handles ReplacementsListView.DoubleClick
        EditItem()
    End Sub

    Private Sub ListViewMenu_Opening(sender As Object, e As ComponentModel.CancelEventArgs) Handles ListViewMenu.Opening
        If ReplacementsListView.SelectedItems.Count = 0 And ReplacementsListView.SelectedItems.Count > 1 Then
            e.Cancel = True
            Exit Sub
        Else
            Dim selectedItem As MyReplacementsListViewItem = ReplacementsListView.SelectedItems(0)
            EnableDisableToolStripMenuItem.Text = If(selectedItem.BoolEnabled, "Disable", "Enable")
        End If
    End Sub

    Private Sub DisableEnableItem()
        Dim selectedItem As MyReplacementsListViewItem = ReplacementsListView.SelectedItems(0)

        If selectedItem.BoolEnabled Then
            selectedItem.BoolEnabled = False
            selectedItem.SubItems(4).Text = "No"
            BtnEnableDisable.Text = "Enable"
        Else
            selectedItem.BoolEnabled = True
            selectedItem.SubItems(4).Text = "Yes"
            BtnEnableDisable.Text = "Disable"
        End If

        boolChanged = True
    End Sub

    Private Sub BtnEnableDisable_Click(sender As Object, e As EventArgs) Handles BtnEnableDisable.Click
        DisableEnableItem()
    End Sub

    Private Sub EnableDisableToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles EnableDisableToolStripMenuItem.Click
        DisableEnableItem()
    End Sub

    Private Sub ReplacementsListView_ColumnWidthChanged(sender As Object, e As ColumnWidthChangedEventArgs) Handles ReplacementsListView.ColumnWidthChanged
        If boolDoneLoading Then
            My.Settings.colReplacementsReplace = Replace.Width
            My.Settings.colReplacementsReplaceWith = ReplaceWith.Width
            My.Settings.colReplacementsRegex = Regex.Width
            My.Settings.colReplacementsCaseSensitive = CaseSensitive.Width
            My.Settings.colReplacementsEnabled = ColEnabled.Width
        End If
    End Sub

    Private Sub Replacements_ResizeEnd(sender As Object, e As EventArgs) Handles Me.ResizeEnd
        If boolDoneLoading Then My.Settings.ConfigureReplacementsSize = Size
    End Sub

    Private Sub BtnExport_Click(sender As Object, e As EventArgs) Handles BtnExport.Click
        If ReplacementsListView.Items.Count() = 0 Then
            MsgBox("There's nothing to export.", MsgBoxStyle.Critical, Text)
            Exit Sub
        End If

        Dim saveFileDialog As New SaveFileDialog() With {.Title = "Export Replacements", .Filter = "JSON File|*.json", .OverwritePrompt = True}
        Dim listOfReplacementsClass As New List(Of ReplacementsClass)

        If saveFileDialog.ShowDialog() = DialogResult.OK Then
            For Each item As MyReplacementsListViewItem In ReplacementsListView.Items
                listOfReplacementsClass.Add(New ReplacementsClass With {.BoolRegex = item.BoolRegex, .StrReplace = item.SubItems(0).Text, .StrReplaceWith = item.SubItems(1).Text, .BoolCaseSensitive = item.BoolCaseSensitive, .BoolEnabled = item.BoolEnabled})
            Next

            IO.File.WriteAllText(saveFileDialog.FileName, Newtonsoft.Json.JsonConvert.SerializeObject(listOfReplacementsClass, Newtonsoft.Json.Formatting.Indented))

            MsgBox("Data exported successfully.", MsgBoxStyle.Information, Text)
        End If
    End Sub

    Private Sub BtnImport_Click(sender As Object, e As EventArgs) Handles BtnImport.Click
        Dim openFileDialog As New OpenFileDialog() With {.Title = "Import Alerts", .Filter = "JSON File|*.json"}
        Dim listOfReplacementsClass As New List(Of ReplacementsClass)

        If openFileDialog.ShowDialog() = DialogResult.OK Then
            Try
                listOfReplacementsClass = Newtonsoft.Json.JsonConvert.DeserializeObject(Of List(Of ReplacementsClass))(IO.File.ReadAllText(openFileDialog.FileName), JSONDecoderSettingsForLogFiles)

                ReplacementsListView.Items.Clear()
                replacementsList.Clear()

                Dim tempReplacements As New Specialized.StringCollection()

                For Each item As ReplacementsClass In listOfReplacementsClass
                    replacementsList.Add(item)
                    tempReplacements.Add(Newtonsoft.Json.JsonConvert.SerializeObject(item))
                    ReplacementsListView.Items.Add(item.ToListViewItem())
                Next

                My.Settings.replacements = tempReplacements
                My.Settings.Save()

                MsgBox("Data imported successfully.", MsgBoxStyle.Information, Text)
                boolChanged = True
            Catch ex As Newtonsoft.Json.JsonSerializationException
                MsgBox("There was an error decoding the JSON data.", MsgBoxStyle.Critical, Text)
            End Try
        End If
    End Sub

    Private Sub Replacements_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode = Keys.Escape Then Close()
    End Sub

    Private Sub btnDeleteAll_Click(sender As Object, e As EventArgs) Handles btnDeleteAll.Click
        If MsgBox("Are you sure you want to delete all of the replacements?", MsgBoxStyle.Question + MsgBoxStyle.YesNo, Text) = MsgBoxResult.Yes Then
            ReplacementsListView.Items.Clear()
            boolChanged = True
        End If
    End Sub

    Private Sub BtnUp_Click(sender As Object, e As EventArgs) Handles BtnUp.Click
        If ReplacementsListView.SelectedItems.Count = 0 Then Return ' No item selected
        Dim selectedIndex As Integer = ReplacementsListView.SelectedIndices(0)

        ' Ensure the item is not already at the top
        If selectedIndex > 0 Then
            Dim item As MyReplacementsListViewItem = ReplacementsListView.SelectedItems(0)
            ReplacementsListView.Items.RemoveAt(selectedIndex)
            ReplacementsListView.Items.Insert(selectedIndex - 1, item)
            ReplacementsListView.Items(selectedIndex - 1).Selected = True
            boolChanged = True
        End If

        BtnUp.Enabled = ReplacementsListView.SelectedIndices(0) <> 0
        BtnDown.Enabled = ReplacementsListView.SelectedIndices(0) <> ReplacementsListView.Items.Count - 1
    End Sub

    Private Sub BtnDown_Click(sender As Object, e As EventArgs) Handles BtnDown.Click
        If ReplacementsListView.SelectedItems.Count = 0 Then Return ' No item selected
        Dim selectedIndex As Integer = ReplacementsListView.SelectedIndices(0)

        ' Ensure the item is not already at the bottom
        If selectedIndex < ReplacementsListView.Items.Count - 1 Then
            Dim item As MyReplacementsListViewItem = ReplacementsListView.SelectedItems(0)
            ReplacementsListView.Items.RemoveAt(selectedIndex)
            ReplacementsListView.Items.Insert(selectedIndex + 1, item)
            ReplacementsListView.Items(selectedIndex + 1).Selected = True
            boolChanged = True
        End If

        BtnUp.Enabled = ReplacementsListView.SelectedIndices(0) <> 0
        BtnDown.Enabled = ReplacementsListView.SelectedIndices(0) <> ReplacementsListView.Items.Count - 1
    End Sub

    Private Sub BtnCancel_Click(sender As Object, e As EventArgs) Handles BtnCancel.Click
        ReplacementsListView.Enabled = True
        BtnAdd.Text = "Add"
        Label3.Text = "Add Replacement"
        boolEditMode = False
        boolChanged = True
        TxtReplace.Text = Nothing
        TxtReplaceWith.Text = Nothing
        ChkCaseSensitive.Checked = False
        ChkRegex.Checked = False
        ChkEnabled.Checked = True
        BtnCancel.Visible = False
    End Sub
End Class