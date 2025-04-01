#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using TMPro;
using System.Collections;
using JetBrains.Annotations;
using UnityEngine.SceneManagement;

public class GameManager : SingletonMonoBehavior<GameManager>
{
    [SerializeField] private int maxLives = 3;
    [SerializeField] private GameObject[] hearts;
    [SerializeField] private TextMeshProUGUI gameOver;
    public TextMeshProUGUI scoreText; // Text for updating coin count
    private int score = 0;

    [SerializeField] private PlayerController mario; //referring to the mario character
    [SerializeField] private Barrel barrel;

    [SerializeField] private float countdownTime = 90f; // Total countdown time in seconds
    [SerializeField] private TextMeshProUGUI timerText; // Reference to the UI text for the timer
    private float currentTime; // Tracks the current time left in the countdown
    private bool isTimerPaused = false;

    private bool isMarioBig = false;
    private bool isImmune = false; //Track whether Mario is immune to the barrels
    [SerializeField] private InputManager inputManager;
    [SerializeField] private GameObject settingsMenu;
    private bool isSettingsMenuActive;
    public bool IsSettingsMenuActive => isSettingsMenuActive;

    private bool isGameOver = false; // Flag to track if the game over sequence has started

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(settingsMenu);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        inputManager.OnSettingsMenu.AddListener(ToggleSettingsMenu);
        DisableSettingsMenu();

        InitializeGame(); 
    }


    void Start()
    {
        UpdateScoreUI();
    }

    void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "" + score;
        }
    }

    public void AddScore()
    {
        score++;
        UpdateScoreUI();
    }

    // private void OnEnable()
    // {
    //     Debug.Log("Yo estoy aqui");
    //     //Update the array of hearts
    //     UpdateHeartsUI();
    //     gameOver.gameObject.SetActive(false); // Hide the Game Over text initially
    //     isGameOver = false;

    //     currentTime = countdownTime;
    //     UpdateTimerUI(); //Updates timer text on the screen
    // }
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
        // Countdown timer logic
        if (!isSettingsMenuActive && !isTimerPaused && currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            UpdateTimerUI();
        }

        if (currentTime <= 0 && !isSettingsMenuActive)
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
        if (!isGameOver)
        {
            StartCoroutine(GameOverSequence());
        }
    }
    public void RemoveLife()
    {
        FindFirstObjectByType<CameraShake>().StartShake();

        if (isImmune)
        {
            return; // Don't do anything when Mario is immune
        }
        if (AudioManager.instance != null && maxLives != 1)
        {
            AudioManager.instance.sfxSource.PlayOneShot(AudioManager.instance.loseLifeClip);
            AudioManager.instance.sfxSource.PlayOneShot(AudioManager.instance.kongClip);
        }
        maxLives--;
        UpdateHeartsUI();

        // Start Game Over sequence if lives are 0
        if (maxLives <= 0 && !isGameOver)
        {
            StartCoroutine(GameOverSequence());
        }
        else
        {
            StartCoroutine(ResetWithImmunity());
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

    private IEnumerator ResetWithImmunity()
    {
        isImmune = true; //set immune to true
        mario.EnableImmunityEffect();
        mario.ResetPosition(); //reset his position

        yield return new WaitForSeconds(3f); //wait 3 second for immunity;
        isImmune = false;
        mario.DisableImmunityEffect();
    }

    private IEnumerator GameOverSequence()
    {
        if (isGameOver) yield break; // Prevent multiple calls
        Debug.Log("GameOverSequence triggered");
        isGameOver = true; // Set the flag to true
        PauseTimer();

        if (AudioManager.instance != null)
        {
            AudioManager.instance.ambienceSource.Stop();
            AudioManager.instance.PlaySound(AudioManager.instance.gameOverClip);
        }

        Time.timeScale = 0f; // Freeze the time

        // Show the Game Over UI
        gameOver.gameObject.SetActive(true);

        // Wait for 4 seconds
        yield return new WaitForSecondsRealtime(4f);

        // Unfreeze time and transition to the main menu
        Time.timeScale = 1f;

        // Handle Next Scene
        Debug.Log("Back to Main Menu");
        SceneHandler.instance.LoadMenuScene();
    }

    private void ToggleSettingsMenu()
    {
        if (isSettingsMenuActive)
        {
            AudioManager.instance.ambienceSource.Play();
            DisableSettingsMenu();
        }
        else EnableSettingsMenu();
    }
    private void EnableSettingsMenu()
    {
        if (AudioManager.instance != null)
        {
            AudioManager.instance.ambienceSource.Stop();
            AudioManager.instance.sfxSource.PlayOneShot(AudioManager.instance.pauseClip);
        }
        Time.timeScale = 0f;
        settingsMenu.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        isSettingsMenuActive = true;
    }

    public void DisableSettingsMenu()
    {
        
        AudioManager.instance.ambienceSource.Play();      
        Time.timeScale = 1f;
        settingsMenu.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        isSettingsMenuActive = false;
    }

    public void PauseTimer()
    {
        isTimerPaused = true; // Pause the timer
        StartCoroutine(WinSequence()); // Trigger win sequence when timer is paused
    }
    private void InitializeGame()
{
    Debug.Log("Initializing Game");
    UpdateHeartsUI();
    gameOver.gameObject.SetActive(false); // Hide the Game Over text initially
    isGameOver = false;

    currentTime = countdownTime;
    UpdateTimerUI(); // Updates timer text on the screen
}

    private IEnumerator WinSequence()
    {
        Debug.Log("WinSequence started");
        // yield return new WaitForSeconds(1f); 

        Scene currentScene = SceneManager.GetActiveScene();

        if (currentScene.buildIndex == 1) // if current is scene1
        {
            yield return new WaitForSeconds(4f); 
            if (AudioManager.instance != null)
            {
                // AudioManager.instance.ambienceSource.Stop(); // Stop ambient sound if needed
                AudioManager.instance.PlayAmbientSound(AudioManager.instance.ambientClip_Level2); // Play Level 2 ambient sound
            }
            if (SceneHandler.instance != null)
            {
                SceneHandler.instance.LoadLevel2Scene(); // load scene2
            }
            else
            {
                Debug.LogError("SceneHandler instance not found");
            }
        }
        else if (currentScene.buildIndex == 2) // If current is scene2
        {
            if (SceneHandler.instance != null)
            {
                if (AudioManager.instance != null)
                {
                    AudioManager.instance.StopAmbientSound(); // Stop ambient sound if needed
                    AudioManager.instance.PlaySound(AudioManager.instance.stageClearClip); // Play start sound
                }
                SceneHandler.instance.LoadWinScene(); // load the win scene
            }
            else
            {
                Debug.LogError("SceneHandler instance not found");
            }

            yield return new WaitForSeconds(10f);

            if (SceneHandler.instance != null)
            {
                SceneHandler.instance.LoadMenuScene(); // load the main menu
            }
            else
            {
                Debug.LogError("SceneHandler instance not found");
            }
        }
        else
        {
            Debug.LogError("Unexpected scene index: " + currentScene.buildIndex);
        }
        
    }

    public void QuitGame()
    {
# if UNITY_EDITOR
        EditorApplication.isPlaying = false;
# else
        Application.Quit();
# endif
    }


}
