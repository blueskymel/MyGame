@echo off
echo ========================================
echo   🎮 HOW TO RUN YOUR DONKEY KONG GAME 🎮
echo ========================================
echo.
echo The "multi-dimensional" look means your scene needs 
echo proper 2D setup. Here's how to fix it and play:
echo.

echo ========================================
echo   📋 STEP-BY-STEP INSTRUCTIONS
echo ========================================
echo.
echo 1️⃣  OPEN THE SCENE:
echo    • In Unity, go to Project window
echo    • Navigate to Assets ^> Scenes
echo    • Double-click "MainScene.unity"
echo.

echo 2️⃣  SETUP THE GAME SCENE:
echo    • In Unity menu bar, click: "Donkey Kong" ^> "Quick Play Setup"
echo    • This will automatically create all game objects
echo    • Wait for "Ready to play!" message in Console
echo.

echo 3️⃣  PLAY THE GAME:
echo    • Click the PLAY button ▶️ at the top of Unity
echo    • Game will start immediately!
echo.

echo ========================================
echo   🔧 FIXING THE "MULTI-DIMENSIONAL" LOOK
echo ========================================
echo.
echo The issue is likely one of these:
echo.
echo 🎯 CAMERA ISSUES:
echo    • Camera not set to "Orthographic" (2D mode)
echo    • Camera positioned wrong in 3D space
echo    • Camera size not optimized for 2D
echo.
echo 🎯 OBJECT ISSUES:
echo    • Game objects have 3D colliders instead of 2D
echo    • Objects positioned in wrong Z-depth
echo    • Missing proper 2D setup
echo.
echo ✅ THE QUICK FIX:
echo    Use "Donkey Kong" ^> "Quick Play Setup" menu
echo    This automatically fixes ALL common issues!
echo.

echo ========================================
echo   🎮 GAME CONTROLS WHEN PLAYING
echo ========================================
echo.
echo 🖱️  DESKTOP CONTROLS:
echo    • A/D or Arrow Keys: Move left/right
echo    • W/Space: Jump
echo    • S: Crouch/climb down
echo    • E: Use hammer (when available)
echo.
echo 📱 MOBILE CONTROLS (when built for mobile):
echo    • Touch buttons appear on screen
echo    • Drag for movement
echo    • Tap jump button
echo.

echo ========================================
echo   🏆 WHAT TO EXPECT
echo ========================================
echo.
echo When properly set up, you'll see:
echo.
echo 🦍 Donkey Kong at the top throwing barrels
echo 🏃 Red player character (Mario-style) you can control
echo 🛢️  Barrels rolling down platforms
echo 🏗️  Brown wooden platforms to climb
echo 🎯 Score display in top-left corner
echo 📱 Touch controls (if on mobile build)
echo.

echo ========================================
echo   ⚠️  TROUBLESHOOTING
echo ========================================
echo.
echo 🚨 IF GAME DOESN'T START:
echo    1. Check Console window for errors
echo    2. Run "Donkey Kong" ^> "Fix Compilation Errors"
echo    3. Try "Quick Play Setup" again
echo.
echo 🚨 IF SCENE LOOKS WEIRD:
echo    1. Select Main Camera in Hierarchy
echo    2. In Inspector, check "Orthographic" is enabled
echo    3. Set "Size" to 5
echo    4. Position at (0, 0, -10)
echo.
echo 🚨 IF CONTROLS DON'T WORK:
echo    1. Make sure Game view is focused (click on it)
echo    2. Check that Player object exists in scene
echo    3. Verify PlayerController script is attached
echo.

echo ========================================
echo   🎊 ENJOY YOUR GAME! 🎊
echo ========================================
echo.
echo You've built a complete Donkey Kong game!
echo.
echo Features included:
echo • Classic arcade gameplay
echo • Mobile touch controls
echo • Physics-based barrels
echo • Hammer power-ups
echo • Score system
echo • Multiple platforms
echo • Retro audio
echo.
echo Have fun playing your creation! 🕹️
echo.
pause
