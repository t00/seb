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
using System.Xml.Serialization;



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
        const String TargetSebStarterIni = "SebStarter.ini";
        const String TargetSebStarterXml = "SebStarter.xml";
        const String TargetSebStarterSeb = "SebStarter.seb";

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
        const String MessageGeneral         = "General";
        const String MessageConfigFile      = "ConfigFile";
        const String MessageAppearance      = "Appearance";
        const String MessageBrowser         = "Browser";
        const String MessageDownUploads     = "DownUploads";
        const String MessageExam            = "Exam";
        const String MessageApplications    = "Applications";
        const String MessageNetwork         = "Network";
        const String MessageSecurity        = "Security";
        const String MessageRegistry        = "Registry";
        const String MessageInsideSeb       = "InsideSeb";
        const String MessageOutsideSeb      = "OutsideSeb";
        const String MessageHookedKeys      = "HookedKeys";
        const String MessageSpecialKeys     = "SpecialKeys";
        const String MessageFunctionKeys    = "FunctionKeys";
        const String MessageExitKeys        = "ExitKeys";
        const String MessageSecurityOptions = "SecurityOptions";

        // Group "General"
        const int ValueStartURL                     = 1;
        const int ValueSEBServerURL                 = 2;
        const int ValueAdministratorPassword        = 3;
        const int ValueConfirmAdministratorPassword = 4;
        const int ValueAllowUserToQuitSEB           = 5;
        const int ValueQuitPassword                 = 6;
        const int ValueConfirmQuitPassword          = 7;
        const int ValueQuitHashcode                 = 8;
        const int NumValueGeneral = 8;

        const String MessageStartURL                     = "StartURL";
        const String MessageSEBServerURL                 = "SEBServerURL";
        const String MessageAdministratorPassword        = "AdministratorPassword";
        const String MessageConfirmAdministratorPassword = "ConfirmAdministratorPassword";
        const String MessageAllowUserToQuitSEB           = "AllowUserToQuitSEB";
        const String MessageQuitPassword                 = "QuitPassword";
        const String MessageConfirmQuitPassword          = "ConfirmQuitPassword";
        const String MessageQuitHashcode                 = "QuitHashcode";

        // Group "Config File"
        const int ValueStartingAnExam               = 1;
        const int ValueConfiguringAClient           = 2;
        const int ValueAllowToOpenPreferencesWindow = 3;
        const int ValueChooseIdentity               = 4;
        const int ValueSettingsPassword             = 5;
        const int ValueConfirmSettingsPassword      = 6;
        const int NumValueConfigFile = 6;

        const String MessageStartingAnExam               = "StartingAnExam";
        const String MessageConfiguringAClient           = "ConfiguringAClient";
        const String MessageAllowToOpenPreferencesWindow = "AllowToOpenPreferencesWindow";
        const String MessageChooseIdentity               = "ChooseIdentity";
        const String MessageSettingsPassword             = "SettingsPassword";
        const String MessageConfirmSettingsPassword      = "ConfirmSettingsPassword";

        // Group "Appearance"
        const int ValueUseBrowserWindow            = 1;
        const int ValueUseFullScreenMode           = 2;
        const int ValueMainBrowserWindowWidth      = 3;
        const int ValueMainBrowserWindowHeight     = 4;
        const int ValueMainBrowserWindowPosition   = 5;
        const int ValueEnableBrowserWindowToolbar  = 6;
        const int ValueHideToolbarAsDefault        = 7;
        const int ValueShowMenuBar                 = 8;
        const int ValueDisplaySEBDockTaskBar       = 9;
        const int NumValueAppearance = 9;

        const String MessageUseBrowserWindow           = "UseBrowserWindow";
        const String MessageUseFullScreenMode          = "UseFullScreenMode";
        const String MessageMainBrowserWindowWidth     = "MainBrowserWindowWidth";
        const String MessageMainBrowserWindowHeight    = "MainBrowserWindowHeight";
        const String MessageMainBrowserWindowPosition  = "MainBrowserWindowPosition";
        const String MessageEnableBrowserWindowToolbar = "EnableBrowserWindowToolbar";
        const String MessageHideToolbarAsDefault       = "HideToolbarAsDefault";
        const String MessageShowMenuBar                = "ShowMenuBar";
        const String MessageDisplaySEBDockTaskBar      = "DisplaySEBDockTaskBar";

        // Group "Browser"
        const int ValueLinksInHTML                = 1;
        const int ValueLinksInJava                = 2;
        const int ValueBlockLinksToDiffServerHTML = 3;
        const int ValueBlockLinksToDiffServerJava = 4;
        const int ValueNewBrowserWindowWidth      = 5;
        const int ValueNewBrowserWindowHeight     = 6;
        const int ValueNewBrowserWindowPosition   = 7;
        const int ValueEnablePlugIns              = 8;
        const int ValueEnableJava                 = 9;
        const int ValueEnableJavaScript           = 10;
        const int ValueBlockPopupWindows          = 11;
        const int ValueAllowBrowsingBackForward   = 12;
        const int ValueUseSEBWithoutBrowserWindow = 13;
        const int NumValueBrowser = 13;

        const String MessageLinksInHTML                 = "LinksInHTML";
        const String MessageLinksInJava                 = "LinksInJava";
        const String MessageBlockLinksToDiffServerHTML  = "BlockLinksToDiffServerHTML";
        const String MessageBlockLinksToDiffServerJava  = "BlockLinksToDiffServerJava";
        const String MessageNewBrowserWindowWidth       = "NewBrowserWindowWidth";
        const String MessageNewBrowserWindowHeight      = "NewBrowserWindowHeight";
        const String MessageNewBrowserWindowPosition    = "NewBrowserWindowPosition";
        const String MessageEnablePlugIns               = "EnablePlugIns";
        const String MessageEnableJava                  = "EnableJava";
        const String MessageEnableJavaScript            = "EnableJavaScript";
        const String MessageBlockPopupWindows           = "BlockPopupWindows";
        const String MessageAllowBrowsingBackForward    = "AllowBrowsingBackForward";
        const String MessageUseSEBWithoutBrowserWindow  = "UseSEBWithoutBrowserWindow";

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
        const int ValueBrowserExamKey                 = 1;
        const int ValueCopyBrowserExamKeyToClipboard  = 2;
        const int ValueSendBrowserExamKeyInHTTPHeader = 3;
        const int ValueQuitURL                        = 4;
        const int NumValueExam = 4;

        const String MessageBrowserExamKey                 = "BrowserExamKey";
        const String MessageCopyBrowserExamKeyToClipboard  = "CopyBrowserExamKeyToClipboard";
        const String MessageSendBrowserExamKeyInHTTPHeader = "SendBrowserExamKeyInHTTPHeader";
        const String MessageQuitURL                        = "QuitURL";

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
        const int ValueAllowPreferencesWindow = 1;
        const int ValueCreateNewDesktop       = 2;
        const int ValueIgnoreQuitPassword     = 3;
        const int NumValueSecurityOptions = 3;

        const String MessageAllowPreferencesWindow    = "AllowPreferencesWindow";
        const String MessageCreateNewDesktop          = "CreateNewDesktop";
        const String MessageIgnoreQuitPassword        = "IgnoreQuitPassword";


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
        static String[]        windowWidthString = new String[5];
        static String[]       windowHeightString = new String[5];
        static String[]     windowPositionString = new String[4];
        static String[]         linkOpeningPolicyString = new String[4];
        static String[] chooseFileToUploadString = new String[4];
        static String[]   sebServicePolicyString = new String[4];
        static String[]        functionKeyString = new String[13];

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
        static SEBSettings            sebSettings            = new SEBSettings();
