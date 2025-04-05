using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Microsoft.VisualBasic;

namespace Free_SysLog
{

    public partial class Replacements
    {
        private bool boolDoneLoading = false;
        public bool boolChanged = false;
        private bool boolEditMode = false;

        public Replacements()
        {
            InitializeComponent();
        }

        private bool CheckForExistingItem(string strReplace, string strReplaceWith)
        {
            return ReplacementsListView.Items.Cast<MyReplacementsListViewItem>().Any((item) => item.SubItems[0].Text.Equals(strReplace, StringComparison.OrdinalIgnoreCase) & item.SubItems[1].Text.Equals(strReplaceWith, StringComparison.OrdinalIgnoreCase));
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(TxtReplace.Text))
            {
                if (ChkRegex.Checked && !SupportCode.SupportCode.IsRegexPatternValid(TxtReplace.Text))
                {
                    Interaction.MsgBox("Invalid regex pattern detected.", MsgBoxStyle.Critical, Text);
                    return;
                }

                if (boolEditMode)
                {
                    MyReplacementsListViewItem selectedItemObject = (MyReplacementsListViewItem)ReplacementsListView.SelectedItems[0];

                    selectedItemObject.SubItems[0].Text = TxtReplace.Text;
                    selectedItemObject.SubItems[1].Text = TxtReplaceWith.Text;
                    selectedItemObject.SubItems[2].Text = ChkRegex.Checked ? "Yes" : "No";
                    selectedItemObject.SubItems[3].Text = ChkCaseSensitive.Checked ? "Yes" : "No";
                    selectedItemObject.SubItems[4].Text = ChkEnabled.Checked ? "Yes" : "No";
                    selectedItemObject.BoolRegex = ChkRegex.Checked;
                    selectedItemObject.BoolCaseSensitive = ChkCaseSensitive.Checked;
                    selectedItemObject.BoolEnabled = ChkEnabled.Checked;

                    ReplacementsListView.Enabled = true;
                    BtnAdd.Text = "Add";
                    Label3.Text = "Add Replacement";
                }
                else
                {
                    if (CheckForExistingItem(TxtReplace.Text, TxtReplaceWith.Text))
                    {
                        Interaction.MsgBox("A similar item has already been found in your replacements list.", MsgBoxStyle.Critical, Text);
                        return;
                    }

                    var MyReplacementsListViewItem = new MyReplacementsListViewItem(TxtReplace.Text);

                    MyReplacementsListViewItem.SubItems.Add(TxtReplaceWith.Text);
                    MyReplacementsListViewItem.SubItems.Add(ChkRegex.Checked ? "Yes" : "No");
                    MyReplacementsListViewItem.SubItems.Add(ChkCaseSensitive.Checked ? "Yes" : "No");
                    MyReplacementsListViewItem.SubItems.Add(ChkEnabled.Checked ? "Yes" : "No");
                    MyReplacementsListViewItem.BoolRegex = ChkRegex.Checked;
                    MyReplacementsListViewItem.BoolCaseSensitive = ChkCaseSensitive.Checked;
                    MyReplacementsListViewItem.BoolEnabled = ChkEnabled.Checked;
                    if (My.MySettingsProperty.Settings.font is not null)
                        MyReplacementsListViewItem.Font = My.MySettingsProperty.Settings.font;

                    ReplacementsListView.Items.Add(MyReplacementsListViewItem);
                    boolChanged = true;
                }

                boolEditMode = false;
                boolChanged = true;
                TxtReplace.Text = null;
                TxtReplaceWith.Text = null;
                ChkCaseSensitive.Checked = false;
                ChkRegex.Checked = false;
                ChkEnabled.Checked = true;
            }
            else
            {
                Interaction.MsgBox("You need to fill in the appropriate information to create an replacement.", MsgBoxStyle.Critical, Text);
            }
        }

        protected override void WndProc(ref Message m)
        {
            const int WM_EXITSIZEMOVE = 0x232;

            base.WndProc(ref m);

            if (m.Msg == WM_EXITSIZEMOVE && boolDoneLoading)
                My.MySettingsProperty.Settings.replacementsLocation = Location;
        }

