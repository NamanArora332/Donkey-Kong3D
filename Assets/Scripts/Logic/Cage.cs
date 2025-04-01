using UnityEngine;
using System.Collections;

public class Cage : MonoBehaviour
{

    public float moveUpDuration = 3f; // Duration for the cage to move upward
    public float moveUpDistance = 5f; // Distance to move upward
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Mario"))
        {
            StartCoroutine(MoveUpAndDestroy());
        }
    }

    private IEnumerator MoveUpAndDestroy()
    {
        float elapsedTime = 0f; // Tracks how much time has passed
        Vector3 startPosition = transform.position; // Initial position of the cage
        Vector3 targetPosition = startPosition + Vector3.up * moveUpDistance; // Final position after moving up

        // Gradually move the cage upward over the specified duration
        while (elapsedTime < moveUpDuration)
        {
            elapsedTime += Time.deltaTime; // Increment elapsed time
            float t = Mathf.Clamp01(elapsedTime / moveUpDuration); // Normalize time to [0, 1]
            transform.position = Vector3.Lerp(startPosition, targetPosition, t); // Interpolate position
            yield return null; // Wait until the next frame
        }

        Destroy(gameObject);
    }
}
