Imports System.Net.Sockets
Imports System.Text
Imports Microsoft.VisualBasic.ApplicationServices

Namespace My
    ' The following events are available for MyApplication:
    ' Startup: Raised when the application starts, before the startup form is created.
    ' Shutdown: Raised after all application forms are closed.  This event is not raised if the application terminates abnormally.
    ' UnhandledException: Raised if the application encounters an unhandled exception.
    ' StartupNextInstance: Raised when launching a single-instance application and the application is already active. 
    ' NetworkAvailabilityChanged: Raised when the network connection is connected or disconnected.
    Partial Friend Class MyApplication
        Private Sub MyApplication_Startup(sender As Object, e As StartupEventArgs) Handles Me.Startup
            mutex = New Threading.Mutex(False, strMutexName, False)

            If Not mutex.WaitOne(0, False) And Not Debugger.IsAttached Then
                SendMessageToSysLogServer("restore", Settings.sysLogPort)
                e.Cancel = True
                Exit Sub
            End If
        End Sub

        Private Sub SendMessageToSysLogServer(strMessage As String, intPort As Integer)
            Using udpClient As New UdpClient()
                udpClient.Connect("127.0.0.1", intPort)
                Dim data As Byte() = Encoding.UTF8.GetBytes(strMessage)
                udpClient.Send(data, data.Length)
            End Using
        End Sub
    End Class
End Namespace
