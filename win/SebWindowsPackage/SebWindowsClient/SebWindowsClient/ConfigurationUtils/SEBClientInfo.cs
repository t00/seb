//
//  SEBClientInfo.cs
//  SafeExamBrowser
//
//  Copyright (c) 2010-2014 Viktor Tomas, Dirk Bauer, Daniel R. Schneider, Pascal Wyss,
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
using System.Text;
using System.IO;
using System.Windows.Forms;
using SebShared;
using SebShared.DiagnosticUtils;
using SebWindowsClient.DesktopUtils;
using System.Collections;

namespace SebWindowsClient.ConfigurationUtils
{
	public class SEBClientInfo
	{
		public static bool ExplorerShellWasKilled { get; set; }
		public static bool SupportsMultipleDesktops { get; set; }
		public static bool HasExamStarted = false;

		// SEB Client Socket properties
		public static string UserName { get; set; }

		public static SEBDesktopController OriginalDesktop { get; set; }
		public static SEBDesktopController SEBNewlDesktop { get; set; }
		public static string DesktopName { get; set; }

		// SEB Client Directories properties
		public static string ApplicationExecutableDirectory { get; set; }
		public static string ProgramFilesX86Directory { get; set; }
		public static bool LogFileDesiredMsgHook { get; set; }
		public static bool LogFileDesiredSebClient { get; set; }
		public static string SebClientLogFileDirectory { get; set; }
		public static string SebClientDirectory { get; set; }
		public static string SebClientLogFile { get; set; }
		public static string SebClientSettingsProgramDataDirectory { get; set; }
		public static string SebClientSettingsAppDataDirectory { get; set; }
		public static string XulRunnerDirectory { get; set; }
		public static string XulSebDirectory { get; set; }
		public static string SebClientSettingsProgramDataFile;
		public static string SebClientSettingsAppDataFile;
		public static string XulRunnerConfigFileDirectory { get; set; }
		public static string XulRunnerConfigFile;
		public static string XulRunnerExePath;
		public static string XulRunnerSebIniPath;
		public static string XulRunnerParameter;
		//public static string XulRunnerFlashContainerState { get; set; }

		public static string ExamUrl { get; set; }
		public static string QuitPassword { get; set; }
		public static string QuitHashcode { get; set; }

		//public static Dictionary<string, object> sebSettings = new Dictionary<string, object>();

		public static SebWindowsClientForm SebWindowsClientForm;

		public static string LoadingSettingsFileName = "";

		public static float scaleFactor = 1;
		public static int appChooserHeight = 132;

		/// <summary>
		/// Sets user, host info, send-recv interval, recv timeout, Logger and read SebClient configuration.
		/// </summary>
		/// <returns></returns>
		public static bool SetSebClientConfiguration()
		{
			// Initialise socket properties
			ExplorerShellWasKilled = false;
			UserName = Environment.UserName;

			// Initialise error messages
			//SEBMessageBox.SetCurrentLanguage();
			//SEBMessageBox.InitErrorMessages();
			//SEBSettings     .CreateDefaultAndCurrentSettingsFromScratch();

			//Sets paths to files SEB has to save or read from the file system
			SetSebPaths();

			// Write settings into log
			Logger.AddInformation("User Name: " + UserName);
			Logger.AddInformation("SebClientConfigFileDirectory: " + SebClientSettingsAppDataDirectory);
			Logger.AddInformation("SebClientConfigFile: " + SebClientSettingsAppDataFile);
			return true;
		}

		/// <summary>
		/// Initialise Logger if it's enabled.
		/// </summary>
		public static void InitializeLogger()
		{
			if(SebInstance.Settings.Get<bool>(SebSettings.KeyEnableLogging))
			{
				var logDirectory = SebInstance.Settings.Get<string>(SebSettings.KeyLogDirectoryWin);
				if(!String.IsNullOrEmpty(logDirectory))
				{
					// Expand environment variables in log file path
					SebClientLogFileDirectory = Environment.ExpandEnvironmentVariables(logDirectory);
					SebClientLogFile = String.Format(@"{0}\{1}", SebClientLogFileDirectory, SebConstants.SEB_CLIENT_LOG);
				}
				else
				{
					SetDefaultClientLogFile();
				}
				Logger.InitLogger(SebClientLogFileDirectory, SebClientLogFile);
			}
		}

