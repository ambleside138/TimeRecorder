using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace TimeRecorder.Controls.WindowLocation;

public static class NativeMethods
{
    [DllImport("user32.dll")]
    public static extern bool SetWindowPlacement(IntPtr hWnd, [In] ref WINDOWPLACEMENT lpwndpl);

    [DllImport("user32.dll")]
    public static extern bool GetWindowPlacement(IntPtr hWnd, out WINDOWPLACEMENT lpwndpl);

}
