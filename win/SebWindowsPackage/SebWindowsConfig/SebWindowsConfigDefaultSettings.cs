using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Serialization;
using SebWindowsClient;
using SebWindowsClient.CryptographyUtils;
using PlistCS;



namespace SebWindowsConfig
{
    public partial class SebWindowsConfigForm : Form
    {

        // **************************
        // Constants for SEB settings
        // **************************

        // The default SEB configuration file
        const String DefaultSebConfigXml = "SebClient.xml";
        const String DefaultSebConfigSeb = "SebClient.seb";

        // The values can be in 3 different states:
        // new, temporary and default values
        const int StateNew = 1;
        const int StateTmp = 2;
        const int StateDef = 3;
        const int StateNum = 3;

        // 5 key/value pairs are not stored in the sebSettings Plist structures,
        // so they must be separately stored in arrays
        const int ValueCryptoIdentity               = 1;
        const int ValueMainBrowserWindowWidth       = 2;
        const int ValueMainBrowserWindowHeight      = 3;
        const int ValueNewBrowserWindowByLinkWidth  = 4;
        const int ValueNewBrowserWindowByLinkHeight = 5;
        const int ValueNum = 5;

        // Group "General"
        const String MessageStartURL             = "startURL";
        const String MessageSebServerURL         = "sebServerURL";
        const String MessageAdminPassword        = "adminPassword";
        const String MessageConfirmAdminPassword = "confirmAdminPassword";
        const String MessageHashedAdminPassword  = "hashedAdminPassword";
        const String MessageAllowQuit            = "allowQuit";
        const String MessageIgnoreQuitPassword   = "ignoreQuitPassword";
        const String MessageQuitPassword         = "quitPassword";
        const String MessageConfirmQuitPassword  = "confirmQuitPassword";
        const String MessageHashedQuitPassword   = "hashedQuitPassword";
        const String MessageExitKey1             = "exitKey1";
        const String MessageExitKey2             = "exitKey2";
        const String MessageExitKey3             = "exitKey3";
        const String MessageSebMode              = "sebMode";

        // Group "Config File"
        const String MessageSebConfigPurpose        = "sebConfigPurpose";
        const String MessageAllowPreferencesWindow  = "allowPreferencesWindow";
        const String MessageCryptoIdentity          = "cryptoIdentity";
        const String MessageSettingsPassword        = "settingsPassword";
        const String MessageConfirmSettingsPassword = "confirmSettingsPassword";
        const String MessageHashedSettingsPassword  = "hashedSettingsPassword";

        // Group "Appearance"
        const String MessageBrowserViewMode              = "browserViewMode";
        const String MessageMainBrowserWindowWidth       = "mainBrowserWindowWidth";
        const String MessageMainBrowserWindowHeight      = "mainBrowserWindowHeight";
        const String MessageMainBrowserWindowPositioning = "mainBrowserWindowPositioning";
        const String MessageEnableBrowserWindowToolbar   = "enableBrowserWindowToolbar";
        const String MessageHideBrowserWindowToolbar     = "hideBrowserWindowToolbar";
        const String MessageShowMenuBar                  = "showMenuBar";
        const String MessageShowTaskBar                  = "showTaskBar";

        // Group "Browser"
        const String MessageNewBrowserWindowByLinkPolicy         = "newBrowserWindowByLinkPolicy";
        const String MessageNewBrowserWindowByScriptPolicy       = "newBrowserWindowByScriptPolicy";
        const String MessageNewBrowserWindowByLinkBlockForeign   = "newBrowserWindowByLinkBlockForeign";
        const String MessageNewBrowserWindowByScriptBlockForeign = "newBrowserWindowByScriptBlockForeign";
        const String MessageNewBrowserWindowByLinkWidth          = "newBrowserWindowByLinkWidth";
        const String MessageNewBrowserWindowByLinkHeight         = "newBrowserWindowByLinkHeight";
        const String MessageNewBrowserWindowByLinkPositioning    = "newBrowserWindowByLinkPositioning";
        const String MessageEnablePlugIns                        = "enablePlugIns";
        const String MessageEnableJava                           = "enableJava";
        const String MessageEnableJavaScript                     = "enableJavaScript";
        const String MessageBlockPopUpWindows                    = "blockPopUpWindows";
        const String MessageAllowBrowsingBackForward             = "allowBrowsingBackForward";
        const String MessageEnableSebBrowser                     = "enableSebBrowser";

