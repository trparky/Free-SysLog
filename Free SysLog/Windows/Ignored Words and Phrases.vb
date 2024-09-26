Imports Free_SysLog.SupportCode

Public Class IgnoredWordsAndPhrases
    Private boolDoneLoading As Boolean = False
    Public boolChanged As Boolean = False

    Private Function CheckForExistingItem(strIgnored As String) As Boolean
        Return IgnoredListView.Items.Cast(Of MyIgnoredListViewItem).Any(Function(item As MyIgnoredListViewItem)
                                                                            Return item.SubItems(0).Text.Equals(strIgnored, StringComparison.OrdinalIgnoreCase)
                                                                        End Function)
    End Function

    Private Sub BtnAdd_Click(sender As Object, e As EventArgs) Handles BtnAdd.Click
        Using AddIgnored As New AddIgnored With {.StartPosition = FormStartPosition.CenterParent, .Icon = Icon, .Text = "Add Ignored String"}
            AddIgnored.ShowDialog(Me)

            If AddIgnored.boolSuccess Then
                If CheckForExistingItem(AddIgnored.strIgnored) Then
                    MsgBox("A similar item has already been found in your ignored list.", MsgBoxStyle.Critical, Text)
                    Exit Sub
                End If

                Dim IgnoredListViewItem As New MyIgnoredListViewItem(AddIgnored.strIgnored)

                With IgnoredListViewItem
                    .SubItems.Add(If(AddIgnored.boolRegex, "Yes", "No"))
                    .SubItems.Add(If(AddIgnored.boolCaseSensitive, "Yes", "No"))
                    .SubItems.Add(If(AddIgnored.boolEnabled, "Yes", "No"))
                    .BoolRegex = AddIgnored.boolRegex
                    .BoolCaseSensitive = AddIgnored.boolCaseSensitive
                    .BoolEnabled = AddIgnored.boolEnabled
                End With

                IgnoredListView.Items.Add(IgnoredListViewItem)
                boolChanged = True
            End If
        End Using
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
        If IgnoredListView.SelectedItems().Count > 0 Then
            IgnoredListView.Items.Remove(IgnoredListView.SelectedItems(0))
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
            Using AddIgnored As New AddIgnored With {.StartPosition = FormStartPosition.CenterParent, .Icon = Icon, .boolEditMode = True}
                Dim selectedItemObject As MyIgnoredListViewItem = DirectCast(IgnoredListView.SelectedItems(0), MyIgnoredListViewItem)

                With AddIgnored
                    .strIgnored = selectedItemObject.SubItems(0).Text
                    .boolRegex = selectedItemObject.BoolRegex
                    .boolCaseSensitive = selectedItemObject.BoolCaseSensitive
                    .boolEnabled = selectedItemObject.BoolEnabled
                End With

                AddIgnored.ShowDialog(Me)

                If AddIgnored.boolSuccess Then
                    With selectedItemObject
                        .SubItems(0).Text = AddIgnored.strIgnored
                        .SubItems(1).Text = If(AddIgnored.boolRegex, "Yes", "No")
                        .SubItems(2).Text = If(AddIgnored.boolCaseSensitive, "Yes", "No")
                        .SubItems(3).Text = If(AddIgnored.boolEnabled, "Yes", "No")
                        .BoolRegex = AddIgnored.boolRegex
                        .BoolCaseSensitive = AddIgnored.boolCaseSensitive
                        .BoolEnabled = AddIgnored.boolEnabled
                    End With

                    boolChanged = True
                End If
            End Using
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

            IO.File.WriteAllText(saveFileDialog.FileName, Newtonsoft.Json.JsonConvert.SerializeObject(listOfIgnoredClass))

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
End Class