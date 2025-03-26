using System;
using System.Runtime.InteropServices;

namespace Free_SysLog.NativeMethod
{
    internal class NativeMethods
    {
        [DllImport("User32.dll", EntryPoint = "SetForegroundWindow")]
        public static extern int SetForegroundWindow(int hWnd);

        [DllImport("kernel32.dll")]
        internal static extern IntPtr OpenProcess(APIs.ProcessAccessFlags dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern bool CloseHandle(IntPtr hHandle);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        internal static extern bool QueryFullProcessImageName(IntPtr hprocess, int dwFlags, System.Text.StringBuilder lpExeName, ref int size);

        [DllImport("shell32.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
        public static extern IntPtr ILCreateFromPathW(string pszPath);
        [DllImport("shell32.dll", ExactSpelling = true)]
        public static extern int SHOpenFolderAndSelectItems(IntPtr pidlList, uint cild, IntPtr children, uint dwFlags);

        [DllImport("shell32.dll", ExactSpelling = true)]
        public static extern void ILFree(IntPtr pidlList);
    }

    static class APIs
    {
        [Flags]
        public enum ProcessAccessFlags : uint
        {
            PROCESS_QUERY_LIMITED_INFORMATION = 0x1000U,
            All = 0x1F0FFFU,
            Terminate = 0x1U,
            CreateThread = 0x2U,
            VirtualMemoryOperation = 0x8U,
            VirtualMemoryRead = 0x10U,
            VirtualMemoryWrite = 0x20U,
            DuplicateHandle = 0x40U,
            CreateProcess = 0x80U,
            SetQuota = 0x100U,
            SetInformation = 0x200U,
            QueryInformation = 0x400U,
            QueryLimitedInformation = 0x1000U,
            Synchronize = 0x100000U
        }
    }
}