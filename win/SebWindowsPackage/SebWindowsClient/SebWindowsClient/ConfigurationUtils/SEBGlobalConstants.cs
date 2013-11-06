using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SebWindowsClient.ConfigurationUtils
{
    public class SEBGlobalConstants
    {
        #region Constants

        // Error levels
        public const int ERROR       = 0;
        public const int WARNING     = 1;
        public const int INFORMATION = 2;
        public const int QUESTION    = 3;

        public const string HKCU = "HKEY_CURRENT_USER";
        public const string HKLM = "HKEY_LOCAL_MACHINE";

        // Strings for registry keys
        public const string KEY_POLICIES_SYSTEM   = "Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\System";
        public const string KEY_POLICIES_EXPLORER = "Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\Explorer";
        public const string KEY_POLICIES_SEB      = "Software\\Policies\\SEB";
        public const string KEY_UTILMAN_EXE       = "Software\\Microsoft\\Windows NT\\CurrentVersion\\Image File Execution Options\\Utilman.exe";
        public const string KEY_VM_WARE_CLIENT    = "Software\\VMware, Inc.\\VMware VDM\\Client";

        // Strings for registry values
        public const string VAL_HIDE_FAST_USER_SWITCHING = "HideFastUserSwitching";
        public const string VAL_DISABLE_LOCK_WORKSTATION = "DisableLockWorkstation";
        public const string VAL_DISABLE_CHANGE_PASSWORD  = "DisableChangePassword";
        public const string VAL_DISABLE_TASK_MANAGER     = "DisableTaskMgr";
        public const string VAL_NO_LOG_OFF               = "NoLogoff";
        public const string VAL_NO_CLOSE                 = "NoClose";
        public const string VAL_ENABLE_EASE_OF_ACCESS    = "Debugger";
        public const string VAL_ENABLE_SHADE             = "EnableShade";

        // Aligned strings for printing out registry values
        public const string MSG_HIDE_FAST_USER_SWITCHING = "HideFastUserSwitching ";
        public const string MSG_DISABLE_LOCK_WORKSTATION = "DisableLockWorkstation";
        public const string MSG_DISABLE_CHANGE_PASSWORD  = "DisableChangePassword ";
        public const string MSG_DISABLE_TASK_MANAGER     = "DisableTaskMgr        ";
        public const string MSG_NO_LOG_OFF               = "NoLogoff              ";
        public const string MSG_NO_CLOSE                 = "NoClose               ";
        public const string MSG_ENABLE_EASE_OF_ACCESS    = "Debugger              ";
        public const string MSG_ENABLE_SHADE             = "EnableShade           ";

        // Only for trunk version necessary (XULrunner)
        public const string VAL_PERMITTED_APPLICATIONS       = "PermittedApplications";
        public const string VAL_SHOW_SEB_APPLICATION_CHOOSER = "ShowSebApplicationChooser";

        // Languages
        public const int IND_LANGUAGE_MIN = 0;
        public const int IND_LANGUAGE_GERMAN  = 0;
        public const int IND_LANGUAGE_ENGLISH = 1;
        public const int IND_LANGUAGE_FRENCH  = 2;
        public const int IND_LANGUAGE_MAX = 2;
        public const int IND_LANGUAGE_NUM = 3;

        public const int IND_MESSAGE_TEXT_MIN = 0;

        // Error codes
        public const int IND_FILE_NOT_FOUND       = 0;
        public const int IND_PATH_NOT_FOUND       = 1;
        public const int IND_ACCESS_DENIED        = 2;
        public const int IND_UNDEFINED_ERROR      = 3;
        public const int IND_NO_WRITE_PERMISSION  = 4;
        public const int IND_SEB_CLIENT_SEB_ERROR = 5;
        public const int IND_CONFIG_JSON_ERROR    = 6;
        public const int IND_NO_CLIENT_INFO_ERROR = 7;
        public const int IND_INITIALISE_ERROR     = 8;
        public const int IND_REGISTRY_EDIT_ERROR  = 9;
        public const int IND_NOT_ENOUGH_REGISTRY_RIGHTS_ERROR = 10;
        public const int IND_REGISTRY_WARNING                 = 11;
        public const int IND_PROCESS_CALL_FAILED              = 12;
        public const int IND_PROCESS_WINDOW_NOT_FOUND         = 13;
        public const int IND_LOAD_LIBRARY_ERROR               = 14;
        public const int IND_NO_LANGUAGE_STRING_FOUND         = 15;
        public const int IND_NO_INSTANCE                      = 16;
        public const int IND_NO_FILE_ERROR                    = 17;
        public const int IND_NO_TASKBAR_HANDLE                = 18;
        public const int IND_FIREFOX_START_FAILED             = 19;
        public const int IND_KEY_LOGGER_FAILED                = 20;
        public const int IND_KIOX_TERMINATED                  = 21;
        public const int IND_SEB_TERMINATED                   = 22;
        public const int IND_NO_OS_SUPPORT                    = 23;
        public const int IND_KILL_PROCESS_FAILED              = 24;
        public const int IND_VIRTUAL_MACHINE_FORBIDDEN        = 25;
        public const int IND_CLOSE_PROCESS_FAILED             = 26;
        public const int IND_WINDOWS_SERVICE_NOT_AVAILABLE    = 27;
        public const int IND_CLOSE_SEB_FAILED                 = 28;

        public const int IND_MESSAGE_TEXT_MAX = 28;
        public const int IND_MESSAGE_TEXT_NUM = 29;


        // MessageBox supports errors and warnings
        public const int IND_MESSAGE_KIND_ERROR    = 0;
        public const int IND_MESSAGE_KIND_WARNING  = 1;
        public const int IND_MESSAGE_KIND_QUESTION = 2;
        public const int IND_MESSAGE_KIND_NUM      = 3;


        public const int  OS_UNKNOWN =  800;
        public const int  WIN_95     =  950;
        public const int  WIN_98     =  980;
        public const int  WIN_ME     =  999;
        public const int  WIN_NT_351 = 1351;
        public const int  WIN_NT_40  = 1400;
        public const int  WIN_2000   = 2000;
        public const int  WIN_XP     = 2010;
        public const int WIN_VISTA   = 2050;
        public const int WIN_7       = 2050;
        public const int WIN_8       = 2050;


        // Group "General"
        public const String MessageStartURL = "startURL";
        public const String MessageSebServerURL = "sebServerURL";
        public const String MessageAdminPassword = "adminPassword";
        public const String MessageConfirmAdminPassword = "confirmAdminPassword";
        public const String MessageHashedAdminPassword = "hashedAdminPassword";
        public const String MessageAllowQuit = "allowQuit";
        public const String MessageIgnoreQuitPassword = "ignoreQuitPassword";
        public const String MessageQuitPassword = "quitPassword";
        public const String MessageConfirmQuitPassword = "confirmQuitPassword";
        public const String MessageHashedQuitPassword = "hashedQuitPassword";
        public const String MessageExitKey1 = "exitKey1";
        public const String MessageExitKey2 = "exitKey2";
        public const String MessageExitKey3 = "exitKey3";
        public const String MessageSebMode = "sebMode";

        // Group "Config File"
        public const int ValueSebConfigPurpose = 1;
        public const int ValueAllowPreferencesWindow = 2;
        public const int ValueCryptoIdentity = 3;
        public const int ValueSettingsPassword = 4;
        public const int ValueConfirmSettingsPassword = 5;
        public const int ValueHashedSettingsPassword = 6;
        public const int NumValueConfigFile = 6;

        public const String MessageSebConfigPurpose = "sebConfigPurpose";
        public const String MessageAllowPreferencesWindow = "allowPreferencesWindow";
        public const String MessageCryptoIdentity = "cryptoIdentity";
        public const String MessageSettingsPassword = "settingsPassword";
        public const String MessageConfirmSettingsPassword = "confirmSettingsPassword";
        public const String MessageHashedSettingsPassword = "hashedSettingsPassword";

        // Group "Appearance"
        public const int ValueBrowserViewMode = 1;
        public const int ValueMainBrowserWindowWidth = 2;
        public const int ValueMainBrowserWindowHeight = 3;
        public const int ValueMainBrowserWindowPositioning = 4;
        public const int ValueEnableBrowserWindowToolbar = 5;
        public const int ValueHideBrowserWindowToolbar = 6;
        public const int ValueShowMenuBar = 7;
        public const int ValueShowTaskBar = 8;
        public const int NumValueAppearance = 8;

        public const String MessageBrowserViewMode = "browserViewMode";
        public const String MessageMainBrowserWindowWidth = "mainBrowserWindowWidth";
        public const String MessageMainBrowserWindowHeight = "mainBrowserWindowHeight";
        public const String MessageMainBrowserWindowPositioning = "mainBrowserWindowPositioning";
        public const String MessageEnableBrowserWindowToolbar = "enableBrowserWindowToolbar";
        public const String MessageHideBrowserWindowToolbar = "hideBrowserWindowToolbar";
        public const String MessageShowMenuBar = "showMenuBar";
        public const String MessageShowTaskBar = "showTaskBar";

        // Group "Browser"
        public const int ValueNewBrowserWindowByLinkPolicy = 1;
        public const int ValueNewBrowserWindowByScriptPolicy = 2;
        public const int ValueNewBrowserWindowByLinkBlockForeign = 3;
        public const int ValueNewBrowserWindowByScriptBlockForeign = 4;
        public const int ValueNewBrowserWindowByLinkWidth = 5;
        public const int ValueNewBrowserWindowByLinkHeight = 6;
        public const int ValueNewBrowserWindowByLinkPositioning = 7;
        public const int ValueEnablePlugIns = 8;
        public const int ValueEnableJava = 9;
        public const int ValueEnableJavaScript = 10;
        public const int ValueBlockPopUpWindows = 11;
        public const int ValueAllowBrowsingBackForward = 12;
        public const int ValueEnableSebBrowser = 13;
        public const int NumValueBrowser = 13;

        public const String MessageNewBrowserWindowByLinkPolicy = "newBrowserWindowByLinkPolicy";
        public const String MessageNewBrowserWindowByScriptPolicy = "newBrowserWindowByScriptPolicy";
        public const String MessageNewBrowserWindowByLinkBlockForeign = "newBrowserWindowByLinkBlockForeign";
        public const String MessageNewBrowserWindowByScriptBlockForeign = "newBrowserWindowByScriptBlockForeign";
        public const String MessageNewBrowserWindowByLinkWidth = "newBrowserWindowByLinkWidth";
        public const String MessageNewBrowserWindowByLinkHeight = "newBrowserWindowByLinkHeight";
        public const String MessageNewBrowserWindowByLinkPositioning = "newBrowserWindowByLinkPositioning";
        public const String MessageEnablePlugIns = "enablePlugIns";
        public const String MessageEnableJava = "enableJava";
        public const String MessageEnableJavaScript = "enableJavaScript";
        public const String MessageBlockPopUpWindows = "blockPopUpWindows";
        public const String MessageAllowBrowsingBackForward = "allowBrowsingBackForward";
        public const String MessageEnableSebBrowser = "enableSebBrowser";

        // Group "DownUploads"
        public const int ValueAllowDownUploads = 1;
        public const int ValueDownloadDirectoryOSX = 2;
        public const int ValueDownloadDirectoryWin = 3;
        public const int ValueOpenDownloads = 4;
        public const int ValueChooseFileToUploadPolicy = 5;
        public const int ValueDownloadPDFFiles = 6;
        public const int NumValueDownUploads = 6;

        public const String MessageAllowDownUploads = "allowDownUploads";
        public const String MessageDownloadDirectoryOSX = "downloadDirectoryOSX";
        public const String MessageDownloadDirectoryWin = "downloadDirectoryWin";
        public const String MessageOpenDownloads = "openDownloads";
        public const String MessageChooseFileToUploadPolicy = "chooseFileToUploadPolicy";
        public const String MessageDownloadPDFFiles = "downloadPDFFiles";

        // Group "Exam"
        public const int ValueExamKeySalt = 1;
        public const int ValueBrowserExamKey = 2;
        public const int ValueCopyBrowserExamKey = 3;
        public const int ValueSendBrowserExamKey = 4;
        public const int ValueQuitURL = 5;
        public const int NumValueExam = 5;

        public const String MessageExamKeySalt = "examKeySalt";
        public const String MessageBrowserExamKey = "browserExamKey";
        public const String MessageCopyBrowserExamKey = "copyBrowserExamKeyToClipboardWhenQuitting";
        public const String MessageSendBrowserExamKey = "sendBrowserExamKey";
        public const String MessageQuitURL = "quitURL";

        // Group "Applications"
        public const int ValueMonitorProcesses = 1;
        public const int ValuePermittedProcesses = 2;
        public const int ValueAllowSwitchToApplications = 3;
        public const int ValueAllowFlashFullscreen = 4;
        public const int ValueProhibitedProcesses = 5;
        public const int NumValueApplications = 5;

        public const String MessageMonitorProcesses = "monitorProcesses";
        public const String MessagePermittedProcesses = "permittedProcesses";
        public const String MessageAllowSwitchToApplications = "allowSwitchToApplications";
        public const String MessageAllowFlashFullscreen = "allowFlashFullscreen";
        public const String MessageProhibitedProcesses = "prohibitedProcesses";

        public const String MessageActive = "active";
        public const String MessageAutostart = "autostart";
        public const String MessageAutohide = "autohide";
        public const String MessageAllowUser = "allowUserToChooseApp";
        public const String MessageCurrentUser = "currentUser";
        public const String MessageStrongKill = "strongKill";
        public const String MessageOS = "os";
        public const String MessageTitle = "title";
        public const String MessageDescription = "description";
        public const String MessageExecutable = "executable";
        public const String MessagePath = "path";
        public const String MessageIdentifier = "identifier";
        public const String MessageUser = "user";
        public const String MessageArguments = "arguments";
        public const String MessageArgument = "argument";

        // Group "Network"
        public const String MessageEnableURLFilter = "enableURLFilter";
        public const String MessageEnableURLContentFilter = "enableURLContentFilter";

        // Group "Network - Filter"
        public const String MessageURLFilterRules = "URLFilterRules";
        public const String MessageExpression = "expression";
        public const String MessageRuleActions = "ruleActions";
        public const String MessageRegex = "regex";
        public const String MessageAction = "action";

        // Group "Filter"
        public const int ValueEnableURLFilter = 1;
        public const int ValueEnableURLContentFilter = 2;
        public const int ValueURLFilterRules = 3;
        public const int ValueProxySettingsPolicy = 4;
        public const int ValueProxies = 5;
        public const int NumValueNetwork = 5;

        // Group "Network - Certificates"
        public const String MessageEmbedSSLServerCertificate = "EmbedSSLServerCertificate";
        public const String MessageEmbedIdentity = "EmbedIdentity";
        public const String MessageEmbeddedCertificates = "embeddedCertificates";
        public const String MessageCertificateData = "certificateData";
        public const String MessageType = "type";
        public const String MessageName = "name";

        // Group "Network - Proxies"
        public const String MessageProxyProtocol = "proxyProtocol";
        public const String MessageProxyConfigurationFileURL = "proxyConfigurationFileURL";
        public const String MessageExcludeSimpleHostnames = "excludeSimpleHostnames";
        public const String MessageUsePassiveFTPMode = "usePassiveFTPMode";
        public const String MessageBypassHostsAndDomains = "bypassHostsAndDomains";
        public const String MessageBypassDomain = "domain";
        public const String MessageBypassHost = "host";
        public const String MessageBypassPort = "port";

        //public const String MessageEnableURLFilter = "enableURLFilter";
        //public const String MessageEnableURLContentFilter = "enableURLContentFilter";
        //public const String MessageURLFilterRules = "URLFilterRules";
        public const String MessageProxySettingsPolicy = "proxySettingsPolicy";
        public const String MessageProxies = "proxies";

        // Group "Security"
        public const int ValueSebServicePolicy = 1;
        public const int ValueAllowVirtualMachine = 2;
        public const int ValueCreateNewDesktop = 3;
        public const int ValueKillExplorerShell = 4;
        public const int ValueAllowUserSwitching = 5;
        public const int ValueEnableLogging = 6;
        public const int ValueLogDirectoryOSX = 7;
        public const int ValueLogDirectoryWin = 8;
        public const int NumValueSecurity = 8;

        public const String MessageSebServicePolicy = "sebServicePolicy";
        public const String MessageAllowVirtualMachine = "allowVirtualMachine";
        public const String MessageCreateNewDesktop = "createNewDesktop";
        public const String MessageKillExplorerShell = "killExplorerShell";
        public const String MessageAllowUserSwitching = "allowUserSwitching";
        public const String MessageEnableLogging = "enableLogging";
        public const String MessageLogDirectoryOSX = "logDirectoryOSX";
        public const String MessageLogDirectoryWin = "logDirectoryWin";

        // Group "Registry"
        public const int NumValueRegistry = 0;

        // Group "Inside SEB"
        public const int ValueInsideSebEnableSwitchUser = 1;
        public const int ValueInsideSebEnableLockThisComputer = 2;
        public const int ValueInsideSebEnableChangeAPassword = 3;
        public const int ValueInsideSebEnableStartTaskManager = 4;
        public const int ValueInsideSebEnableLogOff = 5;
        public const int ValueInsideSebEnableShutDown = 6;
        public const int ValueInsideSebEnableEaseOfAccess = 7;
        public const int ValueInsideSebEnableVmWareClientShade = 8;
        public const int NumValueInsideSeb = 8;

        // Group "Outside SEB"
        public const int ValueOutsideSebEnableSwitchUser = 1;
        public const int ValueOutsideSebEnableLockThisComputer = 2;
        public const int ValueOutsideSebEnableChangeAPassword = 3;
        public const int ValueOutsideSebEnableStartTaskManager = 4;
        public const int ValueOutsideSebEnableLogOff = 5;
        public const int ValueOutsideSebEnableShutDown = 6;
        public const int ValueOutsideSebEnableEaseOfAccess = 7;
        public const int ValueOutsideSebEnableVmWareClientShade = 8;
        public const int NumValueOutsideSeb = 8;

        public const String MessageInsideSebEnableSwitchUser = "insideSebEnableSwitchUser";
        public const String MessageInsideSebEnableLockThisComputer = "insideSebEnableLockThisComputer";
        public const String MessageInsideSebEnableChangeAPassword = "insideSebEnableChangeAPassword";
        public const String MessageInsideSebEnableStartTaskManager = "insideSebEnableStartTaskManager";
        public const String MessageInsideSebEnableLogOff = "insideSebEnableLogOff";
        public const String MessageInsideSebEnableShutDown = "insideSebEnableShutDown";
        public const String MessageInsideSebEnableEaseOfAccess = "insideSebEnableEaseOfAccess";
        public const String MessageInsideSebEnableVmWareClientShade = "insideSebEnableVmWareClientShade";

        public const String MessageOutsideSebEnableSwitchUser = "outsideSebEnableSwitchUser";
        public const String MessageOutsideSebEnableLockThisComputer = "outsideSebEnableLockThisComputer";
        public const String MessageOutsideSebEnableChangeAPassword = "outsideSebEnableChangeAPassword";
        public const String MessageOutsideSebEnableStartTaskManager = "outsideSebEnableStartTaskManager";
        public const String MessageOutsideSebEnableLogOff = "outsideSebEnableLogOff";
        public const String MessageOutsideSebEnableShutDown = "outsideSebEnableShutDown";
        public const String MessageOutsideSebEnableEaseOfAccess = "outsideSebEnableEaseOfAccess";
        public const String MessageOutsideSebEnableVmWareClientShade = "outsideSebEnableVmWareClientShade";

        // Group "Hooked Keys"
        public const int ValueHookKeys = 1;
        public const int NumValueHookedKeys = 1;

        public const String MessageHookKeys = "hookKeys";

        // Group "Special Keys"
        public const int ValueEnableEsc = 1;
        public const int ValueEnableCtrlEsc = 2;
        public const int ValueEnableAltEsc = 3;
        public const int ValueEnableAltTab = 4;
        public const int ValueEnableAltF4 = 5;
        public const int ValueEnableStartMenu = 6;
        public const int ValueEnableRightMouse = 7;
        public const int NumValueSpecialKeys = 7;

        public const String MessageEnableEsc = "enableEsc";
        public const String MessageEnableCtrlEsc = "enableCtrlEsc";
        public const String MessageEnableAltEsc = "enableAltEsc";
        public const String MessageEnableAltTab = "enableAltTab";
        public const String MessageEnableAltF4 = "enableAltF4";
        public const String MessageEnableStartMenu = "enableStartMenu";
        public const String MessageEnableRightMouse = "enableRightMouse";

        // Group "Function Keys"
        public const int ValueEnableF1 = 1;
        public const int ValueEnableF2 = 2;
        public const int ValueEnableF3 = 3;
        public const int ValueEnableF4 = 4;
        public const int ValueEnableF5 = 5;
        public const int ValueEnableF6 = 6;
        public const int ValueEnableF7 = 7;
        public const int ValueEnableF8 = 8;
        public const int ValueEnableF9 = 9;
        public const int ValueEnableF10 = 10;
        public const int ValueEnableF11 = 11;
        public const int ValueEnableF12 = 12;
        public const int NumValueFunctionKeys = 12;

        public const String MessageEnableF1 = "enableF1";
        public const String MessageEnableF2 = "enableF2";
        public const String MessageEnableF3 = "enableF3";
        public const String MessageEnableF4 = "enableF4";
        public const String MessageEnableF5 = "enableF5";
        public const String MessageEnableF6 = "enableF6";
        public const String MessageEnableF7 = "enableF7";
        public const String MessageEnableF8 = "enableF8";
        public const String MessageEnableF9 = "enableF9";
        public const String MessageEnableF10 = "enableF10";
        public const String MessageEnableF11 = "enableF11";
        public const String MessageEnableF12 = "enableF12";


        // Types of values
        public const int TypeBoolean = 1;
        public const int TypeString = 2;
        public const int TypeInteger = 3;

        public static bool explorerShellWasKilled = false;

        #endregion
    }
}
