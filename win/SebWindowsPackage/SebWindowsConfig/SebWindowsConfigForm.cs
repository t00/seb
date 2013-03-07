using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Security.Cryptography;



namespace SebWindowsConfig
{
    public partial class SebWindowsConfigForm : Form
    {
        // Constants for indexing the ini file values

        // SEB has 1 different ini file:
        // SebStarter.ini
        const int FileMin = 1;
        const int FileSebStarter = 1;
        const int FileMax = 1;
        const int FileNum = 1;

        // The target files the user must configure,
        // because these are used by the application SebStarter.exe
        const String ConfigSebStarter    = "SebStarter config file";
        const String TargetSebStarterIni = "SebStarter.ini";

        // The values can be in 4 different states:
        // old, new, temporary and default values
        const int StateMin = 1;
        const int StateOld   = 1;
        const int StateNew   = 2;
        const int StateTmp   = 3;
        const int StateDef   = 4;
        const int StateMax = 4;
        const int StateNum = 4;

        // The Graphical User Interface contains 19 groups
        const int GroupMin =   1;
        const int GroupMax =  19;
        const int GroupNum =  19;

        // SebStarter contains the 19 groups
        // General, ConfigFile, Appearance, Browser,
        // DownUploads, Exam, Applications, Network, Security,
        // Registry, HookedKeys, ExitKeys,
        // InsideSeb, OutsideSeb, SecurityOptions,
        // OnlineExam, SpecialKeys, FunctionKeys, Other
        const int GroupMinSebStarter = 1;

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
        const int GroupHookedKeys      = 11;
        const int GroupExitKeys        = 12;

        const int GroupInsideSeb       = 13;
        const int GroupOutsideSeb      = 14;
        const int GroupSecurityOptions = 15;
        const int GroupOnlineExam      = 16;
        const int GroupSpecialKeys     = 17;
        const int GroupFunctionKeys    = 18;
        const int GroupOther           = 19;

        const int GroupMaxSebStarter = 19;
        const int GroupNumSebStarter = 19;

        // Each group contains up to 20 values
        const int ValueMin =  1;
        const int ValueMax = 20;
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
        const String MessageHookedKeys      = "HookedKeys";
        const String MessageExitKeys        = "ExitKeys";

        const String MessageInsideSeb       = "InsideSeb";
        const String MessageOutsideSeb      = "OutsideSeb";
        const String MessageSecurityOptions = "SecurityOptions";
        const String MessageOnlineExam      = "OnlineExam";
        const String MessageSpecialKeys     = "SpecialKeys";
        const String MessageFunctionKeys    = "FunctionKeys";
        const String MessageOther           = "Other";


        // Group "General"
        const int MinValueGeneral  = 1;
        const int ValueStartURL                     = 1;
        const int ValueSEBServerURL                 = 2;
        const int ValueAdministratorPassword        = 3;
        const int ValueConfirmAdministratorPassword = 4;
        const int ValueAllowUserToQuitSEB           = 5;
        const int ValueQuitPassword                 = 6;
        const int ValueConfirmQuitPassword          = 7;
        const int ValueQuitHashcode                 = 8;
        const int MaxValueGeneral  = 8;

        const String MessageStartURL                     = "StartURL";
        const String MessageSEBServerURL                 = "SEBServerURL";
        const String MessageAdministratorPassword        = "QuitPassword";
        const String MessageConfirmAdministratorPassword = "ConfirmQuitPassword";
        const String MessageAllowUserToQuitSEB           = "AllowUserToQuitSEB";
        const String MessageQuitPassword                 = "QuitPassword";
        const String MessageConfirmQuitPassword          = "ConfirmQuitPassword";
        const String MessageQuitHashcode                 = "QuitHashcode";

        // Group "Config File"
        const int MinValueConfigFile = 1;
        const int ValueWriteSebStarterLogFile = 1;
        const int MaxValueConfigFile = 1;

        const String MessageCurrentSebStarterIni   = "CurrentSebStarterIni";
        const String MessageWriteSebStarterLogFile = "WriteSebStarterLogFile";

        // Groups "Inside SEB" and "Outside SEB"
        const int MinValueInsideSeb  = 1;
        const int MinValueOutsideSeb = 1;
        const int ValueEnableSwitchUser        = 1;
        const int ValueEnableLockThisComputer  = 2;
        const int ValueEnableChangeAPassword   = 3;
        const int ValueEnableStartTaskManager  = 4;
        const int ValueEnableLogOff            = 5;
        const int ValueEnableShutDown          = 6;
        const int ValueEnableEaseOfAccess      = 7;
        const int ValueEnableVmWareClientShade = 8;
        const int MaxValueInsideSeb  = 8;
        const int MaxValueOutsideSeb = 8;

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

        // Group "Security Options"
        const int MinValueSecurityOptions = 1;

        const int ValueAllowDownUploads          = 1;
        const int ValueAllowFlashFullscreen      = 2;
        const int ValueAllowPreferencesWindow    = 3;
        const int ValueAllowQuit                 = 4;
        const int ValueAllowSwitchToApplications = 5;
        const int ValueAllowVirtualMachine       = 6;

        const int ValueBlockPopupWindows         = 7;
        const int ValueCreateNewDesktop          = 8;
        const int ValueDownloadPDFFiles          = 9;

        const int ValueEnableBrowsingBackForward = 10;
        const int ValueEnableJava                = 11;
        const int ValueEnableJavaScript          = 12;
        const int ValueEnableLog                 = 13;
        const int ValueEnablePlugins             = 14;

