using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderColorChanger : MonoBehaviour
{
    public MeshRenderer[] parts;
    public Material aggressiveColor;
    public Material patrollingColor; 
    public AIStateMachine stateMachine;
    void Awake()
    {
        stateMachine.combatStateChangedDelegate += OnStateChange; 
    }

    void OnStateChange(AICombatState val)
    {
        Material colorToChangeTo = val == AICombatState.Aggressive ? aggressiveColor : patrollingColor;
        foreach(MeshRenderer m in parts) {
            m.material = colorToChangeTo;
        }
    }
}