//      static SEBProtectionControler sebProtectionControler = new SEBProtectionControler();



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
            settingString [StateDef, GroupGeneral, ValueStartURL                    ] = "http://www.safeexambrowser.org";
            settingString [StateDef, GroupGeneral, ValueSEBServerURL                ] = "";
            settingString [StateDef, GroupGeneral, ValueAdministratorPassword       ] = "";
            settingString [StateDef, GroupGeneral, ValueConfirmAdministratorPassword] = "";
            settingBoolean[StateDef, GroupGeneral, ValueAllowUserToQuitSEB          ] = true;
            settingString [StateDef, GroupGeneral, ValueQuitPassword                ] = "";
            settingString [StateDef, GroupGeneral, ValueConfirmQuitPassword         ] = "";
            settingString [StateDef, GroupGeneral, ValueQuitHashcode                ] = "";

            // Default settings for group "Config File"
            settingBoolean[StateDef, GroupConfigFile, ValueStartingAnExam              ] = true;
            settingBoolean[StateDef, GroupConfigFile, ValueConfiguringAClient          ] = false;
            settingBoolean[StateDef, GroupConfigFile, ValueAllowToOpenPreferencesWindow] = true;
            settingString [StateDef, GroupConfigFile, ValueChooseIdentity              ] = "none";
            settingString [StateDef, GroupConfigFile, ValueSettingsPassword            ] = "";
            settingString [StateDef, GroupConfigFile, ValueConfirmSettingsPassword     ] = "";

            settingInteger[StateDef, GroupConfigFile, ValueChooseIdentity] = 0;

            // Default settings for group "Appearance"
            settingBoolean[StateDef, GroupAppearance, ValueUseBrowserWindow         ] = true;
            settingBoolean[StateDef, GroupAppearance, ValueUseFullScreenMode        ] = false;
            settingString [StateDef, GroupAppearance, ValueMainBrowserWindowWidth   ] = "100%";
            settingString [StateDef, GroupAppearance, ValueMainBrowserWindowHeight  ] = "100%";
            settingString [StateDef, GroupAppearance, ValueMainBrowserWindowPosition] = "Center";

            settingBoolean[StateDef, GroupAppearance, ValueEnableBrowserWindowToolbar] = false;
            settingBoolean[StateDef, GroupAppearance, ValueHideToolbarAsDefault      ] = false;
            settingBoolean[StateDef, GroupAppearance, ValueShowMenuBar               ] = false;
            settingBoolean[StateDef, GroupAppearance, ValueDisplaySEBDockTaskBar     ] = false;

            settingInteger[StateDef, GroupAppearance, ValueMainBrowserWindowWidth   ] = 0;
            settingInteger[StateDef, GroupAppearance, ValueMainBrowserWindowHeight  ] = 0;
            settingInteger[StateDef, GroupAppearance, ValueMainBrowserWindowPosition] = 2;

            // Default settings for group "Browser"
            settingString [StateDef, GroupBrowser, ValueLinksInHTML                 ] = "open in new window";
            settingString [StateDef, GroupBrowser, ValueLinksInJava                 ] = "open in new window";
            settingBoolean[StateDef, GroupBrowser, ValueBlockLinksToDiffServerHTML     ] = false;
            settingBoolean[StateDef, GroupBrowser, ValueBlockLinksToDiffServerJava] = false;

            settingString [StateDef, GroupBrowser, ValueNewBrowserWindowWidth           ] = "100%";
            settingString [StateDef, GroupBrowser, ValueNewBrowserWindowHeight          ] = "100%";
            settingString [StateDef, GroupBrowser, ValueNewBrowserWindowPosition        ] = "Center";

            settingBoolean[StateDef, GroupBrowser, ValueEnablePlugIns                   ] = true;
            settingBoolean[StateDef, GroupBrowser, ValueEnableJava                      ] = false;
            settingBoolean[StateDef, GroupBrowser, ValueEnableJavaScript                ] = true;
            settingBoolean[StateDef, GroupBrowser, ValueBlockPopupWindows               ] = false;
            settingBoolean[StateDef, GroupBrowser, ValueAllowBrowsingBackForward        ] = false;
            settingBoolean[StateDef, GroupBrowser, ValueUseSEBWithoutBrowserWindow      ] = false;

            settingInteger[StateDef, GroupBrowser, ValueLinksInHTML] = 1;
            settingInteger[StateDef, GroupBrowser, ValueLinksInJava] = 1;

            settingInteger[StateDef, GroupBrowser, ValueNewBrowserWindowWidth   ] = 0;
            settingInteger[StateDef, GroupBrowser, ValueNewBrowserWindowHeight  ] = 0;
            settingInteger[StateDef, GroupBrowser, ValueNewBrowserWindowPosition] = 2;

            // Default settings for group "DownUploads"
            settingBoolean[StateDef, GroupDownUploads, ValueAllowDownUploadingFiles  ] = true;
            settingString [StateDef, GroupDownUploads, ValueSaveDownloadedFilesTo    ] = "Desktop";
            settingBoolean[StateDef, GroupDownUploads, ValueOpenFilesAfterDownloading] = true;
            settingString [StateDef, GroupDownUploads, ValueChooseFileToUpload       ] = "manually with file requester";
            settingBoolean[StateDef, GroupDownUploads, ValueDownloadAndOpenPDFFiles  ] = false;

            settingInteger[StateDef, GroupDownUploads, ValueChooseFileToUpload] = 1;

            // Default settings for group "Exam"
            settingString [StateDef, GroupExam, ValueBrowserExamKey                ] = "";
            settingBoolean[StateDef, GroupExam, ValueCopyBrowserExamKeyToClipboard ] = false;
            settingBoolean[StateDef, GroupExam, ValueSendBrowserExamKeyInHTTPHeader] = true;
            settingString [StateDef, GroupExam, ValueQuitURL                       ] = "http://www.safeexambrowser.org/exit";

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
            settingBoolean[StateDef, GroupSecurityOptions, ValueAllowPreferencesWindow   ] = false;
            settingBoolean[StateDef, GroupSecurityOptions, ValueCreateNewDesktop         ] = true;
            settingBoolean[StateDef, GroupSecurityOptions, ValueIgnoreQuitPassword       ] = false;


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
                dataType[GroupGeneral        , value] = TypeString;
                dataType[GroupConfigFile     , value] = TypeBoolean;
                dataType[GroupAppearance     , value] = TypeBoolean;
                dataType[GroupBrowser        , value] = TypeBoolean;
                dataType[GroupDownUploads    , value] = TypeBoolean;
                dataType[GroupExam           , value] = TypeBoolean;
                dataType[GroupApplications   , value] = TypeBoolean;
                dataType[GroupNetwork        , value] = TypeBoolean;
                dataType[GroupSecurity       , value] = TypeBoolean;
                dataType[GroupRegistry       , value] = TypeBoolean;
                dataType[GroupInsideSeb      , value] = TypeBoolean;
                dataType[GroupOutsideSeb     , value] = TypeBoolean;
                dataType[GroupHookedKeys     , value] = TypeBoolean;
                dataType[GroupSpecialKeys    , value] = TypeBoolean;
                dataType[GroupFunctionKeys   , value] = TypeBoolean;
                dataType[GroupExitKeys       , value] = TypeString;
                dataType[GroupSecurityOptions, value] = TypeBoolean;
            }

            // Exceptional data types of some special values
            dataType[GroupGeneral   , ValueAllowUserToQuitSEB     ] = TypeBoolean;

            dataType[GroupConfigFile, ValueChooseIdentity         ] = TypeString;
            dataType[GroupConfigFile, ValueSettingsPassword       ] = TypeString;
            dataType[GroupConfigFile, ValueConfirmSettingsPassword] = TypeString;

            dataType[GroupAppearance, ValueMainBrowserWindowWidth   ] = TypeString;
            dataType[GroupAppearance, ValueMainBrowserWindowHeight  ] = TypeString;
            dataType[GroupAppearance, ValueMainBrowserWindowPosition] = TypeString;

            dataType[GroupBrowser, ValueNewBrowserWindowWidth   ] = TypeString;
            dataType[GroupBrowser, ValueNewBrowserWindowHeight  ] = TypeString;
            dataType[GroupBrowser, ValueNewBrowserWindowPosition] = TypeString;

            dataType[GroupBrowser, ValueLinksInHTML] = TypeString;
            dataType[GroupBrowser, ValueLinksInJava] = TypeString;

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
            valueString[GroupGeneral, ValueStartURL                    ] = MessageStartURL;
            valueString[GroupGeneral, ValueSEBServerURL                ] = MessageSEBServerURL;
            valueString[GroupGeneral, ValueAdministratorPassword       ] = MessageAdministratorPassword;
            valueString[GroupGeneral, ValueConfirmAdministratorPassword] = MessageConfirmAdministratorPassword;
            valueString[GroupGeneral, ValueAllowUserToQuitSEB          ] = MessageAllowUserToQuitSEB;
            valueString[GroupGeneral, ValueQuitPassword                ] = MessageQuitPassword;
            valueString[GroupGeneral, ValueConfirmQuitPassword         ] = MessageConfirmQuitPassword;
            valueString[GroupGeneral, ValueQuitHashcode                ] = MessageQuitHashcode;

            valueString[GroupConfigFile, ValueStartingAnExam              ] = MessageStartingAnExam;
            valueString[GroupConfigFile, ValueConfiguringAClient          ] = MessageConfiguringAClient;
            valueString[GroupConfigFile, ValueAllowToOpenPreferencesWindow] = MessageAllowToOpenPreferencesWindow;
            valueString[GroupConfigFile, ValueChooseIdentity              ] = MessageChooseIdentity;
            valueString[GroupConfigFile, ValueSettingsPassword            ] = MessageSettingsPassword;
            valueString[GroupConfigFile, ValueConfirmSettingsPassword     ] = MessageConfirmSettingsPassword;

            valueString[GroupAppearance, ValueUseBrowserWindow           ] = MessageUseBrowserWindow;
            valueString[GroupAppearance, ValueUseFullScreenMode          ] = MessageUseFullScreenMode;
            valueString[GroupAppearance, ValueMainBrowserWindowWidth     ] = MessageMainBrowserWindowWidth;
            valueString[GroupAppearance, ValueMainBrowserWindowHeight    ] = MessageMainBrowserWindowHeight;
            valueString[GroupAppearance, ValueMainBrowserWindowPosition  ] = MessageMainBrowserWindowPosition;
            valueString[GroupAppearance, ValueEnableBrowserWindowToolbar ] = MessageEnableBrowserWindowToolbar;
            valueString[GroupAppearance, ValueHideToolbarAsDefault       ] = MessageHideToolbarAsDefault;
            valueString[GroupAppearance, ValueShowMenuBar                ] = MessageShowMenuBar;
            valueString[GroupAppearance, ValueDisplaySEBDockTaskBar      ] = MessageDisplaySEBDockTaskBar;

            valueString[GroupBrowser, ValueLinksInHTML                 ] = MessageLinksInHTML;
            valueString[GroupBrowser, ValueLinksInJava                 ] = MessageLinksInJava;
            valueString[GroupBrowser, ValueBlockLinksToDiffServerHTML     ] = MessageBlockLinksToDiffServerHTML;
            valueString[GroupBrowser, ValueBlockLinksToDiffServerJava] = MessageBlockLinksToDiffServerJava;
            valueString[GroupBrowser, ValueNewBrowserWindowWidth           ] = MessageNewBrowserWindowWidth;
            valueString[GroupBrowser, ValueNewBrowserWindowHeight          ] = MessageNewBrowserWindowHeight;
            valueString[GroupBrowser, ValueNewBrowserWindowPosition        ] = MessageNewBrowserWindowPosition;
            valueString[GroupBrowser, ValueEnablePlugIns                   ] = MessageEnablePlugIns;
            valueString[GroupBrowser, ValueEnableJava                      ] = MessageEnableJava;
            valueString[GroupBrowser, ValueEnableJavaScript                ] = MessageEnableJavaScript;
            valueString[GroupBrowser, ValueBlockPopupWindows               ] = MessageBlockPopupWindows;
            valueString[GroupBrowser, ValueAllowBrowsingBackForward        ] = MessageAllowBrowsingBackForward;
            valueString[GroupBrowser, ValueUseSEBWithoutBrowserWindow      ] = MessageUseSEBWithoutBrowserWindow;

            valueString[GroupDownUploads, ValueAllowDownUploadingFiles  ] = MessageAllowDownUploadingFiles;
            valueString[GroupDownUploads, ValueSaveDownloadedFilesTo    ] = MessageSaveDownloadedFilesTo;
            valueString[GroupDownUploads, ValueOpenFilesAfterDownloading] = MessageOpenFilesAfterDownloading;
            valueString[GroupDownUploads, ValueChooseFileToUpload       ] = MessageChooseFileToUpload;
            valueString[GroupDownUploads, ValueDownloadAndOpenPDFFiles  ] = MessageDownloadAndOpenPDFFiles;

            valueString[GroupExam, ValueBrowserExamKey                ] = MessageBrowserExamKey;
            valueString[GroupExam, ValueCopyBrowserExamKeyToClipboard ] = MessageCopyBrowserExamKeyToClipboard;
            valueString[GroupExam, ValueSendBrowserExamKeyInHTTPHeader] = MessageSendBrowserExamKeyInHTTPHeader;
            valueString[GroupExam, ValueQuitURL                       ] = MessageQuitURL;

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

            valueString[GroupSecurityOptions, ValueAllowPreferencesWindow] = MessageAllowPreferencesWindow;
            valueString[GroupSecurityOptions, ValueCreateNewDesktop      ] = MessageCreateNewDesktop;
            valueString[GroupSecurityOptions, ValueIgnoreQuitPassword    ] = MessageIgnoreQuitPassword;


            // Define the strings for the encryption identity
            chooseIdentityStringList.Add("none");
            chooseIdentityStringList.Add("alpha");
            chooseIdentityStringList.Add("beta");
            chooseIdentityStringList.Add("gamma");
            chooseIdentityStringList.Add("delta");
            String[] chooseIdentityStringArray = chooseIdentityStringList.ToArray();

            // Define the strings for the link treatment
            linkOpeningPolicyString[0] = "";
            linkOpeningPolicyString[1] = "open in new window";
            linkOpeningPolicyString[2] = "open in same window";
            linkOpeningPolicyString[3] = "get generally blocked";

            // Define the strings for the window width
            windowWidthString[0] = "";
            windowWidthString[1] = "50%";
            windowWidthString[2] = "100%";
            windowWidthString[3] = "800";
            windowWidthString[4] = "1000";

            // Define the strings for the window height
            windowHeightString[0] = "";
            windowHeightString[1] = "80%";
            windowHeightString[2] = "100%";
            windowHeightString[3] = "600";
            windowHeightString[4] = "800";

            // Define the strings for the window horizontal positioning
            windowPositionString[0] = "";
            windowPositionString[1] = "Left";
            windowPositionString[2] = "Center";
            windowPositionString[3] = "Right";

            // Define the strings for the link treatment
            chooseFileToUploadString[0] = "";
            chooseFileToUploadString[1] = "manually with file requester";
            chooseFileToUploadString[2] = "by attempting to upload the same file downloaded before";
            chooseFileToUploadString[3] = "by only allowing to upload the same file downloaded before";

            // Define the strings for the SEB service policy
            sebServicePolicyString[0] = "";
            sebServicePolicyString[1] = "allow to use SEB only with service";
            sebServicePolicyString[2] = "display warning when service is not running";
            sebServicePolicyString[3] = "allow to run SEB without service";

            // Define the strings for the function keys F1, F2, ..., F12
            for (int i = 1; i <= 12; i++)
            {
                functionKeyString[i] = "F" + i.ToString();
            }


            // Try to open the ini file (SebStarter.ini)
            // given in the local directory (where SebWindowsConfig.exe was called)
            currentDireSebStarterIni = Directory.GetCurrentDirectory();
            currentFileSebStarterIni = "";
            currentPathSebStarterIni = "";

             targetDireSebStarterIni = Directory.GetCurrentDirectory();
             targetFileSebStarterIni = TargetSebStarterIni;
             targetPathSebStarterIni = Path.GetFullPath(TargetSebStarterIni);

            // Read the settings from the standard configuration file
            OpenIniFile(targetPathSebStarterIni);
          //OpenXmlFile(targetPathSebStarterIni);
          //OpenSebFile(targetPathSebStarterIni);

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
            // Link Opening Policy for Requesting/JavaScript
            // Choose File To Upload
            // SEB Service Policy
            // Exit Key Sequence (exit keys 1,2,3)

            String tmpStringMainWindowWidth    = settingString[StateTmp, GroupAppearance, ValueMainBrowserWindowWidth];
            String tmpStringMainWindowHeight   = settingString[StateTmp, GroupAppearance, ValueMainBrowserWindowHeight];
            String tmpStringMainWindowPosition = settingString[StateTmp, GroupAppearance, ValueMainBrowserWindowPosition];

            String tmpStringNewWindowWidth     = settingString[StateTmp, GroupBrowser   , ValueNewBrowserWindowWidth];
            String tmpStringNewWindowHeight    = settingString[StateTmp, GroupBrowser   , ValueNewBrowserWindowHeight];
            String tmpStringNewWindowPosition  = settingString[StateTmp, GroupBrowser   , ValueNewBrowserWindowPosition];

            String tmpStringLinksRequesting    = settingString[StateTmp, GroupBrowser    , ValueLinksInHTML];
            String tmpStringLinksJavaScript    = settingString[StateTmp, GroupBrowser    , ValueLinksInJava];
            String tmpStringChooseFileToUpload = settingString[StateTmp, GroupDownUploads, ValueChooseFileToUpload];
            String tmpStringSEBServicePolicy   = settingString[StateTmp, GroupSecurity   , ValueSEBServicePolicy];

            String tmpStringExitKey1           = settingString[StateTmp, GroupExitKeys, ValueExitKey1];
            String tmpStringExitKey2           = settingString[StateTmp, GroupExitKeys, ValueExitKey2];
            String tmpStringExitKey3           = settingString[StateTmp, GroupExitKeys, ValueExitKey3];

            int tmpIndexMainWindowWidth    = 0;
            int tmpIndexMainWindowHeight   = 0;
            int tmpIndexMainWindowPosition = 0;

            int tmpIndexNewWindowWidth    = 0;
            int tmpIndexNewWindowHeight   = 0;
            int tmpIndexNewWindowPosition = 0;

            int tmpIndexLinksRequesting    = 0;
            int tmpIndexLinksJavaScript    = 0;
            int tmpIndexChooseFileToUpload = 0;
            int tmpIndexSEBServicePolicy   = 0;

            int tmpIndexExitKey1 = 0;
            int tmpIndexExitKey2 = 0;
            int tmpIndexExitKey3 = 0;

            for (int index = 1; index <= 20; index++)
            {
                String width    =        windowWidthString[index];
                String height   =       windowHeightString[index];
                String position =     windowPositionString[index];
                String link     =  linkOpeningPolicyString[index];
                String upload   = chooseFileToUploadString[index];
                String service  =   sebServicePolicyString[index];
                String key      =        functionKeyString[index];

                if (tmpStringMainWindowWidth   .Equals(width   )) tmpIndexMainWindowWidth    = index;
                if (tmpStringMainWindowHeight  .Equals(height  )) tmpIndexMainWindowHeight   = index;
                if (tmpStringMainWindowPosition.Equals(position)) tmpIndexMainWindowPosition = index;

                if (tmpStringNewWindowWidth   .Equals(width   )) tmpIndexNewWindowWidth    = index;
                if (tmpStringNewWindowHeight  .Equals(height  )) tmpIndexNewWindowHeight   = index;
                if (tmpStringNewWindowPosition.Equals(position)) tmpIndexNewWindowPosition = index;

                if (tmpStringLinksRequesting   .Equals(link   )) tmpIndexLinksRequesting    = index;
                if (tmpStringLinksJavaScript   .Equals(link   )) tmpIndexLinksJavaScript    = index;
                if (tmpStringChooseFileToUpload.Equals(upload )) tmpIndexChooseFileToUpload = index;
                if (tmpStringSEBServicePolicy  .Equals(service)) tmpIndexSEBServicePolicy   = index;

                if (tmpStringExitKey1.Equals(key)) tmpIndexExitKey1 = index;
                if (tmpStringExitKey2.Equals(key)) tmpIndexExitKey2 = index;
                if (tmpStringExitKey3.Equals(key)) tmpIndexExitKey3 = index;
            }

            settingInteger[StateTmp, GroupAppearance , ValueMainBrowserWindowWidth   ] = tmpIndexMainWindowWidth;
            settingInteger[StateTmp, GroupAppearance , ValueMainBrowserWindowHeight  ] = tmpIndexMainWindowHeight;
            settingInteger[StateTmp, GroupAppearance , ValueMainBrowserWindowPosition] = tmpIndexMainWindowPosition;

            settingInteger[StateTmp, GroupBrowser    , ValueNewBrowserWindowWidth    ] = tmpIndexNewWindowWidth;
            settingInteger[StateTmp, GroupBrowser    , ValueNewBrowserWindowHeight   ] = tmpIndexNewWindowHeight;
            settingInteger[StateTmp, GroupBrowser    , ValueNewBrowserWindowPosition ] = tmpIndexNewWindowPosition;

            settingInteger[StateTmp, GroupBrowser    , ValueLinksInHTML          ] = tmpIndexLinksRequesting;
            settingInteger[StateTmp, GroupBrowser    , ValueLinksInJava          ] = tmpIndexLinksJavaScript;
            settingInteger[StateTmp, GroupDownUploads, ValueChooseFileToUpload       ] = tmpIndexChooseFileToUpload;
            settingInteger[StateTmp, GroupSecurity   , ValueSEBServicePolicy         ] = tmpIndexSEBServicePolicy;

            settingInteger[StateTmp, GroupExitKeys   , ValueExitKey1                 ] = tmpIndexExitKey1;
            settingInteger[StateTmp, GroupExitKeys   , ValueExitKey2                 ] = tmpIndexExitKey2;
            settingInteger[StateTmp, GroupExitKeys   , ValueExitKey3                 ] = tmpIndexExitKey3;


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

            int newIndexMainWindowWidth    = settingInteger[StateNew, GroupAppearance, ValueMainBrowserWindowWidth];
            int newIndexMainWindowHeight   = settingInteger[StateNew, GroupAppearance, ValueMainBrowserWindowHeight];
            int newIndexMainWindowPosition = settingInteger[StateNew, GroupAppearance, ValueMainBrowserWindowPosition];

            int newIndexNewWindowWidth     = settingInteger[StateNew, GroupBrowser   , ValueNewBrowserWindowWidth];
            int newIndexNewWindowHeight    = settingInteger[StateNew, GroupBrowser   , ValueNewBrowserWindowHeight];
            int newIndexNewWindowPosition  = settingInteger[StateNew, GroupBrowser   , ValueNewBrowserWindowPosition];

            int newIndexLinksRequesting    = settingInteger[StateNew, GroupBrowser    , ValueLinksInHTML];
            int newIndexLinksJavaScript    = settingInteger[StateNew, GroupBrowser    , ValueLinksInJava];
            int newIndexChooseFileToUpload = settingInteger[StateNew, GroupDownUploads, ValueChooseFileToUpload];
            int newIndexSEBServicePolicy   = settingInteger[StateNew, GroupSecurity   , ValueSEBServicePolicy];

            int newIndexExitKey1 = settingInteger[StateNew, GroupExitKeys, ValueExitKey1];
            int newIndexExitKey2 = settingInteger[StateNew, GroupExitKeys, ValueExitKey2];
            int newIndexExitKey3 = settingInteger[StateNew, GroupExitKeys, ValueExitKey3];

            settingString[StateNew, GroupConfigFile, ValueChooseIdentity           ] = chooseIdentityStringList[newIndexChooseIdentity];

            settingString[StateNew, GroupAppearance, ValueMainBrowserWindowWidth   ] =    windowWidthString[newIndexMainWindowWidth];
            settingString[StateNew, GroupAppearance, ValueMainBrowserWindowHeight  ] =   windowHeightString[newIndexMainWindowHeight];
            settingString[StateNew, GroupAppearance, ValueMainBrowserWindowPosition] = windowPositionString[newIndexMainWindowPosition];

            settingString[StateNew, GroupBrowser   , ValueNewBrowserWindowWidth    ] =    windowWidthString[newIndexNewWindowWidth];
            settingString[StateNew, GroupBrowser   , ValueNewBrowserWindowHeight   ] =   windowHeightString[newIndexNewWindowHeight];
            settingString[StateNew, GroupBrowser   , ValueNewBrowserWindowPosition ] = windowPositionString[newIndexNewWindowPosition];

            settingString[StateNew, GroupBrowser    , ValueLinksInHTML   ] =  linkOpeningPolicyString[newIndexLinksRequesting];
            settingString[StateNew, GroupBrowser    , ValueLinksInJava   ] =  linkOpeningPolicyString[newIndexLinksJavaScript];
            settingString[StateNew, GroupDownUploads, ValueChooseFileToUpload] = chooseFileToUploadString[newIndexChooseFileToUpload];
            settingString[StateTmp, GroupSecurity   , ValueSEBServicePolicy  ] =   sebServicePolicyString[newIndexSEBServicePolicy];

            settingString[StateNew, GroupExitKeys, ValueExitKey1] = functionKeyString[newIndexExitKey1];
            settingString[StateNew, GroupExitKeys, ValueExitKey2] = functionKeyString[newIndexExitKey2];
            settingString[StateNew, GroupExitKeys, ValueExitKey3] = functionKeyString[newIndexExitKey3];

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
                XmlSerializer deserializer = new XmlSerializer(typeof(SEBSettings));
                TextReader      textReader = new StreamReader (fileName);

                // Parse the XML structure into a C# object
                sebSettings = (SEBSettings)deserializer.Deserialize(textReader);

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
/*
                // Decrypt seb client settings
                string decriptedSebClientSettings = sebProtectionControler.DecryptSebClientSettings(encryptedTextWithPrefix);

                // Deserialise seb client settings
                // Deserialise decrypted string
                decriptedSebClientSettings = decriptedSebClientSettings.Trim();
                MemoryStream     memStream = new MemoryStream(Encoding.UTF8.GetBytes(decriptedSebClientSettings));

                XmlSerializer deserializer = new XmlSerializer(typeof(SEBSettings));
                //TextReader textReader = new StreamReader(fileName);

                // Parse the XML structure into a C# object
                sebSettings = (SEBSettings)deserializer.Deserialize(memStream);

                // Close the .seb file
                //textReader.Close();
*/

                // Open the .seb file for reading
                XmlSerializer deserializer = new XmlSerializer(typeof(SEBSettings));
                TextReader      textReader = new StreamReader (fileName);

                // Parse the XML structure into a C# object
                sebSettings = (SEBSettings)deserializer.Deserialize(textReader);

                // Close the .seb file
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
                XmlSerializer serializer = new XmlSerializer(typeof(SEBSettings));
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

                // Open the .seb file for writing
                XmlSerializer serializer = new XmlSerializer(typeof(SEBSettings));
                TextWriter    textWriter = new StreamWriter(fileName);

                // Copy the C# object into an XML structure
                serializer.Serialize(textWriter, sebSettings);

                // Close the .seb file
                textWriter.Close();

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

            settingString [StateTmp, GroupGeneral, ValueStartURL             ] = sebSettings.getUrlAddress("startURL").Url;
          //settingString [StateTmp, GroupGeneral, ValueSEBServerURL         ] = sebSettings.getUrlAddress("***").Url;
            settingString [StateTmp, GroupGeneral, ValueAdministratorPassword] = sebSettings.getPassword("hashedAdminPassword").Value;
            settingBoolean[StateTmp, GroupGeneral, ValueAllowUserToQuitSEB   ] = sebSettings.getSecurityOption("allowQuit").getBool();
            settingString [StateTmp, GroupGeneral, ValueQuitPassword         ] = sebSettings.getPassword("hashedQuitPassword").Value;

          //settingBoolean[StateTmp, GroupConfigFile, ValueStartingAnExam        ] = sebSettings.getSecurityOption("***").getBool();
          //settingBoolean[StateTmp, GroupConfigFile, ValueConfiguringAClient    ] = sebSettings.getSecurityOption("***").getBool();
            settingBoolean[StateTmp, GroupConfigFile, ValueAllowPreferencesWindow] = sebSettings.getSecurityOption("allowPreferencesWindow").getBool();

          //settingString [StateTmp, GroupConfigFile, ValueChooseIdentity  ] = sebSettings.getPassword("***").Value;
          //settingString [StateTmp, GroupConfigFile, ValueSettingsPassword] = sebSettings.getPassword("***").Value;

          //settingString [StateTmp, GroupExam   , ValueQuitUrl] = sebSettings.getUrlAddress("quitURL" ).Url;

            return true;
        }



        // ****************************************************************
        // Convert arrays to C# object (to be written to .xml or .seb file)
        // ****************************************************************
        private Boolean ConvertArraysToCSharpObject()
        {
            // Copy the arrays "settingString"/"settingBoolean" to the C# object "sebSettings"

            sebSettings.getUrlAddress("startURL")         .Url   = settingString [StateNew, GroupGeneral, ValueStartURL];
          //sebSettings.getUrlAddress("********")         .Url   = settingString [StateNew, GroupGeneral, ValueSEBServerURL];
            sebSettings.getPassword("hashedAdminPassword").Value = settingString [StateNew, GroupGeneral, ValueAdministratorPassword];
            sebSettings.getSecurityOption("allowQuit")    .setBool(settingBoolean[StateNew, GroupGeneral, ValueAllowUserToQuitSEB]);
            sebSettings.getPassword("hashedQuitPassword") .Value = settingString [StateNew, GroupGeneral, ValueQuitPassword];

          //sebSettings.getSecurityOption("**********************").setBool(settingBoolean[StateNew, GroupConfigFile, ValueStartingAnExam]);
          //sebSettings.getSecurityOption("**********************").setBool(settingBoolean[StateNew, GroupConfigFile, ValueConfiguringAClient]);
            sebSettings.getSecurityOption("allowPreferencesWindow").setBool(settingBoolean[StateNew, GroupConfigFile, ValueAllowPreferencesWindow]);

          //sebSettings.getPassword("***").Value = settingString[StateNew, GroupConfigFile, ValueChooseIdentity  ];
          //sebSettings.getPassword("***").Value = settingString[StateNew, GroupConfigFile, ValueSettingsPassword];

          //sebSettings.getUrlAddress("quitURL").Url = settingString[StateNew, GroupExam, ValueQuitUrl];

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

        private void textBoxAdministratorPassword_TextChanged(object sender, EventArgs e)
        {
            settingString[StateNew, GroupGeneral, ValueAdministratorPassword] = textBoxAdministratorPassword.Text;
        }

        private void textBoxConfirmAdministratorPassword_TextChanged(object sender, EventArgs e)
        {
            settingString[StateNew, GroupGeneral, ValueConfirmAdministratorPassword] = textBoxConfirmAdministratorPassword.Text;
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
            settingString[StateNew, GroupGeneral, ValueQuitHashcode] = newStringQuitHashcode;
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
            String fileNameIni = fileNameRaw + ".ini";
            String fileNameXml = fileNameRaw + ".xml";
            String fileNameSeb = fileNameRaw + ".seb";

            // Save the configuration file so that nothing gets lost
            SaveIniFile(fileNameIni);
          //SaveXmlFile(fileNameXml);
          //SaveSebFile(fileNameSeb);
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

        private void checkBoxAllowToOpenPreferencesWindowOnClient_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupConfigFile, ValueAllowToOpenPreferencesWindow] = checkBoxAllowToOpenPreferencesWindow.Checked;
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
            String fileNameIni = fileNameRaw + ".ini";
            String fileNameXml = fileNameRaw + ".xml";
            String fileNameSeb = fileNameRaw + ".seb";

            // If the user clicked "OK", read the settings from the configuration file
            OpenIniFile(fileNameIni);
          //OpenXmlFile(fileNameXml);
          //OpenSebFile(fileNameSeb);
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
            String fileNameIni = fileNameRaw + ".ini";
            String fileNameXml = fileNameRaw + ".xml";
            String fileNameSeb = fileNameRaw + ".seb";

            // If the user clicked "OK", write the settings to the configuration file
            SaveIniFile(fileNameIni);
          //SaveXmlFile(fileNameXml);
          //SaveSebFile(fileNameSeb);
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

        private void listBoxMainBrowserWindowHorizPos_SelectedIndexChanged(object sender, EventArgs e)
        {
            settingInteger[StateNew, GroupAppearance, ValueMainBrowserWindowPosition] = listBoxMainBrowserWindowHorizPos.SelectedIndex + 1;
        }

        private void checkBoxEnableBrowserWindowToolbar_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupAppearance, ValueEnableBrowserWindowToolbar] = checkBoxEnableBrowserWindowToolbar.Checked;
            checkBoxHideToolbarAsDefault.Enabled = checkBoxEnableBrowserWindowToolbar.Checked;
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
        private void listBoxLinksRequesting_SelectedIndexChanged(object sender, EventArgs e)
        {
            settingInteger[StateNew, GroupBrowser, ValueLinksInHTML] = listBoxLinksRequesting.SelectedIndex + 1;
        }

        private void checkBoxBlockLinksWhenDirectingToADifferentServer_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupBrowser, ValueBlockLinksToDiffServerHTML] = checkBoxBlockLinksToDifferentServer.Checked;
        }

        private void comboBoxNewBrowserWindowWidth_SelectedIndexChanged(object sender, EventArgs e)
        {
            settingInteger[StateNew, GroupBrowser, ValueNewBrowserWindowWidth] = comboBoxNewBrowserWindowWidth.SelectedIndex + 1;
            settingString [StateNew, GroupBrowser, ValueNewBrowserWindowWidth] = comboBoxNewBrowserWindowWidth.Text;
        }

        private void comboBoxNewBrowserWindowWidth_TextUpdate(object sender, EventArgs e)
        {
            settingInteger[StateNew, GroupBrowser, ValueNewBrowserWindowWidth] = comboBoxNewBrowserWindowWidth.SelectedIndex + 1;
            settingString [StateNew, GroupBrowser, ValueNewBrowserWindowWidth] = comboBoxNewBrowserWindowWidth.Text;
        }

        private void comboBoxNewBrowserWindowHeight_SelectedIndexChanged(object sender, EventArgs e)
        {
            settingInteger[StateNew, GroupBrowser, ValueNewBrowserWindowHeight] = comboBoxNewBrowserWindowHeight.SelectedIndex + 1;
            settingString [StateNew, GroupBrowser, ValueNewBrowserWindowHeight] = comboBoxNewBrowserWindowHeight.Text;
        }

        private void comboBoxNewBrowserWindowHeight_TextUpdate(object sender, EventArgs e)
        {
            settingInteger[StateNew, GroupBrowser, ValueNewBrowserWindowHeight] = comboBoxNewBrowserWindowHeight.SelectedIndex + 1;
            settingString [StateNew, GroupBrowser, ValueNewBrowserWindowHeight] = comboBoxNewBrowserWindowHeight.Text;
        }

        private void listBoxNewBrowserWindowHorizPos_SelectedIndexChanged(object sender, EventArgs e)
        {
            settingInteger[StateNew, GroupBrowser, ValueNewBrowserWindowPosition] = listBoxNewBrowserWindowHorizPos.SelectedIndex + 1;
        }

        private void listBoxLinksJavaScript_SelectedIndexChanged(object sender, EventArgs e)
        {
            settingInteger[StateNew, GroupBrowser, ValueLinksInJava] = listBoxLinksJavaScript.SelectedIndex + 1;
        }

        private void checkBoxBlockJavaScriptWhenDirectingToADifferentServer_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupBrowser, ValueBlockLinksToDiffServerJava] = checkBoxBlockJavaScriptToDifferentServer.Checked;
        }

        private void checkBoxEnablePlugins_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupBrowser, ValueEnablePlugIns] = checkBoxEnablePlugIns.Checked;
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

        private void checkBoxUseSEBWithoutBrowserWindow_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupBrowser, ValueUseSEBWithoutBrowserWindow] = checkBoxUseSEBWithoutBrowserWindow.Checked;
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

        private void checkBoxCopyBrowserExamKeyToClipboard_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupExam, ValueCopyBrowserExamKeyToClipboard] = checkBoxCopyBrowserExamKeyToClipboard.Checked;
        }

        private void checkBoxSendBrowserExamKeyInHTTPHeader_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupExam, ValueSendBrowserExamKeyInHTTPHeader] = checkBoxSendBrowserExamKeyInHTTPHeader.Checked;
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
            settingBoolean[StateNew, GroupOutsideSeb, ValueEnableSwitchUser] = checkBoxInsideSebEnableSwitchUser.Checked;
        }

        private void checkBoxOutsideSebEnableLockThisComputer_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupOutsideSeb, ValueEnableLockThisComputer] = checkBoxInsideSebEnableLockThisComputer.Checked;
        }

        private void checkBoxOutsideSebEnableChangeAPassword_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupOutsideSeb, ValueEnableChangeAPassword] = checkBoxInsideSebEnableChangeAPassword.Checked;
        }

        private void checkBoxOutsideSebEnableStartTaskManager_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupOutsideSeb, ValueEnableStartTaskManager] = checkBoxInsideSebEnableStartTaskManager.Checked;
        }

        private void checkBoxOutsideSebEnableLogOff_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupOutsideSeb, ValueEnableLogOff] = checkBoxInsideSebEnableLogOff.Checked;
        }

        private void checkBoxOutsideSebEnableShutDown_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupOutsideSeb, ValueEnableShutDown] = checkBoxInsideSebEnableShutDown.Checked;
        }

        private void checkBoxOutsideSebEnableEaseOfAccess_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupOutsideSeb, ValueEnableEaseOfAccess] = checkBoxInsideSebEnableEaseOfAccess.Checked;
        }

        private void checkBoxOutsideSebEnableVmWareClientShade_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupOutsideSeb, ValueEnableVmWareClientShade] = checkBoxInsideSebEnableVmWareClientShade.Checked;
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
            settingBoolean[StateNew, GroupFunctionKeys, ValueEnableF4] = checkBoxEnableF3.Checked;
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
        private void checkBoxAllowPreferencesWindow_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupSecurityOptions, ValueAllowPreferencesWindow] = checkBoxAllowPreferencesWindow.Checked;
        }

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
            textBoxStartURL                    .Text    = settingString [StateNew, GroupGeneral, ValueStartURL];
            textBoxSEBServerURL                .Text    = settingString [StateNew, GroupGeneral, ValueSEBServerURL];
            textBoxAdministratorPassword       .Text    = settingString [StateNew, GroupGeneral, ValueAdministratorPassword];
            textBoxConfirmAdministratorPassword.Text    = settingString [StateNew, GroupGeneral, ValueConfirmAdministratorPassword];
            checkBoxAllowUserToQuitSEB         .Checked = settingBoolean[StateNew, GroupGeneral, ValueAllowUserToQuitSEB];
            textBoxQuitPassword                .Text    = settingString [StateNew, GroupGeneral, ValueQuitPassword];
            textBoxConfirmQuitPassword         .Text    = settingString [StateNew, GroupGeneral, ValueConfirmQuitPassword];

            radioButtonStartingAnExam           .Checked = settingBoolean[StateNew, GroupConfigFile, ValueStartingAnExam];
            radioButtonConfiguringAClient       .Checked = settingBoolean[StateNew, GroupConfigFile, ValueConfiguringAClient];
            checkBoxAllowToOpenPreferencesWindow.Checked = settingBoolean[StateNew, GroupConfigFile, ValueAllowToOpenPreferencesWindow];
          //comboBoxChooseIdentity      .SelectedIndex   = settingInteger[StateNew, GroupConfigFile, ValueChooseIdentity];
          //comboBoxChooseIdentity      .SelectedIndex   = 0;
            textBoxSettingsPassword             .Text    = settingString [StateNew, GroupConfigFile, ValueSettingsPassword];
            textBoxConfirmSettingsPassword      .Text    = settingString [StateNew, GroupConfigFile, ValueConfirmSettingsPassword];

            radioButtonUseBrowserWindow        .Checked       = settingBoolean[StateNew, GroupAppearance, ValueUseBrowserWindow];
            radioButtonUseFullScreenMode       .Checked       = settingBoolean[StateNew, GroupAppearance, ValueUseFullScreenMode];
            checkBoxEnableBrowserWindowToolbar .Checked       = settingBoolean[StateNew, GroupAppearance, ValueEnableBrowserWindowToolbar];
            checkBoxHideToolbarAsDefault       .Checked       = settingBoolean[StateNew, GroupAppearance, ValueHideToolbarAsDefault];
            checkBoxShowMenuBar                .Checked       = settingBoolean[StateNew, GroupAppearance, ValueShowMenuBar];
            checkBoxDisplaySEBDockTaskBar      .Checked       = settingBoolean[StateNew, GroupAppearance, ValueDisplaySEBDockTaskBar];

            comboBoxMainBrowserWindowWidth   .SelectedIndex = settingInteger[StateNew, GroupAppearance, ValueMainBrowserWindowWidth   ] - 1;
            comboBoxMainBrowserWindowHeight  .SelectedIndex = settingInteger[StateNew, GroupAppearance, ValueMainBrowserWindowHeight  ] - 1;
             listBoxMainBrowserWindowHorizPos.SelectedIndex = settingInteger[StateNew, GroupAppearance, ValueMainBrowserWindowPosition] - 1;

            comboBoxNewBrowserWindowWidth    .SelectedIndex = settingInteger[StateNew, GroupBrowser, ValueNewBrowserWindowWidth   ] - 1;
            comboBoxNewBrowserWindowHeight   .SelectedIndex = settingInteger[StateNew, GroupBrowser, ValueNewBrowserWindowHeight  ] - 1;
             listBoxNewBrowserWindowHorizPos .SelectedIndex = settingInteger[StateNew, GroupBrowser, ValueNewBrowserWindowPosition] - 1;

             listBoxLinksRequesting           .SelectedIndex = settingInteger[StateNew, GroupBrowser, ValueLinksInHTML] - 1;
             listBoxLinksJavaScript           .SelectedIndex = settingInteger[StateNew, GroupBrowser, ValueLinksInJava] - 1;
            checkBoxBlockLinksToDifferentServer     .Checked = settingBoolean[StateNew, GroupBrowser, ValueBlockLinksToDiffServerHTML];
            checkBoxBlockJavaScriptToDifferentServer.Checked = settingBoolean[StateNew, GroupBrowser, ValueBlockLinksToDiffServerJava];

            checkBoxEnablePlugIns             .Checked = settingBoolean[StateNew, GroupBrowser, ValueEnablePlugIns];
            checkBoxEnableJava                .Checked = settingBoolean[StateNew, GroupBrowser, ValueEnableJava];
            checkBoxEnableJavaScript          .Checked = settingBoolean[StateNew, GroupBrowser, ValueEnableJavaScript];
            checkBoxBlockPopupWindows         .Checked = settingBoolean[StateNew, GroupBrowser, ValueBlockPopupWindows];
            checkBoxAllowBrowsingBackForward  .Checked = settingBoolean[StateNew, GroupBrowser, ValueAllowBrowsingBackForward];
            checkBoxUseSEBWithoutBrowserWindow.Checked = settingBoolean[StateNew, GroupBrowser, ValueUseSEBWithoutBrowserWindow];

            checkBoxAllowDownUploadingFiles  .Checked = settingBoolean[StateNew, GroupDownUploads, ValueAllowDownUploadingFiles];
            checkBoxOpenFilesAfterDownloading.Checked = settingBoolean[StateNew, GroupDownUploads, ValueOpenFilesAfterDownloading];
            checkBoxDownloadAndOpenPDFFiles  .Checked = settingBoolean[StateNew, GroupDownUploads, ValueDownloadAndOpenPDFFiles];
             listBoxChooseFileToUpload.SelectedIndex  = settingInteger[StateNew, GroupDownUploads, ValueChooseFileToUpload] - 1;
            labelSaveDownloadedFilesTo       .Text    = settingString [StateNew, GroupDownUploads, ValueSaveDownloadedFilesTo];

             textBoxBrowserExamKey                .Text    = settingString [StateNew, GroupExam, ValueBrowserExamKey];
             textBoxQuitURL                       .Text    = settingString [StateNew, GroupExam, ValueQuitURL];
            checkBoxCopyBrowserExamKeyToClipboard .Checked = settingBoolean[StateNew, GroupExam, ValueCopyBrowserExamKeyToClipboard];
            checkBoxSendBrowserExamKeyInHTTPHeader.Checked = settingBoolean[StateNew, GroupExam, ValueSendBrowserExamKeyInHTTPHeader];

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

            checkBoxInsideSebEnableSwitchUser       .Checked = settingBoolean[StateNew, GroupOutsideSeb, ValueEnableSwitchUser];
            checkBoxInsideSebEnableLockThisComputer .Checked = settingBoolean[StateNew, GroupOutsideSeb, ValueEnableLockThisComputer];
            checkBoxInsideSebEnableChangeAPassword  .Checked = settingBoolean[StateNew, GroupOutsideSeb, ValueEnableChangeAPassword];
            checkBoxInsideSebEnableStartTaskManager .Checked = settingBoolean[StateNew, GroupOutsideSeb, ValueEnableStartTaskManager];
            checkBoxInsideSebEnableLogOff           .Checked = settingBoolean[StateNew, GroupOutsideSeb, ValueEnableLogOff];
            checkBoxInsideSebEnableShutDown         .Checked = settingBoolean[StateNew, GroupOutsideSeb, ValueEnableShutDown];
            checkBoxInsideSebEnableEaseOfAccess     .Checked = settingBoolean[StateNew, GroupOutsideSeb, ValueEnableEaseOfAccess];
            checkBoxInsideSebEnableVmWareClientShade.Checked = settingBoolean[StateNew, GroupOutsideSeb, ValueEnableVmWareClientShade];

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
            checkBoxEnableF3 .Checked = settingBoolean[StateNew, GroupFunctionKeys, ValueEnableF4];
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

            checkBoxAllowPreferencesWindow.Checked = settingBoolean[StateNew, GroupSecurityOptions, ValueAllowPreferencesWindow];
            checkBoxCreateNewDesktop      .Checked = settingBoolean[StateNew, GroupSecurityOptions, ValueCreateNewDesktop];
            checkBoxIgnoreQuitPassword    .Checked = settingBoolean[StateNew, GroupSecurityOptions, ValueIgnoreQuitPassword];
        }





    } // end of   class     SebWindowsConfigForm
}     // end of   namespace SebWindowsConfig
