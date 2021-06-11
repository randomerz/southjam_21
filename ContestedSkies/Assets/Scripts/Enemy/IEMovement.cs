using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IEMovement : MonoBehaviour
{
    public bool canMove = true;

    public abstract void Move();
}
