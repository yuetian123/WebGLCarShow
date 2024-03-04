using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//控制游戏的状态/管理游戏
public class GameSystem : Singleton<GameSystem>
{
    private Transform mainCamera;
    public GameState currentGameState;//存储当前游戏状态
    public void Awake()
    {
        base.Awake();
        InitGameSystem();
        //游戏开始加载ab包

    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            AssetBundle.LoadFromFile(Application.dataPath + "/StreamingAssets/models.u3d");
        }
    }
    /// <summary>
    /// 进行初始化游戏
    /// </summary>
    private void InitGameSystem()
    {
        currentGameState = GameState.Observer;
        mainCamera = Camera.main.transform;
    }
    /// <summary>
    /// 进入内部
    /// </summary>
    public void EnterInner()
    {
        //关闭所有门窗
        //关闭控制动画按钮
    }
    /// <summary>
    /// 关闭所有门窗
    /// </summary>
    public void CloseAnimator()
    {
        for (int i = 0; i < AnimatorManager.instance.ans.Length; i++)
        {
            if (i == 9 || i == 10)
            {
                continue;
            }
            if (AnimatorManager.instance.ans[i] != null)
            {
                AnimatorManager.instance.ans[i].SetBool("isOpen",false);
            }
        }
        AnimatorManager.instance.ans[4].SetBool("smallDoorOpen",false);
    }

    /// <summary>
    /// 关闭控制动画按钮
    /// </summary>
    public void CloseCtrlButton()
    {
        MenuManager.instance.uis[1].gameObject.SetActive(false);
    }
    /// <summary>
    /// 打开控制动画按钮
    /// </summary>
    public void OpenCtrlButton()
    {
        MenuManager.instance.uis[1].gameObject.SetActive(true);
    }
    /// <summary>
    /// 更改游戏状态
    /// </summary>
    /// <param name="index"></param>
    public void ChangeGameState(int state)
    {
        currentGameState = (GameState)state;
        if (state == 0)
        {
            OpenCtrlButton();
        }
        else
        {
            CloseCtrlButton();
            CloseAnimator();
        }
        if (state >= 7)
        {
            mainCamera.gameObject.SetActive(false);
            MenuManager.instance.uis[4].gameObject.SetActive(true);
        }
        else
        {
            mainCamera.gameObject.SetActive(true);
            MenuManager.instance.uis[2].gameObject.SetActive(true);
            MenuManager.instance.uis[3].gameObject.SetActive(false);
            MenuManager.instance.uis[4].gameObject.SetActive(false);
        }
        CameraManager.instance.UseCamera((int)state);//使用对应的摄像机
    }
}
public enum GameState
{
    Observer,//观察者
    Wheel,//车轮
    FrontFace,//前脸
    Floor,//地板
    InteriorDecoration,//内饰
    OverheadLight,//顶灯
    Headlights,//车灯
    InnerCamera//内部观看
}
