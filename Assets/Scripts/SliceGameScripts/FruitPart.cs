using UnityEngine;

public class FruitPart : MonoBehaviour
{
    public float minYPosition = -7f;  // Минимальная координата Y, ниже которой объект удаляется

    void Update()
    {
        if (transform.position.y < minYPosition)
        {
            Destroy(gameObject);  // Уничтожаем объект при выходе за экран
        }
    }
}
