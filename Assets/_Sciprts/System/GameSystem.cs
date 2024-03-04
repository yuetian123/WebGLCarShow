using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//������Ϸ��״̬/������Ϸ
public class GameSystem : Singleton<GameSystem>
{
    private Transform mainCamera;
    public GameState currentGameState;//�洢��ǰ��Ϸ״̬
    public void Awake()
    {
        base.Awake();
        InitGameSystem();
        //��Ϸ��ʼ����ab��

    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            AssetBundle.LoadFromFile(Application.dataPath + "/StreamingAssets/models.u3d");
        }
    }
    /// <summary>
    /// ���г�ʼ����Ϸ
    /// </summary>
    private void InitGameSystem()
    {
        currentGameState = GameState.Observer;
        mainCamera = Camera.main.transform;
    }
    /// <summary>
    /// �����ڲ�
    /// </summary>
    public void EnterInner()
    {
        //�ر������Ŵ�
        //�رտ��ƶ�����ť
    }
    /// <summary>
    /// �ر������Ŵ�
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
    /// �رտ��ƶ�����ť
    /// </summary>
    public void CloseCtrlButton()
    {
        MenuManager.instance.uis[1].gameObject.SetActive(false);
    }
    /// <summary>
    /// �򿪿��ƶ�����ť
    /// </summary>
    public void OpenCtrlButton()
    {
        MenuManager.instance.uis[1].gameObject.SetActive(true);
    }
    /// <summary>
    /// ������Ϸ״̬
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
        CameraManager.instance.UseCamera((int)state);//ʹ�ö�Ӧ�������
    }
}
public enum GameState
{
    Observer,//�۲���
    Wheel,//����
    FrontFace,//ǰ��
    Floor,//�ذ�
    InteriorDecoration,//����
    OverheadLight,//����
    Headlights,//����
    InnerCamera//�ڲ��ۿ�
}