        // Group "DownUploads"
        const String MessageAllowDownUploads         = "allowDownUploads";
        const String MessageDownloadDirectoryOSX     = "downloadDirectoryOSX";
        const String MessageDownloadDirectoryWin     = "downloadDirectoryWin";
        const String MessageOpenDownloads            = "openDownloads";
        const String MessageChooseFileToUploadPolicy = "chooseFileToUploadPolicy";
        const String MessageDownloadPDFFiles         = "downloadPDFFiles";

        // Group "Exam"
        const String MessageExamKeySalt        = "examKeySalt";
        const String MessageBrowserExamKey     = "browserExamKey";
        const String MessageCopyBrowserExamKey = "copyBrowserExamKeyToClipboardWhenQuitting";
        const String MessageSendBrowserExamKey = "sendBrowserExamKey";
        const String MessageQuitURL            = "quitURL";

        // Group "Applications"
        const String MessageMonitorProcesses = "monitorProcesses";

        // Group "Applications - Permitted  Processes"
        const String MessagePermittedProcesses        = "permittedProcesses";
        const String MessageAllowSwitchToApplications = "allowSwitchToApplications";
        const String MessageAllowFlashFullscreen      = "allowFlashFullscreen";

        // Group "Applications - Prohibited Processes"
        const String MessageProhibitedProcesses       = "prohibitedProcesses";

        const String MessageActive      = "active";
        const String MessageAutostart   = "autostart";
        const String MessageAutohide    = "autohide";
        const String MessageAllowUser   = "allowUserToChooseApp";
        const String MessageCurrentUser = "currentUser";
        const String MessageStrongKill  = "strongKill";
        const String MessageOS          = "os";
        const String MessageTitle       = "title";
        const String MessageDescription = "description";
        const String MessageExecutable  = "executable";
        const String MessagePath        = "path";
        const String MessageIdentifier  = "identifier";
        const String MessageUser        = "user";
        const String MessageArguments   = "arguments";
        const String MessageArgument    = "argument";

        // Group "Network"
        const String MessageEnableURLFilter        = "enableURLFilter";
        const String MessageEnableURLContentFilter = "enableURLContentFilter";

        // Group "Network - Filter"
        const String MessageURLFilterRules = "URLFilterRules";
        const String MessageExpression     = "expression";
        const String MessageRuleActions    = "ruleActions";
        const String MessageRegex          = "regex";
        const String MessageAction         = "action";

        // Group "Network - Certificates"
        const String MessageEmbedSSLServerCertificate = "EmbedSSLServerCertificate";
        const String MessageEmbedIdentity             = "EmbedIdentity";
        const String MessageEmbeddedCertificates      = "embeddedCertificates";
        const String MessageCertificateData           = "certificateData";
        const String MessageType                      = "type";
        const String MessageName                      = "name";

        // Group "Network - Proxies"
        const String MessageProxySettingsPolicy       = "proxySettingsPolicy";

        const String MessageProxies                     = "proxies";
        const String MessageExceptionsList              = "ExceptionsList";
        const String MessageExcludeSimpleHostnames      = "ExcludeSimpleHostnames";
        const String MessageFTPPassive                  = "FTPPassive";

        const String MessageAutoDiscoveryEnabled        = "AutoDiscoveryEnabled";
        const String MessageAutoConfigurationEnabled    = "AutoConfigurationEnabled";
        const String MessageAutoConfigurationJavaScript = "AutoConfigurationJavaScript";
        const String MessageAutoConfigurationURL        = "AutoConfigurationURL";

        const String MessageAutoDiscovery     = "";
        const String MessageAutoConfiguration = "";
        const String MessageHTTP              = "HTTP";
        const String MessageHTTPS             = "HTTPS";
        const String MessageFTP               = "FTP";
        const String MessageSOCKS             = "SOCKS";
        const String MessageRTSP              = "RTSP";

        const String MessageEnable           = "Enable";
        const String MessagePort             = "Port";
        const String MessageHost             = "Proxy";
        const String MessageRequires = "RequiresPassword";
        const String MessageUsername         = "Username";
        const String MessagePassword         = "Password";

