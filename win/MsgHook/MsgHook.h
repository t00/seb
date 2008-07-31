#define strict
#ifdef __cplusplus
#define EXPORT extern "C" __declspec (dllexport)
#else
#define EXPORT __declspec (dllexport)
#endif
//For Initialization
#define	NO_OS_SUPPORT "The OS is not supported!"
#define	NO_INI_ERROR "No *.ini File found!"
#define	INITIALIZE_ERROR "Initialization of message hook library failed!"
#define	TERMINATION_ERROR "Termination of message hook library failed!"

//For the registry fields
#include "AlterFlags.h"
#include "SystemVersionInfo.h"
/*
registry WRITE,CURUSER,Software\Microsoft\Windows\CurrentVersion\Policies\System,DisableLockWorkstation,1,DWORD
registry WRITE,CURUSER,Software\Microsoft\Windows\CurrentVersion\Policies\System,DisableTaskMgr,1,DWORD
registry WRITE,CURUSER,Software\Microsoft\Windows\CurrentVersion\Policies\System,DisableChangePassword,1,DWORD
registry WRITE,CURUSER,Software\Microsoft\Windows\CurrentVersion\Policies\Explorer,NoClose,1,DWORD
registry WRITE,CURUSER,Software\Microsoft\Windows\CurrentVersion\Policies\Explorer,NoLogoff,1,DWORD
*/