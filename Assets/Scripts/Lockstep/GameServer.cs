using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class GameServer : MonoBehaviour
{
    public static GameServer Instance;

    public long frameIdx;  // 帧号
    public float _accumilatedTime = 0f;
    public float _frameLength = 0.067f; //帧率15
    public Text tickText;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        frameIdx = LockStepManager.DEFAULT_START_FRAME;
    }

    void Update()
    {
        //if (!GameManager.Instance.gameStart) return;

        _accumilatedTime = _accumilatedTime + Time.deltaTime;

        while (_accumilatedTime > _frameLength)
        {
            long curFrameIdx = frameIdx;

            ServerFrameTurn(curFrameIdx);

            frameIdx++;
            tickText.text = "Tick: " + frameIdx;

            _accumilatedTime = _accumilatedTime - _frameLength;
        }
    }

    void ServerFrameTurn(long curFrameIdx)
    {
        MessageRequest.Instance.ReturnMessage(curFrameIdx);
    }
}
