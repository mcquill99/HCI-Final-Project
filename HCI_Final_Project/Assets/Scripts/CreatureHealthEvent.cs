using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;

public enum Comparison {Equals, GreaterThan, LessThan, GreaterEquals, LesserEquals}

public class CreatureHealthEvent : CreatureEvent
{

    [BoxGroup("Settings")] public bool singleExecution = true;
    [BoxGroup("Settings")] public bool usePercentage = true;
    [BoxGroup("Settings")] public Comparison comparison = Comparison.LessThan;
    [BoxGroup("Settings")][HideIf("usePercentage")] public float comparedValue;
    [BoxGroup("Settings")][ShowIf("usePercentage")][Slider(0f, 1f)] public float comparedPercentage = 0.5f;
    [BoxGroup("Events")] public UnityEvent onCreatureHealthEvent;
    [BoxGroup("Events")] public VoidDelegate onCreatureHealthDelegate;
    [BoxGroup("DEBUG")][ReadOnly] public bool hasExecuted = false;
    private float maxHealth;

    public override void initEvent(HealthController hcr) {
        hcr.onHealthChangedDelegate += onHealthChanged;
        maxHealth = hcr.maxHealth;
    }
    
    public void onHealthChanged(float currentHealth) {
        if(singleExecution && hasExecuted) {
            return;
        }

        float eventValue = usePercentage ? comparedPercentage : comparedValue;
        float creatureValue = usePercentage ? currentHealth / maxHealth : currentHealth;

        bool isTrue = comparison == Comparison.Equals && creatureValue == eventValue ||
                    comparison == Comparison.GreaterThan && creatureValue > eventValue ||
                    comparison == Comparison.LessThan && creatureValue < eventValue ||
                    comparison == Comparison.GreaterEquals && creatureValue >= eventValue ||
                    comparison == Comparison.LesserEquals && creatureValue <= eventValue;

        if(isTrue) {
            hasExecuted = true;

            onCreatureHealthEvent.Invoke();
            if(onCreatureHealthDelegate != null) {
                onCreatureHealthDelegate();
            }
        }
        
    }
}
