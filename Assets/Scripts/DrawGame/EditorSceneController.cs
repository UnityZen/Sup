using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class PointsData
{
    public Vector2[] points; // ������ ��������� �����
}

public class EditorSceneController : MonoBehaviour
{
    public Texture2D image; // �����������, �� ������� ����� ������������� �����
    public SpriteRenderer spriteRenderer; // SpriteRenderer ��� ����������� �����������
    public GameObject pointPrefab; // ������ ��� ������������ ����� �� �����
    public List<Vector2> points = new List<Vector2>(); // ������ ����� ��� ����������

    private string filePath; // ���� � ����� ����������

    void Start()
    {
        // ����������� ����������� �� �����
        if (image != null)
        {
            Sprite sprite = Sprite.Create(image, new Rect(0, 0, image.width, image.height), new Vector2(0.5f, 0.5f));
            spriteRenderer.sprite = sprite;
        }

        // ��������� ���� ��� ���������� ������
        filePath = Path.Combine(Application.persistentDataPath, "pointsData.json");
    }

    void Update()
    {
        // ���������, ���� ���� ������ ����� ������ ����
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 point = new Vector2(worldPosition.x, worldPosition.y);
            points.Add(point); // ��������� ����� � ������

            // ������ ������������ �����
            Instantiate(pointPrefab, new Vector3(point.x, point.y, 0), Quaternion.identity);
        }
    }

    // ����� ��� ���������� ����� � JSON
    public void SavePoints()
    {
        PointsData data = new PointsData();
        data.points = points.ToArray(); // ������������ ������ ����� � ������

        string json = JsonUtility.ToJson(data); // ������������ ������ � JSON ������
        File.WriteAllText(filePath, json); // ���������� JSON � ����

        Debug.Log($"Points saved to: {filePath}");
    }
}
