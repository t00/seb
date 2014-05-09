using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Alchemy;
using Alchemy.Classes;
using SebWindowsClient.DiagnosticsUtils;

namespace SebWindowsClient.WebSocketsServer
{
    public class SEBXULRunnerWebSocketServer
    {
        public static string ServerAddress
        {
            get
            {
                return String.Format("ws://localhost:{0}/",port);
            }
        }

        private static int port = 8706;
        private static WebSocketServer server;

        private static UserContext SEB;
        private static UserContext XULRunner;

        public static void StartServer()
        {
            if (server != null)
                return;

            try
            {
                server = new WebSocketServer(port, IPAddress.Parse("127.0.0.1"))
                {
                    OnReceive = OnReceive,
                    OnSend = OnSend,
                    OnConnect = OnConnect,
                    OnConnected = OnConnected,
                    OnDisconnect = OnDisconnect,
                    TimeOut = new TimeSpan(0, 5, 0)
                };
                server.Start();
                Logger.AddInformation("Starting WebSocketServer on " + ServerAddress,null,null);
            }
            catch (Exception ex)
            {
                Logger.AddError("Unable to start WebSocketsServer for communication with XulRunner", null, ex);
            }
            
        }

        public static void StopServer()
        {
            if (server != null)
            {
                server.Stop();
                server.Dispose();
                server = null; 
            }
        }

        private static void OnDisconnect(UserContext context)
        {
        }

        private static void OnConnect(UserContext context)
        {
        }

        private static void OnSend(UserContext context)
        {
        }

        private static void OnReceive(UserContext context)
        {
            Console.WriteLine("Received Data From :" + context.ClientAddress + " : " + context.DataFrame.ToString());

            if (SEB == null || XULRunner == null)
                return;

            //Get the receiver - it's not the sender :)
            var receiver = context.ClientAddress == SEB.ClientAddress ? XULRunner : SEB;
            //Forward the message to him
            receiver.Send(context.DataFrame);
        }

        static void OnConnected(UserContext context)
        {
            Console.WriteLine("Client Connection From : " + context.ClientAddress.ToString());

            if (SEB == null)
                SEB = context;
            else
                XULRunner = context;
        }
    }
}
