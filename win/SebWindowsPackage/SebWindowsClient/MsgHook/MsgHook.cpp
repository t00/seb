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

//
// MsgHook.cpp:
// Defines the entry point for the DLL application,
// intercepting and suppressing mouse and key events.
//

#include "stdafx.h"
#include "MsgHook.h"
#include "resource.h"
#include "../ErrorMessage.h"   // multilingual (German, English, French)


// C structures for logfile handling
extern char programDataDirectory[MAX_PATH];
extern bool logFileDesiredMsgHook;
extern bool logFileDesiredSebStarter;
extern char logFileDirectory [BUFLEN];
extern char logFileMsgHook   [BUFLEN];
extern char logFileSebStarter[BUFLEN];
extern char iniFileDirectory [BUFLEN];
extern char iniFileMsgHook   [BUFLEN];
extern char iniFileSebStarter[BUFLEN];
extern char examUrl          [BUFLEN];
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
BOOL      ReadMsgHookIni();
BOOL TerminateMsgHook();
BOOL getBool(string);
int  getInt (string);
//BOOL IsNewOS; //NT4, W2k, XP, ...
BOOL isValidOperatingSystem();
int  GetButtonForHotKeyFromRegistry(LPCTSTR);
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





// **********************************************
//* Gets the name of a mouse event in string form
// **********************************************
string GetMouseName(WPARAM wParam)
{
  //logg(fp, "   Enter GetMouseName()\n");
  //logg(fp, "      wParam   hex = %6x   dec = %8d\n",  wParam,  wParam);

	string mouseName = "";

    switch (wParam)
    {
        case WM_LBUTTONDOWN: mouseName = "WM_LBUTTONDOWN"; break;  //   left button down
        case WM_MBUTTONDOWN: mouseName = "WM_MBUTTONDOWN"; break;  // middle button down
        case WM_RBUTTONDOWN: mouseName = "WM_RBUTTONDOWN"; break;  //  right button down

        case WM_LBUTTONUP  : mouseName = "WM_LBUTTONUP"  ; break;  //   left button up
        case WM_MBUTTONUP  : mouseName = "WM_MBUTTONUP"  ; break;  // middle button up
        case WM_RBUTTONUP  : mouseName = "WM_RBUTTONUP"  ; break;  //  right button up

        case WM_LBUTTONDBLCLK: mouseName = "WM_LBUTTONDBLCLK"; break;  //   left button down
        case WM_MBUTTONDBLCLK: mouseName = "WM_MBUTTONDBLCLK"; break;  // middle button down
        case WM_RBUTTONDBLCLK: mouseName = "WM_RBUTTONDBLCLK"; break;  //  right button down

      //case WM_MOUSEFIRST: mouseName = "WM_LBUTTONDOWN"; break;  // mouse first
        case WM_MOUSEMOVE : mouseName = "WM_MBUTTONDOWN"; break;  // mouse move
        case WM_MOUSEWHEEL: mouseName = "WM_RBUTTONDOWN"; break;  // mouse wheel

        default            : mouseName = "[Error]"       ; break;  // name not found
    }

  //logg(fp, "      mouseName   = %s\n",  mouseName.c_str());
  //logg(fp, "   Leave GetMouseName()\n");

    return mouseName;

} // end of method   GetMouseName()




// **************************************
//* Gets the name of a key in string form
// **************************************
string GetKeyName(UINT keyCode)
{
	//logg(fp, "   Enter GetKeyName()\n");

	char keyName[50];
	int  keyNameLength = 0;

	// Call the first Windows API function
	UINT scanCode = MapVirtualKey(keyCode, MAPVK_VK_TO_VSC);
/*
	logg(fp, "        keyCode   hex = %6x   dec = %8d\n",  keyCode,  keyCode);
	logg(fp, "       scanCode   hex = %6x   dec = %8d\n", scanCode, scanCode);
*/
    // Because MapVirtualKey strips the extended bit for some keys
    switch (keyCode)
    {
        case VK_LEFT  : case VK_UP  : // arrow keys
		case VK_RIGHT : case VK_DOWN: // arrow keys
        case VK_PRIOR : case VK_NEXT: // page up and page down
        case VK_END   : case VK_HOME:
        case VK_INSERT: case VK_DELETE:
        case VK_DIVIDE: // numpad slash
        case VK_NUMLOCK:
        {
			//logg(fp, "      keyCode is special and strips the extended bit:::\n");
            scanCode |= 0x100; // set extended bit
            break;
        }
    }

	// Call the second Windows API function
	keyNameLength = GetKeyNameText(scanCode << 16, keyName, sizeof(keyName));

  //logg(fp, "        keyName       = %s\n",  keyName);
  //logg(fp, "   Leave GetKeyName()\n");

    if (keyNameLength != 0) return keyName;
					   else return "[Error]";

} // end of method   GetKeyName()



