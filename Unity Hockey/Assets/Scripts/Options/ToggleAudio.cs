//Script to toggle all audio, Announcer, Sfx, and music if all 3 options are off. Does this by disabling Audio Listener on main camera

﻿using UnityEngine;
using System.Collections;

public class ToggleAudio : MonoBehaviour {

    public GameObject obj;

    // Use this for initialization
	void Start () {
        obj = this.gameObject;
	}
	
	// Update is called once per frame
	void Update () {
        //@NOT WORKING CORRECTLY
        /*bool Sound;
        bool Music;
        bool Announcer;
        bool Audio;

        Sound=Toolbox.Instance.Options.sound;
        Music=Toolbox.Instance.Options.music;
        Announcer=Toolbox.Instance.Options.announcer;
        Audio=(((Sound) && (Music)) && (Announcer));

        if (Audio==true)
        {
            obj.GetComponent<AudioListener>().enabled = true;
        }
        else
        {
            obj.GetComponent<AudioListener>().enabled = false;
        }
        */
	}
}