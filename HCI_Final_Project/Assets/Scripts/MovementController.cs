using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    protected float speedMultiplier = 1f;

    public void multiplySpeed(float val) {
        speedMultiplier *= val;
    }

    protected virtual void LateUpdate()
    {
        speedMultiplier = 1f;
    }
}
