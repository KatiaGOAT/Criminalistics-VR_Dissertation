using UnityEngine;

/// <summary>
/// Simple data holder for each evidence marker.
/// Stores its number, sound, and initial position.
/// Placement logic is handled by the XR Socket Interactor via EvidenceSocketHandler.
/// </summary>
public class EvidenceMarker : MonoBehaviour
{
    [Header("Marker Info")]
    public int markerNumber;         // Unique marker ID (1–7)
    public bool isPlaced = false;    // True once it’s used to label evidence

    [Header("Audio Feedback")]
    public AudioClip placeSound;     // Sound played on successful placement
    public AudioSource audioSource;  // Audio source on the marker object

    private Vector3 startPos;
    private Quaternion startRot;

    void Start()
    {
        // Save initial transform data
        startPos = transform.position;
        startRot = transform.rotation;

        // Register this marker’s position in CaseFileManager if not already stored
        if (CaseFileManager.Instance != null &&
            !CaseFileManager.Instance.markerStartPositions.ContainsKey(gameObject))
        {
            CaseFileManager.Instance.markerStartPositions.Add(gameObject, startPos);
        }
    }

    /// <summary>
    /// Resets marker back to its initial table position and rotation.
    /// Called when labeling happens too early or needs to be undone.
    /// </summary>
    public void ResetToStartPosition()
    {
        transform.position = startPos;
        transform.rotation = startRot;

        if (audioSource != null)
            audioSource.Stop(); // stop any ongoing sound to avoid overlap

        isPlaced = false;

        Debug.Log($"{name} reset to starting position.");
    }
}
