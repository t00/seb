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

        // The target files the user must configure,
        // because these are used by the application SebStarter.exe
        const String TargetSebStarterIni = "SebClient.ini";
        const String TargetSebStarterXml = "SebClient.xml";
        const String TargetSebStarterSeb = "SebClient.seb";

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
        const int NumValueGeneral = 13;

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

        // Group "Config File"
        const int ValueSebPurpose              = 1;
        const int ValueAllowPreferencesWindow  = 2;
        const int ValueCryptoIdentity          = 3;
        const int ValueSettingsPassword        = 4;
        const int ValueConfirmSettingsPassword = 5;
        const int ValueHashedSettingsPassword  = 6;
        const int NumValueConfigFile = 6;

        const String MessageSebPurpose              = "sebPurpose";
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
        const int ValueEnablePlugins             = 8;
        const int ValueEnableJava                = 9;
        const int ValueEnableJavaScript          = 10;
        const int ValueBlockPopupWindows         = 11;
        const int ValueAllowBrowsingBackForward  = 12;
        const int ValueEnableSebBrowser          = 13;
        const int NumValueBrowser = 13;

        const String MessageNewBrowserWindowByLinkPolicy         = "newBrowserWindowByLinkPolicy";
        const String MessageNewBrowserWindowByScriptPolicy       = "newBrowserWindowByScriptPolicy";
        const String MessageNewBrowserWindowByLinkBlockForeign   = "newBrowserWindowByLinkBlockForeign";
        const String MessageNewBrowserWindowByScriptBlockForeign = "newBrowserWindowByScriptBlockForeign";
        const String MessageNewBrowserWindowByLinkWidth          = "newBrowserWindowByLinkWidth";
        const String MessageNewBrowserWindowByLinkHeight         = "newBrowserWindowByLinkHeight";
        const String MessageNewBrowserWindowByLinkPositioning    = "newBrowserWindowByLinkPositioning";
        const String MessageEnablePlugins            = "enablePlugins";
        const String MessageEnableJava               = "enableJava";
        const String MessageEnableJavaScript         = "enableJavaScript";
        const String MessageBlockPopupWindows        = "blockPopupWindows";
        const String MessageAllowBrowsingBackForward = "allowBrowsingBackForward";
        const String MessageEnableSebBrowser         = "enableSebBrowser";

        // Group "DownUploads"
        const int ValueAllowDownUploads         = 1;
        const int ValueDownloadDirectoryWin     = 2;
        const int ValueDownloadDirectoryOSX     = 3;
        const int ValueOpenDownloads            = 4;
        const int ValueChooseFileToUploadPolicy = 5;
        const int ValueDownloadPDFFiles         = 6;
        const int NumValueDownUploads = 6;

        const String MessageAllowDownUploads         = "allowDownUploads";
        const String MessageDownloadDirectoryWin     = "downloadDirectoryWin";
        const String MessageDownloadDirectoryOSX     = "downloadDirectoryOSX";
        const String MessageOpenDownloads            = "openDownloads";
        const String MessageChooseFileToUploadPolicy = "chooseFileToUploadPolicy";
        const String MessageDownloadPDFFiles         = "downloadPDFFiles";

        // Group "Exam"
        const int ValueBrowserExamKey     = 1;
        const int ValueCopyBrowserExamKey = 2;
        const int ValueSendBrowserExamKey = 3;
        const int ValueQuitURL            = 4;
        const int NumValueExam = 4;

        const String MessageBrowserExamKey     = "browserExamKey";
        const String MessageCopyBrowserExamKey = "copyBrowserExamKeyToClipboardWhenQuitting";
        const String MessageSendBrowserExamKey = "sendBrowserExamKey";
        const String MessageQuitURL            = "quitURL";

        // Group "Applications"
        const int ValueMonitorProcesses          = 1;
        const int ValueAllowSwitchToApplications = 2;
        const int ValueAllowFlashFullscreen      = 3;
        const int NumValueApplications = 3;

        const String MessageMonitorProcesses          = "monitorProcesses";
        const String MessageAllowSwitchToApplications = "allowSwitchToApplications";
        const String MessageAllowFlashFullscreen      = "allowFlashFullscreen";

        // Group "Network"
        //const int Value = 1;
        const int NumValueNetwork = 0;

        // Group "Security"
        const int ValueSebServicePolicy    = 1;
        const int ValueAllowVirtualMachine = 2;
        const int ValueCreateNewDesktop    = 3;
        const int ValueAllowUserSwitching  = 4;
        const int ValueEnableLog           = 5;
        const int NumValueSecurity = 5;

        const String MessageSebServicePolicy    = "sebServicePolicy";
        const String MessageAllowVirtualMachine = "allowVirtualMachine";
        const String MessageCreateNewDesktop    = "createNewDesktop";
        const String MessageAllowUserSwitching  = "allowUserSwitching";
        const String MessageEnableLog           = "enableLog";

        // Group "Registry"
        const int NumValueRegistry = 0;

        // Groups "Inside SEB" and "Outside SEB"
        const int ValueEnableSwitchUser        = 1;
        const int ValueEnableLockThisComputer  = 2;
        const int ValueEnableChangeAPassword   = 3;
        const int ValueEnableStartTaskManager  = 4;
        const int ValueEnableLogOff            = 5;
        const int ValueEnableShutDown          = 6;
        const int ValueEnableEaseOfAccess      = 7;
        const int ValueEnableVmWareClientShade = 8;
        const int NumValueInsideSeb  = 8;
        const int NumValueOutsideSeb = 8;

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

        // Group "HookedKeys"
        const int ValueHookMessages = 1;
        const int NumValueHookedKeys = 1;

        const String MessageHookMessages = "hookMessages";

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

        // The ini file currently being modified
        String currentDireSebStarterIni;
        String currentFileSebStarterIni;
        String currentPathSebStarterIni;

        // The target file the user must configure,
        // because this is used by the application SebStarter.exe
        String targetDireSebStarterIni;
        String targetFileSebStarterIni;
        String targetPathSebStarterIni;

        // Strings for encryption identities (KeyChain, Certificate Store)
        //static ArrayList chooseIdentityStringArrayList = new ArrayList();
        //static String[]  chooseIdentityStringArray = new String[1];
        static List<String> StringCryptoIdentity = new List<String>();

        // Entries of ListBoxes
        static String[] StringSebPurpose        = new String[2];
        static String[] StringBrowserViewMode   = new String[2];
        static String[] StringWindowWidth       = new String[4];
        static String[] StringWindowHeight      = new String[4];
        static String[] StringWindowPositioning = new String[3];
        static String[] StringPolicyLinkOpening = new String[3];
        static String[] StringPolicyFileUpload  = new String[3];
        static String[] StringPolicySebService  = new String[3];
        static String[] StringFunctionKey       = new String[12];

        // Number of values per group
        // Names  of groups and values
        // Types  of values (Boolean, Integer, String)
        static    int[ ]         minValue  = new    int[GroupNum + 1];
        static    int[ ]         maxValue  = new    int[GroupNum + 1];
        static String[ ]      configString = new String[ FileNum + 1];
        static String[ ]       groupString = new String[GroupNum + 1];
        static String[,]       valueString = new String[GroupNum + 1, ValueNum + 1];
        static    int[,]        dataType   = new    int[GroupNum + 1, ValueNum + 1];

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
        static SEBClientConfig            sebSettObso    = new SEBClientConfig();
        static SEBProtectionController    sebController  = new SEBProtectionController();
        static XmlSerializer              sebSerializer  = new XmlSerializer(typeof(SEBClientConfig));



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
                settingString [state, group, value] = "";
                settingInteger[state, group, value] = 0;
            }

            // Default settings for group "General"
            settingString [StateDef, GroupGeneral, ValueStartURL            ] = "http://www.safeexambrowser.org";
            settingString [StateDef, GroupGeneral, ValueSebServerURL        ] = "http://www.switch.ch";
            settingString [StateDef, GroupGeneral, ValueAdminPassword       ] = "";
            settingString [StateDef, GroupGeneral, ValueConfirmAdminPassword] = "";
            settingBoolean[StateDef, GroupGeneral, ValueAllowQuit           ] = true;
            settingBoolean[StateDef, GroupGeneral, ValueIgnoreQuitPassword  ] = false;
            settingString [StateDef, GroupGeneral, ValueQuitPassword        ] = "";
            settingString [StateDef, GroupGeneral, ValueConfirmQuitPassword ] = "";
            settingString [StateDef, GroupGeneral, ValueHashedQuitPassword  ] = "";
            settingInteger[StateDef, GroupGeneral, ValueExitKey1            ] =  2;
            settingInteger[StateDef, GroupGeneral, ValueExitKey2            ] = 10;
            settingInteger[StateDef, GroupGeneral, ValueExitKey3            ] =  5;

            // Default settings for group "Config File"
            settingInteger[StateDef, GroupConfigFile, ValueSebPurpose             ] = 0;
            settingBoolean[StateDef, GroupConfigFile, ValueAllowPreferencesWindow ] = true;
            settingInteger[StateDef, GroupConfigFile, ValueCryptoIdentity         ] = 0;
            settingString [StateDef, GroupConfigFile, ValueSettingsPassword       ] = "";
            settingString [StateDef, GroupConfigFile, ValueConfirmSettingsPassword] = "";

            // Default settings for group "Appearance"
            settingInteger[StateDef, GroupAppearance, ValueBrowserViewMode             ] = 0;
            settingInteger[StateDef, GroupAppearance, ValueMainBrowserWindowWidth      ] = 0;
            settingInteger[StateDef, GroupAppearance, ValueMainBrowserWindowHeight     ] = 0;
            settingInteger[StateDef, GroupAppearance, ValueMainBrowserWindowPositioning] = 1;
            settingBoolean[StateDef, GroupAppearance, ValueEnableBrowserWindowToolbar  ] = true;
            settingBoolean[StateDef, GroupAppearance, ValueHideBrowserWindowToolbar    ] = false;
            settingBoolean[StateDef, GroupAppearance, ValueShowMenuBar                 ] = false;
            settingBoolean[StateDef, GroupAppearance, ValueShowTaskBar                 ] = false;

            // Default settings for group "Browser"
            settingInteger[StateDef, GroupBrowser, ValueNewBrowserWindowByLinkPolicy        ] = 2;
            settingInteger[StateDef, GroupBrowser, ValueNewBrowserWindowByScriptPolicy      ] = 2;
            settingBoolean[StateDef, GroupBrowser, ValueNewBrowserWindowByLinkBlockForeign  ] = false;
            settingBoolean[StateDef, GroupBrowser, ValueNewBrowserWindowByScriptBlockForeign] = false;
            settingInteger[StateDef, GroupBrowser, ValueNewBrowserWindowByLinkWidth         ] = 0;
            settingInteger[StateDef, GroupBrowser, ValueNewBrowserWindowByLinkHeight        ] = 0;
            settingInteger[StateDef, GroupBrowser, ValueNewBrowserWindowByLinkPositioning   ] = 2;

            settingBoolean[StateDef, GroupBrowser, ValueEnablePlugins            ] = true;
            settingBoolean[StateDef, GroupBrowser, ValueEnableJava               ] = false;
            settingBoolean[StateDef, GroupBrowser, ValueEnableJavaScript         ] = true;
            settingBoolean[StateDef, GroupBrowser, ValueBlockPopupWindows        ] = false;
            settingBoolean[StateDef, GroupBrowser, ValueAllowBrowsingBackForward] = false;
            settingBoolean[StateDef, GroupBrowser, ValueEnableSebBrowser         ] = true;

            // Default settings for group "DownUploads"
            settingBoolean[StateDef, GroupDownUploads, ValueAllowDownUploads        ] = true;
            settingString [StateDef, GroupDownUploads, ValueDownloadDirectoryWin    ] = "Desktop";
            settingString [StateDef, GroupDownUploads, ValueDownloadDirectoryOSX    ] = "~/Downloads";
            settingBoolean[StateDef, GroupDownUploads, ValueOpenDownloads           ] = true;
            settingInteger[StateDef, GroupDownUploads, ValueChooseFileToUploadPolicy] = 0;
            settingBoolean[StateDef, GroupDownUploads, ValueDownloadPDFFiles        ] = false;

            // Default settings for group "Exam"
            settingString [StateDef, GroupExam, ValueBrowserExamKey    ] = "";
            settingBoolean[StateDef, GroupExam, ValueCopyBrowserExamKey] = false;
            settingBoolean[StateDef, GroupExam, ValueSendBrowserExamKey] = true;
            settingString [StateDef, GroupExam, ValueQuitURL           ] = "http://www.safeexambrowser.org/exit";

            // Default settings for group "Applications"
            settingBoolean[StateDef, GroupApplications, ValueMonitorProcesses         ] = true;
            settingBoolean[StateDef, GroupApplications, ValueAllowSwitchToApplications] = true;
            settingBoolean[StateDef, GroupApplications, ValueAllowFlashFullscreen     ] = false;

            // Default settings for group "Network"

            // Default settings for group "Security"
            settingInteger[StateDef, GroupSecurity, ValueSebServicePolicy   ] = 2;
            settingBoolean[StateDef, GroupSecurity, ValueAllowVirtualMachine] = false;
            settingBoolean[StateDef, GroupSecurity, ValueCreateNewDesktop   ] = true;
            settingBoolean[StateDef, GroupSecurity, ValueAllowUserSwitching ] = true;
            settingBoolean[StateDef, GroupSecurity, ValueEnableLog          ] = true;

            // Default settings for group "Hooked Keys"
            settingBoolean[StateDef, GroupHookedKeys, ValueHookMessages] = true;

            // Default settings for groups "Inside SEB", "Outside SEB"
            // Default settings for groups "Special Keys", "Function Keys"
            for (value = 1; value <= ValueNum; value++)
            {
                settingBoolean[StateDef, GroupInsideSeb   , value] = false;
                settingBoolean[StateDef, GroupOutsideSeb  , value] = true;
                settingBoolean[StateDef, GroupSpecialKeys , value] = false;
                settingBoolean[StateDef, GroupFunctionKeys, value] = false;
            }

            // Default settings for groups "Special Keys"
            settingBoolean[StateDef, GroupSpecialKeys , ValueEnableAltTab] = true;

            // Default settings for groups "Function Keys"
            settingBoolean[StateDef, GroupFunctionKeys, ValueEnableF5] = true;