        const String MessageHTTPEnable           = "HTTPEnable";
        const String MessageHTTPPort             = "HTTPPort";
        const String MessageHTTPHost             = "HTTPProxy";
        const String MessageHTTPRequiresPassword = "HTTPRequiresPassword";
        const String MessageHTTPUsername         = "HTTPUsername";
        const String MessageHTTPPassword         = "HTTPPassword";

        const String MessageHTTPSEnable           = "HTTPSEnable";
        const String MessageHTTPSPort             = "HTTPSPort";
        const String MessageHTTPSHost             = "HTTPSProxy";
        const String MessageHTTPSRequiresPassword = "HTTPSRequiresPassword";
        const String MessageHTTPSUsername         = "HTTPSUsername";
        const String MessageHTTPSPassword         = "HTTPSPassword";

        const String MessageFTPEnable           = "FTPEnable";
        const String MessageFTPPort             = "FTPPort";
        const String MessageFTPHost             = "FTPProxy";
        const String MessageFTPRequiresPassword = "FTPRequiresPassword";
        const String MessageFTPUsername         = "FTPUsername";
        const String MessageFTPPassword         = "FTPPassword";

        const String MessageSOCKSEnable           = "SOCKSEnable";
        const String MessageSOCKSPort             = "SOCKSPort";
        const String MessageSOCKSHost             = "SOCKSProxy";
        const String MessageSOCKSRequiresPassword = "SOCKSRequiresPassword";
        const String MessageSOCKSUsername         = "SOCKSUsername";
        const String MessageSOCKSPassword         = "SOCKSPassword";

        const String MessageRTSPEnable           = "RTSPEnable";
        const String MessageRTSPPort             = "RTSPPort";
        const String MessageRTSPHost             = "RTSPProxy";
        const String MessageRTSPRequiresPassword = "RTSPRequiresPassword";
        const String MessageRTSPUsername         = "RTSPUsername";
        const String MessageRTSPPassword         = "RTSPPassword";

        // Group "Security"
        const String MessageSebServicePolicy    = "sebServicePolicy";
        const String MessageAllowVirtualMachine = "allowVirtualMachine";
        const String MessageCreateNewDesktop    = "createNewDesktop";
        const String MessageKillExplorerShell   = "killExplorerShell";
        const String MessageAllowUserSwitching  = "allowUserSwitching";
        const String MessageEnableLogging       = "enableLogging";
        const String MessageLogDirectoryOSX     = "logDirectoryOSX";
        const String MessageLogDirectoryWin     = "logDirectoryWin";

        // Group "Registry"

        // Group "Inside SEB"
        const String MessageInsideSebEnableSwitchUser        = "insideSebEnableSwitchUser";
        const String MessageInsideSebEnableLockThisComputer  = "insideSebEnableLockThisComputer";
        const String MessageInsideSebEnableChangeAPassword   = "insideSebEnableChangeAPassword";
        const String MessageInsideSebEnableStartTaskManager  = "insideSebEnableStartTaskManager";
        const String MessageInsideSebEnableLogOff            = "insideSebEnableLogOff";
        const String MessageInsideSebEnableShutDown          = "insideSebEnableShutDown";
        const String MessageInsideSebEnableEaseOfAccess      = "insideSebEnableEaseOfAccess";
        const String MessageInsideSebEnableVmWareClientShade = "insideSebEnableVmWareClientShade";

        // Group "Outside SEB"
        const String MessageOutsideSebEnableSwitchUser        = "outsideSebEnableSwitchUser";
        const String MessageOutsideSebEnableLockThisComputer  = "outsideSebEnableLockThisComputer";
        const String MessageOutsideSebEnableChangeAPassword   = "outsideSebEnableChangeAPassword";
        const String MessageOutsideSebEnableStartTaskManager  = "outsideSebEnableStartTaskManager";
        const String MessageOutsideSebEnableLogOff            = "outsideSebEnableLogOff";
        const String MessageOutsideSebEnableShutDown          = "outsideSebEnableShutDown";
        const String MessageOutsideSebEnableEaseOfAccess      = "outsideSebEnableEaseOfAccess";
        const String MessageOutsideSebEnableVmWareClientShade = "outsideSebEnableVmWareClientShade";

        // Group "Hooked Keys"
        const String MessageHookKeys = "hookKeys";

