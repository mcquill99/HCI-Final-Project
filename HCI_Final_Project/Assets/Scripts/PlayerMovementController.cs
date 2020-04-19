using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MovementController
{
    private VHS.FirstPersonController controller;

    void Start()
    {
        controller = GetComponent<VHS.FirstPersonController>();
    }
    
    protected override void LateUpdate() {
        controller.speedMultiplier = this.speedMultiplier;
        
        base.LateUpdate();
    }
}
