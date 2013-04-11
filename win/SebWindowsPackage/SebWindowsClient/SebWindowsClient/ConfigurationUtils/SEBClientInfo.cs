﻿using System;
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

namespace SebWindowsClient.ConfigurationUtils
{
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
        private const string SEB_CLIENT_LOG = "SebClient.log";
        private const string XUL_RUNNER_CONFIG = "config.json";
 
        // Application path contains [MANUFACTURER]\[PRODUCT_NAME]
        // (see also "SebWindowsPackageSetup" Project in MS Visual Studio 10)
        private const string MANUFACTURER = "ETH Zuerich";
        private const string PRODUCT_NAME = "SEB Windows 1.9.1";

        public const string END_OF_STRING_KEYWORD = "---SEB---";
        private const string DEFAULT_USERNAME = "";
        private const string DEFAULT_HOSTNAME = "localhost";
        private const int DEFAULT_PORTNUMBER = 57016;
        private const int DEFAULT_SEND_INTERVAL = 100;
        private const int DEFAULT_RECV_TIMEOUT = 100;
        private const int DEFAULT_NUM_MESSAGES = 3;

        #endregion

        #region Public Properties

        public static bool IsNewOS { get; set; }

        // SEB Client Socket properties
        public static char[] UserNameRegistryFlags { get; set; }
        public static char[] RegistryFlags { get; set; }
        public static string HostName { get; set; }
        public static string UserName  { get; set; }
        public static char[] UserSid  { get; set; }
        public static int PortNumber  { get; set; }
        public static int SendInterval  { get; set; }
        public static int RecvTimeout  { get; set; }
        public static int NumMessages  { get; set; }
        public static int MessageNr  { get; set; }

        public static SEBDesktopController OriginalDesktop { get; set; }
        public static SEBDesktopController SEBNewlDesktop { get; set; }
        //public static string OriginalDesktopName { get; set; }

       // SEB Client Directories properties
        public static string ProgramDataDirectory { get; set; }
        public static bool LogFileDesiredMsgHook { get; set; }
        public static bool LogFileDesiredSebClient { get; set; }
        public static string SebClientLogFileDirectory { get; set; }
        public static string SebClientLogFile { get; set; }
        public static string SebClientConfigFileDirectory { get; set; }
        public static string SebClientConfigFile; 
        public static string XulRunnerConfigFileDirectory { get; set; }
        public static string XulRunnerConfigFile;
        public static string ExamUrl { get; set; }
        public static string QuitPassword { get; set; }
        public static string QuitHashcode { get; set; }

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

         public static SEBClientConfig sebClientConfig;

