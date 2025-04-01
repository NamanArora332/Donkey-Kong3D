using UnityEngine;

public class Ladder : MonoBehaviour
{
    public Transform bottom;
    public Transform top;
    public void Start(){
        bottom = transform.Find("Bottom");
        top = transform.Find("Top");
    }
    private void OnTriggerEnter(Collider collider){
        // Debug.Log("Ladder trigger entered");
        // Debug.Log(collider.gameObject.tag);
        if(collider.gameObject.tag == "Mario"){
            // Debug.Log("Player entered ladder");
            PlayerController player=collider.gameObject.GetComponent<PlayerController>();
            onPlayerEnter(player);
            player.setLadder(this);
        }
    
    }
    public void OnTriggerExit(Collider collider){
        // Debug.Log("Ladder trigger exited");
        // Debug.Log(collider.gameObject.tag);
        if(collider.gameObject.tag == "Mario"){
            // Debug.Log("Player exited ladder");
            PlayerController player=collider.gameObject.GetComponent<PlayerController>();
            onPlayerExit(player);
            player.setLadder(null);
        }
    }
    public void onPlayerEnter(PlayerController player){
        player.setCanClimb(true);
        // Debug.Log("Player can climb");
        
    }
    public void onPlayerExit(PlayerController player){
        player.setCanClimb(false);
    }

}
