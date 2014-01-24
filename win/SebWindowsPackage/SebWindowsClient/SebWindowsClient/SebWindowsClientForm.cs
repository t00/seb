using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SebWindowsClient.ConfigurationUtils;
using SebWindowsClient.DiagnosticsUtils;
using SebWindowsClient.DesktopUtils;
using SebWindowsClient.ClientSocketUtils;
using System.Net;
using System.IO;
using System.Security.Principal;
using SebWindowsClient.RegistryUtils;
using SebWindowsClient.ProcessUtils;
using SebWindowsClient.BlockShortcutsUtils;
using System.Runtime.InteropServices;
using System.Diagnostics;
using SebWindowsClient.CryptographyUtils;
using SebWindowsClient.ServiceUtils;
using DictObj = System.Collections.Generic.Dictionary<string, object>;


// -------------------------------------------------------------
//     Viktor tomas
//     BFH-TI, http://www.ti.bfh.ch
//     Biel, 2012
// -------------------------------------------------------------
namespace SebWindowsClient
{
    

    public partial class SebWindowsClientForm : Form
    {
        [DllImport("user32.dll")]
        private static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        [DllImport("user32.dll")]
        static extern IntPtr GetDesktopWindow();

         [DllImport("user32.dll")]
        private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X,
           int Y, int cx, int cy, uint uFlags);

        [DllImport("user32.dll")]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern bool EnumThreadWindows(int threadId, EnumThreadProc pfnEnum, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern System.IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr FindWindowEx(IntPtr parentHandle, IntPtr childAfter, string className, string windowTitle);

        [DllImport("user32.dll")]
        private static extern IntPtr FindWindowEx(IntPtr parentHwnd, IntPtr childAfterHwnd, IntPtr className, string windowText);

        [DllImport("User32.dll")]
        private static extern bool IsIconic(IntPtr handle);

        [DllImport("user32.dll")]
        private static extern int ShowWindow(IntPtr hwnd, int nCmdShow);

        [DllImportAttribute("User32.dll")]
        private static extern IntPtr SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern uint GetWindowThreadProcessId(IntPtr hwnd, out int lpdwProcessId);

        //[System.Runtime.InteropServices.DllImport("User32")]
        //private static extern bool SetForegroundWindow(IntPtr hWnd);


        private const int SW_HIDE = 0;
        private const int SW_SHOW = 5;
        private const int SW_RESTORE = 9;

        private const string VistaStartMenuCaption = "Start";
        private static IntPtr vistaStartMenuWnd = IntPtr.Zero;
        private delegate bool EnumThreadProc(IntPtr hwnd, IntPtr lParam);

        private SebSocketClient sebSocketClient = new SebSocketClient();
        //private SebApplicationChooserForm SebApplicationChooser = null;

        public bool closeSebClient = true;
        public string sebPassword = null;

        private SebCloseDialogForm sebCloseDialogForm;
        private SebApplicationChooserForm sebApplicationChooserForm;

        public Process xulRunner = new Process();
        private int xulRunnerExitCode;
        private DateTime xulRunnerExitTime;
        //private bool xulRunnerExitEventHandled;

        public List<string> permittedProcessesCalls = new List<string>();
        public List<Process> permittedProcessesReferences = new List<Process>();

        //private System.Windows.Forms.OpenFileDialog openFileDialog;

        /// ----------------------------------------------------------------------------------------
        /// <summary>
        /// Constructor - initialise components.
        /// </summary>
        /// ----------------------------------------------------------------------------------------
        public SebWindowsClientForm()
        {
            InitializeComponent();
        }

        /// ----------------------------------------------------------------------------------------
        /// <summary>
        /// OnLoad: Get the file name from command line arguments and load it.
        /// </summary>
        /// ----------------------------------------------------------------------------------------
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            string[] args = Environment.GetCommandLineArgs();
            if (args.Length > 1)
            {
                LoadFile(args[1]);
            }
        }
        public void LoadFile(string file)
        {
            byte[] sebSettings = null;
            Uri uri;
            try
            {
                uri = new Uri(file);
            }
            catch (Exception ex)
            {
                Logger.AddError("SEB was opened with a wrong parameter", this, ex, ex.Message); 
                return;
            }
            // Check if we're running in exam mode already, if yes, then refuse to load a .seb file
            if (SEBClientInfo.examMode)
            {
                SEBClientInfo.SebWindowsClientForm.Activate();
                SEBErrorMessages.OutputErrorMessageNew(SEBUIStrings.loadingSettingsNotAllowed, SEBUIStrings.loadingSettingsNotAllowedReason, SEBGlobalConstants.IND_MESSAGE_KIND_ERROR, MessageBoxButtons.OK);
                return;
            }

            if (uri.Scheme == "seb")
            {
                // The URI is holding a seb:// web address for a .seb settings file: download it
                WebClient myWebClient = new WebClient();
                // Try first by http
                UriBuilder httpURL = new UriBuilder("http", uri.Host, uri.Port, uri.AbsolutePath);
                using (myWebClient)
                {
                    sebSettings = myWebClient.DownloadData(httpURL.Uri);
                }
                if (sebSettings == null)
                {
                    // Nothing got downloaded: Try by https
                    UriBuilder httpsURL = new UriBuilder("https", uri.Host, uri.Port, uri.AbsolutePath);
                    using (myWebClient)
                    {
                        sebSettings = myWebClient.DownloadData(httpsURL.Uri);
                    }
                }
            }
            else if (uri.IsFile)
            {
                try
                {
                    sebSettings = File.ReadAllBytes(file);
                }
                catch (Exception streamReadException)
                {
                    // Write error into string with temporary log string builder
                    Logger.AddError("Settings could not be read from file.", this, streamReadException, streamReadException.Message);
                    return;
                }
            }
            // If some settings got loaded in the end
            if (sebSettings == null) return;

            // Decrypt, parse and store new settings and restart SEB if this was successfull
            SEBConfigFileManager.StoreDecryptedSEBSettings(sebSettings);
        }

