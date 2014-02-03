using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using SebWindowsClient.DiagnosticsUtils;
using SebWindowsClient.DesktopUtils;
using System.Xml.Serialization;
using SebWindowsClient.CryptographyUtils;
using System.Security.Cryptography.X509Certificates;
using PlistCS;

namespace SebWindowsClient.ConfigurationUtils
{
    public enum chooseFileToUploadPolicies
    {
        manuallyWithFileRequester               = 0,
        attemptUploadSameFileDownloadedBefore   = 1,
        onlyAllowUploadSameFileDownloadedBefore = 2
    };

    public enum newBrowserWindowPolicies
    {
        getGenerallyBlocked = 0,
        openInSameWindow    = 1,
        openInNewWindow     = 2
    };

    public enum sebServicePolicies
    {
        ignoreService          = 0,
        indicateMissingService = 1,
        forceSebService        = 2
    };

    public enum browserViewModes
    {
        browserViewModeWindow     = 0,
        browserViewModeFullscreen = 1
    };

    // MAC
    public enum sebPurposePolicies
    {
        sebPurposePolicyStartingExam      = 0,
        sebPurposePolicyConfiguringClient = 1
    };

    public enum urlFilterRuleActions
    {
        urlFilterActionBlock = 0,
        urlFilterActionAllow = 1,
        urlFilterActionSkip  = 2,
        urlFilterActionAn    = 3,
        urlFilterActionOr    = 4
    };

    public class SEBClientInfo
    {
		#region Imports
        [DllImport("kernel32.Dll")]
        public static extern short GetVersionEx(ref OSVERSIONINFO o);
        #endregion

        // Socket protocol
        //static int ai_family   = AF_INET;
        //static int ai_socktype = SOCK_STREAM;
        //static int ai_protocol = IPPROTO_TCP;

        #region Constants

        // Name and location of SEB configuration files and logfiles
        private const string SEB_CLIENT_CONFIG = "SebClientSettings.seb";
        private const string SEB_CLIENT_LOG    = "SebClient.log";
        private const string XUL_RUNNER_CONFIG = "config.json";
        public  const string XUL_RUNNER        = "xulrunner.exe";
        private const string XUL_RUNNER_INI    = "seb.ini";
 
        // Application path contains [MANUFACTURER]\[PRODUCT_NAME]
        // (see also "SebWindowsPackageSetup" Project in MS Visual Studio 10)
        private const string MANUFACTURER_LOCAL     = "SafeExamBrowser";
        //private const string MANUFACTURER         = "ETH Zuerich";
        private const string PRODUCT_NAME           = "SafeExamBrowser";
        private const string SEB_BROWSER_DIRECTORY  = "SebWindowsBrowser";
        private const string XUL_RUNNER_DIRECTORY   = "xulrunner";
        private const string XUL_SEB_DIRECTORY      = "xul_seb";

        public  const string END_OF_STRING_KEYWORD   = "---SEB---";
        private const string DEFAULT_USERNAME        = "";
        private const string DEFAULT_HOSTNAME        = "localhost";
        private const string DEFAULT_HOST_IP_ADDRESS = "127.0.0.1";
        private const int    DEFAULT_PORTNUMBER      = 57016;
        private const int    DEFAULT_SEND_INTERVAL   = 100;
        private const int    DEFAULT_RECV_TIMEOUT    = 100;
        private const int    DEFAULT_NUM_MESSAGES    = 3;

        public const string SEB_NEW_DESKTOP_NAME     = "SEBDesktop";
        public const string SEB_WINDOWS_SERVICE_NAME = "SebWindowsService";

        #endregion

        #region Public Properties

        public static bool ExplorerShellWasKilled { get; set; }
        public static bool IsNewOS { get; set; }
        public static bool examMode = false;

        // SEB Client Socket properties
        public static char[] UserNameRegistryFlags { get; set; }
        public static char[] RegistryFlags         { get; set; }
        public static string HostName              { get; set; }
        public static string HostIpAddress         { get; set; }
        public static string UserName  { get; set; }
        public static char[] UserSid   { get; set; }
        public static int PortNumber   { get; set; }
        public static int SendInterval { get; set; }
        public static int RecvTimeout  { get; set; }
        public static int NumMessages  { get; set; }
        public static int MessageNr    { get; set; }

        public static SEBDesktopController OriginalDesktop { get; set; }
        public static SEBDesktopController  SEBNewlDesktop { get; set; }
        public static string DesktopName { get; set; }

