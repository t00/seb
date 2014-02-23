using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
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
        public bool adminPasswordFieldsContainHash = false;
        public bool quitPasswordFieldsContainHash = false;
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

            // Read the settings from the standard configuration file??? Currently not
            //SEBSettings.WriteSebConfigurationFile(defaultPathSebConfigFile);
            //SEBSettings. ReadSebConfigurationFile(defaultPathSebConfigFile);
            //SEBSettings.WriteSebConfigurationFile("sebClientDefaultMist.seb");
            //SEBSettings. ReadSebConfigurationFile("sebClientDefaultMist.seb");

            //SEBSettings.LoggSettingsDictionary(ref SEBSettings.settingsDefault, "DebugSettingsDefault_In_Constructor.txt");
            //SEBSettings.LoggSettingsDictionary(ref SEBSettings.settingsCurrent, "DebugSettingsCurrent_In_Constructor.txt");

            // Initialise the global variables for the GUI widgets
            InitialiseGlobalVariablesForGUIWidgets();

            // Initialise the GUI widgets themselves
            InitialiseGUIWidgets();

            // When starting up, set the widgets to the default values
            UpdateAllWidgetsOfProgram();

        } // end of contructor   SebWindowsConfigForm()




        // *************************************************
        // Open the configuration file and read the settings
        // *************************************************
        private Boolean LoadConfigurationFileIntoEditor(String fileName)
        {
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
            SEBSettings.urlFilterRuleIndex   = -1;
            SEBSettings.urlFilterActionIndex = -1;

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
            return true;
        }



        // ********************************************************
        // Write the settings to the configuration file and save it
        // ********************************************************
        private Boolean SaveConfigurationFileFromEditor(String fileName)
        {
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
            return true;
        }



        // ****************************************
        // Update the table of the URL Filter Rules
        // ****************************************
        private void UpdateTableOfURLFilterRules()
        {
            // Clear all help structures for table access.
            // CAUTION:
            // Do NOT clear the urlFilterTableShowRule list here!
            // Its information is needed for building up the URL filter table!
            urlFilterTableRuleIndex     .Clear();
            urlFilterTableActionIndex   .Clear();
            urlFilterTableIsTitleRow    .Clear();
            urlFilterTableStartRow      .Clear();
            urlFilterTableEndRow        .Clear();
            urlFilterTableCellIsDisabled.Clear();

            // Get the URL Filter Rules
            SEBSettings.urlFilterRuleList = (ListObj)SEBSettings.settingsCurrent[SEBSettings.KeyURLFilterRules];

            // Clear the table itself
            dataGridViewURLFilterRules.Enabled = (SEBSettings.urlFilterRuleList.Count > 0);
            dataGridViewURLFilterRules.Rows.Clear();

            int row = 0;

            // Add URL Filter Rules of currently opened file to DataGridView
            for (int ruleIndex = 0; ruleIndex < SEBSettings.urlFilterRuleList.Count; ruleIndex++)
            {
                SEBSettings.urlFilterRuleData   = (DictObj)SEBSettings.urlFilterRuleList[ruleIndex];
                Boolean     active              = (Boolean)SEBSettings.urlFilterRuleData[SEBSettings.KeyActive];
                String      expression          = (String )SEBSettings.urlFilterRuleData[SEBSettings.KeyExpression];
                SEBSettings.urlFilterActionList = (ListObj)SEBSettings.urlFilterRuleData[SEBSettings.KeyRuleActions];

                urlFilterTableRuleIndex  .Add(ruleIndex);
                urlFilterTableActionIndex.Add(-1);
                urlFilterTableIsTitleRow .Add(true);
                urlFilterTableStartRow   .Add(row);
                urlFilterTableEndRow     .Add(row);

                // If user chose EXPANDED view for this rule, add the action rows
                if (urlFilterTableShowRule[ruleIndex])
                    urlFilterTableEndRow  [ruleIndex] += SEBSettings.urlFilterActionList.Count;

                urlFilterTableCellIsDisabled.Add(new List<Boolean>());
                urlFilterTableCellIsDisabled[row] = urlFilterTableDisabledColumnsOfRule.ToList();
              //urlFilterTableCellIsDisabled[row, IntColumnURLFilterRuleRegex ] = true;
              //urlFilterTableCellIsDisabled[row, IntColumnURLFilterRuleAction] = true;

                // Add  title row for current Filter Rule.
                // Show title row in LightGrey and Expression in Bold.
                // For  title row, disable the Regex and Action widgets.

                if (urlFilterTableShowRule[ruleIndex])
                     dataGridViewURLFilterRules.Rows.Add(StringCollapse, active, false, expression, "");
                else dataGridViewURLFilterRules.Rows.Add(StringExpand  , active, false, expression, "");

                dataGridViewURLFilterRules.Rows[row].DefaultCellStyle.BackColor                         = Color.LightGray;
                dataGridViewURLFilterRules.Rows[row].Cells[IntColumnURLFilterRuleExpression].Style.Font = new Font(DefaultFont, FontStyle.Bold);
                dataGridViewURLFilterRules.Rows[row].Cells[IntColumnURLFilterRuleRegex     ].ReadOnly   = true;
                dataGridViewURLFilterRules.Rows[row].Cells[IntColumnURLFilterRuleAction    ].ReadOnly   = true;

                row++;

                // If user chose COLLAPSED view for this rule:
                // Do not show the actions, but continue with next rule.
                if (urlFilterTableShowRule[ruleIndex] == false) continue;

                // If user chose EXPANDED view for this rule:
                // Add actions of current rule to DataGridView
                for (int actionIndex = 0; actionIndex < SEBSettings.urlFilterActionList.Count; actionIndex++)
                {
                    SEBSettings.urlFilterActionData = (DictObj)SEBSettings.urlFilterActionList[actionIndex];

                    Boolean Active     = (Boolean)SEBSettings.urlFilterActionData[SEBSettings.KeyActive];
                    Boolean Regex      = (Boolean)SEBSettings.urlFilterActionData[SEBSettings.KeyRegex];
                    String  Expression = (String )SEBSettings.urlFilterActionData[SEBSettings.KeyExpression];
                    Int32   Action     = (Int32  )SEBSettings.urlFilterActionData[SEBSettings.KeyAction];

                    urlFilterTableRuleIndex  .Add(  ruleIndex);
                    urlFilterTableActionIndex.Add(actionIndex);
                    urlFilterTableIsTitleRow .Add(false);

                    urlFilterTableCellIsDisabled.Add(new List<Boolean>());
                    urlFilterTableCellIsDisabled[row] = urlFilterTableDisabledColumnsOfAction.ToList();
                  //urlFilterTableCellIsDisabled[row, IntColumnURLFilterRuleShow] = true;

                    // Add Action row for current Filter Rule.
                    // For Action row, disable the Show widget.
                    dataGridViewURLFilterRules.Rows.Add("", Active, Regex, Expression, StringAction[Action]);
                    dataGridViewURLFilterRules.Rows[row].Cells[IntColumnURLFilterRuleShow].ReadOnly = true;

                    row++;

                } // next actionIndex
            } // next ruleIndex


            // Set the "selected index" focus to the row of current rule and action
            if (SEBSettings.urlFilterRuleList.Count == 0) return;

            urlFilterTableRow = urlFilterTableStartRow[SEBSettings.urlFilterRuleIndex] + SEBSettings.urlFilterActionIndex + 1;
            dataGridViewURLFilterRules.Rows[urlFilterTableRow].Selected = true;

            // Determine if the selected row is a title row or action row.
            // Determine which rule and action belong to the selected row.
                        urlFilterTableRowIsTitle = urlFilterTableIsTitleRow [urlFilterTableRow];
            SEBSettings.urlFilterRuleIndex       = urlFilterTableRuleIndex  [urlFilterTableRow];
            SEBSettings.urlFilterActionIndex     = urlFilterTableActionIndex[urlFilterTableRow];
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
                // The order of setting the placeholders and the flag is very much relevant!
                textBoxAdminPassword.Text = "0000000000000000";
                adminPasswordFieldsContainHash = true;
                textBoxConfirmAdminPassword.Text = "0000000000000000";
            }
            else
            {
                // Same here: The order of setting the placeholders and the flag is very much relevant!
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
                // The order of setting the placeholders and the flag is very much relevant!
                textBoxQuitPassword.Text = "0000000000000000";
                quitPasswordFieldsContainHash = true;
                textBoxConfirmQuitPassword.Text = "0000000000000000";
            }
            else
            {
                // Same here: The order of setting the placeholders and the flag is very much relevant!
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
                // We need to reset this flag before changing the textBox text value, because otherwise the compare passwords
                // method will delete the first textBox again
                settingsPasswordFieldsContainHash = false;
                textBoxSettingsPassword.Text = "0000000000000000";
                settingsPasswordFieldsContainHash = true;
                textBoxConfirmSettingsPassword.Text = "0000000000000000";
            }
            else
            {
                textBoxSettingsPassword       .Text = settingsPassword;
                textBoxConfirmSettingsPassword.Text = settingsPassword;
            }

            // Group "Appearance"
            radioButtonUseBrowserWindow       .Checked     =    ((int)SEBSettings.settingsCurrent[SEBSettings.KeyBrowserViewMode] == 0);
            radioButtonUseFullScreenMode      .Checked     =    ((int)SEBSettings.settingsCurrent[SEBSettings.KeyBrowserViewMode] == 1);
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
            checkBoxUseSebWithoutBrowser    .Checked = !((Boolean)SEBSettings.settingsCurrent[SEBSettings.KeyEnableSebBrowser]);
            // BEWARE: you must invert this value since "Use Without" is "Not Enable"!

            // Group "Down/Uploads"
            checkBoxAllowDownUploads.Checked           = (Boolean)SEBSettings.settingsCurrent[SEBSettings.KeyAllowDownUploads];
            checkBoxOpenDownloads   .Checked           = (Boolean)SEBSettings.settingsCurrent[SEBSettings.KeyOpenDownloads];
            checkBoxDownloadPDFFiles.Checked           = (Boolean)SEBSettings.settingsCurrent[SEBSettings.KeyDownloadPDFFiles];
            labelDownloadDirectoryWin.Text             =  (String)SEBSettings.settingsCurrent[SEBSettings.KeyDownloadDirectoryWin];
             listBoxChooseFileToUploadPolicy.SelectedIndex = (int)SEBSettings.settingsCurrent[SEBSettings.KeyChooseFileToUploadPolicy];

            // Group "Exam"
           //textBoxBrowserExamKey    .Text    =  (String)SEBSettings.settingsCurrent[SEBSettings.KeyBrowserExamKey];
             textBoxQuitURL           .Text    =  (String)SEBSettings.settingsCurrent[SEBSettings.KeyQuitURL];
            checkBoxSendBrowserExamKey.Checked = (Boolean)SEBSettings.settingsCurrent[SEBSettings.KeySendBrowserExamKey];

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

            dataGridViewBypassedProxies.Enabled = (SEBSettings.bypassedProxyList.Count > 0);
            dataGridViewBypassedProxies.Rows.Clear();

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

            // Add URL Filter Rules of currently opened file to DataGridView
            UpdateTableOfURLFilterRules();

            // Add Embedded Certificates of Certificate Store to DataGridView
            for (int index = 0; index < SEBSettings.embeddedCertificateList.Count; index++)
            {
                SEBSettings.embeddedCertificateData = (DictObj)SEBSettings.embeddedCertificateList[index];
                String      data                    = (String )SEBSettings.embeddedCertificateData[SEBSettings.KeyCertificateData];
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

            // Add Bypassed Proxies of currently opened file to DataGridView
            for (int index = 0; index < SEBSettings.bypassedProxyList.Count; index++)
            {
                SEBSettings.bypassedProxyData = (String)SEBSettings.bypassedProxyList[index];
                dataGridViewBypassedProxies.Rows.Add   (SEBSettings.bypassedProxyData);
            }

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


            // Group "Network - Filter"
            checkBoxEnableURLFilter       .Checked = (Boolean)SEBSettings.settingsCurrent[SEBSettings.KeyEnableURLFilter];
            checkBoxEnableURLContentFilter.Checked = (Boolean)SEBSettings.settingsCurrent[SEBSettings.KeyEnableURLContentFilter];

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
            checkBoxCreateNewDesktop   .Checked    = (Boolean)SEBSettings.settingsCurrent[SEBSettings.KeyCreateNewDesktop];
            checkBoxKillExplorerShell  .Checked    = (Boolean)SEBSettings.settingsCurrent[SEBSettings.KeyKillExplorerShell];
            checkBoxAllowUserSwitching .Checked    = (Boolean)SEBSettings.settingsCurrent[SEBSettings.KeyAllowUserSwitching];
            checkBoxEnableLogging      .Checked    = (Boolean)SEBSettings.settingsCurrent[SEBSettings.KeyEnableLogging];
            labelLogDirectoryWin       .Text       =  (String)SEBSettings.settingsCurrent[SEBSettings.KeyLogDirectoryWin];

            // Group "Registry"
            checkBoxInsideSebEnableSwitchUser       .Checked = (Boolean)SEBSettings.settingsCurrent[SEBSettings.KeyInsideSebEnableSwitchUser];
            checkBoxInsideSebEnableLockThisComputer .Checked = (Boolean)SEBSettings.settingsCurrent[SEBSettings.KeyInsideSebEnableLockThisComputer];
            checkBoxInsideSebEnableChangeAPassword  .Checked = (Boolean)SEBSettings.settingsCurrent[SEBSettings.KeyInsideSebEnableChangeAPassword];
            checkBoxInsideSebEnableStartTaskManager .Checked = (Boolean)SEBSettings.settingsCurrent[SEBSettings.KeyInsideSebEnableStartTaskManager];
            checkBoxInsideSebEnableLogOff           .Checked = (Boolean)SEBSettings.settingsCurrent[SEBSettings.KeyInsideSebEnableLogOff];
            checkBoxInsideSebEnableShutDown         .Checked = (Boolean)SEBSettings.settingsCurrent[SEBSettings.KeyInsideSebEnableShutDown];
            checkBoxInsideSebEnableEaseOfAccess     .Checked = (Boolean)SEBSettings.settingsCurrent[SEBSettings.KeyInsideSebEnableEaseOfAccess];
            checkBoxInsideSebEnableVmWareClientShade.Checked = (Boolean)SEBSettings.settingsCurrent[SEBSettings.KeyInsideSebEnableVmWareClientShade];

            checkBoxOutsideSebEnableSwitchUser       .Checked = (Boolean)SEBSettings.settingsCurrent[SEBSettings.KeyOutsideSebEnableSwitchUser];
            checkBoxOutsideSebEnableLockThisComputer .Checked = (Boolean)SEBSettings.settingsCurrent[SEBSettings.KeyOutsideSebEnableLockThisComputer];
            checkBoxOutsideSebEnableChangeAPassword  .Checked = (Boolean)SEBSettings.settingsCurrent[SEBSettings.KeyOutsideSebEnableChangeAPassword];
            checkBoxOutsideSebEnableStartTaskManager .Checked = (Boolean)SEBSettings.settingsCurrent[SEBSettings.KeyOutsideSebEnableStartTaskManager];
            checkBoxOutsideSebEnableLogOff           .Checked = (Boolean)SEBSettings.settingsCurrent[SEBSettings.KeyOutsideSebEnableLogOff];
            checkBoxOutsideSebEnableShutDown         .Checked = (Boolean)SEBSettings.settingsCurrent[SEBSettings.KeyOutsideSebEnableShutDown];
            checkBoxOutsideSebEnableEaseOfAccess     .Checked = (Boolean)SEBSettings.settingsCurrent[SEBSettings.KeyOutsideSebEnableEaseOfAccess];
            checkBoxOutsideSebEnableVmWareClientShade.Checked = (Boolean)SEBSettings.settingsCurrent[SEBSettings.KeyOutsideSebEnableVmWareClientShade];

            // Group "Hooked Keys"
            checkBoxHookKeys.Checked = (Boolean)SEBSettings.settingsCurrent[SEBSettings.KeyHookKeys];

            checkBoxEnableEsc        .Checked = (Boolean)SEBSettings.settingsCurrent[SEBSettings.KeyEnableEsc];
            checkBoxEnableCtrlEsc    .Checked = (Boolean)SEBSettings.settingsCurrent[SEBSettings.KeyEnableCtrlEsc];
            checkBoxEnableAltEsc     .Checked = (Boolean)SEBSettings.settingsCurrent[SEBSettings.KeyEnableAltEsc];
            checkBoxEnableAltTab     .Checked = (Boolean)SEBSettings.settingsCurrent[SEBSettings.KeyEnableAltTab];
            checkBoxEnableAltF4      .Checked = (Boolean)SEBSettings.settingsCurrent[SEBSettings.KeyEnableAltF4];
            checkBoxEnableStartMenu  .Checked = (Boolean)SEBSettings.settingsCurrent[SEBSettings.KeyEnableStartMenu];
            checkBoxEnableRightMouse .Checked = (Boolean)SEBSettings.settingsCurrent[SEBSettings.KeyEnableRightMouse];
            checkBoxEnablePrintScreen.Checked = (Boolean)SEBSettings.settingsCurrent[SEBSettings.KeyEnablePrintScreen];

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


        private void buttonDefaultSettings_Click(object sender, EventArgs e)
        {
            //Plist.writeXml(SEBSettings.settingsDefault, "DebugSettingsDefault_Before_RevertToDefault.xml");
            //Plist.writeXml(SEBSettings.settingsCurrent, "DebugSettingsCurrent_Before_RevertToDefault.xml");
            settingsPassword = "";
            settingsPasswordFieldsContainHash = false;
            SEBSettings.RestoreDefaultAndCurrentSettings();
            SEBSettings.PermitXulRunnerProcess();
          //SEBSettings.LoggSettingsDictionary(ref SEBSettings.settingsDefault, "DebugSettingsDefault_In_ButtonDefault.txt");
          //SEBSettings.LoggSettingsDictionary(ref SEBSettings.settingsCurrent, "DebugSettingsCurrent_In_ButtonDefault.txt");
            UpdateAllWidgetsOfProgram();
            //Plist.writeXml(SEBSettings.settingsDefault, "DebugSettingsDefault_After_RevertToDefault.xml");
            //Plist.writeXml(SEBSettings.settingsCurrent, "DebugSettingsCurrent_After_RevertToDefault.xml");
        }


        private void buttonRevertToLastOpened_Click(object sender, EventArgs e)
        {
            //Plist.writeXml(SEBSettings.settingsCurrent, "DebugSettingsCurrent_Before_RevertToLastOpened.xml");
            LoadConfigurationFileIntoEditor(currentPathSebConfigFile);
            //Plist.writeXml(SEBSettings.settingsCurrent, "DebugSettingsCurrent_After_RevertToLastOpened.xml");
        }


        private void buttonOpenSettings_Click(object sender, EventArgs e)
        {
            // Set the default directory and file name in the File Dialog
            openFileDialogSebConfigFile.InitialDirectory = currentDireSebConfigFile;
            openFileDialogSebConfigFile.FileName         = currentFileSebConfigFile;

            // Get the user inputs in the File Dialog
            DialogResult fileDialogResult = openFileDialogSebConfigFile.ShowDialog();
            String       fileName         = openFileDialogSebConfigFile.FileName;

            // If the user clicked "Cancel", do nothing
            // If the user clicked "OK"    , read the settings from the configuration file
            if (fileDialogResult.Equals(DialogResult.Cancel)) return;
            if (fileDialogResult.Equals(DialogResult.OK    )) LoadConfigurationFileIntoEditor(fileName);
            // Generate Browser Exam Key of this new settings
            lastBrowserExamKey = SEBProtectionController.ComputeBrowserExamKey();
            // Display the new Browser Exam Key in Exam pane
            textBoxBrowserExamKey.Text = lastBrowserExamKey;
        }


        private void buttonSaveSettingsAs_Click(object sender, EventArgs e)
        {
            // Set the default directory and file name in the File Dialog
            saveFileDialogSebConfigFile.InitialDirectory = currentDireSebConfigFile;
            saveFileDialogSebConfigFile.FileName         = currentFileSebConfigFile;

            // Get the user inputs in the File Dialog
            DialogResult fileDialogResult = saveFileDialogSebConfigFile.ShowDialog();
            String       fileName         = saveFileDialogSebConfigFile.FileName;

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
            if (fileDialogResult.Equals(DialogResult.OK    )) SaveConfigurationFileFromEditor(fileName);
        }



        // ******************
        // Group "Appearance"
        // ******************
        private void radioButtonUseBrowserWindow_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonUseBrowserWindow.Checked == true)
                 SEBSettings.settingsCurrent[SEBSettings.KeyBrowserViewMode] = 0;
            else SEBSettings.settingsCurrent[SEBSettings.KeyBrowserViewMode] = 1;
        }

        private void radioButtonUseFullScreenMode_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonUseFullScreenMode.Checked == true)
                 SEBSettings.settingsCurrent[SEBSettings.KeyBrowserViewMode] = 1;
            else SEBSettings.settingsCurrent[SEBSettings.KeyBrowserViewMode] = 0;
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
            SEBSettings.settingsCurrent[SEBSettings.KeyDownloadDirectoryWin]     = path;
                                                  labelDownloadDirectoryWin.Text = path;
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

            // Update the widgets in the "Selected Process" group
            checkBoxPermittedProcessActive    .Checked = (Boolean)SEBSettings.permittedProcessData[SEBSettings.KeyActive];
            checkBoxPermittedProcessAutostart .Checked = (Boolean)SEBSettings.permittedProcessData[SEBSettings.KeyAutostart];
            checkBoxPermittedProcessAutohide  .Checked = (Boolean)SEBSettings.permittedProcessData[SEBSettings.KeyAutohide];
            checkBoxPermittedProcessAllowUser .Checked = (Boolean)SEBSettings.permittedProcessData[SEBSettings.KeyAllowUser];
            checkBoxPermittedProcessStrongKill.Checked = (Boolean)SEBSettings.permittedProcessData[SEBSettings.KeyStrongKill];
             listBoxPermittedProcessOS.SelectedIndex   =   (Int32)SEBSettings.permittedProcessData[SEBSettings.KeyOS];
             textBoxPermittedProcessTitle      .Text   =  (String)SEBSettings.permittedProcessData[SEBSettings.KeyTitle];
             textBoxPermittedProcessDescription.Text   =  (String)SEBSettings.permittedProcessData[SEBSettings.KeyDescription];
             textBoxPermittedProcessExecutable .Text   =  (String)SEBSettings.permittedProcessData[SEBSettings.KeyExecutable];
             textBoxPermittedProcessPath       .Text   =  (String)SEBSettings.permittedProcessData[SEBSettings.KeyPath];
             textBoxPermittedProcessIdentifier .Text   =  (String)SEBSettings.permittedProcessData[SEBSettings.KeyIdentifier];

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
             textBoxPermittedProcessPath       .Text   = "";
             textBoxPermittedProcessIdentifier .Text   = "";

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

            // Update the widget belonging to the current cell (in "Selected Process" group)
            if (column == IntColumnProcessActive    ) checkBoxPermittedProcessActive.Checked   = (Boolean)value;
            if (column == IntColumnProcessOS        )  listBoxPermittedProcessOS.SelectedIndex = (Int32  )value;
            if (column == IntColumnProcessExecutable)  textBoxPermittedProcessExecutable.Text  = (String )value;
            if (column == IntColumnProcessTitle     )  textBoxPermittedProcessTitle     .Text  = (String )value;
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
            processData[SEBSettings.KeyAutostart  ] = true;
            processData[SEBSettings.KeyAutohide   ] = true;
            processData[SEBSettings.KeyAllowUser  ] = true;
            processData[SEBSettings.KeyStrongKill ] = false;
            processData[SEBSettings.KeyOS         ] = IntWin;
            processData[SEBSettings.KeyTitle      ] = "";
            processData[SEBSettings.KeyDescription] = "";
            processData[SEBSettings.KeyExecutable ] = "";
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
            if (SEBSettings.permittedProcessIndex < 0) return;
            SEBSettings.permittedProcessList = (ListObj)SEBSettings.settingsCurrent     [SEBSettings.KeyPermittedProcesses];
            SEBSettings.permittedProcessData = (DictObj)SEBSettings.permittedProcessList[SEBSettings.permittedProcessIndex];
            SEBSettings.permittedProcessData[SEBSettings.KeyActive] = checkBoxPermittedProcessActive.Checked;
            Boolean                                         active  = checkBoxPermittedProcessActive.Checked;
            dataGridViewPermittedProcesses.Rows[SEBSettings.permittedProcessIndex].Cells[IntColumnProcessActive].Value = active.ToString();
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
            SEBSettings.permittedProcessData[SEBSettings.KeyAutohide] = checkBoxPermittedProcessAutohide.Checked;
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
            if (SEBSettings.permittedProcessIndex < 0) return;
            SEBSettings.permittedProcessList = (ListObj)SEBSettings.settingsCurrent     [SEBSettings.KeyPermittedProcesses];
            SEBSettings.permittedProcessData = (DictObj)SEBSettings.permittedProcessList[SEBSettings.permittedProcessIndex];
            SEBSettings.permittedProcessData[SEBSettings.KeyOS] = listBoxPermittedProcessOS.SelectedIndex;
            Int32                                           os  = listBoxPermittedProcessOS.SelectedIndex;
            dataGridViewPermittedProcesses.Rows[SEBSettings.permittedProcessIndex].Cells[IntColumnProcessOS].Value = StringOS[os];
        }

        private void textBoxPermittedProcessTitle_TextChanged(object sender, EventArgs e)
        {
            if (SEBSettings.permittedProcessIndex < 0) return;
            SEBSettings.permittedProcessList = (ListObj)SEBSettings.settingsCurrent     [SEBSettings.KeyPermittedProcesses];
            SEBSettings.permittedProcessData = (DictObj)SEBSettings.permittedProcessList[SEBSettings.permittedProcessIndex];
            SEBSettings.permittedProcessData[SEBSettings.KeyTitle] = textBoxPermittedProcessTitle.Text;
            String                                          title  = textBoxPermittedProcessTitle.Text;
            dataGridViewPermittedProcesses.Rows[SEBSettings.permittedProcessIndex].Cells[IntColumnProcessTitle].Value = title;
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
            if (SEBSettings.permittedProcessIndex < 0) return;
            SEBSettings.permittedProcessList = (ListObj)SEBSettings.settingsCurrent     [SEBSettings.KeyPermittedProcesses];
            SEBSettings.permittedProcessData = (DictObj)SEBSettings.permittedProcessList[SEBSettings.permittedProcessIndex];
            SEBSettings.permittedProcessData[SEBSettings.KeyExecutable] = textBoxPermittedProcessExecutable.Text;
            String                                          executable  = textBoxPermittedProcessExecutable.Text;
            dataGridViewPermittedProcesses.Rows[SEBSettings.permittedProcessIndex].Cells[IntColumnProcessExecutable].Value = executable;
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

            // Update the widgets in the "Selected Process" group
            checkBoxProhibitedProcessActive     .Checked = (Boolean)SEBSettings.prohibitedProcessData[SEBSettings.KeyActive];
            checkBoxProhibitedProcessCurrentUser.Checked = (Boolean)SEBSettings.prohibitedProcessData[SEBSettings.KeyCurrentUser];
            checkBoxProhibitedProcessStrongKill .Checked = (Boolean)SEBSettings.prohibitedProcessData[SEBSettings.KeyStrongKill];
             listBoxProhibitedProcessOS.SelectedIndex    =   (Int32)SEBSettings.prohibitedProcessData[SEBSettings.KeyOS];
             textBoxProhibitedProcessExecutable .Text    =  (String)SEBSettings.prohibitedProcessData[SEBSettings.KeyExecutable];
             textBoxProhibitedProcessDescription.Text    =  (String)SEBSettings.prohibitedProcessData[SEBSettings.KeyDescription];
             textBoxProhibitedProcessIdentifier .Text    =  (String)SEBSettings.prohibitedProcessData[SEBSettings.KeyIdentifier];
             textBoxProhibitedProcessUser       .Text    =  (String)SEBSettings.prohibitedProcessData[SEBSettings.KeyUser];
        }


        private void ClearProhibitedSelectedProcessGroup()
        {
            // Clear the widgets in the "Selected Process" group
            checkBoxProhibitedProcessActive     .Checked = true;
            checkBoxProhibitedProcessCurrentUser.Checked = true;
            checkBoxProhibitedProcessStrongKill .Checked = false;
             listBoxProhibitedProcessOS.SelectedIndex    = IntWin;
             textBoxProhibitedProcessExecutable .Text    = "";
             textBoxProhibitedProcessDescription.Text    = "";
             textBoxProhibitedProcessIdentifier .Text    = "";
             textBoxProhibitedProcessUser       .Text    = "";
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

            // Update the widget belonging to the current cell (in "Selected Process" group)
            if (column == IntColumnProcessActive     ) checkBoxProhibitedProcessActive.Checked   = (Boolean)value;
            if (column == IntColumnProcessOS         )  listBoxProhibitedProcessOS.SelectedIndex = (Int32  )value;
            if (column == IntColumnProcessExecutable )  textBoxProhibitedProcessExecutable .Text = (String )value;
            if (column == IntColumnProcessDescription)  textBoxProhibitedProcessDescription.Text = (String )value;
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
            if (SEBSettings.prohibitedProcessIndex < 0) return;
            SEBSettings.prohibitedProcessList = (ListObj)SEBSettings.settingsCurrent      [SEBSettings.KeyProhibitedProcesses];
            SEBSettings.prohibitedProcessData = (DictObj)SEBSettings.prohibitedProcessList[SEBSettings.prohibitedProcessIndex];
            SEBSettings.prohibitedProcessData[SEBSettings.KeyActive] = checkBoxProhibitedProcessActive.Checked;
            Boolean                                          active  = checkBoxProhibitedProcessActive.Checked;
            dataGridViewProhibitedProcesses.Rows[SEBSettings.prohibitedProcessIndex].Cells[IntColumnProcessActive].Value = active.ToString();
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
            if (SEBSettings.prohibitedProcessIndex < 0) return;
            SEBSettings.prohibitedProcessList = (ListObj)SEBSettings.settingsCurrent      [SEBSettings.KeyProhibitedProcesses];
            SEBSettings.prohibitedProcessData = (DictObj)SEBSettings.prohibitedProcessList[SEBSettings.prohibitedProcessIndex];
            SEBSettings.prohibitedProcessData[SEBSettings.KeyOS] = listBoxProhibitedProcessOS.SelectedIndex;
            Int32                                            os  = listBoxProhibitedProcessOS.SelectedIndex;
            dataGridViewProhibitedProcesses.Rows[SEBSettings.prohibitedProcessIndex].Cells[IntColumnProcessOS].Value = StringOS[os];
        }

        private void textBoxProhibitedProcessExecutable_TextChanged(object sender, EventArgs e)
        {
            if (SEBSettings.prohibitedProcessIndex < 0) return;
            SEBSettings.prohibitedProcessList = (ListObj)SEBSettings.settingsCurrent      [SEBSettings.KeyProhibitedProcesses];
            SEBSettings.prohibitedProcessData = (DictObj)SEBSettings.prohibitedProcessList[SEBSettings.prohibitedProcessIndex];
            SEBSettings.prohibitedProcessData[SEBSettings.KeyExecutable] = textBoxProhibitedProcessExecutable.Text;
            String                                           executable  = textBoxProhibitedProcessExecutable.Text;
            dataGridViewProhibitedProcesses.Rows[SEBSettings.prohibitedProcessIndex].Cells[IntColumnProcessExecutable].Value = executable;
        }

        private void textBoxProhibitedProcessDescription_TextChanged(object sender, EventArgs e)
        {
            if (SEBSettings.prohibitedProcessIndex < 0) return;
            SEBSettings.prohibitedProcessList = (ListObj)SEBSettings.settingsCurrent      [SEBSettings.KeyProhibitedProcesses];
            SEBSettings.prohibitedProcessData = (DictObj)SEBSettings.prohibitedProcessList[SEBSettings.prohibitedProcessIndex];
            SEBSettings.prohibitedProcessData[SEBSettings.KeyDescription] = textBoxProhibitedProcessDescription.Text;
            String                                           description  = textBoxProhibitedProcessDescription.Text;
            dataGridViewProhibitedProcesses.Rows[SEBSettings.prohibitedProcessIndex].Cells[IntColumnProcessDescription].Value = description;
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
        // Group "Network - Filter"
        // ************************
        private void checkBoxEnableURLFilter_CheckedChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsCurrent[SEBSettings.KeyEnableURLFilter] = checkBoxEnableURLFilter.Checked;
        }

        private void checkBoxEnableURLContentFilter_CheckedChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsCurrent[SEBSettings.KeyEnableURLContentFilter] = checkBoxEnableURLContentFilter.Checked;
        }


        private void dataGridViewURLFilterRules_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            // Skip the cell painting event if
            // - the URL filter rules have not yet been loaded
            // - the cell is no inner cell of the table
            if (urlFilterTableIsTitleRow.Count == 0) return;
            if (e.   RowIndex                   < 0) return;
            if (e.ColumnIndex                   < 0) return;

            // If the cell is disabled, paint over it
            if (urlFilterTableCellIsDisabled[e.RowIndex][e.ColumnIndex])
            {
                // Fill the cell using its background colour, and finish the paint event
                using (Brush backColorBrush = new SolidBrush(e.CellStyle.BackColor))
                {
                    e.Graphics.FillRectangle(backColorBrush, e.CellBounds);
                    e.Handled = true;
                }
            }
        }


        private void dataGridViewURLFilterRules_SelectionChanged(object sender, EventArgs e)
        {
            // CAUTION:
            // If a row was previously selected and the user clicks onto another row,
            // the SelectionChanged() event is fired TWICE!!!
            // The first time, it is only for UNselecting the old row,
            // so the SelectedRows.Count is ZERO, so ignore this event handler!
            // The second time, SelectedRows.Count is ONE.
            // Now you can set the widgets in the "Selected Process" groupBox.
            if (dataGridViewURLFilterRules.SelectedRows.Count != 1) return;

            // CAUTION:
            // Do ONLY set urlFilterTableRow here!
            // Do NOT  set urlFilterIsTitleRow, urlFilterRuleIndex, urlFilterActionIndex here,
            // because this event is called several times (e.g. whenever the user adds/removes
            // a rule/action to/from dataGridViewURLFilterRules). This caused some errors in the past.
            // Do only set these variables shortly before you need them in the other event handlers.
            urlFilterTableRow = dataGridViewURLFilterRules.SelectedRows[0].Index;
        }


        private void dataGridViewURLFilterRules_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            // When a CheckBox/ListBox/TextBox entry of a DataGridView table cell is edited,
            // immediately call the CellValueChanged() event.
            if (dataGridViewURLFilterRules.IsCurrentCellDirty)
                dataGridViewURLFilterRules.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }


        private void dataGridViewURLFilterRules_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            // Get the current cell where the user has changed a value
            int row    = dataGridViewURLFilterRules.CurrentCellAddress.Y;
            int column = dataGridViewURLFilterRules.CurrentCellAddress.X;

            // At the beginning, row = -1 and column = -1, so skip this event
            if (row    < 0) return;
            if (column < 0) return;

            // Get the changed value of the current cell
            object value = dataGridViewURLFilterRules.CurrentCell.EditedFormattedValue;

            // Convert the selected "Action" ListBox entry from String to Integer
            if (column == IntColumnURLFilterRuleAction)
            {
                     if ((String)value == StringBlock) value = IntBlock;
                else if ((String)value == StringAllow) value = IntAllow;
                else if ((String)value == StringSkip ) value = IntSkip;
                else if ((String)value == StringAnd  ) value = IntAnd;
                else if ((String)value == StringOr   ) value = IntOr;
            }

            // Determine if the selected row is a title row or action row.
            // Determine which rule and action belong to the selected row.
                        urlFilterTableRow        = row;
                        urlFilterTableRowIsTitle = urlFilterTableIsTitleRow [urlFilterTableRow];
            SEBSettings.urlFilterRuleIndex       = urlFilterTableRuleIndex  [urlFilterTableRow];
            SEBSettings.urlFilterActionIndex     = urlFilterTableActionIndex[urlFilterTableRow];

            // Get the rule data belonging to the current row
            SEBSettings.urlFilterRuleList = (ListObj)SEBSettings.settingsCurrent  [SEBSettings.KeyURLFilterRules];
            SEBSettings.urlFilterRuleData = (DictObj)SEBSettings.urlFilterRuleList[SEBSettings.urlFilterRuleIndex];

            // Update the rule data belonging to the current cell
            if (urlFilterTableRowIsTitle)
            {
                if (column == IntColumnURLFilterRuleActive    ) SEBSettings.urlFilterRuleData[SEBSettings.KeyActive    ] = (Boolean)value;
                if (column == IntColumnURLFilterRuleExpression) SEBSettings.urlFilterRuleData[SEBSettings.KeyExpression] = (String )value;
              //if (column == IntColumnURLFilterRuleShow      ) urlFilterTableShowRule[SEBSettings.urlFilterRuleIndex  ] = (Boolean)value;
              //if (column == IntColumnURLFilterRuleShow      ) UpdateTableOfURLFilterRules();
            }
            else
            {
                // Get the action data belonging to the current cell
                SEBSettings.urlFilterActionList = (ListObj)SEBSettings.urlFilterRuleData  [SEBSettings.KeyRuleActions];
                SEBSettings.urlFilterActionData = (DictObj)SEBSettings.urlFilterActionList[SEBSettings.urlFilterActionIndex];

                if (column == IntColumnURLFilterRuleActive    ) SEBSettings.urlFilterActionData[SEBSettings.KeyActive    ] = (Boolean)value;
                if (column == IntColumnURLFilterRuleRegex     ) SEBSettings.urlFilterActionData[SEBSettings.KeyRegex     ] = (Boolean)value;
                if (column == IntColumnURLFilterRuleExpression) SEBSettings.urlFilterActionData[SEBSettings.KeyExpression] = (String )value;
                if (column == IntColumnURLFilterRuleAction    ) SEBSettings.urlFilterActionData[SEBSettings.KeyAction    ] = (Int32  )value;
            }
        }


        private void dataGridViewURLFilterRules_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Get the current cell where the user has changed a value
            int row    = dataGridViewURLFilterRules.CurrentCellAddress.Y;
            int column = dataGridViewURLFilterRules.CurrentCellAddress.X;

            // At the beginning, row = -1 and column = -1, so skip this event
            if (row    < 0) return;
            if (column < 0) return;

            // Get the changed value of the current cell
            object value = dataGridViewURLFilterRules.CurrentCell.EditedFormattedValue;

            // Determine if the selected row is a title row or action row.
            // Determine which rule and action belong to the selected row.
                        urlFilterTableRow        = row;
                        urlFilterTableRowIsTitle = urlFilterTableIsTitleRow [urlFilterTableRow];
            SEBSettings.urlFilterRuleIndex       = urlFilterTableRuleIndex  [urlFilterTableRow];
            SEBSettings.urlFilterActionIndex     = urlFilterTableActionIndex[urlFilterTableRow];

            // Check if the button "Collapse" or "Expand" was pressed
            if (urlFilterTableRowIsTitle)
            {
                // Convert the selected "Show" Button value from String to Boolean
                if (column == IntColumnURLFilterRuleShow)
                {
                         if ((String)value == StringCollapse) value = false;
                    else if ((String)value == StringExpand  ) value = true;

                    // If "Collapse" was pressed, set Show flag of this rule to false.
                    // If "Expand"   was pressed, set Show flag of this rule to true.
                    // Update the URL filter table according to the new rule flags.
                    urlFilterTableShowRule[SEBSettings.urlFilterRuleIndex] = (Boolean)value;
                    UpdateTableOfURLFilterRules();
                }
            }
        }


        private void InsertPasteRuleAction(int operation, int location)
        {
            // Get the rule list
            SEBSettings.urlFilterRuleList = (ListObj)SEBSettings.settingsCurrent[SEBSettings.KeyURLFilterRules];

            if (SEBSettings.urlFilterRuleList.Count > 0)
            {
                // Determine if the selected row is a title row or action row.
                // Determine which rule and action belong to the selected row.
                            urlFilterTableRow        = dataGridViewURLFilterRules.SelectedRows[0].Index;
                            urlFilterTableRowIsTitle = urlFilterTableIsTitleRow [urlFilterTableRow];
                SEBSettings.urlFilterRuleIndex       = urlFilterTableRuleIndex  [urlFilterTableRow];
                SEBSettings.urlFilterActionIndex     = urlFilterTableActionIndex[urlFilterTableRow];
            }
            else
            {
                // If rule list was empty before, enable it
                            urlFilterTableRow        =  0;
                            urlFilterTableRowIsTitle =  true;
                SEBSettings.urlFilterRuleIndex       =  0;
                SEBSettings.urlFilterActionIndex     = -1;
            }

            // If the user clicked onto a TITLE row (RULE),
            // add a new rule BEFORE or AFTER the current rule.
            if (urlFilterTableRowIsTitle)
            {
                // If the rule is added AFTER current selection, increment the rule index
                // (exception: when rule list was empty, rule index becomes 0 in any case)
                if ((location == IntLocationAfter) && (SEBSettings.urlFilterRuleList.Count > 0))
                    SEBSettings.urlFilterRuleIndex++;

                // Load default rule for Insert operation.
                // Load stored  rule for Paste  operation.
                if (operation == IntOperationInsert) SEBSettings.urlFilterRuleData = SEBSettings.urlFilterRuleDataDefault;
                if (operation == IntOperationPaste ) SEBSettings.urlFilterRuleData = SEBSettings.urlFilterRuleDataStorage;

                // INSERT or PASTE new rule into rule list at correct position index
                SEBSettings.urlFilterRuleList     .Insert(SEBSettings.urlFilterRuleIndex, SEBSettings.urlFilterRuleData);
                            urlFilterTableShowRule.Insert(SEBSettings.urlFilterRuleIndex, true);
            }
            // If the user clicked onto an ACTION row,
            // add a new action BEFORE or AFTER the current action.
            else
            {
                // Get the action list
                SEBSettings.urlFilterRuleData   = (DictObj)SEBSettings.urlFilterRuleList[SEBSettings.urlFilterRuleIndex];
                SEBSettings.urlFilterActionList = (ListObj)SEBSettings.urlFilterRuleData[SEBSettings.KeyRuleActions];

                // If the action is added AFTER current selection, increment the action index
                if (location == IntLocationAfter)
                    SEBSettings.urlFilterActionIndex++;

                // Load default action for Insert operation.
                // Load stored  action for Paste  operation.
                if (operation == IntOperationInsert) SEBSettings.urlFilterActionData = SEBSettings.urlFilterActionDataDefault;
                if (operation == IntOperationPaste ) SEBSettings.urlFilterActionData = SEBSettings.urlFilterActionDataStorage;

                // INSERT or PASTE new action into action list at correct position index
                SEBSettings.urlFilterActionList.Insert(SEBSettings.urlFilterActionIndex, SEBSettings.urlFilterActionData);
            }

            // Update the table of URL Filter Rules
            UpdateTableOfURLFilterRules();
        }


        private void CopyCutDeleteRuleAction(int operation)
        {
            // Get the rule list
            SEBSettings.urlFilterRuleList = (ListObj)SEBSettings.settingsCurrent[SEBSettings.KeyURLFilterRules];

            if (SEBSettings.urlFilterRuleList.Count > 0)
            {
                // Determine if the selected row is a title row or action row.
                // Determine which rule and action belong to the selected row.
                            urlFilterTableRow        = dataGridViewURLFilterRules.SelectedRows[0].Index;
                            urlFilterTableRowIsTitle = urlFilterTableIsTitleRow [urlFilterTableRow];
                SEBSettings.urlFilterRuleIndex       = urlFilterTableRuleIndex  [urlFilterTableRow];
                SEBSettings.urlFilterActionIndex     = urlFilterTableActionIndex[urlFilterTableRow];
            }
            else
            {
                // If rule list is empty, abort since nothing can be deleted anymore
                return;
            }

            // If the user clicked onto a TITLE row (RULE), delete this rule
            if (urlFilterTableRowIsTitle)
            {
                if ((operation == IntOperationCopy) || (operation == IntOperationCut))
                {
                    // Store currently selected rule for later Paste operation
                    SEBSettings.urlFilterRuleDataStorage = (DictObj)SEBSettings.urlFilterRuleList[SEBSettings.urlFilterRuleIndex];
                }

                if ((operation == IntOperationDelete) || (operation == IntOperationCut))
                {
                    // Delete rule from rule list at position index
                        SEBSettings.urlFilterRuleList     .RemoveAt(SEBSettings.urlFilterRuleIndex);
                                    urlFilterTableShowRule.RemoveAt(SEBSettings.urlFilterRuleIndex);
                    if (SEBSettings.urlFilterRuleIndex ==           SEBSettings.urlFilterRuleList.Count)
                        SEBSettings.urlFilterRuleIndex--;
                }
            }
            // If the user clicked onto an ACTION row, delete this action
            else
            {
                // Get the action list
                SEBSettings.urlFilterRuleData   = (DictObj)SEBSettings.urlFilterRuleList[SEBSettings.urlFilterRuleIndex];
                SEBSettings.urlFilterActionList = (ListObj)SEBSettings.urlFilterRuleData[SEBSettings.KeyRuleActions];

                if ((operation == IntOperationCopy) || (operation == IntOperationCut))
                {
                    // Store currently selected action for later Paste operation
                    SEBSettings.urlFilterActionDataStorage = (DictObj)SEBSettings.urlFilterActionList[SEBSettings.urlFilterActionIndex];
                }

                if ((operation == IntOperationDelete) || (operation == IntOperationCut))
                {
                    // Delete action from action list at position index
                        SEBSettings.urlFilterActionList.RemoveAt(SEBSettings.urlFilterActionIndex);
                    if (SEBSettings.urlFilterActionIndex ==      SEBSettings.urlFilterActionList.Count)
                        SEBSettings.urlFilterActionIndex--;
                }
            }

            // Update the table of URL Filter Rules
            UpdateTableOfURLFilterRules();
        }


        private void buttonInsertBeforeSelected_Click(object sender, EventArgs e)
        {
            InsertPasteRuleAction(IntOperationInsert, IntLocationBefore);
        }

        private void buttonInsertAfterSelected_Click(object sender, EventArgs e)
        {
            InsertPasteRuleAction(IntOperationInsert, IntLocationAfter);
        }

        private void buttonPasteBeforeSelected_Click(object sender, EventArgs e)
        {
            InsertPasteRuleAction(IntOperationPaste, IntLocationBefore);
        }

        private void buttonPasteAfterSelected_Click(object sender, EventArgs e)
        {
            InsertPasteRuleAction(IntOperationPaste, IntLocationAfter);
        }

        private void buttonCopySelected_Click(object sender, EventArgs e)
        {
            CopyCutDeleteRuleAction(IntOperationCopy);
        }

        private void buttonCutSelected_Click(object sender, EventArgs e)
        {
            CopyCutDeleteRuleAction(IntOperationCut);
        }

        private void buttonDeleteSelected_Click(object sender, EventArgs e)
        {
            CopyCutDeleteRuleAction(IntOperationDelete);
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


        private void dataGridViewBypassedProxies_SelectionChanged(object sender, EventArgs e)
        {
            // CAUTION:
            // If a row was previously selected and the user clicks onto another row,
            // the SelectionChanged() event is fired TWICE!!!
            // The first time, it is only for UNselecting the old row,
            // so the SelectedRows.Count is ZERO, so ignore this event handler!
            // The second time, SelectedRows.Count is ONE.
            // Now you can set the widgets in the "Selected Process" groupBox.

            if (dataGridViewBypassedProxies.SelectedRows.Count != 1) return;
            SEBSettings.bypassedProxyIndex = dataGridViewBypassedProxies.SelectedRows[0].Index;
        }


        private void dataGridViewBypassedProxies_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            // When a CheckBox/ListBox/TextBox entry of a DataGridView table cell is edited,
            // immediately call the CellValueChanged() event.
            if (dataGridViewBypassedProxies.IsCurrentCellDirty)
                dataGridViewBypassedProxies.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }


        private void dataGridViewBypassedProxies_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            // Get the current cell where the user has changed a value
            int row    = dataGridViewBypassedProxies.CurrentCellAddress.Y;
            int column = dataGridViewBypassedProxies.CurrentCellAddress.X;

            // At the beginning, row = -1 and column = -1, so skip this event
            if (row    < 0) return;
            if (column < 0) return;

            // Get the changed value of the current cell
            object value = dataGridViewBypassedProxies.CurrentCell.EditedFormattedValue;

            // Get the data of the bypassed proxy belonging to the cell (row)
            SEBSettings.proxiesData = (DictObj)SEBSettings.settingsCurrent[SEBSettings.KeyProxies];

            SEBSettings.bypassedProxyIndex = row;
            SEBSettings.bypassedProxyList  = (ListObj)SEBSettings.proxiesData[SEBSettings.KeyExceptionsList];

            // Update the certificate data belonging to the current cell
            if (column == IntColumnDomainHostPort) SEBSettings.bypassedProxyList[SEBSettings.bypassedProxyIndex] = (String)value;
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

        private void checkBoxCreateNewDesktop_CheckedChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsCurrent[SEBSettings.KeyCreateNewDesktop] = checkBoxCreateNewDesktop.Checked;
        }

        private void checkBoxKillExplorerShell_CheckedChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsCurrent[SEBSettings.KeyKillExplorerShell] = checkBoxKillExplorerShell.Checked;
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
        }



        // ****************
        // Group "Registry"
        // ****************
        private void radioButtonPreviousValuesFromFile_CheckedChanged(object sender, EventArgs e)
        {
            groupBoxOutsideSeb.Visible = (radioButtonInsideValuesManually.Checked == true);
            groupBoxOutsideSeb.Enabled = (radioButtonInsideValuesManually.Checked == true);
        }

        private void radioButtonEnvironmentValues_CheckedChanged(object sender, EventArgs e)
        {
            groupBoxOutsideSeb.Visible = true;
            groupBoxOutsideSeb.Enabled = (radioButtonInsideValuesManually.Checked == true);
        }

        private void radioButtonInsideValuesManually_CheckedChanged(object sender, EventArgs e)
        {
            groupBoxOutsideSeb.Visible = true;
            groupBoxOutsideSeb.Enabled = (radioButtonInsideValuesManually.Checked == true);
        }



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
        // Group "Outside SEB"
        // *******************
        private void checkBoxOutsideSebEnableSwitchUser_CheckedChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsCurrent[SEBSettings.KeyOutsideSebEnableSwitchUser] = checkBoxOutsideSebEnableSwitchUser.Checked;
        }

        private void checkBoxOutsideSebEnableLockThisComputer_CheckedChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsCurrent[SEBSettings.KeyOutsideSebEnableLockThisComputer] = checkBoxOutsideSebEnableLockThisComputer.Checked;
        }

        private void checkBoxOutsideSebEnableChangeAPassword_CheckedChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsCurrent[SEBSettings.KeyOutsideSebEnableChangeAPassword] = checkBoxOutsideSebEnableChangeAPassword.Checked;
        }

        private void checkBoxOutsideSebEnableStartTaskManager_CheckedChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsCurrent[SEBSettings.KeyOutsideSebEnableStartTaskManager] = checkBoxOutsideSebEnableStartTaskManager.Checked;
        }

        private void checkBoxOutsideSebEnableLogOff_CheckedChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsCurrent[SEBSettings.KeyOutsideSebEnableLogOff] = checkBoxOutsideSebEnableLogOff.Checked;
        }

        private void checkBoxOutsideSebEnableShutDown_CheckedChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsCurrent[SEBSettings.KeyOutsideSebEnableShutDown] = checkBoxOutsideSebEnableShutDown.Checked;
        }

        private void checkBoxOutsideSebEnableEaseOfAccess_CheckedChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsCurrent[SEBSettings.KeyOutsideSebEnableEaseOfAccess] = checkBoxOutsideSebEnableEaseOfAccess.Checked;
        }

        private void checkBoxOutsideSebEnableVmWareClientShade_CheckedChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsCurrent[SEBSettings.KeyOutsideSebEnableVmWareClientShade] = checkBoxOutsideSebEnableVmWareClientShade.Checked;
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

        private void checkBoxEnableStartMenu_CheckedChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsCurrent[SEBSettings.KeyEnableStartMenu] = checkBoxEnableStartMenu.Checked;
        }

        private void checkBoxEnableRightMouse_CheckedChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsCurrent[SEBSettings.KeyEnableRightMouse] = checkBoxEnableRightMouse.Checked;
        }

        private void checkBoxEnablePrintScreen_CheckedChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsCurrent[SEBSettings.KeyEnablePrintScreen] = checkBoxEnablePrintScreen.Checked;
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

        private void radioButtonPreviousValuesFromFile_CheckedChanged_1(object sender, EventArgs e)
        {

        }



    } // end of   class     SebWindowsConfigForm
}     // end of   namespace SebWindowsConfig
