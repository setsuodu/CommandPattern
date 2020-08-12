using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCommand : ICommand //接口要有实现
{
    MoveBase target;
    InputBuffer buffer;

    public MoveCommand(MoveBase target, InputBuffer buffer)
    {
        this.target = target;
        this.buffer = buffer;
    }

    public void Execute()
    {
        target.PlacePos(target.transform, buffer);
    }

    public void Undo()
    {
        target.RemovePos(target.transform, buffer);
    }

    public override string ToString()
    {
        return buffer.ToString();
    }
}
