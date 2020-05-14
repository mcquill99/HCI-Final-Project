using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
public class ExplosionController : MonoBehaviour
{
    [Tooltip("Rate at which damage decreases based on distance from explosion center and explosion radius")]
    [BoxGroup("Settings")]public AnimationCurve damageFalloffCurve;
    [Tooltip("Sound that plays when Explosion goes off")]
    [BoxGroup("Settings")]public AudioSource explosionSound;

    private float radius;
    private float damage;

    public void initExplosion(float radius, float damage, AudioSource explosionSound) {

        this.explosionSound = explosionSound;
        this.radius = radius;
        this.damage = damage;
        StartCoroutine(explode());
    }

    IEnumerator explode() {
        yield return new WaitForSeconds(0.1f);
        if(explosionSound!= null){
            explosionSound.pitch = Random.Range(0.85f, 1.15f);
            AudioSource.PlayClipAtPoint(explosionSound.clip, transform.position);
        }
        
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius, 1<<10 | 1<<12| 1<<13);
        foreach(Collider col in colliders) {
            
            HealthControllerReferencer hcr = col.GetComponent<HealthControllerReferencer>();
            float val = Vector3.Magnitude(col.ClosestPoint(transform.position) - transform.position) / radius;
            float calculatedDamage = damage * damageFalloffCurve.Evaluate(val);
            //print(calculatedDamage);
            if(hcr != null) {
                hcr.healthController.recieveDamage(transform.position, calculatedDamage);
            }
        }
        Destroy(gameObject, 2f);
    }
}
