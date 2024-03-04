using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InnerCtrlAnimationButton : MonoBehaviour
{
    public InnerCtrlPosition ctrlPos;
    private Transform camera;
    private Vector3 target;
    private Button but;
    private void Awake()
    {
        camera = CameraManager.instance.cameras[7].transform;
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
        temp = AnimatorManager.instance.ans[(int)ctrlPos + 5];
        if (temp.GetBool("isOpen"))
        {
            temp.SetBool("isOpen", false);
        }
        else
        {
            temp.SetBool("isOpen", true);
        }
    }

}

public enum InnerCtrlPosition
{
    ForwardLeftWindow,
    ForwardRightWindow,
    ForwardLeftLittleTable,
    ForwardRightLittleTable,
    MiddleLeftWindow,
    MiddleRightWindow,
    MiddleLeftChair,
    MiddleRightChair,
    behindChair,
}