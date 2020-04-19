﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[System.Serializable]
public class DamageOverTimeStatus : Status
{
    public float damagePerTick;
    public float tickPerSecond;
    private float tickTimestamp;


    public override void EvaluateStatus(StatusManager manager) {
        if(tickTimestamp < Time.time) {
            tickTimestamp = Time.time + (1f / tickPerSecond);
            manager.healthController.recieveDamage(damagePerTick);
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