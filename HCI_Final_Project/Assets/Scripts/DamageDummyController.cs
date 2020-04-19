using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageDummyController : MonoBehaviour
{
    public TextMeshProUGUI text;
    private HealthController healthController;
    private float dps;
    private float timestamp;
    public void onRecieveDamage(float val) {
        dps += val;
        healthController.currentHealth += val;
    }

    void Update() {
        text.text = dps.ToString("F2");
        if(timestamp < Time.time) {
            timestamp = Time.time + 1f;
            dps = 0;
        }
    }

    void Start()
    {
        healthController = GetComponent<HealthController>();
        text.text = "";
    }

}