       // SEB Client Directories properties
        public static string ApplicationExecutableDirectory { get; set; }
        public static string ProgramFilesX86Directory       { get; set; }
        public static bool   LogFileDesiredMsgHook          { get; set; }
        public static bool   LogFileDesiredSebClient        { get; set; }
        public static string SebClientLogFileDirectory      { get; set; }
        public static string SebClientDirectory             { get; set; }
        public static string SebClientLogFile               { get; set; }
        public static string SebClientSettingsProgramDataDirectory { get; set; }
        public static string SebClientSettingsLocalAppDirectory   { get; set; }
        public static string XulRunnerDirectory { get; set; }
        public static string XulSebDirectory    { get; set; }
        public static string SebClientSettingsProgramDataFile;
        public static string SebClientSettingsLocalAppDataFile; 
        public static string XulRunnerConfigFileDirectory { get; set; }
        public static string XulRunnerConfigFile;
        public static string XulRunnerExePath;
        public static string XulRunnerSebIniPath;
        public static string XulRunnerParameter;
        public static string XulRunnerFlashContainerState { get; set; }

        public static string ExamUrl { get; set; }
        public static string QuitPassword { get; set; }
        public static string QuitHashcode { get; set; }

        //public static Dictionary<string, object> sebSettings = new Dictionary<string, object>();

        public static SebWindowsClientForm SebWindowsClientForm;

        #endregion

		#region Structures
       /// <summary>
        /// Stores windows version info.
        /// </summary>
         [StructLayout(LayoutKind.Sequential)]
        public struct OSVERSIONINFO
        {
            public int dwOSVersionInfoSize;
            public int dwMajorVersion;
            public int dwMinorVersion;
            public int dwBuildNumber;
            public int dwPlatformId;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string szCSDVersion;
        }
        #endregion

         //public static SEBClientConfig sebClientConfig;

         public static Dictionary<string, object> getSebSetting(string key)
         {
             object sebSetting = null;
             try
             {
                 sebSetting = SEBSettings.settingsCurrent[key];
              } 
             catch 
             {
                 sebSetting = null;
             }

             if (sebSetting != null)
                 return SEBSettings.settingsCurrent;
             else
                 return SEBSettings.settingsDefault;
         }

        /// <summary>
         /// Sets user, host info, send-recv interval, recv timeout, Logger and read SebClient configuration.
        /// </summary>
        /// <returns></returns>
        public static bool SetSebClientConfiguration()
        {

            bool setSebClientConfiguration = false;

           // Initialise socket properties
            IsNewOS                = false;
            ExplorerShellWasKilled = false;
            UserNameRegistryFlags  = new char[100];
            RegistryFlags          = new char[50];
            UserSid                = new char[512];
            UserName               = DEFAULT_USERNAME;
            HostName               = DEFAULT_HOSTNAME;
            HostIpAddress          = DEFAULT_HOST_IP_ADDRESS;
            PortNumber             = DEFAULT_PORTNUMBER;
            SendInterval           = DEFAULT_SEND_INTERVAL;
            RecvTimeout            = DEFAULT_RECV_TIMEOUT;
            NumMessages            = DEFAULT_NUM_MESSAGES;

            // Initialise error messages
            SEBErrorMessages.SetCurrentLanguage();
            SEBErrorMessages.InitErrorMessages();
            //SEBSettings     .CreateDefaultAndCurrentSettingsFromScratch();

            //Sets paths to files SEB has to save or read from the file system
            SetSebPaths();

            byte[] sebClientSettings = null;

            // Create a string builder for a temporary log (until we can write it with the Logger)
            StringBuilder tempLogStringBuilder = new StringBuilder();

            // Try to read the SebClientSettigs.seb file from the program data directory
            try
            {
                sebClientSettings = File.ReadAllBytes(SebClientSettingsProgramDataFile);
            }
            catch (Exception streamReadException)
            {
                // Write error into string with temporary log string builder
                tempLogStringBuilder.Append("Could not load SebClientSettigs.seb from the Program Data directory").Append(streamReadException == null ? null : streamReadException.GetType().ToString()).Append(streamReadException.Message);
            }
            if (sebClientSettings == null)
            {
                // Try to read the SebClientSettigs.seb file from the local application data directory
                try
                {
                    sebClientSettings = File.ReadAllBytes(SebClientSettingsLocalAppDataFile);
                }
                catch (Exception streamReadException)
                {
                    // Write error into string with temporary log string builder
                    tempLogStringBuilder.Append("Could not load SebClientSettigs.seb from the Local Application Data directory. ").Append(streamReadException == null ? null : streamReadException.GetType().ToString()).Append(streamReadException.Message);
                }
            }

            //// Initialize the password entry dialog form
            //SebWindowsClient.ConfigurationUtils.SEBConfigFileManager.InitSEBConfigFileManager();

            // Store the decrypted configuration settings.
            if (!SEBSettings.StoreDecryptedSebClientSettings(sebClientSettings))
                return false;

            // Close the password entry form
            //SEBConfigFileManager.sebPasswordDialogForm.Close();
            //SEBConfigFileManager.sebPasswordDialogForm.Dispose();
            //int i = 0;
            //do
            //{
            //    Console.WriteLine("Waiting for password dialog to get disposed...");
            //    i++;
            //} while (SEBConfigFileManager.sebPasswordDialogForm.Disposing && i < 10000);
            //SEBConfigFileManager.sebPasswordDialogForm = null;

            // Initialise Logger, if enabled
            InitializeLogger();

            // Save the temporary log string into the log
            Logger.AddError(tempLogStringBuilder.ToString(), null, null);

            // Set username
            UserName = Environment.UserName;

            setSebClientConfiguration = true;
            
            // Write settings in log
            StringBuilder userInfo =
                new StringBuilder ("User Name: "                   ).Append(UserName)
                          .Append(" Host Name: "                   ).Append(HostName)                         
                          .Append(" Port Number: "                 ).Append(PortNumber)
                          .Append(" Send Interval: "               ).Append(SendInterval)
                          .Append(" Recv Timeout: "                ).Append(RecvTimeout)
                          .Append(" Num Messages: "                ).Append(NumMessages)
                          .Append(" SebClientConfigFileDirectory: ").Append(SebClientSettingsLocalAppDirectory)
                          .Append(" SebClientConfigFile: "         ).Append(SebClientSettingsLocalAppDataFile);
            Logger.AddInformation(userInfo.ToString(), null, null);

            return setSebClientConfiguration;
        }

