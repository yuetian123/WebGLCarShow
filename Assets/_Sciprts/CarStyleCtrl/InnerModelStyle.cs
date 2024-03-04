using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InnerModelStyle : StyleModels
{
    [Header("��������")]
    public Material[] chuangLianMats;
    [Header("�ڲ�����")]
    public Material[] innerMats;
    [Header("���β���")]
    public Material[] chairMats;
    [Header("�ڶ�����")]
    public Material[] dingKeMats;
    [Header("�пز���")]
    public Material[] panelMats;

    [Header("��������")]
    public MeshRenderer[] chuangLianMesh;
    [Header("�ڲ�����")]
    public MeshRenderer[] innerMesh;
    [Header("��������")]
    public MeshRenderer[] chairMesh;
    [Header("�п�����")]
    public MeshRenderer[] panelMesh;
    [Header("�ڶ�����")]
    public MeshRenderer dingKeMesh;
    /// <summary>
    /// �л�����
    /// ����
    /// ����
    /// </summary>
    /// <param name="index"></param>
    public override void ChangeStyle(int index)
    {
        for (int i = 0; i < chuangLianMesh.Length; i++)
        {
            chuangLianMesh[i].material = chuangLianMats[index];
        }
        for (int i = 0; i < innerMesh.Length; i++)
        {
            innerMesh[i].material = innerMats[index];
        }
        for (int i = 0; i < chairMesh.Length; i++)
        {
            chairMesh[i].material = chairMats[index];
        }
        for (int i = 0; i < panelMesh.Length; i++)
        {
            panelMesh[i].material = panelMats[index];
        }
        dingKeMesh.material = dingKeMats[index];
    }
}
