using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;


public class WeaponAnimationSyncController : MonoBehaviour
{
    public static WeaponAnimationSyncController instance;
    public Animator hands;
    [ReorderableList]public List<Animator> animators;

    void Awake() {
        instance = this;
    }

    void Update() {
        
    }

    public void setFloat(string name, float value) {
        foreach(Animator a in animators) {
            if(a)
                a.SetFloat(name, value);
        }
    }

    public void setBool(string name, bool value) {
        foreach(Animator a in animators) {
            if(a)
                a.SetBool(name, value);
        }
    }

    public void setTrigger(string name) {
        foreach(Animator a in animators) {
            if(a)
                a.SetTrigger(name);
        }
    }

    public void addAnimator(Animator animator) {
        animators = new List<Animator>();
        animators.Add(hands);
        animators.Add(animator);
    }

    public bool getAnimationEquiped() {
        return  hands.GetCurrentAnimatorStateInfo(0).IsTag("Equiped");
    }

    public bool getCanEquip() {
        return  hands.GetCurrentAnimatorStateInfo(0).IsTag("Equiped") || hands.GetCurrentAnimatorStateInfo(0).IsTag("Equiping");
    }
}
