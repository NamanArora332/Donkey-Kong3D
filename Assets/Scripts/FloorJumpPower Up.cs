using UnityEngine;
using UnityEngine.Events;

public class FloorJumpPowerUp : MonoBehaviour
{
    public UnityEvent OnPowerUpCollected; // Event to invoke when the power-up is collected
    public float heightOffset = 1f; // Offset to ensure Mario lands on top of the floor

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object that entered the trigger is Mario
        if (other.gameObject.CompareTag("Mario"))
        {
            Debug.Log("Floor Jump Power-Up collected by Mario!");

            // Get the PlayerController component to access Mario's functionality
            PlayerController player = other.gameObject.GetComponent<PlayerController>();
            if (player != null)
            {
                // Call a method to move Mario to the next floor
                MoveToNextFloor(player);
            }

            // Invoke any additional events (e.g., for UI or score updates)
            OnPowerUpCollected?.Invoke();

            // Destroy the power-up after it's collected
            Destroy(gameObject);
        }
    }

    private void MoveToNextFloor(PlayerController player)
    {
        // Find all floors in the scene (tagged as "Ground")
        GameObject[] floors = GameObject.FindGameObjectsWithTag("Ground");

        if (floors.Length == 0)
        {
            Debug.LogWarning("No floors found with tag 'Ground'!");
            return;
        }

        // Get Mario's current position
        Vector3 playerPosition = player.transform.position;

        // Find the next floor above Mario
        GameObject nextFloor = null;
        float minDistanceAbove = float.MaxValue;

        foreach (GameObject floor in floors)
        {
            float floorY = floor.transform.position.y;
            float playerY = playerPosition.y;

            // Check if the floor is above Mario and closer than the current closest floor
            if (floorY > playerY && (floorY - playerY) < minDistanceAbove)
            {
                minDistanceAbove = floorY - playerY;
                nextFloor = floor;
            }
        }

        if (nextFloor != null)
        {
            // Get the floor's collider to determine its size (to land on top properly)
            Collider floorCollider = nextFloor.GetComponent<Collider>();
            float floorHeight = floorCollider != null ? floorCollider.bounds.size.y : 0f;

            // Calculate the target position:
            // - X: Keep Mario's X position (no change in left/right position)
            // - Y: Set to the floor's Y position + half the floor's height + offset (to land on top)
            // - Z: Set to the floor's Z position (to align with the floor's depth)
            Vector3 targetPosition = new Vector3(
                playerPosition.x, // Keep Mario's X position
                nextFloor.transform.position.y + (floorHeight / 2f) + heightOffset, // Land on top of the floor
                nextFloor.transform.position.z // Match the floor's Z position
            );

            // Teleport Mario to the target position
            player.transform.position = targetPosition;
            Debug.Log($"Teleported Mario to floor at Position: {targetPosition}");
        }
        else
        {
            Debug.Log("No floor found above Mario!");
        }
    }
}