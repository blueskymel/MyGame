using UnityEngine;
using UnityEditor;
using UnityEditor.Compilation;
using System.IO;

public class CompilationFixer
{
    [MenuItem("Donkey Kong/Fix Compilation Errors")]
    public static void FixCompilationErrors()
    {
        Debug.Log("Checking for compilation issues...");
        
        FixMissingDirectories();
        RefreshAssetDatabase();
        FixScriptExecutionOrder();
        FixAssemblyReferences();
        
        Debug.Log("Compilation fixes applied! If errors persist, check the Console for specific details.");
    }
    
    static void FixMissingDirectories()
    {
        string[] requiredFolders = {
            "Assets/Scenes",
            "Assets/Prefabs", 
            "Assets/Materials",
            "Assets/Sprites",
            "Assets/Audio",
            "Assets/Scripts/Player",
            "Assets/Scripts/Enemies",
            "Assets/Scripts/GameManagement",
            "Assets/Scripts/UI",
            "Assets/Scripts/Editor"
        };
        
        foreach (string folder in requiredFolders)
        {
            if (!AssetDatabase.IsValidFolder(folder))
            {
                string parentFolder = Path.GetDirectoryName(folder);
                string newFolderName = Path.GetFileName(folder);
                AssetDatabase.CreateFolder(parentFolder, newFolderName);
                Debug.Log($"Created missing folder: {folder}");
            }
        }
    }
    
    static void RefreshAssetDatabase()
    {
        AssetDatabase.Refresh();
        Debug.Log("Asset database refreshed.");
    }
    
    static void FixScriptExecutionOrder()
    {
        // Set script execution order to prevent initialization issues
        SetScriptExecutionOrder("GameManager", -100);
        SetScriptExecutionOrder("AudioManager", -90);
        SetScriptExecutionOrder("UIManager", -80);
        SetScriptExecutionOrder("LevelBuilder", -70);
        
        Debug.Log("Script execution order configured.");
    }
    
    static void SetScriptExecutionOrder(string scriptName, int order)
    {
        var script = AssetDatabase.FindAssets($"t:MonoScript {scriptName}");
        if (script.Length > 0)
        {
            var scriptPath = AssetDatabase.GUIDToAssetPath(script[0]);
            var monoScript = AssetDatabase.LoadAssetAtPath<MonoScript>(scriptPath);
            if (monoScript != null)
            {
                MonoImporter.SetExecutionOrder(monoScript, order);
            }
        }
    }
    
    static void FixAssemblyReferences()
    {
        // Force recompilation
        CompilationPipeline.RequestScriptCompilation();
        Debug.Log("Forced script recompilation.");
    }
    
    [MenuItem("Donkey Kong/Validate Project Structure")]
    public static void ValidateProject()
    {
        Debug.Log("Validating project structure...");
        
        // Check for essential scripts
        string[] essentialScripts = {
            "Assets/Scripts/GameManagement/GameManager.cs",
            "Assets/Scripts/Player/PlayerController.cs",
            "Assets/Scripts/UI/UIManager.cs",
            "Assets/Scripts/GameManagement/AudioManager.cs",
            "Assets/Scripts/GameManagement/LevelBuilder.cs"
        };
        
        bool allScriptsFound = true;
        foreach (string script in essentialScripts)
        {
            if (!File.Exists(script))
            {
                Debug.LogError($"Missing essential script: {script}");
                allScriptsFound = false;
            }
        }
        
        if (allScriptsFound)
        {
            Debug.Log("✓ All essential scripts found.");
        }
        
        // Check scene
        if (!File.Exists("Assets/Scenes/MainScene.unity"))
        {
            Debug.LogWarning("MainScene.unity not found. Run 'Setup Complete Project' to create it.");
        }
        else
        {
            Debug.Log("✓ MainScene.unity found.");
        }
        
        Debug.Log("Project validation complete.");
    }
}
