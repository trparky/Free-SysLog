using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Free_SysLog
{
    [Microsoft.VisualBasic.CompilerServices.DesignerGenerated()]
    public partial class IntegerInputForm : Form
    {

        // Form overrides dispose to clean up the component list.
        [DebuggerNonUserCode()]
        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing && components is not null)
                {
                    components.Dispose();
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        // Required by the Windows Form Designer
        private System.ComponentModel.IContainer components;

        // NOTE: The following procedure is required by the Windows Form Designer
        // It can be modified using the Windows Form Designer.  
        // Do not modify it using the code editor.
        [DebuggerStepThrough()]
        private void InitializeComponent()
        {
            lblSetting = new Label();
            TxtSetting = new TextBox();
            BtnUp = new Button();
            BtnUp.Click += new EventHandler(BtnUp_Click);
            BtnDown = new Button();
            BtnDown.Click += new EventHandler(BtnDown_Click);
            BtnSave = new Button();
            BtnSave.Click += new EventHandler(BtnSave_Click);
            BtnCancel = new Button();
            BtnCancel.Click += new EventHandler(BtnCancel_Click);
            SuspendLayout();
            // 
            // lblSetting
            // 
            lblSetting.AutoSize = true;
            lblSetting.Location = new Point(12, 9);
            lblSetting.Name = "lblSetting";
            lblSetting.Size = new Size(40, 13);
            lblSetting.TabIndex = 0;
            lblSetting.Text = "Setting";
            // 
            // TxtSetting
            // 
            TxtSetting.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            TxtSetting.Location = new Point(15, 26);
            TxtSetting.Name = "TxtSetting";
            TxtSetting.Size = new Size(186, 20);
            TxtSetting.TabIndex = 1;
            // 
            // BtnUp
            // 
            BtnUp.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            BtnUp.Location = new Point(207, 9);
            BtnUp.Name = "BtnUp";
            BtnUp.Size = new Size(24, 23);
            BtnUp.TabIndex = 2;
            BtnUp.Text = "▲";
            BtnUp.UseVisualStyleBackColor = true;
            // 
            // BtnDown
            // 
            BtnDown.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            BtnDown.Location = new Point(207, 32);
            BtnDown.Name = "BtnDown";
            BtnDown.Size = new Size(24, 23);
            BtnDown.TabIndex = 3;
            BtnDown.Text = "▼";
            BtnDown.UseVisualStyleBackColor = true;
            // 
            // BtnSave
            // 
            BtnSave.Location = new Point(15, 52);
            BtnSave.Name = "BtnSave";
            BtnSave.Size = new Size(90, 23);
            BtnSave.TabIndex = 4;
            BtnSave.Text = "Save";
            BtnSave.UseVisualStyleBackColor = true;
            // 
            // BtnCancel
            // 
            BtnCancel.Location = new Point(111, 52);
            BtnCancel.Name = "BtnCancel";
            BtnCancel.Size = new Size(90, 23);
            BtnCancel.TabIndex = 5;
            BtnCancel.Text = "Cancel";
            BtnCancel.UseVisualStyleBackColor = true;
            // 
            // IntegerInputForm
            // 
            AutoScaleDimensions = new SizeF(6.0f, 13.0f);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(243, 84);
            Controls.Add(BtnCancel);
            Controls.Add(BtnSave);
            Controls.Add(BtnDown);
            Controls.Add(BtnUp);
            Controls.Add(TxtSetting);
            Controls.Add(lblSetting);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            KeyPreview = true;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "IntegerInputForm";
            Text = "Integer Input Form";
            KeyUp += new KeyEventHandler(IntegerInputForm_KeyUp);
            ResumeLayout(false);
            PerformLayout();

        }

        internal Label lblSetting;
        internal TextBox TxtSetting;
        internal Button BtnUp;
        internal Button BtnDown;
        internal Button BtnSave;
        internal Button BtnCancel;
    }
}