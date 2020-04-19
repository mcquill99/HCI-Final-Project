using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using NaughtyAttributes;
public class WeaponSwapController : MonoBehaviour
{
    [BoxGroup("References")] public GameObject radialMenu;
    [BoxGroup("References")] public PostProcessVolume blurVolume; 
    [BoxGroup("References")] public VHS.CameraController cameraController; 
    [BoxGroup("References")] public RMF_RadialMenu radialMenuController;
    [BoxGroup("References")] public Transform weaponsHolder;

    [Space]

    [BoxGroup("Settings")] public float slowDownTimeScale = 0.25f; //Value for timescale when choosing weapon
    [BoxGroup("Settings")] public float timeScaleChangeDuration = 0.3f; //Duration of time it takes to animate
    [BoxGroup("Settings")] public AnimationCurve slowDownCurve; //Custom easing curve for animation

    [Space]

    private float regularTimeScale = 1f; //default timescale for returning to
    private float keyChangeTimestamp; //Represents the unscaled time in game-time when key is pressed/released
    [BoxGroup("DEBUG")][ShowNonSerializedField] private bool isGoalSlow = false; //is the goal to open the menu or close it
    [BoxGroup("DEBUG")][ShowNonSerializedField] private int currentWeaponIndex;
    private const int TOTAL_NUM_WEAPONS = 4;
    private List<Transform> weapons;

    void Start()
    {
        keyChangeTimestamp = -100f; //Time is 0 by default, so I set the timestamp to -100 initially so that there is no animation at the beginning
        radialMenu.SetActive(false);

        //Populate weapon list with all weapons
        weapons = new List<Transform>();
        foreach(Transform t in weaponsHolder) {
            weapons.Add(t);
        }

        //Set current weapon index and choose a weapon that isn't that index to play the equip animation
        currentWeaponIndex = 0;
        chooseWeapon(1);

    }

    void Update()
    {
        
        if(Input.GetKeyDown(KeyCode.Q)) {
            //if the key is pressed down, open the weapon menu
            enableWeaponMenu();
        } else if(Input.GetKeyUp(KeyCode.Q)) {
            //if the key is released, tell the radial menu to choose the highlighted segment and disable the weapon menu
            radialMenuController.submitCurrentButton();
            disableWeaponMenu();
        }

        //if the user scrolls, that changes the equiped weapon
        if(Input.mouseScrollDelta.y != 0) {
            int choice = currentWeaponIndex + Mathf.Clamp(Mathf.CeilToInt(Input.mouseScrollDelta.y), -1, 1);
            choice = choice % TOTAL_NUM_WEAPONS;
            choice = choice < 0 ? choice + TOTAL_NUM_WEAPONS : choice;
            chooseWeapon(choice);
        }

        //animate the screen blur and timescale
        float progress = 1f - (Mathf.Clamp(keyChangeTimestamp - Time.unscaledTime, 0, timeScaleChangeDuration) / timeScaleChangeDuration);
        Time.timeScale = Mathf.Lerp(isGoalSlow ? regularTimeScale : slowDownTimeScale, isGoalSlow ? slowDownTimeScale : regularTimeScale, slowDownCurve.Evaluate(progress));
        blurVolume.weight = Mathf.Lerp(isGoalSlow ? 0 : 1, isGoalSlow ? 1 : 0, slowDownCurve.Evaluate(progress));
    }

    void enableWeaponMenu() {
        if(!isGoalSlow) {
            keyChangeTimestamp = Time.time;
            isGoalSlow = true;
            Cursor.lockState = CursorLockMode.None;
            radialMenu.SetActive(true);
            cameraController.lockLook = true;
        }
    }

    void disableWeaponMenu() {
        if(isGoalSlow) {
            keyChangeTimestamp = Time.time;
            isGoalSlow = false;
            radialMenu.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            cameraController.lockLook = false;
        }
        
    }

    public void chooseWeapon(int weaponNum) {
        //choosing can be done by clicking or pressing enter, so close the menu
        disableWeaponMenu();
        //print("CHOSE WEAPON : " + weaponNum);
        //if the chosen weapon isn't the already equiped one, equip it
        if(weaponNum != currentWeaponIndex) {
            currentWeaponIndex = weaponNum;
            for(int i = 0; i < weapons.Count; i++) {
                weapons[i].gameObject.SetActive(i==currentWeaponIndex);
            }
        }
    }
}
