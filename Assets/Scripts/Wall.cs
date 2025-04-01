using UnityEngine;

public class Wall : MonoBehaviour
{
    public float rollSpeed =10f;
    private void OnTriggerEnter(Collider other)
    {
        //Check the trigger zone with the barrel
        // if (other.CompareTag("Barrel"))
        // {
        //     Rigidbody barrelRigidbody = other.GetComponent<Rigidbody>();
        //     if (barrelRigidbody != null)
        //     {
        //         barrelRigidbody.AddForce(-Vector3.forward * rollSpeed, ForceMode.Impulse);
        //     }
        // }

    }
}