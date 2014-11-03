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

    GameObject FieldCurrent;

    bool[] aPwr = new bool[Toolbox.Consts.Num_Pwrs];        //Bitfield determining which powerup(s) is/are active
    byte[] sPwr = new byte[Toolbox.Consts.Num_Pwrs];        //Multiplicity of poweurp collected in inventory
    
    int Score;                          //Score 0-10
    bool Alive;                         //Whether the puck is alive

    //Variables
    float speed;
    private RaycastHit hit;             // Struct to hold the hit data
    byte Pwr=0;
    bool Ready = false;
    bool Repeled = false;

    //Resources
    public Material[] Colors = new Material[5 - 1]; //Color materials
	
    void Start () {
        byte i = 0; //Generic for loop counter

        //Set all saved and active powerups to false/0
        for (i = 0; i <= Toolbox.Consts.Num_Pwrs-1; i++)
        {
            aPwr[i] = false;
            sPwr[i] = 0;
        }

        //Initialize everything
        Player.go = this.gameObject;
        Player.Type = Type;
        Player.ColType = ColType;
        Player.Color = Color;
        Player.aPwr = aPwr;
        Player.sPwr = sPwr;
        Player.Score = 0;
        Player.Alive = true;
        Ready = true;        

        switch(Toolbox.Options.Diff)
        {
            case Toolbox.difficulty.Easy:
                speed = 5;
                break;
            case Toolbox.difficulty.Medium:
                speed = 10;
                break;
            case Toolbox.difficulty.Hard:
                speed = 15;
                break;       
        }
	}
	
	// Update is called once per frame
    void Update()
    {
        if (Ready == false)
        {
            return;
        }

        MovePaddle();
    }

    void MovePaddle()
    {
        byte i;

        RaycastHit hitinfo;
        List<GameObject> Pux;
        Vector3 Origin;
        GameObject OtherObj;
        List<Tags> OtherTags;
        List<GameObject> Field;

        bool MoveIt=false;        
        
        if ((Player.Type==Toolbox.PlayerType.CPULocal)||(Player.Type==Toolbox.PlayerType.CPUNet))
        {
            Pux=MultiTag.FindGameObjectsWithTags(Tags.Puck);

            for (i=0;i<=Toolbox.Consts.Num_Pux;i++)
            {
                Origin = Pux[i].transform.position;

                //http://answers.unity3d.com/questions/32251/raycasting-to-check-what-object-with-a-particular.html
                if(Physics.Raycast(Origin,Vector3.down,out hitinfo)==true)
                {
                    OtherObj = hitinfo.collider.gameObject;
                    OtherTags = OtherObj.tagsEnum();
                    MoveIt = false;
                    
                    if (OtherTags.Contains(Tags.Ground)==true)
                    {
                        if ((OtherTags.Contains(Tags.Red) == true) && (Player.Color == Toolbox.Color.Red))
                        {
                            MoveIt = true;
                        }
                        
                        if ((OtherTags.Contains(Tags.Blue) == true) && (Player.Color == Toolbox.Color.Blue))
                        {
                            MoveIt = true;
                        }

                        if ((OtherTags.Contains(Tags.Green) == true) && (Player.Color == Toolbox.Color.Green))
                        {
                            MoveIt = true;
                        }

                        if ((OtherTags.Contains(Tags.Yellow) == true) && (Player.Color == Toolbox.Color.Yellow))
                        {
                            MoveIt = true;
                        }

                        if (MoveIt == true)
                        {
                            Field=MultiTag.FindGameObjectsWithTags(Tags.Ground, Toolbox.Tagmgmt.ColorVal2Tag(Player.Color), TagMatch.Exact);
                            if (OtherObj != Field[0])
                            {
                                RepelPux(Pux[i].gameObject);
                            }
                        }
                        else
                        {
                            PuppyGuard(Pux[i].gameObject,Toolbox.Tagmgmt.ColorVal2Tag(Player.Color));
                        }
                    }
                }
            }
        }

        else
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, (1 << 8)))
            {
                List<Tags> enumTags = hit.collider.gameObject.tagsEnum();   //Get tags of raycasted object
                if (enumTags.Contains(Tags.Ground) == true) //If object is ground
                {
                    //Log somethings if in DBug mode
                    if (Toolbox.Options.DBug == true)
                    {
                    Debug.Log("Player.Color = " + Player.Color.ToString());
                    Debug.Log("Color2Tag = " + Toolbox.Tagmgmt.ColorVal2Tag(Player.Color).ToString());
                    }

                    //Check if Player has same color as ground, and if so, move it
                    //If DBug is on, always move

                    //if ((enumTags.Contains(Toolbox.Tagmgmt.ColorVal2Tag(Player.Color)) == true)||(Toolbox.Options.DBug==true))
                    //{  
                        rigidbody.MovePosition(hit.point);
                    //}
                }
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

        if (enumTags.Contains(Tags.Puck) == true)
        {
            Repeled = true;
        }

        if (enumTags.Contains(Tags.Ground) == true)
        {
            FieldCurrent = collision.gameObject;
        }
    }

    void RepelPux(GameObject Pux)
    {        
        float Angle;
        Vector3 Target;
        
        Angle=Vector3.Angle(Player.go.transform.position,Pux.transform.position);
        //Debug.Log(Angle);
        
        Target=Vector3.MoveTowards(transform.position, Pux.transform.position, speed * Time.deltaTime);
        Target.y=Player.go.transform.position.y;
        Player.go.transform.position = Target;
    }

    void PuppyGuard(GameObject Pux,Tags Color)
    {
        Vector3 Origin;        
        List<GameObject> FieldObj;

        Vector3 Offset;

        Origin=Player.go.transform.position;
        FieldObj = MultiTag.FindGameObjectsWithTags(Tags.Ground, Color, TagMatch.Exact);

        /*
        if (FieldCurrent == FieldObj[0])
        {
            Offset.x = Player.go.transform.position.x;
            Offset.y = Player.go.transform.position.y;
            Offset.z = Pux.transform.position.z;
            Player.go.transform.position = Vector3.MoveTowards(Player.go.transform.position, Offset, speed * Time.deltaTime);
            //http://answers.unity3d.com/questions/179605/moving-an-object-towards-another-moving-object.html
            //Player.go.transform.position = Vector3.MoveTowards(transform.position, Pux.transform.position, speed * Time.deltaTime);          
        }
        else
        {
         */
            Player.go.transform.position = Vector3.MoveTowards(transform.position, FieldObj[0].transform.position, speed * Time.deltaTime);    
        //}
    }
}
