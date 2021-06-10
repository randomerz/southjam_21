using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleMovement : IEMovement
{
    // r * sin(x(t + c))
    [Tooltip("Radius")]
    public float R = 1;
    [Tooltip("Period = something lmao")]
    public float X = 1;
    [Tooltip("Radial Shift in Radians")]
    public float C;

    private Vector3 offset;
    private Vector3 basePos;
    private float t;

    void Awake()
    {
        basePos = transform.position;
        t = Time.time;
    }
    
    public override void Move()
    {
        if (!canMove)
            return;

        t += Time.deltaTime;
        offset = new Vector3(R * Mathf.Cos(X * (t + C)), R * Mathf.Sin(X * (t + C)));

        transform.position = basePos + offset;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;

        Vector3 p0 = transform.position + new Vector3(R * Mathf.Cos(X * C), R * Mathf.Sin(X * C));

        Gizmos.DrawSphere(p0, 0.1f);
        Gizmos.DrawWireSphere(transform.position, R);
    }
}