        // Group "Special Keys"
        const String MessageEnableEsc        = "enableEsc";
        const String MessageEnableCtrlEsc    = "enableCtrlEsc";
        const String MessageEnableAltEsc     = "enableAltEsc";
        const String MessageEnableAltTab     = "enableAltTab";
        const String MessageEnableAltF4      = "enableAltF4";
        const String MessageEnableStartMenu  = "enableStartMenu";
        const String MessageEnableRightMouse = "enableRightMouse";

        // Group "Function Keys"
        const String MessageEnableF1  = "enableF1";
        const String MessageEnableF2  = "enableF2";
        const String MessageEnableF3  = "enableF3";
        const String MessageEnableF4  = "enableF4";
        const String MessageEnableF5  = "enableF5";
        const String MessageEnableF6  = "enableF6";
        const String MessageEnableF7  = "enableF7";
        const String MessageEnableF8  = "enableF8";
        const String MessageEnableF9  = "enableF9";
        const String MessageEnableF10 = "enableF10";
        const String MessageEnableF11 = "enableF11";
        const String MessageEnableF12 = "enableF12";



        // *********************************
        // Global Variables for SEB settings
        // *********************************

        // Some settings are not stored in Plists but in Arrays
        static String [,] settingString  = new String [StateNum + 1, ValueNum + 1];
        static     int[,] settingInteger = new     int[StateNum + 1, ValueNum + 1];

        // Class SEBSettings contains all settings
        // and is used for importing/exporting the settings
        // from/to a human-readable .xml and an encrypted.seb file format.
        static Dictionary<string, object> sebSettingsNew = new Dictionary<string, object>();
        static Dictionary<string, object> sebSettingsTmp = new Dictionary<string, object>();
        static Dictionary<string, object> sebSettingsDef = new Dictionary<string, object>();

        static SEBProtectionController    sebController  = new SEBProtectionController();

        static int                        permittedProcessIndex;
        static List<object>               permittedProcessList    = new List<object>();
        static Dictionary<string, object> permittedProcessData    = new Dictionary<string, object>();
        static Dictionary<string, object> permittedProcessDataDef = new Dictionary<string, object>();

        static int                        permittedArgumentIndex;
        static List<object>               permittedArgumentList    = new List<object>();
        static Dictionary<string, object> permittedArgumentData    = new Dictionary<string, object>();
        static Dictionary<string, object> permittedArgumentDataDef = new Dictionary<string, object>();

        static int                        prohibitedProcessIndex;
        static List<object>               prohibitedProcessList    = new List<object>();
        static Dictionary<string, object> prohibitedProcessData    = new Dictionary<string, object>();
        static Dictionary<string, object> prohibitedProcessDataDef = new Dictionary<string, object>();

        static int                        urlFilterRuleIndex;
        static List<object>               urlFilterRuleList       = new List<object>();
        static Dictionary<string, object> urlFilterRuleData       = new Dictionary<string, object>();
        static Dictionary<string, object> urlFilterRuleDataDef    = new Dictionary<string, object>();
        static Dictionary<string, object> urlFilterRuleDataStored = new Dictionary<string, object>();

        static int                        urlFilterActionIndex;
        static List<object>               urlFilterActionList       = new List<object>();
        static List<object>               urlFilterActionListDef    = new List<object>();
        static List<object>               urlFilterActionListStored = new List<object>();
        static Dictionary<string, object> urlFilterActionData       = new Dictionary<string, object>();
        static Dictionary<string, object> urlFilterActionDataDef    = new Dictionary<string, object>();
        static Dictionary<string, object> urlFilterActionDataStored = new Dictionary<string, object>();

        static int                        embeddedCertificateIndex;
        static List<object>               embeddedCertificateList    = new List<object>();
        static Dictionary<string, object> embeddedCertificateData    = new Dictionary<string, object>();
        static Dictionary<string, object> embeddedCertificateDataDef = new Dictionary<string, object>();

        static Dictionary<string, object> proxiesData    = new Dictionary<string, object>();
        static Dictionary<string, object> proxiesDataDef = new Dictionary<string, object>();

        static int                        proxyProtocolIndex;
      //static List<object>               proxyProtocolList    = new List<object>();
      //static Dictionary<string, object> proxyProtocolData    = new Dictionary<string, object>();
      //static Dictionary<string, object> proxyProtocolDataDef = new Dictionary<string, object>();

        static int                        bypassedProxyIndex;
        static List<object>               bypassedProxyList    = new List<object>();
        static String                     bypassedProxyData    = "";
        static String                     bypassedProxyDataDef = "";



