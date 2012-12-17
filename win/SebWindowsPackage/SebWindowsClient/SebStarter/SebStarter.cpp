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
*   Stefan Schneider, Uni Hamburg, <stefan.schneider@uni-hamburg.de>
*   Oliver Rahs     , ETH Zurich , <rahs@net.ethz.ch>
*   Dirk Bauer      , ETH Zurich , <dirk.bauer@let.ethz.ch>
*   Jan Derriks     , Hogeschool van Amsterdam, <j.derriks@hva.nl>
*
* ***** END LICENSE BLOCK ***** */

//
// SebStarter.cpp:
// Defines the SEB main application,
// starting the kiosk mode and eventually the XULRunner browser.
//

#include "stdafx.h"
#include "SebStarter.h"
#include "KillProc.h"
#include "ProcMonitor.h"
#include "../ErrorMessage.h"   // multilingual (German, English, French)

#include <intrin.h>   // for detecting virtual machines

//JAND
#include <time.h>
#include "sha1.h"

// C structures for logfile handling
extern char  programDataDirectory[MAX_PATH];
extern char localAppDataDirectory[MAX_PATH];
extern bool logFileDesiredMsgHook;
extern bool logFileDesiredSebStarter;
extern char logFileDirectory [BUFLEN];
extern char logFileMsgHook   [BUFLEN];
extern char logFileSebStarter[BUFLEN];
extern char iniFileDirectory [BUFLEN];
extern char iniFileMsgHook   [BUFLEN];
extern char iniFileSebStarter[BUFLEN];
extern char examUrl          [BUFLEN];
extern char quitPassword     [BUFLEN];
extern char quitHashcode     [BUFLEN];
extern FILE* fp;

// Function for easier writing into the logfile
#define logg if (fp != NULL) fprintf

extern int languageIndex;
extern int    errorIndex;

// Global arrays for messages in different languages
extern LPCSTR languageString  [IND_LanguageNum];
extern LPCSTR   messageText   [IND_LanguageNum][IND_MessageTextNum];
extern LPCSTR   messageCaption[IND_LanguageNum][IND_MessageKindNum];
extern int      messageIcon                    [IND_MessageKindNum];

//extern void DefineErrorMessages();
//extern int  GetCurrentLanguage();
//extern void OutputErrorMessage(int languageIndex, int messageTextIndex, int messageKindIndex);



//These usings are only working in .NET, not in Standard C++:
//using namespace System::Security::Principal;
//using System.Security.Principal;


/* Forward declarations of functions included in this code module: */
LRESULT CALLBACK	WndProc(HWND,  UINT, WPARAM, LPARAM);
LRESULT	CALLBACK	LLKeyboardHook( int, WPARAM, LPARAM);
LRESULT	CALLBACK	  KeyboardHook( int, WPARAM, LPARAM);
LRESULT CALLBACK	  About(HWND,  UINT, WPARAM, LPARAM);
LRESULT CALLBACK    EnterQuitPasswordProc(HWND,  UINT, WPARAM, LPARAM);

ATOM				MyRegisterClass(HINSTANCE hInstance);
BOOL				InitInstance   (HINSTANCE, int);
BOOL				ReadSebStarterIni();
BOOL				ReadProcessesInRegistry();
BOOL				ShowSebApplicationChooser();

BOOL				OpenSocket();
BOOL				CloseSocket();
int                 DetermineUserSid(char*);
int					SendEquationToSocketServer(char*, char*, int);
int					RecvEquationOfSocketServer(char*, char*, int);

BOOL				GetClientInfo();
BOOL				AlterTaskBar(BOOL);
BOOL				MessageHook (BOOL);
BOOL				CreateExternalProcess(string);
BOOL				ShutdownInstance();						//cleaning up and resetting altered system before destroying the window
VOID				MonitorProcesses(threadParameters & parameters);

typedef void (*KEYHOOK)(HINSTANCE*, bool); //typedef for the KeyHook function of the loaded MsgHook.dll
KEYHOOK KeyHook;

typedef void (*MOUSEHOOK)(HINSTANCE*, bool); //typedef for the MouseHook function of the loaded MsgHook.dll
MOUSEHOOK MouseHook;

typedef void (*ITOPIATEST)(HINSTANCE*, bool); 
ITOPIATEST ItopiaTest;

/* Utility Functions */
BOOL				CheckWritePermission(LPCSTR);
VOID				Tokenize(const string&, vector<string>&, const string&);
BOOL				killedExplorer;
BOOL				getBool      (string);
string				getLangString(string);
int					getInt       (string);
BOOL				HandleOpenRegistryKey(HKEY, LPCSTR, HKEY*, BOOL);
//BOOL				HandleSetRegistryKeyValue(HKEY, LPCSTR, string);
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
HANDLE    procMonitorThread;



// Socket structures for IPC (interprocess communication)
// between SEB (client) and Windows service (server)

static WORD     wVersionRequested;
//static int    hostRes;
static PHOSTENT hostInfo;
static WSADATA  wsaData;
static SOCKET   ConnectSocket = INVALID_SOCKET;
static struct   hostent*    remoteHost;
static struct   in_addr     addr;
static struct   sockaddr_in clientService;
static int      socketResult;
static char     sendBuffer[BUFLEN];
static char     recvBuffer[BUFLEN];


// Socket protocol
static int ai_family   = AF_INET;
static int ai_socktype = SOCK_STREAM;
static int ai_protocol = IPPROTO_TCP;

const char* endOfStringKeyWord = "---SEB---";

static char*   defaultUserName     = "";
static char*   defaultHostName     = "localhost";
static int     defaultPortNumber   = 57016;
static int     defaultSendInterval = 100;
static int     defaultRecvTimeout  = 100;
static int     defaultNumMessages  = 3;

static char    userNameRegistryFlags[100];
static char    registryFlags[50];
static char*   hostName     = "";
static char*   userName     = "";
static char    userSid[512];
static int     portNumber   = 0;
static int     sendInterval = 0;
static int     recvTimeout  = 0;
static int     numMessages  = 0;
static int     messageNr    = 0;

// Store the desired Blue Screen inside SEB options as integers
static int intInsideSebEnableSwitchUser        = 0;
static int intInsideSebEnableLockThisComputer  = 0;
static int intInsideSebEnableChangeAPassword   = 0;
static int intInsideSebEnableStartTaskManager  = 0;
static int intInsideSebEnableLogOff            = 0;
static int intInsideSebEnableShutDown          = 0;
static int intInsideSebEnableEaseOfAccess      = 0;
static int intInsideSebEnableVmWareClientShade = 0;

// Store the desired Blue Screen outside SEB options as integers
static int intOutsideSebEnableSwitchUser        = 1;
static int intOutsideSebEnableLockThisComputer  = 1;
static int intOutsideSebEnableChangeAPassword   = 1;
static int intOutsideSebEnableStartTaskManager  = 1;
static int intOutsideSebEnableLogOff            = 1;
static int intOutsideSebEnableShutDown          = 1;
static int intOutsideSebEnableEaseOfAccess      = 1;
static int intOutsideSebEnableVmWareClientShade = 1;

// Store the desired Blue Screen inside SEB options as strings
static char stringInsideSebEnableSwitchUser       [10];
static char stringInsideSebEnableLockThisComputer [10];
static char stringInsideSebEnableChangeAPassword  [10];
static char stringInsideSebEnableStartTaskManager [10];
static char stringInsideSebEnableLogOff           [10];
static char stringInsideSebEnableShutDown         [10];
static char stringInsideSebEnableEaseOfAccess     [10];
static char stringInsideSebEnableVmWareClientShade[10];

// Store the desired Blue Screen outside SEB options as strings
static char stringOutsideSebEnableSwitchUser       [10];
static char stringOutsideSebEnableLockThisComputer [10];
static char stringOutsideSebEnableChangeAPassword  [10];
static char stringOutsideSebEnableStartTaskManager [10];
static char stringOutsideSebEnableLogOff           [10];
static char stringOutsideSebEnableShutDown         [10];
static char stringOutsideSebEnableEaseOfAccess     [10];
static char stringOutsideSebEnableVmWareClientShade[10];



TCHAR szTitle      [MAX_LOADSTRING];		// The title bar text
TCHAR szWindowClass[MAX_LOADSTRING];		// the main window class name
SystemVersionInfo sysVersionInfo;				
BOOL IsNewOS = FALSE;
BOOL b1, b2, b3;
char  cHostName[255];						//char with hostname
char  cUserName[255];						//char with username
char* cIp;									//Pointer to hostname with IP Address
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
	char currentSebStarterDir[MAX_PATH];
	char currentSebStarterIni[MAX_PATH];
	char currentSebStarterLog[MAX_PATH];
	bool sebLightVersion = false;
	ifstream inputStream;

	MSG msg;
	HACCEL hAccelTable;

	// Initialize global strings
	LoadString     (hInstance, IDS_APP_TITLE, szTitle      , MAX_LOADSTRING);
	LoadString     (hInstance, IDC_SEB      , szWindowClass, MAX_LOADSTRING);
	MyRegisterClass(hInstance);
	DWORD dwRet = 0; 

	// By default, a logfile should be written
	logFileDesiredSebStarter = true;

	// Initialise socket variables
	userName     = defaultUserName;
	hostName     = defaultHostName;
	portNumber   = defaultPortNumber;
	sendInterval = defaultSendInterval;
	recvTimeout  = defaultRecvTimeout;
	numMessages  = defaultNumMessages;

	// Determine the location of the .ini files
	SetIniFileDirectoryAndName();
	//MessageBox(NULL, iniFileSebStarter, "iniFileSebStarter", MB_ICONERROR);

	GetCurrentDirectory(MAX_PATH, currentSebStarterDir);

	strcpy(currentSebStarterIni, currentSebStarterDir);
	strcat(currentSebStarterIni, "\\");
	strcat(currentSebStarterIni, SEB_STARTER_INI);

	strcpy(currentSebStarterLog, currentSebStarterDir);
	strcat(currentSebStarterLog, "\\");
	strcat(currentSebStarterLog, SEB_STARTER_LOG);

	//MessageBox(NULL, currentSebStarterDir, "currentSebStarterDir", MB_ICONERROR);
	//MessageBox(NULL, currentSebStarterIni, "currentSebStarterIni", MB_ICONERROR);
	//MessageBox(NULL, currentSebStarterLog, "currentSebStarterLog", MB_ICONERROR);


	// Test if ProgramData contains a SebStarter.ini file
	//logg(fp, "Try to open ini file %s\n", iniFileSebStarter);
	inputStream.open(iniFileSebStarter);

	// If ProgramData contains no SebStarter.ini file
	if (!inputStream.is_open())
	{
		// Test if current directory contains a SebStarter.ini file
		//logg(fp, "Try to open ini file %s\n", currentSebStarterIni);
		inputStream.open(currentSebStarterIni);

		// If yes, change the paths to the SebStarter.ini and SebStarter.log files
		if (inputStream.is_open())
		{
			sebLightVersion = true;
		}
		// If none of these directories contains a SebStarter.ini file, give up
		else
		{
//			OutputErrorMessage(languageIndex, IND_SebStarterIniError, IND_MessageKindError);
			//MessageBox(NULL, messageText[languageIndex][IND_SebStarterIniError], "Error", 16);
			//logg(fp, "Error: %s\n", messageText[languageIndex][IND_SebStarterIniError]);
			//logg(fp, "Leave _tWinMain() and return FALSE\n\n");
//			return FALSE;
		}
	}

	inputStream.close();


	// Get the current username
	DWORD cUserNameLen =      sizeof(cUserName);
	BOOL   user        = GetUserName(cUserName, &cUserNameLen);

