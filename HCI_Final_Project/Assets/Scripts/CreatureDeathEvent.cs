using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;
public class CreatureDeathEvent : CreatureEvent
{
    [BoxGroup("Events")]public UnityEvent creatureDeathEvent;
    [BoxGroup("Events")]public VoidDelegate creatureDeathDelegate;

    public override void initEvent(HealthController hcr) {
        hcr.onDeathDelegate += onCreatureDeathEvent;
    }

    public void onCreatureDeathEvent() {
        creatureDeathEvent.Invoke();
        if(creatureDeathDelegate != null) {
            creatureDeathDelegate();
        }
    }
}