        private void Replacements_Load(object sender, EventArgs e)
        {
            BtnCancel.Visible = false;
            Form argwindow = this;
            Location = SupportCode.SupportCode.VerifyWindowLocation(My.MySettingsProperty.Settings.replacementsLocation, ref argwindow);
            var listOfReplacementsToAdd = new List<MyReplacementsListViewItem>();

            if (My.MySettingsProperty.Settings.replacements is not null && My.MySettingsProperty.Settings.replacements.Count > 0)
            {
                foreach (string strJSONString in My.MySettingsProperty.Settings.replacements)
                    listOfReplacementsToAdd.Add(Newtonsoft.Json.JsonConvert.DeserializeObject<ReplacementsClass>(strJSONString, SupportCode.SupportCode.JSONDecoderSettingsForSettingsFiles).ToListViewItem());
            }

            ReplacementsListView.Items.AddRange(listOfReplacementsToAdd.ToArray());

            Replace.Width = My.MySettingsProperty.Settings.colReplacementsReplace;
            ReplaceWith.Width = My.MySettingsProperty.Settings.colReplacementsReplaceWith;
            Regex.Width = My.MySettingsProperty.Settings.colReplacementsRegex;
            CaseSensitive.Width = My.MySettingsProperty.Settings.colReplacementsCaseSensitive;
            ColEnabled.Width = My.MySettingsProperty.Settings.colReplacementsEnabled;

            Size = My.MySettingsProperty.Settings.ConfigureReplacementsSize;

            boolDoneLoading = true;
        }

