using System;
using System.Net.NetworkInformation;
using System.Threading;
using System.Windows;
using Fleck;
using SebShared.DiagnosticUtils;
using SebShared.Properties;
using SebShared.Utils;
using SebShared;
using SebWindowsClient.ConfigurationUtils;

namespace SebWindowsClient.XULRunnerCommunication
{
	/// <summary>
	/// WebSocket Server to communicate with the XULRunner
	/// </summary>
	public class SEBXULRunnerWebSocketServer
	{
		public static bool Started = false;

		/// <summary>
		/// The URL to connect to
		/// </summary>
		public static string ServerAddress
		{
			get
			{
				return String.Format("ws://127.0.0.1:{0}", XulRunnerPort);
			}
		}

		public static bool IsRunning
		{
			get
			{
				if(server != null)
					return true;

				IPGlobalProperties ipGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();
				TcpConnectionInformation[] tcpConnInfoArray = ipGlobalProperties.GetActiveTcpConnections();

				foreach(TcpConnectionInformation tcpi in tcpConnInfoArray)
				{
					if(tcpi.LocalEndPoint.Port == XulRunnerPort && tcpi.State != TcpState.TimeWait)
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
		public static event EventHandler OnXulRunnerTextFocus;
		public static event EventHandler OnXulRunnerTextBlur;

		private static IWebSocketConnection XULRunner;

		private const int XulRunnerPort = 8706;

		private static WebSocketServer server;

		public static bool CheckStartServer()
		{
			StartServer();
			if(!Started)
			{
				Logger.AddInformation("SEBXULRunnerWebSocketServer.Started returned false, this means the WebSocketServer communicating with the SEB XULRunner browser couldn't be started, exiting");
				SebMessageBox.Show(SEBUIStrings.webSocketServerNotStarted, SEBUIStrings.webSocketServerNotStartedMessage, MessageBoxImage.Error, MessageBoxButton.OK);
				SEBClientInfo.SebWindowsClientForm.ExitApplication();
				return false;
			}
			return true;
		}

		/// <summary>
		/// Start the server if not already running
		/// </summary>
		public static void StartServer()
		{
			if(IsRunning && Started)
				return;

			if(IsRunning)
			{
				for(int i = 0; i < 60; i++)
				{
					if(!IsRunning)
						break;

					Thread.Sleep(1000);
				}
				if(IsRunning)
				{
					SebMessageBox.Show(SEBUIStrings.alertWebSocketPortBlocked, SEBUIStrings.alertWebSocketPortBlockedMessage, MessageBoxImage.Error, MessageBoxButton.OK);
				}
			}

			try
			{
				Logger.AddInformation("Starting WebSocketServer on " + ServerAddress);
				server = new WebSocketServer(ServerAddress);
				FleckLog.Level = LogLevel.Debug;
				server.Start(socket =>
				{
					socket.OnOpen = () => OnClientConnected(socket);
					socket.OnClose = OnClientDisconnected;
					socket.OnMessage = OnClientMessage;
				});
				Logger.AddInformation("Started WebSocketServer on " + ServerAddress);
				Started = true;
			}
			catch(Exception ex)
			{
				Logger.AddError("Unable to start WebSocketsServer for communication with XULRunner", null, ex);
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
				if(XULRunner != null)
				{
					Console.WriteLine("SEB.Close sent");
					Logger.AddInformation("WebSocket: Send message: SEB.close");
					XULRunner.Send("SEB.close");
				}
			}
			catch(Exception)
			{
			}
		}

		public static void SendRestartExam()
		{
			try
			{
				if(XULRunner != null &&
				   (!string.IsNullOrEmpty(SebInstance.Settings.Get<string>(SebSettings.KeyRestartExamURL))
				   || SebInstance.Settings.Get<bool>(SebSettings.KeyRestartExamUseStartURL)))
				{
					Console.WriteLine("SEB.Restart Exam sent");
					Logger.AddInformation("WebSocket: Send message: SEB.restartExam");
					XULRunner.Send("SEB.restartExam");
				}
			}
			catch(Exception)
			{
			}
		}

		public static void SendReloadPage()
		{
			try
			{
				if(XULRunner != null)
				{
					Console.WriteLine("SEB.Reload Sent");
					Logger.AddInformation("WebSocket: Send message: SEB.reload");
					XULRunner.Send("SEB.reload");
				}
			}
			catch(Exception)
			{
			}
		}

		private static void OnClientMessage(string message)
		{
			Console.WriteLine("RECV: " + message);
			Logger.AddInformation("WebSocket: Received message: " + message);
			switch(message)
			{
				case "seb.beforeclose.manual":
					if(OnXulRunnerCloseRequested != null)
						OnXulRunnerCloseRequested(null, EventArgs.Empty);
					break;
				case "seb.beforeclose.quiturl":
					if(OnXulRunnerQuitLinkClicked != null)
						OnXulRunnerQuitLinkClicked(null, EventArgs.Empty);
					break;
				case "seb.input.focus":
					if(OnXulRunnerTextFocus != null)
						OnXulRunnerTextFocus(null, EventArgs.Empty);
					break;
				case "seb.input.blur":
					if(OnXulRunnerTextBlur != null)
						OnXulRunnerTextBlur(null, EventArgs.Empty);
					break;
			}
		}

		public static void SendDisplaySettingsChanged()
		{
			try
			{
				if(XULRunner != null)
				{
					Console.WriteLine("SEB.ChangedDisplaySettingsChanged");
					Logger.AddInformation("WebSocket: Send message: SEB.displaySettingsChanged");
					XULRunner.Send("SEB.displaySettingsChanged");
				}
			}
			catch(Exception)
			{
			}
		}
	}
}
