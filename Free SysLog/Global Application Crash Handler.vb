Imports System.Web
Imports System.Net
Imports System.Web.HttpUtility
Imports System.Threading
Imports System.Reflection

Public Class frmCrash
    Property crashData As Exception
    Private boolSubmittedCrashData As Boolean = False

    Public Function postData(ByRef data As String, ByRef url As String) As String
        Try
            Dim request As Net.HttpWebRequest = DirectCast(Net.WebRequest.Create(url), Net.HttpWebRequest)
            request.Method = "POST"
            request.ContentType = "application/x-www-form-urlencoded"
            request.UserAgent = "Tom's Program Bug Reporter"
            Dim postDataString As String = data
            request.ContentLength = postDataString.Length

            Dim writer As New IO.StreamWriter(request.GetRequestStream(), System.Text.Encoding.ASCII)
            writer.Write(postDataString)
            writer.Close()

            Return New IO.StreamReader(request.GetResponse.GetResponseStream, System.Text.Encoding.Default).ReadToEnd.Trim
        Catch ex As Exception
            Return "error"
        End Try
    End Function

    Private Sub frmCrash_FormClosing(sender As Object, e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        If boolSubmittedCrashData = False Then
            Dim msgBoxText As New System.Text.StringBuilder
            msgBoxText.AppendLine("Are you sure you want to close this program crash notification without submitting the crash analysis data to the developer of this program?")
            msgBoxText.AppendLine()
            msgBoxText.AppendLine("Submitting this data is very valuable in correcting program bugs.  Without this crash data, the developer won't know how to fix the issue you have encountered.")
            msgBoxText.AppendLine()
            msgBoxText.AppendLine("Please reconsider submitting the crash analysis data to the developer of this program.  Don't worry, your information will be kept strictly confidential.")
            msgBoxText.AppendLine()
            msgBoxText.AppendLine("Are you sure about this?  If you want to submit the crash analysis data, click No.")

            Dim msgBoxResult As MsgBoxResult = MsgBox(msgBoxText.ToString, MsgBoxStyle.YesNo + MsgBoxStyle.Question, Me.Text)
            msgBoxText = Nothing

            If msgBoxResult = Microsoft.VisualBasic.MsgBoxResult.No Then
                e.Cancel = True
                Exit Sub
            End If
        End If

        Process.GetCurrentProcess.Kill()
    End Sub

    Private Sub frmCrash_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        System.Media.SystemSounds.Hand.Play()

        Dim stringBuilder As New System.Text.StringBuilder
        stringBuilder.AppendLine("Message: " & crashData.Message.Trim)
        stringBuilder.AppendLine("Exception Type: " & crashData.GetType().ToString())
        stringBuilder.AppendLine()

        stringBuilder.Append("The exception occurred ")

        For Each lineInStackTrace As String In crashData.StackTrace.Split(vbCrLf)
            stringBuilder.AppendLine(lineInStackTrace.Trim)
        Next

        txtStackTrace.Text = stringBuilder.ToString.Trim
        stringBuilder = Nothing

        txtWhatWereYouDoing.Select()

        Dim stopBitmapIcon As New Bitmap(My.Resources.iconstoptom)
        Dim stopIcon As Icon = Icon.FromHandle(stopBitmapIcon.GetHicon())
        Me.Icon = stopIcon

        stopBitmapIcon.Dispose()
        stopIcon.Dispose()
        stopIcon = Nothing
        stopBitmapIcon = Nothing
    End Sub

    Private Sub btnSubmitCrashData_Click(sender As System.Object, e As System.EventArgs) Handles btnSubmitCrashData.Click
        Dim msgBoxResult As MsgBoxResult

        If txtEmailAddress.Text.Trim = Nothing Then
            msgBoxResult = MsgBox("You did not enter an email address." & vbCrLf & vbCrLf & "If you don't want to provide an email address, press the OK button.  Press the Cancel button to try again.", MsgBoxStyle.Information + MsgBoxStyle.OkCancel, Me.Text)

            If msgBoxResult = Microsoft.VisualBasic.MsgBoxResult.Cancel Then Exit Sub
        End If

        msgBoxResult = Microsoft.VisualBasic.MsgBoxResult.Retry

        If System.Text.RegularExpressions.Regex.IsMatch(txtEmailAddress.Text.Trim, "[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?") = False Then
            msgBoxResult = MsgBox("Invalid Email Address Format." & vbCrLf & vbCrLf & "If you don't want to provide an email address, press the OK button.  Press the Cancel button to try again.", MsgBoxStyle.Information + MsgBoxStyle.OkCancel, Me.Text)

            If msgBoxResult = Microsoft.VisualBasic.MsgBoxResult.Cancel Then
                Exit Sub
            ElseIf msgBoxResult = Microsoft.VisualBasic.MsgBoxResult.Ok Then
                txtEmailAddress.Text = ""
            End If
        End If

        Dim dataToBeSent As String = String.Format("doing={0}&program={1}&error={2}", UrlEncode(txtWhatWereYouDoing.Text), UrlEncode(Application.ProductName), UrlEncode(txtStackTrace.Text))

        If (txtEmailAddress.Text.Trim = Nothing) = False Then
            dataToBeSent &= "&email=" & UrlEncode(txtEmailAddress.Text)
        End If

        Dim webPageResults As String = postData(dataToBeSent, "http://www.toms-world.org/crashReporter")
        boolSubmittedCrashData = True
        dataToBeSent = Nothing

        If webPageResults = "ok" Then
            MsgBox("Bug report submitted successfully.", MsgBoxStyle.Information, Me.Text)
        Else
            MsgBox("Bug report submission error." & vbCrLf & vbCrLf & "Error: " & webPageResults, MsgBoxStyle.Information, Me.Text)
        End If

        Process.GetCurrentProcess.Kill()
    End Sub

    Private Sub Timer1_Tick(sender As System.Object, e As System.EventArgs) Handles Timer1.Tick
        If lblHeader.Visible = True Then
            lblHeader.Visible = False
        Else
            lblHeader.Visible = True
        End If
    End Sub
End Class

Module loadExeptionHandler
    Public Sub loadExceptionHandler()
        ' Subscribe to thread (unhandled) exception events
        Dim handler As ThreadExceptionHandler = New ThreadExceptionHandler()
        AddHandler Application.ThreadException, AddressOf handler.Application_ThreadException
    End Sub

    Public Sub manuallyLoadCrashWindow(ex As Exception)
        Dim crashWindow As New frmCrash
        crashWindow.crashData = ex
        crashWindow.Text = "Critical Application Error Detected!"
        crashWindow.lblHeader.Text = "Critical Application Error Detected!"
        crashWindow.ShowDialog()
        Dim currentProcess As Process = Process.GetCurrentProcess()
        currentProcess.Kill()
    End Sub
End Module

Friend Class ThreadExceptionHandler
    '''
    ''' Handles the thread exception.
    '''
    Public Sub Application_ThreadException(ByVal sender As System.Object, ByVal e As ThreadExceptionEventArgs)
        Try
            Dim crashWindow As New frmCrash
            crashWindow.crashData = e.Exception
            crashWindow.ShowDialog()
            Dim currentProcess As Process = Process.GetCurrentProcess()
            currentProcess.Kill()

            '' Exit the program if the user clicks Abort.
            'Dim result As DialogResult = ShowThreadExceptionDialog(e.Exception)

            'If (result = DialogResult.Abort) Then
            '    Application.Exit()
            'End If
        Catch
            ' Fatal error, terminate program
            Try
                MessageBox.Show("Fatal Error", "Fatal Error", MessageBoxButtons.OK, MessageBoxIcon.Stop)
            Finally
                Dim currentProcess As Process = Process.GetCurrentProcess()
                currentProcess.Kill()
            End Try
        End Try
    End Sub
End Class