using UnityEngine;
using System.Collections;

//STRUCTS AND ENUMS
//for gameplay

//Unfortunately, typedef DNE in C# :(
//http://www.codeproject.com/Questions/141385/typedef-in-C

//Enum for powerup names & IDs
public enum Pwrs
{
    Nothing,
    Army,
    BSaver,
    Big,
    Block,
    Bumpers,
    Fastmo,
    Grav,
    HotPot,
    Invis,
    Kill,
    Multi,
    Portals,
    Ramps,
    Shield,
    Slomo,
    Swap,
    Random
}

//Enums for Player Types and ID
public enum PlayerType
{
    Null = -1,          //@Nothing/not in use. Can enums have negatives?
    PlayerLocal = 0,    //Local player; i.e, player not in netgame
    CPULocal = 1,       //Local CPU; i.e, CPU not in netgame
    PlayerNet = 2,      //Netgame player
    CPUNet = 3          //Netgame CPU
}

//Enums for color types and IDs, for players/puck/powerup collisions
public enum Color
{
    Null,   //No color
    Red,    //Pretty self-explanatory :P
    Green,
    Blue,
    Yellow
}

//Player structure type
public struct Plyr
{
    public PlayerType Type; //Type of player
    public Color Color;     //Color of player

    public bool[] aPwr;// = new bool[18];     //@Bitfield determining which powerup(s) is/are active. Why can't this element be an array/how to?
    public int[] sPwr;//= new int[18];        //@Multiplicity of poweurp collected in inventory. Why can't this element be an array/how to?

    public int Score;                         //Score 0-10
    public bool Alive;                        //Flag to determine if player is alive or not (either killed or not in use)
}

//Enums of Mode types
public enum mode
{
    Menus,  //Menus; i.e, not in a game
    Air,    //Rest are self-explanatory
    Pinball,
    Battle
}

//Enums for difficulty types; self-explanatory
public enum diff
{
    Easy,
    Medium,
    Hard
}

//Struct holding all of the options
public struct Option
{
    public mode Mode;       //Mode type
    public diff Diff;       //Difficulty type
    public bool Pwr;        //Powerups on/off
    public bool music;      //Music on/off
    public bool sound;      //Sfx on/off
    public bool announcer;  //Announcer on/off
    public bool Reservetype;//@Determiens whether one or multiple powerups in reserve. NOT implemnted in Options menu yet!
    public bool DBug;       //Debug mode variable, for us developers to debug stuff :).
}

public class __GameData : MonoBehaviour {
    
    void Start()
    {
        //Initialize variables

        Plyr[] Players = new Plyr[4];   //Create 4 new player objects
        Option Options;                 //Create options struct variable
        Options.Mode = mode.Menus;      //Force mode to be in menus
        Options.Diff = diff.Medium;     //Force medium difficulty
        Options.Pwr = true;             //Force powerups, music, sound, and announcer guy to be On
        Options.music = true;
        Options.sound = true;
        Options.announcer = true;
        Options.Reservetype = false;
        Options.DBug = false;           //Force debug mode to be on or off
    }

    // Update is called once per frame
    void Update()
    {
        //NOP
    }
}

