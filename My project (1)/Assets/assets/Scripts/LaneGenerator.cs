using System.Collections.Generic;
using UnityEngine;

public class LaneGenerator : MonoBehaviour
{
    public GameObject buildingPrefab;        // The prefab for the buildings
    public GameObject planePrefab;           // The prefab for the planes
    public GameObject car;                   // The car object to determine spawn progress

    // Spawning parameters
    public float spawnDistance = 85f;        // Distance ahead of the player to spawn buildings
    public float buildingSpacing = 20f;      // Distance between buildings along the path
    public float pathWidth = 50f;            // Width of the path (buildings spawn on either side)
    public float minBuildingHeight = 20f;    // Minimum building height
    public float maxBuildingHeight = 80f;    // Maximum building height

    private Vector3 spawnPosition;           // Current position for spawning buildings
    private Vector3 planeSpawnPosition;      // Current position for spawning planes
    private Queue<GameObject> spawnedBuildings = new Queue<GameObject>(); // To manage spawned buildings
    private Queue<GameObject> spawnedPlanes = new Queue<GameObject>();    // To manage spawned planes

    // Maximum number of buildings and planes allowed at a time (to optimize performance)
    public int maxBuildings = 50;
    public int maxPlanes = 5;

    void Start()
    {
        // Initialize the spawn position ahead of the car
        spawnPosition = car.transform.position ;
        planeSpawnPosition = car.transform.position; // Start the plane spawn position at the car's position

        // Initial spawning to populate the environment
        for (int i = 0; i < 10; i++)
        {
            SpawnBuildings();
            //SpawnPlane();
        }
    }

    void Update()
    {
        // Continuously update the spawn position ahead of the player
        while (Vector3.Distance(car.transform.position, spawnPosition) < spawnDistance)
        {
            SpawnBuildings();
            RemoveOldBuildings();
        }

        
    }

    void SpawnBuildings()
    {
        // Spawn buildings on both sides of the path
        for (int i = -1; i <= 1; i += 2) // -1 for left, +1 for right
        {
            Vector3 buildingPosition = spawnPosition + new Vector3(spawnDistance * 3, 0f, i * pathWidth / 2f);
            SpawnBuilding(buildingPosition);
        }

        // Move the spawn position further ahead
        spawnPosition += Vector3.right * buildingSpacing;
    }

    

    void SpawnBuilding(Vector3 position)
    {
        // Instantiate the building
        GameObject newBuilding = Instantiate(buildingPrefab, position, Quaternion.identity);

        // Set random height
        float buildingHeight = Random.Range(minBuildingHeight, maxBuildingHeight);
        newBuilding.transform.localScale = new Vector3(10f, buildingHeight, 10f); // Adjust scale
        newBuilding.transform.position = new Vector3(position.x, buildingHeight / 2f, position.z); // Adjust height

        // Set a random color
        Renderer renderer = newBuilding.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = new Color(Random.value, Random.value, Random.value);
        }

        // Add to the queue for tracking
        spawnedBuildings.Enqueue(newBuilding);
    }

    void RemoveOldBuildings()
    {
        // Remove excess buildings when the maximum limit is exceeded
        while (spawnedBuildings.Count > maxBuildings)
        {
            GameObject oldestBuilding = spawnedBuildings.Dequeue();
            Destroy(oldestBuilding);
        }
    }

    
}




