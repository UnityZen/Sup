using System.Collections;
using UnityEngine;

public class DelayedActivator : MonoBehaviour
{
    public GameObject objectToActivate; // Объект, который нужно активировать
    public float delay = 2f; // Задержка перед активацией (в секундах)

    void Start()
    {
        // Запускаем корутину для активации объекта с задержкой
        StartCoroutine(ActivateAfterDelay());
    }

    // Корутина для активации объекта с задержкой
    IEnumerator ActivateAfterDelay()
    {
        // Ждем указанное количество времени
        yield return new WaitForSeconds(delay);

        // Активируем объект
        objectToActivate.SetActive(true);
    }
}
