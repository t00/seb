using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using SebWindowsClient.ConfigurationUtils;
using SebWindowsClient.WebSocketsServer;

namespace SebWindowsClient.XULRunnerCommunication
{
    public static class SEBXulRunnerHandler
    {
        /// <summary>
        /// Initializes the Communication to the XULRunner
        /// This must be executed before XULrunner starts for the first time!
        /// </summary>
        public static void Initialize()
        {
            SEBXULRunnerWebSocketServer.StartServer();
            SEBWebSocketClient.Initialize();
        }

        public static bool IsCommunicationEstablished
        {
            get
            {
                return SEBWebSocketClient.IsConnectionEstablished;
            }
        }

        /// <summary>
        /// Send a shutdown request to the XULRunner
        /// </summary>
        public static void AllowCloseXulRunner()
        {
            SEBWebSocketClient.Send(SEBWebSocketClient.XULRunner_Close);
        }
    }
}
