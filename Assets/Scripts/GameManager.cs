using UnityEngine;
using TMPro;
using System.Collections;

public class GameManager : SingletonMonoBehavior<GameManager>
{
    [SerializeField] private int maxLives = 3;
    [SerializeField] private GameObject[] hearts;
    [SerializeField] private TextMeshProUGUI gameOver;

    [SerializeField] private PlayerController mario; //referring to the mario character
    [SerializeField] private Barrel barrel;

    [SerializeField] private float countdownTime = 90f; // Total countdown time in seconds
    [SerializeField] private TextMeshProUGUI timerText; // Reference to the UI text for the timer
    private float currentTime; // Tracks the current time left in the countdown

    private bool isMarioBig=false;
    private void OnEnable()
    {
        //Update the array of hearts
        UpdateHeartsUI();
        gameOver.gameObject.SetActive(false); // Hide the Game Over text initially

        currentTime = countdownTime;
        UpdateTimerUI(); //Updates timer text on the screen
    }
    public bool getMarioBig()
    {
        return isMarioBig;
    }
    public void setMarioBig(bool big)
    {
        isMarioBig = big;
    }

    private void Update()
    {
        if (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            UpdateTimerUI();
        }
        if (currentTime <= 0)
        {
            currentTime = 0;
            OnCountdownEnd();
        }
    }
    private void UpdateTimerUI()
    {
        float displayTime = Mathf.Max(currentTime, 0);

        // Calculate minutes and seconds from the current time
        int minutes = Mathf.FloorToInt(displayTime / 60);
        int seconds = Mathf.FloorToInt(displayTime % 60);

        // Format the time as "minutes:seconds" with leading zero for seconds
        string formattedTime = string.Format("{0}:{1:00}", minutes, seconds);

        // Update the timer text
        if (timerText != null)
        {
            timerText.text = formattedTime;
        }
    }
    private void OnCountdownEnd()
    {
        StartCoroutine(GameOverSequence());
    }
    public void RemoveLife()
    {
        FindFirstObjectByType<CameraShake>().StartShake();

        maxLives--;
        UpdateHeartsUI();
        // game over UI if maxLives < 0, then exit to main menu after delay
        if (maxLives <= 0)
        {
            // Show Game Over text and return to the main menu after a delay
            StartCoroutine(GameOverSequence());
        }
        else
        {
            mario.ResetPosition();
        }


    }
    private void UpdateHeartsUI()
    {
        // Loop through the hearts array and enable/disable hearts based on maxLives
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].SetActive(i < maxLives);
        }
    }

    private IEnumerator GameOverSequence()
    {
        Time.timeScale = 0f; //freezing the time

        // Show the Game Over UI
        gameOver.gameObject.SetActive(true);

        // Wait for 1.5 seconds
        yield return new WaitForSecondsRealtime(1.5f);

        // Unfreeze time and transition to the main menu
        Time.timeScale = 1f;

        // Handle Next Scene
        Debug.Log("Back to Main Menu");
        // SceneHandler.Instance.LoadMenuScene();
    }
}
