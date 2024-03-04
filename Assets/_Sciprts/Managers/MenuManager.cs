using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 菜单管理类/负责管理按钮触发事件
/// </summary>
public class MenuManager : Singleton<MenuManager>
{
    public Transform[] uis;//存储menu ui
    /// <summary>
    /// 当按钮被按下
    /// </summary>
    public void OnButtonClick()
    {
        int currentGameState = (int)GameSystem.instance.currentGameState;
        if ( currentGameState == 0 || currentGameState == 7)
        {
            uis[0].gameObject.SetActive(false);
        }
        else if(!uis[0].gameObject.activeSelf)
        {
            uis[0].gameObject.SetActive(true);
        }
    }
    /// <summary>
    /// 上一个样式
    /// </summary>
    public void NextStyle()
    {
        Debug.Log("next");
        StyleModelsManager.instance.models[(int)GameSystem.instance.currentGameState].NextStyle();
    }
    /// <summary>
    /// 下一个样式
    /// </summary>
    public void LastStyle()
    {
        Debug.Log("last");
        StyleModelsManager.instance.models[(int)GameSystem.instance.currentGameState].LastStyle();
    }
}
