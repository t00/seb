

@echo off

echo.
echo.
echo Safe Exam Browser cleanup
echo -------------------------
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
set Version=1.8
set Component=SebWindowsClient
set Build=Release

set SebConfigDir=%ProgramData%\%Manufacturer%\%Product% %Version%

set SebInstallDir=%ProgramFiles%\%Manufacturer%\%Product% %Version%
set SebClientDir=%ProgramFiles%\%Manufacturer%\%Product% %Version%\%Component%
set SebReleaseDir=%ProgramFiles%\%Manufacturer%\%Product% %Version%\%Component%\%Build%

set SebInstallDir(x86)=%ProgramFiles(x86)%\%Manufacturer%\%Product% %Version%
set SebClientDir(x86)=%ProgramFiles(x86)%\%Manufacturer%\%Product% %Version%\%Component%
set SebReleaseDir(x86)=%ProgramFiles(x86)%\%Manufacturer%\%Product% %Version%\%Component%\%Build%

set InstallMsi=SebWindowsInstall.msi
set SebStarterBat=SebStarter.bat
set SebStarterIni=SebStarter.ini
set MsgHookIni=MsgHook.ini

set InstallMsiFile=%BatchDir%%InstallMsi%
set SebStarterBatFile=%BatchDir%%SebStarterBat%
set SebStarterIniFile=%BatchDir%%SebStarterIni%
set MsgHookIniFile=%BatchDir%%MsgHookIni%

set XulSebZip=xul_seb.zip
set XulRunnerZip=xulrunner.zip
set XulRunnerNoSslZip=xulrunner_no_ssl_warning.zip

set XulSebZipFile=%SebClientDir%\%XulSebZip%
set XulRunnerZipFile=%SebClientDir%\%XulRunnerZip%
set XulRunnerNoSslZipFile=%SebClientDir%\%XulRunnerNoSslZip%

set XulSebZipFile(x86)=%SebClientDir(x86)%\%XulSebZip%
set XulRunnerZipFile(x86)=%SebClientDir(x86)%\%XulRunnerZip%
set XulRunnerNoSslZipFile(x86)=%SebClientDir(x86)%\%XulRunnerNoSslZip%


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
echo XulSebZip          = %XulSebZip%
echo XulRunnerZip       = %XulRunnerZip%
echo XulRunnerNoSslZip  = %XulRunnerNoSSlZip%
echo.
echo XulSebZipFile         = %XulSebZipFile%
echo XulRunnerZipFile      = %XulRunnerZipFile%
echo XulRunnerNoSslZipFile = %XulRunnerNoSSlZipFile%
echo.
echo XulSebZipFile(x86)         = %XulSebZipFile(x86)%
echo XulRunnerZipFile(x86)      = %XulRunnerZipFile(x86)%
echo XulRunnerNoSslZipFile(x86) = %XulRunnerNoSSlZipFile(x86)%



@REM echo Expand the PATH environment variable by the batch directory
@REM echo -----------------------------------------------------------

path %path%;%BatchDir%



echo.
echo.
echo Delete the SEB configuration directory
echo Delete the SEB  installation directory
echo -----------------------------------------------------------------

@echo on

rmdir /s /q "%SebConfigDir%"
rmdir /s /q "%SebInstallDir%"
rmdir /s /q "%SebInstallDir(x86)%"

@echo off



echo.
pause
@echo on


