using System;
using System.Collections.Generic;

[Serializable]
public class MessageQueue
{
    public int frameIdx;
    public BaseMessage[] messages;

    public MessageQueue() { } // 这个构造函数必须写，不然无法解析
    public MessageQueue(int frameIdx, BaseMessage[] messages)
    {
        this.frameIdx = frameIdx;
        this.messages = messages;
    }
}
