Imports System.Net
Imports System.Net.Sockets
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Threading.Tasks
Imports Free_SysLog.SupportCode

Namespace SyslogTcpServer
    Public Class SyslogTcpServer
        Private ReadOnly _port As Integer
        Private ReadOnly _syslogMessageHandler As SyslogMessageHandlerDelegate
        Private TCPListener As TcpListener
        Private boolLoopControl As Boolean = True

        ' Strongly typed delegate
        Public Delegate Sub SyslogMessageHandlerDelegate(strReceivedData As String, strSourceIP As String)

        Public Sub New(syslogMessageHandler As SyslogMessageHandlerDelegate, Optional port As Integer = 514)
            _port = port
            _syslogMessageHandler = syslogMessageHandler
        End Sub

        Public Async Function StartAsync() As Task
            Try
                ' These are initialized as IPv4 mode.
                Dim ipAddressSetting As IPAddress = IPAddress.Any

                If My.Settings.IPv6Support Then ipAddressSetting = IPAddress.IPv6Any

                TCPListener = New TcpListener(ipAddressSetting, _port)
                If My.Settings.IPv6Support Then TCPListener.Server.DualMode = True
                TCPListener.Start()

                While boolLoopControl
                    Dim tcpClient As TcpClient = Await TCPListener.AcceptTcpClientAsync()
                    Await HandleClientAsync(tcpClient)
                End While
            Catch ex As Exception
                Dim processIPv4 As Process = GetProcessByTcpPort(My.Settings.sysLogPort, AddressFamily.InterNetwork)
                Dim processIPv6 As Process = GetProcessByTcpPort(My.Settings.sysLogPort, AddressFamily.InterNetworkV6)
                Dim activeProcess As Process = Nothing

                If processIPv4 IsNot Nothing Then
                    activeProcess = processIPv4
                ElseIf processIPv6 IsNot Nothing Then
                    activeProcess = processIPv6
                End If

                If activeProcess Is Nothing Then
                    _syslogMessageHandler($"Exception Type: {ex.GetType}{vbCrLf}Exception Message: {ex.Message}{vbCrLf}{vbCrLf}Exception Stack Trace{vbCrLf}{ex.StackTrace}", IPAddress.Loopback.ToString)
                Else
                    Dim strLogText As String = $"Unable to start syslog server. A process with a PID of {activeProcess.Id} already has the port open."
                    _syslogMessageHandler($"Exception Type: {ex.GetType}{vbCrLf}Exception Message: {ex.Message}{vbCrLf}{vbCrLf}Exception Stack Trace{vbCrLf}{ex.StackTrace}{vbCrLf}{vbCrLf}{strLogText}", IPAddress.Loopback.ToString)
                End If
            End Try
        End Function

        Private Async Function HandleClientAsync(tcpClient As TcpClient) As Task
            Using tcpClient
                Dim remoteIPEndPoint As IPEndPoint = DirectCast(tcpClient.Client.RemoteEndPoint, IPEndPoint)

                Using stream As NetworkStream = tcpClient.GetStream()
                    Dim dataBuffer(4095) As Byte
                    Dim intBytesRead As Integer
                    Dim strMessage As String

                    Try
                        Do
                            intBytesRead = Await stream.ReadAsync(dataBuffer, 0, dataBuffer.Length)

                            If intBytesRead <> 0 Then
                                strMessage = Encoding.UTF8.GetString(dataBuffer, 0, intBytesRead).Trim()

                                If strMessage.Equals(strTerminate, StringComparison.OrdinalIgnoreCase) Then
                                    TCPListener.Stop()
                                    boolLoopControl = False
                                    Exit Do
                                Else
                                    _syslogMessageHandler(strMessage, IPAddress.Loopback.ToString)
                                End If
                            End If
                        Loop While intBytesRead <> 0
                    Catch ex As Exception
                    End Try
                End Using
            End Using
        End Function
    End Class
End Namespace