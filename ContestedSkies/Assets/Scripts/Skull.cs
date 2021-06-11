using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skull : MonoBehaviour
{
    public float vision_rad = 7f;
    private bool canAttack;
    public GameObject projectile;

    public IEMovement enemyMovement;
    // Start is called before the first frame update
    void Start()
    {
        canAttack = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (canAttack)
        {
            Collider2D[] objs = Physics2D.OverlapCircleAll(transform.position, vision_rad);
            foreach (Collider2D item in objs)
            {
                Player player = item.GetComponent<Player>();
                if (player) {Shoot();}
            }
        }
        if (enemyMovement != null)
        {
            enemyMovement.Move();
        }
    }

    void Shoot()
    {
        AudioManager.Play("Enemy Shoot");
        for (int i = 0; i < 4; i++)
        {
            Instantiate(projectile, this.transform.position, Quaternion.Euler(0f,0f,i * 360/4 + 45));
        }
        StartCoroutine(DelayShot());
    }

    private IEnumerator DelayShot()
    {
        canAttack = false;
        yield return new WaitForSeconds(2f);
        canAttack = true;
    }

}
