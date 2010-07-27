/* ***** BEGIN LICENSE BLOCK *****
* Version: MPL 1.1
*
* The contents of this file are subject to the Mozilla Public License Version
* 1.1 (the "License"); you may not use this file except in compliance with
* the License. You may obtain a copy of the License at
* http://www.mozilla.org/MPL/
*
* Software distributed under the License is distributed on an "AS IS" basis,
* WITHOUT WARRANTY OF ANY KIND, either express or implied. See the License
* for the specific language governing rights and limitations under the
* License.
*
* The Original Code is the SEB kiosk application.
*
* The Initial Developer of the Original Code is Justus-Liebig-Universitaet Giessen.
* Portions created by the Initial Developer are Copyright (C) 2005
* the Initial Developer. All Rights Reserved.
*
* Contributor(s):
*   Stefan Schneider <stefan.schneider@hrz.uni-giessen.de>
*   Oliver Rahs <rahs@net.ethz.ch>
*
* ***** END LICENSE BLOCK ***** */

#include "stdafx.h"
#include "Seb.h"
#include "KillProc.h"
#include "ProcMonitor.h"


/* Forward declarations of functions included in this code module: */
LRESULT CALLBACK	WndProc(HWND,  UINT, WPARAM, LPARAM);
LRESULT	CALLBACK	LLKeyboardHook( int, WPARAM, LPARAM);
LRESULT	CALLBACK	  KeyboardHook( int, WPARAM, LPARAM);
LRESULT CALLBACK	  About(HWND,  UINT, WPARAM, LPARAM);
ATOM				MyRegisterClass(HINSTANCE hInstance);
BOOL				InitInstance   (HINSTANCE, int);
BOOL				ReadIniFile();
BOOL				ReadProcessesInRegistry();
BOOL				ShowSebAppChooser();
BOOL				GetClientInfo();
BOOL				 EditRegistry();
BOOL				ResetRegistry();
BOOL				AlterTaskBar(BOOL);
BOOL				MessageHook (BOOL);
BOOL				CreateExternalProcess(string);
BOOL				ShutdownInstance();						//cleaning up and resetting altered system before destroying the window
VOID				MonitorProcesses(threadParameters & parameters);

typedef void (*KEYHOOK)(HINSTANCE*, bool); //typedef for the KeyHook function of the loaded MsgHook.dll
KEYHOOK KeyHook;

typedef void (*MOUSEHOOK)(HINSTANCE*, bool); //typedef for the MouseHook function of the loaded MsgHook.dll
MOUSEHOOK MouseHook;

/* Utility Functions */
BOOL				CheckWritePermission(LPCSTR);
VOID				Tokenize(const string&, vector<string>&, const string&);
BOOL				killedExplorer;
BOOL				getBool      (string);
string				getLangString(string);
int					getInt       (string);
BOOL				HandleOpenRegistryKey(HKEY, LPCSTR, HKEY*, BOOL);
BOOL				HandleSetRegistryKeyValue(HKEY, LPCSTR, string);
DWORD				dwExitCode;
BOOL				SetVersionInfo();

/* Global Variables */
HDESK hOriginalThread;						//Original Desktop Thread
HDESK hOriginalInput;						//Original Desktop Input
HDESK hNewDesktop;							//New Desktop
HINSTANCE hInst;							//Current Instance
HWND      hWnd;								//Handle to the own SEB Window
HMENU     hMenu;
HINSTANCE hinstDLL = NULL;					//instance of the hook library
HANDLE procMonitorThread;

TCHAR szTitle      [MAX_LOADSTRING];		// The title bar text
TCHAR szWindowClass[MAX_LOADSTRING];		// the main window class name
SystemVersionInfo sysVersionInfo;				
BOOL IsNewOS = FALSE;
BOOL b1, b2, b3;
char cHostname[255];						//char with Hostname
char* cIp;									//Pointer to char with IP Address
map< string, string > mpParam;				//map for *.ini parameters
map< string, string > mpProcesses;
vector< long> previousProcesses;			// troxler
vector< long>  allowedProcesses;			// troxler
threadParameters parameters;				// troxler


map< string, string >::iterator itProcesses;
map< int, string > mpProcessCommands;
map< int, string >::iterator itProcessCommands;
map< string, PROCESS_INFORMATION > mpProcessInformations;
map< string, PROCESS_INFORMATION >::iterator itProcessInformations;
PROCESS_INFORMATION piProcess;				//PROCESS_INFORMATION created process
//ofstream of;



/* Api Entry */
int APIENTRY _tWinMain(HINSTANCE hInstance,
                       HINSTANCE hPrevInstance,
                       LPTSTR    lpCmdLine,
                       int       nCmdShow)
{
	MSG msg;
	HACCEL hAccelTable;	
	// Initialize global strings
	LoadString     (hInstance, IDS_APP_TITLE, szTitle      , MAX_LOADSTRING);
	LoadString     (hInstance, IDC_SEB      , szWindowClass, MAX_LOADSTRING);
	MyRegisterClass(hInstance);
	DWORD dwRet = 0; 

	// Perform application initialization:
	//InitInstance (hInstance, nCmdShow);
	/*
	{
		MessageBox(hWnd,INITIALIZE_ERROR,"Error",MB_ICONERROR);
		return FALSE;
	}
	*/
	if (!InitInstance (hInstance, nCmdShow))
	{
		MessageBox(hWnd, INITIALIZE_ERROR, "Error", MB_ICONERROR);
	}

	hAccelTable = LoadAccelerators(hInstance, (LPCTSTR)IDC_SEB);
	PROCESS_INFORMATION pi;
	string shutDownProcess = mpParam["AUTOSTART_PROCESS"];

	/*
	if (!pi.hProcess)
	{
		MessageBox(hWnd,"kjhkjhkjh","Warning",MB_ICONWARNING);
	}
	*/

	/* main message loop */
	/*
	If the autostart process is finished it signals and closes SEB.
	It does not work if p.e. firefox writes something into the profile at startup process. 
	(firefox possibly finishes and starts itself in a new process.) 
	In this case "SHUTDOWN_AFTER_PROCESS_TERMINATES" should be empty (!)
	and the only way to finish SEB is the defined Hotkey.
	*/

	if (getBool("SHUTDOWN_AFTER_AUTOSTART_PROCESS_TERMINATES") && shutDownProcess != "")
	{
		pi = mpProcessInformations[shutDownProcess];
		while (1)
		{
			dwRet = MsgWaitForMultipleObjects(1, &(pi.hProcess), FALSE, INFINITE, QS_ALLINPUT);
			if (dwRet == WAIT_OBJECT_0)
			{
				//MessageBox(NULL,"1","Error",MB_ICONERROR);
				SendMessage(hWnd,WM_DESTROY,NULL,NULL);
				return TRUE;    // The event was signaled
			}

			if (dwRet != WAIT_OBJECT_0 + 1)
			{
				// Something else happened
				//MessageBox(NULL,"2","Error",MB_ICONERROR);
				break;
			}

			// There is one or more window message available. Dispatch them

			while(PeekMessage(&msg,NULL,NULL,NULL,PM_REMOVE))
			{
				if (!TranslateAccelerator(msg.hwnd, hAccelTable, &msg))
				{
					TranslateMessage(&msg);
					 DispatchMessage(&msg);
				}
				if (WaitForSingleObject(pi.hProcess, 0) == WAIT_OBJECT_0)
				{
					//MessageBox(NULL,"3","Error",MB_ICONERROR);
					return TRUE; // Event is now signaled.
				}
			}
		}
	}
	else
	{
		while (GetMessage(&msg, NULL, 0, 0)) 
		{
			if (!TranslateAccelerator(msg.hwnd, hAccelTable, &msg)) 
			{
				TranslateMessage(&msg);
				 DispatchMessage(&msg);
			}
		}
	}
	return (int) msg.wParam;
}



