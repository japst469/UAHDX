//Behavior to attach/process physical goal objects

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TagFrenzy;
using System;

public class PwrHUD : MonoBehaviour
{
    public Toolbox.Color Color;
    public Texture[] Tex = new Texture[19];   //The textures for each powerup
    byte Pwr = 0;

    void PowerupHUD()
    {
        bool Change = false;

        if (Input.GetMouseButton(0) == true)
        {
            if (Pwr == 0)
            {
                Pwr = Toolbox.Consts.Num_Pwrs;
            }
            else
            {
                Pwr--;
            }
            Change = true;
        }

        if (Input.GetMouseButton(1) == true)
        {
            if (Pwr == Toolbox.Consts.Num_Pwrs)
            {
                Pwr = 0;
            }
            else
            {
                Pwr--;
            }
            Change = true;
        }

        if (Change == true)
        {
            //UpdateHUD();
        }
    }

    void UpdateHUD(Toolbox.Color Clr, byte Index, byte[] sPwr)
    {
        Tags Color;
        Color=Toolbox.Tagmgmt.ColorVal2Tag(Clr);
        
    }
}