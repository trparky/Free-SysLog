Public Class Ignored_Words_and_Phrases
    Private boolDoneLoading As Boolean = False

    Private Sub BtnAdd_Click(sender As Object, e As EventArgs) Handles BtnAdd.Click
        Dim input As String = InputBox("Provide a word or phrase that Free SysLog will ignore...", "Add Ignored Word or Phrase")

        If Not String.IsNullOrWhiteSpace(input) Then
            ListOfWords.Items.Add(input)
        End If
    End Sub

    Private Sub Ignored_Words_and_Phrases_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        Dim strings As New Specialized.StringCollection()

        For Each item As ListViewItem In ListOfWords.Items
            strings.Add(item.Text)
        Next

        My.Settings.ignored = strings
        My.Settings.Save()
    End Sub

    Private Sub Ignored_Words_and_Phrases_Load(sender As Object, e As EventArgs) Handles Me.Load
        Location = VerifyWindowLocation(My.Settings.ignoredWordsLocation, Me)

        If My.Settings.ignored IsNot Nothing Then
            For Each word As String In My.Settings.ignored
                ListOfWords.Items.Add(word)
            Next
        End If

        boolDoneLoading = True
    End Sub

    Private Sub ListOfWords_KeyUp(sender As Object, e As KeyEventArgs) Handles ListOfWords.KeyUp
        If e.KeyCode = Keys.Delete And ListOfWords.SelectedItems().Count > 0 Then ListOfWords.Items.Remove(ListOfWords.SelectedItems(0))
    End Sub

    Private Sub BtnDelete_Click(sender As Object, e As EventArgs) Handles BtnDelete.Click
        If ListOfWords.SelectedItems().Count > 0 Then
            ListOfWords.Items.Remove(ListOfWords.SelectedItems(0))
            BtnDelete.Enabled = False
        End If
    End Sub

    Private Sub ListOfWords_Click(sender As Object, e As EventArgs) Handles ListOfWords.Click
        BtnDelete.Enabled = ListOfWords.SelectedItems().Count > 0
    End Sub

    Private Sub Ignored_Words_and_Phrases_LocationChanged(sender As Object, e As EventArgs) Handles Me.LocationChanged
        If boolDoneLoading Then My.Settings.ignoredWordsLocation = Location
    End Sub
End Class