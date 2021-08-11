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
    [SerializeField] public bool canFlip;

    private bool isWallSliding;
    private bool isGrounded;
    private bool canJump;
    private bool isDashing;
    private bool canMove; 

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
    public float dashTime;
    public float dashSpeed;
    public float dashCoolDown;
    public float groundCheckRadius;

     
    public Vector2 wallHopDirection;
    public Vector2 wallJumpDirection;

    private float lastDash = -100f;
    private float dashTimeLeft;
    


    public Transform WallCheck;
    [SerializeField] public Transform groundCheck;


    [SerializeField] public LayerMask whatIsGround;
    

    public void Start()
    {
        facingRight = true;

        //healthbarcode
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        canFlip = true;
        wallHopDirection.Normalize();
        wallJumpDirection.Normalize();
        canMove = true;

    }

    public void Update()
    {
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
        CheckDash();
        UpdateAnimations();


    }
    private void UpdateAnimations()
    {
        anim.SetBool("isWallSliding", isWallSliding);

    }
    private void FixedUpdate()
    {
        CheckInput();
        CheckSurroundings();
        ApplyMovement();



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
        else if ((isWallSliding || isTouchingWall) && mx !=0 && canJump && mx!= facingDirection)
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

        if (Input.GetButtonDown("Dash"))
        {
            if (Time.time >= (lastDash + dashCoolDown))
                AttemptToDash();
        }
    }

    private void AttemptToDash()
    {
        isDashing = true;
        dashTimeLeft = dashTime;
        lastDash = Time.time;


    }
    private void CheckDash()
    {
        if (isDashing)
            {
                if(dashTimeLeft > 0)
                {
                    canMove = false;
                    canFlip = false;
                    rb.velocity = new Vector2(dashSpeed * facingDirection, 0.0f);
                    dashTimeLeft -= Time.deltaTime;

                
                }

                if (dashTimeLeft <= 0 || isTouchingWall)
                    {
                        isDashing = false;
                        canMove = true;
                        canFlip = true;
                    }
            
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
        isTouchingWall = Physics2D.Raycast(WallCheck.position, transform.right, wallCheckDistance, whatIsGround);
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius,whatIsGround);
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
        if(!isGrounded && !isWallSliding && mx ==0)
        {
            rb.velocity = new Vector2(rb.velocity.x * airDragMultiplier, rb.velocity.y);
        }
        else if(canMove)
        {
            rb.velocity = new Vector2(movementSpeed * mx, rb.velocity.y);
        }

        if(isWallSliding)
        {
            
            if (rb.velocity.y < -wallSlideSpeed)
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

            facingRight = !facingRight;

            transform.Rotate(0.0f, 180.0f, 0.0f);

            facingDirection *= -1;

        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius); 
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