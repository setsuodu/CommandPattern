using UnityEngine;

[System.Serializable]
public class InputClass
{
    #region 每帧需要收集的参数
    //public Vector2 inputUV; //Vector2不允许
    public int horizontal;
    public int vertical;
    public bool isJump;
    //public Color color;
    public int R, G, B;
    //public bool isInputFire;
    //public int skillId;
    //public bool isSpeedUp;
    #endregion
    public InputClass() { }
    public void Echo()
    {
        //string _tick = tick.ToString().PadLeft(4, '0');
        //string str = _tick + " | " + HighLight(left) + " | " + HighLight(right) + " | " + HighLight(up) + " | " + HighLight(down) + " | " + HighLight(space) + " | ";
        //UnityEngine.Debug.Log(str);
    }
    //private static string HighLight(bool value)
    //{
    //    string ret = value ? "<color=yellow>true</color>" : "false";
    //    return ret;
    //}
}
