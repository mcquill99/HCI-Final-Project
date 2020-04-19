using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public enum FireMode {Single, SemiAuto, FullAuto}
public enum DeliveryType {Hitscan, Projectile}

public class WeaponFireController : MonoBehaviour
{
    [Tooltip("Reference to muzzle of weapon. Used for instantiating projectiles")]
    [BoxGroup("References")] public GameObject muzzle;

    [Tooltip("Automatically use camera transform. If false use aimingTransform")]
    [BoxGroup("References")] public bool useCamera;

    [Tooltip("Reference to transform used for aiming")]
    [BoxGroup("References")][HideIf("useCamera")] public Transform aimingTransform;
    
    [Tooltip("Reference to weapon controller. Used to link delegates")]
    [BoxGroup("References")] public WeaponController weaponController;
    [Space]
    
    [Tooltip("Damage of weapon")]
    [BoxGroup("Settings")] public float damage;
    
    [Tooltip("Fire rate of weapon. Represented as shots per second")]
    [BoxGroup("Settings")] public float fireRate;

    [Tooltip("Maximum ammo that can be stored")]
    [BoxGroup("Settings")] public int maxAmmo;
    
    [Tooltip("Amount of bullets to instantiate per shot")]
    [BoxGroup("Settings")] public int bulletsPerAmmo = 1;

    [Tooltip("Weapon spread represened. Higher number is less accurate")]
    [BoxGroup("Settings")][Slider(0f, 1f)] public float inaccuracy;

    [Tooltip("LayerMask to check for hit")]
    [BoxGroup("Settings")] public LayerMask layers;

    [Tooltip("Weapon fire mode. Can be Single, Semi-Auto, or Full Auto")]
    [OnValueChanged("onFireModeChangedCallback")]
    
    [BoxGroup("Settings")] public FireMode fireMode;
    
    [Tooltip("Weapon delivery type. Either Hitscan or Projectile")]
    [OnValueChanged("onDeliveryTypeChangedCallback")]
    [BoxGroup("Settings")] public DeliveryType deliveryType;
    [Space]

    [Tooltip("If Semi-Auto, maximum amount of shots possible in a burst")]
    [BoxGroup("Additional Settings")][ShowIf("isSemiAuto")] public float shotsPerBurst;

    [Tooltip("Time in seconds required after full burst is fired before weapon can fire again")]
    [BoxGroup("Additional Settings")][ShowIf("isSemiAuto")]  public float burstCooldown;

    [Tooltip("Prefab to be instantiated when weapon fired and is using Projectile delivery type")]
    [BoxGroup("Additional Settings")][ShowIf("isProjectile")]  public GameObject projectilePrefab;

    [Tooltip("Effective distance that hitscan weapon can do damage. Used to calculate damage falloff")]
    [BoxGroup("Additional Settings")][ShowIf("isHitscan")]  public float effectiveDistance;

    [Tooltip("Curve for damage falloff. Used with effective distance to calculate damage")]
    [BoxGroup("Additional Settings")][ShowIf("isHitscan")]  public AnimationCurve damageFalloff;

    [Tooltip("Reference to particle system for bullet hit effects")]
    [BoxGroup("Additional Settings")][ShowIf("isHitscan")]  public ParticleSystem bulletHitEffect;

    private int numFiredInBurst;
    private float burstTimestamp;
    private float shotTimestamp;
    private int currentAmmo;
    private int accuracyConeNum = 0;
    private int numConeSegments = 15;

    private bool isSemiAuto = false;
    private bool isProjectile = false;
    private bool isHitscan = true;

    private void onDeliveryTypeChangedCallback() {
        isProjectile = deliveryType == DeliveryType.Projectile;
        isHitscan = deliveryType == DeliveryType.Hitscan;
    }
    private void onFireModeChangedCallback() {
        isSemiAuto = fireMode == FireMode.SemiAuto;
    }

    public void Start() {
        if(aimingTransform == null && useCamera) {
            aimingTransform = Camera.main.transform;
        }

        if(weaponController == null) {
            weaponController = GetComponent<WeaponController>();
        }

        if(weaponController != null) {
            weaponController.onFireStartDelegate += onFireStart;
            weaponController.onFireStayDelegate += onFireStay;
            weaponController.onFireStopDelegate += onFireStop;
        } else {
            Debug.LogError("Could not find WeaponController");
            this.enabled = false;
        }

        addAmmo(maxAmmo);
    }

