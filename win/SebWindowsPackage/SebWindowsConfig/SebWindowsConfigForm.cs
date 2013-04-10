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

        // The Graphical User Interface contains 19 groups
        const int GroupNum = 19;

        // SebStarter contains the 19 groups
        // General, ConfigFile, Appearance, Browser,
        // DownUploads, Exam, Applications, Network, Security,
        // Registry, HookedKeys, ExitKeys,
        // InsideSeb, OutsideSeb, SecurityOptions,
        // OnlineExam, SpecialKeys, FunctionKeys, Other

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

        const int GroupNumSebStarter = 19;

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
        const int ValueMainBrowserWindowSizeWidth  = 3;
        const int ValueMainBrowserWindowSizeHeight = 4;
        const int ValueMainBrowserWindowHorizPos   = 5;
        const int ValueEnableBrowserWindowToolbar  = 6;
        const int ValueHideToolbarAsDefault        = 7;
        const int ValueShowMenuBar                 = 8;
        const int ValueDisplaySEBDockTaskBar       = 9;
        const int NumValueAppearance = 9;

        const String MessageUseBrowserWindow            = "UseBrowserWindow";
        const String MessageUseFullScreenMode           = "UseFullScreenMode";
        const String MessageMainBrowserWindowSizeWidth  = "MainBrowserWindowSizeWidth";
        const String MessageMainBrowserWindowSizeHeight = "MainBrowserWindowSizeHeight";
        const String MessageMainBrowserWindowHorizPos   = "MainBrowserWindowHorizPos";
        const String MessageEnableBrowserWindowToolbar  = "EnableBrowserWindowToolbar";
        const String MessageHideToolbarAsDefault        = "HideToolbarAsDefault";
        const String MessageShowMenuBar                 = "ShowMenuBar";
        const String MessageDisplaySEBDockTaskBar       = "DisplaySEBDockTaskBar";

        // Group "Browser"
        // Group "DownUploads"
        // Group "Exam"
        // Group "Applications"
        // Group "Network"

        // Group "Security"
        const int ValueEnableLogging = 1;
        const int NumValueSecurity = 1;

        const String MessageEnableLogging = "EnableLogging";

        // Group "Exit Keys"
        const int ValueExitKey1 = 1;
        const int ValueExitKey2 = 2;
        const int ValueExitKey3 = 3;
        const int NumValueExitKeys = 3;

        const String MessageExitKey1 = "ExitKey1";
        const String MessageExitKey2 = "ExitKey2";
        const String MessageExitKey3 = "ExitKey3";

        const int NumValueBrowser      = 0;
        const int NumValueDownUploads  = 0;
        const int NumValueExam         = 0;
        const int NumValueApplications = 0;
        const int NumValueNetwork      = 0;
//      const int NumValueSecurity     = 0;
        const int NumValueRegistry     = 0;
        const int NumValueHookedKeys   = 0;
