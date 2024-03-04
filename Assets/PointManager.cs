using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointManager : MonoBehaviour
{
    public Transform innerCamera;
    [Header("Œª÷√")]
    public Transform[] ps;

    public void SetPoint(int index)
    {
        innerCamera.position = ps[index].position;
    }


}