/*
LRESULT CALLBACK DlgProc(HWND hWndDlg, UINT Msg, WPARAM wParam, LPARAM lParam)
{
	switch(Msg)
	{
	case WM_INITDIALOG:
		return TRUE;

	case WM_COMMAND:
		switch(wParam)
		{
		case IDOK:
			EndDialog(hWndDlg, 0);
			return TRUE;
		}
		break;
	}

	return FALSE;
}
*/


/* private hook functions */
LRESULT CALLBACK LLKeyboardHook(int nCode, WPARAM wParam, LPARAM lParam)
{
	//logg(fp, "Enter LLKeyboardHook()\n");
/*
	logg(fp, "   nCode     hex = %6x   dec = %8d\n", nCode , nCode);
	logg(fp, "   wParam    hex = %6x   dec = %8d\n", wParam, wParam);
	logg(fp, "   lParam    hex = %6x   dec = %8d\n", lParam, lParam);
*/
	BOOL bEatKeystroke;
	BOOL bCtrlKeyDown = GetAsyncKeyState(VK_CONTROL) >> ((sizeof(SHORT) * 8) - 1);

	KBDLLHOOKSTRUCT* p = (KBDLLHOOKSTRUCT*) lParam;
	BOOL bAltKeyDown   = p->flags & LLKHF_ALTDOWN;

	// Get the virtual key code of the pressed key,
	// and convert it to a clear text name
	// (e.g. the key code 0x72 means the key "F3").
	DWORD  keyCode = p->vkCode;
	string keyName = GetKeyName(keyCode);

  //logg(fp, "   keyCode   hex = %6x   dec = %8d\n", keyCode, keyCode);
	logg(fp, "   keyName = %-8s   ", keyName.c_str());

	if (nCode < 0 || nCode != HC_ACTION)
	{
		logg(fp, "\n");
		return CallNextHookEx(g_hHookKbdLL, nCode, wParam, lParam);
	}


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
					if ((keyCode != VK_B1) && (keyCode != VK_B2) && (keyCode != VK_B3))
					{
						b1 = FALSE;
						b2 = FALSE;				
						b3 = FALSE;
					}
					else
					{
						if ((b1 == TRUE) && (b2 == TRUE) && (b3 == TRUE) && (keyCode == VK_B3))
						{
							logg(fp, "\n\n");
							//TerminateProcess(hPiProcess->hProcess,0);
							SendMessage(hWndCaller,WM_DESTROY,NULL,NULL);
							logg(fp, "   SEB exit sequence, destroy window\n");
						  //logg(fp, "Leave LLKeyboardHook() and return -1\n\n");
							return -1;
						}

						if (keyCode == VK_B1) 
						{
							b1 = TRUE;	
							//return -1;						
						}
						if (keyCode == VK_B2)
						{
							b2 = TRUE;
							//return -1;
						}
						if (keyCode == VK_B3)
						{
							b3 = TRUE;
							//return -1;
						}
					} // end else
				} // end if (bHotKeyKill)


				/* some keys to eat */
				if (
						((alter_flags.DisableStartMenu && ((keyCode == VK_LWIN) || (keyCode == VK_RWIN)))) ||
						((alter_flags.DisableCtrlEsc   && ((keyCode == VK_ESCAPE && bCtrlKeyDown)))) ||
						((alter_flags.DisableCtrlP     && ((keyCode == VK_P      && bCtrlKeyDown)))) ||
						((alter_flags.DisableAltTab    && ((keyCode == VK_TAB    && bAltKeyDown)))) ||
						((alter_flags.DisableAltEsc    && ((keyCode == VK_ESCAPE && bAltKeyDown)))) ||
						((alter_flags.DisableAltF4     && ((keyCode == VK_F4     && bAltKeyDown)))) ||
						((alter_flags.DisableF1  && keyCode == VK_F1 )) ||
						((alter_flags.DisableF2  && keyCode == VK_F2 )) ||
						((alter_flags.DisableF3  && keyCode == VK_F3 )) ||
						((alter_flags.DisableF4  && keyCode == VK_F4 )) ||
						((alter_flags.DisableF5  && keyCode == VK_F5 )) ||
						((alter_flags.DisableF6  && keyCode == VK_F6 )) ||
						((alter_flags.DisableF7  && keyCode == VK_F7 )) ||
						((alter_flags.DisableF8  && keyCode == VK_F8 )) ||
						((alter_flags.DisableF9  && keyCode == VK_F9 )) ||
						((alter_flags.DisableF10 && keyCode == VK_F10)) ||
						((alter_flags.DisableF11 && keyCode == VK_F11)) ||
						((alter_flags.DisableF12 && keyCode == VK_F12)) ||
						((alter_flags.DisableEsc && keyCode == VK_ESCAPE))
					) 
				{
					bEatKeystroke = true;
					logg(fp, "   Suppress this key...");
				}

			} // end   if (wParam == WM_KEYDOWN || wParam == WM_SYSKEYDOWN)
		} // end   case
    } // end   switch (wParam)

	logg(fp, "\n");


	// If the "Esc" (Escape) key has been pressed and is enabled,
	// invoke a popup window asking for the quit password.
	// If the user enters the correct quit password,
	// close the window and exit the Safe Exam Browser.
	if ((keyCode == VK_ESCAPE) && (!alter_flags.DisableEsc))
	{
		logg(fp, "   \n");
		logg(fp, "   Esc pressed, calling pop window for quit password...\n\n");

		// TODO: modal popup window for entering the quit password
		string quitPasswordEntered = "";
		string quitHashcodeStored  = "";
		string quitHashcodeEntered = "";

		quitHashcodeStored = quitHashcode;

		// only temporarily for testing purposes
		quitPasswordEntered = "Davos";
		quitHashcodeEntered = "47E2361A7D358FA46394ACBCB899536D816774BE7B53AD8777BB23464DA54E";

		//hWnd = CreateWindow(szWindowClass, szTitle, WS_MAXIMIZE, 10, 10, 200, 55, NULL, NULL, hInstance, NULL);
		//SendMessage(hWndCaller,WM_DESTROY,NULL,NULL);

		//MessageBox(NULL, "Enter quit password:", "Quit SEB", MB_OKCANCEL | MB_ICONQUESTION | MB_DEFBUTTON2);
/*
		HINSTANCE    hInstance    = *hDll;
		LPCTSTR     lpTemplate    = "abba";
		HWND         hWndParent   = hWndCaller;
		DLGPROC     lpDialogFunc;
*/
		//DialogBox(NULL, MAKEINTRESOURCE(IDD_DIALOG_QUIT_PASSWORD), hWndCaller, reinterpret_cast<DLGPROC>(DlgProc));
		//DialogBox((HINSTANCE)*hDll, MAKEINTRESOURCE(IDD_DIALOG_QUIT_PASSWORD), hWndCaller, reinterpret_cast<DLGPROC>(DlgProc));

		//INT_PTR DialogBox(HINSTANCE hInstance, LPCTSTR lpTemplate, HWND hWndParent, DLGPROC lpDialogFunc);

		//DialogBox(hInstance, lpTemplate, hWndCaller, lpDialogFunc);

      //quitPasswordEntered = CreateWindow(Popup, "Enter quit password:");
	  //quitHashcodeEntered = quitPasswordEntered.ComputeHashcode();

		logg(fp, "   quitPasswordEntered = %s\n", quitPasswordEntered.c_str());
		logg(fp, "   quitHashcodeEntered = %s\n", quitHashcodeEntered.c_str());
		logg(fp, "   quitHashcodeStored  = %s\n", quitHashcodeStored .c_str());
		logg(fp, "\n");
/*
		if (quitHashcodeEntered == quitHashcodeStored)
		{
			logg(fp, "\n\n");
			//TerminateProcess(hPiProcess->hProcess,0);
			SendMessage(hWndCaller,WM_DESTROY,NULL,NULL);
			logg(fp, "   SEB quit password entered correctly, therefore destroy window\n");
			//logg(fp, "Leave LLKeyboardHook() and return -1\n\n");
			return -1;
		}
*/
	} // end if (keyCode == VK_ESCAPE)


    if (bEatKeystroke)
	{
		//logg(fp, "   bEatKeystroke = true\n");
		//logg(fp, "Leave LLKeyboardHook() and return -1\n\n");
        return -1;
	}
    else
	{
		//logg(fp, "   bEatKeystroke = false\n");
		//logg(fp, "Leave LLKeyboardHook() and return CallNextHookEx()\n\n");
        return CallNextHookEx(g_hHookKbdLL, nCode, wParam, lParam);
	}

}





