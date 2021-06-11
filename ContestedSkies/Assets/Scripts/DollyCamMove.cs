using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DollyCamMove : MonoBehaviour
{
    public float camera_dolly_speed = 10f;
    void Update()
    {

        Vector3 nextPos = transform.position + -(transform.up) * camera_dolly_speed * Time.deltaTime;

        transform.position = nextPos;

    }

}
