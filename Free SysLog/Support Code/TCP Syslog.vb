﻿Imports System.Net
Imports System.Net.Sockets
Imports System.Text
Imports System.Threading.Tasks

Public Class SyslogTcpServer
    Implements IDisposable
    Private ReadOnly _port As Integer
    Private ReadOnly _syslogMessageHandler As [Delegate]
    Private TCPListener As TcpListener
    Private boolLoopControl As Boolean = True

    Public Sub New(syslogMessageHandler As [Delegate], Optional port As Integer = 514)
        _port = port
        _syslogMessageHandler = syslogMessageHandler
    End Sub

    Public Sub Dispose() Implements IDisposable.Dispose
        If TCPListener IsNot Nothing Then
            TCPListener.Stop()
            TCPListener = Nothing
        End If
    End Sub

    Public Async Function StartAsync() As Task
        TCPListener = New TcpListener(IPAddress.Any, _port)
        TCPListener.Start()

        While boolLoopControl
            Dim tcpClient As TcpClient = Await TCPListener.AcceptTcpClientAsync()
            Await Task.Run(Function() HandleClientAsync(tcpClient))
        End While
    End Function

    Private Async Function HandleClientAsync(tcpClient As TcpClient) As Task
        Using tcpClient
            Dim remoteIPEndPoint As IPEndPoint = DirectCast(tcpClient.Client.RemoteEndPoint, IPEndPoint)

            Using stream As NetworkStream = tcpClient.GetStream()
                Dim dataBuffer(4095) As Byte
                Dim intBytesRead As Integer
                Dim strMessage As String

                While intBytesRead = Await stream.ReadAsync(dataBuffer, 0, dataBuffer.Length) <> 0
                    strMessage = Encoding.UTF8.GetString(dataBuffer, 0, intBytesRead).Trim()

                    If strMessage.Equals("terminate", StringComparison.OrdinalIgnoreCase) Then boolLoopControl = False

                    _syslogMessageHandler.DynamicInvoke(strMessage, remoteIPEndPoint.Address.ToString)
                End While
            End Using
        End Using
    End Function
End Class