        const int ValueHookMessages              = 15;
        const int ValueIgnoreQuitPassword        = 16;
        const int ValueMonitorProcesses          = 17;
        const int ValueNewBrowserWindowByLinkBlockForeign   = 18;
        const int ValueNewBrowserWindowByScriptBlockForeign = 19;
        const int ValueOpenDownloads             = 20;

        const int MaxValueSecurityOptions = 20;

        const String MessageAllowDownUploads          = "AllowDownUploads";
        const String MessageAllowFlashFullscreen      = "AllowFlashFullscreen";
        const String MessageAllowPreferencesWindow    = "AllowPreferencesWindow";
        const String MessageAllowQuit                 = "AllowQuit";
        const String MessageAllowSwitchToApplications = "AllowSwitchToApplications";
        const String MessageAllowVirtualMachine       = "AllowVirtualMachine";

        const String MessageBlockPopupWindows         = "BlockPopupWindows";
        const String MessageCreateNewDesktop          = "CreateNewDesktop";
        const String MessageDownloadPDFFiles          = "DownloadPDFFiles";

        const String MessageEnableBrowsingBackForward = "EnableBrowsingBackForward";
        const String MessageEnableJava                = "EnableJava";
        const String MessageEnableJavaScript          = "EnableJavaScript";
        const String MessageEnableLog                 = "EnableLog";
        const String MessageEnablePlugins             = "EnablePlugins";

        const String MessageHookMessages              = "HookMessages";
        const String MessageIgnoreQuitPassword        = "IgnoreQuitPassword";
        const String MessageMonitorProcesses          = "MonitorProcesses";
        const String MessageNewBrowserWindowByLinkBlockForeign   = "NewBrowserWindowByLinkBlockForeign";
        const String MessageNewBrowserWindowByScriptBlockForeign = "NewBrowserWindowByScriptBlockForeign";
        const String MessageOpenDownloads             = "OpenDownloads";

        // Group "Online Exam"
        const int MinValueOnlineExam = 1;
        const int ValueSebBrowser            = 1;
        const int ValueAutostartProcess      = 2;
        const int ValuePermittedApplications = 3;
        const int MaxValueOnlineExam = 3;

        const String MessageSebBrowser            = "SebBrowser";
        const String MessageAutostartProcess      = "AutostartProcess";
        const String MessagePermittedApplications = "PermittedApplications";

        // Group "Special Keys"
        const int MinValueSpecialKeys = 1;
        const int ValueEnableEsc        = 1;
        const int ValueEnableCtrlEsc    = 2;
        const int ValueEnableAltEsc     = 3;
        const int ValueEnableAltTab     = 4;
        const int ValueEnableAltF4      = 5;
        const int ValueEnableStartMenu  = 6;
        const int ValueEnableRightMouse = 7;
        const int MaxValueSpecialKeys = 7;

        const String MessageEnableEsc        = "EnableEsc";
        const String MessageEnableCtrlEsc    = "EnableCtrlEsc";
        const String MessageEnableAltEsc     = "EnableAltEsc";
        const String MessageEnableAltTab     = "EnableAltTab";
        const String MessageEnableAltF4      = "EnableAltF4";
        const String MessageEnableStartMenu  = "EnableStartMenu";
        const String MessageEnableRightMouse = "EnableRightMouse";

        // Group "Function Keys"
        const int MinValueFunctionKeys = 1;
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
        const int MaxValueFunctionKeys = 12;

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
        const int MinValueExitKeys = 1;
        const int ValueExitKey1 = 1;
        const int ValueExitKey2 = 2;
        const int ValueExitKey3 = 3;
        const int MaxValueExitKeys = 3;

        const String MessageExitKey1 = "B1";
        const String MessageExitKey2 = "B2";
        const String MessageExitKey3 = "B3";

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

        // Virtual key code strings
        static String[] virtualKeyCodeString = new String[ValueNum + 1];

        // Number of groups per file
        // Number of values per group
        // Names  of groups and values
        // Types  of values (Boolean, Integer, String)
        static    int[ ]    minGroup  = new    int[ FileNum + 1];
        static    int[ ]    maxGroup  = new    int[ FileNum + 1];
        static    int[ ]    minValue  = new    int[GroupNum + 1];
        static    int[ ]    maxValue  = new    int[GroupNum + 1];
        static String[ ] configString = new String[ FileNum + 1];
        static String[ ]  groupString = new String[GroupNum + 1];
        static String[,]  valueString = new String[GroupNum + 1, ValueNum + 1];
        static    int[,]   dataType   = new    int[GroupNum + 1, ValueNum + 1];

        // Settings as Booleans ("true" or "false") or Strings
        static Boolean[,,] settingBoolean = new Boolean[StateNum + 1, GroupNum + 1, ValueNum + 1];
        static String [,,] settingString  = new String [StateNum + 1, GroupNum + 1, ValueNum + 1];
        static     int[,,] settingInteger = new     int[StateNum + 1, GroupNum + 1, ValueNum + 1];

        // Password encryption using the SHA-256 hash algorithm
        SHA256 sha256 = new SHA256Managed();



