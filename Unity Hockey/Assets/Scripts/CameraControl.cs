//@DEAD CODE? DELETE?

using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {

    public GameObject obj;

	// Use this for initialization
	void Start () {
        /*/ Position the camera on the puck
        obj = GameObject.FindGameObjectWithTag("Puck");
        transform.position = obj.transform.position;
        transform.Translate(0.0f,0.0f,-40.0f);
        // Look down
        transform.rotation = Quaternion.AngleAxis( 90.0f, Vector3.right );
        /*/
	}
	
	// Update is called once per frame
	void Update () {
        // position the paddle over top of the paddle
        //transform.position = obj.transform.position;
        //transform.Translate(0.0f,0.0f,-25.0f);
        
	}
}
