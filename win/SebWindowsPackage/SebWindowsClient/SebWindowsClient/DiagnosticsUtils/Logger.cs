// -------------------------------------------------------------
//     Viktor tomas
//     BFH-TI, http://www.ti.bfh.ch
//     Biel, 2012
// -------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security;
using System.IO;
using SebWindowsClient.ConfigurationUtils;

namespace SebWindowsClient.DiagnosticsUtils
{
    /// ----------------------------------------------------------------------------------------
    /// <summary>
    /// Manage logging of events.
    /// </summary>
    /// ----------------------------------------------------------------------------------------
    public static class Logger
    {

        //private static FileStream _logFile = null;
        private static StreamWriter _sw = null;

        /// ----------------------------------------------------------------------------------------
        /// <summary>
        /// Open Logger.
        /// </summary>
        /// ----------------------------------------------------------------------------------------
        public static bool initLogger(string logFileDirectory, string logFilePath)
        {
            try
            {
                if (_sw == null)
                {
                    //_logFile = new FileStream(logFile, FileMode.OpenOrCreate);
                    if (File.Exists(logFileDirectory) == false)
                        File.Create(logFileDirectory);

                    _sw = new StreamWriter(logFilePath, true);
                }
                return true;
            }
            catch (Exception ex)
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
        public static void closeLoger()
        {
            try
            {
                if (_sw != null)
                {
                    //_logFile = new FileStream(logFile, FileMode.OpenOrCreate);
                    _sw.Close();
                    _sw = null;
                }
            }
            catch (Exception ex)
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
            string machineName = System.Environment.MachineName;
            if (!eventDateTime.HasValue) eventDateTime = DateTime.Now;

            StringBuilder logEntry = new StringBuilder("Event code: ");
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

            if (_sw != null)
            {

                _sw.WriteLine(logEntry);

                //_sw.Close();
            }
            else
            {
                Console.WriteLine(logEntry);
            }

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
            Insert(SEBGlobalConstants.ERROR, eventSource == null ? null : eventSource.ToString(), 0, message, exception == null ? null : exception.GetType().ToString(), details);
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
            Insert(SEBGlobalConstants.WARNING, eventSource == null ? null : eventSource.ToString(), 0, message, exception == null ? null : exception.GetType().ToString(), details);
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
        public static void AddInformation(string message, object eventSource, Exception exception, string details = null)
        {
            Insert(SEBGlobalConstants.INFORMATION, eventSource == null ? null : eventSource.ToString(), 0, message, exception == null ? null : exception.GetType().ToString(), details);
        }

    }
}
