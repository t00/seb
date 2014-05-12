using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Input;
using System.Windows.Threading;
using SebWindowsClient.Properties;


namespace SebWindowsClient.UI
{
    public class SEBOnScreenKeyboardToolStripButton : SEBToolStripButton
    {
        private Process _keyBoardProcess;
        public SEBOnScreenKeyboardToolStripButton()
        {
            base.Image = (Bitmap)Resources.ResourceManager.GetObject("keyboard");
        }

        private void CheckTablet()
        {
        
        }

        protected override void OnClick(EventArgs e)
        {
            if (_keyBoardProcess == null)
            {
                string progFiles = @"C:\Program Files\Common Files\Microsoft Shared\ink";
                string onScreenKeyboardPath = Path.Combine(progFiles, "TabTip.exe");
                _keyBoardProcess = Process.Start(onScreenKeyboardPath);
            }
            else
            {
                //Kill all on screen keyboards
                Process[] oskProcessArray = Process.GetProcessesByName("TabTip");
                foreach (Process onscreenProcess in oskProcessArray)
                {
                    onscreenProcess.Kill();
                }
                _keyBoardProcess = null;
            }
        }

        private void InitializeComponent()
        {
            // 
            // SEBOnScreenKeyboardToolStripButton
            // 
            this.ToolTipText = "Show/Hide on Screen Keyboard";

        }
    }
}
