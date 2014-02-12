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

        private static int appChooserFormXPadding = 32;
        private static int appChooserFormXGap = 45;

        private int selectedItemIndex = 0;

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

        [DllImportAttribute("User32.dll")]
        private static extern IntPtr SetActiveWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        static extern IntPtr LoadIcon(IntPtr hInstance, IntPtr lpIconName);

        [DllImport("user32.dll", EntryPoint = "GetClassLong")]
        static extern uint GetClassLong32(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", EntryPoint = "GetClassLongPtr")]
        static extern IntPtr GetClassLong64(IntPtr hWnd, int nIndex);

        [DllImport("User32.DLL")]
        private static extern int AttachThreadInput(uint CurrentForegroundThread, uint MakeThisThreadForegrouond, bool boolAttach);

        [DllImport("user32.dll")]
        static extern uint GetWindowThreadProcessId(IntPtr hWnd, IntPtr ProcessId);

        [DllImportAttribute("user32.dll", EntryPoint = "GetForegroundWindow")]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("kernel32.dll")]
        public static extern uint GetCurrentThreadId();

        [DllImportAttribute("user32.dll", EntryPoint = "BringWindowToTop")]
        public static extern bool BringWindowToTop([InAttribute()] IntPtr hWnd);



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
                    return new Bitmap(Icon.FromHandle(hIcon).ToBitmap(), 32, 32);
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
            //ilApplicationIcons.TransparentColor = Color.White;
            ilApplicationIcons.ImageSize = new Size(32, 32);
            ilApplicationIcons.ColorDepth = ColorDepth.Depth32Bit;
            this.lWindowHandles.Clear();
            int index = 0;
            List<object> permittedProcessList = (List<object>)SEBClientInfo.getSebSetting(SEBSettings.KeyPermittedProcesses)[SEBSettings.KeyPermittedProcesses];
            if (permittedProcessList.Count > 0)
            {
                List<Process> runningApplications = SEBClientInfo.SebWindowsClientForm.permittedProcessesReferences;
                for (int i = 0; i < permittedProcessList.Count(); i++)
                {
                    Dictionary<string, object> permittedProcess = (Dictionary<string, object>)permittedProcessList[i];
                    string permittedProcessExecutable = (string)permittedProcess[SEBSettings.KeyExecutable];
                    string permittedProcessTitle = (string)permittedProcess[SEBSettings.KeyTitle];
                    if ((Boolean)permittedProcess[SEBSettings.KeyActive])
                    {
                        for (int j = 0; j < runningApplications.Count(); j++)
                        {
                            if (runningApplications[j] != null)
                            {
                                try
                                {
                                    runningApplications[j].Refresh();
                                    if (!runningApplications[j].HasExited)
                                    {
                                        if (permittedProcessExecutable.Contains(runningApplications[j].ProcessName))
                                        {
                                            this.lWindowHandles.Add(runningApplications[j].MainWindowHandle);
                                            //lRunningApplications.Add(runningApplications[j].ProcessName);
                                            lRunningApplications.Add(permittedProcessTitle);
                                            if (permittedProcessExecutable == "xulrunner.exe")
                                            {
                                                Bitmap sebIconBitmap = Icon.ExtractAssociatedIcon(Application.ExecutablePath).ToBitmap();
                                                //sebIconBitmap.MakeTransparent(BackColor);
                                                ilApplicationIcons.Images.Add(sebIconBitmap);
                                            }
                                            else
                                            {
                                                ilApplicationIcons.Images.Add("rAppIcon" + index, GetSmallWindowIcon(runningApplications[j].MainWindowHandle));
                                            }
                                            index++;
                                        }
                                    }
                                }
                                catch (Exception)
                                {
                                    
                                }
                            }
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

            // Calculate necessary size of the app chooser form according to number of applications/icons
            int numberIcons = lRunningApplications.Count();
            int formWidth;
            if (numberIcons > 0) formWidth = 2 * appChooserFormXPadding + numberIcons * 32 + (numberIcons - 1) * appChooserFormXGap;
            else formWidth = 2 * appChooserFormXPadding;
            // Check if calculated width is larger that current screen width, if yes, adjust height accordingly
            if (Screen.PrimaryScreen.Bounds.Width < formWidth)
            {
                formWidth = Screen.PrimaryScreen.Bounds.Width;
                this.Height = 140;
            } 

            this.Width = formWidth;
            this.CenterToScreen();

            // Re-enable the display.
            this.listApplications.EndUpdate();
            if (this.listApplications.Items.Count > 0)
            {
                this.listApplications.Items[0].Selected = true;
            }
            selectedItemIndex = 0;
            if (this.listApplications.Items.Count > 1)
            {
                this.listApplications.Items[1].Selected = true;
            }
            selectedItemIndex = 1;
            SelectNextListItem();
        }

        /// ----------------------------------------------------------------------------------------
        /// <summary>
        /// Fill listApplications with running Applications, lWindowHandles with Window Handles
        ///  of running Applications and ilApplicationIcons with running Applications Icons.
        /// </summary>
        /// ----------------------------------------------------------------------------------------
        public void SelectNextListItem()
        {
            //this.Focus();
            if (this.listApplications.Items.Count > 0)
            {
                int lastSelectedItemIndex = selectedItemIndex-1;
                if (selectedItemIndex >= listApplications.Items.Count)
                {
                    selectedItemIndex = 0;
                }
                if (lastSelectedItemIndex < 0)
                {
                    lastSelectedItemIndex = listApplications.Items.Count - 1;
                }
                this.listApplications.Items[selectedItemIndex].Selected = true;
                this.listApplications.Items[selectedItemIndex].Focused = true;
                string selectedThreadName = this.listApplications.Items[selectedItemIndex].Text;
                if (!selectedThreadName.Contains("xulrunner"))
                {
                    //this.listApplications.Items[selectedItemIndex].ForeColor = Color.White;
                    //this.listApplications.Items[lastSelectedItemIndex].ForeColor = Color.Black; ;
                    //uint selectedWindowThreadId = GetWindowThreadProcessId(this.Handle, IntPtr.Zero);
                    IntPtr hWndForegroundWindow = GetForegroundWindow();
                    uint activeThreadID = GetWindowThreadProcessId(hWndForegroundWindow, IntPtr.Zero);
                    uint currentThreadID = GetCurrentThreadId();  //GetWindowThreadProcessId(GetForegroundWindow(), IntPtr.Zero);
                    AttachThreadInput(activeThreadID, currentThreadID, true);
                    SetForegroundWindow(lWindowHandles[selectedItemIndex]);
                    BringWindowToTop(lWindowHandles[selectedItemIndex]);
                    ShowWindow(lWindowHandles[selectedItemIndex], WindowShowStyle.ShowNormal);
                    AttachThreadInput(activeThreadID, currentThreadID, false);
                }
                else
                {
                    IntPtr hWndForegroundWindow = GetForegroundWindow();
                    uint activeThreadID = GetWindowThreadProcessId(hWndForegroundWindow, IntPtr.Zero);
                    uint currentThreadID = GetCurrentThreadId();  //GetWindowThreadProcessId(GetForegroundWindow(), IntPtr.Zero);
                    AttachThreadInput(activeThreadID, currentThreadID, true);
                    SetForegroundWindow(lWindowHandles[selectedItemIndex]);
                    //BringWindowToTop(lWindowHandles[selectedItemIndex]);
                    //ShowWindow(lWindowHandles[selectedItemIndex], WindowShowStyle.ShowNormal);
                    AttachThreadInput(activeThreadID, currentThreadID, false);
                }
                selectedItemIndex++;
                this.listApplications.Focus();
            }
        }

        public static void forceSetForegroundWindow(IntPtr hWnd)
        {
            uint selectedWindowThreadId = GetWindowThreadProcessId(hWnd, IntPtr.Zero);
            uint foregroundThreadID = GetCurrentThreadId();  //GetWindowThreadProcessId(GetForegroundWindow(), IntPtr.Zero);
            if (foregroundThreadID != selectedWindowThreadId)
            {
                AttachThreadInput(selectedWindowThreadId, foregroundThreadID, true);
                SetForegroundWindow(hWnd);
                AttachThreadInput(selectedWindowThreadId, foregroundThreadID, false);
            }
            else
                SetForegroundWindow(hWnd);
        }
         /// ----------------------------------------------------------------------------------------
        /// <summary>
        /// Set selected Process window in foreground.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// ----------------------------------------------------------------------------------------
        //private void listApplications_ItemActivate(object sender, EventArgs e)
        //{
        //    // identify which button was clicked and perform necessary actions
        //    ListView listApplications = sender as ListView;
        //    if (listApplications.SelectedItems.Count > 0)
        //    {
        //        ListViewItem listViewItem = listApplications.SelectedItems[0]; //the second time you will get the selected item here
        //        SetForegroundWindow(lWindowHandles[listViewItem.Index]);
        //        //SetActiveWindow(lWindowHandles[listViewItem.Index]);
        //        ShowWindow(lWindowHandles[listViewItem.Index], WindowShowStyle.ShowNormal);
        //    }
        //}
    }
}
