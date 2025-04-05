using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Windows.Forms;
using Microsoft.VisualBasic;

namespace Free_SysLog
{

    public partial class ConfigureSysLogMirrorClients
    {
        public bool boolSuccess = false;
        private bool boolEditMode = false;
        private bool boolDoneLoading = false;

        public ConfigureSysLogMirrorClients()
        {
            InitializeComponent();
        }

        private void ConfigureSysLogMirrorServers_Load(object sender, EventArgs e)
        {
            BtnCancel.Visible = false;
            Form argwindow = this;
            Location = SupportCode.SupportCode.VerifyWindowLocation(My.MySettingsProperty.Settings.syslogProxyLocation, ref argwindow);
            if (My.MySettingsProperty.Settings.ServersToSendTo is not null && My.MySettingsProperty.Settings.ServersToSendTo.Count > 0)
            {
                SysLogProxyServer SysLogProxyServer;

                foreach (string strJSONString in My.MySettingsProperty.Settings.ServersToSendTo)
                {
                    SysLogProxyServer = Newtonsoft.Json.JsonConvert.DeserializeObject<SysLogProxyServer>(strJSONString, SupportCode.SupportCode.JSONDecoderSettingsForSettingsFiles);
                    servers.Items.Add(SysLogProxyServer.ToListViewItem());
                    SysLogProxyServer = null;
                }
            }

            boolDoneLoading = true;
        }

        protected override void WndProc(ref Message m)
        {
            const int WM_EXITSIZEMOVE = 0x232;

            base.WndProc(ref m);

            if (m.Msg == WM_EXITSIZEMOVE && boolDoneLoading)
                My.MySettingsProperty.Settings.syslogProxyLocation = Location;
        }

        private void Servers_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (servers.SelectedItems.Count > 0)
            {
                BtnDeleteServer.Enabled = true;
                BtnEditServer.Enabled = true;
                btnEnableDisable.Enabled = true;

                btnEnableDisable.Text = ((ServerListViewItem)servers.SelectedItems[0]).BoolEnabled ? "Disable" : "Enable";
            }
            else
            {
                BtnDeleteServer.Enabled = false;
                BtnEditServer.Enabled = false;
                btnEnableDisable.Enabled = false;
            }
        }

        private void EditItem()
        {
            if (servers.SelectedItems.Count > 0)
            {
                BtnCancel.Visible = true;
                servers.Enabled = false;
                boolEditMode = true;
                BtnAddServer.Text = "Save";
                Label4.Text = "Edit Server";

                ServerListViewItem selectedItemObject = (ServerListViewItem)servers.SelectedItems[0];

                txtIP.Text = selectedItemObject.SubItems[0].Text;
                txtPort.Text = selectedItemObject.SubItems[1].Text;
                txtName.Text = selectedItemObject.SubItems[3].Text;
                chkEnabled.Checked = selectedItemObject.BoolEnabled;
            }
        }

