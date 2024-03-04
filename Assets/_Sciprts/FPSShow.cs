using UnityEngine;
using System.Collections;

[AddComponentMenu("Utilities/HUDFPS")]
public class FPSShow : MonoBehaviour
{
    //fps ��ʾ�ĳ�ʼλ�úʹ�С
    public Rect startRect = new Rect(512, 10f, 75f, 50f);
    //fps ����ʱ�Ƿ�ı�UI��ɫ
    public bool updateColor = true;
    //fps UI �Ƿ������϶� 
    public bool allowDrag = true;
    //fps ���µ�Ƶ��
    public float frequency = 0.5F;
    //fps ��ʾ�ľ���
    public int nbDecimal = 1;
    //һ��ʱ���ڵ�fps����
    private float accum = 0f;
    //fps�����ʱ��
    private int frames = 0;
    //GUI ����fps����ɫ fps<10 ��ɫ fps<30 ��ɫ fps>=30 ��ɫ
    private Color color = Color.white;
    //fps 
    private string sFPS = "";
    //GUI ����ʽ
    private GUIStyle style;

    void Start()
    {
        StartCoroutine(FPS());
    }

    void Update()
    {
        accum += Time.timeScale / Time.deltaTime;
        ++frames;
    }

    IEnumerator FPS()
    {
        while (true)
        {
            //����fps
            float fps = accum / frames;
            sFPS = fps.ToString("f" + Mathf.Clamp(nbDecimal, 0, 10));

            //������ɫ
            color = (fps >= 30) ? Color.green : ((fps > 10) ? Color.yellow : Color.red);

            accum = 0.0F;
            frames = 0;

            yield return new WaitForSeconds(frequency);
        }
    }

    void OnGUI()
    {
        if (style == null)
        {
            style = new GUIStyle(GUI.skin.label);
            style.normal.textColor = Color.white;
            style.alignment = TextAnchor.MiddleCenter;
        }

        GUI.color = updateColor ? color : Color.white;
        startRect = GUI.Window(0, startRect, DoMyWindow, "");
    }

    void DoMyWindow(int windowID)
    {
        GUI.Label(new Rect(0, 0, startRect.width, startRect.height), sFPS + " FPS", style);
        if (allowDrag) GUI.DragWindow(new Rect(0, 0, Screen.width, Screen.height));
    }
}