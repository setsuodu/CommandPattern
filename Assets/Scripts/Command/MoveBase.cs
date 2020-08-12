using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//影子物体（没有插值的）
public class MoveBase : MonoBehaviour
{
    public int Tick;

    public float _lerpTime;
    static Queue<InputRender> buffers;
    InputRender frameBuffer;

    protected static float MOVE_SPEED = 0.2f;
    protected const float JUMP_HEIGHT = 2.0f;
    protected const float GRAVITY = -0.25f;//-9.81f;
    public static Vector3 playerVelocity;

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

    //20f（每帧发送按键输入）
    void FixedUpdate()
    {
        if (!GameManager.Instance.IsStart)
            return;

        Tick++;

        //模拟发送
        InputBuffer buffer = new InputBuffer();
        buffer.Tick = this.Tick;
        buffer.W = W();
        buffer.S = S();
        buffer.A = A();
        buffer.D = D();

        //模拟解析
        ICommand command = new MoveCommand(transform, buffer);
        CommandInvoker.AddCommand(command);
    }

    protected static bool W()
    {
        return Input.GetKey(KeyCode.W);
    }

    protected static bool S()
    {
        return Input.GetKey(KeyCode.S);
    }

    protected static bool A()
    {
        return Input.GetKey(KeyCode.A);
    }

    protected static bool D()
    {
        return Input.GetKey(KeyCode.D);
    }

    protected static bool OnGround(Vector3 pos)
    {
        return pos.y <= 0;
    }

    //protected virtual bool _Crouch()
    //{
    //    bool value = Input.GetKey(KeyCode.S) && OnGround();
    //    return value;
    //}

    //20f（收到消息解析，同步影子玩家）
    public static void PlacePos(Transform target, InputBuffer buffer)
    {
        float y = (buffer.W ? MOVE_SPEED : 0) + (buffer.S ? -MOVE_SPEED : 0);
        float z = (buffer.D ? MOVE_SPEED : 0) + (buffer.A ? -MOVE_SPEED : 0);

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
                playerVelocity.y += Mathf.Sqrt(JUMP_HEIGHT * -3.0f * GRAVITY);//平方根
            }
            else if (z > 0)
            {
                //向前跳跃
                playerVelocity.y += Mathf.Sqrt(JUMP_HEIGHT * -3.0f * GRAVITY);
                playerVelocity.z += 0.5f;
            }
            else if (z < 0)
            {
                //向后跳跃
                playerVelocity.y += Mathf.Sqrt(JUMP_HEIGHT * -3.0f * GRAVITY);
                playerVelocity.z -= 0.5f;
            }
        }

        //衰减
        playerVelocity.y += GRAVITY;
        if (OnGround(target.position) == false)
        {
            //在空中
            if (playerVelocity.z > 0)
            {
                playerVelocity.z -= 0.05f;
            }
            else if (playerVelocity.z < 0)
            {
                playerVelocity.z += 0.05f;
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
