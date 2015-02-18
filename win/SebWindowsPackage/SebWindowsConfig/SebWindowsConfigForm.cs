using System;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Serialization;
using SebWindowsClient;
using SebWindowsClient.CryptographyUtils;
using SebWindowsClient.ConfigurationUtils;
using SebWindowsClient.DiagnosticsUtils;
using ListObj  = System.Collections.Generic.List                <object>;
using DictObj  = System.Collections.Generic.Dictionary  <string, object>;
using KeyValue = System.Collections.Generic.KeyValuePair<string, object>;



namespace SebWindowsConfig
{
    public partial class SebWindowsConfigForm : Form
    {
        public bool    adminPasswordFieldsContainHash = false;
        public bool     quitPasswordFieldsContainHash = false;
        public bool settingsPasswordFieldsContainHash = false;

        string settingsPassword = "";

        private string lastBrowserExamKey = "";

        private const string SEB_CONFIG_LOG = "SebConfig.log";

        //X509Certificate2 fileCertificateRef = null;

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
            string        SebConfigLogFile        = sebConfigLogFileBuilder.ToString();

            Logger.initLogger(SEBClientInfo.SebClientLogFileDirectory, SebConfigLogFile);

            // Set all the default values for the Plist structure "SEBSettings.settingsCurrent"
            SEBSettings.RestoreDefaultAndCurrentSettings();
            SEBSettings.PermitXulRunnerProcess();

            // Initialise the global variables for the GUI widgets
            InitialiseGlobalVariablesForGUIWidgets();

            // Initialise the GUI widgets themselves
            InitialiseGUIWidgets();

            // When starting up, load the default local client settings
            if (!LoadConfigurationFileIntoEditor(currentPathSebConfigFile))
            {
                // If this didn't work, then there are no local client settings and we set the current settings title to "Default Settings"
                currentPathSebConfigFile = SEBUIStrings.settingsTitleDefaultSettings;
                UpdateAllWidgetsOfProgram();
            };

        } // end of contructor   SebWindowsConfigForm()




