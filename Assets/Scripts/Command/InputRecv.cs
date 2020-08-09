using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//影子物体（没有插值的）
public class InputRecv : MonoBehaviour
{
    public InputSend m_InputSend;

    static Queue<InputRender> buffers;
    InputRender frameBuffer;

    public float m_Lerp; //前后两帧数据间隔时间（默认0.1s）

    private float AccumilatedTime = 0f;

    private float FrameLength = 0.02f; //50 miliseconds

    void Awake()
    {
        buffers = new Queue<InputRender>();
    }

    //60f，不稳定
    void Update()
    {
        if (buffers.Count > 0)
        {
            //新的帧
            frameBuffer = buffers.Dequeue();
        }

        if (frameBuffer == null)
            return;

        transform.position = frameBuffer.position;
    }

    //10f
    public static void PlacePos(Transform target, InputBuffer buffer)
    {
        float h = (buffer.D ? 1f : 0) + (buffer.A ? -1f : 0);
        Vector3 position = new Vector3(0, 0, h);
        Vector3 pos = target.position + position;
        InputRender render = new InputRender();
        render.position = pos;
        buffers.Enqueue(render);
    }

    public static void RemovePos(Transform target, InputBuffer buffer)
    {
        float h = (buffer.D ? 1f : 0) + (buffer.A ? -1f : 0);
        Vector3 position = new Vector3(0, 0, h);
        Vector3 pos = target.position - position;
        InputRender render = new InputRender();
        render.position = pos;
        buffers.Enqueue(render);
    }
}
