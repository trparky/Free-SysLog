<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Ignored_Logs_and_Search_Results
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    Private Shadows parentForm As Form1

    Public Sub New(parentForm As Form1)
        InitializeComponent()
        Me.parentForm = parentForm
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.Logs = New System.Windows.Forms.DataGridView()
        Me.ColTime = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ColIPAddress = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ColLog = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.LblCount = New System.Windows.Forms.ToolStripStatusLabel()
        Me.StatusStrip1.SuspendLayout()
        Me.BtnExport = New System.Windows.Forms.Button()
        Me.SaveFileDialog = New System.Windows.Forms.SaveFileDialog()
        Me.BtnClearIgnoredLogs = New System.Windows.Forms.Button()
        CType(Me.Logs, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'StatusStrip1
        '
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.LblCount})
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 403)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New System.Drawing.Size(1152, 22)
        Me.StatusStrip1.TabIndex = 5
        Me.StatusStrip1.Text = "StatusStrip1"
        '
        'LblCount
        '
        Me.LblCount.Name = "lblCount"
        Me.LblCount.Size = New System.Drawing.Size(53, 17)
        Me.LblCount.Text = "lblCount"
        '
        'Logs
        '
        Me.Logs.AllowUserToAddRows = False
        Me.Logs.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Logs.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.Logs.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.ColTime, Me.ColIPAddress, Me.ColLog})
        Me.Logs.Location = New System.Drawing.Point(12, 12)
        Me.Logs.Name = "logs"
        Me.Logs.ReadOnly = True
        Me.Logs.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.Logs.Size = New System.Drawing.Size(1128, 359)
        Me.Logs.TabIndex = 19
        '
        'ColTime
        '
        Me.ColTime.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
        Me.ColTime.HeaderCell.Style.Padding = New Padding(0, 0, 1, 0)
        Me.ColTime.HeaderText = "Time"
        Me.ColTime.Name = "colTime"
        Me.ColTime.ReadOnly = True
        Me.ColTime.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic
        '
        'ColIPAddress
        '
        Me.ColIPAddress.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
        Me.ColIPAddress.HeaderCell.Style.Padding = New Padding(0, 0, 2, 0)
        Me.ColIPAddress.HeaderText = "IP Address"
        Me.ColIPAddress.Name = "colIPAddress"
        Me.ColIPAddress.ReadOnly = True
        '
        'ColLog
        '
        Me.ColLog.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        Me.ColLog.HeaderText = "Log"
        Me.ColLog.Name = "colLog"
        Me.ColLog.ReadOnly = True
        '
        'BtnExport
        '
        Me.BtnExport.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.BtnExport.Location = New System.Drawing.Point(1065, 377)
        Me.BtnExport.Name = "BtnExport"
        Me.BtnExport.Size = New System.Drawing.Size(75, 23)
        Me.BtnExport.TabIndex = 20
        Me.BtnExport.Text = "Export"
        Me.BtnExport.UseVisualStyleBackColor = True
        '
        'BtnClearIgnoredLogs
        '
        Me.BtnClearIgnoredLogs.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.BtnClearIgnoredLogs.Location = New System.Drawing.Point(923, 377)
        Me.BtnClearIgnoredLogs.Name = "BtnClearIgnoredLogs"
        Me.BtnClearIgnoredLogs.Size = New System.Drawing.Size(136, 23)
        Me.BtnClearIgnoredLogs.TabIndex = 21
        Me.BtnClearIgnoredLogs.Text = "Clear Ignored Logs"
        Me.BtnClearIgnoredLogs.UseVisualStyleBackColor = True
        '
        'Ignored_Logs_and_Search_Results
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1152, 425)
        Me.Controls.Add(Me.BtnExport)
        Me.Controls.Add(Me.Logs)
        Me.Controls.Add(Me.StatusStrip1)
        Me.Controls.Add(Me.BtnClearIgnoredLogs)
        Me.MinimumSize = New System.Drawing.Size(1168, 464)
        Me.Name = "Ignored_Logs_and_Search_Results"
        Me.Text = "Ignored Logs"
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        CType(Me.Logs, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents StatusStrip1 As StatusStrip
    Friend WithEvents LblCount As ToolStripStatusLabel
    Friend WithEvents Logs As DataGridView
    Friend WithEvents ColTime As DataGridViewTextBoxColumn
    Friend WithEvents ColIPAddress As DataGridViewTextBoxColumn
    Friend WithEvents ColLog As DataGridViewTextBoxColumn
    Friend WithEvents BtnExport As Button
    Friend WithEvents SaveFileDialog As SaveFileDialog
    Friend WithEvents BtnClearIgnoredLogs As Button
End Class
