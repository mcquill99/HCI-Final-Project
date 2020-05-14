using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerVictoryController : MonoBehaviour
{
    public GameObject player;
    public GameObject victoryPanel;
    public GameObject tilter;
    public GameObject crosshair;
    public GameObject deathCam;
    public TimerSystem timer;
    public Text victoryTimeText;

    void Start()
    {
        victoryPanel.SetActive(false);
        deathCam.SetActive(false);
    }

    void Update() {
        if(Input.GetKeyDown(KeyCode.R) && deathCam.activeInHierarchy) {
            resetLevel();
        }
    }

    public void Victory() {
        victoryPanel.SetActive(true);
        player.SetActive(false);
        deathCam.SetActive(true);
        tilter.SetActive(false);
        crosshair.SetActive(false);
        victoryTimeText.text = timer.splits[timer.splits.Count - 1].time.ToString("n2");
    }

    public void resetLevel() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
    }
}
