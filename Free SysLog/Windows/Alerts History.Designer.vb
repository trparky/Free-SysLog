<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Alerts_History
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
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
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.AlertHistoryList = New System.Windows.Forms.DataGridView()
        Me.colTime = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colAlertType = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colAlert = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.BtnRefresh = New System.Windows.Forms.Button()
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.lblNumberOfAlerts = New System.Windows.Forms.ToolStripStatusLabel()
        CType(Me.AlertHistoryList, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.StatusStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'AlertHistoryList
        '
        Me.AlertHistoryList.AllowUserToAddRows = False
        Me.AlertHistoryList.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.AlertHistoryList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.AlertHistoryList.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.colTime, Me.colAlertType, Me.colAlert})
        Me.AlertHistoryList.Location = New System.Drawing.Point(12, 12)
        Me.AlertHistoryList.Name = "AlertHistoryList"
        Me.AlertHistoryList.ReadOnly = True
        Me.AlertHistoryList.RowHeadersVisible = False
        Me.AlertHistoryList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.AlertHistoryList.Size = New System.Drawing.Size(1227, 381)
        Me.AlertHistoryList.TabIndex = 0
        '
        'colTime
        '
        Me.colTime.HeaderText = "Time"
        Me.colTime.Name = "colTime"
        Me.colTime.ReadOnly = True
        '
        'colAlertType
        '
        Me.colAlertType.HeaderText = "Type"
        Me.colAlertType.Name = "colAlertType"
        Me.colAlertType.ReadOnly = True
        '
        'colAlert
        '
        Me.colAlert.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        Me.colAlert.HeaderText = "Alert Text"
        Me.colAlert.Name = "colAlert"
        Me.colAlert.ReadOnly = True
        '
        'BtnRefresh
        '
        Me.BtnRefresh.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.BtnRefresh.Location = New System.Drawing.Point(1139, 402)
        Me.BtnRefresh.Name = "BtnRefresh"
        Me.BtnRefresh.Size = New System.Drawing.Size(100, 23)
        Me.BtnRefresh.TabIndex = 1
        Me.BtnRefresh.Text = "&Refresh (F5)"
        Me.BtnRefresh.UseVisualStyleBackColor = True
        '
        'StatusStrip1
        '
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.lblNumberOfAlerts})
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 428)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New System.Drawing.Size(1251, 22)
        Me.StatusStrip1.TabIndex = 2
        Me.StatusStrip1.Text = "StatusStrip1"
        '
        'lblNumberOfAlerts
        '
        Me.lblNumberOfAlerts.Name = "lblNumberOfAlerts"
        Me.lblNumberOfAlerts.Size = New System.Drawing.Size(101, 17)
        Me.lblNumberOfAlerts.Text = "Number of Alerts:"
        '
        'Alerts_History
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1251, 450)
        Me.Controls.Add(Me.StatusStrip1)
        Me.Controls.Add(Me.BtnRefresh)
        Me.Controls.Add(Me.AlertHistoryList)
        Me.KeyPreview = True
        Me.MinimumSize = New System.Drawing.Size(1267, 489)
        Me.Name = "Alerts_History"
        Me.Text = "Alerts History"
        CType(Me.AlertHistoryList, System.ComponentModel.ISupportInitialize).EndInit()
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents AlertHistoryList As DataGridView
    Friend WithEvents colTime As DataGridViewTextBoxColumn
    Friend WithEvents colAlertType As DataGridViewTextBoxColumn
    Friend WithEvents colAlert As DataGridViewTextBoxColumn
    Friend WithEvents BtnRefresh As Button
    Friend WithEvents StatusStrip1 As StatusStrip
    Friend WithEvents lblNumberOfAlerts As ToolStripStatusLabel
End Class
