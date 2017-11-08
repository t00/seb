// -------------------------------------------------------------
//     Viktor tomas
//     BFH-TI, http://www.ti.bfh.ch
//     Biel, 2012
// -------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Windows;
using SebShared.CryptographyUtils;
using SebShared.DiagnosticUtils;
using SebShared.Properties;
using SebShared.Utils;
using SebShared;
using SebWindowsClient.ConfigurationUtils;
using SebWindowsClient.CryptographyUtils;
using SebWindowsClient.ProcessUtils;
using SebWindowsClient.DesktopUtils;
using System.Diagnostics;
using System.Text;
using SebWindowsClient.Properties;
using SebWindowsClient.ServiceUtils;
using System.Runtime.InteropServices;
using System.Threading;
using SebWindowsClient.XULRunnerCommunication;
using Application = System.Windows.Forms.Application;

//
//  SebWindowsClient.cs
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

namespace SebWindowsClient
{
	static class SebWindowsClientMain
	{
		public static SingleInstanceController<SebWindowsClientForm> Instance;

		[DllImport("user32.dll", SetLastError = true)]
		static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

		[DllImport("user32.dll")]
		private static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

		[DllImport("user32.dll")]
		static extern IntPtr GetDesktopWindow();

		[DllImport("user32.dll")]
		private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X,
		   int Y, int cx, int cy, uint uFlags);

		[DllImport("user32.dll")]
		private static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		private static extern bool EnumThreadWindows(int threadId, EnumThreadProc pfnEnum, IntPtr lParam);

		[DllImport("user32.dll", SetLastError = true)]
		private static extern IntPtr FindWindowEx(IntPtr parentHandle, IntPtr childAfter, string className, string windowTitle);

		[DllImport("user32.dll")]
		private static extern IntPtr FindWindowEx(IntPtr parentHwnd, IntPtr childAfterHwnd, IntPtr className, string windowText);

		[DllImport("User32.dll")]
		private static extern bool IsIconic(IntPtr handle);

		[DllImport("user32.dll")]
		private static extern int ShowWindow(IntPtr hwnd, int nCmdShow);

		[DllImport("User32.dll")]
		public static extern IntPtr SetForegroundWindow(IntPtr hWnd);

		[DllImport("user32.dll")]
		private static extern uint GetWindowThreadProcessId(IntPtr hwnd, out int lpdwProcessId);

		//[System.Runtime.InteropServices.DllImport("User32")]
		//private static extern bool SetForegroundWindow(IntPtr hWnd);

		private const int SW_HIDE = 0;
		private const int SW_SHOW = 5;
		private const int SW_RESTORE = 9;

		private const string VistaStartMenuCaption = "Start";
		private static IntPtr vistaStartMenuWnd = IntPtr.Zero;
		private delegate bool EnumThreadProc(IntPtr hwnd, IntPtr lParam);

		public static bool sessionCreateNewDesktop;

		private static bool isLoadingSettings;
		private static readonly object LoadingFileLock = new object();

		public static bool clientSettingsSet { get; set; }

		public static SEBSplashScreen splash;

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		//[STAThread] //Do not use this, it breaks the ability to switch to a new desktop
		static void Main()
		{
			Instance = new SingleInstanceController<SebWindowsClientForm>(CreateMainForm, (mainForm, args) =>
			{
				if(CheckLoadSettingsCommandLine(args) == true)
				{
					SebWindowsClientForm.SEBToForeground();
					OpenExam(false, true);
				}
				else
				{
				    Logger.AddError("SingleInstanceController command line load failed", null, null);
				}
			});
			Instance.Run();
		}

