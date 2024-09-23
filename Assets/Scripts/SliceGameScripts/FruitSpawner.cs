using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitSpawner : MonoBehaviour
{
    public GameObject[] fruits; // ������ �������, ������� ����� ����������
    public float minSpawnDelay = 0.3f; // ����������� �������� ����� ������� �������
    public float maxSpawnDelay = 1.0f; // ������������ �������� ����� ������� �������
    public float minForce = 10f; // ����������� ���� ������������� ������
    public float maxForce = 20f; // ������������ ���� ������������� ������
    public float minTorque = -10f; // ����������� ��������� ������
    public float maxTorque = 10f; // ������������ ��������� ������
    public float spawnXMin = -8f; // ����������� ���������� X ��� ������
    public float spawnXMax = 8f; // ������������ ���������� X ��� ������
    public float spawnY = -6f; // Y ���������� ��� ������ (�� ��������� ������ �����)

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

            // �������� ��������� ����� � ��������� ����� ��� ��� ������ �� X
            Vector3 spawnPosition = new Vector3(Random.Range(spawnXMin, spawnXMax), spawnY, 0);
            GameObject fruit = fruits[Random.Range(0, fruits.Length)];

            // ������� �����
            GameObject spawnedFruit = Instantiate(fruit, spawnPosition, Quaternion.identity);

            // ��������� ������ ��� ������������� �����
            Rigidbody2D rb = spawnedFruit.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                // ��������� ��������� ���� �����
                float force = Random.Range(minForce, maxForce);
                rb.AddForce(new Vector2(0, force), ForceMode2D.Impulse);

                // ��������� ��������� ��������� ������
                float torque = Random.Range(minTorque, maxTorque);
                rb.AddTorque(torque);
            }
        }
    }
}
