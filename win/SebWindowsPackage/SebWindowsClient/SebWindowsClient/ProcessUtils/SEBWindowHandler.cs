﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using SebShared.DiagnosticUtils;
using System.Windows.Forms;

namespace SebWindowsClient.ProcessUtils
{
    /// <summary>
    /// Offers methods to handle windows
    /// </summary>
    public static class SEBWindowHandler
    {

        #region Public Members

        /// <summary>
        /// A list of not allowed window titles (the title must not exactly match but only contain the values in here
        /// </summary>
        public static List<string> AllowedExecutables = new List<string>();

        /// <summary>
        /// The possible actions for a window defined by ShowWindowAsync()
        /// </summary>
        enum ShowWindowCommand
        {
            SW_HIDE = 0,
            SW_SHOWNORMAL = 1,
            SW_SHOWMINIMIZED = 2,
            SW_SHOWMAXIMIZED = 3,
            SW_SHOWNOACTIVATE = 4,
            SW_RESTORE = 9,
            SW_SHOWDEFAULT = 10,
            SW_MAX = 11
        }

        #endregion

        #region Private Members

        private static List<IntPtr> _hiddenWindowHandles = new List<IntPtr>();
        private static ForegroundWatchDog _foregroundChecker;

        #endregion

        #region Public Methods

        /// <summary>
        /// Checks if the process that holds the windowhandle is allowed to show its window
        /// </summary>
        /// <param name="processName"></param>
        /// <returns></returns>
        public static bool IsWindowAllowedByProcessName(string processName)
        {
            if (String.IsNullOrWhiteSpace(processName))
                return false;

            processName = processName.ToLower();

            //If no allowed apps are specified, meaning all apps are allowed return true
            if (AllowedExecutables.Count == 0)
                return true;
            //If explicitly allowed return true
            if (AllowedExecutables.Count > 0 && AllowedExecutables.Any(ex => ex.Contains(processName) || processName.Contains(ex)))
                return true;

            //else return false
            return false;
        }

        /// <summary>Returns a dictionary that contains the handle and title of all the open windows.</summary>
        /// <returns>A dictionary that contains the handle and title (in lower case) of all the open windows.</returns>
        public static IDictionary<IntPtr, string> GetOpenWindows()
        {
            try
            {
                var lShellWindow = GetShellWindow();
                var lWindows = new Dictionary<IntPtr, string>();

                EnumWindows(delegate(IntPtr hWnd, int lParam)
                {
                    if (hWnd == lShellWindow) return true;
                    if (!IsWindowVisible(hWnd)) return true;

                    var lLength = GetWindowTextLength(hWnd);
                    if (lLength == 0) return true;

                    var lBuilder = new StringBuilder(lLength);
                    GetWindowText(hWnd, lBuilder, lLength + 1);

                    lWindows[hWnd] = lBuilder.ToString().ToLower();
                    return true;

                }, 0);

                return lWindows;
            }
            catch (Exception ex)
            {
                Logger.AddError("Unable to retrieve open windows", null, ex);
                return new Dictionary<IntPtr, string>();
            }
            
        }

        /// <summary>
        /// Hides all open windows except the ones defined in AllowedApps
        /// </summary>
        public static void HideAllOpenWindows()
        {
            EditAllOpenWindows(ShowWindowCommand.SW_HIDE);
        }

        private const int WM_COMMAND = 0x111;
        private const int MIN_ALL = 419;
        /// <summary>
        /// Minimizes all open windows except the ones defines in AllowedApps
        /// </summary>
        /// <param name="force">If force is true then it minimized ALL open windows, else it minimized only not explicitly allowed executables</param>
        public static void MinimizeAllOpenWindows(bool force = false)
        {
            if (force)
            {
                //Minimize ALL Open Windows
                IntPtr l_Hwnd = FindWindow("Shell_TrayWnd", null);
                SendMessage(l_Hwnd, WM_COMMAND, (IntPtr)MIN_ALL, IntPtr.Zero);
            }
            else
            {
                EditAllOpenWindows(ShowWindowCommand.SW_SHOWMINIMIZED);
            }
            

        }

