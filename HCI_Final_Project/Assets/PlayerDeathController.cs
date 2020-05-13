using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class PlayerDeathController : MonoBehaviour
{
    public HealthController healthController;
    public GameObject resetPanel;
    public GameObject deathPanel;
    public GameObject tilter;
    public GameObject crosshair;
    public GameObject deathCam;
    public Slider resetSlider;
    float resetTimestamp;
    public float resetHoldTime = 3f;
    float cannotResetTimestamp;

    void Start()
    {
        healthController.onDeathDelegate += OnDeath;
        deathPanel.SetActive(false);
        deathCam.SetActive(false);
        resetPanel.SetActive(false);
        cannotResetTimestamp = Time.time + resetHoldTime;
    }

    void Update()
    {
        if(cannotResetTimestamp > Time.time)
            return;

        if(Input.GetKeyDown(KeyCode.R)) {
            resetPanel.SetActive(true);
            resetTimestamp = Time.time + resetHoldTime;
        } else if(Input.GetKey(KeyCode.R)) {
            float progress = 1f - Mathf.Clamp((resetTimestamp - Time.time) / resetHoldTime, 0, 1f);
            resetSlider.value = progress;
            if(resetTimestamp < Time.time) {
                resetLevel();
            }
        } else if(Input.GetKeyUp(KeyCode.R)) {
            resetPanel.SetActive(false);
        }
    }

    public void OnDeath() {
        deathPanel.SetActive(true);
        healthController.gameObject.SetActive(false);
        deathCam.SetActive(true);
        tilter.SetActive(false);
        crosshair.SetActive(false);
        resetHoldTime = 0;
    }

    public void resetLevel() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
    }
}
