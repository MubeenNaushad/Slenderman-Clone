using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlendermanChase : MonoBehaviour
{
    public float detectionRadius = 13f;  // Radius within which the NPC detects the player
    public float approachSpeed = 5f;     // Speed at which the NPC approaches the player
    public float stopDistance = 1f;      // Distance at which the NPC stops moving towards the player

    public Transform player;             // Reference to the player's transform

    private CharacterController controller; // Reference to the CharacterController component
    private bool isPlayerInRange = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        if (controller == null)
        {
            Debug.LogError("CharacterController component is missing.");
            enabled = false;
            return;
        }

        if (player == null)
        {
            // If the player is not set in the inspector, try to find the player by tag
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null)
            {
                player = playerObject.transform;
            }
        }
    }

    void Update()
    {
        if (player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            // Check if the player is within the detection radius
            isPlayerInRange = distanceToPlayer <= detectionRadius;

            // If the player is in range, move towards the player
            if (isPlayerInRange)
            {
                MoveTowardsPlayer(distanceToPlayer);
            }
        }
    }

    void MoveTowardsPlayer(float distanceToPlayer)
    {
        // Move towards the player if further than the stop distance
        if (distanceToPlayer > stopDistance)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            Vector3 move = direction * approachSpeed * Time.deltaTime;

            // Move the NPC using the CharacterController
            controller.Move(move);

            // Adjust rotation to face the player
            transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
        }
    }

    // (Optional) Visualize the detection radius in the editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