if (cUserName == NULL)
{
		//MessageBox(NULL, "is NULL", "userName", MB_ICONERROR);
}
else
{
	userName = cUserName;
	//MessageBox(NULL,    userName, "   userName", MB_ICONERROR);

	// Open or create a logfile for SebStarter.exe
	if (fp == NULL)
	{
		// Determine the location of the .log files
		SetLogFileDirectoryAndName();

		if (sebLightVersion == true)
		{
			strcpy(iniFileSebStarter, currentSebStarterIni);
			strcpy(logFileSebStarter, currentSebStarterLog);
		}

		// MessageBox(NULL, iniFileSebStarter, "iniFileSebStarter", MB_ICONERROR);
		// MessageBox(NULL, logFileSebStarter, "logFileSebStarter", MB_ICONERROR);
		// Open the logfile for debug output
		fp = fopen(logFileSebStarter, "w");
	}

	if (fp == NULL)
	{
		//MessageBox(NULL, logFileSebStarter, "_tWinMain(): Could not open logfile SebStarter.log", MB_ICONERROR);
	}

	logg(fp, "\n");
	logg(fp, " userName     = %s\n",  userName);
	logg(fp, "cUserName     = %s\n", cUserName);
	logg(fp, "cUserNameLen  = %d\n", cUserNameLen);
	logg(fp, "\n");
	logg(fp, "   programDataDirectory = %s\n",  programDataDirectory);
	logg(fp, "  localAppDataDirectory = %s\n", localAppDataDirectory);
	logg(fp, "\n");
	logg(fp, "  iniFileDirectory     = %s\n", iniFileDirectory);
	logg(fp, "  logFileDirectory     = %s\n", logFileDirectory);
	logg(fp, "  iniFileSebStarter    = %s\n", iniFileSebStarter);
	logg(fp, "  logFileSebStarter    = %s\n", logFileSebStarter);
	logg(fp, "\n");
}


	// Initialise the error messages in different languages
	DefineErrorMessages();

	// Get the current language
	languageIndex = GetCurrentLanguage();
	//OutputErrorMessage(languageIndex, IND_RegistryEditError, IND_MessageKindError);
	//OutputErrorMessage(languageIndex, IND_RegistryWarning  , IND_MessageKindWarning);


	//MessageBox(NULL, "Starting SEB...", "tWinMain():", MB_ICONERROR);

	logg(fp, "\n");
	logg(fp, "\n");
	logg(fp, "Enter _tWinMain()\n\n");
	logg(fp, "\n");
	logg(fp, "languageIndex  = %d\n", languageIndex);
	logg(fp, "languageString = %s\n", languageString[languageIndex]);
	logg(fp, "\n");


	// Perform application initialization:
	//InitInstance (hInstance, nCmdShow);
	/*
	{
		OutputErrorMessage(languageIndex, IND_InitialiseError, IND_MessageKindError);
		//MessageBox(hWnd, INITIALIZE_ERROR, "Error", MB_ICONERROR);
		//logg(fp, "Error: %s\n", INITIALIZE_ERROR);

		logg(fp, "Leave _tWinMain()\n\n");
		// Close the logfile for debug output
		logg(fp, "Closing the logfile...()\n\n");
		if (fp != NULL) fclose(fp);
		return -1;
	}
	*/


	if (!InitInstance (hInstance, nCmdShow))
	{
		OutputErrorMessage(languageIndex, IND_InitialiseError, IND_MessageKindError);
		//MessageBox(hWnd, INITIALIZE_ERROR, "Error", MB_ICONERROR);
		//logg(fp, "Error: %s\n", INITIALIZE_ERROR);

		if (forceWindowsService)
		{
			logg(fp, "Windows Service could not be forced, should I exit now?\n");
		}

		if ((runningOnVirtualMachine == true) && (allowVirtualMachine == false))
		{
			logg(fp, "Attempt to run SEB on a virtual machine, should I exit now?\n");
		}

		logg(fp, "Exiting the SEB program...\n");
		//ShutdownInstance();
		//SendMessage(hWndCaller,WM_DESTROY,NULL,NULL);

		logg(fp, "Leave _tWinMain()\n\n");
		// Close the logfile for debug output
		logg(fp, "Closing the logfile...()\n\n");
		if (fp != NULL) fclose(fp);
		return -1;
		//return (int) msg.wParam;
	}

	logg(fp, "InitInstance was successful()\n\n");

	hAccelTable = LoadAccelerators(hInstance, (LPCTSTR)IDC_SEB);
	PROCESS_INFORMATION pi;
	string shutDownProcess = mpParam["AutostartProcess"];

	
	/* main message loop */
	/*
	If the autostart process is finished it signals and closes SEB.
	It does not work if p.e. firefox writes something into the profile at startup process. 
	(firefox possibly finishes and starts itself in a new process.) 
	In this case "SHUTDOWN_AFTER_PROCESS_TERMINATES" should be empty (!)
	and the only way to finish SEB is the defined Hotkey.
	*/

	if (getBool("ShutdownAfterAutostartProcessTerminates") && shutDownProcess != "")
	{
		pi = mpProcessInformations[shutDownProcess];
		while (1)
		{
			dwRet = MsgWaitForMultipleObjects(1, &(pi.hProcess), FALSE, INFINITE, QS_ALLINPUT);
			if (dwRet == WAIT_OBJECT_0)
			{
				//MessageBox(NULL,"1","Error",MB_ICONERROR);
				logg(fp, "Error 1\n");
				logg(fp, "Leave _tWinMain()\n\n");
				SendMessage(hWnd,WM_DESTROY,NULL,NULL);
				return TRUE;    // The event was signaled
			}

			if (dwRet != WAIT_OBJECT_0 + 1)
			{
				// Something else happened
				//MessageBox(NULL,"2","Error",MB_ICONERROR);
				logg(fp, "Error 2\n");
				break;
			}

			// There is one or more window message available. Dispatch them

			while (PeekMessage(&msg,NULL,NULL,NULL,PM_REMOVE))
			{
				if (!TranslateAccelerator(msg.hwnd, hAccelTable, &msg))
				{
					TranslateMessage(&msg);
					 DispatchMessage(&msg);
				}
				if (WaitForSingleObject(pi.hProcess, 0) == WAIT_OBJECT_0)
				{
					//MessageBox(NULL,"3","Error",MB_ICONERROR);
					logg(fp, "Error 3\n");
					logg(fp, "Leave _tWinMain()\n\n");
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

	logg(fp, "Leave _tWinMain()\n\n");
	return (int) msg.wParam;

} // end _tWinMain()





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
	int  ret;
	char buffer[100];

	// It is important to set the CurrentDirectory
	// to the AppDir if you call the App from a Link
	char szAppPath[MAX_PATH] = "";
	string strAppDirectory;
	GetModuleFileName(0, szAppPath, sizeof(szAppPath) - 1);

	// Extract directory
	strAppDirectory   =  szAppPath;
	strAppDirectory   = strAppDirectory.substr(0, strAppDirectory.rfind("\\"));
	SetCurrentDirectory(strAppDirectory.c_str());

	logg(fp, "Enter InitInstance()\n");

	//MessageBox(NULL, strAppDirectory.c_str(), "InitInstance(): strAppDirectory", MB_ICONERROR);
	logg(fp, "strAppDirectory = %s\n\n", strAppDirectory.c_str());

	if (!ReadSebStarterIni())
	{
		OutputErrorMessage(languageIndex, IND_SebStarterIniError, IND_MessageKindError);
		logg(fp, "Leave InitInstance()\n\n");
		return FALSE;
	}

/*
	if (getBool("CHECK_WRITE_PERMISSION") && (!CheckWritePermission("\\\\Three\\kiox_dev\\kiox_clients\\windows\\WinKeyox\\WinKeyox\\Release\\WinKeyox.ini")))
	{
		OutputErrorMessage(languageIndex, IND_NoWritePermission, IND_MessageKindError);
		//MessageBox(NULL, NO_WRITE_PERMISSION, "Error", MB_ICONERROR);
		//logg(fp, "Error: %s\n", NO_WRITE_PERMISSION_ERROR);
		logg(fp, "Leave InitInstance()\n\n");
		return FALSE;
	}
*/

	// Trunk version (XUL-Runner)
	if (!SetVersionInfo())
	{
		OutputErrorMessage(languageIndex, IND_NoOsSupport, IND_MessageKindError);
		//MessageBox(NULL, NO_OS_SUPPORT, "Error", 16);
		//logg(fp, "Error: %s\n", NO_OS_SUPPORT);
		logg(fp, "Leave InitInstance()\n\n");
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
			logg(fp, "IsNewOS = TRUE\n");
			break;
		case WIN_95 :
		case WIN_98 :
		case WIN_ME :
			IsNewOS = FALSE;
			break;
		default :
			OutputErrorMessage(languageIndex, IND_NoOsSupport, IND_MessageKindError);
			//MessageBox(NULL,NO_OS_SUPPORT,"Error",16);
			//logg(fp, "Error: %s\n", NO_OS_SUPPORT);
			logg(fp, "Leave InitInstance()\n\n");
			return FALSE;
	}
*/


	// locks OS
	if (!IsNewOS)
	{
		//just kill explorer.exe on Win9x / Me
		if (getBool("Win9xKillExplorer"))
		{
			logg(fp, "Calling  KILL_PROC_BY_NAME(explorer.exe)\n");
			ret = KILL_PROC_BY_NAME("explorer.exe");
			if (ret != 0)
			{
				sprintf(buffer, messageText[languageIndex][IND_KillProcessFailed], "explorer.exe", ret);
				//MessageBox(NULL, buffer, "Error", 16);
				logg(fp, "Error: %s\n", buffer);
				killedExplorer = FALSE;
			}
			else
			{
				killedExplorer = TRUE;
			}
		}
		//tell Win9x / Me that the screensaver is running to lock system tasks
		if (getBool("Win9xScreenSaverRunning"))
		{
			SystemParametersInfo(SPI_SCREENSAVERRUNNING, TRUE, &dwNotUsedForAnything, NULL);
		}
	}
	else
	{
		//on NT4/NT5 a new desktop is created
		if (getBool("CreateNewDesktop"))
		{
			hOriginalThread = GetThreadDesktop(GetCurrentThreadId());
			hOriginalInput  = OpenInputDesktop(0, FALSE, DESKTOP_SWITCHDESKTOP);

			// Create a new Desktop and switch to it
			hNewDesktop = CreateDesktop(SEB_DESK, NULL, NULL, 0, GENERIC_ALL, NULL);
			SetThreadDesktop(hNewDesktop);
			   SwitchDesktop(hNewDesktop);
		}		
	}

	// start the SEB window
	hWnd = CreateWindow(szWindowClass, szTitle, WS_MAXIMIZE, 10, 10, 200, 55, NULL, NULL, hInstance, NULL);

	if (!hWnd)
	{
		OutputErrorMessage(languageIndex, IND_InitialiseError, IND_MessageKindError);
		//MessageBox(NULL, INITIALIZE_ERROR, "Error", MB_ICONERROR);
		//logg(fp, "Error: %s\n", INITIALIZE_ERROR);
		logg(fp, "Leave InitInstance()\n\n");
		return FALSE;
	}

	if (!GetClientInfo())
	{
		OutputErrorMessage(languageIndex, IND_NoClientInfoError, IND_MessageKindError);
		//MessageBox(NULL, NO_CLIENT_INFO_ERROR, "Error", MB_ICONERROR);
		//logg(fp, "Error: %s\n", NO_CLIENT_INFO_ERROR);
		logg(fp, "Leave InitInstance()\n\n");
		return FALSE;
	}

	sStrongKillProcessesBefore = mpParam["StrongKillProcessesBefore"];
	if (sStrongKillProcessesBefore != "")
	{		
		Tokenize(sStrongKillProcessesBefore, vStrongKillProcessesBefore, ";");
		//MessageBox(hWnd, vKillProcess[1].c_str(), "Error", MB_ICONWARNING);
		//logg(fp, "Error: %s\n", vKillProcess[1].c_str());
		logg(fp, "\n");
		for (int i = 0; i < (int)vStrongKillProcessesBefore.size(); i++)
		{
			logg(fp, "KILL_PROC_BY_NAME(%s) Before\n", vStrongKillProcessesBefore[i].c_str());
			ret = KILL_PROC_BY_NAME(vStrongKillProcessesBefore[i].c_str());
		}
	}

	if (mpParam["AutostartProcess"] != "")
	{
		if (!CreateExternalProcess(mpParam["AutostartProcess"]))
		{
			OutputErrorMessage(languageIndex, IND_ProcessCallFailed, IND_MessageKindWarning);
			//MessageBox(hWnd, PROCESS_FAILED, "Error", MB_ICONERROR);
			//logg(fp, "Error: %s\n", PROCESS_FAILED);
			logg(fp, "Leave InitInstance()\n\n");
			return FALSE;
		}		
	}

	/* setting the HOOK */

	if (getBool("HookMessages"))
	{
		if (hinstDLL == NULL) 
		{	
			hinstDLL = LoadLibrary((LPCTSTR) mpParam["HookDll"].c_str());
		}

		if (hinstDLL == NULL) 
		{
			OutputErrorMessage(languageIndex, IND_LoadLibraryError, IND_MessageKindError);
			//MessageBox(NULL, LOAD_LIBRARY_ERROR, "Error", 16);
			//logg(fp, "Error: %s\n", LOAD_LIBRARY_ERROR);
			logg(fp, "Leave InitInstance()\n\n");
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


	//	if (GetProcAddress(hinstDLL,   "ItopiaTest") == 0)
	//	{
	//		MessageBox(NULL, "DLL problem", "Error", 16);
	//	}
	}

	// Show Window	
	if (ShowSebApplicationChooser())
	{
		// Assure that the new desktop has been created
		// BEFORE the AppChooser window starts up,
		// so the AppChooser window is NOT hided at start.
		// Since it takes a few seconds for the new desktop to appear,
		// wait for 3 seconds (3000 milliseconds)
		// before launching the AppChooser window.
		// But this is a hack and sometimes does not work,
		// put the sleep() command in a comment only.

		// Sleep(3000);

		int cmd = (getBool("AutostartProcess")) ? SW_SHOWNORMAL : SW_SHOWNORMAL; //Not very suggestive yet

		  ShowWindow(hWnd, cmd);
		UpdateWindow(hWnd);
	}

	// This is not the set of allowed processes to run & the processes in the list process
	// Why "not"???
	GetRunningProcesses(previousProcesses);
	allowedProcesses.insert(allowedProcesses.end(), previousProcesses.begin(), previousProcesses.end());


	// Print the previous / allowed processes into the logfile

	vector<long>::iterator previousIterator;
	vector<long>::iterator  allowedIterator;
	long   previousNr;
	long    allowedNr;
	long   previousID;
	long    allowedID;
	string previousName;
	string  allowedName;
  //char previousString[260];
  //char  allowedString[260];


	// Print the previous processes into the logfile
	logg(fp, "\n");
	logg(fp, " Nr\t   ID\tpreviousProcess\n");
	logg(fp, "----------------------------------\n");
	previousNr = 0;
	for (previousIterator  = previousProcesses.begin();
		 previousIterator != previousProcesses.end();
		 previousIterator++)
	{
		previousNr++;
		previousID   = *previousIterator;
		previousName =  GetProcessNameFromID(previousID);
		logg(fp, "%3d\t", previousNr);
		logg(fp, "%5d\t", previousID);
		logg(fp, "% s\n", previousName.c_str());
	}
	logg(fp, "\n\n");


	// Print the allowed processes into the logfile
	logg(fp, "\n");
	logg(fp, " Nr\t   ID\tallowedProcess\n");
	logg(fp, "----------------------------------\n");
	allowedNr = 0;
	for (allowedIterator  = allowedProcesses.begin();
		 allowedIterator != allowedProcesses.end();
		 allowedIterator++)
	{
		allowedNr++;
		allowedID   = *allowedIterator;
		allowedName =  GetProcessNameFromID(allowedID);
		logg(fp, "%3d\t", allowedNr);
		logg(fp, "%5d\t", allowedID);
		logg(fp, "% s\n", allowedName.c_str());
	}
	logg(fp, "\n\n");


	long threadID;

	parameters.allowedProcesses = &allowedProcesses;
	parameters.mpProcesses      = mpProcesses; // Only used in Branches, not in Tags!
	parameters.desktop          = hNewDesktop;
	parameters.hold             = 0;
	parameters.confirm          = 0;

	if (getBool("MonitorProcesses"))
	{
		procMonitorThread = CreateThread(NULL, 0, (LPTHREAD_START_ROUTINE)MonitorProcesses, (LPVOID)&parameters, 0, (LPDWORD)&threadID);
		parameters.procedureReady = 1;
	}

	logg(fp, "Leave InitInstance()\n\n");
	return TRUE;

} // end InitInstance()





// ***************************************
//  Wrapper for cryptic __cpuid() function
// ***************************************
void GetInfoAboutCPU(int infoType, PUINT eax, PUINT ebx, PUINT ecx, PUINT edx)
{
	int CPUinfo[4];

	logg(fp, "   Enter GetCPUID()\n");

	__cpuid(CPUinfo, infoType);

	*eax = CPUinfo[0];
	*ebx = CPUinfo[1];
	*ecx = CPUinfo[2];
	*edx = CPUinfo[3];

	logg(fp, "      infoType = %d   CPUinfo[0] = %0.8x   eax = %0.8x\n", infoType, CPUinfo[0], eax);
	logg(fp, "      infoType = %d   CPUinfo[1] = %0.8x   ebx = %0.8x\n", infoType, CPUinfo[1], ebx);
	logg(fp, "      infoType = %d   CPUinfo[2] = %0.8x   ecx = %0.8x\n", infoType, CPUinfo[2], ecx);
	logg(fp, "      infoType = %d   CPUinfo[3] = %0.8x   edx = %0.8x\n", infoType, CPUinfo[3], edx);

	logg(fp, "   Leave GetTheCPUID()\n\n");
	return;

} // end of method   GetInfoAboutCPU()





// **********************************************
//  Checks if SEB is running on a virtual machine
// **********************************************
bool IsSebRunningOnVirtualMachineOld()
{
	bool    virtualMachine = false;
	char      vendorID[50] = "";
	char hyperVendorID[50] = "";

	UINT eax, ebx, ecx, edx;
	UINT ecxBit31;

	logg(fp, "Enter IsSebRunningOnVirtualMachineOld()\n\n");


	// Get the vendor ID string via parameter EAX = 0000_0000h
	GetInfoAboutCPU(0x0, &eax, &ebx, &ecx, &edx);

	// ATTENTION: the order ebx, edx, ecx (rather than ebx, ecx, edx) is NO typo -
	// this is the correct order for getting the hyper vendor string!!!
	memcpy(vendorID + 0, &ebx, 4);
	memcpy(vendorID + 4, &edx, 4);
	memcpy(vendorID + 8, &ecx, 4);
	vendorID[49] = '\0';
	logg(fp, "   vendorID = %s\n\n", vendorID);


	// Get the "hypervisor present bit" by parameter EAX = 0000_0001h
	// The "hypervisor present bit", if set to 1, indicates a virtual machine.
	GetInfoAboutCPU(0x1, &eax, &ebx, &ecx, &edx);

	ecxBit31 = ((ecx & 0x80000000) >> 31);

	logg(fp, "   (ecx & 0x80000000)        = %0.8x\n",  (ecx & 0x80000000));
	logg(fp, "   (ecx & 0x80000000)  >> 31 = %0.8x\n", ((ecx & 0x80000000) >> 31));
	logg(fp, "    ecxBit31                 = %0.8x\n",   ecxBit31);
	logg(fp, "\n");

	// If  (bit 31 of register ECX is set),
	// SEB is probably running in a virtual machine

	if (ecxBit31 == 1) virtualMachine = true;
				  else virtualMachine = false;

	if (virtualMachine == true)
	{
		logg(fp, "   Yes, bit 31 of register ECX is INDEED set!\n");
		logg(fp, "   SEB seems to run on a VIRTUAL machine!\n\n");
	}
	else
	{
		logg(fp, "   No, bit 31 of register ECX is NOT set!\n");
		logg(fp, "   SEB seems to run on a PHYSICAL machine!\n\n");
	}


	// Get the hyper vendor ID
	if (virtualMachine == true)
	{
		// Get the vendor ID string via parameter EAX = 4000_0000h
		GetInfoAboutCPU(0x40000000, &eax, &ebx, &ecx, &edx);

		// ATTENTION: the order ebx, edx, ecx (rather than ebx, ecx, edx) is NO typo -
		// this is the correct order for getting the hyper vendor string!!!
		memcpy(hyperVendorID + 0, &ebx, 4);
		memcpy(hyperVendorID + 4, &edx, 4);
		memcpy(hyperVendorID + 8, &ecx, 4);
		hyperVendorID[49] = '\0';
		logg(fp, "   hyperVendorID = %s\n\n", hyperVendorID);

		// Success - running under VMware
		//if (!strcmp(hyperVendorID, "VMwareVMware")) return 1;
	}

	logg(fp, "Leave IsSebRunningOnVirtualMachineOld()\n\n");
	return virtualMachine;

} // end of method   IsSebRunningOnVirtualMachineOld()





// *******************************************************
// Since the VM detection using the hypervisor present bit
// does not work on every virtual machine,
// here is better working version 
// It does not only detect SEB running on a virtual machine,
// but also SEB running on a remote desktop.
// Suggested by Jan Derriks, Hogeschool van Amsterdam.
// *******************************************************

// "Red Pill" code from
// http://invisiblethings.org/papers/redpill.html
// but this code seems to be not working!
/*
int insideMatrixB()
{
	unsigned char m[2+4];
	unsigned char rpill[] = "\x0f\x01\x0d\x00\x00\x00\x00\xc3";
	*((unsigned*) &rpill[3]) = (unsigned)m;
	((void(*)())  &rpill)();
	return (m[5]>0xd0) ? 1 : 0;
}
*/



// This code, however, does work!
// (At least it detects VM Player virtual machines).
bool insideMatrix()
{
	unsigned char mem[4] = {0,0,0,0};
	__asm str mem;
	if ((mem[0] == 0x00) && (mem[1] == 0x40))
		return true ; //printf("INSIDE  MATRIX!!\n");
	else
		return false; //printf("OUTSIDE MATRIX!!\n");
	return false;
}



bool IsSebRunningOnVirtualMachineNew()
{
	bool virtualMachine = false;

	logg(fp, "Enter IsSebRunningOnVirtualMachineNew()\n\n");

	// STR or SIDT code?
	virtualMachine = insideMatrix();

	if (virtualMachine == true)
	{
		logg(fp, "   Red Pill: Virtual Machine detected\n");
		logg(fp, "   SEB seems to run on a VIRTUAL machine!\n\n");
	}
	else
	{
		logg(fp, "   Red Pill: No Virtual Machine detected\n");
		logg(fp, "   SEB seems to run on a PHYSICAL machine!\n\n");
	}

	// JAND: add RDP detection
	// from Answers/186016/How-to-detect-if-a-remote-desktop-session-is-present
	if (GetSystemMetrics(SM_REMOTESESSION))
	{
		logg(fp, "   Red Pill: Remote Desktop detected\n");
		logg(fp, "   SEB seems to run on a REMOTE machine!\n\n");
		virtualMachine = true;
	}
	else
	{
		logg(fp, "   Red Pill: No Remote Desktop detected\n");
		logg(fp, "   SEB seems to run on a LOCAL machine!\n\n");
	}

	logg(fp, "Leave IsSebRunningOnVirtualMachineNew\n\n");
	return virtualMachine;

} // end of method   IsSebRunningOnVirtualMachineNew()





// Get the hardcoded values for SebStarter.ini,
// in case the Exam URL etc.
// shall be hidden from the students.

void GetHardcodedSebStarterIni()
{
	mpParam["WriteSebStarterLogFile"] = "1";

	mpParam["InsideSebEnableSwitchUser"       ] = "0";
	mpParam["InsideSebEnableLockThisComputer" ] = "0";
	mpParam["InsideSebEnableChangeAPassword"  ] = "0";
	mpParam["InsideSebEnableStartTaskManager" ] = "0";
	mpParam["InsideSebEnableLogOff"           ] = "0";
	mpParam["InsideSebEnableShutDown"         ] = "0";
	mpParam["InsideSebEnableEaseOfAccess"     ] = "0";
	mpParam["InsideSebEnableVmWareClientShade"] = "0";

	mpParam["OutsideSebEnableSwitchUser"       ] = "1";
	mpParam["OutsideSebEnableLockThisComputer" ] = "1";
	mpParam["OutsideSebEnableChangeAPassword"  ] = "1";
	mpParam["OutsideSebEnableStartTaskManager" ] = "1";
	mpParam["OutsideSebEnableLogOff"           ] = "1";
	mpParam["OutsideSebEnableShutDown"         ] = "1";
	mpParam["OutsideSebEnableEaseOfAccess"     ] = "1";
	mpParam["OutsideSebEnableVmWareClientShade"] = "1";

	mpParam["AllowVirtualMachine"      ] = "0";
	mpParam["ForceWindowsService"      ] = "1";
	mpParam["CreateNewDesktop"         ] = "1";
	mpParam["ShowSebApplicationChooser"] = "1";
	mpParam["HookMessages"             ] = "1";
	mpParam["EditRegistry"             ] = "1";
	mpParam["MonitorProcesses"         ] = "0";
	mpParam["ShutdownAfterAutostartProcessTerminates"] = "0";

	string s1 = "Seb,../xulrunner/xulrunner.exe ../xul_seb/application.ini -profile ";
	string s2 = "\"";
	string s3 = "%LOCALAPPDATA%\\ETH_Zuerich\\xul_seb\\Profiles";
	string s4 = "\"";
    string SebBrowserString = s1 + s2 + s3 + s4;

  //mpParam["SebBrowser"           ] = "Seb,../xulrunner/xulrunner.exe ../xul_seb/application.ini -profile "%LOCALAPPDATA%\ETH_Zuerich\xul_seb\Profiles"";
	mpParam["SebBrowser"           ] =  SebBrowserString;
	mpParam["AutostartProcess"     ] = "Seb";
	mpParam["ExamUrl"              ] = "http://www.safeexambrowser.org";
	mpParam["PermittedApplications"] = "Calculator,calc.exe;Notepad,notepad.exe;";

	mpParam["Win9xKillExplorer"        ] = "1";
	mpParam["Win9xScreenSaverRunning"  ] = "0";
	mpParam["StrongKillProcessesBefore"] = "";
	mpParam["StrongKillProcessesAfter" ] = "";

	mpParam["HookDll"] = MSG_HOOK_DLL;

	return;
}





BOOL ReadSebStarterIni()
{
	string strLine  = "";
	string strKey   = "";
	string strValue = "";
	size_t strFound = -1;
	string sExamUrl           = "";
	string sLocalAppDataConst = "";
	string sLocalAppDataDir   = "";
	string sApplicationName = "";
	string sCommandLine     = "";
  //string sCommandProcess  = "";
	string sProcess   = "";
	string sProcesses = "";
	vector<string> vProcesses;
	vector<string>::iterator itProcesses;
	vector<string>::iterator itProcess;
	ifstream inputStream;

	logg(fp, "Enter ReadSebStarterIni()\n\n");

	try
	{
		// The SebStarter.ini and MsgHook.ini configuration files have moved:
		// Previously:
		// SebStarter.ini was lying in the /SebStarter subdirectory,
		//    MsgHook.ini was lying in the /MsgHook    subdirectory.
		// Both had to be copied to the /Debug and /Release directories.
		// before starting SebStarter.exe .
		// Now:
		// SebStarter.ini and MsgHook.ini are both in the SebClient main project directory,
		// together with the SebClient.sln project file,
		// the /Debug subdirectory and the /Release subdirectory.
		// Advantage: the .ini files are lying together, being accessible
		// for both the /Debug and the /Release version without copying
		// being necessary anymore.


		// In case the Exam URL etc. shall be hidden from the students,
		// use the hardcoded values rather than loading them from SebStarter.ini file

		bool useHardCodedSebStarterIni =  false;

		if  (useHardCodedSebStarterIni == true)
		{
			GetHardcodedSebStarterIni();
		}
		else
		{
			logg(fp, "Try to open ini file %s\n", iniFileSebStarter);
			inputStream.open(iniFileSebStarter);

			// If the SebStarter.ini file could not be opened, give up
			if (!inputStream.is_open()) 
			{
				OutputErrorMessage(languageIndex, IND_SebStarterIniError, IND_MessageKindError);
				//MessageBox(NULL       , messageText[languageIndex][IND_SebStarterIniError], "Error", 16);
				//logg(fp, "Error: %s\n", messageText[languageIndex][IND_SebStarterIniError]);
				logg(fp, "Leave ReadSebStarterIni() and return FALSE\n\n");
				return FALSE;
			}

			logg(fp, "\n");
			logg(fp, "key = value\n");
			logg(fp, "-----------\n");

			while(!getline(inputStream, strLine).eof())
			{
				strFound = strLine.find  ("=", 0);
				strKey   = strLine.substr(0, strFound);
				strValue = strLine.substr(   strFound + 1, strLine.length());

				// Skip lines without a "=" character
				if (strFound == string::npos)
				{
					logg(fp, "%s\n", strLine.c_str());
				}
				else
				{
					mpParam[strKey] = strValue;
					//captionString = strKey  .c_str();
					//messageString = strValue.c_str();
					//MessageBox(NULL, messageString, captionString, 16);
					logg(fp, "%s = %s\n", strKey.c_str(), strValue.c_str());
				}
			}

			inputStream.close();
			logg(fp, "-----------\n\n");

		} // end if  (useHardCodedSebStarterIni == true)


		// In any case, set the correct MsgHook.dll library
		mpParam["HookDll"] = MSG_HOOK_DLL;


		// Decide whether to write data into the logfile
		if (getBool("WriteSebStarterLogFile"))
		{
			logFileDesiredSebStarter = true;
			logg(fp, "Logfile desired, therefore keeping logfile\n\n");
		}
		else
		{
			logFileDesiredSebStarter = false;
			logg(fp, "No logfile desired, therefore closing and removing logfile\n\n");
			if (fp != NULL)
			{
				fclose(fp);
				remove(logFileSebStarter);
			}
		}


		// Decide whether to enforce socket communication with SEB Windows Service
		if (getBool("ForceWindowsService"))
		{
			forceWindowsService = true;
			logg(fp, "Windows Service demanded    , setting forceWindowsService = true\n");
		}
		else
		{
			forceWindowsService = false;
			logg(fp, "Windows Service not demanded, setting forceWindowsService = false\n");
		}


		// Decide whether to allow SEB running on a virtual machine
		if (getBool("AllowVirtualMachine"))
		{
			allowVirtualMachine = true;
			logg(fp, "Virtual machine allowed     , setting allowVirtualMachine = true\n\n\n\n");
		}
		else
		{
			allowVirtualMachine = false;
			logg(fp, "Virtual machine not allowed , setting allowVirtualMachine = false\n\n\n\n");
		}


		// Detect whether SEB runs on a virtual machine
		// (or on a remote desktop)
		runningOnVirtualMachine = false;
		runningOnVirtualMachine |= IsSebRunningOnVirtualMachineOld();
		runningOnVirtualMachine |= IsSebRunningOnVirtualMachineNew();

		if (runningOnVirtualMachine == true)
		{
			 logg(fp, "Caution  , SEB seems to run on a VIRTUAL  machine!\n\n\n\n");
		}
		else
		{
			logg(fp, "All right, SEB seems to run on a PHYSICAL machine!\n\n\n\n");
		}


		// If SEB is running on a virtual machine and this is not allowed, take action
		if ((runningOnVirtualMachine == true) && (allowVirtualMachine == false))
		{
			OutputErrorMessage(languageIndex, IND_VirtualMachineForbidden, IND_MessageKindError);
			logg(fp, "Forbidden to run SEB on a VIRTUAL machine!\n\n\n\n");
			logg(fp, "Leave ReadSebStarterIni() and return FALSE\n\n");
			return FALSE;
		}


		// Get the start URL for SEB
		sExamUrl = mpParam["ExamUrl"];

		// Store the start URL for SEB
		strcpy(examUrl, sExamUrl.c_str());

		//MessageBox(NULL, sExamUrl.c_str(), "ExamUrl", MB_ICONERROR);
		logg(fp, "sExamUrl = %s\n", sExamUrl.c_str());
		logg(fp, "\n");


		// Get the processes (SEB and third-party applications)
		sProcesses = mpParam["Processes"];

		// Get the SEB Process, which is defined separately
		// from the other processes in SebStarter.ini
		string sebProcess = mpParam["SebBrowser"];

		vector<string> sebProcessVector;
		Tokenize(sebProcess, sebProcessVector, ",");

		// Append the start URL of the exam to the browser command line,
		// so the Safe Exam Browser starts directly on the exam homepage
		sApplicationName = sebProcessVector[0];
		sCommandLine     = sebProcessVector[1];


		// If the xulrunner.exe command line contains the environment variable
		// %LOCALAPPDATA%, then replace it by its current value, e.g.
		//
		// xulrunner.exe -profile "%LOCALAPPDATA%\ETH_Zuerich\xul_seb\Profiles"
		// xulrunner.exe -profile "C:\Users\<Username>\AppData\Local\ETH_Zuerich\xul_seb\Profiles"
		//
		// CAUTION: The quotation marks are mandatory because "%LOCALAPPDATA%"
		// might contain spaces and this would corrupt the xulrunner.exe call,
		// making it believe a new command line parameter is there!

		string sLocalAppDataConst = "%LOCALAPPDATA%";
		string sLocalAppDataDir   = localAppDataDirectory;
		//e.g. sLocalAppDataDir   = "C:/Users/<Username>/AppData/Local";

		int iLocalAppDataConstPos = sCommandLine.find(sLocalAppDataConst);
		int iLocalAppDataConstLen = sLocalAppDataConst.length();

		logg(fp, "sCommandLine          = %s\n", sCommandLine      .c_str());
		logg(fp, "sLocalAppDataConst    = %s\n", sLocalAppDataConst.c_str());
		logg(fp, "sLocalAppDataDir      = %s\n", sLocalAppDataDir  .c_str());
		logg(fp, "iLocalAppDataConstPos = %d\n", iLocalAppDataConstPos);
		logg(fp, "iLocalAppDataConstLen = %d\n", iLocalAppDataConstLen);

		// If the %LOCALAPPDATA% constant is contained,
		// replace it by its current value
		if (iLocalAppDataConstPos >= 0)
		{
			sCommandLine.replace(iLocalAppDataConstPos, iLocalAppDataConstLen, sLocalAppDataDir);
			logg(fp, "sCommandLine     = %s\n", sCommandLine .c_str());
		}

		logg(fp, "\n");


		// If the xulrunner.exe command line contains a special profile,
		// then create an empty directory at the desired location, e.g.
		//
		// xulrunner.exe -profile "%LOCALAPPDATA%\ETH_Zuerich\xul_seb\Profiles"
		// xulrunner.exe -profile "C:\Users\<Username>\AppData\Local\ETH_Zuerich\xul_seb\Profiles"

		string sProfileConst = "-profile ";
		string sProfileDir   = "";
		//e.g. sProfileDir = "C:\Users\<Username>\AppData\Local\ETH_Zuerich\xul_seb\Profiles";

		int iProfileConstPos = sCommandLine.find(sProfileConst);
		int iProfileConstLen = sProfileConst.length();
		int iProfileDirPos   = iProfileConstPos + iProfileConstLen;

		if (iProfileConstPos >= 0)
		{
			sProfileDir = sCommandLine.substr(iProfileDirPos);

			logg(fp, "sProfileConst    = %s\n", sProfileConst.c_str());
			logg(fp, "sProfileDir      = %s\n", sProfileDir  .c_str());
			logg(fp, "iProfileConstPos = %d\n", iProfileConstPos);
			logg(fp, "iProfileConstLen = %d\n", iProfileConstLen);
			logg(fp, "iProfileDirPos   = %d\n", iProfileDirPos);
			logg(fp, "\n");

			// If the "-profile" constant is contained,
			// create the desired profile directory

			string sManufacturer = "ETH_Zuerich";
			string sProduct      = "xul_seb";
			string sProfiles     = "Profiles";

			string sProfileDir0 = sLocalAppDataDir;
			string sProfileDir1 = sLocalAppDataDir + "\\" + sManufacturer;
			string sProfileDir2 = sLocalAppDataDir + "\\" + sManufacturer + "\\" + sProduct;
			string sProfileDir3 = sLocalAppDataDir + "\\" + sManufacturer + "\\" + sProduct + "\\" + sProfiles;

			logg(fp, "sProfileDir0 = %s\n", sProfileDir0);
			logg(fp, "sProfileDir1 = %s\n", sProfileDir1);
			logg(fp, "sProfileDir2 = %s\n", sProfileDir2);
			logg(fp, "sProfileDir3 = %s\n", sProfileDir3);
			logg(fp, "\n");

		  //BOOL bDirectoryCreated0 = CreateDirectory(sProfileDir0.c_str(), NULL);
			BOOL bDirectoryCreated1 = CreateDirectory(sProfileDir1.c_str(), NULL);
			BOOL bDirectoryCreated2 = CreateDirectory(sProfileDir2.c_str(), NULL);
			BOOL bDirectoryCreated3 = CreateDirectory(sProfileDir3.c_str(), NULL);

		  //logg(fp, "bDirectoryCreated0 = %d\n", bDirectoryCreated0);
			logg(fp, "bDirectoryCreated1 = %d\n", bDirectoryCreated1);
			logg(fp, "bDirectoryCreated2 = %d\n", bDirectoryCreated2);
			logg(fp, "bDirectoryCreated3 = %d\n", bDirectoryCreated3);
			logg(fp, "\n\n\n");
		}


		// Append the exam URL to the xulrunner command line
		sCommandLine.append(" -url ");
		sCommandLine.append(sExamUrl);

		sebProcessVector[1] = sCommandLine;

		//MessageBox(NULL,       sCommandLine.c_str(),   sApplicationName.c_str(),MB_ICONERROR);
		//MessageBox(NULL,sebProcessVector[1].c_str(),sebProcessVector[0].c_str(),MB_ICONERROR);

		logg(fp, "sApplicationName = %s\n", sApplicationName.c_str());
		logg(fp, "sCommandLine     = %s\n",     sCommandLine.c_str());
		logg(fp, "\n");
		logg(fp, "sebProcessVector[0] = %s\n", sebProcessVector[0].c_str());
		logg(fp, "sebProcessVector[1] = %s\n", sebProcessVector[1].c_str());
		logg(fp, "\n");

		// Add the SEB process to the process list
		mpProcesses.insert(make_pair(sebProcessVector[0], sebProcessVector[1]));

		// handle processes from Registry
		ReadProcessesInRegistry();

		if (mpProcesses.size() == 1)
		{
			// if nothing is found in registry  -> read SebStarter.ini
			// handle processes from configuration file SebStarter.ini
			sProcesses = mpParam["PermittedApplications"];
			if (sProcesses != "")
			{
				Tokenize(sProcesses, vProcesses, ";");

				int cntCommand = IDM_OFFSET;
				logg(fp, "List of third party applications to insert:\n");
				logg(fp, "-------------------------------------------\n");

				for (itProcesses  = vProcesses.begin();
					 itProcesses != vProcesses.end();
					 itProcesses++)
				{
					vector<string> vProcess;
					sProcess = *itProcesses;
					Tokenize(sProcess, vProcess, ",");
					mpProcesses.insert(make_pair(vProcess[0], vProcess[1]));
					//MessageBox(NULL, vProcess[1].c_str(), vProcess[0].c_str(), MB_ICONERROR);
					logg(fp, "vProcess[0] = %s\n", vProcess[0].c_str());
					logg(fp, "vProcess[1] = %s\n", vProcess[1].c_str());
					cntCommand++;
				}

				logg(fp, "-------------------------------------------\n");
				logg(fp, "\n");

			} // end if (sProcesses != "")
		} // end if (mpProcesses.size() == 1)
	} // end try

	catch (char* str)
	{
		MessageBox(NULL, str, "Error", MB_ICONERROR);
		logg(fp, "Error: %s\n", str);
		logg(fp, "Leave ReadSebStarterIni() and return FALSE\n\n");
		return FALSE;
	}

	logg(fp, "Leave ReadSebStarterIni()\n\n");
	return TRUE;

} // end ReadSebStarterIni()





BOOL ReadProcessesInRegistry()
{
	logg(fp, "Enter ReadProcessesInRegistry()\n");

	try
	{
		vector<string> vProcesses;
		string sProcesses = "";
		string sProcess   = "";
		vector<string>::iterator itProcesses;
		HKEY  hKeySeb;
		char  lszValue[255];
		LONG  returnStatus;
		DWORD dwType = REG_SZ;
		DWORD dwSize = 255;
		if (!HandleOpenRegistryKey(HKLM, KEY_PoliciesSeb, &hKeySeb, FALSE)) return FALSE;
		returnStatus = RegQueryValueEx(hKeySeb, VAL_PermittedApplications, NULL, &dwType,(LPBYTE)&lszValue, &dwSize);

		if (returnStatus == ERROR_SUCCESS)
		{
			Tokenize(lszValue, vProcesses, ";");

			for (itProcesses  = vProcesses.begin();
				 itProcesses != vProcesses.end();
				 itProcesses++)
			{
				vector<string> vProcess;
				sProcess = *itProcesses;				
				Tokenize(sProcess, vProcess, ",");
				mpProcesses.insert(make_pair(vProcess[0], vProcess[1]));
				//MessageBox(NULL, vProcess[1].c_str(), vProcess[0].c_str(), MB_ICONERROR);
				logg(fp, "vProcess[0] = %s\n", vProcess[0].c_str());
				logg(fp, "vProcess[1] = %s\n", vProcess[1].c_str());
			}
		}
		RegCloseKey(hKeySeb);
	}
	catch (char* str)
	{		
		MessageBox(NULL, str, "Error", MB_ICONERROR);
		logg(fp, "Error: %s\n", str);
		logg(fp, "Leave ReadProcessesInRegistry()\n\n");
		return FALSE;
	}

	logg(fp, "Leave ReadProcessesInRegistry()\n\n");
	return TRUE;

} // end ReadProcessesInRegistry()





BOOL GetClientInfo()
{
	//WORD     wVersionRequested;
	//WSADATA  wsaData;		
	//PHOSTENT hostInfo;
	//int      socketResult;

	wVersionRequested = MAKEWORD(2, 0);

	logg(fp, "Enter GetClientInfo()\n\n");

	// Set the default values for the
	// socket communication parameters.
	// Currently, we use sockets on the same machine,
	// so we could in the future
	// define in "SebStarter.ini" the default values:
	//
	// defaultUserName     = "";
	// defaultHostName     = "localhost";
	// defaultPortNumber   = 57016;
	// defaultSendInterval = 100;
	// defaultRecvTimeout  = 100;
	// defaultNumMessages  = 3;

	try
	{
		if (WSAStartup(wVersionRequested, &wsaData) == 0)
		{

			HANDLE  ProcessHandle = NULL;
			DWORD   DesiredAccess = 0;
			HANDLE  TokenHandle   = NULL;

			TOKEN_INFORMATION_CLASS TokenInformationClass;
			LPVOID TokenInformation       = NULL;
			DWORD  TokenInformationLength = 0;
			DWORD  ReturnLength           = 0;

			BOOL b1 = OpenProcessToken(ProcessHandle, DesiredAccess, &TokenHandle);
			BOOL b2 = GetTokenInformation(TokenHandle     , TokenInformationClass,
										  TokenInformation, TokenInformationLength,
										  &ReturnLength);

/*
			BOOL OpenProcessToken(HANDLE ProcessHandle, DWORD DesiredAccess, PHANDLE TokenHandle);
			BOOL GetTokenInformation(HANDLE TokenHandle     , TOKEN_INFORMATION_CLASS TokenInformationClass,
									 LPVOID TokenInformation, DWORD                   TokenInformationLength,
									 PDWORD ReturnLength);
*/

			//if ((b1 == TRUE) && (b2 == TRUE))
			{
				//MessageBox(NULL, cHostName, "cHostName", 16);
			  //logg(fp, "TokenInformationLength = %s\n", TokenHandle->
			  //logg(fp, "TokenInformationClass  = %s\n", TokenInformationClass->
				logg(fp, "TokenInformationLength = %d\n", TokenInformationLength);
				logg(fp, "          ReturnLength = %d\n",           ReturnLength);
				logg(fp, "\n");
			}


			// Debug output of socket communication parameters
			logg(fp, " userName     = %s\n", userName);
			logg(fp, " hostName     = %s\n", hostName);
			logg(fp, " portNumber   = %d\n", portNumber);
			logg(fp, " sendInterval = %d\n", sendInterval);
			logg(fp, " recvTimeout  = %d\n", recvTimeout);
			logg(fp, " numMessages  = %d\n", numMessages);
			logg(fp, "\n");


			// If the user input is an alpha name for the host, use gethostbyname()
			// If not, get host by addr (assume IPv4)

			if (isalpha(hostName[0]))
			{
				/* host address is a name, e.g. "ilgpcs" or "ilgpcs.d.ethz.ch" */
				logg(fp, "Calling gethostbyname() with symbolic address: %s\n\n", hostName);
				remoteHost = gethostbyname(hostName);
			}
			else
			{
				/* host address is a number (IP address, e.g. "129.132.26.158") */    
				logg(fp, "Calling gethostbyaddr() with numeric address: %s\n\n", hostName);
				addr.s_addr = inet_addr(hostName);
				if (addr.s_addr == INADDR_NONE)
				{
					logg(fp, "The IPv4 address entered must be a legal address\n");
					//WSACleanup();
					//return 1;
				}
				remoteHost = gethostbyaddr((char *) &addr, 4, AF_INET);
			}

			if (remoteHost == NULL)
			{
				logg(fp, "gethostbyname / gethostbyaddr failed: %ld\n", WSAGetLastError());
				//WSACleanup();
				//return 1;
			}


			strcpy(cHostName, hostName);

			//hostRes  = gethostname(cHostName, sizeof(cHostName));
			//hostName = cHostName;

			if (remoteHost != NULL)
			//if (hostRes == 0)
			{
				//MessageBox(NULL, cHostName, "cHostName", 16);
				logg(fp, "cHostName   = %s\n", cHostName);
				logg(fp, " hostName   = %s\n",  hostName);
				logg(fp, " portNumber = %d\n",  portNumber);
				logg(fp, "\n");
			  //hostInfo   = gethostbyname(cHostName);
				hostInfo   = remoteHost;
				remoteHost = gethostbyname(hostName);
				if (hostInfo != NULL)
				{
					logg(fp, "hostInfo->h_name = %s\n", hostInfo->h_name);
					cIp = inet_ntoa(*(struct in_addr *)*hostInfo->h_addr_list);
				}
			}
			//Do NOT call WSACleanup() here, since from now on we use sockets!
			//WSACleanup();
		}
	}
	catch (char* str)
	{		
		MessageBox(NULL, str, "Error", MB_ICONERROR);
		logg(fp, "Error: %s\n\n", str);
		logg(fp, "Leave GetClientInfo()\n\n");
		WSACleanup();
		return FALSE;
	}


	// Get the symbolic and numeric IP address

	addr.s_addr = *(u_long *) remoteHost->h_addr_list[0];

	logg(fp, "h_name      : %s\n", remoteHost->h_name);
	logg(fp, "IP Address  : %s\n", inet_ntoa(addr));
	logg(fp, "address type: ");
	switch (remoteHost->h_addrtype)
	{
		case AF_INET:    logg(fp, "AF_INET\n");    break;
		case AF_INET6:   logg(fp, "AF_INET6\n");   break;
		case AF_NETBIOS: logg(fp, "AF_NETBIOS\n"); break;
		default:                                   break;
	}
	logg(fp, "h_addrtype  : %d\n", remoteHost->h_addrtype);
	logg(fp, "h_length    : %d\n", remoteHost->h_length);    
	logg(fp, "\n");


	// The sockaddr_in structure specifies the address family,
	// IP address, and port of the server to be connected to.

	clientService.sin_family      = AF_INET;
  //clientService.sin_addr.s_addr = inet_addr("129.132.26.158");
	clientService.sin_addr.s_addr = addr.s_addr;
	clientService.sin_port        = htons(portNumber);

	logg(fp, "clientService.sin_family      = %d\n", clientService.sin_family);
	logg(fp, "clientService.sin_addr.s_addr = %d\n", clientService.sin_addr.s_addr);
	logg(fp, "clientService.sin_port        = %d\n", clientService.sin_port);
	logg(fp, "\n");


	// Create a SOCKET for connecting to the server
	// Connect to the server

	socketResult = OpenSocket();
	if (socketResult == FALSE)
	{
		logg(fp, "socketResult = OpenSocket() failed!\n");
		logg(fp, "What shall I do now?\n");
		logg(fp, "forceWindowsService = %d\n", forceWindowsService);
		logg(fp, "\n");
		return TRUE;
	}


	// Set the timeout for the send() and recv() socket commands. 
	// The timeouts are expected in milliseconds by the subsequent
	// setsockopt() and getsockopt() socket commands (see below).
	//
	// If the flag ForceWindowsService is set to 1 (true) in the SebStarter.ini,
	// the timeout for the recv() socket command is set to "0" = "infinite".
	// The SEB client then waits in ANY case for an acknowledgement
	// from the SEB Windows Service that the registry values have been set.
	// So the SEB client is BLOCKED until the server acknowledgement comes in.
	// Problem: should the server be down or blocked,
	// the SEB client remains blocked, too, and thus cannot start up.
	// So the exam cannot take place then, not even in "unsure" mode!
	//
	// If the flag ForceWindowsService is set to 0 (false) in the SebStarter.ini,
	// the timeout for the recv() socket command is set to the default timeout, e.g. 100 msecs.
	// The SEB client then only waits for a normal (short) timespan,
	// and then continues. The SEB Windows Service might have problems,
	// or succeeded in setting the registry values and just did not answer quickly.
	// But in any case the SEB client is NOT blocked by a missing server acknowledgement,
	// so the exam can take place with or without registry changes ("safe" or "unsafe" mode).

	if (forceWindowsService)
	{
		recvTimeout = 0;   // timeout "0" means "infinite" in this case !!!
		logg(fp, "Force Windows Service demanded, therefore socket recvTimeout = infinite\n");
	}
	else
	{
		recvTimeout = defaultRecvTimeout;   // e.g. 100 milliseconds
		logg(fp, "Force Windows Service not demanded, therefore socket recvTimeout = %d\n", recvTimeout);
	}

	// Call getsockopt. 
	// The SO_RCVTIMEO parameter is a socket option 
	// that tells the function to check the timeout
	// for send() and recv() commands. 

	int timeoutVal = recvTimeout;   // e.g. 100 msec
	int timeoutLen = sizeof(int);

	logg(fp, "\n");

	if (setsockopt(ConnectSocket, SOL_SOCKET, SO_RCVTIMEO, (char*)&timeoutVal,  timeoutLen) != SOCKET_ERROR)
		logg(fp, "Set SO_RCVTIMEO timeout value: %d milliseconds\n", timeoutVal);

	if (getsockopt(ConnectSocket, SOL_SOCKET, SO_RCVTIMEO, (char*)&timeoutVal, &timeoutLen) != SOCKET_ERROR)
		logg(fp, "Get SO_RCVTIMEO timeout value: %d milliseconds\n", timeoutVal);

	logg(fp, "\n");


	// Get the SID (security identifier) of the current user.
	// This works only in .NET projects!
	//WindowsIdentity    currentUser     = WindowsIdentity.GetCurrent();
	//SecurityIdentifier currentUserSid  = currentUser.User;
	//string             currentUserName = currentUser.Name;

	strcpy(userSid, "");
	DetermineUserSid(userSid);
	logg(fp, "userSid       = %s\n", userSid);
	logg(fp, "\n");


	// Send username, hostname etc. to server.
	// Format of the sent strings is "leftSide=rightSide",
	// exactly as in the SebStarter.ini configuration file.

	intInsideSebEnableSwitchUser        = (int) getBool("InsideSebEnableSwitchUser");
	intInsideSebEnableLockThisComputer  = (int) getBool("InsideSebEnableLockThisComputer");
	intInsideSebEnableChangeAPassword   = (int) getBool("InsideSebEnableChangeAPassword");
	intInsideSebEnableStartTaskManager  = (int) getBool("InsideSebEnableStartTaskManager");
	intInsideSebEnableLogOff            = (int) getBool("InsideSebEnableLogOff");
	intInsideSebEnableShutDown          = (int) getBool("InsideSebEnableShutDown");
	intInsideSebEnableEaseOfAccess      = (int) getBool("InsideSebEnableEaseOfAccess");
	intInsideSebEnableVmWareClientShade = (int) getBool("InsideSebEnableVmWareClientShade");

	intOutsideSebEnableSwitchUser        = (int) getBool("OutsideSebEnableSwitchUser");
	intOutsideSebEnableLockThisComputer  = (int) getBool("OutsideSebEnableLockThisComputer");
	intOutsideSebEnableChangeAPassword   = (int) getBool("OutsideSebEnableChangeAPassword");
	intOutsideSebEnableStartTaskManager  = (int) getBool("OutsideSebEnableStartTaskManager");
	intOutsideSebEnableLogOff            = (int) getBool("OutsideSebEnableLogOff");
	intOutsideSebEnableShutDown          = (int) getBool("OutsideSebEnableShutDown");
	intOutsideSebEnableEaseOfAccess      = (int) getBool("OutsideSebEnableEaseOfAccess");
	intOutsideSebEnableVmWareClientShade = (int) getBool("OutsideSebEnableVmWareClientShade");

	logg(fp, "intInsideSebEnableSwitchUser        = %d\n", intInsideSebEnableSwitchUser);
	logg(fp, "intInsideSebEnableLockThisComputer  = %d\n", intInsideSebEnableLockThisComputer);
	logg(fp, "intInsideSebEnableChangeAPassword   = %d\n", intInsideSebEnableChangeAPassword);
	logg(fp, "intInsideSebEnableStartTaskManager  = %d\n", intInsideSebEnableStartTaskManager);
	logg(fp, "intInsideSebEnableLogOff            = %d\n", intInsideSebEnableLogOff);
	logg(fp, "intInsideSebEnableShutDown          = %d\n", intInsideSebEnableShutDown);
	logg(fp, "intInsideSebEnableEaseOfAccess      = %d\n", intInsideSebEnableEaseOfAccess);
	logg(fp, "intInsideSebEnableVmWareClientShade = %d\n", intInsideSebEnableVmWareClientShade);
	logg(fp, "\n");

	logg(fp, "intOutsideSebEnableSwitchUser        = %d\n", intOutsideSebEnableSwitchUser);
	logg(fp, "intOutsideSebEnableLockThisComputer  = %d\n", intOutsideSebEnableLockThisComputer);
	logg(fp, "intOutsideSebEnableChangeAPassword   = %d\n", intOutsideSebEnableChangeAPassword);
	logg(fp, "intOutsideSebEnableStartTaskManager  = %d\n", intOutsideSebEnableStartTaskManager);
	logg(fp, "intOutsideSebEnableLogOff            = %d\n", intOutsideSebEnableLogOff);
	logg(fp, "intOutsideSebEnableShutDown          = %d\n", intOutsideSebEnableShutDown);
	logg(fp, "intOutsideSebEnableEaseOfAccess      = %d\n", intOutsideSebEnableEaseOfAccess);
	logg(fp, "intOutsideSebEnableVmWareClientShade = %d\n", intOutsideSebEnableVmWareClientShade);
	logg(fp, "\n");

	sprintf(stringInsideSebEnableSwitchUser       , "%d", intInsideSebEnableSwitchUser);
	sprintf(stringInsideSebEnableLockThisComputer , "%d", intInsideSebEnableLockThisComputer);
	sprintf(stringInsideSebEnableChangeAPassword  , "%d", intInsideSebEnableChangeAPassword);
	sprintf(stringInsideSebEnableStartTaskManager , "%d", intInsideSebEnableStartTaskManager);
	sprintf(stringInsideSebEnableLogOff           , "%d", intInsideSebEnableLogOff);
	sprintf(stringInsideSebEnableShutDown         , "%d", intInsideSebEnableShutDown);
	sprintf(stringInsideSebEnableEaseOfAccess     , "%d", intInsideSebEnableEaseOfAccess);
	sprintf(stringInsideSebEnableVmWareClientShade, "%d", intInsideSebEnableVmWareClientShade);

	sprintf(stringOutsideSebEnableSwitchUser       , "%d", intOutsideSebEnableSwitchUser);
	sprintf(stringOutsideSebEnableLockThisComputer , "%d", intOutsideSebEnableLockThisComputer);
	sprintf(stringOutsideSebEnableChangeAPassword  , "%d", intOutsideSebEnableChangeAPassword);
	sprintf(stringOutsideSebEnableStartTaskManager , "%d", intOutsideSebEnableStartTaskManager);
	sprintf(stringOutsideSebEnableLogOff           , "%d", intOutsideSebEnableLogOff);
	sprintf(stringOutsideSebEnableShutDown         , "%d", intOutsideSebEnableShutDown);
	sprintf(stringOutsideSebEnableEaseOfAccess     , "%d", intOutsideSebEnableEaseOfAccess);
	sprintf(stringOutsideSebEnableVmWareClientShade, "%d", intOutsideSebEnableVmWareClientShade);

	// Build a binary string containing the "0"/"1" registry settings

	strcpy(registryFlags, "");

	strcat(registryFlags, stringInsideSebEnableSwitchUser);
	strcat(registryFlags, stringInsideSebEnableLockThisComputer);
	strcat(registryFlags, stringInsideSebEnableChangeAPassword);
	strcat(registryFlags, stringInsideSebEnableStartTaskManager);
	strcat(registryFlags, stringInsideSebEnableLogOff);
	strcat(registryFlags, stringInsideSebEnableShutDown);
	strcat(registryFlags, stringInsideSebEnableEaseOfAccess);
	strcat(registryFlags, stringInsideSebEnableVmWareClientShade);

	strcat(registryFlags, stringOutsideSebEnableSwitchUser);
	strcat(registryFlags, stringOutsideSebEnableLockThisComputer);
	strcat(registryFlags, stringOutsideSebEnableChangeAPassword);
	strcat(registryFlags, stringOutsideSebEnableStartTaskManager);
	strcat(registryFlags, stringOutsideSebEnableLogOff);
	strcat(registryFlags, stringOutsideSebEnableShutDown);
	strcat(registryFlags, stringOutsideSebEnableEaseOfAccess);
	strcat(registryFlags, stringOutsideSebEnableVmWareClientShade);

	//strcpy(userNameRegistryFlags, userName);
	//strcpy(userNameRegistryFlags, endOfStringKeyWord);
	//strcat(userNameRegistryFlags, registryFlags);


	// Transmission of equation strings to server.
	// The most important data are:
	//
	// 1.) userName:
	// The user name must be sent so the server knows
	// for whom it shall set/change the registry values.
	//
	// 2.) userSid:
	// For being able to set/change the registry values,
	// the server must know the Security Identifier (SID)
	// of the user currently logged in.
	// Either the client or the server must therefore
	// convert the userName to the userSid.
	//
	// 3.) registryFlags:
	// To reduce network traffic, the registry flags are sent
	// as a "0000000011111111" pattern in the registryFlags string.
	//
	// The host name can be omitted,
	// since it is "localhost" by default anyway
	// (SEB Windows service runs on same machine as SEB client)

	char  leftSide[BUFLEN];
	char rightSide[BUFLEN];

	strcpy( leftSide, "");
	strcpy(rightSide, "");

  //socketResult = SendEquationToSocketServer("HostName"     ,  hostName     , sendInterval);
  //socketResult = RecvEquationOfSocketServer( leftSide      ,  rightSide    , recvTimeout);

	socketResult = SendEquationToSocketServer("UserName"     ,   userName    , sendInterval);
	socketResult = RecvEquationOfSocketServer( leftSide      ,  rightSide    , recvTimeout);

	socketResult = SendEquationToSocketServer("UserSid"      ,  userSid , sendInterval);
	socketResult = RecvEquationOfSocketServer( leftSide      , rightSide, recvTimeout);

	socketResult = SendEquationToSocketServer("RegistryFlags", registryFlags, sendInterval);
	socketResult = RecvEquationOfSocketServer( leftSide      ,    rightSide , recvTimeout);


	// Alternatively, the registry flags could also be sent
	// one by one, namely in the "InsideSebEnableStartTaskManager=1" format:
/*
	socketResult = SendEquationToSocketServer((char*)MSG_InsideSebEnableSwitchUser       , stringInsideSebEnableSwitchUser       , sendInterval);
	socketResult = SendEquationToSocketServer((char*)MSG_InsideSebEnableLockThisComputer , stringInsideSebEnableLockThisComputer , sendInterval);
	socketResult = SendEquationToSocketServer((char*)MSG_InsideSebEnableChangeAPassword  , stringInsideSebEnableChangeAPassword  , sendInterval);
	socketResult = SendEquationToSocketServer((char*)MSG_InsideSebEnableStartTaskManager , stringInsideSebEnableStartTaskManager , sendInterval);
	socketResult = SendEquationToSocketServer((char*)MSG_InsideSebEnableLogOff           , stringInsideSebEnableLogOff           , sendInterval);
	socketResult = SendEquationToSocketServer((char*)MSG_InsideSebEnableShutDown         , stringInsideSebEnableShutDown         , sendInterval);
	socketResult = SendEquationToSocketServer((char*)MSG_InsideSebEnableEaseOfAccess     , stringInsideSebEnableEaseOfAccess     , sendInterval);
	socketResult = SendEquationToSocketServer((char*)MSG_InsideSebEnableVmWareClientShade, stringInsideSebEnableVmWareClientShade, sendInterval);

	socketResult = SendEquationToSocketServer((char*)MSG_OutsideSebEnableSwitchUser       , stringOutsideSebEnableSwitchUser       , sendInterval);
	socketResult = SendEquationToSocketServer((char*)MSG_OutsideSebEnableLockThisComputer , stringOutsideSebEnableLockThisComputer , sendInterval);
	socketResult = SendEquationToSocketServer((char*)MSG_OutsideSebEnableChangeAPassword  , stringOutsideSebEnableChangeAPassword  , sendInterval);
	socketResult = SendEquationToSocketServer((char*)MSG_OutsideSebEnableStartTaskManager , stringOutsideSebEnableStartTaskManager , sendInterval);
	socketResult = SendEquationToSocketServer((char*)MSG_OutsideSebEnableLogOff           , stringOutsideSebEnableLogOff           , sendInterval);
	socketResult = SendEquationToSocketServer((char*)MSG_OutsideSebEnableShutDown         , stringOutsideSebEnableShutDown         , sendInterval);
	socketResult = SendEquationToSocketServer((char*)MSG_OutsideSebEnableEaseOfAccess     , stringOutsideSebEnableEaseOfAccess     , sendInterval);
	socketResult = SendEquationToSocketServer((char*)MSG_OutsideSebEnableVmWareClientShade, stringOutsideSebEnableVmWareClientShade, sendInterval);
*/

	// Close the socket, so the server loop
	// does not block its receive() all the time.
	// Open the socket again before SEB shuts down.
	// See method ShutdownInstance().

	//CloseSocket();

	logg(fp, "Leave GetClientInfo()\n\n");
	return TRUE;

} // end GetClientInfo()





//*************************************************
// Send the string "key=value" to the socket server
// ************************************************
int SendEquationToSocketServer(char* leftSide, char* rightSide, int sendInterval)
{
	//int socketResult;

	logg(fp, "Enter SendEquationToSocketServer()\n");

	logg(fp, "    leftSide = ***%s***\n",  leftSide);
	logg(fp, "   rightSide = ***%s***\n", rightSide);

	if (sendBuffer == NULL) return -1;

	sprintf(sendBuffer, "%s=%s", leftSide, rightSide);
	strcat (sendBuffer, endOfStringKeyWord);
	fflush (stdout);

 	//logg(fp, "sendBuffer = ***%s***\n", sendBuffer);
	//logg(fp, "\n");

	if (ConnectSocket != NULL)
	{
		socketResult = send(ConnectSocket, sendBuffer, (int)strlen(sendBuffer), 0);
		if (socketResult < 0) logg(fp, "   send() failed with error code %d\n", WSAGetLastError());
		logg(fp, "   Text sent : ***%s*** with %d Bytes\n", sendBuffer, socketResult);
	}

	strcpy(sendBuffer, "");
	fflush(stdout);

	// Wait [sendInterval] milliseconds
	// before sending the next message,
	// to avoid several messages sent together in one string
	// and "jamming" the socket pipe!
	// A send interval of 100 milliseconds = 1/10   second is too long.
	// A send interval of  10 milliseconds = 1/100  second is sufficient.
	// A send interval of   1 millisecond  = 1/1000 second is too short!
	Sleep(sendInterval);

	logg(fp, "Leave SendEquationToSocketServer()\n\n");
	return socketResult;

} // end SendEquationToSocketServer()





//******************************************************
// Receive the string "key=value" from the socket server
// *****************************************************
int RecvEquationOfSocketServer(char* leftSide, char* rightSide, int timeout)
{
	//int socketResult;

	logg(fp, "Enter ReceiveEquationFromSocketServer()\n");

	if (recvBuffer == NULL) return -1;

	strcpy(recvBuffer, "");
	fflush(stdout);

	if (ConnectSocket != NULL)
	{
		socketResult = recv(ConnectSocket, recvBuffer, BUFLEN, 0);
		if (socketResult < 0) logg(fp, "   recv() failed with error code %d\n", WSAGetLastError());
		logg(fp, "   Text received : ***%s*** with %d Bytes\n", recvBuffer, socketResult);
	}

	char* endOfStringKeyWordPointer  = strstr(recvBuffer, endOfStringKeyWord);
	if   (endOfStringKeyWordPointer != NULL)
	     *endOfStringKeyWordPointer  = '\0';

 	logg(fp, "   recvBuffer    = ***%s***\n", recvBuffer);

	// Extract the left and right side from the "leftSide=rightSide" message.
	// Actually, this could easily be done using the sscanf() function.
	// However, sscanf() did not work here, so lets take the hard way...
	//sscanf(recvBuffer, "%s=%s", leftSide, rightSide);

	strcpy( leftSide, recvBuffer);
	strcpy(rightSide, "");

	char* equalSignPointer  =        strstr( leftSide, "=");
	if   (equalSignPointer != NULL)  strcpy(rightSide, equalSignPointer + 1);
	if   (equalSignPointer != NULL) *equalSignPointer = '\0';

	fflush(stdout);

	logg(fp, "        leftSide = ***%s***\n",  leftSide);
	logg(fp, "       rightSide = ***%s***\n", rightSide);

	logg(fp, "Leave ReceiveEquationFromSocketServer()\n\n");
	return socketResult;

} // end RecvEquationFromSocketServer()





//*****************************************
// Open the socket connection to the server
// ****************************************
BOOL OpenSocket()
{
	// Create a SOCKET for connecting to the server
	logg(fp, "Creating the ConnectSocket...\n");
	ConnectSocket = socket(ai_family, ai_socktype, ai_protocol);
	if (ConnectSocket == INVALID_SOCKET)
	{
		logg(fp, "socket() failed with error code %d\n\n", WSAGetLastError());
		logg(fp, "Leave GetClientInfo()\n\n");
		WSACleanup();
		return FALSE;
	}
	else logg(fp, "ConnectSocket created\n\n");

	// Connect to the server
	logg(fp, "Connecting to server...\n");
	socketResult = connect(ConnectSocket, (SOCKADDR*) &clientService, sizeof(clientService));
	if (socketResult == SOCKET_ERROR)
	{
		logg(fp, "connect() failed with error code %d\n\n", WSAGetLastError());
		logg(fp, "Leave GetClientInfo()\n\n");
		closesocket(ConnectSocket);
		ConnectSocket = INVALID_SOCKET;
		WSACleanup();
		return FALSE;
	}
	else logg(fp, "Connected to server\n\n");

	return TRUE;
}





//******************************************
// Close the socket connection to the server
// *****************************************
BOOL CloseSocket()
{
	// Shutdown the ConnectSocket
	logg(fp, "Shutting down the ConnectSocket...\n");
	socketResult = shutdown(ConnectSocket, SD_SEND);
	if (socketResult == SOCKET_ERROR)
	{
		logg(fp, "shutdown() failed with error code %d\n\n", WSAGetLastError());
		closesocket(ConnectSocket);
		ConnectSocket = INVALID_SOCKET;
		WSACleanup();
		return FALSE;
	}
	else logg(fp, "ConnectSocket has been shut down\n\n");

	// Close the ConnectSocket
	closesocket(ConnectSocket);
	ConnectSocket = INVALID_SOCKET;
	WSACleanup();
	return TRUE;
}





//****************************************************
// Convert the given SID to a string format.
// Use the ConvertSidToStringSid() Win32 API function.
// Use the AtlThrow() function to signal errors.
//****************************************************
CString ConvertSidToString(PSID pSID)
{
	// Check input pointer
	ATLASSERT(pSID != NULL);
	if (pSID == NULL)
	{
		AtlThrow(E_POINTER);
	}

	// Get string corresponding to SID
	LPTSTR pszSID = NULL;
	if (!ConvertSidToStringSid(pSID, &pszSID))
	{
		AtlThrowLastWin32();
	}

	// Deep copy result in a CString instance
	CString strSID(pszSID);

	// Release buffer allocated by ConvertSidToStringSid API
	LocalFree(pszSID);
	pszSID = NULL;

	// Return string representation of the SID
	return strSID;
}





//******************************************************
// Determine the user SID from the current process token
//******************************************************
int DetermineUserSid(char* userSid)
{
	// Open the access token associated with the calling process
	HANDLE hToken = NULL;

	if (!OpenProcessToken(GetCurrentProcess(), TOKEN_QUERY, &hToken)) 
	{
		logg(fp, "OpenProcessToken failed. GetLastError returned: %d\n", GetLastError());
		return -1;
	}

	// Get the size of the memory buffer needed for the SID
	DWORD dwBufferSize = 0;
	if (!GetTokenInformation(hToken, TokenUser, NULL, 0, &dwBufferSize) &&
		(GetLastError() != ERROR_INSUFFICIENT_BUFFER))
	{
		logg(fp, "GetTokenInformation failed. GetLastError returned: %d\n", GetLastError());
		// Cleanup
		CloseHandle(hToken);
		hToken = NULL;
		return -1;
	}

	// Allocate buffer for user token data
	std::vector<BYTE> buffer;
	buffer.resize(dwBufferSize);
	PTOKEN_USER pTokenUser = reinterpret_cast<PTOKEN_USER>(&buffer[0]);

	// Retrieve the token information in a TOKEN_USER structure
	if (!GetTokenInformation(hToken, TokenUser, pTokenUser, dwBufferSize, &dwBufferSize)) 
	{
		logg(fp, "GetTokenInformation failed. GetLastError returned: %d\n", GetLastError());
		// Cleanup
		CloseHandle( hToken );
		hToken = NULL;
		return -1;
	}

    // Check if SID is valid
	if (!IsValidSid(pTokenUser->User.Sid)) 
	{
		logg(fp, "The owner SID is invalid\n");
		// Cleanup
		CloseHandle(hToken);
		hToken = NULL;
		return -1;
	}

	// Get and print the SID string
	CString userSidString = ConvertSidToString(pTokenUser->User.Sid).GetString();
	strcpy (userSid, userSidString);
	logg(fp, "userSidString = %s\n", userSidString);

	// Cleanup
	CloseHandle(hToken);
	hToken = NULL;
	return 0;

} // end of method DetermineUserSid()





BOOL CreateExternalProcess(string sProcess)
{
	//Examples find the path to winword.exe
	//char szBuf[256];
	//if (FindExecutable("D:\\Folders\\Eigene Dateien\\Vorlage.Doc", ".", szBuf ) > 31)
	{
	} 

	logg(fp, "\n");
	logg(fp, "Enter CreateExternalProcess()\n");

	long counter = 0;

	//MessageBox(NULL, cmdLine.c_str(), "Error", MB_ICONERROR);
	//SHGetSpecialFolderLocation
	STARTUPINFO			siProcess;				//STARTUPINFO     for created process
	PROCESS_INFORMATION piProcess;				//PROCESS_INFORMATION created process
	ZeroMemory(&siProcess, sizeof(siProcess));
	siProcess.cb         = sizeof(siProcess);
	if (IsNewOS && getBool("CreateNewDesktop"))
	{
		siProcess.lpDesktop = SEB_DESK;
	}
	ZeroMemory(&piProcess, sizeof(piProcess));

	// give the process 100s to start
	// after 100 s
	if (parameters.procedureReady != 0)
	{
		parameters.hold = 1;
		while (parameters.confirm != 1)
		{
			// Wait for the process to sync, but max 0.5 seconds
			Sleep(500);
			counter++;
			if (counter == 200)
			{
				logg(fp, "\t");
				logg(fp, "Timeout of 100 secs exceeded!\n");
				logg(fp, "Leave CreateExternalProcess()\n\n");
				return FALSE;
			}
		}
		parameters.hold = 0;
	}

	try
	{
		string applicationName = sProcess.c_str();
		//MessageBox(hWnd, applicationName.c_str(), "applicationName", MB_ICONERROR);
		logg(fp, "\tsProcess        = %s\n",        sProcess.c_str());
		logg(fp, "\tapplicationName = %s\n", applicationName.c_str());

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
			MessageBox(hWnd, messageText[languageIndex][IND_ProcessCallFailed], applicationName.c_str(), MB_ICONERROR);
			logg(fp, "\tError: creating process %s failed!\n", applicationName.c_str());
			logg(fp, "Leave CreateExternalProcess()\n\n");
			return FALSE;
		}

		// Add the process to the allowed processes, not required anymore
		//allowedProcesses.push_back(piProcess.dwProcessId);

		mpProcessInformations.insert(make_pair(sProcess, piProcess));
		//MessageBox(NULL, sProcess.c_str(), "sProcess =", MB_ICONERROR);
		logg(fp, "\tsProcess        = %s\n",        sProcess.c_str());
	}

	catch (char* str)
	{	
		//ResumeThread(procMonitorThread);
		MessageBox(NULL, str, "Error", MB_ICONERROR);
		logg(fp, "\tError: %s\n", str);
		logg(fp, "Leave CreateExternalProcess()\n\n");
		return FALSE;
	}

	//ResumeThread(procMonitorThread);
	logg(fp, "Leave CreateExternalProcess()\n\n");
	return TRUE;

} // end CreateExternalProcess()





BOOL ShutdownInstance()
{
	//MessageBox(NULL,"shutdown","Error",MB_ICONERROR);
	logg(fp, "Enter ShutdownInstance()\n\n");

	DWORD dwNotUsedForAnything = 0;	
	int ret;
	string sStrongKillProcesssesAfter = ""; 
	vector< string >vStrongKillProcessesAfter;

	char  leftSide[BUFLEN];
	char rightSide[BUFLEN];

	strcpy( leftSide, "");
	strcpy(rightSide, "");


	// Debug output of socket communication parameters
	logg(fp, " userName     = %s\n", userName);
	logg(fp, " hostName     = %s\n", hostName);
	logg(fp, " portNumber   = %d\n", portNumber);
	logg(fp, " sendInterval = %d\n", sendInterval);
	logg(fp, " numMessages  = %d\n", numMessages);
	logg(fp, "\n");


	// Reopen the socket connection to the server
	//socketResult = OpenSocket();
	//if (socketResult == TRUE)
	{
		// Tell the Seb Windows Service that we want to shutdown SEB,
		// so the Seb Windows Service can reset the registry keys
		// to their original values.
		socketResult = SendEquationToSocketServer("ShutDown",        "1", sendInterval);
		socketResult = RecvEquationOfSocketServer( leftSide , rightSide , recvTimeout);

		// Close the socket connection finally
		CloseSocket();
	}


	if (getBool("HookMessages"))
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

	sStrongKillProcesssesAfter = mpParam["StrongKillProcessesAfter"];
	if (sStrongKillProcesssesAfter != "")
	{		
		Tokenize(sStrongKillProcesssesAfter, vStrongKillProcessesAfter, ";");
		//MessageBox(hWnd, vKillProcess[1].c_str(), "Error", MB_ICONWARNING);
		//logg(fp, "vKillProcess[1] = %s\n", vKillProcess[1]);
		for (int i = 0; i < (int)vStrongKillProcessesAfter.size(); i++)
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
		if (getBool("Win9xKillExplorer") && killedExplorer)
		{
			system("start explorer.exe");	
		}
		if (getBool("Win9xScreenSaverRunning"))
		{
			SystemParametersInfo(SPI_SCREENSAVERRUNNING,FALSE,&dwNotUsedForAnything,NULL);
		}						
	}
	else
	{
		if (getBool("CreateNewDesktop"))
		{
			   SwitchDesktop(hOriginalInput);
			SetThreadDesktop(hOriginalThread);
				CloseDesktop(hNewDesktop);		
		}
	}

	// Close the logfile for debug output
	logg(fp, "Leave ShutdownInstance()\n\n");
	if (fp != NULL) fclose(fp);
	return TRUE;

} // end ShutdownInstance()





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
//	HANDLE hIcon, hIconSm;
	string applicationName;
	string appChooserTitle;

	HINSTANCE hInstance;
	HWND      hWndParent;
	HWND      hWndDesktop;
	HWND      hWndOwner;

	int    dialogRes;
	string quitPasswordEntered;
	string quitHashcodeEntered;
	string quitHashcodeStored;

	//logg(fp, "Enter WndProc()\n");

	if (languageIndex == IND_LanguageGerman ) appChooserTitle = "&Zugelassene Anwendungen";
	if (languageIndex == IND_LanguageEnglish) appChooserTitle = "&Permitted applications";
	if (languageIndex == IND_LanguageFrench ) appChooserTitle = "&Applications permies";


	switch (message)
	{
		case WM_CREATE:

		HMENU hMenu, hMenu2, hSubMenu, hSubMenu2;
		hMenu     = CreateMenu();
		hMenu2    = CreateMenu();
		hSubMenu  = CreatePopupMenu();
		hSubMenu2 = CreatePopupMenu();

		AppendMenu(hMenu, MF_STRING | MF_POPUP , (UINT)hSubMenu, appChooserTitle.c_str());

		// Append the permitted applications to the AppChooser menu
		for (itProcesses  = mpProcesses.begin();
			 itProcesses != mpProcesses.end();
			 itProcesses++)
		{
			// applicationName = name of the process.
			// If the "continue" command is active,
			// "Seb" is not appended to the third party application menu.
			applicationName = (*itProcesses).first;
			if (applicationName == "Seb") continue;
			AppendMenu(hSubMenu, MF_STRING,    cntProcess, applicationName.c_str());
			mpProcessCommands.insert(make_pair(cntProcess, applicationName));
			cntProcess ++;
		}

		// Append the entry for quitting SEB
		//AppendMenu(hSubMenu, MF_STRING, IDM_QUIT_PASSWORD, "Quit SEB...");
		SetMenu(hWnd, hMenu);



		case WM_COMMAND:

		wmId    = LOWORD(wParam);
		wmEvent = HIWORD(wParam);

		// Parse the menu selections:

		if (wmId == IDM_QUIT_PASSWORD)
		{
			logg(fp, "   \n");
			logg(fp, "   Quit SEB... selected, calling popup window for quit password...\n\n");

			// TODO: modal popup window for entering the quit password
			quitPasswordEntered = "";
			quitHashcodeEntered = "";
			quitHashcodeStored  = "";

			quitHashcodeStored = quitHashcode;

			//hWnd = CreateWindow(szWindowClass, szTitle, WS_MAXIMIZE, 10, 10, 200, 55, NULL, NULL, hInstance, NULL);
			MessageBox(NULL, "Enter quit password:", "SebStarter: Quit SEB", MB_OKCANCEL | MB_ICONQUESTION | MB_DEFBUTTON2);

			//hInstance = *hDll;
			//hInstance = NULL;

			hWndParent  = GetParent(hWnd);
			hWndDesktop = GetDesktopWindow();

			// Get the owner window and dialog box rectangles
								   hWndOwner = hWndParent;
			if (hWndOwner == NULL) hWndOwner = hWndDesktop;

			logg(fp, "   hWnd        = %d\n", hWnd);
			logg(fp, "   hWndParent  = %d\n", hWndParent);
			logg(fp, "   hWndDesktop = %d\n", hWndDesktop);
			logg(fp, "   hWndOwner   = %d\n", hWndOwner);

			dialogRes = DialogBox(hInst,
								  MAKEINTRESOURCE(IDD_DIALOG_QUIT_PASSWORD), 
								  hWnd, 
								  (DLGPROC)EnterQuitPasswordProc);

			logg(fp, "   IDOK      = %d\n", IDOK);
			logg(fp, "   IDCANCEL  = %d\n", IDCANCEL);
			logg(fp, "   dialogRes = %d\n", dialogRes);

			if (dialogRes == IDOK)
			{
				logg(fp, "   res = IDOK\n");

				// Complete the command:
				// quitPasswordEntered contains the entered password

				quitPasswordEntered = quitPassword;
				//quitHashcodeEntered = quitPasswordEntered.ComputeHashcode();

				// only temporarily for testing purposes
				quitPasswordEntered = "Davos";
				quitHashcodeEntered = "47E2361A7D358FA46394ACBCB899536D816774BE7B53AD8777BB23464DA54E";

				logg(fp, "   quitPasswordEntered = %s\n", quitPasswordEntered.c_str());
				logg(fp, "   quitHashcodeEntered = %s\n", quitHashcodeEntered.c_str());
				logg(fp, "   quitHashcodeStored  = %s\n", quitHashcodeStored .c_str());
				logg(fp, "\n");

				if (quitHashcodeEntered == quitHashcodeStored)
				{
					logg(fp, "\n\n");
					//TerminateProcess(hPiProcess->hProcess,0);
					SendMessage(hWnd, WM_DESTROY, NULL, NULL);
					logg(fp, "   SEB quit password entered correctly, therefore destroy window\n");
					//logg(fp, "Leave LLKeyboardHook() and return -1\n\n");
					return -1;
				}
			}
			else
			{
				logg(fp, "   res != IDOK\n");
				// Cancel the command.
			}
		} // end if (wmId == IDM_QUIT_PASSWORD)


		else if (wmId == IDM_ABOUT)
		{
			logg(fp, "   WM_COMMAND: IDM_ABOUT\n");
			MessageBox(NULL, "IDM_ABOUT", "WM_COMMAND", MB_ICONERROR);
			//DialogBox(hInst, MAKEINTRESOURCE(IDD_DIALOG_QUIT_PASSWORD), hWnd, reinterpret_cast<DLGPROC>(EnterQuitPasswordProc));
			//DialogBox(hInst, MAKEINTRESOURCE(IDD_ABOUTBOX), hWnd, About);
		}

		else if (wmId == IDM_EXIT)
		{
			logg(fp, "   WM_COMMAND: IDM_EXIT\n");
			MessageBox(NULL, "IDM_EXIT", "WM_COMMAND", MB_ICONERROR);
			//DialogBox(hInst, MAKEINTRESOURCE(IDD_DIALOG_QUIT_PASSWORD), hWnd, reinterpret_cast<DLGPROC>(EnterQuitPasswordProc));
			DestroyWindow(hWnd);
		}

		// supports 20 different processes
		else if ((wmId >= 37265) && (wmId <= 37285))
		{
			CreateExternalProcess(mpProcessCommands[wmId]);
		}

		else
		{
			//logg(fp, "Leave WndProc()\n\n");
			return DefWindowProc(hWnd, message, wParam, lParam);
		}

		break; // end case WM_COMMAND



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
		//logg(fp, "Leave WndProc()\n\n");
		return DefWindowProc(hWnd, message, wParam, lParam);

	} // end switch (message)

	//logg(fp, "Leave WndProc()\n\n");
	return 0;

} // end WndProc()





// *********************************************
//* Evaluates the Enter Quit Password dialog box
// *********************************************
LRESULT CALLBACK EnterQuitPasswordProc(HWND hWndDialog, UINT message, WPARAM wParam, LPARAM lParam)
{
	HWND hWndOwner; 
	RECT rc, rcDialog, rcOwner;

	switch (message) 
	{
		case WM_INITDIALOG:

			// Get the owner window and dialog box rectangles
			if ((hWndOwner = GetParent(hWndDialog)) == NULL) 
			{
				hWndOwner = GetDesktopWindow(); 
			}

			GetWindowRect(hWndOwner , &rcOwner); 
			GetWindowRect(hWndDialog, &rcDialog); 
			CopyRect(&rc, &rcOwner); 

			// Offset the owner and dialog box rectangles so that right and bottom 
			// values represent the width and height, and then offset the owner again 
			// to discard space taken up by the dialog box. 
			OffsetRect(&rcDialog, -rcDialog.left , -rcDialog.top   ); 
			OffsetRect(&rc      , -rc.left       , -rc.top         ); 
			OffsetRect(&rc      , -rcDialog.right, -rcDialog.bottom); 

			// The new position is the sum of half the remaining space and the owner's 
			// original position. 
			SetWindowPos(hWndDialog, HWND_TOP, 
						 rcOwner.left + (rc.right  / 2), 
						 rcOwner.top  + (rc.bottom / 2), 
						 0, 0,          // Ignores size arguments. 
						 SWP_NOSIZE); 

			if (GetDlgCtrlID((HWND) wParam) != IDC_MFCMASKEDEDIT_QUIT_PASSWORD) 
			{ 
				SetFocus(GetDlgItem(hWndDialog, IDC_MFCMASKEDEDIT_QUIT_PASSWORD)); 
				return FALSE; 
			}

			return TRUE; 
		// end case WM_INITDIALOG



		case WM_COMMAND:
			switch (LOWORD(wParam)) 
			{ 
				case IDOK: 
					if (!GetDlgItemText(hWndDialog, IDC_MFCMASKEDEDIT_QUIT_PASSWORD, quitPassword, 80)) 
						*quitPassword = 0; 
 
				// Fall through...
 
				case IDCANCEL: 
					EndDialog(hWndDialog, wParam); 
					return TRUE; 
			}

			break;
		// end case WM_COMMAND

	} // end switch (message)

	return FALSE;
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

} // end About()





/****************** Utility Functions  *********************/
BOOL CheckWritePermission(LPCSTR szPath) 
{
	logg(fp, "Enter CheckWritePermission()\n");
	DWORD dwAttr = GetFileAttributes(szPath);
	if (dwAttr == 0xffffffff)
	{
		DWORD dwError = GetLastError();
		if (dwError == ERROR_FILE_NOT_FOUND)
		{
			OutputErrorMessage(languageIndex, IND_FileNotFound, IND_MessageKindError);
			//MessageBox(hWnd,FILE_NOT_FOUND,"Error",MB_ICONERROR);
			//logg(fp, "Error: %s\n", FILE_NOT_FOUND);
			logg(fp, "Leave CheckWritePermission()\n\n");
			return FALSE;
		}
		else if (dwError == ERROR_PATH_NOT_FOUND)
		{
			OutputErrorMessage(languageIndex, IND_PathNotFound, IND_MessageKindError);
			//MessageBox(hWnd,PATH_NOT_FOUND,"Error",MB_ICONERROR);
			//logg(fp, "Error: %s\n", PATH_NOT_FOUND);
			logg(fp, "Leave CheckWritePermission()\n\n");
			return FALSE;
		}
		else if (dwError == ERROR_ACCESS_DENIED)
		{
			OutputErrorMessage(languageIndex, IND_AccessDenied, IND_MessageKindError);
			//MessageBox(hWnd,ACCESS_DENIED,"Error",MB_ICONERROR);
			//logg(fp, "Error: %s\n", ACCESS_DENIED);
			logg(fp, "Leave CheckWritePermission()\n\n");
			return FALSE;
		}
		else
		{
			OutputErrorMessage(languageIndex, IND_UndefinedError, IND_MessageKindError);
			//MessageBox(hWnd,UNDEFINED_ERROR,"Error",MB_ICONERROR);
			//logg(fp, "Error: %s\n", UNDEFINED_ERROR);
			logg(fp, "Leave CheckWritePermission()\n\n");
			return FALSE;
		}
	}
	else
	{
		if (dwAttr & FILE_ATTRIBUTE_DIRECTORY)
		{
/*
			// this is a directory
			if (dwAttr & FILE_ATTRIBUTE_ARCHIVE)
			// Directory is archive file
			if (dwAttr & FILE_ATTRIBUTE_COMPRESSED)
			// Directory is compressed
			if (dwAttr & FILE_ATTRIBUTE_ENCRYPTED)
			// Directory is encrypted
			if (dwAttr & FILE_ATTRIBUTE_HIDDEN)
			// Directory is hidden
*/
			if (dwAttr & FILE_ATTRIBUTE_READONLY)
			{
				logg(fp, "Leave CheckWritePermission()\n\n");
				return FALSE;
			}
			// Directory is read-only
/*
			if (dwAttr & FILE_ATTRIBUTE_REPARSE_POINT)
			// Directory has an associated reparse point
			if (dwAttr & FILE_ATTRIBUTE_SYSTEM)
			// Directory is part or used exclusively by the operating system
*/
		}
		else
		{
/*
			// this is an ordinary file
			if (dwAttr & FILE_ATTRIBUTE_ARCHIVE)
			// File is archive file
			if (dwAttr & FILE_ATTRIBUTE_COMPRESSED)
			// File is compressed
			if (dwAttr & FILE_ATTRIBUTE_ENCRYPTED)
			// File is encrypted
			if (dwAttr & FILE_ATTRIBUTE_HIDDEN)
			// File is hidden
			if (dwAttr & FILE_ATTRIBUTE_NOT_CONTENT_INDEXED)
			// File will not be indexed
			if (dwAttr & FILE_ATTRIBUTE_OFFLINE)
			// Data of file is not immediately available
*/
			if (dwAttr & FILE_ATTRIBUTE_READONLY)
			{
				logg(fp, "Leave CheckWritePermission()\n\n");
				return FALSE;
			}
			// File is read-only
/*
			if (dwAttr & FILE_ATTRIBUTE_REPARSE_POINT)
			// File has an associated reparse point
			if (dwAttr & FILE_ATTRIBUTE_SPARSE_FILE)
			// File is a sparse file
			if (dwAttr & FILE_ATTRIBUTE_SYSTEM)
			// File is part or used exclusively by the operating system
			if (dwAttr & FILE_ATTRIBUTE_TEMPORARY)
			// File is being used for temporary storage
*/
		}
	}

	logg(fp, "Leave CheckWritePermission()\n\n");
	return TRUE;

} // BOOL CheckWritePermission()





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
} // end Tokenize()



