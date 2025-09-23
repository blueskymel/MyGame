using UnityEngine;

public class SpriteGenerator : MonoBehaviour
{
    [Header("Sprite Settings")]
    public int spriteWidth = 32;
    public int spriteHeight = 32;
    public FilterMode filterMode = FilterMode.Point; // Pixel art style
    
    public static SpriteGenerator Instance { get; private set; }
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public Sprite CreateMarioSprite()
    {
        Texture2D texture = new Texture2D(spriteWidth, spriteHeight);
        Color[] pixels = new Color[spriteWidth * spriteHeight];
        
        // Initialize with transparent pixels
        for (int i = 0; i < pixels.Length; i++)
        {
            pixels[i] = Color.clear;
        }
        
        // Mario colors
        Color red = new Color(0.8f, 0.1f, 0.1f);      // Hat/shirt
        Color blue = new Color(0.1f, 0.1f, 0.8f);     // Overalls
        Color brown = new Color(0.6f, 0.4f, 0.2f);    // Hair/mustache
        Color skin = new Color(1f, 0.8f, 0.6f);       // Skin
        Color yellow = new Color(1f, 1f, 0.2f);       // Buttons
        Color black = Color.black;                      // Outline
        
        // Draw Mario (simplified pixel art)
        // Hat (top)
        DrawRect(pixels, 8, 4, 16, 6, red);
        DrawRect(pixels, 6, 6, 20, 2, red);
        
        // Face
        DrawRect(pixels, 10, 10, 12, 8, skin);
        
        // Eyes
        SetPixel(pixels, 12, 14, black);
        SetPixel(pixels, 18, 14, black);
        
        // Mustache
        DrawRect(pixels, 11, 12, 10, 2, brown);
        
        // Shirt
        DrawRect(pixels, 8, 18, 16, 8, red);
        
        // Overalls
        DrawRect(pixels, 10, 20, 12, 6, blue);
        
        // Buttons
        SetPixel(pixels, 14, 22, yellow);
        SetPixel(pixels, 18, 22, yellow);
        
        // Arms
        DrawRect(pixels, 6, 20, 2, 6, skin);
        DrawRect(pixels, 24, 20, 2, 6, skin);
        
        // Legs
        DrawRect(pixels, 10, 26, 4, 6, blue);
        DrawRect(pixels, 18, 26, 4, 6, blue);
        
        texture.SetPixels(pixels);
        texture.filterMode = filterMode;
        texture.Apply();
        
        return Sprite.Create(texture, new Rect(0, 0, spriteWidth, spriteHeight), Vector2.one * 0.5f, spriteWidth);
    }
    
    public Sprite CreateDonkeyKongSprite()
    {
        Texture2D texture = new Texture2D(spriteWidth, spriteHeight);
        Color[] pixels = new Color[spriteWidth * spriteHeight];
        
        // Initialize with transparent pixels
        for (int i = 0; i < pixels.Length; i++)
        {
            pixels[i] = Color.clear;
        }
        
        // Donkey Kong colors
        Color darkBrown = new Color(0.3f, 0.15f, 0.05f);
        Color lightBrown = new Color(0.5f, 0.3f, 0.1f);
        Color tan = new Color(0.8f, 0.6f, 0.4f);
        Color red = new Color(0.8f, 0.1f, 0.1f);
        Color black = Color.black;
        Color white = Color.white;
        
        // Body (large gorilla shape)
        DrawRect(pixels, 6, 8, 20, 20, darkBrown);
        
        // Chest/belly
        DrawRect(pixels, 10, 12, 12, 12, tan);
        
        // Head
        DrawRect(pixels, 8, 2, 16, 12, darkBrown);
        
        // Face
        DrawRect(pixels, 10, 6, 12, 8, tan);
        
        // Eyes
        DrawRect(pixels, 12, 10, 2, 2, white);
        DrawRect(pixels, 18, 10, 2, 2, white);
        SetPixel(pixels, 13, 11, black);
        SetPixel(pixels, 19, 11, black);
        
        // Nose
        DrawRect(pixels, 15, 8, 2, 2, black);
        
        // Mouth (angry expression)
        DrawRect(pixels, 13, 6, 6, 1, black);
        
        // Arms
        DrawRect(pixels, 2, 12, 4, 12, darkBrown);
        DrawRect(pixels, 26, 12, 4, 12, darkBrown);
        
        // Hands (fists)
        DrawRect(pixels, 0, 18, 4, 4, darkBrown);
        DrawRect(pixels, 28, 18, 4, 4, darkBrown);
        
        // Legs
        DrawRect(pixels, 8, 24, 6, 8, darkBrown);
        DrawRect(pixels, 18, 24, 6, 8, darkBrown);
        
        // Red tie
        DrawRect(pixels, 14, 14, 4, 8, red);
        
        texture.SetPixels(pixels);
        texture.filterMode = filterMode;
        texture.Apply();
        
        return Sprite.Create(texture, new Rect(0, 0, spriteWidth, spriteHeight), Vector2.one * 0.5f, spriteWidth);
    }
    
