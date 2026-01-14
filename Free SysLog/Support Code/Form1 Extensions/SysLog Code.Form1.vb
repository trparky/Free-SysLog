Imports System.Net
Imports System.Net.Sockets
Imports System.Text
Imports Free_SysLog.SupportCode

Partial Class Form1
    Sub SysLogThread()
        Try
            ' These are initialized as IPv4 mode.
            Dim addressFamilySetting As AddressFamily = AddressFamily.InterNetwork
            Dim ipAddressSetting As IPAddress = IPAddress.Any

            If My.Settings.IPv6Support Then
                addressFamilySetting = AddressFamily.InterNetworkV6
                ipAddressSetting = IPAddress.IPv6Any
            End If

            Using socket As New Socket(addressFamilySetting, SocketType.Dgram, ProtocolType.Udp)
                If My.Settings.IPv6Support Then socket.DualMode = True
                socket.Bind(New IPEndPoint(ipAddressSetting, My.Settings.sysLogPort))

                Dim boolDoServerLoop As Boolean = True
                Dim buffer(4095) As Byte
                Dim remoteEndPoint As EndPoint = New IPEndPoint(ipAddressSetting, 0)
                Dim ProxiedSysLogData As ProxiedSysLogData
                Dim bytesReceived As Integer
                Dim strReceivedData, strSourceIP As String

                While boolDoServerLoop
                    bytesReceived = socket.ReceiveFrom(buffer, remoteEndPoint)
                    strReceivedData = Encoding.UTF8.GetString(buffer, 0, bytesReceived)
                    strSourceIP = GetIPv4Address(CType(remoteEndPoint, IPEndPoint).Address).ToString()

                    If strReceivedData.Trim.Equals(strRestore, StringComparison.OrdinalIgnoreCase) Then
                        Invoke(Sub() RestoreWindowAfterReceivingRestoreCommand())
                    ElseIf strReceivedData.Trim.Equals(strTerminate, StringComparison.OrdinalIgnoreCase) Then
                        boolDoServerLoop = False
                    ElseIf strReceivedData.Trim.StartsWith(strProxiedString, StringComparison.OrdinalIgnoreCase) Then
                        Try
                            strReceivedData = strReceivedData.Replace(strProxiedString, "", StringComparison.OrdinalIgnoreCase)
                            ProxiedSysLogData = Newtonsoft.Json.JsonConvert.DeserializeObject(Of ProxiedSysLogData)(strReceivedData, JSONDecoderSettingsForLogFiles)
                            SyslogParser.ProcessIncomingLog(ProxiedSysLogData.log, ProxiedSysLogData.ip)
                        Catch ex As Newtonsoft.Json.JsonSerializationException
                        End Try
                    Else
                        If serversList IsNot Nothing AndAlso serversList.GetSnapshot.Any() Then
                            Threading.ThreadPool.QueueUserWorkItem(Sub()
                                                                       ProxiedSysLogData = New ProxiedSysLogData() With {.ip = strSourceIP, .log = strReceivedData}
                                                                       Dim strDataToSend As String = strProxiedString & Newtonsoft.Json.JsonConvert.SerializeObject(ProxiedSysLogData)

                                                                       For Each item As SysLogProxyServer In serversList.GetSnapshot
                                                                           SendMessageToSysLogServer(strDataToSend, item.ip, item.port)
                                                                       Next

                                                                       ProxiedSysLogData = Nothing
                                                                       strDataToSend = Nothing
                                                                   End Sub)
                        End If

                        SyslogParser.ProcessIncomingLog(strReceivedData, strSourceIP)
                    End If

                    strReceivedData = Nothing
                    strSourceIP = Nothing
                    bytesReceived = Nothing
                End While
            End Using
        Catch ex As Threading.ThreadAbortException
            ' Does nothing
        Catch e As Exception
            Invoke(Sub()
                       Dim activeProcess As Process = GetProcessByPort(ProtocolType.Udp)

                       If activeProcess Is Nothing Then
                           MsgBox("Unable to start syslog server, perhaps another instance of this program is running on your system.", MsgBoxStyle.Critical + MsgBoxStyle.ApplicationModal, Text)
                       Else
                           Dim strLogText As String = $"Unable to start UDP syslog server. A process with a PID of {activeProcess.Id} already has the UDP port open."

                           SyncLock dataGridLockObject
                               Logs.Rows.Add(SyslogParser.MakeLocalDataGridRowEntry($"Exception Type: {e.GetType}{vbCrLf}Exception Message: {e.Message}{vbCrLf}{vbCrLf}Exception Stack Trace{vbCrLf}{e.StackTrace}", Logs))
                               Logs.Rows.Add(SyslogParser.MakeLocalDataGridRowEntry(strLogText, Logs))
                               SelectLatestLogEntry()
                               UpdateLogCount()
                           End SyncLock

                           MsgBox(strLogText, MsgBoxStyle.Critical + MsgBoxStyle.ApplicationModal, Text)
                       End If
                   End Sub)
        End Try
    End Sub

    Private Async Sub RestoreWindowAfterReceivingRestoreCommand()
        If boolMaximizedBeforeMinimize Then
            WindowState = FormWindowState.Maximized
        ElseIf My.Settings.boolMaximized Then
            WindowState = FormWindowState.Maximized
        Else
            WindowState = FormWindowState.Normal
        End If

        Await Threading.Tasks.Task.Delay(100)

        If boolDebugBuild Or ChkDebug.Checked Then
            Logs.Rows.Add(SyslogParser.MakeLocalDataGridRowEntry("Restore command received.", Logs))
            SelectLatestLogEntry()
            UpdateLogCount()
            BtnSaveLogsToDisk.Enabled = True
        End If

        SelectLatestLogEntry()
    End Sub
End Class