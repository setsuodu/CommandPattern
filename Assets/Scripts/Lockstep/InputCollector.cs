using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 收集玩家操作
public class InputCollector : MonoBehaviour
{
    public static InputCollector Instance;

    uint PlayerId = 0;
    public InputClass CurGameInput;

    void Awake()
    {
        Instance = this;

        CurGameInput = new InputClass();
    }

    void Update()
    {
        if (!LockStepManager.Instance.IsStart) return;

        int _r = -1;
        int _g = -1;
        int _b = -1;
        if (GetKeyDown_1())
        {
            _r = Random.Range(0, 1000);
            _g = Random.Range(0, 1000);
            _b = Random.Range(0, 1000);
        }

        // 统一收集
        CurGameInput.horizontal = (int)(GetInputDirection().x * 1000);
        CurGameInput.vertical = (int)(GetInputDirection().y * 1000);
        CurGameInput.isJump = GetKeyDown_Space();
        CurGameInput.R = _r;
        CurGameInput.G = _g;
        CurGameInput.B = _b;
    }

    #region 输入按键

    private Vector2 GetInputDirection()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        return new Vector2(h, v);
    }

    private bool GetKeyDown_Space() 
    {
        return Input.GetKeyDown(KeyCode.Space);
    }

    private bool GetKeyDown_1()
    {
        return Input.GetKeyDown(KeyCode.Keypad1);
    }

    #endregion
}
