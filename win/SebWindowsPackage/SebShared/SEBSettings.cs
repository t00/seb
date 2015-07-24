//
//  SEBSettings.cs
//  SafeExamBrowser
//
//  Copyright (c) 2010-2015 Viktor Tomas, Dirk Bauer, Daniel R. Schneider, Pascal Wyss,
//  ETH Zurich, Educational Development and Technology (LET),
//  based on the original idea of Safe Exam Browser
//  by Stefan Schneider, University of Giessen
//  Project concept: Thomas Piendl, Daniel R. Schneider,
//  Dirk Bauer, Kai Reuter, Tobias Halbherr, Karsten Burger, Marco Lehre,
//  Brigitte Schmucki, Oliver Rahs. French localization: Nicolas Dunand
//
//  ``The contents of this file are subject to the Mozilla Public License
//  Version 1.1 (the "License"); you may not use this file except in
//  compliance with the License. You may obtain a copy of the License at
//  http://www.mozilla.org/MPL/
//
//  Software distributed under the License is distributed on an "AS IS"
//  basis, WITHOUT WARRANTY OF ANY KIND, either express or implied. See the
//  License for the specific language governing rights and limitations
//  under the License.
//
//  The Original Code is Safe Exam Browser for Windows.
//
//  The Initial Developers of the Original Code are Viktor Tomas, 
//  Dirk Bauer, Daniel R. Schneider, Pascal Wyss.
//  Portions created by Viktor Tomas, Dirk Bauer, Daniel R. Schneider, Pascal Wyss
//  are Copyright (c) 2010-2014 Viktor Tomas, Dirk Bauer, Daniel R. Schneider, 
//  Pascal Wyss, ETH Zurich, Educational Development and Technology (LET), 
//  based on the original idea of Safe Exam Browser
//  by Stefan Schneider, University of Giessen. All Rights Reserved.
//
//  Contributor(s): ______________________________________.
//

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using SebShared.DiagnosticUtils;

namespace SebShared
{
	public class SebSettings
	{
		// **************************
		// Constants for SEB settings
		// **************************

		// The default SEB configuration file
		public const String DefaultSebConfigXml = "SebClient.xml";
		public const String DefaultSebConfigSeb = "SebClient.seb";

		// Operating systems
		const int IntOSX = 0;
		const int IntWin = 1;

		// Some key/value pairs are not stored in the sebSettings Plist structures,
		// so they must be separately stored in arrays
		public const int ValCryptoIdentity = 1;
		public const int ValMainBrowserWindowWidth = 2;
		public const int ValMainBrowserWindowHeight = 3;
		public const int ValNewBrowserWindowByLinkWidth = 4;
		public const int ValNewBrowserWindowByLinkHeight = 5;
		public const int ValTaskBarHeight = 6;
		public const int ValNum = 6;

		// Keys not belonging to any group
		public const String KeyOriginatorVersion = "originatorVersion";

		// Group "General"
		public const String KeyStartURL = "startURL";
		public const String KeySebServerURL = "sebServerURL";
		public const String KeyHashedAdminPassword = "hashedAdminPassword";
		public const String KeyAllowQuit = "allowQuit";
		public const String KeyIgnoreExitKeys = "ignoreExitKeys";
		public const String KeyHashedQuitPassword = "hashedQuitPassword";
		public const String KeyExitKey1 = "exitKey1";
		public const String KeyExitKey2 = "exitKey2";
		public const String KeyExitKey3 = "exitKey3";
		public const String KeySebMode = "sebMode";
		public const String KeyBrowserMessagingSocket = "browserMessagingSocket";
		public const String KeyBrowserMessagingPingTime = "browserMessagingPingTime";

		// Group "Config File"
		public const String KeySebConfigPurpose = "sebConfigPurpose";
		public const String KeyAllowPreferencesWindow = "allowPreferencesWindow";
		public const String KeyCryptoIdentity = "cryptoIdentity";
		public const String KeySebStoreConfig = "storeConfig";

		// Group "Appearance"
		public const String KeyBrowserViewMode = "browserViewMode";
		public const String KeyMainBrowserWindowWidth = "mainBrowserWindowWidth";
		public const String KeyMainBrowserWindowHeight = "mainBrowserWindowHeight";
		public const String KeyMainBrowserWindowPositioning = "mainBrowserWindowPositioning";
		public const String KeyEnableBrowserWindowToolbar = "enableBrowserWindowToolbar";
		public const String KeyHideBrowserWindowToolbar = "hideBrowserWindowToolbar";
		public const String KeyShowMenuBar = "showMenuBar";
		public const String KeyShowTaskBar = "showTaskBar";
		public const String KeyTaskBarHeight = "taskBarHeight";
		public const String KeyTouchOptimized = "touchOptimized";
		public const String KeyEnableZoomText = "enableZoomText";
		public const String KeyEnableZoomPage = "enableZoomPage";
		public const String KeyEnableOnScreenKeyboardNative = "enableOSKWindows";
		public const String KeyEnableOnScreenKeyboardWeb = "enableOSKWeb";
		public const String KeyZoomMode = "zoomMode";
		public const String KeyAllowSpellCheck = "allowSpellCheck";
		public const String KeyShowTime = "showTime";
		public const String KeyShowInputLanguage = "showInputLanguage";
		public const String KeyAllowDictionaryLookup = "allowDictionaryLookup";

		//Touch optimized settings
		public const String KeyBrowserScreenKeyboard = "browserScreenKeyboard";

		// Group "Browser"
		public const String KeyNewBrowserWindowByLinkPolicy = "newBrowserWindowByLinkPolicy";
		public const String KeyNewBrowserWindowByScriptPolicy = "newBrowserWindowByScriptPolicy";
		public const String KeyNewBrowserWindowByLinkBlockForeign = "newBrowserWindowByLinkBlockForeign";
		public const String KeyNewBrowserWindowByScriptBlockForeign = "newBrowserWindowByScriptBlockForeign";
		public const String KeyNewBrowserWindowByLinkWidth = "newBrowserWindowByLinkWidth";
		public const String KeyNewBrowserWindowByLinkHeight = "newBrowserWindowByLinkHeight";
		public const String KeyNewBrowserWindowByLinkPositioning = "newBrowserWindowByLinkPositioning";
		public const String KeyEnablePlugIns = "enablePlugIns";
		public const String KeyEnableJava = "enableJava";
		public const String KeyEnableJavaScript = "enableJavaScript";
		public const String KeyBlockPopUpWindows = "blockPopUpWindows";
		public const String KeyAllowBrowsingBackForward = "allowBrowsingBackForward";
		public const String KeyRemoveBrowserProfile = "removeBrowserProfile";
		public const String KeyDisableLocalStorage = "removeLocalStorage";
		public const String KeyEnableSebBrowser = "enableSebBrowser";
		public const String KeyShowReloadButton = "showReloadButton";
		public const String KeyShowReloadWarning = "showReloadWarning";
		public const String KeyBrowserUserAgentDesktopMode = "browserUserAgentWinDesktopMode";
		public const String KeyBrowserUserAgentDesktopModeCustom = "browserUserAgentWinDesktopModeCustom";
		public const String KeyBrowserUserAgentTouchMode = "browserUserAgentWinTouchMode";
		public const String KeyBrowserUserAgentTouchModeCustom = "browserUserAgentWinTouchModeCustom";
		public const String KeyBrowserUserAgent = "browserUserAgent";
		public const String KeyBrowserUserAgentMac = "browserUserAgentMac";
		public const String KeyBrowserUserAgentMacCustom = "browserUserAgentMacCustom";

		// Group "DownUploads"
		public const String KeyAllowDownUploads = "allowDownUploads";
		public const String KeyDownloadDirectoryOSX = "downloadDirectoryOSX";
		public const String KeyDownloadDirectoryWin = "downloadDirectoryWin";
		public const String KeyOpenDownloads = "openDownloads";
		public const String KeyChooseFileToUploadPolicy = "chooseFileToUploadPolicy";
		public const String KeyDownloadPDFFiles = "downloadPDFFiles";
		public const String KeyDownloadAndOpenSebConfig = "downloadAndOpenSebConfig";

		// Group "Exam"
		public const String KeyExamKeySalt = "examKeySalt";
		public const String KeyBrowserExamKey = "browserExamKey";
		public const String KeyBrowserURLSalt = "browserURLSalt";
		public const String KeySendBrowserExamKey = "sendBrowserExamKey";
		public const String KeyQuitURL = "quitURL";
		public const String KeyRestartExamText = "restartExamText";
		public const String KeyRestartExamURL = "restartExamURL";
		public const String KeyRestartExamUseStartURL = "restartExamUseStartURL";
		public const String KeyRestartExamPasswordProtected = "restartExamPasswordProtected";

		// Group "Applications"
		public const String KeyMonitorProcesses = "monitorProcesses";

		// Group "Applications - Permitted  Processes"
		public const String KeyPermittedProcesses = "permittedProcesses";
		public const String KeyAllowSwitchToApplications = "allowSwitchToApplications";
		public const String KeyAllowFlashFullscreen = "allowFlashFullscreen";

