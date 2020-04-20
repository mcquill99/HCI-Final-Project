using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[System.Serializable]
public class SpeedStatus : Status
{
    [Tooltip("Speed Multiplier to apply to MovementController")]
    [BoxGroup("Settings")]public float speedMultiplier;

    public override void EvaluateStatus(StatusManager manager) {
        manager.movementController.multiplySpeed(speedMultiplier);
    }

    
    public override Status copyStatus() {
        SpeedStatus copiedStatus = new SpeedStatus();
        copiedStatus.duration = duration;
        copiedStatus.speedMultiplier = speedMultiplier;
        return copiedStatus;
    }
}
