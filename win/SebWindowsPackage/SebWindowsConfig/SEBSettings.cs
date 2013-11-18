using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using SebWindowsClient.CryptographyUtils;
using PlistCS;



namespace SebWindowsClient.ConfigurationUtils
{
    public class SEBSettings
    {

        // **************************
        // Constants for SEB settings
        // **************************

        // The default SEB configuration file
        public const String DefaultSebConfigXml = "SebClient.xml";
        public const String DefaultSebConfigSeb = "SebClient.seb";

        // The values can be in 3 different states:
        // new, temporary and default values
        public const int StateNew = 1;
        public const int StateTmp = 2;
        public const int StateDef = 3;
        public const int StateNum = 3;

        // Operating systems
        const int IntOSX = 0;
        const int IntWin = 1;

        // Some key/value pairs are not stored in the sebSettings Plist structures,
        // so they must be separately stored in arrays
        public const int ValueCryptoIdentity               = 1;
        public const int ValueMainBrowserWindowWidth       = 2;
        public const int ValueMainBrowserWindowHeight      = 3;
        public const int ValueNewBrowserWindowByLinkWidth  = 4;
        public const int ValueNewBrowserWindowByLinkHeight = 5;
        public const int ValueTaskBarHeight                = 6;
        public const int ValueNum = 6;

        // Group "General"
        public const String MessageStartURL             = "startURL";
        public const String MessageSebServerURL         = "sebServerURL";
        public const String MessageAdminPassword        = "adminPassword";
        public const String MessageConfirmAdminPassword = "confirmAdminPassword";
        public const String MessageHashedAdminPassword  = "hashedAdminPassword";
        public const String MessageAllowQuit            = "allowQuit";
        public const String MessageIgnoreQuitPassword   = "ignoreQuitPassword";
        public const String MessageQuitPassword         = "quitPassword";
        public const String MessageConfirmQuitPassword  = "confirmQuitPassword";
        public const String MessageHashedQuitPassword   = "hashedQuitPassword";
        public const String MessageExitKey1             = "exitKey1";
        public const String MessageExitKey2             = "exitKey2";
        public const String MessageExitKey3             = "exitKey3";
        public const String MessageSebMode              = "sebMode";

        // Group "Config File"
        public const String MessageSebConfigPurpose        = "sebConfigPurpose";
        public const String MessageAllowPreferencesWindow  = "allowPreferencesWindow";
        public const String MessageCryptoIdentity          = "cryptoIdentity";
        public const String MessageSettingsPassword        = "settingsPassword";
        public const String MessageConfirmSettingsPassword = "confirmSettingsPassword";
        public const String MessageHashedSettingsPassword  = "hashedSettingsPassword";

        // Group "Appearance"
        public const String MessageBrowserViewMode              = "browserViewMode";
        public const String MessageMainBrowserWindowWidth       = "mainBrowserWindowWidth";
        public const String MessageMainBrowserWindowHeight      = "mainBrowserWindowHeight";
        public const String MessageMainBrowserWindowPositioning = "mainBrowserWindowPositioning";
        public const String MessageEnableBrowserWindowToolbar   = "enableBrowserWindowToolbar";
        public const String MessageHideBrowserWindowToolbar     = "hideBrowserWindowToolbar";
        public const String MessageShowMenuBar                  = "showMenuBar";
        public const String MessageShowTaskBar                  = "showTaskBar";
        public const String MessageTaskBarHeight                = "taskBarHeight";

        // Group "Browser"
        public const String MessageNewBrowserWindowByLinkPolicy         = "newBrowserWindowByLinkPolicy";
        public const String MessageNewBrowserWindowByScriptPolicy       = "newBrowserWindowByScriptPolicy";
        public const String MessageNewBrowserWindowByLinkBlockForeign   = "newBrowserWindowByLinkBlockForeign";
        public const String MessageNewBrowserWindowByScriptBlockForeign = "newBrowserWindowByScriptBlockForeign";
        public const String MessageNewBrowserWindowByLinkWidth          = "newBrowserWindowByLinkWidth";
        public const String MessageNewBrowserWindowByLinkHeight         = "newBrowserWindowByLinkHeight";
        public const String MessageNewBrowserWindowByLinkPositioning    = "newBrowserWindowByLinkPositioning";
        public const String MessageEnablePlugIns                        = "enablePlugIns";
        public const String MessageEnableJava                           = "enableJava";
        public const String MessageEnableJavaScript                     = "enableJavaScript";
        public const String MessageBlockPopUpWindows                    = "blockPopUpWindows";
        public const String MessageAllowBrowsingBackForward             = "allowBrowsingBackForward";
        public const String MessageEnableSebBrowser                     = "enableSebBrowser";

        // Group "DownUploads"
        public const String MessageAllowDownUploads         = "allowDownUploads";
        public const String MessageDownloadDirectoryOSX     = "downloadDirectoryOSX";
        public const String MessageDownloadDirectoryWin     = "downloadDirectoryWin";
        public const String MessageOpenDownloads            = "openDownloads";
        public const String MessageChooseFileToUploadPolicy = "chooseFileToUploadPolicy";
        public const String MessageDownloadPDFFiles         = "downloadPDFFiles";

        // Group "Exam"
        public const String MessageExamKeySalt        = "examKeySalt";
        public const String MessageBrowserExamKey     = "browserExamKey";
        public const String MessageCopyBrowserExamKey = "copyBrowserExamKeyToClipboardWhenQuitting";
        public const String MessageSendBrowserExamKey = "sendBrowserExamKey";
        public const String MessageQuitURL            = "quitURL";

        // Group "Applications"
        public const String MessageMonitorProcesses = "monitorProcesses";

