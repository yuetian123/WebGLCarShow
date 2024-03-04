using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelModelStyle : StyleModels
{
    public GameObject[] wheels;
    
    public override void ChangeStyle(int index)
    {
        HiddenWheels();
        wheels[index].SetActive(true);
    }
    private void HiddenWheels()
    {
        for (int i = 0; i < wheels.Length; i++)
        {
            if (wheels[i].activeSelf)
            {
                wheels[i].SetActive(false);
            }
        }
    }
}
