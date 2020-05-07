using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HealthUIController : MonoBehaviour
{

    public Slider healthSlider;
    public Slider invertedHealthSlider;

    //TODO: Change this if we decide to have player health not max at 100
    public int maxHealth = 100;

    public void healthChanged(float health) {
        healthSlider.value = health;
        invertedHealthSlider.value = maxHealth - health;
    }
}
