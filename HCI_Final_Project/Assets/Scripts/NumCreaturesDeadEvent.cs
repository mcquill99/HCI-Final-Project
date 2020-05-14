using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;

public class NumCreaturesDeadEvent : MonoBehaviour
{
    [Tooltip("List of spawners to listen to. Creatures spawned from these spawners are counted towards requiredAmountDead.")]
    [BoxGroup("Settings")][ReorderableList]public List<SpawnerController> spawners;

    [Tooltip("Required number of creatures to die from spawners in order to invoke event")]
    [BoxGroup("Settings")]public int requiredAmountDead;

    [Tooltip("Only invoke event once")]
    [BoxGroup("Settings")]public bool singleExecution = true;

    [Tooltip("UnityEvent called when required number of deaths is reached. Delegate onNumCreaturesDeadDelegate also executed")]
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
        if(!this.enabled){
            return;
        }
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
