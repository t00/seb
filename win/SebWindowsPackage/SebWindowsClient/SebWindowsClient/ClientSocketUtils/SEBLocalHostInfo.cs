// -------------------------------------------------------------
//     Viktor tomas
//     BFH-TI, http://www.ti.bfh.ch
//     Biel, 2012
// -------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using SebWindowsClient.DiagnosticsUtils;
using System.Security.Principal;

namespace SebWindowsClient.ClientSocketUtils
{
    /// ---------------------------------------------------------------------------------------
    /// <summary>
    /// Gets info about host and user SID.
    /// </summary>
    /// ---------------------------------------------------------------------------------------
    public class SEBLocalHostInfo
    {
        public const string defaultUserName = "";
        public const string defaultHostName = "localhost";
        public const int defaultPortNumber = 57016;
        public const int defaultSendInterval = 100;
        public const int defaultRecvTimeout = 100;
        public const int defaultNumMessages = 3;


        /// <summary>
        /// Gets host info.
        /// </summary>
        /// <returns></returns>
        public IPHostEntry GetHostInfo()
        {
            IPHostEntry hostInfo = null;
            try
            {
                // Die Methode Dns.GetHostName() gibt den Namen der lokalen Maschine zurück 
                String localHostName = Dns.GetHostName();
                Logger.AddInformation("Host Name: " + localHostName, this, null);
                hostInfo = GettHostInfo(localHostName);
            } catch (Exception ex) {
                Logger.AddError("Error ocurred by GetHostInfo.", this, ex);
            }
            return hostInfo;
        }

        /// <summary>
        /// Gets user SID.
        /// </summary>
        /// <returns></returns>
        public SecurityIdentifier GetSID()
        {
            SecurityIdentifier sid = null;
            try
            {
                WindowsIdentity user = WindowsIdentity.GetCurrent();
                sid = user.User;
            }
            catch (Exception ex)
            {
                Logger.AddError("Error ocurred by GetSID.", this, ex);
            }

            return sid;
        }

        /// <summary>
        /// Gets user name.
        /// </summary>
        /// <returns></returns>
        public string GetUserName()
        {
            string name = null;
            try
            {
                WindowsIdentity user = WindowsIdentity.GetCurrent();
                name = user.Name;
            }
            catch (Exception ex)
            {
                Logger.AddError("Error ocurred by GetUserName.", this, ex);
            }

            return name;
        }


        /// <summary>
        /// Gets host info by name.
        /// </summary>
        /// <returns></returns>
        public IPHostEntry GetHostInfoByName(string hostNameOrAddress)
        {
            IPHostEntry hostInfo = null;
            try
            {
                 Logger.AddInformation("Host Name: " + hostNameOrAddress, this, null);
                 hostInfo = GettHostInfo(hostNameOrAddress);
            }
            catch (Exception ex)
            {
                Logger.AddError("Error ocurred by GetHostInfoByName.", this, ex);
            }
            return hostInfo;
        }

        /// <summary>
        /// Gets host info.
        /// </summary>
        /// <returns></returns>
        private IPHostEntry GettHostInfo(String host)
        {
            IPHostEntry hostInfo = null;
            try
            {

                // Versuche die DNS für die übergebenen Host und IP-Adressen aufzulösen
                hostInfo = Dns.GetHostEntry(host);

                // Ausgabe des kanonischen Namens
                Logger.AddInformation("Canonical Name: " + hostInfo.HostName, this, null);
                //Console.WriteLine("\tCanonical Name: " + hostInfo.HostName);

                // Ausgabe der IP-Adressen
                StringBuilder addesses = new StringBuilder("IP Addresses: ");
                foreach (IPAddress ipaddr in hostInfo.AddressList) {
                    addesses.Append(ipaddr.ToString() + " ");
                }
                Logger.AddInformation(addesses.ToString(), this, null);

                // Ausgabe der Alias-Namen für diesen Host
                StringBuilder aliases = new StringBuilder("Aliases: ");
                foreach (String alias in hostInfo.Aliases)
                {
                    aliases.Append(alias + " ");
                }
                Logger.AddInformation(aliases.ToString(), this, null);

            } catch (Exception ex) {
                Logger.AddError("Unable to resolve host: " + host, this, ex);
                //Console.WriteLine("\tUnable to resolve host: " + host + "\n");
            }
            return hostInfo;
        }

    }
}
