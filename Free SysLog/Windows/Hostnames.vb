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

                ListHostnames.Items.Add(newListViewItem)

                txtIP.Text = Nothing
                txtHostname.Text = Nothing
            End If
        Else
            MsgBox("Invalid IP Address.", MsgBoxStyle.Critical, Text)
        End If
    End Sub

    Private Sub ListHostnames_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListHostnames.SelectedIndexChanged
        BtnDelete.Enabled = ListHostnames.SelectedItems.Count > 0
        BtnEdit.Enabled = ListHostnames.SelectedItems.Count > 0
    End Sub

    Private Sub Hostnames_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim ipHostNameSplit As String()
        Dim newListViewItem As ListViewItem

        If My.Settings.hostnames IsNot Nothing AndAlso My.Settings.hostnames.Count > 0 Then
            For Each item As String In My.Settings.hostnames
                ipHostNameSplit = item.Split("|")

                newListViewItem = New ListViewItem(ipHostNameSplit(0))
                newListViewItem.SubItems.Add(ipHostNameSplit(1))

                ListHostnames.Items.Add(newListViewItem)
            Next
        End If
    End Sub

    Private Sub Hostnames_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        SupportCode.hostnames.Clear()

        If My.Settings.hostnames IsNot Nothing Then
            My.Settings.hostnames.Clear()
        Else
            My.Settings.hostnames = New Specialized.StringCollection
        End If

        For Each item As ListViewItem In ListHostnames.Items
            My.Settings.hostnames.Add($"{item.SubItems(0).Text}|{item.SubItems(1).Text}")
            SupportCode.hostnames.Add(item.SubItems(0).Text, item.SubItems(1).Text)
        Next

        My.Settings.Save()
    End Sub

    Private Sub BtnDelete_Click(sender As Object, e As EventArgs) Handles BtnDelete.Click
        ListHostnames.SelectedItems(0).Remove()
    End Sub
End Class