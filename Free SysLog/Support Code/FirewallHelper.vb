Imports NetFwTypeLib
Imports System.Runtime.InteropServices

''' Allows basic access to the Windows firewall API.
''' This can be used to add an exception to the Windows firewall exceptions list, so that our programs can continue to run merrily even when the Windows firewall is running.
''' Please note: It is not enforced here, but it might be a good idea to actually prompt the user before messing with their firewall settings, just as a matter of politeness.
Public Class FirewallHelper
    Private Shared _instance As FirewallHelper = Nothing
    Private ReadOnly fwMgr As INetFwMgr = Nothing

    Public Shared ReadOnly Property Instance() As FirewallHelper
        Get
            SyncLock GetType(FirewallHelper)
                If _instance Is Nothing Then _instance = New FirewallHelper()
                Return _instance
            End SyncLock
        End Get
    End Property

    Private Sub New()
        Dim fwMgrType As Type = Type.GetTypeFromProgID("HNetCfg.FwMgr", False)

        fwMgr = Nothing

        If fwMgrType IsNot Nothing Then
            Try
                fwMgr = CType(Activator.CreateInstance(fwMgrType), INetFwMgr)
            Catch ex As ArgumentException
            Catch ex As NotSupportedException
            Catch ex As Reflection.TargetInvocationException
            Catch ex As MissingMethodException
            Catch ex As MethodAccessException
            Catch ex As MemberAccessException
            Catch ex As InvalidComObjectException
            Catch ex As COMException
            Catch ex As TypeLoadException
            End Try
        End If
    End Sub

    ''' <returns>A Boolean value indicating if the firewall is installed.</returns>
    Public ReadOnly Property IsFirewallInstalled() As Boolean
        Get
            Return fwMgr IsNot Nothing AndAlso fwMgr.LocalPolicy IsNot Nothing AndAlso fwMgr.LocalPolicy.CurrentProfile IsNot Nothing
        End Get
    End Property

    ''' <returns>A Boolean value indicating if the firewall is enabled.</returns>
    Public ReadOnly Property IsFirewallEnabled() As Boolean
        Get
            Return IsFirewallInstalled AndAlso fwMgr.LocalPolicy.CurrentProfile.FirewallEnabled
        End Get
    End Property

    ''' <returns>A Boolean value indicating if the firewall allows Application "Exceptions".</returns>
    Public ReadOnly Property AppAuthorizationsAllowed() As Boolean
        Get
            Return IsFirewallInstalled AndAlso Not fwMgr.LocalPolicy.CurrentProfile.ExceptionsNotAllowed
        End Get
    End Property

    ''' Adds an application to the list of authorized applications.
    ''' If the application is already authorized, does nothing.
    ''' The full path to the application executable. This cannot be blank, and cannot be a relative path.
    ''' This is the name of the application, purely for display purposes in the Microsoft Security Center.
    Public Sub GrantAuthorization(applicationFullPath As String, appName As String)
        If String.IsNullOrWhiteSpace(applicationFullPath) Then Throw New ArgumentNullException("applicationFullPath must not be blank")
        If String.IsNullOrWhiteSpace(appName) Then Throw New ArgumentNullException("appName must not be blank")
        If applicationFullPath.IndexOfAny(IO.Path.GetInvalidPathChars()) >= 0 Then Throw New ArgumentException("applicationFullPath must not contain invalid path characters")
        If Not IO.Path.IsPathRooted(applicationFullPath) Then Throw New ArgumentException("applicationFullPath is not an absolute path")
        If Not IO.File.Exists(applicationFullPath) Then Throw New IO.FileNotFoundException("File does not exist", applicationFullPath)

        If Not IsFirewallInstalled Then Throw New FirewallHelperException("Cannot grant authorization: Firewall is not installed.")
        If Not AppAuthorizationsAllowed Then Throw New FirewallHelperException("Application exemptions are not allowed.")

        If Not HasAuthorization(applicationFullPath) Then
            Dim authAppType As Type = Type.GetTypeFromProgID("HNetCfg.FwAuthorizedApplication", False)
            Dim appInfo As INetFwAuthorizedApplication = Nothing

            If authAppType IsNot Nothing Then
                Try
                    appInfo = CType(Activator.CreateInstance(authAppType), INetFwAuthorizedApplication)
                Catch ex As ArgumentException
                Catch ex As NotSupportedException
                Catch ex As Reflection.TargetInvocationException
                Catch ex As MissingMethodException
                Catch ex As MethodAccessException
                Catch ex As MemberAccessException
                Catch ex As InvalidComObjectException
                Catch ex As COMException
                Catch ex As TypeLoadException
                End Try
            End If

            If appInfo Is Nothing Then Throw New FirewallHelperException("Could not grant authorization: can't create INetFwAuthorizedApplication instance.")

            appInfo.Name = appName
            appInfo.ProcessImageFileName = applicationFullPath

            fwMgr.LocalPolicy.CurrentProfile.AuthorizedApplications.Add(appInfo)
        End If
    End Sub

    ''' Removes an application from the list of authorized applications.
    ''' Note that the specified application must exist or a FileNotFound exception will be thrown.
    ''' If the specified application exists but does not currently have authorization, this method will do nothing.
    ''' The full path to the application executable. This cannot be blank, and cannot be a relative path.
    Public Sub RemoveAuthorization(applicationFullPath As String)
        If String.IsNullOrWhiteSpace(applicationFullPath) Then Throw New ArgumentNullException("applicationFullPath must not be blank")
        If applicationFullPath.IndexOfAny(IO.Path.GetInvalidPathChars()) >= 0 Then Throw New ArgumentException("applicationFullPath must not contain invalid path characters")
        If Not IO.Path.IsPathRooted(applicationFullPath) Then Throw New ArgumentException("applicationFullPath is not an absolute path")
        If Not IO.File.Exists(applicationFullPath) Then Throw New IO.FileNotFoundException("File does not exist", applicationFullPath)

        If Not IsFirewallInstalled Then Throw New FirewallHelperException("Cannot remove authorization: Firewall is not installed.")

        If HasAuthorization(applicationFullPath) Then fwMgr.LocalPolicy.CurrentProfile.AuthorizedApplications.Remove(applicationFullPath)
    End Sub

    ''' Returns whether an application is in the list of authorized applications.
    ''' Note if the file does not exist, this throws a FileNotFound exception.
    ''' The full path to the application executable. This cannot be blank, and cannot be a relative path.
    ''' <returns>A Boolean value indicating if the program has been granted access through the Windows Firewall.</returns>
    Public Function HasAuthorization(applicationFullPath As String) As Boolean
        If String.IsNullOrWhiteSpace(applicationFullPath) Then Throw New ArgumentNullException("applicationFullPath must not be blank")
        If applicationFullPath.IndexOfAny(IO.Path.GetInvalidPathChars()) >= 0 Then Throw New ArgumentException("applicationFullPath must not contain invalid path characters")
        If Not IO.Path.IsPathRooted(applicationFullPath) Then Throw New ArgumentException("applicationFullPath is not an absolute path")
        If Not IO.File.Exists(applicationFullPath) Then Throw New IO.FileNotFoundException("File does not exist.", applicationFullPath)

        If Not IsFirewallInstalled Then Throw New FirewallHelperException("Cannot remove authorization: Firewall is not installed.")

        For Each appName As String In GetAuthorizedAppPaths()
            If appName.Equals(applicationFullPath, StringComparison.OrdinalIgnoreCase) Then Return True
        Next

        Return False
    End Function

    ''' <summary>
    ''' Returns a list of programs authorized by the Windows Firewall.
    ''' </summary>
    ''' <returns>An ArrayList of Strings.</returns>
    Public Function GetAuthorizedAppPaths() As ArrayList
        If Not IsFirewallInstalled Then Throw New FirewallHelperException("Cannot remove authorization: Firewall is not installed.")

        Dim list As New ArrayList()

        For Each app As INetFwAuthorizedApplication In fwMgr.LocalPolicy.CurrentProfile.AuthorizedApplications
            list.Add(app.ProcessImageFileName)
        Next

        Return list
    End Function
End Class

Public Class FirewallHelperException
    Inherits Exception

    Public Sub New(message As String)
        MyBase.New(message)
    End Sub
End Class