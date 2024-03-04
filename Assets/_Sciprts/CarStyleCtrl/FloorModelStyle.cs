using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorModelStyle : StyleModels
{
    public Material[] mats;//²ÄÖÊ
    private Material[] meshMats;
    private void Awake()
    {
        meshMats = GetComponent<MeshRenderer>().materials;
    }

    public override void ChangeStyle(int index)
    {
        meshMats[1].CopyPropertiesFromMaterial(mats[index]);   
    }
}
