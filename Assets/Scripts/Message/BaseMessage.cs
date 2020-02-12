using System;

[Serializable]
public class BaseMessage
{
    public byte Type;           // 消息号
    public uint PlayerId;       // 玩家Id
    public InputClass Inputs;   // 所有输入数据

    public BaseMessage() { }
    public BaseMessage(byte type, uint playerId, InputClass input) 
    {
        this.Type = type;
        this.PlayerId = playerId;
        this.Inputs = input;
    }
}

// 不需要派生类了，都归为InputClass的属性
// MoveMessage, JumpMessage, ColorMessage
