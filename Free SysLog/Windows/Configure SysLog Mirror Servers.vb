Imports System.ComponentModel
Imports System.Net
Imports Free_SysLog.SupportCode
Imports Windows.AI.MachineLearning

Public Class ConfigureSysLogMirrorServers
    Public boolSuccess As Boolean = False
    Private boolEditMode As Boolean = False

    Private Sub ConfigureSysLogMirrorServers_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If My.Settings.ServersToSendTo IsNot Nothing AndAlso My.Settings.ServersToSendTo.Count > 0 Then
            Dim SysLogProxyServer As SysLogProxyServer

            For Each strJSONString As String In My.Settings.ServersToSendTo
                SysLogProxyServer = Newtonsoft.Json.JsonConvert.DeserializeObject(Of SysLogProxyServer)(strJSONString, JSONDecoderSettingsForSettingsFiles)
                servers.Items.Add(SysLogProxyServer.ToListViewItem())
                SysLogProxyServer = Nothing
            Next
        End If
    End Sub

    Private Sub Servers_SelectedIndexChanged(sender As Object, e As EventArgs) Handles servers.SelectedIndexChanged
        If servers.SelectedItems.Count > 0 Then
            BtnDeleteServer.Enabled = True
            BtnEditServer.Enabled = True
            btnEnableDisable.Enabled = True

            btnEnableDisable.Text = If(DirectCast(servers.SelectedItems(0), ServerListViewItem).BoolEnabled, "Disable", "Enable")
        Else
            BtnDeleteServer.Enabled = False
            BtnEditServer.Enabled = False
            btnEnableDisable.Enabled = False
        End If
    End Sub

    Private Sub EditItem()
        If servers.SelectedItems.Count > 0 Then
            servers.Enabled = False
            boolEditMode = True
            BtnAddServer.Text = "Save"
            Label4.Text = "Edit Server"

            Dim selectedItemObject As ServerListViewItem = servers.SelectedItems(0)

            With selectedItemObject
                txtIP.Text = .SubItems(0).Text
                txtPort.Text = .SubItems(1).Text
                txtName.Text = .SubItems(3).Text
                chkEnabled.Checked = .BoolEnabled
            End With
        End If
    End Sub

    Private Sub BtnAddServer_Click(sender As Object, e As EventArgs) Handles BtnAddServer.Click
        If Not String.IsNullOrWhiteSpace(txtIP.Text) And Not String.IsNullOrWhiteSpace(txtPort.Text) And Not String.IsNullOrWhiteSpace(txtName.Text) Then
            Dim tempIP As IPAddress = Nothing
            If Not IPAddress.TryParse(txtIP.Text, tempIP) Then
                MsgBox("You must input a valid IP address.", MsgBoxStyle.Critical, Text)
                Exit Sub
            End If

            If boolEditMode Then
                Dim selectedItemObject As ServerListViewItem = servers.SelectedItems(0)

                With selectedItemObject
                    .SubItems(0).Text = txtIP.Text
                    .SubItems(1).Text = txtPort.Text
                    .SubItems(2).Text = If(chkEnabled.Checked, "Yes", "No")
                    .SubItems(3).Text = txtName.Text
                    .BoolEnabled = chkEnabled.Checked
                End With

                servers.Enabled = True
                BtnAddServer.Text = "Add"
                Label4.Text = "Add Server"
            Else
                Dim ServerListView As New ServerListViewItem(txtIP.Text)

                With ServerListView
                    .SubItems.Add(txtPort.Text)
                    .SubItems.Add(If(chkEnabled.Checked, "Yes", "No"))
                    .SubItems.Add(txtName.Text)
                    .BoolEnabled = chkEnabled.Checked
                End With

                servers.Items.Add(ServerListView)
            End If

            boolEditMode = False
            txtIP.Text = Nothing
            txtName.Text = Nothing
            txtPort.Text = Nothing
            chkEnabled.Checked = True
        Else
            MsgBox("You need to fill in the appropriate information to create a server entry.", MsgBoxStyle.Critical, Text)
        End If
    End Sub

    Private Sub BtnEditServer_Click(sender As Object, e As EventArgs) Handles BtnEditServer.Click
        EditItem()
    End Sub

    Private Sub Servers_DoubleClick(sender As Object, e As EventArgs) Handles servers.DoubleClick
        EditItem()
    End Sub

    Private Sub BtnDeleteServer_Click(sender As Object, e As EventArgs) Handles BtnDeleteServer.Click
        servers.SelectedItems(0).Remove()
    End Sub

    Private Sub ConfigureSysLogMirrorServers_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        serversList.Clear()

        Dim SysLogProxyServer As SysLogProxyServer
        Dim tempServer As New Specialized.StringCollection()

        For Each item As ServerListViewItem In servers.Items
            SysLogProxyServer = New SysLogProxyServer() With {.ip = item.SubItems(0).Text, .port = Integer.Parse(item.SubItems(1).Text), .boolEnabled = item.BoolEnabled, .name = item.SubItems(3).Text}
            If SysLogProxyServer.boolEnabled Then serversList.Add(SysLogProxyServer)
            tempServer.Add(Newtonsoft.Json.JsonConvert.SerializeObject(SysLogProxyServer))
        Next

        My.Settings.ServersToSendTo = tempServer
        My.Settings.Save()
    End Sub

    Private Sub ContextMenuStrip1_Opening(sender As Object, e As CancelEventArgs) Handles ContextMenuStrip1.Opening
        Dim selectedItem As ServerListViewItem = servers.SelectedItems(0)
        EnableDisableToolStripMenuItem.Text = If(selectedItem.BoolEnabled, "Disable", "Enable")
    End Sub

    Private Sub EnableDisableToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles EnableDisableToolStripMenuItem.Click
        Dim selectedItem As ServerListViewItem = servers.SelectedItems(0)
        selectedItem.BoolEnabled = Not selectedItem.BoolEnabled
        selectedItem.SubItems(2).Text = If(selectedItem.BoolEnabled, "Yes", "No")
        btnEnableDisable.Text = If(selectedItem.BoolEnabled, "Disable", "Enable")
    End Sub

    Private Sub BtnEnableDisable_Click(sender As Object, e As EventArgs) Handles btnEnableDisable.Click
        Dim selectedItem As ServerListViewItem = servers.SelectedItems(0)
        selectedItem.BoolEnabled = Not selectedItem.BoolEnabled
        selectedItem.SubItems(2).Text = If(selectedItem.BoolEnabled, "Yes", "No")
        btnEnableDisable.Text = If(selectedItem.BoolEnabled, "Disable", "Enable")
    End Sub

    Private Sub Servers_KeyUp(sender As Object, e As KeyEventArgs) Handles servers.KeyUp
        If e.KeyCode = Keys.Delete And servers.SelectedItems().Count > 0 Then
            servers.Items.Remove(servers.SelectedItems(0))
            BtnDeleteServer.Enabled = False
            BtnEditServer.Enabled = False
        End If
    End Sub

    Private Sub BtnExport_Click(sender As Object, e As EventArgs) Handles BtnExport.Click
        If servers.Items.Count() = 0 Then
            MsgBox("There's nothing to export.", MsgBoxStyle.Critical, Text)
            Exit Sub
        End If

        Dim saveFileDialog As New SaveFileDialog() With {.Title = "Export Servers", .Filter = "JSON File|*.json", .OverwritePrompt = True}
        Dim listOfSysLogProxyServer As New List(Of SysLogProxyServer)

        If saveFileDialog.ShowDialog() = DialogResult.OK Then
            For Each item As ServerListViewItem In servers.Items
                listOfSysLogProxyServer.Add(New SysLogProxyServer() With {.ip = item.SubItems(0).Text, .port = Integer.Parse(item.SubItems(1).Text), .boolEnabled = item.BoolEnabled, .name = item.SubItems(3).Text})
            Next

            IO.File.WriteAllText(saveFileDialog.FileName, Newtonsoft.Json.JsonConvert.SerializeObject(listOfSysLogProxyServer, Newtonsoft.Json.Formatting.Indented))

            MsgBox("Data exported successfully.", MsgBoxStyle.Information, Text)
        End If
    End Sub

    Private Sub BtnImport_Click(sender As Object, e As EventArgs) Handles BtnImport.Click
        Dim openFileDialog As New OpenFileDialog() With {.Title = "Import Alerts", .Filter = "JSON File|*.json"}
        Dim listOfSysLogProxyServer As New List(Of SysLogProxyServer)

        If openFileDialog.ShowDialog() = DialogResult.OK Then
            Try
                listOfSysLogProxyServer = Newtonsoft.Json.JsonConvert.DeserializeObject(Of List(Of SysLogProxyServer))(IO.File.ReadAllText(openFileDialog.FileName), JSONDecoderSettingsForLogFiles)

                servers.Items.Clear()
                serversList.Clear()

                Dim tempServer As New Specialized.StringCollection()

                For Each item As SysLogProxyServer In listOfSysLogProxyServer
                    serversList.Add(item)
                    tempServer.Add(Newtonsoft.Json.JsonConvert.SerializeObject(item))
                    servers.Items.Add(item.ToListViewItem())
                Next

                My.Settings.ServersToSendTo = tempServer
                My.Settings.Save()

                MsgBox("Data imported successfully.", MsgBoxStyle.Information, Text)
            Catch ex As Newtonsoft.Json.JsonSerializationException
                MsgBox("There was an error decoding the JSON data.", MsgBoxStyle.Critical, Text)
            End Try
        End If
    End Sub

    Private Sub ConfigureSysLogMirrorServers_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode = Keys.Escape Then Close()
    End Sub

    Private Sub btnDeleteAll_Click(sender As Object, e As EventArgs) Handles btnDeleteAll.Click
        If MsgBox("Are you sure you want to delete all of the mirror servers?", MsgBoxStyle.Question + MsgBoxStyle.YesNo, Text) = MsgBoxResult.Yes Then
            servers.Items.Clear()
        End If
    End Sub

    Private Sub BtnUp_Click(sender As Object, e As EventArgs) Handles BtnUp.Click
        If servers.SelectedItems.Count = 0 Then Return ' No item selected
        Dim selectedIndex As Integer = servers.SelectedIndices(0)

        ' Ensure the item is not already at the top
        If selectedIndex > 0 Then
            Dim item As ServerListViewItem = servers.SelectedItems(0)
            servers.Items.RemoveAt(selectedIndex)
            servers.Items.Insert(selectedIndex - 1, item)
            servers.Items(selectedIndex - 1).Selected = True
        End If
    End Sub

    Private Sub BtnDown_Click(sender As Object, e As EventArgs) Handles BtnDown.Click
        If servers.SelectedItems.Count = 0 Then Return ' No item selected
        Dim selectedIndex As Integer = servers.SelectedIndices(0)

        ' Ensure the item is not already at the bottom
        If selectedIndex < servers.Items.Count - 1 Then
            Dim item As ServerListViewItem = servers.SelectedItems(0)
            servers.Items.RemoveAt(selectedIndex)
            servers.Items.Insert(selectedIndex + 1, item)
            servers.Items(selectedIndex + 1).Selected = True
        End If
    End Sub
End Class