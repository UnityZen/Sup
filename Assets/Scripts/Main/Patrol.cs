using UnityEngine;

public class Patrol : MonoBehaviour
{
    public float speed = 2f; // Speed of the patrol
    public float leftLimit = -5f; // Left limit of the patrol
    public float rightLimit = 5f; // Right limit of the patrol

    private bool movingRight = true; // Determine direction of movement

    void Update()
    {
        // If moving right and reached the right limit, flip direction
        if (transform.position.x >= rightLimit)
        {
            movingRight = false;
            Flip(-1); // Face left
        }

        // If moving left and reached the left limit, flip direction
        else if (transform.position.x <= leftLimit)
        {
            movingRight = true;
            Flip(1); // Face right
        }

        // Move the object
        float move = speed * Time.deltaTime * (movingRight ? 1 : -1);
        transform.Translate(move, 0, 0);
    }

    // Flip the object by adjusting the Y scale
    void Flip(float direction)
    {
        Vector3 scale = transform.localScale;
        scale.x = direction;
        transform.localScale = scale;
    }
}
