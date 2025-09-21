using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 12f;
    public float climbSpeed = 3f;
    
    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayerMask;
    
    [Header("Ladder Check")]
    public Transform ladderCheck;
    public float ladderCheckRadius = 0.3f;
    public LayerMask ladderLayerMask;
    
    [Header("Audio")]
    public AudioClip jumpSound;
    public AudioClip walkSound;
    
    // Private variables
    private Rigidbody2D rb;
    private Animator animator;
    private AudioSource audioSource;
    private SpriteRenderer spriteRenderer;
    
    private bool isGrounded;
    private bool isOnLadder;
    private bool canClimb;
    private bool facingRight = true;
    
    // Mobile touch controls
    private TouchControls touchControls;
    private Vector2 moveInput;
    private bool jumpInput;
    
    // Animation hashes for performance
    private int walkingHash;
    private int jumpingHash;
    private int climbingHash;
    
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        // Cache animation hashes
        walkingHash = Animator.StringToHash("isWalking");
        jumpingHash = Animator.StringToHash("isJumping");
        climbingHash = Animator.StringToHash("isClimbing");
        
        // Initialize touch controls
        touchControls = FindObjectOfType<TouchControls>();
        if (touchControls == null)
        {
            Debug.LogWarning("TouchControls not found! Creating default touch controls.");
            CreateDefaultTouchControls();
        }
    }
    
    void Update()
    {
        CheckGroundStatus();
        CheckLadderStatus();
        HandleInput();
        HandleMovement();
        HandleJumping();
        HandleClimbing();
        UpdateAnimations();
    }
    
    void CheckGroundStatus()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayerMask);
    }
    
    void CheckLadderStatus()
    {
        canClimb = Physics2D.OverlapCircle(ladderCheck.position, ladderCheckRadius, ladderLayerMask);
        
        // If not touching ladder anymore, stop climbing
        if (!canClimb && isOnLadder)
        {
            isOnLadder = false;
            rb.gravityScale = 1f; // Restore gravity
        }
    }
    
    void HandleInput()
    {
        // Get input from mobile touch controls or keyboard for testing
        if (touchControls != null)
        {
            moveInput = touchControls.GetMovementInput();
            jumpInput = touchControls.GetJumpInput();
        }
        else
        {
            // Fallback to keyboard input for testing in editor
            moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            jumpInput = Input.GetButtonDown("Jump");
        }
    }
    
    void HandleMovement()
    {
        if (!isOnLadder)
        {
            // Horizontal movement
            rb.velocity = new Vector2(moveInput.x * moveSpeed, rb.velocity.y);
            
            // Flip sprite based on movement direction
            if (moveInput.x > 0 && !facingRight)
            {
                FlipSprite();
            }
            else if (moveInput.x < 0 && facingRight)
            {
                FlipSprite();
            }
            
            // Play walking sound
            if (Mathf.Abs(moveInput.x) > 0.1f && isGrounded && walkSound != null)
            {
                if (!audioSource.isPlaying || audioSource.clip != walkSound)
                {
                    audioSource.clip = walkSound;
                    audioSource.loop = true;
                    audioSource.Play();
                }
            }
            else
            {
                if (audioSource.isPlaying && audioSource.clip == walkSound)
                {
                    audioSource.Stop();
                }
            }
        }
    }
    
    void HandleJumping()
    {
        if (jumpInput && isGrounded && !isOnLadder)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            
            // Play jump sound
            if (jumpSound != null)
            {
                audioSource.PlayOneShot(jumpSound);
            }
        }
    }
    
    void HandleClimbing()
    {
        if (canClimb && Mathf.Abs(moveInput.y) > 0.1f)
        {
            if (!isOnLadder)
            {
                isOnLadder = true;
                rb.gravityScale = 0f; // Disable gravity while climbing
            }
            
            // Vertical movement on ladder
            rb.velocity = new Vector2(rb.velocity.x, moveInput.y * climbSpeed);
        }
        else if (isOnLadder && Mathf.Abs(moveInput.y) <= 0.1f)
        {
            // Stop vertical movement when not inputting
            rb.velocity = new Vector2(rb.velocity.x, 0f);
        }
    }
    
    void UpdateAnimations()
    {
        if (animator != null)
        {
            animator.SetBool(walkingHash, Mathf.Abs(moveInput.x) > 0.1f && isGrounded && !isOnLadder);
            animator.SetBool(jumpingHash, !isGrounded && !isOnLadder);
            animator.SetBool(climbingHash, isOnLadder);
        }
    }
    
    void FlipSprite()
    {
        facingRight = !facingRight;
        spriteRenderer.flipX = !facingRight;
    }
    
    void CreateDefaultTouchControls()
    {
        // Create a basic touch control system if none exists
        GameObject touchControlsObj = new GameObject("TouchControls");
        touchControls = touchControlsObj.AddComponent<TouchControls>();
    }
    
    // Collision detection for barrels and enemies
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Barrel") || other.CompareTag("Enemy"))
        {
            // Handle player death
            GameManager.Instance?.PlayerDied();
        }
        else if (other.CompareTag("Hammer"))
        {
            // Handle hammer pickup
            GameManager.Instance?.PlayerPickedUpHammer();
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Princess"))
        {
            // Handle reaching the princess (win condition)
            GameManager.Instance?.PlayerReachedPrincess();
        }
    }
    
    // Draw gizmos for debugging
    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = isGrounded ? Color.green : Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
        
        if (ladderCheck != null)
        {
            Gizmos.color = canClimb ? Color.blue : Color.yellow;
            Gizmos.DrawWireSphere(ladderCheck.position, ladderCheckRadius);
        }
    }
}