//      const int NumValueExitKeys     = 0;

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

        // Group "Security Options"
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
        const int ValueNewBrowserWindowByLink    = 18;
        const int ValueNewBrowserWindowByScript  = 19;
        const int ValueOpenDownloads             = 20;

        const int NumValueSecurityOptions = 20;

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
        const String MessageNewBrowserWindowByLink    = "NewBrowserWindowByLink";
        const String MessageNewBrowserWindowByScript  = "NewBrowserWindowByScript";
        const String MessageOpenDownloads             = "OpenDownloads";

        // Group "Online Exam"
        const int ValueSebBrowser            = 1;
        const int ValueAutostartProcess      = 2;
        const int ValuePermittedApplications = 3;
        const int NumValueOnlineExam = 3;

        const String MessageSebBrowser            = "SebBrowser";
        const String MessageAutostartProcess      = "AutostartProcess";
        const String MessagePermittedApplications = "PermittedApplications";

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

        // Group "Other"
        const int NumValueOther = 0;


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
        static String[] horizontalPositioningString = new String[4];
        static String[]           functionKeyString = new String[13];

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
            settingBoolean[StateDef, GroupAppearance, ValueUseBrowserWindow           ] = true;
            settingBoolean[StateDef, GroupAppearance, ValueUseFullScreenMode          ] = false;
            settingString [StateDef, GroupAppearance, ValueMainBrowserWindowSizeWidth ] = "100%";
            settingString [StateDef, GroupAppearance, ValueMainBrowserWindowSizeHeight] = "100%";
            settingString [StateDef, GroupAppearance, ValueMainBrowserWindowHorizPos  ] = "Center";
            settingBoolean[StateDef, GroupAppearance, ValueEnableBrowserWindowToolbar ] = false;
            settingBoolean[StateDef, GroupAppearance, ValueHideToolbarAsDefault       ] = false;
            settingBoolean[StateDef, GroupAppearance, ValueShowMenuBar                ] = false;
            settingBoolean[StateDef, GroupAppearance, ValueDisplaySEBDockTaskBar      ] = false;

            settingInteger[StateDef, GroupAppearance, ValueMainBrowserWindowSizeWidth ] = 0;
            settingInteger[StateDef, GroupAppearance, ValueMainBrowserWindowSizeHeight] = 0;
            settingInteger[StateDef, GroupAppearance, ValueMainBrowserWindowHorizPos  ] = 2;

            // Default settings for group "Browser"
            // Default settings for group "DownUploads"
            // Default settings for group "Exam"
            // Default settings for group "Applications"
            // Default settings for group "Network"

            // Default settings for group "Security"
            settingBoolean[StateDef, GroupSecurity, ValueEnableLogging] = true;

            // Default settings for group "Registry"
            // Default settings for group "HookedKeys"

            // Default settings for group "ExitKeys"
            settingInteger[StateDef, GroupExitKeys, ValueExitKey1] =  3;
            settingInteger[StateDef, GroupExitKeys, ValueExitKey2] = 11;
            settingInteger[StateDef, GroupExitKeys, ValueExitKey3] =  6;

            // Default values for groups "Inside SEB", "Outside SEB" etc.
            for (value = 1; value <= ValueNum; value++)
            {
                settingBoolean[StateDef, GroupInsideSeb      , value] = false;
                settingBoolean[StateDef, GroupOutsideSeb     , value] = true;
                settingBoolean[StateDef, GroupSecurityOptions, value] = false;
                settingBoolean[StateDef, GroupSpecialKeys    , value] = false;
                settingBoolean[StateDef, GroupFunctionKeys   , value] = false;
            }

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
            settingBoolean[StateDef, GroupSecurityOptions, ValueNewBrowserWindowByLink   ] = false;
            settingBoolean[StateDef, GroupSecurityOptions, ValueNewBrowserWindowByScript ] = false;
            settingBoolean[StateDef, GroupSecurityOptions, ValueOpenDownloads            ] = false;

            // Default settings for group "Online exam"
            String s0 = "Seb,../xulrunner/xulrunner.exe";
            String s1 = " -app \"..\\xul_seb\\seb.ini\"";
            String s2 = " -profile \"%LOCALAPPDATA%\\ETH_Zuerich\\xul_seb\\Profiles\"";
            String SebBrowserString = s0 + s1 + s2;

            settingString[StateDef, GroupOnlineExam, ValueSebBrowser           ] =  SebBrowserString;
            settingString[StateDef, GroupOnlineExam, ValueAutostartProcess     ] = "Seb";
            settingString[StateDef, GroupOnlineExam, ValuePermittedApplications] = "Calculator,calc.exe;Notepad,notepad.exe;";

            // Default settings for groups "Special keys"
            settingBoolean[StateDef, GroupSpecialKeys , ValueEnableAltTab] = true;

            // Default settings for groups "Function keys"
            settingBoolean[StateDef, GroupFunctionKeys, ValueEnableF5] = true;


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
                dataType[GroupHookedKeys     , value] = TypeBoolean;
                dataType[GroupExitKeys       , value] = TypeString;

                dataType[GroupInsideSeb      , value] = TypeBoolean;
                dataType[GroupOutsideSeb     , value] = TypeBoolean;
                dataType[GroupSecurityOptions, value] = TypeBoolean;
                dataType[GroupOnlineExam     , value] = TypeString;
                dataType[GroupSpecialKeys    , value] = TypeBoolean;
                dataType[GroupFunctionKeys   , value] = TypeBoolean;
            }

            // Exceptional data types of some special values
            dataType[GroupGeneral   , ValueAllowUserToQuitSEB     ] = TypeBoolean;

            dataType[GroupConfigFile, ValueChooseIdentity         ] = TypeString;
            dataType[GroupConfigFile, ValueSettingsPassword       ] = TypeString;
            dataType[GroupConfigFile, ValueConfirmSettingsPassword] = TypeString;

            dataType[GroupAppearance, ValueMainBrowserWindowSizeWidth ] = TypeString;
            dataType[GroupAppearance, ValueMainBrowserWindowSizeHeight] = TypeString;
            dataType[GroupAppearance, ValueMainBrowserWindowHorizPos  ] = TypeString;

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
            maxValue[GroupHookedKeys  ] = NumValueHookedKeys;
            maxValue[GroupExitKeys    ] = NumValueExitKeys;

            maxValue[GroupInsideSeb      ] = NumValueInsideSeb;
            maxValue[GroupOutsideSeb     ] = NumValueOutsideSeb;
            maxValue[GroupSecurityOptions] = NumValueSecurityOptions;
            maxValue[GroupOnlineExam     ] = NumValueOnlineExam;
            maxValue[GroupSpecialKeys    ] = NumValueSpecialKeys;
            maxValue[GroupFunctionKeys   ] = NumValueFunctionKeys;
            maxValue[GroupOther          ] = NumValueOther;

            // File names
            configString[FileSebStarterIni] = TargetSebStarterIni;
            configString[FileSebStarterXml] = TargetSebStarterXml;
            configString[FileSebStarterSeb] = TargetSebStarterSeb;

            // Group names
            groupString[GroupGeneral        ] = MessageGeneral;
            groupString[GroupConfigFile     ] = MessageConfigFile;
            groupString[GroupAppearance     ] = MessageAppearance;
            groupString[GroupBrowser        ] = MessageBrowser;
            groupString[GroupDownUploads    ] = MessageDownUploads;
            groupString[GroupExam           ] = MessageExam;
            groupString[GroupApplications   ] = MessageApplications;
            groupString[GroupNetwork        ] = MessageNetwork;
            groupString[GroupSecurity       ] = MessageSecurity;
            groupString[GroupRegistry       ] = MessageRegistry;
            groupString[GroupHookedKeys     ] = MessageHookedKeys;
            groupString[GroupExitKeys       ] = MessageExitKeys;

            groupString[GroupInsideSeb      ] = MessageInsideSeb;
            groupString[GroupOutsideSeb     ] = MessageOutsideSeb;
            groupString[GroupSecurityOptions] = MessageSecurityOptions;
            groupString[GroupOnlineExam     ] = MessageOnlineExam;
            groupString[GroupSpecialKeys    ] = MessageSpecialKeys;
            groupString[GroupFunctionKeys   ] = MessageFunctionKeys;


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
            valueString[GroupAppearance, ValueMainBrowserWindowSizeWidth ] = MessageMainBrowserWindowSizeWidth;
            valueString[GroupAppearance, ValueMainBrowserWindowSizeHeight] = MessageMainBrowserWindowSizeHeight;
            valueString[GroupAppearance, ValueMainBrowserWindowHorizPos  ] = MessageMainBrowserWindowHorizPos;
            valueString[GroupAppearance, ValueEnableBrowserWindowToolbar ] = MessageEnableBrowserWindowToolbar;
            valueString[GroupAppearance, ValueHideToolbarAsDefault       ] = MessageHideToolbarAsDefault;
            valueString[GroupAppearance, ValueShowMenuBar                ] = MessageShowMenuBar;
            valueString[GroupAppearance, ValueDisplaySEBDockTaskBar      ] = MessageDisplaySEBDockTaskBar;


            valueString[GroupSecurity, ValueEnableLogging] = MessageEnableLogging;


            valueString[GroupExitKeys, ValueExitKey1] = MessageExitKey1;
            valueString[GroupExitKeys, ValueExitKey2] = MessageExitKey2;
            valueString[GroupExitKeys, ValueExitKey3] = MessageExitKey3;

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
            valueString[GroupSecurityOptions, ValueNewBrowserWindowByLink   ] = MessageNewBrowserWindowByLink;
            valueString[GroupSecurityOptions, ValueNewBrowserWindowByScript ] = MessageNewBrowserWindowByScript;
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


            // Define the strings for the encryption identity
            chooseIdentityStringList.Add("none");
            chooseIdentityStringList.Add("alpha");
            chooseIdentityStringList.Add("beta");
            chooseIdentityStringList.Add("gamma");
            chooseIdentityStringList.Add("delta");
            String[] chooseIdentityStringArray = chooseIdentityStringList.ToArray();

            // Define the strings for the horizontal positioning
            horizontalPositioningString[0] = "";
            horizontalPositioningString[1] = "Left";
            horizontalPositioningString[2] = "Center";
            horizontalPositioningString[3] = "Right";

            // Define the strings for the function keys F1, F2, ..., F12
            for (int i = 1; i <= 12; i++)
                functionKeyString[i] = "F" + i.ToString();


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

            // Horizontal Positioning needs a conversion from string to integer
            String tmpStringHorizPos = settingString[StateTmp, GroupAppearance, ValueMainBrowserWindowHorizPos];
            int    tmpIndexHorizPos  = 0;
            for (int  indexHorizPos = 1; indexHorizPos <= 3; indexHorizPos++)
            {
                String hps = horizontalPositioningString[indexHorizPos];
                if (tmpStringHorizPos.Equals(hps)) tmpIndexHorizPos = indexHorizPos;
            }
            settingInteger[StateTmp, GroupAppearance, ValueMainBrowserWindowHorizPos] = tmpIndexHorizPos;

            // Exit Key Sequence needs a conversion from string to integer
            String tmpStringExitKey1 = settingString[StateTmp, GroupExitKeys, ValueExitKey1];
            String tmpStringExitKey2 = settingString[StateTmp, GroupExitKeys, ValueExitKey2];
            String tmpStringExitKey3 = settingString[StateTmp, GroupExitKeys, ValueExitKey3];

            // Remove the first character "F", e.g. convert "F12" to "12"
            tmpStringExitKey1 = tmpStringExitKey1.Substring(1);
            tmpStringExitKey2 = tmpStringExitKey2.Substring(1);
            tmpStringExitKey3 = tmpStringExitKey3.Substring(1);

            // Finally, convert the string to an integer, e.g. "12" to 12
            settingInteger[StateTmp, GroupExitKeys, ValueExitKey1] = Int32.Parse(tmpStringExitKey1);
            settingInteger[StateTmp, GroupExitKeys, ValueExitKey2] = Int32.Parse(tmpStringExitKey2);
            settingInteger[StateTmp, GroupExitKeys, ValueExitKey3] = Int32.Parse(tmpStringExitKey3);

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
            // Choose Identity needs a conversion from integer to string
            int newIndexChooseIdentity = settingInteger[StateNew, GroupConfigFile, ValueChooseIdentity];
            settingString[StateNew, GroupConfigFile, ValueChooseIdentity] = chooseIdentityStringList[newIndexChooseIdentity];

            // Horizontal Positioning needs a conversion from integer to string
            int newIndexHorizPos = settingInteger[StateNew, GroupAppearance, ValueMainBrowserWindowHorizPos];
            settingString[StateNew, GroupAppearance, ValueMainBrowserWindowHorizPos] = horizontalPositioningString[newIndexHorizPos];

            // Exit Key Sequence needs a conversion from integer to string
            int newIndexExitKey1 = settingInteger[StateNew, GroupExitKeys, ValueExitKey1];
            int newIndexExitKey2 = settingInteger[StateNew, GroupExitKeys, ValueExitKey2];
            int newIndexExitKey3 = settingInteger[StateNew, GroupExitKeys, ValueExitKey3];

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

        private void listBoxMainBrowserWindowHorizPos_SelectedIndexChanged(object sender, EventArgs e)
        {
            settingInteger[StateNew, GroupAppearance, ValueMainBrowserWindowHorizPos] = listBoxMainBrowserWindowHorizPos.SelectedIndex + 1;
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



        // ****************
        // Group "Browser"
        // ****************



        // ****************
        // Group "Security"
        // ****************
        private void checkBoxEnableLogging_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupSecurity, ValueEnableLogging] = checkBoxEnableLogging.Checked;
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


        // ************************
        // Group "Security Options"
        // ************************
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
            settingBoolean[StateNew, GroupSecurityOptions, ValueNewBrowserWindowByLink] = checkBoxNewBrowserWindowByLinkBlockForeign.Checked;
        }

        private void checkBoxNewBrowserWindowByScriptBlockForeign_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupSecurityOptions, ValueNewBrowserWindowByScript] = checkBoxNewBrowserWindowByScriptBlockForeign.Checked;
        }

        private void checkBoxOpenDownloads_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupSecurityOptions, ValueOpenDownloads] = checkBoxOpenDownloads.Checked;
        }


        // *******************
        // Group "Online Exam"
        // *******************
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

            radioButtonUseBrowserWindow       .Checked       = settingBoolean[StateNew, GroupAppearance, ValueUseBrowserWindow];
            radioButtonUseFullScreenMode      .Checked       = settingBoolean[StateNew, GroupAppearance, ValueUseFullScreenMode];
             listBoxMainBrowserWindowHorizPos .SelectedIndex = settingInteger[StateNew, GroupAppearance, ValueMainBrowserWindowHorizPos] - 1;
            checkBoxEnableBrowserWindowToolbar.Checked       = settingBoolean[StateNew, GroupAppearance, ValueEnableBrowserWindowToolbar];
            checkBoxHideToolbarAsDefault      .Checked       = settingBoolean[StateNew, GroupAppearance, ValueHideToolbarAsDefault];
            checkBoxShowMenuBar               .Checked       = settingBoolean[StateNew, GroupAppearance, ValueShowMenuBar];
            checkBoxDisplaySEBDockTaskBar     .Checked       = settingBoolean[StateNew, GroupAppearance, ValueDisplaySEBDockTaskBar];

            checkBoxEnableLogging.Checked = settingBoolean[StateNew, GroupSecurity, ValueEnableLogging];

            listBoxExitKey1.SelectedIndex = settingInteger[StateNew, GroupExitKeys, ValueExitKey1] - 1;
            listBoxExitKey2.SelectedIndex = settingInteger[StateNew, GroupExitKeys, ValueExitKey2] - 1;
            listBoxExitKey3.SelectedIndex = settingInteger[StateNew, GroupExitKeys, ValueExitKey3] - 1;

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
            checkBoxNewBrowserWindowByLinkBlockForeign  .Checked = settingBoolean[StateNew, GroupSecurityOptions, ValueNewBrowserWindowByLink];
            checkBoxNewBrowserWindowByScriptBlockForeign.Checked = settingBoolean[StateNew, GroupSecurityOptions, ValueNewBrowserWindowByScript];
            checkBoxOpenDownloads                       .Checked = settingBoolean[StateNew, GroupSecurityOptions, ValueOpenDownloads];

            textBoxSebBrowser           .Text = settingString[StateNew, GroupOnlineExam, ValueSebBrowser];
            textBoxAutostartProcess     .Text = settingString[StateNew, GroupOnlineExam, ValueAutostartProcess];
            textBoxPermittedApplications.Text = settingString[StateNew, GroupOnlineExam, ValuePermittedApplications];

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
            SaveIniFile(currentPathSebStarterIni);

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





    } // end of   class     SebWindowsConfigForm
}     // end of   namespace SebWindowsConfig
