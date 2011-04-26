using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;



namespace SebWindowsService
{
    public partial class SebWindowsService : ServiceBase
    {

        // Memorise the current state of the thread
        private Thread  threadLoop     = null;
        private Boolean threadExisting = false;
        private Boolean threadRunning  = false;


        // Constructor
        public SebWindowsService()
        {
            InitializeComponent();

            // Event logger
            if (!EventLog.SourceExists("SebEventLogSource"))
            {
                EventLog.CreateEventSource("SebEventLogSource", "SebEventLogFile");
            }
            sebEventLog.Source = "SebEventLogSource";
            sebEventLog.Log    = "SebEventLogFile";

            // Timer
            //System.Timers.Timer sebTimer = new System.Timers.Timer();
            //sebTimer.Enabled = true;

            DebugOutputLine(debugMode, "");
            DebugOutputLine(debugMode, "Enter SebWindowsService::SebWindowsService(Constructor)");

            // Set the initial state of the server loop thread

            threadLoop     = null;
            threadExisting = false;
            threadRunning  = false;

            DebugOutputLine(debugMode, "   threadLoop     = " + threadLoop);
            DebugOutputLine(debugMode, "   threadExisting = " + threadExisting);
            DebugOutputLine(debugMode, "   threadRunning  = " + threadRunning);

            DebugOutputLine(debugMode, "Leave SebWindowsService::SebWindowsService(Constructor)");
            DebugOutputLine(debugMode, "");
        }


/*
        private void TheTask()
        {
            IPAddress   localaddr = IPAddress.Parse("172.16.6.233");
            TcpListener listener  = new TcpListener(localaddr, 27016);
            listener.Start();
            while (true)
            {
                Thread.Sleep(1000);
                if (serviceOn)
                {
                    EventLog.WriteEntry("Started");
                    TcpClient client   = listener.AcceptTcpClient();
                    Stream    stream   = client.GetStream();
                    string    response = DateTime.Now.ToString();
                    stream.Write(Encoding.Unicode.GetBytes(resp), 0, 2*response.Length);
                    stream.Close();
                    client.Close();
                }
            }
        }
*/


        protected override void OnStart(string[] args)
        {
            DebugOutputLine(debugMode, "");
            DebugOutputLine(debugMode, "Enter SebWindowsService::OnStart()");

            // Create a new server loop thread

            DebugOutputLine(debugMode, "   threadLoop     = " + threadLoop);
            DebugOutputLine(debugMode, "   threadExisting = " + threadExisting);
            DebugOutputLine(debugMode, "   threadRunning  = " + threadRunning);

          //if (threadExisting == false)
            if (threadLoop == null)
            {
                DebugOutputLine(debugMode, "      Enter SebWindowsService::threadLoop = new Thread()");
                threadLoop     = new Thread(new ThreadStart(SebSocketServerLoop));
                threadExisting = true;
                threadRunning  = false;
                DebugOutputLine(debugMode, "      Leave SebWindowsService::threadLoop = new Thread()");
            }

            // Start the server loop thread

            DebugOutputLine(debugMode, "   threadLoop     = " + threadLoop);
            DebugOutputLine(debugMode, "   threadExisting = " + threadExisting);
            DebugOutputLine(debugMode, "   threadRunning  = " + threadRunning);

          //if (threadRunning == false)
            if (threadLoop != null)
            {
                DebugOutputLine(debugMode, "      Enter SebWindowsService::threadLoop.Start()");
                threadLoop.Start();
                threadExisting = true;
                threadRunning  = true;
                DebugOutputLine(debugMode, "      Leave SebWindowsService::threadLoop.Start()");
            }

            DebugOutputLine(debugMode, "   threadLoop     = " + threadLoop);
            DebugOutputLine(debugMode, "   threadExisting = " + threadExisting);
            DebugOutputLine(debugMode, "   threadRunning  = " + threadRunning);

            DebugOutputLine(debugMode, "Leave SebWindowsService::OnStart()");
            DebugOutputLine(debugMode, "");
        }