        // ************************
        // Methods for SEB settings
        // ************************

        // *******************************************************************
        // Set all the default values for the Plist structure "sebSettingsDef"
        // *******************************************************************
        private void InitialiseSEBConfigurationSettings()
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
            sebSettingsDef.Add(MessageStartURL            , "http://www.safeexambrowser.org");
            sebSettingsDef.Add(MessageSebServerURL        , "");
            sebSettingsDef.Add(MessageAdminPassword       , "");
            sebSettingsDef.Add(MessageConfirmAdminPassword, "");
            sebSettingsDef.Add(MessageHashedAdminPassword , "");
            sebSettingsDef.Add(MessageAllowQuit           , true);
            sebSettingsDef.Add(MessageIgnoreQuitPassword  , false);
            sebSettingsDef.Add(MessageQuitPassword        , "");
            sebSettingsDef.Add(MessageConfirmQuitPassword , "");
            sebSettingsDef.Add(MessageHashedQuitPassword  , "");
            sebSettingsDef.Add(MessageExitKey1,  2);
            sebSettingsDef.Add(MessageExitKey2, 10);
            sebSettingsDef.Add(MessageExitKey3,  5);
            sebSettingsDef.Add(MessageSebMode, 0);

            // Default settings for group "Config File"
            sebSettingsDef.Add(MessageSebConfigPurpose       , 0);
            sebSettingsDef.Add(MessageAllowPreferencesWindow , true);
            sebSettingsDef.Add(MessageSettingsPassword       , "");
            sebSettingsDef.Add(MessageConfirmSettingsPassword, "");
            sebSettingsDef.Add(MessageHashedSettingsPassword , "");

            // CryptoIdentity is stored additionally
            settingInteger[StateDef, ValueCryptoIdentity] = 0;
            settingString [StateDef, ValueCryptoIdentity] = "";

            // Default settings for group "Appearance"
            sebSettingsDef.Add(MessageBrowserViewMode             , 0);
            sebSettingsDef.Add(MessageMainBrowserWindowWidth      , "100%");
            sebSettingsDef.Add(MessageMainBrowserWindowHeight     , "100%");
            sebSettingsDef.Add(MessageMainBrowserWindowPositioning, 1);
            sebSettingsDef.Add(MessageEnableBrowserWindowToolbar  , false);
            sebSettingsDef.Add(MessageHideBrowserWindowToolbar    , false);
            sebSettingsDef.Add(MessageShowMenuBar                 , false);
            sebSettingsDef.Add(MessageShowTaskBar                 , true);

            // MainBrowserWindow Width and Height is stored additionally
            settingInteger[StateDef, ValueMainBrowserWindowWidth ] = 1;
            settingInteger[StateDef, ValueMainBrowserWindowHeight] = 1;
            settingString [StateDef, ValueMainBrowserWindowWidth ] = "100%";
            settingString [StateDef, ValueMainBrowserWindowHeight] = "100%";

            // Default settings for group "Browser"
            sebSettingsDef.Add(MessageNewBrowserWindowByLinkPolicy        , 2);
            sebSettingsDef.Add(MessageNewBrowserWindowByScriptPolicy      , 2);
            sebSettingsDef.Add(MessageNewBrowserWindowByLinkBlockForeign  , false);
            sebSettingsDef.Add(MessageNewBrowserWindowByScriptBlockForeign, false);
            sebSettingsDef.Add(MessageNewBrowserWindowByLinkWidth         , "1000");
            sebSettingsDef.Add(MessageNewBrowserWindowByLinkHeight        , "100%");
            sebSettingsDef.Add(MessageNewBrowserWindowByLinkPositioning   , 2);

            sebSettingsDef.Add(MessageEnablePlugIns           , true);
            sebSettingsDef.Add(MessageEnableJava              , false);
            sebSettingsDef.Add(MessageEnableJavaScript        , true);
            sebSettingsDef.Add(MessageBlockPopUpWindows       , false);
            sebSettingsDef.Add(MessageAllowBrowsingBackForward, false);
            sebSettingsDef.Add(MessageEnableSebBrowser        , true);

            // NewBrowserWindow Width and Height is stored additionally
            settingInteger[StateDef, ValueNewBrowserWindowByLinkWidth ] = 3;
            settingInteger[StateDef, ValueNewBrowserWindowByLinkHeight] = 1;
            settingString [StateDef, ValueNewBrowserWindowByLinkWidth ] = "1000";
            settingString [StateDef, ValueNewBrowserWindowByLinkHeight] = "100%";

