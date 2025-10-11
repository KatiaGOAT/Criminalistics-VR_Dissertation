using UnityEngine;

public class PPEManager : MonoBehaviour
{
    public static PPEManager Instance;

    private bool maskEquipped = false;
    private bool suitEquipped = false;

    [Header("UI References")]
    public GameObject completeUI; // UI for "All PPE equipped"

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    // Called when a PPE item is equipped
    public void RegisterEquippedItem(string itemName)
    {
        Debug.Log($"RegisterEquippedItem called for: {itemName}");

        // Normalize to lower case for safer string checking
        string lowerName = itemName.ToLower();

        // Check for mask
        if (lowerName.Contains("mask"))
        {
            if (!maskEquipped)
            {
                maskEquipped = true;
                Debug.Log("Mask equipped!");
            }
            else
            {
                Debug.Log("Mask already equipped (duplicate event ignored).");
            }
        }

        // Check for PPE kit / suit
        if (lowerName.Contains("ppe") || lowerName.Contains("kit") || lowerName.Contains("suit"))
        {
            if (!suitEquipped)
            {
                suitEquipped = true;
                Debug.Log("PPE suit equipped!");
            }
            else
            {
                Debug.Log("PPE suit already equipped (duplicate event ignored).");
            }
        }

        // Check completion
        CheckCompletion();
    }

    void CheckCompletion()
    {
        Debug.Log($"Checking completion... Mask: {maskEquipped}, Suit: {suitEquipped}");

        if (maskEquipped && suitEquipped)
        {
            Debug.Log("All PPE equipped! You can now enter the apartment.");
            if (completeUI != null)
            {
                completeUI.SetActive(true);
                Debug.Log("Complete UI activated.");
            }
            else
            {
                Debug.LogWarning("completeUI not assigned in Inspector.");
            }
        }
    }

    public bool IsPPEComplete()
    {
        bool complete = (maskEquipped && suitEquipped);
        Debug.Log($"IsPPEComplete() called ? {complete}");
        return complete;
    }
}
