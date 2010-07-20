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
 * The Original Code provides message hooks. 
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

// MsgHook.cpp : Defines the entry point for the DLL application.
//

#include "stdafx.h"
#include "MsgHook.h"

#ifdef _MANAGED
#pragma managed(push, off)
#endif

////////////////
// The section is SHARED among all instances of this DLL.
// A low-level keyboard hook is always a system-wide hook.
// 

#pragma data_seg (".mydata")
static HHOOK g_hHookKbdLL   = NULL;	// Low Level Key hook handle (NT)
static HHOOK g_hHookMouseLL = NULL;	// Low Level Mouse hook handle (NT)
static HHOOK g_hHookKbd     = NULL;	// Key   hook handle (9x)
static HHOOK g_hHookMouse   = NULL;	// Mouse hook handle (9x)
static HWND  hWndCaller     = NULL;	//Handle Pointer to the caller window 
//static HWND hWndProcess   = NULL;	//Handle Pointer to the process window 
static BOOL bHotKeyKill = FALSE;
static ALTER_FLAGS alter_flags; //AlterFlags struct with flags for alternating system functions
#pragma data_seg ()
#pragma comment(linker, "/SECTION:.mydata,RWS") // tell linker: make it shared

HINSTANCE *hDll;	//the same pointer as hModule
HMODULE   *hMod;	//the same pointer as hDll
PROCESS_INFORMATION *hPiProcess;
BOOL      InitMsgHook();
BOOL TerminateMsgHook();
BOOL getBool(string);
int  getInt (string);
BOOL isValidOperatingSystem();
int GetButtonForHotKeyFromRegistry(LPCTSTR);
BOOL b1, b2, b3; //for hot key
DWORD VK_B1, VK_B2, VK_B3;
map<string, string> mpParam;//map for *.ini parameters
SystemVersionInfo sysVersionInfo; 
ofstream of;



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



