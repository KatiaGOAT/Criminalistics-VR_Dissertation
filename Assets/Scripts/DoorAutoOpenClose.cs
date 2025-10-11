using UnityEngine;
using System.Collections;

public class DoorAutoOpenClose : MonoBehaviour
{
    [Header("Door Animation")]
    public Animator doorAnimator;
    public bool isOpen = false;

    [Header("Audio")]
    public AudioSource sceneAudioSource;         // The AudioSource component
    public AudioClip doorSound;
    public AudioClip warningSound;

    [Header("UI References")]
    public GameObject warningUI;           // UI to show when PPE not equipped

    private Coroutine hideWarningRoutine;  // To manage multiple triggers safely

    private void OnTriggerEnter(Collider other)
    {
        // Only react when the player enters trigger zone
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered door trigger zone.");

            // Check if PPEManager exists
            if (PPEManager.Instance == null)
            {
                Debug.LogError("PPEManager not found in scene! Cannot check PPE status.");
                return;
            }

            // Log current PPE status
            bool isComplete = PPEManager.Instance.IsPPEComplete();
            Debug.Log($"PPE Status Checked → Is Complete: {isComplete}");

            // Check PPE status before opening door
            if (!isComplete)
            {
                Debug.Log("PPE NOT complete. Door will NOT open.");

                // PPE not equipped → show warning UI, block door
                if (warningUI != null)
                {
                    Debug.Log("Showing warning UI: 'Please wear PPE before entering.'");
                    warningUI.SetActive(true);

                    // Play warning sound
                    if (sceneAudioSource && warningSound)
                    {
                        sceneAudioSource.PlayOneShot(warningSound);
                    }

                    // Start coroutine to hide after 3 seconds
                    if (hideWarningRoutine != null)
                    {
                        Debug.Log("Stopping existing hide coroutine to restart timer.");
                        StopCoroutine(hideWarningRoutine);
                    }

                    hideWarningRoutine = StartCoroutine(HideWarningAfterDelay(3f));
                }
                else
                {
                    Debug.LogWarning("Warning UI not assigned in Inspector!");
                }

                Debug.Log("Access denied: PPE not equipped.");
                return;
            }

            // PPE is complete → hide warning (if shown) and open the door
            Debug.Log("PPE complete! Door will open now.");

            if (warningUI != null && warningUI.activeSelf)
            {
                Debug.Log("Hiding any existing warning UI.");
                warningUI.SetActive(false);
            }

            if (!isOpen)
            {
                Debug.Log("Starting OpenDoor() coroutine.");
                StartCoroutine(OpenDoor());
            }
            else
            {
                Debug.Log("Door is already open; skipping animation.");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && isOpen)
        {
            StartCoroutine(CloseDoor());
        }
    }

    IEnumerator OpenDoor()
    {
        Debug.Log("Door opening...");
        doorAnimator.Play("Opening");

        if (sceneAudioSource && doorSound)
            sceneAudioSource.PlayOneShot(doorSound);

        isOpen = true;
        yield return new WaitForSeconds(1f);
    }

    IEnumerator CloseDoor()
    {
        Debug.Log("Door closing...");
        doorAnimator.Play("Closing");

        if (sceneAudioSource && doorSound)
            sceneAudioSource.PlayOneShot(doorSound);

        isOpen = false;
        yield return new WaitForSeconds(0.5f);
    }

    //Coroutine to hide the warning message after a delay
    IEnumerator HideWarningAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (warningUI != null)
            warningUI.SetActive(false);
    }
}
