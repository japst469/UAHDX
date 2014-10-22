//Script "borrowed"
//from somewhere. IDR where

//Causes camera to point towards an object, and revolve in circle around it
//Need to select target GameObject in Editor

using UnityEngine;
using System.Collections;

public class CameraRevolve : MonoBehaviour {

    public Transform target;
    public int speed;
     
    // Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        transform.LookAt(target);
        transform.Translate(Vector3.right * speed *Time.deltaTime);
	}
}
