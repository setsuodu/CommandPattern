using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 模拟从服务器返回结果
public class SimServer : MonoBehaviour
{
    public static SimServer Instance;

    void Awake()
    {
        Instance = this;
    }

    public void Send(int frame) 
    {
        ReturnMessage(frame);
    }

    public void ReturnMessage(int frameIdx)
    {
        BaseMessage[] buffer = MessageManager.Instance.MsgBuffer.ToArray();
        MessageQueue msgQueue = new MessageQueue(frameIdx, buffer);
        MessageManager.Instance.MsgBuffer.Clear();

        if (!MessageManager.Instance.FrameMsgs.ContainsKey(frameIdx))
        {
            MessageManager.Instance.FrameMsgs.Add(frameIdx, msgQueue);
        }
        MessageManager.Instance.WaitMsgs.Add(msgQueue.frameIdx, msgQueue);
    }
}
