using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GameSceneController : MonoBehaviour
{
    public SpriteRenderer spriteRenderer; // SpriteRenderer для изображения
    public GameObject pointPrefab; // Префаб для точек
    public GameObject linePrefab; // Префаб для LineRenderer
    public float pointRadius = 0.1f; // Радиус для кругов вокруг точек

    private List<GameObject> pointObjects = new List<GameObject>(); // Список точек на сцене
    private List<Vector2> points = new List<Vector2>(); // Координаты точек из файла
    private string filePath; // Путь к файлу точек
    private bool imageRevealed = false; // Флаг, чтобы показать, что изображение уже проявлено
    private int pointsCovered = 0; // Счётчик точек, через которые прошла линия

    void Start()
    {
        // Путь к файлу данных с точками
        filePath = Path.Combine(Application.persistentDataPath, "pointsData.json");

        // Загружаем и спавним точки
        LoadPointsAndSpawn();

        // Делаем изображение невидимым в начале
        Color color = spriteRenderer.color;
        color.a = 0f;
        spriteRenderer.color = color;
    }

    void Update()
    {
        // Начинаем рисовать линии, если игрок нажал на экран
        if (Input.GetMouseButtonDown(0) && !imageRevealed)
        {
            CreateNewLine();
        }

        // Если кнопка мыши удерживается, продолжаем рисовать линию
        if (Input.GetMouseButton(0) && !imageRevealed)
        {
            UpdateCurrentLine();
        }
    }

    // Создаём новый LineRenderer при клике
    void CreateNewLine()
    {
        GameObject newLine = Instantiate(linePrefab);
        LineRenderer lineRenderer = newLine.GetComponent<LineRenderer>();
        lineRenderer.positionCount = 0; // Обнуляем количество точек в линии
    }

    // Обновляем текущую линию и проверяем пересечение с точками
    void UpdateCurrentLine()
    {
        LineRenderer currentLine = FindObjectOfType<LineRenderer>();
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        worldPosition.z = 0;

        // Добавляем новые точки в линию
        currentLine.positionCount++;
        currentLine.SetPosition(currentLine.positionCount - 1, worldPosition);

        // Проверяем пересечение с точками
        foreach (GameObject pointObj in pointObjects)
        {
            if (pointObj != null)
            {
                CircleCollider2D collider = pointObj.GetComponent<CircleCollider2D>();

                // Если линия проходит через точку и она ещё не была покрыта
                if (collider.OverlapPoint(worldPosition))
                {
                    pointObj.SetActive(false); // Отключаем точку
                    pointsCovered++;

                    // Если все точки покрыты, проявляем изображение
                    if (pointsCovered >= points.Count)
                    {
                        RevealImage();
                    }
                }
            }
        }
    }

    // Загрузка точек из JSON и спавн точек на сцене
    void LoadPointsAndSpawn()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            PointsData data = JsonUtility.FromJson<PointsData>(json);
            points.AddRange(data.points);

            foreach (Vector2 point in points)
            {
                // Спавним префабы точек на каждой координате
                GameObject pointObj = Instantiate(pointPrefab, new Vector3(point.x, point.y, 0), Quaternion.identity);
                pointObjects.Add(pointObj);

                // Добавляем коллайдер для каждой точки
                CircleCollider2D collider = pointObj.GetComponent<CircleCollider2D>();
                if (collider == null)
                {
                    collider = pointObj.AddComponent<CircleCollider2D>();
                    collider.radius = pointRadius; // Устанавливаем радиус
                    collider.isTrigger = true; // Делаем коллайдер триггером
                }
            }

            Debug.Log($"Loaded {points.Count} points from: {filePath}");
        }
        else
        {
            Debug.LogError("Points data file not found!");
        }
    }

    // Метод для проявления изображения
    void RevealImage()
    {
        StartCoroutine(FadeInImage());
    }

    // Постепенное проявление изображения
    private IEnumerator FadeInImage()
    {
        imageRevealed = true;
        Color color = spriteRenderer.color;

        while (color.a < 1f)
        {
            color.a += Time.deltaTime;
            spriteRenderer.color = color;
            yield return null;
        }

        Debug.Log("Image revealed!");
    }
}
