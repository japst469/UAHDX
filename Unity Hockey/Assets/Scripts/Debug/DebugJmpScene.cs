//Routine which skips to inputted Scene

using UnityEngine;
using System.Collections;

public class DebugJmpScene : MonoBehaviour
{
    public string Scene;    //String of Scene to load. Gotten from editor

    void Update()
    {
        //If DBug is on, skip to scene
        if (Toolbox.Options.DBug == true)
        {
            Application.LoadLevel(Scene);
        }
    }
}