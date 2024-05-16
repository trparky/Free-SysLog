Public Class ConfigureSysLogMirrorServers
    Public boolSuccess As Boolean = False

    Private Sub ConfigureSysLogMirrorServers_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If My.Settings.ServersToSendTo IsNot Nothing AndAlso My.Settings.ServersToSendTo.Count > 0 Then
            Dim listViewItem As ListViewItem
            Dim SysLogProxyServer As SysLogProxyServer

            For Each strJSONString As String In My.Settings.ServersToSendTo
                SysLogProxyServer = Newtonsoft.Json.JsonConvert.DeserializeObject(Of SysLogProxyServer)(strJSONString)

                listViewItem = New ListViewItem(SysLogProxyServer.ip)
                listViewItem.SubItems.Add(SysLogProxyServer.port)
                servers.Items.Add(listViewItem)

                SysLogProxyServer = Nothing
                listViewItem = Nothing
            Next
        End If
    End Sub

    Private Sub Servers_SelectedIndexChanged(sender As Object, e As EventArgs) Handles servers.SelectedIndexChanged
        If servers.SelectedItems.Count > 0 Then
            BtnDeleteServer.Enabled = True
            BtnEditServer.Enabled = True
        Else
            BtnDeleteServer.Enabled = False
            BtnEditServer.Enabled = False
        End If
    End Sub

    Private Sub EditItem()
        Using AddSysLogMirrorServer As New AddSysLogMirrorServer With {.StartPosition = FormStartPosition.CenterParent, .Icon = Icon, .boolEditMode = True}
            Dim selectedItemObject As ListViewItem = servers.SelectedItems(0)

            With AddSysLogMirrorServer
                .strIP = selectedItemObject.SubItems(0).Text
                .intPort = selectedItemObject.SubItems(1).Text
            End With

            AddSysLogMirrorServer.ShowDialog(Me)

            If AddSysLogMirrorServer.boolSuccess Then
                With selectedItemObject
                    .SubItems(0).Text = AddSysLogMirrorServer.strIP
                    .SubItems(1).Text = AddSysLogMirrorServer.intPort
                End With
            End If
        End Using
    End Sub

    Private Sub BtnAddServer_Click(sender As Object, e As EventArgs) Handles BtnAddServer.Click
        Using AddSysLogMirrorServer As New AddSysLogMirrorServer With {.StartPosition = FormStartPosition.CenterParent, .Icon = Icon}
            AddSysLogMirrorServer.ShowDialog(Me)

            If AddSysLogMirrorServer.boolSuccess Then
                Dim ListViewItem As New ListViewItem(AddSysLogMirrorServer.strIP)
                ListViewItem.SubItems.Add(AddSysLogMirrorServer.intPort.ToString)
                servers.Items.Add(ListViewItem)
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

        For Each item As ListViewItem In servers.Items
            SysLogProxyServer = New SysLogProxyServer() With {.ip = item.SubItems(0).Text, .port = Integer.Parse(item.SubItems(1).Text)}
            serversList.Add(SysLogProxyServer)
            tempServer.Add(Newtonsoft.Json.JsonConvert.SerializeObject(SysLogProxyServer))
        Next

        My.Settings.ServersToSendTo = tempServer
        My.Settings.Save()
    End Sub
End Class