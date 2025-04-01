using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public float duration = 0.2f; // How long the shake lasts
    public float magnitude = 0.2f; // How strong the shake is

    public IEnumerator Shake()
    {
        Vector3 originalPosition = transform.localPosition;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = originalPosition + new Vector3(x, y, 0);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalPosition; // Reset camera position
    }

    // Public method to trigger the shake
    public void StartShake()
    {
        StartCoroutine(Shake());
    }
}
