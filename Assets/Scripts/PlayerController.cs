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
    private bool isBlinking=false;
    private Renderer playerRenderer;
    private Color originalColor;
    void Start()
    {
        rb = GetComponent<Rigidbody>();  // Get the Rigidbody component
        //store Mario's original position
        originalPosition= gameObject.transform.position;
    }

    void Update()
    {
        // Horizontal movement using InputManager
        float moveDirection = 0f;
        if (InputManager.Instance.GetLeftPressed())
        {
            moveDirection = -1f;  // Move left
        }
        else if (InputManager.Instance.GetRightPressed())
        {
            moveDirection = 1f;   // Move right
        }

        Vector3 movement = new Vector3(moveDirection * moveSpeed, rb.linearVelocity.y, 0);  // Move along X-axis only
        rb.linearVelocity = movement;  // Apply velocity directly

        // Jumping using InputManager
        if (InputManager.Instance.GetJumpPressed() && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);  // Jump upward
            isGrounded = false;  // Prevent double jumping
        }
        if(ladder && canClimb && InputManager.Instance.GetUpPressed()){
            climbUpLadder();
        }
        if(ladder && canClimb && InputManager.Instance.GetDownPressed()){
            climbDownLadder();
        }
    }

    private void Awake()
    {
        playerRenderer=GetComponent<Renderer>();
        originalColor=playerRenderer.material.color;
        
    }

    // Detect if Mario is grounded
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Debug.Log("Grounded, can jump");
            isGrounded = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Debug.Log("Not grounded, can't jump");
            isGrounded = false;
        }
    }

    public void EnableImmunityEffect(){
        StartCoroutine(BlinkingEffect());
    }

    public void DisableImmunityEffect(){
        StartCoroutine(BlinkingEffect());
        playerRenderer.material.color=originalColor;
        isBlinking=false;
    }

    public void ResetPosition(){
        gameObject.transform.position=originalPosition;
    }
    public void setLadder(Ladder ladder){
        Debug.Log("Ladder set ");
        this.ladder = ladder;
    }
    public void setCanClimb(bool canClimb){
        Debug.Log("Can climb set to " + canClimb);
        this.canClimb= canClimb;
    }
    public void climbUpLadder(){
        Debug.Log("Climbing up ladder");
       Transform upPosition = ladder.top;
       this.transform.position = upPosition.position;

    }
    public void climbDownLadder(){
        Debug.Log("Climbing down ladder");
        Transform downPosition = ladder.bottom;
        this.transform.position = downPosition.position;
    }

    private IEnumerator BlinkingEffect(){
        isBlinking=true;
        while(isBlinking){
            playerRenderer.material.color= Color.red;
            yield return new WaitForSeconds(0.25f);

            playerRenderer.material.color=originalColor;
            yield return new WaitForSeconds(0.25f);
        }
    }

}