using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class SlowDownGooBarrelController : MonoBehaviour
{
    [Tooltip("Prefab to instantiate when barrel dies")]
    [BoxGroup("Settings")]public GameObject gooPrefab;
    private bool hasExploded = false;
    private HealthController healthController;

    void Start()
    {
        HealthControllerReferencer healthControllerReferencer = GetComponent<HealthControllerReferencer>();
        if(healthControllerReferencer != null) {
            healthController = healthControllerReferencer.healthController;
            healthController.onDeathDelegate += onDeath;
        }
    }

    public void onDeath() {
        if(!hasExploded) {
            hasExploded = true;
            RaycastHit hit;
            if(Physics.Raycast(transform.position, Vector3.down, out hit, 100f, 1 << 0 | 1 << 9)) {
                GameObject controller = ((GameObject)Instantiate(gooPrefab, hit.point, Quaternion.identity));
            }
        }
        Destroy(healthController.gameObject);
        
    }
}
