using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public delegate void VoidDelegate();
public delegate void SingleDelegate(float val);
public delegate void CombatStateDelegate(AICombatState val);
public delegate void DamageDelegate(Vector3 pos, float val);
public delegate void BoolDelegate(bool val);
public delegate void HealthControllerDelegate(HealthController val);


[System.Serializable]
public class SingleUnityEvent : UnityEvent<float>{};
[System.Serializable]
public class BoolUnityEvent : UnityEvent<bool>{};
[System.Serializable]
public class HealthControllerEvent : UnityEvent<HealthController>{};