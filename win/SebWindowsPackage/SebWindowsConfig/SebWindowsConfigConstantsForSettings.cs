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
        // Global variables for SEB settings
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

    } // end of   class     SebWindowsConfigForm
}     // end of   namespace SebWindowsConfig
