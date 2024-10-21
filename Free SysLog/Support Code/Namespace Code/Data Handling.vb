Imports System.IO
Imports System.Text
Imports Free_SysLog.SupportCode

Namespace DataHandling
    Public Module DataHandling
        Private ParentForm As Form1

        Public WriteOnly Property SetParentForm As Form1
            Set(value As Form1)
                ParentForm = value
            End Set
        End Property

        Public Sub ExportSelectedLogs()
            SyncLock ParentForm.dataGridLockObject
                Dim saveFileDialog As New SaveFileDialog With {.Title = "Export Data...", .Filter = "CSV (Comma Separated Value)|*.csv|JSON File|*.json|XML File|*.xml"}

                If saveFileDialog.ShowDialog() = DialogResult.OK Then
                    Dim fileInfo As New FileInfo(saveFileDialog.FileName)

                    Dim collectionOfSavedData As New List(Of SavedData)
                    Dim myItem As MyDataGridViewRow
                    Dim csvStringBuilder As New StringBuilder
                    Dim strLogType, strTime, strSourceIP, strHeader, strLogText, strAlerted, strHostname, strRemoteProcess, strServerTime, strRawLog, strAlertText As String

                    If fileInfo.Extension.Equals(".csv", StringComparison.OrdinalIgnoreCase) Then csvStringBuilder.AppendLine("Time,Server Time,Log Type,IP Address,Hostname,Remote Process,Log Text,Alerted,Raw Log Text,Alert Text")

                    For Each item As DataGridViewRow In ParentForm.Logs.SelectedRows
                        If Not String.IsNullOrWhiteSpace(item.Cells(ColumnIndex_ComputedTime).Value) Then
                            myItem = DirectCast(item, MyDataGridViewRow)

                            If fileInfo.Extension.Equals(".csv", StringComparison.OrdinalIgnoreCase) Then
                                strTime = SanitizeForCSV(myItem.Cells(ColumnIndex_ComputedTime).Value)
                                strLogType = SanitizeForCSV(myItem.Cells(ColumnIndex_LogType).Value)
                                strSourceIP = SanitizeForCSV(myItem.Cells(ColumnIndex_IPAddress).Value)
                                strHeader = SanitizeForCSV(myItem.Cells(ColumnIndex_RemoteProcess).Value)
                                strLogText = SanitizeForCSV(myItem.Cells(ColumnIndex_LogText).Value)
                                strHostname = SanitizeForCSV(myItem.Cells(ColumnIndex_Hostname).Value)
                                strRemoteProcess = SanitizeForCSV(myItem.Cells(ColumnIndex_RemoteProcess).Value)
                                strServerTime = SanitizeForCSV(myItem.ServerDate)
                                strRawLog = SanitizeForCSV(myItem.RawLogData)
                                strAlertText = If(myItem.BoolAlerted, myItem.AlertText, "")
                                strAlerted = If(myItem.BoolAlerted, "Yes", "No")

                                csvStringBuilder.AppendLine($"{strTime},{strServerTime},{strLogType},{strSourceIP},{strHostname},{strRemoteProcess},{strLogText},{strAlerted},{strRawLog},{strAlertText}")
                            Else
                                collectionOfSavedData.Add(New SavedData With {
                                                        .time = myItem.Cells(ColumnIndex_ComputedTime).Value,
                                                        .ServerDate = myItem.ServerDate,
                                                        .logType = myItem.Cells(ColumnIndex_LogType).Value,
                                                        .ip = myItem.Cells(ColumnIndex_IPAddress).Value,
                                                        .hostname = myItem.Cells(ColumnIndex_Hostname).Value,
                                                        .appName = myItem.Cells(ColumnIndex_RemoteProcess).Value,
                                                        .log = myItem.Cells(ColumnIndex_LogText).Value,
                                                        .DateObject = myItem.DateObject,
                                                        .BoolAlerted = myItem.BoolAlerted,
                                                        .rawLogData = myItem.RawLogData,
                                                        .alertText = myItem.AlertText
                                                      })
                            End If
                        End If
                    Next

                    Using fileStream As New StreamWriter(saveFileDialog.FileName)
                        If fileInfo.Extension.Equals(".xml", StringComparison.OrdinalIgnoreCase) Then
                            Dim xmlSerializerObject As New Xml.Serialization.XmlSerializer(collectionOfSavedData.GetType)
                            xmlSerializerObject.Serialize(fileStream, collectionOfSavedData)
                        ElseIf fileInfo.Extension.Equals(".json", StringComparison.OrdinalIgnoreCase) Then
                            fileStream.Write(Newtonsoft.Json.JsonConvert.SerializeObject(collectionOfSavedData, Newtonsoft.Json.Formatting.Indented))
                        ElseIf fileInfo.Extension.Equals(".csv", StringComparison.OrdinalIgnoreCase) Then
                            fileStream.Write(csvStringBuilder.ToString.Trim)
                        End If
                    End Using

                    If MsgBox($"Data exported to ""{saveFileDialog.FileName}"" successfully.{vbCrLf}{vbCrLf}Do you want to open Windows Explorer to the location of the file?", MsgBoxStyle.Question + MsgBoxStyle.YesNo + vbDefaultButton2, ParentForm.Text) = MsgBoxResult.Yes Then
                        SelectFileInWindowsExplorer(saveFileDialog.FileName)
                    End If
                End If
            End SyncLock
        End Sub

        Public Sub ExportAllLogs()
            SyncLock ParentForm.dataGridLockObject
                Dim saveFileDialog As New SaveFileDialog With {.Title = "Export Data...", .Filter = "CSV (Comma Separated Value)|*.csv|JSON File|*.json|XML File|*.xml"}

                If saveFileDialog.ShowDialog() = DialogResult.OK Then
                    Dim fileInfo As New FileInfo(saveFileDialog.FileName)

                    Dim collectionOfSavedData As New List(Of SavedData)
                    Dim myItem As MyDataGridViewRow
                    Dim csvStringBuilder As New StringBuilder
                    Dim strLogType, strTime, strSourceIP, strHeader, strLogText, strAlerted, strHostname, strRemoteProcess, strServerTime, strRawLog, strAlertText As String

                    If fileInfo.Extension.Equals(".csv", StringComparison.OrdinalIgnoreCase) Then csvStringBuilder.AppendLine("Time,Server Time,Log Type,IP Address,Hostname,Remote Process,Log Text,Alerted,Raw Log Text,Alert Text")

                    For Each item As DataGridViewRow In ParentForm.Logs.Rows
                        If Not String.IsNullOrWhiteSpace(item.Cells(ColumnIndex_ComputedTime).Value) Then
                            myItem = DirectCast(item, MyDataGridViewRow)

                            If fileInfo.Extension.Equals(".csv", StringComparison.OrdinalIgnoreCase) Then
                                strTime = SanitizeForCSV(myItem.Cells(ColumnIndex_ComputedTime).Value)
                                strLogType = SanitizeForCSV(myItem.Cells(ColumnIndex_LogType).Value)
                                strSourceIP = SanitizeForCSV(myItem.Cells(ColumnIndex_IPAddress).Value)
                                strHeader = SanitizeForCSV(myItem.Cells(ColumnIndex_RemoteProcess).Value)
                                strLogText = SanitizeForCSV(myItem.Cells(ColumnIndex_LogText).Value)
                                strHostname = SanitizeForCSV(myItem.Cells(ColumnIndex_Hostname).Value)
                                strRemoteProcess = SanitizeForCSV(myItem.Cells(ColumnIndex_RemoteProcess).Value)
                                strServerTime = SanitizeForCSV(myItem.ServerDate)
                                strRawLog = SanitizeForCSV(myItem.RawLogData)
                                strAlertText = If(myItem.BoolAlerted, myItem.AlertText, "")
                                strAlerted = If(myItem.BoolAlerted, "Yes", "No")

                                csvStringBuilder.AppendLine($"{strTime},{strServerTime},{strLogType},{strSourceIP},{strHostname},{strRemoteProcess},{strLogText},{strAlerted},{strRawLog},{strAlertText}")
                            Else
                                collectionOfSavedData.Add(New SavedData With {
                                                       .time = myItem.Cells(ColumnIndex_ComputedTime).Value,
                                                       .ServerDate = myItem.ServerDate,
                                                       .logType = myItem.Cells(ColumnIndex_LogType).Value,
                                                       .ip = myItem.Cells(ColumnIndex_IPAddress).Value,
                                                       .hostname = myItem.Cells(ColumnIndex_Hostname).Value,
                                                       .appName = myItem.Cells(ColumnIndex_RemoteProcess).Value,
                                                       .log = myItem.Cells(ColumnIndex_LogText).Value,
                                                       .DateObject = myItem.DateObject,
                                                       .BoolAlerted = myItem.BoolAlerted,
                                                       .rawLogData = myItem.RawLogData,
                                                       .alertText = myItem.AlertText
                                                      })
                            End If
                        End If
                    Next

                    Using fileStream As New StreamWriter(saveFileDialog.FileName)
                        If fileInfo.Extension.Equals(".xml", StringComparison.OrdinalIgnoreCase) Then
                            Dim xmlSerializerObject As New Xml.Serialization.XmlSerializer(collectionOfSavedData.GetType)
                            xmlSerializerObject.Serialize(fileStream, collectionOfSavedData)
                        ElseIf fileInfo.Extension.Equals(".json", StringComparison.OrdinalIgnoreCase) Then
                            fileStream.Write(Newtonsoft.Json.JsonConvert.SerializeObject(collectionOfSavedData, Newtonsoft.Json.Formatting.Indented))
                        ElseIf fileInfo.Extension.Equals(".csv", StringComparison.OrdinalIgnoreCase) Then
                            fileStream.Write(csvStringBuilder.ToString.Trim)
                        End If
                    End Using

                    If MsgBox($"Data exported to ""{saveFileDialog.FileName}"" successfully.{vbCrLf}{vbCrLf}Do you want to open Windows Explorer to the location of the file?", MsgBoxStyle.Question + MsgBoxStyle.YesNo + vbDefaultButton2, ParentForm.Text) = MsgBoxResult.Yes Then
                        SelectFileInWindowsExplorer(saveFileDialog.FileName)
                    End If
                End If
            End SyncLock
        End Sub

        Public Sub WriteLogsToDisk()
            SyncLock ParentForm.lockObject
                Dim collectionOfSavedData As New List(Of SavedData)
                Dim myItem As MyDataGridViewRow

                SyncLock ParentForm.dataGridLockObject
                    For Each item As DataGridViewRow In ParentForm.Logs.Rows
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
                                                .alertText = myItem.AlertText
                                              })
                        End If
                    Next
                End SyncLock

                Try
                    Using fileStream As New StreamWriter(strPathToDataFile & ".new")
                        fileStream.Write(Newtonsoft.Json.JsonConvert.SerializeObject(collectionOfSavedData, Newtonsoft.Json.Formatting.Indented))
                    End Using

                    File.Delete(strPathToDataFile)
                    File.Move(strPathToDataFile & ".new", strPathToDataFile)
                Catch ex As Exception
                    MsgBox("A critical error occurred while writing log data to disk. The old data had been saved to prevent data corruption.", MsgBoxStyle.Critical, ParentForm.Text)
                    Process.GetCurrentProcess.Kill()
                End Try

                ParentForm.LblLogFileSize.Text = $"Log File Size: {FileSizeToHumanSize(New FileInfo(strPathToDataFile).Length)}"

                ParentForm.BtnSaveLogsToDisk.Enabled = False
            End SyncLock
        End Sub
    End Module
End Namespace