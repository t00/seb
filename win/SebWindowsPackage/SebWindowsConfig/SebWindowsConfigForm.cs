using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Forms;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using SebShared.CryptographyUtils;
using SebShared.DiagnosticUtils;
using SebShared.Properties;
using SebShared.Utils;
using SebShared;
using SebWindowsClient.ConfigurationUtils;
using Application = System.Windows.Forms.Application;
using ListObj = System.Collections.Generic.List<object>;
using DictObj = System.Collections.Generic.Dictionary<string, object>;
using KeyValue = System.Collections.Generic.KeyValuePair<string, object>;
using MessageBox = System.Windows.Forms.MessageBox;


namespace SebWindowsConfig
{
	public partial class SebWindowsConfigForm: Form
	{
		public bool adminPasswordFieldsContainHash = false;
		public bool quitPasswordFieldsContainHash = false;
		public bool settingsPasswordFieldsContainHash = false;

		public bool quittingMyself = false;

		string settingsPassword = "";

		private string lastBrowserExamKey = "";
		private string lastSettingsPassword = "";

		private const string SEB_CONFIG_LOG = "SebConfig.log";

		//X509Certificate2 fileCertificateRef = null;


		/// ----------------------------------------------------------------------------------------
		/// <summary>
		/// OnLoad: Get the file name from command line arguments and load it.
		/// </summary>
		/// ----------------------------------------------------------------------------------------
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			string[] args = Environment.GetCommandLineArgs();

			string es = string.Join(", ", args);
			Logger.AddInformation("OnLoad EventArgs: " + es, null, null);

			if(args.Length > 1)
			{
				LoadConfigurationFileIntoEditor(args[1]);
				// Update Browser Exam Key
				lastBrowserExamKey = SebProtectionController.ComputeBrowserExamKey();
				lastSettingsPassword = textBoxSettingsPassword.Text;
				// Display the new Browser Exam Key in Exam pane
				textBoxBrowserExamKey.Text = lastBrowserExamKey;
			}
		}


		// ***********
		//
		// Constructor
		//
		// ***********

		public SebWindowsConfigForm()
		{
			InitializeComponent();

			// This is necessary to instanciate the password dialog
			//SEBConfigFileManager.InitSEBConfigFileManager();

			/// Initialize the Logger

			//Sets paths to files SEB has to save or read from the file system
			SEBClientInfo.SetSebPaths();

			// Set the path of the SebConfig.log file
			StringBuilder sebConfigLogFileBuilder = new StringBuilder(SEBClientInfo.SebClientLogFileDirectory).Append(SEB_CONFIG_LOG);
			string SebConfigLogFile = sebConfigLogFileBuilder.ToString();

			Logger.InitLogger(SEBClientInfo.SebClientLogFileDirectory, SebConfigLogFile ?? SEBClientInfo.SebClientLogFile);

			// Set all the default values for the Plist structure "SebSettings.settingsCurrent"
			SebInstance.Settings.RestoreDefaultAndCurrentSettings();
			SebInstance.Settings.PermitXulRunnerProcess();

			// Initialise the global variables for the GUI widgets
			InitialiseGlobalVariablesForGUIWidgets();

			// Initialise the GUI widgets themselves
			InitialiseGUIWidgets();

			// When starting up, load the default local client settings
			Logger.AddInformation("Loading the default local client settings", null, null);
			SEBClientInfo.LoadingSettingsFileName = "Local Client Settings";
			if(!LoadConfigurationFileIntoEditor(currentPathSebConfigFile))
			{
				// If this didn't work, then there are no local client settings and we set the current settings title to "Default Settings"
				currentPathSebConfigFile = SEBUIStrings.settingsTitleDefaultSettings;
				UpdateAllWidgetsOfProgram();
			}

			// Update Browser Exam Key
			lastBrowserExamKey = SebProtectionController.ComputeBrowserExamKey();
			lastSettingsPassword = textBoxSettingsPassword.Text;
			// Display the new Browser Exam Key in Exam pane
			textBoxBrowserExamKey.Text = lastBrowserExamKey;

		} // end of contructor   SebWindowsConfigForm()




		// *************************************************
		// Open the configuration file and read the settings
		// *************************************************
		private Boolean LoadConfigurationFileIntoEditor(String fileName)
		{
			Cursor.Current = Cursors.WaitCursor;
			// Read the file into "new" settings
			Logger.AddInformation("Loading settings from file " + fileName, null, null);

			// Set the filename into the global variable so it gets displayed in the password dialogs
			if(String.IsNullOrEmpty(SEBClientInfo.LoadingSettingsFileName))
			{
				SEBClientInfo.LoadingSettingsFileName = Path.GetFileName(fileName);
			}

			// In these variables we get back the configuration file password the user entered for decrypting and/or 
			// the certificate reference found in the config file:
			string filePassword = null;
			X509Certificate2 fileCertificateRef = null;
			bool passwordIsHash = false;

			if(!SebInstance.Settings.ReadSebConfigurationFile(fileName, SebPasswordInput.ClientGetPassword, true, ref filePassword, ref passwordIsHash, ref fileCertificateRef))
			{
				SEBClientInfo.LoadingSettingsFileName = "";
				return false;
			}
			SEBClientInfo.LoadingSettingsFileName = "";

			if(!String.IsNullOrEmpty(filePassword))
			{
				// If we got the settings password because the user entered it when opening the .seb file, 
				// we store it in a local variable
				settingsPassword = filePassword;
				settingsPasswordFieldsContainHash = passwordIsHash;
			}
			else
			{
				// We didn't get back any settings password, we clear the local variable
				settingsPassword = "";
				settingsPasswordFieldsContainHash = false;
			}

			// Check if we got a certificate reference used to encrypt the openend settings back
			if(fileCertificateRef != null)
			{
				comboBoxCryptoIdentity.SelectedIndex = 0;
				int indexOfCertificateRef = certificateReferences.IndexOf(fileCertificateRef);
				// Find this certificate reference in the list of all certificates from the certificate store
				// if found (this should always be the case), select that certificate in the comboBox list
				if(indexOfCertificateRef != -1) comboBoxCryptoIdentity.SelectedIndex = indexOfCertificateRef + 1;
			}

			// GUI-related part: Update the widgets
			currentDireSebConfigFile = Path.GetDirectoryName(fileName);
			currentFileSebConfigFile = Path.GetFileName(fileName);
			currentPathSebConfigFile = Path.GetFullPath(fileName);

			// After loading a new config file, reset the URL Filter Table indices
			// to avoid errors, in case there was a non-empty URL Filter Table displayed
			// in the DataGridViewURLFilterRules prior to loading the new config file.
			SebInstance.Settings.urlFilterRuleIndex = -1;
			SebInstance.Settings.urlFilterActionIndex = -1;

			// Get the URL Filter Rules
			SebInstance.Settings.urlFilterRuleList = (ListObj)SebInstance.Settings.settingsCurrent[SebSettings.KeyURLFilterRules];

			// If there are any filter rules, select first filter rule.
			// If there are no  filter rules, select no    filter rule.
			if(SebInstance.Settings.urlFilterRuleList.Count > 0)
				SebInstance.Settings.urlFilterRuleIndex = 0;
			else SebInstance.Settings.urlFilterRuleIndex = -1;

			// Initially show all filter rules with their actions (expanded view)
			urlFilterTableShowRule.Clear();
			for(int ruleIndex = 0; ruleIndex < SebInstance.Settings.urlFilterRuleList.Count; ruleIndex++)
			{
				urlFilterTableShowRule.Add(true);
			}

			UpdateAllWidgetsOfProgram();
			buttonRevertToLastOpened.Enabled = true;
			Cursor.Current = Cursors.Default;
			return true;
		}

		// ********************************************************
		// Write the settings to the configuration file and save it
		// ********************************************************
		private Boolean SaveConfigurationFileFromEditor(String fileName)
		{
			Cursor.Current = Cursors.WaitCursor;
			// Get settings password
			string filePassword = settingsPassword;

			// Get selected certificate
			X509Certificate2 fileCertificateRef = null;
			int selectedCertificate = (int)SebInstance.Settings.intArrayCurrent[SebSettings.ValCryptoIdentity];
			if(selectedCertificate > 0)
			{
				fileCertificateRef = (X509Certificate2)certificateReferences[selectedCertificate - 1];
			} //comboBoxCryptoIdentity.SelectedIndex;

			// Get selected config purpose
			int currentConfigPurpose = (int)SebInstance.Settings.valueForDictionaryKey(SebInstance.Settings.settingsCurrent, SebSettings.KeySebConfigPurpose);
			SebSettings.sebConfigPurposes configPurpose = (SebSettings.sebConfigPurposes)currentConfigPurpose;

			// Write the "new" settings to file
			if(!SebInstance.Settings.WriteSebConfigurationFile(fileName, filePassword, settingsPasswordFieldsContainHash, fileCertificateRef, configPurpose, forEditing : true)) return false;

			// If the settings could be written to file, update the widgets
			currentDireSebConfigFile = Path.GetDirectoryName(fileName);
			currentFileSebConfigFile = Path.GetFileName(fileName);
			currentPathSebConfigFile = Path.GetFullPath(fileName);

			UpdateAllWidgetsOfProgram();
			Cursor.Current = Cursors.Default;
			return true;
		}





