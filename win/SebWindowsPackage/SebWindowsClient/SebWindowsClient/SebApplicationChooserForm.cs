using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SebWindowsClient.ConfigurationUtils;
using System.Diagnostics;
using SebWindowsClient.DesktopUtils;
using System.Runtime.InteropServices;

// -------------------------------------------------------------
//     Viktor tomas
//     BFH-TI, http://www.ti.bfh.ch
//     Biel, 2012
// -------------------------------------------------------------
namespace SebWindowsClient
{
    public partial class SebApplicationChooserForm : Form
    {
        List<IntPtr> lWindowHandles = new List<IntPtr>();

        private static uint WM_GETICON = 0x007f;
        private static IntPtr ICON_SMALL2 = new IntPtr(2);
        private static IntPtr IDI_APPLICATION = new IntPtr(0x7F00);
        private static int GCL_HICON = -14;

        /// <summary>
        /// Enumeration of the different ways of showing a window using 
        /// ShowWindow</summary>
        private enum WindowShowStyle : uint
        {
            Hide = 0,
            ShowNormal = 1,
            ShowMinimized = 2,
            ShowMaximized = 3,
            Maximize = 3,
            ShowNormalNoActivate = 4,
            Show = 5,
            Minimize = 6,
            ShowMinNoActivate = 7,
            ShowNoActivate = 8,
            Restore = 9,
            ShowDefault = 10,
            /// <summary>Windows 2000/XP: Minimizes a window, even if the thread 
            /// that owns the window is hung. This flag should only be used when 
            /// minimizing windows from a different thread.</summary>
            /// <remarks>See SW_FORCEMINIMIZE</remarks>
            ForceMinimized = 11
        }

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, WindowShowStyle nCmdShow);

        [DllImportAttribute("User32.dll")]
        private static extern IntPtr SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        static extern IntPtr LoadIcon(IntPtr hInstance, IntPtr lpIconName);

        [DllImport("user32.dll", EntryPoint = "GetClassLong")]
        static extern uint GetClassLong32(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", EntryPoint = "GetClassLongPtr")]
        static extern IntPtr GetClassLong64(IntPtr hWnd, int nIndex);


        /// ----------------------------------------------------------------------------------------
        /// <summary>
        /// Constructor - initialise components.
        /// </summary>
        /// ----------------------------------------------------------------------------------------
        public SebApplicationChooserForm()
        {
            InitializeComponent();
            //fillListApplications();
        }

        /// <summary>
        /// 64 bit version maybe loses significant 64-bit specific information
        /// </summary>
        static IntPtr GetClassLongPtr(IntPtr hWnd, int nIndex)
        {
            if (IntPtr.Size == 4)
                return new IntPtr((long)GetClassLong32(hWnd, nIndex));
            else
                return GetClassLong64(hWnd, nIndex);
        }

        /// ----------------------------------------------------------------------------------------
        /// <summary>
        /// Get Process Icon.
        /// </summary>
        /// <returns>Process Icon</returns>
        /// ----------------------------------------------------------------------------------------
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
                    return new Bitmap(Icon.FromHandle(hIcon).ToBitmap(), 16, 16);
                else
                    return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// ----------------------------------------------------------------------------------------
        /// <summary>
        /// Fill listApplications with running Applications, lWindowHandles with Window Handles
        ///  of running Applications and ilApplicationIcons with running Applications Icons.
        /// </summary>
        /// ----------------------------------------------------------------------------------------
        public void fillListApplications()
        {
            List<string> lRunningApplications = new List<string>();
            ImageList ilApplicationIcons = new ImageList();
            this.lWindowHandles.Clear();
            int index = 0;

            if (SEBClientInfo.sebClientConfig.PermittedProcesses.Count() > 0)
            {
                Process[] runningApplications = SEBDesktopController.GetInputProcessesWithGI();
                for (int i = 0; i < SEBClientInfo.sebClientConfig.PermittedProcesses.Count(); i++)
                {
                    for (int j = 0; j < runningApplications.Count(); j++)
                    {
                        if (SEBClientInfo.sebClientConfig.PermittedProcesses[i].NameWin.Contains(runningApplications[j].ProcessName))
                        {
                            this.lWindowHandles.Add(runningApplications[j].MainWindowHandle);
                            lRunningApplications.Add(runningApplications[j].ProcessName);
                            ilApplicationIcons.Images.Add("rAppIcon" + index, GetSmallWindowIcon(runningApplications[j].MainWindowHandle));
                            index++;
                        }
                    }
                }

            }
 
            // Suspending automatic refreshes as items are added/removed.
            this.listApplications.BeginUpdate();
            this.listApplications.Clear();
            this.listApplications.View = View.LargeIcon;
            this.listApplications.LargeImageList = ilApplicationIcons;
 
            //listApplications.SmallImageList = imagesSmall;
            //listApplications.LargeImageList = imagesLarge;
            for (int i = 0; i < lRunningApplications.Count(); i++)
            {
                ListViewItem listItem = new ListViewItem(lRunningApplications[i]);
                listItem.ImageIndex = i;
                this.listApplications.Items.Add(listItem);
            }
            this.listApplications.Dock = DockStyle.Fill;
            //listApplications.k
            // Re-enable the display.
            this.listApplications.EndUpdate();

        }

         /// ----------------------------------------------------------------------------------------
        /// <summary>
        /// Set selected Process window in foreground.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// ----------------------------------------------------------------------------------------
        private void listApplications_ItemActivate(object sender, EventArgs e)
        {
            // identify which button was clicked and perform necessary actions
            ListView listApplications = sender as ListView;
            if (listApplications.SelectedItems.Count > 0)
            {
                ListViewItem listViewItem = listApplications.SelectedItems[0]; //the second time you will get the selected item here
                ShowWindow(lWindowHandles[listViewItem.Index], WindowShowStyle.Restore);
                SetForegroundWindow(lWindowHandles[listViewItem.Index]);
            }
        }

    //    public void fillText(string text)
    //    {
    //        textBox1.Text = "";
    //        textBox1.Text = text;
    //    }
    }
}
