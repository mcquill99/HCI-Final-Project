using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public delegate void VoidEvent();
public delegate void SingleEvent(float val);
public delegate void BoolEvent(bool val);


[System.Serializable]
public class SingleUnityEvent : UnityEvent<float>{};
[System.Serializable]
public class BoolUnityEvent : UnityEvent<bool>{};
