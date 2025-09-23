using UnityEngine;
using System.Collections;

public class RuntimeGraphicsApplier : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(ApplyGraphicsToAllCharacters());
    }
    
    IEnumerator ApplyGraphicsToAllCharacters()
    {
        // Wait longer for all objects to be created, especially intro characters
        yield return new WaitForSeconds(0.5f);
        
        Debug.Log("🎨 Applying graphics to all characters...");
        
        CharacterGraphics graphics = FindObjectOfType<CharacterGraphics>();
        if (graphics == null)
        {
            Debug.LogError("❌ CharacterGraphics not found! Graphics system failed!");
            yield break;
        }
        
        // Apply Mario graphics to player
        GameObject player = GameObject.Find("Player");
        if (player != null)
        {
            Debug.Log("🟥 Applying Mario graphics to Player...");
            graphics.ApplyMarioGraphics(player);
        }
        else
        {
            Debug.LogWarning("⚠️ Player not found for graphics!");
        }
        
        // Apply Donkey Kong graphics
        GameObject dk = GameObject.Find("DonkeyKong");
        if (dk != null)
        {
            Debug.Log("🦍 Applying DK graphics to DonkeyKong...");
            graphics.ApplyDonkeyKongGraphics(dk);
        }
        else
        {
            Debug.LogWarning("⚠️ DonkeyKong not found for graphics!");
        }
        
        // Wait and check for intro characters multiple times
        for (int attempts = 0; attempts < 10; attempts++)
        {
            // Apply graphics to intro characters if they exist
            GameObject introDK = GameObject.Find("IntroDonkeyKong");
            if (introDK != null && introDK.GetComponent<SpriteRenderer>() == null)
            {
                Debug.Log("🦍 Applying DK graphics to IntroDonkeyKong...");
                graphics.ApplyDonkeyKongGraphics(introDK);
            }
            
            GameObject introPrincess = GameObject.Find("IntroPrincess");
            if (introPrincess != null && introPrincess.GetComponent<SpriteRenderer>() == null)
            {
                Debug.Log("👸 Applying Princess graphics to IntroPrincess...");
                graphics.ApplyPrincessGraphics(introPrincess);
            }
            
            yield return new WaitForSeconds(0.2f);
        }
        
        Debug.Log("✅ Graphics application complete!");
    }
}
