Namespace listViewSorter
    ' Implements a comparer for ListView columns.
    Class ListViewComparer
        Implements IComparer

        Private intColumnNumber As Integer
        Private soSortOrder As SortOrder

        Public Sub New(ByVal intInputColumnNumber As Integer, ByVal soInputSortOrder As SortOrder)
            intColumnNumber = intInputColumnNumber
            soSortOrder = soInputSortOrder
        End Sub

        ' Compare the items in the appropriate column
        ' for objects x and y.
        Public Function Compare(lvInputFirstListView As Object, lvInputSecondListView As Object) As Integer Implements IComparer.Compare
            Dim dbl1, dbl2 As Double
            Dim long1, long2 As Long
            Dim date1, date2 As Date
            Dim strFirstString, strSecondString As String
            Dim lvFirstListView As ListViewItem = lvInputFirstListView
            Dim lvSecondListView As ListViewItem = lvInputSecondListView
            Dim lvFirstListViewType As Type = lvFirstListView.GetType
            Dim lvSecondListViewType As Type = lvSecondListView.GetType

            ' Get the sub-item values.
            strFirstString = If(lvFirstListView.SubItems.Count <= intColumnNumber, "", lvFirstListView.SubItems(intColumnNumber).Text)
            strSecondString = If(lvSecondListView.SubItems.Count <= intColumnNumber, "", lvSecondListView.SubItems(intColumnNumber).Text)

            ' Compare them.
            If Double.TryParse(strFirstString, dbl1) And Double.TryParse(strSecondString, dbl2) Then
                Return If(soSortOrder = SortOrder.Ascending, dbl1.CompareTo(dbl2), dbl2.CompareTo(dbl1))
            ElseIf Date.TryParse(strFirstString, date1) And Date.TryParse(strSecondString, date2) Then
                Return If(soSortOrder = SortOrder.Ascending, date1.CompareTo(date2), date2.CompareTo(date1))
            ElseIf Long.TryParse(strFirstString, long1) And Long.TryParse(strSecondString, long2) Then
                Return If(soSortOrder = SortOrder.Ascending, long1.CompareTo(long2), long2.CompareTo(long1))
            Else
                Return If(soSortOrder = SortOrder.Ascending, String.Compare(strFirstString, strSecondString), String.Compare(strSecondString, strFirstString))
            End If
        End Function
    End Class

    ' Implements a comparer for ListView columns.
    Class IgnoredWordsAndPhrasesListViewComparer
        Implements IComparer

        Private intColumnNumber As Integer
        Private soSortOrder As SortOrder

        Public Sub New(ByVal intInputColumnNumber As Integer, ByVal soInputSortOrder As SortOrder)
            intColumnNumber = intInputColumnNumber
            soSortOrder = soInputSortOrder
        End Sub

        ' Compare the items in the appropriate column
        ' for objects x and y.
        Public Function Compare(lvInputFirstListView As Object, lvInputSecondListView As Object) As Integer Implements IComparer.Compare
            Dim dbl1, dbl2 As Double
            Dim long1, long2 As Long
            Dim date1, date2 As Date
            Dim strFirstString, strSecondString As String
            Dim lvFirstListView As ListViewItem = lvInputFirstListView
            Dim lvSecondListView As ListViewItem = lvInputSecondListView
            Dim lvFirstListViewType As Type = lvFirstListView.GetType
            Dim lvSecondListViewType As Type = lvSecondListView.GetType

            ' Get the sub-item values.
            strFirstString = If(lvFirstListView.SubItems.Count <= intColumnNumber, "", lvFirstListView.SubItems(intColumnNumber).Text)
            strSecondString = If(lvSecondListView.SubItems.Count <= intColumnNumber, "", lvSecondListView.SubItems(intColumnNumber).Text)

            If intColumnNumber = 8 AndAlso TypeOf lvFirstListView Is MyIgnoredListViewItem AndAlso TypeOf lvSecondListView Is MyIgnoredListViewItem Then
                Dim item1 As MyIgnoredListViewItem = DirectCast(lvFirstListView, MyIgnoredListViewItem)
                Dim item2 As MyIgnoredListViewItem = DirectCast(lvSecondListView, MyIgnoredListViewItem)

                Dim enabled1 As Boolean = item1.BoolEnabled
                Dim enabled2 As Boolean = item2.BoolEnabled

                ' Always keep enabled items above disabled items (independent of sort order)
                If enabled1 AndAlso Not enabled2 Then Return -1     ' item1 first
                If Not enabled1 AndAlso enabled2 Then Return 1      ' item2 first

                ' If both disabled, don't change their relative order
                If Not enabled1 AndAlso Not enabled2 Then Return 0

                ' --- Now compare ENABLED items by hits first ---
                Dim hits1, hits2 As Long
                If Not Long.TryParse(item1.SubItems(4).Text, hits1) Then hits1 = 0
                If Not Long.TryParse(item2.SubItems(4).Text, hits2) Then hits2 = 0

                Dim bothZeroHits As Boolean = (hits1 = 0 AndAlso hits2 = 0)

                ' Enabled items with hits > 0 should sort ahead of hits = 0
                If hits1 = 0 AndAlso hits2 <> 0 Then Return 1    ' item1 goes below
                If hits1 <> 0 AndAlso hits2 = 0 Then Return -1   ' item1 goes above

                ' If both have zero hits, do not reorder them
                If bothZeroHits Then Return 0

                ' If both are enabled, sort by timeSpanOfLastOccurrence
                Dim timeSpan1 As TimeSpan = item1.timeSpanOfLastOccurrence
                Dim timeSpan2 As TimeSpan = item2.timeSpanOfLastOccurrence

                Return If(soSortOrder = SortOrder.Ascending, timeSpan1.CompareTo(timeSpan2), timeSpan2.CompareTo(timeSpan1))
            ElseIf intColumnNumber = 7 AndAlso TypeOf lvFirstListView Is MyIgnoredListViewItem AndAlso TypeOf lvSecondListView Is MyIgnoredListViewItem Then
                Dim item1 As MyIgnoredListViewItem = DirectCast(lvFirstListView, MyIgnoredListViewItem)
                Dim item2 As MyIgnoredListViewItem = DirectCast(lvSecondListView, MyIgnoredListViewItem)

                Dim enabled1 As Boolean = item1.BoolEnabled
                Dim enabled2 As Boolean = item2.BoolEnabled

                ' Always keep enabled items above disabled items (independent of sort order)
                If enabled1 AndAlso Not enabled2 Then Return -1     ' item1 first
                If Not enabled1 AndAlso enabled2 Then Return 1      ' item2 first

                ' If both disabled, don't change their relative order
                If Not enabled1 AndAlso Not enabled2 Then Return 0

                ' --- Now compare ENABLED items by hits first ---
                Dim hits1, hits2 As Long
                If Not Long.TryParse(item1.SubItems(4).Text, hits1) Then hits1 = 0
                If Not Long.TryParse(item2.SubItems(4).Text, hits2) Then hits2 = 0

                Dim bothZeroHits As Boolean = (hits1 = 0 AndAlso hits2 = 0)

                ' Enabled items with hits > 0 should sort ahead of hits = 0
                If hits1 = 0 AndAlso hits2 <> 0 Then Return 1    ' item1 goes below
                If hits1 <> 0 AndAlso hits2 = 0 Then Return -1   ' item1 goes above

                ' If both have zero hits, do not reorder them
                If bothZeroHits Then Return 0

                ' If both are enabled, sort by dateOfLastOccurrence
                Return If(soSortOrder = SortOrder.Ascending, item1.dateOfLastOccurrence.CompareTo(item2.dateOfLastOccurrence), item2.dateOfLastOccurrence.CompareTo(item1.dateOfLastOccurrence))
            ElseIf intColumnNumber = 4 AndAlso TypeOf lvFirstListView Is MyIgnoredListViewItem AndAlso TypeOf lvSecondListView Is MyIgnoredListViewItem Then
                Dim item1 As MyIgnoredListViewItem = DirectCast(lvFirstListView, MyIgnoredListViewItem)
                Dim item2 As MyIgnoredListViewItem = DirectCast(lvSecondListView, MyIgnoredListViewItem)

                Dim enabled1 As Boolean = item1.BoolEnabled
                Dim enabled2 As Boolean = item2.BoolEnabled

                ' Always keep enabled items above disabled items (independent of sort order)
                If enabled1 AndAlso Not enabled2 Then Return -1     ' item1 first
                If Not enabled1 AndAlso enabled2 Then Return 1      ' item2 first

                ' If both disabled, don't change their relative order
                If Not enabled1 AndAlso Not enabled2 Then Return 0

                ' --- Now compare ENABLED items by hits first ---
                Dim hits1, hits2 As Long
                If Not Long.TryParse(item1.SubItems(4).Text, hits1) Then hits1 = 0
                If Not Long.TryParse(item2.SubItems(4).Text, hits2) Then hits2 = 0

                Dim bothZeroHits As Boolean = (hits1 = 0 AndAlso hits2 = 0)

                ' Enabled items with hits > 0 should sort ahead of hits = 0
                If hits1 = 0 AndAlso hits2 <> 0 Then Return 1    ' item1 goes below
                If hits1 <> 0 AndAlso hits2 = 0 Then Return -1   ' item1 goes above

                ' If both have zero hits, do not reorder them
                If bothZeroHits Then Return 0

                ' If both are enabled, sort by dateOfLastOccurrence
                Return If(soSortOrder = SortOrder.Ascending, hits1.CompareTo(hits2), hits2.CompareTo(hits1))
            Else
                ' Compare them.
                If Double.TryParse(strFirstString, dbl1) And Double.TryParse(strSecondString, dbl2) Then
                    Return If(soSortOrder = SortOrder.Ascending, dbl1.CompareTo(dbl2), dbl2.CompareTo(dbl1))
                ElseIf Date.TryParse(strFirstString, date1) And Date.TryParse(strSecondString, date2) Then
                    Return If(soSortOrder = SortOrder.Ascending, date1.CompareTo(date2), date2.CompareTo(date1))
                ElseIf Long.TryParse(strFirstString, long1) And Long.TryParse(strSecondString, long2) Then
                    Return If(soSortOrder = SortOrder.Ascending, long1.CompareTo(long2), long2.CompareTo(long1))
                Else
                    Return If(soSortOrder = SortOrder.Ascending, String.Compare(strFirstString, strSecondString), String.Compare(strSecondString, strFirstString))
                End If
            End If
        End Function
    End Class
End Namespace