string getLangString(string key)
{
	string ret = "";
	string err = "";
	try
	{
		ret = mpParam[key + "_" + mpParam["DEFAULT_LANGUAGE"]];
		if (ret == "")
		{	
			err = messageText[languageIndex][IND_NoLanguageStringFound];
			err += "\n" + key;
			MessageBox(NULL, err.c_str(), "Error",16);
			logg(fp, "Error: %s\n", err);
		}
		return ret;
	}
	catch (char* str)
	{		
		MessageBox(NULL, str, "Error", 16);
		logg(fp, "Error: %s\n", str);
		return "";
	}	
} // end getLangString()



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
		MessageBox(NULL, str, "Error", 16);
		logg(fp, "Error: %s\n", str);
		return FALSE;
	}	
} // end getBool()



int getInt(string key)
{
	try
	{
		return (atoi(mpParam[key].c_str()));
	}
	catch (char* str)
	{		
		MessageBox(NULL, str, "Error", 16);
		logg(fp, "Error: %s\n", str);
		return FALSE;
	}
} // end getInt()





BOOL HandleOpenRegistryKey(HKEY hKey, LPCSTR subKey, PHKEY pKey, BOOL bCreate)
{
	logg(fp, "   Enter HandleOpenRegistryKey()\n");

	try
	{
		long lngRegOpen;
		long lngRegCreate;

		lngRegOpen = RegOpenKey(hKey, subKey, pKey);

		//MessageBox(NULL,subKey,"RegOpenKey(hKey, subKey, pKey)",16);

		// What the heck is meant by "ERROR_SUCCESS"???
		// Was the operation a success or did it produce an error?
		// This makes absolutely no sense!

		if (lngRegOpen == ERROR_SUCCESS) //MessageBox(hWnd, " = ERROR_SUCCESS", "lngRegOpen", 16);
		if (lngRegOpen != ERROR_SUCCESS)   MessageBox(hWnd, "!= ERROR_SUCCESS", "lngRegOpen", 16);

		if (lngRegOpen != ERROR_SUCCESS)
		{   logg(fp, "  HandleOpenRegistryKey() ERROR_SUCCESS???\n");
			switch (lngRegOpen)
			{
				case ERROR_SUCCESS:
					//MessageBox(hWnd, "HandleOpenRegistryKey(): Yes, enough rights for editing registry 1", "Registry enough rights 1", 16);
					break;
				case ERROR_FILE_NOT_FOUND :
					if (bCreate)
					{
						lngRegCreate = RegCreateKey(hKey, subKey, pKey);
						switch (lngRegCreate)
						{
							case ERROR_SUCCESS:
								//MessageBox(hWnd,"HandleOpenRegistryKey(): Yes, enough rights for editing registry 2","Registry enough rights 2",16);
								break;
							case ERROR_ACCESS_DENIED:
							  //MessageBox(hWnd,"HandleOpenRegistryKey(): Not enough rights for editing registry",REGISTRY_WARNING,MB_ICONWARNING);
							  //MessageBox(hWnd, NOT_ENOUGH_REGISTRY_RIGHTS_ERROR, REGISTRY_WARNING, MB_ICONWARNING);
								logg(fp, "   Leave HandleOpenRegistryKey()\n");
								return FALSE;
								break;
							default:
								logg(fp, "   Leave HandleOpenRegistryKey()\n");
								return FALSE;
						}
					}
					break;
				default :
					logg(fp, "   Leave HandleOpenRegistryKey()\n");
					return FALSE;
			} // end switch
		} // end if
	} // end try

	catch (char* str)
	{		
		MessageBox(NULL, str, "Error", 16);
		logg(fp, "   Error: %s\n", str);
		logg(fp, "   Leave HandleOpenRegistryKey()\n");
		return FALSE;
	}

	logg(fp, "   Leave HandleOpenRegistryKey()\n");
	return TRUE;

} // end HandleOpenRegistryKey()




