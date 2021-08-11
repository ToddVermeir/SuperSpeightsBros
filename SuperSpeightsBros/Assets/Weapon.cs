using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform firePoint;
    public GameObject canPrefab;
    [SerializeField]
    private bool combatEnabled;
    [SerializeField]
    private float inputTimer;


    private bool gotInput, isAttacking, isFirstAttack;

    private float lastInputTime = Mathf.NegativeInfinity;

    private float[] attackDetails = new float[2];

    private Animator anim;
    private void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetBool("canAttack", combatEnabled);
    }
    // Update is called once per frame
    void Update()
    {
        CheckCombatInput();
    }
        
    private void CheckCombatInput() {
        if (Input.GetButtonDown("Fire1"))
        {
            if (combatEnabled)
            {
                Shoot();
                gotInput = true;
                lastInputTime = Time.time;
            }


        }
    }

    void Shoot()
    {
        if (gotInput)
        {
            
            if (!isAttacking)
            {
                gotInput = false;
                isAttacking = true;
                isFirstAttack = !isFirstAttack;
                anim.SetBool("attack1", true);
                anim.SetBool("firstAttack", isFirstAttack);
                anim.SetBool("isAttacking", isAttacking);
  
            }
        }
        if (Time.time >= lastInputTime + inputTimer)
        {
            gotInput = false;
        }
    
      


    }
    private void FinishAttack1()
    {
        isAttacking = false;
        anim.SetBool("isAttacking", isAttacking);
        anim.SetBool("attack1", false);

    }
    private void Release()
    {
        Instantiate(canPrefab, firePoint.position, firePoint.rotation);
    }

}
