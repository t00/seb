using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SebWindowsClient.ConfigurationUtils;
using SebWindowsClient.ProcessUtils;

namespace SebWindowsClient
{
    public partial class WindowChooser : Form
    {
        private Process _process;
        private List<KeyValuePair<IntPtr, string>> _openedWindows;
        /// <summary>
        /// This displays a small window where the icons and titles of the opened windows are placed and shows them on above the icon in the taskbar (just like windows does)
        /// </summary>
        /// <param name="process">The process that handles the window(s)</param>
        /// <param name="left">The positiop where the icon on the taskbar is placed</param>
        /// <param name="top">The positiop where the icon on the taskbar is placed</param>
        public WindowChooser(Process process, int left, int top)
        {
            this.Left = left;
            this.Top = top - 75;
            InitializeComponent();
            this.appList.Click += ShowWindow;
            try
            {
                _process = process;
                var appImages = new ImageList();
                if ((int)SEBClientInfo.getSebSetting(SEBSettings.KeyTouchOptimized)[SEBSettings.KeyTouchOptimized] == 1)
                {
                    appImages.ImageSize = new Size(48, 48);
                    this.Height = this.Height + 16;
                    this.Top = this.Top - 10;
                }
                else
                {
                    appImages.ImageSize = new Size(32, 32);
                }
                appImages.ColorDepth = ColorDepth.Depth32Bit;

                _openedWindows = process.GetOpenWindows().ToList();

                //Add the mainwindowhandle if not yet added
                if (_process.MainWindowHandle != IntPtr.Zero && !_openedWindows.Any(oW => oW.Key == _process.MainWindowHandle))
                {
                    _openedWindows.Add(new KeyValuePair<IntPtr, string>(_process.MainWindowHandle, _process.MainWindowTitle));
                }

                //Directly show the window if just one is opened
                if (_openedWindows.Count == 1)
                {
                    ShowWindow(_openedWindows.First().Key);
                }

                //Add the icons
                int index = 0;
                foreach (var openWindow in _openedWindows)
                {
                    Image image = GetSmallWindowIcon(openWindow.Key);
                    appImages.Images.Add(image);
                    appList.Items.Add(openWindow.Value, index);
                    index++;
                }

                this.appList.View = View.LargeIcon;
                this.appList.LargeImageList = appImages;
            }
            catch (Exception)
            {
                this.Close();
            }

            //Calculate the width
            this.Width = Math.Min(appList.Items.Count*200,Screen.PrimaryScreen.Bounds.Width);
            this.Show();
            this.appList.Focus();

            //Hide it after 3 secs
            var t = new Timer();
            t.Tick += CloseIt;
            t.Interval = 3000;
            t.Start();
        }

        private void CloseIt(object sender, EventArgs e)
        {
            this.Close();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            Console.WriteLine("Closing");
            this.appList.Click -= ShowWindow;
            base.OnClosing(e);
        }

        private void ShowWindow(object sender, EventArgs e)
        {
            var selectedIndex = appList.SelectedIndices[0];
            ShowWindow(_openedWindows[selectedIndex].Key);
        }

        /// <summary>
        /// Shows the window
        /// </summary>
        /// <param name="windowHandle"></param>
        private void ShowWindow(IntPtr windowHandle)
        {
            windowHandle.BringToTop();

            //If we are working in touch optimized mode, open every window in full screen (e.g. maximized), except XULRunner because it seems not to accept the working area property and resizes to fully fullscreen
            if ((int)SEBClientInfo.getSebSetting(SEBSettings.KeyTouchOptimized)[SEBSettings.KeyTouchOptimized] == 1
                        && !_process.ProcessName.Contains("xulrunner"))
                _openedWindows.First().Key.MaximizeWindow();

            this.Close();
        }

        public static Image GetSmallWindowIcon(IntPtr hWnd)
        {
            try
            {
                IntPtr hIcon = default(IntPtr);

                hIcon = SendMessage(hWnd, WM_GETICON, ICON_SMALL2, IntPtr.Zero);

                if (hIcon == IntPtr.Zero)
                    hIcon = GetClassLongPtr(hWnd, GCL_HICON);

                if (hIcon == IntPtr.Zero)
                    hIcon = LoadIcon(IntPtr.Zero, (IntPtr)0x7F00/*IDI_APPLICATION*/);

                if (hIcon != IntPtr.Zero)
                    return new Bitmap(Icon.FromHandle(hIcon).ToBitmap(), 32, 32);
                else
                    return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        static IntPtr GetClassLongPtr(IntPtr hWnd, int nIndex)
        {
            if (IntPtr.Size == 4)
                return new IntPtr((long)GetClassLong32(hWnd, nIndex));
            else
                return GetClassLong64(hWnd, nIndex);
        }

        private static uint WM_GETICON = 0x007f;
        private static IntPtr ICON_SMALL2 = new IntPtr(2);
        private static int GCL_HICON = -14;


        [DllImport("user32.dll")]
        static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        static extern IntPtr LoadIcon(IntPtr hInstance, IntPtr lpIconName);

        [DllImport("user32.dll", EntryPoint = "GetClassLong")]
        static extern uint GetClassLong32(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", EntryPoint = "GetClassLongPtr")]
        static extern IntPtr GetClassLong64(IntPtr hWnd, int nIndex);

    }



}