		/// <summary>
		/// Sets paths to files SEB has to save or read from the file system.
		/// </summary>
		public static void SetSebPaths()
		{
			// Get the path of the directory the application executable lies in
			ApplicationExecutableDirectory = Path.GetDirectoryName(Application.ExecutablePath);

			// Get the path of the "Program Files X86" directory.
			ProgramFilesX86Directory = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);

			// Get the path of the "Program Data" and "Local Application Data" directory.
			string programDataDirectory = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData); //GetEnvironmentVariable("PROGRAMMDATA");
			string appDataDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

			/// Get paths for the two possible locations of the SebClientSettings.seb file
			/// 
			// In the program data directory (for managed systems, only an administrator can write in this directory):
			// If there is a SebClientSettigs.seb file, then this has priority and is used by the SEB client, another
			// SebClientSettigs.seb file in the local app data folder is ignored then and the SEB client cannot be 
			// reconfigured by opening a .seb file saved for configuring a client
			StringBuilder sebClientSettingsProgramDataDirectoryBuilder = new StringBuilder(programDataDirectory).Append("\\").Append(SebConstants.MANUFACTURER_LOCAL).Append("\\"); //.Append(PRODUCT_NAME).Append("\\");
			SebClientSettingsProgramDataDirectory = sebClientSettingsProgramDataDirectoryBuilder.ToString();

			// In the local application data directory (for unmanaged systems like student computers, user can write in this directory):
			// A SebClientSettigs.seb file in this directory can be created or replaced by opening a .seb file saved for configuring a client
			StringBuilder sebClientSettingsAppDataDirectoryBuilder = new StringBuilder(appDataDirectory).Append("\\").Append(SebConstants.MANUFACTURER_LOCAL).Append("\\"); //.Append(PRODUCT_NAME).Append("\\");
			SebClientSettingsAppDataDirectory = sebClientSettingsAppDataDirectoryBuilder.ToString();

			// Set the location of the SebWindowsClientDirectory
			StringBuilder sebClientDirectoryBuilder = new StringBuilder(ProgramFilesX86Directory).Append("\\").Append(SebConstants.PRODUCT_NAME).Append("\\");
			SebClientDirectory = sebClientDirectoryBuilder.ToString();

			// Set the location of the XulRunnerDirectory
			//StringBuilder xulRunnerDirectoryBuilder = new StringBuilder(SebClientDirectory).Append(XUL_RUNNER_DIRECTORY).Append("\\");
			//XulRunnerDirectory = xulRunnerDirectoryBuilder.ToString();
			StringBuilder xulRunnerDirectoryBuilder = new StringBuilder(SebConstants.SEB_BROWSER_DIRECTORY).Append("\\").Append(SebConstants.XUL_RUNNER_DIRECTORY).Append("\\");
			XulRunnerDirectory = xulRunnerDirectoryBuilder.ToString();

			// Set the location of the XulSebDirectory
			//StringBuilder xulSebDirectoryBuilder = new StringBuilder(SebClientDirectory).Append(XUL_SEB_DIRECTORY).Append("\\");
			//XulSebDirectory = xulSebDirectoryBuilder.ToString();
			StringBuilder xulSebDirectoryBuilder = new StringBuilder(SebConstants.SEB_BROWSER_DIRECTORY).Append("\\").Append(SebConstants.XUL_SEB_DIRECTORY).Append("\\");
			XulSebDirectory = xulSebDirectoryBuilder.ToString();

			// Set the location of the XulRunnerExePath
			//StringBuilder xulRunnerExePathBuilder = new StringBuilder("\"").Append(XulRunnerDirectory).Append(XUL_RUNNER).Append("\"");
			//XulRunnerExePath = xulRunnerExePathBuilder.ToString();
			StringBuilder xulRunnerExePathBuilder = new StringBuilder(XulRunnerDirectory).Append(SebConstants.XUL_RUNNER); //.Append("\"");
			XulRunnerExePath = xulRunnerExePathBuilder.ToString();

			// Set the location of the seb.ini
			StringBuilder xulRunnerSebIniPathBuilder = new StringBuilder(XulSebDirectory).Append(SebConstants.XUL_RUNNER_INI); //.Append("\"");
			XulRunnerSebIniPath = xulRunnerSebIniPathBuilder.ToString();

			// Get the two possible paths of the SebClientSettings.seb file
			StringBuilder sebClientSettingsProgramDataBuilder = new StringBuilder(SebClientSettingsProgramDataDirectory).Append(SebConstants.SEB_CLIENT_CONFIG);
			SebClientSettingsProgramDataFile = sebClientSettingsProgramDataBuilder.ToString();

