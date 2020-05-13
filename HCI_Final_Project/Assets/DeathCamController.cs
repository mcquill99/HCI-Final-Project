using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathCamController : MonoBehaviour
{
    public Transform player;
    public Transform deathCam;
    public float cameraDistance = 5f;
    public float rotationSpeed = 5f;
    void OnEnable() {
        transform.position = player.position + Vector3.up;
    }

    void Update()
    {
        transform.eulerAngles += new Vector3(0, rotationSpeed * Time.deltaTime, 0);

        RaycastHit hit;
        if(Physics.SphereCast(transform.position, 0.5f, transform.forward, out hit, cameraDistance, 1 | 1 << 9 | 1 << 10 | 1 << 14)) {
            deathCam.position = hit.point + hit.normal * 0.5f;
        } else {
            deathCam.position = transform.position + (cameraDistance * transform.forward);
        }
        deathCam.LookAt(transform.position, Vector3.up);
    }
}