		// Group "Applications - Prohibited Processes"
		public const String KeyProhibitedProcesses = "prohibitedProcesses";

		public const String KeyActive = "active";
		public const String KeyAutostart = "autostart";
		public const String KeyIconInTaskbar = "iconInTaskbar";
		public const String KeyRunInBackground = "runInBackground";
		public const String KeyAllowUser = "allowUserToChooseApp";
		public const String KeyCurrentUser = "currentUser";
		public const String KeyStrongKill = "strongKill";
		public const String KeyOS = "os";
		public const String KeyTitle = "title";
		public const String KeyDescription = "description";
		public const String KeyExecutable = "executable";
		public const String KeyPath = "path";
		public const String KeyIdentifier = "identifier";
		public const String KeyUser = "user";
		public const String KeyArguments = "arguments";
		public const String KeyArgument = "argument";
		public const String KeyWindowHandlingProcess = "windowHandlingProcess";

		// Group "Network"
		public const String KeyEnableURLFilter = "enableURLFilter";
		public const String KeyEnableURLContentFilter = "enableURLContentFilter";

		// New "Network" - Filter
		public const String KeyURLFilterEnable = "URLFilterEnable";
		public const String KeyURLFilterEnableContentFilter = "URLFilterEnableContentFilter";
		public const String KeyURLFilterRules = "URLFilterRules";

		//Group "Network" - URL Filter XULRunner keys
		public const String KeyUrlFilterBlacklist = "blacklistURLFilter";
		public const String KeyUrlFilterWhitelist = "whitelistURLFilter";
		public const String KeyUrlFilterTrustedContent = "urlFilterTrustedContent";
		public const String KeyUrlFilterRulesAsRegex = "urlFilterRegex";

		// Group "Network - Certificates"
		//public const String KeyEmbedSSLServerCertificate = "EmbedSSLServerCertificate";
		//public const String KeyEmbedIdentity             = "EmbedIdentity";
		public const String KeyEmbeddedCertificates = "embeddedCertificates";
		public const String KeyCertificateDataWin = "certificateDataWin";
		public const String KeyCertificateData = "certificateData";
		public const String KeyType = "type";
		public const String KeyName = "name";

		// Group "Network - Proxies"
		public const String KeyProxySettingsPolicy = "proxySettingsPolicy";

		public const String KeyProxies = "proxies";
		public const String KeyExceptionsList = "ExceptionsList";
		public const String KeyExcludeSimpleHostnames = "ExcludeSimpleHostnames";
		public const String KeyFTPPassive = "FTPPassive";

		public const String KeyAutoDiscoveryEnabled = "AutoDiscoveryEnabled";
		public const String KeyAutoConfigurationEnabled = "AutoConfigurationEnabled";
		public const String KeyAutoConfigurationJavaScript = "AutoConfigurationJavaScript";
		public const String KeyAutoConfigurationURL = "AutoConfigurationURL";

		public const String KeyAutoDiscovery = "";
		public const String KeyAutoConfiguration = "";
		public const String KeyHTTP = "HTTP";
		public const String KeyHTTPS = "HTTPS";
		public const String KeyFTP = "FTP";
		public const String KeySOCKS = "SOCKS";
		public const String KeyRTSP = "RTSP";

		public const String KeyEnable = "Enable";
		public const String KeyPort = "Port";
		public const String KeyHost = "Proxy";
		public const String KeyRequires = "RequiresPassword";
		public const String KeyUsername = "Username";
		public const String KeyPassword = "Password";

		public const String KeyHTTPEnable = "HTTPEnable";
		public const String KeyHTTPPort = "HTTPPort";
		public const String KeyHTTPHost = "HTTPProxy";
		public const String KeyHTTPRequires = "HTTPRequiresPassword";
		public const String KeyHTTPUsername = "HTTPUsername";
		public const String KeyHTTPPassword = "HTTPPassword";

		public const String KeyHTTPSEnable = "HTTPSEnable";
		public const String KeyHTTPSPort = "HTTPSPort";
		public const String KeyHTTPSHost = "HTTPSProxy";
		public const String KeyHTTPSRequires = "HTTPSRequiresPassword";
		public const String KeyHTTPSUsername = "HTTPSUsername";
		public const String KeyHTTPSPassword = "HTTPSPassword";

		public const String KeyFTPEnable = "FTPEnable";
		public const String KeyFTPPort = "FTPPort";
		public const String KeyFTPHost = "FTPProxy";
		public const String KeyFTPRequires = "FTPRequiresPassword";
		public const String KeyFTPUsername = "FTPUsername";
		public const String KeyFTPPassword = "FTPPassword";

		public const String KeySOCKSEnable = "SOCKSEnable";
		public const String KeySOCKSPort = "SOCKSPort";
		public const String KeySOCKSHost = "SOCKSProxy";
		public const String KeySOCKSRequires = "SOCKSRequiresPassword";
		public const String KeySOCKSUsername = "SOCKSUsername";
		public const String KeySOCKSPassword = "SOCKSPassword";

		public const String KeyRTSPEnable = "RTSPEnable";
		public const String KeyRTSPPort = "RTSPPort";
		public const String KeyRTSPHost = "RTSPProxy";
		public const String KeyRTSPRequires = "RTSPRequiresPassword";
		public const String KeyRTSPUsername = "RTSPUsername";
		public const String KeyRTSPPassword = "RTSPPassword";

		// Group "Security"
		public const String KeySebServicePolicy = "sebServicePolicy";
		public const String KeyAllowVirtualMachine = "allowVirtualMachine";
		public const String KeyCreateNewDesktop = "createNewDesktop";
		public const String KeyKillExplorerShell = "killExplorerShell";
		public const String KeyAllowUserSwitching = "allowUserSwitching";
		public const String KeyEnableAppSwitcherCheck = "enableAppSwitcherCheck";
		public const String KeyForceAppFolderInstall = "forceAppFolderInstall";
		public const String KeyEnableLogging = "enableLogging";
		public const String KeyLogDirectoryOSX = "logDirectoryOSX";
		public const String KeyLogDirectoryWin = "logDirectoryWin";
		public const String KeyAllowWLAN = "allowWlan";

		// Group "Registry"

		// Group "Inside SEB"
		public const String KeyInsideSebEnableSwitchUser = "insideSebEnableSwitchUser";
		public const String KeyInsideSebEnableLockThisComputer = "insideSebEnableLockThisComputer";
		public const String KeyInsideSebEnableChangeAPassword = "insideSebEnableChangeAPassword";
		public const String KeyInsideSebEnableStartTaskManager = "insideSebEnableStartTaskManager";
		public const String KeyInsideSebEnableLogOff = "insideSebEnableLogOff";
		public const String KeyInsideSebEnableShutDown = "insideSebEnableShutDown";
		public const String KeyInsideSebEnableEaseOfAccess = "insideSebEnableEaseOfAccess";
		public const String KeyInsideSebEnableVmWareClientShade = "insideSebEnableVmWareClientShade";

		// Group "Hooked Keys"
		public const String KeyHookKeys = "hookKeys";

		// Group "Special Keys"
		public const String KeyEnableEsc = "enableEsc";
		public const String KeyEnablePrintScreen = "enablePrintScreen";
		public const String KeyEnableCtrlEsc = "enableCtrlEsc";
		public const String KeyEnableAltEsc = "enableAltEsc";
		public const String KeyEnableAltTab = "enableAltTab";
		public const String KeyEnableAltF4 = "enableAltF4";
		public const String KeyEnableStartMenu = "enableStartMenu";
		public const String KeyEnableRightMouse = "enableRightMouse";
		public const String KeyEnableAltMouseWheel = "enableAltMouseWheel";

		// Group "Function Keys"
		public const String KeyEnableF1 = "enableF1";
		public const String KeyEnableF2 = "enableF2";
		public const String KeyEnableF3 = "enableF3";
		public const String KeyEnableF4 = "enableF4";
		public const String KeyEnableF5 = "enableF5";
		public const String KeyEnableF6 = "enableF6";
		public const String KeyEnableF7 = "enableF7";
		public const String KeyEnableF8 = "enableF8";
		public const String KeyEnableF9 = "enableF9";
		public const String KeyEnableF10 = "enableF10";
		public const String KeyEnableF11 = "enableF11";
		public const String KeyEnableF12 = "enableF12";

		public enum sebConfigPurposes
		{
			sebConfigPurposeStartingExam, sebConfigPurposeConfiguringClient
		}

		public enum operatingSystems
		{
			operatingSystemOSX, operatingSystemWin
		}

		public T Get<T>(string key)
		{
			return Get<T>(key, default(T));
		}

		public T Get<T>(string key, T defaultValue)
		{
			object value;
			if(settingsCurrent.TryGetValue(key, out value))
			{
				return (T) value;
			}
			return defaultValue;
		}

		public void Set<T>(string key, T value)
		{
			settingsCurrent[key] = value;
		}

		// *********************************
		// Global Variables for SEB settings
		// *********************************

