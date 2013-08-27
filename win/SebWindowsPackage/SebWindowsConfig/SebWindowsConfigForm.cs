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

        // ******************************
        //
        // Constants and global variables
        //
        // ******************************

        // The default SEB configuration file
        const String DefaultSebConfigXml = "SebClient.xml";
        const String DefaultSebConfigSeb = "SebClient.seb";

        // The values can be in 4 different states:
        // old, new, temporary and default values
        const int StateOld = 1;
        const int StateNew = 2;
        const int StateTmp = 3;
        const int StateDef = 4;
        const int StateNum = 4;

        // 5 values are not stored in the sebSettings Plist structures,
        // so they must be separately stored in arrays
        const int ValueCryptoIdentity               = 1;
        const int ValueMainBrowserWindowWidth       = 2;
        const int ValueMainBrowserWindowHeight      = 3;
        const int ValueNewBrowserWindowByLinkWidth  = 4;
        const int ValueNewBrowserWindowByLinkHeight = 5;
        const int ValueNum = 5;

        // Group "General"
        const String MessageStartURL             = "startURL";
        const String MessageSebServerURL         = "sebServerURL";
        const String MessageAdminPassword        = "adminPassword";
        const String MessageConfirmAdminPassword = "confirmAdminPassword";
        const String MessageHashedAdminPassword  = "hashedAdminPassword";
        const String MessageAllowQuit            = "allowQuit";
        const String MessageIgnoreQuitPassword   = "ignoreQuitPassword";
        const String MessageQuitPassword         = "quitPassword";
        const String MessageConfirmQuitPassword  = "confirmQuitPassword";
        const String MessageHashedQuitPassword   = "hashedQuitPassword";
        const String MessageExitKey1             = "exitKey1";
        const String MessageExitKey2             = "exitKey2";
        const String MessageExitKey3             = "exitKey3";
        const String MessageSebMode              = "sebMode";

        // Group "Config File"
        const String MessageSebConfigPurpose        = "sebConfigPurpose";
        const String MessageAllowPreferencesWindow  = "allowPreferencesWindow";
        const String MessageCryptoIdentity          = "cryptoIdentity";
        const String MessageSettingsPassword        = "settingsPassword";
        const String MessageConfirmSettingsPassword = "confirmSettingsPassword";
        const String MessageHashedSettingsPassword  = "hashedSettingsPassword";

        // Group "Appearance"
        const String MessageBrowserViewMode              = "browserViewMode";
        const String MessageMainBrowserWindowWidth       = "mainBrowserWindowWidth";
        const String MessageMainBrowserWindowHeight      = "mainBrowserWindowHeight";
        const String MessageMainBrowserWindowPositioning = "mainBrowserWindowPositioning";
        const String MessageEnableBrowserWindowToolbar   = "enableBrowserWindowToolbar";
        const String MessageHideBrowserWindowToolbar     = "hideBrowserWindowToolbar";
        const String MessageShowMenuBar                  = "showMenuBar";
        const String MessageShowTaskBar                  = "showTaskBar";

        // Group "Browser"
        const String MessageNewBrowserWindowByLinkPolicy         = "newBrowserWindowByLinkPolicy";
        const String MessageNewBrowserWindowByScriptPolicy       = "newBrowserWindowByScriptPolicy";
        const String MessageNewBrowserWindowByLinkBlockForeign   = "newBrowserWindowByLinkBlockForeign";
        const String MessageNewBrowserWindowByScriptBlockForeign = "newBrowserWindowByScriptBlockForeign";
        const String MessageNewBrowserWindowByLinkWidth          = "newBrowserWindowByLinkWidth";
        const String MessageNewBrowserWindowByLinkHeight         = "newBrowserWindowByLinkHeight";
        const String MessageNewBrowserWindowByLinkPositioning    = "newBrowserWindowByLinkPositioning";
        const String MessageEnablePlugIns                        = "enablePlugIns";
        const String MessageEnableJava                           = "enableJava";
        const String MessageEnableJavaScript                     = "enableJavaScript";
        const String MessageBlockPopUpWindows                    = "blockPopUpWindows";
        const String MessageAllowBrowsingBackForward             = "allowBrowsingBackForward";
        const String MessageEnableSebBrowser                     = "enableSebBrowser";

        // Group "DownUploads"
        const String MessageAllowDownUploads         = "allowDownUploads";
        const String MessageDownloadDirectoryOSX     = "downloadDirectoryOSX";
        const String MessageDownloadDirectoryWin     = "downloadDirectoryWin";
        const String MessageOpenDownloads            = "openDownloads";
        const String MessageChooseFileToUploadPolicy = "chooseFileToUploadPolicy";
        const String MessageDownloadPDFFiles         = "downloadPDFFiles";

        // Group "Exam"
        const String MessageExamKeySalt        = "examKeySalt";
        const String MessageBrowserExamKey     = "browserExamKey";
        const String MessageCopyBrowserExamKey = "copyBrowserExamKeyToClipboardWhenQuitting";
        const String MessageSendBrowserExamKey = "sendBrowserExamKey";
        const String MessageQuitURL            = "quitURL";

        // Group "Applications"
        const String MessageMonitorProcesses = "monitorProcesses";

        // Group "Applications - Permitted  Processes"
        const String MessagePermittedProcesses        = "permittedProcesses";
        const String MessageAllowSwitchToApplications = "allowSwitchToApplications";
        const String MessageAllowFlashFullscreen      = "allowFlashFullscreen";

        // Group "Applications - Prohibited Processes"
        const String MessageProhibitedProcesses       = "prohibitedProcesses";

        const String MessageActive      = "active";
        const String MessageAutostart   = "autostart";
        const String MessageAutohide    = "autohide";
        const String MessageAllowUser   = "allowUserToChooseApp";
        const String MessageCurrentUser = "currentUser";
        const String MessageStrongKill  = "strongKill";
        const String MessageOS          = "os";
        const String MessageTitle       = "title";
        const String MessageDescription = "description";
        const String MessageExecutable  = "executable";
        const String MessagePath        = "path";
        const String MessageIdentifier  = "identifier";
        const String MessageUser        = "user";
        const String MessageArguments   = "arguments";
        const String MessageArgument    = "argument";

        // Group "Network"
        const String MessageEnableURLFilter        = "enableURLFilter";
        const String MessageEnableURLContentFilter = "enableURLContentFilter";

        // Group "Network - Filter"
        const String MessageURLFilterRules = "URLFilterRules";
        const String MessageExpression     = "expression";
        const String MessageRuleActions    = "ruleActions";
        const String MessageRegex          = "regex";
        const String MessageAction         = "action";

        // Group "Network - Certificates"
        const String MessageEmbedSSLServerCertificate = "EmbedSSLServerCertificate";
        const String MessageEmbedIdentity             = "EmbedIdentity";
        const String MessageEmbeddedCertificates      = "embeddedCertificates";
        const String MessageCertificateData           = "certificateData";
        const String MessageType                      = "type";
        const String MessageName                      = "name";

        // Group "Network - Proxies"
        const String MessageProxySettingsPolicy       = "proxySettingsPolicy";
        const String MessageProxyProtocol             = "proxyProtocol";
        const String MessageProxyConfigurationFileURL = "proxyConfigurationFileURL";
        const String MessageExcludeSimpleHostnames    = "excludeSimpleHostnames";
        const String MessageUsePassiveFTPMode         = "usePassiveFTPMode";
        const String MessageBypassHostsAndDomains     = "bypassHostsAndDomains";
        const String MessageBypassDomain              = "domain";
        const String MessageBypassHost                = "host";
        const String MessageBypassPort                = "port";

        // Group "Security"
        const String MessageSebServicePolicy    = "sebServicePolicy";
        const String MessageAllowVirtualMachine = "allowVirtualMachine";
        const String MessageCreateNewDesktop    = "createNewDesktop";
        const String MessageAllowUserSwitching  = "allowUserSwitching";
        const String MessageEnableLogging       = "enableLogging";
        const String MessageLogDirectoryOSX     = "logDirectoryOSX";
        const String MessageLogDirectoryWin     = "logDirectoryWin";

        // Group "Registry"

        // Group "Inside SEB"
        const String MessageInsideSebEnableSwitchUser        = "insideSebEnableSwitchUser";
        const String MessageInsideSebEnableLockThisComputer  = "insideSebEnableLockThisComputer";
        const String MessageInsideSebEnableChangeAPassword   = "insideSebEnableChangeAPassword";
        const String MessageInsideSebEnableStartTaskManager  = "insideSebEnableStartTaskManager";
        const String MessageInsideSebEnableLogOff            = "insideSebEnableLogOff";
        const String MessageInsideSebEnableShutDown          = "insideSebEnableShutDown";
        const String MessageInsideSebEnableEaseOfAccess      = "insideSebEnableEaseOfAccess";
        const String MessageInsideSebEnableVmWareClientShade = "insideSebEnableVmWareClientShade";

        // Group "Outside SEB"
        const String MessageOutsideSebEnableSwitchUser        = "outsideSebEnableSwitchUser";
        const String MessageOutsideSebEnableLockThisComputer  = "outsideSebEnableLockThisComputer";
        const String MessageOutsideSebEnableChangeAPassword   = "outsideSebEnableChangeAPassword";
        const String MessageOutsideSebEnableStartTaskManager  = "outsideSebEnableStartTaskManager";
        const String MessageOutsideSebEnableLogOff            = "outsideSebEnableLogOff";
        const String MessageOutsideSebEnableShutDown          = "outsideSebEnableShutDown";
        const String MessageOutsideSebEnableEaseOfAccess      = "outsideSebEnableEaseOfAccess";
        const String MessageOutsideSebEnableVmWareClientShade = "outsideSebEnableVmWareClientShade";

        // Group "Hooked Keys"
        const String MessageHookKeys = "hookKeys";

        // Group "Special Keys"
        const String MessageEnableEsc        = "enableEsc";
        const String MessageEnableCtrlEsc    = "enableCtrlEsc";
        const String MessageEnableAltEsc     = "enableAltEsc";
        const String MessageEnableAltTab     = "enableAltTab";
        const String MessageEnableAltF4      = "enableAltF4";
        const String MessageEnableStartMenu  = "enableStartMenu";
        const String MessageEnableRightMouse = "enableRightMouse";

        // Group "Function Keys"
        const String MessageEnableF1  = "enableF1";
        const String MessageEnableF2  = "enableF2";
        const String MessageEnableF3  = "enableF3";
        const String MessageEnableF4  = "enableF4";
        const String MessageEnableF5  = "enableF5";
        const String MessageEnableF6  = "enableF6";
        const String MessageEnableF7  = "enableF7";
        const String MessageEnableF8  = "enableF8";
        const String MessageEnableF9  = "enableF9";
        const String MessageEnableF10 = "enableF10";
        const String MessageEnableF11 = "enableF11";
        const String MessageEnableF12 = "enableF12";

        // Boolean values
        const int IntFalse = 0;
        const int IntTrue  = 1;

        // Operating systems
        const int IntOSX = 0;
        const int IntWin = 1;

        const String StringOSX = "OS X";
        const String StringWin = "Win";

        // URL filter actions
        const int IntBlock = 0;
        const int IntAllow = 1;
        const int IntSkip  = 2;
        const int IntAnd   = 3;
        const int IntOr    = 4;

        const String StringBlock = "block";
        const String StringAllow = "allow";
        const String StringSkip  = "skip";
        const String StringAnd   = "and";
        const String StringOr    = "or";

        // URL filter table operations
        const int IntOperationInsert = 0;
        const int IntOperationPaste  = 1;
        const int IntOperationDelete = 2;
        const int IntOperationCut    = 3;
        const int IntOperationCopy   = 4;

        const int IntLocationBefore = 0;
        const int IntLocationAfter  = 1;
        const int IntLocationAt     = 2;

        // Embedded certificate types
        const int IntSSLClientCertificate = 0;
        const int IntIdentity             = 1;

        const String StringSSLClientCertificate = "SSL Certificate";
        const String StringIdentity             = "Identity";


        // Permitted and Prohibited Processes table columns (0,1,2,3).
        // Permitted  Processes: Active, OS, Executable, Title
        // Prohibited Processes: Active, OS, Executable, Description
        // Process    Arguments: ArgumentActive, ArgumentParameter
        const int IntColumnProcessActive      = 0;
        const int IntColumnProcessOS          = 1;
        const int IntColumnProcessExecutable  = 2;
        const int IntColumnProcessTitle       = 3;
        const int IntColumnProcessDescription = 3;

        const int IntColumnProcessArgument = 1;
