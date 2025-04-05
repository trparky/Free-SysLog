using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Microsoft.VisualBasic;

namespace Free_SysLog
{

    public partial class Alerts
    {
        private bool boolDoneLoading = false;
        public bool boolChanged = false;
        private bool boolEditMode = false;

        public Alerts()
        {
            InitializeComponent();
        }

        private void AlertTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (AlertTypeComboBox.SelectedIndex == 0)
            {
                IconPictureBox.Image = SystemIcons.Warning.ToBitmap();
            }
            else if (AlertTypeComboBox.SelectedIndex == 1)
            {
                IconPictureBox.Image = SystemIcons.Error.ToBitmap();
            }
            else if (AlertTypeComboBox.SelectedIndex == 2)
            {
                IconPictureBox.Image = SystemIcons.Information.ToBitmap();
            }
            else if (AlertTypeComboBox.SelectedIndex == 3)
            {
                IconPictureBox.Image = null;
            }

            if (boolDoneLoading)
                boolChanged = true;
        }

        private bool CheckForExistingItem(string strIgnored)
        {
            return AlertsListView.Items.Cast<AlertsListViewItem>().Any((item) => item.SubItems[0].Text.Equals(strIgnored, StringComparison.OrdinalIgnoreCase));
        }

        private void Alerts_Load(object sender, EventArgs e)
        {
            BtnCancel.Visible = false;
            Form argwindow = this;
            Location = SupportCode.SupportCode.VerifyWindowLocation(My.MySettingsProperty.Settings.alertsLocation, ref argwindow);
            var MyIgnoredListViewItem = new List<AlertsListViewItem>();

            if (My.MySettingsProperty.Settings.alerts is not null)
            {
                foreach (string strJSONString in My.MySettingsProperty.Settings.alerts)
                    MyIgnoredListViewItem.Add(Newtonsoft.Json.JsonConvert.DeserializeObject<AlertsClass>(strJSONString, SupportCode.SupportCode.JSONDecoderSettingsForSettingsFiles).ToListViewItem());

                AlertsListView.Items.AddRange(MyIgnoredListViewItem.ToArray());
            }

            AlertLogText.Width = My.MySettingsProperty.Settings.colAlertsAlertLogText;
            AlertText.Width = My.MySettingsProperty.Settings.colAlertsAlertText;
            Regex.Width = My.MySettingsProperty.Settings.colAlertsRegex;
            CaseSensitive.Width = My.MySettingsProperty.Settings.colAlertsCaseSensitive;
            AlertTypeColumn.Width = My.MySettingsProperty.Settings.colAlertsType;
            ColEnabled.Width = My.MySettingsProperty.Settings.colAlertsEnabled;

            Size = My.MySettingsProperty.Settings.ConfigureAlertsSize;

            boolDoneLoading = true;
        }

