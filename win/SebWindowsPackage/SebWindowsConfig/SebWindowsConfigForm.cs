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
        // Constants for indexing the ini file settings

        // Maximum number of text lines in ini files
        const int MAX_LINES = 100;

        // The Graphical User Interface contains 5 groups
        const int IND_GroupNon = 0;
        const int IND_GroupMin = 1;
        const int IND_GroupMax = 9;
        const int IND_GroupNum = 9;

        // SebStarter contains the 5 groups
        // SebStarterFiles, InsideSeb, OutsideSeb, SecurityOptions, OnlineExam
        const int IND_GroupNonSebStarter = 0;
        const int IND_GroupMinSebStarter = 1;
        const int IND_GroupMaxSebStarter = 5;
        const int IND_GroupNumSebStarter = 5;

        // MsgHook contains the 4 groups
        // MsgHookFiles, SpecialKeys, FunctionKeys, ExitSequence
        const int IND_GroupNonMsgHook = 0;
        const int IND_GroupMinMsgHook = 6;
        const int IND_GroupMaxMsgHook = 9;
        const int IND_GroupNumMsgHook = 4;

        // Group indices for SebStarter.ini
        const int IND_SebStarterFiles = 1;
        const int IND_InsideSeb       = 2;
        const int IND_OutsideSeb      = 3;
        const int IND_SecurityOptions = 4;
        const int IND_OnlineExam      = 5;

        // Group indices for MsgHook.ini
        const int IND_MsgHookFiles    = 6;
        const int IND_SpecialKeys     = 7;
        const int IND_FunctionKeys    = 8;
        const int IND_ExitSequence    = 9;

        // Each group contains up to 12 settings
        const int IND_SettingNone =  0;
        const int IND_SettingMin  =  1;
        const int IND_SettingMax  = 12;
        const int IND_SettingNum  = 12;

        // Group "SebStarter files"
        const int    IND_WriteLogFileSebStarterLog = 1;
        const String MSG_WriteLogFileSebStarterLog = "WriteLogFileSebStarterLog";

        // Groups "Inside SEB" / "Outside SEB"
        const int IND_EnableSwitchUser        = 1;
        const int IND_EnableLockThisComputer  = 2;
        const int IND_EnableChangeAPassword   = 3;
        const int IND_EnableStartTaskManager  = 4;
        const int IND_EnableLogOff            = 5;
        const int IND_EnableShutDown          = 6;
        const int IND_EnableEaseOfAccess      = 7;
        const int IND_EnableVmWareClientShade = 8;

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
        const int IND_Win9xKillExplorer         = 9;
        const int IND_Win9xScreenSaverRunning   = 10;

        const String MSG_AllowVirtualMachine       = "AllowVirtualMachine";
        const String MSG_ForceWindowsService       = "ForceWindowsService";
        const String MSG_CreateNewDesktop          = "CreateNewDesktop";
        const String MSG_ShowSebApplicationChooser = "ShowSebApplicationChooser";
        const String MSG_HookMessages              = "HookMessages";
        const String MSG_EditRegistry              = "EditRegistry";
        const String MSG_MonitorProcesses          = "MonitorProcesses";
        const String MSG_ShutdownAfterAutostart    = "ShutdownAfterAutostartProcessTerminates";
        const String MSG_Win9xKillExplorer         = "Win9xKillExplorer";
        const String MSG_Win9xScreenSaverRunning   = "Win9xScreenSaverRunning";

        // Group "Online exam"
        const int IND_SebBrowser            = 1;
        const int IND_AutostartProcess      = 2;
        const int IND_ExamUrl               = 3;
        const int IND_PermittedApplications = 4;
        const int IND_QuitPassword          = 5;
        const int IND_QuitHashcode          = 6;

        const String MSG_SebBrowser            = "SebBrowser";
        const String MSG_AutostartProcess      = "AutostartProcess";
        const String MSG_ExamUrl               = "ExamUrl";
        const String MSG_PermittedApplications = "PermittedApplications";
        const String MSG_QuitPassword          = "QuitPassword";
        const String MSG_QuitHashcode          = "QuitHashcode";

        // Group "MsgHook files"
        const int    IND_WriteLogFileMsgHookLog = 1;
        const String MSG_WriteLogFileMsgHookLog = "WriteLogFileMsgHookLog";

        // Group "Special keys"
        const int IND_EnableEsc        = 1;
        const int IND_EnableCtrlEsc    = 2;
        const int IND_EnableAltEsc     = 3;
        const int IND_EnableAltTab     = 4;
        const int IND_EnableAltF4      = 5;
        const int IND_EnableStartMenu  = 6;
        const int IND_EnableRightMouse = 7;

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
        const int IND_B1 = 1;
        const int IND_B2 = 2;
        const int IND_B3 = 3;

        const String MSG_B1 = "B1";
        const String MSG_B2 = "B2";
        const String MSG_B3 = "B3";

        // Types of settings
        const int TYPE_Boolean = 1;
        const int TYPE_Integer = 2;
        const int TYPE_String  = 3;


        // Global variables

        // Text lines of the ini files before and after modification
        int oldNumLinesSebStarterIni = 0;
        int newNumLinesSebStarterIni = 0;
        int tmpNumLinesSebStarterIni = 0;

        int oldNumLinesMsgHookIni = 0;
        int newNumLinesMsgHookIni = 0;
        int tmpNumLinesMsgHookIni = 0;

        static String[] oldLinesSebStarterIni = new String[MAX_LINES + 1];
        static String[] newLinesSebStarterIni = new String[MAX_LINES + 1];
        static String[] tmpLinesSebStarterIni = new String[MAX_LINES + 1];

        static String[] oldLinesMsgHookIni    = new String[MAX_LINES + 1];
        static String[] newLinesMsgHookIni    = new String[MAX_LINES + 1];
        static String[] tmpLinesMsgHookIni    = new String[MAX_LINES + 1];

        // Virtual key code strings
        static String[] virtualKeyCodeString = new String[IND_SettingNum + 1];

        // Names and datatypes of settings (Boolean, Integer, String)
        static String[,] msgString = new String[IND_GroupNum + 1, IND_SettingNum + 1];
        static    int[,] dataType  = new    int[IND_GroupNum + 1, IND_SettingNum + 1];

        // Values of settings as Booleans (true or false)
        static Boolean[,] oldBoolean = new Boolean[IND_GroupNum + 1, IND_SettingNum + 1];
        static Boolean[,] newBoolean = new Boolean[IND_GroupNum + 1, IND_SettingNum + 1];
        static Boolean[,] tmpBoolean = new Boolean[IND_GroupNum + 1, IND_SettingNum + 1];

        // Values of settings as Strings
        static String[,] oldString = new String[IND_GroupNum + 1, IND_SettingNum + 1];
        static String[,] newString = new String[IND_GroupNum + 1, IND_SettingNum + 1];
        static String[,] tmpString = new String[IND_GroupNum + 1, IND_SettingNum + 1];

        // File names
        String oldStringCurrentSebStarterIni = "";
        String newStringCurrentSebStarterIni = "";
        String tmpStringCurrentSebStarterIni = "";

        String oldStringCurrentMsgHookIni = "";
        String newStringCurrentMsgHookIni = "";
        String tmpStringCurrentMsgHookIni = "";

        int oldIndexExitKey1 = 0;
        int oldIndexExitKey2 = 0;
        int oldIndexExitKey3 = 0;

        int newIndexExitKey1 = 0;
        int newIndexExitKey2 = 0;
        int newIndexExitKey3 = 0;

        int tmpIndexExitKey1 = 0;
        int tmpIndexExitKey2 = 0;
        int tmpIndexExitKey3 = 0;


        // Password encryption using the SHA-256 hash algorithm
        SHA256 sha256 = new SHA256Managed();

        FileStream fileStreamSebStarterIni;
        FileStream fileStreamMsgHookIni;

        StreamReader streamReaderSebStarterIni;
        StreamWriter streamWriterSebStarterIni;
        StreamReader streamReaderMsgHookIni;
        StreamWriter streamWriterMsgHookIni;

        DialogResult dialogResultSebStarterIni;
        DialogResult dialogResultMsgHookIni;



        // ***********
        // Constructor
        // ***********
        public SebWindowsConfigForm()
        {
            InitializeComponent();

            // Initialise the global arrays

            int group, setting, lineNr;

            oldNumLinesSebStarterIni = 0;
            newNumLinesSebStarterIni = 0;
            tmpNumLinesSebStarterIni = 0;

            oldNumLinesMsgHookIni = 0;
            newNumLinesMsgHookIni = 0;
            tmpNumLinesMsgHookIni = 0;

            for (lineNr = 0; lineNr <= MAX_LINES; lineNr++)
            {
                oldLinesSebStarterIni[lineNr] = "";
                newLinesSebStarterIni[lineNr] = "";
                tmpLinesSebStarterIni[lineNr] = "";

                oldLinesMsgHookIni[lineNr] = "";
                newLinesMsgHookIni[lineNr] = "";
                tmpLinesMsgHookIni[lineNr] = "";
            }

            for (group   = IND_GroupMin  ; group   <= IND_GroupMax  ; group++)
            for (setting = IND_SettingMin; setting <= IND_SettingMax; setting++)
            {
                oldBoolean[group, setting] = false;
                newBoolean[group, setting] = false;
                tmpBoolean[group, setting] = false;

                oldString[group, setting] = "";
                newString[group, setting] = "";
                tmpString[group, setting] = "";
            }

            // Assign the data types to the different ini settings
            for (setting = IND_SettingMin; setting <= IND_SettingMax; setting++)
            {
                dataType[IND_SebStarterFiles, setting] = TYPE_Boolean;
                dataType[IND_InsideSeb      , setting] = TYPE_Boolean;
                dataType[IND_OutsideSeb     , setting] = TYPE_Boolean;
                dataType[IND_SecurityOptions, setting] = TYPE_Boolean;
                dataType[IND_OnlineExam     , setting] = TYPE_String;

                dataType[IND_MsgHookFiles, setting] = TYPE_Boolean;
                dataType[IND_SpecialKeys , setting] = TYPE_Boolean;
                dataType[IND_FunctionKeys, setting] = TYPE_Boolean;
                dataType[IND_ExitSequence, setting] = TYPE_String;
            }

            msgString[IND_SebStarterFiles, IND_WriteLogFileSebStarterLog] = MSG_WriteLogFileSebStarterLog;

            msgString[IND_InsideSeb, IND_EnableSwitchUser       ] = MSG_InsideSebEnableSwitchUser;
            msgString[IND_InsideSeb, IND_EnableLockThisComputer ] = MSG_InsideSebEnableLockThisComputer;
            msgString[IND_InsideSeb, IND_EnableChangeAPassword  ] = MSG_InsideSebEnableChangeAPassword;
            msgString[IND_InsideSeb, IND_EnableStartTaskManager ] = MSG_InsideSebEnableStartTaskManager;
            msgString[IND_InsideSeb, IND_EnableLogOff           ] = MSG_InsideSebEnableLogOff;
            msgString[IND_InsideSeb, IND_EnableShutDown         ] = MSG_InsideSebEnableShutDown;
            msgString[IND_InsideSeb, IND_EnableEaseOfAccess     ] = MSG_InsideSebEnableEaseOfAccess;
            msgString[IND_InsideSeb, IND_EnableVmWareClientShade] = MSG_InsideSebEnableVmWareClientShade;

            msgString[IND_OutsideSeb, IND_EnableSwitchUser       ] = MSG_OutsideSebEnableSwitchUser;
            msgString[IND_OutsideSeb, IND_EnableLockThisComputer ] = MSG_OutsideSebEnableLockThisComputer;
            msgString[IND_OutsideSeb, IND_EnableChangeAPassword  ] = MSG_OutsideSebEnableChangeAPassword;
            msgString[IND_OutsideSeb, IND_EnableStartTaskManager ] = MSG_OutsideSebEnableStartTaskManager;
            msgString[IND_OutsideSeb, IND_EnableLogOff           ] = MSG_OutsideSebEnableLogOff;
            msgString[IND_OutsideSeb, IND_EnableShutDown         ] = MSG_OutsideSebEnableShutDown;
            msgString[IND_OutsideSeb, IND_EnableEaseOfAccess     ] = MSG_OutsideSebEnableEaseOfAccess;
            msgString[IND_OutsideSeb, IND_EnableVmWareClientShade] = MSG_OutsideSebEnableVmWareClientShade;

            msgString[IND_SecurityOptions, IND_AllowVirtualMachine      ] = MSG_AllowVirtualMachine;
            msgString[IND_SecurityOptions, IND_ForceWindowsService      ] = MSG_ForceWindowsService;
            msgString[IND_SecurityOptions, IND_CreateNewDesktop         ] = MSG_CreateNewDesktop;
            msgString[IND_SecurityOptions, IND_ShowSebApplicationChooser] = MSG_ShowSebApplicationChooser;
            msgString[IND_SecurityOptions, IND_HookMessages             ] = MSG_HookMessages;
            msgString[IND_SecurityOptions, IND_EditRegistry             ] = MSG_EditRegistry;
            msgString[IND_SecurityOptions, IND_MonitorProcesses         ] = MSG_MonitorProcesses;
            msgString[IND_SecurityOptions, IND_ShutdownAfterAutostart   ] = MSG_ShutdownAfterAutostart;

            msgString[IND_OnlineExam, IND_SebBrowser           ] = MSG_SebBrowser;
            msgString[IND_OnlineExam, IND_AutostartProcess     ] = MSG_AutostartProcess;
            msgString[IND_OnlineExam, IND_ExamUrl              ] = MSG_ExamUrl;
            msgString[IND_OnlineExam, IND_PermittedApplications] = MSG_PermittedApplications;
            msgString[IND_OnlineExam, IND_QuitPassword         ] = MSG_QuitPassword;
            msgString[IND_OnlineExam, IND_QuitHashcode         ] = MSG_QuitHashcode;

            msgString[IND_MsgHookFiles, IND_WriteLogFileMsgHookLog] = MSG_WriteLogFileMsgHookLog;

            msgString[IND_SpecialKeys, IND_EnableEsc       ] = MSG_EnableEsc;
            msgString[IND_SpecialKeys, IND_EnableCtrlEsc   ] = MSG_EnableCtrlEsc;
            msgString[IND_SpecialKeys, IND_EnableAltEsc    ] = MSG_EnableAltEsc;
            msgString[IND_SpecialKeys, IND_EnableAltTab    ] = MSG_EnableAltTab;
            msgString[IND_SpecialKeys, IND_EnableAltF4     ] = MSG_EnableAltF4;
            msgString[IND_SpecialKeys, IND_EnableStartMenu ] = MSG_EnableStartMenu;
            msgString[IND_SpecialKeys, IND_EnableRightMouse] = MSG_EnableRightMouse;

            msgString[IND_FunctionKeys, IND_EnableF1 ] = MSG_EnableF1;
            msgString[IND_FunctionKeys, IND_EnableF2 ] = MSG_EnableF2;
            msgString[IND_FunctionKeys, IND_EnableF3 ] = MSG_EnableF3;
            msgString[IND_FunctionKeys, IND_EnableF4 ] = MSG_EnableF4;
            msgString[IND_FunctionKeys, IND_EnableF5 ] = MSG_EnableF5;
            msgString[IND_FunctionKeys, IND_EnableF6 ] = MSG_EnableF6;
            msgString[IND_FunctionKeys, IND_EnableF7 ] = MSG_EnableF7;
            msgString[IND_FunctionKeys, IND_EnableF8 ] = MSG_EnableF8;
            msgString[IND_FunctionKeys, IND_EnableF9 ] = MSG_EnableF9;
            msgString[IND_FunctionKeys, IND_EnableF10] = MSG_EnableF10;
            msgString[IND_FunctionKeys, IND_EnableF11] = MSG_EnableF11;
            msgString[IND_FunctionKeys, IND_EnableF12] = MSG_EnableF12;

            msgString[IND_ExitSequence, IND_B1] = MSG_B1;
            msgString[IND_ExitSequence, IND_B2] = MSG_B2;
            msgString[IND_ExitSequence, IND_B3] = MSG_B3;

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

            openFileDialogSebStarterIni.InitialDirectory = System.Environment.CurrentDirectory;
            saveFileDialogSebStarterIni.InitialDirectory = System.Environment.CurrentDirectory;
            openFileDialogMsgHookIni   .InitialDirectory = System.Environment.CurrentDirectory;
            saveFileDialogMsgHookIni   .InitialDirectory = System.Environment.CurrentDirectory;

        } // end of contructor   SebWindowsConfigForm()




        // ******************************
        // Open file SebStarter.ini click
        // ******************************
        private void labelOpenFileSebStarterIni_Click(object sender, EventArgs e)
        {
                dialogResultSebStarterIni = openFileDialogSebStarterIni.ShowDialog();
            tmpStringCurrentSebStarterIni = openFileDialogSebStarterIni.FileName;

            OpenFileSebStarterIni();

        } // end of method   labelOpenFileSebStarterIni_Click()



        // ************************
        // Open file SebStarter.ini
        // ************************
        private void OpenFileSebStarterIni()
        {
            int   group, setting, lineNr;
            int   groupMin = IND_GroupMinSebStarter;
            int   groupMax = IND_GroupMaxSebStarter;
            int settingMin = IND_SettingMin;
            int settingMax = IND_SettingMax;

            try 
            {
                // Open the SebStarter.ini file for reading
                  fileStreamSebStarterIni = new   FileStream(tmpStringCurrentSebStarterIni, FileMode.Open, FileAccess.Read);
                streamReaderSebStarterIni = new StreamReader(      fileStreamSebStarterIni);
                String line;

                // Read lines from the SebStarter.ini file until end of file is reached
                tmpNumLinesSebStarterIni = 0;

                while ((line = streamReaderSebStarterIni.ReadLine()) != null) 
                {
                    tmpNumLinesSebStarterIni++;
                    tmpLinesSebStarterIni[tmpNumLinesSebStarterIni] = line;

                    // Skip empty lines and lines not in "leftSide = rightSide" format
                    if (line.Contains("="))
                    {
                        int      equalPosition = line.IndexOf  ("=");
                        String   leftString    = line.Remove   (equalPosition);
                        String  rightString    = line.Substring(equalPosition + 1);
                        Boolean rightBoolean   = rightString.Equals("1");

                        // Find the appropriate group and setting
                        for (group   =   groupMin;   group <=   groupMax;   group++)
                        for (setting = settingMin; setting <= settingMax; setting++)
                        {
                            if (leftString.Equals(msgString[group, setting]))
                            {
                                tmpBoolean[group, setting] = rightBoolean;
                                tmpString [group, setting] = rightString;
                            }
                        }

                    } // end if line.Contains("=")
                } // end while

                // Close the SebStarter.ini file
                streamReaderSebStarterIni.Close();
                  fileStreamSebStarterIni.Close();

            } // end try

            catch (Exception streamReadException) 
            {
                // Let the user know what went wrong.
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(streamReadException.Message);
                return;
            }


            // Accept the tmp values as the new values
            oldNumLinesSebStarterIni = tmpNumLinesSebStarterIni;
            newNumLinesSebStarterIni = tmpNumLinesSebStarterIni;

            for (lineNr = 0; lineNr <= MAX_LINES; lineNr++)
            {
                oldLinesSebStarterIni[lineNr] = tmpLinesSebStarterIni[lineNr];
                newLinesSebStarterIni[lineNr] = tmpLinesSebStarterIni[lineNr];
            }

            for (group   =   groupMin;   group <=   groupMax;   group++)
            for (setting = settingMin; setting <= settingMax; setting++)
            {
                oldBoolean[group, setting] = tmpBoolean[group, setting];
                newBoolean[group, setting] = tmpBoolean[group, setting];
                oldString [group, setting] = tmpString [group, setting];
                newString [group, setting] = tmpString [group, setting];
            }

            oldStringCurrentSebStarterIni = tmpStringCurrentSebStarterIni;
            newStringCurrentSebStarterIni = tmpStringCurrentSebStarterIni;

            // Assign the settings from the SebStarter.ini file to the widgets
            SetWidgetsToNewSettingsOfSebStarterIni();

        } // end of method   OpenFileSebStarterIni()




        // ******************************
        // Save file SebStarter.ini click
        // ******************************
        private void labelSaveFileSebStarterIni_Click(object sender, EventArgs e)
        {
                dialogResultSebStarterIni = saveFileDialogSebStarterIni.ShowDialog();
            tmpStringCurrentSebStarterIni = saveFileDialogSebStarterIni.FileName;

            SaveFileSebStarterIni();

        } // end of method   labelSaveFileSebStarterIni_Click()



        // ************************
        // Save file SebStarter.ini
        // ************************
        private void SaveFileSebStarterIni()
        {
            int   group, setting, lineNr;
            int   groupMin = IND_GroupMinSebStarter;
            int   groupMax = IND_GroupMaxSebStarter;
            int settingMin = IND_SettingMin;
            int settingMax = IND_SettingMax;

            try 
            {
                // Open the SebStarter.ini file for writing
                  fileStreamSebStarterIni = new   FileStream(tmpStringCurrentSebStarterIni, FileMode.OpenOrCreate, FileAccess.Write);
                streamWriterSebStarterIni = new StreamWriter(      fileStreamSebStarterIni);
                String line;

                // Write lines into the SebStarter.ini file until end of file is reached
                for (lineNr = 1; lineNr <= newNumLinesSebStarterIni; lineNr++)
                {
                    line = oldLinesSebStarterIni[lineNr];

                    // Skip empty lines and lines not in "leftSide = rightSide" format
                    if (line.Contains("="))
                    {
                        int      equalPosition = line.IndexOf  ("=");
                        String   leftString    = line.Remove   (equalPosition);
                        String  rightString    = line.Substring(equalPosition + 1);
                        Boolean rightBoolean   = rightString.Equals("1");
                        int     rightType      = TYPE_Boolean;

                        // Find the appropriate group and setting
                        for (group   =   groupMin;   group <=   groupMax;   group++)
                        for (setting = settingMin; setting <= settingMax; setting++)
                        {
                            if (leftString.Equals(msgString[group, setting]))
                            {
                                rightBoolean = newBoolean[group, setting];
                                rightString  = newString [group, setting];
                                rightType    =   dataType[group, setting];
                                if ((rightType == TYPE_Boolean) && (rightBoolean == false)) rightString = "0";
                                if ((rightType == TYPE_Boolean) && (rightBoolean ==  true)) rightString = "1";
                            }
                        }

                        // Concatenate the modified line
                        line = "";
                        line = leftString + "=" + rightString;

                    } // end if line.Contains("=")

                    // Write the modified line back into the file
                        tmpLinesSebStarterIni[lineNr] = line;
                    streamWriterSebStarterIni.WriteLine(line);

                } // next lineNr

                // Close the SebStarter.ini file
                streamWriterSebStarterIni.Close();
                  fileStreamSebStarterIni.Close();

            } // end try

            catch (Exception streamWriteException) 
            {
                // Let the user know what went wrong.
                Console.WriteLine("The file could not be written:");
                Console.WriteLine(streamWriteException.Message);
                return;
            }


            // Accept the tmp values as the new values
            oldStringCurrentSebStarterIni = tmpStringCurrentSebStarterIni;
            newStringCurrentSebStarterIni = tmpStringCurrentSebStarterIni;

            oldNumLinesSebStarterIni = newNumLinesSebStarterIni;
            for (lineNr = 0; lineNr <= MAX_LINES; lineNr++)
            {
                oldLinesSebStarterIni[lineNr] = tmpLinesSebStarterIni[lineNr];
                newLinesSebStarterIni[lineNr] = tmpLinesSebStarterIni[lineNr];
            }

            for (group   =   groupMin;   group <=   groupMax;   group++)
            for (setting = settingMin; setting <= settingMax; setting++)
            {
                oldBoolean[group, setting] = newBoolean[group, setting];
                oldString [group, setting] = newString [group, setting];
            }

            oldStringCurrentSebStarterIni    = newStringCurrentSebStarterIni;
            textBoxCurrentSebStarterIni.Text = newStringCurrentSebStarterIni;

        } // end of method   SaveFileSebStarterIni()




        // ***************************
        // Open file MsgHook.ini click
        // ***************************
        private void labelOpenFileMsgHookIni_Click(object sender, EventArgs e)
        {
                dialogResultMsgHookIni = openFileDialogMsgHookIni.ShowDialog();
            tmpStringCurrentMsgHookIni = openFileDialogMsgHookIni.FileName;

            OpenFileMsgHookIni();

        }  // end of method   labelOpenFileMsgHookIni_Click()



        // *********************
        // Open file MsgHook.ini
        // *********************
        private void OpenFileMsgHookIni()
        {
            int   group, setting, lineNr;
            int   groupMin = IND_GroupMinMsgHook;
            int   groupMax = IND_GroupMaxMsgHook;
            int settingMin = IND_SettingMin;
            int settingMax = IND_SettingMax;

            try 
            {
                // Open the MsgHook.ini file for reading
                  fileStreamMsgHookIni = new   FileStream(tmpStringCurrentMsgHookIni, FileMode.Open, FileAccess.Read);
                streamReaderMsgHookIni = new StreamReader(      fileStreamMsgHookIni);
                String line;

                // Read lines from the SebStarter.ini file until end of file is reached
                tmpNumLinesMsgHookIni = 0;

                while ((line = streamReaderMsgHookIni.ReadLine()) != null)
                {
                    tmpNumLinesMsgHookIni++;
                    tmpLinesMsgHookIni[tmpNumLinesMsgHookIni] = line;

                    // Skip empty lines and lines not in "leftSide = rightSide" format
                    if (line.Contains("="))
                    {
                        int      equalPosition = line.IndexOf  ("=");
                        String   leftString    = line.Remove   (equalPosition);
                        String  rightString    = line.Substring(equalPosition + 1);
                        Boolean rightBoolean   = rightString.Equals("1");

                        // Find the appropriate group and setting
                        for (group   =   groupMin;   group <=   groupMax;   group++)
                        for (setting = settingMin; setting <= settingMax; setting++)
                        {
                            if (leftString.Equals(msgString[group, setting]))
                            {
                                tmpBoolean[group, setting] = rightBoolean;
                                tmpString [group, setting] = rightString;
                            }
                        }

                    } // end if line.Contains("=")
                } // end while

                // Close the MsgHook.ini file
                streamReaderMsgHookIni.Close();
                  fileStreamMsgHookIni.Close();

            } // end try

            catch (Exception streamReadException) 
            {
                // Let the user know what went wrong.
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(streamReadException.Message);
                return;
            }


            // Accept the tmp values as the new values
            oldNumLinesMsgHookIni = tmpNumLinesMsgHookIni;
            newNumLinesMsgHookIni = tmpNumLinesMsgHookIni;

            for (lineNr = 0; lineNr <= MAX_LINES; lineNr++)
            {
                oldLinesMsgHookIni[lineNr] = tmpLinesMsgHookIni[lineNr];
                newLinesMsgHookIni[lineNr] = tmpLinesMsgHookIni[lineNr];
            }

            for (group   =   groupMin;   group <=   groupMax;   group++)
            for (setting = settingMin; setting <= settingMax; setting++)
            {
                oldBoolean[group, setting] = tmpBoolean[group, setting];
                newBoolean[group, setting] = tmpBoolean[group, setting];
                oldString [group, setting] = tmpString [group, setting];
                newString [group, setting] = tmpString [group, setting];
            }

            oldStringCurrentMsgHookIni = tmpStringCurrentMsgHookIni;
            newStringCurrentMsgHookIni = tmpStringCurrentMsgHookIni;

            // Assign the settings from the MsgHook.ini file to the widgets
            SetWidgetsToNewSettingsOfMsgHookIni();

        }  // end of method   OpenFileMsgHookIni()




        // ***************************
        // Save file MsgHook.ini click
        // ***************************
        private void labelSaveFileMsgHookIni_Click(object sender, EventArgs e)
        {
                dialogResultMsgHookIni = saveFileDialogMsgHookIni.ShowDialog();
            tmpStringCurrentMsgHookIni = saveFileDialogMsgHookIni.FileName;

            SaveFileMsgHookIni();

        }  // end of method   labelSaveFileMsgHookIni_Click()



        // *********************
        // Save file MsgHook.ini
        // *********************
        private void SaveFileMsgHookIni()
        {
            int   group, setting, lineNr;
            int   groupMin = IND_GroupMinMsgHook;
            int   groupMax = IND_GroupMaxMsgHook;
            int settingMin = IND_SettingMin;
            int settingMax = IND_SettingMax;

            newString[IND_ExitSequence, IND_B1] = virtualKeyCodeString[newIndexExitKey1];
            newString[IND_ExitSequence, IND_B2] = virtualKeyCodeString[newIndexExitKey2];
            newString[IND_ExitSequence, IND_B3] = virtualKeyCodeString[newIndexExitKey3];

            try 
            {
                // Open the MsgHook.ini file for writing
                  fileStreamMsgHookIni = new   FileStream(tmpStringCurrentMsgHookIni, FileMode.OpenOrCreate, FileAccess.Write);
                streamWriterMsgHookIni = new StreamWriter(      fileStreamMsgHookIni);
                String line;

                // Write lines into the MsgHook.ini file until end of file is reached
                for (lineNr = 1; lineNr <= newNumLinesMsgHookIni; lineNr++)
                {
                    line = oldLinesMsgHookIni[lineNr];

                    // Skip empty lines and lines not in "leftSide = rightSide" format
                    if (line.Contains("="))
                    {
                        int      equalPosition = line.IndexOf  ("=");
                        String   leftString    = line.Remove   (equalPosition);
                        String  rightString    = line.Substring(equalPosition + 1);
                        Boolean rightBoolean   = rightString.Equals("1");
                        int     rightType      = TYPE_Boolean;

                        // Find the appropriate group and setting
                        for (group   =   groupMin;   group <=   groupMax;   group++)
                        for (setting = settingMin; setting <= settingMax; setting++)
                        {
                            if (leftString.Equals(msgString[group, setting]))
                            {
                                rightBoolean = newBoolean[group, setting];
                                rightString  = newString [group, setting];
                                rightType    =   dataType[group, setting];
                                if ((rightType == TYPE_Boolean) && (rightBoolean == false)) rightString = "0";
                                if ((rightType == TYPE_Boolean) && (rightBoolean ==  true)) rightString = "1";
                            }
                        }

                        // Concatenate the modified line
                        line = "";
                        line = leftString + "=" + rightString;

                    } // end if line.Contains("=")

                    // Write the modified line back into the file
                        tmpLinesMsgHookIni[lineNr] = line;
                    streamWriterMsgHookIni.WriteLine(line);

                } // next lineNr

                // Close the MsgHook.ini file
                streamWriterMsgHookIni.Close();
                  fileStreamMsgHookIni.Close();

            } // end try

            catch (Exception streamWriteException) 
            {
                // Let the user know what went wrong.
                Console.WriteLine("The file could not be written:");
                Console.WriteLine(streamWriteException.Message);
                return;
            }


            // Accept the tmp values as the new values
            oldStringCurrentMsgHookIni = tmpStringCurrentMsgHookIni;
            newStringCurrentMsgHookIni = tmpStringCurrentMsgHookIni;

            oldNumLinesMsgHookIni = newNumLinesMsgHookIni;
            for (lineNr = 0; lineNr <= MAX_LINES; lineNr++)
            {
                oldLinesMsgHookIni[lineNr] = tmpLinesMsgHookIni[lineNr];
                newLinesMsgHookIni[lineNr] = tmpLinesMsgHookIni[lineNr];
            }

            for (group   =   groupMin;   group <=   groupMax;   group++)
            for (setting = settingMin; setting <= settingMax; setting++)
            {
                oldBoolean[group, setting] = newBoolean[group, setting];
                oldString [group, setting] = newString [group, setting];
            }

            oldIndexExitKey1 = newIndexExitKey1;
            oldIndexExitKey2 = newIndexExitKey2;
            oldIndexExitKey3 = newIndexExitKey3;

            oldStringCurrentMsgHookIni    = newStringCurrentMsgHookIni;
            textBoxCurrentMsgHookIni.Text = newStringCurrentMsgHookIni;

        }  // end of method   SaveFileMsgHookIni()




        // ******************************************************
        // Event handlers:
        // If the user changes a setting by clicking or typing,
        // update the setting in memory for later saving on file.
        // ******************************************************

        // Group "SebStarter files"
        private void checkBoxWriteLogFileSebStarterLog_CheckedChanged(object sender, EventArgs e)
        {
            newBoolean[IND_SebStarterFiles, IND_WriteLogFileSebStarterLog] = checkBoxWriteLogFileSebStarterLog.Checked;
        }


        // Group "Inside SEB"
        private void checkBoxInsideSebEnableSwitchUser_CheckedChanged(object sender, EventArgs e)
        {
            newBoolean[IND_InsideSeb, IND_EnableSwitchUser] = checkBoxInsideSebEnableSwitchUser.Checked;
        }

        private void checkBoxInsideSebEnableLockThisComputer_CheckedChanged(object sender, EventArgs e)
        {
            newBoolean[IND_InsideSeb, IND_EnableLockThisComputer] = checkBoxInsideSebEnableLockThisComputer.Checked;
        }

        private void checkBoxInsideSebEnableChangeAPassword_CheckedChanged(object sender, EventArgs e)
        {
            newBoolean[IND_InsideSeb, IND_EnableChangeAPassword] = checkBoxInsideSebEnableChangeAPassword.Checked;
        }

        private void checkBoxInsideSebEnableStartTaskManager_CheckedChanged(object sender, EventArgs e)
        {
            newBoolean[IND_InsideSeb, IND_EnableStartTaskManager] = checkBoxInsideSebEnableStartTaskManager.Checked;
        }

        private void checkBoxInsideSebEnableLogOff_CheckedChanged(object sender, EventArgs e)
        {
            newBoolean[IND_InsideSeb, IND_EnableLogOff] = checkBoxInsideSebEnableLogOff.Checked;
        }

        private void checkBoxInsideSebEnableShutDown_CheckedChanged(object sender, EventArgs e)
        {
            newBoolean[IND_InsideSeb, IND_EnableShutDown] = checkBoxInsideSebEnableShutDown.Checked;
        }

        private void checkBoxInsideSebEnableEaseOfAccess_CheckedChanged(object sender, EventArgs e)
        {
            newBoolean[IND_InsideSeb, IND_EnableEaseOfAccess] = checkBoxInsideSebEnableEaseOfAccess.Checked;
        }

        private void checkBoxInsideSebEnableVmWareClientShade_CheckedChanged(object sender, EventArgs e)
        {
            newBoolean[IND_InsideSeb, IND_EnableVmWareClientShade] = checkBoxInsideSebEnableVmWareClientShade.Checked;
        }
 

        // Group "Outside SEB"
        private void checkBoxOutsideSebEnableSwitchUser_CheckedChanged(object sender, EventArgs e)
        {
            newBoolean[IND_OutsideSeb, IND_EnableSwitchUser] = checkBoxOutsideSebEnableSwitchUser.Checked;
        }

        private void checkBoxOutsideSebEnableLockThisComputer_CheckedChanged(object sender, EventArgs e)
        {
            newBoolean[IND_OutsideSeb, IND_EnableLockThisComputer] = checkBoxOutsideSebEnableLockThisComputer.Checked;
        }

        private void checkBoxOutsideSebEnableChangeAPassword_CheckedChanged(object sender, EventArgs e)
        {
            newBoolean[IND_OutsideSeb, IND_EnableChangeAPassword] = checkBoxOutsideSebEnableChangeAPassword.Checked;
        }

        private void checkBoxOutsideSebEnableStartTaskManager_CheckedChanged(object sender, EventArgs e)
        {
            newBoolean[IND_OutsideSeb, IND_EnableStartTaskManager] = checkBoxOutsideSebEnableStartTaskManager.Checked;
        }

        private void checkBoxOutsideSebEnableLogOff_CheckedChanged(object sender, EventArgs e)
        {
            newBoolean[IND_OutsideSeb, IND_EnableLogOff] = checkBoxOutsideSebEnableLogOff.Checked;
        }

        private void checkBoxOutsideSebEnableShutDown_CheckedChanged(object sender, EventArgs e)
        {
            newBoolean[IND_OutsideSeb, IND_EnableShutDown] = checkBoxOutsideSebEnableShutDown.Checked;
        }

        private void checkBoxOutsideSebEnableEaseOfAccess_CheckedChanged(object sender, EventArgs e)
        {
            newBoolean[IND_OutsideSeb, IND_EnableEaseOfAccess] = checkBoxOutsideSebEnableEaseOfAccess.Checked;
        }

        private void checkBoxOutsideSebEnableVmWareClientShade_CheckedChanged(object sender, EventArgs e)
        {
            newBoolean[IND_OutsideSeb, IND_EnableVmWareClientShade] = checkBoxOutsideSebEnableVmWareClientShade.Checked;
        }


        // Group "Security options"
        private void checkBoxAllowVirtualMachine_CheckedChanged(object sender, EventArgs e)
        {
            newBoolean[IND_SecurityOptions, IND_AllowVirtualMachine] = checkBoxAllowVirtualMachine.Checked;
        }

        private void checkBoxForceWindowsService_CheckedChanged(object sender, EventArgs e)
        {
            newBoolean[IND_SecurityOptions, IND_ForceWindowsService] = checkBoxForceWindowsService.Checked;
        }

        private void checkBoxCreateNewDesktop_CheckedChanged(object sender, EventArgs e)
        {
            newBoolean[IND_SecurityOptions, IND_CreateNewDesktop] = checkBoxCreateNewDesktop.Checked;
        }

        private void checkBoxShowSebApplicationChooser_CheckedChanged(object sender, EventArgs e)
        {
            newBoolean[IND_SecurityOptions, IND_ShowSebApplicationChooser] = checkBoxShowSebApplicationChooser.Checked;
        }

        private void checkBoxHookMessages_CheckedChanged(object sender, EventArgs e)
        {
            newBoolean[IND_SecurityOptions, IND_HookMessages] = checkBoxHookMessages.Checked;
        }

        private void checkBoxEditRegistry_CheckedChanged(object sender, EventArgs e)
        {
            newBoolean[IND_SecurityOptions, IND_EditRegistry] = checkBoxEditRegistry.Checked;
        }

        private void checkBoxMonitorProcesses_CheckedChanged(object sender, EventArgs e)
        {
            newBoolean[IND_SecurityOptions, IND_MonitorProcesses] = checkBoxMonitorProcesses.Checked;
        }

        private void checkBoxShutdownAfterAutostart_CheckedChanged(object sender, EventArgs e)
        {
            newBoolean[IND_SecurityOptions, IND_ShutdownAfterAutostart] = checkBoxShutdownAfterAutostart.Checked;
        }


        // Group "Online exam"
        private void textBoxSebBrowser_TextChanged(object sender, EventArgs e)
        {
            newString[IND_OnlineExam, IND_SebBrowser] = textBoxSebBrowser.Text;
        }

        private void textBoxAutostartProcess_TextChanged(object sender, EventArgs e)
        {
            newString[IND_OnlineExam, IND_AutostartProcess] = textBoxAutostartProcess.Text;
        }

        private void textBoxExamUrl_TextChanged(object sender, EventArgs e)
        {
            newString[IND_OnlineExam, IND_ExamUrl] = textBoxExamUrl.Text;
        }

        private void textBoxPermittedApplications_TextChanged(object sender, EventArgs e)
        {
            newString[IND_OnlineExam, IND_PermittedApplications] = textBoxPermittedApplications.Text;
        }


        // Group "MsgHook files"
        private void checkBoxWriteLogFileMsgHookLog_CheckedChanged(object sender, EventArgs e)
        {
            newBoolean[IND_MsgHookFiles, IND_WriteLogFileMsgHookLog] = checkBoxWriteLogFileMsgHookLog.Checked;
        }


        // Group "Special keys"
        private void checkBoxEnableEsc_CheckedChanged(object sender, EventArgs e)
        {
            newBoolean[IND_SpecialKeys, IND_EnableEsc] = checkBoxEnableEsc.Checked;
        }

        private void checkBoxEnableCtrlEsc_CheckedChanged(object sender, EventArgs e)
        {
            newBoolean[IND_SpecialKeys, IND_EnableCtrlEsc] = checkBoxEnableCtrlEsc.Checked;
        }

        private void checkBoxEnableAltEsc_CheckedChanged(object sender, EventArgs e)
        {
            newBoolean[IND_SpecialKeys, IND_EnableAltEsc] = checkBoxEnableAltEsc.Checked;
        }

        private void checkBoxEnableAltTab_CheckedChanged(object sender, EventArgs e)
        {
            newBoolean[IND_SpecialKeys, IND_EnableAltTab] = checkBoxEnableAltTab.Checked;
        }

        private void checkBoxEnableAltF4_CheckedChanged(object sender, EventArgs e)
        {
            newBoolean[IND_SpecialKeys, IND_EnableAltF4] = checkBoxEnableAltF4.Checked;
        }

        private void checkBoxEnableStartMenu_CheckedChanged(object sender, EventArgs e)
        {
            newBoolean[IND_SpecialKeys, IND_EnableStartMenu] = checkBoxEnableStartMenu.Checked;
        }

        private void checkBoxEnableRightMouse_CheckedChanged(object sender, EventArgs e)
        {
            newBoolean[IND_SpecialKeys, IND_EnableRightMouse] = checkBoxEnableRightMouse.Checked;
        }


        // Group "Function keys"
        private void checkBoxEnableF1_CheckedChanged(object sender, EventArgs e)
        {
            newBoolean[IND_FunctionKeys, IND_EnableF1] = checkBoxEnableF1.Checked;
        }

        private void checkBoxEnableF2_CheckedChanged(object sender, EventArgs e)
        {
            newBoolean[IND_FunctionKeys, IND_EnableF2] = checkBoxEnableF2.Checked;
        }

        private void checkBoxEnableF3_CheckedChanged(object sender, EventArgs e)
        {
            newBoolean[IND_FunctionKeys, IND_EnableF3] = checkBoxEnableF3.Checked;
        }

        private void checkBoxEnableF4_CheckedChanged(object sender, EventArgs e)
        {
            newBoolean[IND_FunctionKeys, IND_EnableF4] = checkBoxEnableF4.Checked;
        }

        private void checkBoxEnableF5_CheckedChanged(object sender, EventArgs e)
        {
            newBoolean[IND_FunctionKeys, IND_EnableF5] = checkBoxEnableF5.Checked;
        }

        private void checkBoxEnableF6_CheckedChanged(object sender, EventArgs e)
        {
            newBoolean[IND_FunctionKeys, IND_EnableF6] = checkBoxEnableF6.Checked;
        }

        private void checkBoxEnableF7_CheckedChanged(object sender, EventArgs e)
        {
            newBoolean[IND_FunctionKeys, IND_EnableF7] = checkBoxEnableF7.Checked;
        }

        private void checkBoxEnableF8_CheckedChanged(object sender, EventArgs e)
        {
            newBoolean[IND_FunctionKeys, IND_EnableF8] = checkBoxEnableF8.Checked;
        }

        private void checkBoxEnableF9_CheckedChanged(object sender, EventArgs e)
        {
            newBoolean[IND_FunctionKeys, IND_EnableF9] = checkBoxEnableF9.Checked;
        }

        private void checkBoxEnableF10_CheckedChanged(object sender, EventArgs e)
        {
            newBoolean[IND_FunctionKeys, IND_EnableF10] = checkBoxEnableF10.Checked;
        }

        private void checkBoxEnableF11_CheckedChanged(object sender, EventArgs e)
        {
            newBoolean[IND_FunctionKeys, IND_EnableF11] = checkBoxEnableF11.Checked;
        }

        private void checkBoxEnableF12_CheckedChanged(object sender, EventArgs e)
        {
            newBoolean[IND_FunctionKeys, IND_EnableF12] = checkBoxEnableF12.Checked;
        }


        // Group "Exit sequence"
        private void listBoxExitKeyFirst_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Make sure that all three exit keys are different.
            // If selected key is already occupied, revert to previously selected key.
            tmpIndexExitKey1 = listBoxExitKey1.SelectedIndex + 1;

            if ((tmpIndexExitKey1 == newIndexExitKey2) ||
                (tmpIndexExitKey1 == newIndexExitKey3))
                  listBoxExitKey1.SelectedIndex = newIndexExitKey1 - 1;
            else newIndexExitKey1 = tmpIndexExitKey1;
        }

        private void listBoxExitKeySecond_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Make sure that all three exit keys are different.
            // If selected key is already occupied, revert to previously selected key.
            tmpIndexExitKey2 = listBoxExitKey2.SelectedIndex + 1;

            if ((tmpIndexExitKey2 == newIndexExitKey1) ||
                (tmpIndexExitKey2 == newIndexExitKey3))
                  listBoxExitKey2.SelectedIndex = newIndexExitKey2 - 1;
            else newIndexExitKey2 = tmpIndexExitKey2;
        }

        private void listBoxExitKeyThird_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Make sure that all three exit keys are different.
            // If selected key is already occupied, revert to previously selected key.
            tmpIndexExitKey3 = listBoxExitKey3.SelectedIndex + 1;

            if ((tmpIndexExitKey3 == newIndexExitKey1) ||
                (tmpIndexExitKey3 == newIndexExitKey2))
                  listBoxExitKey3.SelectedIndex = newIndexExitKey3 - 1;
            else newIndexExitKey3 = tmpIndexExitKey3;
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

            newString[IND_OnlineExam, IND_QuitPassword] = newStringQuitPassword;
            newString[IND_OnlineExam, IND_QuitHashcode] = newStringQuitHashcode;
        }




        // ***************************************
        // Restore settings of file SebStarter.ini
        // ***************************************
        private void buttonRestoreSettingsOfSebStarterIni_Click(object sender, EventArgs e)
        {
            int   group, setting;
            int   groupMin = IND_GroupMinSebStarter;
            int   groupMax = IND_GroupMaxSebStarter;
            int settingMin = IND_SettingMin;
            int settingMax = IND_SettingMax;

            // Restore the old settings by copying them to the new settings
            for (group   =   groupMin; group   <=   groupMax;   group++)
            for (setting = settingMin; setting <= settingMax; setting++)
            {
                newBoolean[group, setting] = oldBoolean[group, setting];
                newString [group, setting] = oldString [group, setting];
            }

            newStringCurrentSebStarterIni = oldStringCurrentSebStarterIni;
            SetWidgetsToNewSettingsOfSebStarterIni();
        }




        // ************************************
        // Restore settings of file MsgHook.ini
        // ************************************
        private void buttonRestoreSettingsOfMsgHookIni_Click(object sender, EventArgs e)
        {
            int   group, setting;
            int   groupMin = IND_GroupMinMsgHook;
            int   groupMax = IND_GroupMaxMsgHook;
            int settingMin = IND_SettingMin;
            int settingMax = IND_SettingMax;

            // Restore the old settings by copying them to the new settings
            for (group   =   groupMin; group   <=   groupMax;   group++)
            for (setting = settingMin; setting <= settingMax; setting++)
            {
                newBoolean[group, setting] = oldBoolean[group, setting];
                newString [group, setting] = oldString [group, setting];
            }

            newStringCurrentMsgHookIni = oldStringCurrentMsgHookIni;
            SetWidgetsToNewSettingsOfMsgHookIni();
        }




        // *****************************************************
        // Set the widgets to the new settings of SebStarter.ini
        // *****************************************************
        private void SetWidgetsToNewSettingsOfSebStarterIni()
        {
            // Set the widgets to the new settings
                  textBoxCurrentSebStarterIni.Text    = newStringCurrentSebStarterIni;
            checkBoxWriteLogFileSebStarterLog.Checked = newBoolean[IND_SebStarterFiles, IND_WriteLogFileSebStarterLog];

            checkBoxInsideSebEnableSwitchUser       .Checked = newBoolean[IND_InsideSeb, IND_EnableSwitchUser];
            checkBoxInsideSebEnableLockThisComputer .Checked = newBoolean[IND_InsideSeb, IND_EnableLockThisComputer];
            checkBoxInsideSebEnableChangeAPassword  .Checked = newBoolean[IND_InsideSeb, IND_EnableChangeAPassword];
            checkBoxInsideSebEnableStartTaskManager .Checked = newBoolean[IND_InsideSeb, IND_EnableStartTaskManager];
            checkBoxInsideSebEnableLogOff           .Checked = newBoolean[IND_InsideSeb, IND_EnableLogOff];
            checkBoxInsideSebEnableShutDown         .Checked = newBoolean[IND_InsideSeb, IND_EnableShutDown];
            checkBoxInsideSebEnableEaseOfAccess     .Checked = newBoolean[IND_InsideSeb, IND_EnableEaseOfAccess];
            checkBoxInsideSebEnableVmWareClientShade.Checked = newBoolean[IND_InsideSeb, IND_EnableVmWareClientShade];

            checkBoxOutsideSebEnableSwitchUser       .Checked = newBoolean[IND_OutsideSeb, IND_EnableSwitchUser];
            checkBoxOutsideSebEnableLockThisComputer .Checked = newBoolean[IND_OutsideSeb, IND_EnableLockThisComputer];
            checkBoxOutsideSebEnableChangeAPassword  .Checked = newBoolean[IND_OutsideSeb, IND_EnableChangeAPassword];
            checkBoxOutsideSebEnableStartTaskManager .Checked = newBoolean[IND_OutsideSeb, IND_EnableStartTaskManager];
            checkBoxOutsideSebEnableLogOff           .Checked = newBoolean[IND_OutsideSeb, IND_EnableLogOff];
            checkBoxOutsideSebEnableShutDown         .Checked = newBoolean[IND_OutsideSeb, IND_EnableShutDown];
            checkBoxOutsideSebEnableEaseOfAccess     .Checked = newBoolean[IND_OutsideSeb, IND_EnableEaseOfAccess];
            checkBoxOutsideSebEnableVmWareClientShade.Checked = newBoolean[IND_OutsideSeb, IND_EnableVmWareClientShade];

            checkBoxAllowVirtualMachine      .Checked = newBoolean[IND_SecurityOptions, IND_AllowVirtualMachine];
            checkBoxForceWindowsService      .Checked = newBoolean[IND_SecurityOptions, IND_ForceWindowsService];
            checkBoxCreateNewDesktop         .Checked = newBoolean[IND_SecurityOptions, IND_CreateNewDesktop];
            checkBoxShowSebApplicationChooser.Checked = newBoolean[IND_SecurityOptions, IND_ShowSebApplicationChooser];
            checkBoxHookMessages             .Checked = newBoolean[IND_SecurityOptions, IND_HookMessages];
            checkBoxEditRegistry             .Checked = newBoolean[IND_SecurityOptions, IND_EditRegistry];
            checkBoxMonitorProcesses         .Checked = newBoolean[IND_SecurityOptions, IND_MonitorProcesses];
            checkBoxShutdownAfterAutostart   .Checked = newBoolean[IND_SecurityOptions, IND_ShutdownAfterAutostart];

            textBoxSebBrowser           .Text = newString[IND_OnlineExam, IND_SebBrowser];
            textBoxAutostartProcess     .Text = newString[IND_OnlineExam, IND_AutostartProcess];
            textBoxExamUrl              .Text = newString[IND_OnlineExam, IND_ExamUrl];
            textBoxPermittedApplications.Text = newString[IND_OnlineExam, IND_PermittedApplications];
            textBoxQuitPassword         .Text = newString[IND_OnlineExam, IND_QuitPassword];
            textBoxQuitHashcode         .Text = newString[IND_OnlineExam, IND_QuitHashcode];
        }




        // **************************************************
        // Set the widgets to the new settings of MsgHook.ini
        // **************************************************
        private void SetWidgetsToNewSettingsOfMsgHookIni()
        {
            // Set the widgets to the new settings
                  textBoxCurrentMsgHookIni.Text    = newStringCurrentMsgHookIni;
            checkBoxWriteLogFileMsgHookLog.Checked = newBoolean[IND_MsgHookFiles, IND_WriteLogFileMsgHookLog];

            checkBoxEnableEsc       .Checked = newBoolean[IND_SpecialKeys, IND_EnableEsc];
            checkBoxEnableCtrlEsc   .Checked = newBoolean[IND_SpecialKeys, IND_EnableCtrlEsc];
            checkBoxEnableAltEsc    .Checked = newBoolean[IND_SpecialKeys, IND_EnableAltEsc];
            checkBoxEnableAltTab    .Checked = newBoolean[IND_SpecialKeys, IND_EnableAltTab];
            checkBoxEnableAltF4     .Checked = newBoolean[IND_SpecialKeys, IND_EnableAltF4];
            checkBoxEnableStartMenu .Checked = newBoolean[IND_SpecialKeys, IND_EnableStartMenu];
            checkBoxEnableRightMouse.Checked = newBoolean[IND_SpecialKeys, IND_EnableRightMouse];

            checkBoxEnableF1 .Checked = newBoolean[IND_FunctionKeys, IND_EnableF1];
            checkBoxEnableF2 .Checked = newBoolean[IND_FunctionKeys, IND_EnableF2];
            checkBoxEnableF3 .Checked = newBoolean[IND_FunctionKeys, IND_EnableF3];
            checkBoxEnableF4 .Checked = newBoolean[IND_FunctionKeys, IND_EnableF4];
            checkBoxEnableF5 .Checked = newBoolean[IND_FunctionKeys, IND_EnableF5];
            checkBoxEnableF6 .Checked = newBoolean[IND_FunctionKeys, IND_EnableF6];
            checkBoxEnableF7 .Checked = newBoolean[IND_FunctionKeys, IND_EnableF7];
            checkBoxEnableF8 .Checked = newBoolean[IND_FunctionKeys, IND_EnableF8];
            checkBoxEnableF9 .Checked = newBoolean[IND_FunctionKeys, IND_EnableF9];
            checkBoxEnableF10.Checked = newBoolean[IND_FunctionKeys, IND_EnableF10];
            checkBoxEnableF11.Checked = newBoolean[IND_FunctionKeys, IND_EnableF11];
            checkBoxEnableF12.Checked = newBoolean[IND_FunctionKeys, IND_EnableF12];

            // Convert the B1, B2, B3 strings to integers
            int  indexFunctionKey;
            for (indexFunctionKey = 1; indexFunctionKey <= 12; indexFunctionKey++)
            {
                if (newString[IND_ExitSequence, IND_B1].Equals(virtualKeyCodeString[indexFunctionKey]))
                    newIndexExitKey1 = indexFunctionKey;

                if (newString[IND_ExitSequence, IND_B2].Equals(virtualKeyCodeString[indexFunctionKey]))
                    newIndexExitKey2 = indexFunctionKey;

                if (newString[IND_ExitSequence, IND_B3].Equals(virtualKeyCodeString[indexFunctionKey]))
                    newIndexExitKey3 = indexFunctionKey;
            }

            listBoxExitKey1.SelectedIndex = newIndexExitKey1 - 1;
            listBoxExitKey2.SelectedIndex = newIndexExitKey2 - 1;
            listBoxExitKey3.SelectedIndex = newIndexExitKey3 - 1;

            textBoxQuitPassword.Text = newString[IND_OnlineExam, IND_QuitPassword];
            textBoxQuitHashcode.Text = newString[IND_OnlineExam, IND_QuitHashcode];
        }




        // ***********************************
        // Close the configuration application
        // ***********************************
        private void buttonExitAndSave_Click(object sender, EventArgs e)
        {
            // Save both ini files so that nothing gets lost
            SaveFileSebStarterIni();
            SaveFileMsgHookIni();

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