		// Some settings are not stored in Plists but in Arrays
		public String[] strArrayDefault = new String[ValNum + 1];
		public String[] strArrayCurrent = new String[ValNum + 1];

		public int[] intArrayDefault = new int[ValNum + 1];
		public int[] intArrayCurrent = new int[ValNum + 1];

		// Class SEBSettings contains all settings
		// and is used for importing/exporting the settings
		// from/to a human-readable .xml and an encrypted.seb file format.
		public Dictionary<string, object> settingsDefault = new Dictionary<string, object>();
		public Dictionary<string, object> settingsCurrent = new Dictionary<string, object>();

		public int permittedProcessIndex;
		public List<object> permittedProcessList = new List<object>();
		public Dictionary<string, object> permittedProcessData = new Dictionary<string, object>();
		public Dictionary<string, object> permittedProcessDataDefault = new Dictionary<string, object>();
		public Dictionary<string, object> permittedProcessDataXulRunner = new Dictionary<string, object>();

		public int permittedArgumentIndex;
		public List<object> permittedArgumentList = new List<object>();
		public Dictionary<string, object> permittedArgumentData = new Dictionary<string, object>();
		public Dictionary<string, object> permittedArgumentDataDefault = new Dictionary<string, object>();
		public Dictionary<string, object> permittedArgumentDataXulRunner1 = new Dictionary<string, object>();
		public Dictionary<string, object> permittedArgumentDataXulRunner2 = new Dictionary<string, object>();
		public List<object> permittedArgumentListXulRunner = new List<object>();

		public int prohibitedProcessIndex;
		public List<object> prohibitedProcessList = new List<object>();
		public Dictionary<string, object> prohibitedProcessData = new Dictionary<string, object>();
		public Dictionary<string, object> prohibitedProcessDataDefault = new Dictionary<string, object>();

		public int urlFilterRuleIndex;
		public List<object> urlFilterRuleList = new List<object>();
		public Dictionary<string, object> urlFilterRuleData = new Dictionary<string, object>();
		public Dictionary<string, object> urlFilterRuleDataDefault = new Dictionary<string, object>();
		public Dictionary<string, object> urlFilterRuleDataStorage = new Dictionary<string, object>();

		public int urlFilterActionIndex;
		public List<object> urlFilterActionList = new List<object>();
		public List<object> urlFilterActionListDefault = new List<object>();
		public List<object> urlFilterActionListStorage = new List<object>();
		public Dictionary<string, object> urlFilterActionData = new Dictionary<string, object>();
		public Dictionary<string, object> urlFilterActionDataDefault = new Dictionary<string, object>();
		public Dictionary<string, object> urlFilterActionDataStorage = new Dictionary<string, object>();

		public int embeddedCertificateIndex;
		public List<object> embeddedCertificateList = new List<object>();
		public Dictionary<string, object> embeddedCertificateData = new Dictionary<string, object>();
		public Dictionary<string, object> embeddedCertificateDataDefault = new Dictionary<string, object>();

		public Dictionary<string, object> proxiesData = new Dictionary<string, object>();
		public Dictionary<string, object> proxiesDataDefault = new Dictionary<string, object>();

		public int proxyProtocolIndex;

		public int bypassedProxyIndex;
		public List<object> bypassedProxyList = new List<object>();
		public String bypassedProxyData = "";
		public String bypassedProxyDataDefault = "";

		public bool IsEmpty
		{
			get { return !settingsCurrent.Any(); }
		}


		// ************************
		// Methods for SEB settings
		// ************************


