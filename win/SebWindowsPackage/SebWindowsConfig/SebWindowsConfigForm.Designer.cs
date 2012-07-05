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
            this.splitContainerSebConfigExe = new System.Windows.Forms.SplitContainer();
            this.groupBoxSecurityOptions = new System.Windows.Forms.GroupBox();
            this.checkBoxShutdownAfterAutostart = new System.Windows.Forms.CheckBox();
            this.checkBoxMonitorProcesses = new System.Windows.Forms.CheckBox();
            this.checkBoxEditRegistry = new System.Windows.Forms.CheckBox();
            this.checkBoxHookMessages = new System.Windows.Forms.CheckBox();
            this.checkBoxShowSebApplicationChooser = new System.Windows.Forms.CheckBox();
            this.checkBoxCreateNewDesktop = new System.Windows.Forms.CheckBox();
            this.checkBoxAllowVirtualMachine = new System.Windows.Forms.CheckBox();
            this.checkBoxForceWindowsService = new System.Windows.Forms.CheckBox();
            this.groupBoxSebStarterFiles = new System.Windows.Forms.GroupBox();
            this.textBoxCurrentFileSebStarterIni = new System.Windows.Forms.TextBox();
            this.buttonDefaultSebStarterSettings = new System.Windows.Forms.Button();
            this.textBoxCurrentDireSebStarterIni = new System.Windows.Forms.TextBox();
            this.labelOpenSebStarterConfigFile = new System.Windows.Forms.Label();
            this.checkBoxWriteSebStarterLogFile = new System.Windows.Forms.CheckBox();
            this.labelSaveSebStarterConfigFile = new System.Windows.Forms.Label();
            this.buttonRestoreSebStarterConfigFile = new System.Windows.Forms.Button();
            this.groupBoxOnlineExam = new System.Windows.Forms.GroupBox();
            this.labelQuitHashCode = new System.Windows.Forms.Label();
            this.textBoxQuitHashcode = new System.Windows.Forms.TextBox();
            this.labelQuitPassword = new System.Windows.Forms.Label();
            this.textBoxQuitPassword = new System.Windows.Forms.TextBox();
            this.textBoxAutostartProcess = new System.Windows.Forms.TextBox();
            this.labelSebBrowser = new System.Windows.Forms.Label();
            this.labelAutostartProcess = new System.Windows.Forms.Label();
            this.textBoxSebBrowser = new System.Windows.Forms.TextBox();
            this.labelExamUrl = new System.Windows.Forms.Label();
            this.labelPermittedApplications = new System.Windows.Forms.Label();
            this.textBoxPermittedApplications = new System.Windows.Forms.TextBox();
            this.textBoxExamUrl = new System.Windows.Forms.TextBox();
            this.groupBoxInsideOutsideSeb = new System.Windows.Forms.GroupBox();
            this.checkBoxInsideSebEnableVmWareClientShade = new System.Windows.Forms.CheckBox();
            this.checkBoxInsideSebEnableEaseOfAccess = new System.Windows.Forms.CheckBox();
            this.checkBoxInsideSebEnableShutDown = new System.Windows.Forms.CheckBox();
            this.checkBoxInsideSebEnableLogOff = new System.Windows.Forms.CheckBox();
            this.checkBoxInsideSebEnableStartTaskManager = new System.Windows.Forms.CheckBox();
            this.checkBoxInsideSebEnableChangeAPassword = new System.Windows.Forms.CheckBox();
            this.checkBoxInsideSebEnableLockThisComputer = new System.Windows.Forms.CheckBox();
            this.checkBoxInsideSebEnableSwitchUser = new System.Windows.Forms.CheckBox();
            this.checkBoxOutsideSebEnableSwitchUser = new System.Windows.Forms.CheckBox();
            this.checkBoxOutsideSebEnableLockThisComputer = new System.Windows.Forms.CheckBox();
            this.checkBoxOutsideSebEnableChangeAPassword = new System.Windows.Forms.CheckBox();
            this.checkBoxOutsideSebEnableStartTaskManager = new System.Windows.Forms.CheckBox();
            this.checkBoxOutsideSebEnableLogOff = new System.Windows.Forms.CheckBox();
            this.checkBoxOutsideSebEnableShutDown = new System.Windows.Forms.CheckBox();
            this.checkBoxOutsideSebEnableEaseOfAccess = new System.Windows.Forms.CheckBox();
            this.checkBoxOutsideSebEnableVmWareClientShade = new System.Windows.Forms.CheckBox();
            this.buttonExitWithoutSaving = new System.Windows.Forms.Button();
            this.buttonSaveBothConfigFilesAndExit = new System.Windows.Forms.Button();
            this.groupBoxExitSequence = new System.Windows.Forms.GroupBox();
            this.listBoxExitKey1 = new System.Windows.Forms.ListBox();
            this.listBoxExitKey3 = new System.Windows.Forms.ListBox();
            this.listBoxExitKey2 = new System.Windows.Forms.ListBox();
            this.groupBoxMsgHookFiles = new System.Windows.Forms.GroupBox();
            this.textBoxCurrentFileMsgHookIni = new System.Windows.Forms.TextBox();
            this.buttonDefaultMsgHookSettings = new System.Windows.Forms.Button();
            this.textBoxCurrentDireMsgHookIni = new System.Windows.Forms.TextBox();
            this.labelOpenMsgHookConfigFile = new System.Windows.Forms.Label();
            this.labelSaveMsgHookConfigFile = new System.Windows.Forms.Label();
            this.checkBoxWriteMsgHookLogFile = new System.Windows.Forms.CheckBox();
            this.buttonRestoreMsgHookConfigFile = new System.Windows.Forms.Button();
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
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerSebConfigExe)).BeginInit();
            this.splitContainerSebConfigExe.Panel1.SuspendLayout();
            this.splitContainerSebConfigExe.Panel2.SuspendLayout();
            this.splitContainerSebConfigExe.SuspendLayout();
            this.groupBoxSecurityOptions.SuspendLayout();
            this.groupBoxSebStarterFiles.SuspendLayout();
            this.groupBoxOnlineExam.SuspendLayout();
            this.groupBoxInsideOutsideSeb.SuspendLayout();
            this.groupBoxExitSequence.SuspendLayout();
            this.groupBoxMsgHookFiles.SuspendLayout();
            this.groupBoxFunctionKeys.SuspendLayout();
            this.groupBoxSpecialKeys.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainerSebConfigExe
            // 
            this.splitContainerSebConfigExe.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerSebConfigExe.Location = new System.Drawing.Point(0, 0);
            this.splitContainerSebConfigExe.Name = "splitContainerSebConfigExe";
            // 
            // splitContainerSebConfigExe.Panel1
            // 
            this.splitContainerSebConfigExe.Panel1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("splitContainerSebConfigExe.Panel1.BackgroundImage")));
            this.splitContainerSebConfigExe.Panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.splitContainerSebConfigExe.Panel1.Controls.Add(this.groupBoxSecurityOptions);
            this.splitContainerSebConfigExe.Panel1.Controls.Add(this.groupBoxSebStarterFiles);
            this.splitContainerSebConfigExe.Panel1.Controls.Add(this.groupBoxOnlineExam);
            this.splitContainerSebConfigExe.Panel1.Controls.Add(this.groupBoxInsideOutsideSeb);
            // 
            // splitContainerSebConfigExe.Panel2
            // 
            this.splitContainerSebConfigExe.Panel2.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("splitContainerSebConfigExe.Panel2.BackgroundImage")));
            this.splitContainerSebConfigExe.Panel2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.splitContainerSebConfigExe.Panel2.Controls.Add(this.buttonExitWithoutSaving);
            this.splitContainerSebConfigExe.Panel2.Controls.Add(this.buttonSaveBothConfigFilesAndExit);
            this.splitContainerSebConfigExe.Panel2.Controls.Add(this.groupBoxExitSequence);
            this.splitContainerSebConfigExe.Panel2.Controls.Add(this.groupBoxMsgHookFiles);
            this.splitContainerSebConfigExe.Panel2.Controls.Add(this.groupBoxFunctionKeys);
            this.splitContainerSebConfigExe.Panel2.Controls.Add(this.groupBoxSpecialKeys);
            this.splitContainerSebConfigExe.Size = new System.Drawing.Size(1332, 705);
            this.splitContainerSebConfigExe.SplitterDistance = 644;
            this.splitContainerSebConfigExe.TabIndex = 0;
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
            this.groupBoxSecurityOptions.Location = new System.Drawing.Point(290, 190);
            this.groupBoxSecurityOptions.Name = "groupBoxSecurityOptions";
            this.groupBoxSecurityOptions.Size = new System.Drawing.Size(330, 270);
            this.groupBoxSecurityOptions.TabIndex = 45;
            this.groupBoxSecurityOptions.TabStop = false;
            this.groupBoxSecurityOptions.Text = "Security options";
            // 
            // checkBoxShutdownAfterAutostart
            // 
            this.checkBoxShutdownAfterAutostart.AutoSize = true;
            this.checkBoxShutdownAfterAutostart.Enabled = false;
            this.checkBoxShutdownAfterAutostart.Location = new System.Drawing.Point(10, 240);
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
            this.checkBoxMonitorProcesses.Location = new System.Drawing.Point(10, 210);
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
            this.checkBoxEditRegistry.Location = new System.Drawing.Point(10, 180);
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
            this.checkBoxHookMessages.Location = new System.Drawing.Point(10, 150);
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
            this.checkBoxShowSebApplicationChooser.Location = new System.Drawing.Point(10, 120);
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
            this.checkBoxCreateNewDesktop.Location = new System.Drawing.Point(10, 90);
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
            this.checkBoxAllowVirtualMachine.Location = new System.Drawing.Point(10, 30);
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
            this.checkBoxForceWindowsService.Location = new System.Drawing.Point(10, 60);
            this.checkBoxForceWindowsService.Name = "checkBoxForceWindowsService";
            this.checkBoxForceWindowsService.Size = new System.Drawing.Size(175, 21);
            this.checkBoxForceWindowsService.TabIndex = 44;
            this.checkBoxForceWindowsService.Text = "Force Windows service";
            this.checkBoxForceWindowsService.UseVisualStyleBackColor = true;
            this.checkBoxForceWindowsService.CheckedChanged += new System.EventHandler(this.checkBoxForceWindowsService_CheckedChanged);
            // 
            // groupBoxSebStarterFiles
            // 
            this.groupBoxSebStarterFiles.Controls.Add(this.textBoxCurrentFileSebStarterIni);
            this.groupBoxSebStarterFiles.Controls.Add(this.buttonDefaultSebStarterSettings);
            this.groupBoxSebStarterFiles.Controls.Add(this.textBoxCurrentDireSebStarterIni);
            this.groupBoxSebStarterFiles.Controls.Add(this.labelOpenSebStarterConfigFile);
            this.groupBoxSebStarterFiles.Controls.Add(this.checkBoxWriteSebStarterLogFile);
            this.groupBoxSebStarterFiles.Controls.Add(this.labelSaveSebStarterConfigFile);
            this.groupBoxSebStarterFiles.Controls.Add(this.buttonRestoreSebStarterConfigFile);
            this.groupBoxSebStarterFiles.Location = new System.Drawing.Point(20, 20);
            this.groupBoxSebStarterFiles.Name = "groupBoxSebStarterFiles";
            this.groupBoxSebStarterFiles.Size = new System.Drawing.Size(600, 150);
            this.groupBoxSebStarterFiles.TabIndex = 26;
            this.groupBoxSebStarterFiles.TabStop = false;
            this.groupBoxSebStarterFiles.Text = "SebStarter files";
            // 
            // textBoxCurrentFileSebStarterIni
            // 
            this.textBoxCurrentFileSebStarterIni.Location = new System.Drawing.Point(10, 60);
            this.textBoxCurrentFileSebStarterIni.Name = "textBoxCurrentFileSebStarterIni";
            this.textBoxCurrentFileSebStarterIni.ReadOnly = true;
            this.textBoxCurrentFileSebStarterIni.Size = new System.Drawing.Size(220, 22);
            this.textBoxCurrentFileSebStarterIni.TabIndex = 45;
            // 
            // buttonDefaultSebStarterSettings
            // 
            this.buttonDefaultSebStarterSettings.Location = new System.Drawing.Point(270, 87);
            this.buttonDefaultSebStarterSettings.Name = "buttonDefaultSebStarterSettings";
            this.buttonDefaultSebStarterSettings.Size = new System.Drawing.Size(250, 25);
            this.buttonDefaultSebStarterSettings.TabIndex = 44;
            this.buttonDefaultSebStarterSettings.Text = "Default SebStarter settings";
            this.buttonDefaultSebStarterSettings.UseVisualStyleBackColor = true;
            this.buttonDefaultSebStarterSettings.Click += new System.EventHandler(this.buttonDefaultSebStarterSettings_Click);
            // 
            // textBoxCurrentDireSebStarterIni
            // 
            this.textBoxCurrentDireSebStarterIni.Location = new System.Drawing.Point(10, 30);
            this.textBoxCurrentDireSebStarterIni.Name = "textBoxCurrentDireSebStarterIni";
            this.textBoxCurrentDireSebStarterIni.ReadOnly = true;
            this.textBoxCurrentDireSebStarterIni.Size = new System.Drawing.Size(580, 22);
            this.textBoxCurrentDireSebStarterIni.TabIndex = 43;
            // 
            // labelOpenSebStarterConfigFile
            // 
            this.labelOpenSebStarterConfigFile.AutoSize = true;
            this.labelOpenSebStarterConfigFile.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelOpenSebStarterConfigFile.Location = new System.Drawing.Point(10, 90);
            this.labelOpenSebStarterConfigFile.Name = "labelOpenSebStarterConfigFile";
            this.labelOpenSebStarterConfigFile.Size = new System.Drawing.Size(181, 19);
            this.labelOpenSebStarterConfigFile.TabIndex = 9;
            this.labelOpenSebStarterConfigFile.Text = "Open SebStarter config file";
            this.labelOpenSebStarterConfigFile.Click += new System.EventHandler(this.labelOpenSebStarterConfigFile_Click);
            // 
            // checkBoxWriteSebStarterLogFile
            // 
            this.checkBoxWriteSebStarterLogFile.AutoSize = true;
            this.checkBoxWriteSebStarterLogFile.Location = new System.Drawing.Point(270, 60);
            this.checkBoxWriteSebStarterLogFile.Name = "checkBoxWriteSebStarterLogFile";
            this.checkBoxWriteSebStarterLogFile.Size = new System.Drawing.Size(180, 21);
            this.checkBoxWriteSebStarterLogFile.TabIndex = 42;
            this.checkBoxWriteSebStarterLogFile.Text = "Write SebStarter log file";
            this.checkBoxWriteSebStarterLogFile.UseVisualStyleBackColor = true;
            this.checkBoxWriteSebStarterLogFile.CheckedChanged += new System.EventHandler(this.checkBoxWriteSebStarterLogFile_CheckedChanged);
            // 
            // labelSaveSebStarterConfigFile
            // 
            this.labelSaveSebStarterConfigFile.AutoSize = true;
            this.labelSaveSebStarterConfigFile.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelSaveSebStarterConfigFile.Location = new System.Drawing.Point(13, 120);
            this.labelSaveSebStarterConfigFile.Name = "labelSaveSebStarterConfigFile";
            this.labelSaveSebStarterConfigFile.Size = new System.Drawing.Size(178, 19);
            this.labelSaveSebStarterConfigFile.TabIndex = 10;
            this.labelSaveSebStarterConfigFile.Text = "Save SebStarter config file";
            this.labelSaveSebStarterConfigFile.Click += new System.EventHandler(this.labelSaveSebStarterConfigFile_Click);
            // 
            // buttonRestoreSebStarterConfigFile
            // 
            this.buttonRestoreSebStarterConfigFile.Location = new System.Drawing.Point(270, 117);
            this.buttonRestoreSebStarterConfigFile.Name = "buttonRestoreSebStarterConfigFile";
            this.buttonRestoreSebStarterConfigFile.Size = new System.Drawing.Size(250, 25);
            this.buttonRestoreSebStarterConfigFile.TabIndex = 19;
            this.buttonRestoreSebStarterConfigFile.Text = "Restore SebStarter config file";
            this.buttonRestoreSebStarterConfigFile.UseVisualStyleBackColor = true;
            this.buttonRestoreSebStarterConfigFile.Click += new System.EventHandler(this.buttonRestoreSebStarterConfigFile_Click);
            // 
            // groupBoxOnlineExam
            // 
            this.groupBoxOnlineExam.Controls.Add(this.labelQuitHashCode);
            this.groupBoxOnlineExam.Controls.Add(this.textBoxQuitHashcode);
            this.groupBoxOnlineExam.Controls.Add(this.labelQuitPassword);
            this.groupBoxOnlineExam.Controls.Add(this.textBoxQuitPassword);
            this.groupBoxOnlineExam.Controls.Add(this.textBoxAutostartProcess);
            this.groupBoxOnlineExam.Controls.Add(this.labelSebBrowser);
            this.groupBoxOnlineExam.Controls.Add(this.labelAutostartProcess);
            this.groupBoxOnlineExam.Controls.Add(this.textBoxSebBrowser);
            this.groupBoxOnlineExam.Controls.Add(this.labelExamUrl);
            this.groupBoxOnlineExam.Controls.Add(this.labelPermittedApplications);
            this.groupBoxOnlineExam.Controls.Add(this.textBoxPermittedApplications);
            this.groupBoxOnlineExam.Controls.Add(this.textBoxExamUrl);
            this.groupBoxOnlineExam.Location = new System.Drawing.Point(20, 480);
            this.groupBoxOnlineExam.Name = "groupBoxOnlineExam";
            this.groupBoxOnlineExam.Size = new System.Drawing.Size(600, 210);
            this.groupBoxOnlineExam.TabIndex = 25;
            this.groupBoxOnlineExam.TabStop = false;
            this.groupBoxOnlineExam.Text = "Online exam";
            // 
            // labelQuitHashCode
            // 
            this.labelQuitHashCode.AutoSize = true;
            this.labelQuitHashCode.Location = new System.Drawing.Point(6, 182);
            this.labelQuitHashCode.Name = "labelQuitHashCode";
            this.labelQuitHashCode.Size = new System.Drawing.Size(100, 17);
            this.labelQuitHashCode.TabIndex = 31;
            this.labelQuitHashCode.Text = "Quit hashcode";
            this.labelQuitHashCode.Visible = false;
            // 
            // textBoxQuitHashcode
            // 
            this.textBoxQuitHashcode.Location = new System.Drawing.Point(153, 180);
            this.textBoxQuitHashcode.Name = "textBoxQuitHashcode";
            this.textBoxQuitHashcode.ReadOnly = true;
            this.textBoxQuitHashcode.Size = new System.Drawing.Size(430, 22);
            this.textBoxQuitHashcode.TabIndex = 30;
            this.textBoxQuitHashcode.Visible = false;
            // 
            // labelQuitPassword
            // 
            this.labelQuitPassword.AutoSize = true;
            this.labelQuitPassword.Location = new System.Drawing.Point(6, 152);
            this.labelQuitPassword.Name = "labelQuitPassword";
            this.labelQuitPassword.Size = new System.Drawing.Size(98, 17);
            this.labelQuitPassword.TabIndex = 29;
            this.labelQuitPassword.Text = "Quit password";
            this.labelQuitPassword.Visible = false;
            // 
            // textBoxQuitPassword
            // 
            this.textBoxQuitPassword.Location = new System.Drawing.Point(153, 150);
            this.textBoxQuitPassword.Name = "textBoxQuitPassword";
            this.textBoxQuitPassword.ReadOnly = true;
            this.textBoxQuitPassword.Size = new System.Drawing.Size(430, 22);
            this.textBoxQuitPassword.TabIndex = 28;
            this.textBoxQuitPassword.Visible = false;
            this.textBoxQuitPassword.WordWrap = false;
            this.textBoxQuitPassword.TextChanged += new System.EventHandler(this.textBoxQuitPassword_TextChanged);
            // 
            // textBoxAutostartProcess
            // 
            this.textBoxAutostartProcess.Location = new System.Drawing.Point(153, 60);
            this.textBoxAutostartProcess.Name = "textBoxAutostartProcess";
            this.textBoxAutostartProcess.Size = new System.Drawing.Size(430, 22);
            this.textBoxAutostartProcess.TabIndex = 27;
            this.textBoxAutostartProcess.TextChanged += new System.EventHandler(this.textBoxAutostartProcess_TextChanged);
            // 
            // labelSebBrowser
            // 
            this.labelSebBrowser.AutoSize = true;
            this.labelSebBrowser.Location = new System.Drawing.Point(6, 32);
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
            this.textBoxSebBrowser.Location = new System.Drawing.Point(153, 30);
            this.textBoxSebBrowser.Name = "textBoxSebBrowser";
            this.textBoxSebBrowser.Size = new System.Drawing.Size(430, 22);
            this.textBoxSebBrowser.TabIndex = 24;
            this.textBoxSebBrowser.TextChanged += new System.EventHandler(this.textBoxSebBrowser_TextChanged);
            // 
            // labelExamUrl
            // 
            this.labelExamUrl.AutoSize = true;
            this.labelExamUrl.Location = new System.Drawing.Point(6, 92);
            this.labelExamUrl.Name = "labelExamUrl";
            this.labelExamUrl.Size = new System.Drawing.Size(74, 17);
            this.labelExamUrl.TabIndex = 21;
            this.labelExamUrl.Text = "Exam URL";
            // 
            // labelPermittedApplications
            // 
            this.labelPermittedApplications.AutoSize = true;
            this.labelPermittedApplications.Location = new System.Drawing.Point(6, 122);
            this.labelPermittedApplications.Name = "labelPermittedApplications";
            this.labelPermittedApplications.Size = new System.Drawing.Size(147, 17);
            this.labelPermittedApplications.TabIndex = 22;
            this.labelPermittedApplications.Text = "Permitted applications";
            // 
            // textBoxPermittedApplications
            // 
            this.textBoxPermittedApplications.Location = new System.Drawing.Point(153, 120);
            this.textBoxPermittedApplications.Name = "textBoxPermittedApplications";
            this.textBoxPermittedApplications.Size = new System.Drawing.Size(430, 22);
            this.textBoxPermittedApplications.TabIndex = 23;
            this.textBoxPermittedApplications.TextChanged += new System.EventHandler(this.textBoxPermittedApplications_TextChanged);
            // 
            // textBoxExamUrl
            // 
            this.textBoxExamUrl.Location = new System.Drawing.Point(153, 90);
            this.textBoxExamUrl.Name = "textBoxExamUrl";
            this.textBoxExamUrl.Size = new System.Drawing.Size(430, 22);
            this.textBoxExamUrl.TabIndex = 20;
            this.textBoxExamUrl.TextChanged += new System.EventHandler(this.textBoxExamUrl_TextChanged);
            // 
            // groupBoxInsideOutsideSeb
            // 
            this.groupBoxInsideOutsideSeb.Controls.Add(this.checkBoxInsideSebEnableVmWareClientShade);
            this.groupBoxInsideOutsideSeb.Controls.Add(this.checkBoxInsideSebEnableEaseOfAccess);
            this.groupBoxInsideOutsideSeb.Controls.Add(this.checkBoxInsideSebEnableShutDown);
            this.groupBoxInsideOutsideSeb.Controls.Add(this.checkBoxInsideSebEnableLogOff);
            this.groupBoxInsideOutsideSeb.Controls.Add(this.checkBoxInsideSebEnableStartTaskManager);
            this.groupBoxInsideOutsideSeb.Controls.Add(this.checkBoxInsideSebEnableChangeAPassword);
            this.groupBoxInsideOutsideSeb.Controls.Add(this.checkBoxInsideSebEnableLockThisComputer);
            this.groupBoxInsideOutsideSeb.Controls.Add(this.checkBoxInsideSebEnableSwitchUser);
            this.groupBoxInsideOutsideSeb.Controls.Add(this.checkBoxOutsideSebEnableSwitchUser);
            this.groupBoxInsideOutsideSeb.Controls.Add(this.checkBoxOutsideSebEnableLockThisComputer);
            this.groupBoxInsideOutsideSeb.Controls.Add(this.checkBoxOutsideSebEnableChangeAPassword);
            this.groupBoxInsideOutsideSeb.Controls.Add(this.checkBoxOutsideSebEnableStartTaskManager);
            this.groupBoxInsideOutsideSeb.Controls.Add(this.checkBoxOutsideSebEnableLogOff);
            this.groupBoxInsideOutsideSeb.Controls.Add(this.checkBoxOutsideSebEnableShutDown);
            this.groupBoxInsideOutsideSeb.Controls.Add(this.checkBoxOutsideSebEnableEaseOfAccess);
            this.groupBoxInsideOutsideSeb.Controls.Add(this.checkBoxOutsideSebEnableVmWareClientShade);
            this.groupBoxInsideOutsideSeb.Location = new System.Drawing.Point(20, 190);
            this.groupBoxInsideOutsideSeb.Name = "groupBoxInsideOutsideSeb";
            this.groupBoxInsideOutsideSeb.Size = new System.Drawing.Size(250, 270);
            this.groupBoxInsideOutsideSeb.TabIndex = 24;
            this.groupBoxInsideOutsideSeb.TabStop = false;
            this.groupBoxInsideOutsideSeb.Text = "Inside / outside SEB";
            // 
            // checkBoxInsideSebEnableVmWareClientShade
            // 
            this.checkBoxInsideSebEnableVmWareClientShade.AutoSize = true;
            this.checkBoxInsideSebEnableVmWareClientShade.Location = new System.Drawing.Point(10, 242);
            this.checkBoxInsideSebEnableVmWareClientShade.Name = "checkBoxInsideSebEnableVmWareClientShade";
            this.checkBoxInsideSebEnableVmWareClientShade.Size = new System.Drawing.Size(18, 17);
            this.checkBoxInsideSebEnableVmWareClientShade.TabIndex = 24;
            this.checkBoxInsideSebEnableVmWareClientShade.UseVisualStyleBackColor = true;
            this.checkBoxInsideSebEnableVmWareClientShade.CheckedChanged += new System.EventHandler(this.checkBoxInsideSebEnableVmWareClientShade_CheckedChanged);
            // 
            // checkBoxInsideSebEnableEaseOfAccess
            // 
            this.checkBoxInsideSebEnableEaseOfAccess.AutoSize = true;
            this.checkBoxInsideSebEnableEaseOfAccess.Location = new System.Drawing.Point(10, 212);
            this.checkBoxInsideSebEnableEaseOfAccess.Name = "checkBoxInsideSebEnableEaseOfAccess";
            this.checkBoxInsideSebEnableEaseOfAccess.Size = new System.Drawing.Size(18, 17);
            this.checkBoxInsideSebEnableEaseOfAccess.TabIndex = 23;
            this.checkBoxInsideSebEnableEaseOfAccess.UseVisualStyleBackColor = true;
            this.checkBoxInsideSebEnableEaseOfAccess.CheckedChanged += new System.EventHandler(this.checkBoxInsideSebEnableEaseOfAccess_CheckedChanged);
            // 
            // checkBoxInsideSebEnableShutDown
            // 
            this.checkBoxInsideSebEnableShutDown.AutoSize = true;
            this.checkBoxInsideSebEnableShutDown.Location = new System.Drawing.Point(10, 182);
            this.checkBoxInsideSebEnableShutDown.Name = "checkBoxInsideSebEnableShutDown";
            this.checkBoxInsideSebEnableShutDown.Size = new System.Drawing.Size(18, 17);
            this.checkBoxInsideSebEnableShutDown.TabIndex = 22;
            this.checkBoxInsideSebEnableShutDown.UseVisualStyleBackColor = true;
            this.checkBoxInsideSebEnableShutDown.CheckedChanged += new System.EventHandler(this.checkBoxInsideSebEnableShutDown_CheckedChanged);
            // 
            // checkBoxInsideSebEnableLogOff
            // 
            this.checkBoxInsideSebEnableLogOff.AutoSize = true;
            this.checkBoxInsideSebEnableLogOff.Location = new System.Drawing.Point(10, 152);
            this.checkBoxInsideSebEnableLogOff.Name = "checkBoxInsideSebEnableLogOff";
            this.checkBoxInsideSebEnableLogOff.Size = new System.Drawing.Size(18, 17);
            this.checkBoxInsideSebEnableLogOff.TabIndex = 21;
            this.checkBoxInsideSebEnableLogOff.UseVisualStyleBackColor = true;
            this.checkBoxInsideSebEnableLogOff.CheckedChanged += new System.EventHandler(this.checkBoxInsideSebEnableLogOff_CheckedChanged);
            // 
            // checkBoxInsideSebEnableStartTaskManager
            // 
            this.checkBoxInsideSebEnableStartTaskManager.AutoSize = true;
            this.checkBoxInsideSebEnableStartTaskManager.Location = new System.Drawing.Point(10, 122);
            this.checkBoxInsideSebEnableStartTaskManager.Name = "checkBoxInsideSebEnableStartTaskManager";
            this.checkBoxInsideSebEnableStartTaskManager.Size = new System.Drawing.Size(18, 17);
            this.checkBoxInsideSebEnableStartTaskManager.TabIndex = 20;
            this.checkBoxInsideSebEnableStartTaskManager.UseVisualStyleBackColor = true;
            this.checkBoxInsideSebEnableStartTaskManager.CheckedChanged += new System.EventHandler(this.checkBoxInsideSebEnableStartTaskManager_CheckedChanged);
            // 
            // checkBoxInsideSebEnableChangeAPassword
            // 
            this.checkBoxInsideSebEnableChangeAPassword.AutoSize = true;
            this.checkBoxInsideSebEnableChangeAPassword.Location = new System.Drawing.Point(10, 92);
            this.checkBoxInsideSebEnableChangeAPassword.Name = "checkBoxInsideSebEnableChangeAPassword";
            this.checkBoxInsideSebEnableChangeAPassword.Size = new System.Drawing.Size(18, 17);
            this.checkBoxInsideSebEnableChangeAPassword.TabIndex = 19;
            this.checkBoxInsideSebEnableChangeAPassword.UseVisualStyleBackColor = true;
            this.checkBoxInsideSebEnableChangeAPassword.CheckedChanged += new System.EventHandler(this.checkBoxInsideSebEnableChangeAPassword_CheckedChanged);
            // 
            // checkBoxInsideSebEnableLockThisComputer
            // 
            this.checkBoxInsideSebEnableLockThisComputer.AutoSize = true;
            this.checkBoxInsideSebEnableLockThisComputer.Location = new System.Drawing.Point(10, 62);
            this.checkBoxInsideSebEnableLockThisComputer.Name = "checkBoxInsideSebEnableLockThisComputer";
            this.checkBoxInsideSebEnableLockThisComputer.Size = new System.Drawing.Size(18, 17);
            this.checkBoxInsideSebEnableLockThisComputer.TabIndex = 18;
            this.checkBoxInsideSebEnableLockThisComputer.UseVisualStyleBackColor = true;
            this.checkBoxInsideSebEnableLockThisComputer.CheckedChanged += new System.EventHandler(this.checkBoxInsideSebEnableLockThisComputer_CheckedChanged);
            // 
            // checkBoxInsideSebEnableSwitchUser
            // 
            this.checkBoxInsideSebEnableSwitchUser.AutoSize = true;
            this.checkBoxInsideSebEnableSwitchUser.Location = new System.Drawing.Point(10, 32);
            this.checkBoxInsideSebEnableSwitchUser.Name = "checkBoxInsideSebEnableSwitchUser";
            this.checkBoxInsideSebEnableSwitchUser.Size = new System.Drawing.Size(18, 17);
            this.checkBoxInsideSebEnableSwitchUser.TabIndex = 17;
            this.checkBoxInsideSebEnableSwitchUser.UseVisualStyleBackColor = true;
            this.checkBoxInsideSebEnableSwitchUser.CheckedChanged += new System.EventHandler(this.checkBoxInsideSebEnableSwitchUser_CheckedChanged);
            // 
            // checkBoxOutsideSebEnableSwitchUser
            // 
            this.checkBoxOutsideSebEnableSwitchUser.AutoSize = true;
            this.checkBoxOutsideSebEnableSwitchUser.Location = new System.Drawing.Point(30, 30);
            this.checkBoxOutsideSebEnableSwitchUser.Name = "checkBoxOutsideSebEnableSwitchUser";
            this.checkBoxOutsideSebEnableSwitchUser.Size = new System.Drawing.Size(152, 21);
            this.checkBoxOutsideSebEnableSwitchUser.TabIndex = 0;
            this.checkBoxOutsideSebEnableSwitchUser.Text = "Enable Switch User";
            this.checkBoxOutsideSebEnableSwitchUser.UseVisualStyleBackColor = true;
            this.checkBoxOutsideSebEnableSwitchUser.CheckedChanged += new System.EventHandler(this.checkBoxOutsideSebEnableSwitchUser_CheckedChanged);
            // 
            // checkBoxOutsideSebEnableLockThisComputer
            // 
            this.checkBoxOutsideSebEnableLockThisComputer.AutoSize = true;
            this.checkBoxOutsideSebEnableLockThisComputer.Location = new System.Drawing.Point(30, 60);
            this.checkBoxOutsideSebEnableLockThisComputer.Name = "checkBoxOutsideSebEnableLockThisComputer";
            this.checkBoxOutsideSebEnableLockThisComputer.Size = new System.Drawing.Size(197, 21);
            this.checkBoxOutsideSebEnableLockThisComputer.TabIndex = 1;
            this.checkBoxOutsideSebEnableLockThisComputer.Text = "Enable Lock this computer";
            this.checkBoxOutsideSebEnableLockThisComputer.UseVisualStyleBackColor = true;
            this.checkBoxOutsideSebEnableLockThisComputer.CheckedChanged += new System.EventHandler(this.checkBoxOutsideSebEnableLockThisComputer_CheckedChanged);
            // 
            // checkBoxOutsideSebEnableChangeAPassword
            // 
            this.checkBoxOutsideSebEnableChangeAPassword.AutoSize = true;
            this.checkBoxOutsideSebEnableChangeAPassword.Location = new System.Drawing.Point(30, 90);
            this.checkBoxOutsideSebEnableChangeAPassword.Name = "checkBoxOutsideSebEnableChangeAPassword";
            this.checkBoxOutsideSebEnableChangeAPassword.Size = new System.Drawing.Size(203, 21);
            this.checkBoxOutsideSebEnableChangeAPassword.TabIndex = 3;
            this.checkBoxOutsideSebEnableChangeAPassword.Text = "Enable Change a password";
            this.checkBoxOutsideSebEnableChangeAPassword.UseVisualStyleBackColor = true;
            this.checkBoxOutsideSebEnableChangeAPassword.CheckedChanged += new System.EventHandler(this.checkBoxOutsideSebEnableChangeAPassword_CheckedChanged);
            // 
            // checkBoxOutsideSebEnableStartTaskManager
            // 
            this.checkBoxOutsideSebEnableStartTaskManager.AutoSize = true;
            this.checkBoxOutsideSebEnableStartTaskManager.Location = new System.Drawing.Point(30, 120);
            this.checkBoxOutsideSebEnableStartTaskManager.Name = "checkBoxOutsideSebEnableStartTaskManager";
            this.checkBoxOutsideSebEnableStartTaskManager.Size = new System.Drawing.Size(203, 21);
            this.checkBoxOutsideSebEnableStartTaskManager.TabIndex = 2;
            this.checkBoxOutsideSebEnableStartTaskManager.Text = "Enable Start Task Manager";
            this.checkBoxOutsideSebEnableStartTaskManager.UseVisualStyleBackColor = true;
            this.checkBoxOutsideSebEnableStartTaskManager.CheckedChanged += new System.EventHandler(this.checkBoxOutsideSebEnableStartTaskManager_CheckedChanged);
            // 
            // checkBoxOutsideSebEnableLogOff
            // 
            this.checkBoxOutsideSebEnableLogOff.AutoSize = true;
            this.checkBoxOutsideSebEnableLogOff.Location = new System.Drawing.Point(30, 150);
            this.checkBoxOutsideSebEnableLogOff.Name = "checkBoxOutsideSebEnableLogOff";
            this.checkBoxOutsideSebEnableLogOff.Size = new System.Drawing.Size(122, 21);
            this.checkBoxOutsideSebEnableLogOff.TabIndex = 6;
            this.checkBoxOutsideSebEnableLogOff.Text = "Enable Log off";
            this.checkBoxOutsideSebEnableLogOff.UseVisualStyleBackColor = true;
            this.checkBoxOutsideSebEnableLogOff.CheckedChanged += new System.EventHandler(this.checkBoxOutsideSebEnableLogOff_CheckedChanged);
            // 
            // checkBoxOutsideSebEnableShutDown
            // 
            this.checkBoxOutsideSebEnableShutDown.AutoSize = true;
            this.checkBoxOutsideSebEnableShutDown.Location = new System.Drawing.Point(30, 180);
            this.checkBoxOutsideSebEnableShutDown.Name = "checkBoxOutsideSebEnableShutDown";
            this.checkBoxOutsideSebEnableShutDown.Size = new System.Drawing.Size(144, 21);
            this.checkBoxOutsideSebEnableShutDown.TabIndex = 4;
            this.checkBoxOutsideSebEnableShutDown.Text = "Enable Shut down";
            this.checkBoxOutsideSebEnableShutDown.UseVisualStyleBackColor = true;
            this.checkBoxOutsideSebEnableShutDown.CheckedChanged += new System.EventHandler(this.checkBoxOutsideSebEnableShutDown_CheckedChanged);
            // 
            // checkBoxOutsideSebEnableEaseOfAccess
            // 
            this.checkBoxOutsideSebEnableEaseOfAccess.AutoSize = true;
            this.checkBoxOutsideSebEnableEaseOfAccess.Location = new System.Drawing.Point(30, 210);
            this.checkBoxOutsideSebEnableEaseOfAccess.Name = "checkBoxOutsideSebEnableEaseOfAccess";
            this.checkBoxOutsideSebEnableEaseOfAccess.Size = new System.Drawing.Size(175, 21);
            this.checkBoxOutsideSebEnableEaseOfAccess.TabIndex = 16;
            this.checkBoxOutsideSebEnableEaseOfAccess.Text = "Enable Ease of Access";
            this.checkBoxOutsideSebEnableEaseOfAccess.UseVisualStyleBackColor = true;
            this.checkBoxOutsideSebEnableEaseOfAccess.CheckedChanged += new System.EventHandler(this.checkBoxOutsideSebEnableEaseOfAccess_CheckedChanged);
            // 
            // checkBoxOutsideSebEnableVmWareClientShade
            // 
            this.checkBoxOutsideSebEnableVmWareClientShade.AutoSize = true;
            this.checkBoxOutsideSebEnableVmWareClientShade.Location = new System.Drawing.Point(30, 240);
            this.checkBoxOutsideSebEnableVmWareClientShade.Name = "checkBoxOutsideSebEnableVmWareClientShade";
            this.checkBoxOutsideSebEnableVmWareClientShade.Size = new System.Drawing.Size(212, 21);
            this.checkBoxOutsideSebEnableVmWareClientShade.TabIndex = 7;
            this.checkBoxOutsideSebEnableVmWareClientShade.Text = "Enable VMware Client Shade";
            this.checkBoxOutsideSebEnableVmWareClientShade.UseVisualStyleBackColor = true;
            this.checkBoxOutsideSebEnableVmWareClientShade.CheckedChanged += new System.EventHandler(this.checkBoxOutsideSebEnableVmWareClientShade_CheckedChanged);
            // 
            // buttonExitWithoutSaving
            // 
            this.buttonExitWithoutSaving.Location = new System.Drawing.Point(320, 610);
            this.buttonExitWithoutSaving.Name = "buttonExitWithoutSaving";
            this.buttonExitWithoutSaving.Size = new System.Drawing.Size(170, 60);
            this.buttonExitWithoutSaving.TabIndex = 53;
            this.buttonExitWithoutSaving.Text = "Exit without saving";
            this.buttonExitWithoutSaving.UseVisualStyleBackColor = true;
            this.buttonExitWithoutSaving.Click += new System.EventHandler(this.buttonExitWithoutSaving_Click);
            // 
            // buttonSaveBothConfigFilesAndExit
            // 
            this.buttonSaveBothConfigFilesAndExit.Location = new System.Drawing.Point(20, 610);
            this.buttonSaveBothConfigFilesAndExit.Name = "buttonSaveBothConfigFilesAndExit";
            this.buttonSaveBothConfigFilesAndExit.Size = new System.Drawing.Size(170, 60);
            this.buttonSaveBothConfigFilesAndExit.TabIndex = 52;
            this.buttonSaveBothConfigFilesAndExit.Text = "Save both config files and exit";
            this.buttonSaveBothConfigFilesAndExit.UseVisualStyleBackColor = true;
            this.buttonSaveBothConfigFilesAndExit.Click += new System.EventHandler(this.buttonSaveBothConfigFilesAndExit_Click);
            // 
            // groupBoxExitSequence
            // 
            this.groupBoxExitSequence.Controls.Add(this.listBoxExitKey1);
            this.groupBoxExitSequence.Controls.Add(this.listBoxExitKey3);
            this.groupBoxExitSequence.Controls.Add(this.listBoxExitKey2);
            this.groupBoxExitSequence.Location = new System.Drawing.Point(340, 190);
            this.groupBoxExitSequence.Name = "groupBoxExitSequence";
            this.groupBoxExitSequence.Size = new System.Drawing.Size(160, 240);
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
            this.listBoxExitKey1.Location = new System.Drawing.Point(10, 30);
            this.listBoxExitKey1.Name = "listBoxExitKey1";
            this.listBoxExitKey1.Size = new System.Drawing.Size(40, 196);
            this.listBoxExitKey1.TabIndex = 47;
            this.listBoxExitKey1.SelectedIndexChanged += new System.EventHandler(this.listBoxExitKey1_SelectedIndexChanged);
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
            this.listBoxExitKey3.Location = new System.Drawing.Point(110, 30);
            this.listBoxExitKey3.Name = "listBoxExitKey3";
            this.listBoxExitKey3.Size = new System.Drawing.Size(40, 196);
            this.listBoxExitKey3.TabIndex = 50;
            this.listBoxExitKey3.SelectedIndexChanged += new System.EventHandler(this.listBoxExitKey3_SelectedIndexChanged);
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
            this.listBoxExitKey2.Location = new System.Drawing.Point(60, 30);
            this.listBoxExitKey2.Name = "listBoxExitKey2";
            this.listBoxExitKey2.Size = new System.Drawing.Size(40, 196);
            this.listBoxExitKey2.TabIndex = 49;
            this.listBoxExitKey2.SelectedIndexChanged += new System.EventHandler(this.listBoxExitKey2_SelectedIndexChanged);
            // 
            // groupBoxMsgHookFiles
            // 
            this.groupBoxMsgHookFiles.Controls.Add(this.textBoxCurrentFileMsgHookIni);
            this.groupBoxMsgHookFiles.Controls.Add(this.buttonDefaultMsgHookSettings);
            this.groupBoxMsgHookFiles.Controls.Add(this.textBoxCurrentDireMsgHookIni);
            this.groupBoxMsgHookFiles.Controls.Add(this.labelOpenMsgHookConfigFile);
            this.groupBoxMsgHookFiles.Controls.Add(this.labelSaveMsgHookConfigFile);
            this.groupBoxMsgHookFiles.Controls.Add(this.checkBoxWriteMsgHookLogFile);
            this.groupBoxMsgHookFiles.Controls.Add(this.buttonRestoreMsgHookConfigFile);
            this.groupBoxMsgHookFiles.Location = new System.Drawing.Point(20, 20);
            this.groupBoxMsgHookFiles.Name = "groupBoxMsgHookFiles";
            this.groupBoxMsgHookFiles.Size = new System.Drawing.Size(600, 150);
            this.groupBoxMsgHookFiles.TabIndex = 40;
            this.groupBoxMsgHookFiles.TabStop = false;
            this.groupBoxMsgHookFiles.Text = "MsgHook files";
            // 
            // textBoxCurrentFileMsgHookIni
            // 
            this.textBoxCurrentFileMsgHookIni.Location = new System.Drawing.Point(10, 60);
            this.textBoxCurrentFileMsgHookIni.Name = "textBoxCurrentFileMsgHookIni";
            this.textBoxCurrentFileMsgHookIni.ReadOnly = true;
            this.textBoxCurrentFileMsgHookIni.Size = new System.Drawing.Size(220, 22);
            this.textBoxCurrentFileMsgHookIni.TabIndex = 48;
            // 
            // buttonDefaultMsgHookSettings
            // 
            this.buttonDefaultMsgHookSettings.Location = new System.Drawing.Point(270, 87);
            this.buttonDefaultMsgHookSettings.Name = "buttonDefaultMsgHookSettings";
            this.buttonDefaultMsgHookSettings.Size = new System.Drawing.Size(220, 25);
            this.buttonDefaultMsgHookSettings.TabIndex = 47;
            this.buttonDefaultMsgHookSettings.Text = "Default MsgHook settings";
            this.buttonDefaultMsgHookSettings.UseVisualStyleBackColor = true;
            this.buttonDefaultMsgHookSettings.Click += new System.EventHandler(this.buttonDefaultMsgHookSettings_Click);
            // 
            // textBoxCurrentDireMsgHookIni
            // 
            this.textBoxCurrentDireMsgHookIni.Location = new System.Drawing.Point(10, 30);
            this.textBoxCurrentDireMsgHookIni.Name = "textBoxCurrentDireMsgHookIni";
            this.textBoxCurrentDireMsgHookIni.ReadOnly = true;
            this.textBoxCurrentDireMsgHookIni.Size = new System.Drawing.Size(580, 22);
            this.textBoxCurrentDireMsgHookIni.TabIndex = 46;
            // 
            // labelOpenMsgHookConfigFile
            // 
            this.labelOpenMsgHookConfigFile.AutoSize = true;
            this.labelOpenMsgHookConfigFile.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelOpenMsgHookConfigFile.Location = new System.Drawing.Point(10, 90);
            this.labelOpenMsgHookConfigFile.Name = "labelOpenMsgHookConfigFile";
            this.labelOpenMsgHookConfigFile.Size = new System.Drawing.Size(172, 19);
            this.labelOpenMsgHookConfigFile.TabIndex = 17;
            this.labelOpenMsgHookConfigFile.Text = "Open MsgHook config file";
            this.labelOpenMsgHookConfigFile.Click += new System.EventHandler(this.labelOpenMsgHookConfigFile_Click);
            // 
            // labelSaveMsgHookConfigFile
            // 
            this.labelSaveMsgHookConfigFile.AutoSize = true;
            this.labelSaveMsgHookConfigFile.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelSaveMsgHookConfigFile.Location = new System.Drawing.Point(13, 120);
            this.labelSaveMsgHookConfigFile.Name = "labelSaveMsgHookConfigFile";
            this.labelSaveMsgHookConfigFile.Size = new System.Drawing.Size(169, 19);
            this.labelSaveMsgHookConfigFile.TabIndex = 18;
            this.labelSaveMsgHookConfigFile.Text = "Save MsgHook config file";
            this.labelSaveMsgHookConfigFile.Click += new System.EventHandler(this.labelSaveMsgHookConfigFile_Click);
            // 
            // checkBoxWriteMsgHookLogFile
            // 
            this.checkBoxWriteMsgHookLogFile.AutoSize = true;
            this.checkBoxWriteMsgHookLogFile.Location = new System.Drawing.Point(270, 60);
            this.checkBoxWriteMsgHookLogFile.Name = "checkBoxWriteMsgHookLogFile";
            this.checkBoxWriteMsgHookLogFile.Size = new System.Drawing.Size(171, 21);
            this.checkBoxWriteMsgHookLogFile.TabIndex = 41;
            this.checkBoxWriteMsgHookLogFile.Text = "Write MsgHook log file";
            this.checkBoxWriteMsgHookLogFile.UseVisualStyleBackColor = true;
            this.checkBoxWriteMsgHookLogFile.CheckedChanged += new System.EventHandler(this.checkBoxWriteMsgHookLogFile_CheckedChanged);
            // 
            // buttonRestoreMsgHookConfigFile
            // 
            this.buttonRestoreMsgHookConfigFile.Location = new System.Drawing.Point(270, 117);
            this.buttonRestoreMsgHookConfigFile.Name = "buttonRestoreMsgHookConfigFile";
            this.buttonRestoreMsgHookConfigFile.Size = new System.Drawing.Size(220, 25);
            this.buttonRestoreMsgHookConfigFile.TabIndex = 45;
            this.buttonRestoreMsgHookConfigFile.Text = "Restore MsgHook config file";
            this.buttonRestoreMsgHookConfigFile.UseVisualStyleBackColor = true;
            this.buttonRestoreMsgHookConfigFile.Click += new System.EventHandler(this.buttonRestoreMsgHookConfigFile_Click);
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
            this.groupBoxFunctionKeys.Location = new System.Drawing.Point(210, 190);
            this.groupBoxFunctionKeys.Name = "groupBoxFunctionKeys";
            this.groupBoxFunctionKeys.Size = new System.Drawing.Size(110, 390);
            this.groupBoxFunctionKeys.TabIndex = 39;
            this.groupBoxFunctionKeys.TabStop = false;
            this.groupBoxFunctionKeys.Text = "Function keys";
            // 
            // checkBoxEnableF1
            // 
            this.checkBoxEnableF1.AutoSize = true;
            this.checkBoxEnableF1.Location = new System.Drawing.Point(10, 30);
            this.checkBoxEnableF1.Name = "checkBoxEnableF1";
            this.checkBoxEnableF1.Size = new System.Drawing.Size(94, 21);
            this.checkBoxEnableF1.TabIndex = 25;
            this.checkBoxEnableF1.Text = "Enable F1";
            this.checkBoxEnableF1.UseVisualStyleBackColor = true;
            this.checkBoxEnableF1.CheckedChanged += new System.EventHandler(this.checkBoxEnableF1_CheckedChanged);
            // 
            // checkBoxEnableF2
            // 
            this.checkBoxEnableF2.AutoSize = true;
            this.checkBoxEnableF2.Location = new System.Drawing.Point(10, 60);
            this.checkBoxEnableF2.Name = "checkBoxEnableF2";
            this.checkBoxEnableF2.Size = new System.Drawing.Size(94, 21);
            this.checkBoxEnableF2.TabIndex = 26;
            this.checkBoxEnableF2.Text = "Enable F2";
            this.checkBoxEnableF2.UseVisualStyleBackColor = true;
            this.checkBoxEnableF2.CheckedChanged += new System.EventHandler(this.checkBoxEnableF2_CheckedChanged);
            // 
            // checkBoxEnableF12
            // 
            this.checkBoxEnableF12.AutoSize = true;
            this.checkBoxEnableF12.Location = new System.Drawing.Point(10, 360);
            this.checkBoxEnableF12.Name = "checkBoxEnableF12";
            this.checkBoxEnableF12.Size = new System.Drawing.Size(102, 21);
            this.checkBoxEnableF12.TabIndex = 37;
            this.checkBoxEnableF12.Text = "Enable F12";
            this.checkBoxEnableF12.UseVisualStyleBackColor = true;
            this.checkBoxEnableF12.CheckedChanged += new System.EventHandler(this.checkBoxEnableF12_CheckedChanged);
            // 
            // checkBoxEnableF3
            // 
            this.checkBoxEnableF3.AutoSize = true;
            this.checkBoxEnableF3.Location = new System.Drawing.Point(10, 90);
            this.checkBoxEnableF3.Name = "checkBoxEnableF3";
            this.checkBoxEnableF3.Size = new System.Drawing.Size(94, 21);
            this.checkBoxEnableF3.TabIndex = 27;
            this.checkBoxEnableF3.Text = "Enable F3";
            this.checkBoxEnableF3.UseVisualStyleBackColor = true;
            this.checkBoxEnableF3.CheckedChanged += new System.EventHandler(this.checkBoxEnableF3_CheckedChanged);
            // 
            // checkBoxEnableF11
            // 
            this.checkBoxEnableF11.AutoSize = true;
            this.checkBoxEnableF11.Location = new System.Drawing.Point(10, 330);
            this.checkBoxEnableF11.Name = "checkBoxEnableF11";
            this.checkBoxEnableF11.Size = new System.Drawing.Size(102, 21);
            this.checkBoxEnableF11.TabIndex = 36;
            this.checkBoxEnableF11.Text = "Enable F11";
            this.checkBoxEnableF11.UseVisualStyleBackColor = true;
            this.checkBoxEnableF11.CheckedChanged += new System.EventHandler(this.checkBoxEnableF11_CheckedChanged);
            // 
            // checkBoxEnableF4
            // 
            this.checkBoxEnableF4.AutoSize = true;
            this.checkBoxEnableF4.Location = new System.Drawing.Point(10, 120);
            this.checkBoxEnableF4.Name = "checkBoxEnableF4";
            this.checkBoxEnableF4.Size = new System.Drawing.Size(94, 21);
            this.checkBoxEnableF4.TabIndex = 28;
            this.checkBoxEnableF4.Text = "Enable F4";
            this.checkBoxEnableF4.UseVisualStyleBackColor = true;
            this.checkBoxEnableF4.CheckedChanged += new System.EventHandler(this.checkBoxEnableF4_CheckedChanged);
            // 
            // checkBoxEnableF5
            // 
            this.checkBoxEnableF5.AutoSize = true;
            this.checkBoxEnableF5.Location = new System.Drawing.Point(10, 150);
            this.checkBoxEnableF5.Name = "checkBoxEnableF5";
            this.checkBoxEnableF5.Size = new System.Drawing.Size(94, 21);
            this.checkBoxEnableF5.TabIndex = 29;
            this.checkBoxEnableF5.Text = "Enable F5";
            this.checkBoxEnableF5.UseVisualStyleBackColor = true;
            this.checkBoxEnableF5.CheckedChanged += new System.EventHandler(this.checkBoxEnableF5_CheckedChanged);
            // 
            // checkBoxEnableF10
            // 
            this.checkBoxEnableF10.AutoSize = true;
            this.checkBoxEnableF10.Location = new System.Drawing.Point(10, 300);
            this.checkBoxEnableF10.Name = "checkBoxEnableF10";
            this.checkBoxEnableF10.Size = new System.Drawing.Size(102, 21);
            this.checkBoxEnableF10.TabIndex = 34;
            this.checkBoxEnableF10.Text = "Enable F10";
            this.checkBoxEnableF10.UseVisualStyleBackColor = true;
            this.checkBoxEnableF10.CheckedChanged += new System.EventHandler(this.checkBoxEnableF10_CheckedChanged);
            // 
            // checkBoxEnableF6
            // 
            this.checkBoxEnableF6.AutoSize = true;
            this.checkBoxEnableF6.Location = new System.Drawing.Point(10, 180);
            this.checkBoxEnableF6.Name = "checkBoxEnableF6";
            this.checkBoxEnableF6.Size = new System.Drawing.Size(94, 21);
            this.checkBoxEnableF6.TabIndex = 30;
            this.checkBoxEnableF6.Text = "Enable F6";
            this.checkBoxEnableF6.UseVisualStyleBackColor = true;
            this.checkBoxEnableF6.CheckedChanged += new System.EventHandler(this.checkBoxEnableF6_CheckedChanged);
            // 
            // checkBoxEnableF9
            // 
            this.checkBoxEnableF9.AutoSize = true;
            this.checkBoxEnableF9.Location = new System.Drawing.Point(10, 270);
            this.checkBoxEnableF9.Name = "checkBoxEnableF9";
            this.checkBoxEnableF9.Size = new System.Drawing.Size(94, 21);
            this.checkBoxEnableF9.TabIndex = 33;
            this.checkBoxEnableF9.Text = "Enable F9";
            this.checkBoxEnableF9.UseVisualStyleBackColor = true;
            this.checkBoxEnableF9.CheckedChanged += new System.EventHandler(this.checkBoxEnableF9_CheckedChanged);
            // 
            // checkBoxEnableF7
            // 
            this.checkBoxEnableF7.AutoSize = true;
            this.checkBoxEnableF7.Location = new System.Drawing.Point(10, 210);
            this.checkBoxEnableF7.Name = "checkBoxEnableF7";
            this.checkBoxEnableF7.Size = new System.Drawing.Size(94, 21);
            this.checkBoxEnableF7.TabIndex = 31;
            this.checkBoxEnableF7.Text = "Enable F7";
            this.checkBoxEnableF7.UseVisualStyleBackColor = true;
            this.checkBoxEnableF7.CheckedChanged += new System.EventHandler(this.checkBoxEnableF7_CheckedChanged);
            // 
            // checkBoxEnableF8
            // 
            this.checkBoxEnableF8.AutoSize = true;
            this.checkBoxEnableF8.Location = new System.Drawing.Point(10, 240);
            this.checkBoxEnableF8.Name = "checkBoxEnableF8";
            this.checkBoxEnableF8.Size = new System.Drawing.Size(94, 21);
            this.checkBoxEnableF8.TabIndex = 32;
            this.checkBoxEnableF8.Text = "Enable F8";
            this.checkBoxEnableF8.UseVisualStyleBackColor = true;
            this.checkBoxEnableF8.CheckedChanged += new System.EventHandler(this.checkBoxEnableF8_CheckedChanged);
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
            this.groupBoxSpecialKeys.Location = new System.Drawing.Point(20, 190);
            this.groupBoxSpecialKeys.Name = "groupBoxSpecialKeys";
            this.groupBoxSpecialKeys.Size = new System.Drawing.Size(170, 240);
            this.groupBoxSpecialKeys.TabIndex = 38;
            this.groupBoxSpecialKeys.TabStop = false;
            this.groupBoxSpecialKeys.Text = "Special keys";
            // 
            // checkBoxEnableEsc
            // 
            this.checkBoxEnableEsc.AutoSize = true;
            this.checkBoxEnableEsc.Location = new System.Drawing.Point(10, 30);
            this.checkBoxEnableEsc.Name = "checkBoxEnableEsc";
            this.checkBoxEnableEsc.Size = new System.Drawing.Size(101, 21);
            this.checkBoxEnableEsc.TabIndex = 41;
            this.checkBoxEnableEsc.Text = "Enable Esc";
            this.checkBoxEnableEsc.UseVisualStyleBackColor = true;
            this.checkBoxEnableEsc.CheckedChanged += new System.EventHandler(this.checkBoxEnableEsc_CheckedChanged);
            // 
            // checkBoxEnableCtrlEsc
            // 
            this.checkBoxEnableCtrlEsc.AutoSize = true;
            this.checkBoxEnableCtrlEsc.Location = new System.Drawing.Point(10, 60);
            this.checkBoxEnableCtrlEsc.Name = "checkBoxEnableCtrlEsc";
            this.checkBoxEnableCtrlEsc.Size = new System.Drawing.Size(127, 21);
            this.checkBoxEnableCtrlEsc.TabIndex = 19;
            this.checkBoxEnableCtrlEsc.Text = "Enable Ctrl-Esc";
            this.checkBoxEnableCtrlEsc.UseVisualStyleBackColor = true;
            this.checkBoxEnableCtrlEsc.CheckedChanged += new System.EventHandler(this.checkBoxEnableCtrlEsc_CheckedChanged);
            // 
            // checkBoxEnableAltEsc
            // 
            this.checkBoxEnableAltEsc.AutoSize = true;
            this.checkBoxEnableAltEsc.Location = new System.Drawing.Point(10, 90);
            this.checkBoxEnableAltEsc.Name = "checkBoxEnableAltEsc";
            this.checkBoxEnableAltEsc.Size = new System.Drawing.Size(122, 21);
            this.checkBoxEnableAltEsc.TabIndex = 20;
            this.checkBoxEnableAltEsc.Text = "Enable Alt-Esc";
            this.checkBoxEnableAltEsc.UseVisualStyleBackColor = true;
            this.checkBoxEnableAltEsc.CheckedChanged += new System.EventHandler(this.checkBoxEnableAltEsc_CheckedChanged);
            // 
            // checkBoxEnableAltTab
            // 
            this.checkBoxEnableAltTab.AutoSize = true;
            this.checkBoxEnableAltTab.Location = new System.Drawing.Point(10, 120);
            this.checkBoxEnableAltTab.Name = "checkBoxEnableAltTab";
            this.checkBoxEnableAltTab.Size = new System.Drawing.Size(124, 21);
            this.checkBoxEnableAltTab.TabIndex = 21;
            this.checkBoxEnableAltTab.Text = "Enable Alt-Tab";
            this.checkBoxEnableAltTab.UseVisualStyleBackColor = true;
            this.checkBoxEnableAltTab.CheckedChanged += new System.EventHandler(this.checkBoxEnableAltTab_CheckedChanged);
            // 
            // checkBoxEnableAltF4
            // 
            this.checkBoxEnableAltF4.AutoSize = true;
            this.checkBoxEnableAltF4.Location = new System.Drawing.Point(10, 150);
            this.checkBoxEnableAltF4.Name = "checkBoxEnableAltF4";
            this.checkBoxEnableAltF4.Size = new System.Drawing.Size(115, 21);
            this.checkBoxEnableAltF4.TabIndex = 22;
            this.checkBoxEnableAltF4.Text = "Enable Alt-F4";
            this.checkBoxEnableAltF4.UseVisualStyleBackColor = true;
            this.checkBoxEnableAltF4.CheckedChanged += new System.EventHandler(this.checkBoxEnableAltF4_CheckedChanged);
            // 
            // checkBoxEnableStartMenu
            // 
            this.checkBoxEnableStartMenu.AutoSize = true;
            this.checkBoxEnableStartMenu.Location = new System.Drawing.Point(10, 180);
            this.checkBoxEnableStartMenu.Name = "checkBoxEnableStartMenu";
            this.checkBoxEnableStartMenu.Size = new System.Drawing.Size(147, 21);
            this.checkBoxEnableStartMenu.TabIndex = 23;
            this.checkBoxEnableStartMenu.Text = "Enable Start Menu";
            this.checkBoxEnableStartMenu.UseVisualStyleBackColor = true;
            this.checkBoxEnableStartMenu.CheckedChanged += new System.EventHandler(this.checkBoxEnableStartMenu_CheckedChanged);
            // 
            // checkBoxEnableRightMouse
            // 
            this.checkBoxEnableRightMouse.AutoSize = true;
            this.checkBoxEnableRightMouse.Location = new System.Drawing.Point(10, 210);
            this.checkBoxEnableRightMouse.Name = "checkBoxEnableRightMouse";
            this.checkBoxEnableRightMouse.Size = new System.Drawing.Size(157, 21);
            this.checkBoxEnableRightMouse.TabIndex = 24;
            this.checkBoxEnableRightMouse.Text = "Enable Right Mouse";
            this.checkBoxEnableRightMouse.UseVisualStyleBackColor = true;
            this.checkBoxEnableRightMouse.CheckedChanged += new System.EventHandler(this.checkBoxEnableRightMouse_CheckedChanged);
            // 
            // openFileDialogSebStarterIni
            // 
            this.openFileDialogSebStarterIni.DefaultExt = "ini";
            this.openFileDialogSebStarterIni.FileName = "SebStarter.ini";
            this.openFileDialogSebStarterIni.Filter = "Initialisierungsdatei (*.ini) | *.ini";
            this.openFileDialogSebStarterIni.Title = "Open file SebStarter.ini";
            // 
            // saveFileDialogSebStarterIni
            // 
            this.saveFileDialogSebStarterIni.DefaultExt = "ini";
            this.saveFileDialogSebStarterIni.FileName = "SebStarter.ini";
            this.saveFileDialogSebStarterIni.Filter = "Initialisierungsdatei (*.ini) | *.ini";
            this.saveFileDialogSebStarterIni.Title = "Save file SebStarter.ini";
            // 
            // openFileDialogMsgHookIni
            // 
            this.openFileDialogMsgHookIni.DefaultExt = "ini";
            this.openFileDialogMsgHookIni.FileName = "MsgHook.ini";
            this.openFileDialogMsgHookIni.Filter = "Initialisierungsdatei (*.ini) | *.ini";
            this.openFileDialogMsgHookIni.Title = "Open file MsgHook.ini";
            // 
            // saveFileDialogMsgHookIni
            // 
            this.saveFileDialogMsgHookIni.DefaultExt = "ini";
            this.saveFileDialogMsgHookIni.FileName = "MsgHook.ini";
            this.saveFileDialogMsgHookIni.Filter = "Initialisierungsdatei (*.ini) | *.ini";
            this.saveFileDialogMsgHookIni.Title = "Save file MsgHook.ini";
            // 
            // SebWindowsConfigForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1332, 705);
            this.Controls.Add(this.splitContainerSebConfigExe);
            this.Name = "SebWindowsConfigForm";
            this.Text = "SEB Windows Configuration Editor";
            this.splitContainerSebConfigExe.Panel1.ResumeLayout(false);
            this.splitContainerSebConfigExe.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerSebConfigExe)).EndInit();
            this.splitContainerSebConfigExe.ResumeLayout(false);
            this.groupBoxSecurityOptions.ResumeLayout(false);
            this.groupBoxSecurityOptions.PerformLayout();
            this.groupBoxSebStarterFiles.ResumeLayout(false);
            this.groupBoxSebStarterFiles.PerformLayout();
            this.groupBoxOnlineExam.ResumeLayout(false);
            this.groupBoxOnlineExam.PerformLayout();
            this.groupBoxInsideOutsideSeb.ResumeLayout(false);
            this.groupBoxInsideOutsideSeb.PerformLayout();
            this.groupBoxExitSequence.ResumeLayout(false);
            this.groupBoxMsgHookFiles.ResumeLayout(false);
            this.groupBoxMsgHookFiles.PerformLayout();
            this.groupBoxFunctionKeys.ResumeLayout(false);
            this.groupBoxFunctionKeys.PerformLayout();
            this.groupBoxSpecialKeys.ResumeLayout(false);
            this.groupBoxSpecialKeys.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainerSebConfigExe;

        private System.Windows.Forms.GroupBox groupBoxSebStarterFiles;
        private System.Windows.Forms.GroupBox groupBoxMsgHookFiles;

        private System.Windows.Forms.GroupBox groupBoxInsideOutsideSeb;
        private System.Windows.Forms.GroupBox groupBoxSecurityOptions;
        private System.Windows.Forms.GroupBox groupBoxOnlineExam;

        private System.Windows.Forms.GroupBox groupBoxFunctionKeys;
        private System.Windows.Forms.GroupBox groupBoxSpecialKeys;
        private System.Windows.Forms.GroupBox groupBoxExitSequence;

        private System.Windows.Forms.OpenFileDialog openFileDialogSebStarterIni;
        private System.Windows.Forms.SaveFileDialog saveFileDialogSebStarterIni;
        private System.Windows.Forms.OpenFileDialog openFileDialogMsgHookIni;
        private System.Windows.Forms.SaveFileDialog saveFileDialogMsgHookIni;

        private System.Windows.Forms.Label labelOpenSebStarterConfigFile;
        private System.Windows.Forms.Label labelSaveSebStarterConfigFile;
        private System.Windows.Forms.Label labelOpenMsgHookConfigFile;
        private System.Windows.Forms.Label labelSaveMsgHookConfigFile;

        private System.Windows.Forms.Button buttonRestoreSebStarterConfigFile;
        private System.Windows.Forms.Button buttonRestoreMsgHookConfigFile;

        private System.Windows.Forms.CheckBox checkBoxWriteMsgHookLogFile;
        private System.Windows.Forms.CheckBox checkBoxWriteSebStarterLogFile;

        private System.Windows.Forms.CheckBox checkBoxOutsideSebEnableSwitchUser;
        private System.Windows.Forms.CheckBox checkBoxOutsideSebEnableLogOff;
        private System.Windows.Forms.CheckBox checkBoxOutsideSebEnableShutDown;
        private System.Windows.Forms.CheckBox checkBoxOutsideSebEnableChangeAPassword;
        private System.Windows.Forms.CheckBox checkBoxOutsideSebEnableStartTaskManager;
        private System.Windows.Forms.CheckBox checkBoxOutsideSebEnableLockThisComputer;
        private System.Windows.Forms.CheckBox checkBoxOutsideSebEnableEaseOfAccess;
        private System.Windows.Forms.CheckBox checkBoxOutsideSebEnableVmWareClientShade;

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
        private System.Windows.Forms.Label labelQuitPassword;
        private System.Windows.Forms.TextBox textBoxQuitPassword;
        private System.Windows.Forms.Label labelQuitHashCode;
        private System.Windows.Forms.TextBox textBoxQuitHashcode;
        private System.Windows.Forms.Button buttonSaveBothConfigFilesAndExit;
        private System.Windows.Forms.CheckBox checkBoxInsideSebEnableSwitchUser;
        private System.Windows.Forms.CheckBox checkBoxInsideSebEnableLockThisComputer;
        private System.Windows.Forms.CheckBox checkBoxInsideSebEnableChangeAPassword;
        private System.Windows.Forms.CheckBox checkBoxInsideSebEnableStartTaskManager;
        private System.Windows.Forms.CheckBox checkBoxInsideSebEnableLogOff;
        private System.Windows.Forms.CheckBox checkBoxInsideSebEnableShutDown;
        private System.Windows.Forms.CheckBox checkBoxInsideSebEnableEaseOfAccess;
        private System.Windows.Forms.CheckBox checkBoxInsideSebEnableVmWareClientShade;
        private System.Windows.Forms.TextBox textBoxCurrentDireSebStarterIni;
        private System.Windows.Forms.TextBox textBoxCurrentDireMsgHookIni;
        private System.Windows.Forms.Button buttonExitWithoutSaving;
        private System.Windows.Forms.Button buttonDefaultSebStarterSettings;
        private System.Windows.Forms.Button buttonDefaultMsgHookSettings;
        private System.Windows.Forms.TextBox textBoxCurrentFileSebStarterIni;
        private System.Windows.Forms.TextBox textBoxCurrentFileMsgHookIni;

    }
}

