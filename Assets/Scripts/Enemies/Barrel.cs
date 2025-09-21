using UnityEngine;

public class Barrel : MonoBehaviour
{
    [Header("Movement Settings")]
    public float rollSpeed = 3f;
    public float bounceForce = 5f;
    public float gravityScale = 1f;
    
    [Header("Points")]
    public int pointsValue = 100;
    
    [Header("Audio")]
    public AudioClip rollSound;
    public AudioClip bounceSound;
    
    [Header("Effects")]
    public GameObject explosionEffect;
    
    private Rigidbody2D rb;
    private AudioSource audioSource;
    private bool isRolling = true;
    private bool hasBeenDestroyed = false;
    
    // Direction of rolling (-1 for left, 1 for right)
    private float rollDirection = -1f;
    
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        
        // Set initial physics properties
        rb.gravityScale = gravityScale;
        
        // Start rolling sound
        if (rollSound != null && audioSource != null)
        {
            audioSource.clip = rollSound;
            audioSource.loop = true;
            audioSource.Play();
        }
    }
    
    void Start()
    {
        // Set initial velocity
        rb.velocity = new Vector2(rollDirection * rollSpeed, rb.velocity.y);
        
        // Auto-destroy after a certain time to prevent memory leaks
        Destroy(gameObject, 30f);
    }
    
    void FixedUpdate()
    {
        if (isRolling && !hasBeenDestroyed)
        {
            // Maintain rolling speed
            float currentVelocityX = rb.velocity.x;
            if (Mathf.Abs(currentVelocityX) < rollSpeed * 0.5f)
            {
                rb.velocity = new Vector2(rollDirection * rollSpeed, rb.velocity.y);
            }
            
            // Add rotation for visual effect
            transform.Rotate(0, 0, -rollDirection * rollSpeed * 50f * Time.fixedDeltaTime);
        }
    }
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (hasBeenDestroyed) return;
        
        // Check what we hit
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Platform"))
        {
            HandleGroundCollision(collision);
        }
        else if (collision.gameObject.CompareTag("Wall"))
        {
            HandleWallCollision();
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            HandlePlayerCollision(collision.gameObject);
        }
        else if (collision.gameObject.CompareTag("Hammer"))
        {
            HandleHammerHit();
        }
    }
    
    void HandleGroundCollision(Collision2D collision)
    {
        // Play bounce sound
        if (bounceSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(bounceSound);
        }
        
        // Check if we're hitting a slope or platform edge
        Vector2 normal = collision.contacts[0].normal;
        
        // If hitting a steep slope, change direction
        if (Mathf.Abs(normal.x) > 0.3f)
        {
            rollDirection = Mathf.Sign(normal.x);
            rb.velocity = new Vector2(rollDirection * rollSpeed, rb.velocity.y);
        }
        
        // Add some bounce if hitting from above
        if (normal.y > 0.7f && rb.velocity.y < 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, bounceForce);
        }
    }
    
    void HandleWallCollision()
    {
        // Reverse direction when hitting a wall
        rollDirection *= -1f;
        rb.velocity = new Vector2(rollDirection * rollSpeed, rb.velocity.y);
        
        if (bounceSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(bounceSound);
        }
    }
    
    void HandlePlayerCollision(GameObject player)
    {
        // Check if player has hammer
        PlayerController playerController = player.GetComponent<PlayerController>();
        if (playerController != null)
        {
            // Player gets hit by barrel - handle in GameManager
            GameManager.Instance?.PlayerHitByBarrel();
        }
    }
    
    void HandleHammerHit()
    {
        // Player hit barrel with hammer
        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddScore(pointsValue);
            GameManager.Instance.BarrelDestroyed();
        }
        
        DestroyBarrel();
    }
    
    public void DestroyBarrel()
    {
        if (hasBeenDestroyed) return;
        
        hasBeenDestroyed = true;
        
        // Stop sounds
        if (audioSource != null)
        {
            audioSource.Stop();
        }
        
        // Create explosion effect
        if (explosionEffect != null)
        {
            GameObject explosion = Instantiate(explosionEffect, transform.position, Quaternion.identity);
            Destroy(explosion, 2f);
        }
        
        // Destroy the barrel
        Destroy(gameObject);
    }
    
    // Method to set initial direction
    public void SetRollDirection(float direction)
    {
        rollDirection = Mathf.Sign(direction);
        if (rb != null)
        {
            rb.velocity = new Vector2(rollDirection * rollSpeed, rb.velocity.y);
        }
    }
    
    // Method to set roll speed
    public void SetRollSpeed(float speed)
    {
        rollSpeed = speed;
        if (rb != null)
        {
            rb.velocity = new Vector2(rollDirection * rollSpeed, rb.velocity.y);
        }
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        // Handle trigger collisions (like falling off platforms)
        if (other.CompareTag("DeathZone"))
        {
            DestroyBarrel();
        }
    }
}
