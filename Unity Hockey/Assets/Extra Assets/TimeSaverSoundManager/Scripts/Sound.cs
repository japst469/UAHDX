//----------------------------------------------
//            TSSM: TimeSaver SoundManager
// 		   Created by: Murillo Pugliesi Lopes
//----------------------------------------------

using UnityEngine;
using System.Collections;

/// <summary>
/// Used to hold all Sound information, such as name, clip and all stuff. Also has a isMuted variable to check if the sound is muted or not.
/// </summary>

[System.Serializable]
public enum soundTrack {
	BackgroundSound = 0,
	EffectSound = 1,
	VoiceSound =2
}
public enum track{
	BackgroundSound = 0,
	EffectSound = 1,
	VoiceSound = 2,
	All = 3
}

[System.Serializable]
public class Sound : ScriptableObject{
	
	[SerializeField]
	public AudioClip clip;
	[SerializeField]
	public new string name;
	[SerializeField]
	public float volume;
	[SerializeField]
	public soundTrack track;
	[SerializeField]
	public bool loop;
	[SerializeField]
	public bool fadeIn;
	[SerializeField]
	public float timeToFadeIn;
	[SerializeField]
	public float controllerFadeIn;
	[SerializeField]
	public bool fadeOut;
	[SerializeField]
	public float timeToFadeOut;
	[SerializeField]
	public bool _3D;
	[SerializeField]
	public Vector3 v3Target;
	[SerializeField]
	public GameObject tTarget;
	[SerializeField]
	public bool followObject;
	[SerializeField]
	public bool trigger;
	[SerializeField]
	public GameObject targetForTrigger;
	[SerializeField]
	public float timeToTrigger;
	[SerializeField]
	public string language;
	[SerializeField]
	public string[] allLenguages;
	[SerializeField]
	public int languageIndexHolder;
	
	[SerializeField]
	public AudioSource currentSource;
	
	public bool isPreviwing;
	public float stopAt;
	public bool showCarac;
	public float controllerFadeOut;
	public float fadeOutTimer;
	public float delayToPlay;
	public float maxVolume;
	public bool isMuted;
	public float lastVolumeSetted;
	public bool canShootEvent;
	
	private float oldVolumeSetted;
	
	// Use this for initialization
	void Start () {
		oldVolumeSetted = currentSource.volume;
	}
	
	public void Stop(){
		if(this.currentSource)
			this.currentSource.Stop();	
	}
	public void Mute(bool value){
		if(value)
			oldVolumeSetted = this.currentSource.volume;
		
		isMuted = value;
		this.currentSource.volume = (value) ? 0 : oldVolumeSetted;
	}
	public void Pause(bool value){
		this.currentSource.Pause();
	}
	public void Volume(int value){
		
		this.currentSource.volume = Mathf.Clamp(value, 0, 1);
	}
}