        // Group "Applications - Permitted  Processes"
        public const String MessagePermittedProcesses        = "permittedProcesses";
        public const String MessageAllowSwitchToApplications = "allowSwitchToApplications";
        public const String MessageAllowFlashFullscreen      = "allowFlashFullscreen";

        // Group "Applications - Prohibited Processes"
        public const String MessageProhibitedProcesses = "prohibitedProcesses";

        public const String MessageActive      = "active";
        public const String MessageAutostart   = "autostart";
        public const String MessageAutohide    = "autohide";
        public const String MessageAllowUser   = "allowUserToChooseApp";
        public const String MessageCurrentUser = "currentUser";
        public const String MessageStrongKill  = "strongKill";
        public const String MessageOS          = "os";
        public const String MessageTitle       = "title";
        public const String MessageDescription = "description";
        public const String MessageExecutable  = "executable";
        public const String MessagePath        = "path";
        public const String MessageIdentifier  = "identifier";
        public const String MessageUser        = "user";
        public const String MessageArguments   = "arguments";
        public const String MessageArgument    = "argument";

        // Group "Network"
        public const String MessageEnableURLFilter        = "enableURLFilter";
        public const String MessageEnableURLContentFilter = "enableURLContentFilter";

        // Group "Network - Filter"
        public const String MessageURLFilterRules = "URLFilterRules";
        public const String MessageExpression     = "expression";
        public const String MessageRuleActions    = "ruleActions";
        public const String MessageRegex          = "regex";
        public const String MessageAction         = "action";

        // Group "Network - Certificates"
        public const String MessageEmbedSSLServerCertificate = "EmbedSSLServerCertificate";
        public const String MessageEmbedIdentity             = "EmbedIdentity";
        public const String MessageEmbeddedCertificates      = "embeddedCertificates";
        public const String MessageCertificateData           = "certificateData";
        public const String MessageType                      = "type";
        public const String MessageName                      = "name";

        // Group "Network - Proxies"
        public const String MessageProxySettingsPolicy       = "proxySettingsPolicy";

        public const String MessageProxies                     = "proxies";
        public const String MessageExceptionsList              = "ExceptionsList";
        public const String MessageExcludeSimpleHostnames      = "ExcludeSimpleHostnames";
        public const String MessageFTPPassive                  = "FTPPassive";

        public const String MessageAutoDiscoveryEnabled        = "AutoDiscoveryEnabled";
        public const String MessageAutoConfigurationEnabled    = "AutoConfigurationEnabled";
        public const String MessageAutoConfigurationJavaScript = "AutoConfigurationJavaScript";
        public const String MessageAutoConfigurationURL        = "AutoConfigurationURL";

        public const String MessageAutoDiscovery     = "";
        public const String MessageAutoConfiguration = "";
        public const String MessageHTTP              = "HTTP";
        public const String MessageHTTPS             = "HTTPS";
        public const String MessageFTP               = "FTP";
        public const String MessageSOCKS             = "SOCKS";
        public const String MessageRTSP              = "RTSP";

        public const String MessageEnable   = "Enable";
        public const String MessagePort     = "Port";
        public const String MessageHost     = "Proxy";
        public const String MessageRequires = "RequiresPassword";
        public const String MessageUsername = "Username";
        public const String MessagePassword = "Password";

        public const String MessageHTTPEnable   = "HTTPEnable";
        public const String MessageHTTPPort     = "HTTPPort";
        public const String MessageHTTPHost     = "HTTPProxy";
        public const String MessageHTTPRequires = "HTTPRequiresPassword";
        public const String MessageHTTPUsername = "HTTPUsername";
        public const String MessageHTTPPassword = "HTTPPassword";

        public const String MessageHTTPSEnable   = "HTTPSEnable";
        public const String MessageHTTPSPort     = "HTTPSPort";
        public const String MessageHTTPSHost     = "HTTPSProxy";
        public const String MessageHTTPSRequires = "HTTPSRequiresPassword";
        public const String MessageHTTPSUsername = "HTTPSUsername";
        public const String MessageHTTPSPassword = "HTTPSPassword";

        public const String MessageFTPEnable   = "FTPEnable";
        public const String MessageFTPPort     = "FTPPort";
        public const String MessageFTPHost     = "FTPProxy";
        public const String MessageFTPRequires = "FTPRequiresPassword";
        public const String MessageFTPUsername = "FTPUsername";
        public const String MessageFTPPassword = "FTPPassword";

        public const String MessageSOCKSEnable   = "SOCKSEnable";
        public const String MessageSOCKSPort     = "SOCKSPort";
        public const String MessageSOCKSHost     = "SOCKSProxy";
        public const String MessageSOCKSRequires = "SOCKSRequiresPassword";
        public const String MessageSOCKSUsername = "SOCKSUsername";
        public const String MessageSOCKSPassword = "SOCKSPassword";

        public const String MessageRTSPEnable   = "RTSPEnable";
        public const String MessageRTSPPort     = "RTSPPort";
        public const String MessageRTSPHost     = "RTSPProxy";
        public const String MessageRTSPRequires = "RTSPRequiresPassword";
        public const String MessageRTSPUsername = "RTSPUsername";
        public const String MessageRTSPPassword = "RTSPPassword";

        // Group "Security"
        public const String MessageSebServicePolicy    = "sebServicePolicy";
        public const String MessageAllowVirtualMachine = "allowVirtualMachine";
        public const String MessageCreateNewDesktop    = "createNewDesktop";
        public const String MessageKillExplorerShell   = "killExplorerShell";
        public const String MessageAllowUserSwitching  = "allowUserSwitching";
        public const String MessageEnableLogging       = "enableLogging";
        public const String MessageLogDirectoryOSX     = "logDirectoryOSX";
        public const String MessageLogDirectoryWin     = "logDirectoryWin";