        /// <summary>
        /// Returns all Window handles that contain the specified title
        /// </summary>
        /// <param name="title">the title of the window, case insensitive</param>
        /// <returns>A List of window handles</returns>
        public static List<IntPtr> GetWindowHandlesByTitle(string title)
        {
            try
            {
                title = title.ToLower();
                return
                    GetOpenWindows()
                        .Where(lWindow => lWindow.Value.Contains(title))
                        .Select(lWindow => lWindow.Key)
                        .ToList();
            }
            catch (Exception ex)
            {
                Logger.AddError("Unable to GetWindowHandlesByTitle " + title, null, ex);
                return new List<IntPtr>();
            }
            
        }

        /// <summary>
        /// Returns the first window handle found that contains the title
        /// </summary>
        /// <param name="title">the title of the window, case insensitivee</param>
        /// <returns>The window handle or IntPtr.Zero</returns>
        public static IntPtr GetWindowHandleByTitle(string title)
        {
            try
            {
                title = title.ToLower();
                return GetOpenWindows().FirstOrDefault(lWindow => lWindow.Value.Contains(title)).Key;
            }
            catch (Exception)
            {
                return IntPtr.Zero;
            }
        }

        /// <summary>
        /// Shows all Windows that have been hidden
        /// </summary>
        public static void RestoreHiddenWindows()
        {
            foreach (var handle in _hiddenWindowHandles)
            {
                EditWindowByHandle(handle, ShowWindowCommand.SW_RESTORE);
            }
        }

        #endregion

        #region WindowHandle Extensions (IntPtr)

        /// <summary>
        /// Hides the specified window
        /// </summary>
        /// <param name="handle">The handle of the window</param>
        public static void HideWindow(this IntPtr handle)
        {
            EditWindowByHandle(handle, ShowWindowCommand.SW_HIDE);
        }

        /// <summary>
        /// Minimize the window
        /// </summary>
        /// <param name="handle">The handle</param>
        /// <param name="waitForShowingUp">If this is a live event to catch a window when it comes up, then you must wait for it to show unless you can minimize it</param>
        public static void MinimizeWindow(this IntPtr windowHandle, bool waitForShowingUp = false)
        {
            EditWindowByHandle(windowHandle, ShowWindowCommand.SW_SHOWMINIMIZED, waitForShowingUp);
        }

        /// <summary>
        /// Minimize the window
        /// </summary>
        /// <param name="handle">The handle</param>
        public static void MaximizeWindow(this IntPtr windowHandle)
        {
            EditWindowByHandle(windowHandle, ShowWindowCommand.SW_SHOWMAXIMIZED);
        }

        /// <summary>
        /// Restore the window
        /// </summary>
        /// <param name="windowHandle">The handle</param>
        public static void AdaptWindowToWorkingArea(this IntPtr windowHandle, int? taskbarHeight = null)
        {
            EditWindowByHandle(windowHandle, ShowWindowCommand.SW_SHOWNORMAL);
            if (taskbarHeight != null)
            {
                MoveWindow(windowHandle, 0, 0, Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height - taskbarHeight.Value, true);    
            }
            else
            {
                MoveWindow(windowHandle, 0, 0, Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.WorkingArea.Height, true);
            }
            
            EditWindowByHandle(windowHandle, ShowWindowCommand.SW_SHOWMAXIMIZED);
        }

        public static int GetWindowHeight(this IntPtr windowHandle)
        {
            var rct = new RECT();
            if (!GetWindowRect(new HandleRef(null, windowHandle), out rct))
            {
                return 0;
            }
            return rct.Bottom - rct.Top;
        }

        public static string GetWindowTitle(this IntPtr windowHandle)
        {
            try
            {
                const int nChars = 256;
                var buff = new StringBuilder(nChars);

                var title = "";
                if (GetWindowText(windowHandle, buff, nChars) > 0)
                {
                    title = buff.ToString();
                }
                return title;
            }
            catch (Exception ex)
            {
                Logger.AddError("Unable to GetWindowTitle",null,ex);
                return "";
            }
        }

