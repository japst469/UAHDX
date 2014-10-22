using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


namespace TagFrenzy
{
    public static class GameObjectExtensions
    {
        /// <summary>
        /// Return a list of all tags associated with this gameobject
        /// </summary>
        /// <param name="go">The game object</param>
        /// <returns>A list of tags</returns>
        public static List<string> tags(this GameObject go)
        {
            var tagger = go.GetComponent<TagFrenzyList>();
            if (tagger == null)
            {
                return new List<string>();
            }
            else
            {
                return tagger.SelectedEditorTags.Select(x => x.Tag).ToList();
            }
        }

        /// <summary>
        /// Return a enum list of all tags associated with this gameobject
        /// </summary>
        /// <param name="go">The game object</param>
        /// <returns>A list of tags</returns>
        public static List<Tags> tagsEnum(this GameObject go)
        {
            var tagger = go.GetComponent<TagFrenzyList>();
            if (tagger == null)
            {
                return new List<Tags>();
            }
            else
            {
                List<Tags> enumTags = new List<Tags>();
                for (int i = 0; i < tagger.SelectedEditorTags.Count; i++)
                {
                    string s = tagger.SelectedEditorTags[i].Tag;              
	                Tags currentTag = (Tags)Enum.Parse(typeof(Tags), s, true);
	                enumTags.Add(currentTag);
                }
                return enumTags;
            }
        }

        /// <summary>
        /// Add a tag to this gameobject if it does not already exist.  Also add the tagger component to the gameobject if it doesn't exist
        /// </summary>
        /// <param name="go">The game object</param>
        /// <returns>A list of tags</returns>
        public static void AddTag(this GameObject go, string tag)
        {
            var tagger = go.GetComponent<TagFrenzyList>();
            if (tagger == null)
            {
                tagger = go.AddComponent<TagFrenzyList>();
            }

            //Make sure no deleted tags are hanging around before we add new ones
            tagger.CleanupDeletedTags();

            if (!tagger.SelectedEditorTags.Where(t=>t.Tag == tag).Any())
            {
                EditorTag et = MultiTagManager.EditorTags.Where(t => t.Tag == tag).FirstOrDefault();
                tagger.SelectedEditorTags.Add(et);
            }
        }

        /// <summary>
        /// Add a tag to this gameobject if it does not already exist.  Also add the tagger component to the gameobject if it doesn't exist
        /// </summary>
        /// <param name="go">The game object</param>
        /// <returns>A list of tags</returns>
        public static void AddTag(this GameObject go, Tags tag)
        {
            var tagger = go.GetComponent<TagFrenzyList>();
            if (tagger == null)
            {
                tagger = go.AddComponent<TagFrenzyList>();
            }

            //Make sure no deleted tags are hanging around before we add new ones
            tagger.CleanupDeletedTags();

            if (!tagger.SelectedEditorTags.Where(t => t.Tag == tag.ToString()).Any())
            {
                EditorTag et = MultiTagManager.EditorTags.Where(t => t.Tag == tag.ToString()).FirstOrDefault();
                tagger.SelectedEditorTags.Add(et);
            }
        }

        public static void RemoveTag(this GameObject go, string tag)
        {
            var tagger = go.GetComponent<TagFrenzyList>();
            if (tagger == null)
            {
                tagger = go.AddComponent<TagFrenzyList>();
            }

            //Make sure no deleted tags are hanging around before we remove old ones
            tagger.CleanupDeletedTags();

            if (!tagger.SelectedEditorTags.Where(t => t.Tag == tag).Any())
            {
                EditorTag et = MultiTagManager.EditorTags.Where(t => t.Tag == tag).FirstOrDefault();
                tagger.SelectedEditorTags.Remove(et);
            }
        }

        /// <summary>
        /// Add a tag to this gameobject if it does not already exist.  Also add the tagger component to the gameobject if it doesn't exist
        /// </summary>
        /// <param name="go">The game object</param>
        /// <returns>A list of tags</returns>
        public static void RemoveTag(this GameObject go, Tags tag)
        {
            var tagger = go.GetComponent<TagFrenzyList>();
            if (tagger == null)
            {
                tagger = go.AddComponent<TagFrenzyList>();
            }

            //Make sure no deleted tags are hanging around before we add new ones
            tagger.CleanupDeletedTags();

            if (!tagger.SelectedEditorTags.Where(t => t.Tag == tag.ToString()).Any())
            {
                EditorTag et = MultiTagManager.EditorTags.Where(t => t.Tag == tag.ToString()).FirstOrDefault();
                tagger.SelectedEditorTags.Remove(et);
            }
        }

    }

}

