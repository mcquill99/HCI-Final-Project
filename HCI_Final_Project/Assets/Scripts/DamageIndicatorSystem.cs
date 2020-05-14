using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class DamageIndicatorSystem : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private DamageIndicator indicatorPrefab = null;
    [SerializeField]
    private RectTransform holder = null;
    [SerializeField]
    private new Camera camera = null;
    [SerializeField]
    private Transform player = null;
    [SerializeField]
    private float timer = 0.00F;

    private Dictionary<Vector3, DamageIndicator> Indicators = new Dictionary<Vector3, DamageIndicator>();

    #region Delegates
    public static Action<Vector3, float> CreateIndicator = delegate {};
    public static Func<Vector3, bool> CheckIfObjectInSight = null;
    #endregion

    private void OnEnable()
    {
        CreateIndicator += AddDamageIndicator;
        CheckIfObjectInSight += InSight;
    
        player.gameObject.GetComponent<HealthController>().onRecieveDamageDelegate += AddDamageIndicator;
    }

    private void OnDisable()
    {
        player.gameObject.GetComponent<HealthController>().onRecieveDamageDelegate -= AddDamageIndicator;
    }
    public void AddDamageIndicator(Vector3 target, float damage)
    {
        if(damage <= 0)
            return;

        if(Indicators.ContainsKey(target))
        {
            Indicators[target].Restart();
            return;
        }
        DamageIndicator newIndicator = Instantiate(indicatorPrefab, holder);
        newIndicator.Register(target, player, new Action( () => {Indicators.Remove(target); } ), timer, damage);

        Indicators.Add(target, newIndicator);
    }
    bool InSight(Vector3 target)
    {
        Vector3 screenPoint = camera.WorldToViewportPoint(target);
        return screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
    }
}