        // Group "Registry"

        // Group "Inside SEB"
        public const String MessageInsideSebEnableSwitchUser        = "insideSebEnableSwitchUser";
        public const String MessageInsideSebEnableLockThisComputer  = "insideSebEnableLockThisComputer";
        public const String MessageInsideSebEnableChangeAPassword   = "insideSebEnableChangeAPassword";
        public const String MessageInsideSebEnableStartTaskManager  = "insideSebEnableStartTaskManager";
        public const String MessageInsideSebEnableLogOff            = "insideSebEnableLogOff";
        public const String MessageInsideSebEnableShutDown          = "insideSebEnableShutDown";
        public const String MessageInsideSebEnableEaseOfAccess      = "insideSebEnableEaseOfAccess";
        public const String MessageInsideSebEnableVmWareClientShade = "insideSebEnableVmWareClientShade";

        // Group "Outside SEB"
        public const String MessageOutsideSebEnableSwitchUser        = "outsideSebEnableSwitchUser";
        public const String MessageOutsideSebEnableLockThisComputer  = "outsideSebEnableLockThisComputer";
        public const String MessageOutsideSebEnableChangeAPassword   = "outsideSebEnableChangeAPassword";
        public const String MessageOutsideSebEnableStartTaskManager  = "outsideSebEnableStartTaskManager";
        public const String MessageOutsideSebEnableLogOff            = "outsideSebEnableLogOff";
        public const String MessageOutsideSebEnableShutDown          = "outsideSebEnableShutDown";
        public const String MessageOutsideSebEnableEaseOfAccess      = "outsideSebEnableEaseOfAccess";
        public const String MessageOutsideSebEnableVmWareClientShade = "outsideSebEnableVmWareClientShade";

        // Group "Hooked Keys"
        public const String MessageHookKeys = "hookKeys";

        // Group "Special Keys"
        public const String MessageEnableEsc        = "enableEsc";
        public const String MessageEnableCtrlEsc    = "enableCtrlEsc";
        public const String MessageEnableAltEsc     = "enableAltEsc";
        public const String MessageEnableAltTab     = "enableAltTab";
        public const String MessageEnableAltF4      = "enableAltF4";
        public const String MessageEnableStartMenu  = "enableStartMenu";
        public const String MessageEnableRightMouse = "enableRightMouse";

        // Group "Function Keys"
        public const String MessageEnableF1  = "enableF1";
        public const String MessageEnableF2  = "enableF2";
        public const String MessageEnableF3  = "enableF3";
        public const String MessageEnableF4  = "enableF4";
        public const String MessageEnableF5  = "enableF5";
        public const String MessageEnableF6  = "enableF6";
        public const String MessageEnableF7  = "enableF7";
        public const String MessageEnableF8  = "enableF8";
        public const String MessageEnableF9  = "enableF9";
        public const String MessageEnableF10 = "enableF10";
        public const String MessageEnableF11 = "enableF11";
        public const String MessageEnableF12 = "enableF12";


        // *********************************
        // Global Variables for SEB settings
        // *********************************

        // Some settings are not stored in Plists but in Arrays
        public static String [,] settingsStr = new String [StateNum + 1, ValueNum + 1];
        public static     int[,] settingsInt = new     int[StateNum + 1, ValueNum + 1];

        // Class SEBSettings contains all settings
        // and is used for importing/exporting the settings
        // from/to a human-readable .xml and an encrypted.seb file format.
        public static Dictionary<string, object> settingsNew = new Dictionary<string, object>();
        public static Dictionary<string, object> settingsTmp = new Dictionary<string, object>();
        public static Dictionary<string, object> settingsDef = new Dictionary<string, object>();

        public static int                        permittedProcessIndex;
        public static List<object>               permittedProcessList    = new List<object>();
        public static Dictionary<string, object> permittedProcessData    = new Dictionary<string, object>();
        public static Dictionary<string, object> permittedProcessDataDef = new Dictionary<string, object>();

        public static int                        permittedArgumentIndex;
        public static List<object>               permittedArgumentList    = new List<object>();
        public static Dictionary<string, object> permittedArgumentData    = new Dictionary<string, object>();
        public static Dictionary<string, object> permittedArgumentDataDef = new Dictionary<string, object>();

        public static int                        prohibitedProcessIndex;
        public static List<object>               prohibitedProcessList    = new List<object>();
        public static Dictionary<string, object> prohibitedProcessData    = new Dictionary<string, object>();
        public static Dictionary<string, object> prohibitedProcessDataDef = new Dictionary<string, object>();

        public static int                        urlFilterRuleIndex;
        public static List<object>               urlFilterRuleList       = new List<object>();
        public static Dictionary<string, object> urlFilterRuleData       = new Dictionary<string, object>();
        public static Dictionary<string, object> urlFilterRuleDataDef    = new Dictionary<string, object>();
        public static Dictionary<string, object> urlFilterRuleDataStored = new Dictionary<string, object>();

        public static int                        urlFilterActionIndex;
        public static List<object>               urlFilterActionList       = new List<object>();
        public static List<object>               urlFilterActionListDef    = new List<object>();
        public static List<object>               urlFilterActionListStored = new List<object>();
        public static Dictionary<string, object> urlFilterActionData       = new Dictionary<string, object>();
        public static Dictionary<string, object> urlFilterActionDataDef    = new Dictionary<string, object>();
        public static Dictionary<string, object> urlFilterActionDataStored = new Dictionary<string, object>();

        public static int                        embeddedCertificateIndex;
        public static List<object>               embeddedCertificateList    = new List<object>();
        public static Dictionary<string, object> embeddedCertificateData    = new Dictionary<string, object>();
        public static Dictionary<string, object> embeddedCertificateDataDef = new Dictionary<string, object>();

