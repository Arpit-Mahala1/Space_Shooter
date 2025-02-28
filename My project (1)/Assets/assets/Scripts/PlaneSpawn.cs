using UnityEngine;
using UnityEngine.UIElements;

public class PlaneSpawn : MonoBehaviour
{

    public GameObject PlayerPrefab;
    public GameObject buildingPrefab;        // The prefab for the buildings
    public GameObject planePrefab;           // The prefab for the planes
    public GameObject[] smallCubePrefabs;    // Array of prefabs for the small cubes
    public GameObject startPrefab;           // The prefab for the start of the plane sequence
    public GameObject endPrefab;             // The prefab for the end of the plane sequence
    public float PlayerSeperation = 5f;
    // Building spawning parameters
    public float buildingSpacing = 20f;      // Distance between buildings along the path
    public float pathWidth = 50f;            // Width of the path (buildings spawn on either side)
    public float minBuildingHeight = 20f;    // Minimum building height
    public float maxBuildingHeight = 80f;    // Maximum building height

    // Plane spawning parameters
    public int numberOfPlanes = 20;          // Number of planes between the start and end prefabs
    public float planeSpacing = 100f;        // Distance between consecutive prefabs
    public float planeYOffset = 0f;          // Offset height for planes

    private Vector3 planeStartPosition;      // Start position of the plane sequence
    private Vector3 planeEndPosition;        // End position of the plane sequence

    public void Initialize()
    {

        GameObject Player1 = Instantiate(PlayerPrefab, new Vector3(2000f, 2f, PlayerSeperation), Quaternion.AngleAxis(180f,Vector3.up));
        Player1.name = "Player1";
        Player1.tag = Player1.name;

        GameObject Player2 = Instantiate(PlayerPrefab, new Vector3(0f, 2f, -PlayerSeperation), Quaternion.identity);
        Player2.name = "Player2";
        Player2.tag = Player2.name;

        //GameObject gameTimerObject = GameObject.Find("GameTimer");
        //if (gameTimerObject != null)
        //{
        //    GameTimer gameTimer = gameTimerObject.GetComponent<GameTimer>();
        //    if (gameTimer != null)
        //    {
        //        gameTimer.player1 = Player1;
        //        gameTimer.player2 = Player2;
        //    }
        //}

        // Spawn the planes and calculate start and end positions
        planeStartPosition = SpawnPlanes();
        planeEndPosition = planeStartPosition + new Vector3((numberOfPlanes - 1) * planeSpacing, 0, 0);

        // Spawn all buildings based on the plane sequence
        SpawnBuildings();

        // Spawn small cubes
        SpawnSmallCubes();
    }

    void SpawnBuildings()
    {
        // Calculate the total length of the plane sequence
        float totalLength = Vector3.Distance(planeStartPosition, planeEndPosition);

        // Start spawning buildings from the start of the plane sequence
        Vector3 buildingSpawnPosition = planeStartPosition;

        while (Vector3.Distance(buildingSpawnPosition, planeEndPosition) > 0)
        {
            // Spawn buildings on both sides of the path
            for (int i = -1; i <= 1; i += 2) // -1 for left, +1 for right
            {
                Vector3 buildingPosition = buildingSpawnPosition + new Vector3(0f, 0f, i * pathWidth / 2f);
                SpawnBuilding(buildingPosition);
            }

            // Move the spawn position further along the plane sequence
            buildingSpawnPosition += Vector3.right * buildingSpacing;

            // Stop if we go beyond the end position
            if (buildingSpawnPosition.x > planeEndPosition.x)
                break;
        }
    }

    void SpawnBuilding(Vector3 position)
    {
        GameObject newBuilding = Instantiate(buildingPrefab, position, Quaternion.identity);

        // Set random height
        float buildingHeight = Random.Range(minBuildingHeight, maxBuildingHeight);
        newBuilding.transform.localScale = new Vector3(10f, buildingHeight, 10f);
        newBuilding.transform.position = new Vector3(position.x+50f, buildingHeight / 2f, position.z);

        // Set a random color
        Renderer renderer = newBuilding.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = new Color(Random.value, Random.value, Random.value);
        }
    }

    Vector3 SpawnPlanes()
    {
        float startX = 0f; // Start from a fixed x-coordinate

        // Spawn the start prefab
        Vector3 startPrefabPosition = new Vector3(startX+80f, planeYOffset, 0f);
        Instantiate(startPrefab, startPrefabPosition, Quaternion.Euler(-90, -90, 0));

        // Spawn the plane prefabs
        for (int i = 1; i <= numberOfPlanes; i++)
        {
            float xPosition = startX + i * planeSpacing;
            Instantiate(planePrefab, new Vector3(xPosition, planeYOffset, 0f), Quaternion.identity);
        }

        // Spawn the end prefab
        float endX = startX + (numberOfPlanes - 2) * planeSpacing;
        Instantiate(endPrefab, new Vector3(endX, planeYOffset, 0f), Quaternion.Euler(-90, -90, 0));

        // Return the starting position for the planes
        return startPrefabPosition;
    }

    void SpawnSmallCubes()
    {
        int groupCount = Random.Range(5, 7); // Spawn between 5 to 7 groups of cubes
        float lastX = planeStartPosition.x;  // Start spawning cubes from the plane's start position

        for (int g = 0; g < groupCount; g++)
        {
            float randomX = lastX + Random.Range(400f, 500f); // Ensure at least 400-500 units of separation between groups
            int randomY = Random.Range(4, 20);               // Random height for the group center

            // Spawn a group of 3 cubes in the z-direction
            for (int i = 0; i < 3; i++)
            {
                float zPosition = -20f + (i * 20f); // Spawn cubes 20 units apart along the z-axis
                Vector3 cubePosition = new Vector3(randomX, randomY, zPosition);

                // Select a random prefab from the array and spawn it
                GameObject selectedPrefab = smallCubePrefabs[Random.Range(0, smallCubePrefabs.Length)];
                Instantiate(selectedPrefab, cubePosition, Quaternion.identity);
            }

            lastX = randomX; // Update the last x-position for the next group
        }
    }
}