    public Sprite CreatePrincessSprite()
    {
        Texture2D texture = new Texture2D(spriteWidth, spriteHeight);
        Color[] pixels = new Color[spriteWidth * spriteHeight];
        
        // Initialize with transparent pixels
        for (int i = 0; i < pixels.Length; i++)
        {
            pixels[i] = Color.clear;
        }
        
        // Princess colors
        Color pink = new Color(1f, 0.7f, 0.8f);
        Color darkPink = new Color(0.9f, 0.4f, 0.6f);
        Color yellow = new Color(1f, 1f, 0.2f);      // Hair
        Color skin = new Color(1f, 0.8f, 0.6f);
        Color blue = new Color(0.3f, 0.3f, 1f);      // Eyes
        Color black = Color.black;
        Color white = Color.white;
        
        // Hair (blonde)
        DrawRect(pixels, 6, 2, 20, 8, yellow);
        
        // Crown
        DrawRect(pixels, 8, 2, 16, 3, yellow);
        // Crown jewels
        SetPixel(pixels, 12, 3, pink);
        SetPixel(pixels, 16, 3, pink);
        SetPixel(pixels, 20, 3, pink);
        
        // Head
        DrawRect(pixels, 8, 6, 16, 10, skin);
        
        // Eyes
        DrawRect(pixels, 11, 12, 2, 2, white);
        DrawRect(pixels, 19, 12, 2, 2, white);
        SetPixel(pixels, 12, 13, blue);
        SetPixel(pixels, 20, 13, blue);
        
        // Nose
        SetPixel(pixels, 16, 10, skin);
        
        // Mouth (small smile)
        DrawRect(pixels, 15, 8, 2, 1, darkPink);
        
        // Dress (pink)
        DrawRect(pixels, 6, 16, 20, 12, pink);
        
        // Dress details
        DrawRect(pixels, 8, 18, 16, 2, darkPink);
        
        // Arms
        DrawRect(pixels, 4, 18, 2, 6, skin);
        DrawRect(pixels, 26, 18, 2, 6, skin);
        
        // Hands
        SetPixel(pixels, 3, 22, skin);
        SetPixel(pixels, 28, 22, skin);
        
        texture.SetPixels(pixels);
        texture.filterMode = filterMode;
        texture.Apply();
        
        return Sprite.Create(texture, new Rect(0, 0, spriteWidth, spriteHeight), Vector2.one * 0.5f, spriteWidth);
    }
    
    public Sprite CreateBarrelSprite()
    {
        Texture2D texture = new Texture2D(spriteWidth, spriteHeight);
        Color[] pixels = new Color[spriteWidth * spriteHeight];
        
        // Initialize with transparent pixels
        for (int i = 0; i < pixels.Length; i++)
        {
            pixels[i] = Color.clear;
        }
        
        // Barrel colors
        Color brown = new Color(0.6f, 0.3f, 0.1f);
        Color darkBrown = new Color(0.4f, 0.2f, 0.05f);
        Color lightBrown = new Color(0.8f, 0.5f, 0.2f);
        
        // Barrel body
        DrawRect(pixels, 4, 8, 24, 16, brown);
        
        // Barrel bands
        DrawRect(pixels, 4, 10, 24, 2, darkBrown);
        DrawRect(pixels, 4, 20, 24, 2, darkBrown);
        
        // Barrel highlights
        DrawRect(pixels, 6, 12, 2, 8, lightBrown);
        
        // Barrel shadow
        DrawRect(pixels, 24, 14, 2, 6, darkBrown);
        
        texture.SetPixels(pixels);
        texture.filterMode = filterMode;
        texture.Apply();
        
        return Sprite.Create(texture, new Rect(0, 0, spriteWidth, spriteHeight), Vector2.one * 0.5f, spriteWidth);
    }
    
    // Helper methods
    void DrawRect(Color[] pixels, int x, int y, int width, int height, Color color)
    {
        for (int px = x; px < x + width && px < spriteWidth; px++)
        {
            for (int py = y; py < y + height && py < spriteHeight; py++)
            {
                SetPixel(pixels, px, py, color);
            }
        }
    }
    
    void SetPixel(Color[] pixels, int x, int y, Color color)
    {
        if (x >= 0 && x < spriteWidth && y >= 0 && y < spriteHeight)
        {
            pixels[y * spriteWidth + x] = color;
        }
    }
}
