//Script to toggle announcer guy auidosources acording to Options
﻿using UnityEngine;
using System.Collections;

public class ToggleAnn : MonoBehaviour
{

    public GameObject obj; //Attached audiosource object

    // Use this for initialization
    void Start()
    {
        obj = this.gameObject; //Get da gameobject
    }

    // Update is called once per frame
    void Update()
    {
//Toggle the announcer
        if (Toolbox.Instance.Options.announcer== true)
        {
            obj.audio.enabled = true;
        }
        else
        {
            obj.audio.enabled = false;
        }

    }
}