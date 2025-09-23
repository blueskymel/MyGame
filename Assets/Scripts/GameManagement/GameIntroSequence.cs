using UnityEngine;
using System.Collections;

public class GameIntroSequence : MonoBehaviour
{
    [Header("Intro Settings")]
    public float introDuration = 8f;
    public float climbSpeed = 2f;
    public float textDisplayTime = 2f;
    
    [Header("Characters")]
    public GameObject donkeyKongPrefab;
    public GameObject princessPrefab;
    public GameObject playerPrefab;
    
    [Header("UI")]
    public string introText = "DONKEY KONG HAS KIDNAPPED THE PRINCESS!";
    public string startText = "RESCUE HER!";
    
    private bool introComplete = false;
    private GameObject introDK;
    private GameObject introPrincess;
    private Camera mainCamera;
    
    void Start()
    {
        mainCamera = Camera.main;
        StartCoroutine(PlayIntroSequence());
    }
    
    IEnumerator PlayIntroSequence()
    {
        Debug.Log("Starting Donkey Kong intro sequence...");
        
        // Hide gameplay elements during intro
        HideGameplayElements();
        
        // Show intro text
        yield return StartCoroutine(ShowIntroText());
        
        // Create intro characters
        CreateIntroCharacters();
        
        // Animate DK climbing with princess
        yield return StartCoroutine(ClimbingSequence());
        
        // Show "Rescue Her" text
        yield return StartCoroutine(ShowStartText());
        
        // Clean up intro and start game
        StartGameplay();
    }
    
    void HideGameplayElements()
    {
        // Hide existing game objects during intro
        GameObject[] gameObjects = {
            GameObject.Find("Player"),
            GameObject.Find("DonkeyKong"), 
            GameObject.Find("SimpleBarrelSpawner")
        };
        
        foreach (GameObject obj in gameObjects)
        {
            if (obj != null)
            {
                obj.SetActive(false);
            }
        }
    }
    
    IEnumerator ShowIntroText()
    {
        Debug.Log("Showing intro text: " + introText);
        
        // You could add UI text display here
        // For now, just show in console and wait
        yield return new WaitForSeconds(textDisplayTime);
    }
    
    void CreateIntroCharacters()
    {
        // Create Donkey Kong at bottom (tall layout)
        introDK = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        introDK.name = "IntroDonkeyKong";
        introDK.transform.position = new Vector3(-2, -6, 0); // Bottom of tall layout
        introDK.transform.localScale = new Vector3(1.2f, 1.8f, 1.2f);
        
        // Make DK brown
        Renderer dkRenderer = introDK.GetComponent<Renderer>();
        Material dkMaterial = new Material(Shader.Find("Standard"));
        dkMaterial.color = new Color(0.3f, 0.15f, 0.05f);
        dkRenderer.material = dkMaterial;
        
        // Create Princess (smaller, pink character) - tall layout
        introPrincess = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        introPrincess.name = "IntroPrincess";
        introPrincess.transform.position = new Vector3(-1.5f, -5.5f, 0); // Bottom of tall layout
        introPrincess.transform.localScale = new Vector3(0.6f, 1.2f, 0.6f);
        
        // Make Princess pink
        Renderer princessRenderer = introPrincess.GetComponent<Renderer>();
        Material princessMaterial = new Material(Shader.Find("Standard"));
        princessMaterial.color = new Color(1f, 0.7f, 0.8f); // Pink
        princessRenderer.material = princessMaterial;
        
        Debug.Log("Intro characters created (tall layout)");
    }
    
    IEnumerator ClimbingSequence()
    {
        Debug.Log("DK climbing with princess to platforms...");
        
        Vector3 startPosDK = introDK.transform.position;
        Vector3 startPosPrincess = introPrincess.transform.position;
        
        // DK stops at top platform (left side, standing ON the platform) - TALL LAYOUT
        Vector3 endPosDK = new Vector3(-0.8f, 6.5f, 0); // Above top platform (y=6 + 0.5 for standing on it)
        
        // Princess goes to highest platform (standing ON the small platform above) - TALL LAYOUT  
        Vector3 endPosPrincess = new Vector3(0f, 8.5f, 0); // Above Princess platform (y=8 + 0.5 for standing on it)
        
        Debug.Log($"🦍 DK climbing from {startPosDK} to TOP PLATFORM: {endPosDK}");
        Debug.Log($"👸 Princess going from {startPosPrincess} to HIGHEST PLATFORM: {endPosPrincess}");
        
        float climbTime = Vector3.Distance(startPosDK, endPosDK) / climbSpeed;
        float timer = 0f;
        
        while (timer < climbTime)
        {
            timer += Time.deltaTime;
            float t = timer / climbTime;
            
            // Move both characters up
            introDK.transform.position = Vector3.Lerp(startPosDK, endPosDK, t);
            introPrincess.transform.position = Vector3.Lerp(startPosPrincess, endPosPrincess, t);
            
            // Add some bobbing animation
            float bobOffset = Mathf.Sin(timer * 5f) * 0.1f;
            introDK.transform.position += Vector3.up * bobOffset;
            
            yield return null;
        }
        
        // Final positions
        introDK.transform.position = endPosDK;
        introPrincess.transform.position = endPosPrincess;
        
        Debug.Log($"🏗️ DK at top platform: {introDK.transform.position}");
        Debug.Log($"👸 Princess at highest platform: {introPrincess.transform.position}");
        
        // DK chest beating animation
        yield return StartCoroutine(ChestBeatAnimation());
    }
    