		public static SebWindowsClientForm CreateMainForm(string[] arguments)
		{
			Application.SetCompatibleTextRenderingDefault(false);
			// If we are running in SebWindowsClient we need to activate it before showing a dialog
			SebMessageBox.BeforeShow += delegate
			{
				if(SEBClientInfo.SebWindowsClientForm != null)
				{
					SEBToForeground();
				}
			};

			Application.EnableVisualStyles();

			var showSplash = !arguments.Any();
			var areSettingsFromCommandLine = false;
			Logger.AddInformation("---------- INITIALIZING SEB - STARTING SESSION -------------");
			try
			{
				if(!CheckLoadClientInfo())
				{
					return null;
				}

				var loadedCommandLine = CheckLoadSettingsCommandLine(arguments);
				if(loadedCommandLine == null)
				{
					if(!LoadClientSettings())
					{
						return null;
					}
				}
				else if(loadedCommandLine.Value)
				{
					areSettingsFromCommandLine = true;
				}
				else
				{
					return null;
				}

				if(!CheckCreateDesktop())
				{
					return null;
				}
				if(!SEBXULRunnerWebSocketServer.CheckStartServer())
				{
					return null;
				}
			}
			catch(Exception ex)
			{
				Logger.AddError("Unable to InitSebSettings", null, ex);
				return null;
			}

			if(showSplash)
			{
				var splashThread = new Thread(StartSplash);
				splashThread.Start();
			}

			SEBClientInfo.CreateNewDesktopOldValue = SebInstance.Settings.Get<bool>(SebSettings.KeyCreateNewDesktop);

			SEBClientInfo.SebWindowsClientForm = new SebWindowsClientForm();
			try
			{
				OpenExam(true, areSettingsFromCommandLine);
			}
			catch(Exception)
			{
				Logger.AddInformation("Unable to InitSEBDesktop");
			}
			finally
			{
				if(showSplash)
				{
					CloseSplash();
				}
			}

			return SEBClientInfo.SebWindowsClientForm;
		}

