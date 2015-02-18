using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Threading;
using SebWindowsClient.ConfigurationUtils;
using SebWindowsClient.ProcessUtils;
using SebWindowsClient.Properties;
using SebWindowsClient.XULRunnerCommunication;


namespace SebWindowsClient.UI
{
    public class SEBOnScreenKeyboardToolStripButton : SEBToolStripButton
    {
        public SEBOnScreenKeyboardToolStripButton()
        {
            InitializeComponent();
            this.Alignment = ToolStripItemAlignment.Right;
        }

        private void CheckTablet()
        {
        
        }

        protected override void OnClick(EventArgs e)
        {
            ShowKeyboard();
            //if (_keyBoardProcess == null)
            //{
            //    string progFiles = @"C:\Program Files\Common Files\Microsoft Shared\ink";
            //    SEBWindowHandler.AllowedExecutables.Add("tabtip.ex");
            //    string onScreenKeyboardPath = Path.Combine(progFiles, "TabTip.exe");
            //    _keyBoardProcess = Process.Start(onScreenKeyboardPath);
            //}
            //else
            //{
            //    //Kill all on screen keyboards
            //    Process[] oskProcessArray = Process.GetProcessesByName("TabTip");
            //    foreach (Process onscreenProcess in oskProcessArray)
            //    {
            //        onscreenProcess.Kill();
            //    }
            //    _keyBoardProcess = null;
            //}
        }

        public static void ShowKeyboard()
        {
            try
            {
                if ((Boolean)SEBClientInfo.getSebSetting(SEBSettings.KeyTouchOptimized)[SEBSettings.KeyTouchOptimized] == true)
                {
                    string progFiles = @"C:\Program Files\Common Files\Microsoft Shared\ink";
                    SEBWindowHandler.AllowedExecutables.Add("tabtip.ex");
                    string onScreenKeyboardPath = Path.Combine(progFiles, "TabTip.exe");
                    Process.Start(onScreenKeyboardPath);
                }
            }
            catch
            {}
        }

        public static void HideKeyboard()
        {
            try
            {
                //Kill all on screen keyboards
                Process[] oskProcessArray = Process.GetProcessesByName("TabTip");
                foreach (Process onscreenProcess in oskProcessArray)
                {
                    onscreenProcess.Kill();
                }
            }
            catch
            {}
            
        }

        private void InitializeComponent()
        {
            // 
            // SEBOnScreenKeyboardToolStripButton
            // 
            this.ToolTipText = "Show/Hide on Screen Keyboard";
            base.Image = (Bitmap)Resources.ResourceManager.GetObject("keyboard");

            SEBXULRunnerWebSocketServer.OnXulRunnerTextFocus += OnTextFocus;
            SEBXULRunnerWebSocketServer.OnXulRunnerTextBlur += OnTextBlur;
        }

        private void OnTextBlur(object sender, EventArgs e)
        {
            HideKeyboard();
        }

        private void OnTextFocus(object sender, EventArgs e)
        {
            ShowKeyboard();
        }

        protected override void Dispose(bool disposing)
        {
            SEBXULRunnerWebSocketServer.OnXulRunnerTextFocus -= OnTextFocus;
            SEBXULRunnerWebSocketServer.OnXulRunnerTextBlur -= OnTextBlur;
            base.Dispose(disposing);
        }
    }
}
