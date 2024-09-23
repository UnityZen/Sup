using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitSpawner : MonoBehaviour
{
    public GameObject[] fruits; // Массив фруктов, которые будут спавниться
    public float minSpawnDelay = 0.3f; // Минимальная задержка между спавном фруктов
    public float maxSpawnDelay = 1.0f; // Максимальная задержка между спавном фруктов
    public float minForce = 10f; // Минимальная сила подбрасывания фрукта
    public float maxForce = 20f; // Максимальная сила подбрасывания фрукта
    public float minTorque = -10f; // Минимальный вращающий момент
    public float maxTorque = 10f; // Максимальный вращающий момент
    public float spawnXMin = -8f; // Минимальная координата X для спавна
    public float spawnXMax = 8f; // Максимальная координата X для спавна
    public float spawnY = -6f; // Y координата для спавна (за пределами экрана снизу)

    void Start()
    {
        StartCoroutine(SpawnFruits());
    }

    IEnumerator SpawnFruits()
    {
        while (true)
        {
            float delay = Random.Range(minSpawnDelay, maxSpawnDelay);
            yield return new WaitForSeconds(delay);

            // Выбираем случайный фрукт и случайное место для его спавна по X
            Vector3 spawnPosition = new Vector3(Random.Range(spawnXMin, spawnXMax), spawnY, 0);
            GameObject fruit = fruits[Random.Range(0, fruits.Length)];

            // Спавним фрукт
            GameObject spawnedFruit = Instantiate(fruit, spawnPosition, Quaternion.identity);

            // Добавляем физику для подбрасывания вверх
            Rigidbody2D rb = spawnedFruit.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                // Применяем случайную силу вверх
                float force = Random.Range(minForce, maxForce);
                rb.AddForce(new Vector2(0, force), ForceMode2D.Impulse);

                // Применяем случайный вращающий момент
                float torque = Random.Range(minTorque, maxTorque);
                rb.AddTorque(torque);
            }
        }
    }
}
