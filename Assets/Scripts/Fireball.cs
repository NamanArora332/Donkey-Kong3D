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
    public int currentLadder=0;

void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        lifetime -= Time.deltaTime;
        if(lifetime >0) {
            MoveTowardsPlayer();
            }
        else
        {
            Destroy(gameObject);
        }
        

    }
    void MoveTowardsPlayer()
    {
        //check if player is on same level as fireball
        
        if (Player.transform.position.y <= ladders[currentLadder].top.position.y)
        {
            Debug.Log("Player is on same level as fireball");
            // Determine movement direction (-1 for left, 1 for right)
            float moveDirection = Mathf.Sign(ladders[currentLadder].top.position.x - transform.position.x);

            // Set velocity to move only in the X direction
            rb.linearVelocity = new Vector3(moveDirection * speed, rb.linearVelocity.y, rb.linearVelocity.z); 



        }
        else
        {
            Debug.Log("Player is not on same level as fireball, climbing ladder");
            climbCurrentLadder();
        }

    }
    void climbCurrentLadder()
    {
        // move towards ladders[currentladder]
        if (ladders[currentLadder].top.position.x - transform.position.x < 0)
        {
            rb.AddForce(Vector3.left * speed * Time.deltaTime);
        }
        else
        {
            rb.AddForce(Vector3.right * speed * Time.deltaTime);
        }
    }

    void OnCollisionEnter(Collision collision){
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player hit by fireball!");
        }
        if (collision.gameObject.CompareTag("Ladder"))
        {
            Debug.Log("Fireball appraoched ladder");
            if(ladders[currentLadder].top.position.y !=collision.gameObject.transform.position.y)
            {
                Debug.Log("Can climb up this ladder");
                climbUpLadder(ladders[currentLadder++]);
            }
            else
            {
                Debug.Log("Cannot climb up this ladder");
            }
        }

    }
    void climbUpLadder(Ladder ladder)
    {
        Debug.Log("Climbing up ladder");
        this.transform.position = ladder.top.position;

    }
}
