

@echo off

echo.
echo.
echo Safe Exam Browser silent installation
echo =====================================
echo.
echo The whole installation procedure comprises two phases:
echo.
echo.
echo 1st phase (configuration):
echo --------------------------
echo The teacher configures SEB by modifiying
echo the "MsgHook.ini" and "SebStarter.ini" files using a text editor.
echo.
echo If third-party applications shall be permitted,
echo the teacher can additionally edit the "SebStarter.bat" file.
echo This batch file enhances the PATH variable by the paths to the
echo third-party applications, such that SEB can find the executables
echo of these applications when they are called during an exam.
echo.
echo The teacher packs the whole directory into a zip file,
echo e.g. "SebWindowsInstall.zip",
echo and distributes the zip file to the students.
echo.
echo.
echo 2nd phase (actual installation):
echo --------------------------------
echo The student unzips the "SebWindowsInstall.zip" file
echo in an arbitrary folder (e.g. "C:\tmp") on his machine.
echo.
echo Then he double-clicks the "SebWindowsInstall.bat" script
echo when logged in as an administrator (not as a standard user!)
echo.
echo This script runs the MSI installer with the option /i
echo and the installation file "SebWindowsInstall.msi",
echo and uses the installation directory given by the
echo INSTALLDIR parameter.
echo.
echo Usually INSTALLDIR is a subdirectory of "C:\Program Files"
echo or "C:\Program Files (x86)". The parameter string is
echo built by this script from the ingredients
echo "Manufacturer", "Product", "Version" etc. (see below).
echo.
echo After installation, the script copies the (previously modified)
echo configuration files "MsgHook.ini" and "SebStarter.ini",
echo (and also the batch file "SebStarter.bat"),
echo to the application data directory, which usually is
echo a subdirectory of "C:\Program Data".
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
set Version=1.9.0
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



set SebInstallZip=SebWindowsInstall.zip
set SebInstallMsi=SebWindowsInstall.msi
set SebStarterBat=SebStarter.bat
set SebStarterIni=SebStarter.ini
set SebMsgHookIni=MsgHook.ini

set SebInstallZipFile=%BatchDir%%SebInstallZip%
set SebInstallMsiFile=%BatchDir%%SebInstallMsi%
set SebStarterBatFile=%BatchDir%%SebStarterBat%
set SebStarterIniFile=%BatchDir%%SebStarterIni%
set SebMsgHookIniFile=%BatchDir%%SebMsgHookIni%

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
echo SebInstallZip      = %SebInstallZip%
echo SebInstallMsi      = %SebInstallMsi%
echo SebStarterBat      = %SebStarterBat%
echo SebStarterIni      = %SebStarterIni%
echo SebMsgHookIni      = %SebMsgHookIni%
echo.
echo SebInstallZipFile  = %SebInstallZipFile%
echo SebInstallMsiFile  = %SebInstallMsiFile%
echo SebStarterBatFile  = %SebStarterBatFile%
echo SebStarterIniFile  = %SebStarterIniFile%
echo SebMsgHookIniFile  = %SebMsgHookIniFile%
echo.
echo CommonAppDataFolder = %CommonAppDataFolder%
echo SebAdminImage       = %SebAdminImage%
echo SebAdminImageDir    = %SebAdminImageDir%
echo SebAdminConfigDir   = %SebAdminConfigDir%
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
echo Run the installation with the .msi file
echo ---------------------------------------

@echo on
@REM pause

@REM msiexec /q /i "%SebInstallMsiFile%" INSTALLDIR="%SebInstallDir%"
     msiexec /q /i "%SebInstallMsiFile%" INSTALLDIR="%SebInstallDir(x86)%"

@REM For example:
@REM
@REM If this script "SebWindowsInstall.bat" and the installer
@REM "SebWindowsInstall.msi" are both located in "C:\tmp",
@REM the Windows version is English,
@REM and 32 bit program version desired even on a 64 bit machine,
@REM then the following command line installs Safe Exam Browser
@REM for Windows 1.8.1 on the computer:
@REM
@REM msiexec /q /i "C:\tmp\SebWindowsInstall.msi" INSTALLDIR="C:\Program Files (x86)\ETH Zuerich\SEB Windows 1.8.1"

@REM pause
@echo off



echo.
echo.
echo Copy the configured .ini files to the SEB configuration directory
echo Copy the configured .bat file  to the SEB  installation directory
echo -----------------------------------------------------------------

@echo on

copy "%SebMsgHookIniFile%" "%SebConfigDir%"
copy "%SebStarterIniFile%" "%SebConfigDir%"

copy "%SebStarterBatFile%" "%SebInstallDir%"
copy "%SebStarterBatFile%" "%SebInstallDir(x86)%"

@echo off



echo.
@REM pause
@echo on