		// ********************************************************************
		// Set all the default values for the Plist structure "settingsDefault"
		// ********************************************************************
		public void CreateDefaultAndCurrentSettingsFromScratch()
		{
			// Destroy all default lists and dictionaries
			settingsDefault = new Dictionary<string, object>();
			settingsCurrent = new Dictionary<string, object>();

			permittedProcessList = new List<object>();
			permittedProcessData = new Dictionary<string, object>();
			permittedProcessDataDefault = new Dictionary<string, object>();
			permittedProcessDataXulRunner = new Dictionary<string, object>();

			permittedArgumentList = new List<object>();
			permittedArgumentData = new Dictionary<string, object>();
			permittedArgumentDataDefault = new Dictionary<string, object>();
			permittedArgumentDataXulRunner1 = new Dictionary<string, object>();
			permittedArgumentDataXulRunner2 = new Dictionary<string, object>();
			permittedArgumentListXulRunner = new List<object>();

			prohibitedProcessList = new List<object>();
			prohibitedProcessData = new Dictionary<string, object>();
			prohibitedProcessDataDefault = new Dictionary<string, object>();

			urlFilterRuleList = new List<object>();
			urlFilterRuleData = new Dictionary<string, object>();
			urlFilterRuleDataDefault = new Dictionary<string, object>();
			urlFilterRuleDataStorage = new Dictionary<string, object>();

			urlFilterActionList = new List<object>();
			urlFilterActionListDefault = new List<object>();
			urlFilterActionListStorage = new List<object>();
			urlFilterActionData = new Dictionary<string, object>();
			urlFilterActionDataDefault = new Dictionary<string, object>();
			urlFilterActionDataStorage = new Dictionary<string, object>();

			embeddedCertificateList = new List<object>();
			embeddedCertificateData = new Dictionary<string, object>();
			embeddedCertificateDataDefault = new Dictionary<string, object>();

			proxiesData = new Dictionary<string, object>();
			proxiesDataDefault = new Dictionary<string, object>();

			bypassedProxyList = new List<object>();
			bypassedProxyData = "";
			bypassedProxyDataDefault = "";


			// Initialise the global arrays
			for(int value = 1; value <= ValNum; value++)
			{
				intArrayDefault[value] = 0;
				intArrayCurrent[value] = 0;

				strArrayDefault[value] = "";
				strArrayCurrent[value] = "";
			}

			// Initialise the default settings Plist
			settingsDefault.Clear();

			// Default settings for keys not belonging to any group
			settingsDefault.Add(KeyOriginatorVersion, SebConstants.SEB_VERSION);

			// Default settings for group "General"
			settingsDefault.Add(KeyStartURL, "");
			settingsDefault.Add(KeySebServerURL, "");
			settingsDefault.Add(KeyHashedAdminPassword, "");
			settingsDefault.Add(KeyAllowQuit, true);
			settingsDefault.Add(KeyIgnoreExitKeys, true);
			settingsDefault.Add(KeyHashedQuitPassword, "");
			settingsDefault.Add(KeyExitKey1, 2);
			settingsDefault.Add(KeyExitKey2, 10);
			settingsDefault.Add(KeyExitKey3, 5);
			settingsDefault.Add(KeySebMode, 0);
			settingsDefault.Add(KeyBrowserMessagingPingTime, 120000);

			// Default settings for group "Config File"
			settingsDefault.Add(KeySebConfigPurpose, 0);
			settingsDefault.Add(KeySebStoreConfig, true);
			settingsDefault.Add(KeyAllowPreferencesWindow, true);
			//SEBSettings.settingsDefault.Add(SEBSettings.KeyHashedSettingsPassword , "");

			// CryptoIdentity is stored additionally
			intArrayDefault[ValCryptoIdentity] = 0;
			strArrayDefault[ValCryptoIdentity] = "";

			// Default settings for group "User Interface"
			settingsDefault.Add(KeyBrowserViewMode, 0);
			settingsDefault.Add(KeyMainBrowserWindowWidth, "100%");
			settingsDefault.Add(KeyMainBrowserWindowHeight, "100%");
			settingsDefault.Add(KeyMainBrowserWindowPositioning, 1);
			settingsDefault.Add(KeyEnableBrowserWindowToolbar, false);
			settingsDefault.Add(KeyHideBrowserWindowToolbar, false);
			settingsDefault.Add(KeyShowMenuBar, false);
			settingsDefault.Add(KeyShowTaskBar, true);
			settingsDefault.Add(KeyTaskBarHeight, 40);
			settingsDefault.Add(KeyTouchOptimized, false);
			settingsDefault.Add(KeyEnableOnScreenKeyboardNative, false);
			settingsDefault.Add(KeyEnableOnScreenKeyboardWeb, true);
			settingsDefault.Add(KeyEnableZoomText, true);
			settingsDefault.Add(KeyEnableZoomPage, true);
			settingsDefault.Add(KeyZoomMode, 0);
			settingsDefault.Add(KeyAllowSpellCheck, false);
			settingsDefault.Add(KeyAllowDictionaryLookup, false);
			settingsDefault.Add(KeyShowTime, true);
			settingsDefault.Add(KeyShowInputLanguage, true);

			//Touch Settings
			settingsDefault.Add(KeyBrowserScreenKeyboard, false);

			// MainBrowserWindow Width and Height is stored additionally
			intArrayDefault[ValMainBrowserWindowWidth] = 2;
			intArrayDefault[ValMainBrowserWindowHeight] = 2;
			strArrayDefault[ValMainBrowserWindowWidth] = "100%";
			strArrayDefault[ValMainBrowserWindowHeight] = "100%";

			// Default settings for group "Browser"
			settingsDefault.Add(KeyNewBrowserWindowByLinkPolicy, 2);
			settingsDefault.Add(KeyNewBrowserWindowByScriptPolicy, 2);
			settingsDefault.Add(KeyNewBrowserWindowByLinkBlockForeign, false);
			settingsDefault.Add(KeyNewBrowserWindowByScriptBlockForeign, false);
			settingsDefault.Add(KeyNewBrowserWindowByLinkWidth, "1000");
			settingsDefault.Add(KeyNewBrowserWindowByLinkHeight, "100%");
			settingsDefault.Add(KeyNewBrowserWindowByLinkPositioning, 2);

			settingsDefault.Add(KeyEnablePlugIns, true);
			settingsDefault.Add(KeyEnableJava, false);
			settingsDefault.Add(KeyEnableJavaScript, true);
			settingsDefault.Add(KeyBlockPopUpWindows, false);
			settingsDefault.Add(KeyAllowBrowsingBackForward, false);
			settingsDefault.Add(KeyRemoveBrowserProfile, true);
			settingsDefault.Add(KeyDisableLocalStorage, true);
			settingsDefault.Add(KeyEnableSebBrowser, true);
			settingsDefault.Add(KeyShowReloadButton, true);
			settingsDefault.Add(KeyShowReloadWarning, true);
			settingsDefault.Add(KeyBrowserUserAgentDesktopMode, 0);
			settingsDefault.Add(KeyBrowserUserAgentDesktopModeCustom, "");
			settingsDefault.Add(KeyBrowserUserAgentTouchMode, 0);
			settingsDefault.Add(KeyBrowserUserAgentTouchModeCustom, "");
			settingsDefault.Add(KeyBrowserUserAgent, "");
			settingsDefault.Add(KeyBrowserUserAgentMac, 0);
			settingsDefault.Add(KeyBrowserUserAgentMacCustom, "");
			// NewBrowserWindow Width and Height is stored additionally
			intArrayDefault[ValNewBrowserWindowByLinkWidth] = 4;
			intArrayDefault[ValNewBrowserWindowByLinkHeight] = 2;
			strArrayDefault[ValNewBrowserWindowByLinkWidth] = "1000";
			strArrayDefault[ValNewBrowserWindowByLinkHeight] = "100%";

			// Default settings for group "DownUploads"
			settingsDefault.Add(KeyAllowDownUploads, true);
			settingsDefault.Add(KeyDownloadDirectoryOSX, "~/Downloads");
			settingsDefault.Add(KeyDownloadDirectoryWin, "");
			settingsDefault.Add(KeyOpenDownloads, false);
			settingsDefault.Add(KeyChooseFileToUploadPolicy, 0);
			settingsDefault.Add(KeyDownloadPDFFiles, false);
			settingsDefault.Add(KeyDownloadAndOpenSebConfig, true);

			// Default settings for group "Exam"
			settingsDefault.Add(KeyExamKeySalt, new Byte[] { });
			settingsDefault.Add(KeyBrowserExamKey, "");
			settingsDefault.Add(KeyBrowserURLSalt, true);
			settingsDefault.Add(KeySendBrowserExamKey, false);
			settingsDefault.Add(KeyQuitURL, "");
			settingsDefault.Add(KeyRestartExamURL, "");
			settingsDefault.Add(KeyRestartExamUseStartURL, true);
			settingsDefault.Add(KeyRestartExamText, "");
			settingsDefault.Add(KeyRestartExamPasswordProtected, true);

			// Default settings for group "Applications"
			settingsDefault.Add(KeyMonitorProcesses, true);
			settingsDefault.Add(KeyAllowSwitchToApplications, false);
			settingsDefault.Add(KeyAllowFlashFullscreen, false);
			settingsDefault.Add(KeyPermittedProcesses, new List<object>());
			settingsDefault.Add(KeyProhibitedProcesses, new List<object>());

			// Default settings for permitted argument data
			permittedArgumentDataDefault.Clear();
			permittedArgumentDataDefault.Add(KeyActive, true);
			permittedArgumentDataDefault.Add(KeyArgument, "");

			// Define the XulRunner arguments
			//SEBSettings.permittedArgumentDataXulRunner1.Clear();
			//SEBSettings.permittedArgumentDataXulRunner1.Add(SEBSettings.KeyActive, true);
			//SEBSettings.permittedArgumentDataXulRunner1.Add(SEBSettings.KeyArgument, "-app \"..\\xul_seb\\seb.ini\"");

			//SEBSettings.permittedArgumentDataXulRunner2.Clear();
			//SEBSettings.permittedArgumentDataXulRunner2.Add(SEBSettings.KeyActive, true);
			//SEBSettings.permittedArgumentDataXulRunner2.Add(SEBSettings.KeyArgument, "-profile \"%LOCALAPPDATA%\\ETH Zuerich\\xul_seb\\Profiles\"");

			// Create the XulRunner argument list with the XulRunner arguments
			permittedArgumentListXulRunner.Clear();
			permittedArgumentListXulRunner.Add(permittedArgumentDataDefault);
			//SEBSettings.permittedArgumentListXulRunner.Add(SEBSettings.permittedArgumentDataXulRunner1);
			//SEBSettings.permittedArgumentListXulRunner.Add(SEBSettings.permittedArgumentDataXulRunner2);

			// Create a XulRunner process with the XulRunner argument list
			permittedProcessDataXulRunner.Clear();
			permittedProcessDataXulRunner.Add(KeyActive, true);
			permittedProcessDataXulRunner.Add(KeyAutostart, true);
			permittedProcessDataXulRunner.Add(KeyIconInTaskbar, true);
			permittedProcessDataXulRunner.Add(KeyRunInBackground, false);
			permittedProcessDataXulRunner.Add(KeyAllowUser, false);
			permittedProcessDataXulRunner.Add(KeyStrongKill, true);
			permittedProcessDataXulRunner.Add(KeyOS, IntWin);
			permittedProcessDataXulRunner.Add(KeyTitle, "SEB");
			permittedProcessDataXulRunner.Add(KeyDescription, "");
			permittedProcessDataXulRunner.Add(KeyExecutable, SebConstants.XUL_RUNNER);
			permittedProcessDataXulRunner.Add(KeyPath, "../xulrunner/");
			permittedProcessDataXulRunner.Add(KeyIdentifier, "XULRunner");
			permittedProcessDataXulRunner.Add(KeyWindowHandlingProcess, "");
			permittedProcessDataXulRunner.Add(KeyArguments, new List<object>());

			// Default settings for permitted process data
			permittedProcessDataDefault.Clear();
			permittedProcessDataDefault.Add(KeyActive, true);
			permittedProcessDataDefault.Add(KeyAutostart, true);
			permittedProcessDataDefault.Add(KeyIconInTaskbar, true);
			permittedProcessDataDefault.Add(KeyRunInBackground, false);
			permittedProcessDataDefault.Add(KeyAllowUser, false);
			permittedProcessDataDefault.Add(KeyStrongKill, false);
			permittedProcessDataDefault.Add(KeyOS, IntWin);
			permittedProcessDataDefault.Add(KeyTitle, "");
			permittedProcessDataDefault.Add(KeyDescription, "");
			permittedProcessDataDefault.Add(KeyExecutable, "");
			permittedProcessDataDefault.Add(KeyPath, "");
			permittedProcessDataDefault.Add(KeyIdentifier, "");
			permittedProcessDataDefault.Add(KeyWindowHandlingProcess, "");
			permittedProcessDataDefault.Add(KeyArguments, new List<object>());

			// Default settings for prohibited process data
			prohibitedProcessDataDefault.Clear();
			prohibitedProcessDataDefault.Add(KeyActive, true);
			prohibitedProcessDataDefault.Add(KeyCurrentUser, true);
			prohibitedProcessDataDefault.Add(KeyStrongKill, false);
			prohibitedProcessDataDefault.Add(KeyOS, IntWin);
			prohibitedProcessDataDefault.Add(KeyExecutable, "");
			prohibitedProcessDataDefault.Add(KeyDescription, "");
			prohibitedProcessDataDefault.Add(KeyIdentifier, "");
			prohibitedProcessDataDefault.Add(KeyWindowHandlingProcess, "");
			prohibitedProcessDataDefault.Add(KeyUser, "");

			// Default settings for group "Network - Filter"
			settingsDefault.Add(KeyEnableURLFilter, false);
			settingsDefault.Add(KeyEnableURLContentFilter, false);
			settingsDefault.Add(KeyURLFilterRules, new List<object>());

			//// Create a default action
			//SEBSettings.urlFilterActionDataDefault.Clear();
			//SEBSettings.urlFilterActionDataDefault.Add(SEBSettings.KeyActive    , true);
			//SEBSettings.urlFilterActionDataDefault.Add(SEBSettings.KeyRegex     , false);
			//SEBSettings.urlFilterActionDataDefault.Add(SEBSettings.KeyExpression, "*");
			//SEBSettings.urlFilterActionDataDefault.Add(SEBSettings.KeyAction    , 0);

			//// Create a default action list with one entry (the default action)
			//SEBSettings.urlFilterActionListDefault.Clear();
			//SEBSettings.urlFilterActionListDefault.Add(SEBSettings.urlFilterActionDataDefault);

			//// Create a default rule with this default action list.
			//// This default rule is used for the "Insert Rule" operation:
			//// when a new rule is created, it initially contains one action.
			//SEBSettings.urlFilterRuleDataDefault.Clear();
			//SEBSettings.urlFilterRuleDataDefault.Add(SEBSettings.KeyActive     , true);
			//SEBSettings.urlFilterRuleDataDefault.Add(SEBSettings.KeyExpression , "Rule");
			//SEBSettings.urlFilterRuleDataDefault.Add(SEBSettings.KeyRuleActions, SEBSettings.urlFilterActionListDefault);

			//// Initialise the stored action
			//SEBSettings.urlFilterActionDataStorage.Clear();
			//SEBSettings.urlFilterActionDataStorage.Add(SEBSettings.KeyActive    , true);
			//SEBSettings.urlFilterActionDataStorage.Add(SEBSettings.KeyRegex     , false);
			//SEBSettings.urlFilterActionDataStorage.Add(SEBSettings.KeyExpression, "*");
			//SEBSettings.urlFilterActionDataStorage.Add(SEBSettings.KeyAction    , 0);

			//// Initialise the stored action list with no entry
			//SEBSettings.urlFilterActionListStorage.Clear();

			//// Initialise the stored rule
			//SEBSettings.urlFilterRuleDataStorage.Clear();
			//SEBSettings.urlFilterRuleDataStorage.Add(SEBSettings.KeyActive     , true);
			//SEBSettings.urlFilterRuleDataStorage.Add(SEBSettings.KeyExpression , "Rule");
			//SEBSettings.urlFilterRuleDataStorage.Add(SEBSettings.KeyRuleActions, SEBSettings.urlFilterActionListStorage);

			// Default settings for group "Network - Filter"
			settingsDefault.Add(KeyURLFilterEnable, false);
			settingsDefault.Add(KeyURLFilterEnableContentFilter, false);
			//SEBSettings.settingsDefault.Add(SEBSettings.KeyURLFilterRules, new ListObj());

			//Group "Network" - URL Filter XULRunner keys
			settingsDefault.Add(KeyUrlFilterBlacklist, "");
			settingsDefault.Add(KeyUrlFilterWhitelist, "");
			settingsDefault.Add(KeyUrlFilterTrustedContent, false);
			settingsDefault.Add(KeyUrlFilterRulesAsRegex, false);

			// Default settings for group "Network - Certificates"
			settingsDefault.Add(KeyEmbeddedCertificates, new List<object>());

			embeddedCertificateDataDefault.Clear();
			embeddedCertificateDataDefault.Add(KeyCertificateData, new Byte[0]);
			embeddedCertificateDataDefault.Add(KeyCertificateDataWin, "");
			embeddedCertificateDataDefault.Add(KeyType, 0);
			embeddedCertificateDataDefault.Add(KeyName, "");

			// Default settings for group "Network - Proxies"
			proxiesDataDefault.Clear();

			proxiesDataDefault.Add(KeyExceptionsList, new List<object>());
			proxiesDataDefault.Add(KeyExcludeSimpleHostnames, false);
			proxiesDataDefault.Add(KeyAutoDiscoveryEnabled, false);
			proxiesDataDefault.Add(KeyAutoConfigurationEnabled, false);
			proxiesDataDefault.Add(KeyAutoConfigurationJavaScript, "");
			proxiesDataDefault.Add(KeyAutoConfigurationURL, "");
			proxiesDataDefault.Add(KeyFTPPassive, true);

			proxiesDataDefault.Add(KeyHTTPEnable, false);
			proxiesDataDefault.Add(KeyHTTPPort, 80);
			proxiesDataDefault.Add(KeyHTTPHost, "");
			proxiesDataDefault.Add(KeyHTTPRequires, false);
			proxiesDataDefault.Add(KeyHTTPUsername, "");
			proxiesDataDefault.Add(KeyHTTPPassword, "");

			proxiesDataDefault.Add(KeyHTTPSEnable, false);
			proxiesDataDefault.Add(KeyHTTPSPort, 443);
			proxiesDataDefault.Add(KeyHTTPSHost, "");
			proxiesDataDefault.Add(KeyHTTPSRequires, false);
			proxiesDataDefault.Add(KeyHTTPSUsername, "");
			proxiesDataDefault.Add(KeyHTTPSPassword, "");

			proxiesDataDefault.Add(KeyFTPEnable, false);
			proxiesDataDefault.Add(KeyFTPPort, 21);
			proxiesDataDefault.Add(KeyFTPHost, "");
			proxiesDataDefault.Add(KeyFTPRequires, false);
			proxiesDataDefault.Add(KeyFTPUsername, "");
			proxiesDataDefault.Add(KeyFTPPassword, "");

			proxiesDataDefault.Add(KeySOCKSEnable, false);
			proxiesDataDefault.Add(KeySOCKSPort, 1080);
			proxiesDataDefault.Add(KeySOCKSHost, "");
			proxiesDataDefault.Add(KeySOCKSRequires, false);
			proxiesDataDefault.Add(KeySOCKSUsername, "");
			proxiesDataDefault.Add(KeySOCKSPassword, "");

			proxiesDataDefault.Add(KeyRTSPEnable, false);
			proxiesDataDefault.Add(KeyRTSPPort, 554);
			proxiesDataDefault.Add(KeyRTSPHost, "");
			proxiesDataDefault.Add(KeyRTSPRequires, false);
			proxiesDataDefault.Add(KeyRTSPUsername, "");
			proxiesDataDefault.Add(KeyRTSPPassword, "");

			bypassedProxyDataDefault = "";

			settingsDefault.Add(KeyProxySettingsPolicy, 0);
			settingsDefault.Add(KeyProxies, proxiesDataDefault);

			// Default settings for group "Security"
			settingsDefault.Add(KeySebServicePolicy, 1);
			settingsDefault.Add(KeyAllowVirtualMachine, false);
			settingsDefault.Add(KeyCreateNewDesktop, true);
			settingsDefault.Add(KeyKillExplorerShell, false);
			settingsDefault.Add(KeyAllowUserSwitching, true);
			settingsDefault.Add(KeyEnableAppSwitcherCheck, true);
			settingsDefault.Add(KeyForceAppFolderInstall, true);
			settingsDefault.Add(KeyEnableLogging, true);
			settingsDefault.Add(KeyLogDirectoryOSX, "~/Documents");
			settingsDefault.Add(KeyLogDirectoryWin, "");
			settingsDefault.Add(KeyAllowWLAN, false);

			// Default settings for group "Inside SEB"
			settingsDefault.Add(KeyInsideSebEnableSwitchUser, false);
			settingsDefault.Add(KeyInsideSebEnableLockThisComputer, false);
			settingsDefault.Add(KeyInsideSebEnableChangeAPassword, false);
			settingsDefault.Add(KeyInsideSebEnableStartTaskManager, false);
			settingsDefault.Add(KeyInsideSebEnableLogOff, false);
			settingsDefault.Add(KeyInsideSebEnableShutDown, false);
			settingsDefault.Add(KeyInsideSebEnableEaseOfAccess, false);
			settingsDefault.Add(KeyInsideSebEnableVmWareClientShade, false);

			// Default settings for group "Hooked Keys"
			settingsDefault.Add(KeyHookKeys, true);

			// Default settings for group "Special Keys"
			settingsDefault.Add(KeyEnableEsc, false);
			settingsDefault.Add(KeyEnableCtrlEsc, false);
			settingsDefault.Add(KeyEnableAltEsc, false);
			settingsDefault.Add(KeyEnableAltTab, true);
			settingsDefault.Add(KeyEnableAltF4, false);
			settingsDefault.Add(KeyEnableStartMenu, false);
			settingsDefault.Add(KeyEnableRightMouse, false);
			settingsDefault.Add(KeyEnablePrintScreen, false);
			settingsDefault.Add(KeyEnableAltMouseWheel, false);

			// Default settings for group "Function Keys"
			settingsDefault.Add(KeyEnableF1, false);
			settingsDefault.Add(KeyEnableF2, false);
			settingsDefault.Add(KeyEnableF3, false);
			settingsDefault.Add(KeyEnableF4, false);
			settingsDefault.Add(KeyEnableF5, true);
			settingsDefault.Add(KeyEnableF6, false);
			settingsDefault.Add(KeyEnableF7, false);
			settingsDefault.Add(KeyEnableF8, false);
			settingsDefault.Add(KeyEnableF9, false);
			settingsDefault.Add(KeyEnableF10, false);
			settingsDefault.Add(KeyEnableF11, false);
			settingsDefault.Add(KeyEnableF12, false);


			// Clear all "current" lists and dictionaries

			permittedProcessIndex = -1;
			permittedProcessList.Clear();
			permittedProcessData.Clear();

			permittedArgumentIndex = -1;
			permittedArgumentList.Clear();
			permittedArgumentData.Clear();

			prohibitedProcessIndex = -1;
			prohibitedProcessList.Clear();
			prohibitedProcessData.Clear();

			urlFilterRuleIndex = -1;
			urlFilterRuleList.Clear();
			urlFilterRuleData.Clear();

			urlFilterActionIndex = -1;
			urlFilterActionList.Clear();
			urlFilterActionData.Clear();

			embeddedCertificateIndex = -1;
			embeddedCertificateList.Clear();
			embeddedCertificateData.Clear();

			proxyProtocolIndex = -1;
			proxiesData.Clear();

			bypassedProxyIndex = -1;
			bypassedProxyList.Clear();
			bypassedProxyData = "";
		}