		// *****************************************************
		// Set the widgets to the new settings of SebStarter.ini
		// *****************************************************
		private void UpdateAllWidgetsOfProgram()
		{
			// Update the filename in the title bar
			this.Text = this.ProductName;
			this.Text += " - ";
			this.Text += currentPathSebConfigFile;

			// Group "General"
			textBoxStartURL.Text = (String)SebInstance.Settings.settingsCurrent[SebSettings.KeyStartURL];
			textBoxSebServerURL.Text = (String)SebInstance.Settings.settingsCurrent[SebSettings.KeySebServerURL];

			// If an admin password is saved in the settings (as a hash), 
			// then we fill a placeholder string into the admin password text fields
			if(!String.IsNullOrEmpty((String)SebInstance.Settings.settingsCurrent[SebSettings.KeyHashedAdminPassword]))
			{
				// CAUTION: Do not change the order of setting the placeholders and the flag,
				// since the fired textBox..._TextChanged() events use these data!
				textBoxAdminPassword.Text = "0000000000000000";
				adminPasswordFieldsContainHash = true;
				textBoxConfirmAdminPassword.Text = "0000000000000000";
			}
			else
			{
				// CAUTION: Do not change the order of setting the placeholders and the flag,
				// since the fired textBox..._TextChanged() events use these data!
				adminPasswordFieldsContainHash = false;
				textBoxAdminPassword.Text = "";
				textBoxConfirmAdminPassword.Text = "";
			}

			checkBoxAllowQuit.Checked = (Boolean)SebInstance.Settings.settingsCurrent[SebSettings.KeyAllowQuit];
			checkBoxIgnoreExitKeys.Checked = (Boolean)SebInstance.Settings.settingsCurrent[SebSettings.KeyIgnoreExitKeys];

			// If a quit password is saved in the settings (as a hash), 
			// then we fill a placeholder string into the quit password text fields
			if(!String.IsNullOrEmpty((String)SebInstance.Settings.settingsCurrent[SebSettings.KeyHashedQuitPassword]))
			{
				// CAUTION: Do not change the order of setting the placeholders and the flag,
				// since the fired textBox..._TextChanged() events use these data!
				textBoxQuitPassword.Text = "0000000000000000";
				quitPasswordFieldsContainHash = true;
				textBoxConfirmQuitPassword.Text = "0000000000000000";
			}
			else
			{
				// CAUTION: Do not change the order of setting the placeholders and the flag,
				// since the fired textBox..._TextChanged() events use these data!
				quitPasswordFieldsContainHash = false;
				textBoxQuitPassword.Text = "";
				textBoxConfirmQuitPassword.Text = "";
			}

			listBoxExitKey1.SelectedIndex = (int)SebInstance.Settings.settingsCurrent[SebSettings.KeyExitKey1];
			listBoxExitKey2.SelectedIndex = (int)SebInstance.Settings.settingsCurrent[SebSettings.KeyExitKey2];
			listBoxExitKey3.SelectedIndex = (int)SebInstance.Settings.settingsCurrent[SebSettings.KeyExitKey3];

			// Group "Config File"
			radioButtonStartingAnExam.Checked = ((int)SebInstance.Settings.settingsCurrent[SebSettings.KeySebConfigPurpose] == 0);
			radioButtonConfiguringAClient.Checked = ((int)SebInstance.Settings.settingsCurrent[SebSettings.KeySebConfigPurpose] == 1);
			checkBoxAllowPreferencesWindow.Checked = (Boolean)SebInstance.Settings.settingsCurrent[SebSettings.KeyAllowPreferencesWindow];
			comboBoxCryptoIdentity.SelectedIndex = SebInstance.Settings.intArrayCurrent[SebSettings.ValCryptoIdentity];

			// If the settings password local variable contains a hash (and it isn't empty)
			if(settingsPasswordFieldsContainHash && !String.IsNullOrEmpty(settingsPassword))
			{
				// CAUTION: We need to reset this flag BEFORE changing the textBox text value,
				// because otherwise the compare passwords method will delete the first textBox again.
				settingsPasswordFieldsContainHash = false;
				textBoxSettingsPassword.Text = "0000000000000000";
				settingsPasswordFieldsContainHash = true;
				textBoxConfirmSettingsPassword.Text = "0000000000000000";
			}
			else
			{
				textBoxSettingsPassword.Text = settingsPassword;
				textBoxConfirmSettingsPassword.Text = settingsPassword;
			}

			// Group "Appearance"
			if((Boolean)SebInstance.Settings.settingsCurrent[SebSettings.KeyTouchOptimized] == true)
			{
				radioButtonTouchOptimized.Checked = true;
			}
			else
			{
				radioButtonUseBrowserWindow.Checked = ((int)SebInstance.Settings.settingsCurrent[SebSettings.KeyBrowserViewMode] == 0);
				radioButtonUseFullScreenMode.Checked = ((int)SebInstance.Settings.settingsCurrent[SebSettings.KeyBrowserViewMode] == 1);
			}
			comboBoxMainBrowserWindowWidth.Text = (String)SebInstance.Settings.settingsCurrent[SebSettings.KeyMainBrowserWindowWidth];
			comboBoxMainBrowserWindowHeight.Text = (String)SebInstance.Settings.settingsCurrent[SebSettings.KeyMainBrowserWindowHeight];
			listBoxMainBrowserWindowPositioning.SelectedIndex = (int)SebInstance.Settings.settingsCurrent[SebSettings.KeyMainBrowserWindowPositioning];
			checkBoxEnableBrowserWindowToolbar.Checked = (Boolean)SebInstance.Settings.settingsCurrent[SebSettings.KeyEnableBrowserWindowToolbar];
			checkBoxHideBrowserWindowToolbar.Checked = (Boolean)SebInstance.Settings.settingsCurrent[SebSettings.KeyHideBrowserWindowToolbar];
			checkBoxShowMenuBar.Checked = (Boolean)SebInstance.Settings.settingsCurrent[SebSettings.KeyShowMenuBar];
			checkBoxShowTaskBar.Checked = (Boolean)SebInstance.Settings.settingsCurrent[SebSettings.KeyShowTaskBar];
			comboBoxTaskBarHeight.Text = (String)SebInstance.Settings.settingsCurrent[SebSettings.KeyTaskBarHeight].ToString();
			radioButtonUseZoomPage.Checked = ((int)SebInstance.Settings.settingsCurrent[SebSettings.KeyZoomMode] == 0);
			radioButtonUseZoomText.Checked = ((int)SebInstance.Settings.settingsCurrent[SebSettings.KeyZoomMode] == 1);

			// Group "Browser"
			listBoxOpenLinksHTML.SelectedIndex = (int)SebInstance.Settings.settingsCurrent[SebSettings.KeyNewBrowserWindowByLinkPolicy];
			listBoxOpenLinksJava.SelectedIndex = (int)SebInstance.Settings.settingsCurrent[SebSettings.KeyNewBrowserWindowByScriptPolicy];
			checkBoxBlockLinksHTML.Checked = (Boolean)SebInstance.Settings.settingsCurrent[SebSettings.KeyNewBrowserWindowByLinkBlockForeign];
			checkBoxBlockLinksJava.Checked = (Boolean)SebInstance.Settings.settingsCurrent[SebSettings.KeyNewBrowserWindowByScriptBlockForeign];

			comboBoxNewBrowserWindowWidth.Text = (String)SebInstance.Settings.settingsCurrent[SebSettings.KeyNewBrowserWindowByLinkWidth];
			comboBoxNewBrowserWindowHeight.Text = (String)SebInstance.Settings.settingsCurrent[SebSettings.KeyNewBrowserWindowByLinkHeight];
			listBoxNewBrowserWindowPositioning.SelectedIndex = (int)SebInstance.Settings.settingsCurrent[SebSettings.KeyNewBrowserWindowByLinkPositioning];

			checkBoxEnablePlugIns.Checked = (Boolean)SebInstance.Settings.settingsCurrent[SebSettings.KeyEnablePlugIns];
			checkBoxEnableJava.Checked = (Boolean)SebInstance.Settings.settingsCurrent[SebSettings.KeyEnableJava];
			checkBoxEnableJavaScript.Checked = (Boolean)SebInstance.Settings.settingsCurrent[SebSettings.KeyEnableJavaScript];
			checkBoxBlockPopUpWindows.Checked = (Boolean)SebInstance.Settings.settingsCurrent[SebSettings.KeyBlockPopUpWindows];
			checkBoxAllowBrowsingBackForward.Checked = (Boolean)SebInstance.Settings.settingsCurrent[SebSettings.KeyAllowBrowsingBackForward];
			checkBoxRemoveProfile.Checked = (Boolean)SebInstance.Settings.settingsCurrent[SebSettings.KeyRemoveBrowserProfile];
			checkBoxDisableLocalStorage.Checked = (Boolean)SebInstance.Settings.settingsCurrent[SebSettings.KeyDisableLocalStorage];
			checkBoxUseSebWithoutBrowser.Checked = !((Boolean)SebInstance.Settings.settingsCurrent[SebSettings.KeyEnableSebBrowser]);
			// BEWARE: you must invert this value since "Use Without" is "Not Enable"!

			radioButtonUserAgentMacDefault.Checked = ((int)SebInstance.Settings.settingsCurrent[SebSettings.KeyBrowserUserAgentMac] == 0);
			radioButtonUserAgentMacCustom.Checked = ((int)SebInstance.Settings.settingsCurrent[SebSettings.KeyBrowserUserAgentMac] == 1);
			textBoxUserAgentMacCustom.Text = (String)SebInstance.Settings.settingsCurrent[SebSettings.KeyBrowserUserAgentMacCustom];

			radioButtonUserAgentDesktopDefault.Checked = ((int)SebInstance.Settings.settingsCurrent[SebSettings.KeyBrowserUserAgentDesktopMode] == 0);
			radioButtonUserAgentDesktopCustom.Checked = ((int)SebInstance.Settings.settingsCurrent[SebSettings.KeyBrowserUserAgentDesktopMode] == 1);
			textBoxUserAgentDesktopModeCustom.Text = (String)SebInstance.Settings.settingsCurrent[SebSettings.KeyBrowserUserAgentDesktopModeCustom];
			textBoxUserAgentDesktopModeDefault.Text = SebConstants.BROWSER_USERAGENT_DESKTOP;

			radioButtonUserAgentTouchDefault.Checked = ((int)SebInstance.Settings.settingsCurrent[SebSettings.KeyBrowserUserAgentTouchMode] == 0);
			radioButtonUserAgentTouchIPad.Checked = ((int)SebInstance.Settings.settingsCurrent[SebSettings.KeyBrowserUserAgentTouchMode] == 1);
			radioButtonUserAgentTouchCustom.Checked = ((int)SebInstance.Settings.settingsCurrent[SebSettings.KeyBrowserUserAgentTouchMode] == 2);
			textBoxUserAgentTouchModeCustom.Text = (String)SebInstance.Settings.settingsCurrent[SebSettings.KeyBrowserUserAgentTouchModeCustom];
			textBoxUserAgentTouchModeDefault.Text = SebConstants.BROWSER_USERAGENT_TOUCH;
			textBoxUserAgentTouchModeIPad.Text = SebConstants.BROWSER_USERAGENT_TOUCH_IPAD;

			// Group "Down/Uploads"
			checkBoxAllowDownUploads.Checked = (Boolean)SebInstance.Settings.settingsCurrent[SebSettings.KeyAllowDownUploads];
			checkBoxOpenDownloads.Checked = (Boolean)SebInstance.Settings.settingsCurrent[SebSettings.KeyOpenDownloads];
			checkBoxDownloadPDFFiles.Checked = (Boolean)SebInstance.Settings.settingsCurrent[SebSettings.KeyDownloadPDFFiles];
			textBoxDownloadDirectoryWin.Text = (String)SebInstance.Settings.settingsCurrent[SebSettings.KeyDownloadDirectoryWin];
			textBoxDownloadDirectoryOSX.Text = (String)SebInstance.Settings.settingsCurrent[SebSettings.KeyDownloadDirectoryOSX];
			listBoxChooseFileToUploadPolicy.SelectedIndex = (int)SebInstance.Settings.settingsCurrent[SebSettings.KeyChooseFileToUploadPolicy];
			checkBoxDownloadOpenSEBFiles.Checked = (Boolean)SebInstance.Settings.settingsCurrent[SebSettings.KeyDownloadAndOpenSebConfig];

			// Group "Exam"
			//textBoxBrowserExamKey    .Text    =  (String)SebSettings.settingsCurrent[SebSettings.KeyBrowserExamKey];
			textBoxQuitURL.Text = (String)SebInstance.Settings.settingsCurrent[SebSettings.KeyQuitURL];
			checkBoxSendBrowserExamKey.Checked = (Boolean)SebInstance.Settings.settingsCurrent[SebSettings.KeySendBrowserExamKey];
			checkBoxUseStartURL.Checked = (Boolean)SebInstance.Settings.settingsCurrent[SebSettings.KeyRestartExamUseStartURL];
			textBoxRestartExamLink.Enabled = !(Boolean)SebInstance.Settings.settingsCurrent[SebSettings.KeyRestartExamUseStartURL];
			checkBoxRestartExamPasswordProtected.Checked = (Boolean)SebInstance.Settings.settingsCurrent[SebSettings.KeyRestartExamPasswordProtected];
			textBoxRestartExamLink.Text = (String)SebInstance.Settings.settingsCurrent[SebSettings.KeyRestartExamURL];
			textBoxRestartExamText.Text = (String)SebInstance.Settings.settingsCurrent[SebSettings.KeyRestartExamText];

			// Group "Applications"
			checkBoxMonitorProcesses.Checked = (Boolean)SebInstance.Settings.settingsCurrent[SebSettings.KeyMonitorProcesses];
			checkBoxAllowSwitchToApplications.Checked = (Boolean)SebInstance.Settings.settingsCurrent[SebSettings.KeyAllowSwitchToApplications];
			checkBoxAllowFlashFullscreen.Checked = (Boolean)SebInstance.Settings.settingsCurrent[SebSettings.KeyAllowFlashFullscreen];


			// Group "Applications - Permitted/Prohibited Processes"
			// Group "Network      -    Filter/Certificates/Proxies"

			// Update the lists for the DataGridViews
			SebInstance.Settings.permittedProcessList = (ListObj)SebInstance.Settings.settingsCurrent[SebSettings.KeyPermittedProcesses];
			SebInstance.Settings.prohibitedProcessList = (ListObj)SebInstance.Settings.settingsCurrent[SebSettings.KeyProhibitedProcesses];
			SebInstance.Settings.embeddedCertificateList = (ListObj)SebInstance.Settings.settingsCurrent[SebSettings.KeyEmbeddedCertificates];
			SebInstance.Settings.proxiesData = (DictObj)SebInstance.Settings.settingsCurrent[SebSettings.KeyProxies];

			SebInstance.Settings.bypassedProxyList = (ListObj)SebInstance.Settings.proxiesData[SebSettings.KeyExceptionsList];

			// Check if currently loaded lists have any entries
			if(SebInstance.Settings.permittedProcessList.Count > 0)
				SebInstance.Settings.permittedProcessIndex = 0;
			else SebInstance.Settings.permittedProcessIndex = -1;

			if(SebInstance.Settings.prohibitedProcessList.Count > 0)
				SebInstance.Settings.prohibitedProcessIndex = 0;
			else SebInstance.Settings.prohibitedProcessIndex = -1;

			if(SebInstance.Settings.embeddedCertificateList.Count > 0)
				SebInstance.Settings.embeddedCertificateIndex = 0;
			else SebInstance.Settings.embeddedCertificateIndex = -1;

			SebInstance.Settings.proxyProtocolIndex = 0;

			if(SebInstance.Settings.bypassedProxyList.Count > 0)
				SebInstance.Settings.bypassedProxyIndex = 0;
			else SebInstance.Settings.bypassedProxyIndex = -1;

			// Remove all previously displayed list entries from DataGridViews
			groupBoxPermittedProcess.Enabled = (SebInstance.Settings.permittedProcessList.Count > 0);
			dataGridViewPermittedProcesses.Enabled = (SebInstance.Settings.permittedProcessList.Count > 0);
			dataGridViewPermittedProcesses.Rows.Clear();

			groupBoxProhibitedProcess.Enabled = (SebInstance.Settings.prohibitedProcessList.Count > 0);
			dataGridViewProhibitedProcesses.Enabled = (SebInstance.Settings.prohibitedProcessList.Count > 0);
			dataGridViewProhibitedProcesses.Rows.Clear();

			dataGridViewEmbeddedCertificates.Enabled = (SebInstance.Settings.embeddedCertificateList.Count > 0);
			dataGridViewEmbeddedCertificates.Rows.Clear();

			dataGridViewProxyProtocols.Enabled = true;
			dataGridViewProxyProtocols.Rows.Clear();

			textBoxBypassedProxyHostList.Text = "";

			// Add Permitted Processes of currently opened file to DataGridView
			for(int index = 0; index < SebInstance.Settings.permittedProcessList.Count; index++)
			{
				SebInstance.Settings.permittedProcessData = (DictObj)SebInstance.Settings.permittedProcessList[index];
				Boolean active = (Boolean)SebInstance.Settings.permittedProcessData[SebSettings.KeyActive];
				Int32 os = (Int32)SebInstance.Settings.permittedProcessData[SebSettings.KeyOS];
				String executable = (String)SebInstance.Settings.permittedProcessData[SebSettings.KeyExecutable];
				String title = (String)SebInstance.Settings.permittedProcessData[SebSettings.KeyTitle];
				dataGridViewPermittedProcesses.Rows.Add(active, StringOS[os], executable, title);
			}

			// Add Prohibited Processes of currently opened file to DataGridView
			for(int index = 0; index < SebInstance.Settings.prohibitedProcessList.Count; index++)
			{
				SebInstance.Settings.prohibitedProcessData = (DictObj)SebInstance.Settings.prohibitedProcessList[index];
				Boolean active = (Boolean)SebInstance.Settings.prohibitedProcessData[SebSettings.KeyActive];
				Int32 os = (Int32)SebInstance.Settings.prohibitedProcessData[SebSettings.KeyOS];
				String executable = (String)SebInstance.Settings.prohibitedProcessData[SebSettings.KeyExecutable];
				String description = (String)SebInstance.Settings.prohibitedProcessData[SebSettings.KeyDescription];
				dataGridViewProhibitedProcesses.Rows.Add(active, StringOS[os], executable, description);
			}

			// Add Url Filters
			datagridWhitelist.Rows.Clear();
			foreach(var whiteListFilterItem in SebInstance.Settings.settingsCurrent[SebSettings.KeyUrlFilterWhitelist].ToString().Split(';'))
			{
				if(!String.IsNullOrWhiteSpace(whiteListFilterItem))
					datagridWhitelist.Rows.Add(whiteListFilterItem);
			}

			datagridBlackListFilter.Rows.Clear();
			foreach(var blackListFilterItem in SebInstance.Settings.settingsCurrent[SebSettings.KeyUrlFilterBlacklist].ToString().Split(';'))
			{
				if(!String.IsNullOrWhiteSpace(blackListFilterItem))
					datagridBlackListFilter.Rows.Add(blackListFilterItem);
			}

			// Add Embedded Certificates of Certificate Store to DataGridView
			for(int index = 0; index < SebInstance.Settings.embeddedCertificateList.Count; index++)
			{
				SebInstance.Settings.embeddedCertificateData = (DictObj)SebInstance.Settings.embeddedCertificateList[index];
				Int32 type = (Int32)SebInstance.Settings.embeddedCertificateData[SebSettings.KeyType];
				String name = (String)SebInstance.Settings.embeddedCertificateData[SebSettings.KeyName];
				dataGridViewEmbeddedCertificates.Rows.Add(StringCertificateType[type], name);
			}
			/*
						// Get the "Enabled" boolean values of current "proxies" dictionary
						BooleanProxyProtocolEnabled[IntProxyAutoDiscovery    ] = (Boolean)SebSettings.proxiesData[SebSettings.KeyAutoDiscoveryEnabled];
						BooleanProxyProtocolEnabled[IntProxyAutoConfiguration] = (Boolean)SebSettings.proxiesData[SebSettings.KeyAutoConfigurationEnabled];
						BooleanProxyProtocolEnabled[IntProxyHTTP             ] = (Boolean)SebSettings.proxiesData[SebSettings.KeyHTTPEnable];
						BooleanProxyProtocolEnabled[IntProxyHTTPS            ] = (Boolean)SebSettings.proxiesData[SebSettings.KeyHTTPSEnable];
						BooleanProxyProtocolEnabled[IntProxyFTP              ] = (Boolean)SebSettings.proxiesData[SebSettings.KeyFTPEnable];
						BooleanProxyProtocolEnabled[IntProxySOCKS            ] = (Boolean)SebSettings.proxiesData[SebSettings.KeySOCKSEnable];
						BooleanProxyProtocolEnabled[IntProxyRTSP             ] = (Boolean)SebSettings.proxiesData[SebSettings.KeyRTSPEnable];
			*/
			// Get the "Enabled" boolean values of current "proxies" dictionary.
			// Add Proxy Protocols of currently opened file to DataGridView.
			for(int index = 0; index < NumProxyProtocols; index++)
			{
				Boolean enable = (Boolean)SebInstance.Settings.proxiesData[KeyProxyProtocolEnable[index]];
				String type = (String)StringProxyProtocolTableCaption[index];
				dataGridViewProxyProtocols.Rows.Add(enable, type);
				BooleanProxyProtocolEnabled[index] = enable;
			}

			// Add Bypassed Proxies of currently opened file to the comma separated list
			StringBuilder bypassedProxiesStringBuilder = new StringBuilder();
			for(int index = 0; index < SebInstance.Settings.bypassedProxyList.Count; index++)
			{
				SebInstance.Settings.bypassedProxyData = (String)SebInstance.Settings.bypassedProxyList[index];
				if(bypassedProxiesStringBuilder.Length > 0)
				{
					bypassedProxiesStringBuilder.Append(", ");
				}
				bypassedProxiesStringBuilder.Append(SebInstance.Settings.bypassedProxyData);
			}
			textBoxBypassedProxyHostList.Text = bypassedProxiesStringBuilder.ToString();

			// Load the currently selected process data
			if(SebInstance.Settings.permittedProcessList.Count > 0)
				LoadAndUpdatePermittedSelectedProcessGroup(SebInstance.Settings.permittedProcessIndex);
			else ClearPermittedSelectedProcessGroup();

			if(SebInstance.Settings.prohibitedProcessList.Count > 0)
				LoadAndUpdateProhibitedSelectedProcessGroup(SebInstance.Settings.prohibitedProcessIndex);
			else ClearProhibitedSelectedProcessGroup();

			// Auto-resize the columns and cells

			//dataGridViewPermittedProcesses  .AutoResizeColumns();
			//dataGridViewProhibitedProcesses .AutoResizeColumns();
			//dataGridViewURLFilterRules      .AutoResizeColumns();
			//dataGridViewEmbeddedCertificates.AutoResizeColumns();
			//dataGridViewProxyProtocols      .AutoResizeColumns();
			//dataGridViewBypassedProxies     .AutoResizeColumns();

			//dataGridViewPermittedProcesses  .AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
			//dataGridViewProhibitedProcesses .AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
			//dataGridViewURLFilterRules      .AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
			//dataGridViewEmbeddedCertificates.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
			//dataGridViewProxyProtocols      .AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
			//dataGridViewBypassedProxies     .AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);

			//Group "Network - URL Filter"
			checkBoxEnableURLFilter.Checked = (Boolean)SebInstance.Settings.settingsCurrent[SebSettings.KeyURLFilterEnable];
			checkBoxUrlFilterRulesRegex.Checked = (Boolean)SebInstance.Settings.settingsCurrent[SebSettings.KeyUrlFilterRulesAsRegex];
			checkBoxEnableURLContentFilter.Checked = (Boolean)SebInstance.Settings.settingsCurrent[SebSettings.KeyURLFilterEnableContentFilter];
			checkBoxEnableURLContentFilter.Enabled = checkBoxEnableURLFilter.Checked;
			checkBoxUrlFilterRulesRegex.Enabled = checkBoxEnableURLFilter.Checked;


			// Group "Network - Certificates"

			// Group "Network - Proxies"
			radioButtonUseSystemProxySettings.Checked = ((int)SebInstance.Settings.settingsCurrent[SebSettings.KeyProxySettingsPolicy] == 0);
			radioButtonUseSebProxySettings.Checked = ((int)SebInstance.Settings.settingsCurrent[SebSettings.KeyProxySettingsPolicy] == 1);

			textBoxAutoProxyConfigurationURL.Text = (String)SebInstance.Settings.proxiesData[SebSettings.KeyAutoConfigurationURL];
			checkBoxExcludeSimpleHostnames.Checked = (Boolean)SebInstance.Settings.proxiesData[SebSettings.KeyExcludeSimpleHostnames];
			checkBoxUsePassiveFTPMode.Checked = (Boolean)SebInstance.Settings.proxiesData[SebSettings.KeyFTPPassive];

			// Group "Security"
			listBoxSebServicePolicy.SelectedIndex = (int)SebInstance.Settings.settingsCurrent[SebSettings.KeySebServicePolicy];
			checkBoxAllowVirtualMachine.Checked = (Boolean)SebInstance.Settings.settingsCurrent[SebSettings.KeyAllowVirtualMachine];
			radioCreateNewDesktop.Checked = (Boolean)SebInstance.Settings.settingsCurrent[SebSettings.KeyCreateNewDesktop];
			radioKillExplorerShell.Checked = (Boolean)SebInstance.Settings.settingsCurrent[SebSettings.KeyKillExplorerShell];
			radioNoKiosMode.Checked = !(Boolean)SebInstance.Settings.settingsCurrent[SebSettings.KeyKillExplorerShell] && !(Boolean)SebInstance.Settings.settingsCurrent[SebSettings.KeyCreateNewDesktop];
			checkBoxAllowUserSwitching.Checked = (Boolean)SebInstance.Settings.settingsCurrent[SebSettings.KeyAllowUserSwitching];
			checkBoxEnableAppSwitcherCheck.Checked = (Boolean)SebInstance.Settings.settingsCurrent[SebSettings.KeyEnableAppSwitcherCheck];
			checkBoxForceAppFolderInstall.Checked = (Boolean)SebInstance.Settings.settingsCurrent[SebSettings.KeyForceAppFolderInstall];
			checkBoxEnableLogging.Checked = (Boolean)SebInstance.Settings.settingsCurrent[SebSettings.KeyEnableLogging];
			textBoxLogDirectoryWin.Text = (String)SebInstance.Settings.settingsCurrent[SebSettings.KeyLogDirectoryWin];
			if(String.IsNullOrEmpty(textBoxLogDirectoryWin.Text))
			{
				checkBoxUseStandardDirectory.Checked = true;
			}
			else
			{
				checkBoxUseStandardDirectory.Checked = false;
			}
			textBoxLogDirectoryOSX.Text = (String)SebInstance.Settings.settingsCurrent[SebSettings.KeyLogDirectoryOSX];
			checkboxAllowWlan.Checked = (Boolean)SebInstance.Settings.settingsCurrent[SebSettings.KeyAllowWLAN];
			checkBoxEnableScreenCapture.Checked = (Boolean)SebInstance.Settings.settingsCurrent[SebSettings.KeyEnablePrintScreen];

			// Group "Registry"
			checkBoxInsideSebEnableSwitchUser.Checked = (Boolean)SebInstance.Settings.settingsCurrent[SebSettings.KeyInsideSebEnableSwitchUser];
			checkBoxInsideSebEnableLockThisComputer.Checked = (Boolean)SebInstance.Settings.settingsCurrent[SebSettings.KeyInsideSebEnableLockThisComputer];
			checkBoxInsideSebEnableChangeAPassword.Checked = (Boolean)SebInstance.Settings.settingsCurrent[SebSettings.KeyInsideSebEnableChangeAPassword];
			checkBoxInsideSebEnableStartTaskManager.Checked = (Boolean)SebInstance.Settings.settingsCurrent[SebSettings.KeyInsideSebEnableStartTaskManager];
			checkBoxInsideSebEnableLogOff.Checked = (Boolean)SebInstance.Settings.settingsCurrent[SebSettings.KeyInsideSebEnableLogOff];
			checkBoxInsideSebEnableShutDown.Checked = (Boolean)SebInstance.Settings.settingsCurrent[SebSettings.KeyInsideSebEnableShutDown];
			checkBoxInsideSebEnableEaseOfAccess.Checked = (Boolean)SebInstance.Settings.settingsCurrent[SebSettings.KeyInsideSebEnableEaseOfAccess];
			checkBoxInsideSebEnableVmWareClientShade.Checked = (Boolean)SebInstance.Settings.settingsCurrent[SebSettings.KeyInsideSebEnableVmWareClientShade];

			// Group "Hooked Keys"
			checkBoxHookKeys.Checked = (Boolean)SebInstance.Settings.settingsCurrent[SebSettings.KeyHookKeys];

			checkBoxEnableEsc.Checked = (Boolean)SebInstance.Settings.settingsCurrent[SebSettings.KeyEnableEsc];
			checkBoxEnableCtrlEsc.Checked = (Boolean)SebInstance.Settings.settingsCurrent[SebSettings.KeyEnableCtrlEsc];
			checkBoxEnableAltEsc.Checked = (Boolean)SebInstance.Settings.settingsCurrent[SebSettings.KeyEnableAltEsc];
			checkBoxEnableAltTab.Checked = (Boolean)SebInstance.Settings.settingsCurrent[SebSettings.KeyEnableAltTab];
			checkBoxEnableAltF4.Checked = (Boolean)SebInstance.Settings.settingsCurrent[SebSettings.KeyEnableAltF4];
			checkBoxEnableRightMouse.Checked = (Boolean)SebInstance.Settings.settingsCurrent[SebSettings.KeyEnableRightMouse];
			checkBoxEnablePrintScreen.Checked = (Boolean)SebInstance.Settings.settingsCurrent[SebSettings.KeyEnablePrintScreen];
			checkBoxEnableAltMouseWheel.Checked = (Boolean)SebInstance.Settings.settingsCurrent[SebSettings.KeyEnableAltMouseWheel];

			checkBoxEnableF1.Checked = (Boolean)SebInstance.Settings.settingsCurrent[SebSettings.KeyEnableF1];
			checkBoxEnableF2.Checked = (Boolean)SebInstance.Settings.settingsCurrent[SebSettings.KeyEnableF2];
			checkBoxEnableF3.Checked = (Boolean)SebInstance.Settings.settingsCurrent[SebSettings.KeyEnableF3];
			checkBoxEnableF4.Checked = (Boolean)SebInstance.Settings.settingsCurrent[SebSettings.KeyEnableF4];
			checkBoxEnableF5.Checked = (Boolean)SebInstance.Settings.settingsCurrent[SebSettings.KeyEnableF5];
			checkBoxEnableF6.Checked = (Boolean)SebInstance.Settings.settingsCurrent[SebSettings.KeyEnableF6];
			checkBoxEnableF7.Checked = (Boolean)SebInstance.Settings.settingsCurrent[SebSettings.KeyEnableF7];
			checkBoxEnableF8.Checked = (Boolean)SebInstance.Settings.settingsCurrent[SebSettings.KeyEnableF8];
			checkBoxEnableF9.Checked = (Boolean)SebInstance.Settings.settingsCurrent[SebSettings.KeyEnableF9];
			checkBoxEnableF10.Checked = (Boolean)SebInstance.Settings.settingsCurrent[SebSettings.KeyEnableF10];
			checkBoxEnableF11.Checked = (Boolean)SebInstance.Settings.settingsCurrent[SebSettings.KeyEnableF11];
			checkBoxEnableF12.Checked = (Boolean)SebInstance.Settings.settingsCurrent[SebSettings.KeyEnableF12];

			checkBoxShowReloadButton.Checked = (Boolean)SebInstance.Settings.settingsCurrent[SebSettings.KeyShowReloadButton];
			checkBoxShowReloadWarning.Checked = (Boolean)SebInstance.Settings.settingsCurrent[SebSettings.KeyShowReloadWarning];
			checkBoxEnableZoomPage.Checked = (Boolean)SebInstance.Settings.settingsCurrent[SebSettings.KeyEnableZoomPage];
			checkBoxEnableZoomText.Checked = (Boolean)SebInstance.Settings.settingsCurrent[SebSettings.KeyEnableZoomText];
			radioButtonUseZoomPage.Checked = ((int)SebInstance.Settings.settingsCurrent[SebSettings.KeyZoomMode] == 0);
			radioButtonUseZoomText.Checked = ((int)SebInstance.Settings.settingsCurrent[SebSettings.KeyZoomMode] == 1);
			enableZoomAdjustZoomMode();

			checkBoxAllowSpellCheck.Checked = (Boolean)SebInstance.Settings.settingsCurrent[SebSettings.KeyAllowSpellCheck];
			checkBoxAllowDictionaryLookup.Checked = (Boolean)SebInstance.Settings.settingsCurrent[SebSettings.KeyAllowDictionaryLookup];
			checkBoxShowTime.Checked = (Boolean)SebInstance.Settings.settingsCurrent[SebSettings.KeyShowTime];
			checkBoxShowKeyboardLayout.Checked = (Boolean)SebInstance.Settings.settingsCurrent[SebSettings.KeyShowInputLanguage];

			return;
		}

		/// ----------------------------------------------------------------------------------------
		/// <summary>
		/// Compare password textfields and show or hide compare label accordingly
		/// if passwords are same, save the password hash
		/// </summary>
		/// ----------------------------------------------------------------------------------------
		public void ComparePasswords(TextBox passwordField, TextBox confirmPasswordField, ref bool passwordFieldsContainHash, Label label, string settingsKey)
		{
			// Get the password text from the text fields
			string password = passwordField.Text;
			string confirmPassword = confirmPasswordField.Text;

			if(passwordFieldsContainHash)
			{
				// If the flag is set for password fields contain a placeholder 
				// instead of the hash loaded from settings (no clear text password)
				if(password.CompareTo(confirmPassword) != 0)
				{
					// and when the password texts aren't the same anymore, this means the user tries to edit the password
					// (which is only the placeholder right now), we have to clear the placeholder from the textFields
					passwordField.Text = "";
					confirmPasswordField.Text = "";
					password = "";
					confirmPassword = "";
					passwordFieldsContainHash = false;
				}
			}

			// Compare text value of password fields, regardless if they contain actual passwords or a hash
			if(password.CompareTo(confirmPassword) == 0)
			{
				/// Passwords are same
				// Hide the "Please confirm password" label
				label.Visible = false;

				String newStringHashcode = "";
				if(!passwordFieldsContainHash && !String.IsNullOrEmpty(password) && settingsKey != null)
				{
					// If the password isn't the placeholder for the hash, isn't empty 
					// and we got the key to where to save the hash (for the settings pw we don't need a hash)
					// we hash the password, otherwise just save the empty string into settings
					// Password hashing using the SHA-256 hash algorithm
					SHA256 sha256Algorithm = new SHA256Managed();
					// Hash the new quit password
					byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
					byte[] hashcodeBytes = sha256Algorithm.ComputeHash(passwordBytes);
					// Generate a base16 hash string
					newStringHashcode = BitConverter.ToString(hashcodeBytes);
					newStringHashcode = newStringHashcode.Replace("-", "");
				}
				// Save the new password into settings password variable
				if(!passwordFieldsContainHash && settingsKey == null)
				{
					settingsPassword = password;
				}
				// Save the new hash string into settings 
				if(!passwordFieldsContainHash && settingsKey != null) SebInstance.Settings.settingsCurrent[settingsKey] = newStringHashcode;
				// Enable the save/use settings buttons
				//SetButtonsCommandsEnabled(true);
			}
			else
			{
				/// Passwords are not same

				// If this was a settings password hash and it got edited: Clear the settings password variable and the hash flag
				if(passwordFieldsContainHash && settingsKey == null)
				{
					settingsPassword = "";
					settingsPasswordFieldsContainHash = false;
				}

				//SetButtonsCommandsEnabled(false);
				label.Visible = true;
			}
		}

		/// ----------------------------------------------------------------------------------------
		/// <summary>
		/// Change the enabled status of buttons and menu commands 
		/// for saving and using current settings.
		/// </summary>
		/// ----------------------------------------------------------------------------------------
		private void SetButtonsCommandsEnabled(bool enabledStatus)
		{
			buttonSaveSettings.Enabled = enabledStatus;
			buttonSaveSettingsAs.Enabled = enabledStatus;
			buttonConfigureClient.Enabled = enabledStatus;
			buttonApplyAndStartSEB.Enabled = enabledStatus;

			saveSettingsToolStripMenuItem.Enabled = enabledStatus;
			saveSettingsAsToolStripMenuItem.Enabled = enabledStatus;
			configureClientToolStripMenuItem.Enabled = enabledStatus;
			applyAndStartSEBToolStripMenuItem.Enabled = enabledStatus;
		}


		/// ----------------------------------------------------------------------------------------
		/// <summary>
		/// Check if there are some unconfirmed passwords and show alert if so.
		/// </summary>
		/// ----------------------------------------------------------------------------------------
		private bool ArePasswordsUnconfirmed()
		{
			bool passwordIsUnconfirmed = false;
			string unconfirmedPassword;

			if(textBoxAdminPassword.Text.CompareTo(textBoxConfirmAdminPassword.Text) != 0)
			{
				unconfirmedPassword = SEBUIStrings.passwordAdmin;
				PresentAlertForUnconfirmedPassword(unconfirmedPassword);
				passwordIsUnconfirmed = true;
			}

			if(textBoxQuitPassword.Text.CompareTo(textBoxConfirmQuitPassword.Text) != 0)
			{
				unconfirmedPassword = SEBUIStrings.passwordQuit;
				PresentAlertForUnconfirmedPassword(unconfirmedPassword);
				passwordIsUnconfirmed = true;
			}

			if(textBoxSettingsPassword.Text.CompareTo(textBoxConfirmSettingsPassword.Text) != 0)
			{
				unconfirmedPassword = SEBUIStrings.passwordSettings;
				PresentAlertForUnconfirmedPassword(unconfirmedPassword);
				passwordIsUnconfirmed = true;
			}

			return passwordIsUnconfirmed;
		}

		/// ----------------------------------------------------------------------------------------
		/// <summary>
		/// Check if there are some unconfirmed passwords and show alert if so.
		/// </summary>
		/// ----------------------------------------------------------------------------------------
		private void PresentAlertForUnconfirmedPassword(string unconfirmedPassword)
		{
			SebMessageBox.Show(SEBUIStrings.unconfirmedPasswordTitle, SEBUIStrings.unconfirmedPasswordMessage.Replace("%s", unconfirmedPassword), MessageBoxImage.Error, MessageBoxButton.OK);
		}

		// **************
		//
		// Event handlers
		//
		// **************



		// ***************
		// Group "General"
		// ***************
		private void textBoxStartURL_TextChanged(object sender, EventArgs e)
		{
			SebInstance.Settings.settingsCurrent[SebSettings.KeyStartURL] = textBoxStartURL.Text;
		}

		private void textBoxSebServerURL_TextChanged(object sender, EventArgs e)
		{
			SebInstance.Settings.settingsCurrent[SebSettings.KeySebServerURL] = textBoxSebServerURL.Text;
		}

		private void textBoxAdminPassword_TextChanged(object sender, EventArgs e)
		{
			ComparePasswords(textBoxAdminPassword, textBoxConfirmAdminPassword, ref adminPasswordFieldsContainHash, labelAdminPasswordCompare, SebSettings.KeyHashedAdminPassword);
		}

		private void textBoxConfirmAdminPassword_TextChanged(object sender, EventArgs e)
		{
			ComparePasswords(textBoxAdminPassword, textBoxConfirmAdminPassword, ref adminPasswordFieldsContainHash, labelAdminPasswordCompare, SebSettings.KeyHashedAdminPassword);
		}

		private void checkBoxAllowQuit_CheckedChanged(object sender, EventArgs e)
		{
			SebInstance.Settings.settingsCurrent[SebSettings.KeyAllowQuit] = checkBoxAllowQuit.Checked;
		}

