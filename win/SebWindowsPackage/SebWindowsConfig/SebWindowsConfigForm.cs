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
using PlistCS;



namespace SebWindowsConfig
{
    public partial class SebWindowsConfigForm : Form
    {

        // ***********
        //
        // Constructor
        //
        // ***********

        public SebWindowsConfigForm()
        {
            InitializeComponent();

            // Set all the default values for the Plist structure "SEBSettings.settingsNew"
            SEBSettings.BuildUpDefaultSettings();

            // Initialise the global variables for the GUI widgets
            InitialiseGlobalVariablesForGUIWidgets();

            // Initialise the GUI widgets themselves
            InitialiseGUIWidgets();

            // IMPORTANT:
            // Create a second dictionary "new settings"
            // and copy all default settings to the new settings.
            // This must be done BEFORE any config file is loaded
            // and assures that every (key, value) pair is contained
            // in the "new" and "def" dictionaries,
            // even if the loaded "tmp" dictionary does NOT contain every pair.

            SEBSettings.settingsNew.Clear();
            SEBSettings.CopySettingsArrays    (SEBSettings.StateDef   , SEBSettings.StateNew);
            SEBSettings.CopySettingsDictionary(SEBSettings.settingsDef, SEBSettings.settingsNew);

            SEBSettings.PrintSettingsDictionary(SEBSettings.settingsDef, "SettingsDef.txt");
            SEBSettings.PrintSettingsDictionary(SEBSettings.settingsNew, "SettingsNew.txt");

            // When starting up, set the widgets to the default values
            UpdateAllWidgetsOfProgram();

            // Try to open the configuration file ("SebClient.ini/xml/seb")
            // given in the local directory (where SebWindowsConfig.exe was called)
            currentDireSebConfigFile = Directory.GetCurrentDirectory();
            currentFileSebConfigFile = "";
            currentPathSebConfigFile = "";

            defaultDireSebConfigFile = Directory.GetCurrentDirectory();
            defaultFileSebConfigFile =                  SEBSettings.DefaultSebConfigXml;
            defaultPathSebConfigFile = Path.GetFullPath(SEBSettings.DefaultSebConfigXml);

            // Read the settings from the standard configuration file??? Currently not
            //OpenConfigurationFile(defaultPathSebConfigFile);

            openFileDialogSebConfigFile.InitialDirectory = Environment.CurrentDirectory;
            saveFileDialogSebConfigFile.InitialDirectory = Environment.CurrentDirectory;
          //folderBrowserDialogDownloadDirectoryWin.RootFolder = Environment.SpecialFolder.DesktopDirectory;
          //folderBrowserDialogLogDirectoryWin     .RootFolder = Environment.SpecialFolder.MyDocuments;

        } // end of contructor   SebWindowsConfigForm()




        // *************************************************
        // Open the configuration file and read the settings
        // *************************************************
        private Boolean LoadConfigurationFileIntoEditor(String fileName)
        {
            // Read the file into "tmp" settings
            if (!SEBSettings.ReadSebConfigurationFile(fileName)) return false;

            // If the settings could be read from file,
            // recreate "def" settings and "new" settings
            SEBSettings.RestoreDefaultAndNewSettings();

            // And merge "tmp" settings into "new" settings
            SEBSettings.CopySettingsArrays    (SEBSettings.StateTmp   , SEBSettings.StateNew);
            SEBSettings.CopySettingsDictionary(SEBSettings.settingsTmp, SEBSettings.settingsNew);

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
            SEBSettings.urlFilterRuleList = (List<object>)SEBSettings.settingsNew[SEBSettings.MessageURLFilterRules];

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
            //Plist.writeXml(SEBSettings.settingsNew, "DebugSettingsNew_in_OpenConfigurationFile.xml");
            //PrintSettingsDictionary(SEBSettings.settingsTmp, "SettingsTmp.txt");
            //PrintSettingsDictionary(SEBSettings.settingsNew, "SettingsNew.txt");
            return true;
        }



