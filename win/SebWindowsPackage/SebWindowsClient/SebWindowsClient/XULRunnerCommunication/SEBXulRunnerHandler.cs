using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using SebWindowsClient.ConfigurationUtils;
using SebWindowsClient.DiagnosticsUtils;
using SebWindowsClient.WebSocketsServer;

namespace SebWindowsClient.XULRunnerCommunication
{
    /// <summary>
    /// Handles the communication with the XULRunner
    /// </summary>
    public static class SEBXulRunnerHandler
    {
        /// <summary>
        /// Initializes the Communication to the XULRunner
        /// This must be executed before XULrunner starts for the first time!
        /// </summary>
        public static void Initialize()
        {
            SEBXULRunnerWebSocketServer.StartServer();
            SEBXULWebSocketClient.Initialize();
        }

        /// <summary>
        /// Checks if the communication to the WebSocketServer can be established
        /// </summary>
        public static bool IsCommunicationEstablished
        {
            get
            {
                return SEBXULWebSocketClient.IsConnectionEstablished;
            }
        }

        /// <summary>
        /// Send a shutdown request to the XULRunner
        /// This forces the xulrunner to allow a closing request which will be denied normally
        /// </summary>
        public static void AllowCloseXulRunner()
        {
            /*
            Logger.AddInformation("Sending Allow Close to XULRunner",null,null);
            SEBXULWebSocketClient.Send(SEBXULWebSocketClient.XULRunner_Close);
             * */
        }
    }
}
