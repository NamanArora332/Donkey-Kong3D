using UnityEngine;

public class InputManager : MonoBehaviour
{
    // Singleton pattern to ensure only one InputManager exists
    public static InputManager Instance { get; private set; }

    void Awake()
    {
        // Set up singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keep this object across scenes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicates
        }
    }

    // Methods to check input states
    public bool GetLeftPressed()
    {
        return Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow);
    }

    public bool GetRightPressed()
    {
        return Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow);
    }

    public bool GetJumpPressed()
    {
        
        return Input.GetKeyDown(KeyCode.Space); // Use KeyDown for single press
    }
    public bool GetUpPressed()
    {
        
        return Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);
    }
    public bool GetDownPressed()
    {
        return Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow);
    }
}