//Routine to process the puck(s)/pinball(s)

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TagFrenzy;
using System;

public class Pucky : MonoBehaviour {

    //Init Stats
    Toolbox.Puck Pux=new Toolbox.Puck();    //Construct new puck object

    //Initz values, from editor
    public Toolbox.PuckType Type; //Type of puck
    public Toolbox.Color Color;     //Color of puck
    public GameObject Child;        //GameObject of child, that is, the physical model
    public bool Alive;      //Flag to determine if puck is alive or not
    public bool Extra;      //Flag to determine if puck is an extra one or not   
         
    //Resources
    public GameObject PuckSpawner;  //Puck Spawner object
    public Material[] Colors = new Material[5 - 1]; //Color materials
    bool Ready = false;      //Flag to determine if Start() was ran
    bool blnReset = false;  //Flag to determine whether to respawn puck at PuckSpawner

	// Use this for initialization
	void Awake () {

        //Initz everything
        Pux.go = this.gameObject;
        Pux.Type = Type;
        Pux.Color = Color;
        Pux.Alive = Alive;
        Pux.Extra = Extra;

        //Constrain object on Y, and with no rotation
        Pux.go.rigidbody.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
        Pux.go.rigidbody.useGravity = false;    //Disable gravity
        Ready = true;   //I.M Rdy!
	}
	
	// Update is called once per frame
	void Update () {
        List<Tags> PuxTags = Pux.go.tagsEnum(); //Get the tags on the puck

        //If it has reset, reset the puck
        if (PuxTags.Contains(Tags.Reset)==true)
        {
            Reset();
        }

        //If the puck is NOT ready, or it is NOT alive
        if ((Ready == false)||(Pux.Alive==false))
        {
            Child.gameObject.renderer.enabled=false;    //@Disable its visual. Malfunctioning?
            return;
        }
        Child.gameObject.renderer.enabled=true; //Enable its rendere

        Chameleon();    //Do color changing
        
	}

    //Routine to change the puck's color upon paddle hit
    void Chameleon()
    {
        Toolbox.Color Clr = Pux.Color;  //The puck's color

        //Log the color
        if (Toolbox.Options.DBug == true)
        {
            Debug.Log(Pux.Color);
        }

        //Change the puck's physical color according to its tag
        switch (Clr)
        {
            case (Toolbox.Color.Red):
                Toolbox.Tagmgmt.RadioColorTags(Child.gameObject, Tags.Red); //Remove all other tags attached, except current color
                Child.renderer.material = Colors[1];    //Change the color
                break;

            //Repeat for other cases
            case (Toolbox.Color.Blue):
                Toolbox.Tagmgmt.RadioColorTags(Child.gameObject, Tags.Blue);
                Child.renderer.material = Colors[2];
                break;
            case (Toolbox.Color.Green):
                Toolbox.Tagmgmt.RadioColorTags(Child.gameObject, Tags.Green);
                Child.renderer.material = Colors[3];
                break;
            case (Toolbox.Color.Yellow):
                Toolbox.Tagmgmt.RadioColorTags(Child.gameObject, Tags.Yellow);
                Child.renderer.material = Colors[4];
                break;
            default:    //NoColor case
                Toolbox.Tagmgmt.RadioColorTags(Child.gameObject, Tags.NoColor);
                Child.renderer.material = Colors[0];
                break;
        }
    }

    //Routine to respawn a puck
    //@NOT entirely working!
    void Reset()
    {
            
            //If reset runonce flag is false, move puck to PuckSpanwer's position
            if (blnReset==false)
            {
                Pux.go.transform.position = PuckSpawner.gameObject.transform.position;
                blnReset = true;
            }

            //Change constraints to only X & Z, and use gravity
            Pux.go.rigidbody.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
            Pux.go.rigidbody.useGravity = true;
            Toolbox.Tagmgmt.RadioColorTags(Pux.go, Tags.NoColor);   //Remove tag
            Chameleon();    //Change colors again
    }

    //Routine to process puck collision actions
    void OnCollisionEnter(Collision otherObj)
    {
        //IF NOT ready, and NOT alive, NOP
        if ((Ready == false)||(Pux.Alive==false))
        {
            return;
        }
        
        //Get list of tags
        Toolbox.Color Clr = Pux.Color;  //Puck color
        List<Tags> OtherTags = otherObj.gameObject.tagsEnum();  //Colided object's tags
        List<Tags> PuxTags = Pux.go.tagsEnum();                 //The puck's tags

        //If Reset status, then
        if (PuxTags.Contains(Tags.Reset)==true)
        {
            //Check collision with ground
            if (OtherTags.Contains(Tags.Ground)==true)
            {
                //@Attempt, and FAIL, at resetting constraints to Y and all rotations. Disable gravity too
                Pux.go.rigidbody.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
                Pux.go.rigidbody.useGravity = false;
                Pux.go.RemoveTag(Tags.Reset);   //Remove the Reset tag
                blnReset = false;   //Set reset to false. Resume normal operations!
            }
        }

        //Can't use switch here
        //Due to it being fussy with having to use immediates, NOT variables :(

        //Check collision with Bounds
        if (OtherTags.Contains(Tags.Bounds) == true)
        {
            //Play appropriate deflect sound based on mode
            if (Toolbox.Options.Mode != Toolbox.mode.Pinball)
            {
                SoundManager.Play("DeflectH");
            }
            else
            {
                SoundManager.Play("DeflectP");
            }
        }
        else
        {
            //Check collisino with Paddle
            if (OtherTags.Contains(Tags.Paddle) == true)
            {
                //Transfer color of paddle to puck
                Clr=Toolbox.Tagmgmt.ObjectTag2Color(otherObj.gameObject);
                Pux.Color = Clr;

                //Play appropriate puck hit sound, depending on mode
                if (Toolbox.Options.Mode != Toolbox.mode.Pinball)
                {
                    SoundManager.Play("Hit");
                }
                else
                {
                    SoundManager.Play("DeflectP");
                }
                //Play ColorChange sfx
                SoundManager.Play("ColorChange");
            }
            else
            {
                //Check collision with another puck
                if (OtherTags.Contains(Tags.Puck) == true)
                {
                    //Play appropriate deflection sound, based on mode
                    if (Toolbox.Options.Mode != Toolbox.mode.Pinball)
                    {
                        SoundManager.Play("DeflectH");
                    }
                    else
                    {
                        SoundManager.Play("DeflectP");
                    }
                }
            }
        }
            
    }

}