using UnityEngine;

public class SimplePlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 12f;
    
    [Header("Ground Check")]
    public float groundCheckDistance = 0.1f;
    public LayerMask groundLayerMask = -1; // All layers
    
    // Private variables
    private Rigidbody2D rb;
    private bool isGrounded;
    private float horizontalInput;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
        }
        
        rb.freezeRotation = true;
        
        Debug.Log("SimplePlayerController initialized for " + gameObject.name);
    }
    
    void Update()
    {
        HandleInput();
        CheckGroundStatus();
        Move();
    }
    
    void HandleInput()
    {
        // Get horizontal input
        horizontalInput = 0f;
        
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            horizontalInput = -1f;
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            horizontalInput = 1f;
        }
        
        // Handle jumping
        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space)) && isGrounded)
        {
            Jump();
        }
    }
    
    void CheckGroundStatus()
    {
        // Simple ground check using raycast downward
        Vector2 rayOrigin = transform.position;
        Vector2 rayDirection = Vector2.down;
        
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, rayDirection, groundCheckDistance, groundLayerMask);
        isGrounded = hit.collider != null;
        
        // Debug visualization
        Debug.DrawRay(rayOrigin, rayDirection * groundCheckDistance, isGrounded ? Color.green : Color.red);
    }
    
    void Move()
    {
        // Apply horizontal movement
        if (horizontalInput != 0)
        {
            rb.velocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);
            
            // Flip sprite direction
            if (horizontalInput > 0)
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            else
                transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else
        {
            // Stop horizontal movement when no input
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
    }
    
    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        Debug.Log("Player jumped!");
    }
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        // Handle collision with barrels
        if (collision.gameObject.CompareTag("Barrel"))
        {
            Debug.Log("Player hit by barrel!");
            
            // Find GameManager and handle player hit
            GameManager gm = FindObjectOfType<GameManager>();
            if (gm != null)
            {
                gm.PlayerHitByBarrel();
            }
        }
    }
    
    void OnDrawGizmosSelected()
    {
        // Draw ground check distance in editor
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * groundCheckDistance);
    }
}
