using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameActionManager : MonoBehaviour
{
    void Awake()
    {
        RegisteActionHanler();
    }

    void RegisteActionHanler()
    {
        Debug.Log("RegisteActionHanler");
        MessageManager.Instance.RegisteMessage(MessageType.MOVE_MSG, OnMoveMessageReceived);
        MessageManager.Instance.RegisteMessage(MessageType.COLOR_MSG, OnColorMessageReceived);
        MessageManager.Instance.RegisteMessage(MessageType.JUMP_MSG, OnJumpMessageReceived);
    }

    #region 解析服务器消息

    void OnMoveMessageReceived(BaseMessage msg)
    {
        MoveMessage moveMsg = msg as MoveMessage;
        uint playerId = moveMsg.PlayerId;
        Debug.Log("OnMoveMessageReceived：" + playerId);
        PlayerController player = PlayerInput.Instance.GetComponent<PlayerController>();
        if (player != null)
        {
            player.OnMoveMsgReceived(moveMsg);
        }
    }

    void OnJumpMessageReceived(BaseMessage msg)
    {
        JumpMessage jumpMsg = msg as JumpMessage;
        uint playerId = jumpMsg.PlayerId;
        Debug.Log("OnJumpMessageReceived：" + playerId);
        PlayerController player = PlayerInput.Instance.GetComponent<PlayerController>();
        if (player != null)
        {
            player.OnJumpMsgReceived(jumpMsg);
        }
    }

    void OnColorMessageReceived(BaseMessage msg)
    {
        ColorMessage colorMsg = msg as ColorMessage;
        uint playerId = colorMsg.PlayerId;
        Debug.Log("OnColorMessageReceived：" + playerId);
        PlayerController player = PlayerInput.Instance.GetComponent<PlayerController>();
        if (player != null)
        {
            player.OnColorMsgReceived(colorMsg);
        }
    }

    #endregion
}
