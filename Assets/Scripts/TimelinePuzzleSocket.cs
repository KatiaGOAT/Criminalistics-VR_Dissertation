using UnityEngine;

public class TimelinePuzzleSocket : MonoBehaviour
{
    [Header("Socket Settings")]
    public int socketIndex; // 0 for S1, 1 for S2, etc.

    private TimelinePuzzleManager timelinePuzzleManager;
    private AudioSource errorAudioSource; // to stop sound if removed

    private void OnTriggerEnter(Collider other)
    {
        if (timelinePuzzleManager == null) return;

        string expectedTag = "PP" + (socketIndex + 1);

        if (other.CompareTag(expectedTag))
        {
            //Correct piece
            timelinePuzzleManager.PiecePlaced(socketIndex, true, other.gameObject, this);
        }
        else if (other.CompareTag("PP1") || other.CompareTag("PP2") || other.CompareTag("PP3") ||
                 other.CompareTag("PP4") || other.CompareTag("PP5") || other.CompareTag("PP6") ||
                 other.CompareTag("PP7"))
        {
            // Wrong piece
            timelinePuzzleManager.PiecePlaced(socketIndex, false, other.gameObject, this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (timelinePuzzleManager == null) return;

        // Stop error sound and hide cross when wrong piece is removed
        timelinePuzzleManager.PieceRemoved(socketIndex);
    }
}
