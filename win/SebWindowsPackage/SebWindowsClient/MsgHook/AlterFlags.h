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
	BYTE	Disable_Ctrl_Esc : 1,
			Disable_Ctrl_P  : 1,
			Disable_Alt_Tab : 1,
			Disable_Alt_Esc : 1,
			Disable_Alt_F4  : 1,
			Disable_Start_Menu  : 1,
			Disable_Right_Mouse : 1,
			Disable_Left_Mouse  : 1,
			Disable_F1  : 1,
			Disable_F2  : 1,
			Disable_F3  : 1,
			Disable_F4  : 1,
			Disable_F5  : 1,
			Disable_F6  : 1,
			Disable_F7  : 1,
			Disable_F8  : 1,
			Disable_F9  : 1,
			Disable_F10 : 1,
			Disable_F11 : 1,
			Disable_F12 : 1,
			Disable_Esc : 1;
} ALTER_FLAGS;


// Strings for registry keys
LPCTSTR KEY_PoliciesSystem   = "Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\System";
LPCTSTR KEY_PoliciesExplorer = "Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\Explorer";
LPCTSTR KEY_PoliciesSEB      = "Software\\Policies\\SEB";
LPCTSTR KEY_VmWareClient     = "Software\\VMware, Inc.\\VMware VDM\\Client";

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

