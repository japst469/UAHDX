//Routine to process Player Selection scene

//@This routine is under HEAVY development
//@and needs commented, cleaned up, and fully tested!

using UnityEngine;
using System.Collections;

public class PlayerSel : MonoBehaviour
{
    public const byte CLICKS = 5; //# of clickable objects  
    public Material Tex_Disabled;    //Texture to show for invalid clickables
    public GameObject[] Clickables = new GameObject[CLICKS]; //Array of Gameobjects of clickable objects

    public bool[] Clickbit = new bool[CLICKS];  //Bitfield describing what Clickables are available for mode selection
    public bool[] TwoPlayers = new bool[CLICKS];    //Bitfield constant for two players
    public bool[] FourPlayers = new bool[CLICKS];    //Bitfield constant for four players
    bool thereIsConnection = false;    //Determines if there is an active internet connection
    byte max_all;       //total amount of players & CPUs for mode
    byte max_players;   //total amount of players
    byte max_CPUs;      //total amount of CPUs
    RaycastHit hit; //Raycast, like always

    // Use this for initialization
    //Routine to load the approriate # of players selectable, according to mode type
    //@None of this is working properly!
    void Start()
    {
        Application.LoadLevel("Game");  //@Jump straight into game, for now, until script cleaned
        //SoundManager.Play("vPlayers");
        /*string temp;
        byte i;

        //Initialize clickbit to shutup nagging compilerass
        bool[] Clickbit =
        {
            false,
            false,
            false,
            false,
            false
        };

        bool[] TwoPlayers =
        {
            true,
            true,
            false,
            false,
            false
        };

        bool[] FourPlayers =
        {
            true,
            true,
            true,
            true,
            false
        };

        //Setup applicable # of players selectable for each mode
        switch (Toolbox.Options.Mode)
        {
            case mode.Pinball:
                max_all=2;
                TwoPlayers.CopyTo(Clickbit, 0);
                break;
            case mode.Battle:
                max_all=4;
                FourPlayers.CopyTo(Clickbit, 0);
                break;
            default:    //Air hockey mode
                max_all=2;
                TwoPlayers.CopyTo(Clickbit, 0);
                break;
        }

        if (Toolbox.Options.DBug==true)
        {
            temp = string.Empty;
            for (i = 0; i <= CLICKS; i++)
            {
                if(Clickbit[i]==false)
                {
                    temp+="0,";
                }
                else
                {
                    temp+="1,";
                }
            }
            Debug.Log("Bitfield: "+temp);
        }
       
        //Dis/enable netgame option dependent if internet connection is present
        //@Does not work!
        if (thereIsConnection==true)
        {
            Clickbit[CLICKS]=true;
        }
        else
        {
            Clickbit[CLICKS]=false;
        }

        //Dis/enable textures based on what is dis/enabled
        //@Dis/enabling mesh renderer Does not work!
        for (i = 0; i <= CLICKS; i++)
        {
            switch (Clickbit[i])
            {
                case false:
                    Clickables[i].renderer.material = Tex_Disabled;
                    Clickables[i].GetComponent<MeshRenderer>().enabled = true;
                    break;                    
                default:
                    Clickables[i].GetComponent<MeshRenderer>().enabled = true;
                    break;
            }
        }
         */

    }

