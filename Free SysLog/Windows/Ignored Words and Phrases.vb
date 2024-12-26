Imports Free_SysLog.SupportCode

Public Class IgnoredWordsAndPhrases
    Private boolDoneLoading As Boolean = False
    Public boolChanged As Boolean = False
    Private boolEditMode As Boolean = False

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
                    .BoolCaseSensitive = ChkCaseSensitive.Checked
                    .BoolEnabled = ChkEnabled.Checked
                    .BoolRegex = ChkRegex.Checked
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
                    .BoolRegex = ChkRegex.Checked
                    .BoolCaseSensitive = ChkCaseSensitive.Checked
                    .BoolEnabled = ChkEnabled.Checked
                    If My.Settings.font IsNot Nothing Then .Font = My.Settings.font
                End With

                IgnoredListView.Items.Add(IgnoredListViewItem)
            End If

            boolEditMode = False
            boolChanged = True
            TxtIgnored.Text = Nothing
            ChkCaseSensitive.Checked = False
            ChkRegex.Checked = False
            ChkEnabled.Checked = True
        End If
    End Sub

    Private Sub IgnoredWordsAndPhrases_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        If boolChanged Then
            ignoredList.Clear()

            Dim ignoredClass As IgnoredClass
            Dim tempIgnored As New Specialized.StringCollection()

            For Each item As MyIgnoredListViewItem In IgnoredListView.Items
                ignoredClass = New IgnoredClass() With {.StrIgnore = item.SubItems(0).Text, .BoolCaseSensitive = item.BoolCaseSensitive, .BoolRegex = item.BoolRegex, .BoolEnabled = item.BoolEnabled}
                If ignoredClass.BoolEnabled Then ignoredList.Add(ignoredClass)
                tempIgnored.Add(Newtonsoft.Json.JsonConvert.SerializeObject(ignoredClass))
            Next

            My.Settings.ignored2 = tempIgnored
            My.Settings.Save()
        End If
    End Sub

    Private Sub IgnoredWordsAndPhrases_Load(sender As Object, e As EventArgs) Handles Me.Load
        BtnCancel.Visible = False
        Location = VerifyWindowLocation(My.Settings.ignoredWordsLocation, Me)
        Dim MyIgnoredListViewItem As New List(Of MyIgnoredListViewItem)

        If My.Settings.ignored2 IsNot Nothing AndAlso My.Settings.ignored2.Count > 0 Then
            For Each strJSONString As String In My.Settings.ignored2
                MyIgnoredListViewItem.Add(Newtonsoft.Json.JsonConvert.DeserializeObject(Of IgnoredClass)(strJSONString, JSONDecoderSettingsForSettingsFiles).ToListViewItem())
            Next
        End If

        IgnoredListView.Items.AddRange(MyIgnoredListViewItem.ToArray())

        Replace.Width = My.Settings.colIgnoredReplace
        Regex.Width = My.Settings.colIgnoredRegex
        CaseSensitive.Width = My.Settings.colIgnoredCaseSensitive
        ColEnabled.Width = My.Settings.colIgnoredEnabled

        Size = My.Settings.ConfigureIgnoredSize

        boolDoneLoading = True
    End Sub

    Private Sub IgnoredListView_KeyUp(sender As Object, e As KeyEventArgs) Handles IgnoredListView.KeyUp
        If e.KeyCode = Keys.Delete And IgnoredListView.SelectedItems().Count > 0 Then
            IgnoredListView.Items.Remove(IgnoredListView.SelectedItems(0))
            BtnDelete.Enabled = False
            BtnEdit.Enabled = False
            boolChanged = True
        End If
    End Sub

    Private Sub BtnDelete_Click(sender As Object, e As EventArgs) Handles BtnDelete.Click
        If IgnoredListView.SelectedItems.Count > 0 Then
            If IgnoredListView.SelectedItems.Count = 1 Then
                IgnoredListView.Items.Remove(IgnoredListView.SelectedItems(0))
            Else
                For Each item As ListViewItem In IgnoredListView.SelectedItems
                    item.Remove()
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
            BtnEdit.Enabled = True
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

            TxtIgnored.Text = selectedItemObject.SubItems(0).Text
            ChkRegex.Checked = selectedItemObject.BoolRegex
            ChkCaseSensitive.Checked = selectedItemObject.BoolCaseSensitive
            ChkEnabled.Checked = selectedItemObject.BoolEnabled
        End If
    End Sub

    Private Sub IgnoredListView_DoubleClick(sender As Object, e As EventArgs) Handles IgnoredListView.DoubleClick
        EditItem()
    End Sub

    Private Sub BtnEdit_Click(sender As Object, e As EventArgs) Handles BtnEdit.Click
        EditItem()
    End Sub

    Private Sub ListViewMenu_Opening(sender As Object, e As ComponentModel.CancelEventArgs) Handles ListViewMenu.Opening
        If IgnoredListView.SelectedItems.Count = 0 And IgnoredListView.SelectedItems.Count > 1 Then
            e.Cancel = True
            Exit Sub
        Else
            Dim selectedItem As MyIgnoredListViewItem = IgnoredListView.SelectedItems(0)
            EnableDisableToolStripMenuItem.Text = If(selectedItem.BoolEnabled, "Disable", "Enable")
        End If
    End Sub

    Private Sub DisableEnableItem()
        Dim selectedItem As MyIgnoredListViewItem = IgnoredListView.SelectedItems(0)

        If selectedItem.BoolEnabled Then
            selectedItem.BoolEnabled = False
            selectedItem.SubItems(3).Text = "No"
            BtnEnableDisable.Text = "Enable"
        Else
            selectedItem.BoolEnabled = True
            selectedItem.SubItems(3).Text = "Yes"
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

    Private Sub IgnoredListView_ColumnWidthChanged(sender As Object, e As ColumnWidthChangedEventArgs) Handles IgnoredListView.ColumnWidthChanged
        If boolDoneLoading Then
            My.Settings.colIgnoredReplace = Replace.Width
            My.Settings.colIgnoredRegex = Regex.Width
            My.Settings.colIgnoredCaseSensitive = CaseSensitive.Width
            My.Settings.colIgnoredEnabled = ColEnabled.Width
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
                listOfIgnoredClass.Add(New IgnoredClass() With {.StrIgnore = item.SubItems(0).Text, .BoolCaseSensitive = item.BoolCaseSensitive, .BoolRegex = item.BoolRegex, .BoolEnabled = item.BoolEnabled})
            Next

            IO.File.WriteAllText(saveFileDialog.FileName, Newtonsoft.Json.JsonConvert.SerializeObject(listOfIgnoredClass, Newtonsoft.Json.Formatting.Indented))

            MsgBox("Data exported successfully.", MsgBoxStyle.Information, Text)
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
        If e.KeyCode = Keys.Escape Then Close()
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
        ChkCaseSensitive.Checked = False
        ChkRegex.Checked = False
        ChkEnabled.Checked = True
        BtnCancel.Visible = False
    End Sub
End Class