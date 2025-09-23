using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.Collections;

public class SceneSetup
{
    [MenuItem("Donkey Kong/Setup Game Scene")]
    public static void SetupGameScene()
    {
        Debug.Log("Setting up Donkey Kong game scene...");
        
        // Clear existing objects first
        ClearExistingObjects();
        
        // Create graphics manager
        SetupGraphics();
        
        // Create main camera with 2D settings
        SetupMainCamera();
        
        // Create game manager
        SetupGameManager();
        
        // Create level structure
        SetupLevel();
        
        // Create player
        SetupPlayer();
        
        // Create Donkey Kong
        SetupDonkeyKong();
        
        // Create barrel spawner
        SetupSimpleBarrelSpawner();
        
        // Create UI
        SetupUI();
        
        // Create intro sequence
        SetupIntroSequence();
        
        // Save the scene
        EditorSceneManager.SaveOpenScenes();
        
        Debug.Log("✅ Game scene setup complete! You should now see:");
        Debug.Log("   🏃 Red player character on bottom left");
        Debug.Log("   🦍 Brown Donkey Kong at the top");
        Debug.Log("   🏗️ Brown wooden platforms");
        Debug.Log("   🎬 INTRO CUTSCENE will play when you press PLAY!");
        Debug.Log("   Press PLAY to start the opening sequence!");
    }
    
    static void SetupMainCamera()
    {
        Camera mainCamera = Camera.main;
        if (mainCamera == null)
        {
            GameObject cameraObj = new GameObject("Main Camera");
            mainCamera = cameraObj.AddComponent<Camera>();
            cameraObj.tag = "MainCamera";
        }
        
        // Configure for TALL 2D game (portrait orientation like classic Donkey Kong)
        mainCamera.orthographic = true;
        mainCamera.orthographicSize = 8f; // Increased height to see more vertical space
        mainCamera.transform.position = new Vector3(0, 0, -10);
        mainCamera.backgroundColor = Color.black;
        mainCamera.clearFlags = CameraClearFlags.SolidColor;
        
        // Position camera to center on the level structure
        mainCamera.transform.position = new Vector3(0, 1, -10); // Centered on middle of platforms
        
        Debug.Log($"✓ Main Camera configured for TALL layout at position {mainCamera.transform.position}");
        Debug.Log($"  Camera size: {mainCamera.orthographicSize} (TALL view for classic Donkey Kong)");
    }
    
    static void SetupGameManager()
    {
        GameObject gameManagerObj = GameObject.Find("GameManager");
        if (gameManagerObj == null)
        {
            gameManagerObj = new GameObject("GameManager");
            gameManagerObj.AddComponent<GameManager>();
        }
        
        Debug.Log("✓ GameManager created");
    }
    
    static void SetupLevel()
    {
        GameObject levelObj = GameObject.Find("Level");
        if (levelObj == null)
        {
            levelObj = new GameObject("Level");
            levelObj.AddComponent<LevelBuilder>();
        }
        
        // Create platforms (TALL classic Donkey Kong layout - narrower platforms, more vertical)
        CreatePlatform("Bottom Platform", new Vector3(0, -6, 0), new Vector3(6, 0.5f, 1));   // Bottom
        CreatePlatform("Platform 1", new Vector3(0, -3, 0), new Vector3(5, 0.5f, 1));        // Level 1
        CreatePlatform("Platform 2", new Vector3(0, 0, 0), new Vector3(4, 0.5f, 1));         // Level 2  
        CreatePlatform("Platform 3", new Vector3(0, 3, 0), new Vector3(3, 0.5f, 1));         // Level 3
        CreatePlatform("Top Platform", new Vector3(0, 6, 0), new Vector3(2.5f, 0.5f, 1));    // DK's platform
        
        // Create highest platform for Princess (small platform at very top)
        CreatePlatform("Princess Platform", new Vector3(0, 8, 0), new Vector3(1.5f, 0.3f, 1));
        
        // Create ladders connecting the levels (positioned for tall layout)
        CreateLadder("Ladder 1", new Vector3(2, -4.5f, 0), 3f);    // Bottom to Level 1
        CreateLadder("Ladder 2", new Vector3(-2, -1.5f, 0), 3f);   // Level 1 to Level 2
        CreateLadder("Ladder 3", new Vector3(1.5f, 1.5f, 0), 3f);  // Level 2 to Level 3
        CreateLadder("Ladder 4", new Vector3(-1, 4.5f, 0), 3f);    // Level 3 to Top
        CreateLadder("Ladder 5", new Vector3(0, 7, 0), 2f);        // Top to Princess platform
        
        Debug.Log("✓ TALL level platforms and ladders created (classic Donkey Kong layout)");
    }
    
