using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace Free_SysLog.DataHandling
{
    public static class DataHandling
    {
        private static Form1 ParentForm;

        public static Form1 SetParentForm
        {
            set
            {
                ParentForm = value;
            }
        }

        public static void ExportSelectedLogs(DataGridViewSelectedRowCollection selectedRows)
        {
            lock (ParentForm.dataGridLockObject)
            {
                var saveFileDialog = new SaveFileDialog() { Title = "Export Data...", Filter = "CSV (Comma Separated Value)|*.csv|JSON File|*.json|XML File|*.xml" };

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    var fileInfo = new FileInfo(saveFileDialog.FileName);

                    var collectionOfSavedData = new List<SavedData>();
                    MyDataGridViewRow myItem;
                    var csvStringBuilder = new StringBuilder();
                    string strLogType, strTime, strSourceIP, strHeader, strLogText, strAlerted, strHostname, strRemoteProcess, strServerTime, strRawLog, strAlertText;

                    if (fileInfo.Extension.Equals(".csv", StringComparison.OrdinalIgnoreCase))
                        csvStringBuilder.AppendLine("Time,Server Time,Log Type,IP Address,Hostname,Remote Process,Log Text,Alerted,Raw Log Text,Alert Text");

                    foreach (DataGridViewRow item in selectedRows)
                    {
                        if (!string.IsNullOrWhiteSpace(Conversions.ToString(item.Cells[SupportCode.SupportCode.ColumnIndex_ComputedTime].Value)))
                        {
                            myItem = (MyDataGridViewRow)item;

                            if (fileInfo.Extension.Equals(".csv", StringComparison.OrdinalIgnoreCase))
                            {
                                strTime = SupportCode.SupportCode.SanitizeForCSV(Conversions.ToString(myItem.Cells[SupportCode.SupportCode.ColumnIndex_ComputedTime].Value));
                                strLogType = SupportCode.SupportCode.SanitizeForCSV(Conversions.ToString(myItem.Cells[SupportCode.SupportCode.ColumnIndex_LogType].Value));
                                strSourceIP = SupportCode.SupportCode.SanitizeForCSV(Conversions.ToString(myItem.Cells[SupportCode.SupportCode.ColumnIndex_IPAddress].Value));
                                strHeader = SupportCode.SupportCode.SanitizeForCSV(Conversions.ToString(myItem.Cells[SupportCode.SupportCode.ColumnIndex_RemoteProcess].Value));
                                strLogText = SupportCode.SupportCode.SanitizeForCSV(Conversions.ToString(myItem.Cells[SupportCode.SupportCode.ColumnIndex_LogText].Value));
                                strHostname = SupportCode.SupportCode.SanitizeForCSV(Conversions.ToString(myItem.Cells[SupportCode.SupportCode.ColumnIndex_Hostname].Value));
                                strRemoteProcess = SupportCode.SupportCode.SanitizeForCSV(Conversions.ToString(myItem.Cells[SupportCode.SupportCode.ColumnIndex_RemoteProcess].Value));
                                strServerTime = SupportCode.SupportCode.SanitizeForCSV(Conversions.ToString(myItem.ServerDate));
                                strRawLog = SupportCode.SupportCode.SanitizeForCSV(myItem.RawLogData);
                                strAlertText = myItem.BoolAlerted ? myItem.AlertText : "";
                                strAlerted = myItem.BoolAlerted ? "Yes" : "No";

                                csvStringBuilder.AppendLine($"{strTime},{strServerTime},{strLogType},{strSourceIP},{strHostname},{strRemoteProcess},{strLogText},{strAlerted},{strRawLog},{strAlertText}");
                            }
                            else
                            {
                                collectionOfSavedData.Add(new SavedData()
                                {
                                    time = Conversions.ToString(myItem.Cells[SupportCode.SupportCode.ColumnIndex_ComputedTime].Value),
                                    ServerDate = myItem.ServerDate,
                                    logType = Conversions.ToString(myItem.Cells[SupportCode.SupportCode.ColumnIndex_LogType].Value),
                                    ip = Conversions.ToString(myItem.Cells[SupportCode.SupportCode.ColumnIndex_IPAddress].Value),
                                    hostname = Conversions.ToString(myItem.Cells[SupportCode.SupportCode.ColumnIndex_Hostname].Value),
                                    appName = Conversions.ToString(myItem.Cells[SupportCode.SupportCode.ColumnIndex_RemoteProcess].Value),
                                    log = Conversions.ToString(myItem.Cells[SupportCode.SupportCode.ColumnIndex_LogText].Value),
                                    DateObject = myItem.DateObject,
                                    BoolAlerted = myItem.BoolAlerted,
                                    rawLogData = myItem.RawLogData,
                                    alertText = myItem.AlertText
                                });
                            }
                        }
                    }

                    using (var fileStream = new StreamWriter(saveFileDialog.FileName))
                    {
                        if (fileInfo.Extension.Equals(".xml", StringComparison.OrdinalIgnoreCase))
                        {
                            var xmlSerializerObject = new System.Xml.Serialization.XmlSerializer(collectionOfSavedData.GetType());
                            xmlSerializerObject.Serialize(fileStream, collectionOfSavedData);
                        }
                        else if (fileInfo.Extension.Equals(".json", StringComparison.OrdinalIgnoreCase))
                        {
                            fileStream.Write(Newtonsoft.Json.JsonConvert.SerializeObject(collectionOfSavedData, Newtonsoft.Json.Formatting.Indented));
                        }
                        else if (fileInfo.Extension.Equals(".csv", StringComparison.OrdinalIgnoreCase))
                        {
                            fileStream.Write(csvStringBuilder.ToString().Trim());
                        }
                    }

                    if (Interaction.MsgBox($"Data exported to \"{saveFileDialog.FileName}\" successfully.{Constants.vbCrLf}{Constants.vbCrLf}Do you want to open Windows Explorer to the location of the file?", (MsgBoxStyle)((int)MsgBoxStyle.Question + (int)MsgBoxStyle.YesNo + (int)Constants.vbDefaultButton2), ParentForm.Text) == MsgBoxResult.Yes)
                    {
                        SupportCode.SupportCode.SelectFileInWindowsExplorer(saveFileDialog.FileName);
                    }
                }
            }
        }

        public static void ExportAllLogs(DataGridViewRowCollection rows)
        {
            lock (ParentForm.dataGridLockObject)
            {
                var saveFileDialog = new SaveFileDialog() { Title = "Export Data...", Filter = "CSV (Comma Separated Value)|*.csv|JSON File|*.json|XML File|*.xml" };

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    var fileInfo = new FileInfo(saveFileDialog.FileName);

                    var collectionOfSavedData = new List<SavedData>();
                    MyDataGridViewRow myItem;
                    var csvStringBuilder = new StringBuilder();
                    string strLogType, strTime, strSourceIP, strHeader, strLogText, strAlerted, strHostname, strRemoteProcess, strServerTime, strRawLog, strAlertText;

                    if (fileInfo.Extension.Equals(".csv", StringComparison.OrdinalIgnoreCase))
                        csvStringBuilder.AppendLine("Time,Server Time,Log Type,IP Address,Hostname,Remote Process,Log Text,Alerted,Raw Log Text,Alert Text");

                    foreach (DataGridViewRow item in rows)
                    {
                        if (!string.IsNullOrWhiteSpace(Conversions.ToString(item.Cells[SupportCode.SupportCode.ColumnIndex_ComputedTime].Value)))
                        {
                            myItem = (MyDataGridViewRow)item;

                            if (fileInfo.Extension.Equals(".csv", StringComparison.OrdinalIgnoreCase))
                            {
                                strTime = SupportCode.SupportCode.SanitizeForCSV(Conversions.ToString(myItem.Cells[SupportCode.SupportCode.ColumnIndex_ComputedTime].Value));
                                strLogType = SupportCode.SupportCode.SanitizeForCSV(Conversions.ToString(myItem.Cells[SupportCode.SupportCode.ColumnIndex_LogType].Value));
                                strSourceIP = SupportCode.SupportCode.SanitizeForCSV(Conversions.ToString(myItem.Cells[SupportCode.SupportCode.ColumnIndex_IPAddress].Value));
                                strHeader = SupportCode.SupportCode.SanitizeForCSV(Conversions.ToString(myItem.Cells[SupportCode.SupportCode.ColumnIndex_RemoteProcess].Value));
                                strLogText = SupportCode.SupportCode.SanitizeForCSV(Conversions.ToString(myItem.Cells[SupportCode.SupportCode.ColumnIndex_LogText].Value));
                                strHostname = SupportCode.SupportCode.SanitizeForCSV(Conversions.ToString(myItem.Cells[SupportCode.SupportCode.ColumnIndex_Hostname].Value));
                                strRemoteProcess = SupportCode.SupportCode.SanitizeForCSV(Conversions.ToString(myItem.Cells[SupportCode.SupportCode.ColumnIndex_RemoteProcess].Value));
                                strServerTime = SupportCode.SupportCode.SanitizeForCSV(Conversions.ToString(myItem.ServerDate));
                                strRawLog = SupportCode.SupportCode.SanitizeForCSV(myItem.RawLogData);
                                strAlertText = myItem.BoolAlerted ? myItem.AlertText : "";
                                strAlerted = myItem.BoolAlerted ? "Yes" : "No";

                                csvStringBuilder.AppendLine($"{strTime},{strServerTime},{strLogType},{strSourceIP},{strHostname},{strRemoteProcess},{strLogText},{strAlerted},{strRawLog},{strAlertText}");
                            }
                            else
                            {
                                collectionOfSavedData.Add(new SavedData()
                                {
                                    time = Conversions.ToString(myItem.Cells[SupportCode.SupportCode.ColumnIndex_ComputedTime].Value),
                                    ServerDate = myItem.ServerDate,
                                    logType = Conversions.ToString(myItem.Cells[SupportCode.SupportCode.ColumnIndex_LogType].Value),
                                    ip = Conversions.ToString(myItem.Cells[SupportCode.SupportCode.ColumnIndex_IPAddress].Value),
                                    hostname = Conversions.ToString(myItem.Cells[SupportCode.SupportCode.ColumnIndex_Hostname].Value),
                                    appName = Conversions.ToString(myItem.Cells[SupportCode.SupportCode.ColumnIndex_RemoteProcess].Value),
                                    log = Conversions.ToString(myItem.Cells[SupportCode.SupportCode.ColumnIndex_LogText].Value),
                                    DateObject = myItem.DateObject,
                                    BoolAlerted = myItem.BoolAlerted,
                                    rawLogData = myItem.RawLogData,
                                    alertText = myItem.AlertText
                                });
                            }
                        }
                    }

                    using (var fileStream = new StreamWriter(saveFileDialog.FileName))
                    {
                        if (fileInfo.Extension.Equals(".xml", StringComparison.OrdinalIgnoreCase))
                        {
                            var xmlSerializerObject = new System.Xml.Serialization.XmlSerializer(collectionOfSavedData.GetType());
                            xmlSerializerObject.Serialize(fileStream, collectionOfSavedData);
                        }
                        else if (fileInfo.Extension.Equals(".json", StringComparison.OrdinalIgnoreCase))
                        {
                            fileStream.Write(Newtonsoft.Json.JsonConvert.SerializeObject(collectionOfSavedData, Newtonsoft.Json.Formatting.Indented));
                        }
                        else if (fileInfo.Extension.Equals(".csv", StringComparison.OrdinalIgnoreCase))
                        {
                            fileStream.Write(csvStringBuilder.ToString().Trim());
                        }
                    }

                    if (Interaction.MsgBox($"Data exported to \"{saveFileDialog.FileName}\" successfully.{Constants.vbCrLf}{Constants.vbCrLf}Do you want to open Windows Explorer to the location of the file?", (MsgBoxStyle)((int)MsgBoxStyle.Question + (int)MsgBoxStyle.YesNo + (int)Constants.vbDefaultButton2), ParentForm.Text) == MsgBoxResult.Yes)
                    {
                        SupportCode.SupportCode.SelectFileInWindowsExplorer(saveFileDialog.FileName);
                    }
                }
            }
        }

        public static void WriteLogsToDisk()
        {
            lock (ParentForm.dataGridLockObject)
            {
                var collectionOfSavedData = new List<SavedData>();
                MyDataGridViewRow myItem;

                foreach (DataGridViewRow item in ParentForm.Logs.Rows)
                {
                    if (!string.IsNullOrWhiteSpace(Conversions.ToString(item.Cells[SupportCode.SupportCode.ColumnIndex_ComputedTime].Value)))
                    {
                        myItem = (MyDataGridViewRow)item;

                        collectionOfSavedData.Add(new SavedData()
                        {
                            time = Conversions.ToString(myItem.Cells[SupportCode.SupportCode.ColumnIndex_ComputedTime].Value),
                            logType = Conversions.ToString(myItem.Cells[SupportCode.SupportCode.ColumnIndex_LogType].Value),
                            ip = Conversions.ToString(myItem.Cells[SupportCode.SupportCode.ColumnIndex_IPAddress].Value),
                            appName = Conversions.ToString(myItem.Cells[SupportCode.SupportCode.ColumnIndex_RemoteProcess].Value),
                            log = Conversions.ToString(myItem.Cells[SupportCode.SupportCode.ColumnIndex_LogText].Value),
                            hostname = Conversions.ToString(myItem.Cells[SupportCode.SupportCode.ColumnIndex_Hostname].Value),
                            DateObject = myItem.DateObject,
                            BoolAlerted = myItem.BoolAlerted,
                            ServerDate = myItem.ServerDate,
                            rawLogData = myItem.RawLogData,
                            alertText = myItem.AlertText,
                            alertType = myItem.alertType
                        });
                    }
                }

                try
                {
                    using (var fileStream = new StreamWriter(SupportCode.SupportCode.strPathToDataFile + ".new"))
                    {
                        fileStream.Write(Newtonsoft.Json.JsonConvert.SerializeObject(collectionOfSavedData, Newtonsoft.Json.Formatting.Indented));
                    }

                    File.Delete(SupportCode.SupportCode.strPathToDataFile);
                    File.Move(SupportCode.SupportCode.strPathToDataFile + ".new", SupportCode.SupportCode.strPathToDataFile);
                }
                catch (Exception ex)
                {
                    Interaction.MsgBox("A critical error occurred while writing log data to disk. The old data had been saved to prevent data corruption.", MsgBoxStyle.Critical, ParentForm.Text);
                    Process.GetCurrentProcess().Kill();
                }

                ParentForm.LblLogFileSize.Text = $"Log File Size: {SupportCode.SupportCode.FileSizeToHumanSize(new FileInfo(SupportCode.SupportCode.strPathToDataFile).Length)}";

                ParentForm.BtnSaveLogsToDisk.Enabled = false;
            }
        }
    }
}