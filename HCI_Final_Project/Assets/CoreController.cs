using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoreController : MonoBehaviour
{
    public HealthController health;
    public Transform player;
    public Transform upperLid;
    public Transform lowerLid;
    public Transform eye;
    public AnimationCurve lidEasing;
    public float lidAnimDuration = 1f;
    public Vector2 upperlidAngles;
    public Vector2 lowerlidAngles;
    private float lidTimestamp = -1000f;
    private bool coreEnabled = false;
    void Start()
    {
        // startCore();
        if(player == null) {
            //TODO: not this
            player = GameObject.Find("Player").transform;
        }
        health.invincible = true;
    }

    void Update()
    {
        eye.LookAt(player.position + Vector3.up);

        float progress = Mathf.Clamp((Time.time - lidTimestamp) / lidAnimDuration, 0, 1f);
        progress = coreEnabled ? 1f - progress : progress;
        float upperValue = Mathf.Lerp(upperlidAngles.x, upperlidAngles.y, progress);
        float lowerValue = Mathf.Lerp(lowerlidAngles.x, lowerlidAngles.y, progress);
        upperLid.localEulerAngles = new Vector3(0, 90, upperValue);
        lowerLid.localEulerAngles = new Vector3(0, 90, lowerValue);

    }

    public void startCore() {
        lidTimestamp = Time.time;
        coreEnabled = true;
        health.invincible = false;
    }
}
