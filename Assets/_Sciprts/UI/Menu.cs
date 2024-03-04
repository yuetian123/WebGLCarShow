using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    private Animator an;
    private void Awake()
    {
        an = GetComponent<Animator>();
    }
    private void Update()
    {
        if (Input.mousePosition.x > UnityEngine.Screen.width * 0.94f && Input.mousePosition.y > UnityEngine.Screen.height*0.2f)
        {
            ShowMenu();
        }else
        {
            HiddenMenu();
        }
    }
    public void ShowMenu()
    {
        if (an.GetBool("isClose"))
        {
            an.SetBool("isClose", false);
        }
    }
    public void HiddenMenu()
    {
        if (!an.GetBool("isClose"))
        {
            an.SetBool("isClose", true);
        }
    }
}
