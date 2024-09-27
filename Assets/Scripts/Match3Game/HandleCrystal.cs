using UnityEngine;

public class HandleCrystal : MonoBehaviour
{
    public GameManager gameManager; // Ссылка на GameManager
    private void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>();
    }
    private void OnMouseDown()
    {
        // Обработчик нажатия на кристалл
        Crystal crystal = GetComponent<Crystal>();
        if (crystal != null)
        {
            gameManager.HandleCrystalClick(crystal); // Передаем выбранный кристалл в GameManager
        }
    }
}
