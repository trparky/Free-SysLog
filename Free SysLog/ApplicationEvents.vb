﻿Imports CrashReporterDotNET
Imports Microsoft.VisualBasic.ApplicationServices
Imports Free_SysLog.SupportCode

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
            If Not Debugger.IsAttached Then
                AddHandler System.Windows.Forms.Application.ThreadException, Sub(exSender, args) SendReport(args.Exception, "I crashed!")
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
            End If

            If Not IO.Directory.Exists(strPathToDataFolder) Then IO.Directory.CreateDirectory(strPathToDataFolder)
            If Not IO.Directory.Exists(strPathToDataBackupFolder) Then IO.Directory.CreateDirectory(strPathToDataBackupFolder)

            If Not String.IsNullOrWhiteSpace(Settings.logFileLocation) Then
                If Not IO.File.Exists(strPathToDataFile) Then IO.File.Move(Settings.logFileLocation, strPathToDataFile)
                Settings.logFileLocation = Nothing
            End If

            If IO.File.Exists(strUpdaterEXE) Then
                ProcessHandling.SearchForProcessAndKillIt(strUpdaterEXE, False)
                IO.File.Delete(strUpdaterEXE)
                If IO.File.Exists(strUpdaterPDB) Then IO.File.Delete(strUpdaterPDB)
            End If

            mutex = New Threading.Mutex(True, strMutexName, boolDoWeOwnTheMutex)

            If Not boolDoWeOwnTheMutex And Not Debugger.IsAttached Then
                SendMessageToSysLogServer(strRestore, Settings.sysLogPort)
                e.Cancel = True
                Exit Sub
            End If

            uniqueObjects = LoadUniqueLogTypesAndProcesses()
        End Sub

        Private Function LoadUniqueLogTypesAndProcesses() As (HashSet(Of String), HashSet(Of String), HashSet(Of String), HashSet(Of String))
            Dim filesInDirectory As IO.FileInfo() = New IO.DirectoryInfo(strPathToDataBackupFolder).GetFiles()
            Dim collectionOfSavedData As List(Of SavedData)
            Dim uniqueLogTypes As New HashSet(Of String)
            Dim uniqueProcess As New HashSet(Of String)
            Dim uniqueHostnames As New HashSet(Of String)
            Dim uniqueIPAddresses As New HashSet(Of String)

            Threading.Tasks.Parallel.ForEach(filesInDirectory, Sub(file As IO.FileInfo)
                                                                   Using fileStream As New IO.StreamReader(file.FullName)
                                                                       collectionOfSavedData = Newtonsoft.Json.JsonConvert.DeserializeObject(Of List(Of SavedData))(fileStream.ReadToEnd.Trim, JSONDecoderSettingsForLogFiles)

                                                                       Threading.Tasks.Parallel.ForEach(collectionOfSavedData, Sub(savedData As SavedData)
                                                                                                                                   SyncLock uniqueLogTypes
                                                                                                                                       If Not String.IsNullOrWhiteSpace(savedData.logType) Then uniqueLogTypes.Add(savedData.logType)
                                                                                                                                       If Not String.IsNullOrWhiteSpace(savedData.appName) Then uniqueProcess.Add(savedData.appName)
                                                                                                                                       If Not String.IsNullOrWhiteSpace(savedData.hostname) Then uniqueHostnames.Add(savedData.hostname)
                                                                                                                                       If Not String.IsNullOrWhiteSpace(savedData.ip) Then uniqueIPAddresses.Add(savedData.ip)
                                                                                                                                   End SyncLock
                                                                                                                               End Sub)
                                                                   End Using
                                                               End Sub)

            Using fileStream As New IO.StreamReader(strPathToDataFile)
                collectionOfSavedData = Newtonsoft.Json.JsonConvert.DeserializeObject(Of List(Of SavedData))(fileStream.ReadToEnd.Trim, JSONDecoderSettingsForLogFiles)

                Threading.Tasks.Parallel.ForEach(collectionOfSavedData, Sub(savedData As SavedData)
                                                                            SyncLock uniqueLogTypes
                                                                                If Not String.IsNullOrWhiteSpace(savedData.logType) Then uniqueLogTypes.Add(savedData.logType)
                                                                                If Not String.IsNullOrWhiteSpace(savedData.appName) Then uniqueProcess.Add(savedData.appName)
                                                                                If Not String.IsNullOrWhiteSpace(savedData.hostname) Then uniqueHostnames.Add(savedData.hostname)
                                                                                If Not String.IsNullOrWhiteSpace(savedData.ip) Then uniqueIPAddresses.Add(savedData.ip)
                                                                            End SyncLock
                                                                        End Sub)
            End Using

            Return (uniqueLogTypes, uniqueProcess, uniqueHostnames, uniqueIPAddresses)
        End Function

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
