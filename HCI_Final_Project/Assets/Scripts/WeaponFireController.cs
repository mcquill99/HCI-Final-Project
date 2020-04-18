﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public enum FireMode {Single, SemiAuto, FullAuto}
public enum DeliveryType {Hitscan, Projectile}

public class WeaponFireController : MonoBehaviour
{
    
    [BoxGroup("References")] public GameObject muzzle;
    [BoxGroup("References")] public WeaponController weaponController;
    [Space]
    [BoxGroup("Settings")] public float damage;
    [BoxGroup("Settings")] public float fireRate;
    [BoxGroup("Settings")] public int maxAmmo;
    [BoxGroup("Settings")] public int bulletsPerAmmo = 1;
    [BoxGroup("Settings")][Slider(0f, 1f)] public float inaccuracy;

    [OnValueChanged("onFireModeChangedCallback")]
    [BoxGroup("Settings")] public FireMode fireMode;
    [OnValueChanged("onDeliveryTypeChangedCallback")]
    [BoxGroup("Settings")] public DeliveryType deliveryType;
    [Space]
    [BoxGroup("Additional Settings")][ShowIf("isSemiAuto")] public float shotsPerBurst;
    [BoxGroup("Additional Settings")][ShowIf("isSemiAuto")]  private int numFiredInBurst;
    [BoxGroup("Additional Settings")][ShowIf("isSemiAuto")]  public float burstCooldown;
    [BoxGroup("Additional Settings")][ShowIf("isProjectile")]  public GameObject projectilePrefab;


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
        //TODO: Make this not forced to use the camera
        Transform cam = Camera.main.transform;
        int layerMask = 1 | 1 << 9 | 1 << 13;
        float distanceFromCenter = Random.Range(0, inaccuracy);
        float radians = accuracyConeNum / numConeSegments * 2f * Mathf.PI;
        Vector2 accuracyOffset = new Vector2(distanceFromCenter * Mathf.Cos(radians), distanceFromCenter * Mathf.Sin(radians));
        Vector3 trajectory = cam.forward + cam.rotation * accuracyOffset;

        accuracyConeNum = (accuracyConeNum + 1) % numConeSegments;

        if(Physics.Raycast(cam.position, trajectory, out hit, 100000f, layerMask)) {
            Debug.DrawLine(cam.position, hit.point, Color.yellow);

            HealthControllerReferencer r = hit.collider.GetComponent<HealthControllerReferencer>();
            if(r != null) {
                HealthController healthController = r.healthController;
                if(healthController) {
                    healthController.recieveDamage(damage);
                }
            }
        }
    }

    public void shootProjectile() {
        RaycastHit hit;
        Transform cam = Camera.main.transform;
        int layerMask = 1 | 1 << 9 | 1 << 13;
        Quaternion projectileRotation;
        if(Physics.Raycast(cam.position, cam.forward, out hit, 100000f, layerMask)) {
            projectileRotation = Quaternion.LookRotation(hit.point - muzzle.transform.position, Vector3.up);
        } else {
            projectileRotation = Quaternion.LookRotation(cam.forward * 100000f - muzzle.transform.position, Vector3.up);
        }
        ProjectileController projectileController = ((GameObject)Instantiate(projectilePrefab, muzzle.transform.position, projectileRotation)).GetComponent<ProjectileController>();
        projectileController.InitializeProjectile(damage, weaponController.firstPersonController.getVelocity());
    }

}
