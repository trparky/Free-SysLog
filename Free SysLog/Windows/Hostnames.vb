Imports System.Net

Public Class Hostnames
    Private selectedItem As ListViewItem
    Private boolEditMode As Boolean = False
    Private boolDoneLoading As Boolean = False
    Private draggedItem As ListViewItem

    Private Sub IgnoredListView_ItemDrag(sender As Object, e As ItemDragEventArgs) Handles ListHostnames.ItemDrag
        draggedItem = CType(e.Item, ListViewItem)
        DoDragDrop(e.Item, DragDropEffects.Move)
    End Sub

    Private Sub IgnoredListView_DragEnter(sender As Object, e As DragEventArgs) Handles ListHostnames.DragEnter
        If e.Data.GetDataPresent(GetType(ListViewItem)) Then
            e.Effect = DragDropEffects.Move
        Else
            e.Effect = DragDropEffects.None
        End If
    End Sub

    Private Sub IgnoredListView_DragOver(sender As Object, e As DragEventArgs) Handles ListHostnames.DragOver
        e.Effect = DragDropEffects.Move
    End Sub

    Private Sub IgnoredListView_DragDrop(sender As Object, e As DragEventArgs) Handles ListHostnames.DragDrop
        If draggedItem Is Nothing Then Return

        Dim cp As Point = ListHostnames.PointToClient(New Point(e.X, e.Y))
        Dim targetItem As ListViewItem = ListHostnames.GetItemAt(cp.X, cp.Y)

        If targetItem Is Nothing OrElse targetItem Is draggedItem Then Return

        Dim targetIndex As Integer = targetItem.Index
        Dim draggedIndex As Integer = draggedItem.Index

        ' Remove and re-insert the dragged item
        ListHostnames.Items.RemoveAt(draggedIndex)
        ListHostnames.Items.Insert(targetIndex, draggedItem)

        ' Re-select the moved item
        draggedItem.Selected = True
        draggedItem.Focused = True
    End Sub

    Private Sub BtnEdit_Click(sender As Object, e As EventArgs) Handles BtnEdit.Click
        If ListHostnames.SelectedItems.Count > 0 Then
            selectedItem = ListHostnames.SelectedItems(0)

            txtIP.Text = selectedItem.SubItems(0).Text
            txtHostname.Text = selectedItem.SubItems(1).Text

            boolEditMode = True
            BtnAddSave.Text = "Save"
            lblAddEditHostNameLabel.Text = "Edit Custom Hostname"
        End If
    End Sub

    Private Sub BtnAddSave_Click(sender As Object, e As EventArgs) Handles BtnAddSave.Click
        Dim tempIP As IPAddress = Nothing

        If IPAddress.TryParse(txtIP.Text, tempIP) Then
            If boolEditMode Then
                selectedItem.SubItems(0).Text = txtIP.Text
                selectedItem.SubItems(1).Text = txtHostname.Text

                lblAddEditHostNameLabel.Text = "Add New Custom Hostname"
                BtnAddSave.Text = "Add"
            Else
                Dim newListViewItem As New ListViewItem(txtIP.Text)
                newListViewItem.SubItems.Add(txtHostname.Text)
                If My.Settings.font IsNot Nothing Then newListViewItem.Font = My.Settings.font

                ListHostnames.Items.Add(newListViewItem)
            End If

            boolEditMode = False
            txtIP.Text = Nothing
            txtHostname.Text = Nothing
        Else
            MsgBox("Invalid IP Address.", MsgBoxStyle.Critical, Text)
        End If
    End Sub

    Private Sub ListHostnames_Click(sender As Object, e As EventArgs) Handles ListHostnames.Click
        BtnDelete.Enabled = ListHostnames.SelectedItems.Count > 0
        BtnEdit.Enabled = ListHostnames.SelectedItems.Count > 0

        BtnUp.Enabled = ListHostnames.SelectedIndices(0) <> 0
        BtnDown.Enabled = ListHostnames.SelectedIndices(0) <> ListHostnames.Items.Count - 1
    End Sub

    Protected Overrides Sub WndProc(ByRef m As Message)
        Const WM_EXITSIZEMOVE As Integer = &H232

        MyBase.WndProc(m)

        If m.Msg = WM_EXITSIZEMOVE AndAlso boolDoneLoading Then My.Settings.hostnamesLocation = Location
    End Sub

    Private Sub Hostnames_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Location = SupportCode.VerifyWindowLocation(My.Settings.hostnamesLocation, Me)
        Dim listOfHostnamesToAdd As New List(Of ListViewItem)

        If My.Settings.hostnames IsNot Nothing AndAlso My.Settings.hostnames.Count > 0 Then
            For Each strJSONString As String In My.Settings.hostnames
                listOfHostnamesToAdd.Add(Newtonsoft.Json.JsonConvert.DeserializeObject(Of CustomHostname)(strJSONString, SupportCode.JSONDecoderSettingsForSettingsFiles).ToListViewItem())
            Next
        End If

        ListHostnames.Items.AddRange(listOfHostnamesToAdd.ToArray)
        boolDoneLoading = True
    End Sub

    Private Sub Hostnames_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        Dim tempHostnames As New Specialized.StringCollection()
        Dim newHostNames As New Concurrent.ConcurrentDictionary(Of String, String)

        Try
            Dim hostName As CustomHostname

            For Each item As ListViewItem In ListHostnames.Items
                hostName = New CustomHostname() With {
                    .ip = item.SubItems(0).Text,
                    .deviceName = item.SubItems(1).Text
                }

                tempHostnames.Add(Newtonsoft.Json.JsonConvert.SerializeObject(hostName))
                newHostNames(item.SubItems(0).Text) = item.SubItems(1).Text
            Next

            ' We now save the new list to the main lists in memory now that we know nothing wrong happened above.
            SupportCode.hostnames.Clear()
            SupportCode.hostnames = newHostNames

            My.Settings.hostnames = tempHostnames
            My.Settings.Save()
        Catch
        End Try
    End Sub

    Private Sub BtnDelete_Click(sender As Object, e As EventArgs) Handles BtnDelete.Click
        If ListHostnames.SelectedItems.Count > 0 Then
            If ListHostnames.SelectedItems.Count = 1 Then
                ListHostnames.Items.Remove(ListHostnames.SelectedItems(0))
            Else
                For i As Integer = ListHostnames.SelectedItems.Count - 1 To 0 Step -1
                    ListHostnames.SelectedItems(i).Remove()
                Next
            End If
        End If
    End Sub

    Private Sub BtnUp_Click(sender As Object, e As EventArgs) Handles BtnUp.Click
        If ListHostnames.SelectedItems.Count = 0 Then Return ' No item selected
        Dim selectedIndex As Integer = ListHostnames.SelectedIndices(0)

        ' Ensure the item is not already at the top
        If selectedIndex > 0 Then
            Dim item As ListViewItem = ListHostnames.SelectedItems(0)
            ListHostnames.Items.RemoveAt(selectedIndex)
            ListHostnames.Items.Insert(selectedIndex - 1, item)
            ListHostnames.Items(selectedIndex - 1).Selected = True
        End If

        BtnUp.Enabled = ListHostnames.SelectedIndices(0) <> 0
        BtnDown.Enabled = ListHostnames.SelectedIndices(0) <> ListHostnames.Items.Count - 1
    End Sub

    Private Sub BtnDown_Click(sender As Object, e As EventArgs) Handles BtnDown.Click
        If ListHostnames.SelectedItems.Count = 0 Then Return ' No item selected
        Dim selectedIndex As Integer = ListHostnames.SelectedIndices(0)

        ' Ensure the item is not already at the bottom
        If selectedIndex < ListHostnames.Items.Count - 1 Then
            Dim item As ListViewItem = ListHostnames.SelectedItems(0)
            ListHostnames.Items.RemoveAt(selectedIndex)
            ListHostnames.Items.Insert(selectedIndex + 1, item)
            ListHostnames.Items(selectedIndex + 1).Selected = True
        End If

        BtnUp.Enabled = ListHostnames.SelectedIndices(0) <> 0
        BtnDown.Enabled = ListHostnames.SelectedIndices(0) <> ListHostnames.Items.Count - 1
    End Sub

    Private Sub BtnExport_Click(sender As Object, e As EventArgs) Handles BtnExport.Click
        Dim saveFileDialog As New SaveFileDialog() With {.Title = "Export Hostnames", .Filter = "JSON File|*.json", .OverwritePrompt = True}
        Dim stringCollection As New Specialized.StringCollection()

        If saveFileDialog.ShowDialog() = DialogResult.OK Then
            For Each item As ListViewItem In ListHostnames.Items
                stringCollection.Add(Newtonsoft.Json.JsonConvert.SerializeObject(New CustomHostname() With {.ip = item.SubItems(0).Text, .deviceName = item.SubItems(1).Text}))
            Next

            IO.File.WriteAllText(saveFileDialog.FileName, Newtonsoft.Json.JsonConvert.SerializeObject(My.Settings.hostnames, Newtonsoft.Json.Formatting.Indented))

            If My.Settings.AskOpenExplorer Then
                Using OpenExplorer As New OpenExplorer()
                    OpenExplorer.StartPosition = FormStartPosition.CenterParent
                    OpenExplorer.MyParentForm = Me

                    Dim result As DialogResult = OpenExplorer.ShowDialog(Me)

                    If result = DialogResult.No Then
                        Exit Sub
                    ElseIf result = DialogResult.Yes Then
                        SupportCode.SelectFileInWindowsExplorer(saveFileDialog.FileName)
                    End If
                End Using
            Else
                MsgBox("Data exported successfully.", MsgBoxStyle.Information, SupportCode.ParentForm.Text)
            End If
        End If
    End Sub

    Private Sub BtnImport_Click(sender As Object, e As EventArgs) Handles BtnImport.Click
        Dim openFileDialog As New OpenFileDialog() With {.Title = "Import Alerts", .Filter = "JSON File|*.json"}
        Dim stringCollection As New Specialized.StringCollection()
        Dim ipHostnameSplit As String()
        Dim newListViewItem As ListViewItem

        If openFileDialog.ShowDialog() = DialogResult.OK Then
            Try
                Dim strDataFromFile As String = IO.File.ReadAllText(openFileDialog.FileName)
                ListHostnames.Items.Clear()

                If strDataFromFile.CaseInsensitiveContains("ip") And strDataFromFile.CaseInsensitiveContains("deviceName") Then
                    stringCollection = Newtonsoft.Json.JsonConvert.DeserializeObject(Of Specialized.StringCollection)(strDataFromFile, SupportCode.JSONDecoderSettingsForLogFiles)

                    Dim customHostname As CustomHostname

                    For Each item As String In stringCollection
                        customHostname = Newtonsoft.Json.JsonConvert.DeserializeObject(Of CustomHostname)(item, SupportCode.JSONDecoderSettingsForLogFiles)

                        newListViewItem = New ListViewItem(customHostname.ip)
                        newListViewItem.SubItems.Add(customHostname.deviceName)

                        ListHostnames.Items.Add(newListViewItem)
                    Next
                Else
                    stringCollection = Newtonsoft.Json.JsonConvert.DeserializeObject(Of Specialized.StringCollection)(strDataFromFile, SupportCode.JSONDecoderSettingsForLogFiles)

                    For Each item As String In stringCollection
                        ipHostnameSplit = item.Split("|")

                        newListViewItem = New ListViewItem(ipHostnameSplit(0))
                        newListViewItem.SubItems.Add(ipHostnameSplit(1))

                        ListHostnames.Items.Add(newListViewItem)
                    Next
                End If

                My.Settings.Save()

                MsgBox("Data imported successfully.", MsgBoxStyle.Information, Text)
            Catch ex As Newtonsoft.Json.JsonSerializationException
                MsgBox("There was an error decoding the JSON data.", MsgBoxStyle.Critical, Text)
            End Try
        End If
    End Sub

    Private Sub Hostnames_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode = Keys.Delete Then BtnDelete.PerformClick()
    End Sub

    Private Sub ListHostnames_KeyUp(sender As Object, e As KeyEventArgs) Handles ListHostnames.KeyUp
        If e.KeyCode = Keys.Delete Then BtnDelete.PerformClick()
    End Sub
End Class