Imports System.Collections.Concurrent

Namespace ThreadSafetyLists
    Public Class ConcurrentHashSet(Of T)
        Implements ISet(Of T)

        Private ReadOnly _dict As ConcurrentDictionary(Of T, Byte)
        Private ReadOnly _comparer As IEqualityComparer(Of T)

        Public Sub New()
            _comparer = EqualityComparer(Of T).Default
            _dict = New ConcurrentDictionary(Of T, Byte)(_comparer)
        End Sub

        Public Sub New(comparer As IEqualityComparer(Of T))
            _comparer = If(comparer, EqualityComparer(Of T).Default)
            _dict = New ConcurrentDictionary(Of T, Byte)(_comparer)
        End Sub

        Public Function Add(item As T) As Boolean Implements ISet(Of T).Add
            Return _dict.TryAdd(item, 0)
        End Function

        Private Sub ICollection_Add(item As T) Implements ICollection(Of T).Add
            _dict.TryAdd(item, 0)
        End Sub

        Public Sub UnionWith(other As IEnumerable(Of T)) Implements ISet(Of T).UnionWith
            For Each item In other
                _dict.TryAdd(item, 0)
            Next
        End Sub

        Public Sub IntersectWith(other As IEnumerable(Of T)) Implements ISet(Of T).IntersectWith
            Dim otherSet = New HashSet(Of T)(other, _comparer)
            For Each key In _dict.Keys
                If Not otherSet.Contains(key) Then
                    _dict.TryRemove(key, Nothing)
                End If
            Next
        End Sub

        Public Sub ExceptWith(other As IEnumerable(Of T)) Implements ISet(Of T).ExceptWith
            For Each item In other
                _dict.TryRemove(item, Nothing)
            Next
        End Sub

        Public Sub SymmetricExceptWith(other As IEnumerable(Of T)) Implements ISet(Of T).SymmetricExceptWith
            For Each item In other
                If Not _dict.TryAdd(item, 0) Then
                    _dict.TryRemove(item, Nothing)
                End If
            Next
        End Sub

        Public Function IsSubsetOf(other As IEnumerable(Of T)) As Boolean Implements ISet(Of T).IsSubsetOf
            Dim otherSet = New HashSet(Of T)(other, _comparer)
            Return _dict.Keys.All(Function(k) otherSet.Contains(k))
        End Function

        Public Function IsSupersetOf(other As IEnumerable(Of T)) As Boolean Implements ISet(Of T).IsSupersetOf
            Return other.All(Function(k) _dict.ContainsKey(k))
        End Function

        Public Function IsProperSupersetOf(other As IEnumerable(Of T)) As Boolean Implements ISet(Of T).IsProperSupersetOf
            Dim otherSet = New HashSet(Of T)(other, _comparer)
            Return otherSet.All(Function(k) _dict.ContainsKey(k)) AndAlso _dict.Count > otherSet.Count
        End Function

        Public Function IsProperSubsetOf(other As IEnumerable(Of T)) As Boolean Implements ISet(Of T).IsProperSubsetOf
            Dim otherSet = New HashSet(Of T)(other, _comparer)
            Return _dict.Keys.All(Function(k) otherSet.Contains(k)) AndAlso _dict.Count < otherSet.Count
        End Function

        Public Function Overlaps(other As IEnumerable(Of T)) As Boolean Implements ISet(Of T).Overlaps
            Return other.Any(Function(k) _dict.ContainsKey(k))
        End Function

        Public Function SetEquals(other As IEnumerable(Of T)) As Boolean Implements ISet(Of T).SetEquals
            Dim otherSet = New HashSet(Of T)(other, _comparer)
            Return _dict.Count = otherSet.Count AndAlso _dict.Keys.All(Function(k) otherSet.Contains(k))
        End Function

        Public Sub Clear() Implements ICollection(Of T).Clear
            _dict.Clear()
        End Sub

        Public Function Contains(item As T) As Boolean Implements ICollection(Of T).Contains
            Return _dict.ContainsKey(item)
        End Function

        Public Sub CopyTo(array() As T, arrayIndex As Integer) Implements ICollection(Of T).CopyTo
            _dict.Keys.CopyTo(array, arrayIndex)
        End Sub

        Public ReadOnly Property Count As Integer Implements ICollection(Of T).Count
            Get
                Return _dict.Count
            End Get
        End Property

        Public ReadOnly Property IsReadOnly As Boolean Implements ICollection(Of T).IsReadOnly
            Get
                Return False
            End Get
        End Property

        Public Function Remove(item As T) As Boolean Implements ICollection(Of T).Remove
            Return _dict.TryRemove(item, Nothing)
        End Function

        Public Function GetEnumerator() As IEnumerator(Of T) Implements IEnumerable(Of T).GetEnumerator
            Return _dict.Keys.GetEnumerator()
        End Function

        Private Function IEnumerable_GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
            Return GetEnumerator()
        End Function
    End Class

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