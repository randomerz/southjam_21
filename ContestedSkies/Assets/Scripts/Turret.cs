using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
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
        Instantiate(projectile,this.transform.position, this.transform.rotation);
        StartCoroutine(DelayShot());
    }

    private IEnumerator DelayShot()
    {
        canAttack = false;
        yield return new WaitForSeconds(1f);
        canAttack = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;

        Vector3 p0 = transform.position + new Vector3(vision_rad * Mathf.Cos(1), vision_rad * Mathf.Sin(1));

        Gizmos.DrawSphere(p0, 0.1f);
        Gizmos.DrawWireSphere(transform.position, vision_rad);
    }

}
