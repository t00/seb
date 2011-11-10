using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace SebWindowsConfig
{
    public partial class SebWindowsConfigForm : Form
    {
        // Constants for indexing the ini file settings

        // The Graphical User Interface contains 7 groups
        const int IND_GroupNone = 0;
        const int IND_GroupMin  = 1;
        const int IND_GroupMax  = 7;
        const int IND_GroupNum  = 7;

        // Group indices
        const int IND_RegistryValues  = 1;
        const int IND_SecurityOptions = 2;
        const int IND_SpecialKeys     = 3;
        const int IND_FunctionKeys    = 4;

        // Each group contains up to 12 settings
        const int IND_SettingNone =  0;
        const int IND_SettingMin  =  1;
        const int IND_SettingMax  = 12;
        const int IND_SettingNum  = 12;

        // Group Registry values
        const int IND_EnableSwitchUser        = 1;
        const int IND_EnableLockThisComputer  = 2;
        const int IND_EnableChangeAPassword   = 3;
        const int IND_EnableStartTaskManager  = 4;
        const int IND_EnableLogOff            = 5;
        const int IND_EnableShutDown          = 6;
        const int IND_EnableEaseOfAccess      = 7;
        const int IND_EnableVmWareClientShade = 8;

        const String MSG_EnableSwitchUser        = "EnableSwitchUser";
        const String MSG_EnableLockThisComputer  = "EnableLockThisComputer";
        const String MSG_EnableChangeAPassword   = "EnableChangeAPassword";
        const String MSG_EnableStartTaskManager  = "EnableStartTaskManager";
        const String MSG_EnableLogOff            = "EnableLogOff";
        const String MSG_EnableShutDown          = "EnableShutDown";
        const String MSG_EnableEaseOfAccess      = "EnableEaseOfAccess";
        const String MSG_EnableVmWareClientShade = "EnableVmWareClientShade";

        // Group Security options
        const int IND_AllowVirtualMachine       = 1;
        const int IND_ForceWindowsService       = 2;
        const int IND_WriteLogFileSebStarterLog = 3;
        const int IND_CreateNewDesktop          = 4;
        const int IND_ShowSebApplicationChooser = 5;
        const int IND_HookMessages              = 6;
        const int IND_EditRegistry              = 7;
        const int IND_ShutdownAfterAutostart    = 8;
        const int IND_MonitorProcesses          = 9;
        const int IND_Win9xKillExplorer         = 10;
        const int IND_Win9xScreenSaverRunning   = 11;

        const String MSG_AllowVirtualMachine       = "AllowVirtualMachine";
        const String MSG_ForceWindowsService       = "ForceWindowsService";
        const String MSG_WriteLogFileSebStarterLog = "WriteLogFileSebStarterLog";
        const String MSG_CreateNewDesktop          = "CreateNewDesktop";
        const String MSG_ShowSebApplicationChooser = "ShowSebApplicationChooser";
        const String MSG_HookMessages              = "HookMessages";
        const String MSG_EditRegistry              = "EditRegistry";
        const String MSG_MonitorProcesses          = "MonitorProcesses";
        const String MSG_ShutdownAfterAutostart    = "ShutdownAfterAutostartProcessTerminates";
        const String MSG_Win9xKillExplorer         = "Win9xKillExplorer";
        const String MSG_Win9xScreenSaverRunning   = "Win9xScreenSaverRunning";

        const String MSG_SebBrowser            = "SebBrowser";
        const String MSG_AutostartProcess      = "AutostartProcess";
        const String MSG_ExamUrl               = "ExamUrl";
        const String MSG_PermittedApplications = "PermittedApplications";

        // Group Special keys
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

        // Group Function keys
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

        const String MSG_B1 = "B1";
        const String MSG_B2 = "B2";
        const String MSG_B3 = "B3";


        // Global variables

        // Names of settings
        static String[,]  msgString = new String[IND_GroupNum + 1, IND_SettingNum + 1];

        // Values of settings as booleans (true or false)
        static Boolean[,] defSetting = new Boolean[IND_GroupNum + 1, IND_SettingNum + 1];
        static Boolean[,] oldSetting = new Boolean[IND_GroupNum + 1, IND_SettingNum + 1];
        static Boolean[,] newSetting = new Boolean[IND_GroupNum + 1, IND_SettingNum + 1];

        static Boolean[,]  allowSetting = new Boolean[IND_GroupNum + 1, IND_SettingNum + 1];
        static Boolean[,] forbidSetting = new Boolean[IND_GroupNum + 1, IND_SettingNum + 1];

        static String[] virtualKeyCodeString = new String[IND_SettingNum + 1];

        String oldStringSebBrowser = "";
        String newStringSebBrowser = "";
        String msgStringSebBrowser = "";

        String oldStringAutostartProcess = "";
        String newStringAutostartProcess = "";
        String msgStringAutostartProcess = "";

        String oldStringExamUrl = "";
        String newStringExamUrl = "";
        String msgStringExamUrl = "";

        String oldStringPermittedApplications = "";
        String newStringPermittedApplications = "";
        String msgStringPermittedApplications = "";

        String oldStringB1 = "";
        String newStringB1 = "";
        String msgStringB1 = "";

        String oldStringB2 = "";
        String newStringB2 = "";
        String msgStringB2 = "";

        String oldStringB3 = "";
        String newStringB3 = "";
        String msgStringB3 = "";

        int oldIndexExitKeyFirst  = 0;
        int oldIndexExitKeySecond = 0;
        int oldIndexExitKeyThird  = 0;

        int newIndexExitKeyFirst  = 0;
        int newIndexExitKeySecond = 0;
        int newIndexExitKeyThird  = 0;

        String       stringPathSebStarterIni = "";
        String       stringPathMsgHookIni    = "";
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

            int  indexGroup;
            int  indexSetting;
            for (indexGroup   = IND_GroupMin   ; indexGroup   <= IND_GroupMax  ; indexGroup++)
            for (indexSetting = IND_SettingMin ; indexSetting <= IND_SettingMax; indexSetting++)
            {
                   oldSetting[indexGroup, indexSetting] = false;
                   newSetting[indexGroup, indexSetting] = false;
                   defSetting[indexGroup, indexSetting] = false;
                 allowSetting[indexGroup, indexSetting] = true;
                forbidSetting[indexGroup, indexSetting] = false;
            }

            msgString[IND_RegistryValues, IND_EnableSwitchUser       ] = MSG_EnableSwitchUser;
            msgString[IND_RegistryValues, IND_EnableLockThisComputer ] = MSG_EnableLockThisComputer;
            msgString[IND_RegistryValues, IND_EnableChangeAPassword  ] = MSG_EnableChangeAPassword;
            msgString[IND_RegistryValues, IND_EnableStartTaskManager ] = MSG_EnableStartTaskManager;
            msgString[IND_RegistryValues, IND_EnableLogOff           ] = MSG_EnableLogOff;
            msgString[IND_RegistryValues, IND_EnableShutDown         ] = MSG_EnableShutDown;
            msgString[IND_RegistryValues, IND_EnableEaseOfAccess     ] = MSG_EnableEaseOfAccess;
            msgString[IND_RegistryValues, IND_EnableVmWareClientShade] = MSG_EnableVmWareClientShade;

            msgString[IND_SecurityOptions, IND_AllowVirtualMachine      ] = MSG_AllowVirtualMachine;
            msgString[IND_SecurityOptions, IND_ForceWindowsService      ] = MSG_ForceWindowsService;
            msgString[IND_SecurityOptions, IND_WriteLogFileSebStarterLog] = MSG_WriteLogFileSebStarterLog;
            msgString[IND_SecurityOptions, IND_CreateNewDesktop         ] = MSG_CreateNewDesktop;
            msgString[IND_SecurityOptions, IND_ShowSebApplicationChooser] = MSG_ShowSebApplicationChooser;
            msgString[IND_SecurityOptions, IND_HookMessages             ] = MSG_HookMessages;
            msgString[IND_SecurityOptions, IND_EditRegistry             ] = MSG_EditRegistry;
            msgString[IND_SecurityOptions, IND_MonitorProcesses         ] = MSG_MonitorProcesses;
            msgString[IND_SecurityOptions, IND_ShutdownAfterAutostart   ] = MSG_ShutdownAfterAutostart;

            msgStringSebBrowser            = MSG_SebBrowser;
            msgStringAutostartProcess      = MSG_AutostartProcess;
            msgStringExamUrl               = MSG_ExamUrl;
            msgStringPermittedApplications = MSG_PermittedApplications;

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

            msgStringB1 = MSG_B1;
            msgStringB2 = MSG_B2;
            msgStringB3 = MSG_B3;

            virtualKeyCodeString[1] = "112";
            virtualKeyCodeString[2] = "113";
            virtualKeyCodeString[3] = "114";
            virtualKeyCodeString[4] = "115";
            virtualKeyCodeString[5] = "116";
            virtualKeyCodeString[6] = "117";
            virtualKeyCodeString[7] = "118";
            virtualKeyCodeString[8] = "119";
            virtualKeyCodeString[9] = "120";
            virtualKeyCodeString[10] = "121";
            virtualKeyCodeString[11] = "122";
            virtualKeyCodeString[12] = "123";

        } // end of contructor   SebWindowsConfigForm()




        // ************************
        // Open file SebStarter.ini
        // ************************
        private void labelOpenFileSebStarterIni_Click(object sender, EventArgs e)
        {
            dialogResultSebStarterIni = openFileDialogSebStarterIni.ShowDialog();
            stringPathSebStarterIni   = openFileDialogSebStarterIni.FileName;

            try 
            {
                // Create an instance of StreamReader to read from a file.
                streamReaderSebStarterIni = new StreamReader(stringPathSebStarterIni);
                String line;

                // Read and display lines from the file until the end of 
                // the file is reached.
                while ((line = streamReaderSebStarterIni.ReadLine()) != null) 
                {
                    Console.WriteLine(line);

                    // Skip empty lines and lines not in "leftSide = rightSide" format
                    if (line.Contains("="))
                    {
                        int     equalPos = line.IndexOf  ("=");
                        String  leftSide = line.Remove   (equalPos);
                        String rightSide = line.Substring(equalPos + 1);

                        int  indexGroup;
                        int  indexSetting;
                        for (indexGroup   = IND_GroupMin   ; indexGroup   <= IND_GroupMax  ; indexGroup++)
                        for (indexSetting = IND_SettingMin ; indexSetting <= IND_SettingMax; indexSetting++)
                        {
                            if (leftSide.Equals(msgString[indexGroup, indexSetting]))
                            {
                                Boolean rightBool = false;
                                if (rightSide.Equals("0")) rightBool = false;
                                if (rightSide.Equals("1")) rightBool = true;
                                newSetting[indexGroup, indexSetting] = rightBool;
                            }
                        }

                        if (leftSide.Equals(msgStringSebBrowser))
                            newStringSebBrowser = rightSide;

                        if (leftSide.Equals(msgStringAutostartProcess))
                            newStringAutostartProcess = rightSide;

                        if (leftSide.Equals(msgStringExamUrl))
                            newStringExamUrl = rightSide;

                        if (leftSide.Equals(msgStringPermittedApplications))
                            newStringPermittedApplications = rightSide;

                    } // end if line.Contains("=")
                } // end while

                // Close the StreamReader
                streamReaderSebStarterIni.Close();

                // Assign the settings from the SebStarter.ini file to the widgets
                checkBoxEnableSwitchUser       .Checked = newSetting[IND_RegistryValues, IND_EnableSwitchUser];
                checkBoxEnableLockThisComputer .Checked = newSetting[IND_RegistryValues, IND_EnableLockThisComputer];
                checkBoxEnableChangeAPassword  .Checked = newSetting[IND_RegistryValues, IND_EnableChangeAPassword];
                checkBoxEnableStartTaskManager .Checked = newSetting[IND_RegistryValues, IND_EnableStartTaskManager];
                checkBoxEnableLogOff           .Checked = newSetting[IND_RegistryValues, IND_EnableLogOff];
                checkBoxEnableShutDown         .Checked = newSetting[IND_RegistryValues, IND_EnableShutDown];
                checkBoxEnableEaseOfAccess     .Checked = newSetting[IND_RegistryValues, IND_EnableEaseOfAccess];
                checkBoxEnableVmWareClientShade.Checked = newSetting[IND_RegistryValues, IND_EnableVmWareClientShade];

                checkBoxAllowVirtualMachine      .Checked = newSetting[IND_SecurityOptions, IND_AllowVirtualMachine];
                checkBoxForceWindowsService      .Checked = newSetting[IND_SecurityOptions, IND_ForceWindowsService];
                checkBoxWriteLogFileSebStarterLog.Checked = newSetting[IND_SecurityOptions, IND_WriteLogFileSebStarterLog];
                checkBoxCreateNewDesktop         .Checked = newSetting[IND_SecurityOptions, IND_CreateNewDesktop];
                checkBoxShowSebApplicationChooser.Checked = newSetting[IND_SecurityOptions, IND_ShowSebApplicationChooser];
                checkBoxHookMessages             .Checked = newSetting[IND_SecurityOptions, IND_HookMessages];
                checkBoxEditRegistry             .Checked = newSetting[IND_SecurityOptions, IND_EditRegistry];
                checkBoxMonitorProcesses         .Checked = newSetting[IND_SecurityOptions, IND_MonitorProcesses];
                checkBoxShutdownAfterAutostart   .Checked = newSetting[IND_SecurityOptions, IND_ShutdownAfterAutostart];

                textBoxSebBrowser           .Text = newStringSebBrowser;
                textBoxAutostartProcess     .Text = newStringAutostartProcess;
                textBoxExamUrl              .Text = newStringExamUrl;
                textBoxPermittedApplications.Text = newStringPermittedApplications;

            } // end try
            catch (Exception streamReadException) 
            {
                // Let the user know what went wrong.
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(streamReadException.Message);
            }

        } // end of method   labelOpenFileSebStarterIni_Click()




        // ************************
        // Save file SebStarter.ini
        // ************************
        private void labelSaveFileSebStarterIni_Click(object sender, EventArgs e)
        {
            DialogResult saveFileName = saveFileDialogSebStarterIni.ShowDialog();
        }




        // *********************
        // Open file MsgHook.ini
        // *********************
        private void labelOpenFileMsgHookIni_Click(object sender, EventArgs e)
        {
            dialogResultMsgHookIni = openFileDialogMsgHookIni.ShowDialog();
            stringPathMsgHookIni   = openFileDialogMsgHookIni.FileName;

            try 
            {
                // Create an instance of StreamReader to read from a file.
                streamReaderMsgHookIni = new StreamReader(stringPathMsgHookIni);
                String line;

                // Read and display lines from the file until the end of 
                // the file is reached.
                while ((line = streamReaderMsgHookIni.ReadLine()) != null) 
                {
                    Console.WriteLine(line);
                    //textBoxDebug.Text = line;

                    // Skip empty lines and lines not in "leftSide = rightSide" format
                    if (line.Contains("="))
                    {
                        int     equalPos = line.IndexOf  ("=");
                        String  leftSide = line.Remove   (equalPos);
                        String rightSide = line.Substring(equalPos + 1);

                        int  indexGroup;
                        int  indexSetting;
                        for (indexGroup   = IND_GroupMin   ; indexGroup   <= IND_GroupMax  ; indexGroup++)
                        for (indexSetting = IND_SettingMin ; indexSetting <= IND_SettingMax; indexSetting++)
                        {
                            if (leftSide.Equals(msgString[indexGroup, indexSetting]))
                            {
                                Boolean rightBool = false;
                                if (rightSide.Equals("0")) rightBool = false;
                                if (rightSide.Equals("1")) rightBool = true;
                                oldSetting[indexGroup, indexSetting] = rightBool;
                                newSetting[indexGroup, indexSetting] = rightBool;
                            }
                        }

                        if (leftSide.Equals(msgStringB1))
                        {
                            oldStringB1 = rightSide;
                            newStringB1 = rightSide;
                        }

                        if (leftSide.Equals(msgStringB2))
                        {
                            oldStringB2 = rightSide;
                            newStringB2 = rightSide;
                        }

                        if (leftSide.Equals(msgStringB3))
                        {
                            oldStringB3 = rightSide;
                            newStringB3 = rightSide;
                        }

                    } // end if line.Contains("=")
                } // end while

                // Close the StreamReader
                streamReaderMsgHookIni.Close();


                // Assign the settings from the MsgHook.ini file to the widgets
                checkBoxEnableEsc       .Checked = newSetting[IND_SpecialKeys, IND_EnableEsc];
                checkBoxEnableCtrlEsc   .Checked = newSetting[IND_SpecialKeys, IND_EnableCtrlEsc];
                checkBoxEnableAltEsc    .Checked = newSetting[IND_SpecialKeys, IND_EnableAltEsc];
                checkBoxEnableAltTab    .Checked = newSetting[IND_SpecialKeys, IND_EnableAltTab];
                checkBoxEnableAltF4     .Checked = newSetting[IND_SpecialKeys, IND_EnableAltF4];
                checkBoxEnableStartMenu .Checked = newSetting[IND_SpecialKeys, IND_EnableStartMenu];
                checkBoxEnableRightMouse.Checked = newSetting[IND_SpecialKeys, IND_EnableRightMouse];

                checkBoxEnableF1.Checked  = newSetting[IND_FunctionKeys, IND_EnableF1];
                checkBoxEnableF2.Checked  = newSetting[IND_FunctionKeys, IND_EnableF2];
                checkBoxEnableF3.Checked  = newSetting[IND_FunctionKeys, IND_EnableF3];
                checkBoxEnableF4.Checked  = newSetting[IND_FunctionKeys, IND_EnableF4];
                checkBoxEnableF5.Checked  = newSetting[IND_FunctionKeys, IND_EnableF5];
                checkBoxEnableF6.Checked  = newSetting[IND_FunctionKeys, IND_EnableF6];
                checkBoxEnableF7.Checked  = newSetting[IND_FunctionKeys, IND_EnableF7];
                checkBoxEnableF8.Checked  = newSetting[IND_FunctionKeys, IND_EnableF8];
                checkBoxEnableF9.Checked  = newSetting[IND_FunctionKeys, IND_EnableF9];
                checkBoxEnableF10.Checked = newSetting[IND_FunctionKeys, IND_EnableF10];
                checkBoxEnableF11.Checked = newSetting[IND_FunctionKeys, IND_EnableF11];
                checkBoxEnableF12.Checked = newSetting[IND_FunctionKeys, IND_EnableF12];

                // Convert the B1, B2, B3 strings to integers and booleans

                int  indexFunctionKey;
                for (indexFunctionKey = 1; indexFunctionKey <= 12; indexFunctionKey++)
                {
                    if (newStringB1.Equals(virtualKeyCodeString[indexFunctionKey]))
                    {
                        oldIndexExitKeyFirst = indexFunctionKey;
                        newIndexExitKeyFirst = indexFunctionKey;
                    }

                    if (newStringB2.Equals(virtualKeyCodeString[indexFunctionKey]))
                    {
                        oldIndexExitKeySecond = indexFunctionKey;
                        newIndexExitKeySecond = indexFunctionKey;
                    }

                    if (newStringB3.Equals(virtualKeyCodeString[indexFunctionKey]))
                    {
                        oldIndexExitKeyThird = indexFunctionKey;
                        newIndexExitKeyThird = indexFunctionKey;
                    }
                }

                listBoxExitKeyFirst .SelectedIndex = newIndexExitKeyFirst  - 1;
                listBoxExitKeySecond.SelectedIndex = newIndexExitKeySecond - 1;
                listBoxExitKeyThird .SelectedIndex = newIndexExitKeyThird  - 1;

            } // end try
            catch (Exception streamReadException) 
            {
                // Let the user know what went wrong.
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(streamReadException.Message);
            }

        }  // end of method   labelOpenFileMsgHookIni_Click()




        // *********************
        // Save file MsgHook.ini
        // *********************
        private void labelSaveFileMsgHookIni_Click(object sender, EventArgs e)
        {

        }


        // If the user changes a setting by clicking the checkbox,
        // update the setting in memory for it can be saved on file later.

        // Group "Registry values"

        private void checkBoxEnableSwitchUser_CheckedChanged(object sender, EventArgs e)
        {
            newSetting[IND_RegistryValues, IND_EnableSwitchUser] = checkBoxEnableSwitchUser.Checked;
        }

        private void checkBoxEnableLockThisComputer_CheckedChanged(object sender, EventArgs e)
        {
            newSetting[IND_RegistryValues, IND_EnableLockThisComputer] = checkBoxEnableLockThisComputer.Checked;
        }

        private void checkBoxEnableChangeAPassword_CheckedChanged(object sender, EventArgs e)
        {
            newSetting[IND_RegistryValues, IND_EnableChangeAPassword] = checkBoxEnableChangeAPassword.Checked;
        }

        private void checkBoxEnableStartTaskManager_CheckedChanged(object sender, EventArgs e)
        {
            newSetting[IND_RegistryValues, IND_EnableStartTaskManager] = checkBoxEnableStartTaskManager.Checked;
        }

        private void checkBoxEnableLogOff_CheckedChanged(object sender, EventArgs e)
        {
            newSetting[IND_RegistryValues, IND_EnableLogOff] = checkBoxEnableLogOff.Checked;
        }

        private void checkBoxEnableShutDown_CheckedChanged(object sender, EventArgs e)
        {
            newSetting[IND_RegistryValues, IND_EnableShutDown] = checkBoxEnableShutDown.Checked;
        }

        private void checkBoxEnableEaseOfAccess_CheckedChanged(object sender, EventArgs e)
        {
            newSetting[IND_RegistryValues, IND_EnableEaseOfAccess] = checkBoxEnableEaseOfAccess.Checked;
        }

        private void checkBoxEnableVmWareClientShade_CheckedChanged(object sender, EventArgs e)
        {
            newSetting[IND_RegistryValues, IND_EnableVmWareClientShade] = checkBoxEnableVmWareClientShade.Checked;
        }


        // Group "Security options"

        private void checkBoxAllowVirtualMachine_CheckedChanged(object sender, EventArgs e)
        {
            newSetting[IND_SecurityOptions, IND_AllowVirtualMachine] = checkBoxAllowVirtualMachine.Checked;
        }

        private void checkBoxForceWindowsService_CheckedChanged(object sender, EventArgs e)
        {
            newSetting[IND_SecurityOptions, IND_ForceWindowsService] = checkBoxForceWindowsService.Checked;
        }

        private void checkBoxWriteLogFileSebStarterLog_CheckedChanged(object sender, EventArgs e)
        {
            newSetting[IND_SecurityOptions, IND_WriteLogFileSebStarterLog] = checkBoxWriteLogFileSebStarterLog.Checked;
        }

        private void checkBoxCreateNewDesktop_CheckedChanged(object sender, EventArgs e)
        {
            newSetting[IND_SecurityOptions, IND_CreateNewDesktop] = checkBoxCreateNewDesktop.Checked;
        }

        private void checkBoxShowSebApplicationChooser_CheckedChanged(object sender, EventArgs e)
        {
            newSetting[IND_SecurityOptions, IND_ShowSebApplicationChooser] = checkBoxShowSebApplicationChooser.Checked;
        }

        private void checkBoxHookMessages_CheckedChanged(object sender, EventArgs e)
        {
            newSetting[IND_SecurityOptions, IND_HookMessages] = checkBoxHookMessages.Checked;
        }

        private void checkBoxEditRegistry_CheckedChanged(object sender, EventArgs e)
        {
            newSetting[IND_SecurityOptions, IND_EditRegistry] = checkBoxEditRegistry.Checked;
        }

        private void checkBoxMonitorProcesses_CheckedChanged(object sender, EventArgs e)
        {
            newSetting[IND_SecurityOptions, IND_MonitorProcesses] = checkBoxMonitorProcesses.Checked;
        }

        private void checkBoxShutdownAfterAutostart_CheckedChanged(object sender, EventArgs e)
        {
            newSetting[IND_SecurityOptions, IND_ShutdownAfterAutostart] = checkBoxShutdownAfterAutostart.Checked;
        }


        // Group "Online exam"

        private void textBoxSebBrowser_TextChanged(object sender, EventArgs e)
        {
            newStringSebBrowser = textBoxSebBrowser.Text;
        }

        private void textBoxAutostartProcess_TextChanged(object sender, EventArgs e)
        {
            newStringAutostartProcess = textBoxAutostartProcess.Text;
        }

        private void textBoxExamUrl_TextChanged(object sender, EventArgs e)
        {
            newStringExamUrl = textBoxExamUrl.Text;
        }

        private void textBoxPermittedApplications_TextChanged(object sender, EventArgs e)
        {
            newStringPermittedApplications = textBoxPermittedApplications.Text;
        }


        // Group "Special keys"

        private void checkBoxEnableEsc_CheckedChanged(object sender, EventArgs e)
        {
            newSetting[IND_SpecialKeys, IND_EnableEsc] = checkBoxEnableEsc.Checked;
        }

        private void checkBoxEnableCtrlEsc_CheckedChanged(object sender, EventArgs e)
        {
            newSetting[IND_SpecialKeys, IND_EnableCtrlEsc] = checkBoxEnableCtrlEsc.Checked;
        }

        private void checkBoxEnableAltEsc_CheckedChanged(object sender, EventArgs e)
        {
            newSetting[IND_SpecialKeys, IND_EnableAltEsc] = checkBoxEnableAltEsc.Checked;
        }

        private void checkBoxEnableAltTab_CheckedChanged(object sender, EventArgs e)
        {
            newSetting[IND_SpecialKeys, IND_EnableAltTab] = checkBoxEnableAltTab.Checked;
        }

        private void checkBoxEnableAltF4_CheckedChanged(object sender, EventArgs e)
        {
            newSetting[IND_SpecialKeys, IND_EnableAltF4] = checkBoxEnableAltF4.Checked;
        }

        private void checkBoxEnableStartMenu_CheckedChanged(object sender, EventArgs e)
        {
            newSetting[IND_SpecialKeys, IND_EnableStartMenu] = checkBoxEnableStartMenu.Checked;
        }

        private void checkBoxEnableRightMouse_CheckedChanged(object sender, EventArgs e)
        {
            newSetting[IND_SpecialKeys, IND_EnableRightMouse] = checkBoxEnableRightMouse.Checked;
        }


        // Group "Function keys"

        private void checkBoxEnableF1_CheckedChanged(object sender, EventArgs e)
        {
            newSetting[IND_FunctionKeys, IND_EnableF1] = checkBoxEnableF1.Checked;
        }

        private void checkBoxEnableF2_CheckedChanged(object sender, EventArgs e)
        {
            newSetting[IND_FunctionKeys, IND_EnableF2] = checkBoxEnableF2.Checked;
        }

        private void checkBoxEnableF3_CheckedChanged(object sender, EventArgs e)
        {
            newSetting[IND_FunctionKeys, IND_EnableF3] = checkBoxEnableF3.Checked;
        }

        private void checkBoxEnableF4_CheckedChanged(object sender, EventArgs e)
        {
            newSetting[IND_FunctionKeys, IND_EnableF4] = checkBoxEnableF4.Checked;
        }

        private void checkBoxEnableF5_CheckedChanged(object sender, EventArgs e)
        {
            newSetting[IND_FunctionKeys, IND_EnableF5] = checkBoxEnableF5.Checked;
        }

        private void checkBoxEnableF6_CheckedChanged(object sender, EventArgs e)
        {
            newSetting[IND_FunctionKeys, IND_EnableF6] = checkBoxEnableF6.Checked;
        }

        private void checkBoxEnableF7_CheckedChanged(object sender, EventArgs e)
        {
            newSetting[IND_FunctionKeys, IND_EnableF7] = checkBoxEnableF7.Checked;
        }

        private void checkBoxEnableF8_CheckedChanged(object sender, EventArgs e)
        {
            newSetting[IND_FunctionKeys, IND_EnableF8] = checkBoxEnableF8.Checked;
        }

        private void checkBoxEnableF9_CheckedChanged(object sender, EventArgs e)
        {
            newSetting[IND_FunctionKeys, IND_EnableF9] = checkBoxEnableF9.Checked;
        }

        private void checkBoxEnableF10_CheckedChanged(object sender, EventArgs e)
        {
            newSetting[IND_FunctionKeys, IND_EnableF10] = checkBoxEnableF10.Checked;
        }

        private void checkBoxEnableF11_CheckedChanged(object sender, EventArgs e)
        {
            newSetting[IND_FunctionKeys, IND_EnableF11] = checkBoxEnableF11.Checked;
        }

        private void checkBoxEnableF12_CheckedChanged(object sender, EventArgs e)
        {
            newSetting[IND_FunctionKeys, IND_EnableF12] = checkBoxEnableF12.Checked;
        }


        // Group "Exit sequence"

        private void listBoxExitKeyFirst_SelectedIndexChanged(object sender, EventArgs e)
        {
            newIndexExitKeyFirst = listBoxExitKeyFirst.SelectedIndex + 1;
        }


        private void listBoxExitKeySecond_SelectedIndexChanged(object sender, EventArgs e)
        {
            newIndexExitKeySecond = listBoxExitKeySecond.SelectedIndex + 1;
        }


        private void listBoxExitKeyThird_SelectedIndexChanged(object sender, EventArgs e)
        {
            newIndexExitKeyThird = listBoxExitKeyThird.SelectedIndex + 1;
        }



        // ***************************************
        // Restore settings of file SebStarter.ini
        // ***************************************
        private void buttonRestoreSettingsOfSebStarterIni_Click(object sender, EventArgs e)
        {
            int  indexGroup;
            int  indexSetting;
            for (indexGroup   = IND_GroupMin   ; indexGroup   <= IND_GroupMax  ; indexGroup++)
            for (indexSetting = IND_SettingMin ; indexSetting <= IND_SettingMax; indexSetting++)
            {
                newSetting[indexGroup, indexSetting] = oldSetting[indexGroup, indexSetting];
            }

            newStringSebBrowser            = oldStringSebBrowser;
            newStringAutostartProcess      = oldStringAutostartProcess;
            newStringExamUrl               = oldStringExamUrl;
            newStringPermittedApplications = oldStringPermittedApplications;

            // Assign the old settings from the SebStarter.ini file to the widgets again
            checkBoxEnableSwitchUser       .Checked = oldSetting[IND_RegistryValues, IND_EnableSwitchUser];
            checkBoxEnableLockThisComputer .Checked = oldSetting[IND_RegistryValues, IND_EnableLockThisComputer];
            checkBoxEnableChangeAPassword  .Checked = oldSetting[IND_RegistryValues, IND_EnableChangeAPassword];
            checkBoxEnableStartTaskManager .Checked = oldSetting[IND_RegistryValues, IND_EnableStartTaskManager];
            checkBoxEnableLogOff           .Checked = oldSetting[IND_RegistryValues, IND_EnableLogOff];
            checkBoxEnableShutDown         .Checked = oldSetting[IND_RegistryValues, IND_EnableShutDown];
            checkBoxEnableEaseOfAccess     .Checked = oldSetting[IND_RegistryValues, IND_EnableEaseOfAccess];
            checkBoxEnableVmWareClientShade.Checked = oldSetting[IND_RegistryValues, IND_EnableVmWareClientShade];

            checkBoxAllowVirtualMachine      .Checked = oldSetting[IND_SecurityOptions, IND_AllowVirtualMachine];
            checkBoxForceWindowsService      .Checked = oldSetting[IND_SecurityOptions, IND_ForceWindowsService];
            checkBoxWriteLogFileSebStarterLog.Checked = oldSetting[IND_SecurityOptions, IND_WriteLogFileSebStarterLog];
            checkBoxCreateNewDesktop         .Checked = oldSetting[IND_SecurityOptions, IND_CreateNewDesktop];
            checkBoxShowSebApplicationChooser.Checked = oldSetting[IND_SecurityOptions, IND_ShowSebApplicationChooser];
            checkBoxHookMessages             .Checked = oldSetting[IND_SecurityOptions, IND_HookMessages];
            checkBoxEditRegistry             .Checked = oldSetting[IND_SecurityOptions, IND_EditRegistry];
            checkBoxMonitorProcesses         .Checked = oldSetting[IND_SecurityOptions, IND_MonitorProcesses];
            checkBoxShutdownAfterAutostart   .Checked = oldSetting[IND_SecurityOptions, IND_ShutdownAfterAutostart];

            textBoxSebBrowser           .Text = oldStringSebBrowser;
            textBoxAutostartProcess     .Text = oldStringAutostartProcess;
            textBoxExamUrl              .Text = oldStringExamUrl;
            textBoxPermittedApplications.Text = oldStringPermittedApplications;
        }



        // ************************************
        // Restore settings of file MsgHook.ini
        // ************************************
        private void buttonRestoreSettingsOfMsgHookIni_Click(object sender, EventArgs e)
        {
            int  indexGroup;
            int  indexSetting;
            for (indexGroup   = IND_GroupMin   ; indexGroup   <= IND_GroupMax  ; indexGroup++)
            for (indexSetting = IND_SettingMin ; indexSetting <= IND_SettingMax; indexSetting++)
            {
                newSetting[indexGroup, indexSetting] = oldSetting[indexGroup, indexSetting];
            }

            newStringB1 = oldStringB1;
            newStringB2 = oldStringB2;
            newStringB3 = oldStringB3;

            newIndexExitKeyFirst  = oldIndexExitKeyFirst;
            newIndexExitKeySecond = oldIndexExitKeySecond;
            newIndexExitKeyThird  = oldIndexExitKeyThird;

            // Assign the old settings from the MsgHook.ini file to the widgets again
            checkBoxEnableEsc       .Checked = oldSetting[IND_SpecialKeys, IND_EnableEsc];
            checkBoxEnableCtrlEsc   .Checked = oldSetting[IND_SpecialKeys, IND_EnableCtrlEsc];
            checkBoxEnableAltEsc    .Checked = oldSetting[IND_SpecialKeys, IND_EnableAltEsc];
            checkBoxEnableAltTab    .Checked = oldSetting[IND_SpecialKeys, IND_EnableAltTab];
            checkBoxEnableAltF4     .Checked = oldSetting[IND_SpecialKeys, IND_EnableAltF4];
            checkBoxEnableStartMenu .Checked = oldSetting[IND_SpecialKeys, IND_EnableStartMenu];
            checkBoxEnableRightMouse.Checked = oldSetting[IND_SpecialKeys, IND_EnableRightMouse];

            checkBoxEnableF1.Checked  = oldSetting[IND_FunctionKeys, IND_EnableF1];
            checkBoxEnableF2.Checked  = oldSetting[IND_FunctionKeys, IND_EnableF2];
            checkBoxEnableF3.Checked  = oldSetting[IND_FunctionKeys, IND_EnableF3];
            checkBoxEnableF4.Checked  = oldSetting[IND_FunctionKeys, IND_EnableF4];
            checkBoxEnableF5.Checked  = oldSetting[IND_FunctionKeys, IND_EnableF5];
            checkBoxEnableF6.Checked  = oldSetting[IND_FunctionKeys, IND_EnableF6];
            checkBoxEnableF7.Checked  = oldSetting[IND_FunctionKeys, IND_EnableF7];
            checkBoxEnableF8.Checked  = oldSetting[IND_FunctionKeys, IND_EnableF8];
            checkBoxEnableF9.Checked  = oldSetting[IND_FunctionKeys, IND_EnableF9];
            checkBoxEnableF10.Checked = oldSetting[IND_FunctionKeys, IND_EnableF10];
            checkBoxEnableF11.Checked = oldSetting[IND_FunctionKeys, IND_EnableF11];
            checkBoxEnableF12.Checked = oldSetting[IND_FunctionKeys, IND_EnableF12];

            listBoxExitKeyFirst .SelectedIndex = oldIndexExitKeyFirst  - 1;
            listBoxExitKeySecond.SelectedIndex = oldIndexExitKeySecond - 1;
            listBoxExitKeyThird .SelectedIndex = oldIndexExitKeyThird  - 1;
        }



    }
}