    static void CreateLadder(string name, Vector3 position, float height)
    {
        GameObject ladder = GameObject.CreatePrimitive(PrimitiveType.Cube);
        ladder.name = name;
        ladder.transform.position = position;
        ladder.transform.localScale = new Vector3(0.3f, height, 0.1f);
        
        // Make it yellow like classic DK ladders
        Renderer renderer = ladder.GetComponent<Renderer>();
        Material material = new Material(Shader.Find("Standard"));
        material.color = Color.yellow;
        material.SetFloat("_Metallic", 0);
        material.SetFloat("_Glossiness", 0.2f);
        renderer.material = material;
        
        // Remove collider so player can climb through
        UnityEngine.Object.DestroyImmediate(ladder.GetComponent<BoxCollider>());
        
        ladder.tag = "Ladder";
        
        Debug.Log($"✓ Ladder '{name}' created at {position}");
    }
    
    static void CreatePlatform(string name, Vector3 position, Vector3 scale)
    {
        GameObject platform = GameObject.CreatePrimitive(PrimitiveType.Cube);
        platform.name = name;
        platform.transform.position = position;
        platform.transform.localScale = scale;
        
        // Make it brown like wood - very visible!
        Renderer renderer = platform.GetComponent<Renderer>();
        Material material = new Material(Shader.Find("Standard"));
        material.color = new Color(0.6f, 0.3f, 0.1f); // Brown wood color
        material.SetFloat("_Metallic", 0);
        material.SetFloat("_Glossiness", 0.2f);
        renderer.material = material;
        
        // Add collider for platforms
        platform.tag = "Platform";
        
        Debug.Log($"✓ Platform '{name}' created at {position}");
    }
    
    static void SetupPlayer()
    {
        Debug.Log("Creating player character (tall layout)...");
        
        GameObject playerObj = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        playerObj.name = "Player";
        playerObj.transform.position = new Vector3(-2.5f, -5.5f, 0); // Bottom left of tall layout
        playerObj.transform.localScale = new Vector3(0.5f, 1f, 0.5f);
        
        // Make it bright red like Mario - very visible!
        Renderer renderer = playerObj.GetComponent<Renderer>();
        Material material = new Material(Shader.Find("Standard"));
        material.color = Color.red;
        material.SetFloat("_Metallic", 0);
        material.SetFloat("_Glossiness", 0.5f);
        renderer.material = material;
        
        // Convert to 2D collider FIRST (before adding Rigidbody2D)
        UnityEngine.Object.DestroyImmediate(playerObj.GetComponent<CapsuleCollider>());
        CapsuleCollider2D collider2D = playerObj.AddComponent<CapsuleCollider2D>();
        
        // Add 2D physics components
        Rigidbody2D rb = playerObj.AddComponent<Rigidbody2D>();
        rb.freezeRotation = true;
        
        // Add simple player controller (no complex dependencies)
        SimplePlayerController playerController = playerObj.AddComponent<SimplePlayerController>();
        playerController.moveSpeed = 5f;
        playerController.jumpForce = 12f;
        
        playerObj.tag = "Player";
        
        Debug.Log($"✓ Player created at bottom (tall layout): {playerObj.transform.position}");
    }
    
