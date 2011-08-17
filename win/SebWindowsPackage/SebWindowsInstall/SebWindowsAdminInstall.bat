

@echo off

echo.
echo.
echo Safe Exam Browser administrative installation
echo ---------------------------------------------
echo.
echo The Administrative Installation, contains two phases:
echo.
echo 1st phase: the teacher executes
echo.
echo SebWindowsAdminInstall.bat
echo.
echo script when logged in as an administrator, not as a standard user!
echo.
echo Reason:
echo.
echo This script first runs the MSI installer with the
echo installation file SebWindowsInstall.msi.
echo.
echo Afterwards, the script copies the configuration files
echo MsgHook.ini and SebStarter.ini to the "Common Application Folder".
echo.
echo [And if desired, the script copies also the batch file
echo SebStarter.bat to the Program Files directory,
echo where SEB has been installed.]
echo.
echo Solution:
echo.
echo To avoid this, please execute the SebWindowsAdminInstall.bat
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
echo CommonAppDataFolder=%CommonAppDataFolder%



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

set CommonAppDataFolder=Common Application Data Folder
set SebAdminImage=SebWindowsAdminImage



set SebConfigDir=%ProgramData%\%Manufacturer%\%Product% %Version%

set SebInstallDir=%ProgramFiles%\%Manufacturer%\%Product% %Version%
set SebClientDir=%ProgramFiles%\%Manufacturer%\%Product% %Version%\%Component%
set SebReleaseDir=%ProgramFiles%\%Manufacturer%\%Product% %Version%\%Component%\%Build%

set SebInstallDir(x86)=%ProgramFiles(x86)%\%Manufacturer%\%Product% %Version%
set SebClientDir(x86)=%ProgramFiles(x86)%\%Manufacturer%\%Product% %Version%\%Component%
set SebReleaseDir(x86)=%ProgramFiles(x86)%\%Manufacturer%\%Product% %Version%\%Component%\%Build%

set SebAdminImageDir=%BatchDir%%SebAdminImage%
set SebAdminConfigDir=%BatchDir%%SebAdminImage%\%CommonAppDataFolder%



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
echo.
echo SebInstallDir      = %SebInstallDir%
echo SebClientDir       = %SebClientDir%
echo SebReleaseDir      = %SebReleaseDir%
echo.
echo SebInstallDir(x86) = %SebInstallDir(x86)%
echo SebClientDir(x86)  = %SebClientDir(x86)%
echo SebReleaseDir(x86) = %SebReleaseDir(x86)%
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
echo CommonAppDataFolder = %CommonAppDataFolder%
echo SebAdminImage       = %SebAdminImage%
echo SebAdminImageDir    = %SebAdminImageDir%
echo SebAdminConfigDir   = %SebAdminConfigDir%



@REM echo Expand the PATH environment variable by the batch directory
@REM echo -----------------------------------------------------------

path %path%;%BatchDir%



echo.
echo.
echo Run the Administrative Install
echo ------------------------------

@echo on
msiexec /a "%InstallMsiFile%" TARGETDIR=%SebAdminImageDir%
@echo off



echo.
echo.
echo Copy the configured .ini files to the SEB configuration directory
echo Copy the configured .bat file  to the SEB  installation directory
echo -----------------------------------------------------------------

@echo on

copy    "%MsgHookIniFile%" "%SebAdminConfigDir%"
copy "%SebStarterIniFile%" "%SebAdminConfigDir%"

@REM copy "%SebStarterBatFile%" "%SebReleaseDir%"
@REM copy "%SebStarterBatFile%" "%SebReleaseDir(x86)%"

@echo off



echo.
pause
@echo on


