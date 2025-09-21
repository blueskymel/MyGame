using UnityEngine;

public class Hammer : MonoBehaviour
{
    [Header("Hammer Settings")]
    public float swingDamage = 100f;
    public float swingRange = 2f;
    public float swingCooldown = 0.5f;
    public LayerMask enemyLayerMask;
    
    [Header("Audio")]
    public AudioClip swingSound;
    public AudioClip hitSound;
    
    private bool canSwing = true;
    private float swingTimer = 0f;
    private AudioSource audioSource;
    private Animator animator;
    
    // Animation hashes
    private int swingHash;
    
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        
        swingHash = Animator.StringToHash("Swing");
    }
    
    void Update()
    {
        HandleSwingCooldown();
        HandleInput();
    }
    
    void HandleSwingCooldown()
    {
        if (!canSwing)
        {
            swingTimer -= Time.deltaTime;
            if (swingTimer <= 0f)
            {
                canSwing = true;
            }
        }
    }
    
    void HandleInput()
    {
        // Check for hammer swing input
        bool swingInput = false;
        
        // Get input from touch controls or keyboard
        TouchControls touchControls = FindObjectOfType<TouchControls>();
        if (touchControls != null)
        {
            // Add hammer swing to touch controls (could be a separate button or gesture)
            swingInput = Input.GetButtonDown("Fire1"); // Temporary fallback
        }
        else
        {
            swingInput = Input.GetButtonDown("Fire1") || Input.GetKeyDown(KeyCode.Space);
        }
        
        if (swingInput && canSwing && GameManager.Instance.PlayerHasHammer())
        {
            SwingHammer();
        }
    }
    
    public void SwingHammer()
    {
        if (!canSwing) return;
        
        canSwing = false;
        swingTimer = swingCooldown;
        
        // Play swing animation
        if (animator != null)
        {
            animator.SetTrigger(swingHash);
        }
        
        // Play swing sound
        if (swingSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(swingSound);
        }
        
        // Check for hits
        CheckForHits();
    }
    
    void CheckForHits()
    {
        Vector2 hammerPosition = transform.position;
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(hammerPosition, swingRange, enemyLayerMask);
        
        foreach (Collider2D enemy in hitEnemies)
        {
            HitEnemy(enemy);
        }
    }
    
    void HitEnemy(Collider2D enemy)
    {
        // Play hit sound
        if (hitSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(hitSound);
        }
        
        // Handle different enemy types
        if (enemy.CompareTag("Barrel"))
        {
            Barrel barrel = enemy.GetComponent<Barrel>();
            if (barrel != null)
            {
                barrel.HandleHammerHit();
            }
        }
        else if (enemy.CompareTag("Enemy"))
        {
            // Handle other enemy types here
            Enemy enemyScript = enemy.GetComponent<Enemy>();
            if (enemyScript != null)
            {
                enemyScript.TakeDamage(swingDamage);
            }
        }
        
        // Create hit effect
        CreateHitEffect(enemy.transform.position);
    }
    
    void CreateHitEffect(Vector3 position)
    {
        // Create simple hit effect
        GameObject hitEffect = new GameObject("HitEffect");
        hitEffect.transform.position = position;
        
        // Add particle system or sprite effect
        ParticleSystem particles = hitEffect.AddComponent<ParticleSystem>();
        
        var main = particles.main;
        main.startLifetime = 0.5f;
        main.startSpeed = 5f;
        main.startColor = Color.yellow;
        main.startSize = 0.2f;
        main.maxParticles = 20;
        
        var emission = particles.emission;
        emission.rateOverTime = 0;
        emission.SetBursts(new ParticleSystem.Burst[]
        {
            new ParticleSystem.Burst(0.0f, 20)
        });
        
        var shape = particles.shape;
        shape.shapeType = ParticleSystemShapeType.Circle;
        shape.radius = 0.5f;
        
        // Destroy effect after a short time
        Destroy(hitEffect, 2f);
    }
    
    void OnDrawGizmosSelected()
    {
        // Draw swing range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, swingRange);
    }
}

// Base enemy class for hammer hits
public class Enemy : MonoBehaviour
{
    [Header("Enemy Settings")]
    public float health = 100f;
    public int pointsValue = 200;
    
    protected float currentHealth;
    
    protected virtual void Start()
    {
        currentHealth = health;
    }
    
    public virtual void TakeDamage(float damage)
    {
        currentHealth -= damage;
        
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    
    protected virtual void Die()
    {
        // Award points
        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddScore(pointsValue);
        }
        
        // Destroy enemy
        Destroy(gameObject);
    }
}
