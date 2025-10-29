using UnityEngine;
using TMPro;
using System.Collections;

public class InterrogationManager : MonoBehaviour
{
    [System.Serializable]
    public class DialogueLine
    {
        //public string speakerName;                  // Investigator / Suspect
        public string subtitleText;
        public AudioClip voiceClip;
        public float delayAfterLine = 1f;           // short pause between lines
    }

    [Header("Dialogue Sequence")]
    public DialogueLine[] lines;

    [Header("UI Elements")]
    public TextMeshProUGUI subtitleTextUI;          // Assign your subtitle TMP here
    public GameObject interrogationUI;
    public GameObject endUI;                        // "Case Closed" or "End" screen
    public GameObject interrogationSetup;

    [Header("Audio")]
    public AudioSource dialogueAudio;               // Assign an AudioSource component

    [Header("NPC Animator")]
    public Animator suspectAnimator;                // The suspect npc Animator

    private bool dialogueStarted = false;


    void Start()
    {
        if (endUI) endUI.SetActive(false);
        
    }

    public void BeginInterrogation()
    {
        if (dialogueStarted) return; // prevent double click
        dialogueStarted = true;

        Debug.Log("Interrogation starting in 3 seconds...");
        StartCoroutine(StartAfterDelay(3f));
    }

    // Waits for delay, then begins dialogue
    private IEnumerator StartAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        StartCoroutine(PlayDialogueSequence());
    }

    IEnumerator PlayDialogueSequence()
    {
        foreach (DialogueLine line in lines)
        {
            if (line.voiceClip != null)
            {
                // Play voice clip
                dialogueAudio.clip = line.voiceClip;
                dialogueAudio.Play();

                // If the clip name includes "suspect", trigger talking animation
                if (dialogueAudio.clip.name.ToLower().Contains("suspect"))
                {
                    suspectAnimator.SetBool("isTalking", true);
                }
                else
                {
                    suspectAnimator.SetBool("isTalking", false);
                }
            }

            // Update subtitle text
            if (subtitleTextUI != null)
                subtitleTextUI.text = line.subtitleText;

            // Wait for the clip to finish, plus a short pause
            yield return new WaitForSeconds(line.voiceClip.length + line.delayAfterLine);
        }

        yield return new WaitForSeconds(1.5f);

        // After all lines are done
        suspectAnimator.SetBool("isTalking", false); // back to idle

        if (subtitleTextUI != null) subtitleTextUI.text = "";
        //if (interrogationUI) interrogationUI.SetActive(false);
        if (interrogationSetup) interrogationSetup.SetActive(false);

        if (endUI) endUI.SetActive(true);

        Debug.Log("Dialogue complete — case closed.");
    }
}
