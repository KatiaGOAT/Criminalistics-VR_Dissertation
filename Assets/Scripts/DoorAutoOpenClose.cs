using UnityEngine;
using System.Collections;

public class DoorAutoOpenClose : MonoBehaviour
{
    public Animator doorAnimator;
    public bool isOpen = false;

    public AudioSource sceneAudioSource;         // The AudioSource component
    public AudioClip doorSound;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isOpen)
        {
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
