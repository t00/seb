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
	BYTE	DisableCtrlEsc : 1,
			DisableCtrlP   : 1,
			DisableAltTab   : 1,
			DisableAltEsc   : 1,
			DisableAltSpace : 1,
			DisableAltF4    : 1,
			DisableStartMenu  : 1,
			DisableRightMouse : 1,
			DisableLeftMouse  : 1,
			DisableF1  : 1,
			DisableF2  : 1,
			DisableF3  : 1,
			DisableF4  : 1,
			DisableF5  : 1,
			DisableF6  : 1,
			DisableF7  : 1,
			DisableF8  : 1,
			DisableF9  : 1,
			DisableF10 : 1,
			DisableF11 : 1,
			DisableF12 : 1,
			DisableEsc : 1;
} ALTER_FLAGS;


// Strings for registry keys
LPCTSTR KEY_PoliciesSystem   = "Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\System";
LPCTSTR KEY_PoliciesExplorer = "Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\Explorer";
LPCTSTR KEY_PoliciesSeb      = "Software\\Policies\\SEB";
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

