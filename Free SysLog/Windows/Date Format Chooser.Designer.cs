using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Free_SysLog
{
    [Microsoft.VisualBasic.CompilerServices.DesignerGenerated()]
    public partial class DateFormatChooser : Form
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
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(DateFormatChooser));
            DateFormat1 = new RadioButton();
            DateFormat1.CheckedChanged += new EventHandler(DateFormat1_CheckedChanged);
            DateFormat2 = new RadioButton();
            DateFormat2.CheckedChanged += new EventHandler(DateFormat2_CheckedChanged);
            DateFormat3 = new RadioButton();
            DateFormat3.CheckedChanged += new EventHandler(DateFormat3_CheckedChanged);
            TxtCustom = new TextBox();
            TxtCustom.KeyUp += new KeyEventHandler(TxtCustom_KeyUp);
            lblCustom = new Label();
            lblExplain = new Label();
            lblCustomDateOutput = new Label();
            BtnSave = new Button();
            BtnSave.Click += new EventHandler(BtnSave_Click);
            Label1 = new Label();
            SuspendLayout();
            // 
            // DateFormat1
            // 
            DateFormat1.AutoSize = true;
            DateFormat1.Location = new Point(12, 50);
            DateFormat1.Name = "DateFormat1";
            DateFormat1.Size = new Size(90, 17);
            DateFormat1.TabIndex = 0;
            DateFormat1.TabStop = true;
            DateFormat1.Text = "RadioButton1";
            DateFormat1.UseVisualStyleBackColor = true;
            // 
            // DateFormat2
            // 
            DateFormat2.AutoSize = true;
            DateFormat2.Location = new Point(12, 73);
            DateFormat2.Name = "DateFormat2";
            DateFormat2.Size = new Size(90, 17);
            DateFormat2.TabIndex = 1;
            DateFormat2.TabStop = true;
            DateFormat2.Text = "RadioButton1";
            DateFormat2.UseVisualStyleBackColor = true;
            // 
            // DateFormat3
            // 
            DateFormat3.AutoSize = true;
            DateFormat3.Location = new Point(12, 96);
            DateFormat3.Name = "DateFormat3";
            DateFormat3.Size = new Size(95, 17);
            DateFormat3.TabIndex = 2;
            DateFormat3.TabStop = true;
            DateFormat3.Text = "Custom Format";
            DateFormat3.UseVisualStyleBackColor = true;
            // 
            // TxtCustom
            // 
            TxtCustom.Enabled = false;
            TxtCustom.Location = new Point(92, 119);
            TxtCustom.Name = "TxtCustom";
            TxtCustom.Size = new Size(168, 20);
            TxtCustom.TabIndex = 3;
            TxtCustom.Text = "MM-dd-yyyy";
            // 
            // lblCustom
            // 
            lblCustom.AutoSize = true;
            lblCustom.Location = new Point(9, 122);
            lblCustom.Name = "lblCustom";
            lblCustom.Size = new Size(77, 13);
            lblCustom.TabIndex = 4;
            lblCustom.Text = "Custom Format";
            // 
            // lblExplain
            // 
            lblExplain.AutoSize = true;
            lblExplain.Location = new Point(9, 148);
            lblExplain.Name = "lblExplain";
            lblExplain.Size = new Size(313, 208);
            lblExplain.TabIndex = 5;
            lblExplain.Text = resources.GetString("lblExplain.Text");
            // 
            // lblCustomDateOutput
            // 
            lblCustomDateOutput.AutoSize = true;
            lblCustomDateOutput.Location = new Point(266, 122);
            lblCustomDateOutput.Name = "lblCustomDateOutput";
            lblCustomDateOutput.Size = new Size(39, 13);
            lblCustomDateOutput.TabIndex = 6;
            lblCustomDateOutput.Text = "Label1";
            // 
            // BtnSave
            // 
            BtnSave.Location = new Point(185, 148);
            BtnSave.Name = "BtnSave";
            BtnSave.Size = new Size(75, 23);
            BtnSave.TabIndex = 7;
            BtnSave.Text = "Save";
            BtnSave.UseVisualStyleBackColor = true;
            // 
            // Label1
            // 
            Label1.AutoSize = true;
            Label1.Location = new Point(12, 9);
            Label1.Name = "Label1";
            Label1.Size = new Size(442, 26);
            Label1.TabIndex = 8;
            Label1.Text = "Be very careful using this tool." + '\r' + '\n' + "This is an advanced tool and preference for tho" + "se who known how to construct date strings.";
            // 
            // DateFormatChooser
            // 
            AutoScaleDimensions = new SizeF(6.0f, 13.0f);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(473, 366);
            Controls.Add(Label1);
            Controls.Add(BtnSave);
            Controls.Add(lblCustomDateOutput);
            Controls.Add(lblExplain);
            Controls.Add(lblCustom);
            Controls.Add(TxtCustom);
            Controls.Add(DateFormat3);
            Controls.Add(DateFormat2);
            Controls.Add(DateFormat1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "DateFormatChooser";
            Text = "Backup File Name Date Format Chooser";
            Load += new EventHandler(DateFormatChooser_Load);
            FormClosing += new FormClosingEventHandler(DateFormatChooser_FormClosing);
            ResumeLayout(false);
            PerformLayout();

        }

        internal RadioButton DateFormat1;
        internal RadioButton DateFormat2;
        internal RadioButton DateFormat3;
        internal TextBox TxtCustom;
        internal Label lblCustom;
        internal Label lblExplain;
        internal Label lblCustomDateOutput;
        internal Button BtnSave;
        internal Label Label1;
    }
}