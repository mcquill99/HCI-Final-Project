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
    [BoxGroup("Settings")] public float explosionRadius = 2f;
    [BoxGroup("Settings")] public float timeBeforeSelfDestruct = 8f;
    [BoxGroup("Settings")] public LayerMask layers;

    [BoxGroup("Settings")] public GameObject explosionPrefab;
    [BoxGroup("Events")] public UnityEvent onImpactEvent;
    public VoidDelegate onImpactDelegate;
    private Rigidbody rigidbody;
    private float damage;
    private float selfDestructTimeStamp;
    
    public void InitializeProjectile(float damage, Vector3 inheritVelocity, LayerMask layers) {
        this.damage = damage;
        this.layers = layers;
        rigidbody = GetComponent<Rigidbody>();  

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

    void explode() {
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
