using UnityEngine;

public class Fruit : MonoBehaviour
{
    public GameObject leftHalfPrefab;   // Prefab for the left half of the fruit
    public GameObject rightHalfPrefab;  // Prefab for the right half of the fruit
    public float sliceForce = 5f;       // Force applied to each half when sliced
    public float minYPosition = -7f;    // Y position threshold for destroying off-screen halves

    private Blade blade;  // Reference to the Blade script

    private void Start()
    {
        // Find the blade in the scene (assuming there is one Blade object)
        blade = FindObjectOfType<Blade>();
    }

    private void Update()
    {
        // Destroy fruit if it goes below the screen (off-screen)
        if (transform.position.y < minYPosition)
        {
            Destroy(gameObject);
        }
    }

    // Method to slice the fruit into two halves
    public void Slice(Vector3 bladeDirection)
    {
        // Instantiate the two halves at the fruit's current position
        GameObject leftHalf = Instantiate(leftHalfPrefab, transform.position, Quaternion.identity);
        GameObject rightHalf = Instantiate(rightHalfPrefab, transform.position, Quaternion.identity);

        // Calculate the angle of the slice based on the blade direction
        float angle = Mathf.Atan2(bladeDirection.y, bladeDirection.x) * Mathf.Rad2Deg;

        // Rotate the halves based on the slice direction
        leftHalf.transform.rotation = Quaternion.Euler(0, 0, angle - 90f);  // Rotate left half
        rightHalf.transform.rotation = Quaternion.Euler(0, 0, angle - 90f); // Rotate right half

        // Get Rigidbody2D components for each half
        Rigidbody2D leftRb = leftHalf.GetComponent<Rigidbody2D>();
        Rigidbody2D rightRb = rightHalf.GetComponent<Rigidbody2D>();

        // Apply forces to the halves in opposite directions
        Vector2 perpendicularDir = Vector2.Perpendicular(bladeDirection);  // Get a perpendicular direction to the blade
        leftRb.AddForce((perpendicularDir + Vector2.up) * sliceForce, ForceMode2D.Impulse);
        rightRb.AddForce((-perpendicularDir + Vector2.down) * sliceForce, ForceMode2D.Impulse);

        // Apply random torque for rotation
        leftRb.AddTorque(Random.Range(-30f, 30f));
        rightRb.AddTorque(Random.Range(-30f, 30f));

        // Destroy the original fruit
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Blade"))
        {
            // Get the blade's movement direction from the Blade script
            Vector3 bladeDirection = blade.GetBladeDirection();
            Debug.Log(bladeDirection);
            // Slice the fruit when the blade hits
            Slice(bladeDirection);
        }
    }
}
