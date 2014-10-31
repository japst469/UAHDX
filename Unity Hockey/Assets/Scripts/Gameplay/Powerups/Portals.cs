using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TagFrenzy;
using System;

public class Portals : MonoBehaviour {

    public GameObject PwrSpawn;
    public float radius_X, radius_Z, height;
    public GameObject OtherPortal;
    GameObject Portal;

    bool Alive;

	// Use this for initialization
	void Start () {
        Alive = true;
        Portal = this.gameObject;
        Toolbox.Position.RandomizeInRegion(PwrSpawn, Portal, radius_X, radius_Z);
	}

    void OnCollisionEnter(Collision otherObj)
    {
        if (Alive == false)
        {
            return;
        }

        Vector3 offset;

        //Get list of tags
        GameObject Pux=otherObj.gameObject;
        List<Tags> OtherTags = Pux.tagsEnum();  //Colided object's tags
        List<Tags> PortalTags = Portal.tagsEnum();              //This tags

        if (OtherTags.Contains(Tags.Puck) == true)
        {
            if ((PortalTags.Contains(Tags.P_PortalB)==true)||(PortalTags.Contains(Tags.P_PortalR)==true))
            {
                offset.x=OtherPortal.transform.position.x;
                offset.y=OtherPortal.transform.position.y+height;
                offset.z=OtherPortal.transform.position.z;

                //Move and constrain object on Y, and with no rotation
                Pux.transform.position = offset;
                Pux.rigidbody.constraints = RigidbodyConstraints.None;
                Pux.rigidbody.useGravity = true;    //Disable gravity                
                SoundManager.Play("Teleport");  //Play Teleport sfx
            }

        }
    }

    void Kill()
    {
        Alive = false;
        Portal.SetActive(false);
    }
}
