using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public bool IsStart;

    void Awake()
    {
        Instance = this;

        Application.targetFrameRate = 60;
    }
}