    IEnumerator ChestBeatAnimation()
    {
        Debug.Log("DK beating chest...");
        
        Vector3 originalScale = introDK.transform.localScale;
        
        for (int i = 0; i < 3; i++)
        {
            // Scale up
            yield return StartCoroutine(ScaleTo(introDK, originalScale * 1.2f, 0.3f));
            // Scale down  
            yield return StartCoroutine(ScaleTo(introDK, originalScale, 0.3f));
            yield return new WaitForSeconds(0.2f);
        }
    }
    
    IEnumerator ScaleTo(GameObject obj, Vector3 targetScale, float duration)
    {
        Vector3 startScale = obj.transform.localScale;
        float timer = 0f;
        
        while (timer < duration)
        {
            timer += Time.deltaTime;
            float t = timer / duration;
            obj.transform.localScale = Vector3.Lerp(startScale, targetScale, t);
            yield return null;
        }
        
        obj.transform.localScale = targetScale;
    }
    
    IEnumerator ShowStartText()
    {
        Debug.Log("Showing start text: " + startText);
        
        // Display "Rescue Her!" text
        yield return new WaitForSeconds(textDisplayTime);
    }
    
    void StartGameplay()
    {
        Debug.Log("Starting gameplay...");
        
        // KEEP intro characters at the top! Don't destroy them!
        if (introDK != null)
        {
            // Rename and reposition DK to replace the gameplay DK
            introDK.name = "DonkeyKong";
            Debug.Log($"🦍 DK staying at top: {introDK.transform.position}");
            
            // Make sure DK has the right components for gameplay
            if (introDK.GetComponent<SimpleDonkeyKong>() == null)
            {
                SimpleDonkeyKong dkScript = introDK.AddComponent<SimpleDonkeyKong>();
                dkScript.bobSpeed = 2f;
                dkScript.bobAmount = 0.1f;
            }
        }
        
        if (introPrincess != null)
        {
            // Keep princess at the top too
            introPrincess.name = "Princess";
            Debug.Log($"👸 Princess staying at top: {introPrincess.transform.position}");
        }
        
        // Remove the OLD DonkeyKong that was created during scene setup
        GameObject oldDK = GameObject.Find("DonkeyKong");
        if (oldDK != null && oldDK != introDK)
        {
            Debug.Log("🗑️ Removing old DonkeyKong, keeping intro version at top");
            Destroy(oldDK);
        }
        
        // Show gameplay elements
        ShowGameplayElements();
        
        // Start barrel spawning
        SimpleBarrelSpawner spawner = FindObjectOfType<SimpleBarrelSpawner>();
        if (spawner != null)
        {
            spawner.StartSpawning();
        }
        
        introComplete = true;
        
        // Destroy this intro manager
        Destroy(gameObject);
    }
    
    void ShowGameplayElements()
    {
        // Show game objects (but not the old DonkeyKong since we're keeping the intro one)
        GameObject player = GameObject.Find("Player");
        if (player != null)
        {
            player.SetActive(true);
            Debug.Log("✅ Player activated for gameplay");
        }
        
        GameObject spawner = GameObject.Find("SimpleBarrelSpawner");
        if (spawner != null)
        {
            spawner.SetActive(true);
            Debug.Log("✅ Barrel spawner activated for gameplay");
        }
        
        // Don't activate the old DonkeyKong - we're using the intro version now!
        
        Debug.Log("✅ Gameplay elements activated (keeping DK and Princess at top)");
    }
    
    void OnGUI()
    {
        if (!introComplete)
        {
            // Simple on-screen text during intro
            GUI.skin.label.fontSize = 24;
            GUI.skin.label.normal.textColor = Color.white;
            
            if (introDK != null && introPrincess != null)
            {
                // Show intro text
                float screenWidth = Screen.width;
                float screenHeight = Screen.height;
                
                GUILayout.BeginArea(new Rect(50, 50, screenWidth - 100, 100));
                GUILayout.Label(introText);
                GUILayout.EndArea();
                
                // Show rescue text when climbing is done
                if (introDK.transform.position.y > 2f)
                {
                    GUILayout.BeginArea(new Rect(50, screenHeight - 150, screenWidth - 100, 100));
                    GUILayout.Label(startText);
                    GUILayout.EndArea();
                }
            }
        }
    }
}
