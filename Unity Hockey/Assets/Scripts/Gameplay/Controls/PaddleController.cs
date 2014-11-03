//Routine to control the paddle.
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TagFrenzy;
using System;

public class PaddleController : MonoBehaviour {

    //Init Data

    //Data initialized in editor, for attached paddle object
    public Toolbox.Plyr Player = new Toolbox.Plyr();    //Construct player object
    public Toolbox.PlayerType Type; //Type of player to initz to
    public Toolbox.ObjType ColType; //Type of object in play to initz to
    public Toolbox.Color Color;     //Color of player to initz to
    
    const byte LEN=17;                  //Length of arrays below
    bool[] aPwr = new bool[LEN+1];        //Bitfield determining which powerup(s) is/are active
    byte[] sPwr = new byte[LEN+1];        //Multiplicity of poweurp collected in inventory
    
    int Score;                          //Score 0-10
    bool Alive;                         //Whether the puck is alive

    //Variables
    public float Height = 0.25f;        //?
    public float multiplier;            //?
    private RaycastHit hit;             // Struct to hold the hit data
    bool Ready = false;

    //Resources
    public Material[] Colors = new Material[5 - 1]; //Color materials
	
    void Start () {
        byte i = 0; //Generic for loop counter

        //Set all saved and active powerups to false/0
        for (i = 0; i <= LEN; i++)
        {
            aPwr[i] = false;
            sPwr[i] = 0;
        }

        //Initialize everything
        Player.Type = Type;
        Player.ColType = ColType;
        Player.Color = Color;
        Player.aPwr = aPwr;
        Player.sPwr = sPwr;
        Player.Score = 0;
        Player.Alive = true;
        Ready = true;
        
	}
	
	// Update is called once per frame
	void Update () {
        if (Ready == false)
        {
            return;
        }

        // Raycast and detect only collisions with the game board.
        //Player.Color = Toolbox.Color.Red;   //Get Player's color
        if ( Physics.Raycast( Camera.main.ScreenPointToRay(Input.mousePosition), out hit, (1<<8) ) )
        {
            List<Tags> enumTags = hit.collider.gameObject.tagsEnum();   //Get tags of raycasted object
            if (enumTags.Contains(Tags.Ground) == true) //If object is ground
            {
                //Log somethings if in DBug mode
                //if (Toolbox.Options.DBug == true)
                //{
                    Debug.Log("Player.Color = " + Player.Color.ToString());
                    Debug.Log("Color2Tag = " + Toolbox.Tagmgmt.ColorVal2Tag(Player.Color).ToString());
                //}

                //Check if Player has same color as ground, and if so, move it
                //If DBug is on, always move

                //if ((enumTags.Contains(Toolbox.Tagmgmt.ColorVal2Tag(Player.Color)) == true)||(Toolbox.Options.DBug==true))
                //{  
                    rigidbody.MovePosition(hit.point);
                //}
            }
        }
    }

    //Check collision types
    void OnCollisionEnter(Collision collision)
    {
        if (Ready == false)
        {
            return;
        }

        List<Tags> enumTags = collision.gameObject.tagsEnum();  //Get tags of collided object
        
        //If object collided with is the invisible boundries, play sfx
        if (enumTags.Contains(Tags.Bounds) == true)
        {
            SoundManager.Play("DeflectH");  //Play deflection (hockey) sfx
        }
    }
}
