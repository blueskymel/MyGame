@echo off
REM Git repository setup for sharing Donkey Kong Mobile project
REM Modified to skip Visual Studio 2022 installation (already installed)

echo ========================================
echo   Donkey Kong Mobile - Git Setup
echo ========================================
echo.
echo Visual Studio 2022 detected - skipping installation
echo Setting up Git repository...
echo.

REM 1. Initialize git repository
echo Initializing Git repository...
git init

REM 2. Add all files
echo Adding project files...
git add .

REM 3. Make initial commit
echo Making initial commit...
git commit -m "Initial commit - Donkey Kong Mobile Unity project"

echo.
echo ========================================
echo   Setup Complete!
echo ========================================
echo.
echo Your project is ready for GitHub upload!
echo.
echo Next steps:
echo 1. Create a new repository on GitHub
echo 2. Copy the repository URL
echo 3. Run: git remote add origin [your-repo-url]
echo 4. Run: git push -u origin main
echo.
echo Visual Studio 2022 integration ready!
echo Unity will use your existing VS2022 installation.
echo.
pause
