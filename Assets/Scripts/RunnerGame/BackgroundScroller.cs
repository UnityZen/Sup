using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    public float scrollSpeed = 0.5f;  // Базовая скорость прокрутки фона
    private Renderer backgroundRenderer;
    private float offset;
    public ObstacleSpawner obstacleSpawner;
    public float paralax;

    void Start()
    {
        // Получаем Renderer объекта для доступа к материалу
        backgroundRenderer = GetComponent<Renderer>();
    }

    void Update()
    {
        scrollSpeed = obstacleSpawner.obstacleSpeed;
        // Вычисляем офсет на основе времени и скорости прокрутки
        offset += Time.deltaTime * scrollSpeed/transform.localScale.x * paralax;
        // Обновляем офсет материала по оси X (для горизонтального движения фона)
        backgroundRenderer.material.mainTextureOffset = new Vector2(offset, 0);
    }

    // Метод для обновления скорости скроллинга
    public void UpdateScrollSpeed(float newSpeed)
    {
        scrollSpeed = newSpeed;
    }
}