        // ********************************************************
        // Write the settings to the configuration file and save it
        // ********************************************************
        private Boolean SaveConfigurationFileFromEditor(String fileName)
        {
            // Write the "new" settings to file
            if (!SEBSettings.WriteSebConfigurationFile(fileName)) return false;

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
            SEBSettings.urlFilterRuleList = (List<object>)SEBSettings.settingsNew[SEBSettings.MessageURLFilterRules];

            // Clear the table itself
            dataGridViewURLFilterRules.Enabled = (SEBSettings.urlFilterRuleList.Count > 0);
            dataGridViewURLFilterRules.Rows.Clear();

            int row = 0;

            // Add URL Filter Rules of currently opened file to DataGridView
            for (int ruleIndex = 0; ruleIndex < SEBSettings.urlFilterRuleList.Count; ruleIndex++)
            {
                SEBSettings.urlFilterRuleData   = (Dictionary<string, object>)SEBSettings.urlFilterRuleList[ruleIndex];
                Boolean active                  = (Boolean                   )SEBSettings.urlFilterRuleData[SEBSettings.MessageActive];
                String  expression              = (String                    )SEBSettings.urlFilterRuleData[SEBSettings.MessageExpression];
                SEBSettings.urlFilterActionList = (List<object>              )SEBSettings.urlFilterRuleData[SEBSettings.MessageRuleActions];

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
                    SEBSettings.urlFilterActionData = (Dictionary<string, object>)SEBSettings.urlFilterActionList[actionIndex];

                    Boolean Active     = (Boolean)SEBSettings.urlFilterActionData[SEBSettings.MessageActive];
                    Boolean Regex      = (Boolean)SEBSettings.urlFilterActionData[SEBSettings.MessageRegex];
                    String  Expression = (String )SEBSettings.urlFilterActionData[SEBSettings.MessageExpression];
                    Int32   Action     = (Int32  )SEBSettings.urlFilterActionData[SEBSettings.MessageAction];

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
            textBoxStartURL            .Text   =  (String)SEBSettings.settingsNew[SEBSettings.MessageStartURL];
            textBoxSebServerURL        .Text   =  (String)SEBSettings.settingsNew[SEBSettings.MessageSebServerURL];
          //textBoxAdminPassword       .Text   =  (String)SEBSettings.settingsNew[SEBSettings.MessageAdminPassword];
          //textBoxConfirmAdminPassword.Text   =  (String)SEBSettings.settingsNew[SEBSettings.MessageConfirmAdminPassword];
            textBoxHashedAdminPassword .Text   =  (String)SEBSettings.settingsNew[SEBSettings.MessageHashedAdminPassword];
            checkBoxAllowQuit         .Checked = (Boolean)SEBSettings.settingsNew[SEBSettings.MessageAllowQuit];
            checkBoxIgnoreQuitPassword.Checked = (Boolean)SEBSettings.settingsNew[SEBSettings.MessageIgnoreQuitPassword];
          //textBoxQuitPassword        .Text   =  (String)SEBSettings.settingsNew[SEBSettings.MessageQuitPassword];
          //textBoxConfirmQuitPassword .Text   =  (String)SEBSettings.settingsNew[SEBSettings.MessageConfirmQuitPassword];
            textBoxHashedQuitPassword  .Text   =  (String)SEBSettings.settingsNew[SEBSettings.MessageHashedQuitPassword];
            listBoxExitKey1.SelectedIndex      =     (int)SEBSettings.settingsNew[SEBSettings.MessageExitKey1];
            listBoxExitKey2.SelectedIndex      =     (int)SEBSettings.settingsNew[SEBSettings.MessageExitKey2];
            listBoxExitKey3.SelectedIndex      =     (int)SEBSettings.settingsNew[SEBSettings.MessageExitKey3];

            // Group "Config File"
            radioButtonStartingAnExam     .Checked =    ((int)SEBSettings.settingsNew[SEBSettings.MessageSebConfigPurpose] == 0);
            radioButtonConfiguringAClient .Checked =    ((int)SEBSettings.settingsNew[SEBSettings.MessageSebConfigPurpose] == 1);
            checkBoxAllowPreferencesWindow.Checked = (Boolean)SEBSettings.settingsNew[SEBSettings.MessageAllowPreferencesWindow];
            comboBoxCryptoIdentity.SelectedIndex   =          SEBSettings.settingsInt[SEBSettings.StateNew, SEBSettings.ValueCryptoIdentity];
             textBoxSettingsPassword       .Text   =  (String)SEBSettings.settingsNew[SEBSettings.MessageSettingsPassword];
           //textBoxConfirmSettingsPassword.Text   =  (String)SEBSettings.settingsNew[SEBSettings.MessageConfirmSettingsPassword];
           //textBoxHashedSettingsPassword .Text   =  (String)SEBSettings.settingsNew[SEBSettings.MessageHashedSettingsPassword];

            // Group "Appearance"
            radioButtonUseBrowserWindow       .Checked     =    ((int)SEBSettings.settingsNew[SEBSettings.MessageBrowserViewMode] == 0);
            radioButtonUseFullScreenMode      .Checked     =    ((int)SEBSettings.settingsNew[SEBSettings.MessageBrowserViewMode] == 1);
            comboBoxMainBrowserWindowWidth    .Text        =  (String)SEBSettings.settingsNew[SEBSettings.MessageMainBrowserWindowWidth];
            comboBoxMainBrowserWindowHeight   .Text        =  (String)SEBSettings.settingsNew[SEBSettings.MessageMainBrowserWindowHeight];
             listBoxMainBrowserWindowPositioning.SelectedIndex = (int)SEBSettings.settingsNew[SEBSettings.MessageMainBrowserWindowPositioning];
            checkBoxEnableBrowserWindowToolbar.Checked     = (Boolean)SEBSettings.settingsNew[SEBSettings.MessageEnableBrowserWindowToolbar];
            checkBoxHideBrowserWindowToolbar  .Checked     = (Boolean)SEBSettings.settingsNew[SEBSettings.MessageHideBrowserWindowToolbar];
            checkBoxShowMenuBar               .Checked     = (Boolean)SEBSettings.settingsNew[SEBSettings.MessageShowMenuBar];
            checkBoxShowTaskBar               .Checked     = (Boolean)SEBSettings.settingsNew[SEBSettings.MessageShowTaskBar];
            comboBoxTaskBarHeight             .Text        =  (String)SEBSettings.settingsNew[SEBSettings.MessageTaskBarHeight].ToString();

            // Group "Browser"
             listBoxOpenLinksHTML .SelectedIndex =     (int)SEBSettings.settingsNew[SEBSettings.MessageNewBrowserWindowByLinkPolicy];
             listBoxOpenLinksJava .SelectedIndex =     (int)SEBSettings.settingsNew[SEBSettings.MessageNewBrowserWindowByScriptPolicy];
            checkBoxBlockLinksHTML.Checked       = (Boolean)SEBSettings.settingsNew[SEBSettings.MessageNewBrowserWindowByLinkBlockForeign];
            checkBoxBlockLinksJava.Checked       = (Boolean)SEBSettings.settingsNew[SEBSettings.MessageNewBrowserWindowByScriptBlockForeign];

            comboBoxNewBrowserWindowWidth      .Text          = (String)SEBSettings.settingsNew[SEBSettings.MessageNewBrowserWindowByLinkWidth ];
            comboBoxNewBrowserWindowHeight     .Text          = (String)SEBSettings.settingsNew[SEBSettings.MessageNewBrowserWindowByLinkHeight];
             listBoxNewBrowserWindowPositioning.SelectedIndex =    (int)SEBSettings.settingsNew[SEBSettings.MessageNewBrowserWindowByLinkPositioning];

            checkBoxEnablePlugIns           .Checked =   (Boolean)SEBSettings.settingsNew[SEBSettings.MessageEnablePlugIns];
            checkBoxEnableJava              .Checked =   (Boolean)SEBSettings.settingsNew[SEBSettings.MessageEnableJava];
            checkBoxEnableJavaScript        .Checked =   (Boolean)SEBSettings.settingsNew[SEBSettings.MessageEnableJavaScript];
            checkBoxBlockPopUpWindows       .Checked =   (Boolean)SEBSettings.settingsNew[SEBSettings.MessageBlockPopUpWindows];
            checkBoxAllowBrowsingBackForward.Checked =   (Boolean)SEBSettings.settingsNew[SEBSettings.MessageAllowBrowsingBackForward];
            checkBoxUseSebWithoutBrowser    .Checked = !((Boolean)SEBSettings.settingsNew[SEBSettings.MessageEnableSebBrowser]);
            // BEWARE: you must invert this value since "Use Without" is "Not Enable"!

            // Group "Down/Uploads"
            checkBoxAllowDownUploads.Checked           = (Boolean)SEBSettings.settingsNew[SEBSettings.MessageAllowDownUploads];
            checkBoxOpenDownloads   .Checked           = (Boolean)SEBSettings.settingsNew[SEBSettings.MessageOpenDownloads];
            checkBoxDownloadPDFFiles.Checked           = (Boolean)SEBSettings.settingsNew[SEBSettings.MessageDownloadPDFFiles];
            labelDownloadDirectoryWin.Text             =  (String)SEBSettings.settingsNew[SEBSettings.MessageDownloadDirectoryWin];
             listBoxChooseFileToUploadPolicy.SelectedIndex = (int)SEBSettings.settingsNew[SEBSettings.MessageChooseFileToUploadPolicy];

            // Group "Exam"
           //textBoxBrowserExamKey    .Text    =  (String)SEBSettings.settingsNew[SEBSettings.MessageBrowserExamKey];
             textBoxQuitURL           .Text    =  (String)SEBSettings.settingsNew[SEBSettings.MessageQuitURL];
            checkBoxCopyBrowserExamKey.Checked = (Boolean)SEBSettings.settingsNew[SEBSettings.MessageCopyBrowserExamKey];
            checkBoxSendBrowserExamKey.Checked = (Boolean)SEBSettings.settingsNew[SEBSettings.MessageSendBrowserExamKey];

            // Group "Applications"
            checkBoxMonitorProcesses         .Checked = (Boolean)SEBSettings.settingsNew[SEBSettings.MessageMonitorProcesses];
            checkBoxAllowSwitchToApplications.Checked = (Boolean)SEBSettings.settingsNew[SEBSettings.MessageAllowSwitchToApplications];
            checkBoxAllowFlashFullscreen     .Checked = (Boolean)SEBSettings.settingsNew[SEBSettings.MessageAllowFlashFullscreen];


            // Group "Applications - Permitted/Prohibited Processes"
            // Group "Network      -    Filter/Certificates/Proxies"

            // Update the lists for the DataGridViews
            SEBSettings.   permittedProcessList   = (List<object>)SEBSettings.settingsNew[SEBSettings.MessagePermittedProcesses];
            SEBSettings.  prohibitedProcessList   = (List<object>)SEBSettings.settingsNew[SEBSettings.MessageProhibitedProcesses];
            SEBSettings.embeddedCertificateList   = (List<object>)SEBSettings.settingsNew[SEBSettings.MessageEmbeddedCertificates];
            SEBSettings.proxiesData = (Dictionary<string, object>)SEBSettings.settingsNew[SEBSettings.MessageProxies];

            SEBSettings.bypassedProxyList = (List<object>)SEBSettings.proxiesData[SEBSettings.MessageExceptionsList];

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
                SEBSettings.permittedProcessData = (Dictionary<string, object>)SEBSettings.permittedProcessList[index];
                Boolean active     = (Boolean)SEBSettings.permittedProcessData[SEBSettings.MessageActive];
                Int32   os         = (Int32  )SEBSettings.permittedProcessData[SEBSettings.MessageOS];
                String  executable = (String )SEBSettings.permittedProcessData[SEBSettings.MessageExecutable];
                String  title      = (String )SEBSettings.permittedProcessData[SEBSettings.MessageTitle];
                dataGridViewPermittedProcesses.Rows.Add(active, StringOS[os], executable, title);
            }

            // Add Prohibited Processes of currently opened file to DataGridView
            for (int index = 0; index < SEBSettings.prohibitedProcessList.Count; index++)
            {
                SEBSettings.prohibitedProcessData =  (Dictionary<string, object>)SEBSettings.prohibitedProcessList[index];
                Boolean active      = (Boolean)SEBSettings.prohibitedProcessData[SEBSettings.MessageActive];
                Int32   os          = (Int32  )SEBSettings.prohibitedProcessData[SEBSettings.MessageOS];
                String  executable  = (String )SEBSettings.prohibitedProcessData[SEBSettings.MessageExecutable];
                String  description = (String )SEBSettings.prohibitedProcessData[SEBSettings.MessageDescription];
                dataGridViewProhibitedProcesses.Rows.Add(active, StringOS[os], executable, description);
            }

            // Add URL Filter Rules of currently opened file to DataGridView
            UpdateTableOfURLFilterRules();

            // Add Embedded Certificates of Certificate Store to DataGridView
            for (int index = 0; index < SEBSettings.embeddedCertificateList.Count; index++)
            {
                SEBSettings.embeddedCertificateData = (Dictionary<string, object>)SEBSettings.embeddedCertificateList[index];
                String         data = (String)SEBSettings.embeddedCertificateData[SEBSettings.MessageCertificateData];
                Int32          type = (Int32 )SEBSettings.embeddedCertificateData[SEBSettings.MessageType];
                String         name = (String)SEBSettings.embeddedCertificateData[SEBSettings.MessageName];
                dataGridViewEmbeddedCertificates.Rows.Add(StringCertificateType[type], name);
            }
/*
            // Get the "Enabled" boolean values of current "proxies" dictionary
            BooleanProxyProtocolEnabled[IntProxyAutoDiscovery    ] = (Boolean)SEBSettings.proxiesData[SEBSettings.MessageAutoDiscoveryEnabled];
            BooleanProxyProtocolEnabled[IntProxyAutoConfiguration] = (Boolean)SEBSettings.proxiesData[SEBSettings.MessageAutoConfigurationEnabled];
            BooleanProxyProtocolEnabled[IntProxyHTTP             ] = (Boolean)SEBSettings.proxiesData[SEBSettings.MessageHTTPEnable];
            BooleanProxyProtocolEnabled[IntProxyHTTPS            ] = (Boolean)SEBSettings.proxiesData[SEBSettings.MessageHTTPSEnable];
            BooleanProxyProtocolEnabled[IntProxyFTP              ] = (Boolean)SEBSettings.proxiesData[SEBSettings.MessageFTPEnable];
            BooleanProxyProtocolEnabled[IntProxySOCKS            ] = (Boolean)SEBSettings.proxiesData[SEBSettings.MessageSOCKSEnable];
            BooleanProxyProtocolEnabled[IntProxyRTSP             ] = (Boolean)SEBSettings.proxiesData[SEBSettings.MessageRTSPEnable];
*/
            // Get the "Enabled" boolean values of current "proxies" dictionary.
            // Add Proxy Protocols of currently opened file to DataGridView.
            for (int index = 0; index < NumProxyProtocols; index++)
            {
                Boolean enable = (Boolean)SEBSettings.proxiesData[MessageProxyProtocolEnableKey[index]];
                String  type   = (String )                      StringProxyProtocolTableCaption[index];
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
            checkBoxEnableURLFilter       .Checked = (Boolean)SEBSettings.settingsNew[SEBSettings.MessageEnableURLFilter];
            checkBoxEnableURLContentFilter.Checked = (Boolean)SEBSettings.settingsNew[SEBSettings.MessageEnableURLContentFilter];

            // Group "Network - Certificates"

            // Group "Network - Proxies"
            radioButtonUseSystemProxySettings.Checked =    ((int)SEBSettings.settingsNew[SEBSettings.MessageProxySettingsPolicy] == 0);
            radioButtonUseSebProxySettings   .Checked =    ((int)SEBSettings.settingsNew[SEBSettings.MessageProxySettingsPolicy] == 1);

            textBoxAutoProxyConfigurationURL .Text    =  (String)SEBSettings.proxiesData[SEBSettings.MessageAutoConfigurationURL];
            checkBoxExcludeSimpleHostnames   .Checked = (Boolean)SEBSettings.proxiesData[SEBSettings.MessageExcludeSimpleHostnames];
            checkBoxUsePassiveFTPMode        .Checked = (Boolean)SEBSettings.proxiesData[SEBSettings.MessageFTPPassive];

            // Group "Security"
             listBoxSebServicePolicy.SelectedIndex =     (int)SEBSettings.settingsNew[SEBSettings.MessageSebServicePolicy];
            checkBoxAllowVirtualMachine.Checked    = (Boolean)SEBSettings.settingsNew[SEBSettings.MessageAllowVirtualMachine];
            checkBoxCreateNewDesktop   .Checked    = (Boolean)SEBSettings.settingsNew[SEBSettings.MessageCreateNewDesktop];
            checkBoxKillExplorerShell  .Checked    = (Boolean)SEBSettings.settingsNew[SEBSettings.MessageKillExplorerShell];
            checkBoxAllowUserSwitching .Checked    = (Boolean)SEBSettings.settingsNew[SEBSettings.MessageAllowUserSwitching];
            checkBoxEnableLogging      .Checked    = (Boolean)SEBSettings.settingsNew[SEBSettings.MessageEnableLogging];
            labelLogDirectoryWin       .Text       =  (String)SEBSettings.settingsNew[SEBSettings.MessageLogDirectoryWin];

            // Group "Registry"
            checkBoxInsideSebEnableSwitchUser       .Checked = (Boolean)SEBSettings.settingsNew[SEBSettings.MessageInsideSebEnableSwitchUser];
            checkBoxInsideSebEnableLockThisComputer .Checked = (Boolean)SEBSettings.settingsNew[SEBSettings.MessageInsideSebEnableLockThisComputer];
            checkBoxInsideSebEnableChangeAPassword  .Checked = (Boolean)SEBSettings.settingsNew[SEBSettings.MessageInsideSebEnableChangeAPassword];
            checkBoxInsideSebEnableStartTaskManager .Checked = (Boolean)SEBSettings.settingsNew[SEBSettings.MessageInsideSebEnableStartTaskManager];
            checkBoxInsideSebEnableLogOff           .Checked = (Boolean)SEBSettings.settingsNew[SEBSettings.MessageInsideSebEnableLogOff];
            checkBoxInsideSebEnableShutDown         .Checked = (Boolean)SEBSettings.settingsNew[SEBSettings.MessageInsideSebEnableShutDown];
            checkBoxInsideSebEnableEaseOfAccess     .Checked = (Boolean)SEBSettings.settingsNew[SEBSettings.MessageInsideSebEnableEaseOfAccess];
            checkBoxInsideSebEnableVmWareClientShade.Checked = (Boolean)SEBSettings.settingsNew[SEBSettings.MessageInsideSebEnableVmWareClientShade];

            checkBoxOutsideSebEnableSwitchUser       .Checked = (Boolean)SEBSettings.settingsNew[SEBSettings.MessageOutsideSebEnableSwitchUser];
            checkBoxOutsideSebEnableLockThisComputer .Checked = (Boolean)SEBSettings.settingsNew[SEBSettings.MessageOutsideSebEnableLockThisComputer];
            checkBoxOutsideSebEnableChangeAPassword  .Checked = (Boolean)SEBSettings.settingsNew[SEBSettings.MessageOutsideSebEnableChangeAPassword];
            checkBoxOutsideSebEnableStartTaskManager .Checked = (Boolean)SEBSettings.settingsNew[SEBSettings.MessageOutsideSebEnableStartTaskManager];
            checkBoxOutsideSebEnableLogOff           .Checked = (Boolean)SEBSettings.settingsNew[SEBSettings.MessageOutsideSebEnableLogOff];
            checkBoxOutsideSebEnableShutDown         .Checked = (Boolean)SEBSettings.settingsNew[SEBSettings.MessageOutsideSebEnableShutDown];
            checkBoxOutsideSebEnableEaseOfAccess     .Checked = (Boolean)SEBSettings.settingsNew[SEBSettings.MessageOutsideSebEnableEaseOfAccess];
            checkBoxOutsideSebEnableVmWareClientShade.Checked = (Boolean)SEBSettings.settingsNew[SEBSettings.MessageOutsideSebEnableVmWareClientShade];

            // Group "Hooked Keys"
            checkBoxHookKeys.Checked = (Boolean)SEBSettings.settingsNew[SEBSettings.MessageHookKeys];

            checkBoxEnableEsc       .Checked = (Boolean)SEBSettings.settingsNew[SEBSettings.MessageEnableEsc];
            checkBoxEnableCtrlEsc   .Checked = (Boolean)SEBSettings.settingsNew[SEBSettings.MessageEnableCtrlEsc];
            checkBoxEnableAltEsc    .Checked = (Boolean)SEBSettings.settingsNew[SEBSettings.MessageEnableAltEsc];
            checkBoxEnableAltTab    .Checked = (Boolean)SEBSettings.settingsNew[SEBSettings.MessageEnableAltTab];
            checkBoxEnableAltF4     .Checked = (Boolean)SEBSettings.settingsNew[SEBSettings.MessageEnableAltF4];
            checkBoxEnableStartMenu .Checked = (Boolean)SEBSettings.settingsNew[SEBSettings.MessageEnableStartMenu];
            checkBoxEnableRightMouse.Checked = (Boolean)SEBSettings.settingsNew[SEBSettings.MessageEnableRightMouse];

            checkBoxEnableF1 .Checked = (Boolean)SEBSettings.settingsNew[SEBSettings.MessageEnableF1];
            checkBoxEnableF2 .Checked = (Boolean)SEBSettings.settingsNew[SEBSettings.MessageEnableF2];
            checkBoxEnableF3 .Checked = (Boolean)SEBSettings.settingsNew[SEBSettings.MessageEnableF3];
            checkBoxEnableF4 .Checked = (Boolean)SEBSettings.settingsNew[SEBSettings.MessageEnableF4];
            checkBoxEnableF5 .Checked = (Boolean)SEBSettings.settingsNew[SEBSettings.MessageEnableF5];
            checkBoxEnableF6 .Checked = (Boolean)SEBSettings.settingsNew[SEBSettings.MessageEnableF6];
            checkBoxEnableF7 .Checked = (Boolean)SEBSettings.settingsNew[SEBSettings.MessageEnableF7];
            checkBoxEnableF8 .Checked = (Boolean)SEBSettings.settingsNew[SEBSettings.MessageEnableF8];
            checkBoxEnableF9 .Checked = (Boolean)SEBSettings.settingsNew[SEBSettings.MessageEnableF9];
            checkBoxEnableF10.Checked = (Boolean)SEBSettings.settingsNew[SEBSettings.MessageEnableF10];
            checkBoxEnableF11.Checked = (Boolean)SEBSettings.settingsNew[SEBSettings.MessageEnableF11];
            checkBoxEnableF12.Checked = (Boolean)SEBSettings.settingsNew[SEBSettings.MessageEnableF12];

            return;
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
            SEBSettings.settingsNew[SEBSettings.MessageStartURL] = textBoxStartURL.Text;
        }

        private void buttonPasteFromSavedClipboard_Click(object sender, EventArgs e)
        {

        }

        private void textBoxSebServerURL_TextChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsNew[SEBSettings.MessageSebServerURL] = textBoxSebServerURL.Text;
        }

