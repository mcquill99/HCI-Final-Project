using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HealthController : HealthControllerReferencer
{
    public float maxHealth;
    private float currentHealth;

    public UnityEvent onDeathEvent;
    public VoidEvent onDeathDelegate;

    public SingleUnityEvent onRecieveDamageEvent;
    public SingleEvent onRecieveDamageDelegate;

    void Start()
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
