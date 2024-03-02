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

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.logs = New System.Windows.Forms.DataGridView()
        Me.colTime = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colType = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colIPAddress = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colLog = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.lblCount = New System.Windows.Forms.ToolStripStatusLabel()
        Me.StatusStrip1.SuspendLayout()
        CType(Me.logs, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'StatusStrip1
        '
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.lblCount})
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 403)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New System.Drawing.Size(1152, 22)
        Me.StatusStrip1.TabIndex = 5
        Me.StatusStrip1.Text = "StatusStrip1"
        '
        'lblCount
        '
        Me.lblCount.Name = "lblCount"
        Me.lblCount.Size = New System.Drawing.Size(53, 17)
        Me.lblCount.Text = "lblCount"
        '
        'logs
        '
        Me.logs.AllowUserToAddRows = False
        Me.logs.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.logs.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.logs.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.colTime, Me.colType, Me.colIPAddress, Me.colLog})
        Me.logs.Location = New System.Drawing.Point(12, 12)
        Me.logs.Name = "logs"
        Me.logs.ReadOnly = True
        Me.logs.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.logs.Size = New System.Drawing.Size(1128, 388)
        Me.logs.TabIndex = 19
        '
        'colTime
        '
        Me.colTime.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
        Me.colTime.HeaderCell.Style.Padding = New Padding(0, 0, 1, 0)
        Me.colTime.HeaderText = "Time"
        Me.colTime.Name = "colTime"
        Me.colTime.ReadOnly = True
        Me.colTime.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic
        '
        'colType
        '
        Me.colType.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
        Me.colType.HeaderCell.Style.Padding = New Padding(0, 0, 2, 0)
        Me.colType.HeaderText = "Type"
        Me.colType.Name = "colType"
        Me.colType.ReadOnly = True
        '
        'colIPAddress
        '
        Me.colIPAddress.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
        Me.colIPAddress.HeaderCell.Style.Padding = New Padding(0, 0, 2, 0)
        Me.colIPAddress.HeaderText = "IP Address"
        Me.colIPAddress.Name = "colIPAddress"
        Me.colIPAddress.ReadOnly = True
        '
        'colLog
        '
        Me.colLog.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        Me.colLog.HeaderText = "Log"
        Me.colLog.Name = "colLog"
        Me.colLog.ReadOnly = True
        '
        'Ignored_Logs_and_Search_Results
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1152, 425)
        Me.Controls.Add(Me.logs)
        Me.Controls.Add(Me.StatusStrip1)
        Me.Name = "Ignored_Logs_and_Search_Results"
        Me.Text = "Ignored Logs"
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        CType(Me.logs, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents StatusStrip1 As StatusStrip
    Friend WithEvents lblCount As ToolStripStatusLabel
    Friend WithEvents logs As DataGridView
    Friend WithEvents colTime As DataGridViewTextBoxColumn
    Friend WithEvents colType As DataGridViewTextBoxColumn
    Friend WithEvents colIPAddress As DataGridViewTextBoxColumn
    Friend WithEvents colLog As DataGridViewTextBoxColumn
End Class
