using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class CreatureMovementController : MovementController
{
    NavMeshAgent controller;
    public float movementSpeed;

    void Start()
    {
        controller = GetComponent<NavMeshAgent>();   
    }

    protected override void LateUpdate() {
        controller.speed = movementSpeed * this.speedMultiplier;
        
        base.LateUpdate();
    }
}
