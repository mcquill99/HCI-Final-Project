using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
public class PhaseManager : MonoBehaviour
{
    [BoxGroup("Settings")]public bool startPhaseOnStart = true;
    [BoxGroup("Settings")][ReorderableList]public List<Phase> phases;
    [BoxGroup("DEBUG")][ReadOnly]public int currentPhaseIndex;

    void Start() {
        if(startPhaseOnStart) {
            setPhase(0);
        }
    }

    public void nextPhase() {
        setPhase(currentPhaseIndex + 1);
    }

    public void setPhase(int phaseNum) {
        if(phaseNum >= phases.Count) {
            Debug.LogWarning("Attempting to enter non-existant phase: " + phaseNum + " Max allowable: " + phases.Count);
            return;
        }
        currentPhaseIndex = phaseNum;
        Phase currentPhase = phases[currentPhaseIndex];
        foreach(SpawnerController s in currentPhase.spawners) {
            s.activateSpawner();
        }

    }

}

[System.Serializable]
public class Phase {    
    public List<SpawnerController> spawners;
}
