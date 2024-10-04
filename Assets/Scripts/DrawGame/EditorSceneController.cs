using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class PointsData
{
    public Vector2[] points; // Массив координат точек
}

public class EditorSceneController : MonoBehaviour
{
    public Texture2D image; // Изображение, на котором будут расставляться точки
    public SpriteRenderer spriteRenderer; // SpriteRenderer для отображения изображения
    public GameObject pointPrefab; // Префаб для визуализации точки на сцене
    public List<Vector2> points = new List<Vector2>(); // Список точек для сохранения

    private string filePath; // Путь к файлу сохранения
    private string spriteName; // Имя спрайта для создания имени файла

    void Start()
    {
        // Отображение изображения на сцене
        if (image != null)
        {
            // Создаём спрайт из текстуры
            Sprite sprite = Sprite.Create(image, new Rect(0, 0, image.width, image.height), new Vector2(0.5f, 0.5f));
            spriteRenderer.sprite = sprite;

            // Получаем имя текстуры для использования в названии файла
            spriteName = image.name;

            // Указываем путь для сохранения данных (будет использоваться spriteName)
            filePath = Path.Combine(Application.persistentDataPath, spriteName + "_points.json");
        }
        else
        {
            Debug.LogError("Image is not set.");
        }
    }

    void Update()
    {
        // Проверяем, если была нажата левая кнопка мыши
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 point = new Vector2(worldPosition.x, worldPosition.y);
            points.Add(point); // Добавляем точку в список

            // Создаём визуализацию точки
            Instantiate(pointPrefab, new Vector3(point.x, point.y, 0), Quaternion.identity);
        }

        // Проверка нажатия клавиши пробела для сохранения точек
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SavePoints(); // Сохраняем точки при нажатии пробела
        }
    }

    // Метод для сохранения точек в JSON
    public void SavePoints()
    {
        if (string.IsNullOrEmpty(spriteName))
        {
            Debug.LogError("Sprite name is empty. Cannot save points.");
            return;
        }

        // Создаём объект для сохранения данных
        PointsData data = new PointsData();
        data.points = points.ToArray(); // Конвертируем список точек в массив

        // Преобразуем объект данных в JSON строку
        string json = JsonUtility.ToJson(data);

        // Записываем JSON в файл, перезаписывая, если он уже существует
        File.WriteAllText(filePath, json);

        Debug.Log($"Points saved to: {filePath}");
    }
}
