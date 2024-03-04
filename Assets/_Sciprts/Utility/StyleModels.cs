using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 样式模块基类
/// </summary>
public class StyleModels : MonoBehaviour
{
    public int length;//当前可更换的样式长度
    protected int currentIndex;//当前所在的样式

    /// <summary>
    /// 下一个样式
    /// </summary>
    public void NextStyle()
    {
        int detection = currentIndex + 1;
        if (detection >= length)
        {
            currentIndex = 0;
        }
        else
        {
            currentIndex = detection;
        }
        ChangeStyle(currentIndex);
    }
    /// <summary>
    /// 上一个样式
    /// </summary>
    public void LastStyle()
    {
        int detection = currentIndex - 1;
        if (detection < 0)
        {
            currentIndex = length - 1;
        }
        else
        {
            currentIndex = detection;
        }
        ChangeStyle(currentIndex);
    }
    /// <summary>
    /// 指定下标样式
    /// </summary>
    /// <param name="index"></param>
    public virtual void ChangeStyle(int index)
    {

    }

}
