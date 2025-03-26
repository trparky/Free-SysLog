using System;
using System.Collections.Generic;
using System.Net;
using System.Windows.Forms;
using Microsoft.VisualBasic;

namespace Free_SysLog
{

    public partial class Hostnames
    {
        private ListViewItem selectedItem;
        private bool boolEditMode = false;
        private bool boolDoneLoading = false;

        public Hostnames()
        {
            InitializeComponent();
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (ListHostnames.SelectedItems.Count > 0)
            {
                selectedItem = ListHostnames.SelectedItems[0];

                txtIP.Text = selectedItem.SubItems[0].Text;
                txtHostname.Text = selectedItem.SubItems[1].Text;

                boolEditMode = true;
                BtnAddSave.Text = "Save";
                lblAddEditHostNameLabel.Text = "Edit Custom Hostname";
            }
        }

        private void BtnAddSave_Click(object sender, EventArgs e)
        {
            IPAddress tempIP = null;

            if (IPAddress.TryParse(txtIP.Text, out tempIP))
            {
                if (boolEditMode)
                {
                    selectedItem.SubItems[0].Text = txtIP.Text;
                    selectedItem.SubItems[1].Text = txtHostname.Text;

                    lblAddEditHostNameLabel.Text = "Add New Custom Hostname";
                    BtnAddSave.Text = "Add";
                }
                else
                {
                    var newListViewItem = new ListViewItem(txtIP.Text);
                    newListViewItem.SubItems.Add(txtHostname.Text);
                    if (My.MySettingsProperty.Settings.font is not null)
                        newListViewItem.Font = My.MySettingsProperty.Settings.font;

                    ListHostnames.Items.Add(newListViewItem);
                }

                boolEditMode = false;
                txtIP.Text = null;
                txtHostname.Text = null;
            }
            else
            {
                Interaction.MsgBox("Invalid IP Address.", MsgBoxStyle.Critical, Text);
            }
        }

        private void ListHostnames_Click(object sender, EventArgs e)
        {
            BtnDelete.Enabled = ListHostnames.SelectedItems.Count > 0;
            BtnEdit.Enabled = ListHostnames.SelectedItems.Count > 0;

            BtnUp.Enabled = ListHostnames.SelectedIndices[0] != 0;
            BtnDown.Enabled = ListHostnames.SelectedIndices[0] != ListHostnames.Items.Count - 1;
        }

        protected override void WndProc(ref Message m)
        {
            const int WM_EXITSIZEMOVE = 0x232;

            base.WndProc(ref m);

            if (m.Msg == WM_EXITSIZEMOVE && boolDoneLoading)
                My.MySettingsProperty.Settings.hostnamesLocation = Location;
        }

        private void Hostnames_Load(object sender, EventArgs e)
        {
            Form argwindow = this;
            Location = SupportCode.SupportCode.VerifyWindowLocation(My.MySettingsProperty.Settings.hostnamesLocation, ref argwindow);
            var listOfHostnamesToAdd = new List<ListViewItem>();

            if (My.MySettingsProperty.Settings.hostnames is not null && My.MySettingsProperty.Settings.hostnames.Count > 0)
            {
                foreach (string strJSONString in My.MySettingsProperty.Settings.hostnames)
                    listOfHostnamesToAdd.Add(Newtonsoft.Json.JsonConvert.DeserializeObject<CustomHostname>(strJSONString, SupportCode.SupportCode.JSONDecoderSettingsForSettingsFiles).ToListViewItem());
            }

            ListHostnames.Items.AddRange(listOfHostnamesToAdd.ToArray());
            boolDoneLoading = true;
        }

        private void Hostnames_FormClosing(object sender, FormClosingEventArgs e)
        {
            var tempHostnames = new System.Collections.Specialized.StringCollection();
            SupportCode.SupportCode.hostnames.Clear();

            foreach (ListViewItem item in ListHostnames.Items)
            {
                tempHostnames.Add(Newtonsoft.Json.JsonConvert.SerializeObject(new CustomHostname() { ip = item.SubItems[0].Text, deviceName = item.SubItems[1].Text }));
                SupportCode.SupportCode.hostnames.Add(item.SubItems[0].Text, item.SubItems[1].Text);
            }

            My.MySettingsProperty.Settings.hostnames = tempHostnames;
            My.MySettingsProperty.Settings.Save();
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (ListHostnames.SelectedItems.Count > 0)
            {
                if (ListHostnames.SelectedItems.Count == 1)
                {
                    ListHostnames.Items.Remove(ListHostnames.SelectedItems[0]);
                }
                else
                {
                    foreach (ListViewItem item in ListHostnames.SelectedItems)
                        item.Remove();
                }
            }
        }

