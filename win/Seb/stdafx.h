// stdafx.h : include file for standard system include files,
// or project specific include files that are used frequently, but
// are changed infrequently
//

#pragma once
#ifndef WINVER				// Allow use of features specific to Windows 95 and Windows NT 4 or later.
#define WINVER 0x0400		// Change this to the appropriate value to target Windows 98 and Windows 2000 or later.
#endif

#ifndef _WIN32_WINDOWS		  // Allow use of features specific to Windows 98 or later.
#define _WIN32_WINDOWS 0x0410 // Change this to the appropriate value to target Windows Me or later.
#endif

#ifndef _WIN32_WINNT			// Allow use of features specific to Windows NT 4 or later.
#define _WIN32_WINNT 0x0400		// Change this to the appropriate value to target Windows 98 and Windows 2000 or later.
#endif

#define WIN32_LEAN_AND_MEAN		// Exclude rarely-used stuff from Windows headers
// Windows Header Files:
#include <windows.h>
// C RunTime Header Files
#include <stdlib.h>
#include <malloc.h>
#include <memory.h>
#include <tchar.h>

// TODO: reference additional headers your program requires here
#include<winsock2.h>
#include<process.h>
#include<signal.h>
#include<stdio.h>
#include<iostream>
#include<iomanip>
#include<fstream>
#include<string>
#include<map>
#include <tlhelp32.h>
#include <algorithm>
#include <vector>
#include <winwlx.h> 

using namespace std;

