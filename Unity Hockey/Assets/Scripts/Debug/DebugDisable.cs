//Routine to disable an object is DBug is on

using UnityEngine;
using System.Collections;

public class DebugDisable: MonoBehaviour
{

    public GameObject obj;  //GameObject script is attached to

    void Start()
    {
        obj = this.gameObject;  //Get this Gameobject
    }

    void Update()
    {
        //En/disable object as appropriate
        if (Toolbox.Options.DBug== true)
        {
            obj.SetActive(false);
        }
        else
        {
            obj.SetActive(true);
        }
    }
}
