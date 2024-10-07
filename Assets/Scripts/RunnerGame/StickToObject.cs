using UnityEngine;

public class StickToObject : MonoBehaviour
{
    // Reference to the target object you want to stick to
    public Transform targetObject;

    // Optionally, maintain an offset between the objects
    public Vector3 offset;
    public Animator animator;

    // Update is called once per frame
    void Update()
    {
        if (targetObject != null)
        {
            // Set this object's position to the target object's position plus the offset
            transform.position = targetObject.position + offset;
            if(transform.position.y < -1.7)
            {
                transform.position = new Vector3(transform.position.x, -1.7f, transform.position.z);
            }
        }
      
    }
}
