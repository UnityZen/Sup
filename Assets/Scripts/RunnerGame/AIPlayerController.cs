using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPlayerController : MonoBehaviour
{
    public float jumpForce = 10f;          // Сила прыжка
    public float crouchScaleY = 0.5f;      // Размер ИИ при пригибании
    public float crouchDuration = 1.0f;    // Время, которое ИИ находится в состоянии пригибания
    public float moveSpeed = 2f;           // Скорость движения ИИ к x = -7
    private Rigidbody2D rb;
    private Vector3 originalScale;         // Оригинальный размер ИИ
    private bool isGrounded = true;        // Находится ли ИИ на земле
    private bool isCrouching = false;      // Находится ли ИИ в состоянии пригибания
    private GameObject lastObstacle = null;// Ссылка на последнее препятствие
    private bool isMovingToPosition = true;// Состояние движения ИИ к позиции

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        originalScale = transform.localScale;
    }

    void Update()
    {
        // Если нет препятствий и ИИ должен двигаться влево
        if (isMovingToPosition)
        {
            MoveToPosition(-7f); // Двигаемся к x = -7
        }
    }

    // Метод для движения ИИ к координате x = -7
    void MoveToPosition(float targetX)
    {
        // Двигаем ИИ влево, пока его координата x больше чем targetX
        if (transform.position.x > targetX)
        {
            transform.position = new Vector3(
                transform.position.x - moveSpeed * Time.deltaTime,
                transform.position.y,
                transform.position.z
            );
        }
        else
        {
            // Как только ИИ достиг координаты x = targetX, останавливаем движение
            isMovingToPosition = false;
        }
    }

    // Прыжок
    void Jump()
    {
        if (isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce); // Выполняем прыжок
            isGrounded = false;
            isMovingToPosition = false; // Останавливаем движение во время прыжка
        }
    }

    // Пригибание
    void Crouch()
    {
        if (!isCrouching) // Если ИИ еще не в состоянии пригибания
        {
            isCrouching = true;
            transform.localScale = new Vector3(originalScale.x, crouchScaleY, originalScale.z); // Изменение размера ИИ
            StartCoroutine(CrouchTimer()); // Запускаем таймер для пригибания
            isMovingToPosition = false; // Останавливаем движение во время пригибания
        }
    }

    // Таймер пригибания
    IEnumerator CrouchTimer()
    {
        yield return new WaitForSeconds(crouchDuration); // Ожидание указанного времени
        StandUp(); // Возвращаемся в нормальное положение
        isMovingToPosition = true; // Возобновляем движение после пригибания
    }

    // Восстановление после пригибания
    void StandUp()
    {
        if (isCrouching)
        {
            transform.localScale = originalScale; // Возвращаем размер
            isCrouching = false;
        }
    }

    // Проверка на приземление
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            // Если ИИ пригнулся, возвращаемся в нормальное положение
          /*  if (isCrouching)
            {
                StandUp(); // Автоматически встаем, если находимся на земле
            }
          */
            // Сбрасываем препятствие после того, как ИИ касается земли
            lastObstacle = null;

            // После приземления продолжаем движение к x = -7
            isMovingToPosition = true;
        }
    }

    // Реакция на препятствия в триггерной зоне
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Если это то же самое препятствие, игнорируем
        if (lastObstacle == other.gameObject)
        {
            return; // Пропускаем повторное взаимодействие с тем же объектом
        }

        if (isGrounded)
        {
            // Обработка в зависимости от тега препятствия
            if (other.CompareTag("HighObstacle"))
            {
                Jump();  // Препятствие, которое нужно перепрыгнуть
            }
            else if (other.CompareTag("LowObstacle"))
            {
                Crouch();  // Препятствие, под которым нужно пригнуться
            }
            else if (other.CompareTag("ComboObstacle"))
            {
                JumpAndCrouch();  // Комбинированное препятствие
            }

            // Сохраняем текущее препятствие как последнее
            lastObstacle = other.gameObject;
        }

        // Когда обнаружено препятствие, ИИ перестает двигаться к x = -7
        isMovingToPosition = false;
    }

    // Комбинированное действие: прыжок и затем пригибание
    void JumpAndCrouch()
    {
        Jump(); // Сначала прыгаем
        Invoke("Crouch", 0.1f); // Через 0.5 секунд после прыжка пригибаемся
        isMovingToPosition = false; // Останавливаем движение во время комбинированного действия
    }
}
