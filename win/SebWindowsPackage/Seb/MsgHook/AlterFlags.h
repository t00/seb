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
 * The Original Code defines a struct for buttons and several constants. 
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

typedef struct
{
	BYTE	DISABLE_CTRL_ESC : 1,
			DISABLE_CTRL_P  : 1,
			DISABLE_ALT_TAB : 1,
			DISABLE_ALT_ESC : 1,
			DISABLE_ALT_F4  : 1,
			DISABLE_START_MENU  : 1,
			DISABLE_RIGHT_MOUSE : 1,
			DISABLE_LEFT_MOUSE  : 1,
			DISABLE_F1  : 1,
			DISABLE_F2  : 1,
			DISABLE_F3  : 1,
			DISABLE_F4  : 1,
			DISABLE_F5  : 1,
			DISABLE_F6  : 1,
			DISABLE_F7  : 1,
			DISABLE_F8  : 1,
			DISABLE_F9  : 1,
			DISABLE_F10 : 1,
			DISABLE_F11 : 1,
			DISABLE_F12 : 1,
			DISABLE_ESCAPE : 1;
} ALTER_FLAGS;


// Strings for registry keys
LPCTSTR KEY_PoliciesSystem   = "Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\System";
LPCTSTR KEY_PoliciesExplorer = "Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\Explorer";
LPCTSTR KEY_PoliciesSEB      = "Software\\Policies\\SEB";
LPCTSTR KEY_VMwareClient     = "Software\\VMware, Inc.\\VMware VDM\\Client";

// Strings for registry values
LPCTSTR VAL_HideFastUserSwitching  = "HideFastUserSwitching";
LPCTSTR VAL_DisableLockWorkstation = "DisableLockWorkstation";
LPCTSTR VAL_DisableChangePassword  = "DisableChangePassword";
LPCTSTR VAL_DisableTaskMgr         = "DisableTaskMgr";
LPCTSTR VAL_NoLogoff               = "NoLogoff";
LPCTSTR VAL_NoClose                = "NoClose";
LPCTSTR VAL_EnableShade            = "EnableShade";

// Strings for SEB exit sequence
LPCTSTR VAL_Button1 = "B1";
LPCTSTR VAL_Button2 = "B2";
LPCTSTR VAL_Button3 = "B3";

#define VK_P              0x50

