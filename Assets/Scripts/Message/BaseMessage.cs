using System;

[Serializable]
public class BaseMessage
{
    //消息号
    private byte _type;
	public byte Type
    {
		protected set
        {
            _type = value;
        }
        get
        {
            return _type;
        }
    }

    //玩家Id
    private uint _playerId;
    public uint PlayerId
    {
        set
        {
            _playerId = value;
        }
        get
        {
            return _playerId;
        }
    }
}
