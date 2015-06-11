using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Controls;
using System.Windows.Forms;
using Microsoft.Win32;
using SebWindowsClient.ProcessUtils;
using SebWindowsClient.Properties;
using SebWindowsClient.XULRunnerCommunication;


namespace SebWindowsClient.UI
{
    public class SEBOnScreenKeyboardToolStripButton : SEBToolStripButton
    {
        public delegate void KeyboardStateChangedEventHandler(bool shown);
        public event KeyboardStateChangedEventHandler OnKeyboardStateChanged;

        public SEBOnScreenKeyboardToolStripButton()
        {
            InitializeComponent();
            this.Alignment = ToolStripItemAlignment.Right;
        }

        protected override void OnClick(EventArgs e)
        {
            if (TapTipHandler.IsKeyboardVisible())
            {
                TapTipHandler.HideKeyboard();
            }
            else
            {
                TapTipHandler.ShowKeyboard();
            }
        }

        private void InitializeComponent()
        {
            // 
            // SEBOnScreenKeyboardToolStripButton
            // 
            this.ToolTipText = SEBUIStrings.toolTipOnScreenKeyboard;
            base.Image = (Bitmap)Resources.ResourceManager.GetObject("keyboard");

            SEBXULRunnerWebSocketServer.OnXulRunnerTextFocus += OnTextFocus;
            SEBXULRunnerWebSocketServer.OnXulRunnerTextBlur += OnTextBlur;

            TapTipHandler.OnKeyboardStateChanged += shown => OnKeyboardStateChanged(shown);
        }

        private void OnTextBlur(object sender, EventArgs e)
        {
            TapTipHandler.HideKeyboard();
        }

        private void OnTextFocus(object sender, EventArgs e)
        {
            TapTipHandler.ShowKeyboard();
        }

        protected override void Dispose(bool disposing)
        {
            SEBXULRunnerWebSocketServer.OnXulRunnerTextFocus -= OnTextFocus;
            SEBXULRunnerWebSocketServer.OnXulRunnerTextBlur -= OnTextBlur;
            base.Dispose(disposing);
        }
    }

    public static class TapTipHandler
    {
        public delegate void KeyboardStateChangedEventHandler(bool shown);
        public static event KeyboardStateChangedEventHandler OnKeyboardStateChanged;

        public static void ShowKeyboard()
        {
            if (IsKeyboardVisible())
                return;

            try
            {
                if (!SEBWindowHandler.AllowedExecutables.Contains("taptip.exe"))
                    SEBWindowHandler.AllowedExecutables.Add("tabtip.ex");

                string progFiles = @"C:\Program Files\Common Files\Microsoft Shared\ink";
                string onScreenKeyboardPath = Path.Combine(progFiles, "TabTip.exe");
                Process.Start(onScreenKeyboardPath);
                if (OnKeyboardStateChanged != null)
                {
                    OnKeyboardStateChanged(true);
                    var t = new System.Windows.Forms.Timer();
                    t.Interval = 500;
                    t.Start();
                    t.Tick += (sender, args) =>
                    {
                        if (!IsKeyboardVisible())
                        {
                            OnKeyboardStateChanged(false);
                            t.Stop();
                        }
                    };
                }
            }
            catch
            { }
        }


        public static void HideKeyboard()
        {
            if (!IsKeyboardVisible())
                return;

            try
            {
                //Kill all on screen keyboards
                foreach (Process onscreenProcess in Process.GetProcessesByName("TabTip"))
                {
                    onscreenProcess.Kill();
                }
                if (OnKeyboardStateChanged != null)
                {
                    OnKeyboardStateChanged(false);
                }
            }
            catch
            { }
        }

        /// <summary>
        /// The window is disabled. See http://msdn.microsoft.com/en-gb/library/windows/desktop/ms632600(v=vs.85).aspx.
        /// </summary>
        public const UInt32 WS_DISABLED = 0x8000000;

        /// <summary>
        /// Specifies we wish to retrieve window styles.
        /// </summary>
        public const int GWL_STYLE = -16;

        [DllImport("user32.dll")]
        public static extern IntPtr FindWindow(String sClassName, String sAppName);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern UInt32 GetWindowLong(IntPtr hWnd, int nIndex);


        /// <summary>
        /// Gets the window handler for the virtual keyboard.
        /// </summary>
        /// <returns>The handle.</returns>
        public static IntPtr GetKeyboardWindowHandle()
        {
            return FindWindow("IPTip_Main_Window", null);
        }

        /// <summary>
        /// Checks to see if the virtual keyboard is visible.
        /// </summary>
        /// <returns>True if visible.</returns>
        public static bool IsKeyboardVisible()
        {
            IntPtr keyboardHandle = GetKeyboardWindowHandle();

            bool visible = false;

            if (keyboardHandle != IntPtr.Zero)
            {
                UInt32 style = GetWindowLong(keyboardHandle, GWL_STYLE);
                visible = ((style & WS_DISABLED) != WS_DISABLED);
            }

            return visible;
        }

        public static bool IsKeyboardDocked()
        {
            int docked = 1;

            try
            {
                //HKEY_CURRENT_USER\Software\Microsoft\TabletTip\1.7\EdgeTargetDockedState -> 0 = floating, 1 = docked
                docked = (int)Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\TabletTip\1.7\", "EdgeTargetDockedState", 1);
            }
            catch { }

            return docked == 1;

        }
    }
}
