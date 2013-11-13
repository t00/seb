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
        private const string SEB_CLIENT_CONFIG = "SebClient.seb";
        private const string SEB_CLIENT_LOG    = "SebClient.log";
        private const string XUL_RUNNER_CONFIG = "config.json";
        public  const string XUL_RUNNER        = "xulrunner.exe";
        private const string XUL_RUNNER_INI    = "seb.ini";
 
        // Application path contains [MANUFACTURER]\[PRODUCT_NAME]
        // (see also "SebWindowsPackageSetup" Project in MS Visual Studio 10)
        private const string MANUFACTURER_LOCAL   = "ETH_Zuerich";
        private const string MANUFACTURER         = "ETH Zuerich";
        private const string PRODUCT_NAME         = "SEB Windows 1.9.1";
        private const string XUL_RUNNER_DIRECTORY = "SebWindowsClient\\xulrunner";
        private const string XUL_SEB_DIRECTORY    = "SebWindowsClient\\xul_seb";

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
        public static string SebClientConfigFileDirectory   { get; set; }
        public static string XulRunnerDirectory { get; set; }
        public static string XulSebDirectory    { get; set; }
        public static string SebClientConfigFile; 
        public static string XulRunnerConfigFileDirectory { get; set; }
        public static string XulRunnerConfigFile;
        public static string XulRunnerExePath;
        public static string XulRunnerSebIniPath;
        public static string ExamUrl      { get; set; }
        public static string QuitPassword { get; set; }
        public static string QuitHashcode { get; set; }

        public static Dictionary<string, object> sebSettings = new Dictionary<string, object>();

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
                 sebSetting = sebSettings[key];
              } 
             catch 
             {
                 sebSetting = null;
             }

             if (sebSetting != null)
                 return sebSettings;
             else
                 return SEBSettings.settingsDef;
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
            SEBErrorMessages  .SetCurrentLanguage();
            SEBErrorMessages  .InitErrorMessages();
            SEBSettings.InitialiseSEBDefaultSettings();

            // Get the path of the "Program" directory.
            ApplicationExecutableDirectory = Path.GetDirectoryName(Application.ExecutablePath);

            // Get the path of the "Program Files X86" directory.
            ProgramFilesX86Directory = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);

            // Get the path of the "Program Data" directory.
            string localAppDataDirectory = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
          //string  programDataDirectory = Environment.GetEnvironmentVariable("PROGRAMMDATA");

            // Set the location of the SebClientConfigFileDirectory
            StringBuilder sebClientConfigFileDirectoryBuilder = new StringBuilder(localAppDataDirectory).Append("\\").Append(MANUFACTURER_LOCAL).Append("\\"); //.Append(PRODUCT_NAME).Append("\\");
            SebClientConfigFileDirectory = sebClientConfigFileDirectoryBuilder.ToString();

            // Set the location of the SebWindowsClientDirectory
            StringBuilder sebClientDirectoryBuilder = new StringBuilder(ProgramFilesX86Directory).Append("\\").Append(MANUFACTURER).Append("\\").Append(PRODUCT_NAME).Append("\\");
            SebClientDirectory = sebClientDirectoryBuilder.ToString();

            // Set the location of the XulRunnerDirectory
            StringBuilder xulRunnerDirectoryBuilder = new StringBuilder(SebClientDirectory).Append(XUL_RUNNER_DIRECTORY).Append("\\");
            XulRunnerDirectory = xulRunnerDirectoryBuilder.ToString();

            // Set the location of the XulSebDirectory
            StringBuilder xulSebDirectoryBuilder = new StringBuilder(SebClientDirectory).Append(XUL_SEB_DIRECTORY).Append("\\");
            XulSebDirectory = xulSebDirectoryBuilder.ToString();

            // Set the location of the XulRunnerExePath
            StringBuilder xulRunnerExePathBuilder = new StringBuilder("\"").Append(XulRunnerDirectory).Append(XUL_RUNNER).Append("\"");
            XulRunnerExePath = xulRunnerExePathBuilder.ToString();

            // Set the location of the seb.ini
            StringBuilder xulRunnerSebIniPathBuilder = new StringBuilder("\"").Append(XulSebDirectory).Append(XUL_RUNNER_INI).Append("\"");
            XulRunnerSebIniPath = xulRunnerSebIniPathBuilder.ToString();

            // Set the location of the SebLogConfigFileDirectory
            StringBuilder SebClientLogFileDirectoryBuilder = new StringBuilder(localAppDataDirectory).Append("\\").Append(MANUFACTURER_LOCAL).Append("\\"); //.Append(PRODUCT_NAME).Append("\\");
            SebClientLogFileDirectory = SebClientLogFileDirectoryBuilder.ToString();


            // Set the location of the SebClient.seb file
            StringBuilder sebClientConfigFileBuilder = new StringBuilder(SebClientConfigFileDirectory).Append(SEB_CLIENT_CONFIG);
            SebClientConfigFile = sebClientConfigFileBuilder.ToString();

            // Set the path of the SebClient.log file
            StringBuilder sebClientLogFileBuilder = new StringBuilder(SebClientLogFileDirectory).Append(SEB_CLIENT_LOG);
            SebClientLogFile = sebClientLogFileBuilder.ToString();

            try
            {
                // Load encrypted SebClient configuration and seserialise it in SEBClientConfig class 
                SEBProtectionController sebProtectionControler = new SEBProtectionController();
                //TextReader sebClientConfigFileReader = new StreamReader(SebClientConfigFile);
                //string encryptedSebClientConfig = sebClientConfigFileReader.ReadToEnd();
                //sebClientConfigFileReader.Close();

                //// Decrypt seb client settings
                //string decriptedSebClientConfig = sebProtectionControler.DecryptSebClientSettings(encryptedSebClientConfig);
                //sebClientConfig = DeserializeFromDeryptedXML(decriptedSebClientConfig);

                // Deserialise SebClient configuration in SEBClientConfig class
                //TextReader sebClientConfigFileReader = new StreamReader(SebClientConfigFile);
                //XmlSerializer deserializer = new XmlSerializer(typeof(SEBClientConfig));
                //sebClientConfig = (SEBClientConfig)deserializer.Deserialize(sebClientConfigFileReader);


                if (!OpenSebFile(SebClientConfigFile))
                    return false;

                //sebSettings.

                // Initialise Loger, if enabled
                if ((Boolean)getSebSetting(SEBSettings.MessageEnableLogging)[SEBSettings.MessageEnableLogging])
                {
                    Logger.initLogger(SebClientLogFile);
                }

                // Set username
                UserName = Environment.UserName;

                setSebClientConfiguration = true;
            }
            catch (Exception ex)
            {
                Logger.AddError("Error ocurred by setting SebClient configuration.", null, ex, ex.Message);
            }
            // Write settings in log
            StringBuilder userInfo =
                new StringBuilder ("User Name: "                   ).Append(UserName)
                          .Append(" Host Name: "                   ).Append(HostName)                         
                          .Append(" Port Number: "                 ).Append(PortNumber)
                          .Append(" Send Interval: "               ).Append(SendInterval)
                          .Append(" Recv Timeout: "                ).Append(RecvTimeout)
                          .Append(" Num Messages: "                ).Append(NumMessages)
                          .Append(" SebClientConfigFileDirectory: ").Append(SebClientConfigFileDirectory)
                          .Append(" SebClientConfigFile: "         ).Append(SebClientConfigFile);
            Logger.AddInformation(userInfo.ToString(), null, null);

            return setSebClientConfiguration;
        }

         // ****************************************
         // Open the .seb file and read the settings
         // ****************************************
         private static bool OpenSebFile(String fileName)
         {
             try
             {
                 // Read the configuration settings from .seb file
                 // Decrypt the configuration settings
                 // Convert the XML structure into a C# object

                 SEBProtectionController sebProtectionControler = new SEBProtectionController();
                 //TextReader textReader;
                 //String encryptedSettings = "";
                 String decryptedSettings = "";
                 //String password          = "Seb";
                 //X509Certificate2 certificate = null;

                 //textReader = new StreamReader(fileName);
                 //encryptedSettings = textReader.ReadToEnd();
                 //textReader.Close();
                 byte[] encryptedSettings = File.ReadAllBytes(fileName);

                 decryptedSettings = sebProtectionControler.DecryptSebClientSettings(encryptedSettings);
                 //decryptedSettings = decryptedSettings.Trim();
                 //decryptedSettings = encryptedSettings;

                 sebSettings = (Dictionary<string, object>)Plist.readPlistSource(decryptedSettings);

             }
             catch (Exception streamReadException)
             {
                 // Let the user know what went wrong
                 Console.WriteLine("The .seb file could not be read:");
                 Console.WriteLine(streamReadException.Message);
                 return false;
             }

             return true;
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
                 xulRunnerConfig.seb_openwin_width  = Int32.Parse(SEBClientInfo.getSebSetting(SEBSettings.MessageNewBrowserWindowByLinkWidth)[SEBSettings.MessageNewBrowserWindowByLinkWidth].ToString());
                 xulRunnerConfig.seb_openwin_height = Int32.Parse(SEBClientInfo.getSebSetting(SEBSettings.MessageNewBrowserWindowByLinkHeight)[SEBSettings.MessageNewBrowserWindowByLinkHeight].ToString());
                 if ((Int32)SEBClientInfo.getSebSetting(SEBSettings.MessageBrowserViewMode)[SEBSettings.MessageBrowserViewMode] == (int)browserViewModes.browserViewModeWindow)
                 {
                     xulRunnerConfig.seb_mainWindow_titlebar_enabled = true;
                 }
                 else
                 {
                     xulRunnerConfig.seb_mainWindow_titlebar_enabled = false;

                 }
                 xulRunnerConfig.seb_url = SEBClientInfo.getSebSetting(SEBSettings.MessageStartURL)[SEBSettings.MessageStartURL].ToString();
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
         /// Deserialize SEBClientConfig from decripted string.
         /// </summary>
         /// <returns></returns>
        // private static SEBClientConfig DeserializeFromDeryptedXML(string decriptedSebClientSettings)
        //{
        //    decriptedSebClientSettings = decriptedSebClientSettings.Trim();
        //    MemoryStream memStream = new MemoryStream(Encoding.UTF8.GetBytes(decriptedSebClientSettings));
        //    XmlSerializer deserializer = new XmlSerializer(typeof(SEBClientConfig));
        //    SEBClientConfig sebSettings;
        //    sebSettings = (SEBClientConfig)deserializer.Deserialize(memStream);
        //    memStream.Close();

        //    return sebSettings;
        //}

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