        // *************************************************
        // Open the configuration file and read the settings
        // *************************************************
        private Boolean LoadConfigurationFileIntoEditor(String fileName)
        {
            Cursor.Current = Cursors.WaitCursor;
            // Read the file into "new" settings

            // In these variables we get back the configuration file password the user entered for decrypting and/or 
            // the certificate reference found in the config file:
            string           filePassword       = null;
            X509Certificate2 fileCertificateRef = null;
            bool             passwordIsHash     = false;

            if (!SEBSettings.ReadSebConfigurationFile(fileName, true, ref filePassword, ref passwordIsHash, ref fileCertificateRef)) return false;

            if (!String.IsNullOrEmpty(filePassword)) {
                // If we got the settings password because the user entered it when opening the .seb file, 
                // we store it in a local variable
                settingsPassword                  = filePassword;
                settingsPasswordFieldsContainHash = passwordIsHash;
            }
            else
            {
                // We didn't get back any settings password, we clear the local variable
                settingsPassword                  = "";
                settingsPasswordFieldsContainHash = false;
            }

            // Check if we got a certificate reference used to encrypt the openend settings back
            if (fileCertificateRef != null)
            {
                comboBoxCryptoIdentity.SelectedIndex = 0;
                int indexOfCertificateRef = certificateReferences.IndexOf(fileCertificateRef);
                // Find this certificate reference in the list of all certificates from the certificate store
                // if found (this should always be the case), select that certificate in the comboBox list
                if (indexOfCertificateRef != -1) comboBoxCryptoIdentity.SelectedIndex = indexOfCertificateRef+1;
            }

            //Plist.writeXml(SEBSettings.settingsDefault, "DebugSettingsDefault_In_LoadConfigurationFile.xml");
            //Plist.writeXml(SEBSettings.settingsCurrent, "DebugSettingsCurrent_In_LoadConfigurationFile.xml");
            //SEBSettings.LoggSettingsDictionary(ref SEBSettings.settingsDefault, "DebugSettingsDefault_In_LoadConfigurationFile.txt");
            //SEBSettings.LoggSettingsDictionary(ref SEBSettings.settingsCurrent, "DebugSettingsCurrent_In_LoadConfigurationFile.txt");

            // GUI-related part: Update the widgets
            currentDireSebConfigFile = Path.GetDirectoryName(fileName);
            currentFileSebConfigFile = Path.GetFileName     (fileName);
            currentPathSebConfigFile = Path.GetFullPath     (fileName);

            // After loading a new config file, reset the URL Filter Table indices
            // to avoid errors, in case there was a non-empty URL Filter Table displayed
            // in the DataGridViewURLFilterRules prior to loading the new config file.
                        urlFilterTableRow        = -1;
                        urlFilterTableRowIsTitle =  false;
            SEBSettings.urlFilterRuleIndex       = -1;
            SEBSettings.urlFilterActionIndex     = -1;

            // Get the URL Filter Rules
            SEBSettings.urlFilterRuleList = (ListObj)SEBSettings.settingsCurrent[SEBSettings.KeyURLFilterRules];

            // If there are any filter rules, select first filter rule.
            // If there are no  filter rules, select no    filter rule.
            if  (SEBSettings.urlFilterRuleList.Count > 0)
                 SEBSettings.urlFilterRuleIndex =  0;
            else SEBSettings.urlFilterRuleIndex = -1;

            // Initially show all filter rules with their actions (expanded view)
            urlFilterTableShowRule.Clear();
            for (int ruleIndex = 0; ruleIndex < SEBSettings.urlFilterRuleList.Count; ruleIndex++)
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
            int selectedCertificate = (int)SEBSettings.intArrayCurrent[SEBSettings.ValCryptoIdentity];
            if (selectedCertificate > 0)
            {
                fileCertificateRef = (X509Certificate2)certificateReferences[selectedCertificate-1];
            } //comboBoxCryptoIdentity.SelectedIndex;

            // Get selected config purpose
            int currentConfigPurpose = (int)SEBSettings.valueForDictionaryKey(SEBSettings.settingsCurrent, SEBSettings.KeySebConfigPurpose);
            SEBSettings.sebConfigPurposes configPurpose = (SEBSettings.sebConfigPurposes)currentConfigPurpose;

            // Write the "new" settings to file
            if (!SEBSettings.WriteSebConfigurationFile(fileName, filePassword, settingsPasswordFieldsContainHash, fileCertificateRef, configPurpose)) return false;

            // If the settings could be written to file, update the widgets
            currentDireSebConfigFile = Path.GetDirectoryName(fileName);
            currentFileSebConfigFile = Path.GetFileName     (fileName);
            currentPathSebConfigFile = Path.GetFullPath     (fileName);

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
            this.Text  = this.ProductName;
            this.Text += " - ";
            this.Text += currentPathSebConfigFile;

            // Group "General"
            textBoxStartURL    .Text = (String)SEBSettings.settingsCurrent[SEBSettings.KeyStartURL];
            textBoxSebServerURL.Text = (String)SEBSettings.settingsCurrent[SEBSettings.KeySebServerURL];

            // If an admin password is saved in the settings (as a hash), 
            // then we fill a placeholder string into the admin password text fields
            if (!String.IsNullOrEmpty((String)SEBSettings.settingsCurrent[SEBSettings.KeyHashedAdminPassword]))
            {
                // CAUTION: Do not change the order of setting the placeholders and the flag,
                // since the fired textBox..._TextChanged() events use these data!
                textBoxAdminPassword.Text        = "0000000000000000";
                adminPasswordFieldsContainHash   = true;
                textBoxConfirmAdminPassword.Text = "0000000000000000";
            }
            else
            {
                // CAUTION: Do not change the order of setting the placeholders and the flag,
                // since the fired textBox..._TextChanged() events use these data!
                adminPasswordFieldsContainHash   = false;
                textBoxAdminPassword       .Text = "";
                textBoxConfirmAdminPassword.Text = "";
            }

            checkBoxAllowQuit         .Checked = (Boolean)SEBSettings.settingsCurrent[SEBSettings.KeyAllowQuit];
            checkBoxIgnoreQuitPassword.Checked = (Boolean)SEBSettings.settingsCurrent[SEBSettings.KeyIgnoreQuitPassword];
            checkBoxIgnoreExitKeys    .Checked = (Boolean)SEBSettings.settingsCurrent[SEBSettings.KeyIgnoreExitKeys];

            // If a quit password is saved in the settings (as a hash), 
            // then we fill a placeholder string into the quit password text fields
            if (!String.IsNullOrEmpty((String)SEBSettings.settingsCurrent[SEBSettings.KeyHashedQuitPassword]))
            {
                // CAUTION: Do not change the order of setting the placeholders and the flag,
                // since the fired textBox..._TextChanged() events use these data!
                textBoxQuitPassword.Text        = "0000000000000000";
                quitPasswordFieldsContainHash   = true;
                textBoxConfirmQuitPassword.Text = "0000000000000000";
            }
            else
            {
                // CAUTION: Do not change the order of setting the placeholders and the flag,
                // since the fired textBox..._TextChanged() events use these data!
                quitPasswordFieldsContainHash   = false;
                textBoxQuitPassword       .Text = "";
                textBoxConfirmQuitPassword.Text = "";
            }

            listBoxExitKey1.SelectedIndex = (int)SEBSettings.settingsCurrent[SEBSettings.KeyExitKey1];
            listBoxExitKey2.SelectedIndex = (int)SEBSettings.settingsCurrent[SEBSettings.KeyExitKey2];
            listBoxExitKey3.SelectedIndex = (int)SEBSettings.settingsCurrent[SEBSettings.KeyExitKey3];

            // Group "Config File"
            radioButtonStartingAnExam     .Checked =    ((int)SEBSettings.settingsCurrent[SEBSettings.KeySebConfigPurpose] == 0);
            radioButtonConfiguringAClient .Checked =    ((int)SEBSettings.settingsCurrent[SEBSettings.KeySebConfigPurpose] == 1);
            checkBoxAllowPreferencesWindow.Checked = (Boolean)SEBSettings.settingsCurrent[SEBSettings.KeyAllowPreferencesWindow];
            comboBoxCryptoIdentity.SelectedIndex   =          SEBSettings.intArrayCurrent[SEBSettings.ValCryptoIdentity];

            // If the settings password local variable contains a hash (and it isn't empty)
            if (settingsPasswordFieldsContainHash && !String.IsNullOrEmpty(settingsPassword))
            {
                // CAUTION: We need to reset this flag BEFORE changing the textBox text value,
                // because otherwise the compare passwords method will delete the first textBox again.
                settingsPasswordFieldsContainHash   = false;
                textBoxSettingsPassword.Text        = "0000000000000000";
                settingsPasswordFieldsContainHash   = true;
                textBoxConfirmSettingsPassword.Text = "0000000000000000";
            }
            else
            {
                textBoxSettingsPassword       .Text = settingsPassword;
                textBoxConfirmSettingsPassword.Text = settingsPassword;
            }

            // Group "Appearance"
            if ((int)SEBSettings.settingsCurrent[SEBSettings.KeyTouchOptimized] == 1)
            {
                radioButtonTouchOptimized.Checked = true;
            }
            else
            {
                radioButtonUseBrowserWindow.Checked = ((int)SEBSettings.settingsCurrent[SEBSettings.KeyBrowserViewMode] == 0);
                radioButtonUseFullScreenMode.Checked = ((int)SEBSettings.settingsCurrent[SEBSettings.KeyBrowserViewMode] == 1);
            }
            comboBoxMainBrowserWindowWidth    .Text        =  (String)SEBSettings.settingsCurrent[SEBSettings.KeyMainBrowserWindowWidth];
            comboBoxMainBrowserWindowHeight   .Text        =  (String)SEBSettings.settingsCurrent[SEBSettings.KeyMainBrowserWindowHeight];
             listBoxMainBrowserWindowPositioning.SelectedIndex = (int)SEBSettings.settingsCurrent[SEBSettings.KeyMainBrowserWindowPositioning];
            checkBoxEnableBrowserWindowToolbar.Checked     = (Boolean)SEBSettings.settingsCurrent[SEBSettings.KeyEnableBrowserWindowToolbar];
            checkBoxHideBrowserWindowToolbar  .Checked     = (Boolean)SEBSettings.settingsCurrent[SEBSettings.KeyHideBrowserWindowToolbar];
            checkBoxShowMenuBar               .Checked     = (Boolean)SEBSettings.settingsCurrent[SEBSettings.KeyShowMenuBar];
            checkBoxShowTaskBar               .Checked     = (Boolean)SEBSettings.settingsCurrent[SEBSettings.KeyShowTaskBar];
            comboBoxTaskBarHeight             .Text        =  (String)SEBSettings.settingsCurrent[SEBSettings.KeyTaskBarHeight].ToString();

            // Group "Browser"
             listBoxOpenLinksHTML .SelectedIndex =     (int)SEBSettings.settingsCurrent[SEBSettings.KeyNewBrowserWindowByLinkPolicy];
             listBoxOpenLinksJava .SelectedIndex =     (int)SEBSettings.settingsCurrent[SEBSettings.KeyNewBrowserWindowByScriptPolicy];
            checkBoxBlockLinksHTML.Checked       = (Boolean)SEBSettings.settingsCurrent[SEBSettings.KeyNewBrowserWindowByLinkBlockForeign];
            checkBoxBlockLinksJava.Checked       = (Boolean)SEBSettings.settingsCurrent[SEBSettings.KeyNewBrowserWindowByScriptBlockForeign];

            comboBoxNewBrowserWindowWidth      .Text          = (String)SEBSettings.settingsCurrent[SEBSettings.KeyNewBrowserWindowByLinkWidth ];
            comboBoxNewBrowserWindowHeight     .Text          = (String)SEBSettings.settingsCurrent[SEBSettings.KeyNewBrowserWindowByLinkHeight];
             listBoxNewBrowserWindowPositioning.SelectedIndex =    (int)SEBSettings.settingsCurrent[SEBSettings.KeyNewBrowserWindowByLinkPositioning];

            checkBoxEnablePlugIns           .Checked =   (Boolean)SEBSettings.settingsCurrent[SEBSettings.KeyEnablePlugIns];
            checkBoxEnableJava              .Checked =   (Boolean)SEBSettings.settingsCurrent[SEBSettings.KeyEnableJava];
            checkBoxEnableJavaScript        .Checked =   (Boolean)SEBSettings.settingsCurrent[SEBSettings.KeyEnableJavaScript];
            checkBoxBlockPopUpWindows       .Checked =   (Boolean)SEBSettings.settingsCurrent[SEBSettings.KeyBlockPopUpWindows];
            checkBoxAllowBrowsingBackForward.Checked =   (Boolean)SEBSettings.settingsCurrent[SEBSettings.KeyAllowBrowsingBackForward];
            checkBoxRemoveProfile           .Checked =   (Boolean)SEBSettings.settingsCurrent[SEBSettings.KeyRemoveBrowserProfile];
            checkBoxUseSebWithoutBrowser    .Checked = !((Boolean)SEBSettings.settingsCurrent[SEBSettings.KeyEnableSebBrowser]);
            // BEWARE: you must invert this value since "Use Without" is "Not Enable"!

            // Group "Down/Uploads"
            checkBoxAllowDownUploads.Checked           = (Boolean)SEBSettings.settingsCurrent[SEBSettings.KeyAllowDownUploads];
            checkBoxOpenDownloads   .Checked           = (Boolean)SEBSettings.settingsCurrent[SEBSettings.KeyOpenDownloads];
            checkBoxDownloadPDFFiles.Checked           = (Boolean)SEBSettings.settingsCurrent[SEBSettings.KeyDownloadPDFFiles];
            labelDownloadDirectoryWin.Text             =  (String)SEBSettings.settingsCurrent[SEBSettings.KeyDownloadDirectoryWin];
            textBoxDownloadDirectoryOSX.Text = (String)SEBSettings.settingsCurrent[SEBSettings.KeyDownloadDirectoryOSX];
            listBoxChooseFileToUploadPolicy.SelectedIndex = (int)SEBSettings.settingsCurrent[SEBSettings.KeyChooseFileToUploadPolicy];
            checkBoxDownloadOpenSEBFiles.Checked = (Boolean)SEBSettings.settingsCurrent[SEBSettings.KeyDownloadAndOpenSebConfig];

            // Group "Exam"
           //textBoxBrowserExamKey    .Text    =  (String)SEBSettings.settingsCurrent[SEBSettings.KeyBrowserExamKey];
            textBoxQuitURL           .Text    =  (String)SEBSettings.settingsCurrent[SEBSettings.KeyQuitURL];
            checkBoxSendBrowserExamKey.Checked = (Boolean)SEBSettings.settingsCurrent[SEBSettings.KeySendBrowserExamKey];
            checkBoxRestartExamPasswordProtected.Checked = (Boolean)SEBSettings.settingsCurrent[SEBSettings.KeyRestartExamPasswordProtected];
            textBoxRestartExamLink.Text = (String)SEBSettings.settingsCurrent[SEBSettings.KeyRestartExamURL];
            textBoxRestartExamText.Text = (String)SEBSettings.settingsCurrent[SEBSettings.KeyRestartExamText];

            // Group "Applications"
            checkBoxMonitorProcesses         .Checked = (Boolean)SEBSettings.settingsCurrent[SEBSettings.KeyMonitorProcesses];
            checkBoxAllowSwitchToApplications.Checked = (Boolean)SEBSettings.settingsCurrent[SEBSettings.KeyAllowSwitchToApplications];
            checkBoxAllowFlashFullscreen     .Checked = (Boolean)SEBSettings.settingsCurrent[SEBSettings.KeyAllowFlashFullscreen];


            // Group "Applications - Permitted/Prohibited Processes"
            // Group "Network      -    Filter/Certificates/Proxies"

            // Update the lists for the DataGridViews
            SEBSettings.   permittedProcessList = (ListObj)SEBSettings.settingsCurrent[SEBSettings.KeyPermittedProcesses];
            SEBSettings.  prohibitedProcessList = (ListObj)SEBSettings.settingsCurrent[SEBSettings.KeyProhibitedProcesses];
            SEBSettings.embeddedCertificateList = (ListObj)SEBSettings.settingsCurrent[SEBSettings.KeyEmbeddedCertificates];
            SEBSettings.proxiesData             = (DictObj)SEBSettings.settingsCurrent[SEBSettings.KeyProxies];

            SEBSettings.bypassedProxyList       = (ListObj)SEBSettings.proxiesData[SEBSettings.KeyExceptionsList];

             // Check if currently loaded lists have any entries
            if  (SEBSettings.permittedProcessList.Count > 0)
                 SEBSettings.permittedProcessIndex =  0;
            else SEBSettings.permittedProcessIndex = -1;

            if  (SEBSettings.prohibitedProcessList.Count > 0)
                 SEBSettings.prohibitedProcessIndex =  0;
            else SEBSettings.prohibitedProcessIndex = -1;

            if  (SEBSettings.embeddedCertificateList.Count > 0)
                 SEBSettings.embeddedCertificateIndex =  0;
            else SEBSettings.embeddedCertificateIndex = -1;

            SEBSettings.proxyProtocolIndex = 0;

            if  (SEBSettings.bypassedProxyList.Count > 0)
                 SEBSettings.bypassedProxyIndex =  0;
            else SEBSettings.bypassedProxyIndex = -1;

            // Remove all previously displayed list entries from DataGridViews
                groupBoxPermittedProcess  .Enabled = (SEBSettings.permittedProcessList.Count > 0);
            dataGridViewPermittedProcesses.Enabled = (SEBSettings.permittedProcessList.Count > 0);
            dataGridViewPermittedProcesses.Rows.Clear();

                groupBoxProhibitedProcess  .Enabled = (SEBSettings.prohibitedProcessList.Count > 0);
            dataGridViewProhibitedProcesses.Enabled = (SEBSettings.prohibitedProcessList.Count > 0);
            dataGridViewProhibitedProcesses.Rows.Clear();

            dataGridViewEmbeddedCertificates.Enabled = (SEBSettings.embeddedCertificateList.Count > 0);
            dataGridViewEmbeddedCertificates.Rows.Clear();

            dataGridViewProxyProtocols.Enabled = true;
            dataGridViewProxyProtocols.Rows.Clear();

            textBoxBypassedProxyHostList.Text = "";

            // Add Permitted Processes of currently opened file to DataGridView
            for (int index = 0; index < SEBSettings.permittedProcessList.Count; index++)
            {
                SEBSettings.permittedProcessData = (DictObj)SEBSettings.permittedProcessList[index];
                Boolean     active               = (Boolean)SEBSettings.permittedProcessData[SEBSettings.KeyActive];
                Int32       os                   = (Int32  )SEBSettings.permittedProcessData[SEBSettings.KeyOS];
                String      executable           = (String )SEBSettings.permittedProcessData[SEBSettings.KeyExecutable];
                String      title                = (String )SEBSettings.permittedProcessData[SEBSettings.KeyTitle];
                dataGridViewPermittedProcesses.Rows.Add(active, StringOS[os], executable, title);
            }

            // Add Prohibited Processes of currently opened file to DataGridView
            for (int index = 0; index < SEBSettings.prohibitedProcessList.Count; index++)
            {
                SEBSettings.prohibitedProcessData = (DictObj)SEBSettings.prohibitedProcessList[index];
                Boolean     active                = (Boolean)SEBSettings.prohibitedProcessData[SEBSettings.KeyActive];
                Int32       os                    = (Int32  )SEBSettings.prohibitedProcessData[SEBSettings.KeyOS];
                String      executable            = (String )SEBSettings.prohibitedProcessData[SEBSettings.KeyExecutable];
                String      description           = (String )SEBSettings.prohibitedProcessData[SEBSettings.KeyDescription];
                dataGridViewProhibitedProcesses.Rows.Add(active, StringOS[os], executable, description);
            }

            // Add Url Filters
            datagridWhitelist.Rows.Clear();
            foreach (var whiteListFilterItem in SEBSettings.settingsCurrent[SEBSettings.KeyUrlFilterWhitelist].ToString().Split(';'))
            {
                if(!String.IsNullOrWhiteSpace(whiteListFilterItem))
                    datagridWhitelist.Rows.Add(whiteListFilterItem);
            }

            datagridBlackListFilter.Rows.Clear();
            foreach (var blackListFilterItem in SEBSettings.settingsCurrent[SEBSettings.KeyUrlFilterBlacklist].ToString().Split(';'))
            {
                if (!String.IsNullOrWhiteSpace(blackListFilterItem))
                    datagridBlackListFilter.Rows.Add(blackListFilterItem);
            }

            // Add Embedded Certificates of Certificate Store to DataGridView
            for (int index = 0; index < SEBSettings.embeddedCertificateList.Count; index++)
            {
                SEBSettings.embeddedCertificateData = (DictObj)SEBSettings.embeddedCertificateList[index];
                byte[] data = (byte[])SEBSettings.embeddedCertificateData[SEBSettings.KeyCertificateData];
                Int32       type                    = (Int32  )SEBSettings.embeddedCertificateData[SEBSettings.KeyType];
                String      name                    = (String )SEBSettings.embeddedCertificateData[SEBSettings.KeyName];
                dataGridViewEmbeddedCertificates.Rows.Add(StringCertificateType[type], name);
            }
/*
            // Get the "Enabled" boolean values of current "proxies" dictionary
            BooleanProxyProtocolEnabled[IntProxyAutoDiscovery    ] = (Boolean)SEBSettings.proxiesData[SEBSettings.KeyAutoDiscoveryEnabled];
            BooleanProxyProtocolEnabled[IntProxyAutoConfiguration] = (Boolean)SEBSettings.proxiesData[SEBSettings.KeyAutoConfigurationEnabled];
            BooleanProxyProtocolEnabled[IntProxyHTTP             ] = (Boolean)SEBSettings.proxiesData[SEBSettings.KeyHTTPEnable];
            BooleanProxyProtocolEnabled[IntProxyHTTPS            ] = (Boolean)SEBSettings.proxiesData[SEBSettings.KeyHTTPSEnable];
            BooleanProxyProtocolEnabled[IntProxyFTP              ] = (Boolean)SEBSettings.proxiesData[SEBSettings.KeyFTPEnable];
            BooleanProxyProtocolEnabled[IntProxySOCKS            ] = (Boolean)SEBSettings.proxiesData[SEBSettings.KeySOCKSEnable];
            BooleanProxyProtocolEnabled[IntProxyRTSP             ] = (Boolean)SEBSettings.proxiesData[SEBSettings.KeyRTSPEnable];
*/
            // Get the "Enabled" boolean values of current "proxies" dictionary.
            // Add Proxy Protocols of currently opened file to DataGridView.
            for (int index = 0; index < NumProxyProtocols; index++)
            {
                Boolean enable = (Boolean)SEBSettings.proxiesData[KeyProxyProtocolEnable      [index]];
                String  type   = (String )                     StringProxyProtocolTableCaption[index];
                dataGridViewProxyProtocols.Rows.Add(enable, type);
                BooleanProxyProtocolEnabled[index] = enable;
            }

            // Add Bypassed Proxies of currently opened file to the comma separated list
            StringBuilder bypassedProxiesStringBuilder = new StringBuilder();
            for (int index = 0; index < SEBSettings.bypassedProxyList.Count; index++)
            {
                SEBSettings.bypassedProxyData = (String)SEBSettings.bypassedProxyList[index];
                if (bypassedProxiesStringBuilder.Length > 0)
                {
                    bypassedProxiesStringBuilder.Append(", ");
                }
                bypassedProxiesStringBuilder.Append(SEBSettings.bypassedProxyData);
            }
            textBoxBypassedProxyHostList.Text = bypassedProxiesStringBuilder.ToString();

            // Load the currently selected process data
            if (SEBSettings.permittedProcessList.Count > 0)
                 LoadAndUpdatePermittedSelectedProcessGroup(SEBSettings.permittedProcessIndex);
            else ClearPermittedSelectedProcessGroup();

            if (SEBSettings.prohibitedProcessList.Count > 0)
                 LoadAndUpdateProhibitedSelectedProcessGroup(SEBSettings.prohibitedProcessIndex);
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
            checkBoxUrlFilterRulesRegex.Checked = (Boolean)SEBSettings.settingsCurrent[SEBSettings.KeyUrlFilterRulesAsRegex];
            chkFilterEmbeddedContent.Checked = (Boolean)SEBSettings.settingsCurrent[SEBSettings.KeyUrlFilterEnableContentFilter];

            // Group "Network - Certificates"

            // Group "Network - Proxies"
            radioButtonUseSystemProxySettings.Checked =    ((int)SEBSettings.settingsCurrent[SEBSettings.KeyProxySettingsPolicy] == 0);
            radioButtonUseSebProxySettings   .Checked =    ((int)SEBSettings.settingsCurrent[SEBSettings.KeyProxySettingsPolicy] == 1);

            textBoxAutoProxyConfigurationURL .Text    =  (String)SEBSettings.proxiesData[SEBSettings.KeyAutoConfigurationURL];
            checkBoxExcludeSimpleHostnames   .Checked = (Boolean)SEBSettings.proxiesData[SEBSettings.KeyExcludeSimpleHostnames];
            checkBoxUsePassiveFTPMode        .Checked = (Boolean)SEBSettings.proxiesData[SEBSettings.KeyFTPPassive];

            // Group "Security"
             listBoxSebServicePolicy.SelectedIndex =     (int)SEBSettings.settingsCurrent[SEBSettings.KeySebServicePolicy];
            checkBoxAllowVirtualMachine.Checked    = (Boolean)SEBSettings.settingsCurrent[SEBSettings.KeyAllowVirtualMachine];
            radioCreateNewDesktop   .Checked    = (Boolean)SEBSettings.settingsCurrent[SEBSettings.KeyCreateNewDesktop];
            radioKillExplorerShell  .Checked    = (Boolean)SEBSettings.settingsCurrent[SEBSettings.KeyKillExplorerShell];
            radioNoKiosMode  .Checked    = !(Boolean)SEBSettings.settingsCurrent[SEBSettings.KeyKillExplorerShell] && !(Boolean)SEBSettings.settingsCurrent[SEBSettings.KeyCreateNewDesktop];
            checkBoxAllowUserSwitching .Checked    = (Boolean)SEBSettings.settingsCurrent[SEBSettings.KeyAllowUserSwitching];
            checkBoxEnableLogging      .Checked    = (Boolean)SEBSettings.settingsCurrent[SEBSettings.KeyEnableLogging];
            labelLogDirectoryWin.Text = (String)SEBSettings.settingsCurrent[SEBSettings.KeyLogDirectoryWin];
            if (String.IsNullOrEmpty(labelLogDirectoryWin.Text))
            {
                checkBoxUseStandardDirectory.Checked = true;
            }
            else
            {
                checkBoxUseStandardDirectory.Checked = false;
            }
            textBoxLogDirectoryOSX.Text = (String)SEBSettings.settingsCurrent[SEBSettings.KeyLogDirectoryOSX];
            checkboxAllowWlan.Checked = (Boolean)SEBSettings.settingsCurrent[SEBSettings.KeyAllowWLAN];
            checkBoxEnableScreenCapture.Checked = (Boolean)SEBSettings.settingsCurrent[SEBSettings.KeyEnablePrintScreen];

            // Group "Registry"
            checkBoxInsideSebEnableSwitchUser       .Checked = (Boolean)SEBSettings.settingsCurrent[SEBSettings.KeyInsideSebEnableSwitchUser];
            checkBoxInsideSebEnableLockThisComputer .Checked = (Boolean)SEBSettings.settingsCurrent[SEBSettings.KeyInsideSebEnableLockThisComputer];
            checkBoxInsideSebEnableChangeAPassword  .Checked = (Boolean)SEBSettings.settingsCurrent[SEBSettings.KeyInsideSebEnableChangeAPassword];
            checkBoxInsideSebEnableStartTaskManager .Checked = (Boolean)SEBSettings.settingsCurrent[SEBSettings.KeyInsideSebEnableStartTaskManager];
            checkBoxInsideSebEnableLogOff           .Checked = (Boolean)SEBSettings.settingsCurrent[SEBSettings.KeyInsideSebEnableLogOff];
            checkBoxInsideSebEnableShutDown         .Checked = (Boolean)SEBSettings.settingsCurrent[SEBSettings.KeyInsideSebEnableShutDown];
            checkBoxInsideSebEnableEaseOfAccess     .Checked = (Boolean)SEBSettings.settingsCurrent[SEBSettings.KeyInsideSebEnableEaseOfAccess];
            checkBoxInsideSebEnableVmWareClientShade.Checked = (Boolean)SEBSettings.settingsCurrent[SEBSettings.KeyInsideSebEnableVmWareClientShade];

            // Group "Hooked Keys"
            checkBoxHookKeys.Checked = (Boolean)SEBSettings.settingsCurrent[SEBSettings.KeyHookKeys];

            checkBoxEnableEsc        .Checked = (Boolean)SEBSettings.settingsCurrent[SEBSettings.KeyEnableEsc];
            checkBoxEnableCtrlEsc    .Checked = (Boolean)SEBSettings.settingsCurrent[SEBSettings.KeyEnableCtrlEsc];
            checkBoxEnableAltEsc     .Checked = (Boolean)SEBSettings.settingsCurrent[SEBSettings.KeyEnableAltEsc];
            checkBoxEnableAltTab     .Checked = (Boolean)SEBSettings.settingsCurrent[SEBSettings.KeyEnableAltTab];
            checkBoxEnableAltF4      .Checked = (Boolean)SEBSettings.settingsCurrent[SEBSettings.KeyEnableAltF4];
            checkBoxEnableRightMouse .Checked = (Boolean)SEBSettings.settingsCurrent[SEBSettings.KeyEnableRightMouse];
            checkBoxEnablePrintScreen.Checked = (Boolean)SEBSettings.settingsCurrent[SEBSettings.KeyEnablePrintScreen];
            checkBoxEnableAltMouseWheel.Checked = (Boolean)SEBSettings.settingsCurrent[SEBSettings.KeyEnableAltMouseWheel];

            checkBoxEnableF1 .Checked = (Boolean)SEBSettings.settingsCurrent[SEBSettings.KeyEnableF1];
            checkBoxEnableF2 .Checked = (Boolean)SEBSettings.settingsCurrent[SEBSettings.KeyEnableF2];
            checkBoxEnableF3 .Checked = (Boolean)SEBSettings.settingsCurrent[SEBSettings.KeyEnableF3];
            checkBoxEnableF4 .Checked = (Boolean)SEBSettings.settingsCurrent[SEBSettings.KeyEnableF4];
            checkBoxEnableF5 .Checked = (Boolean)SEBSettings.settingsCurrent[SEBSettings.KeyEnableF5];
            checkBoxEnableF6 .Checked = (Boolean)SEBSettings.settingsCurrent[SEBSettings.KeyEnableF6];
            checkBoxEnableF7 .Checked = (Boolean)SEBSettings.settingsCurrent[SEBSettings.KeyEnableF7];
            checkBoxEnableF8 .Checked = (Boolean)SEBSettings.settingsCurrent[SEBSettings.KeyEnableF8];
            checkBoxEnableF9 .Checked = (Boolean)SEBSettings.settingsCurrent[SEBSettings.KeyEnableF9];
            checkBoxEnableF10.Checked = (Boolean)SEBSettings.settingsCurrent[SEBSettings.KeyEnableF10];
            checkBoxEnableF11.Checked = (Boolean)SEBSettings.settingsCurrent[SEBSettings.KeyEnableF11];
            checkBoxEnableF12.Checked = (Boolean)SEBSettings.settingsCurrent[SEBSettings.KeyEnableF12];
            checkboxShowReloadButton.Checked = (Boolean)SEBSettings.settingsCurrent[SEBSettings.KeyShowReloadButton];

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

            if (passwordFieldsContainHash)
            {
                // If the flag is set for password fields contain a placeholder 
                // instead of the hash loaded from settings (no clear text password)
                if (password.CompareTo(confirmPassword) != 0)
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

            // Password fields contain actual passwords, not the placeholder for a hash value
            if (password.CompareTo(confirmPassword) == 0)
            {
                /// Passwords are same
                // Hide the "Please confirm password" label
                label.Visible = false;

                String newStringHashcode = "";
                if (!passwordFieldsContainHash && !String.IsNullOrEmpty(password) && settingsKey != null)
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
                // Save the new hash string into settings
                if (!passwordFieldsContainHash && settingsKey != null) SEBSettings.settingsCurrent[settingsKey] = newStringHashcode;
                // Enable the save settings button
                this.buttonSaveSettingsAs.Enabled = true;
            }
            else
            {
                this.buttonSaveSettingsAs.Enabled = false;
                label.Visible = true;
            }
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
            SEBSettings.settingsCurrent[SEBSettings.KeyStartURL] = textBoxStartURL.Text;
        }

        private void buttonPasteFromSavedClipboard_Click(object sender, EventArgs e)
        {

        }

        private void textBoxSebServerURL_TextChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsCurrent[SEBSettings.KeySebServerURL] = textBoxSebServerURL.Text;
        }