LRESULT CALLBACK KeyboardHook(int nCode, WPARAM wParam, LPARAM lParam)
{
	BOOL KeyUp, KeyDown, bAltKeyDown, bCtrlKeyDown;

	logg(fp, "Enter KeyboardHook()\n");
/*
	logg(fp, "   nCode     hex = %6x   dec = %8d\n", nCode , nCode);
	logg(fp, "   wParam    hex = %6x   dec = %8d\n", wParam, wParam);
	logg(fp, "   lParam    hex = %6x   dec = %8d\n", lParam, lParam);
*/
	if (nCode < 0 || nCode != HC_ACTION) 
	{
		return CallNextHookEx(g_hHookKbd, nCode, wParam, lParam); 
	}

	if ((lParam & (1 << 31)) == 0) //Bit 31 set?
	{		
		KeyUp   = FALSE;
		KeyDown = TRUE;
	}
	else
	{
		KeyUp   = TRUE;
		KeyDown = FALSE;		
	}

	bCtrlKeyDown = GetAsyncKeyState(VK_CONTROL) >> ((sizeof(SHORT) * 8) - 1);
	bAltKeyDown  = ((lParam & (1 << 29)) != 0) ? TRUE : FALSE;


	if (KeyDown || KeyUp)
	{			
		/* hotkeys */
		/* every keyup resets hotkey pressed flags */
		if (KeyUp)
		{								
			b1 = FALSE; //of << "Erase KeyUp b1\n";
			b2 = FALSE; //of << "Erase KeyUp b2\n";
			b3 = FALSE; //of << "Erase KeyUp b3\n";
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
						logg(fp, "   SEB exit sequence, destroy window\n");
						logg(fp, "Leave KeyboardHook() and return -1\n\n");
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
					((alter_flags.DisableStartMenu && ((wParam == VK_LWIN) || (wParam == VK_RWIN)))) ||	//no effect
					((alter_flags.DisableCtrlEsc   && ((wParam == VK_ESCAPE && bCtrlKeyDown)))) ||		//no effect
					((alter_flags.DisableCtrlP     && ((wParam == VK_P      && bCtrlKeyDown)))) ||		//no effect ???
					((alter_flags.DisableAltTab    && ((wParam == VK_TAB    && bAltKeyDown)))) ||		//no effect
					((alter_flags.DisableAltEsc    && ((wParam == VK_ESCAPE && bAltKeyDown)))) ||		//no effect
					((alter_flags.DisableAltF4     && ((wParam == VK_F4     && bAltKeyDown)))) ||
					((alter_flags.DisableF1  && wParam == VK_F1 )) ||
					((alter_flags.DisableF2  && wParam == VK_F2 )) ||
					((alter_flags.DisableF3  && wParam == VK_F3 )) ||
					((alter_flags.DisableF4  && wParam == VK_F4 )) ||
					((alter_flags.DisableF5  && wParam == VK_F5 )) ||
					((alter_flags.DisableF6  && wParam == VK_F6 )) ||
					((alter_flags.DisableF7  && wParam == VK_F7 )) ||
					((alter_flags.DisableF8  && wParam == VK_F8 )) ||
					((alter_flags.DisableF9  && wParam == VK_F9 )) ||
					((alter_flags.DisableF10 && wParam == VK_F10)) ||
					((alter_flags.DisableF11 && wParam == VK_F11)) ||
					((alter_flags.DisableF12 && wParam == VK_F12))
				) 
			{
				logg(fp, "   Suppress this key...\n");
				logg(fp, "Leave KeyboardHook() and return -1\n\n");
				return -1;			
			}
		}
	}

	logg(fp, "Leave KeyboardHook() and return CallNextHookEx()\n\n");
	return CallNextHookEx(g_hHookKbd, nCode, wParam, lParam);
}



