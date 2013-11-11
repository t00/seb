using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SebWindowsClient.CryptographyUtils;



namespace SebWindowsClient.ConfigurationUtils
{
    public class SEBDefaultSettings
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

        public const String MessageEnable           = "Enable";
        public const String MessagePort             = "Port";
        public const String MessageHost             = "Proxy";
        public const String MessageRequiresPassword = "RequiresPassword";
        public const String MessageUsername         = "Username";
        public const String MessagePassword         = "Password";

        public const String MessageHTTPEnable           = "HTTPEnable";
        public const String MessageHTTPPort             = "HTTPPort";
        public const String MessageHTTPHost             = "HTTPProxy";
        public const String MessageHTTPRequiresPassword = "HTTPRequiresPassword";
        public const String MessageHTTPUsername         = "HTTPUsername";
        public const String MessageHTTPPassword         = "HTTPPassword";

        public const String MessageHTTPSEnable           = "HTTPSEnable";
        public const String MessageHTTPSPort             = "HTTPSPort";
        public const String MessageHTTPSHost             = "HTTPSProxy";
        public const String MessageHTTPSRequiresPassword = "HTTPSRequiresPassword";
        public const String MessageHTTPSUsername         = "HTTPSUsername";
        public const String MessageHTTPSPassword         = "HTTPSPassword";

        public const String MessageFTPEnable           = "FTPEnable";
        public const String MessageFTPPort             = "FTPPort";
        public const String MessageFTPHost             = "FTPProxy";
        public const String MessageFTPRequiresPassword = "FTPRequiresPassword";
        public const String MessageFTPUsername         = "FTPUsername";
        public const String MessageFTPPassword         = "FTPPassword";

        public const String MessageSOCKSEnable           = "SOCKSEnable";
        public const String MessageSOCKSPort             = "SOCKSPort";
        public const String MessageSOCKSHost             = "SOCKSProxy";
        public const String MessageSOCKSRequiresPassword = "SOCKSRequiresPassword";
        public const String MessageSOCKSUsername         = "SOCKSUsername";
        public const String MessageSOCKSPassword         = "SOCKSPassword";

        public const String MessageRTSPEnable           = "RTSPEnable";
        public const String MessageRTSPPort             = "RTSPPort";
        public const String MessageRTSPHost             = "RTSPProxy";
        public const String MessageRTSPRequiresPassword = "RTSPRequiresPassword";
        public const String MessageRTSPUsername         = "RTSPUsername";
        public const String MessageRTSPPassword         = "RTSPPassword";

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
        public static String [,] settingString  = new String [StateNum + 1, ValueNum + 1];
        public static     int[,] settingInteger = new     int[StateNum + 1, ValueNum + 1];

        // Class SEBSettings contains all settings
        // and is used for importing/exporting the settings
        // from/to a human-readable .xml and an encrypted.seb file format.
        public static Dictionary<string, object> sebSettingsNew = new Dictionary<string, object>();
        public static Dictionary<string, object> sebSettingsTmp = new Dictionary<string, object>();
        public static Dictionary<string, object> sebSettingsDef = new Dictionary<string, object>();

