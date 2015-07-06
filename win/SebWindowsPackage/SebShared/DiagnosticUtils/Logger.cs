// -------------------------------------------------------------
//     Viktor tomas
//     BFH-TI, http://www.ti.bfh.ch
//     Biel, 2012
// -------------------------------------------------------------

using System;
using System.IO;
using System.Text;

namespace SebShared.DiagnosticUtils
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Manage logging of events.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public static class Logger
	{
		//private static FileStream _logFile = null;
		private static StreamWriter logWriter;

		/// ----------------------------------------------------------------------------------------
		/// <summary>
		/// Open Logger.
		/// </summary>
		/// ----------------------------------------------------------------------------------------
		public static bool InitLogger(string logFileDirectory = null, string logFilePath = null)
		{
			if(String.IsNullOrEmpty(logFileDirectory))
			{
				logFileDirectory = String.Format(@"{0}\{1}", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), SebConstants.MANUFACTURER_LOCAL);
			}
			if(String.IsNullOrEmpty(logFilePath))
			{
				logFilePath = String.Format(@"{0}\{1}", logFileDirectory, SebConstants.SEB_CLIENT_LOG);
			}
			try
			{
				if(logWriter == null)
				{
					//_logFile = new FileStream(logFile, FileMode.OpenOrCreate);
					if(!Directory.Exists(logFileDirectory))
					{
						Directory.CreateDirectory(logFileDirectory);
					}

					logWriter = new StreamWriter(logFilePath, true);
				}
				return true;
			}
			catch(Exception ex)
			{
				Console.WriteLine(ex.ToString());
				return false;
			}

		}

		/// ----------------------------------------------------------------------------------------
		/// <summary>
		/// Close Logger.
		/// </summary>
		/// ----------------------------------------------------------------------------------------
		public static void CloseLogger()
		{
			try
			{
				if(logWriter != null)
				{
					//_logFile = new FileStream(logFile, FileMode.OpenOrCreate);
					logWriter.Close();
					logWriter = null;
				}
			}
			catch(Exception ex)
			{
				Console.WriteLine(ex.ToString());
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Insert an entry in the log file.
		/// </summary>
		/// <param name="eventCode">The event's code.</param>
		/// <param name="eventType">The event's type.</param>
		/// <param name="eventDetailCode">The event's detailed code.</param>
		/// <param name="message">The event's message.</param>
		/// <param name="exceptionType">The type of the exception.</param>
		/// <param name="details">A detailed description.</param>
		/// <param name="eventDateTime">The date of the event.</param>
		/// <param name="additionalData">Some additional data in XML format.</param>
		/// ------------------------------------------------------------------------------------
		private static void Insert(int eventCode, string eventType, int eventDetailCode, string message, string exceptionType,
			string details, DateTime? eventDateTime = null, string additionalData = null)
		{
			InitLogger();
			if(!eventDateTime.HasValue) eventDateTime = DateTime.Now;

			var logEntry = new StringBuilder("Event code: ");
			logEntry.Append(eventCode);
			logEntry.Append(" Event type: ");
			logEntry.Append(eventType);
			logEntry.Append(" Event detail code: ");
			logEntry.Append(eventDetailCode);
			logEntry.Append(" Message: ");
			logEntry.Append(message);
			logEntry.Append(" Exception type: ");
			logEntry.Append(exceptionType);
			logEntry.Append(" Details: ");
			logEntry.Append(details);
			logEntry.Append(" Event date: ");
			logEntry.Append(eventDateTime);
			logEntry.Append(" Additional data: ");
			logEntry.Append(additionalData);

			if(logWriter != null)
			{

				logWriter.WriteLine(logEntry);

				//_sw.Close();
			}
			else
			{
				Console.WriteLine(logEntry);
			}
			CloseLogger();
		}


		/// ---------------------------------------------------------------------------------------
		/// <summary>
		/// Raise a generic error event .
		/// </summary>
		/// <param name="message">The error's message.</param>
		/// <param name="eventSource">The object that is the source of the event.</param>
		/// <param name="exception">The exception.</param>
		/// <param name="details">Optional details about the error.</param>
		/// ---------------------------------------------------------------------------------------
		public static void AddError(string message, object eventSource, Exception exception, string details = null)
		{
			// AddGeneric(message, eventSource, VSDEventCode.GenericError, exception, details);
			Insert(SebConstants.ERROR, eventSource == null ? null : eventSource.ToString(), 0, message, exception == null ? null : exception.GetType().ToString(), details);
		}

		/// ---------------------------------------------------------------------------------------
		/// <summary>
		/// Raise a generic warning event.
		/// </summary>
		/// <param name="message">The error's message.</param>
		/// <param name="eventSource">The object that is the source of the event.</param>
		/// <param name="exception">The exception.</param>
		/// <param name="details">Optional details about the error.</param>
		/// ---------------------------------------------------------------------------------------
		public static void AddWarning(string message, object eventSource, Exception exception, string details = null)
		{
			// AddGeneric(message, eventSource, VSDEventCode.GenericWarning, exception, details);
			Insert(SebConstants.WARNING, eventSource == null ? null : eventSource.ToString(), 0, message, exception == null ? null : exception.GetType().ToString(), details);
		}

		/// ---------------------------------------------------------------------------------------
		/// <summary>
		/// Raise a generic information event.
		/// </summary>
		/// <param name="message">The error's message.</param>
		/// <param name="eventSource">The object that is the source of the event.</param>
		/// <param name="exception">The exception.</param>
		/// <param name="details">Optional details about the error.</param>
		/// ---------------------------------------------------------------------------------------
		public static void AddInformation(string message, object eventSource = null, Exception exception = null, string details = null)
		{
			Insert(SebConstants.INFORMATION, eventSource == null ? null : eventSource.ToString(), 0, message, exception == null ? null : exception.GetType().ToString(), details);
		}
	}
}
