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

    void Start()
    {
        keyChangeTimestamp = -100f;
        radialMenu.SetActive(false);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q)) {
            enableWeaponMenu();
        } else if(Input.GetKeyUp(KeyCode.Q)) {
            radialMenuController.submitCurrentButton();
            disableWeaponMenu();
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
    }
}
