using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinEMovement : IEMovement
{
    [Tooltip("True to move on X axis, false to move on Y axis")]
    public bool onXAxis = true;

    [Tooltip("Amplitude")]
    public float A = 1;
    [Tooltip("Period = 2pi/B")]
    public float B = 1;
    [Tooltip("Phase Shift")]
    public float C;
    [Tooltip("Vertical Shift")]
    public float D;

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
        float y = A * Mathf.Sin(B * (t - C)) + D;
        if (onXAxis)
        {
            offset = new Vector3(y, 0);
        }
        else
        {
            offset = new Vector3(0, y);
        }

        transform.position = basePos + offset;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        float y = A * Mathf.Sin(B * -C);
        Vector3 p0 = transform.position;
        Vector3 p1 = transform.position;
        Vector3 p2 = transform.position;
        if (onXAxis)
        {
            p0 += new Vector3(y, D);
            p1 += new Vector3(A, D);
            p2 += new Vector3(-A, D);
        }
        else
        {
            p0 += new Vector3(D, y);
            p1 += new Vector3(D, A);
            p2 += new Vector3(D, -A);
        }

        Gizmos.DrawSphere(p0, 0.1f);
        Gizmos.DrawLine(p1, p2);
    }
}
