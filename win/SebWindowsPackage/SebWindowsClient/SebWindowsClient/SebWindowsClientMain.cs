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

namespace SebWindowsClient
{
    static class SebWindowsClientMain
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
//        [STAThread]
        static void Main()
        {

            if (InitSebDesktop())
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new SebWindowsClientForm());
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
        /// Serialise and encrypt settings.
        /// </summary>
        /// <param name="publicKeyHash">public key hash</param>
        /// ----------------------------------------------------------------------------------------
        public static void SerialiseAndEncryptSettings(byte[] publicKeyHash)
        {
            MemoryStream settingsMemStream = new MemoryStream();
            XmlSerializer serializer = new XmlSerializer(typeof(SEBClientConfig));
            serializer.Serialize(settingsMemStream, SEBClientInfo.sebClientConfig);
            byte[] settingsDataBytes = settingsMemStream.ToArray();
            // Get certificate from disk
            SEBProtectionController sEBProtectionControler = new SEBProtectionController();
            //sEBProtectionControler.KeyCertFilename = "C:\\SebWindowsClient\\SEBConfigKeys\\SEB-Configuration.pfx";
            //sEBProtectionControler.KeyCertPassword = "seb-configuration";
            //X509Certificate2 myCertificate = new X509Certificate2(sEBProtectionControler.KeyCertFilename, sEBProtectionControler.KeyCertPassword, X509KeyStorageFlags.Exportable);
            // Encrypt seb client settings
            string settingsData = Encoding.ASCII.GetString(settingsDataBytes);
            sEBProtectionControler.EncryptWithPasswordAndSave(settingsData);
        }


        /// ----------------------------------------------------------------------------------------
        /// <summary>
        /// Create and initialise new desktop.
        /// </summary>
        /// <returns>true if succeed</returns>
        /// ----------------------------------------------------------------------------------------
        private static bool InitSebDesktop()
        {

           // Set SebClient configuration
            if (!SEBClientInfo.SetSebClientConfiguration())
            {
                SEBErrorMessages.OutputErrorMessage(SEBGlobalConstants.IND_SEB_CLIENT_SEB_ERROR, SEBGlobalConstants.IND_MESSAGE_KIND_ERROR);
                Logger.AddError("Error when opening the file SebClient.seb!", null, null);
                return false;
            }
 
            // Set XulRunner configuration
            if (!SEBClientInfo.SetXulRunnerConfiguration())
            {
                SEBErrorMessages.OutputErrorMessage(SEBGlobalConstants.IND_CONFIG_JSON_ERROR, SEBGlobalConstants.IND_MESSAGE_KIND_ERROR);
                Logger.AddError("Error when opening the file config.json!", null, null);
                return false;
            }

            // Trunk version (XUL-Runner)
            if (!SEBClientInfo.SetSystemVersionInfo())
            {
                SEBErrorMessages.OutputErrorMessage(SEBGlobalConstants.IND_NO_OS_SUPPORT, SEBGlobalConstants.IND_MESSAGE_KIND_ERROR);
                Logger.AddError("Unknown OS. Exit SEB.",null,null);
                return false;
            }

            // Test if Windows Service is running
            bool serviceAvailable = SEBWindowsServiceController.ServiceAvailable("SebWindowsService");
            if (serviceAvailable)
            {
                Logger.AddInformation("SEB Windows service available", null, null);
            }
            else
            {
                Logger.AddInformation("SEB Windows service is not available.", null, null);
            }

            int forceService = int.Parse(SEBClientInfo.sebClientConfig.getPolicySetting("sebServicePolicy").Value);
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
            if ((IsInsideVMWare() || IsInsideVPC()) && (!SEBClientInfo.sebClientConfig.getSecurityOption("allowVirtualMachine").getBool()))
            {
                SEBErrorMessages.OutputErrorMessage(SEBGlobalConstants.IND_VIRTUAL_MACHINE_FORBIDDEN, SEBGlobalConstants.IND_MESSAGE_KIND_ERROR);
                Logger.AddError("Forbidden to run SEB on a VIRTUAL machine!", null, null);
                Logger.AddInformation("Leave SebStarter", null, null);
                return false;

            }

            // Clean clipboard
            SEBClipboard.CleanClipboard();
            Logger.AddInformation("Clipboard deleted.", null, null);

            // locks OS
            if (!SEBClientInfo.IsNewOS)
            {
                ////just kill explorer.exe on Win9x / Me
                //sEBSettings
                bool killExplorer = false;
                for (int i = 0; i < SEBClientInfo.sebClientConfig.ProhibitedProcesses.Count(); i++)
                {
                    if(SEBClientInfo.sebClientConfig.ProhibitedProcesses[i].nameWin.CompareTo("explorer.exe") == 0)
                    {
                        killExplorer = true;
                    }
                }

                if (killExplorer)
                {
                    Logger.AddInformation("Kill process by name(explorer.exe)", null, null);
                    SEBNotAllowedProcessController.KillProcessByName("explorer.exe");
                    Logger.AddInformation("Process by name(explorer.exe) killed", null, null);
                 }
                //tell Win9x / Me that the screensaver is running to lock system tasks
                if (!SEBClientInfo.sebClientConfig.getSecurityOption("createNewDesktop").getBool())
                {
                    SEBDesktopController.DisableTaskSwitching();
                }
            }
            else
            {
                //on NT4/NT5 a new desktop is created
                if (SEBClientInfo.sebClientConfig.getSecurityOption("createNewDesktop").getBool())
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
