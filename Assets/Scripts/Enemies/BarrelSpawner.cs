using UnityEngine;
using System.Collections;

public class BarrelSpawner : MonoBehaviour
{
    [Header("Spawning Settings")]
    public GameObject barrelPrefab;
    public Transform spawnPoint;
    public float spawnInterval = 3f;
    public float spawnIntervalVariation = 1f;
    
    [Header("Barrel Settings")]
    public float barrelSpeed = 3f;
    public float barrelSpeedVariation = 1f;
    
    [Header("Difficulty Scaling")]
    public bool scaleDifficulty = true;
    public float difficultyIncreaseRate = 0.1f;
    public float minSpawnInterval = 0.5f;
    public float maxBarrelSpeed = 8f;
    
    [Header("Audio")]
    public AudioClip spawnSound;
    
    private AudioSource audioSource;
    private bool isSpawning = false;
    private float currentSpawnInterval;
    private float currentBarrelSpeed;
    private int barrelsSpawned = 0;
    
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        
        currentSpawnInterval = spawnInterval;
        currentBarrelSpeed = barrelSpeed;
    }
    
    void Start()
    {
        if (barrelPrefab == null)
        {
            Debug.LogError("Barrel prefab not assigned to BarrelSpawner!");
            return;
        }
        
        if (spawnPoint == null)
        {
            spawnPoint = transform;
        }
        
        StartSpawning();
    }
    
    public void StartSpawning()
    {
        if (!isSpawning)
        {
            isSpawning = true;
            StartCoroutine(SpawnBarrels());
        }
    }
    
    public void StopSpawning()
    {
        isSpawning = false;
        StopAllCoroutines();
    }
    
    IEnumerator SpawnBarrels()
    {
        while (isSpawning)
        {
            yield return new WaitForSeconds(GetRandomizedSpawnInterval());
            
            if (isSpawning && GameManager.Instance != null && GameManager.Instance.IsGameActive())
            {
                SpawnBarrel();
            }
        }
    }
    
    void SpawnBarrel()
    {
        if (barrelPrefab == null || spawnPoint == null) return;
        
        // Create barrel
        GameObject newBarrel = Instantiate(barrelPrefab, spawnPoint.position, spawnPoint.rotation);
        
        // Configure barrel
        Barrel barrelScript = newBarrel.GetComponent<Barrel>();
        if (barrelScript != null)
        {
            float randomizedSpeed = currentBarrelSpeed + Random.Range(-barrelSpeedVariation, barrelSpeedVariation);
            barrelScript.SetRollSpeed(randomizedSpeed);
            
            // Randomly choose direction (mostly left, sometimes right for variety)
            float direction = Random.value < 0.8f ? -1f : 1f;
            barrelScript.SetRollDirection(direction);
        }
        
        // Play spawn sound
        if (spawnSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(spawnSound);
        }
        
        barrelsSpawned++;
        
        // Scale difficulty if enabled
        if (scaleDifficulty)
        {
            ScaleDifficulty();
        }
        
        // Notify game manager
        if (GameManager.Instance != null)
        {
            GameManager.Instance.BarrelSpawned();
        }
    }
    
    void ScaleDifficulty()
    {
        // Increase spawn rate (decrease interval)
        currentSpawnInterval = Mathf.Max(minSpawnInterval, 
            spawnInterval - (barrelsSpawned * difficultyIncreaseRate));
        
        // Increase barrel speed
        currentBarrelSpeed = Mathf.Min(maxBarrelSpeed, 
            barrelSpeed + (barrelsSpawned * difficultyIncreaseRate * 0.5f));
    }
    
    float GetRandomizedSpawnInterval()
    {
        return currentSpawnInterval + Random.Range(-spawnIntervalVariation, spawnIntervalVariation);
    }
    
    // Public methods for external control
    public void SetSpawnInterval(float interval)
    {
        spawnInterval = interval;
        currentSpawnInterval = interval;
    }
    
    public void SetBarrelSpeed(float speed)
    {
        barrelSpeed = speed;
        currentBarrelSpeed = speed;
    }
    
    public void ResetDifficulty()
    {
        currentSpawnInterval = spawnInterval;
        currentBarrelSpeed = barrelSpeed;
        barrelsSpawned = 0;
    }
    
    // Get current stats for UI display
    public float GetCurrentSpawnInterval()
    {
        return currentSpawnInterval;
    }
    
    public float GetCurrentBarrelSpeed()
    {
        return currentBarrelSpeed;
    }
    
    public int GetBarrelsSpawned()
    {
        return barrelsSpawned;
    }
}
