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
            If Not IO.Directory.Exists(strPathToDataFolder) Then IO.Directory.CreateDirectory(strPathToDataFolder)
            If Not IO.Directory.Exists(strPathToDataBackupFolder) Then IO.Directory.CreateDirectory(strPathToDataBackupFolder)

            If Not String.IsNullOrWhiteSpace(Settings.logFileLocation) Then
                If Not IO.File.Exists(strPathToDataFile) Then IO.File.Move(Settings.logFileLocation, strPathToDataFile)
                Settings.logFileLocation = Nothing
            End If

            If IO.File.Exists("updater.exe") Then
                SearchForProcessAndKillIt("updater.exe", False)
                IO.File.Delete("updater.exe")
            End If

            mutex = New Threading.Mutex(True, strMutexName, boolDoWeOwnTheMutex)

            If Not boolDoWeOwnTheMutex And Not Debugger.IsAttached Then
                SendMessageToSysLogServer("restore", Settings.sysLogPort)
                e.Cancel = True
                Exit Sub
            End If

            If My.Application.CommandLineArgs.Count > 0 AndAlso My.Application.CommandLineArgs(0).Trim.Equals("/background", StringComparison.OrdinalIgnoreCase) Then
                Threading.Thread.Sleep(TimeSpan.FromSeconds(30).TotalMilliseconds)
            End If
        End Sub
    End Class
End Namespace
