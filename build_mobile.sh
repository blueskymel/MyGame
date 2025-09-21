#!/bin/bash

# Build script for Donkey Kong Mobile Game
# Requires Unity to be installed

echo "Building Donkey Kong Mobile for all platforms..."

# Set Unity path (modify this to match your Unity installation)
UNITY_PATH="/Applications/Unity/Hub/Editor/2022.3.45f1/Unity.app/Contents/MacOS/Unity"

# Set project path
PROJECT_PATH="$(pwd)"

# Create builds directory
mkdir -p "Builds"

echo ""
echo "========================================"
echo "Building Android APK..."
echo "========================================"
"$UNITY_PATH" -batchmode -quit -projectPath "$PROJECT_PATH" -executeMethod MobileBuildScript.BuildAndroid -logFile "Builds/android_build.log"

if [ $? -eq 0 ]; then
    echo "Android build completed successfully!"
else
    echo "Android build failed! Check Builds/android_build.log for details."
fi

echo ""
echo "========================================"
echo "Building iOS Xcode Project..."
echo "========================================"
"$UNITY_PATH" -batchmode -quit -projectPath "$PROJECT_PATH" -executeMethod MobileBuildScript.BuildiOS -logFile "Builds/ios_build.log"

if [ $? -eq 0 ]; then
    echo "iOS build completed successfully!"
    echo "Note: Open the Xcode project in Builds/iOS/ to build for device."
else
    echo "iOS build failed! Check Builds/ios_build.log for details."
fi

echo ""
echo "========================================"
echo "Build process completed!"
echo "========================================"
echo "Android APK: Builds/Android/DonkeyKongMobile.apk"
echo "iOS Project: Builds/iOS/"
echo ""
echo "For iOS deployment:"
echo "1. Open Builds/iOS/Unity-iPhone.xcodeproj in Xcode"
echo "2. Configure signing and provisioning profiles"
echo "3. Build and deploy to device or App Store"
echo ""

read -p "Press any key to continue..."
