//Script does opening main menu stuff, related to intro sounds.
//@Supposed to RUNS ONCE PER BOOT, but doesn't!

using UnityEngine;
using System.Collections;

public class InitTitle : MonoBehaviour {
    
    public GameObject Siren;       //The Siren sfx audiosource
    public GameObject UAHDX;        //"UAHDX" announcer
    public GameObject Music;       //The mainmenu music audiosource

    //@TODO: Add runonce instance, so it only plays on 1st boot!

	void Start () {
        //Play 3 sounds
        UAHDX.audio.Play();  //Play "Ultra Air Hockey Deluxe" announcer
        ExecuteAfterTime(2f);   //Wait 2 seconds
        Siren.audio.Play();     //Play siren sfx
        ExecuteAfterTime(7f);   //Wait 7 seconds
        Music.audio.Play();     //Play main menu music (Hockey Fever Loop)
	}
	
	// Update is called once per frame
	void Update () {
	    //NOP
	}

    //@Delay co-routine. NOT WORKING, EVER!
    IEnumerator ExecuteAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
    }
}
