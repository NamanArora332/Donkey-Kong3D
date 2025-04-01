using System.Collections;
using UnityEngine;

public class DonkeyKongController : MonoBehaviour
{
    public Barrel barrelScript;
    public Rigidbody rb;
    public SphereCollider coll;
    public Transform player, barrelContainer;

    public GameObject barrelPrefab;
    public ParticleSystem barrelHitEffect;
    public Transform spawnPoint;

    public float pickUpRange;
    public float dropForwardForce, dropUpwardForce;
    public bool equipped;
    public static bool slotFull;

    //Time delay range for picking and throwing barrels
    public float minTimeBetweenActions = 2f;
    public float maxTimeBetweenActions = 5f;
    private float timeBetweenActions; // randomised time delay between picking up and throwing barrel
    private bool canPerformAction = true; //flag to control timing logic
    [SerializeField] private Animator animator; //calling the animations for Donkey Kong
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (!equipped)
        {
            barrelScript.enabled = false;
            rb.isKinematic = false;
            coll.isTrigger = false;
        }
        if (equipped)
        {
            barrelScript.enabled = true;
            rb.isKinematic = true;
            coll.isTrigger = true;
        }
        // Initialize the randomized time delay
        timeBetweenActions = GetRandomTime();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 distanceToPlayer = player.position - transform.position;
        if (canPerformAction)
        {
            //if player is in the range of the barrel and time to pick up has reached
            if (!equipped && distanceToPlayer.magnitude <= pickUpRange && !slotFull)
            {
                PickUp();
                StartCoroutine(PerformActionAfterDelay());
            }
            //For dropping if player is equipped and timer for dropping is reached
            else if (equipped)
            {
                Drop();
                StartCoroutine(PerformActionAfterDelay());
            }
        }
    }

    private void PickUp()
    {
        equipped = true;
        slotFull = true;

        // Make barrel a child of the Donkey Kong
        transform.SetParent(barrelContainer);

        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(Vector3.zero);
        transform.localScale = Vector3.one;

        //Making the rigidbody kinematic and the collider a trigger
        rb.isKinematic = true;
        coll.isTrigger = true;

        //Performing the action on barrels when it is thrown
        barrelScript.enabled = true;
    }
    private void Drop()
    {
        animator.ResetTrigger("isPickingUp");
        equipped = false;
        slotFull = false;

        transform.SetParent(null);

        rb.isKinematic = false;
        coll.isTrigger = false;

        rb.linearVelocity = player.right * dropForwardForce + Vector3.up * dropUpwardForce;

        barrelScript.enabled = false;
    }

    private IEnumerator PerformActionAfterDelay()
    {
        animator.SetTrigger("isPickingUp");
        canPerformAction = false;
        yield return new WaitForSeconds(timeBetweenActions);
        InstantiateNewBarrel();
        canPerformAction = true;

        // Randomize the time delay for the next action
        timeBetweenActions = GetRandomTime();
    }

    private void InstantiateNewBarrel()
    {
        
        // Create a new barrel instance at the spawn point
        GameObject newBarrel = Instantiate(barrelPrefab, spawnPoint.position, spawnPoint.rotation);

        // Ensure the new barrel has the correct components and references
        DonkeyKongController newBarrelController = newBarrel.GetComponent<DonkeyKongController>();
        if (newBarrelController != null)
        {
            newBarrelController.player = player;
            newBarrelController.barrelContainer = barrelContainer;
            newBarrelController.barrelPrefab = barrelPrefab;
            newBarrelController.spawnPoint = spawnPoint;
            newBarrelController.animator=animator;
            newBarrelController.barrelHitEffect=barrelHitEffect;
        }
    }

    // Method to generate a random time delay within the specified range
    private float GetRandomTime()
    {
        return Random.Range(minTimeBetweenActions, maxTimeBetweenActions);
    }

}