LRESULT CALLBACK LLMouseHook(int nCode, WPARAM wParam, LPARAM lParam)
{
	//logg(fp, "Enter LLMouseHook()\n");
/*
	logg(fp, "   nCode     hex = %6x   dec = %8d\n", nCode , nCode);
	logg(fp, "   wParam    hex = %6x   dec = %8d\n", wParam, wParam);
	logg(fp, "   lParam    hex = %6x   dec = %8d\n", lParam, lParam);
*/
	//BOOL bEatMouseClick;

	PMOUSEHOOKSTRUCT m = (PMOUSEHOOKSTRUCT) lParam;
	UINT mouseCode = m->wHitTestCode;

	string mouseName = GetMouseName(wParam);
	logg(fp, "   mouseName = %s\n", mouseName.c_str());


	if (nCode < 0 || nCode != HC_ACTION)
	{
		//logg(fp, "Leave LLMouseHook() and return CallNextHookEx()\n\n");
		return CallNextHookEx(g_hHookMouseLL, nCode, wParam, lParam); 
	}

    if ((wParam == WM_RBUTTONUP || wParam == WM_RBUTTONDOWN) && alter_flags.DisableRightMouse)
	{
		//logg(fp, "Leave LLMouseHook() and return -1\n\n");
        return -1;
	}

	//logg(fp, "Leave LLMouseHook() and return CallNextHookEx()\n\n");
    return CallNextHookEx(g_hHookMouseLL, nCode, wParam, lParam);
}



