using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneTransitionManager : SingletonMonoBehavior<SceneTransitionManager>
{
    [SerializeField] private float transitionDelay = 4f; // Delay before transitioning to the next scene

    public void StartWinSequence(int currentSceneIndex)
    {
        Debug.Log($"StartWinSequence called for scene index: {currentSceneIndex}");
        StartCoroutine(WinSequence(currentSceneIndex));
    }

    private IEnumerator WinSequence(int currentSceneIndex)
    {
        Debug.Log("WinSequence started");

        // Play ambient sounds or other effects here if needed
        if (AudioManager.instance != null)
        {
            AudioManager.instance.StopAmbientSound(); // Stop current ambient sound
        }

        yield return new WaitForSeconds(transitionDelay);

        // Transition to the next scene based on the current scene index
        if (currentSceneIndex == 1) // If current is scene1
        {
            if (AudioManager.instance != null)
            {
                AudioManager.instance.PlayAmbientSound(AudioManager.instance.ambientClip_Level2); // Play Level 2 ambient sound
            }
            if (SceneHandler.instance != null)
            {
                SceneHandler.instance.LoadLevel2Scene(); // Load scene2
            }
            else
            {
                Debug.LogError("SceneHandler instance not found");
            }
        }
        else if (currentSceneIndex == 2) // If current is scene2
        {
            if (AudioManager.instance != null)
            {
                AudioManager.instance.PlaySound(AudioManager.instance.stageClearClip); // Play stage clear sound
            }
            if (SceneHandler.instance != null)
            {
                SceneHandler.instance.LoadWinScene(); // Load the win scene
            }
            else
            {
                Debug.LogError("SceneHandler instance not found");
            }

            yield return new WaitForSeconds(10f);

            if (SceneHandler.instance != null)
            {
                SceneHandler.instance.LoadMenuScene(); // Load the main menu
            }
            else
            {
                Debug.LogError("SceneHandler instance not found");
            }
        }
        else
        {
            Debug.LogError("Unexpected scene index: " + currentSceneIndex);
        }
    }
}