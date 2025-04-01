using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{
    public static SceneHandler instance { get; private set; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadMenuScene()
    {
        SceneManager.LoadScene(0);
    }

    public void LoadLevel1Scene()
    {
        SceneManager.LoadScene(1);
    }

    public void LoadLevel2Scene()
    {
        SceneManager.LoadScene(2);
    }

    public void LoadWinScene()
    {
        SceneManager.LoadScene(3);
    }
}
