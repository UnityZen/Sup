using System.Collections;
using UnityEngine;

public class FlickerEffect : MonoBehaviour
{
    public float flickerSpeed = 1f; // Скорость изменения прозрачности
    public float minAlpha = 0.3f; // Минимальная прозрачность (полупрозрачность)
    public float maxAlpha = 1f; // Максимальная прозрачность (полная видимость)

    private SpriteRenderer spriteRenderer;
    private bool isFadingOut = false; // Для отслеживания текущего направления анимации

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer not found on the object.");
            return;
        }

        // Начинаем корутину для циклического изменения альфа-канала
        StartCoroutine(FlickerRoutine());
    }

    private IEnumerator FlickerRoutine()
    {
        while (true)
        {
            // Получаем текущий цвет спрайта
            Color color = spriteRenderer.color;

            // Если идём к уменьшению альфа-канала
            if (isFadingOut)
            {
                // Уменьшаем альфа
                color.a -= flickerSpeed * Time.deltaTime;
                if (color.a <= minAlpha)
                {
                    color.a = minAlpha; // Устанавливаем минимальный альфа
                    isFadingOut = false; // Меняем направление
                }
            }
            else
            {
                // Увеличиваем альфа
                color.a += flickerSpeed * Time.deltaTime;
                if (color.a >= maxAlpha)
                {
                    color.a = maxAlpha; // Устанавливаем максимальный альфа
                    isFadingOut = true; // Меняем направление
                }
            }

            // Применяем изменённый цвет к спрайту
            spriteRenderer.color = color;

            // Ожидаем один кадр, чтобы анимация была плавной
            yield return null;
        }
    }
}
