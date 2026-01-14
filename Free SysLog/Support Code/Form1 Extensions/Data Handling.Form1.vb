Imports System.IO
Imports Free_SysLog.SupportCode.SupportCode

Partial Class Form1
    Private Sub MakeLogBackup()
        WriteLogsToDisk()
        Dim strLogFileBackupFileName As String = GetUniqueFileName(Path.Combine(strPathToDataBackupFolder, $"{GetDateStringBasedOnUserPreference(Now.AddDays(-1))} Backup.json"))
        File.Copy(strPathToDataFile, strLogFileBackupFileName)
        If My.Settings.CompressBackupLogFiles Then CompressFile(strLogFileBackupFileName)
    End Sub

    Private Sub LoadDataFile(Optional boolShowDataLoadedEvent As Boolean = True)
        If File.Exists(strPathToDataFile) Then
            Try
                Invoke(Sub()
                           Logs.Rows.Add(SyslogParser.MakeLocalDataGridRowEntry("Loading data and populating data grid... Please Wait.", Logs))
                           LblLogFileSize.Text = $"Log File Size: {FileSizeToHumanSize(New FileInfo(strPathToDataFile).Length)}"
                       End Sub)

                Dim collectionOfSavedData As New List(Of SavedData)

                Using fileStream As New StreamReader(strPathToDataFile)
                    collectionOfSavedData = Newtonsoft.Json.JsonConvert.DeserializeObject(Of List(Of SavedData))(fileStream.ReadToEnd.Trim, JSONDecoderSettingsForLogFiles)
                End Using

                Dim listOfLogEntries As New List(Of MyDataGridViewRow)
                Dim stopwatch As Stopwatch = Stopwatch.StartNew

                If collectionOfSavedData.Any() Then
                    Dim intProgress As Integer = 0

                    Invoke(Sub() LoadingProgressBar.Visible = True)

                    For Each item As SavedData In collectionOfSavedData
                        listOfLogEntries.Add(item.MakeDataGridRow(Logs))
                        intProgress += 1
                        Invoke(Sub() LoadingProgressBar.Value = intProgress / collectionOfSavedData.Count * 100)
                    Next

                    Invoke(Sub() LoadingProgressBar.Visible = False)
                End If

                If boolShowDataLoadedEvent Then
                    listOfLogEntries.Add(SyslogParser.MakeLocalDataGridRowEntry($"Free SysLog Server Started. Data loaded in {MyRoundingFunction(stopwatch.Elapsed.TotalMilliseconds / 1000, 2)} seconds.", Logs))
                End If

                SyncLock dataGridLockObject
                    Invoke(Sub()
                               Logs.SuspendLayout()
                               Logs.Rows.Clear()

                               If listOfLogEntries.Count > 2000 Then
                                   Dim intBatchSize As Integer = 250

                                   Threading.Tasks.Task.Run(Sub()
                                                                For index As Integer = 0 To listOfLogEntries.Count - 1 Step intBatchSize
                                                                    Dim batch As MyDataGridViewRow() = listOfLogEntries.Skip(index).Take(intBatchSize).ToArray()
                                                                    Invoke(Sub() Logs.Rows.AddRange(batch)) ' Invoke needed for UI updates
                                                                Next

                                                                Invoke(Sub()
                                                                           SelectLatestLogEntry()

                                                                           Logs.SelectedRows(0).Selected = False
                                                                           UpdateLogCount()
                                                                           Logs.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders
                                                                           Logs.ResumeLayout()
                                                                       End Sub)
                                                            End Sub)
                               Else
                                   Logs.Rows.AddRange(listOfLogEntries.ToArray)
                                   Logs.ResumeLayout()

                                   SelectLatestLogEntry()

                                   Logs.SelectedRows(0).Selected = False
                                   UpdateLogCount()
                                   Logs.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders
                               End If
                           End Sub)
                End SyncLock
            Catch ex As Newtonsoft.Json.JsonSerializationException
                HandleLogFileLoadException(ex)
            Catch ex As Newtonsoft.Json.JsonReaderException
                HandleLogFileLoadException(ex)
            End Try
        End If
    End Sub

    Public Sub WriteLogsToDisk()
        SyncLock dataGridLockObject
            Dim collectionOfSavedData As New List(Of SavedData)
            Dim myItem As MyDataGridViewRow

            Try
                For Each item As DataGridViewRow In Logs.Rows
                    If Not String.IsNullOrWhiteSpace(item.Cells(ColumnIndex_ComputedTime).Value) Then
                        myItem = DirectCast(item, MyDataGridViewRow)

                        collectionOfSavedData.Add(New SavedData With {
                                                .time = myItem.Cells(ColumnIndex_ComputedTime).Value,
                                                .logType = myItem.Cells(ColumnIndex_LogType).Value,
                                                .ip = myItem.Cells(ColumnIndex_IPAddress).Value,
                                                .appName = myItem.Cells(ColumnIndex_RemoteProcess).Value,
                                                .log = myItem.Cells(ColumnIndex_LogText).Value,
                                                .hostname = myItem.Cells(ColumnIndex_Hostname).Value,
                                                .DateObject = myItem.DateObject,
                                                .BoolAlerted = myItem.BoolAlerted,
                                                .ServerDate = myItem.ServerDate,
                                                .rawLogData = myItem.RawLogData,
                                                .alertText = myItem.AlertText,
                                                .alertType = myItem.alertType
                                              })
                    End If
                Next

                WriteFileAtomically(strPathToDataFile, Newtonsoft.Json.JsonConvert.SerializeObject(collectionOfSavedData, Newtonsoft.Json.Formatting.Indented))

                NumberOfIgnoredLogs = longNumberOfIgnoredLogs
                WriteFileAtomically(strPathToIgnoredStatsFile, Newtonsoft.Json.JsonConvert.SerializeObject(IgnoredStats, Newtonsoft.Json.Formatting.Indented))
            Catch ex As Exception
                MsgBox("A critical error occurred while writing log data to disk. The old data had been saved to prevent data corruption and loss.", MsgBoxStyle.Critical, Text)
                Process.GetCurrentProcess.Kill()
            End Try

            LblLogFileSize.Text = $"Log File Size: {FileSizeToHumanSize(New FileInfo(strPathToDataFile).Length)}"

            BtnSaveLogsToDisk.Enabled = False
        End SyncLock
    End Sub

    Private Function GetUniqueFileName(fileName As String) As String
        Dim fileInfo As New FileInfo(fileName)

        Dim strDirectory As String = fileInfo.DirectoryName
        Dim strFileBase As String = Path.GetFileNameWithoutExtension(fileInfo.Name)
        Dim strFileExtension As String = fileInfo.Extension

        If String.IsNullOrWhiteSpace(strDirectory) Then strDirectory = Directory.GetCurrentDirectory

        Dim strNewFileName As String = Path.Combine(strDirectory, fileInfo.Name)
        Dim intCount As Integer = 1

        While File.Exists(strNewFileName)
            strNewFileName = Path.Combine(strDirectory, $"{strFileBase} ({intCount}){strFileExtension}")
            intCount += 1
        End While

        Return strNewFileName
    End Function

    Private Sub HandleLogFileLoadException(ex As Exception)
        If File.Exists($"{strPathToDataFile}.bad") Then
            File.Copy(strPathToDataFile, GetUniqueFileName($"{strPathToDataFile}.bad"))
        Else
            File.Copy(strPathToDataFile, $"{strPathToDataFile}.bad")
        End If

        WriteFileAtomically(strPathToDataFile, "[]")
        LblLogFileSize.Text = $"Log File Size: {FileSizeToHumanSize(New FileInfo(strPathToDataFile).Length)}"

        SyncLock dataGridLockObject
            Invoke(Sub()
                       Logs.SuspendLayout()
                       Logs.Rows.Clear()

                       Dim listOfLogEntries As New List(Of MyDataGridViewRow) From {
                           SyslogParser.MakeLocalDataGridRowEntry("Free SysLog Server Started.", Logs),
                           SyslogParser.MakeLocalDataGridRowEntry("There was an error while decoing the JSON data, existing data was copied to another file and the log file was reset.", Logs),
                           SyslogParser.MakeLocalDataGridRowEntry($"Exception Type: {ex.GetType}{vbCrLf}Exception Message: {ex.Message}{vbCrLf}{vbCrLf}Exception Stack Trace{vbCrLf}{ex.StackTrace}", Logs)
                       }

                       Logs.Rows.AddRange(listOfLogEntries.ToArray)
                       Logs.ResumeLayout()
                       UpdateLogCount()
                   End Sub)
        End SyncLock
    End Sub

    Private Sub ClearLogsOlderThan(daysToKeep As Integer)
        Try
            SyncLock dataGridLockObject
                Logs.AllowUserToOrderColumns = False
                Logs.Enabled = False

                Dim intOldCount As Integer = Logs.Rows.Count
                Dim newListOfLogs As New List(Of MyDataGridViewRow)
                Dim dateChosenDate As Date = Now.AddDays(daysToKeep * -1)

                For Each item As MyDataGridViewRow In Logs.Rows
                    If item.DateObject.Date >= dateChosenDate Then
                        newListOfLogs.Add(item.Clone())
                    End If
                Next

                If MsgBox("Do you want to make a backup of the logs before deleting them?", MsgBoxStyle.Question + MsgBoxStyle.YesNo + vbDefaultButton2, Text) = MsgBoxResult.Yes Then MakeLogBackup()

                Logs.Enabled = True
                Logs.AllowUserToOrderColumns = True

                Logs.SuspendLayout()
                Logs.Rows.Clear()
                Logs.Rows.AddRange(newListOfLogs.ToArray)

                Dim intCountDifference As Integer = intOldCount - Logs.Rows.Count
                Logs.Rows.Add(SyslogParser.MakeLocalDataGridRowEntry($"The user deleted {intCountDifference:N0} log {If(intCountDifference = 1, "entry", "entries")}.", Logs))

                Logs.ResumeLayout()

                SelectLatestLogEntry()
            End SyncLock

            UpdateLogCount()
            SaveLogsToDiskSub()
        Catch ex As ArgumentOutOfRangeException
            ' Handle exception if necessary
        End Try
    End Sub

    Private Function GetDateStringBasedOnUserPreference(dateObject As Date) As String
        Select Case My.Settings.DateFormat
            Case 1
                Return dateObject.ToLongDateString.Replace("/", "-").Replace("\", "-")
            Case 2
                Return dateObject.ToShortDateString.Replace("/", "-").Replace("\", "-")
            Case 3
                Return dateObject.ToString(My.Settings.CustomDateFormat).Replace("/", "-").Replace("\", "-")
            Case Else
                Return dateObject.ToLongDateString().Replace("/", "-").Replace("\", "-")
        End Select
    End Function
End Class