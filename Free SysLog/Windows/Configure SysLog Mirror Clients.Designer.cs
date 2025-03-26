using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Free_SysLog
{
    [Microsoft.VisualBasic.CompilerServices.DesignerGenerated()]
    public partial class ConfigureSysLogMirrorClients : Form
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
            components = new System.ComponentModel.Container();
            servers = new ListView();
            servers.SelectedIndexChanged += new EventHandler(Servers_SelectedIndexChanged);
            servers.DoubleClick += new EventHandler(Servers_DoubleClick);
            servers.KeyUp += new KeyEventHandler(Servers_KeyUp);
            colServer = new ColumnHeader();
            colPort = new ColumnHeader();
            colEnabled = new ColumnHeader();
            colName = new ColumnHeader();
            ContextMenuStrip1 = new ContextMenuStrip(components);
            ContextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(ContextMenuStrip1_Opening);
            EnableDisableToolStripMenuItem = new ToolStripMenuItem();
            EnableDisableToolStripMenuItem.Click += new EventHandler(EnableDisableToolStripMenuItem_Click);
            BtnAddServer = new Button();
            BtnAddServer.Click += new EventHandler(BtnAddServer_Click);
            BtnEditServer = new Button();
            BtnEditServer.Click += new EventHandler(BtnEditServer_Click);
            BtnDeleteServer = new Button();
            BtnDeleteServer.Click += new EventHandler(BtnDeleteServer_Click);
            btnEnableDisable = new Button();
            btnEnableDisable.Click += new EventHandler(BtnEnableDisable_Click);
            BtnExport = new Button();
            BtnExport.Click += new EventHandler(BtnExport_Click);
            BtnImport = new Button();
            BtnImport.Click += new EventHandler(BtnImport_Click);
            btnDeleteAll = new Button();
            btnDeleteAll.Click += new EventHandler(btnDeleteAll_Click);
            BtnDown = new Button();
            BtnDown.Click += new EventHandler(BtnDown_Click);
            BtnUp = new Button();
            BtnUp.Click += new EventHandler(BtnUp_Click);
            txtName = new TextBox();
            Label3 = new Label();
            chkEnabled = new CheckBox();
            txtPort = new TextBox();
            Label2 = new Label();
            txtIP = new TextBox();
            Label1 = new Label();
            SeparatingLine = new Label();
            Label4 = new Label();
            BtnCancel = new Button();
            BtnCancel.Click += new EventHandler(BtnCancel_Click);
            ContextMenuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // servers
            // 
            servers.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            servers.Columns.AddRange(new ColumnHeader[] { colServer, colPort, colEnabled, colName });
            servers.ContextMenuStrip = ContextMenuStrip1;
            servers.FullRowSelect = true;
            servers.HideSelection = false;
            servers.Location = new Point(12, 12);
            servers.Name = "servers";
            servers.Size = new Size(587, 200);
            servers.TabIndex = 0;
            servers.UseCompatibleStateImageBehavior = false;
            servers.View = View.Details;
            // 
            // colServer
            // 
            colServer.Text = "Server IP";
            colServer.Width = 250;
            // 
            // colPort
            // 
            colPort.Text = "Server Port";
            colPort.Width = 80;
            // 
            // colEnabled
            // 
            colEnabled.Text = "Enabled";
            // 
            // colName
            // 
            colName.Text = "Server Name";
            colName.Width = 180;
            // 
            // ContextMenuStrip1
            // 
            ContextMenuStrip1.Items.AddRange(new ToolStripItem[] { EnableDisableToolStripMenuItem });
            ContextMenuStrip1.Name = "ContextMenuStrip1";
            ContextMenuStrip1.Size = new Size(153, 26);
            // 
            // EnableDisableToolStripMenuItem
            // 
            EnableDisableToolStripMenuItem.Name = "EnableDisableToolStripMenuItem";
            EnableDisableToolStripMenuItem.Size = new Size(152, 22);
            EnableDisableToolStripMenuItem.Text = "Enable/Disable";
            // 
            // BtnAddServer
            // 
            BtnAddServer.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            BtnAddServer.Location = new Point(12, 365);
            BtnAddServer.Name = "BtnAddServer";
            BtnAddServer.Size = new Size(75, 23);
            BtnAddServer.TabIndex = 1;
            BtnAddServer.Text = "Add Server";
            BtnAddServer.UseVisualStyleBackColor = true;
            // 
            // BtnEditServer
            // 
            BtnEditServer.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            BtnEditServer.Enabled = false;
            BtnEditServer.Location = new Point(12, 218);
            BtnEditServer.Name = "BtnEditServer";
            BtnEditServer.Size = new Size(75, 23);
            BtnEditServer.TabIndex = 2;
            BtnEditServer.Text = "Edit Server";
            BtnEditServer.UseVisualStyleBackColor = true;
            // 
            // BtnDeleteServer
            // 
            BtnDeleteServer.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            BtnDeleteServer.Enabled = false;
            BtnDeleteServer.Location = new Point(93, 218);
            BtnDeleteServer.Name = "BtnDeleteServer";
            BtnDeleteServer.Size = new Size(82, 23);
            BtnDeleteServer.TabIndex = 3;
            BtnDeleteServer.Text = "Delete Server";
            BtnDeleteServer.UseVisualStyleBackColor = true;
            // 
            // btnEnableDisable
            // 
            btnEnableDisable.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnEnableDisable.Enabled = false;
            btnEnableDisable.Location = new Point(181, 218);
            btnEnableDisable.Name = "btnEnableDisable";
            btnEnableDisable.Size = new Size(75, 23);
            btnEnableDisable.TabIndex = 4;
            btnEnableDisable.Text = "Enable";
            btnEnableDisable.UseVisualStyleBackColor = true;
            // 
            // BtnExport
            // 
            BtnExport.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            BtnExport.Location = new Point(469, 218);
            BtnExport.Name = "BtnExport";
            BtnExport.Size = new Size(75, 23);
            BtnExport.TabIndex = 14;
            BtnExport.Text = "Export";
            BtnExport.UseVisualStyleBackColor = true;
            // 
            // BtnImport
            // 
            BtnImport.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            BtnImport.Location = new Point(550, 218);
            BtnImport.Name = "BtnImport";
            BtnImport.Size = new Size(75, 23);
            BtnImport.TabIndex = 13;
            BtnImport.Text = "Import";
            BtnImport.UseVisualStyleBackColor = true;
            // 
            // btnDeleteAll
            // 
            btnDeleteAll.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnDeleteAll.Location = new Point(262, 218);
            btnDeleteAll.Name = "btnDeleteAll";
            btnDeleteAll.Size = new Size(75, 23);
            btnDeleteAll.TabIndex = 17;
            btnDeleteAll.Text = "Delete All";
            btnDeleteAll.UseVisualStyleBackColor = true;
            // 
            // BtnDown
            // 
            BtnDown.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            BtnDown.Location = new Point(605, 189);
            BtnDown.Name = "BtnDown";
            BtnDown.Size = new Size(24, 23);
            BtnDown.TabIndex = 19;
            BtnDown.Text = "▼";
            BtnDown.UseVisualStyleBackColor = true;
            // 
            // BtnUp
            // 
            BtnUp.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            BtnUp.Location = new Point(605, 12);
            BtnUp.Name = "BtnUp";
            BtnUp.Size = new Size(24, 23);
            BtnUp.TabIndex = 18;
            BtnUp.Text = "▲";
            BtnUp.UseVisualStyleBackColor = true;
            // 
            // txtName
            // 
            txtName.Location = new Point(76, 339);
            txtName.Name = "txtName";
            txtName.Size = new Size(553, 20);
            txtName.TabIndex = 26;
            // 
            // Label3
            // 
            Label3.AutoSize = true;
            Label3.Location = new Point(35, 342);
            Label3.Name = "Label3";
            Label3.Size = new Size(35, 13);
            Label3.TabIndex = 25;
            Label3.Text = "Name";
            // 
            // chkEnabled
            // 
            chkEnabled.AutoSize = true;
            chkEnabled.Checked = true;
            chkEnabled.CheckState = CheckState.Checked;
            chkEnabled.Location = new Point(149, 316);
            chkEnabled.Name = "chkEnabled";
            chkEnabled.Size = new Size(71, 17);
            chkEnabled.TabIndex = 24;
            chkEnabled.Text = "Enabled?";
            chkEnabled.UseVisualStyleBackColor = true;
            // 
            // txtPort
            // 
            txtPort.Location = new Point(76, 313);
            txtPort.Name = "txtPort";
            txtPort.Size = new Size(57, 20);
            txtPort.TabIndex = 23;
            // 
            // Label2
            // 
            Label2.AutoSize = true;
            Label2.Location = new Point(44, 316);
            Label2.Name = "Label2";
            Label2.Size = new Size(26, 13);
            Label2.TabIndex = 22;
            Label2.Text = "Port";
            // 
            // txtIP
            // 
            txtIP.Location = new Point(76, 287);
            txtIP.Name = "txtIP";
            txtIP.Size = new Size(553, 20);
            txtIP.TabIndex = 21;
            // 
            // Label1
            // 
            Label1.AutoSize = true;
            Label1.Location = new Point(12, 290);
            Label1.Name = "Label1";
            Label1.Size = new Size(58, 13);
            Label1.TabIndex = 20;
            Label1.Text = "IP Address";
            // 
            // SeparatingLine
            // 
            SeparatingLine.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            SeparatingLine.BackColor = Color.Black;
            SeparatingLine.Location = new Point(-1, 250);
            SeparatingLine.Name = "SeparatingLine";
            SeparatingLine.Size = new Size(650, 1);
            SeparatingLine.TabIndex = 27;
            // 
            // Label4
            // 
            Label4.AutoSize = true;
            Label4.Location = new Point(12, 265);
            Label4.Name = "Label4";
            Label4.Size = new Size(60, 13);
            Label4.TabIndex = 28;
            Label4.Text = "Add Server";
            // 
            // BtnCancel
            // 
            BtnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            BtnCancel.Location = new Point(93, 365);
            BtnCancel.Name = "BtnCancel";
            BtnCancel.Size = new Size(75, 23);
            BtnCancel.TabIndex = 40;
            BtnCancel.Text = "Cancel";
            BtnCancel.UseVisualStyleBackColor = true;
            // 
            // ConfigureSysLogMirrorClients
            // 
            AutoScaleDimensions = new SizeF(6.0f, 13.0f);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(637, 400);
            Controls.Add(BtnCancel);
            Controls.Add(Label4);
            Controls.Add(SeparatingLine);
            Controls.Add(txtName);
            Controls.Add(Label3);
            Controls.Add(chkEnabled);
            Controls.Add(txtPort);
            Controls.Add(Label2);
            Controls.Add(txtIP);
            Controls.Add(Label1);
            Controls.Add(BtnDown);
            Controls.Add(BtnUp);
            Controls.Add(btnDeleteAll);
            Controls.Add(BtnExport);
            Controls.Add(BtnImport);
            Controls.Add(btnEnableDisable);
            Controls.Add(BtnDeleteServer);
            Controls.Add(BtnEditServer);
            Controls.Add(BtnAddServer);
            Controls.Add(servers);
            KeyPreview = true;
            MaximizeBox = false;
            MinimizeBox = false;
            MinimumSize = new Size(653, 439);
            Name = "ConfigureSysLogMirrorClients";
            Text = "Configure SysLog Mirror Clients";
            ContextMenuStrip1.ResumeLayout(false);
            Load += new EventHandler(ConfigureSysLogMirrorServers_Load);
            FormClosing += new FormClosingEventHandler(ConfigureSysLogMirrorServers_FormClosing);
            KeyUp += new KeyEventHandler(ConfigureSysLogMirrorServers_KeyUp);
            ResumeLayout(false);
            PerformLayout();

        }

        internal ListView servers;
        internal ColumnHeader colServer;
        internal ColumnHeader colPort;
        internal Button BtnAddServer;
        internal Button BtnEditServer;
        internal Button BtnDeleteServer;
        internal ColumnHeader colEnabled;
        internal ContextMenuStrip ContextMenuStrip1;
        internal ToolStripMenuItem EnableDisableToolStripMenuItem;
        internal Button btnEnableDisable;
        internal ColumnHeader colName;
        internal Button BtnExport;
        internal Button BtnImport;
        internal Button btnDeleteAll;
        internal Button BtnDown;
        internal Button BtnUp;
        internal TextBox txtName;
        internal Label Label3;
        internal CheckBox chkEnabled;
        internal TextBox txtPort;
        internal Label Label2;
        internal TextBox txtIP;
        internal Label Label1;
        internal Label SeparatingLine;
        internal Label Label4;
        internal Button BtnCancel;
    }
}