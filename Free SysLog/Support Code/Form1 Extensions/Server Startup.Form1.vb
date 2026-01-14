Imports System.ComponentModel
Imports System.Net.Sockets
Imports Free_SysLog.SyslogTcpServer.SyslogTcpServer
Imports Free_SysLog.SupportCode.SupportCode

Partial Class Form1
    Private Sub RunWorkerCompleted(sender As Object, e As RunWorkerCompletedEventArgs)
        If Not boolDebugBuild AndAlso My.Settings.boolCheckForUpdates Then Threading.ThreadPool.QueueUserWorkItem(Sub()
                                                                                                                      Dim checkForUpdatesClassObject As New checkForUpdates.CheckForUpdatesClass(Me)
                                                                                                                      checkForUpdatesClassObject.CheckForUpdates(False)
                                                                                                                  End Sub)

        If boolDoWeOwnTheMutex Then
            serverThread = New Threading.Thread(AddressOf SysLogThread) With {.Name = "UDP Server Thread", .Priority = Threading.ThreadPriority.Normal}
            serverThread.Start()

            If My.Settings.EnableTCPServer Then
                StartTCPServer()
                boolTCPServerRunning = True
            End If

            boolServerRunning = True
        Else
            Dim activeProcess As Process = GetProcessByPort(ProtocolType.Udp)

            If activeProcess Is Nothing Then
                MsgBox("Unable to start syslog server, perhaps another instance of this program is running on your system.", MsgBoxStyle.Critical + MsgBoxStyle.ApplicationModal, Text)
            Else
                Dim strLogText As String = $"Unable to start UDP syslog server. A process with a PID of {activeProcess.Id} already has the UDP port open."

                SyncLock dataGridLockObject
                    Logs.Rows.Add(SyslogParser.MakeLocalDataGridRowEntry(strLogText, Logs))
                End SyncLock

                MsgBox(strLogText, MsgBoxStyle.Critical + MsgBoxStyle.ApplicationModal, Text)
            End If
        End If
    End Sub

    Private Async Sub StartTCPServer()
        Dim subRoutine As New SyslogMessageHandlerDelegate(Sub(strReceivedData As String, strSourceIP As String)
                                                               SyslogParser.ProcessIncomingLog(strReceivedData, strSourceIP)
                                                           End Sub)

        SyslogTcpServer = New SyslogTcpServer.SyslogTcpServer(subRoutine, My.Settings.sysLogPort)
        Await SyslogTcpServer.StartAsync()
    End Sub
End Class