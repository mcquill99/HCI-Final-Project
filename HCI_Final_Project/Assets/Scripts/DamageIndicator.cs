using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class DamageIndicator : MonoBehaviour
{
    private float MaxTime = 8;
    private float timer;
    private float damage;


    private CanvasGroup canvasGroup = null;
    protected CanvasGroup CanvasGroup
    {
        get
        {
            if(canvasGroup == null)
            {
                canvasGroup = GetComponent<CanvasGroup>();
                if(canvasGroup == null)
                {
                    canvasGroup = gameObject.AddComponent<CanvasGroup>();
                }
            }
            return canvasGroup;
        }

    }

    private RectTransform rect = null;
    protected RectTransform Rect
    {
        get
        {
            if(rect == null)
            {
                rect = GetComponent<RectTransform>();
                if(rect == null)
                {
                    rect = gameObject.AddComponent<RectTransform>();
                }
            }
            return rect;
        }
    }

    public Vector3 Target {get; protected set; }
    private Transform Player = null;

    private IEnumerator IE_Countdown = null;
    private Action unRegister = null;

    private Quaternion targetRot = Quaternion.identity;
    private Vector3 targetPos = Vector3.zero;

    public void Register(Vector3 target, Transform player, Action unRegister, float time, float damage)
    {
        this.Target = target;
        this.Player = player;
        this.unRegister = unRegister;

        this.MaxTime = time;
        this.timer = MaxTime;
        this.damage = damage;

        StartCoroutine(RotateToTarget());
        StartTimer();


    }
    public void Restart()
    {
        timer = MaxTime;
        StartTimer();
    }
    private void StartTimer()
    {
        if(IE_Countdown != null)
        {
           StopCoroutine(IE_Countdown); 
        }
        IE_Countdown = Countdown();
        StartCoroutine(IE_Countdown);
    }

    private IEnumerator Countdown()
    {
        float AlphaVal = Mathf.Clamp(185 + damage, 155, 255);
        // float RedVal = Mathf.Clamp(255 - (damage * 2),100, 255);

        CanvasGroup.GetComponentInChildren<Image>().color = new Color(255,255,255,(byte)AlphaVal);
        while(CanvasGroup.alpha < 1.00F)
        {
            CanvasGroup.alpha += 4 * Time.deltaTime;
            yield return null;
        }

        while(timer > 0)
        {
            timer--;
            yield return new WaitForSeconds(1);
        }

        while(CanvasGroup.alpha > 0.0f)
        {
            CanvasGroup.alpha -= 2 * Time.deltaTime;
            yield return null;
        }
        unRegister();
        Destroy(gameObject);
    }
    IEnumerator RotateToTarget()
    {
        while(enabled)
        {

            targetPos = Target;
            // targetRot = Target;
            Vector3 direction = Player.position - targetPos;

            targetRot = Quaternion.LookRotation(direction);
            targetRot.z = -targetRot.y;
            targetRot.x = 0;
            targetRot.y = 0; 

            Vector3 northDirection = new Vector3(0,0,Player.eulerAngles.y);
            Rect.localRotation = targetRot * Quaternion.Euler(northDirection);

            yield return null;
        }
    }
}