        private void BtnAddServer_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtIP.Text) & !string.IsNullOrWhiteSpace(txtPort.Text) & !string.IsNullOrWhiteSpace(txtName.Text))
            {
                IPAddress tempIP = null;
                if (!IPAddress.TryParse(txtIP.Text, out tempIP))
                {
                    Interaction.MsgBox("You must input a valid IP address.", MsgBoxStyle.Critical, Text);
                    return;
                }

                if (boolEditMode)
                {
                    ServerListViewItem selectedItemObject = (ServerListViewItem)servers.SelectedItems[0];

                    selectedItemObject.SubItems[0].Text = txtIP.Text;
                    selectedItemObject.SubItems[1].Text = txtPort.Text;
                    selectedItemObject.SubItems[2].Text = chkEnabled.Checked ? "Yes" : "No";
                    selectedItemObject.SubItems[3].Text = txtName.Text;
                    selectedItemObject.BoolEnabled = chkEnabled.Checked;

                    servers.Enabled = true;
                    BtnAddServer.Text = "Add";
                    Label4.Text = "Add Server";
                }
                else
                {
                    var ServerListView = new ServerListViewItem(txtIP.Text);

                    ServerListView.SubItems.Add(txtPort.Text);
                    ServerListView.SubItems.Add(chkEnabled.Checked ? "Yes" : "No");
                    ServerListView.SubItems.Add(txtName.Text);
                    ServerListView.BoolEnabled = chkEnabled.Checked;

                    servers.Items.Add(ServerListView);
                }

                boolEditMode = false;
                txtIP.Text = null;
                txtName.Text = null;
                txtPort.Text = null;
                chkEnabled.Checked = true;
            }
            else
            {
                Interaction.MsgBox("You need to fill in the appropriate information to create a server entry.", MsgBoxStyle.Critical, Text);
            }
        }

        private void BtnEditServer_Click(object sender, EventArgs e)
        {
            EditItem();
        }

        private void Servers_DoubleClick(object sender, EventArgs e)
        {
            EditItem();
        }

        private void BtnDeleteServer_Click(object sender, EventArgs e)
        {
            servers.SelectedItems[0].Remove();
        }

        private void ConfigureSysLogMirrorServers_FormClosing(object sender, FormClosingEventArgs e)
        {
            SupportCode.SupportCode.serversList.Clear();

            SysLogProxyServer SysLogProxyServer;
            var tempServer = new System.Collections.Specialized.StringCollection();

            foreach (ServerListViewItem item in servers.Items)
            {
                SysLogProxyServer = new SysLogProxyServer() { ip = item.SubItems[0].Text, port = int.Parse(item.SubItems[1].Text), boolEnabled = item.BoolEnabled, name = item.SubItems[3].Text };
                if (SysLogProxyServer.boolEnabled)
                    SupportCode.SupportCode.serversList.Add(SysLogProxyServer);
                tempServer.Add(Newtonsoft.Json.JsonConvert.SerializeObject(SysLogProxyServer));
            }

            My.MySettingsProperty.Settings.ServersToSendTo = tempServer;
            My.MySettingsProperty.Settings.Save();
        }

        private void ContextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            ServerListViewItem selectedItem = (ServerListViewItem)servers.SelectedItems[0];
            EnableDisableToolStripMenuItem.Text = selectedItem.BoolEnabled ? "Disable" : "Enable";
        }

        private void EnableDisableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ServerListViewItem selectedItem = (ServerListViewItem)servers.SelectedItems[0];
            selectedItem.BoolEnabled = !selectedItem.BoolEnabled;
            selectedItem.SubItems[2].Text = selectedItem.BoolEnabled ? "Yes" : "No";
            btnEnableDisable.Text = selectedItem.BoolEnabled ? "Disable" : "Enable";
        }

        private void BtnEnableDisable_Click(object sender, EventArgs e)
        {
            ServerListViewItem selectedItem = (ServerListViewItem)servers.SelectedItems[0];
            selectedItem.BoolEnabled = !selectedItem.BoolEnabled;
            selectedItem.SubItems[2].Text = selectedItem.BoolEnabled ? "Yes" : "No";
            btnEnableDisable.Text = selectedItem.BoolEnabled ? "Disable" : "Enable";
        }

        private void Servers_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete & servers.SelectedItems.Count > 0)
            {
                servers.Items.Remove(servers.SelectedItems[0]);
                BtnDeleteServer.Enabled = false;
                BtnEditServer.Enabled = false;
            }
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            if (servers.Items.Count == 0)
            {
                Interaction.MsgBox("There's nothing to export.", MsgBoxStyle.Critical, Text);
                return;
            }

            var saveFileDialog = new SaveFileDialog() { Title = "Export Servers", Filter = "JSON File|*.json", OverwritePrompt = true };
            var listOfSysLogProxyServer = new List<SysLogProxyServer>();

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                foreach (ServerListViewItem item in servers.Items)
                    listOfSysLogProxyServer.Add(new SysLogProxyServer() { ip = item.SubItems[0].Text, port = int.Parse(item.SubItems[1].Text), boolEnabled = item.BoolEnabled, name = item.SubItems[3].Text });

                System.IO.File.WriteAllText(saveFileDialog.FileName, Newtonsoft.Json.JsonConvert.SerializeObject(listOfSysLogProxyServer, Newtonsoft.Json.Formatting.Indented));

                Interaction.MsgBox("Data exported successfully.", MsgBoxStyle.Information, Text);
            }
        }

        private void BtnImport_Click(object sender, EventArgs e)
        {
            var openFileDialog = new OpenFileDialog() { Title = "Import Alerts", Filter = "JSON File|*.json" };
            var listOfSysLogProxyServer = new List<SysLogProxyServer>();

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    listOfSysLogProxyServer = Newtonsoft.Json.JsonConvert.DeserializeObject<List<SysLogProxyServer>>(System.IO.File.ReadAllText(openFileDialog.FileName), SupportCode.SupportCode.JSONDecoderSettingsForLogFiles);

                    servers.Items.Clear();
                    SupportCode.SupportCode.serversList.Clear();

                    var tempServer = new System.Collections.Specialized.StringCollection();

                    foreach (SysLogProxyServer item in listOfSysLogProxyServer)
                    {
                        SupportCode.SupportCode.serversList.Add(item);
                        tempServer.Add(Newtonsoft.Json.JsonConvert.SerializeObject(item));
                        servers.Items.Add(item.ToListViewItem());
                    }

                    My.MySettingsProperty.Settings.ServersToSendTo = tempServer;
                    My.MySettingsProperty.Settings.Save();

                    Interaction.MsgBox("Data imported successfully.", MsgBoxStyle.Information, Text);
                }
                catch (Newtonsoft.Json.JsonSerializationException)
                {
                    Interaction.MsgBox("There was an error decoding the JSON data.", MsgBoxStyle.Critical, Text);
                }
            }
        }

        private void ConfigureSysLogMirrorServers_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Close();
        }

        private void btnDeleteAll_Click(object sender, EventArgs e)
        {
            if (Interaction.MsgBox("Are you sure you want to delete all of the mirror servers?", (MsgBoxStyle)((int)MsgBoxStyle.Question + (int)MsgBoxStyle.YesNo), Text) == MsgBoxResult.Yes)
            {
                servers.Items.Clear();
            }
        }

        private void BtnUp_Click(object sender, EventArgs e)
        {
            if (servers.SelectedItems.Count == 0)
                return; // No item selected
            int selectedIndex = servers.SelectedIndices[0];

            // Ensure the item is not already at the top
            if (selectedIndex > 0)
            {
                ServerListViewItem item = (ServerListViewItem)servers.SelectedItems[0];
                servers.Items.RemoveAt(selectedIndex);
                servers.Items.Insert(selectedIndex - 1, item);
                servers.Items[selectedIndex - 1].Selected = true;
            }
        }

        private void BtnDown_Click(object sender, EventArgs e)
        {
            if (servers.SelectedItems.Count == 0)
                return; // No item selected
            int selectedIndex = servers.SelectedIndices[0];

            // Ensure the item is not already at the bottom
            if (selectedIndex < servers.Items.Count - 1)
            {
                ServerListViewItem item = (ServerListViewItem)servers.SelectedItems[0];
                servers.Items.RemoveAt(selectedIndex);
                servers.Items.Insert(selectedIndex + 1, item);
                servers.Items[selectedIndex + 1].Selected = true;
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            servers.Enabled = true;
            BtnAddServer.Text = "Add";
            Label4.Text = "Add Server";
            boolEditMode = false;
            txtIP.Text = null;
            txtName.Text = null;
            txtPort.Text = null;
            chkEnabled.Checked = true;
            BtnCancel.Visible = false;
        }
    }
}