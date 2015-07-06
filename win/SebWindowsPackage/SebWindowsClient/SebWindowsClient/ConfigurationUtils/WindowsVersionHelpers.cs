using System;
using System.Runtime.InteropServices;
using SebShared.DiagnosticUtils;

namespace SebWindowsClient.ConfigurationUtils
{
	public static class WindowsVersionHelpers
	{
		public static bool SupportsMultipleDesktops
		{
			get
			{
				if(version == 0)
				{
					GetVersionInfo();
				}
				// Is new windows version
				switch(version)
				{
					case WIN_NT_351:
					case WIN_NT_40:
					case WIN_2000:
					case WIN_XP:
					case WIN_VISTA:
					case OS_UNKNOWN:
						return true;
					case WIN_95:
					case WIN_98:
					case WIN_ME:
						return false;
					default:
						return true;
				}
			}
		}

		private static int version;

		/// <summary>
		/// Stores windows version info.
		/// </summary>
		[StructLayout(LayoutKind.Sequential)]
		public struct OSVERSIONINFO
		{
			public int dwOSVersionInfoSize;
			public int dwMajorVersion;
			public int dwMinorVersion;
			public int dwBuildNumber;
			public int dwPlatformId;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
			public string szCSDVersion;
		}

		private const int OS_UNKNOWN = 800;
		private const int WIN_95 = 950;
		private const int WIN_98 = 980;
		private const int WIN_ME = 999;
		private const int WIN_NT_351 = 1351;
		private const int WIN_NT_40 = 1400;
		private const int WIN_2000 = 2000;
		private const int WIN_XP = 2010;
		private const int WIN_VISTA = 2050;
		private const int WIN_7 = 2050;
		private const int WIN_8 = 2050;

		[DllImport("kernel32.Dll")]
		public static extern short GetVersionEx(ref OSVERSIONINFO o);

		/// <summary>
		/// Sets system version info.
		/// </summary>
		/// <returns></returns>
		public static void GetVersionInfo()
		{
			var os = new OSVERSIONINFO();
			os.dwOSVersionInfoSize = Marshal.SizeOf(typeof(OSVERSIONINFO));
			try
			{
				// Gets os version
				if(GetVersionEx(ref os) != 0)
				{
					switch(os.dwPlatformId)
					{
						case 1:
							switch(os.dwMinorVersion)
							{
								case 0:
									version = WIN_95;
									break;
								case 10:
									version = WIN_98;
									break;
								case 90:
									version = WIN_ME;
									break;
								default:
									version = OS_UNKNOWN;
									break;
							}
							break;
						case 2:
							switch(os.dwMajorVersion)
							{
								case 3:
									version = WIN_NT_351;
									break;
								case 4:
									version = WIN_NT_40;
									break;
								case 5:
									if(os.dwMinorVersion == 0)
										version = WIN_2000;
									else
										version = WIN_XP;
									break;
								case 6:
									if(os.dwMinorVersion == 0)
										version = WIN_VISTA;
									else if(os.dwMinorVersion == 1)
										version = WIN_7;
									else if(os.dwMinorVersion == 2)
										version = WIN_8;
									else
										version = WIN_VISTA;
									break;
								default:
									version = OS_UNKNOWN;
									break;
							}
							break;
						default:
							version = OS_UNKNOWN;
							break;
					}
				}
			}
			catch(Exception ex)
			{
				Logger.AddError("SetSystemVersionInfo.", null, ex);
				version = OS_UNKNOWN;
			}

			Logger.AddInformation("OS Version: " + version);
		}
	}
}