        public static Dictionary<string, object> proxiesData    = new Dictionary<string, object>();
        public static Dictionary<string, object> proxiesDataDef = new Dictionary<string, object>();

        public static int                        proxyProtocolIndex;

        public static int                        bypassedProxyIndex;
        public static List<object>               bypassedProxyList    = new List<object>();
        public static String                     bypassedProxyData    = "";
        public static String                     bypassedProxyDataDef = "";



        // ************************
        // Methods for SEB settings
        // ************************


        // *******************************************************************
        // Set all the default values for the Plist structure "sebSettingsDef"
        // *******************************************************************
        public static void BuildUpDefaultSettings()
        {
            // Initialise the global arrays
            for (int state = 1; state <= StateNum; state++)
            for (int value = 1; value <= ValueNum; value++)
            {
                settingsInt[state, value] = 0;
                settingsStr[state, value] = "";
            }

            // Initialise the default settings Plist
            settingsDef.Clear();

            // Default settings for group "General"
            settingsDef.Add(SEBSettings.MessageStartURL            , "http://www.safeexambrowser.org");
            settingsDef.Add(SEBSettings.MessageSebServerURL        , "");
            settingsDef.Add(SEBSettings.MessageAdminPassword       , "");
            settingsDef.Add(SEBSettings.MessageConfirmAdminPassword, "");
            settingsDef.Add(SEBSettings.MessageHashedAdminPassword , "");
            settingsDef.Add(SEBSettings.MessageAllowQuit           , true);
            settingsDef.Add(SEBSettings.MessageIgnoreQuitPassword  , false);
            settingsDef.Add(SEBSettings.MessageQuitPassword        , "");
            settingsDef.Add(SEBSettings.MessageConfirmQuitPassword , "");
            settingsDef.Add(SEBSettings.MessageHashedQuitPassword  , "");
            settingsDef.Add(SEBSettings.MessageExitKey1,  2);
            settingsDef.Add(SEBSettings.MessageExitKey2, 10);
            settingsDef.Add(SEBSettings.MessageExitKey3,  5);
            settingsDef.Add(SEBSettings.MessageSebMode, 0);

            // Default settings for group "Config File"
            settingsDef.Add(SEBSettings.MessageSebConfigPurpose       , 0);
            settingsDef.Add(SEBSettings.MessageAllowPreferencesWindow , true);
            settingsDef.Add(SEBSettings.MessageSettingsPassword       , "");
            settingsDef.Add(SEBSettings.MessageConfirmSettingsPassword, "");
            settingsDef.Add(SEBSettings.MessageHashedSettingsPassword , "");

            // CryptoIdentity is stored additionally
            settingsInt[StateDef, SEBSettings.ValueCryptoIdentity] = 0;
            settingsStr [StateDef, SEBSettings.ValueCryptoIdentity] = "";

            // Default settings for group "Appearance"
            settingsDef.Add(SEBSettings.MessageBrowserViewMode             , 0);
            settingsDef.Add(SEBSettings.MessageMainBrowserWindowWidth      , "100%");
            settingsDef.Add(SEBSettings.MessageMainBrowserWindowHeight     , "100%");
            settingsDef.Add(SEBSettings.MessageMainBrowserWindowPositioning, 1);
            settingsDef.Add(SEBSettings.MessageEnableBrowserWindowToolbar  , false);
            settingsDef.Add(SEBSettings.MessageHideBrowserWindowToolbar    , false);
            settingsDef.Add(SEBSettings.MessageShowMenuBar                 , false);
            settingsDef.Add(SEBSettings.MessageShowTaskBar                 , true);
            settingsDef.Add(SEBSettings.MessageTaskBarHeight               , 40);

            // MainBrowserWindow Width and Height is stored additionally
            settingsInt[StateDef, SEBSettings.ValueMainBrowserWindowWidth ] = 1;
            settingsInt[StateDef, SEBSettings.ValueMainBrowserWindowHeight] = 1;
            settingsStr [StateDef, SEBSettings.ValueMainBrowserWindowWidth ] = "100%";
            settingsStr [StateDef, SEBSettings.ValueMainBrowserWindowHeight] = "100%";

            // Default settings for group "Browser"
            settingsDef.Add(SEBSettings.MessageNewBrowserWindowByLinkPolicy        , 2);
            settingsDef.Add(SEBSettings.MessageNewBrowserWindowByScriptPolicy      , 2);
            settingsDef.Add(SEBSettings.MessageNewBrowserWindowByLinkBlockForeign  , false);
            settingsDef.Add(SEBSettings.MessageNewBrowserWindowByScriptBlockForeign, false);
            settingsDef.Add(SEBSettings.MessageNewBrowserWindowByLinkWidth         , "1000");
            settingsDef.Add(SEBSettings.MessageNewBrowserWindowByLinkHeight        , "100%");
            settingsDef.Add(SEBSettings.MessageNewBrowserWindowByLinkPositioning   , 2);

            settingsDef.Add(SEBSettings.MessageEnablePlugIns           , true);
            settingsDef.Add(SEBSettings.MessageEnableJava              , false);
            settingsDef.Add(SEBSettings.MessageEnableJavaScript        , true);
            settingsDef.Add(SEBSettings.MessageBlockPopUpWindows       , false);
            settingsDef.Add(SEBSettings.MessageAllowBrowsingBackForward, false);
            settingsDef.Add(SEBSettings.MessageEnableSebBrowser        , true);

            // NewBrowserWindow Width and Height is stored additionally
            settingsInt[StateDef, SEBSettings.ValueNewBrowserWindowByLinkWidth ] = 3;
            settingsInt[StateDef, SEBSettings.ValueNewBrowserWindowByLinkHeight] = 1;
            settingsStr [StateDef, SEBSettings.ValueNewBrowserWindowByLinkWidth ] = "1000";
            settingsStr [StateDef, SEBSettings.ValueNewBrowserWindowByLinkHeight] = "100%";

            // Default settings for group "DownUploads"
            settingsDef.Add(SEBSettings.MessageAllowDownUploads        , true);
            settingsDef.Add(SEBSettings.MessageDownloadDirectoryOSX    , "~/Downloads");
            settingsDef.Add(SEBSettings.MessageDownloadDirectoryWin    , "Desktop");
            settingsDef.Add(SEBSettings.MessageOpenDownloads           , false);
            settingsDef.Add(SEBSettings.MessageChooseFileToUploadPolicy, 0);
            settingsDef.Add(SEBSettings.MessageDownloadPDFFiles        , false);

            // Default settings for group "Exam"
            settingsDef.Add(SEBSettings.MessageExamKeySalt       , new Byte[] {});
            settingsDef.Add(SEBSettings.MessageBrowserExamKey    , "");
            settingsDef.Add(SEBSettings.MessageCopyBrowserExamKey, false);
            settingsDef.Add(SEBSettings.MessageSendBrowserExamKey, false);
            settingsDef.Add(SEBSettings.MessageQuitURL           , "");

            // Default settings for group "Applications"
            settingsDef.Add(SEBSettings.MessageMonitorProcesses         , false);
            settingsDef.Add(SEBSettings.MessagePermittedProcesses       , new List<object>());
            settingsDef.Add(SEBSettings.MessageAllowSwitchToApplications, false);
            settingsDef.Add(SEBSettings.MessageAllowFlashFullscreen     , false);
            settingsDef.Add(SEBSettings.MessageProhibitedProcesses      , new List<object>());

            // Default settings for permitted process data
            permittedProcessDataDef.Clear();
            permittedProcessDataDef.Add(SEBSettings.MessageActive     , true);
            permittedProcessDataDef.Add(SEBSettings.MessageAutostart  , true);
            permittedProcessDataDef.Add(SEBSettings.MessageAutohide   , true);
            permittedProcessDataDef.Add(SEBSettings.MessageAllowUser  , true);
            permittedProcessDataDef.Add(SEBSettings.MessageOS         , IntWin);
            permittedProcessDataDef.Add(SEBSettings.MessageTitle      , "");
            permittedProcessDataDef.Add(SEBSettings.MessageDescription, "");
            permittedProcessDataDef.Add(SEBSettings.MessageExecutable , "");
            permittedProcessDataDef.Add(SEBSettings.MessagePath       , "");
            permittedProcessDataDef.Add(SEBSettings.MessageIdentifier , "");
            permittedProcessDataDef.Add(SEBSettings.MessageArguments  , new List<object>());

            // Default settings for permitted argument data
            permittedArgumentDataDef.Clear();
            permittedArgumentDataDef.Add(SEBSettings.MessageActive  , true);
            permittedArgumentDataDef.Add(SEBSettings.MessageArgument, "");

            // Default settings for prohibited process data
            prohibitedProcessDataDef.Clear();
            prohibitedProcessDataDef.Add(SEBSettings.MessageActive     , true);
            prohibitedProcessDataDef.Add(SEBSettings.MessageCurrentUser, true);
            prohibitedProcessDataDef.Add(SEBSettings.MessageStrongKill , false);
            prohibitedProcessDataDef.Add(SEBSettings.MessageOS         , IntWin);
            prohibitedProcessDataDef.Add(SEBSettings.MessageExecutable , "");
            prohibitedProcessDataDef.Add(SEBSettings.MessageDescription, "");
            prohibitedProcessDataDef.Add(SEBSettings.MessageIdentifier , "");
            prohibitedProcessDataDef.Add(SEBSettings.MessageUser       , "");

            // Default settings for group "Network - Filter"
            settingsDef.Add(SEBSettings.MessageEnableURLFilter       , false);
            settingsDef.Add(SEBSettings.MessageEnableURLContentFilter, false);
            settingsDef.Add(SEBSettings.MessageURLFilterRules        , new List<object>());

            // Create a default action
            urlFilterActionDataDef.Clear();
            urlFilterActionDataDef.Add(SEBSettings.MessageActive    , true);
            urlFilterActionDataDef.Add(SEBSettings.MessageRegex     , false);
            urlFilterActionDataDef.Add(SEBSettings.MessageExpression, "");
            urlFilterActionDataDef.Add(SEBSettings.MessageAction    , 0);

            // Create a default action list with one entry (the default action)
            urlFilterActionListDef.Clear();
            urlFilterActionListDef.Add(urlFilterActionDataDef);

            // Create a default rule with this default action list.
            // This default rule is used for the "Insert Rule" operation:
            // when a new rule is created, it initially contains one action.
            urlFilterRuleDataDef.Clear();
            urlFilterRuleDataDef.Add(SEBSettings.MessageActive     , true);
            urlFilterRuleDataDef.Add(SEBSettings.MessageExpression , "Rule");
            urlFilterRuleDataDef.Add(SEBSettings.MessageRuleActions, urlFilterActionListDef);

            // Initialise the stored action
            urlFilterActionDataStored.Clear();
            urlFilterActionDataStored.Add(SEBSettings.MessageActive    , true);
            urlFilterActionDataStored.Add(SEBSettings.MessageRegex     , false);
            urlFilterActionDataStored.Add(SEBSettings.MessageExpression, "*");
            urlFilterActionDataStored.Add(SEBSettings.MessageAction    , 0);

            // Initialise the stored rule
            urlFilterRuleDataStored.Clear();
            urlFilterRuleDataStored.Add(SEBSettings.MessageActive     , true);
            urlFilterRuleDataStored.Add(SEBSettings.MessageExpression , "Rule");
            urlFilterRuleDataStored.Add(SEBSettings.MessageRuleActions, urlFilterActionListStored);

            // Default settings for group "Network - Certificates"
            settingsDef.Add(SEBSettings.MessageEmbeddedCertificates, new List<object>());

            embeddedCertificateDataDef.Clear();
            embeddedCertificateDataDef.Add(SEBSettings.MessageCertificateData, "");
            embeddedCertificateDataDef.Add(SEBSettings.MessageType           , 0);
            embeddedCertificateDataDef.Add(SEBSettings.MessageName           , "");

            // Default settings for group "Network - Proxies"
            proxiesDataDef.Clear();

            proxiesDataDef.Add(SEBSettings.MessageExceptionsList             , new List<object>());
            proxiesDataDef.Add(SEBSettings.MessageExcludeSimpleHostnames     , true);
            proxiesDataDef.Add(SEBSettings.MessageAutoDiscoveryEnabled       , false);
            proxiesDataDef.Add(SEBSettings.MessageAutoConfigurationEnabled   , false);
            proxiesDataDef.Add(SEBSettings.MessageAutoConfigurationJavaScript, "");
            proxiesDataDef.Add(SEBSettings.MessageAutoConfigurationURL       , "");
            proxiesDataDef.Add(SEBSettings.MessageFTPPassive                 , true);

            proxiesDataDef.Add(SEBSettings.MessageHTTPEnable          , false);
            proxiesDataDef.Add(SEBSettings.MessageHTTPPort            , 0);
            proxiesDataDef.Add(SEBSettings.MessageHTTPHost            , "");
            proxiesDataDef.Add(SEBSettings.MessageHTTPRequires, false);
            proxiesDataDef.Add(SEBSettings.MessageHTTPUsername        , "");
            proxiesDataDef.Add(SEBSettings.MessageHTTPPassword        , "");

            proxiesDataDef.Add(SEBSettings.MessageHTTPSEnable          , false);
            proxiesDataDef.Add(SEBSettings.MessageHTTPSPort            , 0);
            proxiesDataDef.Add(SEBSettings.MessageHTTPSHost            , "");
            proxiesDataDef.Add(SEBSettings.MessageHTTPSRequires, false);
            proxiesDataDef.Add(SEBSettings.MessageHTTPSUsername        , "");
            proxiesDataDef.Add(SEBSettings.MessageHTTPSPassword        , "");

            proxiesDataDef.Add(SEBSettings.MessageFTPEnable          , false);
            proxiesDataDef.Add(SEBSettings.MessageFTPPort            , 0);
            proxiesDataDef.Add(SEBSettings.MessageFTPHost            , "");
            proxiesDataDef.Add(SEBSettings.MessageFTPRequires, false);
            proxiesDataDef.Add(SEBSettings.MessageFTPUsername        , "");
            proxiesDataDef.Add(SEBSettings.MessageFTPPassword        , "");

            proxiesDataDef.Add(SEBSettings.MessageSOCKSEnable          , false);
            proxiesDataDef.Add(SEBSettings.MessageSOCKSPort            , 0);
            proxiesDataDef.Add(SEBSettings.MessageSOCKSHost            , "");
            proxiesDataDef.Add(SEBSettings.MessageSOCKSRequires, false);
            proxiesDataDef.Add(SEBSettings.MessageSOCKSUsername        , "");
            proxiesDataDef.Add(SEBSettings.MessageSOCKSPassword        , "");

            proxiesDataDef.Add(SEBSettings.MessageRTSPEnable          , false);
            proxiesDataDef.Add(SEBSettings.MessageRTSPPort            , 0);
            proxiesDataDef.Add(SEBSettings.MessageRTSPHost            , "");
            proxiesDataDef.Add(SEBSettings.MessageRTSPRequires, false);
            proxiesDataDef.Add(SEBSettings.MessageRTSPUsername        , "");
            proxiesDataDef.Add(SEBSettings.MessageRTSPPassword        , "");

            bypassedProxyDataDef = "";

            settingsDef.Add(SEBSettings.MessageProxySettingsPolicy, 0);
            settingsDef.Add(SEBSettings.MessageProxies            , proxiesDataDef);

            // Default settings for group "Security"
            settingsDef.Add(SEBSettings.MessageSebServicePolicy   , 2);
            settingsDef.Add(SEBSettings.MessageAllowVirtualMachine, false);
            settingsDef.Add(SEBSettings.MessageCreateNewDesktop   , true);
            settingsDef.Add(SEBSettings.MessageKillExplorerShell  , false);
            settingsDef.Add(SEBSettings.MessageAllowUserSwitching , true);
            settingsDef.Add(SEBSettings.MessageEnableLogging      , false);
            settingsDef.Add(SEBSettings.MessageLogDirectoryOSX    , "~/Documents");
            settingsDef.Add(SEBSettings.MessageLogDirectoryWin    , "My Documents");

            // Default settings for group "Inside SEB"
            settingsDef.Add(SEBSettings.MessageInsideSebEnableSwitchUser       , false);
            settingsDef.Add(SEBSettings.MessageInsideSebEnableLockThisComputer , false);
            settingsDef.Add(SEBSettings.MessageInsideSebEnableChangeAPassword  , false);
            settingsDef.Add(SEBSettings.MessageInsideSebEnableStartTaskManager , false);
            settingsDef.Add(SEBSettings.MessageInsideSebEnableLogOff           , false);
            settingsDef.Add(SEBSettings.MessageInsideSebEnableShutDown         , false);
            settingsDef.Add(SEBSettings.MessageInsideSebEnableEaseOfAccess     , false);
            settingsDef.Add(SEBSettings.MessageInsideSebEnableVmWareClientShade, false);

            // Default settings for group "Outside SEB"
            settingsDef.Add(SEBSettings.MessageOutsideSebEnableSwitchUser       , true);
            settingsDef.Add(SEBSettings.MessageOutsideSebEnableLockThisComputer , true);
            settingsDef.Add(SEBSettings.MessageOutsideSebEnableChangeAPassword  , true);
            settingsDef.Add(SEBSettings.MessageOutsideSebEnableStartTaskManager , true);
            settingsDef.Add(SEBSettings.MessageOutsideSebEnableLogOff           , true);
            settingsDef.Add(SEBSettings.MessageOutsideSebEnableShutDown         , true);
            settingsDef.Add(SEBSettings.MessageOutsideSebEnableEaseOfAccess     , true);
            settingsDef.Add(SEBSettings.MessageOutsideSebEnableVmWareClientShade, true);

            // Default settings for group "Hooked Keys"
            settingsDef.Add(SEBSettings.MessageHookKeys, true);

            // Default settings for group "Special Keys"
            settingsDef.Add(SEBSettings.MessageEnableEsc       , false);
            settingsDef.Add(SEBSettings.MessageEnableCtrlEsc   , false);
            settingsDef.Add(SEBSettings.MessageEnableAltEsc    , false);
            settingsDef.Add(SEBSettings.MessageEnableAltTab    , true);
            settingsDef.Add(SEBSettings.MessageEnableAltF4     , false);
            settingsDef.Add(SEBSettings.MessageEnableStartMenu , false);
            settingsDef.Add(SEBSettings.MessageEnableRightMouse, false);

            // Default settings for group "Function Keys"
            settingsDef.Add(SEBSettings.MessageEnableF1 , false);
            settingsDef.Add(SEBSettings.MessageEnableF2 , false);
            settingsDef.Add(SEBSettings.MessageEnableF3 , false);
            settingsDef.Add(SEBSettings.MessageEnableF4 , false);
            settingsDef.Add(SEBSettings.MessageEnableF5 , true);
            settingsDef.Add(SEBSettings.MessageEnableF6 , false);
            settingsDef.Add(SEBSettings.MessageEnableF7 , false);
            settingsDef.Add(SEBSettings.MessageEnableF8 , false);
            settingsDef.Add(SEBSettings.MessageEnableF9 , false);
            settingsDef.Add(SEBSettings.MessageEnableF10, false);
            settingsDef.Add(SEBSettings.MessageEnableF11, false);
            settingsDef.Add(SEBSettings.MessageEnableF12, false);

/*
            // Default settings for group "Online exam"
            String s0 = "Seb,../xulrunner/xulrunner.exe";
            String s1 = " -app \"..\\xul_seb\\seb.ini\"";
            String s2 = " -profile \"%LOCALAPPDATA%\\ETH_Zuerich\\xul_seb\\Profiles\"";
            String SebBrowserString = s0 + s1 + s2;

            settingString[StateDef, SEBDefaultSettings.ValueSebBrowser           ] = SebBrowserString;
            settingString[StateDef, SEBDefaultSettings.ValueAutostartProcess     ] = "Seb";
            settingString[StateDef, SEBDefaultSettings.ValuePermittedApplications] = "Calculator,calc.exe;Notepad,notepad.exe;";
*/

            permittedProcessIndex = -1;
            permittedProcessList.Clear();
            permittedProcessData.Clear();

            permittedArgumentIndex = -1;
            permittedArgumentList.Clear();
            permittedArgumentData.Clear();

            prohibitedProcessIndex = -1;
            prohibitedProcessList.Clear();
            prohibitedProcessData.Clear();

            urlFilterRuleIndex = -1;
            urlFilterRuleList.Clear();
            urlFilterRuleData.Clear();

            urlFilterActionIndex = -1;
            urlFilterActionList.Clear();
            urlFilterActionData.Clear();

            embeddedCertificateIndex = -1;
            embeddedCertificateList.Clear();
            embeddedCertificateData.Clear();

            proxyProtocolIndex = -1;

            bypassedProxyIndex = -1;
            bypassedProxyList.Clear();
            bypassedProxyData = "";
        }



