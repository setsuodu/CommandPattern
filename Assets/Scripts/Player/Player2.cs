using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2 : MoveBase
{
    void Start()
    {
        
    }

    void Update()
    {

    }

    protected static bool W()
    {
        return Input.GetKey(KeyCode.Keypad8);
    }

    protected static bool S()
    {
        return Input.GetKey(KeyCode.Keypad5);
    }

    protected static bool A()
    {
        return Input.GetKey(KeyCode.Keypad4);
    }

    protected static bool D()
    {
        return Input.GetKey(KeyCode.Keypad6);
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
}