        protected override void OnStop()
        {
            DebugOutputLine(debugMode, "");
            DebugOutputLine(debugMode, "Enter SebWindowsService::OnStop()");

            // Abort/Finish the server loop thread

            DebugOutputLine(debugMode, "   threadLoop     = " + threadLoop);
            DebugOutputLine(debugMode, "   threadExisting = " + threadExisting);
            DebugOutputLine(debugMode, "   threadRunning  = " + threadRunning);

          //if (threadExisting == true)
            if (threadLoop != null)
            {
                DebugOutputLine(debugMode, "      Enter SebWindowsService::threadLoop.Abort()");
                threadLoop.Abort();
                threadExisting = false;
                threadRunning  = false;
                DebugOutputLine(debugMode, "      Leave SebWindowsService::threadLoop.Abort()");
            }

            DebugOutputLine(debugMode, "   threadLoop     = " + threadLoop);
            DebugOutputLine(debugMode, "   threadExisting = " + threadExisting);
            DebugOutputLine(debugMode, "   threadRunning  = " + threadRunning);

            DebugOutputLine(debugMode, "Leave SebWindowsService::OnStop()");
            DebugOutputLine(debugMode, "");
        }



        protected override void OnPause()
        {
            DebugOutputLine(debugMode, "");
            DebugOutputLine(debugMode, "Enter SebWindowsService::OnPause()");

            // Pause/Suspend the server loop thread

            DebugOutputLine(debugMode, "   threadLoop     = " + threadLoop);
            DebugOutputLine(debugMode, "   threadExisting = " + threadExisting);
            DebugOutputLine(debugMode, "   threadRunning  = " + threadRunning);

            if (threadLoop != null)
            {
                DebugOutputLine(debugMode, "      Enter SebWindowsService::threadLoop.Suspend()");
                threadLoop.Suspend();
                threadExisting = true;
                threadRunning  = false;
                DebugOutputLine(debugMode, "      Leave SebWindowsService::threadLoop.Suspend()");
            }

            DebugOutputLine(debugMode, "   threadLoop     = " + threadLoop);
            DebugOutputLine(debugMode, "   threadExisting = " + threadExisting);
            DebugOutputLine(debugMode, "   threadRunning  = " + threadRunning);

            DebugOutputLine(debugMode, "Leave SebWindowsService::OnPause()");
            DebugOutputLine(debugMode, "");
        }



        protected override void OnContinue()
        {
            DebugOutputLine(debugMode, "");
            DebugOutputLine(debugMode, "Enter SebWindowsService::OnContinue()");

            // Continue/Resume the server loop thread

            DebugOutputLine(debugMode, "   threadLoop     = " + threadLoop);
            DebugOutputLine(debugMode, "   threadExisting = " + threadExisting);
            DebugOutputLine(debugMode, "   threadRunning  = " + threadRunning);

            if (threadLoop != null)
            {
                DebugOutputLine(debugMode, "      Enter SebWindowsService::threadLoop.Resume()");
                threadLoop.Resume();
                threadExisting = true;
                threadRunning  = true;
                DebugOutputLine(debugMode, "      Leave SebWindowsService::threadLoop.Resume()");
            }

            DebugOutputLine(debugMode, "   threadLoop     = " + threadLoop);
            DebugOutputLine(debugMode, "   threadExisting = " + threadExisting);
            DebugOutputLine(debugMode, "   threadRunning  = " + threadRunning);

            DebugOutputLine(debugMode, "Leave SebWindowsService::OnContinue()");
            DebugOutputLine(debugMode, "");
        }



        protected override void OnShutdown()
        {
            DebugOutputLine(debugMode, "");
            DebugOutputLine(debugMode, "Enter SebWindowsService::OnShutdown()");

            // Abort/Finish the server loop thread

            DebugOutputLine(debugMode, "   threadLoop     = " + threadLoop);
            DebugOutputLine(debugMode, "   threadExisting = " + threadExisting);
            DebugOutputLine(debugMode, "   threadRunning  = " + threadRunning);

          //if (threadExisting == true)
            if (threadLoop != null)
            {
                DebugOutputLine(debugMode, "      Enter SebWindowsService::threadLoop.Abort()");
                threadLoop.Abort();
                threadExisting = false;
                threadRunning  = false;
                DebugOutputLine(debugMode, "      Leave SebWindowsService::threadLoop.Abort()");
            }

            DebugOutputLine(debugMode, "   threadLoop     = " + threadLoop);
            DebugOutputLine(debugMode, "   threadExisting = " + threadExisting);
            DebugOutputLine(debugMode, "   threadRunning  = " + threadRunning);

            DebugOutputLine(debugMode, "Leave SebWindowsService::OnShutdown()");
            DebugOutputLine(debugMode, "");
        }

    } // end class SebWindowsService
} // end namespace SebWindowsService