            // Default settings for group "DownUploads"
            sebSettingsDef.Add(MessageAllowDownUploads        , true);
            sebSettingsDef.Add(MessageDownloadDirectoryOSX    , "~/Downloads");
            sebSettingsDef.Add(MessageDownloadDirectoryWin    , "Desktop");
            sebSettingsDef.Add(MessageOpenDownloads           , false);
            sebSettingsDef.Add(MessageChooseFileToUploadPolicy, 0);
            sebSettingsDef.Add(MessageDownloadPDFFiles        , false);

            // Default settings for group "Exam"
            sebSettingsDef.Add(MessageExamKeySalt       , new Byte[] {});
            sebSettingsDef.Add(MessageBrowserExamKey    , "");
            sebSettingsDef.Add(MessageCopyBrowserExamKey, false);
            sebSettingsDef.Add(MessageSendBrowserExamKey, false);
            sebSettingsDef.Add(MessageQuitURL           , "");

            // Default settings for group "Applications"
            sebSettingsDef.Add(MessageMonitorProcesses         , false);
            sebSettingsDef.Add(MessagePermittedProcesses       , new List<object>());
            sebSettingsDef.Add(MessageAllowSwitchToApplications, false);
            sebSettingsDef.Add(MessageAllowFlashFullscreen     , false);
            sebSettingsDef.Add(MessageProhibitedProcesses      , new List<object>());

            // Default settings for permitted process data
            permittedProcessDataDef.Clear();
            permittedProcessDataDef.Add(MessageActive     , true);
            permittedProcessDataDef.Add(MessageAutostart  , true);
            permittedProcessDataDef.Add(MessageAutohide   , true);
            permittedProcessDataDef.Add(MessageAllowUser  , true);
            permittedProcessDataDef.Add(MessageOS         , IntWin);
            permittedProcessDataDef.Add(MessageTitle      , "");
            permittedProcessDataDef.Add(MessageDescription, "");
            permittedProcessDataDef.Add(MessageExecutable , "");
            permittedProcessDataDef.Add(MessagePath       , "");
            permittedProcessDataDef.Add(MessageIdentifier , "");
            permittedProcessDataDef.Add(MessageArguments  , new List<object>());

            // Default settings for permitted argument data
            permittedArgumentDataDef.Clear();
            permittedArgumentDataDef.Add(MessageActive  , true);
            permittedArgumentDataDef.Add(MessageArgument, "");

            // Default settings for prohibited process data
            prohibitedProcessDataDef.Clear();
            prohibitedProcessDataDef.Add(MessageActive     , true);
            prohibitedProcessDataDef.Add(MessageCurrentUser, true);
            prohibitedProcessDataDef.Add(MessageStrongKill , false);
            prohibitedProcessDataDef.Add(MessageOS         , IntWin);
            prohibitedProcessDataDef.Add(MessageExecutable , "");
            prohibitedProcessDataDef.Add(MessageDescription, "");
            prohibitedProcessDataDef.Add(MessageIdentifier , "");
            prohibitedProcessDataDef.Add(MessageUser       , "");

            // Default settings for group "Network - Filter"
            sebSettingsDef.Add(MessageEnableURLFilter       , false);
            sebSettingsDef.Add(MessageEnableURLContentFilter, false);
            sebSettingsDef.Add(MessageURLFilterRules        , new List<object>());

            // Create a default action
            urlFilterActionDataDef.Clear();
            urlFilterActionDataDef.Add(MessageActive    , true);
            urlFilterActionDataDef.Add(MessageRegex     , false);
            urlFilterActionDataDef.Add(MessageExpression, "");
            urlFilterActionDataDef.Add(MessageAction    , 0);

            // Create a default action list with one entry (the default action)
            urlFilterActionListDef.Clear();
            urlFilterActionListDef.Add(urlFilterActionDataDef);

            // Create a default rule with this default action list.
            // This default rule is used for the "Insert Rule" operation:
            // when a new rule is created, it initially contains one action.
            urlFilterRuleDataDef.Clear();
            urlFilterRuleDataDef.Add(MessageActive     , true);
            urlFilterRuleDataDef.Add(MessageExpression , "Rule");
            urlFilterRuleDataDef.Add(MessageRuleActions, urlFilterActionListDef);

