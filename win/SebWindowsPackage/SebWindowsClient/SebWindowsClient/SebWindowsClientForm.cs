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

// -------------------------------------------------------------
//     Viktor tomas
//     BFH-TI, http://www.ti.bfh.ch
//     Biel, 2012
// -------------------------------------------------------------
namespace SebWindowsClient
{
    public partial class SebWindowsClientForm : Form
    {
        public SebWindowsClientForm()
        {
            InitializeComponent();
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
            SEBDesktopController.Show(SEBClientInfo.OriginalDesktop.DesktopName);
            SEBDesktopController.SetCurrent(SEBClientInfo.OriginalDesktop);
            SEBClientInfo.SEBNewlDesktop.Close();

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
            KeyCapture.FilterKeys = true;
        }
    }
}
