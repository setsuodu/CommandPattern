using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputPlane : MonoBehaviour
{
    public Transform cubePrefab;

    void Start()
    {
        cubePrefab = Resources.Load<GameObject>("Prefabs/Cube").transform;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity)) 
            {
                Color c = new Color(Random.Range(0.5f, 1), Random.Range(0.5f, 1), Random.Range(0.5f, 1));

                // 传统写法
                //CubePlacer.PlaceCube(hitInfo.point, c, cubePrefab);

                // 命令模式
                ICommand command = new PlaceCubeCommand(hitInfo.point, c, cubePrefab);
                CommandInvoker.AddCommand(command);
            }
        }
    }
}
