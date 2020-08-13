using UnityEngine;

public class InputBuffer
{
    public int UID;
    public int Tick;
    public bool W;
    public bool S;
    public bool A;
    public bool D;

    public override string ToString()
    {
        return $"Input：\t{(W ? "W" : "")}\t{(S ? "S" : "")}\t{(A ? "A" : "")}\t{(D ? "D" : "")}";
    }
}

public class InputRender
{
    public int Tick;
    public Vector3 position;
    public bool Jump;
}

public enum Direction
{
    Left = -1,
    Error = 0, //重叠
    Right = 1,
}

public enum Body
{
    Free = 0, //没有碰撞，自由行动
    Fixed = 1, //碰到固定物体，墙等，不可往该方向移动
    Pushable = 2, //可碰撞物体，可以行动，但受到阻力
    Pushed = 3, //对方也在推，不可向该方向移动
}

public enum MotionStatus
{
    Idle,
    MoveForward,
    MoveBackward,
    Jump,
    JumpForward,
    JumpBackward,
    Crouch,
    Attack,
    AirAttack,
    Defend,
}
