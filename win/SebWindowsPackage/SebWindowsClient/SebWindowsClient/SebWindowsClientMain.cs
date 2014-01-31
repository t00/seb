// -------------------------------------------------------------
//     Viktor tomas
//     BFH-TI, http://www.ti.bfh.ch
//     Biel, 2012
// -------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using SebWindowsClient.ConfigurationUtils;
using SebWindowsClient.DiagnosticsUtils;
using SebWindowsClient.ProcessUtils;
using SebWindowsClient.DesktopUtils;
using System.Diagnostics;
using SebWindowsClient.ClientSocketUtils;
using System.Net;
using System.Security.Principal;
using System.Xml.Serialization;
using System.IO;
using SebWindowsClient.CryptographyUtils;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;
using System.Text;
//using COM.Tools.VMDetect;
using SebWindowsClient.ServiceUtils;
using System.Runtime.InteropServices;
using System.Threading;

using Microsoft.VisualBasic.ApplicationServices;

namespace SebWindowsClient
{
    public class SingleInstanceController : WindowsFormsApplicationBase
    {
        public SingleInstanceController()
        {
            IsSingleInstance = true;

            StartupNextInstance += this_StartupNextInstance;
        }

        void this_StartupNextInstance(object sender, StartupNextInstanceEventArgs e)
        {
            SebWindowsClientForm form = MainForm as SebWindowsClientForm; //My derived form type
            form.LoadFile(e.CommandLine[1]);
        }

        protected override void OnCreateMainForm()
        {
            //SEBClientInfo.SebWindowsClientForm = new SebWindowsClientForm();
            MainForm = SEBClientInfo.SebWindowsClientForm;

        }

        public void SetMainForm(Form newMainForm)
        {
            MainForm = newMainForm;
        }
    }

    static class SebWindowsClientMain
    {
        public static SingleInstanceController singleInstanceController;

        // For killing the Explorer Shell at SEB startup

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool PostMessage(IntPtr hWnd, [MarshalAs(UnmanagedType.U4)] uint Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

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
        private static extern IntPtr FindWindowEx(IntPtr parentHandle, IntPtr childAfter, string className, string windowTitle);

        [DllImport("user32.dll")]
        private static extern IntPtr FindWindowEx(IntPtr parentHwnd, IntPtr childAfterHwnd, IntPtr className, string windowText);

        [DllImport("User32.dll")]
        private static extern bool IsIconic(IntPtr handle);

        [DllImport("user32.dll")]
        private static extern int ShowWindow(IntPtr hwnd, int nCmdShow);

        [DllImportAttribute("User32.dll")]
        public static extern IntPtr SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern uint GetWindowThreadProcessId(IntPtr hwnd, out int lpdwProcessId);

        const int WM_USER = 0x0400; //http://msdn.microsoft.com/en-us/library/windows/desktop/ms644931(v=vs.85).aspx

        private const int SW_HIDE = 0;
        private const int SW_SHOW = 5;
        private const int SW_RESTORE = 9;

        private const string VistaStartMenuCaption = "Start";
        private static IntPtr vistaStartMenuWnd = IntPtr.Zero;
        private delegate bool EnumThreadProc(IntPtr hwnd, IntPtr lParam);

        public static bool sessionCreateNewDesktop;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        //[STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if (InitSebSettings())
            {
                SEBClientInfo.SebWindowsClientForm = new SebWindowsClientForm();
                string[] arguments = Environment.GetCommandLineArgs();
                singleInstanceController = new SingleInstanceController();
                singleInstanceController.Run(arguments);
            }
        }


