using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCommand : ICommand //接口要有实现
{
    Vector3 position;
    Transform cube;

    public MoveCommand(Vector3 position, Transform cube) 
    {
        this.position = position;
        this.cube = cube;
    }

    public void Execute()
    {
        InputFrame.PlaceCube(position, cube);
    }

    public void Undo()
    {
        InputFrame.RemoveCube(position, cube);
    }

    public override string ToString()
    {
        return "PlaceCube\t" + position.x + ":" + position.y + ":" + position.z;
    }
}
