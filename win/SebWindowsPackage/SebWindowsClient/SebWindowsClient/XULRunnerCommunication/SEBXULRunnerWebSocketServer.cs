using System;
using System.ComponentModel;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.ServiceModel.Channels;
using System.ServiceModel.Security;
using System.Threading;
using System.Windows;
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
                if (server != null)
                    return true;

                IPGlobalProperties ipGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();
                TcpConnectionInformation[] tcpConnInfoArray = ipGlobalProperties.GetActiveTcpConnections();

                foreach (TcpConnectionInformation tcpi in tcpConnInfoArray)
                {
                    if (tcpi.LocalEndPoint.Port == port)
                    {
                        Logger.AddInformation("Server already running!");
                        return true;
                    }
                }

                return false;
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
            if (IsRunning)
                return;

            try
            {
                server = new WebSocketServer(ServerAddress);
                FleckLog.Level = LogLevel.Debug;
                server.Start(socket =>
                {
                    socket.OnOpen = () => OnClientConnected(socket);
                    socket.OnClose = OnClientDisconnected;
                    socket.OnMessage = OnClientMessage;
                });
                Logger.AddInformation("Starting WebSocketServer on " + ServerAddress, null, null);
            }
            catch (Exception ex)
            {
                Logger.AddError("Unable to start WebSocketsServer for communication with XulRunner", null, ex);
            }
        }

        private static void OnClientDisconnected()
        {
            Logger.AddInformation("WebSocket: Client disconnected");
            XULRunner = null;
        }

        private static void OnClientConnected(IWebSocketConnection socket)
        {
            Logger.AddInformation("WebSocket: Client Connectedon port:" + socket.ConnectionInfo.ClientPort);
            XULRunner = socket;
        }

        public static void SendAllowCloseToXulRunner()
        {
            try
            {
                if (XULRunner != null)
                {
                    Console.WriteLine("SEB.Close sent");
                    Logger.AddInformation("WebSocket: Send message: SEB.close");
                    XULRunner.Send("SEB.close");
                }
            }
            catch (Exception)
            {
            }
        }

        private static void OnClientMessage(string message)
        {
            Console.WriteLine("RECV: " + message);
            Logger.AddInformation("WebSocket: Received message: " + message);
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
