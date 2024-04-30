Public Class IntegerInputForm
    Public boolSuccess As Boolean = False
    Public intResult As Integer
    Public intMin, intMax As Integer

    Private Sub BtnSave_Click(sender As Object, e As EventArgs) Handles BtnSave.Click
        If Integer.TryParse(TxtSetting.Text, intResult) Then
            boolSuccess = True
            Close()
        Else
            MsgBox("Invalid user input!", MsgBoxStyle.Critical, Text)
        End If
    End Sub

    Private Sub BtnUp_Click(sender As Object, e As EventArgs) Handles BtnUp.Click
        Dim myInteger As Integer

        If Integer.TryParse(TxtSetting.Text, myInteger) Then
            If myInteger = intMax Then Exit Sub
            myInteger += 1
            TxtSetting.Text = myInteger.ToString
        End If
    End Sub

    Private Sub BtnDown_Click(sender As Object, e As EventArgs) Handles BtnDown.Click
        Dim myInteger As Integer

        If Integer.TryParse(TxtSetting.Text, myInteger) Then
            If myInteger = intMin Then Exit Sub
            myInteger -= 1
            TxtSetting.Text = myInteger.ToString
        End If
    End Sub
End Class