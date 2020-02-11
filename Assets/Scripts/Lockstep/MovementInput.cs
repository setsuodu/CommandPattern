using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 玩家操作
public class MovementInput : MonoBehaviour
{
    uint PlayerId = 0;

    void Update()
    {
        Vector2 inputDir = GetInputDirection();
        if (inputDir != Vector2.zero)
        {
            SendMoveMessage(inputDir);
        }

        if (GetKeyDown_Space())
        {
            SendJumpMessage(true);
        }

        if (GetKeyDown_1())
        {
            Color color = new Color(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f));
            SendColorMessage(color);
        }
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

    private void SendMoveMessage(Vector2 dir)
    {
        MoveMessage cmd = new MoveMessage(PlayerId, dir);
        MessageRequest.Instance.SendMessage(cmd);
    }

    private void SendJumpMessage(bool jump)
    {
        JumpMessage cmd = new JumpMessage(PlayerId, jump);
        MessageRequest.Instance.SendMessage(cmd);
    }

    private void SendColorMessage(Color color)
    {
        ColorMessage cmd = new ColorMessage(PlayerId, color);
        MessageRequest.Instance.SendMessage(cmd);
    }

    #endregion
}
