public class InputClass
{
    public uint tick;
    public bool left;       //A
    public bool right;      //D
    public bool up;         //W
    public bool down;       //S
    public bool space;      //Space
    //public bool keycode5; //J
    //public bool keycode6; //K
    public void Echo()
    {
        string _tick = tick.ToString().PadLeft(4, '0');
        string str = _tick + " | " + HighLight(left) + " | " + HighLight(right) + " | " + HighLight(up) + " | " + HighLight(down) + " | " + HighLight(space) + " | ";
        UnityEngine.Debug.Log(str);
    }
    private static string HighLight(bool value)
    {
        string ret = value ? "<color=yellow>true</color>" : "false";
        return ret;
    }
}
