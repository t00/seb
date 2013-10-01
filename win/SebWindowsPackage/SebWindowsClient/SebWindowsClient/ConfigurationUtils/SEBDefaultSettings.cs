using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SebWindowsClient.ConfigurationUtils
{
    public class SEBDefaultSettings
    {
        // Operating systems
        const int IntOSX = 0;
        const int IntWin = 1;

       public static Dictionary<string, object> sebSettingsDef = new Dictionary<string, object>();
       public static Dictionary<string, object> permittedProcessDataDef = new Dictionary<string, object>();
       public static Dictionary<string, object> permittedArgumentDataDef = new Dictionary<string, object>();
       public static Dictionary<string, object> prohibitedProcessDataDef = new Dictionary<string, object>();
       public static Dictionary<string, object> urlFilterRuleDataDefault = new Dictionary<string, object>();
       public static Dictionary<string, object> urlFilterActionDataDefault = new Dictionary<string, object>();
       public static List<object> urlFilterActionListDefault = new List<object>();
       public static List<object> urlFilterActionListStored = new List<object>();
       public static Dictionary<string, object> urlFilterActionDataStored = new Dictionary<string, object>();
       public static Dictionary<string, object> urlFilterRuleDataStored = new Dictionary<string, object>();
       public static Dictionary<string, object> embeddedCertificateDataDef = new Dictionary<string, object>();

        public static void InitialiseSEBDefaultSettings()
        {
                // Default settings for group "General"
            sebSettingsDef.Add(SEBGlobalConstants.MessageStartURL            , "http://www.safeexambrowser.org");
            sebSettingsDef.Add(SEBGlobalConstants.MessageSebServerURL        , "");
            sebSettingsDef.Add(SEBGlobalConstants.MessageAdminPassword       , "");
            sebSettingsDef.Add(SEBGlobalConstants.MessageConfirmAdminPassword, "");
            sebSettingsDef.Add(SEBGlobalConstants.MessageHashedAdminPassword , "");
            sebSettingsDef.Add(SEBGlobalConstants.MessageAllowQuit           , true);
            sebSettingsDef.Add(SEBGlobalConstants.MessageIgnoreQuitPassword  , false);
            sebSettingsDef.Add(SEBGlobalConstants.MessageQuitPassword        , "");
            sebSettingsDef.Add(SEBGlobalConstants.MessageConfirmQuitPassword , "");
            sebSettingsDef.Add(SEBGlobalConstants.MessageHashedQuitPassword  , "");
            sebSettingsDef.Add(SEBGlobalConstants.MessageExitKey1,  2);
            sebSettingsDef.Add(SEBGlobalConstants.MessageExitKey2, 10);
            sebSettingsDef.Add(SEBGlobalConstants.MessageExitKey3,  5);
            sebSettingsDef.Add(SEBGlobalConstants.MessageSebMode, 0);

            // Default settings for group "Config File"
            sebSettingsDef.Add(SEBGlobalConstants.MessageSebConfigPurpose       , 0);
            sebSettingsDef.Add(SEBGlobalConstants.MessageAllowPreferencesWindow , true);
            sebSettingsDef.Add(SEBGlobalConstants.MessageSettingsPassword       , "");
            sebSettingsDef.Add(SEBGlobalConstants.MessageConfirmSettingsPassword, "");
            sebSettingsDef.Add(SEBGlobalConstants.MessageHashedSettingsPassword , "");

            // Default settings for group "Appearance"
            sebSettingsDef.Add(SEBGlobalConstants.MessageBrowserViewMode             , 0);
            sebSettingsDef.Add(SEBGlobalConstants.MessageMainBrowserWindowWidth      , "100%");
            sebSettingsDef.Add(SEBGlobalConstants.MessageMainBrowserWindowHeight     , "100%");
            sebSettingsDef.Add(SEBGlobalConstants.MessageMainBrowserWindowPositioning, 1);
            sebSettingsDef.Add(SEBGlobalConstants.MessageEnableBrowserWindowToolbar  , false);
            sebSettingsDef.Add(SEBGlobalConstants.MessageHideBrowserWindowToolbar    , false);
            sebSettingsDef.Add(SEBGlobalConstants.MessageShowMenuBar                 , false);
            sebSettingsDef.Add(SEBGlobalConstants.MessageShowTaskBar                 , true);

            // Default settings for group "Browser"
            sebSettingsDef.Add(SEBGlobalConstants.MessageNewBrowserWindowByLinkPolicy        , 2);
            sebSettingsDef.Add(SEBGlobalConstants.MessageNewBrowserWindowByScriptPolicy      , 2);
            sebSettingsDef.Add(SEBGlobalConstants.MessageNewBrowserWindowByLinkBlockForeign  , false);
            sebSettingsDef.Add(SEBGlobalConstants.MessageNewBrowserWindowByScriptBlockForeign, false);
            sebSettingsDef.Add(SEBGlobalConstants.MessageNewBrowserWindowByLinkWidth         , "1000");
            sebSettingsDef.Add(SEBGlobalConstants.MessageNewBrowserWindowByLinkHeight        , "100%");
            sebSettingsDef.Add(SEBGlobalConstants.MessageNewBrowserWindowByLinkPositioning   , 2);

            sebSettingsDef.Add(SEBGlobalConstants.MessageEnablePlugIns           , true);
            sebSettingsDef.Add(SEBGlobalConstants.MessageEnableJava              , false);
            sebSettingsDef.Add(SEBGlobalConstants.MessageEnableJavaScript        , true);
            sebSettingsDef.Add(SEBGlobalConstants.MessageBlockPopUpWindows       , false);
            sebSettingsDef.Add(SEBGlobalConstants.MessageAllowBrowsingBackForward, false);
            sebSettingsDef.Add(SEBGlobalConstants.MessageEnableSebBrowser        , true);

            // Default settings for group "DownUploads"
            sebSettingsDef.Add(SEBGlobalConstants.MessageAllowDownUploads        , true);
            sebSettingsDef.Add(SEBGlobalConstants.MessageDownloadDirectoryOSX    , "~/Downloads");
            sebSettingsDef.Add(SEBGlobalConstants.MessageDownloadDirectoryWin    , "Desktop");
            sebSettingsDef.Add(SEBGlobalConstants.MessageOpenDownloads           , false);
            sebSettingsDef.Add(SEBGlobalConstants.MessageChooseFileToUploadPolicy, 0);
            sebSettingsDef.Add(SEBGlobalConstants.MessageDownloadPDFFiles        , false);

            // Default settings for group "Exam"
            sebSettingsDef.Add(SEBGlobalConstants.MessageExamKeySalt       , new Byte[] {});
            sebSettingsDef.Add(SEBGlobalConstants.MessageBrowserExamKey    , "");
            sebSettingsDef.Add(SEBGlobalConstants.MessageCopyBrowserExamKey, false);
            sebSettingsDef.Add(SEBGlobalConstants.MessageSendBrowserExamKey, false);
            sebSettingsDef.Add(SEBGlobalConstants.MessageQuitURL           , "");

            // Default settings for group "Applications"
            sebSettingsDef.Add(SEBGlobalConstants.MessageMonitorProcesses         , false);
            sebSettingsDef.Add(SEBGlobalConstants.MessagePermittedProcesses       , new List<object>());
            sebSettingsDef.Add(SEBGlobalConstants.MessageAllowSwitchToApplications, false);
            sebSettingsDef.Add(SEBGlobalConstants.MessageAllowFlashFullscreen     , false);
            sebSettingsDef.Add(SEBGlobalConstants.MessageProhibitedProcesses      , new List<object>());

            permittedProcessDataDef.Add(SEBGlobalConstants.MessageActive     , true);
            permittedProcessDataDef.Add(SEBGlobalConstants.MessageAutostart  , true);
            permittedProcessDataDef.Add(SEBGlobalConstants.MessageAutohide   , true);
            permittedProcessDataDef.Add(SEBGlobalConstants.MessageAllowUser  , true);
            permittedProcessDataDef.Add(SEBGlobalConstants.MessageOS         , IntWin);
            permittedProcessDataDef.Add(SEBGlobalConstants.MessageTitle      , "");
            permittedProcessDataDef.Add(SEBGlobalConstants.MessageDescription, "");
            permittedProcessDataDef.Add(SEBGlobalConstants.MessageExecutable , "");
            permittedProcessDataDef.Add(SEBGlobalConstants.MessagePath       , "");
            permittedProcessDataDef.Add(SEBGlobalConstants.MessageIdentifier , "");
            permittedProcessDataDef.Add(SEBGlobalConstants.MessageArguments  , new List<object>());

            permittedArgumentDataDef.Add(SEBGlobalConstants.MessageActive  , true);
            permittedArgumentDataDef.Add(SEBGlobalConstants.MessageArgument, "");

            prohibitedProcessDataDef.Add(SEBGlobalConstants.MessageActive     , true);
            prohibitedProcessDataDef.Add(SEBGlobalConstants.MessageCurrentUser, true);
            prohibitedProcessDataDef.Add(SEBGlobalConstants.MessageStrongKill , false);
            prohibitedProcessDataDef.Add(SEBGlobalConstants.MessageOS         , IntWin);
            prohibitedProcessDataDef.Add(SEBGlobalConstants.MessageExecutable , "");
            prohibitedProcessDataDef.Add(SEBGlobalConstants.MessageDescription, "");
            prohibitedProcessDataDef.Add(SEBGlobalConstants.MessageIdentifier , "");
            prohibitedProcessDataDef.Add(SEBGlobalConstants.MessageUser       , "");

            // Default settings for group "Network - Filter"
            sebSettingsDef.Add(SEBGlobalConstants.MessageEnableURLFilter       , false);
            sebSettingsDef.Add(SEBGlobalConstants.MessageEnableURLContentFilter, false);
            sebSettingsDef.Add(SEBGlobalConstants.MessageURLFilterRules        , new List<object>());

            // Create a default action
            urlFilterActionDataDefault.Add(SEBGlobalConstants.MessageActive    , true);
            urlFilterActionDataDefault.Add(SEBGlobalConstants.MessageRegex     , false);
            urlFilterActionDataDefault.Add(SEBGlobalConstants.MessageExpression, "*");
            urlFilterActionDataDefault.Add(SEBGlobalConstants.MessageAction    , 0);

            // Create a default action list with one entry (SEBGlobalConstants.the default action)
            urlFilterActionListDefault.Add(urlFilterActionDataDefault);

            // Create a default rule with this default action list.
            // This default rule is used for the "Insert Rule" operation:
            // when a new rule is created, it initially contains one action.
            urlFilterRuleDataDefault.Add(SEBGlobalConstants.MessageActive     , true);
            urlFilterRuleDataDefault.Add(SEBGlobalConstants.MessageExpression , "Rule");
            urlFilterRuleDataDefault.Add(SEBGlobalConstants.MessageRuleActions, urlFilterActionListDefault);

            // Initialise the stored action
            urlFilterActionDataStored.Add(SEBGlobalConstants.MessageActive    , true);
            urlFilterActionDataStored.Add(SEBGlobalConstants.MessageRegex     , false);
            urlFilterActionDataStored.Add(SEBGlobalConstants.MessageExpression, "*");
            urlFilterActionDataStored.Add(SEBGlobalConstants.MessageAction    , 0);

            // Initialise the stored rule
            urlFilterRuleDataStored.Add(SEBGlobalConstants.MessageActive     , true);
            urlFilterRuleDataStored.Add(SEBGlobalConstants.MessageExpression , "Rule");
            urlFilterRuleDataStored.Add(SEBGlobalConstants.MessageRuleActions, urlFilterActionListStored);

            // Default settings for group "Network - Certificates"
            sebSettingsDef.Add(SEBGlobalConstants.MessageEmbeddedCertificates, new List<object>());

            embeddedCertificateDataDef.Add(SEBGlobalConstants.MessageCertificateData, "");
            embeddedCertificateDataDef.Add(SEBGlobalConstants.MessageType           , 0);
            embeddedCertificateDataDef.Add(SEBGlobalConstants.MessageName           , "");

            // Default settings for group "Network - Proxies"
            sebSettingsDef.Add(SEBGlobalConstants.MessageProxySettingsPolicy      , 0);
            sebSettingsDef.Add(SEBGlobalConstants.MessageProxyProtocol            , 0);
            sebSettingsDef.Add(SEBGlobalConstants.MessageProxyConfigurationFileURL, "");
            sebSettingsDef.Add(SEBGlobalConstants.MessageExcludeSimpleHostnames   , true);
            sebSettingsDef.Add(SEBGlobalConstants.MessageUsePassiveFTPMode        , true);
            sebSettingsDef.Add(SEBGlobalConstants.MessageBypassHostsAndDomains, new List<object>());

            // Default settings for group "Security"
            sebSettingsDef.Add(SEBGlobalConstants.MessageSebServicePolicy   , 2);
            sebSettingsDef.Add(SEBGlobalConstants.MessageAllowVirtualMachine, false);
            sebSettingsDef.Add(SEBGlobalConstants.MessageCreateNewDesktop   , true);
            sebSettingsDef.Add(SEBGlobalConstants.MessageAllowUserSwitching , true);
            sebSettingsDef.Add(SEBGlobalConstants.MessageEnableLogging      , false);
            sebSettingsDef.Add(SEBGlobalConstants.MessageLogDirectoryOSX    , "~/Documents");
            sebSettingsDef.Add(SEBGlobalConstants.MessageLogDirectoryWin    , "My Documents");

            // Default settings for group "Inside SEB"
            sebSettingsDef.Add(SEBGlobalConstants.MessageInsideSebEnableSwitchUser       , false);
            sebSettingsDef.Add(SEBGlobalConstants.MessageInsideSebEnableLockThisComputer , false);
            sebSettingsDef.Add(SEBGlobalConstants.MessageInsideSebEnableChangeAPassword  , false);
            sebSettingsDef.Add(SEBGlobalConstants.MessageInsideSebEnableStartTaskManager , false);
            sebSettingsDef.Add(SEBGlobalConstants.MessageInsideSebEnableLogOff           , false);
            sebSettingsDef.Add(SEBGlobalConstants.MessageInsideSebEnableShutDown         , false);
            sebSettingsDef.Add(SEBGlobalConstants.MessageInsideSebEnableEaseOfAccess     , false);
            sebSettingsDef.Add(SEBGlobalConstants.MessageInsideSebEnableVmWareClientShade, false);

            // Default settings for group "Outside SEB"
            sebSettingsDef.Add(SEBGlobalConstants.MessageOutsideSebEnableSwitchUser       , true);
            sebSettingsDef.Add(SEBGlobalConstants.MessageOutsideSebEnableLockThisComputer , true);
            sebSettingsDef.Add(SEBGlobalConstants.MessageOutsideSebEnableChangeAPassword  , true);
            sebSettingsDef.Add(SEBGlobalConstants.MessageOutsideSebEnableStartTaskManager , true);
            sebSettingsDef.Add(SEBGlobalConstants.MessageOutsideSebEnableLogOff           , true);
            sebSettingsDef.Add(SEBGlobalConstants.MessageOutsideSebEnableShutDown         , true);
            sebSettingsDef.Add(SEBGlobalConstants.MessageOutsideSebEnableEaseOfAccess     , true);
            sebSettingsDef.Add(SEBGlobalConstants.MessageOutsideSebEnableVmWareClientShade, true);

            // Default settings for group "Hooked Keys"
            sebSettingsDef.Add(SEBGlobalConstants.MessageHookKeys, true);

            // Default settings for group "Special Keys"
            sebSettingsDef.Add(SEBGlobalConstants.MessageEnableEsc       , false);
            sebSettingsDef.Add(SEBGlobalConstants.MessageEnableCtrlEsc   , false);
            sebSettingsDef.Add(SEBGlobalConstants.MessageEnableAltEsc    , false);
            sebSettingsDef.Add(SEBGlobalConstants.MessageEnableAltTab    , true);
            sebSettingsDef.Add(SEBGlobalConstants.MessageEnableAltF4     , false);
            sebSettingsDef.Add(SEBGlobalConstants.MessageEnableStartMenu , false);
            sebSettingsDef.Add(SEBGlobalConstants.MessageEnableRightMouse, false);

            // Default settings for group "Function Keys"
            sebSettingsDef.Add(SEBGlobalConstants.MessageEnableF1 , false);
            sebSettingsDef.Add(SEBGlobalConstants.MessageEnableF2 , false);
            sebSettingsDef.Add(SEBGlobalConstants.MessageEnableF3 , false);
            sebSettingsDef.Add(SEBGlobalConstants.MessageEnableF4 , false);
            sebSettingsDef.Add(SEBGlobalConstants.MessageEnableF5 , true);
            sebSettingsDef.Add(SEBGlobalConstants.MessageEnableF6 , false);
            sebSettingsDef.Add(SEBGlobalConstants.MessageEnableF7 , false);
            sebSettingsDef.Add(SEBGlobalConstants.MessageEnableF8 , false);
            sebSettingsDef.Add(SEBGlobalConstants.MessageEnableF9 , false);
            sebSettingsDef.Add(SEBGlobalConstants.MessageEnableF10, false);
            sebSettingsDef.Add(SEBGlobalConstants.MessageEnableF11, false);
            sebSettingsDef.Add(SEBGlobalConstants.MessageEnableF12, false);
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
        }
    }
}
