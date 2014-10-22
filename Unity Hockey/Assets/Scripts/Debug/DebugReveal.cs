//Routine to reveal an invis object if DBug is on

using UnityEngine;
using System.Collections;

public class DebugReveal : MonoBehaviour
{

    GameObject obj; //Gameobject to reveal

    // Use this for initialization
    void Start()
    {        
        obj = this.gameObject;  //Get this gameobject
        if (obj.GetComponent<MeshFilter>().mesh == null)
        {
            CreateCube();   //If don't have mesh filter, create a cube
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        //Enable/disable mesh renderer, as appropriate
        if (Toolbox.Options.DBug == true)
        {
            obj.GetComponent<MeshRenderer>().enabled= true;
        }
        else
        {
            obj.GetComponent<MeshRenderer>().enabled = false;
        }
         
    }
    
    //Create a cube
    void CreateCube()
    {
        GameObject Cube;
        Cube=GameObject.CreatePrimitive(PrimitiveType.Cube);
        Cube.transform.position=obj.transform.position;
    }
    
}