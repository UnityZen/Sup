using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GameSceneController : MonoBehaviour
{
    public SpriteRenderer spriteRenderer; // SpriteRenderer ��� ��������� �����������
    public SpriteRenderer helpSpriteRenderer; // SpriteRenderer ��� �����������-���������
    public GameObject pointPrefab; // ������ ��� �����
    public GameObject linePrefab; // ������ ��� LineRenderer
    public float pointRadius = 0.1f; // ������ ����������� ��� �����
    public List<Sprite> sprites; // ������ ���� �������� ��� �������
    public List<Sprite> helpSprites; // ������ ��������-���������
    public float lineFadeSpeed = 1f; // �������� ������������ �����

    private List<GameObject> pointObjects = new List<GameObject>(); // ������� ����� �� �����
    private List<Vector2> points = new List<Vector2>(); // ����� ��� �������� ������
    private string currentSpriteName; // ��� �������� �������
    private string filePath; // ���� � ����� � �������
    private int currentSpriteIndex = 0; // ������� ������ �����������/������
    private bool imageRevealed = false; // ����, �����������, ��� ����������� ��� ���������
    private int pointsCovered = 0; // ���������� �����, ����� ������� ��������� �����

    void Start()
    {
        // ��������� ������ ������ � �����
        LoadSpriteAndPoints(currentSpriteIndex);

        // ������ �������� ����������� ����������
        Color color = spriteRenderer.color;
        color.a = 0f;
        spriteRenderer.color = color;

        // ��������� ������ ����� � ��������� �������������
        Color helpColor = helpSpriteRenderer.color;
        helpColor.a = 0.5f; // ��������� �������� ������������ ���������
        helpSpriteRenderer.color = helpColor;
    }

    void Update()
    {
        // �������� �������� ����� ��� �����
        if (Input.GetMouseButtonDown(0) && !imageRevealed)
        {
            CreateNewLine();
        }

        // ���������� �������� �����, ���� ������������ ������ ����
        if (Input.GetMouseButton(0) && !imageRevealed)
        {
            UpdateCurrentLine();
        }

        // ������������ �� ��������� �����������/�������
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            LoadNextSpriteAndPoints();
        }

        // ������������ �� ���������� �����������/�������
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            LoadPreviousSpriteAndPoints();
        }
    }

    // ������ ����� LineRenderer ��� �������
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

                // ���� ����� �������� ����� �����
                if (collider.OverlapPoint(worldPosition))
                {
                    pointObj.SetActive(false); // ��������� �����
                    pointsCovered++;

                    // ���� ��� ����� ������� ������, ��������� �����������
                    if (pointsCovered >= points.Count)
                    {
                        RevealImage();
                    }
                }
            }
        }
    }

    // �������� �������� ������� � �����
    void LoadSpriteAndPoints(int spriteIndex)
    {
        if (spriteIndex < 0 || spriteIndex >= sprites.Count)
        {
            Debug.LogError("Invalid sprite index!");
            return;
        }

        // ��������� ������
        Sprite selectedSprite = sprites[spriteIndex];
        spriteRenderer.sprite = selectedSprite;
        currentSpriteName = selectedSprite.name;

        // ��������� �����������-���������
        Sprite selectedHelpSprite = helpSprites[spriteIndex];
        helpSpriteRenderer.sprite = selectedHelpSprite;

        // ������������� ���������������� ���������
        Color helpColor = helpSpriteRenderer.color;
        helpColor.a = 0.5f; // ��������� ������������
        helpSpriteRenderer.color = helpColor;

        // ��������� ����� ��� ������� �������
        LoadPoints();

        // ������� ������ ������� �����
        foreach (GameObject pointObj in pointObjects)
        {
            Destroy(pointObj);
        }
        pointObjects.Clear();

        // ������� ����� �����
        foreach (Vector2 point in points)
        {
            GameObject pointObj = Instantiate(pointPrefab, new Vector3(point.x, point.y, 0), Quaternion.identity);
            pointObjects.Add(pointObj);

            // ��������� ���������� � ������ �����
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

        // ������ ����������� ����� ����������
        Color color = spriteRenderer.color;
        color.a = 0f;
        spriteRenderer.color = color;
    }

    // �������� ����� �� ����� ��� �������� �����������
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

    // ������������ �� ��������� �����������/�������
    void LoadNextSpriteAndPoints()
    {
        currentSpriteIndex = (currentSpriteIndex + 1) % sprites.Count;
        LoadSpriteAndPoints(currentSpriteIndex);
    }

    // ������������ �� ���������� �����������/�������
    void LoadPreviousSpriteAndPoints()
    {
        currentSpriteIndex = (currentSpriteIndex - 1 + sprites.Count) % sprites.Count;
        LoadSpriteAndPoints(currentSpriteIndex);
    }

    // ���������� �����������
    void RevealImage()
    {
        StartCoroutine(FadeInImage());
        StartCoroutine(FadeOutLines());
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

    // ����������� ������������ ���� �����
    private IEnumerator FadeOutLines()
    {
        // �������� ��� LineRenderer �� �����
        LineRenderer[] lineRenderers = FindObjectsOfType<LineRenderer>();

        while (true)
        {
            bool allLinesInvisible = true;

            // ��� ������� LineRenderer ��������� �����-����� ��� �����
            foreach (LineRenderer line in lineRenderers)
            {
                Color startColor = line.startColor;
                Color endColor = line.endColor;

                // ��������� ����� � ����������� �� lineFadeSpeed
                startColor.a -= Time.deltaTime * lineFadeSpeed;
                endColor.a -= Time.deltaTime * lineFadeSpeed;

                // ������������ �������� �����, ����� �� ���������� ���� ����
                startColor.a = Mathf.Clamp(startColor.a, 0, 1);
                endColor.a = Mathf.Clamp(endColor.a, 0, 1);

                // ��������� ����� ����� � LineRenderer
                line.startColor = startColor;
                line.endColor = endColor;

                // ���� ���� ��� ������� �����, ���������� ����
                if (startColor.a > 0 || endColor.a > 0)
                {
                    allLinesInvisible = false;
                }
            }

            // ���� ��� ����� ����� ����������, ��������� ����
            if (allLinesInvisible)
            {
                break;
            }

            yield return null; // ��� ��������� ����
        }

        Debug.Log("All lines faded out!");
    }
}
