using UnityEngine;

public class FruitPart : MonoBehaviour
{
    public float minYPosition = -7f;  // ����������� ���������� Y, ���� ������� ������ ���������

    void Update()
    {
        if (transform.position.y < minYPosition)
        {
            Destroy(gameObject);  // ���������� ������ ��� ������ �� �����
        }
    }
}