//
//  FUNCTION: MyRegisterClass()
//
//  PURPOSE: Registers the window class.
//
//  COMMENTS:
//
//    This function and its usage are only necessary if you want this code
//    to be compatible with Win32 systems prior to the 'RegisterClassEx'
//    function that was added to Windows 95. It is important to call this function
//    so that the application will get 'well formed' small icons associated
//    with it.
//
ATOM MyRegisterClass(HINSTANCE hInstance)
{
	WNDCLASSEX wcex;
	wcex.cbSize         = sizeof(WNDCLASSEX); 
	wcex.style			= CS_HREDRAW | CS_VREDRAW;
	wcex.lpfnWndProc	= (WNDPROC)WndProc;
	wcex.cbClsExtra		= 0;
	wcex.cbWndExtra		= 0;
	wcex.hInstance		= hInstance;
	wcex.hIcon			= LoadIcon(hInstance, (LPCTSTR)IDI_SEB);
	wcex.hCursor		= LoadCursor(NULL, IDC_ARROW);
	wcex.hbrBackground	= (HBRUSH)(COLOR_WINDOW+1);
	wcex.lpszMenuName	= (LPCTSTR)IDC_SEB;
	wcex.lpszClassName	= szWindowClass;
	wcex.hIconSm		= LoadIcon(wcex.hInstance, (LPCTSTR)IDI_SEB);

	return RegisterClassEx(&wcex);
}


//
//   FUNCTION: InitInstance(HANDLE, int)
//
//   PURPOSE: Saves instance handle and creates main window
//
//   COMMENTS:
//
//        In this function, we save the instance handle in a global variable and
//        create and display the main program window.
//