		private void textBoxQuitPassword_TextChanged(object sender, EventArgs e)
		{
			ComparePasswords(textBoxQuitPassword, textBoxConfirmQuitPassword, ref quitPasswordFieldsContainHash, labelQuitPasswordCompare, SebSettings.KeyHashedQuitPassword);
		}


		private void textBoxConfirmQuitPassword_TextChanged(object sender, EventArgs e)
		{
			ComparePasswords(textBoxQuitPassword, textBoxConfirmQuitPassword, ref quitPasswordFieldsContainHash, labelQuitPasswordCompare, SebSettings.KeyHashedQuitPassword);
		}


		private void checkBoxIgnoreExitKeys_CheckedChanged(object sender, EventArgs e)
		{
			SebInstance.Settings.settingsCurrent[SebSettings.KeyIgnoreExitKeys] = checkBoxIgnoreExitKeys.Checked;
			groupBoxExitSequence.Enabled = !checkBoxIgnoreExitKeys.Checked;
		}

		private void listBoxExitKey1_SelectedIndexChanged(object sender, EventArgs e)
		{
			// Make sure that all three exit keys are different.
			// If selected key is already occupied, revert to previously selected key.
			if((listBoxExitKey1.SelectedIndex == listBoxExitKey2.SelectedIndex) ||
				(listBoxExitKey1.SelectedIndex == listBoxExitKey3.SelectedIndex))
				listBoxExitKey1.SelectedIndex = (int)SebInstance.Settings.settingsCurrent[SebSettings.KeyExitKey1];
			SebInstance.Settings.settingsCurrent[SebSettings.KeyExitKey1] = listBoxExitKey1.SelectedIndex;
		}

		private void listBoxExitKey2_SelectedIndexChanged(object sender, EventArgs e)
		{
			// Make sure that all three exit keys are different.
			// If selected key is already occupied, revert to previously selected key.
			if((listBoxExitKey2.SelectedIndex == listBoxExitKey1.SelectedIndex) ||
				(listBoxExitKey2.SelectedIndex == listBoxExitKey3.SelectedIndex))
				listBoxExitKey2.SelectedIndex = (int)SebInstance.Settings.settingsCurrent[SebSettings.KeyExitKey2];
			SebInstance.Settings.settingsCurrent[SebSettings.KeyExitKey2] = listBoxExitKey2.SelectedIndex;
		}

		private void listBoxExitKey3_SelectedIndexChanged(object sender, EventArgs e)
		{
			// Make sure that all three exit keys are different.
			// If selected key is already occupied, revert to previously selected key.
			if((listBoxExitKey3.SelectedIndex == listBoxExitKey1.SelectedIndex) ||
				(listBoxExitKey3.SelectedIndex == listBoxExitKey2.SelectedIndex))
				listBoxExitKey3.SelectedIndex = (int)SebInstance.Settings.settingsCurrent[SebSettings.KeyExitKey3];
			SebInstance.Settings.settingsCurrent[SebSettings.KeyExitKey3] = listBoxExitKey3.SelectedIndex;
		}

		private void buttonAbout_Click(object sender, EventArgs e)
		{

		}

		private void buttonHelp_Click(object sender, EventArgs e)
		{

		}

		private void buttonQuit_Click(object sender, EventArgs e)
		{
			/*
						// If no file has been opened, save the current settings
						// to the default configuration file ("SebStarter.xml/seb")
						if (currentFileSebStarterIni.Equals(""))
						{
							currentFileSebStarter = defaultFileSebStarter;
							currentPathSebStarter = defaultPathSebStarter;
						}

						// Save the configuration file so that nothing gets lost
						SaveConfigurationFile(currentPathSebStarter);
			*/
			// Close the configuration window and exit
			this.Close();
		}



		// *******************
		// Group "Config File"
		// *******************
		private void radioButtonStartingAnExam_CheckedChanged(object sender, EventArgs e)
		{
			if(radioButtonStartingAnExam.Checked == true)
				SebInstance.Settings.settingsCurrent[SebSettings.KeySebConfigPurpose] = 0;
			else SebInstance.Settings.settingsCurrent[SebSettings.KeySebConfigPurpose] = 1;
		}

		private void radioButtonConfiguringAClient_CheckedChanged(object sender, EventArgs e)
		{
			if(radioButtonConfiguringAClient.Checked == true)
				SebInstance.Settings.settingsCurrent[SebSettings.KeySebConfigPurpose] = 1;
			else SebInstance.Settings.settingsCurrent[SebSettings.KeySebConfigPurpose] = 0;
		}

		private void checkBoxAllowPreferencesWindow_CheckedChanged(object sender, EventArgs e)
		{
			SebInstance.Settings.settingsCurrent[SebSettings.KeyAllowPreferencesWindow] = checkBoxAllowPreferencesWindow.Checked;
		}

		private void comboBoxCryptoIdentity_SelectedIndexChanged(object sender, EventArgs e)
		{
			SebInstance.Settings.intArrayCurrent[SebSettings.ValCryptoIdentity] = comboBoxCryptoIdentity.SelectedIndex;
			SebInstance.Settings.strArrayCurrent[SebSettings.ValCryptoIdentity] = comboBoxCryptoIdentity.Text;
		}

		private void comboBoxCryptoIdentity_TextUpdate(object sender, EventArgs e)
		{
			SebInstance.Settings.intArrayCurrent[SebSettings.ValCryptoIdentity] = comboBoxCryptoIdentity.SelectedIndex;
			SebInstance.Settings.strArrayCurrent[SebSettings.ValCryptoIdentity] = comboBoxCryptoIdentity.Text;
		}

		private void textBoxSettingsPassword_TextChanged(object sender, EventArgs e)
		{
			ComparePasswords(textBoxSettingsPassword, textBoxConfirmSettingsPassword, ref settingsPasswordFieldsContainHash, labelSettingsPasswordCompare, null);
			// We can store the settings password regardless if the same is entered in the confirm text field, 
			// as saving the .seb file is only allowed when they are same
			//settingsPassword = textBoxSettingsPassword.Text;
		}

		private void textBoxConfirmSettingsPassword_TextChanged(object sender, EventArgs e)
		{
			ComparePasswords(textBoxSettingsPassword, textBoxConfirmSettingsPassword, ref settingsPasswordFieldsContainHash, labelSettingsPasswordCompare, null);
			// We can store the settings password regardless if the same is entered in the confirm text field, 
			// as saving the .seb file is only allowed when they are same
			//settingsPassword = textBoxSettingsPassword.Text;
		}

		/// ----------------------------------------------------------------------------------------
		/// <summary>
		/// Check if settings changed since last saved/opened
		/// </summary>
		/// ---------------------------------------------------------------------------------------- 
		private int checkSettingsChanged()
		{
			int result = 0;
			// Generate current Browser Exam Key
			string currentBrowserExamKey = SebProtectionController.ComputeBrowserExamKey();
			if(!lastBrowserExamKey.Equals(currentBrowserExamKey) || !lastSettingsPassword.Equals(textBoxSettingsPassword.Text))
			{
				var messageBoxResult = SebMessageBox.Show(SEBUIStrings.unsavedChangesTitle, SEBUIStrings.unsavedChangesQuestion, MessageBoxImage.Question, MessageBoxButton.YesNoCancel);
				if(messageBoxResult == MessageBoxResult.Yes)
				{
					result = 1;
				}
				if(messageBoxResult == MessageBoxResult.Cancel)
				{
					result = 2;
				}
			}
			return result;
		}

		private void buttonOpenSettings_Click(object sender, EventArgs e)
		{
			// Check if settings changed since last saved/opened
			int result = checkSettingsChanged();
			// User selected cancel, abort
			if(result == 2) return;
			// User selected "Save current settings first: yes"
			if(result == 1)
			{
				// Abort if saving settings failed
				if(!saveCurrentSettings()) return;
			}

			// Set the default directory and file name in the File Dialog
			openFileDialogSebConfigFile.InitialDirectory = currentDireSebConfigFile;
			openFileDialogSebConfigFile.FileName = "";
			openFileDialogSebConfigFile.DefaultExt = "seb";
			openFileDialogSebConfigFile.Filter = "SEB Files|*.seb";

			// Get the user inputs in the File Dialog
			DialogResult fileDialogResult = openFileDialogSebConfigFile.ShowDialog();
			String fileName = openFileDialogSebConfigFile.FileName;

			// If the user clicked "Cancel", do nothing
			// If the user clicked "OK"    , read the settings from the configuration file
			if(fileDialogResult.Equals(DialogResult.Cancel)) return;
			if(fileDialogResult.Equals(DialogResult.OK))
			{
				if(!LoadConfigurationFileIntoEditor(fileName))
				{
					SebMessageBox.Show(SEBUIStrings.openingSettingsFailed, SEBUIStrings.openingSettingsFailedMessage, MessageBoxImage.Error, MessageBoxButton.OK);
					return;
				}
				// Generate Browser Exam Key of this new settings
				lastBrowserExamKey = SebProtectionController.ComputeBrowserExamKey();
				// Save the current settings password so it can be used for comparing later if it changed
				lastSettingsPassword = textBoxSettingsPassword.Text;
				// Display the new Browser Exam Key in Exam pane
				textBoxBrowserExamKey.Text = lastBrowserExamKey;
				// Reset the path of the last saved file which is used in case "Edit duplicate" was used
				lastPathSebConfigFile = null;
			}
		}


		public void openSettingsFile(string filePath)
		{
			// Check if settings changed since last saved/opened
			int result = checkSettingsChanged();
			// User selected cancel, abort
			if(result == 2) return;
			// User selected "Save current settings first: yes"
			// User selected "Save current settings first: yes"
			if(result == 1)
			{
				// Abort if saving settings failed
				if(!saveCurrentSettings()) return;
			}

			if(!LoadConfigurationFileIntoEditor(filePath))
			{
				SebMessageBox.Show(SEBUIStrings.openingSettingsFailed, SEBUIStrings.openingSettingsFailedMessage, MessageBoxImage.Error, MessageBoxButton.OK);
				return;
			}
			// Generate Browser Exam Key of this new settings
			lastBrowserExamKey = SebProtectionController.ComputeBrowserExamKey();
			// Save the current settings password so it can be used for comparing later if it changed
			lastSettingsPassword = textBoxSettingsPassword.Text;
			// Display the new Browser Exam Key in Exam pane
			textBoxBrowserExamKey.Text = lastBrowserExamKey;
			// Reset the path of the last saved file which is used in case "Edit duplicate" was used
			lastPathSebConfigFile = null;
		}


		private void buttonSaveSettings_Click(object sender, EventArgs e)
		{
			saveCurrentSettings();
		}

