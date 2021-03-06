﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
public class ExplosionController : MonoBehaviour
{
    [Tooltip("Rate at which damage decreases based on distance from explosion center and explosion radius")]
    [BoxGroup("Settings")]public AnimationCurve damageFalloffCurve;

    private float radius;
    private float damage;

    public void initExplosion(float radius, float damage) {

        this.radius = radius;
        this.damage = damage;
        StartCoroutine(explode());
    }

    IEnumerator explode() {
        yield return new WaitForSeconds(0.1f);
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius, 1<<10 | 1<<12| 1<<13);
        foreach(Collider col in colliders) {
            
            HealthControllerReferencer hcr = col.GetComponent<HealthControllerReferencer>();
            float val = Vector3.Magnitude(col.ClosestPoint(transform.position) - transform.position) / radius;
            float calculatedDamage = damage * damageFalloffCurve.Evaluate(val);
            //print(calculatedDamage);
            if(hcr != null) {
                hcr.healthController.recieveDamage(transform.position, calculatedDamage);
            }
        }
        Destroy(gameObject, 2f);
    }
}
