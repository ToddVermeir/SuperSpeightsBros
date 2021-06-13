using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float movementSpeed;
    public Rigidbody2D rb;
    public Animator anim;
    public bool facingRight;

    
    public float jumpForce = 5f;

    float mx;
    [SerializeField] public LayerMask groundLayers;
    [SerializeField] public Transform feet;

    private void Start()
    {
        facingRight = true;
    }

    private void Update()
    {
        mx = Input.GetAxis("Horizontal");
        

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            Jump();
        }
        if (Mathf.Abs(mx) > 0.05f)
        {
            anim.SetBool("isRunning", true);
        }
        else
        {
            anim.SetBool("isRunning", false);
        }
        
    }

    private void FixedUpdate()
    {
        Vector2 movement = new Vector2(mx * movementSpeed, rb.velocity.y);

        rb.velocity = movement;

        Flip(mx);



    }
    void Jump()
    {
        Vector2 movement = new Vector2(rb.velocity.x, jumpForce);

        rb.velocity = movement;
    }
    public bool IsGrounded()
    {
        Collider2D groundCheck = Physics2D.OverlapCircle(feet.position, 0.5f, groundLayers);

        if (groundCheck != null)
        {
            return true;
        }
        return false;
    }
    private void Flip(float mx)
    {
        if (mx > 0 && !facingRight || mx < 0 && facingRight)
        {
            facingRight = !facingRight;

            Vector3 theScale = transform.localScale;

            theScale.x*=-1;

            transform.localScale = theScale;
        }

    }
}