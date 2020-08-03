using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 命令模式
public class InputSend : MonoBehaviour
{
    public bool IsStart;
    public int Tick;

    public static bool W()
    {
        return Input.GetKey(KeyCode.W);
    }

    public static bool S()
    {
        return Input.GetKey(KeyCode.S);
    }

    public static bool A()
    {
        return Input.GetKey(KeyCode.A);
    }

    public static bool D()
    {
        return Input.GetKey(KeyCode.D);
    }

    //10f
    void FixedUpdate()
    {
        if (!IsStart) 
            return;

        Tick++;

        InputBuffer buffer = new InputBuffer();
        buffer.Tick = this.Tick;
        buffer.W = W();
        buffer.S = S();
        buffer.A = A();
        buffer.D = D();
        ICommand command = new MoveCommand(transform, buffer);
        CommandInvoker.AddCommand(command);
    }
}

public class InputBuffer
{
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
