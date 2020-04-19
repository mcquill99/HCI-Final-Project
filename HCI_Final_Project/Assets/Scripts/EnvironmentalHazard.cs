using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public enum HazardType {DoT, Speed}
public class EnvironmentalHazard : MonoBehaviour
{
    [Tooltip("Type of status that hazard should apply")]
    [OnValueChanged("onHazardTypeChangedCallback")]
    [BoxGroup("Settings")]public HazardType type = HazardType.DoT;
    
    [Tooltip("Should hazard destroy itself after selfDestructTime seconds")]
    [BoxGroup("Settings")]public bool shouldSelfDestruct = false;
    
    [Tooltip("Amount of time in seconds after instantiation before hazard destroys itself")]
    [BoxGroup("Settings")][ShowIf("shouldSelfDestruct")]public float selfDestructTime;

    [Tooltip("Status to apply if Type equals DoT")]
    [EnableIf("isDoTStatus")] [BoxGroup("Settings")]public DamageOverTimeStatus damageOverTimeStatus;
    
    [Tooltip("Status to apply if Type equals Speed")]
    [EnableIf("isSpeedStatus")][BoxGroup("Settings")] public SpeedStatus speedStatus;
    private float tickTimestamp;

    [Tooltip("List of creatures currently effected by hazard")]
    [BoxGroup("Debug")][ReadOnly]public List<StatusManager> effectedCreatures;

    private bool isDoTStatus;
    private bool isSpeedStatus;

    private void onHazardTypeChangedCallback() {
        isDoTStatus = type == HazardType.DoT;
        isSpeedStatus = type == HazardType.Speed;
    }

    void Start()
    {
        effectedCreatures = new List<StatusManager>();
        if(shouldSelfDestruct) {
            Destroy(gameObject, selfDestructTime);
        }
    }

    void Update()
    {
        if(tickTimestamp < Time.time) {
            Status typedStatus = damageOverTimeStatus;
            if(type == HazardType.Speed)
                typedStatus = speedStatus;


            tickTimestamp = Time.time + typedStatus.duration;
            foreach(StatusManager h in effectedCreatures) {
                Status statusToApply = typedStatus.copyStatus();
                statusToApply.initStatus();
                h.addStatus(statusToApply);
            }
        }
    }

    public void OnTriggerEnter(Collider collider) {
        HealthControllerReferencer hcr = collider.gameObject.GetComponent<HealthControllerReferencer>();
        if(hcr != null) {
            StatusManager manager = hcr.healthController.GetComponent<StatusManager>();
            if(!effectedCreatures.Contains(manager))
                effectedCreatures.Add(manager);
        }
    }

    public void OnTriggerExit(Collider collider) {
        HealthControllerReferencer hcr = collider.gameObject.GetComponent<HealthControllerReferencer>();
        if(hcr != null) {
            StatusManager manager = hcr.healthController.GetComponent<StatusManager>();
            if(effectedCreatures.Contains(manager))
                effectedCreatures.Remove(manager);
        }
    }
}
