using UnityEngine;
using UnityEngine.UI;

public class TouchControls : MonoBehaviour
{
    [Header("UI References")]
    public Button leftButton;
    public Button rightButton;
    public Button jumpButton;
    public Button upButton;
    public Button downButton;
    
    [Header("Touch Settings")]
    public float touchSensitivity = 1f;
    public bool useVirtualButtons = true;
    public bool useTouchGestures = true;
    
    // Input states
    private Vector2 movementInput;
    private bool jumpPressed;
    private bool jumpInput;
    
    // Touch gesture areas
    private Rect leftTouchArea;
    private Rect rightTouchArea;
    private Rect jumpTouchArea;
    
    void Start()
    {
        SetupTouchAreas();
        CreateVirtualButtons();
    }
    
    void Update()
    {
        HandleTouchInput();
        HandleVirtualButtons();
        
        // Reset jump input after frame
        jumpPressed = false;
    }
    
    void SetupTouchAreas()
    {
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;
        
        // Left third of screen for movement
        leftTouchArea = new Rect(0, 0, screenWidth / 3f, screenHeight);
        
        // Middle third for vertical movement (ladders)
        rightTouchArea = new Rect(screenWidth / 3f, 0, screenWidth / 3f, screenHeight);
        
        // Right third for jumping
        jumpTouchArea = new Rect(2 * screenWidth / 3f, 0, screenWidth / 3f, screenHeight);
    }
    
    void HandleTouchInput()
    {
        if (!useTouchGestures) return;
        
        movementInput = Vector2.zero;
        
        for (int i = 0; i < Input.touchCount; i++)
        {
            Touch touch = Input.GetTouch(i);
            Vector2 touchPos = touch.position;
            
            if (leftTouchArea.Contains(touchPos))
            {
                // Handle horizontal movement
                if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
                {
                    float deltaX = touch.deltaPosition.x * touchSensitivity;
                    if (Mathf.Abs(deltaX) > 5f) // Minimum movement threshold
                    {
                        movementInput.x = Mathf.Sign(deltaX);
                    }
                }
            }
            else if (rightTouchArea.Contains(touchPos))
            {
                // Handle vertical movement (ladders)
                if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
                {
                    float deltaY = touch.deltaPosition.y * touchSensitivity;
                    if (Mathf.Abs(deltaY) > 5f) // Minimum movement threshold
                    {
                        movementInput.y = Mathf.Sign(deltaY);
                    }
                }
            }
            else if (jumpTouchArea.Contains(touchPos))
            {
                // Handle jumping
                if (touch.phase == TouchPhase.Began)
                {
                    jumpPressed = true;
                }
            }
        }
        
        // Also handle single tap for simple controls
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                Vector2 touchPos = touch.position;
                float screenCenterX = Screen.width / 2f;
                
                if (touchPos.x < screenCenterX / 2f)
                {
                    movementInput.x = -1f; // Move left
                }
                else if (touchPos.x > screenCenterX * 1.5f)
                {
                    jumpPressed = true; // Jump
                }
                else if (touchPos.x > screenCenterX / 2f && touchPos.x < screenCenterX * 1.5f)
                {
                    movementInput.x = 1f; // Move right
                }
            }
        }
    }
    
    void HandleVirtualButtons()
    {
        if (!useVirtualButtons) return;
        
        // Reset movement
        movementInput = Vector2.zero;
        
        // Check virtual button states
        if (leftButton != null && IsButtonPressed(leftButton))
        {
            movementInput.x = -1f;
        }
        
        if (rightButton != null && IsButtonPressed(rightButton))
        {
            movementInput.x = 1f;
        }
        
        if (upButton != null && IsButtonPressed(upButton))
        {
            movementInput.y = 1f;
        }
        
        if (downButton != null && IsButtonPressed(downButton))
        {
            movementInput.y = -1f;
        }
        
        if (jumpButton != null && WasButtonPressed(jumpButton))
        {
            jumpPressed = true;
        }
    }
    
    bool IsButtonPressed(Button button)
    {
        // This would need to be implemented with proper UI button handling
        // For now, we'll use a simple approach
        return false; // Placeholder
    }
    
    bool WasButtonPressed(Button button)
    {
        // This would need to be implemented with proper UI button handling
        // For now, we'll use a simple approach
        return false; // Placeholder
    }
    
    void CreateVirtualButtons()
    {
        if (!useVirtualButtons) return;
        
        // Create Canvas for UI
        GameObject canvasObj = new GameObject("TouchControlsCanvas");
        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 100;
        
        CanvasScaler scaler = canvasObj.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        
        canvasObj.AddComponent<GraphicRaycaster>();
        
        // Create movement buttons
        CreateButton("LeftButton", canvas.transform, new Vector2(-700, -350), "←", () => movementInput.x = -1f);
        CreateButton("RightButton", canvas.transform, new Vector2(-500, -350), "→", () => movementInput.x = 1f);
        CreateButton("UpButton", canvas.transform, new Vector2(-600, -250), "↑", () => movementInput.y = 1f);
        CreateButton("DownButton", canvas.transform, new Vector2(-600, -450), "↓", () => movementInput.y = -1f);
        
        // Create jump button
        CreateButton("JumpButton", canvas.transform, new Vector2(600, -350), "JUMP", () => jumpPressed = true);
    }
    
    void CreateButton(string name, Transform parent, Vector2 position, string text, System.Action onPress)
    {
        GameObject buttonObj = new GameObject(name);
        buttonObj.transform.SetParent(parent);
        
        RectTransform rect = buttonObj.AddComponent<RectTransform>();
        rect.anchoredPosition = position;
        rect.sizeDelta = new Vector2(150, 100);
        
        Image image = buttonObj.AddComponent<Image>();
        image.color = new Color(1f, 1f, 1f, 0.7f);
        
        Button button = buttonObj.AddComponent<Button>();
        
        // Add text
        GameObject textObj = new GameObject("Text");
        textObj.transform.SetParent(buttonObj.transform);
        
        RectTransform textRect = textObj.AddComponent<RectTransform>();
        textRect.anchoredPosition = Vector2.zero;
        textRect.sizeDelta = rect.sizeDelta;
        
        Text buttonText = textObj.AddComponent<Text>();
        buttonText.text = text;
        buttonText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        buttonText.fontSize = 24;
        buttonText.color = Color.black;
        buttonText.alignment = TextAnchor.MiddleCenter;
        
        // Store button references
        switch (name)
        {
            case "LeftButton": leftButton = button; break;
            case "RightButton": rightButton = button; break;
            case "UpButton": upButton = button; break;
            case "DownButton": downButton = button; break;
            case "JumpButton": jumpButton = button; break;
        }
    }
    
    // Public methods for other scripts to access input
    public Vector2 GetMovementInput()
    {
        return movementInput;
    }
    
    public bool GetJumpInput()
    {
        bool result = jumpPressed;
        return result;
    }
    
    public void SetMovementInput(Vector2 input)
    {
        movementInput = input;
    }
    
    public void SetJumpInput(bool pressed)
    {
        jumpPressed = pressed;
    }
}