BOOL InitInstance(HINSTANCE hInstance, int nCmdShow)
{	
	hInst = hInstance; // Store instance handle in our global variable
	DWORD dwNotUsedForAnything = 0;	
	parameters.procedureReady  = 0;
	vector<string> vStrongKillProcessesBefore;
	string         sStrongKillProcessesBefore;
	int ret;
	char buffer [100];
	//its important to set the CurrentDirectory to the AppDir if you call the App from a Link
	char szAppPath[MAX_PATH] = "";
	string strAppDirectory;
	GetModuleFileName(0, szAppPath, sizeof(szAppPath) - 1);
	// Extract directory
	strAppDirectory = szAppPath;
	strAppDirectory = strAppDirectory.substr(0, strAppDirectory.rfind("\\"));
	SetCurrentDirectory(strAppDirectory.c_str());

	if (!ReadIniFile())
	{
		MessageBox(NULL, NO_INI_ERROR, "Error", MB_ICONERROR);
		return FALSE;
	}


/*
	if (getBool("CHECK_WRITE_PERMISSION") && (!CheckWritePermission("\\\\Three\\kiox_dev\\kiox_clients\\windows\\WinKeyox\\WinKeyox\\Release\\WinKeyox.ini")))
	{
		MessageBox(NULL,NO_WRITE_PERMISSION,"Error",MB_ICONERROR);
		return FALSE;
	}
*/


	// Trunk version (XUL-Runner)
	if (!SetVersionInfo())
	{
		MessageBox(NULL, NO_OS_SUPPORT, "Error", 16);
		return FALSE;
	}

	// Tags 1.3 version (Portable Firefox)
/*
	switch (sysVersionInfo.GetVersion())
	{
		case WIN_NT_351 :
		case WIN_NT_40  :
		case WIN_2000   :
		case WIN_XP     :
		case WIN_VISTA  :
	  //case WIN_7      :
			IsNewOS = TRUE;
			//MessageBox(NULL,"= TRUE","IsNewOS",16);
			break;
		case WIN_95 :
		case WIN_98 :
		case WIN_ME :
			IsNewOS = FALSE;
			break;
		default :
			MessageBox(NULL,NO_OS_SUPPORT,"Error",16);
			return FALSE;
	}
*/


	// locks OS
	if (!IsNewOS)
	{
		//just kill explorer.exe on Win9x / Me
		if (getBool("WIN9X_KILL_EXPLORER"))
		{
			ret = KILL_PROC_BY_NAME("explorer.exe");
			if (ret != 0)
			{
				sprintf(buffer, KILL_PROC_FAILED, "explorer.exe", ret);
				MessageBox(NULL,buffer,"Error",16);
				killedExplorer = FALSE;
			}
			else
			{
				killedExplorer = TRUE;
			}
		}
		//tell Win9x / Me that the screensaver is running to lock system tasks
		if (getBool("WIN9X_SCREENSAVERRUNNING"))
		{
			SystemParametersInfo(SPI_SCREENSAVERRUNNING, TRUE, &dwNotUsedForAnything, NULL);
		}
	}
	else
	{
		//on NT4/NT5 a new desktop is created
		if (getBool("NEW_DESKTOP"))
		{
			hOriginalThread = GetThreadDesktop(GetCurrentThreadId());
			hOriginalInput = OpenInputDesktop(0, FALSE, DESKTOP_SWITCHDESKTOP);

			// Create a new Desktop and switch to it
			hNewDesktop = CreateDesktop(SEB_DESK, NULL, NULL, 0, GENERIC_ALL, NULL);
			SetThreadDesktop(hNewDesktop);
			   SwitchDesktop(hNewDesktop);
		}		
	}

	// start the SEB window
	hWnd = CreateWindow(szWindowClass, szTitle, WS_MAXIMIZE, 10, 10, 150, 50, NULL, NULL, hInstance, NULL);

	if (!hWnd)
	{
		MessageBox(NULL, INITIALIZE_ERROR, "Error", MB_ICONERROR);
		return FALSE;
	}

	if (!GetClientInfo())
	{
		MessageBox(NULL, NO_CLIENT_INFO_ERROR, "Error", MB_ICONERROR);
		return FALSE;
	}

	if (getBool("EDIT_REGISTRY") && IsNewOS)
	{
		if (!EditRegistry())
		{
			//MessageBox(hWnd,REG_EDIT_ERROR,REGISTRY_WARNING,MB_ICONWARNING);
			mpParam["EDIT_REGISTRY"] = "0"; //thats for ResetRegistry: do nothing because editing failed
		}
	}

	sStrongKillProcessesBefore = mpParam["STRONG_KILL_PROCESSES_BEFORE"];
	if (sStrongKillProcessesBefore != "")
	{		
		Tokenize(sStrongKillProcessesBefore, vStrongKillProcessesBefore, ";");
		//MessageBox(hWnd, vKillProcess[1].c_str(), "Error", MB_ICONWARNING);
		for (int i=0; i < (int)vStrongKillProcessesBefore.size(); i++)
		{
			ret = KILL_PROC_BY_NAME(vStrongKillProcessesBefore[i].c_str());
		}
	}

	if (mpParam["AUTOSTART_PROCESS"] != "")
	{
		if (!CreateExternalProcess(mpParam["AUTOSTART_PROCESS"]))
		{
			MessageBox(hWnd, PROCESS_FAILED, "Error", MB_ICONWARNING);
			return FALSE;
		}		
	}

	/* setting the HOOK */

	if (getBool("MESSAGE_HOOK"))
	{
		if (hinstDLL == NULL) 
		{	
			hinstDLL = LoadLibrary((LPCTSTR) mpParam["HOOK_DLL"].c_str());
		}
		if (hinstDLL == NULL) 
		{
			MessageBox(NULL, LOAD_LIBRARY_ERROR, "Error", 16);
			return FALSE;
		}
		if (IsNewOS)
		{
			  KeyHook =   (KEYHOOK)GetProcAddress(hinstDLL,   "KeyHookNT"); //Address Of KeyHookNT
			MouseHook = (MOUSEHOOK)GetProcAddress(hinstDLL, "MouseHookNT"); //Address Of KeyHookNT
		}
		else
		{
			  KeyHook =   (KEYHOOK)GetProcAddress(hinstDLL,   "KeyHook9x"); //Address Of KeyHookNT
			MouseHook = (MOUSEHOOK)GetProcAddress(hinstDLL, "MouseHook9x"); //Address Of KeyHookNT
		}
		  KeyHook(&hinstDLL, TRUE);
		MouseHook(&hinstDLL, TRUE);
	}

	// Show Window	
	if (ShowSebAppChooser())
	{
		int cmd = (getBool("AUTOSTART_PROCESS")) ? SW_SHOWNORMAL : SW_SHOWNORMAL; //Not very suggestive yet
		ShowWindow(hWnd,cmd);
		UpdateWindow(hWnd);
	}

	// this is not the set of allowd processes to run & the processes in the list process
	GetRunningProcesses(previousProcesses);
	allowedProcesses.insert(allowedProcesses.end(), previousProcesses.begin(), previousProcesses.end());

	long threadID;

	parameters.allowedProcesses = &allowedProcesses;
	parameters.mpProcesses = mpProcesses; // Only used in Branches, not in Tags!
	parameters.desktop = hNewDesktop;
	parameters.hold    = 0;
	parameters.confirm = 0;

	if(getBool("PROC_MONITORING"))
	{
		procMonitorThread = CreateThread(NULL, 0, (LPTHREAD_START_ROUTINE)MonitorProcesses, (LPVOID)&parameters, 0, (LPDWORD)&threadID);
		parameters.procedureReady = 1;
	}

	return TRUE;	
}



