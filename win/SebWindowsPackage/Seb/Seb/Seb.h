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



// Socket structures for IPC (interprocess communication)
// between SEB (client) and SEB Windows Service (server)
bool    forceWindowsService = false;
#define BUFLEN 512


// Other constants
#define IDM_START   9001
#define IDM_OFFSET 37265

#define MAX_LOADSTRING 100
#define SEB_DESK       "SEB Desktop"
#define WINKEYOX_DESK  "WinKeyox Desktop"



// HKEY_CURRENT_USER contains the Windows Registry keys
//
// DisableLockWorkstation ("Lock this computer")
// DisableChangePassword  ("Change a password")
// DisableTaskMgr         ("Start Task Manager")
// NoLogoff               ("Log off")
// NoClose                ("No shutdown symbol in lower right desktop corner")
// EnableShade            ("Enable Shade on VMware virtual desktop")
//
// HKEY_LOCAL_MACHINE contains the Windows Registry key
//
// HideFastUserSwitching  ("Switch User")
// UtilmanExe/Debugger    ("Program started by Ease-of-Access icon in lower left desktop corner")

#define HKCU HKEY_CURRENT_USER
#define HKLM HKEY_LOCAL_MACHINE

#define SHOW_WIN_HIDE      0
#define SHOW_WIN_NORMAL    1
#define SHOW_WIN_MINIMIZED 2
#define GINA_PATH TEXT("MSGINA.DLL")


// Strings for registry keys
LPCTSTR KEY_PoliciesSystem   = "Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\System";
LPCTSTR KEY_PoliciesExplorer = "Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\Explorer";
LPCTSTR KEY_PoliciesSEB      = "Software\\Policies\\SEB";
LPCTSTR KEY_VMwareClient     = "Software\\VMware, Inc.\\VMware VDM\\Client";
LPCTSTR KEY_UtilmanExe       = "Software\\Microsoft\\Windows NT\\CurrentVersion\\Image File Execution Options\\Utilman.exe";

// Strings for registry values
LPCTSTR VAL_HideFastUserSwitching  = "HideFastUserSwitching";
LPCTSTR VAL_DisableLockWorkstation = "DisableLockWorkstation";
LPCTSTR VAL_DisableChangePassword  = "DisableChangePassword";
LPCTSTR VAL_DisableTaskMgr         = "DisableTaskMgr";
LPCTSTR VAL_NoLogoff               = "NoLogoff";
LPCTSTR VAL_NoClose                = "NoClose";
LPCTSTR VAL_EnableShade            = "EnableShade";
LPCTSTR VAL_EnableEaseOfAccess     = "Debugger";

// Aligned strings for printing out registry values
LPCTSTR MSG_HideFastUserSwitching  = "HideFastUserSwitching ";
LPCTSTR MSG_DisableLockWorkstation = "DisableLockWorkstation";
LPCTSTR MSG_DisableChangePassword  = "DisableChangePassword ";
LPCTSTR MSG_DisableTaskMgr         = "DisableTaskMgr        ";
LPCTSTR MSG_NoLogoff               = "NoLogoff              ";
LPCTSTR MSG_NoClose                = "NoClose               ";
LPCTSTR MSG_EnableShade            = "EnableShade           ";
LPCTSTR MSG_EnableEaseOfAccess     = "Debugger              ";


// Only for trunk version necessary (XULrunner)

LPCTSTR VAL_PERMITTED_APPS    = "PermittedApps";
LPCTSTR VAL_ShowSEBAppChooser = "ShowSEBAppChooser";