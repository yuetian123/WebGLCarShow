using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpLightModelStyle : StyleModels
{
    public GameObject[] upLights;//²ÄÖÊ
    
    private void Awake()
    {
    }
    private void Start()
    {
        
    }
    public override void ChangeStyle(int index)
    {
        for (int i = 0; i < upLights.Length; i++)
        {
            if (upLights[i].activeSelf)
            {
                upLights[i].SetActive(false);
            }
        }
        upLights[index].gameObject.SetActive(true);
    }
}
