using System;
using System.Diagnostics;

namespace Free_SysLog.ProcessHandling
{
    static class ProcessHandling
    {
        /// <summary>Checks to see if a Process ID or PID exists on the system.</summary>
        /// <param name="PID">The PID of the process you are checking the existance of.</param>
        /// <param name="processObject">If the PID does exist, the function writes back to this argument in a ByRef way a Process Object that can be interacted with outside of this function.</param>
        /// <returns>Return a Boolean value. If the PID exists, it return a True value. If the PID doesn't exist, it returns a False value.</returns>
        private static bool DoesProcessIDExist(int PID, ref Process processObject)
        {
            try
            {
                processObject = Process.GetProcessById(PID);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private static void KillProcess(int processID)
        {
            KillProcessSubRoutine(processID);
            System.Threading.Thread.Sleep(250); // We're going to sleep to give the system some time to kill the process.
            KillProcessSubRoutine(processID);
            System.Threading.Thread.Sleep(250); // We're going to sleep (again) to give the system some time to kill the process.
        }

        private static void KillProcessSubRoutine(int processID)
        {
            Process processObject = null;
            if (DoesProcessIDExist(processID, ref processObject))
            {
                try
                {
                    processObject.Kill();
                }
                catch (Exception)
                {
                    // Wow, it seems that even with double-checking if a process exists by it's PID number things can still go wrong.
                    // So this Try-Catch block is here to trap any possible errors when trying to kill a process by it's PID number.
                }
            }
        }

        public static void SearchForProcessAndKillIt(string strFileName, bool boolFullFilePathPassed)
        {
            string processExecutablePath;

            foreach (Process process in Process.GetProcesses())
            {
                processExecutablePath = GetProcessExecutablePath(process.Id);

                if (!string.IsNullOrWhiteSpace(processExecutablePath))
                {
                    try
                    {
                        processExecutablePath = boolFullFilePathPassed ? new System.IO.FileInfo(processExecutablePath).FullName : new System.IO.FileInfo(processExecutablePath).Name;
                        if (strFileName.Equals(processExecutablePath, StringComparison.OrdinalIgnoreCase))
                            KillProcess(process.Id);
                    }
                    catch (ArgumentException)
                    {
                    }
                }
            }
        }

        private static string GetProcessExecutablePath(int processID)
        {
            try
            {
                var memoryBuffer = new System.Text.StringBuilder(1024);
                var processHandle = NativeMethod.NativeMethods.OpenProcess(NativeMethod.APIs.ProcessAccessFlags.PROCESS_QUERY_LIMITED_INFORMATION, false, processID);

                if (!processHandle.Equals(IntPtr.Zero))
                {
                    try
                    {
                        int memoryBufferSize = memoryBuffer.Capacity;
                        if (NativeMethod.NativeMethods.QueryFullProcessImageName(processHandle, 0, memoryBuffer, ref memoryBufferSize))
                            return memoryBuffer.ToString();
                    }
                    finally
                    {
                        NativeMethod.NativeMethods.CloseHandle(processHandle);
                    }
                }

                NativeMethod.NativeMethods.CloseHandle(processHandle);
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}