            // Initialise the stored action
            urlFilterActionDataStored.Clear();
            urlFilterActionDataStored.Add(MessageActive    , true);
            urlFilterActionDataStored.Add(MessageRegex     , false);
            urlFilterActionDataStored.Add(MessageExpression, "*");
            urlFilterActionDataStored.Add(MessageAction    , 0);

            // Initialise the stored rule
            urlFilterRuleDataStored.Clear();
            urlFilterRuleDataStored.Add(MessageActive     , true);
            urlFilterRuleDataStored.Add(MessageExpression , "Rule");
            urlFilterRuleDataStored.Add(MessageRuleActions, urlFilterActionListStored);

            // Default settings for group "Network - Certificates"
            sebSettingsDef.Add(MessageEmbeddedCertificates, new List<object>());

            embeddedCertificateDataDef.Clear();
            embeddedCertificateDataDef.Add(MessageCertificateData, "");
            embeddedCertificateDataDef.Add(MessageType           , 0);
            embeddedCertificateDataDef.Add(MessageName           , "");

            // Default settings for group "Network - Proxies"
            proxiesDataDef.Clear();

            proxiesDataDef.Add(MessageExceptionsList             , new List<object>());
            proxiesDataDef.Add(MessageExcludeSimpleHostnames     , true);
            proxiesDataDef.Add(MessageAutoDiscoveryEnabled       , false);
            proxiesDataDef.Add(MessageAutoConfigurationEnabled   , false);
            proxiesDataDef.Add(MessageAutoConfigurationJavaScript, "");
            proxiesDataDef.Add(MessageAutoConfigurationURL       , "");
            proxiesDataDef.Add(MessageFTPPassive                 , true);

            proxiesDataDef.Add(MessageHTTPEnable          , false);
            proxiesDataDef.Add(MessageHTTPPort            , 0);
            proxiesDataDef.Add(MessageHTTPHost           , "");
            proxiesDataDef.Add(MessageHTTPRequiresPassword, false);
            proxiesDataDef.Add(MessageHTTPUsername        , "");
            proxiesDataDef.Add(MessageHTTPPassword        , "");

            proxiesDataDef.Add(MessageHTTPSEnable          , false);
            proxiesDataDef.Add(MessageHTTPSPort            , 0);
            proxiesDataDef.Add(MessageHTTPSHost           , "");
            proxiesDataDef.Add(MessageHTTPSRequiresPassword, false);
            proxiesDataDef.Add(MessageHTTPSUsername        , "");
            proxiesDataDef.Add(MessageHTTPSPassword        , "");

            proxiesDataDef.Add(MessageFTPEnable          , false);
            proxiesDataDef.Add(MessageFTPPort            , 0);
            proxiesDataDef.Add(MessageFTPHost           , "");
            proxiesDataDef.Add(MessageFTPRequiresPassword, false);
            proxiesDataDef.Add(MessageFTPUsername        , "");
            proxiesDataDef.Add(MessageFTPPassword        , "");

            proxiesDataDef.Add(MessageSOCKSEnable          , false);
            proxiesDataDef.Add(MessageSOCKSPort            , 0);
            proxiesDataDef.Add(MessageSOCKSHost           , "");
            proxiesDataDef.Add(MessageSOCKSRequiresPassword, false);
            proxiesDataDef.Add(MessageSOCKSUsername        , "");
            proxiesDataDef.Add(MessageSOCKSPassword        , "");

            proxiesDataDef.Add(MessageRTSPEnable          , false);
            proxiesDataDef.Add(MessageRTSPPort            , 0);
            proxiesDataDef.Add(MessageRTSPHost           , "");
            proxiesDataDef.Add(MessageRTSPRequiresPassword, false);
            proxiesDataDef.Add(MessageRTSPUsername        , "");
            proxiesDataDef.Add(MessageRTSPPassword        , "");

            bypassedProxyDataDef = "";

            sebSettingsDef.Add(MessageProxySettingsPolicy, 0);
            sebSettingsDef.Add(MessageProxies            , proxiesDataDef);