BOOL ReadIniFile()
{
	char   cCurrDir[MAX_PATH];
	string sCurrDir = "";
	string strLine;
	string strKey;
	string strValue;
	string sProcesses = "";
	vector<string> vProcesses;
	vector<string>::iterator itProcesses;
  //vector<string>::iterator itProcess;
	string sProcess = "";
  //string sCommandProcess = "";

	try
	{
		GetModuleFileName(NULL, cCurrDir, sizeof(cCurrDir));
		sCurrDir = (string)cCurrDir;

		const char* captionString;
		const char* messageString;
		captionString = "Program executable:";
		messageString = cCurrDir;
	  //MessageBox(NULL, messageString, captionString, 16);

		// The Seb.ini and MsgHook.ini configuration files have moved:
		// Previously:
		//     Seb.ini was lying in the /Seb     subdirectory,
		// MsgHook.ini was lying in the /MsgHook subdirectory.
		// Both had to be copied to the /Debug and /Release directories.
		// before starting Seb.exe .
		// Now:
		// Seb.ini and MsgHook.ini are both in the Seb main project directory,
		// together with the Seb.sln project file,
		// the /Debug subdirectory and the /Release subdirectory.
		// Advantage: the .ini files are lying together, being accessible
		// for both the /Debug and the /Release version without copying
		// being necessary anymore.

	  //sCurrDir.replace(((size_t)sCurrDir.length()-3), 3, "ini");
		sCurrDir = SEB_INI;

		ifstream inf(sCurrDir.c_str());	
		if (!inf.is_open()) 
		{
			MessageBox(NULL, NO_INI_ERROR, "Error", 16);
			return FALSE;
		}

		while(!getline(inf, strLine).eof())
		{			
			strKey   = strLine.substr(0, strLine.find("=", 0));
			strValue = strLine.substr(   strLine.find("=", 0)+1, strLine.length());
			mpParam[strKey] = strValue;

			//captionString = strKey  .c_str();
			//messageString = strValue.c_str();
			//MessageBox(NULL, messageString, captionString, 16);
		}
		inf.close();

		/* Get Processes */
		string sebProcess = mpParam["SEB_BROWSER"];

		vector<string> sebProcessVector;
		Tokenize(sebProcess, sebProcessVector, ",");												
		mpProcesses.insert(make_pair(sebProcessVector[0], sebProcessVector[1]));

		// handle processes from Registry
		ReadProcessesInRegistry();

		if (mpProcesses.size() == 1)
		{
			// if nothing is found in registry  -> read Seb.ini
			// handle processes from configuration file Seb.ini
			sProcesses = mpParam["PERMITTED_APPS"];
			if (sProcesses != "")
			{
				Tokenize(sProcesses, vProcesses, ";");
				for (itProcesses = vProcesses.begin(); itProcesses != vProcesses.end(); itProcesses++)
				{
					vector<string> vProcess;
					sProcess = *itProcesses;
					Tokenize(sProcess, vProcess, ",");
					mpProcesses.insert(make_pair(vProcess[0], vProcess[1]));
				}
			}
		}
	}
	catch (char* str)
	{		
		MessageBox(NULL, str, "Error", MB_ICONERROR); 
		return FALSE;
	}
	return TRUE;
}



BOOL ReadProcessesInRegistry() 
{
	try
	{
		vector<string> vProcesses;
		string sProcesses = "";
		string sProcess   = "";
		vector<string>::iterator itProcesses;
		HKEY  hKeySEB;
		char  lszValue[255];
		LONG  returnStatus;
		DWORD dwType = REG_SZ;
		DWORD dwSize = 255;
		if (!HandleOpenRegistryKey(HKLM, KEY_RegPolicySEB, &hKeySEB, FALSE)) return FALSE;
		returnStatus = RegQueryValueEx(hKeySEB, VAL_PERMITTED_APPS, NULL, &dwType,(LPBYTE)&lszValue, &dwSize);

		if (returnStatus == ERROR_SUCCESS)
		{
			Tokenize(sProcesses, vProcesses, ";");
			//MessageBox(hWnd,vKillProcesses[1].c_str(),"Error",MB_ICONWARNING);
			int cntCommand = IDM_OFFSET;
			for (itProcesses = vProcesses.begin(); itProcesses != vProcesses.end(); itProcesses++)
			{
				vector<string> vProcess;
				sProcess = *itProcesses;				
				Tokenize(sProcess, vProcess, ",");												
				mpProcesses.insert(make_pair(vProcess[0], vProcess[1]));	
			}
		}
		RegCloseKey(hKeySEB);
	}
	catch (char* str)
	{		
		MessageBox(NULL, str, "Error", MB_ICONERROR); 
		return FALSE;
	}
	return TRUE;
}



BOOL GetClientInfo()
{
	WORD     wVersionRequested;
	WSADATA  wsaData;		
	PHOSTENT hostinfo;
	wVersionRequested = MAKEWORD(2, 0);
	try
	{
		if (WSAStartup(wVersionRequested, &wsaData) == 0)
		{
			if (gethostname(cHostname, sizeof(cHostname)) == 0)
			{
				//MessageBox(NULL,cHostname,"cHostname",16);
				if((hostinfo = gethostbyname(cHostname)) != NULL)
				{
					cIp = inet_ntoa(*(struct in_addr *)*hostinfo->h_addr_list);
				}
			}
			WSACleanup( );
		}
	}
	catch (char* str)
	{		
		MessageBox(NULL,str,"Error",MB_ICONERROR); 
		return FALSE;
	}
	return TRUE;
}



