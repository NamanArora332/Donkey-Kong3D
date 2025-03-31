using UnityEngine;
using UnityEngine.Events;

public class Coins : MonoBehaviour
{
    public float speed;
    public int scoreValue = 10;
    public UnityEvent OnCoinCollected = new UnityEvent();
    
    void Update()
    {
        transform.Rotate(Vector3.up, speed * Time.deltaTime);
        // Debug.Log("Rotation: " + transform.rotation.eulerAngles);
    }
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger");
        if (other.gameObject.CompareTag("Mario"))
        {
            if(AudioManager.instance != null)
            {
                AudioManager.instance.sfxSource.PlayOneShot(AudioManager.instance.getCoinClip);
            }
            OnCoinCollected.Invoke();
            GameManager.Instance.AddScore();
            Destroy(gameObject);
        }
    }
}

