<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Alerts
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
        Me.BtnEdit = New System.Windows.Forms.Button()
        Me.AlertsListView = New System.Windows.Forms.ListView()
        Me.AlertLogText = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.AlertText = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.Regex = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.CaseSensitive = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.AlertTypeColumn = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.BtnDelete = New System.Windows.Forms.Button()
        Me.BtnAdd = New System.Windows.Forms.Button()
        Me.ColEnabled = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.SuspendLayout()
        '
        'BtnEdit
        '
        Me.BtnEdit.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.BtnEdit.Enabled = False
        Me.BtnEdit.Location = New System.Drawing.Point(154, 248)
        Me.BtnEdit.Name = "BtnEdit"
        Me.BtnEdit.Size = New System.Drawing.Size(75, 23)
        Me.BtnEdit.TabIndex = 12
        Me.BtnEdit.Text = "Edit"
        Me.BtnEdit.UseVisualStyleBackColor = True
        '
        'AlertsListView
        '
        Me.AlertsListView.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.AlertsListView.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.AlertLogText, Me.AlertText, Me.Regex, Me.CaseSensitive, Me.AlertTypeColumn, Me.ColEnabled})
        Me.AlertsListView.FullRowSelect = True
        Me.AlertsListView.HideSelection = False
        Me.AlertsListView.Location = New System.Drawing.Point(12, 12)
        Me.AlertsListView.Name = "AlertsListView"
        Me.AlertsListView.Size = New System.Drawing.Size(963, 230)
        Me.AlertsListView.TabIndex = 11
        Me.AlertsListView.UseCompatibleStateImageBehavior = False
        Me.AlertsListView.View = System.Windows.Forms.View.Details
        '
        'AlertLogText
        '
        Me.AlertLogText.Text = "Alert Log Text"
        Me.AlertLogText.Width = 308
        '
        'AlertText
        '
        Me.AlertText.Text = "Alert Text"
        Me.AlertText.Width = 257
        '
        'Regex
        '
        Me.Regex.Text = "Regex"
        '
        'CaseSensitive
        '
        Me.CaseSensitive.Text = "Case Sensitive"
        Me.CaseSensitive.Width = 91
        '
        'AlertTypeColumn
        '
        Me.AlertTypeColumn.Text = "Alert Type"
        Me.AlertTypeColumn.Width = 90
        '
        'BtnDelete
        '
        Me.BtnDelete.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.BtnDelete.Enabled = False
        Me.BtnDelete.Location = New System.Drawing.Point(83, 248)
        Me.BtnDelete.Name = "BtnDelete"
        Me.BtnDelete.Size = New System.Drawing.Size(65, 23)
        Me.BtnDelete.TabIndex = 10
        Me.BtnDelete.Text = "Delete"
        Me.BtnDelete.UseVisualStyleBackColor = True
        '
        'BtnAdd
        '
        Me.BtnAdd.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.BtnAdd.Location = New System.Drawing.Point(12, 248)
        Me.BtnAdd.Name = "BtnAdd"
        Me.BtnAdd.Size = New System.Drawing.Size(65, 23)
        Me.BtnAdd.TabIndex = 9
        Me.BtnAdd.Text = "Add"
        Me.BtnAdd.UseVisualStyleBackColor = True
        '
        'ColEnabled
        '
        Me.ColEnabled.Text = "Enabled"
        '
        'Alerts
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(987, 283)
        Me.Controls.Add(Me.BtnEdit)
        Me.Controls.Add(Me.AlertsListView)
        Me.Controls.Add(Me.BtnDelete)
        Me.Controls.Add(Me.BtnAdd)
        Me.Name = "Alerts"
        Me.Text = "Alerts"
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents BtnEdit As Button
    Friend WithEvents AlertsListView As ListView
    Friend WithEvents AlertLogText As ColumnHeader
    Friend WithEvents Regex As ColumnHeader
    Friend WithEvents CaseSensitive As ColumnHeader
    Friend WithEvents BtnDelete As Button
    Friend WithEvents BtnAdd As Button
    Friend WithEvents AlertText As ColumnHeader
    Friend WithEvents AlertTypeColumn As ColumnHeader
    Friend WithEvents ColEnabled As ColumnHeader
End Class