        private void Replacements_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (boolChanged)
            {
                SupportCode.SupportCode.replacementsList.Clear();

                ReplacementsClass replacementsClass;
                var tempReplacements = new System.Collections.Specialized.StringCollection();

                foreach (MyReplacementsListViewItem item in ReplacementsListView.Items)
                {
                    replacementsClass = new ReplacementsClass() { BoolRegex = item.BoolRegex, StrReplace = item.SubItems[0].Text, StrReplaceWith = item.SubItems[1].Text, BoolCaseSensitive = item.BoolCaseSensitive, BoolEnabled = item.BoolEnabled };
                    if (replacementsClass.BoolEnabled)
                        SupportCode.SupportCode.replacementsList.Add(replacementsClass);
                    tempReplacements.Add(Newtonsoft.Json.JsonConvert.SerializeObject(replacementsClass));
                }

                My.MySettingsProperty.Settings.replacements = tempReplacements;
                My.MySettingsProperty.Settings.Save();
            }
        }

        private void ReplacementsListView_KeyUp(object sender, KeyEventArgs e)
        {
            if (ReplacementsListView.SelectedItems.Count > 0)
            {
                if (e.KeyCode == Keys.Delete)
                {
                    ReplacementsListView.Items.Remove(ReplacementsListView.SelectedItems[0]);
                    BtnDelete.Enabled = false;
                    BtnEdit.Enabled = false;
                    boolChanged = true;
                }
                else if (e.KeyCode == Keys.Enter)
                {
                    EditItem();
                }
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (ReplacementsListView.SelectedItems.Count > 0)
            {
                if (ReplacementsListView.SelectedItems.Count == 1)
                {
                    ReplacementsListView.Items.Remove(ReplacementsListView.SelectedItems[0]);
                }
                else
                {
                    foreach (ListViewItem item in ReplacementsListView.SelectedItems)
                        item.Remove();
                }

                boolChanged = true;
            }
        }

        private void EditItem()
        {
            if (ReplacementsListView.SelectedItems.Count > 0)
            {
                BtnCancel.Visible = true;
                ReplacementsListView.Enabled = false;
                boolEditMode = true;
                BtnAdd.Text = "Save";
                Label3.Text = "Edit Replacement";

                MyReplacementsListViewItem selectedItemObject = (MyReplacementsListViewItem)ReplacementsListView.SelectedItems[0];

                TxtReplace.Text = selectedItemObject.SubItems[0].Text;
                TxtReplaceWith.Text = selectedItemObject.SubItems[1].Text;
                ChkEnabled.Checked = selectedItemObject.BoolEnabled;
                ChkCaseSensitive.Checked = selectedItemObject.BoolCaseSensitive;
                ChkRegex.Checked = selectedItemObject.BoolRegex;
            }
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            EditItem();
        }

        private void ReplacementsListView_Click(object sender, EventArgs e)
        {
            if (ReplacementsListView.SelectedItems.Count > 0)
            {
                BtnDelete.Enabled = true;
                BtnEdit.Enabled = true;
                BtnEnableDisable.Enabled = true;

                BtnEnableDisable.Text = ((MyReplacementsListViewItem)ReplacementsListView.SelectedItems[0]).BoolEnabled ? "Disable" : "Enable";

                BtnUp.Enabled = ReplacementsListView.SelectedIndices[0] != 0;
                BtnDown.Enabled = ReplacementsListView.SelectedIndices[0] != ReplacementsListView.Items.Count - 1;
            }
            else
            {
                BtnDelete.Enabled = false;
                BtnEdit.Enabled = false;
                BtnEnableDisable.Enabled = false;
            }
        }

        private void ReplacementsListView_DoubleClick(object sender, EventArgs e)
        {
            EditItem();
        }

        private void ListViewMenu_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (ReplacementsListView.SelectedItems.Count == 0 & ReplacementsListView.SelectedItems.Count > 1)
            {
                e.Cancel = true;
                return;
            }
            else
            {
                MyReplacementsListViewItem selectedItem = (MyReplacementsListViewItem)ReplacementsListView.SelectedItems[0];
                EnableDisableToolStripMenuItem.Text = selectedItem.BoolEnabled ? "Disable" : "Enable";
            }
        }

        private void DisableEnableItem()
        {
            MyReplacementsListViewItem selectedItem = (MyReplacementsListViewItem)ReplacementsListView.SelectedItems[0];

            if (selectedItem.BoolEnabled)
            {
                selectedItem.BoolEnabled = false;
                selectedItem.SubItems[4].Text = "No";
                BtnEnableDisable.Text = "Enable";
            }
            else
            {
                selectedItem.BoolEnabled = true;
                selectedItem.SubItems[4].Text = "Yes";
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

        private void ReplacementsListView_ColumnWidthChanged(object sender, ColumnWidthChangedEventArgs e)
        {
            if (boolDoneLoading)
            {
                My.MySettingsProperty.Settings.colReplacementsReplace = Replace.Width;
                My.MySettingsProperty.Settings.colReplacementsReplaceWith = ReplaceWith.Width;
                My.MySettingsProperty.Settings.colReplacementsRegex = Regex.Width;
                My.MySettingsProperty.Settings.colReplacementsCaseSensitive = CaseSensitive.Width;
                My.MySettingsProperty.Settings.colReplacementsEnabled = ColEnabled.Width;
            }
        }

        private void Replacements_ResizeEnd(object sender, EventArgs e)
        {
            if (boolDoneLoading)
                My.MySettingsProperty.Settings.ConfigureReplacementsSize = Size;
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            if (ReplacementsListView.Items.Count == 0)
            {
                Interaction.MsgBox("There's nothing to export.", MsgBoxStyle.Critical, Text);
                return;
            }

            var saveFileDialog = new SaveFileDialog() { Title = "Export Replacements", Filter = "JSON File|*.json", OverwritePrompt = true };
            var listOfReplacementsClass = new List<ReplacementsClass>();

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                foreach (MyReplacementsListViewItem item in ReplacementsListView.Items)
                    listOfReplacementsClass.Add(new ReplacementsClass() { BoolRegex = item.BoolRegex, StrReplace = item.SubItems[0].Text, StrReplaceWith = item.SubItems[1].Text, BoolCaseSensitive = item.BoolCaseSensitive, BoolEnabled = item.BoolEnabled });

                System.IO.File.WriteAllText(saveFileDialog.FileName, Newtonsoft.Json.JsonConvert.SerializeObject(listOfReplacementsClass, Newtonsoft.Json.Formatting.Indented));

                Interaction.MsgBox("Data exported successfully.", MsgBoxStyle.Information, Text);
            }
        }

        private void BtnImport_Click(object sender, EventArgs e)
        {
            var openFileDialog = new OpenFileDialog() { Title = "Import Alerts", Filter = "JSON File|*.json" };
            var listOfReplacementsClass = new List<ReplacementsClass>();

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    listOfReplacementsClass = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ReplacementsClass>>(System.IO.File.ReadAllText(openFileDialog.FileName), SupportCode.SupportCode.JSONDecoderSettingsForLogFiles);

                    ReplacementsListView.Items.Clear();
                    SupportCode.SupportCode.replacementsList.Clear();

                    var tempReplacements = new System.Collections.Specialized.StringCollection();

                    foreach (ReplacementsClass item in listOfReplacementsClass)
                    {
                        SupportCode.SupportCode.replacementsList.Add(item);
                        tempReplacements.Add(Newtonsoft.Json.JsonConvert.SerializeObject(item));
                        ReplacementsListView.Items.Add(item.ToListViewItem());
                    }

                    My.MySettingsProperty.Settings.replacements = tempReplacements;
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

        private void Replacements_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Close();
        }

        private void btnDeleteAll_Click(object sender, EventArgs e)
        {
            if (Interaction.MsgBox("Are you sure you want to delete all of the replacements?", (MsgBoxStyle)((int)MsgBoxStyle.Question + (int)MsgBoxStyle.YesNo), Text) == MsgBoxResult.Yes)
            {
                ReplacementsListView.Items.Clear();
                boolChanged = true;
            }
        }

        private void BtnUp_Click(object sender, EventArgs e)
        {
            if (ReplacementsListView.SelectedItems.Count == 0)
                return; // No item selected
            int selectedIndex = ReplacementsListView.SelectedIndices[0];

            // Ensure the item is not already at the top
            if (selectedIndex > 0)
            {
                MyReplacementsListViewItem item = (MyReplacementsListViewItem)ReplacementsListView.SelectedItems[0];
                ReplacementsListView.Items.RemoveAt(selectedIndex);
                ReplacementsListView.Items.Insert(selectedIndex - 1, item);
                ReplacementsListView.Items[selectedIndex - 1].Selected = true;
                boolChanged = true;
            }

            BtnUp.Enabled = ReplacementsListView.SelectedIndices[0] != 0;
            BtnDown.Enabled = ReplacementsListView.SelectedIndices[0] != ReplacementsListView.Items.Count - 1;
        }

        private void BtnDown_Click(object sender, EventArgs e)
        {
            if (ReplacementsListView.SelectedItems.Count == 0)
                return; // No item selected
            int selectedIndex = ReplacementsListView.SelectedIndices[0];

            // Ensure the item is not already at the bottom
            if (selectedIndex < ReplacementsListView.Items.Count - 1)
            {
                MyReplacementsListViewItem item = (MyReplacementsListViewItem)ReplacementsListView.SelectedItems[0];
                ReplacementsListView.Items.RemoveAt(selectedIndex);
                ReplacementsListView.Items.Insert(selectedIndex + 1, item);
                ReplacementsListView.Items[selectedIndex + 1].Selected = true;
                boolChanged = true;
            }

            BtnUp.Enabled = ReplacementsListView.SelectedIndices[0] != 0;
            BtnDown.Enabled = ReplacementsListView.SelectedIndices[0] != ReplacementsListView.Items.Count - 1;
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            ReplacementsListView.Enabled = true;
            BtnAdd.Text = "Add";
            Label3.Text = "Add Replacement";
            boolEditMode = false;
            boolChanged = true;
            TxtReplace.Text = null;
            TxtReplaceWith.Text = null;
            ChkCaseSensitive.Checked = false;
            ChkRegex.Checked = false;
            ChkEnabled.Checked = true;
            BtnCancel.Visible = false;
        }
    }
}