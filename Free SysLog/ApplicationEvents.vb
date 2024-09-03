Imports CrashReporterDotNET
Imports Microsoft.VisualBasic.ApplicationServices

Namespace My
    ' The following events are available for MyApplication:
    ' Startup: Raised when the application starts, before the startup form is created.
    ' Shutdown: Raised after all application forms are closed.  This event is not raised if the application terminates abnormally.
    ' UnhandledException: Raised if the application encounters an unhandled exception.
    ' StartupNextInstance: Raised when launching a single-instance application and the application is already active. 
    ' NetworkAvailabilityChanged: Raised when the network connection is connected or disconnected.
    Partial Friend Class MyApplication
        Private _reportCrash As ReportCrash

        Private Sub MyApplication_Startup(sender As Object, e As StartupEventArgs) Handles Me.Startup
            AddHandler Windows.Forms.Application.ThreadException, Sub(exSender, args) SendReport(args.Exception, "I crashed!")
            AddHandler AppDomain.CurrentDomain.UnhandledException, Sub(exSender, args)
                                                                       SendReport(DirectCast(args.ExceptionObject, Exception), "I crashed!")
                                                                   End Sub

            _reportCrash = New ReportCrash("5v22h1sh@anonaddy.me") With {
                .Silent = True,
                .ShowScreenshotTab = True,
                .IncludeScreenshot = False,
                .AnalyzeWithDoctorDump = True,
                .DoctorDumpSettings = New DoctorDumpSettings With {
                    .ApplicationID = New Guid("72ab07a3-16e5-4362-b661-b686561b2731"),
                    .OpenReportInBrowser = True
                }
            }

            _reportCrash.RetryFailedReports()

            If Not IO.Directory.Exists(strPathToDataFolder) Then IO.Directory.CreateDirectory(strPathToDataFolder)
            If Not IO.Directory.Exists(strPathToDataBackupFolder) Then IO.Directory.CreateDirectory(strPathToDataBackupFolder)

            If Not String.IsNullOrWhiteSpace(Settings.logFileLocation) Then
                If Not IO.File.Exists(strPathToDataFile) Then IO.File.Move(Settings.logFileLocation, strPathToDataFile)
                Settings.logFileLocation = Nothing
            End If

            If IO.File.Exists("updater.exe") Then
                SearchForProcessAndKillIt("updater.exe", False)
                IO.File.Delete("updater.exe")
                If IO.File.Exists("updater.pdb") Then IO.File.Delete("updater.pdb")
            End If

            mutex = New Threading.Mutex(True, strMutexName, boolDoWeOwnTheMutex)

            If Not boolDoWeOwnTheMutex And Not Debugger.IsAttached Then
                SendMessageToSysLogServer("restore", Settings.sysLogPort)
                e.Cancel = True
                Exit Sub
            End If
        End Sub

        Public Sub SendReport(exception As Exception, Optional developerMessage As String = "")
            _reportCrash.DeveloperMessage = developerMessage
            _reportCrash.Silent = False
            _reportCrash.Send(exception)
        End Sub

        Public Sub SendReportSilently(exception As Exception, Optional developerMessage As String = "")
            _reportCrash.DeveloperMessage = developerMessage
            _reportCrash.Silent = True
            _reportCrash.Send(exception)
        End Sub
    End Class
End Namespace
