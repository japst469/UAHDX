//----------------------------------------------
//            TSSM: TimeSaver SoundManager
// 		   Created by: Murillo Pugliesi Lopes
//----------------------------------------------

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.Reflection;
using System.IO;
using System.Linq;


[CustomEditor (typeof(SoundManager))]
public class TSSoundManagerEditor : Editor {
	
	private SerializedObject _sManager;
	private SoundManager _soundManager;
	
	private List<string> holderBkgOpt = new List<string>();
	private List<Transform> todelete = new List<Transform>();
	private string[] bkgOpt = new string[0];
	private int choiseBkgOpt, qnt, choosedIndexLanguage, languageIndexHolder;
	private bool showEfx = false, showBkg = true, showV3 = true, showTrans = false;
	private bool showVoice = false;
	private string[] LanguageNames = new string[12]{"Sumerian","Egyptian","Akkadian","Eblaite","Elamite","Hurrian","Hittite","Greek","Luwian","Hattic","Ugaritic","Old Chinese"};
	private int indexLanguage = 0;
	private SerializedProperty _fpsInitial;
	private SerializedProperty _useSame;
	private SerializedProperty _usePool;
	private SerializedProperty _qntPool;
	private SerializedProperty _pool;
	private SerializedProperty _bkgVolume;
	private SerializedProperty _playAtStart;
	private SerializedProperty _efxVolume;
	private SerializedProperty _voiceVolume;
	private SerializedProperty _nameBkgStart;
	private SerializedProperty _AllSoundsHolderSize, _AllLanguagesHolderSize;
	private SerializedProperty _useMultiLanguage;
	private SerializedProperty _AllLanguages;
	private SerializedProperty _startingLanguage, _startingLanguageIndex;
	
	private SerializedProperty _AllSoundsHolder;
	
	public void OnEnable()
    {
		 _soundManager = (SoundManager)target;
		
		_sManager = new SerializedObject(target);
		_fpsInitial = _sManager.FindProperty("fpsInitial");
		_fpsInitial.intValue = 100;
		
		_useSame = _sManager.FindProperty("useSameSM");
		_usePool = _sManager.FindProperty("usePool");
		_qntPool = _sManager.FindProperty("qntPool");
		_pool = _sManager.FindProperty("pool");
		_bkgVolume = _sManager.FindProperty("bkgVolume");
		_bkgVolume.floatValue = 1.0f;
		_playAtStart = _sManager.FindProperty("playAtStart");
		_efxVolume = _sManager.FindProperty("efxVolume");
		_efxVolume.floatValue = 1.0f;
		_voiceVolume = _sManager.FindProperty("voiVolume");
		_AllSoundsHolder = _sManager.FindProperty("AllSoundsHolder");
		_nameBkgStart = _sManager.FindProperty("nameBkgStart");
		_AllSoundsHolderSize = _sManager.FindProperty("AllSoundsHolder.Array.size");
		_AllLanguagesHolderSize = _sManager.FindProperty("AllLanguages.Array.size");
		_useMultiLanguage = _sManager.FindProperty("useMultiLanguage");
		_AllSoundsHolder = _sManager.FindProperty("AllSoundsHolder");
		_AllLanguages = _sManager.FindProperty("AllLanguages");
		_startingLanguage = _sManager.FindProperty("startingLanguage");
		_startingLanguageIndex = _sManager.FindProperty("startingLanguageIndex");
    }
	
	private void PlayClip(AudioClip clip) {
	     Assembly unityEditorAssembly = typeof(AudioImporter).Assembly;
	     System.Type audioUtilClass = unityEditorAssembly.GetType("UnityEditor.AudioUtil");
	     MethodInfo method = audioUtilClass.GetMethod(
	         "PlayClip",
	         BindingFlags.Static | BindingFlags.Public,
	         null,
	         new System.Type[] {
	             typeof(AudioClip)
	         },
	         null
	     );
	  
	     method.Invoke(
	         null,
	         new object[] {
	             clip
	         }
	     );
	 }
	private void StopClip(AudioClip clip) {
	  
	     Assembly unityEditorAssembly = typeof(AudioImporter).Assembly;
	     System.Type audioUtilClass = unityEditorAssembly.GetType("UnityEditor.AudioUtil");
	     MethodInfo method = audioUtilClass.GetMethod(
	         "StopClip",
	         BindingFlags.Static | BindingFlags.Public,
	         null,
	         new System.Type[] {
	             typeof(AudioClip)
	         },
	         null
	     );
			method.Invoke(
	         null,
	         new object[] {
	             clip
	         }
	     );
	}
	
