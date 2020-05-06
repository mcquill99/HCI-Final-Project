using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;
public class HealthController : HealthControllerReferencer
{
    [Tooltip("Maximum health")]
    [BoxGroup("Settings")]public float maxHealth;
    
    [Tooltip("Current health")]
    [ReadOnly]public float currentHealth;


    [Tooltip("UnityEvent called when health <= 0. Delegate onDeathDelegate also executed")]
    [BoxGroup("Events")]public UnityEvent onDeathEvent;
    [BoxGroup("Events")]public VoidDelegate onDeathDelegate;

    [Tooltip("UnityEvent called when controller recieves damage. Amount of damage passed through as parameter. Delegate onRecieveDamageDelegate also executed")]
    [BoxGroup("Events")]public SingleUnityEvent onRecieveDamageEvent;
    [BoxGroup("Events")]public DamageDelegate onRecieveDamageDelegate;

    [Tooltip("UnityEvent called when health is changed. Changed health value is passed through as parameter. Delegate onHealthChanged also executed")]
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

    public void recieveDamage(Vector3 position, float amount) {
        currentHealth -= amount;

        onRecieveDamageEvent.Invoke(amount);
        if(onRecieveDamageDelegate != null) {
            onRecieveDamageDelegate(position, amount);
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