		// *****************************************
		// Restore default settings and new settings
		// *****************************************
		public void RestoreDefaultAndCurrentSettings()
		{
			// Set all the default values for the Plist structure "settingsCurrent"

			// Create a default Dictionary "settingsDefault".
			// Create a current Dictionary "settingsCurrent".
			// Fill up new settings by default settings, where necessary.
			// This assures that every (key, value) pair is contained
			// in the "default" and "current" dictionaries,
			// even if the loaded "current" dictionary did NOT contain every pair.

			CreateDefaultAndCurrentSettingsFromScratch();
			settingsCurrent.Clear();
			FillSettingsDictionary();
			FillSettingsArrays();
		}



		// ********************
		// Copy settings arrays
		// ********************
		public void FillSettingsArrays()
		{
			// Set all array values to default values
			for(int value = 1; value <= ValNum; value++)
			{
				intArrayCurrent[value] = intArrayDefault[value];
				strArrayCurrent[value] = strArrayDefault[value];
			}
			return;
		}

		// ************************
		// Fill settings dictionary
		// ************************
		public void FillSettingsDictionary()
		{

			// Add potentially missing keys to current Main Dictionary
			foreach(KeyValuePair<string, object> p in settingsDefault)
				if(settingsCurrent.ContainsKey(p.Key) == false)
				{
					// Key is missing: Add the default value
					settingsCurrent.Add(p.Key, p.Value);
				}
				else
				{
					// Key exists in new settings: Check if it has the correct object type
					object value = settingsCurrent[p.Key];
					object defaultValueObject = p.Value;
					if(!value.GetType().Equals(defaultValueObject.GetType()))
					{
						// The object type is not correct: Replace the object with the default value object
						settingsCurrent[p.Key] = defaultValueObject;
					}
				}

			// Get the Permitted Process List
			permittedProcessList = (List<object>)settingsCurrent[KeyPermittedProcesses];

			// Traverse Permitted Processes of currently opened file
			for(int listIndex = 0; listIndex < permittedProcessList.Count; listIndex++)
			{
				// Get the Permitted Process Data
				permittedProcessData = (Dictionary<string, object>)permittedProcessList[listIndex];

				// Add potentially missing keys to current Process Dictionary
				foreach(KeyValuePair<string, object> p in permittedProcessDataDefault)
					if(permittedProcessData.ContainsKey(p.Key) == false)
						permittedProcessData.Add(p.Key, p.Value);

				// Get the Permitted Argument List
				permittedArgumentList = (List<object>)permittedProcessData[KeyArguments];

				// Traverse Arguments of current Process
				for(int sublistIndex = 0; sublistIndex < permittedArgumentList.Count; sublistIndex++)
				{
					// Get the Permitted Argument Data
					permittedArgumentData = (Dictionary<string, object>)permittedArgumentList[sublistIndex];

					// Add potentially missing keys to current Argument Dictionary
					foreach(var p in permittedArgumentDataDefault)
					{
						if(!permittedArgumentData.ContainsKey(p.Key) == false && !string.Empty.Equals(p.Value) && p.Value != null)
						{
							permittedArgumentData.Add(p.Key, p.Value);
						}
					}
				} // next sublistIndex
			} // next listIndex



			// Get the Prohibited Process List
			prohibitedProcessList = (List<object>)settingsCurrent[KeyProhibitedProcesses];

			// Traverse Prohibited Processes of currently opened file
			for(int listIndex = 0; listIndex < prohibitedProcessList.Count; listIndex++)
			{
				// Get the Prohibited Process Data
				prohibitedProcessData = (Dictionary<string, object>)prohibitedProcessList[listIndex];

				// Add potentially missing keys to current Process Dictionary
				foreach(KeyValuePair<string, object> p in prohibitedProcessDataDefault)
					if(prohibitedProcessData.ContainsKey(p.Key) == false)
						prohibitedProcessData.Add(p.Key, p.Value);

			} // next listIndex



			// Get the Embedded Certificate List
			embeddedCertificateList = (List<object>)settingsCurrent[KeyEmbeddedCertificates];

			// Traverse Embedded Certificates of currently opened file
			for(int listIndex = 0; listIndex < embeddedCertificateList.Count; listIndex++)
			{
				// Get the Embedded Certificate Data
				embeddedCertificateData = (Dictionary<string, object>)embeddedCertificateList[listIndex];

				// Add potentially missing keys to current Certificate Dictionary
				foreach(KeyValuePair<string, object> p in embeddedCertificateDataDefault)
					if(embeddedCertificateData.ContainsKey(p.Key) == false)
						embeddedCertificateData.Add(p.Key, p.Value);

			} // next listIndex



			//// Get the URL Filter Rule List
			//SEBSettings.urlFilterRuleList = (ListObj)SEBSettings.settingsCurrent[SEBSettings.KeyURLFilterRules];

			//// Traverse URL Filter Rules of currently opened file
			//for (int listIndex = 0; listIndex < SEBSettings.urlFilterRuleList.Count; listIndex++)
			//{
			//    // Get the URL Filter Rule Data
			//    SEBSettings.urlFilterRuleData = (DictObj)SEBSettings.urlFilterRuleList[listIndex];

			//    // Add potentially missing keys to current Rule Dictionary
			//    foreach (KeyValue p in SEBSettings.urlFilterRuleDataDefault)
			//                       if (SEBSettings.urlFilterRuleData.ContainsKey(p.Key) == false)
			//                           SEBSettings.urlFilterRuleData.Add        (p.Key, p.Value);

			//    // Get the URL Filter Action List
			//    SEBSettings.urlFilterActionList = (ListObj)SEBSettings.urlFilterRuleData[SEBSettings.KeyRuleActions];

			//    // Traverse Actions of current Rule
			//    for (int sublistIndex = 0; sublistIndex < SEBSettings.urlFilterActionList.Count; sublistIndex++)
			//    {
			//        // Get the URL Filter Action Data
			//        SEBSettings.urlFilterActionData = (DictObj)SEBSettings.urlFilterActionList[sublistIndex];

			//        // Add potentially missing keys to current Action Dictionary
			//        foreach (KeyValue p in SEBSettings.urlFilterActionDataDefault)
			//                           if (SEBSettings.urlFilterActionData.ContainsKey(p.Key) == false)
			//                               SEBSettings.urlFilterActionData.Add        (p.Key, p.Value);

			//    } // next sublistIndex
			//} // next listIndex



			// Get the Proxies Dictionary
			proxiesData = (Dictionary<string, object>)settingsCurrent[KeyProxies];

			// Add potentially missing keys to current Proxies Dictionary
			foreach(KeyValuePair<string, object> p in proxiesDataDefault)
				if(proxiesData.ContainsKey(p.Key) == false)
					proxiesData.Add(p.Key, p.Value);

			// Get the Bypassed Proxy List
			bypassedProxyList = (List<object>)proxiesData[KeyExceptionsList];

			// Traverse Bypassed Proxies of currently opened file
			for(int listIndex = 0; listIndex < bypassedProxyList.Count; listIndex++)
			{
				if((String)bypassedProxyList[listIndex] == "")
					bypassedProxyList[listIndex] = bypassedProxyDataDefault;
			} // next listIndex


			return;
		}


