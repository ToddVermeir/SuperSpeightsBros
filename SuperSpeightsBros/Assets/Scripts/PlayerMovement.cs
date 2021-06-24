using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    public Healthbar healthBar;
    public float movementSpeed;
    public Rigidbody2D rb;
    public Animator anim;
    public bool facingRight;

    [SerializeField] public bool canFlip;

    public float jumpForce = 5f;

    public float mx;
    [SerializeField] public LayerMask groundLayers;
    [SerializeField] public Transform feet;

    public void Start()
    {
        facingRight = true;

        //healthbarcode
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        canFlip = true;
    }

    public void Update()
    {
        mx = Input.GetAxis("Horizontal");
        

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            Jump();
        }
        if (Mathf.Abs(rb.velocity.x) >= 0.01f)
        {
            anim.SetBool("isRunning", true);
        }
        else
        {
            anim.SetBool("isRunning", false);
        }

        //healthbarcode
        
    }

    private void OnCollisionEnter2D(Collision2D other){
        if(other.gameObject.tag =="cop"){
            TakeDamage(20);
        }
    }

    public void TakeDamage(int damage){
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
    }

    private void FixedUpdate()
    {
        Vector2 movement = new Vector2(mx * movementSpeed, rb.velocity.y);

        rb.velocity = movement;

        Flip();
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
    public void DisableFLip()
    {
        canFlip = false;


    }
    public void EnableFLip()
    {
        canFlip = true;


    }
    private void Flip()
    {
        if (mx > 0 && !facingRight || mx < 0 && facingRight && canFlip)
        {
            facingRight = !facingRight;

            Vector3 theScale = transform.localScale;

            theScale.x*=-1;

            transform.localScale = theScale;
        }

    }
}