using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputRecv : MonoBehaviour
{
    public InputSend m_InputSend;

    static Queue<InputRender> buffers;
    InputRender frameBuffer;

    void Awake()
    {
        buffers = new Queue<InputRender>();
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

        transform.position = frameBuffer.position;
    }

    //10f
    public static void PlacePos(Transform target, InputBuffer buffer)
    {
        int h = (buffer.D ? 1 : 0) + (buffer.A ? -1 : 0);
        Vector3 position = new Vector3(0, 0, h);
        Vector3 pos = target.position + position;
        InputRender render = new InputRender();
        render.position = pos;
        buffers.Enqueue(render);
    }

    public static void RemovePos(Transform target, InputBuffer buffer)
    {
        int h = (buffer.D ? 1 : 0) + (buffer.A ? -1 : 0);
        Vector3 position = new Vector3(0, 0, h);
        Vector3 pos = target.position - position;
        InputRender render = new InputRender();
        render.position = pos;
        buffers.Enqueue(render);
    }
}
