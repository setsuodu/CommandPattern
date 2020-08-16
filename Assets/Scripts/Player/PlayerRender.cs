using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRender : MonoBehaviour
{
    public Transform shadow;
    public MoveBase m_BaseMove;

    private Animator _animator;
    protected Animator animator
    {
        get
        {
            if (_animator == null)
            {
                _animator = transform.GetComponentInChildren<Animator>();
            }
            return _animator;
        }
    }

    void Update()
    {
        //transform.position = shadow.position;
        //transform.position = Vector3.Lerp(transform.position, shadow.position, 0.1f);
        transform.position = Vector3.Lerp(transform.position, shadow.position, m_BaseMove._lerpTime);
        m_BaseMove._lerpTime += Time.deltaTime / Time.fixedDeltaTime;

        switch (m_BaseMove.direction)
        {
            case Direction.Left:
                transform.localScale = new Vector3(-1, 1, 1);
                transform.rotation = new Quaternion(0, 1, 0, 0);
                break;
            case Direction.Right:
                transform.localScale = new Vector3(1, 1, 1);
                transform.rotation = new Quaternion(0, 0, 0, 0);
                break;
        }
    }
}