        public static SEBProtectionController    sebController  = new SEBProtectionController();

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
        public static void InitialiseSEBDefaultSettings()
        {
            // Initialise the global arrays
            for (int state = 1; state <= StateNum; state++)
            for (int value = 1; value <= ValueNum; value++)
            {
                settingInteger[state, value] = 0;
                settingString [state, value] = "";
            }

            // Initialise the default settings Plist
            sebSettingsDef.Clear();

            // Default settings for group "General"
            sebSettingsDef.Add(SEBDefaultSettings.MessageStartURL            , "http://www.safeexambrowser.org");
            sebSettingsDef.Add(SEBDefaultSettings.MessageSebServerURL        , "");
            sebSettingsDef.Add(SEBDefaultSettings.MessageAdminPassword       , "");
            sebSettingsDef.Add(SEBDefaultSettings.MessageConfirmAdminPassword, "");
            sebSettingsDef.Add(SEBDefaultSettings.MessageHashedAdminPassword , "");
            sebSettingsDef.Add(SEBDefaultSettings.MessageAllowQuit           , true);
            sebSettingsDef.Add(SEBDefaultSettings.MessageIgnoreQuitPassword  , false);
            sebSettingsDef.Add(SEBDefaultSettings.MessageQuitPassword        , "");
            sebSettingsDef.Add(SEBDefaultSettings.MessageConfirmQuitPassword , "");
            sebSettingsDef.Add(SEBDefaultSettings.MessageHashedQuitPassword  , "");
            sebSettingsDef.Add(SEBDefaultSettings.MessageExitKey1,  2);
            sebSettingsDef.Add(SEBDefaultSettings.MessageExitKey2, 10);
            sebSettingsDef.Add(SEBDefaultSettings.MessageExitKey3,  5);
            sebSettingsDef.Add(SEBDefaultSettings.MessageSebMode, 0);

            // Default settings for group "Config File"
            sebSettingsDef.Add(SEBDefaultSettings.MessageSebConfigPurpose       , 0);
            sebSettingsDef.Add(SEBDefaultSettings.MessageAllowPreferencesWindow , true);
            sebSettingsDef.Add(SEBDefaultSettings.MessageSettingsPassword       , "");
            sebSettingsDef.Add(SEBDefaultSettings.MessageConfirmSettingsPassword, "");
            sebSettingsDef.Add(SEBDefaultSettings.MessageHashedSettingsPassword , "");

            // CryptoIdentity is stored additionally
            settingInteger[StateDef, SEBDefaultSettings.ValueCryptoIdentity] = 0;
            settingString [StateDef, SEBDefaultSettings.ValueCryptoIdentity] = "";

            // Default settings for group "Appearance"
            sebSettingsDef.Add(SEBDefaultSettings.MessageBrowserViewMode             , 0);
            sebSettingsDef.Add(SEBDefaultSettings.MessageMainBrowserWindowWidth      , "100%");
            sebSettingsDef.Add(SEBDefaultSettings.MessageMainBrowserWindowHeight     , "100%");
            sebSettingsDef.Add(SEBDefaultSettings.MessageMainBrowserWindowPositioning, 1);
            sebSettingsDef.Add(SEBDefaultSettings.MessageEnableBrowserWindowToolbar  , false);
            sebSettingsDef.Add(SEBDefaultSettings.MessageHideBrowserWindowToolbar    , false);
            sebSettingsDef.Add(SEBDefaultSettings.MessageShowMenuBar                 , false);
            sebSettingsDef.Add(SEBDefaultSettings.MessageShowTaskBar                 , true);
            sebSettingsDef.Add(SEBDefaultSettings.MessageTaskBarHeight               , 40);

            // MainBrowserWindow Width and Height is stored additionally
            settingInteger[StateDef, SEBDefaultSettings.ValueMainBrowserWindowWidth ] = 1;
            settingInteger[StateDef, SEBDefaultSettings.ValueMainBrowserWindowHeight] = 1;
            settingString [StateDef, SEBDefaultSettings.ValueMainBrowserWindowWidth ] = "100%";
            settingString [StateDef, SEBDefaultSettings.ValueMainBrowserWindowHeight] = "100%";

            // Default settings for group "Browser"
            sebSettingsDef.Add(SEBDefaultSettings.MessageNewBrowserWindowByLinkPolicy        , 2);
            sebSettingsDef.Add(SEBDefaultSettings.MessageNewBrowserWindowByScriptPolicy      , 2);
            sebSettingsDef.Add(SEBDefaultSettings.MessageNewBrowserWindowByLinkBlockForeign  , false);
            sebSettingsDef.Add(SEBDefaultSettings.MessageNewBrowserWindowByScriptBlockForeign, false);
            sebSettingsDef.Add(SEBDefaultSettings.MessageNewBrowserWindowByLinkWidth         , "1000");
            sebSettingsDef.Add(SEBDefaultSettings.MessageNewBrowserWindowByLinkHeight        , "100%");
            sebSettingsDef.Add(SEBDefaultSettings.MessageNewBrowserWindowByLinkPositioning   , 2);

            sebSettingsDef.Add(SEBDefaultSettings.MessageEnablePlugIns           , true);
            sebSettingsDef.Add(SEBDefaultSettings.MessageEnableJava              , false);
            sebSettingsDef.Add(SEBDefaultSettings.MessageEnableJavaScript        , true);
            sebSettingsDef.Add(SEBDefaultSettings.MessageBlockPopUpWindows       , false);
            sebSettingsDef.Add(SEBDefaultSettings.MessageAllowBrowsingBackForward, false);
            sebSettingsDef.Add(SEBDefaultSettings.MessageEnableSebBrowser        , true);

            // NewBrowserWindow Width and Height is stored additionally
            settingInteger[StateDef, SEBDefaultSettings.ValueNewBrowserWindowByLinkWidth ] = 3;
            settingInteger[StateDef, SEBDefaultSettings.ValueNewBrowserWindowByLinkHeight] = 1;
            settingString [StateDef, SEBDefaultSettings.ValueNewBrowserWindowByLinkWidth ] = "1000";
            settingString [StateDef, SEBDefaultSettings.ValueNewBrowserWindowByLinkHeight] = "100%";

            // Default settings for group "DownUploads"
            sebSettingsDef.Add(SEBDefaultSettings.MessageAllowDownUploads        , true);
            sebSettingsDef.Add(SEBDefaultSettings.MessageDownloadDirectoryOSX    , "~/Downloads");
            sebSettingsDef.Add(SEBDefaultSettings.MessageDownloadDirectoryWin    , "Desktop");
            sebSettingsDef.Add(SEBDefaultSettings.MessageOpenDownloads           , false);
            sebSettingsDef.Add(SEBDefaultSettings.MessageChooseFileToUploadPolicy, 0);
            sebSettingsDef.Add(SEBDefaultSettings.MessageDownloadPDFFiles        , false);

            // Default settings for group "Exam"
            sebSettingsDef.Add(SEBDefaultSettings.MessageExamKeySalt       , new Byte[] {});
            sebSettingsDef.Add(SEBDefaultSettings.MessageBrowserExamKey    , "");
            sebSettingsDef.Add(SEBDefaultSettings.MessageCopyBrowserExamKey, false);
            sebSettingsDef.Add(SEBDefaultSettings.MessageSendBrowserExamKey, false);
            sebSettingsDef.Add(SEBDefaultSettings.MessageQuitURL           , "");

            // Default settings for group "Applications"
            sebSettingsDef.Add(SEBDefaultSettings.MessageMonitorProcesses         , false);
            sebSettingsDef.Add(SEBDefaultSettings.MessagePermittedProcesses       , new List<object>());
            sebSettingsDef.Add(SEBDefaultSettings.MessageAllowSwitchToApplications, false);
            sebSettingsDef.Add(SEBDefaultSettings.MessageAllowFlashFullscreen     , false);
            sebSettingsDef.Add(SEBDefaultSettings.MessageProhibitedProcesses      , new List<object>());

            // Default settings for permitted process data
            permittedProcessDataDef.Clear();
            permittedProcessDataDef.Add(SEBDefaultSettings.MessageActive     , true);
            permittedProcessDataDef.Add(SEBDefaultSettings.MessageAutostart  , true);
            permittedProcessDataDef.Add(SEBDefaultSettings.MessageAutohide   , true);
            permittedProcessDataDef.Add(SEBDefaultSettings.MessageAllowUser  , true);
            permittedProcessDataDef.Add(SEBDefaultSettings.MessageOS         , IntWin);
            permittedProcessDataDef.Add(SEBDefaultSettings.MessageTitle      , "");
            permittedProcessDataDef.Add(SEBDefaultSettings.MessageDescription, "");
            permittedProcessDataDef.Add(SEBDefaultSettings.MessageExecutable , "");
            permittedProcessDataDef.Add(SEBDefaultSettings.MessagePath       , "");
            permittedProcessDataDef.Add(SEBDefaultSettings.MessageIdentifier , "");
            permittedProcessDataDef.Add(SEBDefaultSettings.MessageArguments  , new List<object>());

            // Default settings for permitted argument data
            permittedArgumentDataDef.Clear();
            permittedArgumentDataDef.Add(SEBDefaultSettings.MessageActive  , true);
            permittedArgumentDataDef.Add(SEBDefaultSettings.MessageArgument, "");

            // Default settings for prohibited process data
            prohibitedProcessDataDef.Clear();
            prohibitedProcessDataDef.Add(SEBDefaultSettings.MessageActive     , true);
            prohibitedProcessDataDef.Add(SEBDefaultSettings.MessageCurrentUser, true);
            prohibitedProcessDataDef.Add(SEBDefaultSettings.MessageStrongKill , false);
            prohibitedProcessDataDef.Add(SEBDefaultSettings.MessageOS         , IntWin);
            prohibitedProcessDataDef.Add(SEBDefaultSettings.MessageExecutable , "");
            prohibitedProcessDataDef.Add(SEBDefaultSettings.MessageDescription, "");
            prohibitedProcessDataDef.Add(SEBDefaultSettings.MessageIdentifier , "");
            prohibitedProcessDataDef.Add(SEBDefaultSettings.MessageUser       , "");

            // Default settings for group "Network - Filter"
            sebSettingsDef.Add(SEBDefaultSettings.MessageEnableURLFilter       , false);
            sebSettingsDef.Add(SEBDefaultSettings.MessageEnableURLContentFilter, false);
            sebSettingsDef.Add(SEBDefaultSettings.MessageURLFilterRules        , new List<object>());

            // Create a default action
            urlFilterActionDataDef.Clear();
            urlFilterActionDataDef.Add(SEBDefaultSettings.MessageActive    , true);
            urlFilterActionDataDef.Add(SEBDefaultSettings.MessageRegex     , false);
            urlFilterActionDataDef.Add(SEBDefaultSettings.MessageExpression, "");
            urlFilterActionDataDef.Add(SEBDefaultSettings.MessageAction    , 0);

            // Create a default action list with one entry (the default action)
            urlFilterActionListDef.Clear();
            urlFilterActionListDef.Add(urlFilterActionDataDef);

            // Create a default rule with this default action list.
            // This default rule is used for the "Insert Rule" operation:
            // when a new rule is created, it initially contains one action.
            urlFilterRuleDataDef.Clear();
            urlFilterRuleDataDef.Add(SEBDefaultSettings.MessageActive     , true);
            urlFilterRuleDataDef.Add(SEBDefaultSettings.MessageExpression , "Rule");
            urlFilterRuleDataDef.Add(SEBDefaultSettings.MessageRuleActions, urlFilterActionListDef);

            // Initialise the stored action
            urlFilterActionDataStored.Clear();
            urlFilterActionDataStored.Add(SEBDefaultSettings.MessageActive    , true);
            urlFilterActionDataStored.Add(SEBDefaultSettings.MessageRegex     , false);
            urlFilterActionDataStored.Add(SEBDefaultSettings.MessageExpression, "*");
            urlFilterActionDataStored.Add(SEBDefaultSettings.MessageAction    , 0);

            // Initialise the stored rule
            urlFilterRuleDataStored.Clear();
            urlFilterRuleDataStored.Add(SEBDefaultSettings.MessageActive     , true);
            urlFilterRuleDataStored.Add(SEBDefaultSettings.MessageExpression , "Rule");
            urlFilterRuleDataStored.Add(SEBDefaultSettings.MessageRuleActions, urlFilterActionListStored);

            // Default settings for group "Network - Certificates"
            sebSettingsDef.Add(SEBDefaultSettings.MessageEmbeddedCertificates, new List<object>());

            embeddedCertificateDataDef.Clear();
            embeddedCertificateDataDef.Add(SEBDefaultSettings.MessageCertificateData, "");
            embeddedCertificateDataDef.Add(SEBDefaultSettings.MessageType           , 0);
            embeddedCertificateDataDef.Add(SEBDefaultSettings.MessageName           , "");

            // Default settings for group "Network - Proxies"
            proxiesDataDef.Clear();

            proxiesDataDef.Add(SEBDefaultSettings.MessageExceptionsList             , new List<object>());
            proxiesDataDef.Add(SEBDefaultSettings.MessageExcludeSimpleHostnames     , true);
            proxiesDataDef.Add(SEBDefaultSettings.MessageAutoDiscoveryEnabled       , false);
            proxiesDataDef.Add(SEBDefaultSettings.MessageAutoConfigurationEnabled   , false);
            proxiesDataDef.Add(SEBDefaultSettings.MessageAutoConfigurationJavaScript, "");
            proxiesDataDef.Add(SEBDefaultSettings.MessageAutoConfigurationURL       , "");
            proxiesDataDef.Add(SEBDefaultSettings.MessageFTPPassive                 , true);

            proxiesDataDef.Add(SEBDefaultSettings.MessageHTTPEnable          , false);
            proxiesDataDef.Add(SEBDefaultSettings.MessageHTTPPort            , 0);
            proxiesDataDef.Add(SEBDefaultSettings.MessageHTTPHost            , "");
            proxiesDataDef.Add(SEBDefaultSettings.MessageHTTPRequiresPassword, false);
            proxiesDataDef.Add(SEBDefaultSettings.MessageHTTPUsername        , "");
            proxiesDataDef.Add(SEBDefaultSettings.MessageHTTPPassword        , "");

            proxiesDataDef.Add(SEBDefaultSettings.MessageHTTPSEnable          , false);
            proxiesDataDef.Add(SEBDefaultSettings.MessageHTTPSPort            , 0);
            proxiesDataDef.Add(SEBDefaultSettings.MessageHTTPSHost            , "");
            proxiesDataDef.Add(SEBDefaultSettings.MessageHTTPSRequiresPassword, false);
            proxiesDataDef.Add(SEBDefaultSettings.MessageHTTPSUsername        , "");
            proxiesDataDef.Add(SEBDefaultSettings.MessageHTTPSPassword        , "");

            proxiesDataDef.Add(SEBDefaultSettings.MessageFTPEnable          , false);
            proxiesDataDef.Add(SEBDefaultSettings.MessageFTPPort            , 0);
            proxiesDataDef.Add(SEBDefaultSettings.MessageFTPHost            , "");
            proxiesDataDef.Add(SEBDefaultSettings.MessageFTPRequiresPassword, false);
            proxiesDataDef.Add(SEBDefaultSettings.MessageFTPUsername        , "");
            proxiesDataDef.Add(SEBDefaultSettings.MessageFTPPassword        , "");

            proxiesDataDef.Add(SEBDefaultSettings.MessageSOCKSEnable          , false);
            proxiesDataDef.Add(SEBDefaultSettings.MessageSOCKSPort            , 0);
            proxiesDataDef.Add(SEBDefaultSettings.MessageSOCKSHost            , "");
            proxiesDataDef.Add(SEBDefaultSettings.MessageSOCKSRequiresPassword, false);
            proxiesDataDef.Add(SEBDefaultSettings.MessageSOCKSUsername        , "");
            proxiesDataDef.Add(SEBDefaultSettings.MessageSOCKSPassword        , "");

            proxiesDataDef.Add(SEBDefaultSettings.MessageRTSPEnable          , false);
            proxiesDataDef.Add(SEBDefaultSettings.MessageRTSPPort            , 0);
            proxiesDataDef.Add(SEBDefaultSettings.MessageRTSPHost            , "");
            proxiesDataDef.Add(SEBDefaultSettings.MessageRTSPRequiresPassword, false);
            proxiesDataDef.Add(SEBDefaultSettings.MessageRTSPUsername        , "");
            proxiesDataDef.Add(SEBDefaultSettings.MessageRTSPPassword        , "");

            bypassedProxyDataDef = "";

            sebSettingsDef.Add(SEBDefaultSettings.MessageProxySettingsPolicy, 0);
            sebSettingsDef.Add(SEBDefaultSettings.MessageProxies            , proxiesDataDef);

            // Default settings for group "Security"
            sebSettingsDef.Add(SEBDefaultSettings.MessageSebServicePolicy   , 2);
            sebSettingsDef.Add(SEBDefaultSettings.MessageAllowVirtualMachine, false);
            sebSettingsDef.Add(SEBDefaultSettings.MessageCreateNewDesktop   , true);
            sebSettingsDef.Add(SEBDefaultSettings.MessageKillExplorerShell  , false);
            sebSettingsDef.Add(SEBDefaultSettings.MessageAllowUserSwitching , true);
            sebSettingsDef.Add(SEBDefaultSettings.MessageEnableLogging      , false);
            sebSettingsDef.Add(SEBDefaultSettings.MessageLogDirectoryOSX    , "~/Documents");
            sebSettingsDef.Add(SEBDefaultSettings.MessageLogDirectoryWin    , "My Documents");

            // Default settings for group "Inside SEB"
            sebSettingsDef.Add(SEBDefaultSettings.MessageInsideSebEnableSwitchUser       , false);
            sebSettingsDef.Add(SEBDefaultSettings.MessageInsideSebEnableLockThisComputer , false);
            sebSettingsDef.Add(SEBDefaultSettings.MessageInsideSebEnableChangeAPassword  , false);
            sebSettingsDef.Add(SEBDefaultSettings.MessageInsideSebEnableStartTaskManager , false);
            sebSettingsDef.Add(SEBDefaultSettings.MessageInsideSebEnableLogOff           , false);
            sebSettingsDef.Add(SEBDefaultSettings.MessageInsideSebEnableShutDown         , false);
            sebSettingsDef.Add(SEBDefaultSettings.MessageInsideSebEnableEaseOfAccess     , false);
            sebSettingsDef.Add(SEBDefaultSettings.MessageInsideSebEnableVmWareClientShade, false);

            // Default settings for group "Outside SEB"
            sebSettingsDef.Add(SEBDefaultSettings.MessageOutsideSebEnableSwitchUser       , true);
            sebSettingsDef.Add(SEBDefaultSettings.MessageOutsideSebEnableLockThisComputer , true);
            sebSettingsDef.Add(SEBDefaultSettings.MessageOutsideSebEnableChangeAPassword  , true);
            sebSettingsDef.Add(SEBDefaultSettings.MessageOutsideSebEnableStartTaskManager , true);
            sebSettingsDef.Add(SEBDefaultSettings.MessageOutsideSebEnableLogOff           , true);
            sebSettingsDef.Add(SEBDefaultSettings.MessageOutsideSebEnableShutDown         , true);
            sebSettingsDef.Add(SEBDefaultSettings.MessageOutsideSebEnableEaseOfAccess     , true);
            sebSettingsDef.Add(SEBDefaultSettings.MessageOutsideSebEnableVmWareClientShade, true);

            // Default settings for group "Hooked Keys"
            sebSettingsDef.Add(SEBDefaultSettings.MessageHookKeys, true);

            // Default settings for group "Special Keys"
            sebSettingsDef.Add(SEBDefaultSettings.MessageEnableEsc       , false);
            sebSettingsDef.Add(SEBDefaultSettings.MessageEnableCtrlEsc   , false);
            sebSettingsDef.Add(SEBDefaultSettings.MessageEnableAltEsc    , false);
            sebSettingsDef.Add(SEBDefaultSettings.MessageEnableAltTab    , true);
            sebSettingsDef.Add(SEBDefaultSettings.MessageEnableAltF4     , false);
            sebSettingsDef.Add(SEBDefaultSettings.MessageEnableStartMenu , false);
            sebSettingsDef.Add(SEBDefaultSettings.MessageEnableRightMouse, false);

            // Default settings for group "Function Keys"
            sebSettingsDef.Add(SEBDefaultSettings.MessageEnableF1 , false);
            sebSettingsDef.Add(SEBDefaultSettings.MessageEnableF2 , false);
            sebSettingsDef.Add(SEBDefaultSettings.MessageEnableF3 , false);
            sebSettingsDef.Add(SEBDefaultSettings.MessageEnableF4 , false);
            sebSettingsDef.Add(SEBDefaultSettings.MessageEnableF5 , true);
            sebSettingsDef.Add(SEBDefaultSettings.MessageEnableF6 , false);
            sebSettingsDef.Add(SEBDefaultSettings.MessageEnableF7 , false);
            sebSettingsDef.Add(SEBDefaultSettings.MessageEnableF8 , false);
            sebSettingsDef.Add(SEBDefaultSettings.MessageEnableF9 , false);
            sebSettingsDef.Add(SEBDefaultSettings.MessageEnableF10, false);
            sebSettingsDef.Add(SEBDefaultSettings.MessageEnableF11, false);
            sebSettingsDef.Add(SEBDefaultSettings.MessageEnableF12, false);

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



    }
}
