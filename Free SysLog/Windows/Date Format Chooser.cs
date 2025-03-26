using System;
using System.Windows.Forms;
using Microsoft.VisualBasic;

namespace Free_SysLog
{

    public partial class DateFormatChooser
    {
        private bool boolDoneLoading = false;
        private bool boolChanged = false;

        public DateFormatChooser()
        {
            InitializeComponent();
        }

        private void DateFormatChooser_Load(object sender, EventArgs e)
        {
            Form argwindow = this;
            Location = SupportCode.SupportCode.VerifyWindowLocation(My.MySettingsProperty.Settings.DateChooserWindowLocation, ref argwindow);

            DateFormat1.Text = DateTime.Now.ToLongDateString();
            DateFormat2.Text = DateTime.Now.ToShortDateString().Replace("/", "-").Replace(@"\", "-");

            switch (My.MySettingsProperty.Settings.DateFormat)
            {
                case 1:
                    {
                        DateFormat1.Checked = true;
                        TxtCustom.Enabled = false;
                        break;
                    }
                case 2:
                    {
                        DateFormat2.Checked = true;
                        TxtCustom.Enabled = false;
                        break;
                    }
                case 3:
                    {
                        DateFormat3.Checked = true;
                        TxtCustom.Enabled = true;
                        TxtCustom.Text = My.MySettingsProperty.Settings.CustomDateFormat;
                        break;
                    }
            }

            lblCustomDateOutput.Text = DateTime.Now.ToString("MM-dd-yyyy");

            boolDoneLoading = true;
        }

        protected override void WndProc(ref Message m)
        {
            const int WM_EXITSIZEMOVE = 0x232;

            base.WndProc(ref m);

            if (m.Msg == WM_EXITSIZEMOVE && boolDoneLoading)
                My.MySettingsProperty.Settings.DateChooserWindowLocation = Location;
        }

        private void DateFormat3_CheckedChanged(object sender, EventArgs e)
        {
            TxtCustom.Enabled = true;
            if (boolDoneLoading)
                boolChanged = true;
        }

        private void DateFormat1_CheckedChanged(object sender, EventArgs e)
        {
            TxtCustom.Enabled = false;
            if (boolDoneLoading)
                boolChanged = true;
        }

        private void DateFormat2_CheckedChanged(object sender, EventArgs e)
        {
            TxtCustom.Enabled = false;
            if (boolDoneLoading)
                boolChanged = true;
        }

        private void TxtCustom_KeyUp(object sender, KeyEventArgs e)
        {
            lblCustomDateOutput.Text = DateTime.Now.ToString(TxtCustom.Text);
            if (boolDoneLoading)
                boolChanged = true;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (DateFormat1.Checked)
            {
                My.MySettingsProperty.Settings.DateFormat = 1;
            }
            else if (DateFormat2.Checked)
            {
                My.MySettingsProperty.Settings.DateFormat = 2;
            }
            else if (DateFormat3.Checked)
            {
                My.MySettingsProperty.Settings.DateFormat = 3;
                My.MySettingsProperty.Settings.CustomDateFormat = TxtCustom.Text;
            }

            My.MySettingsProperty.Settings.Save();
            boolChanged = false;
        }

        private void DateFormatChooser_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (boolChanged && Interaction.MsgBox("Preferences have changed, do you want to save them?", (MsgBoxStyle)((int)MsgBoxStyle.Question + (int)MsgBoxStyle.YesNo), Text) == MsgBoxResult.Yes)
                BtnSave.PerformClick();
        }
    }
}