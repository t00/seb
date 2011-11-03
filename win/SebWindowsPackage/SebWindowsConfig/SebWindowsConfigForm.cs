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
        const int IND_RegistryValues   = 1;
        const int IND_SecurityOptions  = 2;
        const int IND_SpecialKeys      = 3;
        const int IND_FunctionKeys     = 4;
        const int IND_Key1             = 5;
        const int IND_Key2             = 6;
        const int IND_Key3             = 7;


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

/*
        const String TYPE_EnableSwitchUser        = "REG_DWORD";
        const String TYPE_EnableLockThisComputer  = "REG_DWORD";
        const String TYPE_EnableChangeAPassword   = "REG_DWORD";
        const String TYPE_EnableStartTaskManager  = "REG_DWORD";
        const String TYPE_EnableLogOff            = "REG_DWORD";
        const String TYPE_EnableShutDown          = "REG_DWORD";
        const String TYPE_EnableEaseOfAccess      = "REG_SZ";
        const String TYPE_EnableVmWareClientShade = "REG_DWORD";
*/

        // Global variables

        // Names of settings
        static String[,]  msgString = new String[IND_GroupNum + 1, IND_SettingNum + 1];
      //static String[,] typeString = new String[IND_GroupNum + 1, IND_SettingNum + 1];

        // Values of settings as booleans (true or false)
        static Boolean[,] defSetting = new Boolean[IND_GroupNum + 1, IND_SettingNum + 1];
        static Boolean[,] oldSetting = new Boolean[IND_GroupNum + 1, IND_SettingNum + 1];
        static Boolean[,] newSetting = new Boolean[IND_GroupNum + 1, IND_SettingNum + 1];

        static Boolean[,]  allowSetting = new Boolean[IND_GroupNum + 1, IND_SettingNum + 1];
        static Boolean[,] forbidSetting = new Boolean[IND_GroupNum + 1, IND_SettingNum + 1];

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

