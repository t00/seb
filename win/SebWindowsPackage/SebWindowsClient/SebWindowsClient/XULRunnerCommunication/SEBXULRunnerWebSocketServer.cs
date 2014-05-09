using System;
using System.Net;
using System.Net.Sockets;
using System.ServiceModel.Security;
using Fleck;
using SebWindowsClient.DiagnosticsUtils;

namespace SebWindowsClient.XULRunnerCommunication
{
    /// <summary>
    /// WebSocket Server to communicate with the XULRunner
    /// </summary>
    public class SEBXULRunnerWebSocketServer
    {
        /// <summary>
        /// The URL to connect to
        /// </summary>
        public static string ServerAddress
        {
            get
            {
                return String.Format("ws://localhost:{0}",port);
            }
        }

        public static bool IsRunning
        {
            get
            {
                return server != null && XULRunner != null;
            }
        }

        public static event EventHandler OnXulRunnerCloseRequested;
        public static event EventHandler OnXulRunnerQuitLinkClicked;

        private static IWebSocketConnection XULRunner;

        private static int port = 8706;
        private static WebSocketServer server;

        /// <summary>
        /// Start the server if not already running
        /// </summary>
        public static void StartServer()
        {
            if (server != null)
                return;

            try
            {
                server = new WebSocketServer(ServerAddress);
                FleckLog.Level = LogLevel.Debug;
                server.Start(socket =>
                {
                    socket.OnOpen = () => XULRunner = socket;
                    socket.OnClose = () => XULRunner = null;
                    socket.OnMessage = message => OnClientMessage(message);
                });
                Logger.AddInformation("Starting WebSocketServer on " + ServerAddress,null,null);
            }
            catch (Exception ex)
            {
                Logger.AddError("Unable to start WebSocketsServer for communication with XulRunner", null, ex);
            }
        }

        public static void SendAllowCloseToXulRunner()
        {
            try
            {
                if(XULRunner != null)
                XULRunner.Send("SEB.close");
            }
            catch (Exception)
            {
            }
            
        }

        private static void OnClientMessage(string message)
        {
            switch (message)
            {
                case "seb.beforeclose.manual":
                    if (OnXulRunnerCloseRequested != null)
                        OnXulRunnerCloseRequested(null, EventArgs.Empty);
                    break;
                case "seb.beforeclose.quiturl":
                    if (OnXulRunnerQuitLinkClicked != null)
                        OnXulRunnerQuitLinkClicked(null, EventArgs.Empty);
                    break;
            }
        }
    }
}
