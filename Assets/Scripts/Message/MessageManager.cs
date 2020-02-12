using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class MessageManager : Singleton<MessageManager>
{
    public static Dictionary<byte, List<Action<BaseMessage>>> _msgHandlerMap = new Dictionary<byte, List<Action<BaseMessage>>>(); // <消息类型, 动作委托列表>
    public List<BaseMessage> MsgBuffer = new List<BaseMessage>(); // 临时变量
    public Dictionary<int, MessageQueue> FrameMsgs = new Dictionary<int, MessageQueue>();
    public Dictionary<int, MessageQueue> WaitMsgs = new Dictionary<int, MessageQueue>();
    private int _tryTime = 0;

    // 注册消息
    public void RegisteMessage(byte type, Action<BaseMessage> action)
    {
        List<Action<BaseMessage>> actions;
        if (_msgHandlerMap.ContainsKey(type))
        {
            actions = _msgHandlerMap[type];
            actions.Add(action);
        }
        else
        {
            actions = new List<Action<BaseMessage>>();
            actions.Add(action);
            _msgHandlerMap.Add(type, actions);
        }
    }

    // 按消息号查找该消息所有要处理的动作
    public List<Action<BaseMessage>> GetActions(byte type)
    {
        if (_msgHandlerMap.ContainsKey(type))
        {
            return _msgHandlerMap[type];
        }
        Debug.LogError("GetActions error");
        return null;
    }

    // 得到某一帧消息
    public MessageQueue GetMessages(int frameIdx)
    {
        if (WaitMsgs.ContainsKey(frameIdx))
        {
            return WaitMsgs[frameIdx];
        }
        else
        {
            // 等待队列中不存在，所以是本地慢了
            //if (LockStepManager.Instance.frameIdx < GameServer.Instance.frameIdx)
            //{
            //    _tryTime++;
            //    if (_tryTime >= 3)
            //    {
            //        MessageRequest.Instance.RequestMessageQueueAtFrame(frameIdx); // 超时，主动再次请求
            //        _tryTime = 0;
            //    }
            //}

            // 要从服务器获取，或者从模拟服务器数据的类
            // 接收 LockStepManager.SendInput 的结果
            // 最终目的是 WaitMsgs.Add() 一个消息
        }
        //Debug.LogError("GetMessages error");
        return null;
    }

    // 处理消息
    public void HandleMessage(BaseMessage msg)
    {
        List<Action<BaseMessage>> actions = GetActions(msg.Type);

        if (actions == null)
        {
            Debug.Log("Action null");
        }
        else
        {
            for (int i = 0; i < actions.Count; i++)
            {
                actions[i](msg); //ColorMessage, MoveMessge等执行内部实现，通过委托监听在GameActionManager中执行
            }
        }
    }

    // 完成消息队列处理
    public void CompleteMessageQueue(int frameIdx)
    {
        RemoveWaitMessage(frameIdx);
    }

    // 移除等待消息
    public void RemoveWaitMessage(int frameIdx)
    {
        if (WaitMsgs.ContainsKey(frameIdx))
        {
            WaitMsgs.Remove(frameIdx);
        }
    }

    #region 序列化

    // 序列化消息队列
    public byte[] SerializeObj<T>(T obj)
    {
        IFormatter formatter = new BinaryFormatter();
        MemoryStream stream = new MemoryStream();
        formatter.Serialize(stream, obj);
        stream.Position = 0;
        byte[] buffer = new byte[stream.Length];
        stream.Read(buffer, 0, buffer.Length);
        stream.Flush();
        stream.Close();
        return buffer;
    }

    // 反序列化消息队列
    public T DesrializeObj<T>(byte[] bytes)
    {
        IFormatter formatter = new BinaryFormatter();
        MemoryStream stream = new MemoryStream(bytes);
        T obj = (T)formatter.Deserialize(stream);
        stream.Flush();
        stream.Close();
        return obj;
    }

    #endregion
}