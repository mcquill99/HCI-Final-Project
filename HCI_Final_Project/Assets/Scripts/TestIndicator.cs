using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestIndicator : MonoBehaviour
{
    [SerializeField]
    float destroyTimer = 20.0f;

    void Start()
    {
        Invoke("Register", Random.Range(0,8));
    }
    void Register()
    {
        if(!DamageIndicatorSystem.CheckIfObjectInSight(transform.position))
        {
            DamageIndicatorSystem.CreateIndicator(transform.position, 50);
        }
        Destroy(this.gameObject, destroyTimer);
    }
}
