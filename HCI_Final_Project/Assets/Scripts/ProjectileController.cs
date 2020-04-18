using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;
public class ProjectileController : MonoBehaviour
{
    [BoxGroup("Settings")] public float initialSpeed;
    [BoxGroup("Settings")] public float accelerationAmount;
    [BoxGroup("Settings")] public bool destroyOnImpact = false;
    [BoxGroup("Events")] public UnityEvent onImpactEvent;
    public VoidDelegate onImpactDelegate;
    private Rigidbody rigidbody;
    private float damage;
    
    public void InitializeProjectile(float damage, Vector3 inheritVelocity) {
        this.damage = damage;
        rigidbody = GetComponent<Rigidbody>();  

        rigidbody.velocity = Vector3.Project(inheritVelocity, transform.forward);
        rigidbody.velocity += transform.forward * initialSpeed;   
    }
    
    void Update() {
        rigidbody.AddForce(transform.forward * accelerationAmount);
    }

    void OnCollisionEnter(Collision collision) {
        onImpactEvent.Invoke();
        if(onImpactDelegate != null) {
            onImpactDelegate();
        }

        HealthControllerReferencer r = collision.gameObject.GetComponent<HealthControllerReferencer>();
        if(r != null) {
            HealthController healthController = r.healthController;
            healthController.recieveDamage(damage);
        }

        
        if(destroyOnImpact) {
            Destroy(gameObject);
        }
    }

}
