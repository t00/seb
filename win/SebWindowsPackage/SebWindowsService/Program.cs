using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace SebWindowsService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
			{ 
				new SebWindowsService() 
			};
            ServiceBase.Run(ServicesToRun);
/*
            DebugOutputLine(true, "");
            DebugOutputLine(true, "Enter Program::Main()");
            DebugOutputLine(true, "Leave Program::Main()");
            DebugOutputLine(true, "");
*/
        }
    }
}
