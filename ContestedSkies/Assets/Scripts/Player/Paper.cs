using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paper : MonoBehaviour
{
    public AnimationCurve paperDistCurve;
    public Collider2D myCollider;
    private float timeAlive;
    private Vector3 origPos;
    private Vector3 targetPos;

    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        timeAlive += Time.deltaTime;

        if (timeAlive < 0.5f)
        {
            myCollider.enabled = false;
            float dp = paperDistCurve.Evaluate(timeAlive);
            transform.position = (dp * targetPos) + ((1 - dp) * origPos);
        }
        else
        {
            myCollider.enabled = true;
            transform.position = targetPos;
        }
    }

    public void SetTarget(Vector3 target)
    {
        timeAlive = 0;
        origPos = transform.position;
        targetPos = target;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Destroy(gameObject);
        }
    }
}
