Module SupportCode
    Public ignoredLogsWindow As Ignored_Logs_and_Search_Results = Nothing
    Public replacementsList As New List(Of ReplacementsClass)

    Public Function VerifyWindowLocation(point As Point, ByRef window As Form) As Point
        Dim screen As Screen = Screen.FromControl(window)

        Dim windowBounds As New Rectangle(window.Left, window.Top, window.Width, window.Height)
        Dim screenBounds As Rectangle = screen.WorkingArea

        Return If(screenBounds.Contains(windowBounds), point, New Point(0, 0))
    End Function

    Public Sub SelectFileInWindowsExplorer(strFullPath As String)
        If Not String.IsNullOrEmpty(strFullPath) AndAlso IO.File.Exists(strFullPath) Then
            Dim pidlList As IntPtr = NativeMethod.NativeMethods.ILCreateFromPathW(strFullPath)

            If Not pidlList.Equals(IntPtr.Zero) Then
                Try
                    NativeMethod.NativeMethods.SHOpenFolderAndSelectItems(pidlList, 0, IntPtr.Zero, 0)
                Finally
                    NativeMethod.NativeMethods.ILFree(pidlList)
                End Try
            End If
        End If
    End Sub

    Public Function SearchForExistingProcess(strFileName As String, currentProcessID As Integer, boolFullFilePathPassed As Boolean) As Boolean
        Dim processExecutablePath As String

        For Each process As Process In Process.GetProcesses()
            processExecutablePath = GetProcessExecutablePath(process.Id)

            If Not String.IsNullOrWhiteSpace(processExecutablePath) Then
                Try
                    processExecutablePath = If(boolFullFilePathPassed, New IO.FileInfo(processExecutablePath).FullName, New IO.FileInfo(processExecutablePath).Name)
                    If strFileName.Equals(processExecutablePath, StringComparison.OrdinalIgnoreCase) And process.Id <> currentProcessID Then Return True
                Catch ex As ArgumentException
                End Try
            End If
        Next

        Return False
    End Function

    Private Function GetProcessExecutablePath(processID As Integer) As String
        Try
            Dim memoryBuffer As New Text.StringBuilder(1024)
            Dim processHandle As IntPtr = NativeMethod.NativeMethods.OpenProcess(NativeMethod.ProcessAccessFlags.PROCESS_QUERY_LIMITED_INFORMATION, False, processID)

            If Not processHandle.Equals(IntPtr.Zero) Then
                Try
                    Dim memoryBufferSize As Integer = memoryBuffer.Capacity
                    If NativeMethod.NativeMethods.QueryFullProcessImageName(processHandle, 0, memoryBuffer, memoryBufferSize) Then Return memoryBuffer.ToString()
                Finally
                    NativeMethod.NativeMethods.CloseHandle(processHandle)
                End Try
            End If

            NativeMethod.NativeMethods.CloseHandle(processHandle)
            Return Nothing
        Catch ex As Exception
            Return Nothing
        End Try
    End Function
End Module

Public Class ReplacementsClass
    Public BoolRegex As Boolean
    Public BoolCaseSensitive As Boolean
    Public StrReplace, StrReplaceWith As String

    Public Function ToListViewItem() As MyReplacementsListViewItem
        Dim listViewItem As New MyReplacementsListViewItem(StrReplace)
        listViewItem.SubItems.Add(StrReplaceWith)
        listViewItem.SubItems.Add(BoolRegex.ToString)
        listViewItem.SubItems.Add(BoolCaseSensitive.ToString)
        listViewItem.BoolRegex = BoolRegex
        listViewItem.BoolCaseSensitive = BoolCaseSensitive
        Return listViewItem
    End Function
End Class