using UnityEngine;
using System.Collections;

public class SimpleBarrelSpawner : MonoBehaviour
{
    [Header("Spawning")]
    public float spawnInterval = 3f;
    public Vector3 spawnOffset = new Vector3(0.5f, 2f, 0); // Spawn well above the spawner position
    
    [Header("Barrel Physics")]
    public float initialForce = 5f;
    public float torque = 5f;
    
    private bool isSpawning = false;
    
    void Start()
    {
        Debug.Log("SimpleBarrelSpawner starting barrel production...");
        
        // Don't auto-start if there's an intro sequence
        GameIntroSequence intro = FindObjectOfType<GameIntroSequence>();
        if (intro == null)
        {
            StartCoroutine(SpawnBarrels());
        }
        else
        {
            Debug.Log("Waiting for intro sequence to complete...");
        }
    }
    
    IEnumerator SpawnBarrels()
    {
        yield return new WaitForSeconds(2f); // Wait 2 seconds before first barrel
        
        isSpawning = true;
        
        while (isSpawning)
        {
            SpawnBarrel();
            yield return new WaitForSeconds(spawnInterval);
        }
    }
    
    void SpawnBarrel()
    {
        Debug.Log("🛢️ Spawning barrel from above DonkeyKong...");
        
        // Create barrel
        GameObject barrel = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        barrel.name = "Barrel";
        
        // Spawn from position of this spawner (which is positioned above DK)
        Vector3 spawnPosition = transform.position;
        barrel.transform.position = spawnPosition;
        
        barrel.transform.localScale = new Vector3(0.6f, 0.3f, 0.6f);
        
        Debug.Log($"🛢️ Barrel spawned at: {barrel.transform.position}");
        
        // Make it brown like a wooden barrel
        Renderer renderer = barrel.GetComponent<Renderer>();
        Material material = new Material(Shader.Find("Standard"));
        material.color = new Color(0.5f, 0.25f, 0.1f);
        material.SetFloat("_Metallic", 0);
        material.SetFloat("_Glossiness", 0.1f);
        renderer.material = material;
        
        // Convert to 2D collider FIRST
        UnityEngine.Object.DestroyImmediate(barrel.GetComponent<CapsuleCollider>());
        CircleCollider2D collider = barrel.AddComponent<CircleCollider2D>();
        
        // Add physics
        Rigidbody2D rb = barrel.AddComponent<Rigidbody2D>();
        rb.gravityScale = 1f;
        
        // Add barrel behavior
        barrel.AddComponent<SimpleBarrel>();
        
        // Apply initial force (roll down)
        rb.AddForce(Vector2.right * initialForce, ForceMode2D.Impulse);
        rb.AddTorque(torque, ForceMode2D.Impulse);
        
        barrel.tag = "Barrel";
        
        Debug.Log($"✅ Barrel physics applied - should roll down from: {barrel.transform.position}");
    }
    
    public void StopSpawning()
    {
        isSpawning = false;
    }
    
    public void StartSpawning()
    {
        if (!isSpawning)
        {
            StartCoroutine(SpawnBarrels());
        }
    }
}