BOOL EditRegistry()
{
	HKEY hklmSystem;
	HKEY hkcuSystem;
	HKEY hkcuExplorer;
	BOOL openedHKLMPolicySystem;
	BOOL openedHKCUPolicySystem;
	BOOL openedHKCUPolicyExplorer;

	try
	{
		// Open the Windows Registry Key
		// HKEY_LOCAL_MACHINE\Software\Microsoft\Windows\CurrentVersion\Policies\System
		openedHKLMPolicySystem = HandleOpenRegistryKey(HKLM, KEY_RegPolicySystem  , &hklmSystem  , TRUE);
		if (openedHKLMPolicySystem)
		{
			//MessageBox(NULL,"HandleOpenRegistryKey(HKLM, System) succeeded","Registry",16);
		}
		else
		{
			MessageBox(NULL,"HandleOpenRegistryKey(HKLM, System) failed","Registry Error",16);
			return FALSE;
		}

		// Open the Windows Registry Key
		// HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\System
		openedHKCUPolicySystem = HandleOpenRegistryKey(HKCU, KEY_RegPolicySystem  , &hkcuSystem  , TRUE);
		if (openedHKCUPolicySystem)
		{
			//MessageBox(NULL,"HandleOpenRegistryKey(HKCU, System) succeeded","Registry",16);
		}
		else
		{
			MessageBox(NULL,"HandleOpenRegistryKey(HKCU, System) failed","Registry Error",16);
			return FALSE;
		}

		// Open the Windows Registry Key
		// HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer
		openedHKCUPolicyExplorer = HandleOpenRegistryKey(HKCU, KEY_RegPolicyExplorer, &hkcuExplorer, TRUE);
		if (openedHKCUPolicyExplorer)
		{
			//MessageBox(NULL,"HandleOpenRegistryKey(HKCU, Explorer) succeeded","Registry",16);
		}
		else
		{
			MessageBox(NULL,"HandleOpenRegistryKey(HKCU, Explorer) failed","Registry Error",16);
			return FALSE;
		}


		// Set the Windows Registry Key
		// HKEY_LOCAL_MACHINE\Software\Microsoft\Windows\CurrentVersion\Policies\System\HideFastUserSwitching
		if (getBool("REG_HIDE_FAST_USER_SWITCHING")) 
		{
			//MessageBox(NULL,"= true","REG_HIDE_FAST_USER_SWITCHING",16);

			if (HandleSetRegistryKeyValue(hklmSystem,VAL_HideFastUserSwitching,"HIDE_FAST_USER_SWITCHING"))
			{
				//MessageBox(NULL,"Setting HIDE_FAST_USER_SWITCHING succeeded","HandleSetRegistryKeyValue",16);
			}
			else
			{
				MessageBox(NULL,"Setting HIDE_FAST_USER_SWITCHING failed","HandleSetRegistryKeyValue",16);
				return FALSE;
			}
		} 
		else
		{
			MessageBox(NULL,"= false","REG_HIDE_FAST_USER_SWITCHING",16);
		}


		// Set the Windows Registry Key
		// HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\System\DisableLockWorkstation
		if (getBool("REG_DISABLE_LOCK_WORKSTATION")) 
		{
			if (!HandleSetRegistryKeyValue(hkcuSystem,VAL_DisableLockWorkstation,"DISABLE_LOCK_WORKSTATION")) return FALSE;
		}

		// Set the Windows Registry Key
		// HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\System\DisableChangePassword
		if (getBool("REG_DISABLE_CHANGE_PASSWORD")) 
		{ 			
			if (!HandleSetRegistryKeyValue(hkcuSystem,VAL_DisableChangePassword,"DISABLE_CHANGE_PASSWORD")) return FALSE;
		}


		// Set the Windows Registry Key
		// HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\System\DisableTaskMgr
		if (getBool("REG_DISABLE_TASKMGR")) 
		{
			//MessageBox(NULL,"= true","REG_DISABLE_TASKMGR",16);

			if (HandleSetRegistryKeyValue(hkcuSystem,VAL_DisableTaskMgr,"DISABLE_TASKMGR"))
			{
				//MessageBox(NULL,"Setting DISABLE_TASKMGR succeeded","HandleSetRegistryKeyValue",16);
			}
			else
			{
				MessageBox(NULL,"Setting DISABLE_TASKMGR failed","HandleSetRegistryKeyValue",16);
				return FALSE;
			}
		} 
		else
		{
			MessageBox(NULL,"= false","REG_DISABLE_TASKMGR",16);
		}


		// Set the Windows Registry Key
		// HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer\NoLogoff
		if (getBool("REG_NO_LOGOFF"))
		{ 			
			if (!HandleSetRegistryKeyValue(hkcuExplorer,VAL_NoLogoff,"NO_LOGOFF")) return FALSE;
		}

		// Set the Windows Registry Key
		// HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer\NoClose
		if (getBool("REG_NO_CLOSE"))
		{ 	
			if (!HandleSetRegistryKeyValue(hkcuExplorer,VAL_NoClose,"NO_CLOSE")) return FALSE;
		}
	}
	catch (char* str)
	{		
		MessageBox(NULL,str,"Error",MB_ICONERROR);
		return FALSE;
	}
	return TRUE;
}



BOOL ResetRegistry()
{
	HKEY hklmSystem;
	HKEY hkcuSystem;
	HKEY hkcuExplorer;

	try
	{
		if (!HandleOpenRegistryKey(HKLM, KEY_RegPolicySystem  , &hklmSystem  , TRUE)) return FALSE;
		if (!HandleOpenRegistryKey(HKCU, KEY_RegPolicySystem  , &hkcuSystem  , TRUE)) return FALSE;
		if (!HandleOpenRegistryKey(HKCU, KEY_RegPolicyExplorer, &hkcuExplorer, TRUE)) return FALSE;

		if (getBool("REG_HIDE_FAST_USER_SWITCHING")) 
		{
			RegDeleteValue(hklmSystem, VAL_HideFastUserSwitching);
		}
		if (getBool("REG_DISABLE_LOCK_WORKSTATION")) 
		{
			RegDeleteValue(hkcuSystem, VAL_DisableLockWorkstation);
		}
		if (getBool("REG_DISABLE_TASKMGR")) 
		{ 		
			RegDeleteValue(hkcuSystem, VAL_DisableTaskMgr);
		} 
		if (getBool("REG_DISABLE_CHANGE_PASSWORD")) 
		{ 			
			RegDeleteValue(hkcuSystem, VAL_DisableChangePassword);
		}
		if (getBool("REG_NO_CLOSE"))
		{ 				
			RegDeleteValue(hkcuExplorer, VAL_NoClose);
		} 
		if (getBool("REG_NO_LOGOFF"))
		{ 			
			RegDeleteValue(hkcuExplorer, VAL_NoLogoff);
		}
	}
	catch (char* str)
	{		
		MessageBox(NULL,str,"Error",MB_ICONERROR);
		return FALSE;
	}
	return TRUE;
}



