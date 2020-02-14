/* 
 * 管理帧同步的核心逻辑
 * 消息的基本单元是BaseMessage
 * GetMessages[frameIdx]，获取帧消息
 * HandleMessage()，处理帧消息
 */
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

public class LockStepManager : MonoBehaviour
{
    public static LockStepManager Instance;

    public int FPS = 30;
    public const int DEFAULT_START_FRAME = 0;   // 起始帧
    public int frameIdx;                        // 帧号（int完全足够）
    private float _remainTime = 0f;             // 距离下一帧的时间
    private float _frameLength = 0.033f;        // 单位帧的时间长度

    public bool IsStart;
    [SerializeField] bool IsReplay;
    [SerializeField] List<MessageQueue> queueList = new List<MessageQueue>();

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        Application.targetFrameRate = FPS;
        frameIdx = DEFAULT_START_FRAME;
    }

    void Update()
    {
        if (!IsStart) return;

        _remainTime += Time.deltaTime;          // 跑时间

        while (_remainTime > _frameLength)      // 到了下一帧的时间，向下推进一帧
        {
            _remainTime -= _frameLength;

            // 发送帧
            if (!IsReplay)
                SendInput(frameIdx);

            if (HasNextFrame() == false)
            {
                Debug.Log("<color=red>没有帧数据</color>");
                return;
            }

            GameFrameTurn();
        }
    }

    #region Client

    // 把这一帧所有玩家操作数据，发给服务器
    void SendInput(int frame)
    {
        //SimServer.Instance.Send(frame); //TODO: 这里仅仅是模拟
        Debug.Log($"{InputCollector.Instance.CurGameInput.R}, {InputCollector.Instance.CurGameInput.G}, {InputCollector.Instance.CurGameInput.B}");
        BaseMessage cmd = new BaseMessage(0, 0, InputCollector.Instance.CurGameInput);
        MessageRequest.Instance.SendMessage(cmd);
    }

    // 消息管理器中有缓存消息
    bool HasNextFrame()
    {
        //Debug.Log("HasNextFrame: " + frameIdx);
        return MessageManager.Instance.GetMessages(frameIdx) != null;
    }

    // 推进下一帧
    void GameFrameTurn()
    {
        if (HandleMessages())
        {
            frameIdx++; //这帧的消息全部处理完成
        }
    }

    // 处理完成这一帧的所有消息
    bool HandleMessages()
    {
        MessageQueue msgQueue = MessageManager.Instance.GetMessages(frameIdx);
        if (!IsReplay)
            queueList.Add(msgQueue);

        if (msgQueue != null) // 消息管理器中有缓存消息
        {
            for (int i = 0; i < msgQueue.messages.Length; i++)
            {
                MessageManager.Instance.HandleMessage(msgQueue.messages[i]); // 循环处理（Handle）所有消息
            }
            MessageManager.Instance.CompleteMessageQueue(frameIdx); //这一帧处理完成，返回True
            return true;
        }
        return false;
    }

    #endregion

    public void ControlStart(bool value) 
    {
        IsStart = value;
    }

    [ContextMenu("Export")]
    void Export()
    {
        string json = JsonMapper.ToJson(queueList);
        Debug.Log(json);

        string path = Path.Combine(Application.dataPath, "Logs/recode.txt");
        File.WriteAllText(path, json);
    }

    [ContextMenu("Import")]
    void Import()
    {
        IsReplay = true;

        string path = Path.Combine(Application.dataPath, "Logs/recode.txt");
        string json = File.ReadAllText(path);
        //Debug.Log(json);

        queueList = JsonMapper.ToObject<List<MessageQueue>>(json);
        //Debug.Log(temList.Count);

        //回放模式不用发送帧数据（SendInput）
        //直接给 MessageManager 装填消息
        //MessageManager.Instance.MsgBuffer = new List<BaseMessage>();

        MessageManager.Instance.FrameMsgs = new Dictionary<int, MessageQueue>();
        for (int i = 0; i < queueList.Count; i++)
        {
            var val = queueList[i].frameIdx;
            var key = queueList[i];
            MessageManager.Instance.FrameMsgs.Add(val, key);
        }

        MessageManager.Instance.WaitMsgs = new Dictionary<int, MessageQueue>();
        for (int i = 0; i < queueList.Count; i++)
        {
            var val = queueList[i].frameIdx;
            var key = queueList[i];
            MessageManager.Instance.WaitMsgs.Add(val, key);
        }
        Debug.Log(MessageManager.Instance.WaitMsgs.Count);

        IsStart = true;
    }
}
