using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// �˵�������/�������ť�����¼�
/// </summary>
public class MenuManager : Singleton<MenuManager>
{
    public Transform[] uis;//�洢menu ui
    /// <summary>
    /// ����ť������
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
    /// ��һ����ʽ
    /// </summary>
    public void NextStyle()
    {
        Debug.Log("next");
        StyleModelsManager.instance.models[(int)GameSystem.instance.currentGameState].NextStyle();
    }
    /// <summary>
    /// ��һ����ʽ
    /// </summary>
    public void LastStyle()
    {
        Debug.Log("last");
        StyleModelsManager.instance.models[(int)GameSystem.instance.currentGameState].LastStyle();
    }
}
