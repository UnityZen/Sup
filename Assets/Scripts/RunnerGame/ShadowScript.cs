using UnityEngine;

public class ShadowScript : MonoBehaviour
{
    // Reference to the object casting the shadow
    public Transform targetObject;

    // Fixed Y position for the shadow (ground level)
    public float fixedYPosition = 0f;

    // Minimum and maximum Y position for scaling
    public float minYPosition = -1.7f; // When the object is at this Y, shadow is full size
    public float maxYPosition = 3f;    // When the object is at this Y, shadow is almost 0

    // Initial full scale of the shadow (at minYPosition)
    public float initialScale = 0.09f;

    // Minimum scale of the shadow (at maxYPosition)
    public float minScale = 0.01f; // Prevents the shadow from being completely 0

    // Update is called once per frame
    void Update()
    {
        if (targetObject != null)
        {
            // Stick the shadow to the target object's X and Z position, but maintain a fixed Y position
            transform.position = new Vector3(targetObject.position.x, fixedYPosition, targetObject.position.z);

            // Calculate the proportion of how far the object is between minY and maxY
            float normalizedHeight = Mathf.InverseLerp(minYPosition, maxYPosition, targetObject.position.y);

            // Calculate the new scale based on the height, between the initialScale and minScale
            float newScale = Mathf.Lerp(initialScale, minScale, normalizedHeight);

            // Apply the new scale uniformly to the shadow's x and z axes, keep y unchanged
            transform.localScale = new Vector3(newScale, transform.localScale.y, newScale);
        }
    }
}
