//using UnityEngine;

//public class Projectile : MonoBehaviour
//{
//    public float speed = 10f; // Speed of the projectile
//    private Transform target; // Target to move towards
//    private System.Action<Transform> onHitEffect; // Effect to apply on hit

//    public void SetTarget(Transform target, System.Action<Transform> effect)
//    {
//        this.target = target;
//        this.onHitEffect = effect;
//    }

//    void Update()
//    {
//        if (target == null)
//        {
//            Destroy(gameObject);
//            return;
//        }

//        // Move the projectile towards the target
//        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

//        // Check if the projectile has reached the target
//        if (Vector3.Distance(transform.position, target.position) < 0.5f)
//        {
//            // Apply the effect on hit
//            onHitEffect.Invoke(target);

//            // Destroy the projectile
//            Destroy(gameObject);
//        }
//    }
//}
