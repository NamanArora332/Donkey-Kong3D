using UnityEngine;

public class VictoryCameraShake : MonoBehaviour
{
    public float shakeDuration = 5f; // Duration of the shake in seconds
    public float shakeSpeed = 2f;   // Speed of the shake (medium pace)
    public float shakeIntensity = 0.1f; // Intensity of the shake (how far it moves)

    private Vector3 originalPosition;
    private float elapsedTime = 0f; 
    [SerializeField] private Animator marioAnimator;
    [SerializeField] private Animator peachAnimator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        originalPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        bool isMarioJumping = IsAnimatorJumping(marioAnimator);
        bool isPeachJumping = IsAnimatorJumping(peachAnimator);

        if (isMarioJumping || isPeachJumping)
        {
            if (elapsedTime < shakeDuration)
            {
                elapsedTime += Time.deltaTime;
                float yOffset = Mathf.Sin(Time.time * shakeSpeed) * shakeIntensity;

                // Apply the shake to the camera's position
                transform.position = originalPosition + new Vector3(0f, yOffset, 0f);
            }
        }

        else
        {
            // Reset the camera to its original position when the shake ends
            transform.position = originalPosition;
        }
    }

    private bool IsAnimatorJumping(Animator animator)
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("VictoryJumping")||stateInfo.IsName("VictoryPeachJumping"))
        {
            return true;
        }

        return false;
    }
}
