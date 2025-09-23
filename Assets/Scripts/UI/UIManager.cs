using UnityEngine;
using System.Collections;

public class UIManager : MonoBehaviour
{
    [Header("UI Settings")]
    public bool showDebugUI = true;
    public bool showGameUI = true;
    public float messageDisplayTime = 3f;
    public Color scorePopupColor = Color.yellow;
    
    [Header("Screen Layout")]
    public Rect scoreRect = new Rect(10, 10, 200, 50);
    public Rect livesRect = new Rect(10, 70, 200, 50);
    public Rect levelRect = new Rect(10, 130, 200, 50);
    public Rect messageRect = new Rect(Screen.width/2 - 200, Screen.height/2 - 100, 400, 200);
    
    // Game state
    private int currentScore = 0;
    private int currentLives = 3;
    private int currentLevel = 1;
    private string currentMessage = "";
    private float messageTimer = 0f;
    private bool showMessage = false;
    private bool isGamePaused = false;
    private bool isGameOver = false;
    
    // GUI styles
    private GUIStyle scoreStyle;
    private GUIStyle messageStyle;
    private GUIStyle buttonStyle;
    
    void Start()
    {
        SetupGUIStyles();
    }
    
    void SetupGUIStyles()
    {
        scoreStyle = new GUIStyle();
        scoreStyle.fontSize = 24;
        scoreStyle.fontStyle = FontStyle.Bold;
        scoreStyle.normal.textColor = Color.white;
        
        messageStyle = new GUIStyle();
        messageStyle.fontSize = 32;
        messageStyle.fontStyle = FontStyle.Bold;
        messageStyle.normal.textColor = Color.yellow;
        messageStyle.alignment = TextAnchor.MiddleCenter;
        messageStyle.wordWrap = true;
        
        buttonStyle = new GUIStyle("button");
        buttonStyle.fontSize = 20;
        buttonStyle.fontStyle = FontStyle.Bold;
    }
    
    void Update()
    {
        // Handle message timer
        if (showMessage && messageTimer > 0f)
        {
            messageTimer -= Time.deltaTime;
            if (messageTimer <= 0f)
            {
                showMessage = false;
                currentMessage = "";
            }
        }
        
        // Handle pause input
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }
    
    void OnGUI()
    {
        if (!showGameUI) return;
        
        // Draw game UI
        DrawGameUI();
        
        // Draw messages
        if (showMessage && !string.IsNullOrEmpty(currentMessage))
        {
            DrawMessage();
        }
        
        // Draw pause menu
        if (isGamePaused)
        {
            DrawPauseMenu();
        }
        
        // Draw game over screen
        if (isGameOver)
        {
            DrawGameOverScreen();
        }
    }
    
    void DrawGameUI()
    {
        // Draw score
        GUI.Label(scoreRect, $"SCORE: {currentScore:000000}", scoreStyle);
        
        // Draw lives
        GUI.Label(livesRect, $"LIVES: {currentLives}", scoreStyle);
        
        // Draw level
        GUI.Label(levelRect, $"LEVEL: {currentLevel}", scoreStyle);
    }
    
    void DrawMessage()
    {
        // Draw background box
        GUI.Box(messageRect, "");
        
        // Draw message text
        GUI.Label(messageRect, currentMessage, messageStyle);
    }
    
    void DrawPauseMenu()
    {
        // Draw pause background
        GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "");
        
        // Draw pause menu
        Rect pauseRect = new Rect(Screen.width/2 - 150, Screen.height/2 - 100, 300, 200);
        GUI.Box(pauseRect, "GAME PAUSED");
        
        // Resume button
        if (GUI.Button(new Rect(Screen.width/2 - 75, Screen.height/2 - 50, 150, 50), "RESUME", buttonStyle))
        {
            Resume();
        }
        
        // Restart button
        if (GUI.Button(new Rect(Screen.width/2 - 75, Screen.height/2, 150, 50), "RESTART", buttonStyle))
        {
            RestartGame();
        }
    }
    
    void DrawGameOverScreen()
    {
        // Draw game over background
        GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "");
        
        // Draw game over panel
        Rect gameOverRect = new Rect(Screen.width/2 - 200, Screen.height/2 - 150, 400, 300);
        GUI.Box(gameOverRect, "");
        
        // Game over text
        GUI.Label(new Rect(Screen.width/2 - 100, Screen.height/2 - 120, 200, 50), "GAME OVER", messageStyle);
        
        // Final score
        GUI.Label(new Rect(Screen.width/2 - 100, Screen.height/2 - 70, 200, 30), $"Final Score: {currentScore}", scoreStyle);
        
        // Play again button
        if (GUI.Button(new Rect(Screen.width/2 - 75, Screen.height/2 - 20, 150, 50), "PLAY AGAIN", buttonStyle))
        {
            RestartGame();
        }
    }
    
    // Public methods called by GameManager
    public void UpdateScore(int newScore)
    {
        currentScore = newScore;
    }
    
    public void UpdateLives(int newLives)
    {
        currentLives = newLives;
    }
    
    public void UpdateLevel(int newLevel)
    {
        currentLevel = newLevel;
    }
    
    public void ShowMessage(string message, float duration = 3f)
    {
        currentMessage = message;
        messageTimer = duration;
        showMessage = true;
    }
    
    public void ShowGameUI()
    {
        showGameUI = true;
    }
    
    public void ShowScorePopup(int points, Vector3 worldPosition)
    {
        // Simple popup implementation
        ShowMessage($"+{points}", 1f);
    }
    
    public void ShowHammerTimer(float duration)
    {
        // Show hammer timer - could implement a visual timer here
        ShowMessage("HAMMER TIME!", 1f);
    }
    
    public void UpdateHammerTimer(float timeRemaining)
    {
        // Update hammer timer display
        // For now, we'll just use debug
        if (showDebugUI)
        {
            Debug.Log($"Hammer time remaining: {timeRemaining:F1}");
        }
    }
    
    public void HideHammerTimer()
    {
        // Hide hammer timer
        // For now, just clear any hammer-related messages
    }
    
    public void ShowGameOver(int finalScore, int finalLevel, float gameTime)
    {
        isGameOver = true;
        isGamePaused = false;
        currentScore = finalScore;
        currentLevel = finalLevel;
        ShowMessage($"GAME OVER!\nScore: {finalScore}\nLevel: {finalLevel}\nTime: {gameTime:F1}s", 10f);
    }
    
    public void ShowPauseMenu()
    {
        isGamePaused = true;
        Time.timeScale = 0f;
    }
    
    public void HidePauseMenu()
    {
        isGamePaused = false;
        Time.timeScale = 1f;
    }
    
    public void TogglePause()
    {
        if (isGameOver) return;
        
        isGamePaused = !isGamePaused;
        Time.timeScale = isGamePaused ? 0f : 1f;
    }
    
    public void Resume()
    {
        isGamePaused = false;
        Time.timeScale = 1f;
    }
    
    public void RestartGame()
    {
        isGamePaused = false;
        isGameOver = false;
        Time.timeScale = 1f;
        
        // Find and restart game manager
        GameManager gameManager = FindObjectOfType<GameManager>();
        if (gameManager != null)
        {
            gameManager.StartNewGame();
        }
    }
}