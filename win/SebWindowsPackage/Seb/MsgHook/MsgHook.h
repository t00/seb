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

#define strict
#ifdef __cplusplus
#define EXPORT extern "C" __declspec (dllexport)
#else
#define EXPORT __declspec (dllexport)
#endif


// Name and location of MsgHook configuration file
#define MSGHOOK_INI "../MsgHook.ini"

//For the registry fields
#define HKCU HKEY_CURRENT_USER
#define HKLM HKEY_LOCAL_MACHINE

#include "AlterFlags.h"
#include "../SystemVersionInfo.h"
/*
registry WRITE,LOCMACH,Software\Microsoft\Windows\CurrentVersion\Policies\System,HideFastUserSwitching,1,DWORD
registry WRITE,CURUSER,Software\Microsoft\Windows\CurrentVersion\Policies\System,DisableLockWorkstation,1,DWORD
registry WRITE,CURUSER,Software\Microsoft\Windows\CurrentVersion\Policies\System,DisableTaskMgr,1,DWORD
registry WRITE,CURUSER,Software\Microsoft\Windows\CurrentVersion\Policies\System,DisableChangePassword,1,DWORD
registry WRITE,CURUSER,Software\Microsoft\Windows\CurrentVersion\Policies\Explorer,NoClose,1,DWORD
registry WRITE,CURUSER,Software\Microsoft\Windows\CurrentVersion\Policies\Explorer,NoLogoff,1,DWORD
*/