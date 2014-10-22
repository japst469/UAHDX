//Script, on Awake of attached object
//plays a song using the SoundManager package

using UnityEngine;
using System.Collections;

public class PlaySong : MonoBehaviour
{
    //Variables editable with Unity Editor,
    //for attached object

    public string Playsong;     //String name of song to play
    public bool KillAll;        //Option to stop all sounds on play. True=do so, false=don't

    //Do it!
    void Awake()
    {
        if (KillAll == true)
        {
            SoundManager.StopAllSounds();
        }
        SoundManager.Play(Playsong);
    }
}