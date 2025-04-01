
using System.Security.AccessControl;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class MainMenu : MonoBehaviour
{
    public Button playButton;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (playButton != null) {
            playButton.onClick.AddListener(OnPlayButtonClicked);
        
        }
        if (AudioManager.instance != null)
        {
            Debug.Log("Loading Menu Sound");
            AudioManager.instance.PlaySound(AudioManager.instance.introClip); // Play intro sound
        }
        
    }

        void OnPlayButtonClicked()
    {
        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlaySound(AudioManager.instance.gameStartClip); // Play start sound
            Debug.Log("Loading Level1 Sound");
            AudioManager.instance.PlayAmbientSound(AudioManager.instance.ambientClip_Level1); // Play Level 1 ambient sound
        }

        if (SceneHandler.instance != null)
        {
            StartCoroutine(LoadLevel1WithDelay(1f)); // Add a 1-second delay before loading the scene
        }
        else
        {
            Debug.LogError("Scene Handler instance not found");
        }
    }

    private IEnumerator LoadLevel1WithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneHandler.instance.LoadLevel1Scene();
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit Game");
    }
}
