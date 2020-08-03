using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class InputRecv : MonoBehaviour
{
    const float JUMP_HEIGHT = 2f;
    const float GRAVITY = -30f;

    static Queue<InputBuffer> buffers;
    InputBuffer frameBuffer;
    float vy;

    void Awake()
    {
        buffers = new Queue<InputBuffer>();
        //frameBuffer = null;
        vy = 0;
    }

    //60f
    void Update()
    {
        if (buffers.Count > 0)
        {
            frameBuffer = buffers.Dequeue();
        }

        if (frameBuffer == null)
            return;

        int z = (frameBuffer.D ? 1 : 0) + (frameBuffer.A ? -1 : 0);
        //Vector3 newpos = transform.position + new Vector3(0, 0, z);
        //transform.position = Vector3.Lerp(transform.position, newpos, 0.1f);

        bool jump = frameBuffer.W;
        if (OnGround())
        {
            vy = 0;

            if (jump)
            {
                //velocity.y = Mathf.Sqrt(JUMP_HEIGHT * GRAVITY * -2f);
                vy = Mathf.Sqrt(5f);
            }
        }
        else
        {
            vy -= 10 * Time.deltaTime;
        }

        Vector3 newpos = transform.position + new Vector3(0, vy, z);
        Mathf.Clamp(newpos.y, 0, newpos.y);
        transform.position = Vector3.Lerp(transform.position, newpos, 0.1f);
    }

    public static void PlacePos(Transform target, InputBuffer buffer)
    {
        //int h = (buffer.D ? 1 : 0) + (buffer.A ? -1 : 0);
        //Vector3 position = new Vector3(0, 0, h);
        //Vector3 pos = target.position + position;
        //buffers.Enqueue(pos);
        buffers.Enqueue(buffer);
    }

    public static void RemovePos(Transform target, InputBuffer buffer)
    {
        //int h = (buffer.D ? 1 : 0) + (buffer.A ? -1 : 0);
        //Vector3 position = new Vector3(0, 0, h);
        //Vector3 pos = target.position - position;
        //buffers.Enqueue(pos);
        buffers.Enqueue(buffer);
    }

    public bool OnGround()
    {
        Vector3 start = transform.position + Vector3.up * 0.2f;
        Vector3 end = transform.position + Vector3.down * 0.2f;

        Debug.DrawLine(start, end, Color.red);
        return Physics.Raycast(start, end, 1f, 1 << 4);
    }
}