        public static Process GetProcess(this IntPtr windowHandle)
        {
            try
            {
                uint procId;
                GetWindowThreadProcessId(windowHandle, out procId);
                return Process.GetProcessById((int)procId);
            }
            catch (Exception ex)
            {
                Logger.AddError("Unable to get Process",null,ex);
                return null;
            }
                
        }

        public static void BringToTop(this IntPtr windowHandle, bool restoreIfMinimizedOrHidden = true)
        {
            SetForegroundWindow(windowHandle);
            BringWindowToTop(windowHandle);
            var windowState = windowHandle.GetPlacement().showCmd;
            if (restoreIfMinimizedOrHidden &&
                (windowState == (int) ShowWindowCommand.SW_SHOWMINIMIZED ||
                windowState == (int) ShowWindowCommand.SW_HIDE))
            {
                EditWindowByHandle(windowHandle, ShowWindowCommand.SW_RESTORE);
            }
        }


        const int WM_CLOSE = 0x0010;
        public static void CloseWindow(this IntPtr windowHandle)
        {
            SendMessage(windowHandle, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
        }

        private static WINDOWPLACEMENT GetPlacement(this IntPtr hwnd)
        {
            var placement = new WINDOWPLACEMENT();
            placement.length = Marshal.SizeOf(placement);
            GetWindowPlacement(hwnd, ref placement);
            return placement;
        }

        public static bool IsAllowed(this IntPtr windowHandle)
        {
            var proc = windowHandle.GetProcess();
            if (proc == null) return false;
            return proc.IsWindowAllowed();
        }

        #endregion

        #region Foreground Checker control

        /// <summary>
        /// Enables a foreground checker that does the specified action to all windows that come to foreground and are not in AllowedApps
        /// </summary>
        public static void EnableForegroundWatchDog()
        {
            if (_foregroundChecker == null)
                _foregroundChecker = new ForegroundWatchDog();
            _foregroundChecker.StartWatchDog();
        }

        /// <summary>
        /// Disables the foreground checker if enabled
        /// </summary>
        public static void DisableForegroundWatchDog()
        {
            if (_foregroundChecker != null)
            {
                _foregroundChecker.StopWatchDog();
            }
        }

        #endregion

        #region Process Extensions

        public static bool IsWindowAllowed(this Process process)
        {
            return IsWindowAllowedByProcessName(process.GetExecutableName());
        }

        #endregion

        #region Private Methods

        private static void EditAllOpenWindows(ShowWindowCommand action)
        {
            foreach (KeyValuePair<IntPtr, string> lWindow in GetOpenWindows())
            {
                var handle = lWindow.Key;
                if (!handle.IsAllowed())
                    EditWindowByHandle(handle, action);
            }
        }

        const int SC_MINIMIZE = 0xF020;
        const int SC_MAXIMIZE = 0xF030;
        const int SC_CLOSE = 0xF060;
        private static void EditWindowByHandle(IntPtr windowHandle, ShowWindowCommand action, bool waitForShowingUp = false)
        {
            if (action == ShowWindowCommand.SW_HIDE)
                _hiddenWindowHandles.Add(windowHandle);

            //Do this in a sepparate Thread because in case of minimizing windows the thread needs to wait for the window to show up
            ThreadPool.QueueUserWorkItem(
                delegate
                {
                    try
                    {
                        //If you want to minimize the window you have to wait for it to show up
                        if (action == ShowWindowCommand.SW_SHOWMINIMIZED && waitForShowingUp)
                            Thread.Sleep(500);
                        //ShowWindow is the standard way of editing the window status
                        if (!ShowWindowAsync(windowHandle, (int) action))
                        {
                            //If ShowWindowAsync failes (attention: it returns a value != zero if the state has been changed, if the state has remained unchained it returns false)
                            //Do it the hard way
                            if (action == ShowWindowCommand.SW_SHOWMINIMIZED)
                            {
                                SendMessage(windowHandle, 274, (IntPtr) SC_MINIMIZE, IntPtr.Zero);
                                Logger.AddInformation(String.Format("Window {0} minimized",
                                    windowHandle.GetWindowTitle()));
                            }
                            else if (action == ShowWindowCommand.SW_HIDE)
                            {
                                //This is a drastic action! If an application cannot be handled by the previous actions, for example if it has administrator privileges, then it gets closed!
                                SendMessage(windowHandle, 274, (IntPtr) SC_CLOSE, IntPtr.Zero);
                                Logger.AddInformation(String.Format(
                                    "Window {0} closed, because i was unable to hide it", windowHandle.GetWindowTitle()));
                            }
                            else if (action == ShowWindowCommand.SW_SHOWMAXIMIZED)
                            {
                                SendMessage(windowHandle, 274, (IntPtr) SC_MAXIMIZE, IntPtr.Zero);
                                Logger.AddInformation(String.Format("Window {0} Maximized",
                                    windowHandle.GetWindowTitle()));
                            }
                        }
                        else
                        {
                            Logger.AddInformation(String.Format("Window {0} {1}",
                                    windowHandle.GetWindowTitle(), action.ToString()));
                        }
                    }
                    catch
                    {
                        try
                        {
                            //If ShowWindowAsync failes (attention: it returns a value != zero if the state has been changed, if the state has remained unchained it returns false)
                            //Do it the hard way
                            if (action == ShowWindowCommand.SW_SHOWMINIMIZED)
                            {
                                SendMessage(windowHandle, 274, (IntPtr)SC_MINIMIZE, IntPtr.Zero);
                                Logger.AddInformation(String.Format("Window {0} minimized", windowHandle.GetWindowTitle()));
                            }
                            else if (action == ShowWindowCommand.SW_HIDE)
                            {
                                //This is a drastic action! If an application cannot be handled by the previous actions, for example if it has administrator privileges, then it gets closed!
                                SendMessage(windowHandle, 274, (IntPtr)SC_CLOSE, IntPtr.Zero);
                                Logger.AddInformation(String.Format("Window {0} closed, because i was unable to hide it", windowHandle.GetWindowTitle()));
                            }
                            else if (action == ShowWindowCommand.SW_SHOWMAXIMIZED)
                            {
                                SendMessage(windowHandle, 274, (IntPtr)SC_MAXIMIZE, IntPtr.Zero);
                                Logger.AddInformation(String.Format("Window {0} Maximized", windowHandle.GetWindowTitle()));
                            }
                        }
                        catch (Exception)
                        {
                        }
                    }
                });
        }

        delegate bool EnumWindowsProc(IntPtr hWnd, int lParam);

        #endregion

        #region DLL Imports

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

        [DllImport("user32.dll")]
        private static extern bool ShowWindowAsync(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

        [DllImport("user32.dll")]
        public static extern uint GetWindowThreadProcessId(IntPtr hwnd, out uint lpdwProcessId);

        [DllImport("USER32.DLL")]
        static extern bool EnumWindows(EnumWindowsProc enumFunc, int lParam);

        [DllImport("USER32.DLL")]
        static extern int GetWindowTextLength(IntPtr hWnd);

        [DllImport("USER32.DLL")]
        static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport("USER32.DLL")]
        static extern IntPtr GetShellWindow();

        [DllImport("user32.dll", EntryPoint = "SendMessage", SetLastError = true)]
        private static extern IntPtr SendMessage(IntPtr hWnd, Int32 Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImportAttribute("User32.dll")]
        private static extern IntPtr SetForegroundWindow(IntPtr hWnd);
        [DllImportAttribute("User32.dll")]
        public static extern IntPtr GetForegroundWindow();

        [DllImportAttribute("user32.dll", EntryPoint = "BringWindowToTop")]
        public static extern bool BringWindowToTop([InAttribute()] IntPtr hWnd);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetWindowPlacement(IntPtr hWnd, ref WINDOWPLACEMENT lpwndpl);

        private struct WINDOWPLACEMENT
        {
            public int length;
            public int flags;
            public int showCmd;
            public System.Drawing.Point ptMinPosition;
            public System.Drawing.Point ptMaxPosition;
            public System.Drawing.Rectangle rcNormalPosition;
        }

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetWindowRect(HandleRef hWnd, out RECT lpRect);

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;        // x position of upper-left corner
            public int Top;         // y position of upper-left corner
            public int Right;       // x position of lower-right corner
            public int Bottom;      // y position of lower-right corner
        }

