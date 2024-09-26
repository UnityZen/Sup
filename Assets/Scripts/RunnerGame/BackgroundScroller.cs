using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    public float scrollSpeed = 0.5f;  // ������� �������� ��������� ����
    private Renderer backgroundRenderer;
    private float offset;
    public ObstacleSpawner obstacleSpawner;
    public float paralax;

    void Start()
    {
        // �������� Renderer ������� ��� ������� � ���������
        backgroundRenderer = GetComponent<Renderer>();
    }

    void Update()
    {
        scrollSpeed = obstacleSpawner.obstacleSpeed;
        // ��������� ����� �� ������ ������� � �������� ���������
        offset += Time.deltaTime * scrollSpeed/transform.localScale.x * paralax;
        // ��������� ����� ��������� �� ��� X (��� ��������������� �������� ����)
        backgroundRenderer.material.mainTextureOffset = new Vector2(offset, 0);
    }

    // ����� ��� ���������� �������� ����������
    public void UpdateScrollSpeed(float newSpeed)
    {
        scrollSpeed = newSpeed;
    }
}
