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
        const int IND_EnableSwitchUser        = 0;
        const int IND_EnableLockThisComputer  = 1;
        const int IND_EnableChangeAPassword   = 2;
        const int IND_EnableStartTaskManager  = 3;
        const int IND_EnableLogOff            = 4;
        const int IND_EnableShutDown          = 5;
        const int IND_EnableEaseOfAccess      = 6;
        const int IND_EnableVmWareClientShade = 7;

        const int IND_RegistrySettingNone = -1;
        const int IND_RegistrySettingMin  =  0;
        const int IND_RegistrySettingMax  =  7;
        const int IND_RegistrySettingNum  =  8;

        const String MSG_EnableSwitchUser        = "EnableSwitchUser";
        const String MSG_EnableLockThisComputer  = "EnableLockThisComputer";
        const String MSG_EnableChangeAPassword   = "EnableChangeAPassword";
        const String MSG_EnableStartTaskManager  = "EnableStartTaskManager";
        const String MSG_EnableLogOff            = "EnableLogOff";
        const String MSG_EnableShutDown          = "EnableShutDown";
        const String MSG_EnableEaseOfAccess      = "EnableEaseOfAccess";
        const String MSG_EnableVmWareClientShade = "EnableVmWareClientShade";

        const String MSG_ExamUrl               = "ExamUrl";
        const String MSG_PermittedApplications = "PermittedApplications";

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

        // Names of registry domains, keys, values, types
        static String[]  msgString = new String[IND_RegistrySettingNum + 1];
      //static String[] typeString = new String[IND_RegistrySettingNum + 1];

        // Registry settings as booleans (true or false)
        static Boolean[] defSetting = new Boolean[IND_RegistrySettingNum + 1];
        static Boolean[] oldSetting = new Boolean[IND_RegistrySettingNum + 1];
        static Boolean[] newSetting = new Boolean[IND_RegistrySettingNum + 1];

        static Boolean[]  allowSetting = new Boolean[IND_RegistrySettingNum + 1];
        static Boolean[] forbidSetting = new Boolean[IND_RegistrySettingNum + 1];

        String oldStringExamUrl = "";
        String newStringExamUrl = "";
        String msgStringExamUrl = "";
        String oldStringPermittedApplications = "";
        String newStringPermittedApplications = "";
        String msgStringPermittedApplications = "";

        String       sebStarterIniPath = "";
        String       sebMsgHookIniPath = "";
        StreamReader streamReaderSebStarterIni;
        StreamReader streamReaderSebMsgHookIni;
        StreamWriter streamWriterSebStarterIni;
        StreamWriter streamWriterSebMsgHookIni;



        // ***********
        // Constructor
        // ***********
        public SebWindowsConfigForm()
        {
            InitializeComponent();

            // Initialise the global arrays

            int  index;
            for (index = IND_RegistrySettingMin; index <= IND_RegistrySettingMax; index++)
            {
                   oldSetting[index] = false;
                   newSetting[index] = false;
                   defSetting[index] = false;
                 allowSetting[index] = true;
                forbidSetting[index] = false;
            }

            msgString[IND_EnableSwitchUser       ] = MSG_EnableSwitchUser;
            msgString[IND_EnableLockThisComputer ] = MSG_EnableLockThisComputer;
            msgString[IND_EnableChangeAPassword  ] = MSG_EnableChangeAPassword;
            msgString[IND_EnableStartTaskManager ] = MSG_EnableStartTaskManager;
            msgString[IND_EnableLogOff           ] = MSG_EnableLogOff;
            msgString[IND_EnableShutDown         ] = MSG_EnableShutDown;
            msgString[IND_EnableEaseOfAccess     ] = MSG_EnableEaseOfAccess;
            msgString[IND_EnableVmWareClientShade] = MSG_EnableVmWareClientShade;

            msgStringExamUrl               = MSG_ExamUrl;
            msgStringPermittedApplications = MSG_PermittedApplications;

/*
            typeString[IND_EnableSwitchUser       ] = TYPE_EnableSwitchUser;
            typeString[IND_EnableLockThisComputer ] = TYPE_EnableLockThisComputer;
            typeString[IND_EnableChangeAPassword  ] = TYPE_EnableChangeAPassword;
            typeString[IND_EnableStartTaskManager ] = TYPE_EnableStartTaskManager;
            typeString[IND_EnableLogOff           ] = TYPE_EnableLogOff;
            typeString[IND_EnableShutDown         ] = TYPE_EnableShutDown;
            typeString[IND_EnableEaseOfAccess     ] = TYPE_EnableEaseOfAccess;
            typeString[IND_EnableVmWareClientShade] = TYPE_EnableVmWareClientShade;
*/
        } // end of contructor   SebWindowsConfigForm()



        private void labelBrowseSebStarterFolder_Click(object sender, EventArgs e)
        {
            DialogResult browseFolderName = folderBrowserDialogBrowseIniFiles.ShowDialog();
        }



        private void labelOpenSebStarterIniFile_Click(object sender, EventArgs e)
        {
            DialogResult openFileDialogResult = openFileDialogSebStarterIni.ShowDialog();
            sebStarterIniPath                 = openFileDialogSebStarterIni.FileName;

            try 
            {
                // Create an instance of StreamReader to read from a file.
                StreamReader streamReader = new StreamReader(sebStarterIniPath);
                String       line;

                // Read and display lines from the file until the end of 
                // the file is reached.
                while ((line = streamReader.ReadLine()) != null) 
                {
                    Console.WriteLine(line);

                    if (line.Contains("="))
                    {
                        int     equalPos    = line.IndexOf  ("=");
                        String  leftSide    = line.Remove   (equalPos);
                        String rightSide    = line.Substring(equalPos + 1);

                        int  index;
                        for (index = IND_RegistrySettingMin; index <= IND_RegistrySettingMax; index++)
                        {
                            if (leftSide.Equals(msgString[index]))
                            {
                                Boolean rightBool = false;
                                if (rightSide.Equals("0")) rightBool = false;
                                if (rightSide.Equals("1")) rightBool = true;
                                newSetting[index] = rightBool;
                            }
                        }

                        if (leftSide.Equals(msgStringExamUrl))
                        {
                            newStringExamUrl = rightSide;
                        }

                        if (leftSide.Equals(msgStringPermittedApplications))
                        {
                            newStringPermittedApplications = rightSide;
                        }

                    } // end if line.Contains("=")
                } // end while


                // Close the StreamReader
                streamReader.Close();

                // Assign the settings from the ini file to the widgets
                checkBoxEnableSwitchUser       .Checked = newSetting[IND_EnableSwitchUser];
                checkBoxEnableLockThisComputer .Checked = newSetting[IND_EnableLockThisComputer];
                checkBoxEnableChangeAPassword  .Checked = newSetting[IND_EnableChangeAPassword];
                checkBoxEnableStartTaskManager .Checked = newSetting[IND_EnableStartTaskManager];
                checkBoxEnableLogOff           .Checked = newSetting[IND_EnableLogOff];
                checkBoxEnableShutDown         .Checked = newSetting[IND_EnableShutDown];
                checkBoxEnableEaseOfAccess     .Checked = newSetting[IND_EnableEaseOfAccess];
                checkBoxEnableVmWareClientShade.Checked = newSetting[IND_EnableVmWareClientShade];

                textBoxExamUrl              .Text = newStringExamUrl;
                textBoxPermittedApplications.Text = newStringPermittedApplications;

            } // end try
            catch (Exception streamReadException) 
            {
                // Let the user know what went wrong.
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(streamReadException.Message);
            }

        } // end of method   labelOpenSebStarterIniFile_Click()



        private void labelSaveSebStarterIniFile_Click(object sender, EventArgs e)
        {
            DialogResult saveFileName = saveFileDialogSebStarterIni.ShowDialog();
        }


        private void labelOpenMsgHookIniFile_Click(object sender, EventArgs e)
        {

        }

        private void labelSaveMsgHookIniFile_Click(object sender, EventArgs e)
        {

        }


        // If the user changes a setting by clicking the checkbox,
        // update the setting in memory for it can be saved on file later.

        private void checkBoxEnableSwitchUser_CheckedChanged(object sender, EventArgs e)
        {
            oldSetting[IND_EnableSwitchUser] = newSetting[IND_EnableSwitchUser];
            newSetting[IND_EnableSwitchUser] = checkBoxEnableSwitchUser.Checked;
        }

        private void checkBoxEnableLockThisComputer_CheckedChanged(object sender, EventArgs e)
        {
            oldSetting[IND_EnableLockThisComputer] = newSetting[IND_EnableLockThisComputer];
            newSetting[IND_EnableLockThisComputer] = checkBoxEnableLockThisComputer.Checked;
        }

        private void checkBoxEnableChangeAPassword_CheckedChanged(object sender, EventArgs e)
        {
            oldSetting[IND_EnableChangeAPassword] = newSetting[IND_EnableChangeAPassword];
            newSetting[IND_EnableChangeAPassword] = checkBoxEnableChangeAPassword.Checked;
        }

        private void checkBoxEnableStartTaskManager_CheckedChanged(object sender, EventArgs e)
        {
            oldSetting[IND_EnableStartTaskManager] = newSetting[IND_EnableStartTaskManager];
            newSetting[IND_EnableStartTaskManager] = checkBoxEnableStartTaskManager.Checked;
        }

        private void checkBoxEnableLogOff_CheckedChanged(object sender, EventArgs e)
        {
            oldSetting[IND_EnableLogOff] = newSetting[IND_EnableLogOff];
            newSetting[IND_EnableLogOff] = checkBoxEnableLogOff.Checked;
        }

        private void checkBoxEnableShutDown_CheckedChanged(object sender, EventArgs e)
        {
            oldSetting[IND_EnableShutDown] = newSetting[IND_EnableShutDown];
            newSetting[IND_EnableShutDown] = checkBoxEnableShutDown.Checked;
        }

        private void checkBoxEnableEaseOfAccess_CheckedChanged(object sender, EventArgs e)
        {
            oldSetting[IND_EnableEaseOfAccess] = newSetting[IND_EnableEaseOfAccess];
            newSetting[IND_EnableEaseOfAccess] = checkBoxEnableEaseOfAccess.Checked;
        }

        private void checkBoxEnableVmWareClientShade_CheckedChanged(object sender, EventArgs e)
        {
            oldSetting[IND_EnableVmWareClientShade] = newSetting[IND_EnableVmWareClientShade];
            newSetting[IND_EnableVmWareClientShade] = checkBoxEnableVmWareClientShade.Checked;
        }


        private void buttonRestoreSettingsOfSebStarterIni_Click(object sender, EventArgs e)
        {
            int  index;
            for (index = IND_RegistrySettingMin; index <= IND_RegistrySettingMax; index++)
            {
                newSetting[index] = oldSetting[index];
            }

            newStringExamUrl               = oldStringExamUrl;
            newStringPermittedApplications = oldStringPermittedApplications;

            // Assign the old settings from the ini file to the widgets again
            checkBoxEnableSwitchUser       .Checked = oldSetting[IND_EnableSwitchUser];
            checkBoxEnableLockThisComputer .Checked = oldSetting[IND_EnableLockThisComputer];
            checkBoxEnableChangeAPassword  .Checked = oldSetting[IND_EnableChangeAPassword];
            checkBoxEnableStartTaskManager .Checked = oldSetting[IND_EnableStartTaskManager];
            checkBoxEnableLogOff           .Checked = oldSetting[IND_EnableLogOff];
            checkBoxEnableShutDown         .Checked = oldSetting[IND_EnableShutDown];
            checkBoxEnableEaseOfAccess     .Checked = oldSetting[IND_EnableEaseOfAccess];
            checkBoxEnableVmWareClientShade.Checked = oldSetting[IND_EnableVmWareClientShade];

            textBoxExamUrl              .Text = oldStringExamUrl;
            textBoxPermittedApplications.Text = oldStringPermittedApplications;
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

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

    }
}
