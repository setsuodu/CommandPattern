using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//影子物体（没有插值的）
public class MoveBase : MonoBehaviour
{
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

    protected static float MOVE_SPEED = 0.2f;
    protected const float JUMP_HEIGHT = 2.0f;
    protected const float GRAVITY = -0.25f;//-9.81f;
    public static Vector3 playerVelocity;

    public virtual void Update() { }

    public virtual void FixedUpdate() { }

    protected virtual bool W()
    {
        return Input.GetKey(KeyCode.W);
    }

    protected virtual bool S()
    {
        return Input.GetKey(KeyCode.S);
    }

    protected virtual bool A()
    {
        return Input.GetKey(KeyCode.A);
    }

    protected virtual bool D()
    {
        return Input.GetKey(KeyCode.D);
    }

    protected virtual bool OnGround()
    {
        return transform.position.y <= 0;
    }

    protected virtual bool _Crouch()
    {
        bool value = Input.GetKey(KeyCode.S) && OnGround();
        return value;
    }

    //20f（收到消息解析，同步影子玩家）
    public void PlacePos(Transform target, InputBuffer buffer)
    {
        float y = (buffer.W ? MOVE_SPEED : 0) + (buffer.S ? -MOVE_SPEED : 0);
        float z = (buffer.D ? MOVE_SPEED : 0) + (buffer.A ? -MOVE_SPEED : 0);

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

        InputRender render = new InputRender();
        render.Tick = buffer.Tick;
        render.position = pos;
        buffers.Enqueue(render);


        if (rival.transform.position.x > transform.position.x)
        {

        }


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
