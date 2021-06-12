using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public GameObject death_explosion;
    private IEnumerator damageCoroutine;
    SpriteRenderer sprite;
    public int health = 1;

    void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DamageEnemy(int damage)
    {
        health -= damage;
        DamageFlash();
        if (health <= 0)
        {
            Die();
        }
    }

    private void DamageFlash()
    {
        if (damageCoroutine != null) {StopCoroutine(damageCoroutine);}

        damageCoroutine = DoFlash();
        StartCoroutine(damageCoroutine);
    }

    private IEnumerator DoFlash()
    {
        float lerpTime = 0;
        while (lerpTime < 0.1f)
        {
            lerpTime += Time.deltaTime;
            // float perc = lerpTime / 0.3f;

            sprite.material.SetFloat("_FlashAmount", 1f);
            yield return null;
        }
        
        sprite.material.SetFloat("_FlashAmount",0);
    }
    
    //feelsbadman
    void Die() 
    {
        gameObject.SetActive(false);
        CameraShake.Shake(.5f, 8);
        Instantiate(death_explosion, new Vector2(gameObject.transform.position.x,gameObject.transform.position.y), Quaternion.identity);
    }
}
