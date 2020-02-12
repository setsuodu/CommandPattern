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
        MessageManager.Instance.RegisteMessage(0, OnInputMessageReceived);
    }

    #region 解析服务器消息

    void OnInputMessageReceived(BaseMessage msg)
    {
        //Debug.Log(msg.Type);
        uint playerId = msg.PlayerId;

        PlayerController player = PlayerInput.Instance.GetComponent<PlayerController>(); //TODO: 改成从PlayerList中用Id找人
        if (player != null)
        {
            player.OnInputMsgReceived(msg);
        }
    }

    #endregion
}