        /// <summary>
        /// Initialise Logger if it's enabled.
        /// </summary>
        public static void InitializeLogger()
        {
            //if ((Boolean)getSebSetting(SEBSettings.KeyEnableLogging)[SEBSettings.KeyEnableLogging])
            //{
                Logger.initLogger(SebClientLogFileDirectory, SebClientLogFile);
            //}
        }

        /// <summary>
        /// Sets paths to files SEB has to save or read from the file system.
        /// </summary>
        public static void SetSebPaths()
        {
            // Get the path of the directory the application executable lies in
            ApplicationExecutableDirectory = Path.GetDirectoryName(Application.ExecutablePath);

            // Get the path of the "Program Files X86" directory.
            ProgramFilesX86Directory = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);

            // Get the path of the "Program Data" and "Local Application Data" directory.
            string programDataDirectory = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData); //GetEnvironmentVariable("PROGRAMMDATA");
            string localAppDataDirectory = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

            /// Get paths for the two possible locations of the SebClientSettings.seb file
            /// 
            // In the program data directory (for managed systems, only an administrator can write in this directory):
            // If there is a SebClientSettigs.seb file, then this has priority and is used by the SEB client, another
            // SebClientSettigs.seb file in the local app data folder is ignored then and the SEB client cannot be 
            // reconfigured by opening a .seb file saved for configuring a client
            StringBuilder sebClientSettingsProgramDataDirectoryBuilder = new StringBuilder(programDataDirectory).Append("\\").Append(MANUFACTURER_LOCAL).Append("\\"); //.Append(PRODUCT_NAME).Append("\\");
            SebClientSettingsProgramDataDirectory = sebClientSettingsProgramDataDirectoryBuilder.ToString();

            // In the local application data directory (for unmanaged systems like student computers, user can write in this directory):
            // A SebClientSettigs.seb file in this directory can be created or replaced by opening a .seb file saved for configuring a client
            StringBuilder sebClientSettingsLocalAppDirectoryBuilder = new StringBuilder(localAppDataDirectory).Append("\\").Append(MANUFACTURER_LOCAL).Append("\\"); //.Append(PRODUCT_NAME).Append("\\");
            SebClientSettingsLocalAppDirectory = sebClientSettingsLocalAppDirectoryBuilder.ToString();

            // Set the location of the SebWindowsClientDirectory
            StringBuilder sebClientDirectoryBuilder = new StringBuilder(ProgramFilesX86Directory).Append("\\").Append(PRODUCT_NAME).Append("\\");
            SebClientDirectory = sebClientDirectoryBuilder.ToString();

