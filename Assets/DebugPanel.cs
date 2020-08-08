using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugPanel : MonoBehaviour
{
    public InputSend m_InputSend;

    public Text m_Text;
    public Toggle m_Start;

    void Awake()
    {
        m_Start.onValueChanged.AddListener((value) =>
        {
            m_InputSend.IsStart = value;
            value = !value;
        });
    }

    void Update()
    {
        //m_Text.text = $"Tick={m_InputSend.Tick}";
        m_Text.text = $"Tick={CommandInvoker.counter}";
    }
}