BOOL CreateExternalProcess(string sProcess)
{
	//Examples find the path to winword.exe
	//char szBuf[256];
	//if (FindExecutable("D:\\Folders\\Eigene Dateien\\Vorlage.Doc", ".", szBuf ) > 31)
	{
	} 

	long counter = 0;

	//MessageBox(NULL,cmdLine.c_str(),"Error",MB_ICONERROR);
	//SHGetSpecialFolderLocation
	STARTUPINFO			siProcess;				//STARTUPINFO     for created process
	PROCESS_INFORMATION piProcess;				//PROCESS_INFORMATION created process
	ZeroMemory(&siProcess, sizeof(siProcess));
	siProcess.cb         = sizeof(siProcess);
	if (IsNewOS && getBool("NEW_DESKTOP"))
	{
		siProcess.lpDesktop = SEB_DESK;
	}
	ZeroMemory(&piProcess, sizeof(piProcess));

	// give the process 10s to start
	// after 10 s
	if (parameters.procedureReady != 0)
	{
		parameters.hold = 1;
		while(parameters.confirm != 1)
		{
			// wait for the process to sync, but max 1s
			Sleep(500);
			counter++;
			if (counter == 20) return FALSE;
		}
		parameters.hold = 0;
	}

	try
	{
		if (!CreateProcess(NULL,   // No module name (use command line). 
		  //TEXT((LPSTR)mpParam["PROCESS_CALL"].c_str()), // Command line. 
			TEXT((LPSTR)mpProcesses[sProcess].c_str()), // Command line. 
		  //TEXT("firefox.exe -profile profile -chrome chrome://kiox/content/"), // Command line. 
			NULL,             // Process handle not inheritable. 
			NULL,             // Thread handle not inheritable. 
			FALSE,            // Set handle inheritance to FALSE. 
			0,                // No creation flags. 
			NULL,             // Use parent's environment block. 
			NULL,             // Use parent's starting directory. 
			&siProcess,       // Pointer to STARTUPINFO structure.
			&piProcess)       // Pointer to PROCESS_INFORMATION structure.
		) 
		{
			ResumeThread(procMonitorThread);
			MessageBox(hWnd,PROCESS_FAILED,"Error",MB_ICONERROR);
			return FALSE;
		}

		// Add the process to the allowed processes, not required anymore
		//allowedProcesses.push_back(piProcess.dwProcessId);

		mpProcessInformations.insert(make_pair(sProcess,piProcess));
		//MessageBox(NULL,sProcess.c_str(),"Error",MB_ICONERROR);
	}
	catch (char* str)
	{	
		//ResumeThread(procMonitorThread);
		MessageBox(NULL,str,"Error",MB_ICONERROR);
		return FALSE;
	}

	//ResumeThread(procMonitorThread);
	return TRUE;
}



BOOL ShutdownInstance()
{
	//MessageBox(NULL,"shutdown","Error",MB_ICONERROR);
	DWORD dwNotUsedForAnything = 0;	
	int ret;
	string sStrongKillProcesssesAfter = ""; 
	vector< string >vStrongKillProcessesAfter;

	if (getBool("EDIT_REGISTRY") && IsNewOS)
	{
		if (!ResetRegistry())
		{
			//MessageBox(hWnd,"ShutdownInstance(): not enough rights for editing registry",REGISTRY_WARNING,MB_ICONWARNING);
			//MessageBox(hWnd,NOT_ENOUGH_REGISTRY_RIGHTS_ERROR,REGISTRY_WARNING,MB_ICONWARNING);
		}
	}

	if (getBool("MESSAGE_HOOK"))
	{
/*
		for (itProcessInformations  = mpProcessInformations.begin();
		     itProcessInformations != mpProcessInformations.end();
			 itProcessInformations++)
		{
			KeyHook(&hinstDLL,&((*itProcessInformations).second),FALSE);			
		}
*/
		   KeyHook(&hinstDLL,FALSE);
		 MouseHook(&hinstDLL,FALSE);
		FreeLibrary(hinstDLL);
	}


	// Kill all processes which are created by CreateExternalProcess()
	for (itProcessInformations  = mpProcessInformations.begin();
		 itProcessInformations != mpProcessInformations.end();
		 itProcessInformations++)
	{
		string sCommandLine = "";
		string sExecutable  = "";
		vector< string > vCommandLine;
		vector< string > vExecutable;
		GetExitCodeProcess(((*itProcessInformations).second).hProcess,&dwExitCode);
		if (dwExitCode == STILL_ACTIVE)
		{
			//of << (*itProcessInformations).first.c_str();
			//of << "\n";
			TerminateProcess(((*itProcessInformations).second).hProcess,0);
			CloseHandle(((*itProcessInformations).second).hProcess);
			CloseHandle(((*itProcessInformations).second).hThread);
		}
	}

	// shut down the proc monitor thread
	TerminateThread(procMonitorThread, 0);
	KillAllNotInList(previousProcesses, mpProcesses, true);

	sStrongKillProcesssesAfter = mpParam["STRONG_KILL_PROCESSES_AFTER"];
	if (sStrongKillProcesssesAfter != "")
	{		
		Tokenize(sStrongKillProcesssesAfter,vStrongKillProcessesAfter,";");
		//MessageBox(hWnd,vKillProcess[1].c_str(),"Error",MB_ICONWARNING);
		for (int i=0; i < (int)vStrongKillProcessesAfter.size(); i++)
		{
			ret = KILL_PROC_BY_NAME(vStrongKillProcessesAfter[i].c_str());
		}
	}

	/* known bug:
	firefox.exe still runs in NT4 Win2k after termination of the stored process (???)
	p.e. calc.exe or notepad.exe are terminated correctly (???)
	*/

	if (!IsNewOS)
	{				
		if (getBool("WIN9X_KILL_EXPLORER") && killedExplorer)
		{
			system("start explorer.exe");	
		}
		if (getBool("WIN9X_SCREENSAVERRUNNING"))
		{
			SystemParametersInfo(SPI_SCREENSAVERRUNNING,FALSE,&dwNotUsedForAnything,NULL);
		}						
	}
	else
	{
		if (getBool("NEW_DESKTOP"))
		{
			   SwitchDesktop(hOriginalInput);
			SetThreadDesktop(hOriginalThread);
				CloseDesktop(hNewDesktop);		
		}
	}	
	return TRUE;
}



//
//  FUNCTION: WndProc(HWND, unsigned, WORD, LONG)
//
//  PURPOSE:  Processes messages for the main window.
//
//  WM_COMMAND	- process the application menu
//  WM_PAINT	- Paint the main window
//  WM_DESTROY	- post a quit message and return
//
//

