using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TagFrenzy;
using System;

public class Bumper: MonoBehaviour
{
    //Bumper variables
    public GameObject PwrSpawn;
    public float radius_X, radius_Z;
    GameObject Bump;
    bool Alive;

    // Use this for initialization
    void Start()
    {
        Alive = true;   //@Change me after debugging!
        Bump = this.gameObject;
        Toolbox.Position.RandomizeInRegion(PwrSpawn, Bump, radius_X, radius_Z);
    }

    void OnCollisionEnter(Collision otherObj)
    {
        byte r;

        if (Alive == false)
        {
            return;
        }

        //Get list of tags
        List<Tags> OtherTags = otherObj.gameObject.tagsEnum();  //Colided object's tags

        if ((OtherTags.Contains(Tags.Puck) == true)||(OtherTags.Contains(Tags.Paddle) == true))
        {
            r = (byte)(UnityEngine.Random.Range(0, 10));
            if (r<=5)
            {
                SoundManager.Play("Bumper1");
            }
            else
            {
                SoundManager.Play("Bumper2");
            }
        }
    }

    void Kill()
    {
        Alive = false;
        Bump.SetActive(false);
    }
}
