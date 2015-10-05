using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace SebWindowsServiceWCF.ServiceImplementations
{
    /// <summary>
    /// Static implementation of a file logger
    /// </summary>
    public static class Logger
    {
        /// <summary>
        /// Logg the content of the exception together with a message
        /// </summary>
        /// <param name="ex">Exception</param>
        /// <param name="content">Message</param>
        public static void Log(Exception ex, string content)
        {
			Log(string.Format("{3} {0}: {1}{4}{2}", ex.Message, content, ex.StackTrace, ex, Environment.NewLine));
        }

        /// <summary>
        /// Log a message
        /// </summary>
        /// <param name="content">Message</param>
        public static void Log(string content)
        {
	        try
	        {
		        File.AppendAllText(LogFilePath, string.Format("{0:O}: {1}{2}", DateTime.Now.ToLocalTime(), content, Environment.NewLine));
	        }
	        catch
	        {
				EventLog.WriteEntry("SebWindowsServiceWCF", content);
			}
        }

		//The logfile is stored where the executable of the service is
		private static readonly string LogFilePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "sebwindowsservice.log");
    }
}
