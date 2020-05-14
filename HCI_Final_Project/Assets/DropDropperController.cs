using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropDropperController : MonoBehaviour
{
    public GameObject ammoDrop;
    public GameObject healthDrop;
    public float chanceToDrop = 0.15f;
    private HealthController healthController;
    void Start()
    {
        healthController = GetComponent<HealthController>();
        healthController.onDeathDelegate += onDeath;
    }

    void onDeath() {
        float chance = Random.Range(0f, 1f);
        if(chance < chanceToDrop) {
            Instantiate(healthDrop, transform.position, Quaternion.identity);
        }
    }
}
