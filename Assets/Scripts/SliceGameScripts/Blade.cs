using UnityEngine;

public class Blade : MonoBehaviour
{
    public TrailRenderer trail;       // Reference to the TrailRenderer for the blade's trail
    private Collider2D bladeCollider;  // The collider for the blade
    private Vector3 lastMousePosition; // Last known mouse position

    private void Start()
    {
        // Initialize the blade's collider
        bladeCollider = GetComponent<Collider2D>();
        bladeCollider.enabled = false;  // Disable the collider initially
        lastMousePosition = Input.mousePosition; // Initialize last mouse position
    }

    private void Update()
    {
        // Get the current position of the mouse in world space
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;  // Ensure the blade stays in 2D space

        // Move the blade directly to the mouse position
        transform.position = mousePosition;

        // Update last mouse position
        if (Input.GetMouseButton(0))
        {
            lastMousePosition = Input.mousePosition;
            lastMousePosition.z = 0f;
            bladeCollider.enabled = true;  // Enable the collider
            trail.enabled = true; // Enable trail
        }
        else
        {
            bladeCollider.enabled = false;  // Disable the collider when released
            trail.enabled = false; // Disable trail
        }
    }

    public Vector3 GetBladeDirection()
    {
        // Calculate the blade direction based on current and last mouse positions
        Vector3 currentMousePosition = Input.mousePosition;
        currentMousePosition.z = 0f;  // Ensure the position is 2D

        return (currentMousePosition - lastMousePosition).normalized; // Calculate direction
    }
}