            // Set the location of the XulRunnerDirectory
            //StringBuilder xulRunnerDirectoryBuilder = new StringBuilder(SebClientDirectory).Append(XUL_RUNNER_DIRECTORY).Append("\\");
            //XulRunnerDirectory = xulRunnerDirectoryBuilder.ToString();
            StringBuilder xulRunnerDirectoryBuilder = new StringBuilder(SEB_BROWSER_DIRECTORY).Append("\\").Append(XUL_RUNNER_DIRECTORY).Append("\\");
            XulRunnerDirectory = xulRunnerDirectoryBuilder.ToString();

            // Set the location of the XulSebDirectory
            //StringBuilder xulSebDirectoryBuilder = new StringBuilder(SebClientDirectory).Append(XUL_SEB_DIRECTORY).Append("\\");
            //XulSebDirectory = xulSebDirectoryBuilder.ToString();
            StringBuilder xulSebDirectoryBuilder = new StringBuilder(SEB_BROWSER_DIRECTORY).Append("\\").Append(XUL_SEB_DIRECTORY).Append("\\");
            XulSebDirectory = xulSebDirectoryBuilder.ToString();

            // Set the location of the XulRunnerExePath
            //StringBuilder xulRunnerExePathBuilder = new StringBuilder("\"").Append(XulRunnerDirectory).Append(XUL_RUNNER).Append("\"");
            //XulRunnerExePath = xulRunnerExePathBuilder.ToString();
            StringBuilder xulRunnerExePathBuilder = new StringBuilder(XulRunnerDirectory).Append(XUL_RUNNER); //.Append("\"");
            XulRunnerExePath = xulRunnerExePathBuilder.ToString();

            // Set the location of the seb.ini
            StringBuilder xulRunnerSebIniPathBuilder = new StringBuilder(XulSebDirectory).Append(XUL_RUNNER_INI); //.Append("\"");
            XulRunnerSebIniPath = xulRunnerSebIniPathBuilder.ToString();

            // Set the location of the SebLogConfigFileDirectory
            StringBuilder SebClientLogFileDirectoryBuilder = new StringBuilder(localAppDataDirectory).Append("\\").Append(MANUFACTURER_LOCAL).Append("\\"); //.Append(PRODUCT_NAME).Append("\\");
            SebClientLogFileDirectory = SebClientLogFileDirectoryBuilder.ToString();


            // Get the two possible paths of the SebClientSettings.seb file
            StringBuilder sebClientSettingsProgramDataBuilder = new StringBuilder(SebClientSettingsProgramDataDirectory).Append(SEB_CLIENT_CONFIG);
            SebClientSettingsProgramDataFile = sebClientSettingsProgramDataBuilder.ToString();

            StringBuilder sebClientSettingsLocalAppDataBuilder = new StringBuilder(SebClientSettingsLocalAppDirectory).Append(SEB_CLIENT_CONFIG);
            SebClientSettingsLocalAppDataFile = sebClientSettingsLocalAppDataBuilder.ToString();

