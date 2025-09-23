using UnityEngine;

public class CharacterGraphics : MonoBehaviour
{
    [Header("Character Sprites")]
    public Sprite marioSprite;
    public Sprite donkeyKongSprite;
    public Sprite princessSprite;
    public Sprite barrelSprite;
    
    private SpriteGenerator spriteGenerator;
    
    void Awake()
    {
        // Create sprite generator
        GameObject generatorObj = new GameObject("SpriteGenerator");
        spriteGenerator = generatorObj.AddComponent<SpriteGenerator>();
        DontDestroyOnLoad(generatorObj);
        
        GenerateAllSprites();
    }
    
    void GenerateAllSprites()
    {
        Debug.Log("Generating character sprites...");
        
        // Generate all character sprites
        marioSprite = spriteGenerator.CreateMarioSprite();
        donkeyKongSprite = spriteGenerator.CreateDonkeyKongSprite();
        princessSprite = spriteGenerator.CreatePrincessSprite();
        barrelSprite = spriteGenerator.CreateBarrelSprite();
        
        Debug.Log("✓ All character sprites generated!");
    }
    
    public void ApplyMarioGraphics(GameObject playerObj)
    {
        SpriteRenderer spriteRenderer = playerObj.GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            spriteRenderer = playerObj.AddComponent<SpriteRenderer>();
        }
        
        spriteRenderer.sprite = marioSprite;
        spriteRenderer.sortingOrder = 1;
        
        // Hide the 3D mesh renderer
        MeshRenderer meshRenderer = playerObj.GetComponent<MeshRenderer>();
        if (meshRenderer != null)
        {
            meshRenderer.enabled = false;
        }
        
        Debug.Log("✓ Mario graphics applied");
    }
    
    public void ApplyDonkeyKongGraphics(GameObject dkObj)
    {
        SpriteRenderer spriteRenderer = dkObj.GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            spriteRenderer = dkObj.AddComponent<SpriteRenderer>();
        }
        
        spriteRenderer.sprite = donkeyKongSprite;
        spriteRenderer.sortingOrder = 1;
        
        // Make DK bigger
        dkObj.transform.localScale = new Vector3(2f, 2f, 1f);
        
        // Hide the 3D mesh renderer
        MeshRenderer meshRenderer = dkObj.GetComponent<MeshRenderer>();
        if (meshRenderer != null)
        {
            meshRenderer.enabled = false;
        }
        
        Debug.Log("✓ Donkey Kong graphics applied");
    }
    
    public void ApplyPrincessGraphics(GameObject princessObj)
    {
        SpriteRenderer spriteRenderer = princessObj.GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            spriteRenderer = princessObj.AddComponent<SpriteRenderer>();
        }
        
        spriteRenderer.sprite = princessSprite;
        spriteRenderer.sortingOrder = 1;
        
        // Hide the 3D mesh renderer
        MeshRenderer meshRenderer = princessObj.GetComponent<MeshRenderer>();
        if (meshRenderer != null)
        {
            meshRenderer.enabled = false;
        }
        
        Debug.Log("✓ Princess graphics applied");
    }
    
    public void ApplyBarrelGraphics(GameObject barrelObj)
    {
        SpriteRenderer spriteRenderer = barrelObj.GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            spriteRenderer = barrelObj.AddComponent<SpriteRenderer>();
        }
        
        spriteRenderer.sprite = barrelSprite;
        spriteRenderer.sortingOrder = 0;
        
        // Hide the 3D mesh renderer
        MeshRenderer meshRenderer = barrelObj.GetComponent<MeshRenderer>();
        if (meshRenderer != null)
        {
            meshRenderer.enabled = false;
        }
        
        Debug.Log("✓ Barrel graphics applied");
    }
    
    // Static method for easy access
    public static CharacterGraphics Instance { get; private set; }
    
    void Start()
    {
        Instance = this;
    }
}
