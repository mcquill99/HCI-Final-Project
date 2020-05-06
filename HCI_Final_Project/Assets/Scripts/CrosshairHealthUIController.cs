using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrosshairHealthUIController : MonoBehaviour
{
    public Image outerHealth;
    public Image innerHealth;
    //TODO: Change this if we decide to have player health not max at 100
    public float maxHealth = 100;

    public void healthChanged(float health) {
        outerHealth.fillAmount = health / maxHealth;
        outerHealth.rectTransform.rotation = Quaternion.Euler(0, 0, 180f + (180f * outerHealth.fillAmount));
        innerHealth.fillAmount = (maxHealth - health) / maxHealth;
        innerHealth.rectTransform.rotation = Quaternion.Euler(0, 0, 180f * innerHealth.fillAmount);

    }
}