    public void OnDestroy() {
        if(weaponController != null) {
            weaponController.onFireStartDelegate -= onFireStart;
            weaponController.onFireStayDelegate -= onFireStay;
            weaponController.onFireStopDelegate -= onFireStop;
        }
    }

    public void onFireStart() {
        
        if(shotTimestamp < Time.time && currentAmmo > 0 && weaponController.getIsEquiped()) {
            if(fireMode == FireMode.Single) {
                shotTimestamp = Time.time + (1f / fireRate);
                shootWeapon();
            }
        }
    }

    public void onFireStay() {
        if(shotTimestamp < Time.time && currentAmmo > 0 && weaponController.getIsEquiped()) {
            if(fireMode == FireMode.SemiAuto) {
                if(burstTimestamp < Time.time && numFiredInBurst < shotsPerBurst) {
                    shotTimestamp = Time.time + (1f / fireRate);
                    numFiredInBurst++;
                    if(numFiredInBurst >= shotsPerBurst) {
                        burstTimestamp = Time.time + burstCooldown;
                    }
                    shootWeapon();
                }
            } else if(fireMode == FireMode.FullAuto) {
                shotTimestamp = Time.time + (1f / fireRate);
                shootWeapon();
            }
        }
    }

    public void onFireStop() {
        numFiredInBurst = 0;
    }

    public void addAmmo(int amount) {
        currentAmmo = Mathf.Clamp(currentAmmo + amount, 0, maxAmmo);
    }

    public void shootWeapon() {
        if(deliveryType == DeliveryType.Hitscan) {
            for(int i = 0; i < bulletsPerAmmo; i++) {
                shootHitscan();    
            }
        } else if(deliveryType == DeliveryType.Projectile) {
            for(int i = 0; i < bulletsPerAmmo; i++) {
                shootProjectile();
            }
        }
        currentAmmo--;
    }

    public void shootHitscan() {
        RaycastHit hit;
        int layerMask = 1 | 1 << 9 | 1 << 13 | 1 << 10;
        float distanceFromCenter = Random.Range(0, inaccuracy);
        float radians = (accuracyConeNum * 1.0f) / numConeSegments * 2f * Mathf.PI;
        Vector2 accuracyOffset = new Vector2(distanceFromCenter * Mathf.Cos(radians), distanceFromCenter * Mathf.Sin(radians));
        Vector3 adjustedOffset = aimingTransform.rotation * accuracyOffset;
        Debug.DrawLine(aimingTransform.forward, aimingTransform.forward + adjustedOffset, Color.magenta);
        Vector3 trajectory = aimingTransform.forward + adjustedOffset;

        accuracyConeNum = (accuracyConeNum + 1) % numConeSegments;

        if(Physics.Raycast(aimingTransform.position, trajectory, out hit, 100000f, layerMask)) {
            Debug.DrawLine(aimingTransform.position, hit.point, Color.yellow);

            if(bulletHitEffect) {
                bulletHitEffect.transform.position = hit.point;
                bulletHitEffect.transform.LookAt(hit.point + hit.normal);
                bulletHitEffect.Emit(1);
            }
            
            HealthControllerReferencer r = hit.collider.GetComponent<HealthControllerReferencer>();
            if(r != null) {
                HealthController healthController = r.healthController;
                if(healthController) {
                    float dist = Mathf.Clamp(Vector3.Distance(hit.collider.ClosestPoint(transform.position), transform.position) / effectiveDistance, 0, 1f);
                    healthController.recieveDamage(damage / bulletsPerAmmo * damageFalloff.Evaluate(dist));
                }
            }
        }
    }

    public void shootProjectile() {
        RaycastHit hit;
        int layerMask = 1 | 1 << 9 | 1 << 13;
        Quaternion projectileRotation;
        if(Physics.Raycast(aimingTransform.position, aimingTransform.forward, out hit, 100000f, layerMask)) {
            projectileRotation = Quaternion.LookRotation(hit.point - muzzle.transform.position, Vector3.up);
        } else {
            projectileRotation = Quaternion.LookRotation(aimingTransform.forward * 100000f - muzzle.transform.position, Vector3.up);
        }
        ProjectileController projectileController = ((GameObject)Instantiate(projectilePrefab, muzzle.transform.position, projectileRotation)).GetComponent<ProjectileController>();
        projectileController.InitializeProjectile(damage, weaponController.movementController.getVelocity(), layers, weaponController.movementController.gameObject);
    }

}