/*
BOOL HandleSetRegistryKeyValue(HKEY hKey, LPCSTR lpVal, string sParam)
{
	DWORD type;
	DWORD dwLen = sizeof(DWORD);
	DWORD val   = 1;

	long lngRegSet;
	long lngRegGet;

	logg(fp, "Enter HandleSetRegistryKeyValue()\n");

	try
	{
		lngRegGet = RegQueryValueEx(hKey, lpVal, NULL, &type, (LPBYTE)&val, &dwLen);
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
					OutputErrorMessage(languageIndex, IND_NotEnoughRegistryRightsError, IND_MessageKindError);
					//MessageBox(hWnd,NOT_ENOUGH_REGISTRY_RIGHTS_ERROR,REGISTRY_WARNING,MB_ICONWARNING);
					logg(fp, "Leave HandleSetRegistryKeyValue()\n");
					return FALSE;
				default :
					mpParam[sParam] = "0";
					logg(fp, "Leave HandleSetRegistryKeyValue()\n");
					return FALSE;
			}
		}
	}

	catch (char* str)
	{		
		MessageBox(NULL, str, "Error", 16);
		logg(fp, "Error: %s\n", str);
		logg(fp, "Leave HandleSetRegistryKeyValue()\n");
		return FALSE;
	}

	logg(fp, "Leave HandleSetRegistryKeyValue()\n");
	return TRUE;

} // end HandleSetRegistryKeyValue()
*/




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
} // end SetVersionInfo()



BOOL ShowSebApplicationChooser()
{
	BOOL retVal = FALSE;
	BOOL inReg  = FALSE;
	try
	{
		HKEY  hKeySeb;
		DWORD showSebApplicationChooser;
		DWORD dwType =    REG_DWORD;
		DWORD dwSize = sizeof(DWORD);
		LONG returnStatus;
		if (HandleOpenRegistryKey(HKLM, KEY_PoliciesSeb, &hKeySeb, FALSE))
		{
			returnStatus = RegQueryValueEx(hKeySeb, VAL_ShowSebApplicationChooser, NULL, &dwType,(LPBYTE)&showSebApplicationChooser, &dwSize);
			if (returnStatus == ERROR_SUCCESS)
			{
				inReg  = TRUE;
				retVal = showSebApplicationChooser;
			}
			RegCloseKey(hKeySeb);
		}
	}

	catch (char* str)
	{
		MessageBox(NULL, str, "Error", MB_ICONERROR);
	}

	if (!inReg)
	{
		retVal = getBool("ShowSebApplicationChooser");
	}

	return retVal;

} // end ShowSebApplicationChooser()
