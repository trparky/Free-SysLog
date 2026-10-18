Imports System.Runtime.InteropServices

Namespace WineSupport
    Public Module WINEDetection
        <DllImport("ntdll.dll", CallingConvention:=CallingConvention.Cdecl)>
        Private Function wine_get_version() As IntPtr
        End Function

        <DllImport("ntdll.dll", CallingConvention:=CallingConvention.Cdecl)>
        Private Sub wine_get_host_version(ByRef sysname As IntPtr, ByRef release As IntPtr)
        End Sub

        Public Function IsRunningUnderWine() As Boolean
            Try
                Dim ptr As IntPtr = wine_get_version()
                Return ptr <> IntPtr.Zero
            Catch ex As EntryPointNotFoundException
                Return False
            Catch ex As DllNotFoundException
                Return False
            End Try
        End Function

        Public Function GetWineVersion() As String
            If Not IsRunningUnderWine() Then Return Nothing

            Try
                Dim ptr As IntPtr = wine_get_version()

                If ptr = IntPtr.Zero Then Return Nothing

                Return Marshal.PtrToStringAnsi(ptr)
            Catch
                Return Nothing
            End Try
        End Function

        Public Function GetWineHostOS() As String
            If Not IsRunningUnderWine() Then Return Nothing

            Try
                Dim sysnamePtr As IntPtr = IntPtr.Zero
                Dim releasePtr As IntPtr = IntPtr.Zero

                wine_get_host_version(sysnamePtr, releasePtr)

                If sysnamePtr = IntPtr.Zero Then Return Nothing

                Return Marshal.PtrToStringAnsi(sysnamePtr)
            Catch
                Return Nothing
            End Try
        End Function
    End Module
End Namespace