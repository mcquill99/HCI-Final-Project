using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;

public class SpawnerController : MonoBehaviour
{
    [Tooltip("Prefab to instantiate when spawner spawns")]
    [BoxGroup("Settings")]public GameObject creatureToSpawn;

    [Tooltip("Is spawner active on instantiation")]
    [BoxGroup("Settings")]public bool activeOnStart;

    [Tooltip("Number of creatures to spawn before deactivation")]
    [BoxGroup("Settings")]public int numToSpawn;

    [Tooltip("Time in seconds to wait before spawning first creature")]
    [BoxGroup("Settings")]public float timeBeforeFirstSpawn = 0f;

    [Tooltip("Time in seconds to wait between creature spawns")]
    [BoxGroup("Settings")]public float betweenSpawnDuration;


    [Tooltip("UnityEvent called when creature spawns. Passes HealthController as parameter. Delegate onSpawnCreatureDelegate also executed")]
    [BoxGroup("Events")]public HealthControllerEvent onSpawnCreatureEvent;
    [BoxGroup("Events")]public HealthControllerDelegate onSpawnCreatureDelegate;

    private bool isActive;
    private int numSpawned;
    private float betweenSpawnTimestamp;
    private CreatureEvent[] events;

    void Start()
    {
        events = gameObject.GetComponents<CreatureEvent>();

        if(activeOnStart) {
            activateSpawner();
        }
    }

    void Update()
    {
        if(isActive && betweenSpawnTimestamp < Time.time) {
            if(numSpawned >= numToSpawn) {
                deactivateSpawner();
                return;
            }
            spawnCreature();
        }
    }

    public void activateSpawner() {
        betweenSpawnTimestamp = Time.time + timeBeforeFirstSpawn;
        isActive = true;
        numSpawned = 0;
    }

    public void deactivateSpawner() {
        isActive = false;
    }

    private void spawnCreature() {
        betweenSpawnTimestamp = Time.time + betweenSpawnDuration;

        HealthControllerReferencer healthControllerReferencer = ((GameObject)Instantiate(creatureToSpawn, transform.position, transform.rotation)).GetComponentInChildren<HealthControllerReferencer>();
        foreach(CreatureEvent e in events) {
            e.initEvent(healthControllerReferencer.healthController);
        }

        onSpawnCreatureEvent.Invoke(healthControllerReferencer.healthController);
        if(onSpawnCreatureDelegate != null) {
            onSpawnCreatureDelegate(healthControllerReferencer.healthController);
        }

        numSpawned++;
    }
}
