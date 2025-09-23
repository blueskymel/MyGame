using UnityEngine;
using System.Collections;

public class SimpleDonkeyKong : MonoBehaviour
{
    [Header("Animation")]
    public float bobSpeed = 2f;
    public float bobAmount = 0.1f;
    
    private Vector3 startPos;
    private float bobTimer = 0f;
    
    void Start()
    {
        startPos = transform.position;
        Debug.Log("SimpleDonkeyKong initialized - no sprite dependencies!");
        
        // Start simple animation
        StartCoroutine(SimpleAnimation());
    }
    
    void Update()
    {
        // Simple bobbing animation to show DK is "alive"
        bobTimer += Time.deltaTime * bobSpeed;
        float newY = startPos.y + Mathf.Sin(bobTimer) * bobAmount;
        transform.position = new Vector3(startPos.x, newY, startPos.z);
    }
    
    IEnumerator SimpleAnimation()
    {
        while (true)
        {
            // Scale animation to simulate chest beating
            yield return StartCoroutine(ScalePulse());
            yield return new WaitForSeconds(2f);
        }
    }
    
    IEnumerator ScalePulse()
    {
        Vector3 originalScale = transform.localScale;
        Vector3 biggerScale = originalScale * 1.1f;
        
        // Scale up
        float timer = 0f;
        while (timer < 0.3f)
        {
            timer += Time.deltaTime;
            float t = timer / 0.3f;
            transform.localScale = Vector3.Lerp(originalScale, biggerScale, t);
            yield return null;
        }
        
        // Scale back down
        timer = 0f;
        while (timer < 0.3f)
        {
            timer += Time.deltaTime;
            float t = timer / 0.3f;
            transform.localScale = Vector3.Lerp(biggerScale, originalScale, t);
            yield return null;
        }
        
        transform.localScale = originalScale;
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player reached Donkey Kong! Victory!");
            
            // Find GameManager and trigger victory
            GameManager gm = FindObjectOfType<GameManager>();
            if (gm != null)
            {
                // You could add a victory method to GameManager
                Debug.Log("Player wins the game!");
            }
        }
    }
}
