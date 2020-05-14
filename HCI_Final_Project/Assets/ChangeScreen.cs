using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
public class ChangeScreen : MonoBehaviour
{
    [Tooltip("The mesh whose material(s) you'd like to switch.")]
    [BoxGroup("Settings")][ReorderableList] public List<MaterialSet> changeToMaterials;
    [BoxGroup("Settings")]public Renderer meshRenderer;

    public void switchMaterial(int materialSetIndex){
        meshRenderer.materials = changeToMaterials[materialSetIndex].getMaterialArray();
    }
}

[System.Serializable]
public class MaterialSet{
    public Material[] materials;

    public Material[] getMaterialArray(){
        return materials;
    }
} 