using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Microsoft.VisualBasic;

namespace Free_SysLog
{

    public partial class IgnoredWordsAndPhrases
    {
        private bool boolDoneLoading = false;
        public bool boolChanged = false;
        private bool boolEditMode = false;

        public IgnoredWordsAndPhrases()
        {
            InitializeComponent();
        }

        private bool CheckForExistingItem(string strIgnored)
        {
            return IgnoredListView.Items.Cast<MyIgnoredListViewItem>().Any((item) => item.SubItems[0].Text.Equals(strIgnored, StringComparison.OrdinalIgnoreCase));
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(TxtIgnored.Text))
            {
                if (ChkRegex.Checked && !SupportCode.SupportCode.IsRegexPatternValid(TxtIgnored.Text))
                {
                    Interaction.MsgBox("Invalid regex pattern detected.", MsgBoxStyle.Critical, Text);
                    return;
                }

                if (boolEditMode)
                {
                    MyIgnoredListViewItem selectedItemObject = (MyIgnoredListViewItem)IgnoredListView.SelectedItems[0];

                    selectedItemObject.SubItems[0].Text = TxtIgnored.Text;
                    selectedItemObject.BoolCaseSensitive = ChkCaseSensitive.Checked;
                    selectedItemObject.BoolEnabled = ChkEnabled.Checked;
                    selectedItemObject.BoolRegex = ChkRegex.Checked;

                    IgnoredListView.Enabled = true;
                    BtnAdd.Text = "Add";
                    Label4.Text = "Add Ignored Words and Phrases";
                }
                else
                {
                    var IgnoredListViewItem = new MyIgnoredListViewItem(TxtIgnored.Text);

                    IgnoredListViewItem.SubItems.Add(ChkRegex.Checked ? "Yes" : "No");
                    IgnoredListViewItem.SubItems.Add(ChkCaseSensitive.Checked ? "Yes" : "No");
                    IgnoredListViewItem.SubItems.Add(ChkEnabled.Checked ? "Yes" : "No");
                    IgnoredListViewItem.BoolRegex = ChkRegex.Checked;
                    IgnoredListViewItem.BoolCaseSensitive = ChkCaseSensitive.Checked;
                    IgnoredListViewItem.BoolEnabled = ChkEnabled.Checked;
                    if (My.MySettingsProperty.Settings.font is not null)
                        IgnoredListViewItem.Font = My.MySettingsProperty.Settings.font;

                    IgnoredListView.Items.Add(IgnoredListViewItem);
                }

                boolEditMode = false;
                boolChanged = true;
                TxtIgnored.Text = null;
                ChkCaseSensitive.Checked = false;
                ChkRegex.Checked = false;
                ChkEnabled.Checked = true;
            }
        }

