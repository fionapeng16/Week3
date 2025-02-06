using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] float speed = 1f;
    [SerializeField] float jumpHeight = 3f;
    [SerializeField] float dashSpeed = 5f;
    [SerializeField] float dashDuration = 0.2f;
    float direction = 0;
    bool isGrounded = false;
    bool isDashing = false;
    bool isFacingRight = true;

    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDashing)
            Move(direction);
        if (isFacingRight && direction == -1 || !isFacingRight && direction == 1){
            Flip();
        }
    }

    void OnMove(InputValue value)
    {
        float v = value.Get<float>();
        direction = v;
    }

    void Move(float dir)
    {
        rb.velocity = new Vector2(dir * speed, rb.velocity.y);
        anim.SetBool("isRunning", dir != 0);
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

    void OnDash(InputValue value)
    {
        Debug.Log("Dash Input Received!");
        if (!isDashing)
        {
            StartCoroutine(Dash());
        }
    }

    IEnumerator Dash()
    {
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(direction * dashSpeed, 0f);
        yield return new WaitForSeconds(dashDuration);
        rb.gravityScale = originalGravity;
        isDashing = false;
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

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 newLocalScale = transform.localScale;
        newLocalScale.x *= -1f;
        transform.localScale = newLocalScale;
    }
}