	private void RemoveSound(int index){
		_soundManager.AllSoundsHolder.RemoveAt(index);
	}
	private void removeElementFromLanguage(int index){
		ScriptableObject.DestroyImmediate(_soundManager.AllLanguages[index]);
		_soundManager.AllLanguages.RemoveAt(index);
		_AllLanguagesHolderSize.intValue--;
		
		Debug.Log("entrei: "+index+"          size "+_AllLanguagesHolderSize.intValue);
	}
	
	public override void OnInspectorGUI()
    {
		_sManager.Update();
		
		EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
		GUILayout.Label("Properties of Sound Manager");
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.HelpBox("If true, this GameObject won't be destroyed when you load another scene, so you can setup only once your sounds.",MessageType.Info);
		_useSame.boolValue = GUILayout.Toggle( _useSame.boolValue, "Use this forever");
		
		EditorGUILayout.BeginHorizontal();
		_fpsInitial.intValue = EditorGUILayout.IntField("Project FPS: ",_fpsInitial.intValue);
		if(_fpsInitial.intValue < 1) _fpsInitial.intValue = 1;
		EditorGUILayout.EndHorizontal();
		
		EditorGUILayout.HelpBox("You can use this to avoid instantiating objects for each sound.It's recommended for mobile development.\nKeep in mind that if all your pool objects are playing, and you want to play another AudioClip, it won't work, they're all busy. You can have more pool objects setting a higher value to Pool Quantity.",MessageType.Info);
		_usePool.boolValue = GUILayout.Toggle(_usePool.boolValue,"Use Pool Feature");
		if(_usePool.boolValue){
			_qntPool.intValue = EditorGUILayout.IntField("Pool Quantity",_qntPool.intValue);
			if(_qntPool.intValue <= 0) _qntPool.intValue = 1;
			if(_qntPool.intValue > 100) _qntPool.intValue = 100;
			if(_pool.arraySize != _qntPool.intValue){
				if(_pool.arraySize > 0){
					for(int i = 0;i<_pool.arraySize;i++)
						GameObject.DestroyImmediate(_pool.GetArrayElementAtIndex(i).objectReferenceValue as GameObject);
				}
				_pool.ClearArray();
				
				for(int i = 0;i<_qntPool.intValue;i++){
					GameObject newGO = new GameObject("AudioEmitter");
					newGO.transform.parent = _soundManager.transform;
					newGO.AddComponent("AudioSource");
					_pool.arraySize++;
					_pool.GetArrayElementAtIndex(_pool.arraySize-1).objectReferenceValue = newGO;
				}
			}
		}else{
			if(!EditorApplication.isPlaying){
				todelete = new List<Transform>();
				foreach (Transform child in _soundManager.transform){
					todelete.Add(child);
				}
				for(int i= 0;i< todelete.Count;i++){
					GameObject.DestroyImmediate(todelete[i].gameObject);	
				}
				_pool.ClearArray();
			}
		}
		
		EditorGUILayout.BeginHorizontal();
		_useMultiLanguage.boolValue = GUILayout.Toggle(_useMultiLanguage.boolValue, "Use Multi-Language Feature");
		EditorGUILayout.EndHorizontal();
		
		if(_useMultiLanguage.boolValue){
			EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
			GUILayout.Label("Multi-Language Manager");
			GUILayout.FlexibleSpace();
			if(GUILayout.Button("Add Language",EditorStyles.toolbarButton)){
				_AllLanguagesHolderSize.intValue++;
				indexLanguage++;
				indexLanguage = (indexLanguage>11) ? 0 : indexLanguage;
				Sound toGetName = ScriptableObject.CreateInstance<Sound>();
				toGetName.name = LanguageNames[indexLanguage];
				_AllLanguages.GetArrayElementAtIndex(_AllLanguagesHolderSize.intValue-1).objectReferenceValue = toGetName;
				(_AllLanguages.GetArrayElementAtIndex(_AllLanguagesHolderSize.intValue-1).objectReferenceValue as Sound).name = LanguageNames[indexLanguage];
			}
			EditorGUILayout.EndHorizontal();
			for(int i = 0;i< _AllLanguagesHolderSize.intValue;i++){
				GUILayout.BeginHorizontal();
				
				(_AllLanguages.GetArrayElementAtIndex(i).objectReferenceValue as Sound).name = GUILayout.TextField((_AllLanguages.GetArrayElementAtIndex(i).objectReferenceValue as Sound).name);
				if(GUILayout.Button("remove", new GUILayoutOption[] {GUILayout.Width(100),GUILayout.Height(16), GUILayout.ExpandWidth(false)})){
					removeElementFromLanguage(i);
					return;
				}
				GUILayout.EndHorizontal();
			}
			
			if(_AllLanguagesHolderSize.intValue > 0){
				GUILayout.BeginHorizontal();
				GUILayout.Label("Choose the starting language");
				
				string[] opts = new string[_AllLanguagesHolderSize.intValue];
				for(int i = 0;i<opts.Length;i++){
					opts[i] = (_AllLanguages.GetArrayElementAtIndex(i).objectReferenceValue as Sound).name;	
				}
				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal();
				_startingLanguageIndex.intValue = EditorGUILayout.Popup(_startingLanguageIndex.intValue, opts);
				if(opts.Length > 0)
					_startingLanguage.stringValue = opts[_startingLanguageIndex.intValue];
				GUILayout.FlexibleSpace();
				GUILayout.EndHorizontal();
			}
			
		}else{
			for(int i = 0;i< _AllLanguagesHolderSize.intValue;i++){
				ScriptableObject.DestroyImmediate(_soundManager.AllLanguages[i]);
			}
			_AllLanguages.arraySize = 0;	
		}
		
		GUILayout.Space(10);
		
		//###########################################		EDIT PART 		###########################################
		
		EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
		GUILayout.Label("Choose the channel to edit:");
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
		if(GUILayout.Button("Bkg",EditorStyles.toolbarButton)){
			showBkg = true;
			showEfx = false;
			showVoice = false;
		}
		if(GUILayout.Button("Efx",EditorStyles.toolbarButton)){
			showBkg = false;
			showEfx = true;
			showVoice = false;
		}
		if(GUILayout.Button("Voice", EditorStyles.toolbarButton)){
			showBkg = false;
			showEfx = false;
			showVoice = true;	
		}
		EditorGUILayout.EndHorizontal();
		
		EditorGUILayout.BeginVertical(EditorStyles.objectFieldThumb);
		if(showBkg){
			GUILayout.Label("Editing Background Channel!");
			GUILayout.BeginHorizontal();
			_bkgVolume.floatValue = EditorGUILayout.Slider("Volume",_bkgVolume.floatValue, 0, 1);
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
			_playAtStart.boolValue = EditorGUILayout.Toggle("Play at start", _playAtStart.boolValue);
			if(_playAtStart.boolValue){
				GUILayout.Label("Choose the Background AudioClip");
				holderBkgOpt = new List<string>();
				for(int i = 0;i< _AllSoundsHolder.arraySize;i++){
					var holder = (Sound)_AllSoundsHolder.GetArrayElementAtIndex(i).objectReferenceValue as Sound;
					if(holder.track == soundTrack.BackgroundSound){
						holderBkgOpt.Add(holder.name);	
					}
				}
				if(holderBkgOpt.Count > 0){
					bkgOpt = new string[holderBkgOpt.Count];
					for(int i = 0 ; i < bkgOpt.Length; i++){
						bkgOpt[i] = holderBkgOpt[i];	
					}
					GUILayout.BeginHorizontal();
					choiseBkgOpt = EditorGUILayout.Popup(choiseBkgOpt, bkgOpt);
					GUILayout.FlexibleSpace();
					GUILayout.EndHorizontal();
					_nameBkgStart.stringValue = bkgOpt[choiseBkgOpt];
				}else{
					EditorGUILayout.HelpBox("You don't have any Background AudioClip.",MessageType.Warning);	
				}
			}
			
			//quantity
			qnt = 0;
			for(int i = 0;i< _AllSoundsHolder.arraySize;i++){
					var holder = (Sound)_AllSoundsHolder.GetArrayElementAtIndex(i).objectReferenceValue as Sound;
					if(holder.track == soundTrack.BackgroundSound){
						qnt++;
					}
				}
			GUILayout.Label("You have: "+qnt+" Background Musics");
		}
		if(showEfx){
			GUILayout.Label("Editing Effects Channel!");
			GUILayout.BeginHorizontal();
			_efxVolume.floatValue = EditorGUILayout.Slider("Volume",_efxVolume.floatValue, 0, 1);
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
			
			//quantity
			qnt = 0;
			for(int i = 0;i< _AllSoundsHolder.arraySize;i++){
					var holder = (Sound)_AllSoundsHolder.GetArrayElementAtIndex(i).objectReferenceValue as Sound;
					if(holder.track == soundTrack.EffectSound){
						qnt++;
					}
				}
			GUILayout.Label("You have: "+qnt+" Sound Effects");
		}
		if(showVoice){
			GUILayout.Label("Editing Voice Channel!");
			GUILayout.BeginHorizontal();
			_voiceVolume.floatValue = EditorGUILayout.Slider("Volume",_voiceVolume.floatValue, 0, 1);
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
			
			//quantity
			qnt = 0;
			for(int i = 0;i< _AllSoundsHolder.arraySize;i++){
					var holder = (Sound)_AllSoundsHolder.GetArrayElementAtIndex(i).objectReferenceValue as Sound;
					if(holder.track == soundTrack.VoiceSound){
						qnt++;
					}
				}
			GUILayout.Label("You have: "+qnt+" Sound Effects");
		}
		EditorGUILayout.EndVertical();
		
		GUILayout.Space(10);
		
		
		//###########################################		SOUND PART 		###########################################
		
		EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
		GUILayout.Label("Sound Configuration Panel");
		EditorGUILayout.EndHorizontal();
		
		for(int i=0;i<_AllSoundsHolder.arraySize;i++){
			var holder = (Sound)_AllSoundsHolder.GetArrayElementAtIndex(i).objectReferenceValue as Sound;
			GUILayout.Space(5);
			GUILayout.BeginVertical(EditorStyles.objectFieldThumb);
			 
			GUILayout.BeginHorizontal(EditorStyles.toolbar);;
				string toWrite;
				if(holder.name != ""){
					toWrite = "Sound: "+holder.name;
				}else{
					toWrite = "Sound number: "+(i+1);
				}
			holder.showCarac = EditorGUILayout.Foldout(holder.showCarac, toWrite);
				GUILayout.FlexibleSpace();
			if(GUILayout.Button("Preview",EditorStyles.toolbarButton)){
				if(holder.clip != null)
					PlayClip(holder.clip as AudioClip);
			}
			if(GUILayout.Button("Stop",EditorStyles.toolbarButton)){
				if(holder.clip != null)
					StopClip(holder.clip as AudioClip);
			}
			
			GUILayout.Label("Track: ");
			GUILayout.Label(holder.track.ToString(), EditorStyles.toolbarTextField);
			if(GUILayout.Button("Delete",EditorStyles.toolbarButton)){
				RemoveSound(i);
				return;
			}
			GUILayout.EndHorizontal();
			
			if(holder.showCarac){
				GUILayout.BeginHorizontal();
				holder.clip = EditorGUILayout.ObjectField("Audio Clip",holder.clip, typeof(AudioClip), true) as AudioClip;
				GUILayout.EndHorizontal();
				
				GUILayout.BeginHorizontal();
				holder.name = EditorGUILayout.TextField("Name", holder.name);
				GUILayout.EndHorizontal();
				
				GUILayout.BeginHorizontal();
				holder.volume = EditorGUILayout.Slider("Volume", holder.volume, 0, 1);
				holder.lastVolumeSetted = holder.volume;
				GUILayout.EndHorizontal();
				
				GUILayout.BeginHorizontal();
				holder.loop = EditorGUILayout.Toggle("Loop",holder.loop);
				GUILayout.EndHorizontal();
				
				GUILayout.BeginHorizontal();
				holder.fadeIn = EditorGUILayout.Toggle("FadeIn",holder.fadeIn);
				if(holder.fadeIn){
					holder.timeToFadeIn = EditorGUILayout.FloatField("Time to fade in: ", holder.timeToFadeIn);
					if(holder.timeToFadeIn < 0.1f) holder.timeToFadeIn = 0.1f;
					GUILayout.FlexibleSpace();
				}
				GUILayout.EndHorizontal();
				
				GUILayout.BeginHorizontal();
				holder.fadeOut = EditorGUILayout.Toggle("FadeOut", holder.fadeOut);
				if(holder.fadeOut){
					holder.timeToFadeOut = EditorGUILayout.FloatField("Time to fade out", holder.timeToFadeOut);
					if(holder.timeToFadeOut < 0.1f) holder.timeToFadeOut = 0.1f;
					GUILayout.FlexibleSpace();
				}
				GUILayout.EndHorizontal();
				
				GUILayout.BeginHorizontal();
				holder._3D = EditorGUILayout.Toggle("3D Sound", holder._3D);
				GUILayout.EndHorizontal();
				if(holder._3D){
					EditorGUILayout.HelpBox("Where in the world the sound should spawn", MessageType.Info);
					EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
					if(GUILayout.Button("Vector3",EditorStyles.toolbarButton)){
						showV3 = true;
						showTrans = false;
					}
					if(GUILayout.Button("GameObject",EditorStyles.toolbarButton)){
						showV3 = false;
						showTrans = true;
					}
					if(holder.tTarget != null){
						showV3 = false;showTrans = true;	
					}
					EditorGUILayout.EndHorizontal();
					
					if(showV3){
						holder.v3Target = EditorGUILayout.Vector3Field("Vector3", holder.v3Target);
						holder.tTarget = null;
						holder.followObject = false;
					}else if(showTrans){
						holder.v3Target = new Vector3(0,0,0);
						holder.tTarget = EditorGUILayout.ObjectField("GameObject",holder.tTarget, typeof(GameObject), true) as GameObject;
						EditorGUILayout.HelpBox("If true, the sound will be this GameObject's children, else, it will be in the world", MessageType.Info);
						GUILayout.BeginHorizontal();
						holder.followObject = EditorGUILayout.Toggle("Follow GameObject", holder.followObject);
						GUILayout.EndHorizontal();
					}
				}
				
				GUILayout.BeginHorizontal();
				GUILayout.Label("Track");
				holder.track = (soundTrack)EditorGUILayout.EnumPopup(holder.track);
				GUILayout.EndHorizontal();
				
				GUILayout.BeginHorizontal();
				if(_soundManager.AllLanguages.Count > 0){
					string[] lenguagesNames = new string[_soundManager.AllLanguages.Count+1];
					for(int k = 0;k< lenguagesNames.Length;k++){
						if(k < lenguagesNames.Length-1)
							lenguagesNames[k] = _soundManager.AllLanguages[k].name;	
						else
							lenguagesNames[k] = "All";
					}
					
					holder.allLenguages = lenguagesNames;
					
					GUILayout.BeginHorizontal();
					GUILayout.Label("SoundClip Language");
					holder.languageIndexHolder = EditorGUILayout.Popup(holder.languageIndexHolder, holder.allLenguages);
					GUILayout.EndHorizontal();
					
					holder.language = holder.allLenguages[holder.languageIndexHolder];
				}
				GUILayout.EndHorizontal();
				
				holder.trigger = GUILayout.Toggle(holder.trigger, "Trigger Event");
				
				GUILayout.BeginVertical(EditorStyles.objectFieldThumb);
				if(holder.trigger){
					holder.timeToTrigger = EditorGUILayout.FloatField("Time (sec): ", holder.timeToTrigger);
					if(holder.timeToTrigger  < 0) holder.timeToTrigger = -1;
					EditorGUILayout.HelpBox("If you leave the Target box blank, the gameObject will be targeted", MessageType.Info);
					holder.targetForTrigger = EditorGUILayout.ObjectField("Target",holder.targetForTrigger, typeof(GameObject),true) as GameObject;
				}else{
					holder.timeToTrigger = -50;	
					holder.targetForTrigger = null;
				}
				GUILayout.EndVertical();
			}
			GUILayout.EndVertical();
			
			for(int m=0;m<_AllSoundsHolder.arraySize;m++){
				var toCheck = (Sound)_AllSoundsHolder.GetArrayElementAtIndex(m).objectReferenceValue as Sound;
				if(toCheck.name == holder.name){
					toCheck.track = holder.track;
					toCheck.trigger = holder.trigger;
					toCheck.timeToTrigger = holder.timeToTrigger;
					toCheck.targetForTrigger = holder.targetForTrigger;
				}
			}
		}
		
		
		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		if(GUILayout.Button("Add Sound",new GUILayoutOption[] {GUILayout.Width(200),GUILayout.Height(25), GUILayout.ExpandWidth(false)})){
			Sound myNewSound = ScriptableObject.CreateInstance<Sound>();
			myNewSound.name = "ToChange";
			myNewSound.volume = 1;
			myNewSound.lastVolumeSetted = 1.0f;
			if(_AllLanguages.arraySize > 0)
				myNewSound.language = (_AllLanguages.GetArrayElementAtIndex(0).objectReferenceValue as Sound).name;	
			_AllSoundsHolderSize.intValue++;
			_AllSoundsHolder.GetArrayElementAtIndex(_AllSoundsHolder.arraySize-1).objectReferenceValue = myNewSound;
		}
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		
		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
			GUILayout.Label("or");
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		
		object[] dropped = DropAreaGUI();
		if(dropped.Length != 0){
			for(int i = 0; i < dropped.Length; i++){
				Sound myNewSound = ScriptableObject.CreateInstance<Sound>();
				myNewSound.clip = dropped[i] as AudioClip;
				string str = dropped[i].ToString();
				string[] nStr = str.Split('/');
				str = nStr[nStr.Length-1];
				nStr = str.Split('.');
				myNewSound.name = nStr[0];
				myNewSound.volume = 1.0f;
				myNewSound.lastVolumeSetted = 1.0f;
				if(_AllLanguages.arraySize > 0)
					myNewSound.language = (_AllLanguages.GetArrayElementAtIndex(0).objectReferenceValue as Sound).name;	
				if(dropped[i].ToString().Contains("Effect") || dropped[i].ToString().Contains("Efx") || dropped[i].ToString().Contains("effect") || dropped[i].ToString().Contains("efx"))
					myNewSound.track = soundTrack.EffectSound;
				else
					myNewSound.track = soundTrack.BackgroundSound;
				
				for(int j = 0; j < _AllLanguagesHolderSize.intValue;j++){
					
					if(dropped[i].ToString().Contains((_AllLanguages.GetArrayElementAtIndex(j).objectReferenceValue as Sound).name)){
						myNewSound.language = (_AllLanguages.GetArrayElementAtIndex(j).objectReferenceValue as Sound).name;
						myNewSound.languageIndexHolder = j;
					}
				}
				
				myNewSound.showCarac = false;
				_AllSoundsHolderSize.intValue++;
				_AllSoundsHolder.GetArrayElementAtIndex(_AllSoundsHolder.arraySize-1).objectReferenceValue = myNewSound;
			}
		}
		
		_sManager.ApplyModifiedProperties();
	}
	
