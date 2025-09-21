using UnityEngine;

public class LevelBuilder : MonoBehaviour
{
    [Header("Level Prefabs")]
    public GameObject platformPrefab;
    public GameObject ladderPrefab;
    public GameObject girderPrefab;
    public GameObject oilDrumPrefab;
    
    [Header("Level Dimensions")]
    public float levelWidth = 20f;
    public float levelHeight = 15f;
    public float platformSpacing = 3f;
    
    [Header("Materials")]
    public Material platformMaterial;
    public Material ladderMaterial;
    public Material backgroundMaterial;
    
    [System.Serializable]
    public class PlatformData
    {
        public Vector2 startPosition;
        public Vector2 endPosition;
        public bool hasLadder;
        public Vector2 ladderPosition;
        public float ladderHeight = 3f;
    }
    
    [Header("Platform Configuration")]
    public PlatformData[] platforms;
    
    void Start()
    {
        BuildLevel();
    }
    
    void BuildLevel()
    {
        CreateBackground();
        CreatePlatforms();
        CreateBoundaries();
        SetupCamera();
    }
    
    void CreateBackground()
    {
        // Create background sprite
        GameObject background = new GameObject("Background");
        SpriteRenderer bgRenderer = background.AddComponent<SpriteRenderer>();
        
        // Create a simple colored background texture
        Texture2D bgTexture = new Texture2D(1, 1);
        bgTexture.SetPixel(0, 0, new Color(0.1f, 0.1f, 0.3f)); // Dark blue
        bgTexture.Apply();
        
        Sprite bgSprite = Sprite.Create(bgTexture, new Rect(0, 0, 1, 1), new Vector2(0.5f, 0.5f), 1);
        bgRenderer.sprite = bgSprite;
        bgRenderer.sortingOrder = -10;
        
        // Scale background to cover the screen
        background.transform.localScale = new Vector3(levelWidth * 2, levelHeight * 2, 1);
        background.transform.position = new Vector3(levelWidth / 2, levelHeight / 2, 0);
    }
    
    void CreatePlatforms()
    {
        if (platforms == null || platforms.Length == 0)
        {
            CreateDefaultPlatforms();
        }
        else
        {
            CreateCustomPlatforms();
        }
    }
    
    void CreateDefaultPlatforms()
    {
        // Create classic Donkey Kong level layout
        
        // Bottom platform (ground)
        CreatePlatform(new Vector2(0, 0), new Vector2(levelWidth, 0), "Ground");
        CreateLadder(new Vector2(2, 0), platformSpacing);
        CreateLadder(new Vector2(levelWidth - 3, 0), platformSpacing);
        
        // Second level - slanted platforms
        CreatePlatform(new Vector2(0, platformSpacing), new Vector2(levelWidth * 0.8f, platformSpacing + 1f), "Platform1");
        CreateLadder(new Vector2(levelWidth * 0.7f, platformSpacing), platformSpacing);
        CreateLadder(new Vector2(3, platformSpacing), platformSpacing);
        
        // Third level
        CreatePlatform(new Vector2(levelWidth * 0.2f, platformSpacing * 2), new Vector2(levelWidth, platformSpacing * 2 + 0.5f), "Platform2");
        CreateLadder(new Vector2(levelWidth * 0.3f, platformSpacing * 2), platformSpacing);
        CreateLadder(new Vector2(levelWidth - 2, platformSpacing * 2), platformSpacing);
        
        // Fourth level - slanted the other way
        CreatePlatform(new Vector2(0, platformSpacing * 3), new Vector2(levelWidth * 0.7f, platformSpacing * 3 - 0.5f), "Platform3");
        CreateLadder(new Vector2(levelWidth * 0.6f, platformSpacing * 3), platformSpacing);
        CreateLadder(new Vector2(2, platformSpacing * 3), platformSpacing);
        
        // Top level - Donkey Kong's platform
        CreatePlatform(new Vector2(levelWidth * 0.3f, platformSpacing * 4), new Vector2(levelWidth, platformSpacing * 4), "TopPlatform");
        
        // Create oil drum (fire hazard)
        CreateOilDrum(new Vector2(1f, platformSpacing));
        
        // Princess platform
        CreatePlatform(new Vector2(levelWidth * 0.1f, platformSpacing * 4.5f), new Vector2(levelWidth * 0.25f, platformSpacing * 4.5f), "PrincessPlatform");
    }
    
    void CreateCustomPlatforms()
    {
        for (int i = 0; i < platforms.Length; i++)
        {
            PlatformData platform = platforms[i];
            CreatePlatform(platform.startPosition, platform.endPosition, $"Platform_{i}");
            
            if (platform.hasLadder)
            {
                CreateLadder(platform.ladderPosition, platform.ladderHeight);
            }
        }
    }
    
