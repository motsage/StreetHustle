using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string sceneName;
    public void PlayGame()
    {
        // Replace "YourSceneNameHere" with the actual name of your scene
        // Example: SceneManager.LoadScene("Level1");
        SceneManager.LoadScene(sceneName);
    }

    public void ExitGame()
    {
        // In the editor this won’t quit, but in a built game it will
        Application.Quit();

#if UNITY_EDITOR
        // This makes the quit button also stop Play Mode inside the Unity editor
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}