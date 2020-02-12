using System;
using UnityEngine;
/*
[Serializable]
public class MoveMessage : BaseMessage
{
    public int H;
    public int V;
    
    public MoveMessage(uint playerId, Vector2 dir) : this(playerId, dir.x, dir.y) { }
    public MoveMessage(uint playerId, float x, float y)
    {
        Type = MessageType.MOVE_MSG;
        this.PlayerId = playerId;
        this.H = (int)(x * 1000); //float都按1000倍创建，使用时再除1000
        this.V = (int)(y * 1000);
    }
}
*/