LRESULT CALLBACK MouseHook(int nCode, WPARAM wParam, LPARAM lParam)
{
/*
	MOUSEHOOKSTRUCT mhs;
	if (nCode >= 0)
	{
		mhs = *(MOUSEHOOKSTRUCT*)lParam; 
	}
*/

	logg(fp, "Enter MouseHook()\n");
	logg(fp, "   nCode     hex = %6x   dec = %8d\n", nCode , nCode);
	logg(fp, "   wParam    hex = %6x   dec = %8d\n", wParam, wParam);
	logg(fp, "   lParam    hex = %6x   dec = %8d\n", lParam, lParam);

	if (nCode < 0 || nCode != HC_ACTION)
	{
		logg(fp, "Leave MouseHook() and return CallNextHookEx()\n\n");
		return CallNextHookEx(g_hHookMouseLL, nCode, wParam, lParam); 
	}

	if ((wParam == WM_RBUTTONUP || wParam == WM_RBUTTONDOWN) && alter_flags.DisableRightMouse)
	{
		logg(fp, "Leave MouseHook() and return -1\n\n");
        return -1;
	}

	logg(fp, "Leave MouseHook() and return CallNextHookEx()\n\n");
	return CallNextHookEx(g_hHookMouse, nCode, wParam, lParam);
}



/* public hook functions */
EXPORT void KeyHookNT(HINSTANCE *hDLL, bool setHook)
{
	logg(fp, "Enter KeyHookNT()\n");
    if (setHook) 
	{
		logg(fp, "   setHook == true\n");
		g_hHookKbdLL = SetWindowsHookEx(WH_KEYBOARD_LL, (HOOKPROC)LLKeyboardHook, (HINSTANCE)*hDLL, 0);
    } 
	else 
	{
		logg(fp, "   setHook == false\n");
		UnhookWindowsHookEx(g_hHookKbdLL);
    }
	logg(fp, "Leave KeyHookNT()\n\n");
    return;
}



EXPORT void KeyHook9x(HINSTANCE *hDLL, bool setHook) 
{
	logg(fp, "Enter KeyHook9x()\n");
	if (setHook) 
	{
		logg(fp, "   setHook == true\n");
	  //g_hHookKbd = SetWindowsHookEx(WH_KEYBOARD, (HOOKPROC)KeyboardHook, (HINSTANCE)*hDLL, piKiox->dwThreadId);
		g_hHookKbd = SetWindowsHookEx(WH_KEYBOARD, (HOOKPROC)KeyboardHook, (HINSTANCE)*hDLL, 0);
    }	
	else 
	{
		logg(fp, "   setHook == false\n");
		UnhookWindowsHookEx(g_hHookKbd);
    }
	logg(fp, "Leave KeyHook9x()\n\n");
    return;
}



