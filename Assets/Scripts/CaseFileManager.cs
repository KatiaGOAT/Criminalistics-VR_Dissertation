using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class CaseFileManager : MonoBehaviour
{

    // Manages evidence tracking, UI updates, and onboarding flow in the VR crime scene. 
    // Handles start activation, success/error sounds, and marker resets if labeling too early.

    //Singleton for easy global access
    public static CaseFileManager Instance;

    [Header("Case File State")]
    public bool isCaseFileActive = false;     // True only after player presses "Start"

    [Header("UI Elements")]
    
    public GameObject completionUI;           // UI shown when all evidence collected
    public GameObject errorUI;                // UI shown when labeling too early

    [Header("Case File Labels")]
    public List<TextMeshProUGUI> evidenceTextFields;  // 7 TMP text fields next to labels (1–7)

    [Header("Evidence Tracking")]
    public int totalEvidenceCount = 7;        // Total evidence in this scene
    private int collectedCount = 0;           // Counter
    public List<LabeledEvidence> collectedEvidence = new List<LabeledEvidence>();

    [Header("Markers Reference")]
    public List<GameObject> evidenceMarkers;  // Assign all 7 evidence marker objects here in Inspector
    private Dictionary<GameObject, Vector3> markerStartPositions = new Dictionary<GameObject, Vector3>();

    [Header("Audio Feedback")]
    public AudioSource audioSource;           // Add an AudioSource to play UI sounds
    //public AudioClip evidenceTickSound;       // Tick sound when evidence labeled
    public AudioClip errorSound;              // Error beep when labeling too early

    private Coroutine hideErrorRoutine;

    //Class to store evidence info
    [System.Serializable]
    public class LabeledEvidence
    {
        public int markerNumber;
        public string evidenceName;
    }

    void Awake()
    {
        // --- Singleton setup ---
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        // --- Save starting positions of all markers ---
        foreach (var marker in evidenceMarkers)
        {
            if (marker != null)
                markerStartPositions[marker] = marker.transform.position;
        }
    }

    //Called by Start Button (through OnClick event)
    public void ActivateCaseFile()
    {
        isCaseFileActive = true;
        Debug.Log("Case File Activated. Player can now start labeling evidence.");
    }

    //Called when player labels an evidence correctly
    public void UpdateCaseFile(int markerNumber, string evidenceName)
    {
        // Create new record
        collectedEvidence.Add(new LabeledEvidence
        {
            markerNumber = markerNumber,
            evidenceName = evidenceName
        });

        collectedCount++;

        //Update corresponding TMP field (based on marker number)
        int index = markerNumber - 1;
        if (index >= 0 && index < evidenceTextFields.Count && evidenceTextFields[index] != null)
        {
            evidenceTextFields[index].text = evidenceName;
        }

        //Play tick sound
        //if (audioSource && evidenceTickSound)
        //    audioSource.PlayOneShot(evidenceTickSound);

        Debug.Log($"Marker {markerNumber} linked to {evidenceName}");

        // If all evidence collected, show completion UI
        if (collectedCount >= totalEvidenceCount)
            ShowCompletionUI();
    }

    //Display "task complete" message when all evidence is labeled
    void ShowCompletionUI()
    {
        if (completionUI != null)
            completionUI.SetActive(true);

        Debug.Log("All evidence labeled. Proceed to the analysis room.");
    }

    //Show error message, play sound, and reset markers if player labels before pressing Start
    public void ShowErrorUI()
    {
        if (errorUI == null) return;

        errorUI.SetActive(true);
        Debug.Log("Case File not active. Please press Start before labeling evidence.");

        // Play error beep
        if (audioSource && errorSound)
            audioSource.PlayOneShot(errorSound);

        // Hide after 3 seconds
        if (hideErrorRoutine != null)
            StopCoroutine(hideErrorRoutine);
        hideErrorRoutine = StartCoroutine(HideErrorAfterDelay(3f));

        // Reset all markers to initial table positions
        ResetMarkersToStart();
    }

    IEnumerator HideErrorAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (errorUI != null)
            errorUI.SetActive(false);
    }

    //Move all markers back to their original start positions
    void ResetMarkersToStart()
    {
        foreach (var marker in evidenceMarkers)
        {
            if (marker != null && markerStartPositions.ContainsKey(marker))
            {
                marker.transform.position = markerStartPositions[marker];
            }
        }
        Debug.Log("All evidence markers reset to start positions.");
    }
}
