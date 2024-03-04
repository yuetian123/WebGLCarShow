using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CtrlLightButton : MonoBehaviour
{
    public WheelModelStyle models;
    public float minValue;
    public float maxValue;
    private bool isOpen;
    private Button button;
    private Transform camera;
    private Vector3 target;
    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
        camera = Camera.main.transform;
        isOpen = true;
    }
    private void Update()
    {
        target = (transform.position - camera.position).normalized + transform.position;
        transform.LookAt(target);
    }
    private void OnClick()
    {
        if (isOpen)
        {
            CloseLight();
        }
        else
        {
            OpenLight();
        }
    }
    private void CloseLight()
    {
        for (int i = 0; i < models.wheels.Length; i++)
        {
            models.wheels[i].GetComponent<MeshRenderer>().material.SetFloat("_EmissionScale", minValue) ;
        }
        
        isOpen = false;
    }
    private void OpenLight()
    {
        for (int i = 0; i < models.wheels.Length; i++)
        {
            models.wheels[i].GetComponent<MeshRenderer>().material.SetFloat("_EmissionScale", maxValue);
        }
        isOpen = true;
    }
}
