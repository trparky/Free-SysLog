using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Free_SysLog
{
    [Microsoft.VisualBasic.CompilerServices.DesignerGenerated()]
    public partial class Alerts : Form
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
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(Alerts));
            BtnEdit = new Button();
            BtnEdit.Click += new EventHandler(BtnEdit_Click);
            AlertsListView = new ListView();
            AlertsListView.KeyUp += new KeyEventHandler(AlertsListView_KeyUp);
            AlertsListView.Click += new EventHandler(AlertsListView_Click);
            AlertsListView.DoubleClick += new EventHandler(AlertsListView_DoubleClick);
            AlertsListView.ColumnWidthChanged += new ColumnWidthChangedEventHandler(AlertsListView_ColumnWidthChanged);
            AlertLogText = new ColumnHeader();
            AlertText = new ColumnHeader();
            Regex = new ColumnHeader();
            CaseSensitive = new ColumnHeader();
            AlertTypeColumn = new ColumnHeader();
            colLimit = new ColumnHeader();
            ColEnabled = new ColumnHeader();
            ListViewMenu = new ContextMenuStrip(components);
            ListViewMenu.Opening += new System.ComponentModel.CancelEventHandler(ListViewMenu_Opening);
            EnableDisableToolStripMenuItem = new ToolStripMenuItem();
            EnableDisableToolStripMenuItem.Click += new EventHandler(EnableDisableToolStripMenuItem_Click);
            BtnDelete = new Button();
            BtnDelete.Click += new EventHandler(BtnDelete_Click);
            BtnAdd = new Button();
            BtnAdd.Click += new EventHandler(BtnAdd_Click);
            BtnEnableDisable = new Button();
            BtnEnableDisable.Click += new EventHandler(BtnEnableDisable_Click);
            BtnExport = new Button();
            BtnExport.Click += new EventHandler(BtnExport_Click);
            BtnImport = new Button();
            BtnImport.Click += new EventHandler(BtnImport_Click);
            BtnDeleteAll = new Button();
            BtnDeleteAll.Click += new EventHandler(BtnDeleteAll_Click);
            BtnDown = new Button();
            BtnDown.Click += new EventHandler(BtnDown_Click);
            BtnUp = new Button();
            BtnUp.Click += new EventHandler(BtnUp_Click);
            SeparatingLine = new Label();
            lblRegExBackReferences = new Label();
            ChkEnabled = new CheckBox();
            IconPictureBox = new PictureBox();
            AlertTypeComboBox = new ComboBox();
            AlertTypeComboBox.SelectedIndexChanged += new EventHandler(AlertTypeComboBox_SelectedIndexChanged);
            Label3 = new Label();
            Label2 = new Label();
            TxtAlertText = new TextBox();
            ChkCaseSensitive = new CheckBox();
            ChkRegex = new CheckBox();
            TxtLogText = new TextBox();
            Label1 = new Label();
            Label4 = new Label();
            BtnCancel = new Button();
            BtnCancel.Click += new EventHandler(BtnCancel_Click);
            ChkLimited = new CheckBox();
            ToolTip = new ToolTip(components);
            ListViewMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)IconPictureBox).BeginInit();
            SuspendLayout();
            // 
            // BtnEdit
            // 
            BtnEdit.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            BtnEdit.Enabled = false;
            BtnEdit.Location = new Point(83, 248);
            BtnEdit.Name = "BtnEdit";
            BtnEdit.Size = new Size(75, 23);
            BtnEdit.TabIndex = 12;
            BtnEdit.Text = "Edit";
            BtnEdit.UseVisualStyleBackColor = true;
            // 
            // AlertsListView
            // 
            AlertsListView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            AlertsListView.Columns.AddRange(new ColumnHeader[] { AlertLogText, AlertText, Regex, CaseSensitive, AlertTypeColumn, colLimit, ColEnabled });
            AlertsListView.ContextMenuStrip = ListViewMenu;
            AlertsListView.FullRowSelect = true;
            AlertsListView.HideSelection = false;
            AlertsListView.Location = new Point(12, 12);
            AlertsListView.Name = "AlertsListView";
            AlertsListView.Size = new Size(933, 230);
            AlertsListView.TabIndex = 11;
            AlertsListView.UseCompatibleStateImageBehavior = false;
            AlertsListView.View = View.Details;
            // 
            // AlertLogText
            // 
            AlertLogText.Text = "Alert Log Text";
            AlertLogText.Width = 308;
            // 
            // AlertText
            // 
            AlertText.Text = "Alert Text";
            AlertText.Width = 257;
            // 
            // Regex
            // 
            Regex.Text = "Regex";
            // 
            // CaseSensitive
            // 
            CaseSensitive.Text = "Case Sensitive";
            CaseSensitive.Width = 91;
            // 
            // AlertTypeColumn
            // 
            AlertTypeColumn.Text = "Alert Type";
            AlertTypeColumn.Width = 90;
            // 
            // colLimit
            // 
            colLimit.DisplayIndex = 6;
            colLimit.Text = "Limited";
            // 
            // ColEnabled
            // 
            ColEnabled.DisplayIndex = 5;
            ColEnabled.Text = "Enabled";
            // 
            // ListViewMenu
            // 
            ListViewMenu.Items.AddRange(new ToolStripItem[] { EnableDisableToolStripMenuItem });
            ListViewMenu.Name = "ContextMenuStrip1";
            ListViewMenu.Size = new Size(153, 26);
            // 
            // EnableDisableToolStripMenuItem
            // 
            EnableDisableToolStripMenuItem.Name = "EnableDisableToolStripMenuItem";
            EnableDisableToolStripMenuItem.Size = new Size(152, 22);
            EnableDisableToolStripMenuItem.Text = "Enable/Disable";
            // 
            // BtnDelete
            // 
            BtnDelete.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            BtnDelete.Enabled = false;
            BtnDelete.Location = new Point(12, 248);
            BtnDelete.Name = "BtnDelete";
            BtnDelete.Size = new Size(65, 23);
            BtnDelete.TabIndex = 10;
            BtnDelete.Text = "Delete";
            BtnDelete.UseVisualStyleBackColor = true;
            // 
            // BtnAdd
            // 
            BtnAdd.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            BtnAdd.Location = new Point(12, 424);
            BtnAdd.Name = "BtnAdd";
            BtnAdd.Size = new Size(65, 23);
            BtnAdd.TabIndex = 9;
            BtnAdd.Text = "Add";
            BtnAdd.UseVisualStyleBackColor = true;
            // 
            // BtnEnableDisable
            // 
            BtnEnableDisable.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            BtnEnableDisable.Enabled = false;
            BtnEnableDisable.Location = new Point(164, 248);
            BtnEnableDisable.Name = "BtnEnableDisable";
            BtnEnableDisable.Size = new Size(75, 23);
            BtnEnableDisable.TabIndex = 13;
            BtnEnableDisable.Text = "Disable";
            BtnEnableDisable.UseVisualStyleBackColor = true;
            // 
            // BtnExport
            // 
            BtnExport.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            BtnExport.Location = new Point(819, 248);
            BtnExport.Name = "BtnExport";
            BtnExport.Size = new Size(75, 23);
            BtnExport.TabIndex = 15;
            BtnExport.Text = "Export";
            BtnExport.UseVisualStyleBackColor = true;
            // 
            // BtnImport
            // 
            BtnImport.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            BtnImport.Location = new Point(900, 248);
            BtnImport.Name = "BtnImport";
            BtnImport.Size = new Size(75, 23);
            BtnImport.TabIndex = 14;
            BtnImport.Text = "Import";
            BtnImport.UseVisualStyleBackColor = true;
            // 
            // BtnDeleteAll
            // 
            BtnDeleteAll.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            BtnDeleteAll.Location = new Point(245, 248);
            BtnDeleteAll.Name = "btnDeleteAll";
            BtnDeleteAll.Size = new Size(75, 23);
            BtnDeleteAll.TabIndex = 16;
            BtnDeleteAll.Text = "Delete All";
            BtnDeleteAll.UseVisualStyleBackColor = true;
            // 
            // BtnDown
            // 
            BtnDown.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            BtnDown.Location = new Point(951, 219);
            BtnDown.Name = "BtnDown";
            BtnDown.Size = new Size(24, 23);
            BtnDown.TabIndex = 21;
            BtnDown.Text = "▼";
            BtnDown.UseVisualStyleBackColor = true;
            // 
            // BtnUp
            // 
            BtnUp.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            BtnUp.Location = new Point(951, 12);
            BtnUp.Name = "BtnUp";
            BtnUp.Size = new Size(24, 23);
            BtnUp.TabIndex = 20;
            BtnUp.Text = "▲";
            BtnUp.UseVisualStyleBackColor = true;
            // 
            // SeparatingLine
            // 
            SeparatingLine.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            SeparatingLine.BackColor = Color.Black;
            SeparatingLine.Location = new Point(-1, 280);
            SeparatingLine.Name = "SeparatingLine";
            SeparatingLine.Size = new Size(1000, 1);
            SeparatingLine.TabIndex = 25;
            // 
            // lblRegExBackReferences
            // 
            lblRegExBackReferences.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            lblRegExBackReferences.AutoSize = true;
            lblRegExBackReferences.Location = new Point(196, 360);
            lblRegExBackReferences.Name = "lblRegExBackReferences";
            lblRegExBackReferences.Size = new Size(602, 26);
            lblRegExBackReferences.TabIndex = 37;
            lblRegExBackReferences.Text = resources.GetString("lblRegExBackReferences.Text");
            // 
            // ChkEnabled
            // 
            ChkEnabled.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            ChkEnabled.AutoSize = true;
            ChkEnabled.Checked = true;
            ChkEnabled.CheckState = CheckState.Checked;
            ChkEnabled.Location = new Point(678, 401);
            ChkEnabled.Name = "ChkEnabled";
            ChkEnabled.Size = new Size(71, 17);
            ChkEnabled.TabIndex = 36;
            ChkEnabled.Text = "Enabled?";
            ChkEnabled.UseVisualStyleBackColor = true;
            // 
            // IconPictureBox
            // 
            IconPictureBox.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            IconPictureBox.Location = new Point(158, 363);
            IconPictureBox.Name = "IconPictureBox";
            IconPictureBox.Size = new Size(32, 32);
            IconPictureBox.TabIndex = 35;
            IconPictureBox.TabStop = false;
            // 
            // AlertTypeComboBox
            // 
            AlertTypeComboBox.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            AlertTypeComboBox.FormattingEnabled = true;
            AlertTypeComboBox.Items.AddRange(new object[] { "Warning", "Error", "Information", "None" });
            AlertTypeComboBox.Location = new Point(73, 363);
            AlertTypeComboBox.Name = "AlertTypeComboBox";
            AlertTypeComboBox.Size = new Size(79, 21);
            AlertTypeComboBox.TabIndex = 34;
            // 
            // Label3
            // 
            Label3.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            Label3.AutoSize = true;
            Label3.Location = new Point(12, 366);
            Label3.Name = "Label3";
            Label3.Size = new Size(55, 13);
            Label3.TabIndex = 33;
            Label3.Text = "Alert Type";
            // 
            // Label2
            // 
            Label2.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            Label2.AutoSize = true;
            Label2.Location = new Point(12, 340);
            Label2.Name = "Label2";
            Label2.Size = new Size(52, 13);
            Label2.TabIndex = 32;
            Label2.Text = "Alert Text";
            // 
            // TxtAlertText
            // 
            TxtAlertText.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            TxtAlertText.Location = new Point(67, 337);
            TxtAlertText.Name = "TxtAlertText";
            TxtAlertText.Size = new Size(908, 20);
            TxtAlertText.TabIndex = 31;
            // 
            // ChkCaseSensitive
            // 
            ChkCaseSensitive.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            ChkCaseSensitive.AutoSize = true;
            ChkCaseSensitive.Location = new Point(499, 401);
            ChkCaseSensitive.Name = "ChkCaseSensitive";
            ChkCaseSensitive.Size = new Size(102, 17);
            ChkCaseSensitive.TabIndex = 30;
            ChkCaseSensitive.Text = "Case Sensitive?";
            ChkCaseSensitive.UseVisualStyleBackColor = true;
            // 
            // ChkRegex
            // 
            ChkRegex.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            ChkRegex.AutoSize = true;
            ChkRegex.Location = new Point(15, 401);
            ChkRegex.Name = "ChkRegex";
            ChkRegex.Size = new Size(478, 17);
            ChkRegex.TabIndex = 29;
            ChkRegex.Text = "Regex? (Be careful with Regex, a broken regex pattern could cause the program to " + "malfunction)";
            ChkRegex.UseVisualStyleBackColor = true;
            // 
            // TxtLogText
            // 
            TxtLogText.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            TxtLogText.Location = new Point(67, 311);
            TxtLogText.Name = "TxtLogText";
            TxtLogText.Size = new Size(908, 20);
            TxtLogText.TabIndex = 28;
            // 
            // Label1
            // 
            Label1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            Label1.AutoSize = true;
            Label1.Location = new Point(12, 314);
            Label1.Name = "Label1";
            Label1.Size = new Size(49, 13);
            Label1.TabIndex = 27;
            Label1.Text = "Log Text";
            Label1.TextAlign = ContentAlignment.MiddleRight;
            // 
            // Label4
            // 
            Label4.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            Label4.AutoSize = true;
            Label4.Location = new Point(12, 291);
            Label4.Name = "Label4";
            Label4.Size = new Size(50, 13);
            Label4.TabIndex = 38;
            Label4.Text = "Add Alert";
            // 
            // BtnCancel
            // 
            BtnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            BtnCancel.Location = new Point(83, 424);
            BtnCancel.Name = "BtnCancel";
            BtnCancel.Size = new Size(75, 23);
            BtnCancel.TabIndex = 39;
            BtnCancel.Text = "Cancel";
            BtnCancel.UseVisualStyleBackColor = true;
            // 
            // ChkLimited
            // 
            ChkLimited.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            ChkLimited.AutoSize = true;
            ChkLimited.Location = new Point(607, 401);
            ChkLimited.Name = "ChkLimited";
            ChkLimited.Size = new Size(65, 17);
            ChkLimited.TabIndex = 40;
            ChkLimited.Text = "Limited?";
            ToolTip.SetToolTip(ChkLimited, "Tells the program if this alert type should be limited by the notification limite" + "r.");
            ChkLimited.UseVisualStyleBackColor = true;
            // 
            // Alerts
            // 
            AutoScaleDimensions = new SizeF(6.0f, 13.0f);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(987, 458);
            Controls.Add(ChkLimited);
            Controls.Add(BtnCancel);
            Controls.Add(Label4);
            Controls.Add(lblRegExBackReferences);
            Controls.Add(ChkEnabled);
            Controls.Add(IconPictureBox);
            Controls.Add(AlertTypeComboBox);
            Controls.Add(Label3);
            Controls.Add(Label2);
            Controls.Add(TxtAlertText);
            Controls.Add(ChkCaseSensitive);
            Controls.Add(ChkRegex);
            Controls.Add(TxtLogText);
            Controls.Add(Label1);
            Controls.Add(SeparatingLine);
            Controls.Add(BtnDown);
            Controls.Add(BtnUp);
            Controls.Add(BtnDeleteAll);
            Controls.Add(BtnExport);
            Controls.Add(BtnImport);
            Controls.Add(BtnEnableDisable);
            Controls.Add(BtnEdit);
            Controls.Add(AlertsListView);
            Controls.Add(BtnDelete);
            Controls.Add(BtnAdd);
            KeyPreview = true;
            MinimumSize = new Size(1003, 497);
            Name = "Alerts";
            Text = "Alerts";
            ListViewMenu.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)IconPictureBox).EndInit();
            Load += new EventHandler(Alerts_Load);
            FormClosing += new FormClosingEventHandler(Alerts_FormClosing);
            ResizeEnd += new EventHandler(Alerts_ResizeEnd);
            KeyUp += new KeyEventHandler(Alerts_KeyUp);
            ResumeLayout(false);
            PerformLayout();

        }

        internal Button BtnEdit;
        internal ListView AlertsListView;
        internal ColumnHeader AlertLogText;
        internal ColumnHeader Regex;
        internal ColumnHeader CaseSensitive;
        internal Button BtnDelete;
        internal Button BtnAdd;
        internal ColumnHeader AlertText;
        internal ColumnHeader AlertTypeColumn;
        internal ColumnHeader ColEnabled;
        internal ContextMenuStrip ListViewMenu;
        internal ToolStripMenuItem EnableDisableToolStripMenuItem;
        internal Button BtnEnableDisable;
        internal Button BtnExport;
        internal Button BtnImport;
        internal Button BtnDeleteAll;
        internal Button BtnDown;
        internal Button BtnUp;
        internal Label SeparatingLine;
        internal Label lblRegExBackReferences;
        internal CheckBox ChkEnabled;
        internal PictureBox IconPictureBox;
        internal ComboBox AlertTypeComboBox;
        internal Label Label3;
        internal Label Label2;
        internal TextBox TxtAlertText;
        internal CheckBox ChkCaseSensitive;
        internal CheckBox ChkRegex;
        internal TextBox TxtLogText;
        internal Label Label1;
        internal Label Label4;
        internal Button BtnCancel;
        internal ColumnHeader colLimit;
        internal CheckBox ChkLimited;
        internal ToolTip ToolTip;
    }
}