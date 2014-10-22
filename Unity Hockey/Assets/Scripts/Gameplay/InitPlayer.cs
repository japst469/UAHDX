using UnityEngine;
using System.Collections;

public class InitPlayer: MonoBehaviour {

    public byte ID;
    public PlayerType Type; //Type of player
    public ObjType ColType; //Type of object in play
    public Color Color;     //Color of player


    const byte LEN=17;
    public bool[] aPwr = new bool[LEN];     //@Bitfield determining which powerup(s) is/are active. Why can't this element be an array/how to?
    public byte[] sPwr = new byte[LEN];        //@Multiplicity of poweurp collected in inventory. Why can't this element be an array/how to?
    public bool[] blnNull18 = new bool[LEN];
    public byte[] bytNull18 = new byte[LEN];

    public int Score;                         //Score 0-10
    public bool Alive;

    void Start()
    {
        
        byte i=0;

        for (i=0;i<=LEN;i++)
        {
            aPwr[i]=false;
            sPwr[i]=0;
        }
        
        Toolbox.Instance.Players[ID].Type = Type;
        Toolbox.Instance.Players[ID].ColType=ColType;
        Toolbox.Instance.Players[ID].Color=Color;
        Toolbox.Instance.Players[ID].aPwr=blnNull18;
        Toolbox.Instance.Players[ID].sPwr=bytNull18;
        Toolbox.Instance.Players[ID].Score = 0;
        Toolbox.Instance.Players[ID].Alive=true;
    }
}