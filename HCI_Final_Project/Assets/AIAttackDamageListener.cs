using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAttackDamageListener : MonoBehaviour
{
    public float damageAmount = 5;
    private bool isAttacking;
    private HashSet<HealthController> attackHits;

    void OnCollisionEnter(Collision collision) {
        HealthController hc = collision.collider.GetComponent<HealthControllerReferencer>()?.healthController;
        if(isAttacking && !attackHits.Contains(hc)) {
            attackHits.Add(hc);
            hc.recieveDamage(transform.position, damageAmount);
        }
    }

    public void startAttack() {
        isAttacking = true;
        attackHits = new HashSet<HealthController>();
    }

    public void endAttack() {
        isAttacking = false;
    }
}
