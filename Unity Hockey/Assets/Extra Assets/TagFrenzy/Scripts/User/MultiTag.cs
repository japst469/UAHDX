using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


namespace TagFrenzy
{
    public static class MultiTag
    {



        /// <summary>
        /// Find game objects that match the given tags
        /// </summary>
        /// <param name="tag1">The tag name</param>
        /// <param name="tagMatchType">Match tags by "Or" (any of the given tags can be selected on the object) or by "And" (all of the given tags must be selected on the object)</param>
        /// <returns>A list of matching game objects</returns>
        public static List<GameObject> FindGameObjectsWithTags(string tag1, TagMatch tagMatchType = TagMatch.Or)
        {
            List<string> temp = new List<string>() { tag1 };
            return FindGameObjectsWithTags(temp, tagMatchType);
        }

        /// <summary>
        /// Find game objects that match the given tags
        /// </summary>
        /// <param name="tag1">The tag name</param>
        /// <param name="tag2">The tag name</param>
        /// <param name="tagMatchType">Match tags by "Or" (any of the given tags can be selected on the object) or by "And" (all of the given tags must be selected on the object)</param>
        /// <returns>A list of matching game objects</returns>
        public static List<GameObject> FindGameObjectsWithTags(string tag1, string tag2, TagMatch tagMatchType = TagMatch.Or)
        {
            List<string> temp = new List<string>() { tag1, tag2 };
            return FindGameObjectsWithTags(temp, tagMatchType);
        }

        /// <summary>
        /// Find game objects that match the given tags
        /// </summary>
        /// <param name="tag1">The tag name</param>
        /// <param name="tag2">The tag name</param>
        /// <param name="tag3">The tag name</param>
        /// <param name="tagMatchType">Match tags by "Or" (any of the given tags can be selected on the object) or by "And" (all of the given tags must be selected on the object)</param>
        /// <returns>A list of matching game objects</returns>
        public static List<GameObject> FindGameObjectsWithTags(string tag1, string tag2, string tag3, TagMatch tagMatchType = TagMatch.Or)
        {
            List<string> temp = new List<string>() { tag1, tag2, tag3 };
            return FindGameObjectsWithTags(temp, tagMatchType);
        }

        /// <summary>
        /// Find game objects that match the given tags
        /// </summary>
        /// <param name="tag1">The tag name</param>
        /// <param name="tagMatchType">Match tags by "Or" (any of the given tags can be selected on the object) or by "And" (all of the given tags must be selected on the object)</param>
        /// <returns>A list of matching game objects</returns>
        public static List<GameObject> FindGameObjectsWithTags(Tags tag1, TagMatch tagMatchType = TagMatch.Or)
        {
            List<string> temp = new List<string>() { tag1.ToString() };
            return FindGameObjectsWithTags(temp, tagMatchType);
        }

        /// <summary>
        /// Find game objects that match the given tags
        /// </summary>
        /// <param name="tag1">The tag name</param>
        /// <param name="tag2">The tag name</param>  
        /// <param name="tagMatchType">Match tags by "Or" (any of the given tags can be selected on the object) or by "And" (all of the given tags must be selected on the object)</param>
        /// <returns>A list of matching game objects</returns>
        public static List<GameObject> FindGameObjectsWithTags(Tags tag1, Tags tag2, TagMatch tagMatchType = TagMatch.Or)
        {
            List<string> temp = new List<string>() { tag1.ToString(), tag2.ToString() };
            return FindGameObjectsWithTags(temp, tagMatchType);
        }

        /// <summary>
        /// Find game objects that match the given tags
        /// </summary>
        /// <param name="tag1">The tag name</param>
        /// <param name="tag2">The tag name</param>
        /// <param name="tag3">The tag name</param>
        /// <param name="tagMatchType">Match tags by "Or" (any of the given tags can be selected on the object) or by "And" (all of the given tags must be selected on the object)</param>
               /// <returns>A list of matching game objects</returns>
        public static List<GameObject> FindGameObjectsWithTags(Tags tag1, Tags tag2, Tags tag3, TagMatch tagMatchType = TagMatch.Or)
        {
            List<string> temp = new List<string>() { tag1.ToString(), tag2.ToString(), tag3.ToString() };
            return FindGameObjectsWithTags(temp, tagMatchType);
        }


        /// <summary>
        /// Find game objects that match the given tags
        /// </summary>
        /// <param name="tags">A string list of tags to match</param>
        /// <param name="tagMatchType">Match tags by "Or" (any of the given tags can be selected on the object) or by "And" (all of the given tags must be selected on the object)</param>
        /// <returns>A list of matching game objects</returns>
        public static List<GameObject> FindGameObjectsWithTags(List<string> tags, TagMatch tagMatchType)
        {
       
            TagFrenzyList[] tagged = GameObject.FindObjectsOfType<TagFrenzyList>();
         


            List<GameObject> matches = new List<GameObject>();

            //Look through all objects with a tagger attached
            for (int i = 0; i < tagged.Length; i++)
            {
                TagFrenzyList obj = tagged[i];

                if (tagMatchType == TagMatch.Or)
                {
                    //Look through all the entered tags, and see any of them match
                    for (int j = 0; j < tags.Count; j++)
                    {
                        if (obj.SelectedEditorTags.Where(et=>et.Tag.ToLower().Trim() == tags[j].ToLower().Trim()).Any())
                        {
                            //If so, return it
                            matches.Add(obj.gameObject);
                            break;
                        }
                    }

                }
                else if (tagMatchType == TagMatch.And)
                {
                    //Look through all the entered tags, and see if all of them match
                    bool addMatch = true;
                    for (int j = 0; j < tags.Count; j++)
                    {
						if (!obj.SelectedEditorTags.Where(et=>et.Tag.ToLower().Trim() == tags[j].ToLower().Trim()).Any())
                        {
                            addMatch = false;
                        }
                    }
                    if (addMatch)
                    {
                        matches.Add(obj.gameObject);
                    }
                }
                else if (tagMatchType == TagMatch.Not)
                {
                    bool addMatch = true;
                    for (int j = 0; j < tags.Count; j++)
                    {
                        if (obj.SelectedEditorTags.Where(et => et.Tag.ToLower().Trim() == tags[j].ToLower().Trim()).Any())
                        {
                            addMatch = false;
                        }
                    }
                    if (addMatch)
                    {
                        matches.Add(obj.gameObject);
                    }
                }
                else if (tagMatchType == TagMatch.Exact)
                {
                    bool addMatch = true;
                    int matchCount = 0;
                    for (int j = 0; j < tags.Count; j++)
                    {
                        //Make sure all the tags match
                        if (!obj.SelectedEditorTags.Where(et => et.Tag.ToLower().Trim() == tags[j].ToLower().Trim()).Any())
                        {
                            addMatch = false;
                        }
                        else
                        {
                            matchCount += 1;
                        }
                    }

                    //And make sure there are only matching tags in the result
                    if (matchCount != obj.SelectedEditorTags.Count())
                    {
                        addMatch = false;
                    }

                    if (addMatch)
                    {
                        matches.Add(obj.gameObject);
                    }
                }



           
            }
            return matches;
        }

    }

}




