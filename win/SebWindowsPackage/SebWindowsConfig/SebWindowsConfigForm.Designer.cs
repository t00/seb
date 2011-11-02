namespace SebWindowsConfig
{
    partial class SebWindowsConfigForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SebWindowsConfigForm));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.groupBoxFurtherSettings = new System.Windows.Forms.GroupBox();
            this.checkBoxShutdownAfterAutostartProcessTerminates = new System.Windows.Forms.CheckBox();
            this.checkBoxMonitorProcesses = new System.Windows.Forms.CheckBox();
            this.checkBoxEditRegistry = new System.Windows.Forms.CheckBox();
            this.checkBoxHookMessages = new System.Windows.Forms.CheckBox();
            this.checkBoxShowSebApplicationChooser = new System.Windows.Forms.CheckBox();
            this.checkBoxCreateNewDesktop = new System.Windows.Forms.CheckBox();
            this.checkBoxAllowVirtualMachine = new System.Windows.Forms.CheckBox();
            this.checkBoxWriteLogFileSebStarterLog = new System.Windows.Forms.CheckBox();
            this.checkBoxForceWindowsService = new System.Windows.Forms.CheckBox();
            this.groupBoxSebStarterIni = new System.Windows.Forms.GroupBox();
            this.labelOpenFileSebStarterIni = new System.Windows.Forms.Label();
            this.labelSaveFileSebStarterIni = new System.Windows.Forms.Label();
            this.buttonRestoreSettingsOfSebStarterIni = new System.Windows.Forms.Button();
            this.groupBoxOnlineExam = new System.Windows.Forms.GroupBox();
            this.textBoxAutostartProcess = new System.Windows.Forms.TextBox();
            this.labelSebBrowser = new System.Windows.Forms.Label();
            this.labelAutostartProcess = new System.Windows.Forms.Label();
            this.textBoxSebBrowser = new System.Windows.Forms.TextBox();
            this.labelExamUrl = new System.Windows.Forms.Label();
            this.labelPermittedApplications = new System.Windows.Forms.Label();
            this.textBoxPermittedApplications = new System.Windows.Forms.TextBox();
            this.textBoxExamUrl = new System.Windows.Forms.TextBox();
            this.groupBoxWindowsSecurityScreen = new System.Windows.Forms.GroupBox();
            this.checkBoxEnableSwitchUser = new System.Windows.Forms.CheckBox();
            this.checkBoxEnableLockThisComputer = new System.Windows.Forms.CheckBox();
            this.checkBoxEnableChangeAPassword = new System.Windows.Forms.CheckBox();
            this.checkBoxEnableStartTaskManager = new System.Windows.Forms.CheckBox();
            this.checkBoxEnableLogOff = new System.Windows.Forms.CheckBox();
            this.checkBoxEnableShutDown = new System.Windows.Forms.CheckBox();
            this.checkBoxEnableEaseOfAccess = new System.Windows.Forms.CheckBox();
            this.checkBoxEnableVmWareClientShade = new System.Windows.Forms.CheckBox();
            this.checkBoxWriteLogFileMsgHookLog = new System.Windows.Forms.CheckBox();
            this.groupBoxMsgHookIni = new System.Windows.Forms.GroupBox();
            this.labelOpenFileMsgHookIni = new System.Windows.Forms.Label();
            this.labelSaveFileMsgHookIni = new System.Windows.Forms.Label();
            this.groupBoxFunctionKeys = new System.Windows.Forms.GroupBox();
            this.checkBoxF1 = new System.Windows.Forms.CheckBox();
            this.checkBoxF2 = new System.Windows.Forms.CheckBox();
            this.checkBoxF12 = new System.Windows.Forms.CheckBox();
            this.checkBoxF3 = new System.Windows.Forms.CheckBox();
            this.checkBoxF11 = new System.Windows.Forms.CheckBox();
            this.checkBoxF4 = new System.Windows.Forms.CheckBox();
            this.checkBoxF5 = new System.Windows.Forms.CheckBox();
            this.checkBox10 = new System.Windows.Forms.CheckBox();
            this.checkBoxF6 = new System.Windows.Forms.CheckBox();
            this.checkBoxF9 = new System.Windows.Forms.CheckBox();
            this.checkBoxF7 = new System.Windows.Forms.CheckBox();
            this.checkBoxF8 = new System.Windows.Forms.CheckBox();
            this.groupBoxSpecialKeys = new System.Windows.Forms.GroupBox();
            this.checkBoxEsc = new System.Windows.Forms.CheckBox();
            this.checkBoxCtrlEsc = new System.Windows.Forms.CheckBox();
            this.checkBoxAltEsc = new System.Windows.Forms.CheckBox();
            this.checkBoxAltTab = new System.Windows.Forms.CheckBox();
            this.checkBoxAltF4 = new System.Windows.Forms.CheckBox();
            this.checkBoxStartMenu = new System.Windows.Forms.CheckBox();
            this.checkBoxRightMouse = new System.Windows.Forms.CheckBox();
            this.folderBrowserDialogBrowseIniFiles = new System.Windows.Forms.FolderBrowserDialog();
            this.openFileDialogSebStarterIni = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialogSebStarterIni = new System.Windows.Forms.SaveFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBoxFurtherSettings.SuspendLayout();
            this.groupBoxSebStarterIni.SuspendLayout();
            this.groupBoxOnlineExam.SuspendLayout();
            this.groupBoxWindowsSecurityScreen.SuspendLayout();
            this.groupBoxMsgHookIni.SuspendLayout();
            this.groupBoxFunctionKeys.SuspendLayout();
            this.groupBoxSpecialKeys.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.groupBoxFurtherSettings);
            this.splitContainer1.Panel1.Controls.Add(this.groupBoxSebStarterIni);
            this.splitContainer1.Panel1.Controls.Add(this.groupBoxOnlineExam);
            this.splitContainer1.Panel1.Controls.Add(this.groupBoxWindowsSecurityScreen);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.checkBoxWriteLogFileMsgHookLog);
            this.splitContainer1.Panel2.Controls.Add(this.groupBoxMsgHookIni);
            this.splitContainer1.Panel2.Controls.Add(this.groupBoxFunctionKeys);
            this.splitContainer1.Panel2.Controls.Add(this.groupBoxSpecialKeys);
            this.splitContainer1.Size = new System.Drawing.Size(1435, 655);
            this.splitContainer1.SplitterDistance = 708;
            this.splitContainer1.TabIndex = 0;
            // 
            // groupBoxFurtherSettings
            // 
            this.groupBoxFurtherSettings.Controls.Add(this.checkBoxShutdownAfterAutostartProcessTerminates);
            this.groupBoxFurtherSettings.Controls.Add(this.checkBoxMonitorProcesses);
            this.groupBoxFurtherSettings.Controls.Add(this.checkBoxEditRegistry);
            this.groupBoxFurtherSettings.Controls.Add(this.checkBoxHookMessages);
            this.groupBoxFurtherSettings.Controls.Add(this.checkBoxShowSebApplicationChooser);
            this.groupBoxFurtherSettings.Controls.Add(this.checkBoxCreateNewDesktop);
            this.groupBoxFurtherSettings.Controls.Add(this.checkBoxAllowVirtualMachine);
            this.groupBoxFurtherSettings.Controls.Add(this.checkBoxWriteLogFileSebStarterLog);
            this.groupBoxFurtherSettings.Controls.Add(this.checkBoxForceWindowsService);
            this.groupBoxFurtherSettings.Location = new System.Drawing.Point(343, 166);
            this.groupBoxFurtherSettings.Name = "groupBoxFurtherSettings";
            this.groupBoxFurtherSettings.Size = new System.Drawing.Size(319, 284);
            this.groupBoxFurtherSettings.TabIndex = 45;
            this.groupBoxFurtherSettings.TabStop = false;
            this.groupBoxFurtherSettings.Text = "Further settings";
            // 
            // checkBoxShutdownAfterAutostartProcessTerminates
            // 
            this.checkBoxShutdownAfterAutostartProcessTerminates.AutoSize = true;
            this.checkBoxShutdownAfterAutostartProcessTerminates.Enabled = false;
            this.checkBoxShutdownAfterAutostartProcessTerminates.Location = new System.Drawing.Point(6, 253);
            this.checkBoxShutdownAfterAutostartProcessTerminates.Name = "checkBoxShutdownAfterAutostartProcessTerminates";
            this.checkBoxShutdownAfterAutostartProcessTerminates.Size = new System.Drawing.Size(309, 21);
            this.checkBoxShutdownAfterAutostartProcessTerminates.TabIndex = 50;
            this.checkBoxShutdownAfterAutostartProcessTerminates.Text = "Shutdown after autostart process terminates";
            this.checkBoxShutdownAfterAutostartProcessTerminates.UseVisualStyleBackColor = true;
            this.checkBoxShutdownAfterAutostartProcessTerminates.CheckedChanged += new System.EventHandler(this.checkBoxShutdownAfterAutostartProcessTerminates_CheckedChanged);
            // 
            // checkBoxMonitorProcesses
            // 
            this.checkBoxMonitorProcesses.AutoSize = true;
            this.checkBoxMonitorProcesses.Enabled = false;
            this.checkBoxMonitorProcesses.Location = new System.Drawing.Point(6, 226);
            this.checkBoxMonitorProcesses.Name = "checkBoxMonitorProcesses";
            this.checkBoxMonitorProcesses.Size = new System.Drawing.Size(146, 21);
            this.checkBoxMonitorProcesses.TabIndex = 49;
            this.checkBoxMonitorProcesses.Text = "Monitor processes";
            this.checkBoxMonitorProcesses.UseVisualStyleBackColor = true;
            this.checkBoxMonitorProcesses.CheckedChanged += new System.EventHandler(this.checkBoxMonitorProcesses_CheckedChanged);
            // 
            // checkBoxEditRegistry
            // 
            this.checkBoxEditRegistry.AutoSize = true;
            this.checkBoxEditRegistry.Enabled = false;
            this.checkBoxEditRegistry.Location = new System.Drawing.Point(6, 199);
            this.checkBoxEditRegistry.Name = "checkBoxEditRegistry";
            this.checkBoxEditRegistry.Size = new System.Drawing.Size(105, 21);
            this.checkBoxEditRegistry.TabIndex = 48;
            this.checkBoxEditRegistry.Text = "Edit registry";
            this.checkBoxEditRegistry.UseVisualStyleBackColor = true;
            this.checkBoxEditRegistry.CheckedChanged += new System.EventHandler(this.checkBoxEditRegistry_CheckedChanged);
            // 
            // checkBoxHookMessages
            // 
            this.checkBoxHookMessages.AutoSize = true;
            this.checkBoxHookMessages.Enabled = false;
            this.checkBoxHookMessages.Location = new System.Drawing.Point(6, 172);
            this.checkBoxHookMessages.Name = "checkBoxHookMessages";
            this.checkBoxHookMessages.Size = new System.Drawing.Size(131, 21);
            this.checkBoxHookMessages.TabIndex = 47;
            this.checkBoxHookMessages.Text = "Hook messages";
            this.checkBoxHookMessages.UseVisualStyleBackColor = true;
            this.checkBoxHookMessages.CheckedChanged += new System.EventHandler(this.checkBoxHookMessages_CheckedChanged);
            // 
            // checkBoxShowSebApplicationChooser
            // 
            this.checkBoxShowSebApplicationChooser.AutoSize = true;
            this.checkBoxShowSebApplicationChooser.Location = new System.Drawing.Point(6, 145);
            this.checkBoxShowSebApplicationChooser.Name = "checkBoxShowSebApplicationChooser";
            this.checkBoxShowSebApplicationChooser.Size = new System.Drawing.Size(222, 21);
            this.checkBoxShowSebApplicationChooser.TabIndex = 46;
            this.checkBoxShowSebApplicationChooser.Text = "Show SEB application chooser";
            this.checkBoxShowSebApplicationChooser.UseVisualStyleBackColor = true;
            this.checkBoxShowSebApplicationChooser.CheckedChanged += new System.EventHandler(this.checkBoxShowSebApplicationChooser_CheckedChanged);
            // 
            // checkBoxCreateNewDesktop
            // 
            this.checkBoxCreateNewDesktop.AutoSize = true;
            this.checkBoxCreateNewDesktop.Location = new System.Drawing.Point(6, 118);
            this.checkBoxCreateNewDesktop.Name = "checkBoxCreateNewDesktop";
            this.checkBoxCreateNewDesktop.Size = new System.Drawing.Size(155, 21);
            this.checkBoxCreateNewDesktop.TabIndex = 45;
            this.checkBoxCreateNewDesktop.Text = "Create new desktop";
            this.checkBoxCreateNewDesktop.UseVisualStyleBackColor = true;
            this.checkBoxCreateNewDesktop.CheckedChanged += new System.EventHandler(this.checkBoxCreateNewDesktop_CheckedChanged);
            // 
            // checkBoxAllowVirtualMachine
            // 
            this.checkBoxAllowVirtualMachine.AutoSize = true;
            this.checkBoxAllowVirtualMachine.Location = new System.Drawing.Point(6, 36);
            this.checkBoxAllowVirtualMachine.Name = "checkBoxAllowVirtualMachine";
            this.checkBoxAllowVirtualMachine.Size = new System.Drawing.Size(161, 21);
            this.checkBoxAllowVirtualMachine.TabIndex = 43;
            this.checkBoxAllowVirtualMachine.Text = "Allow virtual machine";
            this.checkBoxAllowVirtualMachine.UseVisualStyleBackColor = true;
            this.checkBoxAllowVirtualMachine.CheckedChanged += new System.EventHandler(this.checkBoxAllowVirtualMachine_CheckedChanged);
            // 
            // checkBoxWriteLogFileSebStarterLog
            // 
            this.checkBoxWriteLogFileSebStarterLog.AutoSize = true;
            this.checkBoxWriteLogFileSebStarterLog.Location = new System.Drawing.Point(6, 90);
            this.checkBoxWriteLogFileSebStarterLog.Name = "checkBoxWriteLogFileSebStarterLog";
            this.checkBoxWriteLogFileSebStarterLog.Size = new System.Drawing.Size(199, 21);
            this.checkBoxWriteLogFileSebStarterLog.TabIndex = 42;
            this.checkBoxWriteLogFileSebStarterLog.Text = "Write logfile SebStarter.log";
            this.checkBoxWriteLogFileSebStarterLog.UseVisualStyleBackColor = true;
            this.checkBoxWriteLogFileSebStarterLog.CheckedChanged += new System.EventHandler(this.checkBoxWriteLogFileSebStarterLog_CheckedChanged);
            // 
            // checkBoxForceWindowsService
            // 
            this.checkBoxForceWindowsService.AutoSize = true;
            this.checkBoxForceWindowsService.Location = new System.Drawing.Point(6, 63);
            this.checkBoxForceWindowsService.Name = "checkBoxForceWindowsService";
            this.checkBoxForceWindowsService.Size = new System.Drawing.Size(175, 21);
            this.checkBoxForceWindowsService.TabIndex = 44;
            this.checkBoxForceWindowsService.Text = "Force Windows service";
            this.checkBoxForceWindowsService.UseVisualStyleBackColor = true;
            this.checkBoxForceWindowsService.CheckedChanged += new System.EventHandler(this.checkBoxForceWindowsService_CheckedChanged);
            // 
            // groupBoxSebStarterIni
            // 
            this.groupBoxSebStarterIni.Controls.Add(this.labelOpenFileSebStarterIni);
            this.groupBoxSebStarterIni.Controls.Add(this.labelSaveFileSebStarterIni);
            this.groupBoxSebStarterIni.Controls.Add(this.buttonRestoreSettingsOfSebStarterIni);
            this.groupBoxSebStarterIni.Location = new System.Drawing.Point(54, 38);
            this.groupBoxSebStarterIni.Name = "groupBoxSebStarterIni";
            this.groupBoxSebStarterIni.Size = new System.Drawing.Size(352, 100);
            this.groupBoxSebStarterIni.TabIndex = 26;
            this.groupBoxSebStarterIni.TabStop = false;
            this.groupBoxSebStarterIni.Text = "SebStarter.ini";
            // 
            // labelOpenFileSebStarterIni
            // 
            this.labelOpenFileSebStarterIni.AutoSize = true;
            this.labelOpenFileSebStarterIni.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelOpenFileSebStarterIni.Location = new System.Drawing.Point(6, 31);
            this.labelOpenFileSebStarterIni.Name = "labelOpenFileSebStarterIni";
            this.labelOpenFileSebStarterIni.Size = new System.Drawing.Size(157, 19);
            this.labelOpenFileSebStarterIni.TabIndex = 9;
            this.labelOpenFileSebStarterIni.Text = "Open file SebStarter.ini";
            this.labelOpenFileSebStarterIni.Click += new System.EventHandler(this.labelOpenFileSebStarterIni_Click);
            // 
            // labelSaveFileSebStarterIni
            // 
            this.labelSaveFileSebStarterIni.AutoSize = true;
            this.labelSaveFileSebStarterIni.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelSaveFileSebStarterIni.Location = new System.Drawing.Point(6, 60);
            this.labelSaveFileSebStarterIni.Name = "labelSaveFileSebStarterIni";
            this.labelSaveFileSebStarterIni.Size = new System.Drawing.Size(154, 19);
            this.labelSaveFileSebStarterIni.TabIndex = 10;
            this.labelSaveFileSebStarterIni.Text = "Save file SebStarter.ini";
            this.labelSaveFileSebStarterIni.Click += new System.EventHandler(this.labelSaveFileSebStarterIni_Click);
            // 
            // buttonRestoreSettingsOfSebStarterIni
            // 
            this.buttonRestoreSettingsOfSebStarterIni.Location = new System.Drawing.Point(196, 21);
            this.buttonRestoreSettingsOfSebStarterIni.Name = "buttonRestoreSettingsOfSebStarterIni";
            this.buttonRestoreSettingsOfSebStarterIni.Size = new System.Drawing.Size(139, 72);
            this.buttonRestoreSettingsOfSebStarterIni.TabIndex = 19;
            this.buttonRestoreSettingsOfSebStarterIni.Text = "Restore settings of SebStarter.ini";
            this.buttonRestoreSettingsOfSebStarterIni.UseVisualStyleBackColor = true;
            this.buttonRestoreSettingsOfSebStarterIni.Click += new System.EventHandler(this.buttonRestoreSettingsOfSebStarterIni_Click);
            // 
            // groupBoxOnlineExam
            // 
            this.groupBoxOnlineExam.Controls.Add(this.textBoxAutostartProcess);
            this.groupBoxOnlineExam.Controls.Add(this.labelSebBrowser);
            this.groupBoxOnlineExam.Controls.Add(this.labelAutostartProcess);
            this.groupBoxOnlineExam.Controls.Add(this.textBoxSebBrowser);
            this.groupBoxOnlineExam.Controls.Add(this.labelExamUrl);
            this.groupBoxOnlineExam.Controls.Add(this.labelPermittedApplications);
            this.groupBoxOnlineExam.Controls.Add(this.textBoxPermittedApplications);
            this.groupBoxOnlineExam.Controls.Add(this.textBoxExamUrl);
            this.groupBoxOnlineExam.Location = new System.Drawing.Point(54, 456);
            this.groupBoxOnlineExam.Name = "groupBoxOnlineExam";
            this.groupBoxOnlineExam.Size = new System.Drawing.Size(637, 149);
            this.groupBoxOnlineExam.TabIndex = 25;
            this.groupBoxOnlineExam.TabStop = false;
            this.groupBoxOnlineExam.Text = "Online Exam";
            // 
            // textBoxAutostartProcess
            // 
            this.textBoxAutostartProcess.Location = new System.Drawing.Point(153, 59);
            this.textBoxAutostartProcess.Name = "textBoxAutostartProcess";
            this.textBoxAutostartProcess.Size = new System.Drawing.Size(478, 22);
            this.textBoxAutostartProcess.TabIndex = 27;
            this.textBoxAutostartProcess.TextChanged += new System.EventHandler(this.textBoxAutostartProcess_TextChanged);
            // 
            // labelSebBrowser
            // 
            this.labelSebBrowser.AutoSize = true;
            this.labelSebBrowser.Location = new System.Drawing.Point(6, 34);
            this.labelSebBrowser.Name = "labelSebBrowser";
            this.labelSebBrowser.Size = new System.Drawing.Size(89, 17);
            this.labelSebBrowser.TabIndex = 26;
            this.labelSebBrowser.Text = "SEB browser";
            // 
            // labelAutostartProcess
            // 
            this.labelAutostartProcess.AutoSize = true;
            this.labelAutostartProcess.Location = new System.Drawing.Point(6, 62);
            this.labelAutostartProcess.Name = "labelAutostartProcess";
            this.labelAutostartProcess.Size = new System.Drawing.Size(119, 17);
            this.labelAutostartProcess.TabIndex = 25;
            this.labelAutostartProcess.Text = "Autostart process";
            // 
            // textBoxSebBrowser
            // 
            this.textBoxSebBrowser.Location = new System.Drawing.Point(153, 31);
            this.textBoxSebBrowser.Name = "textBoxSebBrowser";
            this.textBoxSebBrowser.Size = new System.Drawing.Size(478, 22);
            this.textBoxSebBrowser.TabIndex = 24;
            this.textBoxSebBrowser.TextChanged += new System.EventHandler(this.textBoxSebBrowser_TextChanged);
            // 
            // labelExamUrl
            // 
            this.labelExamUrl.AutoSize = true;
            this.labelExamUrl.Location = new System.Drawing.Point(6, 90);
            this.labelExamUrl.Name = "labelExamUrl";
            this.labelExamUrl.Size = new System.Drawing.Size(74, 17);
            this.labelExamUrl.TabIndex = 21;
            this.labelExamUrl.Text = "Exam URL";
            // 
            // labelPermittedApplications
            // 
            this.labelPermittedApplications.AutoSize = true;
            this.labelPermittedApplications.Location = new System.Drawing.Point(6, 118);
            this.labelPermittedApplications.Name = "labelPermittedApplications";
            this.labelPermittedApplications.Size = new System.Drawing.Size(147, 17);
            this.labelPermittedApplications.TabIndex = 22;
            this.labelPermittedApplications.Text = "Permitted applications";
            // 
            // textBoxPermittedApplications
            // 
            this.textBoxPermittedApplications.Location = new System.Drawing.Point(153, 115);
            this.textBoxPermittedApplications.Name = "textBoxPermittedApplications";
            this.textBoxPermittedApplications.Size = new System.Drawing.Size(478, 22);
            this.textBoxPermittedApplications.TabIndex = 23;
            this.textBoxPermittedApplications.TextChanged += new System.EventHandler(this.textBoxPermittedApplications_TextChanged);
            // 
            // textBoxExamUrl
            // 
            this.textBoxExamUrl.Location = new System.Drawing.Point(153, 87);
            this.textBoxExamUrl.Name = "textBoxExamUrl";
            this.textBoxExamUrl.Size = new System.Drawing.Size(478, 22);
            this.textBoxExamUrl.TabIndex = 20;
            this.textBoxExamUrl.TextChanged += new System.EventHandler(this.textBoxExamUrl_TextChanged);
            // 
            // groupBoxWindowsSecurityScreen
            // 
            this.groupBoxWindowsSecurityScreen.Controls.Add(this.checkBoxEnableSwitchUser);
            this.groupBoxWindowsSecurityScreen.Controls.Add(this.checkBoxEnableLockThisComputer);
            this.groupBoxWindowsSecurityScreen.Controls.Add(this.checkBoxEnableChangeAPassword);
            this.groupBoxWindowsSecurityScreen.Controls.Add(this.checkBoxEnableStartTaskManager);
            this.groupBoxWindowsSecurityScreen.Controls.Add(this.checkBoxEnableLogOff);
            this.groupBoxWindowsSecurityScreen.Controls.Add(this.checkBoxEnableShutDown);
            this.groupBoxWindowsSecurityScreen.Controls.Add(this.checkBoxEnableEaseOfAccess);
            this.groupBoxWindowsSecurityScreen.Controls.Add(this.checkBoxEnableVmWareClientShade);
            this.groupBoxWindowsSecurityScreen.Location = new System.Drawing.Point(54, 166);
            this.groupBoxWindowsSecurityScreen.Name = "groupBoxWindowsSecurityScreen";
            this.groupBoxWindowsSecurityScreen.Size = new System.Drawing.Size(264, 270);
            this.groupBoxWindowsSecurityScreen.TabIndex = 24;
            this.groupBoxWindowsSecurityScreen.TabStop = false;
            this.groupBoxWindowsSecurityScreen.Text = "Windows Security Screen";
            // 
            // checkBoxEnableSwitchUser
            // 
            this.checkBoxEnableSwitchUser.AutoSize = true;
            this.checkBoxEnableSwitchUser.Location = new System.Drawing.Point(19, 37);
            this.checkBoxEnableSwitchUser.Name = "checkBoxEnableSwitchUser";
            this.checkBoxEnableSwitchUser.Size = new System.Drawing.Size(152, 21);
            this.checkBoxEnableSwitchUser.TabIndex = 0;
            this.checkBoxEnableSwitchUser.Text = "Enable Switch User";
            this.checkBoxEnableSwitchUser.UseVisualStyleBackColor = true;
            this.checkBoxEnableSwitchUser.CheckedChanged += new System.EventHandler(this.checkBoxEnableSwitchUser_CheckedChanged);
            // 
            // checkBoxEnableLockThisComputer
            // 
            this.checkBoxEnableLockThisComputer.AutoSize = true;
            this.checkBoxEnableLockThisComputer.Location = new System.Drawing.Point(19, 64);
            this.checkBoxEnableLockThisComputer.Name = "checkBoxEnableLockThisComputer";
            this.checkBoxEnableLockThisComputer.Size = new System.Drawing.Size(197, 21);
            this.checkBoxEnableLockThisComputer.TabIndex = 1;
            this.checkBoxEnableLockThisComputer.Text = "Enable Lock this computer";
            this.checkBoxEnableLockThisComputer.UseVisualStyleBackColor = true;
            this.checkBoxEnableLockThisComputer.CheckedChanged += new System.EventHandler(this.checkBoxEnableLockThisComputer_CheckedChanged);
            // 
            // checkBoxEnableChangeAPassword
            // 
            this.checkBoxEnableChangeAPassword.AutoSize = true;
            this.checkBoxEnableChangeAPassword.Location = new System.Drawing.Point(19, 91);
            this.checkBoxEnableChangeAPassword.Name = "checkBoxEnableChangeAPassword";
            this.checkBoxEnableChangeAPassword.Size = new System.Drawing.Size(203, 21);
            this.checkBoxEnableChangeAPassword.TabIndex = 3;
            this.checkBoxEnableChangeAPassword.Text = "Enable Change a password";
            this.checkBoxEnableChangeAPassword.UseVisualStyleBackColor = true;
            this.checkBoxEnableChangeAPassword.CheckedChanged += new System.EventHandler(this.checkBoxEnableChangeAPassword_CheckedChanged);
            // 
            // checkBoxEnableStartTaskManager
            // 
            this.checkBoxEnableStartTaskManager.AutoSize = true;
            this.checkBoxEnableStartTaskManager.Location = new System.Drawing.Point(19, 118);
            this.checkBoxEnableStartTaskManager.Name = "checkBoxEnableStartTaskManager";
            this.checkBoxEnableStartTaskManager.Size = new System.Drawing.Size(203, 21);
            this.checkBoxEnableStartTaskManager.TabIndex = 2;
            this.checkBoxEnableStartTaskManager.Text = "Enable Start Task Manager";
            this.checkBoxEnableStartTaskManager.UseVisualStyleBackColor = true;
            this.checkBoxEnableStartTaskManager.CheckedChanged += new System.EventHandler(this.checkBoxEnableStartTaskManager_CheckedChanged);
            // 
            // checkBoxEnableLogOff
            // 
            this.checkBoxEnableLogOff.AutoSize = true;
            this.checkBoxEnableLogOff.Location = new System.Drawing.Point(19, 145);
            this.checkBoxEnableLogOff.Name = "checkBoxEnableLogOff";
            this.checkBoxEnableLogOff.Size = new System.Drawing.Size(122, 21);
            this.checkBoxEnableLogOff.TabIndex = 6;
            this.checkBoxEnableLogOff.Text = "Enable Log off";
            this.checkBoxEnableLogOff.UseVisualStyleBackColor = true;
            this.checkBoxEnableLogOff.CheckedChanged += new System.EventHandler(this.checkBoxEnableLogOff_CheckedChanged);
            // 
            // checkBoxEnableShutDown
            // 
            this.checkBoxEnableShutDown.AutoSize = true;
            this.checkBoxEnableShutDown.Location = new System.Drawing.Point(19, 172);
            this.checkBoxEnableShutDown.Name = "checkBoxEnableShutDown";
            this.checkBoxEnableShutDown.Size = new System.Drawing.Size(144, 21);
            this.checkBoxEnableShutDown.TabIndex = 4;
            this.checkBoxEnableShutDown.Text = "Enable Shut down";
            this.checkBoxEnableShutDown.UseVisualStyleBackColor = true;
            this.checkBoxEnableShutDown.CheckedChanged += new System.EventHandler(this.checkBoxEnableShutDown_CheckedChanged);
            // 
            // checkBoxEnableEaseOfAccess
            // 
            this.checkBoxEnableEaseOfAccess.AutoSize = true;
            this.checkBoxEnableEaseOfAccess.Location = new System.Drawing.Point(19, 199);
            this.checkBoxEnableEaseOfAccess.Name = "checkBoxEnableEaseOfAccess";
            this.checkBoxEnableEaseOfAccess.Size = new System.Drawing.Size(175, 21);
            this.checkBoxEnableEaseOfAccess.TabIndex = 16;
            this.checkBoxEnableEaseOfAccess.Text = "Enable Ease of Access";
            this.checkBoxEnableEaseOfAccess.UseVisualStyleBackColor = true;
            this.checkBoxEnableEaseOfAccess.CheckedChanged += new System.EventHandler(this.checkBoxEnableEaseOfAccess_CheckedChanged);
            // 
            // checkBoxEnableVmWareClientShade
            // 
            this.checkBoxEnableVmWareClientShade.AutoSize = true;
            this.checkBoxEnableVmWareClientShade.Location = new System.Drawing.Point(19, 226);
            this.checkBoxEnableVmWareClientShade.Name = "checkBoxEnableVmWareClientShade";
            this.checkBoxEnableVmWareClientShade.Size = new System.Drawing.Size(212, 21);
            this.checkBoxEnableVmWareClientShade.TabIndex = 7;
            this.checkBoxEnableVmWareClientShade.Text = "Enable VMware Client Shade";
            this.checkBoxEnableVmWareClientShade.UseVisualStyleBackColor = true;
            this.checkBoxEnableVmWareClientShade.CheckedChanged += new System.EventHandler(this.checkBoxEnableVmWareClientShade_CheckedChanged);
            // 
            // checkBoxWriteLogFileMsgHookLog
            // 
            this.checkBoxWriteLogFileMsgHookLog.AutoSize = true;
            this.checkBoxWriteLogFileMsgHookLog.Location = new System.Drawing.Point(45, 425);
            this.checkBoxWriteLogFileMsgHookLog.Name = "checkBoxWriteLogFileMsgHookLog";
            this.checkBoxWriteLogFileMsgHookLog.Size = new System.Drawing.Size(190, 21);
            this.checkBoxWriteLogFileMsgHookLog.TabIndex = 41;
            this.checkBoxWriteLogFileMsgHookLog.Text = "Write logfile MsgHook.log";
            this.checkBoxWriteLogFileMsgHookLog.UseVisualStyleBackColor = true;
            // 
            // groupBoxMsgHookIni
            // 
            this.groupBoxMsgHookIni.Controls.Add(this.labelOpenFileMsgHookIni);
            this.groupBoxMsgHookIni.Controls.Add(this.labelSaveFileMsgHookIni);
            this.groupBoxMsgHookIni.Location = new System.Drawing.Point(39, 38);
            this.groupBoxMsgHookIni.Name = "groupBoxMsgHookIni";
            this.groupBoxMsgHookIni.Size = new System.Drawing.Size(200, 100);
            this.groupBoxMsgHookIni.TabIndex = 40;
            this.groupBoxMsgHookIni.TabStop = false;
            this.groupBoxMsgHookIni.Text = "MsgHook.ini";
            // 
            // labelOpenFileMsgHookIni
            // 
            this.labelOpenFileMsgHookIni.AutoSize = true;
            this.labelOpenFileMsgHookIni.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelOpenFileMsgHookIni.Location = new System.Drawing.Point(6, 36);
            this.labelOpenFileMsgHookIni.Name = "labelOpenFileMsgHookIni";
            this.labelOpenFileMsgHookIni.Size = new System.Drawing.Size(148, 19);
            this.labelOpenFileMsgHookIni.TabIndex = 17;
            this.labelOpenFileMsgHookIni.Text = "Open file MsgHook.ini";
            this.labelOpenFileMsgHookIni.Click += new System.EventHandler(this.labelOpenFileMsgHookIni_Click);
            // 
            // labelSaveFileMsgHookIni
            // 
            this.labelSaveFileMsgHookIni.AutoSize = true;
            this.labelSaveFileMsgHookIni.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelSaveFileMsgHookIni.Location = new System.Drawing.Point(6, 60);
            this.labelSaveFileMsgHookIni.Name = "labelSaveFileMsgHookIni";
            this.labelSaveFileMsgHookIni.Size = new System.Drawing.Size(145, 19);
            this.labelSaveFileMsgHookIni.TabIndex = 18;
            this.labelSaveFileMsgHookIni.Text = "Save file MsgHook.ini";
            this.labelSaveFileMsgHookIni.Click += new System.EventHandler(this.labelSaveFileMsgHookIni_Click);
            // 
            // groupBoxFunctionKeys
            // 
            this.groupBoxFunctionKeys.Controls.Add(this.checkBoxF1);
            this.groupBoxFunctionKeys.Controls.Add(this.checkBoxF2);
            this.groupBoxFunctionKeys.Controls.Add(this.checkBoxF12);
            this.groupBoxFunctionKeys.Controls.Add(this.checkBoxF3);
            this.groupBoxFunctionKeys.Controls.Add(this.checkBoxF11);
            this.groupBoxFunctionKeys.Controls.Add(this.checkBoxF4);
            this.groupBoxFunctionKeys.Controls.Add(this.checkBoxF5);
            this.groupBoxFunctionKeys.Controls.Add(this.checkBox10);
            this.groupBoxFunctionKeys.Controls.Add(this.checkBoxF6);
            this.groupBoxFunctionKeys.Controls.Add(this.checkBoxF9);
            this.groupBoxFunctionKeys.Controls.Add(this.checkBoxF7);
            this.groupBoxFunctionKeys.Controls.Add(this.checkBoxF8);
            this.groupBoxFunctionKeys.Location = new System.Drawing.Point(288, 38);
            this.groupBoxFunctionKeys.Name = "groupBoxFunctionKeys";
            this.groupBoxFunctionKeys.Size = new System.Drawing.Size(200, 362);
            this.groupBoxFunctionKeys.TabIndex = 39;
            this.groupBoxFunctionKeys.TabStop = false;
            this.groupBoxFunctionKeys.Text = "Function Keys";
            // 
            // checkBoxF1
            // 
            this.checkBoxF1.AutoSize = true;
            this.checkBoxF1.Location = new System.Drawing.Point(6, 36);
            this.checkBoxF1.Name = "checkBoxF1";
            this.checkBoxF1.Size = new System.Drawing.Size(94, 21);
            this.checkBoxF1.TabIndex = 25;
            this.checkBoxF1.Text = "Enable F1";
            this.checkBoxF1.UseVisualStyleBackColor = true;
            // 
            // checkBoxF2
            // 
            this.checkBoxF2.AutoSize = true;
            this.checkBoxF2.Location = new System.Drawing.Point(6, 63);
            this.checkBoxF2.Name = "checkBoxF2";
            this.checkBoxF2.Size = new System.Drawing.Size(94, 21);
            this.checkBoxF2.TabIndex = 26;
            this.checkBoxF2.Text = "Enable F2";
            this.checkBoxF2.UseVisualStyleBackColor = true;
            // 
            // checkBoxF12
            // 
            this.checkBoxF12.AutoSize = true;
            this.checkBoxF12.Location = new System.Drawing.Point(6, 333);
            this.checkBoxF12.Name = "checkBoxF12";
            this.checkBoxF12.Size = new System.Drawing.Size(102, 21);
            this.checkBoxF12.TabIndex = 37;
            this.checkBoxF12.Text = "Enable F12";
            this.checkBoxF12.UseVisualStyleBackColor = true;
            // 
            // checkBoxF3
            // 
            this.checkBoxF3.AutoSize = true;
            this.checkBoxF3.Location = new System.Drawing.Point(6, 90);
            this.checkBoxF3.Name = "checkBoxF3";
            this.checkBoxF3.Size = new System.Drawing.Size(94, 21);
            this.checkBoxF3.TabIndex = 27;
            this.checkBoxF3.Text = "Enable F3";
            this.checkBoxF3.UseVisualStyleBackColor = true;
            // 
            // checkBoxF11
            // 
            this.checkBoxF11.AutoSize = true;
            this.checkBoxF11.Location = new System.Drawing.Point(6, 306);
            this.checkBoxF11.Name = "checkBoxF11";
            this.checkBoxF11.Size = new System.Drawing.Size(102, 21);
            this.checkBoxF11.TabIndex = 36;
            this.checkBoxF11.Text = "Enable F11";
            this.checkBoxF11.UseVisualStyleBackColor = true;
            // 
            // checkBoxF4
            // 
            this.checkBoxF4.AutoSize = true;
            this.checkBoxF4.Location = new System.Drawing.Point(6, 117);
            this.checkBoxF4.Name = "checkBoxF4";
            this.checkBoxF4.Size = new System.Drawing.Size(94, 21);
            this.checkBoxF4.TabIndex = 28;
            this.checkBoxF4.Text = "Enable F4";
            this.checkBoxF4.UseVisualStyleBackColor = true;
            // 
            // checkBoxF5
            // 
            this.checkBoxF5.AutoSize = true;
            this.checkBoxF5.Location = new System.Drawing.Point(6, 144);
            this.checkBoxF5.Name = "checkBoxF5";
            this.checkBoxF5.Size = new System.Drawing.Size(94, 21);
            this.checkBoxF5.TabIndex = 29;
            this.checkBoxF5.Text = "Enable F5";
            this.checkBoxF5.UseVisualStyleBackColor = true;
            // 
            // checkBox10
            // 
            this.checkBox10.AutoSize = true;
            this.checkBox10.Location = new System.Drawing.Point(6, 279);
            this.checkBox10.Name = "checkBox10";
            this.checkBox10.Size = new System.Drawing.Size(102, 21);
            this.checkBox10.TabIndex = 34;
            this.checkBox10.Text = "Enable F10";
            this.checkBox10.UseVisualStyleBackColor = true;
            // 
            // checkBoxF6
            // 
            this.checkBoxF6.AutoSize = true;
            this.checkBoxF6.Location = new System.Drawing.Point(6, 171);
            this.checkBoxF6.Name = "checkBoxF6";
            this.checkBoxF6.Size = new System.Drawing.Size(94, 21);
            this.checkBoxF6.TabIndex = 30;
            this.checkBoxF6.Text = "Enable F6";
            this.checkBoxF6.UseVisualStyleBackColor = true;
            // 
            // checkBoxF9
            // 
            this.checkBoxF9.AutoSize = true;
            this.checkBoxF9.Location = new System.Drawing.Point(6, 252);
            this.checkBoxF9.Name = "checkBoxF9";
            this.checkBoxF9.Size = new System.Drawing.Size(94, 21);
            this.checkBoxF9.TabIndex = 33;
            this.checkBoxF9.Text = "Enable F9";
            this.checkBoxF9.UseVisualStyleBackColor = true;
            // 
            // checkBoxF7
            // 
            this.checkBoxF7.AutoSize = true;
            this.checkBoxF7.Location = new System.Drawing.Point(6, 198);
            this.checkBoxF7.Name = "checkBoxF7";
            this.checkBoxF7.Size = new System.Drawing.Size(94, 21);
            this.checkBoxF7.TabIndex = 31;
            this.checkBoxF7.Text = "Enable F7";
            this.checkBoxF7.UseVisualStyleBackColor = true;
            // 
            // checkBoxF8
            // 
            this.checkBoxF8.AutoSize = true;
            this.checkBoxF8.Location = new System.Drawing.Point(6, 225);
            this.checkBoxF8.Name = "checkBoxF8";
            this.checkBoxF8.Size = new System.Drawing.Size(94, 21);
            this.checkBoxF8.TabIndex = 32;
            this.checkBoxF8.Text = "Enable F8";
            this.checkBoxF8.UseVisualStyleBackColor = true;
            // 
            // groupBoxSpecialKeys
            // 
            this.groupBoxSpecialKeys.Controls.Add(this.checkBoxEsc);
            this.groupBoxSpecialKeys.Controls.Add(this.checkBoxCtrlEsc);
            this.groupBoxSpecialKeys.Controls.Add(this.checkBoxAltEsc);
            this.groupBoxSpecialKeys.Controls.Add(this.checkBoxAltTab);
            this.groupBoxSpecialKeys.Controls.Add(this.checkBoxAltF4);
            this.groupBoxSpecialKeys.Controls.Add(this.checkBoxStartMenu);
            this.groupBoxSpecialKeys.Controls.Add(this.checkBoxRightMouse);
            this.groupBoxSpecialKeys.Location = new System.Drawing.Point(39, 166);
            this.groupBoxSpecialKeys.Name = "groupBoxSpecialKeys";
            this.groupBoxSpecialKeys.Size = new System.Drawing.Size(200, 234);
            this.groupBoxSpecialKeys.TabIndex = 38;
            this.groupBoxSpecialKeys.TabStop = false;
            this.groupBoxSpecialKeys.Text = "Special Keys";
            // 
            // checkBoxEsc
            // 
            this.checkBoxEsc.AutoSize = true;
            this.checkBoxEsc.Location = new System.Drawing.Point(6, 37);
            this.checkBoxEsc.Name = "checkBoxEsc";
            this.checkBoxEsc.Size = new System.Drawing.Size(101, 21);
            this.checkBoxEsc.TabIndex = 41;
            this.checkBoxEsc.Text = "Enable Esc";
            this.checkBoxEsc.UseVisualStyleBackColor = true;
            // 
            // checkBoxCtrlEsc
            // 
            this.checkBoxCtrlEsc.AutoSize = true;
            this.checkBoxCtrlEsc.Location = new System.Drawing.Point(6, 64);
            this.checkBoxCtrlEsc.Name = "checkBoxCtrlEsc";
            this.checkBoxCtrlEsc.Size = new System.Drawing.Size(127, 21);
            this.checkBoxCtrlEsc.TabIndex = 19;
            this.checkBoxCtrlEsc.Text = "Enable Ctrl-Esc";
            this.checkBoxCtrlEsc.UseVisualStyleBackColor = true;
            // 
            // checkBoxAltEsc
            // 
            this.checkBoxAltEsc.AutoSize = true;
            this.checkBoxAltEsc.Location = new System.Drawing.Point(6, 91);
            this.checkBoxAltEsc.Name = "checkBoxAltEsc";
            this.checkBoxAltEsc.Size = new System.Drawing.Size(122, 21);
            this.checkBoxAltEsc.TabIndex = 20;
            this.checkBoxAltEsc.Text = "Enable Alt-Esc";
            this.checkBoxAltEsc.UseVisualStyleBackColor = true;
            // 
            // checkBoxAltTab
            // 
            this.checkBoxAltTab.AutoSize = true;
            this.checkBoxAltTab.Location = new System.Drawing.Point(6, 118);
            this.checkBoxAltTab.Name = "checkBoxAltTab";
            this.checkBoxAltTab.Size = new System.Drawing.Size(124, 21);
            this.checkBoxAltTab.TabIndex = 21;
            this.checkBoxAltTab.Text = "Enable Alt-Tab";
            this.checkBoxAltTab.UseVisualStyleBackColor = true;
            // 
            // checkBoxAltF4
            // 
            this.checkBoxAltF4.AutoSize = true;
            this.checkBoxAltF4.Location = new System.Drawing.Point(6, 145);
            this.checkBoxAltF4.Name = "checkBoxAltF4";
            this.checkBoxAltF4.Size = new System.Drawing.Size(115, 21);
            this.checkBoxAltF4.TabIndex = 22;
            this.checkBoxAltF4.Text = "Enable Alt-F4";
            this.checkBoxAltF4.UseVisualStyleBackColor = true;
            // 
            // checkBoxStartMenu
            // 
            this.checkBoxStartMenu.AutoSize = true;
            this.checkBoxStartMenu.Location = new System.Drawing.Point(6, 172);
            this.checkBoxStartMenu.Name = "checkBoxStartMenu";
            this.checkBoxStartMenu.Size = new System.Drawing.Size(147, 21);
            this.checkBoxStartMenu.TabIndex = 23;
            this.checkBoxStartMenu.Text = "Enable Start Menu";
            this.checkBoxStartMenu.UseVisualStyleBackColor = true;
            // 
            // checkBoxRightMouse
            // 
            this.checkBoxRightMouse.AutoSize = true;
            this.checkBoxRightMouse.Location = new System.Drawing.Point(6, 199);
            this.checkBoxRightMouse.Name = "checkBoxRightMouse";
            this.checkBoxRightMouse.Size = new System.Drawing.Size(157, 21);
            this.checkBoxRightMouse.TabIndex = 24;
            this.checkBoxRightMouse.Text = "Enable Right Mouse";
            this.checkBoxRightMouse.UseVisualStyleBackColor = true;
            // 
            // openFileDialogSebStarterIni
            // 
            this.openFileDialogSebStarterIni.FileName = "openFileDialogSebStarterIni";
            // 
            // SebWindowsConfigForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1435, 655);
            this.Controls.Add(this.splitContainer1);
            this.Name = "SebWindowsConfigForm";
            this.Text = "SEB Windows Configuration Window";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.groupBoxFurtherSettings.ResumeLayout(false);
            this.groupBoxFurtherSettings.PerformLayout();
            this.groupBoxSebStarterIni.ResumeLayout(false);
            this.groupBoxSebStarterIni.PerformLayout();
            this.groupBoxOnlineExam.ResumeLayout(false);
            this.groupBoxOnlineExam.PerformLayout();
            this.groupBoxWindowsSecurityScreen.ResumeLayout(false);
            this.groupBoxWindowsSecurityScreen.PerformLayout();
            this.groupBoxMsgHookIni.ResumeLayout(false);
            this.groupBoxMsgHookIni.PerformLayout();
            this.groupBoxFunctionKeys.ResumeLayout(false);
            this.groupBoxFunctionKeys.PerformLayout();
            this.groupBoxSpecialKeys.ResumeLayout(false);
            this.groupBoxSpecialKeys.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.CheckBox checkBoxEnableSwitchUser;
        private System.Windows.Forms.CheckBox checkBoxEnableLogOff;
        private System.Windows.Forms.CheckBox checkBoxEnableShutDown;
        private System.Windows.Forms.CheckBox checkBoxEnableChangeAPassword;
        private System.Windows.Forms.CheckBox checkBoxEnableStartTaskManager;
        private System.Windows.Forms.CheckBox checkBoxEnableLockThisComputer;
        private System.Windows.Forms.CheckBox checkBoxEnableVmWareClientShade;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialogBrowseIniFiles;
        private System.Windows.Forms.OpenFileDialog openFileDialogSebStarterIni;
        private System.Windows.Forms.SaveFileDialog saveFileDialogSebStarterIni;
        private System.Windows.Forms.Label labelOpenFileSebStarterIni;
        private System.Windows.Forms.Label labelSaveFileSebStarterIni;
        private System.Windows.Forms.CheckBox checkBoxEnableEaseOfAccess;
        private System.Windows.Forms.Label labelOpenFileMsgHookIni;
        private System.Windows.Forms.Label labelSaveFileMsgHookIni;
        private System.Windows.Forms.Button buttonRestoreSettingsOfSebStarterIni;
        private System.Windows.Forms.Label labelExamUrl;
        private System.Windows.Forms.TextBox textBoxExamUrl;
        private System.Windows.Forms.Label labelPermittedApplications;
        private System.Windows.Forms.TextBox textBoxPermittedApplications;
        private System.Windows.Forms.CheckBox checkBoxCtrlEsc;
        private System.Windows.Forms.CheckBox checkBoxAltEsc;
        private System.Windows.Forms.CheckBox checkBoxAltTab;
        private System.Windows.Forms.CheckBox checkBoxAltF4;
        private System.Windows.Forms.CheckBox checkBoxStartMenu;
        private System.Windows.Forms.CheckBox checkBoxRightMouse;
        private System.Windows.Forms.CheckBox checkBoxF1;
        private System.Windows.Forms.CheckBox checkBox10;
        private System.Windows.Forms.CheckBox checkBoxF9;
        private System.Windows.Forms.CheckBox checkBoxF8;
        private System.Windows.Forms.CheckBox checkBoxF7;
        private System.Windows.Forms.CheckBox checkBoxF6;
        private System.Windows.Forms.CheckBox checkBoxF5;
        private System.Windows.Forms.CheckBox checkBoxF4;
        private System.Windows.Forms.CheckBox checkBoxF3;
        private System.Windows.Forms.CheckBox checkBoxF2;
        private System.Windows.Forms.CheckBox checkBoxF12;
        private System.Windows.Forms.CheckBox checkBoxF11;
        private System.Windows.Forms.GroupBox groupBoxWindowsSecurityScreen;
        private System.Windows.Forms.GroupBox groupBoxOnlineExam;
        private System.Windows.Forms.GroupBox groupBoxSebStarterIni;
        private System.Windows.Forms.GroupBox groupBoxFunctionKeys;
        private System.Windows.Forms.GroupBox groupBoxSpecialKeys;
        private System.Windows.Forms.GroupBox groupBoxMsgHookIni;
        private System.Windows.Forms.CheckBox checkBoxEsc;
        private System.Windows.Forms.CheckBox checkBoxWriteLogFileMsgHookLog;
        private System.Windows.Forms.CheckBox checkBoxWriteLogFileSebStarterLog;
        private System.Windows.Forms.CheckBox checkBoxForceWindowsService;
        private System.Windows.Forms.CheckBox checkBoxAllowVirtualMachine;
        private System.Windows.Forms.GroupBox groupBoxFurtherSettings;
        private System.Windows.Forms.CheckBox checkBoxEditRegistry;
        private System.Windows.Forms.CheckBox checkBoxHookMessages;
        private System.Windows.Forms.CheckBox checkBoxShowSebApplicationChooser;
        private System.Windows.Forms.CheckBox checkBoxCreateNewDesktop;
        private System.Windows.Forms.CheckBox checkBoxMonitorProcesses;
        private System.Windows.Forms.CheckBox checkBoxShutdownAfterAutostartProcessTerminates;
        private System.Windows.Forms.Label labelAutostartProcess;
        private System.Windows.Forms.TextBox textBoxSebBrowser;
        private System.Windows.Forms.TextBox textBoxAutostartProcess;
        private System.Windows.Forms.Label labelSebBrowser;
    }
}

