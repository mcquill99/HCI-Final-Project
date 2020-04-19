using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using NaughtyAttributes;
public class CreatureMovementController : MovementController
{
    NavMeshAgent controller;

    [Tooltip("Default movement speed of creature NavmeshAgent")]
    [BoxGroup("Settings")] public float movementSpeed;

    void Start()
    {
        controller = GetComponent<NavMeshAgent>();   
    }

    protected override void LateUpdate() {
        this.velocity = controller.velocity;
        controller.speed = movementSpeed * this.speedMultiplier;
        
        base.LateUpdate();
    }
}
