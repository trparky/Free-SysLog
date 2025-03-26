using System;
using System.Diagnostics;
using System.Windows.Forms;
using CrashReporterDotNET;
using Microsoft.VisualBasic.ApplicationServices;

namespace Free_SysLog.My
{
    // The following events are available for MyApplication:
    // Startup: Raised when the application starts, before the startup form is created.
    // Shutdown: Raised after all application forms are closed.  This event is not raised if the application terminates abnormally.
    // UnhandledException: Raised if the application encounters an unhandled exception.
    // StartupNextInstance: Raised when launching a single-instance application and the application is already active. 
    // NetworkAvailabilityChanged: Raised when the network connection is connected or disconnected.
    internal partial class MyApplication
    {
        private ReportCrash _reportCrash;

        private void MyApplication_Startup(object sender, StartupEventArgs e)
        {
            if (!Debugger.IsAttached)
            {
                Application.ThreadException += (exSender, args) => SendReport(args.Exception, "I crashed!");
                AppDomain.CurrentDomain.UnhandledException += (exSender, args) => SendReport((Exception)args.ExceptionObject, "I crashed!");

                _reportCrash = new ReportCrash("5v22h1sh@anonaddy.me")
                {
                    Silent = true,
                    ShowScreenshotTab = true,
                    IncludeScreenshot = false,
                    AnalyzeWithDoctorDump = true,
                    DoctorDumpSettings = new DoctorDumpSettings()
                    {
                        ApplicationID = new Guid("72ab07a3-16e5-4362-b661-b686561b2731"),
                        OpenReportInBrowser = true
                    }
                };

                _reportCrash.RetryFailedReports();
            }

            if (!System.IO.Directory.Exists(SupportCode.SupportCode.strPathToDataFolder))
                System.IO.Directory.CreateDirectory(SupportCode.SupportCode.strPathToDataFolder);
            if (!System.IO.Directory.Exists(SupportCode.SupportCode.strPathToDataBackupFolder))
                System.IO.Directory.CreateDirectory(SupportCode.SupportCode.strPathToDataBackupFolder);

            if (!string.IsNullOrWhiteSpace(MySettingsProperty.Settings.logFileLocation))
            {
                if (!System.IO.File.Exists(SupportCode.SupportCode.strPathToDataFile))
                    System.IO.File.Move(MySettingsProperty.Settings.logFileLocation, SupportCode.SupportCode.strPathToDataFile);
                MySettingsProperty.Settings.logFileLocation = null;
            }

            if (System.IO.File.Exists(SupportCode.SupportCode.strUpdaterEXE))
            {
                ProcessHandling.ProcessHandling.SearchForProcessAndKillIt(SupportCode.SupportCode.strUpdaterEXE, false);
                System.IO.File.Delete(SupportCode.SupportCode.strUpdaterEXE);
                if (System.IO.File.Exists(SupportCode.SupportCode.strUpdaterPDB))
                    System.IO.File.Delete(SupportCode.SupportCode.strUpdaterPDB);
            }

            SupportCode.SupportCode.mutex = new System.Threading.Mutex(true, SupportCode.SupportCode.strMutexName, out SupportCode.SupportCode.boolDoWeOwnTheMutex);

            if (!SupportCode.SupportCode.boolDoWeOwnTheMutex & !Debugger.IsAttached)
            {
                SupportCode.SupportCode.SendMessageToSysLogServer(SupportCode.SupportCode.strRestore, MySettingsProperty.Settings.sysLogPort);
                e.Cancel = true;
                return;
            }
        }

        public void SendReport(Exception exception, string developerMessage = "")
        {
            _reportCrash.DeveloperMessage = developerMessage;
            _reportCrash.Silent = false;
            _reportCrash.Send(exception);
        }

        public void SendReportSilently(Exception exception, string developerMessage = "")
        {
            _reportCrash.DeveloperMessage = developerMessage;
            _reportCrash.Silent = true;
            _reportCrash.Send(exception);
        }
    }
}