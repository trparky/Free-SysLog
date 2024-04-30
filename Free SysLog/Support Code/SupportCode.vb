﻿Imports System.Net.Sockets
Imports System.Text

Public Enum IgnoreOrSearchWindowDisplayMode As Byte
    ignored
    search
End Enum

Module SupportCode
    Public IgnoredLogsAndSearchResultsInstance As IgnoredLogsAndSearchResults = Nothing
    Public replacementsList As New List(Of ReplacementsClass)
    Public ignoredList As New List(Of IgnoredClass)
    Public alertsList As New List(Of AlertsClass)
    Public Const strMutexName As String = "Free SysLog Server"
    Public mutex As Threading.Mutex
    Public strEXEPath As String = Process.GetCurrentProcess.MainModule.FileName
    Public boolDoWeOwnTheMutex As Boolean = False

    Public Function IsRegexPatternValid(pattern As String) As Boolean
        Try
            Dim regex As New RegularExpressions.Regex(pattern)
            Return True
        Catch ex As ArgumentException
            Return False
        End Try
    End Function

    Public Sub SendMessageToSysLogServer(strMessage As String, intPort As Integer)
        Using udpClient As New UdpClient()
            udpClient.Connect(Net.IPAddress.Loopback, intPort)
            Dim data As Byte() = Encoding.UTF8.GetBytes(strMessage)
            udpClient.Send(data, data.Length)
        End Using
    End Sub

    Public Function SanitizeForCSV(input As String) As String
        If input.Contains(Chr(34)) Then input = input.Replace(Chr(34), Chr(34) & Chr(34))
        If input.Contains(",") Then input = $"{Chr(34)}{input}{Chr(34)}"
        Return input
    End Function

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

Public Class IgnoredClass
    Public BoolRegex As Boolean
    Public BoolCaseSensitive As Boolean
    Public StrIgnore As String

    Public Function ToListViewItem() As MyIgnoredListViewItem
        Dim listViewItem As New MyIgnoredListViewItem(StrIgnore)
        listViewItem.SubItems.Add(BoolRegex.ToString)
        listViewItem.SubItems.Add(BoolCaseSensitive.ToString)
        listViewItem.BoolRegex = BoolRegex
        listViewItem.BoolCaseSensitive = BoolCaseSensitive
        Return listViewItem
    End Function
End Class

Public Enum AlertType As Byte
    Warning
    Info
    ErrorMsg
    None
End Enum

Public Class AlertsClass
    Public BoolRegex As Boolean
    Public BoolCaseSensitive As Boolean
    Public StrLogText, StrAlertText As String
    Public alertType As AlertType = AlertType.None

    Public Function ToListViewItem() As AlertsListViewItem
        Dim listViewItem As New AlertsListViewItem(StrLogText) With {.StrLogText = StrLogText, .StrAlertText = StrAlertText}
        listViewItem.SubItems.Add(If(String.IsNullOrWhiteSpace(StrAlertText), "(Shows Log Text)", StrAlertText))
        listViewItem.SubItems.Add(BoolRegex.ToString)
        listViewItem.SubItems.Add(BoolCaseSensitive.ToString)

        If alertType = AlertType.Warning Then
            listViewItem.SubItems.Add("Warning Message")
        ElseIf alertType = AlertType.ErrorMsg Then
            listViewItem.SubItems.Add("Error Message")
        ElseIf alertType = AlertType.Info Then
            listViewItem.SubItems.Add("Information Message")
        ElseIf alertType = AlertType.None Then
            listViewItem.SubItems.Add("None")
        End If

        listViewItem.BoolRegex = BoolRegex
        listViewItem.BoolCaseSensitive = BoolCaseSensitive
        listViewItem.AlertType = alertType
        Return listViewItem
    End Function
End Class