        /// <summary>
         /// Sets user, host info, send-recv interval, recv timeout, Logger and read SebClient configuration.
        /// </summary>
        /// <returns></returns>
         public static bool SetSebClientConfiguration()
        {

            bool setSebClientConfiguration = false;

           // Initialise socket properties
            IsNewOS = false;
            UserNameRegistryFlags = new char[100];
            RegistryFlags = new char[50];
            UserSid = new char[512];
            UserName = DEFAULT_USERNAME;
            HostName = DEFAULT_HOSTNAME;
            PortNumber = DEFAULT_PORTNUMBER;
            SendInterval = DEFAULT_SEND_INTERVAL;
            RecvTimeout = DEFAULT_RECV_TIMEOUT;
            NumMessages = DEFAULT_NUM_MESSAGES;

            // Initialise error messages
            SEBErrorMessages.SetCurrentLanguage();
            SEBErrorMessages.InitErrorMessages();

            // Get the path of the "Program" directory.
            string programDirectory = Path.GetDirectoryName(Application.ExecutablePath);

            // Get the path of the "Program Data" directory.
            //string programDataDirectory = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string programDataDirectory = Environment.GetEnvironmentVariable("PROGRAMMDATA");

            // Set the location of the SebClientConfigFileDirectory
            StringBuilder sebClientConfigFileDirectoryBuilder = new StringBuilder(programDataDirectory).Append("\\").Append(MANUFACTURER).Append("\\").Append(PRODUCT_NAME).Append("\\");
            SebClientConfigFileDirectory = sebClientConfigFileDirectoryBuilder.ToString();

            // Set the location of the SebLogConfigFileDirectory
            StringBuilder SebClientLogFileDirectoryBuilder = new StringBuilder(programDataDirectory).Append("\\").Append(MANUFACTURER).Append("\\").Append(PRODUCT_NAME).Append("\\");
            SebClientLogFileDirectory = SebClientLogFileDirectoryBuilder.ToString();


            // Set the location of the SebClient.seb file
            StringBuilder sebClientConfigFileBuilder = new StringBuilder(SebClientConfigFileDirectory).Append(SEB_CLIENT_CONFIG);
            SebClientConfigFile = sebClientConfigFileBuilder.ToString();

            // Set the path of the SebClient.log file
            StringBuilder sebClientLogFileBuilder = new StringBuilder(SebClientLogFileDirectory).Append(SEB_CLIENT_LOG);
            SebClientLogFile = sebClientLogFileBuilder.ToString();

            try
            {
                //// Load encrypted SebClient configuration and seserialise it in SEBClientConfig class 
                //SEBProtectionController sEBProtectionControler = new SEBProtectionController();
                //TextReader sebClientConfigFileReader = new StreamReader(SebClientConfigFile);
                //string encryptedSebClientConfig = sebClientConfigFileReader.ReadToEnd();
                //sebClientConfigFileReader.Close();

                //// Decrypt seb client settings
                //string decriptedSebClientConfig = sEBProtectionControler.DecryptSebClientSettings(encryptedSebClientConfig);
                //sebClientConfig = DeserializeFromDeryptedXML(decriptedSebClientConfig);

                // Deserialise SebClient configuration in SEBClientConfig class
                TextReader sebClientConfigFileReader = new StreamReader(SebClientConfigFile);
                XmlSerializer deserializer = new XmlSerializer(typeof(SEBClientConfig));
                sebClientConfig = (SEBClientConfig)deserializer.Deserialize(sebClientConfigFileReader);

                // Initialise Loger, if enabled
                if (sebClientConfig.getSecurityOption("enableLog").getBool())
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
            StringBuilder userInfo = new StringBuilder("User Name: ").Append(UserName).Append(" Host Name: ").Append(HostName)
                                    .Append(" Port Number: ").Append(PortNumber).Append(" Send Interval: ").Append(SendInterval).Append(" Recv Timeout: ").Append(RecvTimeout)
                                    .Append(" Num Messages: ").Append(NumMessages).Append(" SebClientConfigFileDirectory: ").Append(SebClientConfigFileDirectory)
                                    .Append(" SebClientConfigFile: ").Append(SebClientConfigFile);
            Logger.AddInformation(userInfo.ToString(), null, null);

            return setSebClientConfiguration;
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
                 //string programDataDirectory = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                 string programDataDirectory = Environment.GetEnvironmentVariable("PROGRAMMDATA");
 
                 // Set the location of the XULRunnerConfigFileDirectory
                 StringBuilder xulRunnerConfigFileDirectoryBuilder = new StringBuilder(programDataDirectory).Append("\\").Append(MANUFACTURER).Append("\\").Append(PRODUCT_NAME).Append("\\");
                 XulRunnerConfigFileDirectory = xulRunnerConfigFileDirectoryBuilder.ToString();

                 // Set the location of the config.json file
                 StringBuilder xulRunnerConfigFileBuilder = new StringBuilder(XulRunnerConfigFileDirectory).Append(XUL_RUNNER_CONFIG);
                 XulRunnerConfigFile = xulRunnerConfigFileBuilder.ToString();

                 XULRunnerConfig xULRunnerConfig = SEBXulRunnerSettings.XULRunnerConfigDeserialize(XulRunnerConfigFile);
                 xULRunnerConfig.seb_openwin_width = int.Parse(SEBClientInfo.sebClientConfig.getPolicySetting("newBrowserWindowByLinkWidth").Value);
                 xULRunnerConfig.seb_openwin_height = int.Parse(SEBClientInfo.sebClientConfig.getPolicySetting("newBrowserWindowByLinkHeight").Value);
                 if (int.Parse(SEBClientInfo.sebClientConfig.getPolicySetting("browserViewMode").Value) == (int)browserViewModes.browserViewModeWindow)
                 {
                     xULRunnerConfig.seb_mainWindow_titlebar_enabled = true;
                 }
                 else
                 {
                     xULRunnerConfig.seb_mainWindow_titlebar_enabled = false;

                 }
                 setXulRunnerConfiguration = true;
                 SEBXulRunnerSettings.XULRunnerConfigSerialize(xULRunnerConfig, XulRunnerConfigFile);
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
         private static SEBClientConfig DeserializeFromDeryptedXML(string decriptedSebClientSettings)
        {
            decriptedSebClientSettings = decriptedSebClientSettings.Trim();
            MemoryStream memStream = new MemoryStream(Encoding.UTF8.GetBytes(decriptedSebClientSettings));
            XmlSerializer deserializer = new XmlSerializer(typeof(SEBClientConfig));
            SEBClientConfig sEBSettings;
            sEBSettings = (SEBClientConfig)deserializer.Deserialize(memStream);
            memStream.Close();

            return sEBSettings;
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