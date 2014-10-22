//Script does opening main menu stuff, related to intro sounds.
//@Make it run ONCE PER BOOT!

using UnityEngine;
using System.Collections;

public class InitTitle : MonoBehaviour {    
    //@TODO: Add runonce instance, so it only plays on 1st boot!
	void Start () {
        //Play 3 sounds
        SoundManager.StopAllSounds();   //Stop all sounds
        SoundManager.Play("vUAHDX");    //"Ultra Air Hockey DX"
        SoundManager.Play("Siren",1.7f);   //Siren sfx
        SoundManager.Play("main_hockey_fever_(looped)", 6.5f);  //Play music
	}
}
