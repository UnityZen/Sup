using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject[] obstacles;      // Prefabs for the obstacles
    public float spawnRate = 2f;        // Initial spawn rate
    public float spawnRateDecrease = 0.05f; // Rate at which spawn interval decreases
    public float minSpawnRate = 1.2f;   // Minimum spawn interval

    public float obstacleSpeed = 5f;    // Initial speed of the obstacles
    public float speedIncrease = 0.1f;  // Rate at which obstacle speed increases
    public float maxSpeed = 7f;         // Maximum speed for the obstacles

    private float nextSpawn = 0f;

    void Update()
    {
        // Handle obstacle spawning based on time
        if (Time.time >= nextSpawn)
        {
            SpawnObstacle();
            nextSpawn = Time.time + spawnRate;
        }

        // Increase obstacle speed over time
        if (obstacleSpeed < maxSpeed)
        {
            obstacleSpeed += speedIncrease * Time.deltaTime;
        }

        // Decrease spawn rate over time
        if (spawnRate > minSpawnRate)
        {
            spawnRate -= spawnRateDecrease * Time.deltaTime;
        }
        else
        {
            spawnRate = minSpawnRate;
        }
    }

    void SpawnObstacle()
    {
        int randomIndex = Random.Range(0, obstacles.Length); // Select a random obstacle

        // Instantiate the obstacle at the spawner's position
        GameObject obstacle = Instantiate(obstacles[randomIndex], new Vector3(transform.position.x, obstacles[randomIndex].transform.position.y, 0), Quaternion.identity);

        // Start the movement coroutine
        StartCoroutine(MoveObstacle(obstacle));
    }

    IEnumerator MoveObstacle(GameObject obstacle)
    {
        while (obstacle != null)
        {
            // Move the obstacle smoothly to the left
            obstacle.transform.Translate(Vector2.left * obstacleSpeed * Time.deltaTime);

            // Destroy the obstacle when it moves off-screen
            if (obstacle.transform.position.x < -10f) // Adjust this value based on your screen width
            {
                Destroy(obstacle);
                yield break;
            }

            yield return null;
        }
    }
}
