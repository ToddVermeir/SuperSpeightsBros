using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ENEMY1 : MonoBehaviour
{
    public enum State
    {
        walking,
        idle,
        Dead
    
    }

    private State currentState;

    [SerializeField]
    private float groundCheckDistance, wallCheckDistance,movementSpeed,maxHealth,idleDuration;
    [SerializeField]
    private Transform groundCheck, wallCheck;
    [SerializeField]
    public LayerMask whatIsGround;
    [SerializeField] 
    private Vector2 idleSpeed;

    private float currentHealth, idleStartTime;

    private Vector2 movement;

    private int facingDirection,DamageDirection;

    private bool groundDetected, wallDetected;

    private GameObject alive;
    private Rigidbody2D aliveRb;

    private Animator aliveAnim;

    private void Start(){
        alive = transform.Find("ALIVE").gameObject;
        aliveRb = alive.GetComponent<Rigidbody2D>();

        aliveAnim =  alive.GetComponent<Animator>();


        facingDirection =1;

    }

    private void update(){
        switch(currentState){
            case State.walking:
                UpdateWalkingState();
                break;
            case State.idle:
                UpdateIdleState();
                break;
            case State.Dead:
                UpdateDeadState();
                break;
        }
    }

    // walking state

    private void EnterWalkingState(){

    }

    //enter walking state

    private void UpdateWalkingState(){
        groundDetected = Physics2D.Raycast(groundCheck.position, Vector2.down,groundCheckDistance,whatIsGround);
        wallDetected = Physics2D.Raycast(wallCheck.position, transform.right,wallCheckDistance,whatIsGround);

        if (groundDetected || wallDetected){
            flip();
        }
        else {
            movement.Set(movementSpeed * facingDirection, aliveRb.velocity.y);
            aliveRb.velocity = movement;
        }
        

    }

    private void ExitWalkingState(){

    }

    //IDLESTATE

    private void EnterIdleState(){

        idleStartTime = Time.time;
        movement.Set(idleSpeed.x * DamageDirection,idleSpeed.y);
        aliveRb.velocity = movement;
        aliveAnim.SetBool("idle",true);

    }

    private void UpdateIdleState(){

        if(Time.time >= idleStartTime + idleDuration){
            SwitchState(State.walking);
        }


    }

    private void ExitIdleState(){

        aliveAnim.SetBool("idle",false);

    }



    //DEADSTATE
    private void EnterDeadState(){

        //spawn particles
        Destroy(gameObject);

    }

    private void UpdateDeadState(){

    }

    private void ExitDeadState(){

    }


    //other functions

    private void Damage(float[] attackDetails)//Sends multiples parameters through 
    {
        currentHealth -=  attackDetails[0];

        if(attackDetails[1]>alive.transform.position.x){

            DamageDirection = -1;
        }
        else
        {
            DamageDirection = 1;

        }

        //hitparticles

        if(currentHealth > 0.0f){
            SwitchState(State.idle);
        }
        else if (currentHealth <= 0.0f){
            SwitchState(State.Dead);

        }


    }

    private void flip(){
        facingDirection *= -1;
        alive.transform.Rotate(0.0f,180.0f,0.0f);

    }

    private void SwitchState(State state)
    {
        switch(currentState){
            case State.walking:
                ExitWalkingState();
                break;
            case State.idle:
                ExitIdleState();
                break;
            case State.Dead:
                ExitDeadState();
                break;
            

        }

        switch(state){
            case State.walking:
                EnterWalkingState();
                break;
            case State.idle:
                EnterIdleState();
                break;
            case State.Dead:
                EnterDeadState();
                break;
            
        }

        currentState = state;


    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector2(groundCheck.position.x, groundCheck.position.y-groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position, new Vector2(wallCheck.position.x + wallCheckDistance, wallCheck.position.y));
    }


    
}
