//Script processes main menu item selection

using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

    public GameObject Select;   //Object that emits Select sfx        
    RaycastHit hit;             //Raycast object
    
	void Start () {
	    //NOP
	}
    
    void Update()
    {
        //Determine if ray has been cast from mouse, then...
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
        {
            var name = hit.transform.name;      //Get name of object raycasted
            var obj = hit.transform.gameObject; //Get GameObject of object raycasted

            //Do something based on object's name
            switch (name)
            {
                //If main menu button
                case ("MainMenuOptionCollider"):
                    //AND if left mouse btn was clicked
                    if (Input.GetMouseButton(0))
                    {
                        PlaySound(obj); //Do sounds
                        Application.LoadLevel("MainMenu");  //Load appropriate scene
                    }
                    break;

                //And so on in format for other menu items

                //Optoins
                case ("PlayOptionCollider"):
                    if (Input.GetMouseButton(0))
                    {
                        PlaySound(obj);
                        Application.LoadLevel("PlayerSel");
                    }
                    break;

                //Credits
                case ("CreditsOptionCollider"):
                    if (Input.GetMouseButton(0))
                    {
                        PlaySound(obj);
                        Application.LoadLevel("Credits");
                    }
                    break;

                //Options
                case ("OptionsOptionCollider"):
                    if (Input.GetMouseButton(0))
                    {
                        PlaySound(obj);
                        Application.LoadLevel("Options");
                    }
                    break;
                
                //Quit
                case ("QuitOptionsCollider"):
                    if (Input.GetMouseButton(0))
                    {
                        PlaySound(obj);
                        Application.Quit();
                    }
                    break;

                //Debug button to skip EagleSoft Ltd FMV
                case ("Skip_FMV_1_Collider"):
                    if (Input.GetMouseButton(0))
                    {
                        PlaySound(obj);
                        Application.LoadLevel("FMV2");
                    }
                    break;

                default:

                    //@Add feature to use Options.DBug variable for this
                    if (Input.GetMouseButton(0))
                    {
                        Debug.Log("You have selected an option from the menu that is not available yet.");
                    }
                    Debug.Log(name);
                    break;
            }

        }
    }

    //Method to play appropriate sounds for each menu
    void PlaySound(GameObject obj)
    {
        Select.audio.Play();    //Play "Select" sfx
        ExecuteAfterTime(1f);   //Delay 1 sec
        obj.audio.Play();       //Play the object's attached audiosource
        ExecuteAfterTime(2f);   //Wait 2 sec
    }

    //@Attempt (and fail) to delay for xf sec
    IEnumerator ExecuteAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
    }


}
