using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle") || collision.gameObject.CompareTag("LowObstacle") || collision.gameObject.CompareTag("HighObstacle") || collision.gameObject.CompareTag("ComboObstacle"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Перезагрузка сцены при столкновении
        }
    }
}
