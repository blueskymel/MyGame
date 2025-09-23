using UnityEngine;

public class TouchControls : MonoBehaviour
{
    [Header("Touch Input Settings")]
    public bool enableTouchControls = true;
    public float touchSensitivity = 1f;
    
    [Header("Button Positions")]
    public Rect leftButtonRect = new Rect(50, 50, 100, 100);
    public Rect rightButtonRect = new Rect(200, 50, 100, 100);
    public Rect jumpButtonRect = new Rect(Screen.width - 150, 50, 100, 100);
    public Rect upButtonRect = new Rect(125, 200, 100, 100);
    public Rect downButtonRect = new Rect(125, 50, 100, 100);
    
    // Input states
    private bool leftPressed = false;
    private bool rightPressed = false;
    private bool jumpPressed = false;
    private bool upPressed = false;
    private bool downPressed = false;
    
    void Start()
    {
        // Adjust button positions for different screen sizes
        AdjustButtonPositions();
    }
    
    void Update()
    {
        if (!enableTouchControls)
            return;
            
        HandleTouchInput();
        HandleKeyboardInput();
    }
    
    void HandleTouchInput()
    {
        // Reset button states
        leftPressed = false;
        rightPressed = false;
        jumpPressed = false;
        upPressed = false;
        downPressed = false;
        
        #if UNITY_ANDROID || UNITY_IOS || UNITY_EDITOR
        // Handle multiple touches
        for (int i = 0; i < Input.touchCount; i++)
        {
            Touch touch = Input.GetTouch(i);
            Vector2 touchPos = touch.position;
            
            // Convert screen position to GUI coordinates
            touchPos.y = Screen.height - touchPos.y;
            
            if (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Stationary)
            {
                CheckButtonPress(touchPos);
            }
        }
        
        // Handle mouse input for testing in editor
        #if UNITY_EDITOR
        if (Input.GetMouseButton(0))
        {
            Vector2 mousePos = Input.mousePosition;
            mousePos.y = Screen.height - mousePos.y;
            CheckButtonPress(mousePos);
        }
        #endif
        #endif
    }
    
    void CheckButtonPress(Vector2 position)
    {
        if (leftButtonRect.Contains(position))
            leftPressed = true;
        else if (rightButtonRect.Contains(position))
            rightPressed = true;
        else if (jumpButtonRect.Contains(position))
            jumpPressed = true;
        else if (upButtonRect.Contains(position))
            upPressed = true;
        else if (downButtonRect.Contains(position))
            downPressed = true;
    }
    
    void HandleKeyboardInput()
    {
        // Fallback keyboard controls
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            leftPressed = true;
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            rightPressed = true;
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            jumpPressed = true;
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
            upPressed = true;
        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
            downPressed = true;
    }
    
    void AdjustButtonPositions()
    {
        // Adjust button positions based on screen size
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;
        
        // Scale buttons for different screen sizes
        float scale = Mathf.Min(screenWidth / 1920f, screenHeight / 1080f);
        
        leftButtonRect = ScaleRect(leftButtonRect, scale);
        rightButtonRect = ScaleRect(rightButtonRect, scale);
        jumpButtonRect = new Rect(screenWidth - 150 * scale, 50 * scale, 100 * scale, 100 * scale);
        upButtonRect = ScaleRect(upButtonRect, scale);
        downButtonRect = ScaleRect(downButtonRect, scale);
    }
    
    Rect ScaleRect(Rect original, float scale)
    {
        return new Rect(original.x * scale, original.y * scale, original.width * scale, original.height * scale);
    }
    
    void OnGUI()
    {
        if (!enableTouchControls)
            return;
            
        // Draw touch buttons for mobile
        #if UNITY_ANDROID || UNITY_IOS || UNITY_EDITOR
        DrawTouchButton(leftButtonRect, "←", leftPressed);
        DrawTouchButton(rightButtonRect, "→", rightPressed);
        DrawTouchButton(jumpButtonRect, "JUMP", jumpPressed);
        DrawTouchButton(upButtonRect, "↑", upPressed);
        DrawTouchButton(downButtonRect, "↓", downPressed);
        #endif
    }
    
    void DrawTouchButton(Rect rect, string label, bool isPressed)
    {
        Color originalColor = GUI.backgroundColor;
        GUI.backgroundColor = isPressed ? Color.yellow : Color.white;
        
        if (GUI.Button(rect, label))
        {
            // Button was pressed
        }
        
        GUI.backgroundColor = originalColor;
    }
    
    // Public methods for other scripts to check input
    public bool GetLeftInput()
    {
        return leftPressed;
    }
    
    public bool GetRightInput()
    {
        return rightPressed;
    }
    
    public bool GetJumpInput()
    {
        return jumpPressed;
    }
    
    public bool GetUpInput()
    {
        return upPressed;
    }
    
    public bool GetDownInput()
    {
        return downPressed;
    }
    
    public Vector2 GetMovementInput()
    {
        Vector2 movement = Vector2.zero;
        
        if (leftPressed)
            movement.x -= 1f;
        if (rightPressed)
            movement.x += 1f;
        if (upPressed)
            movement.y += 1f;
        if (downPressed)
            movement.y -= 1f;
            
        return movement;
    }
    
    // Enable/disable touch controls
    public void SetTouchControlsEnabled(bool enabled)
    {
        enableTouchControls = enabled;
    }
}