/*
            // Default settings for group "Online exam"
            String s0 = "Seb,../xulrunner/xulrunner.exe";
            String s1 = " -app \"..\\xul_seb\\seb.ini\"";
            String s2 = " -profile \"%LOCALAPPDATA%\\ETH_Zuerich\\xul_seb\\Profiles\"";
            String SebBrowserString = s0 + s1 + s2;

            settingString[StateDef, GroupOnlineExam, ValueSebBrowser           ] =  SebBrowserString;
            settingString[StateDef, GroupOnlineExam, ValueAutostartProcess     ] = "Seb";
            settingString[StateDef, GroupOnlineExam, ValuePermittedApplications] = "Calculator,calc.exe;Notepad,notepad.exe;";
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

            dataType[GroupAppearance, ValueMainBrowserWindowWidth      ] = TypeString;
            dataType[GroupAppearance, ValueMainBrowserWindowHeight     ] = TypeString;
            dataType[GroupAppearance, ValueMainBrowserWindowPositioning] = TypeString;

            dataType[GroupBrowser, ValueNewBrowserWindowByLinkWidth      ] = TypeString;
            dataType[GroupBrowser, ValueNewBrowserWindowByLinkHeight     ] = TypeString;
            dataType[GroupBrowser, ValueNewBrowserWindowByLinkPositioning] = TypeString;

            dataType[GroupBrowser, ValueNewBrowserWindowByLinkPolicy  ] = TypeString;
            dataType[GroupBrowser, ValueNewBrowserWindowByScriptPolicy] = TypeString;

            dataType[GroupDownUploads, ValueDownloadDirectoryWin    ] = TypeString;
            dataType[GroupDownUploads, ValueChooseFileToUploadPolicy] = TypeString;

            dataType[GroupExam, ValueBrowserExamKey] = TypeString;
            dataType[GroupExam, ValueQuitURL       ] = TypeString;

            dataType[GroupSecurity, ValueSebServicePolicy] = TypeString;


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
            configString[FileSebStarterIni] = TargetSebStarterIni;
            configString[FileSebStarterXml] = TargetSebStarterXml;
            configString[FileSebStarterSeb] = TargetSebStarterSeb;

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

            valueString[GroupConfigFile, ValueSebPurpose             ] = MessageSebPurpose;
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
            valueString[GroupBrowser, ValueEnablePlugins            ] = MessageEnablePlugins;
            valueString[GroupBrowser, ValueEnableJava               ] = MessageEnableJava;
            valueString[GroupBrowser, ValueEnableJavaScript         ] = MessageEnableJavaScript;
            valueString[GroupBrowser, ValueBlockPopupWindows        ] = MessageBlockPopupWindows;
            valueString[GroupBrowser, ValueAllowBrowsingBackForward] = MessageAllowBrowsingBackForward;
            valueString[GroupBrowser, ValueEnableSebBrowser         ] = MessageEnableSebBrowser;

            valueString[GroupDownUploads, ValueAllowDownUploads        ] = MessageAllowDownUploads;
            valueString[GroupDownUploads, ValueDownloadDirectoryWin    ] = MessageDownloadDirectoryWin;
            valueString[GroupDownUploads, ValueOpenDownloads           ] = MessageOpenDownloads;
            valueString[GroupDownUploads, ValueChooseFileToUploadPolicy] = MessageChooseFileToUploadPolicy;
            valueString[GroupDownUploads, ValueDownloadPDFFiles        ] = MessageDownloadPDFFiles;

            valueString[GroupExam, ValueBrowserExamKey    ] = MessageBrowserExamKey;
            valueString[GroupExam, ValueCopyBrowserExamKey] = MessageCopyBrowserExamKey;
            valueString[GroupExam, ValueSendBrowserExamKey] = MessageSendBrowserExamKey;
            valueString[GroupExam, ValueQuitURL           ] = MessageQuitURL;

            valueString[GroupApplications, ValueMonitorProcesses         ] = MessageMonitorProcesses;
            valueString[GroupApplications, ValueAllowSwitchToApplications] = MessageAllowSwitchToApplications;
            valueString[GroupApplications, ValueAllowFlashFullscreen     ] = MessageAllowFlashFullscreen;

            valueString[GroupSecurity, ValueSebServicePolicy   ] = MessageSebServicePolicy;
            valueString[GroupSecurity, ValueAllowVirtualMachine] = MessageAllowVirtualMachine;
            valueString[GroupSecurity, ValueCreateNewDesktop   ] = MessageCreateNewDesktop;
            valueString[GroupSecurity, ValueAllowUserSwitching ] = MessageAllowUserSwitching;
            valueString[GroupSecurity, ValueEnableLog          ] = MessageEnableLog;

            valueString[GroupInsideSeb, ValueEnableSwitchUser       ] = MessageInsideSebEnableSwitchUser;
            valueString[GroupInsideSeb, ValueEnableLockThisComputer ] = MessageInsideSebEnableLockThisComputer;
            valueString[GroupInsideSeb, ValueEnableChangeAPassword  ] = MessageInsideSebEnableChangeAPassword;
            valueString[GroupInsideSeb, ValueEnableStartTaskManager ] = MessageInsideSebEnableStartTaskManager;
            valueString[GroupInsideSeb, ValueEnableLogOff           ] = MessageInsideSebEnableLogOff;
            valueString[GroupInsideSeb, ValueEnableShutDown         ] = MessageInsideSebEnableShutDown;
            valueString[GroupInsideSeb, ValueEnableEaseOfAccess     ] = MessageInsideSebEnableEaseOfAccess;
            valueString[GroupInsideSeb, ValueEnableVmWareClientShade] = MessageInsideSebEnableVmWareClientShade;

            valueString[GroupOutsideSeb, ValueEnableSwitchUser       ] = MessageOutsideSebEnableSwitchUser;
            valueString[GroupOutsideSeb, ValueEnableLockThisComputer ] = MessageOutsideSebEnableLockThisComputer;
            valueString[GroupOutsideSeb, ValueEnableChangeAPassword  ] = MessageOutsideSebEnableChangeAPassword;
            valueString[GroupOutsideSeb, ValueEnableStartTaskManager ] = MessageOutsideSebEnableStartTaskManager;
            valueString[GroupOutsideSeb, ValueEnableLogOff           ] = MessageOutsideSebEnableLogOff;
            valueString[GroupOutsideSeb, ValueEnableShutDown         ] = MessageOutsideSebEnableShutDown;
            valueString[GroupOutsideSeb, ValueEnableEaseOfAccess     ] = MessageOutsideSebEnableEaseOfAccess;
            valueString[GroupOutsideSeb, ValueEnableVmWareClientShade] = MessageOutsideSebEnableVmWareClientShade;

            valueString[GroupHookedKeys, ValueHookMessages] = MessageHookMessages;

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


            // Define the strings for the Encryption Identity
            StringCryptoIdentity.Add("none");
            StringCryptoIdentity.Add("alpha");
            StringCryptoIdentity.Add("beta");
            StringCryptoIdentity.Add("gamma");
            StringCryptoIdentity.Add("delta");
            String[] chooseIdentityStringArray = StringCryptoIdentity.ToArray();

            // Define the strings for the SEB purpose
            StringSebPurpose[0] = "starting an exam";
            StringSebPurpose[1] = "configuring a client";

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
            StringPolicyLinkOpening[0] = "open in new window";
            StringPolicyLinkOpening[1] = "open in same window";
            StringPolicyLinkOpening[2] = "get generally blocked";

            // Define the strings for the File Upload Policy
            StringPolicyFileUpload[0] = "manually with file requester";
            StringPolicyFileUpload[1] = "by attempting to upload the same file downloaded before";
            StringPolicyFileUpload[2] = "by only allowing to upload the same file downloaded before";

            // Define the strings for the SEB Service Policy
            StringPolicySebService[0] = "allow to use SEB only with service";
            StringPolicySebService[1] = "display warning when service is not running";
            StringPolicySebService[2] = "allow to run SEB without service";

            // Define the strings for the Function Keys F1, F2, ..., F12
            for (int i = 1; i <= 12; i++)
            {
                StringFunctionKey[i-1] = "F" + i.ToString();
            }


            // Try to open the configuration file (SebStarter.ini/xml/seb)
            // given in the local directory (where SebWindowsConfig.exe was called)
            currentDireSebStarterIni = Directory.GetCurrentDirectory();
            currentFileSebStarterIni = "";
            currentPathSebStarterIni = "";

             targetDireSebStarterIni = Directory.GetCurrentDirectory();
             targetFileSebStarterIni = TargetSebStarterIni;
             targetPathSebStarterIni = Path.GetFullPath(TargetSebStarterIni);

            String fileName = targetPathSebStarterIni;

            // Cut off the file extension ".ini", ".xml" or ".seb",
            // that is the last 4 characters of the file name
            String fileNameRaw = fileName.Substring(0, fileName.Length - 4);
            String fileNameExt = fileName.Substring(fileName.Length - 4, 4);
            String fileNameIni = fileNameRaw + ".ini";
            String fileNameXml = fileNameRaw + ".xml";
            String fileNameSeb = fileNameRaw + ".seb";

            // Read the settings from the standard configuration file
            if (fileNameExt.Equals(".ini")) OpenIniFile(fileNameIni);
            if (fileNameExt.Equals(".xml")) OpenXmlFile(fileNameXml);
            if (fileNameExt.Equals(".seb")) OpenSebFile(fileNameSeb);

            openFileDialogSebStarterIni.InitialDirectory = Environment.CurrentDirectory;
            saveFileDialogSebStarterIni.InitialDirectory = Environment.CurrentDirectory;
//          folderBrowserDialogDownloadFolder.RootFolder = Environment.SpecialFolder.DesktopDirectory;

            // Soll das hier stehen oder in SetWidgetsToNewSettingsOfSebStarterIni() ???
            comboBoxCryptoIdentity.Items.AddRange(chooseIdentityStringArray);
            comboBoxCryptoIdentity.SelectedIndex = 0;

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
            settingInteger[StateTmp, GroupConfigFile, ValueCryptoIdentity] = 0;

            // These ListBox and ComboBox entries need a conversion from string to integer:
            //
            // Exit Key Sequence (exit keys 1,2,3)
            // SEB Purpose
            // Crypto Identity
            // Browser View Mode
            // Main Window Width/Height/Positioning
            // New  Window Width/Height/Positioning
            // Link Opening Policy for HTML/JavaScript
            // SEB  Service Policy
            // Choose File To Upload

            String tmpStringExitKey1 = settingString[StateTmp, GroupGeneral, ValueExitKey1];
            String tmpStringExitKey2 = settingString[StateTmp, GroupGeneral, ValueExitKey2];
            String tmpStringExitKey3 = settingString[StateTmp, GroupGeneral, ValueExitKey3];

            String tmpStringSebPurpose     = settingString[StateTmp, GroupConfigFile, ValueSebPurpose];
          //String tmpStringCryptoIdentity = settingString[StateTmp, GroupConfigFile, ValueCryptoIdentity];

            String tmpStringBrowserViewMode       = settingString[StateTmp, GroupAppearance, ValueBrowserViewMode];
            String tmpStringMainWindowWidth       = settingString[StateTmp, GroupAppearance, ValueMainBrowserWindowWidth];
            String tmpStringMainWindowHeight      = settingString[StateTmp, GroupAppearance, ValueMainBrowserWindowHeight];
            String tmpStringMainWindowPositioning = settingString[StateTmp, GroupAppearance, ValueMainBrowserWindowPositioning];

            String tmpStringNewWindowByLinkWidth       = settingString[StateTmp, GroupBrowser, ValueNewBrowserWindowByLinkWidth];
            String tmpStringNewWindowByLinkHeight      = settingString[StateTmp, GroupBrowser, ValueNewBrowserWindowByLinkHeight];
            String tmpStringNewWindowByLinkPositioning = settingString[StateTmp, GroupBrowser, ValueNewBrowserWindowByLinkPositioning];
            String tmpStringNewWindowByLinkPolicy      = settingString[StateTmp, GroupBrowser, ValueNewBrowserWindowByLinkPolicy];
            String tmpStringNewWindowByScriptPolicy    = settingString[StateTmp, GroupBrowser, ValueNewBrowserWindowByScriptPolicy];

            String tmpStringSebServicePolicy = settingString[StateTmp, GroupSecurity   , ValueSebServicePolicy];
            String tmpStringFileUploadPolicy = settingString[StateTmp, GroupDownUploads, ValueChooseFileToUploadPolicy];

            int index;

            int tmpIndexExitKey1 = 0;
            int tmpIndexExitKey2 = 0;
            int tmpIndexExitKey3 = 0;

            int tmpIndexSebPurpose = 0;
 
            int tmpIndexBrowserViewMode       = 0;
            int tmpIndexMainWindowWidth       = 0;
            int tmpIndexMainWindowHeight      = 0;
            int tmpIndexMainWindowPositioning = 0;

            int tmpIndexNewWindowByLinkWidth       = 0;
            int tmpIndexNewWindowByLinkHeight      = 0;
            int tmpIndexNewWindowByLinkPositioning = 0;
            int tmpIndexNewWindowByLinkPolicy      = 0;
            int tmpIndexNewWindowByScriptPolicy    = 0;

            int tmpIndexSebServicePolicy   = 0;
            int tmpIndexChooseFileToUpload = 0;

            // Function keys have 12 possible list entries
            for (index = 0; index <= 11; index++)
            {
                String key = StringFunctionKey[index];

                if (tmpStringExitKey1.Equals(key)) tmpIndexExitKey1 = index;
                if (tmpStringExitKey2.Equals(key)) tmpIndexExitKey2 = index;
                if (tmpStringExitKey3.Equals(key)) tmpIndexExitKey3 = index;
            }

            // SEB Purpose and Browser View Mode have 2 possible list entries
            for (index = 0; index <= 2; index++)
            {
                String purpose  = StringSebPurpose     [index];
                String viewmode = StringBrowserViewMode[index];

                if (tmpStringSebPurpose     .Equals(purpose )) tmpIndexSebPurpose      = index;
                if (tmpStringBrowserViewMode.Equals(viewmode)) tmpIndexBrowserViewMode = index;
            }

            // Window width and height have 4 possible list entries
            for (index = 0; index <= 3; index++)
            {
                String width  = StringWindowWidth [index];
                String height = StringWindowHeight[index];

                if (tmpStringMainWindowWidth .Equals(width )) tmpIndexMainWindowWidth  = index;
                if (tmpStringMainWindowHeight.Equals(height)) tmpIndexMainWindowHeight = index;

                if (tmpStringNewWindowByLinkWidth  .Equals(width )) tmpIndexNewWindowByLinkWidth  = index;
                if (tmpStringNewWindowByLinkHeight .Equals(height)) tmpIndexNewWindowByLinkHeight = index;
            }

            // Window position, policies etc. have 3 possible list entries
            for (index = 0; index <= 2; index++)
            {
                String position = StringWindowPositioning[index];
                String link     = StringPolicyLinkOpening[index];
                String service  = StringPolicySebService [index];
                String upload   = StringPolicyFileUpload [index];

                if (tmpStringMainWindowPositioning     .Equals(position)) tmpIndexMainWindowPositioning      = index;
                if (tmpStringNewWindowByLinkPositioning.Equals(position)) tmpIndexNewWindowByLinkPositioning = index;

                if (tmpStringNewWindowByLinkPolicy  .Equals(link)) tmpIndexNewWindowByLinkPolicy   = index;
                if (tmpStringNewWindowByScriptPolicy.Equals(link)) tmpIndexNewWindowByScriptPolicy = index;

                if (tmpStringSebServicePolicy.Equals(service)) tmpIndexSebServicePolicy   = index;
                if (tmpStringFileUploadPolicy.Equals(upload )) tmpIndexChooseFileToUpload = index;
            }

            // Store the determined integers
            settingInteger[StateTmp, GroupGeneral, ValueExitKey1] = tmpIndexExitKey1;
            settingInteger[StateTmp, GroupGeneral, ValueExitKey2] = tmpIndexExitKey2;
            settingInteger[StateTmp, GroupGeneral, ValueExitKey3] = tmpIndexExitKey3;

            settingInteger[StateTmp, GroupConfigFile, ValueSebPurpose] = tmpIndexSebPurpose;

            settingInteger[StateTmp, GroupAppearance , ValueBrowserViewMode             ] = tmpIndexBrowserViewMode;
            settingInteger[StateTmp, GroupAppearance , ValueMainBrowserWindowWidth      ] = tmpIndexMainWindowWidth;
            settingInteger[StateTmp, GroupAppearance , ValueMainBrowserWindowHeight     ] = tmpIndexMainWindowHeight;
            settingInteger[StateTmp, GroupAppearance , ValueMainBrowserWindowPositioning] = tmpIndexMainWindowPositioning;

            settingInteger[StateTmp, GroupBrowser, ValueNewBrowserWindowByLinkWidth      ] = tmpIndexNewWindowByLinkWidth;
            settingInteger[StateTmp, GroupBrowser, ValueNewBrowserWindowByLinkHeight     ] = tmpIndexNewWindowByLinkHeight;
            settingInteger[StateTmp, GroupBrowser, ValueNewBrowserWindowByLinkPositioning] = tmpIndexNewWindowByLinkPositioning;

            settingInteger[StateTmp, GroupBrowser, ValueNewBrowserWindowByLinkPolicy  ] = tmpIndexNewWindowByLinkPolicy;
            settingInteger[StateTmp, GroupBrowser, ValueNewBrowserWindowByScriptPolicy] = tmpIndexNewWindowByScriptPolicy;

            settingInteger[StateTmp, GroupDownUploads, ValueChooseFileToUploadPolicy] = tmpIndexChooseFileToUpload;
            settingInteger[StateTmp, GroupSecurity   , ValueSebServicePolicy        ] = tmpIndexSebServicePolicy;

            // Accept the tmp values as the old and new values
            for (int group = 1; group <= GroupNum; group++)
            {
                int minvalue = minValue[group];
                int maxvalue = maxValue[group];

                for (int value = minvalue; value <= maxvalue; value++)
                {
                    settingBoolean[StateOld, group, value] = settingBoolean[StateTmp, group, value];
                    settingString [StateOld, group, value] = settingString [StateTmp, group, value];
                    settingInteger[StateOld, group, value] = settingInteger[StateTmp, group, value];

                    settingBoolean[StateNew, group, value] = settingBoolean[StateTmp, group, value];
                    settingString [StateNew, group, value] = settingString [StateTmp, group, value];
                    settingInteger[StateNew, group, value] = settingInteger[StateTmp, group, value];
                }
            }

            currentDireSebStarterIni = Path.GetDirectoryName(fileName);
            currentFileSebStarterIni = Path.GetFileName     (fileName);
            currentPathSebStarterIni = Path.GetFullPath     (fileName);

            return;
        }



        // *************************************************
        // Convert some settings before writing them to file
        // *************************************************
        private void ConvertSomeSettingsBeforeWritingThemToFile()
        {
            // These ListBox and ComboBox entries need a conversion from integer to string:
            //
            // Exit Key Sequence (exit keys 1,2,3)
            // SEB Purpose
            // Crypto Identity
            // Browser View Mode
            // Main Window Width/Height/Positioning
            // New  Window Width/Height/Positioning
            // Link Opening Policy for Requesting/JavaScript
            // Choose File To Upload
            // SEB Service Policy

            int newIndexExitKey1 = settingInteger[StateNew, GroupGeneral, ValueExitKey1];
            int newIndexExitKey2 = settingInteger[StateNew, GroupGeneral, ValueExitKey2];
            int newIndexExitKey3 = settingInteger[StateNew, GroupGeneral, ValueExitKey3];

            int newIndexSebPurpose     = settingInteger[StateNew, GroupConfigFile, ValueSebPurpose];
            int newIndexCryptoIdentity = settingInteger[StateNew, GroupConfigFile, ValueCryptoIdentity];

            int newIndexBrowserViewMode       = settingInteger[StateNew, GroupAppearance, ValueBrowserViewMode];
            int newIndexMainWindowWidth       = settingInteger[StateNew, GroupAppearance, ValueMainBrowserWindowWidth];
            int newIndexMainWindowHeight      = settingInteger[StateNew, GroupAppearance, ValueMainBrowserWindowHeight];
            int newIndexMainWindowPositioning = settingInteger[StateNew, GroupAppearance, ValueMainBrowserWindowPositioning];

            int newIndexNewWindowWidth          = settingInteger[StateNew, GroupBrowser, ValueNewBrowserWindowByLinkWidth];
            int newIndexNewWindowHeight         = settingInteger[StateNew, GroupBrowser, ValueNewBrowserWindowByLinkHeight];
            int newIndexNewWindowPositioning    = settingInteger[StateNew, GroupBrowser, ValueNewBrowserWindowByLinkPositioning];
            int newIndexNewWindowByLinkPolicy   = settingInteger[StateNew, GroupBrowser, ValueNewBrowserWindowByLinkPolicy];
            int newIndexNewWindowByScriptPolicy = settingInteger[StateNew, GroupBrowser, ValueNewBrowserWindowByScriptPolicy];

            int newIndexChooseFileToUploadPolicy = settingInteger[StateNew, GroupDownUploads, ValueChooseFileToUploadPolicy];
            int newIndexSebServicePolicy         = settingInteger[StateNew, GroupSecurity   , ValueSebServicePolicy];

            // Store the determined strings
            settingString[StateNew, GroupGeneral, ValueExitKey1] = StringFunctionKey[newIndexExitKey1];
            settingString[StateNew, GroupGeneral, ValueExitKey2] = StringFunctionKey[newIndexExitKey2];
            settingString[StateNew, GroupGeneral, ValueExitKey3] = StringFunctionKey[newIndexExitKey3];

            settingString[StateNew, GroupConfigFile, ValueSebPurpose    ] = StringSebPurpose    [newIndexSebPurpose];
            settingString[StateNew, GroupConfigFile, ValueCryptoIdentity] = StringCryptoIdentity[newIndexCryptoIdentity];

            settingString[StateNew, GroupAppearance, ValueBrowserViewMode             ] = StringBrowserViewMode  [newIndexBrowserViewMode];
            settingString[StateNew, GroupAppearance, ValueMainBrowserWindowWidth      ] = StringWindowWidth      [newIndexMainWindowWidth];
            settingString[StateNew, GroupAppearance, ValueMainBrowserWindowHeight     ] = StringWindowHeight     [newIndexMainWindowHeight];
            settingString[StateNew, GroupAppearance, ValueMainBrowserWindowPositioning] = StringWindowPositioning[newIndexMainWindowPositioning];

            settingString[StateNew, GroupBrowser, ValueNewBrowserWindowByLinkWidth      ] = StringWindowWidth      [newIndexNewWindowWidth];
            settingString[StateNew, GroupBrowser, ValueNewBrowserWindowByLinkHeight     ] = StringWindowHeight     [newIndexNewWindowHeight];
            settingString[StateNew, GroupBrowser, ValueNewBrowserWindowByLinkPositioning] = StringWindowPositioning[newIndexNewWindowPositioning];
            settingString[StateNew, GroupBrowser, ValueNewBrowserWindowByLinkPolicy     ] = StringPolicyLinkOpening[newIndexNewWindowByLinkPolicy];
            settingString[StateNew, GroupBrowser, ValueNewBrowserWindowByScriptPolicy   ] = StringPolicyLinkOpening[newIndexNewWindowByScriptPolicy];

            settingString[StateNew, GroupDownUploads, ValueChooseFileToUploadPolicy] = StringPolicyFileUpload[newIndexChooseFileToUploadPolicy];
            settingString[StateNew, GroupSecurity   , ValueSebServicePolicy        ] = StringPolicySebService[newIndexSebServicePolicy];

            return;
        }



        // ************************************************
        // Convert some settings after writing them to file
        // ************************************************
        private void ConvertSomeSettingsAfterWritingThemToFile(String fileName)
        {
            // Accept the old values as the new values
            for (int group = 1; group <= GroupNum; group++)
            {
                int minvalue = minValue[group];
                int maxvalue = maxValue[group];

                for (int value = minvalue; value <= maxvalue; value++)
                {
                    settingBoolean[StateOld, group, value] = settingBoolean[StateNew, group, value];
                    settingString [StateOld, group, value] = settingString [StateNew, group, value];
                    settingInteger[StateOld, group, value] = settingInteger[StateNew, group, value];
                }
            }

            currentDireSebStarterIni = Path.GetDirectoryName(fileName);
            currentFileSebStarterIni = Path.GetFileName     (fileName);
            currentPathSebStarterIni = Path.GetFullPath     (fileName);

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
                        rightBoolean  = rightString.Equals("1");
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

            } // end try
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
                                + configString[FileSebStarterIni] + " config file."
                                + " Debug data: "
                                + " fileLine   = " +  fileLine
                                + " leftString = " +  leftString
                                +" rightString = " + rightString,
                                "Error when reading " + configString[FileSebStarterIni],
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            // After reading the settings from file, update the widgets
            ConvertSomeSettingsAfterReadingThemFromFile(fileName);
            SetWidgetsToNewSettingsOfSebStarterIni();
            return true;
        }



        // ****************************************
        // Open the .xml file and read the settings
        // ****************************************
        private Boolean OpenXmlFile(String fileName)
        {
            try 
            {
                // Read the .xml file
                // Parse the XML structure into a C# object
                sebSettingsTmp = (Dictionary<string, object>)Plist.readPlist(fileName);
/*
                // Open the .xml file for reading
                XmlSerializer deserializer = new XmlSerializer(typeof(SEBClientConfig));
                TextReader      textReader = new StreamReader (fileName);

                // Parse the XML structure into a C# object
                sebSettObso = (SEBClientConfig)deserializer.Deserialize(textReader);

                // Close the .xml file
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
            // convert the C# object to arrays and update the widgets
            ConvertCSharpObjectToArrays();
            ConvertSomeSettingsAfterReadingThemFromFile(fileName);
            SetWidgetsToNewSettingsOfSebStarterIni();
            return true;
        }



        // ****************************************
        // Open the .seb file and read the settings
        // ****************************************
        private Boolean OpenSebFile(String fileName)
        {
            try 
            {
                // Open the .seb file for reading
                // Load the encrypted configuration settings
                // Decrypt the configuration settings
                // Deserialise the decrypted string
                TextReader     textReader = new StreamReader(fileName);
                String  encryptedSettings = textReader.ReadToEnd();
                String  decryptedSettings = sebController.DecryptSebClientSettings (encryptedSettings).Trim();
                MemoryStream memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(decryptedSettings));

                // Parse the XML structure into a C# object
                sebSettObso = (SEBClientConfig)sebSerializer.Deserialize(memoryStream);

                // Close the memory stream and text reader
                memoryStream.Close();
                  textReader.Close();
            }
            catch (Exception streamReadException)
            {
                // Let the user know what went wrong
                Console.WriteLine("The .seb file could not be read:");
                Console.WriteLine(streamReadException.Message);
                return false;
            }

            // After reading the settings from file,
            // convert the C# object to arrays and update the widgets
            ConvertCSharpObjectToArrays();
            ConvertSomeSettingsAfterReadingThemFromFile(fileName);
            SetWidgetsToNewSettingsOfSebStarterIni();
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

                        if ((rightType == TypeBoolean) && (rightBoolean == false)) rightString = "0";
                        if ((rightType == TypeBoolean) && (rightBoolean ==  true)) rightString = "1";

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

            } // end try
            catch (Exception streamWriteException) 
            {
                // Let the user know what went wrong
                Console.WriteLine("The .ini file could not be written:");
                Console.WriteLine(streamWriteException.Message);
                return false;
            }

            // After writing the settings to file, update the widgets
            ConvertSomeSettingsAfterWritingThemToFile(fileName);
            SetWidgetsToNewSettingsOfSebStarterIni();
            return true;
        }



        // ***********************************************
        // Write the settings to the .xml file and save it
        // ***********************************************
        private Boolean SaveXmlFile(String fileName)
        {
            // Before writing the settings to file,
            // convert the arrays to the C# object
            ConvertSomeSettingsBeforeWritingThemToFile();
            ConvertArraysToCSharpObject();

            try 
            {
                // If the .xml file already exists, delete it
                // and write it again from scratch with new data
                if (File.Exists(fileName))
                    File.Delete(fileName);

                // Open the .xml file for writing
                XmlSerializer serializer = new XmlSerializer(typeof(SEBClientConfig));
                TextWriter    textWriter = new StreamWriter(fileName);

                // Copy the C# object into an XML structure
                serializer.Serialize(textWriter, sebSettObso);

                // Close the .xml file
                textWriter.Close();

            } // end try
            catch (Exception streamWriteException) 
            {
                // Let the user know what went wrong
                Console.WriteLine("The .xml file could not be written:");
                Console.WriteLine(streamWriteException.Message);
                return false;
            }

            // After writing the settings to file, update the widgets
            ConvertSomeSettingsAfterWritingThemToFile(fileName);
            SetWidgetsToNewSettingsOfSebStarterIni();
            return true;
        }



        // ***********************************************
        // Write the settings to the .seb file and save it
        // ***********************************************
        private Boolean SaveSebFile(String fileName)
        {
            // Before writing the settings to file,
            // convert the arrays to the C# object
            ConvertSomeSettingsBeforeWritingThemToFile();
            ConvertArraysToCSharpObject();

            try 
            {
                // If the .seb file already exists, delete it
                // and write it again from scratch with new data
                if (File.Exists(fileName))
                    File.Delete(fileName);

/*
                MemoryStream memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(decryptedSettings));

                // Copy the C# object into an XML structure
                // Serialise the decrypted string
                sebSerializer.Serialize(memoryStream, sebSettings);

                // Encrypt the configuration settings
                String encryptedSettings = sebController.EncryptWithPassword(decryptedSettings, passPhrase);

                // Open the .seb file for writing
                // Save the encrypted configuration settings

                TextWriter     textWriter = new StreamWriter(fileName);
                textWriter.Write(encryptedSettings);

                // Close the memory stream and text writer
                memoryStream.Close();
                  textWriter.Close();
*/

            } // end try
            catch (Exception streamWriteException) 
            {
                // Let the user know what went wrong
                Console.WriteLine("The .seb file could not be written:");
                Console.WriteLine(streamWriteException.Message);
                return false;
            }

            // After writing the settings to file, update the widgets
            ConvertSomeSettingsAfterWritingThemToFile(fileName);
            SetWidgetsToNewSettingsOfSebStarterIni();
            return true;
        }



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
            settingBoolean[StateTmp, GroupGeneral, ValueAllowQuit ] = sebSettObso.getSecurityOption("allowQuit"         ).getBool();
            settingBoolean[StateTmp, GroupGeneral, ValueIgnoreQuitPassword ] = sebSettObso.getSecurityOption("ignoreQuitPassword").getBool();
            settingString [StateTmp, GroupGeneral, ValueExitKey1           ] = sebSettObso.getExitKey       ("exitKey1").Value;
            settingString [StateTmp, GroupGeneral, ValueExitKey2           ] = sebSettObso.getExitKey       ("exitKey2").Value;
            settingString [StateTmp, GroupGeneral, ValueExitKey3           ] = sebSettObso.getExitKey       ("exitKey3").Value;

          //settingString [StateTmp, GroupConfigFile, ValueSebPurpose            ] = sebSettings.getPolicySetting ("sebPurpose"            ).Value;
          //settingBoolean[StateTmp, GroupConfigFile, ValueStartingAnExam        ] = sebSettings.getSecurityOption("startingAnExam"        ).getBool();
          //settingBoolean[StateTmp, GroupConfigFile, ValueConfiguringAClient    ] = sebSettings.getSecurityOption("configuringAClient"    ).getBool();
            settingBoolean[StateTmp, GroupConfigFile, ValueAllowPreferencesWindow   ] = sebSettObso.getSecurityOption("allowPreferencesWindow").getBool();
          //settingString [StateTmp, GroupConfigFile, ValueChooseIdentity        ] = sebSettings.getPassword      ("chooseIdentity"        ).Value;
          //settingString [StateTmp, GroupConfigFile, ValueHashedSettingsPassword] = sebSettings.getPassword      ("hashedSettingsPassword").Value;

          //settingString [StateTmp, GroupAppearance, ValueBrowserViewMode      ] = sebSettings.getPolicySetting ("browserViewMode").Value;
          //settingBoolean[StateTmp, GroupAppearance, ValueUseBrowserWindow     ] = sebSettings.getSecurityOption("useBrowserWindow" ).getBool();
          //settingBoolean[StateTmp, GroupAppearance, ValueUseFullScreenMode    ] = sebSettings.getSecurityOption("useFullScreenMode").getBool();
            settingString [StateTmp, GroupAppearance, ValueMainBrowserWindowWidth      ] = sebSettObso.getPolicySetting ("mainBrowserWindowWidth"   ).Value;
            settingString [StateTmp, GroupAppearance, ValueMainBrowserWindowHeight     ] = sebSettObso.getPolicySetting ("mainBrowserWindowHeight"  ).Value;
          //settingString [StateTmp, GroupAppearance, ValueMainWindowPosition   ] = sebSettings.getPolicySetting ("mainBrowserWindowPosition").Value;
            settingBoolean[StateTmp, GroupAppearance, ValueEnableBrowserWindowToolbar  ] = sebSettObso.getSecurityOption("enableBrowserWindowToolbar").getBool();
            settingBoolean[StateTmp, GroupAppearance, ValueHideBrowserWindowToolbar ] = sebSettObso.getSecurityOption(  "hideBrowserWindowToolbar").getBool();
            settingBoolean[StateTmp, GroupAppearance, ValueShowMenuBar          ] = sebSettObso.getSecurityOption("showMenuBar").getBool();
            settingBoolean[StateTmp, GroupAppearance, ValueShowTaskBar] = sebSettObso.getSecurityOption("showTaskBar").getBool();

          //settingString [StateTmp, GroupBrowser, ValueNewWindowPolicyHTML ] = sebSettings.getPolicySetting ("newBrowserWindowByLinkPolicy").Value;
          //settingString [StateTmp, GroupBrowser, ValueNewWindowPolicyJava ] = sebSettings.getPolicySetting ("newBrowserWindowByLinkPolicy").Value;
            settingBoolean[StateTmp, GroupBrowser, ValueNewBrowserWindowByLinkBlockForeign      ] = sebSettObso.getSecurityOption("newBrowserWindowByLinkBlockForeign"  ).getBool();
            settingBoolean[StateTmp, GroupBrowser, ValueNewBrowserWindowByScriptBlockForeign      ] = sebSettObso.getSecurityOption("newBrowserWindowByScriptBlockForeign").getBool();
            settingString [StateTmp, GroupBrowser, ValueNewBrowserWindowByLinkWidth      ] = sebSettObso.getPolicySetting ("newBrowserWindowByLinkWidth"   ).Value;
            settingString [StateTmp, GroupBrowser, ValueNewBrowserWindowByLinkHeight     ] = sebSettObso.getPolicySetting ("newBrowserWindowByLinkHeight"  ).Value;
          //settingString [StateTmp, GroupBrowser, ValueNewWindowPosition   ] = sebSettings.getPolicySetting ("newBrowserWindowByLinkPosition").Value;
            settingBoolean[StateTmp, GroupBrowser, ValueEnablePlugins       ] = sebSettObso.getSecurityOption("enablePlugins"   ).getBool();
            settingBoolean[StateTmp, GroupBrowser, ValueEnableJava          ] = sebSettObso.getSecurityOption("enableJava"      ).getBool();
            settingBoolean[StateTmp, GroupBrowser, ValueEnableJavaScript    ] = sebSettObso.getSecurityOption("enableJavaScript").getBool();
            settingBoolean[StateTmp, GroupBrowser, ValueBlockPopupWindows   ] = sebSettObso.getSecurityOption("blockPopUpWindows").getBool();
            settingBoolean[StateTmp, GroupBrowser, ValueAllowBrowsingBackForward] = sebSettObso.getSecurityOption("enableBrowsingBackForward").getBool();
            settingBoolean[StateTmp, GroupBrowser, ValueEnableSebBrowser] = sebSettObso.getSecurityOption("enableSebBrowser").getBool();

            settingBoolean[StateTmp, GroupDownUploads, ValueAllowDownUploads  ] = sebSettObso.getSecurityOption   ("allowDownUploads").getBool();
            settingBoolean[StateTmp, GroupDownUploads, ValueOpenDownloads] = sebSettObso.getSecurityOption   ("openDownloads"   ).getBool();
            settingBoolean[StateTmp, GroupDownUploads, ValueDownloadPDFFiles  ] = sebSettObso.getSecurityOption   ("downloadPDFFiles").getBool();
            settingString [StateTmp, GroupDownUploads, ValueDownloadDirectoryWin    ] = sebSettObso.getDownloadDirectory("downloadDirectoryWin"    ).Path;
            settingString [StateTmp, GroupDownUploads, ValueChooseFileToUploadPolicy       ] = sebSettObso.getPolicySetting    ("chooseFileToUploadPolicy").Value;

          //settingString [StateTmp, GroupExam, ValueBrowserExamKey    ] = sebSettings.getUrlAddress("browserExamKey" ).Url;
            settingBoolean[StateTmp, GroupExam, ValueCopyBrowserExamKey] = sebSettObso.getSecurityOption("copyExamKeyToClipboardWhenQuitting").getBool();
            settingBoolean[StateTmp, GroupExam, ValueSendBrowserExamKey] = sebSettObso.getSecurityOption("sendBrowserExamKey").getBool();
            settingString [StateTmp, GroupExam, ValueQuitURL           ] = sebSettObso.getUrlAddress("quitURL").Url;

            settingBoolean[StateTmp, GroupApplications, ValueMonitorProcesses         ] = sebSettObso.getSecurityOption("monitorProcesses").getBool();
            settingBoolean[StateTmp, GroupApplications, ValueAllowSwitchToApplications] = sebSettObso.getSecurityOption("allowSwitchToApplications").getBool();
            settingBoolean[StateTmp, GroupApplications, ValueAllowFlashFullscreen ] = sebSettObso.getSecurityOption("allowFlashFullscreen").getBool();

            settingString [StateTmp, GroupSecurity, ValueSebServicePolicy   ] = sebSettObso.getPolicySetting ("sebServicePolicy"   ).Value;
            settingBoolean[StateTmp, GroupSecurity, ValueAllowVirtualMachine] = sebSettObso.getSecurityOption("allowVirtualMachine").getBool();
            settingBoolean[StateTmp, GroupSecurity, ValueCreateNewDesktop   ] = sebSettObso.getSecurityOption("createNewDesktop"   ).getBool();
            settingBoolean[StateTmp, GroupSecurity, ValueAllowUserSwitching ] = sebSettObso.getSecurityOption("allowUserSwitching" ).getBool();
            settingBoolean[StateTmp, GroupSecurity, ValueEnableLog      ] = sebSettObso.getSecurityOption("enableLog"          ).getBool();

            settingBoolean[StateTmp, GroupInsideSeb, ValueEnableSwitchUser       ] = sebSettObso.getRegistryValue("insideSebEnableSwitchUser"       ).getBool();
            settingBoolean[StateTmp, GroupInsideSeb, ValueEnableLockThisComputer ] = sebSettObso.getRegistryValue("insideSebEnableLockThisComputer" ).getBool();
            settingBoolean[StateTmp, GroupInsideSeb, ValueEnableChangeAPassword  ] = sebSettObso.getRegistryValue("insideSebEnableChangePassword"   ).getBool();
            settingBoolean[StateTmp, GroupInsideSeb, ValueEnableStartTaskManager ] = sebSettObso.getRegistryValue("insideSebEnableStartTaskManager" ).getBool();
            settingBoolean[StateTmp, GroupInsideSeb, ValueEnableLogOff           ] = sebSettObso.getRegistryValue("insideSebEnableLogOff"           ).getBool();
            settingBoolean[StateTmp, GroupInsideSeb, ValueEnableShutDown         ] = sebSettObso.getRegistryValue("insideSebEnableShutDown"         ).getBool();
            settingBoolean[StateTmp, GroupInsideSeb, ValueEnableEaseOfAccess     ] = sebSettObso.getRegistryValue("insideSebEnableEaseOfAccess"     ).getBool();
            settingBoolean[StateTmp, GroupInsideSeb, ValueEnableVmWareClientShade] = sebSettObso.getRegistryValue("insideSebEnableVmWareClientShade").getBool();

            settingBoolean[StateTmp, GroupOutsideSeb, ValueEnableSwitchUser       ] = sebSettObso.getRegistryValue("outsideSebEnableSwitchUser"       ).getBool();
            settingBoolean[StateTmp, GroupOutsideSeb, ValueEnableLockThisComputer ] = sebSettObso.getRegistryValue("outsideSebEnableLockThisComputer" ).getBool();
            settingBoolean[StateTmp, GroupOutsideSeb, ValueEnableChangeAPassword  ] = sebSettObso.getRegistryValue("outsideSebEnableChangePassword"   ).getBool();
            settingBoolean[StateTmp, GroupOutsideSeb, ValueEnableStartTaskManager ] = sebSettObso.getRegistryValue("outsideSebEnableStartTaskManager" ).getBool();
            settingBoolean[StateTmp, GroupOutsideSeb, ValueEnableLogOff           ] = sebSettObso.getRegistryValue("outsideSebEnableLogOff"           ).getBool();
            settingBoolean[StateTmp, GroupOutsideSeb, ValueEnableShutDown         ] = sebSettObso.getRegistryValue("outsideSebEnableShutDown"         ).getBool();
            settingBoolean[StateTmp, GroupOutsideSeb, ValueEnableEaseOfAccess     ] = sebSettObso.getRegistryValue("outsideSebEnableEaseOfAccess"     ).getBool();
            settingBoolean[StateTmp, GroupOutsideSeb, ValueEnableVmWareClientShade] = sebSettObso.getRegistryValue("outsideSebEnableVmWareClientShade").getBool();

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
            sebSettObso.getSecurityOption("enablePlugins"    ).setBool(settingBoolean[StateNew, GroupBrowser, ValueEnablePlugins]);
            sebSettObso.getSecurityOption("enableJava"       ).setBool(settingBoolean[StateNew, GroupBrowser, ValueEnableJava]);
            sebSettObso.getSecurityOption("enableJavaScript" ).setBool(settingBoolean[StateNew, GroupBrowser, ValueEnableJavaScript]);
            sebSettObso.getSecurityOption("blockPopUpWindows").setBool(settingBoolean[StateNew, GroupBrowser, ValueBlockPopupWindows]);
            sebSettObso.getSecurityOption("enableBrowsingBackForward").setBool(settingBoolean[StateNew, GroupBrowser, ValueAllowBrowsingBackForward]);
            sebSettObso.getSecurityOption("enableSebBrowser"         ).setBool(settingBoolean[StateNew, GroupBrowser, ValueEnableSebBrowser]);

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
            sebSettObso.getSecurityOption("enableLog"          ).setBool(settingBoolean[StateNew, GroupSecurity, ValueEnableLog]);

            sebSettObso.getRegistryValue("insideSebEnableSwitchUser"       ).setBool(settingBoolean[StateNew, GroupInsideSeb, ValueEnableSwitchUser]);
            sebSettObso.getRegistryValue("insideSebEnableLockThisComputer" ).setBool(settingBoolean[StateNew, GroupInsideSeb, ValueEnableLockThisComputer]);
            sebSettObso.getRegistryValue("insideSebEnableChangePassword"   ).setBool(settingBoolean[StateNew, GroupInsideSeb, ValueEnableChangeAPassword]);
            sebSettObso.getRegistryValue("insideSebEnableStartTaskManager" ).setBool(settingBoolean[StateNew, GroupInsideSeb, ValueEnableStartTaskManager]);
            sebSettObso.getRegistryValue("insideSebEnableLogOff"           ).setBool(settingBoolean[StateNew, GroupInsideSeb, ValueEnableLogOff]);
            sebSettObso.getRegistryValue("insideSebEnableShutDown"         ).setBool(settingBoolean[StateNew, GroupInsideSeb, ValueEnableShutDown]);
            sebSettObso.getRegistryValue("insideSebEnableEaseOfAccess"     ).setBool(settingBoolean[StateNew, GroupInsideSeb, ValueEnableEaseOfAccess]);
            sebSettObso.getRegistryValue("insideSebEnableVmWareClientShade").setBool(settingBoolean[StateNew, GroupInsideSeb, ValueEnableVmWareClientShade]);

            sebSettObso.getRegistryValue("outsideSebEnableSwitchUser"       ).setBool(settingBoolean[StateNew, GroupOutsideSeb, ValueEnableSwitchUser]);
            sebSettObso.getRegistryValue("outsideSebEnableLockThisComputer" ).setBool(settingBoolean[StateNew, GroupOutsideSeb, ValueEnableLockThisComputer]);
            sebSettObso.getRegistryValue("outsideSebEnableChangePassword"   ).setBool(settingBoolean[StateNew, GroupOutsideSeb, ValueEnableChangeAPassword]);
            sebSettObso.getRegistryValue("outsideSebEnableStartTaskManager" ).setBool(settingBoolean[StateNew, GroupOutsideSeb, ValueEnableStartTaskManager]);
            sebSettObso.getRegistryValue("outsideSebEnableLogOff"           ).setBool(settingBoolean[StateNew, GroupOutsideSeb, ValueEnableLogOff]);
            sebSettObso.getRegistryValue("outsideSebEnableShutDown"         ).setBool(settingBoolean[StateNew, GroupOutsideSeb, ValueEnableShutDown]);
            sebSettObso.getRegistryValue("outsideSebEnableEaseOfAccess"     ).setBool(settingBoolean[StateNew, GroupOutsideSeb, ValueEnableEaseOfAccess]);
            sebSettObso.getRegistryValue("outsideSebEnableVmWareClientShade").setBool(settingBoolean[StateNew, GroupOutsideSeb, ValueEnableVmWareClientShade]);

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
            settingString[StateNew, GroupGeneral, ValueStartURL] = textBoxStartURL.Text;
        }

        private void buttonPasteFromSavedClipboard_Click(object sender, EventArgs e)
        {

        }

        private void textBoxSebServerURL_TextChanged(object sender, EventArgs e)
        {
            settingString[StateNew, GroupGeneral, ValueSebServerURL] = textBoxSebServerURL.Text;
        }

        private void textBoxAdminPassword_TextChanged(object sender, EventArgs e)
        {
            settingString[StateNew, GroupGeneral, ValueAdminPassword] = textBoxAdminPassword.Text;
        }

        private void textBoxConfirmAdminPassword_TextChanged(object sender, EventArgs e)
        {
            settingString[StateNew, GroupGeneral, ValueConfirmAdminPassword] = textBoxConfirmAdminPassword.Text;
        }

        private void checkBoxAllowQuit_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupGeneral, ValueAllowQuit] = checkBoxAllowQuit.Checked;
        }

        private void checkBoxIgnoreQuitPassword_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupGeneral, ValueIgnoreQuitPassword] = checkBoxIgnoreQuitPassword.Checked;
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

            textBoxQuitHashcode.Text = newStringQuitHashcode;

            settingString[StateNew, GroupGeneral, ValueQuitPassword] = newStringQuitPassword;
            settingString[StateNew, GroupGeneral, ValueHashedQuitPassword] = newStringQuitHashcode;
        }

        private void textBoxConfirmQuitPassword_TextChanged(object sender, EventArgs e)
        {
            settingString[StateNew, GroupGeneral, ValueConfirmQuitPassword] = textBoxConfirmQuitPassword.Text;
        }

        private void listBoxExitKey1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Make sure that all three exit keys are different.
            // If selected key is already occupied, revert to previously selected key.
            if ((listBoxExitKey1.SelectedIndex == listBoxExitKey2.SelectedIndex) ||
                (listBoxExitKey1.SelectedIndex == listBoxExitKey3.SelectedIndex))
                 listBoxExitKey1.SelectedIndex =  settingInteger[StateNew, GroupGeneral, ValueExitKey1] - 1;
            settingInteger[StateNew, GroupGeneral, ValueExitKey1] = listBoxExitKey1.SelectedIndex + 1;
        }

        private void listBoxExitKey2_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Make sure that all three exit keys are different.
            // If selected key is already occupied, revert to previously selected key.
            if ((listBoxExitKey2.SelectedIndex == listBoxExitKey1.SelectedIndex) ||
                (listBoxExitKey2.SelectedIndex == listBoxExitKey3.SelectedIndex))
                 listBoxExitKey2.SelectedIndex =  settingInteger[StateNew, GroupGeneral, ValueExitKey2] - 1;
            settingInteger[StateNew, GroupGeneral, ValueExitKey2] = listBoxExitKey2.SelectedIndex + 1;
        }

        private void listBoxExitKey3_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Make sure that all three exit keys are different.
            // If selected key is already occupied, revert to previously selected key.
            if ((listBoxExitKey3.SelectedIndex == listBoxExitKey1.SelectedIndex) ||
                (listBoxExitKey3.SelectedIndex == listBoxExitKey2.SelectedIndex))
                 listBoxExitKey3.SelectedIndex =  settingInteger[StateNew, GroupGeneral, ValueExitKey3] - 1;
            settingInteger[StateNew, GroupGeneral, ValueExitKey3] = listBoxExitKey3.SelectedIndex + 1;
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
            // to the target configuration file ("SebStarter.ini/xml/seb")
            if (currentFileSebStarterIni.Equals(""))
            {
                currentFileSebStarterIni = targetFileSebStarterIni;
                currentPathSebStarterIni = targetPathSebStarterIni;
            }

            String fileName = currentPathSebStarterIni;

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
                 settingInteger[StateNew, GroupConfigFile, ValueSebPurpose] = 0;
            else settingInteger[StateNew, GroupConfigFile, ValueSebPurpose] = 1;
        }

        private void radioButtonConfiguringAClient_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonConfiguringAClient.Checked == true)
                 settingInteger[StateNew, GroupConfigFile, ValueSebPurpose] = 1;
            else settingInteger[StateNew, GroupConfigFile, ValueSebPurpose] = 0;
        }

        private void checkBoxAllowPreferencesWindow_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupConfigFile, ValueAllowPreferencesWindow] = checkBoxAllowPreferencesWindow.Checked;
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
            settingString[StateNew, GroupConfigFile, ValueSettingsPassword] = textBoxSettingsPassword.Text;
        }

        private void textBoxConfirmSettingsPassword_TextChanged(object sender, EventArgs e)
        {
            settingString[StateNew, GroupConfigFile, ValueConfirmSettingsPassword] = textBoxConfirmSettingsPassword.Text;
        }


        private void buttonDefaultSettings_Click(object sender, EventArgs e)
        {
            SetNewSettingsOfFileToState(StateDef);
            SetWidgetsToNewSettingsOfSebStarterIni();
        }

        private void buttonRevertToLastOpened_Click(object sender, EventArgs e)
        {
            SetNewSettingsOfFileToState(StateOld);
            SetWidgetsToNewSettingsOfSebStarterIni();
        }


        private void labelOpenSettings_Click(object sender, EventArgs e)
        {
            // Set the default directory and file name in the File Dialog
            openFileDialogSebStarterIni.InitialDirectory = currentDireSebStarterIni;
            openFileDialogSebStarterIni.FileName         = currentFileSebStarterIni;

            // Get the user inputs in the File Dialog
            DialogResult fileDialogResult = openFileDialogSebStarterIni.ShowDialog();
            String       fileName         = openFileDialogSebStarterIni.FileName;

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
            saveFileDialogSebStarterIni.InitialDirectory = currentDireSebStarterIni;
            saveFileDialogSebStarterIni.FileName         = currentFileSebStarterIni;

            // Get the user inputs in the File Dialog
            DialogResult fileDialogResult = saveFileDialogSebStarterIni.ShowDialog();
            String       fileName         = saveFileDialogSebStarterIni.FileName;

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
/*
            SaveIniFile(fileNameIni);
            SaveXmlFile(fileNameXml);
            SaveSebFile(fileNameSeb);
*/
        }



        // ******************
        // Group "Appearance"
        // ******************
        private void radioButtonUseBrowserWindow_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonUseBrowserWindow.Checked == true)
                 settingInteger[StateNew, GroupAppearance, ValueBrowserViewMode] = 0;
            else settingInteger[StateNew, GroupAppearance, ValueBrowserViewMode] = 1;
        }

        private void radioButtonUseFullScreenMode_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonUseFullScreenMode.Checked == true)
                 settingInteger[StateNew, GroupAppearance, ValueBrowserViewMode] = 1;
            else settingInteger[StateNew, GroupAppearance, ValueBrowserViewMode] = 0;
        }

        private void comboBoxMainBrowserWindowWidth_SelectedIndexChanged(object sender, EventArgs e)
        {
            settingInteger[StateNew, GroupAppearance, ValueMainBrowserWindowWidth] = comboBoxMainBrowserWindowWidth.SelectedIndex + 1;
            settingString [StateNew, GroupAppearance, ValueMainBrowserWindowWidth] = comboBoxMainBrowserWindowWidth.Text;
        }

        private void comboBoxMainBrowserWindowWidth_TextUpdate(object sender, EventArgs e)
        {
            settingInteger[StateNew, GroupAppearance, ValueMainBrowserWindowWidth] = comboBoxMainBrowserWindowWidth.SelectedIndex + 1;
            settingString [StateNew, GroupAppearance, ValueMainBrowserWindowWidth] = comboBoxMainBrowserWindowWidth.Text;
        }

        private void comboBoxMainBrowserWindowHeight_SelectedIndexChanged(object sender, EventArgs e)
        {
            settingInteger[StateNew, GroupAppearance, ValueMainBrowserWindowHeight] = comboBoxMainBrowserWindowHeight.SelectedIndex + 1;
            settingString [StateNew, GroupAppearance, ValueMainBrowserWindowHeight] = comboBoxMainBrowserWindowHeight.Text;
        }

        private void comboBoxMainBrowserWindowHeight_TextUpdate(object sender, EventArgs e)
        {
            settingInteger[StateNew, GroupAppearance, ValueMainBrowserWindowHeight] = comboBoxMainBrowserWindowHeight.SelectedIndex + 1;
            settingString [StateNew, GroupAppearance, ValueMainBrowserWindowHeight] = comboBoxMainBrowserWindowHeight.Text;
        }

        private void listBoxMainBrowserWindowPositioning_SelectedIndexChanged(object sender, EventArgs e)
        {
            settingInteger[StateNew, GroupAppearance, ValueMainBrowserWindowPositioning] = listBoxMainBrowserWindowPositioning.SelectedIndex + 1;
        }

        private void checkBoxEnableBrowserWindowToolbar_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupAppearance, ValueEnableBrowserWindowToolbar] = checkBoxEnableBrowserWindowToolbar.Checked;
            checkBoxHideBrowserWindowToolbar.Enabled = checkBoxEnableBrowserWindowToolbar.Checked;
        }

        private void checkBoxHideBrowserWindowToolbar_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupAppearance, ValueHideBrowserWindowToolbar] = checkBoxHideBrowserWindowToolbar.Checked;
        }

        private void checkBoxShowMenuBar_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupAppearance, ValueShowMenuBar] = checkBoxShowMenuBar.Checked;
        }

        private void checkBoxShowTaskBar_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupAppearance, ValueShowTaskBar] = checkBoxShowTaskBar.Checked;
        }



        // ***************
        // Group "Browser"
        // ***************
        private void listBoxOpenLinksHTML_SelectedIndexChanged(object sender, EventArgs e)
        {
            settingInteger[StateNew, GroupBrowser, ValueNewBrowserWindowByLinkPolicy] = listBoxOpenLinksHTML.SelectedIndex + 1;
        }

        private void listBoxOpenLinksJava_SelectedIndexChanged(object sender, EventArgs e)
        {
            settingInteger[StateNew, GroupBrowser, ValueNewBrowserWindowByScriptPolicy] = listBoxOpenLinksJava.SelectedIndex + 1;
        }

        private void checkBoxBlockLinksHTML_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupBrowser, ValueNewBrowserWindowByLinkBlockForeign] = checkBoxBlockLinksHTML.Checked;
        }

        private void checkBoxBlockLinksJava_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupBrowser, ValueNewBrowserWindowByScriptBlockForeign] = checkBoxBlockLinksJava.Checked;
        }

        private void comboBoxNewBrowserWindowWidth_SelectedIndexChanged(object sender, EventArgs e)
        {
            settingInteger[StateNew, GroupBrowser, ValueNewBrowserWindowByLinkWidth] = comboBoxNewBrowserWindowWidth.SelectedIndex + 1;
            settingString [StateNew, GroupBrowser, ValueNewBrowserWindowByLinkWidth] = comboBoxNewBrowserWindowWidth.Text;
        }

        private void comboBoxNewBrowserWindowWidth_TextUpdate(object sender, EventArgs e)
        {
            settingInteger[StateNew, GroupBrowser, ValueNewBrowserWindowByLinkWidth] = comboBoxNewBrowserWindowWidth.SelectedIndex + 1;
            settingString [StateNew, GroupBrowser, ValueNewBrowserWindowByLinkWidth] = comboBoxNewBrowserWindowWidth.Text;
        }

        private void comboBoxNewBrowserWindowHeight_SelectedIndexChanged(object sender, EventArgs e)
        {
            settingInteger[StateNew, GroupBrowser, ValueNewBrowserWindowByLinkHeight] = comboBoxNewBrowserWindowHeight.SelectedIndex + 1;
            settingString [StateNew, GroupBrowser, ValueNewBrowserWindowByLinkHeight] = comboBoxNewBrowserWindowHeight.Text;
        }

        private void comboBoxNewBrowserWindowHeight_TextUpdate(object sender, EventArgs e)
        {
            settingInteger[StateNew, GroupBrowser, ValueNewBrowserWindowByLinkHeight] = comboBoxNewBrowserWindowHeight.SelectedIndex + 1;
            settingString [StateNew, GroupBrowser, ValueNewBrowserWindowByLinkHeight] = comboBoxNewBrowserWindowHeight.Text;
        }

        private void listBoxNewBrowserWindowPositioning_SelectedIndexChanged(object sender, EventArgs e)
        {
            settingInteger[StateNew, GroupBrowser, ValueNewBrowserWindowByLinkPositioning] = listBoxNewBrowserWindowPositioning.SelectedIndex + 1;
        }

        private void checkBoxEnablePlugins_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupBrowser, ValueEnablePlugins] = checkBoxEnablePlugIns.Checked;
        }

        private void checkBoxEnableJava_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupBrowser, ValueEnableJava] = checkBoxEnableJava.Checked;
        }

        private void checkBoxEnableJavaScript_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupBrowser, ValueEnableJavaScript] = checkBoxEnableJavaScript.Checked;
        }

        private void checkBoxBlockPopupWindows_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupBrowser, ValueBlockPopupWindows] = checkBoxBlockPopupWindows.Checked;
        }

        private void checkBoxAllowBrowsingBackForward_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupBrowser, ValueAllowBrowsingBackForward] = checkBoxAllowBrowsingBackForward.Checked;
        }

        private void checkBoxUseSebWithoutBrowser_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupBrowser, ValueEnableSebBrowser] = !(checkBoxUseSebWithoutBrowser.Checked);
        }



        // ********************
        // Group "Down/Uploads"
        // ********************
        private void checkBoxAllowDownUploads_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupDownUploads, ValueAllowDownUploads] = checkBoxAllowDownUploads.Checked;
        }

        private void folderBrowserDialogDownloadDirectoryWin_HelpRequest(object sender, EventArgs e)
        {

        }

        private void buttonDownloadDirectoryWin_Click(object sender, EventArgs e)
        {
            // Set the default directory in the Folder Browser Dialog
            folderBrowserDialogDownloadDirectoryWin.RootFolder = Environment.SpecialFolder.DesktopDirectory;
//          folderBrowserDialogDownloadDirectoryWin.RootFolder = Environment.CurrentDirectory;

            // Get the user inputs in the File Dialog
            DialogResult dialogResult = folderBrowserDialogDownloadDirectoryWin.ShowDialog();
            String       downloadPath = folderBrowserDialogDownloadDirectoryWin.SelectedPath;

            // If the user clicked "Cancel", do nothing
            if (dialogResult.Equals(DialogResult.Cancel)) return;

            // If the user clicked "OK", ...
            settingString[StateNew, GroupDownUploads, ValueDownloadDirectoryWin]     = downloadPath;
                                                      labelDownloadDirectoryWin.Text = downloadPath;
        }

        private void checkBoxOpenDownloads_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupDownUploads, ValueOpenDownloads] = checkBoxOpenDownloads.Checked;
        }

        private void listBoxChooseFileToUploadPolicy_SelectedIndexChanged(object sender, EventArgs e)
        {
            settingInteger[StateNew, GroupDownUploads, ValueChooseFileToUploadPolicy] = listBoxChooseFileToUploadPolicy.SelectedIndex + 1;
        }

        private void checkBoxDownloadPDFFiles_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupDownUploads, ValueDownloadPDFFiles] = checkBoxDownloadPDFFiles.Checked;
        }



        // ************
        // Group "Exam"
        // ************
        private void buttonGenerateBrowserExamKey_Click(object sender, EventArgs e)
        {

        }

        private void textBoxBrowserExamKey_TextChanged(object sender, EventArgs e)
        {
            settingString[StateNew, GroupExam, ValueBrowserExamKey] = textBoxBrowserExamKey.Text;
        }

        private void checkBoxCopyBrowserExamKey_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupExam, ValueCopyBrowserExamKey] = checkBoxCopyBrowserExamKey.Checked;
        }

        private void checkBoxSendBrowserExamKey_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupExam, ValueSendBrowserExamKey] = checkBoxSendBrowserExamKey.Checked;
        }

        private void textBoxQuitURL_TextChanged(object sender, EventArgs e)
        {
            settingString[StateNew, GroupExam, ValueQuitURL] = textBoxQuitURL.Text;
        }



        // ********************
        // Group "Applications"
        // ********************
        private void checkBoxMonitorProcesses_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupApplications, ValueMonitorProcesses] = checkBoxMonitorProcesses.Checked;
        }

        private void checkBoxAllowSwitchToApplications_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupApplications, ValueAllowSwitchToApplications] = checkBoxAllowSwitchToApplications.Checked;
        }

        private void checkBoxAllowFlashFullscreenMode_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupApplications, ValueAllowFlashFullscreen] = checkBoxAllowFlashFullscreenMode.Checked;
        }



        // ***************
        // Group "Network"
        // ***************

        // ****************
        // Group "Security"
        // ****************
        private void listBoxSEBServicePolicy_SelectedIndexChanged(object sender, EventArgs e)
        {
            settingInteger[StateNew, GroupSecurity, ValueSebServicePolicy] = listBoxSEBServicePolicy.SelectedIndex + 1;
        }

        private void checkBoxAllowVirtualMachine_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupSecurity, ValueAllowVirtualMachine] = checkBoxAllowVirtualMachine.Checked;
        }

        private void checkBoxCreateNewDesktop_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupSecurity, ValueCreateNewDesktop] = checkBoxCreateNewDesktop.Checked;
        }

        private void checkBoxAllowUserSwitching_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupSecurity, ValueAllowUserSwitching] = checkBoxAllowUserSwitching.Checked;
        }

        private void checkBoxEnableLogging_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupSecurity, ValueEnableLog] = checkBoxEnableLogging.Checked;
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
            settingBoolean[StateNew, GroupInsideSeb, ValueEnableSwitchUser] = checkBoxInsideSebEnableSwitchUser.Checked;
        }

        private void checkBoxInsideSebEnableLockThisComputer_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupInsideSeb, ValueEnableLockThisComputer] = checkBoxInsideSebEnableLockThisComputer.Checked;
        }

        private void checkBoxInsideSebEnableChangeAPassword_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupInsideSeb, ValueEnableChangeAPassword] = checkBoxInsideSebEnableChangeAPassword.Checked;
        }

        private void checkBoxInsideSebEnableStartTaskManager_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupInsideSeb, ValueEnableStartTaskManager] = checkBoxInsideSebEnableStartTaskManager.Checked;
        }

        private void checkBoxInsideSebEnableLogOff_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupInsideSeb, ValueEnableLogOff] = checkBoxInsideSebEnableLogOff.Checked;
        }

        private void checkBoxInsideSebEnableShutDown_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupInsideSeb, ValueEnableShutDown] = checkBoxInsideSebEnableShutDown.Checked;
        }

        private void checkBoxInsideSebEnableEaseOfAccess_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupInsideSeb, ValueEnableEaseOfAccess] = checkBoxInsideSebEnableEaseOfAccess.Checked;
        }

        private void checkBoxInsideSebEnableVmWareClientShade_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupInsideSeb, ValueEnableVmWareClientShade] = checkBoxInsideSebEnableVmWareClientShade.Checked;
        }



        // *******************
        // Group "Outside SEB"
        // *******************
        private void checkBoxOutsideSebEnableSwitchUser_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupOutsideSeb, ValueEnableSwitchUser] = checkBoxOutsideSebEnableSwitchUser.Checked;
        }

        private void checkBoxOutsideSebEnableLockThisComputer_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupOutsideSeb, ValueEnableLockThisComputer] = checkBoxOutsideSebEnableLockThisComputer.Checked;
        }

        private void checkBoxOutsideSebEnableChangeAPassword_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupOutsideSeb, ValueEnableChangeAPassword] = checkBoxOutsideSebEnableChangeAPassword.Checked;
        }

        private void checkBoxOutsideSebEnableStartTaskManager_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupOutsideSeb, ValueEnableStartTaskManager] = checkBoxOutsideSebEnableStartTaskManager.Checked;
        }

        private void checkBoxOutsideSebEnableLogOff_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupOutsideSeb, ValueEnableLogOff] = checkBoxOutsideSebEnableLogOff.Checked;
        }

        private void checkBoxOutsideSebEnableShutDown_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupOutsideSeb, ValueEnableShutDown] = checkBoxOutsideSebEnableShutDown.Checked;
        }

        private void checkBoxOutsideSebEnableEaseOfAccess_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupOutsideSeb, ValueEnableEaseOfAccess] = checkBoxOutsideSebEnableEaseOfAccess.Checked;
        }

        private void checkBoxOutsideSebEnableVmWareClientShade_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupOutsideSeb, ValueEnableVmWareClientShade] = checkBoxOutsideSebEnableVmWareClientShade.Checked;
        }



        // *******************
        // Group "Hooked Keys"
        // *******************
        private void checkBoxHookMessages_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupHookedKeys, ValueHookMessages] = checkBoxHookMessages.Checked;
        }



        // ********************
        // Group "Special Keys"
        // ********************
        private void checkBoxEnableEsc_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupSpecialKeys, ValueEnableEsc] = checkBoxEnableEsc.Checked;
        }

        private void checkBoxEnableCtrlEsc_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupSpecialKeys, ValueEnableCtrlEsc] = checkBoxEnableCtrlEsc.Checked;
        }

        private void checkBoxEnableAltEsc_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupSpecialKeys, ValueEnableAltEsc] = checkBoxEnableAltEsc.Checked;
        }

        private void checkBoxEnableAltTab_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupSpecialKeys, ValueEnableAltTab] = checkBoxEnableAltTab.Checked;
        }

        private void checkBoxEnableAltF4_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupSpecialKeys, ValueEnableAltF4] = checkBoxEnableAltF4.Checked;
        }

        private void checkBoxEnableStartMenu_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupSpecialKeys, ValueEnableStartMenu] = checkBoxEnableStartMenu.Checked;
        }

        private void checkBoxEnableRightMouse_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupSpecialKeys, ValueEnableRightMouse] = checkBoxEnableRightMouse.Checked;
        }



        // *********************
        // Group "Function Keys"
        // *********************
        private void checkBoxEnableF1_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupFunctionKeys, ValueEnableF1] = checkBoxEnableF1.Checked;
        }

        private void checkBoxEnableF2_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupFunctionKeys, ValueEnableF2] = checkBoxEnableF2.Checked;
        }

        private void checkBoxEnableF3_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupFunctionKeys, ValueEnableF3] = checkBoxEnableF3.Checked;
        }

        private void checkBoxEnableF4_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupFunctionKeys, ValueEnableF4] = checkBoxEnableF4.Checked;
        }

        private void checkBoxEnableF5_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupFunctionKeys, ValueEnableF5] = checkBoxEnableF5.Checked;
        }

        private void checkBoxEnableF6_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupFunctionKeys, ValueEnableF6] = checkBoxEnableF6.Checked;
        }

        private void checkBoxEnableF7_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupFunctionKeys, ValueEnableF7] = checkBoxEnableF7.Checked;
        }

        private void checkBoxEnableF8_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupFunctionKeys, ValueEnableF8] = checkBoxEnableF8.Checked;
        }

        private void checkBoxEnableF9_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupFunctionKeys, ValueEnableF9] = checkBoxEnableF9.Checked;
        }

        private void checkBoxEnableF10_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupFunctionKeys, ValueEnableF10] = checkBoxEnableF10.Checked;
        }

        private void checkBoxEnableF11_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupFunctionKeys, ValueEnableF11] = checkBoxEnableF11.Checked;
        }

        private void checkBoxEnableF12_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupFunctionKeys, ValueEnableF12] = checkBoxEnableF12.Checked;
        }





        // ***************************************************
        // Set the new settings of a file to the desired state
        // ***************************************************
        private void SetNewSettingsOfFileToState(int stateDesired)
        {
            int group, value;

            // Restore the desired values by copying them to the new values
            for (group = 1; group <= GroupNum; group++)
            for (value = 1; value <= ValueNum; value++)
            {
                settingBoolean[StateNew, group, value] = settingBoolean[stateDesired, group, value];
                settingString [StateNew, group, value] = settingString [stateDesired, group, value];
                settingInteger[StateNew, group, value] = settingInteger[stateDesired, group, value];
            }
        }



        // *****************************************************
        // Set the widgets to the new settings of SebStarter.ini
        // *****************************************************
        private void SetWidgetsToNewSettingsOfSebStarterIni()
        {
            // Update the filename in the title bar
            this.Text  = this.ProductName;
            this.Text += " - ";
            this.Text += currentPathSebStarterIni;

            textBoxStartURL    .Text = (String)sebSettingsNew[MessageStartURL];
          //textBoxSEBServerURL.Text = (String)sebSettingsNew[MessageSebServerURL];
            textBoxQuitURL     .Text = (String)sebSettingsNew[MessageQuitURL];

            checkBoxAllowDownUploads.Checked = (Boolean)sebSettingsNew[MessageAllowDownUploads];

            listBoxOpenLinksHTML.SelectedIndex = (int)sebSettingsNew[MessageNewBrowserWindowByLinkPolicy];

            // Update the widgets
            textBoxStartURL            .Text    = settingString [StateNew, GroupGeneral, ValueStartURL];
            textBoxSebServerURL        .Text    = settingString [StateNew, GroupGeneral, ValueSebServerURL];
            textBoxAdminPassword       .Text    = settingString [StateNew, GroupGeneral, ValueAdminPassword];
            textBoxConfirmAdminPassword.Text    = settingString [StateNew, GroupGeneral, ValueConfirmAdminPassword];
            checkBoxAllowQuit .Checked = settingBoolean[StateNew, GroupGeneral, ValueAllowQuit];
            checkBoxIgnoreQuitPassword .Checked = settingBoolean[StateNew, GroupGeneral, ValueIgnoreQuitPassword];
            textBoxQuitPassword        .Text    = settingString [StateNew, GroupGeneral, ValueQuitPassword];
            textBoxConfirmQuitPassword .Text    = settingString [StateNew, GroupGeneral, ValueConfirmQuitPassword];
            listBoxExitKey1.SelectedIndex       = settingInteger[StateNew, GroupGeneral, ValueExitKey1] - 1;
            listBoxExitKey2.SelectedIndex       = settingInteger[StateNew, GroupGeneral, ValueExitKey2] - 1;
            listBoxExitKey3.SelectedIndex       = settingInteger[StateNew, GroupGeneral, ValueExitKey3] - 1;

            radioButtonStartingAnExam    .Checked = (settingInteger[StateNew, GroupConfigFile, ValueSebPurpose] == 0);
            radioButtonConfiguringAClient.Checked = (settingInteger[StateNew, GroupConfigFile, ValueSebPurpose] == 1);
            checkBoxAllowPreferencesWindow  .Checked =  settingBoolean[StateNew, GroupConfigFile, ValueAllowPreferencesWindow];
          //comboBoxChooseIdentity.SelectedIndex  =  settingInteger[StateNew, GroupConfigFile, ValueChooseIdentity];
          //comboBoxChooseIdentity.SelectedIndex  =  0;
            textBoxSettingsPassword       .Text   =  settingString [StateNew, GroupConfigFile, ValueSettingsPassword];
            textBoxConfirmSettingsPassword.Text   =  settingString [StateNew, GroupConfigFile, ValueConfirmSettingsPassword];

            radioButtonUseBrowserWindow  .Checked = (settingInteger[StateNew, GroupAppearance, ValueBrowserViewMode] == 0);
            radioButtonUseFullScreenMode .Checked = (settingInteger[StateNew, GroupAppearance, ValueBrowserViewMode] == 1);
            checkBoxEnableBrowserWindowToolbar  .Checked =  settingBoolean[StateNew, GroupAppearance, ValueEnableBrowserWindowToolbar];
            checkBoxHideBrowserWindowToolbar .Checked =  settingBoolean[StateNew, GroupAppearance, ValueHideBrowserWindowToolbar];
            checkBoxShowMenuBar          .Checked =  settingBoolean[StateNew, GroupAppearance, ValueShowMenuBar];
            checkBoxShowTaskBar.Checked =  settingBoolean[StateNew, GroupAppearance, ValueShowTaskBar];

            comboBoxMainBrowserWindowWidth   .SelectedIndex = settingInteger[StateNew, GroupAppearance, ValueMainBrowserWindowWidth   ] - 1;
            comboBoxMainBrowserWindowHeight  .SelectedIndex = settingInteger[StateNew, GroupAppearance, ValueMainBrowserWindowHeight  ] - 1;
             listBoxMainBrowserWindowPositioning.SelectedIndex = settingInteger[StateNew, GroupAppearance, ValueMainBrowserWindowPositioning] - 1;

            comboBoxNewBrowserWindowWidth   .SelectedIndex = settingInteger[StateNew, GroupBrowser, ValueNewBrowserWindowByLinkWidth   ] - 1;
            comboBoxNewBrowserWindowHeight  .SelectedIndex = settingInteger[StateNew, GroupBrowser, ValueNewBrowserWindowByLinkHeight  ] - 1;
             listBoxNewBrowserWindowPositioning.SelectedIndex = settingInteger[StateNew, GroupBrowser, ValueNewBrowserWindowByLinkPositioning] - 1;

             listBoxOpenLinksHTML .SelectedIndex = settingInteger[StateNew, GroupBrowser, ValueNewBrowserWindowByLinkPolicy] - 1;
             listBoxOpenLinksJava .SelectedIndex = settingInteger[StateNew, GroupBrowser, ValueNewBrowserWindowByScriptPolicy] - 1;
            checkBoxBlockLinksHTML.Checked       = settingBoolean[StateNew, GroupBrowser, ValueNewBrowserWindowByLinkBlockForeign];
            checkBoxBlockLinksJava.Checked       = settingBoolean[StateNew, GroupBrowser, ValueNewBrowserWindowByScriptBlockForeign];

            checkBoxEnablePlugIns           .Checked = settingBoolean[StateNew, GroupBrowser, ValueEnablePlugins];
            checkBoxEnableJava              .Checked = settingBoolean[StateNew, GroupBrowser, ValueEnableJava];
            checkBoxEnableJavaScript        .Checked = settingBoolean[StateNew, GroupBrowser, ValueEnableJavaScript];
            checkBoxBlockPopupWindows       .Checked = settingBoolean[StateNew, GroupBrowser, ValueBlockPopupWindows];
            checkBoxAllowBrowsingBackForward.Checked = settingBoolean[StateNew, GroupBrowser, ValueAllowBrowsingBackForward];
            checkBoxUseSebWithoutBrowser    .Checked = settingBoolean[StateNew, GroupBrowser, ValueEnableSebBrowser];

            checkBoxAllowDownUploads  .Checked = settingBoolean[StateNew, GroupDownUploads, ValueAllowDownUploads];
            checkBoxOpenDownloads.Checked = settingBoolean[StateNew, GroupDownUploads, ValueOpenDownloads];
            checkBoxDownloadPDFFiles  .Checked = settingBoolean[StateNew, GroupDownUploads, ValueDownloadPDFFiles];
            labelDownloadDirectoryWin       .Text    = settingString [StateNew, GroupDownUploads, ValueDownloadDirectoryWin];
             listBoxChooseFileToUploadPolicy.SelectedIndex  = settingInteger[StateNew, GroupDownUploads, ValueChooseFileToUploadPolicy] - 1;

             textBoxBrowserExamKey    .Text    = settingString [StateNew, GroupExam, ValueBrowserExamKey];
             textBoxQuitURL           .Text    = settingString [StateNew, GroupExam, ValueQuitURL];
            checkBoxCopyBrowserExamKey.Checked = settingBoolean[StateNew, GroupExam, ValueCopyBrowserExamKey];
            checkBoxSendBrowserExamKey.Checked = settingBoolean[StateNew, GroupExam, ValueSendBrowserExamKey];

            checkBoxMonitorProcesses         .Checked = settingBoolean[StateNew, GroupApplications, ValueMonitorProcesses];
            checkBoxAllowSwitchToApplications.Checked = settingBoolean[StateNew, GroupApplications, ValueAllowSwitchToApplications];
            checkBoxAllowFlashFullscreenMode .Checked = settingBoolean[StateNew, GroupApplications, ValueAllowFlashFullscreen];

             listBoxSEBServicePolicy.SelectedIndex = settingInteger[StateNew, GroupSecurity, ValueSebServicePolicy] - 1;
            checkBoxAllowVirtualMachine.Checked    = settingBoolean[StateNew, GroupSecurity, ValueAllowVirtualMachine];
            checkBoxCreateNewDesktop   .Checked    = settingBoolean[StateNew, GroupSecurity, ValueCreateNewDesktop];
            checkBoxAllowUserSwitching .Checked    = settingBoolean[StateNew, GroupSecurity, ValueAllowUserSwitching];
            checkBoxEnableLogging      .Checked    = settingBoolean[StateNew, GroupSecurity, ValueEnableLog];

            checkBoxInsideSebEnableSwitchUser       .Checked = settingBoolean[StateNew, GroupInsideSeb, ValueEnableSwitchUser];
            checkBoxInsideSebEnableLockThisComputer .Checked = settingBoolean[StateNew, GroupInsideSeb, ValueEnableLockThisComputer];
            checkBoxInsideSebEnableChangeAPassword  .Checked = settingBoolean[StateNew, GroupInsideSeb, ValueEnableChangeAPassword];
            checkBoxInsideSebEnableStartTaskManager .Checked = settingBoolean[StateNew, GroupInsideSeb, ValueEnableStartTaskManager];
            checkBoxInsideSebEnableLogOff           .Checked = settingBoolean[StateNew, GroupInsideSeb, ValueEnableLogOff];
            checkBoxInsideSebEnableShutDown         .Checked = settingBoolean[StateNew, GroupInsideSeb, ValueEnableShutDown];
            checkBoxInsideSebEnableEaseOfAccess     .Checked = settingBoolean[StateNew, GroupInsideSeb, ValueEnableEaseOfAccess];
            checkBoxInsideSebEnableVmWareClientShade.Checked = settingBoolean[StateNew, GroupInsideSeb, ValueEnableVmWareClientShade];

            checkBoxOutsideSebEnableSwitchUser       .Checked = settingBoolean[StateNew, GroupOutsideSeb, ValueEnableSwitchUser];
            checkBoxOutsideSebEnableLockThisComputer .Checked = settingBoolean[StateNew, GroupOutsideSeb, ValueEnableLockThisComputer];
            checkBoxOutsideSebEnableChangeAPassword  .Checked = settingBoolean[StateNew, GroupOutsideSeb, ValueEnableChangeAPassword];
            checkBoxOutsideSebEnableStartTaskManager .Checked = settingBoolean[StateNew, GroupOutsideSeb, ValueEnableStartTaskManager];
            checkBoxOutsideSebEnableLogOff           .Checked = settingBoolean[StateNew, GroupOutsideSeb, ValueEnableLogOff];
            checkBoxOutsideSebEnableShutDown         .Checked = settingBoolean[StateNew, GroupOutsideSeb, ValueEnableShutDown];
            checkBoxOutsideSebEnableEaseOfAccess     .Checked = settingBoolean[StateNew, GroupOutsideSeb, ValueEnableEaseOfAccess];
            checkBoxOutsideSebEnableVmWareClientShade.Checked = settingBoolean[StateNew, GroupOutsideSeb, ValueEnableVmWareClientShade];

            checkBoxHookMessages.Checked = settingBoolean[StateNew, GroupHookedKeys, ValueHookMessages];

            checkBoxEnableEsc       .Checked = settingBoolean[StateNew, GroupSpecialKeys, ValueEnableEsc];
            checkBoxEnableCtrlEsc   .Checked = settingBoolean[StateNew, GroupSpecialKeys, ValueEnableCtrlEsc];
            checkBoxEnableAltEsc    .Checked = settingBoolean[StateNew, GroupSpecialKeys, ValueEnableAltEsc];
            checkBoxEnableAltTab    .Checked = settingBoolean[StateNew, GroupSpecialKeys, ValueEnableAltTab];
            checkBoxEnableAltF4     .Checked = settingBoolean[StateNew, GroupSpecialKeys, ValueEnableAltF4];
            checkBoxEnableStartMenu .Checked = settingBoolean[StateNew, GroupSpecialKeys, ValueEnableStartMenu];
            checkBoxEnableRightMouse.Checked = settingBoolean[StateNew, GroupSpecialKeys, ValueEnableRightMouse];

            checkBoxEnableF1 .Checked = settingBoolean[StateNew, GroupFunctionKeys, ValueEnableF1];
            checkBoxEnableF2 .Checked = settingBoolean[StateNew, GroupFunctionKeys, ValueEnableF2];
            checkBoxEnableF3 .Checked = settingBoolean[StateNew, GroupFunctionKeys, ValueEnableF3];
            checkBoxEnableF4 .Checked = settingBoolean[StateNew, GroupFunctionKeys, ValueEnableF4];
            checkBoxEnableF5 .Checked = settingBoolean[StateNew, GroupFunctionKeys, ValueEnableF5];
            checkBoxEnableF6 .Checked = settingBoolean[StateNew, GroupFunctionKeys, ValueEnableF6];
            checkBoxEnableF7 .Checked = settingBoolean[StateNew, GroupFunctionKeys, ValueEnableF7];
            checkBoxEnableF8 .Checked = settingBoolean[StateNew, GroupFunctionKeys, ValueEnableF8];
            checkBoxEnableF9 .Checked = settingBoolean[StateNew, GroupFunctionKeys, ValueEnableF9];
            checkBoxEnableF10.Checked = settingBoolean[StateNew, GroupFunctionKeys, ValueEnableF10];
            checkBoxEnableF11.Checked = settingBoolean[StateNew, GroupFunctionKeys, ValueEnableF11];
            checkBoxEnableF12.Checked = settingBoolean[StateNew, GroupFunctionKeys, ValueEnableF12];
        }





    } // end of   class     SebWindowsConfigForm
}     // end of   namespace SebWindowsConfig
