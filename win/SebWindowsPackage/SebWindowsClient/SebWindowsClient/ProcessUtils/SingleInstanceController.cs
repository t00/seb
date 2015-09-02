using System;
using System.IO.Pipes;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Windows.Forms;

namespace SebWindowsClient.ProcessUtils
{
	public class SingleInstanceController<TMainForm>
		where TMainForm: Form
	{
		public delegate TMainForm CreateMainFormDelegate(string[] commandLine);
		public delegate void StartNextInstanceDelegate(TMainForm mainForm, string[] commandLine);

		public SingleInstanceController(CreateMainFormDelegate onCreateMainForm, StartNextInstanceDelegate onStartNextInstance)
			: this(onCreateMainForm, onStartNextInstance, ((GuidAttribute) Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(GuidAttribute), false).GetValue(0)).Value)
		{
		
		}

		public SingleInstanceController(CreateMainFormDelegate onCreateMainForm, StartNextInstanceDelegate onStartNextInstance, string appGuid)
		{
			this.onCreateMainForm = onCreateMainForm;
			this.onStartNextInstance = onStartNextInstance;
			this.appGuid = appGuid;
		}

		public void Run()
		{
			var arguments = Environment.GetCommandLineArgs().Skip(1).ToArray();
			var mutexId = string.Format("Global\\{{{0}}}", appGuid);
			using(var mutex = new Mutex(false, mutexId))
			{
				var hasHandle = true;
				try
				{
					try
					{
						hasHandle = mutex.WaitOne(5000, false);
					}
					catch(AbandonedMutexException)
					{
					}

					if(hasHandle)
					{
						var mainForm = onCreateMainForm(arguments);
						if(mainForm != null)
						{
							StartArgumentsServer(mainForm);
							Application.Run(mainForm);
						}
						else
						{
							Application.Exit();
						}
					}
					else
					{
						NotifyMainInstance(arguments);
						Application.Exit();
					}
				}
				finally
				{
					if(hasHandle)
					{
						mutex.ReleaseMutex();
					}
				}
			}
		}

		private void StartArgumentsServer(TMainForm mainForm)
		{
			var srv = new NamedPipeServerStream(appGuid, PipeDirection.InOut, 5, PipeTransmissionMode.Byte, PipeOptions.Asynchronous);

			srv.BeginWaitForConnection(state =>
			{
				var nps = (NamedPipeServerStream)state.AsyncState;
				nps.EndWaitForConnection(state);

				var bf = new BinaryFormatter();
				var args = (string[])bf.Deserialize(nps);

				mainForm.Invoke(new MethodInvoker(() => onStartNextInstance(mainForm, args)));

				nps.Disconnect();

				StartArgumentsServer(mainForm);
			}, srv);
		}

		private void NotifyMainInstance(string[] arguments)
		{
			using(var cli = new NamedPipeClientStream(appGuid))
			{
				cli.Connect();
				var bf = new BinaryFormatter();
				bf.Serialize(cli, arguments);
				cli.Flush();
			}
		}

		private readonly CreateMainFormDelegate onCreateMainForm;
		private readonly StartNextInstanceDelegate onStartNextInstance;
		private readonly string appGuid;
	}
}