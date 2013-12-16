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
using SebWindowsClient.CryptographyUtils;
using SebWindowsClient.ServiceUtils;

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

        [DllImportAttribute("User32.dll")]
        private static extern IntPtr SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern uint GetWindowThreadProcessId(IntPtr hwnd, out int lpdwProcessId);

        //[System.Runtime.InteropServices.DllImport("User32")]
        //private static extern bool SetForegroundWindow(IntPtr hWnd);


        private const int SW_HIDE = 0;
        private const int SW_SHOW = 5;

        private const string VistaStartMenuCaption = "Start";
        private static IntPtr vistaStartMenuWnd = IntPtr.Zero;
        private delegate bool EnumThreadProc(IntPtr hwnd, IntPtr lParam);

        private SebSocketClient sebSocketClient = new SebSocketClient();
        //private SebApplicationChooserForm SebApplicationChooser = null;

        public bool closeSebClient = false;
        public string sebPassword = null;

        private SebCloseDialogForm sebCloseDialogForm;
        private SebApplicationChooserForm sebApplicationChooserForm;

        //private Process xulRunner = new Process();
        //private int xulRunnerExitCode;
        //private DateTime xulRunnerExitTime;
        //private bool xulRunnerExitEventHandled;

        /// ----------------------------------------------------------------------------------------
        /// <summary>
        /// Constructor - initialise components.
        /// </summary>
        /// ----------------------------------------------------------------------------------------
        public SebWindowsClientForm()
        {
            InitializeComponent();
        }

        /// ----------------------------------------------------------------------------------------
        /// <summary>
        /// OnLoad: Get the file name from command line arguments and load it.
        /// </summary>
        /// ----------------------------------------------------------------------------------------
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            string[] args = Environment.GetCommandLineArgs();
            if (args.Length > 1)
            {
                LoadFile(args[1]);
            }
        }
        public void LoadFile(string file)
        {
            //textBox1.Text = File.ReadAllText(file);
        }

        /// ----------------------------------------------------------------------------------------
        /// <summary>
        // Start xulRunner process.
        /// </summary>
        /// ----------------------------------------------------------------------------------------
        private void StartXulRunner()
        {

//            xulRunnerExitEventHandled = false;

            try
            {
                string XULRunnerParameters = SEBXulRunnerSettings.XULRunnerConfigDictionarySerialize(SEBSettings.settingsCurrent);
                // Create JSON object with XULRunner parameters to pass to xulrunner.exe as base64 string
                StringBuilder xulRunnerPathBuilder = new StringBuilder(SEBClientInfo.XulRunnerExePath);
                //StringBuilder xulRunnerArgumentsBuilder = new StringBuilder(" -app ").Append(SEBClientInfo.XulRunnerSebIniPath).
                //Append(" -configpath ").Append(SEBClientInfo.XulRunnerConfigFile);
                StringBuilder xulRunnerArgumentsBuilder = new StringBuilder(" -app ").Append(SEBClientInfo.XulRunnerSebIniPath).Append(" -profile \"").Append(SEBClientInfo.SebClientConfigFileDirectory).Append("Profiles\"").//Append(" -config \"winctrl\" -debug 1 -purgecaches -jsconsole").
                    Append(" -ctrl \"").Append(XULRunnerParameters).Append("\"");
                //StringBuilder xulRunnerArgumentsBuilder = new StringBuilder(" -app ").Append(SEBClientInfo.XulRunnerSebIniPath).Append(" -ctlr 1 ");
                string xulRunnerArguments = xulRunnerArgumentsBuilder.ToString();
                xulRunnerPathBuilder.Append(xulRunnerArguments);
                string xulRunnerPath = xulRunnerPathBuilder.ToString();

                //string path = SEBClientInfo.XulRunnerExePath;
                //path = path + " -app \"C:\\Program Files (x86)\\ETH Zuerich\\SEB Windows 1.9.1\\SebWindowsClient\\xulseb\\seb.ini\" -configpath  \"C:\\Users\\viktor\\AppData\\Local\\ETH_Zuerich\\config.json\"";
                SEBDesktopController.CreateProcess(xulRunnerPath, SEBClientInfo.DesktopName);
                //xulRunner.StartInfo.FileName = "\"C:\\Program Files (x86)\\ETH Zuerich\\SEB Windows 1.9.1\\SebWindowsClient\\xulrunner\\xulrunner.exe\"";
                ////xulRunner.StartInfo.Verb = "XulRunner";
                //xulRunner.StartInfo.Arguments = " -app \"C:\\Program Files (x86)\\ETH Zuerich\\SEB Windows 1.9.1\\SebWindowsClient\\xulseb\\seb.ini\" -configpath  \"C:\\Users\\viktor\\AppData\\Local\\ETH_Zuerich\\config.json\"";
                //xulRunner.StartInfo.CreateNoWindow = true;
                //xulRunner.EnableRaisingEvents = true;
                //xulRunner.Exited += new EventHandler(xulRunner_Exited);
                //xulRunner.Start();

            }
            catch (Exception ex)
            {
                Logger.AddError("An error occurred starting XULRunner", this, ex, ex.Message);
                return;
            }
        }

        /// ----------------------------------------------------------------------------------------
        /// <summary>
        // Handle xulRunner_Exited event and display process information.
        /// </summary>
        /// ----------------------------------------------------------------------------------------
        //private void xulRunner_Exited(object sender, System.EventArgs e)
        //{

        //    xulRunnerExitEventHandled = true;
        //    xulRunnerExitCode = xulRunner.ExitCode;
        //    xulRunnerExitTime = xulRunner.ExitTime;
        //    if (xulRunnerExitCode != 0)
        //    {
        //       Logger.AddError("An error occurred by exitting XulRunner. Exit code: " + xulRunnerExitCode.ToString() , this, null);
        //    }
 
        //}

        /// ----------------------------------------------------------------------------------------
        /// <summary>
        /// Add Permitted process names to ToolStrip control
        /// </summary>
        /// ----------------------------------------------------------------------------------------
        private void addPermittedProcessesToTS()
        {
            List<object> permittedProcessList = (List<object>)SEBClientInfo.getSebSetting(SEBSettings.KeyPermittedProcesses)[SEBSettings.KeyPermittedProcesses];
            if (permittedProcessList.Count > 0)
            {
                for (int i = 0; i < permittedProcessList.Count; i++)
                {
                    ToolStripButton toolStripButton = new ToolStripButton();
                    Dictionary<string, object> permittedProcess = (Dictionary<string, object>)permittedProcessList[i];
                    if ((Boolean)permittedProcess[SEBSettings.KeyActive])
                    {
                        string title      = (string)permittedProcess[SEBSettings.KeyTitle];
                        string executable = (string)permittedProcess[SEBSettings.KeyExecutable];

                        toolStripButton.Name        = executable;
                        toolStripButton.ToolTipText = title;

                        if (executable.Contains("notepad.exe"))
                            toolStripButton.Image = ilProcessIcons.Images[2];
                        else if (executable.Contains("calc.exe"))
                            toolStripButton.Image = ilProcessIcons.Images[1];
                        else if (executable.Contains("xulrunner.exe"))
                            toolStripButton.Image = ilProcessIcons.Images[3];
                        else
                            toolStripButton.Text = title;

                        toolStripButton.Click += new EventHandler(ToolStripButton_Click);

                        tsPermittedProcesses.Items.Add(toolStripButton);

                        // Autostart
                        if ((Boolean)permittedProcess[SEBSettings.KeyAutostart])
                        {
                            if (!executable.Contains(SEBClientInfo.XUL_RUNNER))
                            {
                                StringBuilder startProcessNameBuilder = new StringBuilder(executable);
                                List<object> argumentList = (List<object>)permittedProcess[SEBSettings.KeyArguments];
                                for (int j = 0; j < argumentList.Count; j++)
                                {
                                    Dictionary<string, object> argument = (Dictionary<string, object>)argumentList[j];
                                    if ((Boolean)argument[SEBSettings.KeyActive])
                                    {
                                        startProcessNameBuilder.Append(" ").Append((string)argument[SEBSettings.KeyArgument]);
                                    }
                                }
                                SEBDesktopController.CreateProcess(startProcessNameBuilder.ToString(), SEBClientInfo.DesktopName);
                            }
                        }
                    }
                }
            }
            //System.Diagnostics.Process oskProcess = null;
            // create the process.
            //oskProcess = SEBDesktopController.CreateProcess("C:\Program Files\Common Files\Microsoft Shared\ink\TabTip.exe", SEBClientInfo.DesktopName);
            //SEBDesktopController.CreateProcess("C:\\Program Files\\Common Files\\Microsoft Shared\\ink\\TabTip.exe", SEBClientInfo.DesktopName);
            //oskProcess = Process.Start("C:\\Program Files\\Common Files\\Microsoft Shared\\ink\\TabTip.exe");

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
            List<object> permittedProcessList = (List<object>)SEBClientInfo.getSebSetting(SEBSettings.KeyPermittedProcesses)[SEBSettings.KeyPermittedProcesses];
            if (permittedProcessList.Count > 0)
            {
                for (int i = 0; i < permittedProcessList.Count; i++)
                {
                    Dictionary<string, object> permittedProcess = (Dictionary<string, object>)permittedProcessList[i];
                    string executable = (string)permittedProcess[SEBSettings.KeyExecutable];
                    if((Boolean)permittedProcess[SEBSettings.KeyActive])
                    {
                        if (toolStripButton.Name.CompareTo(executable) == 0)
                        {
                            if (executable.Contains(SEBClientInfo.XUL_RUNNER))
                            {
                                bool xulRunnerRunning = false;
                                Process[] runningApplications = SEBDesktopController.GetInputProcessesWithGI();
                                for (int j = 0; j < runningApplications.Count(); j++)
                                {
                                    if (executable.Contains(runningApplications[j].ProcessName))
                                    {
                                        xulRunnerRunning = true;
                                    }
                                }
                                if (!xulRunnerRunning)
                                    StartXulRunner();
                            }
                            else
                            {
                                StringBuilder startProcessNameBuilder = new StringBuilder(executable);
                                List<object> argumentList = (List<object>)permittedProcess[SEBSettings.KeyArguments];
                                for (int j = 0; j < argumentList.Count; j++)
                                {
                                    Dictionary<string, object> argument = (Dictionary<string, object>)argumentList[j];
                                    if ((Boolean)argument[SEBSettings.KeyActive])
                                    {
                                        startProcessNameBuilder.Append(" ").Append((string)argument[SEBSettings.KeyArgument]);
                                    }
                                }
                                SEBDesktopController.CreateProcess(startProcessNameBuilder.ToString(), SEBClientInfo.DesktopName);
                            }
                        }
                    }
                }
            }
            //if (SebKeyCapture.SebApplicationChooser == null)
            //    SebKeyCapture.SebApplicationChooser = new SebApplicationChooserForm();
            //SebKeyCapture.SebApplicationChooser.fillListApplications();
            //SebKeyCapture.SebApplicationChooser.Visible = true;
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

            //int height = Screen.PrimaryScreen.Bounds.Height;
            int width = Screen.PrimaryScreen.Bounds.Width;
            int x = 0; //Screen.PrimaryScreen.WorkingArea.Width - this.Width;
            int y = Screen.PrimaryScreen.Bounds.Height - this.Height;

            //this.Height = height;
            this.Width = width;
            this.Location = new Point(x, y);
            this.TopMost = true;
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
            if ((bool)SEBSettings.settingsCurrent[SEBSettings.KeyAllowQuit] == true)
            {
                string hashedQuitPassword = (string)SEBSettings.settingsCurrent[SEBSettings.KeyHashedQuitPassword];
                if (String.IsNullOrEmpty(hashedQuitPassword) == true)
                {

                }
                else
                {
                    ShowCloseDialogForm();
                }
            }
            //if (this.closeSebClient)
            //{
            //    this.Close();
            //}
        }

        /// ----------------------------------------------------------------------------------------
        /// <summary>
        /// Initialise Client Socket.  Send User, UserSid, Registry Flags string to SebWindowsService
        /// </summary>
        /// <returns>true if succeed</returns>
        /// ----------------------------------------------------------------------------------------
        private bool InitClientSocket()
        {
            SEBLocalHostInfo sebLocalHostInfo = new SEBLocalHostInfo();
            IPHostEntry              hostInfo = sebLocalHostInfo.GetHostInfo();
            SecurityIdentifier       userSid  = sebLocalHostInfo.GetSID();
            string                   userName = sebLocalHostInfo.GetUserName();

            // Label with Username
            //lbl_User.Text = "Logged in as: " + userName;

            Logger.AddInformation("HostName: " + hostInfo.HostName + "HostAddress: " + SEBClientInfo.HostIpAddress + "PortNumber: " + SEBClientInfo.PortNumber.ToString(), this, null);

            bool serviceAvailable = SEBWindowsServiceController.ServiceAvailable(SEBClientInfo.SEB_WINDOWS_SERVICE_NAME);
            if (serviceAvailable)
            {
                // Open socket
                bool bSocketConnected = sebSocketClient.OpenSocket(SEBClientInfo.HostIpAddress, SEBClientInfo.PortNumber.ToString());

                if (bSocketConnected)
                {
                    // Set receive timeout
                    if (((Int32)SEBClientInfo.getSebSetting(SEBSettings.KeySebServicePolicy)[SEBSettings.KeySebServicePolicy]) == (Int32)sebServicePolicies.forceSebService)
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
                    StringBuilder registryFlagsBuilder = new StringBuilder();
                    registryFlagsBuilder.Append((Boolean)SEBClientInfo.getSebSetting(SEBSettings.KeyInsideSebEnableSwitchUser       )[SEBSettings.KeyInsideSebEnableSwitchUser       ] ? 1 : 0);
                    registryFlagsBuilder.Append((Boolean)SEBClientInfo.getSebSetting(SEBSettings.KeyInsideSebEnableLockThisComputer )[SEBSettings.KeyInsideSebEnableLockThisComputer ] ? 1 : 0);
                    registryFlagsBuilder.Append((Boolean)SEBClientInfo.getSebSetting(SEBSettings.KeyInsideSebEnableChangeAPassword  )[SEBSettings.KeyInsideSebEnableChangeAPassword  ] ? 1 : 0);
                    registryFlagsBuilder.Append((Boolean)SEBClientInfo.getSebSetting(SEBSettings.KeyInsideSebEnableStartTaskManager )[SEBSettings.KeyInsideSebEnableStartTaskManager ] ? 1 : 0);
                    registryFlagsBuilder.Append((Boolean)SEBClientInfo.getSebSetting(SEBSettings.KeyInsideSebEnableLogOff           )[SEBSettings.KeyInsideSebEnableLogOff           ] ? 1 : 0);
                    registryFlagsBuilder.Append((Boolean)SEBClientInfo.getSebSetting(SEBSettings.KeyInsideSebEnableShutDown         )[SEBSettings.KeyInsideSebEnableShutDown         ] ? 1 : 0);
                    registryFlagsBuilder.Append((Boolean)SEBClientInfo.getSebSetting(SEBSettings.KeyInsideSebEnableEaseOfAccess     )[SEBSettings.KeyInsideSebEnableEaseOfAccess     ] ? 1 : 0);
                    registryFlagsBuilder.Append((Boolean)SEBClientInfo.getSebSetting(SEBSettings.KeyInsideSebEnableVmWareClientShade)[SEBSettings.KeyInsideSebEnableVmWareClientShade] ? 1 : 0);

                    registryFlagsBuilder.Append((Boolean)SEBClientInfo.getSebSetting(SEBSettings.KeyOutsideSebEnableSwitchUser       )[SEBSettings.KeyOutsideSebEnableSwitchUser       ] ? 1 : 0);
                    registryFlagsBuilder.Append((Boolean)SEBClientInfo.getSebSetting(SEBSettings.KeyOutsideSebEnableLockThisComputer )[SEBSettings.KeyOutsideSebEnableLockThisComputer ] ? 1 : 0);
                    registryFlagsBuilder.Append((Boolean)SEBClientInfo.getSebSetting(SEBSettings.KeyOutsideSebEnableChangeAPassword  )[SEBSettings.KeyOutsideSebEnableChangeAPassword  ] ? 1 : 0);
                    registryFlagsBuilder.Append((Boolean)SEBClientInfo.getSebSetting(SEBSettings.KeyOutsideSebEnableStartTaskManager )[SEBSettings.KeyOutsideSebEnableStartTaskManager ] ? 1 : 0);
                    registryFlagsBuilder.Append((Boolean)SEBClientInfo.getSebSetting(SEBSettings.KeyOutsideSebEnableLogOff           )[SEBSettings.KeyOutsideSebEnableLogOff           ] ? 1 : 0);
                    registryFlagsBuilder.Append((Boolean)SEBClientInfo.getSebSetting(SEBSettings.KeyOutsideSebEnableShutDown         )[SEBSettings.KeyOutsideSebEnableShutDown         ] ? 1 : 0);
                    registryFlagsBuilder.Append((Boolean)SEBClientInfo.getSebSetting(SEBSettings.KeyOutsideSebEnableEaseOfAccess     )[SEBSettings.KeyOutsideSebEnableEaseOfAccess     ] ? 1 : 0);
                    registryFlagsBuilder.Append((Boolean)SEBClientInfo.getSebSetting(SEBSettings.KeyOutsideSebEnableVmWareClientShade)[SEBSettings.KeyOutsideSebEnableVmWareClientShade] ? 1 : 0);
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
                SEBEditRegistry sebEditRegistry = new SEBEditRegistry();
                if (!sebEditRegistry.EditRegistry())
                {
                    sebEditRegistry.addResetRegValues("EditRegistry", 0);
                }

                try
                {
                    // Kill processes
                    List<object> prohibitedProcessList = (List<object>)SEBClientInfo.getSebSetting(SEBSettings.KeyProhibitedProcesses)[SEBSettings.KeyProhibitedProcesses];
                if (prohibitedProcessList.Count() > 0)
                {
                    for (int i = 0; i < prohibitedProcessList.Count(); i++)
                    {
                        Dictionary<string, object> prohibitedProcess = (Dictionary<string, object>)prohibitedProcessList[i];
                        string prohibitedProcessName = (string)prohibitedProcess[SEBSettings.KeyExecutable];
                        if ((Boolean)prohibitedProcess[SEBSettings.KeyActive])
                        {
                            Logger.AddInformation("Kill process by name: " + prohibitedProcessName, this, null);
                            // Close process
                            //SEBNotAllowedProcessController.CloseProcessByName(prohibitedProcessName);

                            //if (SEBNotAllowedProcessController.CheckIfAProcessIsRunning(prohibitedProcessName))
                            //{
                            //if (SEBErrorMessages.OutputErrorMessage(SEBGlobalConstants.IND_CLOSE_PROCESS_FAILED, SEBGlobalConstants.IND_MESSAGE_KIND_QUESTION, prohibitedProcessName))
                            //{
                            SEBNotAllowedProcessController.KillProcessByName(prohibitedProcessName);
                            //}

                            //}
                        }
                    }

                }
                return true;
            }
            catch (Exception ex)
            {
                Logger.AddError("Error when killing prohibited processes!", null, ex);
                return false;
            }
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

            addPermittedProcessesToTS();
            SetFormOnDesktop();
            StartXulRunner();
            //System.Diagnostics.Process oskProcess = null;
            //oskProcess = Process.Start("OSK");
            //SEBDesktopController d = SEBDesktopController.OpenDesktop(SEBClientInfo.DesktopName);
            //oskProcess = d.CreateProcess("OSK");
            if (sebCloseDialogForm == null)
            {
                sebCloseDialogForm = new SebCloseDialogForm();
//              SetForegroundWindow(sebCloseDialogForm.Handle);
                sebCloseDialogForm.TopMost = true;
                sebCloseDialogForm.Show();
                sebCloseDialogForm.Visible = false;
            }
            if (sebApplicationChooserForm == null)
            {
                sebApplicationChooserForm = new SebApplicationChooserForm();
                sebApplicationChooserForm.TopMost = true;
                sebApplicationChooserForm.Show();
                sebApplicationChooserForm.Visible = false;
            } 
            //if (sebPasswordDialogForm == null)
            //{
            //    sebPasswordDialogForm = new SebPasswordDialogForm();
            //    sebPasswordDialogForm.TopMost = true;
            //    sebPasswordDialogForm.Show();
            //    sebPasswordDialogForm.Visible = false;
            //}
        }

        /// ----------------------------------------------------------------------------------------
        /// <summary>
        /// Show SEB Application Chooser Form.
        /// </summary>
        /// ----------------------------------------------------------------------------------------
        public void ShowApplicationChooserForm()
        {
            // Show testDialog as a modal dialog and determine if DialogResult = OK.
            SetForegroundWindow(this.Handle);
            //this.Activate();
            sebApplicationChooserForm.fillListApplications(); 
            sebApplicationChooserForm.Visible = true;
            sebCloseDialogForm.Activate();
        }

        /// ----------------------------------------------------------------------------------------
        /// <summary>
        /// Show SEB Application Chooser Form.
        /// </summary>
        /// ----------------------------------------------------------------------------------------
        public void SelectNextListItem()
        {
            // Show testDialog as a modal dialog and determine if DialogResult = OK.
            sebApplicationChooserForm.SelectNextListItem();
            //sebApplicationChooserForm.Visible = true;
            sebCloseDialogForm.Activate();
        }

        /// ----------------------------------------------------------------------------------------
        /// <summary>
        /// Hide SEB Application Chooser Form.
        /// </summary>
        /// ----------------------------------------------------------------------------------------
        public void HideApplicationChooserForm()
        {
            // Show testDialog as a modal dialog and determine if DialogResult = OK.
            sebApplicationChooserForm.Visible = false;
            //sebCloseDialogForm.Activate();
        }

        /// ----------------------------------------------------------------------------------------
        /// <summary>
        /// Show SEB Close Form.
        /// </summary>
        /// ----------------------------------------------------------------------------------------
        public void ShowCloseDialogForm()
        {
            // Show testDialog as a modal dialog and determine if DialogResult = OK.
            sebCloseDialogForm.Visible = true;
            sebCloseDialogForm.Activate();
            sebCloseDialogForm.txtQuitPassword.Focus();
        }

        /// ----------------------------------------------------------------------------------------
        /// <summary>
        /// Close form, if Quit Password is correct.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// ----------------------------------------------------------------------------------------
        private void SebWindowsClientForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            //bool bQuit = false;
            //bQuit = CheckQuitPassword();

            //if (bQuit)
            //{
                bool bSocketResult;
                SEBLocalHostInfo  sebLocalHostInfo = new SEBLocalHostInfo();
                string userName = sebLocalHostInfo.GetUserName();

                // ShutDown message to SebWindowsService
                bool serviceAvailable = SEBWindowsServiceController.ServiceAvailable(SEBClientInfo.SEB_WINDOWS_SERVICE_NAME);
                if (serviceAvailable)
                {
                    // Send ShutDown to server
                    bSocketResult = sebSocketClient.SendEquationToSocketServer("ShutDown", userName, SEBClientInfo.SendInterval);
                    string[] resultShutDown = sebSocketClient.RecvEquationOfSocketServer();
                    this.sebSocketClient.CloseSocket();
                }

                // ShutDown Processes
                Process[] runningApplications = SEBDesktopController.GetInputProcessesWithGI();
                List<object> permittedProcessList = (List<object>)SEBClientInfo.getSebSetting(SEBSettings.KeyPermittedProcesses)[SEBSettings.KeyPermittedProcesses];
                for (int i = 0; i < permittedProcessList.Count(); i++)
                {
                    for (int j = 0; j < runningApplications.Count(); j++)
                    {
                        Dictionary<string, object> permittedProcess = (Dictionary<string, object>)permittedProcessList[i];
                        string permittedProcessName = (string)permittedProcess[SEBSettings.KeyExecutable];
                        if ((Boolean)permittedProcess[SEBSettings.KeyActive])
                        {
                            if (permittedProcessName.Contains(runningApplications[j].ProcessName))
                            {
                                // Close process
                                //SEBNotAllowedProcessController.CloseProcessByName(runningApplications[j].ProcessName);

                                //if (SEBNotAllowedProcessController.CheckIfAProcessIsRunning(runningApplications[j].ProcessName))
                                //{
                                    //if (SEBErrorMessages.OutputErrorMessage(SEBGlobalConstants.IND_CLOSE_PROCESS_FAILED, SEBGlobalConstants.IND_MESSAGE_KIND_QUESTION, runningApplications[j].ProcessName))
                                    //{
                                        SEBNotAllowedProcessController.KillProcessByName(runningApplications[j].ProcessName);
                                    //}

                                //}
                            }
                        }
                    }
                }


                // Restart the explorer.exe shell
                if ((Boolean)SEBClientInfo.getSebSetting(SEBSettings.KeyKillExplorerShell)[SEBSettings.KeyKillExplorerShell])
                {
                    if (SEBClientInfo.ExplorerShellWasKilled)
                    {
                        Logger.AddInformation("Restarting the shell.", null, null);
                        string explorer = string.Format("{0}\\{1}", Environment.GetEnvironmentVariable("WINDIR"), "explorer.exe");
                        Process process = new Process();           
                        process.StartInfo.FileName        = explorer;
                        process.StartInfo.UseShellExecute = true;
                        process.Start();
                    }
                }

                // Switch to Default Desktop
                if ((Boolean)SEBClientInfo.getSebSetting(SEBSettings.KeyCreateNewDesktop)[SEBSettings.KeyCreateNewDesktop])
                {
                    SEBDesktopController.Show      (SEBClientInfo.OriginalDesktop.DesktopName);
                    SEBDesktopController.SetCurrent(SEBClientInfo.OriginalDesktop);
                    SEBClientInfo       .SEBNewlDesktop.Close();
                }
                else
                {
                    SetVisibility(true);
                }

                // Clean clipboard
                SEBClipboard.CleanClipboard();
                Logger.AddInformation("Clipboard deleted.", null, null);
                SebKeyCapture.FilterKeys = false;
                Logger.closeLoger();
            //}
            //else
            //{
            //    e.Cancel = true;
            //}

            //sebCloseDialogForm.Dispose();
        }
     }
}
