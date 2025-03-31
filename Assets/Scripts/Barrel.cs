using System.Collections;
using UnityEngine;

public class Barrel : MonoBehaviour
{
    private Rigidbody rigid;
    private bool isRolling = false; // Flag to track if the barrel is rolling
    public float moveSpeed = 7f;
    private Vector3 direction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
    }


    private void OnCollisionEnter(Collision collision)
    {
        // Check if the collided object is tagged as "SideWall"
        if (collision.gameObject.CompareTag("SideWall") && !isRolling)
        {
            Debug.Log("Barrel collided with the Wall. Rolling forward...");
            RestoreFullMovement();

            // Start rolling the barrel forward
            StartCoroutine(RollForwardForDuration(0.5f)); // Roll for 2 seconds
        }
        if (collision.gameObject.CompareTag("Ground"))
        {
            // Determine the direction to move based on the ground's position
            direction = DetermineDirection(collision.transform);

            //only enable forward movement
            rigid.constraints = RigidbodyConstraints.FreezePositionZ;
            //barrel movement speed
            rigid.linearVelocity = direction * moveSpeed;
        }
        //check if barrel collides over mario
        if (collision.gameObject.CompareTag("Mario"))
        {
            Vector3 marioScale = collision.transform.localScale;

            if (GameManager.Instance.getMarioBig()) // Checking if Mario has grown
            {
                if (AudioManager.instance != null)
                {
                    AudioManager.instance.sfxSource.PlayOneShot(AudioManager.instance.fightOffClip); // Play fightOffClip without stopping bigMarioClip
                }
                Destroy(gameObject);
            }
            else
            {
                GameManager.Instance.RemoveLife();
            }
        }

    }
    private void RestoreFullMovement()
    {
        // Remove all constraints to restore full movement
        rigid.constraints = RigidbodyConstraints.None;

        // Reset velocity to zero
        rigid.linearVelocity = Vector3.zero;
    }
    private Vector3 DetermineDirection(Transform groundTransform)
    {
        // Calculate the relative position of the ground compared to the barrel
        float directionX = groundTransform.position.x - transform.position.x;

        // Normalize the direction to +1 (right) or -1 (left)
        return new Vector3(Mathf.Sign(directionX), 0f, 0f);
    }
    private IEnumerator RollForwardForDuration(float duration)
    {
        isRolling = true; // Set the rolling flag to true

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            // Apply a continuous force along the +Z axis
            rigid.AddForce(-Vector3.forward * 3f, ForceMode.Force); // Adjust the force multiplier as needed

            elapsedTime += Time.deltaTime;
            yield return null; // Wait until the next frame
        }

        Debug.Log("Stopping barrel's rolling motion...");
        isRolling = false; // Reset the rolling flag
    }
}
