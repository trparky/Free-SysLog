Imports System.Runtime.InteropServices
Imports Microsoft.Win32.SafeHandles

Namespace NativeMethod
    Friend Class NativeMethods
        <DllImport("User32.dll", EntryPoint:="SetForegroundWindow")>
        Public Shared Function SetForegroundWindow(hWnd As Integer) As Integer
        End Function

        <DllImport("kernel32.dll")>
        Friend Shared Function OpenProcess(dwDesiredAccess As ProcessAccessFlags, bInheritHandle As Boolean, dwProcessId As Integer) As IntPtr
        End Function

        <DllImport("kernel32.dll", SetLastError:=True)>
        Friend Shared Function CloseHandle(hHandle As IntPtr) As Boolean
        End Function

        <DllImport("kernel32.dll", CharSet:=CharSet.Unicode)>
        Friend Shared Function QueryFullProcessImageName(hprocess As IntPtr, dwFlags As Integer, lpExeName As Text.StringBuilder, ByRef size As Integer) As Boolean
        End Function

        <DllImport("shell32.dll", CharSet:=CharSet.Unicode, ExactSpelling:=True)>
        Public Shared Function ILCreateFromPathW(pszPath As String) As IntPtr
        End Function
        <DllImport("shell32.dll", ExactSpelling:=True)>
        Public Shared Function SHOpenFolderAndSelectItems(pidlList As IntPtr, cild As UInteger, children As IntPtr, dwFlags As UInteger) As Integer
        End Function

        <DllImport("shell32.dll", ExactSpelling:=True)>
        Public Shared Sub ILFree(pidlList As IntPtr)
        End Sub

        <DllImport("user32.dll")>
        Public Shared Function SetForegroundWindow(hwnd As IntPtr) As Boolean
        End Function

        <DllImport("iphlpapi.dll", SetLastError:=True)>
        Public Shared Function GetExtendedTcpTable(pTcpTable As IntPtr, ByRef dwOutBufLen As Integer, sort As Boolean, ipVersion As Integer, tblClass As TCP_TABLE_CLASS, reserved As Integer) As UInteger
        End Function

        <DllImport("iphlpapi.dll", SetLastError:=True)>
        Public Shared Function GetExtendedUdpTable(pUdpTable As IntPtr, ByRef dwOutBufLen As Integer, sort As Boolean, ipVersion As Integer, tableClass As UDP_TABLE_CLASS, reserved As Integer) As UInteger
        End Function

        <DllImport("user32.dll")>
        Public Shared Function GetForegroundWindow() As IntPtr
        End Function

        <DllImport("kernel32.dll", SetLastError:=True)>
        Public Shared Function DeviceIoControl(hDevice As SafeFileHandle, dwIoControlCode As UInteger, lpInBuffer As IntPtr, nInBufferSize As UInteger, lpOutBuffer As IntPtr, nOutBufferSize As UInteger, ByRef lpBytesReturned As UInteger, lpOverlapped As IntPtr) As Boolean
        End Function

        Public Const FSCTL_SET_COMPRESSION As UInteger = &H9C040
        Public Const COMPRESSION_FORMAT_NONE As UShort = 0
        Public Const COMPRESSION_FORMAT_DEFAULT As UShort = 1

        <DllImport("kernel32.dll", CharSet:=CharSet.Auto, SetLastError:=True)>
        Public Shared Function GetCompressedFileSize(lpFileName As String, ByRef lpFileSizeHigh As UInteger) As UInteger
        End Function
    End Class

    Module APIs
        Public Enum UDP_TABLE_CLASS
            UDP_TABLE_BASIC
            UDP_TABLE_OWNER_PID
            UDP_TABLE_OWNER_MODULE
        End Enum

        <StructLayout(LayoutKind.Sequential)>
        Public Structure MIB_UDPROW_OWNER_PID
            Public localAddr As UInteger
            Public localPort As UInteger
            Public owningPid As UInteger
        End Structure

        Public Enum MIB_TCP_STATE
            CLOSED = 1
            LISTENING = 2
            SYN_SENT = 3
            SYN_RCVD = 4
            ESTABLISHED = 5
            FIN_WAIT1 = 6
            FIN_WAIT2 = 7
            CLOSE_WAIT = 8
            CLOSING = 9
            LAST_ACK = 10
            TIME_WAIT = 11
            DELETE_TCB = 12
        End Enum

        <StructLayout(LayoutKind.Sequential)>
        Public Structure MIB_TCPROW_OWNER_PID
            Public state As UInteger
            Public localAddr As UInteger
            Public localPort As UInteger
            Public remoteAddr As UInteger
            Public remotePort As UInteger
            Public owningPid As UInteger
        End Structure

        Public Enum TCP_TABLE_CLASS
            TCP_TABLE_BASIC_LISTENER
            TCP_TABLE_BASIC_CONNECTIONS
            TCP_TABLE_BASIC_ALL
            TCP_TABLE_OWNER_PID_LISTENER
            TCP_TABLE_OWNER_PID_CONNECTIONS
            TCP_TABLE_OWNER_PID_ALL
            TCP_TABLE_OWNER_MODULE_LISTENER
            TCP_TABLE_OWNER_MODULE_CONNECTIONS
            TCP_TABLE_OWNER_MODULE_ALL
        End Enum

        <Flags>
        Public Enum ProcessAccessFlags As UInteger
            PROCESS_QUERY_LIMITED_INFORMATION = &H1000
            All = &H1F0FFF
            Terminate = &H1
            CreateThread = &H2
            VirtualMemoryOperation = &H8
            VirtualMemoryRead = &H10
            VirtualMemoryWrite = &H20
            DuplicateHandle = &H40
            CreateProcess = &H80
            SetQuota = &H100
            SetInformation = &H200
            QueryInformation = &H400
            QueryLimitedInformation = &H1000
            Synchronize = &H100000
        End Enum
    End Module
End Namespace