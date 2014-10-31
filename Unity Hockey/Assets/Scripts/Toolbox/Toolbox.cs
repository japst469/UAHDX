//Global class to handle global gameplay variables,
//global classes/structs/enums,
//global methods, additional processing for TagFrenzy Tag, etc

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TagFrenzy;
using System;

public class Toolbox : MonoBehaviour
{
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
    public enum ObjType
    {
        Null = -1,    //No collision type
        Paddle = 0,   //Paddle
        FlipperLeft = 1,   //Left Flipper
        FlipperRight = 2   //Right Flipper
    }

    //Type of puck/main game object for processing goals
    public enum PuckType
    {
        Null = -1,    //Notype
        Pk = 0,   //Puck
        Pinball = 1,   //Pinball
    }

    //Type of player
    public enum PlayerType
    {
        Null = -1,          //Nothing/not in use
        PlayerLocal = 0,    //Local player; i.e, player not in netgame
        CPULocal = 1,       //Local CPU; i.e, CPU not in netgame
        PlayerNet = 2,      //Netgame player
        CPUNet = 3          //Netgame CPU
    }

    //Enums for color types and IDs
    //for players/puck/powerup collisions
    public enum Color
    {
        Null,   //No color
        Red,    //Rest self-explanatory :P
        Green,
        Blue,
        Yellow
    }

    //Enums of game mode types
    public enum mode
    {
        Air,    //Rest are self-explanatory :P
        Pinball,
        Battle
    }

    //Enums for difficulty types; self-explanatory :P
    public enum difficulty
    {
        Easy,   //Shortest goal length
        Medium,
        Hard    //Largest goal length
    }

    //Clases

    //List of constants
    public class consts
    {
        public Quaternion qZero;    //Quaternion for 0,0,0,0

        //For loop max iterators
        public byte Num_Pwrs = 18;  //# of powerup types
        public byte Num_Pux = 2;    //Max # of pucks, 0 to 2
        public byte Num_AudClips = 124; //# of audio clips

        //String forms of color tags
        public string Red = "Red";
        public string Blue = "Blue";
        public string Green = "Green";
        public string Yellow = "Yellow";

        //String forms of object tags
        public string Paddle = "Paddle";
        public string Bounds = "Bounds";
        public string Ground = "Ground";
        public string Puk = "Puck";
        public string Powerup = "Powerup";

    }

    //Struct holding all of the options
    public class Option
    {
        public mode Mode = Toolbox.mode.Air;                //Mode type
        public difficulty Diff = difficulty.Medium;         //Difficulty type
        public bool Pwr = true;                             //Powerups on/off
        public bool music = true;                           //Music on/off
        public bool sound = true;                           //Sfx on/off
        public bool announcer = true;                       //Announcer on/off
        public bool Reservetype = false;                    //@Determines whether one or multiple powerups in reserve. NOT implemnted in Options menu yet!
        public bool DBug = false;                            //Debug mode variable, for us developers to debug stuff :).
    }

    //Puck variables
    public class Puck
    {
        public GameObject go;                   //Gameobject
        public PuckType Type = PuckType.Null;   //Type of puck
        public Color Color = Color.Null;        //Color of puck
        public bool Alive = false;              //Flag to determine if puck is alive or not
        public bool Extra = false;              //Flag to determine if puck is an extra one or not
    }

    //Player structure type
    public class Plyr
    {
        public GameObject go;                       //Gameobject
        public PlayerType Type = PlayerType.Null;   //Type of player
        public ObjType ColType = ObjType.Null;      //Type of object in play
        public Color Color = Color.Null;            //Color of player

        //Bitfield determining which powerup(s) is/are active
        public bool[] aPwr =                    
        {
            false,false,false,false,false,false,   
            false,false,false,false,false,false,
            false,false,false,false,false,false,
        };

        //Multiplicity of poweurp collected in inventory.
        public byte[] sPwr =                     
        {
            0,0,0,0,0,0,
            0,0,0,0,0,0,
            0,0,0,0,0,0   
        };

        //Flag to determine if player is alive or not (either killed or not in use)
        public bool Alive = false;
        public byte Score = 0;  //Score
    }

    //Constructors 
    public static Option Options = new Option();
    public static consts Consts = new consts();

    //Tag Management methods
    public static class Tagmgmt
    {
        //Function to convert Tag 2 Color type

