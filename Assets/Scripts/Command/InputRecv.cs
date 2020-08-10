using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//影子物体（没有插值的）
public class InputRecv : MonoBehaviour
{
    public bool IsStart;
    public int Tick;
    public float _lerpTime;

    static Queue<InputRender> buffers;
    InputRender frameBuffer;
    InputRender lastFrameBuffer;
    bool m_IsJump;

    public AnimationCurve cJump;

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

    void Awake()
    {
        buffers = new Queue<InputRender>();
    }

    //60f，不稳定
    void Update()
    {
        OnGround(transform.position);

        if (buffers.Count > 0)
        {
            //新的帧
            frameBuffer = buffers.Dequeue();
            _lerpTime = 0;
        }

        if (frameBuffer == null)
            return;

        transform.position = frameBuffer.position;

        //if (lastFrameBuffer != null && lastFrameBuffer.position.y > 0)
        //{
        //    var pos = transform.position;
        //    pos.y = 0;
        //    transform.position = pos;
        //}

        //lastFrameBuffer = frameBuffer;
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

    protected static bool OnGround(Vector3 pos)
    {
        Ray ray = new Ray(pos + Vector3.up * 0.01f, Vector3.down);
        bool value = Physics.Raycast(ray, 0.02f, 1 << LayerMask.NameToLayer("Environment"));
        Debug.DrawLine(pos + Vector3.up * 0.01f, pos - Vector3.up * 0.01f, Color.red);
        return value;
    }

    //10f
    public static void PlacePos(Transform target, InputBuffer buffer)
    {
        //TODO: 跳跃中不允许移动
        float y = (buffer.W ? 1f : 0) + (buffer.S ? -1f : 0);
        float z = (buffer.D ? 1f : 0) + (buffer.A ? -1f : 0);

        //TODO: 6帧跳跃、6帧落地
        Vector3 position = Vector3.zero;
        if (y == 0)
        {
            position = new Vector3(0, 0, z);
        }
        else if (y > 0 && OnGround(target.position) == true)
        {
            position = new Vector3(0, 1, 0);
        }
        Vector3 pos = target.position + position;

        InputRender render = new InputRender();
        render.Tick = buffer.Tick;
        render.position = pos;
        buffers.Enqueue(render);
    }

    public static void RemovePos(Transform target, InputBuffer buffer)
    {
        float h = (buffer.D ? 1f : 0) + (buffer.A ? -1f : 0);

        Vector3 position = new Vector3(0, 0, h);
        Vector3 pos = target.position - position;

        InputRender render = new InputRender();
        render.Tick = buffer.Tick;
        render.position = pos;
        buffers.Enqueue(render);
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

public class InputRender
{
    public int Tick;
    public Vector3 position;
    public bool Jump;
}
