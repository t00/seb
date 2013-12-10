using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using SebWindowsClient.CryptographyUtils;
using PlistCS;

using ListObj  = System.Collections.Generic.List                <object>;
using DictObj  = System.Collections.Generic.Dictionary  <string, object>;
using KeyValue = System.Collections.Generic.KeyValuePair<string, object>;



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

        // Keys not belonging to any group
        public const String MessageOriginatorVersion = "originatorVersion";

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
        public static String [] strArrayDefault = new String [ValueNum + 1];
        public static String [] strArrayCurrent = new String [ValueNum + 1];

        public static     int[] intArrayDefault = new     int[ValueNum + 1];
        public static     int[] intArrayCurrent = new     int[ValueNum + 1];

        // Class SEBSettings contains all settings
        // and is used for importing/exporting the settings
        // from/to a human-readable .xml and an encrypted.seb file format.
        public static DictObj settingsDefault = new DictObj();
        public static DictObj settingsCurrent = new DictObj();

        public static int     permittedProcessIndex;
        public static ListObj permittedProcessList          = new ListObj();
        public static DictObj permittedProcessData          = new DictObj();
        public static DictObj permittedProcessDataDefault   = new DictObj();
        public static DictObj permittedProcessDataXulRunner = new DictObj();

        public static int     permittedArgumentIndex;
        public static ListObj permittedArgumentList           = new ListObj();
        public static DictObj permittedArgumentData           = new DictObj();
        public static DictObj permittedArgumentDataDefault    = new DictObj();
        public static DictObj permittedArgumentDataXulRunner1 = new DictObj();
        public static DictObj permittedArgumentDataXulRunner2 = new DictObj();
        public static ListObj permittedArgumentListXulRunner  = new ListObj();

        public static int     prohibitedProcessIndex;
        public static ListObj prohibitedProcessList        = new ListObj();
        public static DictObj prohibitedProcessData        = new DictObj();
        public static DictObj prohibitedProcessDataDefault = new DictObj();

        public static int     urlFilterRuleIndex;
        public static ListObj urlFilterRuleList        = new ListObj();
        public static DictObj urlFilterRuleData        = new DictObj();
        public static DictObj urlFilterRuleDataDefault = new DictObj();
        public static DictObj urlFilterRuleDataStorage = new DictObj();

        public static int     urlFilterActionIndex;
        public static ListObj urlFilterActionList        = new ListObj();
        public static ListObj urlFilterActionListDefault = new ListObj();
        public static ListObj urlFilterActionListStorage = new ListObj();
        public static DictObj urlFilterActionData        = new DictObj();
        public static DictObj urlFilterActionDataDefault = new DictObj();
        public static DictObj urlFilterActionDataStorage = new DictObj();

        public static int     embeddedCertificateIndex;
        public static ListObj embeddedCertificateList        = new ListObj();
        public static DictObj embeddedCertificateData        = new DictObj();
        public static DictObj embeddedCertificateDataDefault = new DictObj();

        public static DictObj proxiesData        = new DictObj();
        public static DictObj proxiesDataDefault = new DictObj();

        public static int     proxyProtocolIndex;

        public static int     bypassedProxyIndex;
        public static ListObj bypassedProxyList        = new ListObj();
        public static String  bypassedProxyData        = "";
        public static String  bypassedProxyDataDefault = "";



        // ************************
        // Methods for SEB settings
        // ************************


        // ********************************************************************
        // Set all the default values for the Plist structure "settingsDefault"
        // ********************************************************************
        public static void CreateDefaultAndCurrentSettingsFromScratch()
        {
            // Destroy all default lists and dictionaries
            settingsDefault = new DictObj();
            settingsCurrent = new DictObj();

            permittedProcessList          = new ListObj();
            permittedProcessData          = new DictObj();
            permittedProcessDataDefault   = new DictObj();
            permittedProcessDataXulRunner = new DictObj();

            permittedArgumentList           = new ListObj();
            permittedArgumentData           = new DictObj();
            permittedArgumentDataDefault    = new DictObj();
            permittedArgumentDataXulRunner1 = new DictObj();
            permittedArgumentDataXulRunner2 = new DictObj();
            permittedArgumentListXulRunner  = new ListObj();

            prohibitedProcessList        = new ListObj();
            prohibitedProcessData        = new DictObj();
            prohibitedProcessDataDefault = new DictObj();

            urlFilterRuleList        = new ListObj();
            urlFilterRuleData        = new DictObj();
            urlFilterRuleDataDefault = new DictObj();
            urlFilterRuleDataStorage = new DictObj();

            urlFilterActionList        = new ListObj();
            urlFilterActionListDefault = new ListObj();
            urlFilterActionListStorage = new ListObj();
            urlFilterActionData        = new DictObj();
            urlFilterActionDataDefault = new DictObj();
            urlFilterActionDataStorage = new DictObj();

            embeddedCertificateList        = new ListObj();
            embeddedCertificateData        = new DictObj();
            embeddedCertificateDataDefault = new DictObj();

            proxiesData        = new DictObj();
            proxiesDataDefault = new DictObj();

            bypassedProxyList        = new ListObj();
            bypassedProxyData        = "";
            bypassedProxyDataDefault = "";


            // Initialise the global arrays
            for (int value = 1; value <= ValueNum; value++)
            {
                SEBSettings.intArrayDefault[value] = 0;
                SEBSettings.intArrayCurrent[value] = 0;

                SEBSettings.strArrayDefault[value] = "";
                SEBSettings.strArrayCurrent[value] = "";
            }

            // Initialise the default settings Plist
            SEBSettings.settingsDefault.Clear();

            // Default settings for keys not belonging to any group
            SEBSettings.settingsDefault.Add(SEBSettings.MessageOriginatorVersion, "SEB_Win_2.0pre_build");

            // Default settings for group "General"
            SEBSettings.settingsDefault.Add(SEBSettings.MessageStartURL            , "http://www.safeexambrowser.org");
            SEBSettings.settingsDefault.Add(SEBSettings.MessageSebServerURL        , "");
            SEBSettings.settingsDefault.Add(SEBSettings.MessageAdminPassword       , "");
            SEBSettings.settingsDefault.Add(SEBSettings.MessageConfirmAdminPassword, "");
            SEBSettings.settingsDefault.Add(SEBSettings.MessageHashedAdminPassword , "");
            SEBSettings.settingsDefault.Add(SEBSettings.MessageAllowQuit           , true);
            SEBSettings.settingsDefault.Add(SEBSettings.MessageIgnoreQuitPassword  , false);
            SEBSettings.settingsDefault.Add(SEBSettings.MessageQuitPassword        , "");
            SEBSettings.settingsDefault.Add(SEBSettings.MessageConfirmQuitPassword , "");
            SEBSettings.settingsDefault.Add(SEBSettings.MessageHashedQuitPassword  , "");
            SEBSettings.settingsDefault.Add(SEBSettings.MessageExitKey1,  2);
            SEBSettings.settingsDefault.Add(SEBSettings.MessageExitKey2, 10);
            SEBSettings.settingsDefault.Add(SEBSettings.MessageExitKey3,  5);
            SEBSettings.settingsDefault.Add(SEBSettings.MessageSebMode, 0);

            // Default settings for group "Config File"
            SEBSettings.settingsDefault.Add(SEBSettings.MessageSebConfigPurpose       , 0);
            SEBSettings.settingsDefault.Add(SEBSettings.MessageAllowPreferencesWindow , true);
            SEBSettings.settingsDefault.Add(SEBSettings.MessageSettingsPassword       , "");
            SEBSettings.settingsDefault.Add(SEBSettings.MessageConfirmSettingsPassword, "");
            SEBSettings.settingsDefault.Add(SEBSettings.MessageHashedSettingsPassword , "");

            // CryptoIdentity is stored additionally
            SEBSettings.intArrayDefault[SEBSettings.ValueCryptoIdentity] = 0;
            SEBSettings.strArrayDefault[SEBSettings.ValueCryptoIdentity] = "";

            // Default settings for group "Appearance"
            SEBSettings.settingsDefault.Add(SEBSettings.MessageBrowserViewMode             , 0);
            SEBSettings.settingsDefault.Add(SEBSettings.MessageMainBrowserWindowWidth      , "100%");
            SEBSettings.settingsDefault.Add(SEBSettings.MessageMainBrowserWindowHeight     , "100%");
            SEBSettings.settingsDefault.Add(SEBSettings.MessageMainBrowserWindowPositioning, 1);
            SEBSettings.settingsDefault.Add(SEBSettings.MessageEnableBrowserWindowToolbar  , false);
            SEBSettings.settingsDefault.Add(SEBSettings.MessageHideBrowserWindowToolbar    , false);
            SEBSettings.settingsDefault.Add(SEBSettings.MessageShowMenuBar                 , false);
            SEBSettings.settingsDefault.Add(SEBSettings.MessageShowTaskBar                 , true);
            SEBSettings.settingsDefault.Add(SEBSettings.MessageTaskBarHeight               , 40);

            // MainBrowserWindow Width and Height is stored additionally
            SEBSettings.intArrayDefault[SEBSettings.ValueMainBrowserWindowWidth ] = 2;
            SEBSettings.intArrayDefault[SEBSettings.ValueMainBrowserWindowHeight] = 2;
            SEBSettings.strArrayDefault[SEBSettings.ValueMainBrowserWindowWidth ] = "100%";
            SEBSettings.strArrayDefault[SEBSettings.ValueMainBrowserWindowHeight] = "100%";

            // Default settings for group "Browser"
            SEBSettings.settingsDefault.Add(SEBSettings.MessageNewBrowserWindowByLinkPolicy        , 2);
            SEBSettings.settingsDefault.Add(SEBSettings.MessageNewBrowserWindowByScriptPolicy      , 2);
            SEBSettings.settingsDefault.Add(SEBSettings.MessageNewBrowserWindowByLinkBlockForeign  , false);
            SEBSettings.settingsDefault.Add(SEBSettings.MessageNewBrowserWindowByScriptBlockForeign, false);
            SEBSettings.settingsDefault.Add(SEBSettings.MessageNewBrowserWindowByLinkWidth         , "1000");
            SEBSettings.settingsDefault.Add(SEBSettings.MessageNewBrowserWindowByLinkHeight        , "100%");
            SEBSettings.settingsDefault.Add(SEBSettings.MessageNewBrowserWindowByLinkPositioning   , 2);

            SEBSettings.settingsDefault.Add(SEBSettings.MessageEnablePlugIns           , true);
            SEBSettings.settingsDefault.Add(SEBSettings.MessageEnableJava              , false);
            SEBSettings.settingsDefault.Add(SEBSettings.MessageEnableJavaScript        , true);
            SEBSettings.settingsDefault.Add(SEBSettings.MessageBlockPopUpWindows       , false);
            SEBSettings.settingsDefault.Add(SEBSettings.MessageAllowBrowsingBackForward, false);
            SEBSettings.settingsDefault.Add(SEBSettings.MessageEnableSebBrowser        , true);

            // NewBrowserWindow Width and Height is stored additionally
            SEBSettings.intArrayDefault[SEBSettings.ValueNewBrowserWindowByLinkWidth ] = 4;
            SEBSettings.intArrayDefault[SEBSettings.ValueNewBrowserWindowByLinkHeight] = 2;
            SEBSettings.strArrayDefault[SEBSettings.ValueNewBrowserWindowByLinkWidth ] = "1000";
            SEBSettings.strArrayDefault[SEBSettings.ValueNewBrowserWindowByLinkHeight] = "100%";

            // Default settings for group "DownUploads"
            SEBSettings.settingsDefault.Add(SEBSettings.MessageAllowDownUploads        , true);
            SEBSettings.settingsDefault.Add(SEBSettings.MessageDownloadDirectoryOSX    , "~/Downloads");
            SEBSettings.settingsDefault.Add(SEBSettings.MessageDownloadDirectoryWin    , "Desktop");
            SEBSettings.settingsDefault.Add(SEBSettings.MessageOpenDownloads           , false);
            SEBSettings.settingsDefault.Add(SEBSettings.MessageChooseFileToUploadPolicy, 0);
            SEBSettings.settingsDefault.Add(SEBSettings.MessageDownloadPDFFiles        , false);

            // Default settings for group "Exam"
            SEBSettings.settingsDefault.Add(SEBSettings.MessageExamKeySalt       , new Byte[] {});
            SEBSettings.settingsDefault.Add(SEBSettings.MessageBrowserExamKey    , "");
            SEBSettings.settingsDefault.Add(SEBSettings.MessageCopyBrowserExamKey, false);
            SEBSettings.settingsDefault.Add(SEBSettings.MessageSendBrowserExamKey, false);
            SEBSettings.settingsDefault.Add(SEBSettings.MessageQuitURL           , "");

            // Default settings for group "Applications"
            SEBSettings.settingsDefault.Add(SEBSettings.MessageMonitorProcesses         , false);
            SEBSettings.settingsDefault.Add(SEBSettings.MessageAllowSwitchToApplications, false);
            SEBSettings.settingsDefault.Add(SEBSettings.MessageAllowFlashFullscreen     , false);
            SEBSettings.settingsDefault.Add(SEBSettings.MessagePermittedProcesses       , new ListObj());
            SEBSettings.settingsDefault.Add(SEBSettings.MessageProhibitedProcesses      , new ListObj());

            // Default settings for permitted argument data
            SEBSettings.permittedArgumentDataDefault.Clear();
            SEBSettings.permittedArgumentDataDefault.Add(SEBSettings.MessageActive, true);
            SEBSettings.permittedArgumentDataDefault.Add(SEBSettings.MessageArgument, "");

            // Define the XulRunner arguments
            SEBSettings.permittedArgumentDataXulRunner1.Clear();
            SEBSettings.permittedArgumentDataXulRunner1.Add(SEBSettings.MessageActive, true);
            SEBSettings.permittedArgumentDataXulRunner1.Add(SEBSettings.MessageArgument, "-app \"..\\xul_seb\\seb.ini\"");

            SEBSettings.permittedArgumentDataXulRunner2.Clear();
            SEBSettings.permittedArgumentDataXulRunner2.Add(SEBSettings.MessageActive, true);
            SEBSettings.permittedArgumentDataXulRunner2.Add(SEBSettings.MessageArgument, "-profile \"%LOCALAPPDATA%\\ETH Zuerich\\xul_seb\\Profiles\"");

            // Create the XulRunner argument list with the XulRunner arguments
            SEBSettings.permittedArgumentListXulRunner.Clear();
            SEBSettings.permittedArgumentListXulRunner.Add(SEBSettings.permittedArgumentDataXulRunner1);
            SEBSettings.permittedArgumentListXulRunner.Add(SEBSettings.permittedArgumentDataXulRunner2);

            // Create a XulRunner process with the XulRunner argument list
            SEBSettings.permittedProcessDataXulRunner.Clear();
            SEBSettings.permittedProcessDataXulRunner.Add(SEBSettings.MessageActive     , true);
            SEBSettings.permittedProcessDataXulRunner.Add(SEBSettings.MessageAutostart  , true);
            SEBSettings.permittedProcessDataXulRunner.Add(SEBSettings.MessageAutohide   , true);
            SEBSettings.permittedProcessDataXulRunner.Add(SEBSettings.MessageAllowUser  , true);
            SEBSettings.permittedProcessDataXulRunner.Add(SEBSettings.MessageOS         , IntWin);
            SEBSettings.permittedProcessDataXulRunner.Add(SEBSettings.MessageTitle      , "SEB");
            SEBSettings.permittedProcessDataXulRunner.Add(SEBSettings.MessageDescription, "");
            SEBSettings.permittedProcessDataXulRunner.Add(SEBSettings.MessageExecutable , "xulrunner.exe");
            SEBSettings.permittedProcessDataXulRunner.Add(SEBSettings.MessagePath       , "../xulrunner/");
            SEBSettings.permittedProcessDataXulRunner.Add(SEBSettings.MessageIdentifier , "XulRunner");
            SEBSettings.permittedProcessDataXulRunner.Add(SEBSettings.MessageArguments  , permittedArgumentListXulRunner);

            // Default settings for permitted process data
            SEBSettings.permittedProcessDataDefault.Clear();
            SEBSettings.permittedProcessDataDefault.Add(SEBSettings.MessageActive     , true);
            SEBSettings.permittedProcessDataDefault.Add(SEBSettings.MessageAutostart  , true);
            SEBSettings.permittedProcessDataDefault.Add(SEBSettings.MessageAutohide   , true);
            SEBSettings.permittedProcessDataDefault.Add(SEBSettings.MessageAllowUser  , true);
            SEBSettings.permittedProcessDataDefault.Add(SEBSettings.MessageOS         , IntWin);
            SEBSettings.permittedProcessDataDefault.Add(SEBSettings.MessageTitle      , "");
            SEBSettings.permittedProcessDataDefault.Add(SEBSettings.MessageDescription, "");
            SEBSettings.permittedProcessDataDefault.Add(SEBSettings.MessageExecutable , "");
            SEBSettings.permittedProcessDataDefault.Add(SEBSettings.MessagePath       , "");
            SEBSettings.permittedProcessDataDefault.Add(SEBSettings.MessageIdentifier , "");
            SEBSettings.permittedProcessDataDefault.Add(SEBSettings.MessageArguments  , new ListObj());

            // Default settings for prohibited process data
            SEBSettings.prohibitedProcessDataDefault.Clear();
            SEBSettings.prohibitedProcessDataDefault.Add(SEBSettings.MessageActive     , true);
            SEBSettings.prohibitedProcessDataDefault.Add(SEBSettings.MessageCurrentUser, true);
            SEBSettings.prohibitedProcessDataDefault.Add(SEBSettings.MessageStrongKill , false);
            SEBSettings.prohibitedProcessDataDefault.Add(SEBSettings.MessageOS         , IntWin);
            SEBSettings.prohibitedProcessDataDefault.Add(SEBSettings.MessageExecutable , "");
            SEBSettings.prohibitedProcessDataDefault.Add(SEBSettings.MessageDescription, "");
            SEBSettings.prohibitedProcessDataDefault.Add(SEBSettings.MessageIdentifier , "");
            SEBSettings.prohibitedProcessDataDefault.Add(SEBSettings.MessageUser       , "");

            // Default settings for group "Network - Filter"
            SEBSettings.settingsDefault.Add(SEBSettings.MessageEnableURLFilter       , false);
            SEBSettings.settingsDefault.Add(SEBSettings.MessageEnableURLContentFilter, false);
            SEBSettings.settingsDefault.Add(SEBSettings.MessageURLFilterRules        , new ListObj());

            // Create a default action
            SEBSettings.urlFilterActionDataDefault.Clear();
            SEBSettings.urlFilterActionDataDefault.Add(SEBSettings.MessageActive    , true);
            SEBSettings.urlFilterActionDataDefault.Add(SEBSettings.MessageRegex     , false);
            SEBSettings.urlFilterActionDataDefault.Add(SEBSettings.MessageExpression, "*");
            SEBSettings.urlFilterActionDataDefault.Add(SEBSettings.MessageAction    , 0);

            // Create a default action list with one entry (the default action)
            SEBSettings.urlFilterActionListDefault.Clear();
            SEBSettings.urlFilterActionListDefault.Add(SEBSettings.urlFilterActionDataDefault);

            // Create a default rule with this default action list.
            // This default rule is used for the "Insert Rule" operation:
            // when a new rule is created, it initially contains one action.
            SEBSettings.urlFilterRuleDataDefault.Clear();
            SEBSettings.urlFilterRuleDataDefault.Add(SEBSettings.MessageActive     , true);
            SEBSettings.urlFilterRuleDataDefault.Add(SEBSettings.MessageExpression , "Rule");
            SEBSettings.urlFilterRuleDataDefault.Add(SEBSettings.MessageRuleActions, SEBSettings.urlFilterActionListDefault);

            // Initialise the stored action
            SEBSettings.urlFilterActionDataStorage.Clear();
            SEBSettings.urlFilterActionDataStorage.Add(SEBSettings.MessageActive    , true);
            SEBSettings.urlFilterActionDataStorage.Add(SEBSettings.MessageRegex     , false);
            SEBSettings.urlFilterActionDataStorage.Add(SEBSettings.MessageExpression, "*");
            SEBSettings.urlFilterActionDataStorage.Add(SEBSettings.MessageAction    , 0);

            // Initialise the stored action list with no entry
            SEBSettings.urlFilterActionListStorage.Clear();

            // Initialise the stored rule
            SEBSettings.urlFilterRuleDataStorage.Clear();
            SEBSettings.urlFilterRuleDataStorage.Add(SEBSettings.MessageActive     , true);
            SEBSettings.urlFilterRuleDataStorage.Add(SEBSettings.MessageExpression , "Rule");
            SEBSettings.urlFilterRuleDataStorage.Add(SEBSettings.MessageRuleActions, SEBSettings.urlFilterActionListStorage);

            // Default settings for group "Network - Certificates"
            SEBSettings.settingsDefault.Add(SEBSettings.MessageEmbeddedCertificates, new ListObj());

            SEBSettings.embeddedCertificateDataDefault.Clear();
            SEBSettings.embeddedCertificateDataDefault.Add(SEBSettings.MessageCertificateData, "");
            SEBSettings.embeddedCertificateDataDefault.Add(SEBSettings.MessageType           , 0);
            SEBSettings.embeddedCertificateDataDefault.Add(SEBSettings.MessageName           , "");

            // Default settings for group "Network - Proxies"
            SEBSettings.proxiesDataDefault.Clear();

            SEBSettings.proxiesDataDefault.Add(SEBSettings.MessageExceptionsList             , new ListObj());
            SEBSettings.proxiesDataDefault.Add(SEBSettings.MessageExcludeSimpleHostnames     , true);
            SEBSettings.proxiesDataDefault.Add(SEBSettings.MessageAutoDiscoveryEnabled       , false);
            SEBSettings.proxiesDataDefault.Add(SEBSettings.MessageAutoConfigurationEnabled   , false);
            SEBSettings.proxiesDataDefault.Add(SEBSettings.MessageAutoConfigurationJavaScript, "");
            SEBSettings.proxiesDataDefault.Add(SEBSettings.MessageAutoConfigurationURL       , "");
            SEBSettings.proxiesDataDefault.Add(SEBSettings.MessageFTPPassive                 , true);

            SEBSettings.proxiesDataDefault.Add(SEBSettings.MessageHTTPEnable  , false);
            SEBSettings.proxiesDataDefault.Add(SEBSettings.MessageHTTPPort    , 0);
            SEBSettings.proxiesDataDefault.Add(SEBSettings.MessageHTTPHost    , "");
            SEBSettings.proxiesDataDefault.Add(SEBSettings.MessageHTTPRequires, false);
            SEBSettings.proxiesDataDefault.Add(SEBSettings.MessageHTTPUsername, "");
            SEBSettings.proxiesDataDefault.Add(SEBSettings.MessageHTTPPassword, "");

            SEBSettings.proxiesDataDefault.Add(SEBSettings.MessageHTTPSEnable  , false);
            SEBSettings.proxiesDataDefault.Add(SEBSettings.MessageHTTPSPort    , 0);
            SEBSettings.proxiesDataDefault.Add(SEBSettings.MessageHTTPSHost    , "");
            SEBSettings.proxiesDataDefault.Add(SEBSettings.MessageHTTPSRequires, false);
            SEBSettings.proxiesDataDefault.Add(SEBSettings.MessageHTTPSUsername, "");
            SEBSettings.proxiesDataDefault.Add(SEBSettings.MessageHTTPSPassword, "");

            SEBSettings.proxiesDataDefault.Add(SEBSettings.MessageFTPEnable  , false);
            SEBSettings.proxiesDataDefault.Add(SEBSettings.MessageFTPPort    , 0);
            SEBSettings.proxiesDataDefault.Add(SEBSettings.MessageFTPHost    , "");
            SEBSettings.proxiesDataDefault.Add(SEBSettings.MessageFTPRequires, false);
            SEBSettings.proxiesDataDefault.Add(SEBSettings.MessageFTPUsername, "");
            SEBSettings.proxiesDataDefault.Add(SEBSettings.MessageFTPPassword, "");

            SEBSettings.proxiesDataDefault.Add(SEBSettings.MessageSOCKSEnable  , false);
            SEBSettings.proxiesDataDefault.Add(SEBSettings.MessageSOCKSPort    , 0);
            SEBSettings.proxiesDataDefault.Add(SEBSettings.MessageSOCKSHost    , "");
            SEBSettings.proxiesDataDefault.Add(SEBSettings.MessageSOCKSRequires, false);
            SEBSettings.proxiesDataDefault.Add(SEBSettings.MessageSOCKSUsername, "");
            SEBSettings.proxiesDataDefault.Add(SEBSettings.MessageSOCKSPassword, "");

            SEBSettings.proxiesDataDefault.Add(SEBSettings.MessageRTSPEnable  , false);
            SEBSettings.proxiesDataDefault.Add(SEBSettings.MessageRTSPPort    , 0);
            SEBSettings.proxiesDataDefault.Add(SEBSettings.MessageRTSPHost    , "");
            SEBSettings.proxiesDataDefault.Add(SEBSettings.MessageRTSPRequires, false);
            SEBSettings.proxiesDataDefault.Add(SEBSettings.MessageRTSPUsername, "");
            SEBSettings.proxiesDataDefault.Add(SEBSettings.MessageRTSPPassword, "");

            SEBSettings.bypassedProxyDataDefault = "";

            SEBSettings.settingsDefault.Add(SEBSettings.MessageProxySettingsPolicy, 0);
            SEBSettings.settingsDefault.Add(SEBSettings.MessageProxies            , SEBSettings.proxiesDataDefault);

            // Default settings for group "Security"
            SEBSettings.settingsDefault.Add(SEBSettings.MessageSebServicePolicy   , 2);
            SEBSettings.settingsDefault.Add(SEBSettings.MessageAllowVirtualMachine, false);
            SEBSettings.settingsDefault.Add(SEBSettings.MessageCreateNewDesktop   , true);
            SEBSettings.settingsDefault.Add(SEBSettings.MessageKillExplorerShell  , false);
            SEBSettings.settingsDefault.Add(SEBSettings.MessageAllowUserSwitching , true);
            SEBSettings.settingsDefault.Add(SEBSettings.MessageEnableLogging      , false);
            SEBSettings.settingsDefault.Add(SEBSettings.MessageLogDirectoryOSX    , "~/Documents");
            SEBSettings.settingsDefault.Add(SEBSettings.MessageLogDirectoryWin    , "My Documents");

            // Default settings for group "Inside SEB"
            SEBSettings.settingsDefault.Add(SEBSettings.MessageInsideSebEnableSwitchUser       , false);
            SEBSettings.settingsDefault.Add(SEBSettings.MessageInsideSebEnableLockThisComputer , false);
            SEBSettings.settingsDefault.Add(SEBSettings.MessageInsideSebEnableChangeAPassword  , false);
            SEBSettings.settingsDefault.Add(SEBSettings.MessageInsideSebEnableStartTaskManager , false);
            SEBSettings.settingsDefault.Add(SEBSettings.MessageInsideSebEnableLogOff           , false);
            SEBSettings.settingsDefault.Add(SEBSettings.MessageInsideSebEnableShutDown         , false);
            SEBSettings.settingsDefault.Add(SEBSettings.MessageInsideSebEnableEaseOfAccess     , false);
            SEBSettings.settingsDefault.Add(SEBSettings.MessageInsideSebEnableVmWareClientShade, false);

            // Default settings for group "Outside SEB"
            SEBSettings.settingsDefault.Add(SEBSettings.MessageOutsideSebEnableSwitchUser       , true);
            SEBSettings.settingsDefault.Add(SEBSettings.MessageOutsideSebEnableLockThisComputer , true);
            SEBSettings.settingsDefault.Add(SEBSettings.MessageOutsideSebEnableChangeAPassword  , true);
            SEBSettings.settingsDefault.Add(SEBSettings.MessageOutsideSebEnableStartTaskManager , true);
            SEBSettings.settingsDefault.Add(SEBSettings.MessageOutsideSebEnableLogOff           , true);
            SEBSettings.settingsDefault.Add(SEBSettings.MessageOutsideSebEnableShutDown         , true);
            SEBSettings.settingsDefault.Add(SEBSettings.MessageOutsideSebEnableEaseOfAccess     , true);
            SEBSettings.settingsDefault.Add(SEBSettings.MessageOutsideSebEnableVmWareClientShade, true);

            // Default settings for group "Hooked Keys"
            SEBSettings.settingsDefault.Add(SEBSettings.MessageHookKeys, true);

            // Default settings for group "Special Keys"
            SEBSettings.settingsDefault.Add(SEBSettings.MessageEnableEsc       , false);
            SEBSettings.settingsDefault.Add(SEBSettings.MessageEnableCtrlEsc   , false);
            SEBSettings.settingsDefault.Add(SEBSettings.MessageEnableAltEsc    , false);
            SEBSettings.settingsDefault.Add(SEBSettings.MessageEnableAltTab    , true);
            SEBSettings.settingsDefault.Add(SEBSettings.MessageEnableAltF4     , false);
            SEBSettings.settingsDefault.Add(SEBSettings.MessageEnableStartMenu , false);
            SEBSettings.settingsDefault.Add(SEBSettings.MessageEnableRightMouse, false);

            // Default settings for group "Function Keys"
            SEBSettings.settingsDefault.Add(SEBSettings.MessageEnableF1 , false);
            SEBSettings.settingsDefault.Add(SEBSettings.MessageEnableF2 , false);
            SEBSettings.settingsDefault.Add(SEBSettings.MessageEnableF3 , false);
            SEBSettings.settingsDefault.Add(SEBSettings.MessageEnableF4 , false);
            SEBSettings.settingsDefault.Add(SEBSettings.MessageEnableF5 , true);
            SEBSettings.settingsDefault.Add(SEBSettings.MessageEnableF6 , false);
            SEBSettings.settingsDefault.Add(SEBSettings.MessageEnableF7 , false);
            SEBSettings.settingsDefault.Add(SEBSettings.MessageEnableF8 , false);
            SEBSettings.settingsDefault.Add(SEBSettings.MessageEnableF9 , false);
            SEBSettings.settingsDefault.Add(SEBSettings.MessageEnableF10, false);
            SEBSettings.settingsDefault.Add(SEBSettings.MessageEnableF11, false);
            SEBSettings.settingsDefault.Add(SEBSettings.MessageEnableF12, false);


            // Clear all "current" lists and dictionaries

            SEBSettings.permittedProcessIndex = -1;
            SEBSettings.permittedProcessList.Clear();
            SEBSettings.permittedProcessData.Clear();

            SEBSettings.permittedArgumentIndex = -1;
            SEBSettings.permittedArgumentList.Clear();
            SEBSettings.permittedArgumentData.Clear();

            SEBSettings.prohibitedProcessIndex = -1;
            SEBSettings.prohibitedProcessList.Clear();
            SEBSettings.prohibitedProcessData.Clear();

            SEBSettings.urlFilterRuleIndex = -1;
            SEBSettings.urlFilterRuleList.Clear();
            SEBSettings.urlFilterRuleData.Clear();

            SEBSettings.urlFilterActionIndex = -1;
            SEBSettings.urlFilterActionList.Clear();
            SEBSettings.urlFilterActionData.Clear();

            SEBSettings.embeddedCertificateIndex = -1;
            SEBSettings.embeddedCertificateList.Clear();
            SEBSettings.embeddedCertificateData.Clear();

            SEBSettings.proxyProtocolIndex = -1;
            SEBSettings.proxiesData.Clear();

            SEBSettings.bypassedProxyIndex = -1;
            SEBSettings.bypassedProxyList.Clear();
            SEBSettings.bypassedProxyData = "";
        }



        // *****************************************
        // Restore default settings and new settings
        // *****************************************
        public static void RestoreDefaultAndCurrentSettings()
        {
            // Set all the default values for the Plist structure "settingsCurrent"

            // Create a default Dictionary "settingsDefault".
            // Create a current Dictionary "settingsCurrent".
            // Fill up new settings by default settings, where necessary.
            // This assures that every (key, value) pair is contained
            // in the "default" and "current" dictionaries,
            // even if the loaded "current" dictionary did NOT contain every pair.

            SEBSettings.CreateDefaultAndCurrentSettingsFromScratch();
            SEBSettings.settingsCurrent.Clear();
            SEBSettings.FillSettingsDictionary();
            SEBSettings.FillSettingsArrays();
        }



        // ********************
        // Copy settings arrays
        // ********************
        public static void FillSettingsArrays()
        {
            // Set all array values to default values
            for (int value = 1; value <= SEBSettings.ValueNum; value++)
            {
                intArrayCurrent[value] = intArrayDefault[value];
                strArrayCurrent[value] = strArrayDefault[value];
            }
            return;
        }



        // ************************
        // Copy settings dictionary
        // ************************
