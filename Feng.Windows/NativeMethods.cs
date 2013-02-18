using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Feng.Windows
{
    internal sealed class NativeMethods
    {
        /// <summary>
        /// QueryPerformanceCounter
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        [DllImport("Kernel32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool QueryPerformanceCounter([MarshalAs(UnmanagedType.I8)] ref long count);

        /// <summary>
        /// QueryPerformanceFrequency
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        [DllImport("Kernel32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool QueryPerformanceFrequency([MarshalAs(UnmanagedType.I8)] ref long count);

        //const int MF_BYPOSITION = 0x400;

        //[DllImport("User32")]
        //private static extern int RemoveMenu(IntPtr hMenu, int nPosition, int wFlags);

        //[DllImport("User32")]
        //private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        //[DllImport("User32")]
        //private static extern int GetMenuItemCount(IntPtr hWnd);
    }
}