/* private hook functions */
LRESULT CALLBACK LLKeyboardHook( int nCode, WPARAM wParam, LPARAM lParam )
{	
	if (nCode < 0 || nCode != HC_ACTION ) 
	{
		return CallNextHookEx( g_hHookKbdLL, nCode, wParam, lParam); 
	}
	BOOL bEatKeystroke;
	BOOL bCtrlKeyDown = GetAsyncKeyState(VK_CONTROL)>>((sizeof(SHORT) * 8) - 1);
	
	KBDLLHOOKSTRUCT* p = (KBDLLHOOKSTRUCT*)lParam;
	BOOL bAltKeyDown = p->flags & LLKHF_ALTDOWN;
    switch (wParam) 
    {
        case WM_KEYDOWN:  
        case WM_KEYUP: 
		case WM_SYSKEYDOWN:	
		case WM_SYSKEYUP:		
        {			
			bEatKeystroke = false;			
			/* hotkeys */
			/* every keyup resets hotkey pressed flags */
			if (wParam == WM_KEYUP || wParam == WM_SYSKEYUP)
			{								
					b1 = FALSE;
					b2 = FALSE;					
					b3 = FALSE;				
			}
			if (wParam == WM_KEYDOWN || wParam == WM_SYSKEYDOWN)
			/* HotKey */			
			{
				if (bHotKeyKill)
				{ 
					/* every other keydowns resets hotkey pressed flags */			
					if ((p->vkCode != VK_B1) && (p->vkCode != VK_B2) && (p->vkCode != VK_B3))
					{					
						b1 = FALSE;
						b2 = FALSE;				
						b3 = FALSE;
					}
					else
					{		
						if ((b1 == TRUE) && (b2 == TRUE) && (b3 == TRUE) && (p->vkCode == VK_B3))
						{			
							//TerminateProcess(hPiProcess->hProcess,0);
							SendMessage(hWndCaller,WM_DESTROY,NULL,NULL);
							return -1;
						}

						if (p->vkCode == VK_B1) 
						{
							b1 = TRUE;	
							//return -1;						
						}
						if (p->vkCode == VK_B2)
						{
							b2 = TRUE;
							//return -1;
						}
						if (p->vkCode == VK_B3)
						{
							b3 = TRUE;
							//return -1;
						}
					}
				}
				/* some keys to eat */
				if (
						((alter_flags.DISABLE_START_MENU && ((p->vkCode == VK_LWIN) || (p->vkCode == VK_RWIN)))) ||						
						((alter_flags.DISABLE_ALT_ESC && ((p->vkCode==VK_ESCAPE && bAltKeyDown)))) ||
						((alter_flags.DISABLE_ALT_TAB && ((p->vkCode==VK_TAB && bAltKeyDown)))) ||
						((alter_flags.DISABLE_ALT_F4 && ((p->vkCode==VK_F4 && bAltKeyDown)))) ||
						((alter_flags.DISABLE_CTRL_ESC && ((p->vkCode==VK_ESCAPE && bCtrlKeyDown)))) ||					
						((alter_flags.DISABLE_F1 && p->vkCode == VK_F1)) ||
						((alter_flags.DISABLE_F2 && p->vkCode == VK_F2)) ||
						((alter_flags.DISABLE_F3 && p->vkCode == VK_F3)) ||
						((alter_flags.DISABLE_F4 && p->vkCode == VK_F4)) ||
						((alter_flags.DISABLE_F5 && p->vkCode == VK_F5)) ||
						((alter_flags.DISABLE_F6 && p->vkCode == VK_F6)) ||
						((alter_flags.DISABLE_F7 && p->vkCode == VK_F7)) ||
						((alter_flags.DISABLE_F8 && p->vkCode == VK_F8)) ||
						((alter_flags.DISABLE_F9 && p->vkCode == VK_F9)) ||
						((alter_flags.DISABLE_F10 && p->vkCode == VK_F10)) ||
						((alter_flags.DISABLE_F11 && p->vkCode == VK_F11)) ||
						((alter_flags.DISABLE_F12 && p->vkCode == VK_F12)) ||
						((alter_flags.DISABLE_ESCAPE && p->vkCode == VK_ESCAPE))
					) 
				{
					bEatKeystroke = true;
				}
			}				
		}
    }
	
    if ( bEatKeystroke )
	{	
        return -1;
	}
    else
	{
        return CallNextHookEx( g_hHookKbdLL, nCode, wParam, lParam );
	}
}