        // ***********
        // Constructor
        // ***********
        public SebWindowsConfigForm()
        {
            InitializeComponent();

            // Initialise the global arrays
            int state, group, value;

            // Intialise the Safe Exam Browser values
            for (state = StateMin; state <= StateMax; state++)
            for (group = GroupMin; group <= GroupMax; group++)
            for (value = ValueMin; value <= ValueMax; value++)
            {
                settingBoolean[state, group, value] = false;
                settingString [state, group, value] = "";
                settingInteger[state, group, value] = 0;
            }

            // Default values for groups "Inside SEB", "Outside SEB" etc.
            for (value = ValueMin; value <= ValueMax; value++)
            {
                settingBoolean[StateDef, GroupInsideSeb      , value] = false;
                settingBoolean[StateDef, GroupOutsideSeb     , value] = true;
                settingBoolean[StateDef, GroupSecurityOptions, value] = false;
                settingBoolean[StateDef, GroupSpecialKeys    , value] = false;
                settingBoolean[StateDef, GroupFunctionKeys   , value] = false;
                settingInteger[StateDef, GroupExitKeys       , value] = 0;
            }

            // Default settings for group "General"
            settingBoolean[StateDef, GroupGeneral, ValueAllowUserToQuitSEB          ] = true;
            settingString [StateDef, GroupGeneral, ValueStartURL                    ] = "http://www.safeexambrowser.org";
            settingString [StateDef, GroupGeneral, ValueSEBServerURL                ] = "";
            settingString [StateDef, GroupGeneral, ValueAdministratorPassword       ] = "";
            settingString [StateDef, GroupGeneral, ValueConfirmAdministratorPassword] = "";
            settingString [StateDef, GroupGeneral, ValueQuitPassword                ] = "";
            settingString [StateDef, GroupGeneral, ValueConfirmQuitPassword         ] = "";
            settingString [StateDef, GroupGeneral, ValueQuitHashcode                ] = "";

            // Default settings for group "Config File"
            settingBoolean[StateDef, GroupConfigFile, ValueWriteSebStarterLogFile] = true;

            // Default settings for group "Security options"
            settingBoolean[StateDef, GroupSecurityOptions, ValueAllowDownUploads         ] = true;
            settingBoolean[StateDef, GroupSecurityOptions, ValueAllowFlashFullscreen     ] = false;
            settingBoolean[StateDef, GroupSecurityOptions, ValueAllowPreferencesWindow   ] = false;
            settingBoolean[StateDef, GroupSecurityOptions, ValueAllowQuit                ] = true;
            settingBoolean[StateDef, GroupSecurityOptions, ValueAllowSwitchToApplications] = false;
            settingBoolean[StateDef, GroupSecurityOptions, ValueAllowVirtualMachine      ] = false;

            settingBoolean[StateDef, GroupSecurityOptions, ValueBlockPopupWindows        ] = true;
            settingBoolean[StateDef, GroupSecurityOptions, ValueCreateNewDesktop         ] = true;
            settingBoolean[StateDef, GroupSecurityOptions, ValueDownloadPDFFiles         ] = false;

            settingBoolean[StateDef, GroupSecurityOptions, ValueEnableBrowsingBackForward] = false;
            settingBoolean[StateDef, GroupSecurityOptions, ValueEnableJava               ] = false;
            settingBoolean[StateDef, GroupSecurityOptions, ValueEnableJavaScript         ] = true;
            settingBoolean[StateDef, GroupSecurityOptions, ValueEnableLog                ] = false;
            settingBoolean[StateDef, GroupSecurityOptions, ValueEnablePlugins            ] = true;

            settingBoolean[StateDef, GroupSecurityOptions, ValueHookMessages             ] = true;
            settingBoolean[StateDef, GroupSecurityOptions, ValueIgnoreQuitPassword       ] = false;
            settingBoolean[StateDef, GroupSecurityOptions, ValueMonitorProcesses         ] = false;
            settingBoolean[StateDef, GroupSecurityOptions, ValueNewBrowserWindowByLinkBlockForeign  ] = false;
            settingBoolean[StateDef, GroupSecurityOptions, ValueNewBrowserWindowByScriptBlockForeign] = false;
            settingBoolean[StateDef, GroupSecurityOptions, ValueOpenDownloads            ] = false;

            // Default settings for group "Online exam"
            String s0 = "Seb,../xulrunner/xulrunner.exe";
            String s1 = " -app \"..\\xul_seb\\seb.ini\"";
            String s2 = " -profile \"%LOCALAPPDATA%\\ETH_Zuerich\\xul_seb\\Profiles\"";
            String SebBrowserString = s0 + s1 + s2;

            settingString[StateDef, GroupOnlineExam, ValueSebBrowser           ] =  SebBrowserString;
            settingString[StateDef, GroupOnlineExam, ValueAutostartProcess     ] = "Seb";
            settingString[StateDef, GroupOnlineExam, ValuePermittedApplications] = "Calculator,calc.exe;Notepad,notepad.exe;";

            // Default settings for groups "Special keys" and "Function keys"
            settingBoolean[StateDef, GroupSpecialKeys , ValueEnableAltTab] = true;
            settingBoolean[StateDef, GroupFunctionKeys, ValueEnableF5    ] = true;

            // Default settings for group "Exit sequence"
            settingInteger[StateDef, GroupExitKeys, ValueExitKey1] =  3;
            settingInteger[StateDef, GroupExitKeys, ValueExitKey2] = 11;
            settingInteger[StateDef, GroupExitKeys, ValueExitKey3] =  6;

            settingInteger[StateNew, GroupExitKeys, ValueExitKey1] =  3;
            settingInteger[StateNew, GroupExitKeys, ValueExitKey2] = 11;
            settingInteger[StateNew, GroupExitKeys, ValueExitKey3] =  6;


            // Data types of the different values
            for (value = ValueMin; value <= ValueMax; value++)
            {
                dataType[GroupGeneral        , value] = TypeString;
                dataType[GroupConfigFile     , value] = TypeBoolean;
                dataType[GroupInsideSeb      , value] = TypeBoolean;
                dataType[GroupOutsideSeb     , value] = TypeBoolean;
                dataType[GroupSecurityOptions, value] = TypeBoolean;
                dataType[GroupOnlineExam     , value] = TypeString;
                dataType[GroupSpecialKeys    , value] = TypeBoolean;
                dataType[GroupFunctionKeys   , value] = TypeBoolean;
                dataType[GroupExitKeys       , value] = TypeString;
            }


            // Number of groups per file
            minGroup[FileSebStarter] = GroupMinSebStarter;
            maxGroup[FileSebStarter] = GroupMaxSebStarter;

            // Number of values per group
            for (group = GroupMin; group <= GroupMax; group++)
            {
                minValue[group] = 1;
            }

            maxValue[GroupGeneral     ] = MaxValueGeneral;
            maxValue[GroupConfigFile  ] = MaxValueConfigFile;
/*
            maxValue[GroupAppearance  ] = MaxValueApperance;
            maxValue[GroupBrowser     ] = MaxValueBrowser;
            maxValue[GroupDownUploads ] = MaxValueDownUploads;
            maxValue[GroupExam        ] = MaxValueExam;
            maxValue[GroupApplications] = MaxValueApplications;
            maxValue[GroupNetwork     ] = MaxValueNetwork;
            maxValue[GroupSecurity    ] = MaxValueSecurity;
            maxValue[GroupRegistry    ] = MaxValueRegistry;
            maxValue[GroupHookedKeys  ] = MaxValueHookedKeys;
            maxValue[GroupExitKeys    ] = MaxValueExitKeys;
*/
            maxValue[GroupInsideSeb      ] = MaxValueInsideSeb;
            maxValue[GroupOutsideSeb     ] = MaxValueOutsideSeb;
            maxValue[GroupSecurityOptions] = MaxValueSecurityOptions;
            maxValue[GroupOnlineExam     ] = MaxValueOnlineExam;
            maxValue[GroupSpecialKeys    ] = MaxValueSpecialKeys;
            maxValue[GroupFunctionKeys   ] = MaxValueFunctionKeys;
//          maxValue[GroupOther          ] = MaxValueOther;

            // File names
            configString[FileSebStarter] = ConfigSebStarter;

            // Group names
            groupString[GroupConfigFile     ] = MessageConfigFile;
            groupString[GroupInsideSeb      ] = MessageInsideSeb;
            groupString[GroupOutsideSeb     ] = MessageOutsideSeb;
            groupString[GroupSecurityOptions] = MessageSecurityOptions;
            groupString[GroupOnlineExam     ] = MessageOnlineExam;
            groupString[GroupSpecialKeys    ] = MessageSpecialKeys;
            groupString[GroupFunctionKeys   ] = MessageFunctionKeys;
            groupString[GroupExitKeys       ] = MessageExitKeys;

            // Value names
            valueString[GroupConfigFile, ValueWriteSebStarterLogFile] = MessageWriteSebStarterLogFile;

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

            valueString[GroupSecurityOptions, ValueAllowDownUploads         ] = MessageAllowDownUploads;
            valueString[GroupSecurityOptions, ValueAllowFlashFullscreen     ] = MessageAllowFlashFullscreen;
            valueString[GroupSecurityOptions, ValueAllowPreferencesWindow   ] = MessageAllowPreferencesWindow;
            valueString[GroupSecurityOptions, ValueAllowQuit                ] = MessageAllowQuit;
            valueString[GroupSecurityOptions, ValueAllowSwitchToApplications] = MessageAllowSwitchToApplications;
            valueString[GroupSecurityOptions, ValueAllowVirtualMachine      ] = MessageAllowVirtualMachine;

            valueString[GroupSecurityOptions, ValueBlockPopupWindows        ] = MessageBlockPopupWindows;
            valueString[GroupSecurityOptions, ValueCreateNewDesktop         ] = MessageCreateNewDesktop;
            valueString[GroupSecurityOptions, ValueDownloadPDFFiles         ] = MessageDownloadPDFFiles;

            valueString[GroupSecurityOptions, ValueEnableBrowsingBackForward] = MessageEnableBrowsingBackForward;
            valueString[GroupSecurityOptions, ValueEnableJava               ] = MessageEnableJava;
            valueString[GroupSecurityOptions, ValueEnableJavaScript         ] = MessageEnableJavaScript;
            valueString[GroupSecurityOptions, ValueEnableLog                ] = MessageEnableLog;
            valueString[GroupSecurityOptions, ValueEnablePlugins            ] = MessageEnablePlugins;

            valueString[GroupSecurityOptions, ValueHookMessages             ] = MessageHookMessages;
            valueString[GroupSecurityOptions, ValueIgnoreQuitPassword       ] = MessageIgnoreQuitPassword;
            valueString[GroupSecurityOptions, ValueMonitorProcesses         ] = MessageMonitorProcesses;
            valueString[GroupSecurityOptions, ValueNewBrowserWindowByLinkBlockForeign  ] = MessageNewBrowserWindowByLinkBlockForeign;
            valueString[GroupSecurityOptions, ValueNewBrowserWindowByScriptBlockForeign] = MessageNewBrowserWindowByScriptBlockForeign;
            valueString[GroupSecurityOptions, ValueOpenDownloads            ] = MessageOpenDownloads;

            valueString[GroupOnlineExam, ValueSebBrowser           ] = MessageSebBrowser;
            valueString[GroupOnlineExam, ValueAutostartProcess     ] = MessageAutostartProcess;
            valueString[GroupOnlineExam, ValuePermittedApplications] = MessagePermittedApplications;

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

            virtualKeyCodeString[ 1] = "112";
            virtualKeyCodeString[ 2] = "113";
            virtualKeyCodeString[ 3] = "114";
            virtualKeyCodeString[ 4] = "115";
            virtualKeyCodeString[ 5] = "116";
            virtualKeyCodeString[ 6] = "117";
            virtualKeyCodeString[ 7] = "118";
            virtualKeyCodeString[ 8] = "119";
            virtualKeyCodeString[ 9] = "120";
            virtualKeyCodeString[10] = "121";
            virtualKeyCodeString[11] = "122";
            virtualKeyCodeString[12] = "123";

            // Try to open the ini file (SebStarter.ini)
            // given in the local directory (where SebWindowsConfig.exe was called)
            currentDireSebStarterIni = Directory.GetCurrentDirectory();
            currentFileSebStarterIni = "";
            currentPathSebStarterIni = "";

             targetDireSebStarterIni = Directory.GetCurrentDirectory();
             targetFileSebStarterIni = TargetSebStarterIni;
             targetPathSebStarterIni = Path.GetFullPath(TargetSebStarterIni);

            // Read the settings from the ini file and update their widgets
            if (OpenIniFile(FileSebStarter, targetPathSebStarterIni) == true)
            {
                currentDireSebStarterIni = targetDireSebStarterIni;
                currentFileSebStarterIni = targetFileSebStarterIni;
                currentPathSebStarterIni = targetPathSebStarterIni;

                SetWidgetsToNewSettingsOfSebStarterIni();
            }

            openFileDialogSebStarterIni.InitialDirectory = Environment.CurrentDirectory;
            saveFileDialogSebStarterIni.InitialDirectory = Environment.CurrentDirectory;

        } // end of contructor   SebWindowsConfigForm()



