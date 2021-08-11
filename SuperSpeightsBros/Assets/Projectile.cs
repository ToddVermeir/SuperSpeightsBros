using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 20f;
    public Rigidbody2D rb;
    public int damage = 10;
    
    // Start is called before the first frame updat


    private void Start()
    {
        
        rb.velocity = transform.right * speed;

    
    }


    private void OnTriggerEnter2D(Collider2D hitInfo)
    {
        Enemy enemy = hitInfo.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }
        if (hitInfo.name == "Enemy" || hitInfo.name == "Ground") {
            Destroy(gameObject);
        }


    
    }

}
