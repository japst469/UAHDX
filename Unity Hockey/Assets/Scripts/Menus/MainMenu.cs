//Script processes main menu item selection

using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {
   
    public RaycastHit hit;             //Raycast object
                   
    void Update()
    {
        //Determine if ray has been cast from mouse, then...
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
        {
            var name = hit.transform.name;      //Get name of object raycasted

            //Do something based on object's name
            switch (name)
            {
                //If main menu button
                case ("MainMenuOptionCollider"):
                    //AND if left mouse btn was clicked
                    if (Input.GetMouseButton(0))
                    {
                        SoundManager.Play("Select");    //Play Select sfx
                        Application.LoadLevel("MainMenu");  //Load appropriate scene
                    }
                    break;

                //And so on in format for other menu items

                //Options
                case ("PlayOptionCollider"):
                    if (Input.GetMouseButton(0))
                    {
                        SoundManager.Play("Select");
                        SoundManager.Play("vPlay",2.0f);    //"Let us play some Hockey!"
                        Application.LoadLevel("PlayerSel"); //Load Player Selection scene
                    }
                    break;
                    
                //Credits
                case ("CreditsOptionCollider"):
                    if (Input.GetMouseButton(0))
                    {
                        SoundManager.Play("Select");
                        SoundManager.Play("vCredits", 1.0f);    //"Credits"
                        Application.LoadLevel("Credits");       //Load Credits scene
                    }
                    break;

                //Options
                case ("OptionsOptionCollider"):
                    if (Input.GetMouseButton(0))
                    {
                        SoundManager.Play("Select");
                        SoundManager.Play("vOptions", 1.0f);    //"Options"
                        Application.LoadLevel("Options");       //Load Options Scene
                    }
                    break;
                
                //Quit
                case ("QuitOptionsCollider"):
                    if (Input.GetMouseButton(0))
                    {
                        SoundManager.Play("Select");
                        SoundManager.Play("vGameover", 1.5f);   //"Gameover" (with portamento down)
                        Application.Quit();                     //Quit teh game!
                    }
                    break;

                //Debug button to skip EagleSoft Ltd FMV
                case ("Skip_FMV_1_Collider"):
                    if (Input.GetMouseButton(0))
                    {
                        SoundManager.Play("Select");
                        Application.LoadLevel("FMV2");      //Load 2nd FMV
                    }
                    break;
                
                //Anything else
                default:                    
                    //If DBug on...
                    if (Toolbox.Options.DBug == true)
                    {
                            if (Input.GetMouseButton(0))
                            {
                                //Show message, report object name
                                Debug.Log("You have selected an option from the menu that is not available yet.");
                                Debug.Log(name);
                            }
                    }
                    break;
            }

        }
    }
}
