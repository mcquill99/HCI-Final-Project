using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
public class WeaponSwapController : MonoBehaviour
{
    float regularTimeSpeed = 1f;
    public float slowDownTimeSpeed = 0.25f;
    public float timeSpeedChangeDuration = 0.3f;
    public AnimationCurve slowDownCurve;
    float keyChangeTimestamp;
    bool isGoalSlow = false;
    public GameObject radialMenu;
    public PostProcessVolume blurVolume;
    public VHS.CameraController cameraController;
    public RMF_RadialMenu radialMenuController;

    private int currentWeaponIndex = 0;
    private const int totalNumWeapons = 4;

    public Transform weaponsHolder;
    private List<Transform> weapons;

    void Start()
    {
        keyChangeTimestamp = -100f;
        radialMenu.SetActive(false);
        weapons = new List<Transform>();
        foreach(Transform t in weaponsHolder) {
            weapons.Add(t);
        }

        chooseWeapon(0);

    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q)) {
            enableWeaponMenu();
        } else if(Input.GetKeyUp(KeyCode.Q)) {
            radialMenuController.submitCurrentButton();
            disableWeaponMenu();
        }

        if(Input.mouseScrollDelta.y != 0) {
            int choice = currentWeaponIndex + Mathf.Clamp(Mathf.CeilToInt(Input.mouseScrollDelta.y), -1, 1);
            choice = choice % totalNumWeapons;
            choice = choice < 0 ? choice + totalNumWeapons : choice;
            chooseWeapon(choice);
        }

        float progress = Mathf.Clamp(Time.time - keyChangeTimestamp, 0, timeSpeedChangeDuration) / timeSpeedChangeDuration;
        Time.timeScale = Mathf.Lerp(isGoalSlow ? regularTimeSpeed : slowDownTimeSpeed, isGoalSlow ? slowDownTimeSpeed : regularTimeSpeed, progress);
        blurVolume.weight = Mathf.Lerp(isGoalSlow ? 0 : 1, isGoalSlow ? 1 : 0, progress);
    }

    void enableWeaponMenu() {
        if(!isGoalSlow) {
            keyChangeTimestamp = Time.time;
            isGoalSlow = true;
            Cursor.lockState = CursorLockMode.None;
            radialMenu.SetActive(true);
            //Cursor.visible = true;
            cameraController.lockLook = true;
        }
    }

    void disableWeaponMenu() {
        if(isGoalSlow) {
            keyChangeTimestamp = Time.time;
            isGoalSlow = false;
            //Cursor.visible = false;
            radialMenu.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            cameraController.lockLook = false;
        }
        
    }

    public void chooseWeapon(int weaponNum) {
        disableWeaponMenu();
        print("CHOSE WEAPON : " + weaponNum);
        if(weaponNum != currentWeaponIndex) {
            currentWeaponIndex = weaponNum;
            for(int i = 0; i < weapons.Count; i++) {
                weapons[i].gameObject.SetActive(i==currentWeaponIndex);
            }
        }
    }
}
