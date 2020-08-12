using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRender : MonoBehaviour
{
    public Transform shadow;
    public MoveBase m_InputRecv;

    void Update()
    {
        //transform.position = Vector3.Lerp(transform.position, shadow.position, 0.1f);

        transform.position = Vector3.Lerp(transform.position, shadow.position, m_InputRecv._lerpTime);
        m_InputRecv._lerpTime += Time.deltaTime / Time.fixedDeltaTime;
    }
}
