@echo off
REM Unity Compilation Error Fix Script
REM This script addresses common Unity compilation issues

echo ========================================
echo   Unity Compilation Error Fixer
echo ========================================
echo.
echo Fixing common Unity compilation issues...
echo.

REM Clean up potential duplicate files
echo [1/6] Checking for duplicate files...
for /r "Assets\Scripts" %%i in (*.cs~*) do (
    echo Removing backup file: %%i
    del "%%i" 2>nul
)

REM Remove meta files for non-existent assets
echo [2/6] Cleaning orphaned meta files...
for /r "Assets" %%i in (*.meta) do (
    set "file=%%i"
    set "original=!file:.meta=!"
    if not exist "!original!" (
        echo Removing orphaned meta file: %%i
        del "%%i" 2>nul
    )
)

REM Clear Unity cache
echo [3/6] Clearing Unity cache...
if exist "Library" (
    echo Removing Library folder...
    rmdir /s /q "Library" 2>nul
)
if exist "Temp" (
    echo Removing Temp folder...
    rmdir /s /q "Temp" 2>nul
)

REM Ensure proper folder structure
echo [4/6] Creating missing folders...
if not exist "Assets\Scenes" mkdir "Assets\Scenes"
if not exist "Assets\Prefabs" mkdir "Assets\Prefabs"
if not exist "Assets\Materials" mkdir "Assets\Materials"
if not exist "Assets\Sprites" mkdir "Assets\Sprites"
if not exist "Assets\Audio" mkdir "Assets\Audio"

REM Fix file permissions
echo [5/6] Fixing file permissions...
attrib -r "Assets\Scripts\*.cs" /s 2>nul

REM Create a simple validation scene if missing
echo [6/6] Checking for MainScene...
if not exist "Assets\Scenes\MainScene.unity" (
    echo MainScene.unity missing - will be created when you run Unity setup
)

echo.
echo ========================================
echo   Compilation Fix Complete!
echo ========================================
echo.
echo Next steps:
echo 1. Open Unity Hub
echo 2. Open this project
echo 3. In Unity, go to: Donkey Kong > Fix Compilation Errors
echo 4. If issues persist, go to: Donkey Kong > Setup Complete Project
echo.
echo Common solutions:
echo - Unity will regenerate Library and Temp folders
echo - Package dependencies will be resolved
echo - Script execution order will be configured
echo.
pause
