Imports Free_SysLog.SupportCode.SupportCode

Partial Class Form1
    Private Sub LoadCheckboxSettings()
        If My.Settings.NotificationLength = 0 Then
            NotificationLengthShort.Checked = True
            NotificationLengthLong.Checked = False
        Else
            NotificationLengthShort.Checked = False
            NotificationLengthLong.Checked = True
        End If

        ColLog.AutoSizeMode = If(My.Settings.colLogAutoFill, DataGridViewAutoSizeColumnMode.Fill, DataGridViewAutoSizeColumnMode.NotSet)

        OnlySaveAlertedLogs.Checked = My.Settings.OnlySaveAlertedLogs
        SaveIgnoredLogCount.Checked = My.Settings.saveIgnoredLogCount
        AskToOpenExplorerWhenSavingData.Checked = My.Settings.AskOpenExplorer
        ColLogsAutoFill.Checked = My.Settings.colLogAutoFill
        IncludeButtonsOnNotifications.Checked = My.Settings.IncludeButtonsOnNotifications
        AutomaticallyCheckForUpdates.Checked = My.Settings.boolCheckForUpdates
        ChkDeselectItemAfterMinimizingWindow.Checked = My.Settings.boolDeselectItemsWhenMinimizing
        ChkEnableRecordingOfIgnoredLogs.Checked = My.Settings.recordIgnoredLogs
        LimitNumberOfIgnoredLogs.Visible = My.Settings.recordIgnoredLogs
        IgnoredLogsToolStripMenuItem.Visible = ChkEnableRecordingOfIgnoredLogs.Checked
        ZerooutIgnoredLogsCounterToolStripMenuItem.Visible = Not ChkEnableRecordingOfIgnoredLogs.Checked
        ChkEnableAutoScroll.Checked = My.Settings.autoScroll
        ChkDisableAutoScrollUponScrolling.Enabled = ChkEnableAutoScroll.Checked
        ChkEnableAutoSave.Checked = My.Settings.autoSave
        ChkEnableConfirmCloseToolStripItem.Checked = My.Settings.boolConfirmClose
        LblAutoSaved.Visible = ChkEnableAutoSave.Checked
        ColAlerts.Visible = My.Settings.boolShowAlertedColumn
        MinimizeToClockTray.Checked = My.Settings.MinimizeToClockTray
        StopServerStripMenuItem.Visible = boolDoWeOwnTheMutex
        ChkEnableStartAtUserStartup.Checked = TaskHandling.DoesTaskExist()
        DeleteOldLogsAtMidnight.Checked = My.Settings.DeleteOldLogsAtMidnight
        BackupOldLogsAfterClearingAtMidnight.Enabled = My.Settings.DeleteOldLogsAtMidnight
        BackupOldLogsAfterClearingAtMidnight.Checked = My.Settings.BackupOldLogsAfterClearingAtMidnight
        ViewLogBackups.Visible = BackupOldLogsAfterClearingAtMidnight.Checked
        ChkEnableTCPSyslogServer.Checked = My.Settings.EnableTCPServer
        ColHostname.Visible = My.Settings.boolShowHostnameColumn
        colServerTime.Visible = My.Settings.boolShowServerTimeColumn
        colLogType.Visible = My.Settings.boolShowLogTypeColumn
        RemoveNumbersFromRemoteApp.Checked = My.Settings.RemoveNumbersFromRemoteApp
        IPv6Support.Checked = My.Settings.IPv6Support
        ChkDisableAutoScrollUponScrolling.Checked = My.Settings.disableAutoScrollUponScrolling
        ChkDebug.Checked = My.Settings.boolDebug
        ConfirmDelete.Checked = My.Settings.ConfirmDelete
        ProcessReplacementsInSyslogDataFirst.Checked = My.Settings.ProcessReplacementsInSyslogDataFirst
        ShowCloseButtonOnNotifications.Checked = My.Settings.ShowCloseButtonOnNotifications
        IncludeCommasInDHMS.Checked = My.Settings.IncludeCommasInDHMS
        CompressBackupLogFilesToolStripMenuItem.Checked = My.Settings.CompressBackupLogFiles
    End Sub

    Private Sub LoadAndDeserializeArrays()
        Dim tempReplacementsClass As ReplacementsClass
        Dim tempSysLogProxyServer As SysLogProxyServer
        Dim tempIgnoredClass As IgnoredClass
        Dim tempAlertsClass As AlertsClass

        If My.Settings.replacements IsNot Nothing AndAlso My.Settings.replacements.Count > 0 Then
            For Each strJSONString As String In My.Settings.replacements
                tempReplacementsClass = Newtonsoft.Json.JsonConvert.DeserializeObject(Of ReplacementsClass)(strJSONString, JSONDecoderSettingsForSettingsFiles)
                If tempReplacementsClass.BoolEnabled Then replacementsList.Add(tempReplacementsClass)
                tempReplacementsClass = Nothing
            Next
        End If

        If My.Settings.hostnames IsNot Nothing AndAlso My.Settings.hostnames.Count > 0 Then
            Dim customHostname As CustomHostname

            For Each strJSONString As String In My.Settings.hostnames
                customHostname = Newtonsoft.Json.JsonConvert.DeserializeObject(Of CustomHostname)(strJSONString, JSONDecoderSettingsForSettingsFiles)
                SupportCode.hostnames(customHostname.ip) = customHostname.deviceName
            Next
        End If

        If My.Settings.ServersToSendTo IsNot Nothing AndAlso My.Settings.ServersToSendTo.Count > 0 Then
            For Each strJSONString As String In My.Settings.ServersToSendTo
                tempSysLogProxyServer = Newtonsoft.Json.JsonConvert.DeserializeObject(Of SysLogProxyServer)(strJSONString, JSONDecoderSettingsForSettingsFiles)
                If tempSysLogProxyServer.boolEnabled Then serversList.Add(tempSysLogProxyServer)
                tempSysLogProxyServer = Nothing
            Next
        End If

        If My.Settings.ignored2 Is Nothing Then
            My.Settings.ignored2 = New Specialized.StringCollection()

            If My.Settings.ignored IsNot Nothing AndAlso My.Settings.ignored.Count > 0 Then
                For Each strIgnoredString As String In My.Settings.ignored
                    My.Settings.ignored2.Add(Newtonsoft.Json.JsonConvert.SerializeObject(New IgnoredClass With {.BoolCaseSensitive = False, .BoolRegex = False, .StrIgnore = strIgnoredString}))
                Next

                My.Settings.ignored.Clear()
                My.Settings.Save()
            End If
        End If

        If My.Settings.ignored2 IsNot Nothing AndAlso My.Settings.ignored2.Count > 0 Then
            For Each strJSONString As String In My.Settings.ignored2
                tempIgnoredClass = Newtonsoft.Json.JsonConvert.DeserializeObject(Of IgnoredClass)(strJSONString, JSONDecoderSettingsForSettingsFiles)
                If tempIgnoredClass.BoolEnabled Then ignoredList.Add(tempIgnoredClass)
                tempIgnoredClass = Nothing
            Next
        End If

        If My.Settings.alerts IsNot Nothing AndAlso My.Settings.alerts.Count > 0 Then
            For Each strJSONString As String In My.Settings.alerts
                tempAlertsClass = Newtonsoft.Json.JsonConvert.DeserializeObject(Of AlertsClass)(strJSONString, JSONDecoderSettingsForSettingsFiles)
                If tempAlertsClass.BoolEnabled Then alertsList.Add(tempAlertsClass)
                tempAlertsClass = Nothing
            Next
        End If
    End Sub
End Class