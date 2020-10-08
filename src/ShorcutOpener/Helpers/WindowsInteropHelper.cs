﻿using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;
using System.Windows.Media;
using Point = System.Windows.Point;

namespace ShorcutOpener.Helpers
{
    public static class WindowsInteropHelper
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1310:Field names should not contain underscore", Justification = "Matching COM")]
        private const int GWL_STYLE = -16;
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1310:Field names should not contain underscore", Justification = "Matching COM")]
        private const int WS_SYSMENU = 0x80000;
        private static IntPtr _hwnd_shell;
        private static IntPtr _hwnd_desktop;

        // Accessors for shell and desktop handlers
        // Will set the variables once and then will return them
        private static IntPtr HWND_SHELL
        {
            get
            {
                return _hwnd_shell != IntPtr.Zero ? _hwnd_shell : _hwnd_shell = NativeMethods.GetShellWindow();
            }
        }

        private static IntPtr HWND_DESKTOP
        {
            get
            {
                return _hwnd_desktop != IntPtr.Zero ? _hwnd_desktop : _hwnd_desktop = NativeMethods.GetDesktopWindow();
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct INPUT
        {
            public INPUTTYPE Type;
            public InputUnion Data;

            public static int Size
            {
                get { return Marshal.SizeOf(typeof(INPUT)); }
            }
        }

        [StructLayout(LayoutKind.Explicit)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Matching COM")]
        internal struct InputUnion
        {
            [FieldOffset(0)]
            internal MOUSEINPUT mi;
            [FieldOffset(0)]
            internal KEYBDINPUT ki;
            [FieldOffset(0)]
            internal HARDWAREINPUT hi;
        }

        [StructLayout(LayoutKind.Sequential)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Matching COM")]
        internal struct MOUSEINPUT
        {
            internal int dx;
            internal int dy;
            internal int mouseData;
            internal uint dwFlags;
            internal uint time;
            internal UIntPtr dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Matching COM")]
        internal struct KEYBDINPUT
        {
            internal short wVk;
            internal short wScan;
            internal uint dwFlags;
            internal int time;
            internal UIntPtr dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Matching COM")]
        internal struct HARDWAREINPUT
        {
            internal int uMsg;
            internal short wParamL;
            internal short wParamH;
        }

        internal enum INPUTTYPE : uint
        {
            INPUTMOUSE = 0,
            INPUTKEYBOARD = 1,
            INPUTHARDWARE = 2,
        }

        private const string WindowClassConsole = "ConsoleWindowClass";
        private const string WindowClassWinTab = "Flip3D";
        private const string WindowClassProgman = "Progman";
        private const string WindowClassWorkerW = "WorkerW";

        public static bool IsWindowFullscreen()
        {
            IntPtr hWnd = NativeMethods.GetForegroundWindow();

            if (hWnd != null && !hWnd.Equals(IntPtr.Zero))
            {
                if (!(hWnd.Equals(HWND_DESKTOP) || hWnd.Equals(HWND_SHELL)))
                {
                    StringBuilder sb = new StringBuilder(256);
                    _ = NativeMethods.GetClassName(hWnd, sb, sb.Capacity);
                    string windowClass = sb.ToString();

                    if (windowClass == WindowClassWinTab)
                    {
                        return false;
                    }

                    _ = NativeMethods.GetWindowRect(hWnd, out RECT appBounds);

                    if (windowClass == WindowClassConsole)
                    {
                        return appBounds.Top < 0 && appBounds.Bottom < 0;
                    }

                    if (windowClass == WindowClassProgman || windowClass == WindowClassWorkerW)
                    {
                        IntPtr hWndDesktop = NativeMethods.FindWindowEx(hWnd, IntPtr.Zero, "SHELLDLL_DefView", null);
                        hWndDesktop = NativeMethods.FindWindowEx(hWndDesktop, IntPtr.Zero, "SysListView32", "FolderView");
                        if (hWndDesktop != null && !hWndDesktop.Equals(IntPtr.Zero))
                        {
                            return false;
                        }
                    }

                    Rectangle screenBounds = Screen.FromHandle(hWnd).Bounds;
                    if ((appBounds.Bottom - appBounds.Top) == screenBounds.Height && (appBounds.Right - appBounds.Left) == screenBounds.Width)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// disable windows toolbar's control box
        /// this will also disable system menu with Alt+Space hotkey
        /// </summary>
        public static void DisableControlBox(Window win)
        {
            var hwnd = new WindowInteropHelper(win).Handle;
            _ = NativeMethods.SetWindowLong(hwnd, GWL_STYLE, NativeMethods.GetWindowLong(hwnd, GWL_STYLE) & ~WS_SYSMENU);
        }

        /// <summary>
        /// Transforms pixels to Device Independent Pixels used by WPF
        /// </summary>
        /// <param name="visual">current window, required to get presentation source</param>
        /// <param name="unitX">horizontal position in pixels</param>
        /// <param name="unitY">vertical position in pixels</param>
        /// <returns>point containing device independent pixels</returns>
        public static Point TransformPixelsToDIP(Visual visual, double unitX, double unitY)
        {
            Matrix matrix;
            var source = PresentationSource.FromVisual(visual);
            if (source != null)
            {
                matrix = source.CompositionTarget.TransformFromDevice;
            }
            else
            {
                using (var src = new HwndSource(default))
                {
                    matrix = src.CompositionTarget.TransformFromDevice;
                }
            }

            return new Point((int)(matrix.M11 * unitX), (int)(matrix.M22 * unitY));
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }
    }
}