LRESULT CALLBACK WndProc(HWND hWnd, UINT message, WPARAM wParam, LPARAM lParam)
{
	int wmId, wmEvent, cntProcess = IDM_OFFSET;
	PAINTSTRUCT ps;
	HDC    hdc;
	HANDLE hIcon, hIconSm;

	switch (message)
	{

	case WM_CREATE:
		HMENU hMenu, hSubMenu;
		hMenu    = CreateMenu();
		hSubMenu = CreatePopupMenu();
		for (itProcesses  = mpProcesses.begin();
			 itProcesses != mpProcesses.end();
			 itProcesses++)
		{
			AppendMenu(hSubMenu, MF_STRING, cntProcess, (*itProcesses).first.c_str()); //Name of the Process	
			mpProcessCommands.insert(make_pair(cntProcess,(*itProcesses).first));		
			cntProcess ++;
		}	
		AppendMenu(hMenu, MF_STRING | MF_POPUP , (UINT)hSubMenu, "&Start");				
		SetMenu(hWnd, hMenu);
	case WM_COMMAND:
		wmId    = LOWORD(wParam);
		wmEvent = HIWORD(wParam);
		// Parse the menu selections:
		switch (wmId)
		{
		case IDM_ABOUT:
			//DialogBox(hInst, MAKEINTRESOURCE(IDD_ABOUTBOX), hWnd, About);
			break;
		case IDM_EXIT:
			DestroyWindow(hWnd);
			break;
			// supports 20 different processes
		case 37265 :
			//MessageBox(NULL,mpProcessCommands[37265][1].c_str(),"Error",MB_ICONERROR);
			CreateExternalProcess(mpProcessCommands[37265]);
			break;
		case 37266 :
			//MessageBox(NULL,mpProcessCommands[37266][1].c_str(),"Error",MB_ICONERROR);
			CreateExternalProcess(mpProcessCommands[37266]);
			break;
		case 37267 :
			//MessageBox(NULL,mpProcessCommands[37267][1].c_str(),"Error",MB_ICONERROR);
			CreateExternalProcess(mpProcessCommands[37267]);
			break;
		case 37268 :
			CreateExternalProcess(mpProcessCommands[37268]);
			break;
		case 37269 :
			CreateExternalProcess(mpProcessCommands[37269]);
			break;
		case 37270 :
			CreateExternalProcess(mpProcessCommands[37270]);
			break;
		case 37271 :
			CreateExternalProcess(mpProcessCommands[37271]);
			break;
		case 37272 :
			CreateExternalProcess(mpProcessCommands[37272]);
			break;
		case 37273 :
			CreateExternalProcess(mpProcessCommands[37273]);
			break;
		case 37274 :
			CreateExternalProcess(mpProcessCommands[37274]);
			break;
		case 37275 :
			CreateExternalProcess(mpProcessCommands[37275]);
			break;
		case 37276 :
			CreateExternalProcess(mpProcessCommands[37276]);
			break;
		case 37277 :
			CreateExternalProcess(mpProcessCommands[37277]);
			break;
		case 37278 :
			CreateExternalProcess(mpProcessCommands[37278]);
			break;
		case 37279 :
			CreateExternalProcess(mpProcessCommands[37279]);
			break;
		case 37280 :
			CreateExternalProcess(mpProcessCommands[37280]);
			break;
		case 37281 :
			CreateExternalProcess(mpProcessCommands[37281]);
			break;
		case 37282 :
			CreateExternalProcess(mpProcessCommands[37282]);
			break;
		case 37283 :
			CreateExternalProcess(mpProcessCommands[37283]);
			break;
		case 37284 :
			CreateExternalProcess(mpProcessCommands[37284]);
			break;
		case 37285 :
			CreateExternalProcess(mpProcessCommands[37285]);
			break;
		default:
			return DefWindowProc(hWnd, message, wParam, lParam);
		}
		break;
	case WM_PAINT:
		hdc = BeginPaint(hWnd, &ps);
		// TODO: Add any drawing code here...
		EndPaint(hWnd, &ps);
		break;
	case WM_DESTROY:
		ShutdownInstance();
		PostQuitMessage(0);
		break;
	default:
		return DefWindowProc(hWnd, message, wParam, lParam);
	}
	return 0;
}



// Message handler for about box.
LRESULT CALLBACK About(HWND hDlg, UINT message, WPARAM wParam, LPARAM lParam) 
{
	switch (message)
	{
	case WM_INITDIALOG:
		return TRUE;
	case WM_COMMAND:
		if (LOWORD(wParam) == IDOK || LOWORD(wParam) == IDCANCEL) 
		{
			EndDialog(hDlg, LOWORD(wParam));
			return TRUE;
		}
		break;
	}
	return FALSE;
}



/****************** Utility Functions  *********************/
BOOL CheckWritePermission(LPCSTR szPath) 
{
	DWORD dwAttr = GetFileAttributes(szPath);
	if (dwAttr == 0xffffffff)
	{
		DWORD dwError = GetLastError();
		if (dwError == ERROR_FILE_NOT_FOUND)
		{
			MessageBox(hWnd,FILE_NOT_FOUND,"Error",MB_ICONERROR);
			return FALSE;
		}
		else if (dwError == ERROR_PATH_NOT_FOUND)
		{
			MessageBox(hWnd,PATH_NOT_FOUND,"Error",MB_ICONERROR);
			return FALSE;
		}
		else if (dwError == ERROR_ACCESS_DENIED)
		{
			MessageBox(hWnd,ACCESS_DENIED,"Error",MB_ICONERROR);
			return FALSE;
		}
		else
		{
			MessageBox(hWnd,UNDEFINED_ERROR,"Error",MB_ICONERROR);
			return FALSE;
		}
	}
	else
	{
		if (dwAttr & FILE_ATTRIBUTE_DIRECTORY)
		{
			if (dwAttr & FILE_ATTRIBUTE_READONLY)
			{
				return FALSE;
			}
			// Directory is read-only
		}
		else
		{
			if (dwAttr & FILE_ATTRIBUTE_READONLY)
			{
				return FALSE;
			}
		}
	}
	return TRUE;
}



void Tokenize(const string& str, vector<string>& tokens, const string& delimiters = " ")
{
	// Skip delimiters at beginning.
	string::size_type lastPos = str.find_first_not_of(delimiters, 0);
	// Find first "non-delimiter".
	string::size_type pos     = str.find_first_of(delimiters, lastPos);

	while (string::npos != pos || string::npos != lastPos)
	{
		// Found a token, add it to the vector.
		tokens.push_back(str.substr(lastPos, pos - lastPos));
		// Skip delimiters.  Note the "not_of"
		lastPos = str.find_first_not_of(delimiters, pos);
		// Find next "non-delimiter"
		pos = str.find_first_of(delimiters, lastPos);
	}
}



