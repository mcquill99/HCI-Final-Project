using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;

public class NumCreaturesDeadEvent : MonoBehaviour
{
    [ReorderableList]public List<SpawnerController> spawners;

    public int requiredAmountDead;
    public bool singleExecution = true;
    public UnityEvent onNumCreaturesDeadEvent;
    public VoidDelegate onNumCreaturesDeadDelegate;
    
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
