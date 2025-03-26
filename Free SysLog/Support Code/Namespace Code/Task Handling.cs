using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Microsoft.Win32;
using Microsoft.Win32.TaskScheduler;

namespace Free_SysLog.TaskHandling
{
    static class TaskHandling
    {
        private static Form1 ParentForm;

        public static Form1 SetParentForm
        {
            set
            {
                ParentForm = value;
            }
        }

        public static bool GetTaskObject(ref TaskService taskServiceObject, string nameOfTask, ref Task taskObject)
        {
            try
            {
                taskObject = taskServiceObject.GetTask($@"\{nameOfTask}");
                return taskObject is not null;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static object DoesTaskExist()
        {
            using (var taskService = new TaskService())
            {
                Task task = null;

                var argtaskServiceObject = taskService;
                if (GetTaskObject(ref argtaskServiceObject, $"Free SysLog for {Environment.UserName}", ref task))
                {
                    if (task.Definition.Settings.IdleSettings.StopOnIdleEnd)
                    {
                        task.Definition.Settings.IdleSettings.StopOnIdleEnd = false;
                        task.RegisterChanges();
                    }

                    if (task.Definition.Triggers.Any())
                    {
                        var trigger = task.Definition.Triggers[0];

                        if (trigger.TriggerType == TaskTriggerType.Logon)
                        {
                            double dblSeconds = ((LogonTrigger)trigger).Delay.TotalSeconds;

                            if (dblSeconds > 0d)
                            {
                                ParentForm.StartUpDelay.Checked = true;
                                ParentForm.StartUpDelay.Text = $"        Startup Delay ({dblSeconds} {(dblSeconds == 1d ? "Second" : "Seconds")})";
                            }
                        }
                    }

                    if (!Debugger.IsAttached)
                    {
                        if (task.Definition.Actions.Any())
                        {
                            var action = task.Definition.Actions[0];

                            if (action.ActionType == TaskActionType.Execute)
                            {
                                if (!((ExecAction)action).Path.Replace(SupportCode.SupportCode.strQuote, "").Equals(SupportCode.SupportCode.strEXEPath, StringComparison.OrdinalIgnoreCase))
                                {
                                    task.Definition.Actions.Remove(action);

                                    var exeFileInfo = new FileInfo(SupportCode.SupportCode.strEXEPath);
                                    task.Definition.Actions.Add(new ExecAction($"{SupportCode.SupportCode.strQuote}{SupportCode.SupportCode.strEXEPath}{SupportCode.SupportCode.strQuote}", null, exeFileInfo.DirectoryName));
                                    task.RegisterChanges();
                                }
                            }
                        }
                        else
                        {
                            var exeFileInfo = new FileInfo(SupportCode.SupportCode.strEXEPath);
                            task.Definition.Actions.Add(new ExecAction($"{SupportCode.SupportCode.strQuote}{SupportCode.SupportCode.strEXEPath}{SupportCode.SupportCode.strQuote}", null, exeFileInfo.DirectoryName));
                            task.RegisterChanges();
                        }
                    }

                    ParentForm.StartUpDelay.Enabled = true;
                    return true;
                }
            }

            return false;
        }

        public static void CreateTask()
        {
            using (var taskService = new TaskService())
            {
                var newTask = taskService.NewTask();

                newTask.RegistrationInfo.Description = "Runs Free SysLog at User Logon";

                newTask.Triggers.Add(new LogonTrigger()
                {
                    Delay = new TimeSpan(0, 1, 0),
                    UserId = Environment.UserName
                });

                newTask.Actions.Add(new ExecAction($"{SupportCode.SupportCode.strQuote}{SupportCode.SupportCode.strEXEPath}{SupportCode.SupportCode.strQuote}", null, new FileInfo(SupportCode.SupportCode.strEXEPath).DirectoryName));
                newTask.Settings.Compatibility = TaskCompatibility.V2;
                newTask.Settings.AllowDemandStart = true;
                newTask.Settings.DisallowStartIfOnBatteries = false;
                newTask.Settings.RunOnlyIfIdle = false;
                newTask.Settings.StopIfGoingOnBatteries = false;
                newTask.Settings.AllowHardTerminate = false;
                newTask.Settings.UseUnifiedSchedulingEngine = true;
                newTask.Settings.ExecutionTimeLimit = default;
                newTask.Settings.Priority = ProcessPriorityClass.Normal;
                newTask.Settings.Compatibility = TaskCompatibility.V2_3;
                newTask.Settings.IdleSettings.StopOnIdleEnd = false;
                newTask.Principal.LogonType = TaskLogonType.InteractiveToken;

                taskService.RootFolder.RegisterTaskDefinition($"Free SysLog for {Environment.UserName}", newTask);

                newTask.Dispose();
            }
        }

        public static void ConvertRegistryRunCommandToTask()
        {
            bool boolDoesRegistryStartupKeyExist = false;

            using (var registryKey = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", false))
            {
                if (registryKey.GetValue("Free Syslog") is not null)
                {
                    boolDoesRegistryStartupKeyExist = true;
                    CreateTask();
                }
            }

            if (boolDoesRegistryStartupKeyExist)
            {
                using (var registryKey = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true))
                {
                    registryKey.DeleteValue("Free Syslog");
                }
            }
        }
    }
}