using System;
using System.Diagnostics;
using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace Free_SysLog.checkForUpdates
{
    static class checkForUpdatesModule
    {
        // Change these variables whenever you import this module into a program's code to handle software updates.
        public const string strMessageBoxTitleText = "Free SysLog";
        public const string strProgramName = "Free SysLog";
        // Change these variables whenever you import this module into a program's code to handle software updates.

        public static string versionString;
        public static string[] versionInfo = Application.ProductVersion.Split('.');

        static checkForUpdatesModule()
        {
            versionString = $"{versionInfo[(int)VersionPieces.major]}.{versionInfo[(int)VersionPieces.minor]} Build {versionInfo[(int)VersionPieces.build]}";
        }
    }

    class CheckForUpdatesClass
    {
        // Change these variables whenever you import this module into a program's code to handle software updates.
        private const string updaterURL = "https://www.toms-world.org/download/updater.exe";
        private const string updaterSHA256URL = "https://www.toms-world.org/download/updater.exe.sha2";
        private const string programUpdateCheckerXMLFile = "https://www.toms-world.org/updates/freesyslog_update.xml";
        private const string programCode = "freesyslog";
        // Change these variables whenever you import this module into a program's code to handle software updates.

        public Form1 windowObject;
        private readonly short shortBuild = short.Parse(checkForUpdatesModule.versionInfo[(int)VersionPieces.build].Trim());
        private readonly double versionStringWithoutBuild = double.Parse($"{checkForUpdatesModule.versionInfo[(int)VersionPieces.major]}.{checkForUpdatesModule.versionInfo[(int)VersionPieces.minor]}");
        private readonly long longInternalVersion = long.Parse(checkForUpdatesModule.versionInfo[(int)VersionPieces.revision]);

        public CheckForUpdatesClass(Form1 inputWindowObject)
        {
            windowObject = inputWindowObject;
        }

        private static bool ExtractFileFromZIPFile(ref MemoryStream memoryStream, string fileToExtract, string fileToWriteExtractedFileTo)
        {
            try
            {
                using (var zipFileObject = new System.IO.Compression.ZipArchive(memoryStream, System.IO.Compression.ZipArchiveMode.Read))
                {
                    using (var fileStream = new FileStream(fileToWriteExtractedFileTo, FileMode.Create))
                    {
                        zipFileObject.GetEntry(fileToExtract).Open().CopyTo(fileStream);
                        return true; // Extraction of file was successful, return True.
                    }
                }
                return false; // Something went wrong, return False.
            }
            catch (Exception ex)
            {
                return false; // Something went wrong, return False.
            }
        }

        public enum ProcessUpdateXMLResponse : byte
        {
            noUpdateNeeded,
            newVersion,
            newerVersionThanWebSite,
            parseError,
            exceptionError
        }

        /// <summary>This parses the XML updata data and determines if an update is needed.</summary>
        /// <param name="xmlData">The XML data from the web site.</param>
        /// <returns>A Boolean value indicating if the program has been updated or not.</returns>
        private ProcessUpdateXMLResponse ProcessUpdateXMLData(string xmlData, ref string remoteVersion, ref string remoteBuild)
        {
            try
            {
                var xmlDocument = new XmlDocument(); // First we create an XML Document Object.
                xmlDocument.Load(new StringReader(xmlData)); // Now we try and parse the XML data.
                var xmlNode = xmlDocument.SelectSingleNode("/xmlroot");

                remoteVersion = xmlNode.SelectSingleNode("version").InnerText.Trim();
                remoteBuild = xmlNode.SelectSingleNode("build").InnerText.Trim();

                long longInternalVersionFromXML = 0;
                if (xmlNode.SelectSingleNode("internalversion") is not null)
                {
                    if (long.TryParse(xmlNode.SelectSingleNode("internalversion").InnerText.Trim(), out longInternalVersionFromXML))
                    {
                        if (longInternalVersionFromXML == longInternalVersion) // If the internal version from the XML file matches the internal version from the program itself, we return a noUpdateNeeded value.
                        {
                            return ProcessUpdateXMLResponse.noUpdateNeeded;
                        }
                        else if (longInternalVersionFromXML > longInternalVersion) // If the internal version from the XML file is greater than the internal version from the program itself, we return a newVersion value.
                        {
                            return ProcessUpdateXMLResponse.newVersion;
                        }
                        else if (longInternalVersionFromXML < longInternalVersion)
                        {
                            return ProcessUpdateXMLResponse.newerVersionThanWebSite; // If the internal version from the XML file is less than the internal version from the program itself, we return a newerVersionThanWebSite value.
                        }
                    }
                    else
                    {
                        return ProcessUpdateXMLResponse.parseError;
                    } // Something went wrong, so we return a parseError value.
                }
                else
                {
                    return ProcessUpdateXMLResponse.exceptionError;
                } // Something really went wrong, so we return a exceptionError value.
            }
            catch (Exception ex)
            {
                // Something went wrong so we return an exceptionError value.
                return ProcessUpdateXMLResponse.exceptionError;
            }

            // We return a noUpdateNeeded flag.
            return ProcessUpdateXMLResponse.noUpdateNeeded;
        }

        private static bool CheckFolderPermissionsByACLs(string folderPath)
        {
            try
            {
                var directoryACLs = Directory.GetAccessControl(folderPath);
                FileSystemAccessRule directoryAccessRights;

                foreach (AuthorizationRule rule in directoryACLs.GetAccessRules(true, true, typeof(SecurityIdentifier)))
                {
                    if (rule.IdentityReference.Value.Equals(WindowsIdentity.GetCurrent().User.Value, StringComparison.OrdinalIgnoreCase))
                    {
                        directoryAccessRights = (FileSystemAccessRule)rule;

                        if (directoryAccessRights.AccessControlType == AccessControlType.Allow && directoryAccessRights.FileSystemRights == (FileSystemRights.Read | FileSystemRights.Modify | FileSystemRights.Write | FileSystemRights.FullControl))
                        {
                            return true;
                        }
                    }
                }

                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static HttpHelper CreateNewHTTPHelperObject()
        {
            var httpHelper = new HttpHelper()
            {
                SetUserAgent = CreateHTTPUserAgentHeaderString(),
                UseHTTPCompression = true,
                SetProxyMode = true
            };
            httpHelper.AddHTTPHeader("PROGRAM_NAME", checkForUpdatesModule.strProgramName);
            httpHelper.AddHTTPHeader("PROGRAM_VERSION", checkForUpdatesModule.versionString);
            httpHelper.AddHTTPHeader("OPERATING_SYSTEM", GetFullOSVersionString());

            return httpHelper;
        }

        private static string SHA256ChecksumStream(ref Stream stream)
        {
            using (var SHA256Engine = new System.Security.Cryptography.SHA256CryptoServiceProvider())
            {
                return BitConverter.ToString(SHA256Engine.ComputeHash(stream)).ToLower().Replace("-", "").Trim();
            }
        }

        private bool VerifyChecksum(string urlOfChecksumFile, ref MemoryStream memStream, ref HttpHelper httpHelper, bool boolGiveUserAnErrorMessage)
        {
            string checksumFromWeb = null;
            memStream.Position = 0;

            try
            {
                if (httpHelper.GetWebData(urlOfChecksumFile, ref checksumFromWeb))
                {
                    var regexObject = new Regex("([a-zA-Z0-9]{64})");

                    // Checks to see if we have a valid SHA256 file.
                    if (regexObject.IsMatch(checksumFromWeb))
                    {
                        // Now that we have a valid SHA256 file we need to parse out what we want.
                        checksumFromWeb = regexObject.Match(checksumFromWeb).Groups[1].Value.Trim();

                        // Now we do the actual checksum verification by passing the name of the file to the SHA256() function
                        // which calculates the checksum of the file on disk. We then compare it to the checksum from the web.
                        string localSHA256ChecksumStream(ref MemoryStream memStream) { Stream argstream = memStream; var ret = SHA256ChecksumStream(ref argstream); memStream = (MemoryStream)argstream; return ret; }

                        if (localSHA256ChecksumStream(ref memStream).Equals(checksumFromWeb, StringComparison.OrdinalIgnoreCase))
                        {
                            return true; // OK, things are good; the file passed checksum verification so we return True.
                        }
                        else
                        {
                            // The checksums don't match. Oops.
                            if (boolGiveUserAnErrorMessage)
                            {
                                windowObject.Invoke(new Action(() => Interaction.MsgBox("There was an error in the download, checksums don't match. Update process aborted.", MsgBoxStyle.Critical, checkForUpdatesModule.strMessageBoxTitleText)));
                            }

                            return false;
                        }
                    }
                    else
                    {
                        if (boolGiveUserAnErrorMessage)
                        {
                            windowObject.Invoke(new Action(() => Interaction.MsgBox("Invalid SHA2 file detected. Update process aborted.", MsgBoxStyle.Critical, checkForUpdatesModule.strMessageBoxTitleText)));
                        }

                        return false;
                    }
                }
                else
                {
                    if (boolGiveUserAnErrorMessage)
                    {
                        windowObject.Invoke(new Action(() => Interaction.MsgBox("There was an error downloading the checksum verification file. Update process aborted.", MsgBoxStyle.Critical, checkForUpdatesModule.strMessageBoxTitleText)));
                    }

                    return false;
                }
            }
            catch (Exception ex)
            {
                if (boolGiveUserAnErrorMessage)
                {
                    windowObject.Invoke(new Action(() => Interaction.MsgBox("There was an error downloading the checksum verification file. Update process aborted.", MsgBoxStyle.Critical, checkForUpdatesModule.strMessageBoxTitleText)));
                }

                return false;
            }
        }

        private void MakeLogEntry(string strLogText, bool boolSaveLogData = false)
        {
            lock (windowObject.dataGridLockObject)
            {
                windowObject.Invoke(new Action(() =>
                {
                    MyDataGridViewRow localMakeLocalDataGridRowEntry() { var argdataGrid = windowObject.Logs; var ret = SyslogParser.SyslogParser.MakeLocalDataGridRowEntry(strLogText, ref argdataGrid); windowObject.Logs = argdataGrid; return ret; }

                    windowObject.Logs.Rows.Add(localMakeLocalDataGridRowEntry());
                    windowObject.UpdateLogCount();
                    windowObject.SelectLatestLogEntry();
                    windowObject.BtnSaveLogsToDisk.Enabled = true;

                    if (boolSaveLogData)
                        windowObject.SaveLogsToDiskSub();
                }));
            }
        }

        private void DownloadAndPerformUpdate()
        {
            try
            {
                var httpHelper = CreateNewHTTPHelperObject();

                using (var memoryStream = new MemoryStream())
                {
                    if (SupportCode.SupportCode.boolDebugBuild | My.MySettingsProperty.Settings.boolDebug)
                    {
                        MakeLogEntry("Downloading updater module.");
                    }

                    bool localDownloadFile() { var argmemStream = memoryStream; var ret = httpHelper.DownloadFile(updaterURL, ref argmemStream, false); return ret; }

                    if (!localDownloadFile())
                    {
                        MakeLogEntry("There was an error while downloading required updater module.");

                        windowObject.Invoke(new Action(() => Interaction.MsgBox("There was an error while downloading required updater module.", MsgBoxStyle.Critical, checkForUpdatesModule.strMessageBoxTitleText)));
                        return;
                    }

                    bool localVerifyChecksum() { var argmemStream1 = memoryStream; var ret = VerifyChecksum(updaterSHA256URL, ref argmemStream1, ref httpHelper, true); return ret; }

                    if (!localVerifyChecksum())
                    {
                        MakeLogEntry("There was an error while downloading required updater module.");

                        windowObject.Invoke(new Action(() => Interaction.MsgBox("There was an error while downloading required updater module.", MsgBoxStyle.Critical, checkForUpdatesModule.strMessageBoxTitleText)));
                        return;
                    }

                    memoryStream.Position = 0;

                    using (var fileStream = new FileStream(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, SupportCode.SupportCode.strUpdaterEXE), FileMode.OpenOrCreate))
                    {
                        memoryStream.CopyTo(fileStream);
                    }
                }

                if (SupportCode.SupportCode.boolDebugBuild | My.MySettingsProperty.Settings.boolDebug)
                {
                    MakeLogEntry("Launching updater module.");
                }

                lock (windowObject.dataGridLockObject)
                    windowObject.SaveLogsToDiskSub();

                var startInfo = new ProcessStartInfo()
                {
                    FileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, SupportCode.SupportCode.strUpdaterEXE),
                    Arguments = $"--programcode={programCode}"
                };
                if (!CheckFolderPermissionsByACLs(AppDomain.CurrentDomain.BaseDirectory))
                    startInfo.Verb = "runas";
                Process.Start(startInfo);

                Process.GetCurrentProcess().Kill();
            }
            catch (Exception ex)
            {
                string strCrashFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Free SysLog Crash Details.log");
                if (File.Exists(strCrashFile))
                    File.Delete(strCrashFile);
                File.WriteAllText(strCrashFile, $"{ex.Message} -- {ex.StackTrace}");

                MakeLogEntry($"{ex.Message} -- {ex.StackTrace}", true);

                Interaction.MsgBox($"An error occurred while attempting to update the program. Crash data has been written to a file named \"Free SysLog Crash Details.log\".{Constants.vbCrLf}{Constants.vbCrLf}Windows Explorer will open to the file location.", MsgBoxStyle.Critical, checkForUpdatesModule.strMessageBoxTitleText);

                SupportCode.SupportCode.SelectFileInWindowsExplorer(strCrashFile);
            }
        }

        /// <summary>Creates a User Agent String for this program to be used in HTTP requests.</summary>
        /// <returns>String type.</returns>
        private static string CreateHTTPUserAgentHeaderString()
        {
            string[] versionInfo = Application.ProductVersion.Split('.');
            string versionString = $"{versionInfo[0]}.{versionInfo[1]} Build {versionInfo[2]}";
            return $"{checkForUpdatesModule.strProgramName} version {versionString} on {GetFullOSVersionString()}";
        }

        private static string GetFullOSVersionString()
        {
            try
            {
                int intOSMajorVersion = Environment.OSVersion.Version.Major;
                int intOSMinorVersion = Environment.OSVersion.Version.Minor;
                double dblDOTNETVersion = double.Parse($"{Environment.Version.Major}.{Environment.Version.Minor}");
                string strOSName;

                if (intOSMajorVersion == 5 & intOSMinorVersion == 1)
                {
                    strOSName = "Windows XP";
                }
                else if (intOSMajorVersion == 6 & intOSMinorVersion == 0)
                {
                    strOSName = "Windows Vista";
                }
                else if (intOSMajorVersion == 6 & intOSMinorVersion == 1)
                {
                    strOSName = "Windows 7";
                }
                else if (intOSMajorVersion == 6 & intOSMinorVersion == 2)
                {
                    strOSName = "Windows 8";
                }
                else if (intOSMajorVersion == 6 & intOSMinorVersion == 3)
                {
                    strOSName = "Windows 8.1";
                }
                else if (intOSMajorVersion == 10)
                {
                    strOSName = "Windows 10";
                }
                else if (intOSMajorVersion == 11)
                {
                    strOSName = "Windows 11";
                }
                else
                {
                    strOSName = $"Windows NT {intOSMajorVersion}.{intOSMinorVersion}";
                }

                return $"{strOSName} {(Environment.Is64BitOperatingSystem ? "64" : "32")}-bit (Microsoft .NET {dblDOTNETVersion})";
            }
            catch (Exception ex)
            {
                try
                {
                    return $"Unknown Windows Operating System ({Environment.OSVersion.VersionString})";
                }
                catch (Exception ex2)
                {
                    return "Unknown Windows Operating System";
                }
            }
        }

        private MsgBoxResult BackgroundThreadMessageBox(string strMsgBoxPrompt, string strMsgBoxTitle)
        {
            if (windowObject.InvokeRequired)
            {
                return (MsgBoxResult)Conversions.ToInteger(windowObject.Invoke(new Func<MsgBoxResult>(() => Interaction.MsgBox(strMsgBoxPrompt, (MsgBoxStyle)((int)MsgBoxStyle.Question + (int)MsgBoxStyle.YesNo), strMsgBoxTitle))));
            }
            else
            {
                return Interaction.MsgBox(strMsgBoxPrompt, (MsgBoxStyle)((int)MsgBoxStyle.Question + (int)MsgBoxStyle.YesNo), strMsgBoxTitle);
            }
        }

        public void CheckForUpdates(bool boolShowMessageBox = true)
        {
            windowObject.Invoke(new Action(() => windowObject.BtnCheckForUpdates.Enabled = false));

            if (!My.MyProject.Computer.Network.IsAvailable)
            {
                windowObject.Invoke(new Action(() =>
                {
                    MakeLogEntry("No Internet connection detected.");
                    Interaction.MsgBox("No Internet connection detected.", MsgBoxStyle.Information, checkForUpdatesModule.strMessageBoxTitleText);
                }));
            }
            else
            {
                try
                {
                    string xmlData = null;
                    var httpHelper = CreateNewHTTPHelperObject();

                    if (httpHelper.GetWebData(programUpdateCheckerXMLFile, ref xmlData, false))
                    {
                        string remoteVersion = null;
                        string remoteBuild = null;
                        var response = ProcessUpdateXMLData(xmlData, ref remoteVersion, ref remoteBuild);

                        if (SupportCode.SupportCode.boolDebugBuild | My.MySettingsProperty.Settings.boolDebug)
                        {
                            MakeLogEntry($"The following data was received from the URL \"{programUpdateCheckerXMLFile}\"...{Constants.vbCrLf}{SyslogParser.SyslogParser.ConvertLineFeeds(xmlData)}");
                        }

                        if (response == ProcessUpdateXMLResponse.newVersion)
                        {
                            if (SupportCode.SupportCode.boolDebugBuild | My.MySettingsProperty.Settings.boolDebug)
                            {
                                MakeLogEntry($"New version detected... {remoteVersion} Build {remoteBuild}.");
                            }

                            if (BackgroundThreadMessageBox($"An update to {checkForUpdatesModule.strProgramName} (version {remoteVersion} Build {remoteBuild}) is available to be downloaded, do you want to download and update to this new version?", checkForUpdatesModule.strMessageBoxTitleText) == MsgBoxResult.Yes)
                            {
                                if (SupportCode.SupportCode.boolDebugBuild | My.MySettingsProperty.Settings.boolDebug)
                                {
                                    MakeLogEntry("Beginning program update process.");
                                }

                                DownloadAndPerformUpdate();
                            }
                            else
                            {
                                windowObject.Invoke(new Action(() => Interaction.MsgBox("The update will not be downloaded.", MsgBoxStyle.Information, checkForUpdatesModule.strMessageBoxTitleText)));
                            }
                        }
                        else if (response == ProcessUpdateXMLResponse.noUpdateNeeded)
                        {
                            if (SupportCode.SupportCode.boolDebugBuild | My.MySettingsProperty.Settings.boolDebug)
                            {
                                MakeLogEntry("You already have the latest version, there is no need to update this program.");
                            }

                            if (boolShowMessageBox)
                            {
                                windowObject.Invoke(new Action(() => Interaction.MsgBox($"You already have the latest version, there is no need to update this program.{Constants.vbCrLf}{Constants.vbCrLf}Your current version is v{checkForUpdatesModule.versionString}.", MsgBoxStyle.Information, checkForUpdatesModule.strMessageBoxTitleText)));
                            }
                        }
                        else if (response == ProcessUpdateXMLResponse.parseError | response == ProcessUpdateXMLResponse.exceptionError)
                        {
                            if (SupportCode.SupportCode.boolDebugBuild | My.MySettingsProperty.Settings.boolDebug)
                            {
                                MakeLogEntry($"There was an error when trying to parse the response from the server. The XML data from the server is below...{Constants.vbCrLf}{Constants.vbCrLf}{xmlData}");
                            }

                            if (boolShowMessageBox)
                            {
                                windowObject.Invoke(new Action(() => Interaction.MsgBox("There was an error when trying to parse the response from the server.", MsgBoxStyle.Critical, checkForUpdatesModule.strMessageBoxTitleText)));
                            }
                        }
                        else if (response == ProcessUpdateXMLResponse.newerVersionThanWebSite)
                        {
                            if (SupportCode.SupportCode.boolDebugBuild | My.MySettingsProperty.Settings.boolDebug)
                            {
                                MakeLogEntry("This is weird, you have a version that's newer than what's listed on the web site.");
                            }

                            if (boolShowMessageBox)
                            {
                                windowObject.Invoke(new Action(() => Interaction.MsgBox("This is weird, you have a version that's newer than what's listed on the web site.", MsgBoxStyle.Information, checkForUpdatesModule.strMessageBoxTitleText)));
                            }
                        }
                    }
                    else
                    {
                        if (SupportCode.SupportCode.boolDebugBuild | My.MySettingsProperty.Settings.boolDebug)
                        {
                            MakeLogEntry("There was an error checking for updates.");
                        }

                        if (boolShowMessageBox)
                        {
                            windowObject.Invoke(new Action(() => Interaction.MsgBox("There was an error checking for updates.", MsgBoxStyle.Information, checkForUpdatesModule.strMessageBoxTitleText)));
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Ok, we crashed but who cares; but we log it.
                    MakeLogEntry($"Exception Type: {ex.GetType()}{Constants.vbCrLf}Exception Message: {ex.Message}{Constants.vbCrLf}{Constants.vbCrLf}Exception Stack Trace{Constants.vbCrLf}{ex.StackTrace}");
                }
                finally
                {
                    windowObject.Invoke(new Action(() => windowObject.BtnCheckForUpdates.Enabled = true));
                    windowObject = null;
                }
            }
        }
    }

    public enum VersionPieces : short
    {
        major = 0,
        minor = 1,
        build = 2,
        revision = 3
    }
}