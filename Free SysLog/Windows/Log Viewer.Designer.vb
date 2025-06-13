<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class LogViewer
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
        Me.LogText = New System.Windows.Forms.TextBox()
        Me.BtnClose = New System.Windows.Forms.Button()
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.LblLogDate = New System.Windows.Forms.ToolStripStatusLabel()
        Me.LblSource = New System.Windows.Forms.ToolStripStatusLabel()
        Me.lblAlertText = New System.Windows.Forms.Label()
        Me.ChkShowRawLog = New System.Windows.Forms.CheckBox()
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.txtAlertText = New System.Windows.Forms.TextBox()
        Me.StatusStrip1.SuspendLayout()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'BtnClose
        '
        Me.BtnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.BtnClose.Location = New System.Drawing.Point(713, 415)
        Me.BtnClose.Name = "BtnClose"
        Me.BtnClose.Size = New System.Drawing.Size(75, 26)
        Me.BtnClose.TabIndex = 0
        Me.BtnClose.Text = "&Close"
        Me.BtnClose.UseVisualStyleBackColor = True
        '
        'StatusStrip1
        '
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.LblLogDate, Me.LblSource})
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 444)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New System.Drawing.Size(800, 22)
        Me.StatusStrip1.TabIndex = 4
        Me.StatusStrip1.Text = "StatusStrip1"
        '
        'LblLogDate
        '
        Me.LblLogDate.Name = "LblLogDate"
        Me.LblLogDate.Size = New System.Drawing.Size(57, 17)
        Me.LblLogDate.Text = "Log Date:"
        '
        'LblSource
        '
        Me.LblSource.Margin = New System.Windows.Forms.Padding(100, 3, 0, 2)
        Me.LblSource.Name = "LblSource"
        Me.LblSource.Size = New System.Drawing.Size(104, 17)
        Me.LblSource.Text = "Source IP Address:"
        '
        'lblAlertText
        '
        Me.lblAlertText.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblAlertText.AutoSize = True
        Me.lblAlertText.Location = New System.Drawing.Point(3, 180)
        Me.lblAlertText.Name = "lblAlertText"
        Me.lblAlertText.Size = New System.Drawing.Size(52, 18)
        Me.lblAlertText.TabIndex = 6
        Me.lblAlertText.Text = "Alert Text"
        Me.lblAlertText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ChkShowRawLog
        '
        Me.ChkShowRawLog.AutoSize = True
        Me.ChkShowRawLog.Location = New System.Drawing.Point(12, 7)
        Me.ChkShowRawLog.Name = "ChkShowRawLog"
        Me.ChkShowRawLog.Size = New System.Drawing.Size(123, 17)
        Me.ChkShowRawLog.TabIndex = 7
        Me.ChkShowRawLog.Text = "Show Raw Log Text"
        Me.ChkShowRawLog.UseVisualStyleBackColor = True
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel1.ColumnCount = 1
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.LogText, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.lblAlertText, 0, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.txtAlertText, 0, 2)
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(12, 30)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 3
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 18.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(776, 379)
        Me.TableLayoutPanel1.TabIndex = 8
        '
        'LogText
        '
        Me.LogText.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.LogText.BackColor = System.Drawing.SystemColors.Window
        Me.LogText.Location = New System.Drawing.Point(3, 3)
        Me.LogText.Multiline = True
        Me.LogText.Name = "LogText"
        Me.LogText.ReadOnly = True
        Me.LogText.Size = New System.Drawing.Size(770, 174)
        Me.LogText.TabIndex = 2
        '
        'txtAlertText
        '
        Me.txtAlertText.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtAlertText.BackColor = System.Drawing.SystemColors.Window
        Me.txtAlertText.Location = New System.Drawing.Point(3, 201)
        Me.txtAlertText.Multiline = True
        Me.txtAlertText.Name = "txtAlertText"
        Me.txtAlertText.ReadOnly = True
        Me.txtAlertText.Size = New System.Drawing.Size(770, 175)
        Me.txtAlertText.TabIndex = 7
        '
        'LogViewer
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(800, 466)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.Controls.Add(Me.ChkShowRawLog)
        Me.Controls.Add(Me.StatusStrip1)
        Me.Controls.Add(Me.BtnClose)
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(816, 173)
        Me.Name = "LogViewer"
        Me.Text = "Log Viewer"
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.TableLayoutPanel1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents BtnClose As Button
    Friend WithEvents StatusStrip1 As StatusStrip
    Friend WithEvents LblLogDate As ToolStripStatusLabel
    Friend WithEvents LblSource As ToolStripStatusLabel
    Friend WithEvents lblAlertText As Label
    Friend WithEvents ChkShowRawLog As CheckBox
    Friend WithEvents TableLayoutPanel1 As TableLayoutPanel
    Friend WithEvents LogText As TextBox
    Friend WithEvents txtAlertText As TextBox
End Class