        /// <summary>
        /// Detect if running in various virtual machines.
        /// C# code only solution which is more compatible.
        /// </summary>
        private static bool IsInsideVM()
        {
            using (var searcher = new System.Management.ManagementObjectSearcher("Select * from Win32_ComputerSystem"))
            {
                using (var items = searcher.Get())
                {
                    foreach (var item in items)
                    {
                        string manufacturer = item["Manufacturer"].ToString().ToLower();
                        if (manufacturer == "microsoft corporation"
                            || manufacturer.Contains("vmware")
                            || manufacturer.Contains("parallels software") 
                            || manufacturer.Contains("xen")
                            || item["Model"].ToString().ToLower().Contains("xen")
                            || item["Model"].ToString() == "VirtualBox")
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        
        /// <summary>
        /// Detect if SEB Running inside VPC.
        /// </summary>
        /*private static bool IsInsideVPC()
        {
            if (VMDetect.IsInsideVPC)
            {
                //MessageBox.Show("Running inside Virtual PC!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Logger.AddInformation("Running inside Virtual PC!", null, null);
                return true;
            }
            else
            {
                //MessageBox.Show("Not running inside Virtual PC!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Logger.AddInformation("Not running inside Virtual PC!", null, null);
                return false;
            }
        }

        /// <summary>
        /// Detect if SEB Running inside VMWare.
        /// </summary>
        private static bool IsInsideVMWare()
        {
            if (VMDetect.IsInsideVMWare)
            {
                //MessageBox.Show("Running inside VMWare!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Logger.AddInformation("Running inside VMWare!", null, null);
                return true;
            }
            else
            {
                //MessageBox.Show("Not running inside VMWare!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Logger.AddInformation("Not running inside VMWare!", null, null);
                return false;
            }
        }*/


        /// ----------------------------------------------------------------------------------------
        /// <summary>
        /// Create and initialise SEB client settings and check system compatibility.
        /// This method needs to be executed only once when SEB first starts 
        /// (not when reconfiguring).
        /// </summary>
        /// <returns>true if succeed</returns>
        /// ----------------------------------------------------------------------------------------
        public static bool InitSebSettings()
        {
            //SebWindowsClientForm.SetVisibility(true);
            //SEBErrorMessages.OutputErrorMessageNew("Test", "Test, ob das Öffnen einer Message-Box createNewDesktop verunmöglicht.", SEBGlobalConstants.IND_MESSAGE_KIND_ERROR, MessageBoxButtons.OK);

            // Set SebClient configuration
            if (!SEBClientInfo.SetSebClientConfiguration())
            {
                SEBErrorMessages.OutputErrorMessage(SEBGlobalConstants.IND_SEB_CLIENT_SEB_ERROR, SEBGlobalConstants.IND_MESSAGE_KIND_ERROR);
                Logger.AddError("Error when opening the file SebClientSettings.seb!", null, null);
                return false;
            }

            // Check system version
            if (!SEBClientInfo.SetSystemVersionInfo())
            {
                SEBErrorMessages.OutputErrorMessage(SEBGlobalConstants.IND_NO_OS_SUPPORT, SEBGlobalConstants.IND_MESSAGE_KIND_ERROR);
                Logger.AddError("Unknown OS. Exiting SEB.", null, null);
                return false;
            }

            //on NT4/NT5 ++ a new desktop is created
            if (SEBClientInfo.IsNewOS)
            {
                sessionCreateNewDesktop = (Boolean)SEBClientInfo.getSebSetting(SEBSettings.KeyCreateNewDesktop)[SEBSettings.KeyCreateNewDesktop];
                if (sessionCreateNewDesktop)
                {
                    SEBClientInfo.OriginalDesktop = SEBDesktopController.GetCurrent();
                    SEBDesktopController OriginalInput = SEBDesktopController.OpenInputDesktop();

                    SEBClientInfo.SEBNewlDesktop = SEBDesktopController.CreateDesktop(SEBClientInfo.SEB_NEW_DESKTOP_NAME);
                    SEBDesktopController.Show(SEBClientInfo.SEBNewlDesktop.DesktopName);
                    if (!SEBDesktopController.SetCurrent(SEBClientInfo.SEBNewlDesktop))
                    {
                        Logger.AddError("SetThreadDesktop failed! Looks like the thread has hooks or windows in the current desktop.", null, null);
                        SEBDesktopController.Show(SEBClientInfo.OriginalDesktop.DesktopName);
                        SEBDesktopController.SetCurrent(SEBClientInfo.OriginalDesktop);
                        SEBClientInfo.SEBNewlDesktop.Close();
                        SEBErrorMessages.OutputErrorMessageNew(SEBUIStrings.createNewDesktopFailed, SEBUIStrings.createNewDesktopFailedReason, SEBGlobalConstants.IND_MESSAGE_KIND_ERROR, MessageBoxButtons.OK);
                        return false;
                    }
                    SEBClientInfo.DesktopName = SEBClientInfo.SEB_NEW_DESKTOP_NAME;
                }
                else
                {
                    SEBClientInfo.OriginalDesktop = SEBDesktopController.GetCurrent();
                    SEBClientInfo.DesktopName = SEBClientInfo.OriginalDesktop.DesktopName;
                    SebWindowsClientForm.SetVisibility(false);
                }
            }


            return InitSEBDesktop();
        }

        /// ----------------------------------------------------------------------------------------
        /// <summary>
        /// Create and initialize new desktop.
        /// </summary>
        /// <returns>true if succeeded</returns>
        /// ----------------------------------------------------------------------------------------
        public static bool InitSEBDesktop()
        {
            // Clean clipboard
            SEBClipboard.CleanClipboard();
            Logger.AddInformation("Clipboard cleaned.", null, null);

            // Global variable indicating if the explorer shell has been killed
            SEBClientInfo.ExplorerShellWasKilled = false;

            // locks OS
            if (!SEBClientInfo.IsNewOS)
            {
                ////just kill explorer.exe on Win9x / Me
                bool killExplorerShell = false;

                List<object> prohibitedProcessList = (List<object>)SEBClientInfo.getSebSetting(SEBSettings.KeyProhibitedProcesses)[SEBSettings.KeyProhibitedProcesses];
                for (int i = 0; i < prohibitedProcessList.Count(); i++)
                {
                    Dictionary<string, object> prohibitedProcess = (Dictionary<string, object>)prohibitedProcessList[i];
                    string prohibitedProcessName = (string)prohibitedProcess[SEBSettings.KeyExecutable];
                    if ((Boolean)prohibitedProcess[SEBSettings.KeyActive])
                    {
                        if (prohibitedProcessName.Contains("explorer.exe"))
                        {
                            killExplorerShell = true;
                        }
                    }
                }

                if (killExplorerShell)
                {
                    Logger.AddInformation("Kill process by name (explorer.exe)", null, null);
                    SEBNotAllowedProcessController.KillProcessByName("explorer.exe");
                    SEBClientInfo.ExplorerShellWasKilled = true;
                    Logger.AddInformation("Process by name (explorer.exe) killed", null, null);
                }
                //tell Win9x / Me that the screensaver is running to lock system tasks
                if (!(Boolean)SEBClientInfo.getSebSetting(SEBSettings.KeyCreateNewDesktop)[SEBSettings.KeyCreateNewDesktop])
                {
                    SEBDesktopController.DisableTaskSwitching();
                }
            }
            else
            {
                //on NT4/NT5 the desktop is killed
                if ((Boolean)SEBClientInfo.getSebSetting(SEBSettings.KeyKillExplorerShell)[SEBSettings.KeyKillExplorerShell])
                {
                    Logger.AddInformation("Kill process by PostMessage(WM_USER + 436)", null, null);

                    //SEBNotAllowedProcessController.KillProcessByName("explorer.exe");
                    //PostMessage(FindWindow("Shell_TrayWnd", null), WM_USER + 436, (IntPtr)0, (IntPtr)0);

                    // When starting up SEB, kill the explorer.exe shell
                    try
                    {
                        var ptr = FindWindow("Shell_TrayWnd", null);
                        Logger.AddInformation("INIT PTR: {0}", ptr.ToInt32(), null, null);
                        PostMessage(ptr, WM_USER + 436, (IntPtr)0, (IntPtr)0);

                        do
                        {
                            ptr = FindWindow("Shell_TrayWnd", null);
                            Logger.AddInformation("PTR: {0}", ptr.ToInt32(), null, null);

                            if (ptr.ToInt32() == 0)
                            {
                                Logger.AddInformation("Success. Breaking out of loop.", null, null);
                                break;
                            }

                            Thread.Sleep(1000);
                        }
                        while (true);
                    }
                    catch (Exception ex)
                    {
                        Logger.AddInformation("{0} {1}", ex.Message, null, null);
                        SEBClientInfo.ExplorerShellWasKilled = false;
                    }

                    Logger.AddInformation("Process by PostMessage(WM_USER + 436) killed", null, null);
                    SEBClientInfo.ExplorerShellWasKilled = true;
                }

            }

            return true;
        }

        /// ----------------------------------------------------------------------------------------
        /// <summary>
        /// Reset desktop to the default one which was active before starting SEB.
        /// </summary>
        /// <returns>true if succeed</returns>
        /// ----------------------------------------------------------------------------------------
        public static void ResetSEBDesktop()
        {
            // Switch to Default Desktop
            if (sessionCreateNewDesktop)
            {
                SEBDesktopController.Show(SEBClientInfo.OriginalDesktop.DesktopName);
                SEBDesktopController.SetCurrent(SEBClientInfo.OriginalDesktop);
                SEBClientInfo.SEBNewlDesktop.Close();
            }
            else
            {
                SetVisibility(true);
            }
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
        /// Check if running in VM and if SEB Windows Service is running or not.
        /// </summary>
        /// <returns>true if both checks are positive, false means SEB needs to quit.</returns>
        /// ----------------------------------------------------------------------------------------
        public static bool CheckVMService()
        {
            // Test if run inside virtual machine
            bool allowVirtualMachine = (Boolean)SEBClientInfo.getSebSetting(SEBSettings.KeyAllowVirtualMachine)[SEBSettings.KeyAllowVirtualMachine];
            if (IsInsideVM() && (!allowVirtualMachine))
            //if ((IsInsideVMWare() || IsInsideVPC()) && (!allowVirtualMachine))
            {
                //SEBClientInfo.SebWindowsClientForm.Activate();
                SEBErrorMessages.OutputErrorMessageNew(SEBUIStrings.detectedVirtualMachine, SEBUIStrings.detectedVirtualMachineForbiddenMessage, SEBGlobalConstants.IND_MESSAGE_KIND_ERROR, MessageBoxButtons.OK);
                Logger.AddError("Forbidden to run SEB on a virtual machine!", null, null);
                Logger.AddInformation("Safe Exam Browser is exiting", null, null);
                Application.Exit();
                return false;
            }

            // Test if Windows Service is running
            bool serviceAvailable = SEBWindowsServiceController.ServiceAvailable(SEBClientInfo.SEB_WINDOWS_SERVICE_NAME);
            if (serviceAvailable)
            {
                Logger.AddInformation("SEB Windows service available", null, null);
            }
            else
            {
                Logger.AddInformation("SEB Windows service is not available.", null, null);
            }

            int forceService = (Int32)SEBClientInfo.getSebSetting(SEBSettings.KeySebServicePolicy)[SEBSettings.KeySebServicePolicy];
            switch (forceService)
            {
                case (int)sebServicePolicies.ignoreService:
                    break;
                case (int)sebServicePolicies.indicateMissingService:
                    if (!serviceAvailable)
                    {
                        //SEBClientInfo.SebWindowsClientForm.Activate();
                        SEBErrorMessages.OutputErrorMessageNew(SEBUIStrings.indicateMissingService, SEBUIStrings.indicateMissingServiceReason, SEBGlobalConstants.IND_MESSAGE_KIND_ERROR, MessageBoxButtons.OK);
                    }
                    break;
                case (int)sebServicePolicies.forceSebService:
                    if (!serviceAvailable)
                    {
                        //SEBClientInfo.SebWindowsClientForm.Activate();
                        SEBErrorMessages.OutputErrorMessageNew(SEBUIStrings.indicateMissingService, SEBUIStrings.forceSebServiceMessage, SEBGlobalConstants.IND_MESSAGE_KIND_ERROR, MessageBoxButtons.OK);
                        Logger.AddError("SEB Windows service is not available and sebServicePolicies is set to forceSebService", null, null);
                        Logger.AddInformation("SafeExamBrowser is exiting", null, null);
                        Application.Exit();

                        return false;
                    }
                    break;
                //default:
                //    if (!serviceAvailable)
                //    {
                //        SEBErrorMessages.OutputErrorMessage(SEBGlobalConstants.IND_WINDOWS_SERVICE_NOT_AVAILABLE, SEBGlobalConstants.IND_MESSAGE_KIND_ERROR);
                //    }
                //    break;
            }

            return true;
        }
    }
}
