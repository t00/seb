using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Windows.Forms;
using SebWindowsClient.ConfigurationUtils;
using SebWindowsClient.DiagnosticsUtils;
using SebWindowsClient.DesktopUtils;
using System.Net;
using System.IO;
using System.Security.Principal;
using SebWindowsClient.ProcessUtils;
using SebWindowsClient.BlockShortcutsUtils;
using System.Runtime.InteropServices;
using System.Diagnostics;
using SebWindowsClient.CryptographyUtils;
using SebWindowsClient.ServiceUtils;
using SebWindowsServiceWCF.ServiceContracts;
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
        private static IntPtr processWindowHandle = IntPtr.Zero;
        private delegate bool EnumThreadProc(IntPtr hwnd, IntPtr lParam);

        private int taskbarHeight;

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
        public List<Image> permittedProcessesIconImages = new List<Image>();

        private List<Process> runningProcessesToClose = new List<Process>();
        private List<string> runningApplicationsToClose = new List<string>();

        //public OpenFileDialog openFileDialog;

        /// ----------------------------------------------------------------------------------------
        /// <summary>
        /// Constructor - initialise components.
        /// </summary>
        /// ----------------------------------------------------------------------------------------
        public SebWindowsClientForm()
        {
            InitializeComponent();
            taskbarHeight = this.Height-2;

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

            string es = string.Join(", ", args);
            Logger.AddError("OnLoad EventArgs: " + es, null, null);

            if (args.Length > 1)
            {
                LoadFile(args[1]);
            }
        }
        public void LoadFile(string file)
        {
            if (!SebWindowsClientMain.isLoadingSebFile())
            {
                SebWindowsClientMain.LoadingSebFile(true);
                // Check if client settings were already set
                if (SebWindowsClientMain.clientSettingsSet == false)
                {
                    // We need to set the client settings first
                    if (SEBClientInfo.SetSebClientConfiguration())
                    {
                        SebWindowsClientMain.clientSettingsSet = true;
                        Logger.AddError("SEB client configuration set in LoadFile(URI).", null, null);
                    }
                }
                byte[] sebSettings = null;
                Uri uri;
                try
                {
                    uri = new Uri(file);
                }
                catch (Exception ex)
                {
                    Logger.AddError("SEB was opened with a wrong parameter", this, ex, ex.Message);
                    SebWindowsClientMain.LoadingSebFile(false);
                    return;
                }
                // Check if we're running in exam mode already, if yes, then refuse to load a .seb file
                if (SEBClientInfo.examMode)
                {
                    //SEBClientInfo.SebWindowsClientForm.Activate();
                    SEBErrorMessages.OutputErrorMessageNew(SEBUIStrings.loadingSettingsNotAllowed, SEBUIStrings.loadingSettingsNotAllowedReason, SEBGlobalConstants.IND_MESSAGE_KIND_ERROR, MessageBoxButtons.OK);
                    SebWindowsClientMain.LoadingSebFile(false);
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
                        SebWindowsClientMain.LoadingSebFile(false);
                        return;
                    }
                }
                // If some settings got loaded in the end
                if (sebSettings == null)
                {
                    SebWindowsClientMain.LoadingSebFile(false);
                    return;
                }

                // Decrypt, parse and store new settings and restart SEB if this was successfull
                SEBConfigFileManager.StoreDecryptedSEBSettings(sebSettings);
                SebWindowsClientMain.LoadingSebFile(false);
            }
        }


        /// ----------------------------------------------------------------------------------------
        /// <summary>
        /// Move SEB to the foreground.
        /// </summary>
        /// ----------------------------------------------------------------------------------------
        public static void SEBToForeground()
        {
            //if ((bool)SEBSettings.valueForDictionaryKey(SEBSettings.settingsCurrent, SEBSettings.KeyShowTaskBar))
            //{
                SetForegroundWindow(SEBClientInfo.SebWindowsClientForm.Handle);
                SEBClientInfo.SebWindowsClientForm.Activate();
            //}
        }

        /// ----------------------------------------------------------------------------------------
        /// <summary>
        // Start xulRunner process.
        /// </summary>
        /// ----------------------------------------------------------------------------------------
        private bool StartXulRunner(string userDefinedArguments)
        {
            //xulRunnerExitEventHandled = false;
            string xulRunnerPath = "";
            string desktopName = "";
            if (userDefinedArguments == null) userDefinedArguments="";
            try
            {
                // Create JSON object with XULRunner parameters to pass to xulrunner.exe as base64 string
                string XULRunnerParameters = SEBXulRunnerSettings.XULRunnerConfigDictionarySerialize(SEBSettings.settingsCurrent);
                // Create the path to xulrunner.exe plus all arguments
                StringBuilder xulRunnerPathBuilder = new StringBuilder(SEBClientInfo.XulRunnerExePath);
                // Create all arguments, including user defined
                StringBuilder xulRunnerArgumentsBuilder = new StringBuilder(" -app \"").Append(Application.StartupPath).Append("\\").Append(SEBClientInfo.XulRunnerSebIniPath).Append("\"");
                // Check if there is a user defined -profile parameter, otherwise use the standard one 
                if (!(userDefinedArguments.ToLower()).Contains("-profile"))
                    xulRunnerArgumentsBuilder.Append(" -profile \"").Append(SEBClientInfo.SebClientSettingsLocalAppDirectory).Append("Profiles\"");
                xulRunnerArgumentsBuilder.Append(" ").Append(userDefinedArguments).Append(" -ctrl \"").Append(XULRunnerParameters).Append("\"");
                string xulRunnerArguments = xulRunnerArgumentsBuilder.ToString();
                xulRunnerPathBuilder.Append(xulRunnerArguments);
                xulRunnerPath = xulRunnerPathBuilder.ToString();
                Logger.AddError("Starting XULRunner with call: " + xulRunnerPath, this, null);

                desktopName = SEBClientInfo.DesktopName;
                xulRunner = SEBDesktopController.CreateProcess(xulRunnerPath, desktopName);
                xulRunner.EnableRaisingEvents = true;
                xulRunner.Exited += new EventHandler(xulRunner_Exited);
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
            Logger.AddError("XULRunner exit event fired.", this, null);

            //xulRunnerExitEventHandled = true;
            // Is the handle for the XULRunner process valid?
            if (xulRunner != null)
            {
                try
                {
                    // Read the exit code. Strange enough this often fails in Windows 8.1
                    xulRunnerExitCode = xulRunner.ExitCode;
                    xulRunnerExitTime = xulRunner.ExitTime;
                }
                catch (Exception ex)
                {
                    xulRunnerExitCode = -1;
                    // An error occured when reading exit code, probably XULRunner didn't actually exit yet
                    Logger.AddError("Error reading XULRunner exit code!", this, ex);
                }
            }
            else
            {
                // The XULRunner process didn't exist anymore
                xulRunnerExitCode = 0;
               // xulRunnerExitTime = ;
            }
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
                if (SEBClientInfo.SebWindowsClientForm.closeSebClient)
                {
                    Logger.AddError("XULRunner was closed, SEB will exit now.", this, null);
                    Application.Exit();
                }
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
            permittedProcessesIconImages.Clear();

            List<object> permittedProcessList = (List<object>)SEBClientInfo.getSebSetting(SEBSettings.KeyPermittedProcesses)[SEBSettings.KeyPermittedProcesses];
            if (permittedProcessList.Count > 0)
            {
                // Check if permitted third party applications are already running
                List<Process> runningApplications = new List<Process>();
                runningApplications = Process.GetProcesses().ToList();
                //Process[] runningApplications = Process.GetProcesses();
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
                        if (!(executable.Contains(SEBClientInfo.XUL_RUNNER) && !(bool)SEBSettings.valueForDictionaryKey(SEBSettings.settingsCurrent, SEBSettings.KeyEnableSebBrowser)))
                        {
                            // Check if the process is already running
                            //runningApplications = Process.GetProcesses();
                            int j = 0;
                            while (j < runningApplications.Count())
                            {
                                try
                                {
                                    // Get the name of the running process. This might fail if the process has terminated in between, we have to catch this case
                                    string name = runningApplications[j].ProcessName;
                                    if (executable.Contains(name))
                                    {
                                        // If the flag strongKill is set, then the process is killed without asking the user
                                        bool strongKill = (bool)SEBSettings.valueForDictionaryKey(permittedProcess, SEBSettings.KeyStrongKill);
                                        if (strongKill)
                                        {
                                            Logger.AddError("Closing already running permitted process with strongKill flag set: " + name, null, null);
                                            SEBNotAllowedProcessController.CloseProcess(runningApplications[j]);
                                            // Remove the process from the list of running processes
                                            runningApplications.RemoveAt(j);
                                        }
                                        else
                                        {
                                            runningProcessesToClose.Add(runningApplications[j]);
                                            runningApplicationsToClose.Add(title == "SEB" ? executable : title);
                                            //runningApplicationsToClose.Add((title == "SEB" ? "" : (title == "" ? "" : title + " - ")) + executable);
                                            j++;
                                        }
                                    }
                                    else
                                    {
                                        j++;
                                    }
                                }
                                catch (Exception)
                                {
                                    // Running process has been terminated in the meantime, so we remove it from the list
                                    runningApplications.RemoveAt(j);
                                }
                            }
                        }
                    }
                }
            }
            // If we found already running permitted or if there were prohibited processes on the list, 
            // we ask the user how to quit them
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
                    runningProcessesToClose.Clear();
                    runningApplicationsToClose.Clear();
                }
                else
                {
                    Application.Exit();
                    return;
                }
            }

            // So if there are any permitted processes, we add them to the SEB task bar
            if (permittedProcessList.Count > 0)
            {
                for (int i = 0; i < permittedProcessList.Count; i++)
                {
                    Dictionary<string, object> permittedProcess = (Dictionary<string, object>)permittedProcessList[i];
                    SEBSettings.operatingSystems permittedProcessOS = (SEBSettings.operatingSystems)SEBSettings.valueForDictionaryKey(permittedProcess, SEBSettings.KeyOS);
                    bool permittedProcessActive = (bool)SEBSettings.valueForDictionaryKey(permittedProcess, SEBSettings.KeyActive);
                    //if (permittedProcessActive == null) permittedProcessActive = false;
                    if (permittedProcessOS == SEBSettings.operatingSystems.operatingSystemWin && permittedProcessActive)
                    {
                        string title = (string)SEBSettings.valueForDictionaryKey(permittedProcess, SEBSettings.KeyTitle);
                        if (title == null) title = "";
                        string executable = (string)permittedProcess[SEBSettings.KeyExecutable];
                        if (!(executable.Contains(SEBClientInfo.XUL_RUNNER) && !(bool)SEBSettings.valueForDictionaryKey(SEBSettings.settingsCurrent, SEBSettings.KeyEnableSebBrowser)))
                        {
                            ToolStripButton toolStripButton = new ToolStripButton();
                            toolStripButton.Padding = new Padding(5, 0, 5, 0);
                            toolStripButton.ToolTipText = title;
                            Icon processIcon = null;
                            string fullPath;
                            if (executable.Contains(SEBClientInfo.XUL_RUNNER))
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
                                // Save the icon image also to be used for the app chooser
                                permittedProcessesIconImages.Add(toolStripButton.Image);
                                toolStripButton.Click += new EventHandler(ToolStripButton_Click);

                                // We save the index of the permitted process to the toolStripButton.Name property
                                toolStripButton.Name = permittedProcessesCalls.Count.ToString();

                                //if ((bool)SEBSettings.valueForDictionaryKey(SEBSettings.settingsCurrent, SEBSettings.KeyShowTaskBar))
                                taskbarToolStrip.Items.Add(toolStripButton);
                                //toolStripButton.Checked = true;

                                // Treat XULRunner different than other processes
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

                                }
                                else
                                {
                                    // The permitted process is XULRunner: Build list of arguments that are allowed to be user defined
                                    if ((bool)SEBSettings.valueForDictionaryKey(SEBSettings.settingsCurrent, SEBSettings.KeyEnableSebBrowser))
                                    {
                                        StringBuilder startProcessNameBuilder = new StringBuilder("");
                                        List<object> argumentList = (List<object>)permittedProcess[SEBSettings.KeyArguments];
                                        for (int j = 0; j < argumentList.Count; j++)
                                        {
                                            Dictionary<string, object> argument = (Dictionary<string, object>)argumentList[j];
                                            if ((Boolean)argument[SEBSettings.KeyActive])
                                            {
                                                string argumentString = (string)argument[SEBSettings.KeyArgument];
                                                // The parameters -app and -ctrl cannot be changed by the user, we skip them 
                                                if (!argumentString.Contains("-app") && !argumentString.Contains("-ctrl")) 
                                                    startProcessNameBuilder.Append(" ").Append((string)argument[SEBSettings.KeyArgument]);
                                            }
                                        }
                                        string fullPathArgumentsCall = startProcessNameBuilder.ToString();

                                        // Save the full path of the permitted process executable including arguments
                                        permittedProcessesCalls.Add(fullPathArgumentsCall);
                                    }
                                }
                            }
                            else
                            {
                                // Permitted application has not been found: Set its call entry to null
                                permittedProcessesCalls.Add(null);
                                //SEBClientInfo.SebWindowsClientForm.Activate();
                                SEBErrorMessages.OutputErrorMessageNew(SEBUIStrings.permittedApplicationNotFound, SEBUIStrings.permittedApplicationNotFoundMessage, SEBGlobalConstants.IND_MESSAGE_KIND_ERROR, MessageBoxButtons.OK, title);
                            }
                        }
                    }
                }
            }

            // Start permitted processes
            int permittedProcessesIndex = 0;
            for (int i = 0; i < permittedProcessList.Count; i++)
            //foreach (string processCallToStart in permittedProcessesCalls)
            {
                Dictionary<string, object> permittedProcess = (Dictionary<string, object>)permittedProcessList[i];
                SEBSettings.operatingSystems permittedProcessOS = (SEBSettings.operatingSystems)SEBSettings.valueForDictionaryKey(permittedProcess, SEBSettings.KeyOS);
                bool permittedProcessActive = (bool)SEBSettings.valueForDictionaryKey(permittedProcess, SEBSettings.KeyActive);
                string executable = (string)permittedProcess[SEBSettings.KeyExecutable];
                if (permittedProcessOS == SEBSettings.operatingSystems.operatingSystemWin && permittedProcessActive)
                {
                    if (!executable.Contains(SEBClientInfo.XUL_RUNNER))
                    {
                        // Autostart processes which have the according flag set
                        Process newProcess = null;
                        if ((Boolean)permittedProcess[SEBSettings.KeyAutostart])
                        {
                            string fullPathArgumentsCall = permittedProcessesCalls[permittedProcessesIndex];
                            if (fullPathArgumentsCall != null) newProcess = CreateProcessWithExitHandler(fullPathArgumentsCall);
                            else newProcess = null;
                        }
                        // Save the process reference if the process was started, otherwise null
                        permittedProcessesReferences.Add(newProcess);
                        permittedProcessesIndex++;
                    }
                    else
                    {
                        if ((bool)SEBSettings.valueForDictionaryKey(SEBSettings.settingsCurrent, SEBSettings.KeyEnableSebBrowser))
                        {
                            // Start XULRunner
                            StartXulRunner((string)permittedProcessesCalls[permittedProcessesIndex]);
                            // Save the process reference of XULRunner
                            permittedProcessesReferences.Add(xulRunner);
                            permittedProcessesIndex++;
                        }
                    }
                }
            }

            SEBToForeground();
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
        public string GetApplicationPath(string executable)
        {
            // Check if executable string contained also a valid path
            if (File.Exists(executable)) return executable;

            // Check if executable is in the Programm Directory
            string programDir = SEBClientInfo.ProgramFilesX86Directory + "\\";
            if (File.Exists(programDir + executable)) return programDir;

            // Check if executable is in the System Directory
            string systemDirectory = Environment.SystemDirectory + "\\";
            if (File.Exists(systemDirectory + executable)) return systemDirectory;

            using (Microsoft.Win32.RegistryKey key = Microsoft.Win32.RegistryKey.OpenRemoteBaseKey(Microsoft.Win32.RegistryHive.LocalMachine, ""))
            {
                //// Get all paths from the PATH environement variable
                //// This code didn't get the results expected, just left here for reference
                //string RegKeyName = @"SYSTEM\CurrentControlSet\Control\Session Manager\Environment";
                //string pathVariableString = (string)Microsoft.Win32.Registry.LocalMachine.OpenSubKey(RegKeyName).GetValue
                //    ("Path", "", Microsoft.Win32.RegistryValueOptions.DoNotExpandEnvironmentNames);
                //string[] paths = pathVariableString.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                //foreach (string subPath in paths)
                //{
                //    Console.WriteLine(subPath);
                //}

                string subKeyName = @"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\" + executable;
                using (Microsoft.Win32.RegistryKey subkey = key.OpenSubKey(subKeyName))
                {
                    if (subkey == null)
                    {
                        return null;
                    }

                    object path = subkey.GetValue("Path");

                    if (path != null)
                    {
                        return (string)path;
                    }
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
            }
            // Otherwise try to determine the applications full path
            string path = GetApplicationPath(executable);

            // If a path to the executable was found
            if (path != null)
            {
                fullPath = path + executable;
                // Maybe the executablePath information wasn't necessary to find the application, then we return this found path
                if (File.Exists(fullPath)) return fullPath;
 
                // But maybe the executable path is a relative path from the applications main directory to some subdirectory with the executable in it?
                fullPath = path + executablePath + "\\" + executable;
                if (File.Exists(fullPath)) return fullPath;
            }

            // In the end we try to find the application using one of the system's standard paths + subdirectory path + executable
            fullPath = null;
            path = GetApplicationPath(executablePath + "\\" + executable);
            if (path != null)
            {
                fullPath = path + executable;
            }

            // If we still didn't find the application and the setting for this permitted process allows user to find the application
            if (fullPath == null && allowChoosingApp && !String.IsNullOrEmpty(executable))
            {
                // Ask the user to locate the application
                SEBToForeground();
                return ThreadedDialog.ShowFileDialogForExecutable(executable);
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
                        StartXulRunner((string)permittedProcessesCalls[i]);
                    }
                    else
                    {
                        processReference.Refresh();
                        IntPtr handle = processReference.MainWindowHandle;
                        if (IsIconic(handle)) ShowWindow(handle, SW_RESTORE);
                        SetForegroundWindow(handle);
                    }
                }
                catch (Exception)  // XULRunner wasn't running anymore
                {
                    StartXulRunner((string)permittedProcessesCalls[i]);
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
                        processReference.Refresh();
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
            int y = Screen.PrimaryScreen.Bounds.Height - taskbarHeight;

            this.Height = taskbarHeight;
            this.Width = width;
            this.Location = new Point(x, y);
            this.TopMost = true;
            return true;
        }

        /// <summary>
        /// Returns the handle of the active window of a process
        /// </summary>
        /// <param name="taskBarWnd">window handle of taskbar</param>
        /// <returns>window handle of start menu</returns>
        private static IntPtr GetActiveWindowOfProcess(Process proc)
        {
            //if (proc != null)
            //{
            //    // enumerate all threads of that process...
            //    foreach (ProcessThread thread in proc.Threads)
            //    {
            //        if (EnumThreadWindows(thread.Id, MyEnumThreadWindowsForProcess, IntPtr.Zero)) break;
            //    }
            //}
            return vistaStartMenuWnd;
        }

        /// <summary>
        /// Callback method that is called from 'EnumThreadWindows' in 'GetVistaStartMenuWnd'.
        /// </summary>
        /// <param name="hWnd">window handle</param>
        /// <param name="lParam">parameter</param>
        /// <returns>true to continue enumeration, false to stop it</returns>
        private static bool MyEnumThreadWindowsForProcess(int pid, IntPtr lParam)
        {
                //if (pid ==)
                //{
                //    vistaStartMenuWnd = hWnd;
                //    return false;
                //}
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
        /// Show dialog if SEB should be closed
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
        /// Set registry values and close prohibited processes.
        /// </summary>
        /// <returns>true if succeed</returns>
        /// ----------------------------------------------------------------------------------------
        private bool InitClientRegistryAndKillProcesses()
        {

            // Add prohibited processes to the "processes not permitted to run" list 
            // which will be dealt with after checking if permitted processes are already running;
            // the user will be asked to quit all those processes him/herself or to let SEB kill them
            // Prohibited processes with the strongKill flag set can be killed without user consent

            List<object> prohibitedProcessList = (List<object>)SEBClientInfo.getSebSetting(SEBSettings.KeyProhibitedProcesses)[SEBSettings.KeyProhibitedProcesses];
            if (prohibitedProcessList.Count() > 0)
            {
                // Check if the prohibited processes are running
                Process[] runningApplications;
                runningProcessesToClose.Clear();
                runningApplicationsToClose.Clear();
                for (int i = 0; i < prohibitedProcessList.Count; i++)
                {
                    Dictionary<string, object> prohibitedProcess = (Dictionary<string, object>)prohibitedProcessList[i];
                    SEBSettings.operatingSystems prohibitedProcessOS = (SEBSettings.operatingSystems)SEBSettings.valueForDictionaryKey(prohibitedProcess, SEBSettings.KeyOS);
                    bool prohibitedProcessActive = (bool)SEBSettings.valueForDictionaryKey(prohibitedProcess, SEBSettings.KeyActive);
                    if (prohibitedProcessOS == SEBSettings.operatingSystems.operatingSystemWin && prohibitedProcessActive)
                    {
                        string title = (string)SEBSettings.valueForDictionaryKey(prohibitedProcess, SEBSettings.KeyTitle);
                        if (title == null) title = "";
                        string executable = (string)prohibitedProcess[SEBSettings.KeyExecutable];
                        // Check if the process is running
                        runningApplications = Process.GetProcesses();
                        for (int j = 0; j < runningApplications.Count(); j++)
                        {
                            string runningProcessName = runningApplications[j].ProcessName;
                            if (runningProcessName != null && executable.Contains(runningProcessName))
                            {
                                // If the flag strongKill is set, then the process is killed without asking the user
                                bool strongKill = (bool)SEBSettings.valueForDictionaryKey(prohibitedProcess, SEBSettings.KeyStrongKill);
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
            }
            return true;
        }

        /// ----------------------------------------------------------------------------------------
        /// <summary>
        /// Show SEB Application Chooser Form.
        /// </summary>
        /// ----------------------------------------------------------------------------------------
        public void ShowApplicationChooserForm()
        {
            //SetForegroundWindow(this.Handle);
            //this.Activate();
            sebApplicationChooserForm.fillListApplications();
            sebApplicationChooserForm.Visible = true;
            //sebCloseDialogForm.Activate();
        }

        /// ----------------------------------------------------------------------------------------
        /// <summary>
        /// Show SEB Application Chooser Form.
        /// </summary>
        /// ----------------------------------------------------------------------------------------
        public void SelectNextListItem()
        {
            sebApplicationChooserForm.SelectNextListItem();
            //sebApplicationChooserForm.Visible = true;
            //sebCloseDialogForm.Activate();
        }

        /// ----------------------------------------------------------------------------------------
        /// <summary>
        /// Hide SEB Application Chooser Form.
        /// </summary>
        /// ----------------------------------------------------------------------------------------
        public void HideApplicationChooserForm()
        {
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
                    SetForegroundWindow(this.Handle);
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
            if ((bool)SEBSettings.valueForDictionaryKey(SEBSettings.settingsCurrent, SEBSettings.KeyShowTaskBar))
            {
                //this.Show();
                SetFormOnDesktop();
                //if (!this.Controls.Contains(this.taskbarToolStrip))
                //{
                //    this.Controls.Add(this.taskbarToolStrip);
                //    taskbarToolStrip.Show();
                //    Logger.AddInformation("Removed SEB taskbar re-added to form.", null, null);
                //}
            }
            else
            {
                //this.Hide();
                this.Location = new System.Drawing.Point(-50000, -50000);

                this.Size = new System.Drawing.Size(1, 1);

                //if (this.Controls.Contains(this.taskbarToolStrip))
                //{
                //    this.Controls.Remove(this.taskbarToolStrip);
                //    taskbarToolStrip.Hide();
                //    Logger.AddInformation("Tried to remove SEB taskbar from form.", null, null);
                //    if (this.Controls.Contains(this.taskbarToolStrip)) Logger.AddInformation("Removing SEB taskbar from form didn't work.", null, null);
                //}
            }


            // Check if VM and SEB Windows Service available and required
            if (SebWindowsClientMain.CheckVMService()) {
                try
                {
                    if(SebWindowsServiceHandler.IsServiceAvailable && !SebWindowsServiceHandler.SetRegistryAccordingToConfiguration())
                        Logger.AddWarning("Unable to set registry values",this,null);
                }
                catch (Exception ex)
                {
                    Logger.AddError("Unable to set Registry values",this,ex);
                }
                
                bool bClientRegistryAndProcesses = InitClientRegistryAndKillProcesses();

                // Disable unwanted keys.
                SebKeyCapture.FilterKeys = true;

                // Save the value of the environment variable determining if XULRunner (and Mozilla Firefox) start plugins in plugins-container.exe
                // If SEB runs on an own desktop (createNewDesktop = true), plugins like Flash won't work if they are started in plugin-container.exe
                //SEBClientInfo.XulRunnerFlashContainerState = System.Environment.GetEnvironmentVariable("MOZ_DISABLE_OOP_PLUGINS", EnvironmentVariableTarget.User);

                //// Disable plugins-container if enablePlugIns = true and createNewDesktop = true
                //if ((bool)SEBSettings.settingsCurrent[SEBSettings.KeyEnablePlugIns] && (bool)SEBSettings.settingsCurrent[SEBSettings.KeyCreateNewDesktop])
                //{
                //    System.Environment.SetEnvironmentVariable("MOZ_DISABLE_OOP_PLUGINS", "1", EnvironmentVariableTarget.User);
                //    string xulRunnerFlashContainer = System.Environment.GetEnvironmentVariable("MOZ_DISABLE_OOP_PLUGINS", EnvironmentVariableTarget.User);
                //    Logger.AddInformation("Environment Variable MOZ_DISABLE_OOP_PLUGINS had value: " +
                //        (SEBClientInfo.XulRunnerFlashContainerState == null ? "null" : SEBClientInfo.XulRunnerFlashContainerState), null, null);
                //    Logger.AddInformation("Environment Variable MOZ_DISABLE_OOP_PLUGINS was set to value: " + xulRunnerFlashContainer, null, null);
                //}

                addPermittedProcessesToTS();
                //SetFormOnDesktop();
                
                //System.Diagnostics.Process oskProcess = null;
                //oskProcess = Process.Start("OSK");
                //SEBDesktopController d = SEBDesktopController.OpenDesktop(SEBClientInfo.DesktopName);
                //oskProcess = d.CreateProcess("OSK");
                
                if (sebCloseDialogForm == null)
                {
                    sebCloseDialogForm = new SebCloseDialogForm();
                    //SetForegroundWindow(sebCloseDialogForm.Handle);
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
                // VM or service not available and set to be required
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

                try
                {
                    if (SebWindowsServiceHandler.IsServiceAvailable && !SebWindowsServiceHandler.ResetRegistry())
                    {
                        Logger.AddWarning("Unable to reset Registry values",this,null);
                    }
                }
                catch (Exception ex)
                {
                    Logger.AddError("Unable to reset Registry values",this,ex);
                }
                    

                // ShutDown Processes
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
                        process.StartInfo.WorkingDirectory = Application.StartupPath;
                        process.StartInfo.CreateNoWindow = true;
                        process.Start();
                    }
                }

                //// Switch to Default Desktop
                //if ((Boolean)SEBClientInfo.getSebSetting(SEBSettings.KeyCreateNewDesktop)[SEBSettings.KeyCreateNewDesktop])
                //{
                //    SEBDesktopController.Show(SEBClientInfo.OriginalDesktop.DesktopName);
                //    SEBDesktopController.SetCurrent(SEBClientInfo.OriginalDesktop);
                //    SEBClientInfo.SEBNewlDesktop.Close();
                //}
                //else
                //{
                //    SetVisibility(true);
                //}

                // Reset plugins-container to the system's state before SEB was started if enablePlugIns = true and createNewDesktop = true
                //if ((bool)SEBSettings.settingsCurrent[SEBSettings.KeyEnablePlugIns] && (bool)SEBSettings.settingsCurrent[SEBSettings.KeyCreateNewDesktop])
                //{
                //    System.Environment.SetEnvironmentVariable("MOZ_DISABLE_OOP_PLUGINS", SEBClientInfo.XulRunnerFlashContainerState, EnvironmentVariableTarget.User);
                //    string xulRunnerFlashContainer = System.Environment.GetEnvironmentVariable("MOZ_DISABLE_OOP_PLUGINS", EnvironmentVariableTarget.User);
                //    Logger.AddInformation("Environment Variable MOZ_DISABLE_OOP_PLUGINS reset to value: " + xulRunnerFlashContainer, null, null);
                //}

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
            //if (SebWindowsClientMain.InitSebSettings())
            //{
                OpenSEBForm();
            //}
            //else {
            //    Application.Exit();
            //}
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
            SebWindowsClientMain.ResetSEBDesktop();
        }

        /// ----------------------------------------------------------------------------------------
        /// <summary>
        /// Show dialog asking whether SEB should be closed
        /// </summary>
        /// ----------------------------------------------------------------------------------------
        private void noSelectButton1_Click(object sender, EventArgs e)
        {
            if ((bool)SEBSettings.settingsCurrent[SEBSettings.KeyAllowQuit] == true)
            {
                ShowCloseDialogForm();
            }
        }
     }

    /// ----------------------------------------------------------------------------------------
    /// <summary>
    /// Button subclass which makes the button-not-selectable. 
    /// Why this isn't a default attribute of a button???.
    /// </summary>
    /// ----------------------------------------------------------------------------------------
    class NoSelectButton : Button
    {
        public NoSelectButton()
        {
            SetStyle(ControlStyles.Selectable, false);
        }
    }

}