        // *****************************************
        // Restore default settings and new settings
        // *****************************************
        public static void RestoreDefaultAndNewSettings()
        {
            // Set all the default values for the Plist structure "settingsNew"
            SEBSettings.BuildUpDefaultSettings();

            // IMPORTANT:
            // Create a second dictionary "new settings"
            // and copy all default settings to the new settings.
            // This must be done BEFORE any config file is loaded
            // and assures that every (key, value) pair is contained
            // in the "new" and "def" dictionaries,
            // even if the loaded "tmp" dictionary does NOT contain every pair.

            SEBSettings.settingsNew.Clear();
            CopySettingsArrays    (SEBSettings.StateDef   , SEBSettings.StateNew);
            CopySettingsDictionary(SEBSettings.settingsDef, SEBSettings.settingsNew);
        }



        // ********************
        // Copy settings arrays
        // ********************
        public static void CopySettingsArrays(int StateSource, int StateTarget)
        {
            // Copy all settings from one array to another
            int value;

            for (value = 1; value <= SEBSettings.ValueNum; value++)
            {
                SEBSettings.settingsStr[StateTarget, value] = SEBSettings.settingsStr[StateSource, value];
                SEBSettings.settingsInt[StateTarget, value] = SEBSettings.settingsInt[StateSource, value];
            }

            return;
        }


