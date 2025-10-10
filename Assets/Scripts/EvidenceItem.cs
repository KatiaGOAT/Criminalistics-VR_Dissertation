using UnityEngine;

/// <summary>
/// Attached to each Evidence object (e.g., Rope, Phone, Watch).
/// Stores its name, ID, and whether it's been labeled.
/// Also controls the small tick mark visual when labeled.
/// </summary>
public class EvidenceItem : MonoBehaviour
{
    [Header("Evidence Info")]
    public string evidenceID;          // Example: "E1"
    public string evidenceName;        // Example: "Victim’s Smartwatch"
    public bool isLabeled = false;     // Has the user placed a marker near this evidence yet?

    [HideInInspector] public int assignedMarkerNumber = -1; // Which marker was used for labeling

    [Header("Optional Visual Feedback")]
    public GameObject tickMark;        // A small tick/check icon next to the evidence

    // Called when the player successfully places a marker near this evidence.
    public void LabelEvidence(int markerNumber)
    {
        // Don’t label again if already labeled
        if (isLabeled) return;

        // Update internal data
        isLabeled = true;
        assignedMarkerNumber = markerNumber;

        // Turn on the tick/checkmark visual if it exists
        if (tickMark != null)
            tickMark.SetActive(true);
    }
}
