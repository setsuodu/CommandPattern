using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugPanel : MonoBehaviour
{
    public Text m_Text;
    public InputSend m_InputSend;

    void Start()
    {
        
    }

    void Update()
    {
        m_Text.text = $"Tick={m_InputSend.Tick}";
    }
}
