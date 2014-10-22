//Routine to process Player Selection scene

using UnityEngine;
using System.Collections;

public class PlayerSel : MonoBehaviour {

    public AudioSource Select;  //Selection sound emitter
    public AudioSource Play;    //"How many players are going to play today" emitter
    public const byte CLICKS=5; //# of clickable objects
    public GameObject[] Clickables = new GameObject[CLICKS]; //Array of Gameobjects of clickable objects

    RaycastHit hit; //Raycast, like always

	// Use this for initialization
	void Start () {
	    //NOP
	}
	
	// Update is called once per frame
	void Update () {

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
                        //@Assign amount of players to variable here
                        Select.audio.Play();    //Play "Let us play some hockey"
                        ExecuteAfterTime(1f);   //Delay
                        obj.audio.Play();       //Play object's audiosource
                        ExecuteAfterTime(2.5f); //Delay

                        //If netgame
                        if (i==CLICKS)
                        {
                            //@Load netgame scene for TCP/IP stuff
                            //@Application.LoadLevel("Netgame");
                        }
                        else
                        {
                            Play.audio.Play();//Play "Let us play some hockey"
                            //@Insert code here to determine which game scene to load
                            Application.LoadLevel("Game");
                        }

                    }
                }
            }
        }
	
	}

    //@DYSFUNCTIONAL delay routine
    IEnumerator ExecuteAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
    }
}