LRESULT CALLBACK KeyboardHook( int nCode, WPARAM wParam, LPARAM lParam )
{
	BOOL KeyUp, KeyDown, bAltKeyDown, bCtrlKeyDown;
	
	if (nCode < 0 || nCode != HC_ACTION ) 
	{
		return CallNextHookEx( g_hHookKbd, nCode, wParam, lParam); 
	}
	if ((lParam & (1 << 31)) == 0) //Bit 31 set?
	{		
		KeyUp = FALSE;
		KeyDown = TRUE;
	}
	else
	{
		KeyUp = TRUE;
		KeyDown = FALSE;		
	}
	bCtrlKeyDown = GetAsyncKeyState(VK_CONTROL)>>((sizeof(SHORT) * 8) - 1);
	bAltKeyDown = ((lParam & (1 << 29)) != 0) ? TRUE : FALSE;
	
	if (KeyDown || KeyUp)
	{			
		/* hotkeys */
		/* every keyup resets hotkey pressed flags */
		if (KeyUp)
		{								
				b1 = FALSE;
				//of << "Erase KeyUp b1\n";
				b2 = FALSE;		
				//of << "Erase KeyUp  b2\n";
				b3 = FALSE;				
				//of << "Erase KeyUp b3\n";
		}
		/* HotKey */
		if (KeyDown)
		{		
			if (bHotKeyKill)
			{				
				/* every other keydowns resets hotkey pressed flags */			
				if ((wParam != VK_B1) && (wParam != VK_B2) && (wParam != VK_B3))
				{					
					b1 = FALSE;
					b2 = FALSE;				
					b3 = FALSE;
				}
				else
				{		
					if ((b1 == TRUE) && (b2 == TRUE) && (b3 == TRUE) && (wParam == VK_B3))
					{	
						//TerminateProcess(hPiProcess->hProcess,0);
						SendMessage(hWndCaller,WM_DESTROY,NULL,NULL);
						return -1;						
					}

					if (wParam == VK_B1) 
					{
						b1 = TRUE;	
						//return -1;						
					}
					if (wParam == VK_B2)
					{
						b2 = TRUE;
						//return -1;
					}
					if (wParam == VK_B3)
					{
						b3 = TRUE;
						//return -1;
					}
				}
			}			
			/* some keys to eat */			
			if (		
					((alter_flags.DISABLE_START_MENU && ((wParam == VK_LWIN) || (wParam == VK_RWIN)))) ||	//no effect					
					((alter_flags.DISABLE_ALT_ESC && ((wParam == VK_ESCAPE && bAltKeyDown)))) ||			//no effect
					((alter_flags.DISABLE_ALT_TAB && ((wParam == VK_TAB && bAltKeyDown)))) ||				//no effect
					((alter_flags.DISABLE_ALT_F4 && ((wParam == VK_F4 && bAltKeyDown)))) ||
					((alter_flags.DISABLE_CTRL_ESC && ((wParam == VK_ESCAPE && bCtrlKeyDown)))) ||			//no effect
					((alter_flags.DISABLE_F1 && wParam == VK_F1)) ||
					((alter_flags.DISABLE_F2 && wParam == VK_F2)) ||
					((alter_flags.DISABLE_F3 && wParam == VK_F3)) ||
					((alter_flags.DISABLE_F4 && wParam == VK_F4)) ||
					((alter_flags.DISABLE_F5 && wParam == VK_F5)) ||
					((alter_flags.DISABLE_F6 && wParam == VK_F6)) ||
					((alter_flags.DISABLE_F7 && wParam == VK_F7)) ||
					((alter_flags.DISABLE_F8 && wParam == VK_F8)) ||
					((alter_flags.DISABLE_F9 && wParam == VK_F9)) ||
					((alter_flags.DISABLE_F10 && wParam == VK_F10)) ||
					((alter_flags.DISABLE_F11 && wParam == VK_F11)) ||
					((alter_flags.DISABLE_F12 && wParam == VK_F12))
				) 
			{	
			//MessageBox(NULL,"Eat","Error",16);
			return -1;			
			}
		}
	}
	return CallNextHookEx( g_hHookKbd, nCode, wParam, lParam );
}

LRESULT CALLBACK LLMouseHook(int nCode, WPARAM wParam, LPARAM lParam) {	
	if (nCode < 0 || nCode != HC_ACTION ) 
	{
		return CallNextHookEx( g_hHookMouseLL, nCode, wParam, lParam); 
	}
    if((wParam==WM_RBUTTONUP || wParam==WM_RBUTTONDOWN) && alter_flags.DISABLE_RIGHT_MOUSE) {			
        return -1;
	}    
    return CallNextHookEx(g_hHookMouseLL, nCode, wParam, lParam);
}

LRESULT CALLBACK MouseHook( int nCode, WPARAM wParam, LPARAM lParam ) 
{
	if (nCode < 0 || nCode != HC_ACTION ) 
	{		
		return CallNextHookEx( g_hHookMouseLL, nCode, wParam, lParam); 
	}
	if((wParam==WM_RBUTTONUP || wParam==WM_RBUTTONDOWN) && alter_flags.DISABLE_RIGHT_MOUSE) 
	{			
        return -1;
	}
	return CallNextHookEx(g_hHookMouse, nCode, wParam, lParam);
}

EXPORT void KeyHookNT(HINSTANCE *hDLL, bool setHook) 
{
    if(setHook) 
	{
		g_hHookKbdLL = SetWindowsHookEx(WH_KEYBOARD_LL, (HOOKPROC)LLKeyboardHook, (HINSTANCE)*hDLL, 0);
    } 
	else 
	{
		UnhookWindowsHookEx(g_hHookKbdLL);
    }
    return;
}

