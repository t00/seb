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


        // ******************************
        //
        // Constants and global variables
        //
        // ******************************


        // SEB has 3 different configuration file types:
        // .ini, .xml and .seb
        const int FileSebStarterIni = 1;
        const int FileSebStarterXml = 2;
        const int FileSebStarterSeb = 3;
        const int FileNum = 3;

        // The default SEB configuration file
        const String DefaultSebConfigIni = "SebClient.ini";
        const String DefaultSebConfigXml = "SebClient.xml";
        const String DefaultSebConfigSeb = "SebClient.seb";

        // The values can be in 4 different states:
        // old, new, temporary and default values
        const int StateOld = 1;
        const int StateNew = 2;
        const int StateTmp = 3;
        const int StateDef = 4;
        const int StateNum = 4;

        // The Graphical User Interface contains 15 groups
        const int GroupNum = 15;

        // SebStarter contains 15 groups
        const int GroupGeneral      = 1;
        const int GroupConfigFile   = 2;
        const int GroupAppearance   = 3;
        const int GroupBrowser      = 4;
        const int GroupDownUploads  = 5;
        const int GroupExam         = 6;
        const int GroupApplications = 7;
        const int GroupNetwork      = 8;
        const int GroupSecurity     = 9;
        const int GroupRegistry     = 10;
        const int GroupInsideSeb    = 11;
        const int GroupOutsideSeb   = 12;
        const int GroupHookedKeys   = 13;
        const int GroupSpecialKeys  = 14;
        const int GroupFunctionKeys = 15;

        const int GroupNumSebStarter = 15;

        // Each group contains up to 20 values
        const int ValueNum = 20;

        // Group names
        const String MessageGeneral      = "General";
        const String MessageConfigFile   = "ConfigFile";
        const String MessageAppearance   = "Appearance";
        const String MessageBrowser      = "Browser";
        const String MessageDownUploads  = "DownUploads";
        const String MessageExam         = "Exam";
        const String MessageApplications = "Applications";
        const String MessageNetwork      = "Network";
        const String MessageSecurity     = "Security";
        const String MessageRegistry     = "Registry";
        const String MessageInsideSeb    = "InsideSeb";
        const String MessageOutsideSeb   = "OutsideSeb";
        const String MessageHookedKeys   = "HookedKeys";
        const String MessageSpecialKeys  = "SpecialKeys";
        const String MessageFunctionKeys = "FunctionKeys";

        // Group "General"
        const int ValueStartURL             = 1;
        const int ValueSebServerURL         = 2;
        const int ValueAdminPassword        = 3;
        const int ValueConfirmAdminPassword = 4;
        const int ValueHashedAdminPassword  = 5;
        const int ValueAllowQuit            = 6;
        const int ValueIgnoreQuitPassword   = 7;
        const int ValueQuitPassword         = 8;
        const int ValueConfirmQuitPassword  = 9;
        const int ValueHashedQuitPassword   = 10;
        const int ValueExitKey1             = 11;
        const int ValueExitKey2             = 12;
        const int ValueExitKey3             = 13;
        const int ValueSebMode              = 14;
        const int NumValueGeneral = 14;

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
        const int ValueSebConfigPurpose        = 1;
        const int ValueAllowPreferencesWindow  = 2;
        const int ValueCryptoIdentity          = 3;
        const int ValueSettingsPassword        = 4;
        const int ValueConfirmSettingsPassword = 5;
        const int ValueHashedSettingsPassword  = 6;
        const int NumValueConfigFile = 6;

        const String MessageSebConfigPurpose        = "sebConfigPurpose";
        const String MessageAllowPreferencesWindow  = "allowPreferencesWindow";
        const String MessageCryptoIdentity          = "cryptoIdentity";
        const String MessageSettingsPassword        = "settingsPassword";
        const String MessageConfirmSettingsPassword = "confirmSettingsPassword";
        const String MessageHashedSettingsPassword  = "hashedSettingsPassword";

        // Group "Appearance"
        const int ValueBrowserViewMode              = 1;
        const int ValueMainBrowserWindowWidth       = 2;
        const int ValueMainBrowserWindowHeight      = 3;
        const int ValueMainBrowserWindowPositioning = 4;
        const int ValueEnableBrowserWindowToolbar   = 5;
        const int ValueHideBrowserWindowToolbar     = 6;
        const int ValueShowMenuBar                  = 7;
        const int ValueShowTaskBar                  = 8;
        const int NumValueAppearance = 8;

        const String MessageBrowserViewMode              = "browserViewMode";
        const String MessageMainBrowserWindowWidth       = "mainBrowserWindowWidth";
        const String MessageMainBrowserWindowHeight      = "mainBrowserWindowHeight";
        const String MessageMainBrowserWindowPositioning = "mainBrowserWindowPositioning";
        const String MessageEnableBrowserWindowToolbar   = "enableBrowserWindowToolbar";
        const String MessageHideBrowserWindowToolbar     = "hideBrowserWindowToolbar";
        const String MessageShowMenuBar                  = "showMenuBar";
        const String MessageShowTaskBar                  = "showTaskBar";

        // Group "Browser"
        const int ValueNewBrowserWindowByLinkPolicy         = 1;
        const int ValueNewBrowserWindowByScriptPolicy       = 2;
        const int ValueNewBrowserWindowByLinkBlockForeign   = 3;
        const int ValueNewBrowserWindowByScriptBlockForeign = 4;
        const int ValueNewBrowserWindowByLinkWidth          = 5;
        const int ValueNewBrowserWindowByLinkHeight         = 6;
        const int ValueNewBrowserWindowByLinkPositioning    = 7;
        const int ValueEnablePlugIns                        = 8;
        const int ValueEnableJava                           = 9;
        const int ValueEnableJavaScript                     = 10;
        const int ValueBlockPopUpWindows                    = 11;
        const int ValueAllowBrowsingBackForward             = 12;
        const int ValueEnableSebBrowser                     = 13;
        const int NumValueBrowser = 13;

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
        const int ValueAllowDownUploads         = 1;
        const int ValueDownloadDirectoryOSX     = 2;
        const int ValueDownloadDirectoryWin     = 3;
        const int ValueOpenDownloads            = 4;
        const int ValueChooseFileToUploadPolicy = 5;
        const int ValueDownloadPDFFiles         = 6;
        const int NumValueDownUploads = 6;

        const String MessageAllowDownUploads         = "allowDownUploads";
        const String MessageDownloadDirectoryOSX     = "downloadDirectoryOSX";
        const String MessageDownloadDirectoryWin     = "downloadDirectoryWin";
        const String MessageOpenDownloads            = "openDownloads";
        const String MessageChooseFileToUploadPolicy = "chooseFileToUploadPolicy";
        const String MessageDownloadPDFFiles         = "downloadPDFFiles";

        // Group "Exam"
        const int ValueExamKeySalt        = 1;
        const int ValueBrowserExamKey     = 2;
        const int ValueCopyBrowserExamKey = 3;
        const int ValueSendBrowserExamKey = 4;
        const int ValueQuitURL            = 5;
        const int NumValueExam = 5;

        const String MessageExamKeySalt        = "examKeySalt";
        const String MessageBrowserExamKey     = "browserExamKey";
        const String MessageCopyBrowserExamKey = "copyBrowserExamKeyToClipboardWhenQuitting";
        const String MessageSendBrowserExamKey = "sendBrowserExamKey";
        const String MessageQuitURL            = "quitURL";

        // Group "Applications"
        const int ValueMonitorProcesses          = 1;
        const int ValuePermittedProcesses        = 2;
        const int ValueAllowSwitchToApplications = 3;
        const int ValueAllowFlashFullscreen      = 4;
        const int ValueProhibitedProcesses       = 5;
        const int NumValueApplications = 5;

        const String MessageMonitorProcesses          = "monitorProcesses";
        const String MessagePermittedProcesses        = "permittedProcesses";
        const String MessageAllowSwitchToApplications = "allowSwitchToApplications";
        const String MessageAllowFlashFullscreen      = "allowFlashFullscreen";
        const String MessageProhibitedProcesses       = "prohibitedProcesses";

        const String MessageActive      = "active";
        const String MessageAutostart   = "autostart";
        const String MessageAutohide    = "autohide";
        const String MessageAllowUser   = "allowUserToChooseApp";
        const String MessageCurrentUser = "currentUser";
        const String MessageStrongKill  = "strongKill";
        const String MessageOS          = "os";
        const String MessageAppTitle    = "appTitle";
        const String MessageDescription = "description";
        const String MessageExecutable  = "executable";
        const String MessagePath        = "path";
        const String MessageIdentifier  = "identifier";
        const String MessageUser        = "user";
        const String MessageArguments   = "arguments";


        // Group "Network"

        // Group "Filter"
        const int ValueEnableURLFilter        = 1;
        const int ValueEnableURLContentFilter = 2;
        const int ValueURLFilterRules         = 3;
        const int ValueProxySettingsPolicy    = 4;
        const int ValueProxies                = 5;
        const int NumValueNetwork = 5;

        const String MessageEnableURLFilter        = "enableURLFilter";
        const String MessageEnableURLContentFilter = "enableURLContentFilter";
        const String MessageURLFilterRules         = "URLFilterRules";
        const String MessageProxySettingsPolicy    = "proxySettingsPolicy";
        const String MessageProxies                = "proxies";

        // Group "Security"
        const int ValueSebServicePolicy    = 1;
        const int ValueAllowVirtualMachine = 2;
        const int ValueCreateNewDesktop    = 3;
        const int ValueAllowUserSwitching  = 4;
        const int ValueEnableLogging       = 5;
        const int ValueLogDirectoryOSX     = 6;
        const int ValueLogDirectoryWin     = 7;
        const int NumValueSecurity = 7;

        const String MessageSebServicePolicy    = "sebServicePolicy";
        const String MessageAllowVirtualMachine = "allowVirtualMachine";
        const String MessageCreateNewDesktop    = "createNewDesktop";
        const String MessageAllowUserSwitching  = "allowUserSwitching";
        const String MessageEnableLogging       = "enableLogging";
        const String MessageLogDirectoryOSX     = "logDirectoryOSX";
        const String MessageLogDirectoryWin     = "logDirectoryWin";

        // Group "Registry"
        const int NumValueRegistry = 0;

        // Group "Inside SEB"
        const int ValueInsideSebEnableSwitchUser        = 1;
        const int ValueInsideSebEnableLockThisComputer  = 2;
        const int ValueInsideSebEnableChangeAPassword   = 3;
        const int ValueInsideSebEnableStartTaskManager  = 4;
        const int ValueInsideSebEnableLogOff            = 5;
        const int ValueInsideSebEnableShutDown          = 6;
        const int ValueInsideSebEnableEaseOfAccess      = 7;
        const int ValueInsideSebEnableVmWareClientShade = 8;
        const int NumValueInsideSeb  = 8;

        // Group "Outside SEB"
        const int ValueOutsideSebEnableSwitchUser        = 1;
        const int ValueOutsideSebEnableLockThisComputer  = 2;
        const int ValueOutsideSebEnableChangeAPassword   = 3;
        const int ValueOutsideSebEnableStartTaskManager  = 4;
        const int ValueOutsideSebEnableLogOff            = 5;
        const int ValueOutsideSebEnableShutDown          = 6;
        const int ValueOutsideSebEnableEaseOfAccess      = 7;
        const int ValueOutsideSebEnableVmWareClientShade = 8;
        const int NumValueOutsideSeb  = 8;

        const String MessageInsideSebEnableSwitchUser        = "insideSebEnableSwitchUser";
        const String MessageInsideSebEnableLockThisComputer  = "insideSebEnableLockThisComputer";
        const String MessageInsideSebEnableChangeAPassword   = "insideSebEnableChangeAPassword";
        const String MessageInsideSebEnableStartTaskManager  = "insideSebEnableStartTaskManager";
        const String MessageInsideSebEnableLogOff            = "insideSebEnableLogOff";
        const String MessageInsideSebEnableShutDown          = "insideSebEnableShutDown";
        const String MessageInsideSebEnableEaseOfAccess      = "insideSebEnableEaseOfAccess";
        const String MessageInsideSebEnableVmWareClientShade = "insideSebEnableVmWareClientShade";

        const String MessageOutsideSebEnableSwitchUser        = "outsideSebEnableSwitchUser";
        const String MessageOutsideSebEnableLockThisComputer  = "outsideSebEnableLockThisComputer";
        const String MessageOutsideSebEnableChangeAPassword   = "outsideSebEnableChangeAPassword";
        const String MessageOutsideSebEnableStartTaskManager  = "outsideSebEnableStartTaskManager";
        const String MessageOutsideSebEnableLogOff            = "outsideSebEnableLogOff";
        const String MessageOutsideSebEnableShutDown          = "outsideSebEnableShutDown";
        const String MessageOutsideSebEnableEaseOfAccess      = "outsideSebEnableEaseOfAccess";
        const String MessageOutsideSebEnableVmWareClientShade = "outsideSebEnableVmWareClientShade";

        // Group "Hooked Keys"
        const int ValueHookKeys = 1;
        const int NumValueHookedKeys = 1;

        const String MessageHookKeys = "hookKeys";

        // Group "Special Keys"
        const int ValueEnableEsc        = 1;
        const int ValueEnableCtrlEsc    = 2;
        const int ValueEnableAltEsc     = 3;
        const int ValueEnableAltTab     = 4;
        const int ValueEnableAltF4      = 5;
        const int ValueEnableStartMenu  = 6;
        const int ValueEnableRightMouse = 7;
        const int NumValueSpecialKeys = 7;

        const String MessageEnableEsc        = "enableEsc";
        const String MessageEnableCtrlEsc    = "enableCtrlEsc";
        const String MessageEnableAltEsc     = "enableAltEsc";
        const String MessageEnableAltTab     = "enableAltTab";
        const String MessageEnableAltF4      = "enableAltF4";
        const String MessageEnableStartMenu  = "enableStartMenu";
        const String MessageEnableRightMouse = "enableRightMouse";

        // Group "Function Keys"
        const int ValueEnableF1  = 1;
        const int ValueEnableF2  = 2;
        const int ValueEnableF3  = 3;
        const int ValueEnableF4  = 4;
        const int ValueEnableF5  = 5;
        const int ValueEnableF6  = 6;
        const int ValueEnableF7  = 7;
        const int ValueEnableF8  = 8;
        const int ValueEnableF9  = 9;
        const int ValueEnableF10 = 10;
        const int ValueEnableF11 = 11;
        const int ValueEnableF12 = 12;
        const int NumValueFunctionKeys = 12;

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


        // Types of values
        const int TypeBoolean = 1;
        const int TypeString  = 2;
        const int TypeInteger = 3;


        // Global variables

        // The current SEB configuration file
        String currentDireSebConfigFile;
        String currentFileSebConfigFile;
        String currentPathSebConfigFile;

        // The default SEB configuration file
        String defaultDireSebConfigFile;
        String defaultFileSebConfigFile;
        String defaultPathSebConfigFile;

        // Strings for encryption identities (KeyChain, Certificate Store)
        //static ArrayList chooseIdentityStringArrayList = new ArrayList();
        //static String[]  chooseIdentityStringArray = new String[1];
        static List<String> StringCryptoIdentity = new List<String>();

        // Entries of ListBoxes
      //static   Byte[] ByteArrayExamKeySalt    = new Byte[] {};
        static String[] StringCryptoIdentityArray;
        static String[] StringSebPurpose          = new String[2];
        static String[] StringSebMode             = new String[2];
        static String[] StringBrowserViewMode     = new String[2];
        static String[] StringWindowWidth         = new String[4];
        static String[] StringWindowHeight        = new String[4];
        static String[] StringWindowPositioning   = new String[3];
        static String[] StringPolicyLinkOpening   = new String[3];
        static String[] StringPolicyFileUpload    = new String[3];
        static String[] StringPolicyProxySettings = new String[2];
        static String[] StringPolicySebService    = new String[3];
        static String[] StringFunctionKey         = new String[12];
        static String[] StringColumnsProcessesPermitted  = new String[4];
        static String[] StringColumnsProcessesProhibited = new String[4];
        static String[] StringActive              = {"true", "false"};
        static String[] StringOperatingSystem     = new String[2];


        // Number of values per group
        // Names  of groups and values
        // Types  of values (Boolean, Integer, String)
        static    int[ ]   minValue  = new    int[GroupNum + 1];
        static    int[ ]   maxValue  = new    int[GroupNum + 1];
        static String[ ]  fileString = new String[ FileNum + 1];
        static String[ ] groupString = new String[GroupNum + 1];
        static String[,] valueString = new String[GroupNum + 1, ValueNum + 1];
        static    int[,]  dataType   = new    int[GroupNum + 1, ValueNum + 1];

        // Settings as Booleans ("true" or "false") or Strings
        static Boolean[,,] settingBoolean = new Boolean[StateNum + 1, GroupNum + 1, ValueNum + 1];
        static String [,,] settingString  = new String [StateNum + 1, GroupNum + 1, ValueNum + 1];
        static     int[,,] settingInteger = new     int[StateNum + 1, GroupNum + 1, ValueNum + 1];

        // Password encryption using the SHA-256 hash algorithm
        SHA256 sha256 = new SHA256Managed();

        // Class SEBSettings contains all settings
        // and is used for importing/exporting the settings
        // from/to a human-readable .xml and an encrypted.seb file format.
        static Dictionary<string, object> sebSettingsOld = new Dictionary<string, object>();
        static Dictionary<string, object> sebSettingsNew = new Dictionary<string, object>();
        static Dictionary<string, object> sebSettingsTmp = new Dictionary<string, object>();
        static Dictionary<string, object> sebSettingsDef = new Dictionary<string, object>();

        static SEBClientConfig            sebSettObso        = new SEBClientConfig();
        static SEBProtectionController    sebController      = new SEBProtectionController();
      //static XmlSerializer              sebSerializerPlist = new XmlSerializer(typeof(Dictionary<string, object>));
        static XmlSerializer              sebSerializerObso  = new XmlSerializer(typeof(SEBClientConfig));



        // ***********
        //
        // Constructor
        //
        // ***********



        public SebWindowsConfigForm()
        {
            InitializeComponent();

            // Initialise the global arrays
            int state, group, value;

            // Intialise the Safe Exam Browser values
            for (state = 1; state <= StateNum; state++)
            for (group = 1; group <= GroupNum; group++)
            for (value = 1; value <= ValueNum; value++)
            {
                settingBoolean[state, group, value] = false;
                settingInteger[state, group, value] = 0;
                settingString [state, group, value] = "";
            }

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
            settingInteger[StateDef, GroupConfigFile, ValueCryptoIdentity] = 0;
            settingString [StateDef, GroupConfigFile, ValueCryptoIdentity] = "";


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
            settingInteger[StateDef, GroupAppearance, ValueMainBrowserWindowWidth ] = 1;
            settingInteger[StateDef, GroupAppearance, ValueMainBrowserWindowHeight] = 1;
            settingString [StateDef, GroupAppearance, ValueMainBrowserWindowWidth ] = "100%";
            settingString [StateDef, GroupAppearance, ValueMainBrowserWindowHeight] = "100%";


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
            settingInteger[StateDef, GroupBrowser, ValueNewBrowserWindowByLinkWidth ] = 3;
            settingInteger[StateDef, GroupBrowser, ValueNewBrowserWindowByLinkHeight] = 1;
            settingString [StateDef, GroupBrowser, ValueNewBrowserWindowByLinkWidth ] = "1000";
            settingString [StateDef, GroupBrowser, ValueNewBrowserWindowByLinkHeight] = "100%";


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

            // Default settings for group "Network"
            sebSettingsDef.Add(MessageEnableURLFilter       , false);
            sebSettingsDef.Add(MessageEnableURLContentFilter, false);
            sebSettingsDef.Add(MessageURLFilterRules        , new List<object>());
            sebSettingsDef.Add(MessageProxySettingsPolicy   , 0);
            sebSettingsDef.Add(MessageProxies               , new Dictionary<string, object>());

            // Default settings for group "Security"
            sebSettingsDef.Add(MessageSebServicePolicy   , 2);
            sebSettingsDef.Add(MessageAllowVirtualMachine, false);
            sebSettingsDef.Add(MessageCreateNewDesktop   , true);
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

            // Default settings for group "Intercepted Keys"
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

            settingString[StateDef, GroupOnlineExam, ValueSebBrowser           ,  SebBrowserString);
            settingString[StateDef, GroupOnlineExam, ValueAutostartProcess     , "Seb");
            settingString[StateDef, GroupOnlineExam, ValuePermittedApplications, "Calculator,calc.exe;Notepad,notepad.exe;");
*/

            // Standard data types of the different groups
            for (value = 1; value <= ValueNum; value++)
            {
                dataType[GroupGeneral     , value] = TypeString;
                dataType[GroupConfigFile  , value] = TypeBoolean;
                dataType[GroupAppearance  , value] = TypeBoolean;
                dataType[GroupBrowser     , value] = TypeBoolean;
                dataType[GroupDownUploads , value] = TypeBoolean;
                dataType[GroupExam        , value] = TypeBoolean;
                dataType[GroupApplications, value] = TypeBoolean;
                dataType[GroupNetwork     , value] = TypeBoolean;
                dataType[GroupSecurity    , value] = TypeBoolean;
                dataType[GroupRegistry    , value] = TypeBoolean;
                dataType[GroupInsideSeb   , value] = TypeBoolean;
                dataType[GroupOutsideSeb  , value] = TypeBoolean;
                dataType[GroupHookedKeys  , value] = TypeBoolean;
                dataType[GroupSpecialKeys , value] = TypeBoolean;
                dataType[GroupFunctionKeys, value] = TypeBoolean;
            }

            // Exceptional data types of some special values
            dataType[GroupGeneral, ValueAllowQuit         ] = TypeBoolean;
            dataType[GroupGeneral, ValueIgnoreQuitPassword] = TypeBoolean;

            dataType[GroupConfigFile, ValueCryptoIdentity         ] = TypeString;
            dataType[GroupConfigFile, ValueSettingsPassword       ] = TypeString;
            dataType[GroupConfigFile, ValueConfirmSettingsPassword] = TypeString;

            dataType[GroupAppearance, ValueBrowserViewMode             ] = TypeString;
            dataType[GroupAppearance, ValueMainBrowserWindowWidth      ] = TypeString;
            dataType[GroupAppearance, ValueMainBrowserWindowHeight     ] = TypeString;
            dataType[GroupAppearance, ValueMainBrowserWindowPositioning] = TypeString;

            dataType[GroupBrowser, ValueNewBrowserWindowByLinkWidth      ] = TypeString;
            dataType[GroupBrowser, ValueNewBrowserWindowByLinkHeight     ] = TypeString;
            dataType[GroupBrowser, ValueNewBrowserWindowByLinkPositioning] = TypeString;
            dataType[GroupBrowser, ValueNewBrowserWindowByLinkPolicy     ] = TypeString;
            dataType[GroupBrowser, ValueNewBrowserWindowByScriptPolicy   ] = TypeString;

            dataType[GroupDownUploads, ValueDownloadDirectoryOSX    ] = TypeString;
            dataType[GroupDownUploads, ValueDownloadDirectoryWin    ] = TypeString;
            dataType[GroupDownUploads, ValueChooseFileToUploadPolicy] = TypeString;

            dataType[GroupExam, ValueExamKeySalt   ] = TypeString;
            dataType[GroupExam, ValueBrowserExamKey] = TypeString;
            dataType[GroupExam, ValueQuitURL       ] = TypeString;

            dataType[GroupApplications, ValuePermittedProcesses ] = TypeString;
            dataType[GroupApplications, ValueProhibitedProcesses] = TypeString;

            dataType[GroupNetwork, ValueURLFilterRules     ] = TypeString;
            dataType[GroupNetwork, ValueProxySettingsPolicy] = TypeString;
            dataType[GroupNetwork, ValueProxies            ] = TypeString;

            dataType[GroupSecurity, ValueSebServicePolicy] = TypeString;
            dataType[GroupSecurity, ValueLogDirectoryOSX ] = TypeString;
            dataType[GroupSecurity, ValueLogDirectoryWin ] = TypeString;


            // Number of values per group
            for (group = 1; group <= GroupNum; group++)
            {
                minValue[group] = 1;
            }

            maxValue[GroupGeneral     ] = NumValueGeneral;
            maxValue[GroupConfigFile  ] = NumValueConfigFile;
            maxValue[GroupAppearance  ] = NumValueAppearance;
            maxValue[GroupBrowser     ] = NumValueBrowser;
            maxValue[GroupDownUploads ] = NumValueDownUploads;
            maxValue[GroupExam        ] = NumValueExam;
            maxValue[GroupApplications] = NumValueApplications;
            maxValue[GroupNetwork     ] = NumValueNetwork;
            maxValue[GroupSecurity    ] = NumValueSecurity;
            maxValue[GroupRegistry    ] = NumValueRegistry;
            maxValue[GroupInsideSeb   ] = NumValueInsideSeb;
            maxValue[GroupOutsideSeb  ] = NumValueOutsideSeb;
            maxValue[GroupHookedKeys  ] = NumValueHookedKeys;
            maxValue[GroupSpecialKeys ] = NumValueSpecialKeys;
            maxValue[GroupFunctionKeys] = NumValueFunctionKeys;


            // File names
            fileString[FileSebStarterIni] = DefaultSebConfigIni;
            fileString[FileSebStarterXml] = DefaultSebConfigXml;
            fileString[FileSebStarterSeb] = DefaultSebConfigSeb;

            // Group names
            groupString[GroupGeneral     ] = MessageGeneral;
            groupString[GroupConfigFile  ] = MessageConfigFile;
            groupString[GroupAppearance  ] = MessageAppearance;
            groupString[GroupBrowser     ] = MessageBrowser;
            groupString[GroupDownUploads ] = MessageDownUploads;
            groupString[GroupExam        ] = MessageExam;
            groupString[GroupApplications] = MessageApplications;
            groupString[GroupNetwork     ] = MessageNetwork;
            groupString[GroupSecurity    ] = MessageSecurity;
            groupString[GroupRegistry    ] = MessageRegistry;
            groupString[GroupInsideSeb   ] = MessageInsideSeb;
            groupString[GroupOutsideSeb  ] = MessageOutsideSeb;
            groupString[GroupHookedKeys  ] = MessageHookedKeys;
            groupString[GroupSpecialKeys ] = MessageSpecialKeys;
            groupString[GroupFunctionKeys] = MessageFunctionKeys;

            // Value names
            valueString[GroupGeneral, ValueStartURL            ] = MessageStartURL;
            valueString[GroupGeneral, ValueSebServerURL        ] = MessageSebServerURL;
            valueString[GroupGeneral, ValueAdminPassword       ] = MessageAdminPassword;
            valueString[GroupGeneral, ValueConfirmAdminPassword] = MessageConfirmAdminPassword;
            valueString[GroupGeneral, ValueHashedAdminPassword ] = MessageHashedAdminPassword;
            valueString[GroupGeneral, ValueAllowQuit           ] = MessageAllowQuit;
            valueString[GroupGeneral, ValueIgnoreQuitPassword  ] = MessageIgnoreQuitPassword;
            valueString[GroupGeneral, ValueQuitPassword        ] = MessageQuitPassword;
            valueString[GroupGeneral, ValueConfirmQuitPassword ] = MessageConfirmQuitPassword;
            valueString[GroupGeneral, ValueHashedQuitPassword  ] = MessageHashedQuitPassword;
            valueString[GroupGeneral, ValueExitKey1            ] = MessageExitKey1;
            valueString[GroupGeneral, ValueExitKey2            ] = MessageExitKey2;
            valueString[GroupGeneral, ValueExitKey3            ] = MessageExitKey3;
            valueString[GroupGeneral, ValueSebMode             ] = MessageSebMode;

            valueString[GroupConfigFile, ValueSebConfigPurpose       ] = MessageSebConfigPurpose;
            valueString[GroupConfigFile, ValueAllowPreferencesWindow ] = MessageAllowPreferencesWindow;
            valueString[GroupConfigFile, ValueCryptoIdentity         ] = MessageCryptoIdentity;
            valueString[GroupConfigFile, ValueSettingsPassword       ] = MessageSettingsPassword;
            valueString[GroupConfigFile, ValueConfirmSettingsPassword] = MessageConfirmSettingsPassword;
            valueString[GroupConfigFile, ValueHashedSettingsPassword ] = MessageHashedSettingsPassword;

            valueString[GroupAppearance, ValueBrowserViewMode             ] = MessageBrowserViewMode;
            valueString[GroupAppearance, ValueMainBrowserWindowWidth      ] = MessageMainBrowserWindowWidth;
            valueString[GroupAppearance, ValueMainBrowserWindowHeight     ] = MessageMainBrowserWindowHeight;
            valueString[GroupAppearance, ValueMainBrowserWindowPositioning] = MessageMainBrowserWindowPositioning;
            valueString[GroupAppearance, ValueEnableBrowserWindowToolbar  ] = MessageEnableBrowserWindowToolbar;
            valueString[GroupAppearance, ValueHideBrowserWindowToolbar    ] = MessageHideBrowserWindowToolbar;
            valueString[GroupAppearance, ValueShowMenuBar                 ] = MessageShowMenuBar;
            valueString[GroupAppearance, ValueShowTaskBar                 ] = MessageShowTaskBar;

            valueString[GroupBrowser, ValueNewBrowserWindowByLinkPolicy        ] = MessageNewBrowserWindowByLinkPolicy;
            valueString[GroupBrowser, ValueNewBrowserWindowByScriptPolicy      ] = MessageNewBrowserWindowByScriptPolicy;
            valueString[GroupBrowser, ValueNewBrowserWindowByLinkBlockForeign  ] = MessageNewBrowserWindowByLinkBlockForeign;
            valueString[GroupBrowser, ValueNewBrowserWindowByScriptBlockForeign] = MessageNewBrowserWindowByScriptBlockForeign;
            valueString[GroupBrowser, ValueNewBrowserWindowByLinkWidth         ] = MessageNewBrowserWindowByLinkWidth;
            valueString[GroupBrowser, ValueNewBrowserWindowByLinkHeight        ] = MessageNewBrowserWindowByLinkHeight;
            valueString[GroupBrowser, ValueNewBrowserWindowByLinkPositioning   ] = MessageNewBrowserWindowByLinkPositioning;

            valueString[GroupBrowser, ValueEnablePlugIns           ] = MessageEnablePlugIns;
            valueString[GroupBrowser, ValueEnableJava              ] = MessageEnableJava;
            valueString[GroupBrowser, ValueEnableJavaScript        ] = MessageEnableJavaScript;
            valueString[GroupBrowser, ValueBlockPopUpWindows       ] = MessageBlockPopUpWindows;
            valueString[GroupBrowser, ValueAllowBrowsingBackForward] = MessageAllowBrowsingBackForward;
            valueString[GroupBrowser, ValueEnableSebBrowser        ] = MessageEnableSebBrowser;

            valueString[GroupDownUploads, ValueAllowDownUploads        ] = MessageAllowDownUploads;
            valueString[GroupDownUploads, ValueDownloadDirectoryOSX    ] = MessageDownloadDirectoryOSX;
            valueString[GroupDownUploads, ValueDownloadDirectoryWin    ] = MessageDownloadDirectoryWin;
            valueString[GroupDownUploads, ValueOpenDownloads           ] = MessageOpenDownloads;
            valueString[GroupDownUploads, ValueChooseFileToUploadPolicy] = MessageChooseFileToUploadPolicy;
            valueString[GroupDownUploads, ValueDownloadPDFFiles        ] = MessageDownloadPDFFiles;

            valueString[GroupExam, ValueExamKeySalt       ] = MessageExamKeySalt;
            valueString[GroupExam, ValueBrowserExamKey    ] = MessageBrowserExamKey;
            valueString[GroupExam, ValueCopyBrowserExamKey] = MessageCopyBrowserExamKey;
            valueString[GroupExam, ValueSendBrowserExamKey] = MessageSendBrowserExamKey;
            valueString[GroupExam, ValueQuitURL           ] = MessageQuitURL;

            valueString[GroupApplications, ValueMonitorProcesses         ] = MessageMonitorProcesses;
            valueString[GroupApplications, ValuePermittedProcesses       ] = MessagePermittedProcesses;
            valueString[GroupApplications, ValueAllowSwitchToApplications] = MessageAllowSwitchToApplications;
            valueString[GroupApplications, ValueAllowFlashFullscreen     ] = MessageAllowFlashFullscreen;
            valueString[GroupApplications, ValueProhibitedProcesses      ] = MessageProhibitedProcesses;

            valueString[GroupNetwork, ValueEnableURLFilter       ] = MessageEnableURLFilter;
            valueString[GroupNetwork, ValueEnableURLContentFilter] = MessageEnableURLContentFilter;
            valueString[GroupNetwork, ValueURLFilterRules        ] = MessageURLFilterRules;
            valueString[GroupNetwork, ValueProxySettingsPolicy   ] = MessageProxySettingsPolicy;
            valueString[GroupNetwork, ValueProxies               ] = MessageProxies;

            valueString[GroupSecurity, ValueSebServicePolicy   ] = MessageSebServicePolicy;
            valueString[GroupSecurity, ValueAllowVirtualMachine] = MessageAllowVirtualMachine;
            valueString[GroupSecurity, ValueCreateNewDesktop   ] = MessageCreateNewDesktop;
            valueString[GroupSecurity, ValueAllowUserSwitching ] = MessageAllowUserSwitching;
            valueString[GroupSecurity, ValueEnableLogging      ] = MessageEnableLogging;
            valueString[GroupSecurity, ValueLogDirectoryOSX    ] = MessageLogDirectoryOSX;
            valueString[GroupSecurity, ValueLogDirectoryWin    ] = MessageLogDirectoryWin;

            valueString[GroupInsideSeb, ValueInsideSebEnableSwitchUser       ] = MessageInsideSebEnableSwitchUser;
            valueString[GroupInsideSeb, ValueInsideSebEnableLockThisComputer ] = MessageInsideSebEnableLockThisComputer;
            valueString[GroupInsideSeb, ValueInsideSebEnableChangeAPassword  ] = MessageInsideSebEnableChangeAPassword;
            valueString[GroupInsideSeb, ValueInsideSebEnableStartTaskManager ] = MessageInsideSebEnableStartTaskManager;
            valueString[GroupInsideSeb, ValueInsideSebEnableLogOff           ] = MessageInsideSebEnableLogOff;
            valueString[GroupInsideSeb, ValueInsideSebEnableShutDown         ] = MessageInsideSebEnableShutDown;
            valueString[GroupInsideSeb, ValueInsideSebEnableEaseOfAccess     ] = MessageInsideSebEnableEaseOfAccess;
            valueString[GroupInsideSeb, ValueInsideSebEnableVmWareClientShade] = MessageInsideSebEnableVmWareClientShade;

            valueString[GroupOutsideSeb, ValueOutsideSebEnableSwitchUser       ] = MessageOutsideSebEnableSwitchUser;
            valueString[GroupOutsideSeb, ValueOutsideSebEnableLockThisComputer ] = MessageOutsideSebEnableLockThisComputer;
            valueString[GroupOutsideSeb, ValueOutsideSebEnableChangeAPassword  ] = MessageOutsideSebEnableChangeAPassword;
            valueString[GroupOutsideSeb, ValueOutsideSebEnableStartTaskManager ] = MessageOutsideSebEnableStartTaskManager;
            valueString[GroupOutsideSeb, ValueOutsideSebEnableLogOff           ] = MessageOutsideSebEnableLogOff;
            valueString[GroupOutsideSeb, ValueOutsideSebEnableShutDown         ] = MessageOutsideSebEnableShutDown;
            valueString[GroupOutsideSeb, ValueOutsideSebEnableEaseOfAccess     ] = MessageOutsideSebEnableEaseOfAccess;
            valueString[GroupOutsideSeb, ValueOutsideSebEnableVmWareClientShade] = MessageOutsideSebEnableVmWareClientShade;

            valueString[GroupHookedKeys, ValueHookKeys] = MessageHookKeys;

            valueString[GroupSpecialKeys, ValueEnableEsc       ] = MessageEnableEsc;
            valueString[GroupSpecialKeys, ValueEnableCtrlEsc   ] = MessageEnableCtrlEsc;
            valueString[GroupSpecialKeys, ValueEnableAltEsc    ] = MessageEnableAltEsc;
            valueString[GroupSpecialKeys, ValueEnableAltTab    ] = MessageEnableAltTab;
            valueString[GroupSpecialKeys, ValueEnableAltF4     ] = MessageEnableAltF4;
            valueString[GroupSpecialKeys, ValueEnableStartMenu ] = MessageEnableStartMenu;
            valueString[GroupSpecialKeys, ValueEnableRightMouse] = MessageEnableRightMouse;

            valueString[GroupFunctionKeys, ValueEnableF1 ] = MessageEnableF1;
            valueString[GroupFunctionKeys, ValueEnableF2 ] = MessageEnableF2;
            valueString[GroupFunctionKeys, ValueEnableF3 ] = MessageEnableF3;
            valueString[GroupFunctionKeys, ValueEnableF4 ] = MessageEnableF4;
            valueString[GroupFunctionKeys, ValueEnableF5 ] = MessageEnableF5;
            valueString[GroupFunctionKeys, ValueEnableF6 ] = MessageEnableF6;
            valueString[GroupFunctionKeys, ValueEnableF7 ] = MessageEnableF7;
            valueString[GroupFunctionKeys, ValueEnableF8 ] = MessageEnableF8;
            valueString[GroupFunctionKeys, ValueEnableF9 ] = MessageEnableF9;
            valueString[GroupFunctionKeys, ValueEnableF10] = MessageEnableF10;
            valueString[GroupFunctionKeys, ValueEnableF11] = MessageEnableF11;
            valueString[GroupFunctionKeys, ValueEnableF12] = MessageEnableF12;


            // Define the strings for the Function Keys F1, F2, ..., F12
            for (int i = 1; i <= 12; i++)
            {
                StringFunctionKey[i - 1] = "F" + i.ToString();
            }

            // Define the strings for the Encryption Identity
            StringCryptoIdentity.Add("none");
            StringCryptoIdentity.Add("alpha");
            StringCryptoIdentity.Add("beta");
            StringCryptoIdentity.Add("gamma");
            StringCryptoIdentity.Add("delta");
            StringCryptoIdentityArray = StringCryptoIdentity.ToArray();

            // Define the strings for the SEB purpose
            StringSebPurpose[0] = "starting an exam";
            StringSebPurpose[1] = "configuring a client";

            // Define the strings for the SEB mode
            StringSebMode[0] = "use local settings and load the start URL";
            StringSebMode[1] = "connect to the SEB server";

            // Define the strings for the Browser View Mode
            StringBrowserViewMode[0] = "use browser window";
            StringBrowserViewMode[1] = "use full screen mode";

            // Define the strings for the Window Width
            StringWindowWidth[0] = "50%";
            StringWindowWidth[1] = "100%";
            StringWindowWidth[2] = "800";
            StringWindowWidth[3] = "1000";

            // Define the strings for the Window Height
            StringWindowHeight[0] = "80%";
            StringWindowHeight[1] = "100%";
            StringWindowHeight[2] = "600";
            StringWindowHeight[3] = "800";

            // Define the strings for the Window Positioning
            StringWindowPositioning[0] = "Left";
            StringWindowPositioning[1] = "Center";
            StringWindowPositioning[2] = "Right";

            // Define the strings for the Link Opening Policy
            StringPolicyLinkOpening[0] = "get generally blocked";
            StringPolicyLinkOpening[1] = "open in same window";
            StringPolicyLinkOpening[2] = "open in new window";

            // Define the strings for the File Upload Policy
            StringPolicyFileUpload[0] = "manually with file requester";
            StringPolicyFileUpload[1] = "by attempting to upload the same file downloaded before";
            StringPolicyFileUpload[2] = "by only allowing to upload the same file downloaded before";

            // Define the strings for the Proxy Settings Policy
            StringPolicyProxySettings[0] = "Use system proxy settings";
            StringPolicyProxySettings[1] = "Use SEB proxy settings";

            // Define the strings for the SEB Service Policy
            StringPolicySebService[0] = "allow to run SEB without service";
            StringPolicySebService[1] = "display warning when service is not running";
            StringPolicySebService[2] = "allow to use SEB only with service";

            // Assign the fixed entries to the listBoxes and comboBoxes
            listBoxExitKey1.Items.AddRange(StringFunctionKey);
            listBoxExitKey2.Items.AddRange(StringFunctionKey);
            listBoxExitKey3.Items.AddRange(StringFunctionKey);

            comboBoxCryptoIdentity.Items.AddRange(StringCryptoIdentity.ToArray());

            comboBoxMainBrowserWindowWidth      .Items.AddRange(StringWindowWidth);
            comboBoxMainBrowserWindowHeight     .Items.AddRange(StringWindowHeight);
             listBoxMainBrowserWindowPositioning.Items.AddRange(StringWindowPositioning);

            comboBoxNewBrowserWindowWidth       .Items.AddRange(StringWindowWidth);
            comboBoxNewBrowserWindowHeight      .Items.AddRange(StringWindowHeight);
             listBoxNewBrowserWindowPositioning .Items.AddRange(StringWindowPositioning);

             listBoxOpenLinksHTML.Items.AddRange(StringPolicyLinkOpening);
             listBoxOpenLinksJava.Items.AddRange(StringPolicyLinkOpening);

             listBoxChooseFileToUploadPolicy.Items.AddRange(StringPolicyFileUpload);
             listBoxSebServicePolicy        .Items.AddRange(StringPolicySebService);


            // Define the strings for the Permitted Processes columns
            StringColumnsProcessesPermitted[0] = "Active";
            StringColumnsProcessesPermitted[1] = "OS";
            StringColumnsProcessesPermitted[2] = "Executable";
            StringColumnsProcessesPermitted[3] = "AppTitle";

            // Assign the fixed entries to the listViews
            listViewPermittedProcesses.View = View.Details;
            listViewPermittedProcesses.Columns.Add("Active");
            listViewPermittedProcesses.Columns.Add("OS");
            listViewPermittedProcesses.Columns.Add("Executable");
            listViewPermittedProcesses.Columns.Add("AppTitle");

            //StringActive[0] = "alpha";
            //StringActive[1] = "beta";

            StringOperatingSystem[0] = "OS X";
            StringOperatingSystem[1] = "Win";


            // IMPORTANT:
            // Create a second dictionary "new settings"
            // and copy all default settings to the new settings.
            // This must be done BEFORE any config file is loaded
            // and assures that every (key, value) pair is contained
            // in the "old", "new" and "def" dictionaries,
            // even if the loaded "tmp" dictionary does NOT contain every pair.

            CopySettingsArrays    (      StateDef,       StateNew);
            CopySettingsDictionary(sebSettingsDef, sebSettingsNew);

            PrintSettingsDictionary(sebSettingsDef, "SettingsDef.txt");
            PrintSettingsDictionary(sebSettingsNew, "SettingsNew.txt");

          //SetWidgetsToNewSettings();

            // Try to open the configuration file ("SebClient.ini/xml/seb")
            // given in the local directory (where SebWindowsConfig.exe was called)
            currentDireSebConfigFile = Directory.GetCurrentDirectory();
            currentFileSebConfigFile = "";
            currentPathSebConfigFile = "";

             defaultDireSebConfigFile = Directory.GetCurrentDirectory();
             defaultFileSebConfigFile =                  DefaultSebConfigXml;
             defaultPathSebConfigFile = Path.GetFullPath(DefaultSebConfigXml);
/*
            // Cut off the file extension ".ini", ".xml" or ".seb",
            // that is the last 4 characters of the file name
            String fileName    = defaultPathSebConfigFile;
            String fileNameRaw = fileName.Substring(0, fileName.Length - 4);
            String fileNameExt = fileName.Substring(fileName.Length - 4, 4);
            String fileNameIni = fileNameRaw + ".ini";
            String fileNameXml = fileNameRaw + ".xml";
            String fileNameSeb = fileNameRaw + ".seb";

            // Read the settings from the standard configuration file
            if (fileNameExt.Equals(".ini")) OpenIniFile(fileNameIni);
            if (fileNameExt.Equals(".xml")) OpenXmlFile(fileNameXml);
            if (fileNameExt.Equals(".seb")) OpenSebFile(fileNameSeb);
*/
            openFileDialogSebConfigFile.InitialDirectory = Environment.CurrentDirectory;
            saveFileDialogSebConfigFile.InitialDirectory = Environment.CurrentDirectory;
            //folderBrowserDialogDownloadDirectoryWin.RootFolder = Environment.SpecialFolder.DesktopDirectory;
            //folderBrowserDialogLogDirectoryWin     .RootFolder = Environment.SpecialFolder.MyDocuments;

        } // end of contructor   SebWindowsConfigForm()




        // **************************************************
        // Convert some settings after reading them from file
        // **************************************************
        private void ConvertSomeSettingsAfterReadingThemFromFile(String fileName)
        {
            // Choose Identity needs a conversion from string to integer.
            // The SEB Windows configuration editor never reads the identity
            // from the config file but instead searches it in the
            // Certificate Store of the computer where it is running,
            // so initially the 0th list entry is displayed ("none").
            //
            //tmpCryptoIdentityInteger = 0;
            //tmpCryptoIdentityString  = 0;

            // These ComboBox entries need a conversion from string to integer:
            //
            // Main Window Width/Height
            // New  Window Width/Height

/*
            String tmpStringMainWindowWidth       = (String)sebSettingsNew[MessageMainBrowserWindowWidth];
            String tmpStringMainWindowHeight      = (String)sebSettingsNew[MessageMainBrowserWindowHeight];
            String tmpStringNewWindowByLinkWidth  = (String)sebSettingsNew[MessageNewBrowserWindowByLinkWidth];
            String tmpStringNewWindowByLinkHeight = (String)sebSettingsNew[MessageNewBrowserWindowByLinkHeight];

            int index;
            int tmpIndexMainWindowWidth       = 0;
            int tmpIndexMainWindowHeight      = 0;
            int tmpIndexNewWindowByLinkWidth  = 0;
            int tmpIndexNewWindowByLinkHeight = 0;

            // Window width and height have 4 possible list entries
            for (index = 0; index <= 3; index++)
            {
                String width  = StringWindowWidth [index];
                String height = StringWindowHeight[index];

                if (tmpStringMainWindowWidth      .Equals(width )) tmpIndexMainWindowWidth       = index;
                if (tmpStringMainWindowHeight     .Equals(height)) tmpIndexMainWindowHeight      = index;
                if (tmpStringNewWindowByLinkWidth .Equals(width )) tmpIndexNewWindowByLinkWidth  = index;
                if (tmpStringNewWindowByLinkHeight.Equals(height)) tmpIndexNewWindowByLinkHeight = index;
            }

            // Store the determined integers
            sebSettingsNew[MessageMainBrowserWindowWidth      ] = tmpIndexMainWindowWidth;
            sebSettingsNew[MessageMainBrowserWindowHeight     ] = tmpIndexMainWindowHeight;
            sebSettingsNew[MessageNewBrowserWindowByLinkWidth ] = tmpIndexNewWindowByLinkWidth;
            sebSettingsNew[MessageNewBrowserWindowByLinkHeight] = tmpIndexNewWindowByLinkHeight;
*/

            // Copy tmp settings to old settings
            // Copy tmp settings to new settings
            CopySettingsArrays    (      StateTmp,       StateOld);
            CopySettingsDictionary(sebSettingsTmp, sebSettingsOld);
            CopySettingsArrays    (      StateTmp,       StateNew);
            CopySettingsDictionary(sebSettingsTmp, sebSettingsNew);

            currentDireSebConfigFile = Path.GetDirectoryName(fileName);
            currentFileSebConfigFile = Path.GetFileName     (fileName);
            currentPathSebConfigFile = Path.GetFullPath     (fileName);

            return;
        }



        // *************************************************
        // Convert some settings before writing them to file
        // *************************************************
        private void ConvertSomeSettingsBeforeWritingThemToFile()
        {
            // These ListBox and ComboBox entries need a conversion from integer to string:
            //
            // Main Window Width/Height
            // New  Window Width/Height
/*
            int newIndexMainWindowWidth  = (int)sebSettingsNew[MessageMainBrowserWindowWidth];
            int newIndexMainWindowHeight = (int)sebSettingsNew[MessageMainBrowserWindowHeight];
            int newIndexNewWindowWidth   = (int)sebSettingsNew[MessageNewBrowserWindowByLinkWidth];
            int newIndexNewWindowHeight  = (int)sebSettingsNew[MessageNewBrowserWindowByLinkHeight];

            // Store the determined strings
            sebSettingsNew[MessageMainBrowserWindowWidth      ] = StringWindowWidth [newIndexMainWindowWidth];
            sebSettingsNew[MessageMainBrowserWindowHeight     ] = StringWindowHeight[newIndexMainWindowHeight];
            sebSettingsNew[MessageNewBrowserWindowByLinkWidth ] = StringWindowWidth [newIndexNewWindowWidth];
            sebSettingsNew[MessageNewBrowserWindowByLinkHeight] = StringWindowHeight[newIndexNewWindowHeight];
*/
            return;
        }



        // ************************************************
        // Convert some settings after writing them to file
        // ************************************************
        private void ConvertSomeSettingsAfterWritingThemToFile(String fileName)
        {
            // Copy new settings to old settings
            CopySettingsArrays    (      StateNew,       StateOld);
            CopySettingsDictionary(sebSettingsNew, sebSettingsOld);

            currentDireSebConfigFile = Path.GetDirectoryName(fileName);
            currentFileSebConfigFile = Path.GetFileName     (fileName);
            currentPathSebConfigFile = Path.GetFullPath     (fileName);

            return;
        }



        // ****************************************
        // Open the .ini file and read the settings
        // ****************************************
        private Boolean OpenIniFile(String fileName)
        {
            FileStream   fileStream;
            StreamReader fileReader;
            String       fileLine;
            Boolean      fileCouldBeRead = true;

            int group;
            int value;
            int minvalue;
            int maxvalue;

            int     equalPosition = 0;
            String   leftString   = "";
            String  rightString   = "";
            Boolean rightBoolean  = false;
            Boolean foundSetting  = false;

            try 
            {
                // Open the .ini file for reading
                fileStream = new   FileStream(fileName, FileMode.Open, FileAccess.Read);
                fileReader = new StreamReader(fileStream);

                // Read lines from the .ini file until end of file is reached
                while ((fileLine = fileReader.ReadLine()) != null) 
                {
                    // Skip empty lines and lines not in "leftSide = rightSide" format
                    if (fileLine.Contains("="))
                    {
                        equalPosition =    fileLine.IndexOf  ("=");
                         leftString   =    fileLine.Remove   (equalPosition);
                        rightString   =    fileLine.Substring(equalPosition + 1);
                        rightBoolean  = rightString.Equals("true");
                        foundSetting  = false;

                        // Find the appropriate group and setting
                        for (group = 1; group <= GroupNum; group++)
                        {
                            minvalue = minValue[group];
                            maxvalue = maxValue[group];

                            for (value = minvalue; value <= maxvalue; value++)
                            {
                                if (leftString.Equals(valueString[group, value]))
                                {
                                    settingBoolean[StateTmp, group, value] = rightBoolean;
                                    settingString [StateTmp, group, value] = rightString;
                                    foundSetting = true;
                                    break;
                                }
                            } // next value
                        } // next group

                        if (foundSetting == false) fileCouldBeRead = false;
                        if (foundSetting == false) break;

                    } // end if line.Contains("=")
                } // end while

                // Close the .ini file
                fileReader.Close();
                fileStream.Close();
            }
            catch (Exception streamReadException) 
            {
                // Let the user know what went wrong
                Console.WriteLine("The .ini file could not be read:");
                Console.WriteLine(streamReadException.Message);
                return false;
            }

            if (fileCouldBeRead == false)
            {
                // Let the user know what went wrong
                MessageBox.Show("The file \"" + fileName + "\" does not match the syntax of a "
                                + fileString[FileSebStarterIni] + " config file."
                                + " Debug data: "
                                + " fileLine   = "     +  fileLine
                                + " leftString = "     +  leftString
                                +" rightString = "     + rightString,
                                 "Error when reading " +  fileString[FileSebStarterIni],
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            // After reading the settings from file,
            // copy them to "new" and "old" settings and update the widgets
            ConvertSomeSettingsAfterReadingThemFromFile(fileName);
            SetWidgetsToNewSettings();
            return true;
        }



        // ****************************************
        // Open the .xml file and read the settings
        // ****************************************
        private Boolean OpenXmlFile(String fileName)
        {
            try 
            {
                // Read the configuration settings from .xml file
                // Convert the XML structure into a C# object
                sebSettingsTmp = (Dictionary<string, object>)Plist.readPlist(fileName);
/*
                XmlSerializer deserializer = new XmlSerializer(typeof(SEBClientConfig));
                TextReader      textReader = new StreamReader (fileName);
                sebSettObso = (SEBClientConfig) deserializer.Deserialize(textReader);
                textReader.Close();
*/
            }
            catch (Exception streamReadException)
            {
                // Let the user know what went wrong
                Console.WriteLine("The .xml file could not be read:");
                Console.WriteLine(streamReadException.Message);
                return false;
            }

            // After reading the settings from file,
            // copy them to "new" and "old" settings and update the widgets
            ConvertSomeSettingsAfterReadingThemFromFile(fileName);
            SetWidgetsToNewSettings();
            PrintSettingsDictionary(sebSettingsTmp, "SettingsTmp.txt");
            PrintSettingsDictionary(sebSettingsNew, "SettingsNew.txt");
            return true;
        }



        // ****************************************
        // Open the .seb file and read the settings
        // ****************************************
        private Boolean OpenSebFile(String fileName)
        {
            try 
            {
                // Read the configuration settings from .seb file
                // Decrypt the configuration settings
                // Convert the XML structure into a C# object

                TextReader textReader;
                String encryptedSettings = "";
                String decryptedSettings = "";
              //String password          = "Seb";
              //X509Certificate2 certificate = null;

                textReader        = new StreamReader(fileName);
                encryptedSettings = textReader.ReadToEnd();
                textReader.Close();

              //decryptedSettings = sebController.DecryptSebClientSettings(encryptedSettings);
              //decryptedSettings = decryptedSettings.Trim();
                decryptedSettings = encryptedSettings;

                sebSettingsTmp = (Dictionary<string, object>)Plist.readPlistSource(decryptedSettings);

              //MemoryStream memStreamObso  = new MemoryStream(Encoding.UTF8.GetBytes(decryptedSettings));
              //MemoryStream memStreamPlist = new MemoryStream(Encoding.UTF8.GetBytes(decryptedSettings));

              //sebSettObso    = (SEBClientConfig)            sebSerializerObso .Deserialize(memStreamObso);
              //sebSettingsTmp = (Dictionary<string, object>) sebSerializerPlist.Deserialize(memStreamPlist);
              //sebSettingsTmp = (Dictionary<string, object>) Plist.parseBinaryDate(0);

              //sebSettingsTmp = (Dictionary<string, object>)Plist.readPlist(memStreamPlist, plistType.Auto);
              //sebSettingsTmp = (Dictionary<string, object>)Plist.readPlist(memStreamPlist, plistType.Xml);
              //sebSettingsTmp = (Dictionary<string, object>)Plist.readPlist(memStreamPlist, plistType.Binary);

              //memStreamObso .Close();
              //memStreamPlist.Close();
            }
            catch (Exception streamReadException)
            {
                // Let the user know what went wrong
                Console.WriteLine("The .seb file could not be read:");
                Console.WriteLine(streamReadException.Message);
                return false;
            }

            // After reading the settings from file,
            // copy them to "new" and "old" settings and update the widgets
            ConvertSomeSettingsAfterReadingThemFromFile(fileName);
            SetWidgetsToNewSettings();
            PrintSettingsDictionary(sebSettingsTmp, "SettingsTmp.txt");
            PrintSettingsDictionary(sebSettingsNew, "SettingsNew.txt");
            return true;
        }



        // ***********************************************
        // Write the settings to the .ini file and save it
        // ***********************************************
        private Boolean SaveIniFile(String fileName)
        {
            FileStream   fileStream;
            StreamWriter fileWriter;
            String       fileLine;

            int group;
            int value;
            int minvalue;
            int maxvalue;

            // Before writing the settings to file,
            // convert some settings
            ConvertSomeSettingsBeforeWritingThemToFile();

            try 
            {
                // If the .ini file already exists, delete it
                // and write it again from scratch with new data
                if (File.Exists(fileName))
                    File.Delete(fileName);

                // Open the .ini file for writing
                fileStream = new   FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write);
                fileWriter = new StreamWriter(fileStream);

                // Write the header lines
                fileWriter.WriteLine("");
                fileWriter.WriteLine("[SEB]");
                fileWriter.WriteLine("");

                // For each group and each key,
                // write the line "key=value" into the .ini file
                for (group = 1; group <= GroupNum; group++)
                {
                    minvalue = minValue[group];
                    maxvalue = maxValue[group];

                    // Write the group name
                    fileWriter.WriteLine("[" + groupString[group] + "]");
                    fileWriter.WriteLine("");

                    for (value = minvalue; value <= maxvalue; value++)
                    {
                        String   leftString    =   valueString [          group, value];
                        String  rightString    = settingString [StateNew, group, value];
                        Boolean rightBoolean   = settingBoolean[StateNew, group, value];
                        int     rightType      =    dataType   [          group, value];

                        if ((rightType == TypeBoolean) && (rightBoolean == false)) rightString = "false";
                        if ((rightType == TypeBoolean) && (rightBoolean ==  true)) rightString = "true";

                        // Concatenate the "...=..." line and write it
                        fileLine = leftString + "=" + rightString;
                        fileWriter.WriteLine(fileLine);

                    } // next value

                    // Write an empty line into the file
                    fileWriter.WriteLine("");

                } // next group

                // Close the .ini file
                fileWriter.Close();
                fileStream.Close();
            }
            catch (Exception streamWriteException) 
            {
                // Let the user know what went wrong
                Console.WriteLine("The .ini file could not be written:");
                Console.WriteLine(streamWriteException.Message);
                return false;
            }

            // After writing the settings to file, update the widgets
            ConvertSomeSettingsAfterWritingThemToFile(fileName);
            SetWidgetsToNewSettings();
            return true;
        }



        // ***********************************************
        // Write the settings to the .xml file and save it
        // ***********************************************
        private Boolean SaveXmlFile(String fileName)
        {
            // Before writing the settings to file,
            // convert some settings
            ConvertSomeSettingsBeforeWritingThemToFile();

            try 
            {
                // If the .xml file already exists, delete it
                // and write it again from scratch with new data
                if (File.Exists(fileName))
                    File.Delete(fileName);

                // Convert the C# object into an XML structure
                // Write the configuration settings into .xml file
                Plist.writeXml(sebSettingsNew, fileName);
/*
                XmlSerializer serializer = new XmlSerializer(typeof(SEBClientConfig));
                TextWriter    textWriter = new StreamWriter(fileName);
                serializer.Serialize(textWriter, sebSettObso);
                textWriter.Close();
*/
            }
            catch (Exception streamWriteException) 
            {
                // Let the user know what went wrong
                Console.WriteLine("The .xml file could not be written:");
                Console.WriteLine(streamWriteException.Message);
                return false;
            }

            // After writing the settings to file, update the widgets
            ConvertSomeSettingsAfterWritingThemToFile(fileName);
            SetWidgetsToNewSettings();
            return true;
        }



        // ***********************************************
        // Write the settings to the .seb file and save it
        // ***********************************************
        private Boolean SaveSebFile(String fileName)
        {
            // Before writing the settings to file,
            // convert some settings
            ConvertSomeSettingsBeforeWritingThemToFile();

            try 
            {
                // If the .xml file already exists, delete it
                // and write it again from scratch with new data
                if (File.Exists(fileName))
                    File.Delete(fileName);

                // Convert the C# object into an XML structure
                // Encrypt the configuration settings
                // Write the configuration settings into .seb file

                TextWriter textWriter;
                String encryptedSettings = "";
                String decryptedSettings = "";
                String password          = "Seb";
                X509Certificate2 certificate = null;

                decryptedSettings = Plist.writeXml(sebSettingsNew);

              //encryptedSettings = sebController.EncryptWithPassword  (decryptedSettings, password);
              //encryptedSettings = sebController.EncryptWithCertifikat(decryptedSettings, certificate);
                encryptedSettings = decryptedSettings;

                textWriter = new StreamWriter(fileName);
                textWriter.Write(encryptedSettings);
                textWriter.Close();

                //decryptedSettings = Plist.writePlist(sebSettingsNew);
                //decryptedSettings = sebSettingsNew.ToString();

                //Stream decryptedStream = null;
                //Plist.writeBinary(sebSettingsNew, decryptedStream);
                //Plist.writeXml   (sebSettingsNew, decryptedStream);

                //MemoryStream memStreamObso  = new MemoryStream(Encoding.UTF8.GetBytes(decryptedSettings));
                //MemoryStream memStreamPlist = new MemoryStream(Encoding.UTF8.GetBytes(decryptedSettings));

                //sebSerializerObso.Serialize(memStreamObso, sebSettObso);

                //memStreamObso .Close();
                //memStreamPlist.Close();
            }
            catch (Exception streamWriteException) 
            {
                // Let the user know what went wrong
                Console.WriteLine("The .seb file could not be written:");
                Console.WriteLine(streamWriteException.Message);
                return false;
            }

            // After writing the settings to file, update the widgets
            ConvertSomeSettingsAfterWritingThemToFile(fileName);
            SetWidgetsToNewSettings();
            return true;
        }


/*
        // *********************************************************************
        // Convert C# object (having been read from .xml or .seb file) to arrays
        // *********************************************************************
        private Boolean ConvertCSharpObjectToArrays()
        {
            // Copy the C# object "sebSettings" to the arrays "settingString"/"settingBoolean"

            settingString [StateTmp, GroupGeneral, ValueStartURL           ] = sebSettObso.getUrlAddress    ("startURL"    ).Url;
          //settingString [StateTmp, GroupGeneral, ValueSEBServerURL       ] = sebSettings.getUrlAddress    ("sebServerURL").Url;
            settingString [StateTmp, GroupGeneral, ValueHashedAdminPassword] = sebSettObso.getPassword      ("hashedAdminPassword").Value;
            settingString [StateTmp, GroupGeneral, ValueHashedQuitPassword ] = sebSettObso.getPassword      ("hashedQuitPassword" ).Value;
            settingBoolean[StateTmp, GroupGeneral, ValueAllowQuit          ] = sebSettObso.getSecurityOption("allowQuit"         ).getBool();
            settingBoolean[StateTmp, GroupGeneral, ValueIgnoreQuitPassword ] = sebSettObso.getSecurityOption("ignoreQuitPassword").getBool();
            settingString [StateTmp, GroupGeneral, ValueExitKey1           ] = sebSettObso.getExitKey       ("exitKey1").Value;
            settingString [StateTmp, GroupGeneral, ValueExitKey2           ] = sebSettObso.getExitKey       ("exitKey2").Value;
            settingString [StateTmp, GroupGeneral, ValueExitKey3           ] = sebSettObso.getExitKey       ("exitKey3").Value;

          //settingString [StateTmp, GroupConfigFile, ValueSebPurpose            ] = sebSettings.getPolicySetting ("sebPurpose"            ).Value;
          //settingBoolean[StateTmp, GroupConfigFile, ValueStartingAnExam        ] = sebSettings.getSecurityOption("startingAnExam"        ).getBool();
          //settingBoolean[StateTmp, GroupConfigFile, ValueConfiguringAClient    ] = sebSettings.getSecurityOption("configuringAClient"    ).getBool();
            settingBoolean[StateTmp, GroupConfigFile, ValueAllowPreferencesWindow] = sebSettObso.getSecurityOption("allowPreferencesWindow").getBool();
          //settingString [StateTmp, GroupConfigFile, ValueChooseIdentity        ] = sebSettings.getPassword      ("chooseIdentity"        ).Value;
          //settingString [StateTmp, GroupConfigFile, ValueHashedSettingsPassword] = sebSettings.getPassword      ("hashedSettingsPassword").Value;

          //settingString [StateTmp, GroupAppearance, ValueBrowserViewMode      ] = sebSettings.getPolicySetting ("browserViewMode").Value;
          //settingBoolean[StateTmp, GroupAppearance, ValueUseBrowserWindow     ] = sebSettings.getSecurityOption("useBrowserWindow" ).getBool();
          //settingBoolean[StateTmp, GroupAppearance, ValueUseFullScreenMode    ] = sebSettings.getSecurityOption("useFullScreenMode").getBool();
            settingString [StateTmp, GroupAppearance, ValueMainBrowserWindowWidth ] = sebSettObso.getPolicySetting ("mainBrowserWindowWidth"   ).Value;
            settingString [StateTmp, GroupAppearance, ValueMainBrowserWindowHeight] = sebSettObso.getPolicySetting ("mainBrowserWindowHeight"  ).Value;
          //settingString [StateTmp, GroupAppearance, ValueMainWindowPositioning  ] = sebSettings.getPolicySetting ("mainBrowserWindowPosition").Value;
            settingBoolean[StateTmp, GroupAppearance, ValueEnableBrowserWindowToolbar] = sebSettObso.getSecurityOption("enableBrowserWindowToolbar").getBool();
            settingBoolean[StateTmp, GroupAppearance, ValueHideBrowserWindowToolbar  ] = sebSettObso.getSecurityOption(  "hideBrowserWindowToolbar").getBool();
            settingBoolean[StateTmp, GroupAppearance, ValueShowMenuBar] = sebSettObso.getSecurityOption("showMenuBar").getBool();
            settingBoolean[StateTmp, GroupAppearance, ValueShowTaskBar] = sebSettObso.getSecurityOption("showTaskBar").getBool();

          //settingString [StateTmp, GroupBrowser, ValueNewWindowPolicyHTML ] = sebSettings.getPolicySetting ("newBrowserWindowByLinkPolicy").Value;
          //settingString [StateTmp, GroupBrowser, ValueNewWindowPolicyJava ] = sebSettings.getPolicySetting ("newBrowserWindowByLinkPolicy").Value;
            settingBoolean[StateTmp, GroupBrowser, ValueNewBrowserWindowByLinkBlockForeign  ] = sebSettObso.getSecurityOption("newBrowserWindowByLinkBlockForeign"  ).getBool();
            settingBoolean[StateTmp, GroupBrowser, ValueNewBrowserWindowByScriptBlockForeign] = sebSettObso.getSecurityOption("newBrowserWindowByScriptBlockForeign").getBool();
            settingString [StateTmp, GroupBrowser, ValueNewBrowserWindowByLinkWidth ] = sebSettObso.getPolicySetting ("newBrowserWindowByLinkWidth"   ).Value;
            settingString [StateTmp, GroupBrowser, ValueNewBrowserWindowByLinkHeight] = sebSettObso.getPolicySetting ("newBrowserWindowByLinkHeight"  ).Value;
          //settingString [StateTmp, GroupBrowser, ValueNewWindowPosition   ] = sebSettings.getPolicySetting ("newBrowserWindowByLinkPosition").Value;
            settingBoolean[StateTmp, GroupBrowser, ValueEnablePlugIns       ] = sebSettObso.getSecurityOption("enablePlugins"   ).getBool();
            settingBoolean[StateTmp, GroupBrowser, ValueEnableJava          ] = sebSettObso.getSecurityOption("enableJava"      ).getBool();
            settingBoolean[StateTmp, GroupBrowser, ValueEnableJavaScript    ] = sebSettObso.getSecurityOption("enableJavaScript").getBool();
            settingBoolean[StateTmp, GroupBrowser, ValueBlockPopUpWindows   ] = sebSettObso.getSecurityOption("blockPopUpWindows").getBool();
            settingBoolean[StateTmp, GroupBrowser, ValueAllowBrowsingBackForward] = sebSettObso.getSecurityOption("allowBrowsingBackForward").getBool();
            settingBoolean[StateTmp, GroupBrowser, ValueEnableSebBrowser    ] = sebSettObso.getSecurityOption("enableSebBrowser").getBool();

            settingBoolean[StateTmp, GroupDownUploads, ValueAllowDownUploads] = sebSettObso.getSecurityOption   ("allowDownUploads").getBool();
            settingBoolean[StateTmp, GroupDownUploads, ValueOpenDownloads   ] = sebSettObso.getSecurityOption   ("openDownloads"   ).getBool();
            settingBoolean[StateTmp, GroupDownUploads, ValueDownloadPDFFiles] = sebSettObso.getSecurityOption   ("downloadPDFFiles").getBool();
            settingString [StateTmp, GroupDownUploads, ValueDownloadDirectoryWin    ] = sebSettObso.getDownloadDirectory("downloadDirectoryWin"    ).Path;
            settingString [StateTmp, GroupDownUploads, ValueChooseFileToUploadPolicy] = sebSettObso.getPolicySetting    ("chooseFileToUploadPolicy").Value;

          //settingString [StateTmp, GroupExam, ValueBrowserExamKey    ] = sebSettings.getUrlAddress("browserExamKey" ).Url;
            settingBoolean[StateTmp, GroupExam, ValueCopyBrowserExamKey] = sebSettObso.getSecurityOption("copyExamKeyToClipboardWhenQuitting").getBool();
            settingBoolean[StateTmp, GroupExam, ValueSendBrowserExamKey] = sebSettObso.getSecurityOption("sendBrowserExamKey").getBool();
            settingString [StateTmp, GroupExam, ValueQuitURL           ] = sebSettObso.getUrlAddress("quitURL").Url;

            settingBoolean[StateTmp, GroupApplications, ValueMonitorProcesses         ] = sebSettObso.getSecurityOption("monitorProcesses").getBool();
            settingBoolean[StateTmp, GroupApplications, ValueAllowSwitchToApplications] = sebSettObso.getSecurityOption("allowSwitchToApplications").getBool();
            settingBoolean[StateTmp, GroupApplications, ValueAllowFlashFullscreen     ] = sebSettObso.getSecurityOption("allowFlashFullscreen").getBool();

            settingString [StateTmp, GroupSecurity, ValueSebServicePolicy   ] = sebSettObso.getPolicySetting ("sebServicePolicy"   ).Value;
            settingBoolean[StateTmp, GroupSecurity, ValueAllowVirtualMachine] = sebSettObso.getSecurityOption("allowVirtualMachine").getBool();
            settingBoolean[StateTmp, GroupSecurity, ValueCreateNewDesktop   ] = sebSettObso.getSecurityOption("createNewDesktop"   ).getBool();
            settingBoolean[StateTmp, GroupSecurity, ValueAllowUserSwitching ] = sebSettObso.getSecurityOption("allowUserSwitching" ).getBool();
            settingBoolean[StateTmp, GroupSecurity, ValueEnableLogging      ] = sebSettObso.getSecurityOption("enableLog"          ).getBool();

            settingBoolean[StateTmp, GroupInsideSeb, ValueInsideSebEnableSwitchUser       ] = sebSettObso.getRegistryValue("insideSebEnableSwitchUser"       ).getBool();
            settingBoolean[StateTmp, GroupInsideSeb, ValueInsideSebEnableLockThisComputer ] = sebSettObso.getRegistryValue("insideSebEnableLockThisComputer" ).getBool();
            settingBoolean[StateTmp, GroupInsideSeb, ValueInsideSebEnableChangeAPassword  ] = sebSettObso.getRegistryValue("insideSebEnableChangePassword"   ).getBool();
            settingBoolean[StateTmp, GroupInsideSeb, ValueInsideSebEnableStartTaskManager ] = sebSettObso.getRegistryValue("insideSebEnableStartTaskManager" ).getBool();
            settingBoolean[StateTmp, GroupInsideSeb, ValueInsideSebEnableLogOff           ] = sebSettObso.getRegistryValue("insideSebEnableLogOff"           ).getBool();
            settingBoolean[StateTmp, GroupInsideSeb, ValueInsideSebEnableShutDown         ] = sebSettObso.getRegistryValue("insideSebEnableShutDown"         ).getBool();
            settingBoolean[StateTmp, GroupInsideSeb, ValueInsideSebEnableEaseOfAccess     ] = sebSettObso.getRegistryValue("insideSebEnableEaseOfAccess"     ).getBool();
            settingBoolean[StateTmp, GroupInsideSeb, ValueInsideSebEnableVmWareClientShade] = sebSettObso.getRegistryValue("insideSebEnableVmWareClientShade").getBool();

            settingBoolean[StateTmp, GroupOutsideSeb, ValueOutsideSebEnableSwitchUser       ] = sebSettObso.getRegistryValue("outsideSebEnableSwitchUser"       ).getBool();
            settingBoolean[StateTmp, GroupOutsideSeb, ValueOutsideSebEnableLockThisComputer ] = sebSettObso.getRegistryValue("outsideSebEnableLockThisComputer" ).getBool();
            settingBoolean[StateTmp, GroupOutsideSeb, ValueOutsideSebEnableChangeAPassword  ] = sebSettObso.getRegistryValue("outsideSebEnableChangePassword"   ).getBool();
            settingBoolean[StateTmp, GroupOutsideSeb, ValueOutsideSebEnableStartTaskManager ] = sebSettObso.getRegistryValue("outsideSebEnableStartTaskManager" ).getBool();
            settingBoolean[StateTmp, GroupOutsideSeb, ValueOutsideSebEnableLogOff           ] = sebSettObso.getRegistryValue("outsideSebEnableLogOff"           ).getBool();
            settingBoolean[StateTmp, GroupOutsideSeb, ValueOutsideSebEnableShutDown         ] = sebSettObso.getRegistryValue("outsideSebEnableShutDown"         ).getBool();
            settingBoolean[StateTmp, GroupOutsideSeb, ValueOutsideSebEnableEaseOfAccess     ] = sebSettObso.getRegistryValue("outsideSebEnableEaseOfAccess"     ).getBool();
            settingBoolean[StateTmp, GroupOutsideSeb, ValueOutsideSebEnableVmWareClientShade] = sebSettObso.getRegistryValue("outsideSebEnableVmWareClientShade").getBool();

            settingBoolean[StateTmp, GroupHookedKeys, ValueHookMessages] = sebSettObso.getSecurityOption("hookMessages").getBool();

            settingBoolean[StateTmp, GroupSpecialKeys, ValueEnableEsc       ] = sebSettObso.getHookedMessageKey("enableEsc"       ).getBool();
            settingBoolean[StateTmp, GroupSpecialKeys, ValueEnableCtrlEsc   ] = sebSettObso.getHookedMessageKey("enableCtrlEsc"   ).getBool();
            settingBoolean[StateTmp, GroupSpecialKeys, ValueEnableAltEsc    ] = sebSettObso.getHookedMessageKey("enableAltEsc"    ).getBool();
            settingBoolean[StateTmp, GroupSpecialKeys, ValueEnableAltTab    ] = sebSettObso.getHookedMessageKey("enableAltTab"    ).getBool();
            settingBoolean[StateTmp, GroupSpecialKeys, ValueEnableAltF4     ] = sebSettObso.getHookedMessageKey("enableAltF4"     ).getBool();
          //settingBoolean[StateTmp, GroupSpecialKeys, ValueEnableStartMenu ] = sebSettings.getHookedMessageKey("enableStartMenu" ).getBool();
            settingBoolean[StateTmp, GroupSpecialKeys, ValueEnableRightMouse] = sebSettObso.getHookedMessageKey("enableRightMouse").getBool();

            settingBoolean[StateTmp, GroupFunctionKeys, ValueEnableF1 ] = sebSettObso.getHookedMessageKey("enableF1" ).getBool();
            settingBoolean[StateTmp, GroupFunctionKeys, ValueEnableF2 ] = sebSettObso.getHookedMessageKey("enableF2" ).getBool();
            settingBoolean[StateTmp, GroupFunctionKeys, ValueEnableF3 ] = sebSettObso.getHookedMessageKey("enableF3" ).getBool();
            settingBoolean[StateTmp, GroupFunctionKeys, ValueEnableF4 ] = sebSettObso.getHookedMessageKey("enableF4" ).getBool();
            settingBoolean[StateTmp, GroupFunctionKeys, ValueEnableF5 ] = sebSettObso.getHookedMessageKey("enableF5" ).getBool();
            settingBoolean[StateTmp, GroupFunctionKeys, ValueEnableF6 ] = sebSettObso.getHookedMessageKey("enableF6" ).getBool();
            settingBoolean[StateTmp, GroupFunctionKeys, ValueEnableF7 ] = sebSettObso.getHookedMessageKey("enableF7" ).getBool();
            settingBoolean[StateTmp, GroupFunctionKeys, ValueEnableF8 ] = sebSettObso.getHookedMessageKey("enableF8" ).getBool();
            settingBoolean[StateTmp, GroupFunctionKeys, ValueEnableF9 ] = sebSettObso.getHookedMessageKey("enableF9" ).getBool();
            settingBoolean[StateTmp, GroupFunctionKeys, ValueEnableF10] = sebSettObso.getHookedMessageKey("enableF10").getBool();
            settingBoolean[StateTmp, GroupFunctionKeys, ValueEnableF11] = sebSettObso.getHookedMessageKey("enableF11").getBool();
            settingBoolean[StateTmp, GroupFunctionKeys, ValueEnableF12] = sebSettObso.getHookedMessageKey("enableF12").getBool();

            return true;
        }



        // ****************************************************************
        // Convert arrays to C# object (to be written to .xml or .seb file)
        // ****************************************************************
        private Boolean ConvertArraysToCSharpObject()
        {
            // Copy the arrays "settingString"/"settingBoolean" to the C# object "sebSettings"

            sebSettObso.getUrlAddress("startURL")         .Url   = settingString [StateNew, GroupGeneral, ValueStartURL];
          //sebSettings.getUrlAddress("sebServerURL")     .Url   = settingString [StateNew, GroupGeneral, ValueSEBServerURL];
            sebSettObso.getPassword("hashedAdminPassword").Value = settingString [StateNew, GroupGeneral, ValueHashedAdminPassword];
            sebSettObso.getPassword("hashedQuitPassword") .Value = settingString [StateNew, GroupGeneral, ValueHashedQuitPassword];
            sebSettObso.getSecurityOption("allowQuit")          .setBool(settingBoolean[StateNew, GroupGeneral, ValueAllowQuit]);
            sebSettObso.getSecurityOption("irgnoreQuitPassword").setBool(settingBoolean[StateNew, GroupGeneral, ValueIgnoreQuitPassword]);
            sebSettObso.getExitKey("exitKey1")            .Value = settingString [StateNew, GroupGeneral, ValueExitKey1];
            sebSettObso.getExitKey("exitKey2")            .Value = settingString [StateNew, GroupGeneral, ValueExitKey2];
            sebSettObso.getExitKey("exitKey3")            .Value = settingString [StateNew, GroupGeneral, ValueExitKey3];

          //sebSettings.getPolicySetting ("sebPurpose"            ).Value = settingString [StateNew, GroupConfigFile, ValueSebPurpose];
          //sebSettings.getSecurityOption("startingAnExam"        ).setBool(settingBoolean[StateNew, GroupConfigFile, ValueStartingAnExam]);
          //sebSettings.getSecurityOption("configuringAClient"    ).setBool(settingBoolean[StateNew, GroupConfigFile, ValueConfiguringAClient]);
            sebSettObso.getSecurityOption("allowPreferencesWindow").setBool(settingBoolean[StateNew, GroupConfigFile, ValueAllowPreferencesWindow]);
          //sebSettings.getPassword("chooseIdentity"              ).Value = settingString [StateNew, GroupConfigFile, ValueChooseIdentity];
          //sebSettings.getPassword("hashedSettingsPassword"      ).Value = settingString [StateNew, GroupConfigFile, ValueHashedSettingsPassword];

          //sebSettings.getPolicySetting ("browserViewMode"           ).Value = settingString [StateNew, GroupAppearance, ValueBrowserViewMode];
          //sebSettings.getSecurityOption("useBrowserWindow"          ).setBool(settingBoolean[StateNew, GroupAppearance, ValueUseBrowserWindow]);
          //sebSettings.getSecurityOption("useFullScreenMode"         ).setBool(settingBoolean[StateNew, GroupAppearance, ValueUseFullScreenMode]);
            sebSettObso.getPolicySetting ("mainBrowserWindowWidth"    ).Value = settingString [StateNew, GroupAppearance, ValueMainBrowserWindowWidth];
            sebSettObso.getPolicySetting ("mainBrowserWindowHeight"   ).Value = settingString [StateNew, GroupAppearance, ValueMainBrowserWindowHeight];
          //sebSettings.getPolicySetting ("mainBrowserWindowPosition" ).Value = settingString [StateNew, GroupAppearance, ValueMainWindowPosition];
            sebSettObso.getSecurityOption("enableBrowserWindowToolbar").setBool(settingBoolean[StateNew, GroupAppearance, ValueEnableBrowserWindowToolbar]);
            sebSettObso.getSecurityOption(  "hideBrowserWindowToolbar").setBool(settingBoolean[StateNew, GroupAppearance, ValueHideBrowserWindowToolbar]);
            sebSettObso.getSecurityOption("showMenuBar"               ).setBool(settingBoolean[StateNew, GroupAppearance, ValueShowMenuBar]);
            sebSettObso.getSecurityOption("showTaskBar"               ).setBool(settingBoolean[StateNew, GroupAppearance, ValueShowTaskBar]);

          //sebSettings.getPolicySetting ("newBrowserWindowByLinkPolicy"  ).Value = settingString[StateNew, GroupBrowser, ValueNewWindowPolicyHTML];
          //sebSettings.getPolicySetting ("newBrowserWindowByLinkPolicy"  ).Value = settingString[StateNew, GroupBrowser, ValueNewWindowPolicyJava];
            sebSettObso.getSecurityOption("newBrowserWindowByLinkBlockForeign  ").setBool(settingBoolean[StateNew, GroupBrowser, ValueNewBrowserWindowByLinkBlockForeign]);
            sebSettObso.getSecurityOption("newBrowserWindowByScriptBlockForeign").setBool(settingBoolean[StateNew, GroupBrowser, ValueNewBrowserWindowByScriptBlockForeign]);
            sebSettObso.getPolicySetting ("newBrowserWindowByLinkWidth"   ).Value = settingString[StateNew, GroupBrowser, ValueNewBrowserWindowByLinkWidth];
            sebSettObso.getPolicySetting ("newBrowserWindowByLinkHeight"  ).Value = settingString[StateNew, GroupBrowser, ValueNewBrowserWindowByLinkHeight];
          //sebSettings.getPolicySetting ("newBrowserWindowByLinkPosition").Value = settingString[StateNew, GroupBrowser, ValueNewWindowPosition];
            sebSettObso.getSecurityOption("enablePlugins"    ).setBool(settingBoolean[StateNew, GroupBrowser, ValueEnablePlugIns]);
            sebSettObso.getSecurityOption("enableJava"       ).setBool(settingBoolean[StateNew, GroupBrowser, ValueEnableJava]);
            sebSettObso.getSecurityOption("enableJavaScript" ).setBool(settingBoolean[StateNew, GroupBrowser, ValueEnableJavaScript]);
            sebSettObso.getSecurityOption("blockPopUpWindows").setBool(settingBoolean[StateNew, GroupBrowser, ValueBlockPopUpWindows]);
            sebSettObso.getSecurityOption("allowBrowsingBackForward").setBool(settingBoolean[StateNew, GroupBrowser, ValueAllowBrowsingBackForward]);
            sebSettObso.getSecurityOption("enableSebBrowser"        ).setBool(settingBoolean[StateNew, GroupBrowser, ValueEnableSebBrowser]);

            sebSettObso.getSecurityOption   ("allowDownUploads").setBool(settingBoolean[StateNew, GroupDownUploads, ValueAllowDownUploads]);
            sebSettObso.getSecurityOption   ("openDownloads"   ).setBool(settingBoolean[StateNew, GroupDownUploads, ValueOpenDownloads]);
            sebSettObso.getSecurityOption   ("downloadPDFFiles").setBool(settingBoolean[StateNew, GroupDownUploads, ValueDownloadPDFFiles]);
            sebSettObso.getDownloadDirectory("downloadDirectoryWin"    ).Path  = settingString[StateNew, GroupDownUploads, ValueDownloadDirectoryWin];
            sebSettObso.getPolicySetting    ("chooseFileToUploadPolicy").Value = settingString[StateNew, GroupDownUploads, ValueChooseFileToUploadPolicy];

          //sebSettings.getUrlAddress("browserExamKey").Url = settingString[StateNew, GroupExam, ValueBrowserExamKey];
            sebSettObso.getSecurityOption("copyExamKeyToClipboardWhenQuitting").setBool(settingBoolean[StateNew, GroupExam, ValueCopyBrowserExamKey]);
            sebSettObso.getSecurityOption("sendBrowserExamKey"                ).setBool(settingBoolean[StateNew, GroupExam, ValueSendBrowserExamKey]);
            sebSettObso.getUrlAddress("quitURL").Url = settingString[StateNew, GroupExam, ValueQuitURL];

            sebSettObso.getSecurityOption("monitorProcesses         ").setBool(settingBoolean[StateNew, GroupApplications, ValueMonitorProcesses]);
            sebSettObso.getSecurityOption("allowSwitchToApplications").setBool(settingBoolean[StateNew, GroupApplications, ValueAllowSwitchToApplications]);
            sebSettObso.getSecurityOption("allowFlashFullscreen     ").setBool(settingBoolean[StateNew, GroupApplications, ValueAllowFlashFullscreen]);

            sebSettObso.getPolicySetting ("sebServicePolicy"   ).Value = settingString [StateNew, GroupSecurity, ValueSebServicePolicy];
            sebSettObso.getSecurityOption("allowVirtualMachine").setBool(settingBoolean[StateNew, GroupSecurity, ValueAllowVirtualMachine]);
            sebSettObso.getSecurityOption("createNewDesktop"   ).setBool(settingBoolean[StateNew, GroupSecurity, ValueCreateNewDesktop]);
            sebSettObso.getSecurityOption("allowUserSwitching" ).setBool(settingBoolean[StateNew, GroupSecurity, ValueAllowUserSwitching]);
            sebSettObso.getSecurityOption("enableLog"          ).setBool(settingBoolean[StateNew, GroupSecurity, ValueEnableLogging]);

            sebSettObso.getRegistryValue("insideSebEnableSwitchUser"       ).setBool(settingBoolean[StateNew, GroupInsideSeb, ValueInsideSebEnableSwitchUser]);
            sebSettObso.getRegistryValue("insideSebEnableLockThisComputer" ).setBool(settingBoolean[StateNew, GroupInsideSeb, ValueInsideSebEnableLockThisComputer]);
            sebSettObso.getRegistryValue("insideSebEnableChangePassword"   ).setBool(settingBoolean[StateNew, GroupInsideSeb, ValueInsideSebEnableChangeAPassword]);
            sebSettObso.getRegistryValue("insideSebEnableStartTaskManager" ).setBool(settingBoolean[StateNew, GroupInsideSeb, ValueInsideSebEnableStartTaskManager]);
            sebSettObso.getRegistryValue("insideSebEnableLogOff"           ).setBool(settingBoolean[StateNew, GroupInsideSeb, ValueInsideSebEnableLogOff]);
            sebSettObso.getRegistryValue("insideSebEnableShutDown"         ).setBool(settingBoolean[StateNew, GroupInsideSeb, ValueInsideSebEnableShutDown]);
            sebSettObso.getRegistryValue("insideSebEnableEaseOfAccess"     ).setBool(settingBoolean[StateNew, GroupInsideSeb, ValueInsideSebEnableEaseOfAccess]);
            sebSettObso.getRegistryValue("insideSebEnableVmWareClientShade").setBool(settingBoolean[StateNew, GroupInsideSeb, ValueInsideSebEnableVmWareClientShade]);

            sebSettObso.getRegistryValue("outsideSebEnableSwitchUser"       ).setBool(settingBoolean[StateNew, GroupOutsideSeb, ValueOutsideSebEnableSwitchUser]);
            sebSettObso.getRegistryValue("outsideSebEnableLockThisComputer" ).setBool(settingBoolean[StateNew, GroupOutsideSeb, ValueOutsideSebEnableLockThisComputer]);
            sebSettObso.getRegistryValue("outsideSebEnableChangePassword"   ).setBool(settingBoolean[StateNew, GroupOutsideSeb, ValueOutsideSebEnableChangeAPassword]);
            sebSettObso.getRegistryValue("outsideSebEnableStartTaskManager" ).setBool(settingBoolean[StateNew, GroupOutsideSeb, ValueOutsideSebEnableStartTaskManager]);
            sebSettObso.getRegistryValue("outsideSebEnableLogOff"           ).setBool(settingBoolean[StateNew, GroupOutsideSeb, ValueOutsideSebEnableLogOff]);
            sebSettObso.getRegistryValue("outsideSebEnableShutDown"         ).setBool(settingBoolean[StateNew, GroupOutsideSeb, ValueOutsideSebEnableShutDown]);
            sebSettObso.getRegistryValue("outsideSebEnableEaseOfAccess"     ).setBool(settingBoolean[StateNew, GroupOutsideSeb, ValueOutsideSebEnableEaseOfAccess]);
            sebSettObso.getRegistryValue("outsideSebEnableVmWareClientShade").setBool(settingBoolean[StateNew, GroupOutsideSeb, ValueOutsideSebEnableVmWareClientShade]);

            sebSettObso.getSecurityOption("hookMessages").setBool(settingBoolean[StateNew, GroupHookedKeys, ValueHookMessages]);

            sebSettObso.getHookedMessageKey("enableEsc"       ).setBool(settingBoolean[StateNew, GroupSpecialKeys, ValueEnableEsc]);
            sebSettObso.getHookedMessageKey("enableCtrlEsc"   ).setBool(settingBoolean[StateNew, GroupSpecialKeys, ValueEnableCtrlEsc]);
            sebSettObso.getHookedMessageKey("enableAltEsc"    ).setBool(settingBoolean[StateNew, GroupSpecialKeys, ValueEnableAltEsc]);
            sebSettObso.getHookedMessageKey("enableAltTab"    ).setBool(settingBoolean[StateNew, GroupSpecialKeys, ValueEnableAltTab]);
            sebSettObso.getHookedMessageKey("enableAltF4"     ).setBool(settingBoolean[StateNew, GroupSpecialKeys, ValueEnableAltF4]);
          //sebSettings.getHookedMessageKey("enableStartMenu" ).setBool(settingBoolean[StateNew, GroupSpecialKeys, ValueEnableStartMenu]);
            sebSettObso.getHookedMessageKey("enableRightMouse").setBool(settingBoolean[StateNew, GroupSpecialKeys, ValueEnableRightMouse]);

            sebSettObso.getHookedMessageKey("enableF1" ).setBool(settingBoolean[StateNew, GroupFunctionKeys, ValueEnableF1 ]);
            sebSettObso.getHookedMessageKey("enableF2" ).setBool(settingBoolean[StateNew, GroupFunctionKeys, ValueEnableF2 ]);
            sebSettObso.getHookedMessageKey("enableF3" ).setBool(settingBoolean[StateNew, GroupFunctionKeys, ValueEnableF3 ]);
            sebSettObso.getHookedMessageKey("enableF4" ).setBool(settingBoolean[StateNew, GroupFunctionKeys, ValueEnableF4 ]);
            sebSettObso.getHookedMessageKey("enableF5" ).setBool(settingBoolean[StateNew, GroupFunctionKeys, ValueEnableF5 ]);
            sebSettObso.getHookedMessageKey("enableF6" ).setBool(settingBoolean[StateNew, GroupFunctionKeys, ValueEnableF6 ]);
            sebSettObso.getHookedMessageKey("enableF7" ).setBool(settingBoolean[StateNew, GroupFunctionKeys, ValueEnableF7 ]);
            sebSettObso.getHookedMessageKey("enableF8" ).setBool(settingBoolean[StateNew, GroupFunctionKeys, ValueEnableF8 ]);
            sebSettObso.getHookedMessageKey("enableF9" ).setBool(settingBoolean[StateNew, GroupFunctionKeys, ValueEnableF9 ]);
            sebSettObso.getHookedMessageKey("enableF10").setBool(settingBoolean[StateNew, GroupFunctionKeys, ValueEnableF10]);
            sebSettObso.getHookedMessageKey("enableF11").setBool(settingBoolean[StateNew, GroupFunctionKeys, ValueEnableF11]);
            sebSettObso.getHookedMessageKey("enableF12").setBool(settingBoolean[StateNew, GroupFunctionKeys, ValueEnableF12]);

            return true;
        }
*/


        // **************
        //
        // Event handlers
        //
        // **************



        // ***************
        // Group "General"
        // ***************
        private void textBoxStartURL_TextChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageStartURL] = textBoxStartURL.Text;
        }

        private void buttonPasteFromSavedClipboard_Click(object sender, EventArgs e)
        {

        }

        private void textBoxSebServerURL_TextChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageSebServerURL] = textBoxSebServerURL.Text;
        }

        private void textBoxAdminPassword_TextChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageAdminPassword] = textBoxAdminPassword.Text;
        }

        private void textBoxConfirmAdminPassword_TextChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageConfirmAdminPassword] = textBoxConfirmAdminPassword.Text;
        }

        private void checkBoxAllowQuit_CheckedChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageAllowQuit] = checkBoxAllowQuit.Checked;
        }

        private void checkBoxIgnoreQuitPassword_CheckedChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageIgnoreQuitPassword] = checkBoxIgnoreQuitPassword.Checked;
        }

        private void textBoxQuitPassword_TextChanged(object sender, EventArgs e)
        {
            // Get the new quit password
            String newStringQuitPassword = textBoxQuitPassword.Text;

            // Encrypt the new quit password
            byte[] passwordBytes = Encoding.Default.GetBytes(newStringQuitPassword);
            byte[] hashcodeBytes = sha256.ComputeHash(passwordBytes);

            String newStringQuitHashcode = string.Empty;
            for (int i = 0; i < hashcodeBytes.Length; i++)
                newStringQuitHashcode += hashcodeBytes[i].ToString("X");

            textBoxHashedQuitPassword.Text = newStringQuitHashcode;

            sebSettingsNew[MessageQuitPassword      ] = newStringQuitPassword;
            sebSettingsNew[MessageHashedQuitPassword] = newStringQuitHashcode;
        }

        private void textBoxConfirmQuitPassword_TextChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageConfirmQuitPassword] = textBoxConfirmQuitPassword.Text;
        }

        private void listBoxExitKey1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Make sure that all three exit keys are different.
            // If selected key is already occupied, revert to previously selected key.
            if ((listBoxExitKey1.SelectedIndex == listBoxExitKey2.SelectedIndex) ||
                (listBoxExitKey1.SelectedIndex == listBoxExitKey3.SelectedIndex))
                 listBoxExitKey1.SelectedIndex =  (int)sebSettingsNew[MessageExitKey1];
            sebSettingsNew[MessageExitKey1] = listBoxExitKey1.SelectedIndex;
        }

        private void listBoxExitKey2_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Make sure that all three exit keys are different.
            // If selected key is already occupied, revert to previously selected key.
            if ((listBoxExitKey2.SelectedIndex == listBoxExitKey1.SelectedIndex) ||
                (listBoxExitKey2.SelectedIndex == listBoxExitKey3.SelectedIndex))
                 listBoxExitKey2.SelectedIndex =  (int)sebSettingsNew[MessageExitKey2];
            sebSettingsNew[MessageExitKey2] = listBoxExitKey2.SelectedIndex;
        }

        private void listBoxExitKey3_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Make sure that all three exit keys are different.
            // If selected key is already occupied, revert to previously selected key.
            if ((listBoxExitKey3.SelectedIndex == listBoxExitKey1.SelectedIndex) ||
                (listBoxExitKey3.SelectedIndex == listBoxExitKey2.SelectedIndex))
                 listBoxExitKey3.SelectedIndex =  (int)sebSettingsNew[MessageExitKey3];
            sebSettingsNew[MessageExitKey3] = listBoxExitKey3.SelectedIndex;
        }

        private void buttonAbout_Click(object sender, EventArgs e)
        {

        }

        private void buttonHelp_Click(object sender, EventArgs e)
        {

        }

        private void buttonQuit_Click(object sender, EventArgs e)
        {
/*
            // If no file has been opened, save the current settings
            // to the default configuration file ("SebStarter.ini/xml/seb")
            if (currentFileSebStarterIni.Equals(""))
            {
                currentFileSebStarter = defaultFileSebStarter;
                currentPathSebStarter = defaultPathSebStarter;
            }

            String fileName = currentPathSebStarter;

            // Cut off the file extension ".ini", ".xml" or ".seb",
            // that is the last 4 characters of the file name
            String fileNameRaw = fileName.Substring(0, fileName.Length - 4);
            String fileNameExt = fileName.Substring(fileName.Length - 4, 4);
            String fileNameIni = fileNameRaw + ".ini";
            String fileNameXml = fileNameRaw + ".xml";
            String fileNameSeb = fileNameRaw + ".seb";

            // Save the configuration file so that nothing gets lost
            SaveIniFile(fileNameIni);
            SaveXmlFile(fileNameXml);
            SaveSebFile(fileNameSeb);
*/
            // Close the configuration window and exit
            this.Close();
        }

        private void buttonRestartSEB_Click(object sender, EventArgs e)
        {

        }



        // *******************
        // Group "Config File"
        // *******************
        private void radioButtonStartingAnExam_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonStartingAnExam.Checked == true)
                 sebSettingsNew[MessageSebConfigPurpose] = 0;
            else sebSettingsNew[MessageSebConfigPurpose] = 1;
        }

        private void radioButtonConfiguringAClient_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonConfiguringAClient.Checked == true)
                 sebSettingsNew[MessageSebConfigPurpose] = 1;
            else sebSettingsNew[MessageSebConfigPurpose] = 0;
        }

        private void checkBoxAllowPreferencesWindow_CheckedChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageAllowPreferencesWindow] = checkBoxAllowPreferencesWindow.Checked;
        }

        private void comboBoxCryptoIdentity_SelectedIndexChanged(object sender, EventArgs e)
        {
            settingInteger[StateNew, GroupConfigFile, ValueCryptoIdentity] = comboBoxCryptoIdentity.SelectedIndex;
            settingString [StateNew, GroupConfigFile, ValueCryptoIdentity] = comboBoxCryptoIdentity.Text;
        }

        private void comboBoxCryptoIdentity_TextUpdate(object sender, EventArgs e)
        {
            settingInteger[StateNew, GroupConfigFile, ValueCryptoIdentity] = comboBoxCryptoIdentity.SelectedIndex;
            settingString [StateNew, GroupConfigFile, ValueCryptoIdentity] = comboBoxCryptoIdentity.Text;
        }

        private void textBoxSettingsPassword_TextChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageSettingsPassword] = textBoxSettingsPassword.Text;
        }

        private void textBoxConfirmSettingsPassword_TextChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageConfirmSettingsPassword] = textBoxConfirmSettingsPassword.Text;
        }


        private void buttonDefaultSettings_Click(object sender, EventArgs e)
        {
            CopySettingsArrays    (      StateDef,       StateNew);
            CopySettingsDictionary(sebSettingsDef, sebSettingsNew);
            SetWidgetsToNewSettings();
        }

        private void buttonRevertToLastOpened_Click(object sender, EventArgs e)
        {
            CopySettingsArrays    (      StateOld,       StateNew);
            CopySettingsDictionary(sebSettingsOld, sebSettingsNew);
            SetWidgetsToNewSettings();
        }


        private void labelOpenSettings_Click(object sender, EventArgs e)
        {
            // Set the default directory and file name in the File Dialog
            openFileDialogSebConfigFile.InitialDirectory = currentDireSebConfigFile;
            openFileDialogSebConfigFile.FileName         = currentFileSebConfigFile;

            // Get the user inputs in the File Dialog
            DialogResult fileDialogResult = openFileDialogSebConfigFile.ShowDialog();
            String       fileName         = openFileDialogSebConfigFile.FileName;

            // If the user clicked "Cancel", do nothing
            if (fileDialogResult.Equals(DialogResult.Cancel)) return;

            // Cut off the file extension ".ini", ".xml" or ".seb",
            // that is the last 4 characters of the file name
            String fileNameRaw = fileName.Substring(0, fileName.Length - 4);
            String fileNameExt = fileName.Substring(fileName.Length - 4, 4);
            String fileNameIni = fileNameRaw + ".ini";
            String fileNameXml = fileNameRaw + ".xml";
            String fileNameSeb = fileNameRaw + ".seb";

            // If the user clicked "OK", read the settings from the configuration file
            if (fileNameExt.Equals(".ini")) OpenIniFile(fileNameIni);
            if (fileNameExt.Equals(".xml")) OpenXmlFile(fileNameXml);
            if (fileNameExt.Equals(".seb")) OpenSebFile(fileNameSeb);
        }


        private void labelSaveSettingsAs_Click(object sender, EventArgs e)
        {
            // Set the default directory and file name in the File Dialog
            saveFileDialogSebConfigFile.InitialDirectory = currentDireSebConfigFile;
            saveFileDialogSebConfigFile.FileName         = currentFileSebConfigFile;

            // Get the user inputs in the File Dialog
            DialogResult fileDialogResult = saveFileDialogSebConfigFile.ShowDialog();
            String       fileName         = saveFileDialogSebConfigFile.FileName;

            // If the user clicked "Cancel", do nothing
            if (fileDialogResult.Equals(DialogResult.Cancel)) return;

            // Cut off the file extension ".ini", ".xml" or ".seb",
            // that is the last 4 characters of the file name
            String fileNameRaw = fileName.Substring(0, fileName.Length - 4);
            String fileNameExt = fileName.Substring(fileName.Length - 4, 4);
            String fileNameIni = fileNameRaw + ".ini";
            String fileNameXml = fileNameRaw + ".xml";
            String fileNameSeb = fileNameRaw + ".seb";

            // If the user clicked "OK", write the settings to the configuration file
            if (fileNameExt.Equals(".ini")) SaveIniFile(fileNameIni);
            if (fileNameExt.Equals(".xml")) SaveXmlFile(fileNameXml);
            if (fileNameExt.Equals(".seb")) SaveSebFile(fileNameSeb);
        }



        // ******************
        // Group "Appearance"
        // ******************
        private void radioButtonUseBrowserWindow_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonUseBrowserWindow.Checked == true)
                 sebSettingsNew[MessageBrowserViewMode] = 0;
            else sebSettingsNew[MessageBrowserViewMode] = 1;
        }

        private void radioButtonUseFullScreenMode_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonUseFullScreenMode.Checked == true)
                 sebSettingsNew[MessageBrowserViewMode] = 1;
            else sebSettingsNew[MessageBrowserViewMode] = 0;
        }

        private void comboBoxMainBrowserWindowWidth_SelectedIndexChanged(object sender, EventArgs e)
        {
            settingInteger[StateNew, GroupAppearance, ValueMainBrowserWindowWidth] = comboBoxMainBrowserWindowWidth.SelectedIndex;
            settingString [StateNew, GroupAppearance, ValueMainBrowserWindowWidth] = comboBoxMainBrowserWindowWidth.Text;
          //sebSettingsNew[MessageMainBrowserWindowWidth] = comboBoxMainBrowserWindowWidth.SelectedIndex;
            sebSettingsNew[MessageMainBrowserWindowWidth] = comboBoxMainBrowserWindowWidth.Text;
        }

        private void comboBoxMainBrowserWindowWidth_TextUpdate(object sender, EventArgs e)
        {
            settingInteger[StateNew, GroupAppearance, ValueMainBrowserWindowWidth] = comboBoxMainBrowserWindowWidth.SelectedIndex;
            settingString [StateNew, GroupAppearance, ValueMainBrowserWindowWidth] = comboBoxMainBrowserWindowWidth.Text;
          //sebSettingsNew[MessageMainBrowserWindowWidth] = comboBoxMainBrowserWindowWidth.SelectedIndex;
            sebSettingsNew[MessageMainBrowserWindowWidth] = comboBoxMainBrowserWindowWidth.Text;
        }

        private void comboBoxMainBrowserWindowHeight_SelectedIndexChanged(object sender, EventArgs e)
        {
            settingInteger[StateNew, GroupAppearance, ValueMainBrowserWindowHeight] = comboBoxMainBrowserWindowHeight.SelectedIndex;
            settingString [StateNew, GroupAppearance, ValueMainBrowserWindowHeight] = comboBoxMainBrowserWindowHeight.Text;
          //sebSettingsNew[MessageMainBrowserWindowHeight] = comboBoxMainBrowserWindowHeight.SelectedIndex;
            sebSettingsNew[MessageMainBrowserWindowHeight] = comboBoxMainBrowserWindowHeight.Text;
        }

        private void comboBoxMainBrowserWindowHeight_TextUpdate(object sender, EventArgs e)
        {
            settingInteger[StateNew, GroupAppearance, ValueMainBrowserWindowHeight] = comboBoxMainBrowserWindowHeight.SelectedIndex;
            settingString [StateNew, GroupAppearance, ValueMainBrowserWindowHeight] = comboBoxMainBrowserWindowHeight.Text;
          //sebSettingsNew[MessageMainBrowserWindowHeight] = comboBoxMainBrowserWindowHeight.SelectedIndex;
            sebSettingsNew[MessageMainBrowserWindowHeight] = comboBoxMainBrowserWindowHeight.Text;
        }

        private void listBoxMainBrowserWindowPositioning_SelectedIndexChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageMainBrowserWindowPositioning] = listBoxMainBrowserWindowPositioning.SelectedIndex;
        }

        private void checkBoxEnableBrowserWindowToolbar_CheckedChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageEnableBrowserWindowToolbar] = checkBoxEnableBrowserWindowToolbar.Checked;
            checkBoxHideBrowserWindowToolbar.Enabled          = checkBoxEnableBrowserWindowToolbar.Checked;
        }

        private void checkBoxHideBrowserWindowToolbar_CheckedChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageHideBrowserWindowToolbar] = checkBoxHideBrowserWindowToolbar.Checked;
        }

        private void checkBoxShowMenuBar_CheckedChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageShowMenuBar] = checkBoxShowMenuBar.Checked;
        }

        private void checkBoxShowTaskBar_CheckedChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageShowTaskBar] = checkBoxShowTaskBar.Checked;
        }



        // ***************
        // Group "Browser"
        // ***************
        private void listBoxOpenLinksHTML_SelectedIndexChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageNewBrowserWindowByLinkPolicy] = listBoxOpenLinksHTML.SelectedIndex;
        }

        private void listBoxOpenLinksJava_SelectedIndexChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageNewBrowserWindowByScriptPolicy] = listBoxOpenLinksJava.SelectedIndex;
        }

        private void checkBoxBlockLinksHTML_CheckedChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageNewBrowserWindowByLinkBlockForeign] = checkBoxBlockLinksHTML.Checked;
        }

        private void checkBoxBlockLinksJava_CheckedChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageNewBrowserWindowByScriptBlockForeign] = checkBoxBlockLinksJava.Checked;
        }

        private void comboBoxNewBrowserWindowWidth_SelectedIndexChanged(object sender, EventArgs e)
        {
            settingInteger[StateNew, GroupBrowser, ValueNewBrowserWindowByLinkWidth] = comboBoxNewBrowserWindowWidth.SelectedIndex;
            settingString [StateNew, GroupBrowser, ValueNewBrowserWindowByLinkWidth] = comboBoxNewBrowserWindowWidth.Text;
          //sebSettingsNew[MessageNewBrowserWindowByLinkWidth] = comboBoxNewBrowserWindowWidth.SelectedIndex;
            sebSettingsNew[MessageNewBrowserWindowByLinkWidth] = comboBoxNewBrowserWindowWidth.Text;
        }

        private void comboBoxNewBrowserWindowWidth_TextUpdate(object sender, EventArgs e)
        {
            settingInteger[StateNew, GroupBrowser, ValueNewBrowserWindowByLinkWidth] = comboBoxNewBrowserWindowWidth.SelectedIndex;
            settingString [StateNew, GroupBrowser, ValueNewBrowserWindowByLinkWidth] = comboBoxNewBrowserWindowWidth.Text;
          //sebSettingsNew[MessageNewBrowserWindowByLinkWidth] = comboBoxNewBrowserWindowWidth.SelectedIndex;
            sebSettingsNew[MessageNewBrowserWindowByLinkWidth] = comboBoxNewBrowserWindowWidth.Text;
        }

        private void comboBoxNewBrowserWindowHeight_SelectedIndexChanged(object sender, EventArgs e)
        {
            settingInteger[StateNew, GroupBrowser, ValueNewBrowserWindowByLinkHeight] = comboBoxNewBrowserWindowHeight.SelectedIndex;
            settingString [StateNew, GroupBrowser, ValueNewBrowserWindowByLinkHeight] = comboBoxNewBrowserWindowHeight.Text;
          //sebSettingsNew[MessageNewBrowserWindowByLinkHeight] = comboBoxNewBrowserWindowHeight.SelectedIndex;
            sebSettingsNew[MessageNewBrowserWindowByLinkHeight] = comboBoxNewBrowserWindowHeight.Text;
        }

        private void comboBoxNewBrowserWindowHeight_TextUpdate(object sender, EventArgs e)
        {
            settingInteger[StateNew, GroupBrowser, ValueNewBrowserWindowByLinkHeight] = comboBoxNewBrowserWindowHeight.SelectedIndex;
            settingString [StateNew, GroupBrowser, ValueNewBrowserWindowByLinkHeight] = comboBoxNewBrowserWindowHeight.Text;
          //sebSettingsNew[MessageNewBrowserWindowByLinkHeight] = comboBoxNewBrowserWindowHeight.SelectedIndex;
            sebSettingsNew[MessageNewBrowserWindowByLinkHeight] = comboBoxNewBrowserWindowHeight.Text;
        }

        private void listBoxNewBrowserWindowPositioning_SelectedIndexChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageNewBrowserWindowByLinkPositioning] = listBoxNewBrowserWindowPositioning.SelectedIndex;
        }

        private void checkBoxEnablePlugins_CheckedChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageEnablePlugIns] = checkBoxEnablePlugIns.Checked;
        }

        private void checkBoxEnableJava_CheckedChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageEnableJava] = checkBoxEnableJava.Checked;
        }

        private void checkBoxEnableJavaScript_CheckedChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageEnableJavaScript] = checkBoxEnableJavaScript.Checked;
        }

        private void checkBoxBlockPopUpWindows_CheckedChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageBlockPopUpWindows] = checkBoxBlockPopUpWindows.Checked;
        }

        private void checkBoxAllowBrowsingBackForward_CheckedChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageAllowBrowsingBackForward] = checkBoxAllowBrowsingBackForward.Checked;
        }

        // BEWARE: you must invert this value since "Use Without" is "Not Enable"!
        private void checkBoxUseSebWithoutBrowser_CheckedChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageEnableSebBrowser] = !(checkBoxUseSebWithoutBrowser.Checked);
        }



        // ********************
        // Group "Down/Uploads"
        // ********************
        private void checkBoxAllowDownUploads_CheckedChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageAllowDownUploads] = checkBoxAllowDownUploads.Checked;
        }

        private void buttonDownloadDirectoryWin_Click(object sender, EventArgs e)
        {
            // Set the default directory in the Folder Browser Dialog
            folderBrowserDialogDownloadDirectoryWin.RootFolder = Environment.SpecialFolder.DesktopDirectory;
//          folderBrowserDialogDownloadDirectoryWin.RootFolder = Environment.CurrentDirectory;

            // Get the user inputs in the File Dialog
            DialogResult dialogResult = folderBrowserDialogDownloadDirectoryWin.ShowDialog();
            String               path = folderBrowserDialogDownloadDirectoryWin.SelectedPath;

            // If the user clicked "Cancel", do nothing
            if (dialogResult.Equals(DialogResult.Cancel)) return;

            // If the user clicked "OK", ...
            sebSettingsNew[MessageDownloadDirectoryWin]     = path;
                             labelDownloadDirectoryWin.Text = path;
        }

        private void checkBoxOpenDownloads_CheckedChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageOpenDownloads] = checkBoxOpenDownloads.Checked;
        }

        private void listBoxChooseFileToUploadPolicy_SelectedIndexChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageChooseFileToUploadPolicy] = listBoxChooseFileToUploadPolicy.SelectedIndex;
        }

        private void checkBoxDownloadPDFFiles_CheckedChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageDownloadPDFFiles] = checkBoxDownloadPDFFiles.Checked;
        }



        // ************
        // Group "Exam"
        // ************
        private void buttonGenerateBrowserExamKey_Click(object sender, EventArgs e)
        {

        }

        private void textBoxBrowserExamKey_TextChanged(object sender, EventArgs e)
        {
          //sebSettingsNew[MessageBrowserExamKey] = textBoxBrowserExamKey.Text;
        }

        private void checkBoxCopyBrowserExamKey_CheckedChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageCopyBrowserExamKey] = checkBoxCopyBrowserExamKey.Checked;
        }

        private void checkBoxSendBrowserExamKey_CheckedChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageSendBrowserExamKey] = checkBoxSendBrowserExamKey.Checked;
        }

        private void textBoxQuitURL_TextChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageQuitURL] = textBoxQuitURL.Text;
        }



        // ********************
        // Group "Applications"
        // ********************
        private void checkBoxMonitorProcesses_CheckedChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageMonitorProcesses] = checkBoxMonitorProcesses.Checked;
        }

        // ***************************
        // Group "Permitted Processes"
        // ***************************
        private void checkBoxAllowSwitchToApplications_CheckedChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageAllowSwitchToApplications] = checkBoxAllowSwitchToApplications.Checked;
            checkBoxAllowFlashFullscreen.Enabled             = checkBoxAllowSwitchToApplications.Checked;
        }

        private void checkBoxAllowFlashFullscreen_CheckedChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageAllowFlashFullscreen] = checkBoxAllowFlashFullscreen.Checked;
        }


        private void listViewPermittedProcesses_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListView.SelectedIndexCollection indices = listViewPermittedProcesses.SelectedIndices;
            ListViewItem selectedItem = listViewPermittedProcesses.SelectedItems[0];
            int selectedIndex1 = selectedItem.Index;
            int selectedIndex  = listViewPermittedProcesses.SelectedItems[0].Index;


            List<object> permittedProcessList = null;
            Dictionary<string, object>   dict = null;

            permittedProcessList = (List<object>) sebSettingsNew[MessagePermittedProcesses];

            if ((selectedIndex >= 0) && (selectedIndex < permittedProcessList.Count))
            {
                dict = (Dictionary<string, object>) permittedProcessList[selectedIndex];

                Boolean activeBoolean = (Boolean) dict[MessageActive];
                Int32       osInteger = (Int32)   dict[MessageOS];

                String  activeString  = activeBoolean.ToString();
                String      osString  = StringOperatingSystem[osInteger];

                checkBoxPermittedProcessActive   .Checked = (Boolean) dict[MessageActive];
                checkBoxPermittedProcessAutostart.Checked = (Boolean) dict[MessageAutostart];
                checkBoxPermittedProcessAutohide .Checked = (Boolean) dict[MessageAutohide];
                checkBoxPermittedProcessAllowUser.Checked = (Boolean) dict[MessageAllowUser];

                 listBoxPermittedProcessOS.SelectedIndex = (Int32) dict[MessageOS];

                 textBoxPermittedProcessAppTitle   .Text = (String) dict[MessageAppTitle];
                 textBoxPermittedProcessDescription.Text = (String) dict[MessageDescription];
                 textBoxPermittedProcessExecutable .Text = (String) dict[MessageExecutable];
                 textBoxPermittedProcessPath       .Text = (String) dict[MessagePath];
                 textBoxPermittedProcessIdentifier .Text = (String) dict[MessageIdentifier];
            }

        }


        // ****************************
        // Group "Prohibited Processes"
        // ****************************


        // ***************
        // Group "Network"
        // ***************



        // ****************
        // Group "Security"
        // ****************
        private void listBoxSebServicePolicy_SelectedIndexChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageSebServicePolicy] = listBoxSebServicePolicy.SelectedIndex;
        }

        private void checkBoxAllowVirtualMachine_CheckedChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageAllowVirtualMachine] = checkBoxAllowVirtualMachine.Checked;
        }

        private void checkBoxCreateNewDesktop_CheckedChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageCreateNewDesktop] = checkBoxCreateNewDesktop.Checked;
        }

        private void checkBoxAllowUserSwitching_CheckedChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageAllowUserSwitching] = checkBoxAllowUserSwitching.Checked;
        }

        private void checkBoxEnableLogging_CheckedChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageEnableLogging] = checkBoxEnableLogging.Checked;
        }

        private void buttonLogDirectoryWin_Click(object sender, EventArgs e)
        {
            // Set the default directory in the Folder Browser Dialog
            folderBrowserDialogLogDirectoryWin.RootFolder = Environment.SpecialFolder.MyDocuments;
//          folderBrowserDialogLogDirectoryWin.RootFolder = Environment.CurrentDirectory;

            // Get the user inputs in the File Dialog
            DialogResult dialogResult = folderBrowserDialogLogDirectoryWin.ShowDialog();
            String               path = folderBrowserDialogLogDirectoryWin.SelectedPath;

            // If the user clicked "Cancel", do nothing
            if (dialogResult.Equals(DialogResult.Cancel)) return;

            // If the user clicked "OK", ...
            sebSettingsNew[MessageLogDirectoryWin]     = path;
                             labelLogDirectoryWin.Text = path;
        }



        // ****************
        // Group "Registry"
        // ****************
        private void radioButtonPreviousValuesFromFile_CheckedChanged(object sender, EventArgs e)
        {
            groupBoxOutsideSeb.Visible = (radioButtonInsideValuesManually.Checked == true);
            groupBoxOutsideSeb.Enabled = (radioButtonInsideValuesManually.Checked == true);
        }

        private void radioButtonEnvironmentValues_CheckedChanged(object sender, EventArgs e)
        {
            groupBoxOutsideSeb.Visible = true;
            groupBoxOutsideSeb.Enabled = (radioButtonInsideValuesManually.Checked == true);
        }

        private void radioButtonInsideValuesManually_CheckedChanged(object sender, EventArgs e)
        {
            groupBoxOutsideSeb.Visible = true;
            groupBoxOutsideSeb.Enabled = (radioButtonInsideValuesManually.Checked == true);
        }



        // ******************
        // Group "Inside SEB"
        // ******************
        private void checkBoxInsideSebEnableSwitchUser_CheckedChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageInsideSebEnableSwitchUser] = checkBoxInsideSebEnableSwitchUser.Checked;
        }

        private void checkBoxInsideSebEnableLockThisComputer_CheckedChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageInsideSebEnableLockThisComputer] = checkBoxInsideSebEnableLockThisComputer.Checked;
        }

        private void checkBoxInsideSebEnableChangeAPassword_CheckedChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageInsideSebEnableChangeAPassword] = checkBoxInsideSebEnableChangeAPassword.Checked;
        }

        private void checkBoxInsideSebEnableStartTaskManager_CheckedChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageInsideSebEnableStartTaskManager] = checkBoxInsideSebEnableStartTaskManager.Checked;
        }

        private void checkBoxInsideSebEnableLogOff_CheckedChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageInsideSebEnableLogOff] = checkBoxInsideSebEnableLogOff.Checked;
        }

        private void checkBoxInsideSebEnableShutDown_CheckedChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageInsideSebEnableShutDown] = checkBoxInsideSebEnableShutDown.Checked;
        }

        private void checkBoxInsideSebEnableEaseOfAccess_CheckedChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageInsideSebEnableEaseOfAccess] = checkBoxInsideSebEnableEaseOfAccess.Checked;
        }

        private void checkBoxInsideSebEnableVmWareClientShade_CheckedChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageInsideSebEnableVmWareClientShade] = checkBoxInsideSebEnableVmWareClientShade.Checked;
        }



        // *******************
        // Group "Outside SEB"
        // *******************
        private void checkBoxOutsideSebEnableSwitchUser_CheckedChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageOutsideSebEnableSwitchUser] = checkBoxOutsideSebEnableSwitchUser.Checked;
        }

        private void checkBoxOutsideSebEnableLockThisComputer_CheckedChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageOutsideSebEnableLockThisComputer] = checkBoxOutsideSebEnableLockThisComputer.Checked;
        }

        private void checkBoxOutsideSebEnableChangeAPassword_CheckedChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageOutsideSebEnableChangeAPassword] = checkBoxOutsideSebEnableChangeAPassword.Checked;
        }

        private void checkBoxOutsideSebEnableStartTaskManager_CheckedChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageOutsideSebEnableStartTaskManager] = checkBoxOutsideSebEnableStartTaskManager.Checked;
        }

        private void checkBoxOutsideSebEnableLogOff_CheckedChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageOutsideSebEnableLogOff] = checkBoxOutsideSebEnableLogOff.Checked;
        }

        private void checkBoxOutsideSebEnableShutDown_CheckedChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageOutsideSebEnableShutDown] = checkBoxOutsideSebEnableShutDown.Checked;
        }

        private void checkBoxOutsideSebEnableEaseOfAccess_CheckedChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageOutsideSebEnableEaseOfAccess] = checkBoxOutsideSebEnableEaseOfAccess.Checked;
        }

        private void checkBoxOutsideSebEnableVmWareClientShade_CheckedChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageOutsideSebEnableVmWareClientShade] = checkBoxOutsideSebEnableVmWareClientShade.Checked;
        }



        // *******************
        // Group "Hooked Keys"
        // *******************
        private void checkBoxHookKeys_CheckedChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageHookKeys] = checkBoxHookKeys.Checked;
        }



        // ********************
        // Group "Special Keys"
        // ********************
        private void checkBoxEnableEsc_CheckedChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageEnableEsc] = checkBoxEnableEsc.Checked;
        }

        private void checkBoxEnableCtrlEsc_CheckedChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageEnableCtrlEsc] = checkBoxEnableCtrlEsc.Checked;
        }

        private void checkBoxEnableAltEsc_CheckedChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageEnableAltEsc] = checkBoxEnableAltEsc.Checked;
        }

        private void checkBoxEnableAltTab_CheckedChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageEnableAltTab] = checkBoxEnableAltTab.Checked;
        }

        private void checkBoxEnableAltF4_CheckedChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageEnableAltF4] = checkBoxEnableAltF4.Checked;
        }

        private void checkBoxEnableStartMenu_CheckedChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageEnableStartMenu] = checkBoxEnableStartMenu.Checked;
        }

        private void checkBoxEnableRightMouse_CheckedChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageEnableRightMouse] = checkBoxEnableRightMouse.Checked;
        }



        // *********************
        // Group "Function Keys"
        // *********************
        private void checkBoxEnableF1_CheckedChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageEnableF1] = checkBoxEnableF1.Checked;
        }

        private void checkBoxEnableF2_CheckedChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageEnableF2] = checkBoxEnableF2.Checked;
        }

        private void checkBoxEnableF3_CheckedChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageEnableF3] = checkBoxEnableF3.Checked;
        }

        private void checkBoxEnableF4_CheckedChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageEnableF4] = checkBoxEnableF4.Checked;
        }

        private void checkBoxEnableF5_CheckedChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageEnableF5] = checkBoxEnableF5.Checked;
        }

        private void checkBoxEnableF6_CheckedChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageEnableF6] = checkBoxEnableF6.Checked;
        }

        private void checkBoxEnableF7_CheckedChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageEnableF7] = checkBoxEnableF7.Checked;
        }

        private void checkBoxEnableF8_CheckedChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageEnableF8] = checkBoxEnableF8.Checked;
        }

        private void checkBoxEnableF9_CheckedChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageEnableF9] = checkBoxEnableF9.Checked;
        }

        private void checkBoxEnableF10_CheckedChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageEnableF10] = checkBoxEnableF10.Checked;
        }

        private void checkBoxEnableF11_CheckedChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageEnableF11] = checkBoxEnableF11.Checked;
        }

        private void checkBoxEnableF12_CheckedChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageEnableF12] = checkBoxEnableF12.Checked;
        }



        // ********************
        // Copy settings arrays
        // ********************
        private void CopySettingsArrays(int StateSource, int StateTarget)
        {
            // Copy all settings from one array to another
            int group, value;

            for (group = 1; group <= GroupNum; group++)
            for (value = 1; value <= ValueNum; value++)
            {
                settingBoolean[StateTarget, group, value] = settingBoolean[StateSource, group, value];
                settingString [StateTarget, group, value] = settingString [StateSource, group, value];
                settingInteger[StateTarget, group, value] = settingInteger[StateSource, group, value];
            }

            return;
        }



        // ************************
        // Copy settings dictionary
        // ************************
        private void CopySettingsDictionary(Dictionary<string, object> sebSettingsSource,
                                            Dictionary<string, object> sebSettingsTarget)
        {
            // Copy all settings from one dictionary to another
            // Create a dictionary "target settings".
            // Copy source settings to target settings
            foreach (KeyValuePair<string, object> pair in sebSettingsSource)
            {
                string key   = pair.Key;
                object value = pair.Value;

//                if (key.GetType == Type.Dictionary)
//                    CopySettingsDictionary(sebSettingsSource, sebSettingsTarget, keyNode);

                if  (sebSettingsTarget.ContainsKey(key))
                     sebSettingsTarget[key] = value;
                else sebSettingsTarget.Add(key, value);
            }


            //Dictionary<string, object>.Enumerator enumerator;
            //enumerator = sebSettingsSource.GetEnumerator();
/*
            var dict = new Dictionary<string, string>();
            dict.Add("SO", "StackOverflow");
            var secondDict = new Dictionary<string, string>(dict);
            dict = null;
            Console.WriteLine(secondDict["SO"]);
*/
            return;
        }



        // *************************
        // Print settings dictionary
        // *************************
        private void PrintSettingsDictionary(Dictionary<string, object> sebSettings,
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



        // *****************************************************
        // Set the widgets to the new settings of SebStarter.ini
        // *****************************************************
        private void SetWidgetsToNewSettings()
        {
            // Update the filename in the title bar
            this.Text  = this.ProductName;
            this.Text += " - ";
            this.Text += currentPathSebConfigFile;

            // Update the widgets

            List<object> permittedProcessList = null;
            Dictionary<string, object>   dict = null;

            permittedProcessList = (List<object>) sebSettingsNew[MessagePermittedProcesses];

            for (int index = 0; index < permittedProcessList.Count; index++)
            {
                ListViewItem processRow;

                dict = (Dictionary<string, object>) permittedProcessList[index];

                Boolean activeBoolean = (Boolean) dict[MessageActive];
                Int32       osInteger = (Int32)   dict[MessageOS];

                String  activeString  = activeBoolean.ToString();
                String      osString  = StringOperatingSystem[osInteger];

                //processRow = new ListViewItem(dict[MessageActive].ToString());
                //processRow.SubItems.Add(StringOperatingSystem[(Int32) dict[MessageOS]]);

                processRow = new ListViewItem(activeString);
                processRow.SubItems.Add(osString);

                processRow.SubItems.Add((String) dict[MessageExecutable]);
                processRow.SubItems.Add((String) dict[MessageAppTitle]);

                listViewPermittedProcesses.Items.Add(processRow);
            }



            // Group "General"
            textBoxStartURL            .Text   =  (String)sebSettingsNew[MessageStartURL];
            textBoxSebServerURL        .Text   =  (String)sebSettingsNew[MessageSebServerURL];
          //textBoxAdminPassword       .Text   =  (String)sebSettingsNew[MessageAdminPassword];
          //textBoxConfirmAdminPassword.Text   =  (String)sebSettingsNew[MessageConfirmAdminPassword];
            textBoxHashedAdminPassword .Text   =  (String)sebSettingsNew[MessageHashedAdminPassword];
            checkBoxAllowQuit         .Checked = (Boolean)sebSettingsNew[MessageAllowQuit];
            checkBoxIgnoreQuitPassword.Checked = (Boolean)sebSettingsNew[MessageIgnoreQuitPassword];
          //textBoxQuitPassword        .Text   =  (String)sebSettingsNew[MessageQuitPassword];
          //textBoxConfirmQuitPassword .Text   =  (String)sebSettingsNew[MessageConfirmQuitPassword];
            textBoxHashedQuitPassword  .Text   =  (String)sebSettingsNew[MessageHashedQuitPassword];
            listBoxExitKey1.SelectedIndex      =     (int)sebSettingsNew[MessageExitKey1];
            listBoxExitKey2.SelectedIndex      =     (int)sebSettingsNew[MessageExitKey2];
            listBoxExitKey3.SelectedIndex      =     (int)sebSettingsNew[MessageExitKey3];

            // Group "Config File"
            radioButtonStartingAnExam     .Checked =    ((int)sebSettingsNew[MessageSebConfigPurpose] == 0);
            radioButtonConfiguringAClient .Checked =    ((int)sebSettingsNew[MessageSebConfigPurpose] == 1);
            checkBoxAllowPreferencesWindow.Checked = (Boolean)sebSettingsNew[MessageAllowPreferencesWindow];
            comboBoxCryptoIdentity.SelectedIndex   =   settingInteger[StateNew, GroupConfigFile, ValueCryptoIdentity];
             textBoxSettingsPassword       .Text   =  (String)sebSettingsNew[MessageSettingsPassword];
           //textBoxConfirmSettingsPassword.Text   =  (String)sebSettingsNew[MessageConfirmSettingsPassword];
           //textBoxHashedSettingsPassword .Text   =  (String)sebSettingsNew[MessageHashedSettingsPassword];

            // Group "Appearance"
            radioButtonUseBrowserWindow       .Checked =    ((int)sebSettingsNew[MessageBrowserViewMode] == 0);
            radioButtonUseFullScreenMode      .Checked =    ((int)sebSettingsNew[MessageBrowserViewMode] == 1);
            comboBoxMainBrowserWindowWidth    .Text    =  (String)sebSettingsNew[MessageMainBrowserWindowWidth];
            comboBoxMainBrowserWindowHeight   .Text    =  (String)sebSettingsNew[MessageMainBrowserWindowHeight];
             listBoxMainBrowserWindowPositioning.SelectedIndex = (int)sebSettingsNew[MessageMainBrowserWindowPositioning];
            checkBoxEnableBrowserWindowToolbar.Checked = (Boolean)sebSettingsNew[MessageEnableBrowserWindowToolbar];
            checkBoxHideBrowserWindowToolbar  .Checked = (Boolean)sebSettingsNew[MessageHideBrowserWindowToolbar];
            checkBoxShowMenuBar               .Checked = (Boolean)sebSettingsNew[MessageShowMenuBar];
            checkBoxShowTaskBar               .Checked = (Boolean)sebSettingsNew[MessageShowTaskBar];

            // Group "Browser"
             listBoxOpenLinksHTML .SelectedIndex =     (int)sebSettingsNew[MessageNewBrowserWindowByLinkPolicy];
             listBoxOpenLinksJava .SelectedIndex =     (int)sebSettingsNew[MessageNewBrowserWindowByScriptPolicy];
            checkBoxBlockLinksHTML.Checked       = (Boolean)sebSettingsNew[MessageNewBrowserWindowByLinkBlockForeign];
            checkBoxBlockLinksJava.Checked       = (Boolean)sebSettingsNew[MessageNewBrowserWindowByScriptBlockForeign];

            comboBoxNewBrowserWindowWidth      .Text          = (String)sebSettingsNew[MessageNewBrowserWindowByLinkWidth ];
            comboBoxNewBrowserWindowHeight     .Text          = (String)sebSettingsNew[MessageNewBrowserWindowByLinkHeight];
             listBoxNewBrowserWindowPositioning.SelectedIndex =    (int)sebSettingsNew[MessageNewBrowserWindowByLinkPositioning];

            checkBoxEnablePlugIns           .Checked =   (Boolean)sebSettingsNew[MessageEnablePlugIns];
            checkBoxEnableJava              .Checked =   (Boolean)sebSettingsNew[MessageEnableJava];
            checkBoxEnableJavaScript        .Checked =   (Boolean)sebSettingsNew[MessageEnableJavaScript];
            checkBoxBlockPopUpWindows       .Checked =   (Boolean)sebSettingsNew[MessageBlockPopUpWindows];
            checkBoxAllowBrowsingBackForward.Checked =   (Boolean)sebSettingsNew[MessageAllowBrowsingBackForward];
            checkBoxUseSebWithoutBrowser    .Checked = !((Boolean)sebSettingsNew[MessageEnableSebBrowser]);
            // BEWARE: you must invert this value since "Use Without" is "Not Enable"!

            // Group "Down/Uploads"
            checkBoxAllowDownUploads.Checked = (Boolean)sebSettingsNew[MessageAllowDownUploads];
            checkBoxOpenDownloads   .Checked = (Boolean)sebSettingsNew[MessageOpenDownloads];
            checkBoxDownloadPDFFiles.Checked = (Boolean)sebSettingsNew[MessageDownloadPDFFiles];
            labelDownloadDirectoryWin.Text   =  (String)sebSettingsNew[MessageDownloadDirectoryWin];
             listBoxChooseFileToUploadPolicy.SelectedIndex = (int)sebSettingsNew[MessageChooseFileToUploadPolicy];

            // Group "Exam"
           //textBoxBrowserExamKey    .Text    =  (String)sebSettingsNew[MessageBrowserExamKey];
             textBoxQuitURL           .Text    =  (String)sebSettingsNew[MessageQuitURL];
            checkBoxCopyBrowserExamKey.Checked = (Boolean)sebSettingsNew[MessageCopyBrowserExamKey];
            checkBoxSendBrowserExamKey.Checked = (Boolean)sebSettingsNew[MessageSendBrowserExamKey];

            // Group "Applications"
            checkBoxMonitorProcesses         .Checked = (Boolean)sebSettingsNew[MessageMonitorProcesses];
            checkBoxAllowSwitchToApplications.Checked = (Boolean)sebSettingsNew[MessageAllowSwitchToApplications];
            checkBoxAllowFlashFullscreen     .Checked = (Boolean)sebSettingsNew[MessageAllowFlashFullscreen];

            // Group "Network"
            //checkBoxEnableURLFilter       .Checked = (Boolean)sebSettingsNew[MessageEnableURLFilter];
            //checkBoxEnableURLContentFilter.Checked = (Boolean)sebSettingsNew[MessageEnableURLContentFilter];

            //radioButtonUseSystemProxySettings.Checked = ((int)sebSettingsNew[MessageProxySettingsPolicy] == 0);
            //radioButtonUseSebProxySettings   .Checked = ((int)sebSettingsNew[MessageProxySettingsPolicy] == 1);

            // Group "Security"
             listBoxSebServicePolicy.SelectedIndex =     (int)sebSettingsNew[MessageSebServicePolicy];
            checkBoxAllowVirtualMachine.Checked    = (Boolean)sebSettingsNew[MessageAllowVirtualMachine];
            checkBoxCreateNewDesktop   .Checked    = (Boolean)sebSettingsNew[MessageCreateNewDesktop];
            checkBoxAllowUserSwitching .Checked    = (Boolean)sebSettingsNew[MessageAllowUserSwitching];
            checkBoxEnableLogging      .Checked    = (Boolean)sebSettingsNew[MessageEnableLogging];
            labelLogDirectoryWin       .Text       =  (String)sebSettingsNew[MessageLogDirectoryWin];

            // Group "Registry"
            checkBoxInsideSebEnableSwitchUser       .Checked = (Boolean)sebSettingsNew[MessageInsideSebEnableSwitchUser];
            checkBoxInsideSebEnableLockThisComputer .Checked = (Boolean)sebSettingsNew[MessageInsideSebEnableLockThisComputer];
            checkBoxInsideSebEnableChangeAPassword  .Checked = (Boolean)sebSettingsNew[MessageInsideSebEnableChangeAPassword];
            checkBoxInsideSebEnableStartTaskManager .Checked = (Boolean)sebSettingsNew[MessageInsideSebEnableStartTaskManager];
            checkBoxInsideSebEnableLogOff           .Checked = (Boolean)sebSettingsNew[MessageInsideSebEnableLogOff];
            checkBoxInsideSebEnableShutDown         .Checked = (Boolean)sebSettingsNew[MessageInsideSebEnableShutDown];
            checkBoxInsideSebEnableEaseOfAccess     .Checked = (Boolean)sebSettingsNew[MessageInsideSebEnableEaseOfAccess];
            checkBoxInsideSebEnableVmWareClientShade.Checked = (Boolean)sebSettingsNew[MessageInsideSebEnableVmWareClientShade];

            checkBoxOutsideSebEnableSwitchUser       .Checked = (Boolean)sebSettingsNew[MessageOutsideSebEnableSwitchUser];
            checkBoxOutsideSebEnableLockThisComputer .Checked = (Boolean)sebSettingsNew[MessageOutsideSebEnableLockThisComputer];
            checkBoxOutsideSebEnableChangeAPassword  .Checked = (Boolean)sebSettingsNew[MessageOutsideSebEnableChangeAPassword];
            checkBoxOutsideSebEnableStartTaskManager .Checked = (Boolean)sebSettingsNew[MessageOutsideSebEnableStartTaskManager];
            checkBoxOutsideSebEnableLogOff           .Checked = (Boolean)sebSettingsNew[MessageOutsideSebEnableLogOff];
            checkBoxOutsideSebEnableShutDown         .Checked = (Boolean)sebSettingsNew[MessageOutsideSebEnableShutDown];
            checkBoxOutsideSebEnableEaseOfAccess     .Checked = (Boolean)sebSettingsNew[MessageOutsideSebEnableEaseOfAccess];
            checkBoxOutsideSebEnableVmWareClientShade.Checked = (Boolean)sebSettingsNew[MessageOutsideSebEnableVmWareClientShade];

            // Group "Hooked Keys"
            checkBoxHookKeys.Checked = (Boolean)sebSettingsNew[MessageHookKeys];

            checkBoxEnableEsc       .Checked = (Boolean)sebSettingsNew[MessageEnableEsc];
            checkBoxEnableCtrlEsc   .Checked = (Boolean)sebSettingsNew[MessageEnableCtrlEsc];
            checkBoxEnableAltEsc    .Checked = (Boolean)sebSettingsNew[MessageEnableAltEsc];
            checkBoxEnableAltTab    .Checked = (Boolean)sebSettingsNew[MessageEnableAltTab];
            checkBoxEnableAltF4     .Checked = (Boolean)sebSettingsNew[MessageEnableAltF4];
            checkBoxEnableStartMenu .Checked = (Boolean)sebSettingsNew[MessageEnableStartMenu];
            checkBoxEnableRightMouse.Checked = (Boolean)sebSettingsNew[MessageEnableRightMouse];

            checkBoxEnableF1 .Checked = (Boolean)sebSettingsNew[MessageEnableF1];
            checkBoxEnableF2 .Checked = (Boolean)sebSettingsNew[MessageEnableF2];
            checkBoxEnableF3 .Checked = (Boolean)sebSettingsNew[MessageEnableF3];
            checkBoxEnableF4 .Checked = (Boolean)sebSettingsNew[MessageEnableF4];
            checkBoxEnableF5 .Checked = (Boolean)sebSettingsNew[MessageEnableF5];
            checkBoxEnableF6 .Checked = (Boolean)sebSettingsNew[MessageEnableF6];
            checkBoxEnableF7 .Checked = (Boolean)sebSettingsNew[MessageEnableF7];
            checkBoxEnableF8 .Checked = (Boolean)sebSettingsNew[MessageEnableF8];
            checkBoxEnableF9 .Checked = (Boolean)sebSettingsNew[MessageEnableF9];
            checkBoxEnableF10.Checked = (Boolean)sebSettingsNew[MessageEnableF10];
            checkBoxEnableF11.Checked = (Boolean)sebSettingsNew[MessageEnableF11];
            checkBoxEnableF12.Checked = (Boolean)sebSettingsNew[MessageEnableF12];
        }



    } // end of   class     SebWindowsConfigForm
}     // end of   namespace SebWindowsConfig
