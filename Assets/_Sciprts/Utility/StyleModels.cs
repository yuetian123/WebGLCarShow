using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ��ʽģ�����
/// </summary>
public class StyleModels : MonoBehaviour
{
    public int length;//��ǰ�ɸ�������ʽ����
    protected int currentIndex;//��ǰ���ڵ���ʽ

    /// <summary>
    /// ��һ����ʽ
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
    /// ��һ����ʽ
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
    /// ָ���±���ʽ
    /// </summary>
    /// <param name="index"></param>
    public virtual void ChangeStyle(int index)
    {

    }

}
