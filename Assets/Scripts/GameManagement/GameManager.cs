using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    [Header("Game Settings")]
    public int startingLives = 3;
    public int pointsPerBarrel = 100;
    public int pointsPerHammerHit = 300;
    public int pointsPerPrincess = 5000;
    public float invincibilityDuration = 2f;
    
    [Header("Hammer Settings")]
    public GameObject hammerPrefab;
    public float hammerDuration = 10f;
    
    [Header("Level Settings")]
    public int currentLevel = 1;
    public float levelCompletionDelay = 3f;
    
    // Singleton instance
    public static GameManager Instance { get; private set; }
    
    // Game state
    private int currentScore = 0;
    private int currentLives;
    private bool gameActive = true;
    private bool playerInvincible = false;
    private bool playerHasHammer = false;
    private float hammerTimer = 0f;
    
    // References
    private PlayerController playerController;
    private DonkeyKong donkeyKong;
    private UIManager uiManager;
    private AudioManager audioManager;
    
    // Statistics
    private int barrelsDestroyed = 0;
    private int barrelsSpawned = 0;
    private float gameTime = 0f;
    
    // Events
    public System.Action<int> OnScoreChanged;
    public System.Action<int> OnLivesChanged;
    public System.Action OnGameOver;
    public System.Action OnLevelComplete;
    public System.Action<bool> OnHammerStateChanged;
    
    void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeGame();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void Start()
    {
        FindGameComponents();
        StartNewGame();
    }
    
    void Update()
    {
        if (gameActive)
        {
            gameTime += Time.deltaTime;
            UpdateHammer();
        }
    }
    
    void InitializeGame()
    {
        currentLives = startingLives;
        currentScore = 0;
        currentLevel = 1;
        gameActive = true;
        playerInvincible = false;
        playerHasHammer = false;
    }
    
    void FindGameComponents()
    {
        playerController = FindObjectOfType<PlayerController>();
        donkeyKong = FindObjectOfType<DonkeyKong>();
        uiManager = FindObjectOfType<UIManager>();
        audioManager = FindObjectOfType<AudioManager>();
        
        if (uiManager == null)
        {
            Debug.LogWarning("UIManager not found! Creating default UI.");
            CreateDefaultUI();
        }
        
        if (audioManager == null)
        {
            Debug.LogWarning("AudioManager not found! Creating default audio manager.");
            CreateDefaultAudioManager();
        }
    }
    
    void CreateDefaultUI()
    {
        GameObject uiObj = new GameObject("UIManager");
        uiManager = uiObj.AddComponent<UIManager>();
    }
    
    void CreateDefaultAudioManager()
    {
        GameObject audioObj = new GameObject("AudioManager");
        audioManager = audioObj.AddComponent<AudioManager>();
    }
    
    public void StartNewGame()
    {
        currentScore = 0;
        currentLives = startingLives;
        currentLevel = 1;
        gameActive = true;
        barrelsDestroyed = 0;
        barrelsSpawned = 0;
        gameTime = 0f;
        
        OnScoreChanged?.Invoke(currentScore);
        OnLivesChanged?.Invoke(currentLives);
        
        if (uiManager != null)
        {
            uiManager.ShowGameUI();
            uiManager.UpdateScore(currentScore);
            uiManager.UpdateLives(currentLives);
            uiManager.UpdateLevel(currentLevel);
        }
        
        RespawnPlayer();
    }
    
    public void AddScore(int points)
    {
        if (!gameActive) return;
        
        currentScore += points;
        OnScoreChanged?.Invoke(currentScore);
        
        if (uiManager != null)
        {
            uiManager.UpdateScore(currentScore);
            uiManager.ShowScorePopup(points, playerController.transform.position);
        }
        
        // Award extra life every 20,000 points
        if (currentScore % 20000 < points)
        {
            AddLife();
        }
    }
    
    public void AddLife()
    {
        currentLives++;
        OnLivesChanged?.Invoke(currentLives);
        
        if (uiManager != null)
        {
            uiManager.UpdateLives(currentLives);
            uiManager.ShowMessage("EXTRA LIFE!", 2f);
        }
        
        if (audioManager != null)
        {
            audioManager.PlayExtraLifeSound();
        }
    }
    
    public void PlayerDied()
    {
        if (!gameActive || playerInvincible) return;
        
        currentLives--;
        OnLivesChanged?.Invoke(currentLives);
        
        if (audioManager != null)
        {
            audioManager.PlayDeathSound();
        }
        
        if (currentLives <= 0)
        {
            GameOver();
        }
        else
        {
            if (uiManager != null)
            {
                uiManager.UpdateLives(currentLives);
                uiManager.ShowMessage($"LIVES: {currentLives}", 2f);
            }
            
            StartCoroutine(RespawnPlayerDelayed());
        }
    }
    
    public void PlayerHitByBarrel()
    {
        PlayerDied();
    }
    
    public void PlayerPickedUpHammer()
    {
        if (!gameActive) return;
        
        playerHasHammer = true;
        hammerTimer = hammerDuration;
        OnHammerStateChanged?.Invoke(true);
        
        if (uiManager != null)
        {
            uiManager.ShowHammerTimer(hammerDuration);
        }
        
        if (audioManager != null)
        {
            audioManager.PlayHammerPickupSound();
        }
        
        AddScore(500); // Bonus for picking up hammer
    }
    
    public void PlayerReachedPrincess()
    {
        if (!gameActive) return;
        
        AddScore(pointsPerPrincess);
        LevelComplete();
    }
    
    public void BarrelDestroyed()
    {
        barrelsDestroyed++;
        AddScore(pointsPerHammerHit);
        
        if (audioManager != null)
        {
            audioManager.PlayBarrelDestroySound();
        }
    }
    
    public void BarrelSpawned()
    {
        barrelsSpawned++;
    }
    
    void UpdateHammer()
    {
        if (playerHasHammer)
        {
            hammerTimer -= Time.deltaTime;
            
            if (uiManager != null)
            {
                uiManager.UpdateHammerTimer(hammerTimer);
            }
            
            if (hammerTimer <= 0)
            {
                playerHasHammer = false;
                OnHammerStateChanged?.Invoke(false);
                
                if (uiManager != null)
                {
                    uiManager.HideHammerTimer();
                }
            }
        }
    }
    
    void LevelComplete()
    {
        gameActive = false;
        currentLevel++;
        
        OnLevelComplete?.Invoke();
        
        if (uiManager != null)
        {
            uiManager.ShowMessage("LEVEL COMPLETE!", levelCompletionDelay);
            uiManager.UpdateLevel(currentLevel);
        }
        
        if (audioManager != null)
        {
            audioManager.PlayLevelCompleteSound();
        }
        
        StartCoroutine(LoadNextLevel());
    }
    
    IEnumerator LoadNextLevel()
    {
        yield return new WaitForSeconds(levelCompletionDelay);
        
        // Reset game state for next level
        gameActive = true;
        playerHasHammer = false;
        hammerTimer = 0f;
        
        // Increase difficulty
        if (donkeyKong != null)
        {
            float newInterval = Mathf.Max(1f, 3f - (currentLevel * 0.2f));
            donkeyKong.SetThrowInterval(newInterval);
        }
        
        RespawnPlayer();
        
        if (uiManager != null)
        {
            uiManager.ShowGameUI();
        }
    }
    
    void GameOver()
    {
        gameActive = false;
        OnGameOver?.Invoke();
        
        if (uiManager != null)
        {
            uiManager.ShowGameOver(currentScore, currentLevel, gameTime);
        }
        
        if (audioManager != null)
        {
            audioManager.PlayGameOverSound();
        }
        
        // Stop all barrel spawning
        if (donkeyKong != null)
        {
            donkeyKong.StopThrowing();
        }
        
        BarrelSpawner[] spawners = FindObjectsOfType<BarrelSpawner>();
        foreach (BarrelSpawner spawner in spawners)
        {
            spawner.StopSpawning();
        }
    }
    
    public void RestartGame()
    {
        // Reload the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    public void PauseGame()
    {
        Time.timeScale = 0f;
        
        if (uiManager != null)
        {
            uiManager.ShowPauseMenu();
        }
    }
    
    public void ResumeGame()
    {
        Time.timeScale = 1f;
        
        if (uiManager != null)
        {
            uiManager.HidePauseMenu();
        }
    }
    
    IEnumerator RespawnPlayerDelayed()
    {
        yield return new WaitForSeconds(2f);
        RespawnPlayer();
    }
    
    void RespawnPlayer()
    {
        if (playerController != null)
        {
            // Move player to spawn position
            Vector3 spawnPosition = new Vector3(1f, 1f, 0f); // Bottom left of level
            playerController.transform.position = spawnPosition;
            
            // Reset player state
            Rigidbody2D playerRb = playerController.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                playerRb.velocity = Vector2.zero;
            }
            
            // Grant temporary invincibility
            StartCoroutine(GrantInvincibility());
        }
    }
    
    IEnumerator GrantInvincibility()
    {
        playerInvincible = true;
        
        // Flash player sprite to indicate invincibility
        SpriteRenderer playerSprite = playerController.GetComponent<SpriteRenderer>();
        if (playerSprite != null)
        {
            for (float t = 0; t < invincibilityDuration; t += 0.1f)
            {
                playerSprite.color = new Color(1f, 1f, 1f, 0.5f);
                yield return new WaitForSeconds(0.05f);
                playerSprite.color = Color.white;
                yield return new WaitForSeconds(0.05f);
            }
            
            playerSprite.color = Color.white;
        }
        
        playerInvincible = false;
    }
    
    // Public getters for game state
    public bool IsGameActive() => gameActive;
    public bool IsPlayerInvincible() => playerInvincible;
    public bool PlayerHasHammer() => playerHasHammer;
    public int GetScore() => currentScore;
    public int GetLives() => currentLives;
    public int GetLevel() => currentLevel;
    public float GetGameTime() => gameTime;
    public int GetBarrelsDestroyed() => barrelsDestroyed;
    public int GetBarrelsSpawned() => barrelsSpawned;
}
