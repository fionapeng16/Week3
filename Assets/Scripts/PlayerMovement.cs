using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] float speed = 1f;
    [SerializeField] float jumpHeight = 3f;
    float direction = 0;
    bool isGrounded = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Move(direction);
    }

    void OnMove(InputValue value)
    {
        float v = value.Get<float>();
        direction = v;
    }

    void Move(float dir)
    {
        rb.velocity = new Vector2(dir * speed, rb.velocity.y);
    }

    void OnJump()
    {
        if (isGrounded)
            Jump();
    }

    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {

    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
            for (int i = 0; i < collision.contactCount; i++)
            {
                if (Vector2.Angle(collision.GetContact(i).normal, Vector2.up) < 45f)
                {
                    isGrounded = true;
                }
            }
        }

    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}
