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
        if (itemName.Contains("Mask"))
            maskEquipped = true;

        if (itemName.Contains("PPE") || itemName.Contains("Kit"))
            suitEquipped = true;

        CheckCompletion();
    }

    void CheckCompletion()
    {
        if (maskEquipped && suitEquipped && completeUI != null)
            completeUI.SetActive(true);
    }

    public bool IsPPEComplete()
    {
        return (maskEquipped && suitEquipped);
    }
}
