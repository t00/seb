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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SebWindowsConfigForm));
            this.openFileDialogSebStarterIni = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialogSebStarterIni = new System.Windows.Forms.SaveFileDialog();
            this.imageListTabIcons = new System.Windows.Forms.ImageList(this.components);
            this.tabPageBrowser = new System.Windows.Forms.TabPage();
            this.tabPageAppearance = new System.Windows.Forms.TabPage();
            this.tabPageConfigFile = new System.Windows.Forms.TabPage();
            this.labelUseEither = new System.Windows.Forms.Label();
            this.labelChooseIdentity = new System.Windows.Forms.Label();
            this.comboBoxChooseIdentity = new System.Windows.Forms.ComboBox();
            this.labelConfirmSettingsPassword = new System.Windows.Forms.Label();
            this.labelSettingsPassword = new System.Windows.Forms.Label();
            this.textBoxConfirmSettingsPassword = new System.Windows.Forms.TextBox();
            this.textBoxSettingsPassword = new System.Windows.Forms.TextBox();
            this.labelUseSEBSettingsFileFor = new System.Windows.Forms.Label();
            this.radioButtonConfiguringAClient = new System.Windows.Forms.RadioButton();
            this.radioButtonStartingAnExam = new System.Windows.Forms.RadioButton();
            this.checkBoxAllowToOpenPreferencesWindow = new System.Windows.Forms.CheckBox();
            this.buttonDefaultSettings = new System.Windows.Forms.Button();
            this.buttonRevertToLastOpened = new System.Windows.Forms.Button();
            this.labelOpenSettings = new System.Windows.Forms.Label();
            this.labelSaveSettingsAs = new System.Windows.Forms.Label();
            this.tabPageGeneral = new System.Windows.Forms.TabPage();
            this.labelSEBServerURL = new System.Windows.Forms.Label();
            this.textBoxSEBServerURL = new System.Windows.Forms.TextBox();
            this.buttonHelp = new System.Windows.Forms.Button();
            this.buttonRestartSEB = new System.Windows.Forms.Button();
            this.buttonQuit = new System.Windows.Forms.Button();
            this.buttonAbout = new System.Windows.Forms.Button();
            this.textBoxConfirmAdministratorPassword = new System.Windows.Forms.TextBox();
            this.textBoxAdministratorPassword = new System.Windows.Forms.TextBox();
            this.textBoxConfirmQuitPassword = new System.Windows.Forms.TextBox();
            this.textBoxQuitHashcode = new System.Windows.Forms.TextBox();
            this.textBoxQuitPassword = new System.Windows.Forms.TextBox();
            this.textBoxStartURL = new System.Windows.Forms.TextBox();
            this.labelConfirmAdministratorPassword = new System.Windows.Forms.Label();
            this.labelAdministratorPassword = new System.Windows.Forms.Label();
            this.labelConfirmQuitPassword = new System.Windows.Forms.Label();
            this.checkBoxAllowUserToQuitSEB = new System.Windows.Forms.CheckBox();
            this.labelQuitHashCode = new System.Windows.Forms.Label();
            this.labelQuitPassword = new System.Windows.Forms.Label();
            this.labelStartURL = new System.Windows.Forms.Label();
            this.tabControlSebWindowsConfig = new System.Windows.Forms.TabControl();
            this.tabPageDownUploads = new System.Windows.Forms.TabPage();
            this.tabPageExam = new System.Windows.Forms.TabPage();
            this.groupBoxOnlineExam = new System.Windows.Forms.GroupBox();
            this.textBoxAutostartProcess = new System.Windows.Forms.TextBox();
            this.labelSebBrowser = new System.Windows.Forms.Label();
            this.labelAutostartProcess = new System.Windows.Forms.Label();
            this.textBoxSebBrowser = new System.Windows.Forms.TextBox();
            this.labelPermittedApplications = new System.Windows.Forms.Label();
            this.textBoxPermittedApplications = new System.Windows.Forms.TextBox();
            this.tabPageApplications = new System.Windows.Forms.TabPage();
            this.tabPageNetwork = new System.Windows.Forms.TabPage();
            this.tabPageSecurity = new System.Windows.Forms.TabPage();
            this.checkBoxEnableLogging = new System.Windows.Forms.CheckBox();
            this.groupBoxSecurityOptions = new System.Windows.Forms.GroupBox();
            this.checkBoxEnablePlugins = new System.Windows.Forms.CheckBox();
            this.checkBoxEnableLog = new System.Windows.Forms.CheckBox();
            this.checkBoxEnableJavaScript = new System.Windows.Forms.CheckBox();
            this.checkBoxEnableJava = new System.Windows.Forms.CheckBox();
            this.checkBoxEnableBrowsingBackForward = new System.Windows.Forms.CheckBox();
            this.checkBoxAllowDownUploads = new System.Windows.Forms.CheckBox();
            this.checkBoxAllowFlashFullscreen = new System.Windows.Forms.CheckBox();
            this.checkBoxAllowPreferencesWindow = new System.Windows.Forms.CheckBox();
            this.checkBoxAllowQuit = new System.Windows.Forms.CheckBox();
            this.checkBoxAllowSwitchToApplications = new System.Windows.Forms.CheckBox();
            this.checkBoxOpenDownloads = new System.Windows.Forms.CheckBox();
            this.checkBoxNewBrowserWindowByScriptBlockForeign = new System.Windows.Forms.CheckBox();
            this.checkBoxNewBrowserWindowByLinkBlockForeign = new System.Windows.Forms.CheckBox();
            this.checkBoxMonitorProcesses = new System.Windows.Forms.CheckBox();
            this.checkBoxIgnoreQuitPassword = new System.Windows.Forms.CheckBox();
            this.checkBoxHookMessages = new System.Windows.Forms.CheckBox();
            this.checkBoxDownloadPDFFiles = new System.Windows.Forms.CheckBox();
            this.checkBoxCreateNewDesktop = new System.Windows.Forms.CheckBox();
            this.checkBoxAllowVirtualMachine = new System.Windows.Forms.CheckBox();
            this.checkBoxBlockPopupWindows = new System.Windows.Forms.CheckBox();
            this.tabPageRegistry = new System.Windows.Forms.TabPage();
            this.groupBoxOutsideSeb = new System.Windows.Forms.GroupBox();
            this.checkBoxOutsideSebEnableSwitchUser = new System.Windows.Forms.CheckBox();
            this.checkBoxOutsideSebEnableLockThisComputer = new System.Windows.Forms.CheckBox();
            this.checkBoxOutsideSebEnableChangeAPassword = new System.Windows.Forms.CheckBox();
            this.checkBoxOutsideSebEnableStartTaskManager = new System.Windows.Forms.CheckBox();
            this.checkBoxOutsideSebEnableLogOff = new System.Windows.Forms.CheckBox();
            this.checkBoxOutsideSebEnableShutDown = new System.Windows.Forms.CheckBox();
            this.checkBoxOutsideSebEnableEaseOfAccess = new System.Windows.Forms.CheckBox();
            this.checkBoxOutsideSebEnableVmWareClientShade = new System.Windows.Forms.CheckBox();
            this.groupBoxSetOutsideSebValues = new System.Windows.Forms.GroupBox();
            this.radioButtonInsideValuesManually = new System.Windows.Forms.RadioButton();
            this.radioButtonPreviousValuesFromFile = new System.Windows.Forms.RadioButton();
            this.radioButtonEnvironmentValues = new System.Windows.Forms.RadioButton();
            this.groupBoxInsideSeb = new System.Windows.Forms.GroupBox();
            this.checkBoxInsideSebEnableSwitchUser = new System.Windows.Forms.CheckBox();
            this.checkBoxInsideSebEnableLockThisComputer = new System.Windows.Forms.CheckBox();
            this.checkBoxInsideSebEnableChangeAPassword = new System.Windows.Forms.CheckBox();
            this.checkBoxInsideSebEnableStartTaskManager = new System.Windows.Forms.CheckBox();
            this.checkBoxInsideSebEnableLogOff = new System.Windows.Forms.CheckBox();
            this.checkBoxInsideSebEnableShutDown = new System.Windows.Forms.CheckBox();
            this.checkBoxInsideSebEnableEaseOfAccess = new System.Windows.Forms.CheckBox();
            this.checkBoxInsideSebEnableVmWareClientShade = new System.Windows.Forms.CheckBox();
            this.tabPageHookedKeys = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
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
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.checkBoxEnableEsc = new System.Windows.Forms.CheckBox();
            this.checkBoxEnableCtrlEsc = new System.Windows.Forms.CheckBox();
            this.checkBoxEnableAltEsc = new System.Windows.Forms.CheckBox();
            this.checkBoxEnableAltTab = new System.Windows.Forms.CheckBox();
            this.checkBoxEnableAltF4 = new System.Windows.Forms.CheckBox();
            this.checkBoxEnableStartMenu = new System.Windows.Forms.CheckBox();
            this.checkBoxEnableRightMouse = new System.Windows.Forms.CheckBox();
            this.tabPageExitKeys = new System.Windows.Forms.TabPage();
            this.groupBoxExitSequence = new System.Windows.Forms.GroupBox();
            this.listBoxExitKey1 = new System.Windows.Forms.ListBox();
            this.listBoxExitKey3 = new System.Windows.Forms.ListBox();
            this.listBoxExitKey2 = new System.Windows.Forms.ListBox();
            this.numericUpDown2 = new System.Windows.Forms.NumericUpDown();
            this.tabPageConfigFile.SuspendLayout();
            this.tabPageGeneral.SuspendLayout();
            this.tabControlSebWindowsConfig.SuspendLayout();
            this.tabPageExam.SuspendLayout();
            this.groupBoxOnlineExam.SuspendLayout();
            this.tabPageSecurity.SuspendLayout();
            this.groupBoxSecurityOptions.SuspendLayout();
            this.tabPageRegistry.SuspendLayout();
            this.groupBoxOutsideSeb.SuspendLayout();
            this.groupBoxSetOutsideSebValues.SuspendLayout();
            this.groupBoxInsideSeb.SuspendLayout();
            this.tabPageHookedKeys.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tabPageExitKeys.SuspendLayout();
            this.groupBoxExitSequence.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).BeginInit();
            this.SuspendLayout();
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
            // imageListTabIcons
            // 
            this.imageListTabIcons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListTabIcons.ImageStream")));
            this.imageListTabIcons.TransparentColor = System.Drawing.Color.White;
            this.imageListTabIcons.Images.SetKeyName(0, "041_Sort_32x32_72.png");
            this.imageListTabIcons.Images.SetKeyName(1, "109_AllAnnotations_Help_32x32_72.png");
            this.imageListTabIcons.Images.SetKeyName(2, "EntityDataModel_ADODotNetDataService_Better.png");
            this.imageListTabIcons.Images.SetKeyName(3, "Gear.png");
            this.imageListTabIcons.Images.SetKeyName(4, "SebIcon.png");
            // 
            // tabPageBrowser
            // 
            this.tabPageBrowser.ImageIndex = 4;
            this.tabPageBrowser.Location = new System.Drawing.Point(4, 39);
            this.tabPageBrowser.Name = "tabPageBrowser";
            this.tabPageBrowser.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageBrowser.Size = new System.Drawing.Size(1054, 469);
            this.tabPageBrowser.TabIndex = 14;
            this.tabPageBrowser.Text = "Browser";
            this.tabPageBrowser.UseVisualStyleBackColor = true;
            // 
            // tabPageAppearance
            // 
            this.tabPageAppearance.Location = new System.Drawing.Point(4, 39);
            this.tabPageAppearance.Name = "tabPageAppearance";
            this.tabPageAppearance.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageAppearance.Size = new System.Drawing.Size(1054, 469);
            this.tabPageAppearance.TabIndex = 8;
            this.tabPageAppearance.Text = "Appearance";
            this.tabPageAppearance.UseVisualStyleBackColor = true;
            // 
            // tabPageConfigFile
            // 
            this.tabPageConfigFile.Controls.Add(this.labelUseEither);
            this.tabPageConfigFile.Controls.Add(this.labelChooseIdentity);
            this.tabPageConfigFile.Controls.Add(this.comboBoxChooseIdentity);
            this.tabPageConfigFile.Controls.Add(this.labelConfirmSettingsPassword);
            this.tabPageConfigFile.Controls.Add(this.labelSettingsPassword);
            this.tabPageConfigFile.Controls.Add(this.textBoxConfirmSettingsPassword);
            this.tabPageConfigFile.Controls.Add(this.textBoxSettingsPassword);
            this.tabPageConfigFile.Controls.Add(this.labelUseSEBSettingsFileFor);
            this.tabPageConfigFile.Controls.Add(this.radioButtonConfiguringAClient);
            this.tabPageConfigFile.Controls.Add(this.radioButtonStartingAnExam);
            this.tabPageConfigFile.Controls.Add(this.checkBoxAllowToOpenPreferencesWindow);
            this.tabPageConfigFile.Controls.Add(this.buttonDefaultSettings);
            this.tabPageConfigFile.Controls.Add(this.buttonRevertToLastOpened);
            this.tabPageConfigFile.Controls.Add(this.labelOpenSettings);
            this.tabPageConfigFile.Controls.Add(this.labelSaveSettingsAs);
            this.tabPageConfigFile.ImageIndex = 3;
            this.tabPageConfigFile.Location = new System.Drawing.Point(4, 39);
            this.tabPageConfigFile.Name = "tabPageConfigFile";
            this.tabPageConfigFile.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageConfigFile.Size = new System.Drawing.Size(1054, 469);
            this.tabPageConfigFile.TabIndex = 6;
            this.tabPageConfigFile.Text = "Config File";
            this.tabPageConfigFile.UseVisualStyleBackColor = true;
            // 
            // labelUseEither
            // 
            this.labelUseEither.AutoSize = true;
            this.labelUseEither.Location = new System.Drawing.Point(67, 218);
            this.labelUseEither.Name = "labelUseEither";
            this.labelUseEither.Size = new System.Drawing.Size(366, 17);
            this.labelUseEither.TabIndex = 59;
            this.labelUseEither.Text = "Use either a cryptographic identity or a password or both";
            // 
            // labelChooseIdentity
            // 
            this.labelChooseIdentity.AutoSize = true;
            this.labelChooseIdentity.Location = new System.Drawing.Point(64, 164);
            this.labelChooseIdentity.Name = "labelChooseIdentity";
            this.labelChooseIdentity.Size = new System.Drawing.Size(385, 17);
            this.labelChooseIdentity.TabIndex = 58;
            this.labelChooseIdentity.Text = "Choose identity to be used for encrypting SEB settings file...";
            // 
            // comboBoxChooseIdentity
            // 
            this.comboBoxChooseIdentity.FormattingEnabled = true;
            this.comboBoxChooseIdentity.Location = new System.Drawing.Point(60, 187);
            this.comboBoxChooseIdentity.Name = "comboBoxChooseIdentity";
            this.comboBoxChooseIdentity.Size = new System.Drawing.Size(657, 24);
            this.comboBoxChooseIdentity.TabIndex = 57;
            this.comboBoxChooseIdentity.SelectedIndexChanged += new System.EventHandler(this.comboBoxChooseIdentity_SelectedIndexChanged);
            // 
            // labelConfirmSettingsPassword
            // 
            this.labelConfirmSettingsPassword.AutoSize = true;
            this.labelConfirmSettingsPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelConfirmSettingsPassword.Location = new System.Drawing.Point(61, 295);
            this.labelConfirmSettingsPassword.Name = "labelConfirmSettingsPassword";
            this.labelConfirmSettingsPassword.Size = new System.Drawing.Size(173, 17);
            this.labelConfirmSettingsPassword.TabIndex = 56;
            this.labelConfirmSettingsPassword.Text = "Confirm settings password";
            // 
            // labelSettingsPassword
            // 
            this.labelSettingsPassword.AutoSize = true;
            this.labelSettingsPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelSettingsPassword.Location = new System.Drawing.Point(61, 260);
            this.labelSettingsPassword.Name = "labelSettingsPassword";
            this.labelSettingsPassword.Size = new System.Drawing.Size(123, 17);
            this.labelSettingsPassword.TabIndex = 55;
            this.labelSettingsPassword.Text = "Settings password";
            // 
            // textBoxConfirmSettingsPassword
            // 
            this.textBoxConfirmSettingsPassword.Font = new System.Drawing.Font("Courier New", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxConfirmSettingsPassword.Location = new System.Drawing.Point(287, 295);
            this.textBoxConfirmSettingsPassword.Name = "textBoxConfirmSettingsPassword";
            this.textBoxConfirmSettingsPassword.Size = new System.Drawing.Size(430, 22);
            this.textBoxConfirmSettingsPassword.TabIndex = 54;
            this.textBoxConfirmSettingsPassword.WordWrap = false;
            this.textBoxConfirmSettingsPassword.TextChanged += new System.EventHandler(this.textBoxConfirmSettingsPassword_TextChanged);
            // 
            // textBoxSettingsPassword
            // 
            this.textBoxSettingsPassword.Font = new System.Drawing.Font("Courier New", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxSettingsPassword.Location = new System.Drawing.Point(287, 255);
            this.textBoxSettingsPassword.Name = "textBoxSettingsPassword";
            this.textBoxSettingsPassword.Size = new System.Drawing.Size(430, 22);
            this.textBoxSettingsPassword.TabIndex = 53;
            this.textBoxSettingsPassword.WordWrap = false;
            this.textBoxSettingsPassword.TextChanged += new System.EventHandler(this.textBoxSettingsPassword_TextChanged);
            // 
            // labelUseSEBSettingsFileFor
            // 
            this.labelUseSEBSettingsFileFor.AutoSize = true;
            this.labelUseSEBSettingsFileFor.Location = new System.Drawing.Point(60, 34);
            this.labelUseSEBSettingsFileFor.Name = "labelUseSEBSettingsFileFor";
            this.labelUseSEBSettingsFileFor.Size = new System.Drawing.Size(172, 17);
            this.labelUseSEBSettingsFileFor.TabIndex = 52;
            this.labelUseSEBSettingsFileFor.Text = "Use SEB settings file for...";
            // 
            // radioButtonConfiguringAClient
            // 
            this.radioButtonConfiguringAClient.AutoSize = true;
            this.radioButtonConfiguringAClient.Location = new System.Drawing.Point(60, 90);
            this.radioButtonConfiguringAClient.Name = "radioButtonConfiguringAClient";
            this.radioButtonConfiguringAClient.Size = new System.Drawing.Size(148, 21);
            this.radioButtonConfiguringAClient.TabIndex = 51;
            this.radioButtonConfiguringAClient.TabStop = true;
            this.radioButtonConfiguringAClient.Text = "configuring a client";
            this.radioButtonConfiguringAClient.UseVisualStyleBackColor = true;
            this.radioButtonConfiguringAClient.CheckedChanged += new System.EventHandler(this.radioButtonConfiguringAClient_CheckedChanged);
            // 
            // radioButtonStartingAnExam
            // 
            this.radioButtonStartingAnExam.AutoSize = true;
            this.radioButtonStartingAnExam.Checked = true;
            this.radioButtonStartingAnExam.Location = new System.Drawing.Point(60, 63);
            this.radioButtonStartingAnExam.Name = "radioButtonStartingAnExam";
            this.radioButtonStartingAnExam.Size = new System.Drawing.Size(133, 21);
            this.radioButtonStartingAnExam.TabIndex = 50;
            this.radioButtonStartingAnExam.TabStop = true;
            this.radioButtonStartingAnExam.Text = "starting an exam";
            this.radioButtonStartingAnExam.UseVisualStyleBackColor = true;
            this.radioButtonStartingAnExam.CheckedChanged += new System.EventHandler(this.radioButtonStartingAnExam_CheckedChanged);
            // 
            // checkBoxAllowToOpenPreferencesWindow
            // 
            this.checkBoxAllowToOpenPreferencesWindow.AutoSize = true;
            this.checkBoxAllowToOpenPreferencesWindow.Checked = true;
            this.checkBoxAllowToOpenPreferencesWindow.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxAllowToOpenPreferencesWindow.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxAllowToOpenPreferencesWindow.Location = new System.Drawing.Point(60, 128);
            this.checkBoxAllowToOpenPreferencesWindow.Name = "checkBoxAllowToOpenPreferencesWindow";
            this.checkBoxAllowToOpenPreferencesWindow.Size = new System.Drawing.Size(300, 21);
            this.checkBoxAllowToOpenPreferencesWindow.TabIndex = 49;
            this.checkBoxAllowToOpenPreferencesWindow.Text = "Allow to open preferences window on client";
            this.checkBoxAllowToOpenPreferencesWindow.UseVisualStyleBackColor = true;
            this.checkBoxAllowToOpenPreferencesWindow.CheckedChanged += new System.EventHandler(this.checkBoxAllowToOpenPreferencesWindowOnClient_CheckedChanged);
            // 
            // buttonDefaultSettings
            // 
            this.buttonDefaultSettings.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonDefaultSettings.Location = new System.Drawing.Point(63, 349);
            this.buttonDefaultSettings.Name = "buttonDefaultSettings";
            this.buttonDefaultSettings.Size = new System.Drawing.Size(191, 25);
            this.buttonDefaultSettings.TabIndex = 44;
            this.buttonDefaultSettings.Text = "Default Settings";
            this.buttonDefaultSettings.UseVisualStyleBackColor = true;
            this.buttonDefaultSettings.Click += new System.EventHandler(this.buttonDefaultSettings_Click);
            // 
            // buttonRevertToLastOpened
            // 
            this.buttonRevertToLastOpened.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonRevertToLastOpened.Location = new System.Drawing.Point(63, 389);
            this.buttonRevertToLastOpened.Name = "buttonRevertToLastOpened";
            this.buttonRevertToLastOpened.Size = new System.Drawing.Size(191, 25);
            this.buttonRevertToLastOpened.TabIndex = 19;
            this.buttonRevertToLastOpened.Text = "Revert To Last Opened";
            this.buttonRevertToLastOpened.UseVisualStyleBackColor = true;
            this.buttonRevertToLastOpened.Click += new System.EventHandler(this.buttonRevertToLastOpened_Click);
            // 
            // labelOpenSettings
            // 
            this.labelOpenSettings.AutoSize = true;
            this.labelOpenSettings.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelOpenSettings.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelOpenSettings.Location = new System.Drawing.Point(287, 349);
            this.labelOpenSettings.Name = "labelOpenSettings";
            this.labelOpenSettings.Size = new System.Drawing.Size(112, 19);
            this.labelOpenSettings.TabIndex = 9;
            this.labelOpenSettings.Text = "Open Settings...";
            this.labelOpenSettings.Click += new System.EventHandler(this.labelOpenSettings_Click);
            // 
            // labelSaveSettingsAs
            // 
            this.labelSaveSettingsAs.AutoSize = true;
            this.labelSaveSettingsAs.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelSaveSettingsAs.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelSaveSettingsAs.Location = new System.Drawing.Point(287, 389);
            this.labelSaveSettingsAs.Name = "labelSaveSettingsAs";
            this.labelSaveSettingsAs.Size = new System.Drawing.Size(129, 19);
            this.labelSaveSettingsAs.TabIndex = 10;
            this.labelSaveSettingsAs.Text = "Save Settings As...";
            this.labelSaveSettingsAs.Click += new System.EventHandler(this.labelSaveSettingsAs_Click);
            // 
            // tabPageGeneral
            // 
            this.tabPageGeneral.Controls.Add(this.labelSEBServerURL);
            this.tabPageGeneral.Controls.Add(this.textBoxSEBServerURL);
            this.tabPageGeneral.Controls.Add(this.buttonHelp);
            this.tabPageGeneral.Controls.Add(this.buttonRestartSEB);
            this.tabPageGeneral.Controls.Add(this.buttonQuit);
            this.tabPageGeneral.Controls.Add(this.buttonAbout);
            this.tabPageGeneral.Controls.Add(this.textBoxConfirmAdministratorPassword);
            this.tabPageGeneral.Controls.Add(this.textBoxAdministratorPassword);
            this.tabPageGeneral.Controls.Add(this.textBoxConfirmQuitPassword);
            this.tabPageGeneral.Controls.Add(this.textBoxQuitHashcode);
            this.tabPageGeneral.Controls.Add(this.textBoxQuitPassword);
            this.tabPageGeneral.Controls.Add(this.textBoxStartURL);
            this.tabPageGeneral.Controls.Add(this.labelConfirmAdministratorPassword);
            this.tabPageGeneral.Controls.Add(this.labelAdministratorPassword);
            this.tabPageGeneral.Controls.Add(this.labelConfirmQuitPassword);
            this.tabPageGeneral.Controls.Add(this.checkBoxAllowUserToQuitSEB);
            this.tabPageGeneral.Controls.Add(this.labelQuitHashCode);
            this.tabPageGeneral.Controls.Add(this.labelQuitPassword);
            this.tabPageGeneral.Controls.Add(this.labelStartURL);
            this.tabPageGeneral.ImageIndex = 5;
            this.tabPageGeneral.Location = new System.Drawing.Point(4, 39);
            this.tabPageGeneral.Name = "tabPageGeneral";
            this.tabPageGeneral.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageGeneral.Size = new System.Drawing.Size(1054, 469);
            this.tabPageGeneral.TabIndex = 4;
            this.tabPageGeneral.Text = "General";
            this.tabPageGeneral.UseVisualStyleBackColor = true;
            // 
            // labelSEBServerURL
            // 
            this.labelSEBServerURL.AutoSize = true;
            this.labelSEBServerURL.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelSEBServerURL.Location = new System.Drawing.Point(126, 71);
            this.labelSEBServerURL.Name = "labelSEBServerURL";
            this.labelSEBServerURL.Size = new System.Drawing.Size(113, 17);
            this.labelSEBServerURL.TabIndex = 47;
            this.labelSEBServerURL.Text = "SEB Server URL";
            // 
            // textBoxSEBServerURL
            // 
            this.textBoxSEBServerURL.Font = new System.Drawing.Font("Courier New", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxSEBServerURL.Location = new System.Drawing.Point(245, 66);
            this.textBoxSEBServerURL.Name = "textBoxSEBServerURL";
            this.textBoxSEBServerURL.Size = new System.Drawing.Size(430, 22);
            this.textBoxSEBServerURL.TabIndex = 46;
            this.textBoxSEBServerURL.TextChanged += new System.EventHandler(this.textBoxSEBServerURL_TextChanged);
            // 
            // buttonHelp
            // 
            this.buttonHelp.Location = new System.Drawing.Point(230, 384);
            this.buttonHelp.Name = "buttonHelp";
            this.buttonHelp.Size = new System.Drawing.Size(75, 23);
            this.buttonHelp.TabIndex = 45;
            this.buttonHelp.Text = "Help";
            this.buttonHelp.UseVisualStyleBackColor = true;
            // 
            // buttonRestartSEB
            // 
            this.buttonRestartSEB.Location = new System.Drawing.Point(492, 384);
            this.buttonRestartSEB.Name = "buttonRestartSEB";
            this.buttonRestartSEB.Size = new System.Drawing.Size(103, 23);
            this.buttonRestartSEB.TabIndex = 44;
            this.buttonRestartSEB.Text = "Restart SEB";
            this.buttonRestartSEB.UseVisualStyleBackColor = true;
            // 
            // buttonQuit
            // 
            this.buttonQuit.Location = new System.Drawing.Point(363, 384);
            this.buttonQuit.Name = "buttonQuit";
            this.buttonQuit.Size = new System.Drawing.Size(75, 23);
            this.buttonQuit.TabIndex = 43;
            this.buttonQuit.Text = "Quit";
            this.buttonQuit.UseVisualStyleBackColor = true;
            // 
            // buttonAbout
            // 
            this.buttonAbout.Location = new System.Drawing.Point(108, 384);
            this.buttonAbout.Name = "buttonAbout";
            this.buttonAbout.Size = new System.Drawing.Size(75, 23);
            this.buttonAbout.TabIndex = 42;
            this.buttonAbout.Text = "About";
            this.buttonAbout.UseVisualStyleBackColor = true;
            // 
            // textBoxConfirmAdministratorPassword
            // 
            this.textBoxConfirmAdministratorPassword.Font = new System.Drawing.Font("Courier New", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxConfirmAdministratorPassword.Location = new System.Drawing.Point(245, 146);
            this.textBoxConfirmAdministratorPassword.Name = "textBoxConfirmAdministratorPassword";
            this.textBoxConfirmAdministratorPassword.Size = new System.Drawing.Size(430, 22);
            this.textBoxConfirmAdministratorPassword.TabIndex = 41;
            this.textBoxConfirmAdministratorPassword.WordWrap = false;
            this.textBoxConfirmAdministratorPassword.TextChanged += new System.EventHandler(this.textBoxConfirmAdministratorPassword_TextChanged);
            // 
            // textBoxAdministratorPassword
            // 
            this.textBoxAdministratorPassword.Font = new System.Drawing.Font("Courier New", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxAdministratorPassword.Location = new System.Drawing.Point(245, 115);
            this.textBoxAdministratorPassword.Name = "textBoxAdministratorPassword";
            this.textBoxAdministratorPassword.Size = new System.Drawing.Size(430, 22);
            this.textBoxAdministratorPassword.TabIndex = 39;
            this.textBoxAdministratorPassword.WordWrap = false;
            this.textBoxAdministratorPassword.TextChanged += new System.EventHandler(this.textBoxAdministratorPassword_TextChanged);
            // 
            // textBoxConfirmQuitPassword
            // 
            this.textBoxConfirmQuitPassword.Font = new System.Drawing.Font("Courier New", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxConfirmQuitPassword.Location = new System.Drawing.Point(245, 283);
            this.textBoxConfirmQuitPassword.Name = "textBoxConfirmQuitPassword";
            this.textBoxConfirmQuitPassword.Size = new System.Drawing.Size(430, 22);
            this.textBoxConfirmQuitPassword.TabIndex = 37;
            this.textBoxConfirmQuitPassword.WordWrap = false;
            this.textBoxConfirmQuitPassword.TextChanged += new System.EventHandler(this.textBoxConfirmQuitPassword_TextChanged);
            // 
            // textBoxQuitHashcode
            // 
            this.textBoxQuitHashcode.Font = new System.Drawing.Font("Courier New", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxQuitHashcode.Location = new System.Drawing.Point(245, 321);
            this.textBoxQuitHashcode.Name = "textBoxQuitHashcode";
            this.textBoxQuitHashcode.ReadOnly = true;
            this.textBoxQuitHashcode.Size = new System.Drawing.Size(430, 22);
            this.textBoxQuitHashcode.TabIndex = 34;
            // 
            // textBoxQuitPassword
            // 
            this.textBoxQuitPassword.Font = new System.Drawing.Font("Courier New", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxQuitPassword.Location = new System.Drawing.Point(245, 244);
            this.textBoxQuitPassword.Name = "textBoxQuitPassword";
            this.textBoxQuitPassword.Size = new System.Drawing.Size(430, 22);
            this.textBoxQuitPassword.TabIndex = 33;
            this.textBoxQuitPassword.WordWrap = false;
            this.textBoxQuitPassword.TextChanged += new System.EventHandler(this.textBoxQuitPassword_TextChanged_1);
            // 
            // textBoxStartURL
            // 
            this.textBoxStartURL.Font = new System.Drawing.Font("Courier New", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxStartURL.Location = new System.Drawing.Point(245, 38);
            this.textBoxStartURL.Name = "textBoxStartURL";
            this.textBoxStartURL.Size = new System.Drawing.Size(430, 22);
            this.textBoxStartURL.TabIndex = 21;
            this.textBoxStartURL.TextChanged += new System.EventHandler(this.textBoxStartURL_TextChanged);
            // 
            // labelConfirmAdministratorPassword
            // 
            this.labelConfirmAdministratorPassword.AutoSize = true;
            this.labelConfirmAdministratorPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelConfirmAdministratorPassword.Location = new System.Drawing.Point(33, 146);
            this.labelConfirmAdministratorPassword.Name = "labelConfirmAdministratorPassword";
            this.labelConfirmAdministratorPassword.Size = new System.Drawing.Size(206, 17);
            this.labelConfirmAdministratorPassword.TabIndex = 40;
            this.labelConfirmAdministratorPassword.Text = "Confirm administrator password";
            // 
            // labelAdministratorPassword
            // 
            this.labelAdministratorPassword.AutoSize = true;
            this.labelAdministratorPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelAdministratorPassword.Location = new System.Drawing.Point(84, 115);
            this.labelAdministratorPassword.Name = "labelAdministratorPassword";
            this.labelAdministratorPassword.Size = new System.Drawing.Size(155, 17);
            this.labelAdministratorPassword.TabIndex = 38;
            this.labelAdministratorPassword.Text = "Administrator password";
            // 
            // labelConfirmQuitPassword
            // 
            this.labelConfirmQuitPassword.AutoSize = true;
            this.labelConfirmQuitPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelConfirmQuitPassword.Location = new System.Drawing.Point(92, 283);
            this.labelConfirmQuitPassword.Name = "labelConfirmQuitPassword";
            this.labelConfirmQuitPassword.Size = new System.Drawing.Size(147, 17);
            this.labelConfirmQuitPassword.TabIndex = 36;
            this.labelConfirmQuitPassword.Text = "Confirm quit password";
            // 
            // checkBoxAllowUserToQuitSEB
            // 
            this.checkBoxAllowUserToQuitSEB.AutoSize = true;
            this.checkBoxAllowUserToQuitSEB.Location = new System.Drawing.Point(62, 198);
            this.checkBoxAllowUserToQuitSEB.Name = "checkBoxAllowUserToQuitSEB";
            this.checkBoxAllowUserToQuitSEB.Size = new System.Drawing.Size(168, 21);
            this.checkBoxAllowUserToQuitSEB.TabIndex = 35;
            this.checkBoxAllowUserToQuitSEB.Text = "Allow user to quit SEB";
            this.checkBoxAllowUserToQuitSEB.UseVisualStyleBackColor = true;
            this.checkBoxAllowUserToQuitSEB.CheckedChanged += new System.EventHandler(this.checkBoxAllowUserToQuitSEB_CheckedChanged);
            // 
            // labelQuitHashCode
            // 
            this.labelQuitHashCode.AutoSize = true;
            this.labelQuitHashCode.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelQuitHashCode.Location = new System.Drawing.Point(130, 321);
            this.labelQuitHashCode.Name = "labelQuitHashCode";
            this.labelQuitHashCode.Size = new System.Drawing.Size(100, 17);
            this.labelQuitHashCode.TabIndex = 32;
            this.labelQuitHashCode.Text = "Quit hashcode";
            // 
            // labelQuitPassword
            // 
            this.labelQuitPassword.AutoSize = true;
            this.labelQuitPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelQuitPassword.Location = new System.Drawing.Point(141, 244);
            this.labelQuitPassword.Name = "labelQuitPassword";
            this.labelQuitPassword.Size = new System.Drawing.Size(98, 17);
            this.labelQuitPassword.TabIndex = 30;
            this.labelQuitPassword.Text = "Quit password";
            // 
            // labelStartURL
            // 
            this.labelStartURL.AutoSize = true;
            this.labelStartURL.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelStartURL.Location = new System.Drawing.Point(169, 40);
            this.labelStartURL.Name = "labelStartURL";
            this.labelStartURL.Size = new System.Drawing.Size(70, 17);
            this.labelStartURL.TabIndex = 22;
            this.labelStartURL.Text = "Start URL";
            // 
            // tabControlSebWindowsConfig
            // 
            this.tabControlSebWindowsConfig.Controls.Add(this.tabPageGeneral);
            this.tabControlSebWindowsConfig.Controls.Add(this.tabPageConfigFile);
            this.tabControlSebWindowsConfig.Controls.Add(this.tabPageAppearance);
            this.tabControlSebWindowsConfig.Controls.Add(this.tabPageBrowser);
            this.tabControlSebWindowsConfig.Controls.Add(this.tabPageDownUploads);
            this.tabControlSebWindowsConfig.Controls.Add(this.tabPageExam);
            this.tabControlSebWindowsConfig.Controls.Add(this.tabPageApplications);
            this.tabControlSebWindowsConfig.Controls.Add(this.tabPageNetwork);
            this.tabControlSebWindowsConfig.Controls.Add(this.tabPageSecurity);
            this.tabControlSebWindowsConfig.Controls.Add(this.tabPageRegistry);
            this.tabControlSebWindowsConfig.Controls.Add(this.tabPageHookedKeys);
            this.tabControlSebWindowsConfig.Controls.Add(this.tabPageExitKeys);
            this.tabControlSebWindowsConfig.ImageList = this.imageListTabIcons;
            this.tabControlSebWindowsConfig.Location = new System.Drawing.Point(45, 31);
            this.tabControlSebWindowsConfig.Name = "tabControlSebWindowsConfig";
            this.tabControlSebWindowsConfig.SelectedIndex = 0;
            this.tabControlSebWindowsConfig.Size = new System.Drawing.Size(1062, 512);
            this.tabControlSebWindowsConfig.TabIndex = 2;
            // 
            // tabPageDownUploads
            // 
            this.tabPageDownUploads.ImageIndex = 0;
            this.tabPageDownUploads.Location = new System.Drawing.Point(4, 39);
            this.tabPageDownUploads.Name = "tabPageDownUploads";
            this.tabPageDownUploads.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageDownUploads.Size = new System.Drawing.Size(1054, 469);
            this.tabPageDownUploads.TabIndex = 17;
            this.tabPageDownUploads.Text = "Down/Uploads";
            this.tabPageDownUploads.UseVisualStyleBackColor = true;
            // 
            // tabPageExam
            // 
            this.tabPageExam.Controls.Add(this.groupBoxOnlineExam);
            this.tabPageExam.Location = new System.Drawing.Point(4, 39);
            this.tabPageExam.Name = "tabPageExam";
            this.tabPageExam.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageExam.Size = new System.Drawing.Size(1054, 469);
            this.tabPageExam.TabIndex = 18;
            this.tabPageExam.Text = "Exam";
            this.tabPageExam.UseVisualStyleBackColor = true;
            // 
            // groupBoxOnlineExam
            // 
            this.groupBoxOnlineExam.Controls.Add(this.textBoxAutostartProcess);
            this.groupBoxOnlineExam.Controls.Add(this.labelSebBrowser);
            this.groupBoxOnlineExam.Controls.Add(this.labelAutostartProcess);
            this.groupBoxOnlineExam.Controls.Add(this.textBoxSebBrowser);
            this.groupBoxOnlineExam.Controls.Add(this.labelPermittedApplications);
            this.groupBoxOnlineExam.Controls.Add(this.textBoxPermittedApplications);
            this.groupBoxOnlineExam.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxOnlineExam.Location = new System.Drawing.Point(23, 22);
            this.groupBoxOnlineExam.Name = "groupBoxOnlineExam";
            this.groupBoxOnlineExam.Size = new System.Drawing.Size(600, 210);
            this.groupBoxOnlineExam.TabIndex = 26;
            this.groupBoxOnlineExam.TabStop = false;
            this.groupBoxOnlineExam.Text = "Online exam";
            // 
            // textBoxAutostartProcess
            // 
            this.textBoxAutostartProcess.Font = new System.Drawing.Font("Courier New", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxAutostartProcess.Location = new System.Drawing.Point(153, 60);
            this.textBoxAutostartProcess.Name = "textBoxAutostartProcess";
            this.textBoxAutostartProcess.Size = new System.Drawing.Size(430, 22);
            this.textBoxAutostartProcess.TabIndex = 27;
            // 
            // labelSebBrowser
            // 
            this.labelSebBrowser.AutoSize = true;
            this.labelSebBrowser.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelSebBrowser.Location = new System.Drawing.Point(6, 32);
            this.labelSebBrowser.Name = "labelSebBrowser";
            this.labelSebBrowser.Size = new System.Drawing.Size(89, 17);
            this.labelSebBrowser.TabIndex = 26;
            this.labelSebBrowser.Text = "SEB browser";
            // 
            // labelAutostartProcess
            // 
            this.labelAutostartProcess.AutoSize = true;
            this.labelAutostartProcess.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelAutostartProcess.Location = new System.Drawing.Point(6, 62);
            this.labelAutostartProcess.Name = "labelAutostartProcess";
            this.labelAutostartProcess.Size = new System.Drawing.Size(119, 17);
            this.labelAutostartProcess.TabIndex = 25;
            this.labelAutostartProcess.Text = "Autostart process";
            // 
            // textBoxSebBrowser
            // 
            this.textBoxSebBrowser.Font = new System.Drawing.Font("Courier New", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxSebBrowser.Location = new System.Drawing.Point(153, 30);
            this.textBoxSebBrowser.Name = "textBoxSebBrowser";
            this.textBoxSebBrowser.Size = new System.Drawing.Size(430, 22);
            this.textBoxSebBrowser.TabIndex = 24;
            // 
            // labelPermittedApplications
            // 
            this.labelPermittedApplications.AutoSize = true;
            this.labelPermittedApplications.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPermittedApplications.Location = new System.Drawing.Point(6, 122);
            this.labelPermittedApplications.Name = "labelPermittedApplications";
            this.labelPermittedApplications.Size = new System.Drawing.Size(147, 17);
            this.labelPermittedApplications.TabIndex = 22;
            this.labelPermittedApplications.Text = "Permitted applications";
            // 
            // textBoxPermittedApplications
            // 
            this.textBoxPermittedApplications.Font = new System.Drawing.Font("Courier New", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxPermittedApplications.Location = new System.Drawing.Point(153, 120);
            this.textBoxPermittedApplications.Name = "textBoxPermittedApplications";
            this.textBoxPermittedApplications.Size = new System.Drawing.Size(430, 22);
            this.textBoxPermittedApplications.TabIndex = 23;
            // 
            // tabPageApplications
            // 
            this.tabPageApplications.Location = new System.Drawing.Point(4, 39);
            this.tabPageApplications.Name = "tabPageApplications";
            this.tabPageApplications.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageApplications.Size = new System.Drawing.Size(1054, 469);
            this.tabPageApplications.TabIndex = 21;
            this.tabPageApplications.Text = "Applications";
            this.tabPageApplications.UseVisualStyleBackColor = true;
            // 
            // tabPageNetwork
            // 
            this.tabPageNetwork.Location = new System.Drawing.Point(4, 39);
            this.tabPageNetwork.Name = "tabPageNetwork";
            this.tabPageNetwork.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageNetwork.Size = new System.Drawing.Size(1054, 469);
            this.tabPageNetwork.TabIndex = 23;
            this.tabPageNetwork.Text = "Network";
            this.tabPageNetwork.UseVisualStyleBackColor = true;
            // 
            // tabPageSecurity
            // 
            this.tabPageSecurity.Controls.Add(this.checkBoxEnableLogging);
            this.tabPageSecurity.Controls.Add(this.groupBoxSecurityOptions);
            this.tabPageSecurity.Location = new System.Drawing.Point(4, 39);
            this.tabPageSecurity.Name = "tabPageSecurity";
            this.tabPageSecurity.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageSecurity.Size = new System.Drawing.Size(1054, 469);
            this.tabPageSecurity.TabIndex = 24;
            this.tabPageSecurity.Text = "Security";
            this.tabPageSecurity.UseVisualStyleBackColor = true;
            // 
            // checkBoxEnableLogging
            // 
            this.checkBoxEnableLogging.AutoSize = true;
            this.checkBoxEnableLogging.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxEnableLogging.Location = new System.Drawing.Point(801, 50);
            this.checkBoxEnableLogging.Name = "checkBoxEnableLogging";
            this.checkBoxEnableLogging.Size = new System.Drawing.Size(124, 21);
            this.checkBoxEnableLogging.TabIndex = 48;
            this.checkBoxEnableLogging.Text = "Enable logging";
            this.checkBoxEnableLogging.UseVisualStyleBackColor = true;
            this.checkBoxEnableLogging.CheckedChanged += new System.EventHandler(this.checkBoxEnableLogging_CheckedChanged);
            // 
            // groupBoxSecurityOptions
            // 
            this.groupBoxSecurityOptions.Controls.Add(this.checkBoxEnablePlugins);
            this.groupBoxSecurityOptions.Controls.Add(this.checkBoxEnableLog);
            this.groupBoxSecurityOptions.Controls.Add(this.checkBoxEnableJavaScript);
            this.groupBoxSecurityOptions.Controls.Add(this.checkBoxEnableJava);
            this.groupBoxSecurityOptions.Controls.Add(this.checkBoxEnableBrowsingBackForward);
            this.groupBoxSecurityOptions.Controls.Add(this.checkBoxAllowDownUploads);
            this.groupBoxSecurityOptions.Controls.Add(this.checkBoxAllowFlashFullscreen);
            this.groupBoxSecurityOptions.Controls.Add(this.checkBoxAllowPreferencesWindow);
            this.groupBoxSecurityOptions.Controls.Add(this.checkBoxAllowQuit);
            this.groupBoxSecurityOptions.Controls.Add(this.checkBoxAllowSwitchToApplications);
            this.groupBoxSecurityOptions.Controls.Add(this.checkBoxOpenDownloads);
            this.groupBoxSecurityOptions.Controls.Add(this.checkBoxNewBrowserWindowByScriptBlockForeign);
            this.groupBoxSecurityOptions.Controls.Add(this.checkBoxNewBrowserWindowByLinkBlockForeign);
            this.groupBoxSecurityOptions.Controls.Add(this.checkBoxMonitorProcesses);
            this.groupBoxSecurityOptions.Controls.Add(this.checkBoxIgnoreQuitPassword);
            this.groupBoxSecurityOptions.Controls.Add(this.checkBoxHookMessages);
            this.groupBoxSecurityOptions.Controls.Add(this.checkBoxDownloadPDFFiles);
            this.groupBoxSecurityOptions.Controls.Add(this.checkBoxCreateNewDesktop);
            this.groupBoxSecurityOptions.Controls.Add(this.checkBoxAllowVirtualMachine);
            this.groupBoxSecurityOptions.Controls.Add(this.checkBoxBlockPopupWindows);
            this.groupBoxSecurityOptions.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxSecurityOptions.Location = new System.Drawing.Point(46, 21);
            this.groupBoxSecurityOptions.Name = "groupBoxSecurityOptions";
            this.groupBoxSecurityOptions.Size = new System.Drawing.Size(701, 414);
            this.groupBoxSecurityOptions.TabIndex = 47;
            this.groupBoxSecurityOptions.TabStop = false;
            this.groupBoxSecurityOptions.Text = "Security options";
            // 
            // checkBoxEnablePlugins
            // 
            this.checkBoxEnablePlugins.AutoSize = true;
            this.checkBoxEnablePlugins.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxEnablePlugins.Location = new System.Drawing.Point(267, 137);
            this.checkBoxEnablePlugins.Name = "checkBoxEnablePlugins";
            this.checkBoxEnablePlugins.Size = new System.Drawing.Size(123, 21);
            this.checkBoxEnablePlugins.TabIndex = 62;
            this.checkBoxEnablePlugins.Text = "Enable plugins";
            this.checkBoxEnablePlugins.UseVisualStyleBackColor = true;
            // 
            // checkBoxEnableLog
            // 
            this.checkBoxEnableLog.AutoSize = true;
            this.checkBoxEnableLog.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxEnableLog.Location = new System.Drawing.Point(267, 110);
            this.checkBoxEnableLog.Name = "checkBoxEnableLog";
            this.checkBoxEnableLog.Size = new System.Drawing.Size(97, 21);
            this.checkBoxEnableLog.TabIndex = 61;
            this.checkBoxEnableLog.Text = "Enable log";
            this.checkBoxEnableLog.UseVisualStyleBackColor = true;
            // 
            // checkBoxEnableJavaScript
            // 
            this.checkBoxEnableJavaScript.AutoSize = true;
            this.checkBoxEnableJavaScript.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxEnableJavaScript.Location = new System.Drawing.Point(267, 83);
            this.checkBoxEnableJavaScript.Name = "checkBoxEnableJavaScript";
            this.checkBoxEnableJavaScript.Size = new System.Drawing.Size(144, 21);
            this.checkBoxEnableJavaScript.TabIndex = 60;
            this.checkBoxEnableJavaScript.Text = "Enable JavaScript";
            this.checkBoxEnableJavaScript.UseVisualStyleBackColor = true;
            // 
            // checkBoxEnableJava
            // 
            this.checkBoxEnableJava.AutoSize = true;
            this.checkBoxEnableJava.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxEnableJava.Location = new System.Drawing.Point(267, 56);
            this.checkBoxEnableJava.Name = "checkBoxEnableJava";
            this.checkBoxEnableJava.Size = new System.Drawing.Size(108, 21);
            this.checkBoxEnableJava.TabIndex = 59;
            this.checkBoxEnableJava.Text = "Enable Java";
            this.checkBoxEnableJava.UseVisualStyleBackColor = true;
            // 
            // checkBoxEnableBrowsingBackForward
            // 
            this.checkBoxEnableBrowsingBackForward.AutoSize = true;
            this.checkBoxEnableBrowsingBackForward.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxEnableBrowsingBackForward.Location = new System.Drawing.Point(267, 29);
            this.checkBoxEnableBrowsingBackForward.Name = "checkBoxEnableBrowsingBackForward";
            this.checkBoxEnableBrowsingBackForward.Size = new System.Drawing.Size(219, 21);
            this.checkBoxEnableBrowsingBackForward.TabIndex = 58;
            this.checkBoxEnableBrowsingBackForward.Text = "Enable browsing back/forward";
            this.checkBoxEnableBrowsingBackForward.UseVisualStyleBackColor = true;
            // 
            // checkBoxAllowDownUploads
            // 
            this.checkBoxAllowDownUploads.AutoSize = true;
            this.checkBoxAllowDownUploads.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxAllowDownUploads.Location = new System.Drawing.Point(10, 29);
            this.checkBoxAllowDownUploads.Name = "checkBoxAllowDownUploads";
            this.checkBoxAllowDownUploads.Size = new System.Drawing.Size(153, 21);
            this.checkBoxAllowDownUploads.TabIndex = 57;
            this.checkBoxAllowDownUploads.Text = "Allow down/uploads";
            this.checkBoxAllowDownUploads.UseVisualStyleBackColor = true;
            // 
            // checkBoxAllowFlashFullscreen
            // 
            this.checkBoxAllowFlashFullscreen.AutoSize = true;
            this.checkBoxAllowFlashFullscreen.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxAllowFlashFullscreen.Location = new System.Drawing.Point(10, 56);
            this.checkBoxAllowFlashFullscreen.Name = "checkBoxAllowFlashFullscreen";
            this.checkBoxAllowFlashFullscreen.Size = new System.Drawing.Size(165, 21);
            this.checkBoxAllowFlashFullscreen.TabIndex = 56;
            this.checkBoxAllowFlashFullscreen.Text = "Allow Flash fullscreen";
            this.checkBoxAllowFlashFullscreen.UseVisualStyleBackColor = true;
            // 
            // checkBoxAllowPreferencesWindow
            // 
            this.checkBoxAllowPreferencesWindow.AutoSize = true;
            this.checkBoxAllowPreferencesWindow.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxAllowPreferencesWindow.Location = new System.Drawing.Point(10, 83);
            this.checkBoxAllowPreferencesWindow.Name = "checkBoxAllowPreferencesWindow";
            this.checkBoxAllowPreferencesWindow.Size = new System.Drawing.Size(191, 21);
            this.checkBoxAllowPreferencesWindow.TabIndex = 55;
            this.checkBoxAllowPreferencesWindow.Text = "Allow preferences window";
            this.checkBoxAllowPreferencesWindow.UseVisualStyleBackColor = true;
            // 
            // checkBoxAllowQuit
            // 
            this.checkBoxAllowQuit.AutoSize = true;
            this.checkBoxAllowQuit.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxAllowQuit.Location = new System.Drawing.Point(10, 110);
            this.checkBoxAllowQuit.Name = "checkBoxAllowQuit";
            this.checkBoxAllowQuit.Size = new System.Drawing.Size(89, 21);
            this.checkBoxAllowQuit.TabIndex = 54;
            this.checkBoxAllowQuit.Text = "Allow quit";
            this.checkBoxAllowQuit.UseVisualStyleBackColor = true;
            // 
            // checkBoxAllowSwitchToApplications
            // 
            this.checkBoxAllowSwitchToApplications.AutoSize = true;
            this.checkBoxAllowSwitchToApplications.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxAllowSwitchToApplications.Location = new System.Drawing.Point(10, 137);
            this.checkBoxAllowSwitchToApplications.Name = "checkBoxAllowSwitchToApplications";
            this.checkBoxAllowSwitchToApplications.Size = new System.Drawing.Size(199, 21);
            this.checkBoxAllowSwitchToApplications.TabIndex = 53;
            this.checkBoxAllowSwitchToApplications.Text = "Allow switch to applications";
            this.checkBoxAllowSwitchToApplications.UseVisualStyleBackColor = true;
            // 
            // checkBoxOpenDownloads
            // 
            this.checkBoxOpenDownloads.AutoSize = true;
            this.checkBoxOpenDownloads.Enabled = false;
            this.checkBoxOpenDownloads.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxOpenDownloads.Location = new System.Drawing.Point(267, 360);
            this.checkBoxOpenDownloads.Name = "checkBoxOpenDownloads";
            this.checkBoxOpenDownloads.Size = new System.Drawing.Size(136, 21);
            this.checkBoxOpenDownloads.TabIndex = 52;
            this.checkBoxOpenDownloads.Text = "Open downloads";
            this.checkBoxOpenDownloads.UseVisualStyleBackColor = true;
            // 
            // checkBoxNewBrowserWindowByScriptBlockForeign
            // 
            this.checkBoxNewBrowserWindowByScriptBlockForeign.AutoSize = true;
            this.checkBoxNewBrowserWindowByScriptBlockForeign.Enabled = false;
            this.checkBoxNewBrowserWindowByScriptBlockForeign.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxNewBrowserWindowByScriptBlockForeign.Location = new System.Drawing.Point(267, 333);
            this.checkBoxNewBrowserWindowByScriptBlockForeign.Name = "checkBoxNewBrowserWindowByScriptBlockForeign";
            this.checkBoxNewBrowserWindowByScriptBlockForeign.Size = new System.Drawing.Size(302, 21);
            this.checkBoxNewBrowserWindowByScriptBlockForeign.TabIndex = 51;
            this.checkBoxNewBrowserWindowByScriptBlockForeign.Text = "New browser window by script block foreign";
            this.checkBoxNewBrowserWindowByScriptBlockForeign.UseVisualStyleBackColor = true;
            // 
            // checkBoxNewBrowserWindowByLinkBlockForeign
            // 
            this.checkBoxNewBrowserWindowByLinkBlockForeign.AutoSize = true;
            this.checkBoxNewBrowserWindowByLinkBlockForeign.Enabled = false;
            this.checkBoxNewBrowserWindowByLinkBlockForeign.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxNewBrowserWindowByLinkBlockForeign.Location = new System.Drawing.Point(267, 306);
            this.checkBoxNewBrowserWindowByLinkBlockForeign.Name = "checkBoxNewBrowserWindowByLinkBlockForeign";
            this.checkBoxNewBrowserWindowByLinkBlockForeign.Size = new System.Drawing.Size(289, 21);
            this.checkBoxNewBrowserWindowByLinkBlockForeign.TabIndex = 50;
            this.checkBoxNewBrowserWindowByLinkBlockForeign.Text = "New browser window by link block foreign";
            this.checkBoxNewBrowserWindowByLinkBlockForeign.UseVisualStyleBackColor = true;
            // 
            // checkBoxMonitorProcesses
            // 
            this.checkBoxMonitorProcesses.AutoSize = true;
            this.checkBoxMonitorProcesses.Enabled = false;
            this.checkBoxMonitorProcesses.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxMonitorProcesses.Location = new System.Drawing.Point(267, 279);
            this.checkBoxMonitorProcesses.Name = "checkBoxMonitorProcesses";
            this.checkBoxMonitorProcesses.Size = new System.Drawing.Size(146, 21);
            this.checkBoxMonitorProcesses.TabIndex = 49;
            this.checkBoxMonitorProcesses.Text = "Monitor processes";
            this.checkBoxMonitorProcesses.UseVisualStyleBackColor = true;
            // 
            // checkBoxIgnoreQuitPassword
            // 
            this.checkBoxIgnoreQuitPassword.AutoSize = true;
            this.checkBoxIgnoreQuitPassword.Enabled = false;
            this.checkBoxIgnoreQuitPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxIgnoreQuitPassword.Location = new System.Drawing.Point(267, 252);
            this.checkBoxIgnoreQuitPassword.Name = "checkBoxIgnoreQuitPassword";
            this.checkBoxIgnoreQuitPassword.Size = new System.Drawing.Size(161, 21);
            this.checkBoxIgnoreQuitPassword.TabIndex = 48;
            this.checkBoxIgnoreQuitPassword.Text = "Ignore quit password";
            this.checkBoxIgnoreQuitPassword.UseVisualStyleBackColor = true;
            // 
            // checkBoxHookMessages
            // 
            this.checkBoxHookMessages.AutoSize = true;
            this.checkBoxHookMessages.Enabled = false;
            this.checkBoxHookMessages.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxHookMessages.Location = new System.Drawing.Point(267, 225);
            this.checkBoxHookMessages.Name = "checkBoxHookMessages";
            this.checkBoxHookMessages.Size = new System.Drawing.Size(131, 21);
            this.checkBoxHookMessages.TabIndex = 47;
            this.checkBoxHookMessages.Text = "Hook messages";
            this.checkBoxHookMessages.UseVisualStyleBackColor = true;
            // 
            // checkBoxDownloadPDFFiles
            // 
            this.checkBoxDownloadPDFFiles.AutoSize = true;
            this.checkBoxDownloadPDFFiles.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxDownloadPDFFiles.Location = new System.Drawing.Point(11, 279);
            this.checkBoxDownloadPDFFiles.Name = "checkBoxDownloadPDFFiles";
            this.checkBoxDownloadPDFFiles.Size = new System.Drawing.Size(152, 21);
            this.checkBoxDownloadPDFFiles.TabIndex = 46;
            this.checkBoxDownloadPDFFiles.Text = "Download PDF files";
            this.checkBoxDownloadPDFFiles.UseVisualStyleBackColor = true;
            // 
            // checkBoxCreateNewDesktop
            // 
            this.checkBoxCreateNewDesktop.AutoSize = true;
            this.checkBoxCreateNewDesktop.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxCreateNewDesktop.Location = new System.Drawing.Point(11, 252);
            this.checkBoxCreateNewDesktop.Name = "checkBoxCreateNewDesktop";
            this.checkBoxCreateNewDesktop.Size = new System.Drawing.Size(155, 21);
            this.checkBoxCreateNewDesktop.TabIndex = 45;
            this.checkBoxCreateNewDesktop.Text = "Create new desktop";
            this.checkBoxCreateNewDesktop.UseVisualStyleBackColor = true;
            // 
            // checkBoxAllowVirtualMachine
            // 
            this.checkBoxAllowVirtualMachine.AutoSize = true;
            this.checkBoxAllowVirtualMachine.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxAllowVirtualMachine.Location = new System.Drawing.Point(10, 167);
            this.checkBoxAllowVirtualMachine.Name = "checkBoxAllowVirtualMachine";
            this.checkBoxAllowVirtualMachine.Size = new System.Drawing.Size(161, 21);
            this.checkBoxAllowVirtualMachine.TabIndex = 43;
            this.checkBoxAllowVirtualMachine.Text = "Allow virtual machine";
            this.checkBoxAllowVirtualMachine.UseVisualStyleBackColor = true;
            // 
            // checkBoxBlockPopupWindows
            // 
            this.checkBoxBlockPopupWindows.AutoSize = true;
            this.checkBoxBlockPopupWindows.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxBlockPopupWindows.Location = new System.Drawing.Point(11, 225);
            this.checkBoxBlockPopupWindows.Name = "checkBoxBlockPopupWindows";
            this.checkBoxBlockPopupWindows.Size = new System.Drawing.Size(164, 21);
            this.checkBoxBlockPopupWindows.TabIndex = 44;
            this.checkBoxBlockPopupWindows.Text = "Block popup windows";
            this.checkBoxBlockPopupWindows.UseVisualStyleBackColor = true;
            // 
            // tabPageRegistry
            // 
            this.tabPageRegistry.Controls.Add(this.groupBoxOutsideSeb);
            this.tabPageRegistry.Controls.Add(this.groupBoxSetOutsideSebValues);
            this.tabPageRegistry.Controls.Add(this.groupBoxInsideSeb);
            this.tabPageRegistry.Location = new System.Drawing.Point(4, 39);
            this.tabPageRegistry.Name = "tabPageRegistry";
            this.tabPageRegistry.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageRegistry.Size = new System.Drawing.Size(1054, 469);
            this.tabPageRegistry.TabIndex = 25;
            this.tabPageRegistry.Text = "Registry";
            this.tabPageRegistry.UseVisualStyleBackColor = true;
            // 
            // groupBoxOutsideSeb
            // 
            this.groupBoxOutsideSeb.Controls.Add(this.checkBoxOutsideSebEnableSwitchUser);
            this.groupBoxOutsideSeb.Controls.Add(this.checkBoxOutsideSebEnableLockThisComputer);
            this.groupBoxOutsideSeb.Controls.Add(this.checkBoxOutsideSebEnableChangeAPassword);
            this.groupBoxOutsideSeb.Controls.Add(this.checkBoxOutsideSebEnableStartTaskManager);
            this.groupBoxOutsideSeb.Controls.Add(this.checkBoxOutsideSebEnableLogOff);
            this.groupBoxOutsideSeb.Controls.Add(this.checkBoxOutsideSebEnableShutDown);
            this.groupBoxOutsideSeb.Controls.Add(this.checkBoxOutsideSebEnableEaseOfAccess);
            this.groupBoxOutsideSeb.Controls.Add(this.checkBoxOutsideSebEnableVmWareClientShade);
            this.groupBoxOutsideSeb.Enabled = false;
            this.groupBoxOutsideSeb.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxOutsideSeb.Location = new System.Drawing.Point(302, 35);
            this.groupBoxOutsideSeb.Name = "groupBoxOutsideSeb";
            this.groupBoxOutsideSeb.Size = new System.Drawing.Size(250, 263);
            this.groupBoxOutsideSeb.TabIndex = 32;
            this.groupBoxOutsideSeb.TabStop = false;
            this.groupBoxOutsideSeb.Text = "Outside SEB";
            // 
            // checkBoxOutsideSebEnableSwitchUser
            // 
            this.checkBoxOutsideSebEnableSwitchUser.AutoSize = true;
            this.checkBoxOutsideSebEnableSwitchUser.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxOutsideSebEnableSwitchUser.Location = new System.Drawing.Point(9, 33);
            this.checkBoxOutsideSebEnableSwitchUser.Name = "checkBoxOutsideSebEnableSwitchUser";
            this.checkBoxOutsideSebEnableSwitchUser.Size = new System.Drawing.Size(152, 21);
            this.checkBoxOutsideSebEnableSwitchUser.TabIndex = 0;
            this.checkBoxOutsideSebEnableSwitchUser.Text = "Enable Switch User";
            this.checkBoxOutsideSebEnableSwitchUser.UseVisualStyleBackColor = true;
            // 
            // checkBoxOutsideSebEnableLockThisComputer
            // 
            this.checkBoxOutsideSebEnableLockThisComputer.AutoSize = true;
            this.checkBoxOutsideSebEnableLockThisComputer.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxOutsideSebEnableLockThisComputer.Location = new System.Drawing.Point(9, 60);
            this.checkBoxOutsideSebEnableLockThisComputer.Name = "checkBoxOutsideSebEnableLockThisComputer";
            this.checkBoxOutsideSebEnableLockThisComputer.Size = new System.Drawing.Size(197, 21);
            this.checkBoxOutsideSebEnableLockThisComputer.TabIndex = 1;
            this.checkBoxOutsideSebEnableLockThisComputer.Text = "Enable Lock this computer";
            this.checkBoxOutsideSebEnableLockThisComputer.UseVisualStyleBackColor = true;
            // 
            // checkBoxOutsideSebEnableChangeAPassword
            // 
            this.checkBoxOutsideSebEnableChangeAPassword.AutoSize = true;
            this.checkBoxOutsideSebEnableChangeAPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxOutsideSebEnableChangeAPassword.Location = new System.Drawing.Point(9, 87);
            this.checkBoxOutsideSebEnableChangeAPassword.Name = "checkBoxOutsideSebEnableChangeAPassword";
            this.checkBoxOutsideSebEnableChangeAPassword.Size = new System.Drawing.Size(203, 21);
            this.checkBoxOutsideSebEnableChangeAPassword.TabIndex = 3;
            this.checkBoxOutsideSebEnableChangeAPassword.Text = "Enable Change a password";
            this.checkBoxOutsideSebEnableChangeAPassword.UseVisualStyleBackColor = true;
            // 
            // checkBoxOutsideSebEnableStartTaskManager
            // 
            this.checkBoxOutsideSebEnableStartTaskManager.AutoSize = true;
            this.checkBoxOutsideSebEnableStartTaskManager.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxOutsideSebEnableStartTaskManager.Location = new System.Drawing.Point(9, 114);
            this.checkBoxOutsideSebEnableStartTaskManager.Name = "checkBoxOutsideSebEnableStartTaskManager";
            this.checkBoxOutsideSebEnableStartTaskManager.Size = new System.Drawing.Size(203, 21);
            this.checkBoxOutsideSebEnableStartTaskManager.TabIndex = 2;
            this.checkBoxOutsideSebEnableStartTaskManager.Text = "Enable Start Task Manager";
            this.checkBoxOutsideSebEnableStartTaskManager.UseVisualStyleBackColor = true;
            // 
            // checkBoxOutsideSebEnableLogOff
            // 
            this.checkBoxOutsideSebEnableLogOff.AutoSize = true;
            this.checkBoxOutsideSebEnableLogOff.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxOutsideSebEnableLogOff.Location = new System.Drawing.Point(9, 141);
            this.checkBoxOutsideSebEnableLogOff.Name = "checkBoxOutsideSebEnableLogOff";
            this.checkBoxOutsideSebEnableLogOff.Size = new System.Drawing.Size(122, 21);
            this.checkBoxOutsideSebEnableLogOff.TabIndex = 6;
            this.checkBoxOutsideSebEnableLogOff.Text = "Enable Log off";
            this.checkBoxOutsideSebEnableLogOff.UseVisualStyleBackColor = true;
            // 
            // checkBoxOutsideSebEnableShutDown
            // 
            this.checkBoxOutsideSebEnableShutDown.AutoSize = true;
            this.checkBoxOutsideSebEnableShutDown.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxOutsideSebEnableShutDown.Location = new System.Drawing.Point(9, 168);
            this.checkBoxOutsideSebEnableShutDown.Name = "checkBoxOutsideSebEnableShutDown";
            this.checkBoxOutsideSebEnableShutDown.Size = new System.Drawing.Size(144, 21);
            this.checkBoxOutsideSebEnableShutDown.TabIndex = 4;
            this.checkBoxOutsideSebEnableShutDown.Text = "Enable Shut down";
            this.checkBoxOutsideSebEnableShutDown.UseVisualStyleBackColor = true;
            // 
            // checkBoxOutsideSebEnableEaseOfAccess
            // 
            this.checkBoxOutsideSebEnableEaseOfAccess.AutoSize = true;
            this.checkBoxOutsideSebEnableEaseOfAccess.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxOutsideSebEnableEaseOfAccess.Location = new System.Drawing.Point(9, 195);
            this.checkBoxOutsideSebEnableEaseOfAccess.Name = "checkBoxOutsideSebEnableEaseOfAccess";
            this.checkBoxOutsideSebEnableEaseOfAccess.Size = new System.Drawing.Size(175, 21);
            this.checkBoxOutsideSebEnableEaseOfAccess.TabIndex = 16;
            this.checkBoxOutsideSebEnableEaseOfAccess.Text = "Enable Ease of Access";
            this.checkBoxOutsideSebEnableEaseOfAccess.UseVisualStyleBackColor = true;
            // 
            // checkBoxOutsideSebEnableVmWareClientShade
            // 
            this.checkBoxOutsideSebEnableVmWareClientShade.AutoSize = true;
            this.checkBoxOutsideSebEnableVmWareClientShade.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxOutsideSebEnableVmWareClientShade.Location = new System.Drawing.Point(9, 222);
            this.checkBoxOutsideSebEnableVmWareClientShade.Name = "checkBoxOutsideSebEnableVmWareClientShade";
            this.checkBoxOutsideSebEnableVmWareClientShade.Size = new System.Drawing.Size(212, 21);
            this.checkBoxOutsideSebEnableVmWareClientShade.TabIndex = 7;
            this.checkBoxOutsideSebEnableVmWareClientShade.Text = "Enable VMware Client Shade";
            this.checkBoxOutsideSebEnableVmWareClientShade.UseVisualStyleBackColor = true;
            // 
            // groupBoxSetOutsideSebValues
            // 
            this.groupBoxSetOutsideSebValues.Controls.Add(this.radioButtonInsideValuesManually);
            this.groupBoxSetOutsideSebValues.Controls.Add(this.radioButtonPreviousValuesFromFile);
            this.groupBoxSetOutsideSebValues.Controls.Add(this.radioButtonEnvironmentValues);
            this.groupBoxSetOutsideSebValues.Location = new System.Drawing.Point(302, 322);
            this.groupBoxSetOutsideSebValues.Name = "groupBoxSetOutsideSebValues";
            this.groupBoxSetOutsideSebValues.Size = new System.Drawing.Size(222, 133);
            this.groupBoxSetOutsideSebValues.TabIndex = 31;
            this.groupBoxSetOutsideSebValues.TabStop = false;
            this.groupBoxSetOutsideSebValues.Text = "Set outside SEB values";
            // 
            // radioButtonInsideValuesManually
            // 
            this.radioButtonInsideValuesManually.AutoSize = true;
            this.radioButtonInsideValuesManually.Location = new System.Drawing.Point(8, 89);
            this.radioButtonInsideValuesManually.Name = "radioButtonInsideValuesManually";
            this.radioButtonInsideValuesManually.Size = new System.Drawing.Size(97, 21);
            this.radioButtonInsideValuesManually.TabIndex = 29;
            this.radioButtonInsideValuesManually.Text = "manually...";
            this.radioButtonInsideValuesManually.UseVisualStyleBackColor = true;
            // 
            // radioButtonPreviousValuesFromFile
            // 
            this.radioButtonPreviousValuesFromFile.AutoSize = true;
            this.radioButtonPreviousValuesFromFile.Checked = true;
            this.radioButtonPreviousValuesFromFile.Location = new System.Drawing.Point(9, 35);
            this.radioButtonPreviousValuesFromFile.Name = "radioButtonPreviousValuesFromFile";
            this.radioButtonPreviousValuesFromFile.Size = new System.Drawing.Size(198, 21);
            this.radioButtonPreviousValuesFromFile.TabIndex = 27;
            this.radioButtonPreviousValuesFromFile.TabStop = true;
            this.radioButtonPreviousValuesFromFile.Text = "to previous values from file";
            this.radioButtonPreviousValuesFromFile.UseVisualStyleBackColor = true;
            // 
            // radioButtonEnvironmentValues
            // 
            this.radioButtonEnvironmentValues.AutoSize = true;
            this.radioButtonEnvironmentValues.Location = new System.Drawing.Point(9, 62);
            this.radioButtonEnvironmentValues.Name = "radioButtonEnvironmentValues";
            this.radioButtonEnvironmentValues.Size = new System.Drawing.Size(168, 21);
            this.radioButtonEnvironmentValues.TabIndex = 28;
            this.radioButtonEnvironmentValues.Text = "to environment values";
            this.radioButtonEnvironmentValues.UseVisualStyleBackColor = true;
            // 
            // groupBoxInsideSeb
            // 
            this.groupBoxInsideSeb.Controls.Add(this.checkBoxInsideSebEnableSwitchUser);
            this.groupBoxInsideSeb.Controls.Add(this.checkBoxInsideSebEnableLockThisComputer);
            this.groupBoxInsideSeb.Controls.Add(this.checkBoxInsideSebEnableChangeAPassword);
            this.groupBoxInsideSeb.Controls.Add(this.checkBoxInsideSebEnableStartTaskManager);
            this.groupBoxInsideSeb.Controls.Add(this.checkBoxInsideSebEnableLogOff);
            this.groupBoxInsideSeb.Controls.Add(this.checkBoxInsideSebEnableShutDown);
            this.groupBoxInsideSeb.Controls.Add(this.checkBoxInsideSebEnableEaseOfAccess);
            this.groupBoxInsideSeb.Controls.Add(this.checkBoxInsideSebEnableVmWareClientShade);
            this.groupBoxInsideSeb.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxInsideSeb.Location = new System.Drawing.Point(27, 35);
            this.groupBoxInsideSeb.Name = "groupBoxInsideSeb";
            this.groupBoxInsideSeb.Size = new System.Drawing.Size(250, 263);
            this.groupBoxInsideSeb.TabIndex = 25;
            this.groupBoxInsideSeb.TabStop = false;
            this.groupBoxInsideSeb.Text = "Inside SEB";
            // 
            // checkBoxInsideSebEnableSwitchUser
            // 
            this.checkBoxInsideSebEnableSwitchUser.AutoSize = true;
            this.checkBoxInsideSebEnableSwitchUser.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxInsideSebEnableSwitchUser.Location = new System.Drawing.Point(9, 33);
            this.checkBoxInsideSebEnableSwitchUser.Name = "checkBoxInsideSebEnableSwitchUser";
            this.checkBoxInsideSebEnableSwitchUser.Size = new System.Drawing.Size(152, 21);
            this.checkBoxInsideSebEnableSwitchUser.TabIndex = 0;
            this.checkBoxInsideSebEnableSwitchUser.Text = "Enable Switch User";
            this.checkBoxInsideSebEnableSwitchUser.UseVisualStyleBackColor = true;
            // 
            // checkBoxInsideSebEnableLockThisComputer
            // 
            this.checkBoxInsideSebEnableLockThisComputer.AutoSize = true;
            this.checkBoxInsideSebEnableLockThisComputer.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxInsideSebEnableLockThisComputer.Location = new System.Drawing.Point(9, 60);
            this.checkBoxInsideSebEnableLockThisComputer.Name = "checkBoxInsideSebEnableLockThisComputer";
            this.checkBoxInsideSebEnableLockThisComputer.Size = new System.Drawing.Size(197, 21);
            this.checkBoxInsideSebEnableLockThisComputer.TabIndex = 1;
            this.checkBoxInsideSebEnableLockThisComputer.Text = "Enable Lock this computer";
            this.checkBoxInsideSebEnableLockThisComputer.UseVisualStyleBackColor = true;
            // 
            // checkBoxInsideSebEnableChangeAPassword
            // 
            this.checkBoxInsideSebEnableChangeAPassword.AutoSize = true;
            this.checkBoxInsideSebEnableChangeAPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxInsideSebEnableChangeAPassword.Location = new System.Drawing.Point(9, 87);
            this.checkBoxInsideSebEnableChangeAPassword.Name = "checkBoxInsideSebEnableChangeAPassword";
            this.checkBoxInsideSebEnableChangeAPassword.Size = new System.Drawing.Size(203, 21);
            this.checkBoxInsideSebEnableChangeAPassword.TabIndex = 3;
            this.checkBoxInsideSebEnableChangeAPassword.Text = "Enable Change a password";
            this.checkBoxInsideSebEnableChangeAPassword.UseVisualStyleBackColor = true;
            // 
            // checkBoxInsideSebEnableStartTaskManager
            // 
            this.checkBoxInsideSebEnableStartTaskManager.AutoSize = true;
            this.checkBoxInsideSebEnableStartTaskManager.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxInsideSebEnableStartTaskManager.Location = new System.Drawing.Point(9, 114);
            this.checkBoxInsideSebEnableStartTaskManager.Name = "checkBoxInsideSebEnableStartTaskManager";
            this.checkBoxInsideSebEnableStartTaskManager.Size = new System.Drawing.Size(203, 21);
            this.checkBoxInsideSebEnableStartTaskManager.TabIndex = 2;
            this.checkBoxInsideSebEnableStartTaskManager.Text = "Enable Start Task Manager";
            this.checkBoxInsideSebEnableStartTaskManager.UseVisualStyleBackColor = true;
            // 
            // checkBoxInsideSebEnableLogOff
            // 
            this.checkBoxInsideSebEnableLogOff.AutoSize = true;
            this.checkBoxInsideSebEnableLogOff.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxInsideSebEnableLogOff.Location = new System.Drawing.Point(9, 141);
            this.checkBoxInsideSebEnableLogOff.Name = "checkBoxInsideSebEnableLogOff";
            this.checkBoxInsideSebEnableLogOff.Size = new System.Drawing.Size(122, 21);
            this.checkBoxInsideSebEnableLogOff.TabIndex = 6;
            this.checkBoxInsideSebEnableLogOff.Text = "Enable Log off";
            this.checkBoxInsideSebEnableLogOff.UseVisualStyleBackColor = true;
            // 
            // checkBoxInsideSebEnableShutDown
            // 
            this.checkBoxInsideSebEnableShutDown.AutoSize = true;
            this.checkBoxInsideSebEnableShutDown.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxInsideSebEnableShutDown.Location = new System.Drawing.Point(9, 168);
            this.checkBoxInsideSebEnableShutDown.Name = "checkBoxInsideSebEnableShutDown";
            this.checkBoxInsideSebEnableShutDown.Size = new System.Drawing.Size(144, 21);
            this.checkBoxInsideSebEnableShutDown.TabIndex = 4;
            this.checkBoxInsideSebEnableShutDown.Text = "Enable Shut down";
            this.checkBoxInsideSebEnableShutDown.UseVisualStyleBackColor = true;
            // 
            // checkBoxInsideSebEnableEaseOfAccess
            // 
            this.checkBoxInsideSebEnableEaseOfAccess.AutoSize = true;
            this.checkBoxInsideSebEnableEaseOfAccess.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxInsideSebEnableEaseOfAccess.Location = new System.Drawing.Point(9, 195);
            this.checkBoxInsideSebEnableEaseOfAccess.Name = "checkBoxInsideSebEnableEaseOfAccess";
            this.checkBoxInsideSebEnableEaseOfAccess.Size = new System.Drawing.Size(175, 21);
            this.checkBoxInsideSebEnableEaseOfAccess.TabIndex = 16;
            this.checkBoxInsideSebEnableEaseOfAccess.Text = "Enable Ease of Access";
            this.checkBoxInsideSebEnableEaseOfAccess.UseVisualStyleBackColor = true;
            // 
            // checkBoxInsideSebEnableVmWareClientShade
            // 
            this.checkBoxInsideSebEnableVmWareClientShade.AutoSize = true;
            this.checkBoxInsideSebEnableVmWareClientShade.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxInsideSebEnableVmWareClientShade.Location = new System.Drawing.Point(9, 222);
            this.checkBoxInsideSebEnableVmWareClientShade.Name = "checkBoxInsideSebEnableVmWareClientShade";
            this.checkBoxInsideSebEnableVmWareClientShade.Size = new System.Drawing.Size(212, 21);
            this.checkBoxInsideSebEnableVmWareClientShade.TabIndex = 7;
            this.checkBoxInsideSebEnableVmWareClientShade.Text = "Enable VMware Client Shade";
            this.checkBoxInsideSebEnableVmWareClientShade.UseVisualStyleBackColor = true;
            // 
            // tabPageHookedKeys
            // 
            this.tabPageHookedKeys.Controls.Add(this.groupBox1);
            this.tabPageHookedKeys.Controls.Add(this.groupBox2);
            this.tabPageHookedKeys.Location = new System.Drawing.Point(4, 39);
            this.tabPageHookedKeys.Name = "tabPageHookedKeys";
            this.tabPageHookedKeys.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageHookedKeys.Size = new System.Drawing.Size(1054, 469);
            this.tabPageHookedKeys.TabIndex = 27;
            this.tabPageHookedKeys.Text = "Hooked Keys";
            this.tabPageHookedKeys.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.checkBoxEnableF1);
            this.groupBox1.Controls.Add(this.checkBoxEnableF2);
            this.groupBox1.Controls.Add(this.checkBoxEnableF12);
            this.groupBox1.Controls.Add(this.checkBoxEnableF3);
            this.groupBox1.Controls.Add(this.checkBoxEnableF11);
            this.groupBox1.Controls.Add(this.checkBoxEnableF4);
            this.groupBox1.Controls.Add(this.checkBoxEnableF5);
            this.groupBox1.Controls.Add(this.checkBoxEnableF10);
            this.groupBox1.Controls.Add(this.checkBoxEnableF6);
            this.groupBox1.Controls.Add(this.checkBoxEnableF9);
            this.groupBox1.Controls.Add(this.checkBoxEnableF7);
            this.groupBox1.Controls.Add(this.checkBoxEnableF8);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(244, 19);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(130, 390);
            this.groupBox1.TabIndex = 41;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Function keys";
            // 
            // checkBoxEnableF1
            // 
            this.checkBoxEnableF1.AutoSize = true;
            this.checkBoxEnableF1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxEnableF1.Location = new System.Drawing.Point(10, 30);
            this.checkBoxEnableF1.Name = "checkBoxEnableF1";
            this.checkBoxEnableF1.Size = new System.Drawing.Size(94, 21);
            this.checkBoxEnableF1.TabIndex = 25;
            this.checkBoxEnableF1.Text = "Enable F1";
            this.checkBoxEnableF1.UseVisualStyleBackColor = true;
            // 
            // checkBoxEnableF2
            // 
            this.checkBoxEnableF2.AutoSize = true;
            this.checkBoxEnableF2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxEnableF2.Location = new System.Drawing.Point(10, 60);
            this.checkBoxEnableF2.Name = "checkBoxEnableF2";
            this.checkBoxEnableF2.Size = new System.Drawing.Size(94, 21);
            this.checkBoxEnableF2.TabIndex = 26;
            this.checkBoxEnableF2.Text = "Enable F2";
            this.checkBoxEnableF2.UseVisualStyleBackColor = true;
            // 
            // checkBoxEnableF12
            // 
            this.checkBoxEnableF12.AutoSize = true;
            this.checkBoxEnableF12.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxEnableF12.Location = new System.Drawing.Point(10, 360);
            this.checkBoxEnableF12.Name = "checkBoxEnableF12";
            this.checkBoxEnableF12.Size = new System.Drawing.Size(102, 21);
            this.checkBoxEnableF12.TabIndex = 37;
            this.checkBoxEnableF12.Text = "Enable F12";
            this.checkBoxEnableF12.UseVisualStyleBackColor = true;
            // 
            // checkBoxEnableF3
            // 
            this.checkBoxEnableF3.AutoSize = true;
            this.checkBoxEnableF3.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxEnableF3.Location = new System.Drawing.Point(10, 90);
            this.checkBoxEnableF3.Name = "checkBoxEnableF3";
            this.checkBoxEnableF3.Size = new System.Drawing.Size(94, 21);
            this.checkBoxEnableF3.TabIndex = 27;
            this.checkBoxEnableF3.Text = "Enable F3";
            this.checkBoxEnableF3.UseVisualStyleBackColor = true;
            // 
            // checkBoxEnableF11
            // 
            this.checkBoxEnableF11.AutoSize = true;
            this.checkBoxEnableF11.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxEnableF11.Location = new System.Drawing.Point(10, 330);
            this.checkBoxEnableF11.Name = "checkBoxEnableF11";
            this.checkBoxEnableF11.Size = new System.Drawing.Size(102, 21);
            this.checkBoxEnableF11.TabIndex = 36;
            this.checkBoxEnableF11.Text = "Enable F11";
            this.checkBoxEnableF11.UseVisualStyleBackColor = true;
            // 
            // checkBoxEnableF4
            // 
            this.checkBoxEnableF4.AutoSize = true;
            this.checkBoxEnableF4.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxEnableF4.Location = new System.Drawing.Point(10, 120);
            this.checkBoxEnableF4.Name = "checkBoxEnableF4";
            this.checkBoxEnableF4.Size = new System.Drawing.Size(94, 21);
            this.checkBoxEnableF4.TabIndex = 28;
            this.checkBoxEnableF4.Text = "Enable F4";
            this.checkBoxEnableF4.UseVisualStyleBackColor = true;
            // 
            // checkBoxEnableF5
            // 
            this.checkBoxEnableF5.AutoSize = true;
            this.checkBoxEnableF5.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxEnableF5.Location = new System.Drawing.Point(10, 150);
            this.checkBoxEnableF5.Name = "checkBoxEnableF5";
            this.checkBoxEnableF5.Size = new System.Drawing.Size(94, 21);
            this.checkBoxEnableF5.TabIndex = 29;
            this.checkBoxEnableF5.Text = "Enable F5";
            this.checkBoxEnableF5.UseVisualStyleBackColor = true;
            // 
            // checkBoxEnableF10
            // 
            this.checkBoxEnableF10.AutoSize = true;
            this.checkBoxEnableF10.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxEnableF10.Location = new System.Drawing.Point(10, 300);
            this.checkBoxEnableF10.Name = "checkBoxEnableF10";
            this.checkBoxEnableF10.Size = new System.Drawing.Size(102, 21);
            this.checkBoxEnableF10.TabIndex = 34;
            this.checkBoxEnableF10.Text = "Enable F10";
            this.checkBoxEnableF10.UseVisualStyleBackColor = true;
            // 
            // checkBoxEnableF6
            // 
            this.checkBoxEnableF6.AutoSize = true;
            this.checkBoxEnableF6.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxEnableF6.Location = new System.Drawing.Point(10, 180);
            this.checkBoxEnableF6.Name = "checkBoxEnableF6";
            this.checkBoxEnableF6.Size = new System.Drawing.Size(94, 21);
            this.checkBoxEnableF6.TabIndex = 30;
            this.checkBoxEnableF6.Text = "Enable F6";
            this.checkBoxEnableF6.UseVisualStyleBackColor = true;
            // 
            // checkBoxEnableF9
            // 
            this.checkBoxEnableF9.AutoSize = true;
            this.checkBoxEnableF9.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxEnableF9.Location = new System.Drawing.Point(10, 270);
            this.checkBoxEnableF9.Name = "checkBoxEnableF9";
            this.checkBoxEnableF9.Size = new System.Drawing.Size(94, 21);
            this.checkBoxEnableF9.TabIndex = 33;
            this.checkBoxEnableF9.Text = "Enable F9";
            this.checkBoxEnableF9.UseVisualStyleBackColor = true;
            // 
            // checkBoxEnableF7
            // 
            this.checkBoxEnableF7.AutoSize = true;
            this.checkBoxEnableF7.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxEnableF7.Location = new System.Drawing.Point(10, 210);
            this.checkBoxEnableF7.Name = "checkBoxEnableF7";
            this.checkBoxEnableF7.Size = new System.Drawing.Size(94, 21);
            this.checkBoxEnableF7.TabIndex = 31;
            this.checkBoxEnableF7.Text = "Enable F7";
            this.checkBoxEnableF7.UseVisualStyleBackColor = true;
            // 
            // checkBoxEnableF8
            // 
            this.checkBoxEnableF8.AutoSize = true;
            this.checkBoxEnableF8.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxEnableF8.Location = new System.Drawing.Point(10, 240);
            this.checkBoxEnableF8.Name = "checkBoxEnableF8";
            this.checkBoxEnableF8.Size = new System.Drawing.Size(94, 21);
            this.checkBoxEnableF8.TabIndex = 32;
            this.checkBoxEnableF8.Text = "Enable F8";
            this.checkBoxEnableF8.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.checkBoxEnableEsc);
            this.groupBox2.Controls.Add(this.checkBoxEnableCtrlEsc);
            this.groupBox2.Controls.Add(this.checkBoxEnableAltEsc);
            this.groupBox2.Controls.Add(this.checkBoxEnableAltTab);
            this.groupBox2.Controls.Add(this.checkBoxEnableAltF4);
            this.groupBox2.Controls.Add(this.checkBoxEnableStartMenu);
            this.groupBox2.Controls.Add(this.checkBoxEnableRightMouse);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(20, 19);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(170, 240);
            this.groupBox2.TabIndex = 39;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Special keys";
            // 
            // checkBoxEnableEsc
            // 
            this.checkBoxEnableEsc.AutoSize = true;
            this.checkBoxEnableEsc.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxEnableEsc.Location = new System.Drawing.Point(10, 30);
            this.checkBoxEnableEsc.Name = "checkBoxEnableEsc";
            this.checkBoxEnableEsc.Size = new System.Drawing.Size(101, 21);
            this.checkBoxEnableEsc.TabIndex = 41;
            this.checkBoxEnableEsc.Text = "Enable Esc";
            this.checkBoxEnableEsc.UseVisualStyleBackColor = true;
            // 
            // checkBoxEnableCtrlEsc
            // 
            this.checkBoxEnableCtrlEsc.AutoSize = true;
            this.checkBoxEnableCtrlEsc.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxEnableCtrlEsc.Location = new System.Drawing.Point(10, 60);
            this.checkBoxEnableCtrlEsc.Name = "checkBoxEnableCtrlEsc";
            this.checkBoxEnableCtrlEsc.Size = new System.Drawing.Size(127, 21);
            this.checkBoxEnableCtrlEsc.TabIndex = 19;
            this.checkBoxEnableCtrlEsc.Text = "Enable Ctrl-Esc";
            this.checkBoxEnableCtrlEsc.UseVisualStyleBackColor = true;
            // 
            // checkBoxEnableAltEsc
            // 
            this.checkBoxEnableAltEsc.AutoSize = true;
            this.checkBoxEnableAltEsc.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxEnableAltEsc.Location = new System.Drawing.Point(10, 90);
            this.checkBoxEnableAltEsc.Name = "checkBoxEnableAltEsc";
            this.checkBoxEnableAltEsc.Size = new System.Drawing.Size(122, 21);
            this.checkBoxEnableAltEsc.TabIndex = 20;
            this.checkBoxEnableAltEsc.Text = "Enable Alt-Esc";
            this.checkBoxEnableAltEsc.UseVisualStyleBackColor = true;
            // 
            // checkBoxEnableAltTab
            // 
            this.checkBoxEnableAltTab.AutoSize = true;
            this.checkBoxEnableAltTab.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxEnableAltTab.Location = new System.Drawing.Point(10, 120);
            this.checkBoxEnableAltTab.Name = "checkBoxEnableAltTab";
            this.checkBoxEnableAltTab.Size = new System.Drawing.Size(124, 21);
            this.checkBoxEnableAltTab.TabIndex = 21;
            this.checkBoxEnableAltTab.Text = "Enable Alt-Tab";
            this.checkBoxEnableAltTab.UseVisualStyleBackColor = true;
            // 
            // checkBoxEnableAltF4
            // 
            this.checkBoxEnableAltF4.AutoSize = true;
            this.checkBoxEnableAltF4.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxEnableAltF4.Location = new System.Drawing.Point(10, 150);
            this.checkBoxEnableAltF4.Name = "checkBoxEnableAltF4";
            this.checkBoxEnableAltF4.Size = new System.Drawing.Size(115, 21);
            this.checkBoxEnableAltF4.TabIndex = 22;
            this.checkBoxEnableAltF4.Text = "Enable Alt-F4";
            this.checkBoxEnableAltF4.UseVisualStyleBackColor = true;
            // 
            // checkBoxEnableStartMenu
            // 
            this.checkBoxEnableStartMenu.AutoSize = true;
            this.checkBoxEnableStartMenu.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxEnableStartMenu.Location = new System.Drawing.Point(10, 180);
            this.checkBoxEnableStartMenu.Name = "checkBoxEnableStartMenu";
            this.checkBoxEnableStartMenu.Size = new System.Drawing.Size(147, 21);
            this.checkBoxEnableStartMenu.TabIndex = 23;
            this.checkBoxEnableStartMenu.Text = "Enable Start Menu";
            this.checkBoxEnableStartMenu.UseVisualStyleBackColor = true;
            // 
            // checkBoxEnableRightMouse
            // 
            this.checkBoxEnableRightMouse.AutoSize = true;
            this.checkBoxEnableRightMouse.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxEnableRightMouse.Location = new System.Drawing.Point(10, 210);
            this.checkBoxEnableRightMouse.Name = "checkBoxEnableRightMouse";
            this.checkBoxEnableRightMouse.Size = new System.Drawing.Size(157, 21);
            this.checkBoxEnableRightMouse.TabIndex = 24;
            this.checkBoxEnableRightMouse.Text = "Enable Right Mouse";
            this.checkBoxEnableRightMouse.UseVisualStyleBackColor = true;
            // 
            // tabPageExitKeys
            // 
            this.tabPageExitKeys.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("tabPageExitKeys.BackgroundImage")));
            this.tabPageExitKeys.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.tabPageExitKeys.Controls.Add(this.numericUpDown2);
            this.tabPageExitKeys.Controls.Add(this.groupBoxExitSequence);
            this.tabPageExitKeys.Location = new System.Drawing.Point(4, 39);
            this.tabPageExitKeys.Name = "tabPageExitKeys";
            this.tabPageExitKeys.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageExitKeys.Size = new System.Drawing.Size(1054, 469);
            this.tabPageExitKeys.TabIndex = 28;
            this.tabPageExitKeys.Text = "Exit Keys";
            this.tabPageExitKeys.UseVisualStyleBackColor = true;
            // 
            // groupBoxExitSequence
            // 
            this.groupBoxExitSequence.Controls.Add(this.listBoxExitKey1);
            this.groupBoxExitSequence.Controls.Add(this.listBoxExitKey3);
            this.groupBoxExitSequence.Controls.Add(this.listBoxExitKey2);
            this.groupBoxExitSequence.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxExitSequence.Location = new System.Drawing.Point(21, 22);
            this.groupBoxExitSequence.Name = "groupBoxExitSequence";
            this.groupBoxExitSequence.Size = new System.Drawing.Size(160, 240);
            this.groupBoxExitSequence.TabIndex = 52;
            this.groupBoxExitSequence.TabStop = false;
            this.groupBoxExitSequence.Text = "Exit sequence";
            // 
            // listBoxExitKey1
            // 
            this.listBoxExitKey1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
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
            // 
            // listBoxExitKey3
            // 
            this.listBoxExitKey3.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
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
            // 
            // listBoxExitKey2
            // 
            this.listBoxExitKey2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
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
            // 
            // numericUpDown2
            // 
            this.numericUpDown2.Location = new System.Drawing.Point(715, 190);
            this.numericUpDown2.Name = "numericUpDown2";
            this.numericUpDown2.Size = new System.Drawing.Size(120, 22);
            this.numericUpDown2.TabIndex = 53;
            // 
            // SebWindowsConfigForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1323, 633);
            this.Controls.Add(this.tabControlSebWindowsConfig);
            this.Name = "SebWindowsConfigForm";
            this.Text = "SEB Windows Configuration Editor";
            this.tabPageConfigFile.ResumeLayout(false);
            this.tabPageConfigFile.PerformLayout();
            this.tabPageGeneral.ResumeLayout(false);
            this.tabPageGeneral.PerformLayout();
            this.tabControlSebWindowsConfig.ResumeLayout(false);
            this.tabPageExam.ResumeLayout(false);
            this.groupBoxOnlineExam.ResumeLayout(false);
            this.groupBoxOnlineExam.PerformLayout();
            this.tabPageSecurity.ResumeLayout(false);
            this.tabPageSecurity.PerformLayout();
            this.groupBoxSecurityOptions.ResumeLayout(false);
            this.groupBoxSecurityOptions.PerformLayout();
            this.tabPageRegistry.ResumeLayout(false);
            this.groupBoxOutsideSeb.ResumeLayout(false);
            this.groupBoxOutsideSeb.PerformLayout();
            this.groupBoxSetOutsideSebValues.ResumeLayout(false);
            this.groupBoxSetOutsideSebValues.PerformLayout();
            this.groupBoxInsideSeb.ResumeLayout(false);
            this.groupBoxInsideSeb.PerformLayout();
            this.tabPageHookedKeys.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.tabPageExitKeys.ResumeLayout(false);
            this.groupBoxExitSequence.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion


        private System.Windows.Forms.OpenFileDialog openFileDialogSebStarterIni;
        private System.Windows.Forms.SaveFileDialog saveFileDialogSebStarterIni;
        private System.Windows.Forms.ImageList imageListTabIcons;
        private System.Windows.Forms.TabPage tabPageBrowser;
        private System.Windows.Forms.TabPage tabPageAppearance;
        private System.Windows.Forms.TabPage tabPageConfigFile;
        private System.Windows.Forms.Button buttonDefaultSettings;
        private System.Windows.Forms.Label labelOpenSettings;
        private System.Windows.Forms.Label labelSaveSettingsAs;
        private System.Windows.Forms.Button buttonRevertToLastOpened;
        private System.Windows.Forms.TabPage tabPageGeneral;
        private System.Windows.Forms.Button buttonHelp;
        private System.Windows.Forms.Button buttonRestartSEB;
        private System.Windows.Forms.Button buttonQuit;
        private System.Windows.Forms.Button buttonAbout;
        private System.Windows.Forms.TextBox textBoxConfirmAdministratorPassword;
        private System.Windows.Forms.TextBox textBoxAdministratorPassword;
        private System.Windows.Forms.TextBox textBoxConfirmQuitPassword;
        private System.Windows.Forms.TextBox textBoxQuitHashcode;
        private System.Windows.Forms.TextBox textBoxQuitPassword;
        private System.Windows.Forms.TextBox textBoxStartURL;
        private System.Windows.Forms.Label labelConfirmAdministratorPassword;
        private System.Windows.Forms.Label labelAdministratorPassword;
        private System.Windows.Forms.Label labelConfirmQuitPassword;
        private System.Windows.Forms.CheckBox checkBoxAllowUserToQuitSEB;
        private System.Windows.Forms.Label labelQuitHashCode;
        private System.Windows.Forms.Label labelQuitPassword;
        private System.Windows.Forms.Label labelStartURL;
        private System.Windows.Forms.TabControl tabControlSebWindowsConfig;
        private System.Windows.Forms.TabPage tabPageDownUploads;
        private System.Windows.Forms.TabPage tabPageExam;
        private System.Windows.Forms.GroupBox groupBoxOnlineExam;
        private System.Windows.Forms.TextBox textBoxAutostartProcess;
        private System.Windows.Forms.Label labelSebBrowser;
        private System.Windows.Forms.Label labelAutostartProcess;
        private System.Windows.Forms.TextBox textBoxSebBrowser;
        private System.Windows.Forms.Label labelPermittedApplications;
        private System.Windows.Forms.TextBox textBoxPermittedApplications;
        private System.Windows.Forms.TabPage tabPageApplications;
        private System.Windows.Forms.TabPage tabPageNetwork;
        private System.Windows.Forms.TabPage tabPageSecurity;
        private System.Windows.Forms.TabPage tabPageRegistry;
        private System.Windows.Forms.GroupBox groupBoxOutsideSeb;
        private System.Windows.Forms.CheckBox checkBoxOutsideSebEnableSwitchUser;
        private System.Windows.Forms.CheckBox checkBoxOutsideSebEnableLockThisComputer;
        private System.Windows.Forms.CheckBox checkBoxOutsideSebEnableChangeAPassword;
        private System.Windows.Forms.CheckBox checkBoxOutsideSebEnableStartTaskManager;
        private System.Windows.Forms.CheckBox checkBoxOutsideSebEnableLogOff;
        private System.Windows.Forms.CheckBox checkBoxOutsideSebEnableShutDown;
        private System.Windows.Forms.CheckBox checkBoxOutsideSebEnableEaseOfAccess;
        private System.Windows.Forms.CheckBox checkBoxOutsideSebEnableVmWareClientShade;
        private System.Windows.Forms.GroupBox groupBoxSetOutsideSebValues;
        private System.Windows.Forms.RadioButton radioButtonInsideValuesManually;
        private System.Windows.Forms.RadioButton radioButtonPreviousValuesFromFile;
        private System.Windows.Forms.RadioButton radioButtonEnvironmentValues;
        private System.Windows.Forms.GroupBox groupBoxInsideSeb;
        private System.Windows.Forms.CheckBox checkBoxInsideSebEnableSwitchUser;
        private System.Windows.Forms.CheckBox checkBoxInsideSebEnableLockThisComputer;
        private System.Windows.Forms.CheckBox checkBoxInsideSebEnableChangeAPassword;
        private System.Windows.Forms.CheckBox checkBoxInsideSebEnableStartTaskManager;
        private System.Windows.Forms.CheckBox checkBoxInsideSebEnableLogOff;
        private System.Windows.Forms.CheckBox checkBoxInsideSebEnableShutDown;
        private System.Windows.Forms.CheckBox checkBoxInsideSebEnableEaseOfAccess;
        private System.Windows.Forms.CheckBox checkBoxInsideSebEnableVmWareClientShade;
        private System.Windows.Forms.TabPage tabPageHookedKeys;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox checkBoxEnableF1;
        private System.Windows.Forms.CheckBox checkBoxEnableF2;
        private System.Windows.Forms.CheckBox checkBoxEnableF12;
        private System.Windows.Forms.CheckBox checkBoxEnableF3;
        private System.Windows.Forms.CheckBox checkBoxEnableF11;
        private System.Windows.Forms.CheckBox checkBoxEnableF4;
        private System.Windows.Forms.CheckBox checkBoxEnableF5;
        private System.Windows.Forms.CheckBox checkBoxEnableF10;
        private System.Windows.Forms.CheckBox checkBoxEnableF6;
        private System.Windows.Forms.CheckBox checkBoxEnableF9;
        private System.Windows.Forms.CheckBox checkBoxEnableF7;
        private System.Windows.Forms.CheckBox checkBoxEnableF8;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox checkBoxEnableEsc;
        private System.Windows.Forms.CheckBox checkBoxEnableCtrlEsc;
        private System.Windows.Forms.CheckBox checkBoxEnableAltEsc;
        private System.Windows.Forms.CheckBox checkBoxEnableAltTab;
        private System.Windows.Forms.CheckBox checkBoxEnableAltF4;
        private System.Windows.Forms.CheckBox checkBoxEnableStartMenu;
        private System.Windows.Forms.CheckBox checkBoxEnableRightMouse;
        private System.Windows.Forms.TabPage tabPageExitKeys;
        private System.Windows.Forms.GroupBox groupBoxExitSequence;
        private System.Windows.Forms.ListBox listBoxExitKey1;
        private System.Windows.Forms.ListBox listBoxExitKey3;
        private System.Windows.Forms.ListBox listBoxExitKey2;
        private System.Windows.Forms.GroupBox groupBoxSecurityOptions;
        private System.Windows.Forms.CheckBox checkBoxEnablePlugins;
        private System.Windows.Forms.CheckBox checkBoxEnableLog;
        private System.Windows.Forms.CheckBox checkBoxEnableJavaScript;
        private System.Windows.Forms.CheckBox checkBoxEnableJava;
        private System.Windows.Forms.CheckBox checkBoxEnableBrowsingBackForward;
        private System.Windows.Forms.CheckBox checkBoxAllowDownUploads;
        private System.Windows.Forms.CheckBox checkBoxAllowFlashFullscreen;
        private System.Windows.Forms.CheckBox checkBoxAllowPreferencesWindow;
        private System.Windows.Forms.CheckBox checkBoxAllowQuit;
        private System.Windows.Forms.CheckBox checkBoxAllowSwitchToApplications;
        private System.Windows.Forms.CheckBox checkBoxOpenDownloads;
        private System.Windows.Forms.CheckBox checkBoxNewBrowserWindowByScriptBlockForeign;
        private System.Windows.Forms.CheckBox checkBoxNewBrowserWindowByLinkBlockForeign;
        private System.Windows.Forms.CheckBox checkBoxMonitorProcesses;
        private System.Windows.Forms.CheckBox checkBoxIgnoreQuitPassword;
        private System.Windows.Forms.CheckBox checkBoxHookMessages;
        private System.Windows.Forms.CheckBox checkBoxDownloadPDFFiles;
        private System.Windows.Forms.CheckBox checkBoxCreateNewDesktop;
        private System.Windows.Forms.CheckBox checkBoxAllowVirtualMachine;
        private System.Windows.Forms.CheckBox checkBoxBlockPopupWindows;
        private System.Windows.Forms.Label labelSEBServerURL;
        private System.Windows.Forms.TextBox textBoxSEBServerURL;
        private System.Windows.Forms.CheckBox checkBoxEnableLogging;
        private System.Windows.Forms.CheckBox checkBoxAllowToOpenPreferencesWindow;
        private System.Windows.Forms.RadioButton radioButtonConfiguringAClient;
        private System.Windows.Forms.RadioButton radioButtonStartingAnExam;
        private System.Windows.Forms.Label labelUseSEBSettingsFileFor;
        private System.Windows.Forms.Label labelConfirmSettingsPassword;
        private System.Windows.Forms.Label labelSettingsPassword;
        private System.Windows.Forms.TextBox textBoxConfirmSettingsPassword;
        private System.Windows.Forms.TextBox textBoxSettingsPassword;
        private System.Windows.Forms.Label labelChooseIdentity;
        private System.Windows.Forms.ComboBox comboBoxChooseIdentity;
        private System.Windows.Forms.Label labelUseEither;
        private System.Windows.Forms.NumericUpDown numericUpDown2;

    }
}