        // ************************
        // Copy settings dictionary
        // ************************
        public static void CopySettingsDictionary(Dictionary<string, object> sebSettingsSource,
                                                  Dictionary<string, object> sebSettingsTarget)
        {
            // Copy all settings from one dictionary to another
            // Create a dictionary "target settings".
            // Copy source settings to target settings
            foreach (KeyValuePair<string, object> pair in sebSettingsSource)
            {
                string key   = pair.Key;
                object value = pair.Value;

//              if (key.GetType == Type.Dictionary)
//                  CopySettingsDictionary(sebSettingsSource, sebSettingsTarget, keyNode);

                if  (sebSettingsTarget.ContainsKey(key))
                     sebSettingsTarget[key] = value;
                else sebSettingsTarget.Add(key, value);
            }

            return;
        }


        // *************************
        // Print settings dictionary
        // *************************
        public static void PrintSettingsDictionary(Dictionary<string, object> sebSettings,
                                                   String                     fileName)
        {
            FileStream   fileStream;
            StreamWriter fileWriter;

            // If the .ini file already exists, delete it
            // and write it again from scratch with new data
            if (File.Exists(fileName))
                File.Delete(fileName);

            // Open the file for writing
            fileStream = new FileStream  (fileName, FileMode.OpenOrCreate, FileAccess.Write);
            fileWriter = new StreamWriter(fileStream);

            // Write the header lines
            fileWriter.WriteLine("");
            fileWriter.WriteLine("number of (key, value) pairs = " + sebSettings.Count);
            fileWriter.WriteLine("");

            // Print (key, value) pairs of dictionary to file
            foreach (KeyValuePair<string, object> pair in sebSettings)
            {
                string key   = pair.Key;
                object value = pair.Value;
                string type  = value.GetType().ToString();

//                if (key.GetType == Type.Dictionary)
//                    CopySettingsDictionary(sebSettingsSource, sebSettingsTarget, keyNode);

                fileWriter.WriteLine("key   = " + key);
                fileWriter.WriteLine("value = " + value);
                fileWriter.WriteLine("type  = " + type);
                fileWriter.WriteLine("");
            }

            // Close the file
            fileWriter.Close();
            fileStream.Close();
            return;
        }