    // Update is called once per frame
    void Update()
    {
        //Process clicking
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
        {
            byte i = 0; //Generic counter
            string name = hit.transform.name;   //Raycasted object's name
            GameObject obj = hit.transform.gameObject;  //Object's GameObject

            //Process the items
            for (i = 0; i <= CLICKS - 1; i++)
            {
                if (obj == Clickables[i])
                {
                    if (Input.GetMouseButton(0))
                    {
                        SoundManager.Play("Select",1f);    //Play Selection sfx

                        //Determine if the amount of players selected was applicable
                        //@Does not work
                        
                        if (Clickbit[i] == false)
                        {
                            SoundManager.Play("Buzzer",1.6f);
                        }
                        else
                        {
                        /*/
                            obj.Play(2.5f);       //Play object's audiosource

                            //If netgame
                            if (i == CLICKS)
                            {
                                //Load netgame scene for TCP/IP stuff
                                Application.LoadLevel("Netgame");
                                
                            }
                            else
                            {
                                //Allocate amount of players and load game scene, as appropriate                                
                                max_players = i++;
                                max_CPUs = (byte)(max_all - max_players);

                                for (i=0;i<=CLICKS-1;i++)
                                {
                                    if (i > max_all)
                                    {
                                        initPlayer(i, false, false);
                                    }
                                    else
                                    {
                                        if (i <=max_players)
                                        {
                                            initPlayer(i, true, false);
                                        }
                                        else
                                        {
                                            initPlayer(i, true, true);
                                        }
                                    }
                                }

                                SoundManager.Play("vPlay",1f);//Play "Let us play some hockey"

                                //@TODO, Load the appropriate mode / assets based on mode
                                switch ((Toolbox.mode)i)
                                {                                    
                                    case mode.Air:
                                        Application.LoadLevel("");
                                        break;
                                    case mode.Pinball:
                                        break;
                                    case mode.Battle:
                                        break;
                                                        
                                    //@Force scene to "Game", for now...
                                    default:
                                        Application.LoadLevel("Game");
                                        break;
                                }
                            }
                           /*/
                        }
                    }
                }
            }
        }
    }
    
    /*/
    void initPlayer(byte index, bool enable,bool isCPU)
    {
        byte i;
        Toolbox.Color clr;
        bool[] blnNada = new bool[Toolbox.Consts.Num_Pwrs];
        byte[] bytNada = new byte[Toolbox.Consts.Num_Pwrs];
        for (i = 0; i <= Toolbox.Consts.Num_Pwrs; i++)
        {
            blnNada[i] = false;
            bytNada[i] = 0;
        }        

        if (enable==true)
        {
            Toolbox.Players[index].Alive = true;
        }
        else
        {
            Toolbox.Players[index].Alive = false;
        }
        blnNada.CopyTo(Toolbox.Players[index].aPwr, Toolbox.Consts.Num_Pwrs);

        switch(index)
        {
            case 0:
                clr = Toolbox.Color.Red;
                break;
            case 1:
                clr = Toolbox.Color.Blue;
                break;
            case 2:
                clr = Toolbox.Color.Green;
                break;
            case 3:
                clr = Toolbox.Color.Yellow;
                break;
            default:
                clr = Toolbox.Color.Null;
                break;
        }

        //If player is disabled, assign no colors; otherwize, allocate colors are appropriately
        if (Toolbox.Players[index].Alive == true)
        {
            Toolbox.Players[index].Color = clr;
        }
        else
        {
            Toolbox.Players[index].Color = Toolbox.Color.Null;
        }
        
        Toolbox.Players[index].Score = 0;
        bytNada.CopyTo(Toolbox.Players[index].sPwr, Toolbox.Consts.Num_Pwrs);

        if (Toolbox.Players[index].Alive == false)
        {
            Toolbox.Players[index].Type = Toolbox.PlayerType.Null;
        }
        else
        {
            if (isCPU == true)
            {
                Toolbox.Players[index].Type = Toolbox.PlayerType.CPULocal;
            }
            else
            {
                Toolbox.Players[index].Type = Toolbox.PlayerType.PlayerLocal;
            }
        }
    }
    /*/

    /*
    //Test internet connection
    //Post #1 at
    //http://forum.unity3d.com/threads/how-can-you-tell-if-there-exists-a-network-connection-of-any-kind.68938/
    IEnumerator TestConnection()
    {

        float timeTaken = 0.0F;
        float maxTime = 2.0F;

        while (true)
        {
            Ping testPing = new Ping("74.125.79.99");

            timeTaken = 0.0F;

            while (!testPing.isDone)
            {

                timeTaken += Time.deltaTime;


                if (timeTaken > maxTime)
                {
                    // if time has exceeded the max
                    // time, break out and return false
                    thereIsConnection = false;
                    break;
                }

                yield return null;
            }
            if (timeTaken <= maxTime) thereIsConnection = true;
            yield return null;
        }
    }
    */

}