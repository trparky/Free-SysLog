Imports System.IO
Imports System.Text
Imports Free_SysLog.SupportCode

Namespace DataHandling
    Public Module DataHandling
        Public Sub ExportSelectedLogs(selectedRows As DataGridViewSelectedRowCollection)
            SyncLock ParentForm.dataGridLockObject
                Dim saveFileDialog As New SaveFileDialog With {.Title = "Export Data...", .Filter = "CSV (Comma Separated Value)|*.csv|JSON File|*.json|XML File|*.xml"}

                If saveFileDialog.ShowDialog() = DialogResult.OK Then
                    Dim fileInfo As New FileInfo(saveFileDialog.FileName)

                    Dim collectionOfSavedData As New List(Of SavedData)
                    Dim myItem As MyDataGridViewRow
                    Dim csvStringBuilder As New StringBuilder
                    Dim strLogType, strTime, strSourceIP, strHeader, strLogText, strAlerted, strHostname, strRemoteProcess, strServerTime, strRawLog, strAlertText As String

                    If fileInfo.Extension.Equals(".csv", StringComparison.OrdinalIgnoreCase) Then csvStringBuilder.AppendLine("Time,Server Time,Log Type,IP Address,Hostname,Remote Process,Log Text,Alerted,Raw Log Text,Alert Text")

                    For Each item As DataGridViewRow In selectedRows
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

                    If fileInfo.Extension.Equals(".xml", StringComparison.OrdinalIgnoreCase) Then
                        Using memoryStream As New MemoryStream
                            Dim xmlSerializerObject As New Xml.Serialization.XmlSerializer(collectionOfSavedData.GetType)
                            xmlSerializerObject.Serialize(memoryStream, collectionOfSavedData)
                            WriteFileAtomically(saveFileDialog.FileName, memoryStream.ToArray())
                        End Using
                    ElseIf fileInfo.Extension.Equals(".json", StringComparison.OrdinalIgnoreCase) Then
                        WriteFileAtomically(saveFileDialog.FileName, Newtonsoft.Json.JsonConvert.SerializeObject(collectionOfSavedData, Newtonsoft.Json.Formatting.Indented))
                    ElseIf fileInfo.Extension.Equals(".csv", StringComparison.OrdinalIgnoreCase) Then
                        WriteFileAtomically(saveFileDialog.FileName, csvStringBuilder.ToString.Trim)
                    End If

                    If My.Settings.AskOpenExplorer Then
                        Using OpenExplorer As New OpenExplorer(saveFileDialog.FileName)
                            OpenExplorer.StartPosition = FormStartPosition.CenterParent
                            OpenExplorer.MyParentForm = ParentForm

                            Dim result As DialogResult = OpenExplorer.ShowDialog(ParentForm)

                            If result = DialogResult.No Then
                                Exit Sub
                            ElseIf result = DialogResult.Yes Then
                                SelectFileInWindowsExplorer(saveFileDialog.FileName)
                            End If
                        End Using
                    Else
                        MsgBox("Data exported successfully.", MsgBoxStyle.Information, ParentForm.Text)
                    End If
                End If
            End SyncLock
        End Sub

        Public Sub ExportAllLogs(rows As DataGridViewRowCollection)
            SyncLock ParentForm.dataGridLockObject
                Dim saveFileDialog As New SaveFileDialog With {.Title = "Export Data...", .Filter = "CSV (Comma Separated Value)|*.csv|JSON File|*.json|XML File|*.xml"}

                If saveFileDialog.ShowDialog() = DialogResult.OK Then
                    Dim fileInfo As New FileInfo(saveFileDialog.FileName)

                    Dim collectionOfSavedData As New List(Of SavedData)
                    Dim myItem As MyDataGridViewRow
                    Dim csvStringBuilder As New StringBuilder
                    Dim strLogType, strTime, strSourceIP, strHeader, strLogText, strAlerted, strHostname, strRemoteProcess, strServerTime, strRawLog, strAlertText As String

                    If fileInfo.Extension.Equals(".csv", StringComparison.OrdinalIgnoreCase) Then csvStringBuilder.AppendLine("Time,Server Time,Log Type,IP Address,Hostname,Remote Process,Log Text,Alerted,Raw Log Text,Alert Text")

                    For Each item As DataGridViewRow In rows
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

                    If fileInfo.Extension.Equals(".xml", StringComparison.OrdinalIgnoreCase) Then
                        Using memoryStream As New MemoryStream
                            Dim xmlSerializerObject As New Xml.Serialization.XmlSerializer(collectionOfSavedData.GetType)
                            xmlSerializerObject.Serialize(memoryStream, collectionOfSavedData)
                            WriteFileAtomically(saveFileDialog.FileName, memoryStream.ToArray())
                        End Using
                    ElseIf fileInfo.Extension.Equals(".json", StringComparison.OrdinalIgnoreCase) Then
                        WriteFileAtomically(saveFileDialog.FileName, Newtonsoft.Json.JsonConvert.SerializeObject(collectionOfSavedData, Newtonsoft.Json.Formatting.Indented))
                    ElseIf fileInfo.Extension.Equals(".csv", StringComparison.OrdinalIgnoreCase) Then
                        WriteFileAtomically(saveFileDialog.FileName, csvStringBuilder.ToString.Trim)
                    End If

                    If My.Settings.AskOpenExplorer Then
                        Using OpenExplorer As New OpenExplorer(saveFileDialog.FileName)
                            OpenExplorer.StartPosition = FormStartPosition.CenterParent
                            OpenExplorer.MyParentForm = ParentForm

                            Dim result As DialogResult = OpenExplorer.ShowDialog(ParentForm)

                            If result = DialogResult.No Then
                                Exit Sub
                            ElseIf result = DialogResult.Yes Then
                                SelectFileInWindowsExplorer(saveFileDialog.FileName)
                            End If
                        End Using
                    Else
                        MsgBox("Data exported successfully.", MsgBoxStyle.Information, ParentForm.Text)
                    End If
                End If
            End SyncLock
        End Sub
    End Module
End Namespace