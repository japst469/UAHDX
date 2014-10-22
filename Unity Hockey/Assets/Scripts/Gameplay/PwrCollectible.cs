//Routine to process randomization of powerup, and collecting it

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TagFrenzy;
using System;

public class PwrCollectible : MonoBehaviour {

    public GameObject Spawner;  //The gameobject of the powerup spawner
    float Spawn_X, Spawn_Y, Spawn_Z;    //Its x, y, z coords
    public float radius_X, radius_Z;    //The x and z square coords to randomize within

    GameObject Powerup; //The powerup collectible gameobject
    public Texture[] Tex=new Texture[19];   //The textures for each powerup
    Toolbox.Pwrs Pwr_Type=Toolbox.Pwrs.Nothing; //Create a new powerup type
    float x, z; //It's x and z offset coords, local to Spawner

    //Time variables. Current time, next spwawn time, difference in time btwn the 2
    float CurrentTime, NextSpawnTime, DifTime;  
    bool Alive=false;   //Flag if powerup is alive
    bool Timing=false;  //Flag is currently doing timing

    //Valid powerup choice in Air/Battle modes
    bool[] ValidAir=
    {false,true,false,true,true,
     true,true,true,true,true,
     true,true,true,true,true,
     true,true,true};

    //Valid powerup choices in Pinball mode
    bool[] ValidPin=
    {
        true,false,true,false,true,
        true,true,true,true,true,
        true,true,true,true,true,
        true,true,true,
    };
    

	// Use this for initialization
	void Start () {
	    //Get base x, y, z coords of spawner
        Spawn_X = Spawner.transform.position.x;
        Spawn_Y = Spawner.transform.position.y;
        Spawn_Z = Spawner.transform.position.z;
        Powerup = this.gameObject;  //Get the powerup object itself
        Randomize();    //@Debuging only. Remove me, uncomment next line!
        //Timer_Start();    //Start the timing
	}
	
	// Update is called once per frame
	void Update () {
        return; //@Debugging only. Remove me!
        Randomize(); //@Debugging only. Remove me!

        //If curerntly timing, update the timer
        if (Timing == true)
        {
            Timer_Update();
        }

        //If powerup is not alive, and not timing, then randomize
        if ((Alive == false) && (Timing=false))
        {
            Randomize();
        }
	}

    //Routine to randomize the powerup collectible's position and type
    void Randomize()
    {        
        bool valid=false;   //Flag to determine if randomized powerup is valid for mode
        Vector3 NewPos;     //New position vector

        const float Pwr_Min=(float)((int)Toolbox.Pwrs.Nothing); //The minimum powerup to randomize.range
        const float Pwr_Max = (float)((int)Toolbox.Pwrs.Random); //The maximum powerup to randomize.range
        UnityEngine.Random.seed = (int)System.DateTime.Now.Ticks;   //@Get new randomization seed from system time. Needs to be synce in netgames!
         
        //Get random X & Z offset, and Type of powerup        
        x = UnityEngine.Random.Range(-radius_X, radius_X);
        z = UnityEngine.Random.Range(-radius_Z, radius_Z);

        //Loop until a valid powerup is gotten
        while (valid==false)
        {
            Pwr_Type = (Toolbox.Pwrs)((int)(UnityEngine.Random.Range(Pwr_Min,Pwr_Max)));
            if (Toolbox.Options.Mode==Toolbox.mode.Pinball)
            {
                valid=ValidPin[(byte)Pwr_Type];
            }
            else
            {
                valid=ValidAir[(byte)Pwr_Type];
            }
        }

        //Set textures
        Powerup.renderer.material.mainTexture = Tex[(byte)Pwr_Type];
        
        //Place It, relative to Spawner
        NewPos.x = Spawn_X + x;
        NewPos.y = Spawn_Y;
        NewPos.z = Spawn_Z+z;
        Powerup.gameObject.transform.position = NewPos; //Update the position
        SoundManager.Play("Teleport");  //Play Teleport sfx
        Alive = true;   //Make the powerup alive
    }

    //Star the timer
    void Timer_Start()
    {
        return; //@Debugging only. Remove me!
        UnityEngine.Random.seed = (int)System.DateTime.Now.Ticks; //Get new random seed //@Debugging only. Remove me!
        Alive = false;  //Make it not alive
        Timing = true;  //Set timing to on
        CurrentTime = Time.timeSinceLevelLoad;  //Get current time
        NextSpawnTime = UnityEngine.Random.Range(0.0f, 10.0f); //Randomize for a new time 0 to 10 seconds later

        //Place the powerup OOB to "hide it"
        Powerup.transform.position=Vector3.zero;
    }

    //Routine to update the timer
    void Timer_Update()
    {
        return; //@Debugging only. Remove me!
        DifTime = Time.timeSinceLevelLoad - CurrentTime;    //Find the difference in time between start and now

        //If the diference in time is greater than the conditioner, set timing to off
        if (DifTime >= NextSpawnTime)
        {
            Timing = false;
        }
    }

    //Process collisions
    void OnCollisionEnter(Collision otherObj)
    {
        //If powerup is alive and timing is off, skip everything
        if ((Alive != true) && (Timing != false))
        {
            return;
        }
       
        //Sound strings for annoucner for each poweurp
        string[] PwrAnn =
        {
            "NULL",
            "vArmy",
            "vBall_Saver",
            "vBig Stick",
            "vBlockade",
            "vBumpers",
            "vFastmo",
            "vGravity",
            "vHotPot",
            "vInvis",
            "vKill",
            "vMultipuck",
            "vPortals",
            "vRamps",
            "vShield",
            "vSlowmo",
            "vSwap Place",
            "NULL"
        };

        ///Change multi-thing powerup to approriate soundclip depending on mode
        if (Toolbox.Options.Mode != Toolbox.mode.Pinball)
        {
            PwrAnn[11]="vMultiball";
        }     

        //List of tags for collided object
        List<Tags> OtherTags = otherObj.gameObject.tagsEnum();
        Tags OtherColor;    //Tag for colidded object's color

        //Check for collision with a puck
        if (OtherTags.Contains(Tags.Puck) == true)
        {
            SoundManager.Play("PwrGet");    //Play sfx

            //Log the annoncer audio clip
            if (Toolbox.Options.DBug == true)
            {
                Debug.Log("Ann = " + PwrAnn[(byte)Pwr_Type].ToString());
            }

            //Let the announcer announcer the powerup collected
            SoundManager.Play(PwrAnn[(byte)Pwr_Type]);

            //Get the color of the puck
            OtherColor = Toolbox.Tagmgmt.GetObjColorTag(otherObj.gameObject);
            Randomize();    //@Debug only. Remove me!
            //Timer_Start();    //Restart timing/hiding process
            //@Do singleton stuff to pass powerup collected to a helper object
        }
    }
}
