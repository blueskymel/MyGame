using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIManager : MonoBehaviour
{
    [Header("UI Panels")]
    public Canvas mainCanvas;
    public GameObject gameUI;
    public GameObject pauseMenu;
    public GameObject gameOverPanel;
    public GameObject mainMenu;
    
    [Header("Game UI Elements")]
    public Text scoreText;
    public Text livesText;
    public Text levelText;
    public Text messageText;
    public Slider hammerTimer;
    public GameObject hammerTimerPanel;
    
    [Header("Pause Menu Elements")]
    public Button resumeButton;
    public Button restartButton;
    public Button mainMenuButton;
    
    [Header("Game Over Elements")]
    public Text finalScoreText;
    public Text finalLevelText;
    public Text finalTimeText;
    public Button playAgainButton;
    public Button quitButton;
    
    [Header("Touch Controls")]
    public GameObject touchControlsPanel;
    public Button leftButton;
    public Button rightButton;
    public Button jumpButton;
    public Button upButton;
    public Button downButton;
    public Button hammerButton;
    
    [Header("Settings")]
    public float messageDisplayTime = 3f;
    public Color scorePopupColor = Color.yellow;
    
    private bool isGamePaused = false;
    private Coroutine messageCoroutine;
    
    void Awake()
    {
        CreateUIIfNeeded();
        SetupButtonListeners();
    }
    
    void Start()
    {
        ShowMainMenu();
    }
    
    void CreateUIIfNeeded()
    {
        if (mainCanvas == null)
        {
            CreateMainCanvas();
        }
        
        if (gameUI == null)
        {
            CreateGameUI();
        }
        
        if (pauseMenu == null)
        {
            CreatePauseMenu();
        }
        
        if (gameOverPanel == null)
        {
            CreateGameOverPanel();
        }
        
        if (touchControlsPanel == null)
        {
            CreateTouchControls();
        }
    }
    
    void CreateMainCanvas()
    {
        GameObject canvasObj = new GameObject("MainCanvas");
        mainCanvas = canvasObj.AddComponent<Canvas>();
        mainCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
        mainCanvas.sortingOrder = 100;
        
        CanvasScaler scaler = canvasObj.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        scaler.matchWidthOrHeight = 0.5f;
        
        canvasObj.AddComponent<GraphicRaycaster>();
        
        // Add EventSystem if it doesn't exist
        if (FindObjectOfType<UnityEngine.EventSystems.EventSystem>() == null)
        {
            GameObject eventSystemObj = new GameObject("EventSystem");
            eventSystemObj.AddComponent<UnityEngine.EventSystems.EventSystem>();
            eventSystemObj.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();
        }
    }
    
    void CreateGameUI()
    {
        gameUI = new GameObject("GameUI");
        gameUI.transform.SetParent(mainCanvas.transform, false);
        
        // Score display
        GameObject scoreObj = CreateUIText("Score", new Vector2(-800, 450), "SCORE: 0", 36);
        scoreText = scoreObj.GetComponent<Text>();
        scoreText.transform.SetParent(gameUI.transform, false);
        
        // Lives display
        GameObject livesObj = CreateUIText("Lives", new Vector2(-400, 450), "LIVES: 3", 36);
        livesText = livesObj.GetComponent<Text>();
        livesText.transform.SetParent(gameUI.transform, false);
        
        // Level display
        GameObject levelObj = CreateUIText("Level", new Vector2(0, 450), "LEVEL: 1", 36);
        levelText = levelObj.GetComponent<Text>();
        levelText.transform.SetParent(gameUI.transform, false);
        
        // Message text (center screen)
        GameObject messageObj = CreateUIText("Message", Vector2.zero, "", 48);
        messageText = messageObj.GetComponent<Text>();
        messageText.color = Color.yellow;
        messageText.transform.SetParent(gameUI.transform, false);
        
        // Hammer timer
        CreateHammerTimer();
        
        // Pause button
        GameObject pauseBtnObj = CreateUIButton("PauseButton", new Vector2(800, 450), "PAUSE", 24);
        Button pauseBtn = pauseBtnObj.GetComponent<Button>();
        pauseBtn.onClick.AddListener(PauseGame);
        pauseBtnObj.transform.SetParent(gameUI.transform, false);
    }
    
    void CreateHammerTimer()
    {
        hammerTimerPanel = new GameObject("HammerTimerPanel");
        hammerTimerPanel.transform.SetParent(gameUI.transform, false);
        
        RectTransform panelRect = hammerTimerPanel.AddComponent<RectTransform>();
        panelRect.anchoredPosition = new Vector2(400, 450);
        panelRect.sizeDelta = new Vector2(200, 50);
        
        // Background
        Image panelBg = hammerTimerPanel.AddComponent<Image>();
        panelBg.color = new Color(0, 0, 0, 0.5f);
        
        // Hammer icon
        GameObject hammerIcon = CreateUIText("HammerIcon", new Vector2(-75, 0), "🔨", 30);
        hammerIcon.transform.SetParent(hammerTimerPanel.transform, false);
        
        // Timer slider
        GameObject sliderObj = new GameObject("HammerSlider");
        sliderObj.transform.SetParent(hammerTimerPanel.transform, false);
        
        RectTransform sliderRect = sliderObj.AddComponent<RectTransform>();
        sliderRect.anchoredPosition = new Vector2(25, 0);
        sliderRect.sizeDelta = new Vector2(120, 20);
        
        hammerTimer = sliderObj.AddComponent<Slider>();
        hammerTimer.minValue = 0;
        hammerTimer.maxValue = 1;
        hammerTimer.value = 1;
        
        // Slider background
        GameObject sliderBg = new GameObject("Background");
        sliderBg.transform.SetParent(sliderObj.transform, false);
        Image bgImage = sliderBg.AddComponent<Image>();
        bgImage.color = Color.gray;
        RectTransform bgRect = sliderBg.GetComponent<RectTransform>();
        bgRect.anchorMin = Vector2.zero;
        bgRect.anchorMax = Vector2.one;
        bgRect.offsetMin = Vector2.zero;
        bgRect.offsetMax = Vector2.zero;
        
        // Slider fill
        GameObject sliderFill = new GameObject("Fill");
        sliderFill.transform.SetParent(sliderObj.transform, false);
        Image fillImage = sliderFill.AddComponent<Image>();
        fillImage.color = Color.red;
        RectTransform fillRect = sliderFill.GetComponent<RectTransform>();
        fillRect.anchorMin = Vector2.zero;
        fillRect.anchorMax = Vector2.one;
        fillRect.offsetMin = Vector2.zero;
        fillRect.offsetMax = Vector2.zero;
        
        hammerTimer.targetGraphic = fillImage;
        hammerTimer.fillRect = fillRect;
        
        hammerTimerPanel.SetActive(false);
    }
    
    void CreatePauseMenu()
    {
        pauseMenu = new GameObject("PauseMenu");
        pauseMenu.transform.SetParent(mainCanvas.transform, false);
        
        // Background
        GameObject bgObj = new GameObject("Background");
        bgObj.transform.SetParent(pauseMenu.transform, false);
        RectTransform bgRect = bgObj.AddComponent<RectTransform>();
        bgRect.anchorMin = Vector2.zero;
        bgRect.anchorMax = Vector2.one;
        bgRect.offsetMin = Vector2.zero;
        bgRect.offsetMax = Vector2.zero;
        Image bgImage = bgObj.AddComponent<Image>();
        bgImage.color = new Color(0, 0, 0, 0.8f);
        
        // Title
        GameObject titleObj = CreateUIText("Title", new Vector2(0, 200), "PAUSED", 60);
        titleObj.transform.SetParent(pauseMenu.transform, false);
        
        // Buttons
        GameObject resumeBtnObj = CreateUIButton("ResumeButton", new Vector2(0, 50), "RESUME", 36);
        resumeButton = resumeBtnObj.GetComponent<Button>();
        resumeBtnObj.transform.SetParent(pauseMenu.transform, false);
        
        GameObject restartBtnObj = CreateUIButton("RestartButton", new Vector2(0, -50), "RESTART", 36);
        restartButton = restartBtnObj.GetComponent<Button>();
        restartBtnObj.transform.SetParent(pauseMenu.transform, false);
        
        GameObject mainMenuBtnObj = CreateUIButton("MainMenuButton", new Vector2(0, -150), "MAIN MENU", 36);
        mainMenuButton = mainMenuBtnObj.GetComponent<Button>();
        mainMenuBtnObj.transform.SetParent(pauseMenu.transform, false);
        
        pauseMenu.SetActive(false);
    }
    
    void CreateGameOverPanel()
    {
        gameOverPanel = new GameObject("GameOverPanel");
        gameOverPanel.transform.SetParent(mainCanvas.transform, false);
        
        // Background
        GameObject bgObj = new GameObject("Background");
        bgObj.transform.SetParent(gameOverPanel.transform, false);
        RectTransform bgRect = bgObj.AddComponent<RectTransform>();
        bgRect.anchorMin = Vector2.zero;
        bgRect.anchorMax = Vector2.one;
        bgRect.offsetMin = Vector2.zero;
        bgRect.offsetMax = Vector2.zero;
        Image bgImage = bgObj.AddComponent<Image>();
        bgImage.color = new Color(0, 0, 0, 0.9f);
        
        // Title
        GameObject titleObj = CreateUIText("Title", new Vector2(0, 250), "GAME OVER", 72);
        titleObj.GetComponent<Text>().color = Color.red;
        titleObj.transform.SetParent(gameOverPanel.transform, false);
        
        // Final stats
        GameObject finalScoreObj = CreateUIText("FinalScore", new Vector2(0, 150), "FINAL SCORE: 0", 36);
        finalScoreText = finalScoreObj.GetComponent<Text>();
        finalScoreObj.transform.SetParent(gameOverPanel.transform, false);
        
        GameObject finalLevelObj = CreateUIText("FinalLevel", new Vector2(0, 100), "LEVEL REACHED: 1", 36);
        finalLevelText = finalLevelObj.GetComponent<Text>();
        finalLevelObj.transform.SetParent(gameOverPanel.transform, false);
        
        GameObject finalTimeObj = CreateUIText("FinalTime", new Vector2(0, 50), "TIME: 00:00", 36);
        finalTimeText = finalTimeObj.GetComponent<Text>();
        finalTimeObj.transform.SetParent(gameOverPanel.transform, false);
        
        // Buttons
        GameObject playAgainBtnObj = CreateUIButton("PlayAgainButton", new Vector2(0, -50), "PLAY AGAIN", 36);
        playAgainButton = playAgainBtnObj.GetComponent<Button>();
        playAgainBtnObj.transform.SetParent(gameOverPanel.transform, false);
        
        GameObject quitBtnObj = CreateUIButton("QuitButton", new Vector2(0, -150), "QUIT", 36);
        quitButton = quitBtnObj.GetComponent<Button>();
        quitBtnObj.transform.SetParent(gameOverPanel.transform, false);
        
        gameOverPanel.SetActive(false);
    }
    
    void CreateTouchControls()
    {
        touchControlsPanel = new GameObject("TouchControls");
        touchControlsPanel.transform.SetParent(mainCanvas.transform, false);
        
        // Left movement buttons
        GameObject leftBtnObj = CreateUIButton("LeftButton", new Vector2(-700, -350), "←", 48);
        leftButton = leftBtnObj.GetComponent<Button>();
        leftBtnObj.transform.SetParent(touchControlsPanel.transform, false);
        
        GameObject rightBtnObj = CreateUIButton("RightButton", new Vector2(-500, -350), "→", 48);
        rightButton = rightBtnObj.GetComponent<Button>();
        rightBtnObj.transform.SetParent(touchControlsPanel.transform, false);
        
        GameObject upBtnObj = CreateUIButton("UpButton", new Vector2(-600, -250), "↑", 48);
        upButton = upBtnObj.GetComponent<Button>();
        upBtnObj.transform.SetParent(touchControlsPanel.transform, false);
        
        GameObject downBtnObj = CreateUIButton("DownButton", new Vector2(-600, -450), "↓", 48);
        downButton = downBtnObj.GetComponent<Button>();
        downBtnObj.transform.SetParent(touchControlsPanel.transform, false);
        
        // Right side buttons
        GameObject jumpBtnObj = CreateUIButton("JumpButton", new Vector2(600, -350), "JUMP", 24);
        jumpButton = jumpBtnObj.GetComponent<Button>();
        jumpBtnObj.transform.SetParent(touchControlsPanel.transform, false);
        
        GameObject hammerBtnObj = CreateUIButton("HammerButton", new Vector2(600, -250), "HAMMER", 20);
        hammerButton = hammerBtnObj.GetComponent<Button>();
        hammerBtnObj.transform.SetParent(touchControlsPanel.transform, false);
    }
    
    GameObject CreateUIText(string name, Vector2 position, string text, int fontSize)
    {
        GameObject textObj = new GameObject(name);
        RectTransform rect = textObj.AddComponent<RectTransform>();
        rect.anchoredPosition = position;
        rect.sizeDelta = new Vector2(200, 50);
        
        Text textComponent = textObj.AddComponent<Text>();
        textComponent.text = text;
        textComponent.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        textComponent.fontSize = fontSize;
        textComponent.color = Color.white;
        textComponent.alignment = TextAnchor.MiddleCenter;
        
        return textObj;
    }
    
    GameObject CreateUIButton(string name, Vector2 position, string text, int fontSize)
    {
        GameObject buttonObj = new GameObject(name);
        RectTransform rect = buttonObj.AddComponent<RectTransform>();
        rect.anchoredPosition = position;
        rect.sizeDelta = new Vector2(150, 80);
        
        Image image = buttonObj.AddComponent<Image>();
        image.color = new Color(0.2f, 0.2f, 0.8f, 0.8f);
        
        Button button = buttonObj.AddComponent<Button>();
        
        // Add text child
        GameObject textObj = new GameObject("Text");
        textObj.transform.SetParent(buttonObj.transform, false);
        
        RectTransform textRect = textObj.AddComponent<RectTransform>();
        textRect.anchoredPosition = Vector2.zero;
        textRect.sizeDelta = rect.sizeDelta;
        
        Text buttonText = textObj.AddComponent<Text>();
        buttonText.text = text;
        buttonText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        buttonText.fontSize = fontSize;
        buttonText.color = Color.white;
        buttonText.alignment = TextAnchor.MiddleCenter;
        
        return buttonObj;
    }
    
    void SetupButtonListeners()
    {
        // Will be set up when buttons are created
    }
    
    public void ShowMainMenu()
    {
        gameUI?.SetActive(false);
        pauseMenu?.SetActive(false);
        gameOverPanel?.SetActive(false);
        touchControlsPanel?.SetActive(false);
        
        // Show main menu (create if needed)
        if (mainMenu == null)
        {
            CreateMainMenu();
        }
        mainMenu.SetActive(true);
    }
    
    void CreateMainMenu()
    {
        mainMenu = new GameObject("MainMenu");
        mainMenu.transform.SetParent(mainCanvas.transform, false);
        
        // Title
        GameObject titleObj = CreateUIText("Title", new Vector2(0, 200), "DONKEY KONG", 72);
        titleObj.GetComponent<Text>().color = Color.yellow;
        titleObj.transform.SetParent(mainMenu.transform, false);
        
        // Start button
        GameObject startBtnObj = CreateUIButton("StartButton", new Vector2(0, 0), "START GAME", 36);
        Button startBtn = startBtnObj.GetComponent<Button>();
        startBtn.onClick.AddListener(() => {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.StartNewGame();
            }
        });
        startBtnObj.transform.SetParent(mainMenu.transform, false);
        
        // Quit button
        GameObject quitBtnObj = CreateUIButton("QuitButton", new Vector2(0, -100), "QUIT", 36);
        Button quitBtn = quitBtnObj.GetComponent<Button>();
        quitBtn.onClick.AddListener(() => Application.Quit());
        quitBtnObj.transform.SetParent(mainMenu.transform, false);
    }
    
    public void ShowGameUI()
    {
        mainMenu?.SetActive(false);
        pauseMenu?.SetActive(false);
        gameOverPanel?.SetActive(false);
        
        gameUI?.SetActive(true);
        touchControlsPanel?.SetActive(true);
    }
    
    public void UpdateScore(int score)
    {
        if (scoreText != null)
        {
            scoreText.text = $"SCORE: {score:N0}";
        }
    }
    
    public void UpdateLives(int lives)
    {
        if (livesText != null)
        {
            livesText.text = $"LIVES: {lives}";
        }
    }
    
    public void UpdateLevel(int level)
    {
        if (levelText != null)
        {
            levelText.text = $"LEVEL: {level}";
        }
    }
    
    public void ShowMessage(string message, float duration = 0f)
    {
        if (messageText != null)
        {
            if (messageCoroutine != null)
            {
                StopCoroutine(messageCoroutine);
            }
            
            messageCoroutine = StartCoroutine(DisplayMessage(message, duration > 0 ? duration : messageDisplayTime));
        }
    }
    
    IEnumerator DisplayMessage(string message, float duration)
    {
        messageText.text = message;
        yield return new WaitForSeconds(duration);
        messageText.text = "";
    }
    
    public void ShowScorePopup(int points, Vector3 worldPosition)
    {
        // Convert world position to screen position
        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPosition);
        Vector2 uiPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            mainCanvas.transform as RectTransform, screenPos, null, out uiPos);
        
        GameObject popupObj = CreateUIText("ScorePopup", uiPos, $"+{points}", 24);
        popupObj.GetComponent<Text>().color = scorePopupColor;
        popupObj.transform.SetParent(mainCanvas.transform, false);
        
        StartCoroutine(AnimateScorePopup(popupObj));
    }
    
    IEnumerator AnimateScorePopup(GameObject popup)
    {
        RectTransform rect = popup.GetComponent<RectTransform>();
        Vector2 startPos = rect.anchoredPosition;
        Vector2 endPos = startPos + Vector2.up * 100;
        
        float duration = 2f;
        float elapsed = 0f;
        
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            
            rect.anchoredPosition = Vector2.Lerp(startPos, endPos, t);
            
            Text text = popup.GetComponent<Text>();
            Color color = text.color;
            color.a = 1f - t;
            text.color = color;
            
            yield return null;
        }
        
        Destroy(popup);
    }
    
    public void ShowHammerTimer(float maxTime)
    {
        if (hammerTimerPanel != null)
        {
            hammerTimerPanel.SetActive(true);
            hammerTimer.maxValue = maxTime;
            hammerTimer.value = maxTime;
        }
    }
    
    public void UpdateHammerTimer(float timeLeft)
    {
        if (hammerTimer != null)
        {
            hammerTimer.value = timeLeft;
        }
    }
    
    public void HideHammerTimer()
    {
        if (hammerTimerPanel != null)
        {
            hammerTimerPanel.SetActive(false);
        }
    }
    
    public void ShowPauseMenu()
    {
        pauseMenu?.SetActive(true);
        isGamePaused = true;
    }
    
    public void HidePauseMenu()
    {
        pauseMenu?.SetActive(false);
        isGamePaused = false;
    }
    
    public void ShowGameOver(int finalScore, int finalLevel, float gameTime)
    {
        gameUI?.SetActive(false);
        touchControlsPanel?.SetActive(false);
        
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
            
            if (finalScoreText != null)
                finalScoreText.text = $"FINAL SCORE: {finalScore:N0}";
            
            if (finalLevelText != null)
                finalLevelText.text = $"LEVEL REACHED: {finalLevel}";
            
            if (finalTimeText != null)
            {
                int minutes = (int)(gameTime / 60);
                int seconds = (int)(gameTime % 60);
                finalTimeText.text = $"TIME: {minutes:D2}:{seconds:D2}";
            }
        }
    }
    
    public void PauseGame()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.PauseGame();
        }
    }
    
    public void ResumeGame()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ResumeGame();
        }
    }
    
    public void RestartGame()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.RestartGame();
        }
    }
}
