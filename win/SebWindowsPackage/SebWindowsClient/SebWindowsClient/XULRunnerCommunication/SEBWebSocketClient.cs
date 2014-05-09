using System;
using System.Timers;
using Alchemy;
using Alchemy.Classes;
using SebWindowsClient.ConfigurationUtils;
using SebWindowsClient.DiagnosticsUtils;

namespace SebWindowsClient.WebSocketsServer
{
    public static class SEBWebSocketClient
    {
        public static event EventHandler OnShutDownRequested;
        public static event EventHandler OnQuitLink;

        private static WebSocketClient client;

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

        public static bool IsConnectionEstablished
        {
            get
            {
                return client != null;
            }
        }

        public const string XULRunner_Close = "SEB.close";
        public const string XULRunner_OnClosing = "seb.beforeclose.manual";
        public const string XULRunner_QuitLink = "seb.beforeclose.quiturl";
        public const string SocketServerPing = "SEB.ping";

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
            if(client == null) Initialize();
            if(client != null) client.Send(message);
        }

    }
}
