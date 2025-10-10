using UnityEngine;
using TMPro;
using System.Collections.Generic;

/// <summary>
/// Central manager for tracking all labeled evidence.
/// Updates the Case File UI with progress and names of evidence.
/// Singleton pattern so other scripts can easily call it.
/// </summary>
public class CaseFileManager : MonoBehaviour
{
    // Static instance for global access
    public static CaseFileManager Instance;

    // Struct-style class to store marker/evidence pair info
    [System.Serializable]
    public class LabeledEvidence
    {
        public int markerNumber;
        public string evidenceName;
    }

    [Header("UI Elements")]
    public TextMeshProUGUI caseFileText;   // Text area showing list of collected evidence
    public TextMeshProUGUI progressText;   // Text showing how many are collected

    [Header("Settings")]
    public int totalEvidenceCount = 7;     // Total evidence pieces in the scene
    private int collectedCount = 0;        // Current progress count

    [Header("Collected Data")]
    public List<LabeledEvidence> collectedEvidence = new List<LabeledEvidence>(); // Stores what’s been found

    void Awake()
    {
        // Simple Singleton pattern — ensures only one manager exists
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    /// <summary>
    /// Called by EvidenceMarker.cs when a new evidence is labeled.
    /// Adds entry to the list and refreshes the UI.
    /// </summary>
    public void UpdateCaseFile(int markerNumber, string evidenceName)
    {
        // Create new record
        collectedEvidence.Add(new LabeledEvidence
        {
            markerNumber = markerNumber,
            evidenceName = evidenceName
        });

        // Update counts
        collectedCount++;
        RefreshUI();

        // If all evidence collected, trigger end-of-task UI
        if (collectedCount >= totalEvidenceCount)
        {
            ShowCompletionUI();
        }
    }

    /// <summary>
    /// Updates the on-screen Case File UI (list + progress).
    /// </summary>
    void RefreshUI()
    {
        string list = "";

        // Loop through all collected evidence and display
        foreach (var e in collectedEvidence)
        {
            list += $"Marker {e.markerNumber}: {e.evidenceName}\n";
        }

        // Update UI text fields
        if (caseFileText)
            caseFileText.text = list;

        if (progressText)
            progressText.text = $"Evidence Collected: {collectedCount}/{totalEvidenceCount}";
    }

    /// <summary>
    /// Displays a message or triggers your "Task Complete" screen.
    /// </summary>
    void ShowCompletionUI()
    {
        Debug.Log("All evidence labeled. Proceed to the analysis room.");
        // Here you can show your completion UI panel or trigger a fade-out.
    }
}
