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

        // SEB has 2 different ini files:
        // SebStarter.ini and MsgHook.ini
        const int FileSebStarter = 1;
        const int FileMsgHook    = 2;

        const int FileNon = 0;
        const int FileMin = 1;
        const int FileMax = 2;
        const int FileNum = 2;

        // The values can be in 4 different states
        const int StateNon = 0;
        const int StateMin = 1;
        const int StateMax = 4;
        const int StateNum = 4;

        // We deal with different states of values:
        // old, new, temporary and default values
        const int StateOld = 1;
        const int StateNew = 2;
        const int StateTmp = 3;
        const int StateDef = 4;

        // The Graphical User Interface contains 5 groups
        const int GroupNon =  0;
        const int GroupMin =  1;
        const int GroupMax = 10;
        const int GroupNum = 10;

        // SebStarter contains the 5 groups
        // SebStarterFiles, InsideSeb, OutsideSeb, SecurityOptions, OnlineExam, OtherOptions
        const int GroupNonSebStarter = 0;
        const int GroupMinSebStarter = 1;
        const int GroupMaxSebStarter = 6;
        const int GroupNumSebStarter = 6;

        // MsgHook contains the 4 groups
        // MsgHookFiles, SpecialKeys, FunctionKeys, ExitSequence
        const int GroupNonMsgHook =  0;
        const int GroupMinMsgHook =  7;
        const int GroupMaxMsgHook = 10;
        const int GroupNumMsgHook =  4;

        // Group indices for SebStarter.ini
        const int GroupSebStarterFiles = 1;
        const int GroupInsideSeb       = 2;
        const int GroupOutsideSeb      = 3;
        const int GroupSecurityOptions = 4;
        const int GroupOnlineExam      = 5;
        const int GroupOtherOptions    = 6;

        // Group indices for MsgHook.ini
        const int GroupMsgHookFiles    =  7;
        const int GroupSpecialKeys     =  8;
        const int GroupFunctionKeys    =  9;
        const int GroupExitSequence    = 10;

        // Each group contains up to 12 values
        const int ValueNone =  0;
        const int ValueMin  =  1;
        const int ValueMax  = 12;
        const int ValueNum  = 12;

        // Group names
        const String MSG_SebStarterFiles = "SebStarterFiles";
        const String MSG_InsideSeb       = "InsideSeb";
        const String MSG_OutsideSeb      = "OutsideSeb";
        const String MSG_SecurityOptions = "SecurityOptions";
        const String MSG_OnlineExam      = "OnlineExam";
        const String MSG_OtherOptions    = "OtherOptions";

        const String MSG_MsgHookFiles    = "MsgHookFiles";
        const String MSG_SpecialKeys     = "SpecialKeys";
        const String MSG_FunctionKeys    = "FunctionKeys";
        const String MSG_ExitSequence    = "ExitSequence";

        // Group "SebStarter files"
        const int IND_CurrentSebStarterIni   = 1;
        const int IND_WriteSebStarterLogFile = 2;
        const int NumValuesSebStarterFiles   = 2;

        const String MSG_CurrentSebStarterIni   = "CurrentSebStarterIni";
        const String MSG_WriteSebStarterLogFile = "WriteSebStarterLogFile";

        // Groups "Inside SEB" and "Outside SEB"
        const int IND_EnableSwitchUser        = 1;
        const int IND_EnableLockThisComputer  = 2;
        const int IND_EnableChangeAPassword   = 3;
        const int IND_EnableStartTaskManager  = 4;
        const int IND_EnableLogOff            = 5;
        const int IND_EnableShutDown          = 6;
        const int IND_EnableEaseOfAccess      = 7;
        const int IND_EnableVmWareClientShade = 8;
        const int NumValuesInsideSeb          = 8;
        const int NumValuesOutsideSeb         = 8;

        const String MSG_InsideSebEnableSwitchUser        = "InsideSebEnableSwitchUser";
        const String MSG_InsideSebEnableLockThisComputer  = "InsideSebEnableLockThisComputer";
        const String MSG_InsideSebEnableChangeAPassword   = "InsideSebEnableChangeAPassword";
        const String MSG_InsideSebEnableStartTaskManager  = "InsideSebEnableStartTaskManager";
        const String MSG_InsideSebEnableLogOff            = "InsideSebEnableLogOff";
        const String MSG_InsideSebEnableShutDown          = "InsideSebEnableShutDown";
        const String MSG_InsideSebEnableEaseOfAccess      = "InsideSebEnableEaseOfAccess";
        const String MSG_InsideSebEnableVmWareClientShade = "InsideSebEnableVmWareClientShade";

        const String MSG_OutsideSebEnableSwitchUser        = "OutsideSebEnableSwitchUser";
        const String MSG_OutsideSebEnableLockThisComputer  = "OutsideSebEnableLockThisComputer";
        const String MSG_OutsideSebEnableChangeAPassword   = "OutsideSebEnableChangeAPassword";
        const String MSG_OutsideSebEnableStartTaskManager  = "OutsideSebEnableStartTaskManager";
        const String MSG_OutsideSebEnableLogOff            = "OutsideSebEnableLogOff";
        const String MSG_OutsideSebEnableShutDown          = "OutsideSebEnableShutDown";
        const String MSG_OutsideSebEnableEaseOfAccess      = "OutsideSebEnableEaseOfAccess";
        const String MSG_OutsideSebEnableVmWareClientShade = "OutsideSebEnableVmWareClientShade";

        // Group "Security options"
        const int IND_AllowVirtualMachine       = 1;
        const int IND_ForceWindowsService       = 2;
        const int IND_CreateNewDesktop          = 3;
        const int IND_ShowSebApplicationChooser = 4;
        const int IND_HookMessages              = 5;
        const int IND_EditRegistry              = 6;
        const int IND_MonitorProcesses          = 7;
        const int IND_ShutdownAfterAutostart    = 8;
        const int NumValuesSecurityOptions      = 8;

        const String MSG_AllowVirtualMachine       = "AllowVirtualMachine";
        const String MSG_ForceWindowsService       = "ForceWindowsService";
        const String MSG_CreateNewDesktop          = "CreateNewDesktop";
        const String MSG_ShowSebApplicationChooser = "ShowSebApplicationChooser";
        const String MSG_HookMessages              = "HookMessages";
        const String MSG_EditRegistry              = "EditRegistry";
        const String MSG_MonitorProcesses          = "MonitorProcesses";
        const String MSG_ShutdownAfterAutostart    = "ShutdownAfterAutostartProcessTerminates";

        // Group "Online exam"
        const int IND_SebBrowser            = 1;
        const int IND_AutostartProcess      = 2;
        const int IND_ExamUrl               = 3;
        const int IND_PermittedApplications = 4;
      //const int IND_QuitPassword          = 5;
      //const int IND_QuitHashcode          = 6;
        const int NumValuesOnlineExam       = 4;

        const String MSG_SebBrowser            = "SebBrowser";
        const String MSG_AutostartProcess      = "AutostartProcess";
        const String MSG_ExamUrl               = "ExamUrl";
        const String MSG_PermittedApplications = "PermittedApplications";
      //const String MSG_QuitPassword          = "QuitPassword";
      //const String MSG_QuitHashcode          = "QuitHashcode";

        // Group "Other options"
        const int IND_Win9xKillExplorer         = 1;
        const int IND_Win9xScreenSaverRunning   = 2;
        const int IND_StrongKillProcessesBefore = 3;
        const int IND_StrongKillProcessesAfter  = 4;
        const int NumValuesOtherOptions         = 4;

        const String MSG_Win9xKillExplorer         = "Win9xKillExplorer";
        const String MSG_Win9xScreenSaverRunning   = "Win9xScreenSaverRunning";
        const String MSG_StrongKillProcessesBefore = "StrongKillProcessesBefore";
        const String MSG_StrongKillProcessesAfter  = "StrongKillProcessesAfter";

        // Group "MsgHook files"
        const int IND_CurrentMsgHookIni   = 1;
        const int IND_WriteMsgHookLogFile = 2;
        const int NumValuesMsgHookFiles   = 2;

        const String MSG_CurrentMsgHookIni   = "CurrentMsgHookIni";
        const String MSG_WriteMsgHookLogFile = "WriteMsgHookLogFile";

        // Group "Special keys"
        const int IND_EnableEsc        = 1;
        const int IND_EnableCtrlEsc    = 2;
        const int IND_EnableAltEsc     = 3;
        const int IND_EnableAltTab     = 4;
        const int IND_EnableAltF4      = 5;
        const int IND_EnableStartMenu  = 6;
        const int IND_EnableRightMouse = 7;
        const int NumValuesSpecialKeys = 7;

        const String MSG_EnableEsc        = "EnableEsc";
        const String MSG_EnableCtrlEsc    = "EnableCtrlEsc";
        const String MSG_EnableAltEsc     = "EnableAltEsc";
        const String MSG_EnableAltTab     = "EnableAltTab";
        const String MSG_EnableAltF4      = "EnableAltF4";
        const String MSG_EnableStartMenu  = "EnableStartMenu";
        const String MSG_EnableRightMouse = "EnableRightMouse";

        // Group "Function keys"
        const int IND_EnableF1  = 1;
        const int IND_EnableF2  = 2;
        const int IND_EnableF3  = 3;
        const int IND_EnableF4  = 4;
        const int IND_EnableF5  = 5;
        const int IND_EnableF6  = 6;
        const int IND_EnableF7  = 7;
        const int IND_EnableF8  = 8;
        const int IND_EnableF9  = 9;
        const int IND_EnableF10 = 10;
        const int IND_EnableF11 = 11;
        const int IND_EnableF12 = 12;
        const int NumValuesFunctionKeys = 12;

        const String MSG_EnableF1  = "EnableF1";
        const String MSG_EnableF2  = "EnableF2";
        const String MSG_EnableF3  = "EnableF3";
        const String MSG_EnableF4  = "EnableF4";
        const String MSG_EnableF5  = "EnableF5";
        const String MSG_EnableF6  = "EnableF6";
        const String MSG_EnableF7  = "EnableF7";
        const String MSG_EnableF8  = "EnableF8";
        const String MSG_EnableF9  = "EnableF9";
        const String MSG_EnableF10 = "EnableF10";
        const String MSG_EnableF11 = "EnableF11";
        const String MSG_EnableF12 = "EnableF12";

        // Group "Exit sequence"
        const int IND_ExitKey1 = 1;
        const int IND_ExitKey2 = 2;
        const int IND_ExitKey3 = 3;
        const int NumValuesExitSequence = 3;

        const String MSG_ExitKey1 = "B1";
        const String MSG_ExitKey2 = "B2";
        const String MSG_ExitKey3 = "B3";

        // Types of values
        const int TYPE_Boolean = 1;
        const int TYPE_String  = 2;
        const int TYPE_Integer = 3;


        // Global variables

        // Virtual key code strings
        static String[] virtualKeyCodeString = new String[ValueNum + 1];

        // Number of values per group
        // Names  of groups and values
        // Types  of values (Boolean, Integer, String)
        static    int[ ]   numValues = new    int[GroupNum + 1];
        static String[ ] groupString = new String[GroupNum + 1];
        static String[,] valueString = new String[GroupNum + 1, ValueNum + 1];
        static    int[,]  dataType   = new    int[GroupNum + 1, ValueNum + 1];

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
                settingBoolean[StateDef, GroupOtherOptions   , value] = false;
                settingBoolean[StateDef, GroupSpecialKeys    , value] = false;
                settingBoolean[StateDef, GroupFunctionKeys   , value] = false;
                settingInteger[StateDef, GroupExitSequence   , value] = 0;
            }

            // Default settings for groups "SebStarter files" and "MsgHook files"
            settingBoolean[StateDef, GroupSebStarterFiles, IND_WriteSebStarterLogFile] = true;
            settingBoolean[StateDef, GroupMsgHookFiles   , IND_WriteMsgHookLogFile   ] = true;
            settingString [StateDef, GroupSebStarterFiles, IND_CurrentSebStarterIni  ] = "";
            settingString [StateDef, GroupMsgHookFiles   , IND_CurrentMsgHookIni     ] = "";

            // Default settings for group "Security options"
            settingBoolean[StateDef, GroupSecurityOptions, IND_AllowVirtualMachine      ] = false;
            settingBoolean[StateDef, GroupSecurityOptions, IND_ForceWindowsService      ] = true;
            settingBoolean[StateDef, GroupSecurityOptions, IND_CreateNewDesktop         ] = true;
            settingBoolean[StateDef, GroupSecurityOptions, IND_ShowSebApplicationChooser] = true;
            settingBoolean[StateDef, GroupSecurityOptions, IND_HookMessages             ] = true;
            settingBoolean[StateDef, GroupSecurityOptions, IND_EditRegistry             ] = true;
            settingBoolean[StateDef, GroupSecurityOptions, IND_MonitorProcesses         ] = false;
            settingBoolean[StateDef, GroupSecurityOptions, IND_ShutdownAfterAutostart   ] = false;

            // Default settings for group "Online exam"
	        String s1 = "Seb,../xulrunner/xulrunner.exe ../xul_seb/application.ini -profile ";
	        String s2 = "\"";
	        String s3 = "%LOCALAPPDATA%\\ETH Zuerich\\xul_seb\\Profiles";
	        String s4 = "\"";
            String SebBrowserString = s1 + s2 + s3 + s4;

            settingString[StateDef, GroupOnlineExam, IND_SebBrowser      ] = SebBrowserString;
            settingString[StateDef, GroupOnlineExam, IND_AutostartProcess] = "Seb";
            settingString[StateDef, GroupOnlineExam, IND_ExamUrl         ] = "http://www.safeexambrowser.org";
            settingString[StateDef, GroupOnlineExam, IND_PermittedApplications] = "Calculator,calc.exe;Notepad,notepad.exe;";
          //settingString[StateDef, GroupOnlineExam, IND_QuitPassword] = "";
          //settingString[StateDef, GroupOnlineExam, IND_QuitHashcode] = "";


            // Default settings for group "Other options"
            settingBoolean[StateDef, GroupOtherOptions, IND_Win9xKillExplorer        ] = true;
            settingBoolean[StateDef, GroupOtherOptions, IND_Win9xScreenSaverRunning  ] = false;
            settingString [StateDef, GroupOtherOptions, IND_StrongKillProcessesBefore] = "";
            settingString [StateDef, GroupOtherOptions, IND_StrongKillProcessesAfter ] = "";


            // Default settings for groups "Special keys" and "Function keys"
            settingBoolean[StateDef, GroupSpecialKeys , IND_EnableAltTab] = true;
            settingBoolean[StateDef, GroupFunctionKeys, IND_EnableF5    ] = true;

            // Default settings for group "Exit sequence"
            settingInteger[StateDef, GroupExitSequence, IND_ExitKey1] =  3;
            settingInteger[StateDef, GroupExitSequence, IND_ExitKey2] = 11;
            settingInteger[StateDef, GroupExitSequence, IND_ExitKey3] =  6;

            settingInteger[StateNew, GroupExitSequence, IND_ExitKey1] =  3;
            settingInteger[StateNew, GroupExitSequence, IND_ExitKey2] = 11;
            settingInteger[StateNew, GroupExitSequence, IND_ExitKey3] =  6;


            // Data types of the different values
            for (value = ValueMin; value <= ValueMax; value++)
            {
                dataType[GroupSebStarterFiles, value] = TYPE_Boolean;
                dataType[GroupInsideSeb      , value] = TYPE_Boolean;
                dataType[GroupOutsideSeb     , value] = TYPE_Boolean;
                dataType[GroupSecurityOptions, value] = TYPE_Boolean;
                dataType[GroupOnlineExam     , value] = TYPE_String;
                dataType[GroupOtherOptions   , value] = TYPE_Boolean;

                dataType[GroupMsgHookFiles, value] = TYPE_Boolean;
                dataType[GroupSpecialKeys , value] = TYPE_Boolean;
                dataType[GroupFunctionKeys, value] = TYPE_Boolean;
                dataType[GroupExitSequence, value] = TYPE_String;
            }

            dataType[GroupOtherOptions, IND_Win9xKillExplorer        ] = TYPE_Boolean;
            dataType[GroupOtherOptions, IND_Win9xScreenSaverRunning  ] = TYPE_Boolean;
            dataType[GroupOtherOptions, IND_StrongKillProcessesBefore] = TYPE_String;
            dataType[GroupOtherOptions, IND_StrongKillProcessesAfter ] = TYPE_String;

            dataType[GroupSebStarterFiles, IND_CurrentSebStarterIni  ] = TYPE_String;
            dataType[GroupSebStarterFiles, IND_WriteSebStarterLogFile] = TYPE_Boolean;

            dataType[GroupMsgHookFiles, IND_CurrentMsgHookIni  ] = TYPE_String;
            dataType[GroupMsgHookFiles, IND_WriteMsgHookLogFile] = TYPE_Boolean;

            // Number of values per group
            numValues[GroupSebStarterFiles] = NumValuesSebStarterFiles;
            numValues[GroupInsideSeb      ] = NumValuesInsideSeb;
            numValues[GroupOutsideSeb     ] = NumValuesOutsideSeb;
            numValues[GroupSecurityOptions] = NumValuesSecurityOptions;
            numValues[GroupOnlineExam     ] = NumValuesOnlineExam;
            numValues[GroupOtherOptions   ] = NumValuesOtherOptions;

            numValues[GroupMsgHookFiles   ] = NumValuesMsgHookFiles;
            numValues[GroupSpecialKeys    ] = NumValuesSpecialKeys;
            numValues[GroupFunctionKeys   ] = NumValuesFunctionKeys;
            numValues[GroupExitSequence   ] = NumValuesExitSequence;

            // Group names
            groupString[GroupSebStarterFiles] = MSG_SebStarterFiles;
            groupString[GroupInsideSeb      ] = MSG_InsideSeb;
            groupString[GroupOutsideSeb     ] = MSG_OutsideSeb;
            groupString[GroupSecurityOptions] = MSG_SecurityOptions;
            groupString[GroupOnlineExam     ] = MSG_OnlineExam;
            groupString[GroupOtherOptions   ] = MSG_OtherOptions;

            groupString[GroupMsgHookFiles   ] = MSG_MsgHookFiles;
            groupString[GroupSpecialKeys    ] = MSG_SpecialKeys;
            groupString[GroupFunctionKeys   ] = MSG_FunctionKeys;
            groupString[GroupExitSequence   ] = MSG_ExitSequence;

            // Value names
            valueString[GroupSebStarterFiles, IND_CurrentSebStarterIni  ] = MSG_CurrentSebStarterIni;
            valueString[GroupSebStarterFiles, IND_WriteSebStarterLogFile] = MSG_WriteSebStarterLogFile;

            valueString[GroupMsgHookFiles, IND_CurrentMsgHookIni  ] = MSG_CurrentMsgHookIni;
            valueString[GroupMsgHookFiles, IND_WriteMsgHookLogFile] = MSG_WriteMsgHookLogFile;

            valueString[GroupInsideSeb, IND_EnableSwitchUser       ] = MSG_InsideSebEnableSwitchUser;
            valueString[GroupInsideSeb, IND_EnableLockThisComputer ] = MSG_InsideSebEnableLockThisComputer;
            valueString[GroupInsideSeb, IND_EnableChangeAPassword  ] = MSG_InsideSebEnableChangeAPassword;
            valueString[GroupInsideSeb, IND_EnableStartTaskManager ] = MSG_InsideSebEnableStartTaskManager;
            valueString[GroupInsideSeb, IND_EnableLogOff           ] = MSG_InsideSebEnableLogOff;
            valueString[GroupInsideSeb, IND_EnableShutDown         ] = MSG_InsideSebEnableShutDown;
            valueString[GroupInsideSeb, IND_EnableEaseOfAccess     ] = MSG_InsideSebEnableEaseOfAccess;
            valueString[GroupInsideSeb, IND_EnableVmWareClientShade] = MSG_InsideSebEnableVmWareClientShade;

            valueString[GroupOutsideSeb, IND_EnableSwitchUser       ] = MSG_OutsideSebEnableSwitchUser;
            valueString[GroupOutsideSeb, IND_EnableLockThisComputer ] = MSG_OutsideSebEnableLockThisComputer;
            valueString[GroupOutsideSeb, IND_EnableChangeAPassword  ] = MSG_OutsideSebEnableChangeAPassword;
            valueString[GroupOutsideSeb, IND_EnableStartTaskManager ] = MSG_OutsideSebEnableStartTaskManager;
            valueString[GroupOutsideSeb, IND_EnableLogOff           ] = MSG_OutsideSebEnableLogOff;
            valueString[GroupOutsideSeb, IND_EnableShutDown         ] = MSG_OutsideSebEnableShutDown;
            valueString[GroupOutsideSeb, IND_EnableEaseOfAccess     ] = MSG_OutsideSebEnableEaseOfAccess;
            valueString[GroupOutsideSeb, IND_EnableVmWareClientShade] = MSG_OutsideSebEnableVmWareClientShade;

            valueString[GroupSecurityOptions, IND_AllowVirtualMachine      ] = MSG_AllowVirtualMachine;
            valueString[GroupSecurityOptions, IND_ForceWindowsService      ] = MSG_ForceWindowsService;
            valueString[GroupSecurityOptions, IND_CreateNewDesktop         ] = MSG_CreateNewDesktop;
            valueString[GroupSecurityOptions, IND_ShowSebApplicationChooser] = MSG_ShowSebApplicationChooser;
            valueString[GroupSecurityOptions, IND_HookMessages             ] = MSG_HookMessages;
            valueString[GroupSecurityOptions, IND_EditRegistry             ] = MSG_EditRegistry;
            valueString[GroupSecurityOptions, IND_MonitorProcesses         ] = MSG_MonitorProcesses;
            valueString[GroupSecurityOptions, IND_ShutdownAfterAutostart   ] = MSG_ShutdownAfterAutostart;

            valueString[GroupOnlineExam, IND_SebBrowser           ] = MSG_SebBrowser;
            valueString[GroupOnlineExam, IND_AutostartProcess     ] = MSG_AutostartProcess;
            valueString[GroupOnlineExam, IND_ExamUrl              ] = MSG_ExamUrl;
            valueString[GroupOnlineExam, IND_PermittedApplications] = MSG_PermittedApplications;
          //valueString[GroupOnlineExam, IND_QuitPassword         ] = MSG_QuitPassword;
          //valueString[GroupOnlineExam, IND_QuitHashcode         ] = MSG_QuitHashcode;

            valueString[GroupOtherOptions, IND_Win9xKillExplorer        ] = MSG_Win9xKillExplorer;
            valueString[GroupOtherOptions, IND_Win9xScreenSaverRunning  ] = MSG_Win9xScreenSaverRunning;
            valueString[GroupOtherOptions, IND_StrongKillProcessesBefore] = MSG_StrongKillProcessesBefore;
            valueString[GroupOtherOptions, IND_StrongKillProcessesAfter ] = MSG_StrongKillProcessesAfter;

            valueString[GroupSpecialKeys, IND_EnableEsc       ] = MSG_EnableEsc;
            valueString[GroupSpecialKeys, IND_EnableCtrlEsc   ] = MSG_EnableCtrlEsc;
            valueString[GroupSpecialKeys, IND_EnableAltEsc    ] = MSG_EnableAltEsc;
            valueString[GroupSpecialKeys, IND_EnableAltTab    ] = MSG_EnableAltTab;
            valueString[GroupSpecialKeys, IND_EnableAltF4     ] = MSG_EnableAltF4;
            valueString[GroupSpecialKeys, IND_EnableStartMenu ] = MSG_EnableStartMenu;
            valueString[GroupSpecialKeys, IND_EnableRightMouse] = MSG_EnableRightMouse;

            valueString[GroupFunctionKeys, IND_EnableF1 ] = MSG_EnableF1;
            valueString[GroupFunctionKeys, IND_EnableF2 ] = MSG_EnableF2;
            valueString[GroupFunctionKeys, IND_EnableF3 ] = MSG_EnableF3;
            valueString[GroupFunctionKeys, IND_EnableF4 ] = MSG_EnableF4;
            valueString[GroupFunctionKeys, IND_EnableF5 ] = MSG_EnableF5;
            valueString[GroupFunctionKeys, IND_EnableF6 ] = MSG_EnableF6;
            valueString[GroupFunctionKeys, IND_EnableF7 ] = MSG_EnableF7;
            valueString[GroupFunctionKeys, IND_EnableF8 ] = MSG_EnableF8;
            valueString[GroupFunctionKeys, IND_EnableF9 ] = MSG_EnableF9;
            valueString[GroupFunctionKeys, IND_EnableF10] = MSG_EnableF10;
            valueString[GroupFunctionKeys, IND_EnableF11] = MSG_EnableF11;
            valueString[GroupFunctionKeys, IND_EnableF12] = MSG_EnableF12;

            valueString[GroupExitSequence, IND_ExitKey1] = MSG_ExitKey1;
            valueString[GroupExitSequence, IND_ExitKey2] = MSG_ExitKey2;
            valueString[GroupExitSequence, IND_ExitKey3] = MSG_ExitKey3;

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

            // Initialise the widgets to the default settings
            SetWidgetsToSettingsOfFile(FileSebStarter, StateDef);
            SetWidgetsToSettingsOfFile(FileMsgHook   , StateDef);

            openFileDialogSebStarterIni.InitialDirectory = System.Environment.CurrentDirectory;
            saveFileDialogSebStarterIni.InitialDirectory = System.Environment.CurrentDirectory;
            openFileDialogMsgHookIni   .InitialDirectory = System.Environment.CurrentDirectory;
            saveFileDialogMsgHookIni   .InitialDirectory = System.Environment.CurrentDirectory;

        } // end of contructor   SebWindowsConfigForm()




        // ****************************************
        // Open SebStarter configuration file click
        // ****************************************
        private void labelOpenSebStarterConfigurationFile_Click(object sender, EventArgs e)
        {
            DialogResult fileDialog = openFileDialogSebStarterIni.ShowDialog();
            String       fileName   = openFileDialogSebStarterIni.FileName;

            OpenFileSebStarterIni(fileName);

        } // end of method   labelOpenSebStarterConfigurationFile_Click()



        // ************************
        // Open file SebStarter.ini
        // ************************
        private void OpenFileSebStarterIni(String fileName)
        {
            FileStream   fileStream;
            StreamReader fileReader;
            String       fileLine;

            int group, value;
            int groupMin = GroupMinSebStarter;
            int groupMax = GroupMaxSebStarter;
            int valueMin = ValueMin;
            int valueMax = ValueMax;

            settingString[StateTmp, GroupSebStarterFiles, IND_CurrentSebStarterIni] = fileName;

            try 
            {
                // Open the SebStarter.ini file for reading
                fileStream = new   FileStream(fileName, FileMode.Open, FileAccess.Read);
                fileReader = new StreamReader(fileStream);

                // Read lines from the SebStarter.ini file until end of file is reached
                while ((fileLine = fileReader.ReadLine()) != null) 
                {
                    // Skip empty lines and lines not in "leftSide = rightSide" format
                    if (fileLine.Contains("="))
                    {
                        int      equalPosition =    fileLine.IndexOf  ("=");
                        String   leftString    =    fileLine.Remove   (equalPosition);
                        String  rightString    =    fileLine.Substring(equalPosition + 1);
                        Boolean rightBoolean   = rightString.Equals("1");

                        // Find the appropriate group and setting
                        for (group = groupMin; group <= groupMax; group++)
                        for (value = valueMin; value <= valueMax; value++)
                        {
                            if (leftString.Equals(valueString[group, value]))
                            {
                                settingBoolean[StateTmp, group, value] = rightBoolean;
                                settingString [StateTmp, group, value] = rightString;
                            }
                        }

                    } // end if line.Contains("=")
                } // end while

                // Close the SebStarter.ini file
                fileReader.Close();
                fileStream.Close();

            } // end try

            catch (Exception streamReadException) 
            {
                // Let the user know what went wrong.
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(streamReadException.Message);
                return;
            }

            // Accept the tmp values as the new values
            for (group = groupMin; group <= groupMax; group++)
            for (value = valueMin; value <= valueMax; value++)
            {
                settingBoolean[StateOld, group, value] = settingBoolean[StateTmp, group, value];
                settingBoolean[StateNew, group, value] = settingBoolean[StateTmp, group, value];
                settingString [StateOld, group, value] = settingString [StateTmp, group, value];
                settingString [StateNew, group, value] = settingString [StateTmp, group, value];
            }

            // Assign the settings from the SebStarter.ini file to the widgets
            SetWidgetsToNewSettingsOfSebStarterIni();

        } // end of method   OpenFileSebStarterIni()




        // ****************************************
        // Save SebStarter configuration file click
        // ****************************************
        private void labelSaveSebStarterConfigurationFile_Click(object sender, EventArgs e)
        {
            DialogResult fileDialog = saveFileDialogSebStarterIni.ShowDialog();
            String       fileName   = saveFileDialogSebStarterIni.FileName;

            SaveFileSebStarterIni(fileName);

        } // end of method   labelSaveSebStarterConfigurationFile_Click()



        // ************************
        // Save file SebStarter.ini
        // ************************
        private void SaveFileSebStarterIni(String fileName)
        {
            FileStream   fileStream;
            StreamWriter fileWriter;
            String       fileLine;

            int group, value;
            int groupMin = GroupMinSebStarter;
            int groupMax = GroupMaxSebStarter;
            int valueMin = 0;
            int valueMax = 0;

            settingString[StateTmp, GroupSebStarterFiles, IND_CurrentSebStarterIni] = fileName;

            try 
            {
                // Open the output file for writing
                fileStream = new   FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write);
                fileWriter = new StreamWriter(fileStream);

                // Write the header lines
                fileWriter.WriteLine("");
                fileWriter.WriteLine("[SEB]");
                fileWriter.WriteLine("");

                // For each group and each value,
                // write the line "...=..." into the output file
                for (group = groupMin; group <= groupMax; group++)
                {
                    valueMin = 1;
                    valueMax = numValues[group];

                    // Write the group name
                    fileWriter.WriteLine("[" + groupString[group] + "]");
                    fileWriter.WriteLine("");

                    for (value = valueMin; value <= valueMax; value++)
                    {
                        String   leftString    =   valueString [          group, value];
                        String  rightString    = settingString [StateNew, group, value];
                        Boolean rightBoolean   = settingBoolean[StateNew, group, value];
                        int     rightType      =    dataType   [          group, value];

                        if ((rightType == TYPE_Boolean) && (rightBoolean == false)) rightString = "0";
                        if ((rightType == TYPE_Boolean) && (rightBoolean ==  true)) rightString = "1";

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
                // Let the user know what went wrong.
                Console.WriteLine("The file could not be written:");
                Console.WriteLine(streamWriteException.Message);
                return;
            }

            // Accept the tmp values as the new values
            settingString[StateOld, GroupSebStarterFiles, IND_CurrentSebStarterIni] = fileName;
            settingString[StateNew, GroupSebStarterFiles, IND_CurrentSebStarterIni] = fileName;

            for (group = groupMin; group <= groupMax; group++)
            for (value = valueMin; value <= valueMax; value++)
            {
                settingBoolean[StateOld, group, value] = settingBoolean[StateNew, group, value];
                settingString [StateOld, group, value] = settingString [StateNew, group, value];
                settingInteger[StateOld, group, value] = settingInteger[StateNew, group, value];
            }

            textBoxCurrentSebStarterIni.Text = fileName;

        } // end of method   SaveFileSebStarterIni()




        // *************************************
        // Open MsgHook configuration file click
        // *************************************
        private void labelOpenMsgHookConfigurationFile_Click(object sender, EventArgs e)
        {
            DialogResult fileDialog = openFileDialogMsgHookIni.ShowDialog();
            String       fileName   = openFileDialogMsgHookIni.FileName;

            OpenFileMsgHookIni(fileName);

        }  // end of method   labelOpenMsgHookConfigurationFile_Click()



        // *********************
        // Open file MsgHook.ini
        // *********************
        private void OpenFileMsgHookIni(String fileName)
        {
            FileStream   fileStream;
            StreamReader fileReader;
            String       fileLine;

            int group, value;
            int groupMin = GroupMinMsgHook;
            int groupMax = GroupMaxMsgHook;
            int valueMin = ValueMin;
            int valueMax = ValueMax;

            settingString[StateTmp, GroupMsgHookFiles, IND_CurrentMsgHookIni] = fileName;

            try 
            {
                // Open the MsgHook.ini file for reading
                fileStream = new   FileStream(fileName, FileMode.Open, FileAccess.Read);
                fileReader = new StreamReader(fileStream);

                // Read lines from the SebStarter.ini file until end of file is reached
                while ((fileLine = fileReader.ReadLine()) != null)
                {
                    // Skip empty lines and lines not in "leftSide = rightSide" format
                    if (fileLine.Contains("="))
                    {
                        int      equalPosition =    fileLine.IndexOf  ("=");
                        String   leftString    =    fileLine.Remove   (equalPosition);
                        String  rightString    =    fileLine.Substring(equalPosition + 1);
                        Boolean rightBoolean   = rightString.Equals("1");

                        // Find the appropriate group and setting
                        for (group = groupMin; group <= groupMax; group++)
                        for (value = valueMin; value <= valueMax; value++)
                        {
                            if (leftString.Equals(valueString[group, value]))
                            {
                                settingBoolean[StateTmp, group, value] = rightBoolean;
                                settingString [StateTmp, group, value] = rightString;
                            }
                        }

                    } // end if line.Contains("=")
                } // end while

                // Close the MsgHook.ini file
                fileReader.Close();
                fileStream.Close();

            } // end try

            catch (Exception streamReadException) 
            {
                // Let the user know what went wrong.
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(streamReadException.Message);
                return;
            }

            // Convert the B1, B2, B3 strings to integers
            String tmpB1 = settingString[StateTmp, GroupExitSequence, IND_ExitKey1];
            String tmpB2 = settingString[StateTmp, GroupExitSequence, IND_ExitKey2];
            String tmpB3 = settingString[StateTmp, GroupExitSequence, IND_ExitKey3];

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

            settingInteger[StateTmp, GroupExitSequence, IND_ExitKey1] = tmpIndexExitKey1;
            settingInteger[StateTmp, GroupExitSequence, IND_ExitKey2] = tmpIndexExitKey2;
            settingInteger[StateTmp, GroupExitSequence, IND_ExitKey3] = tmpIndexExitKey3;

            // Accept the tmp values as the new values
            for (group = groupMin; group <= groupMax; group++)
            for (value = valueMin; value <= valueMax; value++)
            {
                settingBoolean[StateOld, group, value] = settingBoolean[StateTmp, group, value];
                settingBoolean[StateNew, group, value] = settingBoolean[StateTmp, group, value];
                settingString [StateOld, group, value] = settingString [StateTmp, group, value];
                settingString [StateNew, group, value] = settingString [StateTmp, group, value];
                settingInteger[StateOld, group, value] = settingInteger[StateTmp, group, value];
                settingInteger[StateNew, group, value] = settingInteger[StateTmp, group, value];
            }

            // Assign the settings from the MsgHook.ini file to the widgets
            SetWidgetsToNewSettingsOfMsgHookIni();

        }  // end of method   OpenFileMsgHookIni()




        // *************************************
        // Save MsgHook configuration file click
        // *************************************
        private void labelSaveMsgHookConfigurationFile_Click(object sender, EventArgs e)
        {
            DialogResult fileDialog = saveFileDialogMsgHookIni.ShowDialog();
            String       fileName   = saveFileDialogMsgHookIni.FileName;

            SaveFileMsgHookIni(fileName);

        }  // end of method   labelSaveMsgHookConfigurationFile_Click()



        // *********************
        // Save file MsgHook.ini
        // *********************
        private void SaveFileMsgHookIni(String fileName)
        {
            FileStream   fileStream;
            StreamWriter fileWriter;
            String       fileLine;

            int group, value;
            int groupMin = GroupMinMsgHook;
            int groupMax = GroupMaxMsgHook;
            int valueMin = 0;
            int valueMax = 0;

            settingString[StateTmp, GroupMsgHookFiles, IND_CurrentMsgHookIni] = fileName;

            int newIndexExitKey1 = settingInteger[StateNew, GroupExitSequence, IND_ExitKey1];
            int newIndexExitKey2 = settingInteger[StateNew, GroupExitSequence, IND_ExitKey2];
            int newIndexExitKey3 = settingInteger[StateNew, GroupExitSequence, IND_ExitKey3];

            settingString[StateNew, GroupExitSequence, IND_ExitKey1] = virtualKeyCodeString[newIndexExitKey1];
            settingString[StateNew, GroupExitSequence, IND_ExitKey2] = virtualKeyCodeString[newIndexExitKey2];
            settingString[StateNew, GroupExitSequence, IND_ExitKey3] = virtualKeyCodeString[newIndexExitKey3];

            try 
            {
                // Open the output file for writing
                fileStream = new   FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write);
                fileWriter = new StreamWriter(fileStream);

                // Write the header lines
                fileWriter.WriteLine("");
                fileWriter.WriteLine("[SEB]");
                fileWriter.WriteLine("");

                // For each group and each value,
                // write the line "...=..." into the output file
                for (group = groupMin; group <= groupMax; group++)
                {
                    valueMin = 1;
                    valueMax = numValues[group];

                    // Write the group name
                    fileWriter.WriteLine("[" + groupString[group] + "]");
                    fileWriter.WriteLine("");

                    for (value = valueMin; value <= valueMax; value++)
                    {
                        String   leftString    =   valueString [          group, value];
                        String  rightString    = settingString [StateNew, group, value];
                        Boolean rightBoolean   = settingBoolean[StateNew, group, value];
                        int     rightType      =    dataType   [          group, value];

                        if ((rightType == TYPE_Boolean) && (rightBoolean == false)) rightString = "0";
                        if ((rightType == TYPE_Boolean) && (rightBoolean ==  true)) rightString = "1";

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
                // Let the user know what went wrong.
                Console.WriteLine("The file could not be written:");
                Console.WriteLine(streamWriteException.Message);
                return;
            }

            // Accept the tmp values as the new values
            settingString[StateOld, GroupMsgHookFiles, IND_CurrentMsgHookIni] = fileName;
            settingString[StateNew, GroupMsgHookFiles, IND_CurrentMsgHookIni] = fileName;

            for (group = groupMin; group <= groupMax; group++)
            for (value = valueMin; value <= valueMax; value++)
            {
                settingBoolean[StateOld, group, value] = settingBoolean[StateNew, group, value];
                settingString [StateOld, group, value] = settingString [StateNew, group, value];
                settingInteger[StateOld, group, value] = settingInteger[StateNew, group, value];
            }

            textBoxCurrentMsgHookIni.Text = fileName;

        }  // end of method   SaveFileMsgHookIni()




        // ******************************************************
        // Event handlers:
        // If the user changes a setting by clicking or typing,
        // update the setting in memory for later saving on file.
        // ******************************************************

        // Group "SebStarter files"
        private void checkBoxWriteSebStarterLogFile_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupSebStarterFiles, IND_WriteSebStarterLogFile] = checkBoxWriteSebStarterLogFile.Checked;
        }


        // Group "Inside SEB"
        private void checkBoxInsideSebEnableSwitchUser_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupInsideSeb, IND_EnableSwitchUser] = checkBoxInsideSebEnableSwitchUser.Checked;
        }

        private void checkBoxInsideSebEnableLockThisComputer_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupInsideSeb, IND_EnableLockThisComputer] = checkBoxInsideSebEnableLockThisComputer.Checked;
        }

        private void checkBoxInsideSebEnableChangeAPassword_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupInsideSeb, IND_EnableChangeAPassword] = checkBoxInsideSebEnableChangeAPassword.Checked;
        }

        private void checkBoxInsideSebEnableStartTaskManager_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupInsideSeb, IND_EnableStartTaskManager] = checkBoxInsideSebEnableStartTaskManager.Checked;
        }

        private void checkBoxInsideSebEnableLogOff_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupInsideSeb, IND_EnableLogOff] = checkBoxInsideSebEnableLogOff.Checked;
        }

        private void checkBoxInsideSebEnableShutDown_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupInsideSeb, IND_EnableShutDown] = checkBoxInsideSebEnableShutDown.Checked;
        }

        private void checkBoxInsideSebEnableEaseOfAccess_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupInsideSeb, IND_EnableEaseOfAccess] = checkBoxInsideSebEnableEaseOfAccess.Checked;
        }

        private void checkBoxInsideSebEnableVmWareClientShade_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupInsideSeb, IND_EnableVmWareClientShade] = checkBoxInsideSebEnableVmWareClientShade.Checked;
        }
 

        // Group "Outside SEB"
        private void checkBoxOutsideSebEnableSwitchUser_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupOutsideSeb, IND_EnableSwitchUser] = checkBoxOutsideSebEnableSwitchUser.Checked;
        }

        private void checkBoxOutsideSebEnableLockThisComputer_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupOutsideSeb, IND_EnableLockThisComputer] = checkBoxOutsideSebEnableLockThisComputer.Checked;
        }

        private void checkBoxOutsideSebEnableChangeAPassword_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupOutsideSeb, IND_EnableChangeAPassword] = checkBoxOutsideSebEnableChangeAPassword.Checked;
        }

        private void checkBoxOutsideSebEnableStartTaskManager_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupOutsideSeb, IND_EnableStartTaskManager] = checkBoxOutsideSebEnableStartTaskManager.Checked;
        }

        private void checkBoxOutsideSebEnableLogOff_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupOutsideSeb, IND_EnableLogOff] = checkBoxOutsideSebEnableLogOff.Checked;
        }

        private void checkBoxOutsideSebEnableShutDown_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupOutsideSeb, IND_EnableShutDown] = checkBoxOutsideSebEnableShutDown.Checked;
        }

        private void checkBoxOutsideSebEnableEaseOfAccess_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupOutsideSeb, IND_EnableEaseOfAccess] = checkBoxOutsideSebEnableEaseOfAccess.Checked;
        }

        private void checkBoxOutsideSebEnableVmWareClientShade_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupOutsideSeb, IND_EnableVmWareClientShade] = checkBoxOutsideSebEnableVmWareClientShade.Checked;
        }


        // Group "Security options"
        private void checkBoxAllowVirtualMachine_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupSecurityOptions, IND_AllowVirtualMachine] = checkBoxAllowVirtualMachine.Checked;
        }

        private void checkBoxForceWindowsService_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupSecurityOptions, IND_ForceWindowsService] = checkBoxForceWindowsService.Checked;
        }

        private void checkBoxCreateNewDesktop_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupSecurityOptions, IND_CreateNewDesktop] = checkBoxCreateNewDesktop.Checked;
        }

        private void checkBoxShowSebApplicationChooser_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupSecurityOptions, IND_ShowSebApplicationChooser] = checkBoxShowSebApplicationChooser.Checked;
        }

        private void checkBoxHookMessages_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupSecurityOptions, IND_HookMessages] = checkBoxHookMessages.Checked;
        }

        private void checkBoxEditRegistry_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupSecurityOptions, IND_EditRegistry] = checkBoxEditRegistry.Checked;
        }

        private void checkBoxMonitorProcesses_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupSecurityOptions, IND_MonitorProcesses] = checkBoxMonitorProcesses.Checked;
        }

        private void checkBoxShutdownAfterAutostart_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupSecurityOptions, IND_ShutdownAfterAutostart] = checkBoxShutdownAfterAutostart.Checked;
        }


        // Group "Online exam"
        private void textBoxSebBrowser_TextChanged(object sender, EventArgs e)
        {
            settingString[StateNew, GroupOnlineExam, IND_SebBrowser] = textBoxSebBrowser.Text;
        }

        private void textBoxAutostartProcess_TextChanged(object sender, EventArgs e)
        {
            settingString[StateNew, GroupOnlineExam, IND_AutostartProcess] = textBoxAutostartProcess.Text;
        }

        private void textBoxExamUrl_TextChanged(object sender, EventArgs e)
        {
            settingString[StateNew, GroupOnlineExam, IND_ExamUrl] = textBoxExamUrl.Text;
        }

        private void textBoxPermittedApplications_TextChanged(object sender, EventArgs e)
        {
            settingString[StateNew, GroupOnlineExam, IND_PermittedApplications] = textBoxPermittedApplications.Text;
        }


        // Group "MsgHook files"
        private void checkBoxWriteMsgHookLogFile_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupMsgHookFiles, IND_WriteMsgHookLogFile] = checkBoxWriteMsgHookLogFile.Checked;
        }


        // Group "Special keys"
        private void checkBoxEnableEsc_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupSpecialKeys, IND_EnableEsc] = checkBoxEnableEsc.Checked;
        }

        private void checkBoxEnableCtrlEsc_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupSpecialKeys, IND_EnableCtrlEsc] = checkBoxEnableCtrlEsc.Checked;
        }

        private void checkBoxEnableAltEsc_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupSpecialKeys, IND_EnableAltEsc] = checkBoxEnableAltEsc.Checked;
        }

        private void checkBoxEnableAltTab_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupSpecialKeys, IND_EnableAltTab] = checkBoxEnableAltTab.Checked;
        }

        private void checkBoxEnableAltF4_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupSpecialKeys, IND_EnableAltF4] = checkBoxEnableAltF4.Checked;
        }

        private void checkBoxEnableStartMenu_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupSpecialKeys, IND_EnableStartMenu] = checkBoxEnableStartMenu.Checked;
        }

        private void checkBoxEnableRightMouse_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupSpecialKeys, IND_EnableRightMouse] = checkBoxEnableRightMouse.Checked;
        }


        // Group "Function keys"
        private void checkBoxEnableF1_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupFunctionKeys, IND_EnableF1] = checkBoxEnableF1.Checked;
        }

        private void checkBoxEnableF2_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupFunctionKeys, IND_EnableF2] = checkBoxEnableF2.Checked;
        }

        private void checkBoxEnableF3_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupFunctionKeys, IND_EnableF3] = checkBoxEnableF3.Checked;
        }

        private void checkBoxEnableF4_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupFunctionKeys, IND_EnableF4] = checkBoxEnableF4.Checked;
        }

        private void checkBoxEnableF5_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupFunctionKeys, IND_EnableF5] = checkBoxEnableF5.Checked;
        }

        private void checkBoxEnableF6_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupFunctionKeys, IND_EnableF6] = checkBoxEnableF6.Checked;
        }

        private void checkBoxEnableF7_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupFunctionKeys, IND_EnableF7] = checkBoxEnableF7.Checked;
        }

        private void checkBoxEnableF8_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupFunctionKeys, IND_EnableF8] = checkBoxEnableF8.Checked;
        }

        private void checkBoxEnableF9_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupFunctionKeys, IND_EnableF9] = checkBoxEnableF9.Checked;
        }

        private void checkBoxEnableF10_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupFunctionKeys, IND_EnableF10] = checkBoxEnableF10.Checked;
        }

        private void checkBoxEnableF11_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupFunctionKeys, IND_EnableF11] = checkBoxEnableF11.Checked;
        }

        private void checkBoxEnableF12_CheckedChanged(object sender, EventArgs e)
        {
            settingBoolean[StateNew, GroupFunctionKeys, IND_EnableF12] = checkBoxEnableF12.Checked;
        }


        // Group "Exit sequence"
        private void listBoxExitKey1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Make sure that all three exit keys are different.
            // If selected key is already occupied, revert to previously selected key.
            if ((listBoxExitKey1.SelectedIndex == listBoxExitKey2.SelectedIndex) ||
                (listBoxExitKey1.SelectedIndex == listBoxExitKey3.SelectedIndex))
                 listBoxExitKey1.SelectedIndex =  settingInteger[StateNew, GroupExitSequence, IND_ExitKey1] - 1;

            settingInteger[StateNew, GroupExitSequence, IND_ExitKey1] = listBoxExitKey1.SelectedIndex + 1;
        }


        private void listBoxExitKey2_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Make sure that all three exit keys are different.
            // If selected key is already occupied, revert to previously selected key.
            if ((listBoxExitKey2.SelectedIndex == listBoxExitKey1.SelectedIndex) ||
                (listBoxExitKey2.SelectedIndex == listBoxExitKey3.SelectedIndex))
                 listBoxExitKey2.SelectedIndex =  settingInteger[StateNew, GroupExitSequence, IND_ExitKey2] - 1;

            settingInteger[StateNew, GroupExitSequence, IND_ExitKey2] = listBoxExitKey2.SelectedIndex + 1;
        }


        private void listBoxExitKey3_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Make sure that all three exit keys are different.
            // If selected key is already occupied, revert to previously selected key.
            if ((listBoxExitKey3.SelectedIndex == listBoxExitKey1.SelectedIndex) ||
                (listBoxExitKey3.SelectedIndex == listBoxExitKey2.SelectedIndex))
                 listBoxExitKey3.SelectedIndex =  settingInteger[StateNew, GroupExitSequence, IND_ExitKey3] - 1;

            settingInteger[StateNew, GroupExitSequence, IND_ExitKey3] = listBoxExitKey3.SelectedIndex + 1;
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

            //settingString[StateNew, GroupOnlineExam, IND_QuitPassword] = newStringQuitPassword;
            //settingString[StateNew, GroupOnlineExam, IND_QuitHashcode] = newStringQuitHashcode;
        }




        // *****************************************
        // Restore SebStarter default settings click
        // *****************************************
        private void buttonRestoreSebStarterDefaultSettings_Click(object sender, EventArgs e)
        {
            SetWidgetsToSettingsOfFile(FileSebStarter, StateDef);
        }

        // ****************************************************
        // Restore SebStarter configuration file settings click
        // ****************************************************
        private void buttonRestoreSebStarterConfigurationFileSettings_Click(object sender, EventArgs e)
        {
            SetWidgetsToSettingsOfFile(FileSebStarter, StateOld);
        }

        // **************************************
        // Restore MsgHook default settings click
        // **************************************
        private void buttonRestoreMsgHookDefaultSettings_Click(object sender, EventArgs e)
        {
            SetWidgetsToSettingsOfFile(FileMsgHook, StateDef);
        }

        // *************************************************
        // Restore MsgHook configuration file settings click
        // *************************************************
        private void buttonRestoreMsgHookConfigurationFileSettings_Click(object sender, EventArgs e)
        {
            SetWidgetsToSettingsOfFile(FileMsgHook, StateOld);
        }



        // ********************************************************
        // Set the widgets to the desired settings of desired file
        // ********************************************************
        private void SetWidgetsToSettingsOfFile(int iniFile, int stateDesired)
        {
            int group, value;
            int groupMin = 0;
            int groupMax = 0;
            int valueMin = ValueMin;
            int valueMax = ValueMax;

            if (iniFile == FileSebStarter) groupMin = GroupMinSebStarter;
            if (iniFile == FileSebStarter) groupMax = GroupMaxSebStarter;
            if (iniFile == FileMsgHook   ) groupMin = GroupMinMsgHook;
            if (iniFile == FileMsgHook   ) groupMax = GroupMaxMsgHook;

            // Restore the desired values by copying them to the new values
            for (group = groupMin; group <= groupMax; group++)
            for (value = valueMin; value <= valueMax; value++)
            {
                settingBoolean[StateNew, group, value] = settingBoolean[stateDesired, group, value];
                settingString [StateNew, group, value] = settingString [stateDesired, group, value];
                settingInteger[StateNew, group, value] = settingInteger[stateDesired, group, value];
            }

            if (iniFile == FileSebStarter) SetWidgetsToNewSettingsOfSebStarterIni();
            if (iniFile == FileMsgHook   ) SetWidgetsToNewSettingsOfMsgHookIni();
        }



        // *****************************************************
        // Set the widgets to the new settings of SebStarter.ini
        // *****************************************************
        private void SetWidgetsToNewSettingsOfSebStarterIni()
        {
            // Set the widgets to the new settings
            textBoxCurrentSebStarterIni   .Text    = settingString [StateNew, GroupSebStarterFiles, IND_CurrentSebStarterIni];
            checkBoxWriteSebStarterLogFile.Checked = settingBoolean[StateNew, GroupSebStarterFiles, IND_WriteSebStarterLogFile];

            checkBoxInsideSebEnableSwitchUser       .Checked = settingBoolean[StateNew, GroupInsideSeb, IND_EnableSwitchUser];
            checkBoxInsideSebEnableLockThisComputer .Checked = settingBoolean[StateNew, GroupInsideSeb, IND_EnableLockThisComputer];
            checkBoxInsideSebEnableChangeAPassword  .Checked = settingBoolean[StateNew, GroupInsideSeb, IND_EnableChangeAPassword];
            checkBoxInsideSebEnableStartTaskManager .Checked = settingBoolean[StateNew, GroupInsideSeb, IND_EnableStartTaskManager];
            checkBoxInsideSebEnableLogOff           .Checked = settingBoolean[StateNew, GroupInsideSeb, IND_EnableLogOff];
            checkBoxInsideSebEnableShutDown         .Checked = settingBoolean[StateNew, GroupInsideSeb, IND_EnableShutDown];
            checkBoxInsideSebEnableEaseOfAccess     .Checked = settingBoolean[StateNew, GroupInsideSeb, IND_EnableEaseOfAccess];
            checkBoxInsideSebEnableVmWareClientShade.Checked = settingBoolean[StateNew, GroupInsideSeb, IND_EnableVmWareClientShade];

            checkBoxOutsideSebEnableSwitchUser       .Checked = settingBoolean[StateNew, GroupOutsideSeb, IND_EnableSwitchUser];
            checkBoxOutsideSebEnableLockThisComputer .Checked = settingBoolean[StateNew, GroupOutsideSeb, IND_EnableLockThisComputer];
            checkBoxOutsideSebEnableChangeAPassword  .Checked = settingBoolean[StateNew, GroupOutsideSeb, IND_EnableChangeAPassword];
            checkBoxOutsideSebEnableStartTaskManager .Checked = settingBoolean[StateNew, GroupOutsideSeb, IND_EnableStartTaskManager];
            checkBoxOutsideSebEnableLogOff           .Checked = settingBoolean[StateNew, GroupOutsideSeb, IND_EnableLogOff];
            checkBoxOutsideSebEnableShutDown         .Checked = settingBoolean[StateNew, GroupOutsideSeb, IND_EnableShutDown];
            checkBoxOutsideSebEnableEaseOfAccess     .Checked = settingBoolean[StateNew, GroupOutsideSeb, IND_EnableEaseOfAccess];
            checkBoxOutsideSebEnableVmWareClientShade.Checked = settingBoolean[StateNew, GroupOutsideSeb, IND_EnableVmWareClientShade];

            checkBoxAllowVirtualMachine      .Checked = settingBoolean[StateNew, GroupSecurityOptions, IND_AllowVirtualMachine];
            checkBoxForceWindowsService      .Checked = settingBoolean[StateNew, GroupSecurityOptions, IND_ForceWindowsService];
            checkBoxCreateNewDesktop         .Checked = settingBoolean[StateNew, GroupSecurityOptions, IND_CreateNewDesktop];
            checkBoxShowSebApplicationChooser.Checked = settingBoolean[StateNew, GroupSecurityOptions, IND_ShowSebApplicationChooser];
            checkBoxHookMessages             .Checked = settingBoolean[StateNew, GroupSecurityOptions, IND_HookMessages];
            checkBoxEditRegistry             .Checked = settingBoolean[StateNew, GroupSecurityOptions, IND_EditRegistry];
            checkBoxMonitorProcesses         .Checked = settingBoolean[StateNew, GroupSecurityOptions, IND_MonitorProcesses];
            checkBoxShutdownAfterAutostart   .Checked = settingBoolean[StateNew, GroupSecurityOptions, IND_ShutdownAfterAutostart];

            textBoxSebBrowser           .Text = settingString[StateNew, GroupOnlineExam, IND_SebBrowser];
            textBoxAutostartProcess     .Text = settingString[StateNew, GroupOnlineExam, IND_AutostartProcess];
            textBoxExamUrl              .Text = settingString[StateNew, GroupOnlineExam, IND_ExamUrl];
            textBoxPermittedApplications.Text = settingString[StateNew, GroupOnlineExam, IND_PermittedApplications];
          //textBoxQuitPassword         .Text = settingString[StateNew, GroupOnlineExam, IND_QuitPassword];
          //textBoxQuitHashcode         .Text = settingString[StateNew, GroupOnlineExam, IND_QuitHashcode];
        }




        // **************************************************
        // Set the widgets to the new settings of MsgHook.ini
        // **************************************************
        private void SetWidgetsToNewSettingsOfMsgHookIni()
        {
            // Set the widgets to the new values
            textBoxCurrentMsgHookIni   .Text    = settingString [StateNew, GroupMsgHookFiles, IND_CurrentMsgHookIni];
            checkBoxWriteMsgHookLogFile.Checked = settingBoolean[StateNew, GroupMsgHookFiles, IND_WriteMsgHookLogFile];

            checkBoxEnableEsc       .Checked = settingBoolean[StateNew, GroupSpecialKeys, IND_EnableEsc];
            checkBoxEnableCtrlEsc   .Checked = settingBoolean[StateNew, GroupSpecialKeys, IND_EnableCtrlEsc];
            checkBoxEnableAltEsc    .Checked = settingBoolean[StateNew, GroupSpecialKeys, IND_EnableAltEsc];
            checkBoxEnableAltTab    .Checked = settingBoolean[StateNew, GroupSpecialKeys, IND_EnableAltTab];
            checkBoxEnableAltF4     .Checked = settingBoolean[StateNew, GroupSpecialKeys, IND_EnableAltF4];
            checkBoxEnableStartMenu .Checked = settingBoolean[StateNew, GroupSpecialKeys, IND_EnableStartMenu];
            checkBoxEnableRightMouse.Checked = settingBoolean[StateNew, GroupSpecialKeys, IND_EnableRightMouse];

            checkBoxEnableF1 .Checked = settingBoolean[StateNew, GroupFunctionKeys, IND_EnableF1];
            checkBoxEnableF2 .Checked = settingBoolean[StateNew, GroupFunctionKeys, IND_EnableF2];
            checkBoxEnableF3 .Checked = settingBoolean[StateNew, GroupFunctionKeys, IND_EnableF3];
            checkBoxEnableF4 .Checked = settingBoolean[StateNew, GroupFunctionKeys, IND_EnableF4];
            checkBoxEnableF5 .Checked = settingBoolean[StateNew, GroupFunctionKeys, IND_EnableF5];
            checkBoxEnableF6 .Checked = settingBoolean[StateNew, GroupFunctionKeys, IND_EnableF6];
            checkBoxEnableF7 .Checked = settingBoolean[StateNew, GroupFunctionKeys, IND_EnableF7];
            checkBoxEnableF8 .Checked = settingBoolean[StateNew, GroupFunctionKeys, IND_EnableF8];
            checkBoxEnableF9 .Checked = settingBoolean[StateNew, GroupFunctionKeys, IND_EnableF9];
            checkBoxEnableF10.Checked = settingBoolean[StateNew, GroupFunctionKeys, IND_EnableF10];
            checkBoxEnableF11.Checked = settingBoolean[StateNew, GroupFunctionKeys, IND_EnableF11];
            checkBoxEnableF12.Checked = settingBoolean[StateNew, GroupFunctionKeys, IND_EnableF12];

            listBoxExitKey1.SelectedIndex = settingInteger[StateNew, GroupExitSequence, IND_ExitKey1] - 1;
            listBoxExitKey2.SelectedIndex = settingInteger[StateNew, GroupExitSequence, IND_ExitKey2] - 1;
            listBoxExitKey3.SelectedIndex = settingInteger[StateNew, GroupExitSequence, IND_ExitKey3] - 1;

            //textBoxQuitPassword.Text = settingString[StateNew, GroupOnlineExam, IND_QuitPassword];
            //textBoxQuitHashcode.Text = settingString[StateNew, GroupOnlineExam, IND_QuitHashcode];
        }




        // ***********************************
        // Close the configuration application
        // ***********************************
        private void buttonExitAndSave_Click(object sender, EventArgs e)
        {
            // Save both ini files so that nothing gets lost
            String fileName1 = settingString[StateNew, GroupSebStarterFiles, IND_CurrentSebStarterIni];
            String fileName2 = settingString[StateNew, GroupMsgHookFiles   , IND_CurrentMsgHookIni];

            SaveFileSebStarterIni(fileName1);
            SaveFileMsgHookIni   (fileName2);

            // Close the configuration window and exit
            this.Close();
        }

        private void buttonExitWithoutSaving_Click(object sender, EventArgs e)
        {
            // Close the configuration window and exit without saving
            this.Close();
        }

    } // end of   class     SebWindowsConfigForm
}     // end of   namespace SebWindowsConfig
