@echo off
echo ========================================
echo   🔧 DONKEY KONG CHARACTER VISIBILITY FIX 🔧
echo ========================================
echo.
echo If you don't see characters after "Quick Play Setup":
echo.

echo ========================================
echo   ✅ STEP 1: CHECK SCENE VIEW
echo ========================================
echo.
echo 1. In Unity, click on the "Scene" tab (next to Game tab)
echo 2. Look for these objects in the Scene view:
echo    🏃 Red capsule (Player) - bottom left
echo    🦍 Brown capsule (DonkeyKong) - top center  
echo    🏗️ Brown cubes (Platforms) - multiple levels
echo.
echo If you see them in Scene but not Game view, continue...
echo.

echo ========================================
echo   ✅ STEP 2: CHECK CAMERA POSITION
echo ========================================
echo.
echo 1. Select "Main Camera" in Hierarchy
echo 2. In Inspector, verify:
echo    • Position: X=0, Y=0, Z=-10
echo    • Projection: Orthographic ✓
echo    • Size: 6
echo    • Clear Flags: Solid Color
echo    • Background: Black
echo.

echo ========================================
echo   ✅ STEP 3: CHECK HIERARCHY
echo ========================================
echo.
echo Look for these objects in Hierarchy window:
echo ✓ Main Camera
echo ✓ Player (red capsule)
echo ✓ DonkeyKong (brown capsule)
echo ✓ GameManager
echo ✓ UIManager
echo ✓ BarrelSpawner
echo ✓ Bottom Platform
echo ✓ Platform 1
echo ✓ Platform 2  
echo ✓ Top Platform
echo.

echo ========================================
echo   ✅ STEP 4: RESET SCENE VIEW
echo ========================================
echo.
echo 1. In Scene view, press F to focus on selected object
echo 2. Select Player object and press F
echo 3. You should see the red player character
echo 4. Select DonkeyKong and press F  
echo 5. You should see the brown Donkey Kong
echo.

echo ========================================
echo   ✅ STEP 5: FORCE REFRESH
echo ========================================
echo.
echo Try these Unity menu commands:
echo 1. "Donkey Kong" → "Setup Game Scene" (alternative)
echo 2. Edit → "Select All" → Delete → "Quick Play Setup" again
echo 3. Window → General → Console (check for error messages)
echo.

echo ========================================
echo   🎯 WHAT YOU SHOULD SEE
echo ========================================
echo.
echo In GAME view when you press PLAY:
echo 🏃 Red player on bottom-left platform
echo 🦍 Brown Donkey Kong at the top
echo 🏗️ Brown wooden platforms in levels
echo 🛢️ Barrels rolling down (when DK starts throwing)
echo 🎯 Score: 0 in top-left corner
echo.

echo ========================================
echo   🚨 TROUBLESHOOTING TIPS
echo ========================================
echo.
echo PROBLEM: Objects created but not visible
echo SOLUTION: Check camera orthographic size and position
echo.
echo PROBLEM: No objects in Hierarchy
echo SOLUTION: Run "Quick Play Setup" again, check Console for errors
echo.
echo PROBLEM: Objects appear as wireframes
echo SOLUTION: Switch from Wireframe to Shaded mode in Scene view
echo.
echo PROBLEM: Game view is empty/black
echo SOLUTION: Make sure Main Camera exists and is tagged "MainCamera"
echo.

echo ========================================
echo   🎮 READY TO PLAY!
echo ========================================
echo.
echo Once you see the characters:
echo 1. Click PLAY button ▶️ in Unity
echo 2. Use A/D or Arrow keys to move
echo 3. Use W/Space to jump
echo 4. Avoid barrels from Donkey Kong!
echo.
echo Your Donkey Kong game is ready! 🎊
echo.
pause