        /// ----------------------------------------------------------------------------------------
        /// <summary>
        // Start xulRunner process.
        /// </summary>
        /// ----------------------------------------------------------------------------------------
        private bool StartXulRunner()
        {

//            xulRunnerExitEventHandled = false;
            string xulRunnerPath = "";
            string desktopName = "";
            try
            {
                string XULRunnerParameters = SEBXulRunnerSettings.XULRunnerConfigDictionarySerialize(SEBSettings.settingsCurrent);
                // Create JSON object with XULRunner parameters to pass to xulrunner.exe as base64 string
                StringBuilder xulRunnerPathBuilder = new StringBuilder(SEBClientInfo.XulRunnerExePath);
                //StringBuilder xulRunnerArgumentsBuilder = new StringBuilder(" -app ").Append(SEBClientInfo.XulRunnerSebIniPath).
                //Append(" -configpath ").Append(SEBClientInfo.XulRunnerConfigFile);.Append(Application.StartupPath).Append(".\\").Append(SEBClientInfo.XulRunnerSebIniPath)
                StringBuilder xulRunnerArgumentsBuilder = new StringBuilder(" -app \"").Append(Application.StartupPath).Append(".\\").Append(SEBClientInfo.XulRunnerSebIniPath).Append("\" -profile \"").Append(SEBClientInfo.SebClientSettingsLocalAppDirectory).Append("Profiles\""). //Append("-debug 1 -purgecaches -jsconsole"). //" -config \"winctrl\" 
                    Append(" -ctrl \"").Append(XULRunnerParameters).Append("\"");
                //StringBuilder xulRunnerArgumentsBuilder = new StringBuilder(" -app ").Append(SEBClientInfo.XulRunnerSebIniPath).Append(" -ctlr 1 ");
                string xulRunnerArguments = xulRunnerArgumentsBuilder.ToString();
                xulRunnerPathBuilder.Append(xulRunnerArguments);
                xulRunnerPath = xulRunnerPathBuilder.ToString();

                //string path = SEBClientInfo.XulRunnerExePath;
                //path = path + " -app \"C:\\Program Files (x86)\\ETH Zuerich\\SEB Windows 1.9.1\\SebWindowsClient\\xulseb\\seb.ini\" -configpath  \"C:\\Users\\viktor\\AppData\\Local\\ETH_Zuerich\\config.json\"";
                desktopName = SEBClientInfo.DesktopName;
                xulRunner = SEBDesktopController.CreateProcess(xulRunnerPath, desktopName);
                //permittedProcessesReferences.Add(xulRunner);
                //xulRunner.StartInfo.FileName = "\"C:\\Program Files (x86)\\ETH Zuerich\\SEB Windows 1.9.1\\SebWindowsClient\\xulrunner\\xulrunner.exe\"";
                ////xulRunner.StartInfo.Verb = "XulRunner";
                //xulRunner.StartInfo.Arguments = " -app \"C:\\Program Files (x86)\\ETH Zuerich\\SEB Windows 1.9.1\\SebWindowsClient\\xulseb\\seb.ini\" -configpath  \"C:\\Users\\viktor\\AppData\\Local\\ETH_Zuerich\\config.json\"";
                //xulRunner.StartInfo.CreateNoWindow = true;
                xulRunner.EnableRaisingEvents = true;
                xulRunner.Exited += new EventHandler(xulRunner_Exited);
                //xulRunner.Start();
                return true;

            }
            catch (Exception ex)
            {
                Logger.AddError("An error occurred starting XULRunner, path: "+xulRunnerPath+" desktop name: "+desktopName+" ", this, ex, ex.Message);
                return false;
            }
        }

        /// ----------------------------------------------------------------------------------------
        /// <summary>
        /// Handle xulRunner_Exited event and display process information.
        /// </summary>
        /// ----------------------------------------------------------------------------------------
        private void xulRunner_Exited(object sender, System.EventArgs e)
        {

            //xulRunnerExitEventHandled = true;
            xulRunnerExitCode = xulRunner.ExitCode;
            xulRunnerExitTime = xulRunner.ExitTime;
            if (xulRunnerExitCode != 0)
            {
                // An error occured when exiting XULRunner, maybe it crashed?
                Logger.AddError("An error occurred when exiting XULRunner. Exit code: " + xulRunnerExitCode.ToString(), this, null);
                // Restart XULRunner
                //StartXulRunner();
            }
            else
            {
                // If the flag for closing SEB is set, we exit
                if (SEBClientInfo.SebWindowsClientForm.closeSebClient) Application.Exit();
            }

        }

