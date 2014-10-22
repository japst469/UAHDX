using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace TagFrenzy
{
    [CustomEditor(typeof(TagFrenzyList))]
    public class TagFrenzyListEditor : Editor
    {
        private SerializedObject tagger;
        private SerializedProperty selectedTags;
   
        void OnEnable()
        {
            tagger = new SerializedObject(target);
			selectedTags = tagger.FindProperty("SelectedEditorTags");
			
		}

        public override void OnInspectorGUI()
        {

            tagger.Update();

       
            for (int i = 0; i < MultiTagManager.EditorTags.Count; i++)
            {
                EditorTag tagName = MultiTagManager.EditorTags[i];
                bool toggle = ContainsTag(tagName);
                toggle = EditorGUILayout.BeginToggleGroup(new GUIContent(tagName.Tag), toggle);
                if (toggle)
                {
                    AddTag(tagName);
                }
                else
                {
                    RemoveTag(tagName);
                }
                EditorGUILayout.EndToggleGroup();
            }

            tagger.ApplyModifiedProperties();
        }

        private void AddTag(EditorTag tagName)
        {
            if (!ContainsTag(tagName))
            {
				selectedTags.arraySize +=1;
				SerializedProperty id = selectedTags.GetArrayElementAtIndex(selectedTags.arraySize - 1).FindPropertyRelative("Id");
                SerializedProperty tag = selectedTags.GetArrayElementAtIndex(selectedTags.arraySize - 1).FindPropertyRelative("Tag");

                id.stringValue = tagName.Id;
                tag.stringValue = tagName.Tag;
            }
        }

        private void RemoveTag(EditorTag tagName)
        {
            for (int i = 0; i < selectedTags.arraySize; i++)
            {
                if (tagName.Id.Trim() == selectedTags.GetArrayElementAtIndex(i).FindPropertyRelative("Id").stringValue.Trim())
                {
                    selectedTags.DeleteArrayElementAtIndex(i);
                    return;
                }
            }
        }

        private bool ContainsTag(EditorTag tagName)
        {
            for (int i = 0; i < selectedTags.arraySize; i++)
            {
                if (tagName.Id.Trim() == selectedTags.GetArrayElementAtIndex(i).FindPropertyRelative("Id").stringValue.Trim())
                {
                    return true;
                }
            }

            return false;
        }

    }

}
