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
            this.buttonRestoreSettingsOfSebStarterIni = new System.Windows.Forms.Button();
            this.checkBoxEnableEaseOfAccess = new System.Windows.Forms.CheckBox();
            this.labelRightSide = new System.Windows.Forms.Label();
            this.labelLeftSide = new System.Windows.Forms.Label();
            this.labelCurrentLine = new System.Windows.Forms.Label();
            this.labelSebStarterIniPath = new System.Windows.Forms.Label();
            this.labelSaveSebStarterIniFile = new System.Windows.Forms.Label();
            this.labelOpenSebStarterIniFile = new System.Windows.Forms.Label();
            this.labelBrowseSebStarterIniFolder = new System.Windows.Forms.Label();
            this.checkBoxEnableVmWareClientShade = new System.Windows.Forms.CheckBox();
            this.checkBoxEnableLogOff = new System.Windows.Forms.CheckBox();
            this.labelSebStarterOptions = new System.Windows.Forms.Label();
            this.checkBoxEnableShutDown = new System.Windows.Forms.CheckBox();
            this.checkBoxEnableChangeAPassword = new System.Windows.Forms.CheckBox();
            this.checkBoxEnableStartTaskManager = new System.Windows.Forms.CheckBox();
            this.checkBoxEnableLockThisComputer = new System.Windows.Forms.CheckBox();
            this.checkBoxEnableSwitchUser = new System.Windows.Forms.CheckBox();
            this.labelMsgHookOptions = new System.Windows.Forms.Label();
            this.labelSaveMsgHookIniFile = new System.Windows.Forms.Label();
            this.labelOpenMsgHookIniFile = new System.Windows.Forms.Label();
            this.labelMsgHookIniPath = new System.Windows.Forms.Label();
            this.folderBrowserDialogBrowseIniFiles = new System.Windows.Forms.FolderBrowserDialog();
            this.openFileDialogSebStarterIni = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialogSebStarterIni = new System.Windows.Forms.SaveFileDialog();
            this.textBoxExamUrl = new System.Windows.Forms.TextBox();
            this.labelExamUrl = new System.Windows.Forms.Label();
            this.labelPermittedApplications = new System.Windows.Forms.Label();
            this.textBoxPermittedApplications = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
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
            this.splitContainer1.Panel1.Controls.Add(this.textBoxPermittedApplications);
            this.splitContainer1.Panel1.Controls.Add(this.labelPermittedApplications);
            this.splitContainer1.Panel1.Controls.Add(this.labelExamUrl);
            this.splitContainer1.Panel1.Controls.Add(this.textBoxExamUrl);
            this.splitContainer1.Panel1.Controls.Add(this.buttonRestoreSettingsOfSebStarterIni);
            this.splitContainer1.Panel1.Controls.Add(this.checkBoxEnableEaseOfAccess);
            this.splitContainer1.Panel1.Controls.Add(this.labelRightSide);
            this.splitContainer1.Panel1.Controls.Add(this.labelLeftSide);
            this.splitContainer1.Panel1.Controls.Add(this.labelCurrentLine);
            this.splitContainer1.Panel1.Controls.Add(this.labelSebStarterIniPath);
            this.splitContainer1.Panel1.Controls.Add(this.labelSaveSebStarterIniFile);
            this.splitContainer1.Panel1.Controls.Add(this.labelOpenSebStarterIniFile);
            this.splitContainer1.Panel1.Controls.Add(this.labelBrowseSebStarterIniFolder);
            this.splitContainer1.Panel1.Controls.Add(this.checkBoxEnableVmWareClientShade);
            this.splitContainer1.Panel1.Controls.Add(this.checkBoxEnableLogOff);
            this.splitContainer1.Panel1.Controls.Add(this.labelSebStarterOptions);
            this.splitContainer1.Panel1.Controls.Add(this.checkBoxEnableShutDown);
            this.splitContainer1.Panel1.Controls.Add(this.checkBoxEnableChangeAPassword);
            this.splitContainer1.Panel1.Controls.Add(this.checkBoxEnableStartTaskManager);
            this.splitContainer1.Panel1.Controls.Add(this.checkBoxEnableLockThisComputer);
            this.splitContainer1.Panel1.Controls.Add(this.checkBoxEnableSwitchUser);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.labelMsgHookOptions);
            this.splitContainer1.Panel2.Controls.Add(this.labelSaveMsgHookIniFile);
            this.splitContainer1.Panel2.Controls.Add(this.labelOpenMsgHookIniFile);
            this.splitContainer1.Panel2.Controls.Add(this.labelMsgHookIniPath);
            this.splitContainer1.Size = new System.Drawing.Size(962, 655);
            this.splitContainer1.SplitterDistance = 679;
            this.splitContainer1.TabIndex = 0;
            // 
            // buttonRestoreSettingsOfSebStarterIni
            // 
            this.buttonRestoreSettingsOfSebStarterIni.Location = new System.Drawing.Point(356, 112);
            this.buttonRestoreSettingsOfSebStarterIni.Name = "buttonRestoreSettingsOfSebStarterIni";
            this.buttonRestoreSettingsOfSebStarterIni.Size = new System.Drawing.Size(139, 72);
            this.buttonRestoreSettingsOfSebStarterIni.TabIndex = 19;
            this.buttonRestoreSettingsOfSebStarterIni.Text = "Restore settings of SebStarter.ini";
            this.buttonRestoreSettingsOfSebStarterIni.UseVisualStyleBackColor = true;
            this.buttonRestoreSettingsOfSebStarterIni.Click += new System.EventHandler(this.buttonRestoreSettingsOfSebStarterIni_Click);
            // 
            // checkBoxEnableEaseOfAccess
            // 
            this.checkBoxEnableEaseOfAccess.AutoSize = true;
            this.checkBoxEnableEaseOfAccess.Location = new System.Drawing.Point(58, 338);
            this.checkBoxEnableEaseOfAccess.Name = "checkBoxEnableEaseOfAccess";
            this.checkBoxEnableEaseOfAccess.Size = new System.Drawing.Size(175, 21);
            this.checkBoxEnableEaseOfAccess.TabIndex = 16;
            this.checkBoxEnableEaseOfAccess.Text = "Enable Ease of Access";
            this.checkBoxEnableEaseOfAccess.UseVisualStyleBackColor = true;
            this.checkBoxEnableEaseOfAccess.CheckedChanged += new System.EventHandler(this.checkBoxEnableEaseOfAccess_CheckedChanged);
            // 
            // labelRightSide
            // 
            this.labelRightSide.AutoSize = true;
            this.labelRightSide.Location = new System.Drawing.Point(467, 492);
            this.labelRightSide.Name = "labelRightSide";
            this.labelRightSide.Size = new System.Drawing.Size(66, 17);
            this.labelRightSide.TabIndex = 15;
            this.labelRightSide.Text = "right side";
            // 
            // labelLeftSide
            // 
            this.labelLeftSide.AutoSize = true;
            this.labelLeftSide.Location = new System.Drawing.Point(320, 492);
            this.labelLeftSide.Name = "labelLeftSide";
            this.labelLeftSide.Size = new System.Drawing.Size(57, 17);
            this.labelLeftSide.TabIndex = 14;
            this.labelLeftSide.Text = "left side";
            // 
            // labelCurrentLine
            // 
            this.labelCurrentLine.AutoSize = true;
            this.labelCurrentLine.Location = new System.Drawing.Point(56, 492);
            this.labelCurrentLine.Name = "labelCurrentLine";
            this.labelCurrentLine.Size = new System.Drawing.Size(79, 17);
            this.labelCurrentLine.TabIndex = 13;
            this.labelCurrentLine.Text = "current line";
            // 
            // labelSebStarterIniPath
            // 
            this.labelSebStarterIniPath.AutoSize = true;
            this.labelSebStarterIniPath.Location = new System.Drawing.Point(54, 55);
            this.labelSebStarterIniPath.Name = "labelSebStarterIniPath";
            this.labelSebStarterIniPath.Size = new System.Drawing.Size(126, 17);
            this.labelSebStarterIniPath.TabIndex = 11;
            this.labelSebStarterIniPath.Text = "SebStarter.ini path";
            // 
            // labelSaveSebStarterIniFile
            // 
            this.labelSaveSebStarterIniFile.AutoSize = true;
            this.labelSaveSebStarterIniFile.Location = new System.Drawing.Point(54, 603);
            this.labelSaveSebStarterIniFile.Name = "labelSaveSebStarterIniFile";
            this.labelSaveSebStarterIniFile.Size = new System.Drawing.Size(152, 17);
            this.labelSaveSebStarterIniFile.TabIndex = 10;
            this.labelSaveSebStarterIniFile.Text = "Save SebStarter.ini file";
            this.labelSaveSebStarterIniFile.Click += new System.EventHandler(this.labelSaveSebStarterIniFile_Click);
            // 
            // labelOpenSebStarterIniFile
            // 
            this.labelOpenSebStarterIniFile.AutoSize = true;
            this.labelOpenSebStarterIniFile.Location = new System.Drawing.Point(54, 565);
            this.labelOpenSebStarterIniFile.Name = "labelOpenSebStarterIniFile";
            this.labelOpenSebStarterIniFile.Size = new System.Drawing.Size(155, 17);
            this.labelOpenSebStarterIniFile.TabIndex = 9;
            this.labelOpenSebStarterIniFile.Text = "Open SebStarter.ini file";
            this.labelOpenSebStarterIniFile.Click += new System.EventHandler(this.labelOpenSebStarterIniFile_Click);
            // 
            // labelBrowseSebStarterIniFolder
            // 
            this.labelBrowseSebStarterIniFolder.AutoSize = true;
            this.labelBrowseSebStarterIniFolder.Location = new System.Drawing.Point(56, 529);
            this.labelBrowseSebStarterIniFolder.Name = "labelBrowseSebStarterIniFolder";
            this.labelBrowseSebStarterIniFolder.Size = new System.Drawing.Size(184, 17);
            this.labelBrowseSebStarterIniFolder.TabIndex = 8;
            this.labelBrowseSebStarterIniFolder.Text = "Browse SebStarter.ini folder";
            this.labelBrowseSebStarterIniFolder.Click += new System.EventHandler(this.labelBrowseSebStarterFolder_Click);
            // 
            // checkBoxEnableVmWareClientShade
            // 
            this.checkBoxEnableVmWareClientShade.AutoSize = true;
            this.checkBoxEnableVmWareClientShade.Location = new System.Drawing.Point(57, 374);
            this.checkBoxEnableVmWareClientShade.Name = "checkBoxEnableVmWareClientShade";
            this.checkBoxEnableVmWareClientShade.Size = new System.Drawing.Size(212, 21);
            this.checkBoxEnableVmWareClientShade.TabIndex = 7;
            this.checkBoxEnableVmWareClientShade.Text = "Enable VMware Client Shade";
            this.checkBoxEnableVmWareClientShade.UseVisualStyleBackColor = true;
            this.checkBoxEnableVmWareClientShade.CheckedChanged += new System.EventHandler(this.checkBoxEnableVmWareClientShade_CheckedChanged);
            // 
            // checkBoxEnableLogOff
            // 
            this.checkBoxEnableLogOff.AutoSize = true;
            this.checkBoxEnableLogOff.Location = new System.Drawing.Point(58, 264);
            this.checkBoxEnableLogOff.Name = "checkBoxEnableLogOff";
            this.checkBoxEnableLogOff.Size = new System.Drawing.Size(122, 21);
            this.checkBoxEnableLogOff.TabIndex = 6;
            this.checkBoxEnableLogOff.Text = "Enable Log off";
            this.checkBoxEnableLogOff.UseVisualStyleBackColor = true;
            this.checkBoxEnableLogOff.CheckedChanged += new System.EventHandler(this.checkBoxEnableLogOff_CheckedChanged);
            // 
            // labelSebStarterOptions
            // 
            this.labelSebStarterOptions.AutoSize = true;
            this.labelSebStarterOptions.Location = new System.Drawing.Point(55, 24);
            this.labelSebStarterOptions.Name = "labelSebStarterOptions";
            this.labelSebStarterOptions.Size = new System.Drawing.Size(126, 17);
            this.labelSebStarterOptions.TabIndex = 5;
            this.labelSebStarterOptions.Text = "SebStarter options";
            // 
            // checkBoxEnableShutDown
            // 
            this.checkBoxEnableShutDown.AutoSize = true;
            this.checkBoxEnableShutDown.Location = new System.Drawing.Point(58, 300);
            this.checkBoxEnableShutDown.Name = "checkBoxEnableShutDown";
            this.checkBoxEnableShutDown.Size = new System.Drawing.Size(144, 21);
            this.checkBoxEnableShutDown.TabIndex = 4;
            this.checkBoxEnableShutDown.Text = "Enable Shut down";
            this.checkBoxEnableShutDown.UseVisualStyleBackColor = true;
            this.checkBoxEnableShutDown.CheckedChanged += new System.EventHandler(this.checkBoxEnableShutDown_CheckedChanged);
            // 
            // checkBoxEnableChangeAPassword
            // 
            this.checkBoxEnableChangeAPassword.AutoSize = true;
            this.checkBoxEnableChangeAPassword.Location = new System.Drawing.Point(57, 190);
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
            this.checkBoxEnableStartTaskManager.Location = new System.Drawing.Point(57, 227);
            this.checkBoxEnableStartTaskManager.Name = "checkBoxEnableStartTaskManager";
            this.checkBoxEnableStartTaskManager.Size = new System.Drawing.Size(203, 21);
            this.checkBoxEnableStartTaskManager.TabIndex = 2;
            this.checkBoxEnableStartTaskManager.Text = "Enable Start Task Manager";
            this.checkBoxEnableStartTaskManager.UseVisualStyleBackColor = true;
            this.checkBoxEnableStartTaskManager.CheckedChanged += new System.EventHandler(this.checkBoxEnableStartTaskManager_CheckedChanged);
            // 
            // checkBoxEnableLockThisComputer
            // 
            this.checkBoxEnableLockThisComputer.AutoSize = true;
            this.checkBoxEnableLockThisComputer.Location = new System.Drawing.Point(57, 151);
            this.checkBoxEnableLockThisComputer.Name = "checkBoxEnableLockThisComputer";
            this.checkBoxEnableLockThisComputer.Size = new System.Drawing.Size(197, 21);
            this.checkBoxEnableLockThisComputer.TabIndex = 1;
            this.checkBoxEnableLockThisComputer.Text = "Enable Lock this computer";
            this.checkBoxEnableLockThisComputer.UseVisualStyleBackColor = true;
            this.checkBoxEnableLockThisComputer.CheckedChanged += new System.EventHandler(this.checkBoxEnableLockThisComputer_CheckedChanged);
            // 
            // checkBoxEnableSwitchUser
            // 
            this.checkBoxEnableSwitchUser.AutoSize = true;
            this.checkBoxEnableSwitchUser.Location = new System.Drawing.Point(57, 112);
            this.checkBoxEnableSwitchUser.Name = "checkBoxEnableSwitchUser";
            this.checkBoxEnableSwitchUser.Size = new System.Drawing.Size(152, 21);
            this.checkBoxEnableSwitchUser.TabIndex = 0;
            this.checkBoxEnableSwitchUser.Text = "Enable Switch User";
            this.checkBoxEnableSwitchUser.UseVisualStyleBackColor = true;
            this.checkBoxEnableSwitchUser.CheckedChanged += new System.EventHandler(this.checkBoxEnableSwitchUser_CheckedChanged);
            // 
            // labelMsgHookOptions
            // 
            this.labelMsgHookOptions.AutoSize = true;
            this.labelMsgHookOptions.Location = new System.Drawing.Point(43, 36);
            this.labelMsgHookOptions.Name = "labelMsgHookOptions";
            this.labelMsgHookOptions.Size = new System.Drawing.Size(117, 17);
            this.labelMsgHookOptions.TabIndex = 0;
            this.labelMsgHookOptions.Text = "MsgHook options";
            // 
            // labelSaveMsgHookIniFile
            // 
            this.labelSaveMsgHookIniFile.AutoSize = true;
            this.labelSaveMsgHookIniFile.Location = new System.Drawing.Point(46, 603);
            this.labelSaveMsgHookIniFile.Name = "labelSaveMsgHookIniFile";
            this.labelSaveMsgHookIniFile.Size = new System.Drawing.Size(143, 17);
            this.labelSaveMsgHookIniFile.TabIndex = 18;
            this.labelSaveMsgHookIniFile.Text = "Save MsgHook.ini file";
            this.labelSaveMsgHookIniFile.Click += new System.EventHandler(this.labelSaveMsgHookIniFile_Click);
            // 
            // labelOpenMsgHookIniFile
            // 
            this.labelOpenMsgHookIniFile.AutoSize = true;
            this.labelOpenMsgHookIniFile.Location = new System.Drawing.Point(46, 565);
            this.labelOpenMsgHookIniFile.Name = "labelOpenMsgHookIniFile";
            this.labelOpenMsgHookIniFile.Size = new System.Drawing.Size(146, 17);
            this.labelOpenMsgHookIniFile.TabIndex = 17;
            this.labelOpenMsgHookIniFile.Text = "Open MsgHook.ini file";
            this.labelOpenMsgHookIniFile.Click += new System.EventHandler(this.labelOpenMsgHookIniFile_Click);
            // 
            // labelMsgHookIniPath
            // 
            this.labelMsgHookIniPath.AutoSize = true;
            this.labelMsgHookIniPath.Location = new System.Drawing.Point(43, 66);
            this.labelMsgHookIniPath.Name = "labelMsgHookIniPath";
            this.labelMsgHookIniPath.Size = new System.Drawing.Size(117, 17);
            this.labelMsgHookIniPath.TabIndex = 12;
            this.labelMsgHookIniPath.Text = "MsgHook.ini path";
            // 
            // openFileDialogSebStarterIni
            // 
            this.openFileDialogSebStarterIni.FileName = "openFileDialogSebStarterIni";
            // 
            // textBoxExamUrl
            // 
            this.textBoxExamUrl.Location = new System.Drawing.Point(223, 418);
            this.textBoxExamUrl.Name = "textBoxExamUrl";
            this.textBoxExamUrl.Size = new System.Drawing.Size(417, 22);
            this.textBoxExamUrl.TabIndex = 20;
            this.textBoxExamUrl.TextChanged += new System.EventHandler(this.textBoxExamUrl_TextChanged);
            // 
            // labelExamUrl
            // 
            this.labelExamUrl.AutoSize = true;
            this.labelExamUrl.Location = new System.Drawing.Point(59, 421);
            this.labelExamUrl.Name = "labelExamUrl";
            this.labelExamUrl.Size = new System.Drawing.Size(74, 17);
            this.labelExamUrl.TabIndex = 21;
            this.labelExamUrl.Text = "Exam URL";
            // 
            // labelPermittedApplications
            // 
            this.labelPermittedApplications.AutoSize = true;
            this.labelPermittedApplications.Location = new System.Drawing.Point(59, 459);
            this.labelPermittedApplications.Name = "labelPermittedApplications";
            this.labelPermittedApplications.Size = new System.Drawing.Size(148, 17);
            this.labelPermittedApplications.TabIndex = 22;
            this.labelPermittedApplications.Text = "Permitted Applications";
            // 
            // textBoxPermittedApplications
            // 
            this.textBoxPermittedApplications.Location = new System.Drawing.Point(223, 456);
            this.textBoxPermittedApplications.Name = "textBoxPermittedApplications";
            this.textBoxPermittedApplications.Size = new System.Drawing.Size(417, 22);
            this.textBoxPermittedApplications.TabIndex = 23;
            this.textBoxPermittedApplications.TextChanged += new System.EventHandler(this.textBoxPermittedApplications_TextChanged);
            // 
            // SebWindowsConfigForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(962, 655);
            this.Controls.Add(this.splitContainer1);
            this.Name = "SebWindowsConfigForm";
            this.Text = "SEB Windows Configuration Window";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.CheckBox checkBoxEnableSwitchUser;
        private System.Windows.Forms.CheckBox checkBoxEnableLogOff;
        private System.Windows.Forms.Label labelSebStarterOptions;
        private System.Windows.Forms.CheckBox checkBoxEnableShutDown;
        private System.Windows.Forms.CheckBox checkBoxEnableChangeAPassword;
        private System.Windows.Forms.CheckBox checkBoxEnableStartTaskManager;
        private System.Windows.Forms.CheckBox checkBoxEnableLockThisComputer;
        private System.Windows.Forms.Label labelMsgHookOptions;
        private System.Windows.Forms.CheckBox checkBoxEnableVmWareClientShade;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialogBrowseIniFiles;
        private System.Windows.Forms.OpenFileDialog openFileDialogSebStarterIni;
        private System.Windows.Forms.SaveFileDialog saveFileDialogSebStarterIni;
        private System.Windows.Forms.Label labelBrowseSebStarterIniFolder;
        private System.Windows.Forms.Label labelOpenSebStarterIniFile;
        private System.Windows.Forms.Label labelSaveSebStarterIniFile;
        private System.Windows.Forms.Label labelSebStarterIniPath;
        private System.Windows.Forms.Label labelMsgHookIniPath;
        private System.Windows.Forms.Label labelCurrentLine;
        private System.Windows.Forms.Label labelRightSide;
        private System.Windows.Forms.Label labelLeftSide;
        private System.Windows.Forms.CheckBox checkBoxEnableEaseOfAccess;
        private System.Windows.Forms.Label labelOpenMsgHookIniFile;
        private System.Windows.Forms.Label labelSaveMsgHookIniFile;
        private System.Windows.Forms.Button buttonRestoreSettingsOfSebStarterIni;
        private System.Windows.Forms.Label labelExamUrl;
        private System.Windows.Forms.TextBox textBoxExamUrl;
        private System.Windows.Forms.Label labelPermittedApplications;
        private System.Windows.Forms.TextBox textBoxPermittedApplications;
    }
}

