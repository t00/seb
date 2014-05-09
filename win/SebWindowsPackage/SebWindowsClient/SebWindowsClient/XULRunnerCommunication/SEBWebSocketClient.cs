using System;
using System.Timers;
using Alchemy;
using Alchemy.Classes;
using SebWindowsClient.ConfigurationUtils;
using SebWindowsClient.DiagnosticsUtils;

namespace SebWindowsClient.WebSocketsServer
{
    /// <summary>
    /// The WebSocketClient for communication with the SEBXULRunnerWebSocketServer and finally with the XULRunner itself
    /// </summary>
    public static class SEBXULWebSocketClient
    {
        /// <summary>
        /// XULRunner has requested a shutdown (X-Icon was clicked)
        /// </summary>
        public static event EventHandler OnShutDownRequested;
        /// <summary>
        /// XULRunner has detected that the QuitLink has been clicked
        /// </summary>
        public static event EventHandler OnQuitLink;

        private static WebSocketClient client;

        /// <summary>
        /// Initialize the client and connect to the server
        /// </summary>
        public static void Initialize()
        {
            if (client != null)
                return;

            try
            {
                client = new WebSocketClient(SEBXULRunnerWebSocketServer.ServerAddress)
                {
                    OnReceive = OnReceive
                };
                client.Connect();

                //Send a ping every two minutes to keep the connection alive (timeout of the server is 5 minutes)
                var timer = new Timer();
                timer.Interval = 1000 * 60 * 2;
                timer.Elapsed += delegate
                {
                    client.Send(SocketServerPing);
                };
                timer.Start();

            }
            catch (Exception ex)
            {
                Logger.AddError("Unable to start WebSocketClient for communicating with XULRunner",null,ex);
            }
        }

        /// <summary>
        /// Checks if the client is running and connected
        /// </summary>
        public static bool IsConnectionEstablished
        {
            get
            {
                return client != null;
            }
        }

        /// <summary>
        /// The Message to send to the XULRunner so that it allows closing
        /// </summary>
        public const string XULRunner_Close = "SEB.close";
        /// <summary>
        /// The message the XULRunner send when the X-Button has been clicked
        /// </summary>
        public const string XULRunner_OnClosing = "seb.beforeclose.manual";
        /// <summary>
        /// The message the XulRunner sends when the quitlink has been clicked
        /// </summary>
        public const string XULRunner_QuitLink = "seb.beforeclose.quiturl";
        /// <summary>
        /// The ping message to keep the connection alive
        /// </summary>
        public const string SocketServerPing = "SEB.ping";

        /// <summary>
        /// When the client receives a message
        /// It decides which event to throw
        /// </summary>
        /// <param name="context"></param>
        private static void OnReceive(UserContext context)
        {
            Console.WriteLine("Client Received: " + context.DataFrame.ToString());
            switch (context.DataFrame.ToString())
            {
                case XULRunner_OnClosing:
                    if (OnShutDownRequested != null)
                        OnShutDownRequested(null, EventArgs.Empty);
                    break;
                case XULRunner_QuitLink:
                    if (OnQuitLink != null)
                        OnQuitLink(null, EventArgs.Empty);
                    break;
            }
        }

        public static void Send(string message)
        {
            Initialize();
            if(client != null) client.Send(message);
        }

    }
}
