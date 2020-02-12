using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 玩家操作
public class MovementInput : MonoBehaviour
{
    uint PlayerId = 0;

    void Update()
    {
        if (!LockStepManager.Instance.IsStart) return;

        int _r = -1;
        int _g = -1;
        int _b = -1;
        if (GetKeyDown_1())
        {
            _r = Random.Range(0, 1000);
            _g = Random.Range(0, 1000);
            _b = Random.Range(0, 1000);
        }

        // 统一收集
        var CurGameInput = new InputClass()
        {
            horizontal = (int)(GetInputDirection().x * 1000),
            vertical = (int)(GetInputDirection().y * 1000),
            isJump = GetKeyDown_Space(),
            R = _r,
            G = _g,
            B = _b,
        };
        SendInputMessage(CurGameInput);
    }

    #region 输入按键

    private Vector2 GetInputDirection()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        return new Vector2(h, v);
    }

    private bool GetKeyDown_Space() 
    {
        return Input.GetKeyDown(KeyCode.Space);
    }

    private bool GetKeyDown_1()
    {
        return Input.GetKeyDown(KeyCode.Keypad1);
    }

    #endregion

    #region 发送消息

    private void SendInputMessage(InputClass inputs)
    {
        BaseMessage cmd = new BaseMessage(0, PlayerId, inputs);
        MessageRequest.Instance.SendMessage(cmd);
    }

    #endregion
}
