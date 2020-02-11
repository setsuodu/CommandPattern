using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public static PlayerInput Instance;

    public enum SimulationMode
    {
        Device = 0,     // 从设备获取（单机模式）
        Server = 1,     // 从服务器获取（帧同步）
        Record = 2,     // 从内存获取（录制）
        Log = 3,        // 从本地日志获取（回放）
    }
    public SimulationMode simulationMode = SimulationMode.Device;

    void Awake()
    {
        Instance = this;
    }

    public float GetHorizontalValue()
    {
        switch (simulationMode)
        {
            default:
            case (SimulationMode)0:
                return Input.GetAxis("Horizontal");
            case (SimulationMode)1:
                break;
            case (SimulationMode)2:
                break;
            case (SimulationMode)3:
                break;
        }
        return 0;
    }

    public float GetVerticalValue()
    {
        switch (simulationMode)
        {
            default:
            case (SimulationMode)0:
                return Input.GetAxis("Vertical");
            case (SimulationMode)1:
                break;
            case (SimulationMode)2:
                break;
            case (SimulationMode)3:
                break;
        }
        return 0;
    }

    public bool GetJumpValue()
    {
        switch (simulationMode)
        {
            default:
            case (SimulationMode)0:
                return Input.GetKeyDown(KeyCode.Space);
            case (SimulationMode)1:
                break;
            case (SimulationMode)2:
                // 命令模式
                //ICommand command = new PlaceCubeCommand(hitInfo.point, c, cubePrefab);
                //CommandInvoker.AddCommand(command);
                return false;
            case (SimulationMode)3:
                break;
        }
        return false;
    }
}