		public static bool OpenExam(bool firstTime, bool askStartConfiguringClient)
		{
			if(!firstTime)
			{
				// Reset SEB, close third party applications
				SEBClientInfo.SebWindowsClientForm.closeSebClient = false;
				Logger.AddInformation("Attempting to CloseSEBForm for reconfiguration");
				SEBClientInfo.SebWindowsClientForm.CloseSEBForm();
				Logger.AddInformation("Succesfully CloseSEBForm for reconfiguration");
				SEBClientInfo.SebWindowsClientForm.closeSebClient = true;
			}

			if(SebInstance.Settings.Get<int>(SebSettings.KeySebConfigPurpose) == (int)SebSettings.sebConfigPurposes.sebConfigPurposeStartingExam)
			{
				//
				// If these SEB settings are ment to start an exam
				//

				Logger.AddInformation("Reconfiguring to start an exam");
				// If these SEB settings are ment to start an exam

				// Set the flag that SEB is running in exam mode now
				SEBClientInfo.HasExamStarted = true;

				//Re-initialize logger
				SEBClientInfo.InitializeLogger();

				// Check if SEB is running on the standard desktop and the new settings demand to run in new desktop (createNewDesktop = true)
				// or the other way around!
				if(SEBClientInfo.CreateNewDesktopOldValue != SebInstance.Settings.Get<bool>(SebSettings.KeyCreateNewDesktop))
				{
					// If it did, SEB needs to quit and be restarted manually for the new setting to take effekt
					if(SEBClientInfo.CreateNewDesktopOldValue == false)
					{
						SebMessageBox.Show(SEBUIStrings.settingsRequireNewDesktop, SEBUIStrings.settingsRequireNewDesktopReason, MessageBoxImage.Error, MessageBoxButton.OK);
					}
					else
					{
						SebMessageBox.Show(SEBUIStrings.settingsRequireNotNewDesktop, SEBUIStrings.settingsRequireNotNewDesktopReason, MessageBoxImage.Error, MessageBoxButton.OK);
					}

					//SEBClientInfo.SebWindowsClientForm.closeSebClient = true;
					SEBClientInfo.SebWindowsClientForm.ExitApplication();
				}

				// Re-Initialize SEB according to the new settings
				Logger.AddInformation("Attemting to InitSEBDesktop for reconfiguration");
				if(!InitSEBDesktop())
				{
					return false;
				}
				Logger.AddInformation("Sucessfully InitSEBDesktop for reconfiguration");
				// Re-open the main form
				//SEBClientInfo.SebWindowsClientForm = new SebWindowsClientForm();
				//SebWindowsClientMain.singleInstanceController.SetMainForm(SEBClientInfo.SebWindowsClientForm);

				//return if initializing SEB with openend preferences was successful
				Logger.AddInformation("Attempting to OpenSEBForm for reconfiguration");
				var ret = SEBClientInfo.SebWindowsClientForm.OpenSEBForm();
				Logger.AddInformation("Successfully OpenSEBForm for reconfiguration");
				return ret;
			}
			else
			{
				//
				// If these SEB settings are ment to configure a client
				//

				Logger.AddInformation("Reconfiguring to configure a client");
				// If these SEB settings are ment to configure a client

				// Check if we have embedded identities and import them into the Windows Certifcate Store
				var embeddedCertificates = SebInstance.Settings.Get<List<object>>(SebSettings.KeyEmbeddedCertificates);
				for(int i = embeddedCertificates.Count - 1; i >= 0; i--)
				{
					// Get the Embedded Certificate
					var embeddedCertificate = (IDictionary<string, object>)embeddedCertificates[i];
					// Is it an identity?
					if((int)embeddedCertificate[SebSettings.KeyType] == 1)
					{
						// Store the identity into the Windows Certificate Store
						SebProtectionController.StoreCertificateIntoStore((byte[])embeddedCertificate[SebSettings.KeyCertificateData]);
					}
					// Remove the identity from settings, as it should be only stored in the Certificate Store and not in the locally stored settings file
					embeddedCertificates.RemoveAt(i);
				}

				//Re-initialize logger
				SEBClientInfo.InitializeLogger();

				// Write new settings to the localapp directory
				if(SebInstance.Settings.Get<bool>(SebSettings.KeySebStoreConfig))
				{
					SebInstance.Settings.WriteSebConfigurationFile(SebInstance.Settings, SEBClientInfo.SebClientSettingsAppDataFile, "", false, null, SebSettings.sebConfigPurposes.sebConfigPurposeConfiguringClient);
				}

				// Re-Initialize SEB desktop according to the new settings
				if(!InitSEBDesktop())
				{
					return false;
				}

				if(SEBClientInfo.SebWindowsClientForm.OpenSEBForm())
				{
					// Activate SebWindowsClient so the message box gets focus
					//SEBClientInfo.SebWindowsClientForm.Activate();

					// Check if setting for createNewDesktop changed
					if(SEBClientInfo.CreateNewDesktopOldValue != SebInstance.Settings.Get<bool>(SebSettings.KeyCreateNewDesktop))
					{
						// If it did, SEB needs to quit and be restarted manually for the new setting to take effekt
						SebMessageBox.Show(SEBUIStrings.sebReconfiguredRestartNeeded, SEBUIStrings.sebReconfiguredRestartNeededReason, MessageBoxImage.Warning, MessageBoxButton.OK);
						//SEBClientInfo.SebWindowsClientForm.closeSebClient = true;
						SEBClientInfo.SebWindowsClientForm.ExitApplication();
					}

					if((SebConstants.SEB_ASK_START_COMMAND_LINE && askStartConfiguringClient && SebMessageBox.Show(SEBUIStrings.sebReconfigured, SEBUIStrings.sebReconfiguredQuestion, MessageBoxImage.Question, MessageBoxButton.YesNo) == MessageBoxResult.No))
					{
						//SEBClientInfo.SebWindowsClientForm.closeSebClient = true;
						SEBClientInfo.SebWindowsClientForm.ExitApplication();
					}

					return true; //reading preferences was successful
				}
				else
				{
					return false;
				}
			}
		}

		static public void StartSplash()
		{
			//Set the threads desktop to the new desktop if "Create new Desktop" is activated
			if(SebInstance.Settings.Get<bool>(SebSettings.KeyCreateNewDesktop))
				SEBDesktopController.SetCurrent(SEBClientInfo.SEBNewlDesktop);

			// Instance a splash form given the image names
			splash = new SEBSplashScreen();
			// Run the form
			Application.Run(splash);
		}

		private static void CloseSplash()
		{
			if(splash == null)
				return;
			try
			{
				// Shut down the splash screen
				splash.Invoke(new EventHandler(splash.KillMe));
				splash.Dispose();
				splash = null;
			}
			catch(Exception)
			{ }

		}

