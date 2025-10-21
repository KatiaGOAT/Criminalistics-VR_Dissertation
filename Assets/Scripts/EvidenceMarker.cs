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
        if (isPlaced) return;

        //Check Case File activation before allowing labeling
        if (!CaseFileManager.Instance.isCaseFileActive)
        {
            CaseFileManager.Instance.ShowErrorUI();
            return;
        }

        if (other.CompareTag("EvidenceSocket"))
        {
            EvidenceItem evidence = other.GetComponentInParent<EvidenceItem>();

            if (evidence != null && !evidence.isLabeled)
            {
                evidence.LabelEvidence(markerNumber);
                isPlaced = true;

                if (audioSource && placeSound)
                    audioSource.PlayOneShot(placeSound);

                CaseFileManager.Instance.UpdateCaseFile(markerNumber, evidence.evidenceName);
            }
        }
    }

}
