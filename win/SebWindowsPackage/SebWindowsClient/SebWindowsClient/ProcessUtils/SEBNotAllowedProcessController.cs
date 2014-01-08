// -------------------------------------------------------------
//     Viktor tomas
//     BFH-TI, http://www.ti.bfh.ch
//     Biel, 2012
// -------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Collections;
using System.Management;
using System.Threading;
using SebWindowsClient.DiagnosticsUtils;

namespace SebWindowsClient.ProcessUtils
{
    public class SEBNotAllowedProcessController
    {
        /// <summary>
        /// Check if a process is running
        /// </summary>
        /// <param name="processname">the processname</param>
        /// <returns>true if the process runns otherwise false</returns>
        public static bool CheckIfAProcessIsRunning(string processname)
        {
            return System.Diagnostics.Process.GetProcessesByName(processname).Length > 0;
        }

        /// <summary>
        /// Gets process owner.
        /// </summary>
        /// <returns></returns>
        public static string getLocalProcessOwner(int pid)
        {
            string ProcessOwner = "";
            ObjectQuery x = new ObjectQuery("Select * From Win32_Process where Handle='" + pid + "'");
            ManagementObjectSearcher mos = new ManagementObjectSearcher(x);
            foreach (ManagementObject mo in mos.Get())
            {
                string[] s = new string[2];
                mo.InvokeMethod("GetOwner", (object[])s);
                ProcessOwner = s[0].ToString();
                break;
            }

            return ProcessOwner;
        }

        /// <summary>
        /// Closes process by process name.
        /// </summary>
        /// <returns></returns>
        public static void CloseProcessByName(string nameToClosse)
        {
            Process[] processes = System.Diagnostics.Process.GetProcesses();
            foreach (System.Diagnostics.Process processToClosse in processes)
            {
                if (processToClosse.ProcessName == nameToClosse)
                {
                    try
                    {
                        // Display physical memory usage 5 times at intervals of 2 seconds.
                        for (int i = 0; i < 5; i++)
                        {
                            if (!processToClosse.HasExited)
                            {
                                // Discard cached information about the process.
                                processToClosse.Refresh();
                                // Print working set to console.
                                Console.WriteLine("Physical Memory Usage: "
                                                        + processToClosse.WorkingSet64.ToString());
                                // Wait 2 seconds.
                                Thread.Sleep(2000);
                            }
                            else
                            {
                                break;
                            }
                        }

                        // Close process by sending a close message to its main window.
                        processToClosse.CloseMainWindow();
                        // Free resources associated with process.
                        processToClosse.Close();

                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("The following exception was raised: ");
                        Console.WriteLine(e.Message);
                    }
                }
            }
        }

        /// <summary>
        /// Closes process by process name.
        /// </summary>
        /// <returns></returns>
        public static void CloseProcess(Process processToClose)
        {
                    try
                    {
                        //// Display physical memory usage 5 times at intervals of 2 seconds.
                        //for (int i = 0; i < 5; i++)
                        //{
                        //    if (!processToClose.HasExited)
                        //    {
                        //        // Discard cached information about the process.
                        //        processToClose.Refresh();
                        //        // Print working set to console.
                        //        Console.WriteLine("Physical Memory Usage: "
                        //                                + processToClose.WorkingSet64.ToString());
                        //        // Wait 2 seconds.
                        //        Thread.Sleep(2000);
                        //    }
                        //    else
                        //    {
                        //        break;
                        //    }
                        //}

                        if (!processToClose.HasExited)
                        {
                            // Close process by sending a close message to its main window.
                            processToClose.CloseMainWindow();
                            // Free resources associated with process.
                            processToClose.Close();
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("The following exception was raised: ");
                        Console.WriteLine(e.Message);
                    }
        }

        /// <summary>
        /// Gets all processes.
        /// </summary>
        /// <returns></returns>
        public static Hashtable GetProcesses()
        {
            Hashtable ht = new Hashtable();
            Process[] processes = System.Diagnostics.Process.GetProcesses();
            foreach (System.Diagnostics.Process process in processes)
                ht.Add(Convert.ToInt32(process.Id), process.ProcessName);
            return ht;
        }

        /// <summary>
        /// Kills the process by name.
        /// </summary>
        /// <param name="nameToKill">The process name.</param>
        public static void KillProcessByName(string nameToKill)
        {
            try
            {
                Process[] processes = System.Diagnostics.Process.GetProcesses();
                foreach (System.Diagnostics.Process process in processes)
                    if (process.ProcessName == nameToKill)
                        process.Kill();
            }
            catch (Exception ex)
            {
                Logger.AddError("Error when killing process", null, ex);
            }
        }

        /// <summary>
        /// Kills the process by id.
        /// </summary>
        /// <param name="idToKill">The process Id.</param>
        public static void KillProcessById(int idToKill)
        {
            Process[] processes = System.Diagnostics.Process.GetProcesses();
            foreach (System.Diagnostics.Process process in processes)
                if (process.Id == idToKill)
                    process.Kill();
        }


    }
}
