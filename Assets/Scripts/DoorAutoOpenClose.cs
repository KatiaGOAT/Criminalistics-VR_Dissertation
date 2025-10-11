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

    [Header("UI References")]
    public GameObject warningUI;           // UI to show when PPE not equipped

    private void OnTriggerEnter(Collider other)
    {
        //if (other.CompareTag("Player") && !isOpen)
        //{
        //    StartCoroutine(OpenDoor());
        //}

        // Only react when the player enters trigger zone
        if (other.CompareTag("Player"))
        {
            // Check PPE status before opening door
            if (!PPEManager.Instance.IsPPEComplete())
            {
                // PPE not equipped → show warning UI, block door
                if (warningUI != null)
                    warningUI.SetActive(true);

                Debug.Log("Access denied: PPE not equipped.");
                return;
            }

            // PPE is complete → hide warning (if shown) and open the door
            if (warningUI != null)
                warningUI.SetActive(false);

            if (!isOpen)
                StartCoroutine(OpenDoor());
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
}
