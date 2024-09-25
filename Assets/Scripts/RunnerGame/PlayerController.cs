using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float jumpForce = 10f;      
    public float crouchScaleY = 0.5f;   
    private bool isGrounded = true;     
    private Rigidbody2D rb;
    private Vector3 originalScale;      

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        originalScale = transform.localScale;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }
        
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Crouch();
        }

        if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            StandUp();
        }
    }

    void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        isGrounded = false;
    }

    void Crouch()
    {
        transform.localScale = new Vector3(originalScale.x, crouchScaleY, originalScale.z);
    }

    void StandUp()
    {
        transform.localScale = originalScale;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
}