EXPORT void MouseHookNT(HINSTANCE *hDLL, bool setHook)
{
	logg(fp, "Enter MouseHookNT()\n");
    if (setHook) 
	{		
		logg(fp, "   setHook == true\n");
		g_hHookMouseLL = SetWindowsHookEx(WH_MOUSE_LL, (HOOKPROC)LLMouseHook, (HINSTANCE)*hDLL, 0);
    } 
	else 
	{
		logg(fp, "   setHook == false\n");
		UnhookWindowsHookEx(g_hHookMouseLL);
    }
	logg(fp, "Leave MouseHookNT()\n\n");
    return;
}



EXPORT void MouseHook9x(HINSTANCE *hDLL, bool setHook) 
{
	logg(fp, "Enter MouseHook9x()\n");
	if (setHook) 
	{
		logg(fp, "   setHook == true\n");
	  //g_hHookKbd   = SetWindowsHookEx(WH_KEYBOARD, (HOOKPROC)KeyboardHook, (HINSTANCE)*hDLL, piKiox->dwThreadId);
		g_hHookMouse = SetWindowsHookEx(WH_MOUSE   , (HOOKPROC)MouseHook   , (HINSTANCE)*hDLL, 0);
    } 
	else 
	{
		logg(fp, "   setHook == false\n");
		UnhookWindowsHookEx(g_hHookMouse);
    }
	logg(fp, "Leave MouseHook9x()\n\n");
    return; 
}





