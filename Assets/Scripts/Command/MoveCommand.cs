using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCommand : ICommand //接口要有实现
{
    Transform target;
    InputBuffer buffer;

    public MoveCommand(Transform target, InputBuffer buffer)
    {
        this.target = target;
        this.buffer = buffer;
    }

    public void Execute()
    {
        MoveBase.PlacePos(target, buffer);
    }

    public void Undo()
    {
        MoveBase.RemovePos(target, buffer);
    }

    public override string ToString()
    {
        return buffer.ToString();
    }
}
