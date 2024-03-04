using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CtrlAnimatorButton : MonoBehaviour
{
    public CtrlPosition ctrlPos;
    private Transform camera;
    private Vector3 target;
    private Button but;
    private void Awake()
    {
        camera = Camera.main.transform;
        but = GetComponent<Button>();
        but.onClick.AddListener(OnClick);
    }

    private void Update()
    {
        target = (transform.position - camera.position).normalized + transform.position;
        transform.LookAt(target);
    }
    /// <summary>
    /// 当按钮被点击时
    /// </summary>
    public void OnClick()
    {
        Animator temp;
        if (ctrlPos == CtrlPosition.BackSmallDoor)
        {
            temp = AnimatorManager.instance.ans[((int)ctrlPos) - 1];
            if (temp.GetBool("smallDoorOpen"))
            {
                temp.SetBool("smallDoorOpen", false);
            }
            else
            {
                if (!temp.GetBool("isOpen"))
                {
                    temp.SetBool("smallDoorOpen", true);
                }
            }
            return;
        }
        temp = AnimatorManager.instance.ans[(int)ctrlPos];
        if (ctrlPos == CtrlPosition.BackDoor)
        {
            temp.SetBool("smallDoorOpen", false);
        }
        if (temp.GetBool("isOpen"))
        {
            temp.SetBool("isOpen",false);
        }
        else
        {
            temp.SetBool("isOpen", true);
        }
    }

}

public enum CtrlPosition
{
    ForwardDoorLeft,
    ForwardDoorRight,
    MiddelDoorLeft,
    MiddelDoorRight,
    BackDoor,
    BackSmallDoor,
}