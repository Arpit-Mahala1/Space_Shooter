using UnityEngine;
using System.Collections;

public class CubeEffect : MonoBehaviour
{
    [Header("Projectile Settings")]
    public string opponentTag; // Tag of the opponent player
    public float slowDownAmount = 20f; // Amount to slow down
    public float slowDownDuration = 3f; // Duration of the slow down effect
    public float teleportDistance = -200f; // Distance to teleport on the X-axis
    public GameObject teleportEffect; // Particle effect prefab for teleportation

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the projectile hits the opponent
        if (collision.gameObject.CompareTag(opponentTag))
        {
            // Determine behavior based on projectile type
            if (gameObject.CompareTag("Projectile1"))
            {
                ApplySlowDownEffect(collision.gameObject);
            }
            else if (gameObject.CompareTag("Projectile2"))
            {
                ApplyTeleportEffect(collision.gameObject);
            }

            // Destroy the projectile after it hits
            Destroy(gameObject);
        }
    }

    private void ApplySlowDownEffect(GameObject opponent)
    {
        // Access the player's forwardSpeed variable from their movement script
        car_movement playerMovement = opponent.GetComponent<car_movement>();
        if (playerMovement != null)
        {
            // Temporarily reduce the forwardSpeed
            float originalSpeed = playerMovement.forwardSpeed;

            // Only slow down if not already slowed
            if (!playerMovement.isSlowed)
            {
                playerMovement.forwardSpeed = Mathf.Max(0, originalSpeed - slowDownAmount); // Ensure it doesn't go below 0
                playerMovement.isSlowed = true; // Mark player as slowed

                // Restore speed after a delay
                StartCoroutine(RestoreSpeed(playerMovement, originalSpeed));
            }
        }
    }

    private IEnumerator RestoreSpeed(car_movement playerMovement, float originalSpeed)
    {
        yield return new WaitForSeconds(slowDownDuration);
        playerMovement.forwardSpeed = originalSpeed;
        playerMovement.isSlowed = false; // Allow future slow effects
    }

    private void ApplyTeleportEffect(GameObject opponent)
    {
        // Get the opponent's current position
        Vector3 currentPosition = opponent.transform.position;

        // Calculate the teleport position
        Vector3 teleportPosition = currentPosition + new Vector3(teleportDistance, 0, 0);

        // Trigger teleport particle effect
        if (teleportEffect != null)
        {
            Instantiate(teleportEffect, currentPosition, Quaternion.identity);
        }

        // Teleport the player
        opponent.transform.position = teleportPosition;
    }
}
