using UnityEngine;

public class DestroyBarrel : MonoBehaviour
{
   
    private void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.CompareTag("Barrel")){
            // Debug.Log("Destorying barrel");
            Destroy(collision.gameObject);
        }
        if(collision.gameObject.CompareTag("Mario")){
            
            GameManager.Instance.RemoveLife();

            Rigidbody barrelRb = collision.GetComponent<Rigidbody>();
            if (barrelRb != null)
            {
                barrelRb.linearVelocity = barrelRb.linearVelocity.normalized * barrelRb.linearVelocity.magnitude;
            }
        }
        if(collision.gameObject.CompareTag("Kong")){
            
            GameManager.Instance.RemoveLife();
        }
    }
}
