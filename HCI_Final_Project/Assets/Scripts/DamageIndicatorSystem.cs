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

    private Dictionary<Transform, DamageIndicator> Indicators = new Dictionary<Transform, DamageIndicator>();

    #region Delegates
    public static Action<Transform, float> CreateIndicator = delegate {};
    public static Func<Transform, bool> CheckIfObjectInSight = null;
    #endregion

    private void OnEnable()
    {
        CreateIndicator += Create;
        CheckIfObjectInSight += InSight;
    }

    private void OnDisable()
    {

    }
    void Create(Transform target, float damage)
    {
        if(Indicators.ContainsKey(target))
        {
            Indicators[target].Restart();
            return;
        }
        DamageIndicator newIndicator = Instantiate(indicatorPrefab, holder);
        newIndicator.Register(target, player, new Action( () => {Indicators.Remove(target); } ), timer, damage);

        Indicators.Add(target, newIndicator);
    }
    bool InSight(Transform target)
    {
        Vector3 screenPoint = camera.WorldToViewportPoint(target.position);
        return screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
    }
}
