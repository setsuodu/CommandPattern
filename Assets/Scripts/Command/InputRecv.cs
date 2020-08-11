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

    public AnimationCurve cJump;
    public const float jumpHeight = 3.0f;
    public const float gravityValue = -0.25f;//-9.81f;
    public static Vector3 playerVelocity;

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
        return pos.y <= 0;
    }

    //10f
    public static void PlacePos(Transform target, InputBuffer buffer)
    {
        float y = (buffer.W ? 1f : 0) + (buffer.S ? -1f : 0);
        float z = (buffer.D ? 1f : 0) + (buffer.A ? -1f : 0);

        //移动
        Vector3 position = Vector3.zero;
        if (OnGround(target.position) && y == 0)
        {
            position = new Vector3(0, 0, z);
        }

        if (OnGround(target.position) && playerVelocity.y < 0)
        {
            playerVelocity.y = 0;
            playerVelocity.z = 0;
        }

        //跳跃
        if (OnGround(target.position) && y > 0)
        {
            if (z == 0)
            {
                //原地跳跃
                playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);//平方根
            }
            else if (z > 0)
            {
                //向前跳跃
                playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
                //position += new Vector3(0, 0, 3.0f);

                playerVelocity.z += 1f;
            }
            else if (z < 0)
            {
                //向后跳跃
                playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
                //position += new Vector3(0, 0, -3.0f);

                playerVelocity.z -= 1f;
            }
        }

        //衰减
        playerVelocity.y += gravityValue;
        if (OnGround(target.position) == false)
        {
            //在空中
            if (playerVelocity.z > 0)
            {
                playerVelocity.z -= 0.1f;
            }
            else if (playerVelocity.z < 0)
            {
                playerVelocity.z += 0.1f;
            }
        }
        position += playerVelocity;

        Vector3 pos = target.position + position;
        pos.y = Mathf.Clamp(pos.y, 0, pos.y);

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
