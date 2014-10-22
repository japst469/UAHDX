//Routine to do control the paddle.
//Made by Josh

using UnityEngine;
using System.Collections;

public class PaddleController : MonoBehaviour {


    public float Height = 0.25f;
    // a struct to hold the hit data
    private RaycastHit hit;
    public float multiplier;
	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
        // Raycast and detect only collisions with the game board.

        if ( Physics.Raycast( Camera.main.ScreenPointToRay(Input.mousePosition), out hit, (1<<8) ) )
        {
            // What do do if the mouse ray collides with the game board.
            //Vector3.Lerp( transform.position, hit.point, Time.deltaTime);
            //transform.position = hit.point + new Vector3(0.0f,Height,0.0f);
            rigidbody.MovePosition(hit.point);
            //if (Input.GetMouseButton(0)) //(Vector3.Distance(transform.position, hit.point) < 2.0f)
            {
                //rigidbody.AddForce(-rigidbody.velocity);
               // Debug.Log("brakes = " + (-rigidbody.velocity).ToString());
            }
        }

        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
        {
            //Debug.Log(hit.transform.name);
            if (Input.GetMouseButton(0))
            {
                switch (hit.transform.name)
                {
                    case ("mnuNewGame"):
                        Application.LoadLevel(0);
                        break;
                    case ("mnuOptions"):
                        Debug.Log("You have selected an option that is not available yet.");
                        break;
                    case ("mnuMainMenu"):
                        Application.LoadLevel(1);
                        break;
                    default:
                        Debug.Log("You have selected an option that is not available yet.");
                        break;
                }
            }

        }
        
        //rigidbody.AddForce(Input.GetAxis("Horizontal")*1000.0f,0.0f,Input.GetAxis("Vertical")*1000.0f);
        //transform.Translate(Input.GetAxis("Mouse X") * Time.deltaTime, 0.0f, Input.GetAxis("Mouse Y") * Time.deltaTime);
        //Debug.Log(new Vector3(Input.GetAxis("Horizontal"),0.0f,Input.GetAxis("Vertical")).ToString());
	}

    void OnCollisionEnter(Collision collision)
    {
        //Vector3 _relVelocity;
        string name = collision.transform.name;
        if ( name == "Puck")
        {
            //_relVelocity = collision.transform.position - transform.position;
            // Suppress some of the y-impact.
            //_relVelocity.Scale(new Vector3(1.0f, 0.0f, 1.0f));
            //GameObject.FindGameObjectWithTag(collision.transform.name).rigidbody.AddForce(_relVelocity * 100.0f);
            //Debug.Log(collision.relativeVelocity);
            Material newMat = Resources.Load(GetComponentInChildren<MeshRenderer>().material.name, typeof(Material)) as Material;
            GameObject.FindGameObjectWithTag(name).GetComponentInChildren<MeshRenderer>().materials[0] = newMat;
            Debug.Log( this.name + " has collided with " + name );
        }

        
    }
}
