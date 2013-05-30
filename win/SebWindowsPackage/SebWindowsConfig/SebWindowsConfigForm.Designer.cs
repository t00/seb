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
            this.openFileDialogSebConfigFile = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialogSebConfigFile = new System.Windows.Forms.SaveFileDialog();
            this.imageListTabIcons = new System.Windows.Forms.ImageList(this.components);
            this.folderBrowserDialogDownloadDirectoryWin = new System.Windows.Forms.FolderBrowserDialog();
            this.tabPageHookedKeys = new System.Windows.Forms.TabPage();
            this.checkBoxHookKeys = new System.Windows.Forms.CheckBox();
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
            this.tabPageSecurity = new System.Windows.Forms.TabPage();
            this.labelLogDirectoryWin = new System.Windows.Forms.Label();
            this.buttonLogDirectoryWin = new System.Windows.Forms.Button();
            this.checkBoxCreateNewDesktop = new System.Windows.Forms.CheckBox();
            this.checkBoxAllowUserSwitching = new System.Windows.Forms.CheckBox();
            this.labelSebServicePolicy = new System.Windows.Forms.Label();
            this.listBoxSebServicePolicy = new System.Windows.Forms.ListBox();
            this.checkBoxEnableLogging = new System.Windows.Forms.CheckBox();
            this.checkBoxAllowVirtualMachine = new System.Windows.Forms.CheckBox();
            this.tabPageNetwork = new System.Windows.Forms.TabPage();
            this.tabControlNetwork = new System.Windows.Forms.TabControl();
            this.tabPageFilter = new System.Windows.Forms.TabPage();
            this.tabPageCertificates = new System.Windows.Forms.TabPage();
            this.tabPageProxies = new System.Windows.Forms.TabPage();
            this.tabPageApplications = new System.Windows.Forms.TabPage();
            this.tabControlApplications = new System.Windows.Forms.TabControl();
            this.tabPagePermittedProcesses = new System.Windows.Forms.TabPage();
            this.buttonChoosePermittedProcess = new System.Windows.Forms.Button();
            this.buttonChoosePermittedApplication = new System.Windows.Forms.Button();
            this.buttonRemovePermittedProcess = new System.Windows.Forms.Button();
            this.buttonAddPermittedProcess = new System.Windows.Forms.Button();
            this.listViewPermittedProcesses = new System.Windows.Forms.ListView();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.checkedListBoxPermitted = new System.Windows.Forms.CheckedListBox();
            this.groupBoxSelectedProcess = new System.Windows.Forms.GroupBox();
            this.labelPermittedProcessIdentifier = new System.Windows.Forms.Label();
            this.textBoxPermittedProcessIdentifier = new System.Windows.Forms.TextBox();
            this.checkedListBoxPermittedProcessArguments = new System.Windows.Forms.CheckedListBox();
            this.buttonPermittedProcessRemoveArgument = new System.Windows.Forms.Button();
            this.buttonPermittedProcessAddArgument = new System.Windows.Forms.Button();
            this.labelPermittedProcessArguments = new System.Windows.Forms.Label();
            this.labelPermittedProcessOS = new System.Windows.Forms.Label();
            this.listBoxPermittedProcessOS = new System.Windows.Forms.ListBox();
            this.labelPermittedProcessExecutable = new System.Windows.Forms.Label();
            this.labelPermittedProcessPath = new System.Windows.Forms.Label();
            this.textBoxPermittedProcessPath = new System.Windows.Forms.TextBox();
            this.textBoxPermittedProcessExecutable = new System.Windows.Forms.TextBox();
            this.textBoxPermittedProcessDescription = new System.Windows.Forms.TextBox();
            this.labelPermittedProcessDescription = new System.Windows.Forms.Label();
            this.labelPermittedProcessAppTitle = new System.Windows.Forms.Label();
            this.textBoxPermittedProcessAppTitle = new System.Windows.Forms.TextBox();
            this.checkBoxPermittedProcessAllowUser = new System.Windows.Forms.CheckBox();
            this.checkBoxPermittedProcessAutohide = new System.Windows.Forms.CheckBox();
            this.checkBoxPermittedProcessAutostart = new System.Windows.Forms.CheckBox();
            this.checkBoxPermittedProcessActive = new System.Windows.Forms.CheckBox();
            this.checkBoxAllowSwitchToApplications = new System.Windows.Forms.CheckBox();
            this.checkBoxAllowFlashFullscreen = new System.Windows.Forms.CheckBox();
            this.tabPageProhibitedProcesses = new System.Windows.Forms.TabPage();
            this.checkBoxMonitorProcesses = new System.Windows.Forms.CheckBox();
            this.tabPageExam = new System.Windows.Forms.TabPage();
            this.labelPlaceThisQuitLink = new System.Windows.Forms.Label();
            this.labelCopyBrowserExamKey = new System.Windows.Forms.Label();
            this.buttonGenerateBrowserExamKey = new System.Windows.Forms.Button();
            this.labelBrowserExamKey = new System.Windows.Forms.Label();
            this.textBoxBrowserExamKey = new System.Windows.Forms.TextBox();
            this.textBoxQuitURL = new System.Windows.Forms.TextBox();
            this.labelQuitURL = new System.Windows.Forms.Label();
            this.checkBoxSendBrowserExamKey = new System.Windows.Forms.CheckBox();
            this.checkBoxCopyBrowserExamKey = new System.Windows.Forms.CheckBox();
            this.tabPageDownUploads = new System.Windows.Forms.TabPage();
            this.labelDownloadDirectoryWin = new System.Windows.Forms.Label();
            this.buttonDownloadDirectoryWin = new System.Windows.Forms.Button();
            this.listBoxChooseFileToUploadPolicy = new System.Windows.Forms.ListBox();
            this.labelChooseFileToUploadPolicy = new System.Windows.Forms.Label();
            this.checkBoxDownloadPDFFiles = new System.Windows.Forms.CheckBox();
            this.checkBoxOpenDownloads = new System.Windows.Forms.CheckBox();
            this.checkBoxAllowDownUploads = new System.Windows.Forms.CheckBox();
            this.tabPageBrowser = new System.Windows.Forms.TabPage();
            this.listBoxOpenLinksJava = new System.Windows.Forms.ListBox();
            this.listBoxOpenLinksHTML = new System.Windows.Forms.ListBox();
            this.labelUseSEBWithoutBrowser = new System.Windows.Forms.Label();
            this.checkBoxBlockPopUpWindows = new System.Windows.Forms.CheckBox();
            this.checkBoxAllowBrowsingBackForward = new System.Windows.Forms.CheckBox();
            this.checkBoxEnableJavaScript = new System.Windows.Forms.CheckBox();
            this.checkBoxEnableJava = new System.Windows.Forms.CheckBox();
            this.checkBoxEnablePlugIns = new System.Windows.Forms.CheckBox();
            this.checkBoxUseSebWithoutBrowser = new System.Windows.Forms.CheckBox();
            this.checkBoxBlockLinksJava = new System.Windows.Forms.CheckBox();
            this.labelOpenLinksJava = new System.Windows.Forms.Label();
            this.labelOpenLinksHTML = new System.Windows.Forms.Label();
            this.checkBoxBlockLinksHTML = new System.Windows.Forms.CheckBox();
            this.groupBoxNewBrowserWindow = new System.Windows.Forms.GroupBox();
            this.comboBoxNewBrowserWindowHeight = new System.Windows.Forms.ComboBox();
            this.comboBoxNewBrowserWindowWidth = new System.Windows.Forms.ComboBox();
            this.labelNewWindowHeight = new System.Windows.Forms.Label();
            this.labelNewWindowWidth = new System.Windows.Forms.Label();
            this.labelNewWindowPosition = new System.Windows.Forms.Label();
            this.listBoxNewBrowserWindowPositioning = new System.Windows.Forms.ListBox();
            this.tabPageAppearance = new System.Windows.Forms.TabPage();
            this.groupBoxMainBrowserWindow = new System.Windows.Forms.GroupBox();
            this.comboBoxMainBrowserWindowHeight = new System.Windows.Forms.ComboBox();
            this.comboBoxMainBrowserWindowWidth = new System.Windows.Forms.ComboBox();
            this.labelMainWindowHeight = new System.Windows.Forms.Label();
            this.labelMainWindowWidth = new System.Windows.Forms.Label();
            this.labelMainWindowPosition = new System.Windows.Forms.Label();
            this.listBoxMainBrowserWindowPositioning = new System.Windows.Forms.ListBox();
            this.checkBoxShowTaskBar = new System.Windows.Forms.CheckBox();
            this.checkBoxShowMenuBar = new System.Windows.Forms.CheckBox();
            this.checkBoxHideBrowserWindowToolbar = new System.Windows.Forms.CheckBox();
            this.checkBoxEnableBrowserWindowToolbar = new System.Windows.Forms.CheckBox();
            this.radioButtonUseFullScreenMode = new System.Windows.Forms.RadioButton();
            this.radioButtonUseBrowserWindow = new System.Windows.Forms.RadioButton();
            this.tabPageConfigFile = new System.Windows.Forms.TabPage();
            this.textBoxHashedSettingsPassword = new System.Windows.Forms.TextBox();
            this.labelHashedSettingsPassword = new System.Windows.Forms.Label();
            this.labelUseEither = new System.Windows.Forms.Label();
            this.labelChooseIdentity = new System.Windows.Forms.Label();
            this.comboBoxCryptoIdentity = new System.Windows.Forms.ComboBox();
            this.labelConfirmSettingsPassword = new System.Windows.Forms.Label();
            this.labelSettingsPassword = new System.Windows.Forms.Label();
            this.textBoxConfirmSettingsPassword = new System.Windows.Forms.TextBox();
            this.textBoxSettingsPassword = new System.Windows.Forms.TextBox();
            this.labelUseSEBSettingsFileFor = new System.Windows.Forms.Label();
            this.radioButtonConfiguringAClient = new System.Windows.Forms.RadioButton();
            this.radioButtonStartingAnExam = new System.Windows.Forms.RadioButton();
            this.checkBoxAllowPreferencesWindow = new System.Windows.Forms.CheckBox();
            this.buttonDefaultSettings = new System.Windows.Forms.Button();
            this.buttonRevertToLastOpened = new System.Windows.Forms.Button();
            this.labelOpenSettings = new System.Windows.Forms.Label();
            this.labelSaveSettingsAs = new System.Windows.Forms.Label();
            this.tabPageGeneral = new System.Windows.Forms.TabPage();
            this.labelHashedAdminPassword = new System.Windows.Forms.Label();
            this.textBoxHashedAdminPassword = new System.Windows.Forms.TextBox();
            this.groupBoxExitSequence = new System.Windows.Forms.GroupBox();
            this.listBoxExitKey1 = new System.Windows.Forms.ListBox();
            this.listBoxExitKey3 = new System.Windows.Forms.ListBox();
            this.listBoxExitKey2 = new System.Windows.Forms.ListBox();
            this.checkBoxIgnoreQuitPassword = new System.Windows.Forms.CheckBox();
            this.buttonPasteFromSavedClipboard = new System.Windows.Forms.Button();
            this.labelSEBServerURL = new System.Windows.Forms.Label();
            this.textBoxSebServerURL = new System.Windows.Forms.TextBox();
            this.textBoxConfirmAdminPassword = new System.Windows.Forms.TextBox();
            this.textBoxAdminPassword = new System.Windows.Forms.TextBox();
            this.textBoxConfirmQuitPassword = new System.Windows.Forms.TextBox();
            this.textBoxHashedQuitPassword = new System.Windows.Forms.TextBox();
            this.textBoxQuitPassword = new System.Windows.Forms.TextBox();
            this.textBoxStartURL = new System.Windows.Forms.TextBox();
            this.buttonHelp = new System.Windows.Forms.Button();
            this.buttonRestartSEB = new System.Windows.Forms.Button();
            this.buttonQuit = new System.Windows.Forms.Button();
            this.buttonAbout = new System.Windows.Forms.Button();
            this.labelConfirmAdminPassword = new System.Windows.Forms.Label();
            this.labelAdminPassword = new System.Windows.Forms.Label();
            this.labelConfirmQuitPassword = new System.Windows.Forms.Label();
            this.checkBoxAllowQuit = new System.Windows.Forms.CheckBox();
            this.labelHashedQuitPassword = new System.Windows.Forms.Label();
            this.labelQuitPassword = new System.Windows.Forms.Label();
            this.labelStartURL = new System.Windows.Forms.Label();
            this.tabControlSebWindowsConfig = new System.Windows.Forms.TabControl();
            this.folderBrowserDialogLogDirectoryWin = new System.Windows.Forms.FolderBrowserDialog();
            this.tabPageHookedKeys.SuspendLayout();
            this.groupBoxFunctionKeys.SuspendLayout();
            this.groupBoxSpecialKeys.SuspendLayout();
            this.tabPageRegistry.SuspendLayout();
            this.groupBoxOutsideSeb.SuspendLayout();
            this.groupBoxSetOutsideSebValues.SuspendLayout();
            this.groupBoxInsideSeb.SuspendLayout();
            this.tabPageSecurity.SuspendLayout();
            this.tabPageNetwork.SuspendLayout();
            this.tabControlNetwork.SuspendLayout();
            this.tabPageApplications.SuspendLayout();
            this.tabControlApplications.SuspendLayout();
            this.tabPagePermittedProcesses.SuspendLayout();
            this.groupBoxSelectedProcess.SuspendLayout();
            this.tabPageExam.SuspendLayout();
            this.tabPageDownUploads.SuspendLayout();
            this.tabPageBrowser.SuspendLayout();
            this.groupBoxNewBrowserWindow.SuspendLayout();
            this.tabPageAppearance.SuspendLayout();
            this.groupBoxMainBrowserWindow.SuspendLayout();
            this.tabPageConfigFile.SuspendLayout();
            this.tabPageGeneral.SuspendLayout();
            this.groupBoxExitSequence.SuspendLayout();
            this.tabControlSebWindowsConfig.SuspendLayout();
            this.SuspendLayout();
            // 
            // openFileDialogSebConfigFile
            // 
            this.openFileDialogSebConfigFile.DefaultExt = "seb";
            this.openFileDialogSebConfigFile.Title = "Open SEB config file";
            // 
            // saveFileDialogSebConfigFile
            // 
            this.saveFileDialogSebConfigFile.DefaultExt = "seb";
            this.saveFileDialogSebConfigFile.Title = "Save SEB config file";
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
            // tabPageHookedKeys
            // 
            this.tabPageHookedKeys.Controls.Add(this.checkBoxHookKeys);
            this.tabPageHookedKeys.Controls.Add(this.groupBoxFunctionKeys);
            this.tabPageHookedKeys.Controls.Add(this.groupBoxSpecialKeys);
            this.tabPageHookedKeys.Location = new System.Drawing.Point(4, 39);
            this.tabPageHookedKeys.Name = "tabPageHookedKeys";
            this.tabPageHookedKeys.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageHookedKeys.Size = new System.Drawing.Size(1092, 857);
            this.tabPageHookedKeys.TabIndex = 27;
            this.tabPageHookedKeys.Text = "Hooked Keys";
            this.tabPageHookedKeys.UseVisualStyleBackColor = true;
            // 
            // checkBoxHookKeys
            // 
            this.checkBoxHookKeys.AutoSize = true;
            this.checkBoxHookKeys.Checked = true;
            this.checkBoxHookKeys.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxHookKeys.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxHookKeys.Location = new System.Drawing.Point(46, 319);
            this.checkBoxHookKeys.Name = "checkBoxHookKeys";
            this.checkBoxHookKeys.Size = new System.Drawing.Size(96, 21);
            this.checkBoxHookKeys.TabIndex = 48;
            this.checkBoxHookKeys.Text = "Hook keys";
            this.checkBoxHookKeys.UseVisualStyleBackColor = true;
            this.checkBoxHookKeys.CheckedChanged += new System.EventHandler(this.checkBoxHookKeys_CheckedChanged);
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
            this.groupBoxFunctionKeys.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxFunctionKeys.Location = new System.Drawing.Point(244, 19);
            this.groupBoxFunctionKeys.Name = "groupBoxFunctionKeys";
            this.groupBoxFunctionKeys.Size = new System.Drawing.Size(130, 390);
            this.groupBoxFunctionKeys.TabIndex = 41;
            this.groupBoxFunctionKeys.TabStop = false;
            this.groupBoxFunctionKeys.Text = "Function Keys";
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
            this.checkBoxEnableF1.CheckedChanged += new System.EventHandler(this.checkBoxEnableF1_CheckedChanged);
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
            this.checkBoxEnableF2.CheckedChanged += new System.EventHandler(this.checkBoxEnableF2_CheckedChanged);
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
            this.checkBoxEnableF12.CheckedChanged += new System.EventHandler(this.checkBoxEnableF12_CheckedChanged);
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
            this.checkBoxEnableF3.CheckedChanged += new System.EventHandler(this.checkBoxEnableF3_CheckedChanged);
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
            this.checkBoxEnableF11.CheckedChanged += new System.EventHandler(this.checkBoxEnableF11_CheckedChanged);
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
            this.checkBoxEnableF4.CheckedChanged += new System.EventHandler(this.checkBoxEnableF4_CheckedChanged);
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
            this.checkBoxEnableF5.CheckedChanged += new System.EventHandler(this.checkBoxEnableF5_CheckedChanged);
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
            this.checkBoxEnableF10.CheckedChanged += new System.EventHandler(this.checkBoxEnableF10_CheckedChanged);
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
            this.checkBoxEnableF6.CheckedChanged += new System.EventHandler(this.checkBoxEnableF6_CheckedChanged);
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
            this.checkBoxEnableF9.CheckedChanged += new System.EventHandler(this.checkBoxEnableF9_CheckedChanged);
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
            this.checkBoxEnableF7.CheckedChanged += new System.EventHandler(this.checkBoxEnableF7_CheckedChanged);
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
            this.groupBoxSpecialKeys.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxSpecialKeys.Location = new System.Drawing.Point(20, 19);
            this.groupBoxSpecialKeys.Name = "groupBoxSpecialKeys";
            this.groupBoxSpecialKeys.Size = new System.Drawing.Size(170, 240);
            this.groupBoxSpecialKeys.TabIndex = 39;
            this.groupBoxSpecialKeys.TabStop = false;
            this.groupBoxSpecialKeys.Text = "Special Keys";
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
            this.checkBoxEnableEsc.CheckedChanged += new System.EventHandler(this.checkBoxEnableEsc_CheckedChanged);
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
            this.checkBoxEnableCtrlEsc.CheckedChanged += new System.EventHandler(this.checkBoxEnableCtrlEsc_CheckedChanged);
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
            this.checkBoxEnableAltEsc.CheckedChanged += new System.EventHandler(this.checkBoxEnableAltEsc_CheckedChanged);
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
            this.checkBoxEnableAltTab.CheckedChanged += new System.EventHandler(this.checkBoxEnableAltTab_CheckedChanged);
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
            this.checkBoxEnableAltF4.CheckedChanged += new System.EventHandler(this.checkBoxEnableAltF4_CheckedChanged);
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
            this.checkBoxEnableStartMenu.CheckedChanged += new System.EventHandler(this.checkBoxEnableStartMenu_CheckedChanged);
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
            this.checkBoxEnableRightMouse.CheckedChanged += new System.EventHandler(this.checkBoxEnableRightMouse_CheckedChanged);
            // 
            // tabPageRegistry
            // 
            this.tabPageRegistry.Controls.Add(this.groupBoxOutsideSeb);
            this.tabPageRegistry.Controls.Add(this.groupBoxSetOutsideSebValues);
            this.tabPageRegistry.Controls.Add(this.groupBoxInsideSeb);
            this.tabPageRegistry.Location = new System.Drawing.Point(4, 39);
            this.tabPageRegistry.Name = "tabPageRegistry";
            this.tabPageRegistry.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageRegistry.Size = new System.Drawing.Size(1092, 857);
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
            // tabPageSecurity
            // 
            this.tabPageSecurity.Controls.Add(this.labelLogDirectoryWin);
            this.tabPageSecurity.Controls.Add(this.buttonLogDirectoryWin);
            this.tabPageSecurity.Controls.Add(this.checkBoxCreateNewDesktop);
            this.tabPageSecurity.Controls.Add(this.checkBoxAllowUserSwitching);
            this.tabPageSecurity.Controls.Add(this.labelSebServicePolicy);
            this.tabPageSecurity.Controls.Add(this.listBoxSebServicePolicy);
            this.tabPageSecurity.Controls.Add(this.checkBoxEnableLogging);
            this.tabPageSecurity.Controls.Add(this.checkBoxAllowVirtualMachine);
            this.tabPageSecurity.Location = new System.Drawing.Point(4, 39);
            this.tabPageSecurity.Name = "tabPageSecurity";
            this.tabPageSecurity.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageSecurity.Size = new System.Drawing.Size(1092, 857);
            this.tabPageSecurity.TabIndex = 24;
            this.tabPageSecurity.Text = "Security";
            this.tabPageSecurity.UseVisualStyleBackColor = true;
            // 
            // labelLogDirectoryWin
            // 
            this.labelLogDirectoryWin.AutoSize = true;
            this.labelLogDirectoryWin.Location = new System.Drawing.Point(376, 306);
            this.labelLogDirectoryWin.Name = "labelLogDirectoryWin";
            this.labelLogDirectoryWin.Size = new System.Drawing.Size(0, 17);
            this.labelLogDirectoryWin.TabIndex = 79;
            // 
            // buttonLogDirectoryWin
            // 
            this.buttonLogDirectoryWin.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonLogDirectoryWin.Location = new System.Drawing.Point(75, 302);
            this.buttonLogDirectoryWin.Name = "buttonLogDirectoryWin";
            this.buttonLogDirectoryWin.Size = new System.Drawing.Size(191, 25);
            this.buttonLogDirectoryWin.TabIndex = 78;
            this.buttonLogDirectoryWin.Text = "Save log file to...";
            this.buttonLogDirectoryWin.UseVisualStyleBackColor = true;
            this.buttonLogDirectoryWin.Click += new System.EventHandler(this.buttonLogDirectoryWin_Click);
            // 
            // checkBoxCreateNewDesktop
            // 
            this.checkBoxCreateNewDesktop.AutoSize = true;
            this.checkBoxCreateNewDesktop.Checked = true;
            this.checkBoxCreateNewDesktop.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxCreateNewDesktop.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxCreateNewDesktop.Location = new System.Drawing.Point(31, 160);
            this.checkBoxCreateNewDesktop.Name = "checkBoxCreateNewDesktop";
            this.checkBoxCreateNewDesktop.Size = new System.Drawing.Size(155, 21);
            this.checkBoxCreateNewDesktop.TabIndex = 45;
            this.checkBoxCreateNewDesktop.Text = "Create new desktop";
            this.checkBoxCreateNewDesktop.UseVisualStyleBackColor = true;
            this.checkBoxCreateNewDesktop.CheckedChanged += new System.EventHandler(this.checkBoxCreateNewDesktop_CheckedChanged);
            // 
            // checkBoxAllowUserSwitching
            // 
            this.checkBoxAllowUserSwitching.AutoSize = true;
            this.checkBoxAllowUserSwitching.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxAllowUserSwitching.Location = new System.Drawing.Point(31, 213);
            this.checkBoxAllowUserSwitching.Name = "checkBoxAllowUserSwitching";
            this.checkBoxAllowUserSwitching.Size = new System.Drawing.Size(225, 21);
            this.checkBoxAllowUserSwitching.TabIndex = 76;
            this.checkBoxAllowUserSwitching.Text = "Allow user switching (Mac only)";
            this.checkBoxAllowUserSwitching.UseVisualStyleBackColor = true;
            this.checkBoxAllowUserSwitching.CheckedChanged += new System.EventHandler(this.checkBoxAllowUserSwitching_CheckedChanged);
            // 
            // labelSebServicePolicy
            // 
            this.labelSebServicePolicy.AutoSize = true;
            this.labelSebServicePolicy.Location = new System.Drawing.Point(28, 33);
            this.labelSebServicePolicy.Name = "labelSebServicePolicy";
            this.labelSebServicePolicy.Size = new System.Drawing.Size(126, 17);
            this.labelSebServicePolicy.TabIndex = 75;
            this.labelSebServicePolicy.Text = "SEB Service policy";
            // 
            // listBoxSebServicePolicy
            // 
            this.listBoxSebServicePolicy.FormattingEnabled = true;
            this.listBoxSebServicePolicy.ItemHeight = 16;
            this.listBoxSebServicePolicy.Location = new System.Drawing.Point(31, 63);
            this.listBoxSebServicePolicy.Name = "listBoxSebServicePolicy";
            this.listBoxSebServicePolicy.Size = new System.Drawing.Size(374, 52);
            this.listBoxSebServicePolicy.TabIndex = 74;
            this.listBoxSebServicePolicy.SelectedIndexChanged += new System.EventHandler(this.listBoxSebServicePolicy_SelectedIndexChanged);
            // 
            // checkBoxEnableLogging
            // 
            this.checkBoxEnableLogging.AutoSize = true;
            this.checkBoxEnableLogging.Checked = true;
            this.checkBoxEnableLogging.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxEnableLogging.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxEnableLogging.Location = new System.Drawing.Point(30, 266);
            this.checkBoxEnableLogging.Name = "checkBoxEnableLogging";
            this.checkBoxEnableLogging.Size = new System.Drawing.Size(124, 21);
            this.checkBoxEnableLogging.TabIndex = 48;
            this.checkBoxEnableLogging.Text = "Enable logging";
            this.checkBoxEnableLogging.UseVisualStyleBackColor = true;
            this.checkBoxEnableLogging.CheckedChanged += new System.EventHandler(this.checkBoxEnableLogging_CheckedChanged);
            // 
            // checkBoxAllowVirtualMachine
            // 
            this.checkBoxAllowVirtualMachine.AutoSize = true;
            this.checkBoxAllowVirtualMachine.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxAllowVirtualMachine.Location = new System.Drawing.Point(31, 133);
            this.checkBoxAllowVirtualMachine.Name = "checkBoxAllowVirtualMachine";
            this.checkBoxAllowVirtualMachine.Size = new System.Drawing.Size(274, 21);
            this.checkBoxAllowVirtualMachine.TabIndex = 43;
            this.checkBoxAllowVirtualMachine.Text = "Allow SEB to run inside virtual machine";
            this.checkBoxAllowVirtualMachine.UseVisualStyleBackColor = true;
            this.checkBoxAllowVirtualMachine.CheckedChanged += new System.EventHandler(this.checkBoxAllowVirtualMachine_CheckedChanged);
            // 
            // tabPageNetwork
            // 
            this.tabPageNetwork.Controls.Add(this.tabControlNetwork);
            this.tabPageNetwork.Location = new System.Drawing.Point(4, 39);
            this.tabPageNetwork.Name = "tabPageNetwork";
            this.tabPageNetwork.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageNetwork.Size = new System.Drawing.Size(1092, 857);
            this.tabPageNetwork.TabIndex = 23;
            this.tabPageNetwork.Text = "Network";
            this.tabPageNetwork.UseVisualStyleBackColor = true;
            // 
            // tabControlNetwork
            // 
            this.tabControlNetwork.Controls.Add(this.tabPageFilter);
            this.tabControlNetwork.Controls.Add(this.tabPageCertificates);
            this.tabControlNetwork.Controls.Add(this.tabPageProxies);
            this.tabControlNetwork.Location = new System.Drawing.Point(30, 29);
            this.tabControlNetwork.Name = "tabControlNetwork";
            this.tabControlNetwork.SelectedIndex = 0;
            this.tabControlNetwork.Size = new System.Drawing.Size(719, 391);
            this.tabControlNetwork.TabIndex = 0;
            // 
            // tabPageFilter
            // 
            this.tabPageFilter.Location = new System.Drawing.Point(4, 25);
            this.tabPageFilter.Name = "tabPageFilter";
            this.tabPageFilter.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageFilter.Size = new System.Drawing.Size(711, 362);
            this.tabPageFilter.TabIndex = 0;
            this.tabPageFilter.Text = "Filter";
            this.tabPageFilter.UseVisualStyleBackColor = true;
            // 
            // tabPageCertificates
            // 
            this.tabPageCertificates.Location = new System.Drawing.Point(4, 25);
            this.tabPageCertificates.Name = "tabPageCertificates";
            this.tabPageCertificates.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageCertificates.Size = new System.Drawing.Size(711, 362);
            this.tabPageCertificates.TabIndex = 1;
            this.tabPageCertificates.Text = "Certificates";
            this.tabPageCertificates.UseVisualStyleBackColor = true;
            // 
            // tabPageProxies
            // 
            this.tabPageProxies.Location = new System.Drawing.Point(4, 25);
            this.tabPageProxies.Name = "tabPageProxies";
            this.tabPageProxies.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageProxies.Size = new System.Drawing.Size(711, 362);
            this.tabPageProxies.TabIndex = 2;
            this.tabPageProxies.Text = "Proxies";
            this.tabPageProxies.UseVisualStyleBackColor = true;
            // 
            // tabPageApplications
            // 
            this.tabPageApplications.Controls.Add(this.tabControlApplications);
            this.tabPageApplications.Controls.Add(this.checkBoxMonitorProcesses);
            this.tabPageApplications.Location = new System.Drawing.Point(4, 39);
            this.tabPageApplications.Name = "tabPageApplications";
            this.tabPageApplications.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageApplications.Size = new System.Drawing.Size(1092, 757);
            this.tabPageApplications.TabIndex = 21;
            this.tabPageApplications.Text = "Applications";
            this.tabPageApplications.UseVisualStyleBackColor = true;
            // 
            // tabControlApplications
            // 
            this.tabControlApplications.Controls.Add(this.tabPagePermittedProcesses);
            this.tabControlApplications.Controls.Add(this.tabPageProhibitedProcesses);
            this.tabControlApplications.Location = new System.Drawing.Point(32, 66);
            this.tabControlApplications.Name = "tabControlApplications";
            this.tabControlApplications.SelectedIndex = 0;
            this.tabControlApplications.Size = new System.Drawing.Size(819, 671);
            this.tabControlApplications.TabIndex = 79;
            // 
            // tabPagePermittedProcesses
            // 
            this.tabPagePermittedProcesses.Controls.Add(this.buttonChoosePermittedProcess);
            this.tabPagePermittedProcesses.Controls.Add(this.buttonChoosePermittedApplication);
            this.tabPagePermittedProcesses.Controls.Add(this.buttonRemovePermittedProcess);
            this.tabPagePermittedProcesses.Controls.Add(this.buttonAddPermittedProcess);
            this.tabPagePermittedProcesses.Controls.Add(this.listViewPermittedProcesses);
            this.tabPagePermittedProcesses.Controls.Add(this.comboBox1);
            this.tabPagePermittedProcesses.Controls.Add(this.checkedListBoxPermitted);
            this.tabPagePermittedProcesses.Controls.Add(this.groupBoxSelectedProcess);
            this.tabPagePermittedProcesses.Controls.Add(this.checkBoxAllowSwitchToApplications);
            this.tabPagePermittedProcesses.Controls.Add(this.checkBoxAllowFlashFullscreen);
            this.tabPagePermittedProcesses.Location = new System.Drawing.Point(4, 25);
            this.tabPagePermittedProcesses.Name = "tabPagePermittedProcesses";
            this.tabPagePermittedProcesses.Padding = new System.Windows.Forms.Padding(3);
            this.tabPagePermittedProcesses.Size = new System.Drawing.Size(811, 642);
            this.tabPagePermittedProcesses.TabIndex = 0;
            this.tabPagePermittedProcesses.Text = "Permitted Processes";
            this.tabPagePermittedProcesses.UseVisualStyleBackColor = true;
            // 
            // buttonChoosePermittedProcess
            // 
            this.buttonChoosePermittedProcess.Location = new System.Drawing.Point(295, 229);
            this.buttonChoosePermittedProcess.Name = "buttonChoosePermittedProcess";
            this.buttonChoosePermittedProcess.Size = new System.Drawing.Size(141, 23);
            this.buttonChoosePermittedProcess.TabIndex = 88;
            this.buttonChoosePermittedProcess.Text = "Choose Process...";
            this.buttonChoosePermittedProcess.UseVisualStyleBackColor = true;
            // 
            // buttonChoosePermittedApplication
            // 
            this.buttonChoosePermittedApplication.Location = new System.Drawing.Point(123, 229);
            this.buttonChoosePermittedApplication.Name = "buttonChoosePermittedApplication";
            this.buttonChoosePermittedApplication.Size = new System.Drawing.Size(154, 23);
            this.buttonChoosePermittedApplication.TabIndex = 87;
            this.buttonChoosePermittedApplication.Text = "Choose Application...";
            this.buttonChoosePermittedApplication.UseVisualStyleBackColor = true;
            // 
            // buttonRemovePermittedProcess
            // 
            this.buttonRemovePermittedProcess.Location = new System.Drawing.Point(61, 229);
            this.buttonRemovePermittedProcess.Name = "buttonRemovePermittedProcess";
            this.buttonRemovePermittedProcess.Size = new System.Drawing.Size(25, 23);
            this.buttonRemovePermittedProcess.TabIndex = 86;
            this.buttonRemovePermittedProcess.Text = "-";
            this.buttonRemovePermittedProcess.UseVisualStyleBackColor = true;
            // 
            // buttonAddPermittedProcess
            // 
            this.buttonAddPermittedProcess.Location = new System.Drawing.Point(26, 229);
            this.buttonAddPermittedProcess.Name = "buttonAddPermittedProcess";
            this.buttonAddPermittedProcess.Size = new System.Drawing.Size(29, 23);
            this.buttonAddPermittedProcess.TabIndex = 85;
            this.buttonAddPermittedProcess.Text = "+";
            this.buttonAddPermittedProcess.UseVisualStyleBackColor = true;
            // 
            // listViewPermittedProcesses
            // 
            this.listViewPermittedProcesses.HideSelection = false;
            this.listViewPermittedProcesses.Location = new System.Drawing.Point(261, 85);
            this.listViewPermittedProcesses.MultiSelect = false;
            this.listViewPermittedProcesses.Name = "listViewPermittedProcesses";
            this.listViewPermittedProcesses.Size = new System.Drawing.Size(449, 55);
            this.listViewPermittedProcesses.TabIndex = 84;
            this.listViewPermittedProcesses.UseCompatibleStateImageBehavior = false;
            this.listViewPermittedProcesses.SelectedIndexChanged += new System.EventHandler(this.listViewPermittedProcesses_SelectedIndexChanged);
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(112, 85);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 24);
            this.comboBox1.TabIndex = 83;
            // 
            // checkedListBoxPermitted
            // 
            this.checkedListBoxPermitted.FormattingEnabled = true;
            this.checkedListBoxPermitted.Location = new System.Drawing.Point(24, 85);
            this.checkedListBoxPermitted.Name = "checkedListBoxPermitted";
            this.checkedListBoxPermitted.Size = new System.Drawing.Size(73, 55);
            this.checkedListBoxPermitted.TabIndex = 82;
            // 
            // groupBoxSelectedProcess
            // 
            this.groupBoxSelectedProcess.Controls.Add(this.labelPermittedProcessIdentifier);
            this.groupBoxSelectedProcess.Controls.Add(this.textBoxPermittedProcessIdentifier);
            this.groupBoxSelectedProcess.Controls.Add(this.checkedListBoxPermittedProcessArguments);
            this.groupBoxSelectedProcess.Controls.Add(this.buttonPermittedProcessRemoveArgument);
            this.groupBoxSelectedProcess.Controls.Add(this.buttonPermittedProcessAddArgument);
            this.groupBoxSelectedProcess.Controls.Add(this.labelPermittedProcessArguments);
            this.groupBoxSelectedProcess.Controls.Add(this.labelPermittedProcessOS);
            this.groupBoxSelectedProcess.Controls.Add(this.listBoxPermittedProcessOS);
            this.groupBoxSelectedProcess.Controls.Add(this.labelPermittedProcessExecutable);
            this.groupBoxSelectedProcess.Controls.Add(this.labelPermittedProcessPath);
            this.groupBoxSelectedProcess.Controls.Add(this.textBoxPermittedProcessPath);
            this.groupBoxSelectedProcess.Controls.Add(this.textBoxPermittedProcessExecutable);
            this.groupBoxSelectedProcess.Controls.Add(this.textBoxPermittedProcessDescription);
            this.groupBoxSelectedProcess.Controls.Add(this.labelPermittedProcessDescription);
            this.groupBoxSelectedProcess.Controls.Add(this.labelPermittedProcessAppTitle);
            this.groupBoxSelectedProcess.Controls.Add(this.textBoxPermittedProcessAppTitle);
            this.groupBoxSelectedProcess.Controls.Add(this.checkBoxPermittedProcessAllowUser);
            this.groupBoxSelectedProcess.Controls.Add(this.checkBoxPermittedProcessAutohide);
            this.groupBoxSelectedProcess.Controls.Add(this.checkBoxPermittedProcessAutostart);
            this.groupBoxSelectedProcess.Controls.Add(this.checkBoxPermittedProcessActive);
            this.groupBoxSelectedProcess.Location = new System.Drawing.Point(24, 273);
            this.groupBoxSelectedProcess.Name = "groupBoxSelectedProcess";
            this.groupBoxSelectedProcess.Size = new System.Drawing.Size(733, 347);
            this.groupBoxSelectedProcess.TabIndex = 80;
            this.groupBoxSelectedProcess.TabStop = false;
            this.groupBoxSelectedProcess.Text = "Selected Process";
            // 
            // labelPermittedProcessIdentifier
            // 
            this.labelPermittedProcessIdentifier.AutoSize = true;
            this.labelPermittedProcessIdentifier.Location = new System.Drawing.Point(169, 255);
            this.labelPermittedProcessIdentifier.Name = "labelPermittedProcessIdentifier";
            this.labelPermittedProcessIdentifier.Size = new System.Drawing.Size(62, 17);
            this.labelPermittedProcessIdentifier.TabIndex = 89;
            this.labelPermittedProcessIdentifier.Text = "Identifier";
            // 
            // textBoxPermittedProcessIdentifier
            // 
            this.textBoxPermittedProcessIdentifier.Location = new System.Drawing.Point(237, 252);
            this.textBoxPermittedProcessIdentifier.Name = "textBoxPermittedProcessIdentifier";
            this.textBoxPermittedProcessIdentifier.Size = new System.Drawing.Size(449, 22);
            this.textBoxPermittedProcessIdentifier.TabIndex = 89;
            // 
            // checkedListBoxPermittedProcessArguments
            // 
            this.checkedListBoxPermittedProcessArguments.FormattingEnabled = true;
            this.checkedListBoxPermittedProcessArguments.Location = new System.Drawing.Point(114, 186);
            this.checkedListBoxPermittedProcessArguments.Name = "checkedListBoxPermittedProcessArguments";
            this.checkedListBoxPermittedProcessArguments.Size = new System.Drawing.Size(572, 55);
            this.checkedListBoxPermittedProcessArguments.TabIndex = 88;
            // 
            // buttonPermittedProcessRemoveArgument
            // 
            this.buttonPermittedProcessRemoveArgument.Location = new System.Drawing.Point(58, 210);
            this.buttonPermittedProcessRemoveArgument.Name = "buttonPermittedProcessRemoveArgument";
            this.buttonPermittedProcessRemoveArgument.Size = new System.Drawing.Size(25, 23);
            this.buttonPermittedProcessRemoveArgument.TabIndex = 87;
            this.buttonPermittedProcessRemoveArgument.Text = "-";
            this.buttonPermittedProcessRemoveArgument.UseVisualStyleBackColor = true;
            // 
            // buttonPermittedProcessAddArgument
            // 
            this.buttonPermittedProcessAddArgument.Location = new System.Drawing.Point(23, 210);
            this.buttonPermittedProcessAddArgument.Name = "buttonPermittedProcessAddArgument";
            this.buttonPermittedProcessAddArgument.Size = new System.Drawing.Size(29, 23);
            this.buttonPermittedProcessAddArgument.TabIndex = 86;
            this.buttonPermittedProcessAddArgument.Text = "+";
            this.buttonPermittedProcessAddArgument.UseVisualStyleBackColor = true;
            // 
            // labelPermittedProcessArguments
            // 
            this.labelPermittedProcessArguments.AutoSize = true;
            this.labelPermittedProcessArguments.Location = new System.Drawing.Point(20, 186);
            this.labelPermittedProcessArguments.Name = "labelPermittedProcessArguments";
            this.labelPermittedProcessArguments.Size = new System.Drawing.Size(76, 17);
            this.labelPermittedProcessArguments.TabIndex = 14;
            this.labelPermittedProcessArguments.Text = "Arguments";
            // 
            // labelPermittedProcessOS
            // 
            this.labelPermittedProcessOS.AutoSize = true;
            this.labelPermittedProcessOS.Location = new System.Drawing.Point(24, 109);
            this.labelPermittedProcessOS.Name = "labelPermittedProcessOS";
            this.labelPermittedProcessOS.Size = new System.Drawing.Size(28, 17);
            this.labelPermittedProcessOS.TabIndex = 13;
            this.labelPermittedProcessOS.Text = "OS";
            // 
            // listBoxPermittedProcessOS
            // 
            this.listBoxPermittedProcessOS.FormattingEnabled = true;
            this.listBoxPermittedProcessOS.ItemHeight = 16;
            this.listBoxPermittedProcessOS.Location = new System.Drawing.Point(56, 109);
            this.listBoxPermittedProcessOS.Name = "listBoxPermittedProcessOS";
            this.listBoxPermittedProcessOS.Size = new System.Drawing.Size(62, 36);
            this.listBoxPermittedProcessOS.TabIndex = 12;
            this.listBoxPermittedProcessOS.SelectedIndexChanged += new System.EventHandler(this.listBoxPermittedProcessOS_SelectedIndexChanged);
            // 
            // labelPermittedProcessExecutable
            // 
            this.labelPermittedProcessExecutable.AutoSize = true;
            this.labelPermittedProcessExecutable.Location = new System.Drawing.Point(154, 109);
            this.labelPermittedProcessExecutable.Name = "labelPermittedProcessExecutable";
            this.labelPermittedProcessExecutable.Size = new System.Drawing.Size(77, 17);
            this.labelPermittedProcessExecutable.TabIndex = 11;
            this.labelPermittedProcessExecutable.Text = "Executable";
            // 
            // labelPermittedProcessPath
            // 
            this.labelPermittedProcessPath.AutoSize = true;
            this.labelPermittedProcessPath.Location = new System.Drawing.Point(64, 150);
            this.labelPermittedProcessPath.Name = "labelPermittedProcessPath";
            this.labelPermittedProcessPath.Size = new System.Drawing.Size(37, 17);
            this.labelPermittedProcessPath.TabIndex = 10;
            this.labelPermittedProcessPath.Text = "Path";
            // 
            // textBoxPermittedProcessPath
            // 
            this.textBoxPermittedProcessPath.Location = new System.Drawing.Point(114, 150);
            this.textBoxPermittedProcessPath.Name = "textBoxPermittedProcessPath";
            this.textBoxPermittedProcessPath.Size = new System.Drawing.Size(572, 22);
            this.textBoxPermittedProcessPath.TabIndex = 9;
            // 
            // textBoxPermittedProcessExecutable
            // 
            this.textBoxPermittedProcessExecutable.Location = new System.Drawing.Point(237, 109);
            this.textBoxPermittedProcessExecutable.Name = "textBoxPermittedProcessExecutable";
            this.textBoxPermittedProcessExecutable.Size = new System.Drawing.Size(449, 22);
            this.textBoxPermittedProcessExecutable.TabIndex = 8;
            // 
            // textBoxPermittedProcessDescription
            // 
            this.textBoxPermittedProcessDescription.Location = new System.Drawing.Point(114, 68);
            this.textBoxPermittedProcessDescription.Name = "textBoxPermittedProcessDescription";
            this.textBoxPermittedProcessDescription.Size = new System.Drawing.Size(572, 22);
            this.textBoxPermittedProcessDescription.TabIndex = 7;
            // 
            // labelPermittedProcessDescription
            // 
            this.labelPermittedProcessDescription.AutoSize = true;
            this.labelPermittedProcessDescription.Location = new System.Drawing.Point(22, 68);
            this.labelPermittedProcessDescription.Name = "labelPermittedProcessDescription";
            this.labelPermittedProcessDescription.Size = new System.Drawing.Size(79, 17);
            this.labelPermittedProcessDescription.TabIndex = 6;
            this.labelPermittedProcessDescription.Text = "Description";
            // 
            // labelPermittedProcessAppTitle
            // 
            this.labelPermittedProcessAppTitle.AutoSize = true;
            this.labelPermittedProcessAppTitle.Location = new System.Drawing.Point(174, 32);
            this.labelPermittedProcessAppTitle.Name = "labelPermittedProcessAppTitle";
            this.labelPermittedProcessAppTitle.Size = new System.Drawing.Size(60, 17);
            this.labelPermittedProcessAppTitle.TabIndex = 5;
            this.labelPermittedProcessAppTitle.Text = "AppTitle";
            // 
            // textBoxPermittedProcessAppTitle
            // 
            this.textBoxPermittedProcessAppTitle.Location = new System.Drawing.Point(237, 32);
            this.textBoxPermittedProcessAppTitle.Name = "textBoxPermittedProcessAppTitle";
            this.textBoxPermittedProcessAppTitle.Size = new System.Drawing.Size(449, 22);
            this.textBoxPermittedProcessAppTitle.TabIndex = 4;
            // 
            // checkBoxPermittedProcessAllowUser
            // 
            this.checkBoxPermittedProcessAllowUser.AutoSize = true;
            this.checkBoxPermittedProcessAllowUser.Location = new System.Drawing.Point(19, 306);
            this.checkBoxPermittedProcessAllowUser.Name = "checkBoxPermittedProcessAllowUser";
            this.checkBoxPermittedProcessAllowUser.Size = new System.Drawing.Size(292, 21);
            this.checkBoxPermittedProcessAllowUser.TabIndex = 3;
            this.checkBoxPermittedProcessAllowUser.Text = "Allow user to select location of application";
            this.checkBoxPermittedProcessAllowUser.UseVisualStyleBackColor = true;
            // 
            // checkBoxPermittedProcessAutohide
            // 
            this.checkBoxPermittedProcessAutohide.AutoSize = true;
            this.checkBoxPermittedProcessAutohide.Location = new System.Drawing.Point(19, 279);
            this.checkBoxPermittedProcessAutohide.Name = "checkBoxPermittedProcessAutohide";
            this.checkBoxPermittedProcessAutohide.Size = new System.Drawing.Size(86, 21);
            this.checkBoxPermittedProcessAutohide.TabIndex = 2;
            this.checkBoxPermittedProcessAutohide.Text = "Autohide";
            this.checkBoxPermittedProcessAutohide.UseVisualStyleBackColor = true;
            // 
            // checkBoxPermittedProcessAutostart
            // 
            this.checkBoxPermittedProcessAutostart.AutoSize = true;
            this.checkBoxPermittedProcessAutostart.Location = new System.Drawing.Point(18, 252);
            this.checkBoxPermittedProcessAutostart.Name = "checkBoxPermittedProcessAutostart";
            this.checkBoxPermittedProcessAutostart.Size = new System.Drawing.Size(87, 21);
            this.checkBoxPermittedProcessAutostart.TabIndex = 1;
            this.checkBoxPermittedProcessAutostart.Text = "Autostart";
            this.checkBoxPermittedProcessAutostart.UseVisualStyleBackColor = true;
            // 
            // checkBoxPermittedProcessActive
            // 
            this.checkBoxPermittedProcessActive.AutoSize = true;
            this.checkBoxPermittedProcessActive.Location = new System.Drawing.Point(23, 28);
            this.checkBoxPermittedProcessActive.Name = "checkBoxPermittedProcessActive";
            this.checkBoxPermittedProcessActive.Size = new System.Drawing.Size(68, 21);
            this.checkBoxPermittedProcessActive.TabIndex = 0;
            this.checkBoxPermittedProcessActive.Text = "Active";
            this.checkBoxPermittedProcessActive.UseVisualStyleBackColor = true;
            // 
            // checkBoxAllowSwitchToApplications
            // 
            this.checkBoxAllowSwitchToApplications.AutoSize = true;
            this.checkBoxAllowSwitchToApplications.Checked = true;
            this.checkBoxAllowSwitchToApplications.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxAllowSwitchToApplications.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxAllowSwitchToApplications.Location = new System.Drawing.Point(24, 19);
            this.checkBoxAllowSwitchToApplications.Name = "checkBoxAllowSwitchToApplications";
            this.checkBoxAllowSwitchToApplications.Size = new System.Drawing.Size(286, 21);
            this.checkBoxAllowSwitchToApplications.TabIndex = 77;
            this.checkBoxAllowSwitchToApplications.Text = "Allow switching to third party applications";
            this.checkBoxAllowSwitchToApplications.UseVisualStyleBackColor = true;
            this.checkBoxAllowSwitchToApplications.CheckedChanged += new System.EventHandler(this.checkBoxAllowSwitchToApplications_CheckedChanged);
            // 
            // checkBoxAllowFlashFullscreen
            // 
            this.checkBoxAllowFlashFullscreen.AutoSize = true;
            this.checkBoxAllowFlashFullscreen.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxAllowFlashFullscreen.Location = new System.Drawing.Point(47, 46);
            this.checkBoxAllowFlashFullscreen.Name = "checkBoxAllowFlashFullscreen";
            this.checkBoxAllowFlashFullscreen.Size = new System.Drawing.Size(278, 21);
            this.checkBoxAllowFlashFullscreen.TabIndex = 78;
            this.checkBoxAllowFlashFullscreen.Text = "Allow Flash to switch to fullscreen mode";
            this.checkBoxAllowFlashFullscreen.UseVisualStyleBackColor = true;
            this.checkBoxAllowFlashFullscreen.CheckedChanged += new System.EventHandler(this.checkBoxAllowFlashFullscreen_CheckedChanged);
            // 
            // tabPageProhibitedProcesses
            // 
            this.tabPageProhibitedProcesses.Location = new System.Drawing.Point(4, 25);
            this.tabPageProhibitedProcesses.Name = "tabPageProhibitedProcesses";
            this.tabPageProhibitedProcesses.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageProhibitedProcesses.Size = new System.Drawing.Size(811, 642);
            this.tabPageProhibitedProcesses.TabIndex = 1;
            this.tabPageProhibitedProcesses.Text = "Prohibited Processes";
            this.tabPageProhibitedProcesses.UseVisualStyleBackColor = true;
            // 
            // checkBoxMonitorProcesses
            // 
            this.checkBoxMonitorProcesses.AutoSize = true;
            this.checkBoxMonitorProcesses.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxMonitorProcesses.Location = new System.Drawing.Point(32, 25);
            this.checkBoxMonitorProcesses.Name = "checkBoxMonitorProcesses";
            this.checkBoxMonitorProcesses.Size = new System.Drawing.Size(146, 21);
            this.checkBoxMonitorProcesses.TabIndex = 50;
            this.checkBoxMonitorProcesses.Text = "Monitor processes";
            this.checkBoxMonitorProcesses.UseVisualStyleBackColor = true;
            this.checkBoxMonitorProcesses.CheckedChanged += new System.EventHandler(this.checkBoxMonitorProcesses_CheckedChanged);
            // 
            // tabPageExam
            // 
            this.tabPageExam.Controls.Add(this.labelPlaceThisQuitLink);
            this.tabPageExam.Controls.Add(this.labelCopyBrowserExamKey);
            this.tabPageExam.Controls.Add(this.buttonGenerateBrowserExamKey);
            this.tabPageExam.Controls.Add(this.labelBrowserExamKey);
            this.tabPageExam.Controls.Add(this.textBoxBrowserExamKey);
            this.tabPageExam.Controls.Add(this.textBoxQuitURL);
            this.tabPageExam.Controls.Add(this.labelQuitURL);
            this.tabPageExam.Controls.Add(this.checkBoxSendBrowserExamKey);
            this.tabPageExam.Controls.Add(this.checkBoxCopyBrowserExamKey);
            this.tabPageExam.Location = new System.Drawing.Point(4, 39);
            this.tabPageExam.Name = "tabPageExam";
            this.tabPageExam.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageExam.Size = new System.Drawing.Size(1092, 857);
            this.tabPageExam.TabIndex = 18;
            this.tabPageExam.Text = "Exam";
            this.tabPageExam.UseVisualStyleBackColor = true;
            // 
            // labelPlaceThisQuitLink
            // 
            this.labelPlaceThisQuitLink.AutoSize = true;
            this.labelPlaceThisQuitLink.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPlaceThisQuitLink.Location = new System.Drawing.Point(29, 323);
            this.labelPlaceThisQuitLink.Name = "labelPlaceThisQuitLink";
            this.labelPlaceThisQuitLink.Size = new System.Drawing.Size(1039, 17);
            this.labelPlaceThisQuitLink.TabIndex = 81;
            this.labelPlaceThisQuitLink.Text = "Place this quit link to the \"feedback\" page displayed after an exam was successfu" +
    "lly finished. Clicking that link will quit SEB without having to enter the quit " +
    "password.";
            // 
            // labelCopyBrowserExamKey
            // 
            this.labelCopyBrowserExamKey.AutoSize = true;
            this.labelCopyBrowserExamKey.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelCopyBrowserExamKey.Location = new System.Drawing.Point(29, 101);
            this.labelCopyBrowserExamKey.Name = "labelCopyBrowserExamKey";
            this.labelCopyBrowserExamKey.Size = new System.Drawing.Size(1031, 17);
            this.labelCopyBrowserExamKey.TabIndex = 80;
            this.labelCopyBrowserExamKey.Text = "Copy this key (which depends on your SEB configuration) to the according field in" +
    " your quiz settings in the exam system having support for SEB 2.0 or later built" +
    " in.";
            // 
            // buttonGenerateBrowserExamKey
            // 
            this.buttonGenerateBrowserExamKey.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonGenerateBrowserExamKey.Location = new System.Drawing.Point(176, 27);
            this.buttonGenerateBrowserExamKey.Name = "buttonGenerateBrowserExamKey";
            this.buttonGenerateBrowserExamKey.Size = new System.Drawing.Size(133, 25);
            this.buttonGenerateBrowserExamKey.TabIndex = 79;
            this.buttonGenerateBrowserExamKey.Text = "Generate";
            this.buttonGenerateBrowserExamKey.UseVisualStyleBackColor = true;
            this.buttonGenerateBrowserExamKey.Click += new System.EventHandler(this.buttonGenerateBrowserExamKey_Click);
            // 
            // labelBrowserExamKey
            // 
            this.labelBrowserExamKey.AutoSize = true;
            this.labelBrowserExamKey.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelBrowserExamKey.Location = new System.Drawing.Point(29, 31);
            this.labelBrowserExamKey.Name = "labelBrowserExamKey";
            this.labelBrowserExamKey.Size = new System.Drawing.Size(125, 17);
            this.labelBrowserExamKey.TabIndex = 78;
            this.labelBrowserExamKey.Text = "Browser Exam Key";
            // 
            // textBoxBrowserExamKey
            // 
            this.textBoxBrowserExamKey.Location = new System.Drawing.Point(32, 67);
            this.textBoxBrowserExamKey.Name = "textBoxBrowserExamKey";
            this.textBoxBrowserExamKey.Size = new System.Drawing.Size(700, 22);
            this.textBoxBrowserExamKey.TabIndex = 77;
            this.textBoxBrowserExamKey.TextChanged += new System.EventHandler(this.textBoxBrowserExamKey_TextChanged);
            // 
            // textBoxQuitURL
            // 
            this.textBoxQuitURL.Font = new System.Drawing.Font("Courier New", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxQuitURL.Location = new System.Drawing.Point(32, 288);
            this.textBoxQuitURL.Name = "textBoxQuitURL";
            this.textBoxQuitURL.Size = new System.Drawing.Size(535, 22);
            this.textBoxQuitURL.TabIndex = 76;
            this.textBoxQuitURL.TextChanged += new System.EventHandler(this.textBoxQuitURL_TextChanged);
            // 
            // labelQuitURL
            // 
            this.labelQuitURL.AutoSize = true;
            this.labelQuitURL.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelQuitURL.Location = new System.Drawing.Point(29, 255);
            this.labelQuitURL.Name = "labelQuitURL";
            this.labelQuitURL.Size = new System.Drawing.Size(178, 17);
            this.labelQuitURL.TabIndex = 75;
            this.labelQuitURL.Text = "Link to quit SEB after exam";
            // 
            // checkBoxSendBrowserExamKey
            // 
            this.checkBoxSendBrowserExamKey.AutoSize = true;
            this.checkBoxSendBrowserExamKey.Checked = true;
            this.checkBoxSendBrowserExamKey.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxSendBrowserExamKey.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxSendBrowserExamKey.Location = new System.Drawing.Point(32, 176);
            this.checkBoxSendBrowserExamKey.Name = "checkBoxSendBrowserExamKey";
            this.checkBoxSendBrowserExamKey.Size = new System.Drawing.Size(289, 21);
            this.checkBoxSendBrowserExamKey.TabIndex = 74;
            this.checkBoxSendBrowserExamKey.Text = "Send Browser Exam Key in HTTP header";
            this.checkBoxSendBrowserExamKey.UseVisualStyleBackColor = true;
            this.checkBoxSendBrowserExamKey.CheckedChanged += new System.EventHandler(this.checkBoxSendBrowserExamKey_CheckedChanged);
            // 
            // checkBoxCopyBrowserExamKey
            // 
            this.checkBoxCopyBrowserExamKey.AutoSize = true;
            this.checkBoxCopyBrowserExamKey.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxCopyBrowserExamKey.Location = new System.Drawing.Point(66, 139);
            this.checkBoxCopyBrowserExamKey.Name = "checkBoxCopyBrowserExamKey";
            this.checkBoxCopyBrowserExamKey.Size = new System.Drawing.Size(568, 21);
            this.checkBoxCopyBrowserExamKey.TabIndex = 73;
            this.checkBoxCopyBrowserExamKey.Text = "Copy Browser Exam Key to clipboard when quitting SEB Windows Configuration Editor" +
    "";
            this.checkBoxCopyBrowserExamKey.UseVisualStyleBackColor = true;
            this.checkBoxCopyBrowserExamKey.CheckedChanged += new System.EventHandler(this.checkBoxCopyBrowserExamKey_CheckedChanged);
            // 
            // tabPageDownUploads
            // 
            this.tabPageDownUploads.Controls.Add(this.labelDownloadDirectoryWin);
            this.tabPageDownUploads.Controls.Add(this.buttonDownloadDirectoryWin);
            this.tabPageDownUploads.Controls.Add(this.listBoxChooseFileToUploadPolicy);
            this.tabPageDownUploads.Controls.Add(this.labelChooseFileToUploadPolicy);
            this.tabPageDownUploads.Controls.Add(this.checkBoxDownloadPDFFiles);
            this.tabPageDownUploads.Controls.Add(this.checkBoxOpenDownloads);
            this.tabPageDownUploads.Controls.Add(this.checkBoxAllowDownUploads);
            this.tabPageDownUploads.ImageIndex = 0;
            this.tabPageDownUploads.Location = new System.Drawing.Point(4, 39);
            this.tabPageDownUploads.Name = "tabPageDownUploads";
            this.tabPageDownUploads.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageDownUploads.Size = new System.Drawing.Size(1092, 857);
            this.tabPageDownUploads.TabIndex = 17;
            this.tabPageDownUploads.Text = "Down/Uploads";
            this.tabPageDownUploads.UseVisualStyleBackColor = true;
            // 
            // labelDownloadDirectoryWin
            // 
            this.labelDownloadDirectoryWin.AutoSize = true;
            this.labelDownloadDirectoryWin.Location = new System.Drawing.Point(433, 83);
            this.labelDownloadDirectoryWin.Name = "labelDownloadDirectoryWin";
            this.labelDownloadDirectoryWin.Size = new System.Drawing.Size(0, 17);
            this.labelDownloadDirectoryWin.TabIndex = 78;
            // 
            // buttonDownloadDirectoryWin
            // 
            this.buttonDownloadDirectoryWin.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonDownloadDirectoryWin.Location = new System.Drawing.Point(152, 79);
            this.buttonDownloadDirectoryWin.Name = "buttonDownloadDirectoryWin";
            this.buttonDownloadDirectoryWin.Size = new System.Drawing.Size(191, 25);
            this.buttonDownloadDirectoryWin.TabIndex = 77;
            this.buttonDownloadDirectoryWin.Text = "Save downloaded files to...";
            this.buttonDownloadDirectoryWin.UseVisualStyleBackColor = true;
            this.buttonDownloadDirectoryWin.Click += new System.EventHandler(this.buttonDownloadDirectoryWin_Click);
            // 
            // listBoxChooseFileToUploadPolicy
            // 
            this.listBoxChooseFileToUploadPolicy.FormattingEnabled = true;
            this.listBoxChooseFileToUploadPolicy.ItemHeight = 16;
            this.listBoxChooseFileToUploadPolicy.Location = new System.Drawing.Point(33, 279);
            this.listBoxChooseFileToUploadPolicy.Name = "listBoxChooseFileToUploadPolicy";
            this.listBoxChooseFileToUploadPolicy.Size = new System.Drawing.Size(310, 52);
            this.listBoxChooseFileToUploadPolicy.TabIndex = 76;
            this.listBoxChooseFileToUploadPolicy.SelectedIndexChanged += new System.EventHandler(this.listBoxChooseFileToUploadPolicy_SelectedIndexChanged);
            // 
            // labelChooseFileToUploadPolicy
            // 
            this.labelChooseFileToUploadPolicy.AutoSize = true;
            this.labelChooseFileToUploadPolicy.Location = new System.Drawing.Point(30, 244);
            this.labelChooseFileToUploadPolicy.Name = "labelChooseFileToUploadPolicy";
            this.labelChooseFileToUploadPolicy.Size = new System.Drawing.Size(153, 17);
            this.labelChooseFileToUploadPolicy.TabIndex = 75;
            this.labelChooseFileToUploadPolicy.Text = "Choose file to upload...";
            // 
            // checkBoxDownloadPDFFiles
            // 
            this.checkBoxDownloadPDFFiles.AutoSize = true;
            this.checkBoxDownloadPDFFiles.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxDownloadPDFFiles.Location = new System.Drawing.Point(33, 351);
            this.checkBoxDownloadPDFFiles.Name = "checkBoxDownloadPDFFiles";
            this.checkBoxDownloadPDFFiles.Size = new System.Drawing.Size(421, 21);
            this.checkBoxDownloadPDFFiles.TabIndex = 73;
            this.checkBoxDownloadPDFFiles.Text = "Download and open PDF files instead of displaying them inline";
            this.checkBoxDownloadPDFFiles.UseVisualStyleBackColor = true;
            this.checkBoxDownloadPDFFiles.CheckedChanged += new System.EventHandler(this.checkBoxDownloadPDFFiles_CheckedChanged);
            // 
            // checkBoxOpenDownloads
            // 
            this.checkBoxOpenDownloads.AutoSize = true;
            this.checkBoxOpenDownloads.Checked = true;
            this.checkBoxOpenDownloads.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxOpenDownloads.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxOpenDownloads.Location = new System.Drawing.Point(152, 132);
            this.checkBoxOpenDownloads.Name = "checkBoxOpenDownloads";
            this.checkBoxOpenDownloads.Size = new System.Drawing.Size(210, 21);
            this.checkBoxOpenDownloads.TabIndex = 72;
            this.checkBoxOpenDownloads.Text = "Open files after downloading";
            this.checkBoxOpenDownloads.UseVisualStyleBackColor = true;
            this.checkBoxOpenDownloads.CheckedChanged += new System.EventHandler(this.checkBoxOpenDownloads_CheckedChanged);
            // 
            // checkBoxAllowDownUploads
            // 
            this.checkBoxAllowDownUploads.AutoSize = true;
            this.checkBoxAllowDownUploads.Checked = true;
            this.checkBoxAllowDownUploads.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxAllowDownUploads.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxAllowDownUploads.Location = new System.Drawing.Point(33, 32);
            this.checkBoxAllowDownUploads.Name = "checkBoxAllowDownUploads";
            this.checkBoxAllowDownUploads.Size = new System.Drawing.Size(268, 21);
            this.checkBoxAllowDownUploads.TabIndex = 71;
            this.checkBoxAllowDownUploads.Text = "Allow downloading and uploading files";
            this.checkBoxAllowDownUploads.UseVisualStyleBackColor = true;
            this.checkBoxAllowDownUploads.CheckedChanged += new System.EventHandler(this.checkBoxAllowDownUploads_CheckedChanged);
            // 
            // tabPageBrowser
            // 
            this.tabPageBrowser.Controls.Add(this.listBoxOpenLinksJava);
            this.tabPageBrowser.Controls.Add(this.listBoxOpenLinksHTML);
            this.tabPageBrowser.Controls.Add(this.labelUseSEBWithoutBrowser);
            this.tabPageBrowser.Controls.Add(this.checkBoxBlockPopUpWindows);
            this.tabPageBrowser.Controls.Add(this.checkBoxAllowBrowsingBackForward);
            this.tabPageBrowser.Controls.Add(this.checkBoxEnableJavaScript);
            this.tabPageBrowser.Controls.Add(this.checkBoxEnableJava);
            this.tabPageBrowser.Controls.Add(this.checkBoxEnablePlugIns);
            this.tabPageBrowser.Controls.Add(this.checkBoxUseSebWithoutBrowser);
            this.tabPageBrowser.Controls.Add(this.checkBoxBlockLinksJava);
            this.tabPageBrowser.Controls.Add(this.labelOpenLinksJava);
            this.tabPageBrowser.Controls.Add(this.labelOpenLinksHTML);
            this.tabPageBrowser.Controls.Add(this.checkBoxBlockLinksHTML);
            this.tabPageBrowser.Controls.Add(this.groupBoxNewBrowserWindow);
            this.tabPageBrowser.ImageIndex = 4;
            this.tabPageBrowser.Location = new System.Drawing.Point(4, 39);
            this.tabPageBrowser.Name = "tabPageBrowser";
            this.tabPageBrowser.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageBrowser.Size = new System.Drawing.Size(1092, 857);
            this.tabPageBrowser.TabIndex = 14;
            this.tabPageBrowser.Text = "Browser";
            this.tabPageBrowser.UseVisualStyleBackColor = true;
            // 
            // listBoxOpenLinksJava
            // 
            this.listBoxOpenLinksJava.FormattingEnabled = true;
            this.listBoxOpenLinksJava.ItemHeight = 16;
            this.listBoxOpenLinksJava.Location = new System.Drawing.Point(31, 277);
            this.listBoxOpenLinksJava.Name = "listBoxOpenLinksJava";
            this.listBoxOpenLinksJava.Size = new System.Drawing.Size(197, 52);
            this.listBoxOpenLinksJava.TabIndex = 74;
            this.listBoxOpenLinksJava.SelectedIndexChanged += new System.EventHandler(this.listBoxOpenLinksJava_SelectedIndexChanged);
            // 
            // listBoxOpenLinksHTML
            // 
            this.listBoxOpenLinksHTML.FormattingEnabled = true;
            this.listBoxOpenLinksHTML.ItemHeight = 16;
            this.listBoxOpenLinksHTML.Location = new System.Drawing.Point(31, 51);
            this.listBoxOpenLinksHTML.Name = "listBoxOpenLinksHTML";
            this.listBoxOpenLinksHTML.Size = new System.Drawing.Size(197, 52);
            this.listBoxOpenLinksHTML.TabIndex = 73;
            this.listBoxOpenLinksHTML.SelectedIndexChanged += new System.EventHandler(this.listBoxOpenLinksHTML_SelectedIndexChanged);
            // 
            // labelUseSEBWithoutBrowser
            // 
            this.labelUseSEBWithoutBrowser.AutoSize = true;
            this.labelUseSEBWithoutBrowser.Location = new System.Drawing.Point(264, 405);
            this.labelUseSEBWithoutBrowser.Name = "labelUseSEBWithoutBrowser";
            this.labelUseSEBWithoutBrowser.Size = new System.Drawing.Size(587, 17);
            this.labelUseSEBWithoutBrowser.TabIndex = 72;
            this.labelUseSEBWithoutBrowser.Text = "to start another application in kiosk mode (for example a virtual desktop infrast" +
    "ructure client)";
            // 
            // checkBoxBlockPopUpWindows
            // 
            this.checkBoxBlockPopUpWindows.AutoSize = true;
            this.checkBoxBlockPopUpWindows.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxBlockPopUpWindows.Location = new System.Drawing.Point(226, 370);
            this.checkBoxBlockPopUpWindows.Name = "checkBoxBlockPopUpWindows";
            this.checkBoxBlockPopUpWindows.Size = new System.Drawing.Size(169, 21);
            this.checkBoxBlockPopUpWindows.TabIndex = 71;
            this.checkBoxBlockPopUpWindows.Text = "Block pop-up windows";
            this.checkBoxBlockPopUpWindows.UseVisualStyleBackColor = true;
            this.checkBoxBlockPopUpWindows.CheckedChanged += new System.EventHandler(this.checkBoxBlockPopUpWindows_CheckedChanged);
            // 
            // checkBoxAllowBrowsingBackForward
            // 
            this.checkBoxAllowBrowsingBackForward.AutoSize = true;
            this.checkBoxAllowBrowsingBackForward.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxAllowBrowsingBackForward.Location = new System.Drawing.Point(487, 343);
            this.checkBoxAllowBrowsingBackForward.Name = "checkBoxAllowBrowsingBackForward";
            this.checkBoxAllowBrowsingBackForward.Size = new System.Drawing.Size(207, 21);
            this.checkBoxAllowBrowsingBackForward.TabIndex = 70;
            this.checkBoxAllowBrowsingBackForward.Text = "Allow browsing back/forward";
            this.checkBoxAllowBrowsingBackForward.UseVisualStyleBackColor = true;
            this.checkBoxAllowBrowsingBackForward.CheckedChanged += new System.EventHandler(this.checkBoxAllowBrowsingBackForward_CheckedChanged);
            // 
            // checkBoxEnableJavaScript
            // 
            this.checkBoxEnableJavaScript.AutoSize = true;
            this.checkBoxEnableJavaScript.Checked = true;
            this.checkBoxEnableJavaScript.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxEnableJavaScript.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxEnableJavaScript.Location = new System.Drawing.Point(226, 343);
            this.checkBoxEnableJavaScript.Name = "checkBoxEnableJavaScript";
            this.checkBoxEnableJavaScript.Size = new System.Drawing.Size(144, 21);
            this.checkBoxEnableJavaScript.TabIndex = 69;
            this.checkBoxEnableJavaScript.Text = "Enable JavaScript";
            this.checkBoxEnableJavaScript.UseVisualStyleBackColor = true;
            this.checkBoxEnableJavaScript.CheckedChanged += new System.EventHandler(this.checkBoxEnableJavaScript_CheckedChanged);
            // 
            // checkBoxEnableJava
            // 
            this.checkBoxEnableJava.AutoSize = true;
            this.checkBoxEnableJava.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxEnableJava.Location = new System.Drawing.Point(31, 370);
            this.checkBoxEnableJava.Name = "checkBoxEnableJava";
            this.checkBoxEnableJava.Size = new System.Drawing.Size(108, 21);
            this.checkBoxEnableJava.TabIndex = 68;
            this.checkBoxEnableJava.Text = "Enable Java";
            this.checkBoxEnableJava.UseVisualStyleBackColor = true;
            this.checkBoxEnableJava.CheckedChanged += new System.EventHandler(this.checkBoxEnableJava_CheckedChanged);
            // 
            // checkBoxEnablePlugIns
            // 
            this.checkBoxEnablePlugIns.AutoSize = true;
            this.checkBoxEnablePlugIns.Checked = true;
            this.checkBoxEnablePlugIns.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxEnablePlugIns.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxEnablePlugIns.Location = new System.Drawing.Point(31, 343);
            this.checkBoxEnablePlugIns.Name = "checkBoxEnablePlugIns";
            this.checkBoxEnablePlugIns.Size = new System.Drawing.Size(128, 21);
            this.checkBoxEnablePlugIns.TabIndex = 67;
            this.checkBoxEnablePlugIns.Text = "Enable plug-ins";
            this.checkBoxEnablePlugIns.UseVisualStyleBackColor = true;
            this.checkBoxEnablePlugIns.CheckedChanged += new System.EventHandler(this.checkBoxEnablePlugins_CheckedChanged);
            // 
            // checkBoxUseSebWithoutBrowser
            // 
            this.checkBoxUseSebWithoutBrowser.AutoSize = true;
            this.checkBoxUseSebWithoutBrowser.Checked = true;
            this.checkBoxUseSebWithoutBrowser.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxUseSebWithoutBrowser.Location = new System.Drawing.Point(31, 404);
            this.checkBoxUseSebWithoutBrowser.Name = "checkBoxUseSebWithoutBrowser";
            this.checkBoxUseSebWithoutBrowser.Size = new System.Drawing.Size(237, 21);
            this.checkBoxUseSebWithoutBrowser.TabIndex = 66;
            this.checkBoxUseSebWithoutBrowser.Text = "Use SEB without browser window";
            this.checkBoxUseSebWithoutBrowser.UseVisualStyleBackColor = true;
            this.checkBoxUseSebWithoutBrowser.CheckedChanged += new System.EventHandler(this.checkBoxUseSebWithoutBrowser_CheckedChanged);
            // 
            // checkBoxBlockLinksJava
            // 
            this.checkBoxBlockLinksJava.AutoSize = true;
            this.checkBoxBlockLinksJava.Location = new System.Drawing.Point(323, 276);
            this.checkBoxBlockLinksJava.Name = "checkBoxBlockLinksJava";
            this.checkBoxBlockLinksJava.Size = new System.Drawing.Size(286, 21);
            this.checkBoxBlockLinksJava.TabIndex = 62;
            this.checkBoxBlockLinksJava.Text = "block when directing to a different server";
            this.checkBoxBlockLinksJava.UseVisualStyleBackColor = true;
            this.checkBoxBlockLinksJava.CheckedChanged += new System.EventHandler(this.checkBoxBlockLinksJava_CheckedChanged);
            // 
            // labelOpenLinksJava
            // 
            this.labelOpenLinksJava.AutoSize = true;
            this.labelOpenLinksJava.Location = new System.Drawing.Point(28, 248);
            this.labelOpenLinksJava.Name = "labelOpenLinksJava";
            this.labelOpenLinksJava.Size = new System.Drawing.Size(200, 17);
            this.labelOpenLinksJava.TabIndex = 61;
            this.labelOpenLinksJava.Text = "Links in JavaScript / plug-ins...";
            // 
            // labelOpenLinksHTML
            // 
            this.labelOpenLinksHTML.AutoSize = true;
            this.labelOpenLinksHTML.Location = new System.Drawing.Point(28, 19);
            this.labelOpenLinksHTML.Name = "labelOpenLinksHTML";
            this.labelOpenLinksHTML.Size = new System.Drawing.Size(371, 17);
            this.labelOpenLinksHTML.TabIndex = 60;
            this.labelOpenLinksHTML.Text = "Links requesting to be opened in a new browser window...";
            // 
            // checkBoxBlockLinksHTML
            // 
            this.checkBoxBlockLinksHTML.AutoSize = true;
            this.checkBoxBlockLinksHTML.Location = new System.Drawing.Point(323, 51);
            this.checkBoxBlockLinksHTML.Name = "checkBoxBlockLinksHTML";
            this.checkBoxBlockLinksHTML.Size = new System.Drawing.Size(286, 21);
            this.checkBoxBlockLinksHTML.TabIndex = 59;
            this.checkBoxBlockLinksHTML.Text = "block when directing to a different server";
            this.checkBoxBlockLinksHTML.UseVisualStyleBackColor = true;
            this.checkBoxBlockLinksHTML.CheckedChanged += new System.EventHandler(this.checkBoxBlockLinksHTML_CheckedChanged);
            // 
            // groupBoxNewBrowserWindow
            // 
            this.groupBoxNewBrowserWindow.Controls.Add(this.comboBoxNewBrowserWindowHeight);
            this.groupBoxNewBrowserWindow.Controls.Add(this.comboBoxNewBrowserWindowWidth);
            this.groupBoxNewBrowserWindow.Controls.Add(this.labelNewWindowHeight);
            this.groupBoxNewBrowserWindow.Controls.Add(this.labelNewWindowWidth);
            this.groupBoxNewBrowserWindow.Controls.Add(this.labelNewWindowPosition);
            this.groupBoxNewBrowserWindow.Controls.Add(this.listBoxNewBrowserWindowPositioning);
            this.groupBoxNewBrowserWindow.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxNewBrowserWindow.Location = new System.Drawing.Point(31, 119);
            this.groupBoxNewBrowserWindow.Name = "groupBoxNewBrowserWindow";
            this.groupBoxNewBrowserWindow.Size = new System.Drawing.Size(885, 111);
            this.groupBoxNewBrowserWindow.TabIndex = 58;
            this.groupBoxNewBrowserWindow.TabStop = false;
            this.groupBoxNewBrowserWindow.Text = "New browser window size and position";
            // 
            // comboBoxNewBrowserWindowHeight
            // 
            this.comboBoxNewBrowserWindowHeight.FormattingEnabled = true;
            this.comboBoxNewBrowserWindowHeight.Location = new System.Drawing.Point(88, 72);
            this.comboBoxNewBrowserWindowHeight.Name = "comboBoxNewBrowserWindowHeight";
            this.comboBoxNewBrowserWindowHeight.Size = new System.Drawing.Size(121, 24);
            this.comboBoxNewBrowserWindowHeight.TabIndex = 63;
            this.comboBoxNewBrowserWindowHeight.SelectedIndexChanged += new System.EventHandler(this.comboBoxNewBrowserWindowHeight_SelectedIndexChanged);
            this.comboBoxNewBrowserWindowHeight.TextUpdate += new System.EventHandler(this.comboBoxNewBrowserWindowHeight_TextUpdate);
            // 
            // comboBoxNewBrowserWindowWidth
            // 
            this.comboBoxNewBrowserWindowWidth.FormattingEnabled = true;
            this.comboBoxNewBrowserWindowWidth.Location = new System.Drawing.Point(88, 34);
            this.comboBoxNewBrowserWindowWidth.Name = "comboBoxNewBrowserWindowWidth";
            this.comboBoxNewBrowserWindowWidth.Size = new System.Drawing.Size(121, 24);
            this.comboBoxNewBrowserWindowWidth.TabIndex = 62;
            this.comboBoxNewBrowserWindowWidth.SelectedIndexChanged += new System.EventHandler(this.comboBoxNewBrowserWindowWidth_SelectedIndexChanged);
            this.comboBoxNewBrowserWindowWidth.TextUpdate += new System.EventHandler(this.comboBoxNewBrowserWindowWidth_TextUpdate);
            // 
            // labelNewWindowHeight
            // 
            this.labelNewWindowHeight.AutoSize = true;
            this.labelNewWindowHeight.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelNewWindowHeight.Location = new System.Drawing.Point(29, 72);
            this.labelNewWindowHeight.Name = "labelNewWindowHeight";
            this.labelNewWindowHeight.Size = new System.Drawing.Size(49, 17);
            this.labelNewWindowHeight.TabIndex = 61;
            this.labelNewWindowHeight.Text = "Height";
            // 
            // labelNewWindowWidth
            // 
            this.labelNewWindowWidth.AutoSize = true;
            this.labelNewWindowWidth.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelNewWindowWidth.Location = new System.Drawing.Point(29, 37);
            this.labelNewWindowWidth.Name = "labelNewWindowWidth";
            this.labelNewWindowWidth.Size = new System.Drawing.Size(44, 17);
            this.labelNewWindowWidth.TabIndex = 60;
            this.labelNewWindowWidth.Text = "Width";
            // 
            // labelNewWindowPosition
            // 
            this.labelNewWindowPosition.AutoSize = true;
            this.labelNewWindowPosition.Location = new System.Drawing.Point(308, 41);
            this.labelNewWindowPosition.Name = "labelNewWindowPosition";
            this.labelNewWindowPosition.Size = new System.Drawing.Size(144, 17);
            this.labelNewWindowPosition.TabIndex = 58;
            this.labelNewWindowPosition.Text = "Horizontal positioning";
            // 
            // listBoxNewBrowserWindowPositioning
            // 
            this.listBoxNewBrowserWindowPositioning.FormattingEnabled = true;
            this.listBoxNewBrowserWindowPositioning.ItemHeight = 16;
            this.listBoxNewBrowserWindowPositioning.Location = new System.Drawing.Point(458, 21);
            this.listBoxNewBrowserWindowPositioning.Name = "listBoxNewBrowserWindowPositioning";
            this.listBoxNewBrowserWindowPositioning.Size = new System.Drawing.Size(120, 52);
            this.listBoxNewBrowserWindowPositioning.TabIndex = 57;
            this.listBoxNewBrowserWindowPositioning.SelectedIndexChanged += new System.EventHandler(this.listBoxNewBrowserWindowPositioning_SelectedIndexChanged);
            // 
            // tabPageAppearance
            // 
            this.tabPageAppearance.Controls.Add(this.groupBoxMainBrowserWindow);
            this.tabPageAppearance.Controls.Add(this.checkBoxShowTaskBar);
            this.tabPageAppearance.Controls.Add(this.checkBoxShowMenuBar);
            this.tabPageAppearance.Controls.Add(this.checkBoxHideBrowserWindowToolbar);
            this.tabPageAppearance.Controls.Add(this.checkBoxEnableBrowserWindowToolbar);
            this.tabPageAppearance.Controls.Add(this.radioButtonUseFullScreenMode);
            this.tabPageAppearance.Controls.Add(this.radioButtonUseBrowserWindow);
            this.tabPageAppearance.Location = new System.Drawing.Point(4, 39);
            this.tabPageAppearance.Name = "tabPageAppearance";
            this.tabPageAppearance.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageAppearance.Size = new System.Drawing.Size(1092, 857);
            this.tabPageAppearance.TabIndex = 8;
            this.tabPageAppearance.Text = "Appearance";
            this.tabPageAppearance.UseVisualStyleBackColor = true;
            // 
            // groupBoxMainBrowserWindow
            // 
            this.groupBoxMainBrowserWindow.Controls.Add(this.comboBoxMainBrowserWindowHeight);
            this.groupBoxMainBrowserWindow.Controls.Add(this.comboBoxMainBrowserWindowWidth);
            this.groupBoxMainBrowserWindow.Controls.Add(this.labelMainWindowHeight);
            this.groupBoxMainBrowserWindow.Controls.Add(this.labelMainWindowWidth);
            this.groupBoxMainBrowserWindow.Controls.Add(this.labelMainWindowPosition);
            this.groupBoxMainBrowserWindow.Controls.Add(this.listBoxMainBrowserWindowPositioning);
            this.groupBoxMainBrowserWindow.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxMainBrowserWindow.Location = new System.Drawing.Point(31, 115);
            this.groupBoxMainBrowserWindow.Name = "groupBoxMainBrowserWindow";
            this.groupBoxMainBrowserWindow.Size = new System.Drawing.Size(885, 111);
            this.groupBoxMainBrowserWindow.TabIndex = 57;
            this.groupBoxMainBrowserWindow.TabStop = false;
            this.groupBoxMainBrowserWindow.Text = "Main browser window size and position";
            // 
            // comboBoxMainBrowserWindowHeight
            // 
            this.comboBoxMainBrowserWindowHeight.FormattingEnabled = true;
            this.comboBoxMainBrowserWindowHeight.Location = new System.Drawing.Point(95, 69);
            this.comboBoxMainBrowserWindowHeight.Name = "comboBoxMainBrowserWindowHeight";
            this.comboBoxMainBrowserWindowHeight.Size = new System.Drawing.Size(121, 24);
            this.comboBoxMainBrowserWindowHeight.TabIndex = 62;
            this.comboBoxMainBrowserWindowHeight.SelectedIndexChanged += new System.EventHandler(this.comboBoxMainBrowserWindowHeight_SelectedIndexChanged);
            this.comboBoxMainBrowserWindowHeight.TextUpdate += new System.EventHandler(this.comboBoxMainBrowserWindowHeight_TextUpdate);
            // 
            // comboBoxMainBrowserWindowWidth
            // 
            this.comboBoxMainBrowserWindowWidth.FormattingEnabled = true;
            this.comboBoxMainBrowserWindowWidth.Location = new System.Drawing.Point(95, 34);
            this.comboBoxMainBrowserWindowWidth.Name = "comboBoxMainBrowserWindowWidth";
            this.comboBoxMainBrowserWindowWidth.Size = new System.Drawing.Size(121, 24);
            this.comboBoxMainBrowserWindowWidth.TabIndex = 61;
            this.comboBoxMainBrowserWindowWidth.SelectedIndexChanged += new System.EventHandler(this.comboBoxMainBrowserWindowWidth_SelectedIndexChanged);
            this.comboBoxMainBrowserWindowWidth.TextUpdate += new System.EventHandler(this.comboBoxMainBrowserWindowWidth_TextUpdate);
            // 
            // labelMainWindowHeight
            // 
            this.labelMainWindowHeight.AutoSize = true;
            this.labelMainWindowHeight.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelMainWindowHeight.Location = new System.Drawing.Point(32, 72);
            this.labelMainWindowHeight.Name = "labelMainWindowHeight";
            this.labelMainWindowHeight.Size = new System.Drawing.Size(49, 17);
            this.labelMainWindowHeight.TabIndex = 60;
            this.labelMainWindowHeight.Text = "Height";
            // 
            // labelMainWindowWidth
            // 
            this.labelMainWindowWidth.AutoSize = true;
            this.labelMainWindowWidth.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelMainWindowWidth.Location = new System.Drawing.Point(32, 37);
            this.labelMainWindowWidth.Name = "labelMainWindowWidth";
            this.labelMainWindowWidth.Size = new System.Drawing.Size(44, 17);
            this.labelMainWindowWidth.TabIndex = 59;
            this.labelMainWindowWidth.Text = "Width";
            // 
            // labelMainWindowPosition
            // 
            this.labelMainWindowPosition.AutoSize = true;
            this.labelMainWindowPosition.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelMainWindowPosition.Location = new System.Drawing.Point(318, 34);
            this.labelMainWindowPosition.Name = "labelMainWindowPosition";
            this.labelMainWindowPosition.Size = new System.Drawing.Size(144, 17);
            this.labelMainWindowPosition.TabIndex = 58;
            this.labelMainWindowPosition.Text = "Horizontal positioning";
            // 
            // listBoxMainBrowserWindowPositioning
            // 
            this.listBoxMainBrowserWindowPositioning.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listBoxMainBrowserWindowPositioning.FormattingEnabled = true;
            this.listBoxMainBrowserWindowPositioning.ItemHeight = 16;
            this.listBoxMainBrowserWindowPositioning.Location = new System.Drawing.Point(468, 25);
            this.listBoxMainBrowserWindowPositioning.Name = "listBoxMainBrowserWindowPositioning";
            this.listBoxMainBrowserWindowPositioning.Size = new System.Drawing.Size(120, 52);
            this.listBoxMainBrowserWindowPositioning.TabIndex = 57;
            this.listBoxMainBrowserWindowPositioning.SelectedIndexChanged += new System.EventHandler(this.listBoxMainBrowserWindowPositioning_SelectedIndexChanged);
            // 
            // checkBoxShowTaskBar
            // 
            this.checkBoxShowTaskBar.AutoSize = true;
            this.checkBoxShowTaskBar.Location = new System.Drawing.Point(31, 357);
            this.checkBoxShowTaskBar.Name = "checkBoxShowTaskBar";
            this.checkBoxShowTaskBar.Size = new System.Drawing.Size(418, 21);
            this.checkBoxShowTaskBar.TabIndex = 56;
            this.checkBoxShowTaskBar.Text = "Display SEB dock/task bar when using third party applications";
            this.checkBoxShowTaskBar.UseVisualStyleBackColor = true;
            this.checkBoxShowTaskBar.CheckedChanged += new System.EventHandler(this.checkBoxShowTaskBar_CheckedChanged);
            // 
            // checkBoxShowMenuBar
            // 
            this.checkBoxShowMenuBar.AutoSize = true;
            this.checkBoxShowMenuBar.Location = new System.Drawing.Point(31, 330);
            this.checkBoxShowMenuBar.Name = "checkBoxShowMenuBar";
            this.checkBoxShowMenuBar.Size = new System.Drawing.Size(128, 21);
            this.checkBoxShowMenuBar.TabIndex = 55;
            this.checkBoxShowMenuBar.Text = "Show menu bar";
            this.checkBoxShowMenuBar.UseVisualStyleBackColor = true;
            this.checkBoxShowMenuBar.CheckedChanged += new System.EventHandler(this.checkBoxShowMenuBar_CheckedChanged);
            // 
            // checkBoxHideBrowserWindowToolbar
            // 
            this.checkBoxHideBrowserWindowToolbar.AutoSize = true;
            this.checkBoxHideBrowserWindowToolbar.Enabled = false;
            this.checkBoxHideBrowserWindowToolbar.Location = new System.Drawing.Point(51, 286);
            this.checkBoxHideBrowserWindowToolbar.Name = "checkBoxHideBrowserWindowToolbar";
            this.checkBoxHideBrowserWindowToolbar.Size = new System.Drawing.Size(173, 21);
            this.checkBoxHideBrowserWindowToolbar.TabIndex = 54;
            this.checkBoxHideBrowserWindowToolbar.Text = "Hide toolbar as default";
            this.checkBoxHideBrowserWindowToolbar.UseVisualStyleBackColor = true;
            this.checkBoxHideBrowserWindowToolbar.CheckedChanged += new System.EventHandler(this.checkBoxHideBrowserWindowToolbar_CheckedChanged);
            // 
            // checkBoxEnableBrowserWindowToolbar
            // 
            this.checkBoxEnableBrowserWindowToolbar.AutoSize = true;
            this.checkBoxEnableBrowserWindowToolbar.Location = new System.Drawing.Point(31, 259);
            this.checkBoxEnableBrowserWindowToolbar.Name = "checkBoxEnableBrowserWindowToolbar";
            this.checkBoxEnableBrowserWindowToolbar.Size = new System.Drawing.Size(225, 21);
            this.checkBoxEnableBrowserWindowToolbar.TabIndex = 53;
            this.checkBoxEnableBrowserWindowToolbar.Text = "Enable browser window toolbar";
            this.checkBoxEnableBrowserWindowToolbar.UseVisualStyleBackColor = true;
            this.checkBoxEnableBrowserWindowToolbar.CheckedChanged += new System.EventHandler(this.checkBoxEnableBrowserWindowToolbar_CheckedChanged);
            // 
            // radioButtonUseFullScreenMode
            // 
            this.radioButtonUseFullScreenMode.AutoSize = true;
            this.radioButtonUseFullScreenMode.Location = new System.Drawing.Point(31, 62);
            this.radioButtonUseFullScreenMode.Name = "radioButtonUseFullScreenMode";
            this.radioButtonUseFullScreenMode.Size = new System.Drawing.Size(162, 21);
            this.radioButtonUseFullScreenMode.TabIndex = 52;
            this.radioButtonUseFullScreenMode.TabStop = true;
            this.radioButtonUseFullScreenMode.Text = "Use full screen mode";
            this.radioButtonUseFullScreenMode.UseVisualStyleBackColor = true;
            this.radioButtonUseFullScreenMode.CheckedChanged += new System.EventHandler(this.radioButtonUseFullScreenMode_CheckedChanged);
            // 
            // radioButtonUseBrowserWindow
            // 
            this.radioButtonUseBrowserWindow.AutoSize = true;
            this.radioButtonUseBrowserWindow.Checked = true;
            this.radioButtonUseBrowserWindow.Location = new System.Drawing.Point(31, 35);
            this.radioButtonUseBrowserWindow.Name = "radioButtonUseBrowserWindow";
            this.radioButtonUseBrowserWindow.Size = new System.Drawing.Size(157, 21);
            this.radioButtonUseBrowserWindow.TabIndex = 51;
            this.radioButtonUseBrowserWindow.TabStop = true;
            this.radioButtonUseBrowserWindow.Text = "Use browser window";
            this.radioButtonUseBrowserWindow.UseVisualStyleBackColor = true;
            this.radioButtonUseBrowserWindow.CheckedChanged += new System.EventHandler(this.radioButtonUseBrowserWindow_CheckedChanged);
            // 
            // tabPageConfigFile
            // 
            this.tabPageConfigFile.Controls.Add(this.textBoxHashedSettingsPassword);
            this.tabPageConfigFile.Controls.Add(this.labelHashedSettingsPassword);
            this.tabPageConfigFile.Controls.Add(this.labelUseEither);
            this.tabPageConfigFile.Controls.Add(this.labelChooseIdentity);
            this.tabPageConfigFile.Controls.Add(this.comboBoxCryptoIdentity);
            this.tabPageConfigFile.Controls.Add(this.labelConfirmSettingsPassword);
            this.tabPageConfigFile.Controls.Add(this.labelSettingsPassword);
            this.tabPageConfigFile.Controls.Add(this.textBoxConfirmSettingsPassword);
            this.tabPageConfigFile.Controls.Add(this.textBoxSettingsPassword);
            this.tabPageConfigFile.Controls.Add(this.labelUseSEBSettingsFileFor);
            this.tabPageConfigFile.Controls.Add(this.radioButtonConfiguringAClient);
            this.tabPageConfigFile.Controls.Add(this.radioButtonStartingAnExam);
            this.tabPageConfigFile.Controls.Add(this.checkBoxAllowPreferencesWindow);
            this.tabPageConfigFile.Controls.Add(this.buttonDefaultSettings);
            this.tabPageConfigFile.Controls.Add(this.buttonRevertToLastOpened);
            this.tabPageConfigFile.Controls.Add(this.labelOpenSettings);
            this.tabPageConfigFile.Controls.Add(this.labelSaveSettingsAs);
            this.tabPageConfigFile.ImageIndex = 3;
            this.tabPageConfigFile.Location = new System.Drawing.Point(4, 39);
            this.tabPageConfigFile.Name = "tabPageConfigFile";
            this.tabPageConfigFile.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageConfigFile.Size = new System.Drawing.Size(1092, 857);
            this.tabPageConfigFile.TabIndex = 6;
            this.tabPageConfigFile.Text = "Config File";
            this.tabPageConfigFile.UseVisualStyleBackColor = true;
            // 
            // textBoxHashedSettingsPassword
            // 
            this.textBoxHashedSettingsPassword.Font = new System.Drawing.Font("Courier New", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxHashedSettingsPassword.Location = new System.Drawing.Point(465, 311);
            this.textBoxHashedSettingsPassword.Name = "textBoxHashedSettingsPassword";
            this.textBoxHashedSettingsPassword.ReadOnly = true;
            this.textBoxHashedSettingsPassword.Size = new System.Drawing.Size(441, 22);
            this.textBoxHashedSettingsPassword.TabIndex = 61;
            // 
            // labelHashedSettingsPassword
            // 
            this.labelHashedSettingsPassword.AutoSize = true;
            this.labelHashedSettingsPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelHashedSettingsPassword.Location = new System.Drawing.Point(277, 316);
            this.labelHashedSettingsPassword.Name = "labelHashedSettingsPassword";
            this.labelHashedSettingsPassword.Size = new System.Drawing.Size(174, 17);
            this.labelHashedSettingsPassword.TabIndex = 60;
            this.labelHashedSettingsPassword.Text = "Hashed settings password";
            // 
            // labelUseEither
            // 
            this.labelUseEither.AutoSize = true;
            this.labelUseEither.Location = new System.Drawing.Point(36, 214);
            this.labelUseEither.Name = "labelUseEither";
            this.labelUseEither.Size = new System.Drawing.Size(366, 17);
            this.labelUseEither.TabIndex = 59;
            this.labelUseEither.Text = "Use either a cryptographic identity or a password or both";
            // 
            // labelChooseIdentity
            // 
            this.labelChooseIdentity.AutoSize = true;
            this.labelChooseIdentity.Location = new System.Drawing.Point(36, 167);
            this.labelChooseIdentity.Name = "labelChooseIdentity";
            this.labelChooseIdentity.Size = new System.Drawing.Size(385, 17);
            this.labelChooseIdentity.TabIndex = 58;
            this.labelChooseIdentity.Text = "Choose identity to be used for encrypting SEB settings file...";
            // 
            // comboBoxCryptoIdentity
            // 
            this.comboBoxCryptoIdentity.FormattingEnabled = true;
            this.comboBoxCryptoIdentity.Location = new System.Drawing.Point(39, 187);
            this.comboBoxCryptoIdentity.Name = "comboBoxCryptoIdentity";
            this.comboBoxCryptoIdentity.Size = new System.Drawing.Size(657, 24);
            this.comboBoxCryptoIdentity.TabIndex = 57;
            this.comboBoxCryptoIdentity.SelectedIndexChanged += new System.EventHandler(this.comboBoxCryptoIdentity_SelectedIndexChanged);
            this.comboBoxCryptoIdentity.TextUpdate += new System.EventHandler(this.comboBoxCryptoIdentity_TextUpdate);
            // 
            // labelConfirmSettingsPassword
            // 
            this.labelConfirmSettingsPassword.AutoSize = true;
            this.labelConfirmSettingsPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelConfirmSettingsPassword.Location = new System.Drawing.Point(277, 288);
            this.labelConfirmSettingsPassword.Name = "labelConfirmSettingsPassword";
            this.labelConfirmSettingsPassword.Size = new System.Drawing.Size(173, 17);
            this.labelConfirmSettingsPassword.TabIndex = 56;
            this.labelConfirmSettingsPassword.Text = "Confirm settings password";
            // 
            // labelSettingsPassword
            // 
            this.labelSettingsPassword.AutoSize = true;
            this.labelSettingsPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelSettingsPassword.Location = new System.Drawing.Point(327, 260);
            this.labelSettingsPassword.Name = "labelSettingsPassword";
            this.labelSettingsPassword.Size = new System.Drawing.Size(123, 17);
            this.labelSettingsPassword.TabIndex = 55;
            this.labelSettingsPassword.Text = "Settings password";
            // 
            // textBoxConfirmSettingsPassword
            // 
            this.textBoxConfirmSettingsPassword.Font = new System.Drawing.Font("Courier New", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxConfirmSettingsPassword.Location = new System.Drawing.Point(465, 283);
            this.textBoxConfirmSettingsPassword.Name = "textBoxConfirmSettingsPassword";
            this.textBoxConfirmSettingsPassword.Size = new System.Drawing.Size(231, 22);
            this.textBoxConfirmSettingsPassword.TabIndex = 54;
            this.textBoxConfirmSettingsPassword.WordWrap = false;
            this.textBoxConfirmSettingsPassword.TextChanged += new System.EventHandler(this.textBoxConfirmSettingsPassword_TextChanged);
            // 
            // textBoxSettingsPassword
            // 
            this.textBoxSettingsPassword.Font = new System.Drawing.Font("Courier New", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxSettingsPassword.Location = new System.Drawing.Point(465, 255);
            this.textBoxSettingsPassword.Name = "textBoxSettingsPassword";
            this.textBoxSettingsPassword.Size = new System.Drawing.Size(231, 22);
            this.textBoxSettingsPassword.TabIndex = 53;
            this.textBoxSettingsPassword.WordWrap = false;
            this.textBoxSettingsPassword.TextChanged += new System.EventHandler(this.textBoxSettingsPassword_TextChanged);
            // 
            // labelUseSEBSettingsFileFor
            // 
            this.labelUseSEBSettingsFileFor.AutoSize = true;
            this.labelUseSEBSettingsFileFor.Location = new System.Drawing.Point(36, 32);
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
            // checkBoxAllowPreferencesWindow
            // 
            this.checkBoxAllowPreferencesWindow.AutoSize = true;
            this.checkBoxAllowPreferencesWindow.Checked = true;
            this.checkBoxAllowPreferencesWindow.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxAllowPreferencesWindow.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxAllowPreferencesWindow.Location = new System.Drawing.Point(60, 117);
            this.checkBoxAllowPreferencesWindow.Name = "checkBoxAllowPreferencesWindow";
            this.checkBoxAllowPreferencesWindow.Size = new System.Drawing.Size(300, 21);
            this.checkBoxAllowPreferencesWindow.TabIndex = 49;
            this.checkBoxAllowPreferencesWindow.Text = "Allow to open preferences window on client";
            this.checkBoxAllowPreferencesWindow.UseVisualStyleBackColor = true;
            this.checkBoxAllowPreferencesWindow.CheckedChanged += new System.EventHandler(this.checkBoxAllowPreferencesWindow_CheckedChanged);
            // 
            // buttonDefaultSettings
            // 
            this.buttonDefaultSettings.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonDefaultSettings.Location = new System.Drawing.Point(39, 349);
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
            this.buttonRevertToLastOpened.Location = new System.Drawing.Point(39, 383);
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
            this.labelOpenSettings.Location = new System.Drawing.Point(330, 367);
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
            this.labelSaveSettingsAs.Location = new System.Drawing.Point(465, 367);
            this.labelSaveSettingsAs.Name = "labelSaveSettingsAs";
            this.labelSaveSettingsAs.Size = new System.Drawing.Size(129, 19);
            this.labelSaveSettingsAs.TabIndex = 10;
            this.labelSaveSettingsAs.Text = "Save Settings As...";
            this.labelSaveSettingsAs.Click += new System.EventHandler(this.labelSaveSettingsAs_Click);
            // 
            // tabPageGeneral
            // 
            this.tabPageGeneral.Controls.Add(this.labelHashedAdminPassword);
            this.tabPageGeneral.Controls.Add(this.textBoxHashedAdminPassword);
            this.tabPageGeneral.Controls.Add(this.groupBoxExitSequence);
            this.tabPageGeneral.Controls.Add(this.checkBoxIgnoreQuitPassword);
            this.tabPageGeneral.Controls.Add(this.buttonPasteFromSavedClipboard);
            this.tabPageGeneral.Controls.Add(this.labelSEBServerURL);
            this.tabPageGeneral.Controls.Add(this.textBoxSebServerURL);
            this.tabPageGeneral.Controls.Add(this.textBoxConfirmAdminPassword);
            this.tabPageGeneral.Controls.Add(this.textBoxAdminPassword);
            this.tabPageGeneral.Controls.Add(this.textBoxConfirmQuitPassword);
            this.tabPageGeneral.Controls.Add(this.textBoxHashedQuitPassword);
            this.tabPageGeneral.Controls.Add(this.textBoxQuitPassword);
            this.tabPageGeneral.Controls.Add(this.textBoxStartURL);
            this.tabPageGeneral.Controls.Add(this.buttonHelp);
            this.tabPageGeneral.Controls.Add(this.buttonRestartSEB);
            this.tabPageGeneral.Controls.Add(this.buttonQuit);
            this.tabPageGeneral.Controls.Add(this.buttonAbout);
            this.tabPageGeneral.Controls.Add(this.labelConfirmAdminPassword);
            this.tabPageGeneral.Controls.Add(this.labelAdminPassword);
            this.tabPageGeneral.Controls.Add(this.labelConfirmQuitPassword);
            this.tabPageGeneral.Controls.Add(this.checkBoxAllowQuit);
            this.tabPageGeneral.Controls.Add(this.labelHashedQuitPassword);
            this.tabPageGeneral.Controls.Add(this.labelQuitPassword);
            this.tabPageGeneral.Controls.Add(this.labelStartURL);
            this.tabPageGeneral.ImageIndex = 5;
            this.tabPageGeneral.Location = new System.Drawing.Point(4, 39);
            this.tabPageGeneral.Name = "tabPageGeneral";
            this.tabPageGeneral.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageGeneral.Size = new System.Drawing.Size(1092, 857);
            this.tabPageGeneral.TabIndex = 4;
            this.tabPageGeneral.Text = "General";
            this.tabPageGeneral.UseVisualStyleBackColor = true;
            // 
            // labelHashedAdminPassword
            // 
            this.labelHashedAdminPassword.AutoSize = true;
            this.labelHashedAdminPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelHashedAdminPassword.Location = new System.Drawing.Point(21, 166);
            this.labelHashedAdminPassword.Name = "labelHashedAdminPassword";
            this.labelHashedAdminPassword.Size = new System.Drawing.Size(207, 17);
            this.labelHashedAdminPassword.TabIndex = 55;
            this.labelHashedAdminPassword.Text = "Hashed administrator password";
            // 
            // textBoxHashedAdminPassword
            // 
            this.textBoxHashedAdminPassword.Font = new System.Drawing.Font("Courier New", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxHashedAdminPassword.Location = new System.Drawing.Point(234, 166);
            this.textBoxHashedAdminPassword.Name = "textBoxHashedAdminPassword";
            this.textBoxHashedAdminPassword.ReadOnly = true;
            this.textBoxHashedAdminPassword.Size = new System.Drawing.Size(441, 22);
            this.textBoxHashedAdminPassword.TabIndex = 54;
            // 
            // groupBoxExitSequence
            // 
            this.groupBoxExitSequence.Controls.Add(this.listBoxExitKey1);
            this.groupBoxExitSequence.Controls.Add(this.listBoxExitKey3);
            this.groupBoxExitSequence.Controls.Add(this.listBoxExitKey2);
            this.groupBoxExitSequence.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxExitSequence.Location = new System.Drawing.Point(736, 120);
            this.groupBoxExitSequence.Name = "groupBoxExitSequence";
            this.groupBoxExitSequence.Size = new System.Drawing.Size(160, 240);
            this.groupBoxExitSequence.TabIndex = 53;
            this.groupBoxExitSequence.TabStop = false;
            this.groupBoxExitSequence.Text = "Exit Sequence";
            // 
            // listBoxExitKey1
            // 
            this.listBoxExitKey1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listBoxExitKey1.FormattingEnabled = true;
            this.listBoxExitKey1.ItemHeight = 16;
            this.listBoxExitKey1.Location = new System.Drawing.Point(10, 30);
            this.listBoxExitKey1.Name = "listBoxExitKey1";
            this.listBoxExitKey1.Size = new System.Drawing.Size(40, 196);
            this.listBoxExitKey1.TabIndex = 47;
            this.listBoxExitKey1.SelectedIndexChanged += new System.EventHandler(this.listBoxExitKey1_SelectedIndexChanged);
            // 
            // listBoxExitKey3
            // 
            this.listBoxExitKey3.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listBoxExitKey3.FormattingEnabled = true;
            this.listBoxExitKey3.ItemHeight = 16;
            this.listBoxExitKey3.Location = new System.Drawing.Point(110, 30);
            this.listBoxExitKey3.Name = "listBoxExitKey3";
            this.listBoxExitKey3.Size = new System.Drawing.Size(40, 196);
            this.listBoxExitKey3.TabIndex = 50;
            this.listBoxExitKey3.SelectedIndexChanged += new System.EventHandler(this.listBoxExitKey3_SelectedIndexChanged);
            // 
            // listBoxExitKey2
            // 
            this.listBoxExitKey2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listBoxExitKey2.FormattingEnabled = true;
            this.listBoxExitKey2.ItemHeight = 16;
            this.listBoxExitKey2.Location = new System.Drawing.Point(60, 30);
            this.listBoxExitKey2.Name = "listBoxExitKey2";
            this.listBoxExitKey2.Size = new System.Drawing.Size(40, 196);
            this.listBoxExitKey2.TabIndex = 49;
            this.listBoxExitKey2.SelectedIndexChanged += new System.EventHandler(this.listBoxExitKey2_SelectedIndexChanged);
            // 
            // checkBoxIgnoreQuitPassword
            // 
            this.checkBoxIgnoreQuitPassword.AutoSize = true;
            this.checkBoxIgnoreQuitPassword.Enabled = false;
            this.checkBoxIgnoreQuitPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxIgnoreQuitPassword.Location = new System.Drawing.Point(62, 225);
            this.checkBoxIgnoreQuitPassword.Name = "checkBoxIgnoreQuitPassword";
            this.checkBoxIgnoreQuitPassword.Size = new System.Drawing.Size(161, 21);
            this.checkBoxIgnoreQuitPassword.TabIndex = 49;
            this.checkBoxIgnoreQuitPassword.Text = "Ignore quit password";
            this.checkBoxIgnoreQuitPassword.UseVisualStyleBackColor = true;
            this.checkBoxIgnoreQuitPassword.CheckedChanged += new System.EventHandler(this.checkBoxIgnoreQuitPassword_CheckedChanged);
            // 
            // buttonPasteFromSavedClipboard
            // 
            this.buttonPasteFromSavedClipboard.Location = new System.Drawing.Point(736, 36);
            this.buttonPasteFromSavedClipboard.Name = "buttonPasteFromSavedClipboard";
            this.buttonPasteFromSavedClipboard.Size = new System.Drawing.Size(210, 23);
            this.buttonPasteFromSavedClipboard.TabIndex = 48;
            this.buttonPasteFromSavedClipboard.Text = "Paste from saved clipboard";
            this.buttonPasteFromSavedClipboard.UseVisualStyleBackColor = true;
            this.buttonPasteFromSavedClipboard.Click += new System.EventHandler(this.buttonPasteFromSavedClipboard_Click);
            // 
            // labelSEBServerURL
            // 
            this.labelSEBServerURL.AutoSize = true;
            this.labelSEBServerURL.Enabled = false;
            this.labelSEBServerURL.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelSEBServerURL.Location = new System.Drawing.Point(21, 71);
            this.labelSEBServerURL.Name = "labelSEBServerURL";
            this.labelSEBServerURL.Size = new System.Drawing.Size(113, 17);
            this.labelSEBServerURL.TabIndex = 47;
            this.labelSEBServerURL.Text = "SEB Server URL";
            // 
            // textBoxSebServerURL
            // 
            this.textBoxSebServerURL.Enabled = false;
            this.textBoxSebServerURL.Font = new System.Drawing.Font("Courier New", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxSebServerURL.Location = new System.Drawing.Point(140, 66);
            this.textBoxSebServerURL.Name = "textBoxSebServerURL";
            this.textBoxSebServerURL.Size = new System.Drawing.Size(535, 22);
            this.textBoxSebServerURL.TabIndex = 46;
            this.textBoxSebServerURL.TextChanged += new System.EventHandler(this.textBoxSebServerURL_TextChanged);
            // 
            // textBoxConfirmAdminPassword
            // 
            this.textBoxConfirmAdminPassword.Font = new System.Drawing.Font("Courier New", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxConfirmAdminPassword.Location = new System.Drawing.Point(444, 138);
            this.textBoxConfirmAdminPassword.Name = "textBoxConfirmAdminPassword";
            this.textBoxConfirmAdminPassword.Size = new System.Drawing.Size(231, 22);
            this.textBoxConfirmAdminPassword.TabIndex = 41;
            this.textBoxConfirmAdminPassword.WordWrap = false;
            this.textBoxConfirmAdminPassword.TextChanged += new System.EventHandler(this.textBoxConfirmAdminPassword_TextChanged);
            // 
            // textBoxAdminPassword
            // 
            this.textBoxAdminPassword.Font = new System.Drawing.Font("Courier New", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxAdminPassword.Location = new System.Drawing.Point(444, 109);
            this.textBoxAdminPassword.Name = "textBoxAdminPassword";
            this.textBoxAdminPassword.Size = new System.Drawing.Size(231, 22);
            this.textBoxAdminPassword.TabIndex = 39;
            this.textBoxAdminPassword.WordWrap = false;
            this.textBoxAdminPassword.TextChanged += new System.EventHandler(this.textBoxAdminPassword_TextChanged);
            // 
            // textBoxConfirmQuitPassword
            // 
            this.textBoxConfirmQuitPassword.Font = new System.Drawing.Font("Courier New", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxConfirmQuitPassword.Location = new System.Drawing.Point(444, 261);
            this.textBoxConfirmQuitPassword.Name = "textBoxConfirmQuitPassword";
            this.textBoxConfirmQuitPassword.Size = new System.Drawing.Size(231, 22);
            this.textBoxConfirmQuitPassword.TabIndex = 37;
            this.textBoxConfirmQuitPassword.WordWrap = false;
            this.textBoxConfirmQuitPassword.TextChanged += new System.EventHandler(this.textBoxConfirmQuitPassword_TextChanged);
            // 
            // textBoxHashedQuitPassword
            // 
            this.textBoxHashedQuitPassword.Font = new System.Drawing.Font("Courier New", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxHashedQuitPassword.Location = new System.Drawing.Point(175, 289);
            this.textBoxHashedQuitPassword.Name = "textBoxHashedQuitPassword";
            this.textBoxHashedQuitPassword.ReadOnly = true;
            this.textBoxHashedQuitPassword.Size = new System.Drawing.Size(502, 22);
            this.textBoxHashedQuitPassword.TabIndex = 34;
            // 
            // textBoxQuitPassword
            // 
            this.textBoxQuitPassword.Font = new System.Drawing.Font("Courier New", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxQuitPassword.Location = new System.Drawing.Point(444, 233);
            this.textBoxQuitPassword.Name = "textBoxQuitPassword";
            this.textBoxQuitPassword.Size = new System.Drawing.Size(231, 22);
            this.textBoxQuitPassword.TabIndex = 33;
            this.textBoxQuitPassword.WordWrap = false;
            this.textBoxQuitPassword.TextChanged += new System.EventHandler(this.textBoxQuitPassword_TextChanged);
            // 
            // textBoxStartURL
            // 
            this.textBoxStartURL.Font = new System.Drawing.Font("Courier New", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxStartURL.Location = new System.Drawing.Point(140, 38);
            this.textBoxStartURL.Name = "textBoxStartURL";
            this.textBoxStartURL.Size = new System.Drawing.Size(535, 22);
            this.textBoxStartURL.TabIndex = 21;
            this.textBoxStartURL.TextChanged += new System.EventHandler(this.textBoxStartURL_TextChanged);
            // 
            // buttonHelp
            // 
            this.buttonHelp.Location = new System.Drawing.Point(258, 384);
            this.buttonHelp.Name = "buttonHelp";
            this.buttonHelp.Size = new System.Drawing.Size(75, 23);
            this.buttonHelp.TabIndex = 45;
            this.buttonHelp.Text = "Help";
            this.buttonHelp.UseVisualStyleBackColor = true;
            this.buttonHelp.Click += new System.EventHandler(this.buttonHelp_Click);
            // 
            // buttonRestartSEB
            // 
            this.buttonRestartSEB.Location = new System.Drawing.Point(572, 384);
            this.buttonRestartSEB.Name = "buttonRestartSEB";
            this.buttonRestartSEB.Size = new System.Drawing.Size(103, 23);
            this.buttonRestartSEB.TabIndex = 44;
            this.buttonRestartSEB.Text = "Restart SEB";
            this.buttonRestartSEB.UseVisualStyleBackColor = true;
            this.buttonRestartSEB.Click += new System.EventHandler(this.buttonRestartSEB_Click);
            // 
            // buttonQuit
            // 
            this.buttonQuit.Location = new System.Drawing.Point(413, 384);
            this.buttonQuit.Name = "buttonQuit";
            this.buttonQuit.Size = new System.Drawing.Size(75, 23);
            this.buttonQuit.TabIndex = 43;
            this.buttonQuit.Text = "Quit";
            this.buttonQuit.UseVisualStyleBackColor = true;
            this.buttonQuit.Click += new System.EventHandler(this.buttonQuit_Click);
            // 
            // buttonAbout
            // 
            this.buttonAbout.Location = new System.Drawing.Point(108, 384);
            this.buttonAbout.Name = "buttonAbout";
            this.buttonAbout.Size = new System.Drawing.Size(75, 23);
            this.buttonAbout.TabIndex = 42;
            this.buttonAbout.Text = "About";
            this.buttonAbout.UseVisualStyleBackColor = true;
            this.buttonAbout.Click += new System.EventHandler(this.buttonAbout_Click);
            // 
            // labelConfirmAdminPassword
            // 
            this.labelConfirmAdminPassword.AutoSize = true;
            this.labelConfirmAdminPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelConfirmAdminPassword.Location = new System.Drawing.Point(232, 138);
            this.labelConfirmAdminPassword.Name = "labelConfirmAdminPassword";
            this.labelConfirmAdminPassword.Size = new System.Drawing.Size(206, 17);
            this.labelConfirmAdminPassword.TabIndex = 40;
            this.labelConfirmAdminPassword.Text = "Confirm administrator password";
            // 
            // labelAdminPassword
            // 
            this.labelAdminPassword.AutoSize = true;
            this.labelAdminPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelAdminPassword.Location = new System.Drawing.Point(283, 109);
            this.labelAdminPassword.Name = "labelAdminPassword";
            this.labelAdminPassword.Size = new System.Drawing.Size(155, 17);
            this.labelAdminPassword.TabIndex = 38;
            this.labelAdminPassword.Text = "Administrator password";
            // 
            // labelConfirmQuitPassword
            // 
            this.labelConfirmQuitPassword.AutoSize = true;
            this.labelConfirmQuitPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelConfirmQuitPassword.Location = new System.Drawing.Point(291, 266);
            this.labelConfirmQuitPassword.Name = "labelConfirmQuitPassword";
            this.labelConfirmQuitPassword.Size = new System.Drawing.Size(147, 17);
            this.labelConfirmQuitPassword.TabIndex = 36;
            this.labelConfirmQuitPassword.Text = "Confirm quit password";
            // 
            // checkBoxAllowQuit
            // 
            this.checkBoxAllowQuit.AutoSize = true;
            this.checkBoxAllowQuit.Checked = true;
            this.checkBoxAllowQuit.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxAllowQuit.Location = new System.Drawing.Point(62, 198);
            this.checkBoxAllowQuit.Name = "checkBoxAllowQuit";
            this.checkBoxAllowQuit.Size = new System.Drawing.Size(168, 21);
            this.checkBoxAllowQuit.TabIndex = 35;
            this.checkBoxAllowQuit.Text = "Allow user to quit SEB";
            this.checkBoxAllowQuit.UseVisualStyleBackColor = true;
            this.checkBoxAllowQuit.CheckedChanged += new System.EventHandler(this.checkBoxAllowQuit_CheckedChanged);
            // 
            // labelHashedQuitPassword
            // 
            this.labelHashedQuitPassword.AutoSize = true;
            this.labelHashedQuitPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelHashedQuitPassword.Location = new System.Drawing.Point(21, 291);
            this.labelHashedQuitPassword.Name = "labelHashedQuitPassword";
            this.labelHashedQuitPassword.Size = new System.Drawing.Size(148, 17);
            this.labelHashedQuitPassword.TabIndex = 32;
            this.labelHashedQuitPassword.Text = "Hashed quit password";
            // 
            // labelQuitPassword
            // 
            this.labelQuitPassword.AutoSize = true;
            this.labelQuitPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelQuitPassword.Location = new System.Drawing.Point(340, 233);
            this.labelQuitPassword.Name = "labelQuitPassword";
            this.labelQuitPassword.Size = new System.Drawing.Size(98, 17);
            this.labelQuitPassword.TabIndex = 30;
            this.labelQuitPassword.Text = "Quit password";
            // 
            // labelStartURL
            // 
            this.labelStartURL.AutoSize = true;
            this.labelStartURL.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelStartURL.Location = new System.Drawing.Point(64, 43);
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
            this.tabControlSebWindowsConfig.ImageList = this.imageListTabIcons;
            this.tabControlSebWindowsConfig.Location = new System.Drawing.Point(45, 31);
            this.tabControlSebWindowsConfig.Name = "tabControlSebWindowsConfig";
            this.tabControlSebWindowsConfig.SelectedIndex = 0;
            this.tabControlSebWindowsConfig.Size = new System.Drawing.Size(1100, 800);
            this.tabControlSebWindowsConfig.TabIndex = 2;
            // 
            // SebWindowsConfigForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1382, 855);
            this.Controls.Add(this.tabControlSebWindowsConfig);
            this.Name = "SebWindowsConfigForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.Text = "SEB Windows Configuration Editor";
            this.tabPageHookedKeys.ResumeLayout(false);
            this.tabPageHookedKeys.PerformLayout();
            this.groupBoxFunctionKeys.ResumeLayout(false);
            this.groupBoxFunctionKeys.PerformLayout();
            this.groupBoxSpecialKeys.ResumeLayout(false);
            this.groupBoxSpecialKeys.PerformLayout();
            this.tabPageRegistry.ResumeLayout(false);
            this.groupBoxOutsideSeb.ResumeLayout(false);
            this.groupBoxOutsideSeb.PerformLayout();
            this.groupBoxSetOutsideSebValues.ResumeLayout(false);
            this.groupBoxSetOutsideSebValues.PerformLayout();
            this.groupBoxInsideSeb.ResumeLayout(false);
            this.groupBoxInsideSeb.PerformLayout();
            this.tabPageSecurity.ResumeLayout(false);
            this.tabPageSecurity.PerformLayout();
            this.tabPageNetwork.ResumeLayout(false);
            this.tabControlNetwork.ResumeLayout(false);
            this.tabPageApplications.ResumeLayout(false);
            this.tabPageApplications.PerformLayout();
            this.tabControlApplications.ResumeLayout(false);
            this.tabPagePermittedProcesses.ResumeLayout(false);
            this.tabPagePermittedProcesses.PerformLayout();
            this.groupBoxSelectedProcess.ResumeLayout(false);
            this.groupBoxSelectedProcess.PerformLayout();
            this.tabPageExam.ResumeLayout(false);
            this.tabPageExam.PerformLayout();
            this.tabPageDownUploads.ResumeLayout(false);
            this.tabPageDownUploads.PerformLayout();
            this.tabPageBrowser.ResumeLayout(false);
            this.tabPageBrowser.PerformLayout();
            this.groupBoxNewBrowserWindow.ResumeLayout(false);
            this.groupBoxNewBrowserWindow.PerformLayout();
            this.tabPageAppearance.ResumeLayout(false);
            this.tabPageAppearance.PerformLayout();
            this.groupBoxMainBrowserWindow.ResumeLayout(false);
            this.groupBoxMainBrowserWindow.PerformLayout();
            this.tabPageConfigFile.ResumeLayout(false);
            this.tabPageConfigFile.PerformLayout();
            this.tabPageGeneral.ResumeLayout(false);
            this.tabPageGeneral.PerformLayout();
            this.groupBoxExitSequence.ResumeLayout(false);
            this.tabControlSebWindowsConfig.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion


        private System.Windows.Forms.OpenFileDialog openFileDialogSebConfigFile;
        private System.Windows.Forms.SaveFileDialog saveFileDialogSebConfigFile;
        private System.Windows.Forms.ImageList imageListTabIcons;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialogDownloadDirectoryWin;
        private System.Windows.Forms.TabPage tabPageHookedKeys;
        private System.Windows.Forms.CheckBox checkBoxHookKeys;
        private System.Windows.Forms.GroupBox groupBoxFunctionKeys;
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
        private System.Windows.Forms.GroupBox groupBoxSpecialKeys;
        private System.Windows.Forms.CheckBox checkBoxEnableEsc;
        private System.Windows.Forms.CheckBox checkBoxEnableCtrlEsc;
        private System.Windows.Forms.CheckBox checkBoxEnableAltEsc;
        private System.Windows.Forms.CheckBox checkBoxEnableAltTab;
        private System.Windows.Forms.CheckBox checkBoxEnableAltF4;
        private System.Windows.Forms.CheckBox checkBoxEnableStartMenu;
        private System.Windows.Forms.CheckBox checkBoxEnableRightMouse;
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
        private System.Windows.Forms.TabPage tabPageSecurity;
        private System.Windows.Forms.CheckBox checkBoxCreateNewDesktop;
        private System.Windows.Forms.CheckBox checkBoxAllowUserSwitching;
        private System.Windows.Forms.Label labelSebServicePolicy;
        private System.Windows.Forms.ListBox listBoxSebServicePolicy;
        private System.Windows.Forms.CheckBox checkBoxEnableLogging;
        private System.Windows.Forms.CheckBox checkBoxAllowVirtualMachine;
        private System.Windows.Forms.TabPage tabPageNetwork;
        private System.Windows.Forms.TabPage tabPageApplications;
        private System.Windows.Forms.CheckBox checkBoxAllowFlashFullscreen;
        private System.Windows.Forms.CheckBox checkBoxAllowSwitchToApplications;
        private System.Windows.Forms.CheckBox checkBoxMonitorProcesses;
        private System.Windows.Forms.TabPage tabPageExam;
        private System.Windows.Forms.Label labelPlaceThisQuitLink;
        private System.Windows.Forms.Label labelCopyBrowserExamKey;
        private System.Windows.Forms.Button buttonGenerateBrowserExamKey;
        private System.Windows.Forms.Label labelBrowserExamKey;
        private System.Windows.Forms.TextBox textBoxBrowserExamKey;
        private System.Windows.Forms.TextBox textBoxQuitURL;
        private System.Windows.Forms.Label labelQuitURL;
        private System.Windows.Forms.CheckBox checkBoxSendBrowserExamKey;
        private System.Windows.Forms.CheckBox checkBoxCopyBrowserExamKey;
        private System.Windows.Forms.TabPage tabPageDownUploads;
        private System.Windows.Forms.Label labelDownloadDirectoryWin;
        private System.Windows.Forms.Button buttonDownloadDirectoryWin;
        private System.Windows.Forms.ListBox listBoxChooseFileToUploadPolicy;
        private System.Windows.Forms.Label labelChooseFileToUploadPolicy;
        private System.Windows.Forms.CheckBox checkBoxDownloadPDFFiles;
        private System.Windows.Forms.CheckBox checkBoxOpenDownloads;
        private System.Windows.Forms.CheckBox checkBoxAllowDownUploads;
        private System.Windows.Forms.TabPage tabPageBrowser;
        private System.Windows.Forms.ListBox listBoxOpenLinksJava;
        private System.Windows.Forms.ListBox listBoxOpenLinksHTML;
        private System.Windows.Forms.Label labelUseSEBWithoutBrowser;
        private System.Windows.Forms.CheckBox checkBoxBlockPopUpWindows;
        private System.Windows.Forms.CheckBox checkBoxAllowBrowsingBackForward;
        private System.Windows.Forms.CheckBox checkBoxEnableJavaScript;
        private System.Windows.Forms.CheckBox checkBoxEnableJava;
        private System.Windows.Forms.CheckBox checkBoxEnablePlugIns;
        private System.Windows.Forms.CheckBox checkBoxUseSebWithoutBrowser;
        private System.Windows.Forms.CheckBox checkBoxBlockLinksJava;
        private System.Windows.Forms.Label labelOpenLinksJava;
        private System.Windows.Forms.Label labelOpenLinksHTML;
        private System.Windows.Forms.CheckBox checkBoxBlockLinksHTML;
        private System.Windows.Forms.GroupBox groupBoxNewBrowserWindow;
        private System.Windows.Forms.ComboBox comboBoxNewBrowserWindowHeight;
        private System.Windows.Forms.ComboBox comboBoxNewBrowserWindowWidth;
        private System.Windows.Forms.Label labelNewWindowHeight;
        private System.Windows.Forms.Label labelNewWindowWidth;
        private System.Windows.Forms.Label labelNewWindowPosition;
        private System.Windows.Forms.ListBox listBoxNewBrowserWindowPositioning;
        private System.Windows.Forms.TabPage tabPageAppearance;
        private System.Windows.Forms.GroupBox groupBoxMainBrowserWindow;
        private System.Windows.Forms.ComboBox comboBoxMainBrowserWindowHeight;
        private System.Windows.Forms.ComboBox comboBoxMainBrowserWindowWidth;
        private System.Windows.Forms.Label labelMainWindowHeight;
        private System.Windows.Forms.Label labelMainWindowWidth;
        private System.Windows.Forms.Label labelMainWindowPosition;
        private System.Windows.Forms.ListBox listBoxMainBrowserWindowPositioning;
        private System.Windows.Forms.CheckBox checkBoxShowTaskBar;
        private System.Windows.Forms.CheckBox checkBoxShowMenuBar;
        private System.Windows.Forms.CheckBox checkBoxHideBrowserWindowToolbar;
        private System.Windows.Forms.CheckBox checkBoxEnableBrowserWindowToolbar;
        private System.Windows.Forms.RadioButton radioButtonUseFullScreenMode;
        private System.Windows.Forms.RadioButton radioButtonUseBrowserWindow;
        private System.Windows.Forms.TabPage tabPageConfigFile;
        private System.Windows.Forms.Label labelUseEither;
        private System.Windows.Forms.Label labelChooseIdentity;
        private System.Windows.Forms.ComboBox comboBoxCryptoIdentity;
        private System.Windows.Forms.Label labelConfirmSettingsPassword;
        private System.Windows.Forms.Label labelSettingsPassword;
        private System.Windows.Forms.TextBox textBoxConfirmSettingsPassword;
        private System.Windows.Forms.TextBox textBoxSettingsPassword;
        private System.Windows.Forms.Label labelUseSEBSettingsFileFor;
        private System.Windows.Forms.RadioButton radioButtonConfiguringAClient;
        private System.Windows.Forms.RadioButton radioButtonStartingAnExam;
        private System.Windows.Forms.CheckBox checkBoxAllowPreferencesWindow;
        private System.Windows.Forms.Button buttonDefaultSettings;
        private System.Windows.Forms.Button buttonRevertToLastOpened;
        private System.Windows.Forms.Label labelOpenSettings;
        private System.Windows.Forms.Label labelSaveSettingsAs;
        private System.Windows.Forms.TabPage tabPageGeneral;
        private System.Windows.Forms.GroupBox groupBoxExitSequence;
        private System.Windows.Forms.ListBox listBoxExitKey1;
        private System.Windows.Forms.ListBox listBoxExitKey2;
        private System.Windows.Forms.CheckBox checkBoxIgnoreQuitPassword;
        private System.Windows.Forms.Button buttonPasteFromSavedClipboard;
        private System.Windows.Forms.Label labelSEBServerURL;
        private System.Windows.Forms.TextBox textBoxSebServerURL;
        private System.Windows.Forms.TextBox textBoxConfirmAdminPassword;
        private System.Windows.Forms.TextBox textBoxAdminPassword;
        private System.Windows.Forms.TextBox textBoxConfirmQuitPassword;
        private System.Windows.Forms.TextBox textBoxHashedQuitPassword;
        private System.Windows.Forms.TextBox textBoxQuitPassword;
        private System.Windows.Forms.TextBox textBoxStartURL;
        private System.Windows.Forms.Button buttonHelp;
        private System.Windows.Forms.Button buttonRestartSEB;
        private System.Windows.Forms.Button buttonQuit;
        private System.Windows.Forms.Button buttonAbout;
        private System.Windows.Forms.Label labelConfirmAdminPassword;
        private System.Windows.Forms.Label labelAdminPassword;
        private System.Windows.Forms.Label labelConfirmQuitPassword;
        private System.Windows.Forms.CheckBox checkBoxAllowQuit;
        private System.Windows.Forms.Label labelHashedQuitPassword;
        private System.Windows.Forms.Label labelQuitPassword;
        private System.Windows.Forms.Label labelStartURL;
        private System.Windows.Forms.TabControl tabControlSebWindowsConfig;
        private System.Windows.Forms.Button buttonLogDirectoryWin;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialogLogDirectoryWin;
        private System.Windows.Forms.Label labelLogDirectoryWin;
        private System.Windows.Forms.Label labelHashedAdminPassword;
        private System.Windows.Forms.TextBox textBoxHashedAdminPassword;
        private System.Windows.Forms.Label labelHashedSettingsPassword;
        private System.Windows.Forms.TextBox textBoxHashedSettingsPassword;
        private System.Windows.Forms.ListBox listBoxExitKey3;
        private System.Windows.Forms.TabControl tabControlApplications;
        private System.Windows.Forms.TabPage tabPagePermittedProcesses;
        private System.Windows.Forms.TabPage tabPageProhibitedProcesses;
        private System.Windows.Forms.TabControl tabControlNetwork;
        private System.Windows.Forms.TabPage tabPageFilter;
        private System.Windows.Forms.TabPage tabPageCertificates;
        private System.Windows.Forms.TabPage tabPageProxies;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.CheckedListBox checkedListBoxPermitted;
        private System.Windows.Forms.ListView listViewPermittedProcesses;
        private System.Windows.Forms.Button buttonChoosePermittedProcess;
        private System.Windows.Forms.Button buttonChoosePermittedApplication;
        private System.Windows.Forms.Button buttonRemovePermittedProcess;
        private System.Windows.Forms.Button buttonAddPermittedProcess;
        private System.Windows.Forms.GroupBox groupBoxSelectedProcess;
        private System.Windows.Forms.TextBox textBoxPermittedProcessDescription;
        private System.Windows.Forms.Label labelPermittedProcessDescription;
        private System.Windows.Forms.Label labelPermittedProcessAppTitle;
        private System.Windows.Forms.TextBox textBoxPermittedProcessAppTitle;
        private System.Windows.Forms.CheckBox checkBoxPermittedProcessAllowUser;
        private System.Windows.Forms.CheckBox checkBoxPermittedProcessAutohide;
        private System.Windows.Forms.CheckBox checkBoxPermittedProcessAutostart;
        private System.Windows.Forms.CheckBox checkBoxPermittedProcessActive;
        private System.Windows.Forms.Label labelPermittedProcessExecutable;
        private System.Windows.Forms.Label labelPermittedProcessPath;
        private System.Windows.Forms.TextBox textBoxPermittedProcessPath;
        private System.Windows.Forms.TextBox textBoxPermittedProcessExecutable;
        private System.Windows.Forms.Label labelPermittedProcessOS;
        private System.Windows.Forms.ListBox listBoxPermittedProcessOS;
        private System.Windows.Forms.Label labelPermittedProcessArguments;
        private System.Windows.Forms.Button buttonPermittedProcessRemoveArgument;
        private System.Windows.Forms.Button buttonPermittedProcessAddArgument;
        private System.Windows.Forms.CheckedListBox checkedListBoxPermittedProcessArguments;
        private System.Windows.Forms.TextBox textBoxPermittedProcessIdentifier;
        private System.Windows.Forms.Label labelPermittedProcessIdentifier;

    }
}

