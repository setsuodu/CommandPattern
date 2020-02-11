using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 
 * 模拟从服务器返回结果
 */
public class SimServer : MonoBehaviour
{
    public static SimServer Instance;

    void Awake()
    {
        Instance = this;
    }

    public void Send(long frame) 
    {
        ReturnMessage(frame);
    }

    public void ReturnMessage(long frameIdx)
    {
        BaseMessage[] msgs = MessageManager.Instance.MsgBuffer.ToArray();
        MessageQueue msgQueue = new MessageQueue(frameIdx, msgs);
        MessageManager.Instance.MsgBuffer.Clear();

        if (!MessageManager.Instance.FrameMsgs.ContainsKey(frameIdx))
        {
            MessageManager.Instance.FrameMsgs.Add(frameIdx, msgQueue);
        }
        MessageManager.Instance.WaitMsgs.Add(msgQueue.frameIdx, msgQueue);
    }
}
