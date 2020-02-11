using System;
using UnityEngine;

[Serializable]
public class ColorMessage : BaseMessage
{
    public float R;
    public float G;
    public float B;

    public ColorMessage(uint playerId, Color color) : this(playerId, color.r, color.g, color.b) { }
    public ColorMessage(uint playerId, float r, float g, float b)
    {
        Type = MessageType.COLOR_MSG;
        this.PlayerId = playerId;
        this.R = r;
        this.G = g;
        this.B = b;
    }
}
