 

@echo off

echo.
echo.
echo Safe Exam Browser silent installation
echo =====================================



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
set Version=1.9.1
set Component=SebWindowsClient
set Build=Release

set SebInstallZip=SebWindowsInstall.zip
set SebStarterBat=SebStarter.bat
set SebStarterIni=SebStarter.ini

set SebInstallZipFile=%BatchDir%%SebInstallZip%
set SebStarterBatFile=%BatchDir%%SebStarterBat%
set SebStarterIniFile=%BatchDir%%SebStarterIni%



echo Manufacturer = %Manufacturer%
echo Product      = %Product%
echo Version      = %Version%
echo Component    = %Component%
echo Build        = %Build%
echo.
echo SebInstallZip      = %SebInstallZip%
echo SebStarterBat      = %SebStarterBat%
echo SebStarterIni      = %SebStarterIni%
echo.
echo SebInstallZipFile  = %SebInstallZipFile%
echo SebStarterBatFile  = %SebStarterBatFile%
echo SebStarterIniFile  = %SebStarterIniFile%



echo.
echo.
echo Copy the default .ini file to the SEB zip directory
echo Copy the default .bat file to the SEB zip directory
echo ---------------------------------------------------

@echo on

set SourceDir="C:\Users\Dirk\seb\trunk\win\SebWindowsPackage"
set TargetDir="C:\Users\Dirk\tmp\seb_%Version%_win_light\Release"

copy "%SourceDir%\Release\MsgHook.dll"                               %TargetDir%
copy "%SourceDir%\Release\SebStarter.exe"                            %TargetDir%
copy "%SourceDir%\SebWindowsConfig\bin\Release\SebWindowsConfig.exe" %TargetDir%
copy "%SourceDir%\SebWindowsClient\SebStarter\SebStarter.ini"        %TargetDir%
copy "%SourceDir%\SebWindowsClient\SebStarter\SebStarter.bat"        %TargetDir%

@echo off



echo.
@REM pause
@echo on


