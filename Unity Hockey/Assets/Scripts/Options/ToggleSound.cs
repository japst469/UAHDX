//Script to toggle sfx auidosources acording to Options

﻿using UnityEngine;
using System.Collections;

public class ToggleSound : MonoBehaviour {

    GameObject obj;

	// Use this for initialization
	void Start () {
        obj=this.gameObject;
	}
	
	// Update is called once per frame
	void Update () {
        if (Toolbox.Instance.Options.sound == true)
        {
            obj.audio.enabled = true;
        }
        else
        {
            obj.audio.enabled = false;
        }
	    
	}
}