<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
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
        Me.btnServerController = New System.Windows.Forms.Button()
        Me.btnOpenLogLocation = New System.Windows.Forms.Button()
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.logs = New System.Windows.Forms.ListView()
        Me.Time = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.Type = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.IPAddressCol = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.Log = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.SaveFileDialog = New System.Windows.Forms.SaveFileDialog()
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.NumberOfLogs = New System.Windows.Forms.ToolStripStatusLabel()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.StatusStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnServerController
        '
        Me.btnServerController.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnServerController.Location = New System.Drawing.Point(3, 3)
        Me.btnServerController.Name = "btnServerController"
        Me.btnServerController.Size = New System.Drawing.Size(569, 23)
        Me.btnServerController.TabIndex = 0
        Me.btnServerController.Text = "Stop SysLog Server"
        Me.btnServerController.UseVisualStyleBackColor = True
        '
        'btnOpenLogLocation
        '
        Me.btnOpenLogLocation.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnOpenLogLocation.Location = New System.Drawing.Point(578, 3)
        Me.btnOpenLogLocation.Name = "btnOpenLogLocation"
        Me.btnOpenLogLocation.Size = New System.Drawing.Size(570, 23)
        Me.btnOpenLogLocation.TabIndex = 1
        Me.btnOpenLogLocation.Text = "Open Log File Location"
        Me.btnOpenLogLocation.UseVisualStyleBackColor = True
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel1.ColumnCount = 2
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.btnServerController, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.btnOpenLogLocation, 1, 0)
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(12, 12)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 1
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(1151, 29)
        Me.TableLayoutPanel1.TabIndex = 2
        '
        'logs
        '
        Me.logs.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.logs.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.Time, Me.Type, Me.IPAddressCol, Me.Log})
        Me.logs.FullRowSelect = True
        Me.logs.HideSelection = False
        Me.logs.Location = New System.Drawing.Point(12, 47)
        Me.logs.Name = "logs"
        Me.logs.Size = New System.Drawing.Size(1151, 374)
        Me.logs.TabIndex = 3
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
        'StatusStrip1
        '
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.NumberOfLogs})
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 424)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New System.Drawing.Size(1175, 22)
        Me.StatusStrip1.TabIndex = 4
        Me.StatusStrip1.Text = "StatusStrip1"
        '
        'NumberOfLogs
        '
        Me.NumberOfLogs.Name = "NumberOfLogs"
        Me.NumberOfLogs.Size = New System.Drawing.Size(137, 17)
        Me.NumberOfLogs.Text = "Number of Log Entries: 0"
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1175, 446)
        Me.Controls.Add(Me.StatusStrip1)
        Me.Controls.Add(Me.logs)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.Name = "Form1"
        Me.Text = "Free SysLog Server"
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnServerController As System.Windows.Forms.Button
    Friend WithEvents btnOpenLogLocation As System.Windows.Forms.Button
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents logs As System.Windows.Forms.ListView
    Friend WithEvents Time As System.Windows.Forms.ColumnHeader
    Friend WithEvents Type As System.Windows.Forms.ColumnHeader
    Friend WithEvents IPAddressCol As System.Windows.Forms.ColumnHeader
    Friend WithEvents Log As System.Windows.Forms.ColumnHeader
    Friend WithEvents SaveFileDialog As SaveFileDialog
    Friend WithEvents StatusStrip1 As StatusStrip
    Friend WithEvents NumberOfLogs As ToolStripStatusLabel
End Class
