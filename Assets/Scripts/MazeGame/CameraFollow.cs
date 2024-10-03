using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;        // The target for the camera to follow (usually the player)
    public Vector3 offset;          // Offset from the target
    public float smoothSpeed = 0.125f;  // Smoothness factor for camera movement (lower = smoother)

    void FixedUpdate()  // Use FixedUpdate instead of LateUpdate
    {
        if (target != null)
        {
            // Desired position of the camera with the offset
            Vector3 desiredPosition = target.position + offset;

            // Smoothly move the camera towards the desired position
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

            // Update camera position
            transform.position = smoothedPosition;
        }
    }
}