    static void SetupDonkeyKong()
    {
        Debug.Log("Creating Donkey Kong on top platform (tall layout)...");
        
        GameObject dkObj = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        dkObj.name = "DonkeyKong";
        
        // Position on left side of top platform (new tall layout)
        dkObj.transform.position = new Vector3(-0.8f, 6.5f, 0); // Above top platform at y=6
        dkObj.transform.localScale = new Vector3(1.5f, 2f, 1.5f); // Make it bigger and more visible
        
        Debug.Log($"🦍 DK positioned on top platform (tall layout): {dkObj.transform.position}");
        
        // Make it dark brown like an ape - very visible!
        Renderer renderer = dkObj.GetComponent<Renderer>();
        Material material = new Material(Shader.Find("Standard"));
        material.color = new Color(0.3f, 0.15f, 0.05f); // Dark brown
        material.SetFloat("_Metallic", 0);
        material.SetFloat("_Glossiness", 0.3f);
        renderer.material = material;
        
        // Convert to 2D collider FIRST
        UnityEngine.Object.DestroyImmediate(dkObj.GetComponent<CapsuleCollider>());
        CapsuleCollider2D collider2D = dkObj.AddComponent<CapsuleCollider2D>();
        
        // Add simple DonkeyKong script (no sprite dependencies)
        SimpleDonkeyKong dkScript = dkObj.AddComponent<SimpleDonkeyKong>();
        dkScript.bobSpeed = 2f;
        dkScript.bobAmount = 0.1f;
        
        dkObj.tag = "Enemy";
        
        Debug.Log($"✓ Donkey Kong created on top platform (tall layout): {dkObj.transform.position}");
    }
    
    static GameObject CreateBarrelPrefab()
    {
        Debug.Log("Creating barrel prefab...");
        
        GameObject barrelPrefab = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        barrelPrefab.name = "BarrelPrefab";
        barrelPrefab.transform.localScale = new Vector3(0.6f, 0.3f, 0.6f);
        
        // Make it brown like a wooden barrel
        Renderer renderer = barrelPrefab.GetComponent<Renderer>();
        Material material = new Material(Shader.Find("Standard"));
        material.color = new Color(0.5f, 0.25f, 0.1f); // Brown barrel color
        material.SetFloat("_Metallic", 0);
        material.SetFloat("_Glossiness", 0.1f);
        renderer.material = material;
        
        // Convert to 2D collider FIRST
        UnityEngine.Object.DestroyImmediate(barrelPrefab.GetComponent<CapsuleCollider>());
        CircleCollider2D collider = barrelPrefab.AddComponent<CircleCollider2D>();
        
        // Add physics components
        Rigidbody2D rb = barrelPrefab.AddComponent<Rigidbody2D>();
        rb.gravityScale = 1f;
        
        // Add barrel script
        barrelPrefab.AddComponent<Barrel>();
        
        barrelPrefab.tag = "Barrel";
        
        // Deactivate it initially (it's a prefab)
        barrelPrefab.SetActive(false);
        
        Debug.Log("✓ Barrel prefab created");
        return barrelPrefab;
    }
    
    static void SetupBarrelSpawner()
    {
        Debug.Log("Creating barrel spawner...");
        
        // Find the DonkeyKong object to get the barrel prefab
        GameObject dkObj = GameObject.Find("DonkeyKong");
        GameObject barrelPrefab = GameObject.Find("BarrelPrefab");
        
        GameObject spawnerObj = new GameObject("BarrelSpawner");
        spawnerObj.transform.position = new Vector3(0, 3.5f, 0);
        
        BarrelSpawner spawner = spawnerObj.AddComponent<BarrelSpawner>();
        
        // Set up the spawner with the barrel prefab
        if (barrelPrefab != null)
        {
            spawner.barrelPrefab = barrelPrefab;
            spawner.spawnPoint = spawnerObj.transform;
            spawner.spawnInterval = 2f;
            spawner.barrelSpeed = 3f;
            
            Debug.Log("✓ Barrel spawner configured with prefab");
        }
        
        Debug.Log($"✓ Barrel spawner created at position {spawnerObj.transform.position}");
    }
    
