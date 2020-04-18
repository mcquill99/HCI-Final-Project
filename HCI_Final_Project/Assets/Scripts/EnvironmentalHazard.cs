using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentalHazard : MonoBehaviour
{
    public float damagePerTick;
    public float tickPerSecond;
    private float tickTimestamp;
    private List<HealthController> effectedCreatures;


    void Start()
    {
        effectedCreatures = new List<HealthController>();
    }

    void Update()
    {
        if(tickTimestamp < Time.time) {
            tickTimestamp = Time.time + (1f / tickPerSecond);
            foreach(HealthController h in effectedCreatures) {
                h.recieveDamage(damagePerTick);
            }
        }
    }

    public void OnTriggerEnter(Collider collider) {
        HealthControllerReferencer hcr = collider.gameObject.GetComponent<HealthControllerReferencer>();
        if(hcr != null) {
            effectedCreatures.Add(hcr.healthController);
        }
    }

    public void OnTriggerExit(Collider collider) {
        HealthControllerReferencer hcr = collider.gameObject.GetComponent<HealthControllerReferencer>();
        if(hcr != null) {
            effectedCreatures.Remove(hcr.healthController);
        }
    }
}
