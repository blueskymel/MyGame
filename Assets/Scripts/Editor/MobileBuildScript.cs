using UnityEditor;
using UnityEngine;
using System.IO;

public class MobileBuildScript
{
    [MenuItem("Build/Build Android")]
    public static void BuildAndroid()
    {
        PlayerSettings.productName = "Donkey Kong Mobile";
        PlayerSettings.companyName = "DonkeyKongStudio";
        PlayerSettings.bundleVersion = "1.0.0";
        PlayerSettings.Android.bundleVersionCode = 1;
        
        // Android specific settings
        PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARM64 | AndroidArchitecture.ARMv7;
        PlayerSettings.Android.minSdkVersion = AndroidSdkVersions.AndroidApiLevel21;
        PlayerSettings.Android.targetSdkVersion = AndroidSdkVersions.AndroidApiLevelAuto;
        
        // Graphics settings for mobile
        PlayerSettings.SetGraphicsAPIs(BuildTarget.Android, new UnityEngine.Rendering.GraphicsDeviceType[] { 
            UnityEngine.Rendering.GraphicsDeviceType.OpenGLES3,
            UnityEngine.Rendering.GraphicsDeviceType.Vulkan
        });
        
        // Screen orientation
        PlayerSettings.defaultInterfaceOrientation = UIOrientation.LandscapeLeft;
        PlayerSettings.allowedAutorotateToLandscapeLeft = true;
        PlayerSettings.allowedAutorotateToLandscapeRight = true;
        PlayerSettings.allowedAutorotateToPortrait = false;
        PlayerSettings.allowedAutorotateToPortraitUpsideDown = false;
        
        // Quality settings for mobile
        QualitySettings.SetQualityLevel(2, true); // Medium quality
        
        // Build settings
        string buildPath = Path.Combine(Directory.GetCurrentDirectory(), "Builds", "Android");
        if (!Directory.Exists(buildPath))
        {
            Directory.CreateDirectory(buildPath);
        }
        
        string fileName = Path.Combine(buildPath, "DonkeyKongMobile.apk");
        
        BuildPipeline.BuildPlayer(
            GetScenePaths(),
            fileName,
            BuildTarget.Android,
            BuildOptions.None
        );
        
        Debug.Log($"Android build completed: {fileName}");
    }
    
    [MenuItem("Build/Build iOS")]
    public static void BuildiOS()
    {
        PlayerSettings.productName = "Donkey Kong Mobile";
        PlayerSettings.companyName = "DonkeyKongStudio";
        PlayerSettings.bundleVersion = "1.0.0";
        PlayerSettings.iOS.buildNumber = "1";
        
        // iOS specific settings
        PlayerSettings.iOS.targetDevice = iOSTargetDevice.iPhoneAndiPad;
        PlayerSettings.iOS.targetOSVersionString = "11.0";
        PlayerSettings.iOS.requiresPersistentWiFi = false;
        PlayerSettings.iOS.statusBarStyle = iOSStatusBarStyle.LightContent;
        PlayerSettings.iOS.hideHomeButton = false;
        
        // Graphics settings for iOS
        PlayerSettings.SetGraphicsAPIs(BuildTarget.iOS, new UnityEngine.Rendering.GraphicsDeviceType[] { 
            UnityEngine.Rendering.GraphicsDeviceType.Metal
        });
        
        // Screen orientation
        PlayerSettings.defaultInterfaceOrientation = UIOrientation.LandscapeLeft;
        PlayerSettings.allowedAutorotateToLandscapeLeft = true;
        PlayerSettings.allowedAutorotateToLandscapeRight = true;
        PlayerSettings.allowedAutorotateToPortrait = false;
        PlayerSettings.allowedAutorotateToPortraitUpsideDown = false;
        
        // Quality settings for mobile
        QualitySettings.SetQualityLevel(2, true); // Medium quality
        
        // Build settings
        string buildPath = Path.Combine(Directory.GetCurrentDirectory(), "Builds", "iOS");
        if (!Directory.Exists(buildPath))
        {
            Directory.CreateDirectory(buildPath);
        }
        
        BuildPipeline.BuildPlayer(
            GetScenePaths(),
            buildPath,
            BuildTarget.iOS,
            BuildOptions.None
        );
        
        Debug.Log($"iOS build completed: {buildPath}");
    }
    
    [MenuItem("Build/Build Both Platforms")]
    public static void BuildBothPlatforms()
    {
        BuildAndroid();
        BuildiOS();
    }
    
    [MenuItem("Build/Setup for Development")]
    public static void SetupForDevelopment()
    {
        // Enable development build
        EditorUserBuildSettings.development = true;
        EditorUserBuildSettings.allowDebugging = true;
        EditorUserBuildSettings.connectProfiler = true;
        
        Debug.Log("Development build settings enabled");
    }
    
    [MenuItem("Build/Setup for Release")]
    public static void SetupForRelease()
    {
        // Disable development build
        EditorUserBuildSettings.development = false;
        EditorUserBuildSettings.allowDebugging = false;
        EditorUserBuildSettings.connectProfiler = false;
        
        Debug.Log("Release build settings enabled");
    }
    
    private static string[] GetScenePaths()
    {
        string[] scenes = new string[EditorBuildSettings.scenes.Length];
        for (int i = 0; i < scenes.Length; i++)
        {
            scenes[i] = EditorBuildSettings.scenes[i].path;
        }
        return scenes;
    }
    
    [MenuItem("Build/Configure Touch Input")]
    public static void ConfigureTouchInput()
    {
        // Set up input for mobile
        PlayerSettings.virtualRealitySupported = false;
        PlayerSettings.accelerometerFrequency = 60;
        
        // Configure touch settings
        #if UNITY_ANDROID
        PlayerSettings.Android.forceSDCardPermission = false;
        PlayerSettings.Android.forceInternetPermission = false;
        #endif
        
        Debug.Log("Touch input configured for mobile platforms");
    }
    
    [MenuItem("Build/Optimize for Mobile")]
    public static void OptimizeForMobile()
    {
        // Graphics optimization
        PlayerSettings.gpuSkinning = true;
        PlayerSettings.graphicsJobs = true;
        
        // Audio optimization
        AudioSettings.configuration = AudioConfiguration.GetConfiguration();
        var config = AudioSettings.GetConfiguration();
        config.dspBufferSize = 1024; // Higher buffer for mobile
        AudioSettings.Reset(config);
        
        // Physics optimization
        Physics2D.velociterations = 6;
        Physics2D.positionIterations = 2;
        
        Debug.Log("Mobile optimizations applied");
    }
}
