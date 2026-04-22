<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmCalendar
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
        Me.startDateTimePicker = New System.Windows.Forms.DateTimePicker()
        Me.lblStarting = New System.Windows.Forms.Label()
        Me.lblEnding = New System.Windows.Forms.Label()
        Me.endDateTimePicker = New System.Windows.Forms.DateTimePicker()
        Me.btnOK = New System.Windows.Forms.Button()
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'startDateTimePicker
        '
        Me.startDateTimePicker.Location = New System.Drawing.Point(61, 4)
        Me.startDateTimePicker.Name = "startDateTimePicker"
        Me.startDateTimePicker.Size = New System.Drawing.Size(200, 20)
        Me.startDateTimePicker.TabIndex = 1
        '
        'lblStarting
        '
        Me.lblStarting.AutoSize = True
        Me.lblStarting.Location = New System.Drawing.Point(12, 10)
        Me.lblStarting.Name = "lblStarting"
        Me.lblStarting.Size = New System.Drawing.Size(43, 13)
        Me.lblStarting.TabIndex = 2
        Me.lblStarting.Text = "Starting"
        '
        'lblEnding
        '
        Me.lblEnding.AutoSize = True
        Me.lblEnding.Location = New System.Drawing.Point(15, 36)
        Me.lblEnding.Name = "lblEnding"
        Me.lblEnding.Size = New System.Drawing.Size(40, 13)
        Me.lblEnding.TabIndex = 4
        Me.lblEnding.Text = "Ending"
        '
        'endDateTimePicker
        '
        Me.endDateTimePicker.Location = New System.Drawing.Point(61, 30)
        Me.endDateTimePicker.Name = "endDateTimePicker"
        Me.endDateTimePicker.Size = New System.Drawing.Size(200, 20)
        Me.endDateTimePicker.TabIndex = 3
        '
        'btnOK
        '
        Me.btnOK.Location = New System.Drawing.Point(3, 3)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(118, 23)
        Me.btnOK.TabIndex = 5
        Me.btnOK.Text = "OK"
        Me.btnOK.UseVisualStyleBackColor = True
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.ColumnCount = 2
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.btnCancel, 1, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.btnOK, 0, 0)
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(12, 56)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 1
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(249, 31)
        Me.TableLayoutPanel1.TabIndex = 6
        '
        'btnCancel
        '
        Me.btnCancel.Location = New System.Drawing.Point(127, 3)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(118, 23)
        Me.btnCancel.TabIndex = 6
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'frmCalendar
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(271, 93)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.Controls.Add(Me.lblEnding)
        Me.Controls.Add(Me.endDateTimePicker)
        Me.Controls.Add(Me.lblStarting)
        Me.Controls.Add(Me.startDateTimePicker)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmCalendar"
        Me.Text = "Calendar"
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents startDateTimePicker As DateTimePicker
    Friend WithEvents lblStarting As Label
    Friend WithEvents lblEnding As Label
    Friend WithEvents endDateTimePicker As DateTimePicker
    Friend WithEvents btnOK As Button
    Friend WithEvents TableLayoutPanel1 As TableLayoutPanel
    Friend WithEvents btnCancel As Button
End Class