        private void textBoxAdminPassword_TextChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsNew[SEBSettings.MessageAdminPassword] = textBoxAdminPassword.Text;
        }

        private void textBoxConfirmAdminPassword_TextChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsNew[SEBSettings.MessageConfirmAdminPassword] = textBoxConfirmAdminPassword.Text;
        }

        private void checkBoxAllowQuit_CheckedChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsNew[SEBSettings.MessageAllowQuit] = checkBoxAllowQuit.Checked;
        }

        private void checkBoxIgnoreQuitPassword_CheckedChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsNew[SEBSettings.MessageIgnoreQuitPassword] = checkBoxIgnoreQuitPassword.Checked;
        }


        private void textBoxQuitPassword_TextChanged(object sender, EventArgs e)
        {
            // Get the new quit password
            String newStringQuitPassword = textBoxQuitPassword.Text;
            String newStringQuitHashcode = "";

            // Password encryption using the SHA-256 hash algorithm
            SHA256        sha256Algorithm = new SHA256Managed();
          //HashAlgorithm sha256Algorithm = new SHA256CryptoServiceProvider();

            // Encrypt the new quit password
            byte[] passwordBytes = Encoding.UTF8.GetBytes(newStringQuitPassword);
            byte[] hashcodeBytes = sha256Algorithm.ComputeHash(passwordBytes);

            newStringQuitHashcode = BitConverter.ToString(hashcodeBytes);
            newStringQuitHashcode = newStringQuitHashcode.Replace("-", "");

            textBoxHashedQuitPassword.Text = newStringQuitHashcode;

            SEBSettings.settingsNew[SEBSettings.MessageQuitPassword      ] = newStringQuitPassword;
            SEBSettings.settingsNew[SEBSettings.MessageHashedQuitPassword] = newStringQuitHashcode;
        }


        private void textBoxConfirmQuitPassword_TextChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsNew[SEBSettings.MessageConfirmQuitPassword] = textBoxConfirmQuitPassword.Text;
        }

        private void listBoxExitKey1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Make sure that all three exit keys are different.
            // If selected key is already occupied, revert to previously selected key.
            if ((listBoxExitKey1.SelectedIndex == listBoxExitKey2.SelectedIndex) ||
                (listBoxExitKey1.SelectedIndex == listBoxExitKey3.SelectedIndex))
                 listBoxExitKey1.SelectedIndex =  (int)SEBSettings.settingsNew[SEBSettings.MessageExitKey1];
            SEBSettings.settingsNew[SEBSettings.MessageExitKey1] = listBoxExitKey1.SelectedIndex;
        }

        private void listBoxExitKey2_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Make sure that all three exit keys are different.
            // If selected key is already occupied, revert to previously selected key.
            if ((listBoxExitKey2.SelectedIndex == listBoxExitKey1.SelectedIndex) ||
                (listBoxExitKey2.SelectedIndex == listBoxExitKey3.SelectedIndex))
                 listBoxExitKey2.SelectedIndex =  (int)SEBSettings.settingsNew[SEBSettings.MessageExitKey2];
            SEBSettings.settingsNew[SEBSettings.MessageExitKey2] = listBoxExitKey2.SelectedIndex;
        }

        private void listBoxExitKey3_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Make sure that all three exit keys are different.
            // If selected key is already occupied, revert to previously selected key.
            if ((listBoxExitKey3.SelectedIndex == listBoxExitKey1.SelectedIndex) ||
                (listBoxExitKey3.SelectedIndex == listBoxExitKey2.SelectedIndex))
                 listBoxExitKey3.SelectedIndex =  (int)SEBSettings.settingsNew[SEBSettings.MessageExitKey3];
            SEBSettings.settingsNew[SEBSettings.MessageExitKey3] = listBoxExitKey3.SelectedIndex;
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

        private void buttonRestartSEB_Click(object sender, EventArgs e)
        {

        }



        // *******************
        // Group "Config File"
        // *******************
        private void radioButtonStartingAnExam_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonStartingAnExam.Checked == true)
                 SEBSettings.settingsNew[SEBSettings.MessageSebConfigPurpose] = 0;
            else SEBSettings.settingsNew[SEBSettings.MessageSebConfigPurpose] = 1;
        }

        private void radioButtonConfiguringAClient_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonConfiguringAClient.Checked == true)
                 SEBSettings.settingsNew[SEBSettings.MessageSebConfigPurpose] = 1;
            else SEBSettings.settingsNew[SEBSettings.MessageSebConfigPurpose] = 0;
        }

        private void checkBoxAllowPreferencesWindow_CheckedChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsNew[SEBSettings.MessageAllowPreferencesWindow] = checkBoxAllowPreferencesWindow.Checked;
        }

        private void comboBoxCryptoIdentity_SelectedIndexChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsInt[SEBSettings.StateNew, SEBSettings.ValueCryptoIdentity] = comboBoxCryptoIdentity.SelectedIndex;
            SEBSettings.settingsStr[SEBSettings.StateNew, SEBSettings.ValueCryptoIdentity] = comboBoxCryptoIdentity.Text;
        }

        private void comboBoxCryptoIdentity_TextUpdate(object sender, EventArgs e)
        {
            SEBSettings.settingsInt[SEBSettings.StateNew, SEBSettings.ValueCryptoIdentity] = comboBoxCryptoIdentity.SelectedIndex;
            SEBSettings.settingsStr[SEBSettings.StateNew, SEBSettings.ValueCryptoIdentity] = comboBoxCryptoIdentity.Text;
        }

        private void textBoxSettingsPassword_TextChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsNew[SEBSettings.MessageSettingsPassword] = textBoxSettingsPassword.Text;
        }

        private void textBoxConfirmSettingsPassword_TextChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsNew[SEBSettings.MessageConfirmSettingsPassword] = textBoxConfirmSettingsPassword.Text;
        }


        private void buttonDefaultSettings_Click(object sender, EventArgs e)
        {
            //Plist.writeXml(SEBSettings.settingsNew, "DebugSettingsNew_before_RevertToDefault.xml");
            //Plist.writeXml(SEBSettings.settingsNew, "DebugSettingsDef_before_RevertToDefault.xml");
            SEBSettings.RestoreDefaultAndNewSettings();
            UpdateAllWidgetsOfProgram();
            //Plist.writeXml(SEBSettings.settingsNew, "DebugSettingsNew_after_RevertToDefault.xml");
            //Plist.writeXml(SEBSettings.settingsNew, "DebugSettingsDef_after_RevertToDefault.xml");
        }


        private void buttonRevertToLastOpened_Click(object sender, EventArgs e)
        {
            //Plist.writeXml(SEBSettings.settingsNew, "DebugSettingsNew_before_RevertToLastOpened.xml");
            LoadConfigurationFileIntoEditor(currentPathSebConfigFile);
            //Plist.writeXml(SEBSettings.settingsNew, "DebugSettingsNew_after_RevertToLastOpened.xml");
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
            if (fileDialogResult.Equals(DialogResult.OK    )) SaveConfigurationFileFromEditor(fileName);
        }



        // ******************
        // Group "Appearance"
        // ******************
        private void radioButtonUseBrowserWindow_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonUseBrowserWindow.Checked == true)
                 SEBSettings.settingsNew[SEBSettings.MessageBrowserViewMode] = 0;
            else SEBSettings.settingsNew[SEBSettings.MessageBrowserViewMode] = 1;
        }

        private void radioButtonUseFullScreenMode_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonUseFullScreenMode.Checked == true)
                 SEBSettings.settingsNew[SEBSettings.MessageBrowserViewMode] = 1;
            else SEBSettings.settingsNew[SEBSettings.MessageBrowserViewMode] = 0;
        }

        private void comboBoxMainBrowserWindowWidth_SelectedIndexChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsInt[SEBSettings.StateNew, SEBSettings.ValueMainBrowserWindowWidth] = comboBoxMainBrowserWindowWidth.SelectedIndex;
            SEBSettings.settingsStr[SEBSettings.StateNew, SEBSettings.ValueMainBrowserWindowWidth] = comboBoxMainBrowserWindowWidth.Text;
          //SEBSettings.settingsNew[SEBSettings.MessageMainBrowserWindowWidth] = comboBoxMainBrowserWindowWidth.SelectedIndex;
            SEBSettings.settingsNew[SEBSettings.MessageMainBrowserWindowWidth] = comboBoxMainBrowserWindowWidth.Text;
        }

        private void comboBoxMainBrowserWindowWidth_TextUpdate(object sender, EventArgs e)
        {
            SEBSettings.settingsInt[SEBSettings.StateNew, SEBSettings.ValueMainBrowserWindowWidth] = comboBoxMainBrowserWindowWidth.SelectedIndex;
            SEBSettings.settingsStr[SEBSettings.StateNew, SEBSettings.ValueMainBrowserWindowWidth] = comboBoxMainBrowserWindowWidth.Text;
          //SEBSettings.settingsNew[SEBSettings.MessageMainBrowserWindowWidth] = comboBoxMainBrowserWindowWidth.SelectedIndex;
            SEBSettings.settingsNew[SEBSettings.MessageMainBrowserWindowWidth] = comboBoxMainBrowserWindowWidth.Text;
        }

        private void comboBoxMainBrowserWindowHeight_SelectedIndexChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsInt[SEBSettings.StateNew, SEBSettings.ValueMainBrowserWindowHeight] = comboBoxMainBrowserWindowHeight.SelectedIndex;
            SEBSettings.settingsStr[SEBSettings.StateNew, SEBSettings.ValueMainBrowserWindowHeight] = comboBoxMainBrowserWindowHeight.Text;
          //SEBSettings.settingsNew[SEBSettings.MessageMainBrowserWindowHeight] = comboBoxMainBrowserWindowHeight.SelectedIndex;
            SEBSettings.settingsNew[SEBSettings.MessageMainBrowserWindowHeight] = comboBoxMainBrowserWindowHeight.Text;
        }

        private void comboBoxMainBrowserWindowHeight_TextUpdate(object sender, EventArgs e)
        {
            SEBSettings.settingsInt[SEBSettings.StateNew, SEBSettings.ValueMainBrowserWindowHeight] = comboBoxMainBrowserWindowHeight.SelectedIndex;
            SEBSettings.settingsStr[SEBSettings.StateNew, SEBSettings.ValueMainBrowserWindowHeight] = comboBoxMainBrowserWindowHeight.Text;
          //SEBSettings.settingsNew[SEBSettings.MessageMainBrowserWindowHeight] = comboBoxMainBrowserWindowHeight.SelectedIndex;
            SEBSettings.settingsNew[SEBSettings.MessageMainBrowserWindowHeight] = comboBoxMainBrowserWindowHeight.Text;
        }

        private void listBoxMainBrowserWindowPositioning_SelectedIndexChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsNew[SEBSettings.MessageMainBrowserWindowPositioning] = listBoxMainBrowserWindowPositioning.SelectedIndex;
        }

        private void checkBoxEnableBrowserWindowToolbar_CheckedChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsNew[SEBSettings.MessageEnableBrowserWindowToolbar] = checkBoxEnableBrowserWindowToolbar.Checked;
            checkBoxHideBrowserWindowToolbar.Enabled                               = checkBoxEnableBrowserWindowToolbar.Checked;
        }

        private void checkBoxHideBrowserWindowToolbar_CheckedChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsNew[SEBSettings.MessageHideBrowserWindowToolbar] = checkBoxHideBrowserWindowToolbar.Checked;
        }

        private void checkBoxShowMenuBar_CheckedChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsNew[SEBSettings.MessageShowMenuBar] = checkBoxShowMenuBar.Checked;
        }

        private void checkBoxShowTaskBar_CheckedChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsNew[SEBSettings.MessageShowTaskBar] = checkBoxShowTaskBar.Checked;
            comboBoxTaskBarHeight.Enabled                           = checkBoxShowTaskBar.Checked;
        }

        private void comboBoxTaskBarHeight_SelectedIndexChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsInt[SEBSettings.StateNew, SEBSettings.ValueTaskBarHeight] = comboBoxTaskBarHeight.SelectedIndex;
            SEBSettings.settingsStr[SEBSettings.StateNew, SEBSettings.ValueTaskBarHeight] = comboBoxTaskBarHeight.Text;
          //SEBSettings.settingsNew[SEBSettings.MessageTaskBarHeight] = comboBoxTaskBarHeight.SelectedIndex;
            SEBSettings.settingsNew[SEBSettings.MessageTaskBarHeight] = Int32.Parse(comboBoxTaskBarHeight.Text);
        }



        // ***************
        // Group "Browser"
        // ***************
        private void listBoxOpenLinksHTML_SelectedIndexChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsNew[SEBSettings.MessageNewBrowserWindowByLinkPolicy] = listBoxOpenLinksHTML.SelectedIndex;
        }

        private void listBoxOpenLinksJava_SelectedIndexChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsNew[SEBSettings.MessageNewBrowserWindowByScriptPolicy] = listBoxOpenLinksJava.SelectedIndex;
        }

        private void checkBoxBlockLinksHTML_CheckedChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsNew[SEBSettings.MessageNewBrowserWindowByLinkBlockForeign] = checkBoxBlockLinksHTML.Checked;
        }

        private void checkBoxBlockLinksJava_CheckedChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsNew[SEBSettings.MessageNewBrowserWindowByScriptBlockForeign] = checkBoxBlockLinksJava.Checked;
        }

        private void comboBoxNewBrowserWindowWidth_SelectedIndexChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsInt[SEBSettings.StateNew, SEBSettings.ValueNewBrowserWindowByLinkWidth] = comboBoxNewBrowserWindowWidth.SelectedIndex;
            SEBSettings.settingsStr[SEBSettings.StateNew, SEBSettings.ValueNewBrowserWindowByLinkWidth] = comboBoxNewBrowserWindowWidth.Text;
          //SEBSettings.settingsNew[SEBSettings.MessageNewBrowserWindowByLinkWidth] = comboBoxNewBrowserWindowWidth.SelectedIndex;
            SEBSettings.settingsNew[SEBSettings.MessageNewBrowserWindowByLinkWidth] = comboBoxNewBrowserWindowWidth.Text;
        }

        private void comboBoxNewBrowserWindowWidth_TextUpdate(object sender, EventArgs e)
        {
            SEBSettings.settingsInt[SEBSettings.StateNew, SEBSettings.ValueNewBrowserWindowByLinkWidth] = comboBoxNewBrowserWindowWidth.SelectedIndex;
            SEBSettings.settingsStr[SEBSettings.StateNew, SEBSettings.ValueNewBrowserWindowByLinkWidth] = comboBoxNewBrowserWindowWidth.Text;
          //SEBSettings.settingsNew[SEBSettings.MessageNewBrowserWindowByLinkWidth] = comboBoxNewBrowserWindowWidth.SelectedIndex;
            SEBSettings.settingsNew[SEBSettings.MessageNewBrowserWindowByLinkWidth] = comboBoxNewBrowserWindowWidth.Text;
        }

        private void comboBoxNewBrowserWindowHeight_SelectedIndexChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsInt[SEBSettings.StateNew, SEBSettings.ValueNewBrowserWindowByLinkHeight] = comboBoxNewBrowserWindowHeight.SelectedIndex;
            SEBSettings.settingsStr[SEBSettings.StateNew, SEBSettings.ValueNewBrowserWindowByLinkHeight] = comboBoxNewBrowserWindowHeight.Text;
          //SEBSettings.settingsNew[SEBSettings.MessageNewBrowserWindowByLinkHeight] = comboBoxNewBrowserWindowHeight.SelectedIndex;
            SEBSettings.settingsNew[SEBSettings.MessageNewBrowserWindowByLinkHeight] = comboBoxNewBrowserWindowHeight.Text;
        }

        private void comboBoxNewBrowserWindowHeight_TextUpdate(object sender, EventArgs e)
        {
            SEBSettings.settingsInt[SEBSettings.StateNew, SEBSettings.ValueNewBrowserWindowByLinkHeight] = comboBoxNewBrowserWindowHeight.SelectedIndex;
            SEBSettings.settingsStr[SEBSettings.StateNew, SEBSettings.ValueNewBrowserWindowByLinkHeight] = comboBoxNewBrowserWindowHeight.Text;
          //SEBSettings.settingsNew[SEBSettings.MessageNewBrowserWindowByLinkHeight] = comboBoxNewBrowserWindowHeight.SelectedIndex;
            SEBSettings.settingsNew[SEBSettings.MessageNewBrowserWindowByLinkHeight] = comboBoxNewBrowserWindowHeight.Text;
        }

        private void listBoxNewBrowserWindowPositioning_SelectedIndexChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsNew[SEBSettings.MessageNewBrowserWindowByLinkPositioning] = listBoxNewBrowserWindowPositioning.SelectedIndex;
        }

        private void checkBoxEnablePlugins_CheckedChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsNew[SEBSettings.MessageEnablePlugIns] = checkBoxEnablePlugIns.Checked;
        }

        private void checkBoxEnableJava_CheckedChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsNew[SEBSettings.MessageEnableJava] = checkBoxEnableJava.Checked;
        }

        private void checkBoxEnableJavaScript_CheckedChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsNew[SEBSettings.MessageEnableJavaScript] = checkBoxEnableJavaScript.Checked;
        }

        private void checkBoxBlockPopUpWindows_CheckedChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsNew[SEBSettings.MessageBlockPopUpWindows] = checkBoxBlockPopUpWindows.Checked;
        }

        private void checkBoxAllowBrowsingBackForward_CheckedChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsNew[SEBSettings.MessageAllowBrowsingBackForward] = checkBoxAllowBrowsingBackForward.Checked;
        }

        // BEWARE: you must invert this value since "Use Without" is "Not Enable"!
        private void checkBoxUseSebWithoutBrowser_CheckedChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsNew[SEBSettings.MessageEnableSebBrowser] = !(checkBoxUseSebWithoutBrowser.Checked);
        }



        // ********************
        // Group "Down/Uploads"
        // ********************
        private void checkBoxAllowDownUploads_CheckedChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsNew[SEBSettings.MessageAllowDownUploads] = checkBoxAllowDownUploads.Checked;
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
            SEBSettings.settingsNew[SEBSettings.MessageDownloadDirectoryWin]     = path;
                             labelDownloadDirectoryWin.Text = path;
        }

        private void checkBoxOpenDownloads_CheckedChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsNew[SEBSettings.MessageOpenDownloads] = checkBoxOpenDownloads.Checked;
        }

        private void listBoxChooseFileToUploadPolicy_SelectedIndexChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsNew[SEBSettings.MessageChooseFileToUploadPolicy] = listBoxChooseFileToUploadPolicy.SelectedIndex;
        }

        private void checkBoxDownloadPDFFiles_CheckedChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsNew[SEBSettings.MessageDownloadPDFFiles] = checkBoxDownloadPDFFiles.Checked;
        }



        // ************
        // Group "Exam"
        // ************
        private void buttonGenerateBrowserExamKey_Click(object sender, EventArgs e)
        {

        }

        private void textBoxBrowserExamKey_TextChanged(object sender, EventArgs e)
        {
          //SEBSettings.settingsNew[SEBSettings.MessageBrowserExamKey] = textBoxBrowserExamKey.Text;
        }

        private void checkBoxCopyBrowserExamKey_CheckedChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsNew[SEBSettings.MessageCopyBrowserExamKey] = checkBoxCopyBrowserExamKey.Checked;
        }

        private void checkBoxSendBrowserExamKey_CheckedChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsNew[SEBSettings.MessageSendBrowserExamKey] = checkBoxSendBrowserExamKey.Checked;
        }

        private void textBoxQuitURL_TextChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsNew[SEBSettings.MessageQuitURL] = textBoxQuitURL.Text;
        }



        // ********************
        // Group "Applications"
        // ********************
        private void checkBoxMonitorProcesses_CheckedChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsNew[SEBSettings.MessageMonitorProcesses] = checkBoxMonitorProcesses.Checked;
        }


        // ******************************************
        // Group "Applications - Permitted Processes"
        // ******************************************
        private void checkBoxAllowSwitchToApplications_CheckedChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsNew[SEBSettings.MessageAllowSwitchToApplications] = checkBoxAllowSwitchToApplications.Checked;
            checkBoxAllowFlashFullscreen.Enabled                                  = checkBoxAllowSwitchToApplications.Checked;
        }

        private void checkBoxAllowFlashFullscreen_CheckedChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsNew[SEBSettings.MessageAllowFlashFullscreen] = checkBoxAllowFlashFullscreen.Checked;
        }


        private void LoadAndUpdatePermittedSelectedProcessGroup(int selectedProcessIndex)
        {
            // Get the process data of the selected process
            SEBSettings.permittedProcessList  =               (List<object>)SEBSettings.settingsNew         [SEBSettings.MessagePermittedProcesses];
            SEBSettings.permittedProcessData  = (Dictionary<string, object>)SEBSettings.permittedProcessList[selectedProcessIndex];
            SEBSettings.permittedArgumentList =               (List<object>)SEBSettings.permittedProcessData[SEBSettings.MessageArguments];

            // Update the widgets in the "Selected Process" group
            checkBoxPermittedProcessActive   .Checked = (Boolean)SEBSettings.permittedProcessData[SEBSettings.MessageActive];
            checkBoxPermittedProcessAutostart.Checked = (Boolean)SEBSettings.permittedProcessData[SEBSettings.MessageAutostart];
            checkBoxPermittedProcessAutohide .Checked = (Boolean)SEBSettings.permittedProcessData[SEBSettings.MessageAutohide];
            checkBoxPermittedProcessAllowUser.Checked = (Boolean)SEBSettings.permittedProcessData[SEBSettings.MessageAllowUser];
             listBoxPermittedProcessOS.SelectedIndex  =   (Int32)SEBSettings.permittedProcessData[SEBSettings.MessageOS];
             textBoxPermittedProcessTitle      .Text  =  (String)SEBSettings.permittedProcessData[SEBSettings.MessageTitle];
             textBoxPermittedProcessDescription.Text  =  (String)SEBSettings.permittedProcessData[SEBSettings.MessageDescription];
             textBoxPermittedProcessExecutable .Text  =  (String)SEBSettings.permittedProcessData[SEBSettings.MessageExecutable];
             textBoxPermittedProcessPath       .Text  =  (String)SEBSettings.permittedProcessData[SEBSettings.MessagePath];
             textBoxPermittedProcessIdentifier .Text  =  (String)SEBSettings.permittedProcessData[SEBSettings.MessageIdentifier];

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
                SEBSettings.permittedArgumentData = (Dictionary<string, object>)SEBSettings.permittedArgumentList[index];
                Boolean   active   = (Boolean)SEBSettings.permittedArgumentData[SEBSettings.MessageActive];
                String    argument = (String )SEBSettings.permittedArgumentData[SEBSettings.MessageArgument];
                dataGridViewPermittedProcessArguments.Rows.Add(active, argument);
            }

            // Get the selected argument data
            if  (SEBSettings.permittedArgumentList.Count > 0)
                 SEBSettings.permittedArgumentData = (Dictionary<string, object>)SEBSettings.permittedArgumentList[SEBSettings.permittedArgumentIndex];
        }


        private void ClearPermittedSelectedProcessGroup()
        {
            // Clear the widgets in the "Selected Process" group
            checkBoxPermittedProcessActive   .Checked = true;
            checkBoxPermittedProcessAutostart.Checked = true;
            checkBoxPermittedProcessAutohide .Checked = true;
            checkBoxPermittedProcessAllowUser.Checked = true;
             listBoxPermittedProcessOS.SelectedIndex  = IntWin;
             textBoxPermittedProcessTitle      .Text  = "";
             textBoxPermittedProcessDescription.Text  = "";
             textBoxPermittedProcessExecutable .Text  = "";
             textBoxPermittedProcessPath       .Text  = "";
             textBoxPermittedProcessIdentifier .Text  = "";

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
            SEBSettings.permittedProcessList  =               (List<object>)SEBSettings.settingsNew         [SEBSettings.MessagePermittedProcesses];
            SEBSettings.permittedProcessData  = (Dictionary<string, object>)SEBSettings.permittedProcessList[SEBSettings.permittedProcessIndex];

            // Update the process data belonging to the current cell
            if (column == IntColumnProcessActive    ) SEBSettings.permittedProcessData[SEBSettings.MessageActive    ] = (Boolean)value;
            if (column == IntColumnProcessOS        ) SEBSettings.permittedProcessData[SEBSettings.MessageOS        ] = (Int32  )value;
            if (column == IntColumnProcessExecutable) SEBSettings.permittedProcessData[SEBSettings.MessageExecutable] = (String )value;
            if (column == IntColumnProcessTitle     ) SEBSettings.permittedProcessData[SEBSettings.MessageTitle     ] = (String )value;

            // Update the widget belonging to the current cell (in "Selected Process" group)
            if (column == IntColumnProcessActive    ) checkBoxPermittedProcessActive.Checked   = (Boolean)value;
            if (column == IntColumnProcessOS        )  listBoxPermittedProcessOS.SelectedIndex = (Int32  )value;
            if (column == IntColumnProcessExecutable)  textBoxPermittedProcessExecutable.Text  = (String )value;
            if (column == IntColumnProcessTitle     )  textBoxPermittedProcessTitle     .Text  = (String )value;
        }


        private void buttonAddPermittedProcess_Click(object sender, EventArgs e)
        {
            // Get the process list
            SEBSettings.permittedProcessList = (List<object>)SEBSettings.settingsNew[SEBSettings.MessagePermittedProcesses];

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
            Dictionary<string, object> processData = new Dictionary<string, object>();

            processData[SEBSettings.MessageActive     ] = true;
            processData[SEBSettings.MessageAutostart  ] = true;
            processData[SEBSettings.MessageAutohide   ] = true;
            processData[SEBSettings.MessageAllowUser  ] = true;
            processData[SEBSettings.MessageOS         ] = IntWin;
            processData[SEBSettings.MessageTitle      ] = "";
            processData[SEBSettings.MessageDescription] = "";
            processData[SEBSettings.MessageExecutable ] = "";
            processData[SEBSettings.MessagePath       ] = "";
            processData[SEBSettings.MessageIdentifier ] = "";
            processData[SEBSettings.MessageArguments  ] = new List<object>();

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
            SEBSettings.permittedProcessList  = (List<object>)SEBSettings.settingsNew[SEBSettings.MessagePermittedProcesses];
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
            SEBSettings.permittedProcessList =               (List<object>)SEBSettings.settingsNew          [SEBSettings.MessagePermittedProcesses];
            SEBSettings.permittedProcessData = (Dictionary<string, object>)SEBSettings.permittedProcessList [SEBSettings.permittedProcessIndex];
            SEBSettings.permittedProcessData[SEBSettings.MessageActive] = checkBoxPermittedProcessActive.Checked;
            Boolean                                             active  = checkBoxPermittedProcessActive.Checked;
            dataGridViewPermittedProcesses.Rows[SEBSettings.permittedProcessIndex].Cells[IntColumnProcessActive].Value = active.ToString();
        }

        private void checkBoxPermittedProcessAutostart_CheckedChanged(object sender, EventArgs e)
        {
            if (SEBSettings.permittedProcessIndex < 0) return;
            SEBSettings.permittedProcessList =               (List<object>)SEBSettings.settingsNew          [SEBSettings.MessagePermittedProcesses];
            SEBSettings.permittedProcessData = (Dictionary<string, object>)SEBSettings.permittedProcessList [SEBSettings.permittedProcessIndex];
            SEBSettings.permittedProcessData[SEBSettings.MessageAutostart] =   checkBoxPermittedProcessAutostart.Checked;
        }

        private void checkBoxPermittedProcessAutohide_CheckedChanged(object sender, EventArgs e)
        {
            if (SEBSettings.permittedProcessIndex < 0) return;
            SEBSettings.permittedProcessList =               (List<object>)SEBSettings.settingsNew          [SEBSettings.MessagePermittedProcesses];
            SEBSettings.permittedProcessData = (Dictionary<string, object>)SEBSettings.permittedProcessList [SEBSettings.permittedProcessIndex];
            SEBSettings.permittedProcessData[SEBSettings.MessageAutohide] =    checkBoxPermittedProcessAutohide.Checked;
        }

        private void checkBoxPermittedProcessAllowUser_CheckedChanged(object sender, EventArgs e)
        {
            if (SEBSettings.permittedProcessIndex < 0) return;
            SEBSettings.permittedProcessList =               (List<object>)SEBSettings.settingsNew          [SEBSettings.MessagePermittedProcesses];
            SEBSettings.permittedProcessData = (Dictionary<string, object>)SEBSettings.permittedProcessList [SEBSettings.permittedProcessIndex];
            SEBSettings.permittedProcessData[SEBSettings.MessageAllowUser] = checkBoxPermittedProcessAllowUser.Checked;
        }

        private void listBoxPermittedProcessOS_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SEBSettings.permittedProcessIndex < 0) return;
            SEBSettings.permittedProcessList =               (List<object>)SEBSettings.settingsNew          [SEBSettings.MessagePermittedProcesses];
            SEBSettings.permittedProcessData = (Dictionary<string, object>)SEBSettings.permittedProcessList [SEBSettings.permittedProcessIndex];
            SEBSettings.permittedProcessData[SEBSettings.MessageOS] =           listBoxPermittedProcessOS.SelectedIndex;
            Int32                                               os  =           listBoxPermittedProcessOS.SelectedIndex;
            dataGridViewPermittedProcesses.Rows[SEBSettings.permittedProcessIndex].Cells[IntColumnProcessOS].Value = StringOS[os];
        }

        private void textBoxPermittedProcessTitle_TextChanged(object sender, EventArgs e)
        {
            if (SEBSettings.permittedProcessIndex < 0) return;
            SEBSettings.permittedProcessList =               (List<object>)SEBSettings.settingsNew          [SEBSettings.MessagePermittedProcesses];
            SEBSettings.permittedProcessData = (Dictionary<string, object>)SEBSettings.permittedProcessList [SEBSettings.permittedProcessIndex];
            SEBSettings.permittedProcessData[SEBSettings.MessageTitle] =        textBoxPermittedProcessTitle.Text;
            String                                              title  =        textBoxPermittedProcessTitle.Text;
            dataGridViewPermittedProcesses.Rows[SEBSettings.permittedProcessIndex].Cells[IntColumnProcessTitle].Value = title;
        }

        private void textBoxPermittedProcessDescription_TextChanged(object sender, EventArgs e)
        {
            if (SEBSettings.permittedProcessIndex < 0) return;
            SEBSettings.permittedProcessList =               (List<object>)SEBSettings.settingsNew          [SEBSettings.MessagePermittedProcesses];
            SEBSettings.permittedProcessData = (Dictionary<string, object>)SEBSettings.permittedProcessList [SEBSettings.permittedProcessIndex];
            SEBSettings.permittedProcessData[SEBSettings.MessageDescription] =  textBoxPermittedProcessDescription.Text;
        }

        private void textBoxPermittedProcessExecutable_TextChanged(object sender, EventArgs e)
        {
            if (SEBSettings.permittedProcessIndex < 0) return;
            SEBSettings.permittedProcessList =               (List<object>)SEBSettings.settingsNew          [SEBSettings.MessagePermittedProcesses];
            SEBSettings.permittedProcessData = (Dictionary<string, object>)SEBSettings.permittedProcessList [SEBSettings.permittedProcessIndex];
            SEBSettings.permittedProcessData[SEBSettings.MessageExecutable] =   textBoxPermittedProcessExecutable.Text;
            String                                              executable  =   textBoxPermittedProcessExecutable.Text;
            dataGridViewPermittedProcesses.Rows[SEBSettings.permittedProcessIndex].Cells[IntColumnProcessExecutable].Value = executable;
        }

        private void textBoxPermittedProcessPath_TextChanged(object sender, EventArgs e)
        {
            if (SEBSettings.permittedProcessIndex < 0) return;
            SEBSettings.permittedProcessList =               (List<object>)SEBSettings.settingsNew          [SEBSettings.MessagePermittedProcesses];
            SEBSettings.permittedProcessData = (Dictionary<string, object>)SEBSettings.permittedProcessList [SEBSettings.permittedProcessIndex];
            SEBSettings.permittedProcessData[SEBSettings.MessagePath] =         textBoxPermittedProcessPath.Text;
        }

        private void textBoxPermittedProcessIdentifier_TextChanged(object sender, EventArgs e)
        {
            if (SEBSettings.permittedProcessIndex < 0) return;
            SEBSettings.permittedProcessList =               (List<object>)SEBSettings.settingsNew          [SEBSettings.MessagePermittedProcesses];
            SEBSettings.permittedProcessData = (Dictionary<string, object>)SEBSettings.permittedProcessList [SEBSettings.permittedProcessIndex];
            SEBSettings.permittedProcessData[SEBSettings.MessageIdentifier] =   textBoxPermittedProcessIdentifier.Text;
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
            SEBSettings.permittedProcessList   =               (List<object>)SEBSettings.settingsNew          [SEBSettings.MessagePermittedProcesses];
            SEBSettings.permittedProcessData   = (Dictionary<string, object>)SEBSettings.permittedProcessList [SEBSettings.permittedProcessIndex];
            SEBSettings.permittedArgumentList  =               (List<object>)SEBSettings.permittedProcessData [SEBSettings.MessageArguments];
            SEBSettings.permittedArgumentData  = (Dictionary<string, object>)SEBSettings.permittedArgumentList[SEBSettings.permittedArgumentIndex];
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
            SEBSettings.permittedProcessList   =               (List<object>)SEBSettings.settingsNew          [SEBSettings.MessagePermittedProcesses];
            SEBSettings.permittedProcessData   = (Dictionary<string, object>)SEBSettings.permittedProcessList [SEBSettings.permittedProcessIndex];
            SEBSettings.permittedArgumentList  =               (List<object>)SEBSettings.permittedProcessData [SEBSettings.MessageArguments];
            SEBSettings.permittedArgumentData  = (Dictionary<string, object>)SEBSettings.permittedArgumentList[SEBSettings.permittedArgumentIndex];

            // Update the argument data belonging to the current cell
            if (column == IntColumnProcessActive  ) SEBSettings.permittedArgumentData[SEBSettings.MessageActive  ] = (Boolean)value;
            if (column == IntColumnProcessArgument) SEBSettings.permittedArgumentData[SEBSettings.MessageArgument] = (String )value;
        }


        private void buttonPermittedProcessAddArgument_Click(object sender, EventArgs e)
        {
            // Get the permitted argument list
            SEBSettings.permittedProcessList  =               (List<object>)SEBSettings.settingsNew          [SEBSettings.MessagePermittedProcesses];
            SEBSettings.permittedProcessData  = (Dictionary<string, object>)SEBSettings.permittedProcessList [SEBSettings.permittedProcessIndex];
            SEBSettings.permittedArgumentList =               (List<object>)SEBSettings.permittedProcessData [SEBSettings.MessageArguments];

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
            Dictionary<string, object> argumentData = new Dictionary<string, object>();

            argumentData[SEBSettings.MessageActive  ] = true;
            argumentData[SEBSettings.MessageArgument] = "";

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
            SEBSettings.permittedProcessList   =               (List<object>)SEBSettings.settingsNew          [SEBSettings.MessagePermittedProcesses];
            SEBSettings.permittedProcessData   = (Dictionary<string, object>)SEBSettings.permittedProcessList [SEBSettings.permittedProcessIndex];
            SEBSettings.permittedArgumentList  =               (List<object>)SEBSettings.permittedProcessData [SEBSettings.MessageArguments];

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
            SEBSettings.prohibitedProcessList  =               (List<object>)SEBSettings.settingsNew[SEBSettings.MessageProhibitedProcesses];
            SEBSettings.prohibitedProcessData  = (Dictionary<string, object>)SEBSettings.prohibitedProcessList[selectedProcessIndex];

            // Update the widgets in the "Selected Process" group
            checkBoxProhibitedProcessActive     .Checked = (Boolean)SEBSettings.prohibitedProcessData[SEBSettings.MessageActive];
            checkBoxProhibitedProcessCurrentUser.Checked = (Boolean)SEBSettings.prohibitedProcessData[SEBSettings.MessageCurrentUser];
            checkBoxProhibitedProcessStrongKill .Checked = (Boolean)SEBSettings.prohibitedProcessData[SEBSettings.MessageStrongKill];
             listBoxProhibitedProcessOS.SelectedIndex    =   (Int32)SEBSettings.prohibitedProcessData[SEBSettings.MessageOS];
             textBoxProhibitedProcessExecutable .Text    =  (String)SEBSettings.prohibitedProcessData[SEBSettings.MessageExecutable];
             textBoxProhibitedProcessDescription.Text    =  (String)SEBSettings.prohibitedProcessData[SEBSettings.MessageDescription];
             textBoxProhibitedProcessIdentifier .Text    =  (String)SEBSettings.prohibitedProcessData[SEBSettings.MessageIdentifier];
             textBoxProhibitedProcessUser       .Text    =  (String)SEBSettings.prohibitedProcessData[SEBSettings.MessageUser];
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
            SEBSettings.prohibitedProcessList  =               (List<object>)SEBSettings.settingsNew          [SEBSettings.MessageProhibitedProcesses];
            SEBSettings.prohibitedProcessData  = (Dictionary<string, object>)SEBSettings.prohibitedProcessList[SEBSettings.prohibitedProcessIndex];

            // Update the process data belonging to the current cell
            if (column == IntColumnProcessActive     ) SEBSettings.prohibitedProcessData[SEBSettings.MessageActive     ] = (Boolean)value;
            if (column == IntColumnProcessOS         ) SEBSettings.prohibitedProcessData[SEBSettings.MessageOS         ] = (Int32  )value;
            if (column == IntColumnProcessExecutable ) SEBSettings.prohibitedProcessData[SEBSettings.MessageExecutable ] = (String )value;
            if (column == IntColumnProcessDescription) SEBSettings.prohibitedProcessData[SEBSettings.MessageDescription] = (String )value;

            // Update the widget belonging to the current cell (in "Selected Process" group)
            if (column == IntColumnProcessActive     ) checkBoxProhibitedProcessActive.Checked   = (Boolean)value;
            if (column == IntColumnProcessOS         )  listBoxProhibitedProcessOS.SelectedIndex = (Int32  )value;
            if (column == IntColumnProcessExecutable )  textBoxProhibitedProcessExecutable .Text = (String )value;
            if (column == IntColumnProcessDescription)  textBoxProhibitedProcessDescription.Text = (String )value;
        }


        private void buttonAddProhibitedProcess_Click(object sender, EventArgs e)
        {
            // Get the process list
            SEBSettings.prohibitedProcessList = (List<object>)SEBSettings.settingsNew[SEBSettings.MessageProhibitedProcesses];

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
            Dictionary<string, object> processData = new Dictionary<string, object>();

            processData[SEBSettings.MessageActive     ] = true;
            processData[SEBSettings.MessageCurrentUser] = true;
            processData[SEBSettings.MessageStrongKill ] = false;
            processData[SEBSettings.MessageOS         ] = IntWin;
            processData[SEBSettings.MessageExecutable ] = "";
            processData[SEBSettings.MessageDescription] = "";
            processData[SEBSettings.MessageIdentifier ] = "";
            processData[SEBSettings.MessageUser       ] = "";

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
            SEBSettings.prohibitedProcessList  = (List<object>)SEBSettings.settingsNew[SEBSettings.MessageProhibitedProcesses];
            SEBSettings.prohibitedProcessList        .RemoveAt(SEBSettings.prohibitedProcessIndex);
            dataGridViewProhibitedProcesses.Rows     .RemoveAt(SEBSettings.prohibitedProcessIndex);

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
            SEBSettings.prohibitedProcessList =               (List<object>)SEBSettings.settingsNew          [SEBSettings.MessageProhibitedProcesses];
            SEBSettings.prohibitedProcessData = (Dictionary<string, object>)SEBSettings.prohibitedProcessList[SEBSettings.prohibitedProcessIndex];
            SEBSettings.prohibitedProcessData[SEBSettings.MessageActive] =      checkBoxProhibitedProcessActive.Checked;
            Boolean                                              active  =      checkBoxProhibitedProcessActive.Checked;
            dataGridViewProhibitedProcesses.Rows[SEBSettings.prohibitedProcessIndex].Cells[IntColumnProcessActive].Value = active.ToString();
        }

        private void checkBoxProhibitedProcessCurrentUser_CheckedChanged(object sender, EventArgs e)
        {
            if (SEBSettings.prohibitedProcessIndex < 0) return;
            SEBSettings.prohibitedProcessList =               (List<object>)SEBSettings.settingsNew          [SEBSettings.MessageProhibitedProcesses];
            SEBSettings.prohibitedProcessData = (Dictionary<string, object>)SEBSettings.prohibitedProcessList[SEBSettings.prohibitedProcessIndex];
            SEBSettings.prohibitedProcessData[SEBSettings.MessageCurrentUser] = checkBoxProhibitedProcessCurrentUser.Checked;
        }

        private void checkBoxProhibitedProcessStrongKill_CheckedChanged(object sender, EventArgs e)
        {
            if (SEBSettings.prohibitedProcessIndex < 0) return;
            SEBSettings.prohibitedProcessList =               (List<object>)SEBSettings.settingsNew          [SEBSettings.MessageProhibitedProcesses];
            SEBSettings.prohibitedProcessData = (Dictionary<string, object>)SEBSettings.prohibitedProcessList[SEBSettings.prohibitedProcessIndex];
            SEBSettings.prohibitedProcessData[SEBSettings.MessageStrongKill] =  checkBoxProhibitedProcessStrongKill.Checked;
        }

        private void listBoxProhibitedProcessOS_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SEBSettings.prohibitedProcessIndex < 0) return;
            SEBSettings.prohibitedProcessList =               (List<object>)SEBSettings.settingsNew          [SEBSettings.MessageProhibitedProcesses];
            SEBSettings.prohibitedProcessData = (Dictionary<string, object>)SEBSettings.prohibitedProcessList[SEBSettings.prohibitedProcessIndex];
            SEBSettings.prohibitedProcessData[SEBSettings.MessageOS] =           listBoxProhibitedProcessOS.SelectedIndex;
            Int32                                                os  =           listBoxProhibitedProcessOS.SelectedIndex;
            dataGridViewProhibitedProcesses.Rows[SEBSettings.prohibitedProcessIndex].Cells[IntColumnProcessOS].Value = StringOS[os];
        }

        private void textBoxProhibitedProcessExecutable_TextChanged(object sender, EventArgs e)
        {
            if (SEBSettings.prohibitedProcessIndex < 0) return;
            SEBSettings.prohibitedProcessList =               (List<object>)SEBSettings.settingsNew          [SEBSettings.MessageProhibitedProcesses];
            SEBSettings.prohibitedProcessData = (Dictionary<string, object>)SEBSettings.prohibitedProcessList[SEBSettings.prohibitedProcessIndex];
            SEBSettings.prohibitedProcessData[SEBSettings.MessageExecutable] =   textBoxProhibitedProcessExecutable.Text;
            String                                               executable  =   textBoxProhibitedProcessExecutable.Text;
            dataGridViewProhibitedProcesses.Rows[SEBSettings.prohibitedProcessIndex].Cells[IntColumnProcessExecutable].Value = executable;
        }

        private void textBoxProhibitedProcessDescription_TextChanged(object sender, EventArgs e)
        {
            if (SEBSettings.prohibitedProcessIndex < 0) return;
            SEBSettings.prohibitedProcessList =               (List<object>)SEBSettings.settingsNew          [SEBSettings.MessageProhibitedProcesses];
            SEBSettings.prohibitedProcessData = (Dictionary<string, object>)SEBSettings.prohibitedProcessList[SEBSettings.prohibitedProcessIndex];
            SEBSettings.prohibitedProcessData[SEBSettings.MessageDescription] =  textBoxProhibitedProcessDescription.Text;
            String                                               description  =  textBoxProhibitedProcessDescription.Text;
            dataGridViewProhibitedProcesses.Rows[SEBSettings.prohibitedProcessIndex].Cells[IntColumnProcessDescription].Value = description;
        }

        private void textBoxProhibitedProcessIdentifier_TextChanged(object sender, EventArgs e)
        {
            if (SEBSettings.prohibitedProcessIndex < 0) return;
            SEBSettings.prohibitedProcessList =               (List<object>)SEBSettings.settingsNew          [SEBSettings.MessageProhibitedProcesses];
            SEBSettings.prohibitedProcessData = (Dictionary<string, object>)SEBSettings.prohibitedProcessList[SEBSettings.prohibitedProcessIndex];
            SEBSettings.prohibitedProcessData[SEBSettings.MessageIdentifier] =   textBoxProhibitedProcessIdentifier.Text;
        }

        private void textBoxProhibitedProcessUser_TextChanged(object sender, EventArgs e)
        {
            if (SEBSettings.prohibitedProcessIndex < 0) return;
            SEBSettings.prohibitedProcessList =               (List<object>)SEBSettings.settingsNew          [SEBSettings.MessageProhibitedProcesses];
            SEBSettings.prohibitedProcessData = (Dictionary<string, object>)SEBSettings.prohibitedProcessList[SEBSettings.prohibitedProcessIndex];
            SEBSettings.prohibitedProcessData[SEBSettings.MessageUser] =         textBoxProhibitedProcessUser.Text;
        }

        private void buttonProhibitedProcessCodeSignature_Click(object sender, EventArgs e)
        {

        }



        // ************************
        // Group "Network - Filter"
        // ************************
        private void checkBoxEnableURLFilter_CheckedChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsNew[SEBSettings.MessageEnableURLFilter] = checkBoxEnableURLFilter.Checked;
        }

        private void checkBoxEnableURLContentFilter_CheckedChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsNew[SEBSettings.MessageEnableURLContentFilter] = checkBoxEnableURLContentFilter.Checked;
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
            SEBSettings.urlFilterRuleList =               (List<object>)SEBSettings.settingsNew      [SEBSettings.MessageURLFilterRules];
            SEBSettings.urlFilterRuleData = (Dictionary<string, object>)SEBSettings.urlFilterRuleList[SEBSettings.urlFilterRuleIndex];

            // Update the rule data belonging to the current cell
            if (urlFilterTableRowIsTitle)
            {
                if (column == IntColumnURLFilterRuleActive    ) SEBSettings.urlFilterRuleData[SEBSettings.MessageActive    ] = (Boolean)value;
                if (column == IntColumnURLFilterRuleExpression) SEBSettings.urlFilterRuleData[SEBSettings.MessageExpression] = (String )value;
              //if (column == IntColumnURLFilterRuleShow      ) urlFilterTableShowRule[SEBSettings.urlFilterRuleIndex] = (Boolean)value;
              //if (column == IntColumnURLFilterRuleShow      ) UpdateTableOfURLFilterRules();
            }
            else
            {
                // Get the action data belonging to the current cell
                SEBSettings.urlFilterActionList =               (List<object>)SEBSettings.urlFilterRuleData  [SEBSettings.MessageRuleActions];
                SEBSettings.urlFilterActionData = (Dictionary<string, object>)SEBSettings.urlFilterActionList[SEBSettings.urlFilterActionIndex];

                if (column == IntColumnURLFilterRuleActive    ) SEBSettings.urlFilterActionData[SEBSettings.MessageActive    ] = (Boolean)value;
                if (column == IntColumnURLFilterRuleRegex     ) SEBSettings.urlFilterActionData[SEBSettings.MessageRegex     ] = (Boolean)value;
                if (column == IntColumnURLFilterRuleExpression) SEBSettings.urlFilterActionData[SEBSettings.MessageExpression] = (String )value;
                if (column == IntColumnURLFilterRuleAction    ) SEBSettings.urlFilterActionData[SEBSettings.MessageAction    ] = (Int32  )value;
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
            SEBSettings.urlFilterRuleList = (List<object>)SEBSettings.settingsNew[SEBSettings.MessageURLFilterRules];

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
                if (operation == IntOperationInsert) SEBSettings.urlFilterRuleData = SEBSettings.urlFilterRuleDataDef;
                if (operation == IntOperationPaste ) SEBSettings.urlFilterRuleData = SEBSettings.urlFilterRuleDataStored;

                // INSERT or PASTE new rule into rule list at correct position index
                SEBSettings.urlFilterRuleList     .Insert(SEBSettings.urlFilterRuleIndex, SEBSettings.urlFilterRuleData);
                            urlFilterTableShowRule.Insert(SEBSettings.urlFilterRuleIndex, true);
            }
            // If the user clicked onto an ACTION row,
            // add a new action BEFORE or AFTER the current action.
            else
            {
                // Get the action list
                SEBSettings.urlFilterRuleData   = (Dictionary<string, object>)SEBSettings.urlFilterRuleList[SEBSettings.urlFilterRuleIndex];
                SEBSettings.urlFilterActionList =               (List<object>)SEBSettings.urlFilterRuleData[SEBSettings.MessageRuleActions];

                // If the action is added AFTER current selection, increment the action index
                if (location == IntLocationAfter)
                    SEBSettings.urlFilterActionIndex++;

                // Load default action for Insert operation.
                // Load stored  action for Paste  operation.
                if (operation == IntOperationInsert) SEBSettings.urlFilterActionData = SEBSettings.urlFilterActionDataDef;
                if (operation == IntOperationPaste ) SEBSettings.urlFilterActionData = SEBSettings.urlFilterActionDataStored;

                // INSERT or PASTE new action into action list at correct position index
                SEBSettings.urlFilterActionList.Insert(SEBSettings.urlFilterActionIndex, SEBSettings.urlFilterActionData);
            }

            // Update the table of URL Filter Rules
            UpdateTableOfURLFilterRules();
        }


        private void CopyCutDeleteRuleAction(int operation)
        {
            // Get the rule list
            SEBSettings.urlFilterRuleList = (List<object>)SEBSettings.settingsNew[SEBSettings.MessageURLFilterRules];

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
                    SEBSettings.urlFilterRuleDataStored = (Dictionary<string, object>)SEBSettings.urlFilterRuleList[SEBSettings.urlFilterRuleIndex];
                }

                if ((operation == IntOperationDelete) || (operation == IntOperationCut))
                {
                    // Delete rule from rule list at position index
                    SEBSettings.urlFilterRuleList     .RemoveAt(SEBSettings.urlFilterRuleIndex);
                                urlFilterTableShowRule.RemoveAt(SEBSettings.urlFilterRuleIndex);
                    if (SEBSettings.urlFilterRuleIndex == SEBSettings.urlFilterRuleList.Count)
                        SEBSettings.urlFilterRuleIndex--;
                }
            }
            // If the user clicked onto an ACTION row, delete this action
            else
            {
                // Get the action list
                SEBSettings.urlFilterRuleData   = (Dictionary<string, object>)SEBSettings.urlFilterRuleList[SEBSettings.urlFilterRuleIndex];
                SEBSettings.urlFilterActionList =               (List<object>)SEBSettings.urlFilterRuleData[SEBSettings.MessageRuleActions];

                if ((operation == IntOperationCopy) || (operation == IntOperationCut))
                {
                    // Store currently selected action for later Paste operation
                    SEBSettings.urlFilterActionDataStored = (Dictionary<string, object>)SEBSettings.urlFilterActionList[SEBSettings.urlFilterActionIndex];
                }

                if ((operation == IntOperationDelete) || (operation == IntOperationCut))
                {
                    // Delete action from action list at position index
                    SEBSettings.urlFilterActionList.RemoveAt(SEBSettings.urlFilterActionIndex);
                    if (SEBSettings.urlFilterActionIndex == SEBSettings.urlFilterActionList.Count)
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
            SEBSettings.embeddedCertificateList  =               (List<object>)SEBSettings.settingsNew            [SEBSettings.MessageEmbeddedCertificates];
            SEBSettings.embeddedCertificateData  = (Dictionary<string, object>)SEBSettings.embeddedCertificateList[SEBSettings.embeddedCertificateIndex];

            // Update the certificate data belonging to the current cell
            if (column == IntColumnCertificateType) SEBSettings.embeddedCertificateData[SEBSettings.MessageType] = (Int32  )value;
            if (column == IntColumnCertificateName) SEBSettings.embeddedCertificateData[SEBSettings.MessageName] = (String )value;
        }


        private void buttonRemoveEmbeddedCertificate_Click(object sender, EventArgs e)
        {
            if (dataGridViewEmbeddedCertificates.SelectedRows.Count != 1) return;
            SEBSettings.embeddedCertificateIndex = dataGridViewEmbeddedCertificates.SelectedRows[0].Index;

            // Delete certificate from certificate list at position index
            SEBSettings.embeddedCertificateList = (List<object>)SEBSettings.settingsNew[SEBSettings.MessageEmbeddedCertificates];
            SEBSettings.embeddedCertificateList       .RemoveAt(SEBSettings.embeddedCertificateIndex);
            dataGridViewEmbeddedCertificates.Rows     .RemoveAt(SEBSettings.embeddedCertificateIndex);

            if (SEBSettings.embeddedCertificateIndex == SEBSettings.embeddedCertificateList.Count)
                SEBSettings.embeddedCertificateIndex--;

            if (SEBSettings.embeddedCertificateList.Count > 0)
            {
                dataGridViewEmbeddedCertificates.Rows[SEBSettings.embeddedCertificateIndex].Selected = true;
            }
            else
            {
                // If certificate list is now empty, disable it
                SEBSettings.embeddedCertificateIndex = -1;
                dataGridViewEmbeddedCertificates.Enabled = false;
            }
        }



        // *************************
        // Group "Network - Proxies"
        // *************************
        private void radioButtonUseSystemProxySettings_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonUseSystemProxySettings.Checked == true)
                 SEBSettings.settingsNew[SEBSettings.MessageProxySettingsPolicy] = 0;
            else SEBSettings.settingsNew[SEBSettings.MessageProxySettingsPolicy] = 1;
        }

        private void radioButtonUseSebProxySettings_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonUseSebProxySettings.Checked == true)
                 SEBSettings.settingsNew[SEBSettings.MessageProxySettingsPolicy] = 1;
            else SEBSettings.settingsNew[SEBSettings.MessageProxySettingsPolicy] = 0;
        }

        private void checkBoxExcludeSimpleHostnames_CheckedChanged(object sender, EventArgs e)
        {
            // Get the proxies data
            SEBSettings.proxiesData = (Dictionary<string, object>)SEBSettings.settingsNew[SEBSettings.MessageProxies];
            SEBSettings.proxiesData[SEBSettings.MessageExcludeSimpleHostnames] = checkBoxExcludeSimpleHostnames.Checked;
        }

        private void checkBoxUsePassiveFTPMode_CheckedChanged(object sender, EventArgs e)
        {
            // Get the proxies data
            SEBSettings.proxiesData = (Dictionary<string, object>)SEBSettings.settingsNew[SEBSettings.MessageProxies];
            SEBSettings.proxiesData[SEBSettings.MessageFTPPassive] = checkBoxUsePassiveFTPMode.Checked;
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
            labelIfYourNetworkAdministrator.Visible = useAutoConfiguration;

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
            String MessageProtocolType = MessageProxyProtocolType[SEBSettings.proxyProtocolIndex];

            // Get the proxies data
            SEBSettings.proxiesData = (Dictionary<string, object>)SEBSettings.settingsNew[SEBSettings.MessageProxies];

            // Update the proxy widgets
            if (useAutoConfiguration)
            {
                textBoxAutoProxyConfigurationURL.Text = (String)SEBSettings.proxiesData[SEBSettings.MessageAutoConfigurationURL];
            }

            if (useProxyServer)
            {
                checkBoxProxyServerRequires.Checked = (Boolean)SEBSettings.proxiesData[MessageProtocolType + SEBSettings.MessageRequires];
                 textBoxProxyServerHost    .Text    =  (String)SEBSettings.proxiesData[MessageProtocolType + SEBSettings.MessageHost    ];
                 textBoxProxyServerPort    .Text    =  (String)SEBSettings.proxiesData[MessageProtocolType + SEBSettings.MessagePort    ].ToString();
                 textBoxProxyServerUsername.Text    =  (String)SEBSettings.proxiesData[MessageProtocolType + SEBSettings.MessageUsername];
                 textBoxProxyServerPassword.Text    =  (String)SEBSettings.proxiesData[MessageProtocolType + SEBSettings.MessagePassword];

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
            SEBSettings.proxiesData = (Dictionary<string, object>)SEBSettings.settingsNew[SEBSettings.MessageProxies];

            SEBSettings.proxyProtocolIndex = row;

            // Update the proxy enable data belonging to the current cell
            if (column == IntColumnProxyProtocolEnable)
            {
                String key = MessageProxyProtocolEnableKey[row];
                SEBSettings.proxiesData[key]     = (Boolean)value;
                BooleanProxyProtocolEnabled[row] = (Boolean)value;
            }
        }


        private void textBoxAutoProxyConfigurationURL_TextChanged(object sender, EventArgs e)
        {
            // Get the proxies data
            SEBSettings.proxiesData = (Dictionary<string, object>)SEBSettings.settingsNew[SEBSettings.MessageProxies];
            SEBSettings.proxiesData[SEBSettings.MessageAutoConfigurationURL] = textBoxAutoProxyConfigurationURL.Text;
        }

        private void buttonChooseProxyConfigurationFile_Click(object sender, EventArgs e)
        {

        }


        private void textBoxProxyServerHost_TextChanged(object sender, EventArgs e)
        {
            // Get the proxies data
            String key = MessageProxyProtocolType[SEBSettings.proxyProtocolIndex] + SEBSettings.MessageHost;
            SEBSettings.proxiesData      = (Dictionary<string, object>)SEBSettings.settingsNew[SEBSettings.MessageProxies];
            SEBSettings.proxiesData[key] = textBoxProxyServerHost.Text;
        }

        private void textBoxProxyServerPort_TextChanged(object sender, EventArgs e)
        {
            // Get the proxies data
            String key = MessageProxyProtocolType[SEBSettings.proxyProtocolIndex] + SEBSettings.MessagePort;
            SEBSettings.proxiesData = (Dictionary<string, object>)SEBSettings.settingsNew[SEBSettings.MessageProxies];

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
            String key = MessageProxyProtocolType[SEBSettings.proxyProtocolIndex] + SEBSettings.MessageRequires;
            SEBSettings.proxiesData      = (Dictionary<string, object>)SEBSettings.settingsNew[SEBSettings.MessageProxies];
            SEBSettings.proxiesData[key] = (Boolean)checkBoxProxyServerRequires.Checked;

            // Disable the username/password textboxes when they are not required
            textBoxProxyServerUsername.Enabled = checkBoxProxyServerRequires.Checked;
            textBoxProxyServerPassword.Enabled = checkBoxProxyServerRequires.Checked;
        }

        private void textBoxProxyServerUsername_TextChanged(object sender, EventArgs e)
        {
            // Get the proxies data
            String key = MessageProxyProtocolType[SEBSettings.proxyProtocolIndex] + SEBSettings.MessageUsername;
            SEBSettings.proxiesData      = (Dictionary<string, object>)SEBSettings.settingsNew[SEBSettings.MessageProxies];
            SEBSettings.proxiesData[key] = textBoxProxyServerUsername.Text;
        }

        private void textBoxProxyServerPassword_TextChanged(object sender, EventArgs e)
        {
            // Get the proxies data
            String key = MessageProxyProtocolType[SEBSettings.proxyProtocolIndex] + SEBSettings.MessagePassword;
            SEBSettings.proxiesData      = (Dictionary<string, object>)SEBSettings.settingsNew[SEBSettings.MessageProxies];
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
            SEBSettings.proxiesData = (Dictionary<string, object>)SEBSettings.settingsNew[SEBSettings.MessageProxies];

            SEBSettings.bypassedProxyIndex = row;
            SEBSettings.bypassedProxyList  = (List<object>)SEBSettings.proxiesData[SEBSettings.MessageExceptionsList];

            // Update the certificate data belonging to the current cell
            if (column == IntColumnDomainHostPort) SEBSettings.bypassedProxyList[SEBSettings.bypassedProxyIndex] = (String)value;
        }



        // ****************
        // Group "Security"
        // ****************
        private void listBoxSebServicePolicy_SelectedIndexChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsNew[SEBSettings.MessageSebServicePolicy] = listBoxSebServicePolicy.SelectedIndex;
        }

        private void checkBoxAllowVirtualMachine_CheckedChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsNew[SEBSettings.MessageAllowVirtualMachine] = checkBoxAllowVirtualMachine.Checked;
        }

        private void checkBoxCreateNewDesktop_CheckedChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsNew[SEBSettings.MessageCreateNewDesktop] = checkBoxCreateNewDesktop.Checked;
        }

        private void checkBoxKillExplorerShell_CheckedChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsNew[SEBSettings.MessageKillExplorerShell] = checkBoxKillExplorerShell.Checked;
        }

        private void checkBoxAllowUserSwitching_CheckedChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsNew[SEBSettings.MessageAllowUserSwitching] = checkBoxAllowUserSwitching.Checked;
        }

        private void checkBoxEnableLogging_CheckedChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsNew[SEBSettings.MessageEnableLogging] = checkBoxEnableLogging.Checked;
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
            SEBSettings.settingsNew[SEBSettings.MessageLogDirectoryWin]     = path;
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
            SEBSettings.settingsNew[SEBSettings.MessageInsideSebEnableSwitchUser] = checkBoxInsideSebEnableSwitchUser.Checked;
        }

        private void checkBoxInsideSebEnableLockThisComputer_CheckedChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsNew[SEBSettings.MessageInsideSebEnableLockThisComputer] = checkBoxInsideSebEnableLockThisComputer.Checked;
        }

        private void checkBoxInsideSebEnableChangeAPassword_CheckedChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsNew[SEBSettings.MessageInsideSebEnableChangeAPassword] = checkBoxInsideSebEnableChangeAPassword.Checked;
        }

        private void checkBoxInsideSebEnableStartTaskManager_CheckedChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsNew[SEBSettings.MessageInsideSebEnableStartTaskManager] = checkBoxInsideSebEnableStartTaskManager.Checked;
        }

        private void checkBoxInsideSebEnableLogOff_CheckedChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsNew[SEBSettings.MessageInsideSebEnableLogOff] = checkBoxInsideSebEnableLogOff.Checked;
        }

        private void checkBoxInsideSebEnableShutDown_CheckedChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsNew[SEBSettings.MessageInsideSebEnableShutDown] = checkBoxInsideSebEnableShutDown.Checked;
        }

        private void checkBoxInsideSebEnableEaseOfAccess_CheckedChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsNew[SEBSettings.MessageInsideSebEnableEaseOfAccess] = checkBoxInsideSebEnableEaseOfAccess.Checked;
        }

        private void checkBoxInsideSebEnableVmWareClientShade_CheckedChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsNew[SEBSettings.MessageInsideSebEnableVmWareClientShade] = checkBoxInsideSebEnableVmWareClientShade.Checked;
        }



        // *******************
        // Group "Outside SEB"
        // *******************
        private void checkBoxOutsideSebEnableSwitchUser_CheckedChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsNew[SEBSettings.MessageOutsideSebEnableSwitchUser] = checkBoxOutsideSebEnableSwitchUser.Checked;
        }

        private void checkBoxOutsideSebEnableLockThisComputer_CheckedChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsNew[SEBSettings.MessageOutsideSebEnableLockThisComputer] = checkBoxOutsideSebEnableLockThisComputer.Checked;
        }

        private void checkBoxOutsideSebEnableChangeAPassword_CheckedChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsNew[SEBSettings.MessageOutsideSebEnableChangeAPassword] = checkBoxOutsideSebEnableChangeAPassword.Checked;
        }

        private void checkBoxOutsideSebEnableStartTaskManager_CheckedChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsNew[SEBSettings.MessageOutsideSebEnableStartTaskManager] = checkBoxOutsideSebEnableStartTaskManager.Checked;
        }

        private void checkBoxOutsideSebEnableLogOff_CheckedChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsNew[SEBSettings.MessageOutsideSebEnableLogOff] = checkBoxOutsideSebEnableLogOff.Checked;
        }

        private void checkBoxOutsideSebEnableShutDown_CheckedChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsNew[SEBSettings.MessageOutsideSebEnableShutDown] = checkBoxOutsideSebEnableShutDown.Checked;
        }

        private void checkBoxOutsideSebEnableEaseOfAccess_CheckedChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsNew[SEBSettings.MessageOutsideSebEnableEaseOfAccess] = checkBoxOutsideSebEnableEaseOfAccess.Checked;
        }

        private void checkBoxOutsideSebEnableVmWareClientShade_CheckedChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsNew[SEBSettings.MessageOutsideSebEnableVmWareClientShade] = checkBoxOutsideSebEnableVmWareClientShade.Checked;
        }



        // *******************
        // Group "Hooked Keys"
        // *******************
        private void checkBoxHookKeys_CheckedChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsNew[SEBSettings.MessageHookKeys] = checkBoxHookKeys.Checked;
        }



        // ********************
        // Group "Special Keys"
        // ********************
        private void checkBoxEnableEsc_CheckedChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsNew[SEBSettings.MessageEnableEsc] = checkBoxEnableEsc.Checked;
        }

        private void checkBoxEnableCtrlEsc_CheckedChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsNew[SEBSettings.MessageEnableCtrlEsc] = checkBoxEnableCtrlEsc.Checked;
        }

        private void checkBoxEnableAltEsc_CheckedChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsNew[SEBSettings.MessageEnableAltEsc] = checkBoxEnableAltEsc.Checked;
        }

        private void checkBoxEnableAltTab_CheckedChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsNew[SEBSettings.MessageEnableAltTab] = checkBoxEnableAltTab.Checked;
        }

        private void checkBoxEnableAltF4_CheckedChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsNew[SEBSettings.MessageEnableAltF4] = checkBoxEnableAltF4.Checked;
        }

        private void checkBoxEnableStartMenu_CheckedChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsNew[SEBSettings.MessageEnableStartMenu] = checkBoxEnableStartMenu.Checked;
        }

        private void checkBoxEnableRightMouse_CheckedChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsNew[SEBSettings.MessageEnableRightMouse] = checkBoxEnableRightMouse.Checked;
        }



        // *********************
        // Group "Function Keys"
        // *********************
        private void checkBoxEnableF1_CheckedChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsNew[SEBSettings.MessageEnableF1] = checkBoxEnableF1.Checked;
        }

        private void checkBoxEnableF2_CheckedChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsNew[SEBSettings.MessageEnableF2] = checkBoxEnableF2.Checked;
        }

        private void checkBoxEnableF3_CheckedChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsNew[SEBSettings.MessageEnableF3] = checkBoxEnableF3.Checked;
        }

        private void checkBoxEnableF4_CheckedChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsNew[SEBSettings.MessageEnableF4] = checkBoxEnableF4.Checked;
        }

        private void checkBoxEnableF5_CheckedChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsNew[SEBSettings.MessageEnableF5] = checkBoxEnableF5.Checked;
        }

        private void checkBoxEnableF6_CheckedChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsNew[SEBSettings.MessageEnableF6] = checkBoxEnableF6.Checked;
        }

        private void checkBoxEnableF7_CheckedChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsNew[SEBSettings.MessageEnableF7] = checkBoxEnableF7.Checked;
        }

        private void checkBoxEnableF8_CheckedChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsNew[SEBSettings.MessageEnableF8] = checkBoxEnableF8.Checked;
        }

        private void checkBoxEnableF9_CheckedChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsNew[SEBSettings.MessageEnableF9] = checkBoxEnableF9.Checked;
        }

        private void checkBoxEnableF10_CheckedChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsNew[SEBSettings.MessageEnableF10] = checkBoxEnableF10.Checked;
        }

        private void checkBoxEnableF11_CheckedChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsNew[SEBSettings.MessageEnableF11] = checkBoxEnableF11.Checked;
        }

        private void checkBoxEnableF12_CheckedChanged(object sender, EventArgs e)
        {
            SEBSettings.settingsNew[SEBSettings.MessageEnableF12] = checkBoxEnableF12.Checked;
        }



    } // end of   class     SebWindowsConfigForm
}     // end of   namespace SebWindowsConfig