            // Default settings for group "Security"
            sebSettingsDef.Add(MessageSebServicePolicy   , 2);
            sebSettingsDef.Add(MessageAllowVirtualMachine, false);
            sebSettingsDef.Add(MessageCreateNewDesktop   , true);
            sebSettingsDef.Add(MessageKillExplorerShell  , false);
            sebSettingsDef.Add(MessageAllowUserSwitching , true);
            sebSettingsDef.Add(MessageEnableLogging      , false);
            sebSettingsDef.Add(MessageLogDirectoryOSX    , "~/Documents");
            sebSettingsDef.Add(MessageLogDirectoryWin    , "My Documents");

            // Default settings for group "Inside SEB"
            sebSettingsDef.Add(MessageInsideSebEnableSwitchUser       , false);
            sebSettingsDef.Add(MessageInsideSebEnableLockThisComputer , false);
            sebSettingsDef.Add(MessageInsideSebEnableChangeAPassword  , false);
            sebSettingsDef.Add(MessageInsideSebEnableStartTaskManager , false);
            sebSettingsDef.Add(MessageInsideSebEnableLogOff           , false);
            sebSettingsDef.Add(MessageInsideSebEnableShutDown         , false);
            sebSettingsDef.Add(MessageInsideSebEnableEaseOfAccess     , false);
            sebSettingsDef.Add(MessageInsideSebEnableVmWareClientShade, false);

            // Default settings for group "Outside SEB"
            sebSettingsDef.Add(MessageOutsideSebEnableSwitchUser       , true);
            sebSettingsDef.Add(MessageOutsideSebEnableLockThisComputer , true);
            sebSettingsDef.Add(MessageOutsideSebEnableChangeAPassword  , true);
            sebSettingsDef.Add(MessageOutsideSebEnableStartTaskManager , true);
            sebSettingsDef.Add(MessageOutsideSebEnableLogOff           , true);
            sebSettingsDef.Add(MessageOutsideSebEnableShutDown         , true);
            sebSettingsDef.Add(MessageOutsideSebEnableEaseOfAccess     , true);
            sebSettingsDef.Add(MessageOutsideSebEnableVmWareClientShade, true);

            // Default settings for group "Hooked Keys"
            sebSettingsDef.Add(MessageHookKeys, true);

            // Default settings for group "Special Keys"
            sebSettingsDef.Add(MessageEnableEsc       , false);
            sebSettingsDef.Add(MessageEnableCtrlEsc   , false);
            sebSettingsDef.Add(MessageEnableAltEsc    , false);
            sebSettingsDef.Add(MessageEnableAltTab    , true);
            sebSettingsDef.Add(MessageEnableAltF4     , false);
            sebSettingsDef.Add(MessageEnableStartMenu , false);
            sebSettingsDef.Add(MessageEnableRightMouse, false);

            // Default settings for group "Function Keys"
            sebSettingsDef.Add(MessageEnableF1 , false);
            sebSettingsDef.Add(MessageEnableF2 , false);
            sebSettingsDef.Add(MessageEnableF3 , false);
            sebSettingsDef.Add(MessageEnableF4 , false);
            sebSettingsDef.Add(MessageEnableF5 , true);
            sebSettingsDef.Add(MessageEnableF6 , false);
            sebSettingsDef.Add(MessageEnableF7 , false);
            sebSettingsDef.Add(MessageEnableF8 , false);
            sebSettingsDef.Add(MessageEnableF9 , false);
            sebSettingsDef.Add(MessageEnableF10, false);
            sebSettingsDef.Add(MessageEnableF11, false);
            sebSettingsDef.Add(MessageEnableF12, false);
/*
            // Default settings for group "Online exam"
            String s0 = "Seb,../xulrunner/xulrunner.exe";
            String s1 = " -app \"..\\xul_seb\\seb.ini\"";
            String s2 = " -profile \"%LOCALAPPDATA%\\ETH_Zuerich\\xul_seb\\Profiles\"";
            String SebBrowserString = s0 + s1 + s2;

            settingString[StateDef, ValueSebBrowser           ] = SebBrowserString;
            settingString[StateDef, ValueAutostartProcess     ] = "Seb";
            settingString[StateDef, ValuePermittedApplications] = "Calculator,calc.exe;Notepad,notepad.exe;";
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
          //proxyProtocolList.Clear();
          //proxyProtocolData.Clear();

            bypassedProxyIndex = -1;
            bypassedProxyList.Clear();
            bypassedProxyData = "";
        }

    } // end of   class     SebWindowsConfigForm
}     // end of   namespace SebWindowsConfig
