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
using COM.Tools.VMDetect;
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
    }

    static class SebWindowsClientMain
    {
        // For killing the Explorer Shell at SEB startup

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool PostMessage(IntPtr hWnd, [MarshalAs(UnmanagedType.U4)] uint Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        const int WM_USER = 0x0400; //http://msdn.microsoft.com/en-us/library/windows/desktop/ms644931(v=vs.85).aspx


        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        //[STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if (InitSebDesktop())
             {
                 SEBClientInfo.SebWindowsClientForm = new SebWindowsClientForm();
                 string[] arguments = Environment.GetCommandLineArgs();
                 SingleInstanceController controller = new SingleInstanceController();
                 controller.Run(arguments);
             }
        }


        /// <summary>
        /// Detect if SEB Running inside VPC.
        /// </summary>
        private static bool IsInsideVPC()
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
        }


        /// ----------------------------------------------------------------------------------------
        /// <summary>
        /// Create and initialise new desktop.
        /// </summary>
        /// <returns>true if succeed</returns>
        /// ----------------------------------------------------------------------------------------
        private static bool InitSebDesktop()
        {
            //// Initialize the password entry dialog form
            SebWindowsClient.ConfigurationUtils.SEBConfigFileManager.InitSEBConfigFileManager();

            //SebWindowsClientForm.SetVisibility(true);

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
                Logger.AddError("Unknown OS. Exit SEB.",null,null);
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
            switch(forceService)
            {
                case (int)sebServicePolicies.ignoreService:
                    break;
                case (int)sebServicePolicies.indicateMissingService:
                    if (!serviceAvailable)
                    {
                        SEBErrorMessages.OutputErrorMessage(SEBGlobalConstants.IND_WINDOWS_SERVICE_NOT_AVAILABLE, SEBGlobalConstants.IND_MESSAGE_KIND_ERROR);
                    }
                    break;
                case (int)sebServicePolicies.forceSebService:
                    if (!serviceAvailable)
                    {
                        SEBErrorMessages.OutputErrorMessage(SEBGlobalConstants.IND_WINDOWS_SERVICE_NOT_AVAILABLE, SEBGlobalConstants.IND_MESSAGE_KIND_ERROR);
                        Logger.AddError("SEB Windows service is not available and sebServicePolicies is set to forceSebService", null, null);
                        Logger.AddInformation("Leave SebStarter", null, null);
                        return false;
                    }
                    break;
                default:
                    if (!serviceAvailable)
                    {
                        SEBErrorMessages.OutputErrorMessage(SEBGlobalConstants.IND_WINDOWS_SERVICE_NOT_AVAILABLE, SEBGlobalConstants.IND_MESSAGE_KIND_ERROR);
                    }
                    break;
            }

             // Test if run inside virtual machine
            bool allowVirtualMachine = (Boolean)SEBClientInfo.getSebSetting(SEBSettings.KeyAllowVirtualMachine)[SEBSettings.KeyAllowVirtualMachine];
            if ((IsInsideVMWare() || IsInsideVPC()) && (!allowVirtualMachine))
            {
                SEBErrorMessages.OutputErrorMessage(SEBGlobalConstants.IND_VIRTUAL_MACHINE_FORBIDDEN, SEBGlobalConstants.IND_MESSAGE_KIND_ERROR);
                Logger.AddError("Forbidden to run SEB on a VIRTUAL machine!", null, null);
                Logger.AddInformation("Leave SebStarter", null, null);
                return false;

            }

            // Clean clipboard
            SEBClipboard.CleanClipboard();
            Logger.AddInformation("Clipboard deleted.", null, null);


            // Global variable if the explorer shell has been killed
            SEBClientInfo.ExplorerShellWasKilled = false;

            // locks OS
            if (!SEBClientInfo.IsNewOS)
            {
                ////just kill explorer.exe on Win9x / Me
                //sebSettings
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
                    Logger.AddInformation("Kill process by name(explorer.exe)", null, null);
                    SEBNotAllowedProcessController.KillProcessByName("explorer.exe");
                    SEBClientInfo.ExplorerShellWasKilled = true;
                    Logger.AddInformation("Process by name(explorer.exe) killed", null, null);
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


                //on NT4/NT5 a new desktop is created
                if ((Boolean)SEBClientInfo.getSebSetting(SEBSettings.KeyCreateNewDesktop)[SEBSettings.KeyCreateNewDesktop])
                {
                    SEBClientInfo.OriginalDesktop = SEBDesktopController.GetCurrent();
                    SEBDesktopController OriginalInput = SEBDesktopController.OpenInputDesktop();

                    SEBClientInfo.SEBNewlDesktop = SEBDesktopController.CreateDesktop(SEBClientInfo.SEB_NEW_DESKTOP_NAME);
                    SEBDesktopController.Show(SEBClientInfo.SEBNewlDesktop.DesktopName);
                    SEBDesktopController.SetCurrent(SEBClientInfo.SEBNewlDesktop);
                    SEBClientInfo.DesktopName = SEBClientInfo.SEB_NEW_DESKTOP_NAME;

                }
                else
                {
                    SEBClientInfo.OriginalDesktop = SEBDesktopController.GetCurrent();
                    SEBClientInfo.DesktopName = SEBClientInfo.OriginalDesktop.DesktopName;
                    SebWindowsClientForm.SetVisibility(false);
                }
            }

            return true;
        }
    }
}