		public bool saveCurrentSettings()
		{
			// Check if there are unconfirmed passwords, if yes show an alert and abort saving
			if(ArePasswordsUnconfirmed()) return false;

			StringBuilder sebClientSettingsAppDataBuilder = new StringBuilder(currentDireSebConfigFile).Append(@"\").Append(currentFileSebConfigFile);
			String fileName = sebClientSettingsAppDataBuilder.ToString();

			/// Generate Browser Exam Key and its salt, if settings or the settings password changed
			string newBrowserExamKey = SebProtectionController.ComputeBrowserExamKey();
			// Save current Browser Exam Key salt in case saving fails
			byte[] currentExamKeySalt = (byte[])SebInstance.Settings.settingsCurrent[SebSettings.KeyExamKeySalt];

			if(!lastBrowserExamKey.Equals(newBrowserExamKey) || !lastSettingsPassword.Equals(textBoxSettingsPassword.Text))
			{
				// As the exam key changed, we will generate a new salt
				byte[] newExamKeySalt = SebProtectionController.GenerateBrowserExamKeySalt();
				// Save the new salt
				SebInstance.Settings.settingsCurrent[SebSettings.KeyExamKeySalt] = newExamKeySalt;
			}
			if(!SaveConfigurationFileFromEditor(fileName))
			{
				SebMessageBox.Show(SEBUIStrings.savingSettingsFailed, SEBUIStrings.savingSettingsFailedMessage, MessageBoxImage.Error, MessageBoxButton.OK);
				// Restore the old Browser Exam Key salt
				SebInstance.Settings.settingsCurrent[SebSettings.KeyExamKeySalt] = currentExamKeySalt;
				return false;
			}
			// Generate the new Browser Exam Key
			lastBrowserExamKey = SebProtectionController.ComputeBrowserExamKey();
			// Display the new Browser Exam Key in Exam pane
			textBoxBrowserExamKey.Text = lastBrowserExamKey;
			// Save the current settings password so it can be used for comparing later if it changed
			lastSettingsPassword = textBoxSettingsPassword.Text;
			// Reset the path of the last saved file which is used in case "Edit duplicate" was used
			lastPathSebConfigFile = null;
			return true;
		}


		private void buttonSaveSettingsAs_Click(object sender, EventArgs e)
		{
			// Check if there are unconfirmed passwords, if yes show an alert and abort saving
			if(ArePasswordsUnconfirmed()) return;

			// Set the default directory and file name in the File Dialog
			saveFileDialogSebConfigFile.InitialDirectory = currentDireSebConfigFile;
			saveFileDialogSebConfigFile.FileName = currentFileSebConfigFile;

			// Get the user inputs in the File Dialog
			DialogResult fileDialogResult = saveFileDialogSebConfigFile.ShowDialog();
			String fileName = saveFileDialogSebConfigFile.FileName;

			// If the user clicked "Cancel", do nothing
			// If the user clicked "OK"    , write the settings to the configuration file
			if(fileDialogResult.Equals(DialogResult.Cancel)) return;

			// Generate Browser Exam Key and its salt, if settings changed
			string newBrowserExamKey = SebProtectionController.ComputeBrowserExamKey();
			// Save current Browser Exam Key salt in case saving fails
			byte[] currentExamKeySalt = (byte[])SebInstance.Settings.settingsCurrent[SebSettings.KeyExamKeySalt];

			if(!lastBrowserExamKey.Equals(newBrowserExamKey) || !lastSettingsPassword.Equals(textBoxSettingsPassword.Text))
			{
				// If the exam key changed, then settings changed and we will generate a new salt
				byte[] newExamKeySalt = SebProtectionController.GenerateBrowserExamKeySalt();
				// Save the new salt
				SebInstance.Settings.settingsCurrent[SebSettings.KeyExamKeySalt] = newExamKeySalt;
			}
			if(fileDialogResult.Equals(DialogResult.OK))
			{
				if(!SaveConfigurationFileFromEditor(fileName))
				{
					SebMessageBox.Show(SEBUIStrings.savingSettingsFailed, SEBUIStrings.savingSettingsFailedMessage, MessageBoxImage.Error, MessageBoxButton.OK);
					// Restore the old Browser Exam Key salt
					SebInstance.Settings.settingsCurrent[SebSettings.KeyExamKeySalt] = currentExamKeySalt;
					return;
				}
				// Generate the new Browser Exam Key
				lastBrowserExamKey = SebProtectionController.ComputeBrowserExamKey();
				// Display the new Browser Exam Key in Exam pane
				textBoxBrowserExamKey.Text = lastBrowserExamKey;
				// Save the current settings password so it can be used for comparing later if it changed
				lastSettingsPassword = textBoxSettingsPassword.Text;
				// Reset the path of the last saved file which is used in case "Edit duplicate" was used
				lastPathSebConfigFile = null;
			}
		}


		private void buttonRevertToDefaultSettings_Click(object sender, EventArgs e)
		{
			// Check if settings changed since last saved/opened
			int result = checkSettingsChanged();
			// User selected cancel, abort
			if(result == 2) return;
			// User selected "Save current settings first: yes"
			if(result == 1)
			{
				// Abort if saving settings failed
				if(!saveCurrentSettings()) return;
			}

			settingsPassword = "";
			settingsPasswordFieldsContainHash = false;
			SebInstance.Settings.RestoreDefaultAndCurrentSettings();
			SebInstance.Settings.PermitXulRunnerProcess();
			UpdateAllWidgetsOfProgram();
			// Generate Browser Exam Key of default settings
			string currentBrowserExamKey = SebProtectionController.ComputeBrowserExamKey();
			lastSettingsPassword = textBoxSettingsPassword.Text;
			// Display the new Browser Exam Key in Exam pane
			textBoxBrowserExamKey.Text = currentBrowserExamKey;
		}


		private void buttonRevertToLocalClientSettings_Click(object sender, EventArgs e)
		{
			// Check if settings changed since last saved/opened
			int result = checkSettingsChanged();
			// User selected cancel, abort
			if(result == 2) return;
			// User selected "Save current settings first: yes"
			if(result == 1)
			{
				// Abort if saving settings failed
				if(!saveCurrentSettings()) return;
			}

			// Get the path to the local client settings configuration file
			currentDireSebConfigFile = SEBClientInfo.SebClientSettingsAppDataDirectory;
			currentFileSebConfigFile = SebConstants.SEB_CLIENT_CONFIG;
			StringBuilder sebClientSettingsAppDataBuilder = new StringBuilder(currentDireSebConfigFile).Append(currentFileSebConfigFile);
			currentPathSebConfigFile = sebClientSettingsAppDataBuilder.ToString();

			if(!LoadConfigurationFileIntoEditor(currentPathSebConfigFile))
			{
				settingsPassword = "";
				settingsPasswordFieldsContainHash = false;
				SebInstance.Settings.RestoreDefaultAndCurrentSettings();
				SebInstance.Settings.PermitXulRunnerProcess();
				currentPathSebConfigFile = SEBUIStrings.settingsTitleDefaultSettings;
				UpdateAllWidgetsOfProgram();
			}
			// Generate Browser Exam Key of this new settings
			lastBrowserExamKey = SebProtectionController.ComputeBrowserExamKey();
			lastSettingsPassword = textBoxSettingsPassword.Text;
			// Display the new Browser Exam Key in Exam pane
			textBoxBrowserExamKey.Text = lastBrowserExamKey;
		}


		private void buttonRevertToLastOpened_Click(object sender, EventArgs e)
		{
			// Check if settings changed since last saved/opened
			int result = checkSettingsChanged();
			// User selected cancel, abort
			if(result == 2) return;
			// User selected "Save current settings first: yes"
			if(result == 1)
			{
				// Abort if saving settings failed
				if(!saveCurrentSettings()) return;
			}

			if(!LoadConfigurationFileIntoEditor(String.IsNullOrEmpty(lastPathSebConfigFile) ? currentPathSebConfigFile : lastPathSebConfigFile)) return;
			lastPathSebConfigFile = null;
			// Generate Browser Exam Key of this new settings
			lastBrowserExamKey = SebProtectionController.ComputeBrowserExamKey();
			lastSettingsPassword = textBoxSettingsPassword.Text;
			// Display the new Browser Exam Key in Exam pane
			textBoxBrowserExamKey.Text = lastBrowserExamKey;
		}


		private void buttonEditDuplicate_Click(object sender, EventArgs e)
		{
			// Check if settings changed since last saved/opened
			int result = checkSettingsChanged();
			// User selected cancel, abort
			if(result == 2) return;
			// User selected yes: Save current settings first and proceed only, when saving didn't fail
			// User selected "Save current settings first: yes"
			if(result == 1)
			{
				// Abort if saving settings failed
				if(!saveCurrentSettings()) return;
			}

			// Add string " copy" (or " n+1" if the filename already ends with " copy" or " copy n") to the config name filename
			// Get the filename without extension
			string filename = Path.GetFileNameWithoutExtension(currentFileSebConfigFile);
			// Get the extension (should be .seb)
			string extension = Path.GetExtension(currentFileSebConfigFile);
			StringBuilder newFilename = new StringBuilder();
			if(filename.Length == 0)
			{
				newFilename.Append(SEBUIStrings.settingsUntitledFilename);
				extension = ".seb";
			}
			else
			{
				int copyStringPosition = filename.LastIndexOf(SEBUIStrings.settingsDuplicateSuffix);
				if(copyStringPosition == -1)
				{
					newFilename.Append(filename).Append(SEBUIStrings.settingsDuplicateSuffix);
				}
				else
				{
					newFilename.Append(filename.Substring(0, copyStringPosition + SEBUIStrings.settingsDuplicateSuffix.Length));
					string copyNumberString = filename.Substring(copyStringPosition + SEBUIStrings.settingsDuplicateSuffix.Length);
					if(copyNumberString.Length == 0)
					{
						newFilename.Append(" 1");
					}
					else
					{
						int copyNumber = Convert.ToInt16(copyNumberString.Substring(1));
						if(copyNumber == 0)
						{
							newFilename.Append(SEBUIStrings.settingsDuplicateSuffix);
						}
						else
						{
							newFilename.Append(" ").Append((copyNumber + 1).ToString());
						}
					}
				}
			}
			lastPathSebConfigFile = currentPathSebConfigFile;
			currentFileSebConfigFile = newFilename.Append(extension).ToString();

			StringBuilder sebClientSettingsAppDataBuilder = new StringBuilder(currentDireSebConfigFile).Append(@"\").Append(currentFileSebConfigFile);
			currentPathSebConfigFile = sebClientSettingsAppDataBuilder.ToString();
			// Update title of edited settings file
			UpdateAllWidgetsOfProgram();
		}


		private void buttonConfigureClient_Click(object sender, EventArgs e)
		{
			// Get the path to the local client settings configuration file
			StringBuilder sebClientSettingsAppDataBuilder = new StringBuilder(SEBClientInfo.SebClientSettingsAppDataDirectory).Append(SebConstants.SEB_CLIENT_CONFIG);
			string filename = sebClientSettingsAppDataBuilder.ToString();

			// Generate Browser Exam Key and its salt, if settings changed
			string newBrowserExamKey = SebProtectionController.ComputeBrowserExamKey();
			// Save current Browser Exam Key salt in case saving fails
			byte[] currentExamKeySalt = (byte[])SebInstance.Settings.settingsCurrent[SebSettings.KeyExamKeySalt];

			if(!lastBrowserExamKey.Equals(newBrowserExamKey) || !lastSettingsPassword.Equals(textBoxSettingsPassword.Text))
			{
				// If the exam key changed, then settings changed and we will generate a new salt
				byte[] newExamKeySalt = SebProtectionController.GenerateBrowserExamKeySalt();
				// Save the new salt
				SebInstance.Settings.settingsCurrent[SebSettings.KeyExamKeySalt] = newExamKeySalt;
			}
			if(!SaveConfigurationFileFromEditor(filename))
			{
				// SebClientSettings.seb config file wasn't saved successfully, revert changed settings
				// Restore the old Browser Exam Key salt
				SebInstance.Settings.settingsCurrent[SebSettings.KeyExamKeySalt] = currentExamKeySalt;
				return;
			}
			// Generate the new Browser Exam Key
			lastBrowserExamKey = SebProtectionController.ComputeBrowserExamKey();
			// Save the current settings password so it can be used for comparing later if it changed
			lastSettingsPassword = textBoxSettingsPassword.Text;
			// Display the new Browser Exam Key in Exam pane
			textBoxBrowserExamKey.Text = lastBrowserExamKey;
		}


		private void buttonApplyAndStartSEB_Click(object sender, EventArgs e)
		{
			// Check if settings changed and save them if yes
			// Generate current Browser Exam Key
			string currentBrowserExamKey = SebProtectionController.ComputeBrowserExamKey();
			if(!lastBrowserExamKey.Equals(currentBrowserExamKey) || !lastSettingsPassword.Equals(textBoxSettingsPassword.Text) || !String.IsNullOrEmpty(lastPathSebConfigFile))
			{
				if(!saveCurrentSettings()) return;
			}
			// Get the path to the local client settings configuration file
			currentDireSebConfigFile = SEBClientInfo.SebClientSettingsAppDataDirectory;
			currentFileSebConfigFile = SebConstants.SEB_CLIENT_CONFIG;
			StringBuilder sebClientSettingsAppDataBuilder = new StringBuilder(currentDireSebConfigFile).Append(currentFileSebConfigFile);
			string localSebClientSettings = sebClientSettingsAppDataBuilder.ToString();

			StringBuilder sebClientExeBuilder = new StringBuilder(SEBClientInfo.SebClientDirectory).Append(SebConstants.FILENAME_SEB);
			string sebClientExe = sebClientExeBuilder.ToString();
			if(!File.Exists(sebClientExe))
			{
				sebClientExe = Path.Combine(Path.GetDirectoryName(new Uri(Assembly.GetAssembly(typeof(SEBClientInfo)).CodeBase).LocalPath), SebConstants.FILENAME_SEB);
			}

			var p = new Process();
			p.StartInfo.FileName = sebClientExe;

			if(!currentPathSebConfigFile.Equals(localSebClientSettings) && !currentPathSebConfigFile.Equals(SEBUIStrings.settingsTitleDefaultSettings))
			{
				p.StartInfo.Arguments = String.Format("\"{0}\"", currentPathSebConfigFile);
			}

			p.Start();

			Application.Exit();
		}


		// ******************
		// Group "Appearance"
		// ******************
		private void radioButtonUseBrowserWindow_CheckedChanged(object sender, EventArgs e)
		{
			if(radioButtonUseBrowserWindow.Checked == true)
			{
				groupBoxMainBrowserWindow.Enabled = true;
				SebInstance.Settings.settingsCurrent[SebSettings.KeyBrowserViewMode] = 0;
				SebInstance.Settings.settingsCurrent[SebSettings.KeyTouchOptimized] = false;
				SebInstance.Settings.settingsCurrent[SebSettings.KeyBrowserScreenKeyboard] = false;
			}
		}

		private void radioButtonUseFullScreenMode_CheckedChanged(object sender, EventArgs e)
		{
			if(radioButtonUseFullScreenMode.Checked == true)
			{
				groupBoxMainBrowserWindow.Enabled = false;
				SebInstance.Settings.settingsCurrent[SebSettings.KeyBrowserViewMode] = 1;
				SebInstance.Settings.settingsCurrent[SebSettings.KeyTouchOptimized] = false;
				SebInstance.Settings.settingsCurrent[SebSettings.KeyBrowserScreenKeyboard] = false;
			}
		}

		private void radioButtonTouchOptimized_CheckedChanged(object sender, EventArgs e)
		{
			if(radioButtonTouchOptimized.Checked == true)
			{
				if((Boolean)SebInstance.Settings.settingsCurrent[SebSettings.KeyCreateNewDesktop])
				{
					MessageBox.Show(
					"Touch optimization will not work when kiosk mode is set to 'Create new desktop', please change kiosk mode to 'Disable Explorer Shell' in the Security tab.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}
				groupBoxMainBrowserWindow.Enabled = false;
				SebInstance.Settings.settingsCurrent[SebSettings.KeyBrowserViewMode] = 1;
				SebInstance.Settings.settingsCurrent[SebSettings.KeyNewBrowserWindowByLinkWidth] = "100%";
				SebInstance.Settings.settingsCurrent[SebSettings.KeyNewBrowserWindowByLinkHeight] = "100%";
				SebInstance.Settings.settingsCurrent[SebSettings.KeyBrowserViewMode] = 1;
				SebInstance.Settings.settingsCurrent[SebSettings.KeyTouchOptimized] = true;
				SebInstance.Settings.settingsCurrent[SebSettings.KeyBrowserScreenKeyboard] = true;
			}
		}

		private void comboBoxMainBrowserWindowWidth_SelectedIndexChanged(object sender, EventArgs e)
		{
			SebInstance.Settings.intArrayCurrent[SebSettings.ValMainBrowserWindowWidth] = comboBoxMainBrowserWindowWidth.SelectedIndex;
			SebInstance.Settings.strArrayCurrent[SebSettings.ValMainBrowserWindowWidth] = comboBoxMainBrowserWindowWidth.Text;
			//SebSettings.settingsCurrent[SebSettings.KeyMainBrowserWindowWidth] = comboBoxMainBrowserWindowWidth.SelectedIndex;
			SebInstance.Settings.settingsCurrent[SebSettings.KeyMainBrowserWindowWidth] = comboBoxMainBrowserWindowWidth.Text;
		}

		private void comboBoxMainBrowserWindowWidth_TextUpdate(object sender, EventArgs e)
		{
			SebInstance.Settings.intArrayCurrent[SebSettings.ValMainBrowserWindowWidth] = comboBoxMainBrowserWindowWidth.SelectedIndex;
			SebInstance.Settings.strArrayCurrent[SebSettings.ValMainBrowserWindowWidth] = comboBoxMainBrowserWindowWidth.Text;
			//SebSettings.settingsCurrent[SebSettings.KeyMainBrowserWindowWidth] = comboBoxMainBrowserWindowWidth.SelectedIndex;
			SebInstance.Settings.settingsCurrent[SebSettings.KeyMainBrowserWindowWidth] = comboBoxMainBrowserWindowWidth.Text;
		}

		private void comboBoxMainBrowserWindowHeight_SelectedIndexChanged(object sender, EventArgs e)
		{
			SebInstance.Settings.intArrayCurrent[SebSettings.ValMainBrowserWindowHeight] = comboBoxMainBrowserWindowHeight.SelectedIndex;
			SebInstance.Settings.strArrayCurrent[SebSettings.ValMainBrowserWindowHeight] = comboBoxMainBrowserWindowHeight.Text;
			//SebSettings.settingsCurrent[SebSettings.KeyMainBrowserWindowHeight] = comboBoxMainBrowserWindowHeight.SelectedIndex;
			SebInstance.Settings.settingsCurrent[SebSettings.KeyMainBrowserWindowHeight] = comboBoxMainBrowserWindowHeight.Text;
		}

		private void comboBoxMainBrowserWindowHeight_TextUpdate(object sender, EventArgs e)
		{
			SebInstance.Settings.intArrayCurrent[SebSettings.ValMainBrowserWindowHeight] = comboBoxMainBrowserWindowHeight.SelectedIndex;
			SebInstance.Settings.strArrayCurrent[SebSettings.ValMainBrowserWindowHeight] = comboBoxMainBrowserWindowHeight.Text;
			//SebSettings.settingsCurrent[SebSettings.KeyMainBrowserWindowHeight] = comboBoxMainBrowserWindowHeight.SelectedIndex;
			SebInstance.Settings.settingsCurrent[SebSettings.KeyMainBrowserWindowHeight] = comboBoxMainBrowserWindowHeight.Text;
		}

		private void listBoxMainBrowserWindowPositioning_SelectedIndexChanged(object sender, EventArgs e)
		{
			SebInstance.Settings.settingsCurrent[SebSettings.KeyMainBrowserWindowPositioning] = listBoxMainBrowserWindowPositioning.SelectedIndex;
		}

		private void checkBoxEnableBrowserWindowToolbar_CheckedChanged(object sender, EventArgs e)
		{
			SebInstance.Settings.settingsCurrent[SebSettings.KeyEnableBrowserWindowToolbar] = checkBoxEnableBrowserWindowToolbar.Checked;
			checkBoxHideBrowserWindowToolbar.Enabled = checkBoxEnableBrowserWindowToolbar.Checked;
		}

		private void checkBoxHideBrowserWindowToolbar_CheckedChanged(object sender, EventArgs e)
		{
			SebInstance.Settings.settingsCurrent[SebSettings.KeyHideBrowserWindowToolbar] = checkBoxHideBrowserWindowToolbar.Checked;
		}

		private void checkBoxShowMenuBar_CheckedChanged(object sender, EventArgs e)
		{
			SebInstance.Settings.settingsCurrent[SebSettings.KeyShowMenuBar] = checkBoxShowMenuBar.Checked;
		}

		private void checkBoxShowTaskBar_CheckedChanged(object sender, EventArgs e)
		{
			SebInstance.Settings.settingsCurrent[SebSettings.KeyShowTaskBar] = checkBoxShowTaskBar.Checked;
			comboBoxTaskBarHeight.Enabled = checkBoxShowTaskBar.Checked;
		}

		private void comboBoxTaskBarHeight_SelectedIndexChanged(object sender, EventArgs e)
		{
			SebInstance.Settings.intArrayCurrent[SebSettings.ValTaskBarHeight] = comboBoxTaskBarHeight.SelectedIndex;
			SebInstance.Settings.strArrayCurrent[SebSettings.ValTaskBarHeight] = comboBoxTaskBarHeight.Text;
			//SebSettings.settingsCurrent[SebSettings.KeyTaskBarHeight] = comboBoxTaskBarHeight.SelectedIndex;
			SebInstance.Settings.settingsCurrent[SebSettings.KeyTaskBarHeight] = Int32.Parse(comboBoxTaskBarHeight.Text);
		}

		private void comboBoxTaskBarHeight_TextUpdate(object sender, EventArgs e)
		{
			SebInstance.Settings.intArrayCurrent[SebSettings.ValTaskBarHeight] = comboBoxTaskBarHeight.SelectedIndex;
			SebInstance.Settings.strArrayCurrent[SebSettings.ValTaskBarHeight] = comboBoxTaskBarHeight.Text;
			//SebSettings.settingsCurrent[SebSettings.KeyTaskBarHeight] = comboBoxTaskBarHeight.SelectedIndex;
			SebInstance.Settings.settingsCurrent[SebSettings.KeyTaskBarHeight] = Int32.Parse(comboBoxTaskBarHeight.Text);
		}


		// ***************
		// Group "Browser"
		// ***************
		private void listBoxOpenLinksHTML_SelectedIndexChanged(object sender, EventArgs e)
		{
			SebInstance.Settings.settingsCurrent[SebSettings.KeyNewBrowserWindowByLinkPolicy] = listBoxOpenLinksHTML.SelectedIndex;
		}

		private void listBoxOpenLinksJava_SelectedIndexChanged(object sender, EventArgs e)
		{
			SebInstance.Settings.settingsCurrent[SebSettings.KeyNewBrowserWindowByScriptPolicy] = listBoxOpenLinksJava.SelectedIndex;
		}

		private void checkBoxBlockLinksHTML_CheckedChanged(object sender, EventArgs e)
		{
			SebInstance.Settings.settingsCurrent[SebSettings.KeyNewBrowserWindowByLinkBlockForeign] = checkBoxBlockLinksHTML.Checked;
		}

		private void checkBoxBlockLinksJava_CheckedChanged(object sender, EventArgs e)
		{
			SebInstance.Settings.settingsCurrent[SebSettings.KeyNewBrowserWindowByScriptBlockForeign] = checkBoxBlockLinksJava.Checked;
		}

		private void comboBoxNewBrowserWindowWidth_SelectedIndexChanged(object sender, EventArgs e)
		{
			SebInstance.Settings.intArrayCurrent[SebSettings.ValNewBrowserWindowByLinkWidth] = comboBoxNewBrowserWindowWidth.SelectedIndex;
			SebInstance.Settings.strArrayCurrent[SebSettings.ValNewBrowserWindowByLinkWidth] = comboBoxNewBrowserWindowWidth.Text;
			//SebSettings.settingsCurrent[SebSettings.KeyNewBrowserWindowByLinkWidth] = comboBoxNewBrowserWindowWidth.SelectedIndex;
			SebInstance.Settings.settingsCurrent[SebSettings.KeyNewBrowserWindowByLinkWidth] = comboBoxNewBrowserWindowWidth.Text;
		}

		private void comboBoxNewBrowserWindowWidth_TextUpdate(object sender, EventArgs e)
		{
			SebInstance.Settings.intArrayCurrent[SebSettings.ValNewBrowserWindowByLinkWidth] = comboBoxNewBrowserWindowWidth.SelectedIndex;
			SebInstance.Settings.strArrayCurrent[SebSettings.ValNewBrowserWindowByLinkWidth] = comboBoxNewBrowserWindowWidth.Text;
			//SebSettings.settingsCurrent[SebSettings.KeyNewBrowserWindowByLinkWidth] = comboBoxNewBrowserWindowWidth.SelectedIndex;
			SebInstance.Settings.settingsCurrent[SebSettings.KeyNewBrowserWindowByLinkWidth] = comboBoxNewBrowserWindowWidth.Text;
		}

		private void comboBoxNewBrowserWindowHeight_SelectedIndexChanged(object sender, EventArgs e)
		{
			SebInstance.Settings.intArrayCurrent[SebSettings.ValNewBrowserWindowByLinkHeight] = comboBoxNewBrowserWindowHeight.SelectedIndex;
			SebInstance.Settings.strArrayCurrent[SebSettings.ValNewBrowserWindowByLinkHeight] = comboBoxNewBrowserWindowHeight.Text;
			//SebSettings.settingsCurrent[SebSettings.KeyNewBrowserWindowByLinkHeight] = comboBoxNewBrowserWindowHeight.SelectedIndex;
			SebInstance.Settings.settingsCurrent[SebSettings.KeyNewBrowserWindowByLinkHeight] = comboBoxNewBrowserWindowHeight.Text;
		}

		private void comboBoxNewBrowserWindowHeight_TextUpdate(object sender, EventArgs e)
		{
			SebInstance.Settings.intArrayCurrent[SebSettings.ValNewBrowserWindowByLinkHeight] = comboBoxNewBrowserWindowHeight.SelectedIndex;
			SebInstance.Settings.strArrayCurrent[SebSettings.ValNewBrowserWindowByLinkHeight] = comboBoxNewBrowserWindowHeight.Text;
			//SebSettings.settingsCurrent[SebSettings.KeyNewBrowserWindowByLinkHeight] = comboBoxNewBrowserWindowHeight.SelectedIndex;
			SebInstance.Settings.settingsCurrent[SebSettings.KeyNewBrowserWindowByLinkHeight] = comboBoxNewBrowserWindowHeight.Text;
		}

		private void listBoxNewBrowserWindowPositioning_SelectedIndexChanged(object sender, EventArgs e)
		{
			SebInstance.Settings.settingsCurrent[SebSettings.KeyNewBrowserWindowByLinkPositioning] = listBoxNewBrowserWindowPositioning.SelectedIndex;
		}

		private void checkBoxEnablePlugins_CheckedChanged(object sender, EventArgs e)
		{
			SebInstance.Settings.settingsCurrent[SebSettings.KeyEnablePlugIns] = checkBoxEnablePlugIns.Checked;
		}

		private void checkBoxEnableJava_CheckedChanged(object sender, EventArgs e)
		{
			SebInstance.Settings.settingsCurrent[SebSettings.KeyEnableJava] = checkBoxEnableJava.Checked;
		}

		private void checkBoxEnableJavaScript_CheckedChanged(object sender, EventArgs e)
		{
			SebInstance.Settings.settingsCurrent[SebSettings.KeyEnableJavaScript] = checkBoxEnableJavaScript.Checked;
		}

		private void checkBoxBlockPopUpWindows_CheckedChanged(object sender, EventArgs e)
		{
			SebInstance.Settings.settingsCurrent[SebSettings.KeyBlockPopUpWindows] = checkBoxBlockPopUpWindows.Checked;
		}

		private void checkBoxAllowBrowsingBackForward_CheckedChanged(object sender, EventArgs e)
		{
			SebInstance.Settings.settingsCurrent[SebSettings.KeyAllowBrowsingBackForward] = checkBoxAllowBrowsingBackForward.Checked;
			checkBoxEnableAltMouseWheel.Checked = checkBoxAllowBrowsingBackForward.Checked;
		}

		private void checkBoxRemoveProfile_CheckedChanged(object sender, EventArgs e)
		{
			SebInstance.Settings.settingsCurrent[SebSettings.KeyRemoveBrowserProfile] = checkBoxRemoveProfile.Checked;
		}

		private void checkBoxDisableLocalStorage_CheckedChanged(object sender, EventArgs e)
		{
			SebInstance.Settings.settingsCurrent[SebSettings.KeyDisableLocalStorage] = checkBoxDisableLocalStorage.Checked;
		}

		// BEWARE: you must invert this value since "Use Without" is "Not Enable"!
		private void checkBoxUseSebWithoutBrowser_CheckedChanged(object sender, EventArgs e)
		{
			SebInstance.Settings.settingsCurrent[SebSettings.KeyEnableSebBrowser] = !(checkBoxUseSebWithoutBrowser.Checked);
		}

		private void checkBoxShowReloadButton_CheckedChanged(object sender, EventArgs e)
		{
			SebInstance.Settings.settingsCurrent[SebSettings.KeyShowReloadButton] = checkBoxShowReloadButton.Checked;
		}

		private void checkBoxShowReloadWarning_CheckedChanged(object sender, EventArgs e)
		{
			SebInstance.Settings.settingsCurrent[SebSettings.KeyShowReloadWarning] = checkBoxShowReloadWarning.Checked;
		}

		private void radioButtonUseZoomPage_CheckedChanged(object sender, EventArgs e)
		{
			if(radioButtonUseZoomPage.Checked == true)
				SebInstance.Settings.settingsCurrent[SebSettings.KeyZoomMode] = 0;
			else SebInstance.Settings.settingsCurrent[SebSettings.KeyZoomMode] = 1;
		}

		private void radioButtonUseZoomText_CheckedChanged(object sender, EventArgs e)
		{
			if(radioButtonUseZoomText.Checked == true)
				SebInstance.Settings.settingsCurrent[SebSettings.KeyZoomMode] = 1;
			else SebInstance.Settings.settingsCurrent[SebSettings.KeyZoomMode] = 0;
		}

		private void radioButtonUserAgentDesktopDefault_CheckedChanged(object sender, EventArgs e)
		{
			if(radioButtonUserAgentDesktopDefault.Checked == true)
				SebInstance.Settings.settingsCurrent[SebSettings.KeyBrowserUserAgentDesktopMode] = 0;
			//else SebSettings.settingsCurrent[SebSettings.KeyBrowserUserAgentDesktopMode] = 1;
		}

		private void radioButtonUserAgentDesktopCustom_CheckedChanged(object sender, EventArgs e)
		{
			if(radioButtonUserAgentDesktopCustom.Checked == true)
				SebInstance.Settings.settingsCurrent[SebSettings.KeyBrowserUserAgentDesktopMode] = 1;
			//else SebSettings.settingsCurrent[SebSettings.KeyBrowserUserAgentDesktopMode] = 0;
		}

		private void textBoxUserAgentDesktopModeCustom_TextChanged(object sender, EventArgs e)
		{
			SebInstance.Settings.settingsCurrent[SebSettings.KeyBrowserUserAgentDesktopModeCustom] = textBoxUserAgentDesktopModeCustom.Text;
			radioButtonUserAgentDesktopCustom.Checked = true;
		}

		private void radioButtonUserAgentTouchDefault_CheckedChanged(object sender, EventArgs e)
		{
			if(radioButtonUserAgentTouchDefault.Checked == true)
				SebInstance.Settings.settingsCurrent[SebSettings.KeyBrowserUserAgentTouchMode] = 0;
			//else SebSettings.settingsCurrent[SebSettings.KeyBrowserUserAgentTouchMode] = 1;
		}

		private void radioButtonUserAgentTouchIPad_CheckedChanged(object sender, EventArgs e)
		{
			if(radioButtonUserAgentTouchIPad.Checked == true)
				SebInstance.Settings.settingsCurrent[SebSettings.KeyBrowserUserAgentTouchMode] = 1;
		}

		private void radioButtonUserAgentTouchCustom_CheckedChanged(object sender, EventArgs e)
		{
			if(radioButtonUserAgentTouchCustom.Checked == true)
				SebInstance.Settings.settingsCurrent[SebSettings.KeyBrowserUserAgentTouchMode] = 2;
			//else SebSettings.settingsCurrent[SebSettings.KeyBrowserUserAgentTouchMode] = 0;
		}

		private void textBoxUserAgentTouchModeCustom_TextChanged(object sender, EventArgs e)
		{
			SebInstance.Settings.settingsCurrent[SebSettings.KeyBrowserUserAgentTouchModeCustom] = textBoxUserAgentTouchModeCustom.Text;
			radioButtonUserAgentTouchCustom.Checked = true;
		}


		private void radioButtonUserAgentMacDefault_CheckedChanged(object sender, EventArgs e)
		{
			if(radioButtonUserAgentMacDefault.Checked == true)
				SebInstance.Settings.settingsCurrent[SebSettings.KeyBrowserUserAgentMac] = 0;
		}

		private void radioButtonUserAgentMacCustom_CheckedChanged(object sender, EventArgs e)
		{
			if(radioButtonUserAgentMacCustom.Checked == true)
				SebInstance.Settings.settingsCurrent[SebSettings.KeyBrowserUserAgentMac] = 1;
		}

		private void textBoxUserAgentMacCustom_TextChanged(object sender, EventArgs e)
		{
			SebInstance.Settings.settingsCurrent[SebSettings.KeyBrowserUserAgentMacCustom] = textBoxUserAgentMacCustom.Text;
			radioButtonUserAgentMacCustom.Checked = true;
		}


		// ********************
		// Group "Down/Uploads"
		// ********************
		private void checkBoxAllowDownUploads_CheckedChanged(object sender, EventArgs e)
		{
			SebInstance.Settings.settingsCurrent[SebSettings.KeyAllowDownUploads] = checkBoxAllowDownUploads.Checked;
		}

		private void buttonDownloadDirectoryWin_Click(object sender, EventArgs e)
		{
			// Set the default directory in the Folder Browser Dialog
			folderBrowserDialogDownloadDirectoryWin.RootFolder = Environment.SpecialFolder.DesktopDirectory;
			//          folderBrowserDialogDownloadDirectoryWin.RootFolder = Environment.CurrentDirectory;

			// Get the user inputs in the File Dialog
			DialogResult dialogResult = folderBrowserDialogDownloadDirectoryWin.ShowDialog();
			String path = folderBrowserDialogDownloadDirectoryWin.SelectedPath;

			// If the user clicked "Cancel", do nothing
			if(dialogResult.Equals(DialogResult.Cancel)) return;

			// If the user clicked "OK", ...
			string pathUsingEnvironmentVariables = SEBClientInfo.ContractEnvironmentVariables(path);
			SebInstance.Settings.settingsCurrent[SebSettings.KeyDownloadDirectoryWin] = pathUsingEnvironmentVariables;
			textBoxDownloadDirectoryWin.Text = pathUsingEnvironmentVariables;
		}

		private void textBoxDownloadDirectoryWin_TextChanged(object sender, EventArgs e)
		{
			SebInstance.Settings.settingsCurrent[SebSettings.KeyDownloadDirectoryWin] = textBoxDownloadDirectoryWin.Text;
		}

		private void textBoxDownloadDirectoryOSX_TextChanged(object sender, EventArgs e)
		{
			SebInstance.Settings.settingsCurrent[SebSettings.KeyDownloadDirectoryOSX] = textBoxDownloadDirectoryOSX.Text;
		}

		private void checkBoxDownloadOpenSEBFiles_CheckedChanged(object sender, EventArgs e)
		{
			SebInstance.Settings.settingsCurrent[SebSettings.KeyDownloadAndOpenSebConfig] = checkBoxDownloadOpenSEBFiles.Checked;
		}

		private void checkBoxOpenDownloads_CheckedChanged(object sender, EventArgs e)
		{
			SebInstance.Settings.settingsCurrent[SebSettings.KeyOpenDownloads] = checkBoxOpenDownloads.Checked;
		}

		private void listBoxChooseFileToUploadPolicy_SelectedIndexChanged(object sender, EventArgs e)
		{
			SebInstance.Settings.settingsCurrent[SebSettings.KeyChooseFileToUploadPolicy] = listBoxChooseFileToUploadPolicy.SelectedIndex;
		}

		private void checkBoxDownloadPDFFiles_CheckedChanged(object sender, EventArgs e)
		{
			SebInstance.Settings.settingsCurrent[SebSettings.KeyDownloadPDFFiles] = checkBoxDownloadPDFFiles.Checked;
		}



		// ************
		// Group "Exam"
		// ************
		private void buttonGenerateBrowserExamKey_Click(object sender, EventArgs e)
		{
			textBoxBrowserExamKey.Text = SebProtectionController.ComputeBrowserExamKey();
		}

		private void textBoxBrowserExamKey_TextChanged(object sender, EventArgs e)
		{
			//SebSettings.settingsCurrent[SebSettings.KeyBrowserExamKey] = textBoxBrowserExamKey.Text;
		}

		private void checkBoxSendBrowserExamKey_CheckedChanged(object sender, EventArgs e)
		{
			SebInstance.Settings.settingsCurrent[SebSettings.KeySendBrowserExamKey] = checkBoxSendBrowserExamKey.Checked;
		}

		private void textBoxQuitURL_TextChanged(object sender, EventArgs e)
		{
			SebInstance.Settings.settingsCurrent[SebSettings.KeyQuitURL] = textBoxQuitURL.Text;
		}

		private void checkBoxUseStartURL_CheckedChanged(object sender, EventArgs e)
		{
			SebInstance.Settings.settingsCurrent[SebSettings.KeyRestartExamUseStartURL] = checkBoxUseStartURL.Checked;
			textBoxRestartExamLink.Enabled = !checkBoxUseStartURL.Checked;
		}

		private void textBoxRestartExamLink_TextChanged(object sender, EventArgs e)
		{
			SebInstance.Settings.settingsCurrent[SebSettings.KeyRestartExamURL] = textBoxRestartExamLink.Text;
		}

		private void textBoxRestartExamText_TextChanged(object sender, EventArgs e)
		{
			SebInstance.Settings.settingsCurrent[SebSettings.KeyRestartExamText] = textBoxRestartExamText.Text;
		}

		private void checkBoxRestartExamPasswordProtected_CheckedChanged(object sender, EventArgs e)
		{
			SebInstance.Settings.settingsCurrent[SebSettings.KeyRestartExamPasswordProtected] = checkBoxRestartExamPasswordProtected.Checked;
		}



		// ********************
		// Group "Applications"
		// ********************
		private void checkBoxMonitorProcesses_CheckedChanged(object sender, EventArgs e)
		{
			SebInstance.Settings.settingsCurrent[SebSettings.KeyMonitorProcesses] = checkBoxMonitorProcesses.Checked;
		}


		// ******************************************
		// Group "Applications - Permitted Processes"
		// ******************************************
		private void checkBoxAllowSwitchToApplications_CheckedChanged(object sender, EventArgs e)
		{
			SebInstance.Settings.settingsCurrent[SebSettings.KeyAllowSwitchToApplications] = checkBoxAllowSwitchToApplications.Checked;
			checkBoxAllowFlashFullscreen.Enabled = checkBoxAllowSwitchToApplications.Checked;
		}

		private void checkBoxAllowFlashFullscreen_CheckedChanged(object sender, EventArgs e)
		{
			SebInstance.Settings.settingsCurrent[SebSettings.KeyAllowFlashFullscreen] = checkBoxAllowFlashFullscreen.Checked;
		}


		private void LoadAndUpdatePermittedSelectedProcessGroup(int selectedProcessIndex)
		{
			// Get the process data of the selected process
			SebInstance.Settings.permittedProcessList = (ListObj)SebInstance.Settings.settingsCurrent[SebSettings.KeyPermittedProcesses];
			SebInstance.Settings.permittedProcessData = (DictObj)SebInstance.Settings.permittedProcessList[selectedProcessIndex];
			SebInstance.Settings.permittedArgumentList = (ListObj)SebInstance.Settings.permittedProcessData[SebSettings.KeyArguments];

			// Beware double events:
			// Update the widgets in "Selected Process" group,
			// but prevent the following "widget changed" event from firing the "cell changed" event once more!
			ignoreWidgetEventPermittedProcessesActive = true;
			ignoreWidgetEventPermittedProcessesOS = true;
			ignoreWidgetEventPermittedProcessesExecutable = true;
			ignoreWidgetEventPermittedProcessesTitle = true;

			// Update the widgets in the "Selected Process" group
			checkBoxPermittedProcessActive.Checked = (Boolean)SebInstance.Settings.permittedProcessData[SebSettings.KeyActive];
			checkBoxPermittedProcessAutostart.Checked = (Boolean)SebInstance.Settings.permittedProcessData[SebSettings.KeyAutostart];
			checkBoxPermittedProcessIconInTaskbar.Checked = (Boolean)SebInstance.Settings.permittedProcessData[SebSettings.KeyIconInTaskbar];
			checkBoxPermittedProcessAutohide.Checked = (Boolean)SebInstance.Settings.permittedProcessData[SebSettings.KeyRunInBackground];
			checkBoxPermittedProcessIconInTaskbar.Enabled = !checkBoxPermittedProcessAutohide.Checked | checkBoxPermittedProcessAutostart.Checked;
			checkBoxPermittedProcessAllowUser.Checked = (Boolean)SebInstance.Settings.permittedProcessData[SebSettings.KeyAllowUser];
			checkBoxPermittedProcessStrongKill.Checked = (Boolean)SebInstance.Settings.permittedProcessData[SebSettings.KeyStrongKill];
			listBoxPermittedProcessOS.SelectedIndex = (Int32)SebInstance.Settings.permittedProcessData[SebSettings.KeyOS];
			textBoxPermittedProcessTitle.Text = (String)SebInstance.Settings.permittedProcessData[SebSettings.KeyTitle];
			textBoxPermittedProcessDescription.Text = (String)SebInstance.Settings.permittedProcessData[SebSettings.KeyDescription];
			textBoxPermittedProcessExecutable.Text = (String)SebInstance.Settings.permittedProcessData[SebSettings.KeyExecutable];
			textBoxPermittedProcessExecutables.Text = (String)SebInstance.Settings.permittedProcessData[SebSettings.KeyWindowHandlingProcess];
			textBoxPermittedProcessPath.Text = (String)SebInstance.Settings.permittedProcessData[SebSettings.KeyPath];
			textBoxPermittedProcessIdentifier.Text = (String)SebInstance.Settings.permittedProcessData[SebSettings.KeyIdentifier];

			// Reset the ignore widget event flags
			ignoreWidgetEventPermittedProcessesActive = false;
			ignoreWidgetEventPermittedProcessesOS = false;
			ignoreWidgetEventPermittedProcessesExecutable = false;
			ignoreWidgetEventPermittedProcessesTitle = false;

			// Check if selected process has any arguments
			if(SebInstance.Settings.permittedArgumentList.Count > 0)
				SebInstance.Settings.permittedArgumentIndex = 0;
			else SebInstance.Settings.permittedArgumentIndex = -1;

			// Remove all previously displayed arguments from DataGridView
			dataGridViewPermittedProcessArguments.Enabled = (SebInstance.Settings.permittedArgumentList.Count > 0);
			dataGridViewPermittedProcessArguments.Rows.Clear();

			// Add arguments of selected process to DataGridView
			for(int index = 0; index < SebInstance.Settings.permittedArgumentList.Count; index++)
			{
				SebInstance.Settings.permittedArgumentData = (DictObj)SebInstance.Settings.permittedArgumentList[index];
				Boolean active = (Boolean)SebInstance.Settings.permittedArgumentData[SebSettings.KeyActive];
				String argument = (String)SebInstance.Settings.permittedArgumentData[SebSettings.KeyArgument];
				dataGridViewPermittedProcessArguments.Rows.Add(active, argument);
			}

			// Get the selected argument data
			if(SebInstance.Settings.permittedArgumentList.Count > 0)
				SebInstance.Settings.permittedArgumentData = (DictObj)SebInstance.Settings.permittedArgumentList[SebInstance.Settings.permittedArgumentIndex];
		}


		private void ClearPermittedSelectedProcessGroup()
		{
			// Beware double events:
			// Update the widgets in "Selected Process" group,
			// but prevent the following "widget changed" event from firing the "cell changed" event once more!
			ignoreWidgetEventPermittedProcessesActive = true;
			ignoreWidgetEventPermittedProcessesOS = true;
			ignoreWidgetEventPermittedProcessesExecutable = true;
			ignoreWidgetEventPermittedProcessesTitle = true;

			// Clear the widgets in the "Selected Process" group
			checkBoxPermittedProcessActive.Checked = true;
			checkBoxPermittedProcessAutostart.Checked = true;
			checkBoxPermittedProcessAutohide.Checked = true;
			checkBoxPermittedProcessAllowUser.Checked = true;
			checkBoxPermittedProcessStrongKill.Checked = false;
			listBoxPermittedProcessOS.SelectedIndex = IntWin;
			textBoxPermittedProcessTitle.Text = "";
			textBoxPermittedProcessDescription.Text = "";
			textBoxPermittedProcessExecutable.Text = "";
			textBoxPermittedProcessExecutables.Text = "";
			textBoxPermittedProcessPath.Text = "";
			textBoxPermittedProcessIdentifier.Text = "";

			// Reset the ignore widget event flags
			ignoreWidgetEventPermittedProcessesActive = false;
			ignoreWidgetEventPermittedProcessesOS = false;
			ignoreWidgetEventPermittedProcessesExecutable = false;
			ignoreWidgetEventPermittedProcessesTitle = false;

			// Remove all previously displayed arguments from DataGridView
			dataGridViewPermittedProcessArguments.Enabled = false;
			dataGridViewPermittedProcessArguments.Rows.Clear();
		}


		private void dataGridViewPermittedProcesses_SelectionChanged(object sender, EventArgs e)
		{
			// CAUTION:
			// If a row was previously selected and the user clicks onto another row,
			// the SelectionChanged() event is fired TWICE!!!
			// The first time, it is only for UNselecting the old row,
			// so the SelectedRows.Count is ZERO, so ignore this event handler!
			// The second time, SelectedRows.Count is ONE.
			// Now you can set the widgets in the "Selected Process" groupBox.

			if(dataGridViewPermittedProcesses.SelectedRows.Count != 1) return;
			SebInstance.Settings.permittedProcessIndex = dataGridViewPermittedProcesses.SelectedRows[0].Index;

			// The process list should contain at least one element here:
			// SebSettings.permittedProcessList.Count >  0
			// SebSettings.permittedProcessIndex      >= 0
			LoadAndUpdatePermittedSelectedProcessGroup(SebInstance.Settings.permittedProcessIndex);
		}


		private void dataGridViewPermittedProcesses_CurrentCellDirtyStateChanged(object sender, EventArgs e)
		{
			// When a CheckBox/ListBox/TextBox entry of a DataGridView table cell is edited,
			// immediately call the CellValueChanged() event,
			// which will update the SelectedProcess data and widgets.
			if(dataGridViewPermittedProcesses.IsCurrentCellDirty)
				dataGridViewPermittedProcesses.CommitEdit(DataGridViewDataErrorContexts.Commit);
		}


		private void dataGridViewPermittedProcesses_CellValueChanged(object sender, DataGridViewCellEventArgs e)
		{
			// Prevent double events from switching to false process index
			if(ignoreCellEventPermittedProcessesActive == true) return;
			if(ignoreCellEventPermittedProcessesOS == true) return;
			if(ignoreCellEventPermittedProcessesExecutable == true) return;
			if(ignoreCellEventPermittedProcessesTitle == true) return;

			// Get the current cell where the user has changed a value
			int row = dataGridViewPermittedProcesses.CurrentCellAddress.Y;
			int column = dataGridViewPermittedProcesses.CurrentCellAddress.X;

			// At the beginning, row = -1 and column = -1, so skip this event
			if(row < 0) return;
			if(column < 0) return;

			// Get the changed value of the current cell
			object value = dataGridViewPermittedProcesses.CurrentCell.EditedFormattedValue;

			// Convert the selected "OS" ListBox entry from String to Integer
			if(column == IntColumnProcessOS)
			{
				if((String)value == StringOSX) value = IntOSX;
				else if((String)value == StringWin) value = IntWin;
			}

			// Get the process data of the process belonging to the current row
			SebInstance.Settings.permittedProcessIndex = row;
			SebInstance.Settings.permittedProcessList = (ListObj)SebInstance.Settings.settingsCurrent[SebSettings.KeyPermittedProcesses];
			SebInstance.Settings.permittedProcessData = (DictObj)SebInstance.Settings.permittedProcessList[SebInstance.Settings.permittedProcessIndex];

			// Update the process data belonging to the current cell
			if(column == IntColumnProcessActive) SebInstance.Settings.permittedProcessData[SebSettings.KeyActive] = (Boolean)value;
			if(column == IntColumnProcessOS) SebInstance.Settings.permittedProcessData[SebSettings.KeyOS] = (Int32)value;
			if(column == IntColumnProcessExecutable) SebInstance.Settings.permittedProcessData[SebSettings.KeyExecutable] = (String)value;
			if(column == IntColumnProcessTitle) SebInstance.Settings.permittedProcessData[SebSettings.KeyTitle] = (String)value;

			// Beware double events:
			// when a cell is being edited by the user, update its corresponding widget in "Selected Process" group,
			// but prevent the following "widget changed" event from firing the "cell changed" event once more!
			if(column == IntColumnProcessActive) ignoreWidgetEventPermittedProcessesActive = true;
			if(column == IntColumnProcessOS) ignoreWidgetEventPermittedProcessesOS = true;
			if(column == IntColumnProcessExecutable) ignoreWidgetEventPermittedProcessesExecutable = true;
			if(column == IntColumnProcessTitle) ignoreWidgetEventPermittedProcessesTitle = true;

			// In "Selected Process" group: update the widget belonging to the current cell
			// (this will fire the corresponding "widget changed" event).
			if(column == IntColumnProcessActive) checkBoxPermittedProcessActive.Checked = (Boolean)value;
			if(column == IntColumnProcessOS) listBoxPermittedProcessOS.SelectedIndex = (Int32)value;
			if(column == IntColumnProcessExecutable) textBoxPermittedProcessExecutable.Text = (String)value;
			if(column == IntColumnProcessTitle) textBoxPermittedProcessTitle.Text = (String)value;

			// Reset the ignore widget event flags
			if(column == IntColumnProcessActive) ignoreWidgetEventPermittedProcessesActive = false;
			if(column == IntColumnProcessOS) ignoreWidgetEventPermittedProcessesOS = false;
			if(column == IntColumnProcessExecutable) ignoreWidgetEventPermittedProcessesExecutable = false;
			if(column == IntColumnProcessTitle) ignoreWidgetEventPermittedProcessesTitle = false;
		}


		private void buttonAddPermittedProcess_Click(object sender, EventArgs e)
		{
			// Get the process list
			SebInstance.Settings.permittedProcessList = (ListObj)SebInstance.Settings.settingsCurrent[SebSettings.KeyPermittedProcesses];

			if(SebInstance.Settings.permittedProcessList.Count > 0)
			{
				if(dataGridViewPermittedProcesses.SelectedRows.Count != 1) return;
				//SebSettings.permittedProcessIndex = dataGridViewPermittedProcesses.SelectedRows[0].Index;
				SebInstance.Settings.permittedProcessIndex = SebInstance.Settings.permittedProcessList.Count;
			}
			else
			{
				// If process list was empty before, enable it
				SebInstance.Settings.permittedProcessIndex = 0;
				dataGridViewPermittedProcesses.Enabled = true;
				groupBoxPermittedProcess.Enabled = true;
			}

			// Create new process dataset containing default values
			DictObj processData = new DictObj();

			processData[SebSettings.KeyActive] = true;
			processData[SebSettings.KeyAutostart] = false;
			processData[SebSettings.KeyIconInTaskbar] = true;
			processData[SebSettings.KeyRunInBackground] = false;
			processData[SebSettings.KeyAllowUser] = false;
			processData[SebSettings.KeyStrongKill] = false;
			processData[SebSettings.KeyOS] = IntWin;
			processData[SebSettings.KeyTitle] = "";
			processData[SebSettings.KeyDescription] = "";
			processData[SebSettings.KeyExecutable] = "";
			processData[SebSettings.KeyWindowHandlingProcess] = "";
			processData[SebSettings.KeyPath] = "";
			processData[SebSettings.KeyIdentifier] = "";
			processData[SebSettings.KeyArguments] = new ListObj();

			// Insert new process into process list at position index
			SebInstance.Settings.permittedProcessList.Insert(SebInstance.Settings.permittedProcessIndex, processData);
			dataGridViewPermittedProcesses.Rows.Insert(SebInstance.Settings.permittedProcessIndex, true, StringOS[IntWin], "", "");
			dataGridViewPermittedProcesses.Rows[SebInstance.Settings.permittedProcessIndex].Selected = true;
		}


		private void buttonRemovePermittedProcess_Click(object sender, EventArgs e)
		{
			if(dataGridViewPermittedProcesses.SelectedRows.Count != 1) return;

			// Clear the widgets in the "Selected Process" group
			ClearPermittedSelectedProcessGroup();

			// Delete process from process list at position index
			SebInstance.Settings.permittedProcessIndex = dataGridViewPermittedProcesses.SelectedRows[0].Index;
			SebInstance.Settings.permittedProcessList = (ListObj)SebInstance.Settings.settingsCurrent[SebSettings.KeyPermittedProcesses];
			SebInstance.Settings.permittedProcessList.RemoveAt(SebInstance.Settings.permittedProcessIndex);
			dataGridViewPermittedProcesses.Rows.RemoveAt(SebInstance.Settings.permittedProcessIndex);

			if(SebInstance.Settings.permittedProcessIndex == SebInstance.Settings.permittedProcessList.Count)
				SebInstance.Settings.permittedProcessIndex--;

			if(SebInstance.Settings.permittedProcessList.Count > 0)
			{
				dataGridViewPermittedProcesses.Rows[SebInstance.Settings.permittedProcessIndex].Selected = true;
			}
			else
			{
				// If process list is now empty, disable it
				SebInstance.Settings.permittedProcessIndex = -1;
				dataGridViewPermittedProcesses.Enabled = false;
				groupBoxPermittedProcess.Enabled = false;
			}
		}


		private void buttonChoosePermittedApplication_Click(object sender, EventArgs e)
		{
			var permittedApplicationInformation = ChooseApplicationDialog();
			if(permittedApplicationInformation != null)
			{
				buttonAddPermittedProcess_Click(this, EventArgs.Empty);
				textBoxPermittedProcessExecutable.Text = permittedApplicationInformation.Executable;
				textBoxPermittedProcessTitle.Text = permittedApplicationInformation.Title;
				textBoxPermittedProcessPath.Text = permittedApplicationInformation.Path;
			}
		}

		private void ButtonChooseExecutable_Click(object sender, EventArgs e)
		{
			var permittedApplicationInformation = ChooseApplicationDialog();
			if(permittedApplicationInformation != null)
			{
				textBoxPermittedProcessExecutable.Text = permittedApplicationInformation.Executable;
				textBoxPermittedProcessTitle.Text = permittedApplicationInformation.Title;
				textBoxPermittedProcessPath.Text = permittedApplicationInformation.Path;
			}
		}

		private PermittedApplicationInformation ChooseApplicationDialog()
		{
			var permittedApplicationInformation = new PermittedApplicationInformation();

			var fileDialog = new OpenFileDialog
			{
				InitialDirectory = Path.GetPathRoot(Environment.SystemDirectory),
				Multiselect = false
			};
			var res = fileDialog.ShowDialog();

			if(res == DialogResult.OK)
			{
				var filename = fileDialog.FileName.ToLower();
				permittedApplicationInformation.Title = Path.GetFileNameWithoutExtension(fileDialog.FileName);
				permittedApplicationInformation.Executable = Path.GetFileName(filename);

				var filePath = Path.GetDirectoryName(fileDialog.FileName);
				if(filePath == null)
				{
					return null;
				}
				filePath = filePath.ToLower();

				//Check SebWindo2wsClientForm.GetApplicationPath() for how SEB searches the locations
				//Check if Path to the executable is in Registry - SEB gets the path from there if it exists
				using(Microsoft.Win32.RegistryKey key = Microsoft.Win32.RegistryKey.OpenRemoteBaseKey(Microsoft.Win32.RegistryHive.LocalMachine, ""))
				{
					string subKeyName = @"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\" + Path.GetFileName(fileDialog.FileName);
					using(Microsoft.Win32.RegistryKey subkey = key.OpenSubKey(subKeyName))
					{
						if(subkey != null)
						{
							object path = subkey.GetValue("Path");
							if(path != null)
							{
								filePath = filePath.Replace(path.ToString().ToLower(), "");
								filePath = filePath.Replace(path.ToString().TrimEnd('\\').ToLower(), "");
							}
						}
					}
				}

				//Replace all the seach locations - SEB looks in all these directories
				filePath = filePath
					.Replace(SEBClientInfo.ProgramFilesX86Directory.ToLower() + "\\", "")
					.Replace(SEBClientInfo.ProgramFilesX86Directory.ToLower(), "")
					.Replace(Environment.SystemDirectory.ToLower() + "\\", "")
					.Replace(Environment.SystemDirectory.ToLower(), "");

				permittedApplicationInformation.Path = filePath;
				return permittedApplicationInformation;
				//TODO (pwyss 2015/03/13): Keep a list with tools that need special configurations and fill them accordingly (WindowHandlingProcess for example)
			}
			return null;
		}

		private void buttonChoosePermittedProcess_Click(object sender, EventArgs e)
		{

		}


		private void checkBoxPermittedProcessActive_CheckedChanged(object sender, EventArgs e)
		{
			// Prevent double events from switching to false process index
			if(ignoreWidgetEventPermittedProcessesActive == true) return;
			if(SebInstance.Settings.permittedProcessIndex < 0) return;
			SebInstance.Settings.permittedProcessList = (ListObj)SebInstance.Settings.settingsCurrent[SebSettings.KeyPermittedProcesses];
			SebInstance.Settings.permittedProcessData = (DictObj)SebInstance.Settings.permittedProcessList[SebInstance.Settings.permittedProcessIndex];
			SebInstance.Settings.permittedProcessData[SebSettings.KeyActive] = checkBoxPermittedProcessActive.Checked;
			Boolean active = checkBoxPermittedProcessActive.Checked;
			ignoreCellEventPermittedProcessesActive = true;
			dataGridViewPermittedProcesses.Rows[SebInstance.Settings.permittedProcessIndex].Cells[IntColumnProcessActive].Value = active.ToString();
			ignoreCellEventPermittedProcessesActive = false;
		}


		private void checkBoxPermittedProcessAutostart_CheckedChanged(object sender, EventArgs e)
		{
			if(SebInstance.Settings.permittedProcessIndex < 0) return;
			SebInstance.Settings.permittedProcessList = (ListObj)SebInstance.Settings.settingsCurrent[SebSettings.KeyPermittedProcesses];
			SebInstance.Settings.permittedProcessData = (DictObj)SebInstance.Settings.permittedProcessList[SebInstance.Settings.permittedProcessIndex];
			SebInstance.Settings.permittedProcessData[SebSettings.KeyAutostart] = checkBoxPermittedProcessAutostart.Checked;
			checkBoxPermittedProcessIconInTaskbar.Enabled = !checkBoxPermittedProcessAutohide.Checked | checkBoxPermittedProcessAutostart.Checked;
		}

		private void checkBoxPermittedProcessIconInTaskbar_CheckedChanged(object sender, EventArgs e)
		{
			if(SebInstance.Settings.permittedProcessIndex < 0) return;
			SebInstance.Settings.permittedProcessList = (ListObj)SebInstance.Settings.settingsCurrent[SebSettings.KeyPermittedProcesses];
			SebInstance.Settings.permittedProcessData = (DictObj)SebInstance.Settings.permittedProcessList[SebInstance.Settings.permittedProcessIndex];
			SebInstance.Settings.permittedProcessData[SebSettings.KeyIconInTaskbar] = checkBoxPermittedProcessIconInTaskbar.Checked;
		}

		private void checkBoxPermittedProcessAutohide_CheckedChanged(object sender, EventArgs e)
		{
			if(SebInstance.Settings.permittedProcessIndex < 0) return;
			SebInstance.Settings.permittedProcessList = (ListObj)SebInstance.Settings.settingsCurrent[SebSettings.KeyPermittedProcesses];
			SebInstance.Settings.permittedProcessData = (DictObj)SebInstance.Settings.permittedProcessList[SebInstance.Settings.permittedProcessIndex];
			SebInstance.Settings.permittedProcessData[SebSettings.KeyRunInBackground] = checkBoxPermittedProcessAutohide.Checked;
			checkBoxPermittedProcessIconInTaskbar.Enabled = !checkBoxPermittedProcessAutohide.Checked | checkBoxPermittedProcessAutostart.Checked;
		}

		private void checkBoxPermittedProcessAllowUser_CheckedChanged(object sender, EventArgs e)
		{
			if(SebInstance.Settings.permittedProcessIndex < 0) return;
			SebInstance.Settings.permittedProcessList = (ListObj)SebInstance.Settings.settingsCurrent[SebSettings.KeyPermittedProcesses];
			SebInstance.Settings.permittedProcessData = (DictObj)SebInstance.Settings.permittedProcessList[SebInstance.Settings.permittedProcessIndex];
			SebInstance.Settings.permittedProcessData[SebSettings.KeyAllowUser] = checkBoxPermittedProcessAllowUser.Checked;
		}

		private void checkBoxPermittedProcessStrongKill_CheckedChanged(object sender, EventArgs e)
		{
			if(SebInstance.Settings.permittedProcessIndex < 0) return;
			SebInstance.Settings.permittedProcessList = (ListObj)SebInstance.Settings.settingsCurrent[SebSettings.KeyPermittedProcesses];
			SebInstance.Settings.permittedProcessData = (DictObj)SebInstance.Settings.permittedProcessList[SebInstance.Settings.permittedProcessIndex];
			SebInstance.Settings.permittedProcessData[SebSettings.KeyStrongKill] = checkBoxPermittedProcessStrongKill.Checked;
		}


		private void listBoxPermittedProcessOS_SelectedIndexChanged(object sender, EventArgs e)
		{
			// Prevent double events from switching to false process index
			if(ignoreWidgetEventPermittedProcessesOS == true) return;
			if(SebInstance.Settings.permittedProcessIndex < 0) return;
			SebInstance.Settings.permittedProcessList = (ListObj)SebInstance.Settings.settingsCurrent[SebSettings.KeyPermittedProcesses];
			SebInstance.Settings.permittedProcessData = (DictObj)SebInstance.Settings.permittedProcessList[SebInstance.Settings.permittedProcessIndex];
			SebInstance.Settings.permittedProcessData[SebSettings.KeyOS] = listBoxPermittedProcessOS.SelectedIndex;
			Int32 os = listBoxPermittedProcessOS.SelectedIndex;
			ignoreCellEventPermittedProcessesOS = true;
			dataGridViewPermittedProcesses.Rows[SebInstance.Settings.permittedProcessIndex].Cells[IntColumnProcessOS].Value = StringOS[os];
			ignoreCellEventPermittedProcessesOS = false;
		}


		private void textBoxPermittedProcessTitle_TextChanged(object sender, EventArgs e)
		{
			// Prevent double events from switching to false process index
			if(ignoreWidgetEventPermittedProcessesTitle == true) return;
			if(SebInstance.Settings.permittedProcessIndex < 0) return;
			SebInstance.Settings.permittedProcessList = (ListObj)SebInstance.Settings.settingsCurrent[SebSettings.KeyPermittedProcesses];
			SebInstance.Settings.permittedProcessData = (DictObj)SebInstance.Settings.permittedProcessList[SebInstance.Settings.permittedProcessIndex];
			SebInstance.Settings.permittedProcessData[SebSettings.KeyTitle] = textBoxPermittedProcessTitle.Text;
			String title = textBoxPermittedProcessTitle.Text;
			ignoreCellEventPermittedProcessesTitle = true;
			dataGridViewPermittedProcesses.Rows[SebInstance.Settings.permittedProcessIndex].Cells[IntColumnProcessTitle].Value = title;
			ignoreCellEventPermittedProcessesTitle = false;
		}


		private void textBoxPermittedProcessDescription_TextChanged(object sender, EventArgs e)
		{
			if(SebInstance.Settings.permittedProcessIndex < 0) return;
			SebInstance.Settings.permittedProcessList = (ListObj)SebInstance.Settings.settingsCurrent[SebSettings.KeyPermittedProcesses];
			SebInstance.Settings.permittedProcessData = (DictObj)SebInstance.Settings.permittedProcessList[SebInstance.Settings.permittedProcessIndex];
			SebInstance.Settings.permittedProcessData[SebSettings.KeyDescription] = textBoxPermittedProcessDescription.Text;
		}


		private void textBoxPermittedProcessExecutable_TextChanged(object sender, EventArgs e)
		{
			// Prevent double events from switching to false process index
			if(ignoreWidgetEventPermittedProcessesExecutable == true) return;
			if(SebInstance.Settings.permittedProcessIndex < 0) return;
			SebInstance.Settings.permittedProcessList = (ListObj)SebInstance.Settings.settingsCurrent[SebSettings.KeyPermittedProcesses];
			SebInstance.Settings.permittedProcessData = (DictObj)SebInstance.Settings.permittedProcessList[SebInstance.Settings.permittedProcessIndex];
			SebInstance.Settings.permittedProcessData[SebSettings.KeyExecutable] = textBoxPermittedProcessExecutable.Text;
			String executable = textBoxPermittedProcessExecutable.Text;
			ignoreCellEventPermittedProcessesExecutable = true;
			dataGridViewPermittedProcesses.Rows[SebInstance.Settings.permittedProcessIndex].Cells[IntColumnProcessExecutable].Value = executable;
			ignoreCellEventPermittedProcessesExecutable = false;
		}


		private void textBoxPermittedProcessPath_TextChanged(object sender, EventArgs e)
		{
			if(SebInstance.Settings.permittedProcessIndex < 0) return;
			SebInstance.Settings.permittedProcessList = (ListObj)SebInstance.Settings.settingsCurrent[SebSettings.KeyPermittedProcesses];
			SebInstance.Settings.permittedProcessData = (DictObj)SebInstance.Settings.permittedProcessList[SebInstance.Settings.permittedProcessIndex];
			SebInstance.Settings.permittedProcessData[SebSettings.KeyPath] = textBoxPermittedProcessPath.Text;
		}

		private void textBoxPermittedProcessIdentifier_TextChanged(object sender, EventArgs e)
		{
			if(SebInstance.Settings.permittedProcessIndex < 0) return;
			SebInstance.Settings.permittedProcessList = (ListObj)SebInstance.Settings.settingsCurrent[SebSettings.KeyPermittedProcesses];
			SebInstance.Settings.permittedProcessData = (DictObj)SebInstance.Settings.permittedProcessList[SebInstance.Settings.permittedProcessIndex];
			SebInstance.Settings.permittedProcessData[SebSettings.KeyIdentifier] = textBoxPermittedProcessIdentifier.Text;
		}

		private void textBoxPermittedProcessExecutables_TextChanged(object sender, EventArgs e)
		{
			if(SebInstance.Settings.permittedProcessIndex < 0) return;
			SebInstance.Settings.permittedProcessList = (ListObj)SebInstance.Settings.settingsCurrent[SebSettings.KeyPermittedProcesses];
			SebInstance.Settings.permittedProcessData = (DictObj)SebInstance.Settings.permittedProcessList[SebInstance.Settings.permittedProcessIndex];
			SebInstance.Settings.permittedProcessData[SebSettings.KeyWindowHandlingProcess] = textBoxPermittedProcessExecutables.Text;
		}

		private void buttonPermittedProcessCodeSignature_Click(object sender, EventArgs e)
		{

		}


		private void dataGridViewPermittedProcessArguments_SelectionChanged(object sender, EventArgs e)
		{
			// CAUTION:
			// If a row was previously selected and the user clicks onto another row,
			// the SelectionChanged() event is fired TWICE!!!
			// The first time, it is only for UNselecting the old row,
			// so the SelectedRows.Count is ZERO, so ignore this event handler!
			// The second time, SelectedRows.Count is ONE.

			if(dataGridViewPermittedProcessArguments.SelectedRows.Count != 1) return;

			// Get the argument data of the selected argument
			SebInstance.Settings.permittedArgumentIndex = dataGridViewPermittedProcessArguments.SelectedRows[0].Index;
			SebInstance.Settings.permittedProcessList = (ListObj)SebInstance.Settings.settingsCurrent[SebSettings.KeyPermittedProcesses];
			SebInstance.Settings.permittedProcessData = (DictObj)SebInstance.Settings.permittedProcessList[SebInstance.Settings.permittedProcessIndex];
			SebInstance.Settings.permittedArgumentList = (ListObj)SebInstance.Settings.permittedProcessData[SebSettings.KeyArguments];
			SebInstance.Settings.permittedArgumentData = (DictObj)SebInstance.Settings.permittedArgumentList[SebInstance.Settings.permittedArgumentIndex];
		}


		private void dataGridViewPermittedProcessArguments_CurrentCellDirtyStateChanged(object sender, EventArgs e)
		{
			// When a CheckBox/ListBox/TextBox entry of a DataGridView table cell is edited,
			// immediately call the CellValueChanged() event,
			// which will update the SelectedProcess data and widgets.
			if(dataGridViewPermittedProcessArguments.IsCurrentCellDirty)
				dataGridViewPermittedProcessArguments.CommitEdit(DataGridViewDataErrorContexts.Commit);
		}


		private void dataGridViewPermittedProcessArguments_CellValueChanged(object sender, DataGridViewCellEventArgs e)
		{
			// Get the current cell where the user has changed a value
			int row = dataGridViewPermittedProcessArguments.CurrentCellAddress.Y;
			int column = dataGridViewPermittedProcessArguments.CurrentCellAddress.X;

			// At the beginning, row = -1 and column = -1, so skip this event
			if(row < 0) return;
			if(column < 0) return;

			// Get the changed value of the current cell
			object value = dataGridViewPermittedProcessArguments.CurrentCell.EditedFormattedValue;

			// Get the argument data of the argument belonging to the cell (row)
			SebInstance.Settings.permittedArgumentIndex = row;
			SebInstance.Settings.permittedProcessList = (ListObj)SebInstance.Settings.settingsCurrent[SebSettings.KeyPermittedProcesses];
			SebInstance.Settings.permittedProcessData = (DictObj)SebInstance.Settings.permittedProcessList[SebInstance.Settings.permittedProcessIndex];
			SebInstance.Settings.permittedArgumentList = (ListObj)SebInstance.Settings.permittedProcessData[SebSettings.KeyArguments];
			SebInstance.Settings.permittedArgumentData = (DictObj)SebInstance.Settings.permittedArgumentList[SebInstance.Settings.permittedArgumentIndex];

			// Update the argument data belonging to the current cell
			if(column == IntColumnProcessActive) SebInstance.Settings.permittedArgumentData[SebSettings.KeyActive] = (Boolean)value;
			if(column == IntColumnProcessArgument) SebInstance.Settings.permittedArgumentData[SebSettings.KeyArgument] = (String)value;
		}


		private void buttonPermittedProcessAddArgument_Click(object sender, EventArgs e)
		{
			// Get the permitted argument list
			SebInstance.Settings.permittedProcessList = (ListObj)SebInstance.Settings.settingsCurrent[SebSettings.KeyPermittedProcesses];
			SebInstance.Settings.permittedProcessData = (DictObj)SebInstance.Settings.permittedProcessList[SebInstance.Settings.permittedProcessIndex];
			SebInstance.Settings.permittedArgumentList = (ListObj)SebInstance.Settings.permittedProcessData[SebSettings.KeyArguments];

			if(SebInstance.Settings.permittedArgumentList.Count > 0)
			{
				if(dataGridViewPermittedProcessArguments.SelectedRows.Count != 1) return;
				//SebSettings.permittedArgumentIndex = dataGridViewPermittedProcessArguments.SelectedRows[0].Index;
				SebInstance.Settings.permittedArgumentIndex = SebInstance.Settings.permittedArgumentList.Count;
			}
			else
			{
				// If argument list was empty before, enable it
				SebInstance.Settings.permittedArgumentIndex = 0;
				dataGridViewPermittedProcessArguments.Enabled = true;
			}

			// Create new argument dataset containing default values
			DictObj argumentData = new DictObj();

			argumentData[SebSettings.KeyActive] = true;
			argumentData[SebSettings.KeyArgument] = "";

			// Insert new argument into argument list at position SebSettings.permittedArgumentIndex
			SebInstance.Settings.permittedArgumentList.Insert(SebInstance.Settings.permittedArgumentIndex, argumentData);
			dataGridViewPermittedProcessArguments.Rows.Insert(SebInstance.Settings.permittedArgumentIndex, true, "");
			dataGridViewPermittedProcessArguments.Rows[SebInstance.Settings.permittedArgumentIndex].Selected = true;
		}


		private void buttonPermittedProcessRemoveArgument_Click(object sender, EventArgs e)
		{
			if(dataGridViewPermittedProcessArguments.SelectedRows.Count != 1) return;

			// Get the permitted argument list
			SebInstance.Settings.permittedArgumentIndex = dataGridViewPermittedProcessArguments.SelectedRows[0].Index;
			SebInstance.Settings.permittedProcessList = (ListObj)SebInstance.Settings.settingsCurrent[SebSettings.KeyPermittedProcesses];
			SebInstance.Settings.permittedProcessData = (DictObj)SebInstance.Settings.permittedProcessList[SebInstance.Settings.permittedProcessIndex];
			SebInstance.Settings.permittedArgumentList = (ListObj)SebInstance.Settings.permittedProcessData[SebSettings.KeyArguments];

			// Delete argument from argument list at position SebSettings.permittedArgumentIndex
			SebInstance.Settings.permittedArgumentList.RemoveAt(SebInstance.Settings.permittedArgumentIndex);
			dataGridViewPermittedProcessArguments.Rows.RemoveAt(SebInstance.Settings.permittedArgumentIndex);

			if(SebInstance.Settings.permittedArgumentIndex == SebInstance.Settings.permittedArgumentList.Count)
				SebInstance.Settings.permittedArgumentIndex--;

			if(SebInstance.Settings.permittedArgumentList.Count > 0)
			{
				dataGridViewPermittedProcessArguments.Rows[SebInstance.Settings.permittedArgumentIndex].Selected = true;
			}
			else
			{
				// If argument list is now empty, disable it
				SebInstance.Settings.permittedArgumentIndex = -1;
				//SebSettings.permittedArgumentList.Clear();
				//SebSettings.permittedArgumentData.Clear();
				dataGridViewPermittedProcessArguments.Enabled = false;
			}
		}



		// *******************************************
		// Group "Applications - Prohibited Processes"
		// *******************************************
		private void LoadAndUpdateProhibitedSelectedProcessGroup(int selectedProcessIndex)
		{
			// Get the process data of the selected process
			SebInstance.Settings.prohibitedProcessList = (ListObj)SebInstance.Settings.settingsCurrent[SebSettings.KeyProhibitedProcesses];
			SebInstance.Settings.prohibitedProcessData = (DictObj)SebInstance.Settings.prohibitedProcessList[selectedProcessIndex];

			// Beware double events:
			// Update the widgets in "Selected Process" group,
			// but prevent the following "widget changed" event from firing the "cell changed" event once more!
			ignoreWidgetEventProhibitedProcessesActive = true;
			ignoreWidgetEventProhibitedProcessesOS = true;
			ignoreWidgetEventProhibitedProcessesExecutable = true;
			ignoreWidgetEventProhibitedProcessesDescription = true;

			// Update the widgets in the "Selected Process" group
			checkBoxProhibitedProcessActive.Checked = (Boolean)SebInstance.Settings.prohibitedProcessData[SebSettings.KeyActive];
			checkBoxProhibitedProcessCurrentUser.Checked = (Boolean)SebInstance.Settings.prohibitedProcessData[SebSettings.KeyCurrentUser];
			checkBoxProhibitedProcessStrongKill.Checked = (Boolean)SebInstance.Settings.prohibitedProcessData[SebSettings.KeyStrongKill];
			listBoxProhibitedProcessOS.SelectedIndex = (Int32)SebInstance.Settings.prohibitedProcessData[SebSettings.KeyOS];
			textBoxProhibitedProcessExecutable.Text = (String)SebInstance.Settings.prohibitedProcessData[SebSettings.KeyExecutable];
			textBoxProhibitedProcessDescription.Text = (String)SebInstance.Settings.prohibitedProcessData[SebSettings.KeyDescription];
			textBoxProhibitedProcessIdentifier.Text = (String)SebInstance.Settings.prohibitedProcessData[SebSettings.KeyIdentifier];
			textBoxProhibitedProcessUser.Text = (String)SebInstance.Settings.prohibitedProcessData[SebSettings.KeyUser];

			// Reset the ignore widget event flags
			ignoreWidgetEventProhibitedProcessesActive = false;
			ignoreWidgetEventProhibitedProcessesOS = false;
			ignoreWidgetEventProhibitedProcessesExecutable = false;
			ignoreWidgetEventProhibitedProcessesDescription = false;
		}


		private void ClearProhibitedSelectedProcessGroup()
		{
			// Beware double events:
			// Update the widgets in "Selected Process" group,
			// but prevent the following "widget changed" event from firing the "cell changed" event once more!
			ignoreWidgetEventProhibitedProcessesActive = true;
			ignoreWidgetEventProhibitedProcessesOS = true;
			ignoreWidgetEventProhibitedProcessesExecutable = true;
			ignoreWidgetEventProhibitedProcessesDescription = true;

			// Clear the widgets in the "Selected Process" group
			checkBoxProhibitedProcessActive.Checked = true;
			checkBoxProhibitedProcessCurrentUser.Checked = true;
			checkBoxProhibitedProcessStrongKill.Checked = false;
			listBoxProhibitedProcessOS.SelectedIndex = IntWin;
			textBoxProhibitedProcessExecutable.Text = "";
			textBoxProhibitedProcessDescription.Text = "";
			textBoxProhibitedProcessIdentifier.Text = "";
			textBoxProhibitedProcessUser.Text = "";

			// Reset the ignore widget event flags
			ignoreWidgetEventProhibitedProcessesActive = false;
			ignoreWidgetEventProhibitedProcessesOS = false;
			ignoreWidgetEventProhibitedProcessesExecutable = false;
			ignoreWidgetEventProhibitedProcessesDescription = false;
		}


		private void dataGridViewProhibitedProcesses_SelectionChanged(object sender, EventArgs e)
		{
			// CAUTION:
			// If a row was previously selected and the user clicks onto another row,
			// the SelectionChanged() event is fired TWICE!!!
			// The first time, it is only for UNselecting the old row,
			// so the SelectedRows.Count is ZERO, so ignore this event handler!
			// The second time, SelectedRows.Count is ONE.
			// Now you can set the widgets in the "Selected Process" groupBox.

			if(dataGridViewProhibitedProcesses.SelectedRows.Count != 1) return;
			SebInstance.Settings.prohibitedProcessIndex = dataGridViewProhibitedProcesses.SelectedRows[0].Index;

			// The process list should contain at least one element here:
			// SebSettings.prohibitedProcessList.Count >  0
			// SebSettings.prohibitedProcessIndex      >= 0
			LoadAndUpdateProhibitedSelectedProcessGroup(SebInstance.Settings.prohibitedProcessIndex);
		}


		private void dataGridViewProhibitedProcesses_CurrentCellDirtyStateChanged(object sender, EventArgs e)
		{
			// When a CheckBox/ListBox/TextBox entry of a DataGridView table cell is edited,
			// immediately call the CellValueChanged() event,
			// which will update the SelectedProcess data and widgets.
			if(dataGridViewProhibitedProcesses.IsCurrentCellDirty)
				dataGridViewProhibitedProcesses.CommitEdit(DataGridViewDataErrorContexts.Commit);
		}


		private void dataGridViewProhibitedProcesses_CellValueChanged(object sender, DataGridViewCellEventArgs e)
		{

			// Prevent double events from switching to false process index
			if(ignoreCellEventProhibitedProcessesActive == true) return;
			if(ignoreCellEventProhibitedProcessesOS == true) return;
			if(ignoreCellEventProhibitedProcessesExecutable == true) return;
			if(ignoreCellEventProhibitedProcessesDescription == true) return;

			// Get the current cell where the user has changed a value
			int row = dataGridViewProhibitedProcesses.CurrentCellAddress.Y;
			int column = dataGridViewProhibitedProcesses.CurrentCellAddress.X;

			// At the beginning, row = -1 and column = -1, so skip this event
			if(row < 0) return;
			if(column < 0) return;

			// Get the changed value of the current cell
			object value = dataGridViewProhibitedProcesses.CurrentCell.EditedFormattedValue;

			// Convert the selected "OS" ListBox entry from String to Integer
			if(column == IntColumnProcessOS)
			{
				if((String)value == StringOSX) value = IntOSX;
				else if((String)value == StringWin) value = IntWin;
			}

			// Get the process data of the process belonging to the current row
			SebInstance.Settings.prohibitedProcessIndex = row;
			SebInstance.Settings.prohibitedProcessList = (ListObj)SebInstance.Settings.settingsCurrent[SebSettings.KeyProhibitedProcesses];
			SebInstance.Settings.prohibitedProcessData = (DictObj)SebInstance.Settings.prohibitedProcessList[SebInstance.Settings.prohibitedProcessIndex];

			// Update the process data belonging to the current cell
			if(column == IntColumnProcessActive) SebInstance.Settings.prohibitedProcessData[SebSettings.KeyActive] = (Boolean)value;
			if(column == IntColumnProcessOS) SebInstance.Settings.prohibitedProcessData[SebSettings.KeyOS] = (Int32)value;
			if(column == IntColumnProcessExecutable) SebInstance.Settings.prohibitedProcessData[SebSettings.KeyExecutable] = (String)value;
			if(column == IntColumnProcessDescription) SebInstance.Settings.prohibitedProcessData[SebSettings.KeyDescription] = (String)value;

			// Beware double events:
			// when a cell has been edited, update its corresponding widget in "Selected Process" group,
			// but prevent the following "widget changed" event from firing the "cell changed" event once more!
			if(column == IntColumnProcessActive) ignoreWidgetEventProhibitedProcessesActive = true;
			if(column == IntColumnProcessOS) ignoreWidgetEventProhibitedProcessesOS = true;
			if(column == IntColumnProcessExecutable) ignoreWidgetEventProhibitedProcessesExecutable = true;
			if(column == IntColumnProcessDescription) ignoreWidgetEventProhibitedProcessesDescription = true;

			// In "Selected Process" group: update the widget belonging to the current cell
			// (this will fire the corresponding "widget changed" event).
			if(column == IntColumnProcessActive) checkBoxProhibitedProcessActive.Checked = (Boolean)value;
			if(column == IntColumnProcessOS) listBoxProhibitedProcessOS.SelectedIndex = (Int32)value;
			if(column == IntColumnProcessExecutable) textBoxProhibitedProcessExecutable.Text = (String)value;
			if(column == IntColumnProcessDescription) textBoxProhibitedProcessDescription.Text = (String)value;

			// Reset the ignore widget event flags
			if(column == IntColumnProcessActive) ignoreWidgetEventProhibitedProcessesActive = false;
			if(column == IntColumnProcessOS) ignoreWidgetEventProhibitedProcessesOS = false;
			if(column == IntColumnProcessExecutable) ignoreWidgetEventProhibitedProcessesExecutable = false;
			if(column == IntColumnProcessDescription) ignoreWidgetEventProhibitedProcessesDescription = false;
		}


		private void buttonAddProhibitedProcess_Click(object sender, EventArgs e)
		{
			// Get the process list
			SebInstance.Settings.prohibitedProcessList = (ListObj)SebInstance.Settings.settingsCurrent[SebSettings.KeyProhibitedProcesses];

			if(SebInstance.Settings.prohibitedProcessList.Count > 0)
			{
				if(dataGridViewProhibitedProcesses.SelectedRows.Count != 1) return;
				//SebSettings.prohibitedProcessIndex = dataGridViewProhibitedProcesses.SelectedRows[0].Index;
				SebInstance.Settings.prohibitedProcessIndex = SebInstance.Settings.prohibitedProcessList.Count;
			}
			else
			{
				// If process list was empty before, enable it
				SebInstance.Settings.prohibitedProcessIndex = 0;
				dataGridViewProhibitedProcesses.Enabled = true;
				groupBoxProhibitedProcess.Enabled = true;
			}

			// Create new process dataset containing default values
			DictObj processData = new DictObj();

			processData[SebSettings.KeyActive] = true;
			processData[SebSettings.KeyCurrentUser] = true;
			processData[SebSettings.KeyStrongKill] = false;
			processData[SebSettings.KeyOS] = IntWin;
			processData[SebSettings.KeyExecutable] = "";
			processData[SebSettings.KeyDescription] = "";
			processData[SebSettings.KeyIdentifier] = "";
			processData[SebSettings.KeyUser] = "";

			// Insert new process into process list at position index
			SebInstance.Settings.prohibitedProcessList.Insert(SebInstance.Settings.prohibitedProcessIndex, processData);
			dataGridViewProhibitedProcesses.Rows.Insert(SebInstance.Settings.prohibitedProcessIndex, true, StringOS[IntWin], "", "");
			dataGridViewProhibitedProcesses.Rows[SebInstance.Settings.prohibitedProcessIndex].Selected = true;
		}


		private void buttonRemoveProhibitedProcess_Click(object sender, EventArgs e)
		{
			if(dataGridViewProhibitedProcesses.SelectedRows.Count != 1) return;

			// Clear the widgets in the "Selected Process" group
			ClearProhibitedSelectedProcessGroup();

			// Delete process from process list at position index
			SebInstance.Settings.prohibitedProcessIndex = dataGridViewProhibitedProcesses.SelectedRows[0].Index;
			SebInstance.Settings.prohibitedProcessList = (ListObj)SebInstance.Settings.settingsCurrent[SebSettings.KeyProhibitedProcesses];
			SebInstance.Settings.prohibitedProcessList.RemoveAt(SebInstance.Settings.prohibitedProcessIndex);
			dataGridViewProhibitedProcesses.Rows.RemoveAt(SebInstance.Settings.prohibitedProcessIndex);

			if(SebInstance.Settings.prohibitedProcessIndex == SebInstance.Settings.prohibitedProcessList.Count)
				SebInstance.Settings.prohibitedProcessIndex--;

			if(SebInstance.Settings.prohibitedProcessList.Count > 0)
			{
				dataGridViewProhibitedProcesses.Rows[SebInstance.Settings.prohibitedProcessIndex].Selected = true;
			}
			else
			{
				// If process list is now empty, disable it
				SebInstance.Settings.prohibitedProcessIndex = -1;
				dataGridViewProhibitedProcesses.Enabled = false;
				groupBoxProhibitedProcess.Enabled = false;
			}
		}


		private void buttonChooseProhibitedExecutable_Click(object sender, EventArgs e)
		{

		}

		private void buttonChooseProhibitedProcess_Click(object sender, EventArgs e)
		{

		}


		private void checkBoxProhibitedProcessActive_CheckedChanged(object sender, EventArgs e)
		{
			// Prevent double events from switching to false process index
			if(ignoreWidgetEventProhibitedProcessesActive == true) return;
			if(SebInstance.Settings.prohibitedProcessIndex < 0) return;
			SebInstance.Settings.prohibitedProcessList = (ListObj)SebInstance.Settings.settingsCurrent[SebSettings.KeyProhibitedProcesses];
			SebInstance.Settings.prohibitedProcessData = (DictObj)SebInstance.Settings.prohibitedProcessList[SebInstance.Settings.prohibitedProcessIndex];
			SebInstance.Settings.prohibitedProcessData[SebSettings.KeyActive] = checkBoxProhibitedProcessActive.Checked;
			Boolean active = checkBoxProhibitedProcessActive.Checked;
			ignoreCellEventProhibitedProcessesActive = true;
			dataGridViewProhibitedProcesses.Rows[SebInstance.Settings.prohibitedProcessIndex].Cells[IntColumnProcessActive].Value = active.ToString();
			ignoreCellEventProhibitedProcessesActive = false;
		}


		private void checkBoxProhibitedProcessCurrentUser_CheckedChanged(object sender, EventArgs e)
		{
			if(SebInstance.Settings.prohibitedProcessIndex < 0) return;
			SebInstance.Settings.prohibitedProcessList = (ListObj)SebInstance.Settings.settingsCurrent[SebSettings.KeyProhibitedProcesses];
			SebInstance.Settings.prohibitedProcessData = (DictObj)SebInstance.Settings.prohibitedProcessList[SebInstance.Settings.prohibitedProcessIndex];
			SebInstance.Settings.prohibitedProcessData[SebSettings.KeyCurrentUser] = checkBoxProhibitedProcessCurrentUser.Checked;
		}

		private void checkBoxProhibitedProcessStrongKill_CheckedChanged(object sender, EventArgs e)
		{
			if(SebInstance.Settings.prohibitedProcessIndex < 0) return;
			SebInstance.Settings.prohibitedProcessList = (ListObj)SebInstance.Settings.settingsCurrent[SebSettings.KeyProhibitedProcesses];
			SebInstance.Settings.prohibitedProcessData = (DictObj)SebInstance.Settings.prohibitedProcessList[SebInstance.Settings.prohibitedProcessIndex];
			SebInstance.Settings.prohibitedProcessData[SebSettings.KeyStrongKill] = checkBoxProhibitedProcessStrongKill.Checked;
		}


		private void listBoxProhibitedProcessOS_SelectedIndexChanged(object sender, EventArgs e)
		{
			// Prevent double events from switching to false process index
			if(ignoreWidgetEventProhibitedProcessesOS == true) return;
			if(SebInstance.Settings.prohibitedProcessIndex < 0) return;
			SebInstance.Settings.prohibitedProcessList = (ListObj)SebInstance.Settings.settingsCurrent[SebSettings.KeyProhibitedProcesses];
			SebInstance.Settings.prohibitedProcessData = (DictObj)SebInstance.Settings.prohibitedProcessList[SebInstance.Settings.prohibitedProcessIndex];
			SebInstance.Settings.prohibitedProcessData[SebSettings.KeyOS] = listBoxProhibitedProcessOS.SelectedIndex;
			Int32 os = listBoxProhibitedProcessOS.SelectedIndex;
			ignoreCellEventProhibitedProcessesOS = true;
			dataGridViewProhibitedProcesses.Rows[SebInstance.Settings.prohibitedProcessIndex].Cells[IntColumnProcessOS].Value = StringOS[os];
			ignoreCellEventProhibitedProcessesOS = false;
		}


		private void textBoxProhibitedProcessExecutable_TextChanged(object sender, EventArgs e)
		{
			// Prevent double events from switching to false process index
			if(ignoreWidgetEventProhibitedProcessesExecutable == true) return;
			if(SebInstance.Settings.prohibitedProcessIndex < 0) return;
			SebInstance.Settings.prohibitedProcessList = (ListObj)SebInstance.Settings.settingsCurrent[SebSettings.KeyProhibitedProcesses];
			SebInstance.Settings.prohibitedProcessData = (DictObj)SebInstance.Settings.prohibitedProcessList[SebInstance.Settings.prohibitedProcessIndex];
			SebInstance.Settings.prohibitedProcessData[SebSettings.KeyExecutable] = textBoxProhibitedProcessExecutable.Text;
			String executable = textBoxProhibitedProcessExecutable.Text;
			ignoreCellEventProhibitedProcessesExecutable = true;
			dataGridViewProhibitedProcesses.Rows[SebInstance.Settings.prohibitedProcessIndex].Cells[IntColumnProcessExecutable].Value = executable;
			ignoreCellEventProhibitedProcessesExecutable = false;
		}


		private void textBoxProhibitedProcessDescription_TextChanged(object sender, EventArgs e)
		{
			// Prevent double events from switching to false process index
			if(ignoreWidgetEventProhibitedProcessesDescription == true) return;
			if(SebInstance.Settings.prohibitedProcessIndex < 0) return;
			SebInstance.Settings.prohibitedProcessList = (ListObj)SebInstance.Settings.settingsCurrent[SebSettings.KeyProhibitedProcesses];
			SebInstance.Settings.prohibitedProcessData = (DictObj)SebInstance.Settings.prohibitedProcessList[SebInstance.Settings.prohibitedProcessIndex];
			SebInstance.Settings.prohibitedProcessData[SebSettings.KeyDescription] = textBoxProhibitedProcessDescription.Text;
			String description = textBoxProhibitedProcessDescription.Text;
			ignoreCellEventProhibitedProcessesDescription = true;
			dataGridViewProhibitedProcesses.Rows[SebInstance.Settings.prohibitedProcessIndex].Cells[IntColumnProcessDescription].Value = description;
			ignoreCellEventProhibitedProcessesDescription = false;
		}


		private void textBoxProhibitedProcessIdentifier_TextChanged(object sender, EventArgs e)
		{
			if(SebInstance.Settings.prohibitedProcessIndex < 0) return;
			SebInstance.Settings.prohibitedProcessList = (ListObj)SebInstance.Settings.settingsCurrent[SebSettings.KeyProhibitedProcesses];
			SebInstance.Settings.prohibitedProcessData = (DictObj)SebInstance.Settings.prohibitedProcessList[SebInstance.Settings.prohibitedProcessIndex];
			SebInstance.Settings.prohibitedProcessData[SebSettings.KeyIdentifier] = textBoxProhibitedProcessIdentifier.Text;
		}

		private void textBoxProhibitedProcessUser_TextChanged(object sender, EventArgs e)
		{
			if(SebInstance.Settings.prohibitedProcessIndex < 0) return;
			SebInstance.Settings.prohibitedProcessList = (ListObj)SebInstance.Settings.settingsCurrent[SebSettings.KeyProhibitedProcesses];
			SebInstance.Settings.prohibitedProcessData = (DictObj)SebInstance.Settings.prohibitedProcessList[SebInstance.Settings.prohibitedProcessIndex];
			SebInstance.Settings.prohibitedProcessData[SebSettings.KeyUser] = textBoxProhibitedProcessUser.Text;
		}

		private void buttonProhibitedProcessCodeSignature_Click(object sender, EventArgs e)
		{

		}

		// ************************
		// Group "Network - Url Filter"
		// ************************
		private void btnAddWhitelistFilter_Click(object sender, EventArgs e)
		{
			datagridWhitelist.Rows.Add();
		}

		private void btnRemoveWhitelistFilter_Click(object sender, EventArgs e)
		{
			if(datagridWhitelist.CurrentRow != null)
			{
				datagridWhitelist.Rows.Remove(datagridWhitelist.CurrentRow);
				datagridWhitelist_CellValueChanged(null, null);
			}

		}

		private void datagridWhitelist_CellValueChanged(object sender, DataGridViewCellEventArgs e)
		{
			var list = new List<string>();
			foreach(DataGridViewRow r in datagridWhitelist.Rows)
			{
				foreach(DataGridViewCell cell in r.Cells)
				{
					if(cell.Value != null && !String.IsNullOrWhiteSpace(cell.Value.ToString()))
						list.Add(cell.Value.ToString());
				}
			}

			SebInstance.Settings.settingsCurrent[SebSettings.KeyUrlFilterWhitelist] = String.Join(";", list);
		}

		private void btnAddBlacklistFilter_Click(object sender, EventArgs e)
		{
			datagridBlackListFilter.Rows.Add();
		}

		private void btnRemoveBlacklistFilter_Click(object sender, EventArgs e)
		{
			if(datagridBlackListFilter.CurrentRow != null)
			{
				datagridBlackListFilter.Rows.Remove(datagridBlackListFilter.CurrentRow);
				datagridBlacklist_CellValueChanged(null, null);
			}
		}

		private void datagridBlacklist_CellValueChanged(object sender, DataGridViewCellEventArgs e)
		{
			var list = new List<string>();
			foreach(DataGridViewRow r in datagridBlackListFilter.Rows)
			{
				foreach(DataGridViewCell cell in r.Cells)
				{
					if(cell.Value != null && !String.IsNullOrWhiteSpace(cell.Value.ToString()))
						list.Add(cell.Value.ToString());
				}
			}

			SebInstance.Settings.settingsCurrent[SebSettings.KeyUrlFilterBlacklist] = String.Join(";", list);
		}

		private void checkBoxEnableURLFilter_CheckedChanged(object sender, EventArgs e)
		{
			SebInstance.Settings.settingsCurrent[SebSettings.KeyURLFilterEnable] = checkBoxEnableURLFilter.Checked;
			checkBoxEnableURLContentFilter.Enabled = checkBoxEnableURLFilter.Checked;
			checkBoxUrlFilterRulesRegex.Enabled = checkBoxEnableURLFilter.Checked;

		}

		private void checkBoxEnableURLContentFilter_CheckedChanged(object sender, EventArgs e)
		{
			SebInstance.Settings.settingsCurrent[SebSettings.KeyURLFilterEnableContentFilter] = checkBoxEnableURLContentFilter.Checked;
		}

		private void checkBoxUrlFilterRulesRegex_CheckedChanged(object sender, EventArgs e)
		{
			SebInstance.Settings.settingsCurrent[SebSettings.KeyUrlFilterRulesAsRegex] = checkBoxUrlFilterRulesRegex.Checked;
		}

		// ******************************
		// Group "Network - Certificates"
		// ******************************
		private void comboBoxChooseSSLClientCertificate_SelectedIndexChanged(object sender, EventArgs e)
		{
			var cert = (X509Certificate2)certificateSSLReferences[comboBoxChooseSSLClientCertificate.SelectedIndex];

			SebInstance.Settings.embeddedCertificateList = (ListObj)SebInstance.Settings.settingsCurrent[SebSettings.KeyEmbeddedCertificates];

			SebInstance.Settings.embeddedCertificateIndex = SebInstance.Settings.embeddedCertificateList.Count;

			DictObj certData = new DictObj();

			//certData[SebSettings.KeyCertificateData] = cert.RawData;
			certData[SebSettings.KeyCertificateDataWin] = exportToPEM(cert);
			certData[SebSettings.KeyType] = 0;
			certData[SebSettings.KeyName] = comboBoxChooseSSLClientCertificate.SelectedItem;

			SebInstance.Settings.embeddedCertificateList.Insert(SebInstance.Settings.embeddedCertificateIndex, certData);

			dataGridViewEmbeddedCertificates.Rows.Insert(SebInstance.Settings.embeddedCertificateIndex, "SSL Certificate", comboBoxChooseSSLClientCertificate.SelectedItem);
			dataGridViewEmbeddedCertificates.Rows[SebInstance.Settings.embeddedCertificateIndex].Selected = true;

			comboBoxChooseSSLClientCertificate.BeginInvoke((Action)(() =>
			{
				comboBoxChooseSSLClientCertificate.Text = SEBUIStrings.ChooseEmbeddedCert;
			}));

			dataGridViewEmbeddedCertificates.Enabled = true;
		}

		private void comboBoxChooseIdentityToEmbed_SelectedIndexChanged(object sender, EventArgs e)
		{
			var cert = (X509Certificate2)certificateReferences[comboBoxChooseIdentityToEmbed.SelectedIndex];

			SebInstance.Settings.embeddedCertificateList = (ListObj)SebInstance.Settings.settingsCurrent[SebSettings.KeyEmbeddedCertificates];

			SebInstance.Settings.embeddedCertificateIndex = SebInstance.Settings.embeddedCertificateList.Count;

			DictObj identityToEmbedd = new DictObj();

			byte[] certData = new byte[0];

			try
			{
				certData = cert.Export(X509ContentType.Pkcs12, SebConstants.DEFAULT_KEY);
			}

			catch(Exception certExportException)
			{
				Logger.AddError(string.Format("The identity (certificate with private key) {0} could not be exported", comboBoxChooseIdentityToEmbed.SelectedItem), null, certExportException, certExportException.Message);

				SebMessageBox.Show(SEBUIStrings.identityExportError, string.Format(SEBUIStrings.identityExportErrorMessage, comboBoxChooseIdentityToEmbed.SelectedItem), MessageBoxImage.Error, MessageBoxButton.OK);
			}

			if(certData.Length > 0)
			{
				identityToEmbedd[SebSettings.KeyCertificateData] = certData;
				//certData[SebSettings.KeyCertificateDataWin] = exportToPEM(cert);
				identityToEmbedd[SebSettings.KeyType] = 1;
				identityToEmbedd[SebSettings.KeyName] = comboBoxChooseIdentityToEmbed.SelectedItem;

				SebInstance.Settings.embeddedCertificateList.Insert(SebInstance.Settings.embeddedCertificateIndex, identityToEmbedd);

				dataGridViewEmbeddedCertificates.Rows.Insert(SebInstance.Settings.embeddedCertificateIndex, "Identity", comboBoxChooseIdentityToEmbed.SelectedItem);
				dataGridViewEmbeddedCertificates.Rows[SebInstance.Settings.embeddedCertificateIndex].Selected = true;
			}

			comboBoxChooseIdentityToEmbed.BeginInvoke((Action)(() =>
			{
				comboBoxChooseIdentityToEmbed.Text = SEBUIStrings.ChooseEmbeddedCert;
			}));

			dataGridViewEmbeddedCertificates.Enabled = true;
		}

		/// <summary>
		/// Export a certificate to a PEM format string
		/// </summary>
		/// <param name="cert">The certificate to export</param>
		/// <returns>A PEM encoded string</returns>
		private string exportToPEM(X509Certificate cert)
		{
			string certToBase64String = Convert.ToBase64String(cert.Export(X509ContentType.Cert));
			//certToBase64String = certToBase64String.Replace("/", @"\/");
			//certToBase64String = certToBase64String.Substring(0, certToBase64String.Length - 1);

			StringBuilder builder = new StringBuilder();

			//builder.Append("-----BEGIN CERTIFICATE-----");
			builder.Append(certToBase64String); //Convert.ToBase64String(cert.Export(X509ContentType.Cert))); //, Base64FormattingOptions.InsertLineBreaks));
			//builder.Append("-----END CERTIFICATE-----");

			return builder.ToString();
		}

		private void dataGridViewEmbeddedCertificates_SelectionChanged(object sender, EventArgs e)
		{
			// CAUTION:
			// If a row was previously selected and the user clicks onto another row,
			// the SelectionChanged() event is fired TWICE!!!
			// The first time, it is only for UNselecting the old row,
			// so the SelectedRows.Count is ZERO, so ignore this event handler!
			// The second time, SelectedRows.Count is ONE.
			// Now you can set the widgets in the "Selected Process" groupBox.

			if(dataGridViewEmbeddedCertificates.SelectedRows.Count != 1) return;
			SebInstance.Settings.embeddedCertificateIndex = dataGridViewEmbeddedCertificates.SelectedRows[0].Index;
		}


		private void dataGridViewEmbeddedCertificates_CurrentCellDirtyStateChanged(object sender, EventArgs e)
		{
			// When a CheckBox/ListBox/TextBox entry of a DataGridView table cell is edited,
			// immediately call the CellValueChanged() event.
			if(dataGridViewEmbeddedCertificates.IsCurrentCellDirty)
				dataGridViewEmbeddedCertificates.CommitEdit(DataGridViewDataErrorContexts.Commit);
		}


		private void dataGridViewEmbeddedCertificates_CellValueChanged(object sender, DataGridViewCellEventArgs e)
		{
			// Get the current cell where the user has changed a value
			int row = dataGridViewEmbeddedCertificates.CurrentCellAddress.Y;
			int column = dataGridViewEmbeddedCertificates.CurrentCellAddress.X;

			// At the beginning, row = -1 and column = -1, so skip this event
			if(row < 0) return;
			if(column < 0) return;

			// Get the changed value of the current cell
			object value = dataGridViewEmbeddedCertificates.CurrentCell.EditedFormattedValue;

			// Convert the selected Type ListBox entry from String to Integer
			if(column == IntColumnCertificateType)
			{
				if((String)value == StringSSLClientCertificate) value = IntSSLClientCertificate;
				else if((String)value == StringIdentity) value = IntIdentity;
			}

			// Get the data of the certificate belonging to the cell (row)
			SebInstance.Settings.embeddedCertificateIndex = row;
			SebInstance.Settings.embeddedCertificateList = (ListObj)SebInstance.Settings.settingsCurrent[SebSettings.KeyEmbeddedCertificates];
			SebInstance.Settings.embeddedCertificateData = (DictObj)SebInstance.Settings.embeddedCertificateList[SebInstance.Settings.embeddedCertificateIndex];

			// Update the certificate data belonging to the current cell
			if(column == IntColumnCertificateType) SebInstance.Settings.embeddedCertificateData[SebSettings.KeyType] = (Int32)value;
			if(column == IntColumnCertificateName) SebInstance.Settings.embeddedCertificateData[SebSettings.KeyName] = (String)value;
		}


		private void buttonRemoveEmbeddedCertificate_Click(object sender, EventArgs e)
		{
			if(dataGridViewEmbeddedCertificates.SelectedRows.Count != 1) return;
			SebInstance.Settings.embeddedCertificateIndex = dataGridViewEmbeddedCertificates.SelectedRows[0].Index;

			// Delete certificate from certificate list at position index
			SebInstance.Settings.embeddedCertificateList = (ListObj)SebInstance.Settings.settingsCurrent[SebSettings.KeyEmbeddedCertificates];
			SebInstance.Settings.embeddedCertificateList.RemoveAt(SebInstance.Settings.embeddedCertificateIndex);
			dataGridViewEmbeddedCertificates.Rows.RemoveAt(SebInstance.Settings.embeddedCertificateIndex);

			if(SebInstance.Settings.embeddedCertificateIndex == SebInstance.Settings.embeddedCertificateList.Count)
				SebInstance.Settings.embeddedCertificateIndex--;

			if(SebInstance.Settings.embeddedCertificateList.Count > 0)
			{
				dataGridViewEmbeddedCertificates.Rows[SebInstance.Settings.embeddedCertificateIndex].Selected = true;
			}
			else
			{
				// If certificate list is now empty, disable it
				SebInstance.Settings.embeddedCertificateIndex = -1;
				dataGridViewEmbeddedCertificates.Enabled = false;
			}
		}



		// *************************
		// Group "Network - Proxies"
		// *************************
		private void radioButtonUseSystemProxySettings_CheckedChanged(object sender, EventArgs e)
		{
			if(radioButtonUseSystemProxySettings.Checked == true)
				SebInstance.Settings.settingsCurrent[SebSettings.KeyProxySettingsPolicy] = 0;
			else SebInstance.Settings.settingsCurrent[SebSettings.KeyProxySettingsPolicy] = 1;
		}

		private void radioButtonUseSebProxySettings_CheckedChanged(object sender, EventArgs e)
		{
			if(radioButtonUseSebProxySettings.Checked == true)
				SebInstance.Settings.settingsCurrent[SebSettings.KeyProxySettingsPolicy] = 1;
			else SebInstance.Settings.settingsCurrent[SebSettings.KeyProxySettingsPolicy] = 0;
		}

		private void checkBoxExcludeSimpleHostnames_CheckedChanged(object sender, EventArgs e)
		{
			// Get the proxies data
			SebInstance.Settings.proxiesData = (DictObj)SebInstance.Settings.settingsCurrent[SebSettings.KeyProxies];
			SebInstance.Settings.proxiesData[SebSettings.KeyExcludeSimpleHostnames] = checkBoxExcludeSimpleHostnames.Checked;
		}

		private void checkBoxUsePassiveFTPMode_CheckedChanged(object sender, EventArgs e)
		{
			// Get the proxies data
			SebInstance.Settings.proxiesData = (DictObj)SebInstance.Settings.settingsCurrent[SebSettings.KeyProxies];
			SebInstance.Settings.proxiesData[SebSettings.KeyFTPPassive] = checkBoxUsePassiveFTPMode.Checked;
		}


		private void dataGridViewProxyProtocols_SelectionChanged(object sender, EventArgs e)
		{
			// CAUTION:
			// If a row was previously selected and the user clicks onto another row,
			// the SelectionChanged() event is fired TWICE!!!
			// The first time, it is only for UNselecting the old row,
			// so the SelectedRows.Count is ZERO, so ignore this event handler!
			// The second time, SelectedRows.Count is ONE.
			// Now you can set the widgets in the "Selected Process" groupBox.

			if(dataGridViewProxyProtocols.SelectedRows.Count != 1) return;
			SebInstance.Settings.proxyProtocolIndex = dataGridViewProxyProtocols.SelectedRows[0].Index;

			// if proxyProtocolIndex is    0 (AutoDiscovery    ), do nothing
			// if proxyProtocolIndex is    1 (AutoConfiguration), enable Proxy URL    widgets
			// if proxyProtocolIndex is >= 2 (... Proxy Server ), enable Proxy Server widgets

			Boolean useAutoConfiguration = (SebInstance.Settings.proxyProtocolIndex == IntProxyAutoConfiguration);
			Boolean useProxyServer = (SebInstance.Settings.proxyProtocolIndex > IntProxyAutoConfiguration);

			// Enable the proxy widgets belonging to Auto Configuration
			labelAutoProxyConfigurationURL.Visible = useAutoConfiguration;
			labelProxyConfigurationFileURL.Visible = useAutoConfiguration;
			textBoxIfYourNetworkAdministrator.Visible = useAutoConfiguration;
			textBoxAutoProxyConfigurationURL.Visible = useAutoConfiguration;
			buttonChooseProxyConfigurationFile.Visible = useAutoConfiguration;

			// Enable the proxy widgets belonging to Proxy Server
			// (HTTP, HTTPS, FTP, SOCKS, RTSP)
			labelProxyServerHost.Visible = useProxyServer;
			labelProxyServerPort.Visible = useProxyServer;
			textBoxProxyServerHost.Visible = useProxyServer;
			textBoxProxyServerPort.Visible = useProxyServer;

			labelProxyServerUsername.Visible = useProxyServer;
			labelProxyServerPassword.Visible = useProxyServer;
			textBoxProxyServerUsername.Visible = useProxyServer;
			textBoxProxyServerPassword.Visible = useProxyServer;

			checkBoxProxyServerRequires.Visible = useProxyServer;

			if(useProxyServer)
			{
				labelProxyServerHost.Text = StringProxyProtocolServerLabel[SebInstance.Settings.proxyProtocolIndex];
				labelProxyServerHost.Text += " Proxy Server";
			}

			// Get the proxy protocol type
			String KeyProtocolType = KeyProxyProtocolType[SebInstance.Settings.proxyProtocolIndex];

			// Get the proxies data
			SebInstance.Settings.proxiesData = (DictObj)SebInstance.Settings.settingsCurrent[SebSettings.KeyProxies];

			// Update the proxy widgets
			if(useAutoConfiguration)
			{
				textBoxAutoProxyConfigurationURL.Text = (String)SebInstance.Settings.proxiesData[SebSettings.KeyAutoConfigurationURL];
			}

			if(useProxyServer)
			{
				checkBoxProxyServerRequires.Checked = (Boolean)SebInstance.Settings.proxiesData[KeyProtocolType + SebSettings.KeyRequires];
				textBoxProxyServerHost.Text = (String)SebInstance.Settings.proxiesData[KeyProtocolType + SebSettings.KeyHost];
				textBoxProxyServerPort.Text = (String)SebInstance.Settings.proxiesData[KeyProtocolType + SebSettings.KeyPort].ToString();
				textBoxProxyServerUsername.Text = (String)SebInstance.Settings.proxiesData[KeyProtocolType + SebSettings.KeyUsername];
				textBoxProxyServerPassword.Text = (String)SebInstance.Settings.proxiesData[KeyProtocolType + SebSettings.KeyPassword];

				// Disable the username/password textboxes when they are not required
				textBoxProxyServerUsername.Enabled = checkBoxProxyServerRequires.Checked;
				textBoxProxyServerPassword.Enabled = checkBoxProxyServerRequires.Checked;
			}
		}


		private void dataGridViewProxyProtocols_CurrentCellDirtyStateChanged(object sender, EventArgs e)
		{
			// When a CheckBox/ListBox/TextBox entry of a DataGridView table cell is edited,
			// immediately call the CellValueChanged() event.
			if(dataGridViewProxyProtocols.IsCurrentCellDirty)
				dataGridViewProxyProtocols.CommitEdit(DataGridViewDataErrorContexts.Commit);
		}


		private void dataGridViewProxyProtocols_CellValueChanged(object sender, DataGridViewCellEventArgs e)
		{
			// Get the current cell where the user has changed a value
			int row = dataGridViewProxyProtocols.CurrentCellAddress.Y;
			int column = dataGridViewProxyProtocols.CurrentCellAddress.X;

			// At the beginning, row = -1 and column = -1, so skip this event
			if(row < 0) return;
			if(column < 0) return;

			// Get the changed value of the current cell
			object value = dataGridViewProxyProtocols.CurrentCell.EditedFormattedValue;

			// Get the proxies data of the proxy protocol belonging to the cell (row)
			SebInstance.Settings.proxiesData = (DictObj)SebInstance.Settings.settingsCurrent[SebSettings.KeyProxies];

			SebInstance.Settings.proxyProtocolIndex = row;

			// Update the proxy enable data belonging to the current cell
			if(column == IntColumnProxyProtocolEnable)
			{
				String key = KeyProxyProtocolEnable[row];
				SebInstance.Settings.proxiesData[key] = (Boolean)value;
				BooleanProxyProtocolEnabled[row] = (Boolean)value;
			}
		}


		private void textBoxAutoProxyConfigurationURL_TextChanged(object sender, EventArgs e)
		{
			// Get the proxies data
			SebInstance.Settings.proxiesData = (DictObj)SebInstance.Settings.settingsCurrent[SebSettings.KeyProxies];
			SebInstance.Settings.proxiesData[SebSettings.KeyAutoConfigurationURL] = textBoxAutoProxyConfigurationURL.Text;
		}


		private void buttonChooseProxyConfigurationFile_Click(object sender, EventArgs e)
		{

		}


		private void textBoxProxyServerHost_TextChanged(object sender, EventArgs e)
		{
			// Get the proxies data
			String key = KeyProxyProtocolType[SebInstance.Settings.proxyProtocolIndex] + SebSettings.KeyHost;
			SebInstance.Settings.proxiesData = (DictObj)SebInstance.Settings.settingsCurrent[SebSettings.KeyProxies];
			SebInstance.Settings.proxiesData[key] = textBoxProxyServerHost.Text;
		}

		private void textBoxProxyServerPort_TextChanged(object sender, EventArgs e)
		{
			// Get the proxies data
			String key = KeyProxyProtocolType[SebInstance.Settings.proxyProtocolIndex] + SebSettings.KeyPort;
			SebInstance.Settings.proxiesData = (DictObj)SebInstance.Settings.settingsCurrent[SebSettings.KeyProxies];

			// Convert the "Port" string to an integer
			try
			{
				SebInstance.Settings.proxiesData[key] = Int32.Parse(textBoxProxyServerPort.Text);
			}
			catch(FormatException)
			{
				textBoxProxyServerPort.Text = "";
			}
		}

		private void checkBoxProxyServerRequiresPassword_CheckedChanged(object sender, EventArgs e)
		{
			// Get the proxies data
			String key = KeyProxyProtocolType[SebInstance.Settings.proxyProtocolIndex] + SebSettings.KeyRequires;
			SebInstance.Settings.proxiesData = (DictObj)SebInstance.Settings.settingsCurrent[SebSettings.KeyProxies];
			SebInstance.Settings.proxiesData[key] = (Boolean)checkBoxProxyServerRequires.Checked;

			// Disable the username/password textboxes when they are not required
			textBoxProxyServerUsername.Enabled = checkBoxProxyServerRequires.Checked;
			textBoxProxyServerPassword.Enabled = checkBoxProxyServerRequires.Checked;
		}

		private void textBoxProxyServerUsername_TextChanged(object sender, EventArgs e)
		{
			// Get the proxies data
			String key = KeyProxyProtocolType[SebInstance.Settings.proxyProtocolIndex] + SebSettings.KeyUsername;
			SebInstance.Settings.proxiesData = (DictObj)SebInstance.Settings.settingsCurrent[SebSettings.KeyProxies];
			SebInstance.Settings.proxiesData[key] = textBoxProxyServerUsername.Text;
		}

		private void textBoxProxyServerPassword_TextChanged(object sender, EventArgs e)
		{
			// Get the proxies data
			String key = KeyProxyProtocolType[SebInstance.Settings.proxyProtocolIndex] + SebSettings.KeyPassword;
			SebInstance.Settings.proxiesData = (DictObj)SebInstance.Settings.settingsCurrent[SebSettings.KeyProxies];
			SebInstance.Settings.proxiesData[key] = textBoxProxyServerPassword.Text;
		}


		private void textBoxBypassedProxyHostList_TextChanged(object sender, EventArgs e)
		{
			// Get the proxies data
			SebInstance.Settings.proxiesData = (DictObj)SebInstance.Settings.settingsCurrent[SebSettings.KeyProxies];
			string bypassedProxiesCommaSeparatedList = textBoxBypassedProxyHostList.Text;
			// Create List
			List<string> bypassedProxyHostList = bypassedProxiesCommaSeparatedList.Split(',').ToList();
			// Trim whitespace from host strings
			ListObj bypassedProxyTrimmedHostList = new ListObj();
			foreach(string host in bypassedProxyHostList)
			{
				bypassedProxyTrimmedHostList.Add(host.Trim());
			}
			SebInstance.Settings.proxiesData[SebSettings.KeyExceptionsList] = bypassedProxyTrimmedHostList;
		}


		// ****************
		// Group "Security"
		// ****************
		private void listBoxSebServicePolicy_SelectedIndexChanged(object sender, EventArgs e)
		{
			SebInstance.Settings.settingsCurrent[SebSettings.KeySebServicePolicy] = listBoxSebServicePolicy.SelectedIndex;
		}

		private void checkBoxAllowVirtualMachine_CheckedChanged(object sender, EventArgs e)
		{
			SebInstance.Settings.settingsCurrent[SebSettings.KeyAllowVirtualMachine] = checkBoxAllowVirtualMachine.Checked;
		}

		private void radioCreateNewDesktop_CheckedChanged(object sender, EventArgs e)
		{
			SebInstance.Settings.settingsCurrent[SebSettings.KeyCreateNewDesktop] = radioCreateNewDesktop.Checked;
			if(radioCreateNewDesktop.Checked && (Boolean)SebInstance.Settings.settingsCurrent[SebSettings.KeyTouchOptimized] == true)
			{
				MessageBox.Show(
					"Touch optimization will not work when kiosk mode is set to Create New Desktop, please change the appearance.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
		}

		private void radioKillExplorerShell_CheckedChanged(object sender, EventArgs e)
		{
			SebInstance.Settings.settingsCurrent[SebSettings.KeyKillExplorerShell] = radioKillExplorerShell.Checked;
		}

		private void checkBoxAllowWlan_CheckedChanged(object sender, EventArgs e)
		{
			SebInstance.Settings.settingsCurrent[SebSettings.KeyAllowWLAN] = checkboxAllowWlan.Checked;
		}

		private void checkBoxAllowUserSwitching_CheckedChanged(object sender, EventArgs e)
		{
			SebInstance.Settings.settingsCurrent[SebSettings.KeyAllowUserSwitching] = checkBoxAllowUserSwitching.Checked;
		}

		private void checkBoxEnableAppSwitcherCheck_CheckedChanged(object sender, EventArgs e)
		{
			SebInstance.Settings.settingsCurrent[SebSettings.KeyEnableAppSwitcherCheck] = checkBoxEnableAppSwitcherCheck.Checked;
		}

		private void checkBoxForceAppFolderInstall_CheckedChanged(object sender, EventArgs e)
		{
			SebInstance.Settings.settingsCurrent[SebSettings.KeyForceAppFolderInstall] = checkBoxForceAppFolderInstall.Checked;
		}

		private void checkBoxEnableLogging_CheckedChanged(object sender, EventArgs e)
		{
			SebInstance.Settings.settingsCurrent[SebSettings.KeyEnableLogging] = checkBoxEnableLogging.Checked;
		}

		private void buttonLogDirectoryWin_Click(object sender, EventArgs e)
		{
			// Set the default directory in the Folder Browser Dialog
			//folderBrowserDialogLogDirectoryWin.SelectedPath = textBoxLogDirectoryWin.Text;
			folderBrowserDialogLogDirectoryWin.RootFolder = Environment.SpecialFolder.Desktop;
			//          folderBrowserDialogLogDirectoryWin.RootFolder = Environment.CurrentDirectory;

			// Get the user inputs in the File Dialog
			DialogResult dialogResult = folderBrowserDialogLogDirectoryWin.ShowDialog();
			String path = folderBrowserDialogLogDirectoryWin.SelectedPath;

			// If the user clicked "Cancel", do nothing
			if(dialogResult.Equals(DialogResult.Cancel)) return;

			// If the user clicked "OK", ...
			string pathUsingEnvironmentVariables = SEBClientInfo.ContractEnvironmentVariables(path);
			SebInstance.Settings.settingsCurrent[SebSettings.KeyLogDirectoryWin] = pathUsingEnvironmentVariables;
			textBoxLogDirectoryWin.Text = pathUsingEnvironmentVariables;
			if(String.IsNullOrEmpty(path))
			{
				checkBoxUseStandardDirectory.Checked = true;
			}
			else
			{
				checkBoxUseStandardDirectory.Checked = false;
			}
		}

		private void textBoxLogDirectoryWin_TextChanged(object sender, EventArgs e)
		{
			string path = textBoxLogDirectoryWin.Text;
			SebInstance.Settings.settingsCurrent[SebSettings.KeyLogDirectoryWin] = path;

			if(String.IsNullOrEmpty(path))
			{
				checkBoxUseStandardDirectory.Checked = true;
			}
			else
			{
				checkBoxUseStandardDirectory.Checked = false;
			}
		}

		private void checkBoxUseStandardDirectory_CheckedChanged(object sender, EventArgs e)
		{
			if(checkBoxUseStandardDirectory.Checked)
			{
				SebInstance.Settings.settingsCurrent[SebSettings.KeyLogDirectoryWin] = "";
				textBoxLogDirectoryWin.Text = "";
			}
		}

		private void textBoxLogDirectoryOSX_TextChanged(object sender, EventArgs e)
		{
			string path = textBoxLogDirectoryOSX.Text;
			SebInstance.Settings.settingsCurrent[SebSettings.KeyLogDirectoryOSX] = path;
		}


		// ****************
		// Group "Registry"
		// ****************



		// ******************
		// Group "Inside SEB"
		// ******************
		private void checkBoxInsideSebEnableSwitchUser_CheckedChanged(object sender, EventArgs e)
		{
			SebInstance.Settings.settingsCurrent[SebSettings.KeyInsideSebEnableSwitchUser] = checkBoxInsideSebEnableSwitchUser.Checked;
		}

		private void checkBoxInsideSebEnableLockThisComputer_CheckedChanged(object sender, EventArgs e)
		{
			SebInstance.Settings.settingsCurrent[SebSettings.KeyInsideSebEnableLockThisComputer] = checkBoxInsideSebEnableLockThisComputer.Checked;
		}

		private void checkBoxInsideSebEnableChangeAPassword_CheckedChanged(object sender, EventArgs e)
		{
			SebInstance.Settings.settingsCurrent[SebSettings.KeyInsideSebEnableChangeAPassword] = checkBoxInsideSebEnableChangeAPassword.Checked;
		}

		private void checkBoxInsideSebEnableStartTaskManager_CheckedChanged(object sender, EventArgs e)
		{
			SebInstance.Settings.settingsCurrent[SebSettings.KeyInsideSebEnableStartTaskManager] = checkBoxInsideSebEnableStartTaskManager.Checked;

		}

		private void checkBoxInsideSebEnableLogOff_CheckedChanged(object sender, EventArgs e)
		{
			SebInstance.Settings.settingsCurrent[SebSettings.KeyInsideSebEnableLogOff] = checkBoxInsideSebEnableLogOff.Checked;
		}

		private void checkBoxInsideSebEnableShutDown_CheckedChanged(object sender, EventArgs e)
		{
			SebInstance.Settings.settingsCurrent[SebSettings.KeyInsideSebEnableShutDown] = checkBoxInsideSebEnableShutDown.Checked;
		}

		private void checkBoxInsideSebEnableEaseOfAccess_CheckedChanged(object sender, EventArgs e)
		{
			SebInstance.Settings.settingsCurrent[SebSettings.KeyInsideSebEnableEaseOfAccess] = checkBoxInsideSebEnableEaseOfAccess.Checked;
		}

		private void checkBoxInsideSebEnableVmWareClientShade_CheckedChanged(object sender, EventArgs e)
		{
			SebInstance.Settings.settingsCurrent[SebSettings.KeyInsideSebEnableVmWareClientShade] = checkBoxInsideSebEnableVmWareClientShade.Checked;
		}


		// *******************
		// Group "Hooked Keys"
		// *******************
		private void checkBoxHookKeys_CheckedChanged(object sender, EventArgs e)
		{
			SebInstance.Settings.settingsCurrent[SebSettings.KeyHookKeys] = checkBoxHookKeys.Checked;
		}



		// ********************
		// Group "Special Keys"
		// ********************
		private void checkBoxEnableEsc_CheckedChanged(object sender, EventArgs e)
		{
			SebInstance.Settings.settingsCurrent[SebSettings.KeyEnableEsc] = checkBoxEnableEsc.Checked;
		}

		private void checkBoxEnableCtrlEsc_CheckedChanged(object sender, EventArgs e)
		{
			SebInstance.Settings.settingsCurrent[SebSettings.KeyEnableCtrlEsc] = checkBoxEnableCtrlEsc.Checked;
		}

		private void checkBoxEnableAltEsc_CheckedChanged(object sender, EventArgs e)
		{
			SebInstance.Settings.settingsCurrent[SebSettings.KeyEnableAltEsc] = checkBoxEnableAltEsc.Checked;
		}

		private void checkBoxEnableAltTab_CheckedChanged(object sender, EventArgs e)
		{
			SebInstance.Settings.settingsCurrent[SebSettings.KeyEnableAltTab] = checkBoxEnableAltTab.Checked;
		}

		private void checkBoxEnableAltF4_CheckedChanged(object sender, EventArgs e)
		{
			SebInstance.Settings.settingsCurrent[SebSettings.KeyEnableAltF4] = checkBoxEnableAltF4.Checked;
		}

		private void checkBoxEnableRightMouse_CheckedChanged(object sender, EventArgs e)
		{
			SebInstance.Settings.settingsCurrent[SebSettings.KeyEnableRightMouse] = checkBoxEnableRightMouse.Checked;
		}

		private void checkBoxEnablePrintScreen_CheckedChanged(object sender, EventArgs e)
		{
			SebInstance.Settings.settingsCurrent[SebSettings.KeyEnablePrintScreen] = checkBoxEnablePrintScreen.Checked;
			checkBoxEnableScreenCapture.Checked = checkBoxEnablePrintScreen.Checked;

		}

		private void checkBoxEnableAltMouseWheel_CheckedChanged(object sender, EventArgs e)
		{
			SebInstance.Settings.settingsCurrent[SebSettings.KeyEnableAltMouseWheel] = checkBoxEnableAltMouseWheel.Checked;
			checkBoxAllowBrowsingBackForward.Checked = checkBoxEnableAltMouseWheel.Checked;
		}


		// *********************
		// Group "Function Keys"
		// *********************
		private void checkBoxEnableF1_CheckedChanged(object sender, EventArgs e)
		{
			SebInstance.Settings.settingsCurrent[SebSettings.KeyEnableF1] = checkBoxEnableF1.Checked;
		}

		private void checkBoxEnableF2_CheckedChanged(object sender, EventArgs e)
		{
			SebInstance.Settings.settingsCurrent[SebSettings.KeyEnableF2] = checkBoxEnableF2.Checked;
		}

		private void checkBoxEnableF3_CheckedChanged(object sender, EventArgs e)
		{
			SebInstance.Settings.settingsCurrent[SebSettings.KeyEnableF3] = checkBoxEnableF3.Checked;
		}

		private void checkBoxEnableF4_CheckedChanged(object sender, EventArgs e)
		{
			SebInstance.Settings.settingsCurrent[SebSettings.KeyEnableF4] = checkBoxEnableF4.Checked;
		}

		private void checkBoxEnableF5_CheckedChanged(object sender, EventArgs e)
		{
			SebInstance.Settings.settingsCurrent[SebSettings.KeyEnableF5] = checkBoxEnableF5.Checked;
		}

		private void checkBoxEnableF6_CheckedChanged(object sender, EventArgs e)
		{
			SebInstance.Settings.settingsCurrent[SebSettings.KeyEnableF6] = checkBoxEnableF6.Checked;
		}

		private void checkBoxEnableF7_CheckedChanged(object sender, EventArgs e)
		{
			SebInstance.Settings.settingsCurrent[SebSettings.KeyEnableF7] = checkBoxEnableF7.Checked;
		}

		private void checkBoxEnableF8_CheckedChanged(object sender, EventArgs e)
		{
			SebInstance.Settings.settingsCurrent[SebSettings.KeyEnableF8] = checkBoxEnableF8.Checked;
		}

		private void checkBoxEnableF9_CheckedChanged(object sender, EventArgs e)
		{
			SebInstance.Settings.settingsCurrent[SebSettings.KeyEnableF9] = checkBoxEnableF9.Checked;
		}

		private void checkBoxEnableF10_CheckedChanged(object sender, EventArgs e)
		{
			SebInstance.Settings.settingsCurrent[SebSettings.KeyEnableF10] = checkBoxEnableF10.Checked;
		}

		private void checkBoxEnableF11_CheckedChanged(object sender, EventArgs e)
		{
			SebInstance.Settings.settingsCurrent[SebSettings.KeyEnableF11] = checkBoxEnableF11.Checked;
		}

		private void checkBoxEnableF12_CheckedChanged(object sender, EventArgs e)
		{
			SebInstance.Settings.settingsCurrent[SebSettings.KeyEnableF12] = checkBoxEnableF12.Checked;
		}

		private void labelHashedAdminPassword_Click(object sender, EventArgs e)
		{

		}

		private void labelOpenLinksHTML_Click(object sender, EventArgs e)
		{

		}

		private void label6_Click(object sender, EventArgs e)
		{

		}

		private void checkBoxEnableZoomText_CheckedChanged(object sender, EventArgs e)
		{
			SebInstance.Settings.settingsCurrent[SebSettings.KeyEnableZoomText] = checkBoxEnableZoomText.Checked;
			enableZoomAdjustZoomMode();
		}

		private void checkBoxEnableZoomPage_CheckedChanged(object sender, EventArgs e)
		{
			SebInstance.Settings.settingsCurrent[SebSettings.KeyEnableZoomPage] = checkBoxEnableZoomPage.Checked;
			enableZoomAdjustZoomMode();
		}

		private void enableZoomAdjustZoomMode()
		{
			if(!checkBoxEnableZoomPage.Checked && !checkBoxEnableZoomText.Checked)
			{
				groupBoxZoomMode.Enabled = false;
			}
			else if(checkBoxEnableZoomPage.Checked && !checkBoxEnableZoomText.Checked)
			{
				groupBoxZoomMode.Enabled = true;
				radioButtonUseZoomPage.Checked = true;
				radioButtonUseZoomPage.Enabled = true;
				radioButtonUseZoomText.Enabled = false;
			}
			else if(!checkBoxEnableZoomPage.Checked && checkBoxEnableZoomText.Checked)
			{
				groupBoxZoomMode.Enabled = true;
				radioButtonUseZoomText.Checked = true;
				radioButtonUseZoomText.Enabled = true;
				radioButtonUseZoomPage.Enabled = false;
			}
			else
			{
				groupBoxZoomMode.Enabled = true;
				radioButtonUseZoomPage.Enabled = true;
				radioButtonUseZoomText.Enabled = true;
			}
		}

		private void checkBoxAllowSpellCheck_CheckedChanged(object sender, EventArgs e)
		{
			SebInstance.Settings.settingsCurrent[SebSettings.KeyAllowSpellCheck] = checkBoxAllowSpellCheck.Checked;
		}

		private void checkBoxAllowDictionaryLookup_CheckedChanged(object sender, EventArgs e)
		{
			SebInstance.Settings.settingsCurrent[SebSettings.KeyAllowDictionaryLookup] = checkBoxAllowDictionaryLookup.Checked;
		}


		private void checkBoxShowTime_CheckedChanged(object sender, EventArgs e)
		{
			SebInstance.Settings.settingsCurrent[SebSettings.KeyShowTime] = checkBoxShowTime.Checked;
		}

		private void checkBoxShowKeyboardLayout_CheckedChanged(object sender, EventArgs e)
		{
			SebInstance.Settings.settingsCurrent[SebSettings.KeyShowInputLanguage] = checkBoxShowKeyboardLayout.Checked;
		}

		private void SebWindowsConfigForm_Load(object sender, EventArgs e)
		{

		}

		private void editDuplicateToolStripMenuItem_Click(object sender, EventArgs e)
		{
			buttonEditDuplicate_Click(null, null);
		}

		private void configureClientToolStripMenuItem_Click(object sender, EventArgs e)
		{
			buttonConfigureClient_Click(null, null);
		}

		private void applyAndStartSEBToolStripMenuItem_Click(object sender, EventArgs e)
		{
			buttonApplyAndStartSEB_Click(null, null);
		}

		private void openSettingsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			buttonOpenSettings_Click(null, null);
		}

		private void saveSettingsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			buttonSaveSettings_Click(null, null);
		}

		private void saveSettingsAsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			buttonSaveSettingsAs_Click(null, null);
		}

		private void defaultSettingsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			buttonRevertToDefaultSettings_Click(null, null);
		}

		private void localClientSettingsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			buttonRevertToLocalClientSettings_Click(null, null);
		}

		private void lastOpenedToolStripMenuItem_Click(object sender, EventArgs e)
		{
			buttonRevertToLastOpened_Click(null, null);
		}

		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if(ArePasswordsUnconfirmed()) return;
			Application.Exit();
		}

