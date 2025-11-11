Namespace ThreadSafetyLists
    Public Class ThreadSafeIgnoredList
        ' The internal list
        Private ignoredList As New List(Of IgnoredClass)()

        ' The lock object for synchronization
        Private ReadOnly lockObj As New Object()

        ' Add an item safely
        Public Sub Add(item As IgnoredClass)
            SyncLock lockObj
                ignoredList.Add(item)
            End SyncLock
        End Sub

        ' Clear all items safely
        Public Sub Clear()
            SyncLock lockObj
                ignoredList.Clear()
            End SyncLock
        End Sub

        ' Sort the list safely
        Public Sub Sort(comparer As Comparison(Of IgnoredClass))
            SyncLock lockObj
                ignoredList.Sort(comparer)
            End SyncLock
        End Sub

        ' Get a snapshot copy of the list for safe enumeration
        Public Function GetSnapshot() As List(Of IgnoredClass)
            SyncLock lockObj
                Return New List(Of IgnoredClass)(ignoredList)
            End SyncLock
        End Function
    End Class

    Public Class ThreadSafeAlertsList
        ' The internal list
        Private alertsList As New List(Of AlertsClass)()

        ' The lock object for synchronization
        Private ReadOnly lockObj As New Object()

        ' Add an item safely
        Public Sub Add(item As AlertsClass)
            SyncLock lockObj
                alertsList.Add(item)
            End SyncLock
        End Sub

        ' Clear all items safely
        Public Sub Clear()
            SyncLock lockObj
                alertsList.Clear()
            End SyncLock
        End Sub

        ' Sort the list safely
        Public Sub Sort(comparer As Comparison(Of AlertsClass))
            SyncLock lockObj
                alertsList.Sort(comparer)
            End SyncLock
        End Sub

        ' Get a snapshot copy of the list for safe enumeration
        Public Function GetSnapshot() As List(Of AlertsClass)
            SyncLock lockObj
                Return New List(Of AlertsClass)(alertsList)
            End SyncLock
        End Function
    End Class

    Public Class ThreadSafeReplacementsList
        ' The internal list
        Private replacementsList As New List(Of ReplacementsClass)()

        ' The lock object for synchronization
        Private ReadOnly lockObj As New Object()

        ' Add an item safely
        Public Sub Add(item As ReplacementsClass)
            SyncLock lockObj
                replacementsList.Add(item)
            End SyncLock
        End Sub

        ' Clear all items safely
        Public Sub Clear()
            SyncLock lockObj
                replacementsList.Clear()
            End SyncLock
        End Sub

        ' Sort the list safely
        Public Sub Sort(comparer As Comparison(Of ReplacementsClass))
            SyncLock lockObj
                replacementsList.Sort(comparer)
            End SyncLock
        End Sub

        ' Get a snapshot copy of the list for safe enumeration
        Public Function GetSnapshot() As List(Of ReplacementsClass)
            SyncLock lockObj
                Return New List(Of ReplacementsClass)(replacementsList)
            End SyncLock
        End Function
    End Class

    Public Class ThreadSafeProxyServerList
        ' The internal list
        Private proxyList As New List(Of SysLogProxyServer)()

        ' The lock object for synchronization
        Private ReadOnly lockObj As New Object()

        ' Add an item safely
        Public Sub Add(item As SysLogProxyServer)
            SyncLock lockObj
                proxyList.Add(item)
            End SyncLock
        End Sub

        ' Clear all items safely
        Public Sub Clear()
            SyncLock lockObj
                proxyList.Clear()
            End SyncLock
        End Sub

        ' Sort the list safely
        Public Sub Sort(comparer As Comparison(Of SysLogProxyServer))
            SyncLock lockObj
                proxyList.Sort(comparer)
            End SyncLock
        End Sub

        ' Get a snapshot copy of the list for safe enumeration
        Public Function GetSnapshot() As List(Of SysLogProxyServer)
            SyncLock lockObj
                Return New List(Of SysLogProxyServer)(proxyList)
            End SyncLock
        End Function
    End Class
End Namespace