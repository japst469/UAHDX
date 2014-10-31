using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TagFrenzy;
using System;

public class Wall_Piece : MonoBehaviour
{
    GameObject Wall;
    bool Alive;
    byte HP;

    // Use this for initialization
    void Start()
    {
        Alive = true;
        HP = 3;
        Wall = this.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (Alive == false)
        {
            return;
        }

        if (HP <= 0)
        {
            Kill();
        }
    }

    void OnCollisionEnter(Collision otherObj)
    {
        if (Alive == false)
        {
            return;
        }

        //Get list of tags
        List<Tags> OtherTags = otherObj.gameObject.tagsEnum();  //Colided object's tags
        
        if (OtherTags.Contains(Tags.Puck)==true)
        {
            if (Toolbox.Options.Mode == Toolbox.mode.Pinball)
            {
                SoundManager.Play("DeflectP_S");
            }
            else
            {
                SoundManager.Play("DeflectH_S");
            }
            HP--;
        }
    }

    void Kill()
    {
        Alive = false;
        Wall.SetActive(false);
        SoundManager.Play("PwrUse");
    }
}