		private void SebWindowsConfigForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			if(!quittingMyself)
			{
				if(ArePasswordsUnconfirmed())
				{
					e.Cancel = true;
					return;
				}

				int result = checkSettingsChanged();
				// User selected cancel, abort
				if(result == 2)
				{
					e.Cancel = true;
					return;
				}
				// User selected "Save current settings first: yes"
				if(result == 1)
				{
					// Abort if saving settings failed
					if(!saveCurrentSettings())
					{
						e.Cancel = true;
						return;
					}
					quittingMyself = true;
					Application.Exit();
				}
			}
		}

		private void SebWindowsConfigForm_DragDrop(object sender, DragEventArgs e)
		{
			string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
			if(files.Length > 0)
			{
				string filePath = files[0];
				string fileExtension = Path.GetExtension(filePath);
				if(String.Equals(fileExtension, ".seb",
				   StringComparison.OrdinalIgnoreCase))
				{
					openSettingsFile(filePath);
				}
			}
		}

		private void SebWindowsConfigForm_DragEnter(object sender, DragEventArgs e)
		{
			if(e.Data.GetDataPresent(DataFormats.FileDrop))
				e.Effect = DragDropEffects.Copy;
			else
				e.Effect = DragDropEffects.None;
		}

		private void tabControlSebWindowsConfig_Selecting(object sender, TabControlCancelEventArgs e)
		{
			if(ArePasswordsUnconfirmed())
			{
				e.Cancel = true;
			}
		}

		private void label1_Click(object sender, EventArgs e)
		{

		}

		private void checkBoxEnableScreenCapture_CheckedChanged(object sender, EventArgs e)
		{
			SebInstance.Settings.settingsCurrent[SebSettings.KeyEnablePrintScreen] = checkBoxEnableScreenCapture.Checked;
			checkBoxEnablePrintScreen.Checked = checkBoxEnableScreenCapture.Checked;
		}

	} // end of   class     SebWindowsConfigForm
}     // end of   namespace SebWindowsConfig