        #endregion
    }

    class ForegroundWatchDog
    {
        delegate void WinEventDelegate(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime);
        private WinEventDelegate dele = null;

        //Window Event Constants
        private const uint WINEVENT_OUTOFCONTEXT = 0;
        private const uint EVENT_SYSTEM_FOREGROUND = 3;
        private const uint EVENT_SYSTEM_MINIMIZEEND = 16;
        private const uint EVENT_SYSTEM_MOVESIZEEND = 10;
        private const uint EVENT_SYSTEM_SWITCHEND = 21;
        private const uint EVENT_SYSTEM_CAPTURESTART = 8;
        private const uint EVENT_SYSTEM_SWITCHSTART = 20;

        private List<IntPtr> _hooks = new List<IntPtr>();

        private bool running = false;

        public ForegroundWatchDog()
        {
            if (dele == null)
                dele = WinEventProc;

            SetWinEventHook(EVENT_SYSTEM_FOREGROUND, EVENT_SYSTEM_FOREGROUND, IntPtr.Zero, dele, 0, 0,
                    WINEVENT_OUTOFCONTEXT);
            SetWinEventHook(EVENT_SYSTEM_CAPTURESTART, EVENT_SYSTEM_CAPTURESTART, IntPtr.Zero, dele, 0, 0,
                WINEVENT_OUTOFCONTEXT);
        }