    void CreatePlatform(Vector2 startPos, Vector2 endPos, string name)
    {
        GameObject platform = new GameObject(name);
        platform.transform.parent = transform;
        
        // Calculate platform properties
        Vector2 center = (startPos + endPos) / 2f;
        float length = Vector2.Distance(startPos, endPos);
        float angle = Mathf.Atan2(endPos.y - startPos.y, endPos.x - startPos.x) * Mathf.Rad2Deg;
        
        // Set position and rotation
        platform.transform.position = center;
        platform.transform.rotation = Quaternion.Euler(0, 0, angle);
        
        // Add visual component
        SpriteRenderer renderer = platform.AddComponent<SpriteRenderer>();
        
        // Create platform sprite
        Texture2D platformTexture = new Texture2D((int)(length * 10), 20);
        Color platformColor = new Color(0.8f, 0.4f, 0.2f); // Brown color
        
        for (int x = 0; x < platformTexture.width; x++)
        {
            for (int y = 0; y < platformTexture.height; y++)
            {
                if (y < 3 || y > platformTexture.height - 4) // Borders
                {
                    platformTexture.SetPixel(x, y, Color.black);
                }
                else
                {
                    platformTexture.SetPixel(x, y, platformColor);
                }
            }
        }
        platformTexture.Apply();
        
        Sprite platformSprite = Sprite.Create(platformTexture, 
            new Rect(0, 0, platformTexture.width, platformTexture.height), 
            new Vector2(0.5f, 0.5f), 10);
        renderer.sprite = platformSprite;
        
        // Add collider
        BoxCollider2D collider = platform.AddComponent<BoxCollider2D>();
        collider.size = new Vector2(length, 0.2f);
        
        // Add platform tag
        platform.tag = name.Contains("Ground") ? "Ground" : "Platform";
        
        // Add platform layer
        platform.layer = LayerMask.NameToLayer("Ground");
    }
    
    void CreateLadder(Vector2 position, float height)
    {
        GameObject ladder = new GameObject("Ladder");
        ladder.transform.parent = transform;
        ladder.transform.position = position;
        
        // Add visual component
        SpriteRenderer renderer = ladder.AddComponent<SpriteRenderer>();
        
        // Create ladder sprite
        int ladderWidth = 30;
        int ladderHeight = (int)(height * 20);
        Texture2D ladderTexture = new Texture2D(ladderWidth, ladderHeight);
        
        Color ladderColor = new Color(0.6f, 0.3f, 0.1f); // Brown
        Color rungColor = new Color(0.8f, 0.5f, 0.2f); // Lighter brown
        
        for (int x = 0; x < ladderWidth; x++)
        {
            for (int y = 0; y < ladderHeight; y++)
            {
                if (x < 5 || x > ladderWidth - 6) // Sides
                {
                    ladderTexture.SetPixel(x, y, ladderColor);
                }
                else if (y % 20 < 5) // Rungs every 20 pixels
                {
                    ladderTexture.SetPixel(x, y, rungColor);
                }
                else
                {
                    ladderTexture.SetPixel(x, y, Color.clear);
                }
            }
        }
        ladderTexture.Apply();
        
        Sprite ladderSprite = Sprite.Create(ladderTexture, 
            new Rect(0, 0, ladderWidth, ladderHeight), 
            new Vector2(0.5f, 0f), 20);
        renderer.sprite = ladderSprite;
        renderer.sortingOrder = -1;
        
        // Add trigger collider for climbing
        BoxCollider2D collider = ladder.AddComponent<BoxCollider2D>();
        collider.size = new Vector2(1.5f, height);
        collider.isTrigger = true;
        collider.offset = new Vector2(0, height / 2f);
        
        // Add ladder tag and layer
        ladder.tag = "Ladder";
        ladder.layer = LayerMask.NameToLayer("Ladder");
    }
    
