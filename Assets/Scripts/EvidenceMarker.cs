using UnityEngine;

/// <summary>
/// Attached to each evidence marker (e.g., evidence_marker_1).
/// Detects when it’s placed near a valid evidence socket.
/// Plays sound, triggers evidence labeling, and updates the Case File UI.
/// </summary>
public class EvidenceMarker : MonoBehaviour
{
    [Header("Marker Info")]
    public int markerNumber;              // Example: 1, 2, 3... (each must be unique)
    public bool isPlaced = false;         // Has the marker already been placed?

    [Header("Audio")]
    public AudioClip placeSound;          // The "tick" sound when placed
    public AudioSource audioSource;      // To play the sound

    void Start()
    {
        
    }

    /// <summary>
    /// Triggered when this marker enters another collider (like an evidence socket).
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        // If already placed, ignore further triggers
        if (isPlaced) return;

        // Check if the object we hit is tagged as "EvidenceSocket"
        if (other.CompareTag("EvidenceSocket"))
        {
            // Get the EvidenceItem component from the socket’s parent
            EvidenceItem evidence = other.GetComponentInParent<EvidenceItem>();

            // If we found an evidence item and it’s not already labeled
            if (evidence != null && !evidence.isLabeled)
            {
                // Label this evidence using our marker number
                evidence.LabelEvidence(markerNumber);
                isPlaced = true;

                // Play the tick sound for feedback
                if (audioSource && placeSound)
                    audioSource.PlayOneShot(placeSound);

                // Inform the Case File Manager to update the UI
                CaseFileManager.Instance.UpdateCaseFile(markerNumber, evidence.evidenceName);
            }
        }
    }
}
