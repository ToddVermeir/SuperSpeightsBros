using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public int amountOfJumps = 1;

    private int facingDirection = 1;
    private int amountOfJumpsLeft;

    public Healthbar healthBar;
    public Rigidbody2D rb;
    public Animator anim;

    public bool facingRight;
    public bool isTouchingWall;
    public bool isDashing;
    [SerializeField] public bool canFlip;

    private bool isWallSliding;
    private bool isGrounded;
    private bool canJump;

    public float wallCheckDistance;
    public float movementSpeed;
    public float jumpForce = 10f;
    public float mx;
    public float wallSlideSpeed;
    public float movementForceInAir;
    public float airDragMultiplier = 0.95f;
    public float variableJumpHeightMultipler = 0.5f;
    public float wallHopForce;
    public float wallJumpForce;

    float dashDistance = 15f;
    float doubleTapTime;


    

    

    KeyCode lastKeyCode;


    public Vector2 wallHopDirection;
    public Vector2 wallJumpDirection;


    public Transform WallCheck;
    [SerializeField] public Transform feet;


    [SerializeField] public LayerMask groundLayers;
    

    public void Start()
    {
        facingRight = true;

        //healthbarcode
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        canFlip = true;
        wallHopDirection.Normalize();
        wallJumpDirection.Normalize();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            if (doubleTapTime > Time.time && lastKeyCode == KeyCode.A)
            {
                StartCoroutine(Dash(-1));
            }
            else
            {
                doubleTapTime = Time.time + 0.2f;
            }

            lastKeyCode = KeyCode.A;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            if (doubleTapTime > Time.time && lastKeyCode == KeyCode.D)
            {
                StartCoroutine(Dash(1));
            }
            else
            {
                doubleTapTime = Time.time + 0.2f;
            }
            lastKeyCode = KeyCode.D;
        }

        if (Mathf.Abs(rb.velocity.x) >= 0.01f)
        {
            anim.SetBool("isRunning", true);
        }
        else
        {
            anim.SetBool("isRunning", false);
        }

        CheckIfWallSliding();
        CheckIfCanJump();

        //healthbarcode
        
    }

    private void FixedUpdate()
    {
        CheckInput();
        CheckSurroundings();
        ApplyMovement();

        if (!isDashing)
        {
            Vector2 movement = new Vector2(mx * movementSpeed, rb.velocity.y);

            rb.velocity = movement;
        }



            if (facingRight && mx < 0)
        {
            Flip();
        }
        else if (!facingRight && mx > 0)
        {
            Flip();
        }
    }
    void Jump()
    {
        if (canJump && !isWallSliding)
        {
            Vector2 movement = new Vector2(rb.velocity.x, jumpForce);

            rb.velocity = movement;
            amountOfJumpsLeft--;
        }
        else if (isWallSliding && mx == 0 && canJump)
        {
            isWallSliding = false;
            Vector2 forceToAdd = new Vector2(wallHopForce * wallHopDirection.x * -facingDirection, wallHopForce * wallHopDirection.y);
            rb.AddForce(forceToAdd, ForceMode2D.Impulse);
            amountOfJumpsLeft--;
        }
        else if ((isWallSliding || isTouchingWall) && mx !=0 && canJump)
        {
            isWallSliding = false;
            Vector2 forceToAdd = new Vector2(wallJumpForce * wallJumpDirection.x * mx, wallJumpForce * wallJumpDirection.y);
            rb.AddForce(forceToAdd, ForceMode2D.Impulse);
            amountOfJumpsLeft--;
        }

    }
    private void CheckInput()
    {
        mx = Input.GetAxis("Horizontal");


        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }
        if (Input.GetButtonUp("Jump"))
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * variableJumpHeightMultipler);
        }
    }
    private void CheckIfWallSliding()
    {
        if (isTouchingWall && !isGrounded && rb.velocity.y < 0)
        {
            isWallSliding = true;
        }
        else
        {
            isWallSliding = false; 
        }
    }
    private void CheckSurroundings() {
        isTouchingWall = Physics2D.Raycast(WallCheck.position, transform.right, wallCheckDistance, groundLayers);
        isGrounded = Physics2D.OverlapCircle(feet.position, 0.5f, groundLayers);
    }

    private void CheckIfCanJump()
    {
        if ((isGrounded && rb.velocity.y <= 0) || isWallSliding && rb.velocity.y <=0)
        {
            amountOfJumpsLeft = amountOfJumps;
        }

        if (amountOfJumpsLeft <= 0)
        {
            canJump = false;
        }
        else
        {
            canJump = true;
        }
    }
    private void ApplyMovement()
    {
        if(isGrounded)
        {
            Vector2 movement = new Vector2(mx * movementSpeed, rb.velocity.y);
            rb.velocity = movement;

        }
        else if(!isGrounded && !isWallSliding && mx !=0)
        {
            Vector2 forceToAdd = new Vector2(movementForceInAir * mx, 0);
            rb.AddForce(forceToAdd);

            if(Mathf.Abs(rb.velocity.x) > movementSpeed)
            {
                rb.velocity = new Vector2(movementSpeed * mx, rb.velocity.y);
            }
        }
        else if(!isGrounded && !isWallSliding && mx == 0)
        {
            rb.velocity = new Vector2(rb.velocity.x * airDragMultiplier, rb.velocity.y);
        }
        

        if(isWallSliding)
        {
            if(rb.velocity.y < -wallSlideSpeed)
            {
                rb.velocity = new Vector2(rb.velocity.x, -wallSlideSpeed);
            }
        }
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
        if(!isWallSliding)
        {
            facingDirection *= -1;

            facingRight = !facingRight;

            transform.Rotate(0.0f, 180.0f, 0.0f);
        }

    }
    IEnumerator Dash(float direction)
    {
        isDashing = true;
        rb.velocity = new Vector2(rb.velocity.x, 0f);
        rb.AddForce(new Vector2(dashDistance * direction, 0f), ForceMode2D.Impulse);
        float gravity = rb.gravityScale;
        rb.gravityScale = 0;
        yield return new WaitForSeconds(0.4f);
        isDashing = false;
        rb.gravityScale = gravity;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(WallCheck.position, new Vector3(WallCheck.position.x + wallCheckDistance, WallCheck.position.y, WallCheck.position.z));
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "cop")
        {
            TakeDamage(20);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
    }

}