        private void textBoxAdminPassword_TextChanged(object sender, EventArgs e)
        {
            ComparePasswords(textBoxAdminPassword, textBoxConfirmAdminPassword, ref adminPasswordFieldsContainHash, labelAdminPasswordCompare, SEBSettings.KeyHashedAdminPassword);
        }

        private void textBoxConfirmAdminPassword_TextChanged(object sender, EventArgs e)
        {
            ComparePasswords(textBoxAdminPassword, textBoxConfirmAdminPassword, ref adminPasswordFieldsContainHash, labelAdminPasswordCompare, SEBSettings.KeyHashedAdminPassword);
        }

        private void checkBoxAllowQuit_CheckedChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsCurrent[SEBSettings.KeyAllowQuit] = checkBoxAllowQuit.Checked;
        }

        private void checkBoxIgnoreQuitPassword_CheckedChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsCurrent[SEBSettings.KeyIgnoreQuitPassword] = checkBoxIgnoreQuitPassword.Checked;
        }


        private void textBoxQuitPassword_TextChanged(object sender, EventArgs e)
        {
            ComparePasswords(textBoxQuitPassword, textBoxConfirmQuitPassword, ref quitPasswordFieldsContainHash, labelQuitPasswordCompare, SEBSettings.KeyHashedQuitPassword);
        }


        private void textBoxConfirmQuitPassword_TextChanged(object sender, EventArgs e)
        {
            ComparePasswords(textBoxQuitPassword, textBoxConfirmQuitPassword, ref quitPasswordFieldsContainHash, labelQuitPasswordCompare, SEBSettings.KeyHashedQuitPassword);
        }


        private void checkBoxIgnoreExitKeys_CheckedChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsCurrent[SEBSettings.KeyIgnoreExitKeys] =  checkBoxIgnoreExitKeys.Checked;
            groupBoxExitSequence.Enabled                               = !checkBoxIgnoreExitKeys.Checked;
        }

        private void listBoxExitKey1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Make sure that all three exit keys are different.
            // If selected key is already occupied, revert to previously selected key.
            if ((listBoxExitKey1.SelectedIndex == listBoxExitKey2.SelectedIndex) ||
                (listBoxExitKey1.SelectedIndex == listBoxExitKey3.SelectedIndex))
                 listBoxExitKey1.SelectedIndex =  (int)SEBSettings.settingsCurrent[SEBSettings.KeyExitKey1];
            SEBSettings.settingsCurrent[SEBSettings.KeyExitKey1] = listBoxExitKey1.SelectedIndex;
        }

        private void listBoxExitKey2_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Make sure that all three exit keys are different.
            // If selected key is already occupied, revert to previously selected key.
            if ((listBoxExitKey2.SelectedIndex == listBoxExitKey1.SelectedIndex) ||
                (listBoxExitKey2.SelectedIndex == listBoxExitKey3.SelectedIndex))
                 listBoxExitKey2.SelectedIndex =  (int)SEBSettings.settingsCurrent[SEBSettings.KeyExitKey2];
            SEBSettings.settingsCurrent[SEBSettings.KeyExitKey2] = listBoxExitKey2.SelectedIndex;
        }

        private void listBoxExitKey3_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Make sure that all three exit keys are different.
            // If selected key is already occupied, revert to previously selected key.
            if ((listBoxExitKey3.SelectedIndex == listBoxExitKey1.SelectedIndex) ||
                (listBoxExitKey3.SelectedIndex == listBoxExitKey2.SelectedIndex))
                 listBoxExitKey3.SelectedIndex =  (int)SEBSettings.settingsCurrent[SEBSettings.KeyExitKey3];
            SEBSettings.settingsCurrent[SEBSettings.KeyExitKey3] = listBoxExitKey3.SelectedIndex;
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
            if (radioButtonStartingAnExam.Checked == true)
                 SEBSettings.settingsCurrent[SEBSettings.KeySebConfigPurpose] = 0;
            else SEBSettings.settingsCurrent[SEBSettings.KeySebConfigPurpose] = 1;
        }

        private void radioButtonConfiguringAClient_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonConfiguringAClient.Checked == true)
                 SEBSettings.settingsCurrent[SEBSettings.KeySebConfigPurpose] = 1;
            else SEBSettings.settingsCurrent[SEBSettings.KeySebConfigPurpose] = 0;
        }

        private void checkBoxAllowPreferencesWindow_CheckedChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsCurrent[SEBSettings.KeyAllowPreferencesWindow] = checkBoxAllowPreferencesWindow.Checked;
        }

        private void comboBoxCryptoIdentity_SelectedIndexChanged(object sender, EventArgs e)
        {
            SEBSettings.intArrayCurrent[SEBSettings.ValCryptoIdentity] = comboBoxCryptoIdentity.SelectedIndex;
            SEBSettings.strArrayCurrent[SEBSettings.ValCryptoIdentity] = comboBoxCryptoIdentity.Text;
        }

        private void comboBoxCryptoIdentity_TextUpdate(object sender, EventArgs e)
        {
            SEBSettings.intArrayCurrent[SEBSettings.ValCryptoIdentity] = comboBoxCryptoIdentity.SelectedIndex;
            SEBSettings.strArrayCurrent[SEBSettings.ValCryptoIdentity] = comboBoxCryptoIdentity.Text;
        }

        private void textBoxSettingsPassword_TextChanged(object sender, EventArgs e)
        {
            ComparePasswords(textBoxSettingsPassword, textBoxConfirmSettingsPassword, ref settingsPasswordFieldsContainHash, labelSettingsPasswordCompare, null);
            // We can store the settings password regardless if the same is entered in the confirm text field, 
            // as saving the .seb file is only allowed when they are same
            settingsPassword = textBoxSettingsPassword.Text;
        }

        private void textBoxConfirmSettingsPassword_TextChanged(object sender, EventArgs e)
        {
            ComparePasswords(textBoxSettingsPassword, textBoxConfirmSettingsPassword, ref settingsPasswordFieldsContainHash, labelSettingsPasswordCompare, null);
            // We can store the settings password regardless if the same is entered in the confirm text field, 
            // as saving the .seb file is only allowed when they are same
            settingsPassword = textBoxSettingsPassword.Text;
        }


        private void buttonOpenSettings_Click(object sender, EventArgs e)
        {
            // Set the default directory and file name in the File Dialog
            openFileDialogSebConfigFile.InitialDirectory = currentDireSebConfigFile;
            openFileDialogSebConfigFile.FileName = currentFileSebConfigFile;
            openFileDialogSebConfigFile.DefaultExt = "seb";
            openFileDialogSebConfigFile.Filter = "SEB Files|*.seb";

            // Get the user inputs in the File Dialog
            DialogResult fileDialogResult = openFileDialogSebConfigFile.ShowDialog();
            String fileName = openFileDialogSebConfigFile.FileName;

            // If the user clicked "Cancel", do nothing
            // If the user clicked "OK"    , read the settings from the configuration file
            if (fileDialogResult.Equals(DialogResult.Cancel)) return;
            if (fileDialogResult.Equals(DialogResult.OK))
            {
                if (!LoadConfigurationFileIntoEditor(fileName)) return;
                // Generate Browser Exam Key of this new settings
                lastBrowserExamKey = SEBProtectionController.ComputeBrowserExamKey();
                // Display the new Browser Exam Key in Exam pane
                textBoxBrowserExamKey.Text = lastBrowserExamKey;
            }
        }

        private void buttonSaveSettings_Click(object sender, EventArgs e)
        {
            StringBuilder sebClientSettingsAppDataBuilder = new StringBuilder(currentDireSebConfigFile).Append(@"\").Append(currentFileSebConfigFile);
            String fileName = sebClientSettingsAppDataBuilder.ToString();

            // Generate Browser Exam Key and its salt, if settings changed
            string newBrowserExamKey = SEBProtectionController.ComputeBrowserExamKey();
            if (!lastBrowserExamKey.Equals(newBrowserExamKey))
            {
                // If the exam key changed, then settings changed and we will generate a new salt
                byte[] newExamKeySalt = SEBProtectionController.GenerateBrowserExamKeySalt();
                // Save the new salt
                SEBSettings.settingsCurrent[SEBSettings.KeyExamKeySalt] = newExamKeySalt;
                // Generate the new Browser Exam Key
                lastBrowserExamKey = SEBProtectionController.ComputeBrowserExamKey();
                // Display the new Browser Exam Key in Exam pane
                textBoxBrowserExamKey.Text = lastBrowserExamKey;
            }
            SaveConfigurationFileFromEditor(fileName);
        }


        private void buttonSaveSettingsAs_Click(object sender, EventArgs e)
        {
            // Set the default directory and file name in the File Dialog
            saveFileDialogSebConfigFile.InitialDirectory = currentDireSebConfigFile;
            saveFileDialogSebConfigFile.FileName = currentFileSebConfigFile;

            // Get the user inputs in the File Dialog
            DialogResult fileDialogResult = saveFileDialogSebConfigFile.ShowDialog();
            String fileName = saveFileDialogSebConfigFile.FileName;

            // If the user clicked "Cancel", do nothing
            // If the user clicked "OK"    , write the settings to the configuration file
            if (fileDialogResult.Equals(DialogResult.Cancel)) return;

            // Generate Browser Exam Key and its salt, if settings changed
            string newBrowserExamKey = SEBProtectionController.ComputeBrowserExamKey();
            if (!lastBrowserExamKey.Equals(newBrowserExamKey))
            {
                // If the exam key changed, then settings changed and we will generate a new salt
                byte[] newExamKeySalt = SEBProtectionController.GenerateBrowserExamKeySalt();
                // Save the new salt
                SEBSettings.settingsCurrent[SEBSettings.KeyExamKeySalt] = newExamKeySalt;
                // Generate the new Browser Exam Key
                lastBrowserExamKey = SEBProtectionController.ComputeBrowserExamKey();
                // Display the new Browser Exam Key in Exam pane
                textBoxBrowserExamKey.Text = lastBrowserExamKey;
            }
            if (fileDialogResult.Equals(DialogResult.OK)) SaveConfigurationFileFromEditor(fileName);
        }


        private void buttonRevertToDefaultSettings_Click(object sender, EventArgs e)
        {
            settingsPassword                  = "";
            settingsPasswordFieldsContainHash = false;
            SEBSettings.RestoreDefaultAndCurrentSettings();
            SEBSettings.PermitXulRunnerProcess();
            UpdateAllWidgetsOfProgram();
            // Generate Browser Exam Key of default settings
            lastBrowserExamKey = SEBProtectionController.ComputeBrowserExamKey();
            // Display the new Browser Exam Key in Exam pane
            textBoxBrowserExamKey.Text = lastBrowserExamKey;
        }

        private void buttonRevertToLocalClientSettings_Click(object sender, EventArgs e)
        {
            // Get the path to the local client settings configuration file
            currentDireSebConfigFile = SEBClientInfo.SebClientSettingsLocalAppDirectory;
            currentFileSebConfigFile = SEBClientInfo.SEB_CLIENT_CONFIG;
            StringBuilder sebClientSettingsAppDataBuilder = new StringBuilder(currentDireSebConfigFile).Append(currentFileSebConfigFile);
            currentPathSebConfigFile = sebClientSettingsAppDataBuilder.ToString();

            if (!LoadConfigurationFileIntoEditor(currentPathSebConfigFile))
            {
                settingsPassword = "";
                settingsPasswordFieldsContainHash = false;
                SEBSettings.RestoreDefaultAndCurrentSettings();
                SEBSettings.PermitXulRunnerProcess();
                currentPathSebConfigFile = SEBUIStrings.settingsTitleDefaultSettings;
                UpdateAllWidgetsOfProgram();
            }
            // Generate Browser Exam Key of this new settings
            lastBrowserExamKey = SEBProtectionController.ComputeBrowserExamKey();
            // Display the new Browser Exam Key in Exam pane
            textBoxBrowserExamKey.Text = lastBrowserExamKey;
        }


        private void buttonRevertToLastOpened_Click(object sender, EventArgs e)
        {
            if (!LoadConfigurationFileIntoEditor(currentPathSebConfigFile)) return;
            // Generate Browser Exam Key of this new settings
            lastBrowserExamKey = SEBProtectionController.ComputeBrowserExamKey();
            // Display the new Browser Exam Key in Exam pane
            textBoxBrowserExamKey.Text = lastBrowserExamKey;
        }


        private void buttonEditDuplicate_Click(object sender, EventArgs e)
        {
            // Add string " copy" (or " n+1" if the filename already ends with " copy" or " copy n")
            // to the config name filename
            // Get the current config file full path
            //NSURL *currentConfigFilePath = [[MyGlobals sharedMyGlobals] currentConfigURL];
            //// Get the filename without extension
            //NSString *filename = currentConfigFilePath.lastPathComponent.stringByDeletingPathExtension;
            //// Get the extension (should be .seb)
            //NSString *extension = currentConfigFilePath.pathExtension;
            //if (filename.length == 0) {
            //    filename = NSLocalizedString(@"untitled", @"untitled filename");
            //    extension = @".seb";
            //} else {
            //    NSRange copyStringRange = [filename rangeOfString:NSLocalizedString(@" copy", @"word indicating the duplicate of a file, same as in Finder ' copy'") options:NSBackwardsSearch];
            //    if (copyStringRange.location == NSNotFound) {
            //        filename = [filename stringByAppendingString:NSLocalizedString(@" copy", nil)];
            //    } else {
            //        NSString *copyNumberString = [filename substringFromIndex:copyStringRange.location+copyStringRange.length];
            //        if (copyNumberString.length == 0) {
            //            filename = [filename stringByAppendingString:NSLocalizedString(@" 1", nil)];
            //        } else {
            //            NSInteger copyNumber = [[copyNumberString substringFromIndex:1] integerValue];
            //            if (copyNumber == 0) {
            //                filename = [filename stringByAppendingString:NSLocalizedString(@" copy", nil)];
            //            } else {
            //                filename = [[filename substringToIndex:copyStringRange.location+copyStringRange.length+1] stringByAppendingString:[NSString stringWithFormat:@"%ld", copyNumber+1]];
            //            }
            //        }
            //    }
            //}

        }


        private void buttonConfigureClient_Click(object sender, EventArgs e)
        {
            // Get the path to the local client settings configuration file
            currentDireSebConfigFile = SEBClientInfo.SebClientSettingsLocalAppDirectory;
            currentFileSebConfigFile = SEBClientInfo.SEB_CLIENT_CONFIG;
            StringBuilder sebClientSettingsAppDataBuilder = new StringBuilder(currentDireSebConfigFile).Append(currentFileSebConfigFile);
            string filename = sebClientSettingsAppDataBuilder.ToString();

            // Generate Browser Exam Key and its salt, if settings changed
            string newBrowserExamKey = SEBProtectionController.ComputeBrowserExamKey();
            if (!lastBrowserExamKey.Equals(newBrowserExamKey))
            {
                // If the exam key changed, then settings changed and we will generate a new salt
                byte[] newExamKeySalt = SEBProtectionController.GenerateBrowserExamKeySalt();
                // Save the new salt
                SEBSettings.settingsCurrent[SEBSettings.KeyExamKeySalt] = newExamKeySalt;
                // Generate the new Browser Exam Key
                lastBrowserExamKey = SEBProtectionController.ComputeBrowserExamKey();
                // Display the new Browser Exam Key in Exam pane
                textBoxBrowserExamKey.Text = lastBrowserExamKey;
            }
            SaveConfigurationFileFromEditor(filename);
        }


        private void buttonApplyAndStartSEB_Click(object sender, EventArgs e)
        {
            buttonSaveSettings_Click(null, null);

            // Get the path to the local client settings configuration file
            currentDireSebConfigFile = SEBClientInfo.SebClientSettingsLocalAppDirectory;
            currentFileSebConfigFile = SEBClientInfo.SEB_CLIENT_CONFIG;
            StringBuilder sebClientSettingsAppDataBuilder = new StringBuilder(currentDireSebConfigFile).Append(currentFileSebConfigFile);
            string localSebClientSettings = sebClientSettingsAppDataBuilder.ToString();

            StringBuilder sebClientExeBuilder = new StringBuilder(SEBClientInfo.SebClientDirectory).Append(SEBClientInfo.PRODUCT_NAME).Append(".exe");
            string sebClientExe = sebClientExeBuilder.ToString();

            var p = new Process();
            p.StartInfo.FileName = sebClientExe;

            if (!currentPathSebConfigFile.Equals(localSebClientSettings))
            {
                p.StartInfo.Arguments = currentPathSebConfigFile;
            }

            p.Start();

            Application.Exit();
        }


        // ******************
        // Group "Appearance"
        // ******************
        private void radioButtonUseBrowserWindow_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonUseBrowserWindow.Checked == true)
            {
                groupBoxMainBrowserWindow.Enabled = true;
                SEBSettings.settingsCurrent[SEBSettings.KeyBrowserViewMode] = 0;
                SEBSettings.settingsCurrent[SEBSettings.KeyTouchOptimized] = 0;
                SEBSettings.settingsCurrent[SEBSettings.KeyBrowserScreenKeyboard] = 0;
            }
        }

        private void radioButtonUseFullScreenMode_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonUseFullScreenMode.Checked == true)
            {
                groupBoxMainBrowserWindow.Enabled = false;
                SEBSettings.settingsCurrent[SEBSettings.KeyBrowserViewMode] = 1;
                SEBSettings.settingsCurrent[SEBSettings.KeyTouchOptimized] = 0;
                SEBSettings.settingsCurrent[SEBSettings.KeyBrowserScreenKeyboard] = 0;
            }
        }

        private void radioButtonTouchOptimized_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonTouchOptimized.Checked == true)
            {
                if ((Boolean) SEBSettings.settingsCurrent[SEBSettings.KeyCreateNewDesktop])
                {
                    MessageBox.Show(
                    "Touch optimization will not work when kiosk mode is set to Create New Desktop, please change the appearance.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                groupBoxMainBrowserWindow.Enabled = false;
                SEBSettings.settingsCurrent[SEBSettings.KeyBrowserViewMode] = 1;
                SEBSettings.settingsCurrent[SEBSettings.KeyNewBrowserWindowByLinkWidth] = "100%";
                SEBSettings.settingsCurrent[SEBSettings.KeyNewBrowserWindowByLinkHeight] = "100%";
                SEBSettings.settingsCurrent[SEBSettings.KeyBrowserViewMode] = 1;
                SEBSettings.settingsCurrent[SEBSettings.KeyTouchOptimized] = 1;
                SEBSettings.settingsCurrent[SEBSettings.KeyBrowserScreenKeyboard] = 1;
            }
        }

        private void comboBoxMainBrowserWindowWidth_SelectedIndexChanged(object sender, EventArgs e)
        {
            SEBSettings.intArrayCurrent[SEBSettings.ValMainBrowserWindowWidth] = comboBoxMainBrowserWindowWidth.SelectedIndex;
            SEBSettings.strArrayCurrent[SEBSettings.ValMainBrowserWindowWidth] = comboBoxMainBrowserWindowWidth.Text;
          //SEBSettings.settingsCurrent[SEBSettings.KeyMainBrowserWindowWidth] = comboBoxMainBrowserWindowWidth.SelectedIndex;
            SEBSettings.settingsCurrent[SEBSettings.KeyMainBrowserWindowWidth] = comboBoxMainBrowserWindowWidth.Text;
        }

        private void comboBoxMainBrowserWindowWidth_TextUpdate(object sender, EventArgs e)
        {
            SEBSettings.intArrayCurrent[SEBSettings.ValMainBrowserWindowWidth] = comboBoxMainBrowserWindowWidth.SelectedIndex;
            SEBSettings.strArrayCurrent[SEBSettings.ValMainBrowserWindowWidth] = comboBoxMainBrowserWindowWidth.Text;
          //SEBSettings.settingsCurrent[SEBSettings.KeyMainBrowserWindowWidth] = comboBoxMainBrowserWindowWidth.SelectedIndex;
            SEBSettings.settingsCurrent[SEBSettings.KeyMainBrowserWindowWidth] = comboBoxMainBrowserWindowWidth.Text;
        }

        private void comboBoxMainBrowserWindowHeight_SelectedIndexChanged(object sender, EventArgs e)
        {
            SEBSettings.intArrayCurrent[SEBSettings.ValMainBrowserWindowHeight] = comboBoxMainBrowserWindowHeight.SelectedIndex;
            SEBSettings.strArrayCurrent[SEBSettings.ValMainBrowserWindowHeight] = comboBoxMainBrowserWindowHeight.Text;
          //SEBSettings.settingsCurrent[SEBSettings.KeyMainBrowserWindowHeight] = comboBoxMainBrowserWindowHeight.SelectedIndex;
            SEBSettings.settingsCurrent[SEBSettings.KeyMainBrowserWindowHeight] = comboBoxMainBrowserWindowHeight.Text;
        }

        private void comboBoxMainBrowserWindowHeight_TextUpdate(object sender, EventArgs e)
        {
            SEBSettings.intArrayCurrent[SEBSettings.ValMainBrowserWindowHeight] = comboBoxMainBrowserWindowHeight.SelectedIndex;
            SEBSettings.strArrayCurrent[SEBSettings.ValMainBrowserWindowHeight] = comboBoxMainBrowserWindowHeight.Text;
          //SEBSettings.settingsCurrent[SEBSettings.KeyMainBrowserWindowHeight] = comboBoxMainBrowserWindowHeight.SelectedIndex;
            SEBSettings.settingsCurrent[SEBSettings.KeyMainBrowserWindowHeight] = comboBoxMainBrowserWindowHeight.Text;
        }

        private void listBoxMainBrowserWindowPositioning_SelectedIndexChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsCurrent[SEBSettings.KeyMainBrowserWindowPositioning] = listBoxMainBrowserWindowPositioning.SelectedIndex;
        }

        private void checkBoxEnableBrowserWindowToolbar_CheckedChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsCurrent[SEBSettings.KeyEnableBrowserWindowToolbar] = checkBoxEnableBrowserWindowToolbar.Checked;
            checkBoxHideBrowserWindowToolbar.Enabled                               = checkBoxEnableBrowserWindowToolbar.Checked;
        }

        private void checkBoxHideBrowserWindowToolbar_CheckedChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsCurrent[SEBSettings.KeyHideBrowserWindowToolbar] = checkBoxHideBrowserWindowToolbar.Checked;
        }

        private void checkBoxShowMenuBar_CheckedChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsCurrent[SEBSettings.KeyShowMenuBar] = checkBoxShowMenuBar.Checked;
        }

        private void checkBoxShowTaskBar_CheckedChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsCurrent[SEBSettings.KeyShowTaskBar] = checkBoxShowTaskBar.Checked;
            comboBoxTaskBarHeight.Enabled                           = checkBoxShowTaskBar.Checked;
        }

        private void comboBoxTaskBarHeight_SelectedIndexChanged(object sender, EventArgs e)
        {
            SEBSettings.intArrayCurrent[SEBSettings.ValTaskBarHeight] = comboBoxTaskBarHeight.SelectedIndex;
            SEBSettings.strArrayCurrent[SEBSettings.ValTaskBarHeight] = comboBoxTaskBarHeight.Text;
          //SEBSettings.settingsCurrent[SEBSettings.KeyTaskBarHeight] = comboBoxTaskBarHeight.SelectedIndex;
            SEBSettings.settingsCurrent[SEBSettings.KeyTaskBarHeight] = Int32.Parse(comboBoxTaskBarHeight.Text);
        }



        // ***************
        // Group "Browser"
        // ***************
        private void listBoxOpenLinksHTML_SelectedIndexChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsCurrent[SEBSettings.KeyNewBrowserWindowByLinkPolicy] = listBoxOpenLinksHTML.SelectedIndex;
        }

        private void listBoxOpenLinksJava_SelectedIndexChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsCurrent[SEBSettings.KeyNewBrowserWindowByScriptPolicy] = listBoxOpenLinksJava.SelectedIndex;
        }

        private void checkBoxBlockLinksHTML_CheckedChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsCurrent[SEBSettings.KeyNewBrowserWindowByLinkBlockForeign] = checkBoxBlockLinksHTML.Checked;
        }

        private void checkBoxBlockLinksJava_CheckedChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsCurrent[SEBSettings.KeyNewBrowserWindowByScriptBlockForeign] = checkBoxBlockLinksJava.Checked;
        }

        private void comboBoxNewBrowserWindowWidth_SelectedIndexChanged(object sender, EventArgs e)
        {
            SEBSettings.intArrayCurrent[SEBSettings.ValNewBrowserWindowByLinkWidth] = comboBoxNewBrowserWindowWidth.SelectedIndex;
            SEBSettings.strArrayCurrent[SEBSettings.ValNewBrowserWindowByLinkWidth] = comboBoxNewBrowserWindowWidth.Text;
          //SEBSettings.settingsCurrent[SEBSettings.KeyNewBrowserWindowByLinkWidth] = comboBoxNewBrowserWindowWidth.SelectedIndex;
            SEBSettings.settingsCurrent[SEBSettings.KeyNewBrowserWindowByLinkWidth] = comboBoxNewBrowserWindowWidth.Text;
        }

        private void comboBoxNewBrowserWindowWidth_TextUpdate(object sender, EventArgs e)
        {
            SEBSettings.intArrayCurrent[SEBSettings.ValNewBrowserWindowByLinkWidth] = comboBoxNewBrowserWindowWidth.SelectedIndex;
            SEBSettings.strArrayCurrent[SEBSettings.ValNewBrowserWindowByLinkWidth] = comboBoxNewBrowserWindowWidth.Text;
          //SEBSettings.settingsCurrent[SEBSettings.KeyNewBrowserWindowByLinkWidth] = comboBoxNewBrowserWindowWidth.SelectedIndex;
            SEBSettings.settingsCurrent[SEBSettings.KeyNewBrowserWindowByLinkWidth] = comboBoxNewBrowserWindowWidth.Text;
        }

        private void comboBoxNewBrowserWindowHeight_SelectedIndexChanged(object sender, EventArgs e)
        {
            SEBSettings.intArrayCurrent[SEBSettings.ValNewBrowserWindowByLinkHeight] = comboBoxNewBrowserWindowHeight.SelectedIndex;
            SEBSettings.strArrayCurrent[SEBSettings.ValNewBrowserWindowByLinkHeight] = comboBoxNewBrowserWindowHeight.Text;
          //SEBSettings.settingsCurrent[SEBSettings.KeyNewBrowserWindowByLinkHeight] = comboBoxNewBrowserWindowHeight.SelectedIndex;
            SEBSettings.settingsCurrent[SEBSettings.KeyNewBrowserWindowByLinkHeight] = comboBoxNewBrowserWindowHeight.Text;
        }

        private void comboBoxNewBrowserWindowHeight_TextUpdate(object sender, EventArgs e)
        {
            SEBSettings.intArrayCurrent[SEBSettings.ValNewBrowserWindowByLinkHeight] = comboBoxNewBrowserWindowHeight.SelectedIndex;
            SEBSettings.strArrayCurrent[SEBSettings.ValNewBrowserWindowByLinkHeight] = comboBoxNewBrowserWindowHeight.Text;
          //SEBSettings.settingsCurrent[SEBSettings.KeyNewBrowserWindowByLinkHeight] = comboBoxNewBrowserWindowHeight.SelectedIndex;
            SEBSettings.settingsCurrent[SEBSettings.KeyNewBrowserWindowByLinkHeight] = comboBoxNewBrowserWindowHeight.Text;
        }

        private void listBoxNewBrowserWindowPositioning_SelectedIndexChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsCurrent[SEBSettings.KeyNewBrowserWindowByLinkPositioning] = listBoxNewBrowserWindowPositioning.SelectedIndex;
        }

        private void checkBoxEnablePlugins_CheckedChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsCurrent[SEBSettings.KeyEnablePlugIns] = checkBoxEnablePlugIns.Checked;
        }

        private void checkBoxEnableJava_CheckedChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsCurrent[SEBSettings.KeyEnableJava] = checkBoxEnableJava.Checked;
        }

        private void checkBoxEnableJavaScript_CheckedChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsCurrent[SEBSettings.KeyEnableJavaScript] = checkBoxEnableJavaScript.Checked;
        }

        private void checkBoxBlockPopUpWindows_CheckedChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsCurrent[SEBSettings.KeyBlockPopUpWindows] = checkBoxBlockPopUpWindows.Checked;
        }

        private void checkBoxAllowBrowsingBackForward_CheckedChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsCurrent[SEBSettings.KeyAllowBrowsingBackForward] = checkBoxAllowBrowsingBackForward.Checked;
        }

        private void checkBoxRemoveProfile_CheckedChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsCurrent[SEBSettings.KeyRemoveBrowserProfile] = checkBoxRemoveProfile.Checked;
        }

        // BEWARE: you must invert this value since "Use Without" is "Not Enable"!
        private void checkBoxUseSebWithoutBrowser_CheckedChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsCurrent[SEBSettings.KeyEnableSebBrowser] = !(checkBoxUseSebWithoutBrowser.Checked);
        }



        // ********************
        // Group "Down/Uploads"
        // ********************
        private void checkBoxAllowDownUploads_CheckedChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsCurrent[SEBSettings.KeyAllowDownUploads] = checkBoxAllowDownUploads.Checked;
        }

        private void buttonDownloadDirectoryWin_Click(object sender, EventArgs e)
        {
            // Set the default directory in the Folder Browser Dialog
            folderBrowserDialogDownloadDirectoryWin.RootFolder = Environment.SpecialFolder.DesktopDirectory;
//          folderBrowserDialogDownloadDirectoryWin.RootFolder = Environment.CurrentDirectory;

            // Get the user inputs in the File Dialog
            DialogResult dialogResult = folderBrowserDialogDownloadDirectoryWin.ShowDialog();
            String               path = folderBrowserDialogDownloadDirectoryWin.SelectedPath;

            // If the user clicked "Cancel", do nothing
            if (dialogResult.Equals(DialogResult.Cancel)) return;

            // If the user clicked "OK", ...
            string pathUsingEnvironmentVariables = SEBClientInfo.ContractEnvironmentVariables(path);
            SEBSettings.settingsCurrent[SEBSettings.KeyDownloadDirectoryWin]     = pathUsingEnvironmentVariables;
                                                  labelDownloadDirectoryWin.Text = pathUsingEnvironmentVariables;
        }

        private void checkBoxOpenDownloads_CheckedChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsCurrent[SEBSettings.KeyOpenDownloads] = checkBoxOpenDownloads.Checked;
        }

        private void listBoxChooseFileToUploadPolicy_SelectedIndexChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsCurrent[SEBSettings.KeyChooseFileToUploadPolicy] = listBoxChooseFileToUploadPolicy.SelectedIndex;
        }

        private void checkBoxDownloadPDFFiles_CheckedChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsCurrent[SEBSettings.KeyDownloadPDFFiles] = checkBoxDownloadPDFFiles.Checked;
        }



        // ************
        // Group "Exam"
        // ************
        private void buttonGenerateBrowserExamKey_Click(object sender, EventArgs e)
        {
            textBoxBrowserExamKey.Text = SEBProtectionController.ComputeBrowserExamKey();
        }

        private void textBoxBrowserExamKey_TextChanged(object sender, EventArgs e)
        {
          //SEBSettings.settingsCurrent[SEBSettings.KeyBrowserExamKey] = textBoxBrowserExamKey.Text;
        }

        private void checkBoxSendBrowserExamKey_CheckedChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsCurrent[SEBSettings.KeySendBrowserExamKey] = checkBoxSendBrowserExamKey.Checked;
        }

        private void textBoxQuitURL_TextChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsCurrent[SEBSettings.KeyQuitURL] = textBoxQuitURL.Text;
        }

        private void textBoxRestartExamLink_TextChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsCurrent[SEBSettings.KeyRestartExamURL] = textBoxRestartExamLink.Text;
        }

        private void textBoxRestartExamText_TextChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsCurrent[SEBSettings.KeyRestartExamText] = textBoxRestartExamText.Text;
        }

        private void checkBoxRestartExamPasswordProtected_CheckedChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsCurrent[SEBSettings.KeyRestartExamPasswordProtected] = checkBoxRestartExamPasswordProtected.Checked;
        }



        // ********************
        // Group "Applications"
        // ********************
        private void checkBoxMonitorProcesses_CheckedChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsCurrent[SEBSettings.KeyMonitorProcesses] = checkBoxMonitorProcesses.Checked;
        }


        // ******************************************
        // Group "Applications - Permitted Processes"
        // ******************************************
        private void checkBoxAllowSwitchToApplications_CheckedChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsCurrent[SEBSettings.KeyAllowSwitchToApplications] = checkBoxAllowSwitchToApplications.Checked;
            checkBoxAllowFlashFullscreen.Enabled                                  = checkBoxAllowSwitchToApplications.Checked;
        }

        private void checkBoxAllowFlashFullscreen_CheckedChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsCurrent[SEBSettings.KeyAllowFlashFullscreen] = checkBoxAllowFlashFullscreen.Checked;
        }


        private void LoadAndUpdatePermittedSelectedProcessGroup(int selectedProcessIndex)
        {
            // Get the process data of the selected process
            SEBSettings.permittedProcessList  = (ListObj)SEBSettings.settingsCurrent     [SEBSettings.KeyPermittedProcesses];
            SEBSettings.permittedProcessData  = (DictObj)SEBSettings.permittedProcessList[selectedProcessIndex];
            SEBSettings.permittedArgumentList = (ListObj)SEBSettings.permittedProcessData[SEBSettings.KeyArguments];

            // Beware double events:
            // Update the widgets in "Selected Process" group,
            // but prevent the following "widget changed" event from firing the "cell changed" event once more!
            ignoreWidgetEventPermittedProcessesActive     = true;
            ignoreWidgetEventPermittedProcessesOS         = true;
            ignoreWidgetEventPermittedProcessesExecutable = true;
            ignoreWidgetEventPermittedProcessesTitle      = true;

            // Update the widgets in the "Selected Process" group
            checkBoxPermittedProcessActive    .Checked = (Boolean)SEBSettings.permittedProcessData[SEBSettings.KeyActive];
            checkBoxPermittedProcessAutostart .Checked = (Boolean)SEBSettings.permittedProcessData[SEBSettings.KeyAutostart];
            checkBoxPermittedProcessAutohide  .Checked = (Boolean)SEBSettings.permittedProcessData[SEBSettings.KeyRunInBackground];
            checkBoxPermittedProcessAllowUser .Checked = (Boolean)SEBSettings.permittedProcessData[SEBSettings.KeyAllowUser];
            checkBoxPermittedProcessStrongKill.Checked = (Boolean)SEBSettings.permittedProcessData[SEBSettings.KeyStrongKill];
             listBoxPermittedProcessOS.SelectedIndex   =   (Int32)SEBSettings.permittedProcessData[SEBSettings.KeyOS];
             textBoxPermittedProcessTitle      .Text   =  (String)SEBSettings.permittedProcessData[SEBSettings.KeyTitle];
             textBoxPermittedProcessDescription.Text   =  (String)SEBSettings.permittedProcessData[SEBSettings.KeyDescription];
             textBoxPermittedProcessExecutable .Text   =  (String)SEBSettings.permittedProcessData[SEBSettings.KeyExecutable];
             textBoxPermittedProcessExecutables .Text   =  (String)SEBSettings.permittedProcessData[SEBSettings.KeyWindowHandlingProcess];
             textBoxPermittedProcessPath       .Text   =  (String)SEBSettings.permittedProcessData[SEBSettings.KeyPath];
             textBoxPermittedProcessIdentifier .Text   =  (String)SEBSettings.permittedProcessData[SEBSettings.KeyIdentifier];

            // Reset the ignore widget event flags
            ignoreWidgetEventPermittedProcessesActive     = false;
            ignoreWidgetEventPermittedProcessesOS         = false;
            ignoreWidgetEventPermittedProcessesExecutable = false;
            ignoreWidgetEventPermittedProcessesTitle      = false;

             // Check if selected process has any arguments
            if  (SEBSettings.permittedArgumentList.Count > 0)
                 SEBSettings.permittedArgumentIndex =  0;
            else SEBSettings.permittedArgumentIndex = -1;

            // Remove all previously displayed arguments from DataGridView
            dataGridViewPermittedProcessArguments.Enabled = (SEBSettings.permittedArgumentList.Count > 0);
            dataGridViewPermittedProcessArguments.Rows.Clear();

            // Add arguments of selected process to DataGridView
            for (int index = 0; index < SEBSettings.permittedArgumentList.Count; index++)
            {
                SEBSettings.permittedArgumentData = (DictObj)SEBSettings.permittedArgumentList[index];
                Boolean     active                = (Boolean)SEBSettings.permittedArgumentData[SEBSettings.KeyActive];
                String      argument              = (String )SEBSettings.permittedArgumentData[SEBSettings.KeyArgument];
                dataGridViewPermittedProcessArguments.Rows.Add(active, argument);
            }

            // Get the selected argument data
            if  (SEBSettings.permittedArgumentList.Count > 0)
                 SEBSettings.permittedArgumentData = (DictObj)SEBSettings.permittedArgumentList[SEBSettings.permittedArgumentIndex];
        }


        private void ClearPermittedSelectedProcessGroup()
        {
            // Beware double events:
            // Update the widgets in "Selected Process" group,
            // but prevent the following "widget changed" event from firing the "cell changed" event once more!
            ignoreWidgetEventPermittedProcessesActive     = true;
            ignoreWidgetEventPermittedProcessesOS         = true;
            ignoreWidgetEventPermittedProcessesExecutable = true;
            ignoreWidgetEventPermittedProcessesTitle      = true;

            // Clear the widgets in the "Selected Process" group
            checkBoxPermittedProcessActive    .Checked = true;
            checkBoxPermittedProcessAutostart .Checked = true;
            checkBoxPermittedProcessAutohide  .Checked = true;
            checkBoxPermittedProcessAllowUser .Checked = true;
            checkBoxPermittedProcessStrongKill.Checked = false;
             listBoxPermittedProcessOS.SelectedIndex   = IntWin;
             textBoxPermittedProcessTitle      .Text   = "";
             textBoxPermittedProcessDescription.Text   = "";
             textBoxPermittedProcessExecutable .Text   = "";
             textBoxPermittedProcessExecutables .Text   = "";
             textBoxPermittedProcessPath       .Text   = "";
             textBoxPermittedProcessIdentifier .Text   = "";

            // Reset the ignore widget event flags
            ignoreWidgetEventPermittedProcessesActive     = false;
            ignoreWidgetEventPermittedProcessesOS         = false;
            ignoreWidgetEventPermittedProcessesExecutable = false;
            ignoreWidgetEventPermittedProcessesTitle      = false;

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

            if (dataGridViewPermittedProcesses.SelectedRows.Count != 1) return;
            SEBSettings.permittedProcessIndex = dataGridViewPermittedProcesses.SelectedRows[0].Index;

            // The process list should contain at least one element here:
            // SEBSettings.permittedProcessList.Count >  0
            // SEBSettings.permittedProcessIndex      >= 0
            LoadAndUpdatePermittedSelectedProcessGroup(SEBSettings.permittedProcessIndex);
        }


        private void dataGridViewPermittedProcesses_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            // When a CheckBox/ListBox/TextBox entry of a DataGridView table cell is edited,
            // immediately call the CellValueChanged() event,
            // which will update the SelectedProcess data and widgets.
            if (dataGridViewPermittedProcesses.IsCurrentCellDirty)
                dataGridViewPermittedProcesses.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }


        private void dataGridViewPermittedProcesses_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            // Prevent double events from switching to false process index
            if (ignoreCellEventPermittedProcessesActive     == true) return;
            if (ignoreCellEventPermittedProcessesOS         == true) return;
            if (ignoreCellEventPermittedProcessesExecutable == true) return;
            if (ignoreCellEventPermittedProcessesTitle      == true) return;

            // Get the current cell where the user has changed a value
            int row    = dataGridViewPermittedProcesses.CurrentCellAddress.Y;
            int column = dataGridViewPermittedProcesses.CurrentCellAddress.X;

            // At the beginning, row = -1 and column = -1, so skip this event
            if (row    < 0) return;
            if (column < 0) return;

            // Get the changed value of the current cell
            object value = dataGridViewPermittedProcesses.CurrentCell.EditedFormattedValue;

            // Convert the selected "OS" ListBox entry from String to Integer
            if (column == IntColumnProcessOS)
            {
                     if ((String)value == StringOSX) value = IntOSX;
                else if ((String)value == StringWin) value = IntWin;
            }

            // Get the process data of the process belonging to the current row
            SEBSettings.permittedProcessIndex = row;
            SEBSettings.permittedProcessList  = (ListObj)SEBSettings.settingsCurrent     [SEBSettings.KeyPermittedProcesses];
            SEBSettings.permittedProcessData  = (DictObj)SEBSettings.permittedProcessList[SEBSettings.permittedProcessIndex];

            // Update the process data belonging to the current cell
            if (column == IntColumnProcessActive    ) SEBSettings.permittedProcessData[SEBSettings.KeyActive    ] = (Boolean)value;
            if (column == IntColumnProcessOS        ) SEBSettings.permittedProcessData[SEBSettings.KeyOS        ] = (Int32  )value;
            if (column == IntColumnProcessExecutable) SEBSettings.permittedProcessData[SEBSettings.KeyExecutable] = (String )value;
            if (column == IntColumnProcessTitle     ) SEBSettings.permittedProcessData[SEBSettings.KeyTitle     ] = (String )value;

            // Beware double events:
            // when a cell is being edited by the user, update its corresponding widget in "Selected Process" group,
            // but prevent the following "widget changed" event from firing the "cell changed" event once more!
            if (column == IntColumnProcessActive    ) ignoreWidgetEventPermittedProcessesActive     = true;
            if (column == IntColumnProcessOS        ) ignoreWidgetEventPermittedProcessesOS         = true;
            if (column == IntColumnProcessExecutable) ignoreWidgetEventPermittedProcessesExecutable = true;
            if (column == IntColumnProcessTitle     ) ignoreWidgetEventPermittedProcessesTitle      = true;

            // In "Selected Process" group: update the widget belonging to the current cell
            // (this will fire the corresponding "widget changed" event).
            if (column == IntColumnProcessActive    ) checkBoxPermittedProcessActive.Checked   = (Boolean)value;
            if (column == IntColumnProcessOS        )  listBoxPermittedProcessOS.SelectedIndex = (Int32  )value;
            if (column == IntColumnProcessExecutable)  textBoxPermittedProcessExecutable.Text  = (String )value;
            if (column == IntColumnProcessTitle     )  textBoxPermittedProcessTitle     .Text  = (String )value;

            // Reset the ignore widget event flags
            if (column == IntColumnProcessActive    ) ignoreWidgetEventPermittedProcessesActive     = false;
            if (column == IntColumnProcessOS        ) ignoreWidgetEventPermittedProcessesOS         = false;
            if (column == IntColumnProcessExecutable) ignoreWidgetEventPermittedProcessesExecutable = false;
            if (column == IntColumnProcessTitle     ) ignoreWidgetEventPermittedProcessesTitle      = false;
        }


        private void buttonAddPermittedProcess_Click(object sender, EventArgs e)
        {
            // Get the process list
            SEBSettings.permittedProcessList = (ListObj)SEBSettings.settingsCurrent[SEBSettings.KeyPermittedProcesses];

            if (SEBSettings.permittedProcessList.Count > 0)
            {
                if (dataGridViewPermittedProcesses.SelectedRows.Count != 1) return;
              //SEBSettings.permittedProcessIndex = dataGridViewPermittedProcesses.SelectedRows[0].Index;
                SEBSettings.permittedProcessIndex = SEBSettings.permittedProcessList.Count;
            }
            else
            {
                // If process list was empty before, enable it
                SEBSettings.permittedProcessIndex      = 0;
                dataGridViewPermittedProcesses.Enabled = true;
                    groupBoxPermittedProcess  .Enabled = true;
            }

            // Create new process dataset containing default values
            DictObj processData = new DictObj();

            processData[SEBSettings.KeyActive     ] = true;
            processData[SEBSettings.KeyAutostart  ] = false;
            processData[SEBSettings.KeyRunInBackground   ] = false;
            processData[SEBSettings.KeyAllowUser  ] = false;
            processData[SEBSettings.KeyStrongKill ] = false;
            processData[SEBSettings.KeyOS         ] = IntWin;
            processData[SEBSettings.KeyTitle      ] = "";
            processData[SEBSettings.KeyDescription] = "";
            processData[SEBSettings.KeyExecutable ] = "";
            processData[SEBSettings.KeyWindowHandlingProcess ] = "";
            processData[SEBSettings.KeyPath       ] = "";
            processData[SEBSettings.KeyIdentifier ] = "";
            processData[SEBSettings.KeyArguments  ] = new ListObj();

            // Insert new process into process list at position index
            SEBSettings.permittedProcessList   .Insert(SEBSettings.permittedProcessIndex, processData);
            dataGridViewPermittedProcesses.Rows.Insert(SEBSettings.permittedProcessIndex, true, StringOS[IntWin], "", "");
            dataGridViewPermittedProcesses.Rows       [SEBSettings.permittedProcessIndex].Selected = true;
        }


        private void buttonRemovePermittedProcess_Click(object sender, EventArgs e)
        {
            if (dataGridViewPermittedProcesses.SelectedRows.Count != 1) return;

            // Clear the widgets in the "Selected Process" group
            ClearPermittedSelectedProcessGroup();

            // Delete process from process list at position index
            SEBSettings.permittedProcessIndex = dataGridViewPermittedProcesses.SelectedRows[0].Index;
            SEBSettings.permittedProcessList  = (ListObj)SEBSettings.settingsCurrent[SEBSettings.KeyPermittedProcesses];
            SEBSettings.permittedProcessList   .RemoveAt(SEBSettings.permittedProcessIndex);
            dataGridViewPermittedProcesses.Rows.RemoveAt(SEBSettings.permittedProcessIndex);

            if (SEBSettings.permittedProcessIndex == SEBSettings.permittedProcessList.Count)
                SEBSettings.permittedProcessIndex--;

            if (SEBSettings.permittedProcessList.Count > 0)
            {
                dataGridViewPermittedProcesses.Rows[SEBSettings.permittedProcessIndex].Selected = true;
            }
            else
            {
                // If process list is now empty, disable it
                SEBSettings.permittedProcessIndex      = -1;
                dataGridViewPermittedProcesses.Enabled = false;
                    groupBoxPermittedProcess  .Enabled = false;
            }
        }


        private void buttonChoosePermittedApplication_Click(object sender, EventArgs e)
        {

        }

        private void buttonChoosePermittedProcess_Click(object sender, EventArgs e)
        {

        }


        private void checkBoxPermittedProcessActive_CheckedChanged(object sender, EventArgs e)
        {
            // Prevent double events from switching to false process index
            if (ignoreWidgetEventPermittedProcessesActive == true) return;
            if (     SEBSettings.permittedProcessIndex     <    0) return;
            SEBSettings.permittedProcessList = (ListObj)SEBSettings.settingsCurrent     [SEBSettings.KeyPermittedProcesses];
            SEBSettings.permittedProcessData = (DictObj)SEBSettings.permittedProcessList[SEBSettings.permittedProcessIndex];
            SEBSettings.permittedProcessData[SEBSettings.KeyActive] = checkBoxPermittedProcessActive.Checked;
            Boolean                                         active  = checkBoxPermittedProcessActive.Checked;
            ignoreCellEventPermittedProcessesActive = true;
            dataGridViewPermittedProcesses.Rows[SEBSettings.permittedProcessIndex].Cells[IntColumnProcessActive].Value = active.ToString();
            ignoreCellEventPermittedProcessesActive = false;
        }


        private void checkBoxPermittedProcessAutostart_CheckedChanged(object sender, EventArgs e)
        {
            if (SEBSettings.permittedProcessIndex < 0) return;
            SEBSettings.permittedProcessList = (ListObj)SEBSettings.settingsCurrent     [SEBSettings.KeyPermittedProcesses];
            SEBSettings.permittedProcessData = (DictObj)SEBSettings.permittedProcessList[SEBSettings.permittedProcessIndex];
            SEBSettings.permittedProcessData[SEBSettings.KeyAutostart] = checkBoxPermittedProcessAutostart.Checked;
        }

        private void checkBoxPermittedProcessAutohide_CheckedChanged(object sender, EventArgs e)
        {
            if (SEBSettings.permittedProcessIndex < 0) return;
            SEBSettings.permittedProcessList = (ListObj)SEBSettings.settingsCurrent     [SEBSettings.KeyPermittedProcesses];
            SEBSettings.permittedProcessData = (DictObj)SEBSettings.permittedProcessList[SEBSettings.permittedProcessIndex];
            SEBSettings.permittedProcessData[SEBSettings.KeyRunInBackground] = checkBoxPermittedProcessAutohide.Checked;
            //checkBoxPermittedProcessAutostart.Checked = checkBoxPermittedProcessAutohide.Checked;
            //checkBoxPermittedProcessAutostart.Enabled = !checkBoxPermittedProcessAutohide.Checked;
        }

        private void checkBoxPermittedProcessAllowUser_CheckedChanged(object sender, EventArgs e)
        {
            if (SEBSettings.permittedProcessIndex < 0) return;
            SEBSettings.permittedProcessList = (ListObj)SEBSettings.settingsCurrent     [SEBSettings.KeyPermittedProcesses];
            SEBSettings.permittedProcessData = (DictObj)SEBSettings.permittedProcessList[SEBSettings.permittedProcessIndex];
            SEBSettings.permittedProcessData[SEBSettings.KeyAllowUser] = checkBoxPermittedProcessAllowUser.Checked;
        }

        private void checkBoxPermittedProcessStrongKill_CheckedChanged(object sender, EventArgs e)
        {
            if (SEBSettings.permittedProcessIndex < 0) return;
            SEBSettings.permittedProcessList = (ListObj)SEBSettings.settingsCurrent     [SEBSettings.KeyPermittedProcesses];
            SEBSettings.permittedProcessData = (DictObj)SEBSettings.permittedProcessList[SEBSettings.permittedProcessIndex];
            SEBSettings.permittedProcessData[SEBSettings.KeyStrongKill] = checkBoxPermittedProcessStrongKill.Checked;
        }


        private void listBoxPermittedProcessOS_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Prevent double events from switching to false process index
            if (ignoreWidgetEventPermittedProcessesOS == true) return;
            if (     SEBSettings.permittedProcessIndex <    0) return;
            SEBSettings.permittedProcessList = (ListObj)SEBSettings.settingsCurrent     [SEBSettings.KeyPermittedProcesses];
            SEBSettings.permittedProcessData = (DictObj)SEBSettings.permittedProcessList[SEBSettings.permittedProcessIndex];
            SEBSettings.permittedProcessData[SEBSettings.KeyOS] = listBoxPermittedProcessOS.SelectedIndex;
            Int32                                           os  = listBoxPermittedProcessOS.SelectedIndex;
            ignoreCellEventPermittedProcessesOS = true;
            dataGridViewPermittedProcesses.Rows[SEBSettings.permittedProcessIndex].Cells[IntColumnProcessOS].Value = StringOS[os];
            ignoreCellEventPermittedProcessesOS = false;
        }


        private void textBoxPermittedProcessTitle_TextChanged(object sender, EventArgs e)
        {
            // Prevent double events from switching to false process index
            if (ignoreWidgetEventPermittedProcessesTitle == true) return;
            if (     SEBSettings.permittedProcessIndex    <    0) return;
            SEBSettings.permittedProcessList = (ListObj)SEBSettings.settingsCurrent     [SEBSettings.KeyPermittedProcesses];
            SEBSettings.permittedProcessData = (DictObj)SEBSettings.permittedProcessList[SEBSettings.permittedProcessIndex];
            SEBSettings.permittedProcessData[SEBSettings.KeyTitle] = textBoxPermittedProcessTitle.Text;
            String                                          title  = textBoxPermittedProcessTitle.Text;
            ignoreCellEventPermittedProcessesTitle = true;
            dataGridViewPermittedProcesses.Rows[SEBSettings.permittedProcessIndex].Cells[IntColumnProcessTitle].Value = title;
            ignoreCellEventPermittedProcessesTitle = false;
        }


        private void textBoxPermittedProcessDescription_TextChanged(object sender, EventArgs e)
        {
            if (SEBSettings.permittedProcessIndex < 0) return;
            SEBSettings.permittedProcessList = (ListObj)SEBSettings.settingsCurrent     [SEBSettings.KeyPermittedProcesses];
            SEBSettings.permittedProcessData = (DictObj)SEBSettings.permittedProcessList[SEBSettings.permittedProcessIndex];
            SEBSettings.permittedProcessData[SEBSettings.KeyDescription] = textBoxPermittedProcessDescription.Text;
        }


        private void textBoxPermittedProcessExecutable_TextChanged(object sender, EventArgs e)
        {
            // Prevent double events from switching to false process index
            if (ignoreWidgetEventPermittedProcessesExecutable == true) return;
            if (     SEBSettings.permittedProcessIndex         <    0) return;
            SEBSettings.permittedProcessList = (ListObj)SEBSettings.settingsCurrent     [SEBSettings.KeyPermittedProcesses];
            SEBSettings.permittedProcessData = (DictObj)SEBSettings.permittedProcessList[SEBSettings.permittedProcessIndex];
            SEBSettings.permittedProcessData[SEBSettings.KeyExecutable] = textBoxPermittedProcessExecutable.Text;
            String                                          executable  = textBoxPermittedProcessExecutable.Text;
            ignoreCellEventPermittedProcessesExecutable = true;
            dataGridViewPermittedProcesses.Rows[SEBSettings.permittedProcessIndex].Cells[IntColumnProcessExecutable].Value = executable;
            ignoreCellEventPermittedProcessesExecutable = false;
        }


        private void textBoxPermittedProcessPath_TextChanged(object sender, EventArgs e)
        {
            if (SEBSettings.permittedProcessIndex < 0) return;
            SEBSettings.permittedProcessList = (ListObj)SEBSettings.settingsCurrent     [SEBSettings.KeyPermittedProcesses];
            SEBSettings.permittedProcessData = (DictObj)SEBSettings.permittedProcessList[SEBSettings.permittedProcessIndex];
            SEBSettings.permittedProcessData[SEBSettings.KeyPath] = textBoxPermittedProcessPath.Text;
        }

        private void textBoxPermittedProcessIdentifier_TextChanged(object sender, EventArgs e)
        {
            if (SEBSettings.permittedProcessIndex < 0) return;
            SEBSettings.permittedProcessList = (ListObj)SEBSettings.settingsCurrent     [SEBSettings.KeyPermittedProcesses];
            SEBSettings.permittedProcessData = (DictObj)SEBSettings.permittedProcessList[SEBSettings.permittedProcessIndex];
            SEBSettings.permittedProcessData[SEBSettings.KeyIdentifier] = textBoxPermittedProcessIdentifier.Text;
        }

        private void textBoxPermittedProcessExecutables_TextChanged(object sender, EventArgs e)
        {
            if (SEBSettings.permittedProcessIndex < 0) return;
            SEBSettings.permittedProcessList = (ListObj)SEBSettings.settingsCurrent[SEBSettings.KeyPermittedProcesses];
            SEBSettings.permittedProcessData = (DictObj)SEBSettings.permittedProcessList[SEBSettings.permittedProcessIndex];
            SEBSettings.permittedProcessData[SEBSettings.KeyWindowHandlingProcess] = textBoxPermittedProcessExecutables.Text;
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

            if (dataGridViewPermittedProcessArguments.SelectedRows.Count != 1) return;

            // Get the argument data of the selected argument
            SEBSettings.permittedArgumentIndex = dataGridViewPermittedProcessArguments.SelectedRows[0].Index;
            SEBSettings.permittedProcessList   = (ListObj)SEBSettings.settingsCurrent      [SEBSettings.KeyPermittedProcesses];
            SEBSettings.permittedProcessData   = (DictObj)SEBSettings.permittedProcessList [SEBSettings.permittedProcessIndex];
            SEBSettings.permittedArgumentList  = (ListObj)SEBSettings.permittedProcessData [SEBSettings.KeyArguments];
            SEBSettings.permittedArgumentData  = (DictObj)SEBSettings.permittedArgumentList[SEBSettings.permittedArgumentIndex];
        }


        private void dataGridViewPermittedProcessArguments_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            // When a CheckBox/ListBox/TextBox entry of a DataGridView table cell is edited,
            // immediately call the CellValueChanged() event,
            // which will update the SelectedProcess data and widgets.
            if (dataGridViewPermittedProcessArguments.IsCurrentCellDirty)
                dataGridViewPermittedProcessArguments.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }


        private void dataGridViewPermittedProcessArguments_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            // Get the current cell where the user has changed a value
            int row    = dataGridViewPermittedProcessArguments.CurrentCellAddress.Y;
            int column = dataGridViewPermittedProcessArguments.CurrentCellAddress.X;

            // At the beginning, row = -1 and column = -1, so skip this event
            if (row    < 0) return;
            if (column < 0) return;

            // Get the changed value of the current cell
            object value = dataGridViewPermittedProcessArguments.CurrentCell.EditedFormattedValue;

            // Get the argument data of the argument belonging to the cell (row)
            SEBSettings.permittedArgumentIndex = row;
            SEBSettings.permittedProcessList   = (ListObj)SEBSettings.settingsCurrent      [SEBSettings.KeyPermittedProcesses];
            SEBSettings.permittedProcessData   = (DictObj)SEBSettings.permittedProcessList [SEBSettings.permittedProcessIndex];
            SEBSettings.permittedArgumentList  = (ListObj)SEBSettings.permittedProcessData [SEBSettings.KeyArguments];
            SEBSettings.permittedArgumentData  = (DictObj)SEBSettings.permittedArgumentList[SEBSettings.permittedArgumentIndex];

            // Update the argument data belonging to the current cell
            if (column == IntColumnProcessActive  ) SEBSettings.permittedArgumentData[SEBSettings.KeyActive  ] = (Boolean)value;
            if (column == IntColumnProcessArgument) SEBSettings.permittedArgumentData[SEBSettings.KeyArgument] = (String )value;
        }


        private void buttonPermittedProcessAddArgument_Click(object sender, EventArgs e)
        {
            // Get the permitted argument list
            SEBSettings.permittedProcessList  = (ListObj)SEBSettings.settingsCurrent     [SEBSettings.KeyPermittedProcesses];
            SEBSettings.permittedProcessData  = (DictObj)SEBSettings.permittedProcessList[SEBSettings.permittedProcessIndex];
            SEBSettings.permittedArgumentList = (ListObj)SEBSettings.permittedProcessData[SEBSettings.KeyArguments];

            if (SEBSettings.permittedArgumentList.Count > 0)
            {
                if (dataGridViewPermittedProcessArguments.SelectedRows.Count != 1) return;
              //SEBSettings.permittedArgumentIndex = dataGridViewPermittedProcessArguments.SelectedRows[0].Index;
                SEBSettings.permittedArgumentIndex = SEBSettings.permittedArgumentList.Count;
            }
            else
            {
                // If argument list was empty before, enable it
                SEBSettings.permittedArgumentIndex = 0;
                dataGridViewPermittedProcessArguments.Enabled = true;
            }

            // Create new argument dataset containing default values
            DictObj argumentData = new DictObj();

            argumentData[SEBSettings.KeyActive  ] = true;
            argumentData[SEBSettings.KeyArgument] = "";

            // Insert new argument into argument list at position SEBSettings.permittedArgumentIndex
            SEBSettings.permittedArgumentList         .Insert(SEBSettings.permittedArgumentIndex, argumentData);
            dataGridViewPermittedProcessArguments.Rows.Insert(SEBSettings.permittedArgumentIndex, true, "");
            dataGridViewPermittedProcessArguments.Rows       [SEBSettings.permittedArgumentIndex].Selected = true;
        }


        private void buttonPermittedProcessRemoveArgument_Click(object sender, EventArgs e)
        {
            if (dataGridViewPermittedProcessArguments.SelectedRows.Count != 1) return;

            // Get the permitted argument list
            SEBSettings.permittedArgumentIndex = dataGridViewPermittedProcessArguments.SelectedRows[0].Index;
            SEBSettings.permittedProcessList   = (ListObj)SEBSettings.settingsCurrent     [SEBSettings.KeyPermittedProcesses];
            SEBSettings.permittedProcessData   = (DictObj)SEBSettings.permittedProcessList[SEBSettings.permittedProcessIndex];
            SEBSettings.permittedArgumentList  = (ListObj)SEBSettings.permittedProcessData[SEBSettings.KeyArguments];

            // Delete argument from argument list at position SEBSettings.permittedArgumentIndex
            SEBSettings.permittedArgumentList         .RemoveAt(SEBSettings.permittedArgumentIndex);
            dataGridViewPermittedProcessArguments.Rows.RemoveAt(SEBSettings.permittedArgumentIndex);

            if (SEBSettings.permittedArgumentIndex == SEBSettings.permittedArgumentList.Count)
                SEBSettings.permittedArgumentIndex--;

            if (SEBSettings.permittedArgumentList.Count > 0)
            {
                dataGridViewPermittedProcessArguments.Rows[SEBSettings.permittedArgumentIndex].Selected = true;
            }
            else
            {
                // If argument list is now empty, disable it
                SEBSettings.permittedArgumentIndex = -1;
              //SEBSettings.permittedArgumentList.Clear();
              //SEBSettings.permittedArgumentData.Clear();
                dataGridViewPermittedProcessArguments.Enabled = false;
            }
        }



        // *******************************************
        // Group "Applications - Prohibited Processes"
        // *******************************************
        private void LoadAndUpdateProhibitedSelectedProcessGroup(int selectedProcessIndex)
        {
            // Get the process data of the selected process
            SEBSettings.prohibitedProcessList  = (ListObj)SEBSettings.settingsCurrent[SEBSettings.KeyProhibitedProcesses];
            SEBSettings.prohibitedProcessData  = (DictObj)SEBSettings.prohibitedProcessList[selectedProcessIndex];

            // Beware double events:
            // Update the widgets in "Selected Process" group,
            // but prevent the following "widget changed" event from firing the "cell changed" event once more!
            ignoreWidgetEventProhibitedProcessesActive      = true;
            ignoreWidgetEventProhibitedProcessesOS          = true;
            ignoreWidgetEventProhibitedProcessesExecutable  = true;
            ignoreWidgetEventProhibitedProcessesDescription = true;

            // Update the widgets in the "Selected Process" group
            checkBoxProhibitedProcessActive     .Checked = (Boolean)SEBSettings.prohibitedProcessData[SEBSettings.KeyActive];
            checkBoxProhibitedProcessCurrentUser.Checked = (Boolean)SEBSettings.prohibitedProcessData[SEBSettings.KeyCurrentUser];
            checkBoxProhibitedProcessStrongKill .Checked = (Boolean)SEBSettings.prohibitedProcessData[SEBSettings.KeyStrongKill];
             listBoxProhibitedProcessOS.SelectedIndex    =   (Int32)SEBSettings.prohibitedProcessData[SEBSettings.KeyOS];
             textBoxProhibitedProcessExecutable .Text    =  (String)SEBSettings.prohibitedProcessData[SEBSettings.KeyExecutable];
             textBoxProhibitedProcessDescription.Text    =  (String)SEBSettings.prohibitedProcessData[SEBSettings.KeyDescription];
             textBoxProhibitedProcessIdentifier .Text    =  (String)SEBSettings.prohibitedProcessData[SEBSettings.KeyIdentifier];
             textBoxProhibitedProcessUser       .Text    =  (String)SEBSettings.prohibitedProcessData[SEBSettings.KeyUser];

            // Reset the ignore widget event flags
            ignoreWidgetEventProhibitedProcessesActive      = false;
            ignoreWidgetEventProhibitedProcessesOS          = false;
            ignoreWidgetEventProhibitedProcessesExecutable  = false;
            ignoreWidgetEventProhibitedProcessesDescription = false;
        }


        private void ClearProhibitedSelectedProcessGroup()
        {
            // Beware double events:
            // Update the widgets in "Selected Process" group,
            // but prevent the following "widget changed" event from firing the "cell changed" event once more!
            ignoreWidgetEventProhibitedProcessesActive      = true;
            ignoreWidgetEventProhibitedProcessesOS          = true;
            ignoreWidgetEventProhibitedProcessesExecutable  = true;
            ignoreWidgetEventProhibitedProcessesDescription = true;

            // Clear the widgets in the "Selected Process" group
            checkBoxProhibitedProcessActive     .Checked = true;
            checkBoxProhibitedProcessCurrentUser.Checked = true;
            checkBoxProhibitedProcessStrongKill .Checked = false;
             listBoxProhibitedProcessOS.SelectedIndex    = IntWin;
             textBoxProhibitedProcessExecutable .Text    = "";
             textBoxProhibitedProcessDescription.Text    = "";
             textBoxProhibitedProcessIdentifier .Text    = "";
             textBoxProhibitedProcessUser       .Text    = "";

            // Reset the ignore widget event flags
            ignoreWidgetEventProhibitedProcessesActive      = false;
            ignoreWidgetEventProhibitedProcessesOS          = false;
            ignoreWidgetEventProhibitedProcessesExecutable  = false;
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

            if (dataGridViewProhibitedProcesses.SelectedRows.Count != 1) return;
            SEBSettings.prohibitedProcessIndex = dataGridViewProhibitedProcesses.SelectedRows[0].Index;

            // The process list should contain at least one element here:
            // SEBSettings.prohibitedProcessList.Count >  0
            // SEBSettings.prohibitedProcessIndex      >= 0
            LoadAndUpdateProhibitedSelectedProcessGroup(SEBSettings.prohibitedProcessIndex);
        }


        private void dataGridViewProhibitedProcesses_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            // When a CheckBox/ListBox/TextBox entry of a DataGridView table cell is edited,
            // immediately call the CellValueChanged() event,
            // which will update the SelectedProcess data and widgets.
            if (dataGridViewProhibitedProcesses.IsCurrentCellDirty)
                dataGridViewProhibitedProcesses.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }


        private void dataGridViewProhibitedProcesses_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {

            // Prevent double events from switching to false process index
            if (ignoreCellEventProhibitedProcessesActive      == true) return;
            if (ignoreCellEventProhibitedProcessesOS          == true) return;
            if (ignoreCellEventProhibitedProcessesExecutable  == true) return;
            if (ignoreCellEventProhibitedProcessesDescription == true) return;

            // Get the current cell where the user has changed a value
            int row    = dataGridViewProhibitedProcesses.CurrentCellAddress.Y;
            int column = dataGridViewProhibitedProcesses.CurrentCellAddress.X;

            // At the beginning, row = -1 and column = -1, so skip this event
            if (row    < 0) return;
            if (column < 0) return;

            // Get the changed value of the current cell
            object value = dataGridViewProhibitedProcesses.CurrentCell.EditedFormattedValue;

            // Convert the selected "OS" ListBox entry from String to Integer
            if (column == IntColumnProcessOS)
            {
                     if ((String)value == StringOSX) value = IntOSX;
                else if ((String)value == StringWin) value = IntWin;
            }

            // Get the process data of the process belonging to the current row
            SEBSettings.prohibitedProcessIndex = row;
            SEBSettings.prohibitedProcessList  = (ListObj)SEBSettings.settingsCurrent      [SEBSettings.KeyProhibitedProcesses];
            SEBSettings.prohibitedProcessData  = (DictObj)SEBSettings.prohibitedProcessList[SEBSettings.prohibitedProcessIndex];

            // Update the process data belonging to the current cell
            if (column == IntColumnProcessActive     ) SEBSettings.prohibitedProcessData[SEBSettings.KeyActive     ] = (Boolean)value;
            if (column == IntColumnProcessOS         ) SEBSettings.prohibitedProcessData[SEBSettings.KeyOS         ] = (Int32  )value;
            if (column == IntColumnProcessExecutable ) SEBSettings.prohibitedProcessData[SEBSettings.KeyExecutable ] = (String )value;
            if (column == IntColumnProcessDescription) SEBSettings.prohibitedProcessData[SEBSettings.KeyDescription] = (String )value;

            // Beware double events:
            // when a cell has been edited, update its corresponding widget in "Selected Process" group,
            // but prevent the following "widget changed" event from firing the "cell changed" event once more!
            if (column == IntColumnProcessActive     ) ignoreWidgetEventProhibitedProcessesActive      = true;
            if (column == IntColumnProcessOS         ) ignoreWidgetEventProhibitedProcessesOS          = true;
            if (column == IntColumnProcessExecutable ) ignoreWidgetEventProhibitedProcessesExecutable  = true;
            if (column == IntColumnProcessDescription) ignoreWidgetEventProhibitedProcessesDescription = true;

            // In "Selected Process" group: update the widget belonging to the current cell
            // (this will fire the corresponding "widget changed" event).
            if (column == IntColumnProcessActive     ) checkBoxProhibitedProcessActive.Checked   = (Boolean)value;
            if (column == IntColumnProcessOS         )  listBoxProhibitedProcessOS.SelectedIndex = (Int32  )value;
            if (column == IntColumnProcessExecutable )  textBoxProhibitedProcessExecutable .Text = (String )value;
            if (column == IntColumnProcessDescription)  textBoxProhibitedProcessDescription.Text = (String )value;

            // Reset the ignore widget event flags
            if (column == IntColumnProcessActive     ) ignoreWidgetEventProhibitedProcessesActive      = false;
            if (column == IntColumnProcessOS         ) ignoreWidgetEventProhibitedProcessesOS          = false;
            if (column == IntColumnProcessExecutable ) ignoreWidgetEventProhibitedProcessesExecutable  = false;
            if (column == IntColumnProcessDescription) ignoreWidgetEventProhibitedProcessesDescription = false;
        }


        private void buttonAddProhibitedProcess_Click(object sender, EventArgs e)
        {
            // Get the process list
            SEBSettings.prohibitedProcessList = (ListObj)SEBSettings.settingsCurrent[SEBSettings.KeyProhibitedProcesses];

            if (SEBSettings.prohibitedProcessList.Count > 0)
            {
                if (dataGridViewProhibitedProcesses.SelectedRows.Count != 1) return;
              //SEBSettings.prohibitedProcessIndex = dataGridViewProhibitedProcesses.SelectedRows[0].Index;
                SEBSettings.prohibitedProcessIndex = SEBSettings.prohibitedProcessList.Count;
            }
            else
            {
                // If process list was empty before, enable it
                SEBSettings.prohibitedProcessIndex      = 0;
                dataGridViewProhibitedProcesses.Enabled = true;
                    groupBoxProhibitedProcess  .Enabled = true;
            }

            // Create new process dataset containing default values
            DictObj processData = new DictObj();

            processData[SEBSettings.KeyActive     ] = true;
            processData[SEBSettings.KeyCurrentUser] = true;
            processData[SEBSettings.KeyStrongKill ] = false;
            processData[SEBSettings.KeyOS         ] = IntWin;
            processData[SEBSettings.KeyExecutable ] = "";
            processData[SEBSettings.KeyDescription] = "";
            processData[SEBSettings.KeyIdentifier ] = "";
            processData[SEBSettings.KeyUser       ] = "";

            // Insert new process into process list at position index
            SEBSettings.prohibitedProcessList   .Insert(SEBSettings.prohibitedProcessIndex, processData);
            dataGridViewProhibitedProcesses.Rows.Insert(SEBSettings.prohibitedProcessIndex, true, StringOS[IntWin], "", "");
            dataGridViewProhibitedProcesses.Rows       [SEBSettings.prohibitedProcessIndex].Selected = true;
        }


        private void buttonRemoveProhibitedProcess_Click(object sender, EventArgs e)
        {
            if (dataGridViewProhibitedProcesses.SelectedRows.Count != 1) return;

            // Clear the widgets in the "Selected Process" group
            ClearProhibitedSelectedProcessGroup();

            // Delete process from process list at position index
            SEBSettings.prohibitedProcessIndex = dataGridViewProhibitedProcesses.SelectedRows[0].Index;
            SEBSettings.prohibitedProcessList  = (ListObj)SEBSettings.settingsCurrent[SEBSettings.KeyProhibitedProcesses];
            SEBSettings.prohibitedProcessList   .RemoveAt(SEBSettings.prohibitedProcessIndex);
            dataGridViewProhibitedProcesses.Rows.RemoveAt(SEBSettings.prohibitedProcessIndex);

            if (SEBSettings.prohibitedProcessIndex == SEBSettings.prohibitedProcessList.Count)
                SEBSettings.prohibitedProcessIndex--;

            if (SEBSettings.prohibitedProcessList.Count > 0)
            {
                dataGridViewProhibitedProcesses.Rows[SEBSettings.prohibitedProcessIndex].Selected = true;
            }
            else
            {
                // If process list is now empty, disable it
                SEBSettings.prohibitedProcessIndex      = -1;
                dataGridViewProhibitedProcesses.Enabled = false;
                    groupBoxProhibitedProcess  .Enabled = false;
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
            if (ignoreWidgetEventProhibitedProcessesActive == true) return;
            if (     SEBSettings.prohibitedProcessIndex     <    0) return;
            SEBSettings.prohibitedProcessList = (ListObj)SEBSettings.settingsCurrent      [SEBSettings.KeyProhibitedProcesses];
            SEBSettings.prohibitedProcessData = (DictObj)SEBSettings.prohibitedProcessList[SEBSettings.prohibitedProcessIndex];
            SEBSettings.prohibitedProcessData[SEBSettings.KeyActive] = checkBoxProhibitedProcessActive.Checked;
            Boolean                                          active  = checkBoxProhibitedProcessActive.Checked;
            ignoreCellEventProhibitedProcessesActive = true;
            dataGridViewProhibitedProcesses.Rows[SEBSettings.prohibitedProcessIndex].Cells[IntColumnProcessActive].Value = active.ToString();
            ignoreCellEventProhibitedProcessesActive = false;
        }


        private void checkBoxProhibitedProcessCurrentUser_CheckedChanged(object sender, EventArgs e)
        {
            if (SEBSettings.prohibitedProcessIndex < 0) return;
            SEBSettings.prohibitedProcessList = (ListObj)SEBSettings.settingsCurrent      [SEBSettings.KeyProhibitedProcesses];
            SEBSettings.prohibitedProcessData = (DictObj)SEBSettings.prohibitedProcessList[SEBSettings.prohibitedProcessIndex];
            SEBSettings.prohibitedProcessData[SEBSettings.KeyCurrentUser] = checkBoxProhibitedProcessCurrentUser.Checked;
        }

        private void checkBoxProhibitedProcessStrongKill_CheckedChanged(object sender, EventArgs e)
        {
            if (SEBSettings.prohibitedProcessIndex < 0) return;
            SEBSettings.prohibitedProcessList = (ListObj)SEBSettings.settingsCurrent      [SEBSettings.KeyProhibitedProcesses];
            SEBSettings.prohibitedProcessData = (DictObj)SEBSettings.prohibitedProcessList[SEBSettings.prohibitedProcessIndex];
            SEBSettings.prohibitedProcessData[SEBSettings.KeyStrongKill] = checkBoxProhibitedProcessStrongKill.Checked;
        }


        private void listBoxProhibitedProcessOS_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Prevent double events from switching to false process index
            if (ignoreWidgetEventProhibitedProcessesOS == true) return;
            if (     SEBSettings.prohibitedProcessIndex <    0) return;
            SEBSettings.prohibitedProcessList = (ListObj)SEBSettings.settingsCurrent      [SEBSettings.KeyProhibitedProcesses];
            SEBSettings.prohibitedProcessData = (DictObj)SEBSettings.prohibitedProcessList[SEBSettings.prohibitedProcessIndex];
            SEBSettings.prohibitedProcessData[SEBSettings.KeyOS] = listBoxProhibitedProcessOS.SelectedIndex;
            Int32                                            os  = listBoxProhibitedProcessOS.SelectedIndex;
            ignoreCellEventProhibitedProcessesOS = true;
            dataGridViewProhibitedProcesses.Rows[SEBSettings.prohibitedProcessIndex].Cells[IntColumnProcessOS].Value = StringOS[os];
            ignoreCellEventProhibitedProcessesOS = false;
        }


        private void textBoxProhibitedProcessExecutable_TextChanged(object sender, EventArgs e)
        {
            // Prevent double events from switching to false process index
            if (ignoreWidgetEventProhibitedProcessesExecutable == true) return;
            if (     SEBSettings.prohibitedProcessIndex         <    0) return;
            SEBSettings.prohibitedProcessList = (ListObj)SEBSettings.settingsCurrent      [SEBSettings.KeyProhibitedProcesses];
            SEBSettings.prohibitedProcessData = (DictObj)SEBSettings.prohibitedProcessList[SEBSettings.prohibitedProcessIndex];
            SEBSettings.prohibitedProcessData[SEBSettings.KeyExecutable] = textBoxProhibitedProcessExecutable.Text;
            String                                           executable  = textBoxProhibitedProcessExecutable.Text;
            ignoreCellEventProhibitedProcessesExecutable = true;
            dataGridViewProhibitedProcesses.Rows[SEBSettings.prohibitedProcessIndex].Cells[IntColumnProcessExecutable].Value = executable;
            ignoreCellEventProhibitedProcessesExecutable = false;
        }


        private void textBoxProhibitedProcessDescription_TextChanged(object sender, EventArgs e)
        {
            // Prevent double events from switching to false process index
            if (ignoreWidgetEventProhibitedProcessesDescription == true) return;
            if (     SEBSettings.prohibitedProcessIndex          <    0) return;
            SEBSettings.prohibitedProcessList = (ListObj)SEBSettings.settingsCurrent      [SEBSettings.KeyProhibitedProcesses];
            SEBSettings.prohibitedProcessData = (DictObj)SEBSettings.prohibitedProcessList[SEBSettings.prohibitedProcessIndex];
            SEBSettings.prohibitedProcessData[SEBSettings.KeyDescription] = textBoxProhibitedProcessDescription.Text;
            String                                           description  = textBoxProhibitedProcessDescription.Text;
            ignoreCellEventProhibitedProcessesDescription = true;
            dataGridViewProhibitedProcesses.Rows[SEBSettings.prohibitedProcessIndex].Cells[IntColumnProcessDescription].Value = description;
            ignoreCellEventProhibitedProcessesDescription = false;
        }


        private void textBoxProhibitedProcessIdentifier_TextChanged(object sender, EventArgs e)
        {
            if (SEBSettings.prohibitedProcessIndex < 0) return;
            SEBSettings.prohibitedProcessList = (ListObj)SEBSettings.settingsCurrent      [SEBSettings.KeyProhibitedProcesses];
            SEBSettings.prohibitedProcessData = (DictObj)SEBSettings.prohibitedProcessList[SEBSettings.prohibitedProcessIndex];
            SEBSettings.prohibitedProcessData[SEBSettings.KeyIdentifier] = textBoxProhibitedProcessIdentifier.Text;
        }

        private void textBoxProhibitedProcessUser_TextChanged(object sender, EventArgs e)
        {
            if (SEBSettings.prohibitedProcessIndex < 0) return;
            SEBSettings.prohibitedProcessList = (ListObj)SEBSettings.settingsCurrent      [SEBSettings.KeyProhibitedProcesses];
            SEBSettings.prohibitedProcessData = (DictObj)SEBSettings.prohibitedProcessList[SEBSettings.prohibitedProcessIndex];
            SEBSettings.prohibitedProcessData[SEBSettings.KeyUser] = textBoxProhibitedProcessUser.Text;
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
            if (datagridWhitelist.CurrentRow != null)
            {
                datagridWhitelist.Rows.Remove(datagridWhitelist.CurrentRow);
                datagridWhitelist_CellValueChanged(null,null);
            }
                
        }

        private void datagridWhitelist_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            var list = new List<string>();
            foreach (DataGridViewRow r in datagridWhitelist.Rows)
            {
                foreach (DataGridViewCell cell in r.Cells)
                {
                    if(cell.Value != null && !String.IsNullOrWhiteSpace(cell.Value.ToString()))
                        list.Add(cell.Value.ToString());
                }
            }

            SEBSettings.settingsCurrent[SEBSettings.KeyUrlFilterWhitelist] = String.Join(";", list);
        }

        private void btnAddBlacklistFilter_Click(object sender, EventArgs e)
        {
            datagridBlackListFilter.Rows.Add();
        }

        private void btnRemoveBlacklistFilter_Click(object sender, EventArgs e)
        {
            if (datagridBlackListFilter.CurrentRow != null)
            {
                datagridBlackListFilter.Rows.Remove(datagridBlackListFilter.CurrentRow);
                datagridBlacklist_CellValueChanged(null, null);
            }
        }

        private void datagridBlacklist_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            var list = new List<string>();
            foreach (DataGridViewRow r in datagridBlackListFilter.Rows)
            {
                foreach (DataGridViewCell cell in r.Cells)
                {
                    if (cell.Value != null && !String.IsNullOrWhiteSpace(cell.Value.ToString()))
                        list.Add(cell.Value.ToString());
                }
            }

            SEBSettings.settingsCurrent[SEBSettings.KeyUrlFilterBlacklist] = String.Join(";", list);
        }

        private void chkFilterEmbeddedContent_CheckedChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsCurrent[SEBSettings.KeyUrlFilterEnableContentFilter] = chkFilterEmbeddedContent.Checked;
        }

        private void checkBoxUrlFilterRulesRegex_CheckedChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsCurrent[SEBSettings.KeyUrlFilterRulesAsRegex] = checkBoxUrlFilterRulesRegex.Checked;
        }

        // ******************************
        // Group "Network - Certificates"
        // ******************************
        private void comboBoxChooseSSLClientCertificate_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBoxChooseIdentity_SelectedIndexChanged(object sender, EventArgs e)
        {

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

            if (dataGridViewEmbeddedCertificates.SelectedRows.Count != 1) return;
            SEBSettings.embeddedCertificateIndex = dataGridViewEmbeddedCertificates.SelectedRows[0].Index;
        }


        private void dataGridViewEmbeddedCertificates_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            // When a CheckBox/ListBox/TextBox entry of a DataGridView table cell is edited,
            // immediately call the CellValueChanged() event.
            if (dataGridViewEmbeddedCertificates.IsCurrentCellDirty)
                dataGridViewEmbeddedCertificates.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }


        private void dataGridViewEmbeddedCertificates_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            // Get the current cell where the user has changed a value
            int row    = dataGridViewEmbeddedCertificates.CurrentCellAddress.Y;
            int column = dataGridViewEmbeddedCertificates.CurrentCellAddress.X;

            // At the beginning, row = -1 and column = -1, so skip this event
            if (row    < 0) return;
            if (column < 0) return;

            // Get the changed value of the current cell
            object value = dataGridViewEmbeddedCertificates.CurrentCell.EditedFormattedValue;

            // Convert the selected Type ListBox entry from String to Integer
            if (column == IntColumnCertificateType)
            {
                     if ((String)value == StringSSLClientCertificate) value = IntSSLClientCertificate;
                else if ((String)value == StringIdentity            ) value = IntIdentity;
            }

            // Get the data of the certificate belonging to the cell (row)
            SEBSettings.embeddedCertificateIndex = row;
            SEBSettings.embeddedCertificateList  = (ListObj)SEBSettings.settingsCurrent        [SEBSettings.KeyEmbeddedCertificates];
            SEBSettings.embeddedCertificateData  = (DictObj)SEBSettings.embeddedCertificateList[SEBSettings.embeddedCertificateIndex];

            // Update the certificate data belonging to the current cell
            if (column == IntColumnCertificateType) SEBSettings.embeddedCertificateData[SEBSettings.KeyType] = (Int32  )value;
            if (column == IntColumnCertificateName) SEBSettings.embeddedCertificateData[SEBSettings.KeyName] = (String )value;
        }


        private void buttonRemoveEmbeddedCertificate_Click(object sender, EventArgs e)
        {
            if (dataGridViewEmbeddedCertificates.SelectedRows.Count != 1) return;
            SEBSettings.embeddedCertificateIndex = dataGridViewEmbeddedCertificates.SelectedRows[0].Index;

            // Delete certificate from certificate list at position index
            SEBSettings.embeddedCertificateList = (ListObj)SEBSettings.settingsCurrent[SEBSettings.KeyEmbeddedCertificates];
            SEBSettings.embeddedCertificateList  .RemoveAt(SEBSettings.embeddedCertificateIndex);
            dataGridViewEmbeddedCertificates.Rows.RemoveAt(SEBSettings.embeddedCertificateIndex);

            if (SEBSettings.embeddedCertificateIndex == SEBSettings.embeddedCertificateList.Count)
                SEBSettings.embeddedCertificateIndex--;

            if (SEBSettings.embeddedCertificateList.Count > 0)
            {
                dataGridViewEmbeddedCertificates.Rows[SEBSettings.embeddedCertificateIndex].Selected = true;
            }
            else
            {
                // If certificate list is now empty, disable it
                SEBSettings.embeddedCertificateIndex     = -1;
                dataGridViewEmbeddedCertificates.Enabled = false;
            }
        }



        // *************************
        // Group "Network - Proxies"
        // *************************
        private void radioButtonUseSystemProxySettings_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonUseSystemProxySettings.Checked == true)
                 SEBSettings.settingsCurrent[SEBSettings.KeyProxySettingsPolicy] = 0;
            else SEBSettings.settingsCurrent[SEBSettings.KeyProxySettingsPolicy] = 1;
        }

        private void radioButtonUseSebProxySettings_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonUseSebProxySettings.Checked == true)
                 SEBSettings.settingsCurrent[SEBSettings.KeyProxySettingsPolicy] = 1;
            else SEBSettings.settingsCurrent[SEBSettings.KeyProxySettingsPolicy] = 0;
        }

        private void checkBoxExcludeSimpleHostnames_CheckedChanged(object sender, EventArgs e)
        {
            // Get the proxies data
            SEBSettings.proxiesData = (DictObj)SEBSettings.settingsCurrent[SEBSettings.KeyProxies];
            SEBSettings.proxiesData[SEBSettings.KeyExcludeSimpleHostnames] = checkBoxExcludeSimpleHostnames.Checked;
        }

        private void checkBoxUsePassiveFTPMode_CheckedChanged(object sender, EventArgs e)
        {
            // Get the proxies data
            SEBSettings.proxiesData = (DictObj)SEBSettings.settingsCurrent[SEBSettings.KeyProxies];
            SEBSettings.proxiesData[SEBSettings.KeyFTPPassive] = checkBoxUsePassiveFTPMode.Checked;
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

            if (dataGridViewProxyProtocols.SelectedRows.Count != 1) return;
            SEBSettings.proxyProtocolIndex = dataGridViewProxyProtocols.SelectedRows[0].Index;

            // if proxyProtocolIndex is    0 (AutoDiscovery    ), do nothing
            // if proxyProtocolIndex is    1 (AutoConfiguration), enable Proxy URL    widgets
            // if proxyProtocolIndex is >= 2 (... Proxy Server ), enable Proxy Server widgets

            Boolean useAutoConfiguration = (SEBSettings.proxyProtocolIndex == IntProxyAutoConfiguration);
            Boolean useProxyServer       = (SEBSettings.proxyProtocolIndex  > IntProxyAutoConfiguration);

            // Enable the proxy widgets belonging to Auto Configuration
               labelAutoProxyConfigurationURL .Visible = useAutoConfiguration;
               labelProxyConfigurationFileURL .Visible = useAutoConfiguration;
             textBoxIfYourNetworkAdministrator.Visible = useAutoConfiguration;
             textBoxAutoProxyConfigurationURL .Visible = useAutoConfiguration;
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

            if (useProxyServer)
            {
                labelProxyServerHost.Text  = StringProxyProtocolServerLabel[SEBSettings.proxyProtocolIndex];
                labelProxyServerHost.Text += " Proxy Server";
            }

            // Get the proxy protocol type
            String KeyProtocolType = KeyProxyProtocolType[SEBSettings.proxyProtocolIndex];

            // Get the proxies data
            SEBSettings.proxiesData = (DictObj)SEBSettings.settingsCurrent[SEBSettings.KeyProxies];

            // Update the proxy widgets
            if (useAutoConfiguration)
            {
                textBoxAutoProxyConfigurationURL.Text = (String)SEBSettings.proxiesData[SEBSettings.KeyAutoConfigurationURL];
            }

            if (useProxyServer)
            {
                checkBoxProxyServerRequires.Checked = (Boolean)SEBSettings.proxiesData[KeyProtocolType + SEBSettings.KeyRequires];
                 textBoxProxyServerHost    .Text    =  (String)SEBSettings.proxiesData[KeyProtocolType + SEBSettings.KeyHost    ];
                 textBoxProxyServerPort    .Text    =  (String)SEBSettings.proxiesData[KeyProtocolType + SEBSettings.KeyPort    ].ToString();
                 textBoxProxyServerUsername.Text    =  (String)SEBSettings.proxiesData[KeyProtocolType + SEBSettings.KeyUsername];
                 textBoxProxyServerPassword.Text    =  (String)SEBSettings.proxiesData[KeyProtocolType + SEBSettings.KeyPassword];

                 // Disable the username/password textboxes when they are not required
                 textBoxProxyServerUsername.Enabled =  checkBoxProxyServerRequires.Checked;
                 textBoxProxyServerPassword.Enabled =  checkBoxProxyServerRequires.Checked;
            }
        }


        private void dataGridViewProxyProtocols_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            // When a CheckBox/ListBox/TextBox entry of a DataGridView table cell is edited,
            // immediately call the CellValueChanged() event.
            if (dataGridViewProxyProtocols.IsCurrentCellDirty)
                dataGridViewProxyProtocols.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }


        private void dataGridViewProxyProtocols_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            // Get the current cell where the user has changed a value
            int row    = dataGridViewProxyProtocols.CurrentCellAddress.Y;
            int column = dataGridViewProxyProtocols.CurrentCellAddress.X;

            // At the beginning, row = -1 and column = -1, so skip this event
            if (row    < 0) return;
            if (column < 0) return;

            // Get the changed value of the current cell
            object value = dataGridViewProxyProtocols.CurrentCell.EditedFormattedValue;

            // Get the proxies data of the proxy protocol belonging to the cell (row)
            SEBSettings.proxiesData = (DictObj)SEBSettings.settingsCurrent[SEBSettings.KeyProxies];

            SEBSettings.proxyProtocolIndex = row;

            // Update the proxy enable data belonging to the current cell
            if (column == IntColumnProxyProtocolEnable)
            {
                String key = KeyProxyProtocolEnable[row];
                SEBSettings.proxiesData    [key] = (Boolean)value;
                BooleanProxyProtocolEnabled[row] = (Boolean)value;
            }
        }


        private void textBoxAutoProxyConfigurationURL_TextChanged(object sender, EventArgs e)
        {
            // Get the proxies data
            SEBSettings.proxiesData = (DictObj)SEBSettings.settingsCurrent[SEBSettings.KeyProxies];
            SEBSettings.proxiesData[SEBSettings.KeyAutoConfigurationURL] = textBoxAutoProxyConfigurationURL.Text;
        }

        private void buttonChooseProxyConfigurationFile_Click(object sender, EventArgs e)
        {

        }


        private void textBoxProxyServerHost_TextChanged(object sender, EventArgs e)
        {
            // Get the proxies data
            String key = KeyProxyProtocolType[SEBSettings.proxyProtocolIndex] + SEBSettings.KeyHost;
            SEBSettings.proxiesData      = (DictObj)SEBSettings.settingsCurrent[SEBSettings.KeyProxies];
            SEBSettings.proxiesData[key] = textBoxProxyServerHost.Text;
        }

        private void textBoxProxyServerPort_TextChanged(object sender, EventArgs e)
        {
            // Get the proxies data
            String key =  KeyProxyProtocolType[SEBSettings.proxyProtocolIndex] + SEBSettings.KeyPort;
            SEBSettings.proxiesData = (DictObj)SEBSettings.settingsCurrent      [SEBSettings.KeyProxies];

            // Convert the "Port" string to an integer
            try
            {
                SEBSettings.proxiesData[key] = Int32.Parse(textBoxProxyServerPort.Text);
            }
            catch (FormatException)
            {
                textBoxProxyServerPort.Text = "";
            }
        }

        private void checkBoxProxyServerRequiresPassword_CheckedChanged(object sender, EventArgs e)
        {
            // Get the proxies data
            String key = KeyProxyProtocolType[SEBSettings.proxyProtocolIndex] + SEBSettings.KeyRequires;
            SEBSettings.proxiesData      = (DictObj)SEBSettings.settingsCurrent[SEBSettings.KeyProxies];
            SEBSettings.proxiesData[key] = (Boolean)checkBoxProxyServerRequires.Checked;

            // Disable the username/password textboxes when they are not required
            textBoxProxyServerUsername.Enabled = checkBoxProxyServerRequires.Checked;
            textBoxProxyServerPassword.Enabled = checkBoxProxyServerRequires.Checked;
        }

        private void textBoxProxyServerUsername_TextChanged(object sender, EventArgs e)
        {
            // Get the proxies data
            String key = KeyProxyProtocolType[SEBSettings.proxyProtocolIndex] + SEBSettings.KeyUsername;
            SEBSettings.proxiesData      = (DictObj)SEBSettings.settingsCurrent[SEBSettings.KeyProxies];
            SEBSettings.proxiesData[key] = textBoxProxyServerUsername.Text;
        }

        private void textBoxProxyServerPassword_TextChanged(object sender, EventArgs e)
        {
            // Get the proxies data
            String key = KeyProxyProtocolType[SEBSettings.proxyProtocolIndex] + SEBSettings.KeyPassword;
            SEBSettings.proxiesData      = (DictObj)SEBSettings.settingsCurrent[SEBSettings.KeyProxies];
            SEBSettings.proxiesData[key] = textBoxProxyServerPassword.Text;
        }


        private void textBoxBypassedProxyHostList_Validated(object sender, EventArgs e)
        {
            // Get the proxies data
            SEBSettings.proxiesData = (DictObj)SEBSettings.settingsCurrent[SEBSettings.KeyProxies];
            string bypassedProxiesCommaSeparatedList = textBoxBypassedProxyHostList.Text;
            // Create List
            List<string> bypassedProxyHostList = bypassedProxiesCommaSeparatedList.Split(',').ToList();
            // Trim whitespace from host strings
            ListObj bypassedProxyTrimmedHostList = new ListObj();
            foreach (string host in bypassedProxyHostList)
            {
                bypassedProxyTrimmedHostList.Add(host.Trim());
            }
            SEBSettings.proxiesData[SEBSettings.KeyExceptionsList] = bypassedProxyTrimmedHostList;
        }
        

        // ****************
        // Group "Security"
        // ****************
        private void listBoxSebServicePolicy_SelectedIndexChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsCurrent[SEBSettings.KeySebServicePolicy] = listBoxSebServicePolicy.SelectedIndex;
        }

        private void checkBoxAllowVirtualMachine_CheckedChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsCurrent[SEBSettings.KeyAllowVirtualMachine] = checkBoxAllowVirtualMachine.Checked;
        }

        private void radioCreateNewDesktop_CheckedChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsCurrent[SEBSettings.KeyCreateNewDesktop] = radioCreateNewDesktop.Checked;
            if (radioCreateNewDesktop.Checked && (int)SEBSettings.settingsCurrent[SEBSettings.KeyTouchOptimized] == 1)
            {
                MessageBox.Show(
                    "Touch optimization will not work when the kiosk-mode is set to Create New Desktop, please change the appearance.", "Attention", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void radioKillExplorerShell_CheckedChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsCurrent[SEBSettings.KeyKillExplorerShell] = radioKillExplorerShell.Checked;
        }

        private void checkBoxAllowWlan_CheckedChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsCurrent[SEBSettings.KeyAllowWLAN] = checkboxAllowWlan.Checked;
        }

        private void checkBoxAllowUserSwitching_CheckedChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsCurrent[SEBSettings.KeyAllowUserSwitching] = checkBoxAllowUserSwitching.Checked;
        }

        private void checkBoxEnableLogging_CheckedChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsCurrent[SEBSettings.KeyEnableLogging] = checkBoxEnableLogging.Checked;
        }

        private void buttonLogDirectoryWin_Click(object sender, EventArgs e)
        {
            // Set the default directory in the Folder Browser Dialog
            folderBrowserDialogLogDirectoryWin.RootFolder = Environment.SpecialFolder.MyDocuments;
//          folderBrowserDialogLogDirectoryWin.RootFolder = Environment.CurrentDirectory;

            // Get the user inputs in the File Dialog
            DialogResult dialogResult = folderBrowserDialogLogDirectoryWin.ShowDialog();
            String               path = folderBrowserDialogLogDirectoryWin.SelectedPath;

            // If the user clicked "Cancel", do nothing
            if (dialogResult.Equals(DialogResult.Cancel)) return;

            // If the user clicked "OK", ...
            SEBSettings.settingsCurrent[SEBSettings.KeyLogDirectoryWin]     = path;                                      
            labelLogDirectoryWin.Text = path;
            if (String.IsNullOrEmpty(path))
            {
                checkBoxUseStandardDirectory.Checked = true;
            }
            else
            {
                checkBoxUseStandardDirectory.Checked = false;
            }
        }


        // ****************
        // Group "Registry"
        // ****************



        // ******************
        // Group "Inside SEB"
        // ******************
        private void checkBoxInsideSebEnableSwitchUser_CheckedChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsCurrent[SEBSettings.KeyInsideSebEnableSwitchUser] = checkBoxInsideSebEnableSwitchUser.Checked;
        }

        private void checkBoxInsideSebEnableLockThisComputer_CheckedChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsCurrent[SEBSettings.KeyInsideSebEnableLockThisComputer] = checkBoxInsideSebEnableLockThisComputer.Checked;
        }

        private void checkBoxInsideSebEnableChangeAPassword_CheckedChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsCurrent[SEBSettings.KeyInsideSebEnableChangeAPassword] = checkBoxInsideSebEnableChangeAPassword.Checked;
        }

        private void checkBoxInsideSebEnableStartTaskManager_CheckedChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsCurrent[SEBSettings.KeyInsideSebEnableStartTaskManager] = checkBoxInsideSebEnableStartTaskManager.Checked;

        }

        private void checkBoxInsideSebEnableLogOff_CheckedChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsCurrent[SEBSettings.KeyInsideSebEnableLogOff] = checkBoxInsideSebEnableLogOff.Checked;
        }

        private void checkBoxInsideSebEnableShutDown_CheckedChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsCurrent[SEBSettings.KeyInsideSebEnableShutDown] = checkBoxInsideSebEnableShutDown.Checked;
        }

        private void checkBoxInsideSebEnableEaseOfAccess_CheckedChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsCurrent[SEBSettings.KeyInsideSebEnableEaseOfAccess] = checkBoxInsideSebEnableEaseOfAccess.Checked;
        }

        private void checkBoxInsideSebEnableVmWareClientShade_CheckedChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsCurrent[SEBSettings.KeyInsideSebEnableVmWareClientShade] = checkBoxInsideSebEnableVmWareClientShade.Checked;
        }


        // *******************
        // Group "Hooked Keys"
        // *******************
        private void checkBoxHookKeys_CheckedChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsCurrent[SEBSettings.KeyHookKeys] = checkBoxHookKeys.Checked;
        }



        // ********************
        // Group "Special Keys"
        // ********************
        private void checkBoxEnableEsc_CheckedChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsCurrent[SEBSettings.KeyEnableEsc] = checkBoxEnableEsc.Checked;
        }

        private void checkBoxEnableCtrlEsc_CheckedChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsCurrent[SEBSettings.KeyEnableCtrlEsc] = checkBoxEnableCtrlEsc.Checked;
        }

        private void checkBoxEnableAltEsc_CheckedChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsCurrent[SEBSettings.KeyEnableAltEsc] = checkBoxEnableAltEsc.Checked;
        }

        private void checkBoxEnableAltTab_CheckedChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsCurrent[SEBSettings.KeyEnableAltTab] = checkBoxEnableAltTab.Checked;
        }

        private void checkBoxEnableAltF4_CheckedChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsCurrent[SEBSettings.KeyEnableAltF4] = checkBoxEnableAltF4.Checked;
        }

        private void checkBoxEnableRightMouse_CheckedChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsCurrent[SEBSettings.KeyEnableRightMouse] = checkBoxEnableRightMouse.Checked;
        }

        private void checkBoxEnablePrintScreen_CheckedChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsCurrent[SEBSettings.KeyEnablePrintScreen] = checkBoxEnablePrintScreen.Checked;
            checkBoxEnableScreenCapture.Checked = checkBoxEnablePrintScreen.Checked;

        }

        private void checkBoxEnableAltMouseWheel_CheckedChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsCurrent[SEBSettings.KeyEnableAltMouseWheel] = checkBoxEnableAltMouseWheel.Checked;
        }


        // *********************
        // Group "Function Keys"
        // *********************
        private void checkBoxEnableF1_CheckedChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsCurrent[SEBSettings.KeyEnableF1] = checkBoxEnableF1.Checked;
        }

        private void checkBoxEnableF2_CheckedChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsCurrent[SEBSettings.KeyEnableF2] = checkBoxEnableF2.Checked;
        }

        private void checkBoxEnableF3_CheckedChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsCurrent[SEBSettings.KeyEnableF3] = checkBoxEnableF3.Checked;
        }

        private void checkBoxEnableF4_CheckedChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsCurrent[SEBSettings.KeyEnableF4] = checkBoxEnableF4.Checked;
        }

        private void checkBoxEnableF5_CheckedChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsCurrent[SEBSettings.KeyEnableF5] = checkBoxEnableF5.Checked;
        }

        private void checkboxShowReloadButton_CheckedChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsCurrent[SEBSettings.KeyShowReloadButton] = checkboxShowReloadButton.Checked;
        }


        private void checkBoxEnableF6_CheckedChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsCurrent[SEBSettings.KeyEnableF6] = checkBoxEnableF6.Checked;
        }

        private void checkBoxEnableF7_CheckedChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsCurrent[SEBSettings.KeyEnableF7] = checkBoxEnableF7.Checked;
        }

        private void checkBoxEnableF8_CheckedChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsCurrent[SEBSettings.KeyEnableF8] = checkBoxEnableF8.Checked;
        }

        private void checkBoxEnableF9_CheckedChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsCurrent[SEBSettings.KeyEnableF9] = checkBoxEnableF9.Checked;
        }

        private void checkBoxEnableF10_CheckedChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsCurrent[SEBSettings.KeyEnableF10] = checkBoxEnableF10.Checked;
        }

        private void checkBoxEnableF11_CheckedChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsCurrent[SEBSettings.KeyEnableF11] = checkBoxEnableF11.Checked;
        }

        private void checkBoxEnableF12_CheckedChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsCurrent[SEBSettings.KeyEnableF12] = checkBoxEnableF12.Checked;
        }

        private void labelHashedAdminPassword_Click(object sender, EventArgs e)
        {

        }

        private void labelOpenLinksHTML_Click(object sender, EventArgs e)
        {

        }

        private void checkBoxCopyBrowserExamKey_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void textBoxLogDirectoryOSX_TextChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsCurrent[SEBSettings.KeyLogDirectoryOSX] = textBoxLogDirectoryOSX.Text;
        }

        private void textBoxDownloadDirectoryOSX_TextChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsCurrent[SEBSettings.KeyDownloadDirectoryOSX] = textBoxDownloadDirectoryOSX.Text;
        }

        private void checkBoxDownloadOpenSEBFiles_CheckedChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsCurrent[SEBSettings.KeyDownloadAndOpenSebConfig] = checkBoxDownloadOpenSEBFiles.Checked;
        }

        private void checkBoxEnableScreenCapture_CheckedChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsCurrent[SEBSettings.KeyEnablePrintScreen] = checkBoxEnableScreenCapture.Checked;
            checkBoxEnablePrintScreen.Checked = checkBoxEnableScreenCapture.Checked;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void checkBoxReloadWarning_CheckedChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsCurrent[SEBSettings.KeyShowReloadWarning] = checkBoxReloadWarning.Checked;
        }

        private void checkBoxUseStandardDirectory_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxUseStandardDirectory.Checked)
            {
                SEBSettings.settingsCurrent[SEBSettings.KeyLogDirectoryWin] = "";
                labelLogDirectoryWin.Text = "";
            }
        }

        private void checkBoxEnableZoomText_CheckedChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsCurrent[SEBSettings.KeyEnableZoomText] = checkBoxEnableZoomText.Checked;
        }

        private void checkBoxEnableZoomPage_CheckedChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsCurrent[SEBSettings.KeyEnableZoomPage] = checkBoxEnableZoomPage.Checked;
        }

    } // end of   class     SebWindowsConfigForm
}     // end of   namespace SebWindowsConfig