        // *********************************
        // Open SebStarter config file click
        // *********************************
        private void labelOpenSebStarterConfigFile_Click(object sender, EventArgs e)
        {
            // Set the default directory and file name in the File Dialog
            openFileDialogSebStarterIni.InitialDirectory = currentDireSebStarterIni;
            openFileDialogSebStarterIni.FileName         = currentFileSebStarterIni;

            // Get the user inputs in the File Dialog
            DialogResult fileDialogResult = openFileDialogSebStarterIni.ShowDialog();
            String       fileName         = openFileDialogSebStarterIni.FileName;

            // If the user clicked "Cancel", do nothing
            if (fileDialogResult.Equals(DialogResult.Cancel)) return;

            // If the user clicked "OK",
            // read the settings from the ini file and update their widgets
            if (OpenIniFile(FileSebStarter, fileName) == true)
            {
                currentDireSebStarterIni = Path.GetDirectoryName(fileName);
                currentFileSebStarterIni = Path.GetFileName     (fileName);
                currentPathSebStarterIni = Path.GetFullPath     (fileName);

                SetWidgetsToNewSettingsOfSebStarterIni();
            }
        } // end of method   labelOpenSebStarterConfigFile_Click()



        // *********************************
        // Save SebStarter config file click
        // *********************************
        private void labelSaveSebStarterConfigFile_Click(object sender, EventArgs e)
        {
            // Set the default directory and file name in the File Dialog
            saveFileDialogSebStarterIni.InitialDirectory = currentDireSebStarterIni;
            saveFileDialogSebStarterIni.FileName         = currentFileSebStarterIni;

            // Get the user inputs in the File Dialog
            DialogResult fileDialogResult = saveFileDialogSebStarterIni.ShowDialog();
            String       fileName         = saveFileDialogSebStarterIni.FileName;

            // If the user clicked "Cancel", do nothing
            if (fileDialogResult.Equals(DialogResult.Cancel)) return;

            // If the user clicked "OK",
            // write the settings to the ini file and update the filename widget
            if (SaveIniFile(FileSebStarter, fileName) == true)
            {
                currentDireSebStarterIni = Path.GetDirectoryName(fileName);
                currentFileSebStarterIni = Path.GetFileName     (fileName);
                currentPathSebStarterIni = Path.GetFullPath     (fileName);

                textBoxCurrentDireSebStarterIni.Text = currentDireSebStarterIni;
                textBoxCurrentFileSebStarterIni.Text = currentFileSebStarterIni;
            }
        } // end of method   labelSaveSebStarterConfigFile_Click()



