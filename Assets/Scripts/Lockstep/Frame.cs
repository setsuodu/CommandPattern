using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frame : MonoBehaviour
{
    public bool isRecord; //开关
    public int FPS = 30;
    public int tick;

    private float tm;

    void Start()
    {
        Debug.Log("VSync: " + QualitySettings.vSyncCount + " | FPS: " + Application.targetFrameRate);
        Application.targetFrameRate = FPS;
    }

    void Update()
    {
        if (isRecord)
        {
            if (tm >= 1 / Application.targetFrameRate)
            {
                tm = 0;
                Step();
            }
            tm += Time.deltaTime;
        }
    }

    void Reset()
    {
        tick = 0;
        tm = 0;
    }

    // 用这个函数去推进客户端
    public void Step()
    {
        tick++;
        //Debug.LogFormat("<color=yellow>{0}</color>", Mathf.FloorToInt(Time.time));
        //Debug.Log("tick: " + tick);

        InputClass input = new InputClass()
        {
            tick    = (uint)this.tick,
            left    = PlayerInput.Instance.GetHorizontalValue() < 0,
            right   = PlayerInput.Instance.GetHorizontalValue() > 0,
            up      = PlayerInput.Instance.GetVerticalValue() > 0,
            down    = PlayerInput.Instance.GetVerticalValue() < 0,
            space   = PlayerInput.Instance.GetJumpValue(),
        };
        input.Echo();

        //TODO: 压进CommandQueue。操作对象从Queue中循环获取

    }
}