		/// ----------------------------------------------------------------------------------------
		/// <summary>
		/// Return a settings dictionary with removed empty ListObj and DictObj elements 
		/// </summary>
		/// ----------------------------------------------------------------------------------------
		public Dictionary<string, object> CleanSettingsDictionary()
		{
			Dictionary<string, object> cleanSettings = new Dictionary<string, object>();

			// Add key/values to the clear dictionary if they're not an empty array (ListObj) or empty dictionary (DictObj)
			foreach(KeyValuePair<string, object> p in settingsDefault)
				if(!(p.Value is List<object> && ((List<object>)p.Value).Count == 0) && !(p.Value is Dictionary<string, object> && ((Dictionary<string, object>)p.Value).Count == 0))
					cleanSettings.Add(p.Key, p.Value);


			// Get the Permitted Process List
			List<object> permittedProcessList = (List<object>)valueForDictionaryKey(cleanSettings, KeyPermittedProcesses);
			if(permittedProcessList != null)
			{
				// Traverse Permitted Processes of currently opened file
				for(int listIndex = 0; listIndex < permittedProcessList.Count; listIndex++)
				{
					// Get the Permitted Process Data
					Dictionary<string, object> permittedProcessData = (Dictionary<string, object>)permittedProcessList[listIndex];
					if(permittedProcessData != null)
					{
						// Add potentially missing keys to current Process Dictionary
						foreach(KeyValuePair<string, object> p in permittedProcessDataDefault)
							if(permittedProcessData.ContainsKey(p.Key) == false && !(p.Value is List<object> && ((List<object>)p.Value).Count == 0) && !(p.Value is Dictionary<string, object> && ((Dictionary<string, object>)p.Value).Count == 0))
								permittedProcessData.Add(p.Key, p.Value);

						// Get the Permitted Argument List
						var permittedArgumentList = (List<object>)valueForDictionaryKey(permittedProcessData, KeyArguments);
						if(permittedArgumentList != null)
						{
							// Traverse Arguments of current Process
							for(var sublistIndex = 0; sublistIndex < permittedArgumentList.Count; sublistIndex++)
							{
								// Get the Permitted Argument Data
								var permittedArgumentData = (Dictionary<string, object>)permittedArgumentList[sublistIndex];

								// Add potentially missing keys to current Argument Dictionary
								foreach(var p in permittedArgumentDataDefault)
								{
									if(permittedArgumentData.ContainsKey(p.Key) == false && !string.Empty.Equals(p.Value) && p.Value != null)
									{
										permittedArgumentData.Add(p.Key, p.Value);
									}
								}

							} // next sublistIndex
						}
					}
				} // next listIndex
			}

			// Get the Prohibited Process List
			List<object> prohibitedProcessList = (List<object>)valueForDictionaryKey(cleanSettings, KeyProhibitedProcesses);
			if(prohibitedProcessList != null)
			{
				// Traverse Prohibited Processes of currently opened file
				for(int listIndex = 0; listIndex < prohibitedProcessList.Count; listIndex++)
				{
					// Get the Prohibited Process Data
					Dictionary<string, object> prohibitedProcessData = (Dictionary<string, object>)prohibitedProcessList[listIndex];

					// Add potentially missing keys to current Process Dictionary
					foreach(KeyValuePair<string, object> p in prohibitedProcessDataDefault)
						if(!(p.Value is List<object> && ((List<object>)p.Value).Count == 0) && !(p.Value is Dictionary<string, object> && ((Dictionary<string, object>)p.Value).Count == 0))
							prohibitedProcessData.Add(p.Key, p.Value);

				} // next listIndex
			}

			// Get the Embedded Certificate List
			List<object> embeddedCertificateList = (List<object>)valueForDictionaryKey(cleanSettings, KeyEmbeddedCertificates);
			if(embeddedCertificateList != null)
			{
				// Traverse Embedded Certificates of currently opened file
				for(int listIndex = 0; listIndex < embeddedCertificateList.Count; listIndex++)
				{
					// Get the Embedded Certificate Data
					Dictionary<string, object> embeddedCertificateData = (Dictionary<string, object>)embeddedCertificateList[listIndex];

					// Add potentially missing keys to current Certificate Dictionary
					foreach(KeyValuePair<string, object> p in embeddedCertificateDataDefault)
						if(!(p.Value is List<object> && ((List<object>)p.Value).Count == 0) && !(p.Value is Dictionary<string, object> && ((Dictionary<string, object>)p.Value).Count == 0))
							embeddedCertificateData.Add(p.Key, p.Value);

				} // next listIndex
			}

			//// Get the URL Filter Rule List
			//ListObj urlFilterRuleList = (ListObj)valueForDictionaryKey(cleanSettings, SEBSettings.KeyURLFilterRules);
			//if (urlFilterRuleList != null)
			//{
			//    // Traverse URL Filter Rules of currently opened file
			//    for (int listIndex = 0; listIndex < urlFilterRuleList.Count; listIndex++)
			//    {
			//        // Get the URL Filter Rule Data
			//        DictObj urlFilterRuleData = (DictObj)urlFilterRuleList[listIndex];

			//        // Add potentially missing keys to current Rule Dictionary
			//        foreach (KeyValue p in urlFilterRuleDataDefault)
			//            if (!(p.Value is ListObj && ((ListObj)p.Value).Count == 0) && !(p.Value is DictObj && ((DictObj)p.Value).Count == 0))
			//                urlFilterRuleData.Add(p.Key, p.Value);

			//        // Get the URL Filter Action List
			//        ListObj urlFilterActionList = (ListObj)valueForDictionaryKey(urlFilterRuleData, SEBSettings.KeyRuleActions);
			//        if (urlFilterActionList != null)
			//        {
			//            // Traverse Actions of current Rule
			//            for (int sublistIndex = 0; sublistIndex < urlFilterActionList.Count; sublistIndex++)
			//            {
			//                // Get the URL Filter Action Data
			//                DictObj urlFilterActionData = (DictObj)urlFilterActionList[sublistIndex];

			//                // Add potentially missing keys to current Action Dictionary
			//                foreach (KeyValue p in urlFilterActionDataDefault)
			//                    if (!(p.Value is ListObj && ((ListObj)p.Value).Count == 0) && !(p.Value is DictObj && ((DictObj)p.Value).Count == 0))
			//                        urlFilterActionData.Add(p.Key, p.Value);

			//            } // next sublistIndex
			//        }
			//    } // next listIndex
			//}

			// Get the Proxies Dictionary
			Dictionary<string, object> proxiesData = (Dictionary<string, object>)valueForDictionaryKey(cleanSettings, KeyProxies);
			if(proxiesData != null)
			{
				// Add potentially missing keys to current Proxies Dictionary
				foreach(KeyValuePair<string, object> p in proxiesDataDefault)
					if(proxiesData.ContainsKey(p.Key) == false && !(p.Value is List<object> && ((List<object>)p.Value).Count == 0) && !(p.Value is Dictionary<string, object> && ((Dictionary<string, object>)p.Value).Count == 0))
						proxiesData.Add(p.Key, p.Value);

				// Get the Bypassed Proxy List
				List<object> bypassedProxyList = (List<object>)valueForDictionaryKey(proxiesData, KeyExceptionsList);
				if(bypassedProxyList != null)
				{
					if(bypassedProxyList.Count == 0)
					{
						//proxiesData.Remove(SEBSettings.KeyExceptionsList);
					}
					else
					{
						// Traverse Bypassed Proxies of currently opened file
						for(int listIndex = 0; listIndex < bypassedProxyList.Count; listIndex++)
						{
							if((String)bypassedProxyList[listIndex] == "")
								bypassedProxyList[listIndex] = bypassedProxyDataDefault;
						} // next listIndex
					}
				}
			}

			return cleanSettings;
		}