        // ***********************************
        // Open ini file and read the settings
        // ***********************************
        private Boolean OpenIniFile(int iniFile, String fileName)
        {
            FileStream   fileStream;
            StreamReader fileReader;
            String       fileLine;
            Boolean      fileCouldBeRead = true;

            int group, value;
            int mingroup = minGroup[iniFile];
            int maxgroup = maxGroup[iniFile];
            int minvalue;
            int maxvalue;

            try 
            {
                // Open the ini file for reading
                fileStream = new   FileStream(fileName, FileMode.Open, FileAccess.Read);
                fileReader = new StreamReader(fileStream);

                // Read lines from the ini file until end of file is reached
                while ((fileLine = fileReader.ReadLine()) != null) 
                {
                    // Skip empty lines and lines not in "leftSide = rightSide" format
                    if (fileLine.Contains("="))
                    {
                        int      equalPosition =    fileLine.IndexOf  ("=");
                        String   leftString    =    fileLine.Remove   (equalPosition);
                        String  rightString    =    fileLine.Substring(equalPosition + 1);
                        Boolean rightBoolean   = rightString.Equals("1");
                        Boolean foundSetting   = false;

                        // Find the appropriate group and setting
                        for (group = mingroup; group <= maxgroup; group++)
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

                    } // end if line.Contains("=")
                } // end while

                // Close the ini file
                fileReader.Close();
                fileStream.Close();

            } // end try

            catch (Exception streamReadException) 
            {
                // Let the user know what went wrong
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(streamReadException.Message);
                return false;
            }

            if (fileCouldBeRead == false)
            {
                // Let the user know what went wrong
                MessageBox.Show("The file \"" + fileName + "\" does not match the syntax of a " + configString[iniFile],
                                "Error when reading " + configString[iniFile],
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }


            #region After reading, convert ExitKeySequence of SebStarter ini file
            // The Exit Key Sequence of the SebStarter ini file needs a special conversion
            if (iniFile == FileSebStarter)
            {
                // Convert the B1, B2, B3 strings to integers
                String tmpB1 = settingString[StateTmp, GroupExitKeys, ValueExitKey1];
                String tmpB2 = settingString[StateTmp, GroupExitKeys, ValueExitKey2];
                String tmpB3 = settingString[StateTmp, GroupExitKeys, ValueExitKey3];

                int tmpIndexExitKey1 = 0;
                int tmpIndexExitKey2 = 0;
                int tmpIndexExitKey3 = 0;

                for (int indexFunctionKey = 1; indexFunctionKey <= 12; indexFunctionKey++)
                {
                    String vkc = virtualKeyCodeString[indexFunctionKey];

                    if (tmpB1.Equals(vkc)) tmpIndexExitKey1 = indexFunctionKey;
                    if (tmpB2.Equals(vkc)) tmpIndexExitKey2 = indexFunctionKey;
                    if (tmpB3.Equals(vkc)) tmpIndexExitKey3 = indexFunctionKey;
                }

                settingInteger[StateTmp, GroupExitKeys, ValueExitKey1] = tmpIndexExitKey1;
                settingInteger[StateTmp, GroupExitKeys, ValueExitKey2] = tmpIndexExitKey2;
                settingInteger[StateTmp, GroupExitKeys, ValueExitKey3] = tmpIndexExitKey3;
            } 
            #endregion


            // Accept the tmp values as the new values
            for (group = mingroup; group <= maxgroup; group++)
            {
                minvalue = minValue[group];
                maxvalue = maxValue[group];

                for (value = minvalue; value <= maxvalue; value++)
                {
                    settingBoolean[StateOld, group, value] = settingBoolean[StateTmp, group, value];
                    settingString [StateOld, group, value] = settingString [StateTmp, group, value];
                    settingInteger[StateOld, group, value] = settingInteger[StateTmp, group, value];

                    settingBoolean[StateNew, group, value] = settingBoolean[StateTmp, group, value];
                    settingString [StateNew, group, value] = settingString [StateTmp, group, value];
                    settingInteger[StateNew, group, value] = settingInteger[StateTmp, group, value];
                }
            }

            return true;

        } // end of method   OpenIniFile()




        // **************************************
        // Write settings to ini file and save it
        // **************************************
        private Boolean SaveIniFile(int iniFile, String fileName)
        {
            FileStream   fileStream;
            StreamWriter fileWriter;
            String       fileLine;

            int group, value;
            int mingroup = minGroup[iniFile];
            int maxgroup = maxGroup[iniFile];
            int minvalue;
            int maxvalue;


            #region Before writing, convert ExitKeySequence of SebStarter ini file
            // The Exit Key Sequence of the SebStarter ini file needs a special conversion
            if (iniFile == FileSebStarter)
            {
                int newIndexExitKey1 = settingInteger[StateNew, GroupExitKeys, ValueExitKey1];
                int newIndexExitKey2 = settingInteger[StateNew, GroupExitKeys, ValueExitKey2];
                int newIndexExitKey3 = settingInteger[StateNew, GroupExitKeys, ValueExitKey3];

                settingString[StateNew, GroupExitKeys, ValueExitKey1] = virtualKeyCodeString[newIndexExitKey1];
                settingString[StateNew, GroupExitKeys, ValueExitKey2] = virtualKeyCodeString[newIndexExitKey2];
                settingString[StateNew, GroupExitKeys, ValueExitKey3] = virtualKeyCodeString[newIndexExitKey3];
            } 
            #endregion


            try 
            {
                // If the ini file already exists, delete it
                // and write it again from scratch with new data
                if (File.Exists(fileName))
                    File.Delete(fileName);

                // Open the ini file for writing
                fileStream = new   FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write);
                fileWriter = new StreamWriter(fileStream);

                // Write the header lines
                fileWriter.WriteLine("");
                fileWriter.WriteLine("[SEB]");
                fileWriter.WriteLine("");

                // For each group and each key,
                // write the line "key=value" into the ini file
                for (group = mingroup; group <= maxgroup; group++)
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

                // Close the output file
                fileWriter.Close();
                fileStream.Close();

            } // end try

            catch (Exception streamWriteException) 
            {
                // Let the user know what went wrong
                Console.WriteLine("The file could not be written:");
                Console.WriteLine(streamWriteException.Message);
                return false;
            }

            // Accept the tmp values as the new values
            for (group = mingroup; group <= maxgroup; group++)
            {
                minvalue = minValue[group];
                maxvalue = maxValue[group];

                for (value = minvalue; value <= maxvalue; value++)
                {
                    settingBoolean[StateOld, group, value] = settingBoolean[StateNew, group, value];
                    settingString [StateOld, group, value] = settingString [StateNew, group, value];
                    settingInteger[StateOld, group, value] = settingInteger[StateNew, group, value];
                }
            }

            return true;

        } // end of method   SaveIniFile()




