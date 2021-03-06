﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player1 : MoveBase
{
    public override void Awake()
    {
        base.uid = 1;
        buffers = new Queue<InputRender>();
        base.Awake();
    }

    //60f，不稳定
    public override void Update()
    {
        //base.Update();

        OnGround();

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
    public override void FixedUpdate()
    {
        //base.FixedUpdate();

        if (!GameManager.Instance.IsStart)
            return;

        Tick++;

        //模拟发送
        InputBuffer buffer = new InputBuffer();
        buffer.UID = 1;
        buffer.Tick = this.Tick;
        buffer.W = W();
        buffer.S = S();
        buffer.A = A();
        buffer.D = D();

        //模拟解析
        ICommand command = new MoveCommand(this, buffer);
        CommandInvoker.AddCommand(command);
    }
}
