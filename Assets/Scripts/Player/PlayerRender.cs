using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRender : MonoBehaviour
{
    public Transform shadow;

    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, shadow.position, 0.1f);
    }
}
