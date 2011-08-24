

@echo off

echo.
echo.
echo Safe Exam Browser startup
echo -------------------------
echo.
echo If additional applications shall be used in conjunction with SEB,
echo these applications must be installed previously to SEB,
echo using administrator rights, too.
echo.
echo After installing the permitted additional applications,
echo the teacher must check the paths to the program executables
echo of these applications. There are two ways to tell SEB where
echo it can find these program executables:
echo.
echo.
echo 1.) Enter the _full_ pathes to the program executables
echo into the SebStarter.ini file.
echo.
echo Example:
echo.
echo If you have installed the "VMware View Client" software,
echo and its program executable "wswc.exe" lies in the directory
echo "C:\Program Files\VMware\VMware View\Client\bin\",
echo then enter the full path to the program executable
echo in the SebStarter.ini file:
echo.
echo PERMITTED_APPS=VMwareViewClient,"C:\Program Files\VMware\VMware View\Client\bin\wswc.exe";
echo.
echo.
echo 2.) Enter only the _short_ pathes to the program executables
echo into the SebStarter.ini file.
echo.
echo In our example, "wscw.exe" is the short path,
echo so enter in the SebStarter.ini file only
echo.
echo PERMITTED_APPS=VMwareViewClient,wswc.exe;
echo.
echo But in this case, you have to additionally enter in SebStarter.bat the lines
echo.
echo set PermittedAppDir1="%ProgramFiles%\VMware\VMware View\Client\bin"
echo set PermittedAppDir1(x86)="%ProgramFiles(x86)%\VMware\VMware View\Client\bin"
echo.
echo (If you are not sure whether the full path contains "(x86)" or not,
echo just enter both lines to cover both cases).
echo The PATH variable is then automatically enhanced
echo by this path to the program executable.
echo.
echo It is possible to permit several such applications,
echo using "PermittedAppDir1=...", "PermittedAppDir2=...", etc.
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


echo Manufacturer = %Manufacturer%
echo Product      = %Product%
echo Version      = %Version%
echo Component    = %Component%
echo Build        = %Build%
echo.
echo SebConfigDir       = %SebConfigDir%
echo SebInstallDir      = %SebInstallDir%
echo SebInstallDir(x86) = %SebInstallDir(x86)%



echo Expand the PATH environment variable by the SEB directories
echo -----------------------------------------------------------

path %path%;%SebInstallDir%
path %path%;%SebInstallDir(x86)%



echo.
echo.
echo Assign the permitted applications executable directories
echo --------------------------------------------------------

@REM set PermittedAppDir1="%ProgramFiles%\VMware\VMware View\Client\bin"
@REM set PermittedAppDir1(x86)="%ProgramFiles(x86)%\VMware\VMware View\Client\bin"

@REM set PermittedAppDir2="..."
@REM set PermittedAppDir2(x86)="..."

@REM set PermittedAppDir3="..."
@REM set PermittedAppDir3(x86)="..."

@REM ...
@REM ...



echo Display the permitted applications executable directories
echo ---------------------------------------------------------

@REM echo.
@REM echo PermittedAppDir1      = %PermittedAppDir1%
@REM echo PermittedAppDir1(x86) = %PermittedAppDir1(x86)%

@REM echo.
@REM echo PermittedAppDir2      = %PermittedAppDir2%
@REM echo PermittedAppDir2(x86) = %PermittedAppDir2(x86)%

@REM echo.
@REM echo PermittedAppDir3      = %PermittedAppDir3%
@REM echo PermittedAppDir3(x86) = %PermittedAppDir3(x86)%

@REM ...
@REM ...
@REM ...



echo Expand the PATH environment variable by the permitted applications
echo ------------------------------------------------------------------

@REM path %path%;%PermittedAppDir1%
@REM path %path%;%PermittedAppDir1(x86)%

@REM path %path%;%PermittedAppDir2%
@REM path %path%;%PermittedAppDir2(x86)

@REM path %path%;%PermittedAppDir3%
@REM path %path%;%PermittedAppDir3(x86)

@REM ...
@REM ...



echo.
echo.
echo Display the enhanced PATH environment variable
echo ----------------------------------------------

path



echo.
echo.
echo Run the Safe Exam Browser (with permitted applications)
echo -------------------------------------------------------

@echo on

SebStarter.exe

@echo off



echo.
pause
pause
pause
@echo on


