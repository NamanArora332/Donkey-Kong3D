using UnityEngine;

public class Fireball : MonoBehaviour
{
    public Rigidbody rb;
    public float speed = 2f;
    public float moveDirection;
    public float lifetime = 5f;
    public int damage = 10;
    public GameObject explosionPrefab;
    public AudioClip explosionSound;
    public GameObject Player;
    public Ladder[] ladders;
    public int currentLadder = 0;
    private bool isClimbing =false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    void Update()
    {
        // transform.Translate(Vector3.right * speed * Time.deltaTime); //commented out because it gives fireball jumpy motion
        lifetime -= Time.deltaTime;
        if (lifetime <= 0)
        {
            Destroy(gameObject);
        }
        else
        {
            MoveTowardsPlayer();
        }


    }
    void MoveTowardsPlayer()
    {
        //check if player is on same level as fireball

        if (Player.transform.position.y <= ladders[currentLadder].top.position.y)
        {
            // Determine movement direction (-1 for left, 1 for right)
            float moveDirection = Mathf.Sign(Player.transform.position.x - transform.position.x);

            // Set velocity to move only in the X direction
            rb.linearVelocity = new Vector3(moveDirection * speed, rb.linearVelocity.y, rb.linearVelocity.z);
        }
        else
        {
            climbCurrentLadder();
        }

    }
    void climbCurrentLadder()
    {

        // move towards ladders[currentladder]
        if (Mathf.Abs(ladders[currentLadder].top.position.x - transform.position.x) > 0.1f)
        {
            // rb.AddForce(Vector3.left * speed * Time.deltaTime); //commented out because the movement on the ground wasn't really moving in the left/right direction
            float direction = Mathf.Sign(ladders[currentLadder].bottom.position.x - transform.position.x);
            rb.linearVelocity = new Vector3(direction * speed, rb.linearVelocity.y, rb.linearVelocity.z);

        }
        else
        {
            // rb.AddForce(Vector3.right * speed * Time.deltaTime);
            rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, rb.linearVelocity.z);
            climbUpLadder(ladders[currentLadder]);
            currentLadder = (currentLadder + 1) % ladders.Length;
        }
    }

    void OnTriggerEnter(Collider collision)
    { //changed to OnTriggerEnter because ladder uses isTrigger in its collider

        if (collision.gameObject.CompareTag("Ladder") && !isClimbing)
        {
            if (ladders[currentLadder].top.position.y != collision.gameObject.transform.position.y)
            {
                isClimbing=true;
                climbUpLadder(ladders[currentLadder]);
                Invoke("ResetClimbingState", 0.5f);
            }
        }

    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Mario"))
        {
            GameManager.Instance.RemoveLife();
            Destroy(gameObject);
        }
    }

    void climbUpLadder(Ladder ladder)
    {
        rb.isKinematic = true;
        this.transform.position = ladder.top.position;
        rb.isKinematic = false;
    }

    void ResetClimbingState(){
        isClimbing=false;
    }
}
