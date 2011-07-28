

@echo off

echo.
echo.
echo Safe Exam Browser installation
echo ------------------------------
echo.
echo For a full installation, you must execute the
echo.
echo SebWindowsInstall.bat
echo.
echo script when logged in as an administrator, not as a standard user!
echo.
echo Reason:
echo.
echo This script first runs the MSI installer with the
echo installation file SebWindowsInstall.msi.
echo.
echo Afterwards, the script copies the configuration files
echo MsgHook.ini and SebStarter.ini to the Program Data directory
echo (overwriting the default MsgHook.ini and SebStarter.ini).
echo.
echo [And if desired, the script copies also the batch file
echo SebStarter.bat to the Program Files directory,
echo where SEB has been installed.]
echo.
echo If you execute this script as a standard user, the MSI installer
echo will prompt you for an administrator password.
echo Even after successful installation in administrator mode,
echo the script will then fall back to standard user rights.
echo.
echo So the SebStarter.ini and MsgHook.ini files will _not_ be copied to
echo the Program Data directory due to lack of administrator rights!
echo In this case, you will have to _manually_ copy these .ini files
echo as an administrator afterwards.
echo.
echo Solution:
echo.
echo To avoid this, please execute the SebWindowsInstall.bat
echo script by right-clicking on its name in the Explorer,
echo and then choosing "Run as administrator".
echo You will then be prompted for the administrator password,
echo and the whole script will be executed with administrator rights
echo (running the MSI installer and copying the configured files)
echo.



echo.
echo.
echo Display the ProgramData and ProgramFiles directories
echo ----------------------------------------------------

echo ProgramData       = %ProgramData%
echo ProgramFiles      = %ProgramFiles%
echo ProgramFiles(x86) = %ProgramFiles(x86)%



echo.
echo.
echo Assign the directory containing this batch file
echo -----------------------------------------------

set CurrentDir=%CD%
set BatchDir=%~dp0
set BatchFile=%0

echo CurrentDir = %CurrentDir%
echo BatchDir   = %BatchDir%
echo BatchFile  = %BatchFile%



echo.
echo.
echo Assign the SEB installation and configuration directories
echo ---------------------------------------------------------

set Manufacturer=ETH Zuerich
set Product=SEB Windows
set Version=1.7
set Component=SebWindowsClient
set Build=Release

set SebConfigDir=%ProgramData%\%Manufacturer%\%Product% %Version%
set SebInstallDir=%ProgramFiles%\%Manufacturer%\%Product% %Version%\%Component%\%Build%
set SebInstallDir(x86)=%ProgramFiles(x86)%\%Manufacturer%\%Product% %Version%\%Component%\%Build%

set InstallMsi=SebWindowsInstall.msi
set SebStarterBat=SebStarter.bat
set SebStarterIni=SebStarter.ini
set MsgHookIni=MsgHook.ini

set InstallMsiFile=%BatchDir%%InstallMsi%
set SebStarterBatFile=%BatchDir%%SebStarterBat%
set SebStarterIniFile=%BatchDir%%SebStarterIni%
set MsgHookIniFile=%BatchDir%%MsgHookIni%


echo Manufacturer = %Manufacturer%
echo Product      = %Product%
echo Version      = %Version%
echo Component    = %Component%
echo Build        = %Build%
echo.
echo SebConfigDir       = %SebConfigDir%
echo SebInstallDir      = %SebInstallDir%
echo SebInstallDir(x86) = %SebInstallDir(x86)%
echo.
echo InstallMsi         = %InstallMsi%
echo SebStarterBat      = %SebStarterBat%
echo SebStarterIni      = %SebStarterIni%
echo MsgHookIni         = %MsgHookIni%
echo.
echo InstallMsiFile     = %InstallMsiFile%
echo SebStarterBatFile  = %SebStarterBatFile%
echo SebStarterIniFile  = %SebStarterIniFile%
echo MsgHookIniFile     = %MsgHookIniFile%



echo.
echo.
echo Run the Microsoft Installer with the .msi file
echo ----------------------------------------------

@echo on
msiexec.exe /i "%InstallMsiFile%"
@echo off



echo.
echo.
echo Copy the configured .ini files to the SEB configuration directory
echo Copy the configured .bat file  to the SEB  installation directory
echo -----------------------------------------------------------------

@echo on
copy    "%MsgHookIniFile%" "%SebConfigDir%"
copy "%SebStarterIniFile%" "%SebConfigDir%"
copy "%SebStarterBatFile%" "%SebInstallDir%"
copy "%SebStarterBatFile%" "%SebInstallDir(x86)%"
@echo off



echo.
pause
@echo on


