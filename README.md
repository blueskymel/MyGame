# Donkey Kong Mobile Game

A complete mobile recreation of the classic Donkey Kong arcade game built with Unity, optimized for iOS and Android devices.

## Features

### Core Gameplay
- **Classic Donkey Kong Experience**: Faithful recreation of the original arcade game
- **Mario/Jumpman Character**: Full player controller with jumping, climbing, and movement
- **Donkey Kong Boss**: Animated character that throws barrels from the top platform
- **Rolling Barrels**: Physics-based barrel system with collision detection
- **Platform Climbing**: Ladder system for vertical movement
- **Hammer Power-up**: Temporary weapon to destroy barrels for bonus points

### Mobile Optimizations
- **Touch Controls**: Intuitive mobile-friendly control scheme
- **Multiple Input Methods**: Virtual buttons and gesture-based controls
- **Responsive UI**: Adaptive interface that works on various screen sizes
- **Performance Optimized**: Efficient rendering and physics for mobile devices

### Game Systems
- **Scoring System**: Points for barrels destroyed, reaching Princess Peach, and bonuses
- **Lives System**: Multiple lives with respawn mechanics
- **Progressive Difficulty**: Increasing challenge with barrel spawn rates
- **Audio System**: Complete sound effects and background music
- **Level Progression**: Multiple levels with increasing difficulty

### Technical Features
- **Unity 2022.3 LTS**: Built with latest stable Unity version
- **Cross-Platform**: Supports both iOS and Android
- **Modular Architecture**: Clean, maintainable code structure
- **Build Automation**: Batch scripts for easy deployment

## Project Structure

```
MyGame/
├── Assets/
│   ├── Scenes/
│   │   └── MainScene.unity
│   ├── Scripts/
│   │   ├── Player/
│   │   │   ├── PlayerController.cs
│   │   │   └── Hammer.cs
│   │   ├── Enemies/
│   │   │   ├── DonkeyKong.cs
│   │   │   ├── Barrel.cs
│   │   │   └── BarrelSpawner.cs
│   │   ├── GameManagement/
│   │   │   ├── GameManager.cs
│   │   │   ├── LevelBuilder.cs
│   │   │   └── AudioManager.cs
│   │   ├── UI/
│   │   │   ├── UIManager.cs
│   │   │   └── TouchControls.cs
│   │   └── Editor/
│   │       └── MobileBuildScript.cs
│   ├── Sprites/
│   ├── Audio/
│   ├── Prefabs/
│   └── Materials/
├── ProjectSettings/
├── build_mobile.bat (Windows)
├── build_mobile.sh (Mac/Linux)
└── README.md
```

## Installation & Setup

### Prerequisites
- Unity 2022.3.45f1 or later
- For iOS: Xcode and iOS Developer Account
- For Android: Android SDK and build tools

### Getting Started
1. Clone or download this repository
2. Open the project in Unity
3. Ensure all required platforms are installed in Unity Hub
4. Open the MainScene.unity in Assets/Scenes/

### Building for Mobile

#### Automated Build (Recommended)
**Windows:**
```bash
build_mobile.bat
```

**Mac/Linux:**
```bash
chmod +x build_mobile.sh
./build_mobile.sh
```

#### Manual Build in Unity
1. Go to **File > Build Settings**
2. Select your target platform (Android or iOS)
3. Configure player settings for mobile
4. Click **Build** or **Build and Run**

#### Platform-Specific Setup

**Android:**
- Set minimum API level to 21 (Android 5.0)
- Configure keystore for release builds
- Enable ARM64 architecture for Google Play requirements

**iOS:**
- Set minimum iOS version to 11.0
- Configure Bundle Identifier
- Set up provisioning profiles and signing certificates in Xcode
- Build through Xcode after Unity generates the project

## Controls

### Touch Controls
- **Movement**: Touch and drag on left side of screen OR use virtual D-pad
- **Jump**: Tap jump button on right side of screen
- **Climb Ladders**: Use up/down virtual buttons or vertical gestures
- **Hammer**: Tap hammer button when hammer is equipped

### Desktop Controls (for testing)
- **Movement**: Arrow keys or WASD
- **Jump**: Spacebar
- **Climb**: Up/Down arrows or W/S
- **Hammer**: Left Ctrl or Left Mouse Button

## Game Mechanics

### Scoring
- **Barrel Destroyed**: 300 points
- **Reaching Princess**: 5000 points
- **Hammer Pickup**: 500 points
- **Extra Life**: Every 20,000 points

### Difficulty Progression
- Barrel spawn rate increases with each level
- Barrel movement speed increases
- Additional hazards and obstacles in higher levels

### Power-ups
- **Hammer**: Temporary weapon lasting 10 seconds
- Allows destruction of barrels for bonus points
- Player cannot jump while holding hammer

## Development Notes

### Performance Optimizations
- Efficient sprite batching for mobile GPUs
- Object pooling for barrels and effects
- Optimized physics settings for mobile
- Compressed audio for smaller build sizes

### Mobile Adaptations
- Touch-friendly UI scaling
- Battery-optimized rendering
- Proper handling of device orientation changes
- Background/foreground state management

### Code Architecture
- Singleton pattern for managers (GameManager, AudioManager)
- Event-driven communication between systems
- Modular component design for easy testing
- Comprehensive error handling and logging

## Building for Distribution

### Google Play Store (Android)
1. Build signed APK with release keystore
2. Enable App Bundle format for size optimization
3. Test on various Android devices and screen sizes
4. Submit through Google Play Console

### Apple App Store (iOS)
1. Build through Xcode with distribution certificate
2. Create App Store listing in App Store Connect
3. Submit for review following Apple guidelines
4. Test on various iOS devices and screen sizes

## Troubleshooting

### Common Issues
- **Build Errors**: Ensure all required Unity modules are installed
- **Touch Not Working**: Check EventSystem is present in scene
- **Audio Issues**: Verify AudioManager is properly initialized
- **Performance**: Adjust quality settings for target devices

### Debug Features
- Development builds include console logging
- Profiler support for performance analysis
- Scene view gizmos for collision debugging

## Contributing

When contributing to this project:
1. Follow Unity C# coding conventions
2. Test on both iOS and Android devices
3. Maintain compatibility with Unity 2022.3 LTS
4. Document any new features or systems

## License

This project is for educational and demonstration purposes. Original Donkey Kong is a trademark of Nintendo.

## Credits

- **Original Game**: Nintendo (1981)
- **Unity Implementation**: Built with Unity Engine
- **Audio**: Procedurally generated audio system
- **Graphics**: Runtime sprite generation for cross-platform compatibility
