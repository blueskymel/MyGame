using UnityEngine;
using UnityEditor;
using System.IO;

public class ProjectSetup
{
    [MenuItem("Donkey Kong/Setup Complete Project")]
    public static void SetupProject()
    {
        Debug.Log("Setting up Donkey Kong Mobile project...");
        
        CreateFolderStructure();
        SetupLayers();
        SetupTags();
        ConfigureMobileSettings();
        CreateMainScene();
        
        Debug.Log("Project setup complete! Ready to build Donkey Kong Mobile.");
    }
    
    static void CreateFolderStructure()
    {
        string[] folders = {
            "Assets/Scripts",
            "Assets/Scripts/Player",
            "Assets/Scripts/Enemies", 
            "Assets/Scripts/GameManagement",
            "Assets/Scripts/UI",
            "Assets/Scripts/Editor",
            "Assets/Scenes",
            "Assets/Prefabs",
            "Assets/Materials",
            "Assets/Sprites",
            "Assets/Audio"
        };
        
        foreach (string folder in folders)
        {
            if (!AssetDatabase.IsValidFolder(folder))
            {
                string parentFolder = Path.GetDirectoryName(folder);
                string newFolderName = Path.GetFileName(folder);
                AssetDatabase.CreateFolder(parentFolder, newFolderName);
            }
        }
        
        AssetDatabase.Refresh();
        Debug.Log("Folder structure created.");
    }
    
    static void SetupLayers()
    {
        // Set up physics layers
        string[] layers = {
            "Ground", "Ladder", "Player", "Enemy", "Pickup"
        };
        
        SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
        SerializedProperty layersProp = tagManager.FindProperty("layers");
        
        for (int i = 0; i < layers.Length; i++)
        {
            SerializedProperty layerProp = layersProp.GetArrayElementAtIndex(8 + i); // Start from layer 8
            layerProp.stringValue = layers[i];
        }
        
        tagManager.ApplyModifiedProperties();
        Debug.Log("Physics layers configured.");
    }
    
    static void SetupTags()
    {
        // Set up tags
        string[] tags = {
            "Barrel", "Enemy", "Hammer", "Princess", "Hazard", 
            "Ladder", "Wall", "DeathZone", "Platform"
        };
        
        SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
        SerializedProperty tagsProp = tagManager.FindProperty("tags");
        
        foreach (string tag in tags)
        {
            // Check if tag already exists
            bool tagExists = false;
            for (int i = 0; i < tagsProp.arraySize; i++)
            {
                if (tagsProp.GetArrayElementAtIndex(i).stringValue == tag)
                {
                    tagExists = true;
                    break;
                }
            }
            
            if (!tagExists)
            {
                tagsProp.InsertArrayElementAtIndex(tagsProp.arraySize);
                tagsProp.GetArrayElementAtIndex(tagsProp.arraySize - 1).stringValue = tag;
            }
        }
        
        tagManager.ApplyModifiedProperties();
        Debug.Log("Tags configured.");
    }
    
    static void ConfigureMobileSettings()
    {
        // Configure for mobile
        PlayerSettings.defaultInterfaceOrientation = UIOrientation.LandscapeLeft;
        PlayerSettings.allowedAutorotateToLandscapeLeft = true;
        PlayerSettings.allowedAutorotateToLandscapeRight = true;
        PlayerSettings.allowedAutorotateToPortrait = false;
        PlayerSettings.allowedAutorotateToPortraitUpsideDown = false;
        
        // Set company and product name
        PlayerSettings.companyName = "DonkeyKongStudio";
        PlayerSettings.productName = "Donkey Kong Mobile";
        
        Debug.Log("Mobile settings configured.");
    }
    
    static void CreateMainScene()
    {
        // Create main scene with essential GameObjects
        UnityEngine.SceneManagement.Scene newScene = UnityEditor.SceneManagement.EditorSceneManager.NewScene(UnityEditor.SceneManagement.NewSceneSetup.DefaultGameObjects);
        
        // Create GameManager
        GameObject gameManager = new GameObject("GameManager");
        // Note: You'll need to add the actual component after the script is imported
        
        // Create LevelBuilder
        GameObject levelBuilder = new GameObject("LevelBuilder");
        // Note: You'll need to add the actual component after the script is imported
        
        // Create AudioManager
        GameObject audioManager = new GameObject("AudioManager");
        // Note: You'll need to add the actual component after the script is imported
        
        // Save scene
        UnityEditor.SceneManagement.EditorSceneManager.SaveScene(newScene, "Assets/Scenes/MainScene.unity");
        
        Debug.Log("Main scene created.");
    }
    
    [MenuItem("Donkey Kong/Configure Build Settings")]
    public static void ConfigureBuildSettings()
    {
        // Add scenes to build settings
        string[] scenes = { "Assets/Scenes/MainScene.unity" };
        EditorBuildSettings.scenes = System.Array.ConvertAll(scenes, scene => new EditorBuildSettingsScene(scene, true));
        
        Debug.Log("Build settings configured.");
    }
}
