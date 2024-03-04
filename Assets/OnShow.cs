using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnShow : MonoBehaviour
{
    public GameObject[] shower;
    public GameObject[] hiddener;
    private void OnEnable()
    {
        Invoke("Show",3);
    }
    public void Show()
    {
        for (int i = 0; i < shower.Length; i++)
        {
            shower[i].SetActive(true);
        }
        for (int i = 0; i < hiddener.Length; i++)
        {
            hiddener[i].SetActive(false);
        }
    }
    public void Hidden()
    {
        for (int i = 0; i < shower.Length; i++)
        {
            shower[i].SetActive(false);
        }
        for (int i = 0; i < hiddener.Length; i++)
        {
            hiddener[i].SetActive(true);
        }
    }
    private void OnDisable()
    {
        AnimatorManager.instance.ans[9].SetBool("isOpen",false);
        AnimatorManager.instance.ans[10].SetBool("isOpen", false);
        Invoke("Hidden",5);
    }
}
