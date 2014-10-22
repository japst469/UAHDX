//Routine to handle options GUI and variable assignments

using UnityEngine;
using System.Collections;

public class Opts : MonoBehaviour {

    public GameObject Snd_On;       //Object to emit vOn
    public GameObject Snd_Off;      //Object to emit vOff
    public GameObject Snd_Select;   //Object to emit Select
    public GameObject Snd_HiLite;   //Object to emit HiLite

    public Material Tex_Off;        //Texture for off objects
    public Material Tex_On;         //Texture for on objects
    //@ public Material Tex_Selected;   //Texture to show current options

    public TextMesh Message;        //3D text for message

    const int NUM_CATS = 6;         //# of categories
    const int NUM_ITEMS = 3;        //max # of items per category (some are dummy when needed)
    const int NUM_EASTER = 6;       //# of easter eggs

    public string[] CateArrStr = new string[NUM_CATS];              //Create string array for Categories
    public string[] CateMess = new string[NUM_CATS];                //Create string message array for Categories
    public string[] CateItemStr = new string[NUM_CATS * NUM_ITEMS]; //Create 1D string array for items
    public string[] CateItemMess = new string[NUM_CATS * NUM_ITEMS];//Create string array for item messages
    public string[] EasterArrStr = new string[NUM_EASTER];          //Create string array for Easter eggs
    public string[] EasterMess = new string[NUM_EASTER];            //Create string array for easter egg messages

    //Arrays to hold gameobjects for categories, category items, and Easter eggs
    //All dynamic, for if new options are needed
    public GameObject[] CateArr = new GameObject[NUM_CATS];         
    public GameObject[,] CateItemArr = new GameObject[NUM_CATS,NUM_ITEMS];
    public GameObject[] EasterArr = new GameObject[NUM_EASTER];    

    //Flag to determine if the Debug mode cheat is activated
    bool DBug_Flag;

    //Category enums
    enum categories
    {
        Mode,
        Diff,
        Pwr,
        Music,
        Sound,
        Ann,
        Soundtest
    }
    
    categories CateSel; //Create category enum
    RaycastHit hit;     //Raycast, like always
    
	// Use this for initialization
	void Start () {
        //Fill in all array info

        byte i=0;   //Generic counter #1
        byte j=0;   //Generic counter #2
        byte Ctr1=0; //Counter for nested FOR loop counting

        //Create array of names for Categorie GameObjects
        string[] CateArrStr=
        {
                "ModeCollider",
                "DiffCollider",
                "PwrCollider",
                "MusicCollider",
                "SoundCollider",
                "AnnCollider"
        };

        //Create messages for them
        string[] CateMess =
        {
            "Mode",
            "Difficulty",
            "Powerups",
            "Music",
            "Sound",
            "Announcer",
            "Soundtest"
        };

        //Create array of names for Categorie item GameObjects
        string[] CateItemStr=
        {
            "Mode_AirCollider",
            "Mode_PinCollider",
            "Mode_BatCollider",
            "Diff_EZCollider",
            "Diff_MedCollider",
            "Diff_HardCollider",
            "Pwr_OnCollider",
            "Pwr_OffCollider",
            "NULL",                 //Dummy; unused, just for non-jagged arrays
            "Music_OnCollider",
            "Music_OffCollider",
            "NULL",
            "Sound_OnCollider",
            "Sound_OffCollider",
            "NULL",
            "Ann_OnCollider",
            "Ann_OffCollider",
            "NULL"
        };

        //Messages for those items
        string[] CateItemMess =
        {
            "Air Hockey",
            "Pinball",
            "Battle",
            "Easy",
            "Medium",
            "Hard",
            "On",
            "Off",
            "ERROR!",   //ERROR
            "On",
            "Off",
            "ERROR!",
            "On",
            "Off",
            "ERROR!",
            "On",
            "Off",
            "ERROR!"
        };

        //GameObject names for the easter eggs
        string[] EasterArrStr=
        {
            "Easter1_Goal1Collider",
            "Easter2_Goal2Collider",
            "Easter3_UAHCollider",
            "Easter4_Stick1Collider",
            "Easter5_Stick2Collider",
            "Easter6_Options"
        };

        //Messages for each
        string[] EasterMess =
        {
            "Goal!",
            "Goal!",
            "Ultra Air Hockey Deluxe",
            "Whack",
            "Whack",
            "Options"
        };

        //Get the GameObject objects for everything

        //Categories
        for (i=0;i<=NUM_CATS-1;i++)
        {
            CateArr[i]=GameObject.Find(CateArrStr[i]);
        }

        //Easter eggs
        for (i=0;i<=NUM_EASTER-1;i++)
        {
            EasterArr[i]=GameObject.Find(EasterArrStr[i]);
        }

        //Category items
        for (i=0;i<=NUM_CATS-1;i++)
        {
            for (j = 0; j <= NUM_ITEMS - 1; j++)
            {
                Ctr1 = (byte)((3*i)+j);
                CateItemArr[i,j] = GameObject.Find(CateItemStr[Ctr1]);
            }
        }
        
        //Initialize message
        Message.text = "Options Menu";
	}

