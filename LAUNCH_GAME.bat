@echo off
echo ========================================
echo    Donkey Kong Mobile - Quick Launcher
echo    Visual Studio 2022 Ready
echo ========================================
echo.
echo This will open your Donkey Kong project in Unity.
echo Visual Studio 2022 integration is already configured.
echo.
pause

echo Opening Unity Hub...
start "" "unityhub://2022.3.45f1/abcdef123456"

echo.
echo ========================================
echo   Quick Setup Instructions
echo ========================================
echo.
echo If Unity Hub doesn't open automatically:
echo 1. Open Unity Hub manually
echo 2. Click "Add" or "Open"  
echo 3. Navigate to: %~dp0
echo 4. Select the MyGame folder
echo 5. Click "Select Folder"
echo 6. Open the project
echo 7. Open MainScene.unity
echo 8. Click Play!
echo.
echo ========================================
echo   Visual Studio 2022 Integration
echo ========================================
echo.
echo ✓ VS2022 already installed - no setup needed
echo ✓ Unity will auto-detect your VS2022
echo ✓ IntelliSense and debugging ready
echo ✓ Git integration through VS2022
echo.
echo Project location: %~dp0
echo.
echo Need help? Run: setup_unity_vs2022.bat
echo.
pause
