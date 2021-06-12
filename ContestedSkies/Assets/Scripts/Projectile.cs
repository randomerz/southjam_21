using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;
    public Rigidbody2D rb;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Start()
    {
        rb.velocity = transform.up * speed;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == 9)
        {
            Destroy(gameObject);
        }

        EnemyController enemy = col.gameObject.GetComponent<EnemyController>();
        if (enemy)
        {
            enemy.DamageEnemy(1);
            Destroy(gameObject);
        }
        
    }
    
    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
