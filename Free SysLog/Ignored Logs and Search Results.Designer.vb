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
        Me.logs = New System.Windows.Forms.ListView()
        Me.Time = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.Type = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.IPAddressCol = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.Log = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.SuspendLayout()
        '
        'logs
        '
        Me.logs.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.logs.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.Time, Me.Type, Me.IPAddressCol, Me.Log})
        Me.logs.FullRowSelect = True
        Me.logs.HideSelection = False
        Me.logs.Location = New System.Drawing.Point(12, 12)
        Me.logs.Name = "logs"
        Me.logs.Size = New System.Drawing.Size(1128, 401)
        Me.logs.TabIndex = 4
        Me.logs.UseCompatibleStateImageBehavior = False
        Me.logs.View = System.Windows.Forms.View.Details
        '
        'Time
        '
        Me.Time.Text = "Time"
        Me.Time.Width = 196
        '
        'Type
        '
        Me.Type.Text = "Type"
        Me.Type.Width = 110
        '
        'IPAddressCol
        '
        Me.IPAddressCol.Text = "IP Address"
        Me.IPAddressCol.Width = 102
        '
        'Log
        '
        Me.Log.Text = "Log"
        Me.Log.Width = 670
        '
        'Ignored_Logs_and_Search_Results
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1152, 425)
        Me.Controls.Add(Me.logs)
        Me.Name = "Ignored_Logs_and_Search_Results"
        Me.Text = "Ignored Logs"
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents logs As ListView
    Friend WithEvents Time As ColumnHeader
    Friend WithEvents Type As ColumnHeader
    Friend WithEvents IPAddressCol As ColumnHeader
    Friend WithEvents Log As ColumnHeader
End Class
