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
        Debug.Log(LockStepManager.Instance.frameIdx + "  玩家" + message.PlayerId + "，发送二进制数据： " + message.Type); //一个frame可以包含多个Message
        CmdSendMessage(MessageManager.Instance.SerializeObj<BaseMessage>(message));
    }

    public void RpcReturnMessage(byte[] bytes)
    {
        MessageQueue msgQueue = MessageManager.Instance.DesrializeObj<MessageQueue>(bytes);
        //Debug.Log("Return: " + (msgQueue.frameIdx)); //TODO: Debug输出到Canvas

        if (msgQueue != null && !MessageManager.Instance.WaitMsgs.ContainsKey(msgQueue.frameIdx))
        {
            MessageManager.Instance.WaitMsgs.Add(msgQueue.frameIdx, msgQueue);
        }
    }

    // 请求某一帧的消息队列
    public void RequestMessageQueueAtFrame(long frameIdx)
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
    public void ReturnMessage(long frameIdx)
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
    public void ReturnMessageAtFrame(long frameIdx)
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
