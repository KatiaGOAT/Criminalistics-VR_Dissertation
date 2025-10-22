using UnityEngine;
using System.Collections;

public class TimelinePuzzleManager : MonoBehaviour
{
    [Header("Puzzle Pieces & Sockets")]
    public GameObject[] puzzlePieces; // PP1–PP7
    public GameObject[] tickUIs;      // one per socket
    public GameObject[] crossUIs;     // one per socket

    [Header("Audio")]
    public AudioSource audioSource;   // shared AudioSource
    public AudioClip correctSound;
    public AudioClip errorSound;

    [Header("UI")]
    public GameObject continueButton; // hidden until puzzle complete
    public GameObject instructionUI;  // initial instruction panel

    [Header("Settings")]
    public float tickFadeDelay = 3f;  // hide tick after 3 seconds

    private bool[] isCorrect;         // tracks which sockets are correct
    private bool[] hasPiece;          // tracks if socket currently filled
    private Coroutine[] errorLoops;   // for looping wrong sound

    void Start()
    {
        int count = puzzlePieces.Length;
        isCorrect = new bool[count];
        hasPiece = new bool[count];
        errorLoops = new Coroutine[count];

        // Hide all tick/cross and continue button at start
        foreach (var t in tickUIs) if (t) t.SetActive(false);
        foreach (var c in crossUIs) if (c) c.SetActive(false);
        if (continueButton) continueButton.SetActive(false);
    }

    // Called by PuzzleSocket when piece is placed
    public void PiecePlaced(int index, bool correct, GameObject piece, TimelinePuzzleSocket socket)
    {
        if (index < 0 || index >= isCorrect.Length) return;

        hasPiece[index] = true;

        if (correct)
        {
            // Correct piece
            isCorrect[index] = true;
            if (tickUIs[index]) StartCoroutine(ShowTickTemporary(tickUIs[index]));
            if (crossUIs[index]) crossUIs[index].SetActive(false);

            if (audioSource && correctSound)
                audioSource.PlayOneShot(correctSound);

            // stop error sound if playing
            if (errorLoops[index] != null)
            {
                StopCoroutine(errorLoops[index]);
                errorLoops[index] = null;
            }

            CheckPuzzleCompletion();
        }
        else
        {
            // Wrong piece
            isCorrect[index] = false;

            if (crossUIs[index]) crossUIs[index].SetActive(true);

            // start looping error sound
            if (audioSource && errorSound)
            {
                errorLoops[index] = StartCoroutine(LoopErrorSound(index));
            }
        }
    }

    // Called when a piece is removed
    public void PieceRemoved(int index)
    {
        if (index < 0 || index >= isCorrect.Length) return;

        hasPiece[index] = false;

        // Stop wrong sound if it was looping
        if (errorLoops[index] != null)
        {
            StopCoroutine(errorLoops[index]);
            errorLoops[index] = null;
        }

        // Hide cross if it was showing
        if (crossUIs[index]) crossUIs[index].SetActive(false);
    }

    private IEnumerator LoopErrorSound(int index)
    {
        while (true)
        {
            audioSource.PlayOneShot(errorSound);
            yield return new WaitForSeconds(errorSound.length);
        }
    }

    private IEnumerator ShowTickTemporary(GameObject tick)
    {
        tick.SetActive(true);
        yield return new WaitForSeconds(tickFadeDelay);
        tick.SetActive(false);
    }

    private void CheckPuzzleCompletion()
    {
        // Check if all pieces are correctly placed
        foreach (bool correct in isCorrect)
        {
            if (!correct) return;
        }

        Debug.Log("All puzzle pieces correct! Puzzle solved.");
        if (continueButton) continueButton.SetActive(true);
    }
}
