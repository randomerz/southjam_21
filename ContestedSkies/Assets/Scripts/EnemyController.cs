using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private bool canAttack = true;
    public GameObject projectile;

    public IEMovement enemyMovement;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (canAttack)
        {
            Shoot();
        }
        if (enemyMovement != null)
        {
            enemyMovement.Move();
        }
    }

    void Shoot()
    {
        AudioManager.Play("Enemy Shoot");
        Instantiate(projectile,this.transform.position, this.transform.rotation);
        StartCoroutine(DelayShot());
    }

    private IEnumerator DelayShot()
    {
        canAttack = false;
        yield return new WaitForSeconds(1f);
        canAttack = true;
    }
}
