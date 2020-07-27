using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputFrame : MonoBehaviour
{
    static Queue<Vector3> buffers = new Queue<Vector3>();
    Vector3 buff;

    void Start()
    {
        Application.targetFrameRate = 60;

        buff = transform.position;
    }

    //10f
    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.D))
        {
            // 命令模
            Vector3 move = new Vector3(0, 0, 1);
            ICommand command = new MoveCommand(move, transform);
            CommandInvoker.AddCommand(command);
        }
        if (Input.GetKey(KeyCode.A))
        {
            // 命令模
            Vector3 move = new Vector3(0, 0, -1);
            ICommand command = new MoveCommand(move, transform);
            CommandInvoker.AddCommand(command);
        }
    }

    //60f
    void Update()
    {
        if (buffers.Count > 0)
        {
            buff = buffers.Dequeue();
        }
        transform.position = Vector3.Lerp(transform.position, buff, 0.1f);
    }

    public static void PlaceCube(Vector3 position, Transform cube)
    {
        Vector3 target = cube.position + position;
        buffers.Enqueue(target);
    }

    public static void RemoveCube(Vector3 position, Transform cube)
    {
        Vector3 target = cube.position - position;
        buffers.Enqueue(target);
    }
}
