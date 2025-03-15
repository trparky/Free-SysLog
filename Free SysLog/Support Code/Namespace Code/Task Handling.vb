Imports Microsoft.Win32
Imports Microsoft.Win32.TaskScheduler
Imports System.IO
Imports Free_SysLog.SupportCode

Namespace TaskHandling
    Module TaskHandling
        Private ParentForm As Form1

        Public WriteOnly Property SetParentForm As Form1
            Set(value As Form1)
                ParentForm = value
            End Set
        End Property

        Public Function GetTaskObject(ByRef taskServiceObject As TaskService, nameOfTask As String, ByRef taskObject As Task) As Boolean
            Try
                taskObject = taskServiceObject.GetTask($"\{nameOfTask}")
                Return taskObject IsNot Nothing
            Catch ex As Exception
                Return False
            End Try
        End Function

        Public Function DoesTaskExist()
            Using taskService As New TaskService
                Dim task As Task = Nothing

                If GetTaskObject(taskService, $"Free SysLog for {Environment.UserName}", task) Then
                    If task.Definition.Settings.IdleSettings.StopOnIdleEnd Then
                        task.Definition.Settings.IdleSettings.StopOnIdleEnd = False
                        task.RegisterChanges()
                    End If

                    If task.Definition.Triggers.Any() Then
                        Dim trigger As Trigger = task.Definition.Triggers(0)

                        If trigger.TriggerType = TaskTriggerType.Logon Then
                            Dim dblSeconds As Double = DirectCast(trigger, LogonTrigger).Delay.TotalSeconds

                            If dblSeconds > 0 Then
                                ParentForm.StartUpDelay.Checked = True
                                ParentForm.StartUpDelay.Text = $"        Startup Delay ({dblSeconds} {If(dblSeconds = 1, "Second", "Seconds")})"
                            End If
                        End If
                    End If

                    If Not Debugger.IsAttached Then
                        If task.Definition.Actions.Any() Then
                            Dim action As Action = task.Definition.Actions(0)

                            If action.ActionType = TaskActionType.Execute Then
                                If Not DirectCast(action, ExecAction).Path.Replace(strQuote, "").Equals(strEXEPath, StringComparison.OrdinalIgnoreCase) Then
                                    task.Definition.Actions.Remove(action)

                                    Dim exeFileInfo As New FileInfo(strEXEPath)
                                    task.Definition.Actions.Add(New ExecAction($"{strQuote}{strEXEPath}{strQuote}", Nothing, exeFileInfo.DirectoryName))
                                    task.RegisterChanges()
                                End If
                            End If
                        Else
                            Dim exeFileInfo As New FileInfo(strEXEPath)
                            task.Definition.Actions.Add(New ExecAction($"{strQuote}{strEXEPath}{strQuote}", Nothing, exeFileInfo.DirectoryName))
                            task.RegisterChanges()
                        End If
                    End If

                    ParentForm.StartUpDelay.Enabled = True
                    Return True
                End If
            End Using

            Return False
        End Function

        Public Sub CreateTask()
            Using taskService As New TaskService
                Dim newTask As TaskDefinition = taskService.NewTask

                With newTask
                    .RegistrationInfo.Description = "Runs Free SysLog at User Logon"

                    .Triggers.Add(New LogonTrigger With {
                        .Delay = New TimeSpan(0, 1, 0),
                        .UserId = Environment.UserName
                    })

                    .Actions.Add(New ExecAction($"{strQuote}{strEXEPath}{strQuote}", Nothing, New FileInfo(strEXEPath).DirectoryName))
                    .Settings.Compatibility = TaskCompatibility.V2
                    .Settings.AllowDemandStart = True
                    .Settings.DisallowStartIfOnBatteries = False
                    .Settings.RunOnlyIfIdle = False
                    .Settings.StopIfGoingOnBatteries = False
                    .Settings.AllowHardTerminate = False
                    .Settings.UseUnifiedSchedulingEngine = True
                    .Settings.ExecutionTimeLimit = Nothing
                    .Settings.Priority = ProcessPriorityClass.Normal
                    .Settings.Compatibility = TaskCompatibility.V2_3
                    .Settings.IdleSettings.StopOnIdleEnd = False
                    .Principal.LogonType = TaskLogonType.InteractiveToken
                End With

                taskService.RootFolder.RegisterTaskDefinition($"Free SysLog for {Environment.UserName}", newTask)

                newTask.Dispose()
            End Using
        End Sub

        Public Sub ConvertRegistryRunCommandToTask()
            Dim boolDoesRegistryStartupKeyExist As Boolean = False

            Using registryKey As RegistryKey = Registry.CurrentUser.OpenSubKey("Software\Microsoft\Windows\CurrentVersion\Run", False)
                If registryKey.GetValue("Free Syslog") IsNot Nothing Then
                    boolDoesRegistryStartupKeyExist = True
                    CreateTask()
                End If
            End Using

            If boolDoesRegistryStartupKeyExist Then
                Using registryKey As RegistryKey = Registry.CurrentUser.OpenSubKey("Software\Microsoft\Windows\CurrentVersion\Run", True)
                    registryKey.DeleteValue("Free Syslog")
                End Using
            End If
        End Sub
    End Module
End Namespace