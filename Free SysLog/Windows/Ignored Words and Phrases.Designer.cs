using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Free_SysLog
{
    [Microsoft.VisualBasic.CompilerServices.DesignerGenerated()]
    public partial class IgnoredWordsAndPhrases : Form
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
            BtnAdd = new Button();
            BtnAdd.Click += new EventHandler(BtnAdd_Click);
            BtnDelete = new Button();
            BtnDelete.Click += new EventHandler(BtnDelete_Click);
            IgnoredListView = new ListView();
            IgnoredListView.KeyUp += new KeyEventHandler(IgnoredListView_KeyUp);
            IgnoredListView.Click += new EventHandler(IgnoredListView_Click);
            IgnoredListView.DoubleClick += new EventHandler(IgnoredListView_DoubleClick);
            IgnoredListView.ColumnWidthChanged += new ColumnWidthChangedEventHandler(IgnoredListView_ColumnWidthChanged);
            Ignored = new ColumnHeader();
            Regex = new ColumnHeader();
            CaseSensitive = new ColumnHeader();
            ColEnabled = new ColumnHeader();
            ListViewMenu = new ContextMenuStrip(components);
            ListViewMenu.Opening += new System.ComponentModel.CancelEventHandler(ListViewMenu_Opening);
            EnableDisableToolStripMenuItem = new ToolStripMenuItem();
            EnableDisableToolStripMenuItem.Click += new EventHandler(EnableDisableToolStripMenuItem_Click);
            BtnEdit = new Button();
            BtnEdit.Click += new EventHandler(BtnEdit_Click);
            BtnEnableDisable = new Button();
            BtnEnableDisable.Click += new EventHandler(BtnEnableDisable_Click);
            BtnImport = new Button();
            BtnImport.Click += new EventHandler(BtnImport_Click);
            BtnExport = new Button();
            BtnExport.Click += new EventHandler(BtnExport_Click);
            btnDeleteAll = new Button();
            btnDeleteAll.Click += new EventHandler(btnDeleteAll_Click);
            BtnDown = new Button();
            BtnDown.Click += new EventHandler(BtnDown_Click);
            BtnUp = new Button();
            BtnUp.Click += new EventHandler(BtnUp_Click);
            SeparatingLine = new Label();
            Label4 = new Label();
            ChkEnabled = new CheckBox();
            ChkCaseSensitive = new CheckBox();
            ChkRegex = new CheckBox();
            TxtIgnored = new TextBox();
            Label1 = new Label();
            BtnCancel = new Button();
            BtnCancel.Click += new EventHandler(BtnCancel_Click);
            ListViewMenu.SuspendLayout();
            SuspendLayout();
            // 
            // BtnAdd
            // 
            BtnAdd.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            BtnAdd.Location = new Point(12, 348);
            BtnAdd.Name = "BtnAdd";
            BtnAdd.Size = new Size(65, 23);
            BtnAdd.TabIndex = 1;
            BtnAdd.Text = "Add";
            BtnAdd.UseVisualStyleBackColor = true;
            // 
            // BtnDelete
            // 
            BtnDelete.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            BtnDelete.Enabled = false;
            BtnDelete.Location = new Point(12, 225);
            BtnDelete.Name = "BtnDelete";
            BtnDelete.Size = new Size(65, 23);
            BtnDelete.TabIndex = 2;
            BtnDelete.Text = "Delete";
            BtnDelete.UseVisualStyleBackColor = true;
            // 
            // IgnoredListView
            // 
            IgnoredListView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            IgnoredListView.Columns.AddRange(new ColumnHeader[] { Ignored, Regex, CaseSensitive, ColEnabled });
            IgnoredListView.ContextMenuStrip = ListViewMenu;
            IgnoredListView.FullRowSelect = true;
            IgnoredListView.HideSelection = false;
            IgnoredListView.Location = new Point(12, 12);
            IgnoredListView.Name = "IgnoredListView";
            IgnoredListView.Size = new Size(745, 207);
            IgnoredListView.TabIndex = 5;
            IgnoredListView.UseCompatibleStateImageBehavior = false;
            IgnoredListView.View = View.Details;
            // 
            // Ignored
            // 
            Ignored.Text = "Ignored Word and/or Phrase";
            Ignored.Width = 345;
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
            // ColEnabled
            // 
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
            // BtnEdit
            // 
            BtnEdit.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            BtnEdit.Enabled = false;
            BtnEdit.Location = new Point(83, 225);
            BtnEdit.Name = "BtnEdit";
            BtnEdit.Size = new Size(75, 23);
            BtnEdit.TabIndex = 8;
            BtnEdit.Text = "Edit";
            BtnEdit.UseVisualStyleBackColor = true;
            // 
            // BtnEnableDisable
            // 
            BtnEnableDisable.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            BtnEnableDisable.Enabled = false;
            BtnEnableDisable.Location = new Point(164, 225);
            BtnEnableDisable.Name = "BtnEnableDisable";
            BtnEnableDisable.Size = new Size(75, 23);
            BtnEnableDisable.TabIndex = 10;
            BtnEnableDisable.Text = "Disable";
            BtnEnableDisable.UseVisualStyleBackColor = true;
            // 
            // BtnImport
            // 
            BtnImport.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            BtnImport.Location = new Point(712, 225);
            BtnImport.Name = "BtnImport";
            BtnImport.Size = new Size(75, 23);
            BtnImport.TabIndex = 11;
            BtnImport.Text = "Import";
            BtnImport.UseVisualStyleBackColor = true;
            // 
            // BtnExport
            // 
            BtnExport.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            BtnExport.Location = new Point(631, 225);
            BtnExport.Name = "BtnExport";
            BtnExport.Size = new Size(75, 23);
            BtnExport.TabIndex = 12;
            BtnExport.Text = "Export";
            BtnExport.UseVisualStyleBackColor = true;
            // 
            // btnDeleteAll
            // 
            btnDeleteAll.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnDeleteAll.Location = new Point(245, 225);
            btnDeleteAll.Name = "btnDeleteAll";
            btnDeleteAll.Size = new Size(75, 23);
            btnDeleteAll.TabIndex = 17;
            btnDeleteAll.Text = "Delete All";
            btnDeleteAll.UseVisualStyleBackColor = true;
            // 
            // BtnDown
            // 
            BtnDown.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            BtnDown.Location = new Point(764, 196);
            BtnDown.Name = "BtnDown";
            BtnDown.Size = new Size(24, 23);
            BtnDown.TabIndex = 19;
            BtnDown.Text = "▼";
            BtnDown.UseVisualStyleBackColor = true;
            // 
            // BtnUp
            // 
            BtnUp.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            BtnUp.Location = new Point(763, 12);
            BtnUp.Name = "BtnUp";
            BtnUp.Size = new Size(24, 23);
            BtnUp.TabIndex = 18;
            BtnUp.Text = "▲";
            BtnUp.UseVisualStyleBackColor = true;
            // 
            // SeparatingLine
            // 
            SeparatingLine.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            SeparatingLine.BackColor = Color.Black;
            SeparatingLine.Location = new Point(-1, 260);
            SeparatingLine.Name = "SeparatingLine";
            SeparatingLine.Size = new Size(805, 1);
            SeparatingLine.TabIndex = 26;
            // 
            // Label4
            // 
            Label4.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            Label4.AutoSize = true;
            Label4.Location = new Point(12, 273);
            Label4.Name = "Label4";
            Label4.Size = new Size(161, 13);
            Label4.TabIndex = 39;
            Label4.Text = "Add Ignored Words and Phrases";
            // 
            // ChkEnabled
            // 
            ChkEnabled.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            ChkEnabled.AutoSize = true;
            ChkEnabled.Checked = true;
            ChkEnabled.CheckState = CheckState.Checked;
            ChkEnabled.Location = new Point(607, 325);
            ChkEnabled.Name = "ChkEnabled";
            ChkEnabled.Size = new Size(71, 17);
            ChkEnabled.TabIndex = 44;
            ChkEnabled.Text = "Enabled?";
            ChkEnabled.UseVisualStyleBackColor = true;
            // 
            // ChkCaseSensitive
            // 
            ChkCaseSensitive.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            ChkCaseSensitive.AutoSize = true;
            ChkCaseSensitive.Location = new Point(499, 325);
            ChkCaseSensitive.Name = "ChkCaseSensitive";
            ChkCaseSensitive.Size = new Size(102, 17);
            ChkCaseSensitive.TabIndex = 43;
            ChkCaseSensitive.Text = "Case Sensitive?";
            ChkCaseSensitive.UseVisualStyleBackColor = true;
            // 
            // ChkRegex
            // 
            ChkRegex.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            ChkRegex.AutoSize = true;
            ChkRegex.Location = new Point(15, 325);
            ChkRegex.Name = "ChkRegex";
            ChkRegex.Size = new Size(478, 17);
            ChkRegex.TabIndex = 42;
            ChkRegex.Text = "Regex? (Be careful with Regex, a broken regex pattern could cause the program to " + "malfunction)";
            ChkRegex.UseVisualStyleBackColor = true;
            // 
            // TxtIgnored
            // 
            TxtIgnored.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            TxtIgnored.Location = new Point(61, 299);
            TxtIgnored.Name = "TxtIgnored";
            TxtIgnored.Size = new Size(727, 20);
            TxtIgnored.TabIndex = 41;
            // 
            // Label1
            // 
            Label1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            Label1.AutoSize = true;
            Label1.Location = new Point(12, 302);
            Label1.Name = "Label1";
            Label1.Size = new Size(43, 13);
            Label1.TabIndex = 40;
            Label1.Text = "Ignored";
            Label1.TextAlign = ContentAlignment.MiddleRight;
            // 
            // BtnCancel
            // 
            BtnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            BtnCancel.Location = new Point(83, 348);
            BtnCancel.Name = "BtnCancel";
            BtnCancel.Size = new Size(75, 23);
            BtnCancel.TabIndex = 45;
            BtnCancel.Text = "Cancel";
            BtnCancel.UseVisualStyleBackColor = true;
            // 
            // IgnoredWordsAndPhrases
            // 
            AutoScaleDimensions = new SizeF(6.0f, 13.0f);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(799, 378);
            Controls.Add(BtnCancel);
            Controls.Add(ChkEnabled);
            Controls.Add(ChkCaseSensitive);
            Controls.Add(ChkRegex);
            Controls.Add(TxtIgnored);
            Controls.Add(Label1);
            Controls.Add(Label4);
            Controls.Add(SeparatingLine);
            Controls.Add(BtnDown);
            Controls.Add(BtnUp);
            Controls.Add(btnDeleteAll);
            Controls.Add(BtnExport);
            Controls.Add(BtnImport);
            Controls.Add(BtnEnableDisable);
            Controls.Add(BtnEdit);
            Controls.Add(IgnoredListView);
            Controls.Add(BtnDelete);
            Controls.Add(BtnAdd);
            KeyPreview = true;
            MinimumSize = new Size(815, 417);
            Name = "IgnoredWordsAndPhrases";
            Text = "Ignored Words and Phrases";
            ListViewMenu.ResumeLayout(false);
            FormClosing += new FormClosingEventHandler(IgnoredWordsAndPhrases_FormClosing);
            Load += new EventHandler(IgnoredWordsAndPhrases_Load);
            ResizeEnd += new EventHandler(IgnoredWordsAndPhrases_ResizeEnd);
            KeyUp += new KeyEventHandler(IgnoredWordsAndPhrases_KeyUp);
            ResumeLayout(false);
            PerformLayout();

        }
        internal Button BtnAdd;
        internal Button BtnDelete;
        internal ListView IgnoredListView;
        internal ColumnHeader Ignored;
        internal ColumnHeader Regex;
        internal ColumnHeader CaseSensitive;
        internal Button BtnEdit;
        internal ColumnHeader ColEnabled;
        internal ContextMenuStrip ListViewMenu;
        internal ToolStripMenuItem EnableDisableToolStripMenuItem;
        internal Button BtnEnableDisable;
        internal Button BtnImport;
        internal Button BtnExport;
        internal Button btnDeleteAll;
        internal Button BtnDown;
        internal Button BtnUp;
        internal Label SeparatingLine;
        internal Label Label4;
        internal CheckBox ChkEnabled;
        internal CheckBox ChkCaseSensitive;
        internal CheckBox ChkRegex;
        internal TextBox TxtIgnored;
        internal Label Label1;
        internal Button BtnCancel;
    }
}