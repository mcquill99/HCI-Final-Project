using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;
public class ProjectileController : MonoBehaviour
{
    [Tooltip("Initial speed of projectile in direction of Transform.forward")]
    [BoxGroup("Settings")] public float initialSpeed;

    [Tooltip("Amount of force to apply in direction of Transform.forward every frame")]
    [BoxGroup("Settings")] public float accelerationAmount;

    [Tooltip("Should projectile destroy on impact")]
    [BoxGroup("Settings")] public bool destroyOnImpact = false;

    [Tooltip("Radius of projectile explosion. Used for damage falloff")]
    [BoxGroup("Settings")] public float explosionRadius = 2f;

    [Tooltip("Limit projectile to self destruct after a duration of seconds")]
    [BoxGroup("Settings")] public float timeBeforeSelfDestruct = 8f;

    [Tooltip("Layers to count towards exploding")]
    [BoxGroup("Settings")] public LayerMask layers;

    [Tooltip("Prefab to instantiate on explode")]
    [BoxGroup("Settings")] public GameObject explosionPrefab;

    [Tooltip("UnityEvent called when rocket explodes. Delegate onImpactEvent also executed")]
    [BoxGroup("Events")] public UnityEvent onImpactEvent;
    public VoidDelegate onImpactDelegate;
    private Rigidbody rigidbody;
    private Collider collider;
    private float damage;
    private float selfDestructTimeStamp;
    
    public void InitializeProjectile(float damage, Vector3 inheritVelocity, LayerMask layers, GameObject creatureRoot) {
        this.damage = damage;
        this.layers = layers;
        rigidbody = GetComponent<Rigidbody>(); 
        collider = GetComponent<Collider>();

        Collider[] cols = creatureRoot.GetComponentsInChildren<Collider>();
        foreach(Collider c in cols) {
            Physics.IgnoreCollision(collider, c, true);
        }
        

        rigidbody.velocity = Vector3.Project(inheritVelocity, transform.forward);
        rigidbody.velocity += transform.forward * initialSpeed;   

        selfDestructTimeStamp = Time.time + timeBeforeSelfDestruct;
    }
    
    void Update() {
        rigidbody.AddForce(transform.forward * accelerationAmount);

        if(selfDestructTimeStamp < Time.time) {
            explode();
        }
    }

    void OnCollisionEnter(Collision collision) {
        if((layers & (1 << collision.gameObject.layer)) == 0) {
            return;
        } else {
            explode();
        }
    }

    public void explode() {
        onImpactEvent.Invoke();
        if(onImpactDelegate != null) {
            onImpactDelegate();
        }

        ExplosionController controller = ((GameObject)Instantiate(explosionPrefab, transform.position, transform.rotation)).GetComponent<ExplosionController>();

        controller.initExplosion(explosionRadius, damage);

        if(destroyOnImpact) {
            Destroy(gameObject);
        }
    }

}