            // Set the path of the SebClient.log file
            StringBuilder sebClientLogFileBuilder = new StringBuilder(SebClientLogFileDirectory).Append(SEB_CLIENT_LOG);
            SebClientLogFile = sebClientLogFileBuilder.ToString();
        }

         /// <summary>
         /// Sets properties in config.json XULRunner configuration file.
         /// </summary>
         /// <returns></returns>
         public static bool SetXulRunnerConfiguration()
         {
             bool setXulRunnerConfiguration = false;
             try 
             {
                 // Get the path of the "Program Data" directory.
                 string localAppDataDirectory = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                 //string programDataDirectory = Environment.GetEnvironmentVariable("PROGRAMMDATA");
 
                 // Set the location of the XULRunnerConfigFileDirectory
                 StringBuilder xulRunnerConfigFileDirectoryBuilder = new StringBuilder(localAppDataDirectory).Append("\\").Append(MANUFACTURER_LOCAL).Append("\\"); //.Append(PRODUCT_NAME).Append("\\");
                 XulRunnerConfigFileDirectory = xulRunnerConfigFileDirectoryBuilder.ToString();

                 // Set the location of the config.json file
                 StringBuilder xulRunnerConfigFileBuilder = new StringBuilder(XulRunnerConfigFileDirectory).Append(XUL_RUNNER_CONFIG);
                 XulRunnerConfigFile = xulRunnerConfigFileBuilder.ToString();

                 XULRunnerConfig xulRunnerConfig = SEBXulRunnerSettings.XULRunnerConfigDeserialize(XulRunnerConfigFile);
                 xulRunnerConfig.seb_openwin_width  = Int32.Parse(SEBClientInfo.getSebSetting(SEBSettings.KeyNewBrowserWindowByLinkWidth )[SEBSettings.KeyNewBrowserWindowByLinkWidth ].ToString());
                 xulRunnerConfig.seb_openwin_height = Int32.Parse(SEBClientInfo.getSebSetting(SEBSettings.KeyNewBrowserWindowByLinkHeight)[SEBSettings.KeyNewBrowserWindowByLinkHeight].ToString());
                 if ((Int32)SEBClientInfo.getSebSetting(SEBSettings.KeyBrowserViewMode)[SEBSettings.KeyBrowserViewMode] == (int)browserViewModes.browserViewModeWindow)
                 {
                     xulRunnerConfig.seb_mainWindow_titlebar_enabled = true;
                 }
                 else
                 {
                     xulRunnerConfig.seb_mainWindow_titlebar_enabled = false;

                 }
                 xulRunnerConfig.seb_url = SEBClientInfo.getSebSetting(SEBSettings.KeyStartURL)[SEBSettings.KeyStartURL].ToString();
                 setXulRunnerConfiguration = true;
                 SEBXulRunnerSettings.XULRunnerConfigSerialize(xulRunnerConfig, XulRunnerConfigFile);
             }
             catch(Exception ex)
             {
                 Logger.AddError("Error ocurred by setting XulRunner configuration.", null, ex, ex.Message);
             }

             return setXulRunnerConfiguration;
         }


        /// <summary>
        /// Sets system version info.
        /// </summary>
        /// <returns></returns>
        public static bool SetSystemVersionInfo()
        {
            OSVERSIONINFO os = new OSVERSIONINFO();
            os.dwOSVersionInfoSize = Marshal.SizeOf(typeof(OSVERSIONINFO));
            int getVersion = SEBGlobalConstants.OS_UNKNOWN;

            try
            {
                // Gets os version
                if (GetVersionEx(ref os) != 0)
                {
                    switch (os.dwPlatformId)
                    {
                        case 1:
                            switch (os.dwMinorVersion)
                            {
                                case 0:
                                    getVersion = SEBGlobalConstants.WIN_95;
                                    break;
                                case 10:
                                    getVersion = SEBGlobalConstants.WIN_98;
                                    break;
                                case 90:
                                    getVersion = SEBGlobalConstants.WIN_ME;
                                    break;
                                default:
                                    getVersion = SEBGlobalConstants.OS_UNKNOWN;
                                    break;
                            }
                            break;
                        case 2:
                            switch (os.dwMajorVersion)
                            {
                                case 3:
                                    getVersion = SEBGlobalConstants.WIN_NT_351;
                                    break;
                                case 4:
                                    getVersion = SEBGlobalConstants.WIN_NT_40;
                                    break;
                                case 5:
                                    if (os.dwMinorVersion == 0)
                                        getVersion = SEBGlobalConstants.WIN_2000;
                                    else
                                        getVersion = SEBGlobalConstants.WIN_XP;
                                    break;
                                case 6:
                                    if (os.dwMinorVersion == 0)
                                        getVersion = SEBGlobalConstants.WIN_VISTA;
                                    else if (os.dwMinorVersion == 1)
                                        getVersion = SEBGlobalConstants.WIN_7;
                                    else if (os.dwMinorVersion == 2)
                                        getVersion = SEBGlobalConstants.WIN_8;
                                    else
                                        getVersion = SEBGlobalConstants.WIN_VISTA;
                                    break;
                                default:
                                    getVersion = SEBGlobalConstants.OS_UNKNOWN;
                                    break;
                            }
                            break;
                        default:
                            getVersion = SEBGlobalConstants.OS_UNKNOWN;
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.AddError("SetSystemVersionInfo.", null, ex);
            }

            Logger.AddInformation("OS Version: " + getVersion.ToString(), null, null);

            // Is new windows version
            switch (getVersion)
            {
                case SEBGlobalConstants.WIN_NT_351:
                case SEBGlobalConstants.WIN_NT_40:
                case SEBGlobalConstants.WIN_2000:
                case SEBGlobalConstants.WIN_XP:
                case SEBGlobalConstants.WIN_VISTA:
                    IsNewOS = true;
                    return true;
                case SEBGlobalConstants.WIN_95:
                case SEBGlobalConstants.WIN_98:
                case SEBGlobalConstants.WIN_ME:
                    IsNewOS = false;
                    return true;
                default:
                    return false;

            }
        }

    }
}
