using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
public class ExplosiveBarrelController : MonoBehaviour
{
    [BoxGroup("Settings")]public GameObject explosionPrefab;
    [BoxGroup("Settings")]public float explosionRadius;
    [BoxGroup("Settings")]public float explosionDamage;

    private HealthControllerReferencer healthControllerReferencer;
    private HealthController healthController;
    private bool hasExploded = false;
    
    void Start()
    {
        healthControllerReferencer = GetComponent<HealthControllerReferencer>();
        if(healthControllerReferencer != null) {
            healthController = healthControllerReferencer.healthController;
            healthController.onDeathDelegate += onDeath;
        }
    }

    public void onDeath() {
        if(!hasExploded) {
            hasExploded = true;
            ExplosionController controller = ((GameObject)Instantiate(explosionPrefab, transform.position, transform.rotation)).GetComponent<ExplosionController>();

            controller.initExplosion(explosionRadius, explosionDamage);

        }
        Destroy(healthController.gameObject);
        
    }
}
