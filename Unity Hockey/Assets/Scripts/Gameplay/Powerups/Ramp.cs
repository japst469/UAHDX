using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TagFrenzy;
using System;

public class Ramp : MonoBehaviour {

    public GameObject PwrSpawn;
    public float radius_X, radius_Z;
    GameObject ramp;

    bool Alive;

    // Use this for initialization
    void Start()
    {
        float yrot;

        Alive = true;
        ramp = this.gameObject;
        Toolbox.Position.RandomizeInRegion(PwrSpawn, ramp, radius_X, radius_Z);
        
        //UnityEngine.Random.seed = (int)System.DateTime.Now.Ticks;   //@Get new randomization seed from system time. Needs to be synce in netgames!
        //yrot=UnityEngine.Random.Range(-360,360);
        //ramp.transform.Rotate(270, yrot, 0);
    }

    void OnCollisionEnter(Collision otherObj)
    {
        if (Alive == false)
        {
            return;
        }

        //Get list of tags
        GameObject Pux = otherObj.gameObject;
        List<Tags> OtherTags = Pux.tagsEnum();  //Colided object's tags

        if (OtherTags.Contains(Tags.Puck) == true)
        {
            Pux.rigidbody.constraints = RigidbodyConstraints.None;
            Pux.rigidbody.useGravity= true;
            SoundManager.Play("Rampoff");
        }
    }

    void Kill()
    {
        Alive = false;
        ramp.SetActive(false);
    }
}
