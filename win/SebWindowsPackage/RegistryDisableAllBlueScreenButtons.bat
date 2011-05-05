
REG DELETE "HKLM\Software\Microsoft\Windows\CurrentVersion\Policies\System"   /v HideFastUserSwitching  /f
REG DELETE "HKCU\Software\Microsoft\Windows\CurrentVersion\Policies\System"   /v DisableLockWorkstation /f
REG DELETE "HKCU\Software\Microsoft\Windows\CurrentVersion\Policies\System"   /v DisableChangePassword  /f
REG DELETE "HKCU\Software\Microsoft\Windows\CurrentVersion\Policies\System"   /v DisableTaskMgr         /f
REG DELETE "HKCU\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer" /v NoLogoff               /f
REG DELETE "HKCU\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer" /v NoClose                /f
REG DELETE "HKCU\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer" /v EnableShade            /f
REG DELETE "HKLM\Software\Microsoft\Windows NT\CurrentVersion\Image File Execution Options\Utilman.exe" /v Debugger /f

pause

REG ADD    "HKLM\Software\Microsoft\Windows\CurrentVersion\Policies\System"   /v HideFastUserSwitching  /t REG_DWORD /d 0x00000001 /f
REG ADD    "HKCU\Software\Microsoft\Windows\CurrentVersion\Policies\System"   /v DisableLockWorkstation /t REG_DWORD /d 0x00000001 /f
REG ADD    "HKCU\Software\Microsoft\Windows\CurrentVersion\Policies\System"   /v DisableChangePassword  /t REG_DWORD /d 0x00000001 /f
REG ADD    "HKCU\Software\Microsoft\Windows\CurrentVersion\Policies\System"   /v DisableTaskMgr         /t REG_DWORD /d 0x00000001 /f
REG ADD    "HKCU\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer" /v NoLogoff               /t REG_DWORD /d 0x00000001 /f
REG ADD    "HKCU\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer" /v NoClose                /t REG_DWORD /d 0x00000001 /f
REG ADD    "HKCU\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer" /v EnableShade            /t REG_DWORD /d 0x00000000 /f
REG ADD    "HKLM\Software\Microsoft\Windows NT\CurrentVersion\Image File Execution Options\Utilman.exe" /v Debugger /t REG_SZ /d "sebDummy.exe" /f

pause