        private void IgnoredWordsAndPhrases_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (boolChanged)
            {
                SupportCode.SupportCode.ignoredList.Clear();

                IgnoredClass ignoredClass;
                var tempIgnored = new System.Collections.Specialized.StringCollection();

                foreach (MyIgnoredListViewItem item in IgnoredListView.Items)
                {
                    ignoredClass = new IgnoredClass() { StrIgnore = item.SubItems[0].Text, BoolCaseSensitive = item.BoolCaseSensitive, BoolRegex = item.BoolRegex, BoolEnabled = item.BoolEnabled };
                    if (ignoredClass.BoolEnabled)
                        SupportCode.SupportCode.ignoredList.Add(ignoredClass);
                    tempIgnored.Add(Newtonsoft.Json.JsonConvert.SerializeObject(ignoredClass));
                }

                My.MySettingsProperty.Settings.ignored2 = tempIgnored;
                My.MySettingsProperty.Settings.Save();
            }
        }

        private void IgnoredWordsAndPhrases_Load(object sender, EventArgs e)
        {
            BtnCancel.Visible = false;
            Form argwindow = this;
            Location = SupportCode.SupportCode.VerifyWindowLocation(My.MySettingsProperty.Settings.ignoredWordsLocation, ref argwindow);
            var MyIgnoredListViewItem = new List<MyIgnoredListViewItem>();

            if (My.MySettingsProperty.Settings.ignored2 is not null && My.MySettingsProperty.Settings.ignored2.Count > 0)
            {
                foreach (string strJSONString in My.MySettingsProperty.Settings.ignored2)
                    MyIgnoredListViewItem.Add(Newtonsoft.Json.JsonConvert.DeserializeObject<IgnoredClass>(strJSONString, SupportCode.SupportCode.JSONDecoderSettingsForSettingsFiles).ToListViewItem());
            }

            IgnoredListView.Items.AddRange(MyIgnoredListViewItem.ToArray());

            Replace.Width = My.MySettingsProperty.Settings.colIgnoredReplace;
            Regex.Width = My.MySettingsProperty.Settings.colIgnoredRegex;
            CaseSensitive.Width = My.MySettingsProperty.Settings.colIgnoredCaseSensitive;
            ColEnabled.Width = My.MySettingsProperty.Settings.colIgnoredEnabled;

            Size = My.MySettingsProperty.Settings.ConfigureIgnoredSize;

            boolDoneLoading = true;
        }

        private void IgnoredListView_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete & IgnoredListView.SelectedItems.Count > 0)
            {
                IgnoredListView.Items.Remove(IgnoredListView.SelectedItems[0]);
                BtnDelete.Enabled = false;
                BtnEdit.Enabled = false;
                boolChanged = true;
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (IgnoredListView.SelectedItems.Count > 0)
            {
                if (IgnoredListView.SelectedItems.Count == 1)
                {
                    IgnoredListView.Items.Remove(IgnoredListView.SelectedItems[0]);
                }
                else
                {
                    foreach (ListViewItem item in IgnoredListView.SelectedItems)
                        item.Remove();
                }

                BtnDelete.Enabled = false;
                BtnEdit.Enabled = false;
                boolChanged = true;
            }
        }

        private void IgnoredListView_Click(object sender, EventArgs e)
        {
            if (IgnoredListView.SelectedItems.Count > 0)
            {
                BtnDelete.Enabled = true;
                BtnEdit.Enabled = true;
                BtnEnableDisable.Enabled = true;

                BtnEnableDisable.Text = ((MyIgnoredListViewItem)IgnoredListView.SelectedItems[0]).BoolEnabled ? "Disable" : "Enable";

                BtnUp.Enabled = IgnoredListView.SelectedIndices[0] != 0;
                BtnDown.Enabled = IgnoredListView.SelectedIndices[0] != IgnoredListView.Items.Count - 1;
            }
            else
            {
                BtnDelete.Enabled = false;
                BtnEdit.Enabled = false;
                BtnEnableDisable.Enabled = false;
            }
        }

        protected override void WndProc(ref Message m)
        {
            const int WM_EXITSIZEMOVE = 0x232;

            base.WndProc(ref m);

            if (m.Msg == WM_EXITSIZEMOVE && boolDoneLoading)
                My.MySettingsProperty.Settings.ignoredWordsLocation = Location;
        }

        private void EditItem()
        {
            if (IgnoredListView.SelectedItems.Count > 0)
            {
                BtnCancel.Visible = true;
                IgnoredListView.Enabled = false;
                boolEditMode = true;
                BtnAdd.Text = "Save";
                Label4.Text = "Edit Ignored Words and Phrases";

                MyIgnoredListViewItem selectedItemObject = (MyIgnoredListViewItem)IgnoredListView.SelectedItems[0];

                TxtIgnored.Text = selectedItemObject.SubItems[0].Text;
                ChkRegex.Checked = selectedItemObject.BoolRegex;
                ChkCaseSensitive.Checked = selectedItemObject.BoolCaseSensitive;
                ChkEnabled.Checked = selectedItemObject.BoolEnabled;
            }
        }

        private void IgnoredListView_DoubleClick(object sender, EventArgs e)
        {
            EditItem();
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            EditItem();
        }

        private void ListViewMenu_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (IgnoredListView.SelectedItems.Count == 0 & IgnoredListView.SelectedItems.Count > 1)
            {
                e.Cancel = true;
                return;
            }
            else
            {
                MyIgnoredListViewItem selectedItem = (MyIgnoredListViewItem)IgnoredListView.SelectedItems[0];
                EnableDisableToolStripMenuItem.Text = selectedItem.BoolEnabled ? "Disable" : "Enable";
            }
        }

        private void DisableEnableItem()
        {
            MyIgnoredListViewItem selectedItem = (MyIgnoredListViewItem)IgnoredListView.SelectedItems[0];

            if (selectedItem.BoolEnabled)
            {
                selectedItem.BoolEnabled = false;
                selectedItem.SubItems[3].Text = "No";
                BtnEnableDisable.Text = "Enable";
            }
            else
            {
                selectedItem.BoolEnabled = true;
                selectedItem.SubItems[3].Text = "Yes";
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

        private void IgnoredListView_ColumnWidthChanged(object sender, ColumnWidthChangedEventArgs e)
        {
            if (boolDoneLoading)
            {
                My.MySettingsProperty.Settings.colIgnoredReplace = Replace.Width;
                My.MySettingsProperty.Settings.colIgnoredRegex = Regex.Width;
                My.MySettingsProperty.Settings.colIgnoredCaseSensitive = CaseSensitive.Width;
                My.MySettingsProperty.Settings.colIgnoredEnabled = ColEnabled.Width;
            }
        }

        private void IgnoredWordsAndPhrases_ResizeEnd(object sender, EventArgs e)
        {
            if (boolDoneLoading)
                My.MySettingsProperty.Settings.ConfigureIgnoredSize = Size;
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            if (IgnoredListView.Items.Count == 0)
            {
                Interaction.MsgBox("There's nothing to export.", MsgBoxStyle.Critical, Text);
                return;
            }

            var saveFileDialog = new SaveFileDialog() { Title = "Export Ignored Words and Phrases", Filter = "JSON File|*.json", OverwritePrompt = true };
            var listOfIgnoredClass = new List<IgnoredClass>();

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                foreach (MyIgnoredListViewItem item in IgnoredListView.Items)
                    listOfIgnoredClass.Add(new IgnoredClass() { StrIgnore = item.SubItems[0].Text, BoolCaseSensitive = item.BoolCaseSensitive, BoolRegex = item.BoolRegex, BoolEnabled = item.BoolEnabled });

                System.IO.File.WriteAllText(saveFileDialog.FileName, Newtonsoft.Json.JsonConvert.SerializeObject(listOfIgnoredClass, Newtonsoft.Json.Formatting.Indented));

                Interaction.MsgBox("Data exported successfully.", MsgBoxStyle.Information, Text);
            }
        }

        private void BtnImport_Click(object sender, EventArgs e)
        {
            var openFileDialog = new OpenFileDialog() { Title = "Import Ignored Words and Phrases", Filter = "JSON File|*.json" };
            var listOfIgnoredClass = new List<IgnoredClass>();

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    listOfIgnoredClass = Newtonsoft.Json.JsonConvert.DeserializeObject<List<IgnoredClass>>(System.IO.File.ReadAllText(openFileDialog.FileName), SupportCode.SupportCode.JSONDecoderSettingsForLogFiles);

                    IgnoredListView.Items.Clear();
                    SupportCode.SupportCode.ignoredList.Clear();

                    var tempIgnored = new System.Collections.Specialized.StringCollection();

                    foreach (IgnoredClass item in listOfIgnoredClass)
                    {
                        if (item.BoolEnabled)
                            SupportCode.SupportCode.ignoredList.Add(item);
                        tempIgnored.Add(Newtonsoft.Json.JsonConvert.SerializeObject(item));
                        IgnoredListView.Items.Add(item.ToListViewItem());
                    }

                    My.MySettingsProperty.Settings.ignored2 = tempIgnored;
                    My.MySettingsProperty.Settings.Save();

                    Interaction.MsgBox("Data imported successfully.", MsgBoxStyle.Information, Text);
                    boolChanged = true;
                }
                catch (Newtonsoft.Json.JsonSerializationException ex)
                {
                    Interaction.MsgBox("There was an error decoding the JSON data.", MsgBoxStyle.Critical, Text);
                }
            }
        }

        private void IgnoredWordsAndPhrases_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Close();
        }

        private void btnDeleteAll_Click(object sender, EventArgs e)
        {
            if (Interaction.MsgBox("Are you sure you want to delete all of the ignored words and phrases?", (MsgBoxStyle)((int)MsgBoxStyle.Question + (int)MsgBoxStyle.YesNo), Text) == MsgBoxResult.Yes)
            {
                IgnoredListView.Items.Clear();
                boolChanged = true;
            }
        }

        private void BtnUp_Click(object sender, EventArgs e)
        {
            if (IgnoredListView.SelectedItems.Count == 0)
                return; // No item selected
            int selectedIndex = IgnoredListView.SelectedIndices[0];

            // Ensure the item is not already at the top
            if (selectedIndex > 0)
            {
                MyIgnoredListViewItem item = (MyIgnoredListViewItem)IgnoredListView.SelectedItems[0];
                IgnoredListView.Items.RemoveAt(selectedIndex);
                IgnoredListView.Items.Insert(selectedIndex - 1, item);
                IgnoredListView.Items[selectedIndex - 1].Selected = true;
                boolChanged = true;
            }

            BtnUp.Enabled = IgnoredListView.SelectedIndices[0] != 0;
            BtnDown.Enabled = IgnoredListView.SelectedIndices[0] != IgnoredListView.Items.Count - 1;
        }

        private void BtnDown_Click(object sender, EventArgs e)
        {
            if (IgnoredListView.SelectedItems.Count == 0)
                return; // No item selected
            int selectedIndex = IgnoredListView.SelectedIndices[0];

            // Ensure the item is not already at the bottom
            if (selectedIndex < IgnoredListView.Items.Count - 1)
            {
                MyIgnoredListViewItem item = (MyIgnoredListViewItem)IgnoredListView.SelectedItems[0];
                IgnoredListView.Items.RemoveAt(selectedIndex);
                IgnoredListView.Items.Insert(selectedIndex + 1, item);
                IgnoredListView.Items[selectedIndex + 1].Selected = true;
                boolChanged = true;
            }

            BtnUp.Enabled = IgnoredListView.SelectedIndices[0] != 0;
            BtnDown.Enabled = IgnoredListView.SelectedIndices[0] != IgnoredListView.Items.Count - 1;
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            IgnoredListView.Enabled = true;
            BtnAdd.Text = "Add";
            Label4.Text = "Add Ignored Words and Phrases";
            boolEditMode = false;
            boolChanged = true;
            TxtIgnored.Text = null;
            ChkCaseSensitive.Checked = false;
            ChkRegex.Checked = false;
            ChkEnabled.Checked = true;
            BtnCancel.Visible = false;
        }
    }
}