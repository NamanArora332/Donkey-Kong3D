using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    public float moveSpeed = 5f;  // Speed for moving left and right
    public float jumpForce = 10f;  // Force applied when jumping
    private bool isGrounded;      // Check if Mario is on the ground
    private Vector3 originalPosition;

    private bool canClimb = false;
    private Ladder ladder = null;
    private bool isBlinking = false;
    [SerializeField] private Renderer playerRenderer;
    private bool isClimbingSoundPlaying = false; // Flag to track climbing sound
    [SerializeField] private Animator animator;  //Get the animator of the child object
    private Material[] originalMaterials;

    void Start()
    {
        rb = GetComponent<Rigidbody>();  // Get the Rigidbody component
        //store Mario's original position
        originalPosition = gameObject.transform.position;
    }

    void Update()
    {
        // Horizontal movement using InputManager
        float moveDirection = 0f;
        if (InputManager.Instance.GetLeftPressed())
        {
            moveDirection = -1f;  // Move left
            transform.rotation = Quaternion.Euler(0, -90, 0);
        }
        else if (InputManager.Instance.GetRightPressed())
        {
            moveDirection = 1f;   // Move right
            transform.rotation = Quaternion.Euler(0, 90, 0);
        }
        Vector3 movement = new Vector3(moveDirection * moveSpeed, rb.linearVelocity.y, 0);  // Move along X-axis only
        rb.linearVelocity = movement;  // Apply velocity directly

        // Update Animator's "Speed" parameter
        float currentSpeed = Mathf.Abs(moveDirection); // Use absolute value for speed
        animator.SetFloat("Speed", currentSpeed);

        // Jumping using InputManager
        if (InputManager.Instance.GetJumpPressed() && isGrounded)
        {
            if (AudioManager.instance != null)
            {
                AudioManager.instance.sfxSource.PlayOneShot(AudioManager.instance.jumpClip);
            }
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);  // Jump upward
            isGrounded = false;  // Prevent double jumping

            // Trigger Jump animation
            animator.SetTrigger("isJumping");
        }
        if (ladder && canClimb && InputManager.Instance.GetUpPressed())
        {
            climbUpLadder();
        }
        if (ladder && canClimb && InputManager.Instance.GetDownPressed())
        {
            climbDownLadder();
        }
    }

    private void Awake()
    {
        if (playerRenderer != null)
        {
            // Store the original materials
            Material[] materials = playerRenderer.materials;
            originalMaterials = new Material[materials.Length];

            for (int i = 0; i < materials.Length; i++)
            {
                originalMaterials[i] = new Material(materials[i]);
            }
        }

    }

    // Detect if Mario is grounded
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Debug.Log("Grounded, can jump");
            isGrounded = true;
            animator.SetBool("isGrounded", true);
        }
        if (collision.gameObject.CompareTag("Peach"))
        {
            // Call the PauseTimer method in the GameManager
            GameManager.Instance.PauseTimer();
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Debug.Log("Not grounded, can't jump");
            isGrounded = false;
            animator.SetBool("isGrounded", false);
        }
    }

    public void EnableImmunityEffect()
    {
        StartCoroutine(BlinkingEffect());
    }

    public void DisableImmunityEffect()
    {
        StartCoroutine(BlinkingEffect());
        ResetMaterials();
        isBlinking = false;
    }

    public void ResetPosition()
    {
        gameObject.transform.position = originalPosition;
    }
    public void setLadder(Ladder ladder)
    {
        Debug.Log("Ladder set ");
        this.ladder = ladder;
    }
    public void setCanClimb(bool canClimb)
    {
        Debug.Log("Can climb set to " + canClimb);
        this.canClimb = canClimb;
    }
    public void climbUpLadder()
    {
        if (AudioManager.instance != null && !isClimbingSoundPlaying)
        {
            isClimbingSoundPlaying = true; // Set the flag to true
            AudioManager.instance.sfxSource.PlayOneShot(AudioManager.instance.climbClip);
            StartCoroutine(ResetClimbingSoundFlag(AudioManager.instance.climbClip.length)); // Reset flag after sound finishes
        }
        Debug.Log("Climbing up ladder");
        Transform upPosition = ladder.top;
        this.transform.position = upPosition.position;
    }
    public void climbDownLadder()
    {
        if (AudioManager.instance != null && !isClimbingSoundPlaying)
        {
            isClimbingSoundPlaying = true; // Set the flag to true
            AudioManager.instance.sfxSource.PlayOneShot(AudioManager.instance.climbClip);
            StartCoroutine(ResetClimbingSoundFlag(AudioManager.instance.climbClip.length)); // Reset flag after sound finishes
        }
        Debug.Log("Climbing down ladder");
        Transform downPosition = ladder.bottom;
        this.transform.position = downPosition.position;
    }

    private IEnumerator BlinkingEffect()
    {
        isBlinking = true;
        while (isBlinking)
        {
            ChangeMaterialColors(Color.red);
            yield return new WaitForSeconds(0.25f);

            ResetMaterials();
            yield return new WaitForSeconds(0.25f);
        }
    }

    // Coroutine to reset the climbing sound flag
    private IEnumerator ResetClimbingSoundFlag(float delay)
    {
        yield return new WaitForSeconds(delay); // Wait for the sound to finish
        isClimbingSoundPlaying = false; // Reset the flag
    }

    private void ChangeMaterialColors(Color color)
    {
        Material[] materials = playerRenderer.materials;

        for (int i = 0; i < materials.Length; i++)
        {
            materials[i].color = color;
        }

        playerRenderer.materials = materials; // Reassign the modified materials
    }

    private void ResetMaterials()
    {
        Material[] materials = playerRenderer.materials;

        for (int i = 0; i < materials.Length; i++)
        {

            materials[i].color = originalMaterials[i].color;
        }

        playerRenderer.materials = materials; // Reassign the original materials
    }

}