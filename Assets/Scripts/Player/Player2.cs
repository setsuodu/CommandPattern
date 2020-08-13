using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2 : MoveBase
{
    public override void Awake()
    {
        base.uid = 2;
        base.Awake();
    }

    public override void Update()
    {

    }

    protected override bool W()
    {
        return Input.GetKey(KeyCode.Keypad8);
    }

    protected override bool S()
    {
        return Input.GetKey(KeyCode.Keypad5);
    }

    protected override bool A()
    {
        return Input.GetKey(KeyCode.Keypad4);
    }

    protected override bool D()
    {
        return Input.GetKey(KeyCode.Keypad6);
    }
}