        // *********************************************
        // Read the settings from the configuration file
        // *********************************************
        public static bool ReadSebConfigurationFile(String fileName)
        {
            try
            {
                // Read the configuration settings from .seb file.
                // Decrypt the configuration settings.
                // Convert the XML structure into a C# object.

                SEBProtectionController sebProtectionController = new SEBProtectionController();

                //TextReader textReader;
                //String encryptedSettings     = "";
                //String password              = "seb";
                //X509Certificate2 certificate = null;

                //textReader        = new StreamReader(fileName);
                //encryptedSettings = textReader.ReadToEnd();
                //textReader.Close();

                byte[] encryptedSettings = File.ReadAllBytes(fileName);
                String decryptedSettings = "";

                decryptedSettings = sebProtectionController.DecryptSebClientSettings(encryptedSettings);
              //decryptedSettings = decryptedSettings.Trim();
              //decryptedSettings = encryptedSettings;

                SEBSettings.settingsTmp = (Dictionary<string, object>)Plist.readPlistSource(decryptedSettings);
            }
            catch (Exception streamReadException)
            {
                // Let the user know what went wrong
                Console.WriteLine("The .seb file could not be read:");
                Console.WriteLine(streamReadException.Message);
                return false;
            }

            // If the settings could be read from file,
            // recreate "def" settings and "new" settings
            SEBSettings.RestoreDefaultAndNewSettings();

            // And merge "tmp" settings into "new" settings
            SEBSettings.CopySettingsArrays    (SEBSettings.StateTmp   , SEBSettings.StateNew);
            SEBSettings.CopySettingsDictionary(SEBSettings.settingsTmp, SEBSettings.settingsNew);

            return true;
        }