/*
        const String StringColumnProcessActive      = "Active";
        const String StringColumnProcessOS          = "OS";
        const String StringColumnProcessExecutable  = "Executable";
        const String StringColumnProcessTitle       = "Title";
        const String StringColumnProcessDescription = "Description";

        const String StringColumnProcessArgument = "Argument";
*/

        // URL Filter Rules table columns (0,1,2,3,4).
        // Show, Active, Regex, Expression, Action
        const int IntColumnURLFilterRuleShow       = 0;
        const int IntColumnURLFilterRuleActive     = 1;
        const int IntColumnURLFilterRuleRegex      = 2;
        const int IntColumnURLFilterRuleExpression = 3;
        const int IntColumnURLFilterRuleAction     = 4;
/*
        const String StringColumnURLFilterRuleShow       = "Show";
        const String StringColumnURLFilterRuleActive     = "Active";
        const String StringColumnURLFilterRuleRegex      = "Regex";
        const String StringColumnURLFilterRuleExpression = "Expression";
        const String StringColumnURLFilterRuleAction     = "Action";
*/

        // Certificates table columns (0,1).
        // Type, Name
        const int IntColumnCertificateType = 0;
        const int IntColumnCertificateName = 1;
/*
        const String StringColumnCertificateType = "Type";
        const String StringColumnCertificateName = "Name";
*/

        // Global variables

        // The current SEB configuration file
        String currentDireSebConfigFile;
        String currentFileSebConfigFile;
        String currentPathSebConfigFile;

        // The default SEB configuration file
        String defaultDireSebConfigFile;
        String defaultFileSebConfigFile;
        String defaultPathSebConfigFile;

        // Strings for encryption identities (KeyChain, Certificate Store)
        //static ArrayList chooseIdentityStringArrayList = new ArrayList();
        //static String[]  chooseIdentityStringArray = new String[1];
        static List<String> StringCryptoIdentity = new List<String>();

        // Entries of ListBoxes
      //static   Byte[] ByteArrayExamKeySalt      = new Byte[] {};
        static String[] StringCryptoIdentityArray;
        static String[] StringSebPurpose          = new String[2];
        static String[] StringSebMode             = new String[2];
        static String[] StringBrowserViewMode     = new String[2];
        static String[] StringWindowWidth         = new String[4];
        static String[] StringWindowHeight        = new String[4];
        static String[] StringWindowPositioning   = new String[3];
        static String[] StringPolicyLinkOpening   = new String[3];
        static String[] StringPolicyFileUpload    = new String[3];
        static String[] StringPolicyProxySettings = new String[2];
        static String[] StringPolicySebService    = new String[3];
        static String[] StringFunctionKey         = new String[12];
        static String[] StringActive              = new String[2];
        static String[] StringOS                  = new String[2];
        static String[] StringAction              = new String[5];
        static String[] StringCertificateType     = new String[2];
        static String[] StringProxyProtocol       = new String[7];

        const int NumProxyProtocols = 7;

        // Some settings are not stored in Plists but in Arrays
        static String [,] settingString  = new String [StateNum + 1, ValueNum + 1];
        static     int[,] settingInteger = new     int[StateNum + 1, ValueNum + 1];

        // Class SEBSettings contains all settings
        // and is used for importing/exporting the settings
        // from/to a human-readable .xml and an encrypted.seb file format.
        static Dictionary<string, object> sebSettingsOld = new Dictionary<string, object>();
        static Dictionary<string, object> sebSettingsNew = new Dictionary<string, object>();
        static Dictionary<string, object> sebSettingsTmp = new Dictionary<string, object>();
        static Dictionary<string, object> sebSettingsDef = new Dictionary<string, object>();

        static SEBProtectionController    sebController  = new SEBProtectionController();

        static int                        permittedProcessIndex;
        static List<object>               permittedProcessList = new List<object>();
        static Dictionary<string, object> permittedProcessData = new Dictionary<string, object>();
        static Dictionary<string, object> permittedProcessDataDef = new Dictionary<string, object>();

        static int                        permittedArgumentIndex;
        static List<object>               permittedArgumentList = new List<object>();
        static Dictionary<string, object> permittedArgumentData = new Dictionary<string, object>();
        static Dictionary<string, object> permittedArgumentDataDef = new Dictionary<string, object>();

        static int                        prohibitedProcessIndex;
        static List<object>               prohibitedProcessList = new List<object>();
        static Dictionary<string, object> prohibitedProcessData = new Dictionary<string, object>();
        static Dictionary<string, object> prohibitedProcessDataDef = new Dictionary<string, object>();

        static int                        urlFilterTableRow;
        static Boolean                    urlFilterIsTitleRow;

        static int                        urlFilterRuleIndex;
        static List<object>               urlFilterRuleList = new List<object>();
        static Dictionary<string, object> urlFilterRuleData = new Dictionary<string, object>();
        static Dictionary<string, object> urlFilterRuleDataDefault = new Dictionary<string, object>();
        static Dictionary<string, object> urlFilterRuleDataStored  = new Dictionary<string, object>();

        static int                        urlFilterActionIndex;
        static List<object>               urlFilterActionList = new List<object>();
        static Dictionary<string, object> urlFilterActionData = new Dictionary<string, object>();
        static Dictionary<string, object> urlFilterActionDataDefault = new Dictionary<string, object>();
        static Dictionary<string, object> urlFilterActionDataStored  = new Dictionary<string, object>();

        static int                        embeddedCertificateIndex;
        static List<object>               embeddedCertificateList = new List<object>();
        static Dictionary<string, object> embeddedCertificateData = new Dictionary<string, object>();
        static Dictionary<string, object> embeddedCertificateDataDef = new Dictionary<string, object>();

        // Lookup table: row  ->   ruleIndex
        // Lookup table: row  -> actionIndex
        // Lookup table: row  -> is this row a title row (or action row)?
        // Lookup table: rule -> startRow
        // Lookup table: rule ->   endRow
        // Lookup table: rule -> show this rule or not (expand/collapse)?
        static List<int>     urlFilterTableRuleIndex   = new List<int    >();
        static List<int>     urlFilterTableActionIndex = new List<int    >();
        static List<Boolean> urlFilterTableIsTitleRow  = new List<Boolean>();
        static List<int>     urlFilterTableStartRow    = new List<int    >();
        static List<int>     urlFilterTableEndRow      = new List<int    >();
        static List<Boolean> urlFilterTableShowRule    = new List<Boolean>();

        // Two-dimensional array: shall this cell be disabled (painted over)?
        static List<List<Boolean>> urlFilterTableCellIsDisabled = new List<List<Boolean>>();

        // Default disabled values for title row (rule) and action row (action)
        static Boolean[] urlFilterTableDisabledColumnsOfRule   = { false, false,  true, false,  true };
        static Boolean[] urlFilterTableDisabledColumnsOfAction = {  true, false, false, false, false };



        // ***********
        //
        // Constructor
        //
        // ***********

        public SebWindowsConfigForm()
        {
            InitializeComponent();

            // Initialise the global arrays
            for (int state = 1; state <= StateNum; state++)
            for (int value = 1; value <= ValueNum; value++)
            {
                settingInteger[state, value] = 0;
                settingString [state, value] = "";
            }

            // Default settings for group "General"
            sebSettingsDef.Add(MessageStartURL            , "http://www.safeexambrowser.org");
            sebSettingsDef.Add(MessageSebServerURL        , "");
            sebSettingsDef.Add(MessageAdminPassword       , "");
            sebSettingsDef.Add(MessageConfirmAdminPassword, "");
            sebSettingsDef.Add(MessageHashedAdminPassword , "");
            sebSettingsDef.Add(MessageAllowQuit           , true);
            sebSettingsDef.Add(MessageIgnoreQuitPassword  , false);
            sebSettingsDef.Add(MessageQuitPassword        , "");
            sebSettingsDef.Add(MessageConfirmQuitPassword , "");
            sebSettingsDef.Add(MessageHashedQuitPassword  , "");
            sebSettingsDef.Add(MessageExitKey1,  2);
            sebSettingsDef.Add(MessageExitKey2, 10);
            sebSettingsDef.Add(MessageExitKey3,  5);
            sebSettingsDef.Add(MessageSebMode, 0);

            // Default settings for group "Config File"
            sebSettingsDef.Add(MessageSebConfigPurpose       , 0);
            sebSettingsDef.Add(MessageAllowPreferencesWindow , true);
            sebSettingsDef.Add(MessageSettingsPassword       , "");
            sebSettingsDef.Add(MessageConfirmSettingsPassword, "");
            sebSettingsDef.Add(MessageHashedSettingsPassword , "");

            // CryptoIdentity is stored additionally
            settingInteger[StateDef, ValueCryptoIdentity] = 0;
            settingString [StateDef, ValueCryptoIdentity] = "";

            // Default settings for group "Appearance"
            sebSettingsDef.Add(MessageBrowserViewMode             , 0);
            sebSettingsDef.Add(MessageMainBrowserWindowWidth      , "100%");
            sebSettingsDef.Add(MessageMainBrowserWindowHeight     , "100%");
            sebSettingsDef.Add(MessageMainBrowserWindowPositioning, 1);
            sebSettingsDef.Add(MessageEnableBrowserWindowToolbar  , false);
            sebSettingsDef.Add(MessageHideBrowserWindowToolbar    , false);
            sebSettingsDef.Add(MessageShowMenuBar                 , false);
            sebSettingsDef.Add(MessageShowTaskBar                 , true);

            // MainBrowserWindow Width and Height is stored additionally
            settingInteger[StateDef, ValueMainBrowserWindowWidth ] = 1;
            settingInteger[StateDef, ValueMainBrowserWindowHeight] = 1;
            settingString [StateDef, ValueMainBrowserWindowWidth ] = "100%";
            settingString [StateDef, ValueMainBrowserWindowHeight] = "100%";

            // Default settings for group "Browser"
            sebSettingsDef.Add(MessageNewBrowserWindowByLinkPolicy        , 2);
            sebSettingsDef.Add(MessageNewBrowserWindowByScriptPolicy      , 2);
            sebSettingsDef.Add(MessageNewBrowserWindowByLinkBlockForeign  , false);
            sebSettingsDef.Add(MessageNewBrowserWindowByScriptBlockForeign, false);
            sebSettingsDef.Add(MessageNewBrowserWindowByLinkWidth         , "1000");
            sebSettingsDef.Add(MessageNewBrowserWindowByLinkHeight        , "100%");
            sebSettingsDef.Add(MessageNewBrowserWindowByLinkPositioning   , 2);

            sebSettingsDef.Add(MessageEnablePlugIns           , true);
            sebSettingsDef.Add(MessageEnableJava              , false);
            sebSettingsDef.Add(MessageEnableJavaScript        , true);
            sebSettingsDef.Add(MessageBlockPopUpWindows       , false);
            sebSettingsDef.Add(MessageAllowBrowsingBackForward, false);
            sebSettingsDef.Add(MessageEnableSebBrowser        , true);

            // NewBrowserWindow Width and Height is stored additionally
            settingInteger[StateDef, ValueNewBrowserWindowByLinkWidth ] = 3;
            settingInteger[StateDef, ValueNewBrowserWindowByLinkHeight] = 1;
            settingString [StateDef, ValueNewBrowserWindowByLinkWidth ] = "1000";
            settingString [StateDef, ValueNewBrowserWindowByLinkHeight] = "100%";

            // Default settings for group "DownUploads"
            sebSettingsDef.Add(MessageAllowDownUploads        , true);
            sebSettingsDef.Add(MessageDownloadDirectoryOSX    , "~/Downloads");
            sebSettingsDef.Add(MessageDownloadDirectoryWin    , "Desktop");
            sebSettingsDef.Add(MessageOpenDownloads           , false);
            sebSettingsDef.Add(MessageChooseFileToUploadPolicy, 0);
            sebSettingsDef.Add(MessageDownloadPDFFiles        , false);

            // Default settings for group "Exam"
            sebSettingsDef.Add(MessageExamKeySalt       , new Byte[] {});
            sebSettingsDef.Add(MessageBrowserExamKey    , "");
            sebSettingsDef.Add(MessageCopyBrowserExamKey, false);
            sebSettingsDef.Add(MessageSendBrowserExamKey, false);
            sebSettingsDef.Add(MessageQuitURL           , "");

            // Default settings for group "Applications"
            sebSettingsDef.Add(MessageMonitorProcesses         , false);
            sebSettingsDef.Add(MessagePermittedProcesses       , new List<object>());
            sebSettingsDef.Add(MessageAllowSwitchToApplications, false);
            sebSettingsDef.Add(MessageAllowFlashFullscreen     , false);
            sebSettingsDef.Add(MessageProhibitedProcesses      , new List<object>());

            permittedProcessDataDef.Add(MessageActive     , true);
            permittedProcessDataDef.Add(MessageAutostart  , true);
            permittedProcessDataDef.Add(MessageAutohide   , true);
            permittedProcessDataDef.Add(MessageAllowUser  , true);
            permittedProcessDataDef.Add(MessageOS         , IntWin);
            permittedProcessDataDef.Add(MessageTitle      , "");
            permittedProcessDataDef.Add(MessageDescription, "");
            permittedProcessDataDef.Add(MessageExecutable , "");
            permittedProcessDataDef.Add(MessagePath       , "");
            permittedProcessDataDef.Add(MessageIdentifier , "");
            permittedProcessDataDef.Add(MessageArguments  , new List<object>());

            permittedArgumentDataDef.Add(MessageActive  , true);
            permittedArgumentDataDef.Add(MessageArgument, "");

            prohibitedProcessDataDef.Add(MessageActive     , true);
            prohibitedProcessDataDef.Add(MessageCurrentUser, true);
            prohibitedProcessDataDef.Add(MessageStrongKill , false);
            prohibitedProcessDataDef.Add(MessageOS         , IntWin);
            prohibitedProcessDataDef.Add(MessageExecutable , "");
            prohibitedProcessDataDef.Add(MessageDescription, "");
            prohibitedProcessDataDef.Add(MessageIdentifier , "");
            prohibitedProcessDataDef.Add(MessageUser       , "");

            // Default settings for group "Network - Filter"
            sebSettingsDef.Add(MessageEnableURLFilter       , false);
            sebSettingsDef.Add(MessageEnableURLContentFilter, false);
            sebSettingsDef.Add(MessageURLFilterRules        , new List<object>());

            urlFilterRuleDataDefault.Add(MessageActive     , true);
            urlFilterRuleDataDefault.Add(MessageExpression , "Rule");
            urlFilterRuleDataDefault.Add(MessageRuleActions, new List<object>());

            urlFilterRuleDataStored.Add(MessageActive     , true);
            urlFilterRuleDataStored.Add(MessageExpression , "Rule");
            urlFilterRuleDataStored.Add(MessageRuleActions, new List<object>());

            urlFilterActionDataDefault.Add(MessageActive    , true);
            urlFilterActionDataDefault.Add(MessageRegex     , false);
            urlFilterActionDataDefault.Add(MessageExpression, "*");
            urlFilterActionDataDefault.Add(MessageAction    , 0);

            urlFilterActionDataStored.Add(MessageActive    , true);
            urlFilterActionDataStored.Add(MessageRegex     , false);
            urlFilterActionDataStored.Add(MessageExpression, "*");
            urlFilterActionDataStored.Add(MessageAction    , 0);

            // Default settings for group "Network - Certificates"
            sebSettingsDef.Add(MessageEmbeddedCertificates, new List<object>());

            embeddedCertificateDataDef.Add(MessageCertificateData, "");
            embeddedCertificateDataDef.Add(MessageType           , 0);
            embeddedCertificateDataDef.Add(MessageName           , "");

            // Default settings for group "Network - Proxies"
            sebSettingsDef.Add(MessageProxySettingsPolicy   , 0);
            sebSettingsDef.Add(MessageProxyProtocol         , 0);
            sebSettingsDef.Add(MessageProxyConfigurationFileURL, "");
            sebSettingsDef.Add(MessageExcludeSimpleHostnames, true);
            sebSettingsDef.Add(MessageUsePassiveFTPMode     , true);
            sebSettingsDef.Add(MessageBypassHostsAndDomains , new List<object>());

            // Default settings for group "Security"
            sebSettingsDef.Add(MessageSebServicePolicy   , 2);
            sebSettingsDef.Add(MessageAllowVirtualMachine, false);
            sebSettingsDef.Add(MessageCreateNewDesktop   , true);
            sebSettingsDef.Add(MessageAllowUserSwitching , true);
            sebSettingsDef.Add(MessageEnableLogging      , false);
            sebSettingsDef.Add(MessageLogDirectoryOSX    , "~/Documents");
            sebSettingsDef.Add(MessageLogDirectoryWin    , "My Documents");

            // Default settings for group "Inside SEB"
            sebSettingsDef.Add(MessageInsideSebEnableSwitchUser       , false);
            sebSettingsDef.Add(MessageInsideSebEnableLockThisComputer , false);
            sebSettingsDef.Add(MessageInsideSebEnableChangeAPassword  , false);
            sebSettingsDef.Add(MessageInsideSebEnableStartTaskManager , false);
            sebSettingsDef.Add(MessageInsideSebEnableLogOff           , false);
            sebSettingsDef.Add(MessageInsideSebEnableShutDown         , false);
            sebSettingsDef.Add(MessageInsideSebEnableEaseOfAccess     , false);
            sebSettingsDef.Add(MessageInsideSebEnableVmWareClientShade, false);

            // Default settings for group "Outside SEB"
            sebSettingsDef.Add(MessageOutsideSebEnableSwitchUser       , true);
            sebSettingsDef.Add(MessageOutsideSebEnableLockThisComputer , true);
            sebSettingsDef.Add(MessageOutsideSebEnableChangeAPassword  , true);
            sebSettingsDef.Add(MessageOutsideSebEnableStartTaskManager , true);
            sebSettingsDef.Add(MessageOutsideSebEnableLogOff           , true);
            sebSettingsDef.Add(MessageOutsideSebEnableShutDown         , true);
            sebSettingsDef.Add(MessageOutsideSebEnableEaseOfAccess     , true);
            sebSettingsDef.Add(MessageOutsideSebEnableVmWareClientShade, true);

            // Default settings for group "Hooked Keys"
            sebSettingsDef.Add(MessageHookKeys, true);

            // Default settings for group "Special Keys"
            sebSettingsDef.Add(MessageEnableEsc       , false);
            sebSettingsDef.Add(MessageEnableCtrlEsc   , false);
            sebSettingsDef.Add(MessageEnableAltEsc    , false);
            sebSettingsDef.Add(MessageEnableAltTab    , true);
            sebSettingsDef.Add(MessageEnableAltF4     , false);
            sebSettingsDef.Add(MessageEnableStartMenu , false);
            sebSettingsDef.Add(MessageEnableRightMouse, false);

            // Default settings for group "Function Keys"
            sebSettingsDef.Add(MessageEnableF1 , false);
            sebSettingsDef.Add(MessageEnableF2 , false);
            sebSettingsDef.Add(MessageEnableF3 , false);
            sebSettingsDef.Add(MessageEnableF4 , false);
            sebSettingsDef.Add(MessageEnableF5 , true);
            sebSettingsDef.Add(MessageEnableF6 , false);
            sebSettingsDef.Add(MessageEnableF7 , false);
            sebSettingsDef.Add(MessageEnableF8 , false);
            sebSettingsDef.Add(MessageEnableF9 , false);
            sebSettingsDef.Add(MessageEnableF10, false);
            sebSettingsDef.Add(MessageEnableF11, false);
            sebSettingsDef.Add(MessageEnableF12, false);
/*
            // Default settings for group "Online exam"
            String s0 = "Seb,../xulrunner/xulrunner.exe";
            String s1 = " -app \"..\\xul_seb\\seb.ini\"";
            String s2 = " -profile \"%LOCALAPPDATA%\\ETH_Zuerich\\xul_seb\\Profiles\"";
            String SebBrowserString = s0 + s1 + s2;

            settingString[StateDef, ValueSebBrowser           ] = SebBrowserString;
            settingString[StateDef, ValueAutostartProcess     ] = "Seb";
            settingString[StateDef, ValuePermittedApplications] = "Calculator,calc.exe;Notepad,notepad.exe;";
*/
            // Define the strings for the Encryption Identity
            StringCryptoIdentity.Add("none");
            StringCryptoIdentity.Add("alpha");
            StringCryptoIdentity.Add("beta");
            StringCryptoIdentity.Add("gamma");
            StringCryptoIdentity.Add("delta");
            StringCryptoIdentityArray = StringCryptoIdentity.ToArray();

            // Define the strings for the SEB purpose
            StringSebPurpose[0] = "starting an exam";
            StringSebPurpose[1] = "configuring a client";

            // Define the strings for the SEB mode
            StringSebMode[0] = "use local settings and load the start URL";
            StringSebMode[1] = "connect to the SEB server";

            // Define the strings for the Browser View Mode
            StringBrowserViewMode[0] = "use browser window";
            StringBrowserViewMode[1] = "use full screen mode";

            // Define the strings for the Window Width
            StringWindowWidth[0] = "50%";
            StringWindowWidth[1] = "100%";
            StringWindowWidth[2] = "800";
            StringWindowWidth[3] = "1000";

            // Define the strings for the Window Height
            StringWindowHeight[0] = "80%";
            StringWindowHeight[1] = "100%";
            StringWindowHeight[2] = "600";
            StringWindowHeight[3] = "800";

            // Define the strings for the Window Positioning
            StringWindowPositioning[0] = "Left";
            StringWindowPositioning[1] = "Center";
            StringWindowPositioning[2] = "Right";

            // Define the strings for the Link Opening Policy
            StringPolicyLinkOpening[0] = "get generally blocked";
            StringPolicyLinkOpening[1] = "open in same window";
            StringPolicyLinkOpening[2] = "open in new window";

            // Define the strings for the File Upload Policy
            StringPolicyFileUpload[0] = "manually with file requester";
            StringPolicyFileUpload[1] = "by attempting to upload the same file downloaded before";
            StringPolicyFileUpload[2] = "by only allowing to upload the same file downloaded before";

            // Define the strings for the Proxy Settings Policy
            StringPolicyProxySettings[0] = "Use system proxy settings";
            StringPolicyProxySettings[1] = "Use SEB proxy settings";

            // Define the strings for the SEB Service Policy
            StringPolicySebService[0] = "allow to run SEB without service";
            StringPolicySebService[1] = "display warning when service is not running";
            StringPolicySebService[2] = "allow to use SEB only with service";

            // Define the strings for the Function Keys F1, F2, ..., F12
            for (int i = 1; i <= 12; i++)
            {
                StringFunctionKey[i - 1] = "F" + i.ToString();
            }

            // Define the strings for the Permitted and Prohibited Processes
            StringActive[IntFalse] = "false";
            StringActive[IntTrue ] = "true";

            StringOS[IntOSX] = StringOSX;
            StringOS[IntWin] = StringWin;

            // Define the strings for the Embedded Certificates
            StringCertificateType[IntSSLClientCertificate] = StringSSLClientCertificate;
            StringCertificateType[IntIdentity            ] = StringIdentity;

            // Define the strings for the Proxy Protocols
            StringProxyProtocol[0] = "Auto Proxy Discovery";
            StringProxyProtocol[1] = "Automatic Proxy Configuration";
            StringProxyProtocol[2] = "Web Proxy (HTTP)";
            StringProxyProtocol[3] = "Secure Web Proxy (HTTPS)";
            StringProxyProtocol[4] = "FTP Proxy";
            StringProxyProtocol[5] = "SOCKS Proxy";
            StringProxyProtocol[6] = "Streaming Proxy (RTSP)";

            // Define the strings for the URL Filter Rule Actions
            StringAction[IntBlock] = StringBlock;
            StringAction[IntAllow] = StringAllow;
            StringAction[IntSkip ] = StringSkip;
            StringAction[IntAnd  ] = StringAnd;
            StringAction[IntOr   ] = StringOr;

            // Assign the fixed entries to the ListBoxes and ComboBoxes
            listBoxExitKey1.Items.AddRange(StringFunctionKey);
            listBoxExitKey2.Items.AddRange(StringFunctionKey);
            listBoxExitKey3.Items.AddRange(StringFunctionKey);

            comboBoxCryptoIdentity.Items.AddRange(StringCryptoIdentity.ToArray());

            comboBoxMainBrowserWindowWidth      .Items.AddRange(StringWindowWidth);
            comboBoxMainBrowserWindowHeight     .Items.AddRange(StringWindowHeight);
             listBoxMainBrowserWindowPositioning.Items.AddRange(StringWindowPositioning);

            comboBoxNewBrowserWindowWidth       .Items.AddRange(StringWindowWidth);
            comboBoxNewBrowserWindowHeight      .Items.AddRange(StringWindowHeight);
             listBoxNewBrowserWindowPositioning .Items.AddRange(StringWindowPositioning);

             listBoxOpenLinksHTML.Items.AddRange(StringPolicyLinkOpening);
             listBoxOpenLinksJava.Items.AddRange(StringPolicyLinkOpening);

             listBoxChooseFileToUploadPolicy.Items.AddRange(StringPolicyFileUpload);
             listBoxSebServicePolicy        .Items.AddRange(StringPolicySebService);

            // Assign the fixed entries to the CheckedListBoxes
            checkedListBoxProxyProtocol.Items.AddRange(StringProxyProtocol);

            // Initialise the DataGridViews:
            // Set "AllowUserToAddRows" to false, to avoid an initial empty first row
            // Set "RowHeadersVisible"  to false, to avoid an initial empty first column
            // Set "FullRowSelect"      to true , to select whole row when clicking on a cell
            dataGridViewPermittedProcesses.Enabled            = false;
            dataGridViewPermittedProcesses.ReadOnly           = false;
            dataGridViewPermittedProcesses.AllowUserToAddRows = false;
            dataGridViewPermittedProcesses.RowHeadersVisible  = false;
            dataGridViewPermittedProcesses.MultiSelect        = false;
            dataGridViewPermittedProcesses.SelectionMode      = DataGridViewSelectionMode.FullRowSelect;

            dataGridViewPermittedProcessArguments.Enabled            = false;
            dataGridViewPermittedProcessArguments.ReadOnly           = false;
            dataGridViewPermittedProcessArguments.AllowUserToAddRows = false;
            dataGridViewPermittedProcessArguments.RowHeadersVisible  = false;
            dataGridViewPermittedProcessArguments.MultiSelect        = false;
            dataGridViewPermittedProcessArguments.SelectionMode      = DataGridViewSelectionMode.FullRowSelect;

            dataGridViewProhibitedProcesses.Enabled            = false;
            dataGridViewProhibitedProcesses.ReadOnly           = false;
            dataGridViewProhibitedProcesses.AllowUserToAddRows = false;
            dataGridViewProhibitedProcesses.RowHeadersVisible  = false;
            dataGridViewProhibitedProcesses.MultiSelect        = false;
            dataGridViewProhibitedProcesses.SelectionMode      = DataGridViewSelectionMode.FullRowSelect;

            dataGridViewURLFilterRules.Enabled            = false;
            dataGridViewURLFilterRules.ReadOnly           = false;
            dataGridViewURLFilterRules.AllowUserToAddRows = false;
            dataGridViewURLFilterRules.RowHeadersVisible  = false;
            dataGridViewURLFilterRules.MultiSelect        = false;
            dataGridViewURLFilterRules.SelectionMode      = DataGridViewSelectionMode.FullRowSelect;

            dataGridViewEmbeddedCertificates.Enabled            = false;
            dataGridViewEmbeddedCertificates.ReadOnly           = false;
            dataGridViewEmbeddedCertificates.AllowUserToAddRows = false;
            dataGridViewEmbeddedCertificates.RowHeadersVisible  = false;
            dataGridViewEmbeddedCertificates.MultiSelect        = false;
            dataGridViewEmbeddedCertificates.SelectionMode      = DataGridViewSelectionMode.FullRowSelect;

            dataGridViewPermittedProcesses.Columns[IntColumnProcessActive    ].ValueType = typeof(Boolean);
            dataGridViewPermittedProcesses.Columns[IntColumnProcessOS        ].ValueType = typeof(String);
            dataGridViewPermittedProcesses.Columns[IntColumnProcessExecutable].ValueType = typeof(String);
            dataGridViewPermittedProcesses.Columns[IntColumnProcessTitle     ].ValueType = typeof(String);

            dataGridViewPermittedProcessArguments.Columns[IntColumnProcessActive  ].ValueType = typeof(Boolean);
            dataGridViewPermittedProcessArguments.Columns[IntColumnProcessArgument].ValueType = typeof(String);

            dataGridViewProhibitedProcesses.Columns[IntColumnProcessActive     ].ValueType = typeof(Boolean);
            dataGridViewProhibitedProcesses.Columns[IntColumnProcessOS         ].ValueType = typeof(String);
            dataGridViewProhibitedProcesses.Columns[IntColumnProcessExecutable ].ValueType = typeof(String);
            dataGridViewProhibitedProcesses.Columns[IntColumnProcessDescription].ValueType = typeof(String);

            dataGridViewURLFilterRules.Columns[IntColumnURLFilterRuleShow      ].ValueType = typeof(Boolean);
            dataGridViewURLFilterRules.Columns[IntColumnURLFilterRuleActive    ].ValueType = typeof(Boolean);
            dataGridViewURLFilterRules.Columns[IntColumnURLFilterRuleRegex     ].ValueType = typeof(Boolean);
            dataGridViewURLFilterRules.Columns[IntColumnURLFilterRuleExpression].ValueType = typeof(String);
            dataGridViewURLFilterRules.Columns[IntColumnURLFilterRuleAction    ].ValueType = typeof(String);

            dataGridViewEmbeddedCertificates.Columns[IntColumnCertificateType].ValueType = typeof(String);
            dataGridViewEmbeddedCertificates.Columns[IntColumnCertificateName].ValueType = typeof(String);

            // Assign the column names to the DataGridViews
/*
            dataGridViewPermittedProcesses.Columns.Add(StringColumnActive    , StringColumnActive);
            dataGridViewPermittedProcesses.Columns.Add(StringColumnOS        , StringColumnOS);
            dataGridViewPermittedProcesses.Columns.Add(StringColumnExecutable, StringColumnExecutable);
            dataGridViewPermittedProcesses.Columns.Add(StringColumnTitle     , StringColumnTitle);

            dataGridViewPermittedProcessArguments.Columns.Add(StringColumnActive  , StringColumnActive);
            dataGridViewPermittedProcessArguments.Columns.Add(StringColumnArgument, StringColumnArgument);

            dataGridViewProhibitedProcesses.Columns.Add(StringColumnActive     , StringColumnActive);
            dataGridViewProhibitedProcesses.Columns.Add(StringColumnOS         , StringColumnOS);
            dataGridViewProhibitedProcesses.Columns.Add(StringColumnExecutable , StringColumnExecutable);
            dataGridViewProhibitedProcesses.Columns.Add(StringColumnDescription, StringColumnDescription);

            dataGridViewURLFilterRules.Columns.Add(StringColumnURLFilterRuleShow      , StringColumnURLFilterRuleShow);
            dataGridViewURLFilterRules.Columns.Add(StringColumnURLFilterRuleActive    , StringColumnURLFilterRuleActive);
            dataGridViewURLFilterRules.Columns.Add(StringColumnURLFilterRuleRegex     , StringColumnURLFilterRuleRegex);
            dataGridViewURLFilterRules.Columns.Add(StringColumnURLFilterRuleExpression, StringColumnURLFilterRuleExpression);
            dataGridViewURLFilterRules.Columns.Add(StringColumnURLFilterRuleAction    , StringColumnURLFilterRuleAction);

            dataGridViewCertificates.Columns.Add(StringColumnCertificateType, StringColumnCertificateType);
            dataGridViewCertificates.Columns.Add(StringColumnCertificateName, StringColumnCertificateName);
*/
            groupBoxPermittedProcess .Enabled = false;
            groupBoxProhibitedProcess.Enabled = false;

            listBoxPermittedProcessOS .Items.AddRange(StringOS);
            listBoxProhibitedProcessOS.Items.AddRange(StringOS);

            // Initialise the global variables for the lists and dictionaries
            permittedProcessIndex = -1;
            permittedProcessList.Clear();
            permittedProcessData.Clear();

            permittedArgumentIndex = -1;
            permittedArgumentList.Clear();
            permittedArgumentData.Clear();

            prohibitedProcessIndex = -1;
            prohibitedProcessList.Clear();
            prohibitedProcessData.Clear();

            embeddedCertificateIndex = -1;
            embeddedCertificateList.Clear();
            embeddedCertificateData.Clear();

            urlFilterTableRow   = -1;
            urlFilterIsTitleRow = false;

            urlFilterRuleIndex = -1;
            urlFilterRuleList.Clear();
            urlFilterRuleData.Clear();

            urlFilterActionIndex = -1;
            urlFilterActionList.Clear();
            urlFilterActionData.Clear();

            urlFilterTableRuleIndex     .Clear();
            urlFilterTableActionIndex   .Clear();
            urlFilterTableIsTitleRow    .Clear();
            urlFilterTableStartRow      .Clear();
            urlFilterTableEndRow        .Clear();
            urlFilterTableShowRule      .Clear();
            urlFilterTableCellIsDisabled.Clear();

            // Auto-resize the columns and cells
          //dataGridViewPermittedProcesses  .AutoResizeColumns();
          //dataGridViewProhibitedProcesses .AutoResizeColumns();
          //dataGridViewURLFilterRules      .AutoResizeColumns();
          //dataGridViewEmbeddedCertificates.AutoResizeColumns();

          //dataGridViewPermittedProcesses  .AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
          //dataGridViewProhibitedProcesses .AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
          //dataGridViewURLFilterRules      .AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
          //dataGridViewEmbeddedCertificates.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);

            // IMPORTANT:
            // Create a second dictionary "new settings"
            // and copy all default settings to the new settings.
            // This must be done BEFORE any config file is loaded
            // and assures that every (key, value) pair is contained
            // in the "old", "new" and "def" dictionaries,
            // even if the loaded "tmp" dictionary does NOT contain every pair.

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

            try 
            {
                // Read the configuration settings from .xml or .seb file
                // If encrypted, decrypt the configuration settings
                // Convert the XML structure into a C# object
                if (isEncrypted == true)
                {
                    TextReader textReader;
                    String encryptedSettings = "";
                    String decryptedSettings = "";
                  //String password          = "Seb";
                  //X509Certificate2 certificate = null;

                    textReader        = new StreamReader(fileName);
                    encryptedSettings = textReader.ReadToEnd();
                    textReader.Close();

                  //decryptedSettings = sebController.DecryptSebClientSettings(encryptedSettings);
                  //decryptedSettings = decryptedSettings.Trim();
                    decryptedSettings = encryptedSettings;

                    sebSettingsTmp = (Dictionary<string, object>)Plist.readPlistSource(decryptedSettings);
                }
                else // unencrypted .xml file
                {
                    sebSettingsTmp = (Dictionary<string, object>)Plist.readPlist(fileName);
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
            // copy them to "new" and "old" settings and update the widgets

            // Choose Identity needs a conversion from string to integer.
            // The SEB Windows configuration editor never reads the identity
            // from the config file but instead searches it in the
            // Certificate Store of the computer where it is running,
            // so initially the 0th list entry is displayed ("none").
            //
            //tmpCryptoIdentityInteger = 0;
            //tmpCryptoIdentityString  = 0;

            // Copy tmp settings to old settings
            // Copy tmp settings to new settings
            CopySettingsArrays(StateTmp, StateOld);
            CopySettingsArrays(StateTmp, StateNew);
            CopySettingsDictionary(sebSettingsTmp, sebSettingsOld);
            CopySettingsDictionary(sebSettingsTmp, sebSettingsNew);

            currentDireSebConfigFile = Path.GetDirectoryName(fileName);
            currentFileSebConfigFile = Path.GetFileName     (fileName);
            currentPathSebConfigFile = Path.GetFullPath     (fileName);

            // After loading a new config file, reset the URL Filter Table indices
            // to avoid errors, in case there was a non-empty URL Filter Table displayed
            // in the DataGridViewURLFilterRules prior to loading the new config file.
            urlFilterTableRow    = -1;
            urlFilterIsTitleRow  = false;
            urlFilterRuleIndex   = -1;
            urlFilterActionIndex = -1;

            // Get the URL Filter Rules
            urlFilterRuleList = (List<object>)sebSettingsNew[MessageURLFilterRules];

            // If there are any filter rules, select first filter rule.
            // If there are no  filter rules, select no    filter rule.
            if (urlFilterRuleList.Count > 0) urlFilterRuleIndex =  0;
                                        else urlFilterRuleIndex = -1;

            // At first, show all filter rules with their actions (expanded view).
            for (int ruleIndex = 0; ruleIndex < urlFilterRuleList.Count; ruleIndex++)
            {
                urlFilterTableShowRule.Add(true);
            }

            UpdateAllWidgetsOfProgram();
            //Plist.writeXml(sebSettingsNew, "DebugSettingsNew_in_OpenConfigurationFile.xml");
            //Plist.writeXml(sebSettingsOld, "DebugSettingsOld_in_OpenConfigurationFile.xml");
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
                    Plist.writeXml(sebSettingsOld, "DebugSettingsOld_in_SaveConfigurationFile.xml");
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
            // Copy new settings to old settings
            CopySettingsArrays    (      StateNew,       StateOld);
            CopySettingsDictionary(sebSettingsNew, sebSettingsOld);

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

//                if (key.GetType == Type.Dictionary)
//                    CopySettingsDictionary(sebSettingsSource, sebSettingsTarget, keyNode);

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
            // Clear all help structures for table access
            urlFilterTableRuleIndex     .Clear();
            urlFilterTableActionIndex   .Clear();
            urlFilterTableIsTitleRow    .Clear();
            urlFilterTableStartRow      .Clear();
            urlFilterTableEndRow        .Clear();
          //urlFilterTableShowRule      .Clear();
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
                     dataGridViewURLFilterRules.Rows.Add("Collapse", active, false, expression, "");
                else dataGridViewURLFilterRules.Rows.Add("Expand"  , active, false, expression, "");

                dataGridViewURLFilterRules.Rows[row].DefaultCellStyle.BackColor = Color.LightGray;
                dataGridViewURLFilterRules.Rows[row].Cells[IntColumnURLFilterRuleExpression].Style.Font = new Font(DefaultFont, FontStyle.Bold);
                dataGridViewURLFilterRules.Rows[row].Cells[IntColumnURLFilterRuleRegex     ].ReadOnly = true;
                dataGridViewURLFilterRules.Rows[row].Cells[IntColumnURLFilterRuleAction    ].ReadOnly = true;

                row++;

                // If user chose COLLAPSED view for this rule:
                // Do not show the actions, but continue with next rule.
                if (!urlFilterTableShowRule[ruleIndex]) continue;

                // If user chose EXPANDED view for this rule:
                // Add URL Filter Actions of current URL Filter Rule to DataGridView
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
            // Group "Network      -    Filter/Certificates"

            // Update the lists for the DataGridViews
               permittedProcessList = (List<object>)sebSettingsNew[MessagePermittedProcesses];
              prohibitedProcessList = (List<object>)sebSettingsNew[MessageProhibitedProcesses];
            embeddedCertificateList = (List<object>)sebSettingsNew[MessageEmbeddedCertificates];

             // Check if currently loaded lists have any entries
            if (permittedProcessList.Count > 0) permittedProcessIndex =  0;
                                           else permittedProcessIndex = -1;

            if (prohibitedProcessList.Count > 0) prohibitedProcessIndex =  0;
                                            else prohibitedProcessIndex = -1;

            if (embeddedCertificateList.Count > 0) embeddedCertificateIndex =  0;
                                              else embeddedCertificateIndex = -1;

            // Remove all previously displayed list entries from DataGridViews
                groupBoxPermittedProcess  .Enabled = (permittedProcessList.Count > 0);
            dataGridViewPermittedProcesses.Enabled = (permittedProcessList.Count > 0);
            dataGridViewPermittedProcesses.Rows.Clear();

                groupBoxProhibitedProcess  .Enabled = (prohibitedProcessList.Count > 0);
            dataGridViewProhibitedProcesses.Enabled = (prohibitedProcessList.Count > 0);
            dataGridViewProhibitedProcesses.Rows.Clear();

            dataGridViewEmbeddedCertificates.Enabled = (embeddedCertificateList.Count > 0);
            dataGridViewEmbeddedCertificates.Rows.Clear();

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

          //dataGridViewPermittedProcesses  .AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
          //dataGridViewProhibitedProcesses .AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
          //dataGridViewURLFilterRules      .AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
          //dataGridViewEmbeddedCertificates.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);


            // Group "Network - Filter"
            checkBoxEnableURLFilter       .Checked = (Boolean)sebSettingsNew[MessageEnableURLFilter];
            checkBoxEnableURLContentFilter.Checked = (Boolean)sebSettingsNew[MessageEnableURLContentFilter];

            // Group "Network - Certificates"

            // Group "Network - Proxies"
            radioButtonUseSystemProxySettings.Checked =    ((int)sebSettingsNew[MessageProxySettingsPolicy] == 0);
            radioButtonUseSebProxySettings   .Checked =    ((int)sebSettingsNew[MessageProxySettingsPolicy] == 1);
            checkedListBoxProxyProtocol.SelectedIndex =     (int)sebSettingsNew[MessageProxyProtocol];

            int selectedIndex = (int)sebSettingsNew[MessageProxyProtocol];
            checkedListBoxProxyProtocol.SetItemChecked( selectedIndex, true);
            checkedListBoxProxyProtocol.SelectedIndex = selectedIndex;

            textBoxProxyConfigurationFileURL .Text    =  (String)sebSettingsNew[MessageProxyConfigurationFileURL];
            checkBoxExcludeSimpleHostnames   .Checked = (Boolean)sebSettingsNew[MessageExcludeSimpleHostnames];
            checkBoxUsePassiveFTPMode        .Checked = (Boolean)sebSettingsNew[MessageUsePassiveFTPMode];
          //textBoxBypassHostsAndDomains     .Text    =  (String)sebSettingsNew[MessageBypassHostsAndDomains];

            // Group "Security"
             listBoxSebServicePolicy.SelectedIndex =     (int)sebSettingsNew[MessageSebServicePolicy];
            checkBoxAllowVirtualMachine.Checked    = (Boolean)sebSettingsNew[MessageAllowVirtualMachine];
            checkBoxCreateNewDesktop   .Checked    = (Boolean)sebSettingsNew[MessageCreateNewDesktop];
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
            CopySettingsArrays    (      StateDef,       StateNew);
            CopySettingsDictionary(sebSettingsDef, sebSettingsNew);
            UpdateAllWidgetsOfProgram();
            //Plist.writeXml(sebSettingsNew, "DebugSettingsNew_after_RevertToDefault.xml");
            //Plist.writeXml(sebSettingsDef, "DebugSettingsDef_after_RevertToDefault.xml");
        }

        private void buttonRevertToLastOpened_Click(object sender, EventArgs e)
        {
            //Plist.writeXml(sebSettingsNew, "DebugSettingsNew_before_RevertToLastOpened.xml");
            //Plist.writeXml(sebSettingsOld, "DebugSettingsOld_before_RevertToLastOpened.xml");
            OpenConfigurationFile(currentPathSebConfigFile);
            //Plist.writeXml(sebSettingsNew, "DebugSettingsNew_after_RevertToLastOpened.xml");
            //Plist.writeXml(sebSettingsOld, "DebugSettingsOld_after_RevertToLastOpened.xml");
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
          //sebSettingsNew[MessageMainBrowserWindowWidth] = comboBoxMainBrowserWindowWidth.SelectedIndex;
            sebSettingsNew[MessageMainBrowserWindowWidth] = comboBoxMainBrowserWindowWidth.Text;
        }

        private void comboBoxMainBrowserWindowWidth_TextUpdate(object sender, EventArgs e)
        {
            settingInteger[StateNew, ValueMainBrowserWindowWidth] = comboBoxMainBrowserWindowWidth.SelectedIndex;
            settingString [StateNew, ValueMainBrowserWindowWidth] = comboBoxMainBrowserWindowWidth.Text;
          //sebSettingsNew[MessageMainBrowserWindowWidth] = comboBoxMainBrowserWindowWidth.SelectedIndex;
            sebSettingsNew[MessageMainBrowserWindowWidth] = comboBoxMainBrowserWindowWidth.Text;
        }

        private void comboBoxMainBrowserWindowHeight_SelectedIndexChanged(object sender, EventArgs e)
        {
            settingInteger[StateNew, ValueMainBrowserWindowHeight] = comboBoxMainBrowserWindowHeight.SelectedIndex;
            settingString [StateNew, ValueMainBrowserWindowHeight] = comboBoxMainBrowserWindowHeight.Text;
          //sebSettingsNew[MessageMainBrowserWindowHeight] = comboBoxMainBrowserWindowHeight.SelectedIndex;
            sebSettingsNew[MessageMainBrowserWindowHeight] = comboBoxMainBrowserWindowHeight.Text;
        }

        private void comboBoxMainBrowserWindowHeight_TextUpdate(object sender, EventArgs e)
        {
            settingInteger[StateNew, ValueMainBrowserWindowHeight] = comboBoxMainBrowserWindowHeight.SelectedIndex;
            settingString [StateNew, ValueMainBrowserWindowHeight] = comboBoxMainBrowserWindowHeight.Text;
          //sebSettingsNew[MessageMainBrowserWindowHeight] = comboBoxMainBrowserWindowHeight.SelectedIndex;
            sebSettingsNew[MessageMainBrowserWindowHeight] = comboBoxMainBrowserWindowHeight.Text;
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
                urlFilterRuleIndex   = -1;
                urlFilterActionIndex = -1;
            }

            // If the user clicked onto a TITLE row (RULE),
            // add a new rule BEFORE or AFTER the current rule.
            if (urlFilterIsTitleRow)
            {
                if (((location == IntLocationBefore) && (urlFilterRuleList.Count == 0))
                ||   (location == IntLocationAfter))
                    urlFilterRuleIndex++;

                // Create new rule dataset containing default or stored values
                Dictionary<string, object> ruleData = new Dictionary<string, object>();

                //if (operation == IntOperationInsert) ruleData = urlFilterRuleDataDefault;
                //if (operation == IntOperationPaste ) ruleData = urlFilterRuleDataStored;

                if (operation == IntOperationInsert)
                {
                  //ruleData[MessageActive     ] = url;
                  //ruleData[MessageExpression ] = "Rule";
                  //ruleData[MessageRuleActions] = new List<object>();

                    ruleData[MessageActive     ] = urlFilterRuleDataDefault[MessageActive    ];
                    ruleData[MessageExpression ] = urlFilterRuleDataDefault[MessageExpression];
                    ruleData[MessageRuleActions] = new List<object>();
                }
                if (operation == IntOperationPaste)
                {
                    ruleData[MessageActive     ] = urlFilterRuleDataStored[MessageActive     ];
                    ruleData[MessageExpression ] = urlFilterRuleDataStored[MessageExpression ];
                    ruleData[MessageRuleActions] = urlFilterRuleDataStored[MessageRuleActions];
                }

                // INSERT or PASTE new rule into rule list at correct position index
                urlFilterRuleList     .Insert(urlFilterRuleIndex, ruleData);
                urlFilterTableShowRule.Insert(urlFilterRuleIndex, true);
            }

            // If the user clicked onto an ACTION row,
            // add a new action BEFORE or AFTER the current action.
            // If the user clicked onto a TITLE row (rule),
            // add a new action AFTER the new rule.

            if (true)
            {
                if (((location == IntLocationBefore) && (urlFilterIsTitleRow))
                ||   (location == IntLocationAfter))
                    urlFilterActionIndex++;

                // Create new action dataset containing default or stored values
                Dictionary<string, object> actionData = new Dictionary<string, object>();

                //if (operation == IntOperationInsert) actionData = urlFilterActionDataDefault;
                //if (operation == IntOperationPaste ) actionData = urlFilterActionDataStored;

                if (operation == IntOperationInsert)
                {
                  //actionData[MessageActive    ] = true;
                  //actionData[MessageRegex     ] = false;
                  //actionData[MessageExpression] = "*";
                  //actionData[MessageAction    ] = 0;

                    actionData[MessageActive    ] = urlFilterActionDataDefault[MessageActive    ];
                    actionData[MessageRegex     ] = urlFilterActionDataDefault[MessageRegex     ];
                    actionData[MessageExpression] = urlFilterActionDataDefault[MessageExpression];
                    actionData[MessageAction    ] = urlFilterActionDataDefault[MessageAction    ];
                }

                if (operation == IntOperationPaste)
                {
                    actionData[MessageActive    ] = urlFilterActionDataStored[MessageActive    ];
                    actionData[MessageRegex     ] = urlFilterActionDataStored[MessageRegex     ];
                    actionData[MessageExpression] = urlFilterActionDataStored[MessageExpression];
                    actionData[MessageAction    ] = urlFilterActionDataStored[MessageAction    ];
                }

                // INSERT or PASTE new action into action list at correct position index
                urlFilterRuleData   = (Dictionary<string, object>)urlFilterRuleList[urlFilterRuleIndex];
                urlFilterActionList =               (List<object>)urlFilterRuleData[MessageRuleActions];
                urlFilterActionList.Insert(urlFilterActionIndex, actionData);
            }

            // Update the table of URL Filter Rules
            UpdateTableOfURLFilterRules();
        }


        private void CopyCutDeleteRuleAction(int operation)
        {
            // Get the rule list
            urlFilterRuleList = (List<object>)sebSettingsNew[MessageURLFilterRules];

            // If rule list is empty, abort since nothing can be deleted anymore
            if (urlFilterRuleList.Count == 0) return;

            // Determine if the selected row is a title row or action row.
            // Determine which rule and action belong to the selected row.
            urlFilterTableRow    = dataGridViewURLFilterRules.SelectedRows[0].Index;
            urlFilterIsTitleRow  = urlFilterTableIsTitleRow [urlFilterTableRow];
            urlFilterRuleIndex   = urlFilterTableRuleIndex  [urlFilterTableRow];
            urlFilterActionIndex = urlFilterTableActionIndex[urlFilterTableRow];

            // If the user clicked onto a TITLE row (RULE), delete this rule.
            if (urlFilterIsTitleRow)
            {
                if ((operation == IntOperationCopy) || (operation == IntOperationCut))
                {
                    // Store currently selected rule for later Paste operation
                  //urlFilterRuleList.CopyTo(urlFilterRuleIndex, urlFilterRuleDataStored);
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
            // If the user clicked onto an ACTION row, delete this action.
            else
            {
                // Get the action list
                urlFilterRuleData   = (Dictionary<string, object>)urlFilterRuleList[urlFilterRuleIndex];
                urlFilterActionList =               (List<object>)urlFilterRuleData[MessageRuleActions];

                if ((operation == IntOperationCopy) || (operation == IntOperationCut))
                {
                    // Store currently selected action for later Paste operation
                    urlFilterActionDataStored = (Dictionary<string, object>)urlFilterActionList[urlFilterActionIndex];
/*
                    urlFilterActionDataStored.Clear();

                    // Create new action dataset containing stored values
                    Dictionary<string, object> actionData =
                   (Dictionary<string, object>)urlFilterActionList[urlFilterActionIndex];

                    urlFilterActionDataStored.Add(MessageActive    , actionData[MessageActive    ]);
                    urlFilterActionDataStored.Add(MessageRegex     , actionData[MessageRegex     ]);
                    urlFilterActionDataStored.Add(MessageExpression, actionData[MessageExpression]);
                    urlFilterActionDataStored.Add(MessageAction    , actionData[MessageActive    ]);
*/
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

        private void checkedListBoxProxyProtocol_SelectedIndexChanged(object sender, EventArgs e)
        {
            // If only the selected item shall be checked,
            // we must first uncheck all items.
            // Otherwise, the previously selected
            // but afterwards unselected items keep being checked!)
            for (int index = 0; index < NumProxyProtocols; index++)
                checkedListBoxProxyProtocol.SetItemChecked(index, false);

            int selectedIndex = checkedListBoxProxyProtocol.SelectedIndex;
            checkedListBoxProxyProtocol.SetItemChecked(selectedIndex, true);
            sebSettingsNew[MessageProxyProtocol] = checkedListBoxProxyProtocol.SelectedIndex;
        }

        private void textBoxProxyConfigurationFileURL_TextChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageProxyConfigurationFileURL] = textBoxProxyConfigurationFileURL.Text;
        }

        private void buttonChooseProxyConfigurationFile_Click(object sender, EventArgs e)
        {

        }

        private void checkBoxExcludeSimpleHostnames_CheckedChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageExcludeSimpleHostnames] = checkBoxExcludeSimpleHostnames.Checked;
        }

        private void checkBoxUsePassiveFTPMode_CheckedChanged(object sender, EventArgs e)
        {
            sebSettingsNew[MessageUsePassiveFTPMode] = checkBoxUsePassiveFTPMode.Checked;
        }

        private void textBoxBypassHostsAndDomains_TextChanged(object sender, EventArgs e)
        {
          //sebSettingsNew[MessageBypassHostsAndDomains] = textBoxBypassHostsAndDomains.Text;
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
