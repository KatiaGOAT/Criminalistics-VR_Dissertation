using UnityEngine;
using System.Collections;
using UnityEngine.XR.Interaction.Toolkit;


public class TimelinePuzzleManager : MonoBehaviour
{
    [Header("Puzzle Pieces & Sockets")]
    public GameObject[] puzzlePieces; // PP1–PP7
    public GameObject[] tickUIs;      // one per socket
    public GameObject[] crossUIs;     // one per socket

    public GameObject[] hintUIs;       // optional hint objects near sockets
    public GameObject[] socketObjects; // socket cubes or areas where pieces are placed


    [Header("Audio")]
    public AudioSource audioSource;   // shared AudioSource
    public AudioClip correctSound;
    public AudioClip errorSound;
    public AudioClip finalCompletionSound;

    [Header("UI")]
    public GameObject nextCanvasUI;
    public GameObject completionUI; // hidden until puzzle complete

    private bool[] isCorrect;         // tracks which sockets are correct
    private bool[] hasPiece;          // tracks if socket currently filled
    private Coroutine[] errorLoops;   // for looping wrong sound

    [Header("XR References")]
    public XRInteractionManager xrManager;


    void Start()
    {
        int count = puzzlePieces.Length;
        isCorrect = new bool[count];
        hasPiece = new bool[count];
        errorLoops = new Coroutine[count];

        // Hide all tick/cross and continue button at start
        foreach (var t in tickUIs) if (t) t.SetActive(false);
        foreach (var c in crossUIs) if (c) c.SetActive(false);

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
            //if (tickUIs[index]) StartCoroutine(ShowTickTemporary(tickUIs[index]));
            if (tickUIs[index]) tickUIs[index].SetActive(true);  // Keep tick visible

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
        isCorrect[index] = false; // reset correctness state

        // Stop wrong sound if it was looping
        if (errorLoops[index] != null)
        {
            StopCoroutine(errorLoops[index]);
            errorLoops[index] = null;
        }

        // Hide both cross and tick when piece removed
        if (crossUIs[index]) crossUIs[index].SetActive(false);
        if (tickUIs[index]) tickUIs[index].SetActive(false);
    }


    private IEnumerator LoopErrorSound(int index)
    {
        while (true)
        {
            audioSource.PlayOneShot(errorSound);
            yield return new WaitForSeconds(errorSound.length);
        }
    }
    private void CheckPuzzleCompletion()
    {
        // Check if all pieces are correctly placed
        foreach (bool correct in isCorrect)
        {
            if (!correct) return;
        }

        audioSource.PlayOneShot(finalCompletionSound);

        Debug.Log("All puzzle pieces correct! Puzzle solved.");
        if (nextCanvasUI) nextCanvasUI.SetActive(false);
        if (completionUI) completionUI.SetActive(true);

    }

    // Hides all puzzle-related elements: pieces, ticks, crosses, sockets, and hints
    public void HideAllPuzzleElements()
    {
        // Hide all puzzle pieces
        foreach (var piece in puzzlePieces)
            if (piece) piece.SetActive(false);

        // Hide all tick indicators
        foreach (var tick in tickUIs)
            if (tick) tick.SetActive(false);

        // Hide all cross indicators
        foreach (var cross in crossUIs)
            if (cross) cross.SetActive(false);

        // Hide all socket cubes or colliders
        foreach (var socket in socketObjects)
            if (socket) socket.SetActive(false);

        // Hide all hint objects
        foreach (var hint in hintUIs)
            if (hint) hint.SetActive(false);


        Debug.Log("All puzzle elements (pieces, ticks, crosses, sockets, hints) hidden.");
    }

}
