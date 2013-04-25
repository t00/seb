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

        // The Graphical User Interface contains 17 groups
        const int GroupNum = 17;

        // SebStarter contains 17 groups
        const int GroupGeneral         = 1;
        const int GroupConfigFile      = 2;
        const int GroupAppearance      = 3;
        const int GroupBrowser         = 4;
        const int GroupDownUploads     = 5;
        const int GroupExam            = 6;
        const int GroupApplications    = 7;
        const int GroupNetwork         = 8;
        const int GroupSecurity        = 9;
        const int GroupRegistry        = 10;
        const int GroupInsideSeb       = 11;
        const int GroupOutsideSeb      = 12;
        const int GroupHookedKeys      = 13;
        const int GroupSpecialKeys     = 14;
        const int GroupFunctionKeys    = 15;
        const int GroupExitKeys        = 16;
        const int GroupSecurityOptions = 17;

        const int GroupNumSebStarter = 17;

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
        const String MessageExitKeys     = "ExitKeys";
        const String MessageSecurityOptions = "SecurityOptions";

        // Group "General"
        const int ValueStartURL             = 1;
        const int ValueSEBServerURL         = 2;
        const int ValueAdminPassword        = 3;
        const int ValueConfirmAdminPassword = 4;
        const int ValueHashedAdminPassword  = 5;
        const int ValueAllowUserToQuitSEB   = 6;
        const int ValueQuitPassword         = 7;
        const int ValueConfirmQuitPassword  = 8;
        const int ValueHashedQuitPassword   = 9;
        const int NumValueGeneral = 9;

        const String MessageStartURL             = "StartURL";
        const String MessageSEBServerURL         = "SEBServerURL";
        const String MessageAdminPassword        = "AdminPassword";
        const String MessageConfirmAdminPassword = "ConfirmAdminPassword";
        const String MessageHashedAdminPassword  = "HashedAdminPassword";
        const String MessageAllowUserToQuitSEB   = "AllowUserToQuitSEB";
        const String MessageQuitPassword         = "QuitPassword";
        const String MessageConfirmQuitPassword  = "ConfirmQuitPassword";
        const String MessageHashedQuitPassword   = "HashedQuitPassword";

        // Group "Config File"
        const int ValueStartingAnExam          = 1;
        const int ValueConfiguringAClient      = 2;
        const int ValueAllowOpenPrefWindow     = 3;
        const int ValueChooseIdentity          = 4;
        const int ValueSettingsPassword        = 5;
        const int ValueConfirmSettingsPassword = 6;
        const int ValueHashedSettingsPassword  = 7;
        const int NumValueConfigFile = 7;

        const String MessageStartingAnExam          = "StartingAnExam";
        const String MessageConfiguringAClient      = "ConfiguringAClient";
        const String MessageAllowOpenPrefWindow     = "AllowOpenPrefWindow";
        const String MessageChooseIdentity          = "ChooseIdentity";
        const String MessageSettingsPassword        = "SettingsPassword";
        const String MessageConfirmSettingsPassword = "ConfirmSettingsPassword";
        const String MessageHashedSettingsPassword  = "HashedSettingsPassword";

        // Group "Appearance"
        const int ValueUseBrowserWindow      = 1;
        const int ValueUseFullScreenMode     = 2;
        const int ValueMainWindowWidth       = 3;
        const int ValueMainWindowHeight      = 4;
        const int ValueMainWindowPosition    = 5;
        const int ValueEnableWindowToolbar   = 6;
        const int ValueHideToolbarAsDefault  = 7;
        const int ValueShowMenuBar           = 8;
        const int ValueDisplaySEBDockTaskBar = 9;
        const int NumValueAppearance = 9;

        const String MessageUseBrowserWindow      = "UseBrowserWindow";
        const String MessageUseFullScreenMode     = "UseFullScreenMode";
        const String MessageMainWindowWidth       = "MainWindowWidth";
        const String MessageMainWindowHeight      = "MainWindowHeight";
        const String MessageMainWindowPosition    = "MainWindowPosition";
        const String MessageEnableWindowToolbar   = "EnableWindowToolbar";
        const String MessageHideToolbarAsDefault  = "HideToolbarAsDefault";
        const String MessageShowMenuBar           = "ShowMenuBar";
        const String MessageDisplaySEBDockTaskBar = "DisplaySEBDockTaskBar";

        // Group "Browser"
        const int ValueOpenLinksHTML            = 1;
        const int ValueOpenLinksJava            = 2;
        const int ValueBlockLinksHTML           = 3;
        const int ValueBlockLinksJava           = 4;
        const int ValueNewWindowWidth           = 5;
        const int ValueNewWindowHeight          = 6;
        const int ValueNewWindowPosition        = 7;
        const int ValueEnablePlugins            = 8;
        const int ValueEnableJava               = 9;
        const int ValueEnableJavaScript         = 10;
        const int ValueBlockPopupWindows        = 11;
        const int ValueAllowBrowsingBackForward = 12;
        const int ValueUseSEBWithoutBrowser     = 13;
        const int NumValueBrowser = 13;

        const String MessageOpenLinksHTML            = "OpenLinksHTML";
        const String MessageOpenLinksJava            = "OpenLinksJava";
        const String MessageBlockLinksHTML           = "BlockLinksHTML";
        const String MessageBlockLinksJava           = "BlockLinksJava";
        const String MessageNewWindowWidth           = "NewWindowWidth";
        const String MessageNewWindowHeight          = "NewWindowHeight";
        const String MessageNewWindowPosition        = "NewWindowPosition";
        const String MessageEnablePlugins            = "EnablePlugins";
        const String MessageEnableJava               = "EnableJava";
        const String MessageEnableJavaScript         = "EnableJavaScript";
        const String MessageBlockPopupWindows        = "BlockPopupWindows";
        const String MessageAllowBrowsingBackForward = "AllowBrowsingBackForward";
        const String MessageUseSEBWithoutBrowser     = "UseSEBWithoutBrowser";

        // Group "DownUploads"
        const int ValueAllowDownUploadingFiles   = 1;
        const int ValueSaveDownloadedFilesTo     = 2;
        const int ValueOpenFilesAfterDownloading = 3;
        const int ValueChooseFileToUpload        = 4;
        const int ValueDownloadAndOpenPDFFiles   = 5;
        const int NumValueDownUploads = 5;

        const String MessageAllowDownUploadingFiles   = "AllowDownUploadingFiles";
        const String MessageSaveDownloadedFilesTo     = "SaveDownloadedFilesTo";
        const String MessageOpenFilesAfterDownloading = "OpenFilesAfterDownloading";
        const String MessageChooseFileToUpload        = "ChooseFileToUpload";
        const String MessageDownloadAndOpenPDFFiles   = "DownloadAndOpenPDFFiles";

        // Group "Exam"
        const int ValueBrowserExamKey     = 1;
        const int ValueCopyBrowserExamKey = 2;
        const int ValueSendBrowserExamKey = 3;
        const int ValueQuitURL            = 4;
        const int NumValueExam = 4;

        const String MessageBrowserExamKey     = "BrowserExamKey";
        const String MessageCopyBrowserExamKey = "CopyBrowserExamKey";
        const String MessageSendBrowserExamKey = "SendBrowserExamKey";
        const String MessageQuitURL            = "QuitURL";

        // Group "Applications"
        const int ValueMonitorProcesses          = 1;
        const int ValueAllowSwitchToApplications = 2;
        const int ValueAllowFlashFullscreenMode  = 3;
        const int NumValueApplications = 3;

        const String MessageMonitorProcesses          = "MonitorProcesses";
        const String MessageAllowSwitchToApplications = "AllowSwitchToApplications";
        const String MessageAllowFlashFullscreenMode  = "AllowFlashFullscreenMode";

        // Group "Network"
        //const int Value = 1;
        const int NumValueNetwork = 0;

        // Group "Security"
        const int ValueSEBServicePolicy    = 1;
        const int ValueAllowVirtualMachine = 2;
        const int ValueEnableLogging       = 3;
        const int NumValueSecurity = 3;

        const String MessageSEBServicePolicy    = "SEBServicePolicy";
        const String MessageAllowVirtualMachine = "AllowVirtualMachine";
        const String MessageEnableLogging       = "EnableLogging";

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

        const String MessageInsideSebEnableSwitchUser        = "InsideSebEnableSwitchUser";
        const String MessageInsideSebEnableLockThisComputer  = "InsideSebEnableLockThisComputer";
        const String MessageInsideSebEnableChangeAPassword   = "InsideSebEnableChangeAPassword";
        const String MessageInsideSebEnableStartTaskManager  = "InsideSebEnableStartTaskManager";
        const String MessageInsideSebEnableLogOff            = "InsideSebEnableLogOff";
        const String MessageInsideSebEnableShutDown          = "InsideSebEnableShutDown";
        const String MessageInsideSebEnableEaseOfAccess      = "InsideSebEnableEaseOfAccess";
        const String MessageInsideSebEnableVmWareClientShade = "InsideSebEnableVmWareClientShade";

        const String MessageOutsideSebEnableSwitchUser        = "OutsideSebEnableSwitchUser";
        const String MessageOutsideSebEnableLockThisComputer  = "OutsideSebEnableLockThisComputer";
        const String MessageOutsideSebEnableChangeAPassword   = "OutsideSebEnableChangeAPassword";
        const String MessageOutsideSebEnableStartTaskManager  = "OutsideSebEnableStartTaskManager";
        const String MessageOutsideSebEnableLogOff            = "OutsideSebEnableLogOff";
        const String MessageOutsideSebEnableShutDown          = "OutsideSebEnableShutDown";
        const String MessageOutsideSebEnableEaseOfAccess      = "OutsideSebEnableEaseOfAccess";
        const String MessageOutsideSebEnableVmWareClientShade = "OutsideSebEnableVmWareClientShade";

        // Group "HookedKeys"
        const int ValueHookMessages = 1;
        const int NumValueHookedKeys = 1;

        const String MessageHookMessages = "HookMessages";

        // Group "Special Keys"
        const int ValueEnableEsc        = 1;
        const int ValueEnableCtrlEsc    = 2;
        const int ValueEnableAltEsc     = 3;
        const int ValueEnableAltTab     = 4;
        const int ValueEnableAltF4      = 5;
        const int ValueEnableStartMenu  = 6;
        const int ValueEnableRightMouse = 7;
        const int NumValueSpecialKeys = 7;

        const String MessageEnableEsc        = "EnableEsc";
        const String MessageEnableCtrlEsc    = "EnableCtrlEsc";
        const String MessageEnableAltEsc     = "EnableAltEsc";
        const String MessageEnableAltTab     = "EnableAltTab";
        const String MessageEnableAltF4      = "EnableAltF4";
        const String MessageEnableStartMenu  = "EnableStartMenu";
        const String MessageEnableRightMouse = "EnableRightMouse";

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

        const String MessageEnableF1  = "EnableF1";
        const String MessageEnableF2  = "EnableF2";
        const String MessageEnableF3  = "EnableF3";
        const String MessageEnableF4  = "EnableF4";
        const String MessageEnableF5  = "EnableF5";
        const String MessageEnableF6  = "EnableF6";
        const String MessageEnableF7  = "EnableF7";
        const String MessageEnableF8  = "EnableF8";
        const String MessageEnableF9  = "EnableF9";
        const String MessageEnableF10 = "EnableF10";
        const String MessageEnableF11 = "EnableF11";
        const String MessageEnableF12 = "EnableF12";

        // Group "Exit Keys"
        const int ValueExitKey1 = 1;
        const int ValueExitKey2 = 2;
        const int ValueExitKey3 = 3;
        const int NumValueExitKeys = 3;

        const String MessageExitKey1 = "ExitKey1";
        const String MessageExitKey2 = "ExitKey2";
        const String MessageExitKey3 = "ExitKey3";

        // Group "Security Options"
        const int ValueCreateNewDesktop   = 1;
        const int ValueIgnoreQuitPassword = 2;
        const int NumValueSecurityOptions = 2;

        const String MessageCreateNewDesktop   = "CreateNewDesktop";
        const String MessageIgnoreQuitPassword = "IgnoreQuitPassword";


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
        static List<String> chooseIdentityStringList = new List<String>();

        // Entries of ListBoxes
        static String[] StringWindowWidth       = new String[5];
        static String[] StringWindowHeight      = new String[5];
        static String[] StringWindowPosition    = new String[4];
        static String[] StringFileToUpload      = new String[4];
        static String[] StringPolicyLinkOpening = new String[4];
        static String[] StringPolicySEBService  = new String[4];
        static String[] StringFunctionKey       = new String[13];

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
        static SEBClientConfig         sebSettings   = new SEBClientConfig();
        static SEBProtectionController sebController = new SEBProtectionController();
        static XmlSerializer           sebSerializer = new XmlSerializer(typeof(SEBClientConfig));



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
            settingString [StateDef, GroupGeneral, ValueSEBServerURL        ] = "";
            settingString [StateDef, GroupGeneral, ValueAdminPassword       ] = "";
            settingString [StateDef, GroupGeneral, ValueConfirmAdminPassword] = "";
            settingBoolean[StateDef, GroupGeneral, ValueAllowUserToQuitSEB  ] = true;
            settingString [StateDef, GroupGeneral, ValueQuitPassword        ] = "";
            settingString [StateDef, GroupGeneral, ValueConfirmQuitPassword ] = "";
            settingString [StateDef, GroupGeneral, ValueHashedQuitPassword        ] = "";

            // Default settings for group "Config File"
            settingBoolean[StateDef, GroupConfigFile, ValueStartingAnExam         ] = true;
            settingBoolean[StateDef, GroupConfigFile, ValueConfiguringAClient     ] = false;
            settingBoolean[StateDef, GroupConfigFile, ValueAllowOpenPrefWindow    ] = true;
            settingString [StateDef, GroupConfigFile, ValueChooseIdentity         ] = "none";
            settingString [StateDef, GroupConfigFile, ValueSettingsPassword       ] = "";
            settingString [StateDef, GroupConfigFile, ValueConfirmSettingsPassword] = "";

            settingInteger[StateDef, GroupConfigFile, ValueChooseIdentity] = 0;

            // Default settings for group "Appearance"
            settingBoolean[StateDef, GroupAppearance, ValueUseBrowserWindow ] = true;
            settingBoolean[StateDef, GroupAppearance, ValueUseFullScreenMode] = false;

            settingString [StateDef, GroupAppearance, ValueMainWindowWidth   ] = "100%";
            settingString [StateDef, GroupAppearance, ValueMainWindowHeight  ] = "100%";
            settingString [StateDef, GroupAppearance, ValueMainWindowPosition] = "Center";

            settingBoolean[StateDef, GroupAppearance, ValueEnableWindowToolbar  ] = false;
            settingBoolean[StateDef, GroupAppearance, ValueHideToolbarAsDefault ] = false;
            settingBoolean[StateDef, GroupAppearance, ValueShowMenuBar          ] = false;
            settingBoolean[StateDef, GroupAppearance, ValueDisplaySEBDockTaskBar] = false;

            settingInteger[StateDef, GroupAppearance, ValueMainWindowWidth   ] = 0;
            settingInteger[StateDef, GroupAppearance, ValueMainWindowHeight  ] = 0;
            settingInteger[StateDef, GroupAppearance, ValueMainWindowPosition] = 2;

            // Default settings for group "Browser"
            settingString [StateDef, GroupBrowser, ValueOpenLinksHTML ] = "open in new window";
            settingString [StateDef, GroupBrowser, ValueOpenLinksJava ] = "open in new window";
            settingBoolean[StateDef, GroupBrowser, ValueBlockLinksHTML] = false;
            settingBoolean[StateDef, GroupBrowser, ValueBlockLinksJava] = false;

            settingString [StateDef, GroupBrowser, ValueNewWindowWidth   ] = "100%";
            settingString [StateDef, GroupBrowser, ValueNewWindowHeight  ] = "100%";
            settingString [StateDef, GroupBrowser, ValueNewWindowPosition] = "Center";

            settingBoolean[StateDef, GroupBrowser, ValueEnablePlugins           ] = true;
            settingBoolean[StateDef, GroupBrowser, ValueEnableJava              ] = false;
            settingBoolean[StateDef, GroupBrowser, ValueEnableJavaScript        ] = true;
            settingBoolean[StateDef, GroupBrowser, ValueBlockPopupWindows       ] = false;
            settingBoolean[StateDef, GroupBrowser, ValueAllowBrowsingBackForward] = false;
            settingBoolean[StateDef, GroupBrowser, ValueUseSEBWithoutBrowser    ] = false;

            settingInteger[StateDef, GroupBrowser, ValueOpenLinksHTML] = 1;
            settingInteger[StateDef, GroupBrowser, ValueOpenLinksJava] = 1;

            settingInteger[StateDef, GroupBrowser, ValueNewWindowWidth   ] = 0;
            settingInteger[StateDef, GroupBrowser, ValueNewWindowHeight  ] = 0;
            settingInteger[StateDef, GroupBrowser, ValueNewWindowPosition] = 2;

            // Default settings for group "DownUploads"
            settingBoolean[StateDef, GroupDownUploads, ValueAllowDownUploadingFiles  ] = true;
            settingString [StateDef, GroupDownUploads, ValueSaveDownloadedFilesTo    ] = "Desktop";
            settingBoolean[StateDef, GroupDownUploads, ValueOpenFilesAfterDownloading] = true;
            settingString [StateDef, GroupDownUploads, ValueChooseFileToUpload       ] = "manually with file requester";
            settingBoolean[StateDef, GroupDownUploads, ValueDownloadAndOpenPDFFiles  ] = false;

            settingInteger[StateDef, GroupDownUploads, ValueChooseFileToUpload] = 1;

            // Default settings for group "Exam"
            settingString [StateDef, GroupExam, ValueBrowserExamKey    ] = "";
            settingBoolean[StateDef, GroupExam, ValueCopyBrowserExamKey] = false;
            settingBoolean[StateDef, GroupExam, ValueSendBrowserExamKey] = true;
            settingString [StateDef, GroupExam, ValueQuitURL           ] = "http://www.safeexambrowser.org/exit";

            // Default settings for group "Applications"
            settingBoolean[StateDef, GroupApplications, ValueMonitorProcesses         ] = false;
            settingBoolean[StateDef, GroupApplications, ValueAllowSwitchToApplications] = true;
            settingBoolean[StateDef, GroupApplications, ValueAllowFlashFullscreenMode ] = false;

            // Default settings for group "Network"

            // Default settings for group "Security"
            settingInteger[StateDef, GroupSecurity, ValueSEBServicePolicy   ] = 1;
            settingString [StateDef, GroupSecurity, ValueSEBServicePolicy   ] = "allow to use SEB only with service";
            settingBoolean[StateDef, GroupSecurity, ValueAllowVirtualMachine] = false;
            settingBoolean[StateDef, GroupSecurity, ValueEnableLogging      ] = true;

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
                settingBoolean[StateDef, GroupSecurityOptions, value] = false;
            }

            // Default settings for groups "Special Keys"
            settingBoolean[StateDef, GroupSpecialKeys , ValueEnableAltTab] = true;

            // Default settings for groups "Function Keys"
            settingBoolean[StateDef, GroupFunctionKeys, ValueEnableF5] = true;

            // Default settings for group "Exit Keys"
            settingInteger[StateDef, GroupExitKeys, ValueExitKey1] =  3;
            settingInteger[StateDef, GroupExitKeys, ValueExitKey2] = 11;
            settingInteger[StateDef, GroupExitKeys, ValueExitKey3] =  6;

            // Default settings for group "Security Options"
            settingBoolean[StateDef, GroupSecurityOptions, ValueCreateNewDesktop  ] = true;
            settingBoolean[StateDef, GroupSecurityOptions, ValueIgnoreQuitPassword] = false;


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
                dataType[GroupExitKeys    , value] = TypeString;
                dataType[GroupSecurityOptions, value] = TypeBoolean;
            }

            // Exceptional data types of some special values
            dataType[GroupGeneral, ValueAllowUserToQuitSEB] = TypeBoolean;

            dataType[GroupConfigFile, ValueChooseIdentity         ] = TypeString;
            dataType[GroupConfigFile, ValueSettingsPassword       ] = TypeString;
            dataType[GroupConfigFile, ValueConfirmSettingsPassword] = TypeString;

            dataType[GroupAppearance, ValueMainWindowWidth   ] = TypeString;
            dataType[GroupAppearance, ValueMainWindowHeight  ] = TypeString;
            dataType[GroupAppearance, ValueMainWindowPosition] = TypeString;

            dataType[GroupBrowser, ValueNewWindowWidth   ] = TypeString;
            dataType[GroupBrowser, ValueNewWindowHeight  ] = TypeString;
            dataType[GroupBrowser, ValueNewWindowPosition] = TypeString;

            dataType[GroupBrowser, ValueOpenLinksHTML] = TypeString;
            dataType[GroupBrowser, ValueOpenLinksJava] = TypeString;

            dataType[GroupDownUploads, ValueSaveDownloadedFilesTo] = TypeString;
            dataType[GroupDownUploads, ValueChooseFileToUpload   ] = TypeString;

            dataType[GroupExam, ValueBrowserExamKey] = TypeString;
            dataType[GroupExam, ValueQuitURL       ] = TypeString;

            dataType[GroupSecurity, ValueSEBServicePolicy] = TypeString;


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
            maxValue[GroupExitKeys    ] = NumValueExitKeys;
            maxValue[GroupSecurityOptions] = NumValueSecurityOptions;


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
            groupString[GroupExitKeys    ] = MessageExitKeys;
            groupString[GroupSecurityOptions] = MessageSecurityOptions;

            // Value names
            valueString[GroupGeneral, ValueStartURL            ] = MessageStartURL;
            valueString[GroupGeneral, ValueSEBServerURL        ] = MessageSEBServerURL;
            valueString[GroupGeneral, ValueAdminPassword       ] = MessageAdminPassword;
            valueString[GroupGeneral, ValueConfirmAdminPassword] = MessageConfirmAdminPassword;
            valueString[GroupGeneral, ValueAllowUserToQuitSEB  ] = MessageAllowUserToQuitSEB;
            valueString[GroupGeneral, ValueQuitPassword        ] = MessageQuitPassword;
            valueString[GroupGeneral, ValueConfirmQuitPassword ] = MessageConfirmQuitPassword;
            valueString[GroupGeneral, ValueHashedQuitPassword        ] = MessageHashedQuitPassword;

            valueString[GroupConfigFile, ValueStartingAnExam         ] = MessageStartingAnExam;
            valueString[GroupConfigFile, ValueConfiguringAClient     ] = MessageConfiguringAClient;
            valueString[GroupConfigFile, ValueAllowOpenPrefWindow    ] = MessageAllowOpenPrefWindow;
            valueString[GroupConfigFile, ValueChooseIdentity         ] = MessageChooseIdentity;
            valueString[GroupConfigFile, ValueSettingsPassword       ] = MessageSettingsPassword;
            valueString[GroupConfigFile, ValueConfirmSettingsPassword] = MessageConfirmSettingsPassword;

            valueString[GroupAppearance, ValueUseBrowserWindow     ] = MessageUseBrowserWindow;
            valueString[GroupAppearance, ValueUseFullScreenMode    ] = MessageUseFullScreenMode;
            valueString[GroupAppearance, ValueMainWindowWidth      ] = MessageMainWindowWidth;
            valueString[GroupAppearance, ValueMainWindowHeight     ] = MessageMainWindowHeight;
            valueString[GroupAppearance, ValueMainWindowPosition   ] = MessageMainWindowPosition;
            valueString[GroupAppearance, ValueEnableWindowToolbar  ] = MessageEnableWindowToolbar;
            valueString[GroupAppearance, ValueHideToolbarAsDefault ] = MessageHideToolbarAsDefault;
            valueString[GroupAppearance, ValueShowMenuBar          ] = MessageShowMenuBar;
            valueString[GroupAppearance, ValueDisplaySEBDockTaskBar] = MessageDisplaySEBDockTaskBar;

            valueString[GroupBrowser, ValueOpenLinksHTML           ] = MessageOpenLinksHTML;
            valueString[GroupBrowser, ValueOpenLinksJava           ] = MessageOpenLinksJava;
            valueString[GroupBrowser, ValueBlockLinksHTML          ] = MessageBlockLinksHTML;
            valueString[GroupBrowser, ValueBlockLinksJava          ] = MessageBlockLinksJava;
            valueString[GroupBrowser, ValueNewWindowWidth          ] = MessageNewWindowWidth;
            valueString[GroupBrowser, ValueNewWindowHeight         ] = MessageNewWindowHeight;
            valueString[GroupBrowser, ValueNewWindowPosition       ] = MessageNewWindowPosition;
            valueString[GroupBrowser, ValueEnablePlugins           ] = MessageEnablePlugins;
            valueString[GroupBrowser, ValueEnableJava              ] = MessageEnableJava;
            valueString[GroupBrowser, ValueEnableJavaScript        ] = MessageEnableJavaScript;
            valueString[GroupBrowser, ValueBlockPopupWindows       ] = MessageBlockPopupWindows;
            valueString[GroupBrowser, ValueAllowBrowsingBackForward] = MessageAllowBrowsingBackForward;
            valueString[GroupBrowser, ValueUseSEBWithoutBrowser    ] = MessageUseSEBWithoutBrowser;

            valueString[GroupDownUploads, ValueAllowDownUploadingFiles  ] = MessageAllowDownUploadingFiles;
            valueString[GroupDownUploads, ValueSaveDownloadedFilesTo    ] = MessageSaveDownloadedFilesTo;
            valueString[GroupDownUploads, ValueOpenFilesAfterDownloading] = MessageOpenFilesAfterDownloading;
            valueString[GroupDownUploads, ValueChooseFileToUpload       ] = MessageChooseFileToUpload;
            valueString[GroupDownUploads, ValueDownloadAndOpenPDFFiles  ] = MessageDownloadAndOpenPDFFiles;

            valueString[GroupExam, ValueBrowserExamKey    ] = MessageBrowserExamKey;
            valueString[GroupExam, ValueCopyBrowserExamKey] = MessageCopyBrowserExamKey;
            valueString[GroupExam, ValueSendBrowserExamKey] = MessageSendBrowserExamKey;
            valueString[GroupExam, ValueQuitURL           ] = MessageQuitURL;

            valueString[GroupApplications, ValueMonitorProcesses         ] = MessageMonitorProcesses;
            valueString[GroupApplications, ValueAllowSwitchToApplications] = MessageAllowSwitchToApplications;
            valueString[GroupApplications, ValueAllowFlashFullscreenMode ] = MessageAllowFlashFullscreenMode;

            valueString[GroupSecurity, ValueSEBServicePolicy   ] = MessageSEBServicePolicy;
            valueString[GroupSecurity, ValueAllowVirtualMachine] = MessageAllowVirtualMachine;
            valueString[GroupSecurity, ValueEnableLogging      ] = MessageEnableLogging;

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

            valueString[GroupExitKeys, ValueExitKey1] = MessageExitKey1;
            valueString[GroupExitKeys, ValueExitKey2] = MessageExitKey2;
            valueString[GroupExitKeys, ValueExitKey3] = MessageExitKey3;

            valueString[GroupSecurityOptions, ValueCreateNewDesktop  ] = MessageCreateNewDesktop;
            valueString[GroupSecurityOptions, ValueIgnoreQuitPassword] = MessageIgnoreQuitPassword;


            // Define the strings for the encryption identity
            chooseIdentityStringList.Add("none");
            chooseIdentityStringList.Add("alpha");
            chooseIdentityStringList.Add("beta");
            chooseIdentityStringList.Add("gamma");
            chooseIdentityStringList.Add("delta");
            String[] chooseIdentityStringArray = chooseIdentityStringList.ToArray();

            // Define the strings for the window width
            StringWindowWidth[0] = "";
            StringWindowWidth[1] = "50%";
            StringWindowWidth[2] = "100%";
            StringWindowWidth[3] = "800";
            StringWindowWidth[4] = "1000";

            // Define the strings for the window height
            StringWindowHeight[0] = "";
            StringWindowHeight[1] = "80%";
            StringWindowHeight[2] = "100%";
            StringWindowHeight[3] = "600";
            StringWindowHeight[4] = "800";

            // Define the strings for the window position
            StringWindowPosition[0] = "";
            StringWindowPosition[1] = "Left";
            StringWindowPosition[2] = "Center";
            StringWindowPosition[3] = "Right";

            // Define the strings for the link opening policy
            StringPolicyLinkOpening[0] = "";
            StringPolicyLinkOpening[1] = "open in new window";
            StringPolicyLinkOpening[2] = "open in same window";
            StringPolicyLinkOpening[3] = "get generally blocked";

            // Define the strings for the SEB service policy
            StringPolicySEBService[0] = "";
            StringPolicySEBService[1] = "allow to use SEB only with service";
            StringPolicySEBService[2] = "display warning when service is not running";
            StringPolicySEBService[3] = "allow to run SEB without service";

            // Define the strings for the choosing a file to upload
            StringFileToUpload[0] = "";
            StringFileToUpload[1] = "manually with file requester";
            StringFileToUpload[2] = "by attempting to upload the same file downloaded before";
            StringFileToUpload[3] = "by only allowing to upload the same file downloaded before";

            // Define the strings for the function keys F1, F2, ..., F12
            for (int i = 1; i <= 12; i++)
            {
                StringFunctionKey[i] = "F" + i.ToString();
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
            comboBoxChooseIdentity.Items.AddRange(chooseIdentityStringArray);
            comboBoxChooseIdentity.SelectedIndex = 0;

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
            settingInteger[StateTmp, GroupConfigFile, ValueChooseIdentity] = 0;

            // These ListBox and ComboBox entries need a conversion from string to integer:
            //
            // Main Window Width/Height/Position
            // New  Window Width/Height/Position
            // Link Opening Policy for HTML/JavaScript
            // SEB  Service Policy
            // Choose File To Upload
            // Exit Key Sequence (exit keys 1,2,3)

            String tmpStringMainWindowWidth    = settingString[StateTmp, GroupAppearance, ValueMainWindowWidth];
            String tmpStringMainWindowHeight   = settingString[StateTmp, GroupAppearance, ValueMainWindowHeight];
            String tmpStringMainWindowPosition = settingString[StateTmp, GroupAppearance, ValueMainWindowPosition];

            String tmpStringNewWindowWidth    = settingString[StateTmp, GroupBrowser, ValueNewWindowWidth];
            String tmpStringNewWindowHeight   = settingString[StateTmp, GroupBrowser, ValueNewWindowHeight];
            String tmpStringNewWindowPosition = settingString[StateTmp, GroupBrowser, ValueNewWindowPosition];

            String tmpStringLinksInHTML = settingString[StateTmp, GroupBrowser, ValueOpenLinksHTML];
            String tmpStringLinksInJava = settingString[StateTmp, GroupBrowser, ValueOpenLinksJava];

            String tmpStringSEBServicePolicy   = settingString[StateTmp, GroupSecurity   , ValueSEBServicePolicy];
            String tmpStringChooseFileToUpload = settingString[StateTmp, GroupDownUploads, ValueChooseFileToUpload];

            String tmpStringExitKey1 = settingString[StateTmp, GroupExitKeys, ValueExitKey1];
            String tmpStringExitKey2 = settingString[StateTmp, GroupExitKeys, ValueExitKey2];
            String tmpStringExitKey3 = settingString[StateTmp, GroupExitKeys, ValueExitKey3];

            int index;

            int tmpIndexMainWindowWidth    = 0;
            int tmpIndexMainWindowHeight   = 0;
            int tmpIndexMainWindowPosition = 0;

            int tmpIndexNewWindowWidth    = 0;
            int tmpIndexNewWindowHeight   = 0;
            int tmpIndexNewWindowPosition = 0;

            int tmpIndexLinksInHTML        = 0;
            int tmpIndexLinksInJava        = 0;
            int tmpIndexSEBServicePolicy   = 0;
            int tmpIndexChooseFileToUpload = 0;

            int tmpIndexExitKey1 = 0;
            int tmpIndexExitKey2 = 0;
            int tmpIndexExitKey3 = 0;

            // Window width and height have 4 possible list entries
            for (index = 1; index <= 4; index++)
            {
                String width  = StringWindowWidth [index];
                String height = StringWindowHeight[index];

                if (tmpStringMainWindowWidth .Equals(width )) tmpIndexMainWindowWidth  = index;
                if (tmpStringMainWindowHeight.Equals(height)) tmpIndexMainWindowHeight = index;

                if (tmpStringNewWindowWidth  .Equals(width )) tmpIndexNewWindowWidth   = index;
                if (tmpStringNewWindowHeight .Equals(height)) tmpIndexNewWindowHeight  = index;
            }

            // Window position, policies etc. have 3 possible list entries
            for (index = 1; index <= 3; index++)
            {
                String position = StringWindowPosition   [index];
                String link     = StringPolicyLinkOpening[index];
                String service  = StringPolicySEBService [index];
                String upload   = StringFileToUpload     [index];

                if (tmpStringMainWindowPosition.Equals(position)) tmpIndexMainWindowPosition = index;
                if (tmpStringNewWindowPosition .Equals(position)) tmpIndexNewWindowPosition  = index;

                if (tmpStringLinksInHTML.Equals(link)) tmpIndexLinksInHTML = index;
                if (tmpStringLinksInJava.Equals(link)) tmpIndexLinksInJava = index;

                if (tmpStringSEBServicePolicy  .Equals(service)) tmpIndexSEBServicePolicy   = index;
                if (tmpStringChooseFileToUpload.Equals(upload )) tmpIndexChooseFileToUpload = index;
            }

            // Function keys have 12 possible list entries
            for (index = 1; index <= 12; index++)
            {
                String key = StringFunctionKey[index];

                if (tmpStringExitKey1.Equals(key)) tmpIndexExitKey1 = index;
                if (tmpStringExitKey2.Equals(key)) tmpIndexExitKey2 = index;
                if (tmpStringExitKey3.Equals(key)) tmpIndexExitKey3 = index;
            }

            // Store the determined integers
            settingInteger[StateTmp, GroupAppearance , ValueMainWindowWidth   ] = tmpIndexMainWindowWidth;
            settingInteger[StateTmp, GroupAppearance , ValueMainWindowHeight  ] = tmpIndexMainWindowHeight;
            settingInteger[StateTmp, GroupAppearance , ValueMainWindowPosition] = tmpIndexMainWindowPosition;

            settingInteger[StateTmp, GroupBrowser, ValueNewWindowWidth   ] = tmpIndexNewWindowWidth;
            settingInteger[StateTmp, GroupBrowser, ValueNewWindowHeight  ] = tmpIndexNewWindowHeight;
            settingInteger[StateTmp, GroupBrowser, ValueNewWindowPosition] = tmpIndexNewWindowPosition;

            settingInteger[StateTmp, GroupBrowser, ValueOpenLinksHTML] = tmpIndexLinksInHTML;
            settingInteger[StateTmp, GroupBrowser, ValueOpenLinksJava] = tmpIndexLinksInJava;

            settingInteger[StateTmp, GroupDownUploads, ValueChooseFileToUpload] = tmpIndexChooseFileToUpload;
            settingInteger[StateTmp, GroupSecurity   , ValueSEBServicePolicy  ] = tmpIndexSEBServicePolicy;

            settingInteger[StateTmp, GroupExitKeys, ValueExitKey1] = tmpIndexExitKey1;
            settingInteger[StateTmp, GroupExitKeys, ValueExitKey2] = tmpIndexExitKey2;
            settingInteger[StateTmp, GroupExitKeys, ValueExitKey3] = tmpIndexExitKey3;

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
            // Choose Identity
            // Main Window Width/Height/Position
            // New  Window Width/Height/Position
            // Link Opening Policy for Requesting/JavaScript
            // Choose File To Upload
            // SEB Service Policy
            // Exit Key Sequence (exit keys 1,2,3)

            int newIndexChooseIdentity     = settingInteger[StateNew, GroupConfigFile, ValueChooseIdentity];

            int newIndexMainWindowWidth    = settingInteger[StateNew, GroupAppearance, ValueMainWindowWidth];
            int newIndexMainWindowHeight   = settingInteger[StateNew, GroupAppearance, ValueMainWindowHeight];
            int newIndexMainWindowPosition = settingInteger[StateNew, GroupAppearance, ValueMainWindowPosition];

            int newIndexNewWindowWidth     = settingInteger[StateNew, GroupBrowser, ValueNewWindowWidth];
            int newIndexNewWindowHeight    = settingInteger[StateNew, GroupBrowser, ValueNewWindowHeight];
            int newIndexNewWindowPosition  = settingInteger[StateNew, GroupBrowser, ValueNewWindowPosition];

            int newIndexLinksRequesting    = settingInteger[StateNew, GroupBrowser, ValueOpenLinksHTML];
            int newIndexLinksJavaScript    = settingInteger[StateNew, GroupBrowser, ValueOpenLinksJava];

            int newIndexChooseFileToUpload = settingInteger[StateNew, GroupDownUploads, ValueChooseFileToUpload];
            int newIndexSEBServicePolicy   = settingInteger[StateNew, GroupSecurity   , ValueSEBServicePolicy];

            int newIndexExitKey1 = settingInteger[StateNew, GroupExitKeys, ValueExitKey1];
            int newIndexExitKey2 = settingInteger[StateNew, GroupExitKeys, ValueExitKey2];
            int newIndexExitKey3 = settingInteger[StateNew, GroupExitKeys, ValueExitKey3];

            // Store the determined strings
            settingString[StateNew, GroupConfigFile, ValueChooseIdentity] = chooseIdentityStringList[newIndexChooseIdentity];

            settingString[StateNew, GroupAppearance, ValueMainWindowWidth   ] = StringWindowWidth   [newIndexMainWindowWidth];
            settingString[StateNew, GroupAppearance, ValueMainWindowHeight  ] = StringWindowHeight  [newIndexMainWindowHeight];
            settingString[StateNew, GroupAppearance, ValueMainWindowPosition] = StringWindowPosition[newIndexMainWindowPosition];

            settingString[StateNew, GroupBrowser, ValueNewWindowWidth   ] = StringWindowWidth   [newIndexNewWindowWidth];
            settingString[StateNew, GroupBrowser, ValueNewWindowHeight  ] = StringWindowHeight  [newIndexNewWindowHeight];
            settingString[StateNew, GroupBrowser, ValueNewWindowPosition] = StringWindowPosition[newIndexNewWindowPosition];

            settingString[StateNew, GroupBrowser, ValueOpenLinksHTML] = StringPolicyLinkOpening[newIndexLinksRequesting];
            settingString[StateNew, GroupBrowser, ValueOpenLinksJava] = StringPolicyLinkOpening[newIndexLinksJavaScript];

            settingString[StateNew, GroupDownUploads, ValueChooseFileToUpload] = StringFileToUpload    [newIndexChooseFileToUpload];
            settingString[StateNew, GroupSecurity   , ValueSEBServicePolicy  ] = StringPolicySEBService[newIndexSEBServicePolicy];

            settingString[StateNew, GroupExitKeys, ValueExitKey1] = StringFunctionKey[newIndexExitKey1];
            settingString[StateNew, GroupExitKeys, ValueExitKey2] = StringFunctionKey[newIndexExitKey2];
            settingString[StateNew, GroupExitKeys, ValueExitKey3] = StringFunctionKey[newIndexExitKey3];

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
                // Open the .xml file for reading
                XmlSerializer deserializer = new XmlSerializer(typeof(SEBClientConfig));
                TextReader      textReader = new StreamReader (fileName);

                // Parse the XML structure into a C# object
                sebSettings = (SEBClientConfig)deserializer.Deserialize(textReader);

                // Close the .xml file
                textReader.Close();
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
                sebSettings = (SEBClientConfig)sebSerializer.Deserialize(memoryStream);

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
                serializer.Serialize(textWriter, sebSettings);

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

            settingString [StateTmp, GroupGeneral, ValueStartURL           ] = sebSettings.getUrlAddress    ("startURL"    ).Url;
          //settingString [StateTmp, GroupGeneral, ValueSEBServerURL       ] = sebSettings.getUrlAddress    ("sebServerURL").Url;
            settingString [StateTmp, GroupGeneral, ValueHashedAdminPassword] = sebSettings.getPassword      ("hashedAdminPassword").Value;
            settingString [StateTmp, GroupGeneral, ValueHashedQuitPassword ] = sebSettings.getPassword      ("hashedQuitPassword" ).Value;
            settingBoolean[StateTmp, GroupGeneral, ValueAllowUserToQuitSEB ] = sebSettings.getSecurityOption("allowQuit").getBool();

          //settingString [StateTmp, GroupConfigFile, ValueSebPurpose            ] = sebSettings.getPolicySetting ("sebPurpose"            ).Value;
          //settingBoolean[StateTmp, GroupConfigFile, ValueStartingAnExam        ] = sebSettings.getSecurityOption("startingAnExam"        ).getBool();
          //settingBoolean[StateTmp, GroupConfigFile, ValueConfiguringAClient    ] = sebSettings.getSecurityOption("configuringAClient"    ).getBool();
            settingBoolean[StateTmp, GroupConfigFile, ValueAllowOpenPrefWindow   ] = sebSettings.getSecurityOption("allowPreferencesWindow").getBool();
          //settingString [StateTmp, GroupConfigFile, ValueChooseIdentity        ] = sebSettings.getPassword      ("chooseIdentity"        ).Value;
          //settingString [StateTmp, GroupConfigFile, ValueHashedSettingsPassword] = sebSettings.getPassword      ("hashedSettingsPassword").Value;

          //settingString [StateTmp, GroupAppearance, ValueBrowserViewMode      ] = sebSettings.getPolicySetting ("browserViewMode").Value;
          //settingBoolean[StateTmp, GroupAppearance, ValueUseBrowserWindow     ] = sebSettings.getSecurityOption("useBrowserWindow" ).getBool();
          //settingBoolean[StateTmp, GroupAppearance, ValueUseFullScreenMode    ] = sebSettings.getSecurityOption("useFullScreenMode").getBool();
            settingString [StateTmp, GroupAppearance, ValueMainWindowWidth      ] = sebSettings.getPolicySetting ("mainBrowserWindowWidth"   ).Value;
            settingString [StateTmp, GroupAppearance, ValueMainWindowHeight     ] = sebSettings.getPolicySetting ("mainBrowserWindowHeight"  ).Value;
          //settingString [StateTmp, GroupAppearance, ValueMainWindowPosition   ] = sebSettings.getPolicySetting ("mainBrowserWindowPosition").Value;
            settingBoolean[StateTmp, GroupAppearance, ValueEnableWindowToolbar  ] = sebSettings.getSecurityOption("enableBrowserWindowToolbar").getBool();
            settingBoolean[StateTmp, GroupAppearance, ValueHideToolbarAsDefault ] = sebSettings.getSecurityOption(  "hideBrowserWindowToolbar").getBool();
            settingBoolean[StateTmp, GroupAppearance, ValueShowMenuBar          ] = sebSettings.getSecurityOption("showMenuBar").getBool();
            settingBoolean[StateTmp, GroupAppearance, ValueDisplaySEBDockTaskBar] = sebSettings.getSecurityOption("showTaskBar").getBool();

          //settingString [StateTmp, GroupBrowser, ValueNewWindowPolicyHTML ] = sebSettings.getPolicySetting ("newBrowserWindowByLinkPolicy").Value;
          //settingString [StateTmp, GroupBrowser, ValueNewWindowPolicyJava ] = sebSettings.getPolicySetting ("newBrowserWindowByLinkPolicy").Value;
            settingBoolean[StateTmp, GroupBrowser, ValueBlockLinksHTML      ] = sebSettings.getSecurityOption("newBrowserWindowByLinkBlockForeign"  ).getBool();
            settingBoolean[StateTmp, GroupBrowser, ValueBlockLinksJava      ] = sebSettings.getSecurityOption("newBrowserWindowByScriptBlockForeign").getBool();
            settingString [StateTmp, GroupBrowser, ValueNewWindowWidth      ] = sebSettings.getPolicySetting ("newBrowserWindowByLinkWidth"   ).Value;
            settingString [StateTmp, GroupBrowser, ValueNewWindowHeight     ] = sebSettings.getPolicySetting ("newBrowserWindowByLinkHeight"  ).Value;
          //settingString [StateTmp, GroupBrowser, ValueNewWindowPosition   ] = sebSettings.getPolicySetting ("newBrowserWindowByLinkPosition").Value;
            settingBoolean[StateTmp, GroupBrowser, ValueEnablePlugins       ] = sebSettings.getSecurityOption("enablePlugins"   ).getBool();
            settingBoolean[StateTmp, GroupBrowser, ValueEnableJava          ] = sebSettings.getSecurityOption("enableJava"      ).getBool();
            settingBoolean[StateTmp, GroupBrowser, ValueEnableJavaScript    ] = sebSettings.getSecurityOption("enableJavaScript").getBool();
            settingBoolean[StateTmp, GroupBrowser, ValueBlockPopupWindows   ] = sebSettings.getSecurityOption("blockPopUpWindows").getBool();
            settingBoolean[StateTmp, GroupBrowser, ValueAllowBrowsingBackForward] = sebSettings.getSecurityOption("enableBrowsingBackForward").getBool();
            settingBoolean[StateTmp, GroupBrowser, ValueUseSEBWithoutBrowser] = sebSettings.getSecurityOption("enableSebBrowser").getBool();

            settingBoolean[StateTmp, GroupDownUploads, ValueAllowDownUploadingFiles  ] = sebSettings.getSecurityOption   ("allowDownUploads").getBool();
            settingBoolean[StateTmp, GroupDownUploads, ValueOpenFilesAfterDownloading] = sebSettings.getSecurityOption   ("openDownloads"   ).getBool();
            settingBoolean[StateTmp, GroupDownUploads, ValueDownloadAndOpenPDFFiles  ] = sebSettings.getSecurityOption   ("downloadPDFFiles").getBool();
            settingString [StateTmp, GroupDownUploads, ValueSaveDownloadedFilesTo    ] = sebSettings.getDownloadDirectory("downloadDirectoryWin"    ).Path;
            settingString [StateTmp, GroupDownUploads, ValueChooseFileToUpload       ] = sebSettings.getPolicySetting    ("chooseFileToUploadPolicy").Value;

          //settingString [StateTmp, GroupExam, ValueBrowserExamKey    ] = sebSettings.getUrlAddress("browserExamKey" ).Url;
            settingBoolean[StateTmp, GroupExam, ValueCopyBrowserExamKey] = sebSettings.getSecurityOption("copyExamKeyToClipboardWhenQuitting").getBool();
            settingBoolean[StateTmp, GroupExam, ValueSendBrowserExamKey] = sebSettings.getSecurityOption("sendBrowserExamKey").getBool();
            settingString [StateTmp, GroupExam, ValueQuitURL           ] = sebSettings.getUrlAddress("quitURL").Url;

            settingBoolean[StateTmp, GroupApplications, ValueMonitorProcesses         ] = sebSettings.getSecurityOption("monitorProcesses         ").getBool();
            settingBoolean[StateTmp, GroupApplications, ValueAllowSwitchToApplications] = sebSettings.getSecurityOption("allowSwitchToApplications").getBool();
            settingBoolean[StateTmp, GroupApplications, ValueAllowFlashFullscreenMode ] = sebSettings.getSecurityOption("allowFlashFullscreen     ").getBool();

            settingString [StateTmp, GroupSecurity, ValueSEBServicePolicy   ] = sebSettings.getPolicySetting ("sebServicePolicy"   ).Value;
            settingBoolean[StateTmp, GroupSecurity, ValueAllowVirtualMachine] = sebSettings.getSecurityOption("allowVirtualMachine").getBool();
            settingBoolean[StateTmp, GroupSecurity, ValueEnableLogging      ] = sebSettings.getSecurityOption("enableLog"          ).getBool();

            settingBoolean[StateTmp, GroupInsideSeb, ValueEnableSwitchUser       ] = sebSettings.getRegistryValue("insideSebEnableSwitchUser"       ).getBool();
            settingBoolean[StateTmp, GroupInsideSeb, ValueEnableLockThisComputer ] = sebSettings.getRegistryValue("insideSebEnableLockThisComputer" ).getBool();
            settingBoolean[StateTmp, GroupInsideSeb, ValueEnableChangeAPassword  ] = sebSettings.getRegistryValue("insideSebEnableChangePassword"   ).getBool();
            settingBoolean[StateTmp, GroupInsideSeb, ValueEnableStartTaskManager ] = sebSettings.getRegistryValue("insideSebEnableStartTaskManager" ).getBool();
            settingBoolean[StateTmp, GroupInsideSeb, ValueEnableLogOff           ] = sebSettings.getRegistryValue("insideSebEnableLogOff"           ).getBool();
            settingBoolean[StateTmp, GroupInsideSeb, ValueEnableShutDown         ] = sebSettings.getRegistryValue("insideSebEnableShutDown"         ).getBool();
            settingBoolean[StateTmp, GroupInsideSeb, ValueEnableEaseOfAccess     ] = sebSettings.getRegistryValue("insideSebEnableEaseOfAccess"     ).getBool();
            settingBoolean[StateTmp, GroupInsideSeb, ValueEnableVmWareClientShade] = sebSettings.getRegistryValue("insideSebEnableVmWareClientShade").getBool();

            settingBoolean[StateTmp, GroupOutsideSeb, ValueEnableSwitchUser       ] = sebSettings.getRegistryValue("outsideSebEnableSwitchUser"       ).getBool();
            settingBoolean[StateTmp, GroupOutsideSeb, ValueEnableLockThisComputer ] = sebSettings.getRegistryValue("outsideSebEnableLockThisComputer" ).getBool();
            settingBoolean[StateTmp, GroupOutsideSeb, ValueEnableChangeAPassword  ] = sebSettings.getRegistryValue("outsideSebEnableChangePassword"   ).getBool();
            settingBoolean[StateTmp, GroupOutsideSeb, ValueEnableStartTaskManager ] = sebSettings.getRegistryValue("outsideSebEnableStartTaskManager" ).getBool();
            settingBoolean[StateTmp, GroupOutsideSeb, ValueEnableLogOff           ] = sebSettings.getRegistryValue("outsideSebEnableLogOff"           ).getBool();
            settingBoolean[StateTmp, GroupOutsideSeb, ValueEnableShutDown         ] = sebSettings.getRegistryValue("outsideSebEnableShutDown"         ).getBool();
            settingBoolean[StateTmp, GroupOutsideSeb, ValueEnableEaseOfAccess     ] = sebSettings.getRegistryValue("outsideSebEnableEaseOfAccess"     ).getBool();
            settingBoolean[StateTmp, GroupOutsideSeb, ValueEnableVmWareClientShade] = sebSettings.getRegistryValue("outsideSebEnableVmWareClientShade").getBool();

            settingBoolean[StateTmp, GroupHookedKeys, ValueHookMessages] = sebSettings.getSecurityOption("hookMessages").getBool();

            settingBoolean[StateTmp, GroupSpecialKeys, ValueEnableEsc       ] = sebSettings.getHookedMessageKey("enableEsc"       ).getBool();
            settingBoolean[StateTmp, GroupSpecialKeys, ValueEnableCtrlEsc   ] = sebSettings.getHookedMessageKey("enableCtrlEsc"   ).getBool();
            settingBoolean[StateTmp, GroupSpecialKeys, ValueEnableAltEsc    ] = sebSettings.getHookedMessageKey("enableAltEsc"    ).getBool();
            settingBoolean[StateTmp, GroupSpecialKeys, ValueEnableAltTab    ] = sebSettings.getHookedMessageKey("enableAltTab"    ).getBool();
            settingBoolean[StateTmp, GroupSpecialKeys, ValueEnableAltF4     ] = sebSettings.getHookedMessageKey("enableAltF4"     ).getBool();
          //settingBoolean[StateTmp, GroupSpecialKeys, ValueEnableStartMenu ] = sebSettings.getHookedMessageKey("enableStartMenu" ).getBool();
            settingBoolean[StateTmp, GroupSpecialKeys, ValueEnableRightMouse] = sebSettings.getHookedMessageKey("enableRightMouse").getBool();

            settingBoolean[StateTmp, GroupFunctionKeys, ValueEnableF1 ] = sebSettings.getHookedMessageKey("enableF1" ).getBool();
            settingBoolean[StateTmp, GroupFunctionKeys, ValueEnableF2 ] = sebSettings.getHookedMessageKey("enableF2" ).getBool();
            settingBoolean[StateTmp, GroupFunctionKeys, ValueEnableF3 ] = sebSettings.getHookedMessageKey("enableF3" ).getBool();
            settingBoolean[StateTmp, GroupFunctionKeys, ValueEnableF4 ] = sebSettings.getHookedMessageKey("enableF4" ).getBool();
            settingBoolean[StateTmp, GroupFunctionKeys, ValueEnableF5 ] = sebSettings.getHookedMessageKey("enableF5" ).getBool();
            settingBoolean[StateTmp, GroupFunctionKeys, ValueEnableF6 ] = sebSettings.getHookedMessageKey("enableF6" ).getBool();
            settingBoolean[StateTmp, GroupFunctionKeys, ValueEnableF7 ] = sebSettings.getHookedMessageKey("enableF7" ).getBool();
            settingBoolean[StateTmp, GroupFunctionKeys, ValueEnableF8 ] = sebSettings.getHookedMessageKey("enableF8" ).getBool();
            settingBoolean[StateTmp, GroupFunctionKeys, ValueEnableF9 ] = sebSettings.getHookedMessageKey("enableF9" ).getBool();
            settingBoolean[StateTmp, GroupFunctionKeys, ValueEnableF10] = sebSettings.getHookedMessageKey("enableF10").getBool();
            settingBoolean[StateTmp, GroupFunctionKeys, ValueEnableF11] = sebSettings.getHookedMessageKey("enableF11").getBool();
            settingBoolean[StateTmp, GroupFunctionKeys, ValueEnableF12] = sebSettings.getHookedMessageKey("enableF12").getBool();

            return true;
        }



        // ****************************************************************
        // Convert arrays to C# object (to be written to .xml or .seb file)
        // ****************************************************************
        private Boolean ConvertArraysToCSharpObject()
        {
            // Copy the arrays "settingString"/"settingBoolean" to the C# object "sebSettings"

            sebSettings.getUrlAddress("startURL")         .Url   = settingString [StateNew, GroupGeneral, ValueStartURL];
          //sebSettings.getUrlAddress("sebServerURL")     .Url   = settingString [StateNew, GroupGeneral, ValueSEBServerURL];
            sebSettings.getPassword("hashedAdminPassword").Value = settingString [StateNew, GroupGeneral, ValueHashedAdminPassword];
            sebSettings.getPassword("hashedQuitPassword") .Value = settingString [StateNew, GroupGeneral, ValueHashedQuitPassword];
            sebSettings.getSecurityOption("allowQuit")    .setBool(settingBoolean[StateNew, GroupGeneral, ValueAllowUserToQuitSEB]);

          //sebSettings.getPolicySetting ("sebPurpose"            ).Value = settingString [StateNew, GroupConfigFile, ValueSebPurpose];
          //sebSettings.getSecurityOption("startingAnExam"        ).setBool(settingBoolean[StateNew, GroupConfigFile, ValueStartingAnExam]);
          //sebSettings.getSecurityOption("configuringAClient"    ).setBool(settingBoolean[StateNew, GroupConfigFile, ValueConfiguringAClient]);
            sebSettings.getSecurityOption("allowPreferencesWindow").setBool(settingBoolean[StateNew, GroupConfigFile, ValueAllowOpenPrefWindow]);
          //sebSettings.getPassword("chooseIdentity"              ).Value = settingString [StateNew, GroupConfigFile, ValueChooseIdentity];
          //sebSettings.getPassword("hashedSettingsPassword"      ).Value = settingString [StateNew, GroupConfigFile, ValueHashedSettingsPassword];

          //sebSettings.getPolicySetting ("browserViewMode"           ).Value = settingString [StateNew, GroupAppearance, ValueBrowserViewMode];
          //sebSettings.getSecurityOption("useBrowserWindow"          ).setBool(settingBoolean[StateNew, GroupAppearance, ValueUseBrowserWindow]);
          //sebSettings.getSecurityOption("useFullScreenMode"         ).setBool(settingBoolean[StateNew, GroupAppearance, ValueUseFullScreenMode]);
            sebSettings.getPolicySetting ("mainBrowserWindowWidth"    ).Value = settingString [StateNew, GroupAppearance, ValueMainWindowWidth];
            sebSettings.getPolicySetting ("mainBrowserWindowHeight"   ).Value = settingString [StateNew, GroupAppearance, ValueMainWindowHeight];
          //sebSettings.getPolicySetting ("mainBrowserWindowPosition" ).Value = settingString [StateNew, GroupAppearance, ValueMainWindowPosition];
            sebSettings.getSecurityOption("enableBrowserWindowToolbar").setBool(settingBoolean[StateNew, GroupAppearance, ValueEnableWindowToolbar]);
            sebSettings.getSecurityOption(  "hideBrowserWindowToolbar").setBool(settingBoolean[StateNew, GroupAppearance, ValueHideToolbarAsDefault]);
            sebSettings.getSecurityOption("showMenuBar"               ).setBool(settingBoolean[StateNew, GroupAppearance, ValueShowMenuBar]);
            sebSettings.getSecurityOption("showTaskBar"               ).setBool(settingBoolean[StateNew, GroupAppearance, ValueDisplaySEBDockTaskBar]);

          //sebSettings.getPolicySetting ("newBrowserWindowByLinkPolicy"  ).Value = settingString[StateNew, GroupBrowser, ValueNewWindowPolicyHTML];
          //sebSettings.getPolicySetting ("newBrowserWindowByLinkPolicy"  ).Value = settingString[StateNew, GroupBrowser, ValueNewWindowPolicyJava];
            sebSettings.getSecurityOption("newBrowserWindowByLinkBlockForeign  ").setBool(settingBoolean[StateNew, GroupBrowser, ValueBlockLinksHTML]);
            sebSettings.getSecurityOption("newBrowserWindowByScriptBlockForeign").setBool(settingBoolean[StateNew, GroupBrowser, ValueBlockLinksJava]);
            sebSettings.getPolicySetting ("newBrowserWindowByLinkWidth"   ).Value = settingString[StateNew, GroupBrowser, ValueNewWindowWidth];
            sebSettings.getPolicySetting ("newBrowserWindowByLinkHeight"  ).Value = settingString[StateNew, GroupBrowser, ValueNewWindowHeight];
          //sebSettings.getPolicySetting ("newBrowserWindowByLinkPosition").Value = settingString[StateNew, GroupBrowser, ValueNewWindowPosition];
            sebSettings.getSecurityOption("enablePlugins"    ).setBool(settingBoolean[StateNew, GroupBrowser, ValueEnablePlugins]);
            sebSettings.getSecurityOption("enableJava"       ).setBool(settingBoolean[StateNew, GroupBrowser, ValueEnableJava]);
            sebSettings.getSecurityOption("enableJavaScript" ).setBool(settingBoolean[StateNew, GroupBrowser, ValueEnableJavaScript]);
            sebSettings.getSecurityOption("blockPopUpWindows").setBool(settingBoolean[StateNew, GroupBrowser, ValueBlockPopupWindows]);
            sebSettings.getSecurityOption("enableBrowsingBackForward").setBool(settingBoolean[StateNew, GroupBrowser, ValueAllowBrowsingBackForward]);
            sebSettings.getSecurityOption("enableSebBrowser"         ).setBool(settingBoolean[StateNew, GroupBrowser, ValueUseSEBWithoutBrowser]);

            sebSettings.getSecurityOption   ("allowDownUploads").setBool(settingBoolean[StateNew, GroupDownUploads, ValueAllowDownUploadingFiles]);
            sebSettings.getSecurityOption   ("openDownloads"   ).setBool(settingBoolean[StateNew, GroupDownUploads, ValueOpenFilesAfterDownloading]);
            sebSettings.getSecurityOption   ("downloadPDFFiles").setBool(settingBoolean[StateNew, GroupDownUploads, ValueDownloadAndOpenPDFFiles]);
            sebSettings.getDownloadDirectory("downloadDirectoryWin"    ).Path  = settingString[StateNew, GroupDownUploads, ValueSaveDownloadedFilesTo];
            sebSettings.getPolicySetting    ("chooseFileToUploadPolicy").Value = settingString[StateNew, GroupDownUploads, ValueChooseFileToUpload];

          //sebSettings.getUrlAddress("browserExamKey").Url = settingString[StateNew, GroupExam, ValueBrowserExamKey];
            sebSettings.getSecurityOption("copyExamKeyToClipboardWhenQuitting").setBool(settingBoolean[StateNew, GroupExam, ValueCopyBrowserExamKey]);
            sebSettings.getSecurityOption("sendBrowserExamKey"                ).setBool(settingBoolean[StateNew, GroupExam, ValueSendBrowserExamKey]);
            sebSettings.getUrlAddress("quitURL").Url = settingString[StateNew, GroupExam, ValueQuitURL];

            sebSettings.getSecurityOption("monitorProcesses         ").setBool(settingBoolean[StateNew, GroupApplications, ValueMonitorProcesses]);
            sebSettings.getSecurityOption("allowSwitchToApplications").setBool(settingBoolean[StateNew, GroupApplications, ValueAllowSwitchToApplications]);
            sebSettings.getSecurityOption("allowFlashFullscreen     ").setBool(settingBoolean[StateNew, GroupApplications, ValueAllowFlashFullscreenMode]);

            sebSettings.getPolicySetting ("sebServicePolicy"   ).Value = settingString [StateNew, GroupSecurity, ValueSEBServicePolicy];
            sebSettings.getSecurityOption("allowVirtualMachine").setBool(settingBoolean[StateNew, GroupSecurity, ValueAllowVirtualMachine]);
            sebSettings.getSecurityOption("enableLog"          ).setBool(settingBoolean[StateNew, GroupSecurity, ValueEnableLogging]);

            sebSettings.getRegistryValue("insideSebEnableSwitchUser"       ).setBool(settingBoolean[StateNew, GroupInsideSeb, ValueEnableSwitchUser]);
            sebSettings.getRegistryValue("insideSebEnableLockThisComputer" ).setBool(settingBoolean[StateNew, GroupInsideSeb, ValueEnableLockThisComputer]);
            sebSettings.getRegistryValue("insideSebEnableChangePassword"   ).setBool(settingBoolean[StateNew, GroupInsideSeb, ValueEnableChangeAPassword]);
            sebSettings.getRegistryValue("insideSebEnableStartTaskManager" ).setBool(settingBoolean[StateNew, GroupInsideSeb, ValueEnableStartTaskManager]);
            sebSettings.getRegistryValue("insideSebEnableLogOff"           ).setBool(settingBoolean[StateNew, GroupInsideSeb, ValueEnableLogOff]);
            sebSettings.getRegistryValue("insideSebEnableShutDown"         ).setBool(settingBoolean[StateNew, GroupInsideSeb, ValueEnableShutDown]);
            sebSettings.getRegistryValue("insideSebEnableEaseOfAccess"     ).setBool(settingBoolean[StateNew, GroupInsideSeb, ValueEnableEaseOfAccess]);
            sebSettings.getRegistryValue("insideSebEnableVmWareClientShade").setBool(settingBoolean[StateNew, GroupInsideSeb, ValueEnableVmWareClientShade]);

            sebSettings.getRegistryValue("outsideSebEnableSwitchUser"       ).setBool(settingBoolean[StateNew, GroupOutsideSeb, ValueEnableSwitchUser]);
            sebSettings.getRegistryValue("outsideSebEnableLockThisComputer" ).setBool(settingBoolean[StateNew, GroupOutsideSeb, ValueEnableLockThisComputer]);
            sebSettings.getRegistryValue("outsideSebEnableChangePassword"   ).setBool(settingBoolean[StateNew, GroupOutsideSeb, ValueEnableChangeAPassword]);
            sebSettings.getRegistryValue("outsideSebEnableStartTaskManager" ).setBool(settingBoolean[StateNew, GroupOutsideSeb, ValueEnableStartTaskManager]);
            sebSettings.getRegistryValue("outsideSebEnableLogOff"           ).setBool(settingBoolean[StateNew, GroupOutsideSeb, ValueEnableLogOff]);
            sebSettings.getRegistryValue("outsideSebEnableShutDown"         ).setBool(settingBoolean[StateNew, GroupOutsideSeb, ValueEnableShutDown]);
            sebSettings.getRegistryValue("outsideSebEnableEaseOfAccess"     ).setBool(settingBoolean[StateNew, GroupOutsideSeb, ValueEnableEaseOfAccess]);
            sebSettings.getRegistryValue("outsideSebEnableVmWareClientShade").setBool(settingBoolean[StateNew, GroupOutsideSeb, ValueEnableVmWareClientShade]);

            sebSettings.getSecurityOption("hookMessages").setBool(settingBoolean[StateNew, GroupHookedKeys, ValueHookMessages]);

            sebSettings.getHookedMessageKey("enableEsc"       ).setBool(settingBoolean[StateNew, GroupSpecialKeys, ValueEnableEsc]);
            sebSettings.getHookedMessageKey("enableCtrlEsc"   ).setBool(settingBoolean[StateNew, GroupSpecialKeys, ValueEnableCtrlEsc]);
            sebSettings.getHookedMessageKey("enableAltEsc"    ).setBool(settingBoolean[StateNew, GroupSpecialKeys, ValueEnableAltEsc]);
            sebSettings.getHookedMessageKey("enableAltTab"    ).setBool(settingBoolean[StateNew, GroupSpecialKeys, ValueEnableAltTab]);
            sebSettings.getHookedMessageKey("enableAltF4"     ).setBool(settingBoolean[StateNew, GroupSpecialKeys, ValueEnableAltF4]);
          //sebSettings.getHookedMessageKey("enableStartMenu" ).setBool(settingBoolean[StateNew, GroupSpecialKeys, ValueEnableStartMenu]);
            sebSettings.getHookedMessageKey("enableRightMouse").setBool(settingBoolean[StateNew, GroupSpecialKeys, ValueEnableRightMouse]);

            sebSettings.getHookedMessageKey("enableF1" ).setBool(settingBoolean[StateNew, GroupFunctionKeys, ValueEnableF1 ]);
            sebSettings.getHookedMessageKey("enableF2" ).setBool(settingBoolean[StateNew, GroupFunctionKeys, ValueEnableF2 ]);
            sebSettings.getHookedMessageKey("enableF3" ).setBool(settingBoolean[StateNew, GroupFunctionKeys, ValueEnableF3 ]);
            sebSettings.getHookedMessageKey("enableF4" ).setBool(settingBoolean[StateNew, GroupFunctionKeys, ValueEnableF4 ]);
            sebSettings.getHookedMessageKey("enableF5" ).setBool(settingBoolean[StateNew, GroupFunctionKeys, ValueEnableF5 ]);
            sebSettings.getHookedMessageKey("enableF6" ).setBool(settingBoolean[StateNew, GroupFunctionKeys, ValueEnableF6 ]);
            sebSettings.getHookedMessageKey("enableF7" ).setBool(settingBoolean[StateNew, GroupFunctionKeys, ValueEnableF7 ]);
            sebSettings.getHookedMessageKey("enableF8" ).setBool(settingBoolean[StateNew, GroupFunctionKeys, ValueEnableF8 ]);
            sebSettings.getHookedMessageKey("enableF9" ).setBool(settingBoolean[StateNew, GroupFunctionKeys, ValueEnableF9 ]);
            sebSettings.getHookedMessageKey("enableF10").setBool(settingBoolean[StateNew, GroupFunctionKeys, ValueEnableF10]);
            sebSettings.getHookedMessageKey("enableF11").setBool(settingBoolean[StateNew, GroupFunctionKeys, ValueEnableF11]);
            sebSettings.getHookedMessageKey("enableF12").setBool(settingBoolean[StateNew, GroupFunctionKeys, ValueEnableF12]);

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

        private void textBoxSEBServerURL_TextChanged(object sender, EventArgs e)
        {
            settingString[StateNew, GroupGeneral, ValueSEBServerURL] = textBoxSEBServerURL.Text;
        }

        private void textBoxAdminPassword_TextChanged(object sender, EventArgs e)
        {
            settingString[StateNew, GroupGeneral, ValueAdminPassword] = textBoxAdminPassword.Text;
        }

        private void textBoxConfirmAdminPassword_TextChanged(object sender, EventArgs e)
        {
            settingString[StateNew, GroupGeneral, ValueConfirmAdminPassword] = textBoxConfirmAdminPassword.Text;
        }

        private void checkBoxAllowUserToQuitSEB_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupGeneral, ValueAllowUserToQuitSEB] = checkBoxAllowUserToQuitSEB.Checked;
        }

        private void textBoxQuitPassword_TextChanged_1(object sender, EventArgs e)
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
            settingBoolean[StateNew, GroupConfigFile, ValueStartingAnExam] = (radioButtonStartingAnExam.Checked == true);
        }

        private void radioButtonConfiguringAClient_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupConfigFile, ValueConfiguringAClient] = (radioButtonConfiguringAClient.Checked == true);
        }

        private void checkBoxAllowOpenPrefWindowOnClient_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupConfigFile, ValueAllowOpenPrefWindow] = checkBoxAllowOpenPrefWindow.Checked;
        }

        private void comboBoxChooseIdentity_SelectedIndexChanged(object sender, EventArgs e)
        {
            settingInteger[StateNew, GroupConfigFile, ValueChooseIdentity] = comboBoxChooseIdentity.SelectedIndex;
            settingString [StateNew, GroupConfigFile, ValueChooseIdentity] = comboBoxChooseIdentity.Text;
        }

        private void comboBoxChooseIdentity_TextUpdate(object sender, EventArgs e)
        {
            settingInteger[StateNew, GroupConfigFile, ValueChooseIdentity] = comboBoxChooseIdentity.SelectedIndex;
            settingString [StateNew, GroupConfigFile, ValueChooseIdentity] = comboBoxChooseIdentity.Text;
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
            settingBoolean[StateNew, GroupAppearance, ValueUseBrowserWindow] = radioButtonUseBrowserWindow.Checked;
        }

        private void radioButtonUseFullScreenMode_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupAppearance, ValueUseFullScreenMode] = radioButtonUseFullScreenMode.Checked;
        }

        private void comboBoxMainWindowWidth_SelectedIndexChanged(object sender, EventArgs e)
        {
            settingInteger[StateNew, GroupAppearance, ValueMainWindowWidth] = comboBoxMainWindowWidth.SelectedIndex + 1;
            settingString [StateNew, GroupAppearance, ValueMainWindowWidth] = comboBoxMainWindowWidth.Text;
        }

        private void comboBoxMainWindowWidth_TextUpdate(object sender, EventArgs e)
        {
            settingInteger[StateNew, GroupAppearance, ValueMainWindowWidth] = comboBoxMainWindowWidth.SelectedIndex + 1;
            settingString [StateNew, GroupAppearance, ValueMainWindowWidth] = comboBoxMainWindowWidth.Text;
        }

        private void comboBoxMainWindowHeight_SelectedIndexChanged(object sender, EventArgs e)
        {
            settingInteger[StateNew, GroupAppearance, ValueMainWindowHeight] = comboBoxMainWindowHeight.SelectedIndex + 1;
            settingString [StateNew, GroupAppearance, ValueMainWindowHeight] = comboBoxMainWindowHeight.Text;
        }

        private void comboBoxMainWindowHeight_TextUpdate(object sender, EventArgs e)
        {
            settingInteger[StateNew, GroupAppearance, ValueMainWindowHeight] = comboBoxMainWindowHeight.SelectedIndex + 1;
            settingString [StateNew, GroupAppearance, ValueMainWindowHeight] = comboBoxMainWindowHeight.Text;
        }

        private void listBoxMainWindowPosition_SelectedIndexChanged(object sender, EventArgs e)
        {
            settingInteger[StateNew, GroupAppearance, ValueMainWindowPosition] = listBoxMainWindowPosition.SelectedIndex + 1;
        }

        private void checkBoxEnableWindowToolbar_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupAppearance, ValueEnableWindowToolbar] = checkBoxEnableWindowToolbar.Checked;
            checkBoxHideToolbarAsDefault.Enabled = checkBoxEnableWindowToolbar.Checked;
        }

        private void checkBoxHideToolbarAsDefault_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupAppearance, ValueHideToolbarAsDefault] = checkBoxHideToolbarAsDefault.Checked;
        }

        private void checkBoxShowMenuBar_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupAppearance, ValueShowMenuBar] = checkBoxShowMenuBar.Checked;
        }

        private void checkBoxDisplaySEBDockTaskBar_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupAppearance, ValueDisplaySEBDockTaskBar] = checkBoxDisplaySEBDockTaskBar.Checked;
        }



        // ***************
        // Group "Browser"
        // ***************
        private void listBoxOpenLinksHTML_SelectedIndexChanged(object sender, EventArgs e)
        {
            settingInteger[StateNew, GroupBrowser, ValueOpenLinksHTML] = listBoxOpenLinksHTML.SelectedIndex + 1;
        }

        private void listBoxOpenLinksJava_SelectedIndexChanged(object sender, EventArgs e)
        {
            settingInteger[StateNew, GroupBrowser, ValueOpenLinksJava] = listBoxOpenLinksJava.SelectedIndex + 1;
        }

        private void checkBoxBlockLinksHTML_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupBrowser, ValueBlockLinksHTML] = checkBoxBlockLinksHTML.Checked;
        }

        private void checkBoxBlockLinksJava_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupBrowser, ValueBlockLinksJava] = checkBoxBlockLinksJava.Checked;
        }

        private void comboBoxNewWindowWidth_SelectedIndexChanged(object sender, EventArgs e)
        {
            settingInteger[StateNew, GroupBrowser, ValueNewWindowWidth] = comboBoxNewWindowWidth.SelectedIndex + 1;
            settingString [StateNew, GroupBrowser, ValueNewWindowWidth] = comboBoxNewWindowWidth.Text;
        }

        private void comboBoxNewWindowWidth_TextUpdate(object sender, EventArgs e)
        {
            settingInteger[StateNew, GroupBrowser, ValueNewWindowWidth] = comboBoxNewWindowWidth.SelectedIndex + 1;
            settingString [StateNew, GroupBrowser, ValueNewWindowWidth] = comboBoxNewWindowWidth.Text;
        }

        private void comboBoxNewWindowHeight_SelectedIndexChanged(object sender, EventArgs e)
        {
            settingInteger[StateNew, GroupBrowser, ValueNewWindowHeight] = comboBoxNewWindowHeight.SelectedIndex + 1;
            settingString [StateNew, GroupBrowser, ValueNewWindowHeight] = comboBoxNewWindowHeight.Text;
        }

        private void comboBoxNewWindowHeight_TextUpdate(object sender, EventArgs e)
        {
            settingInteger[StateNew, GroupBrowser, ValueNewWindowHeight] = comboBoxNewWindowHeight.SelectedIndex + 1;
            settingString [StateNew, GroupBrowser, ValueNewWindowHeight] = comboBoxNewWindowHeight.Text;
        }

        private void listBoxNewWindowPosition_SelectedIndexChanged(object sender, EventArgs e)
        {
            settingInteger[StateNew, GroupBrowser, ValueNewWindowPosition] = listBoxNewWindowPosition.SelectedIndex + 1;
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

        private void checkBoxUseSEBWithoutBrowser_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupBrowser, ValueUseSEBWithoutBrowser] = checkBoxUseSEBWithoutBrowser.Checked;
        }



        // ********************
        // Group "Down/Uploads"
        // ********************
        private void checkBoxAllowDownUploadingFiles_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupDownUploads, ValueAllowDownUploadingFiles] = checkBoxAllowDownUploadingFiles.Checked;
        }

        private void folderBrowserDialogDownloadFolder_HelpRequest(object sender, EventArgs e)
        {

        }

        private void buttonSaveDownloadedFilesTo_Click(object sender, EventArgs e)
        {
            // Set the default directory in the Folder Browser Dialog
            folderBrowserDialogDownloadFolder.RootFolder = Environment.SpecialFolder.DesktopDirectory;
//          folderBrowserDialogDownloadFolder.RootFolder = Environment.CurrentDirectory;

            // Get the user inputs in the File Dialog
            DialogResult dialogResult = folderBrowserDialogDownloadFolder.ShowDialog();
            String       downloadPath = folderBrowserDialogDownloadFolder.SelectedPath;

            // If the user clicked "Cancel", do nothing
            if (dialogResult.Equals(DialogResult.Cancel)) return;

            // If the user clicked "OK", ...
            settingString[StateNew, GroupDownUploads, ValueSaveDownloadedFilesTo] = downloadPath;
                                                  labelSaveDownloadedFilesTo.Text = downloadPath;
        }

        private void checkBoxOpenFilesAfterDownloading_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupDownUploads, ValueOpenFilesAfterDownloading] = checkBoxOpenFilesAfterDownloading.Checked;
        }

        private void listBoxChooseFileToUpload_SelectedIndexChanged(object sender, EventArgs e)
        {
            settingInteger[StateNew, GroupDownUploads, ValueChooseFileToUpload] = listBoxChooseFileToUpload.SelectedIndex + 1;
        }

        private void checkBoxDownloadAndOpenPDFFiles_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupDownUploads, ValueDownloadAndOpenPDFFiles] = checkBoxDownloadAndOpenPDFFiles.Checked;
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
            settingBoolean[StateNew, GroupApplications, ValueAllowFlashFullscreenMode] = checkBoxAllowFlashFullscreenMode.Checked;
        }



        // ***************
        // Group "Network"
        // ***************

        // ****************
        // Group "Security"
        // ****************
        private void listBoxSEBServicePolicy_SelectedIndexChanged(object sender, EventArgs e)
        {
            settingInteger[StateNew, GroupSecurity, ValueSEBServicePolicy] = listBoxSEBServicePolicy.SelectedIndex + 1;
        }

        private void checkBoxAllowVirtualMachine_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupSecurity, ValueAllowVirtualMachine] = checkBoxAllowVirtualMachine.Checked;
        }

        private void checkBoxEnableLogging_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupSecurity, ValueEnableLogging] = checkBoxEnableLogging.Checked;
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



        // *****************
        // Group "Exit Keys"
        // *****************
        private void listBoxExitKey1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Make sure that all three exit keys are different.
            // If selected key is already occupied, revert to previously selected key.
            if ((listBoxExitKey1.SelectedIndex == listBoxExitKey2.SelectedIndex) ||
                (listBoxExitKey1.SelectedIndex == listBoxExitKey3.SelectedIndex))
                 listBoxExitKey1.SelectedIndex =  settingInteger[StateNew, GroupExitKeys, ValueExitKey1] - 1;
            settingInteger[StateNew, GroupExitKeys, ValueExitKey1] = listBoxExitKey1.SelectedIndex + 1;
        }

        private void listBoxExitKey2_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Make sure that all three exit keys are different.
            // If selected key is already occupied, revert to previously selected key.
            if ((listBoxExitKey2.SelectedIndex == listBoxExitKey1.SelectedIndex) ||
                (listBoxExitKey2.SelectedIndex == listBoxExitKey3.SelectedIndex))
                 listBoxExitKey2.SelectedIndex =  settingInteger[StateNew, GroupExitKeys, ValueExitKey2] - 1;
            settingInteger[StateNew, GroupExitKeys, ValueExitKey2] = listBoxExitKey2.SelectedIndex + 1;
        }

        private void listBoxExitKey3_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Make sure that all three exit keys are different.
            // If selected key is already occupied, revert to previously selected key.
            if ((listBoxExitKey3.SelectedIndex == listBoxExitKey1.SelectedIndex) ||
                (listBoxExitKey3.SelectedIndex == listBoxExitKey2.SelectedIndex))
                 listBoxExitKey3.SelectedIndex =  settingInteger[StateNew, GroupExitKeys, ValueExitKey3] - 1;
            settingInteger[StateNew, GroupExitKeys, ValueExitKey3] = listBoxExitKey3.SelectedIndex + 1;
        }



        // ************************
        // Group "Security Options"
        // ************************
        private void checkBoxCreateNewDesktop_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupSecurityOptions, ValueCreateNewDesktop] = checkBoxCreateNewDesktop.Checked;
        }

        private void checkBoxIgnoreQuitPassword_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupSecurityOptions, ValueIgnoreQuitPassword] = checkBoxIgnoreQuitPassword.Checked;
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

            // Update the widgets
            textBoxStartURL            .Text    = settingString [StateNew, GroupGeneral, ValueStartURL];
            textBoxSEBServerURL        .Text    = settingString [StateNew, GroupGeneral, ValueSEBServerURL];
            textBoxAdminPassword       .Text    = settingString [StateNew, GroupGeneral, ValueAdminPassword];
            textBoxConfirmAdminPassword.Text    = settingString [StateNew, GroupGeneral, ValueConfirmAdminPassword];
            checkBoxAllowUserToQuitSEB .Checked = settingBoolean[StateNew, GroupGeneral, ValueAllowUserToQuitSEB];
            textBoxQuitPassword        .Text    = settingString [StateNew, GroupGeneral, ValueQuitPassword];
            textBoxConfirmQuitPassword .Text    = settingString [StateNew, GroupGeneral, ValueConfirmQuitPassword];

            radioButtonStartingAnExam    .Checked = settingBoolean[StateNew, GroupConfigFile, ValueStartingAnExam];
            radioButtonConfiguringAClient.Checked = settingBoolean[StateNew, GroupConfigFile, ValueConfiguringAClient];
            checkBoxAllowOpenPrefWindow  .Checked = settingBoolean[StateNew, GroupConfigFile, ValueAllowOpenPrefWindow];
          //comboBoxChooseIdentity.SelectedIndex  = settingInteger[StateNew, GroupConfigFile, ValueChooseIdentity];
          //comboBoxChooseIdentity.SelectedIndex  = 0;
            textBoxSettingsPassword       .Text   = settingString [StateNew, GroupConfigFile, ValueSettingsPassword];
            textBoxConfirmSettingsPassword.Text   = settingString [StateNew, GroupConfigFile, ValueConfirmSettingsPassword];

            radioButtonUseBrowserWindow  .Checked = settingBoolean[StateNew, GroupAppearance, ValueUseBrowserWindow];
            radioButtonUseFullScreenMode .Checked = settingBoolean[StateNew, GroupAppearance, ValueUseFullScreenMode];
            checkBoxEnableWindowToolbar  .Checked = settingBoolean[StateNew, GroupAppearance, ValueEnableWindowToolbar];
            checkBoxHideToolbarAsDefault .Checked = settingBoolean[StateNew, GroupAppearance, ValueHideToolbarAsDefault];
            checkBoxShowMenuBar          .Checked = settingBoolean[StateNew, GroupAppearance, ValueShowMenuBar];
            checkBoxDisplaySEBDockTaskBar.Checked = settingBoolean[StateNew, GroupAppearance, ValueDisplaySEBDockTaskBar];

            comboBoxMainWindowWidth   .SelectedIndex = settingInteger[StateNew, GroupAppearance, ValueMainWindowWidth   ] - 1;
            comboBoxMainWindowHeight  .SelectedIndex = settingInteger[StateNew, GroupAppearance, ValueMainWindowHeight  ] - 1;
             listBoxMainWindowPosition.SelectedIndex = settingInteger[StateNew, GroupAppearance, ValueMainWindowPosition] - 1;

            comboBoxNewWindowWidth   .SelectedIndex = settingInteger[StateNew, GroupBrowser, ValueNewWindowWidth   ] - 1;
            comboBoxNewWindowHeight  .SelectedIndex = settingInteger[StateNew, GroupBrowser, ValueNewWindowHeight  ] - 1;
             listBoxNewWindowPosition.SelectedIndex = settingInteger[StateNew, GroupBrowser, ValueNewWindowPosition] - 1;

             listBoxOpenLinksHTML .SelectedIndex = settingInteger[StateNew, GroupBrowser, ValueOpenLinksHTML] - 1;
             listBoxOpenLinksJava .SelectedIndex = settingInteger[StateNew, GroupBrowser, ValueOpenLinksJava] - 1;
            checkBoxBlockLinksHTML.Checked       = settingBoolean[StateNew, GroupBrowser, ValueBlockLinksHTML];
            checkBoxBlockLinksJava.Checked       = settingBoolean[StateNew, GroupBrowser, ValueBlockLinksJava];

            checkBoxEnablePlugIns           .Checked = settingBoolean[StateNew, GroupBrowser, ValueEnablePlugins];
            checkBoxEnableJava              .Checked = settingBoolean[StateNew, GroupBrowser, ValueEnableJava];
            checkBoxEnableJavaScript        .Checked = settingBoolean[StateNew, GroupBrowser, ValueEnableJavaScript];
            checkBoxBlockPopupWindows       .Checked = settingBoolean[StateNew, GroupBrowser, ValueBlockPopupWindows];
            checkBoxAllowBrowsingBackForward.Checked = settingBoolean[StateNew, GroupBrowser, ValueAllowBrowsingBackForward];
            checkBoxUseSEBWithoutBrowser    .Checked = settingBoolean[StateNew, GroupBrowser, ValueUseSEBWithoutBrowser];

            checkBoxAllowDownUploadingFiles  .Checked = settingBoolean[StateNew, GroupDownUploads, ValueAllowDownUploadingFiles];
            checkBoxOpenFilesAfterDownloading.Checked = settingBoolean[StateNew, GroupDownUploads, ValueOpenFilesAfterDownloading];
            checkBoxDownloadAndOpenPDFFiles  .Checked = settingBoolean[StateNew, GroupDownUploads, ValueDownloadAndOpenPDFFiles];
            labelSaveDownloadedFilesTo       .Text    = settingString [StateNew, GroupDownUploads, ValueSaveDownloadedFilesTo];
             listBoxChooseFileToUpload.SelectedIndex  = settingInteger[StateNew, GroupDownUploads, ValueChooseFileToUpload] - 1;

             textBoxBrowserExamKey                .Text    = settingString [StateNew, GroupExam, ValueBrowserExamKey];
             textBoxQuitURL                       .Text    = settingString [StateNew, GroupExam, ValueQuitURL];
            checkBoxCopyBrowserExamKey .Checked = settingBoolean[StateNew, GroupExam, ValueCopyBrowserExamKey];
            checkBoxSendBrowserExamKey.Checked = settingBoolean[StateNew, GroupExam, ValueSendBrowserExamKey];

            checkBoxMonitorProcesses         .Checked = settingBoolean[StateNew, GroupApplications, ValueMonitorProcesses];
            checkBoxAllowSwitchToApplications.Checked = settingBoolean[StateNew, GroupApplications, ValueAllowSwitchToApplications];
            checkBoxAllowFlashFullscreenMode .Checked = settingBoolean[StateNew, GroupApplications, ValueAllowFlashFullscreenMode];

             listBoxSEBServicePolicy.SelectedIndex = settingInteger[StateNew, GroupSecurity, ValueSEBServicePolicy] - 1;
            checkBoxAllowVirtualMachine.Checked    = settingBoolean[StateNew, GroupSecurity, ValueAllowVirtualMachine];
            checkBoxEnableLogging      .Checked    = settingBoolean[StateNew, GroupSecurity, ValueEnableLogging];

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

            listBoxExitKey1.SelectedIndex = settingInteger[StateNew, GroupExitKeys, ValueExitKey1] - 1;
            listBoxExitKey2.SelectedIndex = settingInteger[StateNew, GroupExitKeys, ValueExitKey2] - 1;
            listBoxExitKey3.SelectedIndex = settingInteger[StateNew, GroupExitKeys, ValueExitKey3] - 1;

            checkBoxCreateNewDesktop      .Checked = settingBoolean[StateNew, GroupSecurityOptions, ValueCreateNewDesktop];
            checkBoxIgnoreQuitPassword    .Checked = settingBoolean[StateNew, GroupSecurityOptions, ValueIgnoreQuitPassword];
        }






    } // end of   class     SebWindowsConfigForm
}     // end of   namespace SebWindowsConfig
