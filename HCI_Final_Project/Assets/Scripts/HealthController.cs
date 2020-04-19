using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;
public class HealthController : HealthControllerReferencer
{
    [BoxGroup("Settings")]public float maxHealth;
    [ReadOnly]public float currentHealth;

    [BoxGroup("Events")]public UnityEvent onDeathEvent;
    [BoxGroup("Events")]public VoidDelegate onDeathDelegate;

    [BoxGroup("Events")]public SingleUnityEvent onRecieveDamageEvent;
    [BoxGroup("Events")]public SingleDelegate onRecieveDamageDelegate;


    [BoxGroup("Events")]public SingleUnityEvent onHealthChangedEvent;
    [BoxGroup("Events")]public SingleDelegate onHealthChangedDelegate;

    void Awake()
    {
        healthController = this;
        initializeHealth();
    }

    void initializeHealth() {
        currentHealth = maxHealth;
    }

    public void recieveDamage(float amount) {
        currentHealth -= amount;

        onRecieveDamageEvent.Invoke(amount);
        if(onRecieveDamageDelegate != null) {
            onRecieveDamageDelegate(amount);
        }

        onHealthChangedEvent.Invoke(currentHealth);
        if(onHealthChangedDelegate != null) {
            onHealthChangedDelegate(currentHealth);
        }
        
        if(currentHealth <= 0) {
            kill();
        }
    }

    public void kill() {
        onDeathEvent.Invoke();
        if(onDeathDelegate != null) {
            onDeathDelegate();
        }
    }

    public float getCurrentHealth() {
        return currentHealth;
    }
}