/*
        public static void CopySettingsDictionary(ref DictObj sebSettingsSource,
                                                  ref DictObj sebSettingsTarget)
        {
            // Copy all settings from one dictionary to another
            // Create a dictionary "target settings".
            // Copy source settings to target settings
            foreach (KeyValue pair in sebSettingsSource)
            {
                string key   = pair.Key;
                object value = pair.Value;

                if  (sebSettingsTarget.ContainsKey(key))
                     sebSettingsTarget[key] = value;
                else sebSettingsTarget.Add(key, value);
            }

            return;
        }
*/


        // **************
        // Merge settings
        // **************
/*
        public static void MergeSettings(ref object objectSource, ref object objectTarget)
        {
            // Determine the type of the input objects
            string typeSource = objectSource.GetType().ToString();
            string typeTarget = objectTarget.GetType().ToString();

            if (typeSource != typeTarget) return;

            // Treat the complex datatype Dictionary<string, object>
            if (typeSource.Contains("Dictionary"))
            {
                DictObj dictSource = (DictObj)objectSource;
                DictObj dictTarget = (DictObj)objectTarget;

                //foreach (KeyValue pair in dictSource)
                for (int index = 0; index < dictSource.Count; index++)
                {
                    KeyValue pair  = dictSource.ElementAt(index);
                    string   key   = pair.Key;
                    object   value = pair.Value;
                    string   type  = pair.Value.GetType().ToString();

                    if  (dictTarget.ContainsKey(key))
                         dictTarget[key] = value;
                    else dictTarget.Add(key, value);

                    if (type.Contains("Dictionary") || type.Contains("List"))
                    {
                        object childSource = dictSource[key];
                        object childTarget = dictTarget[key];
                        MergeSettings(ref childSource, ref childTarget);
                    }

                } // next (KeyValue pair in dictSource)
            } // end if (typeSource.Contains("Dictionary"))


            // Treat the complex datatype List<object>
            if (typeSource.Contains("List"))
            {
                ListObj listSource = (ListObj)objectSource;
                ListObj listTarget = (ListObj)objectTarget;

                //foreach (object elem in listSource)
                for (int index = 0; index < listSource.Count; index++)
                {
                    object elem = listSource[index];
                    string type = elem.GetType().ToString();

                    if  (listTarget.Count > index)
                         listTarget[index] = elem;
                    else listTarget.Add(elem);

                    if (type.Contains("Dictionary") || type.Contains("List"))
                    {
                        object childSource = listSource[index];
                        object childTarget = listTarget[index];
                        MergeSettings(ref childSource, ref childTarget);
                    }

                } // next (element in listSource)
            } // end if (typeSource.Contains("List"))

            return;
        }
*/


        // ************************
        // Fill settings dictionary
        // ************************
        public static void FillSettingsDictionary()
        {

            // Add potentially missing keys to current Main Dictionary
            foreach (KeyValue p in SEBSettings.settingsDefault)
                               if (SEBSettings.settingsCurrent.ContainsKey(p.Key) == false)
                                   SEBSettings.settingsCurrent.Add        (p.Key, p.Value);



            // Get the Permitted Process List
            SEBSettings.permittedProcessList = (ListObj)SEBSettings.settingsCurrent[SEBSettings.MessagePermittedProcesses];

            // Traverse Permitted Processes of currently opened file
            for (int listIndex = 0; listIndex < SEBSettings.permittedProcessList.Count; listIndex++)
            {
                // Get the Permitted Process Data
                SEBSettings.permittedProcessData = (DictObj)SEBSettings.permittedProcessList[listIndex];

                // Add potentially missing keys to current Process Dictionary
                foreach (KeyValue p in SEBSettings.permittedProcessDataDefault)
                                   if (SEBSettings.permittedProcessData.ContainsKey(p.Key) == false)
                                       SEBSettings.permittedProcessData.Add        (p.Key, p.Value);

                // Get the Permitted Argument List
                SEBSettings.permittedArgumentList = (ListObj)SEBSettings.permittedProcessData[SEBSettings.MessageArguments];

                // Traverse Arguments of current Process
                for (int sublistIndex = 0; sublistIndex < SEBSettings.permittedArgumentList.Count; sublistIndex++)
                {
                    // Get the Permitted Argument Data
                    SEBSettings.permittedArgumentData = (DictObj)SEBSettings.permittedArgumentList[sublistIndex];

                    // Add potentially missing keys to current Argument Dictionary
                    foreach (KeyValue p in SEBSettings.permittedArgumentDataDefault)
                                       if (SEBSettings.permittedArgumentData.ContainsKey(p.Key) == false)
                                           SEBSettings.permittedArgumentData.Add        (p.Key, p.Value);

                } // next sublistIndex
            } // next listIndex



            // Get the Prohibited Process List
            SEBSettings.prohibitedProcessList = (ListObj)SEBSettings.settingsCurrent[SEBSettings.MessageProhibitedProcesses];

            // Traverse Prohibited Processes of currently opened file
            for (int listIndex = 0; listIndex < SEBSettings.prohibitedProcessList.Count; listIndex++)
            {
                // Get the Prohibited Process Data
                SEBSettings.prohibitedProcessData = (DictObj)SEBSettings.prohibitedProcessList[listIndex];

                // Add potentially missing keys to current Process Dictionary
                foreach (KeyValue p in SEBSettings.prohibitedProcessDataDefault)
                                   if (SEBSettings.prohibitedProcessData.ContainsKey(p.Key) == false)
                                       SEBSettings.prohibitedProcessData.Add        (p.Key, p.Value);

            } // next listIndex



            // Get the Embedded Certificate List
            SEBSettings.embeddedCertificateList = (ListObj)SEBSettings.settingsCurrent[SEBSettings.MessageEmbeddedCertificates];

            // Traverse Embedded Certificates of currently opened file
            for (int listIndex = 0; listIndex < SEBSettings.embeddedCertificateList.Count; listIndex++)
            {
                // Get the Embedded Certificate Data
                SEBSettings.embeddedCertificateData = (DictObj)SEBSettings.embeddedCertificateList[listIndex];

                // Add potentially missing keys to current Certificate Dictionary
                foreach (KeyValue p in SEBSettings.embeddedCertificateDataDefault)
                                   if (SEBSettings.embeddedCertificateData.ContainsKey(p.Key) == false)
                                       SEBSettings.embeddedCertificateData.Add        (p.Key, p.Value);

            } // next listIndex



            // Get the URL Filter Rule List
            SEBSettings.urlFilterRuleList = (ListObj)SEBSettings.settingsCurrent[SEBSettings.MessageURLFilterRules];

            // Traverse URL Filter Rules of currently opened file
            for (int listIndex = 0; listIndex < SEBSettings.urlFilterRuleList.Count; listIndex++)
            {
                // Get the URL Filter Rule Data
                SEBSettings.urlFilterRuleData = (DictObj)SEBSettings.urlFilterRuleList[listIndex];

                // Add potentially missing keys to current Rule Dictionary
                foreach (KeyValue p in SEBSettings.urlFilterRuleDataDefault)
                                   if (SEBSettings.urlFilterRuleData.ContainsKey(p.Key) == false)
                                       SEBSettings.urlFilterRuleData.Add        (p.Key, p.Value);

                // Get the URL Filter Action List
                SEBSettings.urlFilterActionList = (ListObj)SEBSettings.urlFilterRuleData[SEBSettings.MessageRuleActions];

                // Traverse Actions of current Rule
                for (int sublistIndex = 0; sublistIndex < SEBSettings.urlFilterActionList.Count; sublistIndex++)
                {
                    // Get the URL Filter Action Data
                    SEBSettings.urlFilterActionData = (DictObj)SEBSettings.urlFilterActionList[sublistIndex];

                    // Add potentially missing keys to current Action Dictionary
                    foreach (KeyValue p in SEBSettings.urlFilterActionDataDefault)
                                       if (SEBSettings.urlFilterActionData.ContainsKey(p.Key) == false)
                                           SEBSettings.urlFilterActionData.Add        (p.Key, p.Value);

                } // next sublistIndex
            } // next listIndex



            // Get the Proxies Dictionary
            SEBSettings.proxiesData = (DictObj)SEBSettings.settingsCurrent[SEBSettings.MessageProxies];

            // Add potentially missing keys to current Proxies Dictionary
            foreach (KeyValue p in SEBSettings.proxiesDataDefault)
                               if (SEBSettings.proxiesData.ContainsKey(p.Key) == false)
                                   SEBSettings.proxiesData.Add        (p.Key, p.Value);

            // Get the Bypassed Proxy List
            SEBSettings.bypassedProxyList = (ListObj)proxiesData[SEBSettings.MessageExceptionsList];

            // Traverse Bypassed Proxies of currently opened file
            for (int listIndex = 0; listIndex < SEBSettings.bypassedProxyList.Count; listIndex++)
            {
                if ((String)SEBSettings.bypassedProxyList[listIndex] == "")
                            SEBSettings.bypassedProxyList[listIndex] = bypassedProxyDataDefault;
            } // next listIndex


            return;
        }



        // **********************************************
        // Add XulRunnerProcess to Permitted Process List
        // **********************************************
        public static void PermitXulRunnerProcess()
        {
            // Get the Permitted Process List
            SEBSettings.permittedProcessList = (ListObj)SEBSettings.settingsCurrent[SEBSettings.MessagePermittedProcesses];

            // Position of XulRunner process in Permitted Process List
            int indexOfProcessXulRunnerExe = -1;

            // Traverse Permitted Processes of currently opened file
            for (int listIndex = 0; listIndex < SEBSettings.permittedProcessList.Count; listIndex++)
            {
                SEBSettings.permittedProcessData = (DictObj)SEBSettings.permittedProcessList[listIndex];

                // Check if XulRunner process is in Permitted Process List
                if (SEBSettings.permittedProcessData[SEBSettings.MessageExecutable].Equals("xulrunner.exe"))
                    indexOfProcessXulRunnerExe = listIndex;

            } // next listIndex

            // If XulRunner process was not in Permitted Process List, add it
            if (indexOfProcessXulRunnerExe == -1)
            {
                SEBSettings.permittedProcessList.Add(SEBSettings.permittedProcessDataXulRunner);
            }
            // Assure that XulRunner process has correct settings:
            // Remove XulRunner process from Permitted Process List.
            // Add    XulRunner process to   Permitted Process List.
            else
            {
              //SEBSettings.permittedProcessList.RemoveAt(indexOfProcessXulRunnerExe);
              //SEBSettings.permittedProcessList.Insert  (indexOfProcessXulRunnerExe, SEBSettings.permittedProcessDataXulRunner);
            }

            return;
        }



        // **************
        // Print settings
        // **************
        public static void PrintSettingsRecursively(object objectSource, StreamWriter fileWriter, String indenting)
        {

            // Determine the type of the input object
            string typeSource = objectSource.GetType().ToString();


            // Treat the complex datatype Dictionary<string, object>
            if (typeSource.Contains("Dictionary"))
            {
                DictObj dictSource = (DictObj)objectSource;

                //foreach (KeyValue pair in dictSource)
                for (int index = 0; index < dictSource.Count; index++)
                {
                    KeyValue pair  = dictSource.ElementAt(index);
                    string   key   = pair.Key;
                    object   value = pair.Value;
                    string   type  = pair.Value.GetType().ToString();

                    // Print one (key, value) pair of dictionary
                    fileWriter.WriteLine(indenting + key + "=" + value);

                    if (type.Contains("Dictionary") || type.Contains("List"))
                    {
                        object childSource = dictSource[key];
                        PrintSettingsRecursively(childSource, fileWriter, indenting + "   ");
                    }

                } // next (KeyValue pair in dictSource)
            } // end if (typeSource.Contains("Dictionary"))


            // Treat the complex datatype List<object>
            if (typeSource.Contains("List"))
            {
                ListObj listSource = (ListObj)objectSource;

                //foreach (object elem in listSource)
                for (int index = 0; index < listSource.Count; index++)
                {
                    object elem = listSource[index];
                    string type = elem.GetType().ToString();

                    // Print one element of list
                    fileWriter.WriteLine(indenting + elem);

                    if (type.Contains("Dictionary") || type.Contains("List"))
                    {
                        object childSource = listSource[index];
                        PrintSettingsRecursively(childSource, fileWriter, indenting + "   ");
                    }

                } // next (element in listSource)
            } // end if (typeSource.Contains("List"))

            return;
        }



        // *************************
        // Print settings dictionary
        // *************************
        public static void LoggSettingsDictionary(ref DictObj sebSettings, String  fileName)
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

            // Call the recursive method for printing the contents
            SEBSettings.PrintSettingsRecursively(sebSettings, fileWriter, "");

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
            // Recreate the default and current settings
            SEBSettings.CreateDefaultAndCurrentSettingsFromScratch();

            try
            {
                // Read the configuration settings from .seb file.
                // Decrypt the configuration settings.
                // Convert the XML structure into a C# object.

                SEBProtectionController sebProtectionController = new SEBProtectionController();

                byte[] encryptedSettings = File.ReadAllBytes(fileName);
                String decryptedSettings = "";

                decryptedSettings = sebProtectionController.DecryptSebClientSettings(encryptedSettings);
              //decryptedSettings = decryptedSettings.Trim();

                SEBSettings.settingsCurrent.Clear();
                SEBSettings.settingsCurrent = (DictObj)Plist.readPlistSource(decryptedSettings);
            }
            catch (Exception streamReadException)
            {
                // Let the user know what went wrong
                Console.WriteLine("The .seb file could not be read:");
                Console.WriteLine(streamReadException.Message);
                return false;
            }

            // If the settings could be read from file...

            // Fill up the Dictionary read from file with default settings, where necessary
            SEBSettings.LoggSettingsDictionary(ref SEBSettings.settingsDefault, "SettingsDefInReadSebConfigurationFileFillBefore.txt");
            SEBSettings.LoggSettingsDictionary(ref SEBSettings.settingsCurrent, "settingsCurrentInReadSebConfigurationFileFillBefore.txt");
            SEBSettings.FillSettingsDictionary();
            SEBSettings.FillSettingsArrays();
            SEBSettings.LoggSettingsDictionary(ref SEBSettings.settingsDefault, "SettingsDefInReadSebConfigurationFileFillAfter.txt");
            SEBSettings.LoggSettingsDictionary(ref SEBSettings.settingsCurrent, "settingsCurrentInReadSebConfigurationFileFillAfter.txt");

            // Add the XulRunner process to the Permitted Process List, if necessary
            SEBSettings.LoggSettingsDictionary(ref SEBSettings.settingsDefault, "SettingsDefInReadSebConfigurationFilePermitBefore.txt");
            SEBSettings.LoggSettingsDictionary(ref SEBSettings.settingsCurrent, "settingsCurrentInReadSebConfigurationFilePermitBefore.txt");
            SEBSettings.PermitXulRunnerProcess();
            SEBSettings.LoggSettingsDictionary(ref SEBSettings.settingsDefault, "SettingsDefInReadSebConfigurationFilePermitAfter.txt");
            SEBSettings.LoggSettingsDictionary(ref SEBSettings.settingsCurrent, "settingsCurrentInReadSebConfigurationFilePermitAfter.txt");

            SEBSettings.LoggSettingsDictionary(ref SEBSettings.settingsDefault, "SettingsDefInReadSebConfigurationFile.txt");
            SEBSettings.LoggSettingsDictionary(ref SEBSettings.settingsCurrent, "settingsCurrentInReadSebConfigurationFile.txt");

            return true;
        }



        // ********************************************************
        // Write the settings to the configuration file and save it
        // ********************************************************
        public static bool WriteSebConfigurationFile(String fileName)
        {
            try 
            {
                // Convert the C# object into an XML structure.
                // Encrypt the configuration settings.
                // Write the configuration settings into .seb file.

                SEBProtectionController sebProtectionController = new SEBProtectionController();

                byte[] encryptedSettings;
                String decryptedSettings = "";

                String password              = "seb";
                X509Certificate2 certificate = null;

                if (false)
                {
                    decryptedSettings = Plist.writeXml(SEBSettings.settingsCurrent);
                    encryptedSettings = sebProtectionController.EncryptWithPassword   (decryptedSettings, password);
                    encryptedSettings = sebProtectionController.EncryptWithCertificate(decryptedSettings, certificate);

                    File.WriteAllBytes(fileName, encryptedSettings);
                }
                else // unencrypted .xml file
                {
                    Plist.writeXml(SEBSettings.settingsCurrent, fileName);
                    Plist.writeXml(SEBSettings.settingsCurrent, "DebugsettingsCurrent_in_SaveConfigurationFile.xml");
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
