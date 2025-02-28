//using UnityEngine;

//public class Spawn : MonoBehaviour
//{
//    public GameObject[] rocks; // Array of asteroid prefabs
//    public Vector3 spawnAreaCenter; // Center of the spawning area
//    public Vector3 spawnAreaSize; // Size of the spawning area
//    public int numberOfAsteroidsPerSpawn = 5; // Number of asteroids to spawn at once

//    public void StartSpawning()
//    {
//        SpawnAsteroids(); // Spawn all asteroids immediately
//    }

//    void SpawnAsteroids()
//    {
//        for (int i = 0; i < numberOfAsteroidsPerSpawn; i++)
//        {
//            // Randomly generate a position within the spawn area
//            Vector3 randomPosition = GetRandomPositionInArea();

//            // Randomly select an asteroid prefab
//            GameObject randomAsteroid = rocks[Random.Range(0, rocks.Length)];

//            // Instantiate the asteroid
//            GameObject asteroid = Instantiate(randomAsteroid, randomPosition, Quaternion.identity);

//            // Randomly set the size of the asteroid between 1 and 10
//            float randomSize = Random.Range(1f, 10f);
//            asteroid.transform.localScale = Vector3.one * randomSize;

//            // Set the weight based on the size (for demonstration, weight = size * 10)
//            AsteroidProperties asteroidProperties = asteroid.AddComponent<AsteroidProperties>();
//            asteroidProperties.weight = randomSize * 10;
//        }
//    }

//    Vector3 GetRandomPositionInArea()
//    {
//        float randomX = Random.Range(-spawnAreaSize.x / 2, spawnAreaSize.x / 2);
//        float randomY = Random.Range(-spawnAreaSize.y / 2, spawnAreaSize.y / 2);
//        float randomZ = Random.Range(-spawnAreaSize.z / 2, spawnAreaSize.z / 2);

//        return spawnAreaCenter + new Vector3(randomX, randomY, randomZ);
//    }

//    void OnDrawGizmosSelected()
//    {
//        // Draw the spawn area in the Scene view for visualization
//        Gizmos.color = Color.green;
//        Gizmos.DrawWireCube(spawnAreaCenter, spawnAreaSize);
//    }
//}

//// Class to store asteroid properties
//public class AsteroidProperties : MonoBehaviour
//{
//    public float weight; // Weight of the asteroid
//}

using UnityEngine;

public class Spawn : MonoBehaviour
{
    public GameObject[] rocks; // Array of asteroid prefabs
    public Vector3 spawnAreaCenter; // Center of the spawning area
    public Vector3 spawnAreaSize; // Size of the spawning area
    public int numberOfAsteroidsPerSpawn = 5; // Number of asteroids to spawn at once

    public void StartSpawning()
    {
        if (rocks == null || rocks.Length == 0)
        {
            Debug.LogError("No asteroid prefabs assigned to 'rocks' array!"); // Debugging: Check if prefabs are assigned
            return;
        }

        Debug.Log("Spawning Asteroids..."); // Debugging
        SpawnAsteroids();
    }

    void SpawnAsteroids()
    {
        for (int i = 0; i < numberOfAsteroidsPerSpawn; i++)
        {
            Vector3 randomPosition = GetRandomPositionInArea(); // Get a random spawn position

            GameObject randomAsteroid = rocks[Random.Range(0, rocks.Length)]; // Select a random prefab

            if (randomAsteroid == null)
            {
                Debug.LogError("Selected asteroid prefab is null!"); // Debugging
                continue;
            }

            GameObject asteroid = Instantiate(randomAsteroid, randomPosition, Quaternion.identity); // Instantiate asteroid
            Debug.Log($"Asteroid {i + 1} spawned at {randomPosition}"); // Debugging

            float randomSize = Random.Range(1f, 10f);
            asteroid.transform.localScale = Vector3.one * randomSize; // Set asteroid size
            Debug.Log($"Asteroid {i + 1} size set to {randomSize}"); // Debugging

            AsteroidProperties asteroidProperties = asteroid.AddComponent<AsteroidProperties>();
            asteroidProperties.weight = randomSize * 10; // Set asteroid weight
            Debug.Log($"Asteroid {i + 1} weight set to {asteroidProperties.weight}"); // Debugging
        }
    }

    Vector3 GetRandomPositionInArea()
    {
        float randomX = Random.Range(-spawnAreaSize.x / 2, spawnAreaSize.x / 2);
        float randomY = Random.Range(-spawnAreaSize.y / 2, spawnAreaSize.y / 2);
        float randomZ = Random.Range(-spawnAreaSize.z / 2, spawnAreaSize.z / 2);

        Vector3 position = spawnAreaCenter + new Vector3(randomX, randomY, randomZ);
        Debug.Log($"Generated random position: {position}"); // Debugging
        return position;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(spawnAreaCenter, spawnAreaSize);
    }
}

// Class to store asteroid properties
public class AsteroidProperties : MonoBehaviour
{
    public float weight; // Weight of the asteroid
}
