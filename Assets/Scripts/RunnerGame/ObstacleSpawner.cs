using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject[] obstacles;      // Префабы препятствий
    public float spawnRate = 2f;        // Начальная скорость спавна
    public float spawnRateDecrease = 0.05f; // Скорость уменьшения времени между спавнами
    public float minSpawnRate = 1.2f;     // Минимальный интервал между спавнами

    public float obstacleSpeed = 5f;    // Начальная скорость препятствий
    public float speedIncrease = 0.1f;  // Скорость увеличения скорости препятствий
    public float maxSpeed = 7f;        // Максимальная скорость

    private float nextSpawn = 0f;

    void Update()
    {
        // Спавн препятствий через определенный промежуток времени
        if (Time.time >= nextSpawn)
        {
            SpawnObstacle();
            nextSpawn = Time.time + spawnRate;
        }

        // Увеличение скорости препятствий с течением времени
        if (obstacleSpeed < maxSpeed)
        {
            obstacleSpeed += speedIncrease * Time.deltaTime;
            //Debug.Log(obstacleSpeed);
        }

        // Уменьшение интервала спавна с течением времени
        if (spawnRate > minSpawnRate)
        {
            spawnRate -= spawnRateDecrease * Time.deltaTime;
            Debug.Log(spawnRate);

        }
        else {
            spawnRate = minSpawnRate;
        }
    }

    void SpawnObstacle()
    {
        int randomIndex = Random.Range(0, obstacles.Length); // Выбор случайного препятствия

        GameObject obstacle = Instantiate(obstacles[randomIndex], new Vector3(transform.position.x, obstacles[randomIndex].transform.position.y, 0), Quaternion.identity);

        // Задаем скорость препятствию
        obstacle.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(-obstacleSpeed, 0);
    }
}