		/// <summary>
		/// Detect if running in various virtual machines.
		/// C# code only solution which is more compatible.
		/// </summary>
		private static bool IsInsideVM()
		{
			using(var searcher = new ManagementObjectSearcher("Select * from Win32_ComputerSystem"))
			{
				using(var items = searcher.Get())
				{
					foreach(var item in items)
					{
						Logger.AddInformation("Win32_ComputerSystem Manufacturer: " + item["Manufacturer"].ToString() + ", Model: " + item["Model"].ToString(), null, null);

						string manufacturer = item["Manufacturer"].ToString().ToLower();
						string model = item["Model"].ToString().ToLower();
						if((manufacturer == "microsoft corporation" && !model.Contains("surface"))
							|| manufacturer.Contains("vmware")
							|| manufacturer.Contains("parallels software")
							|| manufacturer.Contains("xen")
							|| model.Contains("xen")
							|| model.Contains("virtualbox"))
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		/// <summary>
		/// Detect if SEB Running inside VPC.
		/// </summary>
		/*private static bool IsInsideVPC()
		{
			if (VMDetect.IsInsideVPC)
			{
				//MessageBox.Show("Running inside Virtual PC!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
				Logger.AddInformation("Running inside Virtual PC!", null, null);
				return true;
			}
			else
			{
				//MessageBox.Show("Not running inside Virtual PC!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Error);
				Logger.AddInformation("Not running inside Virtual PC!", null, null);
				return false;
			}
		}

		/// <summary>
		/// Detect if SEB Running inside VMWare.
		/// </summary>
		private static bool IsInsideVMWare()
		{
			if (VMDetect.IsInsideVMWare)
			{
				//MessageBox.Show("Running inside VMWare!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
				Logger.AddInformation("Running inside VMWare!", null, null);
				return true;
			}
			else
			{
				//MessageBox.Show("Not running inside VMWare!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Error);
				Logger.AddInformation("Not running inside VMWare!", null, null);
				return false;
			}
		}*/

		private static bool CheckCreateDesktop()
		{
			//on NT4/NT5 ++ a new desktop is created
			if(WindowsVersionHelpers.SupportsMultipleDesktops)
			{
				sessionCreateNewDesktop = SebInstance.Settings.Get<bool>(SebSettings.KeyCreateNewDesktop);
				if(sessionCreateNewDesktop)
				{
					SEBClientInfo.OriginalDesktop = SEBDesktopController.GetCurrent();
					var inputDesktop = SEBDesktopController.OpenInputDesktop();

					SEBClientInfo.SEBNewlDesktop = SEBDesktopController.CreateDesktop(SebConstants.SEB_NEW_DESKTOP_NAME);
					SEBDesktopController.Show(SEBClientInfo.SEBNewlDesktop.DesktopName);
					if(!SEBDesktopController.SetCurrent(SEBClientInfo.SEBNewlDesktop))
					{
						Logger.AddError("SetThreadDesktop failed! Looks like the thread has hooks or windows in the current desktop.", null, null);
						SEBDesktopController.Show(SEBClientInfo.OriginalDesktop.DesktopName);
						SEBDesktopController.SetCurrent(SEBClientInfo.OriginalDesktop);
						SEBClientInfo.SEBNewlDesktop.Close();
						SebMessageBox.Show(SEBUIStrings.createNewDesktopFailed, SEBUIStrings.createNewDesktopFailedReason, MessageBoxImage.Error, MessageBoxButton.OK);
						return false;
					}
					SEBClientInfo.DesktopName = SebConstants.SEB_NEW_DESKTOP_NAME;
				}
				else
				{
					SEBClientInfo.OriginalDesktop = SEBDesktopController.GetCurrent();
					SEBClientInfo.DesktopName = SEBClientInfo.OriginalDesktop.DesktopName;
					//If you kill the explorer shell you don't need this!
					//SebWindowsClientForm.SetVisibility(false);
				}
			}
			return true;
		}

		/// ----------------------------------------------------------------------------------------
		/// <summary>
		/// Create and initialize new desktop.
		/// </summary>
		/// <returns>true if succeeded</returns>
		/// ----------------------------------------------------------------------------------------
		public static bool InitSEBDesktop()
		{
			//Info: For reverting this actions see SebWindowsClientForm::CloseSEBForm()

			Logger.AddInformation("Attempting to InitSEBDesktop");

			//Blank the Wallpaper
			SEBDesktopWallpaper.BlankWallpaper();

			// Clean clipboard
			SEBClipboard.CleanClipboard();
			Logger.AddInformation("Clipboard cleaned.");

			//Search for permitted Applications (used in Taskswitcher (ALT-TAB) and in foreground watchdog
			SEBWindowHandler.AllowedExecutables.Clear();
			//Add the SafeExamBrowser to the allowed executables
			SEBWindowHandler.AllowedExecutables.Add(Process.GetCurrentProcess().ProcessName.ToLower());
			//Add allowed executables from all allowedProcessList
			foreach(IDictionary<string, object> process in SebInstance.Settings.permittedProcessList)
			{
				if((bool)process[SebSettings.KeyActive])
				{
					//First add the executable itself
					SEBWindowHandler.AllowedExecutables.Add(
						((string)process[SebSettings.KeyExecutable]).ToLower());
					if(!String.IsNullOrWhiteSpace(process[SebSettings.KeyWindowHandlingProcess].ToString()))
					{
						SEBWindowHandler.AllowedExecutables.Add(
						((string)process[SebSettings.KeyWindowHandlingProcess]).ToLower());
					}
				}
			}

#if DEBUG_VS
			//Add visual studio to allowed executables for debugging
			SEBWindowHandler.AllowedExecutables.Add("devenv");
#endif

			//Process watching
			if(SebInstance.Settings.Get<bool>(SebSettings.KeyMonitorProcesses))
			{
				#region Foreground Window Watching (Allowed Executables)

				//This prevents the not allowed executables from poping up
				try
				{
					SEBWindowHandler.EnableForegroundWatchDog();
				}
				catch(Exception ex)
				{
					Logger.AddError("Unable to EnableForegroundWatchDog", null, ex);
				}
				#endregion

				#region Prohibited Executables watching
				//Handle prohibited executables watching
				SEBProcessHandler.ProhibitedExecutables.Clear();
				//Add prohibited executables
				foreach(IDictionary<string, object> process in SebInstance.Settings.prohibitedProcessList)
				{
					if((bool)process[SebSettings.KeyActive])
					{
						//First add the executable itself
						SEBProcessHandler.ProhibitedExecutables.Add(
							((string)process[SebSettings.KeyExecutable]).ToLower());
					}
				}
				//This prevents the prohibited executables from starting up
				try
				{
					SEBProcessHandler.EnableProcessWatchDog();
				}
				catch(Exception ex)
				{
					Logger.AddError("Unable to EnableProcessWatchDog", null, ex);
				}
				#endregion
			}

			//Kill Explorer Shell
			// Global variable indicating if the explorer shell has been killed
			SEBClientInfo.ExplorerShellWasKilled = false;
			if(SebInstance.Settings.Get<bool>(SebSettings.KeyKillExplorerShell))
			{
				//Minimize all Open Windows
				try
				{
					SEBWindowHandler.MinimizeAllOpenWindows();
				}
				catch(Exception ex)
				{
					Logger.AddError("Unable to MinimizeAllOpenWindows", null, ex);
				}
				//Kill the explorer Shell
				try
				{
					SEBClientInfo.ExplorerShellWasKilled = SEBProcessHandler.KillExplorerShell();
				}
				catch(Exception ex)
				{
					Logger.AddError("Unable to KillExplorerShell", null, ex);
				}
			}

			Logger.AddInformation("Successfully InitSEBDesktop");

			return true;
		}

		/// ----------------------------------------------------------------------------------------
		/// <summary>
		/// Reset desktop to the default one which was active before starting SEB.
		/// </summary>
		/// <returns>true if succeed</returns>
		/// ----------------------------------------------------------------------------------------
		public static void ResetSEBDesktop()
		{
			// Switch to Default Desktop
			if(sessionCreateNewDesktop)
			{
				Logger.AddInformation("Showing Original Desktop");
				SEBDesktopController.Show(SEBClientInfo.OriginalDesktop.DesktopName);
				Logger.AddInformation("Setting original Desktop as current");
				SEBDesktopController.SetCurrent(SEBClientInfo.OriginalDesktop);
				Logger.AddInformation("Closing New Dekstop");
				SEBClientInfo.SEBNewlDesktop.Close();
			}
			else
			{
				//If you kill the explorer shell you don't need this!
				//SetVisibility(true);
			}
		}

		/// <summary>
		/// Hide or show the Windows taskbar and startmenu.
		/// </summary>
		/// <param name="show">true to show, false to hide</param>
		public static void SetVisibility(bool show)
		{
			// get taskbar window
			IntPtr taskBarWnd = FindWindow("Shell_TrayWnd", null);

			// try it the WinXP way first...
			IntPtr startWnd = FindWindowEx(taskBarWnd, IntPtr.Zero, "Button", "Start");

			if(startWnd == IntPtr.Zero)
			{
				// try an alternate way, as mentioned on CodeProject by Earl Waylon Flinn
				startWnd = FindWindowEx(IntPtr.Zero, IntPtr.Zero, (IntPtr)0xC017, "Start");
			}

			if(startWnd == IntPtr.Zero)
			{
				// ok, let's try the Vista easy way...
				startWnd = FindWindow("Button", null);

				if(startWnd == IntPtr.Zero)
				{
					// no chance, we need to to it the hard way...
					startWnd = GetVistaStartMenuWnd(taskBarWnd);
				}
			}

			ShowWindow(taskBarWnd, show ? SW_SHOW : SW_HIDE);
			ShowWindow(startWnd, show ? SW_SHOW : SW_HIDE);
		}

		/// <summary>
		/// Returns the window handle of the Vista start menu orb.
		/// </summary>
		/// <param name="taskBarWnd">windo handle of taskbar</param>
		/// <returns>window handle of start menu</returns>
		private static IntPtr GetVistaStartMenuWnd(IntPtr taskBarWnd)
		{
			// get process that owns the taskbar window
			int procId;
			GetWindowThreadProcessId(taskBarWnd, out procId);

			Process p = Process.GetProcessById(procId);
			if(p != null)
			{
				// enumerate all threads of that process...
				foreach(ProcessThread t in p.Threads)
				{
					EnumThreadWindows(t.Id, MyEnumThreadWindowsProc, IntPtr.Zero);
				}
			}
			return vistaStartMenuWnd;
		}

		/// <summary>
		/// Callback method that is called from 'EnumThreadWindows' in 'GetVistaStartMenuWnd'.
		/// </summary>
		/// <param name="hWnd">window handle</param>
		/// <param name="lParam">parameter</param>
		/// <returns>true to continue enumeration, false to stop it</returns>
		private static bool MyEnumThreadWindowsProc(IntPtr hWnd, IntPtr lParam)
		{
			StringBuilder buffer = new StringBuilder(256);
			if(GetWindowText(hWnd, buffer, buffer.Capacity) > 0)
			{
				Console.WriteLine(buffer);
				if(buffer.ToString() == VistaStartMenuCaption)
				{
					vistaStartMenuWnd = hWnd;
					return false;
				}
			}
			return true;
		}


		/// ----------------------------------------------------------------------------------------
		/// <summary>
		/// Check if running in VM and if SEB Windows Service is running or not.
		/// </summary>
		/// <returns>true if both checks are positive, false means SEB needs to quit.</returns>
		/// ----------------------------------------------------------------------------------------
		public static bool CheckVMService()
		{
			// Test if run inside virtual machine
			bool allowVirtualMachine = SebInstance.Settings.Get<bool>(SebSettings.KeyAllowVirtualMachine);
			if(IsInsideVM() && (!allowVirtualMachine))
			//if ((IsInsideVMWare() || IsInsideVPC()) && (!allowVirtualMachine))
			{
				//SEBClientInfo.SebWindowsClientForm.Activate();
				SebMessageBox.Show(SEBUIStrings.detectedVirtualMachine, SEBUIStrings.detectedVirtualMachineForbiddenMessage, MessageBoxImage.Error, MessageBoxButton.OK);
				Logger.AddError("Forbidden to run SEB on a virtual machine!", null, null);
				Logger.AddInformation("Safe Exam Browser is exiting", null, null);
				Application.Exit();
				return false;
			}

			// Test if Windows Service is running
			bool serviceAvailable = SebWindowsServiceHandler.IsServiceAvailable;

			int forceService = SebInstance.Settings.Get<Int32>(SebSettings.KeySebServicePolicy);
			switch(forceService)
			{
				case (int)sebServicePolicies.ignoreService:
				break;
				case (int)sebServicePolicies.indicateMissingService:
				if(!serviceAvailable)
				{
					//SEBClientInfo.SebWindowsClientForm.Activate();
					SebMessageBox.Show(SEBUIStrings.indicateMissingService, SEBUIStrings.indicateMissingServiceReason, MessageBoxImage.Error, MessageBoxButton.OK);
				}
				break;
				case (int)sebServicePolicies.forceSebService:
				if(!serviceAvailable)
				{
					//SEBClientInfo.SebWindowsClientForm.Activate();
					SebMessageBox.Show(SEBUIStrings.indicateMissingService, SEBUIStrings.forceSebServiceMessage, MessageBoxImage.Error, MessageBoxButton.OK);
					Logger.AddError("SEB Windows service is not available and sebServicePolicies is set to forceSebService", null, null);
					Logger.AddInformation("SEB is exiting", null, null);
					Application.Exit();

					return false;
				}
				break;
				//default:
				//    if (!serviceAvailable)
				//    {
				//        SEBMessageBox.Show(SEBGlobalConstants.IND_WINDOWS_SERVICE_NOT_AVAILABLE, SEBGlobalConstants.IND_MESSAGE_KIND_ERROR);
				//    }
				//    break;
			}

			return true;
		}

		/// ----------------------------------------------------------------------------------------
		/// <summary>
		/// Move SEB to the foreground.
		/// </summary>
		/// ----------------------------------------------------------------------------------------
		public static void SEBToForeground()
		{
			//if ((bool)SebInstance.Settings.valueForDictionaryKey(SEBSettings.settingsCurrent, SEBSettings.KeyShowTaskBar))
			//{
			try
			{
				//SetForegroundWindow(SEBClientInfo.SebWindowsClientForm.Handle);
				SebApplicationChooserForm.forceSetForegroundWindow(SEBClientInfo.SebWindowsClientForm.Handle);
				SEBClientInfo.SebWindowsClientForm.Activate();
			}
			catch(Exception)
			{
			}

			//}
		}

		/// ----------------------------------------------------------------------------------------
		/// <summary>
		/// Create and initialize SEB client settings and check system compatibility.
		/// This method needs to be executed only once when SEB first starts 
		/// (not when reconfiguring).
		/// </summary>
		/// <returns>true if succeed</returns>
		/// ----------------------------------------------------------------------------------------
		public static bool? CheckLoadSettingsCommandLine(string[] args)
		{
			Logger.AddInformation("LoadSettings command line: " + string.Join(", ", args));
			if(args.Length > 0)
			{
				Settings.Default.LastExamUri = args[0];
				return LoadSettings(args[0]);
			}
			return null;
		}

		private static bool LoadClientSettings()
		{
			// Try to read the SebClientSettigs.seb file from the program data directory
			if(File.Exists(SEBClientInfo.SebClientSettingsProgramDataFile))
			{
				if(LoadSettings(SEBClientInfo.SebClientSettingsProgramDataFile))
				{
					return true;
				}
				Logger.AddError("Could not load SebClientSettigs.seb from the Program Data directory", SEBClientInfo.SebClientSettingsProgramDataFile, null);
			}
			if(File.Exists(SEBClientInfo.SebClientSettingsAppDataFile))
			{
				if(LoadSettings(SEBClientInfo.SebClientSettingsAppDataFile))
				{
					return true;
				}
				Logger.AddError("Could not load SebClientSettigs.seb from the Roaming Application Data directory", SEBClientInfo.SebClientSettingsAppDataFile, null);
			}

			if(SebConstants.SEB_FAIL_NO_CONFIG)
			{
				SebMessageBox.Show(SEBUIStrings.openingSettingsTitle, SEBUIStrings.openingSettingsFailed, MessageBoxImage.Error, MessageBoxButton.OK);
				return false;
			}
			do
			{
				var url = Microsoft.VisualBasic.Interaction.InputBox(SEBUIStrings.openingSettingsEnterLocation, SEBUIStrings.openingSettingsTitle, Settings.Default.LastExamUri);
				if(string.IsNullOrEmpty(url))
				{
					return false;
				}
				Settings.Default.LastExamUri = url;
				if(LoadSettings(url))
				{
					return true;
				}
			} while(true);
		}

		public static bool LoadSettings(string path)
		{
			if(!isLoadingSettings)
			{
				lock(LoadingFileLock)
				{
					if(!isLoadingSettings)
					{
						try
						{
							isLoadingSettings = true;
							return InternalLoadSettings(path);
						}
						finally
						{
							isLoadingSettings = false;
						}
					}
				}
			}
			return false;
		}

		private static bool InternalLoadSettings(string path)
		{
			Logger.AddInformation("Attempting to read new configuration file");

			byte[] sebSettings = null;
			Uri uri;
			try
			{
				uri = new Uri(path);
			}
			catch(Exception ex)
			{
				Logger.AddError("SEB was opened with a wrong parameter", path, ex, ex.Message);
				return false;
			}
			// Check if we're running in exam mode already, if yes, then refuse to load a .seb file
			if(SEBClientInfo.HasExamStarted)
			{
				//SEBClientInfo.SebWindowsClientForm.Activate();
				SebMessageBox.Show(SEBUIStrings.loadingSettingsNotAllowed, SEBUIStrings.loadingSettingsNotAllowedReason, MessageBoxImage.Error, MessageBoxButton.OK);
				return false;
			}

			if(uri.Scheme == "seb" || uri.Scheme == "sebs" || uri.Scheme == "http" || uri.Scheme == "https")
			// The URI is holding a seb:// web address for a .seb settings file: download it
			{
				// But only download and use the seb:// link to a .seb file if this is enabled
				if(SebInstance.Settings.IsEmpty || SebInstance.Settings.Get<bool>(SebSettings.KeyDownloadAndOpenSebConfig))
				{
					try
					{
						var protocol = (uri.Scheme == "seb" || uri.Scheme == "http") ? "http" : "https";
						CertificateValidator.SetUntrustedRootCertificateValidation();
						try
						{
							var myWebClient = new WebClient();
							var downloadUri = new UriBuilder(protocol, uri.Host, uri.Port, uri.AbsolutePath, uri.Query);
							using(myWebClient)
							{
								sebSettings = myWebClient.DownloadData(downloadUri.Uri);
							}
						}
						finally
						{
							CertificateValidator.SetTrustedCertificateValidation();
						}
					}
					catch(Exception ex)
					{
						SebMessageBox.Show(SEBUIStrings.cannotOpenSEBLink, string.Format(SEBUIStrings.cannotOpenSEBLinkMessage, ex.Message), MessageBoxImage.Error, MessageBoxButton.OK);
						Logger.AddError("Unable to follow the link provided", path, ex);
						return false;
					}
				}
			}
			else if(uri.IsFile)
			{
				try
				{
					sebSettings = File.ReadAllBytes(path);
				}
				catch(Exception streamReadException)
				{
					// Write error into string with temporary log string builder
					Logger.AddError("Settings could not be read from file.", path, streamReadException, streamReadException.Message);
					return false;
				}
			}
			// If some settings got loaded in the end
			if(sebSettings == null)
			{
				Logger.AddError("Loaded settings were empty.", path, null, null);
				return false;
			}
			Logger.AddInformation("Succesfully read the new configuration");
			// Decrypt, parse and store new settings and restart SEB if this was successfull
			Logger.AddInformation("Attempting to StoreDecryptedSEBSettings");

			// We will need to check if setting for createNewDesktop changed
			SEBClientInfo.CreateNewDesktopOldValue = SebInstance.Settings.GetOrDefault(SebSettings.KeyCreateNewDesktop, false);
			if(!SebConfigFileManager.StoreDecryptedSEBSettings(sebSettings, SebPasswordInput.ClientGetPassword, SebInstance.Settings))
			{
				Logger.AddInformation("StoreDecryptedSettings returned false, this means the user canceled when entering the password, didn't enter a right one after 5 attempts or new settings were corrupted, exiting");
				Logger.AddError("Settings could not be decrypted or stored.", path, null, null);
				return false;
			}
			return true;
		}

		private static bool CheckLoadClientInfo()
		{
			// Check if client configuration was already set
			if(!clientSettingsSet)
			{
				// We need to set the client settings first
				if(SEBClientInfo.SetSebClientConfiguration())
				{
					clientSettingsSet = true;
					Logger.AddInformation("SEB client configuration set.", SEBClientInfo.UserName, null);
				}
				else
				{
					SebMessageBox.Show(SEBUIStrings.ErrorCaption, SEBUIStrings.ErrorWhenOpeningSettingsFile, MessageBoxImage.Error, MessageBoxButton.OK);
					Logger.AddError("Error when opening client configuration", SEBClientInfo.UserName, null);
					return false;
				}
			}
			return true;
		}
	}
}