		/// ----------------------------------------------------------------------------------------
		/// <summary>
		/// Read the value for a key from a dictionary and 
		/// return null for the value if the key doesn't exist 
		/// </summary>
		/// ----------------------------------------------------------------------------------------
		public object valueForDictionaryKey(Dictionary<string, object> dictionary, string key, object defaultValue = null)
		{
			if(dictionary.ContainsKey(key))
			{
				return dictionary[key];
			}
			else
			{
				return defaultValue;
			}
		}


		/// ----------------------------------------------------------------------------------------
		/// <summary>
		/// Clone a dictionary 
		/// </summary>
		/// ----------------------------------------------------------------------------------------
		public Dictionary<TKey, TValue> CloneDictionaryCloningValues<TKey, TValue>(Dictionary<TKey, TValue> original) where TValue: ICloneable
		{
			Dictionary<TKey, TValue> ret = new Dictionary<TKey, TValue>(original.Count, original.Comparer);
			foreach(KeyValuePair<TKey, TValue> entry in original)
			{
				ret.Add(entry.Key, (TValue)entry.Value.Clone());
			}
			return ret;
		}

		// **********************************************
		// Add XulRunnerProcess to Permitted Process List
		// **********************************************
		public void PermitXulRunnerProcess()
		{
			// Get the Permitted Process List
			permittedProcessList = (List<object>)settingsCurrent[KeyPermittedProcesses];

			// Position of XulRunner process in Permitted Process List
			int indexOfProcessXulRunnerExe = -1;

			// Traverse Permitted Processes of currently opened file
			for(int listIndex = 0; listIndex < permittedProcessList.Count; listIndex++)
			{
				permittedProcessData = (Dictionary<string, object>)permittedProcessList[listIndex];

				// Check if XulRunner process is in Permitted Process List
				if(permittedProcessData[KeyExecutable].Equals(SebConstants.XUL_RUNNER))
					indexOfProcessXulRunnerExe = listIndex;

			} // next listIndex

			// If XulRunner process was not in Permitted Process List, add it
			if(indexOfProcessXulRunnerExe == -1)
			{
				permittedProcessList.Add(permittedProcessDataXulRunner);
			}
			// Assure that XulRunner process has correct settings:
			// Remove XulRunner process from Permitted Process List.
			// Add    XulRunner process to   Permitted Process List.
			else
			{
				//SEBSettings.permittedProcessList.RemoveAt(indexOfProcessXulRunnerExe);
				//SEBSettings.permittedProcessList.Insert  (indexOfProcessXulRunnerExe, SEBSettings.permittedProcessDataXulRunner);
			}

			return;
		}