        private void AlertsListView_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete & AlertsListView.SelectedItems.Count > 0)
            {
                AlertsListView.Items.Remove(AlertsListView.SelectedItems[0]);
                BtnDelete.Enabled = false;
                BtnEdit.Enabled = false;
                boolChanged = true;
            }
        }

        private void AlertsListView_Click(object sender, EventArgs e)
        {
            if (AlertsListView.SelectedItems.Count > 0)
            {
                BtnDelete.Enabled = true;
                BtnEdit.Enabled = true;
                BtnEnableDisable.Enabled = true;

                BtnEnableDisable.Text = ((AlertsListViewItem)AlertsListView.SelectedItems[0]).BoolEnabled ? "Disable" : "Enable";

                BtnUp.Enabled = AlertsListView.SelectedIndices[0] != 0;
                BtnDown.Enabled = AlertsListView.SelectedIndices[0] != AlertsListView.Items.Count - 1;
            }
            else
            {
                BtnDelete.Enabled = false;
                BtnEdit.Enabled = false;
                BtnEnableDisable.Enabled = false;
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (AlertsListView.SelectedItems.Count > 0)
            {
                if (AlertsListView.SelectedItems.Count == 1)
                {
                    AlertsListView.Items.Remove(AlertsListView.SelectedItems[0]);
                }
                else
                {
                    foreach (ListViewItem item in AlertsListView.SelectedItems)
                        item.Remove();
                }

                BtnDelete.Enabled = false;
                BtnEdit.Enabled = false;
                boolChanged = true;
            }
        }

        protected override void WndProc(ref Message m)
        {
            const int WM_EXITSIZEMOVE = 0x232;

            base.WndProc(ref m);

            if (m.Msg == WM_EXITSIZEMOVE && boolDoneLoading)
                My.MySettingsProperty.Settings.alertsLocation = Location;
        }

        private void EditItem()
        {
            if (AlertsListView.SelectedItems.Count > 0)
            {
                BtnCancel.Visible = true;
                AlertsListView.Enabled = false;
                boolEditMode = true;
                BtnAdd.Text = "Save";
                Label4.Text = "Edit Alert";

                AlertsListViewItem selectedItemObject = (AlertsListViewItem)AlertsListView.SelectedItems[0];

                TxtAlertText.Text = selectedItemObject.StrAlertText;
                TxtLogText.Text = selectedItemObject.StrLogText;
                ChkEnabled.Checked = selectedItemObject.BoolEnabled;
                ChkCaseSensitive.Checked = selectedItemObject.BoolCaseSensitive;
                ChkRegex.Checked = selectedItemObject.BoolRegex;
                ChkLimited.Checked = selectedItemObject.BoolLimited;

                if (selectedItemObject.AlertType == AlertType.Warning)
                {
                    IconPictureBox.Image = SystemIcons.Warning.ToBitmap();
                    AlertTypeComboBox.SelectedIndex = 0;
                }
                else if (selectedItemObject.AlertType == AlertType.ErrorMsg)
                {
                    IconPictureBox.Image = SystemIcons.Error.ToBitmap();
                    AlertTypeComboBox.SelectedIndex = 1;
                }
                else if (selectedItemObject.AlertType == AlertType.Info)
                {
                    IconPictureBox.Image = SystemIcons.Information.ToBitmap();
                    AlertTypeComboBox.SelectedIndex = 2;
                }
                else if (selectedItemObject.AlertType == AlertType.None)
                {
                    IconPictureBox.Image = null;
                    AlertTypeComboBox.SelectedIndex = 3;
                }
            }
        }

        private void AlertsListView_DoubleClick(object sender, EventArgs e)
        {
            EditItem();
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            EditItem();
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(TxtLogText.Text))
            {
                if (ChkRegex.Checked && !SupportCode.SupportCode.IsRegexPatternValid(TxtLogText.Text))
                {
                    Interaction.MsgBox("Invalid regex pattern detected.", MsgBoxStyle.Critical, Text);
                    return;
                }

                if (boolEditMode)
                {
                    AlertsListViewItem selectedItemObject = (AlertsListViewItem)AlertsListView.SelectedItems[0];

                    selectedItemObject.StrLogText = TxtLogText.Text;
                    selectedItemObject.StrAlertText = TxtAlertText.Text;
                    selectedItemObject.SubItems[0].Text = TxtLogText.Text;
                    selectedItemObject.SubItems[1].Text = string.IsNullOrWhiteSpace(TxtAlertText.Text) ? "(Shows Log Text)" : TxtAlertText.Text;
                    selectedItemObject.SubItems[2].Text = ChkRegex.Checked ? "Yes" : "No";
                    selectedItemObject.SubItems[3].Text = ChkCaseSensitive.Checked ? "Yes" : "No";

                    var AlertType = default(AlertType);

                    if (AlertTypeComboBox.SelectedIndex == 0)
                    {
                        AlertType = Free_SysLog.AlertType.Warning;
                        selectedItemObject.SubItems[4].Text = "Warning";
                    }
                    else if (AlertTypeComboBox.SelectedIndex == 1)
                    {
                        AlertType = Free_SysLog.AlertType.ErrorMsg;
                        selectedItemObject.SubItems[4].Text = "Error";
                    }
                    else if (AlertTypeComboBox.SelectedIndex == 2)
                    {
                        AlertType = Free_SysLog.AlertType.Info;
                        selectedItemObject.SubItems[4].Text = "Information";
                    }
                    else if (AlertTypeComboBox.SelectedIndex == 3)
                    {
                        AlertType = Free_SysLog.AlertType.None;
                        selectedItemObject.SubItems[4].Text = "None";
                    }

                    selectedItemObject.SubItems[5].Text = ChkLimited.Checked ? "Yes" : "No";
                    selectedItemObject.SubItems[6].Text = ChkEnabled.Checked ? "Yes" : "No";
                    selectedItemObject.BoolRegex = ChkRegex.Checked;
                    selectedItemObject.BoolCaseSensitive = ChkCaseSensitive.Checked;
                    selectedItemObject.AlertType = AlertType;
                    selectedItemObject.BoolEnabled = ChkEnabled.Checked;
                    selectedItemObject.BoolLimited = ChkLimited.Checked;

                    AlertsListView.Enabled = true;
                    BtnAdd.Text = "Add";
                    Label4.Text = "Add Alert";
                }
                else
                {
                    var AlertsListViewItem = new AlertsListViewItem(TxtLogText.Text) { StrLogText = TxtLogText.Text, StrAlertText = TxtAlertText.Text };

                    AlertsListViewItem.SubItems.Add(string.IsNullOrWhiteSpace(TxtAlertText.Text) ? "(Shows Log Text)" : TxtAlertText.Text);
                    AlertsListViewItem.SubItems.Add(ChkRegex.Checked ? "Yes" : "No");
                    AlertsListViewItem.SubItems.Add(ChkCaseSensitive.Checked ? "Yes" : "No");

                    var AlertType = default(AlertType);

                    if (AlertTypeComboBox.SelectedIndex == 0)
                    {
                        AlertType = Free_SysLog.AlertType.Warning;
                        AlertsListViewItem.SubItems.Add("Warning");
                    }
                    else if (AlertTypeComboBox.SelectedIndex == 1)
                    {
                        AlertType = Free_SysLog.AlertType.ErrorMsg;
                        AlertsListViewItem.SubItems.Add("Error");
                    }
                    else if (AlertTypeComboBox.SelectedIndex == 2)
                    {
                        AlertType = Free_SysLog.AlertType.Info;
                        AlertsListViewItem.SubItems.Add("Information");
                    }
                    else if (AlertTypeComboBox.SelectedIndex == 3)
                    {
                        AlertType = Free_SysLog.AlertType.None;
                        AlertsListViewItem.SubItems.Add("None");
                    }

                    AlertsListViewItem.SubItems.Add(ChkLimited.Checked ? "Yes" : "No");
                    AlertsListViewItem.SubItems.Add(ChkEnabled.Checked ? "Yes" : "No");
                    AlertsListViewItem.BoolRegex = ChkRegex.Checked;
                    AlertsListViewItem.BoolCaseSensitive = ChkCaseSensitive.Checked;
                    AlertsListViewItem.AlertType = AlertType;
                    AlertsListViewItem.BoolEnabled = ChkEnabled.Checked;
                    AlertsListViewItem.BoolLimited = ChkLimited.Checked;
                    if (My.MySettingsProperty.Settings.font is not null)
                        AlertsListViewItem.Font = My.MySettingsProperty.Settings.font;

                    AlertsListView.Items.Add(AlertsListViewItem);
                }

                boolEditMode = false;
                boolChanged = true;
                TxtAlertText.Text = null;
                TxtLogText.Text = null;
                IconPictureBox.Image = null;
                AlertTypeComboBox.SelectedIndex = -1;
                ChkCaseSensitive.Checked = false;
                ChkRegex.Checked = false;
                ChkEnabled.Checked = true;
                ChkLimited.Checked = true;
            }
            else
            {
                Interaction.MsgBox("You need to fill in the appropriate information to create an alert.", MsgBoxStyle.Critical, Text);
            }
        }

        private void Alerts_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (boolChanged)
            {
                SupportCode.SupportCode.alertsList.Clear();

                AlertsClass AlertsClass;
                var tempAlerts = new System.Collections.Specialized.StringCollection();

                foreach (AlertsListViewItem item in AlertsListView.Items)
                {
                    AlertsClass = new AlertsClass() { StrLogText = item.StrLogText, StrAlertText = item.StrAlertText, BoolCaseSensitive = item.BoolCaseSensitive, BoolRegex = item.BoolRegex, alertType = item.AlertType, BoolEnabled = item.BoolEnabled, BoolLimited = item.BoolLimited };
                    if (AlertsClass.BoolEnabled)
                        SupportCode.SupportCode.alertsList.Add(AlertsClass);
                    tempAlerts.Add(Newtonsoft.Json.JsonConvert.SerializeObject(AlertsClass));
                }

                My.MySettingsProperty.Settings.alerts = tempAlerts;
                My.MySettingsProperty.Settings.Save();
            }
        }

        private void ListViewMenu_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (AlertsListView.SelectedItems.Count == 0 & AlertsListView.SelectedItems.Count > 1)
            {
                e.Cancel = true;
                return;
            }
            else
            {
                AlertsListViewItem selectedItem = (AlertsListViewItem)AlertsListView.SelectedItems[0];
                EnableDisableToolStripMenuItem.Text = selectedItem.BoolEnabled ? "Disable" : "Enable";
            }
        }

        private void DisableEnableItem()
        {
            AlertsListViewItem selectedItem = (AlertsListViewItem)AlertsListView.SelectedItems[0];

            if (selectedItem.BoolEnabled)
            {
                selectedItem.BoolEnabled = false;
                selectedItem.SubItems[5].Text = "No";
                BtnEnableDisable.Text = "Enable";
            }
            else
            {
                selectedItem.BoolEnabled = true;
                selectedItem.SubItems[5].Text = "Yes";
                BtnEnableDisable.Text = "Disable";
            }

            boolChanged = true;
        }

        private void BtnEnableDisable_Click(object sender, EventArgs e)
        {
            DisableEnableItem();
        }

        private void EnableDisableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DisableEnableItem();
        }

        private void AlertsListView_ColumnWidthChanged(object sender, ColumnWidthChangedEventArgs e)
        {
            if (boolDoneLoading)
            {
                My.MySettingsProperty.Settings.colAlertsAlertLogText = AlertLogText.Width;
                My.MySettingsProperty.Settings.colAlertsAlertText = AlertText.Width;
                My.MySettingsProperty.Settings.colAlertsRegex = Regex.Width;
                My.MySettingsProperty.Settings.colAlertsCaseSensitive = CaseSensitive.Width;
                My.MySettingsProperty.Settings.colAlertsType = AlertTypeColumn.Width;
                My.MySettingsProperty.Settings.colAlertsEnabled = ColEnabled.Width;
            }
        }

        private void Alerts_ResizeEnd(object sender, EventArgs e)
        {
            if (boolDoneLoading)
                My.MySettingsProperty.Settings.ConfigureAlertsSize = Size;
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            if (AlertsListView.Items.Count == 0)
            {
                Interaction.MsgBox("There's nothing to export.", MsgBoxStyle.Critical, Text);
                return;
            }

            var saveFileDialog = new SaveFileDialog() { Title = "Export Alerts", Filter = "JSON File|*.json", OverwritePrompt = true };
            var listOfAlertsClass = new List<AlertsClass>();

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                foreach (AlertsListViewItem item in AlertsListView.Items)
                    listOfAlertsClass.Add(new AlertsClass() { StrLogText = item.StrLogText, StrAlertText = item.StrAlertText, BoolCaseSensitive = item.BoolCaseSensitive, BoolRegex = item.BoolRegex, alertType = item.AlertType, BoolEnabled = item.BoolEnabled, BoolLimited = item.BoolLimited });

                System.IO.File.WriteAllText(saveFileDialog.FileName, Newtonsoft.Json.JsonConvert.SerializeObject(listOfAlertsClass, Newtonsoft.Json.Formatting.Indented));

                Interaction.MsgBox("Data exported successfully.", MsgBoxStyle.Information, Text);
            }
        }

        private void BtnImport_Click(object sender, EventArgs e)
        {
            var openFileDialog = new OpenFileDialog() { Title = "Import Alerts", Filter = "JSON File|*.json" };
            var listOfAlertsClass = new List<AlertsClass>();

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    listOfAlertsClass = Newtonsoft.Json.JsonConvert.DeserializeObject<List<AlertsClass>>(System.IO.File.ReadAllText(openFileDialog.FileName), SupportCode.SupportCode.JSONDecoderSettingsForLogFiles);

                    AlertsListView.Items.Clear();
                    SupportCode.SupportCode.alertsList.Clear();

                    var tempAlerts = new System.Collections.Specialized.StringCollection();

                    foreach (AlertsClass item in listOfAlertsClass)
                    {
                        SupportCode.SupportCode.alertsList.Add(item);
                        tempAlerts.Add(Newtonsoft.Json.JsonConvert.SerializeObject(item));
                        AlertsListView.Items.Add(item.ToListViewItem());
                    }

                    My.MySettingsProperty.Settings.alerts = tempAlerts;
                    My.MySettingsProperty.Settings.Save();

                    Interaction.MsgBox("Data imported successfully.", MsgBoxStyle.Information, Text);
                    boolChanged = true;
                }
                catch (Newtonsoft.Json.JsonSerializationException)
                {
                    Interaction.MsgBox("There was an error decoding the JSON data.", MsgBoxStyle.Critical, Text);
                }
            }
        }

        private void Alerts_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Close();
        }

        private void BtnDeleteAll_Click(object sender, EventArgs e)
        {
            if (Interaction.MsgBox("Are you sure you want to delete all of the configured alerts?", (MsgBoxStyle)((int)MsgBoxStyle.Question + (int)MsgBoxStyle.YesNo), Text) == MsgBoxResult.Yes)
            {
                AlertsListView.Items.Clear();
                boolChanged = true;
            }
        }

        private void BtnUp_Click(object sender, EventArgs e)
        {
            if (AlertsListView.SelectedItems.Count == 0)
                return; // No item selected
            int selectedIndex = AlertsListView.SelectedIndices[0];

            // Ensure the item is not already at the top
            if (selectedIndex > 0)
            {
                AlertsListViewItem item = (AlertsListViewItem)AlertsListView.SelectedItems[0];
                AlertsListView.Items.RemoveAt(selectedIndex);
                AlertsListView.Items.Insert(selectedIndex - 1, item);
                AlertsListView.Items[selectedIndex - 1].Selected = true;
                boolChanged = true;
            }

            BtnUp.Enabled = AlertsListView.SelectedIndices[0] != 0;
            BtnDown.Enabled = AlertsListView.SelectedIndices[0] != AlertsListView.Items.Count - 1;
        }

        private void BtnDown_Click(object sender, EventArgs e)
        {
            if (AlertsListView.SelectedItems.Count == 0)
                return; // No item selected
            int selectedIndex = AlertsListView.SelectedIndices[0];

            // Ensure the item is not already at the bottom
            if (selectedIndex < AlertsListView.Items.Count - 1)
            {
                AlertsListViewItem item = (AlertsListViewItem)AlertsListView.SelectedItems[0];
                AlertsListView.Items.RemoveAt(selectedIndex);
                AlertsListView.Items.Insert(selectedIndex + 1, item);
                AlertsListView.Items[selectedIndex + 1].Selected = true;
                boolChanged = true;
            }

            BtnUp.Enabled = AlertsListView.SelectedIndices[0] != 0;
            BtnDown.Enabled = AlertsListView.SelectedIndices[0] != AlertsListView.Items.Count - 1;
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            AlertsListView.Enabled = true;
            BtnAdd.Text = "Add";
            Label4.Text = "Add Alert";
            boolEditMode = false;
            boolChanged = true;
            TxtAlertText.Text = null;
            TxtLogText.Text = null;
            IconPictureBox.Image = null;
            AlertTypeComboBox.SelectedIndex = -1;
            ChkCaseSensitive.Checked = false;
            ChkRegex.Checked = false;
            ChkEnabled.Checked = true;
            BtnCancel.Visible = false;
        }
    }
}