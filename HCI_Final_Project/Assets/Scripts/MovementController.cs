using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    protected float speedMultiplier = 1f;
    protected Vector3 velocity;

    public void multiplySpeed(float val) {
        speedMultiplier *= val;
    }

    protected virtual void LateUpdate()
    {
        speedMultiplier = 1f;
    }

    public Vector3 getVelocity() {
        return velocity;
    }
}