        public void StartWatchDog()
        {
            running = true;
        }

        public void StopWatchDog()
        {
            running = false;
        }

        public void WinEventProc(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime)
        {
            //Only if running
            if (!running) return;

            Console.WriteLine(hwnd);

            if (hwnd == IntPtr.Zero) return;

            if (!hwnd.IsAllowed())
                hwnd.HideWindow();
        }

        #region DLL Imports

        [DllImport("user32.dll")]
        static extern IntPtr SetWinEventHook(uint eventMin, uint eventMax, IntPtr hmodWinEventProc, WinEventDelegate lpfnWinEventProc, uint idProcess, uint idThread, uint dwFlags);

        [DllImport("user32.dll")]
        public static extern uint GetWindowThreadProcessId(IntPtr hwnd, out uint lpdwProcessId);

        #endregion
    }

    public class AppBarHandler
    {
        public enum Edge : int
        {
            Left = 0,
            Top,
            Right,
            Bottom
        }

        public AppBarHandler(Form form, Edge edge)
        {
            this.form = form;
            this.edge = edge;
        }

        public void RegisterBar()
        {
            if (fBarRegistered)
            {
                return;
            }
            APPBARDATA abd = new APPBARDATA();
            abd.cbSize = Marshal.SizeOf(abd);
            abd.hWnd = form.Handle;
            uCallBack = RegisterWindowMessage("AppBarMessage");
            abd.uCallbackMessage = uCallBack;

            uint ret = SHAppBarMessage((int)ABMsg.ABM_NEW, ref abd);
            fBarRegistered = true;

            SetAppBarPos(edge);
        }

        public void UnregisterBar()
        {
            if (fBarRegistered)
            {
                return;
            }
            APPBARDATA abd = new APPBARDATA();
            abd.cbSize = Marshal.SizeOf(abd);
            abd.hWnd = form.Handle;
            SHAppBarMessage((int) ABMsg.ABM_REMOVE, ref abd);
            fBarRegistered = false;
        }