        // ********************************************************
        // Write the settings to the configuration file and save it
        // ********************************************************
        public static bool WriteSebConfigurationFile(String fileName)
        {
            try 
            {
                // If the configuration file already exists, delete it
                // and write it again from scratch with new data
                if (File.Exists(fileName))
                    File.Delete(fileName);

                // Convert the C# object into an XML structure.
                // Encrypt the configuration settings.
                // Write the configuration settings into .seb file.

                SEBProtectionController sebProtectionController = new SEBProtectionController();

                //TextWriter textWriter;
                //String encryptedSettings     = "";
                //String password              = "seb";
                //X509Certificate2 certificate = null;

                Boolean isEncrypted = false;

                if (isEncrypted == true)
                {
                    TextWriter textWriter;
                    String encryptedSettings     = "";
                    String decryptedSettings     = "";
                    String password              = "seb";
                    X509Certificate2 certificate = null;

                    decryptedSettings = Plist.writeXml(SEBSettings.settingsNew);

                  //encryptedSettings = sebController.EncryptWithPassword   (decryptedSettings, password);
                  //encryptedSettings = sebController.EncryptWithCertificate(decryptedSettings, certificate);
                    encryptedSettings = decryptedSettings;

                    textWriter = new StreamWriter(fileName);
                    textWriter.Write(encryptedSettings);
                    textWriter.Close();
                }
                else // unencrypted .xml file
                {
                    Plist.writeXml(SEBSettings.settingsNew, fileName);
                    Plist.writeXml(SEBSettings.settingsNew, "DebugSettingsNew_in_SaveConfigurationFile.xml");
                }
            }
            catch (Exception streamWriteException) 
            {
                // Let the user know what went wrong
                Console.WriteLine("The configuration file could not be written:");
                Console.WriteLine(streamWriteException.Message);
                return false;
            }

            return true;
        }



    }
}