        /// ----------------------------------------------------------------------------------------
        /// <summary>
        /// Add permitted process names and icons to the SEB task bar (ToolStrip control) 
        /// and start permitted processes which have the autostart option set 
        /// </summary>
        /// ----------------------------------------------------------------------------------------
        private void addPermittedProcessesToTS()
        {
            // First clear the permitted processes toolstrip/lists in case of a SEB restart
            taskbarToolStrip.Items.Clear();
            permittedProcessesCalls.Clear();
            permittedProcessesReferences.Clear();

            List<object> permittedProcessList = (List<object>)SEBClientInfo.getSebSetting(SEBSettings.KeyPermittedProcesses)[SEBSettings.KeyPermittedProcesses];
            if (permittedProcessList.Count > 0)
            {
                // Check if the permitted third party applications are already running
                Process[] runningApplications;
                List<Process> runningProcessesToClose = new List<Process>();
                List<string> runningApplicationsToClose = new List<string>();
                for (int i = 0; i < permittedProcessList.Count; i++)
                {
                    Dictionary<string, object> permittedProcess = (Dictionary<string, object>)permittedProcessList[i];
                    SEBSettings.operatingSystems permittedProcessOS = (SEBSettings.operatingSystems)SEBSettings.valueForDictionaryKey(permittedProcess, SEBSettings.KeyOS);
                    bool permittedProcessActive = (bool)SEBSettings.valueForDictionaryKey(permittedProcess, SEBSettings.KeyActive);
                    if (permittedProcessOS == SEBSettings.operatingSystems.operatingSystemWin && permittedProcessActive)
                    {
                        string title = (string)SEBSettings.valueForDictionaryKey(permittedProcess, SEBSettings.KeyTitle);
                        if (title == null) title = "";
                        string executable = (string)permittedProcess[SEBSettings.KeyExecutable];
                        // Check if the process is already running
                        runningApplications = Process.GetProcesses();
                        for (int j = 0; j < runningApplications.Count(); j++)
                        {
                            if (executable.Contains(runningApplications[j].ProcessName))
                            {
                                // If the flag strongKill is set, then the process is killed without asking the user
                                bool strongKill = (bool)SEBSettings.valueForDictionaryKey(permittedProcess, SEBSettings.KeyStrongKill);
                                if (strongKill)
                                    {
                                        SEBNotAllowedProcessController.CloseProcess(runningApplications[j]);
                                    }
                                    else
                                    {
                                        runningProcessesToClose.Add(runningApplications[j]);
                                        runningApplicationsToClose.Add(title == "SEB" ? executable : title);
                                        //runningApplicationsToClose.Add((title == "SEB" ? "" : (title == "" ? "" : title + " - ")) + executable);
                                    }
                            }
                        }
                    }
                }
                // If we found already running permitted processes, we ask the user how to quit them
                if (runningProcessesToClose.Count > 0)
                {
                    StringBuilder applicationsListToClose = new StringBuilder();
                    foreach (string applicationToClose in runningApplicationsToClose)
                    {
                        applicationsListToClose.AppendLine("    " + applicationToClose);
                    }
                    if (SEBErrorMessages.OutputErrorMessageNew(SEBUIStrings.closeProcesses, SEBUIStrings.closeProcessesQuestion + "\n\n" + applicationsListToClose.ToString(), SEBGlobalConstants.IND_MESSAGE_KIND_QUESTION, MessageBoxButtons.OKCancel))
                    {
                        foreach (Process processToClose in runningProcessesToClose)
                        {
                            SEBNotAllowedProcessController.CloseProcess(processToClose);
                        }
                    }
                    else
                    {
                        Application.Exit();
                        return;
                    }
                }


                for (int i = 0; i < permittedProcessList.Count; i++)
                {
                    ToolStripButton toolStripButton = new ToolStripButton();
                    Dictionary<string, object> permittedProcess = (Dictionary<string, object>)permittedProcessList[i];
                    SEBSettings.operatingSystems permittedProcessOS = (SEBSettings.operatingSystems)SEBSettings.valueForDictionaryKey(permittedProcess, SEBSettings.KeyOS);
                    bool permittedProcessActive = (bool)SEBSettings.valueForDictionaryKey(permittedProcess, SEBSettings.KeyActive);
                    //if (permittedProcessActive == null) permittedProcessActive = false;
                    if (permittedProcessOS == SEBSettings.operatingSystems.operatingSystemWin && permittedProcessActive)
                    {
                        string title = (string)SEBSettings.valueForDictionaryKey(permittedProcess, SEBSettings.KeyTitle);
                        if (title == null) title = "";
                        string executable = (string)permittedProcess[SEBSettings.KeyExecutable];
                        toolStripButton.Padding = new Padding(5, 0, 5, 0);
                        toolStripButton.ToolTipText = title;
                        Icon processIcon = null;
                        string fullPath;
                        if (executable.Contains("xulrunner.exe"))
                            fullPath = Application.ExecutablePath;
                        else
                        {
                            //fullPath = GetApplicationPath(executable);
                            fullPath = GetPermittedApplicationPath(permittedProcess);
                        }
                        // Continue only if the application has been found
                        if (fullPath != null)
                        {
                            processIcon = GetApplicationIcon(fullPath);
                            // If the icon couldn't be read, we try it again
                            if (processIcon == null) processIcon = GetApplicationIcon(fullPath);
                            // If it again didn't work out, we try to take the icon of SEB
                            if (processIcon == null) processIcon = GetApplicationIcon(Application.ExecutablePath);
                            if (processIcon != null) toolStripButton.Image = processIcon.ToBitmap();

                            toolStripButton.Click += new EventHandler(ToolStripButton_Click);

                            // We save the index of the permitted process to the toolStripButton.Name property
                            toolStripButton.Name = permittedProcessesCalls.Count.ToString();

                            taskbarToolStrip.Items.Add(toolStripButton);

                            //toolStripButton.Checked = true;
                            if (!executable.Contains(SEBClientInfo.XUL_RUNNER))
                            {
                                StringBuilder startProcessNameBuilder = new StringBuilder(fullPath);
                                List<object> argumentList = (List<object>)permittedProcess[SEBSettings.KeyArguments];
                                for (int j = 0; j < argumentList.Count; j++)
                                {
                                    Dictionary<string, object> argument = (Dictionary<string, object>)argumentList[j];
                                    if ((Boolean)argument[SEBSettings.KeyActive])
                                    {
                                        startProcessNameBuilder.Append(" ").Append((string)argument[SEBSettings.KeyArgument]);
                                    }
                                }
                                string fullPathArgumentsCall = startProcessNameBuilder.ToString();

                                // Save the full path of the permitted process executable including arguments
                                permittedProcessesCalls.Add(fullPathArgumentsCall);

                                // Autostart processes which have the according flag set
                                Process newProcess = null;
                                if ((Boolean)permittedProcess[SEBSettings.KeyAutostart])
                                {
                                    newProcess = CreateProcessWithExitHandler(fullPathArgumentsCall);
                                }
                                // Save the process reference if the process was started, otherwise null
                                permittedProcessesReferences.Add(newProcess);
                            }
                            else
                            {
                                // Start XULRunner
                                StartXulRunner();
                                // Save the process reference of XULRunner
                                permittedProcessesReferences.Add(xulRunner);
                                // Save an empty path for XULRunner (we don't need the path)
                                permittedProcessesCalls.Add("");
                            }

                        }
                        else
                        {
                            SEBClientInfo.SebWindowsClientForm.Activate();
                            SEBErrorMessages.OutputErrorMessageNew(SEBUIStrings.permittedApplicationNotFound, SEBUIStrings.permittedApplicationNotFoundMessage, SEBGlobalConstants.IND_MESSAGE_KIND_ERROR, MessageBoxButtons.OK, title);
                        }
                    }
                }
            }
            //System.Diagnostics.Process oskProcess = null;
            // create the process.
            //oskProcess = SEBDesktopController.CreateProcess("C:\Program Files\Common Files\Microsoft Shared\ink\TabTip.exe", SEBClientInfo.DesktopName);
            //SEBDesktopController.CreateProcess("C:\\Program Files\\Common Files\\Microsoft Shared\\ink\\TabTip.exe", SEBClientInfo.DesktopName);
            //oskProcess = Process.Start("C:\\Program Files\\Common Files\\Microsoft Shared\\ink\\TabTip.exe");

        }

