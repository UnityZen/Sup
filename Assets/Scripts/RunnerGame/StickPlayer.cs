using UnityEngine;

public class StickPLayer : MonoBehaviour
{
    // Reference to the target object you want to stick to
    public Transform targetObject;

    // Optionally, maintain an offset between the objects
    public Vector3 offset;
    public Animator animator;
    public float groundValue;

    // Update is called once per frame
    void Update()
    {
       
        transform.position = targetObject.position + offset;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetTrigger("Jumping");
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            animator.SetBool("isSwimming", true);
        }
        if(Input.GetKeyUp(KeyCode.DownArrow))
        {
            animator.SetBool("isSwimming", false);

            //animator.SetTrigger("Running");
        }

        if (transform.position.y < groundValue)
        {
            transform.position = new Vector3(transform.position.x, groundValue, transform.position.z);
        }

    }
}
