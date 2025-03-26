using System;
using System.Windows.Forms;
using Microsoft.VisualBasic;

namespace Free_SysLog
{
    public partial class IntegerInputForm
    {
        public int intResult;
        private int intMin, intMax;

        public IntegerInputForm(int intInputMin, int intInputMax)
        {
            // This call is required by the designer.
            InitializeComponent();

            // Add any initialization after the InitializeComponent() call.
            intMin = intInputMin;
            intMax = intInputMax;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (int.TryParse(TxtSetting.Text, out intResult))
            {
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                Interaction.MsgBox("Invalid user input!", MsgBoxStyle.Critical, Text);
            }
        }

        private void BtnUp_Click(object sender, EventArgs e)
        {
            int myInteger;

            if (int.TryParse(TxtSetting.Text, out myInteger))
            {
                if (myInteger == intMax)
                    return;
                myInteger += 1;
                TxtSetting.Text = myInteger.ToString();
            }
        }

        private void BtnDown_Click(object sender, EventArgs e)
        {
            int myInteger;

            if (int.TryParse(TxtSetting.Text, out myInteger))
            {
                if (myInteger == intMin)
                    return;
                myInteger -= 1;
                TxtSetting.Text = myInteger.ToString();
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void IntegerInputForm_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Escape)
            {
                DialogResult = DialogResult.Cancel;
            }
            else if (e.KeyData == Keys.Enter)
            {
                DialogResult = DialogResult.OK;
            }

            Close();
        }
    }
}