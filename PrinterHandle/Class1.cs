using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;
namespace PrinterHandle
{
    public class Class1
    {

    }
    public class LPTControl
    {
        private string LptStr = "lpt1";
        public LPTControl(string l_LPT_Str)
        {
            LptStr = l_LPT_Str;
        }
        [StructLayout(LayoutKind.Sequential)]
        private struct OVERLAPPED
        {
            int Internal;
            int InternalHigh;
            int Offset;
            int OffSetHigh;
            int hEvent;
        }
        [DllImport("kernel32.dll")]
        private static extern int CreateFile(
            string lpFileName,
            uint dwDesiredAccess,
            int dwShareMode,
            int lpSecurityAttributes,
            int dwCreationDisposition,
            int dwFlagsAndAttributes,
            int hTemplateFile
            );
        [DllImport("kernel32.dll")]
        private static extern bool WriteFile(
            int hFile,
            byte[] lpBuffer,
            int nNumberOfBytesToWrite,
            ref int lpNumberOfBytesWritten,
            ref OVERLAPPED lpOverlapped
            );
        [DllImport("kernel32.dll")]
        private static extern bool CloseHandle(
            int hObject
            );
        private int iHandle;
        public bool Open()
        {
            iHandle = CreateFile(LptStr, 0x40000000, 0, 0, 3, 0, 0);
            if (iHandle != -1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool Write(String Mystring)
        {
            if (iHandle != -1)
            {
                OVERLAPPED x = new OVERLAPPED();
                int i = 0;
                
                byte[] mybyte = System.Text.Encoding.Default.GetBytes(Mystring);
                bool b = WriteFile(iHandle, mybyte, mybyte.Length, ref i, ref x);
                return b;
            }
            else
            {
                throw new Exception("");
            }
        }
        public bool Write(byte[] mybyte)
        {
            if (iHandle != -1)
            {
                OVERLAPPED x = new OVERLAPPED();
                int i = 0;
                WriteFile(iHandle, mybyte, mybyte.Length,
                    ref i, ref x);
                return true;
            }
            else
            {
                throw new Exception("!");
            }
        }
        public bool Close()
        {
            return CloseHandle(iHandle);
        }
    }
}