        /// ----------------------------------------------------------------------------------------
        /// <summary>
        /// Get icon for a running process.
        /// </summary>
        /// ----------------------------------------------------------------------------------------
        private Icon GetProcessIcon(Process process)
        {
            Icon processIcon;
            try
            {
                string processExecutableFileName = process.MainModule.FileName;
                processIcon = Icon.ExtractAssociatedIcon(processExecutableFileName);
            }
            catch (Exception)
            {
                processIcon = null;
            }
            return processIcon;
        }


        /// ----------------------------------------------------------------------------------------
        /// <summary>
        /// Get icon for an application specified by a full path.
        /// </summary>
        /// ----------------------------------------------------------------------------------------
        private Icon GetApplicationIcon(string fullPath)
        {
            Icon processIcon;
            try
            {
                processIcon = Icon.ExtractAssociatedIcon(fullPath);
            }
            catch (Exception)
            {
                processIcon = null;
            }
            return processIcon;
        }


        /// ----------------------------------------------------------------------------------------
        /// <summary>
        /// Get the full path of an application from which we know the executable name 
        /// by searching the application paths which are set in the Registry.
        /// </summary>
        /// ----------------------------------------------------------------------------------------
        public string GetApplicationPath(string appname)
        {
            if (File.Exists(appname)) return appname;
            // Check if file is in programmdir
            string programdirAppname = SEBClientInfo.ProgramFilesX86Directory + "\\" + appname;
            if (File.Exists(programdirAppname)) return programdirAppname;

            using (Microsoft.Win32.RegistryKey key = Microsoft.Win32.RegistryKey.OpenRemoteBaseKey(Microsoft.Win32.RegistryHive.LocalMachine, ""))
            {
                //// Get all paths from the PATH environement variable
                //string RegKeyName = @"SYSTEM\CurrentControlSet\Control\Session Manager\Environment";
                //string pathVariableString = (string)Microsoft.Win32.Registry.LocalMachine.OpenSubKey(RegKeyName).GetValue
                //    ("Path", "", Microsoft.Win32.RegistryValueOptions.DoNotExpandEnvironmentNames);
                //string[] paths = pathVariableString.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                //foreach (string subPath in paths)
                //{
                //    Console.WriteLine(subPath);
                //}

                string subKeyName = @"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\" + appname;
                using (Microsoft.Win32.RegistryKey subkey = key.OpenSubKey(subKeyName))
                {
                    if (subkey == null)
                    {
                        //string expanded = System.Environment.GetEnvironmentVariable("path") + appname;
                        string fullPath = Environment.SystemDirectory + "\\" + appname;
                        if (File.Exists(fullPath)) return fullPath;
                        return null;
                    }

                    object path = subkey.GetValue("Path");

                    if (path != null)
                        return (string)path + "\\" + appname;
                }
            }
            return null;
        }