	public object[] DropAreaGUI ()
    {
		object[] toReturn = new object[0];
		
        Event evt = Event.current;
		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
        Rect drop_area = GUILayoutUtility.GetRect (210.0f, 50.0f, GUILayout.ExpandWidth(true));
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		GUI.Box(drop_area, "Drag multiple AudioClips here.");
     
        switch (evt.type) {
        case EventType.DragUpdated:
        case EventType.DragPerform:
            if (drop_area.Contains (evt.mousePosition)){
	            DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
	            if (evt.type == EventType.DragPerform) {
					bool canGo = false;
					int quantityToGo = 0;
					for(int i = 0;i< DragAndDrop.objectReferences.Length;i++){
						if(DragAndDrop.objectReferences[i].GetType() == typeof(AudioClip)){
							canGo = true;
							quantityToGo ++;
						}
					}
	             	if(canGo){
						DragAndDrop.AcceptDrag ();
						toReturn = new object[quantityToGo];
						int counter = 0;
						for(int i=0;i<DragAndDrop.objectReferences.Length;i++)
						{
							if(DragAndDrop.objectReferences[i].GetType() == typeof(AudioClip)){
								DragAndDrop.objectReferences[i].name = DragAndDrop.paths[i];
								toReturn[counter] = DragAndDrop.objectReferences[i];
								counter++;
							}
						}
					}
	            }
			}
            break;
        }
		return toReturn;
    }
}