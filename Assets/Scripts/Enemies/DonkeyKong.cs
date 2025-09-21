using UnityEngine;
using System.Collections;

public class DonkeyKong : MonoBehaviour
{
    [Header("Animation Settings")]
    public float animationSpeed = 1f;
    public bool facingRight = false;
    
    [Header("Barrel Throwing")]
    public GameObject barrelPrefab;
    public Transform barrelSpawnPoint;
    public float throwForce = 5f;
    public float throwInterval = 3f;
    public float throwIntervalVariation = 1f;
    
    [Header("Audio")]
    public AudioClip roarSound;
    public AudioClip barrelThrowSound;
    
    [Header("Animation Frames")]
    public Sprite[] idleFrames;
    public Sprite[] throwFrames;
    public Sprite[] beatChestFrames;
    
    private SpriteRenderer spriteRenderer;
    private AudioSource audioSource;
    private Animator animator;
    private BarrelSpawner barrelSpawner;
    
    private bool isThrowingBarrel = false;
    private bool isBeatingChest = false;
    
    // Animation state tracking
    private int currentFrame = 0;
    private float animationTimer = 0f;
    private Sprite[] currentAnimation;
    
    // Barrel throwing state
    private float throwTimer = 0f;
    private float nextThrowTime;
    
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        barrelSpawner = GetComponent<BarrelSpawner>();
        
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        
        // Set initial animation
        currentAnimation = idleFrames;
        nextThrowTime = throwInterval + Random.Range(-throwIntervalVariation, throwIntervalVariation);
    }
    
    void Start()
    {
        CreateDonkeyKongSprites();
        
        // Start the barrel throwing coroutine
        StartCoroutine(BarrelThrowingRoutine());
        StartCoroutine(ChestBeatingRoutine());
    }
    
    void Update()
    {
        UpdateAnimation();
        UpdateBarrelThrowing();
    }
    
    void CreateDonkeyKongSprites()
    {
        if (idleFrames == null || idleFrames.Length == 0)
        {
            // Create basic DK sprites if none provided
            idleFrames = new Sprite[2];
            throwFrames = new Sprite[3];
            beatChestFrames = new Sprite[2];
            
            // Create idle animation frames
            for (int i = 0; i < idleFrames.Length; i++)
            {
                idleFrames[i] = CreateDKSprite(DKSpriteType.Idle, i);
            }
            
            // Create throw animation frames
            for (int i = 0; i < throwFrames.Length; i++)
            {
                throwFrames[i] = CreateDKSprite(DKSpriteType.Throw, i);
            }
            
            // Create chest beating animation frames
            for (int i = 0; i < beatChestFrames.Length; i++)
            {
                beatChestFrames[i] = CreateDKSprite(DKSpriteType.BeatChest, i);
            }
        }
        
        // Set initial sprite
        if (spriteRenderer != null && idleFrames.Length > 0)
        {
            spriteRenderer.sprite = idleFrames[0];
        }
    }
    
    enum DKSpriteType { Idle, Throw, BeatChest }
    
    Sprite CreateDKSprite(DKSpriteType type, int frame)
    {
        int width = 80;
        int height = 100;
        Texture2D texture = new Texture2D(width, height);
        
        // Fill with transparent
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                texture.SetPixel(x, y, Color.clear);
            }
        }
        
        // Draw basic DK shape based on type and frame
        Color bodyColor = new Color(0.4f, 0.2f, 0.1f); // Brown
        Color chestColor = new Color(0.6f, 0.4f, 0.2f); // Lighter brown
        Color eyeColor = Color.white;
        Color pupilColor = Color.black;
        
        switch (type)
        {
            case DKSpriteType.Idle:
                DrawDKIdle(texture, width, height, bodyColor, chestColor, eyeColor, pupilColor, frame);
                break;
            case DKSpriteType.Throw:
                DrawDKThrow(texture, width, height, bodyColor, chestColor, eyeColor, pupilColor, frame);
                break;
            case DKSpriteType.BeatChest:
                DrawDKBeatChest(texture, width, height, bodyColor, chestColor, eyeColor, pupilColor, frame);
                break;
        }
        
        texture.Apply();
        
        return Sprite.Create(texture, new Rect(0, 0, width, height), new Vector2(0.5f, 0f), 20);
    }
    
    void DrawDKIdle(Texture2D texture, int width, int height, Color body, Color chest, Color eye, Color pupil, int frame)
    {
        // Draw basic DK silhouette
        // Head
        DrawCircle(texture, width / 2, height - 20, 25, body);
        
        // Body
        DrawRectangle(texture, width / 2 - 20, height - 50, 40, 30, body);
        
        // Chest
        DrawRectangle(texture, width / 2 - 15, height - 45, 30, 20, chest);
        
        // Arms (animated)
        int armOffset = frame % 2 == 0 ? 0 : 2;
        DrawRectangle(texture, width / 2 - 35, height - 40 + armOffset, 15, 25, body);
        DrawRectangle(texture, width / 2 + 20, height - 40 - armOffset, 15, 25, body);
        
        // Legs
        DrawRectangle(texture, width / 2 - 15, height - 80, 12, 30, body);
        DrawRectangle(texture, width / 2 + 3, height - 80, 12, 30, body);
        
        // Eyes
        DrawCircle(texture, width / 2 - 8, height - 15, 4, eye);
        DrawCircle(texture, width / 2 + 8, height - 15, 4, eye);
        DrawCircle(texture, width / 2 - 8, height - 15, 2, pupil);
        DrawCircle(texture, width / 2 + 8, height - 15, 2, pupil);
    }
    
    void DrawDKThrow(Texture2D texture, int width, int height, Color body, Color chest, Color eye, Color pupil, int frame)
    {
        // Similar to idle but with throwing arm position
        DrawDKIdle(texture, width, height, body, chest, eye, pupil, 0);
        
        // Override right arm for throwing motion
        if (frame == 0) // Wind up
        {
            DrawRectangle(texture, width / 2 + 25, height - 30, 15, 25, body);
        }
        else if (frame == 1) // Mid throw
        {
            DrawRectangle(texture, width / 2 + 30, height - 45, 15, 25, body);
        }
        else // Follow through
        {
            DrawRectangle(texture, width / 2 + 20, height - 50, 15, 25, body);
        }
    }
    
    void DrawDKBeatChest(Texture2D texture, int width, int height, Color body, Color chest, Color eye, Color pupil, int frame)
    {
        // Draw base DK
        DrawDKIdle(texture, width, height, body, chest, eye, pupil, 0);
        
        // Override both arms for chest beating
        int armY = frame % 2 == 0 ? height - 35 : height - 30;
        DrawRectangle(texture, width / 2 - 30, armY, 15, 20, body);
        DrawRectangle(texture, width / 2 + 15, armY, 15, 20, body);
    }
    
    void DrawCircle(Texture2D texture, int centerX, int centerY, int radius, Color color)
    {
        for (int x = centerX - radius; x <= centerX + radius; x++)
        {
            for (int y = centerY - radius; y <= centerY + radius; y++)
            {
                if (x >= 0 && x < texture.width && y >= 0 && y < texture.height)
                {
                    float distance = Vector2.Distance(new Vector2(x, y), new Vector2(centerX, centerY));
                    if (distance <= radius)
                    {
                        texture.SetPixel(x, y, color);
                    }
                }
            }
        }
    }
    
    void DrawRectangle(Texture2D texture, int centerX, int centerY, int width, int height, Color color)
    {
        for (int x = centerX - width / 2; x <= centerX + width / 2; x++)
        {
            for (int y = centerY - height / 2; y <= centerY + height / 2; y++)
            {
                if (x >= 0 && x < texture.width && y >= 0 && y < texture.height)
                {
                    texture.SetPixel(x, y, color);
                }
            }
        }
    }
    
    void UpdateAnimation()
    {
        if (currentAnimation == null || currentAnimation.Length == 0) return;
        
        animationTimer += Time.deltaTime * animationSpeed;
        
        if (animationTimer >= 0.5f) // Change frame every 0.5 seconds
        {
            currentFrame = (currentFrame + 1) % currentAnimation.Length;
            spriteRenderer.sprite = currentAnimation[currentFrame];
            animationTimer = 0f;
        }
    }
    
    void UpdateBarrelThrowing()
    {
        throwTimer += Time.deltaTime;
        
        if (throwTimer >= nextThrowTime && !isThrowingBarrel)
        {
            StartCoroutine(ThrowBarrelAnimation());
            throwTimer = 0f;
            nextThrowTime = throwInterval + Random.Range(-throwIntervalVariation, throwIntervalVariation);
        }
    }
    
    IEnumerator ThrowBarrelAnimation()
    {
        isThrowingBarrel = true;
        currentAnimation = throwFrames;
        currentFrame = 0;
        
        // Play throw animation
        for (int i = 0; i < throwFrames.Length; i++)
        {
            currentFrame = i;
            spriteRenderer.sprite = throwFrames[i];
            
            // Throw barrel on middle frame
            if (i == 1)
            {
                ThrowBarrel();
            }
            
            yield return new WaitForSeconds(0.2f);
        }
        
        // Return to idle
        currentAnimation = idleFrames;
        currentFrame = 0;
        isThrowingBarrel = false;
    }
    
    void ThrowBarrel()
    {
        if (barrelPrefab == null || barrelSpawnPoint == null) return;
        
        // Create barrel
        GameObject barrel = Instantiate(barrelPrefab, barrelSpawnPoint.position, Quaternion.identity);
        
        // Add throw force
        Rigidbody2D barrelRb = barrel.GetComponent<Rigidbody2D>();
        if (barrelRb != null)
        {
            Vector2 throwDirection = facingRight ? Vector2.right : Vector2.left;
            throwDirection += Vector2.down * 0.5f; // Add some downward force
            barrelRb.AddForce(throwDirection * throwForce, ForceMode2D.Impulse);
        }
        
        // Configure barrel
        Barrel barrelScript = barrel.GetComponent<Barrel>();
        if (barrelScript != null)
        {
            barrelScript.SetRollDirection(facingRight ? 1f : -1f);
        }
        
        // Play sound
        if (barrelThrowSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(barrelThrowSound);
        }
        
        // Notify game manager
        if (GameManager.Instance != null)
        {
            GameManager.Instance.BarrelSpawned();
        }
    }
    
    IEnumerator ChestBeatingRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(8f, 15f));
            
            if (!isThrowingBarrel)
            {
                yield return StartCoroutine(BeatChestAnimation());
            }
        }
    }
    
    IEnumerator BeatChestAnimation()
    {
        isBeatingChest = true;
        currentAnimation = beatChestFrames;
        
        // Play roar sound
        if (roarSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(roarSound);
        }
        
        // Beat chest for a few seconds
        float beatDuration = 2f;
        float beatTimer = 0f;
        
        while (beatTimer < beatDuration)
        {
            beatTimer += Time.deltaTime;
            yield return null;
        }
        
        // Return to idle
        currentAnimation = idleFrames;
        isBeatingChest = false;
    }
    
    public void SetFacingDirection(bool right)
    {
        facingRight = right;
        spriteRenderer.flipX = !facingRight;
    }
    
    public void SetThrowInterval(float interval)
    {
        throwInterval = interval;
    }
    
    public void StartThrowing()
    {
        enabled = true;
    }
    
    public void StopThrowing()
    {
        enabled = false;
        StopAllCoroutines();
    }
}
