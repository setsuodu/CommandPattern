using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//影子物体（没有插值的）
public class MoveBase : MonoBehaviour
{
    public int uid = 0;
    public Transform child;//自身
    public MoveBase rival;//对手
    [SerializeField] protected Direction direction = Direction.Error;
    [SerializeField] protected Body body;
    [SerializeField] protected MotionStatus status;
    public static int IdleHash = 2081823275;
    public static int MoveHash = -281135240;
    public static int JumpHash = 608663733;
    public static int FallDownHash = 426567206;
    public static int LandHash = 137525990;
    public static int CrouchHash = 70061904;
    public static int PunchHash = -802095957;
    public static int KickHash = -1916918463;

    public int Tick;

    public float _lerpTime;
    public static Queue<InputRender> buffers;
    public InputRender frameBuffer;
    public float distance; //右减左

    protected static float MOVE_SPEED = 0.1f;
    protected const float JUMP_HEIGHT = 1.5f;
    protected const float GRAVITY = -0.25f;//-9.81f;
    public static Vector3 playerVelocity;
    protected static Vector3 POS_SIZE = new Vector3(0.1f, 1.6f, 0.5f);
    protected static Vector3 NEG_SIZE = new Vector3(-0.1f, 1.6f, 0.5f);

    public virtual void Awake()
    {
        child = transform.GetChild(0);
    }

    public virtual void Update() { }

    public virtual void FixedUpdate() { }

    protected virtual bool OnGround()
    {
        return transform.position.y <= 0;
    }

    // 跳跃
    protected virtual bool W()
    {
        return Input.GetKey(KeyCode.W);
    }
    protected virtual bool _Jump()
    {
        bool value = Input.GetKey(KeyCode.W) && OnGround();
        return value;
    }

    // 蹲下
    protected virtual bool S()
    {
        return Input.GetKey(KeyCode.S);
    }
    protected virtual bool _Crouch()
    {
        bool value = Input.GetKey(KeyCode.S) && OnGround();
        return value;
    }

    protected virtual bool A()
    {
        return Input.GetKey(KeyCode.A);
    }
    protected virtual bool _Left()
    {
        return (Input.GetAxisRaw("Horizontal") < 0)
            && OnGround()
            && !_Crouch();
    }

    protected virtual bool D()
    {
        return Input.GetKey(KeyCode.D);
    }
    protected virtual bool _Right()
    {
        return (Input.GetAxisRaw("Horizontal") > 0)
            && OnGround()
            && !_Crouch();
    }

    protected virtual bool _JumpLeft()
    {
        return Input.GetKey(KeyCode.W)
            //&& animator.GetCurrentAnimatorStateInfo(0).IsName("Idle")
            && (Input.GetAxisRaw("Horizontal") < 0)
            && OnGround();
    }

    protected virtual bool _JumpRight()
    {
        return Input.GetKey(KeyCode.W)
            //&& animator.GetCurrentAnimatorStateInfo(0).IsName("Idle")
            && (Input.GetAxisRaw("Horizontal") > 0)
            && OnGround();
    }

    // 轻拳
    protected virtual bool _Punch0()
    {
        bool value = Input.GetKeyDown(KeyCode.H);
        return value;
    }

    // 中拳
    protected virtual bool _Punch1()
    {
        bool value = Input.GetKeyDown(KeyCode.J);
        return value;
    }

    // 重拳
    protected virtual bool _Punch2()
    {
        bool value = Input.GetKeyDown(KeyCode.K);
        return value;
    }

    // 轻踢
    protected virtual bool _Kick0()
    {
        bool value = Input.GetKeyDown(KeyCode.Y);
        return value;
    }

    // 这个踢
    protected virtual bool _Kick1()
    {
        bool value = Input.GetKeyDown(KeyCode.U);
        return value;
    }

    // 重踢
    protected virtual bool _Kick2()
    {
        bool value = Input.GetKeyDown(KeyCode.I);
        return value;
    }

    // 被另一个Player推动（只有一方静止，并且没有靠墙，才能被推动）
    public void ForceMove(Vector3 move)
    {
        if (move.magnitude > 1f)
            move.Normalize();
        Debug.Log("ForceMove: " + move);
        transform.Translate(move * 0.1f);
    }

