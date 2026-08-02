using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        if (Application.CanStreamedLevelBeLoaded("PlayScene"))
        {
            SceneManager.LoadScene("PlayScene");
        }
        else
        {
            SceneManager.LoadScene(1);
        }
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game requested!");
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
