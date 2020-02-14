using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 挂在玩家身上（Player实例）
public class MessageRequest : MonoBehaviour
{
    public static MessageRequest Instance;

    void Awake()
    {
        Instance = this;
    }

    #region Client

    public void SendMessage(BaseMessage message)
    {
        CmdSendMessage(MessageManager.Instance.SerializeObj<BaseMessage>(message));
    }

    public void RpcReturnMessage(byte[] bytes)
    {
        MessageQueue msgQueue = MessageManager.Instance.DesrializeObj<MessageQueue>(bytes);

        if (msgQueue != null && !MessageManager.Instance.WaitMsgs.ContainsKey(msgQueue.frameIdx))
        {
            MessageManager.Instance.WaitMsgs.Add(msgQueue.frameIdx, msgQueue);
        }
    }

    // 请求某一帧的消息队列
    public void RequestMessageQueueAtFrame(int frameIdx)
    {
        ReturnMessageAtFrame(frameIdx);
    }
    
    #endregion

    #region Server

    public void CmdSendMessage(byte[] bytes)
    {
        ReceiveMessage(bytes); //TODO: 重写
    }

    public void ReceiveMessage(byte[] bytes)
    {
        MessageManager.Instance.MsgBuffer.Add(MessageManager.Instance.DesrializeObj<BaseMessage>(bytes));
    }

    // 客户端接收到服务器消息
    public void ReturnMessage(int frameIdx)
    {
        BaseMessage[] msgs = MessageManager.Instance.MsgBuffer.ToArray();
        MessageQueue msgQueue = new MessageQueue(frameIdx, msgs);

        MessageManager.Instance.MsgBuffer.Clear();
        if (!MessageManager.Instance.FrameMsgs.ContainsKey(frameIdx))
        {
            MessageManager.Instance.FrameMsgs.Add(frameIdx, msgQueue);
        }
        RpcReturnMessage(MessageManager.Instance.SerializeObj<MessageQueue>(msgQueue));
    }

    // 查询方法
    public void ReturnMessageAtFrame(int frameIdx)
    {
        MessageQueue msgQueue = null;
        if (MessageManager.Instance.FrameMsgs.ContainsKey(frameIdx))
        {
            msgQueue = MessageManager.Instance.FrameMsgs[frameIdx];
        }
        RpcReturnMessage(MessageManager.Instance.SerializeObj<MessageQueue>(msgQueue));
    }

    #endregion
}
