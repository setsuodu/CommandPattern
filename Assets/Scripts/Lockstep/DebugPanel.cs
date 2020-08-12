using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugPanel : MonoBehaviour
{
    public MoveBase m_InputRecv;

    public Text m_Text;
    public Toggle m_Start;

    void Awake()
    {
        m_Start.onValueChanged.AddListener((value) =>
        {
            GameManager.Instance.IsStart = value;
            //m_InputSend.m_Lerp = value ? Time.fixedDeltaTime : 1;

            value = !value;
        });
    }

    void Update()
    {
        //m_Text.text = $"Tick={m_InputSend.Tick}";
        m_Text.text = $"Tick={CommandInvoker.counter}";
    }
}
