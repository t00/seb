using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SebWindowsClient.ConfigurationUtils;
using SebWindowsClient.DiagnosticsUtils;
using SebWindowsClient.DesktopUtils;
using SebWindowsClient.ClientSocketUtils;
using System.Net;
using System.Security.Principal;
using SebWindowsClient.RegistryUtils;
using SebWindowsClient.ProcessUtils;
using SebWindowsClient.BlockShortcutsUtils;
using System.Runtime.InteropServices;
using System.Diagnostics;

// -------------------------------------------------------------
//     Viktor tomas
//     BFH-TI, http://www.ti.bfh.ch
//     Biel, 2012
// -------------------------------------------------------------
namespace SebWindowsClient
{
    public partial class SebWindowsClientForm : Form
    {
        [DllImport("user32.dll")]
        private static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        [DllImport("user32.dll")]
        static extern IntPtr GetDesktopWindow();

         [DllImport("user32.dll")]
        private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X,
           int Y, int cx, int cy, uint uFlags);

        [DllImport("user32.dll")]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern bool EnumThreadWindows(int threadId, EnumThreadProc pfnEnum, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern System.IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr FindWindowEx(IntPtr parentHandle, IntPtr childAfter, string className, string windowTitle);

        [DllImport("user32.dll")]
        private static extern IntPtr FindWindowEx(IntPtr parentHwnd, IntPtr childAfterHwnd, IntPtr className, string windowText);

        [DllImport("user32.dll")]
        private static extern int ShowWindow(IntPtr hwnd, int nCmdShow);

        [DllImport("user32.dll")]
        private static extern uint GetWindowThreadProcessId(IntPtr hwnd, out int lpdwProcessId);

        private const int SW_HIDE = 0;
        private const int SW_SHOW = 5;

        private const string VistaStartMenuCaption = "Start";
        private static IntPtr vistaStartMenuWnd = IntPtr.Zero;
        private delegate bool EnumThreadProc(IntPtr hwnd, IntPtr lParam);

        /// ----------------------------------------------------------------------------------------
        /// <summary>
        /// Constructor - initialise components.
        /// </summary>
        /// ----------------------------------------------------------------------------------------
        public SebWindowsClientForm()
        {
            InitializeComponent();
            addPermittedProcessesToTS();
            SetFormOnDesktop();
        }

        /// ----------------------------------------------------------------------------------------
        /// <summary>
        /// Add Permited process names to ToolStrip conrol .
        /// </summary>
        /// ----------------------------------------------------------------------------------------
        private void addPermittedProcessesToTS()
        {
            if (SEBClientInfo.sebClientConfig.PermittedProcesses.Count() > 0)
            {
                for (int i = 0; i < SEBClientInfo.sebClientConfig.PermittedProcesses.Count(); i++)
                {
                    ToolStripButton toolStripButton = new ToolStripButton();
                    string tsbName = SEBClientInfo.sebClientConfig.PermittedProcesses[i].NameWin;
                    toolStripButton.Name = tsbName;
                    if (tsbName.CompareTo("notepad.exe") == 0)
                        toolStripButton.Image = Bitmap.FromFile(@"C:\Users\viktor\seb\trunk\win\SebWindowsPackage\SebWindowsClient\SebWindowsClient\icons\microsoft Notepad 2007.png");
                    else if (tsbName.CompareTo("calc.exe") == 0)
                        toolStripButton.Image = Bitmap.FromFile(@"C:\Users\viktor\seb\trunk\win\SebWindowsPackage\SebWindowsClient\SebWindowsClient\icons\calc_exe_01_10.png");
                    else
                        toolStripButton.Text = tsbName;

                    toolStripButton.Click += new EventHandler(ToolStripButton_Click);

                    tsPermittedProcesses.Items.Add(toolStripButton);
                }
            }
        }

        /// ----------------------------------------------------------------------------------------
        /// <summary>
        /// Execute selected permited Process.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// ----------------------------------------------------------------------------------------
        protected void ToolStripButton_Click(object sender, EventArgs e)
        {
            // identify which button was clicked and perform necessary actions
            ToolStripButton toolStripButton = sender as ToolStripButton;
            if (SEBClientInfo.sebClientConfig.PermittedProcesses.Count() > 0)
            {
                for (int i = 0; i < SEBClientInfo.sebClientConfig.PermittedProcesses.Count(); i++)
                {
                    if (toolStripButton.Name.CompareTo(SEBClientInfo.sebClientConfig.PermittedProcesses[i].NameWin) == 0)
                    {
                        StringBuilder startProcessNameBuilder = new StringBuilder(SEBClientInfo.sebClientConfig.PermittedProcesses[i].NameWin).Append(" ").
                            Append(SEBClientInfo.sebClientConfig.PermittedProcesses[i].Arguments);
                        SEBDesktopController.CreateProcess(startProcessNameBuilder.ToString(), SEBClientInfo.DesktopName);
                    }
                }
            }
            if (SebKeyCapture.SebApplicationChooser == null)
                SebKeyCapture.SebApplicationChooser = new SebApplicationChooserForm();
            SebKeyCapture.SebApplicationChooser.fillListApplications();
        }


        /// ----------------------------------------------------------------------------------------
        /// <summary>
        /// Set form on Desktop.
        /// </summary>
        /// ----------------------------------------------------------------------------------------
        private bool SetFormOnDesktop()
        {

            this.FormBorderStyle = FormBorderStyle.None;

            // sezt das formular auf die Taskbar
            SetParent(this.Handle, GetDesktopWindow());
            //this.BackColor = Color.Red;

            return true;
        }

        /// <summary>
        /// Hide or show the Windows taskbar and startmenu.
        /// </summary>
        /// <param name="show">true to show, false to hide</param>
        public static void SetVisibility(bool show)
        {
            // get taskbar window
            IntPtr taskBarWnd = FindWindow("Shell_TrayWnd", null);

            // try it the WinXP way first...
            IntPtr startWnd = FindWindowEx(taskBarWnd, IntPtr.Zero, "Button", "Start");

            if (startWnd == IntPtr.Zero)
            {
                // try an alternate way, as mentioned on CodeProject by Earl Waylon Flinn
                startWnd = FindWindowEx(IntPtr.Zero, IntPtr.Zero, (IntPtr)0xC017, "Start");
            }

            if (startWnd == IntPtr.Zero)
            {
                // ok, let's try the Vista easy way...
                startWnd = FindWindow("Button", null);

                if (startWnd == IntPtr.Zero)
                {
                    // no chance, we need to to it the hard way...
                    startWnd = GetVistaStartMenuWnd(taskBarWnd);
                }
            }

            ShowWindow(taskBarWnd, show ? SW_SHOW : SW_HIDE);
            ShowWindow(startWnd, show ? SW_SHOW : SW_HIDE);
        }

        /// <summary>
        /// Returns the window handle of the Vista start menu orb.
        /// </summary>
        /// <param name="taskBarWnd">windo handle of taskbar</param>
        /// <returns>window handle of start menu</returns>
        private static IntPtr GetVistaStartMenuWnd(IntPtr taskBarWnd)
        {
            // get process that owns the taskbar window
            int procId;
            GetWindowThreadProcessId(taskBarWnd, out procId);

            Process p = Process.GetProcessById(procId);
            if (p != null)
            {
                // enumerate all threads of that process...
                foreach (ProcessThread t in p.Threads)
                {
                    EnumThreadWindows(t.Id, MyEnumThreadWindowsProc, IntPtr.Zero);
                }
            }
            return vistaStartMenuWnd;
        }

        /// <summary>
        /// Callback method that is called from 'EnumThreadWindows' in 'GetVistaStartMenuWnd'.
        /// </summary>
        /// <param name="hWnd">window handle</param>
        /// <param name="lParam">parameter</param>
        /// <returns>true to continue enumeration, false to stop it</returns>
        private static bool MyEnumThreadWindowsProc(IntPtr hWnd, IntPtr lParam)
        {
            StringBuilder buffer = new StringBuilder(256);
            if (GetWindowText(hWnd, buffer, buffer.Capacity) > 0)
            {
                Console.WriteLine(buffer);
                if (buffer.ToString() == VistaStartMenuCaption)
                {
                    vistaStartMenuWnd = hWnd;
                    return false;
                }
            }
            return true;
        }

        /// ----------------------------------------------------------------------------------------
        /// <summary>
        /// Switch to current desktop and close app.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// ----------------------------------------------------------------------------------------
        private void btn_Exit_Click(object sender, EventArgs e)
        {
            if (SEBClientInfo.sebClientConfig.getSecurityOption("createNewDesktop").getBool())
            {
                SEBDesktopController.Show(SEBClientInfo.OriginalDesktop.DesktopName);
                SEBDesktopController.SetCurrent(SEBClientInfo.OriginalDesktop);
                SEBClientInfo.SEBNewlDesktop.Close();
            }
            else
            {
                SetVisibility(true);
            }

            Logger.closeLoger();
            this.Close();
        }

        /// ----------------------------------------------------------------------------------------
        /// <summary>
        /// Initialise Client Socket.
        /// </summary>
        /// <returns>true if succeed</returns>
        /// ----------------------------------------------------------------------------------------
        private bool InitClientSocket()
        {
            SEBLocalHostInfo sEBLocalHostInfo = new SEBLocalHostInfo();
            IPHostEntry hostInfo = sEBLocalHostInfo.GetHostInfo();
            SecurityIdentifier userSid = sEBLocalHostInfo.GetSID();
            string userName = sEBLocalHostInfo.GetUserName();

            lbl_User.Text = "Logged in as " + userName;

            Logger.AddInformation("HostName: " + hostInfo.HostName + " PortNumber: " + SEBClientInfo.PortNumber.ToString(), this, null);

            // Open socket
            SebSocketClient sebSocketClient = new SebSocketClient();
            bool bSocketConnected = sebSocketClient.OpenSocket(hostInfo.HostName, SEBClientInfo.PortNumber.ToString());

            if (bSocketConnected)
            {
                // Set receive timeout
                if (int.Parse(SEBClientInfo.sebClientConfig.getPolicySetting("sebServicePolicy").Value) == (int)sebServicePolicies.forceSebService)
                {
                    SEBClientInfo.RecvTimeout = 0;   // timeout "0" means "infinite" in this case !!!
                    Logger.AddInformation("Force Windows Service demanded, therefore socket recvTimeout = infinite", this, null);

                }
                else
                {
                    Logger.AddInformation("Force Windows Service not demanded, therefore socket recvTimeout = " + SEBClientInfo.RecvTimeout, this, null);
                }

                bool bSetRecvTimeout = sebSocketClient.SetRecvTimeout(SEBClientInfo.RecvTimeout);

                //Set registry flags
                StringBuilder registryFlagsBuilder = new StringBuilder(
                 SEBClientInfo.sebClientConfig.getPolicySetting("insideSebEnableSwitchUser").Value).Append(",")
                .Append(SEBClientInfo.sebClientConfig.getPolicySetting("insideSebEnableLockThisComputer").Value).Append(",")
                .Append(SEBClientInfo.sebClientConfig.getPolicySetting("insideSebEnableChangePassword").Value).Append(",")
                .Append(SEBClientInfo.sebClientConfig.getPolicySetting("insideSebEnableStartTaskManager").Value).Append(",")
                .Append(SEBClientInfo.sebClientConfig.getPolicySetting("insideSebEnableLogOff").Value).Append(",")
                .Append(SEBClientInfo.sebClientConfig.getPolicySetting("insideSebEnableShutDown").Value).Append(",")
                .Append(SEBClientInfo.sebClientConfig.getPolicySetting("insideSebEnableEaseOfAccess").Value).Append(",")
                .Append(SEBClientInfo.sebClientConfig.getPolicySetting("insideSebEnableVmWareClientShade").Value);
                string registryFlags = registryFlagsBuilder.ToString();

                Logger.AddInformation("UserName: " + userName + " UserSid: " + userSid.ToString() + " RegistryFlags: " + registryFlags, this, null);

                bool bSocketResult;
                // Send UserName to server
                bSocketResult = sebSocketClient.SendEquationToSocketServer("UserName", userName, SEBClientInfo.SendInterval);
                string[] resultUserName = sebSocketClient.RecvEquationOfSocketServer();

                // Send UserSid to server
                bSocketResult = sebSocketClient.SendEquationToSocketServer("UserSid", userSid.ToString(), SEBClientInfo.SendInterval);
                string[] resultUserSid = sebSocketClient.RecvEquationOfSocketServer();

                // Send RegistryFlags to server
                bSocketResult = sebSocketClient.SendEquationToSocketServer("RegistryFlags", registryFlags, SEBClientInfo.SendInterval);
                string[] resultRegistryFlags = sebSocketClient.RecvEquationOfSocketServer();

            }
            return true;
        }

        /// ----------------------------------------------------------------------------------------
        /// <summary>
        /// Set registry values and close prohibited processes.
        /// </summary>
        /// <returns>true if succeed</returns>
        /// ----------------------------------------------------------------------------------------
        private bool InitClientRegistryAndProcesses()
        {
            // Edit Registry
            SEBEditRegistry sEBEditRegistry  = new SEBEditRegistry();
            if (!sEBEditRegistry.EditRegistry())
            {
                sEBEditRegistry.addResetRegValues("EditRegistry", 0);
            }

            // Kill processes
            if (SEBClientInfo.sebClientConfig.ProhibitedProcesses.Count() > 0)
            {
                for (int i = 0; i < SEBClientInfo.sebClientConfig.ProhibitedProcesses.Count(); i++)
                {
                    Logger.AddInformation("Kill process by name: " + SEBClientInfo.sebClientConfig.ProhibitedProcesses[i].nameWin, this, null);
                    // Close process
                    SEBNotAllowedProcessController.CloseProcessByName(SEBClientInfo.sebClientConfig.ProhibitedProcesses[i].nameWin);

                    if (SEBNotAllowedProcessController.CheckIfAProcessIsRunning(SEBClientInfo.sebClientConfig.ProhibitedProcesses[i].nameWin))
                    {
                        if (SEBErrorMessages.OutputErrorMessage(SEBGlobalConstants.IND_CLOSE_PROCESS_FAILED, SEBGlobalConstants.IND_MESSAGE_KIND_QUESTION, SEBClientInfo.sebClientConfig.ProhibitedProcesses[i].nameWin))
                        {
                            SEBNotAllowedProcessController.KillProcessByName(SEBClientInfo.sebClientConfig.ProhibitedProcesses[i].nameWin);
                        }
 
                    }
                }

            }
            return true;
        }

        /// ----------------------------------------------------------------------------------------
        /// <summary>
        /// Load form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// ----------------------------------------------------------------------------------------
        private void SebWindowsClientForm_Load(object sender, EventArgs e)
        {
            bool bClientInfo = InitClientSocket();
            bool bClientRegistryAndProcesses = InitClientRegistryAndProcesses();

            // Disable unwanted keys.
            SebKeyCapture.FilterKeys = true;
        }

     }
}
