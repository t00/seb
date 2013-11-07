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

            // Set all the default values for the Plist structure "sebSettingsDef"
            InitialiseSEBConfigurationSettings();

            // Initialise the global variables for the lists and subdictionaries
            InitialiseGlobalVariablesForGUIWidgets();

            // Initialise the GUI widgets of this configuration editor
            InitialiseGUIWidgets();

            // IMPORTANT:
            // Create a second dictionary "new settings"
            // and copy all default settings to the new settings.
            // This must be done BEFORE any config file is loaded
            // and assures that every (key, value) pair is contained
            // in the "new" and "def" dictionaries,
            // even if the loaded "tmp" dictionary does NOT contain every pair.

            sebSettingsNew.Clear();
            CopySettingsArrays    (      StateDef,       StateNew);
            CopySettingsDictionary(sebSettingsDef, sebSettingsNew);

            PrintSettingsDictionary(sebSettingsDef, "SettingsDef.txt");
            PrintSettingsDictionary(sebSettingsNew, "SettingsNew.txt");

            // When starting up, set the widgets to the default values
            UpdateAllWidgetsOfProgram();

            // Try to open the configuration file ("SebClient.ini/xml/seb")
            // given in the local directory (where SebWindowsConfig.exe was called)
            currentDireSebConfigFile = Directory.GetCurrentDirectory();
            currentFileSebConfigFile = "";
            currentPathSebConfigFile = "";

            defaultDireSebConfigFile = Directory.GetCurrentDirectory();
            defaultFileSebConfigFile =                  DefaultSebConfigXml;
            defaultPathSebConfigFile = Path.GetFullPath(DefaultSebConfigXml);

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
        private Boolean OpenConfigurationFile(String fileName)
        {
            // Cut off the file extension ".xml" or ".seb",
            // that is the last 4 characters of the file name
            String fileNameRaw = fileName.Substring(0, fileName.Length - 4);
            String fileNameExt = fileName.Substring(fileName.Length - 4, 4);

            // Decide whether the configuration file is encrypted or not
            Boolean                         isEncrypted = false;
            if (fileNameExt.Equals(".xml")) isEncrypted = false;
            if (fileNameExt.Equals(".seb")) isEncrypted = true;

            // TODO: decryption does not yet work,
            // TODO: so currently we can only read non-encrypted files
            isEncrypted = false;

            try 
            {
                // Read the configuration settings from the file.
                // If encrypted, decrypt the configuration settings
                // Convert the XML structure into a C# object
                //if (isEncrypted == true)
                {
                    TextReader textReader;
                    String encryptedSettings = "";
                    String decryptedSettings = "";
                  //String password          = "Seb";
                  //X509Certificate2 certificate = null;

                    textReader        = new StreamReader(fileName);
                    encryptedSettings = textReader.ReadToEnd();
                    textReader.Close();

                    // TODO: decryption does not yet work,
                    // TODO: so the decryption is in comments and bypassed
                  //decryptedSettings = sebController.DecryptSebClientSettings(encryptedSettings);
                  //decryptedSettings = decryptedSettings.Trim();

                    // TODO: when decryption works, delete the following bypass assignment:
                    decryptedSettings = encryptedSettings;

                    sebSettingsTmp = (Dictionary<string, object>)Plist.readPlistSource(decryptedSettings);
                }
                //else // unencrypted .xml file
                {
                    //sebSettingsTmp = (Dictionary<string, object>)Plist.readPlist(fileName);
                }
            }
            catch (Exception streamReadException)
            {
                // Let the user know what went wrong
                Console.WriteLine("The configuration file could not be read:");
                Console.WriteLine(streamReadException.Message);
                return false;
            }

            // After reading the settings from file,
            // copy them to "new" settings and update the widgets

            // Choose Identity needs a conversion from string to integer.
            // The SEB Windows configuration editor never reads the identity
            // from the config file but instead searches it in the
            // Certificate Store of the computer where it is running,
            // so initially the 0th list entry is displayed ("none").
            //
            //tmpCryptoIdentityInteger = 0;
            //tmpCryptoIdentityString  = 0;

            // Copy tmp settings to new settings
            sebSettingsNew.Clear();
            CopySettingsArrays    (      StateTmp,       StateNew);
            CopySettingsDictionary(sebSettingsTmp, sebSettingsNew);

            currentDireSebConfigFile = Path.GetDirectoryName(fileName);
            currentFileSebConfigFile = Path.GetFileName     (fileName);
            currentPathSebConfigFile = Path.GetFullPath     (fileName);

            // After loading a new config file, reset the URL Filter Table indices
            // to avoid errors, in case there was a non-empty URL Filter Table displayed
            // in the DataGridViewURLFilterRules prior to loading the new config file.
            urlFilterTableRow    = -1;
            urlFilterIsTitleRow  =  false;
            urlFilterRuleIndex   = -1;
            urlFilterActionIndex = -1;

            // Get the URL Filter Rules
            urlFilterRuleList = (List<object>)sebSettingsNew[MessageURLFilterRules];

            // If there are any filter rules, select first filter rule.
            // If there are no  filter rules, select no    filter rule.
            if (urlFilterRuleList.Count > 0) urlFilterRuleIndex =  0;
                                        else urlFilterRuleIndex = -1;

            // Initially show all filter rules with their actions (expanded view)
            urlFilterTableShowRule.Clear();
            for (int ruleIndex = 0; ruleIndex < urlFilterRuleList.Count; ruleIndex++)
            {
                urlFilterTableShowRule.Add(true);
            }

            UpdateAllWidgetsOfProgram();
            buttonRevertToLastOpened.Enabled = true;
            //Plist.writeXml(sebSettingsNew, "DebugSettingsNew_in_OpenConfigurationFile.xml");
            //PrintSettingsDictionary(sebSettingsTmp, "SettingsTmp.txt");
            //PrintSettingsDictionary(sebSettingsNew, "SettingsNew.txt");
            return true;
        }



        // ********************************************************
        // Write the settings to the configuration file and save it
        // ********************************************************
        private Boolean SaveConfigurationFile(String fileName)
        {
            // Cut off the file extension ".xml" or ".seb",
            // that is the last 4 characters of the file name
            String fileNameRaw = fileName.Substring(0, fileName.Length - 4);
            String fileNameExt = fileName.Substring(fileName.Length - 4, 4);

            // Decide whether the configuration file is encrypted or not
            Boolean                         isEncrypted = false;
            if (fileNameExt.Equals(".xml")) isEncrypted = false;
            if (fileNameExt.Equals(".seb")) isEncrypted = true;

            try 
            {
                // If the configuration file already exists, delete it
                // and write it again from scratch with new data
                if (File.Exists(fileName))
                    File.Delete(fileName);

                // Convert the C# object into an XML structure
                // If unencrypted, encrypt the configuration settings
                // Write the configuration settings into .xml or .seb file

                if (isEncrypted == true)
                {
                    TextWriter textWriter;
                    String encryptedSettings = "";
                    String decryptedSettings = "";
                    String password          = "Seb";
                    X509Certificate2 certificate = null;

                    decryptedSettings = Plist.writeXml(sebSettingsNew);

                  //encryptedSettings = sebController.EncryptWithPassword  (decryptedSettings, password);
                  //encryptedSettings = sebController.EncryptWithCertifikat(decryptedSettings, certificate);
                    encryptedSettings = decryptedSettings;

                    textWriter = new StreamWriter(fileName);
                    textWriter.Write(encryptedSettings);
                    textWriter.Close();
                }
                else // unencrypted .xml file
                {
                    Plist.writeXml(sebSettingsNew, fileName);
                    Plist.writeXml(sebSettingsNew, "DebugSettingsNew_in_SaveConfigurationFile.xml");
                }
            }
            catch (Exception streamWriteException) 
            {
                // Let the user know what went wrong
                Console.WriteLine("The configuration file could not be written:");
                Console.WriteLine(streamWriteException.Message);
                return false;
            }

            // After writing the settings to file, update the widgets
            currentDireSebConfigFile = Path.GetDirectoryName(fileName);
            currentFileSebConfigFile = Path.GetFileName     (fileName);
            currentPathSebConfigFile = Path.GetFullPath     (fileName);

            UpdateAllWidgetsOfProgram();
            return true;
        }



        // ********************
        // Copy settings arrays
        // ********************
        private void CopySettingsArrays(int StateSource, int StateTarget)
        {
            // Copy all settings from one array to another
            int value;

            for (value = 1; value <= ValueNum; value++)
            {
                settingString [StateTarget, value] = settingString [StateSource, value];
                settingInteger[StateTarget, value] = settingInteger[StateSource, value];
            }

            return;
        }



        // ************************
        // Copy settings dictionary
        // ************************
        private void CopySettingsDictionary(Dictionary<string, object> sebSettingsSource,
                                            Dictionary<string, object> sebSettingsTarget)
        {
            // Copy all settings from one dictionary to another
            // Create a dictionary "target settings".
            // Copy source settings to target settings
            foreach (KeyValuePair<string, object> pair in sebSettingsSource)
            {
                string key   = pair.Key;
                object value = pair.Value;

//              if (key.GetType == Type.Dictionary)
//                  CopySettingsDictionary(sebSettingsSource, sebSettingsTarget, keyNode);

                if  (sebSettingsTarget.ContainsKey(key))
                     sebSettingsTarget[key] = value;
                else sebSettingsTarget.Add(key, value);
            }

            return;
        }



        // *************************
        // Print settings dictionary
        // *************************
        private void PrintSettingsDictionary(Dictionary<string, object> sebSettings,
                                             String                     fileName)
        {
            FileStream   fileStream;
            StreamWriter fileWriter;

            // If the .ini file already exists, delete it
            // and write it again from scratch with new data
            if (File.Exists(fileName))
                File.Delete(fileName);

            // Open the file for writing
            fileStream = new FileStream  (fileName, FileMode.OpenOrCreate, FileAccess.Write);
            fileWriter = new StreamWriter(fileStream);

            // Write the header lines
            fileWriter.WriteLine("");
            fileWriter.WriteLine("number of (key, value) pairs = " + sebSettings.Count);
            fileWriter.WriteLine("");

            // Print (key, value) pairs of dictionary to file
            foreach (KeyValuePair<string, object> pair in sebSettings)
            {
                string key   = pair.Key;
                object value = pair.Value;
                string type  = value.GetType().ToString();

//                if (key.GetType == Type.Dictionary)
//                    CopySettingsDictionary(sebSettingsSource, sebSettingsTarget, keyNode);

                fileWriter.WriteLine("key   = " + key);
                fileWriter.WriteLine("value = " + value);
                fileWriter.WriteLine("type  = " + type);
                fileWriter.WriteLine("");
            }

            // Close the file
            fileWriter.Close();
            fileStream.Close();
            return;
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
            urlFilterRuleList = (List<object>)sebSettingsNew[MessageURLFilterRules];

            // Clear the table itself
            dataGridViewURLFilterRules.Enabled = (urlFilterRuleList.Count > 0);
            dataGridViewURLFilterRules.Rows.Clear();

            int row = 0;

            // Add URL Filter Rules of currently opened file to DataGridView
            for (int ruleIndex = 0; ruleIndex < urlFilterRuleList.Count; ruleIndex++)
            {
                urlFilterRuleData   = (Dictionary<string, object>)urlFilterRuleList[ruleIndex];
                Boolean active      = (Boolean     )urlFilterRuleData[MessageActive];
                String  expression  = (String      )urlFilterRuleData[MessageExpression];
                urlFilterActionList = (List<object>)urlFilterRuleData[MessageRuleActions];

                urlFilterTableRuleIndex  .Add(ruleIndex);
                urlFilterTableActionIndex.Add(-1);
                urlFilterTableIsTitleRow .Add(true);
                urlFilterTableStartRow   .Add(row);
                urlFilterTableEndRow     .Add(row);

                // If user chose EXPANDED view for this rule, add the action rows
                if (urlFilterTableShowRule[ruleIndex])
                    urlFilterTableEndRow  [ruleIndex] += urlFilterActionList.Count;

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
                for (int actionIndex = 0; actionIndex < urlFilterActionList.Count; actionIndex++)
                {
                    urlFilterActionData = (Dictionary<string, object>)urlFilterActionList[actionIndex];

                    Boolean Active     = (Boolean)urlFilterActionData[MessageActive];
                    Boolean Regex      = (Boolean)urlFilterActionData[MessageRegex];
                    String  Expression = (String )urlFilterActionData[MessageExpression];
                    Int32   Action     = (Int32  )urlFilterActionData[MessageAction];

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
            if (urlFilterRuleList.Count == 0) return;

            urlFilterTableRow = urlFilterTableStartRow[urlFilterRuleIndex] + urlFilterActionIndex + 1;
            dataGridViewURLFilterRules.Rows[urlFilterTableRow].Selected = true;

            // Determine if the selected row is a title row or action row.
            // Determine which rule and action belong to the selected row.
            urlFilterIsTitleRow  = urlFilterTableIsTitleRow [urlFilterTableRow];
            urlFilterRuleIndex   = urlFilterTableRuleIndex  [urlFilterTableRow];
            urlFilterActionIndex = urlFilterTableActionIndex[urlFilterTableRow];
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
            textBoxStartURL            .Text   =  (String)sebSettingsNew[MessageStartURL];
            textBoxSebServerURL        .Text   =  (String)sebSettingsNew[MessageSebServerURL];
          //textBoxAdminPassword       .Text   =  (String)sebSettingsNew[MessageAdminPassword];
          //textBoxConfirmAdminPassword.Text   =  (String)sebSettingsNew[MessageConfirmAdminPassword];
            textBoxHashedAdminPassword .Text   =  (String)sebSettingsNew[MessageHashedAdminPassword];
            checkBoxAllowQuit         .Checked = (Boolean)sebSettingsNew[MessageAllowQuit];
            checkBoxIgnoreQuitPassword.Checked = (Boolean)sebSettingsNew[MessageIgnoreQuitPassword];
          //textBoxQuitPassword        .Text   =  (String)sebSettingsNew[MessageQuitPassword];
          //textBoxConfirmQuitPassword .Text   =  (String)sebSettingsNew[MessageConfirmQuitPassword];
            textBoxHashedQuitPassword  .Text   =  (String)sebSettingsNew[MessageHashedQuitPassword];
            listBoxExitKey1.SelectedIndex      =     (int)sebSettingsNew[MessageExitKey1];
            listBoxExitKey2.SelectedIndex      =     (int)sebSettingsNew[MessageExitKey2];
            listBoxExitKey3.SelectedIndex      =     (int)sebSettingsNew[MessageExitKey3];

            // Group "Config File"
            radioButtonStartingAnExam     .Checked =    ((int)sebSettingsNew[MessageSebConfigPurpose] == 0);
            radioButtonConfiguringAClient .Checked =    ((int)sebSettingsNew[MessageSebConfigPurpose] == 1);
            checkBoxAllowPreferencesWindow.Checked = (Boolean)sebSettingsNew[MessageAllowPreferencesWindow];
            comboBoxCryptoIdentity.SelectedIndex   =          settingInteger[StateNew, ValueCryptoIdentity];
             textBoxSettingsPassword       .Text   =  (String)sebSettingsNew[MessageSettingsPassword];
           //textBoxConfirmSettingsPassword.Text   =  (String)sebSettingsNew[MessageConfirmSettingsPassword];
           //textBoxHashedSettingsPassword .Text   =  (String)sebSettingsNew[MessageHashedSettingsPassword];

            // Group "Appearance"
            radioButtonUseBrowserWindow       .Checked     =    ((int)sebSettingsNew[MessageBrowserViewMode] == 0);
            radioButtonUseFullScreenMode      .Checked     =    ((int)sebSettingsNew[MessageBrowserViewMode] == 1);
            comboBoxMainBrowserWindowWidth    .Text        =  (String)sebSettingsNew[MessageMainBrowserWindowWidth];
            comboBoxMainBrowserWindowHeight   .Text        =  (String)sebSettingsNew[MessageMainBrowserWindowHeight];
             listBoxMainBrowserWindowPositioning.SelectedIndex = (int)sebSettingsNew[MessageMainBrowserWindowPositioning];
            checkBoxEnableBrowserWindowToolbar.Checked     = (Boolean)sebSettingsNew[MessageEnableBrowserWindowToolbar];
            checkBoxHideBrowserWindowToolbar  .Checked     = (Boolean)sebSettingsNew[MessageHideBrowserWindowToolbar];
            checkBoxShowMenuBar               .Checked     = (Boolean)sebSettingsNew[MessageShowMenuBar];
            checkBoxShowTaskBar               .Checked     = (Boolean)sebSettingsNew[MessageShowTaskBar];
            comboBoxTaskBarHeight             .Text        =  (String)sebSettingsNew[MessageTaskBarHeight].ToString();

            // Group "Browser"
             listBoxOpenLinksHTML .SelectedIndex =     (int)sebSettingsNew[MessageNewBrowserWindowByLinkPolicy];
             listBoxOpenLinksJava .SelectedIndex =     (int)sebSettingsNew[MessageNewBrowserWindowByScriptPolicy];
            checkBoxBlockLinksHTML.Checked       = (Boolean)sebSettingsNew[MessageNewBrowserWindowByLinkBlockForeign];
            checkBoxBlockLinksJava.Checked       = (Boolean)sebSettingsNew[MessageNewBrowserWindowByScriptBlockForeign];

            comboBoxNewBrowserWindowWidth      .Text          = (String)sebSettingsNew[MessageNewBrowserWindowByLinkWidth ];
            comboBoxNewBrowserWindowHeight     .Text          = (String)sebSettingsNew[MessageNewBrowserWindowByLinkHeight];
             listBoxNewBrowserWindowPositioning.SelectedIndex =    (int)sebSettingsNew[MessageNewBrowserWindowByLinkPositioning];

            checkBoxEnablePlugIns           .Checked =   (Boolean)sebSettingsNew[MessageEnablePlugIns];
            checkBoxEnableJava              .Checked =   (Boolean)sebSettingsNew[MessageEnableJava];
            checkBoxEnableJavaScript        .Checked =   (Boolean)sebSettingsNew[MessageEnableJavaScript];
            checkBoxBlockPopUpWindows       .Checked =   (Boolean)sebSettingsNew[MessageBlockPopUpWindows];
            checkBoxAllowBrowsingBackForward.Checked =   (Boolean)sebSettingsNew[MessageAllowBrowsingBackForward];
            checkBoxUseSebWithoutBrowser    .Checked = !((Boolean)sebSettingsNew[MessageEnableSebBrowser]);
            // BEWARE: you must invert this value since "Use Without" is "Not Enable"!

            // Group "Down/Uploads"
            checkBoxAllowDownUploads.Checked           = (Boolean)sebSettingsNew[MessageAllowDownUploads];
            checkBoxOpenDownloads   .Checked           = (Boolean)sebSettingsNew[MessageOpenDownloads];
            checkBoxDownloadPDFFiles.Checked           = (Boolean)sebSettingsNew[MessageDownloadPDFFiles];
            labelDownloadDirectoryWin.Text             =  (String)sebSettingsNew[MessageDownloadDirectoryWin];
             listBoxChooseFileToUploadPolicy.SelectedIndex = (int)sebSettingsNew[MessageChooseFileToUploadPolicy];

            // Group "Exam"
           //textBoxBrowserExamKey    .Text    =  (String)sebSettingsNew[MessageBrowserExamKey];
             textBoxQuitURL           .Text    =  (String)sebSettingsNew[MessageQuitURL];
            checkBoxCopyBrowserExamKey.Checked = (Boolean)sebSettingsNew[MessageCopyBrowserExamKey];
            checkBoxSendBrowserExamKey.Checked = (Boolean)sebSettingsNew[MessageSendBrowserExamKey];

            // Group "Applications"
            checkBoxMonitorProcesses         .Checked = (Boolean)sebSettingsNew[MessageMonitorProcesses];
            checkBoxAllowSwitchToApplications.Checked = (Boolean)sebSettingsNew[MessageAllowSwitchToApplications];
            checkBoxAllowFlashFullscreen     .Checked = (Boolean)sebSettingsNew[MessageAllowFlashFullscreen];


            // Group "Applications - Permitted/Prohibited Processes"
            // Group "Network      -    Filter/Certificates/Proxies"

            // Update the lists for the DataGridViews
               permittedProcessList   = (List<object>)sebSettingsNew[MessagePermittedProcesses];
              prohibitedProcessList   = (List<object>)sebSettingsNew[MessageProhibitedProcesses];
            embeddedCertificateList   = (List<object>)sebSettingsNew[MessageEmbeddedCertificates];
            proxiesData = (Dictionary<string, object>)sebSettingsNew[MessageProxies];

            bypassedProxyList = (List<object>)proxiesData[MessageExceptionsList];

             // Check if currently loaded lists have any entries
            if (permittedProcessList.Count > 0) permittedProcessIndex =  0;
                                           else permittedProcessIndex = -1;

            if (prohibitedProcessList.Count > 0) prohibitedProcessIndex =  0;
                                            else prohibitedProcessIndex = -1;

            if (embeddedCertificateList.Count > 0) embeddedCertificateIndex =  0;
                                              else embeddedCertificateIndex = -1;

            proxyProtocolIndex = 0;

            if (bypassedProxyList.Count > 0) bypassedProxyIndex =  0;
                                        else bypassedProxyIndex = -1;

            // Remove all previously displayed list entries from DataGridViews
                groupBoxPermittedProcess  .Enabled = (permittedProcessList.Count > 0);
            dataGridViewPermittedProcesses.Enabled = (permittedProcessList.Count > 0);
            dataGridViewPermittedProcesses.Rows.Clear();

                groupBoxProhibitedProcess  .Enabled = (prohibitedProcessList.Count > 0);
            dataGridViewProhibitedProcesses.Enabled = (prohibitedProcessList.Count > 0);
            dataGridViewProhibitedProcesses.Rows.Clear();

            dataGridViewEmbeddedCertificates.Enabled = (embeddedCertificateList.Count > 0);
            dataGridViewEmbeddedCertificates.Rows.Clear();

            dataGridViewProxyProtocols.Enabled = true;
            dataGridViewProxyProtocols.Rows.Clear();

            dataGridViewBypassedProxies.Enabled = (bypassedProxyList.Count > 0);
            dataGridViewBypassedProxies.Rows.Clear();

            // Add Permitted Processes of currently opened file to DataGridView
            for (int index = 0; index < permittedProcessList.Count; index++)
            {
                permittedProcessData = (Dictionary<string, object>)permittedProcessList[index];
                Boolean active     = (Boolean)permittedProcessData[MessageActive];
                Int32   os         = (Int32  )permittedProcessData[MessageOS];
                String  executable = (String )permittedProcessData[MessageExecutable];
                String  title      = (String )permittedProcessData[MessageTitle];
                dataGridViewPermittedProcesses.Rows.Add(active, StringOS[os], executable, title);
            }

            // Add Prohibited Processes of currently opened file to DataGridView
            for (int index = 0; index < prohibitedProcessList.Count; index++)
            {
                prohibitedProcessData = (Dictionary<string, object>)prohibitedProcessList[index];
                Boolean active      = (Boolean)prohibitedProcessData[MessageActive];
                Int32   os          = (Int32  )prohibitedProcessData[MessageOS];
                String  executable  = (String )prohibitedProcessData[MessageExecutable];
                String  description = (String )prohibitedProcessData[MessageDescription];
                dataGridViewProhibitedProcesses.Rows.Add(active, StringOS[os], executable, description);
            }

            // Add URL Filter Rules of currently opened file to DataGridView
            UpdateTableOfURLFilterRules();

            // Add Embedded Certificates of Certificate Store to DataGridView
            for (int index = 0; index < embeddedCertificateList.Count; index++)
            {
                embeddedCertificateData = (Dictionary<string, object>)embeddedCertificateList[index];
                String data = (String)embeddedCertificateData[MessageCertificateData];
                Int32  type = (Int32 )embeddedCertificateData[MessageType];
                String name = (String)embeddedCertificateData[MessageName];
                dataGridViewEmbeddedCertificates.Rows.Add(StringCertificateType[type], name);
            }
/*
            // Get the "Enabled" boolean values of current "proxies" dictionary
            BooleanProxyProtocolEnabled[IntProxyAutoDiscovery    ] = (Boolean)proxiesData[MessageAutoDiscoveryEnabled];
            BooleanProxyProtocolEnabled[IntProxyAutoConfiguration] = (Boolean)proxiesData[MessageAutoConfigurationEnabled];
            BooleanProxyProtocolEnabled[IntProxyHTTP             ] = (Boolean)proxiesData[MessageHTTPEnable];
            BooleanProxyProtocolEnabled[IntProxyHTTPS            ] = (Boolean)proxiesData[MessageHTTPSEnable];
            BooleanProxyProtocolEnabled[IntProxyFTP              ] = (Boolean)proxiesData[MessageFTPEnable];
            BooleanProxyProtocolEnabled[IntProxySOCKS            ] = (Boolean)proxiesData[MessageSOCKSEnable];
            BooleanProxyProtocolEnabled[IntProxyRTSP             ] = (Boolean)proxiesData[MessageRTSPEnable];
*/
            // Get the "Enabled" boolean values of current "proxies" dictionary.
            // Add Proxy Protocols of currently opened file to DataGridView.
            for (int index = 0; index < NumProxyProtocols; index++)
            {
                Boolean enable = (Boolean)proxiesData[MessageProxyProtocolEnableKey   [index]];
                String  type   = (String )             StringProxyProtocolTableCaption[index];
                dataGridViewProxyProtocols.Rows.Add(enable, type);
                BooleanProxyProtocolEnabled[index] = enable;
            }

            // Add Bypassed Proxies of currently opened file to DataGridView
            for (int index = 0; index < bypassedProxyList.Count; index++)
            {
                bypassedProxyData = (String)bypassedProxyList[index];
                dataGridViewBypassedProxies.Rows.Add(bypassedProxyData);
            }

            // Load the currently selected process data
            if (permittedProcessList.Count > 0)
                 LoadAndUpdatePermittedSelectedProcessGroup(permittedProcessIndex);
            else ClearPermittedSelectedProcessGroup();

            if (prohibitedProcessList.Count > 0)
                 LoadAndUpdateProhibitedSelectedProcessGroup(prohibitedProcessIndex);
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
            checkBoxEnableURLFilter       .Checked = (Boolean)sebSettingsNew[MessageEnableURLFilter];
            checkBoxEnableURLContentFilter.Checked = (Boolean)sebSettingsNew[MessageEnableURLContentFilter];

            // Group "Network - Certificates"

            // Group "Network - Proxies"
            radioButtonUseSystemProxySettings.Checked =    ((int)sebSettingsNew[MessageProxySettingsPolicy] == 0);
            radioButtonUseSebProxySettings   .Checked =    ((int)sebSettingsNew[MessageProxySettingsPolicy] == 1);

            textBoxAutoProxyConfigurationURL .Text    =  (String)proxiesData[MessageAutoConfigurationURL];
            checkBoxExcludeSimpleHostnames   .Checked = (Boolean)proxiesData[MessageExcludeSimpleHostnames];
            checkBoxUsePassiveFTPMode        .Checked = (Boolean)proxiesData[MessageFTPPassive];

            // Group "Security"
             listBoxSebServicePolicy.SelectedIndex =     (int)sebSettingsNew[MessageSebServicePolicy];
            checkBoxAllowVirtualMachine.Checked    = (Boolean)sebSettingsNew[MessageAllowVirtualMachine];
            checkBoxCreateNewDesktop   .Checked    = (Boolean)sebSettingsNew[MessageCreateNewDesktop];
            checkBoxKillExplorerShell  .Checked    = (Boolean)sebSettingsNew[MessageKillExplorerShell];
            checkBoxAllowUserSwitching .Checked    = (Boolean)sebSettingsNew[MessageAllowUserSwitching];
            checkBoxEnableLogging      .Checked    = (Boolean)sebSettingsNew[MessageEnableLogging];
            labelLogDirectoryWin       .Text       =  (String)sebSettingsNew[MessageLogDirectoryWin];

            // Group "Registry"
            checkBoxInsideSebEnableSwitchUser       .Checked = (Boolean)sebSettingsNew[MessageInsideSebEnableSwitchUser];
            checkBoxInsideSebEnableLockThisComputer .Checked = (Boolean)sebSettingsNew[MessageInsideSebEnableLockThisComputer];
            checkBoxInsideSebEnableChangeAPassword  .Checked = (Boolean)sebSettingsNew[MessageInsideSebEnableChangeAPassword];
            checkBoxInsideSebEnableStartTaskManager .Checked = (Boolean)sebSettingsNew[MessageInsideSebEnableStartTaskManager];
            checkBoxInsideSebEnableLogOff           .Checked = (Boolean)sebSettingsNew[MessageInsideSebEnableLogOff];
            checkBoxInsideSebEnableShutDown         .Checked = (Boolean)sebSettingsNew[MessageInsideSebEnableShutDown];
            checkBoxInsideSebEnableEaseOfAccess     .Checked = (Boolean)sebSettingsNew[MessageInsideSebEnableEaseOfAccess];
            checkBoxInsideSebEnableVmWareClientShade.Checked = (Boolean)sebSettingsNew[MessageInsideSebEnableVmWareClientShade];

            checkBoxOutsideSebEnableSwitchUser       .Checked = (Boolean)sebSettingsNew[MessageOutsideSebEnableSwitchUser];
            checkBoxOutsideSebEnableLockThisComputer .Checked = (Boolean)sebSettingsNew[MessageOutsideSebEnableLockThisComputer];
            checkBoxOutsideSebEnableChangeAPassword  .Checked = (Boolean)sebSettingsNew[MessageOutsideSebEnableChangeAPassword];
            checkBoxOutsideSebEnableStartTaskManager .Checked = (Boolean)sebSettingsNew[MessageOutsideSebEnableStartTaskManager];
            checkBoxOutsideSebEnableLogOff           .Checked = (Boolean)sebSettingsNew[MessageOutsideSebEnableLogOff];
            checkBoxOutsideSebEnableShutDown         .Checked = (Boolean)sebSettingsNew[MessageOutsideSebEnableShutDown];
            checkBoxOutsideSebEnableEaseOfAccess     .Checked = (Boolean)sebSettingsNew[MessageOutsideSebEnableEaseOfAccess];
            checkBoxOutsideSebEnableVmWareClientShade.Checked = (Boolean)sebSettingsNew[MessageOutsideSebEnableVmWareClientShade];

            // Group "Hooked Keys"
            checkBoxHookKeys.Checked = (Boolean)sebSettingsNew[MessageHookKeys];

            checkBoxEnableEsc       .Checked = (Boolean)sebSettingsNew[MessageEnableEsc];
            checkBoxEnableCtrlEsc   .Checked = (Boolean)sebSettingsNew[MessageEnableCtrlEsc];
            checkBoxEnableAltEsc    .Checked = (Boolean)sebSettingsNew[MessageEnableAltEsc];
            checkBoxEnableAltTab    .Checked = (Boolean)sebSettingsNew[MessageEnableAltTab];
            checkBoxEnableAltF4     .Checked = (Boolean)sebSettingsNew[MessageEnableAltF4];
            checkBoxEnableStartMenu .Checked = (Boolean)sebSettingsNew[MessageEnableStartMenu];
            checkBoxEnableRightMouse.Checked = (Boolean)sebSettingsNew[MessageEnableRightMouse];

            checkBoxEnableF1 .Checked = (Boolean)sebSettingsNew[MessageEnableF1];
            checkBoxEnableF2 .Checked = (Boolean)sebSettingsNew[MessageEnableF2];
            checkBoxEnableF3 .Checked = (Boolean)sebSettingsNew[MessageEnableF3];
            checkBoxEnableF4 .Checked = (Boolean)sebSettingsNew[MessageEnableF4];
            checkBoxEnableF5 .Checked = (Boolean)sebSettingsNew[MessageEnableF5];
            checkBoxEnableF6 .Checked = (Boolean)sebSettingsNew[MessageEnableF6];
            checkBoxEnableF7 .Checked = (Boolean)sebSettingsNew[MessageEnableF7];
            checkBoxEnableF8 .Checked = (Boolean)sebSettingsNew[MessageEnableF8];
            checkBoxEnableF9 .Checked = (Boolean)sebSettingsNew[MessageEnableF9];
            checkBoxEnableF10.Checked = (Boolean)sebSettingsNew[MessageEnableF10];
            checkBoxEnableF11.Checked = (Boolean)sebSettingsNew[MessageEnableF11];
            checkBoxEnableF12.Checked = (Boolean)sebSettingsNew[MessageEnableF12];

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
            sebSettingsNew[MessageStartURL] = textBoxStartURL.Text;
        }

        private void buttonPasteFromSavedClipboard_Click(object sender, EventArgs e)
        {

        }

        private void textBoxSebServerURL_TextChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageSebServerURL] = textBoxSebServerURL.Text;
        }

        private void textBoxAdminPassword_TextChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageAdminPassword] = textBoxAdminPassword.Text;
        }

        private void textBoxConfirmAdminPassword_TextChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageConfirmAdminPassword] = textBoxConfirmAdminPassword.Text;
        }

        private void checkBoxAllowQuit_CheckedChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageAllowQuit] = checkBoxAllowQuit.Checked;
        }

        private void checkBoxIgnoreQuitPassword_CheckedChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageIgnoreQuitPassword] = checkBoxIgnoreQuitPassword.Checked;
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

            sebSettingsNew[MessageQuitPassword      ] = newStringQuitPassword;
            sebSettingsNew[MessageHashedQuitPassword] = newStringQuitHashcode;
        }


        private void textBoxConfirmQuitPassword_TextChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageConfirmQuitPassword] = textBoxConfirmQuitPassword.Text;
        }

        private void listBoxExitKey1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Make sure that all three exit keys are different.
            // If selected key is already occupied, revert to previously selected key.
            if ((listBoxExitKey1.SelectedIndex == listBoxExitKey2.SelectedIndex) ||
                (listBoxExitKey1.SelectedIndex == listBoxExitKey3.SelectedIndex))
                 listBoxExitKey1.SelectedIndex =  (int)sebSettingsNew[MessageExitKey1];
            sebSettingsNew[MessageExitKey1] = listBoxExitKey1.SelectedIndex;
        }

        private void listBoxExitKey2_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Make sure that all three exit keys are different.
            // If selected key is already occupied, revert to previously selected key.
            if ((listBoxExitKey2.SelectedIndex == listBoxExitKey1.SelectedIndex) ||
                (listBoxExitKey2.SelectedIndex == listBoxExitKey3.SelectedIndex))
                 listBoxExitKey2.SelectedIndex =  (int)sebSettingsNew[MessageExitKey2];
            sebSettingsNew[MessageExitKey2] = listBoxExitKey2.SelectedIndex;
        }

        private void listBoxExitKey3_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Make sure that all three exit keys are different.
            // If selected key is already occupied, revert to previously selected key.
            if ((listBoxExitKey3.SelectedIndex == listBoxExitKey1.SelectedIndex) ||
                (listBoxExitKey3.SelectedIndex == listBoxExitKey2.SelectedIndex))
                 listBoxExitKey3.SelectedIndex =  (int)sebSettingsNew[MessageExitKey3];
            sebSettingsNew[MessageExitKey3] = listBoxExitKey3.SelectedIndex;
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
                 sebSettingsNew[MessageSebConfigPurpose] = 0;
            else sebSettingsNew[MessageSebConfigPurpose] = 1;
        }

        private void radioButtonConfiguringAClient_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonConfiguringAClient.Checked == true)
                 sebSettingsNew[MessageSebConfigPurpose] = 1;
            else sebSettingsNew[MessageSebConfigPurpose] = 0;
        }

        private void checkBoxAllowPreferencesWindow_CheckedChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageAllowPreferencesWindow] = checkBoxAllowPreferencesWindow.Checked;
        }

        private void comboBoxCryptoIdentity_SelectedIndexChanged(object sender, EventArgs e)
        {
            settingInteger[StateNew, ValueCryptoIdentity] = comboBoxCryptoIdentity.SelectedIndex;
            settingString [StateNew, ValueCryptoIdentity] = comboBoxCryptoIdentity.Text;
        }

        private void comboBoxCryptoIdentity_TextUpdate(object sender, EventArgs e)
        {
            settingInteger[StateNew, ValueCryptoIdentity] = comboBoxCryptoIdentity.SelectedIndex;
            settingString [StateNew, ValueCryptoIdentity] = comboBoxCryptoIdentity.Text;
        }

        private void textBoxSettingsPassword_TextChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageSettingsPassword] = textBoxSettingsPassword.Text;
        }

        private void textBoxConfirmSettingsPassword_TextChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageConfirmSettingsPassword] = textBoxConfirmSettingsPassword.Text;
        }


        private void buttonDefaultSettings_Click(object sender, EventArgs e)
        {
            //Plist.writeXml(sebSettingsNew, "DebugSettingsNew_before_RevertToDefault.xml");
            //Plist.writeXml(sebSettingsDef, "DebugSettingsDef_before_RevertToDefault.xml");
            InitialiseSEBConfigurationSettings();
            sebSettingsNew.Clear();
            CopySettingsArrays    (      StateDef,       StateNew);
            CopySettingsDictionary(sebSettingsDef, sebSettingsNew);
            UpdateAllWidgetsOfProgram();
            //Plist.writeXml(sebSettingsNew, "DebugSettingsNew_after_RevertToDefault.xml");
            //Plist.writeXml(sebSettingsDef, "DebugSettingsDef_after_RevertToDefault.xml");
        }

        private void buttonRevertToLastOpened_Click(object sender, EventArgs e)
        {
            //Plist.writeXml(sebSettingsNew, "DebugSettingsNew_before_RevertToLastOpened.xml");
            OpenConfigurationFile(currentPathSebConfigFile);
            //Plist.writeXml(sebSettingsNew, "DebugSettingsNew_after_RevertToLastOpened.xml");
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
            if (fileDialogResult.Equals(DialogResult.OK    )) OpenConfigurationFile(fileName);
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
            if (fileDialogResult.Equals(DialogResult.OK    )) SaveConfigurationFile(fileName);
        }



        // ******************
        // Group "Appearance"
        // ******************
        private void radioButtonUseBrowserWindow_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonUseBrowserWindow.Checked == true)
                 sebSettingsNew[MessageBrowserViewMode] = 0;
            else sebSettingsNew[MessageBrowserViewMode] = 1;
        }

        private void radioButtonUseFullScreenMode_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonUseFullScreenMode.Checked == true)
                 sebSettingsNew[MessageBrowserViewMode] = 1;
            else sebSettingsNew[MessageBrowserViewMode] = 0;
        }

        private void comboBoxMainBrowserWindowWidth_SelectedIndexChanged(object sender, EventArgs e)
        {
            settingInteger[StateNew, ValueMainBrowserWindowWidth] = comboBoxMainBrowserWindowWidth.SelectedIndex;
            settingString [StateNew, ValueMainBrowserWindowWidth] = comboBoxMainBrowserWindowWidth.Text;
          //sebSettingsNew[        MessageMainBrowserWindowWidth] = comboBoxMainBrowserWindowWidth.SelectedIndex;
            sebSettingsNew[        MessageMainBrowserWindowWidth] = comboBoxMainBrowserWindowWidth.Text;
        }

        private void comboBoxMainBrowserWindowWidth_TextUpdate(object sender, EventArgs e)
        {
            settingInteger[StateNew, ValueMainBrowserWindowWidth] = comboBoxMainBrowserWindowWidth.SelectedIndex;
            settingString [StateNew, ValueMainBrowserWindowWidth] = comboBoxMainBrowserWindowWidth.Text;
          //sebSettingsNew[        MessageMainBrowserWindowWidth] = comboBoxMainBrowserWindowWidth.SelectedIndex;
            sebSettingsNew[        MessageMainBrowserWindowWidth] = comboBoxMainBrowserWindowWidth.Text;
        }

        private void comboBoxMainBrowserWindowHeight_SelectedIndexChanged(object sender, EventArgs e)
        {
            settingInteger[StateNew, ValueMainBrowserWindowHeight] = comboBoxMainBrowserWindowHeight.SelectedIndex;
            settingString [StateNew, ValueMainBrowserWindowHeight] = comboBoxMainBrowserWindowHeight.Text;
          //sebSettingsNew[        MessageMainBrowserWindowHeight] = comboBoxMainBrowserWindowHeight.SelectedIndex;
            sebSettingsNew[        MessageMainBrowserWindowHeight] = comboBoxMainBrowserWindowHeight.Text;
        }

        private void comboBoxMainBrowserWindowHeight_TextUpdate(object sender, EventArgs e)
        {
            settingInteger[StateNew, ValueMainBrowserWindowHeight] = comboBoxMainBrowserWindowHeight.SelectedIndex;
            settingString [StateNew, ValueMainBrowserWindowHeight] = comboBoxMainBrowserWindowHeight.Text;
          //sebSettingsNew[        MessageMainBrowserWindowHeight] = comboBoxMainBrowserWindowHeight.SelectedIndex;
            sebSettingsNew[        MessageMainBrowserWindowHeight] = comboBoxMainBrowserWindowHeight.Text;
        }

        private void listBoxMainBrowserWindowPositioning_SelectedIndexChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageMainBrowserWindowPositioning] = listBoxMainBrowserWindowPositioning.SelectedIndex;
        }

        private void checkBoxEnableBrowserWindowToolbar_CheckedChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageEnableBrowserWindowToolbar] = checkBoxEnableBrowserWindowToolbar.Checked;
            checkBoxHideBrowserWindowToolbar.Enabled          = checkBoxEnableBrowserWindowToolbar.Checked;
        }

        private void checkBoxHideBrowserWindowToolbar_CheckedChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageHideBrowserWindowToolbar] = checkBoxHideBrowserWindowToolbar.Checked;
        }

        private void checkBoxShowMenuBar_CheckedChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageShowMenuBar] = checkBoxShowMenuBar.Checked;
        }

        private void checkBoxShowTaskBar_CheckedChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageShowTaskBar] = checkBoxShowTaskBar.Checked;
            comboBoxTaskBarHeight.Enabled      = checkBoxShowTaskBar.Checked;
        }

        private void comboBoxTaskBarHeight_SelectedIndexChanged(object sender, EventArgs e)
        {
            settingInteger[StateNew, ValueTaskBarHeight] = comboBoxTaskBarHeight.SelectedIndex;
            settingString [StateNew, ValueTaskBarHeight] = comboBoxTaskBarHeight.Text;
          //sebSettingsNew[        MessageTaskBarHeight] = comboBoxTaskBarHeight.SelectedIndex;
            sebSettingsNew[        MessageTaskBarHeight] = Int32.Parse(comboBoxTaskBarHeight.Text);
        }



        // ***************
        // Group "Browser"
        // ***************
        private void listBoxOpenLinksHTML_SelectedIndexChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageNewBrowserWindowByLinkPolicy] = listBoxOpenLinksHTML.SelectedIndex;
        }

        private void listBoxOpenLinksJava_SelectedIndexChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageNewBrowserWindowByScriptPolicy] = listBoxOpenLinksJava.SelectedIndex;
        }

        private void checkBoxBlockLinksHTML_CheckedChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageNewBrowserWindowByLinkBlockForeign] = checkBoxBlockLinksHTML.Checked;
        }

        private void checkBoxBlockLinksJava_CheckedChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageNewBrowserWindowByScriptBlockForeign] = checkBoxBlockLinksJava.Checked;
        }

        private void comboBoxNewBrowserWindowWidth_SelectedIndexChanged(object sender, EventArgs e)
        {
            settingInteger[StateNew, ValueNewBrowserWindowByLinkWidth] = comboBoxNewBrowserWindowWidth.SelectedIndex;
            settingString [StateNew, ValueNewBrowserWindowByLinkWidth] = comboBoxNewBrowserWindowWidth.Text;
          //sebSettingsNew[MessageNewBrowserWindowByLinkWidth] = comboBoxNewBrowserWindowWidth.SelectedIndex;
            sebSettingsNew[MessageNewBrowserWindowByLinkWidth] = comboBoxNewBrowserWindowWidth.Text;
        }

        private void comboBoxNewBrowserWindowWidth_TextUpdate(object sender, EventArgs e)
        {
            settingInteger[StateNew, ValueNewBrowserWindowByLinkWidth] = comboBoxNewBrowserWindowWidth.SelectedIndex;
            settingString [StateNew, ValueNewBrowserWindowByLinkWidth] = comboBoxNewBrowserWindowWidth.Text;
          //sebSettingsNew[MessageNewBrowserWindowByLinkWidth] = comboBoxNewBrowserWindowWidth.SelectedIndex;
            sebSettingsNew[MessageNewBrowserWindowByLinkWidth] = comboBoxNewBrowserWindowWidth.Text;
        }

        private void comboBoxNewBrowserWindowHeight_SelectedIndexChanged(object sender, EventArgs e)
        {
            settingInteger[StateNew, ValueNewBrowserWindowByLinkHeight] = comboBoxNewBrowserWindowHeight.SelectedIndex;
            settingString [StateNew, ValueNewBrowserWindowByLinkHeight] = comboBoxNewBrowserWindowHeight.Text;
          //sebSettingsNew[MessageNewBrowserWindowByLinkHeight] = comboBoxNewBrowserWindowHeight.SelectedIndex;
            sebSettingsNew[MessageNewBrowserWindowByLinkHeight] = comboBoxNewBrowserWindowHeight.Text;
        }

        private void comboBoxNewBrowserWindowHeight_TextUpdate(object sender, EventArgs e)
        {
            settingInteger[StateNew, ValueNewBrowserWindowByLinkHeight] = comboBoxNewBrowserWindowHeight.SelectedIndex;
            settingString [StateNew, ValueNewBrowserWindowByLinkHeight] = comboBoxNewBrowserWindowHeight.Text;
          //sebSettingsNew[MessageNewBrowserWindowByLinkHeight] = comboBoxNewBrowserWindowHeight.SelectedIndex;
            sebSettingsNew[MessageNewBrowserWindowByLinkHeight] = comboBoxNewBrowserWindowHeight.Text;
        }

        private void listBoxNewBrowserWindowPositioning_SelectedIndexChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageNewBrowserWindowByLinkPositioning] = listBoxNewBrowserWindowPositioning.SelectedIndex;
        }

        private void checkBoxEnablePlugins_CheckedChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageEnablePlugIns] = checkBoxEnablePlugIns.Checked;
        }

        private void checkBoxEnableJava_CheckedChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageEnableJava] = checkBoxEnableJava.Checked;
        }

        private void checkBoxEnableJavaScript_CheckedChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageEnableJavaScript] = checkBoxEnableJavaScript.Checked;
        }

        private void checkBoxBlockPopUpWindows_CheckedChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageBlockPopUpWindows] = checkBoxBlockPopUpWindows.Checked;
        }

        private void checkBoxAllowBrowsingBackForward_CheckedChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageAllowBrowsingBackForward] = checkBoxAllowBrowsingBackForward.Checked;
        }

        // BEWARE: you must invert this value since "Use Without" is "Not Enable"!
        private void checkBoxUseSebWithoutBrowser_CheckedChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageEnableSebBrowser] = !(checkBoxUseSebWithoutBrowser.Checked);
        }



        // ********************
        // Group "Down/Uploads"
        // ********************
        private void checkBoxAllowDownUploads_CheckedChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageAllowDownUploads] = checkBoxAllowDownUploads.Checked;
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
            sebSettingsNew[MessageDownloadDirectoryWin]     = path;
                             labelDownloadDirectoryWin.Text = path;
        }

        private void checkBoxOpenDownloads_CheckedChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageOpenDownloads] = checkBoxOpenDownloads.Checked;
        }

        private void listBoxChooseFileToUploadPolicy_SelectedIndexChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageChooseFileToUploadPolicy] = listBoxChooseFileToUploadPolicy.SelectedIndex;
        }

        private void checkBoxDownloadPDFFiles_CheckedChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageDownloadPDFFiles] = checkBoxDownloadPDFFiles.Checked;
        }



        // ************
        // Group "Exam"
        // ************
        private void buttonGenerateBrowserExamKey_Click(object sender, EventArgs e)
        {

        }

        private void textBoxBrowserExamKey_TextChanged(object sender, EventArgs e)
        {
          //sebSettingsNew[MessageBrowserExamKey] = textBoxBrowserExamKey.Text;
        }

        private void checkBoxCopyBrowserExamKey_CheckedChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageCopyBrowserExamKey] = checkBoxCopyBrowserExamKey.Checked;
        }

        private void checkBoxSendBrowserExamKey_CheckedChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageSendBrowserExamKey] = checkBoxSendBrowserExamKey.Checked;
        }

        private void textBoxQuitURL_TextChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageQuitURL] = textBoxQuitURL.Text;
        }



        // ********************
        // Group "Applications"
        // ********************
        private void checkBoxMonitorProcesses_CheckedChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageMonitorProcesses] = checkBoxMonitorProcesses.Checked;
        }


        // ******************************************
        // Group "Applications - Permitted Processes"
        // ******************************************
        private void checkBoxAllowSwitchToApplications_CheckedChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageAllowSwitchToApplications] = checkBoxAllowSwitchToApplications.Checked;
            checkBoxAllowFlashFullscreen.Enabled             = checkBoxAllowSwitchToApplications.Checked;
        }

        private void checkBoxAllowFlashFullscreen_CheckedChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageAllowFlashFullscreen] = checkBoxAllowFlashFullscreen.Checked;
        }


        private void LoadAndUpdatePermittedSelectedProcessGroup(int selectedProcessIndex)
        {
            // Get the process data of the selected process
            permittedProcessList  =               (List<object>)sebSettingsNew[MessagePermittedProcesses];
            permittedProcessData  = (Dictionary<string, object>)permittedProcessList[selectedProcessIndex];
            permittedArgumentList =               (List<object>)permittedProcessData[MessageArguments];

            // Update the widgets in the "Selected Process" group
            checkBoxPermittedProcessActive   .Checked = (Boolean)permittedProcessData[MessageActive];
            checkBoxPermittedProcessAutostart.Checked = (Boolean)permittedProcessData[MessageAutostart];
            checkBoxPermittedProcessAutohide .Checked = (Boolean)permittedProcessData[MessageAutohide];
            checkBoxPermittedProcessAllowUser.Checked = (Boolean)permittedProcessData[MessageAllowUser];
             listBoxPermittedProcessOS.SelectedIndex  =   (Int32)permittedProcessData[MessageOS];
             textBoxPermittedProcessTitle      .Text  =  (String)permittedProcessData[MessageTitle];
             textBoxPermittedProcessDescription.Text  =  (String)permittedProcessData[MessageDescription];
             textBoxPermittedProcessExecutable .Text  =  (String)permittedProcessData[MessageExecutable];
             textBoxPermittedProcessPath       .Text  =  (String)permittedProcessData[MessagePath];
             textBoxPermittedProcessIdentifier .Text  =  (String)permittedProcessData[MessageIdentifier];

             // Check if selected process has any arguments
            if (permittedArgumentList.Count > 0) permittedArgumentIndex =  0;
                                            else permittedArgumentIndex = -1;

            // Remove all previously displayed arguments from DataGridView
            dataGridViewPermittedProcessArguments.Enabled = (permittedArgumentList.Count > 0);
            dataGridViewPermittedProcessArguments.Rows.Clear();

            // Add arguments of selected process to DataGridView
            for (int index = 0; index < permittedArgumentList.Count; index++)
            {
                permittedArgumentData = (Dictionary<string, object>)permittedArgumentList[index];
                Boolean active   = (Boolean)permittedArgumentData[MessageActive];
                String  argument = (String )permittedArgumentData[MessageArgument];
                dataGridViewPermittedProcessArguments.Rows.Add(active, argument);
            }

            // Get the selected argument data
            if  (permittedArgumentList.Count > 0)
                 permittedArgumentData = (Dictionary<string, object>)permittedArgumentList[permittedArgumentIndex];
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
            permittedProcessIndex = dataGridViewPermittedProcesses.SelectedRows[0].Index;

            // The process list should contain at least one element here:
            // permittedProcessList.Count >  0
            // permittedProcessIndex      >= 0
            LoadAndUpdatePermittedSelectedProcessGroup(permittedProcessIndex);
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
            permittedProcessIndex = row;
            permittedProcessList  =               (List<object>)sebSettingsNew[MessagePermittedProcesses];
            permittedProcessData  = (Dictionary<string, object>)permittedProcessList [permittedProcessIndex];

            // Update the process data belonging to the current cell
            if (column == IntColumnProcessActive    ) permittedProcessData[MessageActive    ] = (Boolean)value;
            if (column == IntColumnProcessOS        ) permittedProcessData[MessageOS        ] = (Int32  )value;
            if (column == IntColumnProcessExecutable) permittedProcessData[MessageExecutable] = (String )value;
            if (column == IntColumnProcessTitle     ) permittedProcessData[MessageTitle     ] = (String )value;

            // Update the widget belonging to the current cell (in "Selected Process" group)
            if (column == IntColumnProcessActive    ) checkBoxPermittedProcessActive.Checked   = (Boolean)value;
            if (column == IntColumnProcessOS        )  listBoxPermittedProcessOS.SelectedIndex = (Int32  )value;
            if (column == IntColumnProcessExecutable)  textBoxPermittedProcessExecutable.Text  = (String )value;
            if (column == IntColumnProcessTitle     )  textBoxPermittedProcessTitle     .Text  = (String )value;
        }


        private void buttonAddPermittedProcess_Click(object sender, EventArgs e)
        {
            // Get the process list
            permittedProcessList = (List<object>)sebSettingsNew[MessagePermittedProcesses];

            if (permittedProcessList.Count > 0)
            {
                if (dataGridViewPermittedProcesses.SelectedRows.Count != 1) return;
              //permittedProcessIndex = dataGridViewPermittedProcesses.SelectedRows[0].Index;
                permittedProcessIndex = permittedProcessList.Count;
            }
            else
            {
                // If process list was empty before, enable it
                permittedProcessIndex = 0;
                dataGridViewPermittedProcesses.Enabled = true;
                    groupBoxPermittedProcess  .Enabled = true;
            }

            // Create new process dataset containing default values
            Dictionary<string, object> processData = new Dictionary<string, object>();

            processData[MessageActive     ] = true;
            processData[MessageAutostart  ] = true;
            processData[MessageAutohide   ] = true;
            processData[MessageAllowUser  ] = true;
            processData[MessageOS         ] = IntWin;
            processData[MessageTitle      ] = "";
            processData[MessageDescription] = "";
            processData[MessageExecutable ] = "";
            processData[MessagePath       ] = "";
            processData[MessageIdentifier ] = "";
            processData[MessageArguments  ] = new List<object>();

            // Insert new process into process list at position index
            permittedProcessList               .Insert(permittedProcessIndex, processData);
            dataGridViewPermittedProcesses.Rows.Insert(permittedProcessIndex, true, StringOS[IntWin], "", "");
            dataGridViewPermittedProcesses.Rows       [permittedProcessIndex].Selected = true;
        }


        private void buttonRemovePermittedProcess_Click(object sender, EventArgs e)
        {
            if (dataGridViewPermittedProcesses.SelectedRows.Count != 1) return;

            // Clear the widgets in the "Selected Process" group
            ClearPermittedSelectedProcessGroup();

            // Delete process from process list at position index
            permittedProcessIndex = dataGridViewPermittedProcesses.SelectedRows[0].Index;
            permittedProcessList  = (List<object>)sebSettingsNew[MessagePermittedProcesses];
            permittedProcessList               .RemoveAt(permittedProcessIndex);
            dataGridViewPermittedProcesses.Rows.RemoveAt(permittedProcessIndex);

            if (permittedProcessIndex == permittedProcessList.Count)
                permittedProcessIndex--;

            if (permittedProcessList.Count > 0)
            {
                dataGridViewPermittedProcesses.Rows[permittedProcessIndex].Selected = true;
            }
            else
            {
                // If process list is now empty, disable it
                permittedProcessIndex  = -1;
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
            if (permittedProcessIndex < 0) return;
            permittedProcessList =               (List<object>)sebSettingsNew[MessagePermittedProcesses];
            permittedProcessData = (Dictionary<string, object>)permittedProcessList [permittedProcessIndex];
            permittedProcessData[MessageActive] =      checkBoxPermittedProcessActive.Checked;
            Boolean                     active  =      checkBoxPermittedProcessActive.Checked;
            dataGridViewPermittedProcesses.Rows[permittedProcessIndex].Cells[IntColumnProcessActive].Value = active.ToString();
        }

        private void checkBoxPermittedProcessAutostart_CheckedChanged(object sender, EventArgs e)
        {
            if (permittedProcessIndex < 0) return;
            permittedProcessList =               (List<object>)sebSettingsNew[MessagePermittedProcesses];
            permittedProcessData = (Dictionary<string, object>)permittedProcessList [permittedProcessIndex];
            permittedProcessData[MessageAutostart] =   checkBoxPermittedProcessAutostart.Checked;
        }

        private void checkBoxPermittedProcessAutohide_CheckedChanged(object sender, EventArgs e)
        {
            if (permittedProcessIndex < 0) return;
            permittedProcessList =               (List<object>)sebSettingsNew[MessagePermittedProcesses];
            permittedProcessData = (Dictionary<string, object>)permittedProcessList [permittedProcessIndex];
            permittedProcessData[MessageAutohide] =    checkBoxPermittedProcessAutohide.Checked;
        }

        private void checkBoxPermittedProcessAllowUser_CheckedChanged(object sender, EventArgs e)
        {
            if (permittedProcessIndex < 0) return;
            permittedProcessList =               (List<object>)sebSettingsNew[MessagePermittedProcesses];
            permittedProcessData = (Dictionary<string, object>)permittedProcessList [permittedProcessIndex];
            permittedProcessData[MessageAllowUser] =   checkBoxPermittedProcessAllowUser.Checked;
        }

        private void listBoxPermittedProcessOS_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (permittedProcessIndex < 0) return;
            permittedProcessList =               (List<object>)sebSettingsNew[MessagePermittedProcesses];
            permittedProcessData = (Dictionary<string, object>)permittedProcessList [permittedProcessIndex];
            permittedProcessData[MessageOS] =           listBoxPermittedProcessOS.SelectedIndex;
            Int32                       os  =           listBoxPermittedProcessOS.SelectedIndex;
            dataGridViewPermittedProcesses.Rows[permittedProcessIndex].Cells[IntColumnProcessOS].Value = StringOS[os];
        }

        private void textBoxPermittedProcessTitle_TextChanged(object sender, EventArgs e)
        {
            if (permittedProcessIndex < 0) return;
            permittedProcessList =               (List<object>)sebSettingsNew[MessagePermittedProcesses];
            permittedProcessData = (Dictionary<string, object>)permittedProcessList [permittedProcessIndex];
            permittedProcessData[MessageTitle] =        textBoxPermittedProcessTitle.Text;
            String                      title  =        textBoxPermittedProcessTitle.Text;
            dataGridViewPermittedProcesses.Rows[permittedProcessIndex].Cells[IntColumnProcessTitle].Value = title;
        }

        private void textBoxPermittedProcessDescription_TextChanged(object sender, EventArgs e)
        {
            if (permittedProcessIndex < 0) return;
            permittedProcessList =               (List<object>)sebSettingsNew[MessagePermittedProcesses];
            permittedProcessData = (Dictionary<string, object>)permittedProcessList [permittedProcessIndex];
            permittedProcessData[MessageDescription] =  textBoxPermittedProcessDescription.Text;
        }

        private void textBoxPermittedProcessExecutable_TextChanged(object sender, EventArgs e)
        {
            if (permittedProcessIndex < 0) return;
            permittedProcessList =               (List<object>)sebSettingsNew[MessagePermittedProcesses];
            permittedProcessData = (Dictionary<string, object>)permittedProcessList [permittedProcessIndex];
            permittedProcessData[MessageExecutable] =   textBoxPermittedProcessExecutable.Text;
            String                      executable  =   textBoxPermittedProcessExecutable.Text;
            dataGridViewPermittedProcesses.Rows[permittedProcessIndex].Cells[IntColumnProcessExecutable].Value = executable;
        }

        private void textBoxPermittedProcessPath_TextChanged(object sender, EventArgs e)
        {
            if (permittedProcessIndex < 0) return;
            permittedProcessList =               (List<object>)sebSettingsNew[MessagePermittedProcesses];
            permittedProcessData = (Dictionary<string, object>)permittedProcessList [permittedProcessIndex];
            permittedProcessData[MessagePath] =         textBoxPermittedProcessPath.Text;
        }

        private void textBoxPermittedProcessIdentifier_TextChanged(object sender, EventArgs e)
        {
            if (permittedProcessIndex < 0) return;
            permittedProcessList =               (List<object>)sebSettingsNew[MessagePermittedProcesses];
            permittedProcessData = (Dictionary<string, object>)permittedProcessList [permittedProcessIndex];
            permittedProcessData[MessageIdentifier] =   textBoxPermittedProcessIdentifier.Text;
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
            permittedArgumentIndex = dataGridViewPermittedProcessArguments.SelectedRows[0].Index;
            permittedProcessList   =               (List<object>)sebSettingsNew[MessagePermittedProcesses];
            permittedProcessData   = (Dictionary<string, object>)permittedProcessList [permittedProcessIndex];
            permittedArgumentList  =               (List<object>)permittedProcessData [MessageArguments];
            permittedArgumentData  = (Dictionary<string, object>)permittedArgumentList[permittedArgumentIndex];
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
            permittedArgumentIndex = row;
            permittedProcessList   =               (List<object>)sebSettingsNew[MessagePermittedProcesses];
            permittedProcessData   = (Dictionary<string, object>)permittedProcessList [permittedProcessIndex];
            permittedArgumentList  =               (List<object>)permittedProcessData [MessageArguments];
            permittedArgumentData  = (Dictionary<string, object>)permittedArgumentList[permittedArgumentIndex];

            // Update the argument data belonging to the current cell
            if (column == IntColumnProcessActive  ) permittedArgumentData[MessageActive  ] = (Boolean)value;
            if (column == IntColumnProcessArgument) permittedArgumentData[MessageArgument] = (String )value;
        }


        private void buttonPermittedProcessAddArgument_Click(object sender, EventArgs e)
        {
            // Get the permitted argument list
            permittedProcessList  =               (List<object>)sebSettingsNew[MessagePermittedProcesses];
            permittedProcessData  = (Dictionary<string, object>)permittedProcessList [permittedProcessIndex];
            permittedArgumentList =               (List<object>)permittedProcessData [MessageArguments];

            if (permittedArgumentList.Count > 0)
            {
                if (dataGridViewPermittedProcessArguments.SelectedRows.Count != 1) return;
              //permittedArgumentIndex = dataGridViewPermittedProcessArguments.SelectedRows[0].Index;
                permittedArgumentIndex = permittedArgumentList.Count;
            }
            else
            {
                // If argument list was empty before, enable it
                permittedArgumentIndex = 0;
                dataGridViewPermittedProcessArguments.Enabled = true;
            }

            // Create new argument dataset containing default values
            Dictionary<string, object> argumentData = new Dictionary<string, object>();

            argumentData[MessageActive  ] = true;
            argumentData[MessageArgument] = "";

            // Insert new argument into argument list at position permittedArgumentIndex
            permittedArgumentList                     .Insert(permittedArgumentIndex, argumentData);
            dataGridViewPermittedProcessArguments.Rows.Insert(permittedArgumentIndex, true, "");
            dataGridViewPermittedProcessArguments.Rows       [permittedArgumentIndex].Selected = true;
        }


        private void buttonPermittedProcessRemoveArgument_Click(object sender, EventArgs e)
        {
            if (dataGridViewPermittedProcessArguments.SelectedRows.Count != 1) return;

            // Get the permitted argument list
            permittedArgumentIndex = dataGridViewPermittedProcessArguments.SelectedRows[0].Index;
            permittedProcessList   =               (List<object>)sebSettingsNew[MessagePermittedProcesses];
            permittedProcessData   = (Dictionary<string, object>)permittedProcessList [permittedProcessIndex];
            permittedArgumentList  =               (List<object>)permittedProcessData [MessageArguments];

            // Delete argument from argument list at position permittedArgumentIndex
            permittedArgumentList                     .RemoveAt(permittedArgumentIndex);
            dataGridViewPermittedProcessArguments.Rows.RemoveAt(permittedArgumentIndex);

            if (permittedArgumentIndex == permittedArgumentList.Count)
                permittedArgumentIndex--;

            if (permittedArgumentList.Count > 0)
            {
                dataGridViewPermittedProcessArguments.Rows[permittedArgumentIndex].Selected = true;
            }
            else
            {
                // If argument list is now empty, disable it
                permittedArgumentIndex = -1;
              //permittedArgumentList.Clear();
              //permittedArgumentData.Clear();
                dataGridViewPermittedProcessArguments.Enabled = false;
            }
        }



        // *******************************************
        // Group "Applications - Prohibited Processes"
        // *******************************************
        private void LoadAndUpdateProhibitedSelectedProcessGroup(int selectedProcessIndex)
        {
            // Get the process data of the selected process
            prohibitedProcessList  =               (List<object>)sebSettingsNew[MessageProhibitedProcesses];
            prohibitedProcessData  = (Dictionary<string, object>)prohibitedProcessList[selectedProcessIndex];

            // Update the widgets in the "Selected Process" group
            checkBoxProhibitedProcessActive     .Checked = (Boolean)prohibitedProcessData[MessageActive];
            checkBoxProhibitedProcessCurrentUser.Checked = (Boolean)prohibitedProcessData[MessageCurrentUser];
            checkBoxProhibitedProcessStrongKill .Checked = (Boolean)prohibitedProcessData[MessageStrongKill];
             listBoxProhibitedProcessOS.SelectedIndex    =   (Int32)prohibitedProcessData[MessageOS];
             textBoxProhibitedProcessExecutable .Text    =  (String)prohibitedProcessData[MessageExecutable];
             textBoxProhibitedProcessDescription.Text    =  (String)prohibitedProcessData[MessageDescription];
             textBoxProhibitedProcessIdentifier .Text    =  (String)prohibitedProcessData[MessageIdentifier];
             textBoxProhibitedProcessUser       .Text    =  (String)prohibitedProcessData[MessageUser];
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
            prohibitedProcessIndex = dataGridViewProhibitedProcesses.SelectedRows[0].Index;

            // The process list should contain at least one element here:
            // prohibitedProcessList.Count >  0
            // prohibitedProcessIndex      >= 0
            LoadAndUpdateProhibitedSelectedProcessGroup(prohibitedProcessIndex);
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
            prohibitedProcessIndex = row;
            prohibitedProcessList  =               (List<object>)sebSettingsNew[MessageProhibitedProcesses];
            prohibitedProcessData  = (Dictionary<string, object>)prohibitedProcessList[prohibitedProcessIndex];

            // Update the process data belonging to the current cell
            if (column == IntColumnProcessActive     ) prohibitedProcessData[MessageActive     ] = (Boolean)value;
            if (column == IntColumnProcessOS         ) prohibitedProcessData[MessageOS         ] = (Int32  )value;
            if (column == IntColumnProcessExecutable ) prohibitedProcessData[MessageExecutable ] = (String )value;
            if (column == IntColumnProcessDescription) prohibitedProcessData[MessageDescription] = (String )value;

            // Update the widget belonging to the current cell (in "Selected Process" group)
            if (column == IntColumnProcessActive     ) checkBoxProhibitedProcessActive.Checked   = (Boolean)value;
            if (column == IntColumnProcessOS         )  listBoxProhibitedProcessOS.SelectedIndex = (Int32  )value;
            if (column == IntColumnProcessExecutable )  textBoxProhibitedProcessExecutable .Text = (String )value;
            if (column == IntColumnProcessDescription)  textBoxProhibitedProcessDescription.Text = (String )value;
        }


        private void buttonAddProhibitedProcess_Click(object sender, EventArgs e)
        {
            // Get the process list
            prohibitedProcessList = (List<object>)sebSettingsNew[MessageProhibitedProcesses];

            if (prohibitedProcessList.Count > 0)
            {
                if (dataGridViewProhibitedProcesses.SelectedRows.Count != 1) return;
              //prohibitedProcessIndex = dataGridViewProhibitedProcesses.SelectedRows[0].Index;
                prohibitedProcessIndex = prohibitedProcessList.Count;
            }
            else
            {
                // If process list was empty before, enable it
                prohibitedProcessIndex = 0;
                dataGridViewProhibitedProcesses.Enabled = true;
                    groupBoxProhibitedProcess  .Enabled = true;
            }

            // Create new process dataset containing default values
            Dictionary<string, object> processData = new Dictionary<string, object>();

            processData[MessageActive     ] = true;
            processData[MessageCurrentUser] = true;
            processData[MessageStrongKill ] = false;
            processData[MessageOS         ] = IntWin;
            processData[MessageExecutable ] = "";
            processData[MessageDescription] = "";
            processData[MessageIdentifier ] = "";
            processData[MessageUser       ] = "";

            // Insert new process into process list at position index
            prohibitedProcessList               .Insert(prohibitedProcessIndex, processData);
            dataGridViewProhibitedProcesses.Rows.Insert(prohibitedProcessIndex, true, StringOS[IntWin], "", "");
            dataGridViewProhibitedProcesses.Rows       [prohibitedProcessIndex].Selected = true;
        }


        private void buttonRemoveProhibitedProcess_Click(object sender, EventArgs e)
        {
            if (dataGridViewProhibitedProcesses.SelectedRows.Count != 1) return;

            // Clear the widgets in the "Selected Process" group
            ClearProhibitedSelectedProcessGroup();

            // Delete process from process list at position index
            prohibitedProcessIndex = dataGridViewProhibitedProcesses.SelectedRows[0].Index;
            prohibitedProcessList  = (List<object>)sebSettingsNew[MessageProhibitedProcesses];
            prohibitedProcessList               .RemoveAt(prohibitedProcessIndex);
            dataGridViewProhibitedProcesses.Rows.RemoveAt(prohibitedProcessIndex);

            if (prohibitedProcessIndex == prohibitedProcessList.Count)
                prohibitedProcessIndex--;

            if (prohibitedProcessList.Count > 0)
            {
                dataGridViewProhibitedProcesses.Rows[prohibitedProcessIndex].Selected = true;
            }
            else
            {
                // If process list is now empty, disable it
                prohibitedProcessIndex = -1;
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
            if (prohibitedProcessIndex < 0) return;
            prohibitedProcessList =               (List<object>)sebSettingsNew[MessageProhibitedProcesses];
            prohibitedProcessData = (Dictionary<string, object>)prohibitedProcessList[prohibitedProcessIndex];
            prohibitedProcessData[MessageActive] =      checkBoxProhibitedProcessActive.Checked;
            Boolean                      active  =      checkBoxProhibitedProcessActive.Checked;
            dataGridViewProhibitedProcesses.Rows[prohibitedProcessIndex].Cells[IntColumnProcessActive].Value = active.ToString();
        }

        private void checkBoxProhibitedProcessCurrentUser_CheckedChanged(object sender, EventArgs e)
        {
            if (prohibitedProcessIndex < 0) return;
            prohibitedProcessList =               (List<object>)sebSettingsNew[MessageProhibitedProcesses];
            prohibitedProcessData = (Dictionary<string, object>)prohibitedProcessList[prohibitedProcessIndex];
            prohibitedProcessData[MessageCurrentUser] = checkBoxProhibitedProcessCurrentUser.Checked;
        }

        private void checkBoxProhibitedProcessStrongKill_CheckedChanged(object sender, EventArgs e)
        {
            if (prohibitedProcessIndex < 0) return;
            prohibitedProcessList =               (List<object>)sebSettingsNew[MessageProhibitedProcesses];
            prohibitedProcessData = (Dictionary<string, object>)prohibitedProcessList[prohibitedProcessIndex];
            prohibitedProcessData[MessageStrongKill] =  checkBoxProhibitedProcessStrongKill.Checked;
        }

        private void listBoxProhibitedProcessOS_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (prohibitedProcessIndex < 0) return;
            prohibitedProcessList =               (List<object>)sebSettingsNew[MessageProhibitedProcesses];
            prohibitedProcessData = (Dictionary<string, object>)prohibitedProcessList[prohibitedProcessIndex];
            prohibitedProcessData[MessageOS] =           listBoxProhibitedProcessOS.SelectedIndex;
            Int32                        os  =           listBoxProhibitedProcessOS.SelectedIndex;
            dataGridViewProhibitedProcesses.Rows[prohibitedProcessIndex].Cells[IntColumnProcessOS].Value = StringOS[os];
        }

        private void textBoxProhibitedProcessExecutable_TextChanged(object sender, EventArgs e)
        {
            if (prohibitedProcessIndex < 0) return;
            prohibitedProcessList =               (List<object>)sebSettingsNew[MessageProhibitedProcesses];
            prohibitedProcessData = (Dictionary<string, object>)prohibitedProcessList[prohibitedProcessIndex];
            prohibitedProcessData[MessageExecutable] =   textBoxProhibitedProcessExecutable.Text;
            String                       executable  =   textBoxProhibitedProcessExecutable.Text;
            dataGridViewProhibitedProcesses.Rows[prohibitedProcessIndex].Cells[IntColumnProcessExecutable].Value = executable;
        }

        private void textBoxProhibitedProcessDescription_TextChanged(object sender, EventArgs e)
        {
            if (prohibitedProcessIndex < 0) return;
            prohibitedProcessList =               (List<object>)sebSettingsNew[MessageProhibitedProcesses];
            prohibitedProcessData = (Dictionary<string, object>)prohibitedProcessList[prohibitedProcessIndex];
            prohibitedProcessData[MessageDescription] =   textBoxProhibitedProcessDescription.Text;
            String                       description  =   textBoxProhibitedProcessDescription.Text;
            dataGridViewProhibitedProcesses.Rows[prohibitedProcessIndex].Cells[IntColumnProcessDescription].Value = description;
        }

        private void textBoxProhibitedProcessIdentifier_TextChanged(object sender, EventArgs e)
        {
            if (prohibitedProcessIndex < 0) return;
            prohibitedProcessList =               (List<object>)sebSettingsNew[MessageProhibitedProcesses];
            prohibitedProcessData = (Dictionary<string, object>)prohibitedProcessList[prohibitedProcessIndex];
            prohibitedProcessData[MessageIdentifier] =   textBoxProhibitedProcessIdentifier.Text;
        }

        private void textBoxProhibitedProcessUser_TextChanged(object sender, EventArgs e)
        {
            if (prohibitedProcessIndex < 0) return;
            prohibitedProcessList =               (List<object>)sebSettingsNew[MessageProhibitedProcesses];
            prohibitedProcessData = (Dictionary<string, object>)prohibitedProcessList[prohibitedProcessIndex];
            prohibitedProcessData[MessageUser] =         textBoxProhibitedProcessUser.Text;
        }

        private void buttonProhibitedProcessCodeSignature_Click(object sender, EventArgs e)
        {

        }



        // ************************
        // Group "Network - Filter"
        // ************************
        private void checkBoxEnableURLFilter_CheckedChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageEnableURLFilter] = checkBoxEnableURLFilter.Checked;
        }

        private void checkBoxEnableURLContentFilter_CheckedChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageEnableURLContentFilter] = checkBoxEnableURLContentFilter.Checked;
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
            urlFilterTableRow    = row;
            urlFilterIsTitleRow  = urlFilterTableIsTitleRow [urlFilterTableRow];
            urlFilterRuleIndex   = urlFilterTableRuleIndex  [urlFilterTableRow];
            urlFilterActionIndex = urlFilterTableActionIndex[urlFilterTableRow];

            // Get the rule data belonging to the current row
            urlFilterRuleList =               (List<object>)sebSettingsNew[MessageURLFilterRules];
            urlFilterRuleData = (Dictionary<string, object>)urlFilterRuleList[urlFilterRuleIndex];

            // Update the rule data belonging to the current cell
            if (urlFilterIsTitleRow)
            {
                if (column == IntColumnURLFilterRuleActive    ) urlFilterRuleData[MessageActive    ] = (Boolean)value;
                if (column == IntColumnURLFilterRuleExpression) urlFilterRuleData[MessageExpression] = (String )value;
              //if (column == IntColumnURLFilterRuleShow      ) urlFilterTableShowRule[urlFilterRuleIndex] = (Boolean)value;
              //if (column == IntColumnURLFilterRuleShow      ) UpdateTableOfURLFilterRules();
            }
            else
            {
                // Get the action data belonging to the current cell
                urlFilterActionList =               (List<object>)urlFilterRuleData[MessageRuleActions];
                urlFilterActionData = (Dictionary<string, object>)urlFilterActionList[urlFilterActionIndex];

                if (column == IntColumnURLFilterRuleActive    ) urlFilterActionData[MessageActive    ] = (Boolean)value;
                if (column == IntColumnURLFilterRuleRegex     ) urlFilterActionData[MessageRegex     ] = (Boolean)value;
                if (column == IntColumnURLFilterRuleExpression) urlFilterActionData[MessageExpression] = (String )value;
                if (column == IntColumnURLFilterRuleAction    ) urlFilterActionData[MessageAction    ] = (Int32  )value;
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
            urlFilterTableRow    = row;
            urlFilterIsTitleRow  = urlFilterTableIsTitleRow [urlFilterTableRow];
            urlFilterRuleIndex   = urlFilterTableRuleIndex  [urlFilterTableRow];
            urlFilterActionIndex = urlFilterTableActionIndex[urlFilterTableRow];

            // Check if the button "Collapse" or "Expand" was pressed
            if (urlFilterIsTitleRow)
            {
                // Convert the selected "Show" Button value from String to Boolean
                if (column == IntColumnURLFilterRuleShow)
                {
                         if ((String)value == StringCollapse) value = false;
                    else if ((String)value == StringExpand  ) value = true;

                    // If "Collapse" was pressed, set Show flag of this rule to false.
                    // If "Expand"   was pressed, set Show flag of this rule to true.
                    // Update the URL filter table according to the new rule flags.
                    urlFilterTableShowRule[urlFilterRuleIndex] = (Boolean)value;
                    UpdateTableOfURLFilterRules();
                }
            }
        }


        private void InsertPasteRuleAction(int operation, int location)
        {
            // Get the rule list
            urlFilterRuleList = (List<object>)sebSettingsNew[MessageURLFilterRules];

            if (urlFilterRuleList.Count > 0)
            {
                // Determine if the selected row is a title row or action row.
                // Determine which rule and action belong to the selected row.
                urlFilterTableRow    = dataGridViewURLFilterRules.SelectedRows[0].Index;
                urlFilterIsTitleRow  = urlFilterTableIsTitleRow [urlFilterTableRow];
                urlFilterRuleIndex   = urlFilterTableRuleIndex  [urlFilterTableRow];
                urlFilterActionIndex = urlFilterTableActionIndex[urlFilterTableRow];
            }
            else
            {
                // If rule list was empty before, enable it
                urlFilterTableRow    =  0;
                urlFilterIsTitleRow  =  true;
                urlFilterRuleIndex   =  0;
                urlFilterActionIndex = -1;
            }

            // If the user clicked onto a TITLE row (RULE),
            // add a new rule BEFORE or AFTER the current rule.
            if (urlFilterIsTitleRow)
            {
                // If the rule is added AFTER current selection, increment the rule index
                // (exception: when rule list was empty, rule index becomes 0 in any case)
                if ((location == IntLocationAfter) && (urlFilterRuleList.Count > 0))
                    urlFilterRuleIndex++;

                // Load default rule for Insert operation.
                // Load stored  rule for Paste  operation.
                if (operation == IntOperationInsert) urlFilterRuleData = urlFilterRuleDataDef;
                if (operation == IntOperationPaste ) urlFilterRuleData = urlFilterRuleDataStored;

                // INSERT or PASTE new rule into rule list at correct position index
                urlFilterRuleList     .Insert(urlFilterRuleIndex, urlFilterRuleData);
                urlFilterTableShowRule.Insert(urlFilterRuleIndex, true);
            }
            // If the user clicked onto an ACTION row,
            // add a new action BEFORE or AFTER the current action.
            else
            {
                // Get the action list
                urlFilterRuleData   = (Dictionary<string, object>)urlFilterRuleList[urlFilterRuleIndex];
                urlFilterActionList =               (List<object>)urlFilterRuleData[MessageRuleActions];

                // If the action is added AFTER current selection, increment the action index
                if (location == IntLocationAfter)
                    urlFilterActionIndex++;

                // Load default action for Insert operation.
                // Load stored  action for Paste  operation.
                if (operation == IntOperationInsert) urlFilterActionData = urlFilterActionDataDef;
                if (operation == IntOperationPaste ) urlFilterActionData = urlFilterActionDataStored;

                // INSERT or PASTE new action into action list at correct position index
                urlFilterActionList.Insert(urlFilterActionIndex, urlFilterActionData);
            }

            // Update the table of URL Filter Rules
            UpdateTableOfURLFilterRules();
        }


        private void CopyCutDeleteRuleAction(int operation)
        {
            // Get the rule list
            urlFilterRuleList = (List<object>)sebSettingsNew[MessageURLFilterRules];

            if (urlFilterRuleList.Count > 0)
            {
                // Determine if the selected row is a title row or action row.
                // Determine which rule and action belong to the selected row.
                urlFilterTableRow    = dataGridViewURLFilterRules.SelectedRows[0].Index;
                urlFilterIsTitleRow  = urlFilterTableIsTitleRow [urlFilterTableRow];
                urlFilterRuleIndex   = urlFilterTableRuleIndex  [urlFilterTableRow];
                urlFilterActionIndex = urlFilterTableActionIndex[urlFilterTableRow];
            }
            else
            {
                // If rule list is empty, abort since nothing can be deleted anymore
                return;
            }

            // If the user clicked onto a TITLE row (RULE), delete this rule
            if (urlFilterIsTitleRow)
            {
                if ((operation == IntOperationCopy) || (operation == IntOperationCut))
                {
                    // Store currently selected rule for later Paste operation
                    urlFilterRuleDataStored = (Dictionary<string, object>)urlFilterRuleList[urlFilterRuleIndex];
                }

                if ((operation == IntOperationDelete) || (operation == IntOperationCut))
                {
                    // Delete rule from rule list at position index
                    urlFilterRuleList     .RemoveAt(urlFilterRuleIndex);
                    urlFilterTableShowRule.RemoveAt(urlFilterRuleIndex);
                    if (urlFilterRuleIndex == urlFilterRuleList.Count) urlFilterRuleIndex--;
                }
            }
            // If the user clicked onto an ACTION row, delete this action
            else
            {
                // Get the action list
                urlFilterRuleData   = (Dictionary<string, object>)urlFilterRuleList[urlFilterRuleIndex];
                urlFilterActionList =               (List<object>)urlFilterRuleData[MessageRuleActions];

                if ((operation == IntOperationCopy) || (operation == IntOperationCut))
                {
                    // Store currently selected action for later Paste operation
                    urlFilterActionDataStored = (Dictionary<string, object>)urlFilterActionList[urlFilterActionIndex];
                }

                if ((operation == IntOperationDelete) || (operation == IntOperationCut))
                {
                    // Delete action from action list at position index
                    urlFilterActionList.RemoveAt(urlFilterActionIndex);
                    if (urlFilterActionIndex == urlFilterActionList.Count) urlFilterActionIndex--;
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
            embeddedCertificateIndex = dataGridViewEmbeddedCertificates.SelectedRows[0].Index;
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
            embeddedCertificateIndex = row;
            embeddedCertificateList  =               (List<object>)sebSettingsNew[MessageEmbeddedCertificates];
            embeddedCertificateData  = (Dictionary<string, object>)embeddedCertificateList[embeddedCertificateIndex];

            // Update the certificate data belonging to the current cell
            if (column == IntColumnCertificateType) embeddedCertificateData[MessageType] = (Int32  )value;
            if (column == IntColumnCertificateName) embeddedCertificateData[MessageName] = (String )value;
        }


        private void buttonRemoveEmbeddedCertificate_Click(object sender, EventArgs e)
        {
            if (dataGridViewEmbeddedCertificates.SelectedRows.Count != 1) return;
            embeddedCertificateIndex = dataGridViewEmbeddedCertificates.SelectedRows[0].Index;

            // Delete certificate from certificate list at position index
            embeddedCertificateList = (List<object>)sebSettingsNew[MessageEmbeddedCertificates];
            embeddedCertificateList              .RemoveAt(embeddedCertificateIndex);
            dataGridViewEmbeddedCertificates.Rows.RemoveAt(embeddedCertificateIndex);

            if (embeddedCertificateIndex == embeddedCertificateList.Count)
                embeddedCertificateIndex--;

            if (embeddedCertificateList.Count > 0)
            {
                dataGridViewEmbeddedCertificates.Rows[embeddedCertificateIndex].Selected = true;
            }
            else
            {
                // If certificate list is now empty, disable it
                embeddedCertificateIndex = -1;
                dataGridViewEmbeddedCertificates.Enabled = false;
            }
        }



        // *************************
        // Group "Network - Proxies"
        // *************************
        private void radioButtonUseSystemProxySettings_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonUseSystemProxySettings.Checked == true)
                 sebSettingsNew[MessageProxySettingsPolicy] = 0;
            else sebSettingsNew[MessageProxySettingsPolicy] = 1;
        }

        private void radioButtonUseSebProxySettings_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonUseSebProxySettings.Checked == true)
                 sebSettingsNew[MessageProxySettingsPolicy] = 1;
            else sebSettingsNew[MessageProxySettingsPolicy] = 0;
        }

        private void checkBoxExcludeSimpleHostnames_CheckedChanged(object sender, EventArgs e)
        {
            // Get the proxies data
            proxiesData = (Dictionary<string, object>)sebSettingsNew[MessageProxies];
            proxiesData[MessageExcludeSimpleHostnames] = checkBoxExcludeSimpleHostnames.Checked;
        }

        private void checkBoxUsePassiveFTPMode_CheckedChanged(object sender, EventArgs e)
        {
            // Get the proxies data
            proxiesData = (Dictionary<string, object>)sebSettingsNew[MessageProxies];
            proxiesData[MessageFTPPassive] = checkBoxUsePassiveFTPMode.Checked;
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
            proxyProtocolIndex = dataGridViewProxyProtocols.SelectedRows[0].Index;

            // if proxyProtocolIndex is    0 (AutoDiscovery    ), do nothing
            // if proxyProtocolIndex is    1 (AutoConfiguration), enable Proxy URL    widgets
            // if proxyProtocolIndex is >= 2 (... Proxy Server ), enable Proxy Server widgets

            Boolean useAutoConfiguration = (proxyProtocolIndex == IntProxyAutoConfiguration);
            Boolean useProxyServer       = (proxyProtocolIndex  > IntProxyAutoConfiguration);

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
                labelProxyServerHost.Text  = StringProxyProtocolServerLabel[proxyProtocolIndex];
                labelProxyServerHost.Text += " Proxy Server";
            }

            // Get the proxy protocol type
            String MessageProtocolType = MessageProxyProtocolType[proxyProtocolIndex];

            // Get the proxies data
            proxiesData = (Dictionary<string, object>)sebSettingsNew[MessageProxies];

            // Update the proxy widgets
            if (useAutoConfiguration)
            {
                textBoxAutoProxyConfigurationURL.Text = (String)proxiesData[MessageAutoConfigurationURL];
            }

            if (useProxyServer)
            {
                checkBoxProxyServerRequires.Checked = (Boolean)proxiesData[MessageProtocolType + MessageRequires];
                 textBoxProxyServerHost    .Text    =  (String)proxiesData[MessageProtocolType + MessageHost    ];
                 textBoxProxyServerPort    .Text    =  (String)proxiesData[MessageProtocolType + MessagePort    ].ToString();
                 textBoxProxyServerUsername.Text    =  (String)proxiesData[MessageProtocolType + MessageUsername];
                 textBoxProxyServerPassword.Text    =  (String)proxiesData[MessageProtocolType + MessagePassword];

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
            proxiesData = (Dictionary<string, object>)sebSettingsNew[MessageProxies];

            proxyProtocolIndex = row;

            // Update the proxy enable data belonging to the current cell
            if (column == IntColumnProxyProtocolEnable)
            {
                String key = MessageProxyProtocolEnableKey[row];
                proxiesData[key]                 = (Boolean)value;
                BooleanProxyProtocolEnabled[row] = (Boolean)value;
            }
        }


        private void textBoxAutoProxyConfigurationURL_TextChanged(object sender, EventArgs e)
        {
            // Get the proxies data
            proxiesData = (Dictionary<string, object>)sebSettingsNew[MessageProxies];
            proxiesData[MessageAutoConfigurationURL] = textBoxAutoProxyConfigurationURL.Text;
        }

        private void buttonChooseProxyConfigurationFile_Click(object sender, EventArgs e)
        {

        }


        private void textBoxProxyServerHost_TextChanged(object sender, EventArgs e)
        {
            // Get the proxies data
            String key       = MessageProxyProtocolType[proxyProtocolIndex] + MessageHost;
            proxiesData      = (Dictionary<string, object>)sebSettingsNew[MessageProxies];
            proxiesData[key] = textBoxProxyServerHost.Text;
        }

        private void textBoxProxyServerPort_TextChanged(object sender, EventArgs e)
        {
            // Get the proxies data
            String key  = MessageProxyProtocolType[proxyProtocolIndex] + MessagePort;
            proxiesData = (Dictionary<string, object>)sebSettingsNew[MessageProxies];

            // Convert the "Port" string to an integer
            try
            {
                proxiesData[key] = Int32.Parse(textBoxProxyServerPort.Text);
            }
            catch (FormatException)
            {
                textBoxProxyServerPort.Text = "";
            }
        }

        private void checkBoxProxyServerRequiresPassword_CheckedChanged(object sender, EventArgs e)
        {
            // Get the proxies data
            String key       = MessageProxyProtocolType[proxyProtocolIndex] + MessageRequires;
            proxiesData      = (Dictionary<string, object>)sebSettingsNew[MessageProxies];
            proxiesData[key] = (Boolean)checkBoxProxyServerRequires.Checked;

            // Disable the username/password textboxes when they are not required
            textBoxProxyServerUsername.Enabled = checkBoxProxyServerRequires.Checked;
            textBoxProxyServerPassword.Enabled = checkBoxProxyServerRequires.Checked;
        }

        private void textBoxProxyServerUsername_TextChanged(object sender, EventArgs e)
        {
            // Get the proxies data
            String key       = MessageProxyProtocolType[proxyProtocolIndex] + MessageUsername;
            proxiesData      = (Dictionary<string, object>)sebSettingsNew[MessageProxies];
            proxiesData[key] = textBoxProxyServerUsername.Text;
        }

        private void textBoxProxyServerPassword_TextChanged(object sender, EventArgs e)
        {
            // Get the proxies data
            String key       = MessageProxyProtocolType[proxyProtocolIndex] + MessagePassword;
            proxiesData      = (Dictionary<string, object>)sebSettingsNew[MessageProxies];
            proxiesData[key] = textBoxProxyServerPassword.Text;
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
            bypassedProxyIndex = dataGridViewBypassedProxies.SelectedRows[0].Index;
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
            proxiesData = (Dictionary<string, object>)sebSettingsNew[MessageProxies];

            bypassedProxyIndex = row;
            bypassedProxyList  = (List<object>)proxiesData[MessageExceptionsList];

            // Update the certificate data belonging to the current cell
            if (column == IntColumnDomainHostPort) bypassedProxyList[bypassedProxyIndex] = (String)value;
        }



        // ****************
        // Group "Security"
        // ****************
        private void listBoxSebServicePolicy_SelectedIndexChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageSebServicePolicy] = listBoxSebServicePolicy.SelectedIndex;
        }

        private void checkBoxAllowVirtualMachine_CheckedChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageAllowVirtualMachine] = checkBoxAllowVirtualMachine.Checked;
        }

        private void checkBoxCreateNewDesktop_CheckedChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageCreateNewDesktop] = checkBoxCreateNewDesktop.Checked;
        }

        private void checkBoxKillExplorerShell_CheckedChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageKillExplorerShell] = checkBoxKillExplorerShell.Checked;
        }

        private void checkBoxAllowUserSwitching_CheckedChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageAllowUserSwitching] = checkBoxAllowUserSwitching.Checked;
        }

        private void checkBoxEnableLogging_CheckedChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageEnableLogging] = checkBoxEnableLogging.Checked;
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
            sebSettingsNew[MessageLogDirectoryWin]     = path;
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
            sebSettingsNew[MessageInsideSebEnableSwitchUser] = checkBoxInsideSebEnableSwitchUser.Checked;
        }

        private void checkBoxInsideSebEnableLockThisComputer_CheckedChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageInsideSebEnableLockThisComputer] = checkBoxInsideSebEnableLockThisComputer.Checked;
        }

        private void checkBoxInsideSebEnableChangeAPassword_CheckedChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageInsideSebEnableChangeAPassword] = checkBoxInsideSebEnableChangeAPassword.Checked;
        }

        private void checkBoxInsideSebEnableStartTaskManager_CheckedChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageInsideSebEnableStartTaskManager] = checkBoxInsideSebEnableStartTaskManager.Checked;
        }

        private void checkBoxInsideSebEnableLogOff_CheckedChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageInsideSebEnableLogOff] = checkBoxInsideSebEnableLogOff.Checked;
        }

        private void checkBoxInsideSebEnableShutDown_CheckedChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageInsideSebEnableShutDown] = checkBoxInsideSebEnableShutDown.Checked;
        }

        private void checkBoxInsideSebEnableEaseOfAccess_CheckedChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageInsideSebEnableEaseOfAccess] = checkBoxInsideSebEnableEaseOfAccess.Checked;
        }

        private void checkBoxInsideSebEnableVmWareClientShade_CheckedChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageInsideSebEnableVmWareClientShade] = checkBoxInsideSebEnableVmWareClientShade.Checked;
        }



        // *******************
        // Group "Outside SEB"
        // *******************
        private void checkBoxOutsideSebEnableSwitchUser_CheckedChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageOutsideSebEnableSwitchUser] = checkBoxOutsideSebEnableSwitchUser.Checked;
        }

        private void checkBoxOutsideSebEnableLockThisComputer_CheckedChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageOutsideSebEnableLockThisComputer] = checkBoxOutsideSebEnableLockThisComputer.Checked;
        }

        private void checkBoxOutsideSebEnableChangeAPassword_CheckedChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageOutsideSebEnableChangeAPassword] = checkBoxOutsideSebEnableChangeAPassword.Checked;
        }

        private void checkBoxOutsideSebEnableStartTaskManager_CheckedChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageOutsideSebEnableStartTaskManager] = checkBoxOutsideSebEnableStartTaskManager.Checked;
        }

        private void checkBoxOutsideSebEnableLogOff_CheckedChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageOutsideSebEnableLogOff] = checkBoxOutsideSebEnableLogOff.Checked;
        }

        private void checkBoxOutsideSebEnableShutDown_CheckedChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageOutsideSebEnableShutDown] = checkBoxOutsideSebEnableShutDown.Checked;
        }

        private void checkBoxOutsideSebEnableEaseOfAccess_CheckedChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageOutsideSebEnableEaseOfAccess] = checkBoxOutsideSebEnableEaseOfAccess.Checked;
        }

        private void checkBoxOutsideSebEnableVmWareClientShade_CheckedChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageOutsideSebEnableVmWareClientShade] = checkBoxOutsideSebEnableVmWareClientShade.Checked;
        }



        // *******************
        // Group "Hooked Keys"
        // *******************
        private void checkBoxHookKeys_CheckedChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageHookKeys] = checkBoxHookKeys.Checked;
        }



        // ********************
        // Group "Special Keys"
        // ********************
        private void checkBoxEnableEsc_CheckedChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageEnableEsc] = checkBoxEnableEsc.Checked;
        }

        private void checkBoxEnableCtrlEsc_CheckedChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageEnableCtrlEsc] = checkBoxEnableCtrlEsc.Checked;
        }

        private void checkBoxEnableAltEsc_CheckedChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageEnableAltEsc] = checkBoxEnableAltEsc.Checked;
        }

        private void checkBoxEnableAltTab_CheckedChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageEnableAltTab] = checkBoxEnableAltTab.Checked;
        }

        private void checkBoxEnableAltF4_CheckedChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageEnableAltF4] = checkBoxEnableAltF4.Checked;
        }

        private void checkBoxEnableStartMenu_CheckedChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageEnableStartMenu] = checkBoxEnableStartMenu.Checked;
        }

        private void checkBoxEnableRightMouse_CheckedChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageEnableRightMouse] = checkBoxEnableRightMouse.Checked;
        }



        // *********************
        // Group "Function Keys"
        // *********************
        private void checkBoxEnableF1_CheckedChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageEnableF1] = checkBoxEnableF1.Checked;
        }

        private void checkBoxEnableF2_CheckedChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageEnableF2] = checkBoxEnableF2.Checked;
        }

        private void checkBoxEnableF3_CheckedChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageEnableF3] = checkBoxEnableF3.Checked;
        }

        private void checkBoxEnableF4_CheckedChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageEnableF4] = checkBoxEnableF4.Checked;
        }

        private void checkBoxEnableF5_CheckedChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageEnableF5] = checkBoxEnableF5.Checked;
        }

        private void checkBoxEnableF6_CheckedChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageEnableF6] = checkBoxEnableF6.Checked;
        }

        private void checkBoxEnableF7_CheckedChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageEnableF7] = checkBoxEnableF7.Checked;
        }

        private void checkBoxEnableF8_CheckedChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageEnableF8] = checkBoxEnableF8.Checked;
        }

        private void checkBoxEnableF9_CheckedChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageEnableF9] = checkBoxEnableF9.Checked;
        }

        private void checkBoxEnableF10_CheckedChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageEnableF10] = checkBoxEnableF10.Checked;
        }

        private void checkBoxEnableF11_CheckedChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageEnableF11] = checkBoxEnableF11.Checked;
        }

        private void checkBoxEnableF12_CheckedChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageEnableF12] = checkBoxEnableF12.Checked;
        }



    } // end of   class     SebWindowsConfigForm
}     // end of   namespace SebWindowsConfig
