using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class CameraManager : Singleton<CameraManager>
{
    public GameObject[] cameras;//�洢����Camera 
    private int currentIndex;
    private void Awake()
    {
        base.Awake();
        currentIndex = 0;
    }
    //ʹ��ĳ�������
    public void UseCamera(int index)
    {
        HiddenCamera();
        cameras[index].gameObject.SetActive(true);
    }
    private void HiddenCamera()
    {
        for (int i = 0; i < cameras.Length; i++)
        {
            if (cameras[i] == null)
            {
                break;
            }
            if (cameras[i].gameObject.activeSelf)
            {
                cameras[i].gameObject.SetActive(false);
            }
        }
    }
}
