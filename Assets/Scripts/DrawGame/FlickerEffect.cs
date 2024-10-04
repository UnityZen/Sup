using System.Collections;
using UnityEngine;

public class FlickerEffect : MonoBehaviour
{
    public float flickerSpeed = 1f; // �������� ��������� ������������
    public float minAlpha = 0.3f; // ����������� ������������ (����������������)
    public float maxAlpha = 1f; // ������������ ������������ (������ ���������)

    private SpriteRenderer spriteRenderer;
    private bool isFadingOut = false; // ��� ������������ �������� ����������� ��������

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer not found on the object.");
            return;
        }

        // �������� �������� ��� ������������ ��������� �����-������
        StartCoroutine(FlickerRoutine());
    }

    private IEnumerator FlickerRoutine()
    {
        while (true)
        {
            // �������� ������� ���� �������
            Color color = spriteRenderer.color;

            // ���� ��� � ���������� �����-������
            if (isFadingOut)
            {
                // ��������� �����
                color.a -= flickerSpeed * Time.deltaTime;
                if (color.a <= minAlpha)
                {
                    color.a = minAlpha; // ������������� ����������� �����
                    isFadingOut = false; // ������ �����������
                }
            }
            else
            {
                // ����������� �����
                color.a += flickerSpeed * Time.deltaTime;
                if (color.a >= maxAlpha)
                {
                    color.a = maxAlpha; // ������������� ������������ �����
                    isFadingOut = true; // ������ �����������
                }
            }

            // ��������� ��������� ���� � �������
            spriteRenderer.color = color;

            // ������� ���� ����, ����� �������� ���� �������
            yield return null;
        }
    }
}