    // 主动施加作用力
    public void Push(Vector3 move)
    {
        rival.ForceMove(move);
    }

    //20f（收到消息解析，同步影子玩家）
    public void PlacePos(Transform target, InputBuffer buffer)
    {
        //非本体操作数据
        //if (transform.name.Contains("2"))
        //    return;

        float y = (buffer.W ? MOVE_SPEED : 0) + (buffer.S ? -MOVE_SPEED : 0);
        float z = (buffer.D ? MOVE_SPEED : 0) + (buffer.A ? -MOVE_SPEED : 0);
        //Debug.Log($"(0,{y},{z})");

        //移动
        Vector3 position = Vector3.zero;
        if (OnGround() && y == 0)
        {
            position = new Vector3(0, 0, z);
        }

        if (OnGround() && playerVelocity.y < 0)
        {
            playerVelocity.y = 0;
            playerVelocity.z = 0;
        }

        //跳跃
        if (OnGround() && y > 0)
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
                playerVelocity.z += 0.4f;
            }
            else if (z < 0)
            {
                //向后跳跃
                playerVelocity.y += Mathf.Sqrt(JUMP_HEIGHT * -3.0f * GRAVITY);
                playerVelocity.z -= 0.4f;
            }
        }

        //衰减
        playerVelocity.y += GRAVITY;
        if (OnGround() == false)
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


        //PushBox计算
        if (rival.transform.position.z > pos.z)
        {
            //自己在左，对手在右
            direction = Direction.Right;
            child.localScale = POS_SIZE;
            child.rotation = new Quaternion(0, 0, 0, 0);

            if (z > 0 && OnGround() && y ==0)
            {
                status = MotionStatus.MoveForward;
            }
            else if (z < 0 && OnGround() && y == 0)
            {
                status = MotionStatus.MoveBackward;
            }
            else if (z == 0 && OnGround() && y == 0)
            {
                status = MotionStatus.Idle;
            }
            else if (!OnGround())
            {
                // 向前跳碰撞
                status = MotionStatus.Jump;
            }
            else
            {
                //Debug.Log($"h={h} && hash={animator.GetCurrentAnimatorStateInfo(0).shortNameHash}");
            }

            distance = rival.transform.position.z - pos.z;
            if (distance < 0.5f)
            {
                if ((status == MotionStatus.MoveForward || status == MotionStatus.JumpForward) && z > 0)
                {
                    Push(pos);
                }
                else if (status == MotionStatus.Idle && rival.OnGround())
                {
                    //pos += new Vector3(0, 0, -(0.5f - distance));
                }
            }
        }
        else if (rival.transform.position.z < pos.z)
        {
            //对手在左，自己在右
            direction = Direction.Left;
            child.localScale = NEG_SIZE;
            child.rotation = new Quaternion(0, 1, 0, 0);

            if (z > 0 && OnGround() && y == 0)
            {
                status = MotionStatus.MoveForward;
            }
            else if (z < 0 && OnGround() && y == 0)
            {
                status = MotionStatus.MoveBackward;
            }
            else if (z == 0 && OnGround() && y == 0)
            {
                status = MotionStatus.Idle;
            }
            else if (!OnGround())
            {
                // 向前跳碰撞
                status = MotionStatus.Jump;
            }
            else
            {
                //Debug.Log($"h={h} && hash={animator.GetCurrentAnimatorStateInfo(0).shortNameHash}");
            }

            distance = pos.z - rival.transform.position.z;
            if (distance < 0.5f)
            {
                if ((status == MotionStatus.MoveForward || status == MotionStatus.JumpForward) && z < 0)
                {
                    Push(pos);//移动中推动
                }
                else if (status == MotionStatus.Idle && rival.OnGround())
                {
                    //pos += new Vector3(0, 0, (0.5f - distance));//降落中推动
                }
            }
        }
        else
        {
            direction = Direction.Error;
        }


        InputRender render = new InputRender();
        render.Tick = buffer.Tick;
        render.position = pos;
        buffers.Enqueue(render);
    }

    public void RemovePos(Transform target, InputBuffer buffer)
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
