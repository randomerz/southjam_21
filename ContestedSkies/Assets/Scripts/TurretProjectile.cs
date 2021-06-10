using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretProjectile : MonoBehaviour
{
    public float speed = 10f;
    Rigidbody2D rb;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Start()
    {
        rb.velocity = transform.right * speed;
    }
    
    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