/*
            typeString[IND_RegistryValues, IND_EnableSwitchUser       ] = TYPE_EnableSwitchUser;
            typeString[IND_RegistryValues, IND_EnableLockThisComputer ] = TYPE_EnableLockThisComputer;
            typeString[IND_RegistryValues, IND_EnableChangeAPassword  ] = TYPE_EnableChangeAPassword;
            typeString[IND_RegistryValues, IND_EnableStartTaskManager ] = TYPE_EnableStartTaskManager;
            typeString[IND_RegistryValues, IND_EnableLogOff           ] = TYPE_EnableLogOff;
            typeString[IND_RegistryValues, IND_EnableShutDown         ] = TYPE_EnableShutDown;
            typeString[IND_RegistryValues, IND_EnableEaseOfAccess     ] = TYPE_EnableEaseOfAccess;
            typeString[IND_RegistryValues, IND_EnableVmWareClientShade] = TYPE_EnableVmWareClientShade;
*/
        } // end of contructor   SebWindowsConfigForm()




        // ************************
        // Open File SebStarter.ini
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

                // Assign the settings from the ini file to the widgets
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
        // Save File SebStarter.ini
        // ************************
        private void labelSaveFileSebStarterIni_Click(object sender, EventArgs e)
        {
            DialogResult saveFileName = saveFileDialogSebStarterIni.ShowDialog();
        }




        // *********************
        // Open File MsgHook.ini
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

                        if (leftSide.Equals(msgStringB1))
                            newStringB1 = rightSide;

                        if (leftSide.Equals(msgStringB2))
                            newStringB2 = rightSide;

                        if (leftSide.Equals(msgStringB3))
                            newStringB3 = rightSide;

                    } // end if line.Contains("=")
                } // end while

                // Close the StreamReader
                streamReaderSebStarterIni.Close();

                // Assign the settings from the ini file to the widgets
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

        }  // end of method   labelOpenFileMsgHookIni_Click()




        // *********************
        // Save File MsgHook.ini
        // *********************
        private void labelSaveFileMsgHookIni_Click(object sender, EventArgs e)
        {

        }


        // If the user changes a setting by clicking the checkbox,
        // update the setting in memory for it can be saved on file later.

        // Group Registry values

        private void checkBoxEnableSwitchUser_CheckedChanged(object sender, EventArgs e)
        {
            int    indexGroup   = IND_RegistryValues;
            int    indexSetting = IND_EnableSwitchUser;
            bool booleanChecked = checkBoxEnableSwitchUser.Checked;
            oldSetting[indexGroup, indexSetting] = newSetting[indexGroup, indexSetting];
            newSetting[indexGroup, indexSetting] = booleanChecked;
        }

        private void checkBoxEnableLockThisComputer_CheckedChanged(object sender, EventArgs e)
        {
            int    indexGroup   = IND_RegistryValues;
            int    indexSetting = IND_EnableLockThisComputer;
            bool booleanChecked = checkBoxEnableLockThisComputer.Checked;
            oldSetting[indexGroup, indexSetting] = newSetting[indexGroup, indexSetting];
            newSetting[indexGroup, indexSetting] = booleanChecked;
        }

        private void checkBoxEnableChangeAPassword_CheckedChanged(object sender, EventArgs e)
        {
            int    indexGroup   = IND_RegistryValues;
            int    indexSetting = IND_EnableChangeAPassword;
            bool booleanChecked = checkBoxEnableChangeAPassword.Checked;
            oldSetting[indexGroup, indexSetting] = newSetting[indexGroup, indexSetting];
            newSetting[indexGroup, indexSetting] = booleanChecked;
        }

        private void checkBoxEnableStartTaskManager_CheckedChanged(object sender, EventArgs e)
        {
            int    indexGroup   = IND_RegistryValues;
            int    indexSetting = IND_EnableStartTaskManager;
            bool booleanChecked = checkBoxEnableStartTaskManager.Checked;
            oldSetting[indexGroup, indexSetting] = newSetting[indexGroup, indexSetting];
            newSetting[indexGroup, indexSetting] = booleanChecked;
        }

        private void checkBoxEnableLogOff_CheckedChanged(object sender, EventArgs e)
        {
            int    indexGroup   = IND_RegistryValues;
            int    indexSetting = IND_EnableLogOff;
            bool booleanChecked = checkBoxEnableLogOff.Checked;
            oldSetting[indexGroup, indexSetting] = newSetting[indexGroup, indexSetting];
            newSetting[indexGroup, indexSetting] = booleanChecked;
        }

        private void checkBoxEnableShutDown_CheckedChanged(object sender, EventArgs e)
        {
            int    indexGroup   = IND_RegistryValues;
            int    indexSetting = IND_EnableShutDown;
            bool booleanChecked = checkBoxEnableShutDown.Checked;
            oldSetting[indexGroup, indexSetting] = newSetting[indexGroup, indexSetting];
            newSetting[indexGroup, indexSetting] = booleanChecked;
        }

        private void checkBoxEnableEaseOfAccess_CheckedChanged(object sender, EventArgs e)
        {
            int    indexGroup   = IND_RegistryValues;
            int    indexSetting = IND_EnableEaseOfAccess;
            bool booleanChecked = checkBoxEnableEaseOfAccess.Checked;
            oldSetting[indexGroup, indexSetting] = newSetting[indexGroup, indexSetting];
            newSetting[indexGroup, indexSetting] = booleanChecked;
        }

        private void checkBoxEnableVmWareClientShade_CheckedChanged(object sender, EventArgs e)
        {
            int    indexGroup   = IND_RegistryValues;
            int    indexSetting = IND_EnableVmWareClientShade;
            bool booleanChecked = checkBoxEnableVmWareClientShade.Checked;
            oldSetting[indexGroup, indexSetting] = newSetting[indexGroup, indexSetting];
            newSetting[indexGroup, indexSetting] = booleanChecked;
        }


        // Group Security options

        private void checkBoxAllowVirtualMachine_CheckedChanged(object sender, EventArgs e)
        {
            int    indexGroup   = IND_SecurityOptions;
            int    indexSetting = IND_AllowVirtualMachine;
            bool booleanChecked = checkBoxAllowVirtualMachine.Checked;
            oldSetting[indexGroup, indexSetting] = newSetting[indexGroup, indexSetting];
            newSetting[indexGroup, indexSetting] = booleanChecked;
        }

        private void checkBoxForceWindowsService_CheckedChanged(object sender, EventArgs e)
        {
            int    indexGroup   = IND_SecurityOptions;
            int    indexSetting = IND_ForceWindowsService;
            bool booleanChecked = checkBoxForceWindowsService.Checked;
            oldSetting[indexGroup, indexSetting] = newSetting[indexGroup, indexSetting];
            newSetting[indexGroup, indexSetting] = booleanChecked;
        }

        private void checkBoxWriteLogFileSebStarterLog_CheckedChanged(object sender, EventArgs e)
        {
            int    indexGroup   = IND_SecurityOptions;
            int    indexSetting = IND_WriteLogFileSebStarterLog;
            bool booleanChecked = checkBoxWriteLogFileSebStarterLog.Checked;
            oldSetting[indexGroup, indexSetting] = newSetting[indexGroup, indexSetting];
            newSetting[indexGroup, indexSetting] = booleanChecked;
        }

        private void checkBoxCreateNewDesktop_CheckedChanged(object sender, EventArgs e)
        {
            int    indexGroup   = IND_SecurityOptions;
            int    indexSetting = IND_CreateNewDesktop;
            bool booleanChecked = checkBoxCreateNewDesktop.Checked;
            oldSetting[indexGroup, indexSetting] = newSetting[indexGroup, indexSetting];
            newSetting[indexGroup, indexSetting] = booleanChecked;
        }

        private void checkBoxShowSebApplicationChooser_CheckedChanged(object sender, EventArgs e)
        {
            int    indexGroup   = IND_SecurityOptions;
            int    indexSetting = IND_ShowSebApplicationChooser;
            bool booleanChecked = checkBoxShowSebApplicationChooser.Checked;
            oldSetting[indexGroup, indexSetting] = newSetting[indexGroup, indexSetting];
            newSetting[indexGroup, indexSetting] = booleanChecked;
        }

        private void checkBoxHookMessages_CheckedChanged(object sender, EventArgs e)
        {
            int    indexGroup   = IND_SecurityOptions;
            int    indexSetting = IND_HookMessages;
            bool booleanChecked = checkBoxHookMessages.Checked;
            oldSetting[indexGroup, indexSetting] = newSetting[indexGroup, indexSetting];
            newSetting[indexGroup, indexSetting] = booleanChecked;
        }

        private void checkBoxEditRegistry_CheckedChanged(object sender, EventArgs e)
        {
            int    indexGroup   = IND_SecurityOptions;
            int    indexSetting = IND_EditRegistry;
            bool booleanChecked = checkBoxEditRegistry.Checked;
            oldSetting[indexGroup, indexSetting] = newSetting[indexGroup, indexSetting];
            newSetting[indexGroup, indexSetting] = booleanChecked;
        }

        private void checkBoxMonitorProcesses_CheckedChanged(object sender, EventArgs e)
        {
            int    indexGroup   = IND_SecurityOptions;
            int    indexSetting = IND_MonitorProcesses;
            bool booleanChecked = checkBoxMonitorProcesses.Checked;
            oldSetting[indexGroup, indexSetting] = newSetting[indexGroup, indexSetting];
            newSetting[indexGroup, indexSetting] = booleanChecked;
        }

        private void checkBoxShutdownAfterAutostartProcessTerminates_CheckedChanged(object sender, EventArgs e)
        {
            int    indexGroup   = IND_SecurityOptions;
            int    indexSetting = IND_ShutdownAfterAutostart;
            bool booleanChecked = checkBoxShutdownAfterAutostart.Checked;
            oldSetting[indexGroup, indexSetting] = newSetting[indexGroup, indexSetting];
            newSetting[indexGroup, indexSetting] = booleanChecked;
        }



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

            // Assign the old settings from the ini file to the widgets again
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
            checkBoxShutdownAfterAutostart.Checked = oldSetting[IND_SecurityOptions, IND_ShutdownAfterAutostart];

            textBoxSebBrowser           .Text = oldStringSebBrowser;
            textBoxAutostartProcess     .Text = oldStringAutostartProcess;
            textBoxExamUrl              .Text = oldStringExamUrl;
            textBoxPermittedApplications.Text = oldStringPermittedApplications;
        }



        private void textBoxSebBrowser_TextChanged(object sender, EventArgs e)
        {
            oldStringSebBrowser = newStringSebBrowser;
            newStringSebBrowser = textBoxSebBrowser.Text;
        }

        private void textBoxAutostartProcess_TextChanged(object sender, EventArgs e)
        {
            oldStringAutostartProcess = newStringAutostartProcess;
            newStringAutostartProcess = textBoxAutostartProcess.Text;
        }

        private void textBoxExamUrl_TextChanged(object sender, EventArgs e)
        {
            oldStringExamUrl = newStringExamUrl;
            newStringExamUrl = textBoxExamUrl.Text;
        }

        private void textBoxPermittedApplications_TextChanged(object sender, EventArgs e)
        {
            oldStringPermittedApplications = newStringPermittedApplications;
            newStringPermittedApplications = textBoxPermittedApplications.Text;
        }

    }
}
