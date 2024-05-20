Imports System.ComponentModel

Public Class ConfigureSysLogMirrorServers
    Public boolSuccess As Boolean = False

    Private Sub ConfigureSysLogMirrorServers_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If My.Settings.ServersToSendTo IsNot Nothing AndAlso My.Settings.ServersToSendTo.Count > 0 Then
            Dim SysLogProxyServer As SysLogProxyServer

            For Each strJSONString As String In My.Settings.ServersToSendTo
                SysLogProxyServer = Newtonsoft.Json.JsonConvert.DeserializeObject(Of SysLogProxyServer)(strJSONString)
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
        Using AddSysLogMirrorServer As New AddSysLogMirrorServer With {.StartPosition = FormStartPosition.CenterParent, .Icon = Icon, .boolEditMode = True}
            Dim selectedItemObject As ServerListViewItem = servers.SelectedItems(0)

            With AddSysLogMirrorServer
                .strIP = selectedItemObject.SubItems(0).Text
                .intPort = selectedItemObject.SubItems(1).Text
                .strName = selectedItemObject.SubItems(3).Text
                .boolEnabled = selectedItemObject.BoolEnabled
            End With

            AddSysLogMirrorServer.ShowDialog(Me)

            If AddSysLogMirrorServer.boolSuccess Then
                With selectedItemObject
                    .SubItems(0).Text = AddSysLogMirrorServer.strIP
                    .SubItems(1).Text = AddSysLogMirrorServer.intPort
                    .SubItems(2).Text = If(AddSysLogMirrorServer.boolEnabled, "Yes", "No")
                    .SubItems(3).Text = AddSysLogMirrorServer.strName
                    .BoolEnabled = AddSysLogMirrorServer.boolEnabled
                End With
            End If
        End Using
    End Sub

    Private Sub BtnAddServer_Click(sender As Object, e As EventArgs) Handles BtnAddServer.Click
        Using AddSysLogMirrorServer As New AddSysLogMirrorServer With {.StartPosition = FormStartPosition.CenterParent, .Icon = Icon}
            AddSysLogMirrorServer.ShowDialog(Me)

            If AddSysLogMirrorServer.boolSuccess Then
                Dim ServerListView As New ServerListViewItem(AddSysLogMirrorServer.strIP)
                ServerListView.SubItems.Add(AddSysLogMirrorServer.intPort.ToString)
                ServerListView.SubItems.Add(If(AddSysLogMirrorServer.boolEnabled, "Yes", "No"))
                ServerListView.SubItems.Add(AddSysLogMirrorServer.strName)
                servers.Items.Add(ServerListView)
            End If
        End Using
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

            IO.File.WriteAllText(saveFileDialog.FileName, Newtonsoft.Json.JsonConvert.SerializeObject(listOfSysLogProxyServer))
        End If
    End Sub

    Private Sub BtnImport_Click(sender As Object, e As EventArgs) Handles BtnImport.Click
        Dim openFileDialog As New OpenFileDialog() With {.Title = "Import Alerts", .Filter = "JSON File|*.json"}
        Dim listOfSysLogProxyServer As New List(Of SysLogProxyServer)

        If openFileDialog.ShowDialog() = DialogResult.OK Then
            Try
                listOfSysLogProxyServer = Newtonsoft.Json.JsonConvert.DeserializeObject(Of List(Of SysLogProxyServer))(IO.File.ReadAllText(openFileDialog.FileName), JSONDecoderSettings)

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
            Catch ex As Newtonsoft.Json.JsonSerializationException
                MsgBox("There was an error decoding the JSON data.", MsgBoxStyle.Critical, Text)
            End Try
        End If
    End Sub

    Private Sub ConfigureSysLogMirrorServers_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode = Keys.Escape Then Close()
    End Sub
End Class