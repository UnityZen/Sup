using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GameSceneController : MonoBehaviour
{
    public SpriteRenderer spriteRenderer; // SpriteRenderer для основного изображения
    public SpriteRenderer helpSpriteRenderer; // SpriteRenderer для изображения-подсказки
    public GameObject pointPrefab; // Префаб для точек
    public GameObject linePrefab; // Префаб для LineRenderer
    public float pointRadius = 0.1f; // Радиус коллайдеров для точек
    public List<Sprite> sprites; // Список всех спрайтов для уровней
    public List<Sprite> helpSprites; // Список спрайтов-подсказок
    public float lineFadeSpeed = 1f; // Скорость исчезновения линий

    private List<GameObject> pointObjects = new List<GameObject>(); // Объекты точек на сцене
    private List<Vector2> points = new List<Vector2>(); // Точки для текущего уровня
    private string currentSpriteName; // Имя текущего спрайта
    private string filePath; // Путь к файлу с точками
    private int currentSpriteIndex = 0; // Текущий индекс изображения/уровня
    private bool imageRevealed = false; // Флаг, указывающий, что изображение уже проявлено
    private int pointsCovered = 0; // Количество точек, через которые проведена линия

    void Start()
    {
        // Загружаем первый спрайт и точки
        LoadSpriteAndPoints(currentSpriteIndex);

        // Делаем основное изображение прозрачным
        Color color = spriteRenderer.color;
        color.a = 0f;
        spriteRenderer.color = color;

        // Подсказка всегда видна с частичной прозрачностью
        Color helpColor = helpSpriteRenderer.color;
        helpColor.a = 0.5f; // Примерное значение прозрачности подсказки
        helpSpriteRenderer.color = helpColor;
    }

    void Update()
    {
        // Начинаем рисовать линию при клике
        if (Input.GetMouseButtonDown(0) && !imageRevealed)
        {
            CreateNewLine();
        }

        // Продолжаем рисовать линию, пока удерживается кнопка мыши
        if (Input.GetMouseButton(0) && !imageRevealed)
        {
            UpdateCurrentLine();
        }

        // Переключение на следующее изображение/уровень
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            LoadNextSpriteAndPoints();
        }

        // Переключение на предыдущее изображение/уровень
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            LoadPreviousSpriteAndPoints();
        }
    }

    // Создаём новый LineRenderer при нажатии
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

        // Добавляем новую точку в линию
        currentLine.positionCount++;
        currentLine.SetPosition(currentLine.positionCount - 1, worldPosition);

        // Проверяем пересечение с точками
        foreach (GameObject pointObj in pointObjects)
        {
            if (pointObj != null)
            {
                CircleCollider2D collider = pointObj.GetComponent<CircleCollider2D>();

                // Если линия проходит через точку
                if (collider.OverlapPoint(worldPosition))
                {
                    pointObj.SetActive(false); // Отключаем точку
                    pointsCovered++;

                    // Если все точки покрыты линией, проявляем изображение
                    if (pointsCovered >= points.Count)
                    {
                        RevealImage();
                    }
                }
            }
        }
    }

    // Загрузка текущего спрайта и точек
    void LoadSpriteAndPoints(int spriteIndex)
    {
        if (spriteIndex < 0 || spriteIndex >= sprites.Count)
        {
            Debug.LogError("Invalid sprite index!");
            return;
        }

        // Загружаем спрайт
        Sprite selectedSprite = sprites[spriteIndex];
        spriteRenderer.sprite = selectedSprite;
        currentSpriteName = selectedSprite.name;

        // Загружаем изображение-подсказку
        Sprite selectedHelpSprite = helpSprites[spriteIndex];
        helpSpriteRenderer.sprite = selectedHelpSprite;

        // Устанавливаем полупрозрачность подсказки
        Color helpColor = helpSpriteRenderer.color;
        helpColor.a = 0.5f; // Примерная прозрачность
        helpSpriteRenderer.color = helpColor;

        // Загружаем точки для данного спрайта
        LoadPoints();

        // Удаляем старые объекты точек
        foreach (GameObject pointObj in pointObjects)
        {
            Destroy(pointObj);
        }
        pointObjects.Clear();

        // Спавним новые точки
        foreach (Vector2 point in points)
        {
            GameObject pointObj = Instantiate(pointPrefab, new Vector3(point.x, point.y, 0), Quaternion.identity);
            pointObjects.Add(pointObj);

            // Добавляем коллайдеры к каждой точке
            CircleCollider2D collider = pointObj.GetComponent<CircleCollider2D>();
            if (collider == null)
            {
                collider = pointObj.AddComponent<CircleCollider2D>();
                collider.radius = pointRadius;
                collider.isTrigger = true;
            }
        }

        pointsCovered = 0;
        imageRevealed = false;

        // Делаем изображение снова прозрачным
        Color color = spriteRenderer.color;
        color.a = 0f;
        spriteRenderer.color = color;
    }

    // Загрузка точек из файла для текущего изображения
    void LoadPoints()
    {
        filePath = Path.Combine(Application.persistentDataPath, currentSpriteName + "_points.json");

        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            PointsData data = JsonUtility.FromJson<PointsData>(json);
            points.Clear();
            points.AddRange(data.points);
            Debug.Log($"Loaded {points.Count} points for {currentSpriteName}.");
        }
        else
        {
            Debug.LogError($"Points data for {currentSpriteName} not found!");
        }
    }

    // Переключение на следующее изображение/уровень
    void LoadNextSpriteAndPoints()
    {
        currentSpriteIndex = (currentSpriteIndex + 1) % sprites.Count;
        LoadSpriteAndPoints(currentSpriteIndex);
    }

    // Переключение на предыдущее изображение/уровень
    void LoadPreviousSpriteAndPoints()
    {
        currentSpriteIndex = (currentSpriteIndex - 1 + sprites.Count) % sprites.Count;
        LoadSpriteAndPoints(currentSpriteIndex);
    }

    // Проявление изображения
    void RevealImage()
    {
        StartCoroutine(FadeInImage());
        StartCoroutine(FadeOutLines());
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

    // Постепенное исчезновение всех линий
    private IEnumerator FadeOutLines()
    {
        // Получаем все LineRenderer на сцене
        LineRenderer[] lineRenderers = FindObjectsOfType<LineRenderer>();

        while (true)
        {
            bool allLinesInvisible = true;

            // Для каждого LineRenderer уменьшаем альфа-канал его цвета
            foreach (LineRenderer line in lineRenderers)
            {
                Color startColor = line.startColor;
                Color endColor = line.endColor;

                // Уменьшаем альфа в зависимости от lineFadeSpeed
                startColor.a -= Time.deltaTime * lineFadeSpeed;
                endColor.a -= Time.deltaTime * lineFadeSpeed;

                // Ограничиваем значение альфа, чтобы не опускалась ниже нуля
                startColor.a = Mathf.Clamp(startColor.a, 0, 1);
                endColor.a = Mathf.Clamp(endColor.a, 0, 1);

                // Применяем новые цвета к LineRenderer
                line.startColor = startColor;
                line.endColor = endColor;

                // Если есть ещё видимые линии, продолжаем цикл
                if (startColor.a > 0 || endColor.a > 0)
                {
                    allLinesInvisible = false;
                }
            }

            // Если все линии стали невидимыми, прерываем цикл
            if (allLinesInvisible)
            {
                break;
            }

            yield return null; // Ждём следующий кадр
        }

        Debug.Log("All lines faded out!");
    }
}
