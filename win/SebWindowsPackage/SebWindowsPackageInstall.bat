

@echo off

echo.
echo.
echo Safe Exam Browser installation
echo ------------------------------

echo For a full installation, you must execute this batch script
echo when logged in as an administrator! Otherwise, the MSI installer
echo will ask you for an administrator password, and after installation
echo the SebStarter.ini and MsgHook.ini files will _not_ be copied to
echo the program data directory due to lack of administrative rights!
echo In this case, you will have to _manually_ copy these .ini files
echo as an administrator afterwards.



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
set Version=1.6.2
set Component=SebClient
set Build=Release

set SebConfigDir=%ProgramData%\%Manufacturer%\%Product% %Version%
set SebInstallDir=%ProgramFiles%\%Manufacturer%\%Product% %Version%\%Component%\%Build%
set SebInstallDir(x86)=%ProgramFiles(x86)%\%Manufacturer%\%Product% %Version%\%Component%\%Build%

set InstallMsi=SebWindowsPackageSetup.msi
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


