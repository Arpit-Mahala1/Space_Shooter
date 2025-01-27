

using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class Projectile_2 : MonoBehaviour
{
    
    [Header("Projectile Settings")]
    public GameObject Power2;                // The projectile prefab
    public Transform Fire_point;             // The fire point attached to the car
    public float projectileSpeed = 700f;     // Speed of the projectile
    public float rayLength = 500f;           // Length of the aiming ray
    public float shootCooldown = 5f;         // Cooldown time between shots (in seconds)
    public float raycastOffset = 5f;         // Offset for the raycast origin

    [Header("Effects")]
    public GameObject explosionEffectPrefab; // Reference to your particle effect prefab

    private Transform carTransform;          // Reference to the car's transform
    private LineRenderer lineRenderer;       // LineRenderer for the aiming ray
    private bool isAiming = false;           // Track aiming state
    private bool canShoot = true;            // Controls shooting cooldown

    void Start()
    {
        // Ensure Fire_point is assigned
        if (Fire_point == null)
        {
            Debug.LogError("Fire_point is not assigned. Please attach a fire point to the car.");
            enabled = false;
            return;
        }

        // Cache the car's transform
        carTransform = Fire_point.root; // Assumes Fire_point is a child of the car
        if (carTransform == null)
        {
            Debug.LogError("No car transform found. Ensure the Fire_point is a child of the car.");
        }

        // Set up the LineRenderer for aiming
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.startWidth = 0.25f;
        lineRenderer.endWidth = 1.5f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.green;
        lineRenderer.endColor = Color.green;
        lineRenderer.positionCount = 0;
    }

    public void OnAim(InputAction.CallbackContext context)
    {
        float value = context.ReadValue<float>();
        isAiming = value > 0f;

        if (isAiming)
        {
            StartAiming();
        }
        else
        {
            StopAiming();
        }
    }

    void Update()
    {
        if (isAiming)
        {
            DrawAimingRay();
        }
    }

    void StartAiming()
    {
        lineRenderer.positionCount = 2;
    }

    void StopAiming()
    {
        lineRenderer.positionCount = 0;
    }

    void DrawAimingRay()
    {
        Vector3 shootingDirection = carTransform.right.normalized;
        Vector3 rayStart = Fire_point.position + shootingDirection * raycastOffset;
        Vector3 rayEnd = rayStart + shootingDirection * rayLength;

        if (lineRenderer.positionCount != 2)
        {
            lineRenderer.positionCount = 2;
        }

        lineRenderer.SetPosition(0, rayStart);
        lineRenderer.SetPosition(1, rayEnd);
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed && isAiming && canShoot)
        {
            Shoot();
        }
    }

    void Shoot()
    {
        Vector3 shootingDirection = carTransform.right.normalized;
        Vector3 spawnPosition = Fire_point.position + shootingDirection * raycastOffset;

        // Instantiate the projectile
        var projectileObj = Instantiate(Power2, spawnPosition, Quaternion.identity);

        // Get the shooter's tag (Player1 or Player2)
        string shooterTag = carTransform.tag;

        // Add force to the projectile
        Rigidbody rb = projectileObj.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = shootingDirection * projectileSpeed;
        }

        // Add collision detection script to the projectile
        ProjectileCollision collisionScript = projectileObj.AddComponent<ProjectileCollision>();

        // Get all colliders from the shooter (in case there are multiple colliders)
        Collider[] shooterColliders = carTransform.GetComponentsInChildren<Collider>();
        Collider projectileCollider = projectileObj.GetComponent<Collider>();

        // Ignore collisions between the projectile and all shooter's colliders
        foreach (Collider shooterCollider in shooterColliders)
        {
            Physics.IgnoreCollision(projectileCollider, shooterCollider, true);
        }

        collisionScript.Initialize(shooterTag, explosionEffectPrefab);

        // Apply shooting cooldown
        StartCoroutine(ShootingCooldown());

        // Stop aiming after shooting
        StopAiming();
    }

    private IEnumerator ShootingCooldown()
    {
        canShoot = false;
        yield return new WaitForSeconds(shootCooldown);
        canShoot = true;
    }
}

// Helper component to handle projectile collisions
public class ProjectileCollision : MonoBehaviour
{
    public float damage=25f;
    private string shooterTag;              // Tag of the player who shot this projectile
    private GameObject explosionEffect;      // Reference to the explosion effect prefab

    public void Initialize(string shooterTag, GameObject explosionEffectPrefab)
    {
        this.shooterTag = shooterTag;
        this.explosionEffect = explosionEffectPrefab;
    }

    private void OnCollisionEnter(Collision collision)
    {
            Debug.Log($"Projectile from {shooterTag} hit object with tag: {collision.gameObject.tag}");

            // Check if the collision is with a valid target
            if (collision.gameObject.tag != shooterTag)
            {
                // Adjust max_health if car_movement exists
                var carMovement = collision.gameObject.GetComponent<car_movement>();
                if (carMovement != null)
                {
                    carMovement.max_health -= damage ;
                    carMovement.Current_health=carMovement.max_health-damage;
                    
                }
                
            
                // Adjust Astroid_health if Fracture1 exists
                
                

                // Spawn explosion effect at the collision point
                if (explosionEffect != null)
                {
                    Vector3 collisionPoint = collision.contacts[0].point;
                    Instantiate(explosionEffect, collisionPoint, Quaternion.identity);
                }

                // Destroy the projectile
                Destroy(gameObject);
            }

            if(collision.gameObject.tag =="Asteroid")
            {
                Destroy(collision.gameObject);
                Destroy(gameObject);
            }
            
        


        //// Debug log to help with troubleshooting
        //Debug.Log($"Projectile from {shooterTag} hit object with tag: {collision.gameObject.tag}");

        //// Spawn explosion effect and destroy projectile for any collision except with shooter
        //if (collision.gameObject.tag != shooterTag)
        //{
        //    collision.gameObject.GetComponent<car_movement>().max_health += -100;
        //    collision.gameObject.GetComponent<Fracture1>().Astroid_health += -100;
        //    // Spawn explosion effect at the collision point if we have one
        //    if (explosionEffect != null)
        //    {
        //        Vector3 collisionPoint = collision.contacts[0].point;
        //        Instantiate(explosionEffect, collisionPoint, Quaternion.identity);
        //    }

        //    // Destroy the projectile
        //    Destroy(gameObject);
        //}

    }
    
}