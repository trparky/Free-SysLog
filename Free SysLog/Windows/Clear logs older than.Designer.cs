using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Free_SysLog
{
    [Microsoft.VisualBasic.CompilerServices.DesignerGenerated()]
    public partial class ClearLogsOlderThan : Form
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
            LblLogCount = new Label();
            DateTimePicker = new DateTimePicker();
            DateTimePicker.ValueChanged += new EventHandler(DateTimePicker_ValueChanged);
            LblOlderThan = new Label();
            BtnClearLogs = new Button();
            BtnClearLogs.Click += new EventHandler(BtnClearLogs_Click);
            SuspendLayout();
            // 
            // LblLogCount
            // 
            LblLogCount.AutoSize = true;
            LblLogCount.Location = new Point(12, 9);
            LblLogCount.Name = "lblLogCount";
            LblLogCount.Size = new Size(124, 13);
            LblLogCount.TabIndex = 0;
            LblLogCount.Text = "Number of Log Entries: 0";
            // 
            // DateTimePicker
            // 
            DateTimePicker.Location = new Point(134, 28);
            DateTimePicker.Name = "DateTimePicker";
            DateTimePicker.Size = new Size(200, 20);
            DateTimePicker.TabIndex = 1;
            // 
            // LblOlderThan
            // 
            LblOlderThan.AutoSize = true;
            LblOlderThan.Location = new Point(12, 34);
            LblOlderThan.Name = "lblOlderThan";
            LblOlderThan.Size = new Size(116, 13);
            LblOlderThan.TabIndex = 2;
            LblOlderThan.Text = "Clear Logs older than...";
            // 
            // BtnClearLogs
            // 
            BtnClearLogs.Enabled = false;
            BtnClearLogs.Location = new Point(12, 54);
            BtnClearLogs.Name = "btnClearLogs";
            BtnClearLogs.Size = new Size(322, 23);
            BtnClearLogs.TabIndex = 3;
            BtnClearLogs.Text = "Clear Logs";
            BtnClearLogs.UseVisualStyleBackColor = true;
            // 
            // Clear_logs_older_than
            // 
            AutoScaleDimensions = new SizeF(6.0f, 13.0f);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(344, 88);
            Controls.Add(BtnClearLogs);
            Controls.Add(LblOlderThan);
            Controls.Add(DateTimePicker);
            Controls.Add(LblLogCount);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            KeyPreview = true;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "Clear_logs_older_than";
            Text = "Clear logs older than...";
            Load += new EventHandler(Clear_logs_older_than_Load);
            KeyUp += new KeyEventHandler(Clear_logs_older_than_KeyUp);
            ResumeLayout(false);
            PerformLayout();

        }

        internal Label LblLogCount;
        internal DateTimePicker DateTimePicker;
        internal Label LblOlderThan;
        internal Button BtnClearLogs;
    }
}