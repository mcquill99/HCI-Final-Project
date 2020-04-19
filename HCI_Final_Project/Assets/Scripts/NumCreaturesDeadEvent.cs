using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;

public class NumCreaturesDeadEvent : MonoBehaviour
{
    [BoxGroup("Settings")][ReorderableList]public List<SpawnerController> spawners;

    [BoxGroup("Settings")]public int requiredAmountDead;
    [BoxGroup("Settings")]public bool singleExecution = true;
    [BoxGroup("Events")]public UnityEvent onNumCreaturesDeadEvent;
    [BoxGroup("Events")]public VoidDelegate onNumCreaturesDeadDelegate;
    
    private int numCreaturesDead = 0;
    private bool hasExecuted = false;

    void Start() {
        foreach(SpawnerController sc in spawners) {
            sc.onSpawnCreatureDelegate += onSpawnCreature;
        }
    }

    public void onSpawnCreature(HealthController healthController) {
        healthController.onDeathDelegate += onCreatureDeath;
    }

    private void onCreatureDeath() {
        numCreaturesDead++;
        if(singleExecution && hasExecuted) {
            return;
        }

        if(numCreaturesDead >= requiredAmountDead) {
            onNumCreaturesDeadEvent.Invoke();
            if(onNumCreaturesDeadDelegate != null) {
                onNumCreaturesDeadDelegate();
            }
        }
    }
}