string getLangString(string key)
{
	string ret = "";
	string err = "";
	try
	{
		ret = mpParam[key + "_" + mpParam["DEFAULT_LANGUAGE"]];
		if (ret == "")
		{	
			err = NO_LANGSTRING_FOUND;
			err += "\n" + key;
			MessageBox(NULL,err.c_str(),"Error",16);		
		}
		return ret;
	}
	catch( char * str )
	{		
		MessageBox(NULL,str,"Error",16);
		return "";
	}	
}



BOOL getBool(string key)
{
	BOOL ret = FALSE;
	try
	{
		ret = (atoi(mpParam[key].c_str()) > 0 ) ? TRUE : FALSE;
		return ret;
	}
	catch (char* str)
	{		
		MessageBox(NULL,str,"Error",16);
		return FALSE;
	}	
}



int getInt(string key)
{
	try
	{
		return (atoi(mpParam[key].c_str()));
	}
	catch (char* str)
	{		
		MessageBox(NULL,str,"Error",16);
		return FALSE;
	}
}



BOOL HandleOpenRegistryKey(HKEY hKey, LPCSTR subKey, PHKEY pKey, BOOL bCreate)
{
	try
	{
		long lngRegOpen;
		long lngRegCreate;

		lngRegOpen = RegOpenKey(hKey, subKey, pKey);

		//MessageBox(NULL,subKey,"RegOpenKey(hKey, subKey, pKey)",16);

		// What the heck is meant by "ERROR_SUCCESS"???
		// Was the operation a success or did it produce an error?
		// This makes absolutely no sense!

		if (lngRegOpen == ERROR_SUCCESS) //MessageBox(hWnd," = ERROR_SUCCESS","lngRegOpen",16);
		if (lngRegOpen != ERROR_SUCCESS)   MessageBox(hWnd,"!= ERROR_SUCCESS","lngRegOpen",16);

		if (lngRegOpen != ERROR_SUCCESS)
		{
			switch (lngRegOpen)
			{
				case ERROR_SUCCESS:
					//MessageBox(hWnd,"HandleOpenRegistryKey(): Yes, enough rights for editing registry 1","Registry enough rights 1",16);
					break;
				case ERROR_FILE_NOT_FOUND :
					if (bCreate)
					{
						lngRegCreate = RegCreateKey(hKey,subKey,pKey);
						switch (lngRegCreate)
						{
							case ERROR_SUCCESS:
								//MessageBox(hWnd,"HandleOpenRegistryKey(): Yes, enough rights for editing registry 2","Registry enough rights 2",16);
								break;
							case ERROR_ACCESS_DENIED :
							  //MessageBox(hWnd,"HandleOpenRegistryKey(): Not enough rights for editing registry",REGISTRY_WARNING,MB_ICONWARNING);
							  //MessageBox(hWnd,NOT_ENOUGH_REGISTRY_RIGHTS_ERROR,REGISTRY_WARNING,MB_ICONWARNING);
								return FALSE;
								break;
							default :
								return FALSE;
						}
					}
					break;
				default :
					return FALSE;
			} // end switch
		} // end if
	} // end try
	catch (char* str)
	{		
		MessageBox(NULL,str,"Error",16);
		return FALSE;
	}
	return TRUE;
}



BOOL HandleSetRegistryKeyValue(HKEY hKey, LPCSTR lpVal, string sParam)
{
	DWORD type;
	DWORD dwLen = sizeof(DWORD);
	DWORD val   = 1;

	long lngRegSet;
	long lngRegGet;

	try
	{
		lngRegGet = RegQueryValueEx(hKey,lpVal,NULL,&type,(LPBYTE)&val,&dwLen);
		if (lngRegGet == ERROR_SUCCESS)
		{
			//is already set. don't touch this key value in the ResetRegistry function
			mpParam[sParam] = "0";
		}
		else
		{
			lngRegSet = RegSetValueEx(hKey, lpVal, NULL, REG_DWORD, (BYTE*)&val, sizeof(val));
			switch (lngRegSet)
			{
				case ERROR_SUCCESS:
					break; 
				case ERROR_ACCESS_DENIED :
					mpParam[sParam] = "0";
				  //MessageBox(hWnd,"HandleSetRegistryKeyValue(): not enough rights for editing registry",REGISTRY_WARNING,MB_ICONWARNING);
				  //MessageBox(hWnd,NOT_ENOUGH_REGISTRY_RIGHTS_ERROR,REGISTRY_WARNING,MB_ICONWARNING);
					return FALSE;
				default :
					mpParam[sParam] = "0";
					return FALSE;
			}
		}
	}
	catch (char* str)
	{		
		MessageBox(NULL,str,"Error",16);
		return FALSE;
	}
	return TRUE;
}



//************************************
//* Only in trunk version (XUL-Runner)
//************************************

BOOL SetVersionInfo() 
{
	switch (sysVersionInfo.GetVersion())
	{			
	case WIN_NT_351 :
	case WIN_NT_40  :
	case WIN_2000  :
	case WIN_XP    :
	case WIN_VISTA :
		IsNewOS = TRUE;
		return TRUE;
		break;
	case WIN_95 :
	case WIN_98 :
	case WIN_ME :
		IsNewOS = FALSE;
		return TRUE;
		break;
	default :
		return FALSE;			
	}
}



BOOL ShowSebAppChooser()
{
	BOOL retVal = FALSE;
	BOOL inReg  = FALSE;
	try
	{
		HKEY  hKeySEB;
		DWORD showSEBAppChooser;
		DWORD dwType =    REG_DWORD;
		DWORD dwSize = sizeof(DWORD);
		LONG returnStatus;
		if (HandleOpenRegistryKey(HKLM, KEY_RegPolicySEB, &hKeySEB, FALSE))
		{
			returnStatus = RegQueryValueEx(hKeySEB, VAL_ShowSEBAppChooser, NULL, &dwType,(LPBYTE)&showSEBAppChooser, &dwSize);
			if (returnStatus == ERROR_SUCCESS)
			{
				inReg  = TRUE;
				retVal = showSEBAppChooser;
			}
			RegCloseKey(hKeySEB);
		}
	}
	catch (char* str)
	{
		MessageBox(NULL, str, "Error", MB_ICONERROR);
	}
	if (!inReg)
	{
		retVal = getBool("SHOW_SEB_APP_CHOOSER");
	}
	return retVal;
}
