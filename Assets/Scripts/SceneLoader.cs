using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    // Call this function from your button with the name of the target scene
    public void GoToStartScene()
    {
        SceneManager.LoadScene("1 StartOnboarding Scene");
    }
    public void GoToCrimeScene()
    {
        SceneManager.LoadScene("2 Main Crime Scene");
    }
    public void QuitApplication()
    {
        // Quit the application
        Application.Quit();

        // For testing in the Unity Editor, stop Play mode
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

}
