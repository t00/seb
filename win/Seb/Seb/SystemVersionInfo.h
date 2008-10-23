/*
* SystemVersionInfo.h : header file
*
* SystemVersionInfo - Windows OS Detection Class
*
* Written by netseeker <netseeker@umzingelt.de>
*/

#ifndef SYSTEMVERSIONINFO_H
#define SYSTEMVERSIONINFO_H

#define OS_UNKNOWN    800
#define WIN_95    950
#define WIN_98    980
#define WIN_ME    999
#define WIN_NT_351    1351
#define WIN_NT_40    1400
#define WIN_2000    2000
#define WIN_XP    2010
#define WIN_VISTA    2050

class SystemVersionInfo
{
    public:
        // class constructor
        SystemVersionInfo(){};
        // class destructor
        ~SystemVersionInfo(){};
        int GetVersion();
};

#endif // SYSTEMVERSIONINFO_H
//end header file


/*
* SystemVersionInfo.cpp : implementation file
*
* SystemVersionInfo - Windows OS Detection Class
*
* Written by netseeker <netseeker@umzingelt.de>
*
* History: 17 Aug 2002 - Creation
*/

//if you are using MS VC with precompiled headers include the next line
//#include "stdafx.h"
#include <windows.h>

int SystemVersionInfo::GetVersion()
{
    OSVERSIONINFO ver;
    ver.dwOSVersionInfoSize = sizeof(OSVERSIONINFO);
    int getVersion = OS_UNKNOWN;

    if(GetVersionEx((LPOSVERSIONINFO) &ver))
    {
        switch (ver.dwPlatformId)
        {
            case 1:
                switch(ver.dwMinorVersion)
                {
                    case 0:
                        getVersion = WIN_95;
                        break;
                    case 10:
                        getVersion = WIN_98;
                        break;
                    case 90:
                        getVersion = WIN_ME;
                        break;
                    default:
                        getVersion = OS_UNKNOWN;
                        break;
                }
                break;
            case 2:
                switch(ver.dwMajorVersion)
                {
                    case 3:
                        getVersion = WIN_NT_351;
                        break;
                    case 4:
                        getVersion = WIN_NT_40;
                        break;
                    case 5:
                        if (ver.dwMinorVersion == 0)
                            getVersion = WIN_2000;
                        else
                            getVersion = WIN_XP;
                        break;
					case 6:
                        getVersion = WIN_VISTA;
                        break;
                    default:
                        getVersion = OS_UNKNOWN;
                        break;
                }
                break;
            default:
                getVersion = OS_UNKNOWN;
                break;
        }
    }

    return getVersion;
}
//end implementation file