        /// ----------------------------------------------------------------------------------------
        /// <summary>
        /// Get the full path of an application from which we know the executable name 
        /// by searching the application paths which are set in the Registry.
        /// </summary>
        /// ----------------------------------------------------------------------------------------
        public string GetPermittedApplicationPath(DictObj permittedProcess)
        {
            string executable = (string)SEBSettings.valueForDictionaryKey(permittedProcess, SEBSettings.KeyExecutable);
            if (executable == null) executable = "";
            string executablePath = (string)SEBSettings.valueForDictionaryKey(permittedProcess, SEBSettings.KeyPath);
            if (executablePath == null) executablePath = "";
            bool allowChoosingApp = (bool)SEBSettings.valueForDictionaryKey(permittedProcess, SEBSettings.KeyAllowUser);
            //if (allowChoosingApp == null) allowChoosingApp = false;

            string fullPath;
            // There is a permittedProcess.path value
            if (executablePath != "")
            {
                fullPath = executablePath + "\\" + executable;
                // In case path to the executable's directory + the file name of the executable is already the correct file, we return this full path
                if (File.Exists(fullPath)) return fullPath;
                // Otherwise try to determine the applications full path
                fullPath = GetApplicationPath(fullPath);
                if (fullPath != null) if (File.Exists(fullPath)) return fullPath;
            }
            // There is no permittedProcess.path value:
            // try to find path using just the executable file name
            fullPath = GetApplicationPath(executable);

            // If we still didn't find the application and the setting for this permitted process allows user to find the application
            if (fullPath == null && allowChoosingApp)
            {
                // Ask the user to locate the application
                // Set the default directory and file name in the File Dialog
                // Get the path of the "Program Files X86" directory.
                openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);
                openFileDialog.FileName = executable;
                openFileDialog.Filter = executable + " | " + executable;
                openFileDialog.CheckFileExists = true;
                openFileDialog.CheckPathExists = true;
                openFileDialog.DefaultExt = "exe";
                openFileDialog.Title = SEBUIStrings.locatePermittedApplication;
                //openFileDialog.

                // Get the user inputs in the File Dialog
                DialogResult fileDialogResult = openFileDialog.ShowDialog();
                String fileName = openFileDialog.FileName;

                // If the user clicked "Cancel", do nothing
                // If the user clicked "OK"    , use the third party applications file name and path as the permitted process
                if (fileDialogResult.Equals(DialogResult.Cancel)) return null;
                if (fileDialogResult.Equals(DialogResult.OK)) return fileName;

                return null;
            }
            return fullPath;
        }


        /// ----------------------------------------------------------------------------------------
        /// <summary>
        /// Handle click on permitted process in SEB taskbar: If process isn't running,
        /// it is started, otherwise the click is ignored.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// ----------------------------------------------------------------------------------------
        protected void ToolStripButton_Click(object sender, EventArgs e)
        {
            // identify which button was clicked and perform necessary actions
            ToolStripButton toolStripButton = sender as ToolStripButton;

            int i = Convert.ToInt32(toolStripButton.Name);
            Process processReference = permittedProcessesReferences[i];
            if (processReference == xulRunner)
            {
                try
                {
                    // In case the XULRunner process exited but wasn't closed, this will throw an exception
                    if (xulRunner.HasExited)
                    {
                        StartXulRunner();
                    }
                    else
                    {
                        IntPtr handle = processReference.MainWindowHandle;
                        if (IsIconic(handle)) ShowWindow(handle, SW_RESTORE);
                        SetForegroundWindow(handle);
                    }
                }
                catch (Exception)  // XULRunner wasn't running anymore
                {
                    StartXulRunner();
                }
            }
            else
            {
                try
                {
                    if (processReference == null || processReference.HasExited == true)
                    {
                        string permittedProcessCall = (string)permittedProcessesCalls[i];
                        Process newProcess = CreateProcessWithExitHandler(permittedProcessCall);
                        permittedProcessesReferences[i] = newProcess;
                    }
                    else
                    {
                        IntPtr handle = processReference.MainWindowHandle;
                        if (IsIconic(handle)) ShowWindow(handle, SW_RESTORE);
                        SetForegroundWindow(handle);
                    }
                }
                catch (Exception ex)
                {
                    Logger.AddError("Error when trying to start permitted process by clicking in SEB taskbar: ", null, ex);
                }
            }
            //if (SebKeyCapture.SebApplicationChooser == null)
            //    SebKeyCapture.SebApplicationChooser = new SebApplicationChooserForm();
            //SebKeyCapture.SebApplicationChooser.fillListApplications();
            //SebKeyCapture.SebApplicationChooser.Visible = true;
        }

        /// ----------------------------------------------------------------------------------------
        /// <summary>
        /// Create a new process and add an exited event handler.
        /// </summary>
        /// ----------------------------------------------------------------------------------------
        private Process CreateProcessWithExitHandler(string fullPathArgumentsCall)
        {
            Process newProcess = SEBDesktopController.CreateProcess(fullPathArgumentsCall, SEBClientInfo.DesktopName);
            //newProcess.EnableRaisingEvents = true;
            //newProcess.Exited += new EventHandler(permittedProcess_Exited);

            return newProcess;
        }


        /// ----------------------------------------------------------------------------------------
        /// <summary>
        /// Handle xulRunner_Exited event and display process information.
        /// </summary>
        /// ----------------------------------------------------------------------------------------
        private void permittedProcess_Exited(object sender, System.EventArgs e)
        {
            Process permittedProcess = (Process)sender;
            //int permittedProcessExitCode = permittedProcess.ExitCode;
            //DateTime permittedProcessExitTime = permittedProcess.ExitTime;
            //if (permittedProcessExitCode != 0)
            //{
            //    // An error occured when exiting the permitted process, maybe it crashed?
            //    Logger.AddError("An error occurred when exiting a permitted process. Exit code: " + permittedProcessExitCode.ToString() + ". Exit time: " + permittedProcess.ExitTime.ToString(), this, null);
            //}
        }



        /// ----------------------------------------------------------------------------------------
        /// <summary>
        /// Set form on Desktop.
        /// </summary>
        /// ----------------------------------------------------------------------------------------
        private bool SetFormOnDesktop()
        {

            this.FormBorderStyle = FormBorderStyle.None;
            // sezt das formular auf die Taskbar
            SetParent(this.Handle, GetDesktopWindow());
            //this.BackColor = Color.Red;

            //int height = Screen.PrimaryScreen.Bounds.Height;
            int width = Screen.PrimaryScreen.Bounds.Width;
            int x = 0; //Screen.PrimaryScreen.WorkingArea.Width - this.Width;
            int y = Screen.PrimaryScreen.Bounds.Height - this.Height;

            //this.Height = height;
            this.Width = width;
            this.Location = new Point(x, y);
            this.TopMost = true;
            return true;
        }

        /// <summary>
        /// Hide or show the Windows taskbar and startmenu.
        /// </summary>
        /// <param name="show">true to show, false to hide</param>
        public static void SetVisibility(bool show)
        {
            // get taskbar window
            IntPtr taskBarWnd = FindWindow("Shell_TrayWnd", null);

            // try it the WinXP way first...
            IntPtr startWnd = FindWindowEx(taskBarWnd, IntPtr.Zero, "Button", "Start");

            if (startWnd == IntPtr.Zero)
            {
                // try an alternate way, as mentioned on CodeProject by Earl Waylon Flinn
                startWnd = FindWindowEx(IntPtr.Zero, IntPtr.Zero, (IntPtr)0xC017, "Start");
            }

            if (startWnd == IntPtr.Zero)
            {
                // ok, let's try the Vista easy way...
                startWnd = FindWindow("Button", null);

                if (startWnd == IntPtr.Zero)
                {
                    // no chance, we need to to it the hard way...
                    startWnd = GetVistaStartMenuWnd(taskBarWnd);
                }
            }

            ShowWindow(taskBarWnd, show ? SW_SHOW : SW_HIDE);
            ShowWindow(startWnd, show ? SW_SHOW : SW_HIDE);
        }

        /// <summary>
        /// Returns the window handle of the Vista start menu orb.
        /// </summary>
        /// <param name="taskBarWnd">windo handle of taskbar</param>
        /// <returns>window handle of start menu</returns>
        private static IntPtr GetVistaStartMenuWnd(IntPtr taskBarWnd)
        {
            // get process that owns the taskbar window
            int procId;
            GetWindowThreadProcessId(taskBarWnd, out procId);

            Process p = Process.GetProcessById(procId);
            if (p != null)
            {
                // enumerate all threads of that process...
                foreach (ProcessThread t in p.Threads)
                {
                    EnumThreadWindows(t.Id, MyEnumThreadWindowsProc, IntPtr.Zero);
                }
            }
            return vistaStartMenuWnd;
        }

        /// <summary>
        /// Callback method that is called from 'EnumThreadWindows' in 'GetVistaStartMenuWnd'.
        /// </summary>
        /// <param name="hWnd">window handle</param>
        /// <param name="lParam">parameter</param>
        /// <returns>true to continue enumeration, false to stop it</returns>
        private static bool MyEnumThreadWindowsProc(IntPtr hWnd, IntPtr lParam)
        {
            StringBuilder buffer = new StringBuilder(256);
            if (GetWindowText(hWnd, buffer, buffer.Capacity) > 0)
            {
                Console.WriteLine(buffer);
                if (buffer.ToString() == VistaStartMenuCaption)
                {
                    vistaStartMenuWnd = hWnd;
                    return false;
                }
            }
            return true;
        }

         /// ----------------------------------------------------------------------------------------
        /// <summary>
        /// Switch to current desktop and close app.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// ----------------------------------------------------------------------------------------
        private void btn_Exit_Click(object sender, EventArgs e)
        {
            if ((bool)SEBSettings.settingsCurrent[SEBSettings.KeyAllowQuit] == true)
            {
                ShowCloseDialogForm();
            }
            //if (this.closeSebClient)
            //{
            //    this.Close();
            //}
        }

        /// ----------------------------------------------------------------------------------------
        /// <summary>
        /// Initialise Client Socket.  Send User, UserSid, Registry Flags string to SebWindowsService
        /// </summary>
        /// <returns>true if succeed</returns>
        /// ----------------------------------------------------------------------------------------
        private bool InitClientSocket()
        {
            SEBLocalHostInfo sebLocalHostInfo = new SEBLocalHostInfo();
            IPHostEntry              hostInfo = sebLocalHostInfo.GetHostInfo();
            SecurityIdentifier       userSid  = sebLocalHostInfo.GetSID();
            string                   userName = sebLocalHostInfo.GetUserName();

            // Label with Username
            //lbl_User.Text = "Logged in as: " + userName;

            Logger.AddInformation("HostName: " + hostInfo.HostName + "HostAddress: " + SEBClientInfo.HostIpAddress + "PortNumber: " + SEBClientInfo.PortNumber.ToString(), this, null);

            bool serviceAvailable = SEBWindowsServiceController.ServiceAvailable(SEBClientInfo.SEB_WINDOWS_SERVICE_NAME);
            if (serviceAvailable)
            {
                // Open socket
                bool bSocketConnected = sebSocketClient.OpenSocket(SEBClientInfo.HostIpAddress, SEBClientInfo.PortNumber.ToString());

                if (bSocketConnected)
                {
                    // Set receive timeout
                    if (((Int32)SEBClientInfo.getSebSetting(SEBSettings.KeySebServicePolicy)[SEBSettings.KeySebServicePolicy]) == (Int32)sebServicePolicies.forceSebService)
                    {
                        SEBClientInfo.RecvTimeout = 0;   // timeout "0" means "infinite" in this case !!!
                        Logger.AddInformation("Force Windows Service demanded, therefore socket recvTimeout = infinite", this, null);

                    }
                    else
                    {
                        Logger.AddInformation("Force Windows Service not demanded, therefore socket recvTimeout = " + SEBClientInfo.RecvTimeout, this, null);
                    }

                    bool bSetRecvTimeout = sebSocketClient.SetRecvTimeout(SEBClientInfo.RecvTimeout);

                    //Set registry flags
                    StringBuilder registryFlagsBuilder = new StringBuilder();
                    registryFlagsBuilder.Append((Boolean)SEBClientInfo.getSebSetting(SEBSettings.KeyInsideSebEnableSwitchUser       )[SEBSettings.KeyInsideSebEnableSwitchUser       ] ? 1 : 0);
                    registryFlagsBuilder.Append((Boolean)SEBClientInfo.getSebSetting(SEBSettings.KeyInsideSebEnableLockThisComputer )[SEBSettings.KeyInsideSebEnableLockThisComputer ] ? 1 : 0);
                    registryFlagsBuilder.Append((Boolean)SEBClientInfo.getSebSetting(SEBSettings.KeyInsideSebEnableChangeAPassword  )[SEBSettings.KeyInsideSebEnableChangeAPassword  ] ? 1 : 0);
                    registryFlagsBuilder.Append((Boolean)SEBClientInfo.getSebSetting(SEBSettings.KeyInsideSebEnableStartTaskManager )[SEBSettings.KeyInsideSebEnableStartTaskManager ] ? 1 : 0);
                    registryFlagsBuilder.Append((Boolean)SEBClientInfo.getSebSetting(SEBSettings.KeyInsideSebEnableLogOff           )[SEBSettings.KeyInsideSebEnableLogOff           ] ? 1 : 0);
                    registryFlagsBuilder.Append((Boolean)SEBClientInfo.getSebSetting(SEBSettings.KeyInsideSebEnableShutDown         )[SEBSettings.KeyInsideSebEnableShutDown         ] ? 1 : 0);
                    registryFlagsBuilder.Append((Boolean)SEBClientInfo.getSebSetting(SEBSettings.KeyInsideSebEnableEaseOfAccess     )[SEBSettings.KeyInsideSebEnableEaseOfAccess     ] ? 1 : 0);
                    registryFlagsBuilder.Append((Boolean)SEBClientInfo.getSebSetting(SEBSettings.KeyInsideSebEnableVmWareClientShade)[SEBSettings.KeyInsideSebEnableVmWareClientShade] ? 1 : 0);

                    registryFlagsBuilder.Append((Boolean)SEBClientInfo.getSebSetting(SEBSettings.KeyOutsideSebEnableSwitchUser       )[SEBSettings.KeyOutsideSebEnableSwitchUser       ] ? 1 : 0);
                    registryFlagsBuilder.Append((Boolean)SEBClientInfo.getSebSetting(SEBSettings.KeyOutsideSebEnableLockThisComputer )[SEBSettings.KeyOutsideSebEnableLockThisComputer ] ? 1 : 0);
                    registryFlagsBuilder.Append((Boolean)SEBClientInfo.getSebSetting(SEBSettings.KeyOutsideSebEnableChangeAPassword  )[SEBSettings.KeyOutsideSebEnableChangeAPassword  ] ? 1 : 0);
                    registryFlagsBuilder.Append((Boolean)SEBClientInfo.getSebSetting(SEBSettings.KeyOutsideSebEnableStartTaskManager )[SEBSettings.KeyOutsideSebEnableStartTaskManager ] ? 1 : 0);
                    registryFlagsBuilder.Append((Boolean)SEBClientInfo.getSebSetting(SEBSettings.KeyOutsideSebEnableLogOff           )[SEBSettings.KeyOutsideSebEnableLogOff           ] ? 1 : 0);
                    registryFlagsBuilder.Append((Boolean)SEBClientInfo.getSebSetting(SEBSettings.KeyOutsideSebEnableShutDown         )[SEBSettings.KeyOutsideSebEnableShutDown         ] ? 1 : 0);
                    registryFlagsBuilder.Append((Boolean)SEBClientInfo.getSebSetting(SEBSettings.KeyOutsideSebEnableEaseOfAccess     )[SEBSettings.KeyOutsideSebEnableEaseOfAccess     ] ? 1 : 0);
                    registryFlagsBuilder.Append((Boolean)SEBClientInfo.getSebSetting(SEBSettings.KeyOutsideSebEnableVmWareClientShade)[SEBSettings.KeyOutsideSebEnableVmWareClientShade] ? 1 : 0);
                    string registryFlags = registryFlagsBuilder.ToString();

                    Logger.AddInformation("UserName: " + userName + " UserSid: " + userSid.ToString() + " RegistryFlags: " + registryFlags, this, null);

                    bool bSocketResult;
                    // Send UserName to server
                    bSocketResult = sebSocketClient.SendEquationToSocketServer("UserName", userName, SEBClientInfo.SendInterval);
                    string[] resultUserName = sebSocketClient.RecvEquationOfSocketServer();

                    // Send UserSid to server
                    bSocketResult = sebSocketClient.SendEquationToSocketServer("UserSid", userSid.ToString(), SEBClientInfo.SendInterval);
                    string[] resultUserSid = sebSocketClient.RecvEquationOfSocketServer();

                    // Send RegistryFlags to server
                    bSocketResult = sebSocketClient.SendEquationToSocketServer("RegistryFlags", registryFlags, SEBClientInfo.SendInterval);
                    string[] resultRegistryFlags = sebSocketClient.RecvEquationOfSocketServer();

                }
            }
            return true;
        }

        /// ----------------------------------------------------------------------------------------
        /// <summary>
        /// Set registry values and close prohibited processes.
        /// </summary>
        /// <returns>true if succeed</returns>
        /// ----------------------------------------------------------------------------------------
        private bool InitClientRegistryAndProcesses()
        {
                // Edit Registry
                SEBEditRegistry sebEditRegistry = new SEBEditRegistry();
                if (!sebEditRegistry.EditRegistry())
                {
                    sebEditRegistry.addResetRegValues("EditRegistry", 0);
                }

                try
                {
                    // Kill processes
                    List<object> prohibitedProcessList = (List<object>)SEBClientInfo.getSebSetting(SEBSettings.KeyProhibitedProcesses)[SEBSettings.KeyProhibitedProcesses];
                if (prohibitedProcessList.Count() > 0)
                {
                    for (int i = 0; i < prohibitedProcessList.Count(); i++)
                    {
                        Dictionary<string, object> prohibitedProcess = (Dictionary<string, object>)prohibitedProcessList[i];
                        string prohibitedProcessName = (string)prohibitedProcess[SEBSettings.KeyExecutable];
                        if ((Boolean)prohibitedProcess[SEBSettings.KeyActive])
                        {
                            Logger.AddInformation("Kill process by name: " + prohibitedProcessName, this, null);
                            // Close process
                            //SEBNotAllowedProcessController.CloseProcessByName(prohibitedProcessName);

                            //if (SEBNotAllowedProcessController.CheckIfAProcessIsRunning(prohibitedProcessName))
                            //{
                            //if (SEBErrorMessages.OutputErrorMessage(SEBGlobalConstants.IND_CLOSE_PROCESS_FAILED, SEBGlobalConstants.IND_MESSAGE_KIND_QUESTION, prohibitedProcessName))
                            //{
                            SEBNotAllowedProcessController.KillProcessByName(prohibitedProcessName);
                            //}

                            //}
                        }
                    }

                }
                return true;
            }
            catch (Exception ex)
            {
                Logger.AddError("Error when killing prohibited processes!", null, ex);
                return false;
            }
        }

        /// ----------------------------------------------------------------------------------------
        /// <summary>
        /// Show SEB Application Chooser Form.
        /// </summary>
        /// ----------------------------------------------------------------------------------------
        public void ShowApplicationChooserForm()
        {
            // Show testDialog as a modal dialog and determine if DialogResult = OK.
            SetForegroundWindow(this.Handle);
            //this.Activate();
            sebApplicationChooserForm.fillListApplications();
            sebApplicationChooserForm.Visible = true;
            sebCloseDialogForm.Activate();
        }

        /// ----------------------------------------------------------------------------------------
        /// <summary>
        /// Show SEB Application Chooser Form.
        /// </summary>
        /// ----------------------------------------------------------------------------------------
        public void SelectNextListItem()
        {
            // Show testDialog as a modal dialog and determine if DialogResult = OK.
            sebApplicationChooserForm.SelectNextListItem();
            //sebApplicationChooserForm.Visible = true;
            sebCloseDialogForm.Activate();
        }

        /// ----------------------------------------------------------------------------------------
        /// <summary>
        /// Hide SEB Application Chooser Form.
        /// </summary>
        /// ----------------------------------------------------------------------------------------
        public void HideApplicationChooserForm()
        {
            // Show testDialog as a modal dialog and determine if DialogResult = OK.
            sebApplicationChooserForm.Visible = false;
            //sebCloseDialogForm.Activate();
        }

        /// ----------------------------------------------------------------------------------------
        /// <summary>
        /// Show SEB Close Form.
        /// </summary>
        /// ----------------------------------------------------------------------------------------
        public void ShowCloseDialogForm()
        {
            // Test if quitting SEB is allowed
            if ((bool)SEBSettings.settingsCurrent[SEBSettings.KeyAllowQuit] == true)
            {
                // Is a quit password set?
                string hashedQuitPassword = (string)SEBSettings.settingsCurrent[SEBSettings.KeyHashedQuitPassword];
                if (String.IsNullOrEmpty(hashedQuitPassword) == true)
                // If there is no quit password set, we just ask user to confirm quitting
                {
                    if (SEBErrorMessages.OutputErrorMessageNew(SEBUIStrings.confirmQuitting, SEBUIStrings.confirmQuittingQuestion, SEBGlobalConstants.IND_MESSAGE_KIND_QUESTION, MessageBoxButtons.OKCancel))
                    {
                        //SEBClientInfo.SebWindowsClientForm.closeSebClient = true;
                        Application.Exit();

                    }
                }
                else
                {
                    // Show testDialog as a modal dialog and determine if DialogResult = OK.
                    sebCloseDialogForm.Visible = true;
                    sebCloseDialogForm.Activate();
                    sebCloseDialogForm.txtQuitPassword.Focus();
                }
            }
        }

        /// ----------------------------------------------------------------------------------------
        /// <summary>
        /// Open SEB form.
        /// </summary>
        /// ----------------------------------------------------------------------------------------
        public bool OpenSEBForm()
        {
            if (SebWindowsClientMain.InitSebDesktop()) {
                bool bClientInfo = InitClientSocket();
                bool bClientRegistryAndProcesses = InitClientRegistryAndProcesses();

                // Disable unwanted keys.
                SebKeyCapture.FilterKeys = true;

                addPermittedProcessesToTS();
                SetFormOnDesktop();
                //StartXulRunner();
                //System.Diagnostics.Process oskProcess = null;
                //oskProcess = Process.Start("OSK");
                //SEBDesktopController d = SEBDesktopController.OpenDesktop(SEBClientInfo.DesktopName);
                //oskProcess = d.CreateProcess("OSK");
                if (sebCloseDialogForm == null)
                {
                    sebCloseDialogForm = new SebCloseDialogForm();
                    //              SetForegroundWindow(sebCloseDialogForm.Handle);
                    sebCloseDialogForm.TopMost = true;
                    //sebCloseDialogForm.Show();
                    //sebCloseDialogForm.Visible = false;
                }
                if (sebApplicationChooserForm == null)
                {
                    sebApplicationChooserForm = new SebApplicationChooserForm();
                    sebApplicationChooserForm.TopMost = true;
                    sebApplicationChooserForm.Show();
                    sebApplicationChooserForm.Visible = false;
                }
                return true;
            }
            else
            {
                Application.Exit();
                return false;
            }
        }

        /// ----------------------------------------------------------------------------------------
        /// <summary>
        /// Close SEB Form.
        /// </summary>
        /// ----------------------------------------------------------------------------------------
        public void CloseSEBForm()
        {
            {
                //bool bQuit = false;
                //bQuit = CheckQuitPassword();

                //if (bQuit)
                //{
                bool bSocketResult;
                SEBLocalHostInfo sebLocalHostInfo = new SEBLocalHostInfo();
                string userName = sebLocalHostInfo.GetUserName();

                // ShutDown message to SebWindowsService
                bool serviceAvailable = SEBWindowsServiceController.ServiceAvailable(SEBClientInfo.SEB_WINDOWS_SERVICE_NAME);
                if (serviceAvailable)
                {
                    // Send ShutDown to server
                    bSocketResult = sebSocketClient.SendEquationToSocketServer("ShutDown", userName, SEBClientInfo.SendInterval);
                    string[] resultShutDown = sebSocketClient.RecvEquationOfSocketServer();
                    this.sebSocketClient.CloseSocket();
                }

                // ShutDown Processes
                //Process[] runningApplications = SEBDesktopController.GetInputProcessesWithGI();
                //List<object> permittedProcessList = (List<object>)SEBClientInfo.getSebSetting(SEBSettings.KeyPermittedProcesses)[SEBSettings.KeyPermittedProcesses];
                //for (int i = 0; i < permittedProcessList.Count(); i++)
                //{
                //    for (int j = 0; j < runningApplications.Count(); j++)
                //    {
                //        Dictionary<string, object> permittedProcess = (Dictionary<string, object>)permittedProcessList[i];
                //        string permittedProcessName = (string)permittedProcess[SEBSettings.KeyExecutable];
                //        if ((Boolean)permittedProcess[SEBSettings.KeyActive])
                //        {
                //            if (permittedProcessName.Contains(runningApplications[j].ProcessName))
                //            {
                //                // Close process
                //                //SEBNotAllowedProcessController.CloseProcessByName(runningApplications[j].ProcessName);

                //                //if (SEBNotAllowedProcessController.CheckIfAProcessIsRunning(runningApplications[j].ProcessName))
                //                //{
                //                //if (SEBErrorMessages.OutputErrorMessage(SEBGlobalConstants.IND_CLOSE_PROCESS_FAILED, SEBGlobalConstants.IND_MESSAGE_KIND_QUESTION, runningApplications[j].ProcessName))
                //                //{
                //                SEBNotAllowedProcessController.KillProcessByName(runningApplications[j].ProcessName);
                //                //}

                //                //}
                //            }
                //        }
                //    }
                //}

                foreach (Process processToClose in permittedProcessesReferences)
                {
                    SEBNotAllowedProcessController.CloseProcess(processToClose);
                }
                permittedProcessesReferences.Clear();

                // Restart the explorer.exe shell
                if ((Boolean)SEBClientInfo.getSebSetting(SEBSettings.KeyKillExplorerShell)[SEBSettings.KeyKillExplorerShell])
                {
                    if (SEBClientInfo.ExplorerShellWasKilled)
                    {
                        Logger.AddInformation("Restarting the shell.", null, null);
                        string explorer = string.Format("{0}\\{1}", Environment.GetEnvironmentVariable("WINDIR"), "explorer.exe");
                        Process process = new Process();
                        process.StartInfo.FileName = explorer;
                        process.StartInfo.UseShellExecute = true;
                        process.Start();
                    }
                }

                // Switch to Default Desktop
                if ((Boolean)SEBClientInfo.getSebSetting(SEBSettings.KeyCreateNewDesktop)[SEBSettings.KeyCreateNewDesktop])
                {
                    SEBDesktopController.Show(SEBClientInfo.OriginalDesktop.DesktopName);
                    SEBDesktopController.SetCurrent(SEBClientInfo.OriginalDesktop);
                    SEBClientInfo.SEBNewlDesktop.Close();
                }
                else
                {
                    SetVisibility(true);
                }

                // Clean clipboard
                SEBClipboard.CleanClipboard();
                Logger.AddInformation("Clipboard deleted.", null, null);
                SebKeyCapture.FilterKeys = false;
                Logger.closeLoger();
                //}
                //else
                //{
                //    e.Cancel = true;
                //}

                //sebCloseDialogForm.Dispose();
            }
        }


        /// ----------------------------------------------------------------------------------------
        /// <summary>
        /// Load form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// ----------------------------------------------------------------------------------------
        public void SebWindowsClientForm_Load(object sender, EventArgs e)
        {
            if (SebWindowsClientMain.InitSebSettings())
            {
                OpenSEBForm();
            }
            else {
                Application.Exit();
            }
        }

        /// ----------------------------------------------------------------------------------------
        /// <summary>
        /// Close form, if Quit Password is correct.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// ----------------------------------------------------------------------------------------
        public void SebWindowsClientForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            CloseSEBForm();
        }
     }
}
