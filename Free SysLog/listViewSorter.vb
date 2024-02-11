' Implements a comparer for ListView columns.
Class ListViewComparer
    Implements IComparer

    Private ReadOnly intColumnNumber As Integer
    Private ReadOnly soSortOrder As SortOrder

    Public Sub New(intInputColumnNumber As Integer, soInputSortOrder As SortOrder)
        intColumnNumber = intInputColumnNumber
        soSortOrder = soInputSortOrder
    End Sub

    ' Compare the items in the appropriate column
    ' for objects x and y.
    Public Function Compare(lvInputFirstListView As Object, lvInputSecondListView As Object) As Integer Implements IComparer.Compare
        Dim strFirstString, strSecondString As String
        Dim date1, date2 As Date
        Dim lvFirstListView As ListViewItem = lvInputFirstListView
        Dim lvSecondListView As ListViewItem = lvInputSecondListView

        ' Get the sub-item values.
        strFirstString = If(lvFirstListView.SubItems.Count <= intColumnNumber, "", lvFirstListView.SubItems(intColumnNumber).Text)
        strSecondString = If(lvSecondListView.SubItems.Count <= intColumnNumber, "", lvSecondListView.SubItems(intColumnNumber).Text)

        If lvFirstListView.ListView IsNot Nothing Then
            ' Compare them.
            If intColumnNumber = 0 Then
                date1 = Date.Parse(strFirstString)
                date2 = Date.Parse(strSecondString)

                Return If(soSortOrder = SortOrder.Ascending, String.Compare(date1, date2), String.Compare(date2, date1))
            Else
                Return If(soSortOrder = SortOrder.Ascending, String.Compare(strFirstString, strSecondString), String.Compare(strSecondString, strFirstString))
            End If
        Else
            Return 0
        End If
    End Function
End Class