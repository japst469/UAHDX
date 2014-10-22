//DEAD CODE? Debug stuff

using UnityEngine;
using System.Collections;

public class TableController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    void OnCollisionEnter(Collision collision)
    {
        string name = collision.transform.name;
        if (name == "Puck")
        {
            
            Debug.Log(this.name + " has collided with " + name);
        }
    }
}