        // ******************************************************
        // Event handlers:
        // If the user changes a setting by clicking or typing,
        // update the setting in memory for later saving on file.
        // ******************************************************

        // Group "SebStarter config file"
        private void checkBoxWriteSebStarterLogFile_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupConfigFile, ValueWriteSebStarterLogFile] = checkBoxWriteSebStarterLogFile.Checked;
        }


        // Group "Inside SEB"
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
 

        // Group "Outside SEB"
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


        // Group "Security options"
        private void checkBoxAllowDownUploads_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupSecurityOptions, ValueAllowDownUploads] = checkBoxAllowDownUploads.Checked;
        }

        private void checkBoxAllowFlashFullscreen_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupSecurityOptions, ValueAllowFlashFullscreen] = checkBoxAllowFlashFullscreen.Checked;
        }

        private void checkBoxAllowPreferencesWindow_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupSecurityOptions, ValueAllowPreferencesWindow] = checkBoxAllowPreferencesWindow.Checked;
        }

        private void checkBoxAllowQuit_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupSecurityOptions, ValueAllowQuit] = checkBoxAllowQuit.Checked;
        }

        private void checkBoxAllowSwitchToApplications_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupSecurityOptions, ValueAllowSwitchToApplications] = checkBoxAllowSwitchToApplications.Checked;
        }

        private void checkBoxAllowVirtualMachine_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupSecurityOptions, ValueAllowVirtualMachine] = checkBoxAllowVirtualMachine.Checked;
        }

        private void checkBoxBlockPopupWindows_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupSecurityOptions, ValueBlockPopupWindows] = checkBoxBlockPopupWindows.Checked;
        }

        private void checkBoxCreateNewDesktop_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupSecurityOptions, ValueCreateNewDesktop] = checkBoxCreateNewDesktop.Checked;
        }

        private void checkBoxDownloadPDFFiles_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupSecurityOptions, ValueDownloadPDFFiles] = checkBoxDownloadPDFFiles.Checked;
        }

        private void checkBoxEnableBrowsingBackForward_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupSecurityOptions, ValueEnableBrowsingBackForward] = checkBoxEnableBrowsingBackForward.Checked;
        }

        private void checkBoxEnableJava_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupSecurityOptions, ValueEnableJava] = checkBoxEnableJava.Checked;
        }

        private void checkBoxEnableJavaScript_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupSecurityOptions, ValueEnableJavaScript] = checkBoxEnableJavaScript.Checked;
        }

        private void checkBoxEnableLog_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupSecurityOptions, ValueEnableLog] = checkBoxEnableLog.Checked;
        }

        private void checkBoxEnablePlugins_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupSecurityOptions, ValueEnablePlugins] = checkBoxEnablePlugins.Checked;
        }

        private void checkBoxHookMessages_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupSecurityOptions, ValueHookMessages] = checkBoxHookMessages.Checked;
        }

        private void checkBoxIgnoreQuitPassword_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupSecurityOptions, ValueIgnoreQuitPassword] = checkBoxIgnoreQuitPassword.Checked;
        }

        private void checkBoxMonitorProcesses_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupSecurityOptions, ValueMonitorProcesses] = checkBoxMonitorProcesses.Checked;
        }

        private void checkBoxNewBrowserWindowByLinkBlockForeign_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupSecurityOptions, ValueNewBrowserWindowByLinkBlockForeign] = checkBoxNewBrowserWindowByLinkBlockForeign.Checked;
        }

        private void checkBoxNewBrowserWindowByScriptBlockForeign_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupSecurityOptions, ValueNewBrowserWindowByScriptBlockForeign] = checkBoxNewBrowserWindowByScriptBlockForeign.Checked;
        }

        private void checkBoxOpenDownloads_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupSecurityOptions, ValueOpenDownloads] = checkBoxOpenDownloads.Checked;
        }


        // Group "Online exam"
        private void textBoxSebBrowser_TextChanged(object sender, EventArgs e)
        {
            settingString[StateNew, GroupOnlineExam, ValueSebBrowser] = textBoxSebBrowser.Text;
        }

        private void textBoxAutostartProcess_TextChanged(object sender, EventArgs e)
        {
            settingString[StateNew, GroupOnlineExam, ValueAutostartProcess] = textBoxAutostartProcess.Text;
        }

        private void textBoxPermittedApplications_TextChanged(object sender, EventArgs e)
        {
            settingString[StateNew, GroupOnlineExam, ValuePermittedApplications] = textBoxPermittedApplications.Text;
        }


        // Group "Special keys"
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


        // Group "Function keys"
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


        // Group "Exit sequence"
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


        // *********************************
        // Default SebStarter settings click
        // *********************************
        private void buttonDefaultSebStarterSettings_Click(object sender, EventArgs e)
        {
            SetNewSettingsOfFileToState(FileSebStarter, StateDef);
            SetWidgetsToNewSettingsOfSebStarterIni();
        }

        // ************************************
        // Restore SebStarter config file click
        // ************************************
        private void buttonRestoreSebStarterConfigFile_Click(object sender, EventArgs e)
        {
            SetNewSettingsOfFileToState(FileSebStarter, StateOld);
            SetWidgetsToNewSettingsOfSebStarterIni();
        }



        // ***************************************************
        // Set the new settings of a file to the desired state
        // ***************************************************
        private void SetNewSettingsOfFileToState(int iniFile, int stateDesired)
        {
            int group, value;
            int groupMin = minGroup[iniFile];
            int groupMax = maxGroup[iniFile];
            int valueMin = ValueMin;
            int valueMax = ValueMax;

            // Restore the desired values by copying them to the new values
            for (group = groupMin; group <= groupMax; group++)
            for (value = valueMin; value <= valueMax; value++)
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
            // Set the widgets to the new settings
            textBoxCurrentDireSebStarterIni.Text = currentDireSebStarterIni;
            textBoxCurrentFileSebStarterIni.Text = currentFileSebStarterIni;

            checkBoxWriteSebStarterLogFile.Checked = settingBoolean[StateNew, GroupConfigFile, ValueWriteSebStarterLogFile];

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

            checkBoxAllowDownUploads         .Checked = settingBoolean[StateNew, GroupSecurityOptions, ValueAllowDownUploads];
            checkBoxAllowFlashFullscreen     .Checked = settingBoolean[StateNew, GroupSecurityOptions, ValueAllowFlashFullscreen];
            checkBoxAllowPreferencesWindow   .Checked = settingBoolean[StateNew, GroupSecurityOptions, ValueAllowPreferencesWindow];
            checkBoxAllowQuit                .Checked = settingBoolean[StateNew, GroupSecurityOptions, ValueAllowQuit];
            checkBoxAllowSwitchToApplications.Checked = settingBoolean[StateNew, GroupSecurityOptions, ValueAllowSwitchToApplications];
            checkBoxAllowVirtualMachine      .Checked = settingBoolean[StateNew, GroupSecurityOptions, ValueAllowVirtualMachine];

            checkBoxBlockPopupWindows.Checked = settingBoolean[StateNew, GroupSecurityOptions, ValueBlockPopupWindows];
            checkBoxCreateNewDesktop .Checked = settingBoolean[StateNew, GroupSecurityOptions, ValueCreateNewDesktop];
            checkBoxDownloadPDFFiles .Checked = settingBoolean[StateNew, GroupSecurityOptions, ValueDownloadPDFFiles];

            checkBoxEnableBrowsingBackForward.Checked = settingBoolean[StateNew, GroupSecurityOptions, ValueEnableBrowsingBackForward];
            checkBoxEnableJava               .Checked = settingBoolean[StateNew, GroupSecurityOptions, ValueEnableJava];
            checkBoxEnableJavaScript         .Checked = settingBoolean[StateNew, GroupSecurityOptions, ValueEnableJavaScript];
            checkBoxEnableLog                .Checked = settingBoolean[StateNew, GroupSecurityOptions, ValueEnableLog];
            checkBoxEnablePlugins            .Checked = settingBoolean[StateNew, GroupSecurityOptions, ValueEnablePlugins];

            checkBoxHookMessages                        .Checked = settingBoolean[StateNew, GroupSecurityOptions, ValueHookMessages];
            checkBoxIgnoreQuitPassword                  .Checked = settingBoolean[StateNew, GroupSecurityOptions, ValueIgnoreQuitPassword];
            checkBoxMonitorProcesses                    .Checked = settingBoolean[StateNew, GroupSecurityOptions, ValueMonitorProcesses];
            checkBoxNewBrowserWindowByLinkBlockForeign  .Checked = settingBoolean[StateNew, GroupSecurityOptions, ValueNewBrowserWindowByLinkBlockForeign];
            checkBoxNewBrowserWindowByScriptBlockForeign.Checked = settingBoolean[StateNew, GroupSecurityOptions, ValueNewBrowserWindowByScriptBlockForeign];
            checkBoxOpenDownloads                       .Checked = settingBoolean[StateNew, GroupSecurityOptions, ValueOpenDownloads];

            textBoxSebBrowser           .Text = settingString[StateNew, GroupOnlineExam, ValueSebBrowser];
            textBoxAutostartProcess     .Text = settingString[StateNew, GroupOnlineExam, ValueAutostartProcess];
            textBoxStartURL             .Text = settingString[StateNew, GroupOnlineExam, ValueStartURL];
            textBoxPermittedApplications.Text = settingString[StateNew, GroupOnlineExam, ValuePermittedApplications];
            textBoxQuitPassword         .Text = settingString[StateNew, GroupOnlineExam, ValueQuitPassword];
            textBoxQuitHashcode         .Text = settingString[StateNew, GroupOnlineExam, ValueQuitHashcode];

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
        }



        // *************************************
        // Save both config files and exit click
        // *************************************
        private void buttonSaveConfigFileAndExit_Click(object sender, EventArgs e)
        {
            // If no file has been opened, save the current settings
            // to the target ini file (SebStarter.ini)
            if (currentFileSebStarterIni.Equals(""))
            {
                currentFileSebStarterIni = targetFileSebStarterIni;
                currentPathSebStarterIni = targetPathSebStarterIni;
            }

            // Save the ini file so that nothing gets lost
            SaveIniFile(FileSebStarter, currentPathSebStarterIni);

            // Close the configuration window and exit
            this.Close();
        }


        // *************************
        // Exit without saving click
        // *************************
        private void buttonExitWithoutSaving_Click(object sender, EventArgs e)
        {
            // Close the configuration window and exit without saving
            this.Close();
        }


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


        // *************
        // "General" tab
        // *************
        private void textBoxStartURL_TextChanged(object sender, EventArgs e)
        {
            settingString[StateNew, GroupOnlineExam, ValueStartURL] = textBoxStartURL.Text;
        }

        private void textBoxSEBServerURL_TextChanged(object sender, EventArgs e)
        {
            //settingString[StateNew, GroupOnlineExam, ValueSEBServerURL] = textBoxSEBServerURL.Text;
        }

        private void textBoxAdministratorPassword_TextChanged(object sender, EventArgs e)
        {

        }


        private void textBoxConfirmAdministratorPassword_TextChanged(object sender, EventArgs e)
        {

        }


        private void checkBoxAllowUserToQuitSEB_CheckedChanged(object sender, EventArgs e)
        {

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

            //settingString[StateNew, GroupOnlineExam, ValueQuitPassword] = newStringQuitPassword;
            //settingString[StateNew, GroupOnlineExam, ValueQuitHashcode] = newStringQuitHashcode;
        }


        private void textBoxConfirmQuitPassword_TextChanged(object sender, EventArgs e)
        {

        }









    } // end of   class     SebWindowsConfigForm
}     // end of   namespace SebWindowsConfig
