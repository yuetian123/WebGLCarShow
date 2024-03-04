using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CtrlLightButtonDoubleMesh : MonoBehaviour
{
    public WheelModelStyle models;
    public float minValue;
    public float maxValue;
    private bool isOpen;
    private Button button;
    private Transform camera;
    private Vector3 target;
    private Material mas;
    private MeshRenderer tempMesh;
    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
        camera = Camera.main.transform;
        isOpen = true;
        mas = new Material("KeroTools/URP+/ComplexLit");
        
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
            if (i == 1 | i == 2 || i == 3 || i == 5)
            {
                tempMesh = models.wheels[i].GetComponent<MeshRenderer>();
                mas.CopyPropertiesFromMaterial(tempMesh.materials[0]);
                mas.SetFloat("_EmissionScale", minValue);
                tempMesh.materials[0].CopyPropertiesFromMaterial(mas);
                continue;
            }
            tempMesh = models.wheels[i].GetComponent<MeshRenderer>();
            mas.CopyPropertiesFromMaterial(tempMesh.materials[1]);
            mas.SetFloat("_EmissionScale", minValue);
            tempMesh.materials[1].CopyPropertiesFromMaterial(mas);
        }
        isOpen = false;
    }
    private void OpenLight()
    {
        for (int i = 0; i < models.wheels.Length; i++)
        {
            if (i == 1 | i == 2 || i == 3 || i == 5)
            {
                tempMesh = models.wheels[i].GetComponent<MeshRenderer>();
                mas.CopyPropertiesFromMaterial(tempMesh.materials[0]);
                mas.SetFloat("_EmissionScale", maxValue);
                tempMesh.materials[0].CopyPropertiesFromMaterial(mas);
                continue;
            }
            tempMesh = models.wheels[i].GetComponent<MeshRenderer>();
            mas.CopyPropertiesFromMaterial(tempMesh.materials[1]);
            mas.SetFloat("_EmissionScale", maxValue);
            tempMesh.materials[1].CopyPropertiesFromMaterial(mas);
        }
        isOpen = true;
    }
}