    // Update is called once per frame
    void Update()
    {
        Cates();        //Process categories
        CateItems();    //Process category items
        //@ Create routine to show current options for non-selected categories!
    }    
    
    //Routine to process categories
    void Cates()
    {
        //Do click processing
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
        {
            byte i=0;   //Generic counter
            bool SearchMore=true;   //FOR loop Short-circuit flag
            string name = hit.transform.name;   //Name of clicked object
            GameObject obj = hit.transform.gameObject; //Gameobject of clicked object

            //Process all categories
            for (i=0;i<=NUM_CATS-1;i++)
            {
                if (obj==CateArr[i])    //If clicked object is currently being processed, then...
                {
                    if (Input.GetMouseButton(0))    //And if actually clicked
                    {
                        CateSel = (categories)i;    //Change the CateSel variable
                        UpdateCate(obj,(categories)i);  //Update hilighting of category
                        HiLight(obj);                   //Play sounds
                        SearchMore=false;               //Don't process anymore categories
                        break;                          //break FOR loop
                    }                    
                }
            }

            if (SearchMore==true)                       //If need to process more, then process easter eggs
            {
                //Process easter eggs
                for (i=0;i<=NUM_EASTER-1;i++)
                {
                    //Some coding format...
                    if (obj==EasterArr[i]) 
                    {
                        if (Input.GetMouseButton(0))
                        {
                            Select(obj);            //Play 
                            SearchMore=false;       //Don't process anymore easter eggs
                            
                            //@Show Easter egg's message. NOT WORKING, due to illogical array issue...
                            //Debug.Log("EasterMess" + i);
                            //Message.text = EasterMess[i];
                            break;                  //Break FOR loop
                        }                        
                    }
                }
            }

            if (SearchMore==true)   //If still processing, do extra easter eggs
            {
                //Process special easter eggs by name
                switch (name)
                {           
                    //Collider to enable debug mode secret
                    case ("DebugCollider"):
                        if (Input.GetMouseButton(0))
                        {
                            DBug_Flag=true;     //Turn flag to true
                            Select(obj);
                            Message.text = "DEBUG MODE";    //Show message
                        }
                        break;

                    //If clicked the secret, change options.dbug to true!
                    case ("DebugOnCollider"):
                        if ((Input.GetMouseButton(0))&&(DBug_Flag==true))
                        {
                            Select(obj);
                            Message.text = "Secret Found!";     //Show message
                            Application.LoadLevel("EasterEgg"); //@Change to hidden scene
                        }
                        break;

                    default:
                        //@Implement options.dbug feature here
                        if (Input.GetMouseButton(0))
                        {
                            Debug.Log("You have selected an option from the menu that is not available yet.");
                        }
                        break;
                }
            }

            //@Implement options.dbug feature here
            if (false==true)
            {
                Debug.Log(name);
            }
        }
    }

