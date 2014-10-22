using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEditor;

namespace TagFrenzy
{

    public class MultiTagWindow : EditorWindow
    {
        private List<EditorTag> newTags;
        private Vector2 scrollPosition;

        // This creates a new Menu item where we can access our tool from.
        [MenuItem("Window/Tag Frenzy/Add or Edit Tags")]
        static void OpenWindow()
        {
            MultiTagWindow window = (MultiTagWindow)EditorWindow.GetWindow(typeof(MultiTagWindow));
            window.title = "Tag Frenzy";
            window.InitializeTags();
        }

        [MenuItem("Window/Tag Frenzy/Add Tags to all Scene Objects")]
        static void Setup()
        {
            MultiTagWindow.SetupForTagger();
        }

        public static void SetupForTagger()
        {
            //Setup the current tags
            List<EditorTag> tagsToAdd = new List<EditorTag>();
            for (int i = 0; i < MultiTagManager.EditorTags.Count(); i++)
            {
                tagsToAdd.Add(MultiTagManager.EditorTags[i]);
            }

            //Loop trough all game objects in the scene
            object[] obj = GameObject.FindObjectsOfType(typeof(GameObject));
            foreach (object o in obj)
            {
                //Make sure they have a tagger component attached
                GameObject g = (GameObject)o;
                TagFrenzyList comp = (TagFrenzyList)g.GetComponent(typeof(TagFrenzyList));
                if (comp == null)
                {
                    comp = g.AddComponent<TagFrenzyList>();
                }

                //Find any tags on each current object
                string tag = g.tag;

                if (tag != null && tag.ToLower() != "untagged")
                {
                    //Does the tag exist in our global list of tags? If not, add it.
                    if (!MultiTagManager.TagNameMatch(tag))
                    {
                        EditorTag et = new EditorTag();
                        et.Id = "0";
                        et.Tag = tag;
                        tagsToAdd.Add(et);    
                        MultiTagManager.Update(tagsToAdd);                  
                    }

                    //Add it to the local tagger script
                    g.AddTag(g.tag);
                }

            }

            AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
        }


        void InitializeTags()
        {
            newTags = new List<EditorTag>();
            for (int i = 0; i < MultiTagManager.EditorTags.Count(); i++)
            {
                newTags.Add(MultiTagManager.EditorTags[i]);
            }
        }

        void AddTagCheck()
        {
            if (Event.current.type == EventType.Repaint)
            {
                //Auto add new values 
                if (newTags.Where(n => string.IsNullOrEmpty(n.Tag)).Count() < 1)
                {
                    newTags.Add(new EditorTag() { Id = "0", Tag = "" });
                }
            }
        }

        void OnGUI()
        {

            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

            for (int i = 0; i < newTags.Count(); i++)
            {
                EditorTag tag = newTags[i];
                EditorGUILayout.BeginHorizontal();

                newTags[i].Tag = MultiTagManager.CleanName(EditorGUILayout.TextField(new GUIContent("Tag"), tag.Tag));

                //Don't allow users to add tags that already exist.  Clear if needed
                if (newTags.Where(t=>t.Tag.ToLower().Trim() == newTags[i].Tag.ToLower().Trim()).Count() > 1){
                    newTags[i].Tag = "";
                }


                if (GUILayout.Button(new GUIContent("x", "Delete Tag")))
                {
                    newTags.Remove(tag);
                    AddTagCheck();
                }

                EditorGUILayout.EndHorizontal();
            }
       
             AddTagCheck();

            EditorGUILayout.EndScrollView();


        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button(new GUIContent("Save")))
        {
                MultiTagManager.Update(newTags);
                AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
                this.Close();
            }
       
        if (GUILayout.Button(new GUIContent("Cancel")))
        {            
            this.Close();
        }
        EditorGUILayout.EndHorizontal();

        }


    }
}