BOOL ReadMsgHookIni()
{
	char   cCurrDir[MAX_PATH];
	string sCurrDir = "";
	string strLine  = "";
	string strKey   = "";
	string strValue = "";
	size_t strFound = -1;
	string sHotKey  = "";
	string sQuitHashcode = "";

	logg(fp, "Enter ReadMsgHookIni()\n\n");

	try
	{
		if (!isValidOperatingSystem())
		{
			logg(fp, "Leave ReadMsgHookIni() and return FALSE()\n\n");
			return FALSE;
		}

		GetModuleFileName(*hMod, cCurrDir, sizeof(cCurrDir));
		sCurrDir = (string)cCurrDir;
		sCurrDir.replace(((size_t)sCurrDir.length()-3), 3, "ini");

		// The SebStarter.ini and MsgHook.ini configuration files have moved:
		// Previously:
		// SebStarter.ini was lying in the /SebStarter subdirectory,
		//    MsgHook.ini was lying in the /MsgHook    subdirectory.
		// Both had to be copied to the /Debug and /Release directories.
		// before starting SebStarter.exe .
		// Now:
		// SebStarter.ini and MsgHook.ini are both in the Seb main project directory,
		// together with the Seb.sln project file,
		// the /Debug subdirectory and the /Release subdirectory.
		// Advantage: the .ini files are lying together, being accessible
		// for both the /Debug and the /Release version without copying
		// being necessary anymore.

		ifstream inf;

		logg(fp, "Try to open ini file %s\n", iniFileMsgHook);
		inf.open(iniFileMsgHook);

		//logg(fp, "   Try to open ini file %s\n", sCurrDir.c_str());
		//inf.open(sCurrDir.c_str());

		if (!inf.is_open()) 
		{
			logg(fp, "Try to open ini file %s\n", sCurrDir.c_str());
			inf.open(sCurrDir.c_str());

			if (!inf.is_open()) 
			{
				OutputErrorMessage(languageIndex, IND_MsgHookIniError, IND_MessageKindError);
				//MessageBox(NULL, messageText[languageIndex][IND_MsgHookIniError], "Error", 16);
				//logg(fp, "Error: %s\n", messageText[languageIndex][IND_MsgHookIniError]);
				logg(fp, "Leave ReadMsgHookIni() and return FALSE\n\n");
				return FALSE;
			}
		}

		logg(fp, "\n");
		logg(fp, "key = value\n");
		logg(fp, "-----------\n");

		while (!getline(inf, strLine).eof())
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

		inf.close();
		logg(fp, "-----------\n\n");


		// Decide whether to write data into the logfile
		if (getBool("WriteLogFileMsgHookLog"))
		{
			logFileDesiredMsgHook = true;
			logg(fp, "Logfile desired, therefore keeping logfile\n\n");
		}
		else
		{
			logFileDesiredMsgHook = false;
			logg(fp, "No logfile desired, therefore closing and removing logfile\n\n");
			if (fp != NULL)
			{
				fclose(fp);
				remove(logFileMsgHook);
			}
		}


		// Get the encrypted quit password (hashcode) for SEB
		sQuitHashcode = mpParam["QuitHashcode"];

		// Store the encrypted quit password (hashcode) for SEB
		strcpy(quitHashcode, sQuitHashcode.c_str());

		logg(fp, "quitHashcode = %s\n",  quitHashcode);
		logg(fp, "\n");


		//setting bits of alter_flags_structs
		alter_flags.DisableCtrlEsc = !getBool("EnableCtrlEsc");
		alter_flags.DisableCtrlP   = !getBool("EnableCtrlP");
		alter_flags.DisableAltTab  = !getBool("EnableAltTab");
		alter_flags.DisableAltEsc  = !getBool("EnableAltEsc");
		alter_flags.DisableAltF4   = !getBool("EnableAltF4");
		alter_flags.DisableStartMenu  = !getBool("EnableStartMenu");
		alter_flags.DisableRightMouse = !getBool("EnableRightMouse");
		alter_flags.DisableLeftMouse  = !getBool("EnableLeftMouse");
		alter_flags.DisableF1  = !getBool("EnableF1");
		alter_flags.DisableF2  = !getBool("EnableF2");
		alter_flags.DisableF3  = !getBool("EnableF3");
		alter_flags.DisableF4  = !getBool("EnableF4");
		alter_flags.DisableF5  = !getBool("EnableF5");
		alter_flags.DisableF6  = !getBool("EnableF6");
		alter_flags.DisableF7  = !getBool("EnableF7");
		alter_flags.DisableF8  = !getBool("EnableF8");
		alter_flags.DisableF9  = !getBool("EnableF9");
		alter_flags.DisableF10 = !getBool("EnableF10");
		alter_flags.DisableF11 = !getBool("EnableF11");
		alter_flags.DisableF12 = !getBool("EnableF12");
		alter_flags.DisableEsc = !getBool("EnableEsc");

		// Kill Caller with HotKey
		sHotKey = mpParam["KillCallerHotkey"];

		//Hotkey from Registry (1. priority) and configuration file MsgHook.ini (2. priority). If nothing is found -> default is F3 + F11 + F6.
		VK_B1 = (GetButtonForHotKeyFromRegistry(VAL_Button1) ? GetButtonForHotKeyFromRegistry(VAL_Button1) : (DWORD)getInt("B1") ? (DWORD)getInt("B1") : VK_F3);
		VK_B2 = (GetButtonForHotKeyFromRegistry(VAL_Button2) ? GetButtonForHotKeyFromRegistry(VAL_Button2) : (DWORD)getInt("B2") ? (DWORD)getInt("B2") : VK_F11);
		VK_B3 = (GetButtonForHotKeyFromRegistry(VAL_Button3) ? GetButtonForHotKeyFromRegistry(VAL_Button3) : (DWORD)getInt("B3") ? (DWORD)getInt("B3") : VK_F6);

		if (hWndCaller == NULL) 
		{
			hWndCaller = FindWindow(NULL, sHotKey.c_str());
		}

		if (hWndCaller != NULL)
		{
			if (sHotKey != "") bHotKeyKill = TRUE;
		}
		else
		{
			//OutputErrorMessage(languageIndex, IND_NoCallerWindowFound, IND_MessageKindError);
			MessageBox(NULL, "No caller window found!", "Error", 16);
		}
	} // end try

	catch (char* str)
	{
		MessageBox(NULL, str, "Error", MB_ICONERROR);
		logg(fp, "Error: %s\n", str);
		logg(fp, "Leave ReadMsgHookIni() and return FALSE\n\n");
		return FALSE;
	}

	logg(fp, "Leave ReadMsgHookIni()\n\n");
	return TRUE;
}