		// **************
		// Print settings
		// **************
		public void PrintSettingsRecursively(object objectSource, StreamWriter fileWriter, String indenting)
		{

			// Determine the type of the input object
			string typeSource = objectSource.GetType().ToString();


			// Treat the complex datatype Dictionary<string, object>
			if(typeSource.Contains("Dictionary"))
			{
				Dictionary<string, object> dictSource = (Dictionary<string, object>)objectSource;

				//foreach (KeyValue pair in dictSource)
				for(int index = 0; index < dictSource.Count; index++)
				{
					KeyValuePair<string, object> pair = dictSource.ElementAt(index);
					string key = pair.Key;
					object value = pair.Value;
					string type = pair.Value.GetType().ToString();

					// Print one (key, value) pair of dictionary
					fileWriter.WriteLine(indenting + key + "=" + value);

					if(type.Contains("Dictionary") || type.Contains("List"))
					{
						object childSource = dictSource[key];
						PrintSettingsRecursively(childSource, fileWriter, indenting + "   ");
					}

				} // next (KeyValue pair in dictSource)
			} // end if (typeSource.Contains("Dictionary"))


			// Treat the complex datatype List<object>
			if(typeSource.Contains("List"))
			{
				List<object> listSource = (List<object>)objectSource;

				//foreach (object elem in listSource)
				for(int index = 0; index < listSource.Count; index++)
				{
					object elem = listSource[index];
					string type = elem.GetType().ToString();

					// Print one element of list
					fileWriter.WriteLine(indenting + elem);

					if(type.Contains("Dictionary") || type.Contains("List"))
					{
						object childSource = listSource[index];
						PrintSettingsRecursively(childSource, fileWriter, indenting + "   ");
					}

				} // next (element in listSource)
			} // end if (typeSource.Contains("List"))

			return;
		}

		// *************************
		// Print settings dictionary
		// *************************
		public void LoggSettingsDictionary(ref Dictionary<string, object> sebSettings, String fileName)
		{
			FileStream fileStream;
			StreamWriter fileWriter;

			// If the .ini file already exists, delete it
			// and write it again from scratch with new data
			if(File.Exists(fileName))
				File.Delete(fileName);

			// Open the file for writing
			fileStream = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write);
			fileWriter = new StreamWriter(fileStream);

			// Write the header lines
			fileWriter.WriteLine("");
			fileWriter.WriteLine("number of (key, value) pairs = " + sebSettings.Count);
			fileWriter.WriteLine("");

			// Call the recursive method for printing the contents
			PrintSettingsRecursively(sebSettings, fileWriter, "");

			// Close the file
			fileWriter.Close();
			fileStream.Close();
			return;
		}


		/// ----------------------------------------------------------------------------------------
		/// <summary>
		/// Decrypt, deserialize and store new settings as current SEB settings 
		/// </summary>
		/// ----------------------------------------------------------------------------------------
		public bool LoadEncryptedSettings(byte[] sebSettings, GetPasswordMethod getPassword)
		{
			Dictionary<string, object> settingsDict = null;
			// If we were passed empty settings, we skip decrypting and just use default settings
			if(sebSettings != null)
			{
				string filePassword = null;
				bool passwordIsHash = false;
				X509Certificate2 fileCertificateRef = null;

				try
				{
					// Decrypt the configuration settings.
					// Convert the XML structure into a C# dictionary object.
					settingsDict = SebConfigFileManager.DecryptSEBSettings(sebSettings, getPassword, false, ref filePassword, ref passwordIsHash, ref fileCertificateRef);
					if(settingsDict == null)
					{
						Logger.AddError("The .seb file could not be decrypted. ", null, null, "");
						return false;
					}
				}
				catch(Exception streamReadException)
				{
					// Let the user know what went wrong
					Logger.AddError("The .seb file could not be decrypted. ", null, streamReadException, streamReadException.Message);
					return false;
				}
			}
			// Store the new settings or use defaults if new settings were empty
			StoreSebClientSettings(settingsDict);
			return true;
		}

		/// ----------------------------------------------------------------------------------------
		/// <summary>
		/// Store passed new settings as current SEB settings 
		/// or use default settings if none were passed.
		/// </summary>
		/// ----------------------------------------------------------------------------------------
		public void StoreSebClientSettings(Dictionary<string, object> settingsDict)
		{
			// Recreate the default and current settings dictionaries
			CreateDefaultAndCurrentSettingsFromScratch();
			settingsCurrent.Clear();

			// If we got new settings, we use them (othervise use defaults)
			if(settingsDict != null) settingsCurrent = settingsDict;

			// Fill up the Dictionary read from file with default settings, where necessary
			FillSettingsDictionary();
			FillSettingsArrays();

			// Add the XulRunner process to the Permitted Process List, if necessary
			PermitXulRunnerProcess();
		}

		// *********************************************
		// Read the settings from the configuration file
		// *********************************************
		public bool ReadSebConfigurationFile(String fileName, GetPasswordMethod getPassword, bool forEditing, ref string filePassword, ref bool passwordIsHash, ref X509Certificate2 fileCertificateRef)
		{
			var newSettings = new Dictionary<string, object>();
			try
			{
				// Read the configuration settings from .seb file.
				// Decrypt the configuration settings.
				// Convert the XML structure into a C# object.

				byte[] encryptedSettings = File.ReadAllBytes(fileName);

				newSettings = SebConfigFileManager.DecryptSEBSettings(encryptedSettings, getPassword, forEditing, ref filePassword, ref passwordIsHash, ref fileCertificateRef);
				if(newSettings == null) return false;
			}
			catch(Exception streamReadException)
			{
				// Let the user know what went wrong
				Logger.AddError("The .seb file could not be read: ", null, streamReadException, streamReadException.Message);
				return false;
			}

			// If the settings could be read from file...
			// Recreate the default and current settings
			CreateDefaultAndCurrentSettingsFromScratch();
			settingsCurrent.Clear();
			settingsCurrent = newSettings;

			// Fill up the Dictionary read from file with default settings, where necessary
			//SEBSettings.LoggSettingsDictionary(ref SEBSettings.settingsDefault, "DebugSettingsDefaultInReadSebConfigurationFileFillBefore.txt");
			//SEBSettings.LoggSettingsDictionary(ref SEBSettings.settingsCurrent, "DebugSettingsCurrentInReadSebConfigurationFileFillBefore.txt");
			FillSettingsDictionary();
			FillSettingsArrays();
			//SEBSettings.LoggSettingsDictionary(ref SEBSettings.settingsDefault, "DebugSettingsDefaultInReadSebConfigurationFileFillAfter.txt");
			//SEBSettings.LoggSettingsDictionary(ref SEBSettings.settingsCurrent, "DebugSettingsCurrentInReadSebConfigurationFileFillAfter.txt");

			// Add the XulRunner process to the Permitted Process List, if necessary
			//SEBSettings.LoggSettingsDictionary(ref SEBSettings.settingsDefault, "DebugSettingsDefaultInReadSebConfigurationFilePermitBefore.txt");
			//SEBSettings.LoggSettingsDictionary(ref SEBSettings.settingsCurrent, "DebugSettingsCurrentInReadSebConfigurationFilePermitBefore.txt");
			PermitXulRunnerProcess();
			//SEBSettings.LoggSettingsDictionary(ref SEBSettings.settingsDefault, "DebugSettingsDefaultInReadSebConfigurationFilePermitAfter.txt");
			//SEBSettings.LoggSettingsDictionary(ref SEBSettings.settingsCurrent, "DebugSettingsCurrentInReadSebConfigurationFilePermitAfter.txt");

			//SEBSettings.LoggSettingsDictionary(ref SEBSettings.settingsDefault, "DebugSettingsDefaultInReadSebConfigurationFile.txt");
			//SEBSettings.LoggSettingsDictionary(ref SEBSettings.settingsCurrent, "DebugSettingsCurrentInReadSebConfigurationFile.txt");

			return true;
		}

		// ********************************************************
		// Write the settings to the configuration file and save it
		// ********************************************************
		public bool WriteSebConfigurationFile(SebSettings settings, String fileName, string filePassword, bool passwordIsHash, X509Certificate2 fileCertificateRef, sebConfigPurposes configPurpose, bool forEditing = false)
		{
			try
			{
				// Convert the C# settings dictionary object into an XML structure.
				// Encrypt the configuration settings depending on passed credentials
				// Write the configuration settings into .seb file.

				byte[] encryptedSettings = SebConfigFileManager.EncryptSEBSettingsWithCredentials(settings, filePassword, passwordIsHash, fileCertificateRef, configPurpose, forEditing);
				File.WriteAllBytes(fileName, encryptedSettings);
			}
			catch(Exception streamWriteException)
			{
				// Let the user know what went wrong
				Logger.AddError("The configuration file could not be written: ", null, streamWriteException, streamWriteException.Message);
				return false;
			}
			return true;
		}
	}
}
