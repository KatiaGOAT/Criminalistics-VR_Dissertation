using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TimelinePuzzleSocket : MonoBehaviour
{
    [Header("Socket Settings")]
    public int socketIndex; // 0 for S1, 1 for S2, etc.

    public TimelinePuzzleManager timelinePuzzleManager;

    // Called automatically from XR Socket Interactor event "Select Entered"
    public void OnPiecePlaced(SelectEnterEventArgs args)
    {
        if (timelinePuzzleManager == null) return;

        GameObject piece = args.interactableObject.transform.gameObject;
        string expectedTag = "PP" + (socketIndex + 1);

        if (piece.CompareTag(expectedTag))
            timelinePuzzleManager.PiecePlaced(socketIndex, true, piece, this);
        else
            timelinePuzzleManager.PiecePlaced(socketIndex, false, piece, this);
    }

    // Called automatically from XR Socket Interactor event "Select Exited"
    public void OnPieceRemoved(SelectExitEventArgs args)
    {
        if (timelinePuzzleManager == null) return;

        timelinePuzzleManager.PieceRemoved(socketIndex);
    }
}
