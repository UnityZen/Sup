using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyer : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle")|| collision.gameObject.CompareTag("LowObstacle") || collision.gameObject.CompareTag("HighObstacle") || collision.gameObject.CompareTag("ComboObstacle"))
        {
            Destroy(collision.gameObject);
        }
    }
}
