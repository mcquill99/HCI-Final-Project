using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropsController : MonoBehaviour
{
    public LayerMask layerMask;
    public bool addAmmo;
    public bool addHealth;
    public float amount;

    void OnTriggerEnter(Collider collider) {
        if((layerMask & (1 << collider.gameObject.layer)) != 0) {
            if(addAmmo) {
                WeaponFireController[] weapons = collider.GetComponentsInChildren<WeaponFireController>();
                foreach(WeaponFireController f in weapons) {
                    f.addAmmo(Mathf.RoundToInt(amount));
                }
                Destroy(gameObject);

            }

            if(addHealth) {
                HealthController hc = collider.GetComponent<HealthControllerReferencer>()?.healthController;
                if(hc.currentHealth >= hc.maxHealth)
                    return;
                    
                hc?.recieveDamage(transform.position, -1f * amount);
                Destroy(gameObject);
            }
        }
    }
}
