using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace TagFrenzy{
    [System.Serializable]
    public class TagFrenzyList: MonoBehaviour {    
        public List<EditorTag> SelectedEditorTags;

        public TagFrenzyList()
        {
            InitializeList();
        }

        /// <summary>
        /// Initialaze all the available tabs
        /// </summary>
        public void Awake()
        {
            InitializeList();
            CleanupDeletedTags();
        }


        /// <summary>
        /// If any old tag names are still in this object's SelectedEditorTags list then remove them.  This can happen
        /// after a tag has been deleted
        /// </summary>
        public void CleanupDeletedTags()
        {
            List<EditorTag> toRemove = new List<EditorTag>();

            //Find all tags in the SelectedEditorTags list with no match in our global tag list
            for (int i = 0; i < SelectedEditorTags.Count; i++)
            {
                if (!(MultiTagManager.TagIdMatch(SelectedEditorTags[i])))
                {
                    toRemove.Add(SelectedEditorTags[i]);
                }
            }

            for (int i = 0; i < toRemove.Count; i++)
            {
                SelectedEditorTags.Remove(toRemove[i]);
            }
        }

        private void InitializeList()
        {
            if (SelectedEditorTags == null) { SelectedEditorTags = new List<EditorTag>(); }
        }


    }
}


