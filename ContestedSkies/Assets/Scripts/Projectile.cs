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
    
    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
