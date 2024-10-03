using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;  // For Light2D

public class FlashlightController : MonoBehaviour
{
    public Light2D flashlight;    // Light2D component for the flashlight
    public float lightRange = 10f; // Maximum range of the flashlight beam
    public LayerMask obstructionLayer; // Layer mask for obstacles (walls, etc.)

    void Start()
    {
        if (flashlight == null)
        {
            flashlight = GetComponentInChildren<Light2D>();  // Automatically find the Light2D component
        }
        flashlight.pointLightOuterRadius = lightRange; // Set initial light range
    }

    void Update()
    {
        HandleFlashlightBeam();
    }

    void HandleFlashlightBeam()
    {
        // Cast a ray from the player forward (flashlight direction) to detect obstacles
        Vector3 direction = transform.right;  // Assuming the flashlight is pointing right (adjust if needed)
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, lightRange, obstructionLayer);

        if (hit.collider != null)
        {
            // Obstruction detected, adjust the light range to the point of collision
            float distanceToObstacle = Vector2.Distance(transform.position, hit.point);
            flashlight.pointLightOuterRadius = distanceToObstacle;  // Shorten light to obstacle distance
        }
        else
        {
            // No obstruction, set light range to maximum
            flashlight.pointLightOuterRadius = lightRange;
        }
    }

    // This method is for debugging, so you can visualize the raycast in Scene view
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, transform.right * lightRange);
    }
}
