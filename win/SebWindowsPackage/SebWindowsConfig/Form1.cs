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
    public partial class Form1 : Form
    {
        // Constants for indexing the ini file settings
        const int IND_HideFastUserSwitching  = 0;
        const int IND_DisableLockWorkstation = 1;
        const int IND_DisableChangePassword  = 2;
        const int IND_DisableTaskMgr         = 3;
        const int IND_NoLogoff               = 4;
        const int IND_NoClose                = 5;
        const int IND_EnableShade            = 6;
        const int IND_EnableEaseOfAccess     = 7;

        const int IND_RegistrySettingNone = -1;
        const int IND_RegistrySettingMin  =  0;
        const int IND_RegistrySettingMax  =  7;
        const int IND_RegistrySettingNum  =  8;

        const String MSG_HideFastUserSwitching  = "HideFastUserSwitching ";
        const String MSG_DisableLockWorkstation = "DisableLockWorkstation";
        const String MSG_DisableChangePassword  = "DisableChangePassword ";
        const String MSG_DisableTaskMgr         = "DisableTaskMgr        ";
        const String MSG_NoLogoff               = "NoLogoff              ";
        const String MSG_NoClose                = "NoClose               ";
        const String MSG_EnableShade            = "EnableShade           ";
        const String MSG_EnableEaseOfAccess     = "Debugger              ";

        const String TYPE_HideFastUserSwitching  = "REG_DWORD";
        const String TYPE_DisableLockWorkstation = "REG_DWORD";
        const String TYPE_DisableChangePassword  = "REG_DWORD";
        const String TYPE_DisableTaskMgr         = "REG_DWORD";
        const String TYPE_NoLogoff               = "REG_DWORD";
        const String TYPE_NoClose                = "REG_DWORD";
        const String TYPE_EnableShade            = "REG_DWORD";
        const String TYPE_EnableEaseOfAccess     = "REG_SZ";

        // Global variables

        // Names of registry domains, keys, values, types
        static String[]  msgString = new String[IND_RegistrySettingNum + 1];
        static String[] typeString = new String[IND_RegistrySettingNum + 1];

        // Registry settings as booleans (true or false)
        static Boolean[] defSetting = new Boolean[IND_RegistrySettingNum + 1];
        static Boolean[] oldSetting = new Boolean[IND_RegistrySettingNum + 1];
        static Boolean[] newSetting = new Boolean[IND_RegistrySettingNum + 1];

        static Boolean[]  allowSetting = new Boolean[IND_RegistrySettingNum + 1];
        static Boolean[] forbidSetting = new Boolean[IND_RegistrySettingNum + 1];

        String sebStarterIniPath = "";
        String sebMsgHookIniPath = "";
        StreamReader streamReaderSebStarterIni;
        StreamWriter streamWriterSebStarterIni;
        Boolean  enableTaskManager = false;
        Boolean disableTaskManager = true;



        // ***********
        // Constructor
        // ***********
        public Form1()
        {
            InitializeComponent();

            // Initialise the global arrays

            int index;
            for (index = IND_RegistrySettingMin; index <= IND_RegistrySettingMax; index++)
            {
                oldSetting[index] = false;
                newSetting[index] = false;
            }

            defSetting[IND_HideFastUserSwitching ] = true;
            defSetting[IND_DisableLockWorkstation] = true;
            defSetting[IND_DisableChangePassword ] = true;
            defSetting[IND_DisableTaskMgr        ] = true;
            defSetting[IND_NoLogoff              ] = true;
            defSetting[IND_NoClose               ] = true;
            defSetting[IND_EnableShade           ] = false;
            defSetting[IND_EnableEaseOfAccess    ] = false;

            allowSetting[IND_HideFastUserSwitching ] = false;
            allowSetting[IND_DisableLockWorkstation] = false;
            allowSetting[IND_DisableChangePassword ] = false;
            allowSetting[IND_DisableTaskMgr        ] = false;
            allowSetting[IND_NoLogoff              ] = false;
            allowSetting[IND_NoClose               ] = false;
            allowSetting[IND_EnableShade           ] = true;
            allowSetting[IND_EnableEaseOfAccess    ] = true;

            forbidSetting[IND_HideFastUserSwitching ] = true;
            forbidSetting[IND_DisableLockWorkstation] = true;
            forbidSetting[IND_DisableChangePassword ] = true;
            forbidSetting[IND_DisableTaskMgr        ] = true;
            forbidSetting[IND_NoLogoff              ] = true;
            forbidSetting[IND_NoClose               ] = true;
            forbidSetting[IND_EnableShade           ] = false;
            forbidSetting[IND_EnableEaseOfAccess    ] = false;

            msgString[IND_HideFastUserSwitching ] = MSG_HideFastUserSwitching;
            msgString[IND_DisableLockWorkstation] = MSG_DisableLockWorkstation;
            msgString[IND_DisableChangePassword ] = MSG_DisableChangePassword;
            msgString[IND_DisableTaskMgr        ] = MSG_DisableTaskMgr;
            msgString[IND_NoLogoff              ] = MSG_NoLogoff;
            msgString[IND_NoClose               ] = MSG_NoClose;
            msgString[IND_EnableShade           ] = MSG_EnableShade;
            msgString[IND_EnableEaseOfAccess    ] = MSG_EnableEaseOfAccess;

            typeString[IND_HideFastUserSwitching ] = TYPE_HideFastUserSwitching;
            typeString[IND_DisableLockWorkstation] = TYPE_DisableLockWorkstation;
            typeString[IND_DisableChangePassword ] = TYPE_DisableChangePassword;
            typeString[IND_DisableTaskMgr        ] = TYPE_DisableTaskMgr;
            typeString[IND_NoLogoff              ] = TYPE_NoLogoff;
            typeString[IND_NoClose               ] = TYPE_NoClose;
            typeString[IND_EnableShade           ] = TYPE_EnableShade;
            typeString[IND_EnableEaseOfAccess    ] = TYPE_EnableEaseOfAccess;

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            checkBox2.Checked = !checkBox1.Checked;
        }

        private void labelBrowseSebStarterFolder_Click(object sender, EventArgs e)
        {
            DialogResult browseFolderName = folderBrowserDialogBrowseSebStarterIni.ShowDialog();
        }

        private void labelOpenSebStarterIniFile_Click(object sender, EventArgs e)
        {
            DialogResult openFileDialogResult = openFileDialogSebStarterIni.ShowDialog();
            sebStarterIniPath                 = openFileDialogSebStarterIni.FileName;
            labelSebStarterIniPath.Text       = sebStarterIniPath;

            try 
            {
                // Create an instance of StreamReader to read from a file.
                // The using statement also closes the StreamReader.
                using (StreamReader sr = new StreamReader(sebStarterIniPath)) 
                {
                    String line;
                    // Read and display lines from the file until the end of 
                    // the file is reached.
                    while ((line = sr.ReadLine()) != null) 
                    {
                        Console.WriteLine(line);
                        labelCurrentLine.Text = line;

                        if (line.Contains("="))
                        {
                            int     equalPos = line.IndexOf  ("=");
                            String  leftSide = line.Remove   (equalPos);
                            String rightSide = line.Substring(equalPos + 1);
                            labelLeftSide.Text  =  leftSide;
                            labelRightSide.Text = rightSide;

                            if (leftSide.Equals("REG_DISABLE_TASKMGR"))
                            {
                                if (rightSide.Equals("0")) disableTaskManager = false;
                                if (rightSide.Equals("1")) disableTaskManager = true;
                                enableTaskManager = !disableTaskManager;
                                checkBoxTaskManager.Checked = enableTaskManager;
                            }
                        }
                    }
                }
            }
            catch (Exception streamReadException) 
            {
                // Let the user know what went wrong.
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(streamReadException.Message);
            }

        }

        private void labelSaveSebStarterIniFile_Click(object sender, EventArgs e)
        {
            DialogResult saveFileName = saveFileDialogSebStarterIni.ShowDialog();
        }

        private void checkBoxTaskManager_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
