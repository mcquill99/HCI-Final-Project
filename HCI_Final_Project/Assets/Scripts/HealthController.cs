using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;
public class HealthController : HealthControllerReferencer
{
    public float maxHealth;
    [ReadOnly]public float currentHealth;

    public UnityEvent onDeathEvent;
    public VoidDelegate onDeathDelegate;

    public SingleUnityEvent onRecieveDamageEvent;
    public SingleDelegate onRecieveDamageDelegate;


    public SingleUnityEvent onHealthChangedEvent;
    public SingleDelegate onHealthChangedDelegate;

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
