using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoUIController : MonoBehaviour
{
    public Text ammoText;
    public WeaponSwapController swapController;
    private WeaponFireController weapon;
    public void Update() {
        weapon = swapController.getCurrentWeapon();
        ammoText.text = weapon.currentAmmo + "|" + weapon.maxAmmo;
    }

}