    static void SetupSimpleBarrelSpawner()
    {
        Debug.Log("Creating simple barrel spawner above Princess platform (tall layout)...");
        
        GameObject spawnerObj = new GameObject("SimpleBarrelSpawner");
        
        // Position spawner above Princess platform (y=8) in tall layout
        spawnerObj.transform.position = new Vector3(0, 9.5f, 0);
        
        Debug.Log($"🏭 Barrel spawner positioned above Princess platform (tall): {spawnerObj.transform.position}");
        
        SimpleBarrelSpawner spawner = spawnerObj.AddComponent<SimpleBarrelSpawner>();
        spawner.spawnInterval = 3f;
        spawner.initialForce = 3f;
        spawner.torque = 5f;
        
        Debug.Log($"✓ Barrel spawner created for tall layout: {spawnerObj.transform.position}");
        Debug.Log("  🛢️ Barrels will spawn from very top and tumble down all platforms!");
    }
    
    static void SetupUI()
    {
        GameObject uiObj = GameObject.Find("UIManager");
        if (uiObj == null)
        {
            uiObj = new GameObject("UIManager");
            uiObj.AddComponent<UIManager>();
        }
        
        GameObject touchControlsObj = GameObject.Find("TouchControls");
        if (touchControlsObj == null)
        {
            touchControlsObj = new GameObject("TouchControls");
            touchControlsObj.AddComponent<TouchControls>();
        }
        
        Debug.Log("✓ UI components created");
    }
    
    static void SetupIntroSequence()
    {
        GameObject introObj = new GameObject("GameIntroSequence");
        GameIntroSequence introScript = introObj.AddComponent<GameIntroSequence>();
        
        // Configure intro settings
        introScript.introDuration = 8f;
        introScript.climbSpeed = 2f;
        introScript.textDisplayTime = 2f;
        
        Debug.Log("✓ Game intro sequence created - will play opening cutscene!");
    }
    
    [MenuItem("Donkey Kong/Quick Play Setup")]
    public static void QuickPlaySetup()
    {
        Debug.Log("🎮 Setting up for immediate play...");
        
        SetupGameScene();
        
        // Focus on game view
        EditorApplication.ExecuteMenuItem("Window/General/Game");
        
        Debug.Log("🚀 Ready to play! Press the PLAY button ▶️ in Unity!");
    }
    
    static void ClearExistingObjects()
    {
        // Clear existing game objects to avoid duplicates
        GameObject[] toDestroy = {
            GameObject.Find("Player"),
            GameObject.Find("DonkeyKong"),
            GameObject.Find("GameManager"),
            GameObject.Find("UIManager"),
            GameObject.Find("TouchControls"),
            GameObject.Find("BarrelSpawner"),
            GameObject.Find("Level"),
            GameObject.Find("Bottom Platform"),
            GameObject.Find("Platform 1"),
            GameObject.Find("Platform 2"),
            GameObject.Find("Top Platform")
        };
        
        foreach (GameObject obj in toDestroy)
        {
            if (obj != null)
            {
                UnityEngine.Object.DestroyImmediate(obj);
            }
        }
        
        Debug.Log("✓ Cleared existing objects");
    }
    
    static void SetupGraphics()
    {
        // Create graphics manager with all components
        GameObject graphicsObj = new GameObject("GraphicsManager");
        graphicsObj.AddComponent<SpriteGenerator>();
        graphicsObj.AddComponent<CharacterGraphics>();
        graphicsObj.AddComponent<RuntimeGraphicsApplier>();
        
        Debug.Log("✓ Graphics system initialized with runtime applier");
    }
}
