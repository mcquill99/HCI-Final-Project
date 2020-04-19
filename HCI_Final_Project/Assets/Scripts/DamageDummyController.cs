using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using NaughtyAttributes;

public class DamageDummyController : MonoBehaviour
{
    [Tooltip("TMPro UGUI to print dps to")]
    [Required][BoxGroup("References")]public TextMeshProUGUI text;
    private HealthController healthController;
    private float currentDPS;
    private float previousDPS;
    private float timestamp;
    public void onRecieveDamage(float val) {
        currentDPS += val;
        healthController.currentHealth += val;
    }

    void Update() {
        text.text = previousDPS.ToString("F2");
        if(timestamp < Time.time) {
            timestamp = Time.time + 1f;
            previousDPS = currentDPS;
            currentDPS = 0;
        }
    }

    void Start()
    {
        healthController = GetComponent<HealthController>();
        text.text = "";
    }

}
