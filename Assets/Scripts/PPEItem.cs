using UnityEngine;

public class PPEItem : MonoBehaviour
{
    public string itemName;               // "Mask" or "PPE Suit"
    public GameObject feedbackUI;         // Reference to small UI message
    public AudioClip equipSound;          // Optional sound for feedback

    public AudioSource audioSource;
    private bool isEquipped = false;

    void Start()
    {
        
    }

    // Called when the player clicks on this object
    public void OnSelect()
    {
        if (isEquipped) return;

        isEquipped = true;

        // Hide the model (simulate equipping)
        gameObject.SetActive(false);

        // Play sound
        if (equipSound)
            audioSource.PlayOneShot(equipSound);

        // Show feedback UI
        if (feedbackUI != null)
        {
            feedbackUI.SetActive(true);
            Debug.Log($"Showing feedback UI for {itemName}");
        }
        else
        {
            Debug.LogWarning($"feedbackUI not assigned for {itemName}");
        }

        // Notify manager
        if (PPEManager.Instance != null)
        {
            PPEManager.Instance.RegisterEquippedItem(itemName);
        }
        else
        {
            Debug.LogError("PPEManager.Instance is null — make sure there's one in the scene.");
        }
    }
}
