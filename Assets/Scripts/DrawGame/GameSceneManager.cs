using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GameSceneController : MonoBehaviour
{
    public SpriteRenderer spriteRenderer; // SpriteRenderer ��� �����������
    public GameObject pointPrefab; // ������ ��� �����
    public GameObject linePrefab; // ������ ��� LineRenderer
    public float pointRadius = 0.1f; // ������ ��� ������ ������ �����

    private List<GameObject> pointObjects = new List<GameObject>(); // ������ ����� �� �����
    private List<Vector2> points = new List<Vector2>(); // ���������� ����� �� �����
    private string filePath; // ���� � ����� �����
    private bool imageRevealed = false; // ����, ����� ��������, ��� ����������� ��� ���������
    private int pointsCovered = 0; // ������� �����, ����� ������� ������ �����

    void Start()
    {
        // ���� � ����� ������ � �������
        filePath = Path.Combine(Application.persistentDataPath, "pointsData.json");

        // ��������� � ������� �����
        LoadPointsAndSpawn();

        // ������ ����������� ��������� � ������
        Color color = spriteRenderer.color;
        color.a = 0f;
        spriteRenderer.color = color;
    }

    void Update()
    {
        // �������� �������� �����, ���� ����� ����� �� �����
        if (Input.GetMouseButtonDown(0) && !imageRevealed)
        {
            CreateNewLine();
        }

        // ���� ������ ���� ������������, ���������� �������� �����
        if (Input.GetMouseButton(0) && !imageRevealed)
        {
            UpdateCurrentLine();
        }
    }

    // ������ ����� LineRenderer ��� �����
    void CreateNewLine()
    {
        GameObject newLine = Instantiate(linePrefab);
        LineRenderer lineRenderer = newLine.GetComponent<LineRenderer>();
        lineRenderer.positionCount = 0; // �������� ���������� ����� � �����
    }

    // ��������� ������� ����� � ��������� ����������� � �������
    void UpdateCurrentLine()
    {
        LineRenderer currentLine = FindObjectOfType<LineRenderer>();
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        worldPosition.z = 0;

        // ��������� ����� ����� � �����
        currentLine.positionCount++;
        currentLine.SetPosition(currentLine.positionCount - 1, worldPosition);

        // ��������� ����������� � �������
        foreach (GameObject pointObj in pointObjects)
        {
            if (pointObj != null)
            {
                CircleCollider2D collider = pointObj.GetComponent<CircleCollider2D>();

                // ���� ����� �������� ����� ����� � ��� ��� �� ���� �������
                if (collider.OverlapPoint(worldPosition))
                {
                    pointObj.SetActive(false); // ��������� �����
                    pointsCovered++;

                    // ���� ��� ����� �������, ��������� �����������
                    if (pointsCovered >= points.Count)
                    {
                        RevealImage();
                    }
                }
            }
        }
    }

    // �������� ����� �� JSON � ����� ����� �� �����
    void LoadPointsAndSpawn()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            PointsData data = JsonUtility.FromJson<PointsData>(json);
            points.AddRange(data.points);

            foreach (Vector2 point in points)
            {
                // ������� ������� ����� �� ������ ����������
                GameObject pointObj = Instantiate(pointPrefab, new Vector3(point.x, point.y, 0), Quaternion.identity);
                pointObjects.Add(pointObj);

                // ��������� ��������� ��� ������ �����
                CircleCollider2D collider = pointObj.GetComponent<CircleCollider2D>();
                if (collider == null)
                {
                    collider = pointObj.AddComponent<CircleCollider2D>();
                    collider.radius = pointRadius; // ������������� ������
                    collider.isTrigger = true; // ������ ��������� ���������
                }
            }

            Debug.Log($"Loaded {points.Count} points from: {filePath}");
        }
        else
        {
            Debug.LogError("Points data file not found!");
        }
    }

    // ����� ��� ���������� �����������
    void RevealImage()
    {
        StartCoroutine(FadeInImage());
    }

    // ����������� ���������� �����������
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