    //Routine to update hilighting of categories
    //Input gameobject of selection, and its categories enum ID
    void UpdateCate(GameObject obj, categories ID)
    {
        byte i=0;   //Generic counter
        bool[] bitfield = new bool[NUM_CATS];   //Bitfield to de/enable hilighting
        
        Snd_HiLite.audio.Play();    //Play hilite sound
        ExecuteAfterTime(0.2f);     //Delay
        
        //Process categories
        for (i=0; i <=NUM_CATS-1; i++)
        {
            //Turn on only selection, turn off others
            if (i != (byte)ID)
            {
                bitfield[i] = false;
            }
            else
            {
                bitfield[i] = true;
            }

            //Change textures as appropriate
            if (bitfield[i] == true)
            {
                CateArr[i].gameObject.renderer.material = Tex_On;
                //@Show message of category selection. NOT WORKING
                //Debug.Log("CateMess" + i);
                //Message.text = CateMess[i];
            }
            else
            {
                CateArr[i].gameObject.renderer.material = Tex_Off;
            }    
        }        
        
        obj.audio.Play();   //Play select object's audiosource
    }

    //Routine to process category items
    void CateItems()
    {

        //Check if cursor is hilighing stuff
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
        {
            bool SearchMore=true;   //FOR short-circuit flag
            string name = hit.transform.name;   //Name of raycasted object
            GameObject obj = hit.transform.gameObject;  //Gameobject of it
            byte i=0;   //Generic counter
            byte j=0;   //Generic counter
            byte temp=0;//Dummy variable to get value for options variable to change

            //Process all categories
            for (i = 0; i <= NUM_CATS - 1; i++)
            {
                
                //Enable clicking of items for selected category
                if (CateSel == (categories)i)
                {
                    //Process it's items
                    for (j = 0; j <= NUM_ITEMS - 1; j++)
                    {
                        if (Input.GetMouseButton(0))
                        {
                            //If current item being process was clicked...
                            if (obj == CateItemArr[i, j])
                            {
                                SearchMore = false;     //Stop searching
                                UpdateItems(obj, i, j); //Update item hilighting
                                temp = j;               //Get value to push to appropriate options variable
                            }

                            if (SearchMore == false)
                            {
                                break;  //Stop searching if necessary
                            }
                        }
                    }
                }

                //Stop searching if necessary
                if (SearchMore == false)
                {
                    break;
                }
            }

            //@Insert Code to assign temp to proper options variables here!
            //@Also insert code to NOT() booleans for on/off options

        }
    }

    //Routine to process hilithign of items
    //INputs: gameobject clicked, category ID, item ID
    void UpdateItems(GameObject obj, byte ID1, byte ID2)
    {
        
        byte i=0;   //}
        byte j = 0; //}Generic counters
                    //}

        byte Ctr1=0;    //FOR loop counter
        bool Clicked=false; //Determine clicking? IDR

        Snd_Select.audio.Play();    //Play selection sfx
        ExecuteAfterTime(2f);       //Delay

        //Process categories, then items
        for (i = 0; i <= NUM_CATS - 1; i++)
        {
            for (j = 0; j <= NUM_ITEMS - 1; j++)
            {
                //Turn textures on/off as approrpiately 
                if (CateItemArr[i, j]!=CateItemArr[ID1, ID2])
                {
                    CateItemArr[i, j].gameObject.renderer.material = Tex_Off;
                }
                else
                {
                    CateItemArr[i, j].gameObject.renderer.material = Tex_On;
                    Ctr1=(byte)((3*i)+j);   //Get 1D equilvanet of 2D array for items
                    Clicked = true;         //Flag to prevent errors IIRC
                }                
            }
        }

        obj.audio.Play();   //Play selected object item's audiosource
        if (Clicked == false)
        {
            //NOP
        }
        else
        {
            //@If clicked, show message. NOT WORKING
            //Debug.Log("CateItemMess" + Ctr1);
            //Message.text = CateItemMess[Ctr1];
        }
    }

    //Routine to play hilight sound
    void HiLight(GameObject obj)
    {
        Snd_HiLite.audio.Play();    //Play hilite sound
        ExecuteAfterTime(0.02f);    //Delay
        obj.audio.Play();           //Play hilited object's audiosource
        ExecuteAfterTime(2f);       //Delay
    }

    //Routine to play select sound
    void Select(GameObject obj)
    {
        Snd_Select.audio.Play();    //Play select sound
        ExecuteAfterTime(1f);       //Delay
        obj.audio.Play();           //Play selected object's audiosource
        ExecuteAfterTime(2f);       //Delay
    }

    //@Delay routine, which DOES NOT WORK!
    IEnumerator ExecuteAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
    }

}