        public void SetAppBarPos(Edge edge)
        {
            APPBARDATA abd = new APPBARDATA();
            abd.cbSize = Marshal.SizeOf(abd);
            abd.hWnd = form.Handle;
            abd.uEdge = (int)edge;

            if (abd.uEdge == (int)Edge.Left || abd.uEdge == (int)Edge.Right)
            {
                abd.rc.Top = 0;
                abd.rc.Bottom = SystemInformation.PrimaryMonitorSize.Height;
                if (abd.uEdge == (int)Edge.Left)
                {
                    abd.rc.Left = 0;
                    abd.rc.Right = form.Size.Width;
                }
                else
                {
                    abd.rc.Right = SystemInformation.PrimaryMonitorSize.Width;
                    abd.rc.Left = abd.rc.Right - form.Size.Width;
                }

            }
            else
            {
                abd.rc.Left = 0;
                abd.rc.Right = SystemInformation.PrimaryMonitorSize.Width;
                if (abd.uEdge == (int)Edge.Top)
                {
                    abd.rc.Top = 0;
                    abd.rc.Bottom = form.Size.Height;
                }
                else
                {
                    abd.rc.Bottom = SystemInformation.PrimaryMonitorSize.Height;
                    abd.rc.Top = abd.rc.Bottom - form.Size.Height;
                }
            }

            SHAppBarMessage((int)ABMsg.ABM_QUERYPOS, ref abd);

            switch (abd.uEdge)
            {
                case (int)Edge.Left:
                    abd.rc.Right = abd.rc.Left + form.Size.Width;
                    break;
                case (int)Edge.Right:
                    abd.rc.Left = abd.rc.Right - form.Size.Width;
                    break;
                case (int)Edge.Top:
                    abd.rc.Bottom = abd.rc.Top + form.Size.Height;
                    break;
                case (int)Edge.Bottom:
                    abd.rc.Top = abd.rc.Bottom - form.Size.Height;
                    break;
            }

            SHAppBarMessage((int)ABMsg.ABM_SETPOS, ref abd);
            MoveWindow(abd.hWnd, abd.rc.Left, abd.rc.Top, abd.rc.Right - abd.rc.Left, abd.rc.Bottom - abd.rc.Top, true);
        }

        public void HandleWndProc(Message m)
        {
            if (m.Msg == uCallBack)
            {
                switch (m.WParam.ToInt32())
                {
                    case (int)ABNotify.ABN_POSCHANGED:
                        SetAppBarPos(edge);
                        break;
                }
            }
        }

        private static bool fBarRegistered;
        private static int uCallBack;
        private readonly Form form;
        private Edge edge;

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;        // x position of upper-left corner
            public int Top;         // y position of upper-left corner
            public int Right;       // x position of lower-right corner
            public int Bottom;      // y position of lower-right corner
        }

        [StructLayout(LayoutKind.Sequential)]
        struct APPBARDATA
        {
            public int cbSize;
            public IntPtr hWnd;
            public int uCallbackMessage;
            public int uEdge;
            public RECT rc;
            public IntPtr lParam;
        }

        enum ABMsg : int
        {
            ABM_NEW = 0,
            ABM_REMOVE,
            ABM_QUERYPOS,
            ABM_SETPOS,
            ABM_GETSTATE,
            ABM_GETTASKBARPOS,
            ABM_ACTIVATE,
            ABM_GETAUTOHIDEBAR,
            ABM_SETAUTOHIDEBAR,
            ABM_WINDOWPOSCHANGED,
            ABM_SETSTATE
        }
        enum ABNotify : int
        {
            ABN_STATECHANGE = 0,
            ABN_POSCHANGED,
            ABN_FULLSCREENAPP,
            ABN_WINDOWARRANGE
        }
        [DllImport("SHELL32", CallingConvention = CallingConvention.StdCall)]
        static extern uint SHAppBarMessage(int dwMessage, ref APPBARDATA pData);

        [DllImport("USER32")]
        static extern int GetSystemMetrics(int Index);

        [DllImport("User32.dll", ExactSpelling = true, CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        static extern bool MoveWindow(IntPtr hWnd, int x, int y, int cx, int cy, bool repaint);

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        static extern int RegisterWindowMessage(string msg);
    }
}
