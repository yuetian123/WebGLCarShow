using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InnerModelStyle : StyleModels
{
    [Header("窗帘材质")]
    public Material[] chuangLianMats;
    [Header("内部材质")]
    public Material[] innerMats;
    [Header("座椅材质")]
    public Material[] chairMats;
    [Header("内顶材质")]
    public Material[] dingKeMats;
    [Header("中控材质")]
    public Material[] panelMats;

    [Header("窗帘网格")]
    public MeshRenderer[] chuangLianMesh;
    [Header("内部网格")]
    public MeshRenderer[] innerMesh;
    [Header("座椅网格")]
    public MeshRenderer[] chairMesh;
    [Header("中控网格")]
    public MeshRenderer[] panelMesh;
    [Header("内顶网格")]
    public MeshRenderer dingKeMesh;
    /// <summary>
    /// 切换窗帘
    /// 内饰
    /// 座椅
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
