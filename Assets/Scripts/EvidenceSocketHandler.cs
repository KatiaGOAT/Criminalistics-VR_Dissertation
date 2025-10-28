using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// Handles what happens when an evidence marker is placed in the socket.
/// Called from the XR Socket Interactor's "Select Entered" event.
/// </summary>
public class EvidenceSocketHandler : MonoBehaviour
{
    [Header("Evidence Reference")]
    [Tooltip("Assign the EvidenceItem object this socket belongs to.")]
    public EvidenceItem evidence;

    /// <summary>
    /// Called by the XR Socket Interactor event when a marker is placed.
    /// </summary>
    public void OnMarkerPlaced(SelectEnterEventArgs args)
    {
        EvidenceMarker marker = args.interactableObject.transform.GetComponent<EvidenceMarker>();
        if (marker == null) return;

        if (!CaseFileManager.Instance.IsCaseFileActive())
        {
            // Show warning UI & sound
            CaseFileManager.Instance.ShowErrorUI();

            // Reset this one marker both via CaseFileManager and locally
            CaseFileManager.Instance.ResetSingleMarker(marker.gameObject);
            marker.ResetToStartPosition();

            Debug.Log($"{marker.name} tried to be placed before starting the case — reset to start.");

            return;
        }

        // Label evidence only once
        if (!evidence.isLabeled)
        {
            evidence.LabelEvidence(marker.markerNumber);
            marker.isPlaced = true;

            // Play placement sound
            if (marker.audioSource && marker.placeSound)
                marker.audioSource.PlayOneShot(marker.placeSound);

            // Update the Case File UI
            CaseFileManager.Instance.UpdateCaseFile(marker.markerNumber, evidence.evidenceName);
        }
    }
}
