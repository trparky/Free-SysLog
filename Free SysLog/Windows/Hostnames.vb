Imports System.Net

Public Class Hostnames
    Private selectedItem As ListViewItem

    Private Sub BtnEdit_Click(sender As Object, e As EventArgs) Handles BtnEdit.Click
        If ListHostnames.SelectedItems.Count > 0 Then
            selectedItem = ListHostnames.SelectedItems(0)

            txtIP.Text = selectedItem.SubItems(0).Text
            txtHostname.Text = selectedItem.SubItems(1).Text

            BtnAddSave.Text = "Save"
        End If
    End Sub

    Private Sub BtnAddSave_Click(sender As Object, e As EventArgs) Handles BtnAddSave.Click
        Dim tempIP As IPAddress = Nothing

        If IPAddress.TryParse(txtIP.Text, tempIP) Then
            If BtnAddSave.Text = "Save" Then
                selectedItem.SubItems(0).Text = txtIP.Text
                selectedItem.SubItems(1).Text = txtHostname.Text

                BtnAddSave.Text = "Add"
                txtIP.Text = Nothing
                txtHostname.Text = Nothing
            Else
                Dim newListViewItem As New ListViewItem(txtIP.Text)
                newListViewItem.SubItems.Add(txtHostname.Text)
                If My.Settings.font IsNot Nothing Then newListViewItem.Font = My.Settings.font

                ListHostnames.Items.Add(newListViewItem)

                txtIP.Text = Nothing
                txtHostname.Text = Nothing
            End If
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

    Private Sub Hostnames_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim listOfHostnamesToAdd As New List(Of ListViewItem)

        If My.Settings.hostnames IsNot Nothing AndAlso My.Settings.hostnames.Count > 0 Then
            For Each strJSONString As String In My.Settings.hostnames
                listOfHostnamesToAdd.Add(Newtonsoft.Json.JsonConvert.DeserializeObject(Of CustomHostname)(strJSONString, SupportCode.JSONDecoderSettingsForSettingsFiles).ToListViewItem())
            Next
        End If

        ListHostnames.Items.AddRange(listOfHostnamesToAdd.ToArray)
    End Sub

    Private Sub Hostnames_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        Dim tempHostnames As New Specialized.StringCollection()
        SupportCode.hostnames.Clear()

        For Each item As ListViewItem In ListHostnames.Items
            tempHostnames.Add(Newtonsoft.Json.JsonConvert.SerializeObject(New CustomHostname() With {.ip = item.SubItems(0).Text, .deviceName = item.SubItems(1).Text}))
            SupportCode.hostnames.Add(item.SubItems(0).Text, item.SubItems(1).Text)
        Next

        My.Settings.hostnames = tempHostnames
        My.Settings.Save()
    End Sub

    Private Sub BtnDelete_Click(sender As Object, e As EventArgs) Handles BtnDelete.Click
        ListHostnames.SelectedItems(0).Remove()
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
                stringCollection.Add($"{item.SubItems(0).Text}|{item.SubItems(1).Text}")
            Next

            IO.File.WriteAllText(saveFileDialog.FileName, Newtonsoft.Json.JsonConvert.SerializeObject(stringCollection, Newtonsoft.Json.Formatting.Indented))

            MsgBox("Data exported successfully.", MsgBoxStyle.Information, Text)
        End If
    End Sub

    Private Sub BtnImport_Click(sender As Object, e As EventArgs) Handles BtnImport.Click
        Dim openFileDialog As New OpenFileDialog() With {.Title = "Import Alerts", .Filter = "JSON File|*.json"}
        Dim stringCollection As New Specialized.StringCollection()
        Dim ipHostnameSplit As String()
        Dim newListViewItem As ListViewItem

        If openFileDialog.ShowDialog() = DialogResult.OK Then
            Try
                stringCollection = Newtonsoft.Json.JsonConvert.DeserializeObject(Of Specialized.StringCollection)(IO.File.ReadAllText(openFileDialog.FileName), SupportCode.JSONDecoderSettingsForLogFiles)

                ListHostnames.Items.Clear()

                For Each item As String In stringCollection
                    ipHostnameSplit = item.Split("|")

                    newListViewItem = New ListViewItem(ipHostnameSplit(0))
                    newListViewItem.SubItems.Add(ipHostnameSplit(1))

                    ListHostnames.Items.Add(newListViewItem)
                Next

                My.Settings.Save()

                MsgBox("Data imported successfully.", MsgBoxStyle.Information, Text)
            Catch ex As Newtonsoft.Json.JsonSerializationException
                MsgBox("There was an error decoding the JSON data.", MsgBoxStyle.Critical, Text)
            End Try
        End If
    End Sub
End Class