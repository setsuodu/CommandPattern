using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateFrame : MonoBehaviour
{
    const float fpsMeasurePeriod = 0.5f;        //FPS测量间隔
    private int m_FpsAccumulator = 0;           //帧数累计的数量
    private float m_FpsNextPeriod = 0;          //FPS下一段的间隔
    [SerializeField] private int m_CurrentFps;                   //当前的帧率

    void Awake()
    {
        m_FpsNextPeriod = Time.realtimeSinceStartup + fpsMeasurePeriod; // Time.realtimeSinceStartup获取游戏开始到当前的时间，增加一个测量间隔，计算出下一次帧率计算是要在什么时候
        //DontDestroyOnLoad(this.gameObject); //一直显示不销毁！最好放在第一个场景
    }

    void Update()
    {
        // 测量每一秒的平均帧率
        m_FpsAccumulator++;
        if (Time.realtimeSinceStartup > m_FpsNextPeriod)                    // 当前时间超过了下一次的计算时间
        {
            m_CurrentFps = (int)(m_FpsAccumulator / fpsMeasurePeriod);      // 计算
            m_FpsAccumulator = 0;                                           // 计数器归零
            m_FpsNextPeriod += fpsMeasurePeriod;                            // 在增加下一次的间隔
            //m_Text.text = string.Format(display, m_CurrentFps);             // 处理一下文字显示
        }
    }

    void OnGUI()
    {
        var style = new GUIStyle();
        style.fontSize = 30;
        string fps = m_CurrentFps + "fps\ntick: " + LockStepManager.Instance.frameIdx;
        GUI.Label(new Rect(30, 30, 200, 100), fps, style);
    }
}
