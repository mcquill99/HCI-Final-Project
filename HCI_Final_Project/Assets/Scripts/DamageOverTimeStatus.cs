using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[System.Serializable]
public class DamageOverTimeStatus : Status
{
    [Tooltip("Amount of damage done per status tick")]
    [BoxGroup("Settings")]public float damagePerTick;

    [Tooltip("Amount of status ticks per second")]
    [BoxGroup("Settings")]public float tickPerSecond;
    private float tickTimestamp;


    public override void EvaluateStatus(StatusManager manager) {
        if(tickTimestamp < Time.time) {
            tickTimestamp = Time.time + (1f / tickPerSecond);
            manager.healthController.recieveDamage(manager.transform.position, damagePerTick);
        }
    }

    public override Status copyStatus() {
        DamageOverTimeStatus copiedStatus = new DamageOverTimeStatus();
        copiedStatus.duration = duration;
        copiedStatus.damagePerTick = damagePerTick;
        copiedStatus.tickPerSecond = tickPerSecond;
        return copiedStatus;
    }
}
