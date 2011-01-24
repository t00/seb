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

#pragma once

#include "resource.h"
#include "../SystemVersionInfo.h"

// Name and location of Seb configuration file and logfile
#define SEB_INI_FILE "../Seb.ini"
#define SEB_LOG_FILE "Seb.log"

// Function for easier writing into the logfile
#define logg if (fp != NULL) fprintf

// C structures for logfile handling
bool  logFileDesired = true;
char* logFileName    = SEB_LOG_FILE;
FILE* fp;


// Socket structures for IPC (interprocess communication)
// between SEB (client) and Windows service (server)

#define BUFLEN 512

WSADATA wsaData;
SOCKET  ConnectSocket = INVALID_SOCKET;
int     result;

//char *message  = "$GPRMC,162614,A,5230.5900,N,01322.3900,E,10.0,90.0,131006,1.2,E,A*13";
char   *message1 = "$GPRMC,072436.0000,V,4723.0490,N,00833.2779,E,,311.41,181007,,*0B,AUTO";
char   *message2 = "$GPRMC,072436.00,V,4723.0490,N,00833.2779,E,,311.41,181007,,*0B,AUTO";
char   *message3 = "$GPRMC,072436.00,V,4723.0490,N,833.2779,E,,311.41,181007,,*0B,AUTO";
char    sendBuf[BUFLEN];

char*   hostName   = "";
char*   userName   = "";
int     portNumber = 0;
int     interval   = 10;
int     messages   = 1000000;

struct hostent *remoteHost;
struct in_addr  addr;


// Other constants
#define IDM_START   9001
#define IDM_OFFSET 37265

#define MAX_LOADSTRING 100
#define SEB_DESK       "SEB Desktop"
#define WINKEYOX_DESK  "WinKeyox Desktop"

//System Messages
#define	FILE_NOT_FOUND           "File not found!"
#define	PATH_NOT_FOUND           "Path not found!"
#define	ACCESS_DENIED            "Access denied!"
#define	UNDEFINED_ERROR          "Undefined error in CheckWritePermission!"
#define NO_WRITE_PERMISSION      "No write permission!"
#define	NO_INI_ERROR             "No *.ini File found!"
#define	NO_CLIENT_INFO_ERROR     "No client info!"
#define	INITIALIZE_ERROR         "Initialization failed!"
#define	REG_EDIT_ERROR           "Error editing the registry!"

#define	NOT_ENOUGH_REGISTRY_RIGHTS_ERROR "Not enough rights to edit registry keys!"

#define REGISTRY_WARNING         "Registry Warning!"
#define PROCESS_FAILED           "Process call failed!"

#define PROCESS_WINDOW_NOT_FOUND "No process window found!"

#define LOAD_LIBRARY_ERROR       "Could not load library!"
#define	NO_LANGSTRING_FOUND      "No language string found!"
#define	NO_INSTANCE              "Could not get my instance!"
#define	NO_FILE_ERROR            "Could not create or find file!"
#define	NO_TASKBAR_HANDLE        "No taskbar handle!"
#define	FIREFOX_START_FAILED     "Could not start firefox!"
#define KEYLOGGER_FAILED         "Could not start KeyLogger!"

#define KIOX_TERMINATED          "Kiox terminated!"
#define SEB_TERMINATED           "SEB terminated!"

#define	NO_OS_SUPPORT            "The OS is not supported!"
#define	KILL_PROC_FAILED         "Killing process %s failed: %d"


// HKEY_CURRENT_USER contains the Windows Registry keys
//
// DisableLockWorkstation ("Lock this computer")
// DisableChangePassword  ("Change a password")
// DisableTaskMgr         ("Start Task Manager")
// NoLogoff               ("Log off")
// NoClose                ("No shutdown symbol in lower right desktop corner???")
// EnableShade            ("Enable Shade on VMware virtual desktop")
//
// HKEY_LOCAL_MACHINE contains the Windows Registry key
//
// HideFastUserSwitching  ("Switch User")

#define HKCU HKEY_CURRENT_USER
#define HKLM HKEY_LOCAL_MACHINE

#define SHOW_WIN_HIDE      0
#define SHOW_WIN_NORMAL    1
#define SHOW_WIN_MINIMIZED 2
#define GINA_PATH TEXT("MSGINA.DLL")



LPCTSTR KEY_RegPolicySystem   = "Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\System";
LPCTSTR KEY_RegPolicyExplorer = "Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\Explorer";
LPCTSTR KEY_RegPolicySEB      = "Software\\Policies\\SEB";
LPCTSTR KEY_RegVMwareClient   = "Software\\VMware, Inc.\\VMware VDM\\Client";

LPCTSTR VAL_HideFastUserSwitching  = "HideFastUserSwitching";
LPCTSTR VAL_DisableLockWorkstation = "DisableLockWorkstation";
LPCTSTR VAL_DisableChangePassword  = "DisableChangePassword";
LPCTSTR VAL_DisableTaskMgr         = "DisableTaskMgr";
LPCTSTR VAL_NoLogoff               = "NoLogoff";
LPCTSTR VAL_NoClose                = "NoClose";
LPCTSTR VAL_EnableShade            = "EnableShade";


// Only for trunk version necessary (XULrunner)

LPCTSTR VAL_PERMITTED_APPS    = "PermittedApps";
LPCTSTR VAL_ShowSEBAppChooser = "ShowSEBAppChooser";