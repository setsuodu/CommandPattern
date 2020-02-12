using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    const float JumpHeight = 2f;
    const float Gravity = -30f;
    const float MaxDownYVelocity = 15; // 最大下落速度
    const float RunningSpeed = 3f;
    const float AngulaSpeed = 10f;

    private Animator _animator;
    private CharacterController _controller;
    private MeshRenderer _render;

    private Vector3 _forward;
    private Vector3 _right;
    private Vector3 _move;
    private Vector3 _velocity;

    // Input.
    public float _horizontal;
    public float _vertical;
    private bool _jump;

    void Awake()
    {
        _animator = GetComponent<Animator>();
        _controller = GetComponent<CharacterController>();
        _render = transform.GetChild(0).GetComponent<MeshRenderer>();
    }

    void Update()
    {
        /*
        #region 输入
        _horizontal = PlayerInput.Instance.GetHorizontalValue();
        _vertical = PlayerInput.Instance.GetVerticalValue();
        _jump = PlayerInput.Instance.GetJumpValue();

        if (_jump)
        {
            Jump(JumpHeight);
        }
        #endregion

        #region 移动
        _forward = Camera.main.transform.TransformDirection(Vector3.forward); // 以摄像机为基准移动
        _forward.y = 0f;
        _forward = _forward.normalized;
        _right = new Vector3(_forward.z, 0.0f, -_forward.x);
        _move = (_horizontal * _right + _vertical * _forward);

        if (_controller.enabled)
        {
            _controller.Move(_move * Time.deltaTime * RunningSpeed);
        }
        #endregion

        #region 旋转
        if (_move.magnitude > 0.1f) // 移动增量
        {
            float angle = Mathf.Atan2(_move.x, _move.z) * Mathf.Rad2Deg;
            Quaternion newRot = Quaternion.Euler(new Vector3(0, angle, 0));
            transform.rotation = Quaternion.Slerp(transform.rotation, newRot, Time.deltaTime * AngulaSpeed);
        }
        #endregion
        */

        #region 跳跃
        if (_velocity.y >= -MaxDownYVelocity)
        {
            _velocity.y += Gravity * Time.deltaTime;
        }
        if (_controller.enabled)
        {
            _controller.Move(_velocity * Time.deltaTime);
        }
        #endregion
    }

    #region Actions

    void Jump(float jumpHeight)
    {
        if (_controller.isGrounded)
        {
            _velocity.y = 0;
            _velocity.y += Mathf.Sqrt(jumpHeight * -2f * Gravity);
        }
    }

    public void OnInputMsgReceived(BaseMessage msg)
    {
        #region 跳跃
        if (msg.Inputs.isJump)
            Jump(JumpHeight);
        #endregion

        #region 移动
        _forward = Camera.main.transform.TransformDirection(Vector3.forward); // 以摄像机为基准移动
        _forward.y = 0f;
        _forward = _forward.normalized;
        _right = new Vector3(_forward.z, 0.0f, -_forward.x);
        _move = (msg.Inputs.horizontal * _right + msg.Inputs.vertical * _forward) * 0.001f; // MoveMessage 是1000倍
        if (_controller.enabled)
            _controller.Move(_move * Time.deltaTime * RunningSpeed);

        if (_move.magnitude > 0.1f) // 移动增量
        {
            float angle = Mathf.Atan2(_move.x, _move.z) * Mathf.Rad2Deg;
            Quaternion newRot = Quaternion.Euler(new Vector3(0, angle, 0));
            transform.rotation = Quaternion.Slerp(transform.rotation, newRot, Time.deltaTime * AngulaSpeed);
        }
        #endregion

        #region 颜色
        if (msg.Inputs.R >= 0 && msg.Inputs.G >= 0 && msg.Inputs.B >= 0)
        {
            Color _color = new Color(msg.Inputs.R * 0.001f, msg.Inputs.G * 0.001f, msg.Inputs.B * 0.001f);
            _render.material.color = _color; // ColorMessage 是1000倍
        }
        #endregion
    }

    #endregion
}