        private void BtnUp_Click(object sender, EventArgs e)
        {
            if (ListHostnames.SelectedItems.Count == 0)
                return; // No item selected
            int selectedIndex = ListHostnames.SelectedIndices[0];

            // Ensure the item is not already at the top
            if (selectedIndex > 0)
            {
                var item = ListHostnames.SelectedItems[0];
                ListHostnames.Items.RemoveAt(selectedIndex);
                ListHostnames.Items.Insert(selectedIndex - 1, item);
                ListHostnames.Items[selectedIndex - 1].Selected = true;
            }

            BtnUp.Enabled = ListHostnames.SelectedIndices[0] != 0;
            BtnDown.Enabled = ListHostnames.SelectedIndices[0] != ListHostnames.Items.Count - 1;
        }

        private void BtnDown_Click(object sender, EventArgs e)
        {
            if (ListHostnames.SelectedItems.Count == 0)
                return; // No item selected
            int selectedIndex = ListHostnames.SelectedIndices[0];

            // Ensure the item is not already at the bottom
            if (selectedIndex < ListHostnames.Items.Count - 1)
            {
                var item = ListHostnames.SelectedItems[0];
                ListHostnames.Items.RemoveAt(selectedIndex);
                ListHostnames.Items.Insert(selectedIndex + 1, item);
                ListHostnames.Items[selectedIndex + 1].Selected = true;
            }

            BtnUp.Enabled = ListHostnames.SelectedIndices[0] != 0;
            BtnDown.Enabled = ListHostnames.SelectedIndices[0] != ListHostnames.Items.Count - 1;
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            var saveFileDialog = new SaveFileDialog() { Title = "Export Hostnames", Filter = "JSON File|*.json", OverwritePrompt = true };
            var stringCollection = new System.Collections.Specialized.StringCollection();

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                foreach (ListViewItem item in ListHostnames.Items)
                    stringCollection.Add(Newtonsoft.Json.JsonConvert.SerializeObject(new CustomHostname() { ip = item.SubItems[0].Text, deviceName = item.SubItems[1].Text }));

                System.IO.File.WriteAllText(saveFileDialog.FileName, Newtonsoft.Json.JsonConvert.SerializeObject(My.MySettingsProperty.Settings.hostnames, Newtonsoft.Json.Formatting.Indented));

                Interaction.MsgBox("Data exported successfully.", MsgBoxStyle.Information, Text);
            }
        }

        private void BtnImport_Click(object sender, EventArgs e)
        {
            var openFileDialog = new OpenFileDialog() { Title = "Import Alerts", Filter = "JSON File|*.json" };
            var stringCollection = new System.Collections.Specialized.StringCollection();
            string[] ipHostnameSplit;
            ListViewItem newListViewItem;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string strDataFromFile = System.IO.File.ReadAllText(openFileDialog.FileName);
                    ListHostnames.Items.Clear();

                    if (strDataFromFile.CaseInsensitiveContains("ip") & strDataFromFile.CaseInsensitiveContains("deviceName"))
                    {
                        stringCollection = Newtonsoft.Json.JsonConvert.DeserializeObject<System.Collections.Specialized.StringCollection>(strDataFromFile, SupportCode.SupportCode.JSONDecoderSettingsForLogFiles);

                        CustomHostname customHostname;

                        foreach (string item in stringCollection)
                        {
                            customHostname = Newtonsoft.Json.JsonConvert.DeserializeObject<CustomHostname>(item, SupportCode.SupportCode.JSONDecoderSettingsForLogFiles);

                            newListViewItem = new ListViewItem(customHostname.ip);
                            newListViewItem.SubItems.Add(customHostname.deviceName);

                            ListHostnames.Items.Add(newListViewItem);
                        }
                    }
                    else
                    {
                        stringCollection = Newtonsoft.Json.JsonConvert.DeserializeObject<System.Collections.Specialized.StringCollection>(strDataFromFile, SupportCode.SupportCode.JSONDecoderSettingsForLogFiles);

                        foreach (string item in stringCollection)
                        {
                            ipHostnameSplit = item.Split('|');

                            newListViewItem = new ListViewItem(ipHostnameSplit[0]);
                            newListViewItem.SubItems.Add(ipHostnameSplit[1]);

                            ListHostnames.Items.Add(newListViewItem);
                        }
                    }

                    My.MySettingsProperty.Settings.Save();

                    Interaction.MsgBox("Data imported successfully.", MsgBoxStyle.Information, Text);
                }
                catch (Newtonsoft.Json.JsonSerializationException ex)
                {
                    Interaction.MsgBox("There was an error decoding the JSON data.", MsgBoxStyle.Critical, Text);
                }
            }
        }

        private void Hostnames_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
                BtnDelete.PerformClick();
        }
    }
}