			StringBuilder sebClientSettingsAppDataBuilder = new StringBuilder(SebClientSettingsAppDataDirectory).Append(SebConstants.SEB_CLIENT_CONFIG);
			SebClientSettingsAppDataFile = sebClientSettingsAppDataBuilder.ToString();

			// Set the default location of the SebClientLogFileDirectory
			SetDefaultClientLogFile();
		}

		/// <summary>
		/// Set the default location of the SebClientLogFileDirectory.
		/// </summary>
		public static void SetDefaultClientLogFile()
		{
			StringBuilder SebClientLogFileDirectoryBuilder = new StringBuilder(SebClientSettingsAppDataDirectory); //.Append("\\").Append(MANUFACTURER_LOCAL).Append("\\");
			SebClientLogFileDirectory = SebClientLogFileDirectoryBuilder.ToString();

			// Set the path of the SebClient.log file
			StringBuilder sebClientLogFileBuilder = new StringBuilder(SebClientLogFileDirectory).Append(SebConstants.SEB_CLIENT_LOG);
			SebClientLogFile = sebClientLogFileBuilder.ToString();
		}

		/// <summary>
		/// Sets properties in config.json XULRunner configuration file.
		/// </summary>
		/// <returns></returns>
		public static bool SetXulRunnerConfiguration()
		{
			bool setXulRunnerConfiguration = false;
			try
			{
				// Get the path of the "Program Data" directory.
				string localAppDataDirectory = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
				//string programDataDirectory = Environment.GetEnvironmentVariable("PROGRAMMDATA");

				// Set the location of the XULRunnerConfigFileDirectory
				StringBuilder xulRunnerConfigFileDirectoryBuilder = new StringBuilder(localAppDataDirectory).Append("\\").Append(SebConstants.MANUFACTURER_LOCAL).Append("\\"); //.Append(PRODUCT_NAME).Append("\\");
				XulRunnerConfigFileDirectory = xulRunnerConfigFileDirectoryBuilder.ToString();

				// Set the location of the config.json file
				StringBuilder xulRunnerConfigFileBuilder = new StringBuilder(XulRunnerConfigFileDirectory).Append(SebConstants.XUL_RUNNER_CONFIG);
				XulRunnerConfigFile = xulRunnerConfigFileBuilder.ToString();

				XULRunnerConfig xulRunnerConfig = SEBXulRunnerSettings.XULRunnerConfigDeserialize(XulRunnerConfigFile);
				xulRunnerConfig.seb_openwin_width = Int32.Parse(SebInstance.Settings.Get<string>(SebSettings.KeyNewBrowserWindowByLinkWidth, true));
				xulRunnerConfig.seb_openwin_height = Int32.Parse(SebInstance.Settings.Get<string>(SebSettings.KeyNewBrowserWindowByLinkHeight, true));
				if(SebInstance.Settings.Get<int>(SebSettings.KeyBrowserViewMode) == (int)browserViewModes.browserViewModeWindow)
				{
					xulRunnerConfig.seb_mainWindow_titlebar_enabled = true;
				}
				else
				{
					xulRunnerConfig.seb_mainWindow_titlebar_enabled = false;

				}
				xulRunnerConfig.seb_url = SebInstance.Settings.Get<string>(SebSettings.KeyStartURL);
				setXulRunnerConfiguration = true;
				SEBXulRunnerSettings.XULRunnerConfigSerialize(xulRunnerConfig, XulRunnerConfigFile);
			}
			catch(Exception ex)
			{
				Logger.AddError("Error ocurred by setting XulRunner configuration.", null, ex, ex.Message);
			}

			return setXulRunnerConfiguration;
		}

		public static bool CreateNewDesktopOldValue { get; set; }

		public static string ContractEnvironmentVariables(string path)
		{
			path = Path.GetFullPath(path);
			DictionaryEntry currentEntry = new DictionaryEntry("", "");
			foreach(object key in Environment.GetEnvironmentVariables().Keys)
			{
				string value = (string)Environment.GetEnvironmentVariables()[key];
				if(path.ToUpperInvariant().Contains(value.ToUpperInvariant()) && value.Length > ((string)currentEntry.Value).Length)
				{
					currentEntry.Key = (string)key;
					currentEntry.Value = value;
				}
			}
			return path.Replace((string)currentEntry.Value, "%" + (string)currentEntry.Key + "%");
		}
	}
}