        //Input: GameObject
        public static Toolbox.Color ObjectTag2Color(GameObject obj)
        {
            List<Tags> enumTags = obj.tagsEnum();
            Toolbox.Color clr;

            //Determine 1st color tag that is enabled, return the Color type
            //Supposed to only be one color active at one time
            if (enumTags.Contains(Tags.Red) == true)
            {
                clr = Toolbox.Color.Red;
            }
            else
            {
                if (enumTags.Contains(Tags.Blue) == true)
                {
                    clr = Toolbox.Color.Blue;
                }
                else
                {
                    if (enumTags.Contains(Tags.Green) == true)
                    {
                        clr = Toolbox.Color.Green;
                    }
                    else
                    {
                        if (enumTags.Contains(Tags.Yellow) == true)
                        {
                            clr = Toolbox.Color.Yellow;
                        }
                        else
                        {
                            clr = Toolbox.Color.Null;
                        }
                    }
                }
            }

            return clr;
        }

        //Method to convert a color Tag to a Color type
        //Input: Tag
        public static Toolbox.Color Tag2Color(Tags tag)
        {
            Toolbox.Color clr;  //Color to return

            //Find which tag is active, store the Color type
            if (tag==Tags.Red)
            {
                clr = Toolbox.Color.Red;
            }
            else
            {
                if (tag==Tags.Blue)
                {
                    clr = Toolbox.Color.Blue;
                }
                else
                {
                    if (tag==Tags.Green)
                    {
                        clr = Toolbox.Color.Green;
                    }
                    else
                    {
                        if (tag==Tags.Yellow)
                        {
                            clr = Toolbox.Color.Yellow;
                        }
                        else
                        {
                            clr = Toolbox.Color.Null;
                        }
                    }
                }
            }

            return clr;
        }

        //Method to find what color tag is attached to the object, and returns that color tag
        //Input: GameObject
        //Output: Tag
        public static Tags GetObjColorTag(GameObject obj)
        {
            Tags tag = Tags.NoColor;    //tag to return
            List<Tags> objTags = obj.tagsEnum();    //Current list of tags attached to object

            if (objTags.Contains(Tags.Red) == true)
            {
                tag = Tags.Red;
                return tag;
            }

            if (objTags.Contains(Tags.Blue) == true)
            {
                tag = Tags.Blue;
                return tag;
            }

            if (objTags.Contains(Tags.Green) == true)
            {
                tag = Tags.Green;
                return tag;
            }

            if (objTags.Contains(Tags.Yellow) == true)
            {
                tag = Tags.Yellow;
                return tag;
            }
            return tag;
        }

        //Method to convert a Color value to a Tag
        //Input: Color
        //Output: Tag
        public static Tags ColorVal2Tag(Toolbox.Color Clr)
        {
            Tags RetClr;    //Tag to return

            //Find what Color, return its tag equivalent
            switch (Clr)
            {
                case Toolbox.Color.Blue:
                    RetClr = Tags.Blue;
                    break;
                case Toolbox.Color.Red:
                    RetClr = Tags.Red;
                    break;
                case Toolbox.Color.Green:
                    RetClr = Tags.Green;
                    break;
                case Toolbox.Color.Yellow:
                    RetClr = Tags.Yellow;
                    break;
                default:
                    RetClr=Tags.NoColor;
                    break;
            }        
            return RetClr;
        }

        //Method to disable (make false) colors other than inputted
        //Input: GameObject, color tag
        public static void RadioColorTags(GameObject obj, Tags tag)
        {
            return; //@ Remove me once debugging!
            if (tag != Tags.Red) { obj.RemoveTag(Tags.Red); }
            if (tag != Tags.Blue) { obj.RemoveTag(Tags.Blue); }
            if (tag != Tags.Green) { obj.RemoveTag(Tags.Green); }
            if (tag != Tags.Yellow) { obj.RemoveTag(Tags.Yellow); }
            if (tag != Tags.NoColor) { obj.RemoveTag(Tags.NoColor); }
        }
    }

    //Common position routines
    public static class Position
    {
        public static void RandomizeInRegion(GameObject Anchore, GameObject Object,float radius_X, float radius_Z)
        {
            //Object variables
            float x, z; //It's x and z offset coords, local to Spawner
            //Anchor object
            float Anchor_X, Anchor_Y, Anchor_Z;    //Its x, y, z coords

            Anchor_X = Anchore.transform.position.x;
            Anchor_Y = Anchore.transform.position.y;
            Anchor_Z = Anchore.transform.position.z;

            Vector3 NewPos;     //New position vector
            UnityEngine.Random.seed = (int)System.DateTime.Now.Ticks;   //@Get new randomization seed from system time. Needs to be synce in netgames!

            //Get random X & Z offset, and Type of powerup        
            x = UnityEngine.Random.Range(-radius_X, radius_X);
            z = UnityEngine.Random.Range(-radius_Z, radius_Z);

            //Place It, relative to Spawner
            NewPos.x = Anchor_X + x;
            NewPos.y = Anchor_Y;
            NewPos.z = Anchor_Z + z;
            Object.gameObject.transform.position = NewPos; //Update the position
            SoundManager.Play("Teleport");  //Play Teleport sfx
        }
    }
}

