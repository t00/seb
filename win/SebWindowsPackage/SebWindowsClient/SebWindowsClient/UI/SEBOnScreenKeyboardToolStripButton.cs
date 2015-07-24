using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.Win32;
using SebShared;
using SebShared.Properties;
using SebWindowsClient.ProcessUtils;
using SebWindowsClient.Properties;
using SebWindowsClient.XULRunnerCommunication;

namespace SebWindowsClient.UI
{
	public class SEBOnScreenKeyboardToolStripButton: SEBToolStripButton
	{
		public SEBOnScreenKeyboardToolStripButton()
		{
			InitializeComponent();
			this.Alignment = ToolStripItemAlignment.Right;
		}

		protected override void OnClick(EventArgs e)
		{
			if(SebInstance.Settings.Get<bool>(SebSettings.KeyEnableOnScreenKeyboardNative))
			{
				if(TapTipHandler.IsKeyboardVisible())
				{
					TapTipHandler.HideKeyboard();
				}
				else
				{
					TapTipHandler.ShowKeyboard();
				}
			}
		}

		private void InitializeComponent()
		{
			// 
			// SEBOnScreenKeyboardToolStripButton
			// 
			this.ToolTipText = SEBUIStrings.toolTipOnScreenKeyboard;
			base.Image = (Bitmap)Resources.ResourceManager.GetObject("keyboard");
		}
	}

	public static class TapTipHandler
	{
		public delegate void KeyboardStateChangedEventHandler(bool shown);
		public static event KeyboardStateChangedEventHandler OnKeyboardStateChanged;

		public static void RegisterXulRunnerEvents()
		{
			if(SebInstance.Settings.Get<bool>(SebSettings.KeyEnableOnScreenKeyboardWeb))
			{
				SEBXULRunnerWebSocketServer.OnXulRunnerTextFocus += (x, y) => ShowKeyboard();
				SEBXULRunnerWebSocketServer.OnXulRunnerTextBlur += (x, y) => HideKeyboard();
			}
		}

		public static void ShowKeyboard()
		{
			try
			{
				if(!SEBWindowHandler.AllowedExecutables.Contains("taptip.exe"))
					SEBWindowHandler.AllowedExecutables.Add("tabtip.ex");

				if(!IsKeyboardVisible())
				{
					string progFiles = @"C:\Program Files\Common Files\Microsoft Shared\ink";
					string onScreenKeyboardPath = Path.Combine(progFiles, "TabTip.exe");
					Process.Start(onScreenKeyboardPath);
					if(OnKeyboardStateChanged != null)
					{
						var t = new System.Timers.Timer { Interval = 500 };
						t.Elapsed += (sender, args) =>
						{
							if(!IsKeyboardVisible())
							{
								OnKeyboardStateChanged(false);
								t.Stop();
							}
						};
						t.Start();
					}
				}
				OnKeyboardStateChanged(true);
			}
			catch
			{ }
		}

		public static void HideKeyboard()
		{
			if(IsKeyboardVisible())
			{
				uint WM_SYSCOMMAND = 274;
				IntPtr SC_CLOSE = new IntPtr(61536);
				IntPtr KeyboardWnd = FindWindow("IPTip_Main_Window", null);
				PostMessage(KeyboardWnd, WM_SYSCOMMAND, SC_CLOSE, (IntPtr)0);
			}

			if(OnKeyboardStateChanged != null)
			{
				OnKeyboardStateChanged(false);
			}
		}

		[return: MarshalAs(UnmanagedType.Bool)]
		[DllImport("user32.dll", SetLastError = true)]
		static extern bool PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

		/// <summary>
		/// The window is disabled. See http://msdn.microsoft.com/en-gb/library/windows/desktop/ms632600(v=vs.85).aspx.
		/// </summary>
		public const UInt32 WS_DISABLED = 0x8000000;

		/// <summary>
		/// Specifies we wish to retrieve window styles.
		/// </summary>
		public const int GWL_STYLE = -16;

		[DllImport("user32.dll")]
		public static extern IntPtr FindWindow(String sClassName, String sAppName);

		[DllImport("user32.dll", SetLastError = true)]
		private static extern UInt32 GetWindowLong(IntPtr hWnd, int nIndex);


		/// <summary>
		/// Gets the window handler for the virtual keyboard.
		/// </summary>
		/// <returns>The handle.</returns>
		public static IntPtr GetKeyboardWindowHandle()
		{
			return FindWindow("IPTip_Main_Window", null);
		}

		/// <summary>
		/// Checks to see if the virtual keyboard is visible.
		/// </summary>
		/// <returns>True if visible.</returns>
		public static bool IsKeyboardVisible()
		{
			IntPtr keyboardHandle = GetKeyboardWindowHandle();

			bool visible = false;

			if(keyboardHandle != IntPtr.Zero)
			{
				UInt32 style = GetWindowLong(keyboardHandle, GWL_STYLE);
				visible = ((style & WS_DISABLED) != WS_DISABLED);
			}

			return visible;
		}

		public static bool IsKeyboardDocked()
		{
			int docked = 1;

			try
			{
				//HKEY_CURRENT_USER\Software\Microsoft\TabletTip\1.7\EdgeTargetDockedState -> 0 = floating, 1 = docked
				docked = (int)Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\TabletTip\1.7\", "EdgeTargetDockedState", 1);
			}
			catch { }

			return docked == 1;

		}
	}
}