    void CreateOilDrum(Vector2 position)
    {
        GameObject oilDrum = new GameObject("OilDrum");
        oilDrum.transform.parent = transform;
        oilDrum.transform.position = position;
        
        // Add visual component
        SpriteRenderer renderer = oilDrum.AddComponent<SpriteRenderer>();
        
        // Create oil drum sprite (simple circle)
        int drumSize = 40;
        Texture2D drumTexture = new Texture2D(drumSize, drumSize);
        
        Vector2 center = new Vector2(drumSize / 2f, drumSize / 2f);
        float radius = drumSize / 2f - 2;
        
        for (int x = 0; x < drumSize; x++)
        {
            for (int y = 0; y < drumSize; y++)
            {
                float distance = Vector2.Distance(new Vector2(x, y), center);
                if (distance <= radius)
                {
                    if (distance > radius - 3)
                        drumTexture.SetPixel(x, y, Color.black); // Border
                    else
                        drumTexture.SetPixel(x, y, new Color(0.8f, 0.2f, 0.1f)); // Red drum
                }
                else
                {
                    drumTexture.SetPixel(x, y, Color.clear);
                }
            }
        }
        drumTexture.Apply();
        
        Sprite drumSprite = Sprite.Create(drumTexture, 
            new Rect(0, 0, drumSize, drumSize), 
            new Vector2(0.5f, 0.5f), 20);
        renderer.sprite = drumSprite;
        
        // Add collider
        CircleCollider2D collider = oilDrum.AddComponent<CircleCollider2D>();
        collider.radius = 1f;
        
        // Add fire effect (simple animation)
        FireEffect fireEffect = oilDrum.AddComponent<FireEffect>();
        
        oilDrum.tag = "Hazard";
    }
    
    void CreateBoundaries()
    {
        // Left boundary
        CreateInvisibleWall(new Vector2(-1, levelHeight / 2), new Vector2(1, levelHeight), "LeftWall");
        
        // Right boundary  
        CreateInvisibleWall(new Vector2(levelWidth + 1, levelHeight / 2), new Vector2(1, levelHeight), "RightWall");
        
        // Death zone at bottom
        GameObject deathZone = new GameObject("DeathZone");
        deathZone.transform.position = new Vector2(levelWidth / 2, -2);
        BoxCollider2D deathCollider = deathZone.AddComponent<BoxCollider2D>();
        deathCollider.size = new Vector2(levelWidth + 4, 1);
        deathCollider.isTrigger = true;
        deathZone.tag = "DeathZone";
    }
    
    void CreateInvisibleWall(Vector2 position, Vector2 size, string name)
    {
        GameObject wall = new GameObject(name);
        wall.transform.position = position;
        
        BoxCollider2D collider = wall.AddComponent<BoxCollider2D>();
        collider.size = size;
        
        wall.tag = "Wall";
    }
    
    void SetupCamera()
    {
        Camera mainCamera = Camera.main;
        if (mainCamera != null)
        {
            mainCamera.transform.position = new Vector3(levelWidth / 2, levelHeight / 2, -10);
            mainCamera.orthographicSize = levelHeight / 2 + 2;
        }
    }
}

// Simple fire effect for oil drum
public class FireEffect : MonoBehaviour
{
    private SpriteRenderer fireRenderer;
    private float animationSpeed = 2f;
    
    void Start()
    {
        // Create fire effect above the oil drum
        GameObject fire = new GameObject("Fire");
        fire.transform.parent = transform;
        fire.transform.localPosition = new Vector3(0, 1.5f, 0);
        
        fireRenderer = fire.AddComponent<SpriteRenderer>();
        fireRenderer.sortingOrder = 5;
        
        // Create simple fire texture
        CreateFireTexture();
    }
    
    void Update()
    {
        // Simple fire animation by scaling
        float scale = 1f + Mathf.Sin(Time.time * animationSpeed) * 0.2f;
        fireRenderer.transform.localScale = new Vector3(scale, scale, 1);
        
        // Change color slightly
        Color fireColor = Color.Lerp(Color.red, Color.yellow, Mathf.Sin(Time.time * animationSpeed * 2) * 0.5f + 0.5f);
        fireRenderer.color = fireColor;
    }
    
    void CreateFireTexture()
    {
        int fireSize = 20;
        Texture2D fireTexture = new Texture2D(fireSize, fireSize);
        
        for (int x = 0; x < fireSize; x++)
        {
            for (int y = 0; y < fireSize; y++)
            {
                float distance = Vector2.Distance(new Vector2(x, y), new Vector2(fireSize / 2f, 0));
                if (distance < fireSize / 2f && y < fireSize * 0.8f)
                {
                    fireTexture.SetPixel(x, y, new Color(1f, 0.5f, 0f, 0.8f)); // Orange fire
                }
                else
                {
                    fireTexture.SetPixel(x, y, Color.clear);
                }
            }
        }
        fireTexture.Apply();
        
        Sprite fireSprite = Sprite.Create(fireTexture, 
            new Rect(0, 0, fireSize, fireSize), 
            new Vector2(0.5f, 0f), 20);
        fireRenderer.sprite = fireSprite;
    }
}
