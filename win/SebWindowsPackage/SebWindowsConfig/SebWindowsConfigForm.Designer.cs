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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.groupBoxSecurityOptions = new System.Windows.Forms.GroupBox();
            this.checkBoxShutdownAfterAutostart = new System.Windows.Forms.CheckBox();
            this.checkBoxMonitorProcesses = new System.Windows.Forms.CheckBox();
            this.checkBoxEditRegistry = new System.Windows.Forms.CheckBox();
            this.checkBoxHookMessages = new System.Windows.Forms.CheckBox();
            this.checkBoxShowSebApplicationChooser = new System.Windows.Forms.CheckBox();
            this.checkBoxCreateNewDesktop = new System.Windows.Forms.CheckBox();
            this.checkBoxAllowVirtualMachine = new System.Windows.Forms.CheckBox();
            this.checkBoxForceWindowsService = new System.Windows.Forms.CheckBox();
            this.groupBoxSebStarterIni = new System.Windows.Forms.GroupBox();
            this.labelOpenFileSebStarterIni = new System.Windows.Forms.Label();
            this.labelSaveFileSebStarterIni = new System.Windows.Forms.Label();
            this.buttonRestoreSettingsOfSebStarterIni = new System.Windows.Forms.Button();
            this.checkBoxWriteLogFileSebStarterLog = new System.Windows.Forms.CheckBox();
            this.groupBoxOnlineExam = new System.Windows.Forms.GroupBox();
            this.textBoxAutostartProcess = new System.Windows.Forms.TextBox();
            this.labelSebBrowser = new System.Windows.Forms.Label();
            this.labelAutostartProcess = new System.Windows.Forms.Label();
            this.textBoxSebBrowser = new System.Windows.Forms.TextBox();
            this.labelExamUrl = new System.Windows.Forms.Label();
            this.labelPermittedApplications = new System.Windows.Forms.Label();
            this.textBoxPermittedApplications = new System.Windows.Forms.TextBox();
            this.textBoxExamUrl = new System.Windows.Forms.TextBox();
            this.groupBoxRegistryValues = new System.Windows.Forms.GroupBox();
            this.checkBoxEnableSwitchUser = new System.Windows.Forms.CheckBox();
            this.checkBoxEnableLockThisComputer = new System.Windows.Forms.CheckBox();
            this.checkBoxEnableChangeAPassword = new System.Windows.Forms.CheckBox();
            this.checkBoxEnableStartTaskManager = new System.Windows.Forms.CheckBox();
            this.checkBoxEnableLogOff = new System.Windows.Forms.CheckBox();
            this.checkBoxEnableShutDown = new System.Windows.Forms.CheckBox();
            this.checkBoxEnableEaseOfAccess = new System.Windows.Forms.CheckBox();
            this.checkBoxEnableVmWareClientShade = new System.Windows.Forms.CheckBox();
            this.groupBoxExitSequence = new System.Windows.Forms.GroupBox();
            this.listBoxExitKey1 = new System.Windows.Forms.ListBox();
            this.listBoxExitKey3 = new System.Windows.Forms.ListBox();
            this.listBoxExitKey2 = new System.Windows.Forms.ListBox();
            this.textBoxDebug = new System.Windows.Forms.TextBox();
            this.groupBoxMsgHookIni = new System.Windows.Forms.GroupBox();
            this.labelOpenFileMsgHookIni = new System.Windows.Forms.Label();
            this.labelSaveFileMsgHookIni = new System.Windows.Forms.Label();
            this.checkBoxWriteLogFileMsgHookLog = new System.Windows.Forms.CheckBox();
            this.buttonRestoreSettingsOfMsgHookIni = new System.Windows.Forms.Button();
            this.groupBoxFunctionKeys = new System.Windows.Forms.GroupBox();
            this.checkBoxEnableF1 = new System.Windows.Forms.CheckBox();
            this.checkBoxEnableF2 = new System.Windows.Forms.CheckBox();
            this.checkBoxEnableF12 = new System.Windows.Forms.CheckBox();
            this.checkBoxEnableF3 = new System.Windows.Forms.CheckBox();
            this.checkBoxEnableF11 = new System.Windows.Forms.CheckBox();
            this.checkBoxEnableF4 = new System.Windows.Forms.CheckBox();
            this.checkBoxEnableF5 = new System.Windows.Forms.CheckBox();
            this.checkBoxEnableF10 = new System.Windows.Forms.CheckBox();
            this.checkBoxEnableF6 = new System.Windows.Forms.CheckBox();
            this.checkBoxEnableF9 = new System.Windows.Forms.CheckBox();
            this.checkBoxEnableF7 = new System.Windows.Forms.CheckBox();
            this.checkBoxEnableF8 = new System.Windows.Forms.CheckBox();
            this.groupBoxSpecialKeys = new System.Windows.Forms.GroupBox();
            this.checkBoxEnableEsc = new System.Windows.Forms.CheckBox();
            this.checkBoxEnableCtrlEsc = new System.Windows.Forms.CheckBox();
            this.checkBoxEnableAltEsc = new System.Windows.Forms.CheckBox();
            this.checkBoxEnableAltTab = new System.Windows.Forms.CheckBox();
            this.checkBoxEnableAltF4 = new System.Windows.Forms.CheckBox();
            this.checkBoxEnableStartMenu = new System.Windows.Forms.CheckBox();
            this.checkBoxEnableRightMouse = new System.Windows.Forms.CheckBox();
            this.openFileDialogSebStarterIni = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialogSebStarterIni = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialogMsgHookIni = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialogMsgHookIni = new System.Windows.Forms.SaveFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBoxSecurityOptions.SuspendLayout();
            this.groupBoxSebStarterIni.SuspendLayout();
            this.groupBoxOnlineExam.SuspendLayout();
            this.groupBoxRegistryValues.SuspendLayout();
            this.groupBoxExitSequence.SuspendLayout();
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
            this.splitContainer1.Panel1.Controls.Add(this.groupBoxSecurityOptions);
            this.splitContainer1.Panel1.Controls.Add(this.groupBoxSebStarterIni);
            this.splitContainer1.Panel1.Controls.Add(this.groupBoxOnlineExam);
            this.splitContainer1.Panel1.Controls.Add(this.groupBoxRegistryValues);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.groupBoxExitSequence);
            this.splitContainer1.Panel2.Controls.Add(this.textBoxDebug);
            this.splitContainer1.Panel2.Controls.Add(this.groupBoxMsgHookIni);
            this.splitContainer1.Panel2.Controls.Add(this.groupBoxFunctionKeys);
            this.splitContainer1.Panel2.Controls.Add(this.groupBoxSpecialKeys);
            this.splitContainer1.Size = new System.Drawing.Size(1435, 655);
            this.splitContainer1.SplitterDistance = 694;
            this.splitContainer1.TabIndex = 0;
            // 
            // groupBoxSecurityOptions
            // 
            this.groupBoxSecurityOptions.Controls.Add(this.checkBoxShutdownAfterAutostart);
            this.groupBoxSecurityOptions.Controls.Add(this.checkBoxMonitorProcesses);
            this.groupBoxSecurityOptions.Controls.Add(this.checkBoxEditRegistry);
            this.groupBoxSecurityOptions.Controls.Add(this.checkBoxHookMessages);
            this.groupBoxSecurityOptions.Controls.Add(this.checkBoxShowSebApplicationChooser);
            this.groupBoxSecurityOptions.Controls.Add(this.checkBoxCreateNewDesktop);
            this.groupBoxSecurityOptions.Controls.Add(this.checkBoxAllowVirtualMachine);
            this.groupBoxSecurityOptions.Controls.Add(this.checkBoxForceWindowsService);
            this.groupBoxSecurityOptions.Location = new System.Drawing.Point(294, 166);
            this.groupBoxSecurityOptions.Name = "groupBoxSecurityOptions";
            this.groupBoxSecurityOptions.Size = new System.Drawing.Size(319, 259);
            this.groupBoxSecurityOptions.TabIndex = 45;
            this.groupBoxSecurityOptions.TabStop = false;
            this.groupBoxSecurityOptions.Text = "Security options";
            // 
            // checkBoxShutdownAfterAutostart
            // 
            this.checkBoxShutdownAfterAutostart.AutoSize = true;
            this.checkBoxShutdownAfterAutostart.Enabled = false;
            this.checkBoxShutdownAfterAutostart.Location = new System.Drawing.Point(6, 226);
            this.checkBoxShutdownAfterAutostart.Name = "checkBoxShutdownAfterAutostart";
            this.checkBoxShutdownAfterAutostart.Size = new System.Drawing.Size(309, 21);
            this.checkBoxShutdownAfterAutostart.TabIndex = 50;
            this.checkBoxShutdownAfterAutostart.Text = "Shutdown after autostart process terminates";
            this.checkBoxShutdownAfterAutostart.UseVisualStyleBackColor = true;
            this.checkBoxShutdownAfterAutostart.CheckedChanged += new System.EventHandler(this.checkBoxShutdownAfterAutostart_CheckedChanged);
            // 
            // checkBoxMonitorProcesses
            // 
            this.checkBoxMonitorProcesses.AutoSize = true;
            this.checkBoxMonitorProcesses.Enabled = false;
            this.checkBoxMonitorProcesses.Location = new System.Drawing.Point(6, 199);
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
            this.checkBoxEditRegistry.Location = new System.Drawing.Point(6, 172);
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
            this.checkBoxHookMessages.Location = new System.Drawing.Point(6, 145);
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
            this.checkBoxShowSebApplicationChooser.Location = new System.Drawing.Point(6, 118);
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
            this.checkBoxCreateNewDesktop.Location = new System.Drawing.Point(6, 91);
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
            this.groupBoxSebStarterIni.Controls.Add(this.checkBoxWriteLogFileSebStarterLog);
            this.groupBoxSebStarterIni.Location = new System.Drawing.Point(24, 38);
            this.groupBoxSebStarterIni.Name = "groupBoxSebStarterIni";
            this.groupBoxSebStarterIni.Size = new System.Drawing.Size(589, 100);
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
            // checkBoxWriteLogFileSebStarterLog
            // 
            this.checkBoxWriteLogFileSebStarterLog.AutoSize = true;
            this.checkBoxWriteLogFileSebStarterLog.Location = new System.Drawing.Point(373, 48);
            this.checkBoxWriteLogFileSebStarterLog.Name = "checkBoxWriteLogFileSebStarterLog";
            this.checkBoxWriteLogFileSebStarterLog.Size = new System.Drawing.Size(199, 21);
            this.checkBoxWriteLogFileSebStarterLog.TabIndex = 42;
            this.checkBoxWriteLogFileSebStarterLog.Text = "Write logfile SebStarter.log";
            this.checkBoxWriteLogFileSebStarterLog.UseVisualStyleBackColor = true;
            this.checkBoxWriteLogFileSebStarterLog.CheckedChanged += new System.EventHandler(this.checkBoxWriteLogFileSebStarterLog_CheckedChanged);
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
            this.groupBoxOnlineExam.Location = new System.Drawing.Point(24, 456);
            this.groupBoxOnlineExam.Name = "groupBoxOnlineExam";
            this.groupBoxOnlineExam.Size = new System.Drawing.Size(589, 149);
            this.groupBoxOnlineExam.TabIndex = 25;
            this.groupBoxOnlineExam.TabStop = false;
            this.groupBoxOnlineExam.Text = "Online exam";
            // 
            // textBoxAutostartProcess
            // 
            this.textBoxAutostartProcess.Location = new System.Drawing.Point(153, 59);
            this.textBoxAutostartProcess.Name = "textBoxAutostartProcess";
            this.textBoxAutostartProcess.Size = new System.Drawing.Size(419, 22);
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
            this.textBoxSebBrowser.Size = new System.Drawing.Size(419, 22);
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
            this.textBoxPermittedApplications.Size = new System.Drawing.Size(419, 22);
            this.textBoxPermittedApplications.TabIndex = 23;
            this.textBoxPermittedApplications.TextChanged += new System.EventHandler(this.textBoxPermittedApplications_TextChanged);
            // 
            // textBoxExamUrl
            // 
            this.textBoxExamUrl.Location = new System.Drawing.Point(153, 87);
            this.textBoxExamUrl.Name = "textBoxExamUrl";
            this.textBoxExamUrl.Size = new System.Drawing.Size(419, 22);
            this.textBoxExamUrl.TabIndex = 20;
            this.textBoxExamUrl.TextChanged += new System.EventHandler(this.textBoxExamUrl_TextChanged);
            // 
            // groupBoxRegistryValues
            // 
            this.groupBoxRegistryValues.Controls.Add(this.checkBoxEnableSwitchUser);
            this.groupBoxRegistryValues.Controls.Add(this.checkBoxEnableLockThisComputer);
            this.groupBoxRegistryValues.Controls.Add(this.checkBoxEnableChangeAPassword);
            this.groupBoxRegistryValues.Controls.Add(this.checkBoxEnableStartTaskManager);
            this.groupBoxRegistryValues.Controls.Add(this.checkBoxEnableLogOff);
            this.groupBoxRegistryValues.Controls.Add(this.checkBoxEnableShutDown);
            this.groupBoxRegistryValues.Controls.Add(this.checkBoxEnableEaseOfAccess);
            this.groupBoxRegistryValues.Controls.Add(this.checkBoxEnableVmWareClientShade);
            this.groupBoxRegistryValues.Location = new System.Drawing.Point(24, 166);
            this.groupBoxRegistryValues.Name = "groupBoxRegistryValues";
            this.groupBoxRegistryValues.Size = new System.Drawing.Size(264, 259);
            this.groupBoxRegistryValues.TabIndex = 24;
            this.groupBoxRegistryValues.TabStop = false;
            this.groupBoxRegistryValues.Text = "Registry values";
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
            // groupBoxExitSequence
            // 
            this.groupBoxExitSequence.Controls.Add(this.listBoxExitKey1);
            this.groupBoxExitSequence.Controls.Add(this.listBoxExitKey3);
            this.groupBoxExitSequence.Controls.Add(this.listBoxExitKey2);
            this.groupBoxExitSequence.Location = new System.Drawing.Point(360, 166);
            this.groupBoxExitSequence.Name = "groupBoxExitSequence";
            this.groupBoxExitSequence.Size = new System.Drawing.Size(156, 243);
            this.groupBoxExitSequence.TabIndex = 51;
            this.groupBoxExitSequence.TabStop = false;
            this.groupBoxExitSequence.Text = "Exit sequence";
            // 
            // listBoxExitKey1
            // 
            this.listBoxExitKey1.FormattingEnabled = true;
            this.listBoxExitKey1.ItemHeight = 16;
            this.listBoxExitKey1.Items.AddRange(new object[] {
            "F1",
            "F2",
            "F3",
            "F4",
            "F5",
            "F6",
            "F7",
            "F8",
            "F9",
            "F10",
            "F11",
            "F12"});
            this.listBoxExitKey1.Location = new System.Drawing.Point(19, 36);
            this.listBoxExitKey1.Name = "listBoxExitKey1";
            this.listBoxExitKey1.Size = new System.Drawing.Size(38, 196);
            this.listBoxExitKey1.TabIndex = 47;
            this.listBoxExitKey1.SelectedIndexChanged += new System.EventHandler(this.listBoxExitKeyFirst_SelectedIndexChanged);
            // 
            // listBoxExitKey3
            // 
            this.listBoxExitKey3.FormattingEnabled = true;
            this.listBoxExitKey3.ItemHeight = 16;
            this.listBoxExitKey3.Items.AddRange(new object[] {
            "F1",
            "F2",
            "F3",
            "F4",
            "F5",
            "F6",
            "F7",
            "F8",
            "F9",
            "F10",
            "F11",
            "F12"});
            this.listBoxExitKey3.Location = new System.Drawing.Point(107, 36);
            this.listBoxExitKey3.Name = "listBoxExitKey3";
            this.listBoxExitKey3.Size = new System.Drawing.Size(38, 196);
            this.listBoxExitKey3.TabIndex = 50;
            this.listBoxExitKey3.SelectedIndexChanged += new System.EventHandler(this.listBoxExitKeyThird_SelectedIndexChanged);
            // 
            // listBoxExitKey2
            // 
            this.listBoxExitKey2.FormattingEnabled = true;
            this.listBoxExitKey2.ItemHeight = 16;
            this.listBoxExitKey2.Items.AddRange(new object[] {
            "F1",
            "F2",
            "F3",
            "F4",
            "F5",
            "F6",
            "F7",
            "F8",
            "F9",
            "F10",
            "F11",
            "F12"});
            this.listBoxExitKey2.Location = new System.Drawing.Point(63, 36);
            this.listBoxExitKey2.Name = "listBoxExitKey2";
            this.listBoxExitKey2.Size = new System.Drawing.Size(38, 196);
            this.listBoxExitKey2.TabIndex = 49;
            this.listBoxExitKey2.SelectedIndexChanged += new System.EventHandler(this.listBoxExitKeySecond_SelectedIndexChanged);
            // 
            // textBoxDebug
            // 
            this.textBoxDebug.Location = new System.Drawing.Point(22, 569);
            this.textBoxDebug.Name = "textBoxDebug";
            this.textBoxDebug.Size = new System.Drawing.Size(419, 22);
            this.textBoxDebug.TabIndex = 46;
            // 
            // groupBoxMsgHookIni
            // 
            this.groupBoxMsgHookIni.Controls.Add(this.labelOpenFileMsgHookIni);
            this.groupBoxMsgHookIni.Controls.Add(this.labelSaveFileMsgHookIni);
            this.groupBoxMsgHookIni.Controls.Add(this.checkBoxWriteLogFileMsgHookLog);
            this.groupBoxMsgHookIni.Controls.Add(this.buttonRestoreSettingsOfMsgHookIni);
            this.groupBoxMsgHookIni.Location = new System.Drawing.Point(16, 38);
            this.groupBoxMsgHookIni.Name = "groupBoxMsgHookIni";
            this.groupBoxMsgHookIni.Size = new System.Drawing.Size(557, 100);
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
            // checkBoxWriteLogFileMsgHookLog
            // 
            this.checkBoxWriteLogFileMsgHookLog.AutoSize = true;
            this.checkBoxWriteLogFileMsgHookLog.Location = new System.Drawing.Point(361, 48);
            this.checkBoxWriteLogFileMsgHookLog.Name = "checkBoxWriteLogFileMsgHookLog";
            this.checkBoxWriteLogFileMsgHookLog.Size = new System.Drawing.Size(190, 21);
            this.checkBoxWriteLogFileMsgHookLog.TabIndex = 41;
            this.checkBoxWriteLogFileMsgHookLog.Text = "Write logfile MsgHook.log";
            this.checkBoxWriteLogFileMsgHookLog.UseVisualStyleBackColor = true;
            this.checkBoxWriteLogFileMsgHookLog.CheckedChanged += new System.EventHandler(this.checkBoxWriteLogFileMsgHookLog_CheckedChanged);
            // 
            // buttonRestoreSettingsOfMsgHookIni
            // 
            this.buttonRestoreSettingsOfMsgHookIni.Location = new System.Drawing.Point(185, 21);
            this.buttonRestoreSettingsOfMsgHookIni.Name = "buttonRestoreSettingsOfMsgHookIni";
            this.buttonRestoreSettingsOfMsgHookIni.Size = new System.Drawing.Size(139, 72);
            this.buttonRestoreSettingsOfMsgHookIni.TabIndex = 45;
            this.buttonRestoreSettingsOfMsgHookIni.Text = "Restore settings of MsgHook.ini";
            this.buttonRestoreSettingsOfMsgHookIni.UseVisualStyleBackColor = true;
            this.buttonRestoreSettingsOfMsgHookIni.Click += new System.EventHandler(this.buttonRestoreSettingsOfMsgHookIni_Click);
            // 
            // groupBoxFunctionKeys
            // 
            this.groupBoxFunctionKeys.Controls.Add(this.checkBoxEnableF1);
            this.groupBoxFunctionKeys.Controls.Add(this.checkBoxEnableF2);
            this.groupBoxFunctionKeys.Controls.Add(this.checkBoxEnableF12);
            this.groupBoxFunctionKeys.Controls.Add(this.checkBoxEnableF3);
            this.groupBoxFunctionKeys.Controls.Add(this.checkBoxEnableF11);
            this.groupBoxFunctionKeys.Controls.Add(this.checkBoxEnableF4);
            this.groupBoxFunctionKeys.Controls.Add(this.checkBoxEnableF5);
            this.groupBoxFunctionKeys.Controls.Add(this.checkBoxEnableF10);
            this.groupBoxFunctionKeys.Controls.Add(this.checkBoxEnableF6);
            this.groupBoxFunctionKeys.Controls.Add(this.checkBoxEnableF9);
            this.groupBoxFunctionKeys.Controls.Add(this.checkBoxEnableF7);
            this.groupBoxFunctionKeys.Controls.Add(this.checkBoxEnableF8);
            this.groupBoxFunctionKeys.Location = new System.Drawing.Point(222, 166);
            this.groupBoxFunctionKeys.Name = "groupBoxFunctionKeys";
            this.groupBoxFunctionKeys.Size = new System.Drawing.Size(132, 362);
            this.groupBoxFunctionKeys.TabIndex = 39;
            this.groupBoxFunctionKeys.TabStop = false;
            this.groupBoxFunctionKeys.Text = "Function keys";
            // 
            // checkBoxEnableF1
            // 
            this.checkBoxEnableF1.AutoSize = true;
            this.checkBoxEnableF1.Location = new System.Drawing.Point(6, 36);
            this.checkBoxEnableF1.Name = "checkBoxEnableF1";
            this.checkBoxEnableF1.Size = new System.Drawing.Size(94, 21);
            this.checkBoxEnableF1.TabIndex = 25;
            this.checkBoxEnableF1.Text = "Enable F1";
            this.checkBoxEnableF1.UseVisualStyleBackColor = true;
            // 
            // checkBoxEnableF2
            // 
            this.checkBoxEnableF2.AutoSize = true;
            this.checkBoxEnableF2.Location = new System.Drawing.Point(6, 63);
            this.checkBoxEnableF2.Name = "checkBoxEnableF2";
            this.checkBoxEnableF2.Size = new System.Drawing.Size(94, 21);
            this.checkBoxEnableF2.TabIndex = 26;
            this.checkBoxEnableF2.Text = "Enable F2";
            this.checkBoxEnableF2.UseVisualStyleBackColor = true;
            // 
            // checkBoxEnableF12
            // 
            this.checkBoxEnableF12.AutoSize = true;
            this.checkBoxEnableF12.Location = new System.Drawing.Point(6, 333);
            this.checkBoxEnableF12.Name = "checkBoxEnableF12";
            this.checkBoxEnableF12.Size = new System.Drawing.Size(102, 21);
            this.checkBoxEnableF12.TabIndex = 37;
            this.checkBoxEnableF12.Text = "Enable F12";
            this.checkBoxEnableF12.UseVisualStyleBackColor = true;
            // 
            // checkBoxEnableF3
            // 
            this.checkBoxEnableF3.AutoSize = true;
            this.checkBoxEnableF3.Location = new System.Drawing.Point(6, 90);
            this.checkBoxEnableF3.Name = "checkBoxEnableF3";
            this.checkBoxEnableF3.Size = new System.Drawing.Size(94, 21);
            this.checkBoxEnableF3.TabIndex = 27;
            this.checkBoxEnableF3.Text = "Enable F3";
            this.checkBoxEnableF3.UseVisualStyleBackColor = true;
            // 
            // checkBoxEnableF11
            // 
            this.checkBoxEnableF11.AutoSize = true;
            this.checkBoxEnableF11.Location = new System.Drawing.Point(6, 306);
            this.checkBoxEnableF11.Name = "checkBoxEnableF11";
            this.checkBoxEnableF11.Size = new System.Drawing.Size(102, 21);
            this.checkBoxEnableF11.TabIndex = 36;
            this.checkBoxEnableF11.Text = "Enable F11";
            this.checkBoxEnableF11.UseVisualStyleBackColor = true;
            // 
            // checkBoxEnableF4
            // 
            this.checkBoxEnableF4.AutoSize = true;
            this.checkBoxEnableF4.Location = new System.Drawing.Point(6, 117);
            this.checkBoxEnableF4.Name = "checkBoxEnableF4";
            this.checkBoxEnableF4.Size = new System.Drawing.Size(94, 21);
            this.checkBoxEnableF4.TabIndex = 28;
            this.checkBoxEnableF4.Text = "Enable F4";
            this.checkBoxEnableF4.UseVisualStyleBackColor = true;
            // 
            // checkBoxEnableF5
            // 
            this.checkBoxEnableF5.AutoSize = true;
            this.checkBoxEnableF5.Location = new System.Drawing.Point(6, 144);
            this.checkBoxEnableF5.Name = "checkBoxEnableF5";
            this.checkBoxEnableF5.Size = new System.Drawing.Size(94, 21);
            this.checkBoxEnableF5.TabIndex = 29;
            this.checkBoxEnableF5.Text = "Enable F5";
            this.checkBoxEnableF5.UseVisualStyleBackColor = true;
            // 
            // checkBoxEnableF10
            // 
            this.checkBoxEnableF10.AutoSize = true;
            this.checkBoxEnableF10.Location = new System.Drawing.Point(6, 279);
            this.checkBoxEnableF10.Name = "checkBoxEnableF10";
            this.checkBoxEnableF10.Size = new System.Drawing.Size(102, 21);
            this.checkBoxEnableF10.TabIndex = 34;
            this.checkBoxEnableF10.Text = "Enable F10";
            this.checkBoxEnableF10.UseVisualStyleBackColor = true;
            // 
            // checkBoxEnableF6
            // 
            this.checkBoxEnableF6.AutoSize = true;
            this.checkBoxEnableF6.Location = new System.Drawing.Point(6, 171);
            this.checkBoxEnableF6.Name = "checkBoxEnableF6";
            this.checkBoxEnableF6.Size = new System.Drawing.Size(94, 21);
            this.checkBoxEnableF6.TabIndex = 30;
            this.checkBoxEnableF6.Text = "Enable F6";
            this.checkBoxEnableF6.UseVisualStyleBackColor = true;
            // 
            // checkBoxEnableF9
            // 
            this.checkBoxEnableF9.AutoSize = true;
            this.checkBoxEnableF9.Location = new System.Drawing.Point(6, 252);
            this.checkBoxEnableF9.Name = "checkBoxEnableF9";
            this.checkBoxEnableF9.Size = new System.Drawing.Size(94, 21);
            this.checkBoxEnableF9.TabIndex = 33;
            this.checkBoxEnableF9.Text = "Enable F9";
            this.checkBoxEnableF9.UseVisualStyleBackColor = true;
            // 
            // checkBoxEnableF7
            // 
            this.checkBoxEnableF7.AutoSize = true;
            this.checkBoxEnableF7.Location = new System.Drawing.Point(6, 198);
            this.checkBoxEnableF7.Name = "checkBoxEnableF7";
            this.checkBoxEnableF7.Size = new System.Drawing.Size(94, 21);
            this.checkBoxEnableF7.TabIndex = 31;
            this.checkBoxEnableF7.Text = "Enable F7";
            this.checkBoxEnableF7.UseVisualStyleBackColor = true;
            // 
            // checkBoxEnableF8
            // 
            this.checkBoxEnableF8.AutoSize = true;
            this.checkBoxEnableF8.Location = new System.Drawing.Point(6, 225);
            this.checkBoxEnableF8.Name = "checkBoxEnableF8";
            this.checkBoxEnableF8.Size = new System.Drawing.Size(94, 21);
            this.checkBoxEnableF8.TabIndex = 32;
            this.checkBoxEnableF8.Text = "Enable F8";
            this.checkBoxEnableF8.UseVisualStyleBackColor = true;
            // 
            // groupBoxSpecialKeys
            // 
            this.groupBoxSpecialKeys.Controls.Add(this.checkBoxEnableEsc);
            this.groupBoxSpecialKeys.Controls.Add(this.checkBoxEnableCtrlEsc);
            this.groupBoxSpecialKeys.Controls.Add(this.checkBoxEnableAltEsc);
            this.groupBoxSpecialKeys.Controls.Add(this.checkBoxEnableAltTab);
            this.groupBoxSpecialKeys.Controls.Add(this.checkBoxEnableAltF4);
            this.groupBoxSpecialKeys.Controls.Add(this.checkBoxEnableStartMenu);
            this.groupBoxSpecialKeys.Controls.Add(this.checkBoxEnableRightMouse);
            this.groupBoxSpecialKeys.Location = new System.Drawing.Point(16, 166);
            this.groupBoxSpecialKeys.Name = "groupBoxSpecialKeys";
            this.groupBoxSpecialKeys.Size = new System.Drawing.Size(200, 234);
            this.groupBoxSpecialKeys.TabIndex = 38;
            this.groupBoxSpecialKeys.TabStop = false;
            this.groupBoxSpecialKeys.Text = "Special keys";
            // 
            // checkBoxEnableEsc
            // 
            this.checkBoxEnableEsc.AutoSize = true;
            this.checkBoxEnableEsc.Location = new System.Drawing.Point(6, 37);
            this.checkBoxEnableEsc.Name = "checkBoxEnableEsc";
            this.checkBoxEnableEsc.Size = new System.Drawing.Size(101, 21);
            this.checkBoxEnableEsc.TabIndex = 41;
            this.checkBoxEnableEsc.Text = "Enable Esc";
            this.checkBoxEnableEsc.UseVisualStyleBackColor = true;
            // 
            // checkBoxEnableCtrlEsc
            // 
            this.checkBoxEnableCtrlEsc.AutoSize = true;
            this.checkBoxEnableCtrlEsc.Location = new System.Drawing.Point(6, 64);
            this.checkBoxEnableCtrlEsc.Name = "checkBoxEnableCtrlEsc";
            this.checkBoxEnableCtrlEsc.Size = new System.Drawing.Size(127, 21);
            this.checkBoxEnableCtrlEsc.TabIndex = 19;
            this.checkBoxEnableCtrlEsc.Text = "Enable Ctrl-Esc";
            this.checkBoxEnableCtrlEsc.UseVisualStyleBackColor = true;
            // 
            // checkBoxEnableAltEsc
            // 
            this.checkBoxEnableAltEsc.AutoSize = true;
            this.checkBoxEnableAltEsc.Location = new System.Drawing.Point(6, 91);
            this.checkBoxEnableAltEsc.Name = "checkBoxEnableAltEsc";
            this.checkBoxEnableAltEsc.Size = new System.Drawing.Size(122, 21);
            this.checkBoxEnableAltEsc.TabIndex = 20;
            this.checkBoxEnableAltEsc.Text = "Enable Alt-Esc";
            this.checkBoxEnableAltEsc.UseVisualStyleBackColor = true;
            // 
            // checkBoxEnableAltTab
            // 
            this.checkBoxEnableAltTab.AutoSize = true;
            this.checkBoxEnableAltTab.Location = new System.Drawing.Point(6, 118);
            this.checkBoxEnableAltTab.Name = "checkBoxEnableAltTab";
            this.checkBoxEnableAltTab.Size = new System.Drawing.Size(124, 21);
            this.checkBoxEnableAltTab.TabIndex = 21;
            this.checkBoxEnableAltTab.Text = "Enable Alt-Tab";
            this.checkBoxEnableAltTab.UseVisualStyleBackColor = true;
            // 
            // checkBoxEnableAltF4
            // 
            this.checkBoxEnableAltF4.AutoSize = true;
            this.checkBoxEnableAltF4.Location = new System.Drawing.Point(6, 145);
            this.checkBoxEnableAltF4.Name = "checkBoxEnableAltF4";
            this.checkBoxEnableAltF4.Size = new System.Drawing.Size(115, 21);
            this.checkBoxEnableAltF4.TabIndex = 22;
            this.checkBoxEnableAltF4.Text = "Enable Alt-F4";
            this.checkBoxEnableAltF4.UseVisualStyleBackColor = true;
            // 
            // checkBoxEnableStartMenu
            // 
            this.checkBoxEnableStartMenu.AutoSize = true;
            this.checkBoxEnableStartMenu.Location = new System.Drawing.Point(6, 172);
            this.checkBoxEnableStartMenu.Name = "checkBoxEnableStartMenu";
            this.checkBoxEnableStartMenu.Size = new System.Drawing.Size(147, 21);
            this.checkBoxEnableStartMenu.TabIndex = 23;
            this.checkBoxEnableStartMenu.Text = "Enable Start Menu";
            this.checkBoxEnableStartMenu.UseVisualStyleBackColor = true;
            // 
            // checkBoxEnableRightMouse
            // 
            this.checkBoxEnableRightMouse.AutoSize = true;
            this.checkBoxEnableRightMouse.Location = new System.Drawing.Point(6, 199);
            this.checkBoxEnableRightMouse.Name = "checkBoxEnableRightMouse";
            this.checkBoxEnableRightMouse.Size = new System.Drawing.Size(157, 21);
            this.checkBoxEnableRightMouse.TabIndex = 24;
            this.checkBoxEnableRightMouse.Text = "Enable Right Mouse";
            this.checkBoxEnableRightMouse.UseVisualStyleBackColor = true;
            // 
            // openFileDialogSebStarterIni
            // 
            this.openFileDialogSebStarterIni.FileName = "SebStarter.ini";
            // 
            // saveFileDialogSebStarterIni
            // 
            this.saveFileDialogSebStarterIni.FileName = "SebStarter.ini";
            // 
            // openFileDialogMsgHookIni
            // 
            this.openFileDialogMsgHookIni.FileName = "MsgHook.ini";
            // 
            // saveFileDialogMsgHookIni
            // 
            this.saveFileDialogMsgHookIni.FileName = "MsgHook.ini";
            // 
            // SebWindowsConfigForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
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
            this.groupBoxSecurityOptions.ResumeLayout(false);
            this.groupBoxSecurityOptions.PerformLayout();
            this.groupBoxSebStarterIni.ResumeLayout(false);
            this.groupBoxSebStarterIni.PerformLayout();
            this.groupBoxOnlineExam.ResumeLayout(false);
            this.groupBoxOnlineExam.PerformLayout();
            this.groupBoxRegistryValues.ResumeLayout(false);
            this.groupBoxRegistryValues.PerformLayout();
            this.groupBoxExitSequence.ResumeLayout(false);
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

        private System.Windows.Forms.GroupBox groupBoxSebStarterIni;
        private System.Windows.Forms.GroupBox groupBoxMsgHookIni;

        private System.Windows.Forms.GroupBox groupBoxRegistryValues;
        private System.Windows.Forms.GroupBox groupBoxSecurityOptions;
        private System.Windows.Forms.GroupBox groupBoxOnlineExam;

        private System.Windows.Forms.GroupBox groupBoxFunctionKeys;
        private System.Windows.Forms.GroupBox groupBoxSpecialKeys;
        private System.Windows.Forms.GroupBox groupBoxExitSequence;

        private System.Windows.Forms.OpenFileDialog openFileDialogSebStarterIni;
        private System.Windows.Forms.SaveFileDialog saveFileDialogSebStarterIni;
        private System.Windows.Forms.OpenFileDialog openFileDialogMsgHookIni;
        private System.Windows.Forms.SaveFileDialog saveFileDialogMsgHookIni;

        private System.Windows.Forms.Label labelOpenFileSebStarterIni;
        private System.Windows.Forms.Label labelSaveFileSebStarterIni;
        private System.Windows.Forms.Label labelOpenFileMsgHookIni;
        private System.Windows.Forms.Label labelSaveFileMsgHookIni;

        private System.Windows.Forms.Button buttonRestoreSettingsOfSebStarterIni;
        private System.Windows.Forms.Button buttonRestoreSettingsOfMsgHookIni;

        private System.Windows.Forms.CheckBox checkBoxWriteLogFileMsgHookLog;
        private System.Windows.Forms.CheckBox checkBoxWriteLogFileSebStarterLog;

        private System.Windows.Forms.CheckBox checkBoxEnableSwitchUser;
        private System.Windows.Forms.CheckBox checkBoxEnableLogOff;
        private System.Windows.Forms.CheckBox checkBoxEnableShutDown;
        private System.Windows.Forms.CheckBox checkBoxEnableChangeAPassword;
        private System.Windows.Forms.CheckBox checkBoxEnableStartTaskManager;
        private System.Windows.Forms.CheckBox checkBoxEnableLockThisComputer;
        private System.Windows.Forms.CheckBox checkBoxEnableEaseOfAccess;
        private System.Windows.Forms.CheckBox checkBoxEnableVmWareClientShade;

        private System.Windows.Forms.CheckBox checkBoxForceWindowsService;
        private System.Windows.Forms.CheckBox checkBoxAllowVirtualMachine;
        private System.Windows.Forms.CheckBox checkBoxEditRegistry;
        private System.Windows.Forms.CheckBox checkBoxHookMessages;
        private System.Windows.Forms.CheckBox checkBoxShowSebApplicationChooser;
        private System.Windows.Forms.CheckBox checkBoxCreateNewDesktop;
        private System.Windows.Forms.CheckBox checkBoxMonitorProcesses;
        private System.Windows.Forms.CheckBox checkBoxShutdownAfterAutostart;

        private System.Windows.Forms.TextBox textBoxSebBrowser;
        private System.Windows.Forms.TextBox textBoxAutostartProcess;
        private System.Windows.Forms.TextBox textBoxExamUrl;
        private System.Windows.Forms.TextBox textBoxPermittedApplications;
        private System.Windows.Forms.Label labelSebBrowser;
        private System.Windows.Forms.Label labelAutostartProcess;
        private System.Windows.Forms.Label labelExamUrl;
        private System.Windows.Forms.Label labelPermittedApplications;

        private System.Windows.Forms.CheckBox checkBoxEnableEsc;
        private System.Windows.Forms.CheckBox checkBoxEnableCtrlEsc;
        private System.Windows.Forms.CheckBox checkBoxEnableAltEsc;
        private System.Windows.Forms.CheckBox checkBoxEnableAltTab;
        private System.Windows.Forms.CheckBox checkBoxEnableAltF4;
        private System.Windows.Forms.CheckBox checkBoxEnableStartMenu;
        private System.Windows.Forms.CheckBox checkBoxEnableRightMouse;

        private System.Windows.Forms.CheckBox checkBoxEnableF1;
        private System.Windows.Forms.CheckBox checkBoxEnableF2;
        private System.Windows.Forms.CheckBox checkBoxEnableF3;
        private System.Windows.Forms.CheckBox checkBoxEnableF4;
        private System.Windows.Forms.CheckBox checkBoxEnableF5;
        private System.Windows.Forms.CheckBox checkBoxEnableF6;
        private System.Windows.Forms.CheckBox checkBoxEnableF7;
        private System.Windows.Forms.CheckBox checkBoxEnableF8;
        private System.Windows.Forms.CheckBox checkBoxEnableF9;
        private System.Windows.Forms.CheckBox checkBoxEnableF10;
        private System.Windows.Forms.CheckBox checkBoxEnableF11;
        private System.Windows.Forms.CheckBox checkBoxEnableF12;

        private System.Windows.Forms.ListBox listBoxExitKey1;
        private System.Windows.Forms.ListBox listBoxExitKey3;
        private System.Windows.Forms.ListBox listBoxExitKey2;

        private System.Windows.Forms.TextBox textBoxDebug;

    }
}

