using UnityEngine;

public class HandleCrystal : MonoBehaviour
{
    public GameManager gameManager; // ������ �� GameManager
    private void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>();
    }
    private void OnMouseDown()
    {
        // ���������� ������� �� ��������
        Crystal crystal = GetComponent<Crystal>();
        if (crystal != null)
        {
            gameManager.HandleCrystalClick(crystal); // �������� ��������� �������� � GameManager
        }
    }
}
