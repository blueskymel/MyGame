using UnityEngine;

public class SimpleBarrel : MonoBehaviour
{
    [Header("Movement")]
    public float rollSpeed = 3f;
    public float lifetime = 10f;
    
    [Header("Bouncing")]
    public float bounceForce = 2f;
    public float minBounceVelocity = 1f;
    
    private Rigidbody2D rb;
    private bool hasCollided = false;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        // Destroy barrel after lifetime to prevent infinite accumulation
        Destroy(gameObject, lifetime);
        
        Debug.Log($"Barrel created with lifetime {lifetime}s");
    }
    
    void Update()
    {
        // Keep rolling to the right
        if (rb.velocity.x < rollSpeed)
        {
            rb.AddForce(Vector2.right * rollSpeed * 0.1f);
        }
        
        // Destroy if falls too far down
        if (transform.position.y < -10f)
        {
            Destroy(gameObject);
        }
    }
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        // Bounce when hitting platforms
        if (collision.gameObject.CompareTag("Platform"))
        {
            if (!hasCollided && rb.velocity.y < -minBounceVelocity)
            {
                rb.AddForce(Vector2.up * bounceForce, ForceMode2D.Impulse);
                hasCollided = true;
                
                // Reset collision flag after a short delay
                Invoke("ResetCollision", 0.5f);
            }
        }
        
        // Damage player on collision
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Barrel hit player!");
            
            // Find GameManager and reduce player life
            GameManager gm = FindObjectOfType<GameManager>();
            if (gm != null)
            {
                gm.PlayerHitByBarrel();
            }
            
            // Destroy barrel on player hit
            Destroy(gameObject);
        }
    }
    
    void ResetCollision()
    {
        hasCollided = false;
    }
    
    public void HandleHammerHit()
    {
        Debug.Log("Barrel destroyed by hammer!");
        
        // Award points
        GameManager gm = FindObjectOfType<GameManager>();
        if (gm != null)
        {
            gm.AddScore(100);
        }
        
        // Destroy barrel
        Destroy(gameObject);
    }
}