int GetButtonForHotKeyFromRegistry(LPCTSTR regButton) 
{
	int retValue = 0;
	logg(fp, "Enter GetButtonForHotKeyFromRegistry()\n");

	try
	{
		HKEY  hKey;
		DWORD b1;
		LONG  retStatus;
		DWORD dwType = REG_DWORD;
		DWORD dwSize = sizeof(DWORD);
		retStatus    = RegOpenKeyEx(HKLM, KEY_PoliciesSeb, 0, KEY_READ, &hKey);

		if (retStatus == ERROR_SUCCESS)
		{
			retStatus = RegQueryValueEx(hKey, regButton, NULL, &dwType,(LPBYTE)&b1, &dwSize);
			if (retStatus == ERROR_SUCCESS)
			{
				retValue = b1;
			}
			RegCloseKey(hKey);
		}
		logg(fp, "Leave GetButtonForHotKeyFromRegistry()\n\n");
		return retValue;
	}
	catch (char* str)
	{		
		MessageBox(NULL, str, "Error", MB_ICONERROR);
		logg(fp, "Leave GetButtonForHotKeyFromRegistry()\n\n");
		return retValue;
	}
}





BOOL APIENTRY DllMain(HMODULE hModule,
                      DWORD   ul_reason_for_call,
                      LPVOID  lpReserved)
{
	// By default, a logfile should be written
	//logFileDesiredMsgHook = true;

	// Determine the location of the .ini files
	SetIniFileDirectoryAndName();
	//MessageBox(NULL, iniFileMsgHook, "iniFileMsgHook", MB_ICONERROR);


	// Open or create a logfile for message hooks
	if (fp == NULL)
	{
		// Determine the location of the .log files
		SetLogFileDirectoryAndName();
		// MessageBox(NULL, logFileMsgHook, "logFileMsgHook", MB_ICONERROR);
		// Open the logfile for debug output
		fp = fopen(logFileMsgHook, "w");
	}

	if (fp == NULL)
	{
		//MessageBox(NULL, logFileMsgHook, "DllMain(): Could not open logfile MsgHook.log", MB_ICONERROR);
	}

	logg(fp, "\n");
	logg(fp, "Enter DllMain()\n");


	// Perform actions based on the reason for calling.
    switch (ul_reason_for_call) 
    { 
        case DLL_PROCESS_ATTACH:
			hMod = &hModule;
			hDll = (HINSTANCE*)&hModule;
			if (!ReadMsgHookIni()) 
			{
				OutputErrorMessage(languageIndex, IND_InitialiseError, IND_MessageKindError);
				//MessageBox(NULL, INITIALIZE_ERROR,"Error", 16);
			}
            break;

        case DLL_THREAD_ATTACH:
			// Do thread-specific initialization.
			//MessageBox(NULL,"DLL_THREAD_ATTACH","Error",16);
            break;

        case DLL_THREAD_DETACH:
			// Do thread-specific cleanup.
			//MessageBox(NULL,"DLL_THREAD_DETACH","Error",16);
            break;

        case DLL_PROCESS_DETACH:
			// Perform any necessary cleanup.
			//MessageBox(NULL,"DLL_PROCESS_DETACH","Error",16);
			//of.close();
            break;
    }

	logg(fp, "Leave DllMain()\n\n");
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
	catch (char* str)
	{
		MessageBox(NULL, str, "Error", MB_ICONERROR);
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
		MessageBox(NULL, str, "Error", MB_ICONERROR);
		return FALSE;
	}
}



BOOL isValidOperatingSystem()
{
	int operatingSystem = sysVersionInfo.GetVersion();

	logg(fp, "Enter isValidOperatingSystem()\n");
	logg(fp, "   sysVersionInfo.GetVersion() == %d\n", operatingSystem);

	switch (operatingSystem)
	{			
		case WIN_NT_351 :
		case WIN_NT_40  :
		case WIN_2000  :
		case WIN_XP    :
		case WIN_VISTA :
		case WIN_95 :
		case WIN_98 :
		case WIN_ME :
			logg(fp, "   operatingSystem             == %d\n", operatingSystem);
			logg(fp, "Leave isValidOperatingSystem() and return TRUE\n\n");
			return TRUE;
			break;
		default :
			OutputErrorMessage(languageIndex, IND_NoOsSupport, IND_MessageKindError);
			//MessageBox(NULL,NO_OS_SUPPORT,"Error",16);
			logg(fp, "Leave isValidOperatingSystem() and return FALSE\n\n");
			return FALSE;			
	}
}



#ifdef _MANAGED
#pragma managed(pop)
#endif
