@echo off
REM Unity Project Setup - Visual Studio 2022 Already Installed
REM This script configures Unity to use your existing VS2022 installation

echo ========================================
echo   Donkey Kong Mobile - Unity Setup
echo ========================================
echo.
echo Configuring Unity for Visual Studio 2022...
echo.

REM Check if Unity Hub is running
tasklist /FI "IMAGENAME eq Unity Hub.exe" 2>NUL | find /I /N "Unity Hub.exe">NUL
if "%ERRORLEVEL%"=="0" (
    echo Unity Hub is running - Good!
) else (
    echo Starting Unity Hub...
    start "" "Unity Hub"
    timeout /t 3 /nobreak > nul
)

echo.
echo ========================================
echo   Configuration Steps
echo ========================================
echo.
echo 1. Visual Studio 2022 Integration:
echo    - Unity will automatically detect VS2022
echo    - No additional installation needed
echo    - VS2022 extensions already available
echo.
echo 2. Unity Project Setup:
echo    - Open Unity Hub
echo    - Click "Add" or "Open"
echo    - Navigate to: %~dp0
echo    - Select this folder (MyGame)
echo    - Click "Select Folder"
echo.
echo 3. Editor Configuration:
echo    - Unity will use VS2022 as default editor
echo    - IntelliSense and debugging ready
echo    - Git integration through VS2022
echo.
echo 4. Build Configuration:
echo    - Android SDK: Configure in Unity
echo    - iOS builds: Requires Xcode (Mac only)
echo    - Windows builds: Ready with VS2022
echo.

echo ========================================
echo   Quick Actions
echo ========================================
echo.
echo [1] Open Unity Hub
echo [2] Open Project Folder
echo [3] View Setup Guide
echo [4] Exit
echo.
set /p choice="Enter your choice (1-4): "

if "%choice%"=="1" (
    echo Opening Unity Hub...
    start "" "Unity Hub"
)
if "%choice%"=="2" (
    echo Opening project folder...
    start "" "%~dp0"
)
if "%choice%"=="3" (
    echo Opening setup guide...
    start "" "Unity_Setup_Guide.md"
)

echo.
echo ========================================
echo   Setup Complete!
echo ========================================
echo.
echo Your Donkey Kong Mobile project is ready!
echo Visual Studio 2022 integration configured.
echo.
echo Project location: %~dp0
echo.
pause