EXPORT void KeyHook9x(HINSTANCE *hDLL, bool setHook) 
{
	if(setHook) 
	{				
		g_hHookKbd = SetWindowsHookEx(WH_KEYBOARD, (HOOKPROC)KeyboardHook, (HINSTANCE)*hDLL, 0);
    }	
	else 
	{
		UnhookWindowsHookEx(g_hHookKbd);
    }
    return;
}

EXPORT void MouseHookNT(HINSTANCE *hDLL, bool setHook) 
{
    if(setHook) 
	{		
		g_hHookMouseLL = SetWindowsHookEx(WH_MOUSE_LL, (HOOKPROC)LLMouseHook, (HINSTANCE)*hDLL, 0);
    } 
	else 
	{
       UnhookWindowsHookEx(g_hHookMouseLL);
    }
    return;
}

EXPORT void MouseHook9x(HINSTANCE *hDLL, bool setHook) 
{
	if(setHook) 
	{				
		g_hHookMouse = SetWindowsHookEx(WH_MOUSE, (HOOKPROC)MouseHook, (HINSTANCE)*hDLL, 0);
    } 
	else 
	{
		UnhookWindowsHookEx(g_hHookMouse);
    }
    return; 
}

BOOL InitMsgHook()
{ 
	string strLine = "";
	string strKey = "";
	string strValue = "";
	char cCurrDir[MAX_PATH];
	string sCurrDir = "";
	string sHotKey = "";
		
	try
	{
		if (!isValidOperatingSystem()) 
		{
			return FALSE;
		}

		GetModuleFileName(*hMod,cCurrDir,sizeof(cCurrDir));
		sCurrDir = (string)cCurrDir;
		sCurrDir.replace(((size_t)sCurrDir.length()-3), 3, "ini");
		ifstream inf(sCurrDir.c_str());	
		if (!inf.is_open()) 
		{
			MessageBox(NULL,NO_INI_ERROR,"Error",16);
			return FALSE;
		}
		while(!getline(inf, strLine).eof())
		{			
			strKey = strLine.substr(0,strLine.find("=", 0));
			strValue = strLine.substr(strLine.find("=", 0)+1,strLine.length());
			mpParam[strKey] = strValue;				
		}
		inf.close();
		
		//setting bits of alter_flags_structs
		alter_flags.DISABLE_CTRL_ESC = getBool("DISABLE_CTRL_ESC");
		alter_flags.DISABLE_ALT_TAB = getBool("DISABLE_ALT_TAB");
		alter_flags.DISABLE_ALT_ESC = getBool("DISABLE_ALT_ESC");
		alter_flags.DISABLE_ALT_F4 = getBool("DISABLE_ALT_F4");
		alter_flags.DISABLE_START_MENU = getBool("DISABLE_START_MENU");
		alter_flags.DISABLE_RIGHT_MOUSE = getBool("DISABLE_RIGHT_MOUSE");
		alter_flags.DISABLE_LEFT_MOUSE = getBool("DISABLE_LEFT_MOUSE");
		alter_flags.DISABLE_F1 = getBool("DISABLE_F1");
		alter_flags.DISABLE_F2 = getBool("DISABLE_F2");
		alter_flags.DISABLE_F3 = getBool("DISABLE_F3");
		alter_flags.DISABLE_F4 = getBool("DISABLE_F4");
		alter_flags.DISABLE_F5 = getBool("DISABLE_F5");
		alter_flags.DISABLE_F6 = getBool("DISABLE_F6");
		alter_flags.DISABLE_F7 = getBool("DISABLE_F7");
		alter_flags.DISABLE_F8 = getBool("DISABLE_F8");
		alter_flags.DISABLE_F9 = getBool("DISABLE_F9");
		alter_flags.DISABLE_F10 = getBool("DISABLE_F10");
		alter_flags.DISABLE_F11 = getBool("DISABLE_F11");
		alter_flags.DISABLE_F12 = getBool("DISABLE_F12");
		alter_flags.DISABLE_ESCAPE = getBool("DISABLE_ESCAPE");
		// Kill Caller with HotKey
		sHotKey = mpParam["KILL_CALLER_HOTKEY"];

		//Hotkey from Registry (1. priority) and configuration file MsgHook.ini (2. priority). If nothing is found -> default is F3 + F11 + F6.
		VK_B1 = (GetButtonForHotKeyFromRegistry(VAL_Button1) ? GetButtonForHotKeyFromRegistry(VAL_Button1) : (DWORD)getInt("B1") ? (DWORD)getInt("B1") : VK_F3);				
		VK_B2 = (GetButtonForHotKeyFromRegistry(VAL_Button2) ? GetButtonForHotKeyFromRegistry(VAL_Button2) : (DWORD)getInt("B2") ? (DWORD)getInt("B2") : VK_F11);				
		VK_B3 = (GetButtonForHotKeyFromRegistry(VAL_Button3) ? GetButtonForHotKeyFromRegistry(VAL_Button3) : (DWORD)getInt("B3") ? (DWORD)getInt("B3") : VK_F6);

		if (hWndCaller == NULL) 
		{
			hWndCaller = FindWindow(NULL,sHotKey.c_str());
		}
		if (hWndCaller != NULL)
		{
			if (sHotKey != "") bHotKeyKill = TRUE;
		}
		else
		{
			MessageBox(NULL,"No caller window found!","Error",16);
		}
	}
	catch( char * str )
	{					
		return FALSE;
	}
	return TRUE;
}

