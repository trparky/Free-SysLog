using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Free_SysLog
{
    [Microsoft.VisualBasic.CompilerServices.DesignerGenerated()]
    public partial class Hostnames : Form
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
            ListHostnames = new ListView();
            ListHostnames.Click += new EventHandler(ListHostnames_Click);
            colIP = new ColumnHeader();
            colHostname = new ColumnHeader();
            ipAddressLabel = new Label();
            BtnDelete = new Button();
            BtnDelete.Click += new EventHandler(BtnDelete_Click);
            txtIP = new TextBox();
            txtHostname = new TextBox();
            hostnameLabel = new Label();
            BtnAddSave = new Button();
            BtnAddSave.Click += new EventHandler(BtnAddSave_Click);
            BtnEdit = new Button();
            BtnEdit.Click += new EventHandler(BtnEdit_Click);
            lblAddEditHostNameLabel = new Label();
            BtnDown = new Button();
            BtnDown.Click += new EventHandler(BtnDown_Click);
            BtnUp = new Button();
            BtnUp.Click += new EventHandler(BtnUp_Click);
            BtnImport = new Button();
            BtnImport.Click += new EventHandler(BtnImport_Click);
            BtnExport = new Button();
            BtnExport.Click += new EventHandler(BtnExport_Click);
            SeparatingLine = new Label();
            SuspendLayout();
            // 
            // ListHostnames
            // 
            ListHostnames.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            ListHostnames.Columns.AddRange(new ColumnHeader[] { colIP, colHostname });
            ListHostnames.FullRowSelect = true;
            ListHostnames.HideSelection = false;
            ListHostnames.Location = new Point(12, 12);
            ListHostnames.Name = "ListHostnames";
            ListHostnames.Size = new Size(566, 191);
            ListHostnames.TabIndex = 0;
            ListHostnames.UseCompatibleStateImageBehavior = false;
            ListHostnames.View = View.Details;
            // 
            // colIP
            // 
            colIP.Text = "IP Address";
            colIP.Width = 180;
            // 
            // colHostname
            // 
            colHostname.Text = "Hostname";
            colHostname.Width = 360;
            // 
            // ipAddressLabel
            // 
            ipAddressLabel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            ipAddressLabel.AutoSize = true;
            ipAddressLabel.Location = new Point(12, 275);
            ipAddressLabel.Name = "ipAddressLabel";
            ipAddressLabel.Size = new Size(58, 13);
            ipAddressLabel.TabIndex = 1;
            ipAddressLabel.Text = "IP Address";
            // 
            // BtnDelete
            // 
            BtnDelete.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            BtnDelete.Enabled = false;
            BtnDelete.Location = new Point(12, 209);
            BtnDelete.Name = "BtnDelete";
            BtnDelete.Size = new Size(75, 23);
            BtnDelete.TabIndex = 2;
            BtnDelete.Text = "Delete";
            BtnDelete.UseVisualStyleBackColor = true;
            // 
            // txtIP
            // 
            txtIP.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            txtIP.Location = new Point(76, 272);
            txtIP.Name = "txtIP";
            txtIP.Size = new Size(526, 20);
            txtIP.TabIndex = 3;
            // 
            // txtHostname
            // 
            txtHostname.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            txtHostname.Location = new Point(76, 298);
            txtHostname.Name = "txtHostname";
            txtHostname.Size = new Size(526, 20);
            txtHostname.TabIndex = 4;
            // 
            // hostnameLabel
            // 
            hostnameLabel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            hostnameLabel.AutoSize = true;
            hostnameLabel.Location = new Point(12, 301);
            hostnameLabel.Name = "hostnameLabel";
            hostnameLabel.Size = new Size(55, 13);
            hostnameLabel.TabIndex = 5;
            hostnameLabel.Text = "Hostname";
            // 
            // BtnAddSave
            // 
            BtnAddSave.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            BtnAddSave.Location = new Point(15, 326);
            BtnAddSave.Name = "BtnAddSave";
            BtnAddSave.Size = new Size(75, 23);
            BtnAddSave.TabIndex = 6;
            BtnAddSave.Text = "Add";
            BtnAddSave.UseVisualStyleBackColor = true;
            // 
            // BtnEdit
            // 
            BtnEdit.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            BtnEdit.Enabled = false;
            BtnEdit.Location = new Point(93, 209);
            BtnEdit.Name = "BtnEdit";
            BtnEdit.Size = new Size(75, 23);
            BtnEdit.TabIndex = 7;
            BtnEdit.Text = "Edit";
            BtnEdit.UseVisualStyleBackColor = true;
            // 
            // lblAddEditHostNameLabel
            // 
            lblAddEditHostNameLabel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            lblAddEditHostNameLabel.AutoSize = true;
            lblAddEditHostNameLabel.Location = new Point(12, 251);
            lblAddEditHostNameLabel.Name = "lblAddEditHostNameLabel";
            lblAddEditHostNameLabel.Size = new Size(140, 13);
            lblAddEditHostNameLabel.TabIndex = 8;
            lblAddEditHostNameLabel.Text = "Add New Custom Hostname";
            // 
            // BtnDown
            // 
            BtnDown.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            BtnDown.Location = new Point(584, 180);
            BtnDown.Name = "BtnDown";
            BtnDown.Size = new Size(24, 23);
            BtnDown.TabIndex = 21;
            BtnDown.Text = "▼";
            BtnDown.UseVisualStyleBackColor = true;
            // 
            // BtnUp
            // 
            BtnUp.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            BtnUp.Location = new Point(584, 12);
            BtnUp.Name = "BtnUp";
            BtnUp.Size = new Size(24, 23);
            BtnUp.TabIndex = 20;
            BtnUp.Text = "▲";
            BtnUp.UseVisualStyleBackColor = true;
            // 
            // BtnImport
            // 
            BtnImport.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            BtnImport.Location = new Point(533, 209);
            BtnImport.Name = "BtnImport";
            BtnImport.Size = new Size(75, 23);
            BtnImport.TabIndex = 22;
            BtnImport.Text = "Import";
            BtnImport.UseVisualStyleBackColor = true;
            // 
            // BtnExport
            // 
            BtnExport.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            BtnExport.Location = new Point(452, 209);
            BtnExport.Name = "BtnExport";
            BtnExport.Size = new Size(75, 23);
            BtnExport.TabIndex = 23;
            BtnExport.Text = "Export";
            BtnExport.UseVisualStyleBackColor = true;
            // 
            // SeparatingLine
            // 
            SeparatingLine.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            SeparatingLine.BackColor = Color.Black;
            SeparatingLine.Location = new Point(-1, 240);
            SeparatingLine.Name = "SeparatingLine";
            SeparatingLine.Size = new Size(618, 1);
            SeparatingLine.TabIndex = 24;
            // 
            // Hostnames
            // 
            AutoScaleDimensions = new SizeF(6.0f, 13.0f);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(614, 356);
            Controls.Add(SeparatingLine);
            Controls.Add(BtnExport);
            Controls.Add(BtnImport);
            Controls.Add(BtnDown);
            Controls.Add(BtnUp);
            Controls.Add(lblAddEditHostNameLabel);
            Controls.Add(BtnEdit);
            Controls.Add(BtnAddSave);
            Controls.Add(hostnameLabel);
            Controls.Add(txtHostname);
            Controls.Add(txtIP);
            Controls.Add(BtnDelete);
            Controls.Add(ipAddressLabel);
            Controls.Add(ListHostnames);
            KeyPreview = true;
            MaximizeBox = false;
            MinimizeBox = false;
            MinimumSize = new Size(630, 395);
            Name = "Hostnames";
            Text = "Configure Custom Hostnames";
            Load += new EventHandler(Hostnames_Load);
            FormClosing += new FormClosingEventHandler(Hostnames_FormClosing);
            KeyUp += new KeyEventHandler(Hostnames_KeyUp);
            ResumeLayout(false);
            PerformLayout();

        }

        internal ListView ListHostnames;
        internal ColumnHeader colIP;
        internal ColumnHeader colHostname;
        internal Label ipAddressLabel;
        internal Button BtnDelete;
        internal TextBox txtIP;
        internal TextBox txtHostname;
        internal Label hostnameLabel;
        internal Button BtnAddSave;
        internal Button BtnEdit;
        internal Label lblAddEditHostNameLabel;
        internal Button BtnDown;
        internal Button BtnUp;
        internal Button BtnImport;
        internal Button BtnExport;
        internal Label SeparatingLine;
    }
}