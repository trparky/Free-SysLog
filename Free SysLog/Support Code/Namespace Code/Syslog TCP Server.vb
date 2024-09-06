Imports System.Net
Imports System.Net.Sockets
Imports System.Text
Imports System.Threading.Tasks

Namespace SyslogTcpServer
    Public Class SyslogTcpServer
        Private ReadOnly _port As Integer
        Private ReadOnly _syslogMessageHandler As [Delegate]
        Private TCPListener As TcpListener
        Private boolLoopControl As Boolean = True

        Public Sub New(syslogMessageHandler As [Delegate], Optional port As Integer = 514)
            _port = port
            _syslogMessageHandler = syslogMessageHandler
        End Sub

        Public Async Function StartAsync() As Task
            Try
                TCPListener = New TcpListener(IPAddress.IPv6Any, _port)
                TCPListener.Server.DualMode = True
                TCPListener.Start()
                Console.WriteLine($"Syslog TCP server listening on port {_port}.")

                While boolLoopControl
                    Dim tcpClient As TcpClient = Await TCPListener.AcceptTcpClientAsync()
                    Await HandleClientAsync(tcpClient)
                End While
            Catch ex As Exception
                _syslogMessageHandler.DynamicInvoke($"Exception Type: {ex.GetType}{vbCrLf}Exception Message: {ex.Message}{vbCrLf}{vbCrLf}Exception Stack Trace{vbCrLf}{ex.StackTrace}", IPAddress.Loopback.ToString)
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

                                If strMessage.Equals("terminate", StringComparison.OrdinalIgnoreCase) Then
                                    boolLoopControl = False
                                    TCPListener.Stop()
                                Else
                                    _syslogMessageHandler.DynamicInvoke(strMessage, GetIPv4Address(remoteIPEndPoint.Address).ToString)
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