int GetButtonForHotKeyFromRegistry(LPCTSTR regButton) 
{
	int retValue;
	retValue = 0;
	try
	{
		HKEY hKey;
		DWORD b1;
		LONG retStatus;
		DWORD dwType=REG_DWORD;
		DWORD dwSize=sizeof(DWORD);
		retStatus = RegOpenKeyEx(HKLM, KEY_RegPolicySEB, 0, KEY_READ, &hKey);
		if (retStatus == ERROR_SUCCESS)
		{
			retStatus = RegQueryValueEx(hKey, regButton, NULL, &dwType,(LPBYTE)&b1, &dwSize);
			if (retStatus == ERROR_SUCCESS)
			{
				retValue = b1;
			}
			RegCloseKey(hKey);
		}
		return retValue;
	}
	catch( char * str )
	{		
		MessageBox(NULL, str, "Error", MB_ICONERROR);
		return retValue;
	}
}

BOOL APIENTRY DllMain( HMODULE hModule,
                       DWORD  ul_reason_for_call,
                       LPVOID lpReserved
					 )
{
	// Perform actions based on the reason for calling.
    switch( ul_reason_for_call ) 
    { 
        case DLL_PROCESS_ATTACH:
			hMod = &hModule;
			hDll = (HINSTANCE*)&hModule;
			if (!InitMsgHook()) 
			{
				MessageBox(NULL,INITIALIZE_ERROR,"Error",16);
			}
            break;

        case DLL_THREAD_ATTACH:
			// Do thread-specific initialization.
            break;

        case DLL_THREAD_DETACH:
			// Do thread-specific cleanup.
            break;

        case DLL_PROCESS_DETACH:
			// Perform any necessary cleanup.
            break;
    }
    return TRUE;
}

BOOL getBool(string key)
{
	BOOL ret = FALSE;
	try
	{
		ret = (atoi(mpParam[key].c_str()) > 0 ) ? TRUE : FALSE;
		return ret;
	}
	catch( char * str )
	{				
		return FALSE;
	}	
}

int getInt(string key)
{
	try
	{
		return (atoi(mpParam[key].c_str()));
	}
	catch( char * str )
	{		
		return FALSE;
	}
}

BOOL isValidOperatingSystem() 
{
	switch (sysVersionInfo.GetVersion())
	{			
		case WIN_NT_351 :
		case WIN_NT_40  :
		case WIN_2000 :
		case WIN_XP :
		case WIN_VISTA :
		case WIN_95 :
		case WIN_98 :
		case WIN_ME :
			return TRUE;
			break;
		default :
			MessageBox(NULL,NO_OS_SUPPORT,"Error",16);
			return FALSE;			
	}
}

#ifdef _MANAGED
#pragma managed(pop)
#endif